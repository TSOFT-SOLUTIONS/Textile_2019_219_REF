Public Class ProcessedItem_Excess_Short
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "FPEXS-"
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

    Private Sub clear()
        New_Entry = False
        Insert_Entry = False

        pnl_Back.Enabled = True
        pnl_Filter.Visible = False

        lbl_RefNo.Text = ""
        lbl_RefNo.ForeColor = Color.Black
        cbo_Excess_Short.Text = ""

        dtp_Date.Text = ""
        cbo_ItemFrom.Text = ""
        cbo_RackFrom.Text = ""

        txt_Quantity.Text = ""
        txt_Meters.Text = ""

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
    Private Sub move_record(ByVal no As String)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim NewCode As String

        If Val(no) = 0 Then Exit Sub

        clear()

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Processed_Item_Name as Item_From , D.Rack_No As RackNo_Frm   from ProcessedItem_ExcessShort_Head a INNER JOIN Processed_Item_Head b ON a.Item_From_Idno = b.Processed_Item_IdNo  LEFT OUTER JOIN Rack_Head d ON d.Rack_Idno = A.RackFrom_IdNo  Where a.ProcessedItem_ExcessShort_Code = '" & Trim(NewCode) & "'", con)
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then
                lbl_RefNo.Text = dt1.Rows(0).Item("ProcessedItem_ExcessShort_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("ProcessedItem_ExcessShort_Date").ToString
                cbo_ItemFrom.Text = dt1.Rows(0).Item("Item_From").ToString
                '  cbo_ItemTo.Text = dt1.Rows(0).Item("Item_To").ToString
                cbo_RackFrom.Text = dt1.Rows(0).Item("RackNo_Frm").ToString
                '  cbo_RackTo.Text = dt1.Rows(0).Item("RackNo_Too").ToString
                txt_Quantity.Text = Format(Val(dt1.Rows(0).Item("Quantity").ToString), "########0.00")
                txt_Meters.Text = Format(Val(dt1.Rows(0).Item("Meters").ToString), "########0.00")
                cbo_Excess_Short.Text = dt1.Rows(0).Item("Excess_Short").ToString

            End If

            dt1.Clear()
            dt1.Dispose()
            da1.Dispose()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If dtp_Date.Visible And dtp_Date.Enabled Then dtp_Date.Focus()

    End Sub

    Private Sub Processed_ProcessedItem_ExcessShort_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_ItemFrom.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "FINISHEDPRODUCT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_ItemFrom.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_RackFrom.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "RACK" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_RackFrom.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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
            MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        FrmLdSTS = False

    End Sub

    Private Sub Processed_ProcessedItem_ExcessShort_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim dt5 As New DataTable
        Dim dt6 As New DataTable
        Dim dt7 As New DataTable

        Me.Text = ""

        con.Open()

        'da = New SqlClient.SqlDataAdapter("select Processed_Item_Name from Processed_Item_Head Where (Processed_Item_Type = 'FP' or Processed_Item_IdNo = 0) order by Processed_Item_Name", con)
        'da.Fill(dt1)
        'cbo_ItemFrom.DataSource = dt1
        'cbo_ItemFrom.DisplayMember = "Processed_Item_Name"

        'da = New SqlClient.SqlDataAdapter("select Rack_No from Rack_head order by Rack_No", con)
        'da.Fill(dt2)
        'cbo_RackFrom.DataSource = dt2
        'cbo_RackFrom.DisplayMember = "Rack_no"

        'da = New SqlClient.SqlDataAdapter("select Processed_Item_Name from Processed_Item_Head Where (Processed_Item_Type = 'FP' or Processed_Item_IdNo = 0) order by Processed_Item_Name", con)
        'da.Fill(dt3)
        'cbo_ItemTo.DataSource = dt3
        'cbo_ItemTo.DisplayMember = "Processed_Item_Name"

        'da = New SqlClient.SqlDataAdapter("select Rack_No from Rack_head order by Rack_No", con)
        'da.Fill(dt4)
        'cbo_RackTo.DataSource = dt4
        'cbo_RackTo.DisplayMember = "Rack_no"
        cbo_Excess_Short.Text = ""
        cbo_Excess_Short.Items.Add(" ")
        cbo_Excess_Short.Items.Add("EXCESS")
        cbo_Excess_Short.Items.Add("SHORT")


        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2

        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ItemFrom.GotFocus, AddressOf ControlGotFocus
        'AddHandler cbo_ItemTo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_RackFrom.GotFocus, AddressOf ControlGotFocus
        ' AddHandler cbo_RackTo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_ItemFrom.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_ItemTo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Excess_Short.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_Meters.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Quantity.GotFocus, AddressOf ControlGotFocus

        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ItemFrom.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Excess_Short.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_RackFrom.LostFocus, AddressOf ControlLostFocus
        ' AddHandler cbo_RackTo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_ItemFrom.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_ItemTo.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_Meters.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Quantity.LostFocus, AddressOf ControlLostFocus


        AddHandler dtp_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Quantity.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown


        AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Quantity.KeyPress, AddressOf TextBoxControlKeyPress
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

    Private Sub Processed_ProcessedItem_ExcessShort_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
        Common_Procedures.Hide_CurrentStock_Display()

    End Sub

    Private Sub Processed_ProcessedItem_ExcessShort_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

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

        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim Qa As Windows.Forms.DialogResult

        '  If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.ProcessedItem_ExcessShort_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.ProcessedItem_ExcessShort_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub


        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)



        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.FP_Item_Excess_Short_Entry, New_Entry, Me, con, "ProcessedItem_ExcessShort_Head", "ProcessedItem_ExcessShort_Code", NewCode, "ProcessedItem_ExcessShort_Date", "(ProcessedItem_ExcessShort_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub







        Qa = MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
        If Qa = Windows.Forms.DialogResult.No Or Qa = Windows.Forms.DialogResult.Cancel Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        trans = con.BeginTransaction

        Try

            cmd.Connection = con
            cmd.Transaction = trans

            cmd.CommandText = "Truncate Table TempTable_For_NegativeStock"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Insert into TempTable_For_NegativeStock ( Stock_Type, Reference_Code, Reference_Date, Company_Idno, Ledger_Idno         , Item_IdNo, Rack_IdNo ) " & _
                                    " Select                               'PI'    , Reference_Code, Reference_Date, Company_IdNo, DeliveryTo_StockIdNo, Item_IdNo, Rack_IdNo from Stock_Item_Processing_Details where Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Item_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from ProcessedItem_ExcessShort_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and ProcessedItem_ExcessShort_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            If Val(Common_Procedures.settings.NegativeStock_Restriction) = 1 Then

                If Common_Procedures.Check_is_Negative_Stock_Status(con, trans) = True Then Exit Sub

            End If

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

        If dtp_Date.Enabled = True And dtp_Date.Visible = True Then dtp_Date.Focus()

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        If Filter_Status = False Then

            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable
            Dim dt2 As New DataTable
            Dim dt3 As New DataTable

            da = New SqlClient.SqlDataAdapter("select Processed_Item_Name from Processed_Item_Head Where (Processed_Item_Type = 'GREY' or Processed_Item_IdNo = 0) order by Processed_Item_Name", con)
            da.Fill(dt1)
            cbo_Filter_ItemFrom.DataSource = dt1
            cbo_Filter_ItemFrom.DisplayMember = "Processed_Item_Name"

            da = New SqlClient.SqlDataAdapter("select Processed_Item_Name from Processed_Item_Head Where (Processed_Item_Type = 'GREY' or Processed_Item_IdNo = 0) order by Processed_Item_Name", con)
            da.Fill(dt2)
            cbo_Filter_ItemTo.DataSource = dt2
            cbo_Filter_ItemTo.DisplayMember = "Processed_Item_Name"

            dtp_Filter_Fromdate.Text = ""
            dtp_Filter_ToDate.Text = ""
            cbo_Filter_ItemTo.Text = ""
            cbo_Filter_ItemFrom.Text = ""

            cbo_Filter_ItemTo.SelectedIndex = -1
            cbo_Filter_ItemFrom.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()

        End If

        pnl_Filter.Visible = True
        pnl_Filter.Enabled = True
        pnl_Filter.BringToFront()
        pnl_Back.Enabled = False
        If dtp_Filter_Fromdate.Enabled And dtp_Filter_Fromdate.Visible Then dtp_Filter_Fromdate.Focus()

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""

        Try

            da = New SqlClient.SqlDataAdapter("select top 1 ProcessedItem_ExcessShort_No from ProcessedItem_ExcessShort_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and ProcessedItem_ExcessShort_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby,ProcessedItem_ExcessShort_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 ProcessedItem_ExcessShort_No from ProcessedItem_ExcessShort_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and ProcessedItem_ExcessShort_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby,ProcessedItem_ExcessShort_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 ProcessedItem_ExcessShort_No from ProcessedItem_ExcessShort_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and ProcessedItem_ExcessShort_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc,ProcessedItem_ExcessShort_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 ProcessedItem_ExcessShort_No from ProcessedItem_ExcessShort_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and ProcessedItem_ExcessShort_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc,ProcessedItem_ExcessShort_No desc", con)
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
        Dim dt As New DataTable
        Dim NewID As Integer = 0

        Try
            clear()

            New_Entry = True

            lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "ProcessedItem_ExcessShort_Head", "ProcessedItem_ExcessShort_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

            lbl_RefNo.ForeColor = Color.Red

            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        dt.Dispose()
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

            Da = New SqlClient.SqlDataAdapter("select ProcessedItem_ExcessShort_No from ProcessedItem_ExcessShort_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and ProcessedItem_ExcessShort_Code = '" & Trim(RefCode) & "'", con)
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
                MessageBox.Show("Ref No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.FP_Item_Excess_Short_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.ProcessedItem_ExcessShort_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.FP_Item_Excess_Short_Entry, New_Entry, Me) = False Then Exit Sub



        Try

            inpno = InputBox("Enter New Ref No.", "FOR NEW REF INSERTION...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select ProcessedItem_ExcessShort_No from ProcessedItem_ExcessShort_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and ProcessedItem_ExcessShort_Code = '" & Trim(RecCode) & "'", con)
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
                    MessageBox.Show("Invalid Ref No", "DOES NOT INSERT NEW REF...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_RefNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT NEW REF...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim Led_ID As Integer = 0
        Dim Col_ID As Integer = 0
        Dim Itfp_ID As Integer = 0, Rac_IdNo As Integer = 0
        Dim Sno As Integer = 0
        Dim Partcls As String = ""
        Dim PBlNo As String = ""
        Dim EntID As String = ""
        Dim Procfm_ID As Integer = 0
        Dim procto_ID As Integer = 0
        Dim Racfrm_Id As Integer = 0
        Dim Racto_Id As Integer = 0
        Dim Delv_ID As Integer = 0, Rec_ID As Integer = 0
        Dim Led_type As String = ""
        Dim VouBil As String = ""
        Dim Prtcls_DelvIdNo As Integer = 0, Prtcls_RecIdNo As Integer = 0

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        ' If Common_Procedures.UserRight_Check(Common_Procedures.UR.ProcessedItem_ExcessShort_Entry, New_Entry) = False Then Exit Sub


        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)


        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.FP_Item_Excess_Short_Entry, New_Entry, Me, con, "ProcessedItem_ExcessShort_Head", "ProcessedItem_ExcessShort_Code", NewCode, "ProcessedItem_ExcessShort_Date", "(ProcessedItem_ExcessShort_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and ProcessedItem_ExcessShort_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, ProcessedItem_ExcessShort_No desc", dtp_Date.Value.Date) = False Then Exit Sub

 
        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(dtp_Date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
            Exit Sub
        End If

        If Not (dtp_Date.Value.Date >= Common_Procedures.Company_FromDate And dtp_Date.Value.Date <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
            Exit Sub
        End If

        Procfm_ID = Common_Procedures.Processed_Item_NameToIdNo(con, cbo_ItemFrom.Text)
        If Procfm_ID = 0 Then
            MessageBox.Show("Invalid Item FromName", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_ItemFrom.Enabled And cbo_ItemFrom.Visible Then cbo_ItemFrom.Focus()
            Exit Sub
        End If

        If Trim(txt_Quantity.Text) = "" Then
            MessageBox.Show("Invalid Quantity", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If txt_Quantity.Enabled Then txt_Quantity.Focus()
            Exit Sub
        End If

        Racfrm_Id = Common_Procedures.Rack_NoToIdNo(con, cbo_RackFrom.Text)

        tr = con.BeginTransaction


        Try


            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "ProcessedItem_ExcessShort_Head", "ProcessedItem_ExcessShort_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@TransferDate", dtp_Date.Value.Date)
            cmd.CommandText = "Truncate Table TempTable_For_NegativeStock"
            cmd.ExecuteNonQuery()

            If New_Entry = True Then
                cmd.CommandText = "Insert into ProcessedItem_ExcessShort_Head (ProcessedItem_ExcessShort_Code, Company_IdNo,ProcessedItem_ExcessShort_No, for_OrderBy,ProcessedItem_ExcessShort_Date,Item_From_Idno, RackFrom_IdNo , Quantity , Meters ,Excess_Short ) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ", @TransferDate, " & Val(Procfm_ID) & ", " & Str(Val(Racfrm_Id)) & "," & Val(txt_Quantity.Text) & "," & Val(txt_Meters.Text) & " , '" & Trim(cbo_Excess_Short.Text) & "')"
                cmd.ExecuteNonQuery()

            Else

                cmd.CommandText = "Update ProcessedItem_ExcessShort_Head set ProcessedItem_ExcessShort_Date = @TransferDate, Item_From_Idno = " & Val(Procfm_ID) & ",  RackFrom_IdNo = " & Str(Val(Racfrm_Id)) & " ,  Quantity = " & Val(txt_Quantity.Text) & ", Meters = " & Val(txt_Meters.Text) & " , Excess_Short = '" & Trim(cbo_Excess_Short.Text) & "'  Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and ProcessedItem_ExcessShort_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()
                'cmd.CommandText = "Insert into TempTable_For_NegativeStock ( Stock_Type, Reference_Code, Reference_Date, Company_Idno, Ledger_Idno         , Item_IdNo, Rack_IdNo ) " & _
                '                     " Select                               'PI'    , Reference_Code, Reference_Date, Company_IdNo, DeliveryTo_StockIdNo, Item_IdNo, Rack_IdNo from Stock_Item_Processing_Details where Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                'cmd.ExecuteNonQuery()

            End If
            Partcls = "Ref.No. " & Trim(lbl_RefNo.Text)
            PBlNo = Trim(lbl_RefNo.Text)
            EntID = Trim(Pk_Condition) & Trim(lbl_RefNo.Text)

            cmd.CommandText = "Delete from Stock_Item_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            Delv_ID = 0 : Rec_ID = 0
            Prtcls_DelvIdNo = 0 : Prtcls_RecIdNo = 0

            If Trim(UCase(cbo_Excess_Short.Text)) = "EXCESS" Then
                Delv_ID = Val(Common_Procedures.CommonLedger.Godown_Ac)
                Rec_ID = 0
            Else
                Delv_ID = 0
                Rec_ID = Val(Common_Procedures.CommonLedger.Godown_Ac)
            End If

            cmd.CommandText = "Insert into Stock_Item_Processing_Details ( Reference_Code                          ,            Company_IdNo          ,            Reference_No      ,            For_OrderBy                                                       ,  Reference_Date     ,  DeliveryTo_StockIdNo  ,  ReceivedFrom_StockIdNo          , Delivery_PartyIdNo               , Received_PartyIdNo              , Entry_ID                     , Party_Bill_No                               , Particulars            , SL_No   ,       Item_IdNo            , Rack_IdNo                          ,                      Quantity          ,      Meters                 ) " & _
                                                       " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "'      , " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",    @TransferDate         ,            " & Str(Val(Delv_ID)) & "        ,  " & Str(Val(Rec_ID)) & "        ,              0                   ,         0                    ,'" & Trim(EntID) & "' , '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "',  -1    , " & Str(Val(Procfm_ID)) & "  ," & Str(Val(Racfrm_Id)) & " ,  " & Str(Val(txt_Quantity.Text)) & "   , " & Val(txt_Meters.Text) & "       ) "
            cmd.ExecuteNonQuery()

            'cmd.CommandText = "Insert into Stock_Item_Processing_Details ( Reference_Code                          ,            Company_IdNo          ,            Reference_No      ,            For_OrderBy                                                       ,  Reference_Date     ,   ReceivedFrom_StockIdNo ,    DeliveryTo_StockIdNo        , Delivery_PartyIdNo               , Received_PartyIdNo              , Entry_ID                     , Party_Bill_No                               , Particulars            , SL_No   ,       Item_IdNo            , Rack_IdNo                          ,                      Quantity          ,      Meters                 ) " & _
            '                                           " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "'      , " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",    @TransferDate         ,               0         ,  " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & "        ,              0                   ,         0                    ,'" & Trim(EntID) & "' , '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "',  1    , " & Str(Val(procto_ID)) & " , " & Str(Val(Racto_Id)) & " ,  " & Str(Val(txt_Quantity.Text)) & "   , " & Val(txt_Meters.Text) & "       ) "
            'cmd.ExecuteNonQuery()

            If Val(Common_Procedures.settings.NegativeStock_Restriction) = 1 Then
                cmd.CommandText = "Insert into TempTable_For_NegativeStock ( Stock_Type, Reference_Code, Reference_Date, Company_Idno, Ledger_Idno           , Item_IdNo, Rack_IdNo ) " & _
                                        " Select                               'PI'    , Reference_Code, Reference_Date, Company_IdNo, ReceivedFrom_StockIdNo, Item_IdNo,     0        from Stock_Item_Processing_Details where Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                If Common_Procedures.Check_is_Negative_Stock_Status(con, tr) = True Then Exit Sub

            End If

            tr.Commit()

            Dt1.Dispose()
            Da.Dispose()
            If New_Entry = True Then new_record()


            MessageBox.Show("Saved Successfully!!!", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

    End Sub

    Private Sub cbo_ItemFrom_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ItemFrom.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Processed_Item_Head", "Processed_Item_Name", "(Processed_Item_idno = 0 or Processed_Item_Type = 'FP' and Verified_Status = 1)", "(Processed_Item_IdNo = 0)")

    End Sub

    Private Sub cbo_itemFrom_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ItemFrom.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_ItemFrom, dtp_Date, cbo_RackFrom, "Processed_Item_Head", "Processed_Item_Name", "(Processed_Item_idno = 0 or Processed_Item_Type = 'FP' and Verified_Status = 1)", "(Processed_Item_IdNo = 0)")

    End Sub

    Private Sub cbo_ItemFrom_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_ItemFrom.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_ItemFrom, cbo_RackFrom, "Processed_Item_Head", "Processed_Item_Name", "(Processed_Item_idno = 0 or Processed_Item_Type = 'FP' and Verified_Status = 1)", "(Processed_Item_IdNo = 0)")
    End Sub

    Private Sub cbo_ItemFrom_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ItemFrom.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            Dim f As New FinishedProduct_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_ItemFrom.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_RackFrom_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_RackFrom.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Rack_Head", "Rack_No", "(Close_Status = 0)", "(Rack_IdNo = 0)")

    End Sub

    Private Sub cbo_RackFrom_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_RackFrom.KeyDown
        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_RackFrom, cbo_ItemFrom, cbo_Excess_Short, "Rack_Head", "Rack_No", "(Close_Status = 0)", "(Rack_IdNo = 0)")

    End Sub

    Private Sub cbo_RackFrom_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_RackFrom.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_RackFrom, cbo_Excess_Short, "Rack_Head", "Rack_No", "(Close_Status = 0)", "(Rack_IdNo = 0)")

    End Sub

    Private Sub cbo_RackFrom_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_RackFrom.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New RackNo_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_RackFrom.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub btn_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub btn_Filter_Show_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter_Show.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim n As Integer, i As Integer
        Dim Procto_IdNo As Integer, procfm_IdNo As Integer
        Dim Condt As String = ""

        'Try

        Condt = ""
        Procto_IdNo = 0
        procfm_IdNo = 0

        If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
            Condt = "a.ProcessedItem_ExcessShort_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
        ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
            Condt = "a.ProcessedItem_ExcessShort_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
        ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
            Condt = "a.ProcessedItem_ExcessShort_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
        End If

        If Trim(cbo_Filter_ItemFrom.Text) <> "" Then
            procfm_IdNo = Common_Procedures.Processed_Item_NameToIdNo(con, cbo_Filter_ItemFrom.Text)
        End If
        If Trim(cbo_Filter_ItemTo.Text) <> "" Then
            Procto_IdNo = Common_Procedures.Processed_Item_NameToIdNo(con, cbo_Filter_ItemTo.Text)
        End If

        If Val(procfm_IdNo) <> 0 Then
            Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.Item_From_Idno = " & Str(Val(procfm_IdNo))
        End If

        If Val(Procto_IdNo) <> 0 Then
            Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.Item_To_Idno = '" & Trim(Procto_IdNo) & "'"
        End If

        da = New SqlClient.SqlDataAdapter("select a.*, b.Processed_Item_Name as Item_From_Name,c.Processed_Item_Name as Item_To_Name from ProcessedItem_ExcessShort_Head a INNER JOIN Processed_Item_Head b on a.Item_From_Idno = b.Processed_Item_IdNo INNER JOIN Processed_Item_Head c on a.Item_To_Idno = c.Processed_Item_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.ProcessedItem_ExcessShort_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.ProcessedItem_ExcessShort_No", con)
        da.Fill(dt2)

        dgv_Filter_Details.Rows.Clear()

        If dt2.Rows.Count > 0 Then

            For i = 0 To dt2.Rows.Count - 1

                n = dgv_Filter_Details.Rows.Add()


                dgv_Filter_Details.Rows(n).Cells(0).Value = dt2.Rows(i).Item("ProcessedItem_ExcessShort_No").ToString
                dgv_Filter_Details.Rows(n).Cells(1).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("ProcessedItem_ExcessShort_Date").ToString), "dd-MM-yyyy")
                dgv_Filter_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Item_From_Name").ToString
                dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Item_To_Name").ToString
                dgv_Filter_Details.Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("Meters").ToString), "########0.00")
                dgv_Filter_Details.Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("Quantity").ToString), "########0.00")

            Next i

        End If

        dt2.Clear()
        dt2.Dispose()
        da.Dispose()

        'Catch ex As Exception
        '    MessageBox.Show(ex.Message, "DOES NOT FILTER...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        'End Try

        If dgv_Filter_Details.Visible And dgv_Filter_Details.Enabled Then dgv_Filter_Details.Focus()

    End Sub

    Private Sub cbo_Filter_ItemTo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_ItemTo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Processed_Item_Head", "Processed_Item_Name", "(Processed_Item_idno = 0 or Processed_Item_Type = 'FP' and Verified_Status = 1)", "(Processed_Item_IdNo = 0)")

    End Sub

    Private Sub cbo_Filter_ItemName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_ItemTo.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_ItemTo, cbo_Filter_ItemFrom, btn_Filter_Show, "Processed_Item_Head", "Processed_Item_Name", "(Processed_Item_idno = 0 or Processed_Item_Type = 'FP' and Verified_Status = 1)", "(Processed_Item_IdNo = 0)")

    End Sub

    Private Sub cbo_Filter_ItemName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_ItemTo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_ItemTo, btn_Filter_Show, "Processed_Item_Head", "Processed_Item_Name", "(Processed_Item_idno = 0 or Processed_Item_Type = 'FP' and Verified_Status = 1)", "(Processed_Item_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_ItemFrom_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_ItemFrom.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Processed_Item_Head", "Processed_Item_Name", "(Processed_Item_idno = 0 or Processed_Item_Type = 'FP' and Verified_Status = 1)", "(Processed_Item_IdNo = 0)")

    End Sub

    Private Sub cbo_Filter_RackNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_ItemFrom.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_ItemFrom, dtp_Filter_ToDate, cbo_Filter_ItemTo, "Processed_Item_Head", "Processed_Item_Name", "(Processed_Item_idno = 0 or Processed_Item_Type = 'FP' and Verified_Status = 1)", "(Processed_Item_IdNo = 0)")

    End Sub

    Private Sub cbo_Filter_RackNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_ItemFrom.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_ItemFrom, cbo_Filter_ItemTo, "Processed_Item_Head", "Processed_Item_Name", "(Processed_Item_idno = 0 or Processed_Item_Type = 'FP' and Verified_Status = 1)", "(Processed_Item_IdNo = 0)")

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

    Private Sub txt_Meters_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Meters.KeyDown
        If e.KeyCode = 40 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                dtp_Date.Focus()
            End If
        End If
        If e.KeyCode = 38 Then
            Show_Item_CurrentStock()
            txt_Quantity.Focus()
        End If

    End Sub

    Private Sub txt_Meters_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Meters.KeyPress
        If Val(Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar))) = 0 Then
            e.Handled = True
        End If
        If Asc(e.KeyChar) = 13 Then

            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            End If
        End If
    End Sub

    Private Sub btn_Filter_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter_Close.Click
        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        Filter_Status = False
    End Sub

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        '--
    End Sub

    Private Sub txt_Quantity_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Quantity.Click
        Show_Item_CurrentStock()
        txt_Quantity.Focus()
    End Sub

    Private Sub txt_Quantity_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Quantity.Enter
        '--
    End Sub

    Private Sub txt_Quantity_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Quantity.KeyPress
        If Val(Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar))) = 0 Then
            e.Handled = True
        End If
    End Sub
    Private Sub Show_Item_CurrentStock()
        Dim vItemID As Integer

        With cbo_ItemFrom

            vItemID = Common_Procedures.Processed_Item_NameToIdNo(con, cbo_ItemFrom.Text)

            If Val(vItemID) = 0 Then Exit Sub

            If Val(vItemID) <> 0 Then
                Common_Procedures.Show_ProcessedItem_CurrentStock_Display(con, Val(lbl_Company.Tag), Val(Common_Procedures.CommonLedger.Godown_Ac), vItemID)
            End If

        End With

    End Sub

    Private Sub cbo_Excess_Short_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Excess_Short.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")

    End Sub
    Private Sub cbo_Excess_Short_Keydown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Excess_Short.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Excess_Short, cbo_RackFrom, txt_Quantity, "", "", "", "")

    End Sub


    Private Sub cbo_Excess_Short_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Excess_Short.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Excess_Short, txt_Quantity, "", "", "", "")
    End Sub

End Class