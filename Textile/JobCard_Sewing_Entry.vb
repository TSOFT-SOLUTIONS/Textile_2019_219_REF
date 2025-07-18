Public Class JobCard_Sewing_Entry

    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "JBSEW-"
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

    Public Shared EntFnYrCode As String
    Public vmskOldText As String = ""
    Public vmskSelStrt As Integer = -1

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
        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))

        cbo_SalesParty.Text = ""

        cbo_Ledger.Enabled = True
        cbo_Ledger.BackColor = Color.White

        cbo_SalesParty.Enabled = True
        cbo_SalesParty.BackColor = Color.White

        cbo_Colour.Enabled = True
        cbo_Colour.BackColor = Color.White

        cbo_FpName.Enabled = True
        cbo_FpName.BackColor = Color.White

        cbo_Fabric.Enabled = True
        cbo_Fabric.BackColor = Color.White

        cbo_Size.Enabled = True
        cbo_Size.BackColor = Color.White

        dgv_Details.Rows.Clear()

        Grid_DeSelect()

        cbo_FpName.Visible = False
        cbo_Colour.Visible = False
        cbo_Size.Visible = False
        cbo_Fabric.Visible = False

        cbo_FpName.Tag = -1
        cbo_Grid_Process.Tag = -1
        cbo_Colour.Tag = -1
        cbo_Size.Tag = -1
        cbo_Fabric.Tag = -1

        cbo_FpName.Text = ""
        cbo_Grid_Process.Text = ""
        cbo_Colour.Text = ""
        cbo_Size.Text = ""
        cbo_Fabric.Text = ""

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
        Dim msktxbx As MaskedTextBox
        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Me.ActiveControl Is MaskedTextBox Then
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

        If Me.ActiveControl.Name <> cbo_Colour.Name Then
            cbo_Colour.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_FpName.Name Then
            cbo_FpName.Visible = False
        End If

        If Me.ActiveControl.Name <> cbo_Fabric.Name Then
            cbo_Fabric.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Size.Name Then
            cbo_Size.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Grid_Process.Name Then
            cbo_Grid_Process.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_FpName.Name Then
            cbo_FpName.Visible = False
            cbo_FpName.Tag = -1
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

            da1 = New SqlClient.SqlDataAdapter("select a.* from JobCard_Sewing_head a  Where a.JobCard_Sewing_Code = '" & Trim(NewCode) & "'", con)
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then
                lbl_JobNo.Text = dt1.Rows(0).Item("JobCard_Sewing_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("JobCard_Sewing_Date").ToString
                msk_Date.Text = dtp_Date.Text
                cbo_Ledger.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Sewing_IdNo").ToString))
                cbo_SalesParty.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("SalesParty_IdNo").ToString))
                lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))
                LockSTS = False
                If IsDBNull(dt1.Rows(0).Item("Fabric_Delivery_Code").ToString) = False Then
                    If Trim(dt1.Rows(0).Item("Fabric_Delivery_Code").ToString) <> "" Then
                        LockSTS = True
                    End If
                End If

                da2 = New SqlClient.SqlDataAdapter("select a.*, C.Processed_Item_Name as Fp_Name,d.Colour_Name,e.Size_Name,f.Cloth_Name as Fabric_Name  , g.Process_Name from JobCard_Sewing_Details a INNER JOIN Processed_Item_Head c ON  c.Processed_Item_IdNo = a.FinishedProduct_Idno LEFT OUTER JOIN Colour_Head d ON d.Colour_IdNo = a.Colour_IdNo LEFT OUTER JOIN Size_Head e ON e.Size_IdNo = a.Size_IdNo LEFT OUTER JOIN Cloth_Head f ON f.Cloth_idNo = a.Fabric_Idno LEFT OUTER JOIN Process_Head g ON a.Process_IdNo = g.Process_IdNo where a.JobCard_Sewing_Code = '" & Trim(NewCode) & "' Order by Sl_No", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_Details.Rows.Clear()
                SNo = 0

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_Details.Rows.Add()

                        SNo = SNo + 1
                        dgv_Details.Rows(n).Cells(0).Value = Val(SNo)
                        dgv_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Fp_Name").ToString
                        dgv_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Colour_Name").ToString
                        dgv_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Process_Name").ToString
                        dgv_Details.Rows(n).Cells(4).Value = dt2.Rows(i).Item("Size_Name").ToString
                        dgv_Details.Rows(n).Cells(5).Value = Val(dt2.Rows(i).Item("Quantity").ToString)
                        dgv_Details.Rows(n).Cells(6).Value = dt2.Rows(i).Item("Fabric_Name").ToString
                        dgv_Details.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Consum_Meter_Pcs").ToString), "########0.00")


                        '    dgv_Details.Rows(n).Cells(11).Value = dt2.Rows(i).Item("Cloth_JobCard_Sewing_Entry_Slno").ToString
                        '    dgv_Details.Rows(n).Cells(12).Value = dt2.Rows(i).Item("Receipt_Meters").ToString
                        '    dgv_Details.Rows(n).Cells(13).Value = dt2.Rows(i).Item("Return_Meters").ToString

                        '    If Val(dgv_Details.Rows(n).Cells(12).Value) <> 0 Or Val(dgv_Details.Rows(n).Cells(13).Value) <> 0 Then
                        '        For j = 0 To dgv_Details.ColumnCount - 1
                        '            dgv_Details.Rows(n).Cells(j).Style.BackColor = Color.LightGray
                        '        Next j
                        '        LockSTS = True
                        '    End If

                    Next i

                End If

                With dgv_Details_Total
                    If .RowCount = 0 Then .Rows.Add()
                    .Rows(0).Cells(5).Value = Val(dt1.Rows(0).Item("Total_Quantity").ToString)
                    .Rows(0).Cells(7).Value = Format(Val(dt1.Rows(0).Item("Total_Consum_Meter_Pcs").ToString), "########0.00")
                End With

                Grid_DeSelect()

                dt2.Clear()

                dt2.Dispose()
                da2.Dispose()

            End If

            dt1.Clear()
            dt1.Dispose()
            da1.Dispose()


            If LockSTS = True Then

                cbo_Ledger.Enabled = False
                cbo_Ledger.BackColor = Color.LightGray

                cbo_SalesParty.Enabled = False
                cbo_SalesParty.BackColor = Color.LightGray

                cbo_Colour.Enabled = False
                cbo_Colour.BackColor = Color.LightGray

                cbo_FpName.Enabled = False
                cbo_FpName.BackColor = Color.LightGray

                cbo_Fabric.Enabled = False
                cbo_Fabric.BackColor = Color.LightGray

                cbo_Size.Enabled = False
                cbo_Size.BackColor = Color.LightGray

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If msk_Date.Visible And msk_Date.Enabled Then msk_Date.Focus()

    End Sub

    Private Sub JobCard_Sewing_Entry_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Ledger.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Ledger.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_SalesParty.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_SalesParty.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_FpName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "FINISHEDPRODUCT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_FpName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If


            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Colour.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "COLOUR" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Colour.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Size.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "BRAND" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Size.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Fabric.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "FABRIC" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Fabric.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

    Private Sub JobCard_Sewing_Entry_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
       

        Me.Text = ""

        con.Open()


        cbo_FpName.Visible = False
        cbo_Colour.Visible = False
        cbo_Size.Visible = False
        cbo_Fabric.Visible = False


        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2

        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Colour.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_FpName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Ledger.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Size.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Fabric.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_SalesParty.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_Colour.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_Process.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_Filter_FpName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus

        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Colour.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_FpName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Ledger.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Size.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Fabric.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_SalesParty.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_Colour.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_Process.LostFocus, AddressOf ControlLostFocus


        AddHandler cbo_Filter_FpName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus

        AddHandler dtp_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler msk_Date.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler msk_Date.KeyPress, AddressOf TextBoxControlKeyPress

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

    Private Sub JobCard_Sewing_Entry_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub JobCard_Sewing_Entry_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
                    Exit Sub

                Else
                    If MessageBox.Show("Do you want to Close?", "FOR CLOSING ENTRY...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) <> Windows.Forms.DialogResult.Yes Then
                        Exit Sub

                    Else
                        Me.Close()
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
                                cbo_SalesParty.Focus()

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

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.JobCard_Sewing_Entry_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.JobCard_Sewing_Entry_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        Qa = MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
        If Qa = Windows.Forms.DialogResult.No Or Qa = Windows.Forms.DialogResult.Cancel Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If


        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_JobNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
      
      
        Da = New SqlClient.SqlDataAdapter("select count(*) from JobCard_Sewing_head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and JobCard_Sewing_Code = '" & Trim(NewCode) & "' and  Fabric_Delivery_Code <> ''", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                If Val(Dt1.Rows(0)(0).ToString) > 0 Then
                    MessageBox.Show("Already Fabric Delivered ", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If
        Dt1.Clear()
        
        trans = con.BeginTransaction

        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_JobNo.Text) & "/" & Trim(EntFnYrCode)

            cmd.Connection = con
            cmd.Transaction = trans


          
            cmd.CommandText = "delete from JobCard_Sewing_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and JobCard_Sewing_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from JobCard_Sewing_head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and JobCard_Sewing_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

           

            trans.Commit()

            Dt1.Dispose()
            Da.Dispose()
            trans.Dispose()
            cmd.Dispose()

            new_record()

            MessageBox.Show("Deleted Successfully!!!", "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            trans.Rollback()
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If msk_Date.Visible And msk_Date.Enabled Then msk_Date.Focus()

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        If Filter_Status = False Then


            dtp_Filter_Fromdate.Text = ""
            dtp_Filter_ToDate.Text = ""
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_FpName.Text = ""
            cbo_Filter_Colour.Text = ""

            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_FpName.SelectedIndex = -1
            cbo_Filter_Colour.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()

        End If

        pnl_Filter.Visible = True
        pnl_Filter.Enabled = True
        pnl_Back.Enabled = False
        If dtp_Filter_Fromdate.Enabled And dtp_Filter_Fromdate.Visible Then dtp_Filter_Fromdate.Focus()

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""

        Try

            da = New SqlClient.SqlDataAdapter("select top 1 JobCard_Sewing_No from JobCard_Sewing_head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and JobCard_Sewing_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, JobCard_Sewing_No", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 JobCard_Sewing_No from JobCard_Sewing_head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and JobCard_Sewing_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, JobCard_Sewing_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_JobNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 JobCard_Sewing_No from JobCard_Sewing_head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and JobCard_Sewing_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, JobCard_Sewing_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 JobCard_Sewing_No from JobCard_Sewing_head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and JobCard_Sewing_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, JobCard_Sewing_No desc", con)
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

            lbl_JobNo.Text = Common_Procedures.get_MaxCode(con, "JobCard_Sewing_head", "JobCard_Sewing_Code", "For_OrderBy", "", Val(lbl_Company.Tag), EntFnYrCode)

            lbl_JobNo.ForeColor = Color.Red

            msk_Date.Text = Date.Today.ToShortDateString


            ' dtp_date.Text = Date.Today.ToShortDateString
            da = New SqlClient.SqlDataAdapter("select top 1 * from JobCard_Sewing_head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and JobCard_Sewing_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, JobCard_Sewing_No desc", con)
            dt1 = New DataTable
            da.Fill(dt1)
            If dt1.Rows.Count > 0 Then
                If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)

                    If dt1.Rows(0).Item("JobCard_Sewing_Date").ToString <> "" Then msk_Date.Text = dt1.Rows(0).Item("JobCard_Sewing_Date").ToString
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

            Da = New SqlClient.SqlDataAdapter("select JobCard_Sewing_No from JobCard_Sewing_head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and JobCard_Sewing_Code = '" & Trim(RecCode) & "'", con)
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
                MessageBox.Show("Job No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.JobCard_Sewing_Entry_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.JobCard_Sewing_Entry_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        Try

            inpno = InputBox("Enter New Job No.", "FOR NEW SEWING INSERTION...")
         
            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select JobCard_Sewing_No from JobCard_Sewing_head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and JobCard_Sewing_Code = '" & Trim(RecCode) & "'", con)
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
                    MessageBox.Show("Invalid Job No", "DOES NOT INSERT NEW SEWING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_JobNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT NEW SEWING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim SelcCode As String = ""
        Dim Led_ID As Integer = 0
        Dim Col_ID As Integer = 0
        Dim Itfp_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim Partcls As String = ""
        Dim PBlNo As String = ""
        Dim EntID As String = ""
        Dim vTotMtrPcs As Single, vtotqty As Single
        Dim Sz_ID As Integer = 0
        Dim Fb_ID As Integer = 0
        Dim Sew_ID As Integer = 0
        Dim Proc_ID As Integer = 0
        Dim Sals_Id As Integer = 0
        Dim Nr As Integer = 0

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        ' If Common_Procedures.UserRight_Check(Common_Procedures.UR.JobCard_Sewing_Entry_Entry, New_Entry) = False Then Exit Sub

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

        Sew_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
        If Sew_ID = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
            Exit Sub
        End If

        Sals_Id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_SalesParty.Text)
        lbl_UserName.Text = Common_Procedures.User.IdNo

        With dgv_Details
            For i = 0 To dgv_Details.RowCount - 1
                If Val(.Rows(i).Cells(4).Value) <> 0 Then
                    If Trim(dgv_Details.Rows(i).Cells(1).Value) = "" Then
                        MessageBox.Show("Invalid FINISHEDPRODUCT NAME ", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dgv_Details.Enabled And dgv_Details.Visible Then
                            dgv_Details.Focus()
                            dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(1)

                        End If
                        Exit Sub
                    End If

                    If Trim(dgv_Details.Rows(i).Cells(2).Value) = "" Then
                        MessageBox.Show("Invalid COLOUR NAME", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dgv_Details.Enabled And dgv_Details.Visible Then
                            dgv_Details.Focus()
                            dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(2)

                        End If
                        Exit Sub

                    End If

                    'If Trim(dgv_Details.Rows(i).Cells(3).Value) = "" Then
                    '    MessageBox.Show("Invalid SIZE NAME", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    '    If dgv_Details.Enabled And dgv_Details.Visible Then
                    '        dgv_Details.Focus()
                    '        dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(3)

                    '    End If
                    '    Exit Sub

                    'End If


                    If Val(dgv_Details.Rows(i).Cells(4).Value) = 0 Then
                        MessageBox.Show("Invalid Quantity..", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dgv_Details.Enabled Then dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(4)
                        Exit Sub
                    End If

                End If

            Next
        End With

        Total_Calculation()

        vTotMtrPcs = 0 : vtotqty = 0


        If dgv_Details_Total.RowCount > 0 Then
            vtotqty = Val(dgv_Details_Total.Rows(0).Cells(5).Value())
            vTotMtrPcs = Val(dgv_Details_Total.Rows(0).Cells(7).Value())
        End If

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_JobNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_JobNo.Text = Common_Procedures.get_MaxCode(con, "JobCard_Sewing_head", "JobCard_Sewing_Code", "For_OrderBy", "", Val(lbl_Company.Tag), EntFnYrCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_JobNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            SelcCode = Trim(Val(lbl_JobNo.Text)) & "-" & Trim(lbl_Company.Tag) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@SewingDate", Convert.ToDateTime(msk_Date.Text))

            If New_Entry = True Then
                cmd.CommandText = "Insert into JobCard_Sewing_head(JobCard_Sewing_Code , JobCard_Code_FrSelec ,  Company_IdNo, JobCard_Sewing_No, for_OrderBy, JobCard_Sewing_Date, Sewing_IdNo,  SalesParty_IdNo, Total_Quantity, Total_Consum_Meter_Pcs  ,  User_IdNo ) Values ('" & Trim(NewCode) & "','" & Trim(SelcCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_JobNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_JobNo.Text))) & ", @SewingDate, " & Str(Val(Sew_ID)) & ",  " & Str(Val(Sals_Id)) & "," & Str(Val(vtotqty)) & "," & Str(Val(vTotMtrPcs)) & " , " & Val(lbl_UserName.Text) & ")"
                cmd.ExecuteNonQuery()

            Else

                cmd.CommandText = "Update JobCard_Sewing_head set JobCard_Code_FrSelec = '" & Trim(SelcCode) & "', JobCard_Sewing_Date = @SewingDate, Sewing_IdNo = " & Val(Sew_ID) & ",SalesParty_IdNo = " & Val(Sals_Id) & ",  Total_Quantity = " & Val(vtotqty) & ",Total_Consum_Meter_Pcs = " & Val(vTotMtrPcs) & ", User_idno = " & Val(lbl_UserName.Text) & "  Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and JobCard_Sewing_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If

            cmd.CommandText = "Delete from JobCard_Sewing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and JobCard_Sewing_Code = '" & Trim(NewCode) & "' "
            cmd.ExecuteNonQuery()

            Partcls = "Sew : Job.No. " & Trim(lbl_JobNo.Text)
            PBlNo = Trim(lbl_JobNo.Text)
            EntID = Trim(Pk_Condition) & Trim(lbl_JobNo.Text)

            With dgv_Details
                Sno = 0
                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(5).Value) <> 0 Then

                        Sno = Sno + 1
                        Itfp_ID = Common_Procedures.Processed_Item_NameToIdNo(con, .Rows(i).Cells(1).Value, tr)
                        Col_ID = Common_Procedures.Colour_NameToIdNo(con, .Rows(i).Cells(2).Value, tr)
                        Sz_ID = Common_Procedures.Size_NameToIdNo(con, .Rows(i).Cells(4).Value, tr)
                        Fb_ID = Common_Procedures.Cloth_NameToIdNo(con, .Rows(i).Cells(6).Value, tr)
                        Proc_ID = Common_Procedures.Process_NameToIdNo(con, .Rows(i).Cells(3).Value, tr)

                        'Nr = 0
                        'cmd.CommandText = "Update  JobCard_Sewing_Details set JobCard_Sewing_Date = @SewingDate , Ledger_IdNo = " & Str(Val(Led_ID)) & ", Sl_No  = " & Str(Val(Sno)) & " , Item_Idno = " & Str(Val(itgry_id)) & " , Item_To_Idno = " & Str(Val(Itfp_ID)) & " , Colour_Idno = " & Val(Col_ID) & " , Lot_IdNo = " & Val(Lot_ID) & " ,Processing_Idno = " & Val(Proc_ID) & " , Delivery_Pcs =  " & Val(.Rows(i).Cells(6).Value) & ", Delivery_Qty = " & Val(.Rows(i).Cells(7).Value) & " ,  Meter_Qty = " & Str(Val(.Rows(i).Cells(8).Value)) & " ,    Delivery_Meters = " & Str(Val(.Rows(i).Cells(9).Value)) & " ,    Delivery_Weight = " & Str(Val(.Rows(i).Cells(10).Value)) & " where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and JobCard_Sewing_Code = '" & Trim(NewCode) & "'  and Cloth_JobCard_Sewing_Entry_Slno = " & Val(.Rows(i).Cells(11).Value)
                        'Nr = cmd.ExecuteNonQuery()

                        'If Nr = 0 Then
                        cmd.CommandText = "Insert into JobCard_Sewing_Details(JobCard_Sewing_Code, Company_IdNo, JobCard_Sewing_No, for_OrderBy, JobCard_Sewing_Date,Sl_No, Sewing_IdNo,  FinishedProduct_Idno, Colour_Idno , Process_IdNo , Size_IdNo , Quantity , Fabric_Idno , Consum_Meter_Pcs ) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_JobNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_JobNo.Text))) & ", @SewingDate," & Str(Val(Sno)) & ", " & Str(Val(Sew_ID)) & " ," & Str(Val(Itfp_ID)) & ", " & Str(Val(Col_ID)) & ", " & Val(Proc_ID) & " ," & Val(Sz_ID) & ", " & Val(.Rows(i).Cells(5).Value) & ", " & Val(Fb_ID) & " ," & Val(.Rows(i).Cells(7).Value) & ")"
                        cmd.ExecuteNonQuery()
                        ' End If

                    End If

                Next

            End With

            tr.Commit()

            Dt1.Dispose()
            Da.Dispose()

            MessageBox.Show("Saved Successfully!!!", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)

            If Val(Common_Procedures.settings.OnSave_MoveTo_NewEntry_Status) = 1 Then
                If New_Entry = True Then
                    new_record()
                Else
                    move_record(lbl_JobNo.Text)
                End If
            Else
                move_record(lbl_JobNo.Text)
            End If

        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If msk_Date.Visible And msk_Date.Enabled Then msk_Date.Focus()



    End Sub


    Private Sub Total_Calculation()
        Dim vTotMtrPcs As Single, vtotqty As Single

        Dim i As Integer
        Dim sno As Integer

        vTotMtrPcs = 0 : vtotqty = 0 : sno = 0
        With dgv_Details
            For i = 0 To dgv_Details.Rows.Count - 1

                sno = sno + 1

                .Rows(i).Cells(0).Value = sno

                If Val(dgv_Details.Rows(i).Cells(5).Value) <> 0 Then
                    vtotqty = vtotqty + Val(dgv_Details.Rows(i).Cells(5).Value)
                    vTotMtrPcs = vTotMtrPcs + Val(dgv_Details.Rows(i).Cells(7).Value)

                End If
            Next
        End With
        If dgv_Details_Total.Rows.Count <= 0 Then dgv_Details_Total.Rows.Add()
        dgv_Details_Total.Rows(0).Cells(5).Value = Val(vtotqty)
        dgv_Details_Total.Rows(0).Cells(7).Value = Format(Val(vTotMtrPcs), "#########0.00")
    End Sub

    Private Sub cbo_Ledger_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = 'SEWING' )", "(Ledger_idno = 0)")

    End Sub

    Private Sub cbo_Ledger_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, msk_Date, cbo_SalesParty, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = 'SEWING' )", "(Ledger_idno = 0)")

    End Sub

    Private Sub cbo_Ledger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ledger.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Ledger, cbo_SalesParty, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = 'SEWING' ) ", "(Ledger_idno = 0)")

    End Sub

    Private Sub cbo_Ledger_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Common_Procedures.MDI_LedType = "SEWING"
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
        Dim rect As Rectangle
        Try
            With dgv_Details
                If Val(.CurrentRow.Cells(0).Value) = 0 Then
                    .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
                End If



                If e.ColumnIndex = 1 Then

                    If cbo_FpName.Visible = False Or Val(cbo_FpName.Tag) <> e.RowIndex Then

                        cbo_FpName.Tag = -1
                        Da = New SqlClient.SqlDataAdapter("select Processed_Item_Name from Processed_Item_Head  order by Processed_Item_Name", con)
                        Dt2 = New DataTable
                        Da.Fill(Dt2)
                        cbo_FpName.DataSource = Dt2
                        cbo_FpName.DisplayMember = "Procesed_Item_Name"

                        rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                        cbo_FpName.Left = .Left + rect.Left
                        cbo_FpName.Top = .Top + rect.Top
                        cbo_FpName.Width = rect.Width
                        cbo_FpName.Height = rect.Height

                        cbo_FpName.Text = .CurrentCell.Value

                        cbo_FpName.Tag = Val(e.RowIndex)
                        cbo_FpName.Visible = True

                        cbo_FpName.BringToFront()
                        cbo_FpName.Focus()

                        

                    End If

                Else

                    cbo_FpName.Visible = False
                    

                End If

                If e.ColumnIndex = 2 Then

                    If cbo_Colour.Visible = False Or Val(cbo_Colour.Tag) <> e.RowIndex Then

                        cbo_Colour.Tag = -1
                        Da = New SqlClient.SqlDataAdapter("select Colour_Name from Colour_Head order by Colour_Name", con)
                        Dt3 = New DataTable
                        Da.Fill(Dt3)
                        cbo_Colour.DataSource = Dt3
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

                If e.ColumnIndex = 3 Then

                    If cbo_Grid_Process.Visible = False Or Val(cbo_Grid_Process.Tag) <> e.RowIndex Then

                        cbo_Grid_Process.Tag = -1
                        Da = New SqlClient.SqlDataAdapter("select Process_Name from Process_Head order by Process_Name", con)
                        Dt1 = New DataTable
                        Da.Fill(Dt3)
                        cbo_Grid_Process.DataSource = Dt3
                        cbo_Grid_Process.DisplayMember = "Process_Name"

                        rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                        cbo_Grid_Process.Left = .Left + rect.Left
                        cbo_Grid_Process.Top = .Top + rect.Top

                        cbo_Grid_Process.Width = rect.Width
                        cbo_Grid_Process.Height = rect.Height
                        cbo_Grid_Process.Text = .CurrentCell.Value

                        cbo_Grid_Process.Tag = Val(e.RowIndex)
                        cbo_Grid_Process.Visible = True

                        cbo_Grid_Process.BringToFront()
                        cbo_Grid_Process.Focus()


                    End If

                Else
                    cbo_Grid_Process.Visible = False

                End If

                If e.ColumnIndex = 4 Then

                    If cbo_Size.Visible = False Or Val(cbo_Size.Tag) <> e.RowIndex Then

                        cbo_Size.Tag = -1
                        Da = New SqlClient.SqlDataAdapter("select Size_Name from Size_Head order by Size_Name", con)
                        Dt3 = New DataTable
                        Da.Fill(Dt3)
                        cbo_Size.DataSource = Dt3
                        cbo_Size.DisplayMember = "Size_Name"

                        rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                        cbo_Size.Left = .Left + rect.Left
                        cbo_Size.Top = .Top + rect.Top
                        cbo_Size.Width = rect.Width
                        cbo_Size.Height = rect.Height

                        cbo_Size.Text = .CurrentCell.Value

                        cbo_Size.Tag = Val(e.RowIndex)
                        cbo_Size.Visible = True

                        cbo_Size.BringToFront()
                        cbo_Size.Focus()


                    End If

                Else

                    cbo_Size.Visible = False


                End If

                If e.ColumnIndex = 6 Then

                    If cbo_Fabric.Visible = False Or Val(cbo_Fabric.Tag) <> e.RowIndex Then

                        cbo_Fabric.Tag = -1
                        Da = New SqlClient.SqlDataAdapter("select Cloth_Name from Cloth_Head where Cloth_type = 'PROCESSED FABRIC' order by Cloth_Name", con)
                        Dt3 = New DataTable
                        Da.Fill(Dt3)
                        cbo_Fabric.DataSource = Dt3
                        cbo_Fabric.DisplayMember = "Cloth_Name"

                        rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                        cbo_Fabric.Left = .Left + rect.Left
                        cbo_Fabric.Top = .Top + rect.Top
                        cbo_Fabric.Width = rect.Width
                        cbo_Fabric.Height = rect.Height

                        cbo_Fabric.Text = .CurrentCell.Value

                        cbo_Fabric.Tag = Val(e.RowIndex)
                        cbo_Fabric.Visible = True

                        cbo_Fabric.BringToFront()
                        cbo_Fabric.Focus()


                    End If

                Else

                    cbo_Fabric.Visible = False


                End If


            End With
        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL ENTER....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub dgv_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellLeave
        Try
            With dgv_Details

                If .CurrentCell.ColumnIndex = 7 Then
                    If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                        .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                    End If
                End If

            End With
        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL LEAVE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged
        Try
            If IsNothing(dgv_Details.CurrentCell) Then Exit Sub

            With dgv_Details
                If .Visible Then
                    If .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 7 Then
                        Total_Calculation()
                    End If
                End If
            End With

        Catch ex As NullReferenceException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As ObjectDisposedException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub



    Private Sub dgv_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Details.EditingControlShowing
        Try
            With dgv_Details
                If .Rows.Count > 0 Then
                    dgtxt_details = CType(dgv_Details.EditingControl, DataGridViewTextBoxEditingControl)
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS EDITING SHOWING....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub
    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_details.Enter
        Try

            dgv_Details.EditingControl.BackColor = Color.Lime
            dgv_Details.EditingControl.ForeColor = Color.Blue

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXT ENTER....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub
    Private Sub dgtxt_details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_details.KeyDown
        Try
            vcbo_KeyDwnVal = e.KeyValue

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXT KEYDOWN....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_details.KeyPress


        Try


            With dgv_Details


                If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                    e.Handled = True
                End If

            End With
        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXT KEYPRESS....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
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
        End With
    End Sub

    Private Sub cbo_Size_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Size.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Size_Head", "Size_Name", "", "(Size_Idno=0)")

    End Sub

    Private Sub cbo_Size_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Size.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Size, cbo_Colour, cbo_Fabric, "Size_Head", "Size_Name", "", "(Size_Idno=0)")
        With dgv_Details

            If (e.KeyValue = 38 And cbo_Size.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_Size.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
            End If

        End With
    End Sub

    Private Sub cbo_Size_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Size.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Size, cbo_Fabric, "Size_Head", "Size_Name", "", "(Size_Idno=0)")
        If Asc(e.KeyChar) = 13 Then

            With dgv_Details

                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

            End With

        End If
    End Sub

    Private Sub cbo_Size_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Size.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Size_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Size.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub



    Private Sub cbo_Size_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Size.TextChanged
        Try
            If cbo_Size.Visible Then
                With dgv_Details
                    If Val(cbo_Size.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 4 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Size.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Colour_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Colour.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "colour_Head", "colour_Name", "", "(Colour_IdNo = 0)")

    End Sub
    Private Sub cbo_colour_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Colour.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Colour, cbo_FpName, cbo_Size, "colour_Head", "colour_Name", "", "(Colour_IdNo = 0)")
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
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Colour, cbo_Size, "colour_Head", "colour_Name", "", "(Colour_IdNo = 0)")
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
        Try
            If cbo_Colour.Visible Then
                With dgv_Details
                    If Val(cbo_Colour.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 2 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Colour.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Fabric_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Fabric.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_name", "(Cloth_Type = 'PROCESSED FABRIC')", "(Cloth_iDNO = 0)")

    End Sub

    Private Sub cbo_Fabric_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Fabric.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Fabric, Nothing, Nothing, "Cloth_Head", "Cloth_name", "(Cloth_Type = 'PROCESSED FABRIC')", "(Cloth_iDNO = 0)")
        With dgv_Details

            If (e.KeyValue = 38 And cbo_Fabric.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_Fabric.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
            End If

        End With
    End Sub

    Private Sub cbo_Fabric_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Fabric.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Fabric, Nothing, "Cloth_Head", "Cloth_name", "(Cloth_Type = 'PROCESSED FABRIC')", "(Cloth_iDNO = 0)")
        If Asc(e.KeyChar) = 13 Then

            With dgv_Details

                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

            End With

        End If
    End Sub

    Private Sub cbo_Fabric_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Fabric.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Cloth_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Fabric.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub


    Private Sub cbo_Fabric_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Fabric.TextChanged
        Try
            If cbo_Fabric.Visible Then
                With dgv_Details
                    If Val(cbo_Fabric.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 6 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Fabric.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub
    Private Sub cbo_Grid_Process_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Process.GotFocus

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, Con, "Process_Head", "Process_Name", "", "(Process_IdNo = 0)")

    End Sub
    Private Sub cbo_Grid_Process_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Process.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, Con, cbo_Grid_Process, Nothing, Nothing, "Process_Head", "Process_Name", "", "(process_IdNo = 0)")
        vcbo_KeyDwnVal = e.KeyValue

        With dgv_Details

            If (e.KeyValue = 38 And cbo_Grid_Process.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_Grid_Process.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
            End If

        End With

    End Sub

    Private Sub cbo_Grid_Process_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_Process.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, Con, cbo_Grid_Process, Nothing, "Process_Head", "Process_Name", "", "(Process_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            With dgv_Details
                e.Handled = True
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
            End With

        End If

    End Sub

    Private Sub cbo_Grid_Process_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Process.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Process_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_Process.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub
    Private Sub cbo_Grid_Process_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Process.TextChanged
        Try
            If cbo_Grid_Process.Visible Then
                With dgv_Details
                    If Val(cbo_Grid_Process.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 3 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_Process.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub
    Private Sub cbo_FpName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_FpName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Processed_Item_Head", "Processed_Item_Name", "", "(Processed_Item_idno = 0)")

    End Sub

    Private Sub cbo_FpName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_FpName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_FpName, Nothing, cbo_Colour, "Processed_Item_Head", "Processed_Item_Name", "", "(Processed_Item_idno = 0)")
        With dgv_Details

            If (e.KeyValue = 38 And cbo_FpName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                If Val(.CurrentCell.RowIndex) <= 0 Then

                    cbo_SalesParty.Focus()


                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(7)
                    .CurrentCell.Selected = True

                End If
            End If

            If (e.KeyValue = 40 And cbo_FpName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                If .CurrentCell.RowIndex = .Rows.Count - 1 And Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(1).Value) = "" Then
                    If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                        save_record()
                    Else
                        msk_Date.Focus()

                    End If
                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

                End If

            End If

        End With

    End Sub

    Private Sub cbo_FpName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_FpName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_FpName, cbo_Colour, "Processed_Item_Head", "Processed_Item_Name", "", "(Processed_Item_idno = 0)")
        If Asc(e.KeyChar) = 13 Then

            With dgv_Details

                If Trim(.Rows(.CurrentRow.Index).Cells(1).Value) = "" And .CurrentRow.Index = .Rows.Count - 1 Then
                    If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                        save_record()
                    Else
                        msk_Date.Focus()

                    End If
                Else
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(2)

                End If

            End With

        End If
    End Sub

    Private Sub cbo_FpName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_FpName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New FinishedProduct_Creation_Simple

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_FpName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_FpName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_FpName.TextChanged
        Try
            If cbo_FpName.Visible Then
                With dgv_Details
                    If Val(cbo_FpName.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_FpName.Text)
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
        Dim Led_IdNo As Integer, Col_IdNo As Integer, Fp_IdNo
        Dim Condt As String = ""


        Try

            Condt = ""
            Led_IdNo = 0
            Col_IdNo = 0
            Fp_IdNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.JobCard_Sewing_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.JobCard_Sewing_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.JobCard_Sewing_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If

            If Trim(cbo_Filter_FpName.Text) <> "" Then
                Fp_IdNo = Common_Procedures.Processed_Item_NameToIdNo(con, cbo_Filter_FpName.Text)
            End If

            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.Sewing_IdNo = " & Str(Val(Led_IdNo))
            End If


            If Val(Fp_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " c.FinishedProduct_Idno = " & Str(Val(Fp_IdNo))
            End If
            If Trim(cbo_Filter_Colour.Text) <> "" Then
                Col_IdNo = Common_Procedures.Colour_NameToIdNo(con, cbo_Filter_Colour.Text)
            End If
            If Val(Col_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " c.Colour_Idno = " & Str(Val(Col_IdNo))
            End If
            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name,c.*,d.Processed_Item_Name as Fp_Name,e.Colour_Name,f.Size_Name from JobCard_Sewing_head a INNER JOIN Ledger_Head b on a.Sewing_IdNo = b.Ledger_IdNo INNER JOIN JobCard_Sewing_Details c ON c.JobCard_Sewing_Code = a.JobCard_Sewing_Code INNER JOIN Processed_Item_Head d ON d.Processed_Item_IdNo = c.FinishedProduct_IdNo INNER JOIN Colour_Head e ON c.Colour_Idno = e.Colour_IdNo LEFT oUTER JOIN Size_Head f ON c.Size_Idno = f.Size_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.JobCard_Sewing_Code LIKE '%/" & Trim(EntFnYrCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.JobCard_Sewing_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()


                    dgv_Filter_Details.Rows(n).Cells(0).Value = dt2.Rows(i).Item("JobCard_Sewing_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(1).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("JobCard_Sewing_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Fp_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = dt2.Rows(i).Item("Colour_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(5).Value = dt2.Rows(i).Item("Size_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(6).Value = Val(dt2.Rows(i).Item("Quantity").ToString)
                    dgv_Filter_Details.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Consum_Meter_Pcs").ToString), "########0.00")
                    
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
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = 'SEWING' and Verified_Status = 1)", "(Ledger_idno = 0)")

    End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, cbo_Filter_Colour, cbo_Filter_FpName, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = 'SEWING'  and Verified_Status = 1)", "(Ledger_idno = 0)")

    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, cbo_Filter_FpName, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = 'SEWING' and Verified_Status = 1) ", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_FpName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_FpName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Processed_Item_Head", "Processed_Item_Name", "", "(Processed_Item_idno = 0)")

    End Sub

    Private Sub cbo_Filter_FpName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_FpName.KeyDown

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_FpName, cbo_Filter_PartyName, btn_Filter_Show, "Processed_Item_Head", "Processed_Item_Name", "", "(Processed_Item_idno = 0)")

    End Sub

    Private Sub cbo_Filter_FpName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_FpName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_FpName, btn_Filter_Show, "Processed_Item_Head", "Processed_Item_Name", "", "(Processed_Item_idno = 0)")
    End Sub
    Private Sub cbo_Filter_Colour_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_Colour.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Colour_Head", "Colour_Name", "", "(Colour_iDNO = 0)")
    End Sub

    Private Sub cbo_Filter_ColourName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_Colour.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_Colour, dtp_Filter_ToDate, cbo_Filter_PartyName, "Colour_Head", "Colour_Name", "", "Colour_IdNo = 0")
    End Sub

    Private Sub cbo_Filter_ColourName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_Colour.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_Colour, cbo_Filter_PartyName, "Colour_Head", "Colour_Name", "", "Colour_IdNo = 0")
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


    End Sub

    Private Sub cbo_SalesParty_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_SalesParty.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = '' and Verified_Status = 1)", "(Ledger_IdNo = 0)")

    End Sub


    Private Sub cbo_SalesParty_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_SalesParty.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_SalesParty, cbo_Ledger, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = ''and Verified_Status = 1)", "(Ledger_IdNo = 0)")
       
        If (e.KeyValue = 40 And cbo_SalesParty.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)

            Else
                dtp_Date.Focus()

            End If
        End If
    End Sub

    Private Sub cbo_SalesParty_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_SalesParty.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_SalesParty, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = '' and Verified_Status = 1)", "(Ledger_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)

            Else
                dtp_Date.Focus()

            End If
        End If
    End Sub

    Private Sub cbo_SalesParty_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_SalesParty.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Common_Procedures.MDI_LedType = ""
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_SalesParty.Name
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

    Private Sub cbo_FpName_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_FpName.SelectedIndexChanged

    End Sub
End Class