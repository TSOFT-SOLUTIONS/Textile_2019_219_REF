Public Class System_name_details
    Implements Interface_MDIActions

    Dim con As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)

    Private Sub clear()

        dgv_Details.Rows.Clear()

    End Sub


    Public Sub move_record()
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim sno As Integer, n As Integer

        clear()

        da = New SqlClient.SqlDataAdapter("select * from System_name_Details ORDER BY Type DESC, Last_Opened_SystemDateTime DESC, exe_Date_Time DESC, Computer_name", con)
        dt2 = New DataTable
        da.Fill(dt2)

        dgv_Details.Rows.Clear()
        sno = 0

        If dt2.Rows.Count > 0 Then

            For i = 0 To dt2.Rows.Count - 1

                n = dgv_Details.Rows.Add()

                sno = sno + 1
                dgv_Details.Rows(n).Cells(0).Value = Val(sno)
                dgv_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Type").ToString

                dgv_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Computer_name").ToString
                dgv_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Computer_serialNo").ToString
                dgv_Details.Rows(n).Cells(4).Value = dt2.Rows(i).Item("Sql_Instance_name").ToString
                dgv_Details.Rows(n).Cells(5).Value = dt2.Rows(i).Item("Sql_Data_path").ToString

                dgv_Details.Rows(n).Cells(6).Value = dt2.Rows(i).Item("Software_Path").ToString

                dgv_Details.Rows(n).Cells(7).Value = dt2.Rows(i).Item("Software_Exe_name").ToString

                dgv_Details.Rows(n).Cells(8).Value = ""
                If IsDBNull(dt2.Rows(i).Item("exe_Date_Time")) = False Then
                    If Trim(dt2.Rows(i).Item("exe_Date_Time")) <> "" Then
                        If IsDate(dt2.Rows(i).Item("exe_Date_Time")) = True Then
                            dgv_Details.Rows(n).Cells(8).Value = Format(dt2.Rows(i).Item("exe_Date_Time"), "dd/MM/yyyy HH:MM tt")
                        End If
                    End If
                End If

                dgv_Details.Rows(n).Cells(9).Value = ""
                If IsDBNull(dt2.Rows(i).Item("Last_Opened_SystemDateTime")) = False Then
                    If Trim(dt2.Rows(i).Item("Last_Opened_SystemDateTime")) <> "" Then
                        If IsDate(dt2.Rows(i).Item("Last_Opened_SystemDateTime")) = True Then
                            dgv_Details.Rows(n).Cells(9).Value = Format(dt2.Rows(i).Item("Last_Opened_SystemDateTime"), "dd/MM/yyyy HH:MM tt")
                        End If
                    End If
                End If

                dgv_Details.Rows(n).Cells(10).Value = dt2.Rows(i).Item("Computer_SerialNo").ToString

            Next i

            For i = 0 To dgv_Details.RowCount - 1
                dgv_Details.Rows(i).Cells(0).Value = Val(i) + 1
            Next

        End If

        dt2.Dispose()
        da.Dispose()

        With dgv_Details

            For i = 0 To .RowCount - 1

                If dgv_Details.Enabled And dgv_Details.Visible Then
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(1)
                End If
            Next
        End With

    End Sub

    Private Sub System_name_details_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub System_name_details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        Try
            If Asc(e.KeyChar) = 27 Then


                If MessageBox.Show("Do you want to Close?", "FOR CLOSING ENTRY...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) <> Windows.Forms.DialogResult.Yes Then
                    Exit Sub

                Else
                    Me.Close()

                End If

            End If


        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try


    End Sub

    Private Sub System_name_details_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        con.Open()

        move_record()

    End Sub



    Public Sub new_record() Implements Interface_MDIActions.new_record
        clear()
    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record

        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim nr As Long = 0



        trans = con.BeginTransaction

        Try
            cmd.Connection = con
            cmd.Transaction = trans

            cmd.CommandText = "delete from System_name_Details"
            nr = cmd.ExecuteNonQuery()

            With dgv_Details

                For i = 0 To .RowCount - 1

                    If Trim(.Rows(i).Cells(2).Value) <> "" Then

                        cmd.CommandText = "Insert into System_name_Details   (      Type                                , Computer_name                           ,        Computer_SerialNo                     , Sql_Instance_name                     ,Sql_Data_path                                      ,     Software_Exe_Name                      ,            Exe_Date_Time                             ) " &
                                           "Values                            ( '" & Trim(.Rows(i).Cells(1).Value) & "'  ,'" & Trim(.Rows(i).Cells(2).Value) & "'  ,  '" & Trim(.Rows(i).Cells(3).Value) & "'        ,'" & Trim(.Rows(i).Cells(4).Value) & "'       ,'" & Trim(.Rows(i).Cells(5).Value) & "'  ,    '" & Trim(.Rows(i).Cells(6).Value) & "'  ,  '" & Trim(.Rows(i).Cells(7).Value) & "'         )"
                        cmd.ExecuteNonQuery()

                    End If


                Next
            End With

            trans.Commit()

        Catch ex As Exception
            trans.Rollback()

        End Try

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        '-----
    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        '-----
    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        '-----
    End Sub

    Private Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim cmd As New SqlClient.SqlCommand
        Dim Nr As Integer = 0
        Dim g As New Password

        g.ShowDialog()

        If Trim(UCase(Common_Procedures.Password_Input)) <> "TSDD" Then
            MessageBox.Show("Invalid Password", "PASSWORD FAILED.....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        Try

            If IsNothing(dgv_Details.CurrentCell) Then Exit Sub

            cmd.Connection = con

            cmd.CommandText = "delete from System_name_Details where computer_name = '" & Trim(dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells(2).Value) & "' and Computer_SerialNo = '" & Trim(dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells(10).Value) & "'"
            cmd.ExecuteNonQuery()

            cmd.Dispose()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

            move_record()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
            End If

        End Try

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        '-----
    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        '-----
    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        '-----
    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        '-----
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        '-----
    End Sub

    Private Sub dgv_Details_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Details.RowsAdded
        Dim n As Integer

        With dgv_Details
            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)
        End With
    End Sub

    Private Sub dgv_Details_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyUp
        Dim i, n As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            delete_record()

        End If

    End Sub
End Class