Public Class Cheque_Entry
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "CHEQU-"
    Private prn_HdDt As New DataTable
    Private Prec_ActCtrl As New Control
    Private prn_PageNo As Integer
    Private vcbo_KeyDwnVal As Double

    Private Mov_Status As Boolean = False
    Private NameChqSTS As Boolean = False
    Private prn_Status As Integer
    Private prn_DetSNo As Integer
    Private prn_HeadIndx As Integer
    Dim cmbnkNm As String = ""

    Public vmskOldText As String = ""
    Public vmskSelStrt As Integer = -1

    Private Sub clear()
        New_Entry = False
        Insert_Entry = False
        pnl_filter.Visible = False
        pnl_back.Enabled = True
        lbl_ReceiptNo.Text = ""
        lbl_ReceiptNo.ForeColor = Color.Black
        msk_date.Text = ""
        dtp_Date.Text = ""
        cbo_Bank.Text = ""
        cbo_ACPayee.Text = "A/C PAYEE"
        cbo_PartyName.Text = ""

        txt_ChequeAmt.Text = ""
        txt_ChequeNo.Text = ""
        txt_Narration.Text = ""
        txt_Print_Name.Text = ""
        txt_ChequeNo.Text = ""

    End Sub

    Private Sub move_record(ByVal no As String)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String
        Dim LockSTS As Boolean = False

        If Val(no) = 0 Then Exit Sub

        clear()

        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

            da1 = New SqlClient.SqlDataAdapter("select a.* from Cheque_Head a  where a.Cheque_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then
                lbl_ReceiptNo.Text = dt1.Rows(0).Item("Cheque_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Cheque_Date").ToString

                msk_date.Text = dtp_Date.Text
                lbl_Day.Text = dt1.Rows(0).Item("Cheque_Day").ToString
                cbo_Bank.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Bank_IdNo").ToString))
                cbo_PartyName.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Ledger_IdNo").ToString))
                cbo_ACPayee.Text = dt1.Rows(0).Item("ACPayee_NameCheque").ToString
                txt_ChequeAmt.Text = Format(Val(dt1.Rows(0).Item("Cheque_AmOUNT").ToString), "#########0.00")

                txt_ChequeNo.Text = Val(dt1.Rows(0).Item("ChequeNo").ToString)

                txt_Narration.Text = (dt1.Rows(0).Item("Narration").ToString)
                txt_Print_Name.Text = (dt1.Rows(0).Item("Print_Name").ToString) 'Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Agent_IdNo").ToString))
            End If

            dt1.Dispose()
            da1.Dispose()


        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If msk_date.Visible And msk_date.Enabled Then msk_date.Focus()

    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox
        Dim Msktxbx As MaskedTextBox

        On Error Resume Next

        If FrmLdSTS = True Then Exit Sub

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

        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)

        On Error Resume Next

        If FrmLdSTS = True Then Exit Sub

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


    Private Sub Cheque_Entry_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Dim da As SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NoofComps As Integer = 0
        Dim CompCondt As String = ""

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Bank.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Bank.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_PartyName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_PartyName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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
            '---MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            FrmLdSTS = False

        End Try

    End Sub

    Private Sub Cheque_Entry_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub Cheque_Entry_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Text = ""

        con.Open()

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        pnl_filter.Visible = False
        pnl_filter.Left = (Me.Width - pnl_filter.Width) \ 2
        pnl_filter.Top = (Me.Height - pnl_filter.Height) \ 2

        cbo_ACPayee.Items.Clear()
        cbo_ACPayee.Items.Add("")
        cbo_ACPayee.Items.Add("A/C PAYEE")
        cbo_ACPayee.Items.Add("NAME CHEQUE")
        cbo_ACPayee.Items.Add("RTGS")
        cbo_ACPayee.Items.Add("NEFT")
        cbo_ACPayee.Items.Add("DD")
        cbo_ACPayee.Items.Add("TC")


        AddHandler msk_date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Bank.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ACPayee.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_ChequeAmt.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_ChequeNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Narration.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Print_Name.GotFocus, AddressOf ControlGotFocus

        AddHandler msk_date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Bank.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ACPayee.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_ChequeAmt.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_ChequeNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Narration.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Print_Name.LostFocus, AddressOf ControlLostFocus

        AddHandler msk_date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_ChequeAmt.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_ChequeNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Print_Name.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler dtp_FilterFrom_date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_FilterTo_date.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler msk_date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_ChequeAmt.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_ChequeNo.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler dtp_FilterFrom_date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_FilterTo_date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Print_Name.KeyPress, AddressOf TextBoxControlKeyPress

        FrmLdSTS = True
        new_record()

    End Sub

    Private Sub Cheque_Entry_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

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
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

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

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim cmd As New SqlClient.SqlCommand
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        '  If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Trip_Sheet_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Trip_Sheet_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub


        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If




        tr = con.BeginTransaction

        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ReceiptNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.Transaction = tr

            'cmd.CommandText = "Delete from Stock_Empty_BeamBagCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            'cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Cheque_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cheque_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            tr.Commit()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception

            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If msk_date.Enabled = True And msk_date.Visible = True Then msk_date.Focus()

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        If Filter_Status = False Then

            dtp_FilterFrom_date.Text = ""
            dtp_FilterTo_date.Text = ""
            pnl_filter.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1
            dgv_filter.Rows.Clear()


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


        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Trip_Sheet_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Trip_Sheet_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub



        Try

            inpno = InputBox("Enter New Vou .No.", "FOR INSERTION...")

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.CommandText = "select Cheque_No from Cheque_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cheque_Code = '" & Trim(NewCode) & "'"
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
                    MessageBox.Show("Invalid Vou.No.", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_ReceiptNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String = ""

        Try

            cmd.Connection = con
            cmd.CommandText = "select top 1 Cheque_No from Cheque_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cheque_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Cheque_No"
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
            cmd.CommandText = "select top 1 Cheque_No from Cheque_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cheque_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Cheque_No desc"
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_ReceiptNo.Text))

            cmd.Connection = con
            cmd.CommandText = "select top 1 Cheque_No from Cheque_Head where for_orderby > " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cheque_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Cheque_No"
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

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String = ""
        Dim OrdByNo As Single = 0

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_ReceiptNo.Text))

            cmd.Connection = con
            cmd.CommandText = "select top 1 Cheque_No from Cheque_Head where for_orderby < " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and  Cheque_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc,Cheque_No desc"
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
        Dim dt1 As New DataTable
        Dim ChNo As Integer = 0
        Dim Bnk_id As Integer = 0

        Try

            clear()

            New_Entry = True


            lbl_ReceiptNo.Text = Common_Procedures.get_MaxCode(con, "Cheque_Head", "Cheque_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)
            lbl_ReceiptNo.ForeColor = Color.Red

            msk_date.Text = Date.Today.ToShortDateString
            da = New SqlClient.SqlDataAdapter("select top 1 * from Cheque_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cheque_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Cheque_No desc", con)
            dt1 = New DataTable
            da.Fill(dt1)
            If dt1.Rows.Count > 0 Then
                If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)
                    If dt1.Rows(0).Item("Cheque_Date").ToString <> "" Then msk_date.Text = dt1.Rows(0).Item("Cheque_Date").ToString

                End If
                If Val(dt1.Rows(0).Item("Bank_idNo").ToString) <> 0 Then
                    cbo_Bank.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Bank_IdNo").ToString))
                    Bnk_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Bank.Text)

                    da = New SqlClient.SqlDataAdapter("select max(ChequeNo) from Cheque_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Bank_IdNo =  " & Val(Bnk_id) & " AND Cheque_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' ", con)
                    dt1 = New DataTable
                    da.Fill(dt1)

                    ChNo = 0
                    If dt1.Rows.Count > 0 Then
                        If IsDBNull(dt1.Rows(0)(0).ToString) = False Then
                            ChNo = Val(dt1.Rows(0)(0).ToString)
                        End If
                    End If

                    ChNo = ChNo + 1
                    txt_ChequeNo.Text = ChNo
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
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String, inpno As String
        Dim NewCode As String

        Try

            inpno = InputBox("Enter Vou.No", "FOR FINDING...")

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.CommandText = "select Cheque_No from Cheque_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cheque_Code = '" & Trim(NewCode) & "'"
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
                MessageBox.Show("Vou.No. Does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

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
        Dim Bnk_ID As Integer = 0
        Dim Partcls As String
        Dim PBlNo As String
        Dim EntID As String
        Dim ChNo As Integer = 0
        Dim LedSurName As String = ""
        Dim acgrp_idno As Integer = 0
        Dim Parnt_CD As String = ""
        Dim Agt_Idno As Integer = 0

        If pnl_back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If


        '---If Common_Procedures.UserRight_Check(Common_Procedures.UR.Trip_Sheet_Entry, New_Entry) = False Then Exit Sub

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(msk_date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If msk_date.Enabled Then msk_date.Focus()
            Exit Sub
        End If

        If Not (Convert.ToDateTime(msk_date.Text) >= Common_Procedures.Company_FromDate And Convert.ToDateTime(msk_date.Text) <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If msk_date.Enabled Then msk_date.Focus()
            Exit Sub
        End If


        Bnk_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Bank.Text)

        If Bnk_ID = 0 Then
            MessageBox.Show("Invalid Bank Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Bank.Enabled Then cbo_Bank.Focus()
            Exit Sub
        End If

        led_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyName.Text)
        'If led_id = 0 Then
        '    MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    If cbo_PartyName.Enabled Then cbo_PartyName.Focus()
        '    Exit Sub
        'End If

        If Trim(cbo_ACPayee.Text) = "" Then
            MessageBox.Show("Invalid A/C Payee or Name Cheque", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_ACPayee.Enabled Then cbo_ACPayee.Focus()
            Exit Sub
        End If

        If Val(txt_ChequeAmt.Text) = 0 Then
            MessageBox.Show("Invalid Cheque Amount", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If txt_ChequeAmt.Enabled Then txt_ChequeAmt.Focus()
            Exit Sub
        End If


        If Val(txt_ChequeNo.Text) = 0 Then
            MessageBox.Show("Invalid Cheque No", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If txt_ChequeNo.Enabled Then txt_ChequeNo.Focus()
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ReceiptNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        da = New SqlClient.SqlDataAdapter("select * from Cheque_Head Where Company_Idno = " & Val(lbl_Company.Tag) & " and Bank_Idno = " & Val(Bnk_ID) & " and ChequeNo = " & Str(Val(txt_ChequeNo.Text)) & " and Cheque_Code <> '" & Trim(NewCode) & "'", con)
        dt1 = New DataTable
        da.Fill(dt1)

        ChNo = 0
        If dt1.Rows.Count > 0 Then
            'If Val(dt1.Rows(0).Item("Bank_idNo").ToString) <> 0 Then
            ChNo = Val(dt1.Rows(0).Item("ChequeNo").ToString)

            'If Val(txt_ChequeNo.Text) = Val(ChNo) And Val(Bnk_ID) = Val(dt1.Rows(0).Item("Bank_idNo").ToString) Then
            MessageBox.Show("Duplicate Cheque No", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If txt_ChequeNo.Enabled Then txt_ChequeNo.Focus()
            Exit Sub
            'End If
            'End If
        End If
        dt1.Clear()
        dt1.Dispose()



        ' Agt_Idno = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_AgentName.Text)

        acgrp_idno = 10
        Parnt_CD = "~10~4~"

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ReceiptNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                da = New SqlClient.SqlDataAdapter("select max(for_orderby) from Cheque_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cheque_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' ", con)
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
                If Trim(NewNo) = "" Then NewNo = Trim(lbl_ReceiptNo.Text)

                lbl_ReceiptNo.Text = Trim(NewNo)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ReceiptNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@ChequeDate", Convert.ToDateTime(msk_date.Text))

            LedSurName = Common_Procedures.Remove_NonCharacters(cbo_PartyName.Text)

            'led_id = Val(Common_Procedures.get_FieldValue(con, "ledger_head", "ledger_idno", "(Sur_Name = '" & Trim(LedSurName) & "')", , tr))
            If led_id = 0 Then

                led_id = Common_Procedures.get_MaxIdNo(con, "ledger_head", "ledger_idno", "", tr)
                If Val(led_id) < 101 Then led_id = 101

                cmd.CommandText = "Insert into ledger_head (          Ledger_IdNo   ,               Ledger_Name         ,           Sur_Name        ,            Ledger_MainName        , Ledger_AlaisName , Area_IdNo,      AccountsGroup_IdNo     ,        Parent_Code      ,   Bill_Type   , Ledger_Address1, Ledger_Address2, Ledger_Address3, Ledger_Address4, Ledger_PhoneNo, Ledger_TinNo, Ledger_CstNo, Ledger_Type ) " & _
                                    "          Values      (" & Str(Val(led_id)) & ", '" & Trim(cbo_PartyName.Text) & "', '" & Trim(LedSurName) & "', '" & Trim(cbo_PartyName.Text) & "',       ''         ,     0    , " & Str(Val(acgrp_idno)) & ", '" & Trim(Parnt_CD) & "', 'BALANCE ONLY',       ''       ,        ''      ,        ''      ,        ''      ,    ''         ,     ''      ,     ''      ,     ''      )"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Insert into Ledger_AlaisHead (      Ledger_IdNo       , Sl_No,             Ledger_DisplayName    , Ledger_Type,      AccountsGroup_IdNo      ) " & _
                                    "          Values           (" & Str(Val(led_id)) & ",   1  , '" & Trim(cbo_PartyName.Text) & "',     ''     , " & Str(Val(acgrp_idno)) & " ) "
                cmd.ExecuteNonQuery()

            End If

            If New_Entry = True Then

                cmd.CommandText = "Insert into Cheque_Head(Cheque_Code, Company_IdNo, Cheque_No, for_OrderBy,Cheque_Date ,Cheque_Day  , Bank_IdNo     ,  Ledger_IdNo      , ACPayee_NameCheque      ,           Cheque_Amount        ,       ChequeNo     ,  Narration ,  Print_Name )  Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_ReceiptNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_ReceiptNo.Text))) & ", @ChequeDate, '" & Trim(lbl_Day.Text) & "', " & Val(Bnk_ID) & " ," & Val(led_id) & "  ,  '" & Trim(cbo_ACPayee.Text) & "' , " & Val(txt_ChequeAmt.Text) & " , " & Val(txt_ChequeNo.Text) & " , '" & Trim(txt_Narration.Text) & "' , '" & Trim(txt_Print_Name.Text) & "'  )"
                cmd.ExecuteNonQuery()

            Else

                cmd.CommandText = "Update Cheque_Head set Cheque_Date = @ChequeDate,Cheque_Day  = '" & Trim(lbl_Day.Text) & "' , Bank_IdNo = " & Val(Bnk_ID) & ", Ledger_IdNo = " & Val(led_id) & "  , ACPayee_NameCheque =  '" & Trim(cbo_ACPayee.Text) & "' ,Cheque_Amount = " & Val(txt_ChequeAmt.Text) & "  , ChequeNo = " & Val(txt_ChequeNo.Text) & " , Narration = '" & Trim(txt_Narration.Text) & "' ,  Print_Name =  '" & Trim(txt_Print_Name.Text) & "'    Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Cheque_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If

            tr.Commit()

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)

            move_record(lbl_ReceiptNo.Text)
            'If New_Entry = True Then new_record()

        Catch ex As Exception
            tr.Rollback()



            MessageBox.Show(ex.Message, "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()

    End Sub

    Private Sub cbo_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(ledger_type <> 'BANK')", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_partyname_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PartyName, cbo_Bank, txt_Print_Name, "Ledger_AlaisHead", "Ledger_DisplayName", "(ledger_type <> 'BANK')", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_partyname_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PartyName, txt_Print_Name, "Ledger_AlaisHead", "Ledger_DisplayName", "(ledger_type <> 'BANK')", "(Ledger_idno = 0)", False)
    End Sub

    Private Sub cbo_partyname_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyName.KeyUp
        'If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
        '    Common_Procedures.MDI_LedType = ""
        '    Dim f As New Ledger_Creation

        '    Common_Procedures.Master_Return.Form_Name = Me.Name
        '    Common_Procedures.Master_Return.Control_Name = cbo_PartyName.Name
        '    Common_Procedures.Master_Return.Return_Value = ""
        '    Common_Procedures.Master_Return.Master_Type = ""

        '    f.MdiParent = MDIParent1
        '    f.Show()

        'End If

    End Sub

    Private Sub btn_Save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub btn_print_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        print_record()
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
        Dim Led_Idno As Integer, Itm_IdNo As Integer
        Dim Condt As String = ""

        Try

            Condt = ""
            Led_Idno = 0
            Itm_IdNo = 0

            If IsDate(dtp_FilterFrom_date.Value) = True And IsDate(dtp_FilterTo_date.Value) = True Then
                Condt = "a.Cheque_Date between '" & Trim(Format(dtp_FilterFrom_date.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_FilterTo_date.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_FilterFrom_date.Value) = True Then
                Condt = "a.Cheque_Date = '" & Trim(Format(dtp_FilterFrom_date.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_FilterTo_date.Value) = True Then
                Condt = "a. Cheque_Date= '" & Trim(Format(dtp_FilterTo_date.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_Idno = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If

            If Val(Led_Idno) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " (a.Ledger_Idno = " & Str(Val(Led_Idno)) & ")"
            End If


            da = New SqlClient.SqlDataAdapter("select a.* from Cheque_Head a   where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Cheque_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Cheque_No", con)
            da.Fill(dt2)

            dgv_filter.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_filter.Rows.Add()

                    dgv_filter.Rows(n).Cells(0).Value = " " & dt2.Rows(i).Item("Cheque_No").ToString
                    dgv_filter.Rows(n).Cells(1).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Cheque_Date").ToString), "dd-MM-yyyy")
                    dgv_filter.Rows(n).Cells(2).Value = Common_Procedures.Ledger_IdNoToName(con, Val(dt2.Rows(i).Item("Bank_IdNo").ToString))
                    dgv_filter.Rows(n).Cells(3).Value = Common_Procedures.Ledger_IdNoToName(con, Val(dt2.Rows(i).Item("Ledger_IdNo").ToString))
                    dgv_filter.Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("Cheque_Amount").ToString), "##########0.00")

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



    Private Sub dgv_filter_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_filter.CellEndEdit
        SendKeys.Send("{UP}")
        SendKeys.Send("{TAB}")
    End Sub

    Private Sub dgv_filter_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_filter.CellEnter
        With dgv_filter

            If Val(.Rows(e.RowIndex).Cells(0).Value) = 0 Then
                .Rows(e.RowIndex).Cells(0).Value = e.RowIndex + 1
            End If
        End With
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

        movno = Trim(dgv_filter.CurrentRow.Cells(0).Value)

        If Val(movno) <> 0 Then
            Filter_Status = True
            move_record(movno)
            pnl_back.Enabled = True
            pnl_filter.Visible = False
        End If

    End Sub


    Public Sub print_record() Implements Interface_MDIActions.print_record
        printing_Cheque()
    End Sub

    Private Sub txt_ChequeNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_ChequeNo.GotFocus

    End Sub

    Private Sub txt_ChequeNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_ChequeNo.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub btn_close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub



    Private Sub cbo_Filter_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_PartyName.GotFocus

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", " ", "(Ledger_idno = 0)")

    End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, dtp_FilterTo_date, btn_filtershow, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_idno = 0)")


    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, btn_filtershow, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_idno = 0)")

    End Sub

    Private Sub msk_date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles msk_date.KeyPress
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            'dgv_Details.Focus()
            'dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(0)
        End If
    End Sub

    Private Sub msk_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_date.KeyUp
        Dim vmRetTxt As String = ""
        Dim vmRetSelStrt As Integer = -1

        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            msk_date.Text = Date.Today
        End If
        If e.KeyCode = 107 Then
            msk_date.Text = DateAdd("D", 1, Convert.ToDateTime(msk_date.Text))
        ElseIf e.KeyCode = 109 Then
            msk_date.Text = DateAdd("D", -1, Convert.ToDateTime(msk_date.Text))
        End If

        If e.KeyCode = 46 Or e.KeyCode = 8 Then

            Common_Procedures.maskEdit_Date_ON_DelBackSpace(sender, e, vmskOldText, vmskSelStrt)

            'If e.KeyCode = 46 Then
            '    'If vmskSelStrt > 0 Then
            '    If vmskSelStrt <= 2 Then
            '        vmRetTxt = "  " & Microsoft.VisualBasic.Mid(vmskOldText, 3, Len(vmskOldText))
            '        vmRetSelStrt = 0
            '    ElseIf vmskSelStrt >= 3 And vmskSelStrt <= 5 Then
            '        vmRetTxt = Microsoft.VisualBasic.Left(vmskOldText, 3) & "  " & Microsoft.VisualBasic.Mid(vmskOldText, 6, Len(vmskOldText))
            '        vmRetSelStrt = 3
            '    Else
            '        vmRetTxt = Microsoft.VisualBasic.Left(vmskOldText, 6)
            '        vmRetSelStrt = 6
            '    End If

            '    'If Microsoft.VisualBasic.Mid(vmskOldText, vmskSelStrt + 1, 1) = "-" Then
            '    '    vmRetTxt = Microsoft.VisualBasic.Left(vmskOldText, vmskSelStrt + 1) & "  " & Microsoft.VisualBasic.Mid(vmskOldText, vmskSelStrt + 4, Len(vmskOldText))
            '    'Else
            '    '    vmRetTxt = Microsoft.VisualBasic.Left(vmskOldText, vmskSelStrt) & "  " & Microsoft.VisualBasic.Mid(vmskOldText, vmskSelStrt + 3, Len(vmskOldText))
            '    'End If

            '    'Else

            '    'End If

            '    msk_Date.Text = vmRetTxt
            '    msk_Date.SelectionStart = vmRetSelStrt

            '    'If Microsoft.VisualBasic.Mid(vmskOldText, vmskSelStrt + 1, 1) = "-" Then
            '    '    msk_Date.SelectionStart = vmskSelStrt + 1
            '    'Else
            '    '    msk_Date.SelectionStart = vmskSelStrt
            '    'End If

            'ElseIf e.KeyCode = 8 Then
            '    If vmskSelStrt > 0 Then
            '        vmRetTxt = Microsoft.VisualBasic.Left(vmskOldText, vmskSelStrt - 1) & " " & Microsoft.VisualBasic.Mid(vmskOldText, vmskSelStrt + 1, Len(vmskOldText))
            '    Else
            '        'vmRetTxt = ""
            '        vmRetTxt = vmskOldText
            '    End If

            '    msk_Date.Text = vmRetTxt

            '    If vmskSelStrt > 0 Then
            '        msk_Date.SelectionStart = vmskSelStrt - 1
            '    End If

            'End If

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
        lbl_Day.Text = Trim(Format(dtp_Date.Value, "dddddd"))
    End Sub

    Private Sub dtp_Date_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_Date.TextChanged
        lbl_Day.Text = ""
        If IsDate(dtp_Date.Text) = True Then
            lbl_Day.Text = Format(Convert.ToDateTime(dtp_Date.Text), "dddd").ToString
            msk_date.Text = dtp_Date.Text
            msk_date.SelectionStart = 0
        End If
    End Sub

    Private Sub msk_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_date.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        If e.KeyCode = 40 Then
            e.Handled = True : e.SuppressKeyPress = True
            'dgv_Details.Focus()
            'dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(0)
        End If

        If e.KeyCode = 38 Then
            e.Handled = True : e.SuppressKeyPress = True
            'txt_Narration.Focus()
        End If

        vmskOldText = ""
        vmskSelStrt = -1
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            vmskOldText = msk_date.Text
            vmskSelStrt = msk_date.SelectionStart
        End If

    End Sub

    Private Sub cbo_Bank_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Bank.GotFocus
        With cbo_Bank
            'Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = 'BANK'  ) ", "(Ledger_idno = 0)")
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_Idno = 23) ", "(Ledger_idno = 0)")
            cmbnkNm = cbo_Bank.Text
        End With

    End Sub


    Private Sub cbo_Bank_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Bank.KeyDown

        vcbo_KeyDwnVal = e.KeyValue

        'Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Bank, msk_date, cbo_PartyName, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = 'BANK'  ) ", "(Ledger_idno = 0)")
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Bank, msk_date, cbo_PartyName, "Ledger_AlaisHead", "Ledger_DisplayName", " (AccountsGroup_Idno = 23) ", "(Ledger_idno = 0)")


    End Sub

    Private Sub cbo_Bank_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Bank.KeyPress


        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Bank, cbo_PartyName, "Ledger_AlaisHead", "Ledger_DisplayName", " (AccountsGroup_Idno = 23)   ", "(Ledger_idno = 0)")
        'Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Bank, cbo_PartyName, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = 'BANK'  ) ", "(Ledger_idno = 0)")



    End Sub

    Private Sub cbo_Driver_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Bank.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Common_Procedures.MDI_LedType = "BANK"
            'Dim f As New Bank_Creation
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Bank.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub


    Private Sub cbo_ACPayee_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ACPayee.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")

    End Sub

    Private Sub cbo_ACPayee_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ACPayee.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_ACPayee, txt_Print_Name, txt_ChequeAmt, "", "", "", "")

    End Sub

    Private Sub cbo_ACPayee_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_ACPayee.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_ACPayee, txt_ChequeAmt, "", "", "", "")

    End Sub


    Private Sub txt_Narration_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Narration.KeyDown
        If e.KeyValue = 40 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                msk_date.Focus()
            End If
        End If
        If e.KeyValue = 38 Then
            txt_ChequeNo.Focus()
        End If
    End Sub




    Private Sub txt_Narration_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Narration.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                msk_date.Focus()
            End If
        End If
    End Sub

    Private Sub printing_Cheque()
        Dim Da1 As SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Da2 As SqlClient.SqlDataAdapter
        Dim Dt2 As New DataTable
        Dim entcode As String
        Dim PpSzSTS As Boolean = False
        Dim I As Integer = 0
        Dim ps As Printing.PaperSize

        entcode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ReceiptNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            Da1 = New SqlClient.SqlDataAdapter("Select a.*,B.*,c.*  from cHEQUE_head a, ledger_head c, Company_Head b where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Cheque_Code = '" & Trim(entcode) & "' and a.Ledger_idno = c.ledger_idno", con)
            Dt1 = New DataTable
            Da1.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                Da2 = New SqlClient.SqlDataAdapter("Select *  from Cheque_Print_Positioning_Head where Ledger_IdNo = " & Str(Val(Dt1.Rows(0).Item("bANK_Idno").ToString)) & " order by Cheque_Print_Positioning_No", con)
                Dt2 = New DataTable
                Da2.Fill(Dt2)
                If Dt2.Rows.Count <= 0 Then
                    MessageBox.Show("Cheque Printing Position not Found ", "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If
                Dt2.Clear()

            Else
                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub

            End If
            Dt1.Clear()


            Dt1.Dispose()
            Da1.Dispose()
            Dt2.Dispose()
            Da2.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub

        End Try

        For I = 0 To PrintDocument2.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument2.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument2.PrinterSettings.PaperSizes(I)
                PrintDocument2.PrinterSettings.DefaultPageSettings.PaperSize = ps
                PrintDocument2.DefaultPageSettings.PaperSize = ps

                PrintDocument2.PrinterSettings.DefaultPageSettings.Landscape = True
                PrintDocument2.DefaultPageSettings.Landscape = True

                Exit For
            End If
        Next

        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then

            Try

                PrintDialog1.PrinterSettings = PrintDocument2.PrinterSettings
                If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                    PrintDocument2.PrinterSettings = PrintDialog1.PrinterSettings
                    PrintDocument2.Print()
                End If

            Catch ex As Exception
                MessageBox.Show("The printing operation failed" & vbCrLf & ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End Try


        Else

            Try

                PrintDocument2.DefaultPageSettings.Landscape = True

                Dim ppd As New PrintPreviewDialog

                ppd.Document = PrintDocument2

                ppd.WindowState = FormWindowState.Normal
                ppd.StartPosition = FormStartPosition.CenterScreen
                ppd.ClientSize = New Size(600, 600)

                ppd.ShowDialog()


            Catch ex As Exception
                MessageBox.Show("The printing operation failed" & vbCrLf & ex.Message, "DOES NOT SHOW PRINT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End Try

        End If

    End Sub

    Private Sub PrintDocument2_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument2.BeginPrint
        Dim Da1 As SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim prn_CheqDet As New DataTable
        'Dim ps As Printing.PaperSize
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim PpSzSTS As Boolean = False
        Dim entcode As String = ""

        Try

            entcode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ReceiptNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            prn_HdDt = New DataTable
            prn_PageNo = 0
            prn_HeadIndx = 0

            Da1 = New SqlClient.SqlDataAdapter("Select a.*, b.*,c.*  from Cheque_head a, ledger_head c, Company_Head b where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Cheque_Code = '" & Trim(entcode) & "' and a.Ledger_idno = c.ledger_idno and a.Company_IdNo = b.Company_IdNo", con)
            prn_HdDt = New DataTable
            Da1.Fill(prn_HdDt)
            If prn_HdDt.Rows.Count <= 0 Then

                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If

            'Da1 = New SqlClient.SqlDataAdapter("Select *  from Cheque_Print_Positioning_Head where Ledger_IdNo = " & Str(Val(prn_HdDt.Rows(prn_HeadIndx).Item("Creditor_Idno").ToString)) & "order by Cheque_Print_Positioning_No", con)
            'prn_CheqDet = New DataTable
            'Da1.Fill(prn_CheqDet)

            'If prn_CheqDet.Rows.Count > 0 Then
            '    If Trim(prn_CheqDet.Rows(0).Item("Paper_Orientation").ToString) <> "" Then
            '        If Val(Common_Procedures.Print_OR_Preview_Status) = 2 Then
            '            If Trim(prn_CheqDet.Rows(0).Item("Paper_Orientation").ToString) = "LANDSCAPE" Then
            '                PrintDocument2.DefaultPageSettings.Landscape = True
            '                If PrintDocument2.DefaultPageSettings.Landscape = True Then
            '                    With PrintDocument2.DefaultPageSettings.PaperSize
            '                        PrintWidth = .Height - TMargin - BMargin
            '                        PrintHeight = .Width - RMargin - LMargin
            '                        PageWidth = .Height - TMargin
            '                        PageHeight = .Width - RMargin
            '                    End With
            '                End If
            '            Else
            '                With PrintDocument2.DefaultPageSettings.PaperSize
            '                    PrintWidth = .Width - RMargin - LMargin
            '                    PrintHeight = .Height - TMargin - BMargin
            '                    PageWidth = .Width - RMargin
            '                    PageHeight = .Height - BMargin
            '                End With
            '            End If
            '        End If
            '    End If

            'End If

            'For I = 0 To PrintDocument2.PrinterSettings.PaperSizes.Count - 1
            '    If PrintDocument2.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
            '        ps = PrintDocument2.PrinterSettings.PaperSizes(I)
            '        PrintDocument2.PrinterSettings.DefaultPageSettings.PaperSize = ps
            '        PrintDocument2.DefaultPageSettings.PaperSize = ps
            '        Exit For
            '    End If
            'Next

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub PrintDocument2_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument2.PrintPage
        If prn_HdDt.Rows.Count <= 0 Then Exit Sub
        Printing_Format3_ChequePrint(e)
    End Sub

    Private Sub Printing_Format3_ChequePrint(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim Da1 As SqlClient.SqlDataAdapter
        Dim prn_CheqDet As New DataTable
        Dim pFont As Font, p1Font As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim L1 As Single, T1 As Single, L2 As Single, T2 As Single
        Dim CurX As Double = 0
        Dim CurY As Double = 0
        Dim CurZ As Double = 0
        Dim TxtHgt As Single = 0
        Dim ps As Printing.PaperSize
        Dim W As Single = 0
        Dim dtWdth As Single = 0
        Dim Amt As String = ""
        Dim Rup1 As String = "", Rup2 As String = ""
        Dim m As Integer = 0
        Dim PrtyNm1 As String = "", PrtyNm2 As String = ""
        Dim I As Integer = 0

        For I = 0 To PrintDocument2.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument2.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument2.PrinterSettings.PaperSizes(I)
                PrintDocument2.PrinterSettings.DefaultPageSettings.PaperSize = ps
                PrintDocument2.DefaultPageSettings.PaperSize = ps
                e.PageSettings.PaperSize = ps
                Exit For
            End If
        Next
        PrintDocument2.DefaultPageSettings.Landscape = False

        With PrintDocument2.DefaultPageSettings.Margins
            .Left = 0
            .Right = 0
            .Top = 0
            .Bottom = 0
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        Da1 = New SqlClient.SqlDataAdapter("Select *  from Cheque_Print_Positioning_Head where Ledger_IdNo = " & Str(Val(prn_HdDt.Rows(0).Item("Bank_Idno").ToString)) & "order by Cheque_Print_Positioning_No", con)
        prn_CheqDet = New DataTable
        Da1.Fill(prn_CheqDet)
        If prn_CheqDet.Rows.Count > 0 Then

            If Trim(prn_CheqDet.Rows(0).Item("Paper_Orientation").ToString) <> "" Then
                'If Val(Common_Procedures.Print_OR_Preview_Status) = 2 Then
                If Trim(UCase(prn_CheqDet.Rows(0).Item("Paper_Orientation").ToString)) = "LANDSCAPE" Then
                    PrintDocument2.DefaultPageSettings.Landscape = True
                End If
                'End If
            End If

        Else

            MessageBox.Show("Cheque Print position for entered", "DOES NOT PRINT CHEQUE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            e.HasMorePages = False
            Exit Sub

        End If

        pFont = New Font("arial", 12, FontStyle.Regular)
        'pFont = New Font("Calibri", 11, FontStyle.Bold)
        TxtHgt = 20 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        If Val(prn_CheqDet.Rows(0).Item("Left_Margin").ToString) <> 0 Then
            LMargin = LMargin + (Val(prn_CheqDet.Rows(0).Item("Left_Margin").ToString) / 2.54 * 100)
        End If
        If Val(prn_CheqDet.Rows(0).Item("Top_Margin").ToString) <> 0 Then
            TMargin = TMargin + (Val(prn_CheqDet.Rows(0).Item("Top_Margin").ToString) / 2.54 * 100)
        End If

        If Val(prn_CheqDet.Rows(0).Item("Ac_Payee_Left").ToString) <> 0 And Val(prn_CheqDet.Rows(0).Item("Ac_Payee_Top").ToString) <> 0 Then
            ' If Trim(LCase(Common_Procedures.VoucherType)) <> "" And Val(prn_HdDt.Rows(0).Item("Ledger_Idno").ToString) <> 1 Then
            If Trim(prn_HdDt.Rows(0).Item("ACPayee_NameCheque").ToString) = "A/C PAYEE" And Val(prn_HdDt.Rows(0).Item("Ledger_Idno").ToString) <> 1 Then
                CurX = Val(prn_CheqDet.Rows(0).Item("Ac_Payee_Left").ToString) / 2.54 * 100
                CurY = Val(prn_CheqDet.Rows(0).Item("Ac_Payee_Top").ToString) / 2.54 * 100
                p1Font = New Font("arial", 10, FontStyle.Bold)
                'p1Font = New Font("Calibri", 9, FontStyle.Bold)
                L1 = 0 : T1 = 0 : L2 = 0 : T2 = 0
                L1 = LMargin + CurX
                T1 = TMargin + CurY - 1
                T2 = TMargin + CurY - 1
                L2 = LMargin + CurX + 75
                e.Graphics.DrawLine(Pens.Black, L1, T1, L2, T2)

                Common_Procedures.Print_To_PrintDocument(e, "A/C PAYEE", LMargin + CurX, TMargin + CurY, 0, 0, p1Font)

                L1 = 0 : T1 = 0 : L2 = 0 : T2 = 0
                L1 = LMargin + CurX
                T1 = TMargin + CurY + TxtHgt + 0.5
                T2 = TMargin + CurY + TxtHgt + 0.5
                L2 = LMargin + CurX + 75
                e.Graphics.DrawLine(Pens.Black, L1, T1, L2, T2)

            End If
        End If

        CurX = Val(prn_CheqDet.Rows(0).Item("Date_Left").ToString) / 2.54 * 100
        CurY = Val(prn_CheqDet.Rows(0).Item("Date_Top").ToString) / 2.54 * 100
        dtWdth = Val(prn_CheqDet.Rows(0).Item("Date_Width").ToString) / 2.54 * 100

        If Val(prn_CheqDet.Rows(0).Item("Date_Width").ToString) > 0 Then
            W = CurX
            Common_Procedures.Print_To_PrintDocument(e, Trim(Mid(Format(prn_HdDt.Rows(0).Item("Cheque_Date"), "dd-MM-yyyy"), 1, 1)), LMargin + W, TMargin + CurY, 0, 0, pFont)
            W = W + dtWdth
            Common_Procedures.Print_To_PrintDocument(e, Trim(Mid(Format(prn_HdDt.Rows(0).Item("Cheque_Date"), "dd-MM-yyyy"), 2, 1)), LMargin + W, TMargin + CurY, 0, 0, pFont)
            W = W + dtWdth

            Common_Procedures.Print_To_PrintDocument(e, Trim(Mid(Format(prn_HdDt.Rows(0).Item("Cheque_Date"), "dd-MM-yyyy"), 4, 1)), LMargin + W, TMargin + CurY, 0, 0, pFont)
            W = W + dtWdth
            Common_Procedures.Print_To_PrintDocument(e, Trim(Mid(Format(prn_HdDt.Rows(0).Item("Cheque_Date"), "dd-MM-yyyy"), 5, 1)), LMargin + W, TMargin + CurY, 0, 0, pFont)
            W = W + dtWdth

            Common_Procedures.Print_To_PrintDocument(e, Trim(Mid(Format(prn_HdDt.Rows(0).Item("Cheque_Date"), "dd-MM-yyyy"), 7, 1)), LMargin + W, TMargin + CurY, 0, 0, pFont)
            W = W + dtWdth
            Common_Procedures.Print_To_PrintDocument(e, Trim(Mid(Format(prn_HdDt.Rows(0).Item("Cheque_Date"), "dd-MM-yyyy"), 8, 1)), LMargin + W, TMargin + CurY, 0, 0, pFont)
            W = W + dtWdth
            Common_Procedures.Print_To_PrintDocument(e, Trim(Mid(Format(prn_HdDt.Rows(0).Item("Cheque_Date"), "dd-MM-yyyy"), 9, 1)), LMargin + W, TMargin + CurY, 0, 0, pFont)
            W = W + dtWdth
            Common_Procedures.Print_To_PrintDocument(e, Trim(Mid(Format(prn_HdDt.Rows(0).Item("Cheque_Date"), "dd-MM-yyyy"), 10, 1)), LMargin + W, TMargin + CurY, 0, 0, pFont)

        Else

            Common_Procedures.Print_To_PrintDocument(e, Format(prn_HdDt.Rows(0).Item("Cheque_Date"), "dd-MM-yyyy"), LMargin + CurX, TMargin + CurY, 0, 0, pFont)

        End If

        CurX = Val(prn_CheqDet.Rows(0).Item("PartyName_Left").ToString) / 2.54 * 100
        CurY = Val(prn_CheqDet.Rows(0).Item("PartyName_Top").ToString) / 2.54 * 100
        CurZ = Val(prn_CheqDet.Rows(0).Item("PartyName_Width").ToString)

        If Val(prn_HdDt.Rows(0).Item("Ledger_Idno").ToString) = 1 Then
            Common_Procedures.Print_To_PrintDocument(e, "Self****", LMargin + CurX, TMargin + CurY, 0, 0, pFont)

        ElseIf Trim(prn_HdDt.Rows(0).Item("ACPayee_NameCheque").ToString) = "RTGS" Then

            PrtyNm2 = ""


            If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Print_Name").ToString) <> "" Then
                PrtyNm1 = "YOUR SELF RTGS FOR " & Trim(prn_HdDt.Rows(0).Item("Print_Name").ToString) & "****"
            Else
                PrtyNm1 = "YOUR SELF RTGS FOR " & Trim(prn_HdDt.Rows(0).Item("Ledger_name").ToString) & "****"
            End If

            'PrtyNm1 = "YOUR SELF RTGS FOR " & Trim(prn_HdDt.Rows(0).Item("Ledger_name").ToString) & "****"


            If CurZ > 0 Then
                If Len(PrtyNm1) > CurZ Then
                    For m = CurZ To 1 Step -1
                        If Mid$(Trim(PrtyNm1), m, 1) = " " Or Mid$(Trim(PrtyNm1), m, 1) = "." Or Mid$(Trim(PrtyNm1), m, 1) = "," Or Mid$(Trim(PrtyNm1), m, 1) = "/" Or Mid$(Trim(PrtyNm1), m, 1) = "-" Or Mid$(Trim(PrtyNm1), m, 1) = "'" Or Mid$(Trim(PrtyNm1), m, 1) = """" Or Mid$(Trim(PrtyNm1), m, 1) = "&" Or Mid$(Trim(PrtyNm1), m, 1) = "(" Or Mid$(Trim(PrtyNm1), m, 1) = ")" Then Exit For
                    Next m
                    If m = 0 Then m = CurZ
                    PrtyNm2 = Microsoft.VisualBasic.Right(Trim(PrtyNm1), Len(PrtyNm1) - m)
                    PrtyNm1 = Microsoft.VisualBasic.Left(Trim(PrtyNm1), m - 1)
                End If
            End If
            Common_Procedures.Print_To_PrintDocument(e, PrtyNm1, LMargin + CurX, TMargin + CurY, 0, CurZ, pFont)

            If Trim(PrtyNm2) <> "" Then
                If Val(prn_CheqDet.Rows(0).Item("Second_PartyName_left").ToString) <> 0 And Val(prn_CheqDet.Rows(0).Item("Second_PartyName_Top").ToString) Then
                    CurX = Val(prn_CheqDet.Rows(0).Item("Second_PartyName_left").ToString) / 2.54 * 100
                    CurY = Val(prn_CheqDet.Rows(0).Item("Second_PartyName_Top").ToString) / 2.54 * 100

                Else
                    CurY = CurY + TxtHgt

                End If
                Common_Procedures.Print_To_PrintDocument(e, PrtyNm2, LMargin + CurX, TMargin + CurY, 0, CurZ, pFont)
            End If

        ElseIf Trim(prn_HdDt.Rows(0).Item("ACPayee_NameCheque").ToString) = "NEFT" Then

            PrtyNm2 = ""

            If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Print_Name").ToString) <> "" Then
                PrtyNm1 = "YOUR SELF NEFT FOR " & Trim(prn_HdDt.Rows(0).Item("Print_Name").ToString) & "****"
            Else
                PrtyNm1 = "YOUR SELF NEFT FOR " & Trim(prn_HdDt.Rows(0).Item("Ledger_name").ToString) & "****"
            End If

            'PrtyNm1 = "YOUR SELF NEFT FOR " & Trim(prn_HdDt.Rows(0).Item("Ledger_name").ToString) & "****"


            If CurZ > 0 Then
                If Len(PrtyNm1) > CurZ Then
                    For m = CurZ To 1 Step -1
                        If Mid$(Trim(PrtyNm1), m, 1) = " " Or Mid$(Trim(PrtyNm1), m, 1) = "." Or Mid$(Trim(PrtyNm1), m, 1) = "," Or Mid$(Trim(PrtyNm1), m, 1) = "/" Or Mid$(Trim(PrtyNm1), m, 1) = "-" Or Mid$(Trim(PrtyNm1), m, 1) = "'" Or Mid$(Trim(PrtyNm1), m, 1) = """" Or Mid$(Trim(PrtyNm1), m, 1) = "&" Or Mid$(Trim(PrtyNm1), m, 1) = "(" Or Mid$(Trim(PrtyNm1), m, 1) = ")" Then Exit For
                    Next m
                    If m = 0 Then m = CurZ
                    PrtyNm2 = Microsoft.VisualBasic.Right(Trim(PrtyNm1), Len(PrtyNm1) - m)
                    PrtyNm1 = Microsoft.VisualBasic.Left(Trim(PrtyNm1), m - 1)
                End If
            End If
            Common_Procedures.Print_To_PrintDocument(e, PrtyNm1, LMargin + CurX, TMargin + CurY, 0, CurZ, pFont)

            If Trim(PrtyNm2) <> "" Then
                If Val(prn_CheqDet.Rows(0).Item("Second_PartyName_left").ToString) <> 0 And Val(prn_CheqDet.Rows(0).Item("Second_PartyName_Top").ToString) Then
                    CurX = Val(prn_CheqDet.Rows(0).Item("Second_PartyName_left").ToString) / 2.54 * 100
                    CurY = Val(prn_CheqDet.Rows(0).Item("Second_PartyName_Top").ToString) / 2.54 * 100

                Else
                    CurY = CurY + TxtHgt

                End If
                Common_Procedures.Print_To_PrintDocument(e, PrtyNm2, LMargin + CurX, TMargin + CurY, 0, CurZ, pFont)
            End If

        Else


            PrtyNm2 = ""

            If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Print_Name").ToString) <> "" Then
                PrtyNm1 = Trim(prn_HdDt.Rows(0).Item("Print_Name").ToString) & "****"
            Else
                PrtyNm1 = Trim(prn_HdDt.Rows(0).Item("Ledger_name").ToString) & "****"
            End If



            If CurZ > 0 Then
                If Len(PrtyNm1) > CurZ Then
                    For m = CurZ To 1 Step -1
                        If Mid$(Trim(PrtyNm1), m, 1) = " " Or Mid$(Trim(PrtyNm1), m, 1) = "." Or Mid$(Trim(PrtyNm1), m, 1) = "," Or Mid$(Trim(PrtyNm1), m, 1) = "/" Or Mid$(Trim(PrtyNm1), m, 1) = "-" Or Mid$(Trim(PrtyNm1), m, 1) = "'" Or Mid$(Trim(PrtyNm1), m, 1) = """" Or Mid$(Trim(PrtyNm1), m, 1) = "&" Or Mid$(Trim(PrtyNm1), m, 1) = "(" Or Mid$(Trim(PrtyNm1), m, 1) = ")" Then Exit For
                    Next m
                    If m = 0 Then m = CurZ
                    PrtyNm2 = Microsoft.VisualBasic.Right(Trim(PrtyNm1), Len(PrtyNm1) - m)
                    PrtyNm1 = Microsoft.VisualBasic.Left(Trim(PrtyNm1), m - 1)
                End If
            End If
            Common_Procedures.Print_To_PrintDocument(e, PrtyNm1, LMargin + CurX, TMargin + CurY, 0, CurZ, pFont)

            If Trim(PrtyNm2) <> "" Then
                If Val(prn_CheqDet.Rows(0).Item("Second_PartyName_left").ToString) <> 0 And Val(prn_CheqDet.Rows(0).Item("Second_PartyName_Top").ToString) Then
                    CurX = Val(prn_CheqDet.Rows(0).Item("Second_PartyName_left").ToString) / 2.54 * 100
                    CurY = Val(prn_CheqDet.Rows(0).Item("Second_PartyName_Top").ToString) / 2.54 * 100

                Else
                    CurY = CurY + TxtHgt

                End If
                Common_Procedures.Print_To_PrintDocument(e, PrtyNm2, LMargin + CurX, TMargin + CurY, 0, CurZ, pFont)
            End If


        End If

        CurX = Val(prn_CheqDet.Rows(0).Item("AmountWords_Left").ToString) / 2.54 * 100
        CurY = Val(prn_CheqDet.Rows(0).Item("AmountWords_Top").ToString) / 2.54 * 100
        CurZ = Val(prn_CheqDet.Rows(0).Item("AmountWords_Width").ToString)

        Amt = Common_Procedures.Currency_Format(Math.Abs(Val(prn_HdDt.Rows(0).Item("Cheque_Amount").ToString)))
        'Amt = Microsoft.VisualBasic.Left(Common_Procedures.Currency_Format(Math.Abs(Val(prn_HdDt.Rows(0).Item("Cheque_Amount").ToString))), Len(Trim(Common_Procedures.Currency_Format(Math.Abs(Val(prn_HdDt.Rows(0).Item("Cheque_Amount").ToString))))) - 3) & "/--"
        Rup2 = ""
        Rup1 = Common_Procedures.Rupees_Converstion(Math.Abs(Val(prn_HdDt.Rows(0).Item("Cheque_Amount").ToString)))
        If CurZ > 0 Then
            If Len(Rup1) > CurZ Then
                For m = CurZ To 1 Step -1
                    If Mid$(Trim(Rup1), m, 1) = " " Then Exit For
                Next m
                If m = 0 Then m = CurZ
                Rup2 = Microsoft.VisualBasic.Right(Trim(Rup1), Len(Rup1) - m)
                Rup1 = Microsoft.VisualBasic.Left(Trim(Rup1), m - 1)
            End If
        End If

        Common_Procedures.Print_To_PrintDocument(e, Rup1, LMargin + CurX, TMargin + CurY, 0, 0, pFont)

        If Trim(Rup2) <> "" Then
            If Val(prn_CheqDet.Rows(0).Item("Second_AmountWords_Left").ToString) <> 0 And Val(prn_CheqDet.Rows(0).Item("Second_AmountWords_Top").ToString) Then
                CurX = Val(prn_CheqDet.Rows(0).Item("Second_AmountWords_Left").ToString) / 2.54 * 100
                CurY = Val(prn_CheqDet.Rows(0).Item("Second_AmountWords_Top").ToString) / 2.54 * 100

            Else
                CurY = CurY + TxtHgt

            End If

            Common_Procedures.Print_To_PrintDocument(e, Rup2, LMargin + CurX, TMargin + CurY, 0, 0, pFont)

        End If

        CurX = Val(prn_CheqDet.Rows(0).Item("Rupees_Left").ToString) / 2.54 * 100
        CurY = Val(prn_CheqDet.Rows(0).Item("Rupees_Top").ToString) / 2.54 * 100
        CurZ = Val(prn_CheqDet.Rows(0).Item("Rupees_Width").ToString)
        Common_Procedures.Print_To_PrintDocument(e, "***" & Amt, LMargin + CurX, TMargin + CurY, 0, CurZ, pFont)

        If Val(prn_CheqDet.Rows(0).Item("CompanyName_Left").ToString) <> 0 And Val(prn_CheqDet.Rows(0).Item("CompanyName_Top").ToString) Then
            CurX = Val(prn_CheqDet.Rows(0).Item("CompanyName_Left").ToString) / 2.54 * 100
            CurY = Val(prn_CheqDet.Rows(0).Item("CompanyName_Top").ToString) / 2.54 * 100
            CurZ = Val(prn_CheqDet.Rows(0).Item("CompanyName_Width").ToString)
            If (CurX - LMargin) <> 0 And (CurY - TMargin) <> 0 Then Common_Procedures.Print_To_PrintDocument(e, "For " & Trim(prn_HdDt.Rows(0).Item("Company_Name").ToString), LMargin + CurX, TMargin + CurY, 0, CurZ, pFont)
        End If

        If Val(prn_CheqDet.Rows(0).Item("Partner_Left").ToString) <> 0 And Val(prn_CheqDet.Rows(0).Item("Partner_Top").ToString) Then
            CurX = Val(prn_CheqDet.Rows(0).Item("Partner_Left").ToString) / 2.54 * 100
            CurY = Val(prn_CheqDet.Rows(0).Item("Partner_Top").ToString) / 2.54 * 100
            CurZ = Val(prn_CheqDet.Rows(0).Item("Partner_Width").ToString)
            If (CurX - LMargin) <> 0 And (CurY - TMargin) <> 0 Then Common_Procedures.Print_To_PrintDocument(e, Trim(prn_CheqDet.Rows(0).Item("Partner").ToString), LMargin + CurX, TMargin + CurY, 0, CurZ, pFont)
        End If

        If Val(prn_CheqDet.Rows(0).Item("AccountNo_Left").ToString) <> 0 And Val(prn_CheqDet.Rows(0).Item("AccountNo_Top").ToString) Then
            CurX = Val(prn_CheqDet.Rows(0).Item("AccountNo_Left").ToString) / 2.54 * 100
            CurY = Val(prn_CheqDet.Rows(0).Item("AccountNo_Top").ToString) / 2.54 * 100
            CurZ = Val(prn_CheqDet.Rows(0).Item("AccountNo_Width").ToString)
            If (CurX - LMargin) <> 0 And (CurY - TMargin) <> 0 Then Common_Procedures.Print_To_PrintDocument(e, Trim(prn_CheqDet.Rows(0).Item("Account_No").ToString), LMargin + CurX, TMargin + CurY, 0, CurZ, pFont)
        End If

        e.HasMorePages = False

    End Sub

    Private Sub cbo_Bank_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Bank.LostFocus
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim Bnk_id As Integer = 0
        Dim ChNo As Integer = 0

        Bnk_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Bank.Text)

        If Trim(UCase(cmbnkNm)) <> Trim(UCase(cbo_Bank.Text)) Then
            da = New SqlClient.SqlDataAdapter("select max(ChequeNo) from Cheque_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Bank_IdNo =  " & Val(Bnk_id) & " AND Cheque_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' ", con)
            dt1 = New DataTable
            da.Fill(dt1)

            ChNo = 0
            If dt1.Rows.Count > 0 Then
                If IsDBNull(dt1.Rows(0)(0).ToString) = False Then
                    ChNo = Val(dt1.Rows(0)(0).ToString)
                End If
            End If

            ChNo = ChNo + 1
            txt_ChequeNo.Text = ChNo

        End If

    End Sub

    Private Sub txt_ChequeNo_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_ChequeNo.TextChanged

    End Sub

    Private Sub cbo_ACPayee_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_ACPayee.SelectedIndexChanged

    End Sub

    'Private Sub cbo_AgentName_GotFocus(sender As Object, e As EventArgs) Handles cbo_AgentName.GotFocus

    '    Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'AGENT')", "(Ledger_IdNo = 0)")
    '    cbo_AgentName.Tag = cbo_AgentName.Text
    'End Sub
    'Private Sub cbo_AgentName_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_AgentName.KeyDown
    '    Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_AgentName, cbo_PartyName, cbo_ACPayee, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'AGENT')", "(Ledger_IdNo = 0)")

    '    'If (e.KeyValue = 38 And cbo_AgentName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
    '    '    dgv_Details.Focus()
    '    '    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
    '    'End If
    'End Sub

    'Private Sub cbo_AgentName_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_AgentName.KeyPress
    '    Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_AgentName, cbo_ACPayee, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'AGENT')", "(Ledger_IdNo = 0)")
    'End Sub

    'Private Sub cbo_AgentName_KeyUp(sender As Object, e As KeyEventArgs) Handles cbo_AgentName.KeyUp
    '    If e.Control = False And e.KeyValue = 17 Then
    '        Common_Procedures.MDI_LedType = "AGENT"
    '        Dim f As New Agent_Creation

    '        Common_Procedures.Master_Return.Form_Name = Me.Name
    '        Common_Procedures.Master_Return.Control_Name = cbo_AgentName.Name
    '        Common_Procedures.Master_Return.Return_Value = ""
    '        Common_Procedures.Master_Return.Master_Type = ""

    '        f.MdiParent = MDIParent1
    '        f.Show()

    '    End If
    'End Sub

End Class