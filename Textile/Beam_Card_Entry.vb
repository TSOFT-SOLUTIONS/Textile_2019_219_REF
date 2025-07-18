Public Class Beam_Card_Entry
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "JWORD-"

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
    Private vcbo_KeyDwnVal As Double
    Private Prec_ActCtrl As New Control
    Public vmskOldText As String = ""
    Public vmskSelStrt As Integer = -1

    Public Total_rolls As Integer = 0

    Private Sub clear()
        New_Entry = False
        Insert_Entry = False

        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        vmskOldText = ""
        vmskSelStrt = -1

        lbl_JobNo.Text = ""
        lbl_JobNo.ForeColor = Color.Black
        msk_Date.Text = ""
        dtp_Date.Text = ""

        cbo_Ledger.Text = ""
        cbo_Sizing.Text = ""
        cbo_Cloth_Name.Text = ""
        cbo_SetNo.Text = ""
        cbo_LoomNo.Text = ""
        cbo_EndsCount.Text = ""
        txt_BeamMeters.Text = ""
        txt_BeamNo.Text = ""
        lbl_Crimp.Text = ""
        lbl_BalanceMtrs.Text = ""
        cbo_beamNo.Text = ""
        cbo_Beam_Meters.Text = ""
        dgv_Details.Rows.Clear()

        Grid_DeSelect()


        cbo_beamNo.Items.Clear()


        cbo_Beam_Meters.Items.Clear()


        'cbo_Cloth.Visible = False

        'cbo_Cloth.Tag = -1

        'cbo_Cloth.Text = ""



    End Sub
    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox
        Dim msktxbx As MaskedTextBox
        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Me.ActiveControl Is Button Or TypeOf Me.ActiveControl Is MaskedTextBox Then
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

        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)

        On Error Resume Next

        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Or TypeOf Prec_ActCtrl Is MaskedTextBox Then
                Prec_ActCtrl.BackColor = Color.White
                Prec_ActCtrl.ForeColor = Color.Black
            ElseIf TypeOf Prec_ActCtrl Is Button Then
                Prec_ActCtrl.BackColor = Color.FromArgb(41, 57, 85)
                Prec_ActCtrl.ForeColor = Color.White
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
        If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
        dgv_Details.CurrentCell.Selected = False
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

        'Try



        da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name , c.EndsCount_Name , d.Cloth_Name , e.Loom_Name , f.Ledger_Name as Sizer_Name  from Beam_Card_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo Left Outer Join Endscount_head c On a.Endscount_Idno = c.Endscount_Idno Left Outer Join Cloth_Head d On d.Cloth_idno = a.Cloth_Idno Left Outer Join Loom_Head e On e.Loom_Idno = a.Loom_Idno Left Outer join Ledger_Head f on f.Ledger_idno = a.Sizer_Idno Where a.Beam_Card_Code = '" & Trim(NewCode) & "'", con)
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then
                lbl_JobNo.Text = dt1.Rows(0).Item("Beam_Card_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Beam_Card_Date").ToString
                msk_Date.Text = dtp_Date.Text
                cbo_Ledger.Text = dt1.Rows(0).Item("Ledger_Name").ToString
                cbo_Sizing.Text = dt1.Rows(0).Item("Sizer_Name").ToString
                cbo_EndsCount.Text = dt1.Rows(0).Item("EndsCount_Name").ToString
                cbo_Cloth_Name.Text = dt1.Rows(0).Item("Cloth_Name").ToString
                cbo_SetNo.Text = dt1.Rows(0).Item("Set_No").ToString

                cbo_beamNo.Text = dt1.Rows(0).Item("Beam_No").ToString
            txt_BeamMeters.Text = dt1.Rows(0).Item("Beam_Meters").ToString


            cbo_LoomNo.Text = dt1.Rows(0).Item("Loom_Name").ToString
            'txt_BeamMeters.Text = Format(Val(dt1.Rows(0).Item("Beam_Meters").ToString), "########0.00")
            'txt_BeamNo.Text = dt1.Rows(0).Item("Beam_No").ToString
            lbl_Crimp.Text = dt1.Rows(0).Item("Crimp_Perc").ToString
                lbl_BalanceMtrs.Text = Format(Val(dt1.Rows(0).Item("Balance_Meters").ToString), "########0.00")

                da2 = New SqlClient.SqlDataAdapter("select a.* from Beam_Card_Details a  where a.Beam_Card_Code = '" & Trim(NewCode) & "' Order by a.Sl_No", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_Details.Rows.Clear()
                SNo = 0

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_Details.Rows.Add()

                        SNo = SNo + 1
                        dgv_Details.Rows(n).Cells(0).Value = Val(SNo)
                        dgv_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Roll_No").ToString
                        dgv_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Shift_Date").ToString
                        dgv_Details.Rows(n).Cells(3).Value = Format(Val(dt2.Rows(i).Item("Inspection_Meters").ToString), "########0.00")


                    Next i
                    With dgv_Details_Total
                        If .RowCount = 0 Then .Rows.Add()

                        .Rows(0).Cells(3).Value = Format(Val(dt1.Rows(0).Item("Total_Inspec_Meters").ToString), "########0.00")

                    End With
                End If




                Grid_DeSelect()

                dt2.Clear()


                dt2.Dispose()
                da2.Dispose()

            End If

            dt1.Clear()
            dt1.Dispose()
            da1.Dispose()
        'Catch ex As Exception
        '    MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        'End Try

        If msk_Date.Visible And msk_Date.Enabled Then msk_Date.Focus()

    End Sub

    Private Sub JobWork_Order_Entry_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated


        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Ledger.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Ledger.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If



            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Cloth_Name.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CLOTH" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Cloth_Name.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_EndsCount.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "ENDSCOUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_EndsCount.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            'If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Cloth_Name.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CLOTH" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
            '    cbo_Cloth_Name.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            'End If


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

    Private Sub JobWork_Order_Entry_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
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

        'da = New SqlClient.SqlDataAdapter("select Cloth_Name from Cloth_Head order by Cloth_Name", con)
        'da.Fill(dt2)
        'cbo_Cloth.DataSource = dt2
        'cbo_Cloth.DisplayMember = "Cloth_Name"


        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead Where (ledger_type = 'JOBWORKER' or Ledger_IdNo = 0) order by Ledger_DisplayName", con)
        da.Fill(dt6)
        cbo_Ledger.DataSource = dt6
        cbo_Ledger.DisplayMember = "Ledger_DisplayName"
        'get_Data_From_Sizing_Specification()
        'cbo_Cloth.Visible = False

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2

        btn_UserModification.Visible = False
        If Common_Procedures.settings.User_Modifications_Show_Status = 1 Then
            If Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_UserModification_Status = 1 Then
                btn_UserModification.Visible = True
            End If
        End If

        AddHandler msk_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Ledger.GotFocus, AddressOf ControlGotFocus
        'AddHandler cbo_Cloth.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Sizing.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Cloth_Name.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_SetNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_LoomNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_EndsCount.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_BeamMeters.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_BeamNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Beam_Meters.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_beamNo.GotFocus, AddressOf ControlGotFocus


        AddHandler cbo_Beam_Meters.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_beamNo.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Ledger.LostFocus, AddressOf ControlLostFocus
        'AddHandler cbo_Cloth.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Sizing.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_SetNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Cloth_Name.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_LoomNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_EndsCount.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_BeamMeters.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_BeamNo.LostFocus, AddressOf ControlLostFocus

        AddHandler msk_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_BeamMeters.KeyDown, AddressOf TextBoxControlKeyDown
        '  AddHandler txt_BeamNo.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler msk_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_BeamMeters.KeyPress, AddressOf TextBoxControlKeyPress
        '   AddHandler txt_BeamNo.KeyPress, AddressOf TextBoxControlKeyPress


        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True
        new_record()

    End Sub

    Private Sub JobWork_Order_Entry_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub JobWork_Order_Entry_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

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
                                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                                    save_record()
                                Else
                                    msk_Date.Focus()
                                End If

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
                                cbo_Ledger.Focus()

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
        Dim vOrdByNo As String = ""

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_JobNo.Text)

        '   If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.JobWork_Order_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.JobWork_Order_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub
        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_JobNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)


        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.Jobwork_Order_Entry, New_Entry, Me, con, "Beam_Card_Head", "Beam_Card_Code", NewCode, "Beam_Card_Date", "(Beam_Card_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub








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

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_JobNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.Transaction = trans


            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "DELETE", "Beam_Card_Head", "Beam_Card_Code", Val(lbl_Company.Tag), NewCode, lbl_JobNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "", "", "Beam_Card_Code, Company_IdNo, for_OrderBy", trans)
            Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "DELETE", "Beam_Card_Details", "Beam_Card_Code", Val(lbl_Company.Tag), NewCode, lbl_JobNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "Cloth_Idno,Meters", "Sl_No", "Beam_Card_Code, For_OrderBy, Company_IdNo, Beam_Card_No, Beam_Card_Date, Ledger_Idno", trans)


            cmd.CommandText = "delete from Beam_Card_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Beam_Card_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()



            cmd.CommandText = "delete from Beam_Card_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Beam_Card_Code = '" & Trim(NewCode) & "'"
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

            da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or Ledger_Type = 'JOBWORKER') order by Ledger_DisplayName", con)
            da.Fill(dt1)
            cbo_Filter_PartyName.DataSource = dt1
            cbo_Filter_PartyName.DisplayMember = "Ledger_DisplayName"

            dtp_Filter_Fromdate.Text = ""
            dtp_Filter_ToDate.Text = ""
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
        Dim movno As String = ""

        Try

            da = New SqlClient.SqlDataAdapter("select top 1 Beam_Card_No from Beam_Card_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Beam_Card_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Beam_Card_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_JobNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Beam_Card_No from Beam_Card_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Beam_Card_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Beam_Card_No", con)
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

        'Try

        OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_JobNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Beam_Card_No from Beam_Card_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Beam_Card_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Beam_Card_No desc", con)
            da.Fill(dt)

            movno = ""
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = dt.Rows(0)(0).ToString
                End If
            End If

            If Val(movno) <> 0 Then move_record(movno)

        'Catch ex As Exception
        '    MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        'End Try

    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""

        Try
            da = New SqlClient.SqlDataAdapter("select top 1 Beam_Card_No from Beam_Card_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Beam_Card_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Beam_Card_No desc", con)
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

            lbl_JobNo.Text = Common_Procedures.get_MaxCode(con, "Beam_Card_Head", "Beam_Card_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

            lbl_JobNo.ForeColor = Color.Red


            dtp_Date.Text = Date.Today.ToShortDateString
            da = New SqlClient.SqlDataAdapter("select top 1 * from Beam_Card_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Beam_Card_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Beam_Card_No desc", con)
            dt1 = New DataTable
            da.Fill(dt1)
            If dt1.Rows.Count > 0 Then
                If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)
                    If dt1.Rows(0).Item("Beam_Card_Date").ToString <> "" Then dtp_Date.Text = dt1.Rows(0).Item("Beam_Card_Date").ToString
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

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Beam_Card_No from Beam_Card_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Beam_Card_Code = '" & Trim(RecCode) & "'", con)
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

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.JobWork_Order_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.JobWork_Order_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.Jobwork_Order_Entry, New_Entry, Me) = False Then Exit Sub



        Try

            inpno = InputBox("Enter New Job No.", "FOR NEW RECEIPT INSERTION...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Beam_Card_No from Beam_Card_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Beam_Card_Code = '" & Trim(RecCode) & "'", con)
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
                    MessageBox.Show("Invalid Job No", "DOES NOT INSERT NEW RECEIPT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_JobNo.Text = Trim(UCase(inpno))

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
        Dim Clt_ID As Integer = 0
        Dim Itfp_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim Partcls As String = ""
        Dim PBlNo As String = ""
        Dim vTotPcs As Single, vTotMtrs As Single
        Dim Proc_ID As Integer = 0
        Dim Lot_ID As Integer = 0
        Dim Tr_ID As Integer = 0
        Dim vOrdByNo As String = ""

        Dim EndsCnt_id As Integer = 0
        Dim Clo_ID As Integer = 0
        Dim Lm_ID As Integer = 0
        Dim Siz_ID As Integer = 0


        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_JobNo.Text)

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        '  If Common_Procedures.UserRight_Check(Common_Procedures.UR.JobWork_Order_Entry, New_Entry) = False Then Exit Sub

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_JobNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)


        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Jobwork_Order_Entry, New_Entry, Me, con, "Beam_Card_Head", "Beam_Card_Code", NewCode, "Beam_Card_Date", "(Beam_Card_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Beam_Card_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, Beam_Card_No desc", dtp_Date.Value.Date) = False Then Exit Sub





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

        Led_ID = Common_Procedures.Ledger_NameToIdNo(con, cbo_Ledger.Text)
        If Led_ID = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
            Exit Sub
        End If

        EndsCnt_id = Common_Procedures.EndsCount_NameToIdNo(con, cbo_EndsCount.Text)
        Clo_ID = Common_Procedures.Cloth_NameToIdNo(con, cbo_Cloth_Name.Text)
        Lm_ID = Common_Procedures.Loom_NameToIdNo(con, cbo_LoomNo.Text)
        Siz_ID = Common_Procedures.Ledger_NameToIdNo(con, cbo_Sizing.Text)


        'With dgv_Details
        '    For i = 0 To dgv_Details.RowCount - 1

        '        If Val(.Rows(i).Cells(3).Value) = 0 Then
        '            MessageBox.Show("Invalid Meters", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '            If .Enabled And .Visible Then
        '                .Focus()
        '                .CurrentCell = .Rows(i).Cells(3)
        '                .CurrentCell.Selected = True
        '            End If
        '            Exit Sub
        '        End If

        '    Next
        'End With

        Total_Calculation()
        vTotPcs = 0
        If dgv_Details_Total.RowCount > 0 Then

            vTotMtrs = Val(dgv_Details_Total.Rows(0).Cells(3).Value())

        End If

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_JobNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_JobNo.Text = Common_Procedures.get_MaxCode(con, "Beam_Card_Head", "Beam_Card_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_JobNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@JobWork", Convert.ToDateTime(msk_Date.Text))

            If New_Entry = True Then
                cmd.CommandText = "Insert into Beam_Card_Head(Beam_Card_Code, Company_IdNo, Beam_Card_No, for_OrderBy, Beam_Card_Date, Ledger_IdNo,  Total_Inspec_Meters , Sizer_Idno , Cloth_Idno , Set_No , Loom_Idno  , EndsCount_Idno , Beam_Meters , Beam_No ,  Crimp_Perc , Balance_Meters ) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_JobNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_JobNo.Text))) & ", @JobWork, " & Str(Val(Led_ID)) & "," & Str(Val(vTotMtrs)) & " , " & Str(Val(Siz_ID)) & " , " & Str(Val(Clo_ID)) & " , '" & Trim(cbo_SetNo.Text) & "' , " & Str(Val(Lm_ID)) & " , " & Str(Val(EndsCnt_id)) & " , " & Val(txt_BeamMeters.Text) & " , '" & Trim(cbo_beamNo.Text) & "' , " & Str(Val(lbl_Crimp.Text)) & "  , " & Str(Val(lbl_BalanceMtrs.Text)) & " )"
                cmd.ExecuteNonQuery()

            Else
                Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "OLD", "Beam_Card_Head", "Beam_Card_Code", Val(lbl_Company.Tag), NewCode, lbl_JobNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Beam_Card_Code, Company_IdNo, for_OrderBy", tr)

                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "OLD", "Beam_Card_Details", "Beam_Card_Code", Val(lbl_Company.Tag), NewCode, lbl_JobNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "Cloth_Idno,Meters", "Sl_No", "Beam_Card_Code, For_OrderBy, Company_IdNo, Beam_Card_No, Beam_Card_Date, Ledger_Idno", tr)

                cmd.CommandText = "Update Beam_Card_Head set Beam_Card_Date = @JobWork, Ledger_IdNo = " & Val(Led_ID) & ", Total_Inspec_Meters = " & Val(vTotMtrs) & " , Sizer_Idno = " & Str(Val(Siz_ID)) & ", Cloth_Idno = " & Str(Val(Clo_ID)) & "  , Set_No  = '" & Trim(cbo_SetNo.Text) & "'  , Loom_Idno =  " & Str(Val(Lm_ID)) & " , EndsCount_Idno = " & Str(Val(EndsCnt_id)) & "  , Beam_No = '" & Trim(cbo_beamNo.Text) & "'  , Beam_Meters = " & Val(txt_BeamMeters.Text) & " ,  Crimp_Perc = " & Str(Val(lbl_Crimp.Text)) & " , Balance_Meters =  " & Str(Val(lbl_BalanceMtrs.Text)) & "   Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Beam_Card_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If
            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "NEW", "Beam_Card_Head", "Beam_Card_Code", Val(lbl_Company.Tag), NewCode, lbl_JobNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Beam_Card_Code, Company_IdNo, for_OrderBy", tr)



            cmd.CommandText = "Delete from Beam_Card_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Beam_Card_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            'cmd.CommandText = "Delete from Stock_Item_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            'cmd.ExecuteNonQuery()

            ''Partcls = "Rec : Dc.No. " & Trim(lbl_JobNo.Text)
            ''PBlNo = Trim(lbl_JobNo.Text)

            With dgv_Details
                Sno = 0
                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(3).Value) <> 0 Then
                        Sno = Sno + 1

                        '  Clt_ID = Common_Procedures.Cloth_NameToIdNo(con, .Rows(i).Cells(1).Value, tr)


                        Sno = Sno + 1

                        If Trim(.Rows(i).Cells(2).Value) <> "" Then
                            If IsDate(.Rows(i).Cells(2).Value) = True Then
                                cmd.Parameters.AddWithValue("@ShiftDate", CDate(.Rows(i).Cells(2).Value))
                            End If
                        End If

                        cmd.CommandText = "Insert into Beam_Card_Details(Beam_Card_Code, Company_IdNo, Beam_Card_No, for_OrderBy, Beam_Card_Date,Ledger_IdNo,Sl_No, Roll_No ,  Shift_Date , Shift_DateTime  ,  Inspection_Meters ) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_JobNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_JobNo.Text))) & ", @JobWork," & Val(Led_ID) & " , " & Str(Val(Sno)) & ", '" & Trim(.Rows(i).Cells(1).Value) & "' , '" & Trim(.Rows(i).Cells(2).Value) & "' ,  " & IIf(IsDate(.Rows(i).Cells(2).Value) = True, "@ShiftDate", "Null") & " ,   " & Val(.Rows(i).Cells(3).Value) & ")"
                        cmd.ExecuteNonQuery()
                        ''If Val(vTotWeight) > 0 Then
                        'cmd.CommandText = "Insert into Stock_Item_Processing_Details(Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date,Ledger_IdNo, Item_IdNo,Colour_IdNo,Rack_IdNo, Party_Bill_No, Sl_No, Particulars,Quantity,Meters) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_JobNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_JobNo.Text))) & ", @JobWork, " & Str(Val(Led_ID)) & "," & Val(Itfp_ID) & "," & Val(Col_ID) & ", 0, '" & Trim(PBlNo) & "', 1,'" & Trim(Partcls) & "' ," & Val(.Rows(i).Cells(7).Value) & ", " & Str(Val(.Rows(i).Cells(8).Value)) & " )"
                        'cmd.ExecuteNonQuery()
                        '' End If

                    End If
                    Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "NEW", "Beam_Card_Details", "Beam_Card_Code", Val(lbl_Company.Tag), NewCode, lbl_JobNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "Cloth_Idno,Meters", "Sl_No", "Beam_Card_Code, For_OrderBy, Company_IdNo, Beam_Card_No, Beam_Card_Date, Ledger_Idno", tr)

                Next

            End With

            tr.Commit()

            Dt1.Dispose()
            Da.Dispose()
            If New_Entry = True Then new_record()

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If msk_Date.Visible And msk_Date.Enabled Then msk_Date.Focus()

    End Sub



    Private Sub Total_Calculation()
        Dim vTotMtrs As Single

        Dim i As Integer
        Dim sno As Integer


        vTotMtrs = 0 : sno = 0
        With dgv_Details
            For i = 0 To dgv_Details.Rows.Count - 1

                sno = sno + 1

                .Rows(i).Cells(0).Value = sno

                If Val(dgv_Details.Rows(i).Cells(3).Value) <> 0 Then
                    '.Rows(i).Cells(9).Value = Val(dgv_Details.Rows(i).Cells(7).Value) * Val(dgv_Details.Rows(i).Cells(8).Value)


                    vTotMtrs = vTotMtrs + Val(dgv_Details.Rows(i).Cells(3).Value)


                End If
            Next
            'If Val(dgv_Details_Total.Rows(0).Cells(3).Value) <> 0 Then
            lbl_BalanceMtrs.Text = Val(txt_BeamMeters.Text) - Val(vTotMtrs)
            'End If

        End With




        If Val(txt_BeamMeters.Text) <> 0 Or Val(lbl_BalanceMtrs.Text) <> 0 Then
            lbl_Crimp.Text = Format((Val(lbl_BalanceMtrs.Text) * 100) / Val(txt_BeamMeters.Text).ToString, "########0.00")
        End If

        If dgv_Details_Total.Rows.Count <= 0 Then dgv_Details_Total.Rows.Add()

        dgv_Details_Total.Rows(0).Cells(3).Value = Format(Val(vTotMtrs), "#########0.00")

    End Sub

    Private Sub dtp_Date_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        With dtp_Date
            .BackColor = Color.Lime
            .ForeColor = Color.Blue
        End With
    End Sub
    Private Sub dtp_date_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        dtp_Date.BackColor = Color.White
        dtp_Date.ForeColor = Color.Black
    End Sub

    Private Sub dtp_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyCode = 40 Then e.SuppressKeyPress = True : SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then e.SuppressKeyPress = True : SendKeys.Send("+{TAB}")
    End Sub

    Private Sub dtp_Date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Asc(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub




    Private Sub cbo_Ledger_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyDown

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, msk_Date, cbo_Sizing, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = 'WEAVER'  or Ledger_Type = 'GODOWN'  or Ledger_Type = 'JOBWORKER' or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_Ledger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ledger.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Ledger, cbo_Sizing, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = 'WEAVER'  or Ledger_Type = 'GODOWN'  or Ledger_Type = 'JOBWORKER' or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Ledger_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Common_Procedures.UR.Ledr_Wea_Siz_Rw_Trans_JbWrk_Creation = Common_Procedures.UR.Ledger_Creation
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




    Private Sub dgv_Details_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEndEdit
        With dgv_Details

            If .CurrentCell.ColumnIndex = 3 Then
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
        Dim Dt4 As New DataTable
        Dim rect As Rectangle

        With dgv_Details
            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If






            'If e.ColumnIndex = 1 Then

            '    If cbo_Cloth.Visible = False Or Val(cbo_Cloth.Tag) <> e.RowIndex Then

            '        cbo_Cloth.Tag = -1
            '        Da = New SqlClient.SqlDataAdapter("select Cloth_Name from Cloth_Head order by Cloth_Name", con)
            '        Dt2 = New DataTable
            '        Da.Fill(Dt2)
            '        cbo_Cloth.DataSource = Dt2
            '        cbo_Cloth.DisplayMember = "Cloth_Name"

            '        rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

            '        cbo_Cloth.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
            '        cbo_Cloth.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
            '        cbo_Cloth.Width = rect.Width  ' .CurrentCell.Size.Width
            '        cbo_Cloth.Height = rect.Height  ' rect.Height

            '        cbo_Cloth.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

            '        cbo_Cloth.Tag = Val(e.RowIndex)
            '        cbo_Cloth.Visible = True

            '        cbo_Cloth.BringToFront()
            '        cbo_Cloth.Focus()

            '        'cbo_Grid_CountName.Visible = False
            '        'cbo_Grid_MillName.Visible = False

            '    End If

            'Else

            '    cbo_Cloth.Visible = False
            '    'cbo_Grid_MillName.Tag = -1
            '    'cbo_Grid_MillName.Text = ""

            'End If




        End With
    End Sub

    Private Sub dgv_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellLeave
        With dgv_Details

            If .CurrentCell.ColumnIndex = 3 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                End If
            End If
        End With
    End Sub

    Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged
        On Error Resume Next
        If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
        With dgv_Details
            If .Visible Then
                If .CurrentCell.ColumnIndex = 3 Then
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
                    cbo_Ledger.Focus()

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
                        Else
                            msk_Date.Focus()
                        End If
                    End If
                End If
            End If







        End With
    End Sub
    Private Sub dgv_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Details.EditingControlShowing
        dgtxt_details = CType(dgv_Details.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgtxt_details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_details.Enter
        dgv_Details.EditingControl.BackColor = Color.Lime
    End Sub

    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_details.KeyPress

        With dgv_Details

            If Val(dgv_Details.CurrentCell.ColumnIndex.ToString) = 2 And Val(dgv_Details.CurrentCell.ColumnIndex.ToString) = 3 Then

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

                For i = 0 To .Rows.Count - 1
                    .Rows(i).Cells(0).Value = i + 1
                Next

            End With

            Total_Calculation()

        End If


    End Sub

    Private Sub dgv_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.LostFocus
        On Error Resume Next
        If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
        dgv_Details.CurrentCell.Selected = False
        With dgv_Details
            .BackColor = Color.White
            .ForeColor = Color.Black
        End With
    End Sub

    Private Sub dgv_Details_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Details.RowsAdded
        Dim n As Integer

        With dgv_Details
            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)
        End With
    End Sub





    Private Sub cbo_Cloth_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")

        Catch ex As Exception

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
        Dim Led_IdNo As Integer
        Dim Condt As String = ""


        Try

            Condt = ""
            Led_IdNo = 0


            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Beam_Card_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Beam_Card_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Beam_Card_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If



            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.Ledger_IdNo = " & Str(Val(Led_IdNo))
            End If





            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name from Beam_Card_Head a INNER JOIN Ledger_Head b on a.Ledger_IdNo = b.Ledger_IdNo  where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Beam_Card_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Beam_Card_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()


                    dgv_Filter_Details.Rows(n).Cells(0).Value = dt2.Rows(i).Item("Beam_Card_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(1).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Beam_Card_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(3).Value = Format(Val(dt2.Rows(i).Item("Total_Inspec_Meters").ToString), "########0.00")

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

    Private Sub dtp_Filter_Fromdate_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_Filter_Fromdate.GotFocus
        With dtp_Filter_Fromdate
            .BackColor = Color.Lime
            .ForeColor = Color.Blue
        End With
    End Sub

    Private Sub dtp_Filter_Fromdate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Filter_Fromdate.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub dtp_Filter_Fromdate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_Filter_Fromdate.KeyPress
        If Asc(e.KeyChar) = 13 Then
            dtp_Filter_ToDate.Focus()
        End If

    End Sub

    Private Sub dtp_Filter_ToDate_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_Filter_ToDate.GotFocus
        With dtp_Filter_ToDate
            .BackColor = Color.Lime
            .ForeColor = Color.Blue
        End With
    End Sub

    Private Sub dtp_Filter_ToDate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Filter_ToDate.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub dtp_Filter_ToDate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_Filter_ToDate.KeyPress
        If Asc(e.KeyChar) = 13 Then
            cbo_Filter_PartyName.Focus()
        End If
    End Sub

    Private Sub cbo_Filter_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_PartyName.GotFocus
        With cbo_Filter_PartyName
            .BackColor = Color.Lime
            .ForeColor = Color.Blue
            .SelectionStart = 0
            .SelectionLength = .Text.Length
        End With
    End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        Try
            With cbo_Filter_PartyName
                If e.KeyValue = 38 And .DroppedDown = False Then
                    e.Handled = True
                    dtp_Filter_ToDate.Focus()
                    'SendKeys.Send("+{TAB}")
                ElseIf e.KeyValue = 40 And .DroppedDown = False Then
                    e.Handled = True
                    btn_Filter_Show.Focus()
                    'SendKeys.Send("{TAB}")
                ElseIf e.KeyValue <> 13 And .DroppedDown = False Then
                    .DroppedDown = True
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Condt As String
        Dim FindStr As String

        With cbo_Filter_PartyName

            If Asc(e.KeyChar) = 13 Then

                If Trim(.Text) <> "" Then
                    If .DroppedDown = True Then
                        If Trim(.SelectedText) <> "" Then
                            .Text = .SelectedText
                        Else
                            If .Items.Count > 0 Then
                                .SelectedIndex = 0
                                .SelectedItem = .Items(0)
                                .Text = .GetItemText(.SelectedItem)
                            End If
                        End If
                    End If
                End If
                btn_Filter_Show_Click(sender, e)



            Else

                Condt = ""
                FindStr = ""

                If Asc(e.KeyChar) = 8 Then
                    If .SelectionStart <= 1 Then
                        .Text = ""
                    End If

                    If Trim(.Text) <> "" Then
                        If .SelectionLength = 0 Then
                            FindStr = .Text.Substring(0, .Text.Length - 1)
                        Else
                            FindStr = .Text.Substring(0, .SelectionStart - 1)
                        End If
                    End If

                Else
                    If .SelectionLength = 0 Then
                        FindStr = .Text & e.KeyChar
                    Else
                        FindStr = .Text.Substring(0, .SelectionStart) & e.KeyChar
                    End If

                End If
                FindStr = LTrim(FindStr)

                Condt = "where (ledger_idno = 0 or Ledger_Type = 'JOBWORKER')"
                If Trim(FindStr) <> "" Then
                    Condt = " where (Ledger_Type = 'JOBWORKER') and (Ledger_DisplayName like '" & FindStr & "%' or Ledger_DisplayName like '% " & FindStr & "%')"
                End If

                da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead  " & Condt & " Order by Ledger_DisplayName", con)
                da.Fill(dt)

                .DataSource = dt
                .DisplayMember = "Ledger_DisplayName"

                .Text = Trim(FindStr)

                .SelectionStart = FindStr.Length

                e.Handled = True

            End If

        End With

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


    Private Sub cbo_Filter_PartyName_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_PartyName.LostFocus
        With cbo_Filter_PartyName
            .BackColor = Color.White
            .ForeColor = Color.Black
        End With
    End Sub


    Public Sub print_record() Implements Interface_MDIActions.print_record
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_JobNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.PrintEntry, Common_Procedures.UR.Jobwork_Order_Entry, New_Entry) = False Then Exit Sub

        Try

            da1 = New SqlClient.SqlDataAdapter("select * from Beam_Card_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Beam_Card_Code = '" & Trim(NewCode) & "'", con)
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


        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_JobNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.Ledger_Name as Sizing_Name,d.Ledger_Name as Weaver_Name ,e.endscount_name as EndsCount ,cl.cloth_name as cloth from Beam_Card_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo INNER JOIN Ledger_Head c ON a.sizer_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Ledger_Head d ON a.Ledger_idno = d.Ledger_IdNo LEFT OUTER JOIN EndsCount_Head E ON a.EndsCount_IdNo = E.EndsCount_IdNo LEFT OUTER JOIN cloth_Head cl ON a.cloth_IdNo = cl.cloth_IdNo  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Beam_Card_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then
                da2 = New SqlClient.SqlDataAdapter("select * from  Beam_Card_Details  where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Beam_Card_Code = '" & Trim(NewCode) & "' Order by sl_no", con)
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
            .Left = 15
            .Right = 45
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

        NoofItems_PerPage = 28 ' 6

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = 100 : ClArr(2) = 200 : ClArr(3) = 200
        ClArr(4) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3))

        'ClArr(1) = Val(35) : ClArr(2) = 160 : ClArr(3) = 100 : ClArr(4) = 100 : ClArr(5) = 120 : ClArr(6) = 70 : ClArr(7) = 80
        'ClArr(8) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7))

        TxtHgt = 20 ' 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20
        Total_rolls = 0
        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_JobNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

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

                        'ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Processed_Grey_Name").ToString)
                        'ItmNm2 = ""
                        'If Len(ItmNm1) > 18 Then
                        '    For I = 18 To 1 Step -1
                        '        If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                        '    Next I
                        '    If I = 0 Then I = 18
                        '    ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                        '    ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                        'End If

                        CurY = CurY + TxtHgt
                        SNo = SNo + 1
                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(SNo)), LMargin + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Roll_No").ToString, LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_DetDt.Rows(prn_DetIndx).Item("Shift_DateTime").ToString), "dd-MM-yyyy").ToString, LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                        If Trim(prn_DetDt.Rows(prn_DetIndx).Item("Shift_Date").ToString) <> "" Then
                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Shift_Date").ToString), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                        End If

                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Inspection_Meters").ToString), "########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 10, CurY, 1, 0, pFont)

                        NoofDets = NoofDets + 1



                        prn_DetIndx = prn_DetIndx + 1
                        Total_rolls = Total_rolls + 1


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
        Dim mill_nm As String = ""

        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*,b.set_no, c.Mill_Name as mill from Sizing_SpecificationYarn_Details a LEFT OUTER JOIN Sizing_Specification_Head b on a.Sizing_Specification_Code = b.Sizing_Specification_Code  LEFT OUTER JOIN Mill_Head c on a.Mill_IdNo = c.Mill_IdNo where b.set_Code ='" & Trim(cbo_SetNo.Text) & "'", con)
        da2.Fill(dt2)

        If dt2.Rows.Count > 0 Then
            mill_nm = dt2.Rows(0).Item("mill").ToString

        End If
        'Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        'End If
        'dt2.Clear()

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
        Common_Procedures.Print_To_PrintDocument(e, "BEAM CARD", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height



        CurY = CurY + strHeight + 10 ' + 150
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try
            C1 = ClAr(1) + ClAr(2) + ClAr(3) - 50
            W1 = e.Graphics.MeasureString(" BEAM METERS   : ", pFont).Width
            S1 = e.Graphics.MeasureString("WEAVER NAME  :    ", pFont).Width

            CurY = CurY + TxtHgt - 10
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "CARD NO             :  " & prn_HdDt.Rows(0).Item("Beam_Card_No").ToString, LMargin + 10, CurY, 0, 0, p1Font)

            Common_Procedures.Print_To_PrintDocument(e, "SET NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            If dt2.Rows.Count > 0 Then
                Common_Procedures.Print_To_PrintDocument(e, dt2.Rows(0).Item("Set_no").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            End If
            dt2.Clear()

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " :   " & Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Beam_Card_Date").ToString), "dd-MM-yyyy").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

            p1Font = New Font("Calibri", 14, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "ENDS", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("EndsCount").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, "WEAVER NAME  ", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " :   " & prn_HdDt.Rows(0).Item("Weaver_name").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "MILL ", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(mill_nm), LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, "SIZING NAME  ", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " :   " & prn_HdDt.Rows(0).Item("Sizing_name").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "BEAM METERS   ", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("BEAM_Meters").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            If (prn_HdDt.Rows(0).Item("cloth").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "CLOTH NAME   ", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " :   " & prn_HdDt.Rows(0).Item("cloth").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

            End If

            If (prn_HdDt.Rows(0).Item("loom_idno").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, " LOOM NO ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " :   " & Common_Procedures.Loom_NameToIdNo(con, prn_HdDt.Rows(0).Item("loom_idno").ToString), LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
            End If
            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + C1, LnAr(3), LMargin + C1, LnAr(2))

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "S.NO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "BEAM NO", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "INSPECTION METER", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)

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
                Common_Procedures.Print_To_PrintDocument(e, " TOTAL", LMargin + 10, CurY, 0, 0, pFont)
            End If

            If Val(Total_rolls) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, Total_rolls, LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                End If
            End If
            If Val(prn_HdDt.Rows(0).Item("Total_Inspec_Meters").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Inspec_Meters").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
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

            CurY = CurY + TxtHgt - 5

            Common_Procedures.Print_To_PrintDocument(e, "CRIMP % : " & Val(prn_HdDt.Rows(0).Item("CRImp_PERC").ToString), LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 10
            Common_Procedures.Print_To_PrintDocument(e, "BALANCE METER : " & Val(prn_HdDt.Rows(0).Item("Balance_meters").ToString), LMargin + 10, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(7) = CurY

            CurY = CurY + TxtHgt
            If Val(Common_Procedures.User.IdNo) <> 1 Then
                Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
            End If


            CurY = CurY + TxtHgt
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            'Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 20, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 300, CurY, 0, 0, pFont)
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


    Private Sub btn_Print_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        print_record()
    End Sub

    Private Function cbo_Filter_ProcessName() As Object
        Throw New NotImplementedException
    End Function


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
            f1.Entry_PkValue = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_JobNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            f1.ShowDialog()
        End If
    End Sub

    Private Sub cbo_Sizing_GotFocus(sender As Object, e As EventArgs) Handles cbo_Sizing.GotFocus
        cbo_Sizing.Tag = cbo_Sizing.Text
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(ledger_idno = 0 or Ledger_Type = 'SIZING' or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Sizing_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_Sizing.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Sizing, cbo_Ledger, cbo_Cloth_Name, "Ledger_AlaisHead", "Ledger_DisplayName", "(ledger_idno = 0 or Ledger_Type = 'SIZING'  or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Sizing_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_Sizing.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Sizing, cbo_Cloth_Name, "Ledger_AlaisHead", "Ledger_DisplayName", "(ledger_idno = 0 or Ledger_Type = 'SIZING'  or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Sizing_KeyUp(sender As Object, e As KeyEventArgs) Handles cbo_Sizing.KeyUp
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

    Private Sub cbo_EndsCount_GotFocus(sender As Object, e As EventArgs) Handles cbo_EndsCount.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")
    End Sub

    Private Sub cbo_EndsCount_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_EndsCount.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_EndsCount, cbo_LoomNo, txt_BeamMeters, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")
    End Sub

    Private Sub cbo_EndsCount_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_EndsCount.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_EndsCount, txt_BeamMeters, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")
    End Sub

    Private Sub cbo_EndsCount_KeyUp(sender As Object, e As KeyEventArgs) Handles cbo_EndsCount.KeyUp
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

    Private Sub cbo_LoomNo_GotFocus(sender As Object, e As EventArgs) Handles cbo_LoomNo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Loom_Head", "Loom_Name", "", "(Loom_IdNo = 0)")
    End Sub

    Private Sub cbo_LoomNo_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_LoomNo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_LoomNo, cbo_Beam_Meters, Nothing, "Loom_Head", "Loom_Name", "", "(Loom_IdNo = 0)")

        If e.KeyCode = 40 Then

            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
                dgv_Details.Focus()
                dgv_Details.CurrentCell.Selected = True

        End If

    End Sub

    Private Sub cbo_LoomNo_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_LoomNo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_LoomNo, Nothing, "Loom_Head", "Loom_Name", "", "(Loom_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then

            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
                dgv_Details.Focus()
                dgv_Details.CurrentCell.Selected = True

        End If
    End Sub

    Private Sub cbo_LoomNo_KeyUp(sender As Object, e As KeyEventArgs) Handles cbo_LoomNo.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New LoomNo_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_LoomNo.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_SetNo_GotFocus(sender As Object, e As EventArgs) Handles cbo_SetNo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Sizing_Specification_Head", "set_code", "", "(Sizing_Specification_No = 0)")
        'get_Data_From_Sizing_Specification()
    End Sub

    Private Sub cbo_SetNo_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_SetNo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_SetNo, cbo_Cloth_Name, cbo_LoomNo, "Sizing_Specification_Head", "set_code", "", "(Sizing_Specification_No = 0)")
        'get_Data_From_Sizing_Specification()
    End Sub

    Private Sub get_Data_From_Sizing_Specification()
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Cnt As Integer = 0
        Dim n As Integer = 0
        Dim sno As Integer = 0

        da = New SqlClient.SqlDataAdapter("select s.Beam_No,s.Meters,b.EndsCount_Name , a.Total_Beam , a.Total_Meters , * from Sizing_Specification_Head a Left Outer Join Endscount_head b On a.Endscount_Idno = b.Endscount_Idno  Left Outer Join Sizing_SpecificationPavu_Details s On s.Set_code = a.set_code where a.set_code  = '" & Trim(cbo_SetNo.Text) & "' ", con)
        da.Fill(dt)
        dgv_Details.Rows.Clear()

        If Trim(cbo_SetNo.Text) <> "" Then

            If dt.Rows.Count > 0 Then
                cbo_EndsCount.Text = dt.Rows(0).Item("EndsCount_Name").ToString
                txt_BeamMeters.Text = dt.Rows(0).Item("Total_Meters").ToString
                For Cnt = 0 To dt.Rows.Count - 1



                    cbo_Beam_Meters.Items.Add((dt.Rows(Cnt).Item("Meters").ToString))


                    cbo_beamNo.Items.Add((dt.Rows(Cnt).Item("Beam_no").ToString))
                    n = dgv_Details.Rows.Add
                    dgv_Details.Rows(n).Cells(0).Value = Val(n) + 1
                    dgv_Details.Rows(n).Cells(1).Value = dt.Rows(Cnt).Item("Beam_no").ToString

                Next


            End If

            dt.Clear()

            'cbo_EndsCount.Focus()


        End If
    End Sub

    Private Sub cbo_SetNo_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_SetNo.KeyPress


        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_SetNo, cbo_LoomNo, "Sizing_Specification_Head", "set_code", "", "(Sizing_Specification_No = 0)")
        If Asc(e.KeyChar) = 13 Then
            get_Data_From_Sizing_Specification()
        End If

    End Sub

    Private Sub cbo_Cloth_Name_GotFocus(sender As Object, e As EventArgs) Handles cbo_Cloth_Name.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "cloth_name", "", "(cloth_name)")
    End Sub

    Private Sub cbo_Cloth_Name_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_Cloth_Name.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Cloth_Name, cbo_Sizing, cbo_SetNo, "Cloth_Head", "cloth_name", "", "(cloth_name)")
    End Sub

    Private Sub cbo_Cloth_Name_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_Cloth_Name.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Cloth_Name, cbo_SetNo, "Cloth_Head", "cloth_name", "", "(cloth_name)")
    End Sub

    Private Sub cbo_Ledger_GotFocus(sender As Object, e As EventArgs) Handles cbo_Ledger.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'WEAVER'  or Ledger_Type = 'GODOWN'  or Ledger_Type = 'JOBWORKER' or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Cloth_Name_KeyUp(sender As Object, e As KeyEventArgs) Handles cbo_Cloth_Name.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New Cloth_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Cloth_Name.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub txt_BeamNo_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_BeamNo.KeyDown
        If e.KeyCode = 38 Then
            txt_BeamMeters.Focus()
        End If
        If e.KeyCode = 40 Then
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
            dgv_Details.Focus()
            dgv_Details.CurrentCell.Selected = True
        End If
    End Sub

    Private Sub txt_BeamNo_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_BeamNo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
            dgv_Details.Focus()
            dgv_Details.CurrentCell.Selected = True

        End If
    End Sub

    Private Sub txt_BeamMeters_TextChanged(sender As Object, e As EventArgs) Handles txt_BeamMeters.TextChanged
        Total_Calculation()

        'If Val(txt_BeamMeters.Text) <> 0 Or Val(lbl_BalanceMtrs.Text) <> 0 Then
        '    lbl_Crimp.Text = Format(Val(lbl_BalanceMtrs.Text) * 100 / Val(txt_BeamMeters.Text).ToString, "########0.00")
        'End If

    End Sub

    Private Sub cbo_beamNo_GotFocus(sender As Object, e As EventArgs) Handles cbo_beamNo.GotFocus

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")

    End Sub

    Private Sub cbo_beamNo_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_beamNo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_beamNo, cbo_Beam_Meters, cbo_SetNo, "", "", "", "")
    End Sub

    Private Sub cbo_beamNo_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_beamNo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_beamNo, cbo_Beam_Meters, "", "", "", "", True)
    End Sub

    Private Sub cbo_Beam_Meters_GotFocus(sender As Object, e As EventArgs) Handles cbo_Beam_Meters.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")

    End Sub

    Private Sub cbo_Beam_Meters_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_Beam_Meters.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Beam_Meters, cbo_LoomNo, cbo_beamNo, "", "", "", "")
    End Sub

    Private Sub cbo_Beam_Meters_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_Beam_Meters.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Beam_Meters, cbo_LoomNo, "", "", "", "", True)
    End Sub

    Private Sub cbo_SetNo_TextChanged(sender As Object, e As EventArgs) Handles cbo_SetNo.TextChanged
        'get_Data_From_Sizing_Specification()

    End Sub



    '' BalanceMtr * 100 / Beam_Mtrs = Crimp

End Class