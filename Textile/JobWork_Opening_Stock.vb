Public Class JobWork_Opening_Stock
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Pk_Condition As String = "OPENI-"
    Private OpYrCode As String = ""

    Private WithEvents txtNumeric As New DataGridViewTextBoxEditingControl

    Private Sub clear()

        New_Entry = False

        lbl_IdNo.Text = ""
        lbl_IdNo.ForeColor = Color.Black

        pnl_Back.Enabled = True

        lbl_IdNo.Text = ""
        cbo_Ledger.Text = ""
        txt_OpAmount.Text = "0.00"
        cbo_CrDrType.Text = "Cr"

        txt_EmptyBeam.Text = ""
        txt_EmptyBags.Text = ""
        txt_EmptyCones.Text = ""

        cbo_Grid_CountName.Visible = False
        cbo_Grid_MillName.Visible = False
        cbo_Grid_YarnType.Visible = False
        cbo_PavuGrid_CountName.Visible = False

        cbo_Grid_CountName.Text = ""
        cbo_Grid_MillName.Text = ""
        cbo_Grid_YarnType.Text = ""
        cbo_PavuGrid_CountName.Text = ""

        dgv_YarnDetails.Rows.Clear()
        dgv_PavuDetails.Rows.Clear()

        dgv_YarnDetails_Total.Rows.Clear()
        dgv_YarnDetails_Total.Rows.Add()

        dgv_PavuDetails_Total.Rows.Clear()
        dgv_PavuDetails_Total.Rows.Add()

        tab_Main.SelectTab(0)
        dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(1)
        dgv_YarnDetails.CurrentCell.Selected = True

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

                da2 = New SqlClient.SqlDataAdapter("Select sum(voucher_amount) from voucher_details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and ledger_idno = " & Str(Val(idno)) & " and Entry_Identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                If dt2.Rows.Count > 0 Then
                    If IsDBNull(dt2.Rows(0).Item(0).ToString) = False Then
                        txt_OpAmount.Text = Trim(Format(Math.Abs(Val(dt2.Rows(0).Item(0).ToString)), "#########0.00"))
                        If Val(dt2.Rows(0).Item(0).ToString) >= 0 Then
                            cbo_CrDrType.Text = "Cr"
                        Else
                            cbo_CrDrType.Text = "Dr"
                        End If
                    End If
                End If
                dt2.Clear()

                da2 = New SqlClient.SqlDataAdapter("Select sum(Empty_Beam) as Op_Beam, sum(Empty_Bags) as Op_Bags, sum(Empty_Cones) as Op_Cones from Stock_Empty_BeamBagCone_Processing_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and (DeliveryTo_Idno = " & Str(Val(idno)) & " or ReceivedFrom_Idno = " & Str(Val(idno)) & ") and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                If dt2.Rows.Count > 0 Then
                    If IsDBNull(dt2.Rows(0).Item("Op_Beam").ToString) = False Then
                        txt_EmptyBeam.Text = Val(dt2.Rows(0).Item("Op_Beam").ToString)
                    End If
                    If IsDBNull(dt2.Rows(0).Item("Op_Bags").ToString) = False Then
                        txt_EmptyBags.Text = Val(dt2.Rows(0).Item("Op_Bags").ToString)
                    End If
                    If IsDBNull(dt2.Rows(0).Item("Op_Cones").ToString) = False Then
                        txt_EmptyCones.Text = Val(dt2.Rows(0).Item("Op_Cones").ToString)
                    End If
                End If
                dt2.Clear()

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Count_Name, c.Mill_Name from Stock_Yarn_Processing_Details a INNER JOIN Count_Head b on a.Count_IdNo = b.Count_IdNo INNER JOIN Mill_Head c on a.Mill_IdNo = c.Mill_IdNo where company_idno = " & Str(Val(lbl_Company.Tag)) & " and (DeliveryTo_Idno = " & Str(Val(idno)) & " or ReceivedFrom_Idno = " & Str(Val(idno)) & ") and a.Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' Order by a.sl_no", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_YarnDetails.Rows.Clear()
                Sno = 0

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_YarnDetails.Rows.Add()

                        Sno = Sno + 1
                        dgv_YarnDetails.Rows(n).Cells(0).Value = Val(Sno)
                        dgv_YarnDetails.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Count_Name").ToString
                        dgv_YarnDetails.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Yarn_Type").ToString
                        dgv_YarnDetails.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Mill_Name").ToString
                        dgv_YarnDetails.Rows(n).Cells(4).Value = Val(dt2.Rows(i).Item("Bags").ToString)
                        dgv_YarnDetails.Rows(n).Cells(5).Value = Val(dt2.Rows(i).Item("Cones").ToString)
                        dgv_YarnDetails.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Weight").ToString), "########0.000")
                        dgv_YarnDetails.Rows(n).Cells(7).Value = dt2.Rows(i).Item("set_no").ToString

                    Next i

                    'dgv_YarnDetails.CurrentCell.Selected = False

                End If

                TotalYarn_Calculation()

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.count_name from Stock_SizedPavu_Processing_Details a INNER JOIN Count_Head b ON a.Count_IdNo = b.Count_IdNo Where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Ledger_IdNo = " & Str(Val(idno)) & " and a.Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' Order by a.Sl_No", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_PavuDetails.Rows.Clear()
                Sno = 0

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_PavuDetails.Rows.Add()

                        Sno = Sno + 1
                        dgv_PavuDetails.Rows(n).Cells(0).Value = Val(Sno)
                        dgv_PavuDetails.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Set_No").ToString
                        dgv_PavuDetails.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Ends_Name").ToString
                        dgv_PavuDetails.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Count_Name").ToString
                        dgv_PavuDetails.Rows(n).Cells(4).Value = dt2.Rows(i).Item("Beam_No").ToString
                        dgv_PavuDetails.Rows(n).Cells(5).Value = dt2.Rows(i).Item("Noof_Pcs").ToString
                        dgv_PavuDetails.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Meters").ToString), "########0.00")
                        dgv_PavuDetails.Rows(n).Cells(7).Value = dt2.Rows(i).Item("Pavu_Delivery_Code").ToString

                    Next i

                    'dgv_PavuDetails.CurrentCell.Selected = False

                End If

                TotalPavu_Calculation()

                dt2.Clear()
                dt2.Dispose()
                da2.Dispose()

            End If

            dt1.Clear()
            dt1.Dispose()
            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If cbo_Ledger.Visible And cbo_Ledger.Enabled Then cbo_Ledger.Focus()

    End Sub

    Private Sub JobWork_Opening_Stock_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim Dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim dt5 As New DataTable
        Dim NoofComps As Integer

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Ledger.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Ledger.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_CountName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "COUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_CountName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_PavuGrid_CountName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "COUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_PavuGrid_CountName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_MillName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "MILL" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_MillName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            Common_Procedures.Master_Return.Form_Name = ""
            Common_Procedures.Master_Return.Control_Name = ""
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            Da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead order by Ledger_DisplayName", con)
            Da.Fill(Dt1)
            cbo_Ledger.DataSource = Dt1
            cbo_Ledger.DisplayMember = "Ledger_DisplayName"

            Da = New SqlClient.SqlDataAdapter("select mill_name from Mill_Head order by mill_name", con)
            Da.Fill(dt2)
            cbo_Grid_MillName.DataSource = dt2
            cbo_Grid_MillName.DisplayMember = "mill_name"

            Da = New SqlClient.SqlDataAdapter("select count_name from Count_Head order by count_name", con)
            Da.Fill(dt3)
            cbo_Grid_CountName.DataSource = dt3
            cbo_Grid_CountName.DisplayMember = "count_name"

            Da = New SqlClient.SqlDataAdapter("select yarn_type from YarnType_Head order by yarn_type", con)
            Da.Fill(dt4)
            cbo_Grid_YarnType.DataSource = dt4
            cbo_Grid_YarnType.DisplayMember = "yarn_type"

            Da = New SqlClient.SqlDataAdapter("select count_name from Count_Head order by count_name", con)
            Da.Fill(dt5)
            cbo_PavuGrid_CountName.DataSource = dt5
            cbo_PavuGrid_CountName.DisplayMember = "count_name"

            If FrmLdSTS = True Then

                lbl_Company.Text = ""
                lbl_Company.Tag = 0
                Common_Procedures.CompIdNo = 0

                Me.Text = ""

                Da = New SqlClient.SqlDataAdapter("select count(*) from Company_Head where Company_IdNo <> 0", con)
                Dt = New DataTable
                Da.Fill(Dt)

                NoofComps = 0
                If Dt.Rows.Count > 0 Then
                    If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                        NoofComps = Val(Dt.Rows(0)(0).ToString)
                    End If
                End If
                Dt.Clear()

                If Val(NoofComps) = 1 Then

                    Da = New SqlClient.SqlDataAdapter("select Company_IdNo, Company_Name from Company_Head where Company_IdNo <> 0 Order by Company_IdNo ", con)
                    Dt = New DataTable
                    Da.Fill(Dt)

                    If Dt.Rows.Count > 0 Then
                        If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                            Common_Procedures.CompIdNo = Val(Dt.Rows(0)(0).ToString)
                        End If
                    End If
                    Dt.Clear()

                Else

                    Dim f As New Company_Selection
                    f.ShowDialog()

                End If

                If Val(Common_Procedures.CompIdNo) <> 0 Then

                    Da = New SqlClient.SqlDataAdapter("select Company_IdNo, Company_Name from Company_Head where Company_IdNo = " & Str(Val(Common_Procedures.CompIdNo)), con)
                    Dt = New DataTable
                    Da.Fill(Dt)

                    If Dt.Rows.Count > 0 Then
                        If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                            lbl_Company.Tag = Val(Dt.Rows(0)(0).ToString)
                            lbl_Company.Text = Trim(Dt.Rows(0)(1).ToString)
                            Me.Text = Trim(Dt.Rows(0)(1).ToString)
                        End If
                    End If
                    Dt.Clear()

                    new_record()

                Else
                    'MessageBox.Show("Invalid Company Selection", "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Me.Close()
                    Exit Sub

                End If

            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        FrmLdSTS = False

    End Sub

    Private Sub JobWork_Opening_Stock_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Text = ""

        con.Open()

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        OpYrCode = Microsoft.VisualBasic.Left(Trim(Common_Procedures.FnRange), 4)
        OpYrCode = Trim(Mid(Val(OpYrCode) - 1, 3, 2)) & "-" & Trim(Microsoft.VisualBasic.Right(OpYrCode, 2))

        FrmLdSTS = True
        new_record()

    End Sub

    Private Sub JobWork_Opening_Stock_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
    End Sub

    Private Sub JobWork_Opening_Stock_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        Try
            If Asc(e.KeyChar) = 27 Then
                Close_Form()
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim tr As SqlClient.SqlTransaction
        Dim cmd As New SqlClient.SqlCommand
        Dim da As SqlClient.SqlDataAdapter
        Dim dt As DataTable
        Dim NewCode As String
        Dim LedName As String

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Jobwork_OpeningStock, "~L~") = 0 And InStr(Common_Procedures.UR.Jobwork_OpeningStock, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

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

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(Val(lbl_IdNo.Text)) & "/" & Trim(OpYrCode)

        da = New SqlClient.SqlDataAdapter("select count(*) from Stock_SizedPavu_Processing_Details where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Pavu_Delivery_Code <> ''", con)
        dt = New DataTable
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                If Val(dt.Rows(0)(0).ToString) > 0 Then
                    MessageBox.Show("Pavu Delivered", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If

        'da = New SqlClient.SqlDataAdapter("select sum(Delivered_Weight) from Stock_BabyCone_Processing_Details where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)
        'dt = New DataTable
        'da.Fill(dt)
        'If dt.Rows.Count > 0 Then
        '    If IsDBNull(dt.Rows(0)(0).ToString) = False Then
        '        If Val(dt.Rows(0)(0).ToString) > 0 Then
        '            MessageBox.Show("BabyCone Delivered", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '            Exit Sub
        '        End If
        '    End If
        'End If

        tr = con.BeginTransaction

        Try

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.CommandText = "Delete from Voucher_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Ledger_IdNo = " & Str(Val(lbl_IdNo.Text)) & " and Entry_Identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_SizedPavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Empty_BeamBagCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            'cmd.CommandText = "Delete from Stock_BabyCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            'cmd.ExecuteNonQuery()

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
            cmd.CommandText = "select top 1 Ledger_IdNo from Ledger_Head where Ledger_IdNo <> 0 Order by Ledger_IdNo"
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

            da = New SqlClient.SqlDataAdapter("select top 1 Ledger_IdNo from Ledger_Head where Ledger_IdNo > " & Str(OrdByNo) & " Order by Ledger_IdNo", con)
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
            cmd.CommandText = "select top 1 Ledger_IdNo from Ledger_Head where ledger_idno < " & Str(Val(OrdByNo)) & " Order by Ledger_IdNo desc"

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
        Dim da As New SqlClient.SqlDataAdapter("select top 1 Ledger_IdNo from Ledger_Head where ledger_idno <> 0 Order by Ledger_IdNo desc", con)
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
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim dt2 As New DataTable
        Dim NewID As Integer = 0

        Try

            clear()

            New_Entry = True

            da = New SqlClient.SqlDataAdapter("select max(ledger_idno) from Ledger_Head where ledger_idno <> 0", con)
            da.Fill(dt)

            NewID = 0
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    NewID = Val(dt.Rows(0)(0).ToString)
                End If
            End If

            If Val(NewID) <= 100 Then NewID = 100

            lbl_IdNo.Text = Val(NewID) + 1

            lbl_IdNo.ForeColor = Color.Red

            If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try


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
        Dim Cnt_ID As Integer, pCnt_ID As Integer, pEdsCnt_ID As Integer
        Dim Mil_ID As Integer
        Dim vSetNo As String
        Dim vSetCd As String
        Dim Selc_SetCode As String
        Dim Dup_SetCd As String = ""
        Dim Dup_SetNoBmNo As String = ""

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        ' If Common_Procedures.UserRight_Check(Common_Procedures.UR.Jobwork_OpeningStock, New_Entry) = False Then Exit Sub

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Trim(cbo_Ledger.Text) = "" Then
            MessageBox.Show("Invalid Ledger Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Ledger.Enabled Then cbo_Ledger.Focus()
            Exit Sub
        End If

        If Val(lbl_IdNo.Text) = 0 Then
            MessageBox.Show("Invalid Ledger Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Ledger.Enabled Then cbo_Ledger.Focus()
            Exit Sub
        End If

        LedName = Common_Procedures.Ledger_IdNoToName(con, Val(lbl_IdNo.Text))
        If Trim(LedName) = "" Then
            MessageBox.Show("Invalid Ledger Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Ledger.Enabled Then cbo_Ledger.Focus()
            Exit Sub
        End If

        If Val(txt_OpAmount.Text) <> 0 And Trim(cbo_CrDrType.Text) = "" Then
            MessageBox.Show("Invalid Cr/Dr", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_CrDrType.Enabled Then cbo_CrDrType.Focus()
            Exit Sub
        End If

        For i = 0 To dgv_YarnDetails.RowCount - 1

            If Val(dgv_YarnDetails.Rows(i).Cells(6).Value) <> 0 Then

                Sno = Sno + 1

                Cnt_ID = Common_Procedures.Count_NameToIdNo(con, Trim(dgv_YarnDetails.Rows(i).Cells(1).Value))
                If Val(Cnt_ID) = 0 Then
                    MessageBox.Show("Invalid Count Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    If dgv_YarnDetails.Enabled And dgv_YarnDetails.Visible Then dgv_YarnDetails.Focus()
                    dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(i).Cells(1)
                    dgv_YarnDetails.CurrentCell.Selected = True
                    Exit Sub
                End If

                If Trim(dgv_YarnDetails.Rows(i).Cells(2).Value) = "" Then
                    MessageBox.Show("Invalid Yarn Type", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    If dgv_YarnDetails.Enabled And dgv_YarnDetails.Visible Then dgv_YarnDetails.Focus()
                    dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(i).Cells(2)
                    dgv_YarnDetails.CurrentCell.Selected = True
                    Exit Sub
                End If

                Mil_ID = Common_Procedures.Mill_NameToIdNo(con, Trim(dgv_YarnDetails.Rows(i).Cells(3).Value))
                If Val(Mil_ID) = 0 Then
                    MessageBox.Show("Invalid Mill Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    If dgv_YarnDetails.Enabled And dgv_YarnDetails.Visible Then dgv_YarnDetails.Focus()
                    dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(i).Cells(3)
                    dgv_YarnDetails.CurrentCell.Selected = True
                    Exit Sub
                End If

                If Trim(UCase(dgv_YarnDetails.Rows(i).Cells(2).Value)) = "BABY" And Trim(dgv_YarnDetails.Rows(i).Cells(7).Value) = "" Then
                    MessageBox.Show("Invalid SetNo for BabyYarn", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    If dgv_YarnDetails.Enabled And dgv_YarnDetails.Visible Then dgv_YarnDetails.Focus()
                    dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(i).Cells(7)
                    dgv_YarnDetails.CurrentCell.Selected = True
                    Exit Sub
                End If

                If Trim(dgv_YarnDetails.Rows(i).Cells(7).Value) <> "" Then
                    If InStr(1, Trim(dgv_PavuDetails.Rows(i).Cells(7).Value), " ") > 0 Then
                        MessageBox.Show("Invalid Set No, Spaces not allowed in SetNo", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dgv_PavuDetails.Enabled And dgv_PavuDetails.Visible Then dgv_PavuDetails.Focus()
                        dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(i).Cells(7)
                        dgv_PavuDetails.CurrentCell.Selected = True
                        Exit Sub
                    End If
                End If

                If Trim(UCase(dgv_YarnDetails.Rows(i).Cells(2).Value)) = "BABY" Then

                    If InStr(1, Trim(UCase(Dup_SetCd)), "~" & Trim(UCase(dgv_YarnDetails.Rows(i).Cells(7).Value)) & "~") > 0 Then
                        MessageBox.Show("Duplicate SetNo for BabyYarn", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dgv_YarnDetails.Enabled And dgv_YarnDetails.Visible Then dgv_YarnDetails.Focus()
                        dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(i).Cells(7)
                        dgv_YarnDetails.CurrentCell.Selected = True
                        Exit Sub
                    End If

                    Dup_SetCd = Trim(Dup_SetCd) & "~" & Trim(UCase(dgv_YarnDetails.Rows(i).Cells(7).Value)) & "~"

                End If



            End If

        Next


        For i = 0 To dgv_PavuDetails.RowCount - 1

            If Val(dgv_PavuDetails.Rows(i).Cells(6).Value) <> 0 Then

                If Trim(dgv_PavuDetails.Rows(i).Cells(1).Value) = "" Then
                    MessageBox.Show("Invalid Set No", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    If dgv_PavuDetails.Enabled And dgv_PavuDetails.Visible Then dgv_PavuDetails.Focus()
                    dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(i).Cells(1)
                    dgv_PavuDetails.CurrentCell.Selected = True
                    Exit Sub
                End If

                If InStr(1, Trim(dgv_PavuDetails.Rows(i).Cells(1).Value), " ") > 0 Then
                    MessageBox.Show("Invalid Set No, Spaces not allowed in SetNo", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    If dgv_PavuDetails.Enabled And dgv_PavuDetails.Visible Then dgv_PavuDetails.Focus()
                    dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(i).Cells(1)
                    dgv_PavuDetails.CurrentCell.Selected = True
                    Exit Sub
                End If

                If Val(dgv_PavuDetails.Rows(i).Cells(2).Value) = 0 Then
                    MessageBox.Show("Invalid Ends", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    If dgv_PavuDetails.Enabled And dgv_PavuDetails.Visible Then dgv_PavuDetails.Focus()
                    dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(i).Cells(2)
                    dgv_PavuDetails.CurrentCell.Selected = True
                    Exit Sub
                End If

                If Trim(dgv_PavuDetails.Rows(i).Cells(3).Value) = "" Then
                    MessageBox.Show("Invalid Count", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    If dgv_PavuDetails.Enabled And dgv_PavuDetails.Visible Then dgv_PavuDetails.Focus()
                    dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(i).Cells(3)
                    dgv_PavuDetails.CurrentCell.Selected = True
                    Exit Sub
                End If

                pCnt_ID = Common_Procedures.Count_NameToIdNo(con, Trim(dgv_PavuDetails.Rows(i).Cells(3).Value))
                pEdsCnt_ID = Val(Common_Procedures.get_FieldValue(con, "EndsCount_Head", "EndsCount_IdNo", "(Ends_Name = " & Str(Val(dgv_PavuDetails.Rows(i).Cells(2).Value)) & " and Count_IdNo = " & Str(Val(pCnt_ID)) & ")"))
                If Val(pEdsCnt_ID) = 0 Then
                    MessageBox.Show("Invalid Ends/Count", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    If dgv_PavuDetails.Enabled And dgv_PavuDetails.Visible Then
                        dgv_PavuDetails.Focus()
                        dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(i).Cells(2)
                        dgv_PavuDetails.CurrentCell.Selected = True
                        Exit Sub
                    End If
                End If

                If Trim(dgv_PavuDetails.Rows(i).Cells(4).Value) = "" Then
                    MessageBox.Show("Invalid Beam No", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    If dgv_PavuDetails.Enabled And dgv_PavuDetails.Visible Then dgv_PavuDetails.Focus()
                    dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(i).Cells(4)
                    dgv_PavuDetails.CurrentCell.Selected = True
                    Exit Sub
                End If

                If InStr(1, Trim(dgv_PavuDetails.Rows(i).Cells(4).Value), " ") > 0 Then
                    MessageBox.Show("Invalid Beam No, Spaces not allowed in SetNo", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    If dgv_PavuDetails.Enabled And dgv_PavuDetails.Visible Then dgv_PavuDetails.Focus()
                    dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(i).Cells(4)
                    dgv_PavuDetails.CurrentCell.Selected = True
                    Exit Sub
                End If

                If InStr(1, Trim(UCase(Dup_SetNoBmNo)), "~" & Trim(UCase(dgv_YarnDetails.Rows(i).Cells(1).Value)) & "-" & Trim(UCase(dgv_YarnDetails.Rows(i).Cells(4).Value)) & "~") > 0 Then
                    MessageBox.Show("Duplicate BeamNo for set no. " & Trim(dgv_YarnDetails.Rows(i).Cells(1).Value), "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    If dgv_YarnDetails.Enabled And dgv_YarnDetails.Visible Then dgv_YarnDetails.Focus()
                    dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(i).Cells(4)
                    dgv_YarnDetails.CurrentCell.Selected = True
                    Exit Sub
                End If

                Dup_SetNoBmNo = Trim(Dup_SetNoBmNo) & "~" & Trim(UCase(dgv_YarnDetails.Rows(i).Cells(1).Value)) & "-" & Trim(UCase(dgv_YarnDetails.Rows(i).Cells(4).Value)) & "~"

            End If

        Next

        tr = con.BeginTransaction

        Try

            OpDate = CDate("01-04-" & Microsoft.VisualBasic.Left(Trim(Common_Procedures.FnRange), 4))
            OpDate = DateAdd(DateInterval.Day, -1, OpDate)

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@OpeningDate", OpDate)

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(Val(lbl_IdNo.Text)) & "/" & Trim(OpYrCode)

            cmd.CommandText = "Delete from Voucher_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Ledger_IdNo = " & Str(Val(lbl_IdNo.Text)) & " and Entry_Identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            Sno = 0

            If Val(txt_OpAmount.Text) <> 0 Then

                VouAmt = Val(txt_OpAmount.Text)
                If Trim(UCase(cbo_CrDrType.Text)) = "DR" Then VouAmt = -1 * VouAmt

                Sno = Sno + 1

                cmd.CommandText = "Insert into Voucher_Details(Voucher_Code, For_OrderByCode, Company_IdNo, Voucher_No, For_OrderBy, Voucher_Type, Voucher_Date, Sl_No, Ledger_IdNo, Voucher_Amount, Narration, Year_For_Report, Entry_Identification) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_IdNo.Text)) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(Val(lbl_IdNo.Text)) & "', " & Str(Val(lbl_IdNo.Text)) & ", 'Opng', @OpeningDate, " & Str(Val(Sno)) & ", " & Str(Val(lbl_IdNo.Text)) & ", " & Str(Val(VouAmt)) & ", 'Opening', " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(Pk_Condition) & Trim(NewCode) & "')"
                cmd.ExecuteNonQuery()

            End If

            cmd.CommandText = "Delete from Stock_Empty_BeamBagCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            If Val(txt_EmptyBeam.Text) <> 0 Then

                Dlv_IdNo = 0
                Rec_IdNo = 0

                If Val(txt_EmptyBeam.Text) < 0 Then
                    Dlv_IdNo = Val(lbl_IdNo.Text)
                Else
                    Rec_IdNo = Val(lbl_IdNo.Text)
                End If

                cmd.CommandText = "Insert into Stock_Empty_BeamBagCone_Processing_Details(Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Party_Bill_No, Sl_No, Beam_Width_IdNo, Empty_Beam, Empty_Bags, Empty_Cones, Particulars) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_IdNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_IdNo.Text))) & ", @OpeningDate, " & Str(Val(Dlv_IdNo)) & ", " & Str(Val(Rec_IdNo)) & ", '', 1, 0, " & Str(Val(txt_EmptyBeam.Text)) & ", 0, 0, '' )"
                cmd.ExecuteNonQuery()
            End If

            If Val(txt_EmptyBags.Text) <> 0 Then

                Dlv_IdNo = 0
                Rec_IdNo = 0

                If Val(txt_EmptyBags.Text) < 0 Then
                    Dlv_IdNo = Val(lbl_IdNo.Text)
                Else
                    Rec_IdNo = Val(lbl_IdNo.Text)
                End If

                cmd.CommandText = "Insert into Stock_Empty_BeamBagCone_Processing_Details(Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Party_Bill_No, Sl_No, Beam_Width_IdNo, Empty_Beam, Empty_Bags, Empty_Cones, Particulars) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_IdNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_IdNo.Text))) & ", @OpeningDate, " & Str(Val(Dlv_IdNo)) & ", " & Str(Val(Rec_IdNo)) & ", '', 2, 0, 0, " & Str(Val(txt_EmptyBags.Text)) & ", 0, '' )"
                cmd.ExecuteNonQuery()
            End If

            If Val(txt_EmptyCones.Text) <> 0 Then

                Dlv_IdNo = 0
                Rec_IdNo = 0

                If Val(txt_EmptyCones.Text) < 0 Then
                    Dlv_IdNo = Val(lbl_IdNo.Text)
                Else
                    Rec_IdNo = Val(lbl_IdNo.Text)
                End If

                cmd.CommandText = "Insert into Stock_Empty_BeamBagCone_Processing_Details(Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Party_Bill_No, Sl_No, Beam_Width_IdNo, Empty_Beam, Empty_Bags, Empty_Cones, Particulars) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_IdNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_IdNo.Text))) & ", @OpeningDate, " & Str(Val(Dlv_IdNo)) & ", " & Str(Val(Rec_IdNo)) & ", '', 3, 0, 0, 0, " & Str(Val(txt_EmptyCones.Text)) & ", '' )"
                cmd.ExecuteNonQuery()

            End If

            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            'cmd.CommandText = "Delete from Stock_BabyCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Delivered_Bags = 0 and Delivered_Cones = 0 and Delivered_Weight = 0"
            'cmd.ExecuteNonQuery()

            Sno = 0
            For i = 0 To dgv_YarnDetails.RowCount - 1

                If Val(dgv_YarnDetails.Rows(i).Cells(6).Value) <> 0 Then

                    Sno = Sno + 1

                    Cnt_ID = Common_Procedures.Count_NameToIdNo(con, Trim(dgv_YarnDetails.Rows(i).Cells(1).Value), tr)

                    Mil_ID = Common_Procedures.Mill_NameToIdNo(con, Trim(dgv_YarnDetails.Rows(i).Cells(3).Value), tr)

                    vSetCd = ""
                    vSetNo = ""
                    Selc_SetCode = ""
                    If Trim(UCase(dgv_YarnDetails.Rows(i).Cells(2).Value)) = "BABY" Then
                        vSetNo = Trim(dgv_YarnDetails.Rows(i).Cells(7).Value)
                        If Trim(vSetNo) <> "" Then
                            vSetCd = Trim(Val(lbl_Company.Tag)) & "-" & Trim(vSetNo) & "/" & Trim(OpYrCode)
                            Selc_SetCode = Trim(vSetNo) & "/" & Trim(OpYrCode) & "/" & Trim(Val(lbl_Company.Tag))
                        End If
                    End If

                    Dlv_IdNo = 0
                    Rec_IdNo = 0
                    If Val(txt_EmptyBags.Text) < 0 Then
                        Dlv_IdNo = Val(lbl_IdNo.Text)
                    Else
                        Rec_IdNo = Val(lbl_IdNo.Text)
                    End If

                    cmd.CommandText = "Insert into Stock_Yarn_Processing_Details(Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Party_Bill_No, Sl_No, Count_IdNo, Yarn_Type, Mill_IdNo, Bags, Cones, Weight, Particulars, Set_Code, Set_No) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(Val(lbl_IdNo.Text)) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_IdNo.Text))) & ", @OpeningDate, " & Str(Val(Dlv_IdNo)) & ", " & Str(Val(Rec_IdNo)) & ", '', " & Str(Val(Sno)) & ", " & Str(Val(Cnt_ID)) & ", '" & Trim(dgv_YarnDetails.Rows(i).Cells(2).Value) & "', " & Str(Val(Mil_ID)) & ", " & Str(Val(dgv_YarnDetails.Rows(i).Cells(4).Value)) & ", " & Str(Val(dgv_YarnDetails.Rows(i).Cells(5).Value)) & ", " & Str(Val(dgv_YarnDetails.Rows(i).Cells(6).Value)) & ", '', '" & Trim(vSetCd) & "', '" & Trim(vSetNo) & "')"
                    cmd.ExecuteNonQuery()

                    'If Trim(UCase(dgv_YarnDetails.Rows(i).Cells(2).Value)) = "BABY" Then
                    '    Nr = 0
                    '    cmd.CommandText = "Update Stock_BabyCone_Processing_Details set " & _
                    '                " Baby_Bags = " & Str(Val(dgv_YarnDetails.Rows(i).Cells(4).Value)) & ", " & _
                    '                " Baby_Cones = " & Str(Val(dgv_YarnDetails.Rows(i).Cells(5).Value)) & ", " & _
                    '                " Baby_Weight = " & Str(Val(dgv_YarnDetails.Rows(i).Cells(6).Value)) & " " & _
                    '                " Where Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and " & _
                    '                " Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Set_Code = '" & Trim(vSetCd) & "'"

                    '    Nr = cmd.ExecuteNonQuery()

                    '    If Nr = 0 Then

                    '        cmd.CommandText = "Insert into Stock_BabyCone_Processing_Details( Reference_Code, " _
                    '                  & "Company_IdNo, Reference_No, For_OrderBy, Reference_Date, Ledger_IdNo, " _
                    '                  & "Set_Code, Set_No, setcode_forSelection, " _
                    '                  & "Ends_Name, Mill_Idno, Count_IdNo, Bag_No, Baby_Bags, " _
                    '                  & "Baby_Cones, Baby_Weight, Delivered_Bags, Delivered_Cones, Delivered_Weight) Values ( '" _
                    '                  & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(Val(lbl_IdNo.Text)) & "', " _
                    '                  & Str(Common_Procedures.OrderBy_CodeToValue(Trim(lbl_IdNo.Text))) & ", @OpeningDate, " _
                    '                  & Str(Val(lbl_IdNo.Text)) & ", '" & Trim(vSetCd) & "', '" & Trim(vSetNo) & "', '" & Trim(Selc_SetCode) & "', '', " & Str(Mil_ID) & ", " & Str(Cnt_ID) & ", 1, " _
                    '                  & Str(Val(dgv_YarnDetails.Rows(i).Cells(4).Value)) & ", " & Str(Val(dgv_YarnDetails.Rows(i).Cells(5).Value)) & ", " _
                    '                  & Str(Val(dgv_YarnDetails.Rows(i).Cells(6).Value)) & ", 0, 0, 0)"

                    '        cmd.ExecuteNonQuery()

                    '    End If

                    'End If

                End If

            Next

            cmd.CommandText = "Delete from Stock_SizedPavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Pavu_Delivery_Code = ''"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Pavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            Sno = 0

            For i = 0 To dgv_PavuDetails.RowCount - 1

                If Trim(dgv_PavuDetails.Rows(i).Cells(1).Value) <> "" And Trim(dgv_PavuDetails.Rows(i).Cells(4).Value) <> "" And Val(dgv_PavuDetails.Rows(i).Cells(6).Value) <> 0 Then

                    Sno = Sno + 1

                    pCnt_ID = Common_Procedures.Count_NameToIdNo(con, Trim(dgv_PavuDetails.Rows(i).Cells(3).Value), tr)

                    vSetCd = ""
                    Selc_SetCode = ""
                    vSetNo = Trim(dgv_PavuDetails.Rows(i).Cells(1).Value)
                    If Trim(vSetNo) <> "" Then
                        vSetCd = Trim(Val(lbl_Company.Tag)) & "-" & Trim(vSetNo) & "/" & Trim(OpYrCode)
                        Selc_SetCode = Trim(vSetNo) & "/" & Trim(OpYrCode) & "/" & Trim(Val(lbl_Company.Tag))
                    End If

                    pEdsCnt_ID = Val(Common_Procedures.get_FieldValue(con, "EndsCount_Head", "EndsCount_IdNo", "(Ends_Name = " & Str(Val(dgv_PavuDetails.Rows(i).Cells(2).Value)) & " and Count_IdNo = " & Str(Val(pCnt_ID)) & ")", , tr))

                    Nr = 0
                    cmd.CommandText = "Update Stock_SizedPavu_Processing_Details set Reference_Date = @OpeningDate, Ledger_IdNo = " & Str(Val(lbl_IdNo.Text)) & ", Ends_Name = '" & Trim(dgv_PavuDetails.Rows(i).Cells(2).Value) & "', Count_IdNo = " & Str(Val(pCnt_ID)) & ", EndsCount_IdNo = " & Str(Val(pEdsCnt_ID)) & ", Mill_IdNo = 0, Beam_Width_Idno = 0, Sizing_SlNo = 0, Sl_No = " & Str(Val(Sno)) & ", ForOrderBy_BeamNo = " & Str(Val(Common_Procedures.OrderBy_CodeToValue(dgv_PavuDetails.Rows(i).Cells(4).Value))) & ", Gross_Weight = 0, Tare_Weight = 0, Net_Weight = 0, Noof_Pcs = " & Str(Val(dgv_PavuDetails.Rows(i).Cells(5).Value)) & ", Meters_Pc = 0, Meters = " & Str(Val(dgv_PavuDetails.Rows(i).Cells(6).Value)) & ", Warp_Meters = 0 " & _
                                        " Where Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Set_Code = '" & Trim(vSetCd) & "' and Beam_No = '" & Trim(dgv_PavuDetails.Rows(i).Cells(4).Value) & "'"
                    Nr = cmd.ExecuteNonQuery()

                    If Nr = 0 Then
                        cmd.CommandText = "Insert into Stock_SizedPavu_Processing_Details(Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, Ledger_IdNo, Set_Code, Set_No, setcode_forSelection, Ends_Name, count_idno, EndsCount_IdNo, Mill_IdNo, Beam_Width_Idno, Sizing_SlNo, Sl_No, Beam_No, ForOrderBy_BeamNo, Gross_Weight, Tare_Weight, Net_Weight, Noof_Pcs, Meters_Pc, Meters, Warp_Meters, Pavu_Delivery_Code, Pavu_Delivery_Increment, DeliveryTo_Name )" & _
                                        " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(Val(lbl_IdNo.Text)) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_IdNo.Text))) & ", @OpeningDate, " & Str(Val(lbl_IdNo.Text)) & ", '" & Trim(vSetCd) & "', '" & Trim(vSetNo) & "', '" & Trim(Selc_SetCode) & "', '" & Trim(dgv_PavuDetails.Rows(i).Cells(2).Value) & "', " & Str(Val(pCnt_ID)) & ", " & Str(Val(pEdsCnt_ID)) & ", " & Str(Val(Mil_ID)) & ", 0, 0, " & Str(Val(Sno)) & ", '" & Trim(dgv_PavuDetails.Rows(i).Cells(4).Value) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(dgv_PavuDetails.Rows(i).Cells(4).Value))) & ", 0, 0, 0, " & Str(Val(dgv_PavuDetails.Rows(i).Cells(5).Value)) & ", 0, " & Str(Val(dgv_PavuDetails.Rows(i).Cells(6).Value)) & ", 0, '', 0, '')"
                        cmd.ExecuteNonQuery()
                    End If

                    cmd.CommandText = "Insert into Stock_Pavu_Processing_Details(Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Cloth_Idno, Entry_ID, Party_Bill_No, Particulars, Sl_No, EndsCount_IdNo, Sized_Beam, Meters) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_IdNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_IdNo.Text))) & ", @OpeningDate, 0, " & Str(Val(lbl_IdNo.Text)) & ", 0, 'OPENING', '', '', " & Str(Val(Sno)) & ", " & Str(Val(pEdsCnt_ID)) & ", 1, " & Str(Val(dgv_PavuDetails.Rows(i).Cells(6).Value)) & " )"
                    cmd.ExecuteNonQuery()

                End If

            Next

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

    Private Sub cbo_CrDrType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_CrDrType.GotFocus
        With cbo_CrDrType
            .BackColor = Color.lime
            .ForeColor = Color.Blue
            .SelectionStart = 0
            .SelectionLength = .Text.Length
        End With
    End Sub

    Private Sub cbo_CrDrType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_CrDrType.KeyDown
        Try
            With cbo_CrDrType
                If e.KeyValue = 38 And .DroppedDown = False Then
                    e.Handled = True
                    txt_OpAmount.Focus()
                    'SendKeys.Send("+{TAB}")
                ElseIf e.KeyValue = 40 And .DroppedDown = False Then
                    e.Handled = True
                    txt_EmptyBeam.Focus()
                ElseIf e.KeyValue <> 13 And .DroppedDown = False Then
                    .DroppedDown = True
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_CrDrType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_CrDrType.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Condt As String
        Dim FindStr As String
        Dim indx As Integer

        Try

            With cbo_CrDrType

                If Asc(e.KeyChar) <> 27 Then

                    If Asc(e.KeyChar) = 13 Then

                        If Trim(.Text) <> "" Then
                            If .DroppedDown = True Then
                                If Trim(.SelectedText) <> "" Then
                                    .Text = .GetItemText(.SelectedItem)
                                    '.Text = .SelectedText
                                Else
                                    If .Items.Count > 0 Then
                                        .SelectedIndex = 0
                                        .SelectedItem = .Items(0)
                                        .Text = .GetItemText(.SelectedItem)
                                    End If
                                End If
                            End If
                        End If

                        txt_EmptyBeam.Focus()

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

                        indx = .FindString(FindStr)

                        If indx <> -1 Then
                            .SelectedText = ""
                            .SelectedIndex = indx

                            .SelectionStart = FindStr.Length
                            .SelectionLength = .Text.Length
                            e.Handled = True

                        Else
                            e.Handled = True

                        End If

                    End If

                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_CrDrType_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_CrDrType.LostFocus
        With cbo_CrDrType
            .BackColor = Color.White
            .ForeColor = Color.Black
        End With
    End Sub

    Private Sub cbo_Ledger_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.GotFocus
        With cbo_Ledger
            .BackColor = Color.lime
            .ForeColor = Color.Blue
            .SelectionStart = 0
            .SelectionLength = .Text.Length
        End With
    End Sub

    Private Sub cbo_Ledger_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.LostFocus
        With cbo_Ledger
            .BackColor = Color.White
            .ForeColor = Color.Black
        End With
    End Sub


    Private Sub cbo_Ledger_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyDown
        Try
            With cbo_Ledger
                If e.KeyValue = 38 And .DroppedDown = False Then
                    e.Handled = True
                    'cbo_CrDrType.Focus()
                    'SendKeys.Send("+{TAB}")
                ElseIf e.KeyValue = 40 And .DroppedDown = False Then
                    e.Handled = True
                    txt_OpAmount.Focus()
                    'SendKeys.Send("{TAB}")
                ElseIf e.KeyValue <> 13 And e.KeyValue <> 17 And e.KeyValue <> 27 And .DroppedDown = False Then
                    .DroppedDown = True
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_Ledger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ledger.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Condt As String
        Dim FindStr As String
        Dim LedIdNo As Integer

        Try

            With cbo_Ledger

                If Asc(e.KeyChar) <> 27 Then

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

                        LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
                        If Val(LedIdNo) <> 0 Then
                            move_record(LedIdNo)
                        End If

                        txt_OpAmount.Focus()

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

                        If Trim(FindStr) <> "" Then
                            Condt = " Where Ledger_DisplayName like '" & FindStr & "%' or Ledger_DisplayName like '% " & FindStr & "%' "
                        End If

                        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead " & Condt & " order by Ledger_DisplayName", con)
                        da.Fill(dt)

                        .DataSource = dt
                        .DisplayMember = "Ledger_DisplayName"

                        .Text = FindStr

                        .SelectionStart = FindStr.Length

                        e.Handled = True

                    End If

                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_Ledger_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Ledger.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub txt_OpAmount_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_OpAmount.KeyDown
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub txt_OpAmount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_OpAmount.KeyPress
        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub txt_OpAmount_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_OpAmount.KeyUp
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        save_record()
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub cbo_Grid_CountName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_CountName.GotFocus

        With cbo_Grid_CountName
            .BackColor = Color.lime
            .ForeColor = Color.Blue
            .SelectionStart = 0
            .SelectionLength = cbo_Grid_CountName.Text.Length

            cbo_Grid_CountName.Tag = cbo_Grid_CountName.Text
        End With

    End Sub

    Private Sub cbo_Grid_CountName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_CountName.KeyDown
        Try
            With cbo_Grid_CountName
                If e.KeyValue = 38 And .DroppedDown = False Then
                    e.Handled = True

                    With dgv_YarnDetails
                        If Val(.CurrentCell.RowIndex) <= 0 Then
                            txt_EmptyCones.Focus()

                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(7)
                            .CurrentCell.Selected = True
                            .Focus()

                        End If
                    End With
                    cbo_Grid_CountName.Visible = False
                    cbo_Grid_CountName.Text = ""

                ElseIf e.KeyValue = 40 And .DroppedDown = False Then

                    e.Handled = True
                    With dgv_YarnDetails
                        If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then
                            tab_Main.SelectTab(1)
                            dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(0).Cells(1)
                            dgv_PavuDetails.CurrentCell.Selected = True

                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                            .CurrentCell.Selected = True
                            .Focus()

                        End If
                    End With
                    .Visible = False
                    .Text = ""

                    'SendKeys.Send("{TAB}")
                ElseIf e.KeyValue <> 13 And e.KeyValue <> 17 And e.KeyValue <> 27 And .DroppedDown = False Then
                    .DroppedDown = True

                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT ITEM...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Grid_CountName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_CountName.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Condt As String
        Dim FindStr As String

        Try

            With cbo_Grid_CountName

                If Asc(e.KeyChar) <> 27 Then

                    If Asc(e.KeyChar) = 13 Then

                        With cbo_Grid_CountName
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
                        End With

                        With dgv_YarnDetails
                            .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_CountName.Text)
                            If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then
                                tab_Main.SelectTab(1)
                                dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(0).Cells(1)
                                dgv_PavuDetails.CurrentCell.Selected = True

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                                .CurrentCell.Selected = True
                                .Focus()

                            End If
                        End With
                        .Visible = False
                        .Text = ""

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

                        If Trim(FindStr) <> "" Then
                            Condt = " Where count_name like '" & Trim(FindStr) & "%' or count_name like '% " & Trim(FindStr) & "%' "
                        End If

                        da = New SqlClient.SqlDataAdapter("select count_name from Count_Head " & Condt & " order by count_name", con)
                        da.Fill(dt)

                        .DataSource = dt
                        .DisplayMember = "count_name"

                        .Text = FindStr

                        .SelectionStart = FindStr.Length

                        e.Handled = True

                    End If

                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_Grid_CountName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_CountName.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New Count_Creation

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

        With cbo_Grid_MillName
            .BackColor = Color.lime
            .ForeColor = Color.Blue
            .SelectionStart = 0
            .SelectionLength = cbo_Grid_MillName.Text.Length

            cbo_Grid_MillName.Tag = cbo_Grid_MillName.Text
        End With

    End Sub

    Private Sub cbo_Grid_MillName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_MillName.KeyDown
        Try
            With cbo_Grid_MillName
                If e.KeyValue = 38 And .DroppedDown = False Then
                    e.Handled = True

                    With dgv_YarnDetails
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)
                        .CurrentCell.Selected = True
                        .Focus()
                    End With
                    .Visible = False
                    .Text = ""

                    'SendKeys.Send("+{TAB}")
                ElseIf e.KeyValue = 40 And .DroppedDown = False Then
                    e.Handled = True

                    With dgv_YarnDetails
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                        .CurrentCell.Selected = True
                        .Focus()
                    End With
                    .Visible = False
                    .Text = ""

                    'SendKeys.Send("{TAB}")
                ElseIf e.KeyValue <> 13 And e.KeyValue <> 17 And e.KeyValue <> 27 And .DroppedDown = False Then
                    .DroppedDown = True

                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT ITEM...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Grid_MillName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_MillName.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Condt As String
        Dim FindStr As String

        Try

            With cbo_Grid_MillName

                If Asc(e.KeyChar) <> 27 Then

                    If Asc(e.KeyChar) = 13 Then

                        With cbo_Grid_MillName
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
                        End With

                        With dgv_YarnDetails
                            .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_MillName.Text)
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                            .CurrentCell.Selected = True
                            .Focus()
                        End With
                        .Visible = False
                        .Text = ""

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

                        If Trim(FindStr) <> "" Then
                            Condt = " Where mill_name like '" & Trim(FindStr) & "%' or mill_name like '% " & Trim(FindStr) & "%' "
                        End If

                        da = New SqlClient.SqlDataAdapter("select mill_name from mill_Head " & Condt & " order by mill_name", Con)
                        da.Fill(dt)

                        .DataSource = dt
                        .DisplayMember = "mill_name"

                        .Text = FindStr

                        .SelectionStart = FindStr.Length

                        e.Handled = True

                    End If

                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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

    Private Sub cbo_Grid_YarnType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_YarnType.GotFocus

        With cbo_Grid_YarnType

            If Trim(.Text) = "" Then .Text = "MILL"

            .BackColor = Color.lime
            .ForeColor = Color.Blue
            .SelectionStart = 0
            .SelectionLength = cbo_Grid_YarnType.Text.Length



            .Tag = .Text

        End With

    End Sub

    Private Sub cbo_Grid_YarnType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_YarnType.KeyDown
        Try
            With cbo_Grid_YarnType
                If e.KeyValue = 38 And .DroppedDown = False Then
                    e.Handled = True

                    With dgv_YarnDetails
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)
                        .CurrentCell.Selected = True
                        .Focus()

                    End With
                    cbo_Grid_YarnType.Visible = False
                    cbo_Grid_YarnType.Text = ""

                    'SendKeys.Send("+{TAB}")
                ElseIf e.KeyValue = 40 And .DroppedDown = False Then
                    e.Handled = True

                    With dgv_YarnDetails
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                        .CurrentCell.Selected = True
                        .Focus()
                    End With
                    .Visible = False
                    .Text = ""

                    'SendKeys.Send("{TAB}")
                ElseIf e.KeyValue <> 13 And e.KeyValue <> 17 And e.KeyValue <> 27 And .DroppedDown = False Then
                    .DroppedDown = True

                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT ITEM...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Grid_YarnType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_YarnType.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Condt As String
        Dim FindStr As String

        Try

            With cbo_Grid_YarnType

                If Asc(e.KeyChar) <> 27 Then

                    If Asc(e.KeyChar) = 13 Then

                        With cbo_Grid_YarnType
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
                        End With

                        With dgv_YarnDetails
                            .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_YarnType.Text)
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                            .CurrentCell.Selected = True
                            .Focus()
                        End With
                        .Visible = False
                        .Text = ""

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

                        Condt = " "
                        If Trim(FindStr) <> "" Then
                            Condt = " Where (Yarn_Type like '" & Trim(FindStr) & "%' or Yarn_Type like '% " & Trim(FindStr) & "%') "
                        End If

                        da = New SqlClient.SqlDataAdapter("select Yarn_Type from YarnType_Head " & Condt & " order by Yarn_Type", Con)
                        da.Fill(dt)

                        .DataSource = dt
                        .DisplayMember = "Yarn_Type"

                        .Text = FindStr

                        .SelectionStart = FindStr.Length

                        e.Handled = True

                    End If

                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_Grid_YarnType_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_YarnType.LostFocus

        With cbo_Grid_YarnType
            .BackColor = Color.White
            .ForeColor = Color.Black
        End With

    End Sub

    Private Sub dgv_YarnDetails_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_YarnDetails.CellEndEdit
        TotalYarn_Calculation()
        SendKeys.Send("{up}")
        SendKeys.Send("{Tab}")
    End Sub

    Private Sub dgv_YarnDetails_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_YarnDetails.CellEnter
        With dgv_YarnDetails
            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If
            If .CurrentCell.ColumnIndex = 1 Then

                cbo_Grid_CountName.Left = .Left + .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                cbo_Grid_CountName.Top = .Top + .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                cbo_Grid_CountName.Width = .CurrentCell.Size.Width
                cbo_Grid_CountName.Text = Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                cbo_Grid_CountName.Tag = Val(.CurrentCell.ColumnIndex)
                cbo_Grid_CountName.Visible = True

                cbo_Grid_CountName.BringToFront()
                cbo_Grid_CountName.Focus()

            Else

                cbo_Grid_CountName.Visible = False

                cbo_Grid_CountName.Text = ""

            End If

            If .CurrentCell.ColumnIndex = 2 Then

                cbo_Grid_YarnType.Left = .Left + .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                cbo_Grid_YarnType.Top = .Top + .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                cbo_Grid_YarnType.Width = .CurrentCell.Size.Width
                cbo_Grid_YarnType.Text = Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                cbo_Grid_YarnType.Tag = Val(.CurrentCell.ColumnIndex)
                cbo_Grid_YarnType.Visible = True

                cbo_Grid_YarnType.BringToFront()
                cbo_Grid_YarnType.Focus()

            Else

                cbo_Grid_YarnType.Visible = False

                cbo_Grid_YarnType.Text = ""

            End If

            If .CurrentCell.ColumnIndex = 3 Then

                cbo_Grid_MillName.Left = .Left + .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                cbo_Grid_MillName.Top = .Top + .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                cbo_Grid_MillName.Width = .CurrentCell.Size.Width
                cbo_Grid_MillName.Text = Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                cbo_Grid_MillName.Tag = Val(.CurrentCell.ColumnIndex)
                cbo_Grid_MillName.Visible = True

                cbo_Grid_MillName.BringToFront()
                cbo_Grid_MillName.Focus()

            Else

                cbo_Grid_MillName.Visible = False

                cbo_Grid_MillName.Text = ""

            End If

        End With

    End Sub

    Private Sub dgv_YarnDetails_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_YarnDetails.CellLeave
        With dgv_YarnDetails
            If .CurrentCell.ColumnIndex = 6 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.000")
                End If
            End If
        End With
    End Sub

    Private Sub dgv_YarnDetails_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_YarnDetails.CellValueChanged
        On Error Resume Next
        With dgv_YarnDetails
            If .Visible Then
                If .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 6 Then
                    TotalYarn_Calculation()
                End If
            End If
        End With

    End Sub

    Private Sub dgv_YarnDetails_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_YarnDetails.EditingControlShowing
        txtNumeric = CType(dgv_YarnDetails.EditingControl, DataGridViewTextBoxEditingControl)
        'AddHandler txtNumeric.KeyDown, AddressOf txtNumeric_KeyDown
        'AddHandler txtNumeric.KeyPress, AddressOf txtNumeric_KeyPress
    End Sub

    Private Sub dgv_YarnDetails_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_YarnDetails.KeyDown
        On Error Resume Next

        With dgv_YarnDetails

            If e.KeyCode = Keys.Up Then
                If .CurrentCell.RowIndex = 0 Then
                    .CurrentCell.Selected = False
                    txt_EmptyCones.Focus()
                    'SendKeys.Send("{RIGHT}")
                End If
            End If
            If e.KeyCode = Keys.Left Then
                If .CurrentCell.RowIndex = 0 And .CurrentCell.ColumnIndex <= 0 Then
                    .CurrentCell.Selected = False
                    txt_EmptyCones.Focus()
                    'SendKeys.Send("{RIGHT}")
                End If
            End If

            If e.KeyCode = Keys.Enter Then
                e.SuppressKeyPress = True
                e.Handled = True

                If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then

                    tab_Main.SelectTab(1)
                    dgv_PavuDetails.Focus()
                    dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(0).Cells(1)
                    dgv_PavuDetails.CurrentCell.Selected = True

                Else
                    SendKeys.Send("{Tab}")

                End If


            End If

        End With

    End Sub

    Private Sub dgv_YarnDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_YarnDetails.KeyUp
        Dim i As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_YarnDetails
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

            End With
        End If


    End Sub

    Private Sub dgv_YarnDetails_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_YarnDetails.LostFocus
        dgv_YarnDetails.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_YarnDetails_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_YarnDetails.RowsAdded
        Dim n As Integer

        With dgv_YarnDetails
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
        With dgv_YarnDetails
            For i = 0 To .RowCount - 1
                Sno = Sno + 1
                .Rows(i).Cells(0).Value = Sno
                If Val(.Rows(i).Cells(6).Value) <> 0 Then
                    TotBags = TotBags + Val(.Rows(i).Cells(4).Value)
                    TotCones = TotCones + Val(.Rows(i).Cells(5).Value)
                    TotWeight = TotWeight + Val(.Rows(i).Cells(6).Value)
                End If
            Next
        End With

        With dgv_YarnDetails_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(4).Value = Val(TotBags)
            .Rows(0).Cells(5).Value = Val(TotCones)
            .Rows(0).Cells(6).Value = Format(Val(TotWeight), "########0.000")
        End With

    End Sub

    Private Sub cbo_PavuGrid_CountName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PavuGrid_CountName.GotFocus

        With cbo_PavuGrid_CountName
            .BackColor = Color.lime
            .ForeColor = Color.Blue
            .SelectionStart = 0
            .SelectionLength = cbo_PavuGrid_CountName.Text.Length
        End With

    End Sub

    Private Sub cbo_PavuGrid_CountName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PavuGrid_CountName.KeyDown
        Try
            With cbo_PavuGrid_CountName
                If e.KeyValue = 38 And .DroppedDown = False Then
                    e.Handled = True

                    With dgv_PavuDetails
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)
                        .CurrentCell.Selected = True
                        .Focus()

                    End With

                    .Visible = False
                    .Text = ""

                    'SendKeys.Send("+{TAB}")

                ElseIf e.KeyValue = 40 And .DroppedDown = False Then
                    e.Handled = True
                    With dgv_PavuDetails
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                        .CurrentCell.Selected = True
                        .Focus()
                    End With
                    .Visible = False
                    .Text = ""

                    'SendKeys.Send("{TAB}")

                ElseIf e.KeyValue <> 13 And e.KeyValue <> 17 And e.KeyValue <> 27 And .DroppedDown = False Then
                    .DroppedDown = True

                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT ITEM...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_PavuGrid_CountName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PavuGrid_CountName.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Condt As String
        Dim FindStr As String

        Try

            With cbo_PavuGrid_CountName

                If Asc(e.KeyChar) <> 27 Then

                    If Asc(e.KeyChar) = 13 Then

                        With cbo_PavuGrid_CountName
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
                        End With

                        With dgv_PavuDetails
                            .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_PavuGrid_CountName.Text)
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                            .CurrentCell.Selected = True
                            .Focus()
                        End With
                        .Visible = False
                        .Text = ""

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

                        If Trim(FindStr) <> "" Then
                            Condt = " Where count_name like '" & Trim(FindStr) & "%' or count_name like '% " & Trim(FindStr) & "%' "
                        End If

                        da = New SqlClient.SqlDataAdapter("select count_name from Count_Head " & Condt & " order by count_name", con)
                        da.Fill(dt)

                        .DataSource = dt
                        .DisplayMember = "count_name"

                        .Text = FindStr

                        .SelectionStart = FindStr.Length

                        e.Handled = True

                    End If

                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_PavuGrid_CountName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PavuGrid_CountName.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New Count_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_PavuGrid_CountName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_PavuGrid_CountName_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PavuGrid_CountName.LostFocus

        With cbo_PavuGrid_CountName
            .BackColor = Color.White
            .ForeColor = Color.Black
        End With

    End Sub

    Private Sub dgv_PavuDetails_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_PavuDetails.CellEndEdit
        TotalPavu_Calculation()
        SendKeys.Send("{up}")
        SendKeys.Send("{Tab}")
    End Sub

    Private Sub dgv_PavuDetails_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_PavuDetails.CellEnter

        With dgv_PavuDetails
            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If
            If .CurrentCell.ColumnIndex = 3 Then

                cbo_PavuGrid_CountName.Left = .Left + .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                cbo_PavuGrid_CountName.Top = .Top + .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                cbo_PavuGrid_CountName.Width = .CurrentCell.Size.Width
                cbo_PavuGrid_CountName.Text = Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                cbo_PavuGrid_CountName.Tag = Val(.CurrentCell.ColumnIndex)
                cbo_PavuGrid_CountName.Visible = True

                cbo_PavuGrid_CountName.BringToFront()
                cbo_PavuGrid_CountName.Focus()

            Else

                cbo_PavuGrid_CountName.Visible = False

                cbo_PavuGrid_CountName.Text = ""

            End If

        End With

    End Sub

    Private Sub dgv_PavuDetails_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_PavuDetails.CellLeave
        With dgv_PavuDetails
            If .CurrentCell.ColumnIndex = 6 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                End If
            End If
        End With

    End Sub

    Private Sub dgv_PavuDetails_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_PavuDetails.CellValueChanged
        On Error Resume Next
        With dgv_PavuDetails
            If .Visible Then
                If .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 6 Then
                    TotalPavu_Calculation()
                End If
            End If
        End With

    End Sub

    Private Sub dgv_PavuDetails_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_PavuDetails.KeyDown

        On Error Resume Next

        With dgv_PavuDetails

            If e.KeyCode = Keys.Up Then
                If .CurrentCell.RowIndex = 0 Then
                    .CurrentCell.Selected = False
                    tab_Main.SelectTab(0)
                    dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(1)
                    dgv_YarnDetails.Focus()
                    dgv_YarnDetails.CurrentCell.Selected = True
                End If
            End If
            If e.KeyCode = Keys.Left Then
                If .CurrentCell.RowIndex = 0 And .CurrentCell.ColumnIndex <= 0 Then
                    .CurrentCell.Selected = False
                    tab_Main.SelectTab(0)
                    dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(1)
                    dgv_YarnDetails.Focus()
                    dgv_YarnDetails.CurrentCell.Selected = True
                End If
            End If

            If e.KeyCode = Keys.Enter Then
                e.SuppressKeyPress = True
                e.Handled = True

                If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then

                    If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                        save_record()

                    Else
                        tab_Main.SelectTab(0)
                        dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(1)
                        dgv_YarnDetails.Focus()
                        dgv_YarnDetails.CurrentCell.Selected = False
                        cbo_Ledger.Focus()

                    End If

                Else
                    SendKeys.Send("{Tab}")

                End If


            End If

        End With

    End Sub

    Private Sub dgv_PavuDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_PavuDetails.KeyUp
        Dim i As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_PavuDetails

                .Rows.RemoveAt(.CurrentRow.Index)

                For i = 0 To .Rows.Count - 1
                    .Rows(i).Cells(0).Value = i + 1
                Next

            End With
        End If

    End Sub

    Private Sub dgv_PavuDetails_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_PavuDetails.LostFocus
        dgv_PavuDetails.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_PavuDetails_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_PavuDetails.RowsAdded
        Dim n As Integer

        With dgv_PavuDetails
            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)
        End With

    End Sub

    Private Sub TotalPavu_Calculation()
        Dim Sno As Integer
        Dim TotBms As Single, TotPcs As Single, TotMtrs As Single

        Sno = 0
        TotBms = 0
        TotPcs = 0
        TotMtrs = 0
        With dgv_PavuDetails
            For i = 0 To .RowCount - 1
                Sno = Sno + 1
                .Rows(i).Cells(0).Value = Sno
                If Trim(.Rows(i).Cells(1).Value) <> "" And Trim(.Rows(i).Cells(4).Value) <> "" And Val(.Rows(i).Cells(6).Value) <> 0 Then
                    TotBms = TotBms + 1
                    TotPcs = TotPcs + Val(.Rows(i).Cells(5).Value)
                    TotMtrs = TotMtrs + Val(.Rows(i).Cells(6).Value)
                End If
            Next
        End With

        With dgv_PavuDetails_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(4).Value = Val(TotBms)
            .Rows(0).Cells(5).Value = Val(TotPcs)
            .Rows(0).Cells(6).Value = Format(Val(TotMtrs), "########0.000")
        End With

    End Sub

    Private Sub cbo_PavuGrid_CountName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PavuGrid_CountName.TextChanged
        On Error Resume Next
        With dgv_PavuDetails
            .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_PavuGrid_CountName.Text)
        End With
    End Sub

    Private Sub txt_EmptyBeam_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_EmptyBeam.KeyDown
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub txt_EmptyBeam_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_EmptyBeam.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub txt_EmptyBags_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_EmptyBags.KeyDown
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub txt_EmptyBags_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_EmptyBags.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub txt_EmptyCones_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_EmptyCones.KeyDown
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then
            tab_Main.SelectTab(0)
            dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(1)
            dgv_YarnDetails.Focus()
            dgv_YarnDetails.CurrentCell.Selected = True
        End If
    End Sub

    Private Sub txt_EmptyCones_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_EmptyCones.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            tab_Main.SelectTab(0)
            dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(1)
            dgv_YarnDetails.Focus()
            dgv_YarnDetails.CurrentCell.Selected = True
        End If
    End Sub

    Private Sub txtNumeric_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtNumeric.KeyPress
        If dgv_YarnDetails.CurrentCell.ColumnIndex = 4 Or dgv_YarnDetails.CurrentCell.ColumnIndex = 5 Or dgv_YarnDetails.CurrentCell.ColumnIndex = 6 Then
            If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                e.Handled = True
            End If
        End If
    End Sub

End Class