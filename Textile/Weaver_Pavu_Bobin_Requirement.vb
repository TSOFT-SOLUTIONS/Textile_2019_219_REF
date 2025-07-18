Public Class Weaver_Pavu_Bobin_Requirement
    
        Implements Interface_MDIActions

        Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        Private FrmLdSTS As Boolean = False
        Private New_Entry As Boolean = False
        Private Insert_Entry As Boolean = False
        Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "WPABR-"
        Private vcbo_KeyDwnVal As Double
        Private Prec_ActCtrl As New Control
        Private dgv_DrawNo As String = ""
        Private vCbo_ItmNm As String = ""
        Private vCloPic_STS As Boolean = False

        Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl

        Private prn_HdDt As New DataTable
        Private prn_DetDt As New DataTable
        Private prn_PageNo As Integer
        Private prn_DetIndx As Integer
        Private prn_DetSNo As Integer
        Public vmskOldText As String = ""
        Public vmskSelStrt As Integer = -1


        Private Sub clear()
            pnl_Back.Enabled = True
            pnl_Filter.Visible = False

            New_Entry = False
            vmskOldText = ""
            vmskSelStrt = -1

            lbl_LoomNo.Text = ""
            lbl_LoomNo.ForeColor = Color.Black
        cbo_Weaver.Text = ""
        msk_Date.Text = ""
        dtp_Date.Text = ""

        dgv_Details.Rows.Clear()

        dgv_Details_Total.Rows.Clear()
        dgv_Details_Total.Rows.Add()


        cbo_Grid_Loom.Visible = False
        cbo_Grid_Ends.Visible = False

        cbo_Weaver.Enabled = True
        cbo_Weaver.BackColor = Color.White

        cbo_Grid_Loom.Enabled = True
        cbo_Grid_Loom.BackColor = Color.White



        dgv_Details.AllowUserToAddRows = True


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

        If Me.ActiveControl.Name <> cbo_Grid_Loom.Name Then
            cbo_Grid_Loom.Visible = False
        End If

        If Me.ActiveControl.Name <> cbo_Grid_Ends.Name Then
            cbo_Grid_Ends.Visible = False
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
    Private Sub Grid_DeSelect()
        On Error Resume Next
        dgv_Details.CurrentCell.Selected = False
        dgv_Details_Total.CurrentCell.Selected = False
    End Sub

    Private Sub LoomNo_Production_Entry_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated


        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_Loom.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LOOMNO" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_Loom.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If



            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_Ends.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "EMPLOYEE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_Ends.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

    Private Sub LoomNo_Production_Entry_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load


        Me.Text = ""

        con.Open()


        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2
        pnl_Filter.BringToFront()




        AddHandler msk_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_Loom.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_Fromdate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Weaver.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_Loom.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_Shift.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_ToDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_Ends.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Save.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Cancel.GotFocus, AddressOf ControlGotFocus
        ' AddHandler txt_Mark_Details.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Remarks.GotFocus, AddressOf ControlGotFocus
        AddHandler Chk_Cancel.GotFocus, AddressOf ControlGotFocus

        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_Loom.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_Fromdate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Weaver.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_Shift.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_Loom.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_ToDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_Ends.LostFocus, AddressOf ControlLostFocus
        ' AddHandler txt_Mark_Details.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Remarks.LostFocus, AddressOf ControlLostFocus
        AddHandler Chk_Cancel.LostFocus, AddressOf ControlLostFocus

        AddHandler dtp_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler msk_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler msk_Date.KeyPress, AddressOf TextBoxControlKeyPress
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

    Private Sub LoomNo_Production_Entry_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub LoomNo_Production_Entry_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
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
                                txt_Remarks.Focus()

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                            End If

                        Else
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then

                        If .CurrentCell.ColumnIndex <= 3 Then
                            If .CurrentCell.RowIndex = 0 Then
                                cbo_Weaver.Focus()

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 6)

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


            da1 = New SqlClient.SqlDataAdapter("select a.* from Weaver_PavuBobin_Requirement_Head a  Where a.Weaver_PavuBobin_Requirement_Code = '" & Trim(NewCode) & "'", con)
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_LoomNo.Text = dt1.Rows(0).Item("Weaver_PavuBobin_Requirement_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Weaver_PavuBobin_Requirement_Date").ToString
                msk_Date.Text = dtp_Date.Text
                cbo_Weaver.Text = Common_Procedures.Ledger_IdNoToName(con, dt1.Rows(0).Item("Ledger_Idno").ToString)
                ' txt_Mark_Details.Text = dt1.Rows(0).Item("Mark_Details").ToString
                txt_Remarks.Text = dt1.Rows(0).Item("Remarks").ToString
                If Val(dt1.Rows(0).Item("Cancel_Status").ToString) = 1 Then Chk_Cancel.Checked = True

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.EndsCount_Name from Weaver_PavuBobin_Requirement_Details a INNER JOIN EndsCount_Head b ON a.EndsCount_idno = b.EndsCount_idno   where a.Weaver_PavuBobin_Requirement_Code = '" & Trim(NewCode) & "' Order by a.Sl_No", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_Details.Rows.Clear()
                SNo = 0

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_Details.Rows.Add()

                        SNo = SNo + 1

                        dgv_Details.Rows(n).Cells(0).Value = Val(SNo)



                        dgv_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Loom_No").ToString
                        dgv_Details.Rows(n).Cells(2).Value = (dt2.Rows(i).Item("EndsCount_Name").ToString)
                        dgv_Details.Rows(n).Cells(3).Value = (dt2.Rows(i).Item("Mark").ToString)
                        dgv_Details.Rows(n).Cells(4).Value = Val(dt2.Rows(i).Item("Sized_Beam").ToString)
                        dgv_Details.Rows(n).Cells(5).Value = Val(dt2.Rows(i).Item("Noof_Bobin").ToString)
                        dgv_Details.Rows(n).Cells(6).Value = Val(dt2.Rows(i).Item("Weaver_PavuBobin_Requirement_Slno").ToString)
                        dgv_Details.Rows(n).Cells(7).Value = (dt2.Rows(i).Item("Noof_Bobin_Return").ToString)
                        dgv_Details.Rows(n).Cells(8).Value = (dt2.Rows(i).Item("Sized_Beam_Return").ToString)
                        If Val(dgv_Details.Rows(n).Cells(7).Value) <> 0 Or Val(dgv_Details.Rows(n).Cells(8).Value) <> 0 Then
                            For j = 0 To dgv_Details.ColumnCount - 1
                                dgv_Details.Rows(n).Cells(j).Style.BackColor = Color.LightGray
                            Next j
                            LockSTS = True
                        End If

                    Next i

                End If

                If dgv_Details.RowCount = 0 Then dgv_Details.Rows.Add()
                With dgv_Details_Total
                    If .RowCount = 0 Then .Rows.Add()
                    .Rows(0).Cells(5).Value = Val(dt1.Rows(0).Item("Total_Bobin").ToString)
                    
                End With
                dt2.Dispose()
                da2.Dispose()

            End If

            dt1.Dispose()
            da1.Dispose()

            Grid_DeSelect()
            If LockSTS = True Then

                cbo_Weaver.Enabled = False
                cbo_Weaver.BackColor = Color.LightGray

                cbo_Grid_Loom.Enabled = False
                cbo_Grid_Loom.BackColor = Color.LightGray

              

                dgv_Details.AllowUserToAddRows = False

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            If msk_Date.Visible And msk_Date.Enabled Then msk_Date.Focus()

        End Try

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""


        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close other windows", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> DialogResult.Yes Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_LoomNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        Da = New SqlClient.SqlDataAdapter("select sum(Noof_Bobin_Return) from Weaver_PavuBobin_Requirement_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_PavuBobin_Requirement_Code = '" & Trim(NewCode) & "'", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                If Val(Dt1.Rows(0)(0).ToString) <> 0 Then
                    MessageBox.Show("Alreay some Bobin Delivered against this Requirement", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If
        Dt1.Clear()
        Da = New SqlClient.SqlDataAdapter("select sum(Sized_Beam_Return) from Weaver_PavuBobin_Requirement_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_PavuBobin_Requirement_Code = '" & Trim(NewCode) & "'", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                If Val(Dt1.Rows(0)(0).ToString) <> 0 Then
                    MessageBox.Show("Alreay some Pavu Beam Delivered against this Requirement", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If
        Dt1.Clear()
        trans = con.BeginTransaction

        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_LoomNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.Transaction = trans


            cmd.CommandText = "Delete from Weaver_PavuBobin_Requirement_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_PavuBobin_Requirement_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Weaver_PavuBobin_Requirement_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_PavuBobin_Requirement_Code = '" & Trim(NewCode) & "'"
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

        Finally

            If msk_Date.Visible And msk_Date.Enabled Then msk_Date.Focus()

        End Try
    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        If Filter_Status = False Then



            cbo_Filter_Loom.Text = ""
            cbo_Filter_Loom.SelectedIndex = -1
            cbo_Filter_Shift.Text = ""
            cbo_Filter_Shift.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()

        End If

        pnl_Filter.Visible = True
        pnl_Back.Enabled = False
        If dtp_Filter_Fromdate.Enabled And dtp_Filter_Fromdate.Visible Then dtp_Filter_Fromdate.Focus()

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim RefCode As String

        Try

            inpno = InputBox("Enter New Dc.No.", "FOR NEW DC NO INSERTION...")

            RefCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Weaver_PavuBobin_Requirement_No from Weaver_PavuBobin_Requirement_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_PavuBobin_Requirement_Code = '" & Trim(RefCode) & "'", con)
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
                    MessageBox.Show("Invalid DC No", "DOES NOT INSERT NEW DC NO...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_LoomNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT NEW DC...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String

        Try

            da = New SqlClient.SqlDataAdapter("select top 1 Weaver_PavuBobin_Requirement_No from Weaver_PavuBobin_Requirement_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_PavuBobin_Requirement_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Weaver_PavuBobin_Requirement_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_LoomNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Weaver_PavuBobin_Requirement_No from Weaver_PavuBobin_Requirement_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_PavuBobin_Requirement_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Weaver_PavuBobin_Requirement_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_LoomNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Weaver_PavuBobin_Requirement_No from Weaver_PavuBobin_Requirement_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_PavuBobin_Requirement_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Weaver_PavuBobin_Requirement_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 Weaver_PavuBobin_Requirement_No from Weaver_PavuBobin_Requirement_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_PavuBobin_Requirement_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Weaver_PavuBobin_Requirement_No desc", con)
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

            lbl_LoomNo.Text = Common_Procedures.get_MaxCode(con, "Weaver_PavuBobin_Requirement_Head", "Weaver_PavuBobin_Requirement_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

            lbl_LoomNo.ForeColor = Color.Red

            msk_Date.Text = Date.Today.ToShortDateString
            da = New SqlClient.SqlDataAdapter("select top 1 * from Weaver_PavuBobin_Requirement_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_PavuBobin_Requirement_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Weaver_PavuBobin_Requirement_No desc", con)
            dt1 = New DataTable
            da.Fill(dt1)
            If dt1.Rows.Count > 0 Then
                If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)
                    If dt1.Rows(0).Item("Weaver_PavuBobin_Requirement_Date").ToString <> "" Then msk_Date.Text = dt1.Rows(0).Item("Weaver_PavuBobin_Requirement_Date").ToString
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
        Dim RefCode As String

        Try

            inpno = InputBox("Enter Dc.No.", "FOR FINDING...")

            RefCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Weaver_PavuBobin_Requirement_No from Weaver_PavuBobin_Requirement_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_PavuBobin_Requirement_Code = '" & Trim(RefCode) & "'", con)
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

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim Lom_Id As Integer = 0
        Dim Ends_ID As Integer = 0
        Dim Cnc_Sts As Integer = 0
        Dim Led_ID As Integer = 0
        Dim Brand_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim vTotBoin As Single = 0
        Dim EntID As String = ""
        Dim PBlNo As String = ""
        Dim Partcls As String = ""


        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

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


        Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Weaver.Text)
        If Led_ID = 0 Then
            MessageBox.Show("Invalid Weaver Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Weaver.Enabled And cbo_Weaver.Visible Then cbo_Weaver.Focus()
            Exit Sub
        End If

        For i = 0 To dgv_Details.RowCount - 1

            If Val(dgv_Details.Rows(i).Cells(1).Value) <> 0 Or Val(dgv_Details.Rows(i).Cells(5).Value) <> 0 Then

                Ends_ID = Common_Procedures.EndsCount_NameToIdNo(con, dgv_Details.Rows(i).Cells(2).Value)
                If Ends_ID = 0 Then
                    MessageBox.Show("Invalid EndsCount Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    If dgv_Details.Enabled And dgv_Details.Visible Then
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(2)
                    End If
                    Exit Sub
                End If


                'If Val(dgv_Details.Rows(i).Cells(2).Value) = 0 Then
                '    MessageBox.Show("Invalid Meters", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                '    If dgv_Details.Enabled And dgv_Details.Visible Then
                '        dgv_Details.Focus()
                '        dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(2)
                '    End If
                '    Exit Sub
                'End If



                'Emp_Id = Common_Procedures.Employee_NameToIdNo(con, dgv_Details.Rows(i).Cells(6).Value)
                'If Emp_Id = 0 Then
                '    MessageBox.Show("Invalid Employee Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                '    If dgv_Details.Enabled And dgv_Details.Visible Then
                '        dgv_Details.Focus()
                '        dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(6)
                '    End If
                '    Exit Sub
                'End If

            End If

        Next
        Cnc_Sts = 0
        If Chk_Cancel.Checked = True Then Cnc_Sts = 1
        vTotBoin = 0
        If dgv_Details_Total.RowCount > 0 Then
            vTotBoin = Val(dgv_Details_Total.Rows(0).Cells(5).Value())
            
        End If



        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_LoomNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_LoomNo.Text = Common_Procedures.get_MaxCode(con, "Weaver_PavuBobin_Requirement_Head", "Weaver_PavuBobin_Requirement_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_LoomNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr
            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@LoomDate", Convert.ToDateTime(msk_Date.Text))

            If New_Entry = True Then


                cmd.CommandText = "Insert into Weaver_PavuBobin_Requirement_Head(Weaver_PavuBobin_Requirement_Code, Company_IdNo, Weaver_PavuBobin_Requirement_No, for_OrderBy, Weaver_PavuBobin_Requirement_Date,  Ledger_IdNo, Total_Bobin, Cancel_Status  ,Remarks) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_LoomNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_LoomNo.Text))) & ", @LoomDate," & Val(Led_ID) & ", " & Str(Val(vTotBoin)) & "," & Str(Val(Cnc_Sts)) & " ,'" & Trim(txt_Remarks.Text) & "')"
                cmd.ExecuteNonQuery()

            Else
                cmd.CommandText = "Update Weaver_PavuBobin_Requirement_Head set Ledger_IdNo = " & Val(Led_ID) & ", Weaver_PavuBobin_Requirement_Date= @LoomDate ,  Total_Bobin = " & Str(Val(vTotBoin)) & ", Cancel_Status = " & Str(Val(Cnc_Sts)) & ", Remarks = '" & Trim(txt_Remarks.Text) & "' Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_PavuBobin_Requirement_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If

            EntID = Trim(Pk_Condition) & Trim(lbl_LoomNo.Text)
            PBlNo = Trim(lbl_LoomNo.Text)
            Partcls = "Loom : Dc.No. " & Trim(lbl_LoomNo.Text)

            cmd.CommandText = "Delete from Weaver_PavuBobin_Requirement_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_PavuBobin_Requirement_Code = '" & Trim(NewCode) & "' and Noof_Bobin_Return=0 and Sized_Beam_Return=0"
            cmd.ExecuteNonQuery()

            Dim Nr As Integer

            With dgv_Details
                Sno = 0
                For i = 0 To dgv_Details.RowCount - 1

                    If Val(dgv_Details.Rows(i).Cells(1).Value) <> 0 Or Val(dgv_Details.Rows(i).Cells(5).Value) <> 0 Then

                        Sno = Sno + 1

                        'Lom_Id = Common_Procedures.Loom_NameToIdNo(con, dgv_Details.Rows(i).Cells(1).Value, tr)
                        Ends_ID = Common_Procedures.EndsCount_NameToIdNo(con, dgv_Details.Rows(i).Cells(2).Value, tr)
                        Nr = 0
                        cmd.CommandText = "Update  Weaver_PavuBobin_Requirement_Details set Weaver_PavuBobin_Requirement_Date = @LoomDate , Sl_No  = " & Str(Val(Sno)) & " , Ledger_IdNo = " & Val(Led_ID) & ", Loom_No = " & Val(.Rows(i).Cells(1).Value) & " , EndsCount_Idno = " & Str(Val(Ends_ID)) & "  , Mark = " & Val(.Rows(i).Cells(3).Value) & " , Sized_Beam = " & Val(.Rows(i).Cells(4).Value) & " , Noof_Bobin = " & Val(.Rows(i).Cells(5).Value) & "   where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_PavuBobin_Requirement_Code = '" & Trim(NewCode) & "'  and Weaver_PavuBobin_Requirement_Slno = " & Str(Val(.Rows(i).Cells(6).Value))
                        Nr = cmd.ExecuteNonQuery()

                        If Nr = 0 Then
                            cmd.CommandText = "Insert into Weaver_PavuBobin_Requirement_Details ( Weaver_PavuBobin_Requirement_Code, Company_IdNo, Weaver_PavuBobin_Requirement_No, for_OrderBy, Weaver_PavuBobin_Requirement_Date, Sl_No   ,Ledger_IdNo   , Loom_No, EndsCount_Idno  ,  Mark ,    Sized_Beam  ,  Noof_Bobin) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_LoomNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_LoomNo.Text))) & ", @LoomDate, " & Str(Val(Sno)) & ", " & Val(Led_ID) & " ,  " & Str(Val(.Rows(i).Cells(1).Value)) & ", " & Str(Val(Ends_ID)) & "," & Str(Val(.Rows(i).Cells(3).Value)) & "," & Str(Val(.Rows(i).Cells(4).Value)) & "," & Str(Val(.Rows(i).Cells(5).Value)) & " )"
                            cmd.ExecuteNonQuery()
                        End If

                    End If

                Next

            End With


            tr.Commit()

            move_record(lbl_LoomNo.Text)

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            Dt1.Dispose()
            Da.Dispose()
            tr.Dispose()

            If msk_Date.Enabled And msk_Date.Visible Then msk_Date.Focus()

        End Try

    End Sub

    Private Sub cbo_Grid_Loom_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Loom.GotFocus
        Dim Led_Id As Integer = 0
        Led_Id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Weaver.Text)
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Weaver_Loom_Details", "Loom_no", "(Ledger_idNo = " & Val(Led_Id) & ")", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Grid_Loom_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Loom.KeyDown

        vcbo_KeyDwnVal = e.KeyValue
        Dim Led_Id As Integer = 0
        Led_Id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Weaver.Text)
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_Loom, Nothing, Nothing, "Weaver_Loom_Details", "Loom_no", "(Ledger_idNo = " & Val(Led_Id) & ")", "(Ledger_idno = 0)")

        With dgv_Details

            If (e.KeyValue = 38 And cbo_Grid_Loom.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                e.Handled = True
                If .CurrentRow.Index <= 0 Then
                    cbo_Weaver.Focus()

                Else
                    .Focus()
                    .CurrentCell = dgv_Details.Rows(dgv_Details.CurrentRow.Index - 1).Cells(3)

                End If

            End If


            If (e.KeyValue = 40 And cbo_Grid_Loom.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                e.Handled = True
                If Trim(.Rows(.CurrentRow.Index).Cells(1).Value) = "" And .CurrentRow.Index = .Rows.Count - 1 Then
                    txt_Remarks.Focus()

                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(3)

                End If
            End If

        End With
    End Sub

    Private Sub cbo_Grid_Loom_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_Loom.KeyPress
        Dim Led_Id As Integer = 0
        Led_Id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Weaver.Text)
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_Loom, Nothing, "Weaver_Loom_Details", "Loom_no", "(Ledger_idNo = " & Val(Led_Id) & ")", "(Ledger_idno = 0)")
        If Asc(e.KeyChar) = 13 Then

            e.Handled = True

            With dgv_Details
                If Trim(.Rows(.CurrentRow.Index).Cells(1).Value) = "" And .CurrentRow.Index = .Rows.Count - 1 Then
                    txt_Remarks.Focus()

                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(3)

                End If
            End With

            get_LoomNo_Details()

        End If
    End Sub


    Private Sub dgv_Details_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs)
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
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim Dt4 As New DataTable
        Dim Dt5 As New DataTable
        Dim rect As Rectangle

        With dgv_Details

            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If




            If e.ColumnIndex = 1 Then

                If cbo_Grid_Loom.Visible = False Or Val(cbo_Grid_Loom.Tag) <> e.RowIndex Then

                    cbo_Grid_Loom.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Loom_No from Weaver_Loom_Details order by Loom_No", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Grid_Loom.DataSource = Dt1
                    cbo_Grid_Loom.DisplayMember = "lOOM_No"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_Loom.Left = .Left + rect.Left
                    cbo_Grid_Loom.Top = .Top + rect.Top

                    cbo_Grid_Loom.Width = rect.Width
                    cbo_Grid_Loom.Height = rect.Height
                    cbo_Grid_Loom.Text = .CurrentCell.Value

                    cbo_Grid_Loom.Tag = Val(e.RowIndex)
                    cbo_Grid_Loom.Visible = True

                    cbo_Grid_Loom.BringToFront()
                    cbo_Grid_Loom.Focus()

                End If

            Else
                cbo_Grid_Loom.Visible = False

            End If



            'If e.ColumnIndex = 6 Then

            '    If cbo_Grid_Ends.Visible = False Or Val(cbo_Grid_Ends.Tag) <> e.RowIndex Then

            '        cbo_Grid_Ends.Tag = -1
            '        Da = New SqlClient.SqlDataAdapter("select Employee_Name from PayRoll_Employee_Head order by Employee_Name", con)
            '        Dt1 = New DataTable
            '        Da.Fill(Dt5)
            '        cbo_Grid_Ends.DataSource = Dt5
            '        cbo_Grid_Ends.DisplayMember = "Employee_Name"

            '        rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

            '        cbo_Grid_Ends.Left = .Left + rect.Left
            '        cbo_Grid_Ends.Top = .Top + rect.Top

            '        cbo_Grid_Ends.Width = rect.Width
            '        cbo_Grid_Ends.Height = rect.Height
            '        cbo_Grid_Ends.Text = .CurrentCell.Value

            '        cbo_Grid_Ends.Tag = Val(e.RowIndex)
            '        cbo_Grid_Ends.Visible = True

            '        cbo_Grid_Ends.BringToFront()
            '        cbo_Grid_Ends.Focus()


            '    End If


            'Else
            '    cbo_Grid_Ends.Visible = False

            'End If



        End With
    End Sub
    Private Sub dgv_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellLeave
        Try
            With dgv_Details
                If .Visible = True Then
                    If .Rows.Count > 0 Then


                        'If .CurrentCell.ColumnIndex =  Or .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Then
                        '    If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                        '        .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                        '    Else
                        '        .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
                        '    End If
                        'End If
                    End If
                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL LEAVE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub
    Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged
        On Error Resume Next
        With dgv_Details
            If .Visible Then
                If .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 4 Then

                    Total_Calculation()
                End If
            End If
        End With
    End Sub

    Private Sub dgv_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Details.EditingControlShowing
        dgtxt_Details = CType(dgv_Details.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub
    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.Enter
        dgv_Details.EditingControl.BackColor = Color.Lime
    End Sub

    Private Sub dgtxt_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyDown
        Try

            With dgv_Details

                If e.KeyValue = Keys.Delete Then
                    If Val(.Rows(.CurrentCell.RowIndex).Cells(7).Value) <> 0 Or Val(.Rows(.CurrentCell.RowIndex).Cells(8).Value) <> 0 Then
                        e.Handled = True
                    End If

                End If
            End With

        Catch ex As Exception

        End Try
    End Sub
    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Details.KeyPress
        With dgv_Details
            If .Visible Then
                If Val(.Rows(.CurrentCell.RowIndex).Cells(7).Value) <> 0 Or Val(.Rows(.CurrentCell.RowIndex).Cells(8).Value) <> 0 Then
                    e.Handled = True
                End If
                If .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 4 Then

                    If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                        e.Handled = True
                    End If

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

                For i = 0 To .Rows.Count - 1
                    .Rows(i).Cells(0).Value = i + 1
                Next

            End With

            Total_Calculation()

        End If
    End Sub

    Private Sub dgv_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.LostFocus
        On Error Resume Next
        dgv_Details.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_Details_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Details.RowsAdded
        Dim n As Integer

        With dgv_Details
            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)
        End With
    End Sub

    Private Sub cbo_Weaver_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Weaver.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = 'WEAVER' or (Ledger_Type = '' ) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Weaver_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Weaver.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Weaver, msk_Date, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = 'WEAVER' or (Ledger_Type = '' ) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
        If (e.KeyValue = 40 And cbo_Weaver.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If dgv_Details.Rows.Count > 0 Then


                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
            Else
                txt_Remarks.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_Weaver_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Weaver.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Weaver, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = 'WEAVER' or (Ledger_Type = '' ) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
        If Asc(e.KeyChar) = 13 Then
            If dgv_Details.Rows.Count > 0 Then


                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
            Else
                txt_Remarks.Focus()
            End If
        End If
    End Sub



    'Private Sub cbo_Grid_Employee_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Ends.GotFocus
    '    Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "PayRoll_Employee_Head", "Employee_name", "", "(Employee_idno = 0)")
    'End Sub




    'Private Sub cbo_Grid_Employee_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Ends.KeyDown
    '    vcbo_KeyDwnVal = e.KeyValue
    '    Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_Ends, Nothing, Nothing, "PayRoll_Employee_Head", "Employee_name", "", "(Employee_idno = 0)")
    '    With dgv_Details

    '        If (e.KeyValue = 38 And cbo_Grid_Ends.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

    '            dgv_Details.Focus()
    '            dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(5)

    '        End If

    '        If (e.KeyValue = 40 And cbo_Grid_Ends.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

    '            If .CurrentRow.Index = .Rows.Count - 1 Then

    '                If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
    '                    save_record()
    '                Else
    '                    dtp_date.Focus()
    '                End If

    '            Else
    '                dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentRow.Index + 1).Cells(1)

    '            End If

    '        End If

    '    End With
    'End Sub

    'Private Sub cbo_Grid_Employee_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_Ends.KeyPress
    '    Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_Ends, Nothing, "PayRoll_Employee_Head", "Employee_name", "", "(Employee_idno = 0)")
    '    If Asc(e.KeyChar) = 13 Then
    '        With dgv_Details
    '            If Trim(.Rows(.CurrentRow.Index).Cells(6).Value) = "" And .CurrentRow.Index = .Rows.Count - 1 Then
    '                If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
    '                    save_record()
    '                Else
    '                    dtp_date.Focus()
    '                End If

    '            Else
    '                dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentRow.Index + 1).Cells(1)

    '            End If
    '        End With
    '    End If
    'End Sub

    'Private Sub cbo_Grid_Employee_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Ends.KeyUp
    '    'If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
    '    '    dgv_Details_KeyUp(sender, e)
    '    'End If
    '    If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
    '        Dim f As New Payroll_Employee_Creation

    '        Common_Procedures.Master_Return.Form_Name = Me.Name
    '        Common_Procedures.Master_Return.Control_Name = cbo_Grid_Ends.Name
    '        Common_Procedures.Master_Return.Return_Value = ""
    '        Common_Procedures.Master_Return.Master_Type = ""

    '        f.MdiParent = MDIParent1
    '        f.Show()
    '    End If
    'End Sub


    'Private Sub cbo_Grid_Employeee_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Ends.TextChanged
    '    Try
    '        If cbo_Grid_Ends.Visible Then
    '            With dgv_Details
    '                If Val(cbo_Grid_Ends.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 6 Then
    '                    .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_Ends.Text)
    '                End If
    '            End With
    '        End If

    '    Catch ex As Exception
    '        'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

    '    End Try
    'End Sub

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
        Dim Lom_IdNo As Integer, Sht_IdNo As Integer
        Dim Condt As String = ""

        Try

            Condt = ""
            Lom_IdNo = 0
            Sht_IdNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Weaver_PavuBobin_Requirement_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Weaver_PavuBobin_Requirement_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Weaver_PavuBobin_Requirement_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_Loom.Text) <> "" Then
                Lom_IdNo = Common_Procedures.Loom_NameToIdNo(con, cbo_Filter_Loom.Text)
            End If

            If Val(Lom_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " b.Loom_IdNo = " & Str(Val(Lom_IdNo))
            End If

            If Trim(cbo_Filter_Shift.Text) <> "" Then
                Sht_IdNo = Common_Procedures.Shift_NameToIdNo(con, cbo_Filter_Shift.Text)
            End If

            If Val(Sht_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Shift_IdNo = " & Str(Val(Sht_IdNo))
            End If


            da = New SqlClient.SqlDataAdapter("select a.*, b.*, c.Loom_name, d.Employee_name from Weaver_PavuBobin_Requirement_Head a INNER join Weaver_PavuBobin_Requirement_Details b on a.Weaver_PavuBobin_Requirement_Code = b.Weaver_PavuBobin_Requirement_Code left outer join Loom_head c on b.Loom_idno = c.Loom_idno left outer join PayRoll_Employee_head d on b.Employee_idno = d.Employee_idno  where a.company_idno =" & Str(Val(lbl_Company.Tag)) & "and a.Weaver_PavuBobin_Requirement_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.Weaver_PavuBobin_Requirement_Date, a.for_orderby, a.Weaver_PavuBobin_Requirement_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()


                    dgv_Filter_Details.Rows(n).Cells(0).Value = dt2.Rows(i).Item("Weaver_PavuBobin_Requirement_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Weaver_PavuBobin_Requirement_Date").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = Common_Procedures.Shift_IdNoToName(con, dt2.Rows(i).Item("Shift_Idno").ToString)
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Loom_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("Meters").ToString), "########0.00")
                    dgv_Filter_Details.Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("Warp_Meters").ToString), "########0.00")
                    dgv_Filter_Details.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Weft_Meters").ToString), "########0.00")
                    dgv_Filter_Details.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Pick_Efficiency").ToString), "########0.00")
                    dgv_Filter_Details.Rows(n).Cells(8).Value = dt2.Rows(i).Item("Employee_Name").ToString
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

    Private Sub cbo_Filter_Item_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_Loom.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Loom_Head", "Loom_Name", "", "(Loom_idno = 0)")
    End Sub

    Private Sub cbo_Filter_Item_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_Loom.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_Loom, dtp_Filter_ToDate, btn_Filter_Show, "Loom_Head", "Loom_Name", "", "(Loom_idno = 0)")
    End Sub


    Private Sub cbo_Filter_Item_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_Loom.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_Loom, btn_Filter_Show, "Loom_Head", "Loom_Name", "", "(Loom_idno = 0)")
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

    'Private Sub cbo_Grid_Loom_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Loom.KeyUp
    '    'If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
    '    '    dgv_Details_KeyUp(sender, e)
    '    'End If
    '    If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
    '        Dim f As New LoomNo_Creation

    '        Common_Procedures.Master_Return.Form_Name = Me.Name
    '        Common_Procedures.Master_Return.Control_Name = cbo_Grid_Loom.Name
    '        Common_Procedures.Master_Return.Return_Value = ""
    '        Common_Procedures.Master_Return.Master_Type = ""

    '        f.MdiParent = MDIParent1
    '        f.Show()
    '    End If
    'End Sub



    Private Sub cbo_Grid_Loom_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Loom.TextChanged
        Try
            If cbo_Grid_Loom.Visible Then
                With dgv_Details
                    If Val(cbo_Grid_Loom.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_Loom.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub



    Private Sub Total_Calculation()
        Dim Sno As Integer
        Dim Totbobn As Single

        Sno = 0
        Totbobn = 0
        With dgv_Details
            For i = 0 To .RowCount - 1
                Sno = Sno + 1
                .Rows(i).Cells(0).Value = Sno
                If Val(.Rows(i).Cells(1).Value) <> 0 Or Val(.Rows(i).Cells(3).Value) <> 0 Or Val(.Rows(i).Cells(4).Value) <> 0 Then
                    Totbobn = Totbobn + Val(.Rows(i).Cells(5).Value)

                End If
            Next
        End With

        With dgv_Details_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(5).Value = Format(Val(Totbobn), "########0")
            
        End With

    End Sub

    Private Sub btn_Save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Save.Click
        save_record()
    End Sub

    Private Sub btn_Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Cancel.Click
        Me.Close()
    End Sub



    Public Sub print_record() Implements Interface_MDIActions.print_record
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_LoomNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select * from Weaver_PavuBobin_Requirement_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_PavuBobin_Requirement_Code = '" & Trim(NewCode) & "'", con)
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
        Dim NewCode As String

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_LoomNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.* from Weaver_PavuBobin_Requirement_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_PavuBobin_Requirement_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Item_name, d.Unit_name, e.Machine_name, f.Brand_Name from Weaver_PavuBobin_Requirement_Details a INNER JOIN Item_Head b ON a.Item_idno = b.Item_idno LEFT OUTER JOIN Unit_Head d ON a.Unit_idno = d.Unit_idno LEFT OUTER JOIN Machine_Head e ON a.Machine_idno = e.Machine_idno LEFT OUTER JOIN Brand_Head f ON a.Brand_idno = f.Brand_idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_PavuBobin_Requirement_Code = '" & Trim(NewCode) & "' Order by a.Sl_No", con)
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

        Printing_Format1(e)


    End Sub

    Private Sub Printing_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim pFont As Font
        Dim ps As Printing.PaperSize
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

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
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

        NoofItems_PerPage = 8

        Erase LnAr
        Erase ClAr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClAr(1) = Val(35) : ClAr(2) = 275 : ClAr(3) = 150 : ClAr(4) = 80 : ClAr(5) = 75
        ClAr(6) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5))

        TxtHgt = 19

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_LoomNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try
            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format1_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr)

                NoofDets = 0

                CurY = CurY - 10

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then

                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued...", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 10, CurY, 0, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, False)

                            e.HasMorePages = True
                            Return

                        End If

                        ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Item_Name").ToString)
                        ItmNm2 = ""
                        If Len(ItmNm1) > 15 Then
                            For I = 15 To 1 Step -1
                                If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 15
                            ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                            ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                        End If

                        CurY = CurY + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Sl_No").ToString), LMargin + 15, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Brand_Name").ToString, LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Quantity").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Unit_Name").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Machine_Name").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 10, CurY, 0, 0, pFont)

                        NoofDets = NoofDets + 1

                        If Trim(ItmNm2) <> "" Then
                            CurY = CurY + TxtHgt - 5
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                            NoofDets = NoofDets + 1
                        End If

                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If

                Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True)

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
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
        Dim strHeight As Single
        Dim C1 As Single
        Dim W1 As Single
        Dim S1 As Single

        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Item_name, d.Unit_name, e.Machine_name, f.Brand_Name from Weaver_PavuBobin_Requirement_Details a INNER JOIN Item_Head b ON a.Item_idno = b.Item_idno LEFT OUTER JOIN Unit_Head d ON a.Unit_idno = d.Unit_idno LEFT OUTER JOIN Machine_Head e ON a.Machine_idno = e.Machine_idno LEFT OUTER JOIN Brand_Head f ON a.Brand_idno = f.Brand_idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_PavuBobin_Requirement_Code = '" & Trim(EntryCode) & "' Order by a.Sl_No", con)
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
        Common_Procedures.Print_To_PrintDocument(e, "DELIVERY", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        C1 = ClAr(1) + ClAr(2) + ClAr(3)
        W1 = e.Graphics.MeasureString("D.C DATE  : ", pFont).Width
        S1 = e.Graphics.MeasureString("Received From :  ", pFont).Width


        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "Issued To", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "M/s." & prn_HdDt.Rows(0).Item("Issued_Name").ToString, LMargin + S1 + 30, CurY, 0, 0, pFont)

        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "DC.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Weaver_PavuBobin_Requirement_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)


        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "Received From", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "M/s." & prn_HdDt.Rows(0).Item("Shift_Name").ToString, LMargin + S1 + 30, CurY, 0, 0, pFont)

        'Common_Procedures.Print_To_PrintDocument(e, "Received From :  " & "M/s." & prn_HdDt.Rows(0).Item("Shift_Name").ToString, LMargin + 10, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Weaver_PavuBobin_Requirement_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))

        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "ITEM NAME", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "BRAND", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "QUANTITY", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "UNIT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "MACHINE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY


    End Sub

        Private Sub Printing_Format1_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
            Dim i As Integer
            Dim Cmp_Name As String
            Dim p1Font As Font


            For i = NoofDets + 1 To NoofItems_PerPage
                CurY = CurY + TxtHgt
            Next

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

            CurY = CurY + TxtHgt - 10
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, " TOTAL", LMargin + ClAr(1) + 30, CurY, 2, ClAr(4), pFont)
                Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Quantity").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))

            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt

            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            p1Font = New Font("Calibri", 12, FontStyle.Bold)

            Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 20, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

        End Sub



        Private Sub cbo_Filter_Shift_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_Shift.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = 'WEAVER' or (Ledger_Type = '' ) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
        End Sub

        Private Sub cbo_Filter_Shift_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_Shift.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_Shift, dtp_Filter_ToDate, cbo_Filter_Loom, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = 'WEAVER' or (Ledger_Type = '' ) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")

        End Sub

        Private Sub cbo_Filter_Shift_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_Shift.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_Shift, cbo_Filter_Loom, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = 'WEAVER' or (Ledger_Type = '' ) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")

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

            If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
                msk_Date.Text = Date.Today
            End If
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

    Private Sub txt_Mark_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If (e.KeyValue = 38) Then
            cbo_Weaver.Focus()
        End If
        If (e.KeyValue = 40) Then
            If dgv_Details.Rows.Count > 0 Then


                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
            Else
                txt_Remarks.Focus()
            End If
        End If
    End Sub

    

    Private Sub cbo_Weaver_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Weaver.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Common_Procedures.MDI_LedType = "WEAVER"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Weaver.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub
    Private Sub get_LoomNo_Details()
        Dim q As Single = 0
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim Cn_bag As Single
        Dim Wgt_Bag As Single
        Dim Wgt_Cn As Single
        Dim Lm_No As Integer = 0
        Dim Led_Id As Integer

        Lm_No = Val(dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(1).Value)
        Led_Id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Weaver.Text)

        Wgt_Bag = 0 : Wgt_Cn = 0 : Cn_bag = 0



        If Lm_No <> 0 And Led_Id <> 0 Then

            Da = New SqlClient.SqlDataAdapter("select * from Weaver_LoomnO_Details where Ledger_idno = " & Str(Val(Led_Id)) & " and Loom_No = " & Str(Val(Lm_No)), con)
            Da.Fill(Dt)

            With dgv_Details

                If Dt.Rows.Count > 0 Then
                    If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                        .Rows(.CurrentRow.Index).Cells(2).Value = Common_Procedures.EndsCount_IdNoToName(con, Val(Dt.Rows(0).Item("EndsCount_IdNo").ToString))
                        .Rows(.CurrentRow.Index).Cells(4).Value = "1"
                        .Rows(.CurrentRow.Index).Cells(5).Value = Dt.Rows(0).Item("Noof_Bobin").ToString
                        
                    End If
                End If

                Dt.Clear()
                Dt.Dispose()
                Da.Dispose()

                

            End With

        End If

    End Sub

    Private Sub txt_Remarks_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Remarks.KeyDown
        If (e.KeyValue = 38) Then
            If dgv_Details.Rows.Count > 0 Then


                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
            Else
                cbo_Weaver.Focus()
            End If
        End If
        If (e.KeyValue = 40) Then

            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                msk_Date.Focus()
            End If
        End If
    End Sub

    Private Sub txt_Remarks_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Remarks.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                msk_Date.Focus()
            End If
        End If
    End Sub

   
End Class