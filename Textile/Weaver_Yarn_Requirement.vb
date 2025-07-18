Public Class Weaver_Yarn_Requirement



    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "YNREQ-"
    Private NoCalc_Status As Boolean = False
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Private vCbo_ItmNm As String

    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl

    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
    Private prn_DetAr(50, 10) As String
    Private prn_DetMxIndx As Integer
    Private prn_NoofBmDets As Integer
    Private prn_DetSNo As Integer

    Private prn_TotCopies As Integer = 0
    Private prn_Count As Integer = 0
    Private ConesPer_Bag As Integer = 0
    Private WgtPer_Bag As Integer = 0

    Public vmskOldText As String = ""

    Public vmskSelStrt As Integer = -1
    Private Sub clear()

        NoCalc_Status = True

        New_Entry = False
        Insert_Entry = False

        Pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        '  pnl_Count_Stock.Visible = False
        lbl_DcNo.Text = ""
        lbl_DcNo.ForeColor = Color.Black
        msk_date.Text = ""
        dtp_Date.Text = ""
        cbo_DelvTo.Text = ""
        cbo_RecFrom.Text = Common_Procedures.Ledger_IdNoToName(con, 4)
        cbo_CountName.Text = ""
        cbo_MillName.Text = ""
        cbo_YarnType.Text = ""


        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))
        cbo_Filter_CountName.Text = ""
        cbo_Filter_MillName.Text = ""
        cbo_Filter_PartyName.Text = ""

        txt_Weight_Bag.Text = ""
        txt_Cone_Bag.Text = ""
        txt_Weight_Cone.Text = ""


        dgv_YarnDetails.Rows.Clear()
        dgv_YarnDetails_Total.Rows.Clear()
        dgv_YarnDetails_Total.Rows.Add()
        dgv_YarnStock.Rows.Clear()
        If Filter_Status = False Then
            dtp_Filter_Fromdate.Text = ""
            dtp_Filter_ToDate.Text = ""
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_CountName.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_CountName.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()
        End If

        Grid_Cell_DeSelect()
        cbo_CountName.Enabled = True
        cbo_CountName.BackColor = Color.White

        cbo_RecFrom.Enabled = True
        cbo_RecFrom.BackColor = Color.White

        cbo_YarnType.Enabled = True
        cbo_YarnType.BackColor = Color.White



        cbo_MillName.Enabled = True
        cbo_MillName.BackColor = Color.White

        cbo_DelvTo.Visible = False

        NoCalc_Status = False
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

        If Me.ActiveControl.Name <> cbo_DelvTo.Name Then
            cbo_DelvTo.Visible = False
        End If



        If Me.ActiveControl.Name <> dgv_YarnDetails_Total.Name Then
            Grid_DeSelect()
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

    Private Sub Grid_DeSelect()
        On Error Resume Next
        dgv_YarnDetails.CurrentCell.Selected = False
        dgv_YarnDetails_Total.CurrentCell.Selected = False
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
        dgv_YarnDetails.CurrentCell.Selected = False
        dgv_YarnDetails_Total.CurrentCell.Selected = False
        dgv_Filter_Details.CurrentCell.Selected = False
    End Sub

    Private Sub Weaver_Yarn_Delivery_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_DelvTo.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_DelvTo.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_RecFrom.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_RecFrom.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If



            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_YarnType.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "YARN" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_YarnType.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_CountName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "COUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_CountName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_MillName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "MILL" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_MillName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

    Private Sub Weaver_Yarn_Delivery_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub
    Private Sub Weaver_Yarn_Delivery_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim dt5 As New DataTable
        Dim dt6 As New DataTable
        Dim dt7 As New DataTable
        Dim dt8 As New DataTable

        Me.Text = ""

        con.Open()




        cbo_DelvTo.Visible = False

        dtp_Date.Text = ""
        msk_date.Text = ""

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2

        AddHandler msk_date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_DelvTo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_RecFrom.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_CountName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_MillName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_YarnType.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Weight_Bag.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Cone_Bag.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Weight_Cone.GotFocus, AddressOf ControlGotFocus


        AddHandler cbo_Filter_CountName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_MillName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus




        AddHandler msk_date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_DelvTo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_RecFrom.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_CountName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_MillName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_YarnType.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Weight_Bag.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Weight_Cone.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Cone_Bag.LostFocus, AddressOf ControlLostFocus


        AddHandler cbo_Filter_CountName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_MillName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus

        'AddHandler msk_date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Weight_Bag.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Cone_Bag.KeyDown, AddressOf TextBoxControlKeyDown

        'AddHandler msk_date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Weight_Bag.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Cone_Bag.KeyPress, AddressOf TextBoxControlKeyPress

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True
        new_record()

        'Dgv_Details Columns- 35,100,85,270,70,80,105
        'Dgv_Details Columns- 35,90,80,240,65,70,85,75 After Add Thiri

    End Sub
    Private Sub Weaver_Yarn_Delivery_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

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

        If ActiveControl.Name = dgv_YarnDetails.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            dgv1 = Nothing

            If ActiveControl.Name = dgv_YarnDetails.Name Then
                dgv1 = dgv_YarnDetails

            ElseIf dgv_YarnDetails.IsCurrentRowDirty = True Then
                dgv1 = dgv_YarnDetails
            ElseIf Pnl_Back.Enabled = True Then
                dgv1 = dgv_YarnDetails



            End If

            If IsNothing(dgv1) = False Then

                With dgv1


                    If keyData = Keys.Enter Or keyData = Keys.Down Then
                        If .CurrentCell.ColumnIndex >= .ColumnCount - 5 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then

                                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                                    save_record()
                                Else
                                    msk_date.Focus()
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
                                txt_Weight_Cone.Focus()

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 3)

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

            da1 = New SqlClient.SqlDataAdapter("select a.* from Weaver_Yarn_Requirement_Head a Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_Yarn_Requirement_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_DcNo.Text = dt1.Rows(0).Item("Weaver_Yarn_Requirement_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Weaver_Yarn_Requirement_Date").ToString
                msk_date.Text = dtp_Date.Text

                cbo_RecFrom.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("ReceivedFrom_IdNo").ToString))
                cbo_CountName.Text = Common_Procedures.Count_IdNoToName(con, Val(dt1.Rows(0).Item("Count_IdNo").ToString))
                cbo_MillName.Text = Common_Procedures.Mill_IdNoToName(con, Val(dt1.Rows(0).Item("Mill_IdNo").ToString))
                cbo_YarnType.Text = dt1.Rows(0).Item("Yarn_Type").ToString

                txt_Weight_Bag.Text = (dt1.Rows(0).Item("Weight_Bag").ToString)
                txt_Cone_Bag.Text = Val(dt1.Rows(0).Item("Cone_Bag").ToString)
                txt_Weight_Cone.Text = Format(Val(dt1.Rows(0).Item("Weight_Cone").ToString), "###########0.000")

                lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))

                dt2.Clear()

                da2 = New SqlClient.SqlDataAdapter("select a.* from Weaver_Yarn_Requirement_Details a  where a.Weaver_Yarn_Requirement_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_YarnDetails.Rows.Clear()
                SNo = 0

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_YarnDetails.Rows.Add()

                        SNo = SNo + 1
                        dgv_YarnDetails.Rows(n).Cells(0).Value = Val(SNo)
                        dgv_YarnDetails.Rows(n).Cells(1).Value = Common_Procedures.Ledger_IdNoToName(con, Val(dt2.Rows(i).Item("DeliveryTo_IdNo").ToString))
                        dgv_YarnDetails.Rows(n).Cells(2).Value = Val(dt2.Rows(i).Item("Bags").ToString)
                        dgv_YarnDetails.Rows(n).Cells(3).Value = Val(dt2.Rows(i).Item("Cones").ToString)
                        dgv_YarnDetails.Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("Weight").ToString), "########0.000")
                        dgv_YarnDetails.Rows(n).Cells(5).Value = Val(dt2.Rows(i).Item("Delivery_Bag").ToString)
                        dgv_YarnDetails.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Delivery_Weight").ToString), "########0.000")
                        dgv_YarnDetails.Rows(n).Cells(7).Value = Val(dt2.Rows(i).Item("Delivery_Cone").ToString)
                        dgv_YarnDetails.Rows(n).Cells(8).Value = Val(dt2.Rows(i).Item("Weaver_Yarn_Requirement_Details_slno").ToString)
                        If Val(dgv_YarnDetails.Rows(n).Cells(5).Value) <> 0 Or Val(dgv_YarnDetails.Rows(n).Cells(6).Value) <> 0 Or Val(dgv_YarnDetails.Rows(n).Cells(7).Value) <> 0 Then
                            For j = 0 To dgv_YarnDetails.ColumnCount - 1
                                dgv_YarnDetails.Rows(n).Cells(j).Style.BackColor = Color.LightGray
                            Next j
                            LockSTS = True
                        End If
                    Next i

                End If

                With dgv_YarnDetails_Total
                    If .RowCount = 0 Then .Rows.Add()
                    .Rows(0).Cells(2).Value = Val(dt1.Rows(0).Item("Total_Bags").ToString)
                    .Rows(0).Cells(3).Value = Val(dt1.Rows(0).Item("Total_Cones").ToString)
                    .Rows(0).Cells(4).Value = Format(Val(dt1.Rows(0).Item("Total_Weight").ToString), "########0.000")
                    .Rows(0).Cells(5).Value = Val(dt1.Rows(0).Item("Total_Delivery_Bag").ToString)
                    .Rows(0).Cells(6).Value = Format(Val(dt1.Rows(0).Item("Total_Delivery_Weight").ToString), "########0.000")

                End With

                dt2.Clear()

                dt2.Dispose()
                da2.Dispose()
                Yarn_Stock_Checking()
                If LockSTS = True Then

                    cbo_CountName.Enabled = False
                    cbo_CountName.BackColor = Color.LightGray

                    cbo_RecFrom.Enabled = False
                    cbo_RecFrom.BackColor = Color.LightGray

                    cbo_YarnType.Enabled = False
                    cbo_YarnType.BackColor = Color.LightGray

                   

                    cbo_MillName.Enabled = False
                    cbo_MillName.BackColor = Color.LightGray

                  

                End If
            End If

            dt1.Clear()
            dt1.Dispose()
            da1.Dispose()

            Grid_Cell_DeSelect()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If msk_date.Visible And msk_date.Enabled Then msk_date.Focus()

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""

        If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Weaver_Yarn_Delivery_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Weaver_Yarn_Delivery_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Pnl_Back.Enabled = False Then
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
        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Da = New SqlClient.SqlDataAdapter("select sum(Delivery_Weight) from Weaver_Yarn_Requirement_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Yarn_Requirement_Code = '" & Trim(NewCode) & "'", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                If Val(Dt1.Rows(0)(0).ToString) > 0 Then
                    MessageBox.Show("Already Some Delivery Prepared", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If
        Dt1.Clear()
        trans = con.BeginTransaction

        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.Transaction = trans

            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), trans)

            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Empty_BeamBagCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Weaver_Yarn_Requirement_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Yarn_Requirement_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "delete from Weaver_Yarn_Requirement_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Yarn_Requirement_Code = '" & Trim(NewCode) & "'"
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

            If msk_date.Enabled = True And msk_date.Visible = True Then msk_date.Focus()

        End Try

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        If Filter_Status = False Then




            dtp_Filter_Fromdate.Text = ""
            dtp_Filter_ToDate.Text = ""
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_CountName.Text = ""
            cbo_Filter_MillName.Text = ""

            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_CountName.SelectedIndex = -1
            cbo_Filter_MillName.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()

        End If

        pnl_Filter.Visible = True
        pnl_Filter.Enabled = True
        pnl_Filter.BringToFront()
        Pnl_Back.Enabled = False
        If dtp_Filter_Fromdate.Enabled And dtp_Filter_Fromdate.Visible Then dtp_Filter_Fromdate.Focus()

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String

        Try

            da = New SqlClient.SqlDataAdapter("select top 1 Weaver_Yarn_Requirement_No from Weaver_Yarn_Requirement_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Yarn_Requirement_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Weaver_Yarn_Requirement_No", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 Weaver_Yarn_Requirement_No from Weaver_Yarn_Requirement_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Yarn_Requirement_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Weaver_Yarn_Requirement_No", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 Weaver_Yarn_Requirement_No from Weaver_Yarn_Requirement_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Yarn_Requirement_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Weaver_Yarn_Requirement_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 Weaver_Yarn_Requirement_No from Weaver_Yarn_Requirement_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Yarn_Requirement_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Weaver_Yarn_Requirement_No desc", con)
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

            lbl_DcNo.Text = Common_Procedures.get_MaxCode(con, "Weaver_Yarn_Requirement_Head", "Weaver_Yarn_Requirement_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

            lbl_DcNo.ForeColor = Color.Red


            msk_date.Text = Date.Today.ToShortDateString
            da = New SqlClient.SqlDataAdapter("select top 1 * from Weaver_Yarn_Requirement_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Yarn_Requirement_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Weaver_Yarn_Requirement_No desc", con)
            dt1 = New DataTable
            da.Fill(dt1)
            If dt1.Rows.Count > 0 Then
                If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)
                    If dt1.Rows(0).Item("Weaver_Yarn_Requirement_Date").ToString <> "" Then msk_date.Text = dt1.Rows(0).Item("Weaver_Yarn_Requirement_Date").ToString
                End If
            End If
            dt1.Clear()

            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()

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

            Da = New SqlClient.SqlDataAdapter("select Weaver_Yarn_Requirement_No from Weaver_Yarn_Requirement_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Yarn_Requirement_Code = '" & Trim(RecCode) & "'", con)
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

        If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Weaver_Yarn_Delivery_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Weaver_Yarn_Delivery_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        Try

            inpno = InputBox("Enter New Dc No.", "FOR NEW RECEIPT INSERTION...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Weaver_Yarn_Requirement_No from Weaver_Yarn_Requirement_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Yarn_Requirement_Code = '" & Trim(RecCode) & "'", con)
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
                    MessageBox.Show("Invalid Dc No", "DOES NOT INSERT NEW RECEIPT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_DcNo.Text = Trim(UCase(inpno))

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
        Dim Delv_ID As Integer = 0
        Dim Rec_ID As Integer = 0
        Dim Trans_ID As Integer = 0
        Dim Clo_ID As Integer = 0
        Dim EdsCnt_ID As Integer = 0
        Dim Sno As Integer = 0, YSno As Integer = 0
        Dim Partcls As String = ""
        Dim PBlNo As String = ""
        Dim YCnt_ID As Integer = 0
        Dim YMil_ID As Integer = 0
        Dim vTotYrnWeight_Bag As Single, vTotYrnCones As Single, vTotYrnWeight As Single, vTotYrnDWgt As Single, vTotYrnDBgs As Single
        Dim EntID As String = ""
        Dim yarn_Stock As Double = 0
        Dim Nr As Integer = 0
        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Common_Procedures.UserRight_Check(Common_Procedures.UR.Weaver_Yarn_Delivery_Entry, New_Entry) = False Then Exit Sub

        If Pnl_Back.Enabled = False Then
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

        lbl_UserName.Text = Common_Procedures.User.IdNo
        Rec_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_RecFrom.Text)
        If Rec_ID = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_RecFrom.Enabled And cbo_RecFrom.Visible Then cbo_RecFrom.Focus()
            Exit Sub
        End If
        If Rec_ID = 0 Then Rec_ID = 4

        YCnt_ID = Common_Procedures.Count_NameToIdNo(con, cbo_CountName.Text)
        If Val(YCnt_ID) = 0 Then
            MessageBox.Show("Invalid CountName", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_CountName.Enabled And cbo_CountName.Visible Then cbo_CountName.Focus()
            Exit Sub
        End If

        YMil_ID = Common_Procedures.Mill_NameToIdNo(con, cbo_MillName.Text)
        If Val(YMil_ID) = 0 Then
            MessageBox.Show("Invalid MillName", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_MillName.Enabled And cbo_MillName.Visible Then cbo_MillName.Focus()
            Exit Sub
        End If




        For i = 0 To dgv_YarnDetails.RowCount - 1

            If Val(dgv_YarnDetails.Rows(i).Cells(4).Value) <> 0 Then

                Delv_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, dgv_YarnDetails.Rows(i).Cells(1).Value)
                If Val(Delv_ID) = 0 Then
                    MessageBox.Show("Invalid DeliveryName", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(1)
                    dgv_YarnDetails.Focus()
                    Exit Sub
                End If

            End If

        Next

        vTotYrnWeight_Bag = 0 : vTotYrnCones = 0 : vTotYrnWeight = 0 : vTotYrnDWgt = 0 : vTotYrnDBgs = 0
        If dgv_YarnDetails_Total.RowCount > 0 Then
            vTotYrnWeight_Bag = Val(dgv_YarnDetails_Total.Rows(0).Cells(2).Value())
            vTotYrnCones = Val(dgv_YarnDetails_Total.Rows(0).Cells(3).Value())
            vTotYrnWeight = Format(Val(dgv_YarnDetails_Total.Rows(0).Cells(4).Value()), "#######0.000")
            vTotYrnDBgs = Val(dgv_YarnDetails_Total.Rows(0).Cells(5).Value())
            vTotYrnDWgt = Val(dgv_YarnDetails_Total.Rows(0).Cells(6).Value())
        End If


        'If Format(Val(vTotYrnWeight), "#######0.000") <> Val(txt_Weight.Text) Then
        '    MessageBox.Show("Mismatch of Weight", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    If txt_Weight.Enabled And txt_Weight.Visible Then txt_Weight.Focus()
        '    Exit Sub
        'End If

        'If Val(vTotYrnWeight_Bag) <> Val(txt_Bag.Text) Then
        '    MessageBox.Show("Mismatch of Weight_Bag", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    If txt_Bag.Enabled And txt_Bag.Visible Then txt_Bag.Focus()
        '    Exit Sub
        'End If

        'If Val(vTotYrnCones) <> Val(txt_Cone.Text) Then
        '    MessageBox.Show("Mismatch of Cone", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    If txt_Cone.Enabled And txt_Cone.Visible Then txt_Cone.Focus()
        '    Exit Sub
        'End If

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_DcNo.Text = Common_Procedures.get_MaxCode(con, "Weaver_Yarn_Requirement_Head", "Weaver_Yarn_Requirement_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@EntryDate", Convert.ToDateTime(msk_date.Text))

            If New_Entry = True Then
                cmd.CommandText = "Insert into Weaver_Yarn_Requirement_Head(Weaver_Yarn_Requirement_Code, Company_IdNo, Weaver_Yarn_Requirement_No, for_OrderBy, Weaver_Yarn_Requirement_Date, DeliveryTo_idNo  , ReceivedFrom_IdNo,Count_IdNo  ,  Yarn_Type  ,  Mill_IdNo  ,  Weight_Bag   ,   Cone_Bag  ,   Weight_Cone  , Total_Bags, Total_Cones, Total_Weight ,Total_Delivery_Weight , Total_Delivery_Bag,  User_Idno ) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text))) & ", @EntryDate, 0  , " & Str(Val(Rec_ID)) & " ," & Str(Val(YCnt_ID)) & ",'" & Trim(cbo_YarnType.Text) & "' , " & Str(Val(YMil_ID)) & ", " & Str(Val(txt_Weight_Bag.Text)) & ", " & Str(Val(txt_Cone_Bag.Text)) & "," & Str(Val(txt_Weight_Cone.Text)) & ",    " & Str(Val(vTotYrnWeight_Bag)) & ", " & Str(Val(vTotYrnCones)) & ", " & Str(Val(vTotYrnWeight)) & " , " & Str(Val(vTotYrnDWgt)) & "  ,   " & Str(Val(vTotYrnDBgs)) & " ,  " & Val(lbl_UserName.Text) & ")"
                cmd.ExecuteNonQuery()

            Else
                cmd.CommandText = "Update Weaver_Yarn_Requirement_Head set Weaver_Yarn_Requirement_Date = @EntryDate, DeliveryTo_idno = 0, ReceivedFrom_IdNo = " & Str(Val(Rec_ID)) & " , Count_IdNo = " & Str(Val(YCnt_ID)) & " ,   Yarn_Type = '" & Trim(cbo_YarnType.Text) & "' ,   Mill_IdNo = " & Str(Val(YMil_ID)) & " ,  Weight_Bag = " & Str(Val(txt_Weight_Bag.Text)) & " , Cone_Bag = " & Str(Val(txt_Cone_Bag.Text)) & " , Weight_Cone =  " & Str(Val(txt_Weight_Cone.Text)) & " ,   Total_Bags = " & Str(Val(vTotYrnWeight_Bag)) & ", Total_Cones = " & Str(Val(vTotYrnCones)) & ", Total_Weight = " & Str(Val(vTotYrnWeight)) & "  ,Total_Delivery_Weight = " & Str(Val(vTotYrnDWgt)) & "  , Total_Delivery_Bag = " & Str(Val(vTotYrnDBgs)) & ",  User_idNo = " & Val(lbl_UserName.Text) & "  Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Yarn_Requirement_Code = '" & Trim(NewCode) & "' "
                cmd.ExecuteNonQuery()

            End If

            EntID = Trim(Pk_Condition) & Trim(lbl_DcNo.Text)
            Partcls = "Req : Ref.No. " & Trim(lbl_DcNo.Text)
            PBlNo = Trim(lbl_DcNo.Text)

            cmd.CommandText = "Delete from Weaver_Yarn_Requirement_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Yarn_Requirement_Code = '" & Trim(NewCode) & "' and Delivery_Bag = 0 and Delivery_Weight = 0 and Delivery_Cone = 0"
            cmd.ExecuteNonQuery()





            With dgv_YarnDetails

                Sno = 0
                YSno = 0
                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(4).Value) <> 0 Then

                        Sno = Sno + 1

                        Delv_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, .Rows(i).Cells(1).Value, tr)
                        Nr = 0
                        cmd.CommandText = "Update Weaver_Yarn_Requirement_Details set Weaver_Yarn_Requirement_Date = @EntryDate , Sl_No = " & Str(Val(Sno)) & " ,ReceiverFrom_IdNo = " & Val(Rec_ID) & "  ,DeliveryTo_idNo = " & Val(Delv_ID) & " ,Count_IdNo = " & Val(YCnt_ID) & " ,   Mill_idno = " & Val(YMil_ID) & " ,  Yarn_Type ='" & Trim(cbo_YarnType.Text) & "' ,Bags =  " & Val(dgv_YarnDetails.Rows(i).Cells(2).Value) & ",Cones = " & Val(dgv_YarnDetails.Rows(i).Cells(3).Value) & " , Weight =" & Str(Val(dgv_YarnDetails.Rows(i).Cells(4).Value)) & "   where Company_IdNo =  " & Str(Val(lbl_Company.Tag)) & " and Weaver_Yarn_Requirement_Code = '" & Trim(NewCode) & "' and Weaver_Yarn_Requirement_Details_SlNo = " & Str(Val(dgv_YarnDetails.Rows(i).Cells(8).Value)) & ""
                        Nr = cmd.ExecuteNonQuery()

                        If Nr = 0 Then
                            cmd.CommandText = "Insert into Weaver_Yarn_Requirement_Details(Weaver_Yarn_Requirement_Code, Company_IdNo, Weaver_Yarn_Requirement_No, for_OrderBy, Weaver_Yarn_Requirement_Date,  Sl_No,  ReceiverFrom_IdNo , DeliveryTo_idNo ,Count_IdNo  ,   Mill_idno  ,  Yarn_Type , Bags, Cones, Weight ,Delivery_Bag,Delivery_Weight) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text))) & ", @EntryDate, " & Str(Val(Sno)) & ",   " & Str(Val(Rec_ID)) & ", " & Str(Val(Delv_ID)) & "," & Str(Val(YCnt_ID)) & "," & Str(Val(YMil_ID)) & ", '" & Trim(cbo_YarnType.Text) & "' ," & Str(Val(.Rows(i).Cells(2).Value)) & ", " & Str(Val(.Rows(i).Cells(3).Value)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & "," & Str(Val(.Rows(i).Cells(5).Value)) & "," & Str(Val(.Rows(i).Cells(6).Value)) & ")"
                            cmd.ExecuteNonQuery()
                        End If

                    End If

                Next

            End With


            'Get_YarnStock(con, yarn_Stock, tr)
            'If Val(yarn_Stock) < 0 Then
            '    ' MessageBox.Show("No Stock To Delivery", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            '    Throw New ApplicationException("Negative Yarn Stock : " & yarn_Stock)
            'End If


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

        Finally
            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()

        End Try

    End Sub

    Private Sub cbo_DelvTo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_DelvTo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'WEAVER' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")

    End Sub

    Private Sub cbo_Ledger_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DelvTo.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_DelvTo, Nothing, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'WEAVER' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")

        With dgv_YarnDetails

            If (e.KeyValue = 38 And cbo_DelvTo.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                If .CurrentCell.RowIndex = 0 Then
                    txt_Weight_Cone.Focus()
                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentRow.Index - 1).Cells(.ColumnCount - 5)
                End If
            End If
            If (e.KeyValue = 40 And cbo_DelvTo.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                If .CurrentCell.RowIndex = .Rows.Count - 1 And Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(1).Value) = "" Then
                    If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                        save_record()
                    Else
                        msk_date.Focus()
                    End If
                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                End If
            End If

        End With
    End Sub

    Private Sub cbo_Ledger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_DelvTo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, Nothing, cbo_RecFrom, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'WEAVER' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
        If Asc(e.KeyChar) = 13 Then

            With dgv_YarnDetails
                If .CurrentCell.RowIndex = .Rows.Count - 1 And Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(1).Value) = "" Then
                    If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                        save_record()
                    Else
                        msk_date.Focus()
                    End If
                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                End If
            End With
        End If
    End Sub

    Private Sub cbo_Ledger_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DelvTo.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            ' Common_Procedures.MDI_LedType = ""
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_DelvTo.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_RecFrom_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_RecFrom.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'GODOWN' or Ledger_Type = 'SIZING' or Ledger_Type = 'REWINDING' or Ledger_Type = 'JOBWORKER' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1) and Close_status = 0 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Rec_Ledger_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_RecFrom.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_RecFrom, msk_date, cbo_CountName, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'GODOWN' or Ledger_Type = 'SIZING' or Ledger_Type = 'REWINDING' or Ledger_Type = 'JOBWORKER' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1) and Close_status = 0 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Rec_Ledger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_RecFrom.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_RecFrom, cbo_CountName, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'GODOWN' or Ledger_Type = 'SIZING' or Ledger_Type = 'REWINDING' or Ledger_Type = 'JOBWORKER' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1) and Close_status = 0 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Rec_Ledger_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_RecFrom.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_RecFrom.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub


    Private Sub dgv_YarnDetails_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_YarnDetails.CellEndEdit
        TotalYarnTaken_Calculation()
        'If dgv_YarnDetails.CurrentRow.Cells(2).Value = "MILL" Then
        '    If e.ColumnIndex = 4 Or e.ColumnIndex = 5 Then
        '        get_MillCount_Details()
        '    End If
        'End If

    End Sub

    Private Sub dgv_YarnDetails_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_YarnDetails.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim rect As Rectangle

        With dgv_YarnDetails
            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If


            If e.ColumnIndex = 1 Then

                If cbo_DelvTo.Visible = False Or Val(cbo_DelvTo.Tag) <> e.RowIndex Then

                    cbo_DelvTo.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead WHERE (Ledger_IdNo = 0 or Ledger_Type = 'WEAVER') and Close_status = 0  order by Ledger_DisplayName", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_DelvTo.DataSource = Dt1
                    cbo_DelvTo.DisplayMember = "Ledger_DisplayName"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_DelvTo.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_DelvTo.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top

                    cbo_DelvTo.Width = rect.Width  ' .CurrentCell.Size.Width
                    cbo_DelvTo.Height = rect.Height  ' rect.Height
                    cbo_DelvTo.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_DelvTo.Tag = Val(e.RowIndex)
                    cbo_DelvTo.Visible = True

                    cbo_DelvTo.BringToFront()
                    cbo_DelvTo.Focus()


                End If


            Else

                cbo_DelvTo.Visible = False

            End If


        End With
    End Sub

    Private Sub dgv_YarnDetails_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_YarnDetails.CellLeave
        With dgv_YarnDetails

            If .CurrentCell.ColumnIndex = 4 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.000")
                End If
            End If

            If .CurrentCell.ColumnIndex = 5 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                End If
            End If
        End With
    End Sub

    Private Sub dgv_YarnDetails_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_YarnDetails.CellValueChanged
        On Error Resume Next
        With dgv_YarnDetails
            If .Visible Then

                If e.ColumnIndex = 2 Or e.ColumnIndex = 3 Or e.ColumnIndex = 4 Or e.ColumnIndex = 5 Or e.ColumnIndex = 6 Then
                    TotalYarnTaken_Calculation()
                End If
                If e.ColumnIndex = 2 Then
                    If Val(.Rows(.CurrentCell.RowIndex).Cells(3).Value) = 0 Then
                        .Rows(.CurrentCell.RowIndex).Cells(3).Value = ConesPer_Bag * Val(.Rows(.CurrentCell.RowIndex).Cells(2).Value)
                    End If
                    If Val(.Rows(.CurrentCell.RowIndex).Cells(4).Value) = 0 Then
                        .Rows(.CurrentCell.RowIndex).Cells(4).Value = Format(WgtPer_Bag * Val(.Rows(.CurrentCell.RowIndex).Cells(2).Value), "######0.00")
                    End If
                End If
            End If
        End With
    End Sub

    Private Sub dgv_YarnDetails_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_YarnDetails.EditingControlShowing
        dgtxt_Details = CType(dgv_YarnDetails.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgv_YarnDetails_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_YarnDetails.KeyDown




    End Sub

    Private Sub dgv_YarnDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_YarnDetails.KeyUp




        Dim i As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_YarnDetails

                .Rows.RemoveAt(.CurrentRow.Index)

                For i = 0 To .Rows.Count - 1
                    .Rows(i).Cells(0).Value = i + 1
                Next

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

        End With
    End Sub


    Private Sub get_MillCount_Details()
        Dim q As Single = 0
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim Cn_bag As Single
        Dim Wgt_Bag As Single
        Dim Wgt_Cn As Single
        Dim CntID As Integer
        Dim MilID As Integer

        CntID = Common_Procedures.Count_NameToIdNo(con, cbo_CountName.Text)
        MilID = Common_Procedures.Mill_NameToIdNo(con, cbo_MillName.Text)


        Wgt_Bag = 0 : Wgt_Cn = 0 : Cn_bag = 0

        If CntID <> 0 And MilID <> 0 Then

            Da = New SqlClient.SqlDataAdapter("select * from Mill_Count_Details where mill_idno = " & Str(Val(MilID)) & " and count_idno = " & Str(Val(CntID)), con)
            Da.Fill(Dt)
            With dgv_YarnDetails

                If Dt.Rows.Count > 0 Then
                    If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                        txt_Weight_Bag.Text = Dt.Rows(0).Item("Weight_Bag").ToString
                        txt_Weight_Cone.Text = Dt.Rows(0).Item("Weight_Cone").ToString
                        txt_Cone_Bag.Text = Dt.Rows(0).Item("Cones_Bag").ToString
                    End If
                End If

                Dt.Clear()
                Dt.Dispose()
                Da.Dispose()

                'If Val(txt_Bag.Text) <> 0 Or Val(txt_Cone.Text) <> 0 Then
                '    If Val(txt_Bag.Text) <> 0 Then
                '        If Val(Cn_bag) <> 0 Then
                '            txt_Cone.Text = Val(txt_Bag.Text) * Val(Cn_bag)
                '        End If

                '        If Val(Wgt_Bag) <> 0 Then
                '            txt_Weight.Text = Format(Val(txt_Bag.Text) * Val(Wgt_Bag), "#########0.000")
                '        End If

                '    Else
                '        txt_Cone.Text = 0
                '        txt_Weight.Text = 0

                '    End If

                'If Val(txt_Cone.Text) <> 0 Then
                '    If Val(Wgt_Cn) <> 0 Then
                '        txt_Weight.Text = Format(Val(txt_Cone.Text) * Val(Wgt_Cn), "##########0.000")
                '    End If

                'End If

                'End If

            End With

        End If

    End Sub

    Private Sub Get_YarnStock(ByVal con As SqlClient.SqlConnection, ByRef weight As Integer, Optional ByVal tr As SqlClient.SqlTransaction = Nothing)

        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim Nr As Long = 0
        Dim RptCondt As String = ""
        Dim Wgt As Double = 0
        Dim Rec_Wgt As Double = 0
        Dim Dc_Wgt As Double = 0

        cmd.Connection = con
        If IsNothing(tr) = False Then
            cmd.Transaction = tr
        End If

        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@uptodate", dtp_Date.Value.Date)

        If cbo_CountName.Visible = True And Trim(cbo_CountName.Text) <> "" Then
            RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Count_IdNo =  " & Str(Val(Common_Procedures.Count_NameToIdNo(con, cbo_CountName.Text, tr)))
        End If

        If cbo_MillName.Visible = True And Trim(cbo_MillName.Text) <> "" Then
            RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Mill_IdNo =  " & Str(Val(Common_Procedures.Mill_NameToIdNo(con, cbo_MillName.Text, tr)))
        End If

        cmd.CommandText = "Truncate table ReportTempSub"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Insert into ReportTempSub(Int1, name1, name2, name3, Int2, Int3, weight1, weight2, weight3) Select a.DeliveryTo_Idno, tP.Ledger_Name, c.count_name, d.mill_name, sum(a.Weight_Bag), sum(a.cones), (case when a.yarn_type <> 'R/W' and a.yarn_type <> 'BABY' then sum(a.Weight) else 0 end), (case when a.yarn_type = 'R/W' then sum(a.Weight) else 0 end), (case when a.yarn_type = 'BABY' then sum(a.Weight) else 0 end) from Stock_Yarn_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON a.DeliveryTo_Idno <> 0 and a.DeliveryTo_Idno = tP.Ledger_IdNo INNER JOIN Count_Head c ON a.Count_IdNo <> 0 and a.Count_IdNo = c.Count_IdNo LEFT OUTER JOIN Mill_Head d ON a.Mill_IdNo <> 0 and a.Mill_IdNo = d.Mill_IdNo Where " & RptCondt & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Reference_Date <= @uptodate and a.Weight <> 0 group by a.DeliveryTo_Idno, tP.Ledger_Name, c.count_name, d.mill_name, a.yarn_type having sum(a.Weight) <> 0"
        Nr = cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into ReportTempSub(Int1, name1, name2, name3, Int2, Int3, weight1, weight2, weight3) Select a.ReceivedFrom_Idno, tP.Ledger_Name, c.count_name, d.mill_name, -1*sum(a.Weight_Bag), -1*sum(a.cones), (case when a.yarn_type <> 'R/W' and a.yarn_type <> 'BABY' then -1*sum(a.Weight) else 0 end), (case when a.yarn_type = 'R/W' then -1*sum(a.Weight) else 0 end), (case when a.yarn_type = 'BABY' then -1*sum(a.Weight) else 0 end)  from Stock_Yarn_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON a.ReceivedFrom_Idno <> 0 and a.ReceivedFrom_Idno = tP.Ledger_IdNo INNER JOIN Count_Head c ON a.Count_IdNo <> 0 and a.Count_IdNo = c.Count_IdNo LEFT OUTER JOIN Mill_Head d ON a.Mill_IdNo <> 0 and a.Mill_IdNo = d.Mill_IdNo Where " & RptCondt & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Reference_Date <= @uptodate and a.Weight <> 0 group by a.ReceivedFrom_Idno, tP.Ledger_Name, c.count_name, d.mill_name, a.yarn_type having sum(a.Weight) <> 0"
        Nr = cmd.ExecuteNonQuery()

        cmd.CommandText = "Truncate table ReportTemp"
        Nr = cmd.ExecuteNonQuery()

        ' cmd.CommandText = "Insert into reporttemp(Int1, name1, name2, name3, Int2, Int3, weight1, weight2, weight3) Select Int1, name1, name2, name3, sum(Int2), sum(Int3), sum(weight1), sum(weight2), sum(weight3) from ReportTempSub group by Int1, name1, name2, name3 having sum(Weight1) <> 0 or sum(Weight2) <> 0 or sum(Weight3) <> 0"
        ' Nr = cmd.ExecuteNonQuery()

        cmd.CommandText = "Select Int1, name1, name2, name3, sum(Int2), sum(Int3), sum(weight1) as Rec_Weight , sum(weight2) as Dc_Weight , sum(weight3) as Yrn_Weight from ReportTempSub group by Int1, name1, name2, name3 having sum(Weight1) <> 0 or sum(Weight2) <> 0 or sum(Weight3) <> 0"
        Da = New SqlClient.SqlDataAdapter(cmd)
        Dt = New DataTable
        Da.Fill(Dt)
        Wgt = 0
        If Dt.Rows.Count > 0 Then
            Dc_Wgt = Val(Dt.Rows(0).Item("Rec_Weight"))
            Rec_Wgt = Val(Dt.Rows(0).Item("Dc_Weight"))
        End If

        Wgt = Dc_Wgt - Rec_Wgt

        Dt.Clear()

        weight = Wgt

        'MessageBox.Show("Invalid Cloth Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        'Exit Sub

    End Sub

    Private Sub TotalYarnTaken_Calculation()
        Dim Sno As Integer
        Dim TotWeight_Bag As Single, TotCones As Single, TotWeight As Single, TotDWgt As Single, TotDBgs


        Sno = 0
        TotWeight_Bag = 0
        TotCones = 0
        TotWeight = 0
        TotDBgs = 0
        TotDWgt = 0
        With dgv_YarnDetails
            For i = 0 To .RowCount - 1
                Sno = Sno + 1
                .Rows(i).Cells(0).Value = Sno
                If Val(.Rows(i).Cells(4).Value) <> 0 Then
                    TotWeight_Bag = TotWeight_Bag + Val(.Rows(i).Cells(2).Value)
                    TotCones = TotCones + Val(.Rows(i).Cells(3).Value)
                    TotWeight = TotWeight + Val(.Rows(i).Cells(4).Value)
                    TotDBgs = TotDBgs + Val(.Rows(i).Cells(5).Value)
                    TotDWgt = TotDWgt + Val(.Rows(i).Cells(6).Value)

                End If
            Next
        End With

        With dgv_YarnDetails_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(2).Value = Val(TotWeight_Bag)
            .Rows(0).Cells(3).Value = Val(TotCones)
            .Rows(0).Cells(4).Value = Format(Val(TotWeight), "########0.000")
            .Rows(0).Cells(5).Value = Val(TotDBgs)
            .Rows(0).Cells(6).Value = Format(Val(TotDWgt), "########0.000")

        End With

    End Sub

    Private Sub cbo_Grid_YarnType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_YarnType.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "YarnType_Head", "Yarn_Type", "", "Yarn_Type")

    End Sub

    Private Sub cbo_Grid_YarnType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_YarnType.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_YarnType, cbo_CountName, cbo_MillName, "YarnType_Head", "Yarn_Type", "", "Yarn_Type")



    End Sub

    Private Sub cbo_Grid_YarnType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_YarnType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_YarnType, cbo_MillName, "YarnType_Head", "Yarn_Type", "", "Yarn_Type")

    End Sub


    Private Sub cbo_Grid_MillName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_MillName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")

    End Sub
    Private Sub cbo_Grid_MillName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_MillName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_MillName, cbo_YarnType, txt_Weight_Bag, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")




    End Sub

    Private Sub cbo_Grid_MillName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_MillName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_MillName, txt_Weight_Bag, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")

    End Sub

    Private Sub cbo_Grid_MillName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_MillName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Mill_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_MillName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_Grid_CountName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_CountName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

    End Sub
    Private Sub cbo_Grid_CountName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_CountName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_CountName, cbo_RecFrom, cbo_YarnType, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")



    End Sub

    Private Sub cbo_Grid_CountName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_CountName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_CountName, Nothing, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to select Delivery  :", "FOR WEAVER YARN DELIVERY SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                btn_Selection_Click(sender, e)
            Else
                cbo_YarnType.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_Grid_CountName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_CountName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Count_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_CountName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub


    Private Sub btn_close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub btn_save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub
    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.Enter
        dgv_YarnDetails.EditingControl.BackColor = Color.Lime
        dgv_YarnDetails.EditingControl.ForeColor = Color.Blue
        dgtxt_Details.SelectAll()
    End Sub

    Private Sub dgtxt_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyDown
        Try

            With dgv_YarnDetails

                If e.KeyValue = Keys.Delete Then
                    If Val(.Rows(.CurrentCell.RowIndex).Cells(5).Value) <> 0 Or Val(.Rows(.CurrentCell.RowIndex).Cells(6).Value) <> 0 Or Val(.Rows(.CurrentCell.RowIndex).Cells(7).Value) <> 0 Then
                        e.Handled = True
                    End If

                End If
            End With

        Catch ex As Exception

        End Try
    End Sub

    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Details.KeyPress
        With dgv_YarnDetails
            If .Visible Then
                If Val(.Rows(.CurrentCell.RowIndex).Cells(5).Value) <> 0 Or Val(.Rows(.CurrentCell.RowIndex).Cells(6).Value) <> 0 Or Val(.Rows(.CurrentCell.RowIndex).Cells(7).Value) <> 0 Then
                    e.Handled = True

                Else
                    If .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 4 Then

                        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                            e.Handled = True
                        End If

                    End If
                End If
            End If
        End With
    End Sub

    Private Sub txt_Empty_Beam_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Weight_Bag.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub
    Private Sub btn_Filter_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter_Close.Click
        Pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        Filter_Status = False
    End Sub
    Private Sub btn_Filter_Show_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter_Show.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim n As Integer
        Dim Led_IdNo As Integer, Del_IdNo As Integer, Cnt_IdNo As Integer, Mil_IdNo As Integer
        Dim Condt As String = ""

        Try

            Condt = ""
            Del_IdNo = 0
            Cnt_IdNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Weaver_Yarn_Requirement_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Weaver_Yarn_Requirement_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Weaver_Yarn_Requirement_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
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

            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " (a.DeliveryTo_IdNo = " & Str(Val(Led_IdNo)) & ")"
            End If
            If Val(Cnt_IdNo) <> 0 Then

                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " A.Count_IdNo = " & Str(Val(Cnt_IdNo)) & ""
            End If

            If Val(Mil_IdNo) <> 0 Then

                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " A.Mill_IdNo = " & Str(Val(Mil_IdNo))
            End If

            da = New SqlClient.SqlDataAdapter("select a.*, e.Ledger_Name from Weaver_Yarn_Requirement_Head a inner join Ledger_head e on a.DeliveryTo_IdNo = e.Ledger_idno where a.company_idno =" & Str(Val(lbl_Company.Tag)) & "and a.Weaver_Yarn_Requirement_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.Weaver_Yarn_Requirement_Date, a.for_orderby, a.Weaver_Yarn_Requirement_No", con)
            'da = New SqlClient.SqlDataAdapter("select a.*, e.Ledger_Name from Weaver_Yarn_Requirement_Head a left outer join Weaver_Yarn_Requirement_Details b on a.Weaver_Yarn_Requirement_Code = b.Weaver_Yarn_Requirement_Code left outer join Ledger_head e on a.Ledger_idno = e.Ledger_idno where a.company_idno =" & Str(Val(lbl_Company.Tag)) & "and a.Weaver_Yarn_Requirement_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.Weaver_Yarn_Requirement_Date, a.for_orderby, a.Weaver_Yarn_Requirement_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Weaver_Yarn_Requirement_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Weaver_Yarn_Requirement_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = Val(dt2.Rows(i).Item("Total_Weight_Bag").ToString)
                    dgv_Filter_Details.Rows(n).Cells(5).Value = Val(dt2.Rows(i).Item("Total_Cones").ToString)
                    dgv_Filter_Details.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Total_Weight").ToString), "########0.000")

                Next i

            End If

            dt2.Clear()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FILTER...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            dt2.Dispose()
            da.Dispose()

            If dgv_Filter_Details.Visible And dgv_Filter_Details.Enabled Then dgv_Filter_Details.Focus()

        End Try

    End Sub


    Private Sub dtp_Filter_Fromdate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_Filter_Fromdate.KeyPress
        If Asc(e.KeyChar) = 13 Then
            dtp_Filter_ToDate.Focus()
        End If

    End Sub

    Private Sub dtp_Filter_ToDate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_Filter_ToDate.KeyPress
        If Asc(e.KeyChar) = 13 Then
            cbo_Filter_PartyName.Focus()
        End If
    End Sub

    Private Sub cbo_Filter_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'WEAVER' or Ledger_Type = 'SIZING' or Ledger_Type = 'GODOWN' or Ledger_Type = 'JOBWORKER' or Ledger_Type = 'REWINDING' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, dtp_Filter_ToDate, cbo_Filter_CountName, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'WEAVER' or Ledger_Type = 'SIZING' or Ledger_Type = 'GODOWN' or Ledger_Type = 'JOBWORKER' or Ledger_Type = 'REWINDING' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, cbo_Filter_CountName, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'WEAVER' or Ledger_Type = 'SIZING' or Ledger_Type = 'GODOWN' or Ledger_Type = 'JOBWORKER' or Ledger_Type = 'REWINDING' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_CountName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_CountName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

    End Sub
    Private Sub cbo_Filter_CountName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_CountName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_CountName, cbo_Filter_PartyName, cbo_Filter_MillName, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

    End Sub

    Private Sub cbo_Filter_CountName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_CountName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_CountName, cbo_Filter_MillName, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

    End Sub

    Private Sub Open_FilterEntry()
        Dim movno As String

        On Error Resume Next

        movno = Trim(dgv_Filter_Details.CurrentRow.Cells(1).Value)

        If Val(movno) <> 0 Then
            Filter_Status = True
            move_record(movno)
            Pnl_Back.Enabled = True
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
    Private Sub cbo_Filter_MillName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_MillName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")

    End Sub
    Private Sub cbo_Filter_MillName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_MillName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_MillName, cbo_Filter_CountName, btn_Filter_Show, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_MillName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_MillName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_MillName, btn_Filter_Show, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")
    End Sub

    Private Sub btn_Print_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        print_record()
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String
        Dim PpSzSTS As Boolean = False
        Dim I As Integer = 0
        Dim ps As Printing.PaperSize

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select * from Weaver_Yarn_Requirement_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Yarn_Requirement_Code = '" & Trim(NewCode) & "'", con)
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

        prn_TotCopies = 1
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1037" Then '---- Prakash Sizing (Somanur)
            prn_TotCopies = Val(InputBox("Enter No.of Copies", "FOR DELIVERY PRINTING...", "2"))
            If Val(prn_TotCopies) <= 0 Then
                Exit Sub
            End If
        End If

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


        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then

            Try
                If Val(Common_Procedures.settings.Printing_Show_PrintDialogue) = 1 Then
                    PrintDialog1.PrinterSettings = PrintDocument1.PrinterSettings
                    If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                        PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings

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

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0
        prn_Count = 0

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.*, d.Ledger_MainName as Ledger1_Name ,d.Ledger_Address1 as RecAdd1,d.Ledger_Address2 as RecAdd2,d.Ledger_Address3 as RecAdd3,d.Ledger_Address4 as RecAdd4,d.Area_Idno as RecArea, e.Transport_Name  from Weaver_Yarn_Requirement_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo INNER JOIN Ledger_Head c ON a.DeliveryTo_IdNo = c.Ledger_IdNo Left Outer JOIN Ledger_Head d ON a.ReceivedFrom_IdNo = d.Ledger_IdNo Left Outer JOIN Transport_Head e ON a.Transport_IdNo = e.Transport_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_Yarn_Requirement_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Mill_name, d.Count_name  from Weaver_Yarn_Requirement_Details a INNER JOIN Mill_Head b ON a.Mill_idno = b.Mill_idno LEFT OUTER JOIN Count_Head d ON a.Count_idno = d.Count_idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_Yarn_Requirement_Code = '" & Trim(NewCode) & "' Order by a.Sl_No", con)
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
                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
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
                    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
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
                        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                        PrintDocument1.DefaultPageSettings.PaperSize = ps
                        e.PageSettings.PaperSize = ps
                        Exit For
                    End If
                Next
            End If

        End If

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 50
            .Right = 55
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

        NoofItems_PerPage = 6

        Erase LnAr
        Erase ClAr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClAr(1) = 45 : ClAr(2) = 350 : ClAr(3) = 100 : ClAr(4) = 100
        ClAr(5) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4))

        TxtHgt = 18.5

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try
            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format1_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr)

                NoofDets = 0

                CurY = CurY - 10

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then

                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued...", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 10, CurY, 0, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, False)

                            e.HasMorePages = True
                            Return

                        End If

                        ItmNm1 = Common_Procedures.Ledger_IdNoToName(con, Val(prn_DetDt.Rows(prn_DetIndx).Item("DeliveryTo_IdNo").ToString))
                        ItmNm2 = ""
                        If Len(ItmNm1) > 35 Then
                            For I = 15 To 1 Step -1
                                If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 35
                            ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                            ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                        End If

                        CurY = CurY + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Sl_No").ToString), LMargin + 15, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Cones").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Weight").ToString), " #######0.000"), PageWidth - 10, CurY, 1, 0, pFont)

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

        prn_Count = prn_Count + 1

        e.HasMorePages = False

        If Val(prn_TotCopies) > 1 Then
            If prn_Count < Val(prn_TotCopies) Then

                prn_DetIndx = 0
                prn_DetSNo = 0
                prn_PageNo = 0

                e.HasMorePages = True
                Return

            End If

        End If


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

        da2 = New SqlClient.SqlDataAdapter("select a.* from Weaver_Yarn_Requirement_Details a where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_Yarn_Requirement_Code = '" & Trim(EntryCode) & "' Order by a.Sl_No", con)
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
        Common_Procedures.Print_To_PrintDocument(e, "YARN DELIVERY", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        C1 = ClAr(1) + ClAr(2) + ClAr(3)
        W1 = e.Graphics.MeasureString("D.C DATE  : ", pFont).Width
        S1 = e.Graphics.MeasureString("TO  :  ", pFont).Width


        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "TO  :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_MainName").ToString, LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "DC.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Weaver_Yarn_Requirement_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        p1Font = New Font("Calibri", 14, FontStyle.Bold)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Weaver_Yarn_Requirement_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        CurY = CurY + TxtHgt - 5

        Common_Procedures.Print_To_PrintDocument(e, "COUNT", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Count_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("cOUNT_IDnO").ToString)), LMargin + W1 + 30, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "TYPE", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("YARN_TYPE").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt

        Common_Procedures.Print_To_PrintDocument(e, "MILL NAME", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Mill_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("Mill_IDnO").ToString)), LMargin + W1 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))

        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "DELIVERY TO", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "BAG", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "CONE", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)

        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY


    End Sub

    Private Sub Printing_Format1_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim i As Integer
        Dim Cmp_Name As String
        Dim p1Font As Font
        Dim W1 As Single

        For i = NoofDets + 1 To NoofItems_PerPage
            CurY = CurY + TxtHgt
        Next

        W1 = e.Graphics.MeasureString(" Vehicle No :", pFont).Width

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(5) = CurY

        CurY = CurY + TxtHgt - 10
        If is_LastPage = True Then
            Common_Procedures.Print_To_PrintDocument(e, " TOTAL", LMargin + ClAr(1) + 15, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Bags").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Cones").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString), " #######0.000"), PageWidth - 10, CurY, 1, 0, pFont)

        End If

        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(6) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
        'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
        'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))

        CurY = CurY + 10

        Common_Procedures.Print_To_PrintDocument(e, " Vehicle No : ", LMargin + 20, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vechile_No").ToString, LMargin + W1 + 30, CurY, 0, 0, pFont)
        'If Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString) <> 0 Then
        '    Common_Procedures.Print_To_PrintDocument(e, "Empty Beam : " & Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString), PageWidth - 20, CurY, 1, 0, pFont)
        'End If
        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

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
    'With Thiri
    Private Sub Printing_Format2(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
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

        NoofItems_PerPage = 6

        Erase LnAr
        Erase ClAr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClAr(1) = Val(40) : ClAr(2) = 100 : ClAr(3) = 210 : ClAr(4) = 75 : ClAr(5) = 75 : ClAr(6) = 75 : ClAr(7) = 90
        ClAr(8) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7))

        TxtHgt = 18.5

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try
            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format2_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr)

                NoofDets = 0

                CurY = CurY - 10

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then

                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued...", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 10, CurY, 0, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, False)

                            e.HasMorePages = True
                            Return

                        End If

                        ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Mill_Name").ToString)
                        ItmNm2 = ""
                        If Len(ItmNm1) > 35 Then
                            For I = 15 To 1 Step -1
                                If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 35
                            ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                            ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                        End If

                        CurY = CurY + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Sl_No").ToString), LMargin + 15, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Yarn_Type").ToString), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Count_Name").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Weight_Bag").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Cones").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Weight").ToString), " #######0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Thiri").ToString), " #######0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)

                        NoofDets = NoofDets + 1

                        If Trim(ItmNm2) <> "" Then
                            CurY = CurY + TxtHgt - 5
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                            NoofDets = NoofDets + 1
                        End If

                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If

                Printing_Format2_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format2_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
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

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Mill_name, d.Count_name  from Weaver_Yarn_Requirement_Details a INNER JOIN Mill_Head b ON a.Mill_idno = b.Mill_idno LEFT OUTER JOIN Count_Head d ON a.Count_idno = d.Count_idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_Yarn_Requirement_Code = '" & Trim(EntryCode) & "' Order by a.Sl_No", con)
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
        Common_Procedures.Print_To_PrintDocument(e, "YARN DELIVERY", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        C1 = ClAr(1) + ClAr(2) + ClAr(3)
        W1 = e.Graphics.MeasureString("D.C DATE  : ", pFont).Width
        S1 = e.Graphics.MeasureString("TO  :  ", pFont).Width


        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "TO  :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_MainName").ToString, LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "DC.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Weaver_Yarn_Requirement_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        p1Font = New Font("Calibri", 14, FontStyle.Bold)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Weaver_Yarn_Requirement_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))

        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "YARN TYPE", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "MILL NAME", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "COUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "BAG", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "CONE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "THIRI", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY


    End Sub

    Private Sub Printing_Format2_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim i As Integer
        Dim Cmp_Name As String
        Dim p1Font As Font
        Dim W1 As Single

        For i = NoofDets + 1 To NoofItems_PerPage
            CurY = CurY + TxtHgt
        Next

        W1 = e.Graphics.MeasureString(" Vehicle No :", pFont).Width

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(5) = CurY

        CurY = CurY + TxtHgt - 10
        If is_LastPage = True Then
            Common_Procedures.Print_To_PrintDocument(e, " TOTAL", LMargin + ClAr(1) + 30, CurY, 2, ClAr(4), pFont)

            Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Weight_Bag").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Cones").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString), " #######0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Thiri").ToString), " #######0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)

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

        CurY = CurY + 10

        Common_Procedures.Print_To_PrintDocument(e, " Vehicle No : ", LMargin + 20, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vechile_No").ToString, LMargin + W1 + 30, CurY, 0, 0, pFont)
        If Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString) <> 0 Then
            Common_Procedures.Print_To_PrintDocument(e, "Empty Beam : " & Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString), PageWidth - 20, CurY, 1, 0, pFont)
        End If

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

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
    Private Sub dgtxt_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyUp
        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            dgv_YarnDetails_KeyUp(sender, e)
        End If
    End Sub
    Private Sub msk_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_date.KeyDown

        vcbo_KeyDwnVal = e.KeyValue
        If e.KeyCode = 40 Then
            e.Handled = True : e.SuppressKeyPress = True
            cbo_RecFrom.Focus()
        End If

        If e.KeyCode = 38 Then
            e.Handled = True : e.SuppressKeyPress = True
            txt_Weight_Cone.Focus()
        End If

        vmskOldText = ""
        vmskSelStrt = -1
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            vmskOldText = msk_date.Text
            vmskSelStrt = msk_date.SelectionStart
        End If

    End Sub

    Private Sub msk_date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles msk_date.KeyPress
        If Trim(UCase(e.KeyChar)) = "D" Then
            msk_date.Text = Date.Today
            msk_date.SelectionStart = 0
        End If
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            cbo_RecFrom.Focus()
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


    Private Sub txt_Weight_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Weight_Cone.KeyDown
        If e.KeyCode = 38 Then
            txt_Cone_Bag.Focus()
        End If
        If e.KeyCode = 40 Then
            If dgv_YarnDetails.Rows.Count > 0 Then


                dgv_YarnDetails.Focus()
                dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(1)
                dgv_YarnDetails.CurrentCell.Selected = True
            End If
        End If
    End Sub



    Private Sub txt_Weight_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Weight_Cone.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
        If Asc(e.KeyChar) = 13 Then
            If dgv_YarnDetails.Rows.Count > 0 Then


                dgv_YarnDetails.Focus()
                dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(1)
                dgv_YarnDetails.CurrentCell.Selected = True
            End If


        End If
    End Sub

    Private Sub txt_Cone_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Cone_Bag.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub


    Private Sub cbo_DelvTo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_DelvTo.TextChanged
        Try
            If cbo_DelvTo.Visible Then
                With dgv_YarnDetails
                    If Val(cbo_DelvTo.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_DelvTo.Text)
                    End If
                End With
            End If

        Catch ex As Exception


        End Try
    End Sub


    Private Sub Get_WeightPer_Bag()
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Mill_Id As Integer = 0
        Dim Count_Id As Integer = 0

        Try
            ConesPer_Bag = 0 : WgtPer_Bag = 0

            Mill_Id = Common_Procedures.Mill_NameToIdNo(con, Trim(cbo_MillName.Text))
            Count_Id = Common_Procedures.Count_NameToIdNo(con, Trim(cbo_CountName.Text))

            da = New SqlClient.SqlDataAdapter("select Cones_Bag ,Weight_Bag  from Mill_Count_Details where Mill_IdNo = " & Val(Mill_Id) & " and  Count_IdNo  = " & Val(Count_Id) & " ", con)
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0).Item("Cones_Bag").ToString) = False Then
                    ConesPer_Bag = Val(dt.Rows(0).Item("Cones_Bag").ToString)
                End If
                If IsDBNull(dt.Rows(0).Item("Weight_Bag").ToString) = False Then
                    WgtPer_Bag = Val(dt.Rows(0).Item("Weight_Bag").ToString)
                End If
            End If

            dt.Clear()
            dt.Dispose()
            da.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_CountName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_CountName.TextChanged
        Get_WeightPer_Bag()
    End Sub

    Private Sub cbo_MillName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_MillName.TextChanged
        Get_WeightPer_Bag()
    End Sub


    Private Sub dgtxt_Details_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.TextChanged
        Try
            With dgv_YarnDetails

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

    Private Sub txt_Bag_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Weight_Bag.LostFocus
        get_MillCount_Details()
    End Sub

    Private Sub txt_Bag_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Weight_Bag.TextChanged
        get_MillCount_Details()
    End Sub

    Private Sub txt_value_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                dtp_Date.Focus()
            End If
        End If
    End Sub
    Private Sub Yarn_Stock_Checking()
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim Cnt_IdNo As Integer = 0
        Dim MILL_IdNo As Integer = 0
        Dim CONT As String = ""
        Dim n As Integer, Sno As Integer

        Dim NewCode As String = ""

       

        Cnt_IdNo = Val(Common_Procedures.Count_NameToIdNo(con, Trim(cbo_CountName.Text)))
        'MILL_IdNo = Val(Common_Procedures.Mill_NameToIdNo(con, Trim(cbo_Grid_MillName.Text)))

        If Cnt_IdNo = 0 Then
            dgv_YarnStock.Rows.Clear()
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        pnl_Count_Stock.Visible = True

        Try

            cmd.Connection = con

            cmd.CommandText = "Truncate table ReportTempSub"
            cmd.ExecuteNonQuery()

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@invdate", dtp_Date.Value.Date)


            cmd.CommandText = "Insert into ReportTempSub(int1, int2, weight1 ,Name1) Select sum(a.Bags), sum(a.Cones), sum(a.Weight) , d.Mill_Name from Stock_Yarn_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo   LEFT OUTER JOIN Mill_Head d ON a.Mill_IdNo = d.Mill_IdNo  Where a.Company_IdNo = " & Val(lbl_Company.Tag) & "  and  a.Reference_Date <= @invdate and a.Weight <> 0 and a.Count_IdNo = " & Val(Cnt_IdNo) & " and a.DeliveryTo_Idno <> 0  " & IIf(New_Entry = False, " and a.Reference_Code <> '" & Trim(NewCode) & "'", "") & "  group by d.Mill_Name "
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Insert into ReportTempSub(int1, int2, weight1,Name1) Select -1*sum(a.Bags), -1*sum(a.Cones), -1*sum(a.Weight) , d.Mill_Name  from Stock_Yarn_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo  LEFT OUTER JOIN Mill_Head d ON a.Mill_IdNo <> 0 and a.Mill_IdNo = d.Mill_IdNo Where a.Company_IdNo = " & Val(lbl_Company.Tag) & "  and a.Reference_Date <= @invdate and a.Weight <> 0 and a.Count_IdNo = " & Val(Cnt_IdNo) & "  and a.ReceivedFrom_Idno <> 0 " & IIf(New_Entry = False, " and a.Reference_Code <> '" & Trim(NewCode) & "'", "") & " Group by d.Mill_Name "
            cmd.ExecuteNonQuery()

            da = New SqlClient.SqlDataAdapter("select sum(Int1) as Total_Bag ,Sum(int2) as Total_Cone, Sum(weight1)  as Total_Weight , Name1 as Mill_Name from ReportTempSub group by name1 having Sum(weight1)  <> 0 ", con)
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
                        .Rows(n).Cells(2).Value = dt.Rows(n).Item("Total_Bag").ToString
                        .Rows(n).Cells(3).Value = dt.Rows(n).Item("Total_Cone").ToString
                        .Rows(n).Cells(4).Value = dt.Rows(n).Item("Total_Weight").ToString

                    Next i

                End If

            End With
            dt.Clear()

            If cbo_YarnType.Enabled And cbo_YarnType.Visible Then cbo_YarnType.Focus()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            dt.Dispose()
            da.Dispose()

        End Try

    End Sub
    
    Private Sub btn_Selection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Selection.Click
        Yarn_Stock_Checking()
    End Sub
End Class