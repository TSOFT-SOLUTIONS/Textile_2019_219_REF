Public Class Ledger_Sizing_Rate_Details

    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Pk_Condition As String = "LEDRT-"
    Private OpYrCode As String = ""
    Private vcbo_KeyDwnVal As Double
    Private Prec_ActCtrl As New Control
    Private ClrSTS As Boolean = False
    Private WithEvents dgtxt_YarnDetails As New DataGridViewTextBoxEditingControl
    Private WithEvents dgtxt_PavuDetails As New DataGridViewTextBoxEditingControl
    Private WithEvents dgtxt_BillDetails As New DataGridViewTextBoxEditingControl
    Private WithEvents dgtxt_ClothDetails As New DataGridViewTextBoxEditingControl

    Private Sub clear()

        ClrSTS = True

        New_Entry = False

        lbl_IdNo.Text = ""
        lbl_IdNo.ForeColor = Color.Black

        pnl_Back.Enabled = True

        lbl_IdNo.Text = ""
        cbo_Ledger.Text = ""
        txt_PackingCharge.Text = ""
        txt_RewindingCharge.Text = ""
        txt_WeldingCharge.Text = ""
        cbo_DiscountType.Text = "PERCENTAGE"
        txt_DiscountRate.Text = ""
        cbo_Count.Visible = False



        cbo_Count.Text = ""


        dgv_RateDetails.Rows.Clear()




        tab_Main.SelectTab(0)



        cbo_Count.Visible = False



        Grid_Cell_DeSelect()

        ClrSTS = False

    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox
        Dim dgvtxtedtctrl As DataGridViewTextBoxEditingControl

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


        If Me.ActiveControl.Name <> cbo_Count.Name Then
            cbo_Count.Visible = False
        End If


        Grid_Cell_DeSelect()

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
        If IsNothing(dgv_RateDetails.CurrentCell) Then Exit Sub

        dgv_RateDetails.CurrentCell.Selected = False
    End Sub

    Private Sub move_record(ByVal idno As Integer)
        Dim Cmd As New SqlClient.SqlCommand
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim Sno As Integer = 0, n As Integer = 0
        Dim NewCode As String = ""
        Dim BilType As String = ""
        Dim LedType As String = ""
        Dim LockSTS As Boolean = False
        Dim J As Integer = 0
        Dim Nr As Integer = 0
        Dim CrDr_Amt_ColNm As String = ""


        If Val(idno) = 0 Then Exit Sub

        clear()

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(Val(idno)) & "/" & Trim(OpYrCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.* from Ledger_Sizing_Rate_Head a Where a.Ledger_Rate_IdNo = " & Str(Val(idno)) & "", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_IdNo.Text = dt1.Rows(0).Item("Ledger_Rate_IdNo").ToString

                cbo_Ledger.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Ledger_IdNo").ToString))
                txt_RewindingCharge.Text = Format(Val(dt1.Rows(0).Item("Rewinding_Charge").ToString), "############0.00")
                txt_PackingCharge.Text = Format(Val(dt1.Rows(0).Item("Packing_Charge").ToString), "############0.00")
                txt_WeldingCharge.Text = Format(Val(dt1.Rows(0).Item("Welding_Charge").ToString), "############0.00")
                txt_DiscountRate.Text = Format(Val(dt1.Rows(0).Item("Discount_Rate").ToString), "############0.00")
                cbo_DiscountType.Text = (dt1.Rows(0).Item("Discount_Type").ToString)

                da2 = New SqlClient.SqlDataAdapter("Select a.* from Ledger_Sizing_Rate_Details a  where  a.Ledger_Rate_IdNo = " & Str(Val(idno)) & " ", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_RateDetails.Rows.Clear()
                Sno = 0

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_RateDetails.Rows.Add()

                        Sno = Sno + 1
                        dgv_RateDetails.Rows(n).Cells(0).Value = Val(Sno)
                        dgv_RateDetails.Rows(n).Cells(1).Value = Common_Procedures.Count_IdNoToName(con, Val(dt2.Rows(i).Item("Count_IdNo").ToString))
                        dgv_RateDetails.Rows(n).Cells(2).Value = Val(dt2.Rows(i).Item("Ends_From").ToString)
                        dgv_RateDetails.Rows(n).Cells(3).Value = Val(dt2.Rows(i).Item("Ends_To").ToString)
                        dgv_RateDetails.Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("Rate").ToString), "########0.00")


                    Next i

                End If

                dt2.Clear()



            End If

            dt1.Clear()


            If LockSTS = True Then

            End If


            Grid_Cell_DeSelect()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            dt1.Dispose()
            da1.Dispose()

            dt2.Dispose()
            da2.Dispose()

            If cbo_Ledger.Visible And cbo_Ledger.Enabled Then cbo_Ledger.Focus()

        End Try



    End Sub

    Private Sub Ledger_Sizing_Rate_Details_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        ' Try

        If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Ledger.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
            cbo_Ledger.Text = Trim(Common_Procedures.Master_Return.Return_Value)
        End If

        If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Count.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "COUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
            cbo_Count.Text = Trim(Common_Procedures.Master_Return.Return_Value)
        End If

        '    Common_Procedures.Master_Return.Return_Value = ""
        '    Common_Procedures.Master_Return.Master_Type = ""



    End Sub

    Private Sub Ledger_Sizing_Rate_Details_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        Me.Text = ""

        con.Open()

        cbo_DiscountType.Items.Clear()
        cbo_DiscountType.Items.Add("")
        cbo_DiscountType.Items.Add("PERCENTAGE")
        cbo_DiscountType.Items.Add("PAISE/KG")

        AddHandler cbo_Ledger.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Count.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_DiscountType.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PackingCharge.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_WeldingCharge.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_RewindingCharge.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DiscountRate.GotFocus, AddressOf ControlGotFocus


        AddHandler cbo_Ledger.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Count.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_DiscountType.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PackingCharge.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_WeldingCharge.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_RewindingCharge.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DiscountRate.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_PackingCharge.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_WeldingCharge.KeyDown, AddressOf TextBoxControlKeyDown
        ' AddHandler txt_RewindingCharge.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_PackingCharge.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_WeldingCharge.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_RewindingCharge.KeyPress, AddressOf TextBoxControlKeyPress


        new_record()

    End Sub

    Private Sub Ledger_Sizing_Rate_Details_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub Ledger_Sizing_Rate_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        Try
            If Asc(e.KeyChar) = 27 Then

                Me.Close()
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub



    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
        Dim dgv1 As New DataGridView

        On Error Resume Next

        If ActiveControl.Name = dgv_RateDetails.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            dgv1 = Nothing

            If ActiveControl.Name = dgv_RateDetails.Name Then
                dgv1 = dgv_RateDetails

            ElseIf dgv_RateDetails.IsCurrentRowDirty = True Then
                dgv1 = dgv_RateDetails

            ElseIf tab_Main.SelectedIndex = 0 Then
                dgv1 = dgv_RateDetails

            End If

            With dgv1

                If dgv1.Name = dgv_RateDetails.Name Then

                    If keyData = Keys.Enter Or keyData = Keys.Down Then

                        If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then

                            If .CurrentCell.RowIndex = .RowCount - 1 Then

                                txt_RewindingCharge.Focus()


                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                            End If

                        Else

                            If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then


                                txt_RewindingCharge.Focus()

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
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 1)

                            End If

                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)

                        End If

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

    End Function

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim tr As SqlClient.SqlTransaction
        Dim cmd As New SqlClient.SqlCommand
        Dim NewCode As String = ""
        Dim New_PurSalCode As String = ""
        Dim LedName As String = ""


        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Opening_Balance_Stock, "~L~") = 0 And InStr(Common_Procedures.UR.Opening_Balance_Stock, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
            Exit Sub
        End If

        If Val(lbl_IdNo.Text) = 0 Then
            MessageBox.Show("Invalid Ledger", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        LedName = Common_Procedures.Ledger_IdNoToName(con, Val(lbl_IdNo.Text))

        If Trim(LedName) = "" Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If


        tr = con.BeginTransaction

        Try

            cmd.Connection = con
            cmd.Transaction = tr



            cmd.CommandText = "Delete from Ledger_Sizing_Rate_Details Where Ledger_Rate_IdNo = " & Str(Val(lbl_IdNo.Text)) & " "
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Ledger_Sizing_Rate_Head Where Ledger_Rate_IdNo = " & Str(Val(lbl_IdNo.Text)) & " "
            cmd.ExecuteNonQuery()

            tr.Commit()

            tr.Dispose()
            cmd.Dispose()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If cbo_Ledger.Enabled = True And cbo_Ledger.Visible = True Then cbo_Ledger.Focus()

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        '-----
    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As Integer

        Try
            cmd.Connection = con
            cmd.CommandText = "select top 1 Ledger_Rate_IdNo from Ledger_Sizing_Rate_Head where Ledger_Rate_IdNo <> 0 Order by Ledger_Rate_IdNo"
            dr = cmd.ExecuteReader

            movno = 0
            If dr.HasRows Then
                If dr.Read Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movno = Val(dr(0).ToString)
                    End If
                End If
            End If

            dr.Close()

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As Integer = 0
        Dim OrdByNo As Single

        Try

            OrdByNo = Val(lbl_IdNo.Text)

            da = New SqlClient.SqlDataAdapter("select top 1 Ledger_Rate_IdNo from Ledger_Sizing_Rate_Head where Ledger_Rate_IdNo > " & Str(OrdByNo) & " Order by Ledger_Rate_IdNo", con)
            da.Fill(dt)

            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = Val(dt.Rows(0)(0).ToString)
                End If
            End If

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As Integer = 0
        Dim OrdByNo As Single

        Try

            OrdByNo = Val(lbl_IdNo.Text)

            cmd.Connection = con
            cmd.CommandText = "select top 1 Ledger_Rate_IdNo from Ledger_Sizing_Rate_Head where Ledger_Rate_IdNo < " & Str(Val(OrdByNo)) & " Order by Ledger_Rate_IdNo desc"

            dr = cmd.ExecuteReader

            If dr.HasRows Then
                If dr.Read Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movno = Val(dr(0).ToString)
                    End If
                End If
            End If
            dr.Close()

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try


    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim da As New SqlClient.SqlDataAdapter("select top 1 Ledger_Rate_IdNo from Ledger_Sizing_Rate_Head where Ledger_Rate_IdNo <> 0 Order by Ledger_Rate_IdNo desc", con)
        Dim dt As New DataTable
        Dim movno As Integer

        Try
            da.Fill(dt)

            movno = 0
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = Val(dt.Rows(0)(0).ToString)
                End If
            End If

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record

        clear()

        New_Entry = True
        lbl_IdNo.ForeColor = Color.Red

        lbl_IdNo.Text = Common_Procedures.get_MaxIdNo(con, "Ledger_Sizing_Rate_Head", "Ledger_Rate_IdNo", "")

        If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()


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
        Dim Sno As Integer = 0
        Dim Nr As Integer = 0
        Dim Dup_SetCd As String = ""
        Dim Dup_SetNoBmNo As String = ""
        Dim Mtr_Pc As Double = 0
        Dim Cnt_ID As Integer = 0
        Dim Led_Id As Integer = 0
        Dim bl_amt As Single = 0
        Dim CrDr_Amt_ColNm As String = ""
        Dim vou_bil_no As String = ""
        Dim vou_bil_code As String = ""
        Dim New_PurSalCode As String = "", Dup_PBillNo As String = ""
        Dim Yps_SlNo As Integer = 0, Yps_BillAmt As Double = 0, Yps_CommAmt As Double = 0, Yps_PBillNo As String = ""


        'If Val(lbl_Company.Tag) = 0 Then
        '    MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    Exit Sub
        'End If

        'If Common_Procedures.UserRight_Check(Common_Procedures.UR.Opening_Balance_Stock, New_Entry) = False Then Exit Sub

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        Led_Id = Common_Procedures.Ledger_AlaisNameToIdNo(con, (cbo_Ledger.Text))
        If Val(Led_Id) = 0 Then
            MessageBox.Show("Invalid Ledger Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Ledger.Enabled Then cbo_Ledger.Focus()
            Exit Sub
        End If

        With dgv_RateDetails

            For i = 0 To .RowCount - 1

                If Val(.Rows(i).Cells(4).Value) <> 0 Then

                    Cnt_ID = Common_Procedures.Count_NameToIdNo(con, Trim(.Rows(i).Cells(1).Value))
                    If Val(Cnt_ID) = 0 Then
                        MessageBox.Show("Invalid Count Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            tab_Main.SelectTab(0)
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(1)
                        End If
                        Exit Sub
                    End If

                    If Val(.Rows(i).Cells(4).Value) = 0 Then
                        MessageBox.Show("Invalid Rate", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            tab_Main.SelectTab(0)
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(4)
                            .CurrentCell.Selected = True
                        End If
                        Exit Sub
                    End If

                End If

            Next i

        End With

        tr = con.BeginTransaction

        Try

            cmd.Connection = con
            cmd.Transaction = tr

            If New_Entry = True Then

                lbl_IdNo.Text = Common_Procedures.get_MaxIdNo(con, "Ledger_Sizing_Rate_Head", "Ledger_Rate_IdNo", "", tr)

                cmd.CommandText = "Insert into Ledger_Sizing_Rate_Head(Ledger_Rate_IdNo , Ledger_IdNo   , Rewinding_Charge  ,  Packing_Charge  ,  Welding_Charge  ,   Discount_Type    ,   Discount_Rate) values (" & Str(Val(lbl_IdNo.Text)) & ", " & Val(Led_Id) & ", " & Val(txt_RewindingCharge.Text) & ", " & Val(txt_PackingCharge.Text) & ", " & Val(txt_WeldingCharge.Text) & ", '" & Trim(cbo_DiscountType.Text) & "', " & Val(txt_DiscountRate.Text) & ")"
                cmd.ExecuteNonQuery()

            Else
                cmd.CommandText = "update Ledger_Sizing_Rate_Head set Ledger_IdNo = " & Val(Led_Id) & ", Rewinding_Charge = " & Val(txt_RewindingCharge.Text) & ", Packing_Charge = " & Val(txt_PackingCharge.Text) & ", Welding_Charge = " & Val(txt_WeldingCharge.Text) & ",Discount_Type =  '" & Trim(cbo_DiscountType.Text) & "', Discount_Rate  =  " & Val(txt_DiscountRate.Text) & " Where Ledger_RAte_IdNo = " & Str(Val(lbl_IdNo.Text))
                cmd.ExecuteNonQuery()

            End If


            cmd.CommandText = "delete from Ledger_Sizing_Rate_Details where  Ledger_Rate_IdNo = " & Str(Val(lbl_IdNo.Text)) & " "
            cmd.ExecuteNonQuery()

            Sno = 0

            With dgv_RateDetails

                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(4).Value) <> 0 Then



                        Cnt_ID = Common_Procedures.Count_NameToIdNo(con, Trim(.Rows(i).Cells(1).Value), tr)

                        Sno = Sno + 1

                        If Val(.Rows(i).Cells(4).Value) <> 0 Then
                            'Nr = 0
                            'cmd.CommandText = "update Ledger_Sizing_Rate_Details set " _
                            '                            & " voucher_bill_date = @VouBillDate, " _
                            '                            & " party_bill_no = '" & Trim(.Rows(i).Cells(1).Value) & "', " _
                            '                            & " agent_idno = " & Str(Val(vAgt_ID)) & ", " _
                            '                            & " bill_amount = " & Str(Val(.Rows(i).Cells(4).Value)) & ", " _
                            '                            & " crdr_type = '" & Trim(.Rows(i).Cells(5).Value) & "', " _
                            '                            & " " & CrDr_Amt_ColNm & " = " & Str(Val(.Rows(i).Cells(4).Value)) & " " _
                            '                            & " where " _
                            '                            & " Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and " _
                            '                            & " voucher_bill_code = '" & Trim(.Rows(i).Cells(7).Value) & "'"

                            'Nr = cmd.ExecuteNonQuery()

                            'If Nr = 0 Then
                            '    Throw New ApplicationException("Error On Bill Details")
                            'End If


                            'Else

                            '    vou_bil_no = Common_Procedures.get_MaxCode(con, "Ledger_Sizing_Rate_Details", "Voucher_Bill_Code", "For_OrderBy", "", Val(lbl_Company.Tag), OpYrCode, tr)
                            '    vou_bil_code = Trim(Val(lbl_Company.Tag)) & "-" & Trim(vou_bil_no) & "/" & Trim(OpYrCode)

                            cmd.CommandText = "Insert into Ledger_Sizing_Rate_Details ( Ledger_Rate_IdNo        ,       Ledger_IdNo        , Sl_No            ,           Count_Idno    ,              Ends_From                ,                  Ends_To            ,         Rate                                    ) " _
                                                    & "  Values (  " & Str(Val(lbl_IdNo.Text)) & "       ,  " & Str(Val(Led_Id)) & ", " & Val(Sno) & " ," & Str(Val(Cnt_ID)) & ", " & Str(Val(.Rows(i).Cells(2).Value)) & ", " & Str(Val(.Rows(i).Cells(3).Value)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & " )"
                            cmd.ExecuteNonQuery()

                        End If

                    End If

                Next i

            End With

            tr.Commit()

            If New_Entry = True Then new_record()

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()

    End Sub


    Public Sub print_record() Implements Interface_MDIActions.print_record
        '-----
    End Sub



    Private Sub cbo_Ledger_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Ledger_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, Nothing, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10)", "(Ledger_IdNo = 0)")

        With cbo_Ledger
            If (e.KeyValue = 40 And .DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                tab_Main.SelectTab(0)

                If dgv_RateDetails.RowCount > 0 Then

                    dgv_RateDetails.Focus()
                    dgv_RateDetails.CurrentCell = dgv_RateDetails.Rows(0).Cells(1)
                    dgv_RateDetails.CurrentCell.Selected = True

                Else
                    txt_RewindingCharge.Focus()

                End If

            End If


        End With

    End Sub

    Private Sub cbo_Ledger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ledger.KeyPress
        Dim LedIdNo As Integer
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String
        Dim cmd As New SqlClient.SqlCommand


        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Ledger, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10)", "(Ledger_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
            'If Val(LedIdNo) <> 0 Then
            '    move_record(LedIdNo)
            'End If
            cmd.Connection = con
            cmd.CommandText = "select Ledger_Rate_IdNo from Ledger_Sizing_Rate_Head where Ledger_idno = " & Val(LedIdNo) & "   "
            Da = New SqlClient.SqlDataAdapter(cmd)
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


                tab_Main.SelectTab(0)
                If dgv_RateDetails.RowCount > 0 Then

                    dgv_RateDetails.Focus()
                    dgv_RateDetails.CurrentCell = dgv_RateDetails.Rows(0).Cells(1)
                    dgv_RateDetails.CurrentCell.Selected = True


                Else

                    txt_RewindingCharge.Focus()
                End If

            End If

        End If

    End Sub

    Private Sub cbo_Ledger_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Ledger.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        save_record()
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub



    Private Sub cbo_Count_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Count.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
    End Sub

    Private Sub cbo_Count_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Count.KeyDown

        Try
            vcbo_KeyDwnVal = e.KeyValue

            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Count, Nothing, Nothing, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

            With cbo_Count
                If (e.KeyValue = 38 And .DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                    e.Handled = True

                    With dgv_RateDetails
                        If Val(.CurrentCell.RowIndex) <= 0 Then
                            cbo_Ledger.Focus()

                        Else

                            .Focus()
                            .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(4)
                            .CurrentCell.Selected = True
                        End If
                    End With


                ElseIf (e.KeyValue = 40 And .DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                    e.Handled = True
                    With dgv_RateDetails
                        If .CurrentRow.Index = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then

                            txt_RewindingCharge.Focus()
                        Else

                            .Focus()
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                            .CurrentCell.Selected = True
                        End If
                    End With

                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT ITEM...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Count_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Count.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Count, Nothing, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

        Try

            With cbo_Count

                If Asc(e.KeyChar) = 13 Then

                    With dgv_RateDetails
                        If .CurrentRow.Index = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then

                            txt_RewindingCharge.Focus()
                        Else
                            .Focus()
                            .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Count.Text)
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                        End If
                    End With


                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_Count_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Count.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Count_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Count.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_Count_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Count.TextChanged
        Try
            If cbo_Count.Visible Then
                With dgv_RateDetails
                    If .Rows.Count > 0 Then
                        If Val(cbo_Count.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                            .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Count.Text)
                        End If
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub


    Private Sub dgv_BillDetails_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_RateDetails.CellEndEdit
        dgv_BillDetails_CellLeave(sender, e)
    End Sub

    Private Sub dgv_BillDetails_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_RateDetails.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable

        If ClrSTS = True = True Then Exit Sub

        With dgv_RateDetails
            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If



            If .CurrentCell.ColumnIndex = 1 Then

                If cbo_Count.Visible = False Or Val(cbo_Count.Tag) <> e.RowIndex Then

                    Da = New SqlClient.SqlDataAdapter("select Count_Name from Count_Head order by Count_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Count.DataSource = Dt1
                    cbo_Count.DisplayMember = "Count_Name"

                    cbo_Count.Left = .Left + .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_Count.Top = .Top + .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                    cbo_Count.Width = .CurrentCell.Size.Width
                    cbo_Count.Text = Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_Count.Tag = Val(.CurrentCell.RowIndex)
                    cbo_Count.Visible = True

                    cbo_Count.BringToFront()
                    cbo_Count.Focus()

                End If

            Else

                cbo_Count.Visible = False

            End If



        End With

    End Sub

    Private Sub dgv_BillDetails_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_RateDetails.CellLeave
        Try
            With dgv_RateDetails
                If .Rows.Count > 0 Then
                    If .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 6 Then
                        If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                            .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                        Else
                            .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
                        End If
                    End If
                End If
            End With

        Catch ex As Exception
            '----
        End Try

    End Sub

    Private Sub dgv_BillDetails_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_RateDetails.CellValueChanged
        Try
            If IsNothing(dgv_RateDetails.CurrentCell) Then Exit Sub
            With dgv_RateDetails
                If .Visible Then
                    If .Rows.Count > 0 Then
                        If e.ColumnIndex = 4 Or e.ColumnIndex = 5 Or e.ColumnIndex = 6 Then

                        End If
                    End If
                End If
            End With

        Catch ex As Exception
            '-----
        End Try

    End Sub

    Private Sub dgv_BillDetails_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_RateDetails.EditingControlShowing
        dgtxt_BillDetails = Nothing

        With dgv_RateDetails

            If .Rows.Count > 0 Then


                dgtxt_BillDetails = CType(dgv_RateDetails.EditingControl, DataGridViewTextBoxEditingControl)

            End If

        End With

    End Sub

    Private Sub dgv_BillDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_RateDetails.KeyUp
        Dim i As Integer
        Dim n As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_RateDetails

                If .Rows.Count > 0 Then

                    If Val(.CurrentRow.Cells(6).Value) = 0 Then

                        n = .CurrentRow.Index

                        If n = .Rows.Count - 1 Then
                            For i = 0 To .ColumnCount - 1
                                .Rows(n).Cells(i).Value = ""
                            Next

                        Else
                            .Rows.RemoveAt(n)

                        End If



                    End If

                End If

            End With

        End If

    End Sub

    Private Sub dgv_BillDetails_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_RateDetails.LostFocus
        On Error Resume Next
        dgv_RateDetails.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_BillDetails_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_RateDetails.RowsAdded
        Dim n As Integer

        With dgv_RateDetails
            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)
        End With

    End Sub



    Private Sub dgtxt_BillDetails_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_BillDetails.Enter
        dgv_RateDetails.EditingControl.BackColor = Color.Lime
        dgv_RateDetails.EditingControl.ForeColor = Color.Blue
        dgtxt_BillDetails.SelectAll()
    End Sub



    Private Sub dgtxt_BillDetails_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_BillDetails.KeyDown
        With dgv_RateDetails

            If .Rows.Count > 0 Then

                'If Val(.CurrentRow.Cells(6).Value) <> 0 Then

                '    'e.Handled = True
                '    'e.SuppressKeyPress = True

                'End If

            End If

        End With
    End Sub

    Private Sub dgtxt_BillDetails_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_BillDetails.KeyPress
        With dgv_RateDetails
            ' If Val(.CurrentRow.Cells(6).Value) <> 0 Then
            'e.Handled = True

            ' Else
            'If .CurrentCell.ColumnIndex = 4 Then
            If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                e.Handled = True
            End If
            ' End If

            ' End If


        End With
    End Sub

    Private Sub dgtxt_BillDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_BillDetails.KeyUp
        dgv_BillDetails_KeyUp(sender, e)
    End Sub





    Private Sub tab_Main_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tab_Main.SelectedIndexChanged
        If tab_Main.SelectedIndex = 0 Then
            If dgv_RateDetails.Enabled Then
                dgv_RateDetails.Focus()
                dgv_RateDetails.CurrentCell = dgv_RateDetails.Rows(0).Cells(1)
                dgv_RateDetails.CurrentCell.Selected = True
            End If
        End If
    End Sub




    Private Sub txt_RewindingCharge_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_RewindingCharge.KeyDown
        If e.KeyValue = 38 Then
            If dgv_RateDetails.RowCount > 0 Then


                dgv_RateDetails.Focus()
                dgv_RateDetails.CurrentCell = dgv_RateDetails.Rows(0).Cells(1)
            Else
                cbo_Ledger.Focus()
            End If
        End If
        If e.KeyValue = 40 Then
            txt_PackingCharge.Focus()
        End If

    End Sub
    Private Sub cbo_DiscountType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_DiscountType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_DiscountType, txt_DiscountRate, "", "", "", "")
    End Sub

    Private Sub cbo_DiscountType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DiscountType.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_DiscountType, txt_WeldingCharge, txt_DiscountRate, "", "", "", "")
    End Sub

    Private Sub txt_DiscountRate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_DiscountRate.KeyDown
        If e.KeyCode = 40 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                cbo_Ledger.Focus()
            End If
        End If
        If e.KeyCode = 38 Then cbo_DiscountType.Focus()
    End Sub

    Private Sub txt_DiscountRate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_DiscountRate.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                cbo_Ledger.Focus()
            End If
        End If
    End Sub

    Private Sub txt_PackingCharge_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_PackingCharge.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_RewindingCharge_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_RewindingCharge.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_WeldingCharge_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_WeldingCharge.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub
End Class