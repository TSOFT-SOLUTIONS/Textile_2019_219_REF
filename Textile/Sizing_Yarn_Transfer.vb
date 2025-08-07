Public Class Sizing_Yarn_Transfer
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "YNTRA-"
    Private Prec_ActCtrl As New Control
    Private vCbo_ItmNm As String
    Private vcbo_KeyDwnVal As Double


    Private Sub clear()
        New_Entry = False
        Insert_Entry = False
        pnl_filter.Visible = False
        pnl_back.Enabled = True
        lbl_RefNo.Text = ""
        lbl_RefNo.ForeColor = Color.Black
        dtp_Date.Text = ""
        cbo_PartyName.Text = ""
        cbo_PartyTo.Text = ""
        cbo_Millfrom.Text = ""
        cbo_MillTo.Text = ""
        txt_remarks.Text = ""
        cbo_Countfrom.Text = ""
        cbo_CountTo.Text = ""
        cbo_TypeFrom.Text = ""
        cbo_TypeTo.Text = ""
        cbo_WareHouse.Text = ""


        cbo_ToGodown.Text = ""
        txt_fromLot.Text = ""
        txt_toLot.Text = ""

        txt_cones.Text = ""
        txt_bags.Text = ""
        txt_WeightFrom.Text = ""
        txt_WeightTo.Text = ""
        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))
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

        'If Me.ActiveControl.Name <> cbo_ItemName.Name Then
        '    cbo_ItemName.Visible = False
        'End If
        'If Me.ActiveControl.Name <> cbo_PackingType.Name Then
        '    cbo_PackingType.Visible = False
        'End If

        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        On Error Resume Next
        If IsNothing(Prec_ActCtrl) = False Then
            Prec_ActCtrl.BackColor = Color.White
            Prec_ActCtrl.ForeColor = Color.Black
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

    Private Sub move_record(ByVal no As String)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String

        If Val(no) = 0 Then Exit Sub

        clear()

        Try
            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name, c.Mill_Name ,d.Mill_Name as Mill_ToName ,e.Count_Name,f.Count_Name as Count_ToName,g.Yarn_type,h.Yarn_Type , Lh.Ledger_Name as Godown_Name from Sizing_Yarn_Transfer_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Mill_Head c ON a.Mill_IdNofrom = c.Mill_IdNo LEFT OUTER JOIN Mill_Head d ON a.Mill_IdNoto = d.Mill_IdNo LEFT OUTER JOIN Count_Head e ON a.Count_IdNofrom = e.Count_IdNo LEFT OUTER JOIN Count_Head f ON a.Count_IdNoto = f.Count_IdNo LEFT OUTER JOIN YarnType_Head g ON a.Yarn_Typefrom = g.Yarn_Type LEFT OUTER JOIN YarnType_Head h ON a.Yarn_Typeto=h.Yarn_type LEFT JOIN Ledger_Head Lh ON a.WareHouse_IdNo = Lh.Ledger_IdNo where a.Yarn_Transfer_Code = '" & Trim(NewCode) & "'", con)
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then
                lbl_RefNo.Text = dt1.Rows(0).Item("Yarn_Transfer_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Yarn_Transfer_Date").ToString
                cbo_PartyName.Text = dt1.Rows(0).Item("Ledger_Name").ToString
                cbo_PartyTo.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Ledger_ToIdNo").ToString))
                cbo_TypeFrom.Text = dt1.Rows(0).Item("Yarn_Typefrom").ToString
                cbo_TypeTo.Text = dt1.Rows(0).Item("Yarn_Typeto").ToString
                cbo_Millfrom.Text = dt1.Rows(0).Item("Mill_Name").ToString
                cbo_MillTo.Text = dt1.Rows(0).Item("Mill_ToName").ToString
                cbo_Countfrom.Text = dt1.Rows(0).Item("Count_Name").ToString
                cbo_CountTo.Text = dt1.Rows(0).Item("Count_ToName").ToString
                txt_remarks.Text = dt1.Rows(0).Item("Remarks").ToString
                txt_cones.Text = dt1.Rows(0).Item("Yarn_Cones").ToString
                txt_bags.Text = dt1.Rows(0).Item("Yarn_Bags").ToString
                txt_WeightFrom.Text = dt1.Rows(0).Item("Yarn_Weight").ToString
                txt_WeightTo.Text = dt1.Rows(0).Item("Yarn_WeightTo").ToString

                txt_fromLot.Text = dt1.Rows(0).Item("from_Lot_no").ToString
                txt_toLot.Text = dt1.Rows(0).Item("To_Lot_no").ToString
                cbo_ToGodown.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("To_Location").ToString))

                lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))

                cbo_WareHouse.Text = dt1.Rows(0).Item("Godown_Name").ToString

            End If

            dt1.Dispose()
            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        If cbo_PartyName.Visible And cbo_PartyName.Enabled Then cbo_PartyName.Focus()

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim cmd As New SqlClient.SqlCommand
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""



        Dim vOrdByNo As String = ""
        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text)



        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.ENTRY_SIZING_JOBWORK_MODULE_YARN_TRANSFER, New_Entry, Me, con, "Sizing_Yarn_Transfer_Head", "Yarn_Transfer_Code", NewCode, "Yarn_Transfer_Date", "(Yarn_Transfer_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub



        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        tr = con.BeginTransaction

        Try


            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.Transaction = tr


            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "DELETE", "Sizing_Yarn_Transfer_Head", "Yarn_Transfer_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "", "", "Yarn_Transfer_Code, Company_IdNo, for_OrderBy", tr)


            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Sizing_Yarn_Transfer_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Transfer_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            tr.Commit()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception

            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR DELETION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        If dtp_Date.Enabled = True And dtp_Date.Visible = True Then dtp_Date.Focus()

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        If Filter_Status = False Then
            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable

            da = New SqlClient.SqlDataAdapter("select a.Ledger_DisplayName from Ledger_AlaisHead a, ledger_head b where (a.Ledger_IdNo = 0 or b.AccountsGroup_IdNo = 10) and a.Ledger_IdNo = b.Ledger_IdNo order by a.Ledger_DisplayName", con)
            da.Fill(dt1)
            cbo_PartyNameFilter.DataSource = dt1
            cbo_PartyNameFilter.DisplayMember = "Ledger_DisplayName"

            dtp_FilterFrom_date.Text = ""
            dtp_FilterTo_date.Text = ""
            pnl_filter.Text = ""
            cbo_PartyNameFilter.SelectedIndex = -1
            dgv_filter.Rows.Clear()

            da.Dispose()

        End If

        pnl_filter.Visible = True
        pnl_filter.Enabled = True
        pnl_filter.BringToFront()
        pnl_back.Enabled = False
        If dtp_FilterFrom_date.Enabled And dtp_FilterFrom_date.Visible Then dtp_FilterFrom_date.Focus()

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String, inpno As String
        Dim NewCode As String

        Try
            If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.ENTRY_SIZING_JOBWORK_MODULE_YARN_TRANSFER, New_Entry, Me) = False Then Exit Sub

            inpno = InputBox("Enter New Ref No.", "FOR INSERTION...")

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.CommandText = "select Yarn_Transfer_No from Sizing_Yarn_Transfer_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Transfer_Code = '" & Trim(NewCode) & "'"
            dr = cmd.ExecuteReader

            movno = ""
            If dr.HasRows Then
                If dr.Read Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movno = dr(0).ToString
                    End If
                End If
            End If

            dr.Close()
            cmd.Dispose()

            If Val(movno) <> 0 Then
                move_record(movno)

            Else
                If Val(inpno) = 0 Then
                    MessageBox.Show("Invalid Ref No.", "DOES NOT INSERT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_RefNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String

        Try
            cmd.Connection = con
            cmd.CommandText = "select top 1 Yarn_Transfer_No from Sizing_Yarn_Transfer_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Transfer_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Yarn_Transfer_No"
            dr = cmd.ExecuteReader

            movno = ""
            If dr.HasRows Then
                If dr.Read Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movno = dr(0).ToString
                    End If
                End If
            End If

            dr.Close()

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String

        Try
            cmd.Connection = con
            cmd.CommandText = "select top 1 Yarn_Transfer_No from Sizing_Yarn_Transfer_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Transfer_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Yarn_Transfer_No desc"
            dr = cmd.ExecuteReader

            movno = ""
            If dr.HasRows Then
                If dr.Read Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movno = dr(0).ToString
                    End If
                End If
            End If

            dr.Close()

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String = ""
        Dim OrdByNo As Single = 0

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))

            cmd.Connection = con
            cmd.CommandText = "select top 1 Yarn_Transfer_No from Sizing_Yarn_Transfer_Head where for_orderby > " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Transfer_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby,Yarn_Transfer_No"
            dr = cmd.ExecuteReader

            movno = ""
            If dr.HasRows Then
                If dr.Read Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movno = dr(0).ToString
                    End If
                End If
            End If

            dr.Close()

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String = ""
        Dim OrdByNo As Single = 0

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))

            cmd.Connection = con
            cmd.CommandText = "select top 1 Yarn_Transfer_No from Sizing_Yarn_Transfer_Head where for_orderby < " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Transfer_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc,Yarn_Transfer_No desc"
            dr = cmd.ExecuteReader

            movno = ""
            If dr.HasRows Then
                If dr.Read Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movno = dr(0).ToString
                    End If
                End If
            End If

            dr.Close()

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim NewID As Integer = 0

        Try
            clear()

            New_Entry = True

            da = New SqlClient.SqlDataAdapter("select max(for_orderby) from Sizing_Yarn_Transfer_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Transfer_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' ", con)
            da.Fill(dt)

            NewID = 0
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    NewID = Val(dt.Rows(0)(0).ToString)
                End If
            End If

            dt.Dispose()
            da.Dispose()

            NewID = NewID + 1

            lbl_RefNo.Text = NewID
            lbl_RefNo.ForeColor = Color.Red

            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String, inpno As String
        Dim NewCode As String

        Try

            inpno = InputBox("Enter Ref No", "FOR FINDING...")

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.CommandText = "select Yarn_Transfer_No from Sizing_Yarn_Transfer_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Transfer_Code = '" & Trim(NewCode) & "'"
            dr = cmd.ExecuteReader

            movno = ""
            If dr.HasRows Then
                If dr.Read Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movno = dr(0).ToString
                    End If
                End If
            End If

            dr.Close()
            cmd.Dispose()

            If Val(movno) <> 0 Then
                move_record(movno)

            Else
                MessageBox.Show("Ref No. Does not exists", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        '----
    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim tr As SqlClient.SqlTransaction
        Dim da As SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim dt5 As New DataTable
        Dim NewCode As String = ""
        Dim NewNo As Long = 0
        Dim led_id As Integer = 0
        Dim Gdn_IdNo As Integer = 0
        Dim To_Gdn_IdNo As Integer = 0
        Dim led_Toid As Integer = 0
        Dim ty_id As Integer = 0
        Dim ty_id1 As Integer = 0
        Dim mil_id As Integer = 0
        Dim mil_id1 As Integer = 0
        Dim cou_id As Integer = 0
        Dim cou_id1 As Integer = 0
        Dim Partcls As String = ""
        Dim PBlNo As String = ""
        Dim UserIdNo As Integer = 0


        Dim vOrdByNo As String = ""
        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text)



        UserIdNo = Common_Procedures.User.IdNo

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.ENTRY_SIZING_JOBWORK_MODULE_YARN_TRANSFER, New_Entry, Me, con, "Sizing_Yarn_Transfer_Head", "Yarn_Transfer_Code", NewCode, "Yarn_Transfer_Date", "(Yarn_Transfer_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Transfer_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, Yarn_Transfer_No desc", dtp_Date.Value.Date) = False Then Exit Sub



        If pnl_back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(dtp_Date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If dtp_Date.Enabled Then dtp_Date.Focus()
            Exit Sub
        End If

        If Not (dtp_Date.Value.Date >= Common_Procedures.Company_FromDate And dtp_Date.Value.Date <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If dtp_Date.Enabled Then dtp_Date.Focus()
            Exit Sub
        End If

        led_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyName.Text)

        If led_id = 0 Then
            MessageBox.Show("Invalid Party From Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_PartyName.Enabled Then cbo_PartyName.Focus()
            Exit Sub
        End If

        led_Toid = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyTo.Text)

        If led_Toid = 0 Then
            MessageBox.Show("Invalid Party To Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_PartyTo.Enabled Then cbo_PartyTo.Focus()
            Exit Sub
        End If


        mil_id = Common_Procedures.Mill_NameToIdNo(con, cbo_Millfrom.Text)
        If mil_id = 0 Then
            MessageBox.Show("Invalid MILL Name from", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Millfrom.Enabled Then cbo_Millfrom.Focus()
            Exit Sub
        End If

        mil_id1 = Common_Procedures.Mill_NameToIdNo(con, cbo_MillTo.Text)
        If mil_id1 = 0 Then
            MessageBox.Show("Invalid Mill Name To", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_MillTo.Enabled Then cbo_MillTo.Focus()
            Exit Sub
        End If

        cou_id = Common_Procedures.Count_NameToIdNo(con, cbo_Countfrom.Text)
        If cou_id = 0 Then
            MessageBox.Show("Invalid Count Name from", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Countfrom.Enabled Then cbo_Countfrom.Focus()
            Exit Sub
        End If

        cou_id1 = Common_Procedures.Count_NameToIdNo(con, cbo_CountTo.Text)
        If cou_id1 = 0 Then
            MessageBox.Show("Invalid Count Name To", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_CountTo.Enabled Then cbo_CountTo.Focus()
            Exit Sub
        End If

        Gdn_IdNo = Common_Procedures.Ledger_NameToIdNo(con, cbo_WareHouse.Text)
        To_Gdn_IdNo = Common_Procedures.Ledger_NameToIdNo(con, cbo_ToGodown.Text)

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                da = New SqlClient.SqlDataAdapter("select max(for_orderby) from Sizing_Yarn_Transfer_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and  Yarn_Transfer_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' ", con)
                da.SelectCommand.Transaction = tr
                da.Fill(dt4)

                NewNo = 0
                If dt4.Rows.Count > 0 Then
                    If IsDBNull(dt4.Rows(0)(0).ToString) = False Then
                        NewNo = Int(Val(dt4.Rows(0)(0).ToString))
                        NewNo = Val(NewNo) + 1
                    End If
                End If
                dt4.Clear()
                If Trim(NewNo) = "" Then NewNo = Trim(lbl_RefNo.Text)

                lbl_RefNo.Text = Trim(NewNo)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@ReceiptDate", dtp_Date.Value.Date)

            If New_Entry = True Then

                cmd.CommandText = "Insert into Sizing_Yarn_Transfer_Head(        User_IdNo     ,   Yarn_Transfer_Code    ,              Company_IdNo         ,          Yarn_Transfer_No      ,                                     for_OrderBy                         , Yarn_Transfer_Date ,    Ledger_IdNo      ,           Yarn_Typefrom           ,           Yarn_Typeto           ,  Mill_IdNofrom      ,       Mill_IdNoto    , Count_IdNofrom      ,      Count_IdNoto    ,           Yarn_Bags        ,              Yarn_Cones     ,            Yarn_Weight           ,          Yarn_WeightTo         ,                Remarks           ,         Ledger_ToIdNo ,     WareHouse_IdNo , from_Lot_no,To_Lot_no,To_Location) " &
                                  "Values                        (" & Str(UserIdNo) & " , '" & Trim(NewCode) & "' , " & Str(Val(lbl_Company.Tag)) & " , '" & Trim(lbl_RefNo.Text) & "' , " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & " ,       @ReceiptDate , " & Val(led_id) & " , '" & Trim(cbo_TypeFrom.Text) & "' , '" & Trim(cbo_TypeTo.Text) & "' , " & Val(mil_id) & " , " & Val(mil_id1) & " , " & Val(cou_id) & " , " & Val(cou_id1) & " , " & Val(txt_bags.Text) & " , " & Val(txt_cones.Text) & " , " & Val(txt_WeightFrom.Text) & " , " & Val(txt_WeightTo.Text) & " , '" & Trim(txt_remarks.Text) & "' , " & Val(led_Toid) & " , " & Val(Gdn_IdNo) & ",'" & Trim(txt_fromLot.Text) & "','" & Trim(txt_toLot.Text) & "'," & Val(To_Gdn_IdNo) & ")"

                cmd.ExecuteNonQuery()

            Else

                Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "OLD", "Sizing_Yarn_Transfer_Head", "Yarn_Transfer_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Yarn_Transfer_Code, Company_IdNo, for_OrderBy", tr)

                cmd.CommandText = "Update  Sizing_Yarn_Transfer_Head set User_IdNo=" & Str(UserIdNo) & ", Yarn_Transfer_Date = @ReceiptDate, Ledger_IdNo = " & Val(led_id) & ", Yarn_Typefrom = '" & Trim(cbo_TypeFrom.Text) & "',Yarn_Typeto='" & Trim(cbo_TypeTo.Text) & "', Mill_IdNofrom = " & Val(mil_id) & ",Mill_IdNoto=" & Val(mil_id1) & ",Count_IdNofrom=" & Val(cou_id) & ",Count_IdNoto=" & Val(cou_id1) & ", Yarn_Bags = " & Val(txt_bags.Text) & ",Yarn_Cones= " & Val(txt_cones.Text) & ",Yarn_Weight= " & Val(txt_WeightFrom.Text) & ", Yarn_WeightTo = " & Val(txt_WeightTo.Text) & ", Remarks = '" & Trim(txt_remarks.Text) & "',Ledger_ToIdNo = " & Val(led_Toid) & " , WareHouse_IdNo = " & Val(Gdn_IdNo) & ",from_Lot_no='" & Trim(txt_fromLot.Text) & "',To_Lot_no='" & Trim(txt_toLot.Text) & "',To_Location=" & Val(To_Gdn_IdNo) & " Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and  Yarn_Transfer_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If

            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            Partcls = "Transfer : Ref.No." & Trim(lbl_RefNo.Text)
            PBlNo = Trim(lbl_RefNo.Text)


            'cmd.CommandText = "Insert into Voucher_Details(Voucher_Code, For_OrderByCode, Company_IdNo, Voucher_No, For_OrderBy, Voucher_Type, Voucher_Date, SL_No, Ledger_IdNo, Voucher_Amount, Narration, Year_For_Report, Entry_Identification ) " & _
            '      " Values             ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ", 'Waste.sales', @Wasal, 2, " & Str(Val(Dr_ID)) & ", " & Str(-1 * Val(lbl_Amount.Text)) & ", 'Bill No. : " & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(Pk_Condition) & Trim(NewCode) & "' )"

            If Val(txt_WeightFrom.Text) <> 0 Then
                cmd.CommandText = "Insert into Stock_Yarn_Processing_Details(        SoftwareType_IdNo  ,                  Reference_Code            ,                 Company_IdNo      ,               Reference_No     ,                                 for_OrderBy                             , Reference_Date ,         DeliveryTo_Idno  , ReceivedFrom_Idno ,     Party_Bill_No     , Sl_No,            Count_IdNo    ,                 Yarn_Type         ,             Mill_IdNo    ,                 Bags            ,                 Cones            ,                 Weight                ,        Particulars      , Posting_For,  Set_Code, Set_No ,         WareHouse_IdNo   ,Lot_No  ) " &
                                  "Values                                   (" & Str(Val(Common_Procedures.SoftwareTypes.Sizing_Software)) & " , '" & Trim(Pk_Condition) & Trim(NewCode) & "' , " & Str(Val(lbl_Company.Tag)) & " , '" & Trim(lbl_RefNo.Text) & "' , " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & " ,  @ReceiptDate  , " & Str(Val(led_id)) & " ,           0       , '" & Trim(PBlNo) & "' ,    1 , " & Str(Val(cou_id)) & " , '" & Trim(cbo_TypeFrom.Text) & "' , " & Str(Val(mil_id)) & " , " & Str(Val(txt_bags.Text)) & " , " & Str(Val(txt_cones.Text)) & " , " & Str(Val(txt_WeightFrom.Text)) & " , '" & Trim(Partcls) & "' ,  'TRANSFER',     ''   ,   ''   , " & Str(Val(Gdn_IdNo)) & ",'" & Trim(txt_fromLot.Text) & "')"
                cmd.ExecuteNonQuery()
            End If

            If Val(txt_WeightTo.Text) <> 0 Then
                cmd.CommandText = "Insert into Stock_Yarn_Processing_Details(       SoftwareType_IdNo  ,                 Reference_Code            ,                 Company_IdNo      ,              Reference_No      ,                                     for_OrderBy                        , Reference_Date, DeliveryTo_Idno,       ReceivedFrom_Idno,      Party_Bill_No    , Sl_No ,             Count_IdNo    ,              Yarn_Type          ,            Mill_IdNo      ,                 Bags            ,                 Cones            ,                 Weight              ,        Particulars      , Posting_For, Set_Code, Set_No ,        WareHouse_IdNo     ,Lot_No ) " &
                                  "Values(" & Str(Val(Common_Procedures.SoftwareTypes.Sizing_Software)) & " , '" & Trim(Pk_Condition) & Trim(NewCode) & "' , " & Str(Val(lbl_Company.Tag)) & " , '" & Trim(lbl_RefNo.Text) & "' , " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ", @ReceiptDate  ,        0       ,  " & Val(led_Toid) & " , '" & Trim(PBlNo) & "' ,   2   , " & Str(Val(cou_id1)) & " , '" & Trim(cbo_TypeTo.Text) & "' , " & Str(Val(mil_id1)) & " , " & Str(Val(txt_bags.Text)) & " , " & Str(Val(txt_cones.Text)) & " , " & Str(Val(txt_WeightTo.Text)) & " , '" & Trim(Partcls) & "' ,  'TRANSFER',     ''  ,   ''   , " & Str(Val(To_Gdn_IdNo)) & ",'" & Trim(txt_toLot.Text) & "')"
                cmd.ExecuteNonQuery()
            End If

            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "NEW", "Sizing_Yarn_Transfer_Head", "Yarn_Transfer_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Yarn_Transfer_Code, Company_IdNo, for_OrderBy", tr)


            tr.Commit()

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

            If Val(Common_Procedures.settings.OnSave_MoveTo_NewEntry_Status) = 1 Then
                If New_Entry = True Then
                    new_record()
                Else
                    move_record(lbl_RefNo.Text)
                End If
            Else
                move_record(lbl_RefNo.Text)
            End If


        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

    End Sub

    Private Sub cbo_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10  and Close_Status = 0 )", "(Ledger_IdNo = 0)")
    End Sub


    Private Sub cbo_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PartyName, dtp_Date, cbo_PartyTo, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 and Close_Status = 0 )", "(Ledger_IdNo = 0)")

    End Sub



    Private Sub cbo_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PartyName, cbo_PartyTo, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 and Close_Status = 0 )", "(Ledger_IdNo = 0)")

    End Sub
    Private Sub cbo_partyname_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            Common_Procedures.UR.Ledr_Wea_Siz_Rw_Trans_JbWrk_Creation = Common_Procedures.UR.Ledger_Creation
            Common_Procedures.MDI_LedType = ""
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_PartyName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If

    End Sub

    Private Sub btn_Save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub





    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub txt_remarks_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_remarks.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            End If
        End If
    End Sub

    Private Sub btn_closefilter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_closefilter.Click
        pnl_back.Enabled = True
        pnl_filter.Visible = False
        Filter_Status = False

    End Sub

    Private Sub btn_filtershow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_filtershow.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim n As Integer
        Dim Led_IdNo As Integer
        'dim Itm_IdNo As Integer
        Dim Condt As String = ""

        Try

            Condt = ""
            Led_IdNo = 0
            ' Itm_IdNo = 0

            If IsDate(dtp_FilterFrom_date.Value) = True And IsDate(dtp_FilterTo_date.Value) = True Then
                Condt = "a.Yarn_Transfer_Date between '" & Trim(Format(dtp_FilterFrom_date.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_FilterTo_date.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_FilterFrom_date.Value) = True Then
                Condt = "a.Yarn_Transfer_Date = '" & Trim(Format(dtp_FilterFrom_date.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_FilterTo_date.Value) = True Then
                Condt = "a.Yarn_Transfer _Date= '" & Trim(Format(dtp_FilterTo_date.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_PartyNameFilter.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyNameFilter.Text)
            End If

            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " (a.Ledger_Idno = " & Str(Val(Led_IdNo)) & ")"
            End If

            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name from Sizing_Yarn_Transfer_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo  where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Yarn_Transfer_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Yarn_Transfer_No", con)
            da.Fill(dt2)

            dgv_filter.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_filter.Rows.Add()

                    dgv_filter.Rows(n).Cells(0).Value = " " & dt2.Rows(i).Item("Yarn_Transfer_No").ToString
                    dgv_filter.Rows(n).Cells(1).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Yarn_Transfer_Date").ToString), "dd-MM-yyyy")
                    dgv_filter.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_filter.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Yarn_Weight").ToString

                Next i

            End If

            dt2.Clear()
            dt2.Dispose()
            da.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FILTER...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        If dgv_filter.Visible And dgv_filter.Enabled Then dgv_filter.Focus()

    End Sub





    Private Sub dgv_filter_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_filter.DoubleClick
        Open_FilterEntry()
    End Sub

    Private Sub dgv_filter_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_filter.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        If e.KeyCode = 13 Then
            Open_FilterEntry()
        End If
    End Sub

    Private Sub Open_FilterEntry()
        Dim movno As String

        movno = Trim(dgv_filter.CurrentRow.Cells(0).Value)

        If Val(movno) <> 0 Then

            Filter_Status = True
            move_record(movno)
            pnl_back.Enabled = True
            pnl_filter.Visible = False

        End If

    End Sub

    Private Sub cbo_CountTo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_CountTo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
    End Sub


    Private Sub cbo_CountTo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_CountTo.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_CountTo, cbo_Countfrom, txt_bags, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
    End Sub
    Private Sub cbo_CountTo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_CountTo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_CountTo, txt_bags, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
    End Sub

    Private Sub cbo_Countfrom_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Countfrom.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
    End Sub
    Private Sub cbo_CountFrom_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Countfrom.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Countfrom, cbo_MillTo, cbo_CountTo, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
    End Sub
    Private Sub cbo_CountFrom_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Countfrom.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Countfrom, cbo_CountTo, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
    End Sub

    Private Sub cbo_MillTo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_MillTo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")
    End Sub

    Private Sub cbo_MillTo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_MillTo.KeyDown
        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_MillTo, cbo_Millfrom, cbo_Countfrom, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")

    End Sub

    Private Sub cbo_MillTo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_MillTo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_MillTo, cbo_Countfrom, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")

    End Sub

    Private Sub cbo_Millfrom_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Millfrom.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")
    End Sub
    Private Sub cbo_MillFrom_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Millfrom.KeyDown
        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Millfrom, cbo_TypeTo, cbo_MillTo, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")

    End Sub

    Private Sub cbo_MillFrom_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Millfrom.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Millfrom, cbo_MillTo, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")

    End Sub

    Private Sub cbo_TypeTo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_TypeTo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_TypeTo, cbo_Millfrom, "", "", "", "")
    End Sub

    Private Sub cbo_TypeTo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_TypeTo.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_TypeTo, cbo_TypeFrom, cbo_Millfrom, "", "", "", "")
    End Sub

    Private Sub cbo_TypeFrom_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_TypeFrom.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_TypeFrom, cbo_TypeTo, "", "", "", "")
    End Sub
    Private Sub cbo_TypeFrom_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_TypeFrom.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_TypeFrom, cbo_PartyTo, cbo_TypeTo, "", "", "", "")
    End Sub

    Private Sub txt_WeightFrom_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_WeightFrom.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_WeightTo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_WeightTo.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_bags_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_bags.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

    End Sub

    Private Sub txt_cones_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_cones.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

    End Sub

    Private Sub Yarn_Transfer_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Dim dt1 As New DataTable


        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_PartyName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_PartyName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Millfrom.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "MILL" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Millfrom.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_MillTo.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "MILL" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_MillTo.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Countfrom.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "COUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Countfrom.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_CountTo.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "COUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_CountTo.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_PartyTo.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_PartyTo.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

    Private Sub Yarn_Transfer_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        Common_Procedures.Last_Closed_FormName = Me.Name
        con.Close()
        con.Dispose()
    End Sub

    Private Sub YarnDelivery_Entry_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress

        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_filter.Visible = True Then
                    btn_closefilter_Click(sender, e)
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

    Private Sub Yarn_Transfer_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim Dt4 As New DataTable
        Dim dt5 As New DataTable
        Dim Dt6 As New DataTable
        Dim dt7 As New DataTable
        Me.Text = ""

        con.Open()

        Da = New SqlClient.SqlDataAdapter("select a.Ledger_DisplayName from Ledger_AlaisHead a, ledger_head b where (a.Ledger_IdNo = 0 or b.AccountsGroup_IdNo = 10 and a.Close_Status = 0) and a.Ledger_IdNo = b.Ledger_IdNo order by a.Ledger_DisplayName", con)
        Da.Fill(Dt1)
        cbo_PartyName.DataSource = Dt1
        cbo_PartyName.DisplayMember = "Ledger_DisplayName"

        Da = New SqlClient.SqlDataAdapter("select Yarn_Type from YarnType_head order by Yarn_Type", con)
        Da.Fill(Dt2)
        cbo_TypeFrom.DataSource = Dt2
        cbo_TypeFrom.DisplayMember = "Yarn_Type"

        Da = New SqlClient.SqlDataAdapter("select Yarn_Type from YarnType_head order by Yarn_Type", con)
        Da.Fill(dt3)
        cbo_TypeTo.DataSource = dt3
        cbo_TypeTo.DisplayMember = "Yarn_Type"

        Da = New SqlClient.SqlDataAdapter("select Mill_Name from Mill_head order by Mill_Name", con)
        Da.Fill(Dt4)
        cbo_MillTo.DataSource = Dt4
        cbo_MillTo.DisplayMember = "Mill_Name"

        Da = New SqlClient.SqlDataAdapter("select Mill_Name from Mill_head order by Mill_Name", con)
        Da.Fill(dt5)
        cbo_Millfrom.DataSource = dt5
        cbo_Millfrom.DisplayMember = "Mill_Name"

        Da = New SqlClient.SqlDataAdapter("select Count_Name from Count_head order by Count_Name", con)
        Da.Fill(Dt6)
        cbo_Countfrom.DataSource = Dt6
        cbo_Countfrom.DisplayMember = "Count_Name"

        Da = New SqlClient.SqlDataAdapter("select Count_Name from Count_head order by Count_Name", con)
        Da.Fill(dt7)
        cbo_CountTo.DataSource = dt7
        cbo_CountTo.DisplayMember = "Count_Name"

        btn_UserModification.Visible = False

        If Val(Common_Procedures.User.IdNo) = 1 Then
            btn_UserModification.Visible = True

        End If

        If Val(Common_Procedures.settings.Multi_Godown_Status) = 1 Then
            cbo_WareHouse.Visible = True
            lbl_fromGodown_Caption.Visible = True
        End If

        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PartyTo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_TypeFrom.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_TypeTo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Millfrom.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_MillTo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Countfrom.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_CountTo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_bags.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_cones.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_WeightFrom.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_WeightTo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_remarks.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_FilterFrom_date.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_FilterTo_date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PartyNameFilter.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_WareHouse.GotFocus, AddressOf ControlGotFocus

        AddHandler btn_save.GotFocus, AddressOf ControlGotFocus

        AddHandler btn_close.GotFocus, AddressOf ControlGotFocus



        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_PartyTo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_TypeFrom.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_TypeTo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Millfrom.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_MillTo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Countfrom.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_CountTo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_bags.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_cones.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_WeightFrom.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_WeightTo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_remarks.LostFocus, AddressOf ControlLostFocus

        AddHandler dtp_FilterFrom_date.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_FilterTo_date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_PartyNameFilter.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_WareHouse.LostFocus, AddressOf ControlLostFocus

        'AddHandler cbo_MillFrom.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_save.LostFocus, AddressOf ControlLostFocus
        ' AddHandler btn_Add.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_close.LostFocus, AddressOf ControlLostFocus

        AddHandler dtp_Date.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_bags.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_cones.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_WeightFrom.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_WeightTo.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_remarks.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler dtp_FilterFrom_date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_FilterTo_date.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_bags.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler txt_WeightFrom.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_WeightTo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_cones.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler dtp_FilterFrom_date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_FilterTo_date.KeyPress, AddressOf TextBoxControlKeyPress


        AddHandler txt_fromLot.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_fromLot.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_fromLot.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_fromLot.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler cbo_ToGodown.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ToGodown.LostFocus, AddressOf ControlLostFocus
        'AddHandler cbo_ToGodown.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler cbo_ToGodown.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler txt_toLot.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_toLot.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_toLot.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_toLot.KeyPress, AddressOf TextBoxControlKeyPress



        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        pnl_filter.Visible = False
        pnl_filter.Left = (Me.Width - pnl_filter.Width) \ 2
        pnl_filter.Top = ((Me.Height - pnl_filter.Height) \ 2) + 20

        FrmLdSTS = True

        If Common_Procedures.settings.Yarn_Stock_LotNo_wise_Status Then
            lbl_FromLot.Visible = True
            txt_fromLot.Visible = True

            lbl_ToGodown.Visible = True
            cbo_ToGodown.Visible = True

            lbl_ToLot.Visible = True
            txt_toLot.Visible = True

        Else
            lbl_FromLot.Visible = False
            txt_fromLot.Visible = False

            lbl_ToGodown.Visible = False
            cbo_ToGodown.Visible = False

            lbl_ToLot.Visible = False
            txt_toLot.Visible = False
        End If



        new_record()

    End Sub

    Private Sub cbo_PartyNameFilter_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PartyNameFilter.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_PartyNameFilter_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyNameFilter.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PartyNameFilter, dtp_FilterTo_date, btn_filtershow, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 )", "(Ledger_IdNo = 0)")

    End Sub



    Private Sub cbo_PartyNameFilter_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PartyNameFilter.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PartyNameFilter, btn_filtershow, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 )", "(Ledger_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            btn_filtershow_Click(sender, e)
        End If
    End Sub


    Private Sub cbo_Countfrom_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Countfrom.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Sizing_Count_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Countfrom.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_CountTo_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_CountTo.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Sizing_Count_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_CountTo.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_Millfrom_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Millfrom.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Mill_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Millfrom.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_MillTo_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_MillTo.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Mill_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_MillTo.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub
    Private Sub cbo_PartyTo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PartyTo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 and Close_Status = 0 )", "(Ledger_IdNo = 0)")
    End Sub


    Private Sub cbo_PartyTo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyTo.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PartyTo, cbo_PartyName, cbo_TypeFrom, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 and Close_Status = 0)", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_PartyTo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PartyTo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PartyTo, cbo_TypeFrom, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 and Close_Status = 0 )", "(Ledger_IdNo = 0)")

    End Sub
    Private Sub cbo_partyTo_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyTo.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            Common_Procedures.UR.Ledr_Wea_Siz_Rw_Trans_JbWrk_Creation = Common_Procedures.UR.Ledger_Creation
            Common_Procedures.MDI_LedType = ""
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_PartyTo.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If

    End Sub

    Private Sub cbo_Godown_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_WareHouse.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_Head", "Ledger_Name", "(Ledger_Type ='GODOWN')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Godown_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_WareHouse.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_WareHouse, txt_WeightTo, txt_fromLot, "Ledger_Head", "Ledger_Name", "(Ledger_Type ='GODOWN')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Godown_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_WareHouse.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_WareHouse, txt_fromLot, "Ledger_Head", "Ledger_Name", "(Ledger_Type ='GODOWN')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Godown_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_WareHouse.KeyUp
        If e.KeyCode = 17 And e.Control = True Then
            Common_Procedures.MDI_LedType = "GODOWN"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_WareHouse.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_ToGodown_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ToGodown.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_Head", "Ledger_Name", "(Ledger_Type ='GODOWN')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_ToGodownKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ToGodown.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_ToGodown, txt_toLot, txt_fromLot, "Ledger_Head", "Ledger_Name", "(Ledger_Type ='GODOWN')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_ToGodownKeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_ToGodown.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_ToGodown, txt_toLot, "Ledger_Head", "Ledger_Name", "(Ledger_Type ='GODOWN')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_ToGodown_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ToGodown.KeyUp
        If e.KeyCode = 17 And e.Control = True Then
            Common_Procedures.MDI_LedType = "GODOWN"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_WareHouse.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
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
End Class