Public Class Vendor_Closing_Stock_Entry
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "PRINV-"
    Private dgv_ActiveCtrl_Name As String

    Private NoCalc_Status As Boolean = False
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Private WithEvents dgtxt_CountDetails As New DataGridViewTextBoxEditingControl
    Private WithEvents dgtxt_EndsCountDetails As New DataGridViewTextBoxEditingControl
    Private WithEvents dgtxt_clothDetails As New DataGridViewTextBoxEditingControl



    Private dgvDet_CboBx_ColNos_Arr As Integer() = {1}


    Private prn_HdDt As New DataTable
    Private prn_Dt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
    Private prn_DetMxIndx As Integer
    Private prn_NoofBmDets As Integer
    Private prn_DetSNo As Integer
    Private NoFo_STS As Integer = 0
    Private prn_HdIndx As Integer
    Private prn_HdMxIndx As Integer
    Private prn_Count As Integer
    Private prn_HdAr(100, 10) As String
    Private prn_DetAr(100, 50, 10) As String
    Private prn_InpOpts As String = ""
    Private prn_OriDupTri As String = ""
    Public vmskOldText As String = ""
    Public vmskSelStrt As Integer = -1

    Private Sub clear()
        NoCalc_Status = True
        New_Entry = False
        Insert_Entry = False

        pnl_Back.Enabled = True
        pnl_Filter.Visible = False

        lbl_RefNo.Text = ""
        lbl_RefNo.ForeColor = Color.Black
        'txt_Prefix_InvNo.Text = ""
        'chk_InvNo.Checked = False

        vmskOldText = ""
        vmskSelStrt = -1

        dtp_date.Text = ""

        cbo_WeaverName.Text = ""

        cbo_WeaverName.Enabled = True
        cbo_WeaverName.BackColor = Color.White



        If Filter_Status = False Then
            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1

            dgv_Filter_Details.Rows.Clear()
        End If

        Grid_Cell_DeSelect()

        NoCalc_Status = False

        dgv_Count_details.Rows.Clear()
        dgv_CountDetails_Total.Rows.Clear()
        dgv_CountDetails_Total.Rows.Add()

        dgv_EndsCount_Details.Rows.Clear()
        dgv_EndsCount_Details_Total.Rows.Clear()
        dgv_EndsCount_Details_Total.Rows.Add()

        dgv_Cloth_Details.Rows.Clear()
        dgv_Cloth_Details_Total.Rows.Clear()
        dgv_Cloth_Details_Total.Rows.Add()

    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox
        Dim mskdtxbx As MaskedTextBox
        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Prec_ActCtrl Is MaskedTextBox Then
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
            mskdtxbx = Me.ActiveControl
            mskdtxbx.SelectAll()
        End If

        If Me.ActiveControl.Name <> Cbo_Grid_CountName.Name Then
            Cbo_Grid_CountName.Visible = False
            Cbo_Grid_CountName.Tag = -100
        End If

        If Me.ActiveControl.Name <> Cbo_Grid_EndsCountName.Name Then
            Cbo_Grid_EndsCountName.Visible = False
            Cbo_Grid_EndsCountName.Tag = -100
        End If

        If Me.ActiveControl.Name <> cbo_Grid_ClothName.Name Then
            cbo_Grid_ClothName.Visible = False
            cbo_Grid_ClothName.Tag = -100
        End If

        Grid_Cell_DeSelect()

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

    Private Sub ControlLostFocus1(ByVal sender As Object, ByVal e As System.EventArgs)

        On Error Resume Next

        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Then
                Prec_ActCtrl.BackColor = Color.LightSkyBlue
                Prec_ActCtrl.ForeColor = Color.Blue
            End If
        End If

    End Sub

    Private Sub Grid_DeSelect()
        On Error Resume Next
        If Not IsNothing(dgv_Count_details.CurrentCell) Then dgv_Count_details.CurrentCell.Selected = False

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
        If Not IsNothing(dgv_Count_details.CurrentCell) Then dgv_Count_details.CurrentCell.Selected = False
        If Not IsNothing(dgv_CountDetails_Total.CurrentCell) Then dgv_CountDetails_Total.CurrentCell.Selected = False
        If Not IsNothing(dgv_EndsCount_Details.CurrentCell) Then dgv_EndsCount_Details.CurrentCell.Selected = False
        If Not IsNothing(dgv_EndsCount_Details_Total.CurrentCell) Then dgv_EndsCount_Details_Total.CurrentCell.Selected = False
        If Not IsNothing(dgv_Cloth_Details.CurrentCell) Then dgv_Cloth_Details.CurrentCell.Selected = False
        If Not IsNothing(dgv_Cloth_Details_Total.CurrentCell) Then dgv_Cloth_Details_Total.CurrentCell.Selected = False
        If Not IsNothing(dgv_Filter_Details.CurrentCell) Then dgv_Filter_Details.CurrentCell.Selected = False
    End Sub

    Private Sub Invoice_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Try



            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_WeaverName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_WeaverName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If



            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(Cbo_Grid_CountName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                Cbo_Grid_CountName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If



            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(Cbo_Grid_EndsCountName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                Cbo_Grid_EndsCountName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If



            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_ClothName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_ClothName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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
            'MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        FrmLdSTS = False

    End Sub

    Private Sub Invoice_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub Invoice_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
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
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub Invoice_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load


        Me.Text = ""

        con.Open()



        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2
        pnl_Filter.BringToFront()






        AddHandler dtp_date.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_WeaverName.GotFocus, AddressOf ControlGotFocus


        AddHandler cbo_Grid_ClothName.GotFocus, AddressOf ControlGotFocus
        AddHandler Cbo_Grid_CountName.GotFocus, AddressOf ControlGotFocus
        AddHandler Cbo_Grid_EndsCountName.GotFocus, AddressOf ControlGotFocus


        AddHandler dtp_Filter_Fromdate.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_ToDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus


        AddHandler btn_save.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_close.GotFocus, AddressOf ControlGotFocus


        AddHandler dtp_date.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_date.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_WeaverName.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_Grid_ClothName.LostFocus, AddressOf ControlLostFocus
        AddHandler Cbo_Grid_CountName.LostFocus, AddressOf ControlLostFocus
        AddHandler Cbo_Grid_EndsCountName.LostFocus, AddressOf ControlLostFocus



        AddHandler dtp_Filter_Fromdate.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_ToDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus

        AddHandler btn_save.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_close.LostFocus, AddressOf ControlLostFocus


        AddHandler msk_date.KeyDown, AddressOf TextBoxControlKeyDown


        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler msk_date.KeyPress, AddressOf TextBoxControlKeyPress

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

    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
        Dim dgv1 As New DataGridView


        If ActiveControl.Name = dgv_Count_details.Name Or ActiveControl.Name = dgv_EndsCount_Details.Name Or ActiveControl.Name = dgv_Cloth_Details.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            dgv1 = Nothing

            If ActiveControl.Name = dgv_Count_details.Name Then
                dgv1 = dgv_Count_details

            ElseIf ActiveControl.Name = dgv_EndsCount_Details.Name Then
                dgv1 = dgv_EndsCount_Details

            ElseIf ActiveControl.Name = dgv_Cloth_Details.Name Then
                dgv1 = dgv_Cloth_Details

            ElseIf dgv_Count_details.IsCurrentRowDirty = True Then
                dgv1 = dgv_Count_details

            ElseIf dgv_EndsCount_Details.IsCurrentRowDirty = True Then
                dgv1 = dgv_EndsCount_Details

            ElseIf dgv_Cloth_Details.IsCurrentRowDirty = True Then
                dgv1 = dgv_Cloth_Details

            ElseIf dgv_ActiveCtrl_Name = dgv_Count_details.Name Then
                dgv1 = dgv_Count_details

            ElseIf dgv_ActiveCtrl_Name = dgv_EndsCount_Details.Name Then
                dgv1 = dgv_EndsCount_Details

            ElseIf dgv_ActiveCtrl_Name = dgv_Cloth_Details.Name Then
                dgv1 = dgv_Cloth_Details

            End If

            If IsNothing(dgv1) = False Then

                With dgv1

                    If dgv1.Name = dgv_Count_details.Name Then



                        If keyData = Keys.Enter Or keyData = Keys.Down Or keyData = Keys.Up Then

                            Common_Procedures.DGVGrid_Cursor_Focus(dgv1, keyData, cbo_WeaverName, btn_save, dgvDet_CboBx_ColNos_Arr, dgtxt_CountDetails, dtp_date)

                            Return True

                        Else
                            Return MyBase.ProcessCmdKey(msg, keyData)

                        End If

                    ElseIf dgv1.Name = dgv_EndsCount_Details.Name Then

                        If keyData = Keys.Enter Or keyData = Keys.Down Or keyData = Keys.Up Then

                            Common_Procedures.DGVGrid_Cursor_Focus(dgv1, keyData, Nothing, dgv_Cloth_Details, dgvDet_CboBx_ColNos_Arr, dgtxt_EndsCountDetails, dtp_date)

                            Return True

                        Else
                            Return MyBase.ProcessCmdKey(msg, keyData)

                        End If

                    ElseIf dgv1.Name = dgv_Cloth_Details.Name Then


                        If keyData = Keys.Enter Or keyData = Keys.Down Or keyData = Keys.Up Then

                            Common_Procedures.DGVGrid_Cursor_Focus(dgv1, keyData, Nothing, btn_save, dgvDet_CboBx_ColNos_Arr, dgtxt_clothDetails, dtp_date)

                            Return True

                        Else
                            Return MyBase.ProcessCmdKey(msg, keyData)

                        End If
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

        Return MyBase.ProcessCmdKey(msg, keyData)

    End Function

    Private Sub move_record(ByVal no As String)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim da3 As New SqlClient.SqlDataAdapter
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim NewCode As String
        Dim n As Integer
        Dim SNo As Integer
        Dim LockSTS As Boolean = False
        If Val(no) = 0 Then Exit Sub

        clear()

        NoCalc_Status = True

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.* from Vendor_Closing_Stock_Value_Head a Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.vendor_Stock_Closing_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_RefNo.Text = dt1.Rows(0).Item("vendor_Stock_Closing_No").ToString


                msk_date.Text = dt1.Rows(0).Item("vendor_Stock_Closing_date").ToString
                cbo_WeaverName.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Weaver_idno").ToString))



                da2 = New SqlClient.SqlDataAdapter("Select a.* from vendor_Closing_Count_Stock_Details a Where a.vendor_Stock_Closing_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                With dgv_Count_details

                    .Rows.Clear()
                    SNo = 0

                    If dt2.Rows.Count > 0 Then

                        For i = 0 To dt2.Rows.Count - 1

                            n = .Rows.Add()

                            SNo = SNo + 1

                            .Rows(n).Cells(0).Value = Val(SNo)

                            .Rows(n).Cells(1).Value = Common_Procedures.Count_IdNoToName(con, Val(dt2.Rows(i).Item("Count_idno").ToString))

                            .Rows(n).Cells(2).Value = Format(Val(dt2.Rows(i).Item("Weight").ToString), "###########0.00")

                        Next i

                    End If

                End With
                NoCalc_Status = False
                Total_CountCalculation()
                NoCalc_Status = True
                da2 = New SqlClient.SqlDataAdapter("Select a.* from vendor_Closing_EndsCount_Stock_Details a Where a.vendor_Stock_Closing_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                dt3 = New DataTable
                da2.Fill(dt3)

                With dgv_EndsCount_Details

                    .Rows.Clear()
                    SNo = 0

                    If dt3.Rows.Count > 0 Then

                        For i = 0 To dt3.Rows.Count - 1

                            n = .Rows.Add()

                            SNo = SNo + 1

                            .Rows(n).Cells(0).Value = Val(SNo)


                            .Rows(n).Cells(1).Value = Common_Procedures.EndsCount_IdNoToName(con, Val(dt3.Rows(i).Item("EndsCount_idno").ToString))

                            .Rows(n).Cells(2).Value = Format(Val(dt3.Rows(i).Item("Meters").ToString), "###########0.00")


                        Next i

                    End If

                End With
                NoCalc_Status = False
                Total_EndsCountCalculation()
                '   NetAmount_Calculation()
                NoCalc_Status = True
                da2 = New SqlClient.SqlDataAdapter("Select a.* from vendor_Closing_Cloth_Stock_Details a Where a.vendor_Stock_Closing_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                dt4 = New DataTable
                da2.Fill(dt4)

                With dgv_Cloth_Details

                    .Rows.Clear()
                    SNo = 0

                    If dt4.Rows.Count > 0 Then

                        For i = 0 To dt4.Rows.Count - 1

                            n = .Rows.Add()

                            SNo = SNo + 1

                            'vendor_Closing_Cloth_Stock_Details
                            .Rows(n).Cells(0).Value = Val(SNo)
                            .Rows(n).Cells(1).Value = Common_Procedures.Cloth_IdNoToName(con, Val(dt4.Rows(i).Item("Cloth_idno").ToString))

                            .Rows(n).Cells(2).Value = Format(Val(dt4.Rows(i).Item("Meters").ToString), "###########0.00")


                        Next i

                    End If

                End With


                With dgv_CountDetails_Total
                    If .RowCount = 0 Then .Rows.Add()

                    .Rows(0).Cells(2).Value = Format(Val(dt1.Rows(0).Item("Total_CountWeight").ToString), "########0.00")

                End With
                With dgv_EndsCount_Details_Total
                    If .RowCount = 0 Then .Rows.Add()

                    .Rows(0).Cells(2).Value = Format(Val(dt1.Rows(0).Item("Total_EndsCount_meters").ToString), "########0.00")


                End With
                With dgv_Cloth_Details_Total
                    If .RowCount = 0 Then .Rows.Add()

                    .Rows(0).Cells(2).Value = Format(Val(dt1.Rows(0).Item("Total_Cloth_meters").ToString), "########0.00")


                End With

                NoCalc_Status = False
                Total_ClothCalculation()
                NoCalc_Status = True
            End If

            Grid_Cell_DeSelect()
            If LockSTS = True Then

                cbo_WeaverName.Enabled = False
                cbo_WeaverName.BackColor = Color.LightGray


            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally

            dt1.Dispose()
            da1.Dispose()

            dt2.Dispose()
            da2.Dispose()

            If msk_date.Enabled = True And msk_date.Visible = True Then msk_date.Focus()
            If dtp_date.Enabled = True And dtp_date.Visible = True Then dtp_date.Focus()


        End Try

        NoCalc_Status = False

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Fabric_Delivery_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Fabric_Delivery_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If


        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)



        trans = con.BeginTransaction

        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.Transaction = trans





            cmd.CommandText = "delete from vendor_Closing_EndsCount_Stock_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and vendor_Stock_Closing_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from vendor_Closing_Count_Stock_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and vendor_Stock_Closing_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from vendor_Closing_Cloth_Stock_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and vendor_Stock_Closing_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Vendor_Closing_Stock_Value_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and vendor_Stock_Closing_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()



            trans.Commit()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception
            trans.Rollback()
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally

            Dt1.Dispose()
            Da.Dispose()
            trans.Dispose()
            cmd.Dispose()
            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()
        End Try

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        If Filter_Status = False Then



            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1


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

            da = New SqlClient.SqlDataAdapter("select top 1 vendor_Stock_Closing_no from Vendor_Closing_Stock_Value_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and vendor_Stock_Closing_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, vendor_Stock_Closing_No", con)
            da.Fill(dt)

            movno = ""
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = Trim(dt.Rows(0)(0).ToString)
                End If
            End If

            dt.Clear()

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            dt.Dispose()
            da.Dispose()

        End Try

    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""
        Dim OrdByNo As Single = 0

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 vendor_Stock_Closing_No from Vendor_Closing_Stock_Value_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and vendor_Stock_Closing_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, vendor_Stock_Closing_no", con)
            da.Fill(dt)

            movno = ""
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = dt.Rows(0)(0).ToString
                End If
            End If

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            dt.Dispose()
            da.Dispose()

        End Try

    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""
        Dim OrdByNo As Single = 0

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 vendor_Stock_Closing_No from Vendor_Closing_Stock_Value_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and vendor_Stock_Closing_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, vendor_Stock_Closing_No desc", con)
            da.Fill(dt)

            movno = ""
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = dt.Rows(0)(0).ToString
                End If
            End If

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            dt.Dispose()
            da.Dispose()

        End Try

    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""

        Try
            da = New SqlClient.SqlDataAdapter("select top 1 vendor_Stock_Closing_No from Vendor_Closing_Stock_Value_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and vendor_Stock_Closing_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, vendor_Stock_Closing_No desc", con)
            da.Fill(dt)

            movno = ""
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = dt.Rows(0)(0).ToString
                End If
            End If

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            dt.Dispose()
            da.Dispose()

        End Try

    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable

        Try
            clear()

            New_Entry = True

            lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Vendor_Closing_Stock_Value_Head", "vendor_Stock_Closing_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)
            lbl_RefNo.ForeColor = Color.Red

            'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1175" Then
            '    If Trim(Dt1.Rows(0).Item("Invoice_Prefix_No").ToString) <> "" Then txt_Prefix_InvNo.Text = Dt1.Rows(0).Item("Invoice_Prefix_No").ToString
            'End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()
        End Try

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim InvCode As String

        Try

            inpno = InputBox("Enter Invoice No.", "FOR FINDING...")

            InvCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select vendor_Stock_Closing_No from Vendor_Closing_Stock_Value_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and vendor_Stock_Closing_Code = '" & Trim(InvCode) & "'", con)
            Da.Fill(Dt)

            movno = ""
            If Dt.Rows.Count > 0 Then
                If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                    movno = Trim(Dt.Rows(0)(0).ToString)
                End If
            End If

            Dt.Clear()

            If Val(movno) <> 0 Then
                move_record(movno)

            Else
                MessageBox.Show("Invoice No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            Dt.Dispose()
            Da.Dispose()

        End Try

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim InvCode As String

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Fabric_Receipt_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Fabric_Receipt_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub

        Try

            inpno = InputBox("Enter New Invoice No.", "FOR NEW INVOICE NO. INSERTION...")

            InvCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select vendor_Stock_Closing_No from Vendor_Closing_Stock_Value_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and vendor_Stock_Closing_Code = '" & Trim(InvCode) & "'", con)
            Da.Fill(Dt)

            movno = ""
            If Dt.Rows.Count > 0 Then
                If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                    movno = Trim(Dt.Rows(0)(0).ToString)
                End If
            End If

            Dt.Clear()

            If Val(movno) <> 0 Then
                move_record(movno)

            Else
                If Val(inpno) = 0 Then
                    MessageBox.Show("Invalid Invoice No.", "DOES NOT INSERT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_RefNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT ...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            Dt.Dispose()
            Da.Dispose()

        End Try

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim Led_ID As Integer = 0
        Dim OnAc_ID As Integer = 0
        Dim VaAc_ID As Integer = 0
        Dim PrnPl_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim Slno As Integer = 0
        Dim Sno1 As Integer = 0
        Dim EntID As String = ""
        Dim PBlNo As String = ""
        Dim Partcls As String = ""
        Dim vTot_EndsCntMtrs As Single
        Dim vTotAmt As Single
        Dim vtot_ClthMtr As Single
        Dim vTotScnChg As Single
        Dim vTotFmAChg As Single
        Dim vTotScnAChg As Single
        Dim PrnPlQ_ID As Integer = 0
        Dim Unt_Id As Integer = 0
        Dim YrnClthNm As String = ""
        Dim Nr As Integer = 0
        Dim PurCd As String = ""
        Dim PurSlNo As Long = 0
        Dim DcCd As String = ""
        Dim DcSlNo As Long = 0
        Dim PcsChkCode As String = ""
        Dim Bil_Sts As Integer = 0
        Dim Clr_ID As Integer = 0
        Dim Dc_Date As Single = 0
        Dim vChk_InvNo As Integer = 0
        Dim vChk_FlmScrnCrge As Integer = 0
        Dim vJb_Code_Sltn As String = ""

        Dim vCount_ID As Integer = 0
        Dim vEndsCount_ID As Integer = 0
        Dim vCloth_ID As Integer = 0


        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        '   If Common_Procedures.UserRight_Check(Common_Procedures.UR.Fabric_Receipt_Entry, New_Entry) = False Then Exit Sub

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(msk_date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If dtp_date.Enabled And dtp_date.Visible Then dtp_date.Focus()
            Exit Sub
        End If

        'If Not (dtp_date.Value.Date >= Common_Procedures.Company_FromDate And dtp_date.Value.Date <= Common_Procedures.Company_ToDate) Then
        '    MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '    If dtp_date.Enabled And dtp_date.Visible Then dtp_date.Focus()
        '    Exit Sub
        'End If

        If Not (Convert.ToDateTime(msk_date.Text) >= Common_Procedures.Company_FromDate And Convert.ToDateTime(msk_date.Text) <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If msk_date.Enabled Then msk_date.Focus()
            Exit Sub
        End If
        Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_WeaverName.Text)

        If Led_ID = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_WeaverName.Enabled Then cbo_WeaverName.Focus()
            Exit Sub
        End If



        Bil_Sts = 0

        NoCalc_Status = False
        Total_CountCalculation()
        Dim vTot_CntMtrs As Single = 0
        vTot_EndsCntMtrs = 0 : vTotAmt = 0 : vTotFmAChg = 0 : vtot_ClthMtr = 0 : vTotScnAChg = 0 : vTotScnChg = 0

        If dgv_CountDetails_Total.RowCount > 0 Then
            vTot_CntMtrs = Val(dgv_CountDetails_Total.Rows(0).Cells(2).Value())
        End If
        If dgv_EndsCount_Details_Total.RowCount > 0 Then
            vTot_EndsCntMtrs = Val(dgv_EndsCount_Details_Total.Rows(0).Cells(2).Value())
        End If

        If dgv_Cloth_Details_Total.RowCount > 0 Then
            vtot_ClthMtr = Val(dgv_Cloth_Details_Total.Rows(0).Cells(2).Value())

        End If


        tr = con.BeginTransaction

        'Try

        If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            Else

                lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Vendor_Closing_Stock_Value_Head", "vendor_Stock_Closing_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            End If


            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@InvDate", Convert.ToDateTime(msk_date.Text))

            If New_Entry = True Then

                cmd.CommandText = "Insert into Vendor_Closing_Stock_Value_Head (       vendor_Stock_Closing_Code      ,                 Company_IdNo      ,        vendor_Stock_Closing_No                  ,                                 for_OrderBy                            , vendor_Stock_Closing_date ,         Weaver_IdNo          ,             Total_CountWeight          ,         Total_EndsCount_meters           ,          Total_Cloth_meters       ) " &
                                  "Values                                       ( '" & Trim(NewCode) & "'            , " & Str(Val(lbl_Company.Tag)) & " ,              '" & Trim(lbl_RefNo.Text) & "'      , " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & " ,     @InvDate               , " & Str(Val(Led_ID)) & "   , " & Str(Val(vTot_CntMtrs)) & "         ," & Str(Val(vTot_EndsCntMtrs)) & "        , " & Str(Val(vtot_ClthMtr)) & "     ) "
                cmd.ExecuteNonQuery()

            Else

                cmd.CommandText = "Update Vendor_Closing_Stock_Value_Head set vendor_Stock_Closing_date = @InvDate, Weaver_IdNo = " & Str(Val(Led_ID)) & " ,Total_CountWeight=" & Str(Val(vTot_CntMtrs)) & ",Total_EndsCount_meters=" & Str(Val(vTot_EndsCntMtrs)) & ",Total_Cloth_meters=" & Str(Val(vtot_ClthMtr)) & "  Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and vendor_Stock_Closing_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()


            End If

            EntID = Trim(Pk_Condition) & Trim(lbl_RefNo.Text)
            PBlNo = Trim(lbl_RefNo.Text)
            Partcls = "Invoice : Inv.No. " & Trim(lbl_RefNo.Text)


            cmd.CommandText = "delete from vendor_Closing_Cloth_Stock_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and vendor_Stock_Closing_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from vendor_Closing_EndsCount_Stock_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and vendor_Stock_Closing_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from vendor_Closing_Count_Stock_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and vendor_Stock_Closing_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()


            With dgv_Count_details

                Sno = 0

                For i = 0 To .RowCount - 1

                    If Trim(.Rows(i).Cells(1).Value) <> "" Then

                        Sno = Sno + 1

                        vCount_ID = Common_Procedures.Count_NameToIdNo(con, .Rows(i).Cells(1).Value, tr)

                        cmd.CommandText = "Insert into vendor_Closing_Count_Stock_Details (        vendor_Stock_Closing_Code     ,                 Company_IdNo      ,               vendor_Stock_Closing_No      ,                       for_OrderBy                                      , vendor_Stock_Closing_date ,         Weaver_IdNo       ,             Count_IdNo      ,              Sl_No   ,                       Weight       ) " &
                                          "Values                               ( '" & Trim(NewCode) & "' , " & Str(Val(lbl_Company.Tag)) & " , '" & Trim(lbl_RefNo.Text) & "' , " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & "                                     ,     @InvDate             , " & Str(Val(Led_ID)) & " , " & Str(Val(vCount_ID)) & " , " & Str(Val(Sno)) & ",'" & Trim(.Rows(i).Cells(2).Value) & "'  ) "
                        cmd.ExecuteNonQuery()

                    End If

                Next

            End With
            With dgv_EndsCount_Details
                Slno = 0
                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(1).Value) <> 0 Then
                        vEndsCount_ID = Common_Procedures.EndsCount_NameToIdNo(con, .Rows(i).Cells(1).Value, tr)
                        Slno = Slno + 1
                        cmd.CommandText = "Insert into vendor_Closing_EndsCount_Stock_Details (       vendor_Stock_Closing_Code      ,                 Company_IdNo      ,          vendor_Stock_Closing_No           ,                                 for_OrderBy                            , vendor_Stock_Closing_date ,             Weaver_IdNo       ,             EndsCount_IdNo ,               Sl_No   ,                       Meters          ) " &
                                          "Values                                               ( '" & Trim(NewCode) & "' , " & Str(Val(lbl_Company.Tag)) & "            , '" & Trim(lbl_RefNo.Text) & "'               , " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & " ,     @InvDate , " & Str(Val(Led_ID)) & " , " & Str(Val(vEndsCount_ID)) & "       , " & Str(Val(Slno)) & "          ,'" & Trim(.Rows(i).Cells(2).Value) & "'  ) "
                        cmd.ExecuteNonQuery()

                    End If
                Next
            End With



            With dgv_Cloth_Details

                Sno1 = 0

                For i = 0 To .RowCount - 1

                    If Trim(.Rows(i).Cells(1).Value) <> "" Then

                        Sno1 = Sno1 + 1
                        vCloth_ID = Common_Procedures.Cloth_NameToIdNo(con, .Rows(i).Cells(1).Value, tr)


                        cmd.CommandText = "Insert into vendor_Closing_Cloth_Stock_Details       (        vendor_Stock_Closing_Code      ,                 Company_IdNo      ,          vendor_Stock_Closing_No           ,                                 for_OrderBy                            , vendor_Stock_Closing_date ,             Weaver_IdNo       ,             Cloth_IdNo             ,               Sl_No              ,                       Meters          ) " &
                                          "Values                                               ( '" & Trim(NewCode) & "' , " & Str(Val(lbl_Company.Tag)) & "            , '" & Trim(lbl_RefNo.Text) & "'               , " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & " ,     @InvDate            , " & Str(Val(Led_ID)) & " , " & Val(vCloth_ID) & "       , " & Str(Val(Sno1)) & "          ,'" & Trim(.Rows(i).Cells(2).Value) & "'  ) "
                        cmd.ExecuteNonQuery()




                    End If

                Next

            End With



            tr.Commit()


            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

            If New_Entry = False Then
                move_record(lbl_RefNo.Text)
            Else
                new_record()
            End If

        'Catch ex As Exception
        '    tr.Rollback()
        '    MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        'Finally
        '    Dt1.Dispose()
        '    Da.Dispose()
        '    cmd.Dispose()
        '    tr.Dispose()

        '    If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()

        'End Try

    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        'Dim da1 As New SqlClient.SqlDataAdapter
        'Dim dt1 As New DataTable
        'Dim NewCode As String
        'Dim ps As Printing.PaperSize

        'NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        'Try

        '    da1 = New SqlClient.SqlDataAdapter("select * from Vendor_Closing_Stock_Value_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and vendor_Stock_Closing_Code = '" & Trim(NewCode) & "'", con)
        '    dt1 = New DataTable
        '    da1.Fill(dt1)

        '    If dt1.Rows.Count <= 0 Then

        '        MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '        Exit Sub

        '    End If

        '    dt1.Dispose()
        '    da1.Dispose()

        'Catch ex As Exception
        '    MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        'End Try

        'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
        '        ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '        PrintDocument1.DefaultPageSettings.PaperSize = ps
        '        Exit For
        '    End If
        'Next


        'If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then
        '    Try

        '        If Val(Common_Procedures.settings.Printing_Show_PrintDialogue) = 1 Then
        '            PrintDialog1.PrinterSettings = PrintDocument1.PrinterSettings
        '            If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
        '                PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings

        '                For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '                    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
        '                        ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '                        PrintDocument1.DefaultPageSettings.PaperSize = ps
        '                        Exit For
        '                    End If
        '                Next

        '                PrintDocument1.Print()
        '            End If

        '        Else
        '            PrintDocument1.Print()

        '        End If

        '    Catch ex As Exception
        '        MessageBox.Show("The printing operation failed" & vbCrLf & ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        '    End Try


        'Else
        '    Try

        '        Dim ppd As New PrintPreviewDialog

        '        ppd.Document = PrintDocument1

        '        ppd.WindowState = FormWindowState.Maximized
        '        ppd.StartPosition = FormStartPosition.CenterScreen
        '        ppd.ClientSize = New Size(762, 1024)

        '        ppd.ShowDialog()

        '    Catch ex As Exception
        '        MsgBox("The printing operation failed" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "DOES NOT SHOW PRINT PREVIEW...")

        '    End Try

        'End If


    End Sub

    Private Sub PrintDocument1_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument1.BeginPrint
        'Dim da1 As New SqlClient.SqlDataAdapter
        'Dim da2 As New SqlClient.SqlDataAdapter
        'Dim da3 As New SqlClient.SqlDataAdapter
        'Dim da4 As New SqlClient.SqlDataAdapter
        'Dim Dt1 As New DataTable
        'Dim Dt3 As New DataTable
        'Dim cmd As New SqlClient.SqlCommand
        'Dim NewCode As String
        'Dim Pty_DcNo As String
        'Dim Clt_RcptDate As String
        'Dim sql As String = ""



        'Dim Nr As Long = 0

        'NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        'prn_HdDt.Clear()
        'prn_DetDt.Clear()
        'prn_DetIndx = 0
        'prn_DetSNo = 0
        'prn_PageNo = 0

        'Try

        '    cmd.Connection = con

        '    da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.*,e.*, Csh.State_Name as Company_State_Name, Csh.State_Code as Company_State_Code, Lsh.State_Name as Ledger_State_Name, Lsh.State_Code as Ledger_State_Code from Vendor_Closing_Stock_Value_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo LEFT OUTER JOIN State_Head Csh ON b.Company_State_IdNo = Csh.State_IdNo INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_Idno LEFT OUTER JOIN State_Head Lsh ON c.Ledger_State_IdNo = Lsh.State_IdNo LEFT OUTER JOIN Design_Inward_Head e ON a.Job_Code = e.Design_Inward_Code where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.vendor_Stock_Closing_Code = '" & Trim(NewCode) & "'", con)
        '    prn_HdDt = New DataTable
        '    da1.Fill(prn_HdDt)

        '    If prn_HdDt.Rows.Count > 0 Then





        '        cmd.CommandText = "truncate table " & Trim(Common_Procedures.EntryTempTable) & ""
        '        cmd.ExecuteNonQuery()

        '        ''vendor_Stock_Closing_Code
        '        ''Print_Place_IdNo
        '        ''Cloth_Delivery_Code
        '        ''Design_Inward_Code
        '        ''Rate_For
        '        ''sl_no
        '        ''ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Print_Place_Name").ToString) & " " & Trim(prn_DetDt.Rows(prn_DetIndx).Item("Particulars").ToString)
        '        'Common_Procedures.Print_To_PrintDocument(e, Trim(Val(SNo)), LMargin + 10, CurY, 0, 0, pFont)
        '        'Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("PARTY_DCNO").ToString), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
        '        'Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(Clt_RcptDate), "dd-MM-yyyy").ToString, LMargin + ClArr(1) + ClArr(2) + 5, CurY, 0, 0, pFont)
        '        'Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 5, CurY, 0, 0, pFont)
        '        'Common_Procedures.Print_To_PrintDocument(e, Trim(ClrNm1), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 3, CurY, 0, 0, pFont)
        '        'Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Quantity").ToString), "##,##,##,##0"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 15, CurY, 1, 0, pFont)
        '        'Common_Procedures.Print_To_PrintDocument(e, Format(Val(Rate), "##,##,##,##0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
        '        'Common_Procedures.Print_To_PrintDocument(e, Format(Val(Amount), "##,##,##,##0.00"), PageWidth - 10, CurY, 1, 0, pFont)

        '        ''vendor_Closing_Cloth_Stock_Details
        '        '.Rows(n).Cells(0).Value = Val(SNo)
        '        '.Rows(n).Cells(1).Value = dt4.Rows(i).Item("Job_No").ToString
        '        '.Rows(n).Cells(2).Value = (dt4.Rows(i).Item("Particulars").ToString)
        '        '.Rows(n).Cells(3).Value = Format(Val(dt4.Rows(i).Item("Film_Charge").ToString), "###########0.00")
        '        '.Rows(n).Cells(4).Value = Format(Val(dt4.Rows(i).Item("Screen_Charge").ToString), "############0.00")
        '        '.Rows(n).Cells(5).Value = Format(Val(dt4.Rows(i).Item("Film_Alteration_Charge").ToString), "###########0.00")
        '        '.Rows(n).Cells(6).Value = Format(Val(dt4.Rows(i).Item("Screen_Alteration_Charge").ToString), "###########0.00")
        '        '.Rows(n).Cells(7).Value = dt4.Rows(i).Item("Design_Inward_Code").ToString

        '        '' vinoth
        '        'cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempTable) & "(Int1,     Name1        ,     Name2              ,    Name3              ,     Name4 ,     Int2 ,     Name5     ,     Int3             ,    Name6            ,  Name7          ,  Weight1   ,     Name8    ,    Name9     ,    Name10        ) " & _
        '        '                  "select                 1  ,  PID.vendor_Stock_Closing_Code, PID.Cloth_Delivery_Code, PID.Design_Inward_Code, PID.Job_No, PID.sl_no, PID.PARTY_DCNO,  PID.Print_Place_IdNo, PPH.Print_Place_Name,  PID.Particulars, PID.Quantity, DIH.Rate_For , CDD.Order_No , PIH.Order_RefNo  FROM Vendor_Closing_Stock_Value_Head PIH INNER JOIN vendor_Closing_Count_Stock_Details PID ON PID.vendor_Stock_Closing_Code = PIH.vendor_Stock_Closing_Code INNER JOIN Design_Inward_Head DIH ON PID.Design_Inward_Code = DIH.Design_Inward_Code LEFT OUTER JOIN Cloth_Delivery_Details CDD ON PID.vendor_Stock_Closing_Code = CDD.vendor_Stock_Closing_Code and PID.Cloth_Delivery_Code = CDD.Cloth_Delivery_Code LEFT OUTER JOIN Print_Place_Head PPH ON PID.Print_Place_IdNo = PPH.Print_Place_IdNo  where PID.vendor_Stock_Closing_Code = '" & Trim(NewCode) & "' Order by PID.sl_no"
        '        'Nr = cmd.ExecuteNonQuery()




        '        'cmd.CommandText = " Insert into " & Trim(Common_Procedures.EntryTempTable) & "(		Int1,   		Name1        ,   		Name2              ,    Name3              ,  		Name4 ,      " & _
        '        '                    " Int2, Name5, Int3, Name6, Name7, " & _
        '        '                    " Weight1   ,    		Name8    ,    		Name9     ,    		Name10        )  " & _
        '        '                    " select   	1  ,  			PID.vendor_Stock_Closing_Code, 			PID.Cloth_Delivery_Code, 			PID.Design_Inward_Code,  " & _
        '        '                    " PID.Job_No, 			PID.sl_no, 			PID.PARTY_DCNO,  			PID.Print_Place_IdNo,  " & _
        '        '                    " PPH.Print_Place_Name,  			PID.Particulars, 			IQD.Quantity,  			DIH.Rate_For ,  " & _
        '        '                    " CDD.Order_No , 			PIH.Order_RefNo  			FROM Vendor_Closing_Stock_Value_Head PIH  " & _
        '        '                    " 	INNER JOIN vendor_Closing_Count_Stock_Details PID ON PID.vendor_Stock_Closing_Code = PIH.vendor_Stock_Closing_Code  " & _
        '        '                    " INNER JOIN vendor_Closing_EndsCount_Stock_Details IQD ON PID.vendor_Stock_Closing_Code = IQD.vendor_Stock_Closing_Code and PID.print_place_idno=IQD.print_place_idno " & _
        '        '                    " 	INNER JOIN Design_Inward_Head DIH ON PID.Design_Inward_Code = DIH.Design_Inward_Code  " & _
        '        '                    " 	LEFT OUTER JOIN Cloth_Delivery_Details CDD ON PID.vendor_Stock_Closing_Code = CDD.vendor_Stock_Closing_Code " & _
        '        '                    " and PID.Cloth_Delivery_Code = CDD.Cloth_Delivery_Code  " & _
        '        '                    " and PID.Print_place_idno=CDD.Print_place_idno " & _
        '        '                    " 	LEFT OUTER JOIN Print_Place_Head PPH ON PID.Print_Place_IdNo = PPH.Print_Place_IdNo  " & _
        '        '                    " where PID.vendor_Stock_Closing_Code = '" & Trim(NewCode) & "' Order by PID.sl_no"




        '        'cmd.CommandText = " Insert into " & Trim(Common_Procedures.EntryTempTable) & "(		Int1,   		Name1        ,   		Name2              ,    Name3              ,  		Name4 ,      " & _
        '        '                          " Int2, Name5, Int3, Name6, Name7,  Weight1   ,    		Name8    ,    		Name9     ,    		Name10        ) " & _
        '        '                          " select   	1  ,  			 " & _
        '        '                          "                   IQD.vendor_Stock_Closing_Code, 	 " & _
        '        '                          "                   '' Cloth_Delivery_Code, " & _
        '        '                          "                   IQD.Design_Inward_Code,     " & _
        '        '                          "                   '' Job_No, 			 " & _
        '        '                          " 0 sl_no, 			 " & _
        '        '                          "                   '' PARTY_DCNO,  			 " & _
        '        '                          " IQD.Print_Place_IdNo,    " & _
        '        '                          " PPH.Print_Place_Name,  			 " & _
        '        '                         " PPH.Print_Place_Name,  	 " & _
        '        '                          " IQD.Quantity,  			 " & _
        '        '                          "                     DIH.Rate_For ,      " & _
        '        '                          " 0 Order_No , 			 " & _
        '        '                          " 0 Order_RefNo  			 " & _
        '        '                          " FROM Vendor_Closing_Stock_Value_Head PIH " & _
        '        '                          " INNER JOIN vendor_Closing_EndsCount_Stock_Details IQD ON PIH.vendor_Stock_Closing_Code = IQD.vendor_Stock_Closing_Code 	 " & _
        '        '                            " INNER JOIN Design_Inward_Head DIH ON IQD.Design_Inward_Code = DIH.Design_Inward_Code " & _
        '        '                         " LEFT OUTER JOIN Print_Place_Head PPH ON IQD.Print_Place_IdNo = PPH.Print_Place_IdNo  " & _
        '        '                          "  where PIH.vendor_Stock_Closing_Code = '" & Trim(NewCode) & "' Order by IQD.sl_no "


        '        'Nr = cmd.ExecuteNonQuery()


        '        ''cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempTable) & "(Int1,     Name1       ,     Name2 ,     Name3             ,     Name4 ,     Int2 , Name5 , Int3 , Name6 ,       Name7      ,  Weight1 , Name8,                       Currency1      ) " & _
        '        ''                    "          select      2 , PID.vendor_Stock_Closing_Code,     ''    , DIH.Design_Inward_Code, PID.Job_No, PID.sl_no,  ''   ,   0  ,    ''  ,  'Screen Charge',     0    ,   '' ,  (Film_Charge+Screen_Charge+Film_Alteration_Charge+Screen_Alteration_Charge) FROM vendor_Closing_Cloth_Stock_Details PID INNER JOIN Design_Inward_Head DIH ON PID.Design_Inward_Code = DIH.Design_Inward_Code_forSelection where PID.vendor_Stock_Closing_Code = '" & Trim(NewCode) & "' Order by PID.sl_no"
        '        ''nr = cmd.ExecuteNonQuery()

        '        'da2 = New SqlClient.SqlDataAdapter("select Name1 as vendor_Stock_Closing_Code, Name2 as Cloth_Delivery_Code, Name3 as Design_Inward_Code, Name4 as Job_No, Int2 as sl_no, Name5 as PARTY_DCNO, Int3 as Print_Place_IdNo, Name6 as Print_Place_Name, Name7 as Particulars,  Weight1  as Quantity, Currency1 as Screen_Charge, Name8 as Rate_For, Name9 as Order_No, Name10 as Order_RefNo  FROM " & Trim(Common_Procedures.EntryTempTable) & " Order by Int1, Int2", con)
        '        ''da2 = New SqlClient.SqlDataAdapter("select PID.*, DIH.*, PPH.Print_Place_Name FROM vendor_Closing_Count_Stock_Details PID INNER JOIN Design_Inward_Head DIH ON PID.Design_Inward_Code = DIH.Design_Inward_Code LEFT JOIN Print_Place_Head PPH ON PID.Print_Place = PPH.Print_Place_IdNo  where PID.vendor_Stock_Closing_Code = '" & Trim(NewCode) & "' Order by PID.sl_no", con)
        '        ' ''da2 = New SqlClient.SqlDataAdapter("select PID.*, PPH.*,IQD.* FROM vendor_Closing_Count_Stock_Details PID INNER JOIN vendor_Closing_EndsCount_Stock_Details IQD ON PID.vendor_Stock_Closing_Code = IQD.vendor_Stock_Closing_Code LEFT JOIN Print_Place_Head PPH ON PID.Print_Place = PPH.Print_Place_IdNo  where PID.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and PID.vendor_Stock_Closing_Code = '" & Trim(NewCode) & "' Order by PID.sl_no", con)
        '        '' ''da2 = New SqlClient.SqlDataAdapter("select PID.*,b.*,c.* ,cD.* ,CLRD.Cloth_Receipt_Date, CL.Colour_Name , Pp.* from vendor_Closing_Count_Stock_Details PId INNER JOIN vendor_Closing_EndsCount_Stock_Details IQD ON PId.vendor_Stock_Closing_Code = IQD.vendor_Stock_Closing_Code INNER JOIN Cloth_Delivery_Details cD ON pID.Cloth_Delivery_Code = cD.Cloth_Delivery_Code INNER JOIN Design_Inward_Head b ON PID.Design_Inward_Code = b.Design_Inward_Code INNER JOIN Design_InWard_PrintPlace_Details c ON PID.Design_Inward_Code = c.Design_Inward_Code LEFT JOIN Colour_Head CL ON cD.COLOUR_IDNO= CL.Colour_IdNo LEFT JOIN Print_Place_Head Pp ON PID.Print_Place_IdNo = Pp.Print_Place_IdNo INNER JOIN Cloth_Receipt_Details CLRD ON cD.Cloth_Receipt_Code = CLRD.Cloth_Receipt_Code  where PID.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and PID.vendor_Stock_Closing_Code = '" & Trim(NewCode) & "' Order by PID.sl_no", con)
        '        'prn_DetDt = New DataTable
        '        'da2.Fill(prn_DetDt)


        '        sql = " " & _
        '                " select   	1  ,  			 " & _
        '                "                   IQD.vendor_Stock_Closing_Code, 	 " & _
        '                "                   '' Cloth_Delivery_Code, " & _
        '                "                   IQD.Design_Inward_Code,     " & _
        '                "                   '' Job_No, 			 " & _
        '                " 0 sl_no, 			 " & _
        '                "                   '' PARTY_DCNO,  			 " & _
        '                " IQD.Print_Place_IdNo,    " & _
        '                " PPH.Print_Place_Name,  			 " & _
        '                " PPH.Print_Place_Name,  	 " & _
        '                " IQD.Quantity,  			 " & _
        '                " IQD.Rate, IQD.Amount, " & _
        '                "                     DIH.Rate_For ,      " & _
        '                " 0 Order_No , 			 " & _
        '                " 0 Order_RefNo  			 " & _
        '                " FROM Vendor_Closing_Stock_Value_Head PIH " & _
        '                " INNER JOIN vendor_Closing_EndsCount_Stock_Details IQD ON PIH.vendor_Stock_Closing_Code = IQD.vendor_Stock_Closing_Code 	 " & _
        '                    " INNER JOIN Design_Inward_Head DIH ON IQD.Design_Inward_Code = DIH.Design_Inward_Code " & _
        '                " LEFT OUTER JOIN Print_Place_Head PPH ON IQD.Print_Place_IdNo = PPH.Print_Place_IdNo  " & _
        '                "  where PIH.vendor_Stock_Closing_Code = '" & Trim(NewCode) & "' Order by IQD.sl_no "

        '        '"select Name1 as vendor_Stock_Closing_Code, Name2 as Cloth_Delivery_Code, Name3 as Design_Inward_Code, Name4 as Job_No, Int2 as sl_no, Name5 as PARTY_DCNO, Int3 as Print_Place_IdNo, Name6 as Print_Place_Name, Name7 as Particulars,  Weight1  as Quantity, Currency1 as Screen_Charge, Name8 as Rate_For, Name9 as Order_No, Name10 as Order_RefNo  FROM " & Trim(Common_Procedures.EntryTempTable) & " Order by Int1, Int2"
        '        'Nr = cmd.ExecuteNonQuery()


        '        'cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempTable) & "(Int1,     Name1       ,     Name2 ,     Name3             ,     Name4 ,     Int2 , Name5 , Int3 , Name6 ,       Name7      ,  Weight1 , Name8,                       Currency1      ) " & _
        '        '                    "          select      2 , PID.vendor_Stock_Closing_Code,     ''    , DIH.Design_Inward_Code, PID.Job_No, PID.sl_no,  ''   ,   0  ,    ''  ,  'Screen Charge',     0    ,   '' ,  (Film_Charge+Screen_Charge+Film_Alteration_Charge+Screen_Alteration_Charge) FROM vendor_Closing_Cloth_Stock_Details PID INNER JOIN Design_Inward_Head DIH ON PID.Design_Inward_Code = DIH.Design_Inward_Code_forSelection where PID.vendor_Stock_Closing_Code = '" & Trim(NewCode) & "' Order by PID.sl_no"
        '        'nr = cmd.ExecuteNonQuery()

        '        da2 = New SqlClient.SqlDataAdapter(sql, con)
        '        'da2 = New SqlClient.SqlDataAdapter("select PID.*, DIH.*, PPH.Print_Place_Name FROM vendor_Closing_Count_Stock_Details PID INNER JOIN Design_Inward_Head DIH ON PID.Design_Inward_Code = DIH.Design_Inward_Code LEFT JOIN Print_Place_Head PPH ON PID.Print_Place = PPH.Print_Place_IdNo  where PID.vendor_Stock_Closing_Code = '" & Trim(NewCode) & "' Order by PID.sl_no", con)
        '        ''da2 = New SqlClient.SqlDataAdapter("select PID.*, PPH.*,IQD.* FROM vendor_Closing_Count_Stock_Details PID INNER JOIN vendor_Closing_EndsCount_Stock_Details IQD ON PID.vendor_Stock_Closing_Code = IQD.vendor_Stock_Closing_Code LEFT JOIN Print_Place_Head PPH ON PID.Print_Place = PPH.Print_Place_IdNo  where PID.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and PID.vendor_Stock_Closing_Code = '" & Trim(NewCode) & "' Order by PID.sl_no", con)
        '        ' ''da2 = New SqlClient.SqlDataAdapter("select PID.*,b.*,c.* ,cD.* ,CLRD.Cloth_Receipt_Date, CL.Colour_Name , Pp.* from vendor_Closing_Count_Stock_Details PId INNER JOIN vendor_Closing_EndsCount_Stock_Details IQD ON PId.vendor_Stock_Closing_Code = IQD.vendor_Stock_Closing_Code INNER JOIN Cloth_Delivery_Details cD ON pID.Cloth_Delivery_Code = cD.Cloth_Delivery_Code INNER JOIN Design_Inward_Head b ON PID.Design_Inward_Code = b.Design_Inward_Code INNER JOIN Design_InWard_PrintPlace_Details c ON PID.Design_Inward_Code = c.Design_Inward_Code LEFT JOIN Colour_Head CL ON cD.COLOUR_IDNO= CL.Colour_IdNo LEFT JOIN Print_Place_Head Pp ON PID.Print_Place_IdNo = Pp.Print_Place_IdNo INNER JOIN Cloth_Receipt_Details CLRD ON cD.Cloth_Receipt_Code = CLRD.Cloth_Receipt_Code  where PID.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and PID.vendor_Stock_Closing_Code = '" & Trim(NewCode) & "' Order by PID.sl_no", con)
        '        prn_DetDt = New DataTable
        '        da2.Fill(prn_DetDt)


        '    Else
        '        MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        '    End If

        '    da1.Dispose()

        'Catch ex As Exception
        '    MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        'End Try

    End Sub

    Private Sub PrintDocument1_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        'If prn_HdDt.Rows.Count <= 0 Then Exit Sub

        ''If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1175" Then
        ''    Printing_Format2(e)
        ''Else
        ''    Printing_Format1(e)
        ''End If


        ''---printing_format3_printtech

    End Sub

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
    '        If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
    '            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
    '            PrintDocument1.DefaultPageSettings.PaperSize = ps
    '            e.PageSettings.PaperSize = ps
    '            Exit For
    '        End If
    '    Next

    '    With PrintDocument1.DefaultPageSettings.Margins
    '        .Left = 20
    '        .Right = 50
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

    '    NoofItems_PerPage = 20 ' 6

    '    Erase LnAr
    '    Erase ClArr

    '    LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
    '    ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

    '    ClArr(1) = Val(35) : ClArr(2) = 60 : ClArr(3) = 120 : ClArr(4) = 250 : ClArr(5) = 85 : ClArr(6) = 85
    '    ClArr(7) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6))

    '    TxtHgt = 19.5 ' 18 ' e.Graphics.MeasureString("A", pFont).Height ' 20

    '    EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

    '    Try

    '        If prn_HdDt.Rows.Count > 0 Then

    '            Printing_Format1_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)


    '            ' W1 = e.Graphics.MeasureString("No.of Beams : ", pFont).Width

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

    '                    ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Particulars").ToString)
    '                    ItmNm2 = ""
    '                    If Len(ItmNm1) > 30 Then
    '                        For I = 30 To 1 Step -1
    '                            If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
    '                        Next I
    '                        If I = 0 Then I = 30
    '                        ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
    '                        ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
    '                    End If
    '                    CurY = CurY + TxtHgt

    '                    Common_Procedures.Print_To_PrintDocument(e, "HSN /SAC CODE : 998821", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)

    '                    CurY = CurY + TxtHgt
    '                    SNo = SNo + 1
    '                    Common_Procedures.Print_To_PrintDocument(e, Trim(Val(SNo)), LMargin + 10, CurY, 0, 0, pFont)
    '                    Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Job_NO").ToString, LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
    '                    ' Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.PrintPlace_IdNoToName(con, Val(prn_DetDt.Rows(prn_DetIndx).Item("Print_Place_IdNo").ToString)), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
    '                    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)
    '                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Quantity").ToString), "########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)
    '                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("RAte").ToString), "########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)

    '                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Amount").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)



    '                    NoofDets = NoofDets + 1

    '                    If Trim(ItmNm2) <> "" Then
    '                        CurY = CurY + TxtHgt - 5
    '                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)
    '                        NoofDets = NoofDets + 1
    '                    End If

    '                    prn_DetIndx = prn_DetIndx + 1

    '                Loop

    '            End If


    '            Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)


    '        End If

    '    Catch ex As Exception

    '        MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

    '    End Try

    '    e.HasMorePages = False

    'End Sub

    'Private Sub Printing_Format1_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
    '    Dim da2 As New SqlClient.SqlDataAdapter
    '    Dim dt2 As New DataTable
    '    Dim p1Font As Font
    '    Dim strHeight As Single = 0, strWidth As Single = 0
    '    Dim C1 As Single, W1 As Single, S1 As Single, C2 As Single
    '    Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
    '    Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
    '    Dim Cmp_EMail As String
    '    Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
    '    Dim CurY1 As Single = 0, CurX As Single = 0
    '    PageNo = PageNo + 1

    '    CurY = TMargin

    '    da2 = New SqlClient.SqlDataAdapter("select a.* from vendor_Closing_Count_Stock_Details a where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.vendor_Stock_Closing_Code = '" & Trim(EntryCode) & "' Order by a.sl_no", con)
    '    da2.Fill(dt2)

    '    If dt2.Rows.Count > NoofItems_PerPage Then
    '        Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
    '    End If
    '    dt2.Clear()

    '    e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '    LnAr(1) = CurY

    '    Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
    '    Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_EMail = ""
    '    Cmp_StateCap = "" : Cmp_StateNm = "" : Cmp_StateCode = "" : Cmp_GSTIN_Cap = "" : Cmp_GSTIN_No = ""
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
    '    If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
    '        Cmp_EMail = "Email : " & prn_HdDt.Rows(0).Item("Company_EMail").ToString
    '    End If
    '    If Trim(prn_HdDt.Rows(0).Item("Company_State_Name").ToString) <> "" Then
    '        Cmp_StateCap = "STATE : "
    '        Cmp_StateNm = prn_HdDt.Rows(0).Item("Company_State_Name").ToString
    '    End If
    '    If Trim(prn_HdDt.Rows(0).Item("Company_State_Code").ToString) <> "" Then
    '        Cmp_StateCode = "CODE :" & prn_HdDt.Rows(0).Item("Company_State_Code").ToString
    '    End If
    '    If Trim(prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString) <> "" Then
    '        Cmp_GSTIN_Cap = "GSTIN : "
    '        Cmp_GSTIN_No = prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
    '    End If


    '    CurY = CurY + TxtHgt - 10
    '    p1Font = New Font("Calibri", 18, FontStyle.Bold)
    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
    '    strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

    '    CurY = CurY + strHeight - 1
    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

    '    CurY = CurY + TxtHgt - 1
    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
    '    CurY = CurY + TxtHgt

    '    p1Font = New Font("Calibri", 11, FontStyle.Bold)
    '    strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_GSTIN_Cap), p1Font).Width
    '    strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_StateNm & " " & Cmp_GSTIN_Cap & Cmp_GSTIN_No), pFont).Width
    '    If PrintWidth > strWidth Then
    '        CurX = LMargin + (PrintWidth - strWidth) / 2
    '    Else
    '        CurX = LMargin
    '    End If

    '    p1Font = New Font("Calibri", 11, FontStyle.Bold)
    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_StateCap, CurX, CurY, 0, 0, p1Font)
    '    strWidth = e.Graphics.MeasureString(Cmp_StateCap, p1Font).Width
    '    CurX = CurX + strWidth
    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_StateNm, CurX, CurY, 0, 0, pFont)

    '    strWidth = e.Graphics.MeasureString(Cmp_StateNm, pFont).Width
    '    p1Font = New Font("Calibri", 11, FontStyle.Bold)
    '    CurX = CurX + strWidth
    '    Common_Procedures.Print_To_PrintDocument(e, " " & Cmp_GSTIN_Cap, CurX, CurY, 0, PrintWidth, p1Font)
    '    strWidth = e.Graphics.MeasureString(" " & Cmp_GSTIN_Cap, p1Font).Width
    '    CurX = CurX + strWidth
    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, CurX, CurY, 0, 0, pFont)

    '    CurY = CurY + TxtHgt
    '    Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PhNo & " " & Cmp_EMail), LMargin, CurY, 2, PrintWidth, pFont)

    '    CurY = CurY + TxtHgt
    '    p1Font = New Font("Calibri", 16, FontStyle.Bold)
    '    Common_Procedures.Print_To_PrintDocument(e, "INVOICE", LMargin, CurY, 2, PrintWidth, p1Font)
    '    strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


    '    CurY = CurY + strHeight ' + 150
    '    e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '    LnAr(2) = CurY

    '    Try
    '        C1 = ClAr(1) + ClAr(2) + ClAr(3) + 100
    '        C2 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 50
    '        W1 = e.Graphics.MeasureString("PROCESS N: ", pFont).Width
    '        S1 = e.Graphics.MeasureString("FROM : ", pFont).Width

    '        CurY = CurY + TxtHgt - 10
    '        p1Font = New Font("Calibri", 12, FontStyle.Bold)
    '        Common_Procedures.Print_To_PrintDocument(e, "FROM : " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
    '        Common_Procedures.Print_To_PrintDocument(e, "BILL.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("vendor_Stock_Closing_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)
    '        Common_Procedures.Print_To_PrintDocument(e, "ORDER NO", LMargin + C2 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Order_No").ToString, LMargin + C2 + W1 + 30, CurY, 0, 0, pFont)

    '        CurY = CurY + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
    '        ' p1Font = New Font("Calibri", 14, FontStyle.Bold)

    '        CurY = CurY + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("vendor_Stock_Closing_date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "STYLE NO", LMargin + C2 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Style_No").ToString, LMargin + C2 + W1 + 30, CurY, 0, 0, pFont)

    '        CurY = CurY + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

    '        CurY = CurY + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "BUYER", LMargin + C1 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
    '        ' Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Buyer_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("Buyer_IdNo").ToString)), LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "IO NO", LMargin + C2 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Internal_OrderNo").ToString, LMargin + C2 + W1 + 30, CurY, 0, 0, pFont)

    '        CurY = CurY + TxtHgt
    '        If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
    '            Common_Procedures.Print_To_PrintDocument(e, " GSTIN : " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
    '        End If
    '        CurY = CurY + TxtHgt + 5
    '        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '        LnAr(3) = CurY
    '        e.Graphics.DrawLine(Pens.Black, LMargin + C2, LnAr(3), LMargin + C2, LnAr(2))
    '        e.Graphics.DrawLine(Pens.Black, LMargin + C1, LnAr(3), LMargin + C1, LnAr(2))

    '        CurY = CurY + TxtHgt - 10
    '        Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "JOB NO", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "PRINT PLACE", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "PARTICULARS", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "QUANTITY", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "RATE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)

    '        CurY = CurY + TxtHgt + 5
    '        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '        LnAr(4) = CurY

    '    Catch ex As Exception

    '        MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

    '    End Try

    'End Sub

    'Private Sub Printing_Format1_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
    '    Dim p1Font As Font
    '    Dim I As Integer
    '    Dim Cmp_Name As String
    '    Dim W1 As Single = 0
    '    Dim BmsInWrds As String

    '    W1 = e.Graphics.MeasureString("No.of Beams : ", pFont).Width

    '    Try

    '        For I = NoofDets + 1 To NoofItems_PerPage

    '            CurY = CurY + TxtHgt

    '            prn_DetIndx = prn_DetIndx + 1

    '        Next

    '        CurY = CurY + TxtHgt
    '        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '        LnAr(5) = CurY
    '        p1Font = New Font("Calibri", 11, FontStyle.Bold)
    '        CurY = CurY + TxtHgt - 10
    '        If is_LastPage = True Then
    '            Common_Procedures.Print_To_PrintDocument(e, " TOTAL", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 10, CurY, 2, ClAr(4), pFont)
    '        End If

    '        If Val(prn_HdDt.Rows(0).Item("Total_Quantity").ToString) <> 0 Then
    '            If is_LastPage = True Then
    '                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Quantity").ToString), "#############0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
    '            End If
    '        End If
    '        If Val(prn_HdDt.Rows(0).Item("Total_Amount").ToString) <> 0 Then
    '            If is_LastPage = True Then
    '                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Amount").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
    '            End If
    '        End If

    '        CurY = CurY + TxtHgt - 15

    '        CurY = CurY + TxtHgt
    '        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '        LnAr(6) = CurY

    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))

    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))
    '        CurY = CurY + TxtHgt - 5
    '        If Val(prn_HdDt.Rows(0).Item("Total_Film_Charge").ToString) <> 0 Then
    '            Common_Procedures.Print_To_PrintDocument(e, "Film Charge", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 0, 0, pFont)
    '            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Total_Film_Charge").ToString), PageWidth - 10, CurY, 1, 0, pFont)

    '        End If
    '        If Val(prn_HdDt.Rows(0).Item("Total_Screen_Charge").ToString) <> 0 Then
    '            CurY = CurY + TxtHgt
    '            Common_Procedures.Print_To_PrintDocument(e, "Screen Charge", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 0, 0, pFont)
    '            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Total_Screen_Charge").ToString), PageWidth - 10, CurY, 1, 0, pFont)

    '        End If

    '        If Val(prn_HdDt.Rows(0).Item("Total_Film_Alteration_Charge").ToString) <> 0 Then
    '            CurY = CurY + TxtHgt
    '            Common_Procedures.Print_To_PrintDocument(e, "Film Alteration Charge", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 0, 0, pFont)
    '            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Total_Film_Alteration_Charge").ToString), PageWidth - 10, CurY, 1, 0, pFont)

    '        End If
    '        If Val(prn_HdDt.Rows(0).Item("Total_Screen_Alteration_Charge").ToString) <> 0 Then
    '            CurY = CurY + TxtHgt
    '            Common_Procedures.Print_To_PrintDocument(e, "Screen Alteration Charge", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 0, 0, pFont)
    '            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Total_Screen_Alteration_Charge").ToString), PageWidth - 10, CurY, 1, 0, pFont)

    '        End If

    '        If Val(prn_HdDt.Rows(0).Item("Total_Taxable_Amount").ToString) <> 0 Then
    '            CurY = CurY + TxtHgt
    '            Common_Procedures.Print_To_PrintDocument(e, "Total Before Tax", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 0, 0, pFont)
    '            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Total_Taxable_Amount").ToString), PageWidth - 10, CurY, 1, 0, pFont)

    '        End If

    '        If Val(prn_HdDt.Rows(0).Item("CGST_Percentage").ToString) <> 0 Then
    '            CurY = CurY + TxtHgt

    '            Common_Procedures.Print_To_PrintDocument(e, "CGST" & Trim(prn_HdDt.Rows(0).Item("CGST_Percentage").ToString) & "%", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 0, 0, pFont)
    '            ' Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
    '            Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("CGST_Amount").ToString), PageWidth - 10, CurY, 1, 0, pFont)
    '        End If
    '        If Val(prn_HdDt.Rows(0).Item("SGST_Percentage").ToString) <> 0 Then
    '            CurY = CurY + TxtHgt

    '            Common_Procedures.Print_To_PrintDocument(e, "SGST" & Trim(prn_HdDt.Rows(0).Item("SGST_Percentage").ToString) & "%", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 0, 0, pFont)
    '            ' Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
    '            Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("SGST_Amount").ToString), PageWidth - 10, CurY, 1, 0, pFont)
    '        End If

    '        If Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString) <> 0 Then
    '            CurY = CurY + TxtHgt + 5
    '            Common_Procedures.Print_To_PrintDocument(e, "Net Amount", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 0, 0, p1Font)
    '            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Net_Amount").ToString), PageWidth - 10, CurY, 1, 0, p1Font)

    '        End If
    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, PageWidth, CurY)
    '        p1Font = New Font("Calibri", 12, FontStyle.Bold)
    '        CurY = CurY + TxtHgt + 5
    '        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '        LnAr(9) = CurY
    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))

    '        CurY = CurY + 10

    '        BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))
    '        BmsInWrds = Replace(Trim((BmsInWrds)), "", "")
    '        StrConv(BmsInWrds, vbProperCase)
    '        Common_Procedures.Print_To_PrintDocument(e, "Rupees : " & BmsInWrds & " ", LMargin + 10, CurY, 0, 0, p1Font)
    '        CurY = CurY + TxtHgt + 10
    '        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

    '        CurY = CurY + TxtHgt
    '        If Val(Common_Procedures.User.IdNo) <> 1 Then
    '            Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
    '        End If
    '        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
    '        p1Font = New Font("Calibri", 12, FontStyle.Bold)
    '        Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)


    '        CurY = CurY + TxtHgt
    '        CurY = CurY + TxtHgt
    '        CurY = CurY + TxtHgt

    '        Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 20, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 220, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "Checked By ", LMargin + 370, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "Authorised Signature ", PageWidth - 15, CurY, 1, 0, pFont)

    '        CurY = CurY + TxtHgt + 5

    '        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
    '        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

    '    Catch ex As Exception

    '        MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

    '    End Try

    'End Sub

    'Private Sub Printing_Format2(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
    '    Dim cmd As New SqlClient.SqlCommand

    '    Dim Da1 As New SqlClient.SqlDataAdapter
    '    Dim Dt1 As New DataTable
    '    Dim da As New SqlClient.SqlDataAdapter
    '    Dim dt As New DataTable
    '    Dim Da2 As New SqlClient.SqlDataAdapter
    '    Dim Dt2 As New DataTable

    '    Dim EntryCode As String
    '    Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
    '    Dim pFont As Font
    '    Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
    '    Dim PrintWidth As Single, PrintHeight As Single
    '    Dim PageWidth As Single, PageHeight As Single
    '    Dim CurY As Single, TxtHgt As Single
    '    Dim LnAr(15) As Single, ClArr(15) As Single
    '    Dim ItmNm1 As String, ItmNm2 As String
    '    Dim ClrNm1 As String, ClrNm2 As String
    '    Dim ps As Printing.PaperSize
    '    Dim strHeight As Single = 0
    '    Dim PpSzSTS As Boolean = False
    '    Dim W1 As Single = 0
    '    Dim SNo As Integer = 0
    '    Dim Pty_DcNo As String = ""
    '    Dim Clt_RcptDate As String = ""
    '    Dim Colour_Name As String = ""
    '    Dim unit_Name As String = ""
    '    Dim Rate As String = 0, Amount As String = 0
    '    Dim vPdcNo1 As String = ""
    '    Dim vPdcNo2 As String = ""




    '    For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
    '        If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
    '            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
    '            PrintDocument1.DefaultPageSettings.PaperSize = ps
    '            e.PageSettings.PaperSize = ps
    '            Exit For
    '        End If
    '    Next

    '    With PrintDocument1.DefaultPageSettings.Margins
    '        .Left = 20
    '        .Right = 50
    '        .Top = 20
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

    '    NoofItems_PerPage = 15 ' 6

    '    Erase LnAr
    '    Erase ClArr

    '    LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
    '    ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

    '    ClArr(1) = 37 : ClArr(2) = 220 : ClArr(3) = 85 : ClArr(4) = 85 : ClArr(5) = 70 : ClArr(6) = 90 : ClArr(7) = 60
    '    ClArr(8) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6))

    '    TxtHgt = 19 '19.8 ' e.Graphics.MeasureString("A", pFont).Height ' 20

    '    EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

    '    '  Try

    '    If prn_HdDt.Rows.Count > 0 Then

    '        Printing_Format2_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

    '        ' W1 = e.Graphics.MeasureString("No.of Beams : ", pFont).Width

    '        NoofDets = 0

    '        CurY = CurY - 10

    '        If prn_DetDt.Rows.Count > 0 Then

    '            CurY = CurY + TxtHgt - 5
    '            ' Common_Procedures.Print_To_PrintDocument(e, "HSN /SAC CODE : 998821", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)

    '            Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

    '                If NoofDets >= NoofItems_PerPage Then
    '                    CurY = CurY + TxtHgt

    '                    Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

    '                    NoofDets = NoofDets + 1

    '                    Printing_Format2_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

    '                    e.HasMorePages = True
    '                    Return

    '                End If


    '                CurY = CurY + TxtHgt
    '                SNo = SNo + 1

    '                Pty_DcNo = ""
    '                Clt_RcptDate = ""
    '                Colour_Name = ""
    '                da = New SqlClient.SqlDataAdapter("select b.*,a.cloth_delivery_no,a.cloth_delivery_date,b.PARTY_DCNO, b.Cloth_Receipt_Date ,CLH.Colour_Name,uh.Unit_Name  from Cloth_Delivery_Details a INNER JOIN Cloth_Receipt_Head b ON a.Cloth_Receipt_Code = b.Cloth_Receipt_Code LEFT JOIN Colour_Head CLH ON A.COLOUR_IDNO = CLH.Colour_IdNo Left Join Unit_head uh on a.unit_idno = uh.Unit_IdNo where a.Cloth_Delivery_Code = '" & Trim(prn_DetDt.Rows(prn_DetIndx).Item("Cloth_Delivery_Code").ToString) & "'", con)
    '                dt = New DataTable
    '                da.Fill(dt)

    '                If dt.Rows.Count > 0 Then
    '                    Pty_DcNo = dt.Rows(0).Item("PARTY_DCNO").ToString
    '                    Clt_RcptDate = dt.Rows(0).Item("Cloth_Receipt_Date").ToString
    '                    Colour_Name = dt.Rows(0).Item("Colour_Name").ToString
    '                    unit_Name = dt.Rows(0).Item("unit_Name").ToString
    '                End If
    '                dt.Clear()

    '                Rate = 0
    '                If Trim(UCase(prn_DetDt.Rows(prn_DetIndx).Item("Rate_For").ToString)) = "PRINT" Then
    '                    Da2 = New SqlClient.SqlDataAdapter("select * from vendor_Closing_EndsCount_Stock_Details a where a.vendor_Stock_Closing_Code = '" & Trim(prn_DetDt.Rows(prn_DetIndx).Item("vendor_Stock_Closing_Code").ToString) & "' and a.Design_Inward_Code = '" & Trim(prn_DetDt.Rows(prn_DetIndx).Item("Design_Inward_Code").ToString) & "' and a.Print_Place_IdNo = " & Str(Val(prn_DetDt.Rows(prn_DetIndx).Item("Print_Place_IdNo").ToString)), con)
    '                Else
    '                    Da2 = New SqlClient.SqlDataAdapter("select * from vendor_Closing_EndsCount_Stock_Details a where a.vendor_Stock_Closing_Code = '" & Trim(prn_DetDt.Rows(prn_DetIndx).Item("vendor_Stock_Closing_Code").ToString) & "' and a.Design_Inward_Code = '" & Trim(prn_DetDt.Rows(prn_DetIndx).Item("Design_Inward_Code").ToString) & "'", con)
    '                End If
    '                Dt2 = New DataTable
    '                Da2.Fill(Dt2)
    '                If Dt2.Rows.Count > 0 Then
    '                    Rate = Dt2.Rows(0).Item("Rate").ToString

    '                End If
    '                Dt2.Clear()

    '                'If Val(prn_DetDt.Rows(prn_DetIndx).Item("Screen_Charge").ToString) <> 0 Then
    '                '    Rate = ""
    '                '    Amount = Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Screen_Charge").ToString), "##########.00")
    '                'Else
    '                '    Amount = Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Quantity").ToString) * Val(Rate), "##########.00")
    '                'End If


    '                prn_DetSNo = prn_DetSNo + 1
    '                ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Print_Place_Name").ToString)
    '                ItmNm2 = ""
    '                If Len(ItmNm1) > 30 Then
    '                    For I = 30 To 1 Step -1
    '                        If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
    '                    Next I
    '                    If I = 0 Then I = 30
    '                    ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
    '                    ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
    '                End If

    '                'ClrNm1 = Trim(Colour_Name)
    '                'ClrNm2 = ""
    '                'If Len(ClrNm1) > 8 Then
    '                '    For I = 8 To 1 Step -1
    '                '        If Mid$(Trim(ClrNm1), I, 1) = " " Or Mid$(Trim(ClrNm1), I, 1) = "," Or Mid$(Trim(ClrNm1), I, 1) = "." Or Mid$(Trim(ClrNm1), I, 1) = "(" Or Mid$(Trim(ClrNm1), I, 1) = ")" Or Mid$(Trim(ClrNm1), I, 1) = "-" Or Mid$(Trim(ClrNm1), I, 1) = "/" Or Mid$(Trim(ClrNm1), I, 1) = "_" Or Mid$(Trim(ClrNm1), I, 1) = "\" Or Mid$(Trim(ClrNm1), I, 1) = "[" Or Mid$(Trim(ClrNm1), I, 1) = "]" Or Mid$(Trim(ClrNm1), I, 1) = "{" Or Mid$(Trim(ClrNm1), I, 1) = "}" Then Exit For
    '                '    Next
    '                '    If I = 0 Then I = 8
    '                '    ClrNm2 = Microsoft.VisualBasic.Right(Trim(ClrNm1), Len(ClrNm1) - I)
    '                '    ClrNm1 = Microsoft.VisualBasic.Left(Trim(ClrNm1), I - 1)
    '                'End If


    '                vPdcNo1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Print_Place_Name").ToString)
    '                vPdcNo2 = ""
    '                If Len(vPdcNo1) > 20 Then
    '                    For I = 20 To 1 Step -1
    '                        If Mid$(Trim(vPdcNo1), I, 1) = " " Or Mid$(Trim(vPdcNo1), I, 1) = "," Or Mid$(Trim(vPdcNo1), I, 1) = "." Or Mid$(Trim(vPdcNo1), I, 1) = "(" Or Mid$(Trim(vPdcNo1), I, 1) = ")" Or Mid$(Trim(vPdcNo1), I, 1) = "-" Or Mid$(Trim(vPdcNo1), I, 1) = "/" Or Mid$(Trim(vPdcNo1), I, 1) = "_" Or Mid$(Trim(vPdcNo1), I, 1) = "\" Or Mid$(Trim(vPdcNo1), I, 1) = "[" Or Mid$(Trim(vPdcNo1), I, 1) = "]" Or Mid$(Trim(vPdcNo1), I, 1) = "{" Or Mid$(Trim(vPdcNo1), I, 1) = "}" Then Exit For
    '                    Next
    '                    If I = 0 Then I = 20
    '                    vPdcNo2 = Microsoft.VisualBasic.Right(Trim(vPdcNo1), Len(vPdcNo1) - I)
    '                    vPdcNo1 = Microsoft.VisualBasic.Left(Trim(vPdcNo1), I - 1)
    '                End If


    '                Common_Procedures.Print_To_PrintDocument(e, Trim(Val(SNo)), LMargin + 10, CurY, 0, 0, pFont)
    '                Common_Procedures.Print_To_PrintDocument(e, Trim(vPdcNo1), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
    '                'If Trim(Clt_RcptDate) <> "" Then
    '                '    Common_Procedures.Print_To_PrintDocument(e, "998821", LMargin + ClArr(1) + ClArr(2) + 5, CurY, 0, 0, pFont)
    '                'End If
    '                Common_Procedures.Print_To_PrintDocument(e, "998821", LMargin + ClArr(1) + ClArr(2) + 5, CurY, 0, 0, pFont)

    '                ' Common_Procedures.Print_To_PrintDocument(e, , LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 5, CurY, 0, 0, pFont)
    '                If Val(prn_HdDt.Rows(0).Item("CGST_percentage").ToString) <> 0 Then
    '                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("CGST_percentage").ToString) + Val(prn_HdDt.Rows(0).Item("SGST_Percentage").ToString) & " %", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)
    '                Else
    '                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("IGST_percentage").ToString) & " %", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)
    '                End If

    '                Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Quantity").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 15, CurY, 1, 0, pFont)
    '                ' If Val(prn_DetDt.Rows(prn_DetIndx).Item("Quantity").ToString) <> 0 Then
    '                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Rate_For").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 15, CurY, 1, 0, pFont)
    '                '
    '                ' End If
    '                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Rate").ToString), "##,##,##,##0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
    '                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Amount").ToString), "##,##,##,##0.00"), PageWidth - 10, CurY, 1, 0, pFont)

    '                NoofDets = NoofDets + 1
    '                If Trim(vPdcNo2) <> "" Then
    '                    CurY = CurY + TxtHgt - 5
    '                    Common_Procedures.Print_To_PrintDocument(e, Trim(vPdcNo2), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)

    '                    NoofDets = NoofDets + 1
    '                End If

    '                prn_DetIndx = prn_DetIndx + 1

    '            Loop

    '        End If

    '        Printing_Format2_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)

    '    End If

    '    'Catch ex As Exception

    '    '    MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

    '    'End Try

    '    e.HasMorePages = False

    'End Sub


    'Private Sub Printing_Format2_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
    '    Dim da2 As New SqlClient.SqlDataAdapter
    '    Dim dt2 As New DataTable
    '    Dim p1Font As Font
    '    Dim strHeight As Single = 0, strWidth As Single = 0
    '    Dim C1 As Single, W1 As Single, S1 As Single, C2 As Single
    '    Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
    '    Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_PanNo As String, Desc As String
    '    Dim Cmp_EMail As String
    '    Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
    '    Dim CurY1 As Single = 0, CurX As Single = 0
    '    Dim Y1 As Single = 0, Y2 As Single = 0
    '    Dim vInvNo As String, vInvSubNo As String

    '    Dim Blue_Clr As New Color
    '    Blue_Clr = Color.FromArgb(39, 65, 138)
    '    Dim BlueBrush As New SolidBrush(Blue_Clr)
    '    'Dim pens.black As New Pen(Blue_Clr)
    '    p1Font = New Font("Arial", 15, FontStyle.Bold)
    '    PageNo = PageNo + 1

    '    CurY = TMargin

    '    da2 = New SqlClient.SqlDataAdapter("select a.*,b.*,C.Order_No from vendor_Closing_Count_Stock_Details a INNER JOIN Vendor_Closing_Stock_Value_Head b ON a.vendor_Stock_Closing_Code = b.vendor_Stock_Closing_Code LEFT JOIN Cloth_Delivery_Details C ON b.vendor_Stock_Closing_Code = C.vendor_Stock_Closing_Code where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.vendor_Stock_Closing_Code = '" & Trim(EntryCode) & "' Order by a.sl_no", con)
    '    da2.Fill(dt2)

    '    If dt2.Rows.Count > NoofItems_PerPage Then
    '        Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
    '    End If
    '    dt2.Clear()
    '    Common_Procedures.Print_To_PrintDocument(e, "TAX INVOICE", LMargin, CurY, 2, PrintWidth, p1Font)

    '    e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '    LnAr(1) = CurY
    '    Desc = ""
    '    Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
    '    Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_PanNo = "" : Cmp_EMail = ""

    '    Desc = prn_HdDt.Rows(0).Item("Company_Description").ToString
    '    Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
    '    Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
    '    Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString

    '    If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
    '        Cmp_PhNo = "PHONE : " & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
    '    End If
    '    If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
    '        Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
    '    End If
    '    If Trim(prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString) <> "" Then
    '        Cmp_CstNo = "GST NO.: " & prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
    '    End If
    '    If Trim(prn_HdDt.Rows(0).Item("Company_PanNo").ToString) <> "" Then
    '        Cmp_PanNo = "PAN NO.: " & prn_HdDt.Rows(0).Item("Company_PanNo").ToString
    '    End If
    '    If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
    '        Cmp_EMail = "MAIL ID : " & prn_HdDt.Rows(0).Item("Company_EMail").ToString
    '    End If


    '    'p1Font = New Font("Times New Roman", 15, FontStyle.Bold)

    '    CurY = CurY + TxtHgt + 10
    '    strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height



    '    p1Font = New Font("Arial", 18, FontStyle.Bold)
    '    'p1Font = New Font("Times New Roman", 18, FontStyle.Bold)
    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
    '    strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


    '    CurY = CurY + strHeight - 1
    '    Common_Procedures.Print_To_PrintDocument(e, Desc, LMargin, CurY, 2, PrintWidth, pFont)

    '    CurY = CurY + TxtHgt - 1
    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

    '    CurY = CurY + TxtHgt - 1
    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
    '    CurY = CurY + TxtHgt - 1
    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
    '    CurY = CurY + TxtHgt - 1
    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_EMail, LMargin, CurY, 2, PrintWidth, pFont)
    '    CurY = CurY + TxtHgt - 1
    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, LMargin + 10, CurY, 0, 0, pFont)
    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_PanNo, PageWidth - 10, CurY, 1, 0, pFont)

    '    CurY = CurY + TxtHgt + 10
    '    e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '    LnAr(2) = CurY

    '    'Y1 = CurY + 2
    '    'Y2 = CurY + TxtHgt + TxtHgt - 13
    '    'Common_Procedures.FillRegionRectangle(e, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), Y1, PageWidth, Y2)

    '    CurY = CurY + 3
    '    p1Font = New Font("Calibri", 16, FontStyle.Bold)
    '    pFont = New Font("CALIBRI", 11, FontStyle.Regular)
    '    'Common_Procedures.Print_To_PrintDocument(e, "TAX INVOICE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 0, 0, p1Font, Brushes.White)
    '    p1Font = New Font("Calibri", 12, FontStyle.Bold)
    '    Common_Procedures.Print_To_PrintDocument(e, "FROM : ", LMargin + 10, CurY, 0, 0, p1Font)
    '    strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

    '    Try
    '        C1 = ClAr(1) + ClAr(2) + ClAr(3) + 50 '315
    '        C2 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4)
    '        'C2 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 50 '600

    '        W1 = e.Graphics.MeasureString("Invoice Date  : ", pFont).Width '80.33515
    '        S1 = e.Graphics.MeasureString("FROM : ", pFont).Width '55.9115944

    '        'Y1 = CurY - 2
    '        'Y2 = CurY + TxtHgt + TxtHgt - 13
    '        ' Common_Procedures.FillRegionRectangle(e, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), Y1, PageWidth, Y2)
    '        'CurY = CurY + 3

    '        'CurY = CurY + TxtHgt + 5
    '        'e.Graphics.DrawLine(Pens.Black, LMargin + C1 + ClAr(2) + ClAr(3), CurY, PageWidth, CurY)

    '        CurY = CurY + TxtHgt - 10
    '        p1Font = New Font("Calibri", 11, FontStyle.Bold)
    '        Common_Procedures.Print_To_PrintDocument(e, "M/s." & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + S1 + 10, CurY, 0, 0, p1Font)
    '        Common_Procedures.Print_To_PrintDocument(e, "Invoice No.", LMargin + C2 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY, 0, 0, pFont)
    '        'If Val(dt2.Rows(0).Item("chk_vendor_Stock_Closing_No").ToString) = True Then
    '        vInvNo = prn_HdDt.Rows(0).Item("vendor_Stock_Closing_No").ToString
    '        vInvSubNo = Replace(Trim(vInvNo), Trim(Val(vInvNo)), "")
    '        Common_Procedures.Print_To_PrintDocument(e, "PT-" & Format(Val(vInvNo), "#####000") & Trim(vInvSubNo), LMargin + C2 + W1 + 25, CurY, 0, 0, p1Font)
    '        'Else
    '        '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("vendor_Stock_Closing_No").ToString, LMargin + C2, CurY, 0, 0, p1Font)
    '        'End If

    '        CurY = CurY + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "Invoice Date", LMargin + C2 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("vendor_Stock_Closing_date").ToString), "dd-MM-yyyy").ToString, LMargin + C2 + W1 + 25, CurY, 0, 0, pFont)

    '        CurY = CurY + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "Order No.", LMargin + C2 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(0).Item("Order_No").ToString, LMargin + C2 + W1 + 25, CurY, 0, 0, pFont)

    '        CurY = CurY + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "Order RefNo.", LMargin + C2 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(0).Item("Order_RefNo").ToString, LMargin + C2 + W1 + 25, CurY, 0, 0, pFont)

    '        CurY = CurY + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
    '        'Common_Procedures.Print_To_PrintDocument(e, "BUYER", LMargin + C2 - 130, CurY, 0, 0, pFont)
    '        'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 - 90 + ClAr(2), CurY, 0, 0, pFont)
    '        'Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Buyer_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("Buyer_IdNo").ToString)), LMargin + C2, CurY, 0, 0, pFont)

    '        CurY = CurY + TxtHgt
    '        pFont = New Font("CALIBRI", 11, FontStyle.Bold)
    '        If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
    '            Common_Procedures.Print_To_PrintDocument(e, " GSTIN : ", LMargin + 10, CurY, 0, 0, p1Font)
    '            Common_Procedures.Print_To_PrintDocument(e, "" & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + S1 + 20, CurY, 0, 0, pFont)
    '        End If
    '        CurY = CurY + TxtHgt
    '        If Trim(prn_HdDt.Rows(0).Item("ledger_PanNo").ToString) <> "" Then
    '            Common_Procedures.Print_To_PrintDocument(e, " PAN : ", LMargin + 10, CurY, 0, 0, pFont)
    '            Common_Procedures.Print_To_PrintDocument(e, "" & prn_HdDt.Rows(0).Item("ledger_PanNo").ToString, LMargin + S1 + 20, CurY, 0, 0, pFont)
    '        End If
    '        'pFont = New Font("CALIBRI", 11, FontStyle.Regular)
    '        'Common_Procedures.Print_To_PrintDocument(e, "IO NO", LMargin + C2 - 130, CurY, 0, 0, pFont)
    '        'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 - 90 + ClAr(2), CurY, 0, 0, pFont)
    '        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Internal_OrderNo").ToString, LMargin + C2, CurY, 0, 0, pFont)

    '        CurY = CurY + TxtHgt + 5
    '        e.Graphics.DrawLine(pens.black, LMargin, CurY, PageWidth, CurY)
    '        LnAr(3) = CurY
    '        'e.Graphics.DrawLine(Pens.Black, LMargin + C1 + ClAr(1) + ClAr(2) + ClAr(3) - 30, LnAr(3), LMargin + C1 + ClAr(1) + ClAr(2) + ClAr(3) - 30, LnAr(2))
    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(2))


    '        'Y1 = CurY + 1
    '        'Y2 = CurY + TxtHgt + TxtHgt - 8
    '        'Common_Procedures.FillRegionRectangle(e, LMargin, Y1, PageWidth, Y2)

    '        CurY = CurY + TxtHgt - 12
    '        Common_Procedures.Print_To_PrintDocument(e, "S.NO", LMargin, CurY, 2, ClAr(1), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "PARTICULARS", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "HSN CODE", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "GST %", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "QTY", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "UNIT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "RATE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 40, CurY, 2, ClAr(8), pFont)


    '        CurY = CurY + TxtHgt + 5
    '        e.Graphics.DrawLine(pens.black, LMargin, CurY, PageWidth, CurY)
    '        LnAr(4) = CurY

    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))

    '    Catch ex As Exception

    '        MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

    '    End Try

    'End Sub

    'Private Sub Printing_Format2_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
    '    Dim p1Font As Font
    '    Dim I As Integer
    '    Dim Da1 As New SqlClient.SqlDataAdapter
    '    Dim Dt1 As New DataTable
    '    Dim da As New SqlClient.SqlDataAdapter
    '    Dim dt As New DataTable
    '    Dim Da2 As New SqlClient.SqlDataAdapter
    '    Dim Dt2 As New DataTable
    '    Dim Cmp_Name As String
    '    Dim W1 As Single = 0
    '    Dim BmsInWrds As String
    '    Dim BankDetailsArray() As String
    '    Dim BInc As Integer
    '    Dim BankName As String = ""
    '    Dim BranchName As String = ""
    '    Dim Ac_No As String = ""
    '    Dim ifsc_code As String = ""
    '    Dim C1 As Integer
    '    Dim C2 As Integer

    '    Dim dt3 As New DataTable
    '    Dim Da3 As New SqlClient.SqlDataAdapter

    '    Dim NewCode As String



    '    Dim Nr As Long = 0

    '    Dim Pty_DcNo As String = ""


    '    Dim Blue_Clr As New Color
    '    Blue_Clr = Color.FromArgb(39, 65, 138)
    '    Dim BlueBrush As New SolidBrush(Blue_Clr)
    '    '  Dim pens.black As New Pen(Blue_Clr)

    '    NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

    '    ' If dt.Rows.Count > 0 Then
    '    'Pty_DcNo = prn_Dt.Rows(0).Item("Party_Dcno").ToString
    '    'Dcno = prn_Dt.Rows(0).Item("Cloth_delivery_no").ToString
    '    'Dcdate = Format(Convert.ToDateTime(prn_Dt.Rows(0).Item("cloth_delivery_date").ToString), "dd-MM-yyyy".ToString)
    '    '' End If


    '    W1 = e.Graphics.MeasureString("No.of Beams : ", pFont).Width

    '    Try

    '        For I = NoofDets + 1 To NoofItems_PerPage

    '            CurY = CurY + TxtHgt

    '            prn_DetIndx = prn_DetIndx + 1

    '        Next

    '        CurY = CurY + TxtHgt
    '        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '        LnAr(5) = CurY
    '        p1Font = New Font("Calibri", 11, FontStyle.Bold)
    '        CurY = CurY + TxtHgt - 13
    '        If is_LastPage = True Then
    '            Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + 40, CurY, 2, ClAr(2), pFont)
    '        End If

    '        If Val(prn_HdDt.Rows(0).Item("Total_Quantity").ToString) <> 0 Then
    '            If is_LastPage = True Then
    '                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Quantity").ToString), "#############0"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
    '            End If
    '        End If
    '        If Val(prn_HdDt.Rows(0).Item("Total_Amount").ToString) <> 0 Then
    '            If is_LastPage = True Then
    '                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Amount").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
    '            End If
    '        End If

    '        CurY = CurY + TxtHgt - 15

    '        CurY = CurY + TxtHgt
    '        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '        LnAr(6) = CurY

    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(4))
    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(4))
    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(4))
    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(4))
    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(4))
    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))
    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(4))


    '        '-----*--BANK DETAILS----*---

    '        Erase BankDetailsArray
    '        If Trim(prn_HdDt.Rows(0).Item("Company_Bank_Ac_Details").ToString) <> "" Then
    '            BankDetailsArray = Split(Trim(prn_HdDt.Rows(0).Item("Company_Bank_Ac_Details").ToString), ",")

    '            BInc = -1

    '            BInc = BInc + 1
    '            If UBound(BankDetailsArray) >= BInc Then
    '                BankName = Trim(BankDetailsArray(BInc))
    '            End If

    '            BInc = BInc + 1
    '            If UBound(BankDetailsArray) >= BInc Then
    '                BranchName = Trim(BankDetailsArray(BInc))
    '            End If

    '            BInc = BInc + 1
    '            If UBound(BankDetailsArray) >= BInc Then
    '                Ac_No = Trim(BankDetailsArray(BInc))
    '            End If

    '            BInc = BInc + 1
    '            If UBound(BankDetailsArray) >= BInc Then
    '                ifsc_code = Trim(BankDetailsArray(BInc))
    '            End If

    '        End If

    '        C1 = ClAr(1) + ClAr(2) + 20

    '        p1Font = New Font("Calibri", 11, FontStyle.Bold Or FontStyle.Underline)
    '        CurY = CurY + TxtHgt - 10
    '        Common_Procedures.Print_To_PrintDocument(e, "BANK DETAILS", LMargin + 10, CurY, 0, 0, p1Font)

    '        If (Val(prn_HdDt.Rows(0).Item("Total_Film_Charge").ToString) + Val(prn_HdDt.Rows(0).Item("Total_Screen_Charge").ToString)) <> 0 Then
    '            Common_Procedures.Print_To_PrintDocument(e, "Screen Charge", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 0, 0, pFont)
    '            Common_Procedures.Print_To_PrintDocument(e, Trim(Format(Val(prn_HdDt.Rows(0).Item("Total_Film_Charge").ToString) + Val(prn_HdDt.Rows(0).Item("Total_Screen_Charge").ToString), "##########0.00")), PageWidth - 10, CurY, 1, 0, pFont)
    '        End If

    '        'If Val(prn_HdDt.Rows(0).Item("Total_Screen_Charge").ToString) <> 0 Then
    '        '    If Val(prn_HdDt.Rows(0).Item("Flim_as_Screen_Charge").ToString) <> 1 And Val(prn_HdDt.Rows(0).Item("Total_Screen_Charge").ToString) <> 0 Then
    '        '        Common_Procedures.Print_To_PrintDocument(e, "Screen Charge", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 0, 0, pFont)
    '        '        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Total_Screen_Charge").ToString), PageWidth - 10, CurY, 1, 0, pFont)
    '        '    End If
    '        'End If

    '        '----BANK NAME
    '        CurY = CurY + TxtHgt
    '        If BankName <> "" Then
    '            Common_Procedures.Print_To_PrintDocument(e, Trim(BankName), LMargin + 10, CurY, 0, 0, pFont)
    '            'Common_Procedures.Print_To_PrintDocument(e, "BANK NAME ", LMargin + 10, CurY, 0, 0, pFont)
    '            'Common_Procedures.Print_To_PrintDocument(e, " : ", LMargin + C1, CurY, 0, 0, pFont)
    '            'Common_Procedures.Print_To_PrintDocument(e, Trim(BankName), LMargin + C1 + 15, CurY, 0, 0, pFont)
    '        End If
    '        'If Val(prn_HdDt.Rows(0).Item("Total_Film_Charge").ToString) <> 0 Then
    '        '    If Val(prn_HdDt.Rows(0).Item("Flim_as_Screen_Charge").ToString) = 1 Then
    '        '        Common_Procedures.Print_To_PrintDocument(e, "Screen Charge", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 0, 0, pFont)
    '        '        'Else
    '        '        '    Common_Procedures.Print_To_PrintDocument(e, "Flim Charge", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 0, 0, pFont)
    '        '        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Total_Film_Charge").ToString), PageWidth - 10, CurY, 1, 0, pFont)
    '        '    End If
    '        'End If

    '        CurY = CurY + TxtHgt
    '        '----BRANCH NAME
    '        If BranchName <> "" Then
    '            Common_Procedures.Print_To_PrintDocument(e, Trim(BranchName), LMargin + 10, CurY, 0, 0, pFont)
    '            'Common_Procedures.Print_To_PrintDocument(e, "BRANCH NAME ", LMargin + 10, CurY, 0, 0, pFont)
    '            'Common_Procedures.Print_To_PrintDocument(e, " : ", LMargin + C1, CurY, 0, 0, pFont)
    '            'Common_Procedures.Print_To_PrintDocument(e, Trim(BranchName), LMargin + C1 + 15, CurY, 0, 0, pFont)
    '        End If
    '        'If Val(prn_HdDt.Rows(0).Item("Total_Screen_Alteration_Charge").ToString) <> 0 Then
    '        '    Common_Procedures.Print_To_PrintDocument(e, "Screen Alteration Charge", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 0, 0, pFont)
    '        '    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Total_Screen_Alteration_Charge").ToString), PageWidth - 10, CurY, 1, 0, pFont)
    '        'End If
    '        If Val(prn_HdDt.Rows(0).Item("Total_Taxable_Amount").ToString) <> 0 Then
    '            Common_Procedures.Print_To_PrintDocument(e, "Total Before Tax", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 0, 0, pFont)
    '            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Total_Taxable_Amount").ToString), PageWidth - 10, CurY, 1, 0, pFont)
    '        End If

    '        '----ACCOUNT NO
    '        CurY = CurY + TxtHgt
    '        If Ac_No <> "" Then
    '            Common_Procedures.Print_To_PrintDocument(e, Trim(Ac_No), LMargin + 10, CurY, 0, 0, pFont)
    '            'Common_Procedures.Print_To_PrintDocument(e, "ACCOUNT NO ", LMargin + 10, CurY, 0, 0, pFont)
    '            'Common_Procedures.Print_To_PrintDocument(e, " : ", LMargin + C1, CurY, 0, 0, pFont)
    '            'Common_Procedures.Print_To_PrintDocument(e, Trim(Ac_No), LMargin + C1 + 15, CurY, 0, 0, pFont)
    '        End If
    '        'If Val(prn_HdDt.Rows(0).Item("Total_Film_Alteration_Charge").ToString) <> 0 Then
    '        '    Common_Procedures.Print_To_PrintDocument(e, "Film Alteration Charge", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 0, 0, pFont)
    '        '    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Total_Film_Alteration_Charge").ToString), PageWidth - 10, CurY, 1, 0, pFont)
    '        'End If
    '        If Val(prn_HdDt.Rows(0).Item("CGST_Percentage").ToString) <> 0 Then
    '            Common_Procedures.Print_To_PrintDocument(e, "CGST " & Trim(prn_HdDt.Rows(0).Item("CGST_Percentage").ToString) & "%", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 0, 0, pFont)
    '            Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("CGST_Amount").ToString), PageWidth - 10, CurY, 1, 0, pFont)
    '        End If

    '        CurY = CurY + TxtHgt
    '        '----IFSC CODE
    '        If ifsc_code <> "" Then
    '            Common_Procedures.Print_To_PrintDocument(e, Trim(ifsc_code), LMargin + 10, CurY, 0, 0, pFont)
    '            'Common_Procedures.Print_To_PrintDocument(e, "IFSC CODE ", LMargin + 10, CurY, 0, 0, pFont)
    '            'Common_Procedures.Print_To_PrintDocument(e, " : ", LMargin + C1, CurY, 0, 0, pFont)
    '            'Common_Procedures.Print_To_PrintDocument(e, Trim(ifsc_code), LMargin + C1 + 15, CurY, 0, 0, pFont)
    '        End If
    '        If Val(prn_HdDt.Rows(0).Item("SGST_Percentage").ToString) <> 0 Then
    '            Common_Procedures.Print_To_PrintDocument(e, "SGST " & Trim(prn_HdDt.Rows(0).Item("SGST_Percentage").ToString) & "%", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 0, 0, pFont)
    '            Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("SGST_Amount").ToString), PageWidth - 10, CurY, 1, 0, pFont)
    '        End If

    '        CurY = CurY + TxtHgt
    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, PageWidth, CurY)

    '        p1Font = New Font("Calibri", 11, FontStyle.Bold)
    '        If Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString) <> 0 Then
    '            CurY = CurY + TxtHgt - 15
    '            Common_Procedures.Print_To_PrintDocument(e, "Net Amount", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 0, 0, p1Font)
    '            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Net_Amount").ToString), PageWidth - 10, CurY, 1, 0, p1Font)
    '        End If

    '        p1Font = New Font("Calibri", 12, FontStyle.Bold)
    '        CurY = CurY + TxtHgt + 3
    '        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '        LnAr(9) = CurY
    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))
    '        CurY = CurY - 10

    '        BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))
    '        BmsInWrds = Replace(Trim((BmsInWrds)), "", "")
    '        StrConv(BmsInWrds, vbProperCase)
    '        CurY = CurY + 15
    '        Common_Procedures.Print_To_PrintDocument(e, "Amount in Words :", LMargin + 10, CurY, 0, 0, p1Font)
    '        Common_Procedures.Print_To_PrintDocument(e, " Rupees " & BmsInWrds & " ", LMargin + 150, CurY, 0, 0, p1Font)
    '        CurY = CurY + TxtHgt + 5


    '        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)



    '        Dim vOurDcNo As String = ""
    '        Dim vOurDcDate As String = ""

    '        Da3 = New SqlClient.SqlDataAdapter("select b.cloth_delivery_no, b.cloth_delivery_date from Vendor_Closing_Stock_Value_Head a, Cloth_Delivery_Head b where a.vendor_Stock_Closing_Code =  '" & Trim(NewCode) & "' and a.vendor_Stock_Closing_Code = b.vendor_Stock_Closing_Code Order by b.cloth_delivery_date, b.for_orderby, b.cloth_delivery_no", con)
    '        Dt3 = New DataTable
    '        da3.Fill(Dt3)
    '        If Dt3.Rows.Count > 0 Then
    '            For I = 0 To Dt3.Rows.Count - 1
    '                vOurDcNo = Trim(vOurDcNo) & IIf(Trim(vOurDcNo) <> "", ", ", "") & Trim(Dt3.Rows(I).Item("Cloth_Delivery_No").ToString)
    '                vOurDcDate = Trim(vOurDcDate) & IIf(Trim(vOurDcDate) <> "", ", ", "") & Format(Convert.ToDateTime(Dt3.Rows(I).Item("Cloth_Delivery_Date").ToString), "dd-MM-yyyy").ToString
    '            Next I
    '        End If
    '        Dt3.Clear()



    '        Dim vPDcNo As String = ""
    '        Dim vPDcDate As String = ""
    '        Dim vDupRecCode As String = ""

    '        Da3 = New SqlClient.SqlDataAdapter("select c.Cloth_Receipt_Code, c.PARTY_DCNO, c.Cloth_Receipt_Date from Vendor_Closing_Stock_Value_Head a, Cloth_Delivery_Details b, Cloth_Receipt_Head c where a.vendor_Stock_Closing_Code =  '" & Trim(NewCode) & "' and a.vendor_Stock_Closing_Code = b.vendor_Stock_Closing_Code and b.Cloth_Receipt_Code = c.Cloth_Receipt_Code Order by c.Cloth_Receipt_Date, c.for_orderby, c.Cloth_Receipt_No", con)
    '        Dt3 = New DataTable
    '        da3.Fill(Dt3)
    '        If Dt3.Rows.Count > 0 Then
    '            For I = 0 To dt3.Rows.Count - 1

    '                If InStr(1, Trim(UCase(vDupRecCode)), "~" & Trim(UCase(dt3.Rows(I).Item("Cloth_Receipt_Code").ToString)) & "~") = 0 Then
    '                    vPDcNo = Trim(vPDcNo) & IIf(Trim(vPDcNo) <> "", ", ", "") & Trim(dt3.Rows(I).Item("PARTY_DCNO").ToString)
    '                    vPDcDate = Trim(vPDcDate) & IIf(Trim(vPDcDate) <> "", ", ", "") & Format(Convert.ToDateTime(dt3.Rows(I).Item("Cloth_Receipt_Date").ToString), "dd-MM-yyyy").ToString

    '                End If

    '                vDupRecCode = vDupRecCode & "~" & Trim(dt3.Rows(I).Item("Cloth_Receipt_Code").ToString) & "~"

    '            Next I
    '        End If
    '        Dt3.Clear()


    '        CurY = CurY + 3
    '        p1Font = New Font("Calibri", 11, FontStyle.Bold)
    '        If (vOurDcNo) <> "" Then
    '            Common_Procedures.Print_To_PrintDocument(e, "OUR DC.NO : ", LMargin + 10, CurY, 0, 0, p1Font)
    '            ' CurY = CurY + TxtHgt
    '            Common_Procedures.Print_To_PrintDocument(e, vOurDcNo, LMargin + ClAr(1) + ClAr(2) - 100, CurY, 0, 0, pFont)
    '        End If


    '        If (vOurDcDate) <> "" Then
    '            CurY = CurY + TxtHgt
    '            Common_Procedures.Print_To_PrintDocument(e, "OUR DC.DATE : ", LMargin + 10, CurY, 0, 0, p1Font)

    '            Common_Procedures.Print_To_PrintDocument(e, vOurDcDate, LMargin + ClAr(1) + ClAr(2) - 100, CurY, 0, 0, pFont)
    '        End If

    '        If (vPDcNo) <> "" Then
    '            CurY = CurY + TxtHgt
    '            Common_Procedures.Print_To_PrintDocument(e, "PARTY DC.NO : ", LMargin + 10, CurY, 0, 0, p1Font)

    '            Common_Procedures.Print_To_PrintDocument(e, vPDcNo, LMargin + ClAr(1) + ClAr(2) - 100, CurY, 0, 0, pFont)
    '        End If

    '        If (vPDcDate) <> "" Then
    '            CurY = CurY + TxtHgt
    '            Common_Procedures.Print_To_PrintDocument(e, "PARTY DC.DATE : ", LMargin + 10, CurY, 0, 0, p1Font)

    '            Common_Procedures.Print_To_PrintDocument(e, vPDcDate, LMargin + ClAr(1) + ClAr(2) - 100, CurY, 0, 0, pFont)
    '        End If


    '        CurY = CurY + TxtHgt + 5
    '        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

    '        CurY = CurY + TxtHgt - 10
    '        If Val(Common_Procedures.User.IdNo) <> 1 Then
    '            Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + ClAr(3) + ClAr(4), CurY, 0, 0, pFont)
    '        End If
    '        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
    '        p1Font = New Font("Calibri", 12, FontStyle.Bold)
    '        Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)


    '        CurY = CurY + TxtHgt
    '        CurY = CurY + TxtHgt
    '        CurY = CurY + TxtHgt

    '        Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 20, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + ClAr(3) + ClAr(4) + ClAr(5) + 10, CurY, 0, 0, pFont)
    '        'Common_Procedures.Print_To_PrintDocument(e, "Checked By ", LMargin + 350, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "Authorised Signature ", PageWidth - 15, CurY, 1, 0, pFont)

    '        CurY = CurY + TxtHgt + 5

    '        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
    '        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

    '    Catch ex As Exception

    '        MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

    '    End Try

    'End Sub


    Private Sub Total_CountCalculation()
        Dim Sno As Integer
        Dim TotQty As Single

        If FrmLdSTS = True Then Exit Sub
        If NoCalc_Status = True Then Exit Sub

        Sno = 0
        TotQty = 0

        With dgv_Count_details
            For i = 0 To .RowCount - 1
                Sno = Sno + 1
                .Rows(i).Cells(0).Value = Sno
                If Val(.Rows(i).Cells(2).Value) <> 0 Then

                    TotQty = TotQty + Val(.Rows(i).Cells(2).Value())

                End If

            Next i

        End With


        With dgv_CountDetails_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(2).Value = Format(Val(TotQty), "########0.00")
        End With

    End Sub

    Private Sub Total_ClothCalculation()
        Dim Sno As Integer
        Dim tot_clthmtr As Single



        If NoCalc_Status = True Then Exit Sub

        Sno = 0
        tot_clthmtr = 0

        With dgv_Cloth_Details
            For i = 0 To dgv_Cloth_Details.RowCount - 1
                Sno = Sno + 1
                dgv_Cloth_Details.Rows(i).Cells(0).Value = Sno
                If Val(dgv_Cloth_Details.Rows(i).Cells(2).Value) <> 0 Then
                    tot_clthmtr = tot_clthmtr + Val(dgv_Cloth_Details.Rows(i).Cells(2).Value())


                End If

            Next i

        End With


        With dgv_Cloth_Details_Total
            If .RowCount = 0 Then .Rows.Add()

            .Rows(0).Cells(2).Value = Format(Val(tot_clthmtr), "########0.00")

        End With

    End Sub

    Private Sub Total_EndsCountCalculation()
        Dim Sno As Integer
        Dim TotQty As Single
        Dim TotAmt As Single


        If NoCalc_Status = True Then Exit Sub

        Sno = 0
        TotQty = 0 : TotAmt = 0

        With dgv_EndsCount_Details
            For i = 0 To .RowCount - 1
                Sno = Sno + 1
                .Rows(i).Cells(0).Value = Sno
                If Val(.Rows(i).Cells(2).Value) <> 0 Then

                    TotQty = TotQty + Val(.Rows(i).Cells(2).Value())
                    ' TotAmt = TotAmt + Val(.Rows(i).Cells(5).Value())

                End If

            Next i

        End With


        With dgv_EndsCount_Details_Total
            If .RowCount = 0 Then .Rows.Add()

            .Rows(0).Cells(2).Value = Format(Val(TotQty), "########0.00")
            '.Rows(0).Cells(5).Value = Format(Val(TotAmt), "########0.00")
        End With

    End Sub

    'Private Sub NetAmount_Calculation()
    '    ' Dim Sno As Integer
    '    Dim GrsAmt As Single, ScrBilAmt As Single, NetAmt As Single
    '    Dim FmChge As Single, FmAlCge As Single, ScnChge As Single, ScnAChge As Single
    '    Dim TaxAmt As Single = 0
    '    If NoCalc_Status = True Then Exit Sub


    '    GrsAmt = 0 : ScrBilAmt = 0 : NetAmt = 0 : FmChge = 0 : FmAlCge = 0 : ScnAChge = 0 : ScnChge = 0

    '    '   lbl_Amount.Text = Format(Val(lbl_BillQty.Text) * Val(lbl_BillRate.Text), "#########0.00")

    '    GrsAmt = 0
    '    If Val(lbl_Amount.Text) <> 0 Then
    '        GrsAmt = Val(lbl_Amount.Text)

    '    Else
    '        With dgv_EndsCount_Details_Total
    '            If .Rows.Count > 0 Then
    '                GrsAmt = Val(.Rows(0).Cells(5).Value)
    '            End If
    '        End With

    '    End If

    '    With dgv_Cloth_Details_Total
    '        If .Rows.Count > 0 Then
    '            GrsAmt = Val(GrsAmt) + Val(.Rows(0).Cells(3).Value) + Val(.Rows(0).Cells(4).Value) + Val(.Rows(0).Cells(5).Value) + Val(.Rows(0).Cells(6).Value)
    '        End If
    '    End With

    '    lbl_Taxable_Value.Text = Format(Val(GrsAmt), "###########0.00")
    '    If Trim(UCase(cbo_TaxType.Text)) = "GST" Then

    '        lbl_CGST_Amount.Text = Format(Val(lbl_Taxable_Value.Text) * Val(txt_CGST_Percentage.Text) / 100, "##########0.00")
    '        lbl_SGST_Amount.Text = Format(Val(lbl_Taxable_Value.Text) * Val(txt_SGST_Percentage.Text) / 100, "###########0.00")
    '        TaxAmt = Val(lbl_Taxable_Value.Text) + Val(lbl_CGST_Amount.Text) + Val(lbl_SGST_Amount.Text)
    '    Else
    '        lbl_CGST_Amount.Text = ""
    '        lbl_SGST_Amount.Text = ""
    '        ' TaxAmt = ""
    '    End If
    '    With dgv_Cloth_Details_Total
    '        If .Rows.Count > 0 Then
    '            FmChge = Val(.Rows(0).Cells(3).Value)
    '            ScnChge = Val(.Rows(0).Cells(4).Value)
    '            FmAlCge = Val(.Rows(0).Cells(5).Value)
    '            ScnAChge = Val(.Rows(0).Cells(6).Value)
    '        End If
    '    End With
    '    ScrBilAmt = Val(FmChge) + Val(ScnChge) + Val(FmAlCge) + Val(ScnAChge)
    '    NetAmt = TaxAmt '+ ScrBilAmt + Val(lbl_VatAmount.Text)
    '    lbl_NetAmount.Text = Format(NetAmt, "############0")
    '    lbl_NetAmount.Text = Format(Val(lbl_NetAmount.Text), "############0.00")

    'End Sub

    Private Sub btn_save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub btn_Filter_Close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Filter_Close.Click
        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        Filter_Status = False
    End Sub

    Private Sub btn_Filter_Show_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Filter_Show.Click

        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim n As Integer
        Dim Led_IdNo As Integer, Prnt_IdNo As Integer
        Dim Condt As String = ""

        Try

            Condt = ""
            Led_IdNo = 0
            Prnt_IdNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.vendor_Stock_Closing_date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.vendor_Stock_Closing_date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.vendor_Stock_Closing_date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If



            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If


            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " (a.Weaver_idno = " & Str(Val(Led_IdNo)) & ")"
            End If




            da = New SqlClient.SqlDataAdapter("select a.* ,c.Ledger_Name from Vendor_Closing_Stock_Value_Head a  INNER join Ledger_head c on a.Weaver_idno = c.Ledger_idno where a.company_idno =" & Str(Val(lbl_Company.Tag)) & "and a.vendor_Stock_Closing_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.vendor_Stock_Closing_date, a.for_orderby, a.vendor_Stock_Closing_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = dt2.Rows(i).Item("vendor_Stock_Closing_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(1).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("vendor_Stock_Closing_date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(3).Value = Format(Val(dt2.Rows(i).Item("Total_CountWeight").ToString), "########0.00")
                    dgv_Filter_Details.Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("Total_EndsCount_meters").ToString), "########0.00")
                    dgv_Filter_Details.Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("Total_Cloth_meters").ToString), "########0.00")

                Next i

            End If

            dt2.Clear()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FILTER...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            dt2.Dispose()
            da.Dispose()

            If dgv_Filter_Details.Visible And dgv_Filter_Details.Enabled Then dgv_Filter_Details.Focus()

        End Try

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





    Private Sub cbo_Filter_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'WEAVER' or Ledger_Type = 'JOBWORKER'  or Show_In_All_Entry = 1) and Close_status = 0 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, Nothing, btn_save, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'WEAVER' or Ledger_Type = 'JOBWORKER'  or Show_In_All_Entry = 1) and Close_status = 0 )", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, btn_save, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'WEAVER' or Ledger_Type = 'JOBWORKER'  or Show_In_All_Entry = 1) and Close_status = 0 )", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_WeaverName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_WeaverName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'WEAVER' or Ledger_Type = 'JOBWORKER'  or Show_In_All_Entry = 1) and Close_status = 0 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_WeaverName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_WeaverName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_WeaverName, dtp_date, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'WEAVER' or Ledger_Type = 'JOBWORKER'  or Show_In_All_Entry = 1) and Close_status = 0 )", "(Ledger_IdNo = 0)")
        If (e.KeyValue = 40 And cbo_WeaverName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If dgv_Count_details.Rows.Count > 0 Then
                dgv_Count_details.Focus()
                dgv_Count_details.CurrentCell = dgv_Count_details.Rows(0).Cells(1)
            Else
            End If

        End If
    End Sub

    Private Sub cbo_WeaverName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_WeaverName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_WeaverName, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'WEAVER' or Ledger_Type = 'JOBWORKER'  or Show_In_All_Entry = 1) and Close_status = 0 )", "(Ledger_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            dgv_Count_details.Focus()
            dgv_Count_details.CurrentCell = dgv_Count_details.Rows(0).Cells(1)
        End If
    End Sub

    Private Sub cbo_WeaverName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_WeaverName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Common_Procedures.MDI_LedType = "WEAVER"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_WeaverName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub dgv_Count_Details_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Count_details.CellEndEdit
        dgv_Count_Details_CellLeave(sender, e)
    End Sub

    Private Sub dgv_Count_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Count_details.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Rect As Rectangle
        Dim LedID As Integer = 0
        Dim CloID As Integer = 0
        With dgv_Count_details

            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If

            If e.ColumnIndex = 1 Then

                If Cbo_Grid_CountName.Visible = False Or Val(Cbo_Grid_CountName.Tag) <> e.RowIndex Then

                    Cbo_Grid_CountName.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Count_Name from Count_Head order by Count_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    Cbo_Grid_CountName.DataSource = Dt1
                    Cbo_Grid_CountName.DisplayMember = "Count_Name"

                    Rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    Cbo_Grid_CountName.Left = .Left + Rect.Left
                    Cbo_Grid_CountName.Top = .Top + Rect.Top

                    Cbo_Grid_CountName.Width = Rect.Width
                    Cbo_Grid_CountName.Height = Rect.Height
                    Cbo_Grid_CountName.Text = .CurrentCell.Value

                    Cbo_Grid_CountName.Tag = Val(e.RowIndex)
                    Cbo_Grid_CountName.Visible = True

                    Cbo_Grid_CountName.BringToFront()
                    Cbo_Grid_CountName.Focus()



                End If

            Else
                Cbo_Grid_CountName.Visible = False

            End If

        End With
    End Sub

    Private Sub dgv_Count_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Count_details.CellLeave
        On Error Resume Next
        If FrmLdSTS = True Then Exit Sub
        With dgv_Count_details
            If IsNothing(.CurrentCell) Then Exit Sub
            If .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                Else
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
                End If
            End If
        End With
    End Sub

    Private Sub dgv_Count_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Count_details.CellValueChanged
        On Error Resume Next
        If FrmLdSTS = True Then Exit Sub
        With dgv_Count_details
            If .Visible Then
                If IsNothing(.CurrentCell) Then Exit Sub
                If .CurrentCell.ColumnIndex = 2 Then

                    'If Common_Procedures.settings.Receipt_Delivery_InMeters = 0 Then
                    ' .CurrentRow.Cells(5).Value = Val(.CurrentRow.Cells(3).Value) * Val(.CurrentRow.Cells(4).Value)
                    'End If

                    Total_CountCalculation()
                End If

            End If
        End With

    End Sub

    Private Sub dgv_Count_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Count_details.EditingControlShowing
        dgtxt_CountDetails = Nothing
        ' If dgv_YarnDetails.CurrentCell.ColumnIndex > 2 Then
        dgtxt_CountDetails = CType(dgv_Count_details.EditingControl, DataGridViewTextBoxEditingControl)
        ' End If
    End Sub

    Private Sub dgtxt_CountDetails_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_CountDetails.Enter
        dgv_ActiveCtrl_Name = dgv_Count_details.Name
        'dgv_Count_details.EditingControl.BackColor = Color.Lime
        'dgv_Count_details.EditingControl.ForeColor = Color.Blue
        dgv_Count_details.SelectAll()
    End Sub

    Private Sub dgtxt_CountDetails_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_CountDetails.KeyDown
        With dgv_Count_details

            If e.KeyValue = Keys.Delete Then
                If Val(.Rows(.CurrentCell.RowIndex).Cells(2).Value) <> 0 Then
                    e.Handled = True
                End If
            End If
        End With

    End Sub

    Private Sub dgtxt_CountDetails_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_CountDetails.KeyPress

        With dgv_Count_details
            If .Visible Then

                If .CurrentCell.ColumnIndex = 2 Then

                    If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                        e.Handled = True
                    End If

                End If

            End If
        End With

    End Sub

    Private Sub dgtxt_CountDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_CountDetails.KeyUp

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            dgv_Count_details_keyup(sender, e)
        End If

    End Sub

    Private Sub dgv_Count_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Count_details.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
    End Sub

    Private Sub dgv_Count_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Count_details.KeyUp
        Dim n As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            With dgv_Count_details

                n = .CurrentRow.Index

                If n = .Rows.Count - 1 Then
                    For i = 0 To .ColumnCount - 1
                        .Rows(n).Cells(i).Value = ""
                    Next

                Else
                    .Rows.RemoveAt(n)

                End If

                Total_CountCalculation()

            End With

        End If

    End Sub

    Private Sub dgv_Count_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Count_details.LostFocus
        On Error Resume Next
        If IsNothing(dgv_Count_details.CurrentCell) Then Exit Sub
        dgv_Count_details.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_Count_Details_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Count_details.RowsAdded
        Dim n As Integer
        If FrmLdSTS = True Then Exit Sub
        With dgv_Count_details
            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)
        End With
    End Sub






    Private Sub msk_date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_date.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        vmskOldText = ""
        vmskSelStrt = -1
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            vmskOldText = msk_date.Text
            vmskSelStrt = msk_date.SelectionStart
        End If

    End Sub


    Private Sub msk_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_date.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            msk_date.Text = Date.Today
        End If
        If IsDate(msk_date.Text) = True Then
            If e.KeyCode = 107 Then
                msk_date.Text = DateAdd("D", 1, Convert.ToDateTime(msk_date.Text))
            ElseIf e.KeyCode = 109 Then
                msk_date.Text = DateAdd("D", -1, Convert.ToDateTime(msk_date.Text))
            End If
        End If
        If e.KeyCode = 46 Or e.KeyCode = 8 Then

            Common_Procedures.maskEdit_Date_ON_DelBackSpace(sender, e, vmskOldText, vmskSelStrt)
        End If
    End Sub
    Private Sub dtp_Date_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtp_date.ValueChanged
        msk_date.Text = dtp_date.Text
    End Sub

    Private Sub dtp_Date_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_date.Enter
        msk_date.Focus()
        msk_date.SelectionStart = 0
    End Sub




    Private Sub dgv_EndsCount_Details_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_EndsCount_Details.CellEndEdit
        dgv_EndsCount_Details_CellLeave(sender, e)
    End Sub
    Private Sub dgv_EndsCount_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_EndsCount_Details.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Da3 As New SqlClient.SqlDataAdapter
        Dim Dt3 As New DataTable
        Dim Dt4 As New DataTable
        Dim Dt5 As New DataTable
        Dim Rect As Rectangle

        With dgv_EndsCount_Details

            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If


            If e.ColumnIndex = 1 Then

                If Cbo_Grid_EndsCountName.Visible = False Or Val(Cbo_Grid_EndsCountName.Tag) <> e.RowIndex Then

                    Cbo_Grid_EndsCountName.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select EndsCount_Name from EndsCount_Head order by EndsCount_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    Cbo_Grid_EndsCountName.DataSource = Dt1
                    Cbo_Grid_EndsCountName.DisplayMember = "EndsCount_Name"

                    Rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    Cbo_Grid_EndsCountName.Left = .Left + Rect.Left
                    Cbo_Grid_EndsCountName.Top = .Top + Rect.Top

                    Cbo_Grid_EndsCountName.Width = Rect.Width
                    Cbo_Grid_EndsCountName.Height = Rect.Height
                    Cbo_Grid_EndsCountName.Text = .CurrentCell.Value

                    Cbo_Grid_EndsCountName.Tag = Val(e.RowIndex)
                    Cbo_Grid_EndsCountName.Visible = True

                    Cbo_Grid_EndsCountName.BringToFront()
                    Cbo_Grid_EndsCountName.Focus()



                End If

            Else
                Cbo_Grid_EndsCountName.Visible = False

            End If

        End With

    End Sub

    Private Sub dgv_EndsCount_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_EndsCount_Details.CellLeave
        On Error Resume Next
        If FrmLdSTS = True Then Exit Sub
        With dgv_EndsCount_Details
            If .Visible Then
                If IsNothing(.CurrentCell) Then Exit Sub
                If .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Then
                    If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then


                        .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")


                    Else
                        .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
                    End If
                End If
            End If
        End With
    End Sub

    Private Sub dgv_EndsCount_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_EndsCount_Details.CellValueChanged
        On Error Resume Next

        With dgv_EndsCount_Details
            If .Visible Then
                If IsNothing(.CurrentCell) Then Exit Sub
                If .CurrentCell.ColumnIndex = 2 Then


                    ' .CurrentRow.Cells(5).Value = Val(.CurrentRow.Cells(3).Value) * Val(.CurrentRow.Cells(4).Value)


                    Total_EndsCountCalculation()
                    ' NetAmount_Calculation()
                End If

            End If
        End With

    End Sub

    Private Sub dgv_QuantityDetails_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_EndsCount_Details.EditingControlShowing
        dgtxt_EndsCountDetails = Nothing
        ' If dgv_YarnDetails.CurrentCell.ColumnIndex > 2 Then
        dgtxt_EndsCountDetails = CType(dgv_EndsCount_Details.EditingControl, DataGridViewTextBoxEditingControl)
        ' End If
    End Sub

    Private Sub dgtxt_EndsCountDetails_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_EndsCountDetails.Enter
        dgv_ActiveCtrl_Name = dgv_EndsCount_Details.Name
        dgv_EndsCount_Details.EditingControl.BackColor = Color.Lime
        dgv_EndsCount_Details.EditingControl.ForeColor = Color.Blue
        dgv_EndsCount_Details.SelectAll()
    End Sub

    Private Sub dgtxt_EndsCountDetails_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_EndsCountDetails.KeyDown
        With dgv_Count_details

            If e.KeyValue = Keys.Delete Then
                If Val(.Rows(.CurrentCell.RowIndex).Cells(2).Value) <> 0 Then
                    e.Handled = True
                End If
            End If
        End With

    End Sub

    Private Sub dgtxt_EndsCountDetails_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_EndsCountDetails.KeyPress

        With dgv_EndsCount_Details
            If .Visible Then

                If .CurrentCell.ColumnIndex = 2 Then

                    If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                        e.Handled = True
                    End If

                End If

            End If
        End With

    End Sub

    Private Sub dgtxt_EndsCountDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_EndsCountDetails.KeyUp

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            dgv_EndsCount_Details_KeyUp(sender, e)
        End If

    End Sub

    Private Sub dgv_EndsCount_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_EndsCount_Details.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
    End Sub

    Private Sub dgv_EndsCount_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_EndsCount_Details.KeyUp
        Dim n As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            With dgv_EndsCount_Details

                n = .CurrentRow.Index

                If n = .Rows.Count - 1 Then
                    For i = 0 To .ColumnCount - 1
                        .Rows(n).Cells(i).Value = ""
                    Next

                Else
                    .Rows.RemoveAt(n)

                End If

                Total_EndsCountCalculation()

            End With

        End If

    End Sub

    Private Sub dgv_EndsCount_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_EndsCount_Details.LostFocus
        On Error Resume Next
        If IsNothing(dgv_EndsCount_Details.CurrentCell) Then Exit Sub
        dgv_EndsCount_Details.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_EndsCount_Details_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_EndsCount_Details.RowsAdded
        Dim n As Integer

        With dgv_EndsCount_Details
            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)
        End With
    End Sub
    'Private Sub Selection()


    '    Dim JobNo As String = ""
    '    Dim SLt As String = "", OrNo As String = ""
    '    Dim i As Integer

    '    If Trim(cbo_SampleLot.Text) <> "" Or Trim(txt_OrderNo.Text) <> "" Or Trim(cbo_JobNo.Text) <> "" Then

    '        JobNo = Trim(cbo_JobNo.Text)
    '        SLt = Trim(cbo_SampleLot.Text)
    '        OrNo = Trim(txt_OrderNo.Text)

    '        For i = 0 To dgv_Selection.Rows.Count - 1
    '            If Trim(UCase(SLt)) = Trim(UCase(dgv_Selection.Rows(i).Cells(6).Value)) And Trim(OrNo) = Trim(dgv_Selection.Rows(i).Cells(4).Value) And Trim(JobNo) = Trim(dgv_Selection.Rows(i).Cells(12).Value) Then
    '                Call Select_Piece(i)

    '                dgv_Selection.CurrentCell = dgv_Selection.Rows(i).Cells(0)
    '                If i >= 10 Then dgv_Selection.FirstDisplayedScrollingRowIndex = i - 9

    '                Exit For

    '            End If
    '        Next


    '        'If txt_BaleNoSelection.Enabled = True Then txt_BaleNoSelection.Focus()
    '    End If

    '    dgv_Selection.Focus()
    '    txt_OrderNo.Text = ""
    '    cbo_JobNo.Text = ""
    '    cbo_SampleLot.Text = ""
    'End Sub

    'Private Sub btn_Close_JobSelection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_JobSelection.Click
    '    Dim Da As New SqlClient.SqlDataAdapter
    '    Dim Dt1 As New DataTable
    '    Dim Dt2 As New DataTable
    '    Dim n As Integer = 0
    '    Dim sno As Integer = 0
    '    Dim i As Integer = 0
    '    Dim j As Integer = 0
    '    Dim NewCode As String = ""
    '    dgv_Cloth_Details.Rows.Clear()
    '    sno = 0
    '    For i = 0 To dgv_JobSelection.RowCount - 1

    '        If Val(dgv_JobSelection.Rows(i).Cells(7).Value) = 1 Then
    '            sno = sno + 1
    '            dgv_Cloth_Details.Rows(n).Cells(0).Value = Val(sno)
    '            dgv_Cloth_Details.Rows(n).Cells(1).Value = dgv_JobSelection.Rows(i).Cells(1).Value
    '            dgv_Cloth_Details.Rows(n).Cells(2).Value = dgv_JobSelection.Rows(i).Cells(2).Value
    '            dgv_Cloth_Details.Rows(n).Cells(3).Value = dgv_JobSelection.Rows(i).Cells(3).Value
    '            dgv_Cloth_Details.Rows(n).Cells(4).Value = dgv_JobSelection.Rows(i).Cells(4).Value
    '            dgv_Cloth_Details.Rows(n).Cells(5).Value = dgv_JobSelection.Rows(i).Cells(5).Value
    '            dgv_Cloth_Details.Rows(n).Cells(6).Value = dgv_JobSelection.Rows(i).Cells(6).Value
    '            dgv_Cloth_Details.Rows(n).Cells(7).Value = dgv_JobSelection.Rows(i).Cells(8).Value



    '            Total_ClothCalculation()



    '        End If

    '    Next
    '    ' NetAmount_Calculation()

    '    pnl_Back.Enabled = True
    '    pnl_JobSelection.Visible = False
    '    'If txt_CGST_Percentage.Visible And txt_CGST_Percentage.Enabled Then txt_CGST_Percentage.Focus()
    'End Sub
    'Private Sub dgv_JobSelection_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_JobSelection.CellClick
    '    Select_Piece1(e.RowIndex)
    'End Sub

    'Private Sub Select_Piece1(ByVal RwIndx As Integer)
    '    Dim i As Integer

    '    With dgv_JobSelection

    '        If .RowCount > 0 And RwIndx >= 0 Then

    '            .Rows(RwIndx).Cells(7).Value = (Val(.Rows(RwIndx).Cells(7).Value) + 1) Mod 2

    '            If Val(.Rows(RwIndx).Cells(7).Value) = 1 Then

    '                For i = 0 To .ColumnCount - 1
    '                    .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
    '                Next

    '            Else
    '                .Rows(RwIndx).Cells(7).Value = ""

    '                For i = 0 To .ColumnCount - 1
    '                    .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Black
    '                Next

    '            End If

    '        End If

    '    End With

    'End Sub

    'Private Sub dgv_JobSelection_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_JobSelection.KeyDown
    '    Dim n As Integer

    '    On Error Resume Next

    '    If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Space Then
    '        If dgv_JobSelection.CurrentCell.RowIndex >= 0 Then

    '            n = dgv_JobSelection.CurrentCell.RowIndex

    '            Select_Piece1(n)

    '            e.Handled = True

    '        End If
    '    End If
    'End Sub

    'Private Sub txt_VatPerc_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    '    '  NetAmount_Calculation()
    'End Sub


    'Private Sub txt_Payment_Terms_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
    '    If e.KeyValue = 38 Then
    '        '  txt_SGST_Percentage.Focus()
    '    End If
    '    If e.KeyValue = 40 Then
    '        If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
    '            save_record()
    '        Else
    '            msk_date.Focus()
    '        End If
    '    End If
    'End Sub


    'Private Sub txt_Payment_Terms_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
    '    If Asc(e.KeyChar) = 13 Then

    '        If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
    '            save_record()
    '        Else
    '            msk_date.Focus()
    '        End If
    '    End If
    'End Sub


    'Private Sub txt_SGST_Percentage_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    '    ' NetAmount_Calculation()
    'End Sub

    'Private Sub txt_CGST_Percentage_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    '    ' NetAmount_Calculation()
    'End Sub
    'Private Sub cbo_TaxType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
    '    Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")
    '    ' cbo_TaxType.Tag = cbo_TaxType.Text
    'End Sub

    ''Private Sub cbo_TaxType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
    ''    Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_TaxType, txt_Order_RefNo, Nothing, "", "", "", "")
    ''    If (e.KeyValue = 40 And cbo_TaxType.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
    ''        If dgv_EndsCount_Details.Rows.Count > 0 Then
    ''            dgv_EndsCount_Details.Focus()
    ''            dgv_EndsCount_Details.CurrentCell = dgv_EndsCount_Details.Rows(0).Cells(4)
    ''        Else
    ''            cbo_Selection_JobNO.Focus()
    ''        End If
    ''    End If
    ''End Sub

    ''Private Sub cbo_TaxType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
    ''    Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_TaxType, Nothing, "", "", "", "")
    ''    If Asc(e.KeyChar) = 13 Then
    ''        If dgv_EndsCount_Details.Rows.Count > 0 Then
    ''            dgv_EndsCount_Details.Focus()
    ''            dgv_EndsCount_Details.CurrentCell = dgv_EndsCount_Details.Rows(0).Cells(4)
    ''        Else
    ''            cbo_Selection_JobNO.Focus()



    ''        End If
    ''    End If
    ''End Sub

    ''Private Sub cbo_TaxType_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
    ''    If Trim(UCase(cbo_TaxType.Tag)) <> Trim(UCase(cbo_TaxType.Text)) Then
    ''        cbo_TaxType.Tag = cbo_TaxType.Text
    ''        NetAmount_Calculation()
    ''    End If
    ''End Sub

    ''Private Sub cbo_TaxType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    ''    NetAmount_Calculation()
    ''End Sub
    ' '''-------print_tech

    'Private Sub Printing_Format3(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
    '    Dim cmd As New SqlClient.SqlCommand

    '    Dim Da1 As New SqlClient.SqlDataAdapter
    '    Dim Dt1 As New DataTable
    '    Dim da As New SqlClient.SqlDataAdapter
    '    Dim dt As New DataTable
    '    Dim Da2 As New SqlClient.SqlDataAdapter
    '    Dim Dt2 As New DataTable

    '    Dim EntryCode As String
    '    Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
    '    Dim pFont As Font
    '    Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
    '    Dim PrintWidth As Single, PrintHeight As Single
    '    Dim PageWidth As Single, PageHeight As Single
    '    Dim CurY As Single, TxtHgt As Single
    '    Dim LnAr(15) As Single, ClArr(15) As Single
    '    Dim ItmNm1 As String, ItmNm2 As String
    '    Dim ClrNm1 As String, ClrNm2 As String
    '    Dim ps As Printing.PaperSize
    '    Dim strHeight As Single = 0
    '    Dim PpSzSTS As Boolean = False
    '    Dim W1 As Single = 0
    '    Dim SNo As Integer = 0
    '    Dim Pty_DcNo As String = ""
    '    Dim Clt_RcptDate As String = ""
    '    Dim Colour_Name As String = ""
    '    Dim Rate As String = 0, Amount As String = 0
    '    Dim vPdcNo1 As String = ""
    '    Dim vPdcNo2 As String = ""




    '    For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
    '        If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
    '            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
    '            PrintDocument1.DefaultPageSettings.PaperSize = ps
    '            e.PageSettings.PaperSize = ps
    '            Exit For
    '        End If
    '    Next

    '    With PrintDocument1.DefaultPageSettings.Margins
    '        .Left = 20
    '        .Right = 50
    '        .Top = 20
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

    '    NoofItems_PerPage = 20 ' 6

    '    Erase LnAr
    '    Erase ClArr

    '    LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
    '    ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

    '    ClArr(1) = 37 : ClArr(2) = 100 : ClArr(3) = 85 : ClArr(4) = 220 : ClArr(5) = 70 : ClArr(6) = 90 : ClArr(7) = 60
    '    ClArr(8) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6))

    '    TxtHgt = 20 '19.8 ' e.Graphics.MeasureString("A", pFont).Height ' 20

    '    EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

    '    Try

    '        If prn_HdDt.Rows.Count > 0 Then

    '            Printing_Format3_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

    '            ' W1 = e.Graphics.MeasureString("No.of Beams : ", pFont).Width

    '            NoofDets = 0

    '            CurY = CurY - 10

    '            If prn_DetDt.Rows.Count > 0 Then

    '                CurY = CurY + TxtHgt - 5
    '                Common_Procedures.Print_To_PrintDocument(e, "HSN /SAC CODE : 998821", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)

    '                Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

    '                    If NoofDets >= NoofItems_PerPage Then
    '                        CurY = CurY + TxtHgt

    '                        Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

    '                        NoofDets = NoofDets + 1

    '                        Printing_Format3_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

    '                        e.HasMorePages = True
    '                        Return

    '                    End If


    '                    CurY = CurY + TxtHgt
    '                    SNo = SNo + 1

    '                    Pty_DcNo = ""
    '                    Clt_RcptDate = ""
    '                    Colour_Name = ""
    '                    da = New SqlClient.SqlDataAdapter("select b.*,b.PARTY_DCNO, b.Cloth_Receipt_Date ,CLH.Colour_Name from Cloth_Delivery_Details a INNER JOIN Cloth_Receipt_Head b ON a.Cloth_Receipt_Code = b.Cloth_Receipt_Code LEFT JOIN Colour_Head CLH ON A.COLOUR_IDNO = CLH.Colour_IdNo where a.Cloth_Delivery_Code = '" & Trim(prn_DetDt.Rows(prn_DetIndx).Item("Cloth_Delivery_Code").ToString) & "'", con)
    '                    dt = New DataTable
    '                    da.Fill(dt)

    '                    If dt.Rows.Count > 0 Then
    '                        Pty_DcNo = dt.Rows(0).Item("PARTY_DCNO").ToString
    '                        Clt_RcptDate = dt.Rows(0).Item("Cloth_Receipt_Date").ToString
    '                        Colour_Name = dt.Rows(0).Item("Colour_Name").ToString
    '                    End If
    '                    dt.Clear()

    '                    Rate = 0
    '                    If Trim(UCase(prn_DetDt.Rows(prn_DetIndx).Item("Rate_For").ToString)) = "PRINT" Then
    '                        Da2 = New SqlClient.SqlDataAdapter("select * from vendor_Closing_EndsCount_Stock_Details a where a.vendor_Stock_Closing_Code = '" & Trim(prn_DetDt.Rows(prn_DetIndx).Item("vendor_Stock_Closing_Code").ToString) & "' and a.Design_Inward_Code = '" & Trim(prn_DetDt.Rows(prn_DetIndx).Item("Design_Inward_Code").ToString) & "' and a.Print_Place_IdNo = " & Str(Val(prn_DetDt.Rows(prn_DetIndx).Item("Print_Place_IdNo").ToString)), con)
    '                    Else
    '                        Da2 = New SqlClient.SqlDataAdapter("select * from vendor_Closing_EndsCount_Stock_Details a where a.vendor_Stock_Closing_Code = '" & Trim(prn_DetDt.Rows(prn_DetIndx).Item("vendor_Stock_Closing_Code").ToString) & "' and a.Design_Inward_Code = '" & Trim(prn_DetDt.Rows(prn_DetIndx).Item("Design_Inward_Code").ToString) & "'", con)
    '                    End If
    '                    Dt2 = New DataTable
    '                    Da2.Fill(Dt2)
    '                    If Dt2.Rows.Count > 0 Then
    '                        Rate = Dt2.Rows(0).Item("Rate").ToString

    '                    End If
    '                    Dt2.Clear()

    '                    If Val(prn_DetDt.Rows(prn_DetIndx).Item("Screen_Charge").ToString) <> 0 Then
    '                        Rate = ""
    '                        Amount = Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Screen_Charge").ToString), "##########.00")
    '                    Else
    '                        Amount = Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Quantity").ToString) * Val(Rate), "##########.00")
    '                    End If


    '                    prn_DetSNo = prn_DetSNo + 1
    '                    ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Print_Place_Name").ToString) & " " & Trim(prn_DetDt.Rows(prn_DetIndx).Item("Particulars").ToString)
    '                    ItmNm2 = ""
    '                    If Len(ItmNm1) > 30 Then
    '                        For I = 30 To 1 Step -1
    '                            If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
    '                        Next I
    '                        If I = 0 Then I = 30
    '                        ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
    '                        ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
    '                    End If

    '                    ClrNm1 = Trim(Colour_Name)
    '                    ClrNm2 = ""
    '                    If Len(ClrNm1) > 8 Then
    '                        For I = 8 To 1 Step -1
    '                            If Mid$(Trim(ClrNm1), I, 1) = " " Or Mid$(Trim(ClrNm1), I, 1) = "," Or Mid$(Trim(ClrNm1), I, 1) = "." Or Mid$(Trim(ClrNm1), I, 1) = "(" Or Mid$(Trim(ClrNm1), I, 1) = ")" Or Mid$(Trim(ClrNm1), I, 1) = "-" Or Mid$(Trim(ClrNm1), I, 1) = "/" Or Mid$(Trim(ClrNm1), I, 1) = "_" Or Mid$(Trim(ClrNm1), I, 1) = "\" Or Mid$(Trim(ClrNm1), I, 1) = "[" Or Mid$(Trim(ClrNm1), I, 1) = "]" Or Mid$(Trim(ClrNm1), I, 1) = "{" Or Mid$(Trim(ClrNm1), I, 1) = "}" Then Exit For
    '                        Next
    '                        If I = 0 Then I = 8
    '                        ClrNm2 = Microsoft.VisualBasic.Right(Trim(ClrNm1), Len(ClrNm1) - I)
    '                        ClrNm1 = Microsoft.VisualBasic.Left(Trim(ClrNm1), I - 1)
    '                    End If


    '                    vPdcNo1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("PARTY_DCNO").ToString)
    '                    vPdcNo2 = ""
    '                    If Len(vPdcNo1) > 10 Then
    '                        For I = 10 To 1 Step -1
    '                            If Mid$(Trim(vPdcNo1), I, 1) = " " Or Mid$(Trim(vPdcNo1), I, 1) = "," Or Mid$(Trim(vPdcNo1), I, 1) = "." Or Mid$(Trim(vPdcNo1), I, 1) = "(" Or Mid$(Trim(vPdcNo1), I, 1) = ")" Or Mid$(Trim(vPdcNo1), I, 1) = "-" Or Mid$(Trim(vPdcNo1), I, 1) = "/" Or Mid$(Trim(vPdcNo1), I, 1) = "_" Or Mid$(Trim(vPdcNo1), I, 1) = "\" Or Mid$(Trim(vPdcNo1), I, 1) = "[" Or Mid$(Trim(vPdcNo1), I, 1) = "]" Or Mid$(Trim(vPdcNo1), I, 1) = "{" Or Mid$(Trim(vPdcNo1), I, 1) = "}" Then Exit For
    '                        Next
    '                        If I = 0 Then I = 10
    '                        vPdcNo2 = Microsoft.VisualBasic.Right(Trim(vPdcNo1), Len(vPdcNo1) - I)
    '                        vPdcNo1 = Microsoft.VisualBasic.Left(Trim(vPdcNo1), I - 1)
    '                    End If


    '                    Common_Procedures.Print_To_PrintDocument(e, Trim(Val(SNo)), LMargin + 10, CurY, 0, 0, pFont)
    '                    Common_Procedures.Print_To_PrintDocument(e, Trim(vPdcNo1), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
    '                    If Trim(Clt_RcptDate) <> "" Then
    '                        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(Clt_RcptDate), "dd-MM-yyyy").ToString, LMargin + ClArr(1) + ClArr(2) + 5, CurY, 0, 0, pFont)
    '                    End If

    '                    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 5, CurY, 0, 0, pFont)
    '                    Common_Procedures.Print_To_PrintDocument(e, Trim(ClrNm1), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 3, CurY, 0, 0, pFont)
    '                    If Val(prn_DetDt.Rows(prn_DetIndx).Item("Quantity").ToString) <> 0 Then
    '                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Quantity").ToString), "##,##,##,##0"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 15, CurY, 1, 0, pFont)
    '                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(Rate), "##,##,##,##0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
    '                    End If
    '                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(Amount), "##,##,##,##0.00"), PageWidth - 10, CurY, 1, 0, pFont)

    '                    NoofDets = NoofDets + 1
    '                    If Trim(vPdcNo2) <> "" Or Trim(ItmNm2) <> "" Or Trim(ClrNm2) <> "" Then
    '                        CurY = CurY + TxtHgt - 5
    '                        Common_Procedures.Print_To_PrintDocument(e, Trim(vPdcNo2), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
    '                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 5, CurY, 0, 0, pFont)
    '                        Common_Procedures.Print_To_PrintDocument(e, Trim(ClrNm2), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 3, CurY, 0, 0, pFont)
    '                        NoofDets = NoofDets + 1
    '                    End If

    '                    prn_DetIndx = prn_DetIndx + 1

    '                Loop

    '            End If

    '            Printing_Format3_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)

    '        End If

    '    Catch ex As Exception

    '        MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

    '    End Try

    '    e.HasMorePages = False

    'End Sub


    'Private Sub Printing_Format3_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
    '    Dim da2 As New SqlClient.SqlDataAdapter
    '    Dim dt2 As New DataTable
    '    Dim p1Font As Font
    '    Dim strHeight As Single = 0, strWidth As Single = 0
    '    Dim C1 As Single, W1 As Single, S1 As Single, C2 As Single
    '    Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
    '    Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_PanNo As String
    '    Dim Cmp_EMail As String
    '    Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
    '    Dim CurY1 As Single = 0, CurX As Single = 0
    '    Dim Y1 As Single = 0, Y2 As Single = 0
    '    Dim vInvNo As String, vInvSubNo As String

    '    Dim Blue_Clr As New Color
    '    Blue_Clr = Color.FromArgb(39, 65, 138)
    '    Dim BlueBrush As New SolidBrush(Blue_Clr)
    '    Dim BluePen As New Pen(Blue_Clr)

    '    PageNo = PageNo + 1

    '    CurY = TMargin

    '    da2 = New SqlClient.SqlDataAdapter("select a.*,b.*,C.Order_No from vendor_Closing_Count_Stock_Details a INNER JOIN Vendor_Closing_Stock_Value_Head b ON a.vendor_Stock_Closing_Code = b.vendor_Stock_Closing_Code LEFT JOIN Cloth_Delivery_Details C ON b.vendor_Stock_Closing_Code = C.vendor_Stock_Closing_Code where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.vendor_Stock_Closing_Code = '" & Trim(EntryCode) & "' Order by a.sl_no", con)
    '    da2.Fill(dt2)

    '    If dt2.Rows.Count > NoofItems_PerPage Then
    '        Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont, BlueBrush)
    '    End If
    '    dt2.Clear()

    '    e.Graphics.DrawLine(BluePen, LMargin, CurY, PageWidth, CurY)
    '    LnAr(1) = CurY


    '    Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
    '    Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_EMail = ""
    '    Cmp_StateCap = "" : Cmp_StateNm = "" : Cmp_StateCode = "" : Cmp_GSTIN_Cap = "" : Cmp_GSTIN_No = "" : Cmp_PanNo = ""
    '    Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
    '    Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
    '    Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString

    '    If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
    '        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1175" Then
    '            Cmp_PhNo = prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
    '        Else
    '            Cmp_PhNo = "PHONE NO.: " & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
    '        End If
    '    End If
    '    If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
    '        Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
    '    End If
    '    If Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString) <> "" Then
    '        Cmp_CstNo = "CST NO.: " & prn_HdDt.Rows(0).Item("Company_CstNo").ToString
    '    End If
    '    If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
    '        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1175" Then
    '            Cmp_EMail = prn_HdDt.Rows(0).Item("Company_EMail").ToString
    '        Else
    '            Cmp_EMail = "Email : " & prn_HdDt.Rows(0).Item("Company_EMail").ToString
    '        End If
    '    End If
    '    If Trim(prn_HdDt.Rows(0).Item("Company_State_Name").ToString) <> "" Then
    '        Cmp_StateCap = "STATE : "
    '        Cmp_StateNm = prn_HdDt.Rows(0).Item("Company_State_Name").ToString
    '    End If
    '    If Trim(prn_HdDt.Rows(0).Item("Company_State_Code").ToString) <> "" Then
    '        Cmp_StateCode = "CODE : " & prn_HdDt.Rows(0).Item("Company_State_Code").ToString
    '    End If
    '    If Trim(prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString) <> "" Then
    '        Cmp_GSTIN_No = "GSTIN : " & prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
    '    End If
    '    If Trim(prn_HdDt.Rows(0).Item("Company_PanNo").ToString) <> "" Then
    '        Cmp_PanNo = "PAN NO : " & prn_HdDt.Rows(0).Item("Company_PanNo").ToString
    '    End If

    '    CurY = CurY + TxtHgt - 10
    '    p1Font = New Font("Calibri", 18, FontStyle.Bold)



    '    pFont = New Font("CALIBRI", 8, FontStyle.Regular)
    '    CurY = CurY + strHeight
    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, PageWidth - 10, CurY, 1, PrintWidth, pFont, BlueBrush)

    '    CurY = CurY + TxtHgt - 5
    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, PageWidth - 10, CurY, 1, PrintWidth, pFont, BlueBrush)

    '    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1175" Then
    '        CurY = CurY + TxtHgt - 5
    '        Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_EMail), PageWidth - 10, CurY, 1, PrintWidth, pFont, BlueBrush)
    '    End If

    '    'p1Font = New Font("Calibri", 8, FontStyle.Bold)
    '    'strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_GSTIN_Cap), p1Font).Width
    '    'strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_StateNm & " " & Cmp_GSTIN_Cap & Cmp_GSTIN_No), pFont).Width

    '    'If PrintWidth > strWidth Then
    '    '    CurX = LMargin + (PrintWidth - strWidth) / 2
    '    'Else
    '    '    CurX = LMargin
    '    'End If

    '    'p1Font = New Font("Calibri", 8, FontStyle.Bold)
    '    'Common_Procedures.Print_To_PrintDocument(e, Cmp_StateCap, PageWidth - C1 - 100, CurY, 1, 0, p1Font, BlueBrush)
    '    'strWidth = e.Graphics.MeasureString(Cmp_StateCap, p1Font).Width
    '    'CurX = CurX + strWidth
    '    'Common_Procedures.Print_To_PrintDocument(e, Cmp_StateNm, PageWidth - 10, CurY, 1, 0, pFont, BlueBrush)
    '    'strWidth = e.Graphics.MeasureString(Cmp_StateNm, pFont).Width

    '    p1Font = New Font("Calibri", 8, FontStyle.Bold)
    '    'p1Font = New Font("Calibri", 11, FontStyle.Bold)
    '    CurY = CurY + TxtHgt - 5
    '    'Common_Procedures.Print_To_PrintDocument(e, " " & Cmp_GSTIN_Cap, PageWidth - C1, CurY, 1, PrintWidth, p1Font, BlueBrush)
    '    'strWidth = e.Graphics.MeasureString(" " & Cmp_GSTIN_Cap, p1Font).Width
    '    'CurX = CurX + strWidth
    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, PageWidth - 10, CurY, 1, PrintWidth, pFont, BlueBrush)
    '    CurY = CurY + TxtHgt - 5
    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_PanNo, PageWidth - 10, CurY, 1, PrintWidth, pFont, BlueBrush)
    '    CurY = CurY + TxtHgt - 5
    '    Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PhNo), PageWidth - 10, CurY, 1, PrintWidth, pFont, BlueBrush)
    '    'CurY = CurY + TxtHgt
    '    'Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PhNo & " " & Cmp_EMail), PageWidth - 10, CurY, 1, PrintWidth, pFont, BlueBrush)


    '    CurY = CurY + 20
    '    e.Graphics.DrawLine(BluePen, LMargin, CurY, PageWidth, CurY)
    '    LnAr(2) = CurY

    '    Y1 = CurY + 2
    '    Y2 = CurY + TxtHgt + TxtHgt - 13
    '    Common_Procedures.FillRegionRectangle(e, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), Y1, PageWidth, Y2)

    '    CurY = CurY + 3
    '    p1Font = New Font("Calibri", 16, FontStyle.Bold)
    '    pFont = New Font("CALIBRI", 11, FontStyle.Regular)
    '    'Common_Procedures.Print_To_PrintDocument(e, "TAX INVOICE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 0, 0, p1Font, Brushes.White)
    '    p1Font = New Font("Calibri", 12, FontStyle.Bold)
    '    Common_Procedures.Print_To_PrintDocument(e, "FROM : ", LMargin + 10, CurY, 0, 0, p1Font, BlueBrush)
    '    strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

    '    Try
    '        C1 = ClAr(1) + ClAr(2) + ClAr(3) + 50 '315
    '        C2 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4)
    '        'C2 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 50 '600

    '        W1 = e.Graphics.MeasureString("Invoice Date  : ", pFont).Width '80.33515
    '        S1 = e.Graphics.MeasureString("FROM : ", pFont).Width '55.9115944

    '        Y1 = CurY - 2
    '        Y2 = CurY + TxtHgt + TxtHgt - 13
    '        Common_Procedures.FillRegionRectangle(e, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), Y1, PageWidth, Y2)
    '        CurY = CurY + 3
    '        Common_Procedures.Print_To_PrintDocument(e, "TAX INVOICE", LMargin + ClAr(1) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 0, 0, p1Font, Brushes.White)

    '        CurY = CurY + TxtHgt + 5
    '        e.Graphics.DrawLine(BluePen, LMargin + C1 + ClAr(2) + ClAr(3) - 5, CurY, PageWidth, CurY)

    '        CurY = CurY + TxtHgt - 10
    '        p1Font = New Font("Calibri", 11, FontStyle.Bold)
    '        Common_Procedures.Print_To_PrintDocument(e, "M/s." & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + S1 + 10, CurY, 0, 0, p1Font, BlueBrush)
    '        Common_Procedures.Print_To_PrintDocument(e, "Invoice No.", LMargin + C2 + 10, CurY, 0, 0, pFont, BlueBrush)
    '        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY, 0, 0, pFont, BlueBrush)
    '        'If Val(dt2.Rows(0).Item("chk_vendor_Stock_Closing_No").ToString) = True Then
    '        vInvNo = prn_HdDt.Rows(0).Item("vendor_Stock_Closing_No").ToString
    '        vInvSubNo = Replace(Trim(vInvNo), Trim(Val(vInvNo)), "")
    '        Common_Procedures.Print_To_PrintDocument(e, "PT-" & Format(Val(vInvNo), "#####000") & Trim(vInvSubNo), LMargin + C2 + W1 + 25, CurY, 0, 0, p1Font)
    '        'Else
    '        '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("vendor_Stock_Closing_No").ToString, LMargin + C2, CurY, 0, 0, p1Font)
    '        'End If

    '        CurY = CurY + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "Invoice Date", LMargin + C2 + 10, CurY, 0, 0, pFont, BlueBrush)
    '        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY, 0, 0, pFont, BlueBrush)
    '        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("vendor_Stock_Closing_date").ToString), "dd-MM-yyyy").ToString, LMargin + C2 + W1 + 25, CurY, 0, 0, pFont)

    '        CurY = CurY + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "Order No.", LMargin + C2 + 10, CurY, 0, 0, pFont, BlueBrush)
    '        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY, 0, 0, pFont, BlueBrush)
    '        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(0).Item("Order_No").ToString, LMargin + C2 + W1 + 25, CurY, 0, 0, pFont)

    '        CurY = CurY + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "Order RefNo.", LMargin + C2 + 10, CurY, 0, 0, pFont, BlueBrush)
    '        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY, 0, 0, pFont, BlueBrush)
    '        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(0).Item("Order_RefNo").ToString, LMargin + C2 + W1 + 25, CurY, 0, 0, pFont)

    '        CurY = CurY + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
    '        'Common_Procedures.Print_To_PrintDocument(e, "BUYER", LMargin + C2 - 130, CurY, 0, 0, pFont, BlueBrush)
    '        'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 - 90 + ClAr(2), CurY, 0, 0, pFont, BlueBrush)
    '        'Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Buyer_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("Buyer_IdNo").ToString)), LMargin + C2, CurY, 0, 0, pFont)

    '        CurY = CurY + TxtHgt
    '        pFont = New Font("CALIBRI", 11, FontStyle.Bold)
    '        If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
    '            Common_Procedures.Print_To_PrintDocument(e, " GSTIN : ", LMargin + 10, CurY, 0, 0, p1Font, BlueBrush)
    '            Common_Procedures.Print_To_PrintDocument(e, "" & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + S1 + 20, CurY, 0, 0, pFont)
    '        End If
    '        If Trim(prn_HdDt.Rows(0).Item("ledger_PanNo").ToString) <> "" Then
    '            Common_Procedures.Print_To_PrintDocument(e, " PAN : ", LMargin + C1, CurY, 0, 0, pFont, BlueBrush)
    '            Common_Procedures.Print_To_PrintDocument(e, "" & prn_HdDt.Rows(0).Item("ledger_PanNo").ToString, LMargin + C1 + 45, CurY, 0, 0, pFont)
    '        End If
    '        'pFont = New Font("CALIBRI", 11, FontStyle.Regular)
    '        'Common_Procedures.Print_To_PrintDocument(e, "IO NO", LMargin + C2 - 130, CurY, 0, 0, pFont, BlueBrush)
    '        'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 - 90 + ClAr(2), CurY, 0, 0, pFont, BlueBrush)
    '        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Internal_OrderNo").ToString, LMargin + C2, CurY, 0, 0, pFont)

    '        CurY = CurY + TxtHgt + 5
    '        e.Graphics.DrawLine(BluePen, LMargin, CurY, PageWidth, CurY)
    '        LnAr(3) = CurY
    '        'e.Graphics.DrawLine(Pens.Black, LMargin + C1 + ClAr(1) + ClAr(2) + ClAr(3) - 30, LnAr(3), LMargin + C1 + ClAr(1) + ClAr(2) + ClAr(3) - 30, LnAr(2))
    '        e.Graphics.DrawLine(BluePen, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(2))


    '        Y1 = CurY + 1
    '        Y2 = CurY + TxtHgt + TxtHgt - 8
    '        Common_Procedures.FillRegionRectangle(e, LMargin, Y1, PageWidth, Y2)

    '        CurY = CurY + TxtHgt - 12
    '        Common_Procedures.Print_To_PrintDocument(e, "S.NO", LMargin, CurY, 2, ClAr(1), pFont, Brushes.White)
    '        Common_Procedures.Print_To_PrintDocument(e, "PDC.NO", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont, Brushes.White)
    '        Common_Procedures.Print_To_PrintDocument(e, "PDC DATE", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont, Brushes.White)
    '        Common_Procedures.Print_To_PrintDocument(e, "PRINT DESIGN", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont, Brushes.White)
    '        Common_Procedures.Print_To_PrintDocument(e, "COLOUR", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont, Brushes.White)
    '        Common_Procedures.Print_To_PrintDocument(e, "QUANTITY", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont, Brushes.White)
    '        Common_Procedures.Print_To_PrintDocument(e, "RATE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont, Brushes.White)
    '        Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 40, CurY, 2, ClAr(8), pFont, Brushes.White)


    '        CurY = CurY + TxtHgt + 5
    '        e.Graphics.DrawLine(BluePen, LMargin, CurY, PageWidth, CurY)
    '        LnAr(4) = CurY

    '        e.Graphics.DrawLine(Pens.White, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
    '        e.Graphics.DrawLine(Pens.White, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
    '        e.Graphics.DrawLine(Pens.White, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
    '        e.Graphics.DrawLine(Pens.White, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
    '        e.Graphics.DrawLine(Pens.White, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
    '        e.Graphics.DrawLine(Pens.White, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
    '        e.Graphics.DrawLine(Pens.White, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))

    '    Catch ex As Exception

    '        MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

    '    End Try

    'End Sub

    'Private Sub Printing_Format3_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
    '    Dim p1Font As Font
    '    Dim I As Integer
    '    Dim Cmp_Name As String
    '    Dim W1 As Single = 0
    '    Dim BmsInWrds As String
    '    Dim BankDetailsArray() As String
    '    Dim BInc As Integer
    '    Dim BankName As String = ""
    '    Dim BranchName As String = ""
    '    Dim Ac_No As String = ""
    '    Dim ifsc_code As String = ""
    '    Dim C1 As Integer
    '    Dim C2 As Integer

    '    Dim Blue_Clr As New Color
    '    Blue_Clr = Color.FromArgb(39, 65, 138)
    '    Dim BlueBrush As New SolidBrush(Blue_Clr)
    '    Dim BluePen As New Pen(Blue_Clr)


    '    W1 = e.Graphics.MeasureString("No.of Beams : ", pFont).Width

    '    Try

    '        For I = NoofDets + 1 To NoofItems_PerPage

    '            CurY = CurY + TxtHgt

    '            prn_DetIndx = prn_DetIndx + 1

    '        Next

    '        CurY = CurY + TxtHgt
    '        e.Graphics.DrawLine(BluePen, LMargin, CurY, PageWidth, CurY)
    '        LnAr(5) = CurY
    '        p1Font = New Font("Calibri", 11, FontStyle.Bold)
    '        CurY = CurY + TxtHgt - 13
    '        If is_LastPage = True Then
    '            Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 80, CurY, 2, ClAr(4), pFont, BlueBrush)
    '        End If

    '        If Val(prn_HdDt.Rows(0).Item("Total_Quantity").ToString) <> 0 Then
    '            If is_LastPage = True Then
    '                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Quantity").ToString), "#############0"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
    '            End If
    '        End If
    '        If Val(prn_HdDt.Rows(0).Item("Total_Amount").ToString) <> 0 Then
    '            If is_LastPage = True Then
    '                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Amount").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
    '            End If
    '        End If

    '        CurY = CurY + TxtHgt - 15

    '        CurY = CurY + TxtHgt
    '        e.Graphics.DrawLine(BluePen, LMargin, CurY, PageWidth, CurY)
    '        LnAr(6) = CurY

    '        e.Graphics.DrawLine(BluePen, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(4))
    '        e.Graphics.DrawLine(BluePen, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(4))
    '        e.Graphics.DrawLine(BluePen, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(4))
    '        e.Graphics.DrawLine(BluePen, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(4))
    '        e.Graphics.DrawLine(BluePen, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(4))
    '        e.Graphics.DrawLine(BluePen, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))
    '        e.Graphics.DrawLine(BluePen, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(4))


    '        '-----*--BANK DETAILS----*---

    '        Erase BankDetailsArray
    '        If Trim(prn_HdDt.Rows(0).Item("Company_Bank_Ac_Details").ToString) <> "" Then
    '            BankDetailsArray = Split(Trim(prn_HdDt.Rows(0).Item("Company_Bank_Ac_Details").ToString), ",")

    '            BInc = -1

    '            BInc = BInc + 1
    '            If UBound(BankDetailsArray) >= BInc Then
    '                BankName = Trim(BankDetailsArray(BInc))
    '            End If

    '            BInc = BInc + 1
    '            If UBound(BankDetailsArray) >= BInc Then
    '                BranchName = Trim(BankDetailsArray(BInc))
    '            End If

    '            BInc = BInc + 1
    '            If UBound(BankDetailsArray) >= BInc Then
    '                Ac_No = Trim(BankDetailsArray(BInc))
    '            End If

    '            BInc = BInc + 1
    '            If UBound(BankDetailsArray) >= BInc Then
    '                ifsc_code = Trim(BankDetailsArray(BInc))
    '            End If

    '        End If

    '        C1 = ClAr(1) + ClAr(2) + 20

    '        p1Font = New Font("Calibri", 11, FontStyle.Bold Or FontStyle.Underline)
    '        CurY = CurY + TxtHgt - 10
    '        Common_Procedures.Print_To_PrintDocument(e, "BANK DETAILS", LMargin + 10, CurY, 0, 0, p1Font, BlueBrush)

    '        If (Val(prn_HdDt.Rows(0).Item("Total_Film_Charge").ToString) + Val(prn_HdDt.Rows(0).Item("Total_Screen_Charge").ToString)) <> 0 Then
    '            Common_Procedures.Print_To_PrintDocument(e, "Screen Charge", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 0, 0, pFont, BlueBrush)
    '            Common_Procedures.Print_To_PrintDocument(e, Trim(Format(Val(prn_HdDt.Rows(0).Item("Total_Film_Charge").ToString) + Val(prn_HdDt.Rows(0).Item("Total_Screen_Charge").ToString), "##########0.00")), PageWidth - 10, CurY, 1, 0, pFont)
    '        End If

    '        'If Val(prn_HdDt.Rows(0).Item("Total_Screen_Charge").ToString) <> 0 Then
    '        '    If Val(prn_HdDt.Rows(0).Item("Flim_as_Screen_Charge").ToString) <> 1 And Val(prn_HdDt.Rows(0).Item("Total_Screen_Charge").ToString) <> 0 Then
    '        '        Common_Procedures.Print_To_PrintDocument(e, "Screen Charge", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 0, 0, pFont, BlueBrush)
    '        '        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Total_Screen_Charge").ToString), PageWidth - 10, CurY, 1, 0, pFont)
    '        '    End If
    '        'End If

    '        '----BANK NAME
    '        CurY = CurY + TxtHgt
    '        If BankName <> "" Then
    '            Common_Procedures.Print_To_PrintDocument(e, "BANK NAME ", LMargin + 10, CurY, 0, 0, pFont, BlueBrush)
    '            Common_Procedures.Print_To_PrintDocument(e, " : ", LMargin + C1, CurY, 0, 0, pFont, BlueBrush)
    '            Common_Procedures.Print_To_PrintDocument(e, Trim(BankName), LMargin + C1 + 15, CurY, 0, 0, pFont)
    '        End If
    '        'If Val(prn_HdDt.Rows(0).Item("Total_Film_Charge").ToString) <> 0 Then
    '        '    If Val(prn_HdDt.Rows(0).Item("Flim_as_Screen_Charge").ToString) = 1 Then
    '        '        Common_Procedures.Print_To_PrintDocument(e, "Screen Charge", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 0, 0, pFont, BlueBrush)
    '        '        'Else
    '        '        '    Common_Procedures.Print_To_PrintDocument(e, "Flim Charge", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 0, 0, pFont, BlueBrush)
    '        '        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Total_Film_Charge").ToString), PageWidth - 10, CurY, 1, 0, pFont)
    '        '    End If
    '        'End If

    '        CurY = CurY + TxtHgt
    '        '----BRANCH NAME
    '        If BranchName <> "" Then
    '            Common_Procedures.Print_To_PrintDocument(e, "BRANCH NAME ", LMargin + 10, CurY, 0, 0, pFont, BlueBrush)
    '            Common_Procedures.Print_To_PrintDocument(e, " : ", LMargin + C1, CurY, 0, 0, pFont, BlueBrush)
    '            Common_Procedures.Print_To_PrintDocument(e, Trim(BranchName), LMargin + C1 + 15, CurY, 0, 0, pFont)
    '        End If
    '        'If Val(prn_HdDt.Rows(0).Item("Total_Screen_Alteration_Charge").ToString) <> 0 Then
    '        '    Common_Procedures.Print_To_PrintDocument(e, "Screen Alteration Charge", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 0, 0, pFont, BlueBrush)
    '        '    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Total_Screen_Alteration_Charge").ToString), PageWidth - 10, CurY, 1, 0, pFont)
    '        'End If
    '        If Val(prn_HdDt.Rows(0).Item("Total_Taxable_Amount").ToString) <> 0 Then
    '            Common_Procedures.Print_To_PrintDocument(e, "Total Before Tax", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 0, 0, pFont, BlueBrush)
    '            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Total_Taxable_Amount").ToString), PageWidth - 10, CurY, 1, 0, pFont)
    '        End If

    '        '----ACCOUNT NO
    '        CurY = CurY + TxtHgt
    '        If Ac_No <> "" Then
    '            Common_Procedures.Print_To_PrintDocument(e, "ACCOUNT NO ", LMargin + 10, CurY, 0, 0, pFont, BlueBrush)
    '            Common_Procedures.Print_To_PrintDocument(e, " : ", LMargin + C1, CurY, 0, 0, pFont, BlueBrush)
    '            Common_Procedures.Print_To_PrintDocument(e, Trim(Ac_No), LMargin + C1 + 15, CurY, 0, 0, pFont)
    '        End If
    '        'If Val(prn_HdDt.Rows(0).Item("Total_Film_Alteration_Charge").ToString) <> 0 Then
    '        '    Common_Procedures.Print_To_PrintDocument(e, "Film Alteration Charge", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 0, 0, pFont, BlueBrush)
    '        '    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Total_Film_Alteration_Charge").ToString), PageWidth - 10, CurY, 1, 0, pFont)
    '        'End If
    '        If Val(prn_HdDt.Rows(0).Item("CGST_Percentage").ToString) <> 0 Then
    '            Common_Procedures.Print_To_PrintDocument(e, "CGST " & Trim(prn_HdDt.Rows(0).Item("CGST_Percentage").ToString) & "%", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 0, 0, pFont, BlueBrush)
    '            Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("CGST_Amount").ToString), PageWidth - 10, CurY, 1, 0, pFont)
    '        End If

    '        CurY = CurY + TxtHgt
    '        '----IFSC CODE
    '        If ifsc_code <> "" Then
    '            Common_Procedures.Print_To_PrintDocument(e, "IFSC CODE ", LMargin + 10, CurY, 0, 0, pFont, BlueBrush)
    '            Common_Procedures.Print_To_PrintDocument(e, " : ", LMargin + C1, CurY, 0, 0, pFont, BlueBrush)
    '            Common_Procedures.Print_To_PrintDocument(e, Trim(ifsc_code), LMargin + C1 + 15, CurY, 0, 0, pFont)
    '        End If
    '        If Val(prn_HdDt.Rows(0).Item("SGST_Percentage").ToString) <> 0 Then
    '            Common_Procedures.Print_To_PrintDocument(e, "SGST " & Trim(prn_HdDt.Rows(0).Item("SGST_Percentage").ToString) & "%", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 0, 0, pFont, BlueBrush)
    '            Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("SGST_Amount").ToString), PageWidth - 10, CurY, 1, 0, pFont)
    '        End If

    '        CurY = CurY + TxtHgt
    '        e.Graphics.DrawLine(BluePen, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, PageWidth, CurY)

    '        p1Font = New Font("Calibri", 11, FontStyle.Bold)
    '        If Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString) <> 0 Then
    '            CurY = CurY + TxtHgt - 15
    '            Common_Procedures.Print_To_PrintDocument(e, "Net Amount", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 0, 0, p1Font, BlueBrush)
    '            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Net_Amount").ToString), PageWidth - 10, CurY, 1, 0, p1Font)
    '        End If

    '        p1Font = New Font("Calibri", 12, FontStyle.Bold)
    '        CurY = CurY + TxtHgt + 3
    '        e.Graphics.DrawLine(BluePen, LMargin, CurY, PageWidth, CurY)
    '        LnAr(9) = CurY
    '        e.Graphics.DrawLine(BluePen, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))
    '        CurY = CurY - 10

    '        BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))
    '        BmsInWrds = Replace(Trim((BmsInWrds)), "", "")
    '        StrConv(BmsInWrds, vbProperCase)
    '        CurY = CurY + 15
    '        Common_Procedures.Print_To_PrintDocument(e, "Amount in Words :", LMargin + 10, CurY, 0, 0, p1Font, BlueBrush)
    '        Common_Procedures.Print_To_PrintDocument(e, " Rupees " & BmsInWrds & " ", LMargin + 150, CurY, 0, 0, p1Font)
    '        CurY = CurY + TxtHgt + 5


    '        e.Graphics.DrawLine(BluePen, LMargin, CurY, PageWidth, CurY)

    '        CurY = CurY + 3
    '        p1Font = New Font("Calibri", 11, FontStyle.Bold)
    '        Common_Procedures.Print_To_PrintDocument(e, "Terms & Conditions : ", LMargin + 10, CurY, 0, 0, p1Font, BlueBrush)
    '        CurY = CurY + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, " * Goods received for printing, is returned to party.", LMargin + 10, CurY, 0, 0, pFont, BlueBrush)
    '        CurY = CurY + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, " * Payment should be made within 15 days from the date of delivery otherwise 24% interest will be Charged. ", LMargin + 10, CurY, 0, 0, pFont, BlueBrush)
    '        CurY = CurY + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, " * Subject to Tirupur Jurisdiction only.", LMargin + 10, CurY, 0, 0, pFont, BlueBrush)


    '        CurY = CurY + TxtHgt + 5
    '        e.Graphics.DrawLine(BluePen, LMargin, CurY, PageWidth, CurY)

    '        CurY = CurY + TxtHgt - 10
    '        If Val(Common_Procedures.User.IdNo) <> 1 Then
    '            Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + ClAr(3) + ClAr(4), CurY, 0, 0, pFont)
    '        End If
    '        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
    '        p1Font = New Font("Calibri", 12, FontStyle.Bold)
    '        Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font, BlueBrush)


    '        CurY = CurY + TxtHgt
    '        CurY = CurY + TxtHgt
    '        CurY = CurY + TxtHgt

    '        Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 20, CurY, 0, 0, pFont, BlueBrush)
    '        Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + ClAr(3) + ClAr(4), CurY, 0, 0, pFont, BlueBrush)
    '        'Common_Procedures.Print_To_PrintDocument(e, "Checked By ", LMargin + 350, CurY, 0, 0, pFont, BlueBrush)
    '        Common_Procedures.Print_To_PrintDocument(e, "Authorised Signature ", PageWidth - 15, CurY, 1, 0, pFont, BlueBrush)

    '        CurY = CurY + TxtHgt + 5

    '        e.Graphics.DrawLine(BluePen, LMargin, CurY, PageWidth, CurY)
    '        e.Graphics.DrawLine(BluePen, LMargin, LnAr(1), LMargin, CurY)
    '        e.Graphics.DrawLine(BluePen, PageWidth, LnAr(1), PageWidth, CurY)

    '    Catch ex As Exception

    '        MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

    '    End Try

    'End Sub


    Private Sub dgv_Cloth_Details_CellEndEdit(sender As Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Cloth_Details.CellEndEdit
        dgv_Cloth_Details_CellLeave(sender, e)
    End Sub

    Private Sub dgv_Cloth_Details_CellEnter(sender As Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Cloth_Details.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Rect As Rectangle
        Dim LedID As Integer = 0
        Dim CloID As Integer = 0
        With dgv_Cloth_Details

            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If

            If e.ColumnIndex = 1 Then

                If cbo_Grid_ClothName.Visible = False Or Val(cbo_Grid_ClothName.Tag) <> e.RowIndex Then

                    cbo_Grid_ClothName.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Cloth_Name from Cloth_Head order by Cloth_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Grid_ClothName.DataSource = Dt1
                    cbo_Grid_ClothName.DisplayMember = "Cloth_Name"

                    Rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_ClothName.Left = .Left + Rect.Left
                    cbo_Grid_ClothName.Top = .Top + Rect.Top

                    cbo_Grid_ClothName.Width = Rect.Width
                    cbo_Grid_ClothName.Height = Rect.Height
                    cbo_Grid_ClothName.Text = .CurrentCell.Value

                    cbo_Grid_ClothName.Tag = Val(e.RowIndex)
                    cbo_Grid_ClothName.Visible = True

                    cbo_Grid_ClothName.BringToFront()
                    cbo_Grid_ClothName.Focus()



                End If

            Else
                cbo_Grid_ClothName.Visible = False

            End If

        End With
    End Sub

    Private Sub dgv_Cloth_Details_CellLeave(sender As Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Cloth_Details.CellLeave
        On Error Resume Next
        If FrmLdSTS = True Then Exit Sub

        With dgv_Cloth_Details
            If IsNothing(.CurrentCell) Then Exit Sub
            If .CurrentCell.ColumnIndex = 2 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                Else
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
                End If
            End If
        End With
    End Sub

    Private Sub dgv_Cloth_Details_CellValueChanged(sender As Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Cloth_Details.CellValueChanged
        On Error Resume Next
        If FrmLdSTS = True Then Exit Sub

        If IsNothing(dgv_Cloth_Details.CurrentCell) Then Exit Sub
        With dgv_Cloth_Details
            If .Visible Then

                If .CurrentCell.ColumnIndex = 2 Then

                    'If Common_Procedures.settings.Receipt_Delivery_InMeters = 0 Then
                    ' .CurrentRow.Cells(5).Value = Val(.CurrentRow.Cells(3).Value) * Val(.CurrentRow.Cells(4).Value)
                    'End If

                    Total_ClothCalculation()
                End If

            End If
        End With
    End Sub

    Private Sub dgv_Cloth_Details_EditingControlShowing(sender As Object, e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Cloth_Details.EditingControlShowing
        dgtxt_clothDetails = Nothing
        ' If dgv_YarnDetails.CurrentCell.ColumnIndex > 2 Then
        dgtxt_clothDetails = CType(dgv_Cloth_Details.EditingControl, DataGridViewTextBoxEditingControl)
        ' End If
    End Sub

    Private Sub dgv_Cloth_Details_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles dgv_Cloth_Details.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
    End Sub

    Private Sub dgv_Cloth_Details_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles dgv_Cloth_Details.KeyUp
        Dim n As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            With dgv_Cloth_Details

                n = .CurrentRow.Index

                If n = .Rows.Count - 1 Then
                    For i = 0 To .ColumnCount - 1
                        .Rows(n).Cells(i).Value = ""
                    Next

                Else
                    .Rows.RemoveAt(n)

                End If

                Total_ClothCalculation()

            End With

        End If
    End Sub

    Private Sub dgv_Cloth_Details_LostFocus(sender As Object, e As System.EventArgs) Handles dgv_Cloth_Details.LostFocus
        On Error Resume Next
        If Not IsNothing(dgv_Cloth_Details.CurrentCell) Then dgv_Cloth_Details.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_Cloth_Details_RowsAdded(sender As Object, e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Cloth_Details.RowsAdded
        Dim n As Integer
        If FrmLdSTS = True Then Exit Sub

        If IsNothing(dgv_Cloth_Details.CurrentCell) Then Exit Sub
        With dgv_Cloth_Details
            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)
        End With
    End Sub



    Private Sub cbo_Grid_COuntName_GotFocus(sender As Object, e As System.EventArgs) Handles Cbo_Grid_CountName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
    End Sub

    Private Sub cbo_Grid_COuntName_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Cbo_Grid_CountName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, Cbo_Grid_CountName, Nothing, Nothing, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

        With dgv_Count_details

            If (e.KeyValue = 38 And Cbo_Grid_CountName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                If .CurrentCell.RowIndex <= 0 Then
                    cbo_WeaverName.Focus()
                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(2)
                    .CurrentCell.Selected = True
                End If

            End If

            If (e.KeyValue = 40 And Cbo_Grid_CountName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                If .CurrentCell.RowIndex = .Rows.Count - 1 And Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(1).Value) = "" Then
                    If dgv_EndsCount_Details.Rows.Count > 0 Then
                        dgv_EndsCount_Details.Focus()
                        dgv_EndsCount_Details.CurrentCell = dgv_EndsCount_Details.Rows(0).Cells(1)
                    Else
                    End If

                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(2)
                    .CurrentCell.Selected = True

                End If

            End If

        End With
    End Sub

    Private Sub cbo_Grid_COuntName_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles Cbo_Grid_CountName.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        Dim vCloRate As Single = 0
        Dim trpt_Idno As Integer = 0

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, Cbo_Grid_CountName, Nothing, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            e.Handled = True





            With dgv_Count_details
                If Trim(.Rows(.CurrentRow.Index).Cells(1).Value) = "" Then
                    ' btn_save_Click(sender, e)
                    If dgv_EndsCount_Details.Rows.Count > 0 Then
                        dgv_EndsCount_Details.Focus()
                        dgv_EndsCount_Details.CurrentCell = dgv_EndsCount_Details.Rows(0).Cells(1)
                    Else
                    End If


                Else

                    .Focus()
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(2)


                End If
            End With

        End If
    End Sub

    Private Sub cbo_Grid_COuntName_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Cbo_Grid_CountName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Count_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = Cbo_Grid_CountName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub
    '-----

    Private Sub cbo_Grid_EndsCOuntName_GotFocus(sender As Object, e As System.EventArgs) Handles Cbo_Grid_EndsCountName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")
    End Sub

    Private Sub cbo_Grid_EndsCOuntName_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Cbo_Grid_EndsCountName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, Cbo_Grid_EndsCountName, Nothing, Nothing, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")

        With dgv_EndsCount_Details

            If (e.KeyValue = 38 And Cbo_Grid_EndsCountName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                If .CurrentCell.RowIndex <= 0 Then
                    dgv_Count_details.Focus()
                    dgv_Count_details.CurrentCell = dgv_Count_details.Rows(0).Cells(1)
                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(2)
                    .CurrentCell.Selected = True
                End If

            End If

            If (e.KeyValue = 40 And Cbo_Grid_EndsCountName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                If .CurrentCell.RowIndex = .Rows.Count - 1 And Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(1).Value) = "" Then
                    If dgv_Cloth_Details.Rows.Count > 0 Then
                        dgv_Cloth_Details.Focus()
                        dgv_Cloth_Details.CurrentCell = dgv_Cloth_Details.Rows(0).Cells(1)
                    Else
                        '----
                    End If
                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                    .CurrentCell.Selected = True

                End If

            End If

        End With
    End Sub

    Private Sub cbo_Grid_EndsCOuntName_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles Cbo_Grid_EndsCountName.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        Dim vCloRate As Single = 0
        Dim trpt_Idno As Integer = 0

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, Cbo_Grid_EndsCountName, Nothing, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            e.Handled = True





            With dgv_EndsCount_Details
                If Trim(.Rows(.CurrentRow.Index).Cells(1).Value) = "" And .CurrentRow.Index = .Rows.Count - 1 Then
                    If dgv_Cloth_Details.Rows.Count > 0 Then
                        dgv_Cloth_Details.Focus()
                        dgv_Cloth_Details.CurrentCell = dgv_Cloth_Details.Rows(0).Cells(1)
                    Else
                        '----
                    End If


                Else

                    .Focus()
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(2)


                End If
            End With

        End If
    End Sub

    Private Sub cbo_Grid_EndsCOuntName_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Cbo_Grid_EndsCountName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New EndsCount_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = Cbo_Grid_EndsCountName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    '------

    Private Sub cbo_Grid_ClothName_GotFocus(sender As Object, e As System.EventArgs) Handles cbo_Grid_ClothName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")
    End Sub

    Private Sub cbo_Grid_ClothName_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_ClothName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_ClothName, Nothing, Nothing, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")

        With dgv_Cloth_Details

            If (e.KeyValue = 38 And cbo_Grid_ClothName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                If .CurrentCell.RowIndex <= 0 Then

                    dgv_Cloth_Details.Focus()
                    dgv_Cloth_Details.CurrentCell = dgv_Cloth_Details.Rows(0).Cells(1)



                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(2)
                    .CurrentCell.Selected = True
                End If

            End If

            If (e.KeyValue = 40 And cbo_Grid_ClothName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                If .CurrentCell.RowIndex = .Rows.Count - 1 And Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(1).Value) = "" Then
                    If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                        save_record()

                    Else

                        If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()


                    End If


                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                    .CurrentCell.Selected = True

                End If

            End If

        End With
    End Sub

    Private Sub cbo_Grid_ClothName_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_ClothName.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        Dim vCloRate As Single = 0
        Dim trpt_Idno As Integer = 0

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_ClothName, Nothing, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            e.Handled = True





            With dgv_Cloth_Details
                If Trim(.Rows(.CurrentRow.Index).Cells(1).Value) = "" And .CurrentRow.Index = .Rows.Count - 1 Then
                    If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                        save_record()

                    Else

                        If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()


                    End If


                Else

                    .Focus()
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(2)


                End If
            End With

        End If

    End Sub

    Private Sub cbo_Grid_ClothName_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_ClothName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Cloth_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_ClothName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub dgtxt_clothDetails_Enter(sender As Object, e As System.EventArgs) Handles dgtxt_clothDetails.Enter
        dgv_ActiveCtrl_Name = dgv_Cloth_Details.Name
        'dgv_Cloth_Details.EditingControl.BackColor = Color.Lime
        'dgv_Cloth_Details.EditingControl.ForeColor = Color.Blue
        dgv_Cloth_Details.SelectAll()
    End Sub

    Private Sub dgtxt_clothDetails_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_clothDetails.KeyDown
        With dgv_Cloth_Details

            If e.KeyValue = Keys.Delete Then
                If Val(.Rows(.CurrentCell.RowIndex).Cells(2).Value) <> 0 Then
                    e.Handled = True
                End If
            End If
        End With
    End Sub

    Private Sub dgtxt_clothDetails_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_clothDetails.KeyPress
        With dgv_Cloth_Details
            If .Visible Then

                If .CurrentCell.ColumnIndex = 2 Then

                    If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                        e.Handled = True
                    End If

                End If

            End If
        End With
    End Sub

    Private Sub dgtxt_clothDetails_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_clothDetails.KeyUp
        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            dgv_Cloth_Details_KeyUp(sender, e)
        End If
    End Sub

    Private Sub Cbo_Grid_CountName_TextChanged(sender As Object, e As System.EventArgs) Handles Cbo_Grid_CountName.TextChanged
        Try
            If Cbo_Grid_CountName.Visible Then

                If IsNothing(dgv_Count_details.CurrentCell) Then Exit Sub

                With dgv_Count_details
                    If Val(Cbo_Grid_CountName.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(Cbo_Grid_CountName.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub Cbo_Grid_EndsCountName_TextChanged(sender As Object, e As System.EventArgs) Handles Cbo_Grid_EndsCountName.TextChanged
        Try
            If Cbo_Grid_EndsCountName.Visible Then

                If IsNothing(dgv_EndsCount_Details.CurrentCell) Then Exit Sub

                With dgv_EndsCount_Details
                    If Val(Cbo_Grid_EndsCountName.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(Cbo_Grid_EndsCountName.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub
    Private Sub cbo_Grid_ClothName_TextChanged(sender As Object, e As System.EventArgs) Handles cbo_Grid_ClothName.TextChanged
        Try
            If cbo_Grid_ClothName.Visible Then

                If IsNothing(dgv_Cloth_Details.CurrentCell) Then Exit Sub

                With dgv_Cloth_Details
                    If Val(cbo_Grid_ClothName.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_ClothName.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

End Class