Public Class WeavingUnit_Production_Wages_Entry
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "LOMPR-"
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
    Private NoCalc_Status As Boolean = False
    Private Mov_Status As Boolean = False

    Private dgvDet_CboBx_ColNos_Arr As Integer() = {1, 6, 10, 12, 14, 16}
    Private Enum DgvCol_Details As Integer

        SL_NO
        CLOTH_NAME
        ENDSCOUNT_NAME
        SOUND_METERS
        SECOND_METERS
        BITS_METERS
        TOTAL_METERS

        SOUND_WAGES
        SECONDS_WAGES
        BITS_WAGES
        TOTAL_AMOUNT

        WARP_METERS
        WEFT_COUNT_NAME
        WEIGHT_CONSUMP_METER
        CONSUMED_WEIGHT



    End Enum

    Private Sub clear()
        pnl_Back.Enabled = True
        pnl_Filter.Visible = False

        NoCalc_Status = True
        Mov_Status = False

        New_Entry = False
        vmskOldText = ""
        vmskSelStrt = -1

        lbl_RefNo.Text = ""
        lbl_RefNo.ForeColor = Color.Black

        'Dim today As DateTime = Date.Now
        Dim firstDayOfMonth As New DateTime(Now.Year, Now.Month, 1) '(today.Year, today.Month, 1)


        msk_FromDate.Text = firstDayOfMonth
        dtp_FromDate.Text = firstDayOfMonth
        msk_FromDate.Tag = msk_FromDate.Text

        msk_ToDate.Text = ""
        Dtp_ToDate.Text = ""
        msk_ToDate.Tag = msk_ToDate.Text

        cbo_Weaver.Text = ""

        dgv_Details.Rows.Clear()

        dgv_Details_Total.Rows.Clear()
        dgv_Details_Total.Rows.Add()


        NoCalc_Status = False





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

        'If TypeOf Me.ActiveControl Is TextBox Then
        '    txtbx = Me.ActiveControl
        '    txtbx.SelectAll()
        'ElseIf TypeOf Me.ActiveControl Is ComboBox Then
        '    combobx = Me.ActiveControl
        '    combobx.SelectAll()
        'ElseIf TypeOf Me.ActiveControl Is MaskedTextBox Then
        '    msktxbx = Me.ActiveControl
        '    msktxbx.SelectionStart = 0
        'End If

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
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
        If Not IsNothing(dgv_Details_Total.CurrentCell) Then dgv_Details_Total.CurrentCell.Selected = False
    End Sub

    Private Sub Grid_Cell_DeSelect()
        On Error Resume Next
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
        If Not IsNothing(dgv_Details_Total.CurrentCell) Then dgv_Details_Total.CurrentCell.Selected = False
    End Sub

    Private Sub LoomNo_Production_Entry_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated


        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Weaver.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Weaver.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable


        Me.Text = ""

        con.Open()

        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead Where (Ledger_IdNo = 0 OR ledger_type = 'WEAVER'  or Ledger_Type = 'GODOWN' or Show_In_All_Entry = 1) order by Ledger_DisplayName", con)
        da.Fill(dt1)
        cbo_Weaver.DataSource = dt1
        cbo_Weaver.DisplayMember = "Ledger_DisplayName"

        For i = 0 To dgv_Details.ColumnCount - 1
            If i <> DgvCol_Details.SOUND_WAGES And i <> DgvCol_Details.SECONDS_WAGES And i <> DgvCol_Details.BITS_WAGES Then
                dgv_Details.Columns(i).DefaultCellStyle.BackColor = Color.FromArgb(252, 255, 224) ' Color.FromArgb(214, 255, 255) '(218, 255, 213) '253, 188, 180') '(214, 255, 255) -LIGHT BLUE  ' 255,218,233
            End If
        Next

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2
        pnl_Filter.BringToFront()

        btn_UserModification.Visible = False
        If Common_Procedures.settings.User_Modifications_Show_Status = 1 Then
            If Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_UserModification_Status = 1 Then
                btn_UserModification.Visible = True
            End If
        End If

        AddHandler msk_FromDate.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_FromDate.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_ToDate.GotFocus, AddressOf ControlGotFocus
        AddHandler Dtp_ToDate.GotFocus, AddressOf ControlGotFocus

        AddHandler dtp_Filter_Fromdate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_Loom.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_Shift.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_ToDate.GotFocus, AddressOf ControlGotFocus

        AddHandler btn_Save.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Cancel.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Weaver.GotFocus, AddressOf ControlGotFocus



        AddHandler dtp_FromDate.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_FromDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Weaver.LostFocus, AddressOf ControlLostFocus

        AddHandler Dtp_ToDate.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_ToDate.LostFocus, AddressOf ControlLostFocus

        AddHandler dtp_Filter_Fromdate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_Shift.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_Loom.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_ToDate.LostFocus, AddressOf ControlLostFocus


        AddHandler dtp_FromDate.KeyDown, AddressOf TextBoxControlKeyDown
        ' AddHandler msk_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        ' AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler dtp_FromDate.KeyPress, AddressOf TextBoxControlKeyPress
        ' AddHandler msk_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_Fromdate.KeyPress, AddressOf TextBoxControlKeyPress
        ' AddHandler dtp_Filter_ToDate.KeyPress, AddressOf TextBoxControlKeyPress






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
        Dim vColmnCount_No As Integer = 0
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


                    vColmnCount_No = DgvCol_Details.BITS_WAGES


                    If keyData = Keys.Enter Or keyData = Keys.Down Then

                        '        If .CurrentCell.ColumnIndex >= DgvCol_Details.SHIFT_2 Then
                        If .CurrentCell.ColumnIndex >= vColmnCount_No Then

                            'If .CurrentCell.ColumnIndex >= .ColumnCount - 3 Then

                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                                    save_record()
                                Else
                                    msk_FromDate.Focus()
                                    Return True
                                    Exit Function
                                End If

                            Else
                                .CurrentCell = .Rows(.CurrentRow.Index + 1).Cells(DgvCol_Details.SOUND_WAGES)
                            End If

                            'ElseIf .CurrentCell.ColumnIndex <= DgvCol_Details.SHIFT_1_EMPLOYEE Then
                            '    .CurrentCell = .Rows(.CurrentRow.Index).Cells(DgvCol_Details.SHIFT_1)

                            'ElseIf .CurrentCell.ColumnIndex = DgvCol_Details.SHIFT_2 Then
                            '    .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 2)

                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                            '.CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
                        End If
                        'Else
                        '    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                        'End If

                        Return True

                    ElseIf keyData = Keys.Up Then

                        If .CurrentCell.ColumnIndex <= DgvCol_Details.SOUND_WAGES Then

                            If .CurrentCell.ColumnIndex = DgvCol_Details.SOUND_WAGES And .CurrentCell.RowIndex = 0 Then
                                msk_FromDate.Focus()
                            Else

                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(DgvCol_Details.BITS_WAGES)


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

    Private Sub move_record(ByVal no As String, SELEC As Boolean)
        Dim cmd As New SqlClient.SqlCommand
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim NewCode As String
        Dim n As Integer
        Dim SNo As Integer

        If Val(no) = 0 Then Exit Sub

        clear()

        NoCalc_Status = True
        Mov_Status = True

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)


        Try


            da1 = New SqlClient.SqlDataAdapter("select a.* from WeavingUnit_Production_Wages_Head a  Where a.WeavingUnit_Production_Wages_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_RefNo.Text = dt1.Rows(0).Item("WeavingUnit_Production_Wages_No").ToString
                dtp_FromDate.Text = dt1.Rows(0).Item("WeavingUnit_Production_Wages_FromDate").ToString
                Dtp_ToDate.Text = dt1.Rows(0).Item("WeavingUnit_Production_Wages_ToDate").ToString
                msk_FromDate.Text = dtp_FromDate.Text
                msk_FromDate.Tag = msk_FromDate.Text

                msk_ToDate.Text = Dtp_ToDate.Text
                msk_ToDate.Tag = msk_ToDate.Text

                cbo_Weaver.Text = Common_Procedures.Ledger_IdNoToName(con, dt1.Rows(0).Item("ledger_Idno").ToString)


                da2 = New SqlClient.SqlDataAdapter("select a.*,C.Cloth_Name,Ec.Endscount_name,Wfc.Count_Name from WeavingUnit_Production_Wages_Details a INNER JOIN Cloth_Head c ON a.Cloth_IdNo <> 0 and a.Cloth_IdNo = c.Cloth_IdNo  INNER JOIN EndsCount_Head Ec on a.EndsCount_IdNo = Ec.EndsCount_IdNo LEFT OUTER JOIN Count_Head Wfc on a.WeftCount_IdNo = Wfc.Count_IdNo   Where a.WeavingUnit_Production_Wages_Code = '" & Trim(NewCode) & "'", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                '---------------

                dgv_Details.Rows.Clear()
                SNo = 0

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_Details.Rows.Add()

                        SNo = SNo + 1

                        dgv_Details.Rows(n).Cells(DgvCol_Details.SL_NO).Value = Val(SNo)

                        dgv_Details.Rows(n).Cells(DgvCol_Details.ENDSCOUNT_NAME).Value = Trim(dt2.Rows(i).Item("ENDSCOUNT_NAME").ToString)
                        dgv_Details.Rows(n).Cells(DgvCol_Details.CLOTH_NAME).Value = Trim(dt2.Rows(i).Item("Cloth_Name").ToString)

                        dgv_Details.Rows(n).Cells(DgvCol_Details.SOUND_METERS).Value = Format(Val(dt2.Rows(i).Item("Type1_Meters").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(DgvCol_Details.SECOND_METERS).Value = Format(Val(dt2.Rows(i).Item("Type2_Meters").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(DgvCol_Details.BITS_METERS).Value = Format(Val(dt2.Rows(i).Item("Type3_Meters").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(DgvCol_Details.TOTAL_METERS).Value = Format(Val(dt2.Rows(i).Item("Total_Meters").ToString), "########0.00")

                        dgv_Details.Rows(n).Cells(DgvCol_Details.SOUND_WAGES).Value = Format(Val(dt2.Rows(i).Item("Type1_Wages_Rate").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(DgvCol_Details.SECONDS_WAGES).Value = Format(Val(dt2.Rows(i).Item("Type2_Wages_Rate").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(DgvCol_Details.BITS_WAGES).Value = Format(Val(dt2.Rows(i).Item("Type3_Wages_Rate").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(DgvCol_Details.TOTAL_AMOUNT).Value = Format(Val(dt2.Rows(i).Item("Total_Amount").ToString), "########0.00")


                        dgv_Details.Rows(n).Cells(DgvCol_Details.WARP_METERS).Value = Format(Val(dt2.Rows(i).Item("Warp_Meters").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(DgvCol_Details.WEFT_COUNT_NAME).Value = Trim(dt2.Rows(i).Item("Count_Name").ToString)
                        dgv_Details.Rows(n).Cells(DgvCol_Details.WEIGHT_CONSUMP_METER).Value = Format(Val(dt2.Rows(i).Item("Weight_Meter_Weft").ToString), "########0.000")
                        dgv_Details.Rows(n).Cells(DgvCol_Details.CONSUMED_WEIGHT).Value = Format(Val(dt2.Rows(i).Item("Consumed_Weight").ToString), "########0.000")



                    Next i

                End If

                If dgv_Details.RowCount = 0 Then
                    dgv_Details.Rows.Add()
                End If
                With dgv_Details_Total

                    If dgv_Details_Total.RowCount = 0 Then dgv_Details_Total.Rows.Add()


                    .Rows(0).Cells(DgvCol_Details.SOUND_METERS).Value = Format(Val(dt1.Rows(0).Item("Total_Type1_Meters").ToString), "########0.00")
                    .Rows(0).Cells(DgvCol_Details.SECOND_METERS).Value = Format(Val(dt1.Rows(0).Item("Total_Type2_Meters").ToString), "########0.00")
                    .Rows(0).Cells(DgvCol_Details.BITS_METERS).Value = Format(Val(dt1.Rows(0).Item("Total_Type3_Meters").ToString), "########0.00")

                    .Rows(0).Cells(DgvCol_Details.TOTAL_METERS).Value = Format(Val(dt1.Rows(0).Item("Total_Meters").ToString), "########0.00")
                    .Rows(0).Cells(DgvCol_Details.WARP_METERS).Value = Format(Val(dt1.Rows(0).Item("Total_WarpMeters").ToString), "########0.00")
                    .Rows(0).Cells(DgvCol_Details.TOTAL_AMOUNT).Value = Format(Val(dt1.Rows(0).Item("ToTal_Amount").ToString), "########0.00")

                    .Rows(0).Cells(DgvCol_Details.CONSUMED_WEIGHT).Value = Format(Val(dt1.Rows(0).Item("Total_Consumed_Weight").ToString), "########0.000")






                End With
                dt2.Dispose()
                da2.Dispose()

            End If

            dt1.Dispose()
            da1.Dispose()

            Grid_DeSelect()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            If msk_FromDate.Visible And msk_FromDate.Enabled Then msk_FromDate.Focus()

        End Try
        NoCalc_Status = False
        Mov_Status = False
    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim vOrdByNo As String = ""

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text)

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.Inhouse_Loom_Production_Entry, New_Entry, Me, con, "WeavingUnit_Production_Wages_Head", "WeavingUnit_Production_Wages_Code", NewCode, "WeavingUnit_Production_Wages_FromDate", "(WeavingUnit_Production_Wages_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub


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

        trans = con.BeginTransaction

        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.Transaction = trans

            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "DELETE", "WeavingUnit_Production_Wages_Head", "WeavingUnit_Production_Wages_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "", "", "WeavingUnit_Production_Wages_Code, Company_IdNo, for_OrderBy", trans)

            Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "DELETE", "WeavingUnit_Production_Wages_Details", "WeavingUnit_Production_Wages_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, " Shift_IdNo   , Loom_IdNo, Meters   ,  Warp_Meters  ,   Weft_Meters  ,  Pick_Efficiency , Employee_IdNo", "Sl_No", "WeavingUnit_Production_Wages_Code, For_OrderBy, Company_IdNo, WeavingUnit_Production_Wages_No, WeavingUnit_Production_Wages_FromDate, Ledger_Idno", trans)

            cmd.CommandText = "Delete from WeavingUnit_Production_Wages_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and WeavingUnit_Production_Wages_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from WeavingUnit_Production_Wages_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and WeavingUnit_Production_Wages_Code = '" & Trim(NewCode) & "'"
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

            If msk_FromDate.Visible And msk_FromDate.Enabled Then msk_FromDate.Focus()

        End Try
    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        Exit Sub

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


        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.Inhouse_Loom_Production_Entry, New_Entry, Me) = False Then Exit Sub



        Try

            inpno = InputBox("Enter New LOOM.No.", "FOR NEW LOOM NO INSERTION...")

            RefCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select WeavingUnit_Production_Wages_No from WeavingUnit_Production_Wages_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and WeavingUnit_Production_Wages_Code = '" & Trim(RefCode) & "'", con)
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
                move_record(movno, False)

            Else
                If Val(inpno) = 0 Then
                    MessageBox.Show("Invalid Loom No", "DOES NOT INSERT NEW LOOM NO...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_RefNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT NEW LOOM...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String

        Try

            da = New SqlClient.SqlDataAdapter("select top 1 WeavingUnit_Production_Wages_No from WeavingUnit_Production_Wages_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and WeavingUnit_Production_Wages_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, WeavingUnit_Production_Wages_No", con)
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

            If Val(movno) <> 0 Then move_record(movno, False)

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

            da = New SqlClient.SqlDataAdapter("select top 1 WeavingUnit_Production_Wages_No from WeavingUnit_Production_Wages_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and WeavingUnit_Production_Wages_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, WeavingUnit_Production_Wages_No", con)
            da.Fill(dt)

            movno = ""
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = dt.Rows(0)(0).ToString
                End If
            End If

            If Val(movno) <> 0 Then move_record(movno, False)

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

            da = New SqlClient.SqlDataAdapter("select top 1 WeavingUnit_Production_Wages_No from WeavingUnit_Production_Wages_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and WeavingUnit_Production_Wages_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, WeavingUnit_Production_Wages_No desc", con)
            da.Fill(dt)

            movno = ""
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = dt.Rows(0)(0).ToString
                End If
            End If

            If Val(movno) <> 0 Then move_record(movno, False)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""

        Try
            da = New SqlClient.SqlDataAdapter("select top 1 WeavingUnit_Production_Wages_No from WeavingUnit_Production_Wages_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and WeavingUnit_Production_Wages_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, WeavingUnit_Production_Wages_No desc", con)
            da.Fill(dt)

            movno = ""
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = dt.Rows(0)(0).ToString
                End If
            End If

            If Val(movno) <> 0 Then move_record(movno, False)

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

            lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "WeavingUnit_Production_Wages_Head", "WeavingUnit_Production_Wages_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

            lbl_RefNo.ForeColor = Color.Red

            'msk_FromDate.Text = Date.Today.ToShortDateString
            da = New SqlClient.SqlDataAdapter("select top 1 * from WeavingUnit_Production_Wages_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and WeavingUnit_Production_Wages_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, WeavingUnit_Production_Wages_No desc", con)
            dt1 = New DataTable
            da.Fill(dt1)
            If dt1.Rows.Count > 0 Then
                If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)
                    If dt1.Rows(0).Item("WeavingUnit_Production_Wages_FromDate").ToString <> "" Then msk_FromDate.Text = dt1.Rows(0).Item("WeavingUnit_Production_Wages_FromDate").ToString
                End If
            End If
            dt1.Clear()

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

            inpno = InputBox("Enter Ref.No.", "FOR FINDING...")

            RefCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select WeavingUnit_Production_Wages_No from WeavingUnit_Production_Wages_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and WeavingUnit_Production_Wages_Code = '" & Trim(RefCode) & "'", con)
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
                move_record(movno, False)

            Else
                MessageBox.Show("Loom No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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
        Dim Endcnt_Id As Integer = 0
        Dim Wftcnt_Id As Integer = 0
        Dim Item_ID As Integer = 0
        Dim Emp_Id As Integer = 0
        Dim Sht_ID As Integer = 0
        Dim Brand_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim vTotMtr As Single = 0, vTotWrp As Single = 0, vTotWeft As Single = 0, vTot_Cons_Wgt As Single = 0
        Dim EntID As String = ""
        Dim PBlNo As String = ""
        Dim Partcls As String = ""
        Dim vOrdByNo As String = ""
        Dim vShift1 As String = 0
        Dim vShift2 As String = 0
        Dim vShift3 As String = 0
        Dim vDoff1_Mtrs As String = 0
        Dim vDoff2_Mtrs As String = 0
        Dim vDoff3_Mtrs As String = 0
        Dim vDoff4_Mtrs As String = 0
        Dim vLASTDOFSTS As Integer = 0
        Dim vONLOOMFABMTRS As String = 0
        Dim vSHFTPRODMTRS As String = 0
        Dim DofShit1_Id As Integer = 0
        Dim DofShit2_Id As Integer = 0
        Dim DofShit3_Id As Integer = 0
        Dim DofShit4_Id As Integer = 0
        Dim Shift_Id As Integer
        Dim Emp_Id_Shift_1 As Integer = 0
        Dim Emp_Id_Shift_2 As Integer = 0
        Dim cloth_Idno As Integer = 0


        Dim vTot_Amount As String = 0
        Dim Led_ID As String = 0

        Dim vTOT_Sound_Mtrs = ""
        Dim vTOT_Second_Mtrs = ""
        Dim vTOT_bits_Mtrs = ""


        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text)
        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Inhouse_Loom_Production_Entry, New_Entry, Me, con, "WeavingUnit_Production_Wages_Head", "WeavingUnit_Production_Wages_Code", NewCode, "WeavingUnit_Production_Wages_FromDate", "(WeavingUnit_Production_Wages_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and WeavingUnit_Production_Wages_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, WeavingUnit_Production_Wages_No desc", dtp_FromDate.Value.Date) = False Then Exit Sub

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(msk_FromDate.Text) = False Then
            MessageBox.Show("Invalid From Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If msk_FromDate.Enabled Then msk_FromDate.Focus()
            Exit Sub
        End If


        If Not (Convert.ToDateTime(msk_FromDate.Text) >= Common_Procedures.Company_FromDate And Convert.ToDateTime(msk_FromDate.Text) <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If msk_FromDate.Enabled Then msk_FromDate.Focus()
            Exit Sub
        End If

        If IsDate(msk_ToDate.Text) = False Then
            MessageBox.Show("Invalid To Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If msk_ToDate.Enabled Then msk_ToDate.Focus()
            Exit Sub
        End If

        Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Weaver.Text)

        'If Led_ID = 0 Then
        '    MessageBox.Show("Invalid Ledger Name", "DOES NOT SHOW REPORT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    If cbo_Weaver.Enabled Then cbo_Weaver.Focus()
        '    Exit Sub
        'End If


        vTotMtr = 0 : vTotWrp = 0 : vTot_Amount = 0 : vTot_Cons_Wgt = 0

        vTOT_Sound_Mtrs = 0
        vTOT_Second_Mtrs = 0
        vTOT_bits_Mtrs = 0

        If dgv_Details_Total.RowCount > 0 Then


            vTotMtr = Val(dgv_Details_Total.Rows(0).Cells(DgvCol_Details.TOTAL_METERS).Value())
            vTotWrp = Val(dgv_Details_Total.Rows(0).Cells(DgvCol_Details.WARP_METERS).Value())
            vTot_Amount = Val(dgv_Details_Total.Rows(0).Cells(DgvCol_Details.TOTAL_AMOUNT).Value())
            vTot_Cons_Wgt = Val(dgv_Details_Total.Rows(0).Cells(DgvCol_Details.CONSUMED_WEIGHT).Value())

            vTOT_Sound_Mtrs = vTOT_Sound_Mtrs + Val(dgv_Details_Total.Rows(0).Cells(DgvCol_Details.SOUND_METERS).Value)
            vTOT_Second_Mtrs = vTOT_Second_Mtrs + Val(dgv_Details_Total.Rows(0).Cells(DgvCol_Details.SECOND_METERS).Value)
            vTOT_bits_Mtrs = vTOT_bits_Mtrs + Val(dgv_Details_Total.Rows(0).Cells(DgvCol_Details.BITS_METERS).Value)

        End If

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "WeavingUnit_Production_Wages_Head", "WeavingUnit_Production_Wages_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr
            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@FromDate", Convert.ToDateTime(msk_FromDate.Text))
            cmd.Parameters.AddWithValue("@ToDate", Convert.ToDateTime(msk_ToDate.Text))

            If New_Entry = True Then


                cmd.CommandText = "Insert into WeavingUnit_Production_Wages_Head(      WeavingUnit_Production_Wages_Code ,           Company_IdNo         ,                   WeavingUnit_Production_Wages_No ,                           for_OrderBy                                 ,   WeavingUnit_Production_Wages_FromDate ,      WeavingUnit_Production_Wages_ToDate ,    Ledger_IdNo      ,          Total_Meters      ,    Total_WarpMeters      ,           Total_Amount         ,        Total_Consumed_Weight       ,        Total_Type1_Meters           ,           Total_Type2_Meters       ,              Total_Type3_Meters ) " &
                                                            "   Values          (                '" & Trim(NewCode) & "' , " & Str(Val(lbl_Company.Tag)) & ",                 '" & Trim(lbl_RefNo.Text) & "'  , " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",                        @FromDate        ,                     @ToDate               , " & Val(Led_ID) & " ,  " & Str(Val(vTotMtr)) & " ," & Str(Val(vTotWrp)) & " , " & Str(Val(vTot_Amount)) & "  , " & Str(Val(vTot_Cons_Wgt)) & "  ," & Str(Val(vTOT_Sound_Mtrs)) & "  ," & Str(Val(vTOT_Second_Mtrs)) & "  ," & Str(Val(vTOT_bits_Mtrs)) & "  ) "
                cmd.ExecuteNonQuery()

            Else
                Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "OLD", "WeavingUnit_Production_Wages_Head", "WeavingUnit_Production_Wages_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "WeavingUnit_Production_Wages_Code, Company_IdNo, for_OrderBy", tr)

                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "OLD", "WeavingUnit_Production_Wages_Details", "WeavingUnit_Production_Wages_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, " Shift_IdNo   , Loom_IdNo, Meters   ,  Warp_Meters  ,   Weft_Meters  ,  Pick_Efficiency , Employee_IdNo", "Sl_No", "WeavingUnit_Production_Wages_Code, For_OrderBy, Company_IdNo, WeavingUnit_Production_Wages_No, WeavingUnit_Production_Wages_FromDate, Ledger_Idno", tr)

                cmd.CommandText = "Update WeavingUnit_Production_Wages_Head set ledger_IdNo = " & Val(Led_ID) & ", WeavingUnit_Production_Wages_FromDate= @FromDate , WeavingUnit_Production_Wages_ToDate= @ToDate ,  Total_Meters = " & Str(Val(vTotMtr)) & ",Total_WarpMeters = " & Str(Val(vTotWrp)) & " ,   Total_Amount   =" & Str(Val(vTot_Amount)) & "  , Total_Consumed_Weight  = " & Str(Val(vTot_Cons_Wgt)) & ",Total_Type1_Meters  =" & Str(Val(vTOT_Sound_Mtrs)) & " ,Total_Type2_Meters =" & Str(Val(vTOT_Second_Mtrs)) & "  ,   Total_Type3_Meters = " & Str(Val(vTOT_bits_Mtrs)) & "   Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and WeavingUnit_Production_Wages_Code = '" & Trim(NewCode) & "' "
                cmd.ExecuteNonQuery()

            End If


            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "NEW", "WeavingUnit_Production_Wages_Head", "WeavingUnit_Production_Wages_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "WeavingUnit_Production_Wages_Code, Company_IdNo, for_OrderBy", tr)


            EntID = Trim(Pk_Condition) & Trim(lbl_RefNo.Text)
            PBlNo = Trim(lbl_RefNo.Text)
            Partcls = "Loom : Loom.No. " & Trim(lbl_RefNo.Text)

            cmd.CommandText = "Delete from WeavingUnit_Production_Wages_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and WeavingUnit_Production_Wages_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()



            With dgv_Details
                Sno = 0
                For i = 0 To dgv_Details.RowCount - 1

                    If Trim(dgv_Details.Rows(i).Cells(DgvCol_Details.CLOTH_NAME).Value) <> "" Or Val(dgv_Details.Rows(i).Cells(DgvCol_Details.TOTAL_METERS).Value) <> 0 Then

                        Sno = Sno + 1

                        Endcnt_Id = Common_Procedures.EndsCount_NameToIdNo(con, dgv_Details.Rows(i).Cells(DgvCol_Details.ENDSCOUNT_NAME).Value, tr)
                        cloth_Idno = Common_Procedures.Cloth_NameToIdNo(con, dgv_Details.Rows(i).Cells(DgvCol_Details.CLOTH_NAME).Value, tr)
                        Wftcnt_Id = Common_Procedures.Count_NameToIdNo(con, dgv_Details.Rows(i).Cells(DgvCol_Details.WEFT_COUNT_NAME).Value, tr)


                        cmd.CommandText = "Insert into WeavingUnit_Production_Wages_Details  ( WeavingUnit_Production_Wages_Code    ,         Company_IdNo                   ,     WeavingUnit_Production_Wages_No        ,                        for_OrderBy                                       ,      WeavingUnit_Production_Wages_FromDate  , WeavingUnit_Production_Wages_ToDate  ,    Ledger_IdNo            ,    Sl_No             ,                Cloth_IdNo          ,             Endscount_IdNo          ,                     Type1_Meters                                    ,                                            Type2_Meters               ,                                   Type3_Meters                        ,                                           Total_Meters            ,                            Type1_Wages_Rate                         ,                               Type2_Wages_Rate                           ,                                     Type3_Wages_Rate                ,                                                  Total_Amount            ,                          Warp_Meters                                ,           WeftCount_IdNo        ,                               Weight_Meter_Weft                                ,                                                 Consumed_Weight        ) " &
                                                                                    " Values (  '" & Trim(NewCode) & "'             ,   " & Str(Val(lbl_Company.Tag)) & "    ,      '" & Trim(lbl_RefNo.Text) & "'        ,  " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & "  ,               @FromDate                    ,       @ToDate                        ,    " & Str(Val(Led_ID)) & "  , " & Str(Val(Sno)) & "   ,    " & Val(cloth_Idno) & "       ,    " & Str(Val(Endcnt_Id)) & "  ,  " & Str(Val(.Rows(i).Cells(DgvCol_Details.SOUND_METERS).Value)) & " , " & Str(Val(.Rows(i).Cells(DgvCol_Details.SECOND_METERS).Value)) & " ," & Str(Val(.Rows(i).Cells(DgvCol_Details.BITS_METERS).Value)) & "," & Str(Val(.Rows(i).Cells(DgvCol_Details.TOTAL_METERS).Value)) & "     ,  " & Str(Val(.Rows(i).Cells(DgvCol_Details.SOUND_WAGES).Value)) & " ,   " & Str(Val(.Rows(i).Cells(DgvCol_Details.SECONDS_WAGES).Value)) & "   ,   " & Str(Val(.Rows(i).Cells(DgvCol_Details.BITS_WAGES).Value)) & "  ,    " & Str(Val(.Rows(i).Cells(DgvCol_Details.TOTAL_AMOUNT).Value)) & "  , " & Str(Val(.Rows(i).Cells(DgvCol_Details.WARP_METERS).Value)) & "  ,     " & Str(Val(Wftcnt_Id)) & " ,     " & Str(Val(.Rows(i).Cells(DgvCol_Details.WEIGHT_CONSUMP_METER).Value)) & "  , " & Str(Val(.Rows(i).Cells(DgvCol_Details.CONSUMED_WEIGHT).Value)) & "   )"
                        cmd.ExecuteNonQuery()

                    End If

                Next
                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "NEW", "WeavingUnit_Production_Wages_Details", "WeavingUnit_Production_Wages_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, " Shift_IdNo   , Loom_IdNo, Meters   ,  Warp_Meters  ,   Weft_Meters  ,  Pick_Efficiency , Employee_IdNo", "Sl_No", "WeavingUnit_Production_Wages_Code, For_OrderBy, Company_IdNo, WeavingUnit_Production_Wages_No, WeavingUnit_Production_Wages_FromDate, Ledger_Idno", tr)

            End With

            tr.Commit()

            move_record(lbl_RefNo.Text, False)

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception
            tr.Rollback()

            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)


        Finally

            Dt1.Dispose()
            Da.Dispose()
            tr.Dispose()

            If msk_FromDate.Enabled And msk_FromDate.Visible Then msk_FromDate.Focus()

        End Try

    End Sub

    Private Sub dgv_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim Dt4 As New DataTable
        Dim Dt5 As New DataTable
        Dim Dt6 As New DataTable
        Dim Dt7 As New DataTable
        Dim rect As Rectangle


        With dgv_Details

            If Val(.CurrentRow.Cells(DgvCol_Details.SL_NO).Value) = 0 Then
                .CurrentRow.Cells(DgvCol_Details.SL_NO).Value = .CurrentRow.Index + 1
            End If

        End With
    End Sub
    'Private Sub dgv_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellLeave
    '    Try
    '        With dgv_Details
    '            If .Visible = True Then
    '                If .Rows.Count > 0 Then

    '                    If .CurrentCell.ColumnIndex <> DgvCol_Details.ENDSCOUNT_NAME And .CurrentCell.ColumnIndex <> DgvCol_Details.CLOTH_NAME And .CurrentCell.ColumnIndex <> DgvCol_Details.WEFT_COUNT_NAME And .CurrentCell.ColumnIndex <> DgvCol_Details.SL_NO Then

    '                        If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
    '                            .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
    '                            'Else
    '                            '    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
    '                        End If
    '                    End If
    '                End If
    '            End If

    '        End With

    '    Catch ex As Exception
    '        MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL LEAVE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

    '    End Try

    'End Sub
    Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged
        On Error Resume Next

        Dim vSound_Amt = ""
        Dim vSeconds_Amt = ""
        Dim vBits_Amt = ""

        If FrmLdSTS = True Or NoCalc_Status = True Or Mov_Status = True Then Exit Sub

        If IsNothing(dgv_Details.CurrentCell) Then Exit Sub

        With dgv_Details
            If .Visible Then

                vSound_Amt = 0
                vSeconds_Amt = 0
                vBits_Amt = 0

                If Val(dgv_Details.Rows(e.RowIndex).Cells(DgvCol_Details.SOUND_METERS).Value) <> 0 And Val(dgv_Details.Rows(e.RowIndex).Cells(DgvCol_Details.SOUND_WAGES).Value) <> 0 Then
                    vSound_Amt = (Val(dgv_Details.Rows(e.RowIndex).Cells(DgvCol_Details.SOUND_METERS).Value) * Val(dgv_Details.Rows(e.RowIndex).Cells(DgvCol_Details.SOUND_WAGES).Value))
                End If
                If Val(dgv_Details.Rows(e.RowIndex).Cells(DgvCol_Details.SECOND_METERS).Value) <> 0 And Val(dgv_Details.Rows(e.RowIndex).Cells(DgvCol_Details.SECONDS_WAGES).Value) <> 0 Then
                    vSeconds_Amt = (Val(dgv_Details.Rows(e.RowIndex).Cells(DgvCol_Details.SECOND_METERS).Value) * Val(dgv_Details.Rows(e.RowIndex).Cells(DgvCol_Details.SECONDS_WAGES).Value))
                End If
                If Val(dgv_Details.Rows(e.RowIndex).Cells(DgvCol_Details.BITS_METERS).Value) <> 0 And Val(dgv_Details.Rows(e.RowIndex).Cells(DgvCol_Details.BITS_WAGES).Value) <> 0 Then
                    vBits_Amt = (Val(dgv_Details.Rows(e.RowIndex).Cells(DgvCol_Details.BITS_METERS).Value) * Val(dgv_Details.Rows(e.RowIndex).Cells(DgvCol_Details.BITS_WAGES).Value))
                End If

                dgv_Details.Rows(e.RowIndex).Cells(DgvCol_Details.TOTAL_AMOUNT).Value = Format(Val(vSound_Amt) + Val(vSeconds_Amt) + Val(vBits_Amt), "########0.00")

            End If

        End With

        Total_Calculation()

    End Sub

    Private Sub dgv_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Details.EditingControlShowing
        dgtxt_Details = CType(dgv_Details.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub
    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.Enter
        dgv_Details.EditingControl.BackColor = Color.Lime
    End Sub
    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Details.KeyPress

        If IsNothing(dgv_Details.CurrentCell) Then Exit Sub

        With dgv_Details

            If .Visible Then
                ' If .CurrentCell.ColumnIndex = DgvCol_Details.WARP Or .CurrentCell.ColumnIndex = DgvCol_Details.WEFT Or .CurrentCell.ColumnIndex = DgvCol_Details.PICK_EFFICIENCY Or .CurrentCell.ColumnIndex = DgvCol_Details.SHIFT_1 Or .CurrentCell.ColumnIndex = DgvCol_Details.SHIFT_2 Or .CurrentCell.ColumnIndex = DgvCol_Details.SHIFT_3 Or .CurrentCell.ColumnIndex = DgvCol_Details.DOFF_1_METERS Or .CurrentCell.ColumnIndex = DgvCol_Details.DOFF_2_METERS Or .CurrentCell.ColumnIndex = DgvCol_Details.DOFF_3_METERS Or .CurrentCell.ColumnIndex = DgvCol_Details.DOFF_4_METERS Then
                If .CurrentCell.ColumnIndex = DgvCol_Details.SOUND_WAGES Or .CurrentCell.ColumnIndex <> DgvCol_Details.SECONDS_WAGES Or .CurrentCell.ColumnIndex <> DgvCol_Details.BITS_WAGES Then
                    If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                        e.Handled = True
                    End If

                End If
            End If
        End With

    End Sub



    Private Sub dgv_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyUp
        Dim n As Integer
        Dim nrw As Integer
        Dim LMNO As String
        Dim I As Integer

        If e.Control = True And (UCase(Chr(e.KeyCode)) = "A" Or UCase(Chr(e.KeyCode)) = "I" Or e.KeyCode = Keys.Insert) Then

            With dgv_Details

                n = .CurrentRow.Index

                LMNO = Trim(UCase(.Rows(n).Cells(DgvCol_Details.ENDSCOUNT_NAME).Value))

                nrw = n + 1

                dgv_Details.Rows.Insert(nrw, 1)

                dgv_Details.Rows(nrw).Cells(DgvCol_Details.ENDSCOUNT_NAME).Value = Trim(UCase(LMNO))

                For I = 0 To dgv_Details.Rows.Count - 1
                    dgv_Details.Rows(I).Cells(DgvCol_Details.SL_NO).Value = I + 1
                Next I

            End With

        ElseIf e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_Details

                n = .CurrentRow.Index

                If .Rows.Count = 1 Then
                    For I = 0 To .Columns.Count - 1
                        .Rows(n).Cells(I).Value = ""
                    Next

                Else

                    .Rows.RemoveAt(n)

                End If

                For I = 0 To .Rows.Count - 1
                    .Rows(I).Cells(DgvCol_Details.SL_NO).Value = I + 1
                Next

            End With

            Total_Calculation()

        End If
    End Sub

    Private Sub dgv_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.LostFocus
        On Error Resume Next
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_Details_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Details.RowsAdded
        Dim n As Integer
        If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
        With dgv_Details
            n = .RowCount
            .Rows(n - 1).Cells(DgvCol_Details.SL_NO).Value = Val(n)
        End With
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
        Dim Lom_IdNo As Integer, Sht_IdNo As Integer
        Dim Condt As String = ""

        Try

            Condt = ""
            Lom_IdNo = 0
            Sht_IdNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.WeavingUnit_Production_Wages_FromDate between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.WeavingUnit_Production_Wages_FromDate = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.WeavingUnit_Production_Wages_FromDate = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
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


            da = New SqlClient.SqlDataAdapter("select a.*, b.*, c.Loom_name, d.Employee_name from WeavingUnit_Production_Wages_Head a INNER join WeavingUnit_Production_Wages_Details b on a.WeavingUnit_Production_Wages_Code = b.WeavingUnit_Production_Wages_Code left outer join Loom_head c on b.Loom_idno = c.Loom_idno left outer join PayRoll_Employee_head d on b.Employee_idno = d.Employee_idno  where a.company_idno =" & Str(Val(lbl_Company.Tag)) & "and a.WeavingUnit_Production_Wages_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.WeavingUnit_Production_Wages_FromDate, a.for_orderby, a.WeavingUnit_Production_Wages_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()


                    dgv_Filter_Details.Rows(n).Cells(0).Value = dt2.Rows(i).Item("WeavingUnit_Production_Wages_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("WeavingUnit_Production_Wages_FromDate").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = Common_Procedures.Shift_IdNoToName(con, dt2.Rows(i).Item("Shift_Idno").ToString)
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Loom_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("Meters").ToString), "########0.00")
                    'dgv_Filter_Details.Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("Warp_Meters").ToString), "########0.00")
                    'dgv_Filter_Details.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Weft_Meters").ToString), "########0.00")
                    'dgv_Filter_Details.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Pick_Efficiency").ToString), "########0.00")
                    'dgv_Filter_Details.Rows(n).Cells(8).Value = dt2.Rows(i).Item("Employee_Name").ToString

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
            move_record(movno, False)
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
    Private Sub Total_Calculation()
        Dim Sno As Integer

        Dim vSound_Mtrs As String = 0
        Dim vSecond_Mtrs As String = 0
        Dim vbits_Mtrs As String = 0

        Dim TOTAL_METERS As String = 0
        Dim TOTAL_WARP As String = 0
        Dim TOTAL_WEFT_CONS_WGT As String = 0
        Dim TOTAL_AMOUNT As String = 0


        Sno = 0
        vSound_Mtrs = 0 : vSecond_Mtrs = 0 : vbits_Mtrs = 0 : TOTAL_WEFT_CONS_WGT = 0 : TOTAL_AMOUNT = 0
        TOTAL_WARP = 0

        If FrmLdSTS = True Or NoCalc_Status = True Or Mov_Status = True Then Exit Sub

        With dgv_Details
            For i = 0 To .RowCount - 1
                Sno = Sno + 1
                .Rows(i).Cells(DgvCol_Details.SL_NO).Value = Sno

                If Val(.Rows(i).Cells(DgvCol_Details.TOTAL_METERS).Value) <> 0 Then


                    vSound_Mtrs = vSound_Mtrs + Val(.Rows(i).Cells(DgvCol_Details.SOUND_METERS).Value)
                    vSecond_Mtrs = vSecond_Mtrs + Val(.Rows(i).Cells(DgvCol_Details.SECOND_METERS).Value)
                    vbits_Mtrs = vbits_Mtrs + Val(.Rows(i).Cells(DgvCol_Details.BITS_METERS).Value)
                    TOTAL_METERS = TOTAL_METERS + Val(.Rows(i).Cells(DgvCol_Details.TOTAL_METERS).Value)

                    TOTAL_WARP = TOTAL_WARP + Val(.Rows(i).Cells(DgvCol_Details.WARP_METERS).Value)
                    TOTAL_AMOUNT = TOTAL_AMOUNT + Val(.Rows(i).Cells(DgvCol_Details.TOTAL_AMOUNT).Value)
                    TOTAL_WEFT_CONS_WGT = TOTAL_WEFT_CONS_WGT + Val(.Rows(i).Cells(DgvCol_Details.CONSUMED_WEIGHT).Value)



                End If
            Next
        End With

        With dgv_Details_Total

            If .RowCount = 0 Then .Rows.Add()


            .Rows(0).Cells(DgvCol_Details.SOUND_METERS).Value = Format(Val(vSound_Mtrs), "########0.00")
            .Rows(0).Cells(DgvCol_Details.SECOND_METERS).Value = Format(Val(vSecond_Mtrs), "########0.00")
            .Rows(0).Cells(DgvCol_Details.BITS_METERS).Value = Format(Val(vbits_Mtrs), "########0.00")
            .Rows(0).Cells(DgvCol_Details.TOTAL_METERS).Value = Format(Val(TOTAL_METERS), "########0.00")

            .Rows(0).Cells(DgvCol_Details.WARP_METERS).Value = Format(Val(TOTAL_WARP), "########0.00")
            .Rows(0).Cells(DgvCol_Details.TOTAL_AMOUNT).Value = Format(Val(TOTAL_AMOUNT), "########0.00")
            .Rows(0).Cells(DgvCol_Details.CONSUMED_WEIGHT).Value = Format(Val(TOTAL_WEFT_CONS_WGT), "########0.000")

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

        Exit Sub

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.PrintEntry, Common_Procedures.UR.Inhouse_Loom_Production_Entry, New_Entry) = False Then Exit Sub

        Try

            da1 = New SqlClient.SqlDataAdapter("select * from WeavingUnit_Production_Wages_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and WeavingUnit_Production_Wages_Code = '" & Trim(NewCode) & "'", con)
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

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.* from WeavingUnit_Production_Wages_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.WeavingUnit_Production_Wages_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then


                da2 = New SqlClient.SqlDataAdapter("select a.*,Lh.Loom_Name,sh.Shift_Name,ch.Cloth_Name,Em.Employee_nAME AS Employee_Shift_1,em2.Employee_nAME AS Employee_Shift_2 from WeavingUnit_Production_Wages_Details a  LEFT OUTER JOIN Loom_Head Lh On a.Loom_IdNo = Lh.Loom_IdNo LEFT OUTER JOIN Shift_Head sh on a.Shift_IdNo = sh.Shift_IdNo LEFT OUTER JOIN Employee_Head EM  ON A.Employee_IdNo_Shift_1=EM.Employee_IdNo  LEFT OUTER JOIN Employee_Head EM2 on A.Employee_IdNo_Shift_2=EM2.Employee_IdNo  LEFT OUTER JOIN  Cloth_Head CH on a.Cloth_idno=CH.Cloth_Idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.WeavingUnit_Production_Wages_Code = '" & Trim(NewCode) & "' Order by a.Sl_No", con)
                ' da2 = New SqlClient.SqlDataAdapter("select a.*,eh.Employee_Name,Lh.Loom_Name,sh.Shift_Name from WeavingUnit_Production_Wages_Details a LEFT OUTER JOIN PayRoll_Employee_Head eh ON a.Employee_Idno = eh.Employee_IdNo LEFT OUTER JOIN Loom_Head Lh On a.Loom_IdNo = Lh.Loom_IdNo LEFT OUTER JOIN Shift_Head sh on a.Shift_IdNo = sh.Shift_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.WeavingUnit_Production_Wages_Code = '" & Trim(NewCode) & "' Order by a.Sl_No", con)
                'da2 = New SqlClient.SqlDataAdapter("select a.*, b.Item_name, d.Unit_name, e.Machine_name, f.Brand_Name from WeavingUnit_Production_Wages_Details a INNER JOIN Item_Head b ON a.Item_idno = b.Item_idno LEFT OUTER JOIN Unit_Head d ON a.Unit_idno = d.Unit_idno LEFT OUTER JOIN Machine_Head e ON a.Machine_idno = e.Machine_idno LEFT OUTER JOIN Brand_Head f ON a.Brand_idno = f.Brand_idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.WeavingUnit_Production_Wages_Code = '" & Trim(NewCode) & "' Order by a.Sl_No", con)
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
        If Common_Procedures.settings.CustomerCode = "1520" Then ' --- RAINBOW COTTON FABRIC 
            Printing_Format2_1520(e)
        Else
            Printing_Format1(e)
        End If


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
        Dim LnAr(15) As Single, ClAr(20) As Single
        Dim EntryCode As String
        Dim CurY As Single
        Dim ItmNm1 As String, ItmNm2 As String
        Dim ClthNm1 As String, ClthNm2 As String
        Dim EmpNm1 As String, EmpNm2 As String
        Dim EmpNm4 As String, EmpNm3 As String
        Dim Remark1 As String, Remark2 As String
        Dim itemNmStr1(20) As String
        Dim ClthNmStr1(20) As String
        Dim EmpNmStr1(20) As String
        Dim EmpNmStr2(20) As String
        Dim RemarksStr1(20) As String

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
        'If PrintDocument1.DefaultPageSettings.Landscape = False Then
        If PrintDocument1.DefaultPageSettings.Landscape = True Then
            With PrintDocument1.DefaultPageSettings.PaperSize
                PrintWidth = .Height - TMargin - BMargin
                PrintHeight = .Width - RMargin - LMargin
                PageWidth = .Height - TMargin
                PageHeight = .Width - RMargin
            End With
        End If


        Erase LnAr
        Erase ClAr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(20) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClAr(1) = Val(30) : ClAr(2) = 55 : ClAr(3) = 95 : ClAr(4) = 35 : ClAr(5) = 35 : ClAr(6) = 45 : ClAr(7) = 35 : ClAr(8) = 35 : ClAr(9) = 65 : ClAr(10) = 35 : ClAr(11) = 40 : ClAr(12) = 35 : ClAr(13) = 35 : ClAr(14) = 65 : ClAr(15) = 35 : ClAr(16) = 50
        ClAr(17) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15) + ClAr(16))

        TxtHgt = 19
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1520" Then
            NoofItems_PerPage = 35
        Else
            NoofItems_PerPage = 40
        End If


        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

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

                        Erase itemNmStr1
                        itemNmStr1 = New String(15) {}

                        ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Loom_Name").ToString)
                        ItmNm2 = ""
                        If Len(ItmNm1) > 15 Then
                            For I = 15 To 1 Step -1
                                If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 15
                            ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                            ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                        End If

                        If Trim(ItmNm2) <> "" Then
                            For k = 0 To 9
                                If Len(ItmNm2) > 15 Then

                                    For I = 15 To 1 Step -1
                                        If Mid$(Trim(ItmNm2), I, 1) = " " Or Mid$(Trim(ItmNm2), I, 1) = "," Or Mid$(Trim(ItmNm2), I, 1) = "." Or Mid$(Trim(ItmNm2), I, 1) = "-" Or Mid$(Trim(ItmNm2), I, 1) = "/" Or Mid$(Trim(ItmNm2), I, 1) = "_" Or Mid$(Trim(ItmNm2), I, 1) = "(" Or Mid$(Trim(ItmNm2), I, 1) = ")" Or Mid$(Trim(ItmNm2), I, 1) = "\" Or Mid$(Trim(ItmNm2), I, 1) = "[" Or Mid$(Trim(ItmNm2), I, 1) = "]" Or Mid$(Trim(ItmNm2), I, 1) = "{" Or Mid$(Trim(ItmNm2), I, 1) = "}" Then Exit For
                                    Next I
                                    If I = 0 Then I = 15

                                    'ClthNmStr1(k) = Microsoft.VisualBasic.Right(Trim(ItmNm2), Len(ItmNm2) - I)
                                    'ItmNm2 = Microsoft.VisualBasic.Left(Trim(ItmNm2), I - 1)

                                    itemNmStr1(k) = Microsoft.VisualBasic.Left(Trim(ItmNm2), I)
                                    ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm2), Len(ItmNm2) - I)

                                    If Len(ItmNm2) > 15 Then
                                        ItmNm2 = ItmNm2

                                    Else
                                        k = k + 1
                                        itemNmStr1(k) = ItmNm2
                                        Exit For

                                    End If


                                Else
                                    k = k + 1
                                    itemNmStr1(k) = ItmNm2
                                    Exit For

                                End If
                            Next
                        End If



                        '-------------------

                        Erase ClthNmStr1
                        ClthNmStr1 = New String(15) {}

                        ClthNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Cloth_Name").ToString)
                        ClthNm2 = ""
                        If Len(ClthNm1) > 20 Then
                            For I = 20 To 1 Step -1
                                If Mid$(Trim(ClthNm1), I, 1) = " " Or Mid$(Trim(ClthNm1), I, 1) = "," Or Mid$(Trim(ClthNm1), I, 1) = "." Or Mid$(Trim(ClthNm1), I, 1) = "-" Or Mid$(Trim(ClthNm1), I, 1) = "/" Or Mid$(Trim(ClthNm1), I, 1) = "_" Or Mid$(Trim(ClthNm1), I, 1) = "(" Or Mid$(Trim(ClthNm1), I, 1) = ")" Or Mid$(Trim(ClthNm1), I, 1) = "\" Or Mid$(Trim(ClthNm1), I, 1) = "[" Or Mid$(Trim(ClthNm1), I, 1) = "]" Or Mid$(Trim(ClthNm1), I, 1) = "{" Or Mid$(Trim(ClthNm1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 20
                            ClthNm2 = Microsoft.VisualBasic.Right(Trim(ClthNm1), Len(ClthNm1) - I)
                            ClthNm1 = Microsoft.VisualBasic.Left(Trim(ClthNm1), I - 1)
                        End If


                        If Trim(ClthNm2) <> "" Then
                            For k = 0 To 9
                                If Len(ClthNm2) > 20 Then

                                    For I = 20 To 1 Step -1
                                        If Mid$(Trim(ClthNm2), I, 1) = " " Or Mid$(Trim(ClthNm2), I, 1) = "," Or Mid$(Trim(ClthNm2), I, 1) = "." Or Mid$(Trim(ClthNm2), I, 1) = "-" Or Mid$(Trim(ClthNm2), I, 1) = "/" Or Mid$(Trim(ClthNm2), I, 1) = "_" Or Mid$(Trim(ClthNm2), I, 1) = "(" Or Mid$(Trim(ClthNm2), I, 1) = ")" Or Mid$(Trim(ClthNm2), I, 1) = "\" Or Mid$(Trim(ClthNm2), I, 1) = "[" Or Mid$(Trim(ClthNm2), I, 1) = "]" Or Mid$(Trim(ClthNm2), I, 1) = "{" Or Mid$(Trim(ClthNm2), I, 1) = "}" Then Exit For
                                    Next I
                                    If I = 0 Then I = 20

                                    'ClthNmStr1(k) = Microsoft.VisualBasic.Right(Trim(ClthNm2), Len(ClthNm2) - I)
                                    'ClthNm2 = Microsoft.VisualBasic.Left(Trim(ClthNm2), I - 1)

                                    ClthNmStr1(k) = Microsoft.VisualBasic.Left(Trim(ClthNm2), I)
                                    ClthNm2 = Microsoft.VisualBasic.Right(Trim(ClthNm2), Len(ClthNm2) - I)

                                    If Len(ClthNm2) > 15 Then
                                        ClthNm2 = ClthNm2

                                    Else
                                        k = k + 1
                                        ClthNmStr1(k) = ClthNm2
                                        Exit For

                                    End If


                                Else
                                    k = k + 1
                                    ClthNmStr1(k) = ClthNm2
                                    Exit For

                                End If
                            Next
                        End If

                        '-----------------

                        Erase EmpNmStr1
                        EmpNmStr1 = New String(10) {}

                        EmpNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Employee_shift_1").ToString)
                        EmpNm2 = ""

                        If Len(EmpNm1) > 10 Then
                            For I = 10 To 1 Step -1
                                If Mid$(Trim(EmpNm1), I, 1) = " " Or Mid$(Trim(EmpNm1), I, 1) = "," Or Mid$(Trim(EmpNm1), I, 1) = "." Or Mid$(Trim(EmpNm1), I, 1) = "-" Or Mid$(Trim(EmpNm1), I, 1) = "/" Or Mid$(Trim(EmpNm1), I, 1) = "_" Or Mid$(Trim(EmpNm1), I, 1) = "(" Or Mid$(Trim(EmpNm1), I, 1) = ")" Or Mid$(Trim(EmpNm1), I, 1) = "\" Or Mid$(Trim(EmpNm1), I, 1) = "[" Or Mid$(Trim(EmpNm1), I, 1) = "]" Or Mid$(Trim(EmpNm1), I, 1) = "{" Or Mid$(Trim(EmpNm1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 10
                            EmpNm2 = Microsoft.VisualBasic.Right(Trim(EmpNm1), Len(EmpNm1) - I)
                            EmpNm1 = Microsoft.VisualBasic.Left(Trim(EmpNm1), I - 1)
                        End If


                        If Trim(EmpNm2) <> "" Then
                            For k = 0 To 9
                                If Len(EmpNm2) > 9 Then

                                    For I = 9 To 1 Step -1
                                        If Mid$(Trim(EmpNm2), I, 1) = " " Or Mid$(Trim(EmpNm2), I, 1) = "," Or Mid$(Trim(EmpNm2), I, 1) = "." Or Mid$(Trim(EmpNm2), I, 1) = "-" Or Mid$(Trim(EmpNm2), I, 1) = "/" Or Mid$(Trim(EmpNm2), I, 1) = "_" Or Mid$(Trim(EmpNm2), I, 1) = "(" Or Mid$(Trim(EmpNm2), I, 1) = ")" Or Mid$(Trim(EmpNm2), I, 1) = "\" Or Mid$(Trim(EmpNm2), I, 1) = "[" Or Mid$(Trim(EmpNm2), I, 1) = "]" Or Mid$(Trim(EmpNm2), I, 1) = "{" Or Mid$(Trim(EmpNm2), I, 1) = "}" Then Exit For
                                    Next I
                                    If I = 0 Then I = 9



                                    EmpNmStr1(k) = Microsoft.VisualBasic.Left(Trim(EmpNm2), I)
                                    EmpNm2 = Microsoft.VisualBasic.Right(Trim(EmpNm2), Len(EmpNm2) - I)

                                    If Len(EmpNm2) > 9 Then
                                        EmpNm2 = EmpNm2

                                    Else
                                        k = k + 1
                                        EmpNmStr1(k) = EmpNm2
                                        Exit For

                                    End If


                                Else
                                    k = k + 1
                                    EmpNmStr1(k) = EmpNm2
                                    Exit For

                                End If
                            Next
                        End If

                        '---------------------

                        '-----------------

                        Erase EmpNmStr2
                        EmpNmStr2 = New String(10) {}

                        EmpNm3 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Employee_shift_2").ToString)
                        EmpNm4 = ""

                        If Len(EmpNm3) > 10 Then
                            For I = 10 To 1 Step -1
                                If Mid$(Trim(EmpNm3), I, 1) = " " Or Mid$(Trim(EmpNm3), I, 1) = "," Or Mid$(Trim(EmpNm3), I, 1) = "." Or Mid$(Trim(EmpNm3), I, 1) = "-" Or Mid$(Trim(EmpNm3), I, 1) = "/" Or Mid$(Trim(EmpNm3), I, 1) = "_" Or Mid$(Trim(EmpNm3), I, 1) = "(" Or Mid$(Trim(EmpNm3), I, 1) = ")" Or Mid$(Trim(EmpNm3), I, 1) = "\" Or Mid$(Trim(EmpNm3), I, 1) = "[" Or Mid$(Trim(EmpNm3), I, 1) = "]" Or Mid$(Trim(EmpNm3), I, 1) = "{" Or Mid$(Trim(EmpNm3), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 10
                            EmpNm4 = Microsoft.VisualBasic.Right(Trim(EmpNm3), Len(EmpNm3) - I)
                            EmpNm3 = Microsoft.VisualBasic.Left(Trim(EmpNm3), I - 1)
                        End If


                        If Trim(EmpNm4) <> "" Then
                            For k = 0 To 9
                                If Len(EmpNm4) > 10 Then

                                    For I = 9 To 1 Step -1
                                        If Mid$(Trim(EmpNm4), I, 1) = " " Or Mid$(Trim(EmpNm4), I, 1) = "," Or Mid$(Trim(EmpNm4), I, 1) = "." Or Mid$(Trim(EmpNm4), I, 1) = "-" Or Mid$(Trim(EmpNm4), I, 1) = "/" Or Mid$(Trim(EmpNm4), I, 1) = "_" Or Mid$(Trim(EmpNm4), I, 1) = "(" Or Mid$(Trim(EmpNm4), I, 1) = ")" Or Mid$(Trim(EmpNm4), I, 1) = "\" Or Mid$(Trim(EmpNm4), I, 1) = "[" Or Mid$(Trim(EmpNm4), I, 1) = "]" Or Mid$(Trim(EmpNm4), I, 1) = "{" Or Mid$(Trim(EmpNm4), I, 1) = "}" Then Exit For
                                    Next I
                                    If I = 0 Then I = 9

                                    'ClthNmStr1(k) = Microsoft.VisualBasic.Right(Trim(EMPNM4), Len(EMPNM4) - I)
                                    'EMPNM4 = Microsoft.VisualBasic.Left(Trim(EMPNM4), I - 1)

                                    EmpNmStr2(k) = Microsoft.VisualBasic.Left(Trim(EmpNm4), I)
                                    EmpNm4 = Microsoft.VisualBasic.Right(Trim(EmpNm4), Len(EmpNm4) - I)

                                    If Len(EmpNm4) > 10 Then
                                        EmpNm4 = EmpNm4

                                    Else
                                        k = k + 1
                                        EmpNmStr2(k) = EmpNm4
                                        Exit For

                                    End If


                                Else
                                    k = k + 1
                                    EmpNmStr2(k) = EmpNm4
                                    Exit For

                                End If
                            Next
                        End If

                        '---------------------


                        Erase RemarksStr1
                        RemarksStr1 = New String(10) {}

                        Remark1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Remarks").ToString)
                        Remark2 = ""

                        If Len(Remark1) > 15 Then
                            For I = 15 To 1 Step -1
                                If Mid$(Trim(Remark1), I, 1) = " " Or Mid$(Trim(Remark1), I, 1) = "," Or Mid$(Trim(Remark1), I, 1) = "." Or Mid$(Trim(Remark1), I, 1) = "-" Or Mid$(Trim(Remark1), I, 1) = "/" Or Mid$(Trim(Remark1), I, 1) = "_" Or Mid$(Trim(Remark1), I, 1) = "(" Or Mid$(Trim(Remark1), I, 1) = ")" Or Mid$(Trim(Remark1), I, 1) = "\" Or Mid$(Trim(Remark1), I, 1) = "[" Or Mid$(Trim(Remark1), I, 1) = "]" Or Mid$(Trim(Remark1), I, 1) = "{" Or Mid$(Trim(Remark1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 15
                            Remark2 = Microsoft.VisualBasic.Right(Trim(Remark1), Len(Remark1) - I)
                            Remark1 = Microsoft.VisualBasic.Left(Trim(Remark1), I - 1)
                        End If


                        If Trim(Remark2) <> "" Then
                            For k = 0 To 9
                                If Len(Remark2) > 15 Then

                                    For I = 10 To 1 Step -1
                                        If Mid$(Trim(Remark2), I, 1) = " " Or Mid$(Trim(Remark2), I, 1) = "," Or Mid$(Trim(Remark2), I, 1) = "." Or Mid$(Trim(Remark2), I, 1) = "-" Or Mid$(Trim(Remark2), I, 1) = "/" Or Mid$(Trim(Remark2), I, 1) = "_" Or Mid$(Trim(Remark2), I, 1) = "(" Or Mid$(Trim(Remark2), I, 1) = ")" Or Mid$(Trim(Remark2), I, 1) = "\" Or Mid$(Trim(Remark2), I, 1) = "[" Or Mid$(Trim(Remark2), I, 1) = "]" Or Mid$(Trim(Remark2), I, 1) = "{" Or Mid$(Trim(Remark2), I, 1) = "}" Then Exit For
                                    Next I
                                    If I = 0 Then I = 10

                                    RemarksStr1(k) = Microsoft.VisualBasic.Left(Trim(Remark2), I)
                                    Remark2 = Microsoft.VisualBasic.Right(Trim(Remark2), Len(Remark2) - I)

                                    If Len(Remark2) > 10 Then
                                        Remark2 = Remark2

                                    Else
                                        k = k + 1
                                        RemarksStr1(k) = Remark2
                                        Exit For

                                    End If
                                Else
                                    k = k + 1
                                    RemarksStr1(k) = Remark2
                                    Exit For

                                End If
                            Next
                        End If

                        '---------------------



                        pFont = New Font("Calibri", 8, FontStyle.Regular)
                        CurY = CurY + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Sl_No").ToString), LMargin + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(ClthNm1), LMargin + ClAr(1) + ClAr(2), CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("RPM").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 3, CurY, 1, 0, pFont)

                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Shift_1_Pick_Efficiency").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 3, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Shift1_Mtrs").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 3, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Shift_1_Warp_Breakage").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 3, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Shift_1_weft_Breakage").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 3, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(EmpNm1), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 0, 0, pFont)

                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Shift_2_Pick_Efficiency").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) - 3, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Shift2_Mtrs").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) - 3, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Shift_2_Warp_Breakage").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) - 3, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Shift_2_Warp_Breakage").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) - 3, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(EmpNm3), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13), CurY, 0, 0, pFont)

                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Pick_Efficiency").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15) - 3, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Meters").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15) + ClAr(16) - 3, CurY, 1, 0, pFont)

                        If Trim(Remark1) <> "" Then
                            Common_Procedures.Print_To_PrintDocument(e, Trim(Remark1), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15) + ClAr(16), CurY, 0, 0, pFont)
                        End If

                        NoofDets = NoofDets + 1


                        'If Trim(ItmNm2) <> "" Then
                        '    '     CurY = CurY + TxtHgt
                        '    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)

                        'End If
                        For I = 0 To 9
                            If Trim(itemNmStr1(I)) <> "" Or Trim(ClthNmStr1(I)) <> "" Or Trim(EmpNmStr1(I)) <> "" Or Trim(EmpNmStr2(I)) <> "" Or Trim(RemarksStr1(I)) <> "" Then
                                CurY = CurY + TxtHgt
                                NoofDets = NoofDets + 1
                                Common_Procedures.Print_To_PrintDocument(e, Trim(itemNmStr1(I)), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ClthNmStr1(I)), LMargin + ClAr(1) + ClAr(2), CurY, 0, PageWidth, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Trim(EmpNmStr1(I)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Trim(EmpNmStr2(I)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13), CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Trim(RemarksStr1(I)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15) + ClAr(16), CurY, 0, 0, pFont)

                            End If
                            'If Trim(EmpNmStr1(I)) <> "" Then
                            '    CurY = CurY + TxtHgt
                            '    NoofDets = NoofDets + 1
                            '    Common_Procedures.Print_To_PrintDocument(e, Trim(EmpNmStr1(I)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 0, 0, pFont)
                            'End If

                            'If Trim(EmpNmStr2(I)) <> "" Then
                            '    CurY = CurY + TxtHgt
                            '    NoofDets = NoofDets + 1
                            '    Common_Procedures.Print_To_PrintDocument(e, Trim(EmpNmStr2(I)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13), CurY, 0, 0, pFont)
                            'End If
                        Next

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
        Dim C2 As Single
        Dim W1 As Single
        Dim S1 As Single

        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*,eh.Employee_Name,Lh.Loom_Name,sh.Shift_Name from WeavingUnit_Production_Wages_Details a LEFT OUTER JOIN PayRoll_Employee_Head eh ON a.Employee_Idno = eh.Employee_IdNo LEFT OUTER JOIN Loom_Head Lh On a.Loom_IdNo = Lh.Loom_IdNo LEFT OUTER JOIN Shift_Head sh on a.Shift_IdNo = sh.Shift_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.WeavingUnit_Production_Wages_Code = '" & Trim(EntryCode) & "' Order by a.Sl_No", con)
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

        If Trim(Common_Procedures.settings.CustomerCode) <> "1234" Then '-----ARULJOTHI EXPORTS PVT LTD

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

        End If


        'CurY = CurY + TxtHgt - 5
        'p1Font = New Font("Calibri", 16, FontStyle.Bold)
        'Common_Procedures.Print_To_PrintDocument(e, "DELIVERY", LMargin, CurY, 2, PrintWidth, p1Font)
        'strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY
        C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4)
        C2 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9)
        W1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14)

        Common_Procedures.Print_To_PrintDocument(e, "DATE :  " & Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("WeavingUnit_Production_Wages_FromDate").ToString), "dd-MM-yyyy").ToString, LMargin + ClAr(1), CurY, 2, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "SHIFT A", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "SHIFT B", LMargin + C2 + 50, CurY, 2, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + W1 + 50, CurY, 2, 0, pFont)

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY


        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))
        e.Graphics.DrawLine(Pens.Black, LMargin + C2, CurY, LMargin + C2, LnAr(2))
        e.Graphics.DrawLine(Pens.Black, LMargin + W1, CurY, LMargin + W1, LnAr(2))

        pFont = New Font("Calibri", 8, FontStyle.Bold)
        CurY = CurY + TxtHgt - 20
        Common_Procedures.Print_To_PrintDocument(e, "SL", LMargin, CurY, 2, ClAr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin, CurY + 15, 2, ClAr(1), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "LOOM", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin + ClAr(1), CurY + 15, 2, ClAr(2), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "CLOTH", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "NAME", LMargin + ClAr(1) + ClAr(2), CurY + 15, 2, ClAr(3), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "RPM", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "PICK", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "EFF", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY + 15, 2, ClAr(5), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "METER", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "WARP", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "WEFT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "EMPLOYEE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "PICK", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, 2, ClAr(10), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "EFF", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY + 15, 2, ClAr(10), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "METER", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, 2, ClAr(11), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "WARP", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), CurY, 2, ClAr(12), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "WEFT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), CurY, 2, ClAr(13), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "EMPLOYEE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13), CurY, 2, ClAr(14), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14), CurY, 2, ClAr(15), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "EFF", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14), CurY + 15, 2, ClAr(15), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15), CurY, 2, ClAr(16), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15), CurY + 15, 2, ClAr(16), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "REMARKS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15) + ClAr(16), CurY, 2, ClAr(17), pFont)

        CurY = CurY + TxtHgt + 15
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
        pFont = New Font("Calibri", 8, FontStyle.Regular)
        CurY = CurY + TxtHgt - 10
        If is_LastPage = True Then
            Common_Procedures.Print_To_PrintDocument(e, " TOTAL", LMargin + ClAr(1) + ClAr(2) + 20, CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Avg_RPM").ToString), "########0"), LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Avg_Shift_1_Pick_Efficiency").ToString), "#########0"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Shift1_Mtrs").ToString), "########0"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Avg_Shift_2_Pick_Efficiency").ToString), "#########0"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, 2, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Shift2_Mtrs").ToString), "########0"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, 2, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_PickEfficiency").ToString), "########0"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14), CurY, 2, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString), "#########0"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15), CurY, 2, 0, pFont)
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
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), LnAr(3))

        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15) + ClAr(16), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15) + ClAr(16), LnAr(3))



        If is_LastPage = True Then
            If Val(prn_HdDt.Rows(0).Item("EB_Units_Consumed").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("EB_Amount").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("Employee_Salary").ToString) <> 0 Then
                Dim vStringWidth As Single = 0

                CurY = CurY + TxtHgt - 15

                Common_Procedures.Print_To_PrintDocument(e, "EB Units :  " & Format(Val(prn_HdDt.Rows(0).Item("EB_Units_Consumed").ToString), "##########0.00"), LMargin + 10, CurY, 0, 0, pFont)

                Common_Procedures.Print_To_PrintDocument(e, "EB Amount :  " & Format(Val(prn_HdDt.Rows(0).Item("EB_Amount").ToString), "##########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 10, CurY, 0, 0, pFont)

                Common_Procedures.Print_To_PrintDocument(e, "Employee Salary :  " & Format(Val(prn_HdDt.Rows(0).Item("Employee_Salary").ToString), "##########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + 10, CurY, 0, 0, pFont)

                CurY = CurY + TxtHgt
                e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            End If
        End If



        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        p1Font = New Font("Calibri", 12, FontStyle.Bold)

        If is_LastPage = True Then
            CurY = CurY + TxtHgt - 5
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt + 10
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt


            Common_Procedures.Print_To_PrintDocument(e, "Prepared by", LMargin + ClAr(1) + ClAr(2) + 20, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Checked by", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 1, 0, pFont)
            'End If
            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(6))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(6))
        End If
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

    End Sub



    Private Sub cbo_Filter_Shift_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_Shift.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Shift_Head", "Shift_Name", "", "(Shift_idno = 0)")
    End Sub

    Private Sub cbo_Filter_Shift_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_Shift.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_Shift, dtp_Filter_ToDate, cbo_Filter_Loom, "Shift_Head", "Shift_Name", "", "(Shift_idno = 0)")

    End Sub

    Private Sub cbo_Filter_Shift_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_Shift.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_Shift, cbo_Filter_Loom, "Shift_Head", "Shift_Name", "", "(Shift_idno = 0)")

    End Sub

    Private Sub msk_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_FromDate.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        vmskOldText = ""
        vmskSelStrt = -1
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            vmskOldText = msk_FromDate.Text
            vmskSelStrt = msk_FromDate.SelectionStart
        End If

        If (e.KeyValue = 40) Then
            msk_ToDate.Focus()
        End If

    End Sub

    Private Sub msk_Date_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles msk_FromDate.KeyPress
        Dim vTotMtr As String

        If Trim(UCase(e.KeyChar)) = "D" Then
            msk_FromDate.Text = Date.Today
            msk_FromDate.SelectionStart = 0
        End If

        Try

            If Asc(e.KeyChar) = 13 Then
                msk_ToDate.Focus()
            End If

        Catch ex As Exception
            '----

        End Try

        'If Asc(e.KeyChar) = 13 Then
        '    dgv_Details.Focus()
        '    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
        'End If
    End Sub


    Private Sub msk_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_FromDate.KeyUp
        Dim vmRetTxt As String = ""
        Dim vmRetSelStrt As Integer = -1

        'If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
        '    msk_Date.Text = Date.Today
        'End If
        If e.KeyCode = 107 Then
            msk_FromDate.Text = DateAdd("D", 1, Convert.ToDateTime(msk_FromDate.Text))
        ElseIf e.KeyCode = 109 Then
            msk_FromDate.Text = DateAdd("D", -1, Convert.ToDateTime(msk_FromDate.Text))
        End If

        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            Common_Procedures.maskEdit_Date_ON_DelBackSpace(sender, e, vmskOldText, vmskSelStrt)
        End If

    End Sub

    Private Sub dtp_Date_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_FromDate.TextChanged

        If IsDate(dtp_FromDate.Text) = True Then

            msk_FromDate.Text = dtp_FromDate.Text
            msk_FromDate.SelectionStart = 0
        End If
    End Sub

    Private Sub msk_Date_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles msk_FromDate.LostFocus

        If IsDate(msk_FromDate.Text) = True Then
            If Microsoft.VisualBasic.DateAndTime.Day(Convert.ToDateTime(msk_FromDate.Text)) <= 31 Or Microsoft.VisualBasic.DateAndTime.Month(Convert.ToDateTime(msk_FromDate.Text)) <= 31 Then
                If Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_FromDate.Text)) <= 2050 And Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_FromDate.Text)) >= 2000 Then
                    dtp_FromDate.Value = Convert.ToDateTime(msk_FromDate.Text)
                End If
            End If

        End If
    End Sub

    Private Sub dtp_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_FromDate.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            dtp_FromDate.Text = Date.Today
        End If
    End Sub

    Private Sub btn_UserModification_Click(sender As System.Object, e As System.EventArgs) Handles btn_UserModification.Click
        If Val(Common_Procedures.User.IdNo) = 1 Then
            Dim f1 As New User_Modifications
            f1.Entry_Name = Me.Name
            f1.Entry_PkValue = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            f1.ShowDialog()
        End If
    End Sub
    Private Sub dgtxt_Details_KeyUp(sender As Object, e As KeyEventArgs) Handles dgtxt_Details.KeyUp
        Try
            With dgv_Details
                If .Visible = True Then
                    If .Rows.Count > 0 Then
                        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
                            dgv_Details_KeyUp(sender, e)
                        End If
                    End If
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXT KEYUP....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub dgtxt_Details_TextChanged(sender As Object, e As EventArgs) Handles dgtxt_Details.TextChanged
        Try
            With dgv_Details

                If .Visible Then
                    If .Rows.Count > 0 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(dgtxt_Details.Text)
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

    Private Sub dtp_Filter_ToDate_KeyDown(sender As Object, e As KeyEventArgs) Handles dtp_Filter_ToDate.KeyDown

        If (e.KeyValue = 38) Then
            dtp_Filter_Fromdate.Focus()
        End If


        If (e.KeyValue = 40) Then
            cbo_Filter_Loom.Focus()
        End If

    End Sub

    Private Sub dtp_Filter_ToDate_KeyPress(sender As Object, e As KeyPressEventArgs) Handles dtp_Filter_ToDate.KeyPress
        If Asc(e.KeyChar) = 13 Then
            cbo_Filter_Loom.Focus()
        End If
    End Sub

    Private Sub btn_Selection_Click(sender As Object, e As EventArgs) Handles btn_List_LoomDetails.Click
        Get_Production_Details()
    End Sub

    Private Sub msk_Date_GotFocus(sender As Object, e As EventArgs) Handles msk_FromDate.GotFocus
        msk_FromDate.Tag = msk_FromDate.Text
    End Sub
    Private Sub Cbo_Grid_Cloth_Name_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")
    End Sub

    Private Sub btn_Print_Click(sender As Object, e As EventArgs) Handles btn_Print.Click

        Common_Procedures.Print_OR_Preview_Status = 1
        print_record()
    End Sub
    Private Sub Printing_Format2_1520(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim pFont As Font
        Dim ps As Printing.PaperSize
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim I As Integer
        Dim NoofItems_PerPage As Integer, NoofDets As Integer
        Dim TxtHgt As Single
        Dim PpSzSTS As Boolean = False
        Dim LnAr(15) As Single, ClAr(20) As Single
        Dim EntryCode As String
        Dim CurY As Single
        Dim ItmNm1 As String, ItmNm2 As String
        Dim ClthNm1 As String, ClthNm2 As String
        Dim EmpNm1 As String, EmpNm2 As String
        Dim EmpNm4 As String, EmpNm3 As String
        Dim Remark1 As String, Remark2 As String
        Dim itemNmStr1(20) As String
        Dim ClthNmStr1(20) As String
        Dim EmpNmStr1(20) As String
        Dim EmpNmStr2(20) As String
        Dim RemarksStr1(20) As String
        Dim PartyNameStr1(20) As String
        Dim PartNm1 As String, PartNm2 As String

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
        'If PrintDocument1.DefaultPageSettings.Landscape = False Then
        If PrintDocument1.DefaultPageSettings.Landscape = True Then
            With PrintDocument1.DefaultPageSettings.PaperSize
                PrintWidth = .Height - TMargin - BMargin
                PrintHeight = .Width - RMargin - LMargin
                PageWidth = .Height - TMargin
                PageHeight = .Width - RMargin
            End With
        End If


        Erase LnAr
        Erase ClAr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(20) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClAr(1) = Val(25) : ClAr(2) = 40 : ClAr(3) = 95 : ClAr(4) = 30 : ClAr(5) = 30 : ClAr(6) = 35 : ClAr(7) = 25 : ClAr(8) = 25 : ClAr(9) = 50 : ClAr(10) = 30 : ClAr(11) = 35 : ClAr(12) = 25 : ClAr(13) = 25 : ClAr(14) = 50 : ClAr(15) = 30 : ClAr(16) = 40 : ClAr(17) = 45 : ClAr(18) = 45 : ClAr(19) = 30
        ClAr(20) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15) + ClAr(16) + ClAr(17) + ClAr(18) + ClAr(19))

        TxtHgt = 19
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1520" Then
            NoofItems_PerPage = 35
        Else
            NoofItems_PerPage = 40
        End If


        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        ' Try
        If prn_HdDt.Rows.Count > 0 Then

            Printing_Format2_1520_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr)

            NoofDets = 0

            CurY = CurY - 10

            If prn_DetDt.Rows.Count > 0 Then

                Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                    If NoofDets >= NoofItems_PerPage Then

                        CurY = CurY + TxtHgt

                        Common_Procedures.Print_To_PrintDocument(e, "Continued...", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 10, CurY, 0, 0, pFont)

                        NoofDets = NoofDets + 1

                        Printing_Format2_1520_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, False)

                        e.HasMorePages = True
                        Return

                    End If

                    Erase itemNmStr1
                    itemNmStr1 = New String(15) {}

                    ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Loom_Name").ToString)
                    ItmNm2 = ""
                    If Len(ItmNm1) > 5 Then
                        For I = 5 To 1 Step -1
                            If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                        Next I
                        If I = 0 Then I = 5
                        ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                        ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                    End If

                    If Trim(ItmNm2) <> "" Then
                        For k = 0 To 8
                            If Len(ItmNm2) > 5 Then

                                For I = 5 To 1 Step -1
                                    If Mid$(Trim(ItmNm2), I, 1) = " " Or Mid$(Trim(ItmNm2), I, 1) = "," Or Mid$(Trim(ItmNm2), I, 1) = "." Or Mid$(Trim(ItmNm2), I, 1) = "-" Or Mid$(Trim(ItmNm2), I, 1) = "/" Or Mid$(Trim(ItmNm2), I, 1) = "_" Or Mid$(Trim(ItmNm2), I, 1) = "(" Or Mid$(Trim(ItmNm2), I, 1) = ")" Or Mid$(Trim(ItmNm2), I, 1) = "\" Or Mid$(Trim(ItmNm2), I, 1) = "[" Or Mid$(Trim(ItmNm2), I, 1) = "]" Or Mid$(Trim(ItmNm2), I, 1) = "{" Or Mid$(Trim(ItmNm2), I, 1) = "}" Then Exit For
                                Next I
                                If I = 0 Then I = 5

                                'ClthNmStr1(k) = Microsoft.VisualBasic.Right(Trim(ItmNm2), Len(ItmNm2) - I)
                                'ItmNm2 = Microsoft.VisualBasic.Left(Trim(ItmNm2), I - 1)

                                itemNmStr1(k) = Microsoft.VisualBasic.Left(Trim(ItmNm2), I)
                                ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm2), Len(ItmNm2) - I)

                                If Len(ItmNm2) > 5 Then
                                    ItmNm2 = ItmNm2

                                Else
                                    k = k + 1
                                    itemNmStr1(k) = ItmNm2
                                    Exit For

                                End If


                            Else
                                'k = k + 1
                                itemNmStr1(k) = ItmNm2
                                Exit For

                            End If
                        Next
                    End If



                    '-------------------

                    Erase ClthNmStr1
                    ClthNmStr1 = New String(15) {}

                    ClthNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Cloth_Name").ToString)
                    ClthNm2 = ""
                    If Len(ClthNm1) > 20 Then
                        For I = 20 To 1 Step -1
                            If Mid$(Trim(ClthNm1), I, 1) = " " Or Mid$(Trim(ClthNm1), I, 1) = "," Or Mid$(Trim(ClthNm1), I, 1) = "." Or Mid$(Trim(ClthNm1), I, 1) = "-" Or Mid$(Trim(ClthNm1), I, 1) = "/" Or Mid$(Trim(ClthNm1), I, 1) = "_" Or Mid$(Trim(ClthNm1), I, 1) = "(" Or Mid$(Trim(ClthNm1), I, 1) = ")" Or Mid$(Trim(ClthNm1), I, 1) = "\" Or Mid$(Trim(ClthNm1), I, 1) = "[" Or Mid$(Trim(ClthNm1), I, 1) = "]" Or Mid$(Trim(ClthNm1), I, 1) = "{" Or Mid$(Trim(ClthNm1), I, 1) = "}" Then Exit For
                        Next I
                        If I = 0 Then I = 20
                        ClthNm2 = Microsoft.VisualBasic.Right(Trim(ClthNm1), Len(ClthNm1) - I)
                        ClthNm1 = Microsoft.VisualBasic.Left(Trim(ClthNm1), I - 1)
                    End If


                    If Trim(ClthNm2) <> "" Then
                        For k = 0 To 8
                            If Len(ClthNm2) > 20 Then

                                For I = 20 To 1 Step -1
                                    If Mid$(Trim(ClthNm2), I, 1) = " " Or Mid$(Trim(ClthNm2), I, 1) = "," Or Mid$(Trim(ClthNm2), I, 1) = "." Or Mid$(Trim(ClthNm2), I, 1) = "-" Or Mid$(Trim(ClthNm2), I, 1) = "/" Or Mid$(Trim(ClthNm2), I, 1) = "_" Or Mid$(Trim(ClthNm2), I, 1) = "(" Or Mid$(Trim(ClthNm2), I, 1) = ")" Or Mid$(Trim(ClthNm2), I, 1) = "\" Or Mid$(Trim(ClthNm2), I, 1) = "[" Or Mid$(Trim(ClthNm2), I, 1) = "]" Or Mid$(Trim(ClthNm2), I, 1) = "{" Or Mid$(Trim(ClthNm2), I, 1) = "}" Then Exit For
                                Next I
                                If I = 0 Then I = 20

                                'ClthNmStr1(k) = Microsoft.VisualBasic.Right(Trim(ClthNm2), Len(ClthNm2) - I)
                                'ClthNm2 = Microsoft.VisualBasic.Left(Trim(ClthNm2), I - 1)

                                ClthNmStr1(k) = Microsoft.VisualBasic.Left(Trim(ClthNm2), I)
                                ClthNm2 = Microsoft.VisualBasic.Right(Trim(ClthNm2), Len(ClthNm2) - I)

                                If Len(ClthNm2) > 20 Then
                                    ClthNmStr1(k) = ClthNm2

                                    'ClthNm2 = ClthNm2

                                Else
                                    k = k + 1
                                    ClthNmStr1(k) = ClthNm2
                                    Exit For

                                End If


                            Else
                                ' k = k + 1
                                ClthNmStr1(k) = ClthNm2
                                Exit For

                            End If
                        Next
                    End If

                    '-----------------

                    Erase EmpNmStr1
                    EmpNmStr1 = New String(10) {}

                    EmpNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Employee_shift_1").ToString)
                    EmpNm2 = ""

                    If Len(EmpNm1) > 8 Then
                        For I = 8 To 1 Step -1
                            If Mid$(Trim(EmpNm1), I, 1) = " " Or Mid$(Trim(EmpNm1), I, 1) = "," Or Mid$(Trim(EmpNm1), I, 1) = "." Or Mid$(Trim(EmpNm1), I, 1) = "-" Or Mid$(Trim(EmpNm1), I, 1) = "/" Or Mid$(Trim(EmpNm1), I, 1) = "_" Or Mid$(Trim(EmpNm1), I, 1) = "(" Or Mid$(Trim(EmpNm1), I, 1) = ")" Or Mid$(Trim(EmpNm1), I, 1) = "\" Or Mid$(Trim(EmpNm1), I, 1) = "[" Or Mid$(Trim(EmpNm1), I, 1) = "]" Or Mid$(Trim(EmpNm1), I, 1) = "{" Or Mid$(Trim(EmpNm1), I, 1) = "}" Then Exit For
                        Next I
                        If I = 0 Then I = 8
                        EmpNm2 = Microsoft.VisualBasic.Right(Trim(EmpNm1), Len(EmpNm1) - I)
                        EmpNm1 = Microsoft.VisualBasic.Left(Trim(EmpNm1), I - 1)
                    End If


                    If Trim(EmpNm2) <> "" Then
                        For k = 0 To 8
                            If Len(EmpNm2) > 8 Then

                                For I = 8 To 1 Step -1
                                    If Mid$(Trim(EmpNm2), I, 1) = " " Or Mid$(Trim(EmpNm2), I, 1) = "," Or Mid$(Trim(EmpNm2), I, 1) = "." Or Mid$(Trim(EmpNm2), I, 1) = "-" Or Mid$(Trim(EmpNm2), I, 1) = "/" Or Mid$(Trim(EmpNm2), I, 1) = "_" Or Mid$(Trim(EmpNm2), I, 1) = "(" Or Mid$(Trim(EmpNm2), I, 1) = ")" Or Mid$(Trim(EmpNm2), I, 1) = "\" Or Mid$(Trim(EmpNm2), I, 1) = "[" Or Mid$(Trim(EmpNm2), I, 1) = "]" Or Mid$(Trim(EmpNm2), I, 1) = "{" Or Mid$(Trim(EmpNm2), I, 1) = "}" Then Exit For
                                Next I
                                If I = 0 Then I = 8



                                EmpNmStr1(k) = Microsoft.VisualBasic.Left(Trim(EmpNm2), I)
                                EmpNm2 = Microsoft.VisualBasic.Right(Trim(EmpNm2), Len(EmpNm2) - I)

                                If Len(EmpNm2) > 8 Then
                                    EmpNm2 = EmpNm2

                                Else
                                    k = k + 1
                                    EmpNmStr1(k) = EmpNm2
                                    Exit For

                                End If


                            Else
                                'k = k + 1
                                EmpNmStr1(k) = EmpNm2
                                Exit For

                            End If
                        Next
                    End If

                    '---------------------

                    '-----------------

                    Erase EmpNmStr2
                    EmpNmStr2 = New String(10) {}

                    EmpNm3 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Employee_shift_2").ToString)
                    EmpNm4 = ""

                    If Len(EmpNm3) > 8 Then
                        For I = 8 To 1 Step -1
                            If Mid$(Trim(EmpNm3), I, 1) = " " Or Mid$(Trim(EmpNm3), I, 1) = "," Or Mid$(Trim(EmpNm3), I, 1) = "." Or Mid$(Trim(EmpNm3), I, 1) = "-" Or Mid$(Trim(EmpNm3), I, 1) = "/" Or Mid$(Trim(EmpNm3), I, 1) = "_" Or Mid$(Trim(EmpNm3), I, 1) = "(" Or Mid$(Trim(EmpNm3), I, 1) = ")" Or Mid$(Trim(EmpNm3), I, 1) = "\" Or Mid$(Trim(EmpNm3), I, 1) = "[" Or Mid$(Trim(EmpNm3), I, 1) = "]" Or Mid$(Trim(EmpNm3), I, 1) = "{" Or Mid$(Trim(EmpNm3), I, 1) = "}" Then Exit For
                        Next I
                        If I = 0 Then I = 8
                        EmpNm4 = Microsoft.VisualBasic.Right(Trim(EmpNm3), Len(EmpNm3) - I)
                        EmpNm3 = Microsoft.VisualBasic.Left(Trim(EmpNm3), I - 1)
                    End If


                    If Trim(EmpNm4) <> "" Then
                        For k = 0 To 8
                            If Len(EmpNm4) > 8 Then

                                For I = 8 To 1 Step -1
                                    If Mid$(Trim(EmpNm4), I, 1) = " " Or Mid$(Trim(EmpNm4), I, 1) = "," Or Mid$(Trim(EmpNm4), I, 1) = "." Or Mid$(Trim(EmpNm4), I, 1) = "-" Or Mid$(Trim(EmpNm4), I, 1) = "/" Or Mid$(Trim(EmpNm4), I, 1) = "_" Or Mid$(Trim(EmpNm4), I, 1) = "(" Or Mid$(Trim(EmpNm4), I, 1) = ")" Or Mid$(Trim(EmpNm4), I, 1) = "\" Or Mid$(Trim(EmpNm4), I, 1) = "[" Or Mid$(Trim(EmpNm4), I, 1) = "]" Or Mid$(Trim(EmpNm4), I, 1) = "{" Or Mid$(Trim(EmpNm4), I, 1) = "}" Then Exit For
                                Next I
                                If I = 0 Then I = 8

                                'ClthNmStr1(k) = Microsoft.VisualBasic.Right(Trim(EMPNM4), Len(EMPNM4) - I)
                                'EMPNM4 = Microsoft.VisualBasic.Left(Trim(EMPNM4), I - 1)

                                EmpNmStr2(k) = Microsoft.VisualBasic.Left(Trim(EmpNm4), I)
                                EmpNm4 = Microsoft.VisualBasic.Right(Trim(EmpNm4), Len(EmpNm4) - I)

                                If Len(EmpNm4) > 8 Then
                                    EmpNm4 = EmpNm4

                                Else
                                    k = k + 1
                                    EmpNmStr2(k) = EmpNm4
                                    Exit For

                                End If


                            Else
                                ' k = k + 1
                                EmpNmStr2(k) = EmpNm4
                                Exit For

                            End If
                        Next
                    End If

                    '---------------------


                    Erase RemarksStr1
                    RemarksStr1 = New String(10) {}

                    Remark1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Remarks").ToString)
                    Remark2 = ""

                    If Len(Remark1) > 8 Then
                        For I = 8 To 1 Step -1
                            If Mid$(Trim(Remark1), I, 1) = " " Or Mid$(Trim(Remark1), I, 1) = "," Or Mid$(Trim(Remark1), I, 1) = "." Or Mid$(Trim(Remark1), I, 1) = "-" Or Mid$(Trim(Remark1), I, 1) = "/" Or Mid$(Trim(Remark1), I, 1) = "_" Or Mid$(Trim(Remark1), I, 1) = "(" Or Mid$(Trim(Remark1), I, 1) = ")" Or Mid$(Trim(Remark1), I, 1) = "\" Or Mid$(Trim(Remark1), I, 1) = "[" Or Mid$(Trim(Remark1), I, 1) = "]" Or Mid$(Trim(Remark1), I, 1) = "{" Or Mid$(Trim(Remark1), I, 1) = "}" Then Exit For
                        Next I
                        If I = 0 Then I = 8
                        Remark2 = Microsoft.VisualBasic.Right(Trim(Remark1), Len(Remark1) - I)
                        Remark1 = Microsoft.VisualBasic.Left(Trim(Remark1), I - 1)
                    End If


                    If Trim(Remark2) <> "" Then
                        For k = 0 To 8
                            If Len(Remark2) > 8 Then

                                For I = 8 To 1 Step -1
                                    If Mid$(Trim(Remark2), I, 1) = " " Or Mid$(Trim(Remark2), I, 1) = "," Or Mid$(Trim(Remark2), I, 1) = "." Or Mid$(Trim(Remark2), I, 1) = "-" Or Mid$(Trim(Remark2), I, 1) = "/" Or Mid$(Trim(Remark2), I, 1) = "_" Or Mid$(Trim(Remark2), I, 1) = "(" Or Mid$(Trim(Remark2), I, 1) = ")" Or Mid$(Trim(Remark2), I, 1) = "\" Or Mid$(Trim(Remark2), I, 1) = "[" Or Mid$(Trim(Remark2), I, 1) = "]" Or Mid$(Trim(Remark2), I, 1) = "{" Or Mid$(Trim(Remark2), I, 1) = "}" Then Exit For
                                Next I
                                If I = 0 Then I = 8

                                RemarksStr1(k) = Microsoft.VisualBasic.Left(Trim(Remark2), I)
                                Remark2 = Microsoft.VisualBasic.Right(Trim(Remark2), Len(Remark2) - I)

                                If Len(Remark2) > 8 Then
                                    Remark2 = Remark2

                                Else
                                    k = k + 1
                                    RemarksStr1(k) = Remark2
                                    Exit For

                                End If
                            Else
                                ' k = k + 1
                                RemarksStr1(k) = Remark2
                                Exit For

                            End If
                        Next
                    End If

                    '---------------------   

                    '---------------------


                    Erase PartyNameStr1
                    PartyNameStr1 = New String(10) {}

                    PartNm1 = Common_Procedures.Ledger_IdNoToName(con, prn_DetDt.Rows(prn_DetIndx).Item("Ledger_Idno").ToString)
                    PartNm2 = ""

                    If Len(PartNm1) > 9 Then
                        For I = 10 To 1 Step -1
                            If Mid$(Trim(PartNm1), I, 1) = " " Or Mid$(Trim(PartNm1), I, 1) = "," Or Mid$(Trim(PartNm1), I, 1) = "." Or Mid$(Trim(PartNm1), I, 1) = "-" Or Mid$(Trim(PartNm1), I, 1) = "/" Or Mid$(Trim(PartNm1), I, 1) = "_" Or Mid$(Trim(PartNm1), I, 1) = "(" Or Mid$(Trim(PartNm1), I, 1) = ")" Or Mid$(Trim(PartNm1), I, 1) = "\" Or Mid$(Trim(PartNm1), I, 1) = "[" Or Mid$(Trim(PartNm1), I, 1) = "]" Or Mid$(Trim(PartNm1), I, 1) = "{" Or Mid$(Trim(PartNm1), I, 1) = "}" Then Exit For
                        Next I
                        If I = 0 Then I = 7
                        PartNm2 = Microsoft.VisualBasic.Right(Trim(PartNm1), Len(PartNm1) - I)
                        PartNm1 = Microsoft.VisualBasic.Left(Trim(PartNm1), I - 1)
                    End If


                    If Trim(PartNm2) <> "" Then
                        For k = 0 To 8
                            If Len(PartNm2) > 8 Then

                                For I = 8 To 1 Step -1
                                    If Mid$(Trim(PartNm2), I, 1) = " " Or Mid$(Trim(PartNm2), I, 1) = "," Or Mid$(Trim(PartNm2), I, 1) = "." Or Mid$(Trim(PartNm2), I, 1) = "-" Or Mid$(Trim(PartNm2), I, 1) = "/" Or Mid$(Trim(PartNm2), I, 1) = "_" Or Mid$(Trim(PartNm2), I, 1) = "(" Or Mid$(Trim(PartNm2), I, 1) = ")" Or Mid$(Trim(PartNm2), I, 1) = "\" Or Mid$(Trim(PartNm2), I, 1) = "[" Or Mid$(Trim(PartNm2), I, 1) = "]" Or Mid$(Trim(PartNm2), I, 1) = "{" Or Mid$(Trim(PartNm2), I, 1) = "}" Then Exit For
                                Next I
                                If I = 0 Then I = 8

                                PartyNameStr1(k) = Microsoft.VisualBasic.Left(Trim(PartNm2), I)
                                PartNm2 = Microsoft.VisualBasic.Right(Trim(PartNm2), Len(PartNm2) - I)

                                If Len(PartNm2) > 8 Then
                                    PartNm2 = PartNm2

                                Else
                                    k = k + 1
                                    PartyNameStr1(k) = PartNm2
                                    Exit For

                                End If
                            Else
                                ' k = k + 1
                                PartyNameStr1(k) = PartNm2
                                Exit For

                            End If
                        Next
                    End If

                    '---------------------



                    pFont = New Font("Calibri", 7, FontStyle.Regular)
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Sl_No").ToString), LMargin + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Trim(ClthNm1), LMargin + ClAr(1) + ClAr(2), CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("RPM").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 3, CurY, 1, 0, pFont)

                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Shift_1_Pick_Efficiency").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 3, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Shift1_Mtrs").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 3, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Shift_1_Warp_Breakage").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 3, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Shift_1_weft_Breakage").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 3, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Trim(EmpNm1), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 0, 0, pFont)

                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Shift_2_Pick_Efficiency").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) - 3, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Shift2_Mtrs").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) - 3, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Shift_2_Warp_Breakage").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) - 3, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Shift_2_Warp_Breakage").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) - 3, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Trim(EmpNm3), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13), CurY, 0, 0, pFont)

                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Pick_Efficiency").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15) - 3, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Meters").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15) + ClAr(16) - 3, CurY, 1, 0, pFont)

                    If Trim(Remark1) <> "" Then
                        Common_Procedures.Print_To_PrintDocument(e, Trim(Remark1), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15) + ClAr(16), CurY, 0, 0, pFont)
                    End If

                    Common_Procedures.Print_To_PrintDocument(e, Trim(PartNm1), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15) + ClAr(16) + ClAr(17), CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Rate_Meter").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15) + ClAr(16) + ClAr(17) + ClAr(18) + ClAr(19) - 3, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Amount").ToString, PageWidth - 10, CurY, 1, 0, pFont)
                    'Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15) + ClAr(16) + ClAr(17) + ClAr(18) + ClAr(19), CurY, 2, 0, pFont)



                    NoofDets = NoofDets + 1


                    'If Trim(ItmNm2) <> "" Then
                    '    '     CurY = CurY + TxtHgt
                    '    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)

                    'End If
                    For I = 0 To 9
                        If Trim(itemNmStr1(I)) <> "" Or Trim(ClthNmStr1(I)) <> "" Or Trim(EmpNmStr1(I)) <> "" Or Trim(EmpNmStr2(I)) <> "" Or Trim(RemarksStr1(I)) <> "" Or Trim(PartyNameStr1(I)) <> "" Then
                            CurY = CurY + TxtHgt
                            NoofDets = NoofDets + 1
                            Common_Procedures.Print_To_PrintDocument(e, Trim(itemNmStr1(I)), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ClthNmStr1(I)), LMargin + ClAr(1) + ClAr(2), CurY, 0, PageWidth, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(EmpNmStr1(I)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(EmpNmStr2(I)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13), CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(RemarksStr1(I)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15) + ClAr(16), CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(PartyNameStr1(I)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15) + ClAr(16) + ClAr(17), CurY, 0, 0, pFont)

                        End If
                    Next

                    prn_DetIndx = prn_DetIndx + 1

                Loop

            End If

            Printing_Format2_1520_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True)

        End If

        'Catch ex As Exception
        '    MessageBox.Show(ex.Message, "DOES Not PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        'End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format2_1520_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
        Dim strHeight As Single
        Dim C1 As Single
        Dim C2 As Single
        Dim W1 As Single
        Dim S1 As Single

        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("Select a.*,eh.Employee_Name,Lh.Loom_Name,sh.Shift_Name from WeavingUnit_Production_Wages_Details a LEFT OUTER JOIN PayRoll_Employee_Head eh On a.Employee_Idno = eh.Employee_IdNo LEFT OUTER JOIN Loom_Head Lh On a.Loom_IdNo = Lh.Loom_IdNo LEFT OUTER JOIN Shift_Head sh On a.Shift_IdNo = sh.Shift_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " And a.WeavingUnit_Production_Wages_Code = '" & Trim(EntryCode) & "' Order by a.Sl_No", con)
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

        If Trim(Common_Procedures.settings.CustomerCode) <> "1234" Then '-----ARULJOTHI EXPORTS PVT LTD

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

        End If


        'CurY = CurY + TxtHgt - 5
        'p1Font = New Font("Calibri", 16, FontStyle.Bold)
        'Common_Procedures.Print_To_PrintDocument(e, "DELIVERY", LMargin, CurY, 2, PrintWidth, p1Font)
        'strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height
        p1Font = New Font("Calibri", 15, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "PRODUCTION REPORT", LMargin, CurY, 2, PageWidth, p1Font)


        CurY = CurY + strHeight
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY
        C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4)
        C2 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9)
        W1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14)

        Common_Procedures.Print_To_PrintDocument(e, "DATE :  " & Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("WeavingUnit_Production_Wages_FromDate").ToString), "dd-MM-yyyy").ToString, LMargin + ClAr(1), CurY, 2, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "SHIFT A", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "SHIFT B", LMargin + C2 + 50, CurY, 2, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + W1 + ClAr(15) + ClAr(16), CurY, 2, 0, pFont)

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY


        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))
        e.Graphics.DrawLine(Pens.Black, LMargin + C2, CurY, LMargin + C2, LnAr(2))
        e.Graphics.DrawLine(Pens.Black, LMargin + W1, CurY, LMargin + W1, LnAr(2))

        pFont = New Font("Calibri", 8, FontStyle.Bold)
        CurY = CurY + TxtHgt - 20
        Common_Procedures.Print_To_PrintDocument(e, "SL", LMargin, CurY, 2, ClAr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin, CurY + 15, 2, ClAr(1), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "LOOM", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin + ClAr(1), CurY + 15, 2, ClAr(2), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "CLOTH", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "NAME", LMargin + ClAr(1) + ClAr(2), CurY + 15, 2, ClAr(3), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "RPM", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "PICK", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "EFF", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY + 15, 2, ClAr(5), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "MTR", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "WA", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "-RP", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY + 15, 2, ClAr(7), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "WE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "FT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY + 15, 2, ClAr(8), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "EMP", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "NAME", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY + 15, 2, ClAr(9), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "PICK", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, 2, ClAr(10), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "EFF", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY + 15, 2, ClAr(10), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "MTR", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, 2, ClAr(11), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "WA", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), CurY, 2, ClAr(12), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "-RP", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), CurY + 15, 2, ClAr(12), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "WE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), CurY, 2, ClAr(13), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "-FT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), CurY + 15, 2, ClAr(13), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "EMP", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13), CurY, 2, ClAr(14), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "NAME", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13), CurY + 15, 2, ClAr(14), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "TOT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14), CurY, 2, ClAr(15), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "EFF", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14), CurY + 15, 2, ClAr(15), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "TOT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15), CurY, 2, ClAr(16), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "MTR", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15), CurY + 15, 2, ClAr(16), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "REMA", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15) + ClAr(16), CurY, 2, ClAr(17), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "-RKS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15) + ClAr(16), CurY + 15, 2, ClAr(17), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "PARTY", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15) + ClAr(16) + ClAr(17), CurY, 2, ClAr(18), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "NAME", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15) + ClAr(16) + ClAr(17), CurY + 15, 2, ClAr(18), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "RATE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15) + ClAr(16) + ClAr(17) + ClAr(18), CurY, 2, ClAr(19), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15) + ClAr(16) + ClAr(17) + ClAr(18) + ClAr(19), CurY, 2, ClAr(20), pFont)

        CurY = CurY + TxtHgt + 15
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY


    End Sub

    Private Sub Printing_Format2_1520_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim i As Integer
        Dim Cmp_Name As String
        Dim p1Font As Font


        For i = NoofDets + 1 To NoofItems_PerPage
            CurY = CurY + TxtHgt
        Next

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(5) = CurY
        pFont = New Font("Calibri", 8, FontStyle.Regular)
        CurY = CurY + TxtHgt - 10
        If is_LastPage = True Then
            Common_Procedures.Print_To_PrintDocument(e, " TOTAL", LMargin + ClAr(1) + ClAr(2) + 20, CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Avg_RPM").ToString), "########0"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Avg_Shift_1_Pick_Efficiency").ToString), "#########0"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Shift1_Mtrs").ToString), "########0"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Avg_Shift_2_Pick_Efficiency").ToString), "#########0"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Shift2_Mtrs").ToString), "########0"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_PickEfficiency").ToString), "########0"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15), CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString), "#########0"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15) + ClAr(16), CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_aMOUNT").ToString), "#########0.00"), PageWidth - 5, CurY, 1, 0, pFont)
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
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), LnAr(3))

        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15) + ClAr(16), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15) + ClAr(16), LnAr(3))

        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15) + ClAr(16) + ClAr(17), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15) + ClAr(16) + ClAr(17), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15) + ClAr(16) + ClAr(17) + ClAr(18), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15) + ClAr(16) + ClAr(17) + ClAr(18), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15) + ClAr(16) + ClAr(17) + ClAr(18) + ClAr(19), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15) + ClAr(16) + ClAr(17) + ClAr(18) + ClAr(19), LnAr(3))



        If is_LastPage = True Then

            If Val(prn_HdDt.Rows(0).Item("EB_Units_Consumed").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("EB_Amount").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("Employee_Salary").ToString) <> 0 Then
                Dim vStringWidth As Single = 0

                CurY = CurY + TxtHgt - 15

                Common_Procedures.Print_To_PrintDocument(e, "EB Units :  " & Format(Val(prn_HdDt.Rows(0).Item("EB_Units_Consumed").ToString), "##########0.00"), LMargin + 10, CurY, 0, 0, pFont)

                Common_Procedures.Print_To_PrintDocument(e, "EB Amount :  " & Format(Val(prn_HdDt.Rows(0).Item("EB_Amount").ToString), "##########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 10, CurY, 0, 0, pFont)

                Common_Procedures.Print_To_PrintDocument(e, "Employee Salary :  " & Format(Val(prn_HdDt.Rows(0).Item("Employee_Salary").ToString), "##########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + 10, CurY, 0, 0, pFont)

                CurY = CurY + TxtHgt
                e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            End If
        End If



        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        p1Font = New Font("Calibri", 12, FontStyle.Bold)

        If is_LastPage = True Then
            CurY = CurY + TxtHgt - 5
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt + 10
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt


            Common_Procedures.Print_To_PrintDocument(e, "Prepared by", LMargin + ClAr(1) + ClAr(2) + 20, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Checked by", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 1, 0, pFont)
            'End If
            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(6))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(6))
        End If
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

    End Sub

    Private Sub cbo_Weaver_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Weaver.GotFocus
        cbo_Weaver.Tag = cbo_Weaver.Text
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( (Ledger_Type = 'WEAVER' and Own_Loom_Status = 1) or Ledger_Type = 'GODOWN' or Ledger_Type = 'JOBWORKER' or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Weaver_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Weaver.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Weaver, msk_FromDate, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( (Ledger_Type = 'WEAVER' and Own_Loom_Status = 1) or Ledger_Type = 'GODOWN' or Ledger_Type = 'JOBWORKER' or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")

        If (e.KeyValue = 40 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(DgvCol_Details.SOUND_WAGES)
            Else
                If MessageBox.Show("Do you want to Save?", "FOR CLOSING ENTRY...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes Then
                    save_record()

                End If
            End If

        End If


    End Sub

    Private Sub cbo_Weaver_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Weaver.KeyPress
        Dim q As Single = 0
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim LedID As Integer = 0, NoofLm As Integer = 0
        Dim MxId As Long = 0

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Weaver, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( (Ledger_Type = 'WEAVER' and Own_Loom_Status = 1) or Ledger_Type = 'GODOWN' or Ledger_Type = 'JOBWORKER' or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")

        If Asc(e.KeyChar) = 13 Then

            'If Trim(UCase(cbo_Weaver.Text)) <> "" Then

            If Trim(UCase(cbo_Weaver.Tag)) <> Trim(UCase(cbo_Weaver.Text)) Or Trim(UCase(msk_FromDate.Tag)) <> Trim(UCase(msk_FromDate.Text)) Or Trim(UCase(msk_ToDate.Tag)) <> Trim(UCase(msk_ToDate.Text)) Or (cbo_Weaver.Text = "" And dgv_Details.Rows.Count = 0) Then
                'If Trim(UCase(cbo_Weaver.Tag)) <> Trim(UCase(cbo_Weaver.Text)) Or Trim(UCase(msk_FromDate.Tag)) <> Trim(UCase(msk_FromDate.Text)) Or Trim(UCase(msk_ToDate.Tag)) <> Trim(UCase(msk_ToDate.Text)) Then
                Get_Production_Details()
            End If

            'End If

            If dgv_Details.Rows.Count > 0 Then
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(DgvCol_Details.SOUND_WAGES)

                Else
                    If MessageBox.Show("Do you want to Save?", "FOR CLOSING ENTRY...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes Then
                        save_record()
                    End If

                End If

            End If

    End Sub

    Private Sub cbo_Weaver_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Weaver.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
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
    Private Sub msk_ToDate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_ToDate.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        vmskOldText = ""
        vmskSelStrt = -1
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            vmskOldText = msk_ToDate.Text
            vmskSelStrt = msk_ToDate.SelectionStart
        End If

        If (e.KeyValue = 40) Then
            cbo_Weaver.Focus()
        End If
        If (e.KeyValue = 38) Then
            msk_FromDate.Focus()
        End If

    End Sub

    Private Sub msk_ToDate_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles msk_ToDate.KeyPress
        Dim vTotMtr As String

        If Trim(UCase(e.KeyChar)) = "D" Then
            msk_ToDate.Text = Date.Today
            msk_ToDate.SelectionStart = 0
        End If

        Try

            If Asc(e.KeyChar) = 13 Then
                cbo_Weaver.Focus()
            End If

        Catch ex As Exception
            '----

        End Try

        'If Asc(e.KeyChar) = 13 Then
        '    dgv_Details.Focus()
        '    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
        'End If
    End Sub


    Private Sub msk_ToDate_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_ToDate.KeyUp
        Dim vmRetTxt As String = ""
        Dim vmRetSelStrt As Integer = -1

        'If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
        '    msk_ToDate.Text = Date.Today
        'End If
        If e.KeyCode = 107 Then
            msk_ToDate.Text = DateAdd("D", 1, Convert.ToDateTime(msk_ToDate.Text))
        ElseIf e.KeyCode = 109 Then
            msk_ToDate.Text = DateAdd("D", -1, Convert.ToDateTime(msk_ToDate.Text))
        End If

        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            Common_Procedures.maskEdit_Date_ON_DelBackSpace(sender, e, vmskOldText, vmskSelStrt)
        End If

    End Sub

    Private Sub dtp_ToDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Dtp_ToDate.TextChanged

        If IsDate(Dtp_ToDate.Text) = True Then
            msk_ToDate.Text = Dtp_ToDate.Text
            msk_ToDate.SelectionStart = 0
        End If
    End Sub

    Private Sub msk_ToDate_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles msk_ToDate.LostFocus

        If IsDate(msk_ToDate.Text) = True Then
            If Microsoft.VisualBasic.DateAndTime.Day(Convert.ToDateTime(msk_ToDate.Text)) <= 31 Or Microsoft.VisualBasic.DateAndTime.Month(Convert.ToDateTime(msk_ToDate.Text)) <= 31 Then
                If Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_ToDate.Text)) <= 2050 And Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_ToDate.Text)) >= 2000 Then
                    Dtp_ToDate.Value = Convert.ToDateTime(msk_ToDate.Text)
                End If
            End If

        End If
    End Sub

    Private Sub dtp_ToDate_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Dtp_ToDate.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dtp_ToDate.Text = Date.Today
        End If
    End Sub
    Private Sub Get_Production_Details()
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim tr As SqlClient.SqlTransaction
        Dim Led_ID = 0
        Dim n = 0
        Dim Nr = 0
        Dim Sl_No = 0
        Dim vSound_Amt = ""
        Dim vSeconds_Amt = ""
        Dim vBits_Amt = ""
        Dim vTotal_Amount = ""
        Dim CompIDCondt = ""
        Dim vSQLCondt = ""


        If msk_FromDate.Visible = True And IsDate(msk_FromDate.Text) = False Then
            MessageBox.Show("Invalid From Date", "DOES NOT SHOW REPORT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If dtp_FromDate.Enabled Then dtp_FromDate.Focus()
            Exit Sub
        End If
        If msk_ToDate.Visible = True And IsDate(msk_ToDate.Text) = False Then
            MessageBox.Show("Invalid To Date", "DOES NOT SHOW REPORT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If Dtp_ToDate.Enabled Then Dtp_ToDate.Focus()
            Exit Sub
        End If

        cmd.Connection = con

        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@fromdate", dtp_FromDate.Value.Date)
        cmd.Parameters.AddWithValue("@todate", Dtp_ToDate.Value.Date)


        Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Weaver.Text)

        'If Led_ID = 0 Then
        '    MessageBox.Show("Invalid Ledger Name", "DOES NOT SHOW REPORT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    If cbo_Weaver.Enabled Then cbo_Weaver.Focus()
        '    Exit Sub
        'End If

        CompIDCondt = " (tZ.company_idno = " & Str(Val(lbl_Company.Tag)) & ")"
        'If Common_Procedures.settings.EntrySelection_Combine_AllCompany = 1 Then
        '    CompIDCondt = ""
        '    If Trim(UCase(Common_Procedures.User.Type)) = "ACCOUNT" Then
        '        CompIDCondt = "(Company_Type <> 'UNACCOUNT')"
        '    End If
        'End If

        If Trim(Common_Procedures.settings.CustomerCode) = "1608" And Val(lbl_Company.Tag) = 2 Then
            CompIDCondt = ""
        End If

        vSQLCondt = ""

        If Val(Led_ID) <> 0 Then
            vSQLCondt = " d.Ledger_Idno = " & Val(Led_ID)
        End If

        If Trim(CompIDCondt) <> "" Then
            vSQLCondt = Trim(vSQLCondt) & IIf(Trim(vSQLCondt) <> "", " and ", "") & Trim(CompIDCondt)
        End If


        cmd.CommandText = "Truncate table " & Trim(Common_Procedures.ReportTempSubTable) & ""
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Truncate table " & Trim(Common_Procedures.ReportTempTable) & ""
        cmd.ExecuteNonQuery()


        Nr = 0
        cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempSubTable) & " (             Name10      ,          Name1              ,          Int1 ,             Name2         ,   meters1    ,        Date1                ,        Name3 ,     Name4   ,     Name5      ,     Name6  ,              Name7                             ,   Currency2   ,   Currency3   ,   Currency4   ,   Currency5   ,   Currency6   ,                Meters4                                                               ,   Currency1      ,   Currency7      ,Currency8              ,Weight5      , Meters7      ,       Name8       ,                       Name9      ,           Weight6   ,     Meters8  ) " &
                                           " Select                                       h.Weaver_ClothReceipt_Code, a.Weaver_Piece_Checking_Code, a.Company_IdNo, a.Weaver_Piece_Checking_No, a.for_OrderBy, a.Weaver_Piece_Checking_Date, d.Ledger_Name, c.Cloth_Name, g.LoomType_Name, f.Loom_Name, (h.Weaver_ClothReceipt_Code + '~' + b.Piece_No), b.Type1_Meters, b.Type2_Meters, b.Type3_Meters, b.Type4_Meters, b.Type5_Meters, (b.Type1_Meters + b.Type2_Meters + b.Type3_Meters + b.Type4_Meters + b.Type5_Meters) , c.Wages_For_Type1 , c.Wages_For_Type2 , c.Wages_For_Type3 , h.Consumed_Yarn   ,a.folding , Ec.EndsCount_Name , Wfc.Count_name as WeftCount_Name , c.Weight_Meter_Weft , h.Consumed_Pavu  " &
                                            " from Weaver_Piece_Checking_Head a INNER JOIN Weaver_ClothReceipt_Piece_Details b ON a.Company_IdNo = b.Company_IdNo and a.Weaver_Piece_Checking_Code = b.Weaver_Piece_Checking_Code and (a.Receipt_PkCondition LIKE 'WCLRC-%' or a.Receipt_PkCondition LIKE 'PCDOF-%' or a.Receipt_PkCondition LIKE 'INCHK-%') and (a.Receipt_PkCondition + a.Piece_Receipt_Code = b.Weaver_ClothReceipt_Code or a.Piece_Receipt_Code = b.Weaver_ClothReceipt_Code) INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo INNER JOIN Cloth_Head c ON a.Cloth_IdNo <> 0 and a.Cloth_IdNo = c.Cloth_IdNo INNER JOIN Ledger_Head d ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = d.Ledger_IdNo " &
                                            " LEFT OUTER JOIN Loom_Head f ON b.Loom_IdNo <> 0 and b.Loom_IdNo = f.Loom_IdNo LEFT OUTER JOIN LoomType_Head g ON f.LoomType_IdNo <> 0 and f.LoomType_IdNo = g.LoomType_IdNo INNER JOIN Weaver_Cloth_Receipt_Head h ON a.Weaver_Piece_Checking_Code = h.Weaver_Piece_Checking_Code  " &
                                            " INNER JOIN EndsCount_Head Ec on h.EndsCount_IdNo=Ec.EndsCount_IdNo INNER JOIN Count_Head Wfc on C.Cloth_WeftCount_IdNo = Wfc.Count_IdNo  Where " & vSQLCondt & IIf(vSQLCondt <> "", " and ", "") & " a.Weaver_Piece_Checking_Date between @fromdate and @todate Order by a.Weaver_Piece_Checking_Date, a.for_OrderBy, a.Weaver_Piece_Checking_No, a.Company_IdNo"
        Nr = cmd.ExecuteNonQuery()

        Nr = 0
        cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & "(Name4  ,  Name8  ,     Currency2  ,    Currency3   ,    Currency4  ,  Currency1,Currency7,Currency8 ,   Meters8   , Name9 ,weight6,weight5 )" &
                                                                              " Select  Name4 ,  Name8  ,Sum(Currency2)  ,Sum(Currency3)  ,Sum(Currency4) ,  Currency1,Currency7,Currency8 ,Sum(Meters8) , name9 , weight6 , sum(Weight5) From " & Trim(Common_Procedures.ReportTempSubTable) & " " &
                                                                              " Group by Name4 ,  Name8  ,Currency1,Currency7,Currency8 ,name9 , weight6 "
        Nr = cmd.ExecuteNonQuery()


        Da = New SqlClient.SqlDataAdapter("Select * from " & Trim(Common_Procedures.ReportTempTable) & " Order by Name4", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)

        dgv_Details.Rows.Clear()

        Sl_No = 0

        If Dt1.Rows.Count > 0 Then

            With dgv_Details



                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add

                    Sl_No = Sl_No + 1

                    dgv_Details.Rows(n).Cells(DgvCol_Details.SL_NO).Value = Val(Sl_No)
                    dgv_Details.Rows(n).Cells(DgvCol_Details.CLOTH_NAME).Value = Trim(Dt1.Rows(i).Item("Name4").ToString)
                    dgv_Details.Rows(n).Cells(DgvCol_Details.ENDSCOUNT_NAME).Value = Trim(Dt1.Rows(i).Item("Name8").ToString)
                    dgv_Details.Rows(n).Cells(DgvCol_Details.SOUND_METERS).Value = Format(Val(Dt1.Rows(i).Item("Currency2").ToString), "########0.00")
                    dgv_Details.Rows(n).Cells(DgvCol_Details.SECOND_METERS).Value = Format(Val(Dt1.Rows(i).Item("Currency3").ToString), "########0.00")
                    dgv_Details.Rows(n).Cells(DgvCol_Details.BITS_METERS).Value = Format(Val(Dt1.Rows(i).Item("Currency4").ToString), "########0.00")

                    dgv_Details.Rows(n).Cells(DgvCol_Details.TOTAL_METERS).Value = Val(Dt1.Rows(i).Item("Currency2").ToString + Dt1.Rows(i).Item("Currency3").ToString + Dt1.Rows(i).Item("Currency4").ToString)

                    dgv_Details.Rows(n).Cells(DgvCol_Details.SOUND_WAGES).Value = Val(Dt1.Rows(i).Item("Currency1").ToString)
                    dgv_Details.Rows(n).Cells(DgvCol_Details.SECONDS_WAGES).Value = Val(Dt1.Rows(i).Item("Currency7").ToString)
                    dgv_Details.Rows(n).Cells(DgvCol_Details.BITS_WAGES).Value = Val(Dt1.Rows(i).Item("Currency8").ToString)


                    vSound_Amt = 0
                    vSeconds_Amt = 0
                    vBits_Amt = 0

                    If Val(dgv_Details.Rows(n).Cells(DgvCol_Details.SOUND_METERS).Value) <> 0 And Val(dgv_Details.Rows(n).Cells(DgvCol_Details.SOUND_WAGES).Value) <> 0 Then
                        vSound_Amt = (Val(dgv_Details.Rows(n).Cells(DgvCol_Details.SOUND_METERS).Value) * Val(dgv_Details.Rows(n).Cells(DgvCol_Details.SOUND_WAGES).Value))
                    End If
                    If Val(dgv_Details.Rows(n).Cells(DgvCol_Details.SECOND_METERS).Value) <> 0 And Val(dgv_Details.Rows(n).Cells(DgvCol_Details.SECONDS_WAGES).Value) <> 0 Then
                        vSeconds_Amt = (Val(dgv_Details.Rows(n).Cells(DgvCol_Details.SECOND_METERS).Value) * Val(dgv_Details.Rows(n).Cells(DgvCol_Details.SECONDS_WAGES).Value))
                    End If
                    If Val(dgv_Details.Rows(n).Cells(DgvCol_Details.BITS_METERS).Value) <> 0 And Val(dgv_Details.Rows(n).Cells(DgvCol_Details.BITS_WAGES).Value) <> 0 Then
                        vBits_Amt = (Val(dgv_Details.Rows(n).Cells(DgvCol_Details.BITS_METERS).Value) * Val(dgv_Details.Rows(n).Cells(DgvCol_Details.BITS_WAGES).Value))
                    End If

                    dgv_Details.Rows(n).Cells(DgvCol_Details.TOTAL_AMOUNT).Value = Format(Val(vSound_Amt) + Val(vSeconds_Amt) + Val(vBits_Amt), "########0.00")

                    dgv_Details.Rows(n).Cells(DgvCol_Details.WARP_METERS).Value = Format(Val(Dt1.Rows(i).Item("Meters8").ToString), "########0.00")
                    dgv_Details.Rows(n).Cells(DgvCol_Details.WEFT_COUNT_NAME).Value = Trim(Dt1.Rows(i).Item("Name9").ToString)
                    dgv_Details.Rows(n).Cells(DgvCol_Details.WEIGHT_CONSUMP_METER).Value = Format(Val(Dt1.Rows(i).Item("Weight6").ToString), "########0.000")
                    dgv_Details.Rows(n).Cells(DgvCol_Details.CONSUMED_WEIGHT).Value = Format(Val(Dt1.Rows(i).Item("Weight5").ToString), "########0.000")


                Next i

            End With

        End If

        Total_Calculation()

    End Sub

    Private Sub msk_FromDate_Enter(sender As Object, e As EventArgs) Handles msk_FromDate.Enter
        msk_FromDate.Tag = msk_FromDate.Text
    End Sub

    Private Sub msk_ToDate_Enter(sender As Object, e As EventArgs) Handles msk_ToDate.Enter
        msk_ToDate.Tag = msk_ToDate.Text
    End Sub

End Class