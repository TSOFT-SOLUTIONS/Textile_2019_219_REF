Public Class JobWork_Empty_BeamBagCone_Delivery_Entry
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "JEBDC-"
    Private prn_HdDt As New DataTable
    Private prn_PageNo As Integer
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Public vmskOldText As String = ""
    Public vmskSelStrt As Integer = -1

    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Private Sub clear()
        New_Entry = False
        Insert_Entry = False
        pnl_back.Enabled = True
        pnl_filter.Visible = False
        vmskOldText = ""
        vmskSelStrt = -1
        lbl_dcno.Text = ""
        cbo_delivery_to.Text = ""
        lbl_dcno.ForeColor = Color.Black
        msk_Date.Text = ""
        dtp_date.Text = ""
        cbo_partyname.Text = ""
        cbo_vehicleno.Text = ""
        txt_emptycones.Text = ""
        txt_remarks.Text = ""
        txt_emptybags.Text = ""
        txt_emptybeam.Text = ""
        txt_BeamNo.Text = ""
        cbo_transport.Text = ""
        cbo_partyname.Enabled = True
        cbo_partyname.BackColor = Color.White
        cbo_beam_type.Text = ""

        txt_emptybeam.Enabled = True
        txt_emptybeam.BackColor = Color.White

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

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name,d.Ledger_Name as DeliveryName,T.Ledger_Name as TransportName ,bw.Beam_Width_name as Beam_type from JobWork_Empty_BeamBagCone_Delivery_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Ledger_Head d ON a.DeliveryTo_IdNo = d.Ledger_IdNo LEFT OUTER JOIN Ledger_Head T ON a.Transport_IdNo = T.Ledger_IdNo LEFT OUTER JOIN Beam_Width_Head bw ON a.Beam_Width_IdNo = bw.Beam_Width_IdNo where a.JobWork_Empty_BeamBagCone_Delivery_Code = '" & Trim(NewCode) & "'", con)
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then
                lbl_dcno.Text = dt1.Rows(0).Item("JobWork_Empty_BeamBagCone_Delivery_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("JobWork_Empty_BeamBagCone_Delivery_Date").ToString
                msk_Date.Text = dtp_Date.Text
                cbo_partyname.Text = dt1.Rows(0).Item("Ledger_Name").ToString
                txt_emptybags.Text = dt1.Rows(0).Item("Empty_Bags").ToString
                txt_emptycones.Text = dt1.Rows(0).Item("Empty_Cones").ToString
                cbo_vehicleno.Text = dt1.Rows(0).Item("Vehicle_No").ToString
                txt_remarks.Text = dt1.Rows(0).Item("Remarks").ToString
                txt_emptybeam.Text = dt1.Rows(0).Item("Empty_Beam").ToString
                txt_BeamNo.Text = dt1.Rows(0).Item("Beam_No").ToString
                cbo_delivery_to.Text = dt1.Rows(0).Item("DeliveryName").ToString
                cbo_transport.Text = dt1.Rows(0).Item("TransportName").ToString
                cbo_beam_type.Text = dt1.Rows(0).Item("Beam_Type").ToString

                LockSTS = False
               
                If IsDBNull(dt1.Rows(0).Item("Gate_Pass_Code").ToString) = False Then
                    If Trim(dt1.Rows(0).Item("Gate_Pass_Code").ToString) <> "" Then
                        LockSTS = True
                    End If
                End If
            End If

            dt1.Dispose()
            da1.Dispose()


            If LockSTS = True Then

                cbo_partyname.Enabled = False
                cbo_partyname.BackColor = Color.LightGray

                txt_emptybeam.Enabled = False
                txt_emptybeam.BackColor = Color.LightGray

               

            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If msk_Date.Visible And msk_Date.Enabled Then msk_Date.Focus()

    End Sub

    Private Sub Empty_BeamBagCone_Delivery_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Activated
        Dim da As SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NoofComps As Integer
        Dim CompCondt As String

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_partyname.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_partyname.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_delivery_to.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_delivery_to.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

                CompCondt = ""
                If Trim(UCase(Common_Procedures.User.Type)) = "ACCOUNT" Then
                    CompCondt = "Company_Type = 'ACCOUNT'"
                End If

                da = New SqlClient.SqlDataAdapter("select count(*) from Company_Head Where " & CompCondt & IIf(Trim(CompCondt) <> "", " and ", "") & " Company_IdNo <> 0", con)
                dt1 = New DataTable
                da.Fill(dt1)

                NoofComps = 0
                If dt1.Rows.Count > 0 Then
                    If IsDBNull(dt1.Rows(0)(0).ToString) = False Then
                        NoofComps = Val(dt1.Rows(0)(0).ToString)
                    End If
                End If
                dt1.Clear()

                If Val(NoofComps) = 1 Then

                    da = New SqlClient.SqlDataAdapter("select Company_IdNo, Company_Name from Company_Head Where " & CompCondt & IIf(Trim(CompCondt) <> "", " and ", "") & " Company_IdNo <> 0 Order by Company_IdNo", con)
                    dt1 = New DataTable
                    da.Fill(dt1)

                    If dt1.Rows.Count > 0 Then
                        If IsDBNull(dt1.Rows(0)(0).ToString) = False Then
                            Common_Procedures.CompIdNo = Val(dt1.Rows(0)(0).ToString)
                        End If
                    End If
                    dt1.Clear()

                Else

                    Dim f As New Company_Selection
                    f.ShowDialog()

                End If

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

    Private Sub JobWork_Empty_BeamBagCone_Delivery_Entry_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable

        Me.Text = ""

        con.Open()

        Da = New SqlClient.SqlDataAdapter("select a.Ledger_DisplayName from Ledger_AlaisHead a, ledger_head b where (a.Ledger_IdNo = 0 or b.AccountsGroup_IdNo = 10) and a.Ledger_IdNo = b.Ledger_IdNo order by a.Ledger_DisplayName", con)
        Da.Fill(Dt1)
        cbo_partyname.DataSource = Dt1
        cbo_partyname.DisplayMember = "Ledger_DisplayName"

        Da = New SqlClient.SqlDataAdapter("select vehicle_No from JobWork_Empty_BeamBagCone_Delivery_Head order by Vehicle_No", con)
        Da.Fill(Dt2)
        cbo_vehicleno.DataSource = Dt2
        cbo_vehicleno.DisplayMember = "Vehicle_No"
        Lbl_beam_type_caption.Visible = False
        cbo_beam_type.Visible = False
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1186" Then
            Label11.Text = "EMPTY BEAM DELIVERY"
            Lbl_beam_type_caption.Visible = True
            cbo_beam_type.Visible = True
            Label10.Visible = False
            Label4.Visible = False
            txt_emptybags.Visible = False
            txt_emptycones.Visible = False


        End If
        pnl_filter.Visible = False
        pnl_filter.Left = (Me.Width - pnl_filter.Width) \ 2
        pnl_filter.Top = ((Me.Height - pnl_filter.Height) \ 2) + 20

        AddHandler msk_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_partyname.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_vehicleno.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_BeamNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_emptybags.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_emptybeam.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_emptycones.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_BeamNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_remarks.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_delivery_to.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_transport.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_beam_type.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_beam_type.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_transport.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_delivery_to.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_vehicleno.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_partyname.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_BeamNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_emptybags.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_emptybeam.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_emptycones.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_remarks.LostFocus, AddressOf ControlLostFocus

        AddHandler msk_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Date.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler msk_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        dgv_filter.RowTemplate.Height = 27

        Filter_Status = False
        FrmLdSTS = True
        new_record()

    End Sub

    Private Sub JobWork_Empty_BeamBagCone_Delivery_Entry_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress

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

    Private Sub JobWork_Empty_BeamBagCone_Delivery_Entry_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable

        Dim cmd As New SqlClient.SqlCommand
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_dcno.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.Jobwork_EmptyBeam_Return_Delivery_Entry, New_Entry, Me, con, "JobWork_Empty_BeamBagCone_Delivery_Head", "JobWork_Empty_BeamBagCone_Delivery_Code", NewCode, "JobWork_Empty_BeamBagCone_Delivery_Date", "(JobWork_Empty_BeamBagCone_Delivery_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub

        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_dcno.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Da = New SqlClient.SqlDataAdapter("select * from JobWork_Empty_BeamBagCone_Delivery_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and JobWork_Empty_BeamBagCone_Delivery_Code = '" & Trim(NewCode) & "'", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0).Item("Gate_Pass_Code").ToString) = False Then
                If Trim(Dt1.Rows(0).Item("Gate_Pass_Code").ToString) <> "" Then
                    MessageBox.Show("Already Piece Delivered", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If
        Dt1.Clear()
        Dt1.Dispose()
        Da.Dispose()

        tr = con.BeginTransaction

        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_dcno.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.CommandText = "Delete from Stock_Empty_BeamBagCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from JobWork_Empty_BeamBagCone_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and JobWork_Empty_BeamBagCone_Delivery_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            tr.Commit()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception

            tr.Rollback()
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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
        Dim vCSInvCode As String = ""
        Dim vYSInvCode As String = ""
        Dim vYSMovNo As String = ""
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        Try

            If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Jobwork_EmptyBeam_Return_Delivery_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Jobwork_EmptyBeam_Return_Delivery_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

            inpno = InputBox("Enter New Dc.No.", "FOR INSERTION...")

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.CommandText = "select JobWork_Empty_BeamBagCone_Delivery_No from JobWork_Empty_BeamBagCone_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and JobWork_Empty_BeamBagCone_Delivery_Code = '" & Trim(NewCode) & "'"
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




            vYSMovNo = ""
            If Common_Procedures.settings.Delivery_ContinousNo_Status = 1 Then
                vYSInvCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

                da = New SqlClient.SqlDataAdapter("select Empty_BeamBagCone_Delivery_no from Empty_BeamBagCone_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Empty_BeamBagCone_Delivery_Code = '" & Trim(vYSInvCode) & "'", con)
                dt = New DataTable
                Da.Fill(Dt)
                If Dt.Rows.Count > 0 Then
                    If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                        vYSMovNo = Trim(Dt.Rows(0)(0).ToString)
                    End If
                End If

                Dt.Clear()

            End If



            If Val(movno) <> 0 Then
                move_record(movno)

            ElseIf Val(vYSMovNo) <> 0 Then


                MessageBox.Show("This DC No. is in Jobwork Empty Beam DC", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            Else
                If Val(inpno) = 0 Then
                    MessageBox.Show("Invalid Dc.No", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_dcno.Text = Trim(UCase(inpno))

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
            cmd.CommandText = "select top 1 JobWork_Empty_BeamBagCone_Delivery_No from JobWork_Empty_BeamBagCone_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and JobWork_Empty_BeamBagCone_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "'  Order by for_Orderby, JobWork_Empty_BeamBagCone_Delivery_No"
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
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String

        Try
            cmd.Connection = con
            cmd.CommandText = "select top 1 JobWork_Empty_BeamBagCone_Delivery_No from JobWork_Empty_BeamBagCone_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and JobWork_Empty_BeamBagCone_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "'  Order by for_Orderby desc, JobWork_Empty_BeamBagCone_Delivery_No desc"
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

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String = ""
        Dim OrdByNo As Single = 0

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_dcno.Text))

            cmd.Connection = con
            cmd.CommandText = "select top 1 JobWork_Empty_BeamBagCone_Delivery_No from JobWork_Empty_BeamBagCone_Delivery_Head where for_orderby > " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and JobWork_Empty_BeamBagCone_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "'  Order by for_Orderby, JobWork_Empty_BeamBagCone_Delivery_No"
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_dcno.Text))

            cmd.Connection = con
            cmd.CommandText = "select top 1 JobWork_Empty_BeamBagCone_Delivery_No from JobWork_Empty_BeamBagCone_Delivery_Head where for_orderby < " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and  JobWork_Empty_BeamBagCone_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "'  Order by for_Orderby desc,JobWork_Empty_BeamBagCone_Delivery_No desc"
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
        Dim NewID As Integer = 0

        Try
            clear()

            New_Entry = True
            If Common_Procedures.settings.Delivery_ContinousNo_Status = 1 Then
                lbl_dcno.Text = Common_Procedures.get_Beam_Delivery_MaxCode(con, Val(lbl_Company.Tag), Common_Procedures.FnYearCode)
            Else
                da = New SqlClient.SqlDataAdapter("select max(for_orderby) from JobWork_Empty_BeamBagCone_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and JobWork_Empty_BeamBagCone_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' ", con)
                da.Fill(dt)

                NewID = 0
                If dt.Rows.Count > 0 Then
                    If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                        NewID = Val(dt.Rows(0)(0).ToString)
                    End If
                End If

                NewID = NewID + 1

                lbl_dcno.Text = NewID
            End If

            lbl_dcno.ForeColor = Color.Red

            msk_Date.Text = Date.Today.ToShortDateString
            da = New SqlClient.SqlDataAdapter("select top 1 * from JobWork_Empty_BeamBagCone_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and JobWork_Empty_BeamBagCone_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, JobWork_Empty_BeamBagCone_Delivery_No desc", con)
            dt1 = New DataTable
            da.Fill(dt1)
            If dt1.Rows.Count > 0 Then
                If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)
                    If dt1.Rows(0).Item("JobWork_Empty_BeamBagCone_Delivery_Date").ToString <> "" Then msk_Date.Text = dt1.Rows(0).Item("JobWork_Empty_BeamBagCone_Delivery_Date").ToString
                End If
            End If
            dt1.Clear()

            If msk_Date.Enabled And msk_Date.Visible Then
                msk_Date.Focus()
                msk_Date.SelectionStart = 0
            End If

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
        Dim InvCode As String = ""
        Dim vYSInvCode As String = ""
        Dim vYSMovNo As String = ""
        Dim vOSmovCode As String = ""
        Dim vOSmovNo As String = ""
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim dt1 As New DataTable
        Try

            inpno = InputBox("Enter Dc.No", "FOR FINDING...")

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.CommandText = "select JobWork_Empty_BeamBagCone_Delivery_No from JobWork_Empty_BeamBagCone_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and JobWork_Empty_BeamBagCone_Delivery_Code = '" & Trim(NewCode) & "'"
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


            vYSMovNo = ""
            If Common_Procedures.settings.Delivery_ContinousNo_Status = 1 Then
                vYSInvCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

                da = New SqlClient.SqlDataAdapter("select Empty_BeamBagCone_Delivery_no from Empty_BeamBagCone_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Empty_BeamBagCone_Delivery_Code = '" & Trim(vYSInvCode) & "'", con)
                dt = New DataTable
                da.Fill(dt)
                If dt.Rows.Count > 0 Then
                    If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                        vYSMovNo = Trim(dt.Rows(0)(0).ToString)
                    End If
                End If

                dt.Clear()

            End If



            If Val(movno) <> 0 Then
                move_record(movno)

            ElseIf Val(vYSMovNo) <> 0 Then


                MessageBox.Show("This DC No. is in Jobwork Empty Beam DC", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            Else
                MessageBox.Show("Dc.No. Does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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
        Dim nr As Long = 0
        Dim led_id As Integer = 0
        Dim Partcls As String, PBlNo As String
        Dim Del_id As Integer = 0
        Dim Trans_id As Integer = 0
        Dim BM_Type As Integer = 0
        Dim EntID As String = ""
        Del_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_delivery_to.Text)

        Trans_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_transport.Text)
        BM_Type = Common_Procedures.BeamWidth_NameToIdNo(con, cbo_beam_type.Text)

        If pnl_back.Enabled = False Then
            MessageBox.Show("Close all other windows", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        '  If Common_Procedures.UserRight_Check(Common_Procedures.UR.JobWork_EmptyBeam_Return, New_Entry) = False Then Exit Sub

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(dtp_Date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If dtp_Date.Enabled Then dtp_Date.Focus()
            Exit Sub
        End If

        If Not (dtp_Date.Value.Date >= Common_Procedures.Company_FromDate And dtp_Date.Value.Date <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If dtp_Date.Enabled Then dtp_Date.Focus()
            Exit Sub
        End If


        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_dcno.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Jobwork_EmptyBeam_Return_Delivery_Entry, New_Entry, Me, con, "JobWork_Empty_BeamBagCone_Delivery_Head", "JobWork_Empty_BeamBagCone_Delivery_Code", NewCode, "JobWork_Empty_BeamBagCone_Delivery_Date", "(JobWork_Empty_BeamBagCone_Delivery_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and JobWork_Empty_BeamBagCone_Delivery_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, JobWork_Empty_BeamBagCone_Delivery_No desc", dtp_Date.Value.Date) = False Then Exit Sub


        led_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_partyname.Text)

        If led_id = 0 Then
            MessageBox.Show("Invalid Ledger Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_partyname.Enabled Then cbo_partyname.Focus()
            Exit Sub
        End If

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_dcno.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                If Common_Procedures.settings.Delivery_ContinousNo_Status = 1 Then
                    lbl_dcno.Text = Common_Procedures.get_Beam_Delivery_MaxCode(con, Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)
                Else
                    da = New SqlClient.SqlDataAdapter("select max(for_orderby) from JobWork_Empty_BeamBagCone_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and JobWork_Empty_BeamBagCone_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' ", con)
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
                    If Trim(NewNo) = "" Then NewNo = Trim(lbl_dcno.Text)

                    lbl_dcno.Text = Trim(NewNo)

                    NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_dcno.Text) & "/" & Trim(Common_Procedures.FnYearCode)
                End If
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_dcno.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@DeliveryDate", Convert.ToDateTime(msk_Date.Text))

            If New_Entry = True Then

                cmd.CommandText = "Insert into JobWork_Empty_BeamBagCone_Delivery_Head(JobWork_Empty_BeamBagCone_Delivery_Code, Company_IdNo, JobWork_Empty_BeamBagCone_Delivery_No, for_OrderBy,JobWork_Empty_BeamBagCone_Delivery_Date, Ledger_IdNo, Empty_Beam, Empty_Bags,Empty_Cones,Vehicle_No,Remarks , Beam_No,DeliveryTo_IdNo,Transport_idno ,Beam_Width_IdNo  ) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_dcno.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_dcno.Text))) & ", @DeliveryDate," & Val(led_id) & "," & Val(txt_emptybeam.Text) & ", " & Val(txt_emptybags.Text) & ", " & Val(txt_emptycones.Text) & ",'" & Trim(cbo_vehicleno.Text) & "','" & Trim(txt_remarks.Text) & "' , '" & Trim(txt_BeamNo.Text) & "'," & Val(Del_id) & "," & Val(Trans_id) & " ," & Val(BM_Type) & ")"

                cmd.ExecuteNonQuery()

            Else
                cmd.CommandText = "Update JobWork_Empty_BeamBagCone_Delivery_Head set JobWork_Empty_BeamBagCone_Delivery_Date = @DeliveryDate, Ledger_IdNo = " & Val(led_id) & ", Empty_Beam = " & Val(txt_emptybeam.Text) & ", Empty_Bags = " & Val(txt_emptybags.Text) & ",Empty_Cones=" & Val(txt_emptycones.Text) & ",Vehicle_No='" & Trim(cbo_vehicleno.Text) & "', Remarks='" & Trim(txt_remarks.Text) & "' , Beam_No='" & Trim(txt_BeamNo.Text) & "',DeliveryTo_IdNo=" & Val(Del_id) & " ,Transport_IdNo=" & Val(Trans_id) & ",Beam_Width_IdNo=" & Val(BM_Type) & "  Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and JobWork_Empty_BeamBagCone_Delivery_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If

            cmd.CommandText = "Delete from Stock_Empty_BeamBagCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            Partcls = "Delv : Dc.No. " & Trim(lbl_dcno.Text)
            PBlNo = Trim(lbl_dcno.Text)
            EntID = "JWEBM -" & Trim(NewCode)

            If Val(txt_emptybeam.Text) <> 0 Or Val(txt_emptybags.Text) <> 0 Or Val(txt_emptycones.Text) <> 0 Then
                cmd.CommandText = "Insert into Stock_Empty_BeamBagCone_Processing_Details(Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Party_Bill_No, Sl_No, Empty_Beam, Empty_Bags, Empty_Cones, Particulars , Beam_Width_IdNo , Entry_ID) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_dcno.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_dcno.Text))) & ", @DeliveryDate, " & Str(Val(led_id)) & ", 0, '" & Trim(PBlNo) & "', 1, " & Str(Val(txt_emptybeam.Text)) & ", " & Str(Val(txt_emptybags.Text)) & ", " & Str(Val(txt_emptycones.Text)) & ", '" & Trim(Partcls) & "' , " & Val(BM_Type) & " , '" & Trim(EntID) & "' )"
                cmd.ExecuteNonQuery()
            End If

            tr.Commit()

            If New_Entry = True Then
                'move_record(lbl_RefNo.Text)
                new_record()
            End If

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If msk_Date.Visible And msk_Date.Enabled Then msk_Date.Focus()

    End Sub

   

    Private Sub cbo_partyname_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_partyname.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_partyname, msk_Date, txt_emptybeam, "Ledger_AlaisHead", "Ledger_DisplayName", "(ledger_idno = 0 or Ledger_Type = 'JOBWORKER' )", "(Ledger_IdNo = 0)")


        'Try
        '    With cbo_partyname
        '        If e.KeyValue = 40 And .DroppedDown = False Then
        '            e.Handled = True
        '            txt_emptybeam.Focus()
        '            ' SendKeys.Send("{TAB}")
        '        ElseIf e.KeyValue = 38 And .DroppedDown = False Then
        '            e.Handled = True
        '            msk_Date.Focus()
        '        ElseIf e.KeyValue <> 13 And e.KeyValue <> 17 And e.KeyValue <> 27 And .DroppedDown = False Then
        '            .DroppedDown = True
        '        End If
        '    End With

        'Catch ex As Exception
        '    MessageBox.Show(ex.Message, "DOES NOT SELECT ITEM...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        'End Try

    End Sub

    Private Sub cbo_partyname_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_partyname.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_partyname, txt_emptybeam, "Ledger_AlaisHead", "Ledger_DisplayName", "(ledger_idno = 0 or Ledger_Type = 'JOBWORKER' )", "(Ledger_IdNo = 0)")
        'Dim da As New SqlClient.SqlDataAdapter
        'Dim dt As New DataTable
        'Dim Condt As String
        'Dim FindStr As String

        'Try

        '    With cbo_partyname

        '        If Asc(e.KeyChar) <> 27 Then

        '            If Asc(e.KeyChar) = 13 Then

        '                If Trim(.Text) <> "" Then
        '                    If .DroppedDown = True Then
        '                        If Trim(.SelectedText) <> "" Then
        '                            .Text = .SelectedText
        '                        Else
        '                            If .Items.Count > 0 Then
        '                                .SelectedIndex = 0
        '                                .SelectedItem = .Items(0)
        '                                .Text = .GetItemText(.SelectedItem)
        '                            End If
        '                        End If
        '                    End If
        '                End If

        '                txt_emptybeam.Focus()

        '            Else

        '                Condt = ""
        '                FindStr = ""

        '                If Asc(e.KeyChar) = 8 Then
        '                    If .SelectionStart <= 1 Then
        '                        .Text = ""
        '                    End If

        '                    If Trim(.Text) <> "" Then
        '                        If .SelectionLength = 0 Then
        '                            FindStr = .Text.Substring(0, .Text.Length - 1)
        '                        Else
        '                            FindStr = .Text.Substring(0, .SelectionStart - 1)
        '                        End If
        '                    End If

        '                Else
        '                    If .SelectionLength = 0 Then
        '                        FindStr = .Text & e.KeyChar
        '                    Else
        '                        FindStr = .Text.Substring(0, .SelectionStart) & e.KeyChar
        '                    End If

        '                End If

        '                FindStr = LTrim(FindStr)

        '                Condt = "(a.ledger_idno = 0 or b.AccountsGroup_IdNo = 10)"
        '                If Trim(FindStr) <> "" Then
        '                    Condt = " b.AccountsGroup_IdNo = 10 and (a.Ledger_DisplayName like '" & FindStr & "%' or a.Ledger_DisplayName like '% " & FindStr & "%') "
        '                End If

        '                da = New SqlClient.SqlDataAdapter("select a.Ledger_DisplayName from Ledger_AlaisHead a, ledger_head b where " & Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Ledger_IdNo = b.Ledger_IdNo Order by a.Ledger_DisplayName", con)
        '                da.Fill(dt)

        '                .DataSource = dt
        '                .DisplayMember = "Ledger_DisplayName"

        '                .Text = FindStr

        '                .SelectionStart = FindStr.Length

        '                e.Handled = True

        '            End If

        '        End If

        '    End With

        'Catch ex As Exception
        '    MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        'End Try

    End Sub

    Private Sub cbo_partyname_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_partyname.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Common_Procedures.MDI_LedType = "JOBWORKER"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_partyname.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If

    End Sub

    Private Sub cbo_partyname_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_partyname.LostFocus
        With cbo_partyname
            .BackColor = Color.White
            .ForeColor = Color.Black
        End With
    End Sub
    Private Sub cbo_delivery_to_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_delivery_to.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( Ledger_Type = 'SIZING' or Ledger_Type = 'WEAVER' or Ledger_Type = 'GODOWN' or Ledger_Type = 'JOBWORKER' or (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) or Show_In_All_Entry = 1) ", "(Ledger_idno = 0)")

    End Sub

    Private Sub cbo_delivery_to_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_delivery_to.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_delivery_to, txt_emptybeam, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "( Ledger_Type = 'SIZING' or Ledger_Type = 'WEAVER' or Ledger_Type = 'GODOWN' or Ledger_Type = 'JOBWORKER' or (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) or Show_In_All_Entry = 1) ", "(Ledger_idno = 0)")
        If (e.KeyValue = 40 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If cbo_beam_type.Visible = True Then
                cbo_beam_type.Focus()
            Else
                txt_emptycones.Focus()

            End If
        End If


    End Sub

    Private Sub cbo_delivery_to_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_delivery_to.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_delivery_to, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "( Ledger_Type = 'SIZING' or Ledger_Type = 'WEAVER' or Ledger_Type = 'GODOWN' or Ledger_Type = 'JOBWORKER' or (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) or Show_In_All_Entry = 1) ", "(Ledger_idno = 0)")
        If Asc(e.KeyChar) = 13 Then
            If cbo_beam_type.Visible = True Then
                cbo_beam_type.Focus()
            Else
                txt_emptycones.Focus()

            End If
        End If


    End Sub

    Private Sub cbo_delivery_to_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_delivery_to.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Common_Procedures.MDI_LedType = ""
            Dim f As New Ledger_Creation


            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_delivery_to.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If

    End Sub


    Public Sub Get_vehicle_from_Transport()
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim transport_id As Integer
        transport_id = Common_Procedures.Ledger_NameToIdNo(con, cbo_transport.Text)
        Da = New SqlClient.SqlDataAdapter("select vehicle_no from ledger_head where ledger_idno=" & Str(Val(transport_id)) & "", con)
        Dt = New DataTable
        Da.Fill(Dt)
        If Dt.Rows.Count <> 0 Then
            cbo_vehicleno.Text = Dt.Rows(0).Item("vehicle_no").ToString


        End If
        Dt.Clear()
    End Sub

    Private Sub cbo_transport_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_transport.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = 'TRANSPORT' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) or Show_In_All_Entry = 1) ", "(Ledger_idno = 0)")
        Get_vehicle_from_Transport()
    End Sub

    Private Sub cbo_transport_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_transport.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_transport, txt_BeamNo, cbo_vehicleno, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = 'TRANSPORT' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) or Show_In_All_Entry = 1) ", "(Ledger_idno = 0)")

        Get_vehicle_from_Transport()
    End Sub

    Private Sub cbo_transport_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_transport.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_transport, cbo_vehicleno, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = 'TRANSPORT' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) or Show_In_All_Entry = 1) ", "(Ledger_idno = 0)")


        Get_vehicle_from_Transport()
    End Sub

    Private Sub cbo_transport_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_transport.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            Dim f As New Ledger_Creation
            Common_Procedures.MDI_LedType = "TRANSPORT"

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_transport.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If

    End Sub







    Private Sub txt_emptybeam_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_emptybeam.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub btn_Save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub dtp_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub dtp_Date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Asc(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_emptybags_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_emptybags.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
        'If e.KeyCode = 38 Then cbo_vehicleno.Focus()
    End Sub

    Private Sub txt_emptycones_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_emptycones.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")

    End Sub

    Private Sub txt_remarks_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_remarks.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
        'If e.KeyCode = 38 Then dtp_date.Focus()
    End Sub

    Private Sub cbo_vehicleno_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_vehicleno.KeyDown
        Try
            With cbo_vehicleno
                If e.KeyValue = 38 And .DroppedDown = False Then
                    e.Handled = True
                    txt_BeamNo.Focus()
                    'SendKeys.Send("+{TAB}")
                ElseIf e.KeyValue = 40 And .DroppedDown = False Then
                    e.Handled = True
                    txt_remarks.Focus()
                    'SendKeys.Send("{TAB}")
                ElseIf e.KeyValue <> 13 And .DroppedDown = False Then
                    .DroppedDown = True
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub txt_remarks_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_remarks.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                msk_Date.Focus()
            End If
        End If
    End Sub

    Private Sub txt_emptybeam_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_emptybeam.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_emptybags_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_emptybags.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_emptycones_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_emptycones.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub cbo_vehicleno_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_vehicleno.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Itm_ID As Integer = 0
        Dim Condt As String
        Dim FindStr As String

        Try

            With cbo_vehicleno

                If Asc(e.KeyChar) <> 27 Then

                    If Asc(e.KeyChar) = 13 Then

                        With cbo_partyname
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


                        txt_remarks.Focus()

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

                        Condt = ""
                        If Trim(FindStr) <> "" Then
                            Condt = "Where Vehicle_No like '" & Trim(FindStr) & "%' or Vehicle_No like '%" & Trim(FindStr) & "%'"
                        End If

                        da = New SqlClient.SqlDataAdapter("select Vehicle_No from JobWork_Empty_BeamBagCone_Delivery_Head " & Condt & " order by Vehicle_no", con)
                        dt = New DataTable
                        da.Fill(dt)

                        .DataSource = dt
                        .DisplayMember = "Vehicle_no"

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
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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

                Condt = "(a.ledger_idno = 0 or b.AccountsGroup_IdNo = 10)"
                If Trim(FindStr) <> "" Then
                    Condt = " b.AccountsGroup_IdNo = 10 and (a.Ledger_DisplayName like '" & FindStr & "%' or a.Ledger_DisplayName like '% " & FindStr & "%') "
                End If

                da = New SqlClient.SqlDataAdapter("select a.Ledger_DisplayName from Ledger_AlaisHead a, ledger_head b where " & Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Ledger_IdNo = b.Ledger_IdNo Order by a.Ledger_DisplayName", con)
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
                Condt = "a.JobWork_Empty_BeamBagCone_Delivery_Date between '" & Trim(Format(dtp_FilterFrom_date.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_FilterTo_date.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_FilterFrom_date.Value) = True Then
                Condt = "a.JobWork_Empty_BeamBagCone_Delivery_Date = '" & Trim(Format(dtp_FilterFrom_date.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_FilterTo_date.Value) = True Then
                Condt = "a. JobWork_Empty_BeamBagCone_Delivery_Date= '" & Trim(Format(dtp_FilterTo_date.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If

            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " (a.Ledger_Idno = " & Str(Val(Led_IdNo)) & ")"
            End If

            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name from JobWork_Empty_BeamBagCone_Delivery_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo  where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.JobWork_Empty_BeamBagCone_Delivery_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.JobWork_Empty_BeamBagCone_Delivery_No", con)
            da.Fill(dt2)

            dgv_filter.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_filter.Rows.Add()

                    dgv_filter.Rows(n).Cells(0).Value = dt2.Rows(i).Item("JobWork_Empty_BeamBagCone_Delivery_No").ToString
                    dgv_filter.Rows(n).Cells(1).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("JobWork_Empty_BeamBagCone_Delivery_Date").ToString), "dd-MM-yyyy")
                    dgv_filter.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_filter.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Empty_beam").ToString
                    dgv_filter.Rows(n).Cells(4).Value = dt2.Rows(i).Item("Empty_Bags").ToString
                    dgv_filter.Rows(n).Cells(5).Value = dt2.Rows(i).Item("Empty_Cones").ToString

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

    Public Sub print_record() Implements Interface_MDIActions.print_record
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String


        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_dcno.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.PrintEntry, Common_Procedures.UR.Jobwork_EmptyBeam_Return_Delivery_Entry, New_Entry) = False Then Exit Sub

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.* from JobWork_Empty_BeamBagCone_Delivery_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo where a.JobWork_Empty_BeamBagCone_Delivery_Code = '" & Trim(NewCode) & "'", con)
            da1.Fill(dt1)

            If dt1.Rows.Count <= 0 Then

                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub

            End If

            dt1.Dispose()
            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub

        End Try

        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then

            Try
                PrintDialog1.PrinterSettings = PrintDocument1.PrinterSettings
                If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                    PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings
                    PrintDocument1.Print()
                End If

            Catch ex As Exception
                MessageBox.Show("The printing operation failed" & vbCrLf & ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End Try


        Else
            'Try

            Dim ppd As New PrintPreviewDialog

            ppd.Document = PrintDocument1

            ppd.WindowState = FormWindowState.Normal
            ppd.StartPosition = FormStartPosition.CenterScreen
            ppd.ClientSize = New Size(600, 600)
            ppd.ShowDialog()


            'Catch ex As Exception
            '    MsgBox("The printing operation failed" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "DOES NOT SHOW PRINT PREVIEW...")

            'End Try

        End If

    End Sub

    Private Sub PrintDocument1_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument1.BeginPrint
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim NewCode As String

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_dcno.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt = New DataTable
        prn_PageNo = 0

        'Try

        da1 = New SqlClient.SqlDataAdapter("select a.*, b.*,T.Ledger_mAINname as transport,bw.Beam_Width_Name as Beam_Type, c.*,g.Ledger_mAINname as Del_name,g.Ledger_Address1 as del_address1 ,g.Ledger_Address2 as del_address2 ,g.Ledger_Address3 as del_address3 ,g.Ledger_Address4 as del_address4 ,g.Ledger_GSTinNo as DeliveryTo_LedgerGSTinNo, Dsh.State_Name as DeliveryTo_State_Name,Lsh.State_Name as Ledger_State_Name from JobWork_Empty_BeamBagCone_Delivery_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo  LEFT OUTER JOIN Ledger_Head g ON a.DeliveryTO_IdNo = g.Ledger_IdNo LEFT OUTER JOIN State_Head Dsh ON g.Ledger_State_IdNo = Dsh.State_IdNo  LEFT OUTER JOIN State_Head Lsh ON c.Ledger_State_IdNo = Lsh.State_IdNo LEFT OUTER JOIN Beam_Width_Head bw ON a.Beam_Width_IdNo = bw.Beam_Width_IdNo LEFT OUTER JOIN Ledger_Head T ON T.Ledger_IdNo = a.Transport_IdNo where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.JobWork_Empty_BeamBagCone_Delivery_Code = '" & Trim(NewCode) & "'", con)
        da1.Fill(prn_HdDt)

        If prn_HdDt.Rows.Count <= 0 Then

            MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End If

        da1.Dispose()

        'Catch ex As Exception
        '    MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        'End Try

    End Sub

    Private Sub PrintDocument1_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        If prn_HdDt.Rows.Count <= 0 Then Exit Sub
        If Common_Procedures.settings.CustomerCode = "1186" Then
            Printing_Format1186(e)
        Else
            Printing_Format1(e)
        End If

    End Sub

    Private Sub Printing_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim pFont As Font, p1Font As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single = 0
        Dim TxtHgt As Single = 0, strHeight As Single = 0
        Dim ps As Printing.PaperSize
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim W1 As Single
        Dim C1 As Single, C2 As Single
        Dim BmsInWrds As String
        Dim PpSzSTS As Boolean = False
        Dim SS1 As String = ""

        'PrintDocument pd = new PrintDocument();
        'pd.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("PaperA4", 840, 1180);
        'pd.Print();

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
            Debug.Print(ps.PaperName)
            If ps.Width = 800 And ps.Height = 600 Then
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                e.PageSettings.PaperSize = ps
                PpSzSTS = True
                Exit For
            End If
        Next

        If PpSzSTS = False Then
            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
                    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                    PrintDocument1.DefaultPageSettings.PaperSize = ps
                    e.PageSettings.PaperSize = ps
                    PpSzSTS = True
                    Exit For
                End If
            Next

            If PpSzSTS = False Then
                For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                        ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                        PrintDocument1.DefaultPageSettings.PaperSize = ps
                        e.PageSettings.PaperSize = ps
                        Exit For
                    End If
                Next
            End If

        End If

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 20 ' 65
            .Right = 30
            .Top = 40
            .Bottom = 50
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pFont = New Font("Calibri", 11, FontStyle.Regular)

        'e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        With PrintDocument1.DefaultPageSettings.PaperSize
            PrintWidth = .Width - RMargin - LMargin
            PrintHeight = .Height - TMargin - BMargin
            PageWidth = .Width - RMargin
            PageHeight = .Height - BMargin
        End With

        TxtHgt = 18 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        CurY = TMargin
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = ""

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
        Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString
        If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "PHONE NO.:" & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
            Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString) <> "" Then
            Cmp_CstNo = "CST NO.: " & prn_HdDt.Rows(0).Item("Company_CstNo").ToString
        End If

        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 18, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 15, FontStyle.Bold)

        If Common_Procedures.settings.CustomerCode = "1186" Then
            Common_Procedures.Print_To_PrintDocument(e, "EMPTY BEAM DELIVERY", LMargin, CurY, 2, PrintWidth, p1Font)
        Else
            Common_Procedures.Print_To_PrintDocument(e, "EMPTY BEAM/BAG/CONE DELIVERY", LMargin, CurY, 2, PrintWidth, p1Font)

        End If

        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


        CurY = CurY + strHeight + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "TO : ", LMargin + 10, CurY, 0, 0, pFont)

        C1 = 450
        C2 = PageWidth - (LMargin + C1)

        W1 = e.Graphics.MeasureString("DC NO : ", pFont).Width

        CurY = CurY + TxtHgt
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "     " & "M/S." & prn_HdDt.Rows(0).Item("Ledger_mAINName").ToString, LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "DC.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("JobWork_Empty_BeamBagCone_Delivery_No").ToString), LMargin + C1 + W1 + 25, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("JobWork_Empty_BeamBagCone_Delivery_Date").ToString)), LMargin + C1 + W1 + 25, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))


        ClArr(1) = Val(200) : ClArr(2) = 200
        ClArr(3) = PageWidth - (LMargin + ClArr(1) + ClArr(2))

        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "We sent your", LMargin + 20, CurY, 0, 0, pFont)



        CurY = CurY + TxtHgt
        If Trim(Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString)) <> 0 Then
            BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString))
            BmsInWrds = Replace(Trim(LCase(BmsInWrds)), "only", "")

            SS1 = Trim(Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString)) & " (" & BmsInWrds & ") empty beams "

            Common_Procedures.Print_To_PrintDocument(e, Trim(SS1), LMargin + 100, CurY, 0, 0, pFont)

        End If
        If Trim(prn_HdDt.Rows(0).Item("Del_name").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "Delivery To. : " & Trim(prn_HdDt.Rows(0).Item("Del_name").ToString), LMargin + 400, CurY, 0, 0, pFont)
        End If

        CurY = CurY + TxtHgt + 5
        If Trim(Val(prn_HdDt.Rows(0).Item("Empty_Bags").ToString)) <> 0 Then
            BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Empty_Bags").ToString))
            BmsInWrds = Replace(Trim(LCase(BmsInWrds)), "only", "")

            SS1 = Trim(Val(prn_HdDt.Rows(0).Item("Empty_Bags").ToString)) & " (" & BmsInWrds & ") empty bags "

            Common_Procedures.Print_To_PrintDocument(e, Trim(SS1), LMargin + 100, CurY, 0, 0, pFont)
        End If
        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Del_Address1").ToString), LMargin + 400, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt + 5
        If Val(prn_HdDt.Rows(0).Item("Empty_Cones").ToString) <> 0 Then
            BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Empty_cones").ToString))
            BmsInWrds = Replace(Trim(LCase(BmsInWrds)), "only", "")

            SS1 = Trim(Val(prn_HdDt.Rows(0).Item("Empty_Cones").ToString)) & " (" & BmsInWrds & ") empty cones "


            Common_Procedures.Print_To_PrintDocument(e, Trim(SS1), LMargin + 100, CurY, 0, 0, pFont)
        End If
        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Del_Address2").ToString), LMargin + 400, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt + 5
        Common_Procedures.Print_To_PrintDocument(e, "Beam Nos  :  " & Trim(prn_HdDt.Rows(0).Item("Beam_No").ToString), LMargin + 100, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Del_Address3").ToString), LMargin + 400, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt + 5

        Common_Procedures.Print_To_PrintDocument(e, "Through vehicle no. " & Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString), LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Del_Address4").ToString), LMargin + 400, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "A BEAM - PS : PICKING SIDE", LMargin + 500, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "DIRECTION : REVERSE", LMargin + 500, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "A BEAM - PS : RECEIVING SIDE", LMargin + 500, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "DIRECTION : FORWARD", LMargin + 500, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)


        If Val(Common_Procedures.User.IdNo) <> 1 Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
        End If
        If Common_Procedures.settings.CustomerCode = "1186" Then
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "for " & Cmp_Name, LMargin + PageWidth - 20, CurY, 1, 0, p1Font)

        End If
        CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 20, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 325, CurY, 0, 0, pFont)
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        If Common_Procedures.settings.CustomerCode <> "1186" Then
            Common_Procedures.Print_To_PrintDocument(e, "for " & Cmp_Name, LMargin + PageWidth - 20, CurY, 1, 0, p1Font)

        End If

        If Common_Procedures.settings.CustomerCode = "1186" Then
            Common_Procedures.Print_To_PrintDocument(e, "Authorised Signature", LMargin + PageWidth - 20, CurY, 1, 0, p1Font)

        End If

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin, LnAr(1))
        e.Graphics.DrawLine(Pens.Black, PageWidth, CurY, PageWidth, LnAr(1))

        e.HasMorePages = False

    End Sub

    Private Sub txt_BeamNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_BeamNo.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_BeamNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_BeamNo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If
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

    Private Sub Printing_Format1186(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim pFont As Font, p1Font As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single = 0
        Dim TxtHgt As Single = 0, strHeight As Single = 0
        Dim ps As Printing.PaperSize
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String, Cmp_Add3 As String, city As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_PanNo As String
        Dim Cmp_State As String, Cmp_StateCode As String, Cmp_GSTIN_No As String, Cmp_EMail As String
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim W1 As Single
        Dim C1 As Single, C2 As Single
        Dim BmsInWrds As String
        Dim PpSzSTS As Boolean = False
        Dim SS1 As String = ""
        Dim W2 As Single
        'PrintDocument pd = new PrintDocument();
        'pd.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("PaperA4", 840, 1180);
        'pd.Print();

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
            Debug.Print(ps.PaperName)
            If ps.Width = 800 And ps.Height = 600 Then
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                e.PageSettings.PaperSize = ps
                PpSzSTS = True
                Exit For
            End If
        Next

        If PpSzSTS = False Then
            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
                    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                    PrintDocument1.DefaultPageSettings.PaperSize = ps
                    e.PageSettings.PaperSize = ps
                    PpSzSTS = True
                    Exit For
                End If
            Next

            If PpSzSTS = False Then
                For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                        ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                        PrintDocument1.DefaultPageSettings.PaperSize = ps
                        e.PageSettings.PaperSize = ps
                        Exit For
                    End If
                Next
            End If

        End If

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 20 ' 65
            .Right = 30
            .Top = 40
            .Bottom = 50
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pFont = New Font("Calibri", 11, FontStyle.Regular)

        'e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        With PrintDocument1.DefaultPageSettings.PaperSize
            PrintWidth = .Width - RMargin - LMargin
            PrintHeight = .Height - TMargin - BMargin
            PageWidth = .Width - RMargin
            PageHeight = .Height - BMargin
        End With

        TxtHgt = 17 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        CurY = TMargin
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = "" : Cmp_Add3 = "" : city = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_PanNo = ""
        Cmp_State = "" : Cmp_StateCode = "" : Cmp_GSTIN_No = "" : Cmp_EMail = ""

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        Cmp_Add1 = "Regd. Off : " & prn_HdDt.Rows(0).Item("Company_Address1").ToString
        Cmp_Add2 = "Factory : " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
        Cmp_Add3 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString
        If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "PHONE NO.:" & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
            Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_PanNo").ToString) <> "" Then
            Cmp_CstNo = "PAN NO.: " & prn_HdDt.Rows(0).Item("Company_PanNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
            Cmp_EMail = "EMAIL ID : " & prn_HdDt.Rows(0).Item("Company_EMail").ToString
        End If

        If Trim(prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString) <> "" Then
            Cmp_GSTIN_No = "GSTIN :" & prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_City").ToString) <> "" Then
            City = "" & prn_HdDt.Rows(0).Item("Company_City").ToString
        End If
        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 18, FontStyle.Bold)
        e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.united_weaves_logo_png, Drawing.Image), PageWidth - 150, CurY, 120, 100)


        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin + 10, CurY, 0, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin + 10, CurY, 0, PrintWidth, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin + 10, CurY, 0, PrintWidth, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add3 & ", " & city, LMargin + 10, CurY, 0, PrintWidth, pFont)
        CurY = CurY + TxtHgt

        Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No & "  / " & Cmp_CstNo, LMargin + 10, CurY, 0, PrintWidth, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo & "  /  " & Cmp_EMail, LMargin + 10, CurY, 0, PrintWidth, pFont)
        'CurY = CurY + TxtHgt
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_EMail, LMargin + 10, CurY, 0, PrintWidth, pFont)
        CurY = CurY + TxtHgt - 12
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(15) = CurY
        CurY = CurY + TxtHgt - 12
        p1Font = New Font("Calibri", 16, FontStyle.Bold)


        Common_Procedures.Print_To_PrintDocument(e, "D.C No . : " & Trim(prn_HdDt.Rows(0).Item("JobWork_Empty_BeamBagCone_Delivery_No").ToString), LMargin + 10, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, "EMPTY BEAM DELIVERY", LMargin, CurY, 2, PrintWidth, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "D.C DATE : " & Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("JobWork_Empty_BeamBagCone_Delivery_Date").ToString)), LMargin + 610, CurY, 0, 0, pFont)


        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


        CurY = CurY + strHeight + 5
        e.Graphics.DrawLine(Pens.Black, LMargin + 150, CurY, LMargin + 150, LnAr(15))
        e.Graphics.DrawLine(Pens.Black, LMargin + 600, CurY, LMargin + 600, LnAr(15))
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY
        p1Font = New Font("Calibri", 12, FontStyle.Bold Or FontStyle.Underline Or FontStyle.Italic)
        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, " TO : ", LMargin + 10, CurY, 0, 0, p1Font)

        C1 = 450
        C2 = PageWidth - (LMargin + C1)
        Common_Procedures.Print_To_PrintDocument(e, "DELIVERY AT : ", LMargin + C1 + 10, CurY, 0, 0, p1Font)


        W1 = e.Graphics.MeasureString("DC NO : ", pFont).Width

        CurY = CurY + TxtHgt
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "     " & "M/S." & prn_HdDt.Rows(0).Item("Ledger_MAINName").ToString, LMargin + 10, CurY, 0, 0, p1Font)
        'Common_Procedures.Print_To_PrintDocument(e, "DC.NO", LMargin + C1 + 10, CurY, 0, 0, p1Font)
        'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("del_name").ToString), LMargin + C1 + 25, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt + 5
        Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("del_address1").ToString), LMargin + C1 + 25, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, p1Font)
        'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, p1Font)
        'Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("JobWork_Empty_BeamBagCone_Delivery_Date").ToString)), LMargin + C1 + W1 + 25, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("del_address2").ToString), LMargin + C1 + 25, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("del_address3").ToString), LMargin + C1 + 25, CurY, 0, 0, pFont)
        If prn_HdDt.Rows(0).Item("Ledger_Address4").ToString <> "" Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "     " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("del_address4").ToString), LMargin + C1 + 25, CurY, 0, 0, pFont)

        End If

        If prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString <> "" Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "GSTIN : " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + 10, CurY, 0, 0, pFont)

        End If
        If prn_HdDt.Rows(0).Item("DeliveryTo_LedgerGSTinNo").ToString <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "GSTIN : " & Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerGSTinNo").ToString), LMargin + C1 + 25, CurY, 0, 0, pFont)

        End If
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "STATE : " & prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "STATE : " & Trim(prn_HdDt.Rows(0).Item("DeliveryTo_State_Name").ToString), LMargin + C1 + 25, CurY, 0, 0, pFont)

        'Ledger_GSTinNo DeliveryTo_State_Name
        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))


        ClArr(1) = Val(200) : ClArr(2) = 200
        ClArr(3) = PageWidth - (LMargin + ClArr(1) + ClArr(2))

        W2 = e.Graphics.MeasureString("A BEAM - PS  : ", pFont).Width

        CurY = CurY + TxtHgt - 12
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        'If Trim(prn_HdDt.Rows(0).Item("Del_name").ToString) <> "" Then
        '    Common_Procedures.Print_To_PrintDocument(e, "Delivery At. : " & Trim(prn_HdDt.Rows(0).Item("Del_name").ToString), LMargin + 10, CurY, 0, 0, p1Font)
        'End If


        'If Trim(prn_HdDt.Rows(0).Item("Del_Address1").ToString) <> "" Then
        '    CurY = CurY + TxtHgt + 5
        '    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Del_Address1").ToString), LMargin + 10, CurY, 0, 0, p1Font)

        'End If
        'If Trim(prn_HdDt.Rows(0).Item("Del_Address2").ToString) <> "" Then
        '    CurY = CurY + TxtHgt + 5

        '    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Del_Address2").ToString), LMargin + 10, CurY, 0, 0, p1Font)

        'End If
        'If Trim(prn_HdDt.Rows(0).Item("Del_Address3").ToString) <> "" Then
        '    CurY = CurY + TxtHgt + 5

        '    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Del_Address3").ToString), LMargin + 10, CurY, 0, 0, p1Font)

        'End If
        CurY = CurY + TxtHgt - 12
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        If Trim(Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString)) <> 0 Then
            Common_Procedures.Print_To_PrintDocument(e, "No. Of Beam  ", LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, " :   " & Trim(Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString)), LMargin + W2 + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "NOTE : ", LMargin + 500, CurY, 0, 0, p1Font)
        End If
        CurY = CurY + TxtHgt + 5

        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "A BEAM - PS : PICKING SIDE", LMargin + 500, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "Beam Nos   ", LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, " :   " & Trim(prn_HdDt.Rows(0).Item("Beam_No").ToString), LMargin + W2 + 10, CurY, 0, 0, p1Font)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "Transport  ", LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, " :   " & Trim(prn_HdDt.Rows(0).Item("Transport").ToString), LMargin + W2 + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "DIRECTION   : REVERSE", LMargin + 500, CurY, 0, 0, p1Font)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "Vehicle No ", LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, " :   " & Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString), LMargin + W2 + 10, CurY, 0, 0, p1Font)

        Common_Procedures.Print_To_PrintDocument(e, "B BEAM - RS : RECEIVING SIDE", LMargin + 500, CurY, 0, 0, p1Font)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "Beam Type  ", LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, " :   " & Trim(prn_HdDt.Rows(0).Item("Beam_type").ToString), LMargin + W2 + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "DIRECTION   : FORWARD", LMargin + 500, CurY, 0, 0, p1Font)
        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        'LnAr(11) = CurY
        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))
        If Trim(prn_HdDt.Rows(0).Item("Remarks").ToString) <> "" Then
            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, " REMARKS :  " & Trim(prn_HdDt.Rows(0).Item("Remarks").ToString), LMargin + 10, CurY, 0, 0, p1Font)
            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        End If

        LnAr(11) = CurY
        If Val(Common_Procedures.User.IdNo) <> 1 Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
        End If
        If Common_Procedures.settings.CustomerCode = "1186" Then
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "for " & Cmp_Name, LMargin + PageWidth - 20, CurY, 1, 0, p1Font)

        End If
        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 20, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 300, CurY, 0, 0, pFont)
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        If Common_Procedures.settings.CustomerCode <> "1186" Then
            Common_Procedures.Print_To_PrintDocument(e, "for " & Cmp_Name, LMargin + PageWidth - 20, CurY, 1, 0, p1Font)

        End If

        If Common_Procedures.settings.CustomerCode = "1186" Then
            Common_Procedures.Print_To_PrintDocument(e, "Authorised Signatory", LMargin + PageWidth - 20, CurY, 1, 0, p1Font)

        End If

        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY
        e.Graphics.DrawLine(Pens.Black, LMargin + 250, CurY, LMargin + 250, LnAr(11))
        e.Graphics.DrawLine(Pens.Black, LMargin + 450, CurY, LMargin + 450, LnAr(11))
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin, LnAr(1))
        e.Graphics.DrawLine(Pens.Black, PageWidth, CurY, PageWidth, LnAr(1))

        e.HasMorePages = False

    End Sub

    Private Sub cbo_beam_type_GotFocus(sender As Object, e As EventArgs) Handles cbo_beam_type.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Beam_Width_head", "Beam_Width_name", "", "(Beam_Width_Idno = 0)")

    End Sub

    Private Sub cbo_beam_type_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_beam_type.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_beam_type, cbo_delivery_to, txt_BeamNo, "Beam_Width_head", "Beam_Width_name", "", "(Beam_Width_Idno = 0)")
    End Sub

    Private Sub cbo_beam_type_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_beam_type.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_beam_type, txt_BeamNo, "Beam_Width_head", "Beam_Width_name", "", "(Beam_Width_Idno = 0)")
    End Sub

    Private Sub cbo_beam_type_KeyUp(sender As Object, e As KeyEventArgs) Handles cbo_beam_type.KeyUp
        If e.Control = False And e.KeyValue = 17  Then
            Dim f As New Beam_Width_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_beam_type.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub btn_pdf_Click(sender As Object, e As EventArgs) Handles btn_pdf.Click
        PrintDocument1.DocumentName = "Empty Beam delivery"
        PrintDocument1.PrinterSettings.PrinterName = "doPDF v7"
        PrintDocument1.PrinterSettings.PrintFileName = "c:\Empty_Beam_delivery.pdf"
        PrintDocument1.Print()
    End Sub

    Private Sub cbo_partyname_GotFocus(sender As Object, e As EventArgs) Handles cbo_partyname.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_IdNo = 0 or  Ledger_Type = 'JOBWORKER')", "(Ledger_IdNo = 0)")
    End Sub

End Class