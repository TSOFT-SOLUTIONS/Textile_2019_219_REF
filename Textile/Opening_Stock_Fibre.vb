Public Class Opening_Stock_Fibre
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Pk_Condition As String = "OPENI-"
    Private OpYrCode As String = ""
    Private Prec_ActCtrl As New Control

    Private WithEvents dgtxt_FibreDetails As New DataGridViewTextBoxEditingControl
    Private WithEvents dgtxt_PavuDetails As New DataGridViewTextBoxEditingControl
    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl


    Private Sub clear()

        New_Entry = False

        lbl_IdNo.Text = ""
        lbl_IdNo.ForeColor = Color.Black

        pnl_Back.Enabled = True

        lbl_IdNo.Text = ""
        cbo_Ledger.Text = ""
        cbo_Ledger.Tag = ""
        cbo_Ledger.Enabled = False

        cbo_Grid_CountName.Visible = False
        cbo_Grid_MillName.Visible = False


        cbo_Grid_CountName.Text = ""
        cbo_Grid_MillName.Text = ""


        ' tab_Main.SelectTab(0)
        'dgv_FibreDetails.CurrentCell = dgv_FibreDetails.Rows(0).Cells(1)
        'dgv_FibreDetails.CurrentCell.Selected = True
    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox

        On Error Resume Next

        If FrmLdSTS = True Then Exit Sub

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Prec_ActCtrl Is CheckBox Then
            Me.ActiveControl.BackColor = Color.Lime
            Me.ActiveControl.ForeColor = Color.Blue
        ElseIf TypeOf Me.ActiveControl Is CheckBox Then
            Me.ActiveControl.BackColor = Color.Lime
            Me.ActiveControl.ForeColor = Color.Blue
        End If

        If TypeOf Me.ActiveControl Is TextBox Then
            txtbx = Me.ActiveControl
            txtbx.SelectAll()
        ElseIf TypeOf Me.ActiveControl Is ComboBox Then
            combobx = Me.ActiveControl
            combobx.SelectAll()
        End If

        If Me.ActiveControl.Name <> cbo_Grid_CountName.Name Then
            cbo_Grid_CountName.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Grid_MillName.Name Then
            cbo_Grid_MillName.Visible = False
        End If


        Grid_Cell_DeSelect()

        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        On Error Resume Next
        If FrmLdSTS = True Then Exit Sub
        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Then
                Prec_ActCtrl.BackColor = Color.White
                Prec_ActCtrl.ForeColor = Color.Black
            ElseIf TypeOf Prec_ActCtrl Is CheckBox Then
                Prec_ActCtrl.BackColor = Color.LightSkyBlue
                Prec_ActCtrl.ForeColor = Color.Blue
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

    Private Sub Grid_Cell_DeSelect()
        On Error Resume Next
        If Not IsNothing(dgv_FibreDetails.CurrentCell) Then dgv_FibreDetails.CurrentCell.Selected = False

    End Sub

    Private Sub move_record(ByVal idno As Integer)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim Sno As Integer, n As Integer
        Dim NewCode As String

        If Val(idno) = 0 Then Exit Sub

        clear()

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(Val(idno)) & "/" & Trim(OpYrCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.Ledger_IdNo, a.Ledger_Name from Ledger_Head a Where a.Ledger_IdNo = " & Str(Val(idno)) & "", con)
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then
                lbl_IdNo.Text = dt1.Rows(0).Item("Ledger_IdNo").ToString

                cbo_Ledger.Text = dt1.Rows(0).Item("Ledger_Name").ToString
                cbo_Ledger.Tag = cbo_Ledger.Text

                da2 = New SqlClient.SqlDataAdapter("Select sum(voucher_amount) from voucher_details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and ledger_idno = " & Str(Val(idno)) & " and Entry_Identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)
                dt2 = New DataTable
                da2.Fill(dt2)


                dt2.Clear()


                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Fibre_Name, c.Mill_Name , Lh.Ledger_Name as Godown_Name from Stock_Fibre_Processing_Details a INNER JOIN fibre_Head b on a.Fibre_IdNo = b.Fibre_IdNo INNER JOIN Mill_Head c on a.Mill_IdNo = c.Mill_IdNo LEFT JOIN Ledger_Head Lh ON a.WareHouse_IdNo = Lh.Ledger_IdNo where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and (DeliveryTo_Idno = " & Str(Val(idno)) & " or ReceivedFrom_Idno = " & Str(Val(idno)) & ") and a.Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' Order by a.sl_no", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_FibreDetails.Rows.Clear()
                Sno = 0

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_FibreDetails.Rows.Add()

                        Sno = Sno + 1
                        dgv_FibreDetails.Rows(n).Cells(0).Value = Val(Sno)
                        dgv_FibreDetails.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Fibre_Name").ToString
                        dgv_FibreDetails.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Lot_No").ToString
                        dgv_FibreDetails.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Mill_Name").ToString

                        If IsDBNull(dt2.Rows(i).Item("Bale").ToString) = False Then
                            If Val(dt2.Rows(i).Item("ReceivedFrom_Idno").ToString) <> 0 Then
                                dgv_FibreDetails.Rows(n).Cells(4).Value = -1 * Math.Abs(Val(dt2.Rows(i).Item("Bale").ToString))
                            Else
                                dgv_FibreDetails.Rows(n).Cells(4).Value = Math.Abs(Val(dt2.Rows(i).Item("Bale").ToString))
                            End If

                        End If
                        If Val(dgv_FibreDetails.Rows(n).Cells(4).Value) = 0 Then
                            dgv_FibreDetails.Rows(n).Cells(4).Value = ""
                        End If


                        If IsDBNull(dt2.Rows(i).Item("Weight").ToString) = False Then
                            If Val(dt2.Rows(i).Item("ReceivedFrom_Idno").ToString) <> 0 Then
                                dgv_FibreDetails.Rows(n).Cells(5).Value = -1 * Format(Math.Abs(Val(dt2.Rows(i).Item("Weight").ToString)), "########0.000")
                            Else
                                dgv_FibreDetails.Rows(n).Cells(5).Value = Format(Math.Abs(Val(dt2.Rows(i).Item("Weight").ToString)), "########0.000")
                            End If


                        End If




                    Next i

                    'dgv_FibreDetails.CurrentCell.Selected = False

                End If

                TotalYarn_Calculation()

                dt2.Clear()
                dt2.Dispose()
                da2.Dispose()

            End If

            dt1.Clear()
            dt1.Dispose()
            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        If dgv_FibreDetails.Visible And dgv_FibreDetails.Enabled Then dgv_FibreDetails.Focus()

    End Sub

    Private Sub Opening_Stock_Fibre_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Ledger.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Ledger.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_CountName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "FIBRE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_CountName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            'If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_PavuGrid_CountName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "COUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
            '    cbo_PavuGrid_CountName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            'End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_MillName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "MILL" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_MillName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            'If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_beamwidth.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "BEAMWIDTH" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
            '    cbo_Grid_beamwidth.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            'End If

            'If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_VendorName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "VENDOR" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
            '    cbo_Grid_VendorName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            'End If

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

                ' new_record()
                move_record(4)

            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        FrmLdSTS = False

    End Sub

    Private Sub Opening_Stock_Fibre_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim dttm As DateTime

        FrmLdSTS = True

        Me.Text = ""

        dttm = New DateTime(Val(Microsoft.VisualBasic.Left(Trim(Common_Procedures.FnRange), 4)), Microsoft.VisualBasic.DateAndTime.Month(Common_Procedures.Company_FromDate), Microsoft.VisualBasic.DateAndTime.Day(Common_Procedures.Company_FromDate))
        lbl_Heading.Text = "OPENING STOCK    -    AS ON  :  " & dttm.ToShortDateString

        If Val(Common_Procedures.settings.Multi_Godown_Status) = 1 Then
            For i = 0 To dgv_FibreDetails.Rows.Count - 1
                dgv_FibreDetails.Columns(8).ReadOnly = False
            Next
        End If

        con.Open()

        AddHandler cbo_Ledger.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_CountName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_MillName.GotFocus, AddressOf ControlGotFocus

        AddHandler btnSave.GotFocus, AddressOf ControlGotFocus
        AddHandler btnClose.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_Ledger.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_CountName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_MillName.LostFocus, AddressOf ControlLostFocus

        AddHandler btnSave.LostFocus, AddressOf ControlLostFocus
        AddHandler btnClose.LostFocus, AddressOf ControlLostFocus



        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        OpYrCode = Microsoft.VisualBasic.Left(Trim(Common_Procedures.FnRange), 4)
        OpYrCode = Trim(Mid(Val(OpYrCode) - 1, 3, 2)) & "-" & Trim(Microsoft.VisualBasic.Right(OpYrCode, 2))


        FrmLdSTS = True
        move_record(4)
    End Sub

    Private Sub Opening_Stock_Fibre_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub Opening_Stock_Fibre_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        Try
            If Asc(e.KeyChar) = 27 Then
                Close_Form()
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

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
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
        Dim dgv1 As New DataGridView
        Dim i As Integer

        On Error Resume Next

        If ActiveControl.Name = dgv_FibreDetails.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            dgv1 = Nothing

            If ActiveControl.Name = dgv_FibreDetails.Name Then
                dgv1 = dgv_FibreDetails



            ElseIf tab_Main.SelectedIndex = 0 Then
                dgv1 = dgv_FibreDetails



            Else
                Return MyBase.ProcessCmdKey(msg, keyData)
                Exit Function

            End If

            With dgv1

                '-------------------------- WARPING DETAILS (SET1)

                If dgv1.Name = dgv_FibreDetails.Name Then

                    If keyData = Keys.Enter Or keyData = Keys.Down Then

                        If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then

                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                tab_Main.SelectTab(1)
                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)
                            End If

                        Else

                            If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And ((.CurrentCell.ColumnIndex <> 1 And Val(.CurrentRow.Cells(1).Value) = 0) Or (.CurrentCell.ColumnIndex = 1 And Val(dgtxt_FibreDetails.Text) = 0)) Then
                                tab_Main.SelectTab(1)

                            Else
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                            End If

                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then
                        If .CurrentCell.ColumnIndex <= 1 Then
                            If .CurrentCell.RowIndex = 0 Then
                                cbo_Ledger.Focus()

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.Columns.Count - 1)

                            End If

                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)

                        End If

                        Return True

                    Else
                        Return MyBase.ProcessCmdKey(msg, keyData)

                    End If



                    Return True

                Else
                    Return MyBase.ProcessCmdKey(msg, keyData)

                End If



            End With

        Else

            Return MyBase.ProcessCmdKey(msg, keyData)

        End If

    End Function

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim tr As SqlClient.SqlTransaction
        Dim cmd As New SqlClient.SqlCommand
        Dim da As SqlClient.SqlDataAdapter
        Dim dt As DataTable
        Dim NewCode As String
        Dim LedName As String
        Dim nr As Long = 0

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        '   If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Opening_Stock, "~L~") = 0 And InStr(Common_Procedures.UR.Opening_Stock, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT INSERT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
            Exit Sub
        End If

        If Val(lbl_IdNo.Text) = 0 Then
            MessageBox.Show("Invalid Ledger", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If


        LedName = Common_Procedures.Ledger_IdNoToName(con, Val(lbl_IdNo.Text))

        If Trim(LedName) = "" Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(Val(lbl_IdNo.Text)) & "/" & Trim(OpYrCode)


        tr = con.BeginTransaction

        Try

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.CommandText = "Delete from Voucher_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Ledger_IdNo = " & Str(Val(lbl_IdNo.Text)) & " and Entry_Identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            'cmd.CommandText = "Delete from Stock_SizedPavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            'cmd.ExecuteNonQuery()

            'cmd.CommandText = "Delete from Stock_Empty_BeamBagCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            'cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Fibre_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            nr = cmd.ExecuteNonQuery()

            'cmd.CommandText = "Delete from Stock_BabyCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            'cmd.ExecuteNonQuery()

            tr.Commit()

            tr.Dispose()
            cmd.Dispose()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
            '  new_record()

            move_record(4)



        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR DELETION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        If cbo_Ledger.Enabled = True And cbo_Ledger.Visible = True Then cbo_Ledger.Focus()

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        '-----
    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        '------
    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        '---
    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        '--
    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        '--
    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record
        '---
    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        '-----
    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        '-----
    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim LedName As String
        Dim Sno As Integer = 0
        Dim Nr As Integer = 0
        Dim OpDate As Date
        Dim VouAmt As Single
        Dim Dlv_IdNo As Integer, Rec_IdNo As Integer
        Dim Cnt_ID As Integer, pCnt_ID As Integer
        Dim Mil_ID As Integer
        Dim Gdn_ID As Integer
        Dim vSetNo As String
        Dim vSetCd As String
        Dim Selc_SetCode As String
        Dim Dup_SetCd As String = ""
        Dim Dup_SetNoBmNo As String = ""
        Dim Bw_ID As Integer = 0
        Dim Ven_id As Integer = 0
        Dim vMtr_Pc As String = ""


        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        'If Common_Procedures.UserRight_Check(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Opening_Stock, New_Entry) = False Then Exit Sub

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Trim(cbo_Ledger.Text) = "" Then
            MessageBox.Show("Invalid Ledger Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Ledger.Enabled Then cbo_Ledger.Focus()
            Exit Sub
        End If

        If Val(lbl_IdNo.Text) = 0 Then
            MessageBox.Show("Invalid Ledger Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Ledger.Enabled Then cbo_Ledger.Focus()
            Exit Sub
        End If

        LedName = Common_Procedures.Ledger_IdNoToName(con, Val(lbl_IdNo.Text))
        If Trim(LedName) = "" Then
            MessageBox.Show("Invalid Ledger Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Ledger.Enabled Then cbo_Ledger.Focus()
            Exit Sub
        End If

        'If Val(txt_OpAmount.Text) <> 0 And Trim(cbo_CrDrType.Text) = "" Then
        '    MessageBox.Show("Invalid Cr/Dr", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '    If cbo_CrDrType.Enabled Then cbo_CrDrType.Focus()
        '    Exit Sub
        'End If

        For i = 0 To dgv_FibreDetails.RowCount - 1

            If Val(dgv_FibreDetails.Rows(i).Cells(5).Value) <> 0 Then

                Sno = Sno + 1

                Cnt_ID = Common_Procedures.Fibre_NameToIdNo(con, Trim(dgv_FibreDetails.Rows(i).Cells(1).Value))
                ' If dgv_FibreDetails.RowCount - 1 Then
                If Val(Cnt_ID) = 0 Then
                    MessageBox.Show("Invalid Fibre Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    If dgv_FibreDetails.Enabled And dgv_FibreDetails.Visible Then dgv_FibreDetails.Focus()
                    dgv_FibreDetails.CurrentCell = dgv_FibreDetails.Rows(i).Cells(1)
                    dgv_FibreDetails.CurrentCell.Selected = True
                    Exit Sub
                End If
                '   End If

                'If Common_Procedures.settings.CustomerCode = "1288" Then
                '    Dim l As Integer = Common_Procedures.Ledger_AlaisNameToIdNo(con, Trim(dgv_FibreDetails.Rows(i).Cells(8).Value))
                '    If l = 0 Then
                '        MessageBox.Show("Invalid Location Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                '        tab_Main.SelectTab(0)
                '        dgv_FibreDetails.Focus()
                '        dgv_FibreDetails.CurrentCell = dgv_FibreDetails.Rows(i).Cells(8)
                '        Exit Sub
                '    End If
                'End If

                If Trim(dgv_FibreDetails.Rows(i).Cells(2).Value) = "" Then
                    MessageBox.Show("Invalid Lot No", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    If dgv_FibreDetails.Enabled And dgv_FibreDetails.Visible Then dgv_FibreDetails.Focus()
                    dgv_FibreDetails.CurrentCell = dgv_FibreDetails.Rows(i).Cells(2)
                    dgv_FibreDetails.CurrentCell.Selected = True
                    Exit Sub
                End If

                Mil_ID = Common_Procedures.Mill_NameToIdNo(con, Trim(dgv_FibreDetails.Rows(i).Cells(3).Value))
                If Val(Mil_ID) = 0 Then
                    MessageBox.Show("Invalid Mill Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    If dgv_FibreDetails.Enabled And dgv_FibreDetails.Visible Then dgv_FibreDetails.Focus()
                    dgv_FibreDetails.CurrentCell = dgv_FibreDetails.Rows(i).Cells(3)
                    dgv_FibreDetails.CurrentCell.Selected = True
                    Exit Sub
                End If

            End If

        Next



        tr = con.BeginTransaction

        Try

            OpDate = New DateTime(Val(Microsoft.VisualBasic.Left(Trim(Common_Procedures.FnRange), 4)), Microsoft.VisualBasic.DateAndTime.Month(Common_Procedures.Company_FromDate), Microsoft.VisualBasic.DateAndTime.Day(Common_Procedures.Company_FromDate))
            'OpDate = CDate("01-04-" & Microsoft.VisualBasic.Left(Trim(Common_Procedures.FnRange), 4))
            OpDate = DateAdd(DateInterval.Day, -1, OpDate)

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@OpeningDate", OpDate)

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(Val(lbl_IdNo.Text)) & "/" & Trim(OpYrCode)

            cmd.CommandText = "Delete from Voucher_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Ledger_IdNo = " & Str(Val(lbl_IdNo.Text)) & " and Entry_Identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            Sno = 0



            cmd.CommandText = "Delete from Stock_Fibre_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            'cmd.CommandText = "Delete from Stock_BabyCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Delivered_Bags = 0 and Delivered_Cones = 0 and Delivered_Weight = 0"
            'cmd.ExecuteNonQuery()

            Sno = 0
            For i = 0 To dgv_FibreDetails.RowCount - 1

                If Val(dgv_FibreDetails.Rows(i).Cells(5).Value) <> 0 Then

                    Sno = Sno + 1

                    Cnt_ID = Common_Procedures.Fibre_NameToIdNo(con, Trim(dgv_FibreDetails.Rows(i).Cells(1).Value), tr)

                    Mil_ID = Common_Procedures.Mill_NameToIdNo(con, Trim(dgv_FibreDetails.Rows(i).Cells(3).Value), tr)
                    'Gdn_ID = Common_Procedures.Ledger_NameToIdNo(con, Trim(dgv_FibreDetails.Rows(i).Cells(8).Value), tr)

                    vSetCd = ""
                    vSetNo = ""
                    Selc_SetCode = ""
                    'If Trim(UCase(dgv_FibreDetails.Rows(i).Cells(2).Value)) = "BABY" Then
                    '    vSetNo = Trim(dgv_FibreDetails.Rows(i).Cells(7).Value)
                    '    If Trim(vSetNo) <> "" Then
                    '        vSetCd = Trim(Val(lbl_Company.Tag)) & "-" & Trim(vSetNo) & "/" & Trim(OpYrCode)
                    '        Selc_SetCode = Trim(vSetNo) & "/" & Trim(OpYrCode) & "/" & Trim(Val(lbl_Company.Tag))
                    '    End If
                    'End If

                    Dlv_IdNo = 0
                    Rec_IdNo = 0
                    If Val(dgv_FibreDetails.Rows(i).Cells(5).Value) < 0 Then
                        ' Dlv_IdNo = Val(lbl_IdNo.Text)
                        Rec_IdNo = Val(lbl_IdNo.Text)
                    Else
                        ' Rec_IdNo = Val(lbl_IdNo.Text)
                        Dlv_IdNo = Val(lbl_IdNo.Text)
                    End If

                    cmd.CommandText = "Insert into Stock_Fibre_Processing_Details(                   Reference_Code            ,                 Company_IdNo      ,              Reference_No          ,                                 for_OrderBy                            , Reference_Date ,       DeliveryTo_Idno      ,      ReceivedFrom_Idno     , Party_Bill_No           ,              Sl_No    ,                  Fibre_IdNo     ,                    Yarn_Type                   ,                Mill_IdNo       ,                                        Bale                       ,              Cones        ,                    Weight                                ,              Particulars , Posting_For ,          Set_Code            ,            Set_No      ,               WareHouse_IdNo           ,            Lot_No) " &
                                      "Values                                   ('" & Trim(Pk_Condition) & Trim(NewCode) & "' , " & Str(Val(lbl_Company.Tag)) & " , '" & Trim(Val(lbl_IdNo.Text)) & "' , " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_IdNo.Text))) & " ,   @OpeningDate , " & Str(Val(Dlv_IdNo)) & " , " & Str(Val(Rec_IdNo)) & " ,        ''               ,    " & Str(Val(Sno)) & " ,        " & Str(Val(Cnt_ID)) & " ,                     ''                         ,       " & Str(Val(Mil_ID)) & " ,      " & Str(Math.Abs(Val(dgv_FibreDetails.Rows(i).Cells(4).Value))) & " ,       ''          ,  " & Str(Math.Abs(Val(dgv_FibreDetails.Rows(i).Cells(5).Value))) & " ,     ''      ,   'OPENING' ,       '" & Trim(vSetCd) & "' ,   '" & Trim(vSetNo) & "' ,           " & Val(Gdn_ID) & ",       '" & Trim(dgv_FibreDetails.Rows(i).Cells(2).Value) & "')"
                    cmd.ExecuteNonQuery()



                End If

            Next



            tr.Commit()

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

            move_record(lbl_IdNo.Text)

        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        If dgv_FibreDetails.Enabled And dgv_FibreDetails.Visible Then dgv_FibreDetails.Focus()

    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        '-----
    End Sub

    Private Sub cbo_Ledger_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.GotFocus
        Dim Condt As String = ""

        Condt = ""
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1102" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1144" Then
            Condt = "(AccountsGroup_IdNo <> 14)"
        End If
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", Condt, "(Ledger_IdNo = 0)")
        cbo_Ledger.Tag = cbo_Ledger.Text
    End Sub

    Private Sub cbo_Ledger_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.LostFocus
        Dim LedIdNo As Integer = 0

        With cbo_Ledger
            .BackColor = Color.White
            .ForeColor = Color.Black

            If Trim(cbo_Ledger.Text) <> "" Then

                If Trim(UCase(.Tag)) <> Trim(UCase(.Text)) Then

                    .Tag = .Text

                    LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, .Text)
                    If Val(LedIdNo) <> 0 Then
                        move_record(LedIdNo)
                    End If

                End If

            End If

        End With

    End Sub

    Private Sub cbo_Ledger_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyDown
        Dim Condt As String = ""

        Condt = ""
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1102" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1144" Then
            Condt = "(AccountsGroup_IdNo <> 14)"
        End If
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, Nothing, dgv_FibreDetails, "Ledger_AlaisHead", "Ledger_DisplayName", Condt, "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_Ledger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ledger.KeyPress
        Dim LedIdNo As Integer = 0
        Dim Condt As String = ""

        Condt = ""
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1102" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1144" Then
            Condt = "(AccountsGroup_IdNo <> 14)"
        End If
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Ledger, dgv_FibreDetails, "Ledger_AlaisHead", "Ledger_DisplayName", Condt, "(Ledger_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then
            If Trim(cbo_Ledger.Text) <> "" Then
                If Trim(UCase(cbo_Ledger.Tag)) <> Trim(UCase(cbo_Ledger.Text)) Then

                    cbo_Ledger.Tag = cbo_Ledger.Text

                    LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
                    If Val(LedIdNo) <> 0 Then
                        move_record(LedIdNo)
                    End If


                End If
            End If

            'If txt_EmptyBeam.Enabled And txt_EmptyBeam.Visible Then
            '    txt_EmptyBeam.Focus()

            'Else
            '    txt_EmptyBags.Focus()

            'End If

        End If

    End Sub

    Private Sub cbo_Ledger_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
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

    Private Sub txt_OpAmount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        save_record()
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub cbo_Grid_CountName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_CountName.GotFocus

        Try

            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Fibre_Head", "Fibre_Name", "", "(Fibre_IdNo = 0)")

            With cbo_Grid_CountName
                .BackColor = Color.Lime
                .ForeColor = Color.Blue
                .SelectAll()
            End With

        Catch ex As Exception
            '----

        End Try

    End Sub

    Private Sub cbo_Grid_CountName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_CountName.KeyDown
        Try

            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_CountName, Nothing, Nothing, "Fibre_Head", "Fibre_Name", "", "(Fibre_IdNo = 0)")

            If e.KeyValue = 38 And cbo_Grid_CountName.DroppedDown = False Then
                e.Handled = True

                With dgv_FibreDetails
                    If Val(.CurrentCell.RowIndex) <= 0 Then
                        cbo_Ledger.Focus()

                    Else
                        .Focus()
                        .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(7)
                        .CurrentCell.Selected = True


                    End If
                End With

            ElseIf e.KeyValue = 40 And cbo_Grid_CountName.DroppedDown = False Then

                e.Handled = True
                With dgv_FibreDetails
                    If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then


                        If MessageBox.Show("Do you want to Save?", "FOR SAVE", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                            save_record()
                        Else
                            Exit Sub
                        End If
                        '     tab_Main.SelectTab(1)
                        'dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(0).Cells(1)
                        'dgv_PavuDetails.CurrentCell.Selected = True

                    Else
                        .Focus()
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                        .CurrentCell.Selected = True

                    End If

                End With

            End If

        Catch ex As Exception
            '---
        End Try

    End Sub

    Private Sub cbo_Grid_CountName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_CountName.KeyPress

        Try

            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_CountName, Nothing, "Fibre_Head", "Fibre_Name", "", "(Fibre_IdNo = 0)")

            If Asc(e.KeyChar) = 13 Then

                e.Handled = True

                With dgv_FibreDetails



                    .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_CountName.Text)

                    If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then

                        If MessageBox.Show("Do you want to Save?", "FOR SAVE", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                            save_record()
                        Else
                            Exit Sub
                        End If


                        ' tab_Main.SelectTab(1)
                        'dgv_PavuDetails.Focus()
                        'dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(0).Cells(1)
                        'dgv_PavuDetails.CurrentCell.Selected = True

                    Else
                        .Focus()
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                        .CurrentCell.Selected = True


                    End If

                End With

            End If

        Catch ex As Exception
            '---
        End Try

    End Sub

    Private Sub cbo_Grid_CountName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_CountName.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New Fibre_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_CountName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_Grid_CountName_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_CountName.LostFocus

        With cbo_Grid_CountName
            .BackColor = Color.White
            .ForeColor = Color.Black
        End With

    End Sub

    Private Sub cbo_Grid_MillName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_MillName.GotFocus

        Try
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "mill_Head", "mill_name", "", "(Mill_IdNo = 0)")
            With cbo_Grid_MillName
                .BackColor = Color.Lime
                .ForeColor = Color.Blue
                .SelectAll()
            End With
        Catch ex As Exception
            '----
        End Try

    End Sub

    Private Sub cbo_Grid_MillName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_MillName.KeyDown
        Try

            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_MillName, Nothing, Nothing, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")

            With cbo_Grid_MillName
                If e.KeyValue = 38 And .DroppedDown = False Then
                    e.Handled = True

                    With dgv_FibreDetails
                        .Focus()
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)
                        .CurrentCell.Selected = True
                    End With

                ElseIf e.KeyValue = 40 And .DroppedDown = False Then
                    e.Handled = True

                    With dgv_FibreDetails
                        .Focus()
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                        .CurrentCell.Selected = True
                    End With

                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT ITEM...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Grid_MillName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_MillName.KeyPress

        Try

            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_MillName, Nothing, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")

            If Asc(e.KeyChar) = 13 Then

                With dgv_FibreDetails
                    .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_MillName.Text)
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                    .CurrentCell.Selected = True
                End With

            End If

        Catch ex As Exception
            '---

        End Try

    End Sub

    Private Sub cbo_Grid_MillName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_MillName.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New Mill_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_MillName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_Grid_MillName_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_MillName.LostFocus

        With cbo_Grid_MillName
            .BackColor = Color.White
            .ForeColor = Color.Black
        End With

    End Sub


    Private Sub dgv_FibreDetails_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_FibreDetails.CellEndEdit
        dgv_FibreDetails_CellLeave(sender, e)
        'TotalYarn_Calculation()
        'SendKeys.Send("{up}")
        'SendKeys.Send("{Tab}")
    End Sub

    Private Sub dgv_FibreDetails_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_FibreDetails.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim rect As Rectangle

        With dgv_FibreDetails
            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If

            If e.ColumnIndex = 1 Then

                If cbo_Grid_CountName.Visible = False Or Val(cbo_Grid_CountName.Tag) <> e.RowIndex Then

                    cbo_Grid_CountName.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Fibre_Name from Fibre_Head order by Fibre_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Grid_CountName.DataSource = Dt1
                    cbo_Grid_CountName.DisplayMember = "Fibre_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_CountName.Left = .Left + rect.Left
                    cbo_Grid_CountName.Top = .Top + rect.Top

                    cbo_Grid_CountName.Width = rect.Width
                    cbo_Grid_CountName.Height = rect.Height
                    cbo_Grid_CountName.Text = .CurrentCell.Value

                    cbo_Grid_CountName.Tag = Val(e.RowIndex)
                    cbo_Grid_CountName.Visible = True

                    cbo_Grid_CountName.BringToFront()
                    cbo_Grid_CountName.Focus()

                Else
                    'If cbo_Grid_CountName.Enabled Then
                    '    cbo_Grid_CountName.BringToFront()
                    '    cbo_Grid_CountName.Focus()
                    'End If

                End If

            Else
                cbo_Grid_CountName.Visible = False
                cbo_Grid_CountName.Tag = -1
                cbo_Grid_CountName.Text = ""

            End If



            If .CurrentCell.ColumnIndex = 3 Then

                If cbo_Grid_MillName.Visible = False Or Val(cbo_Grid_MillName.Tag) <> e.RowIndex Then

                    cbo_Grid_MillName.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Mill_Name from Mill_Head order by Mill_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Grid_MillName.DataSource = Dt1
                    cbo_Grid_MillName.DisplayMember = "Mill_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_MillName.Left = .Left + rect.Left
                    cbo_Grid_MillName.Top = .Top + rect.Top

                    cbo_Grid_MillName.Width = rect.Width
                    cbo_Grid_MillName.Height = rect.Height
                    cbo_Grid_MillName.Text = .CurrentCell.Value

                    cbo_Grid_MillName.Tag = Val(e.RowIndex)
                    cbo_Grid_MillName.Visible = True

                    cbo_Grid_MillName.BringToFront()
                    cbo_Grid_MillName.Focus()

                Else
                    'If cbo_Grid_MillName.Enabled Then
                    '    cbo_Grid_MillName.BringToFront()
                    '    cbo_Grid_MillName.Focus()
                    'End If

                End If

            Else
                cbo_Grid_MillName.Visible = False
                cbo_Grid_MillName.Tag = -1
                cbo_Grid_MillName.Text = ""

            End If



        End With

    End Sub

    Private Sub dgv_FibreDetails_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_FibreDetails.CellLeave
        With dgv_FibreDetails
            If .CurrentCell.ColumnIndex = 5 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.000")
                End If
            End If
        End With
    End Sub

    Private Sub dgv_FibreDetails_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_FibreDetails.CellValueChanged
        Try
            If IsNothing(dgv_FibreDetails.CurrentCell) Then Exit Sub
            With dgv_FibreDetails
                If .Visible Then
                    If .CurrentCell.ColumnIndex = 5 Then
                        TotalYarn_Calculation()
                    End If
                End If
            End With
        Catch ex As Exception
            '------
        End Try


    End Sub

    Private Sub dgv_FibreDetails_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_FibreDetails.EditingControlShowing
        dgtxt_FibreDetails = CType(dgv_FibreDetails.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub
    Private Sub dgtxt_FibreDetails_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_FibreDetails.Enter
        dgv_FibreDetails.EditingControl.BackColor = Color.Lime
        dgv_FibreDetails.EditingControl.ForeColor = Color.Blue
        dgtxt_FibreDetails.SelectAll()
    End Sub

    Private Sub dgtxt_FibreDetails_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_FibreDetails.KeyPress
        'If dgv_FibreDetails.CurrentCell.ColumnIndex = 4 Or dgv_FibreDetails.CurrentCell.ColumnIndex = 5 Then
        If dgv_FibreDetails.CurrentCell.ColumnIndex = 5 Then
            If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                e.Handled = True
            End If
        End If
        With dgv_FibreDetails
            If Asc(e.KeyChar) = 13 Then
                If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then
                    If MessageBox.Show("Do you want to Save?", "FOR SAVE", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                        save_record()
                    Else
                        Exit Sub
                    End If
                Else
                    'tab_Main.SelectTab(1)
                    'dgv_PavuDetails.Focus()
                    'dgv_PavuDetails.CurrentCell = dgv_PavuDetails.CurrentRow.Cells(1)
                    'dgv_PavuDetails.CurrentCell.Selected = True

                End If
            End If
        End With

    End Sub

    Private Sub dgtxt_FibreDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_FibreDetails.KeyUp
        dgv_FibreDetails_KeyUp(sender, e)
    End Sub

    Private Sub dgv_FibreDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_FibreDetails.KeyUp
        Dim i As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_FibreDetails
                If .CurrentRow.Index = 0 And .RowCount = 1 Then
                    For i = 1 To .Columns.Count - 1
                        .Rows(.CurrentRow.Index).Cells(i).Value = ""
                    Next
                Else
                    .Rows.RemoveAt(.CurrentRow.Index)

                End If

                For i = 0 To .Rows.Count - 1
                    .Rows(i).Cells(0).Value = i + 1
                Next

                TotalYarn_Calculation()

            End With
        End If


    End Sub

    Private Sub dgv_FibreDetails_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_FibreDetails.LostFocus
        On Error Resume Next
        If Not IsNothing(dgv_FibreDetails.CurrentCell) Then dgv_FibreDetails.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_FibreDetails_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_FibreDetails.RowsAdded
        Dim n As Integer
        If IsNothing(dgv_FibreDetails.CurrentCell) Then Exit Sub
        With dgv_FibreDetails
            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)
        End With
    End Sub

    Private Sub TotalYarn_Calculation()
        Dim Sno As Integer
        Dim TotBags As Single, TotCones As Single, TotWeight As Single

        Sno = 0
        TotBags = 0
        TotCones = 0
        TotWeight = 0
        With dgv_FibreDetails
            For i = 0 To .RowCount - 1
                Sno = Sno + 1
                .Rows(i).Cells(0).Value = Sno
                If Val(.Rows(i).Cells(5).Value) <> 0 Then
                    'TotBags = TotBags + Val(.Rows(i).Cells(4).Value)
                    'TotCones = TotCones + Val(.Rows(i).Cells(5).Value)
                    TotWeight = TotWeight + Val(.Rows(i).Cells(5).Value)
                End If
            Next
        End With

        With dgv_FibreDetails_Total
            If .RowCount = 0 Then .Rows.Add()
            '.Rows(0).Cells(4).Value = Val(TotBags)
            '.Rows(0).Cells(5).Value = Val(TotCones)
            .Rows(0).Cells(5).Value = Format(Val(TotWeight), "########0.000")
        End With

    End Sub


    Private Sub txt_EmptyBeam_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_EmptyBags_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_EmptyCones_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then
            tab_Main.SelectTab(0)
            dgv_FibreDetails.Focus()
            dgv_FibreDetails.CurrentCell = dgv_FibreDetails.Rows(0).Cells(1)
            dgv_FibreDetails.CurrentCell.Selected = True
        End If
    End Sub

    Private Sub txt_EmptyCones_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            tab_Main.SelectTab(0)
            dgv_FibreDetails.Focus()
            dgv_FibreDetails.CurrentCell = dgv_FibreDetails.Rows(0).Cells(1)
            dgv_FibreDetails.CurrentCell.Selected = True
        End If
    End Sub

    Private Sub cbo_Grid_CountName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_CountName.TextChanged
        Try
            If cbo_Grid_CountName.Visible Then
                If IsNothing(dgv_FibreDetails.CurrentCell) Then Exit Sub

                With dgv_FibreDetails
                    If Val(cbo_Grid_CountName.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_CountName.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Grid_MillName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_MillName.TextChanged
        Try
            If cbo_Grid_MillName.Visible Then
                If IsNothing(dgv_FibreDetails.CurrentCell) Then Exit Sub

                With dgv_FibreDetails
                    If Val(cbo_Grid_MillName.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 3 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_MillName.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub



    Private Sub tab_Main_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tab_Main.SelectedIndexChanged

        If tab_Main.SelectedIndex = 0 Then
            If dgv_FibreDetails.Rows.Count <= 0 Then dgv_FibreDetails.Rows.Add()
            dgv_FibreDetails.Focus()
            dgv_FibreDetails.CurrentCell = dgv_FibreDetails.Rows(0).Cells(1)
            dgv_FibreDetails.CurrentCell.Selected = True

        ElseIf tab_Main.SelectedIndex = 1 Then
            'If dgv_PavuDetails.Rows.Count <= 0 Then dgv_PavuDetails.Rows.Add()
            'dgv_PavuDetails.Focus()
            'dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(0).Cells(1)
            'dgv_PavuDetails.CurrentCell.Selected = True

        End If

    End Sub

    Private Sub cbo_Grid_WareHouse_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_Head", "Ledger_Name", "(Ledger_Type ='GODOWN')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub dgv_FibreDetails_CellValueNeeded(sender As Object, e As DataGridViewCellValueEventArgs) Handles dgv_FibreDetails.CellValueNeeded

    End Sub

    'Private Sub dgv_FibreDetails_Total_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles dgv_FibreDetails_Total.CellValueChanged

    '    With dgv_FibreDetails
    '        If dgv_FibreDetails_Total.Rows(0).Cells(5).Value Then
    '            TotalYarn_Calculation()
    '        End If

    '    End With
    'End Sub
End Class