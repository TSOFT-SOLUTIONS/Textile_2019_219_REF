Public Class Processed_Fabric_OpeningStock
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "OPENI-"
    Private OpYrCode As String = ""
    Private NoCalc_Status As Boolean = False
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Private vCbo_ItmNm As String

    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl

    Private Sub clear()

        NoCalc_Status = True

        New_Entry = False
        Insert_Entry = False

        pnl_Back.Enabled = True

        lbl_RollNo.Text = ""
        lbl_RollNo.ForeColor = Color.Black

        cbo_FabricName.Text = ""
        cbo_Colour.Text = ""
        cbo_Processing.Text = ""


        dgv_Details.Rows.Clear()
        dgv_Details_Total.Rows.Clear()
        dgv_Details_Total.Rows.Add()

        dgv_Details.Rows.Clear()

        Grid_Cell_DeSelect()
        NoCalc_Status = False

    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim txtbx As TextBox
        Dim combobx As ComboBox
        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Then
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

        If Me.ActiveControl.Name <> dgv_Details.Name Then
            Grid_DeSelect()
        End If

        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)

        On Error Resume Next

        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Then
                Prec_ActCtrl.BackColor = Color.White
                Prec_ActCtrl.ForeColor = Color.Black
            End If
        End If

    End Sub

    Private Sub ControlLostFocus1(ByVal sender As Object, ByVal e As System.EventArgs)

        On Error Resume Next

        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Then
                Prec_ActCtrl.BackColor = Color.LightBlue
                Prec_ActCtrl.ForeColor = Color.Blue
            End If
        End If

    End Sub

    Private Sub Grid_DeSelect()
        On Error Resume Next
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
        If Not IsNothing(dgv_Details_Total.CurrentCell) Then dgv_Details_Total.CurrentCell.Selected = False
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

    End Sub

    Private Sub Processed_Fabric_OpeningStock_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Try
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_FabricName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CLOTH" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_FabricName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Colour.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "COLOUR" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Colour.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Processing.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "PROCESS" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Processing.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

    Private Sub Processed_Fabric_OpeningStock_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable

        Me.Text = ""

        con.Open()

        da = New SqlClient.SqlDataAdapter("select Cloth_Name from Cloth_Head Where Cloth_Type <> 'GREY' order by Cloth_Name", con)
        da.Fill(dt1)
        cbo_FabricName.DataSource = dt1
        cbo_FabricName.DisplayMember = "Cloth_Name"

        da = New SqlClient.SqlDataAdapter("select Colour_Name from Colour_Head order by Colour_Name", con)
        da.Fill(dt1)
        cbo_Colour.DataSource = dt1
        cbo_Colour.DisplayMember = "Colour_Name"

        da = New SqlClient.SqlDataAdapter("select Process_Name from Process_Head order by Process_Name", con)
        da.Fill(dt1)
        cbo_Processing.DataSource = dt1
        cbo_Processing.DisplayMember = "Process_Name"


        AddHandler cbo_FabricName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Colour.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Processing.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_FabricName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Colour.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Processing.LostFocus, AddressOf ControlLostFocus

        OpYrCode = Microsoft.VisualBasic.Left(Trim(Common_Procedures.FnRange), 4)
        OpYrCode = Trim(Mid(Val(OpYrCode) - 1, 3, 2)) & "-" & Trim(Microsoft.VisualBasic.Right(OpYrCode, 2))

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True
        new_record()
    End Sub

    Private Sub Processed_Fabric_OpeningStock_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        Try
            If Asc(e.KeyChar) = 27 Then

                If MessageBox.Show("Do you want to Close?", "FOR CLOSING ENTRY...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) <> Windows.Forms.DialogResult.Yes Then
                    Exit Sub

                Else
                    Close_Form()

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub Processed_Fabric_OpeningStock_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
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
                        If .CurrentCell.ColumnIndex >= 3 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                                    save_record()
                                Else
                                    cbo_FabricName.Focus()
                                End If

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                            End If

                        Else
                            If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then
                                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                                    save_record()
                                Else
                                    cbo_FabricName.Focus()
                                End If

                            Else
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                            End If


                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then

                        If .CurrentCell.ColumnIndex <= 1 Then
                            If .CurrentCell.RowIndex = 0 Then
                                If cbo_Processing.Enabled Then
                                    cbo_Processing.Focus()
                                ElseIf cbo_FabricName.Enabled Then
                                    cbo_FabricName.Focus()
                                End If

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(8)

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

        NoCalc_Status = True

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(OpYrCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select *  from Processed_Fabric_Opening_Head  Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Processed_Fabric_Opening_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_RollNo.Text = dt1.Rows(0).Item("Processed_Fabric_Opening_No").ToString
                cbo_FabricName.Text = Common_Procedures.Cloth_IdNoToName(con, Val(dt1.Rows(0).Item("Cloth_IdNo").ToString))
                cbo_Colour.Text = Common_Procedures.Colour_IdNoToName(con, Val(dt1.Rows(0).Item("Colour_IdNo").ToString))
                cbo_Processing.Text = Common_Procedures.Process_IdNoToName(con, Val(dt1.Rows(0).Item("Process_IdNo").ToString))

                da2 = New SqlClient.SqlDataAdapter("Select a.* from Processed_Fabric_Inspection_Details a Where a.Processed_Fabric_Inspection_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' Order by Sl_No, Pcs_No", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                With dgv_Details

                    .Rows.Clear()
                    SNo = 0

                    If dt2.Rows.Count > 0 Then

                        For i = 0 To dt2.Rows.Count - 1

                            n = .Rows.Add()
                            SNo = SNo + 1

                            .Rows(n).Cells(0).Value = Val(SNo)
                            .Rows(n).Cells(1).Value = dt2.Rows(i).Item("Pcs_No").ToString

                            .Rows(n).Cells(2).Value = Format(Val(dt2.Rows(i).Item("Meters").ToString), "########0.00")

                            .Rows(n).Cells(3).Value = Format(Val(dt2.Rows(i).Item("Weight").ToString), "########0.000")

                            .Rows(n).Cells(4).Value = dt2.Rows(i).Item("Sales_Invoice_Code").ToString

                            If dt2.Rows(i).Item("Sales_Invoice_Code").ToString <> "" Then
                                .Rows(n).Cells(1).Style.ForeColor = Color.Red
                                .Rows(n).Cells(2).Style.ForeColor = Color.Red
                                .Rows(n).Cells(3).Style.ForeColor = Color.Red
                                LockSTS = True
                            End If

                        Next i

                    End If

                    If .RowCount = 0 Then .Rows.Add()

                End With

                With dgv_Details_Total
                    If .RowCount = 0 Then .Rows.Add()
                    .Rows(0).Cells(1).Value = Val(dt1.Rows(0).Item("Total_Rolls").ToString)
                    .Rows(0).Cells(2).Value = Format(Val(dt1.Rows(0).Item("Total_Meters").ToString), "########0.00")
                    .Rows(0).Cells(3).Value = Format(Val(dt1.Rows(0).Item("Total_Weight").ToString), "########0.000")
                End With

                dt1.Clear()

                If LockSTS = True Then

                    cbo_FabricName.Enabled = False
                    cbo_FabricName.BackColor = Color.LightGray

                    cbo_Colour.Enabled = False
                    cbo_Colour.BackColor = Color.LightGray

                    cbo_Processing.Enabled = False
                    cbo_Processing.BackColor = Color.LightGray
                End If

            End If

            Grid_Cell_DeSelect()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            dt1.Dispose()
            da1.Dispose()

            dt2.Dispose()
            da2.Dispose()

            If cbo_FabricName.Visible And cbo_FabricName.Enabled Then cbo_FabricName.Focus()

        End Try

        NoCalc_Status = False

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Piece_OpeningStock, "~L~") = 0 And InStr(Common_Procedures.UR.Piece_OpeningStock, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

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

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RollNo.Text) & "/" & Trim(OpYrCode)

        Da = New SqlClient.SqlDataAdapter("select COUNT(*) from Processed_Fabric_Inspection_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Processed_Fabric_Inspection_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Roll_Code = '" & Trim(NewCode) & "' and Sales_Invoice_Code <> ''", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                If Val(Dt1.Rows(0)(0).ToString) <> 0 Then
                    MessageBox.Show("Already Delivered/Invoiced", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If
        Dt1.Clear()

        trans = con.BeginTransaction

        Try

            cmd.Connection = con
            cmd.Transaction = trans

            cmd.CommandText = "Delete from Stock_Cloth_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Processed_Fabric_Inspection_Details Where Processed_Fabric_Inspection_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Processed_Fabric_Opening_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Processed_Fabric_Opening_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            trans.Commit()

            new_record()

            MessageBox.Show("Deleted Successfully!!!", "FOR DELETION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception
            trans.Rollback()
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally

            Dt1.Dispose()
            Da.Dispose()
            trans.Dispose()
            cmd.Dispose()

            ' If dtp_Date.Enabled = True And dtp_Date.Visible = True Then dtp_Date.Focus()

        End Try

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        'If Filter_Status = False Then

        '    Dim da As New SqlClient.SqlDataAdapter
        '    Dim dt1 As New DataTable
        '    Dim dt2 As New DataTable

        '    da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead order by Ledger_DisplayName", con)
        '    da.Fill(dt1)
        '    cbo_Filter_PartyName.DataSource = dt1
        '    cbo_Filter_PartyName.DisplayMember = "Ledger_DisplayName"


        '    da = New SqlClient.SqlDataAdapter("select Cloth_Name from Cloth_Head order by Cloth_Name", con)
        '    da.Fill(dt2)
        '    cbo_Filter_PartyName.DataSource = dt2
        '    cbo_Filter_PartyName.DisplayMember = "Cloth_Name"

        '    dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
        '    dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
        '    cbo_Filter_PartyName.Text = ""
        '    cbo_Filter_PartyName.SelectedIndex = -1
        '    cbo_Filter_ClothName.Text = ""
        '    cbo_Filter_ClothName.SelectedIndex = -1
        '    dgv_Filter_Details.Rows.Clear()

        'End If

        'pnl_Filter.Visible = True
        'pnl_Filter.Enabled = True
        'pnl_Back.Enabled = False
        'If dtp_Filter_Fromdate.Enabled And dtp_Filter_Fromdate.Visible Then dtp_Filter_Fromdate.Focus()

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String

        Try

            da = New SqlClient.SqlDataAdapter("select top 1 Processed_Fabric_Opening_No from Processed_Fabric_Opening_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Processed_Fabric_Opening_Code like '%/" & Trim(OpYrCode) & "' Order by for_Orderby, Processed_Fabric_Opening_No", con)
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
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RollNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Processed_Fabric_Opening_No from Processed_Fabric_Opening_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Processed_Fabric_Opening_Code like '%/" & Trim(OpYrCode) & "' Order by for_Orderby, Processed_Fabric_Opening_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RollNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Processed_Fabric_Opening_No from Processed_Fabric_Opening_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Processed_Fabric_Opening_Code like '%/" & Trim(OpYrCode) & "' Order by for_Orderby desc, Processed_Fabric_Opening_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 Processed_Fabric_Opening_No from Processed_Fabric_Opening_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Processed_Fabric_Opening_Code like '%/" & Trim(OpYrCode) & "' Order by for_Orderby desc, Processed_Fabric_Opening_No desc", con)
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

            lbl_RollNo.Text = Common_Procedures.get_MaxCode(con, "Processed_Fabric_Opening_Head", "Processed_Fabric_Opening_Code", "for_OrderBy", "", Val(lbl_Company.Tag), OpYrCode)
            lbl_RollNo.ForeColor = Color.Red

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            Dt1.Dispose()
            Da1.Dispose()

            If cbo_FabricName.Enabled And cbo_FabricName.Visible Then cbo_FabricName.Focus()

        End Try

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim InvCode As String

        Try

            inpno = InputBox("Enter Roll No.", "FOR FINDING...")

            InvCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(OpYrCode)

            Da = New SqlClient.SqlDataAdapter("select Processed_Fabric_Opening_No from Processed_Fabric_Opening_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Processed_Fabric_Opening_Code = '" & Trim(InvCode) & "'", con)
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
                MessageBox.Show("Roll No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Pavu_Delivery_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Pavu_Delivery_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        Try

            inpno = InputBox("Enter New Roll No.", "FOR NEW ROLL NO. FOR INSERTION...")

            InvCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(OpYrCode)

            Da = New SqlClient.SqlDataAdapter("select Processed_Fabric_Opening_No from Processed_Fabric_Opening_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Processed_Fabric_Opening_Code = '" & Trim(InvCode) & "'", con)
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
                    MessageBox.Show("Invalid Roll No.", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_RollNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT ...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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
        Dim OpDate As Date
        Dim Lm_ID As Integer = 0
        Dim Wdth_Typ As String = ""
        Dim Fab_id As Integer = 0
        Dim Prss_ID As Integer = 0
        Dim Clr_id As Integer = 0

        Dim Sno As Integer = 0
        Dim EntID As String = ""
        Dim PBlNo As String = ""
        Dim Partcls As String = ""

        Dim vTot_Pcs As Integer = 0
        Dim vTot_Mtrs As Double = 0
        Dim vTot_Wgt As Double = 0
        Dim stkof_idno As Integer = 0
        Dim Led_type As String = 0

        Dim Nr As Integer = 0

        Dim WagesCode As String = ""

        Dim ConsYarn As Single = 0
        Dim ConsPavu As Single = 0


        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        ' If Common_Procedures.UserRight_Check(Common_Procedures.UR.Piece_OpeningStock, New_Entry) = False Then Exit Sub

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        Fab_id = Common_Procedures.Cloth_NameToIdNo(con, cbo_FabricName.Text)
        If Fab_id = 0 Then
            MessageBox.Show("Invalid Fabric Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_FabricName.Enabled Then cbo_FabricName.Focus()
            Exit Sub
        End If

        Clr_id = Common_Procedures.Colour_NameToIdNo(con, cbo_Colour.Text)
        If Clr_id = 0 Then
            MessageBox.Show("Invalid Colour", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Colour.Enabled Then cbo_Colour.Focus()
            Exit Sub
        End If

        Prss_ID = Common_Procedures.Process_NameToIdNo(con, cbo_Processing.Text)
        If Prss_ID = 0 Then
            MessageBox.Show("Invalid Processing", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Processing.Enabled Then cbo_Processing.Focus()
            Exit Sub
        End If


        With dgv_Details

            Sno = 0
            For i = 0 To .RowCount - 1

                If Trim(.Rows(i).Cells(1).Value) <> "" Or Val(.Rows(i).Cells(2).Value) <> 0 Then

                    If Trim(.Rows(i).Cells(1).Value) = "" Then
                        MessageBox.Show("Invalid Piece No", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(1)
                            .CurrentCell.Selected = True
                        End If
                        Exit Sub
                    End If

                    If Val(.Rows(i).Cells(2).Value) = 0 Then
                        MessageBox.Show("Invalid Meters", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(2)
                            .CurrentCell.Selected = True
                        End If
                        Exit Sub
                    End If

                    'If Val(.Rows(i).Cells(3).Value) = 0 Then
                    '    MessageBox.Show("Invalid Weight", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    '    If .Enabled And .Visible Then
                    '        .Focus()
                    '        .CurrentCell = .Rows(i).Cells(3)
                    '        .CurrentCell.Selected = True
                    '    End If
                    '    Exit Sub
                    'End If

                End If

            Next

        End With

        NoCalc_Status = False
        Total_Calculation()

        vTot_Pcs = 0 : vTot_Mtrs = 0 : vTot_Wgt = 0

        With dgv_Details_Total
            If .RowCount > 0 Then
                vTot_Pcs = Val(.Rows(0).Cells(1).Value())
                vTot_Mtrs = Val(.Rows(0).Cells(2).Value())
                vTot_Wgt = Val(.Rows(0).Cells(3).Value())
            End If
        End With

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RollNo.Text) & "/" & Trim(OpYrCode)

        Da = New SqlClient.SqlDataAdapter("select COUNT(*) from Processed_Fabric_Inspection_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Processed_Fabric_Inspection_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Roll_Code = '" & Trim(NewCode) & "' and Sales_Invoice_Code <> ''", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                If Val(Dt1.Rows(0)(0).ToString) <> 0 Then
                    MessageBox.Show("Already Delivered/Invoiced", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If
        Dt1.Clear()

        tr = con.BeginTransaction

        Try

            OpDate = New DateTime(Val(Microsoft.VisualBasic.Left(Trim(Common_Procedures.FnRange), 4)), 4, 1)
            OpDate = DateAdd(DateInterval.Day, -1, OpDate)

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RollNo.Text) & "/" & Trim(OpYrCode)

            Else

                lbl_RollNo.Text = Common_Procedures.get_MaxCode(con, "Processed_Fabric_Opening_Head", "Processed_Fabric_Opening_Code", "for_OrderBy", "", Val(lbl_Company.Tag), OpYrCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RollNo.Text) & "/" & Trim(OpYrCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@OpDate", OpDate)

            If New_Entry = True Then
                cmd.CommandText = "Insert into Processed_Fabric_Opening_Head (    Processed_Fabric_Opening_Code  ,               Company_IdNo       ,     Processed_Fabric_Opening_No ,                               for_OrderBy                               ,   Colour_IdNo        ,       Cloth_Idno         ,    Process_IdNo         ,              Total_Rolls  ,              Total_Meters  ,           Total_Weight     ) " & _
                                                       "     Values          ( '" & Trim(NewCode) & "'           , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RollNo.Text) & "' , " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RollNo.Text))) & ", " & Val(Clr_id) & "  , " & Str(Val(Fab_id)) & ", " & Str(Val(Prss_ID)) & ", " & Str(Val(vTot_Pcs)) & ", " & Str(Val(vTot_Mtrs)) & ", " & Str(Val(vTot_Wgt)) & " ) "
                cmd.ExecuteNonQuery()

            Else

                cmd.CommandText = "Update Processed_Fabric_Opening_Head set Colour_IdNo =  " & Val(Clr_id) & " , Cloth_IdNo = " & Str(Val(Fab_id)) & ", Process_IdNo =" & Str(Val(Prss_ID)) & ", Total_Rolls = " & Str(Val(vTot_Pcs)) & ", Total_Meters = " & Str(Val(vTot_Mtrs)) & ", Total_Weight = " & Str(Val(vTot_Wgt)) & " Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Processed_Fabric_Opening_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Delete from Processed_Fabric_Inspection_Details Where Processed_Fabric_Inspection_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Sales_Invoice_Code = '' "
                cmd.ExecuteNonQuery()

            End If

            stkof_idno = Val(Common_Procedures.CommonLedger.Godown_Ac)

            With dgv_Details

                Sno = 0
                For i = 0 To .RowCount - 1

                    If Trim(.Rows(i).Cells(1).Value) <> "" And Val(.Rows(i).Cells(2).Value) <> 0 Then

                        Sno = Sno + 1

                        Nr = 0
                        cmd.CommandText = "Update Processed_Fabric_Inspection_Details set    Colour_IdNo = " & Val(Clr_id) & ", Fabric_Idno =" & Val(Fab_id) & ", Process_IdNo= " & Val(Prss_ID) & " , Sl_No = " & Str(Val(Sno)) & ",  Meters = " & Str(Val(.Rows(i).Cells(2).Value)) & ", Weight = " & Str(Val(.Rows(i).Cells(3).Value)) & " where Processed_Fabric_Inspection_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Roll_Code = '" & Trim(NewCode) & "' and Pcs_No = '" & Trim(.Rows(i).Cells(1).Value) & "'"
                        Nr = cmd.ExecuteNonQuery()

                        If Nr = 0 Then
                            cmd.CommandText = "Insert into Processed_Fabric_Inspection_Details (      Roll_Code         ,               Roll_No          ,          Processed_Fabric_Inspection_Code   ,                Company_IdNo      ,   Processed_Fabric_Inspection_No,                               for_OrderBy                               ,  Processed_Fabric_Inspection_Date,    Colour_IdNo       ,       Fabric_Idno        ,    Process_IdNo          ,        Sl_No         ,      Pcs_No                            ,           Meters                       ,                     Weight                 ) " & _
                                                "     Values                                   ( '" & Trim(NewCode) & "', '" & Trim(lbl_RollNo.Text) & "', '" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RollNo.Text) & "' , " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RollNo.Text))) & ",                @OpDate           ,   " & Val(Clr_id) & ", " & Str(Val(Fab_id)) & ", " & Str(Val(Prss_ID)) & ", " & Str(Val(Sno)) & ", '" & Trim(.Rows(i).Cells(1).Value) & "', '" & Trim(.Rows(i).Cells(2).Value) & "',  " & Str(Val(.Rows(i).Cells(3).Value)) & " ) "
                            cmd.ExecuteNonQuery()
                        End If

                    End If

                Next
            End With


            cmd.CommandText = "Delete from Stock_Cloth_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            If Val(vTot_Mtrs) <> 0 Then
                cmd.CommandText = "Insert into Stock_Cloth_Processing_Details ( Reference_Code    ,             Company_IdNo         ,              Reference_No     ,                               for_OrderBy                              , Reference_Date,       StockOff_IdNo      ,                                       DeliveryTo_Idno     , ReceivedFrom_Idno, Entry_ID, Party_Bill_No, Particulars, Sl_No,         Cloth_Idno      ,           Colour_IdNo   ,        Process_IdNo     , Folding, UnChecked_Meters,  Meters_Type1      , Meters_Type2 , Meters_Type3 , Meters_Type4, Meters_Type5 ,             Rolls        ,              Weight        ) " & _
                                            " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RollNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RollNo.Text))) & ",     @OpDate   , " & Val(stkof_idno) & ", " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & ",         0        ,    ''   ,       ''     ,     ''     ,   1  , " & Str(Val(Fab_id)) & ", " & Str(Val(Clr_id)) & ", " & Str(Val(Prss_ID)) & ",      0   ,        0        ,        0       , " & Str(Val(vTot_Mtrs)) & ",       0      ,      0      ,     0        ," & Str(Val(vTot_Pcs)) & ", " & Str(Val(vTot_Wgt)) & " ) "
                cmd.ExecuteNonQuery()
            End If

            tr.Commit()

            MessageBox.Show("Saved Successfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)


            If Val(Common_Procedures.settings.OnSave_MoveTo_NewEntry_Status) = 1 Then
                If New_Entry = True Then
                    new_record()
                Else
                    move_record(lbl_RollNo.Text)
                End If
            Else
                move_record(lbl_RollNo.Text)
            End If

        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            Dt1.Dispose()
            Da.Dispose()
            cmd.Dispose()
            tr.Dispose()

            If cbo_FabricName.Enabled And cbo_FabricName.Visible Then cbo_FabricName.Focus()

        End Try

    End Sub

    Private Sub Total_Calculation()
        Dim TottlPc As Integer = 0
        Dim Tottlmr As Double = 0
        Dim Totwgt As Double = 0

        If NoCalc_Status = True Then Exit Sub

        TottlPc = 0 : Tottlmr = 0 : Totwgt = 0

        With dgv_Details
            For i = 0 To .RowCount - 1
                If Val(.Rows(i).Cells(2).Value) <> 0 Then

                    TottlPc = TottlPc + 1
                    Tottlmr = Tottlmr + Val(.Rows(i).Cells(2).Value())
                    Totwgt = Totwgt + Val(.Rows(i).Cells(3).Value())

                End If
            Next i

        End With

        With dgv_Details_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(1).Value = Val(TottlPc)
            .Rows(0).Cells(2).Value = Format(Val(Tottlmr), "########0.00")
            .Rows(0).Cells(3).Value = Format(Val(Totwgt), "########0.000")
        End With

    End Sub

    'Private Sub TotalMeter_Calculation()
    '    Dim fldmtr As Integer = 0
    '    Dim Tot_Pc_Mtrs As Single = 0, Tot_Pc_Wt As Single = 0
    '    Dim fldperc As Single = 0
    '    Dim Wgt_Mtr As Single = 0
    '    Dim k As Integer = 0

    '    On Error Resume Next

    '    With dgv_Details
    '        If .Visible Then
    '            If .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 6 Or .CurrentCell.ColumnIndex = 8 Then

    '                .CurrentRow.Cells(7).Value = Format(Val(.CurrentRow.Cells(2).Value) + Val(.CurrentRow.Cells(3).Value) + Val(.CurrentRow.Cells(4).Value) + Val(.CurrentRow.Cells(5).Value) + Val(.CurrentRow.Cells(6).Value), "#########0.00")

    '                'Tot_Pc_Mtrs = 0 : Tot_Pc_Wt = 0
    '                'For k = 0 To .Rows.Count - 1

    '                '    If Val(.CurrentRow.Cells(0).Value) = Val(.Rows(k).Cells(0).Value) Then
    '                '        Tot_Pc_Mtrs = Tot_Pc_Mtrs + Val(.Rows(k).Cells(2).Value) + Val(.Rows(k).Cells(3).Value) + Val(.Rows(k).Cells(4).Value) + Val(.Rows(k).Cells(5).Value) + Val(.Rows(k).Cells(6).Value)
    '                '        Tot_Pc_Wt = Tot_Pc_Wt + +Val(.Rows(k).Cells(8).Value)
    '                '    End If

    '                'Next

    '                Total_Calculation()

    '            End If

    '        End If
    '    End With
    'End Sub
    Private Sub cbo_Colour_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Colour.GotFocus, cbo_Colour.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Colour_Head", "Colour_Name", "", "(Colour_IdNo = 0)")
    End Sub

    Private Sub cbo_Colour_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Colour.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Colour, cbo_FabricName, cbo_Processing, "Colour_Head", "Colour_Name", "", "(Colour_IdNo = 0)")
    End Sub

    Private Sub cbo_Colour_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Colour.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Colour, cbo_Processing, "Colour_Head", "Colour_Name", "", "(Colour_IdNo = 0)")
    End Sub
    Private Sub cbo_Colour_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Colour.KeyUp
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

    Private Sub cbo_FabricName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_FabricName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "(Cloth_Type <> 'GREY')", "(Cloth_idno = 0)")
    End Sub

    Private Sub cbo_FabricName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_FabricName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_FabricName, Nothing, cbo_Colour, "Cloth_Head", "Cloth_Name", "(Cloth_Type <> 'GREY')", "(Cloth_idno = 0)")
    End Sub

    Private Sub cbo_FabricName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_FabricName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_FabricName, cbo_Colour, "Cloth_Head", "Cloth_Name", "(Cloth_Type <> 'GREY')", "(Cloth_idno = 0)")
    End Sub

    Private Sub cbo_ClothName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_FabricName.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New Cloth_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_FabricName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub
    Private Sub cbo_Processing_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Processing.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Process_Head", "Process_Name", "", "(Process_IdNo = 0)")
    End Sub


    Private Sub cbo_Processing_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Processing.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Processing, Nothing, Nothing, "Process_Head", "Process_Name", "", "(Process_IdNo = 0)")
        If (e.KeyValue = 38 And cbo_Processing.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then cbo_Colour.Focus() ' SendKeys.Send("+{TAB}")
        If (e.KeyValue = 40 And cbo_Processing.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
                dgv_Details.CurrentCell.Selected = True
            End If
        End If
    End Sub

    Private Sub cbo_Processing_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Processing.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Processing, Nothing, "Process_Head", "Process_Name", "", "(Process_IdNo = 0)")
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
        If Asc(e.KeyChar) = 13 Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
                dgv_Details.CurrentCell.Selected = True
            End If
        End If
    End Sub

    Private Sub cbo_Processing_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Processing.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New Process_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Processing.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub dgv_Details_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEndEdit
        dgv_Details_CellLeave(sender, e)
    End Sub

    Private Sub dgv_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellLeave

        With dgv_Details
            If .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 6 Or .CurrentCell.ColumnIndex = 7 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                Else
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
                End If
            End If

            If .CurrentCell.ColumnIndex = 8 Or .CurrentCell.ColumnIndex = 9 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.000")
                Else
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
                End If
            End If

        End With
    End Sub

    Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged

        On Error Resume Next

        With dgv_Details
            If .Visible Then
                If IsNothing(.CurrentCell) Then Exit Sub
                If .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3 Then
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
        dgv_Details.EditingControl.ForeColor = Color.Blue
        dgtxt_Details.SelectAll()
    End Sub

    Private Sub dgtxt_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyDown
        With dgv_Details


            If e.KeyValue = Keys.Delete Then

                If .CurrentCell.ColumnIndex = 2 Then
                    If Trim(.Rows(.CurrentCell.RowIndex).Cells(4).Value) <> "" Then
                        e.Handled = True
                    End If
                End If

                If .CurrentCell.ColumnIndex = 3 Then
                    If Trim(.Rows(.CurrentCell.RowIndex).Cells(4).Value) <> "" Then
                        e.Handled = True
                    End If
                End If

            End If

        End With

    End Sub

    Private Sub dgtxt_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyUp
        dgv_Details_KeyUp(sender, e)
    End Sub

    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Details.KeyPress
        On Error Resume Next
        With dgv_Details
            If .Visible Then
                If .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3 Then

                    If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                        e.Handled = True
                    End If

                    If .CurrentCell.ColumnIndex = 2 Then
                        If Trim(.Rows(.CurrentCell.RowIndex).Cells(4).Value) <> "" Then
                            e.Handled = True
                        End If
                    End If

                    If .CurrentCell.ColumnIndex = 3 Then
                        If Trim(.Rows(.CurrentCell.RowIndex).Cells(4).Value) <> "" Then
                            e.Handled = True
                        End If
                    End If
                End If
            End If

        End With
        With dgv_Details
            If Asc(e.KeyChar) = 13 Then
                If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then
                    If Trim(.Rows(.CurrentRow.Index).Cells(1).Value) = "" And .CurrentRow.Index = .Rows.Count - 1 Then
                        If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                            save_record()
                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)
                        End If
                    End If
                End If
            End If
        End With
    End Sub

    Private Sub dgv_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyDown

        'With dgv_Details

        '    If e.KeyCode = Keys.Left Then
        '        If .CurrentCell.ColumnIndex <= 0 Then
        '            If .CurrentCell.RowIndex = 0 Then
        '                txt_Folding.Focus()
        '            Else
        '                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 1)
        '            End If
        '        End If
        '    End If

        '    If e.KeyCode = Keys.Right Then
        '        If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then
        '            If Trim(.Rows(.CurrentRow.Index).Cells(1).Value) = "" And .CurrentRow.Index = .Rows.Count - 1 Then
        '                If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
        '                    save_record()
        '                Else
        '                    .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(2)
        '                End If
        '            End If
        '        End If
        '    End If

        'End With

    End Sub

    Private Sub dgv_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyUp
        Dim i As Integer
        Dim n As Integer
        Dim nrw As Integer
        Dim S As String


        If e.Control = True And (UCase(Chr(e.KeyCode)) = "A" Or UCase(Chr(e.KeyCode)) = "I" Or e.KeyCode = Keys.Insert) Then
            With dgv_Details

                n = .CurrentRow.Index

                S = Replace(Trim(.Rows(n).Cells(1).Value), Val(.Rows(n).Cells(1).Value), "")
                If Trim(UCase(S)) <> "Z" Then
                    S = Trim(UCase(S))
                    If Trim(S) = "" Then S = "A" Else S = Trim(Chr(Asc(S) + 1))
                    If n <> .Rows.Count - 1 Then
                        If Trim(Val(.Rows(n).Cells(1).Value)) & Trim(UCase(S)) = Trim(UCase(.Rows(n + 1).Cells(1).Value)) Then
                            MessageBox.Show("Already Piece Inserted", "DES NOT INSERT NEW PIECE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                            Exit Sub
                        End If
                    End If

                    nrw = n + 1

                    dgv_Details.Rows.Insert(nrw, 1)

                    dgv_Details.Rows(nrw).Cells(1).Value = Trim(Val(.Rows(n).Cells(1).Value)) & S

                    For i = 0 To .Rows.Count - 1
                        .Rows(i).Cells(1).Value = ""
                    Next

                End If

            End With

        End If

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_Details

                If Trim(.Rows(.CurrentCell.RowIndex).Cells(4).Value) = "" Then

                    n = .CurrentRow.Index

                    If .CurrentCell.RowIndex = .Rows.Count - 1 Then
                        For i = 0 To .ColumnCount - 1
                            .Rows(n).Cells(i).Value = ""
                        Next

                    Else
                        .Rows.RemoveAt(n)

                    End If

                    Total_Calculation()

                End If

            End With

        End If
    End Sub

    Private Sub dgv_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.LostFocus
        On Error Resume Next
        dgv_Details.CurrentCell.Selected = False
    End Sub


    Private Sub btn_save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub



    Public Sub print_record() Implements Interface_MDIActions.print_record
        '-------
    End Sub

    Private Sub txt_Folding_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Total_Calculation()
    End Sub

    Private Sub dgv_Details_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Details.RowsAdded
        Dim n As Integer

        With dgv_Details
            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)

        End With
    End Sub


    Private Sub dgv_Details_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellContentClick

    End Sub

    Private Sub dgv_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgv_Details.KeyPress
        With dgv_Details
            If Asc(e.KeyChar) = 13 Then
                If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then
                    If Trim(.Rows(.CurrentRow.Index).Cells(1).Value) = "" And .CurrentRow.Index = .Rows.Count - 1 Then
                        If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                            save_record()
                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)
                        End If
                    End If
                End If
            End If
        End With
    End Sub
End Class