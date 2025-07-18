Public Class Stores_Oil_Service_Entry
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "OILSE-"
    Private cbo_KeyDwnVal As Double
    Private Prec_ActCtrl As New Control
    Private dgv_DrawNo As String = ""
    Private vCbo_ItmNm As String = ""

    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl

    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
    Private prn_DetSNo As Integer


    Private Sub clear()
        New_Entry = False
        Insert_Entry = False
        pnl_Back.Enabled = True

        lbl_RefNo.Text = ""
        lbl_RefNo.ForeColor = Color.Black

        pnl_filter.Visible = False
        dtp_date.Text = ""
        cbo_Machine.Text = ""
        txt_Remarks.Text = ""
        cbo_Employe.Text = ""

    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox

        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Then
            Me.ActiveControl.BackColor = Color.PaleGreen
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

    Private Sub Oil_Service_Entry_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NoofComps As Integer = 0
        Dim CompCondt As String = ""
        Try
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Machine.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "MACHINE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Machine.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

    Private Sub Oil_Service_Entry_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub Oil_Service_Entry_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
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
    Private Sub Oil_Service_Entry_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable

        Me.Text = ""

        con.Open()

        da = New SqlClient.SqlDataAdapter("select Machine_Name from Machine_Head order by Machine_Name", con)
        da.Fill(dt1)
        cbo_Machine.DataSource = dt1
        cbo_Machine.DisplayMember = "Machine_Name"

        da = New SqlClient.SqlDataAdapter("select Employe_Name from Stores_Oil_Service_Entry_Head order by Employe_Name", con)
        da.Fill(dt2)
        cbo_Employe.DataSource = dt2
        cbo_Employe.DisplayMember = "Employe_Name"

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2
        pnl_Filter.BringToFront()



        btn_UserModification.Visible = False
        If Common_Procedures.settings.User_Modifications_Show_Status = 1 Then
            If Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_UserModification_Status = 1 Then
                btn_UserModification.Visible = True
            End If
        End If

        AddHandler dtp_date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Machine.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Employe.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_Machine.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Remarks.GotFocus, AddressOf ControlGotFocus

        AddHandler btn_Save.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Cancel.GotFocus, AddressOf ControlGotFocus

        AddHandler dtp_date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Machine.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_Machine.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Employe.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Remarks.LostFocus, AddressOf ControlLostFocus


        AddHandler txt_Remarks.KeyDown, AddressOf TextBoxControlKeyDown
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
        Dim dt1 As New DataTable
        Dim NewCode As String

        If Val(no) = 0 Then Exit Sub

        clear()

        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)
            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Machine_name from Stores_Oil_Service_Entry_Head a INNER JOIN Machine_Head b ON a.Machine_idno = b.Machine_idno where a.Oil_Service_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_RefNo.Text = dt1.Rows(0).Item("Oil_Service_No").ToString
                dtp_date.Text = dt1.Rows(0).Item("Oil_Service_Date").ToString
                cbo_Machine.Text = dt1.Rows(0).Item("Machine_Name").ToString
                cbo_Employe.Text = dt1.Rows(0).Item("Employe_name").ToString
                txt_Remarks.Text = dt1.Rows(0).Item("Remarks").ToString


            End If

            dt1.Dispose()
            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If dtp_date.Visible And dtp_date.Enabled Then dtp_date.Focus()

    End Sub
    Public Sub printrecord() Implements Interface_MDIActions.print_record
        '--
    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim vOrdByNo As String = ""

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text)

        '  If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Entry_Stores_Oil_Service, "~L~") = 0 And InStr(Common_Procedures.UR.Entry_Stores_Oil_Service, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub


        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.Stores_Oil_Service_Entry, New_Entry, Me, con, "Stores_Oil_Service_Entry_Head", "Cloth_Purchase_Code", NewCode, "Cloth_Purchase_Date", "(Cloth_Purchase_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub


        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close other windows", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> DialogResult.Yes Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        trans = con.BeginTransaction
        Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "DELETE", "Stores_Oil_Service_Entry_Head", "Oil_Service_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "", "", "Oil_Service_Code, Company_IdNo, for_OrderBy", trans)

        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.Transaction = trans

            cmd.CommandText = "Delete from Stores_Stock_Item_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Stores_Oil_Service_Entry_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Oil_Service_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            trans.Commit()

            Dt1.Dispose()
            Da.Dispose()
            trans.Dispose()
            cmd.Dispose()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            trans.Rollback()
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            If dtp_date.Enabled = True And dtp_date.Visible = True Then dtp_date.Focus()

        End Try
    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        If Filter_Status = False Then

            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable


            da = New SqlClient.SqlDataAdapter("select Machine_Name from Machine_Head order by Machine_Name", con)
            da.Fill(dt1)
            cbo_Filter_Machine.DataSource = dt1
            cbo_Filter_Machine.DisplayMember = "Machine_Name"

            cbo_Filter_Machine.Text = ""
            cbo_Filter_Machine.SelectedIndex = -1

            dgv_Filter_Details.Rows.Clear()

        End If

        pnl_Filter.Visible = True
        pnl_Back.Enabled = False
        If dtp_Filter_Fromdate.Enabled And dtp_Filter_Fromdate.Visible Then dtp_Filter_Fromdate.Focus()

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim RefCode As String

        Try

            ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Entry_Stores_Oil_Service, "~L~") = 0 And InStr(Common_Procedures.UR.Entry_Stores_Oil_Service, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub


            If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.Stores_Oil_Service_Entry, New_Entry, Me) = False Then Exit Sub


            inpno = InputBox("Enter New Ref.No.", "FOR NEW NO INSERTION...")

            RefCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Oil_Service_No from Stores_Oil_Service_Entry_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Oil_Service_Code = '" & Trim(RefCode) & "'", con)
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
                    MessageBox.Show("Invalid REF No", "DOES NOT INSERT NEW NO...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_RefNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT NEW RECEIPT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String

        Try

            da = New SqlClient.SqlDataAdapter("select top 1 Oil_Service_No from Stores_Oil_Service_Entry_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Oil_Service_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Oil_Service_No", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 Oil_Service_No from Stores_Oil_Service_Entry_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Oil_Service_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Oil_Service_No", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 Oil_Service_No from Stores_Oil_Service_Entry_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Oil_Service_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Oil_Service_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 Oil_Service_No from Stores_Oil_Service_Entry_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Oil_Service_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Oil_Service_No desc", con)
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

            lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Stores_Oil_Service_Entry_Head", "Oil_Service_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

            lbl_RefNo.ForeColor = Color.Red

            If dtp_date.Enabled And dtp_date.Visible Then dtp_date.Focus()

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

            inpno = InputBox("Enter REF.No.", "FOR FINDING...")

            RefCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Oil_Service_No from Stores_Oil_Service_Entry_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Oil_Service_Code = '" & Trim(RefCode) & "'", con)
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

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim Mac_ID As Integer = 0
        Dim EntID As String = ""
        Dim PBlNo As String = ""
        Dim Partcls As String = ""
        Dim vOrdByNo As String = ""

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text)

        ' If Common_Procedures.UserRight_Check(Common_Procedures.UR.Entry_Stores_Oil_Service, New_Entry) = False Then Exit Sub



        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Stores_Oil_Service_Entry, New_Entry, Me, con, "Stores_Oil_Service_Entry_Head", "Cloth_Purchase_Code", NewCode, "Cloth_Purchase_Date", "(Cloth_Purchase_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Cloth_Purchase_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, Cloth_Purchase_No desc", dtp_date.Value.Date) = False Then Exit Sub





        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

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

        Mac_ID = Common_Procedures.Machine_NameToIdNo(con, cbo_Machine.Text)

        If Mac_ID = 0 Then
            MessageBox.Show("Invalid Machine Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Machine.Enabled And cbo_Machine.Visible Then cbo_Machine.Focus()
            Exit Sub
        End If

        ' Mac_ID = Common_Procedures.Machine_NameToIdNo(con, cbo_Machine.Text)

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Stores_Oil_Service_Entry_Head", "Oil_Service_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr
            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@Oil_ServiceDate", dtp_date.Value.Date)

            If New_Entry = True Then
                cmd.CommandText = "Insert into Stores_Oil_Service_Entry_Head(Oil_Service_Code, Company_IdNo, Oil_Service_No, for_OrderBy, Oil_Service_Date, Machine_IdNo, Employe_Name, Remarks) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ", @Oil_ServiceDate,  " & Str(Val(Mac_ID)) & ", '" & Trim(cbo_Employe.Text) & "', '" & Trim(txt_Remarks.Text) & "' )"
                cmd.ExecuteNonQuery()

            Else

                Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "OLD", "Stores_Oil_Service_Entry_Head", "Oil_Service_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Oil_Service_Code, Company_IdNo, for_OrderBy", tr)

                cmd.CommandText = "Update Stores_Oil_Service_Entry_Head set Oil_Service_Date = @Oil_ServiceDate , Machine_IdNo = " & Str(Val(Mac_ID)) & " , Employe_Name = '" & Trim(cbo_Employe.Text) & "', Remarks = '" & Trim(txt_Remarks.Text) & "' where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Oil_Service_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If
            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "NEW", "Stores_Oil_Service_Entry_Head", "Oil_Service_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Oil_Service_Code, Company_IdNo, for_OrderBy", tr)

            EntID = Trim(Pk_Condition) & Trim(lbl_RefNo.Text)
            PBlNo = Trim(lbl_RefNo.Text)
            Partcls = "Oil_Service : Ref.No. " & Trim(lbl_RefNo.Text)

            tr.Commit()

            move_record(lbl_RefNo.Text)

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)

            new_record()

        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            Dt1.Dispose()
            Da.Dispose()
            tr.Dispose()

            If dtp_date.Enabled And dtp_date.Visible Then dtp_date.Focus()

        End Try

    End Sub

    Private Sub cbo_Machine_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Machine.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Machine, dtp_date, cbo_Employe, "Machine_HEAD", "Machine_name", "", "(Machine_idno = 0)")
    End Sub

    Private Sub cbo_Machine_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Machine.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Machine, cbo_Employe, "Machine_Head", "Machine_name", "", "(Machine_idno = 0)")
    End Sub

    Private Sub cbo_Employe_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Employe.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Employe, cbo_Machine, txt_Remarks, "Stores_Oil_Service_Entry_Head", "Employe_Name", "", "Employe_Name")
    End Sub

    Private Sub cbo_Employe_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Employe.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Employe, txt_Remarks, "Stores_Oil_Service_Entry_Head", "Employe_name", "", "Employe_Name", False)
    End Sub

    Private Sub txt_Remarks_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Remarks.KeyDown
        If e.KeyCode = 40 Then
            If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                save_record()
            Else
                dtp_date.Focus()
            End If
        End If
    End Sub

    Private Sub txt_Remarks_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Remarks.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                save_record()
            Else
                dtp_date.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_Machine_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Machine.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New Stores_Machine_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Machine.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub


    Private Sub Open_FilterEntry()
        Dim movno As String

        On Error Resume Next

        movno = Trim(dgv_Filter_Details.CurrentRow.Cells(1).Value)

        If Val(movno) <> 0 Then
            Filter_Status = True
            move_record(movno)
            pnl_Back.Enabled = True
            pnl_Filter.Visible = False
        End If

    End Sub

    Private Sub dtp_date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Asc(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub btn_Filter_Close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Filter_Close.Click
        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        Filter_Status = False
    End Sub

    Private Sub btn_Filter_Show_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Filter_Show.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim n As Integer, i As Integer
        Dim Machine_IdNo As Integer
        Dim Condt As String = ""
        Dim totalqty As Integer = 0
        Dim newqty As Integer = 0
        Dim oldusableqty As Integer = 0
        Dim oldscrapqty As Integer = 0


        Try

            Condt = ""
            Machine_IdNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Oil_Service_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Oil_Service_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Oil_Service_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_Machine.Text) <> "" Then
                Machine_IdNo = Common_Procedures.Machine_NameToIdNo(con, cbo_Filter_Machine.Text)
            End If

            If Val(Machine_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " b.Machine_IdNo = " & Str(Val(Machine_IdNo))
            End If


            da = New SqlClient.SqlDataAdapter("select a.*, b.Machine_name from Stores_Oil_Service_Entry_Head a left outer join Machine_head b on a.Machine_idno = b.Machine_idno where a.company_idno =" & Str(Val(lbl_Company.Tag)) & "and a.Oil_Service_code like '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by Oil_Service_Date, for_orderby, Oil_Service_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Oil_Service_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Oil_Service_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Machine_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = dt2.Rows(i).Item("Employe_Name").ToString

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

    Private Sub dgv_Filter_Details_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Filter_Details.CellDoubleClick
        Open_FilterEntry()
    End Sub

    Private Sub dgv_Filter_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Filter_Details.KeyDown
        If e.KeyCode = 13 Then
            Open_FilterEntry()
        End If
    End Sub



    Private Sub cbo_Filter_Machine_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_Machine.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_Machine, dtp_Filter_ToDate, btn_Filter_Show, "Machine_Head", "Machine_Name", "", "(Machine_idno = 0)")
    End Sub

    Private Sub cbo_Filter_Machine_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_Machine.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_Machine, btn_Filter_Show, "Machine_Head", "Machine_Name", "", "(Machine_idno = 0)")
    End Sub

    Private Sub dtp_date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_date.KeyDown
        If e.KeyValue = 40 Then
            e.Handled = True
            SendKeys.Send("{TAB}")
        End If
        If e.KeyValue = 38 Then
            btn_Cancel.Focus()
        End If
    End Sub

    Private Sub dtp_date_KeyPress1(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_date.KeyPress
        If Asc(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub btn_Save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Save.Click
        save_record()
    End Sub

    Private Sub btn_Cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Cancel.Click
        Me.Close()
    End Sub

    Private Sub btn_UserModification_Click(sender As Object, e As System.EventArgs) Handles btn_UserModification.Click
        If Val(Common_Procedures.User.IdNo) = 1 Then
            Dim f1 As New User_Modifications
            f1.Entry_Name = Me.Name
            f1.Entry_PkValue = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            f1.ShowDialog()
        End If
    End Sub
End Class