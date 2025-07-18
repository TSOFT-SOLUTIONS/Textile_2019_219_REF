Public Class JobWork_Production_Entry
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "JPROD-"
    Private prn_HdDt As New DataTable
    Private prn_PageNo As Integer
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Public vmskOldText As String = ""
    Public vmskSelStrt As Integer = -1

    Private Sub clear()
        New_Entry = False
        Insert_Entry = False
        pnl_back.Enabled = True
        pnl_filter.Visible = False
        vmskOldText = ""
        vmskSelStrt = -1
        lbl_RollNo.Text = ""
        lbl_RollNo.ForeColor = Color.Black
        msk_Date.Text = ""
        dtp_date.Text = ""
        cbo_PartyName.Text = ""
        cbo_PartyName.Tag = ""
        cbo_ClothName.Text = ""
        cbo_ClothName.Tag = ""
        cbo_EndsCount.Text = ""
        cbo_WidthType.Text = ""




        lbl_WeftCount.Text = ""
        txt_Meters.Text = ""
        lbl_ConsumedYarn.Text = ""
        cbo_LoomNo.Text = ""
        cbo_LoomNo.Tag = ""
        txt_BeamNo1.Text = ""
        txt_BeamNo2.Text = ""
        txt_folding.Text = 100
        cbo_PartyName.Enabled = True
        cbo_PartyName.BackColor = Color.White

        cbo_ClothName.Enabled = True
        cbo_ClothName.BackColor = Color.White

        cbo_EndsCount.Enabled = True
        cbo_EndsCount.BackColor = Color.White

        txt_Meters.Enabled = True
        txt_Meters.BackColor = Color.White
        txt_weight.Text = ""

        txt_weight.Enabled = True
        txt_weight.BackColor = Color.White



    End Sub
    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox
        Dim msktxbx As MaskedTextBox
        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Me.ActiveControl Is Button Or TypeOf Me.ActiveControl Is MaskedTextBox Then
            Me.ActiveControl.BackColor = Color.Lime
            Me.ActiveControl.ForeColor = Color.Blue
        End If

        If TypeOf Me.ActiveControl Is TextBox Then
            txtbx = Me.ActiveControl
            txtbx.SelectAll()
        ElseIf TypeOf Me.ActiveControl Is MaskedTextBox Then
            msktxbx = Me.ActiveControl
            msktxbx.SelectionStart = 0
        ElseIf TypeOf Me.ActiveControl Is ComboBox Then
            combobx = Me.ActiveControl
            combobx.SelectAll()
        End If

        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)

        On Error Resume Next

        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Or TypeOf Prec_ActCtrl Is MaskedTextBox Then
                Prec_ActCtrl.BackColor = Color.White
                Prec_ActCtrl.ForeColor = Color.Black
            ElseIf TypeOf Prec_ActCtrl Is Button Then
                Prec_ActCtrl.BackColor = Color.FromArgb(41, 57, 85)
                Prec_ActCtrl.ForeColor = Color.White
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
        Dim dt1 As New DataTable
        Dim NewCode As String
        Dim LockSTS As Boolean = False

        If Val(no) = 0 Then Exit Sub

        clear()

        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name, c.Cloth_Name, d.EndsCount_Name, e.Count_Name, f.Loom_Name from JobWork_Production_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo INNER JOIN Cloth_Head c ON a.Cloth_IdNo = c.Cloth_IdNo LEFT OUTER JOIN EndsCount_Head d ON a.EndsCount_IdNo = d.EndsCount_IdNo LEFT OUTER JOIN Count_Head e ON a.Count_IdNo = e.Count_IdNo LEFT OUTER JOIN Loom_Head f ON a.Loom_IdNo = f.Loom_IdNo Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.JobWork_Production_Code = '" & Trim(NewCode) & "'", con)
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then
                lbl_RollNo.Text = dt1.Rows(0).Item("JobWork_Production_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("JobWork_Production_Date").ToString
                msk_Date.Text = dtp_Date.Text
                cbo_PartyName.Text = dt1.Rows(0).Item("Ledger_Name").ToString
                cbo_ClothName.Text = dt1.Rows(0).Item("Cloth_Name").ToString
                cbo_ClothName.Tag = cbo_ClothName.Text
                cbo_EndsCount.Text = dt1.Rows(0).Item("EndsCount_Name").ToString
                lbl_WeftCount.Text = dt1.Rows(0).Item("Count_Name").ToString
                txt_Meters.Text = dt1.Rows(0).Item("Receipt_Meters").ToString
                lbl_ConsumedYarn.Text = dt1.Rows(0).Item("Rough_Consumed_Yarn").ToString
                cbo_LoomNo.Text = dt1.Rows(0).Item("Loom_Name").ToString
                txt_BeamNo1.Text = dt1.Rows(0).Item("Beam_No1").ToString
                txt_BeamNo2.Text = dt1.Rows(0).Item("Beam_No2").ToString
                cbo_WidthType.Text = dt1.Rows(0).Item("Width_Type").ToString
                txt_weight.Text = dt1.Rows(0).Item("Weight").ToString
                txt_folding.Text = dt1.Rows(0).Item("Folding_Percentage").ToString
                LockSTS = False
                If IsDBNull(dt1.Rows(0).Item("JobWork_Delivery_Code").ToString) = False Then
                    If Trim(dt1.Rows(0).Item("JobWork_Delivery_Code").ToString) <> "" Then
                        LockSTS = True
                    End If
                End If

            End If

            dt1.Dispose()
            da1.Dispose()

            If LockSTS = True Then
                cbo_PartyName.Enabled = False
                cbo_PartyName.BackColor = Color.LightGray

                cbo_ClothName.Enabled = False
                cbo_ClothName.BackColor = Color.LightGray

                cbo_EndsCount.Enabled = False
                cbo_EndsCount.BackColor = Color.LightGray

                txt_Meters.Enabled = False
                txt_Meters.BackColor = Color.LightGray

                txt_weight.Enabled = False
                txt_weight.BackColor = Color.LightGray
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        If msk_Date.Visible And msk_Date.Enabled Then msk_Date.Focus()

    End Sub

    Private Sub Empty_BeamBagCone_Delivery_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Activated

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_PartyName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_PartyName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_ClothName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CLOTH" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_ClothName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_EndsCount.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "ENDSCOUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_EndsCount.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_LoomNo.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LOOMNO" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_LoomNo.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

    Private Sub JobWork_Production_Entry_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable

        Me.Text = ""

        con.Open()

        Da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or Ledger_Type = 'JOBWORKER') order by Ledger_DisplayName", con)
        Da.Fill(Dt1)
        cbo_PartyName.DataSource = Dt1
        cbo_PartyName.DisplayMember = "Ledger_DisplayName"

        Da = New SqlClient.SqlDataAdapter("select Loom_Name from Loom_head order by Loom_Name", con)
        Da.Fill(Dt2)
        cbo_LoomNo.DataSource = Dt2
        cbo_LoomNo.DisplayMember = "Loom_Name"

        Da = New SqlClient.SqlDataAdapter("select Cloth_Name from Cloth_Head order by Cloth_Name", con)
        Da.Fill(dt3)
        cbo_ClothName.DataSource = dt3
        cbo_ClothName.DisplayMember = "Cloth_Name"

        Da = New SqlClient.SqlDataAdapter("select EndsCount_Name from EndsCount_Head order by EndsCount_Name", con)
        Da.Fill(dt4)
        cbo_EndsCount.DataSource = dt4
        cbo_EndsCount.DisplayMember = "EndsCount_Name"


        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1574" Then '---- VASSA TEXTILE MILLS PRIVATE LIMITED (PERUNDURAI)
            btn_get_Weft_CountName_from_Master.Visible = True
        End If

        cbo_WidthType.Items.Clear()
        cbo_WidthType.Items.Add("")
        'cbo_WidthType.Items.Add("SINGLE")
        'cbo_WidthType.Items.Add("DOUBLE")
        'cbo_WidthType.Items.Add("TRIPLE")
        'cbo_WidthType.Items.Add("FOURTH")

        cbo_WidthType.Items.Add("SINGLE FABRIC FROM 1 BEAM")
        cbo_WidthType.Items.Add("SINGLE FABRIC FROM 2 BEAMS")

        cbo_WidthType.Items.Add("DOUBLE FABRIC FROM 1 BEAM")
        cbo_WidthType.Items.Add("DOUBLE FABRIC FROM 2 BEAMS")

        cbo_WidthType.Items.Add("TRIPLE FABRIC FROM 1 BEAM")
        cbo_WidthType.Items.Add("TRIPLE FABRIC FROM 2 BEAMS")

        cbo_WidthType.Items.Add("FOUR FABRIC FROM 1 BEAM")
        cbo_WidthType.Items.Add("FOUR FABRIC FROM 2 BEAMS")

        cbo_WidthType.Items.Add("FIVE FABRIC FROM 1 BEAM")
        cbo_WidthType.Items.Add("FIVE FABRIC FROM 2 BEAMS")

        cbo_WidthType.Items.Add("SIX FABRIC FROM 1 BEAM")
        cbo_WidthType.Items.Add("SIX FABRIC FROM 2 BEAMS")

        pnl_filter.Visible = False
        pnl_filter.Left = (Me.Width - pnl_filter.Width) \ 2
        pnl_filter.Top = ((Me.Height - pnl_filter.Height) \ 2) + 20

        btn_UserModification.Visible = False
        If Common_Procedures.settings.User_Modifications_Show_Status = 1 Then
            If Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_UserModification_Status = 1 Then
                btn_UserModification.Visible = True
            End If
        End If


        AddHandler msk_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ClothName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_EndsCount.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_LoomNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_WidthType.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_BeamNo1.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_BeamNo2.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Meters.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_weight.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_folding.GotFocus, AddressOf ControlGotFocus

        AddHandler msk_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ClothName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_EndsCount.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_LoomNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_WidthType.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_BeamNo1.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_BeamNo2.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Meters.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_weight.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_folding.LostFocus, AddressOf ControlLostFocus

        AddHandler msk_Date.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler msk_Date.KeyPress, AddressOf TextBoxControlKeyPress


        AddHandler txt_weight.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_weight.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler txt_folding.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_folding.KeyPress, AddressOf TextBoxControlKeyPress

        If Common_Procedures.settings.CustomerCode = "1233" Then
            cbo_WidthType.Enabled = False
        End If

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        dgv_filter.RowTemplate.Height = 27

        Filter_Status = False
        FrmLdSTS = True
        new_record()

    End Sub


    Private Sub JobWork_Production_Entry_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub JobWork_Production_Entry_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress

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
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

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
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim cmd As New SqlClient.SqlCommand
        Dim tr As SqlClient.SqlTransaction
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim NewCode As String = ""
        Dim DelvSts As Integer = 0
        Dim vOrdByNo As String = ""

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_RollNo.Text)

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RollNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)


        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.Jobwork_Production_Entry, New_Entry, Me, con, "JobWork_Production_Head", "JobWork_Production_Code", NewCode, "JobWork_Production_Date", "(JobWork_Production_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub




        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.JobWork_Production_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.JobWork_Production_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RollNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Da = New SqlClient.SqlDataAdapter("select JobWork_Delivery_Code from JobWork_Production_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and JobWork_Production_Code = '" & Trim(NewCode) & "'", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)

        DelvSts = 0
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0).Item("JobWork_Delivery_Code").ToString) = False Then
                If Trim(Dt1.Rows(0).Item("JobWork_Delivery_Code").ToString) <> "" Then
                    MessageBox.Show("Already this roll delivered", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If
        Dt1.Clear()
        Dt1.Dispose()
        Da.Dispose()

        tr = con.BeginTransaction

        Try

            cmd.Connection = con
            cmd.Transaction = tr
            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "DELETE", "JobWork_Production_Head", "JobWork_Production_Code", Val(lbl_Company.Tag), NewCode, lbl_RollNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "", "", "JobWork_Production_Code, Company_IdNo, for_OrderBy", tr)

            cmd.CommandText = "Delete from Stock_Cloth_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Pavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from JobWork_Production_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and JobWork_Production_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            tr.Commit()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception

            tr.Rollback()
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        If msk_Date.Visible And msk_Date.Enabled Then msk_Date.Focus()

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        If Filter_Status = False Then
            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable

            da = New SqlClient.SqlDataAdapter("select a.Ledger_DisplayName from Ledger_AlaisHead a, ledger_head b where (a.Ledger_IdNo = 0 or b.AccountsGroup_IdNo = 10) and a.Ledger_IdNo = b.Ledger_IdNo order by a.Ledger_DisplayName", con)
            da.Fill(dt1)
            cbo_Filter_PartyName.DataSource = dt1
            cbo_Filter_PartyName.DisplayMember = "Ledger_DisplayName"

            dtp_FilterFrom_date.Text = ""
            dtp_FilterTo_date.Text = ""
            pnl_filter.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1
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

        '  If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.JobWork_Production_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.JobWork_Production_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub


        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.Jobwork_Production_Entry, New_Entry, Me) = False Then Exit Sub





        Try

            inpno = InputBox("Enter New Roll.No.", "FOR INSERTION...")

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.CommandText = "select JobWork_Production_No from JobWork_Production_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and JobWork_Production_Code = '" & Trim(NewCode) & "'"
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
                    MessageBox.Show("Invalid Roll.No", "DOES NOT INSERT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_RollNo.Text = Trim(UCase(inpno))

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
            cmd.CommandText = "select top 1 JobWork_Production_No from JobWork_Production_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and JobWork_Production_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, JobWork_Production_No"
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
            cmd.CommandText = "select top 1 JobWork_Production_No from JobWork_Production_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and JobWork_Production_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, JobWork_Production_No desc"
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RollNo.Text))

            cmd.Connection = con
            cmd.CommandText = "select top 1 JobWork_Production_No from JobWork_Production_Head where for_orderby > " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and JobWork_Production_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, JobWork_Production_No"
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RollNo.Text))

            cmd.Connection = con
            cmd.CommandText = "select top 1 JobWork_Production_No from JobWork_Production_Head where for_orderby < " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and  JobWork_Production_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, JobWork_Production_No desc"
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
        Dim dt1 As New DataTable
        Dim NewID As Integer = 0

        Try
            clear()

            New_Entry = True

            da = New SqlClient.SqlDataAdapter("select max(for_orderby) from JobWork_Production_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and JobWork_Production_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' ", con)
            da.Fill(dt)

            NewID = 0
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    NewID = Val(dt.Rows(0)(0).ToString)
                End If
            End If

            NewID = NewID + 1

            lbl_RollNo.Text = NewID
            lbl_RollNo.ForeColor = Color.Red


            msk_Date.Text = Date.Today.ToShortDateString
            da = New SqlClient.SqlDataAdapter("select top 1 *, b.Cloth_Name , c.EndsCount_Name , d.Count_Name from JobWork_Production_Head a Left Outer Join Cloth_Head b On a.Cloth_idno = b.Cloth_idno Left Outer Join EndsCount_Head c On a.Endscount_idno = c.Endscount_idno Left Outer join Count_Head d on a.Count_idno = d.Count_idno where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.JobWork_Production_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by a.for_Orderby desc, a.JobWork_Production_No desc", con)
            dt1 = New DataTable
            da.Fill(dt1)
            If dt1.Rows.Count > 0 Then
                If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)
                    If dt1.Rows(0).Item("JobWork_Production_Date").ToString <> "" Then msk_Date.Text = dt1.Rows(0).Item("JobWork_Production_Date").ToString
                End If

                If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1139" Then '------- SivaKumar Textiles
                    If dt1.Rows(0).Item("Cloth_Name").ToString <> "" Then
                        cbo_ClothName.Text = dt1.Rows(0).Item("Cloth_Name").ToString
                        cbo_ClothName.Tag = cbo_ClothName.Text
                    End If
                    If dt1.Rows(0).Item("EndsCount_Name").ToString <> "" Then cbo_EndsCount.Text = dt1.Rows(0).Item("EndsCount_Name").ToString
                End If

                If dt1.Rows(0).Item("Count_Name").ToString <> "" Then lbl_WeftCount.Text = dt1.Rows(0).Item("Count_Name").ToString
                If dt1.Rows(0).Item("Folding_percentage").ToString <> "" Then txt_folding.Text = dt1.Rows(0).Item("Folding_percentage").ToString
            End If
            dt1.Clear()


            If msk_Date.Enabled And msk_Date.Visible Then
                msk_Date.Focus()
                msk_Date.SelectionStart = 0
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

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

            inpno = InputBox("Enter Roll.No", "FOR FINDING...")

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.CommandText = "select JobWork_Production_No from JobWork_Production_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and JobWork_Production_Code = '" & Trim(NewCode) & "'"
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
                MessageBox.Show("Roll.No. Does not exists", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        '------ No Print
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
        Dim led_id As Integer = 0
        Dim Clo_ID As Integer = 0
        Dim EdsCnt_ID As Integer = 0
        Dim Cnt_ID As Integer = 0
        Dim Lm_ID As Integer = 0
        Dim Partcls As String, PBlNo As String, Ent_ID As String
        Dim DelvSts As Integer
        Dim PavuConsMtrs As Single = 0
        'Dim NoofBeams As Integer = 0
        'Dim WidTyp As Single = 0
        'Dim Crmp_Perc As Single = 0
        'Dim Crmp_Mtrs As Single = 0
        Dim vOrdByNo As String = ""
        Dim vProdMtrs As String = 0

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_RollNo.Text)
        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RollNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Jobwork_Production_Entry, New_Entry, Me, con, "JobWork_Production_Head", "JobWork_Production_Code", NewCode, "JobWork_Production_Date", "(JobWork_Production_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and JobWork_Production_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, JobWork_Production_No desc", dtp_Date.Value.Date) = False Then Exit Sub

        If pnl_back.Enabled = False Then
            MessageBox.Show("Close all other windows", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        ' If Common_Procedures.UserRight_Check(Common_Procedures.UR.JobWork_Production_Entry, New_Entry) = False Then Exit Sub


        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If


        If IsDate(msk_Date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If msk_Date.Enabled Then msk_Date.Focus()
            Exit Sub
        End If


        If Not (Convert.ToDateTime(msk_Date.Text) >= Common_Procedures.Company_FromDate And Convert.ToDateTime(msk_Date.Text) <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If msk_Date.Enabled Then msk_Date.Focus()
            Exit Sub
        End If

        led_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyName.Text)
        If led_id = 0 Then
            MessageBox.Show("Invalid Ledger Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_PartyName.Enabled Then cbo_PartyName.Focus()
            Exit Sub
        End If

        Clo_ID = Common_Procedures.Cloth_NameToIdNo(con, cbo_ClothName.Text)
        If Clo_ID = 0 Then
            MessageBox.Show("Invalid Cloth Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_ClothName.Enabled Then cbo_ClothName.Focus()
            Exit Sub
        End If

        EdsCnt_ID = Common_Procedures.EndsCount_NameToIdNo(con, cbo_EndsCount.Text)
        If EdsCnt_ID = 0 Then
            MessageBox.Show("Invalid Ends/Count", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_EndsCount.Enabled Then cbo_EndsCount.Focus()
            Exit Sub
        End If

        Cnt_ID = Common_Procedures.Count_NameToIdNo(con, lbl_WeftCount.Text)
        If Cnt_ID = 0 Then
            MessageBox.Show("Invalid Count Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_ClothName.Enabled Then cbo_ClothName.Focus()
            Exit Sub
        End If

        If Val(txt_Meters.Text) = 0 Then
            MessageBox.Show("Invalid Receipt Meters", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If txt_Meters.Enabled Then txt_Meters.Focus()
            Exit Sub
        End If

        If Val(txt_folding.Text) = 0 Then
            txt_folding.Text = 100
        End If


        'If Common_Procedures.settings.CustomerCode <> "1233" Then '--vipin
        '    If Val(txt_weight.Text) = 0 Then
        '        MessageBox.Show("Invalid Weight", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '        If txt_weight.Enabled Then txt_weight.Focus()
        '        Exit Sub
        '    End If
        'End If

        If Common_Procedures.settings.CustomerCode <> "1233" Then
            If Common_Procedures.settings.JobWorker_PavuWidthWiseConsumption_IN_Delivery = 0 Then
                If Trim(cbo_WidthType.Text) = "" Then
                    MessageBox.Show("Invalid Width Type", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    If cbo_WidthType.Enabled Then cbo_WidthType.Focus()
                    Exit Sub
                End If
            End If
        End If
        Lm_ID = Common_Procedures.Loom_NameToIdNo(con, cbo_LoomNo.Text)

        DelvSts = 0

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RollNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_RollNo.Text = Common_Procedures.get_MaxCode(con, "JobWork_Production_Head", "JobWork_Production_Code", "for_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RollNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@ProdDate", Convert.ToDateTime(msk_Date.Text))

            If New_Entry = True Then

                cmd.CommandText = "Insert into JobWork_Production_Head(JobWork_Production_Code,           Company_IdNo           ,        JobWork_Production_No   ,                               for_OrderBy                               , JobWork_Production_Date,      Ledger_IdNo   ,       Cloth_Idno   ,          EndsCount_IdNo    ,        Count_IdNo       ,         Receipt_Meters      ,         Actual_Meters       , Folding,          Rough_Consumed_Yarn      ,             Consumed_Yarn         ,       Loom_IdNo   ,             Width_Type            ,             Beam_No1            ,                Beam_No2         , JobWork_Delivery_Code, JobWork_Delivery_Date, JobWork_Delivery_Increment, JobWork_Inspection_Code, JobWork_Inspection_Date, JobWork_Inspection_Increment, Cloth_Type1_Meters, Cloth_Type2_Meters, Cloth_Type3_Meters, Cloth_Type4_Meters, Cloth_Type5_Meters   ,Weight                         ,Folding_Percentage) " &
                                        "      Values                 ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RollNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RollNo.Text))) & ",         @ProdDate      , " & Val(led_id) & ", " & Val(Clo_ID) & ", " & Str(Val(EdsCnt_ID)) & ", " & Str(Val(Cnt_ID)) & ", " & Val(txt_Meters.Text) & ", " & Val(txt_Meters.Text) & ",   " & Val(txt_folding.Text) & "  , " & Val(lbl_ConsumedYarn.Text) & ", " & Val(lbl_ConsumedYarn.Text) & ", " & Val(Lm_ID) & ", '" & Trim(cbo_WidthType.Text) & "', '" & Trim(txt_BeamNo1.Text) & "', '" & Trim(txt_BeamNo2.Text) & "',        ''            ,         Null         ,               0           ,          ''            ,           Null         ,               0             ,        0          ,          0        ,        0          ,        0          ,        0             , " & Val(txt_weight.Text) & "  ," & Val(txt_folding.Text) & ")"

                cmd.ExecuteNonQuery()

            Else
                Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "OLD", "JobWork_Production_Head", "JobWork_Production_Code", Val(lbl_Company.Tag), NewCode, lbl_RollNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "JobWork_Production_Code, Company_IdNo, for_OrderBy", tr)

                da = New SqlClient.SqlDataAdapter("select JobWork_Delivery_Code from JobWork_Production_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and JobWork_Production_Code = '" & Trim(NewCode) & "'", con)
                da.SelectCommand.Transaction = tr
                dt1 = New DataTable
                da.Fill(dt1)

                DelvSts = 0
                If dt1.Rows.Count > 0 Then
                    If IsDBNull(dt1.Rows(0).Item("JobWork_Delivery_Code").ToString) = False Then
                        If Trim(dt1.Rows(0).Item("JobWork_Delivery_Code").ToString) <> "" Then
                            DelvSts = 1
                        End If
                    End If
                End If

                cmd.CommandText = "Update JobWork_Production_Head set JobWork_Production_Date = @ProdDate, Ledger_IdNo = " & Str(Val(led_id)) & ", Cloth_Idno = " & Str(Val(Clo_ID)) & ", EndsCount_IdNo = " & Str(Val(EdsCnt_ID)) & ", Count_IdNo = " & Str(Val(Cnt_ID)) & ", Receipt_Meters = " & Str(Val(txt_Meters.Text)) & ", Rough_Consumed_Yarn = " & Str(Val(lbl_ConsumedYarn.Text)) & ", Loom_IdNo = " & Str(Val(Lm_ID)) & ", Beam_No1 = '" & Trim(txt_BeamNo1.Text) & "', Width_Type = '" & Trim(cbo_WidthType.Text) & "', Beam_No2 = '" & Trim(txt_BeamNo2.Text) & "' ,Weight=" & Val(txt_weight.Text) & " ,Folding=" & Val(txt_folding.Text) & " , Folding_Percentage=" & Val(txt_folding.Text) & " Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and JobWork_Production_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If
            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "NEW", "JobWork_Production_Head", "JobWork_Production_Code", Val(lbl_Company.Tag), NewCode, lbl_RollNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "JobWork_Production_Code, Company_IdNo, for_OrderBy", tr)



            Ent_ID = Trim(Pk_Condition) & Trim(lbl_RollNo.Text)
            Partcls = "Prod : Roll.No. " & Trim(lbl_RollNo.Text)
            PBlNo = Trim(lbl_RollNo.Text)

            If DelvSts = 0 Then

                cmd.CommandText = "Update JobWork_Production_Head set Actual_Meters = " & Str(Val(txt_Meters.Text)) & ", Consumed_Yarn = " & Str(Val(lbl_ConsumedYarn.Text)) & " Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and JobWork_Production_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Delete from Stock_Pavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Delete from Stock_Cloth_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                If Val(txt_Meters.Text) <> 0 Then

                    If Common_Procedures.settings.JobWorker_Pavu_Yarn_Stock_Posting_IN_Production = 1 Then

                        vProdMtrs = Format(Val(txt_Meters.Text), "#########0.00")
                        If txt_folding.Visible = True Then
                            vProdMtrs = Format((Val(txt_Meters.Text) * Val(txt_folding.Text)) / 100, "#########0.00")
                        End If

                        PavuConsMtrs = Common_Procedures.get_Pavu_Consumption(con, Clo_ID, Lm_ID, Val(vProdMtrs), Trim(cbo_WidthType.Text), tr, , , True)

                        ''NoofBeams = Val(Common_Procedures.get_FieldValue(con, "Loom_Head", "Noof_Input_Beams", "(Loom_IdNo = " & Str(Val(Lm_ID)) & ")", , tr))

                        ''Crmp_Perc = Val(Common_Procedures.get_FieldValue(con, "Cloth_Head", "Crimp_Percentage", "(Cloth_IdNo = " & Str(Val(Clo_ID)) & ")", , tr))

                        ''If Val(NoofBeams) = 0 Then NoofBeams = 1

                        ''WidTyp = 0
                        ''If Trim(UCase(cbo_WidthType.Text)) = "FOURTH" Then
                        ''    WidTyp = 4
                        ''ElseIf Trim(UCase(cbo_WidthType.Text)) = "TRIPLE" Then
                        ''    WidTyp = 3
                        ''ElseIf Trim(UCase(cbo_WidthType.Text)) = "DOUBLE" Then
                        ''    WidTyp = 2
                        ''Else
                        ''    WidTyp = 1
                        ''End If

                        ''PavuConsMtrs = (Val(txt_Meters.Text) / Val(WidTyp)) * Val(NoofBeams)

                        ''Crmp_Mtrs = Val(PavuConsMtrs) * Crmp_Perc / 100

                        ''PavuConsMtrs = Format(PavuConsMtrs + Crmp_Mtrs, "#########0.00")

                        cmd.CommandText = "Insert into Stock_Pavu_Processing_Details ( Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Cloth_Idno, Entry_ID, Party_Bill_No, Particulars, Sl_No, EndsCount_IdNo, Sized_Beam, Meters ) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RollNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RollNo.Text))) & ", @ProdDate, " & Str(Val(led_id)) & ", 0, " & Str(Val(Clo_ID)) & ", '" & Trim(Ent_ID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', 1, " & Str(Val(EdsCnt_ID)) & ", 0, " & Str(Val(PavuConsMtrs)) & " )"
                        cmd.ExecuteNonQuery()

                    End If

                End If


                cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                If Common_Procedures.settings.JobWorker_Pavu_Yarn_Stock_Posting_IN_Production = 1 Then
                    If Val(lbl_ConsumedYarn.Text) <> 0 Then
                        cmd.CommandText = "Insert into Stock_Yarn_Processing_Details ( Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Entry_ID, Party_Bill_No, Particulars, Sl_No, Count_IdNo, Yarn_Type, Mill_IdNo, Weight) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RollNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RollNo.Text))) & ", @ProdDate, " & Str(Val(led_id)) & ", 0, '" & Trim(Ent_ID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', 1, " & Str(Val(Cnt_ID)) & ", 'MILL', 0, " & Str(Val(lbl_ConsumedYarn.Text)) & " )"
                        cmd.ExecuteNonQuery()
                    End If
                End If

                cmd.CommandText = "Insert into Stock_Cloth_Processing_Details (   Reference_Code     ,             Company_IdNo         ,             Reference_No       ,                               for_OrderBy                               , Reference_Date,         StockOff_IdNo   ,                               DeliveryTo_Idno             ,       ReceivedFrom_Idno ,         Entry_ID      ,      Party_Bill_No   ,      Particulars       , Sl_No,          Cloth_Idno     , Folding, UnChecked_Meters,                Meters_Type1       , Meters_Type2, Meters_Type3, Meters_Type4, Meters_Type5 ) " &
                                            "    Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RollNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RollNo.Text))) & ",    @ProdDate  , " & Str(Val(led_id)) & ", " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & ", " & Str(Val(led_id)) & ", '" & Trim(Ent_ID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "',   1  , " & Str(Val(Clo_ID)) & ",   100  ,        0        ,  " & Str(Val(txt_Meters.Text)) & ",       0     ,       0     ,       0     ,       0      ) "
                cmd.ExecuteNonQuery()

            End If

            tr.Commit()

            If New_Entry = True Then
                'move_record(lbl_RefNo.Text)
                new_record()
            End If

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        If msk_Date.Visible And msk_Date.Enabled Then msk_Date.Focus()

    End Sub



    Private Sub cbo_partyname_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyName.KeyDown
        Try
            With cbo_PartyName
                If e.KeyValue = 40 And .DroppedDown = False Then
                    e.Handled = True
                    cbo_ClothName.Focus()
                    ' SendKeys.Send("{TAB}")
                ElseIf e.KeyValue = 38 And .DroppedDown = False Then
                    e.Handled = True
                    msk_Date.Focus()
                ElseIf e.KeyValue <> 13 And e.KeyValue <> 17 And e.KeyValue <> 27 And .DroppedDown = False Then
                    .DroppedDown = True
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT ITEM...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_partyname_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PartyName.KeyPress

        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Condt As String
        Dim FindStr As String

        Try

            With cbo_PartyName

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

                        cbo_ClothName.Focus()

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

                        Condt = "(ledger_idno = 0 or Ledger_Type = 'JOBWORKER')"
                        If Trim(FindStr) <> "" Then
                            Condt = " Ledger_Type = 'JOBWORKER' and (Ledger_DisplayName like '" & FindStr & "%' or Ledger_DisplayName like '% " & FindStr & "%') "
                        End If

                        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where " & Condt & " Order by Ledger_DisplayName", con)
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
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_partyname_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyName.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Common_Procedures.MDI_LedType = "JOBWORKER"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_PartyName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If

    End Sub

    Private Sub dtp_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyCode = 40 Then e.SuppressKeyPress = True : SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then e.SuppressKeyPress = True : SendKeys.Send("+{TAB}")
    End Sub

    Private Sub dtp_Date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
    End Sub

    Public Sub New()
        FrmLdSTS = True
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Private Sub cbo_Filter_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_PartyName.GotFocus
        With cbo_Filter_PartyName
            .BackColor = Color.Lime
            .ForeColor = Color.Blue
            .SelectionStart = 0
            .SelectionLength = .Text.Length
        End With
    End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        Try
            With cbo_Filter_PartyName
                If e.KeyValue = 38 And .DroppedDown = False Then
                    e.Handled = True
                    dtp_FilterTo_date.Focus()
                    'SendKeys.Send("+{TAB}")
                ElseIf e.KeyValue = 40 And .DroppedDown = False Then
                    e.Handled = True
                    btn_filtershow.Focus()
                    'SendKeys.Send("{TAB}")
                ElseIf e.KeyValue <> 13 And .DroppedDown = False Then
                    .DroppedDown = True
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Condt As String
        Dim FindStr As String

        With cbo_Filter_PartyName

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

                btn_filtershow.Focus()

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

                Condt = "(ledger_idno = 0 or Ledger_Type = 'JOBWORKER')"
                If Trim(FindStr) <> "" Then
                    Condt = " Ledger_Type = 'JOBWORKER' and (Ledger_DisplayName like '" & FindStr & "%' or Ledger_DisplayName like '% " & FindStr & "%') "
                End If

                da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where " & Condt & " Order by Ledger_DisplayName", con)
                dt = New DataTable
                da.Fill(dt)

                .DataSource = dt
                .DisplayMember = "Ledger_DisplayName"


                .Text = Trim(FindStr)

                .SelectionStart = FindStr.Length

                e.Handled = True

            End If

        End With

    End Sub

    Private Sub dtp_FilterTo_date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_FilterTo_date.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub


    Private Sub dtp_FilterTo_date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_FilterTo_date.KeyPress
        If Asc(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub


    Private Sub btn_filtershow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_filtershow.Click
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

            If IsDate(dtp_FilterFrom_date.Value) = True And IsDate(dtp_FilterTo_date.Value) = True Then
                Condt = "a.JobWork_Production_Date between '" & Trim(Format(dtp_FilterFrom_date.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_FilterTo_date.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_FilterFrom_date.Value) = True Then
                Condt = "a.JobWork_Production_Date = '" & Trim(Format(dtp_FilterFrom_date.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_FilterTo_date.Value) = True Then
                Condt = "a. JobWork_Production_Date= '" & Trim(Format(dtp_FilterTo_date.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If

            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " (a.Ledger_Idno = " & Str(Val(Led_IdNo)) & ")"
            End If

            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name from JobWork_Production_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo  where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.JobWork_Production_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.JobWork_Production_No", con)
            dt2 = New DataTable
            da.Fill(dt2)

            dgv_filter.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_filter.Rows.Add()

                    dgv_filter.Rows(n).Cells(0).Value = dt2.Rows(i).Item("JobWork_Production_No").ToString
                    dgv_filter.Rows(n).Cells(1).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("JobWork_Production_Date").ToString), "dd-MM-yyyy")
                    dgv_filter.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_filter.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Receipt_Meters").ToString

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

    Private Sub dtp_FilterFrom_date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_FilterFrom_date.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub dtp_FilterFrom_date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_FilterFrom_date.KeyPress
        If Asc(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub dgv_filter_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_filter.DoubleClick
        Open_FilterEntry()
    End Sub

    Private Sub btn_filtershow_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles btn_filtershow.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub btn_filtershow_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles btn_filtershow.KeyPress
        If Asc(e.KeyChar) = 13 Then
            cbo_Filter_PartyName.Focus()
        End If
    End Sub


    Private Sub btn_closefilter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_closefilter.Click
        pnl_back.Enabled = True
        pnl_filter.Visible = False
        Filter_Status = False
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

   
    
    Private Sub cbo_ClothName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ClothName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")
        cbo_ClothName.Tag = cbo_ClothName.Text
    End Sub

    Private Sub cbo_ClothName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ClothName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_ClothName, cbo_PartyName, cbo_EndsCount, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")

    End Sub

    Private Sub cbo_ClothName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_ClothName.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_ClothName, cbo_EndsCount, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then
            If Trim(UCase(cbo_ClothName.Text)) <> Trim(UCase(cbo_ClothName.Tag)) Or Trim(lbl_WeftCount.Text) = "" Then
                get_CLOTHDETAILS_ENDS_WEFT()
            End If
        End If

    End Sub

    Private Sub cbo_ClothName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ClothName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Cloth_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_ClothName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If

    End Sub

    Private Sub cbo_ClothName_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ClothName.LostFocus
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Clo_ID As Integer

        If Trim(UCase(cbo_ClothName.Text)) <> Trim(UCase(cbo_ClothName.Tag)) Or Trim(lbl_WeftCount.Text) = "" Then
            get_CLOTHDETAILS_ENDS_WEFT()
        End If

        'With cbo_ClothName

        '    If Trim(UCase(.Tag)) <> Trim(UCase(.Text)) Then

        '        Clo_ID = Common_Procedures.Cloth_NameToIdNo(con, .Text)

        '        Da = New SqlClient.SqlDataAdapter("select b.Count_Name as Weft_CountName from cloth_head a, Count_Head b where a.cloth_idno = " & Str(Val(Clo_ID)) & " and a.Cloth_WeftCount_IdNo = b.Count_IdNo", con)
        '        Da.Fill(Dt1)

        '        If Dt1.Rows.Count > 0 Then
        '            If IsDBNull(Dt1.Rows(0).Item("Weft_CountName").ToString) = False Then
        '                If Dt1.Rows(0).Item("Weft_CountName").ToString <> "" Then
        '                    lbl_WeftCount.Text = Dt1.Rows(0).Item("Weft_CountName").ToString
        '                End If
        '            End If
        '        End If

        '    End If

        'End With

    End Sub


    Private Sub cbo_EndsCount_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_EndsCount.GotFocus

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")

        Dim Clo_IdNo As Integer, edscnt_idno As Integer

        If Trim(cbo_EndsCount.Text) = "" Then
            Clo_IdNo = Common_Procedures.Cloth_NameToIdNo(con, cbo_ClothName.Text)
            edscnt_idno = Val(Common_Procedures.get_FieldValue(con, "Cloth_EndsCount_Details", "EndsCount_IdNo", "(cloth_idno = " & Str(Val(Clo_IdNo)) & " and Sl_No = 1 )"))
            cbo_EndsCount.Text = Common_Procedures.EndsCount_IdNoToName(con, edscnt_idno)
        End If

    End Sub


    Private Sub cbo_EndsCount_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_EndsCount.KeyDown
        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_EndsCount, cbo_ClothName, txt_folding, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")


        'Dim Clo_IdNo As Integer, edscnt_idno As Integer

        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1139" Then ' -----Sivakumar Textiles


        '    If (e.KeyValue = 38 And cbo_EndsCount.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
        '        cbo_ClothName.Focus()
        '    End If

        '    If (e.KeyValue = 40 And cbo_EndsCount.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

        '        Clo_IdNo = Common_Procedures.Cloth_NameToIdNo(con, cbo_ClothName.Text)

        '        If Trim(cbo_EndsCount.Text) = "" Then
        '            edscnt_idno = Val(Common_Procedures.get_FieldValue(con, "Cloth_EndsCount_Details", "EndsCount_IdNo", "(cloth_idno = " & Str(Val(Clo_IdNo)) & " and Sl_No = 1 )"))
        '            cbo_EndsCount.Text = Common_Procedures.EndsCount_IdNoToName(con, edscnt_idno)
        '        End If

        '        txt_folding.Focus()

        '    End If

        'Else

        '    Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_EndsCount, cbo_ClothName, txt_folding, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")

        'End If

    End Sub

    Private Sub cbo_EndsCount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_EndsCount.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_EndsCount, txt_folding, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")

        'If Asc(e.KeyChar) = 13 Then

        '    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1139" Then ' -----Sivakumar Textiles
        '        Dim Clo_IdNo As Integer, edscnt_idno As Integer
        '        Clo_IdNo = Common_Procedures.Cloth_NameToIdNo(con, cbo_ClothName.Text)

        '        If Trim(cbo_EndsCount.Text) = "" Then
        '            edscnt_idno = Val(Common_Procedures.get_FieldValue(con, "Cloth_EndsCount_Details", "EndsCount_IdNo", "(cloth_idno = " & Str(Val(Clo_IdNo)) & " and Sl_No = 1 )"))
        '            cbo_EndsCount.Text = Common_Procedures.EndsCount_IdNoToName(con, edscnt_idno)
        '        End If

        '        txt_folding.Focus()

        '    End If

        'Else

        '    Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_EndsCount, txt_folding, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")

        'End If

    End Sub

    Private Sub cbo_EndsCount_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_EndsCount.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New EndsCount_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_EndsCount.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If

    End Sub

    

    Private Sub cbo_LoomNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_LoomNo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Loom_Head", "Loom_Name", "", "(Loom_IdNo = 0)")

    End Sub

    Private Sub cbo_LoomNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_LoomNo.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_LoomNo, txt_weight, cbo_WidthType, "Loom_Head", "Loom_Name", "", "(Loom_IdNo = 0)")

    End Sub

    Private Sub cbo_LoomNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_LoomNo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_LoomNo, cbo_WidthType, "Loom_Head", "Loom_Name", "", "(Loom_IdNo = 0)")
    End Sub

    Private Sub cbo_LoomNo_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_LoomNo.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            Dim f As New LoomNo_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_LoomNo.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub


    Private Sub cbo_LoomNo_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_LoomNo.LostFocus
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Lm_ID As Integer
        If Common_Procedures.settings.CustomerCode <> "1233" Then


            With cbo_LoomNo


                If Trim(UCase(.Tag)) <> Trim(UCase(.Text)) Then

                    Lm_ID = Common_Procedures.Loom_NameToIdNo(con, .Text)

                    Da = New SqlClient.SqlDataAdapter("select Width_Type from JobWork_Production_Head where loom_idno = " & Str(Val(Lm_ID)) & " Order by JobWork_Production_Date, For_OrderBy, JobWork_Production_No", con)
                    Da.Fill(Dt1)

                    If Dt1.Rows.Count > 0 Then
                        If IsDBNull(Dt1.Rows(0).Item("Width_Type").ToString) = False Then
                            If Dt1.Rows(0).Item("Width_Type").ToString <> "" Then
                                cbo_WidthType.Text = Dt1.Rows(0).Item("Width_Type").ToString
                            End If
                        End If
                    End If

                End If

            End With
        End If
    End Sub

    Private Sub btn_save_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

   

    Private Sub txt_Meters_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Meters.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_Meters_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Meters.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
    End Sub

   
    Private Sub txt_BeamNo1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_BeamNo1.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_BeamNo1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_BeamNo1.KeyPress
        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
    End Sub

  

    Private Sub txt_BeamNo2_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_BeamNo2.KeyDown
        If e.KeyCode = 40 Then btn_save.Focus()
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_BeamNo2_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_BeamNo2.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                msk_Date.Focus()
            End If
        End If
    End Sub

    Private Sub ConsumedYarn_Calculation()
        Dim CloID As Integer
        Dim ConsYarn As Single
        'Dim WgtMtr As Single

        CloID = Common_Procedures.Cloth_NameToIdNo(con, cbo_ClothName.Text)

        ConsYarn = Common_Procedures.get_Weft_ConsumedYarn(con, CloID, Val(txt_Meters.Text),,, Val(txt_folding.Text))

        ''WgtMtr = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Weight_Meter_Weft", "(cloth_idno = " & Str(Val(CloID)) & ")"))
        ''ConsYarn = Val(txt_Meters.Text) * Val(WgtMtr)

        lbl_ConsumedYarn.Text = Format(ConsYarn, "#########0.000")

    End Sub


    Private Sub txt_Meters_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Meters.TextChanged
        ConsumedYarn_Calculation()
    End Sub

    Private Sub cbo_WidthType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_WidthType.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")

    End Sub

    Private Sub cbo_WidthType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_WidthType.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_WidthType, cbo_LoomNo, txt_BeamNo1, "", "", "", "")

    End Sub

    Private Sub cbo_WidthType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_WidthType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_WidthType, txt_BeamNo1, "", "", "", "")
        


    End Sub
 
    

  
   
    Private Sub msk_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_Date.KeyDown
        vcbo_KeyDwnVal = e.KeyValue


        vmskOldText = ""
        vmskSelStrt = -1
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            vmskOldText = msk_Date.Text
            vmskSelStrt = msk_Date.SelectionStart
        End If

    End Sub

    Private Sub msk_Date_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles msk_Date.KeyPress
        If Trim(UCase(e.KeyChar)) = "D" Then
            msk_Date.Text = Date.Today
            msk_Date.SelectionStart = 0
        End If
    End Sub


    Private Sub msk_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_Date.KeyUp
        Dim vmRetTxt As String = ""
        Dim vmRetSelStrt As Integer = -1

        'If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
        '    msk_Date.Text = Date.Today
        'End If
        If e.KeyCode = 107 Then
            msk_Date.Text = DateAdd("D", 1, Convert.ToDateTime(msk_Date.Text))
        ElseIf e.KeyCode = 109 Then
            msk_Date.Text = DateAdd("D", -1, Convert.ToDateTime(msk_Date.Text))
        End If

        If e.KeyCode = 46 Or e.KeyCode = 8 Then

            Common_Procedures.maskEdit_Date_ON_DelBackSpace(sender, e, vmskOldText, vmskSelStrt)

        End If

    End Sub

    Private Sub dtp_Date_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_Date.TextChanged

        If IsDate(dtp_Date.Text) = True Then

            msk_Date.Text = dtp_Date.Text
            msk_Date.SelectionStart = 0
        End If
    End Sub

    Private Sub msk_Date_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles msk_Date.LostFocus

        If IsDate(msk_Date.Text) = True Then
            If Microsoft.VisualBasic.DateAndTime.Day(Convert.ToDateTime(msk_Date.Text)) <= 31 Or Microsoft.VisualBasic.DateAndTime.Month(Convert.ToDateTime(msk_Date.Text)) <= 31 Then
                If Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_Date.Text)) <= 2050 And Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_Date.Text)) >= 2000 Then
                    dtp_Date.Value = Convert.ToDateTime(msk_Date.Text)
                End If
            End If

        End If
    End Sub

    Private Sub dtp_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Date.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            dtp_Date.Text = Date.Today
        End If
    End Sub
  
    Private Sub btn_UserModification_Click(sender As Object, e As System.EventArgs) Handles btn_UserModification.Click
        If Val(Common_Procedures.User.IdNo) = 1 Then
            Dim f1 As New User_Modifications
            f1.Entry_Name = Me.Name
            f1.Entry_PkValue = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RollNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            f1.ShowDialog()
        End If
    End Sub

    Private Sub cbo_ClothName_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_ClothName.SelectedIndexChanged

    End Sub

    Private Sub get_CLOTHDETAILS_ENDS_WEFT()
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim Clo_IdNo As Integer, edscnt_idno As Integer
        Dim wftcnt_idno As Integer


        If Trim(cbo_ClothName.Text) <> "" Then

            If Trim(UCase(cbo_ClothName.Text)) <> Trim(UCase(cbo_ClothName.Tag)) Or Trim(lbl_WeftCount.Text) = "" Then

                cbo_ClothName.Tag = cbo_ClothName.Text

                Clo_IdNo = Common_Procedures.Cloth_NameToIdNo(con, cbo_ClothName.Text)

                wftcnt_idno = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_WeftCount_IdNo", "(cloth_idno = " & Str(Val(Clo_IdNo)) & ")"))
                lbl_WeftCount.Text = Common_Procedures.Count_IdNoToName(con, wftcnt_idno)

                edscnt_idno = Val(Common_Procedures.get_FieldValue(con, "Cloth_EndsCount_Details", "EndsCount_IdNo", "(cloth_idno = " & Str(Val(Clo_IdNo)) & " and Sl_No = 1 )"))
                cbo_EndsCount.Text = Common_Procedures.EndsCount_IdNoToName(con, edscnt_idno)

            End If

        End If

    End Sub

    Private Sub btn_get_Weft_CountName_from_Master_Click(sender As Object, e As EventArgs) Handles btn_get_Weft_CountName_from_Master.Click
        Dim cmd As New SqlClient.SqlCommand
        Dim Clo_IdNo As Integer
        Dim wftcnt_idno As Integer
        Dim Nr As Integer
        Dim NewCode As String

        If Trim(cbo_ClothName.Text) <> "" Then

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RollNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Clo_IdNo = Common_Procedures.Cloth_NameToIdNo(con, cbo_ClothName.Text)

            wftcnt_idno = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_WeftCount_IdNo", "(cloth_idno = " & Str(Val(Clo_IdNo)) & ")"))
            lbl_WeftCount.Text = Common_Procedures.Count_IdNoToName(con, wftcnt_idno)

            cmd.Connection = con

            cmd.CommandText = "Update JobWork_Production_Head set Count_IdNo = " & Str(Val(wftcnt_idno)) & " Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and JobWork_Production_Code = '" & Trim(NewCode) & "'"
            Nr = cmd.ExecuteNonQuery()

            cmd.CommandText = "Update Stock_Yarn_Processing_Details set Count_IdNo = " & Str(Val(wftcnt_idno)) & "  Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            Nr = cmd.ExecuteNonQuery()

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        End If

    End Sub

    Private Sub lbl_ConsumedYarn_Click(sender As Object, e As EventArgs) Handles lbl_ConsumedYarn.Click

    End Sub
End Class