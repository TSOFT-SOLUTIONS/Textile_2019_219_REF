Public Class Agent_Creation
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private New_Entry As Boolean = False
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double

    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_DetIndx As Integer
    Private prn_DetSNo As Integer
    Private prn_PageNo As Integer
    Private prn_count As Integer = 0

    Private PrntFormat1_STS As Boolean = False
    Private PrntFormat2_STS As Boolean = False

    Private Sub clear()
        pnl_PrintSetup.Visible = False
        pnl_bank_Details.Visible = False
        lbl_idno.Text = ""
        lbl_idno.ForeColor = Color.Black
        txt_Alaisname.Text = ""
        txt_Name.Text = ""
        cbo_area.Text = ""
        txt_Address1.Text = ""
        txt_address2.Text = ""
        txt_address3.Text = ""
        txt_address4.Text = ""
        txt_phoneno.Text = ""
        txt_emailid.Text = ""
        txt_yarncomm.Text = ""
        txt_TdsPerc.Text = ""
        txt_GSTIN_No.Text = ""
        cbo_State.Text = Common_Procedures.State_IdNoToName(con, 32)
        txt_yarncommbag.Text = ""
        txt_clothcomm.Text = ""
        cbo_group.Text = Common_Procedures.AccountsGroup_IdNoToName(con, 14)
        txt_pan.Text = ""
        cbo_partner.Text = ""
        txt_Cothcommmr.Text = ""
        cbo_open.Text = ""

        txt_Bank_Acc_Name.Text = ""
        txt_bankName.Text = ""
        txt_AccountNo.Text = ""
        txt_Branch.Text = ""
        txt_Ifsc_Code.Text = ""

        txt_TopFromAdds.Text = ""
        txt_TopFromAdds.Enabled = False
        txt_TOPToAdds.Text = ""
        txt_LeftFromAdds.Text = ""
        txt_LeftFromAdds.Enabled = False
        txt_LeftToAdds.Text = ""
        cbo_PaperOrientation.Text = ""
        cbo_FromAddress.Text = Common_Procedures.Company_IdNoToShortName(con, 1)
        chk_FromAddress.Checked = False

        cbo_marketting_Exec_Name.Text = ""

        cbo_partner.Items.Clear()
        cbo_partner.Items.Add("PARTNER")
        cbo_partner.Items.Add("PROPRIETOR")
        Panel_back.Enabled = True
        grp_open.Visible = False
        grp_Filter.Visible = False
        New_Entry = False


    End Sub
    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox

        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Then
            Me.ActiveControl.BackColor = Color.SpringGreen ' Color.MistyRose ' Color.lime
            Me.ActiveControl.ForeColor = Color.Blue
        End If

        If TypeOf Me.ActiveControl Is TextBox Then
            txtbx = Me.ActiveControl
            txtbx.SelectAll()

        ElseIf TypeOf Me.ActiveControl Is ComboBox Then
            combobx = Me.ActiveControl
            combobx.SelectAll()

        End If

        'If Me.ActiveControl.Name <> dgv_Filter.Name Then
        '    Grid_Cell_DeSelect()
        'End If

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
    Public Sub move_record(ByVal idno As Integer)
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim dt1 As New DataTable
        If Val(idno) = 0 Then Exit Sub

        clear()


        da = New SqlClient.SqlDataAdapter("select  a.*, b.AccountsGroup_Name,c.Area_Name , Me.Marketting_Executive_Name , Ch.Company_ShortName from ledger_head a LEFT OUTER JOIN AccountsGroup_Head b ON a.AccountsGroup_IdNo = b.AccountsGroup_IdNo LEFT OUTER JOIN Area_Head c ON a.Area_IdNo=c.Area_IdNo LEFT OUTER JOIN Marketting_Executive_Head Me On Me.Marketting_Executive_IdNo = a.Marketting_Executive_IdNo  Left Outer Join Company_Head Ch ON a.Company_IdNo = Ch.Company_IdNo  where a.ledger_idno = " & Str(Val(idno)) & "   and a.Ledger_Type='AGENT'", con)
        da.Fill(dt)

        If dt.Rows.Count > 0 Then
            lbl_idno.Text = dt.Rows(0).Item("Ledger_IdNo").ToString
            txt_Name.Text = dt.Rows(0).Item("Ledger_MainName").ToString
            txt_alaisname.Text = dt.Rows(0).Item("Ledger_AlaisName").ToString
            cbo_Area.Text = dt.Rows(0)("Area_Name").ToString
            cbo_group.Text = dt.Rows(0)("AccountsGroup_Name").ToString
            txt_Address1.Text = dt.Rows(0)("Ledger_Address1").ToString
            txt_Address2.Text = dt.Rows(0)("Ledger_Address2").ToString
            txt_Address3.Text = dt.Rows(0)("Ledger_Address3").ToString
            txt_Address4.Text = dt.Rows(0)("Ledger_Address4").ToString
            txt_PhoneNo.Text = dt.Rows(0)("Ledger_PhoneNo").ToString
            txt_emailid.Text = dt.Rows(0)("Ledger_Emailid").ToString
            txt_yarncomm.Text = dt.Rows(0)("Yarn_Comm_Percentage").ToString
            txt_TdsPerc.Text = dt.Rows(0)("Tds_Percentage").ToString
            txt_yarncommbag.Text = dt.Rows(0)("Yarn_Comm_Bag").ToString
            txt_clothcomm.Text = dt.Rows(0)("Cloth_Comm_Percentage").ToString
            txt_Cothcommmr.Text = dt.Rows(0)("Cloth_Comm_Meter").ToString
            txt_Pan.Text = dt.Rows(0)("Pan_No").ToString
            cbo_partner.Text = dt.Rows(0)("Partner_Proprietor").ToString
            txt_GSTIN_No.Text = dt.Rows(0)("Ledger_GSTinNo").ToString
            cbo_State.Text = Common_Procedures.State_IdNoToName(con, Val(dt.Rows(0)("Ledger_State_IdNo").ToString))
            cbo_marketting_Exec_Name.Text = dt.Rows(0)("Marketting_Executive_Name").ToString

            txt_Bank_Acc_Name.Text = dt.Rows(0)("Ledger_bank_Ac_Name").ToString
            txt_bankName.Text = dt.Rows(0)("Ledger_BankName").ToString
            txt_AccountNo.Text = dt.Rows(0)("Ledger_AccountNo").ToString
            txt_Branch.Text = dt.Rows(0)("Ledger_BranchName").ToString
            txt_Ifsc_Code.Text = dt.Rows(0)("Ledger_IFSCCode").ToString

            '---------LEDGER ADDRESS PRINT SETUP--------
            txt_TopFromAdds.Text = dt.Rows(0).Item("FROMAddress_Topoint").ToString
            txt_TOPToAdds.Text = dt.Rows(0).Item("TOAddress_Topoint").ToString
            txt_LeftFromAdds.Text = dt.Rows(0).Item("FROMAddress_LeftPoint").ToString
            txt_LeftToAdds.Text = dt.Rows(0).Item("TOAddress_LeftPoint").ToString
            cbo_PaperOrientation.Text = dt.Rows(0).Item("Paper_Orientation").ToString
            cbo_FromAddress.Text = dt.Rows(0).Item("Company_ShortName").ToString
            If Val(dt.Rows(0).Item("FromAddress_SetPosition_Sts").ToString) = 1 Then
                chk_FromAddress.Checked = True
                txt_TopFromAdds.Enabled = True
                txt_LeftFromAdds.Enabled = True
            End If
            '---------LEDGER ADDRESS PRINT SETUP--------

        End If


        dt.Dispose()
        da.Dispose()

        If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()

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

    Private Sub Agent_Creation1_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_area.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "AREA" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
            cbo_area.Text = Trim(Common_Procedures.Master_Return.Return_Value)
        End If

        If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_marketting_Exec_Name.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "MAREXEC" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
            cbo_marketting_Exec_Name.Text = Trim(Common_Procedures.Master_Return.Return_Value)
        End If

    End Sub

    Private Sub Agent_Creation1_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    'Private Sub Agent_Creation1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
    '    If Asc(e.KeyChar) = 27 Then
    '        If grp_Filter.Visible Then
    '            btn_CloseFilter_Click(sender, e)
    '        ElseIf grp_open.Visible Then
    '            btn_find_close_Click(sender, e)
    '        Else
    '            If MessageBox.Show("Do you want to Close?", "FOR CLOSING ENTRY...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) <> Windows.Forms.DialogResult.Yes Then
    '                Exit Sub

    '            Else
    '                Me.Close()

    '            End If

    '        End If

    '    End If
    'End Sub


    Private Sub Agent_Creation1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        con.Open()

        cbo_partner.Items.Clear()
        cbo_partner.Items.Add("")
        cbo_partner.Items.Add("PARTNER")
        cbo_partner.Items.Add("PROPRIETOR")

        grp_open.Visible = False
        grp_open.Left = (Me.Width - grp_open.Width) - 50
        grp_open.Top = (Me.Height - grp_open.Height) - 50

        grp_Filter.Visible = False
        grp_Filter.Left = (Me.Width - grp_Filter.Width) - 30
        grp_Filter.Top = (Me.Height - grp_Filter.Height) - 50

        pnl_PrintSetup.Visible = False
        pnl_PrintSetup.Left = (Me.Width - pnl_PrintSetup.Width) \ 2
        pnl_PrintSetup.Top = (Me.Height - pnl_PrintSetup.Height) \ 2
        pnl_PrintSetup.BringToFront()

        pnl_bank_Details.Visible = False
        pnl_bank_Details.Left = (Me.Width - pnl_bank_Details.Width) \ 2
        pnl_bank_Details.Top = (Me.Height - pnl_bank_Details.Height) \ 2
        pnl_bank_Details.BringToFront()

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1428" Then
            lbl_ExecutingName.Visible = True
            cbo_marketting_Exec_Name.Visible = True
            cbo_State.Width = 185
        Else
            lbl_ExecutingName.Visible = False
            cbo_marketting_Exec_Name.Visible = False

        End If

        If chk_FromAddress.Checked = True Then
            txt_LeftFromAdds.Enabled = True
            txt_TopFromAdds.Enabled = True
        End If

        AddHandler txt_Name.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Alaisname.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_area.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_group.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_partner.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_open.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Address1.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_address2.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_address3.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_address4.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_phoneno.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_emailid.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_pan.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_clothcomm.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Cothcommmr.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_yarncomm.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_yarncommbag.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_TdsPerc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_GSTIN_No.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_State.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_marketting_Exec_Name.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_TopFromAdds.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_TOPToAdds.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_LeftFromAdds.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_LeftToAdds.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PaperOrientation.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_FromAddress.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_Bank_Acc_Name.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_bankName.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Branch.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_AccountNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Ifsc_Code.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_Bank_Acc_Name.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_bankName.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Branch.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_AccountNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Ifsc_Code.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_Name.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Alaisname.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_area.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_group.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_partner.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_open.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Address1.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_address2.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_address3.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_address4.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_phoneno.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_emailid.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_pan.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_clothcomm.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Cothcommmr.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_yarncomm.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_yarncommbag.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_TdsPerc.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_GSTIN_No.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_State.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_marketting_Exec_Name.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_TopFromAdds.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_TOPToAdds.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_LeftFromAdds.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_LeftToAdds.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_PaperOrientation.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_FromAddress.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_Name.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Alaisname.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Address1.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_address2.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_address3.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_address4.KeyDown, AddressOf TextBoxControlKeyDown
        ' AddHandler txt_phoneno.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_pan.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_emailid.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_clothcomm.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Cothcommmr.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_yarncommbag.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_TdsPerc.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_GSTIN_No.KeyDown, AddressOf TextBoxControlKeyDown

        ' AddHandler txt_Name.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Alaisname.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Address1.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_address2.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_address3.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_address4.KeyPress, AddressOf TextBoxControlKeyPress
        ' AddHandler txt_phoneno.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_emailid.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_pan.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_clothcomm.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Cothcommmr.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_yarncommbag.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_TdsPerc.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_GSTIN_No.KeyPress, AddressOf TextBoxControlKeyPress

        new_record()

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()

        Dim cmd As New SqlClient.SqlCommand
        Dim da As SqlClient.SqlDataAdapter
        Dim dt As DataTable

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Agent_Creation, "~L~") = 0 And InStr(Common_Procedures.UR.Agent_Creation, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.Agent_Creation, New_Entry, Me) = False Then Exit Sub




        If MessageBox.Show("Do you want to delete", "FOR DELETION...", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
            Exit Sub
        End If
        If New_Entry = True Then
            MessageBox.Show("This is new entry", "DOES NOT DELETION....", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        Try

            da = New SqlClient.SqlDataAdapter("select count(*) from Voucher_Details where  Ledger_Idno = " & Str(Val(lbl_idno.Text)), con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    If Val(dt.Rows(0)(0).ToString) > 0 Then
                        MessageBox.Show("Already used this Ledger", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If
            End If

            da = New SqlClient.SqlDataAdapter("select count(*) from Stock_Item_Processing_Details where DeliveryTo_StockIdNo = " & Str(Val(lbl_idno.Text)) & " or ReceivedFrom_StockIdNo = " & Str(Val(lbl_idno.Text)) & " or Delivery_PartyIdNo = " & Str(Val(lbl_idno.Text)) & " or Received_PartyIdNo = " & Str(Val(lbl_idno.Text)), con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    If Val(dt.Rows(0)(0).ToString) > 0 Then
                        MessageBox.Show("Already used this Ledger", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If
            End If

            da = New SqlClient.SqlDataAdapter("select count(*) from Stock_Pavu_Processing_Details where DeliveryTo_Idno = " & Str(Val(lbl_idno.Text)) & " or ReceivedFrom_Idno = " & Str(Val(lbl_idno.Text)), con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    If Val(dt.Rows(0)(0).ToString) > 0 Then
                        MessageBox.Show("Already used this Ledger", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If
            End If

            da = New SqlClient.SqlDataAdapter("select count(*) from Stock_SizedPavu_Processing_Details where Ledger_IdNo = " & Str(Val(lbl_idno.Text)) & " or StockAt_IdNo = " & Str(Val(lbl_idno.Text)), con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    If Val(dt.Rows(0)(0).ToString) > 0 Then
                        MessageBox.Show("Already used this Ledger", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If
            End If

            da = New SqlClient.SqlDataAdapter("select count(*) from Stock_Yarn_Processing_Details where DeliveryTo_Idno = " & Str(Val(lbl_idno.Text)) & " or ReceivedFrom_Idno = " & Str(Val(lbl_idno.Text)), con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    If Val(dt.Rows(0)(0).ToString) > 0 Then
                        MessageBox.Show("Already used this Ledger", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If
            End If

            da = New SqlClient.SqlDataAdapter("select count(*) from AgentCommission_Processing_Details where Agent_IdNo = " & Str(Val(lbl_idno.Text)) & " or Ledger_IdNo = " & Str(Val(lbl_idno.Text)), con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    If Val(dt.Rows(0)(0).ToString) > 0 Then
                        MessageBox.Show("Already used this Ledger", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If
            End If

            cmd.Connection = con


            cmd.CommandText = "delete from Ledger_AlaisHead where Ledger_IdNo = " & Str(Val(lbl_idno.Text))
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from ledger_head where ledger_idno = " & Str(Val(lbl_idno.Text))
            cmd.ExecuteNonQuery()

            cmd.Dispose()
            dt.Dispose()
            da.Dispose()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!", "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT DELETE..", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()
            Exit Sub

        End Try
    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        Dim da As New SqlClient.SqlDataAdapter("select ledger_idno, ledger_Name from ledger_head where ledger_Type='AGENT' and ledger_idno <> 0 order by ledger_idno", con)
        Dim dt As New DataTable

        da.Fill(dt)

        dgv_Filter.Columns.Clear()
        dgv_Filter.DataSource = dt
        dgv_Filter.RowHeadersVisible = False

        dgv_Filter.Columns(0).HeaderText = " IDNO"
        dgv_Filter.Columns(1).HeaderText = "AGENT NAME"

        dgv_Filter.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
        dgv_Filter.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

        dgv_Filter.Columns(0).FillWeight = 30
        dgv_Filter.Columns(1).FillWeight = 165

        grp_Filter.Visible = True
        grp_Filter.BringToFront()
        dgv_Filter.Focus()

        Panel_back.Enabled = False

        da.Dispose()
    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim cmd As New SqlClient.SqlCommand
        Dim movid As Integer

        Try
            cmd.Connection = con
            cmd.CommandText = "select min(Ledger_IdNo) from Ledger_Head WHERE Ledger_Type = 'AGENT' and Ledger_IdNo<>0"

            movid = 0
            If IsDBNull(cmd.ExecuteScalar()) = False Then
                movid = cmd.ExecuteScalar
            End If

            cmd.Dispose()

            If movid <> 0 Then Call move_record(movid)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING....", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim cmd As New SqlClient.SqlCommand
        Dim movid As Integer

        Try
            cmd.Connection = con
            cmd.CommandText = "select max(ledger_idno) from ledger_head Where Ledger_Type='AGENT' and ledger_idno<>0"

            movid = 0
            If IsDBNull(cmd.ExecuteScalar()) = False Then
                movid = cmd.ExecuteScalar
            End If

            cmd.Dispose()

            If movid <> 0 Then move_record(movid)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING....", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record

        Dim cmd As New SqlClient.SqlCommand
        Dim movid As Integer = 0

        Try
            cmd.Connection = con
            cmd.CommandText = "select min(ledger_idno ) from ledger_head where Ledger_Type='AGENT'and ledger_idno<>0 and ledger_idno > " & Str((lbl_idno.Text)) & ""
            movid = 0
            If IsDBNull(cmd.ExecuteScalar()) = False Then
                movid = cmd.ExecuteScalar()
            End If

            cmd.Dispose()

            If movid <> 0 Then move_record(movid)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING....", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim cmd As New SqlClient.SqlCommand
        Dim movid As Integer = 0

        Try
            cmd.Connection = con
            cmd.CommandText = "select max(ledger_idno ) from ledger_head where Ledger_Type='AGENT'and ledger_idno<>0 and ledger_idno < " & Str((lbl_idno.Text)) & ""
            movid = 0
            If IsDBNull(cmd.ExecuteScalar()) = False Then
                movid = cmd.ExecuteScalar()
            End If

            cmd.Dispose()

            If movid <> 0 Then move_record(movid)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING....", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record

        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim dt1 As New DataTable
        Dim newid As Integer = 0

        clear()
        lbl_idno.ForeColor = Color.Red
        New_Entry = True

        lbl_idno.Text = Common_Procedures.get_MaxIdNo(con, "ledger_head", "ledger_idno", "")

        If Val(lbl_idno.Text) < 101 Then lbl_idno.Text = 101

        If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer = 0

        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or Ledger_type = 'AGENT') order by Ledger_DisplayName", con)
        da.Fill(dt)
        cbo_open.DataSource = dt
        cbo_open.DisplayMember = "Ledger_DisplayName"

        da.Dispose()

        grp_open.Visible = True
        grp_open.BringToFront()
        If cbo_open.Enabled And cbo_open.Visible Then cbo_open.Focus()
        Panel_back.Enabled = False
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        pnl_PrintSetup.Visible = True
        Panel_back.Enabled = False

        If cbo_FromAddress.Visible And cbo_FromAddress.Focus Then cbo_FromAddress.Focus()
        'Printing_AgentAddress_Print()
    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()

        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim dt As New DataTable
        Dim nr As Long = 0
        Dim acgrp_idno As Integer = 0
        Dim ar_idno As Integer = 0
        Dim Parnt_CD As String = ""
        Dim LedName As String = ""
        Dim SurName As String = ""
        Dim LedArName As String = ""
        Dim LedPhNo As String = ""
        Dim Sno As Integer = 0
        Dim vState_ID As Integer = 0
        Dim MarkExec_Id As Integer = 0
        Dim vCmp_FROMIdNo As String = ""
        Dim FrmAddschk_Sts As Integer = 0

        'If Common_Procedures.UserRight_Check(Common_Procedures.UR.Agent_Creation, New_Entry) = False Then Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Agent_Creation, New_Entry, Me) = False Then Exit Sub



        If Panel_back.Enabled = False Then
            MessageBox.Show("Close other window", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Trim(txt_Name.Text) = "" Then
            MessageBox.Show("Invalid Legder Name", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If txt_Name.Enabled Then txt_Name.Focus()
            Exit Sub
        End If

        acgrp_idno = Common_Procedures.AccountsGroup_NameToIdNo(con, cbo_group.Text)
        If acgrp_idno = 0 Then
            MessageBox.Show("Invalid Accounts Group", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_group.Enabled Then cbo_group.Focus()
            Exit Sub
        End If

        Parnt_CD = Common_Procedures.AccountsGroup_IdNoToCode(con, acgrp_idno)
        ar_idno = Common_Procedures.Area_NameToIdNo(con, cbo_area.Text)

        LedName = Trim(txt_Name.Text)
        If Val(ar_idno) <> 0 Then
            LedName = Trim(txt_Name.Text) & " (" & Trim(cbo_area.Text) & ")"
        End If

        SurName = Common_Procedures.Remove_NonCharacters(LedName)
        If Common_Procedures.Check_Duplicate_LedgerName(con, Val(lbl_idno.Text), SurName) = True Then
            'MessageBox.Show("Duplicate Ledger Name", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If txt_Name.Enabled Then txt_Name.Focus()
            Exit Sub
        End If

        vState_ID = Common_Procedures.State_NameToIdNo(con, cbo_State.Text)
        If vState_ID = 0 Then
            MessageBox.Show("Invalid State", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_State.Enabled Then cbo_State.Focus()
            Exit Sub
        End If

        MarkExec_Id = Common_Procedures.MarketingExecutive_NameToIdNo(con, cbo_marketting_Exec_Name.Text)

        FrmAddschk_Sts = 0
        If chk_FromAddress.Checked = True Then FrmAddschk_Sts = 1

        vCmp_FROMIdNo = Val(Common_Procedures.Company_ShortNameToIdNo(con, cbo_FromAddress.Text))

        trans = con.BeginTransaction

        Try
            cmd.Transaction = trans

            cmd.Connection = con

            If New_Entry = True Then
                lbl_idno.Text = Common_Procedures.get_MaxIdNo(con, "ledger_head", "ledger_idno", "", trans)
                If Val(lbl_idno.Text) < 101 Then lbl_idno.Text = 101
                cmd.CommandText = "Insert into ledger_head(Ledger_IdNo, Ledger_Name, Sur_Name, Ledger_MainName,Ledger_AlaisName,Area_IdNo, AccountsGroup_IdNo, Parent_Code, Ledger_Address1, Ledger_Address2, Ledger_Address3, Ledger_Address4, Ledger_PhoneNo,  Ledger_Type,Ledger_Emailid,Yarn_Comm_Bag,Yarn_Comm_Percentage,Cloth_Comm_Percentage,Cloth_Comm_Meter,Pan_No,Partner_Proprietor , Tds_Percentage,Ledger_GSTinNo,Ledger_State_IdNo ,    Marketting_Executive_IdNo ,           FROMAddress_Topoint    ,         FROMAddress_LeftPoint      ,           TOAddress_Topoint     ,        TOAddress_LeftPoint        ,               Paper_Orientation           , FromAddress_SetPosition_Sts , Company_IdNo, Ledger_bank_Ac_Name , Ledger_BankName , Ledger_AccountNo , Ledger_BranchName, Ledger_IFSCCode )  Values (" & Str(Val(lbl_idno.Text)) & ", '" & Trim(LedName) & "', '" & Trim(SurName) & "', '" & Trim(txt_Name.Text) & "','" & Trim(txt_Alaisname.Text) & "'," & Str(Val(ar_idno)) & ", " & Str(Val(acgrp_idno)) & ", '" & Trim(Parnt_CD) & "', '" & Trim(txt_Address1.Text) & "', '" & Trim(txt_address2.Text) & "', '" & Trim(txt_address3.Text) & "', '" & Trim(txt_address4.Text) & "', '" & Trim(txt_phoneno.Text) & "', 'AGENT','" & Trim(txt_emailid.Text) & "'," & Val(txt_yarncommbag.Text) & "," & Val(txt_yarncomm.Text) & "," & Val(txt_clothcomm.Text) & "," & Val(txt_Cothcommmr.Text) & ",'" & Trim(txt_pan.Text) & "','" & Trim(cbo_partner.Text) & "',  " & Val(txt_TdsPerc.Text) & ",'" & Trim(txt_GSTIN_No.Text) & "', " & Str(vState_ID) & " , " & Str(Val(MarkExec_Id)) & "  ," & Val(txt_TopFromAdds.Text) & " , " & Val(txt_LeftFromAdds.Text) & " , " & Val(txt_TOPToAdds.Text) & " , " & Val(txt_LeftToAdds.Text) & " , '" & Trim(cbo_PaperOrientation.Text) & "' , " & Val(FrmAddschk_Sts) & " , " & Val(vCmp_FROMIdNo) & " , '" & Trim(txt_Bank_Acc_Name.Text) & "', '" & Trim(txt_bankName.Text) & "' , '" & Trim(txt_AccountNo.Text) & "' ,  '" & Trim(txt_Branch.Text) & "',    '" & Trim(txt_Ifsc_Code.Text) & "' )"
                cmd.ExecuteNonQuery()

            Else

                cmd.CommandText = "Update ledger_head set Ledger_Name = '" & Trim(LedName) & "', Sur_Name = '" & Trim(SurName) & "', Ledger_MainName = '" & Trim(txt_Name.Text) & "',Ledger_Alaisname = '" & Trim(txt_Alaisname.Text) & "', Area_IdNo = " & Str(Val(ar_idno)) & ", AccountsGroup_IdNo = " & Str(Val(acgrp_idno)) & ", Parent_Code = '" & Trim(Parnt_CD) & "',  Ledger_Address1 = '" & Trim(txt_Address1.Text) & "', Ledger_Address2 = '" & Trim(txt_address2.Text) & "', Ledger_Address3 = '" & Trim(txt_address3.Text) & "', Ledger_Address4 = '" & Trim(txt_address4.Text) & "', Ledger_PhoneNo = '" & Trim(txt_phoneno.Text) & "',Ledger_Emailid = '" & Trim(txt_emailid.Text) & "',Yarn_Comm_Bag = " & Val(txt_yarncommbag.Text) & ",Yarn_Comm_Percentage = " & Val(txt_yarncomm.Text) & " , Tds_Percentage = " & Val(txt_TdsPerc.Text) & " , Cloth_Comm_Percentage = " & Val(txt_clothcomm.Text) & ",Cloth_Comm_Meter = " & Val(txt_Cothcommmr.Text) & ",Pan_No = '" & Trim(txt_pan.Text) & "', Partner_Proprietor = '" & Trim(cbo_partner.Text) & "', Ledger_GSTinNo = '" & Trim(txt_GSTIN_No.Text) & "', Ledger_State_IdNo = " & Str(vState_ID) & " , Marketting_Executive_IdNo = " & Str(Val(MarkExec_Id)) & "  , FROMAddress_Topoint = " & Val(txt_TopFromAdds.Text) & " , FROMAddress_LeftPoint = " & Val(txt_LeftFromAdds.Text) & " , TOAddress_Topoint = " & Val(txt_TOPToAdds.Text) & " , TOAddress_LeftPoint = " & Val(txt_LeftToAdds.Text) & " , Paper_Orientation = '" & Trim(cbo_PaperOrientation.Text) & "' ,FromAddress_SetPosition_Sts = " & Val(FrmAddschk_Sts) & " , Company_IdNo = " & Val(vCmp_FROMIdNo) & " ,  Ledger_bank_Ac_Name = '" & Trim(txt_Bank_Acc_Name.Text) & "', Ledger_BankName = '" & Trim(txt_bankName.Text) & "', Ledger_AccountNo = '" & Trim(txt_AccountNo.Text) & "',Ledger_BranchName = '" & Trim(txt_Branch.Text) & "', Ledger_IFSCCode = '" & Trim(txt_Ifsc_Code.Text) & "'  Where Ledger_IdNo = " & Str(Val(lbl_idno.Text))
                cmd.ExecuteNonQuery()

            End If



            cmd.CommandText = "delete from Ledger_AlaisHead where Ledger_IdNo = " & Str(Val(lbl_idno.Text))
            cmd.ExecuteNonQuery()

            LedArName = Trim(txt_Name.Text)
            If Val(ar_idno) <> 0 Then
                LedArName = Trim(txt_Name.Text) & " (" & Trim(cbo_area.Text) & ")"
            End If

            cmd.CommandText = "Insert into Ledger_AlaisHead(Ledger_IdNo, Sl_No, Ledger_DisplayName, AccountsGroup_IdNo, Ledger_Type ) Values (" & Str(Val(lbl_idno.Text)) & ", 1, '" & Trim(LedArName) & "', " & Str(Val(acgrp_idno)) & ", 'AGENT')"
            cmd.ExecuteNonQuery()

            If Trim(txt_Alaisname.Text) <> "" Then
                LedArName = Trim(txt_Alaisname.Text)
                If Val(ar_idno) <> 0 Then
                    LedArName = Trim(txt_Alaisname.Text) & " (" & Trim(cbo_area.Text) & ")"
                End If

                cmd.CommandText = "Insert into Ledger_AlaisHead(Ledger_IdNo, Sl_No, Ledger_DisplayName, AccountsGroup_IdNo, Ledger_Type ) Values (" & Str(Val(lbl_idno.Text)) & ", 2, '" & Trim(LedArName) & "', " & Str(Val(acgrp_idno)) & ", 'AGENT')"
                cmd.ExecuteNonQuery()

            End If
            trans.Commit()

            trans.Dispose()
            dt.Dispose()

            Common_Procedures.Master_Return.Return_Value = Trim(txt_Name.Text)
            Common_Procedures.Master_Return.Master_Type = "AGENT"

            If New_Entry = True Then new_record()

            MessageBox.Show("Sucessfully Saved", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)

            'If Val(Common_Procedures.settings.OnSave_MoveTo_NewEntry_Status) = 1 Then
            If New_Entry = True Then
                new_record()
            Else
                move_record(lbl_idno.Text)
            End If
            'Else
            '    move_record(lbl_idno.Text)
            'End If


        Catch ex As Exception
            trans.Rollback()

            If InStr(1, Trim(LCase(ex.Message)), "ix_ledger_head") > 0 Then
                MessageBox.Show("Duplicate Ledger Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            ElseIf InStr(1, Trim(LCase(ex.Message)), "ix_ledger_alaishead") > 0 Then
                MessageBox.Show("Duplicate Ledger Alais Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            Else
                MessageBox.Show(ex.Message, "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
            Exit Sub

        End Try
    End Sub

    Private Sub btn_Save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Save.Click
        save_record()
    End Sub

    Private Sub btn_close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub btn_CloseFilter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_CloseFilter.Click
        Panel_back.Enabled = True
        grp_Filter.Visible = False
    End Sub

    Private Sub btn_find_close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_find_close.Click
        Panel_back.Enabled = True
        grp_open.Visible = False
        If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()
    End Sub

    Private Sub btn_Find_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Find.Click
        Dim cmd As New SqlClient.SqlCommand

        Dim movid As Integer


        movid = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_open.Text)



        If movid <> 0 Then move_record(movid)

        Panel_back.Enabled = True
        grp_open.Visible = False
        grp_Filter.Visible = False
    End Sub

    Private Sub cbo_open_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_open.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'AGENT')", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_open_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_open.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_open, Nothing, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'AGENT')", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_open_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_open.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_open, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'AGENT')", "(Ledger_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            Call btn_Find_Click(sender, e)
        End If
    End Sub
    Private Sub dgv_Filter_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Filter.DoubleClick
        Call btn_OpenFilter_Click(sender, e)
    End Sub


    Private Sub dgv_Filter_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Filter.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        If e.KeyValue = 13 Then
            Call btn_OpenFilter_Click(sender, e)
        End If
    End Sub

    Private Sub txt_Name_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Name.KeyPress
        If Asc(e.KeyChar) = 34 Or Asc(e.KeyChar) = 39 Then   '-- Single Quotes and double quotes blocked
            e.Handled = True
        End If
        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub cbo_group_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_group.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "AccountsGroup_Head", "AccountsGroup_Name", "", "")

    End Sub


    Private Sub cbo_Group_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_group.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_group, cbo_area, txt_Address1, "AccountsGroup_Head", "AccountsGroup_Name", "", "")


    End Sub

    Private Sub cbo_group_Keypress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_group.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_group, txt_Address1, "AccountsGroup_Head", "AccountsGroup_Name", "", "")

    End Sub

    Private Sub txt_clothcomm_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_clothcomm.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

    End Sub



    Private Sub txt_clothcommmtr_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Cothcommmr.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

    End Sub

    Private Sub txt_yarncomm_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_yarncomm.KeyDown
        If e.KeyValue = 40 Then
            SendKeys.Send("{TAB}")
        End If
        If e.KeyValue = 38 Then
            SendKeys.Send("+{TAB}")
        End If
    End Sub

    Private Sub txt_yarncomm_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_yarncomm.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then



            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()
            End If

        End If
    End Sub


    Private Sub txt_yarncommbag_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_yarncommbag.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

    End Sub

    Private Sub cbo_partner_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_partner.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")

    End Sub

    Private Sub cbo_partner_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_partner.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_partner, txt_pan, txt_TdsPerc, "", "", "", "")

    End Sub

    Private Sub cbo_partner_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_partner.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_partner, txt_TdsPerc, "", "", "", "")

    End Sub

    Private Sub cbo_area_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_area.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Area_Head", "Area_Name", "", "")

    End Sub


    Private Sub cbo_Area_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_area.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_area, txt_Alaisname, cbo_group, "Area_Head", "Area_Name", "", "")
    End Sub

    Private Sub cbo_Area_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_area.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_area, cbo_group, "Area_Head", "Area_Name", "", "")

    End Sub

    Private Sub cbo_Area_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_area.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Area_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_area.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub btn_OpenFilter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_OpenFilter.Click


        Dim movid As Integer

        movid = 0
        If IsDBNull((dgv_Filter.CurrentRow.Cells(0).Value)) = False Then
            movid = Val(dgv_Filter.CurrentRow.Cells(0).Value)
        End If

        If Val(movid) <> 0 Then
            move_record(movid)
            btn_CloseFilter_Click(sender, e)

        End If
    End Sub


    Private Sub Agent_Creation_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        If e.KeyCode = Keys.OemQuotes Then
            e.SuppressKeyPress = True
            e.Handled = True
        End If
    End Sub

    Private Sub Agent_Creation_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = 27 Then
            If grp_Filter.Visible Then
                btn_CloseFilter_Click(sender, e)
            ElseIf grp_open.Visible Then
                btn_find_close_Click(sender, e)
            Else
                If MessageBox.Show("Do you want to Close?", "FOR CLOSING ENTRY...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) <> Windows.Forms.DialogResult.Yes Then
                    Exit Sub

                Else
                    Me.Close()

                End If

            End If

        End If
    End Sub

    Private Sub Agent_Creation_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        Try

            If Asc(e.KeyChar) = 27 Then

                If grp_Filter.Visible = True Then
                    btn_CloseFilter_Click(sender, e)
                    Exit Sub

                ElseIf grp_open.Visible = True Then
                    btn_find_close_Click(sender, e)
                    Exit Sub

                ElseIf pnl_PrintSetup.Visible = True Then
                    btn_PrintClose_Click(sender, e)
                    Exit Sub

                ElseIf pnl_bank_Details.Visible = True Then
                    e.Handled = True
                    btn_close_bankdetails_Click(sender, e)
                    Exit Sub

                Else
                    If MessageBox.Show("Do you want to Close?", "FOR CLOSING ENTRY...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) <> Windows.Forms.DialogResult.Yes Then
                        Exit Sub

                    Else
                        Me.Close()

                    End If

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_State_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_State.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "State_Head", "State_Name", "", "(State_Idno = 0)")
    End Sub

    Private Sub cbo_State_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_State.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_State, txt_address4, Nothing, "State_Head", "State_Name", "", "(State_Idno = 0)")
        If e.KeyValue = 40 Then
            If cbo_marketting_Exec_Name.Visible Then
                cbo_marketting_Exec_Name.Focus()
            Else
                txt_phoneno.Focus()
            End If
        End If

    End Sub

    Private Sub cbo_State_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_State.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_State, Nothing, "State_Head", "State_Name", "", "(State_Idno = 0)")

        If Asc(e.KeyChar) = 13 Then
            If cbo_marketting_Exec_Name.Visible Then
                cbo_marketting_Exec_Name.Focus()
            Else
                txt_phoneno.Focus()
            End If
        End If


    End Sub

    Private Sub cbo_marketting_Exec_Name_GotFocus(sender As Object, e As EventArgs) Handles cbo_marketting_Exec_Name.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Marketting_Executive_Head", "Marketting_Executive_Name", "", "(Marketting_Executive_IdNo = 0)")
    End Sub

    Private Sub cbo_marketting_Exec_Name_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_marketting_Exec_Name.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_marketting_Exec_Name, cbo_State, txt_phoneno, "Marketting_Executive_Head", "Marketting_Executive_Name", "", "(Marketting_Executive_IdNo = 0)")
    End Sub

    Private Sub cbo_marketting_Exec_Name_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_marketting_Exec_Name.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_marketting_Exec_Name, txt_phoneno, "Marketting_Executive_Head", "Marketting_Executive_Name", "", "(Marketting_Executive_IdNo = 0)")
    End Sub

    Private Sub cbo_marketting_Exec_Name_KeyUp(sender As Object, e As KeyEventArgs) Handles cbo_marketting_Exec_Name.KeyUp
        'If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

        '    Dim f As New Marketting_Executive_Creation

        '    Common_Procedures.Master_Return.Form_Name = Me.Name
        '    Common_Procedures.Master_Return.Control_Name = cbo_marketting_Exec_Name.Name
        '    Common_Procedures.Master_Return.Return_Value = ""
        '    Common_Procedures.Master_Return.Master_Type = ""

        '    f.MdiParent = MDIParent1
        '    f.Show()

        'End If
    End Sub

    Private Sub txt_phoneno_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_phoneno.KeyDown
        If e.KeyValue = 38 Then
            If cbo_marketting_Exec_Name.Visible Then
                cbo_marketting_Exec_Name.Focus()
            Else
                cbo_State.Focus()
            End If
        End If
        If e.KeyValue = 40 Then
            txt_emailid.Focus()
        End If
    End Sub

    Private Sub txt_phoneno_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_phoneno.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_emailid.Focus()
        End If
    End Sub

    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        print_record()
    End Sub

    Private Sub btn_PrintClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_PrintClose.Click
        Dim cmd As New SqlClient.SqlCommand
        Dim vCmp_FROMIdNo As String = 0
        Dim FrmAddschk_Sts As Integer = 0

        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()

        vCmp_FROMIdNo = Val(Common_Procedures.Company_ShortNameToIdNo(con, cbo_FromAddress.Text))
        FrmAddschk_Sts = 0
        If chk_FromAddress.Checked = True Then FrmAddschk_Sts = 1

        cmd.Connection = con

        cmd.CommandText = "Update ledger_head set FROMAddress_Topoint = " & Val(txt_TopFromAdds.Text) & " , FROMAddress_LeftPoint = " & Val(txt_LeftFromAdds.Text) & " , TOAddress_Topoint = " & Val(txt_TOPToAdds.Text) & " , TOAddress_LeftPoint = " & Val(txt_LeftToAdds.Text) & " , Paper_Orientation = '" & Trim(cbo_PaperOrientation.Text) & "' , FromAddress_SetPosition_Sts = " & Val(FrmAddschk_Sts) & " , Company_IdNo = " & Val(vCmp_FROMIdNo) & " Where Ledger_IdNo = " & Str(Val(lbl_idno.Text))
        cmd.ExecuteNonQuery()

        pnl_PrintSetup.Visible = False
        Panel_back.Enabled = True
        If txt_Name.Visible And txt_Name.Enabled Then txt_Name.Focus()

    End Sub

    Private Sub cbo_PaperOrientation_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PaperOrientation.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")
    End Sub

    Private Sub cbo_PaperOrientation_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PaperOrientation.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PaperOrientation, txt_LeftToAdds, chk_FromAddress, "", "", "", "")
    End Sub

    Private Sub cbo_PaperOrientation_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PaperOrientation.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PaperOrientation, chk_FromAddress, "", "", "", "")
    End Sub

    Private Sub chk_FromAddress_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles chk_FromAddress.Click
        If chk_FromAddress.Checked = True Then
            txt_LeftFromAdds.Enabled = True
            txt_TopFromAdds.Enabled = True
        Else
            txt_LeftFromAdds.Enabled = False
            txt_TopFromAdds.Enabled = False
        End If
    End Sub

    Private Sub txt_LeftFromAdds_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_LeftFromAdds.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_LeftToAdds_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_LeftToAdds.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_TopFromAdds_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_TopFromAdds.KeyDown
        If e.KeyCode = 38 Then e.Handled = True : SendKeys.Send("+{TAB}")
        If e.KeyCode = 40 Then e.Handled = True : btn_PrintClose_Click(sender, e)
    End Sub

    Private Sub txt_TopFromAdds_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_TopFromAdds.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then btn_PrintClose_Click(sender, e)
    End Sub

    Private Sub txt_TOPToAdds_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_TOPToAdds.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub btn_Print_Address_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_Address.Click
        PrntFormat1_STS = True
        Printing_LedgerAddress_Print()
    End Sub

    Private Sub btn_Print_Address_2_Click(sender As Object, e As EventArgs) Handles btn_Print_Address_2.Click
        PrntFormat2_STS = True
        Printing_LedgerAddress_Print()
    End Sub


    Private Sub btn_Cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Cancel.Click
        Call btn_PrintClose_Click(sender, e)
    End Sub

    Private Sub cbo_FromAddress_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_FromAddress.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Company_Head", "Company_ShortName", "(Close_Status=0)", "(Company_IdNo = 0)")
    End Sub

    Private Sub cbo_FromAddress_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_FromAddress.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_FromAddress, Nothing, cbo_PaperOrientation, "Company_Head", "Company_ShortName", "(Close_Status=0)", "(Company_IdNo = 0)")
    End Sub

    Private Sub cbo_FromAddress_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_FromAddress.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_FromAddress, cbo_PaperOrientation, "Company_Head", "Company_ShortName", "(Close_Status=0)", "(Company_IdNo = 0)")
    End Sub

    Public Sub Printing_LedgerAddress_Print()
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim ps As Printing.PaperSize

        Try

            da1 = New SqlClient.SqlDataAdapter("select * from Ledger_Head Where Ledger_IdNo <> 0 and Ledger_IdNo = " & Str(Val(lbl_idno.Text)), con)
            dt1 = New DataTable
            da1.Fill(dt1)
            If dt1.Rows.Count <= 0 Then
                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub
            End If
            dt1.Dispose()
            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try


        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If Trim(cbo_PaperOrientation.Text) = "LANDSCAPE" Then
                PrintDocument1.DefaultPageSettings.Landscape = True
                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                    PrintDocument1.DefaultPageSettings.PaperSize = ps
                    'e.PageSettings.PaperSize = ps
                    Exit For
                End If
            Else
                PrintDocument1.DefaultPageSettings.Landscape = False
                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                    PrintDocument1.DefaultPageSettings.PaperSize = ps
                    'e.PageSettings.PaperSize = ps
                    Exit For
                End If
            End If
        Next

        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then

            Try
                If Val(Common_Procedures.settings.Printing_Show_PrintDialogue) = 1 Then
                    PrintDialog1.PrinterSettings = PrintDocument1.PrinterSettings
                    If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                        PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings
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
                ppd.ClientSize = New Size(900, 800)

                ppd.ShowDialog()

            Catch ex As Exception
                MsgBox("The printing operation failed" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "DOES NOT SHOW PRINT PREVIEW...")

            End Try

        End If

    End Sub

    Private Sub PrintDocument1_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument1.BeginPrint
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim Cmd As New SqlClient.SqlDataAdapter
        Dim vCmp_FROMIdNo As String = 0


        prn_HdDt.Clear()
        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0
        prn_count = 0


        Try

            vCmp_FROMIdNo = Val(Common_Procedures.Company_ShortNameToIdNo(con, cbo_FromAddress.Text))

            da1 = New SqlClient.SqlDataAdapter("Select  a.*, d.State_Name as ledger_statename, Ch.*, cSt.State_name as company_statename from Ledger_Head a LEFT OUTER JOIN State_Head d ON a.Ledger_State_IdNo = d.State_IdNo LEFT OUTER JOIN Company_Head Ch ON Ch.Company_IdNo = " & Str(Val(vCmp_FROMIdNo)) & " LEFT OUTER JOIN State_Head cSt ON cSt.State_IdNo = Ch.Company_State_IdNo Where a.Ledger_IdNo <> 0 and a.Ledger_IdNo = " & Str(Val(lbl_idno.Text)), con)
            'da1 = New SqlClient.SqlDataAdapter("Select  a.*, a.Weaver_LoomType, b.Area_Name , c.AccountsGroup_Name as Ac_Group_Name, d.State_Name , Ch.* , d.State_Name from Ledger_Head a INNER JOIN Area_Head b ON a.Area_IdNo = b.Area_IdNo LEFT JOIN AccountsGroup_Head c ON a.AccountsGroup_IdNo = c.AccountsGroup_IdNo LEFT JOIN  State_Head d ON a.Ledger_State_IdNo = d.State_IdNo INNER JOIN Company_Head Ch ON a.Company_IdNo = Ch.Company_IdNo LEFT JOIN Company_Head St ON d.State_IdNo = Ch.Company_State_IdNo where a.Ledger_IdNo <> 0 and a.Ledger_IdNo = " & Str(Val(lbl_idno.Text)), con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)
            If prn_HdDt.Rows.Count < 0 Then
                MessageBox.Show("This is New Entry", "FOR PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            End If
            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub PrintDocument1_PrintPage(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        If prn_HdDt.Rows.Count <= 0 Then Exit Sub
        Printing_Format1(e)
    End Sub

    Private Sub Printing_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim pFont As Font
        Dim p1Font As Font
        Dim ps As Printing.PaperSize
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim I As Integer
        Dim TxtHgt As Single
        Dim CurY As Single = 0
        Dim ItmNm1 As String = "", ItmNm2 As String = ""
        Dim S1 As Single = 0
        Dim S2 As Single = 0
        Dim S3 As Single = 0
        Dim PhNo1 As String = ""
        Dim PhNo2 As String = ""
        Dim PhNo3 As String = ""
        Dim vLftMrgn_INCM As Single = 0
        Dim vTpMrgn_INCM As Single = 0
        Dim vLftMrgn_INPixel As Single = 0
        Dim vTpMrgn_INPixel As Single = 0
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_PanNo As String, Cmp_GSTIN_No As String
        Dim Cmp_StateNm As String, Cmp_Mail As String

        vLftMrgn_INCM = 0
        vTpMrgn_INCM = 0

        If Trim(cbo_PaperOrientation.Text) = "LANDSCAPE" Then
            PrintDocument1.DefaultPageSettings.Landscape = True

            vLftMrgn_INCM = Val(txt_LeftToAdds.Text)
            vTpMrgn_INCM = Val(txt_TOPToAdds.Text)
            If vLftMrgn_INCM = 0 Then vLftMrgn_INCM = 19 '-----in Cm
            If vTpMrgn_INCM = 0 Then vTpMrgn_INCM = 6 '----in Cm

            vLftMrgn_INPixel = Val(vLftMrgn_INCM) / 2.54 * 100
            vTpMrgn_INPixel = Val(vTpMrgn_INCM) / 2.54 * 100

        Else
            PrintDocument1.DefaultPageSettings.Landscape = False

            vLftMrgn_INCM = Val(txt_LeftToAdds.Text)
            vTpMrgn_INCM = Val(txt_TOPToAdds.Text)

            If vLftMrgn_INCM = 0 Then vLftMrgn_INCM = 11 '-----5 inch
            If vTpMrgn_INCM = 0 Then vTpMrgn_INCM = 0.5 '----1 Inch

            vLftMrgn_INPixel = Val(vLftMrgn_INCM) / 2.54 * 100
            vTpMrgn_INPixel = Val(vTpMrgn_INCM) / 2.54 * 100

        End If

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                e.PageSettings.PaperSize = ps
                Exit For
            End If
        Next

        If Trim(cbo_PaperOrientation.Text) = "LANDSCAPE" Then
            With PrintDocument1.DefaultPageSettings.Margins
                .Left = vLftMrgn_INPixel
                .Right = 0
                .Top = vTpMrgn_INPixel
                .Bottom = 0
                LMargin = .Left
                RMargin = .Right
                TMargin = .Top
                BMargin = .Bottom
            End With
        Else
            With PrintDocument1.DefaultPageSettings.Margins
                .Left = vLftMrgn_INPixel
                .Right = 0
                .Top = vTpMrgn_INPixel
                .Bottom = 0
                LMargin = .Left
                RMargin = .Right
                TMargin = .Top
                BMargin = .Bottom
            End With
        End If


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

        TxtHgt = 20

        CurY = TMargin

        Try

            If prn_HdDt.Rows.Count > 0 Then

                S1 = e.Graphics.MeasureString("TO    :  ", pFont).Width

                CurY = CurY + TxtHgt - 10
                p1Font = New Font("Calibri", 12, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "TO  :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_MainName").ToString, LMargin + 10, CurY, 0, 0, p1Font)
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
                'If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
                '    CurY = CurY + TxtHgt
                '    Common_Procedures.Print_To_PrintDocument(e, " GSTIN No : " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
                'End If
                'If Trim(prn_HdDt.Rows(0).Item("Ledger_Mail").ToString) <> "" Then
                '    CurY = CurY + TxtHgt
                '    Common_Procedures.Print_To_PrintDocument(e, " Email-Id : " & prn_HdDt.Rows(0).Item("Ledger_Mail").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
                'End If
                PhNo1 = ""
                If prn_HdDt.Rows(0).Item("Ledger_PhoneNo").ToString <> "" Then PhNo1 = "PHONE : " & prn_HdDt.Rows(0).Item("Ledger_PhoneNo").ToString

                PhNo2 = ""
                S2 = 0
                If prn_HdDt.Rows(0).Item("MobileNo_Frsms").ToString <> "" Then
                    If Trim(PhNo1) = "" Then
                        PhNo2 = "PHONE : " & prn_HdDt.Rows(0).Item("MobileNo_Frsms").ToString
                    Else
                        PhNo2 = prn_HdDt.Rows(0).Item("MobileNo_Frsms").ToString
                        S2 = e.Graphics.MeasureString("PHONE : ", pFont).Width
                    End If
                End If

                PhNo3 = ""
                S3 = 0
                If prn_HdDt.Rows(0).Item("Ledger_MobileNo").ToString <> "" Then
                    If Trim(PhNo1) = "" And Trim(PhNo2) = "" Then
                        PhNo3 = "PHONE : " & prn_HdDt.Rows(0).Item("Ledger_MobileNo").ToString
                    Else
                        PhNo3 = prn_HdDt.Rows(0).Item("Ledger_MobileNo").ToString
                        S3 = e.Graphics.MeasureString("PHONE : ", pFont).Width
                    End If

                End If

                If Trim(PhNo1) <> "" Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, " " & Trim(PhNo1), LMargin + S1 + 10, CurY, 0, 0, pFont)
                End If
                If Trim(PhNo2) <> "" Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, " " & Trim(PhNo2), LMargin + S1 + S2 + 10, CurY, 0, 0, pFont)
                End If
                If Trim(PhNo3) <> "" Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, " " & Trim(PhNo3), LMargin + S1 + S3 + 10, CurY, 0, 0, pFont)
                End If


                '----- FROM ADDRESS POSITION -----

                vLftMrgn_INCM = 0
                vTpMrgn_INCM = 0

                If Trim(cbo_PaperOrientation.Text) = "LANDSCAPE" Then
                    PrintDocument1.DefaultPageSettings.Landscape = True

                    If chk_FromAddress.Checked = True And Val(txt_LeftFromAdds.Text) <> 0 And Val(txt_TopFromAdds.Text) <> 0 Then
                        vLftMrgn_INCM = Val(txt_LeftFromAdds.Text)
                        vTpMrgn_INCM = Val(txt_TopFromAdds.Text)
                    Else
                        vLftMrgn_INCM = 10.5
                        vTpMrgn_INCM = 9
                    End If

                    vLftMrgn_INPixel = Val(vLftMrgn_INCM) / 2.54 * 100
                    vTpMrgn_INPixel = Val(vTpMrgn_INCM) / 2.54 * 100

                Else

                    PrintDocument1.DefaultPageSettings.Landscape = False

                    vLftMrgn_INCM = Val(txt_LeftFromAdds.Text)
                    vTpMrgn_INCM = Val(txt_TopFromAdds.Text)

                    If chk_FromAddress.Checked = True And Val(txt_LeftFromAdds.Text) <> 0 And Val(txt_TopFromAdds.Text) <> 0 Then
                        vLftMrgn_INCM = Val(txt_LeftFromAdds.Text)
                        vTpMrgn_INCM = Val(txt_TopFromAdds.Text)
                    Else
                        vLftMrgn_INCM = 1.5
                        vTpMrgn_INCM = 2.8
                    End If

                    vLftMrgn_INPixel = Val(vLftMrgn_INCM) / 2.54 * 100
                    vTpMrgn_INPixel = Val(vTpMrgn_INCM) / 2.54 * 100

                End If


                For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                        ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                        PrintDocument1.DefaultPageSettings.PaperSize = ps
                        e.PageSettings.PaperSize = ps
                        Exit For
                    End If
                Next

                With PrintDocument1.DefaultPageSettings.Margins
                    .Left = vLftMrgn_INPixel
                    .Right = 0
                    .Top = vTpMrgn_INPixel
                    .Bottom = 0
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

                Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
                Cmp_StateNm = "" : Cmp_GSTIN_No = "" : Cmp_PanNo = "" : Cmp_PhNo = "" : Cmp_Mail = ""

                TxtHgt = 20

                CurY = TMargin

                S1 = e.Graphics.MeasureString("FROM  :  ", pFont).Width

                Cmp_Name = Trim(prn_HdDt.Rows(0).Item("Company_Name").ToString)

                CurY = CurY + TxtHgt - 10
                p1Font = New Font("Calibri", 12, FontStyle.Bold)
                If Trim(prn_HdDt.Rows(0).Item("Company_Name").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, "FROM  :  " & prn_HdDt.Rows(0).Item("Company_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
                End If
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Company_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Company_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
                'CurY = CurY + TxtHgt
                'Common_Procedures.Print_To_PrintDocument(e, "STATE   : " & prn_HdDt.Rows(0).Item("State_Name").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)


                PhNo1 = ""
                If prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString <> "" Then PhNo1 = "Phone : " & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
                Cmp_Mail = ""
                If prn_HdDt.Rows(0).Item("Company_EMail").ToString <> "" Then Cmp_Mail = "Email : " & prn_HdDt.Rows(0).Item("Company_EMail").ToString

                If Trim(PhNo1) <> "" Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, " " & Trim(PhNo1), LMargin + S1 + 10, CurY, 0, 0, pFont)
                End If
                If Trim(Cmp_Mail) <> "" Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, " " & Trim(Cmp_Mail), LMargin + S1 + 10, CurY, 0, 0, pFont)
                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub btn_bank_Details_Click(sender As Object, e As EventArgs) Handles btn_bank_Details.Click
        Panel_back.Enabled = False
        pnl_bank_Details.Visible = True
        pnl_bank_Details.BringToFront()
        txt_Bank_Acc_Name.Focus()
    End Sub

    Private Sub btn_close_bankdetails_Click(sender As Object, e As EventArgs) Handles btn_close_bankdetails.Click
        Panel_back.Enabled = True
        pnl_bank_Details.Visible = False
    End Sub

    Private Sub txt_Bank_Acc_Name_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_Bank_Acc_Name.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_bankName.Focus()
        End If
    End Sub

    Private Sub txt_Bank_Acc_Name_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Bank_Acc_Name.KeyDown
        If e.KeyCode = 40 Then
            txt_bankName.Focus()
        End If
    End Sub

    Private Sub txt_bankName_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_bankName.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_AccountNo.Focus()
        End If
    End Sub

    Private Sub txt_bankName_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_bankName.KeyDown
        If e.KeyCode = 40 Then
            txt_AccountNo.Focus()
        End If

        If e.KeyCode = 38 Then
            txt_Bank_Acc_Name.Focus()
        End If
    End Sub

    Private Sub txt_AccountNo_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_AccountNo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_Branch.Focus()
        End If
    End Sub

    Private Sub txt_AccountNo_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_AccountNo.KeyDown
        If e.KeyCode = 40 Then
            txt_Branch.Focus()
        End If

        If e.KeyCode = 38 Then
            txt_bankName.Focus()
        End If
    End Sub

    Private Sub txt_Branch_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_Branch.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_Ifsc_Code.Focus()
        End If
    End Sub

    Private Sub txt_Branch_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Branch.KeyDown
        If e.KeyCode = 40 Then
            txt_Ifsc_Code.Focus()
        End If
        If e.KeyCode = 38 Then
            txt_AccountNo.Focus()
        End If
    End Sub

    Private Sub txt_Ifsc_Code_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_Ifsc_Code.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_Bank_Acc_Name.Focus()
        End If
    End Sub

    Private Sub txt_Ifsc_Code_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Ifsc_Code.KeyDown
        If e.KeyCode = 40 Then
            txt_Bank_Acc_Name.Focus()
        End If
        If e.KeyCode = 38 Then
            txt_Branch.Focus()
        End If
    End Sub

End Class