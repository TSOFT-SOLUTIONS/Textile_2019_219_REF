Public Class Loading_unloading_rate_Creation

    Implements Interface_MDIActions
    Private Prec_ActCtrl As New Control
    Private New_Entry As Boolean = False
    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl
    Private vcbo_KeyDwnVal As Double
    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox

        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Then
            Me.ActiveControl.BackColor = Color.SpringGreen ' Color.MistyRose ' Color.PaleGreen
            Me.ActiveControl.ForeColor = Color.Blue
        End If

        If TypeOf Me.ActiveControl Is TextBox Then
            txtbx = Me.ActiveControl
            txtbx.SelectAll()

        ElseIf TypeOf Me.ActiveControl Is ComboBox Then
            combobx = Me.ActiveControl
            combobx.SelectAll()

        End If
        Grid_Cell_DeSelect()
     
        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub Grid_Cell_DeSelect()
        On Error Resume Next
        If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
        dgv_Details.CurrentCell.Selected = False

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

    Private Sub TextBoxControlKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        On Error Resume Next
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub TextBoxControlKeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        On Error Resume Next
        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub Loading_unloading_rate_Creation_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

        con.Close()
        con.Dispose()
    End Sub
    Private Sub Close_Form()


        Me.Close()

   

    End Sub
    Private Sub Loading_unloading_rate_Creation_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        Try
            If Asc(e.KeyChar) = 27 Then
                Close_Form()
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub Loading_unloading_rate_Creation_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Me.Text = ""
        con.Open()


        AddHandler txt_Pavu_beam_Loading.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_pavu_Beam_Unloading.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Empty_Beam_Loading.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Empty_Beam_UnLoading.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Cloth_Loading.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Cloth_Unloading.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Cloth_kgs.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_vehicle.GotFocus, AddressOf ControlGotFocus


        AddHandler cbo_vehicle.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Pavu_beam_Loading.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_pavu_Beam_Unloading.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Empty_Beam_Loading.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Empty_Beam_UnLoading.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Cloth_Loading.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Cloth_Unloading.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Cloth_kgs.LostFocus, AddressOf ControlLostFocus





        AddHandler txt_Pavu_beam_Loading.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_pavu_Beam_Unloading.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Empty_Beam_Loading.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Empty_Beam_UnLoading.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Cloth_Loading.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Cloth_Unloading.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Cloth_kgs.KeyDown, AddressOf TextBoxControlKeyDown


        AddHandler txt_Pavu_beam_Loading.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_pavu_Beam_Unloading.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Empty_Beam_Loading.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Empty_Beam_UnLoading.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Cloth_Loading.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Cloth_Unloading.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Cloth_kgs.KeyPress, AddressOf TextBoxControlKeyPress


        'move_record()

    End Sub

    Public Sub clear()
        txt_Pavu_beam_Loading.Text = ""
        txt_pavu_Beam_Unloading.Text = ""
        cbo_vehicle.Text = ""

        txt_Empty_Beam_Loading.Text = ""
        txt_Empty_Beam_UnLoading.Text = ""
        txt_Cloth_Loading.Text = ""
        txt_Cloth_Unloading.Text = ""
        txt_Cloth_kgs.Text = ""

        dgv_Details.Rows.Clear()
    End Sub

    Private Sub move_record(ByVal idno As Integer)
        Dim da As New SqlClient.SqlDataAdapter
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable

        Dim sno As Integer, n As Integer
        Try
            da1 = New SqlClient.SqlDataAdapter("select * from Loading_unloading_Rate_Head where Vehicle_IdNo = " & Str(Val(idno)) & "", con)
            dt1 = New DataTable
            da1.Fill(dt1)
            If dt1.Rows.Count > 0 Then

            End If

            cbo_vehicle.Text = Common_Procedures.Vehicle_IdNoToName(con, Val(dt1.Rows(0).Item("Vehicle_idno").ToString))

            txt_Pavu_beam_Loading.Text = Format(Val(dt1.Rows(0).Item("Pavu_Beam_Loading_rate").ToString()), "####0.00")

            txt_pavu_Beam_Unloading.Text = Format(Val(dt1.Rows(0).Item("Pavu_Beam_UnLoading_rate").ToString()), "####0.00")

            txt_Empty_Beam_Loading.Text = Format(Val(dt1.Rows(0).Item("Empty_Beam_Loading_rate").ToString()), "####0.00")

            txt_Empty_Beam_UnLoading.Text = Format(Val(dt1.Rows(0).Item("Empty_Beam_UnLoading_rate").ToString()), "####0.00")

            txt_Cloth_kgs.Text = Format(Val(dt1.Rows(0).Item("Cloth_kgs").ToString()), "####0.000")
            txt_Cloth_Loading.Text = Format(Val(dt1.Rows(0).Item("CLoth_Loading_rate").ToString()), "####0.00")
            txt_Cloth_Unloading.Text = Format(Val(dt1.Rows(0).Item("CLoth_UnLoading_rate").ToString()), "####0.00")



            da = New SqlClient.SqlDataAdapter("select * from Loading_Unloading_Details where Vehicle_IdNo = " & Str(Val(idno)) & " Order by sl_no", con)
            dt2 = New DataTable
            da.Fill(dt2)

            dgv_Details.Rows.Clear()
            sno = 0

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Details.Rows.Add()

                    sno = sno + 1
                    dgv_Details.Rows(n).Cells(0).Value = Val(sno)
                    dgv_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("From_Weight")
                    dgv_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("To_Weight").ToString
                    dgv_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Loading_Charges").ToString
                    dgv_Details.Rows(n).Cells(4).Value = dt2.Rows(i).Item("UnLoading_Charges").ToString
                Next i

                For i = 0 To dgv_Details.RowCount - 1
                    dgv_Details.Rows(i).Cells(0).Value = Val(i) + 1
                Next



            Else

                txt_Pavu_beam_Loading.Text = ""

                txt_pavu_Beam_Unloading.Text = ""

                txt_Empty_Beam_Loading.Text = ""

                txt_Empty_Beam_UnLoading.Text = ""

                txt_Cloth_Loading.Text = ""

                txt_Cloth_Unloading.Text = ""

                txt_Cloth_kgs.Text = ""


            End If
            dt1.Dispose()
            da1.Dispose()
            dt2.Dispose()
            da.Dispose()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

        End Try



     


    End Sub



    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim VehTo_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction


        'If Trim(txt_Yarn_Loading.Text) = "" Then
        '    MessageBox.Show("Invalid", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    Exit Sub
        '    txt_Yarn_Loading.Focus()

        'End If

        'If Trim(txt_Yarn_UnLoading.Text) = "" Then
        '    MessageBox.Show("Invalid", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    Exit Sub
        '    txt_Yarn_UnLoading.Focus()

        'End If
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Loading_UnLoading_Rate, New_Entry, Me) = False Then Exit Sub

        If Trim(txt_Pavu_beam_Loading.Text) = "" Then
            MessageBox.Show("Invalid Pavu Beam Loading", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
            txt_Pavu_beam_Loading.Focus()

        End If

        If Trim(txt_pavu_Beam_Unloading.Text) = "" Then
            MessageBox.Show("Invalid Pavu Beam unLoading", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
            txt_pavu_Beam_Unloading.Focus()

        End If

        If Trim(txt_Empty_Beam_Loading.Text) = "" Then
            MessageBox.Show("Invalid", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
            txt_Empty_Beam_Loading.Focus()

        End If

        If Trim(txt_Empty_Beam_UnLoading.Text) = "" Then
            MessageBox.Show("Invalid", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
            txt_Empty_Beam_UnLoading.Focus()

        End If

        If Trim(txt_Cloth_Loading.Text) = "" Then
            MessageBox.Show("Invalid", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
            txt_Cloth_Loading.Focus()

        End If

        If Trim(txt_Cloth_Unloading.Text) = "" Then
            MessageBox.Show("Invalid", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
            txt_Cloth_Unloading.Focus()

        End If
        If Trim(txt_Cloth_kgs.Text) = "" Then
            MessageBox.Show("Invalid", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
            txt_Cloth_kgs.Focus()

        End If

        'If Trim(txt_Yarn_Bag_kgs.Text) = "" Then
        '    MessageBox.Show("Invalid", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    Exit Sub
        '    txt_Yarn_Bag_kgs.Focus()

        'End If
        VehTo_ID = Common_Procedures.Vehicle_NameToIdNo(con, cbo_vehicle.Text)
        Try

            trans = con.BeginTransaction

            cmd.Connection = con
            cmd.Transaction = trans

            cmd.CommandText = "delete from Loading_unloading_Rate_Head where  Vehicle_idno = " & Str(Val(VehTo_ID)) & ""
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Insert into Loading_unloading_Rate_Head (Pavu_Beam_Loading_rate,Pavu_Beam_unLoading_rate,Empty_Beam_Loading_rate,Empty_Beam_unLoading_rate,Cloth_Loading_rate,Cloth_unLoading_rate,CLoth_kgs,Vehicle_idno)values (" & (Val(txt_Pavu_beam_Loading.Text)) & ", " & (Val(txt_pavu_Beam_Unloading.Text)) & "," & (Val(txt_Empty_Beam_Loading.Text)) & ", " & (Val(txt_Empty_Beam_UnLoading.Text)) & "," & (Val(txt_Cloth_Loading.Text)) & ", " & (Val(txt_Cloth_Unloading.Text)) & "," & (Val(txt_Cloth_kgs.Text)) & "," & Val(VehTo_ID) & ")"
            cmd.ExecuteNonQuery()




            cmd.CommandText = "delete from Loading_Unloading_Details where  Vehicle_idno = " & Str(Val(VehTo_ID)) & ""
            cmd.ExecuteNonQuery()

            With dgv_Details
                Sno = 0
                For i = 0 To .RowCount - 1

                    Sno = Sno + 1

                    If .Rows(i).Cells(1).Value <> 0 Then


                        ' vDttm = .Rows(i).Cells(1).Value.ToString

                        ' cmd.Parameters.Clear()
                        ' cmd.Parameters.AddWithValue("@HolidayDate", vDttm)

                        cmd.CommandText = "Insert into Loading_Unloading_Details (                            Vehicle_idno       ,                Sl_No , From_Weight  ,                    To_Weight        ,                    Loading_Charges        ,                    unLoading_Charges               ) " &
                                              " Values                     ( " & Str(Val(VehTo_ID)) & ", " & Str(Val(.Rows(i).Cells(0).Value)) & ",     " & Str(Val(.Rows(i).Cells(1).Value)) & " , " & Str(Val(.Rows(i).Cells(2).Value)) & ", " & Str(Val(.Rows(i).Cells(3).Value)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & " ) "
                        cmd.ExecuteNonQuery()





                    End If

                Next

            End With

            'End If

            trans.Commit()

            'Common_Procedures.Master_Return.Return_Value = Trim(txt_MillName.Text)
            'Common_Procedures.Master_Return.Master_Type = "Mill"



            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING....", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            trans.Rollback()
            If InStr(1, Trim(LCase(ex.Message)), "ix_Loading_Unloading_Details") > 0 Then

                MessageBox.Show(ex.Message, "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

        Finally
            'With dgv_Details

            '    For i = 0 To .RowCount - 1

            '        If dgv_Details.Enabled And dgv_Details.Visible Then
            '            dgv_Details.Focus()
            '            dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(1)
            '        End If
            '    Next
            'End With
        End Try





    End Sub


    Private Sub Btn_save_Click(sender As System.Object, e As System.EventArgs) Handles Btn_save.Click
        save_record()
    End Sub



    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim cmd As New SqlClient.SqlCommand

        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable


        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.Loading_UnLoading_Rate, New_Entry, Me) = False Then Exit Sub





        cmd.Connection = con

        cmd.CommandText = "delete  from Loading_unloading_Rate_Head  "
        cmd.ExecuteNonQuery()

        cmd.CommandText = "delete from Loading_Unloading_Details "
        cmd.ExecuteNonQuery()



        clear()



    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record

    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record

    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record

    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record

    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record
        clear()

        'New_Entry = True
        ' lbl_IdNo.ForeColor = Color.Red

        ' cbo_vehicle.Text = Common_Procedures.get_MaxIdNo(con, "vehicle_Head", "vehicle_IdNo", "")

        If cbo_vehicle.Enabled And cbo_vehicle.Visible Then cbo_vehicle.Focus()
    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        '---
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        '----
    End Sub




    Private Sub txt_Pavu_beam_Loading_Leave(sender As Object, e As System.EventArgs) Handles txt_Pavu_beam_Loading.Leave
        txt_Pavu_beam_Loading.Text = (Format(Val(txt_Pavu_beam_Loading.Text), "########0.00"))
    End Sub

    Private Sub txt_pavu_Beam_Unloading_Leave(sender As Object, e As System.EventArgs) Handles txt_pavu_Beam_Unloading.Leave
        txt_pavu_Beam_Unloading.Text = (Format(Val(txt_pavu_Beam_Unloading.Text), "########0.00"))
    End Sub

    Private Sub txt_Cloth_Loading_Leave(sender As Object, e As System.EventArgs) Handles txt_Cloth_Loading.Leave
        txt_Cloth_Loading.Text = (Format(Val(txt_Cloth_Loading.Text), "########0.00"))
    End Sub

    Private Sub txt_Cloth_Unloading_Leave(sender As Object, e As System.EventArgs) Handles txt_Cloth_Unloading.Leave
        txt_Cloth_Unloading.Text = (Format(Val(txt_Cloth_Unloading.Text), "########0.00"))
    End Sub

    Private Sub txt_Empty_Beam_Loading_Leave(sender As Object, e As System.EventArgs) Handles txt_Empty_Beam_Loading.Leave
        txt_Empty_Beam_Loading.Text = (Format(Val(txt_Empty_Beam_Loading.Text), "########0.00"))
    End Sub

    Private Sub txt_Empty_Beam_UnLoading_Leave(sender As Object, e As System.EventArgs) Handles txt_Empty_Beam_UnLoading.Leave
        txt_Empty_Beam_UnLoading.Text = (Format(Val(txt_Empty_Beam_UnLoading.Text), "########0.00"))
    End Sub

    Private Sub txt_Cloth_kgs_Leave(sender As Object, e As System.EventArgs) Handles txt_Cloth_kgs.Leave
        txt_Cloth_kgs.Text = (Format(Val(txt_Cloth_kgs.Text), "########0.000"))
    End Sub

    Private Sub txt_Pavu_beam_Loading_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Pavu_beam_Loading.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_pavu_Beam_Unloading_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_pavu_Beam_Unloading.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_Empty_Beam_Loading_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Empty_Beam_Loading.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_Empty_Beam_UnLoading_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Empty_Beam_UnLoading.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_Cloth_kgs_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Cloth_kgs.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_Cloth_Loading_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Cloth_Loading.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_Cloth_Unloading_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Cloth_Unloading.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub cbo_vehicle_GotFocus(sender As Object, e As System.EventArgs) Handles cbo_vehicle.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Vehicle_Head", "Vehicle_No", "", "(Vehicle_IdNo = 0)")

    End Sub

    Private Sub cbo_vehicle_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles cbo_vehicle.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_vehicle, Nothing, Nothing, "Vehicle_Head", "Vehicle_No", "", "(Vehicle_IdNo = 0)")

        If (e.KeyValue = 40 And cbo_vehicle.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
            dgv_Details.CurrentCell.Selected = True
        End If
    End Sub

    Private Sub cbo_vehicle_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_vehicle.KeyPress
        Dim vehIdNo As Integer

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_vehicle, Nothing, "Vehicle_Head", "Vehicle_No", "", "(Vehicle_idno = 0)")

        If Asc(e.KeyChar) = 13 Then

            vehIdNo = Common_Procedures.Vehicle_NameToIdNo(con, cbo_vehicle.Text)
           
            If Val(vehIdNo) <> 0 Then
                move_record(vehIdNo)

            End If

            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
            dgv_Details.CurrentCell.Selected = True
        End If

    End Sub

    Private Sub cbo_vehicle_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles cbo_vehicle.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Common_Procedures.MDI_LedType = ""
            Dim f As New VehicleNo_Creation


            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub Btn_Close_Click(sender As Object, e As System.EventArgs) Handles Btn_Close.Click
        Me.Close()

    End Sub








    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
        Dim dgv1 As New DataGridView

        On Error Resume Next


        If ActiveControl.Name = dgv_Details.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Or TypeOf ActiveControl Is DataGridViewComboBoxEditingControl Then

            dgv1 = Nothing

            If ActiveControl.Name = dgv_Details.Name Then
                dgv1 = dgv_Details

            ElseIf dgv_Details.IsCurrentRowDirty = True Then
                dgv1 = dgv_Details

            Else
                dgv1 = dgv_Details

            End If

            With dgv1

                If keyData = Keys.Enter Or keyData = Keys.Down Then

                    If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then
                        If .CurrentCell.RowIndex = .RowCount - 1 Then
                            txt_Pavu_beam_Loading.Focus()


                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                        End If

                    Else

                        If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then
                            txt_Pavu_beam_Loading.Focus()


                        Else
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                        End If

                    End If

                    Return True

                ElseIf keyData = Keys.Up Then
                    If .CurrentCell.ColumnIndex <= 1 Then
                        If .CurrentCell.RowIndex = 0 Then
                            cbo_vehicle.Focus()


                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(4)

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

    End Function

    Private Sub dgv_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgv_Details.KeyPress
        With dgv_Details

            'If .Visible Then
            '    If .CurrentCell.ColumnIndex = 2 Then

            '        If Asc(e.KeyChar) = 13 And e.Handled = True Then
            '            .Focus()
            '            .CurrentCell = .CurrentRow.Cells(3)
            '            .CurrentCell.Selected = True
            '        End If

            '    End If

            'End If

        End With
    End Sub
    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.Enter
        dgv_Details.EditingControl.BackColor = Color.Lime
        dgv_Details.EditingControl.ForeColor = Color.PaleGreen
    End Sub




    Private Sub dgtxt_Details_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.TextChanged
        Try

            With dgv_Details

                If .Visible Then
                    If .Rows.Count > 0 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(dgtxt_Details.Text)
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


    Private Sub dgv_countdetails_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Details.RowsAdded
        Dim n As Integer

        With dgv_Details
            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)
        End With
    End Sub


    Private Sub dgv_countdetails_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEnter
        With dgv_Details
            If Val(.Rows(e.RowIndex).Cells(0).Value) = 0 Then
                .Rows(e.RowIndex).Cells(0).Value = e.RowIndex + 1
            End If
        End With

    End Sub

    Private Sub dgv_Details_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles dgv_Details.DataError
        If e.Exception.Message = "DataGridViewComboBoxCell value is not Valid" Then
            'dgv_Details.CurrentCell.Value = dgv_Details.Rows(e.RowIndex).Cells(e.ColumnIndex).Value
            'If dgv_Details.CurrentRow.Cells(2) = dgv_Details.Columns(e.ColumnIndex) Then
            e.ThrowException = False
        End If
    End Sub



End Class
