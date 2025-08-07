Public Class ReturnToFloor
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "RTRFR-"
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double

    Private WithEvents dgtxt_rackdetails As New DataGridViewTextBoxEditingControl

    Private dgv_LevColNo As Integer

    Private Sub clear()
        New_Entry = False
        Insert_Entry = False
        pnl_back.Enabled = True
        pnl_filter.Visible = False
        lbl_RefNo.Text = ""
        lbl_RefNo.ForeColor = Color.Black
        dtp_date.Text = ""
        dgv_rackdetails.Rows.Clear()
        dgv_rackdetails_total.Rows.Clear()
        dgv_rackdetails_total.Rows.Add()
        cbo_item.Text = ""
        cbo_rackno.Text = ""

        cbo_item.Visible = False
        cbo_rackno.Visible = False

        dgv_rackdetails.Tag = ""

        dgv_LevColNo = -1

        New_Entry = False
    End Sub
    Private Sub Grid_DeSelect()
        On Error Resume Next
        dgv_rackdetails.CurrentCell.Selected = False
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

        If Me.ActiveControl.Name <> cbo_item.Name Then
            cbo_item.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_rackno.Name Then
            cbo_rackno.Visible = False
        End If
        If Me.ActiveControl.Name <> dgv_rackdetails_total.Name Then
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
        dgv_rackdetails.CurrentCell.Selected = False
        dgv_rackdetails_total.CurrentCell.Selected = False
        dgv_filter.CurrentCell.Selected = False
    End Sub
    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean

        Dim dgv1 As New DataGridView

        On Error Resume Next

        If ActiveControl.Name = dgv_rackdetails.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            dgv1 = Nothing

            If ActiveControl.Name = dgv_rackdetails.Name Then
                dgv1 = dgv_rackdetails

            ElseIf dgv_rackdetails.IsCurrentRowDirty = True Then
                dgv1 = dgv_rackdetails

            End If

            If IsNothing(dgv1) = False Then

                With dgv1


                    If keyData = Keys.Enter Or keyData = Keys.Down Then
                        If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                btn_save.Focus()

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
                                dtp_date.Focus()

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

    Public Sub move_record(ByVal no As String)
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim dt2 As New DataTable
        Dim slno As Integer, n As Integer
        Dim NewCode As String

        If Val(no) = 0 Then Exit Sub

        clear()

        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)



            da = New SqlClient.SqlDataAdapter("select * from ReturnTo_Floor_Head where ReturnTo_Floor_code='" & Trim(NewCode) & "'", con)
            da.Fill(dt)

            If dt.Rows.Count > 0 Then
                lbl_RefNo.Text = dt.Rows(0).Item("ReturnTo_Floor_No").ToString
                dtp_date.Text = dt.Rows(0).Item("ReturnTo_Floor_Date").ToString

                da = New SqlClient.SqlDataAdapter("select a.*, b.Processed_Item_Name,c.Rack_No from ReturnTo_Floor_Details a INNER JOIN Processed_Item_Head b ON a.Processed_Item_IdNo = b.Processed_Item_IdNo LEFT OUTER JOIN Rack_Head c ON a.Rack_IdNo = c.Rack_IdNo where a.ReturnTo_Floor_code = '" & Trim(NewCode) & "'  Order by a.sl_no", con)
                da.Fill(dt2)

                dgv_rackdetails.Rows.Clear()
                slno = 0

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_rackdetails.Rows.Add()

                        slno = slno + 1
                        dgv_rackdetails.Rows(n).Cells(0).Value = Val(slno)
                        dgv_rackdetails.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Processed_Item_Name").ToString
                        dgv_rackdetails.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Rack_No").ToString
                        dgv_rackdetails.Rows(n).Cells(3).Value = Val(dt2.Rows(i).Item("Quantity"))
                        dgv_rackdetails.Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("Meter_Qty")), "#########0.00")
                        dgv_rackdetails.Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("Meters")), "#########0.00")

                    Next i

                    For i = 0 To dgv_rackdetails.RowCount - 1
                        dgv_rackdetails.Rows(i).Cells(0).Value = Val(i) + 1
                    Next

                    Total_Calculation()

                End If

            End If
            dt.Clear()
            dt.Dispose()
            da.Dispose()
            cbo_item.Text = ""
            cbo_item.Visible = False
        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try



        If dtp_date.Visible And dtp_date.Enabled Then dtp_date.Focus()

    End Sub

    Private Sub ReturnTo_Floor_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        
        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_item.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "FINISHEDPRODUCT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_item.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_rackno.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "RACK" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_rackno.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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
            '----

        End Try

        FrmLdSTS = False

    End Sub

    Private Sub ReturnTo_Floor_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
        Common_Procedures.Hide_CurrentStock_Display()
    End Sub

    Private Sub ReturnTo_Floor_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim dt3 As New DataTable

        Me.Text = ""

        con.Open()

        'Da = New SqlClient.SqlDataAdapter("select Processed_Item_Name from Processed_Item_Head where (Processed_Item_Type = 'FP' or Processed_Item_IdNo = 0) order by Processed_Item_Name", con)
        'Da.Fill(Dt1)
        'cbo_item.DataSource = Dt1
        'cbo_item.DisplayMember = "Processed_Item_Name"

        'Da = New SqlClient.SqlDataAdapter("select Rack_No from Rack_head order by Rack_No", con)
        'Da.Fill(Dt2)
        'cbo_rackno.DataSource = Dt2
        'cbo_rackno.DisplayMember = "Rack_No"

        pnl_filter.Visible = False
        pnl_filter.Left = (Me.Width - pnl_filter.Width) \ 2
        pnl_filter.Top = ((Me.Height - pnl_filter.Height) \ 2) + 20


        AddHandler dtp_date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_rackno.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_item.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_ItemName.GotFocus, AddressOf ControlGotFocus

        AddHandler dtp_date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_rackno.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_item.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_ItemName.LostFocus, AddressOf ControlLostFocus


        AddHandler dtp_FilterFrom_date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_FilterTo_date.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler dtp_FilterFrom_date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_FilterTo_date.KeyPress, AddressOf TextBoxControlKeyPress



        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0


        Filter_Status = False
        FrmLdSTS = True
        new_record()

    End Sub

    Private Sub ReturnTo_Floor_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        Try

            If Asc(e.KeyChar) = 27 Then

                If pnl_filter.Visible = True Then
                    btn_closefilter_Click(sender, e)
                    Exit Sub

                Else
                    Close_Form()

                End If

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
        Dim cmd As New SqlClient.SqlCommand
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Return_Entry_Rack_To_Floor, "~L~") = 0 And InStr(Common_Procedures.UR.Return_Entry_Rack_To_Floor, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub




        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.FP_Return_Entry, New_Entry, Me, con, "ReturnTo_Floor_Head", "ReturnTo_Floor_Code", NewCode, "ReturnTo_Floor_Date", "(ReturnTo_Floor_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub







        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        tr = con.BeginTransaction

        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.Transaction = tr


            cmd.CommandText = "Truncate Table TempTable_For_NegativeStock"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Insert into TempTable_For_NegativeStock ( Stock_Type, Reference_Code, Reference_Date, Company_Idno, Ledger_Idno         , Item_IdNo, Rack_IdNo ) " & _
                                    " Select                               'PI'    , Reference_Code, Reference_Date, Company_IdNo, DeliveryTo_StockIdNo, Item_IdNo, Rack_IdNo from Stock_Item_Processing_Details where Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Item_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from ReturnTo_Floor_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and ReturnTo_Floor_code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from ReturnTo_Floor_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and ReturnTo_Floor_code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            If Val(Common_Procedures.settings.NegativeStock_Restriction) = 1 Then

                If Common_Procedures.Check_is_Negative_Stock_Status(con, tr) = True Then Exit Sub

            End If

            tr.Commit()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception

            tr.Rollback()
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If dtp_date.Enabled = True And dtp_date.Visible = True Then dtp_date.Focus()

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        If Filter_Status = False Then
            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable

            da = New SqlClient.SqlDataAdapter("select a.Processed_Item_Name from Processed_Item_Head a where (Processed_Item_Type = 'FP' or Processed_Item_IdNo = 0)order by a.Processed_Item_Name", con)
            da.Fill(dt1)
            cbo_Filter_ItemName.DataSource = dt1
            cbo_Filter_ItemName.DisplayMember = "Processed_Item_Name"

            dtp_FilterFrom_date.Text = ""
            dtp_FilterTo_date.Text = ""
            pnl_filter.Text = ""
            cbo_Filter_ItemName.SelectedIndex = -1
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

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Return_Entry_Rack_To_Floor, "~L~") = 0 And InStr(Common_Procedures.UR.Return_Entry_Rack_To_Floor, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.FP_Return_Entry, New_Entry, Me) = False Then Exit Sub



        Try

            inpno = InputBox("Enter New ref.No.", "FOR INSERTION...")

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.CommandText = "select ReturnTo_Floor_No from ReturnTo_Floor_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and ReturnTo_Floor_code = '" & Trim(NewCode) & "'"
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
                    MessageBox.Show("Invalid Ref.No", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_RefNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String

        Try

            cmd.Connection = con
            cmd.CommandText = "select top 1 ReturnTo_Floor_No from ReturnTo_Floor_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and ReturnTo_Floor_code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, ReturnTo_Floor_No"
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

            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String

        Try

            da = New SqlClient.SqlDataAdapter("select top 1 ReturnTo_Floor_No from ReturnTo_Floor_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and ReturnTo_Floor_code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, ReturnTo_Floor_No desc", con)
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

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String = ""
        Dim OrdByNo As Single = 0

        ' Try

        OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))

        cmd.Connection = con
        cmd.CommandText = "select top 1 ReturnTo_Floor_No from ReturnTo_Floor_Head where for_orderby > " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and ReturnTo_Floor_code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby , ReturnTo_Floor_No"
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

        ' Catch ex As Exception
        'MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        'End Try
    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String = ""
        Dim OrdByNo As Single = 0

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))

            cmd.Connection = con
            cmd.CommandText = "select top 1 ReturnTo_Floor_No from ReturnTo_Floor_Head where for_orderby < " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and ReturnTo_Floor_code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc,ReturnTo_Floor_No desc"
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
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim NewID As Integer = 0

        Try
            clear()

            New_Entry = True

            da = New SqlClient.SqlDataAdapter("select max(for_orderby) from ReturnTo_Floor_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and ReturnTo_Floor_code like '%/" & Trim(Common_Procedures.FnYearCode) & "' ", con)
            da.Fill(dt)

            NewID = 0
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    NewID = Val(dt.Rows(0)(0).ToString)
                End If
            End If

            NewID = NewID + 1

            lbl_RefNo.Text = NewID
            lbl_RefNo.ForeColor = Color.Red

            If dtp_date.Enabled And dtp_date.Visible Then dtp_date.Focus()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String, inpno As String
        Dim NewCode As String

        Try

            inpno = InputBox("Enter Ref.No", "FOR FINDING...")

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.CommandText = "select ReturnTo_Floor_No from ReturnTo_Floor_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and ReturnTo_Floor_code = '" & Trim(NewCode) & "'"
            dr = cmd.ExecuteReader

            movno = ""
            If dr.HasRows Then
                If dr.Read Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movno = dr(0).ToString
                    End If
                End If
            End If
            ' dr.Close()
            dr.Close()
            cmd.Dispose()

            If Val(movno) <> 0 Then
                move_record(movno)

            Else
                MessageBox.Show("Ref.No. Does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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
        Dim nr As Long = 0
        Dim it_idno As Integer = 0
        Dim Sno As Integer = 0
        Dim Stockno As Integer = 0
        Dim rac_idno As Integer = 0
        Dim TotQty As Single = 0
        Dim TotMtrs As Single = 0
        Dim DlvID As Integer
        Dim RecID As Integer
        Dim Partcls As String = ""
        Dim PBlNo As String = ""
        Dim EntID As String = ""

        'If Common_Procedures.UserRight_Check(Common_Procedures.UR.Return_Entry_Rack_To_Floor, New_Entry) = False Then Exit Sub

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.FP_Return_Entry, New_Entry, Me, con, "ReturnTo_Floor_Head", "ReturnTo_Floor_Code", NewCode, "ReturnTo_Floor_Date", "(ReturnTo_Floor_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and ReturnTo_Floor_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, ReturnTo_Floor_No desc", dtp_date.Value.Date) = False Then Exit Sub



        If IsDate(dtp_date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If dtp_date.Enabled Then dtp_date.Focus()
            Exit Sub
        End If

        If Not (dtp_date.Value.Date >= Common_Procedures.Company_FromDate And dtp_date.Value.Date <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If dtp_date.Enabled Then dtp_date.Focus()
            Exit Sub
        End If

        With dgv_rackdetails
            For i = 0 To .RowCount - 1
                If Val(.Rows(i).Cells(3).Value) <> 0 Or Val(.Rows(i).Cells(5).Value) <> 0 Then

                    it_idno = Common_Procedures.Processed_Item_NameToIdNo(con, .Rows(i).Cells(1).Value)
                    If Val(it_idno) = 0 Then
                        MessageBox.Show("Invalid ItemName", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dgv_rackdetails.Enabled Then dgv_rackdetails.Focus()
                        dgv_rackdetails.CurrentCell = dgv_rackdetails.Rows(i).Cells(1)
                        Exit Sub
                    End If

                    rac_idno = Common_Procedures.Rack_NoToIdNo(con, .Rows(i).Cells(2).Value)
                    If Val(rac_idno) = 0 Then
                        MessageBox.Show("Invalid Rack No.", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dgv_rackdetails.Enabled Then dgv_rackdetails.Focus()
                        dgv_rackdetails.CurrentCell = dgv_rackdetails.Rows(i).Cells(2)
                        Exit Sub
                    End If

                End If
            Next
        End With

        Total_Calculation()

        TotQty = 0
        TotMtrs = 0
        If dgv_rackdetails_total.Rows.Count >= 1 Then
            TotQty = Val(dgv_rackdetails_total.Rows(0).Cells(3).Value)
            TotMtrs = Val(dgv_rackdetails_total.Rows(0).Cells(5).Value)
        End If

        If TotQty = 0 And TotMtrs = 0 Then
            MessageBox.Show("Invalid Quantity..", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If dgv_rackdetails.Enabled Then dgv_rackdetails.Focus()
            dgv_rackdetails.CurrentCell = dgv_rackdetails.Rows(0).Cells(3)
            Exit Sub
        End If

        tr = con.BeginTransaction

        Try


            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                da = New SqlClient.SqlDataAdapter("select max(for_orderby) from ReturnTo_Floor_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and ReturnTo_Floor_code like '%/" & Trim(Common_Procedures.FnYearCode) & "' ", con)
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
            cmd.Parameters.AddWithValue("@DeliveryDate", dtp_date.Value.Date)
            cmd.CommandText = "Truncate Table TempTable_For_NegativeStock"
            cmd.ExecuteNonQuery()

            If New_Entry = True Then

                cmd.CommandText = "Insert into ReturnTo_Floor_Head(ReturnTo_Floor_code, Company_IdNo, ReturnTo_Floor_No, for_OrderBy,ReturnTo_Floor_Date,Total_Quantity,Total_Meters) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ", @DeliveryDate," & Val(TotQty) & "," & Val(TotMtrs) & ")"

                cmd.ExecuteNonQuery()

            Else
                cmd.CommandText = "Update ReturnTo_Floor_Head set ReturnTo_Floor_Date = @DeliveryDate, Total_Quantity = " & Val(TotQty) & ", Total_Meters = " & Val(TotMtrs) & " Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and ReturnTo_Floor_code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Insert into TempTable_For_NegativeStock ( Stock_Type, Reference_Code, Reference_Date, Company_Idno, Ledger_Idno         , Item_IdNo, Rack_IdNo ) " & _
                                    " Select                               'PI'    , Reference_Code, Reference_Date, Company_IdNo, DeliveryTo_StockIdNo, Item_IdNo, Rack_IdNo from Stock_Item_Processing_Details where Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If


            EntID = Trim(Pk_Condition) & Trim(lbl_RefNo.Text)
            Partcls = "RackToFloor : Ref.No. " & Trim(lbl_RefNo.Text)
            PBlNo = Trim(lbl_RefNo.Text)


            cmd.CommandText = "Delete from ReturnTo_Floor_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and ReturnTo_Floor_code  = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()


            cmd.CommandText = "Delete from Stock_Item_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()


            With dgv_rackdetails
                Sno = 0
                For i = 0 To .RowCount - 1
                    If Val(.Rows(i).Cells(3).Value) <> 0 Or Val(.Rows(i).Cells(4).Value) <> 0 Then
                        it_idno = Common_Procedures.Processed_Item_NameToIdNo(con, .Rows(i).Cells(1).Value, tr)
                        rac_idno = Common_Procedures.Rack_NoToIdNo(con, .Rows(i).Cells(2).Value, tr)

                        Sno = Sno + 1
                        Stockno = Stockno + 1
                        cmd.CommandText = "Insert into ReturnTo_Floor_Details(ReturnTo_Floor_code, Company_IdNo,ReturnTo_Floor_No,for_OrderBy,ReturnTo_Floor_Date, Sl_No,Processed_Item_IdNo,Rack_IdNo,Quantity,Meter_Qty,Meters) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ", @DeliveryDate, " & Str(Val(Sno)) & ", " & Str(Val(it_idno)) & ", " & Str(Val(rac_idno)) & " ," & Str(Val(.Rows(i).Cells(3).Value)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & ", " & Str(Val(.Rows(i).Cells(5).Value)) & ")"
                        cmd.ExecuteNonQuery()

                        RecID = 0
                        RecID = Val(Common_Procedures.CommonLedger.Godown_Ac)

                        cmd.CommandText = "Insert into Stock_Item_Processing_Details ( Reference_Code ,            Company_IdNo          ,            Reference_No    ,            For_OrderBy                                                 ,  Reference_Date ,  DeliveryTo_StockIdNo   ,  ReceivedFrom_StockIdNo, Delivery_PartyIdNo, Received_PartyIdNo , Entry_ID             , Party_Bill_No          , Particulars          , SL_No   ,             Item_IdNo    , Rack_IdNo                  ,                      Quantity           ,                      Meters                 ) " & _
                                       " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "'      , " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",    @DeliveryDate ,  " & Str(Val(DlvID)) & ", " & Str(Val(RecID)) & ",          0        ,          0        ,'" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "',   " & Str(Val(Stockno)) & "  , " & Str(Val(it_idno)) & ", " & Str(Val(rac_idno)) & " , " & Str(Math.Abs(Val(.Rows(i).Cells(3).Value))) & ", " & Str(Math.Abs(Val(.Rows(i).Cells(5).Value))) & " ) "
                        cmd.ExecuteNonQuery()

                        Stockno = Stockno + 1

                        'DlvID = 0
                        'DlvID = Val(Common_Procedures.CommonLedger.Godown_Ac)

                        'cmd.CommandText = "Insert into Stock_Item_Processing_Details ( Reference_Code ,            Company_IdNo          ,            Reference_No    ,            For_OrderBy                                                 ,  Reference_Date ,  DeliveryTo_StockIdNo   ,  ReceivedFrom_StockIdNo, Delivery_PartyIdNo, Received_PartyIdNo , Entry_ID             , Party_Bill_No          , Particulars          , SL_No   ,             Item_IdNo    , Rack_IdNo,                      Quantity           ,                      Meters                 ) " & _
                        '               " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "'      , " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",    @DeliveryDate ,  " & Str(Val(DlvID)) & ", " & Str(Val(RecID)) & ",          0        ,          0        ,'" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "',     " & Str(Val(Stockno)) & "  , " & Str(Val(it_idno)) & ",     0     , " & Str(Math.Abs(Val(.Rows(i).Cells(3).Value))) & ", " & Str(Math.Abs(Val(.Rows(i).Cells(5).Value))) & " ) "
                        'cmd.ExecuteNonQuery()

                    End If
                Next
            End With
            If Val(Common_Procedures.settings.NegativeStock_Restriction) = 1 Then
                cmd.CommandText = "Insert into TempTable_For_NegativeStock ( Stock_Type, Reference_Code, Reference_Date, Company_Idno, Ledger_Idno           , Item_IdNo, Rack_IdNo ) " & _
                                        " Select                               'PI'    , Reference_Code, Reference_Date, Company_IdNo, ReceivedFrom_StockIdNo, Item_IdNo,     0        from Stock_Item_Processing_Details where Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                If Common_Procedures.Check_is_Negative_Stock_Status(con, tr) = True Then Exit Sub

            End If

            tr.Commit()

            If New_Entry = True Then new_record()


            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If dtp_date.Enabled And dtp_date.Visible Then dtp_date.Focus()

    End Sub

    Private Sub btn_Save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub
    Private Sub dtp_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_date.KeyDown
        If e.KeyCode = 40 Then
            dgv_rackdetails.Focus()
            dgv_rackdetails.CurrentCell = dgv_rackdetails.Rows(0).Cells(1)

            dgv_rackdetails.CurrentCell.Selected = True
        End If
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub dtp_Date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_date.KeyPress
        If Asc(e.KeyChar) = 13 Then
            dgv_rackdetails.Focus()
            dgv_rackdetails.CurrentCell = dgv_rackdetails.Rows(0).Cells(1)

            dgv_rackdetails.CurrentCell.Selected = True
        End If
    End Sub

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub cbo_Filter_ItemName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_ItemName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Processed_Item_Head", "Processed_Item_Name", "(Processed_Item_idno = 0 or Processed_Item_Type = 'FP' and Verified_Status = 1)", "(Processed_Item_IdNo = 0)")

    End Sub

    Private Sub cbo_Filter_ItemName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_ItemName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_ItemName, dtp_FilterTo_date, btn_filtershow, "Processed_Item_Head", "Processed_Item_Name", "(Processed_Item_idno = 0 or Processed_Item_Type = 'FP' and Verified_Status = 1)", "(Processed_Item_IdNo = 0)")

    End Sub


    Private Sub cbo_Filter_ItemName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_ItemName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_ItemName, btn_filtershow, "Processed_Item_Head", "Processed_Item_Name", "(Processed_Item_idno = 0 or Processed_Item_Type = 'FP' and Verified_Status = 1)", "(Processed_Item_IdNo = 0)")

    End Sub


    Private Sub btn_filtershow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_filtershow.Click
        Dim cmd As New SqlClient.SqlCommand
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim n As Integer
        Dim Led_IdNo As Integer, Itm_IdNo As Integer
        Dim Condt As String = ""

        Try

            Condt = ""
            Led_IdNo = 0
            Itm_IdNo = 0

            cmd.Connection = con
            cmd.Parameters.Clear()

            If IsDate(dtp_FilterFrom_date.Value) = True And IsDate(dtp_FilterTo_date.Value) = True Then
                cmd.Parameters.AddWithValue("@fromdate", dtp_FilterFrom_date.Value.Date)
                cmd.Parameters.AddWithValue("@todate", dtp_FilterTo_date.Value.Date)
                Condt = "a.ReturnTo_Floor_Date between @fromdate and @todate "
                'Condt = "a.ReturnTo_Floor_Date between '" & Trim(Format(dtp_FilterFrom_date.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_FilterTo_date.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_FilterFrom_date.Value) = True Then
                cmd.Parameters.AddWithValue("@fromdate", dtp_FilterFrom_date.Value.Date)
                Condt = "a.ReturnTo_Floor_Date = @fromdate "
                'Condt = "a.ReturnTo_Floor_Date = '" & Trim(Format(dtp_FilterFrom_date.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_FilterTo_date.Value) = True Then
                cmd.Parameters.AddWithValue("@todate", dtp_FilterTo_date.Value.Date)
                Condt = "a.ReturnTo_Floor_Date = @todate "
                'Condt = "a. ReturnTo_Floor_Date= '" & Trim(Format(dtp_FilterTo_date.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_ItemName.Text) <> "" Then
                Itm_IdNo = Common_Procedures.Processed_Item_NameToIdNo(con, cbo_Filter_ItemName.Text)
            End If

            If Val(Itm_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " (a.Processed_Item_IdNo = " & Str(Val(Itm_IdNo)) & ")"
            End If

            cmd.CommandText = "select a.*, b.Processed_Item_Name from ReturnTo_Floor_Details a INNER JOIN Processed_Item_Head b ON a.Processed_Item_IdNo = b.Processed_Item_IdNo  where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.ReturnTo_Floor_code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.ReturnTo_Floor_No"
            da = New SqlClient.SqlDataAdapter(cmd)

            'da = New SqlClient.SqlDataAdapter("select a.*, b.Processed_Item_SalesName from ReturnTo_Floor_Details a INNER JOIN Processed_Item_SalesName_Head b ON a.Processed_Item_IdNo = b.Processed_Item_IdNo  where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.ReturnTo_Floor_code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.ReturnTo_Floor_No", con)
            da.Fill(dt2)

            dgv_filter.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_filter.Rows.Add()

                    dgv_filter.Rows(n).Cells(0).Value = dt2.Rows(i).Item("ReturnTo_Floor_No").ToString
                    dgv_filter.Rows(n).Cells(1).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("ReturnTo_Floor_Date").ToString), "dd-MM-yyyy")
                    dgv_filter.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Processed_Item_Name").ToString
                    dgv_filter.Rows(n).Cells(3).Value = Val(dt2.Rows(i).Item("Quantity").ToString)
                    dgv_filter.Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("Meters").ToString), "#########0.00")

                Next i

            End If

            dt2.Clear()
            dt2.Dispose()
            da.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FILTER...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If dgv_filter.Visible And dgv_filter.Enabled Then dgv_filter.Focus()

    End Sub


    Private Sub dgv_filter_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_filter.DoubleClick
        Open_FilterEntry()
    End Sub

    Private Sub dgv_filter_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_filter.CellDoubleClick
        Open_FilterEntry()
    End Sub

    Private Sub dgv_filter_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_filter.KeyDown
        If e.KeyCode = 13 Then
            Open_FilterEntry()
        End If
    End Sub

    Private Sub Open_FilterEntry()
       
        Dim movno As String

        On Error Resume Next

        movno = Trim(dgv_filter.CurrentRow.Cells(0).Value)

        If Val(movno) <> 0 Then
            Filter_Status = True
            move_record(movno)
            pnl_back.Enabled = True
            pnl_filter.Visible = False
        End If



    End Sub



    Private Sub dgv_Rackdetails_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_rackdetails.CellEndEdit
        With dgv_rackdetails

            If .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                End If
            End If
            If .CurrentCell.ColumnIndex = 3 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value)
                End If
            End If

            Total_Calculation()

        End With
    End Sub

    Private Sub dgv_Rackdetails_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_rackdetails.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim rect As Rectangle

        With dgv_rackdetails
            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If

            If e.ColumnIndex = 1 Then

                If cbo_item.Visible = False Or Val(cbo_item.Tag) <> e.RowIndex Then

                    cbo_item.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Processed_Item_Name from Processed_Item_Head where Processed_Item_Type = 'FP ' order by Processed_item_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_item.DataSource = Dt1
                    cbo_item.DisplayMember = "Processed_Item_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_item.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_item.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top

                    cbo_item.Width = rect.Width  ' .CurrentCell.Size.Width
                    cbo_item.Height = rect.Height  ' rect.Height
                    cbo_item.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_item.Tag = Val(e.RowIndex)
                    cbo_item.Visible = True

                    cbo_item.BringToFront()
                    cbo_item.Focus()



                End If


            Else

                cbo_item.Visible = False

            End If

            If e.ColumnIndex = 2 Then

                If cbo_rackno.Visible = False Or Val(cbo_rackno.Tag) <> e.RowIndex Then

                    cbo_rackno.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Rack_No from Rack_Head order by Rack_No", con)
                    Dt3 = New DataTable
                    Da.Fill(Dt3)
                    cbo_rackno.DataSource = Dt3
                    cbo_rackno.DisplayMember = "Rack_No"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_rackno.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_rackno.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                    cbo_rackno.Width = rect.Width  ' .CurrentCell.Size.Width
                    cbo_rackno.Height = rect.Height  ' rect.Height

                    cbo_rackno.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_rackno.Tag = Val(e.RowIndex)
                    cbo_rackno.Visible = True

                    cbo_rackno.BringToFront()
                    cbo_rackno.Focus()

                    'cbo_Grid_CountName.Visible = False
                    'cbo_Grid_MillName.Visible = False

                End If

            Else

                cbo_rackno.Visible = False


            End If
            If e.ColumnIndex = 3 And dgv_LevColNo <> 3 Then
                'If (dgv_LevColNo = 1 And e.ColumnIndex = 2) Or (dgv_LevColNo = 2 And e.ColumnIndex = 3) Then
                Show_Item_CurrentStock(e.RowIndex)

                .Focus()
                '.CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex)

            End If
            If e.ColumnIndex = 1 Or e.ColumnIndex = 2 Or e.ColumnIndex = 4 Or e.ColumnIndex = 5 Then
                Common_Procedures.Hide_CurrentStock_Display()
            End If
        End With
    End Sub

    Private Sub dgv_rackdetails_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_rackdetails.EditingControlShowing
        dgtxt_rackdetails = CType(dgv_rackdetails.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub


    Private Sub dgv_rackdetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_rackdetails.KeyUp
        Dim n As Integer
        Dim i As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_rackdetails
                If .CurrentRow.Index = .RowCount - 1 Then
                    For i = 1 To .ColumnCount - 1
                        .Rows(.CurrentRow.Index).Cells(i).Value = ""

                    Next
                Else

                    n = .CurrentRow.Index
                    .Rows.RemoveAt(n)
                End If
                For i = 0 To .Rows.Count - 1
                    .Rows(i).Cells(0).Value = i + 1

                Next

            End With
        End If
    End Sub

    Private Sub dgv_rackdetails_CellLeave1(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_rackdetails.CellLeave
        With dgv_rackdetails
            dgv_LevColNo = .CurrentCell.ColumnIndex

            If .CurrentCell.ColumnIndex = 4 Then
                .CurrentRow.Cells(4).Value = Format(Val(.CurrentRow.Cells(4).Value), "#########0.00")
            End If
            If .CurrentCell.ColumnIndex = 5 Then
                .CurrentRow.Cells(5).Value = Format(Val(.CurrentRow.Cells(5).Value), "#########0.00")
            End If
        End With
    End Sub

    Private Sub dgv_rackdetails_CellValueChanged1(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_rackdetails.CellValueChanged
        On Error Resume Next
        With dgv_rackdetails
            If e.ColumnIndex = 3 Or e.ColumnIndex = 4 Then

                .CurrentRow.Cells(5).Value = Format(Val(.CurrentRow.Cells(3).Value) * Val(.CurrentRow.Cells(4).Value), "#########0.00")
            End If

            If e.ColumnIndex = 3 Or e.ColumnIndex = 5 Then

                Total_Calculation()

            End If

        End With
    End Sub

    Private Sub dgv_rackdetails_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_rackdetails.RowsAdded
        Dim n As Integer
        With dgv_rackdetails
            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)
        End With
    End Sub

    Private Sub cbo_item_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_item.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Processed_Item_Head", "Processed_Item_Name", "(Processed_Item_idno = 0 or Processed_Item_Type = 'FP' and Verified_Status = 1)", "(Processed_Item_IdNo = 0)")

    End Sub


    Private Sub cbo_item_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_item.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_item, Nothing, cbo_rackno, "Processed_Item_Head", "Processed_Item_Name", "(Processed_Item_idno = 0 or Processed_Item_Type = 'FP' and Verified_Status = 1)", "(Processed_Item_IdNo = 0)")
        With dgv_rackdetails

            If (e.KeyValue = 38 And cbo_item.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                If .CurrentCell.RowIndex = 0 Then
                    dtp_date.Focus()
                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentRow.Index - 1).Cells(.ColumnCount - 1)
                End If
            End If

            If (e.KeyValue = 40 And cbo_item.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                If .CurrentCell.RowIndex = .Rows.Count - 1 And Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(1).Value) = "" Then
                    If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                        save_record()
                    Else
                        dtp_date.Focus()
                    End If

                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

                End If
            End If
        End With
    End Sub


    Private Sub cbo_item_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_item.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Mtr_Qty As String
        Dim Unt_nm As String
        Dim Itm_idno As Integer = 0

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_item, cbo_rackno, "Processed_Item_Head", "Processed_Item_Name", "(Processed_Item_idno = 0 or Processed_Item_Type = 'FP' and Verified_Status = 1)", "(Processed_Item_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then

            With dgv_rackdetails
                If Val(.Rows(.CurrentRow.Index).Cells(4).Value) = 0 Then

                    Itm_idno = Common_Procedures.Processed_Item_NameToIdNo(con, Trim(cbo_item.Text))

                    da = New SqlClient.SqlDataAdapter("select a.Meter_Qty from Processed_Item_Head a Where a.Processed_Item_IdNo = " & Str(Val(Itm_idno)), con)
                    dt = New DataTable
                    da.Fill(dt)

                    Mtr_Qty = 0
                    Unt_nm = ""
                    If dt.Rows.Count > 0 Then
                        If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                            Mtr_Qty = Val(dt.Rows(0).Item("Meter_Qty").ToString)
                        End If
                    End If

                    dt.Dispose()
                    da.Dispose()

                    If Val(Mtr_Qty) <> 0 Then .Rows(.CurrentRow.Index).Cells(4).Value = Format(Val(Mtr_Qty), "#########0.00")

                End If

                If (.CurrentCell.RowIndex = .Rows.Count - 1) And Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(1).Value) = "" Then
                    If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                        save_record()
                    Else
                        dtp_date.Focus()
                    End If
                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

                End If
            End With

        End If

    End Sub

    Private Sub cbo_rackno_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_rackno.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Rack_Head", "Rack_No", "", "(Rack_IdNo = 0)")

    End Sub
    Private Sub cbo_Rackno_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_rackno.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_rackno, cbo_item, Nothing, "Rack_Head", "Rack_No", "", "(Rack_IdNo = 0)")
        With dgv_rackdetails

            If (e.KeyValue = 38 And cbo_rackno.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_rackno.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
            End If

        End With
    End Sub


    Private Sub cbo_rackno_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_rackno.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_rackno, Nothing, "Rack_Head", "Rack_No", "", "(Rack_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then

            With dgv_rackdetails

                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

            End With

        End If

    End Sub


    Private Sub cbo_rackno_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_rackno.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New RackNo_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_rackno.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub


    Private Sub cbo_rackno_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_rackno.TextChanged
        Try
            If Val(cbo_rackno.Tag) = Val(dgv_rackdetails.CurrentCell.RowIndex) And dgv_rackdetails.CurrentCell.ColumnIndex = 2 Then
                dgv_rackdetails.Rows(Me.dgv_rackdetails.CurrentCell.RowIndex).Cells.Item(2).Value = Trim(cbo_rackno.Text)
            End If
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cbo_item_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_item.TextChanged
        Try
            If Val(cbo_item.Tag) = Val(dgv_rackdetails.CurrentCell.RowIndex) And dgv_rackdetails.CurrentCell.ColumnIndex = 1 Then
                dgv_rackdetails.Rows(Me.dgv_rackdetails.CurrentCell.RowIndex).Cells.Item(1).Value = Trim(cbo_item.Text)
            End If
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_rackdetails.Enter
        dgv_rackdetails.EditingControl.BackColor = Color.Lime
        dgv_rackdetails.EditingControl.ForeColor = Color.Blue
        dgtxt_rackdetails.SelectAll()
    End Sub
    Private Sub dgtxt_rackdetails_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_rackdetails.KeyPress
        If (dgv_rackdetails.CurrentCell.ColumnIndex = 3 Or dgv_rackdetails.CurrentCell.ColumnIndex = 4) Then
            If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        End If
    End Sub

    Private Sub Total_Calculation()
        Dim TtQty As Single
        Dim TtMtrs As Single
        Dim i As Integer

        TtQty = 0
        TtMtrs = 0

        For i = 0 To dgv_rackdetails.Rows.Count - 1
            If Val(dgv_rackdetails.Rows(i).Cells(3).Value) <> 0 Or Val(dgv_rackdetails.Rows(i).Cells(5).Value) <> 0 Then
                TtQty = TtQty + Val(dgv_rackdetails.Rows(i).Cells(3).Value)
                TtMtrs = TtMtrs + Val(dgv_rackdetails.Rows(i).Cells(5).Value)
            End If
        Next

        If dgv_rackdetails_total.Rows.Count <= 0 Then dgv_rackdetails_total.Rows.Add()
        dgv_rackdetails_total.Rows(0).Cells(3).Value = Val(TtQty)
        dgv_rackdetails_total.Rows(0).Cells(5).Value = Format(Val(TtMtrs), "#########0.00")

    End Sub



    Private Sub cbo_item_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_item.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New FinishedProduct_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_item.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub btn_closefilter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_closefilter.Click
        pnl_back.Enabled = True
        pnl_filter.Visible = False
        Filter_Status = False
    End Sub
    Private Sub Show_Item_CurrentStock(ByVal Rw As Integer)
        Dim vItemID As Integer

        If Val(Rw) < 0 Then Exit Sub

        vItemID = Common_Procedures.Processed_Item_NameToIdNo(con, dgv_rackdetails.Rows(Rw).Cells(1).Value)

        If Val(vItemID) = 0 Then Exit Sub

        If Val(vItemID) <> Val(dgv_rackdetails.Tag) Then
            Common_Procedures.Show_ProcessedItem_CurrentStock_Display(con, Val(lbl_Company.Tag), Val(Common_Procedures.CommonLedger.Godown_Ac), vItemID)
            dgv_rackdetails.Tag = Val(Rw)
        End If

    End Sub
End Class
