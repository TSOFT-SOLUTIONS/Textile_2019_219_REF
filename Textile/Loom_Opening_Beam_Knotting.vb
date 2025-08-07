Public Class Loom_Opening_Beam_Knotting
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "LOMOP-"
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Public Shared OpYrCode As String = ""
    Public Shared EntFnYrCode As String = ""
    Public vmskOldText As String = ""
    Public vmskSelStrt As Integer = -1

    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Private Sub clear()
        Dim dttm As Date

        New_Entry = False
        Insert_Entry = False

        pnl_back.Enabled = True
        pnl_filter.Visible = False

        vmskOldText = ""
        vmskSelStrt = -1

        lbl_RefNo.Text = ""
        lbl_RefNo.ForeColor = Color.Black
        msk_Date.Text = ""
        dtp_Date.Text = ""

        'If dtp_date.Enabled = False Then
        dttm = New DateTime(Microsoft.VisualBasic.Left(Trim(Common_Procedures.FnRange), 4), 3, 31)
        dtp_Date.Text = dttm
        msk_Date.Text = dttm
        'End If

        cbo_Shift.Text = Common_Procedures.Shift_IdNoToName(con, 1)
        cbo_PartyName.Text = ""
        cbo_PartyName.Tag = ""
        cbo_ClothName1.Text = ""
        cbo_ClothName1.Tag = ""
        cbo_ClothName2.Text = ""
        cbo_ClothName2.Tag = ""
        cbo_ClothName3.Text = ""
        cbo_ClothName3.Tag = ""
        cbo_ClothName4.Text = ""
        cbo_ClothName4.Tag = ""
        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))

        cbo_EndsCount.Text = ""

        cbo_WidthType.Text = "SINGLE"
        cbo_ClothSales_OrderNo.Text = ""
        txt_OnLoom_Fabric_Meters.Text = ""

        txt_SetNo1.Text = ""
        txt_SetNo2.Text = ""
        cbo_LoomNo.Text = ""
        cbo_LoomNo.Tag = ""
        txt_BeamNo1.Text = ""
        txt_BeamNo2.Text = ""
        txt_Meters1.Text = ""
        txt_Meters2.Text = ""


        cbo_PartyName.Enabled = True
        cbo_PartyName.BackColor = Color.White

        cbo_ClothName1.Enabled = True
        cbo_ClothName1.BackColor = Color.White

        cbo_ClothName2.Enabled = True
        cbo_ClothName2.BackColor = Color.White

        cbo_ClothName3.Enabled = True
        cbo_ClothName3.BackColor = Color.White

        cbo_ClothName4.Enabled = True
        cbo_ClothName4.BackColor = Color.White

        cbo_WidthType.Enabled = True
        cbo_WidthType.BackColor = Color.White

        cbo_EndsCount.Enabled = True
        cbo_EndsCount.BackColor = Color.White

        cbo_LoomNo.Enabled = True
        cbo_LoomNo.BackColor = Color.White

        txt_SetNo1.Enabled = True
        txt_SetNo1.BackColor = Color.White

        txt_BeamNo1.Enabled = True
        txt_BeamNo1.BackColor = Color.White

        txt_SetNo2.Enabled = True
        txt_SetNo2.BackColor = Color.White

        txt_BeamNo2.Enabled = True
        txt_BeamNo2.BackColor = Color.White

        txt_Meters1.Enabled = True
        txt_Meters1.BackColor = Color.White

        txt_Meters2.Enabled = True
        txt_Meters2.BackColor = Color.White

    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox
        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Me.ActiveControl Is Button Then
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

    Private Sub Grid_Cell_DeSelect()
        On Error Resume Next
        dgv_filter.CurrentCell.Selected = False
    End Sub

    Private Sub move_record(ByVal no As String)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String
        Dim LockSTS As Boolean = False

        If Val(no) = 0 Then Exit Sub

        clear()

        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(EntFnYrCode)

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name, c.Cloth_Name, e.Cloth_Name as Cloth_Name2, f.Cloth_Name as Cloth_Name3, g.Cloth_Name as Cloth_Name4, d.Loom_Name from Beam_Knotting_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo INNER JOIN Cloth_Head c ON a.Cloth_IdNo1 = c.Cloth_IdNo LEFT OUTER JOIN Cloth_Head e ON a.Cloth_IdNo2 = e.Cloth_IdNo LEFT OUTER JOIN Cloth_Head f ON a.Cloth_IdNo3 = f.Cloth_IdNo LEFT OUTER JOIN Cloth_Head g ON a.Cloth_IdNo4 = g.Cloth_IdNo LEFT OUTER JOIN Loom_Head d ON a.Loom_IdNo = d.Loom_IdNo Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Beam_Knotting_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_RefNo.Text = dt1.Rows(0).Item("Beam_Knotting_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Beam_Knotting_Date").ToString
                msk_Date.Text = dtp_Date.Text
                cbo_Shift.Text = dt1.Rows(0).Item("Shift").ToString
                cbo_PartyName.Text = dt1.Rows(0).Item("Ledger_Name").ToString
                cbo_ClothName1.Text = dt1.Rows(0).Item("Cloth_Name").ToString
                If dt1.Rows(0).Item("Cloth_Name2").ToString <> "" Then
                    cbo_ClothName2.Text = dt1.Rows(0).Item("Cloth_Name2").ToString
                End If
                If dt1.Rows(0).Item("Cloth_Name3").ToString <> "" Then
                    cbo_ClothName3.Text = dt1.Rows(0).Item("Cloth_Name3").ToString
                End If
                If dt1.Rows(0).Item("Cloth_Name4").ToString <> "" Then
                    cbo_ClothName4.Text = dt1.Rows(0).Item("Cloth_Name4").ToString
                End If

                cbo_EndsCount.Text = Common_Procedures.EndsCount_IdNoToName(con, dt1.Rows(0).Item("EndsCount_IdNo").ToString)
                cbo_LoomNo.Text = dt1.Rows(0).Item("Loom_Name").ToString
                cbo_WidthType.Text = dt1.Rows(0).Item("Width_Type").ToString
                cbo_ClothSales_OrderNo.Text = dt1.Rows(0).Item("ClothSales_OrderCode_forSelection").ToString
                lbl_SetCode1.Text = dt1.Rows(0).Item("Set_Code1").ToString
                lbl_SetCode2.Text = dt1.Rows(0).Item("Set_Code2").ToString
                txt_SetNo1.Text = dt1.Rows(0).Item("Set_No1").ToString
                txt_SetNo2.Text = dt1.Rows(0).Item("Set_No2").ToString
                txt_BeamNo1.Text = dt1.Rows(0).Item("Beam_No1").ToString
                txt_BeamNo2.Text = dt1.Rows(0).Item("Beam_No2").ToString
                txt_Meters1.Text = dt1.Rows(0).Item("Beam_Meters1").ToString
                txt_OnLoom_Fabric_Meters.Text = dt1.Rows(0).Item("OnLoom_Fabric_Meters").ToString

                lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))
                If Val(txt_Meters1.Text) = 0 Then
                    txt_Meters1.Text = ""
                End If
                txt_Meters2.Text = dt1.Rows(0).Item("Beam_Meters2").ToString
                If Val(txt_Meters2.Text) = 0 Then
                    txt_Meters2.Text = ""
                End If

                LockSTS = False
                If IsDBNull(dt1.Rows(0).Item("Production_Meters").ToString) = False Then
                    If Val(dt1.Rows(0).Item("Production_Meters").ToString) <> 0 Then
                        LockSTS = True
                    End If
                End If

                If IsDBNull(dt1.Rows(0).Item("Beam_RunOut_Code").ToString) = False Then
                    If Trim(dt1.Rows(0).Item("Beam_RunOut_Code").ToString) <> "" Then
                        LockSTS = True
                    End If
                End If

                If LockSTS = True Then

                    cbo_PartyName.Enabled = False
                    cbo_PartyName.BackColor = Color.LightGray

                    cbo_WidthType.Enabled = False
                    cbo_WidthType.BackColor = Color.LightGray

                    cbo_ClothName1.Enabled = False
                    cbo_ClothName1.BackColor = Color.LightGray

                    cbo_ClothName2.Enabled = False
                    cbo_ClothName2.BackColor = Color.LightGray

                    cbo_ClothName3.Enabled = False
                    cbo_ClothName3.BackColor = Color.LightGray

                    cbo_ClothName4.Enabled = False
                    cbo_ClothName4.BackColor = Color.LightGray

                    cbo_EndsCount.Enabled = False
                    cbo_EndsCount.BackColor = Color.LightGray

                    cbo_LoomNo.Enabled = False
                    cbo_LoomNo.BackColor = Color.LightGray

                    txt_SetNo1.Enabled = False
                    txt_SetNo1.BackColor = Color.LightGray

                    txt_BeamNo1.Enabled = False
                    txt_BeamNo1.BackColor = Color.LightGray

                    txt_Meters1.Enabled = False
                    txt_Meters1.BackColor = Color.LightGray

                    txt_SetNo2.Enabled = False
                    txt_SetNo2.BackColor = Color.LightGray

                    txt_BeamNo2.Enabled = False
                    txt_BeamNo2.BackColor = Color.LightGray

                    txt_Meters2.Enabled = False
                    txt_Meters2.BackColor = Color.LightGray

                End If

            End If

            dt1.Dispose()
            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            da1.Dispose()
            dt1.Dispose()

            If cbo_LoomNo.Enabled And cbo_LoomNo.Visible Then cbo_LoomNo.Focus()

        End Try

    End Sub

    Private Sub Loom_Opening_Beam_Knotting_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_PartyName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_PartyName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_ClothName1.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CLOTH" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_ClothName1.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

    Private Sub Loom_Opening_Beam_Knotting_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable


        Me.Text = ""

        con.Open()

        OpYrCode = "00-00"
        EntFnYrCode = OpYrCode
        dtp_Date.Enabled = False
        msk_Date.Enabled = False

        Da = New SqlClient.SqlDataAdapter("select Shift_Name from Shift_Head order by Shift_Name", con)
        Da.Fill(Dt1)
        cbo_Shift.DataSource = Dt1
        cbo_Shift.DisplayMember = "Shift_Name"


        cbo_WidthType.Items.Clear()
        cbo_WidthType.Items.Add("SINGLE")
        cbo_WidthType.Items.Add("DOUBLE")
        cbo_WidthType.Items.Add("TRIPLE")
        cbo_WidthType.Items.Add("FOURTH")

        pnl_filter.Visible = False
        pnl_filter.Left = (Me.Width - pnl_filter.Width) \ 2
        pnl_filter.Top = ((Me.Height - pnl_filter.Height) \ 2) + 20


        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Shift.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ClothName1.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_ClothName2.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ClothName3.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ClothName4.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_EndsCount.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_LoomNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_WidthType.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ClothSales_OrderNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_OnLoom_Fabric_Meters.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_Filter_ClothName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_LoomNo.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_SetNo1.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_SetNo2.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_BeamNo1.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_BeamNo2.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Meters1.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Meters2.GotFocus, AddressOf ControlGotFocus


        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Shift.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ClothName1.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ClothName2.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ClothName3.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ClothName4.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_SetNo1.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_SetNo2.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_BeamNo1.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_BeamNo2.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Meters1.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Meters2.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_EndsCount.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_LoomNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_WidthType.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ClothSales_OrderNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_OnLoom_Fabric_Meters.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_ClothName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_LoomNo.LostFocus, AddressOf ControlLostFocus

        AddHandler dtp_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_FilterFrom_date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_FilterTo_date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_SetNo1.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_SetNo2.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_BeamNo1.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_BeamNo2.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler msk_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Meters2.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_FilterFrom_date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_FilterTo_date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_SetNo1.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_SetNo2.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_BeamNo1.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_BeamNo2.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler msk_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Meters2.KeyPress, AddressOf TextBoxControlKeyPress


        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        dgv_filter.RowTemplate.Height = 27

        Filter_Status = False
        FrmLdSTS = True
        new_record()

    End Sub


    Private Sub Loom_Opening_Beam_Knotting_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
        con.Dispose()
    End Sub

    Private Sub Loom_Opening_Beam_Knotting_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

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
        Dim Dt2 As New DataTable
        Dim NewCode As String = ""
        Dim DelvSts As Integer = 0
        Dim Nr As Integer = 0
        Dim NoofKnotBmsInCD As Integer = 0
        Dim NoofKnotBmsInLom As Integer = 0
        Dim Lm_ID As Integer = 0

        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Loom_Creation, "~L~") = 0 And InStr(Common_Procedures.UR.Loom_Creation, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.Loom_Opening, New_Entry, Me) = False Then Exit Sub


        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(EntFnYrCode)

        Da = New SqlClient.SqlDataAdapter("select * from Beam_Knotting_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Beam_Knotting_Code = '" & Trim(NewCode) & "'", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then

            If IsDBNull(Dt1.Rows(0).Item("Production_Meters").ToString) = False Then
                If Val(Dt1.Rows(0).Item("Production_Meters").ToString) <> 0 Then
                    MessageBox.Show("Invalid : Already Production entered after this knotting", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If

            If IsDBNull(Dt1.Rows(0).Item("Beam_RunOut_Code").ToString) = False Then
                If Trim(Dt1.Rows(0).Item("Beam_RunOut_Code").ToString) <> "" Then
                    MessageBox.Show("Invalid : Already this knotting, was runout", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If

        End If
        Dt1.Clear()

        tr = con.BeginTransaction

        Try

            cmd.Connection = con
            cmd.Transaction = tr

            Lm_ID = 0
            Da = New SqlClient.SqlDataAdapter("select * from Beam_Knotting_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Beam_Knotting_Code = '" & Trim(NewCode) & "'", con)
            Da.SelectCommand.Transaction = tr
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                Lm_ID = Val(Dt1.Rows(0).Item("Loom_IdNo").ToString)

                Nr = 0
                cmd.CommandText = "Update Loom_Head Set Beam_Knotting_Code = '' Where Loom_IdNo = " & Str(Val(Dt1.Rows(0).Item("Loom_IdNo").ToString)) & " and Beam_Knotting_Code = '" & Trim(NewCode) & "'"
                Nr = cmd.ExecuteNonQuery
                'If Nr = 0 Then
                '    Throw New ApplicationException("Invalid Editing : Already this loom was knotted again")
                '    Exit Sub
                'End If

                If Trim(Dt1.Rows(0).Item("Set_Code1").ToString) <> "" And Trim(Dt1.Rows(0).Item("Beam_No1").ToString) <> "" Then
                    Nr = 0
                    cmd.CommandText = "Update Stock_SizedPavu_Processing_Details Set Beam_Knotting_Code = '', Loom_Idno = 0 From Stock_SizedPavu_Processing_Details Where Set_Code = '" & Trim(Dt1.Rows(0).Item("Set_Code1").ToString) & "' and Beam_No = '" & Trim(Dt1.Rows(0).Item("Beam_No1").ToString) & "' and Loom_Idno = " & Str(Val(Dt1.Rows(0).Item("Loom_IdNo").ToString)) & " and Beam_Knotting_Code = '" & Trim(NewCode) & "'"
                    'cmd.CommandText = "Update Stock_SizedPavu_Processing_Details Set Beam_Knotting_Code = '', Loom_Idno = 0 From Stock_SizedPavu_Processing_Details Where Set_Code = '" & Trim(Dt1.Rows(0).Item("Set_Code1").ToString) & "' and Beam_No = '" & Trim(Dt1.Rows(0).Item("Beam_No1").ToString) & "' and Loom_Idno = " & Str(Val(Dt1.Rows(0).Item("Loom_IdNo").ToString)) & " and Beam_Knotting_Code = '" & Trim(NewCode) & "' and Close_Status = 0"
                    Nr = cmd.ExecuteNonQuery
                    'If Nr = 0 Then
                    '    Throw New ApplicationException("Invalid Editing : Already this Beams is running in another loom (or) Closed")
                    '    Exit Sub
                    'End If

                End If

                If Trim(Dt1.Rows(0).Item("Set_Code2").ToString) <> "" And Trim(Dt1.Rows(0).Item("Beam_No2").ToString) <> "" Then
                    Nr = 0
                    cmd.CommandText = "Update Stock_SizedPavu_Processing_Details Set Beam_Knotting_Code = '', Loom_Idno = 0 From Stock_SizedPavu_Processing_Details Where Set_Code = '" & Trim(Dt1.Rows(0).Item("Set_Code2").ToString) & "' and Beam_No = '" & Trim(Dt1.Rows(0).Item("Beam_No2").ToString) & "' and Loom_Idno = " & Str(Val(Dt1.Rows(0).Item("Loom_IdNo").ToString)) & " and Beam_Knotting_Code = '" & Trim(NewCode) & "'"
                    'cmd.CommandText = "Update Stock_SizedPavu_Processing_Details Set Beam_Knotting_Code = '', Loom_Idno = 0 From Stock_SizedPavu_Processing_Details Where Set_Code = '" & Trim(Dt1.Rows(0).Item("Set_Code2").ToString) & "' and Beam_No = '" & Trim(Dt1.Rows(0).Item("Beam_No2").ToString) & "' and Loom_Idno = " & Str(Val(Dt1.Rows(0).Item("Loom_IdNo").ToString)) & " and Beam_Knotting_Code = '" & Trim(NewCode) & "' and Close_Status = 0"
                    Nr = cmd.ExecuteNonQuery
                    'If Nr = 0 Then
                    '    Throw New ApplicationException("Invalid Editing : Already this Beams is running in another loom (or) Closed")
                    '    Exit Sub
                    'End If
                End If

            End If
            'Dt1.Clear()


            cmd.CommandText = "delete from Beam_Knotting_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Beam_Knotting_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Delete from Stock_SizedPavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Delete from Stock_Pavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            tr.Commit()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception

            tr.Rollback()
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            If cbo_LoomNo.Enabled = True And cbo_LoomNo.Visible = True Then cbo_LoomNo.Focus()

        End Try


    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        If Filter_Status = False Then
            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable
            Dim dt2 As New DataTable
            Dim dt3 As New DataTable

            da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or Ledger_Type = 'JOBWORKER') order by Ledger_DisplayName", con)
            da.Fill(dt1)
            cbo_Filter_PartyName.DataSource = dt1
            cbo_Filter_PartyName.DisplayMember = "Ledger_DisplayName"

            da = New SqlClient.SqlDataAdapter("select Loom_Name from Loom_head order by Loom_Name", con)
            da.Fill(Dt2)
            cbo_Filter_LoomNo.DataSource = Dt2
            cbo_Filter_LoomNo.DisplayMember = "Loom_Name"

            da = New SqlClient.SqlDataAdapter("select Cloth_Name from Cloth_Head order by Cloth_Name", con)
            da.Fill(dt3)
            cbo_Filter_ClothName.DataSource = dt3
            cbo_Filter_ClothName.DisplayMember = "Cloth_Name"

            dtp_FilterFrom_date.Text = Common_Procedures.Company_FromDate
            dtp_FilterTo_date.Text = Common_Procedures.Company_ToDate
            pnl_filter.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_ClothName.SelectedIndex = -1
            cbo_Filter_LoomNo.SelectedIndex = -1
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

        If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Loom_Creation, "~L~") = 0 And InStr(Common_Procedures.UR.Loom_Creation, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub

        Try

            inpno = InputBox("Enter New Ref.No.", "FOR INSERTION...")

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(EntFnYrCode)

            cmd.Connection = con
            cmd.CommandText = "select Beam_Knotting_No from Beam_Knotting_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Beam_Knotting_Code = '" & Trim(NewCode) & "'"
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
                    MessageBox.Show("Invalid Ref.No", "DOES NOT INSERT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

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
            cmd.CommandText = "select top 1 Beam_Knotting_No from Beam_Knotting_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Beam_Knotting_Code like '%/" & Trim(EntFnYrCode) & "' Order by for_Orderby, Beam_Knotting_No"
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
            cmd.CommandText = "select top 1 Beam_Knotting_No from Beam_Knotting_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Beam_Knotting_Code like '%/" & Trim(EntFnYrCode) & "' Order by for_Orderby desc, Beam_Knotting_No desc"
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
            cmd.CommandText = "select top 1 Beam_Knotting_No from Beam_Knotting_Head where for_orderby > " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Beam_Knotting_Code like '%/" & Trim(EntFnYrCode) & "' Order by for_Orderby, Beam_Knotting_No"
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
            cmd.CommandText = "select top 1 Beam_Knotting_No from Beam_Knotting_Head where for_orderby < " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and  Beam_Knotting_Code like '%/" & Trim(EntFnYrCode) & "' Order by for_Orderby desc, Beam_Knotting_No desc"
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

            'da = New SqlClient.SqlDataAdapter("select max(for_orderby) from Beam_Knotting_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Beam_Knotting_Code like '%/" & Trim(EntFnYrCode) & "' ", con)
            'da.Fill(dt)

            'NewID = 0
            'If dt.Rows.Count > 0 Then
            '    If IsDBNull(dt.Rows(0)(0).ToString) = False Then
            '        NewID = Val(dt.Rows(0)(0).ToString)
            '    End If
            'End If

            'NewID = NewID + 1

            lbl_RefNo.Text = "NEW"   ' NewID
            lbl_RefNo.ForeColor = Color.Red


            'dtp_date.Text = Date.Today.ToShortDateString
            'da = New SqlClient.SqlDataAdapter("select top 1 * from Beam_Knotting_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Beam_Knotting_Code like '%/" & Trim(EntFnYrCode) & "' Order by for_Orderby desc, Beam_Knotting_No desc", con)
            'dt1 = New DataTable
            'da.Fill(dt1)
            'If dt1.Rows.Count > 0 Then
            '    If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)
            '        If dt1.Rows(0).Item("Beam_Knotting_Date").ToString <> "" Then dtp_date.Text = dt1.Rows(0).Item("Beam_Knotting_Date").ToString
            '    End If
            'End If
            'dt1.Clear()

            If cbo_LoomNo.Enabled And cbo_LoomNo.Visible Then cbo_LoomNo.Focus()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            dt1.Dispose()
            da.Dispose()

        End Try

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String, inpno As String
        Dim NewCode As String

        Try

            inpno = InputBox("Enter Ref.No", "FOR FINDING...")

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(EntFnYrCode)

            cmd.Connection = con
            cmd.CommandText = "select Beam_Knotting_No from Beam_Knotting_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Beam_Knotting_Code = '" & Trim(NewCode) & "'"
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
                MessageBox.Show("Ref.No. Does not exists", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim tr As SqlClient.SqlTransaction
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim NewCode As String = ""
        Dim Nr As Long = 0
        Dim Nr1 As Long = 0, Nr2 As Long = 0
        Dim led_id As Integer = 0
        Dim Clo_ID As Integer = 0
        Dim Clo_ID2 As Integer = 0
        Dim Clo_ID3 As Integer = 0
        Dim Clo_ID4 As Integer = 0
        Dim EdsCnt_ID As Integer = 0
        Dim Cnt_ID As Integer = 0
        Dim Lm_ID As Integer = 0
        Dim NoofInpBmsInLom As Integer = 0
        Dim NoofKnotBmsInCD As Integer = 0
        Dim NoofKnotBmsInLom As Integer = 0
        Dim Emp_id As Integer = 0
        Dim CR_id As Integer = 0
        Dim DR_id As Integer = 0
        Dim Selc_SetCode1 As String = "", Selc_SetCode2 As String = ""
        Dim vTotNoofBms As Single = 0
        Dim vTotPvuMtrs As Single = 0
        Dim Ledtype As String = ""
        Dim Stk_DelvIdNo As Integer = 0
        Dim Stk_RecIdNo As Integer = 0
        Dim EntID As String = ""
        Dim Partcls As String = ""
        Dim PBlNo As String = ""



        If pnl_back.Enabled = False Then
            MessageBox.Show("Close all other windows", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        ' If Common_Procedures.UserRight_Check(Common_Procedures.UR.Loom_Creation, New_Entry) = False Then Exit Sub

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Loom_Opening, New_Entry, Me) = False Then Exit Sub

        

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(msk_Date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If msk_Date.Enabled And msk_Date.Visible Then msk_Date.Focus()
            Exit Sub
        End If

        'If Not (dtp_date.Value.Date >= Common_Procedures.Company_FromDate And dtp_date.Value.Date <= Common_Procedures.Company_ToDate) Then
        '    MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '    If dtp_date.Enabled Then dtp_date.Focus()
        '    Exit Sub
        'End If

        If Trim(cbo_Shift.Text) = "" Then
            If cbo_Shift.Items.Count > 0 Then
                cbo_Shift.Text = cbo_Shift.Items(0).ToString
            End If
            'cbo_Shift.Text = Common_Procedures.Shift_IdNoToName(con, 1)
            'MessageBox.Show("Invalid Shift", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            'If cbo_Shift.Enabled Then cbo_Shift.Focus()
            'Exit Sub
        End If

        Lm_ID = Common_Procedures.Loom_NameToIdNo(con, cbo_LoomNo.Text)
        If Lm_ID = 0 Then
            MessageBox.Show("Invalid Loom No", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_LoomNo.Enabled Then cbo_LoomNo.Focus()
            Exit Sub
        End If

        led_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyName.Text)
        If led_id = 0 Then
            MessageBox.Show("Invalid Ledger Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_PartyName.Enabled Then cbo_PartyName.Focus()
            Exit Sub
        End If

        EdsCnt_ID = Common_Procedures.EndsCount_NameToIdNo(con, cbo_EndsCount.Text)
        If EdsCnt_ID = 0 Then
            MessageBox.Show("Invalid Ends/Count", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_EndsCount.Enabled Then cbo_EndsCount.Focus()
            Exit Sub
        End If

        If Trim(cbo_WidthType.Text) = "" Then
            MessageBox.Show("Invalid Width Type", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_WidthType.Enabled Then cbo_WidthType.Focus()
            Exit Sub
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1464" Then '---- MANI OMEGA FABRICS (THIRUCHENKODU)
            If cbo_ClothSales_OrderNo.Visible Then
                If Trim(cbo_ClothSales_OrderNo.Text) = "" Then
                    MessageBox.Show("Invalid Sales Order No.", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    If cbo_ClothSales_OrderNo.Enabled Then cbo_ClothSales_OrderNo.Focus()
                    Exit Sub
                End If
            End If
        End If

        Clo_ID = Common_Procedures.Cloth_NameToIdNo(con, cbo_ClothName1.Text)
        If Clo_ID = 0 Then
            MessageBox.Show("Invalid Cloth Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_ClothName1.Enabled Then cbo_ClothName1.Focus()
            Exit Sub
        End If

        If Trim(UCase(cbo_WidthType.Text)) = "DOUBLE" Or Trim(UCase(cbo_WidthType.Text)) = "TRIPLE" Or Trim(UCase(cbo_WidthType.Text)) = "FOURTH" Then
            Clo_ID2 = Common_Procedures.Cloth_NameToIdNo(con, cbo_ClothName2.Text)
            If Clo_ID2 = 0 Then
                MessageBox.Show("Invalid Cloth Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_ClothName2.Enabled Then cbo_ClothName2.Focus()
                Exit Sub
            End If
        End If

        If Trim(UCase(cbo_WidthType.Text)) = "TRIPLE" Or Trim(UCase(cbo_WidthType.Text)) = "FOURTH" Then
            Clo_ID3 = Common_Procedures.Cloth_NameToIdNo(con, cbo_ClothName3.Text)
            If Clo_ID3 = 0 Then
                MessageBox.Show("Invalid Cloth Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_ClothName3.Enabled Then cbo_ClothName3.Focus()
                Exit Sub
            End If
        End If

        If Trim(UCase(cbo_WidthType.Text)) = "FOURTH" Then
            Clo_ID4 = Common_Procedures.Cloth_NameToIdNo(con, cbo_ClothName4.Text)
            If Clo_ID4 = 0 Then
                MessageBox.Show("Invalid Cloth Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_ClothName4.Enabled Then cbo_ClothName4.Focus()
                Exit Sub
            End If
        End If

        NoofInpBmsInLom = Common_Procedures.get_FieldValue(con, "Loom_Head", "Noof_Input_Beams", "(Loom_IdNo = " & Str(Val(Lm_ID)) & ")")
        If Val(NoofInpBmsInLom) = 0 Then NoofInpBmsInLom = 1

        If NoofInpBmsInLom = 1 Then
            If Trim(txt_BeamNo1.Text) = "" Then
                MessageBox.Show("Invalid Beams", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If txt_BeamNo1.Enabled Then txt_BeamNo1.Focus()
                Exit Sub
            End If

            If Val(txt_Meters1.Text) = 0 Then
                MessageBox.Show("Invalid Beam Meters", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If txt_Meters1.Enabled Then txt_Meters1.Focus()
                Exit Sub
            End If

            If Trim(txt_BeamNo2.Text) <> "" Then
                MessageBox.Show("Invalid Beams, Select Only One Beam", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If txt_BeamNo2.Enabled Then txt_BeamNo2.Focus()
                Exit Sub
            End If

            If Val(txt_Meters2.Text) <> 0 Then
                MessageBox.Show("Invalid Beam Meters, Select Only One Beam", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If txt_Meters2.Enabled Then txt_Meters2.Focus()
                Exit Sub
            End If

        Else

            If Trim(txt_BeamNo1.Text) = "" Or Trim(txt_BeamNo2.Text) = "" Then
                MessageBox.Show("Invalid Beams", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_LoomNo.Enabled Then cbo_LoomNo.Focus()
                Exit Sub
            End If

            If Val(txt_Meters1.Text) = 0 Or Val(txt_Meters2.Text) = 0 Then
                MessageBox.Show("Invalid Beam Meters", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_LoomNo.Enabled Then cbo_LoomNo.Focus()
                Exit Sub
            End If

        End If

        lbl_SetCode1.Text = ""
        Selc_SetCode1 = ""
        If Trim(txt_SetNo1.Text) <> "" Then
            lbl_SetCode1.Text = Trim(Val(lbl_Company.Tag)) & "-" & Trim(txt_SetNo1.Text) & "/" & Trim(EntFnYrCode)
            Selc_SetCode1 = Trim(txt_SetNo1.Text) & "/" & Trim(EntFnYrCode) & "/" & Trim(Val(lbl_Company.Tag))
        End If

        lbl_SetCode2.Text = ""
        Selc_SetCode2 = ""
        If Trim(txt_SetNo2.Text) <> "" Then
            lbl_SetCode2.Text = Trim(Val(lbl_Company.Tag)) & "-" & Trim(txt_SetNo2.Text) & "/" & Trim(EntFnYrCode)
            Selc_SetCode2 = Trim(txt_SetNo2.Text) & "/" & Trim(EntFnYrCode) & "/" & Trim(Val(lbl_Company.Tag))
        End If


        tr = con.BeginTransaction

        Try


            lbl_RefNo.Text = Lm_ID

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(EntFnYrCode)

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@OpeningDate", Convert.ToDateTime(msk_Date.Text))

            If New_Entry = True Then

                cmd.CommandText = "Insert into Beam_Knotting_Head (     Beam_Knotting_Code,           Company_IdNo      ,        Beam_Knotting_No      ,                               for_OrderBy                          , Beam_Knotting_Date  ,               Shift           ,    Ledger_IdNo     ,       Cloth_Idno1  ,       Cloth_Idno2  ,      Cloth_Idno3    ,      Cloth_Idno4    ,            EndsCount_IdNo  ,       Loom_IdNo   ,              Width_Type           ,            Set_Code1              ,             Set_No1            ,             Beam_No1            ,          Beam_Meters1        ,             Set_Code2            ,             Set_No2          ,           Beam_No2                 ,           Beam_Meters2       ,     ClothSales_OrderCode_forSelection      ,                    OnLoom_Fabric_Meters         ,                      User_idNo           ) " &
                                        "      Values             ('" & Trim(NewCode) & "', " & Val(lbl_Company.Tag) & ", '" & Trim(lbl_RefNo.Text) & "', " & Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text)) & ",     @OpeningDate    , '" & Trim(cbo_Shift.Text) & "', " & Val(led_id) & ", " & Val(Clo_ID) & "," & Val(Clo_ID2) & ", " & Val(Clo_ID3) & ", " & Val(Clo_ID4) & ", " & Str(Val(EdsCnt_ID)) & ", " & Val(Lm_ID) & ", '" & Trim(cbo_WidthType.Text) & "',  '" & Trim(lbl_SetCode1.Text) & "', '" & Trim(txt_SetNo1.Text) & "', '" & Trim(txt_BeamNo1.Text) & "', " & Val(txt_Meters1.Text) & ", '" & Trim(lbl_SetCode2.Text) & "', '" & Trim(txt_SetNo2.Text) & "', '" & Trim(txt_BeamNo2.Text) & "',  " & Val(txt_Meters2.Text) & ", '" & Trim(cbo_ClothSales_OrderNo.Text) & "',  " & Str(Val(txt_OnLoom_Fabric_Meters.Text)) & ", " & Val(Common_Procedures.User.IdNo) & " ) "
                cmd.ExecuteNonQuery()

            Else

                da = New SqlClient.SqlDataAdapter("select * from Beam_Knotting_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Beam_Knotting_Code = '" & Trim(NewCode) & "'", con)
                da.SelectCommand.Transaction = tr
                dt1 = New DataTable
                da.Fill(dt1)

                If dt1.Rows.Count > 0 Then

                    If IsDBNull(dt1.Rows(0).Item("Production_Meters").ToString) = False Then
                        If Val(dt1.Rows(0).Item("Production_Meters").ToString) <> 0 Then
                            Throw New ApplicationException("Invalid Editing : Already Production entered after this knotting")
                            Exit Sub
                        End If
                    End If
                    If IsDBNull(dt1.Rows(0).Item("Beam_RunOut_Code").ToString) = False Then
                        If Trim(dt1.Rows(0).Item("Beam_RunOut_Code").ToString) <> "" Then
                            Throw New ApplicationException("Invalid Editing : Already beam runout for this knotting")
                            Exit Sub
                        End If
                    End If

                    da = New SqlClient.SqlDataAdapter("Select Close_Status from Stock_SizedPavu_Processing_Details Where Set_Code = '" & Trim(dt1.Rows(0).Item("Set_Code1").ToString) & "' and Beam_No = '" & Trim(dt1.Rows(0).Item("Beam_No1").ToString) & "'", con)
                    da.SelectCommand.Transaction = tr
                    dt2 = New DataTable
                    da.Fill(dt2)
                    If dt2.Rows.Count > 0 Then
                        If IsDBNull(dt2.Rows(0).Item("Close_Status").ToString) = False Then
                            If Val(dt2.Rows(0).Item("Close_Status").ToString) <> 0 Then
                                Throw New ApplicationException("Invalid Editing : Already this Beams was Closed")
                                Exit Sub
                            End If
                        End If
                    End If
                    dt2.Clear()

                    da = New SqlClient.SqlDataAdapter("Select Close_Status from Stock_SizedPavu_Processing_Details Where Set_Code = '" & Trim(dt1.Rows(0).Item("Set_Code2").ToString) & "' and Beam_No = '" & Trim(dt1.Rows(0).Item("Beam_No2").ToString) & "'", con)
                    da.SelectCommand.Transaction = tr
                    dt2 = New DataTable
                    da.Fill(dt2)
                    If dt2.Rows.Count > 0 Then
                        If IsDBNull(dt2.Rows(0).Item("Close_Status").ToString) = False Then
                            If Val(dt2.Rows(0).Item("Close_Status").ToString) <> 0 Then
                                Throw New ApplicationException("Invalid Editing : Already this Beams was Closed")
                                Exit Sub
                            End If
                        End If
                    End If
                    dt2.Clear()

                    Nr = 0
                    cmd.CommandText = "Update Loom_Head Set Beam_Knotting_Code = '' Where Loom_IdNo = " & Str(Val(dt1.Rows(0).Item("Loom_IdNo").ToString)) & " and Beam_Knotting_Code = '" & Trim(NewCode) & "'"
                    Nr = cmd.ExecuteNonQuery
                    'If nr = 0 Then
                    '    Throw New ApplicationException("Invalid Editing : Already this loom was knotted again")
                    '    Exit Sub
                    'End If

                End If

                cmd.CommandText = "Update Beam_Knotting_Head set Beam_Knotting_Date = @OpeningDate, Shift = '" & Trim(cbo_Shift.Text) & "', Ledger_IdNo = " & Str(Val(led_id)) & ", Cloth_Idno1 = " & Str(Val(Clo_ID)) & ",  Cloth_Idno2 = " & Str(Val(Clo_ID2)) & ",  Cloth_Idno3 = " & Str(Val(Clo_ID3)) & ", Cloth_Idno4 = " & Str(Val(Clo_ID4)) & ",  EndsCount_IdNo = " & Str(Val(EdsCnt_ID)) & ", Employee_IdNo = " & Str(Val(Emp_id)) & " ,  Loom_IdNo = " & Str(Val(Lm_ID)) & ", Width_Type = '" & Trim(cbo_WidthType.Text) & "', set_Code1 = '" & Trim(lbl_SetCode1.Text) & "', set_no1 = '" & Trim(txt_SetNo1.Text) & "', Beam_No1 = '" & Trim(txt_BeamNo1.Text) & "', Beam_Meters1 = " & Str(Val(txt_Meters1.Text)) & ", set_Code2 = '" & Trim(lbl_SetCode2.Text) & "', set_no2 = '" & Trim(txt_SetNo2.Text) & "', Beam_No2 = '" & Trim(txt_BeamNo2.Text) & "', Beam_Meters2 = " & Str(Val(txt_Meters2.Text)) & ", ClothSales_OrderCode_forSelection = '" & Trim(cbo_ClothSales_OrderNo.Text) & "', OnLoom_Fabric_Meters = " & Str(Val(txt_OnLoom_Fabric_Meters.Text)) & ", User_idNo = " & Val(Common_Procedures.User.IdNo) & "  Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Beam_Knotting_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If

            cmd.CommandText = "Delete from Stock_SizedPavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Pavu_Delivery_Code = '' and Pavu_Delivery_Increment = 0 and Production_Meters = 0 and Close_Status = 0"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Pavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()



            Nr = 0
            cmd.CommandText = "Update Loom_Head Set Beam_Knotting_Code = '" & Trim(NewCode) & "' Where Loom_Idno = " & Str(Val(Lm_ID)) & " and Beam_Knotting_Code = ''"
            nr = cmd.ExecuteNonQuery
            If nr = 0 Then
                Throw New ApplicationException("Already this Loom was knotted")
                Exit Sub
            End If

            If Trim(lbl_SetCode1.Text) <> "" And Trim(txt_BeamNo1.Text) <> "" Then

                Nr1 = 0
                cmd.CommandText = "Update Stock_SizedPavu_Processing_Details Set Reference_Date = @OpeningDate, Ledger_IdNo = " & Str(Val(led_id)) & ",  Count_IdNo = 0 , EndsCount_IdNo = " & Str(Val(EdsCnt_ID)) & ", Mill_IdNo = 0, Beam_Width_Idno = 0, Sizing_SlNo = 0,  ForOrderBy_BeamNo = " & Str(Val(Common_Procedures.OrderBy_CodeToValue(txt_BeamNo1.Text))) & ", Gross_Weight = 0, Tare_Weight = 0, Net_Weight = 0, Noof_Pcs = 0, Meters_Pc = 0 , Meters = " & Str(Val(txt_Meters1.Text)) & ", Warp_Meters = 0 " & _
                                    " Where Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Set_Code = '" & Trim(lbl_SetCode1.Text) & "' and Beam_No = '" & Trim(txt_BeamNo1.Text) & "' and StockAt_IdNo  = " & Str(Val(led_id)) & " and Loom_Idno  = " & Str(Val(Lm_ID)) & " and Beam_Knotting_Code = '" & Trim(NewCode) & "'"
                Nr1 = cmd.ExecuteNonQuery()

                Nr2 = 0
                cmd.CommandText = "Update Stock_SizedPavu_Processing_Details Set Reference_Date = @OpeningDate, Count_IdNo = 0 , Mill_IdNo = 0, Beam_Width_Idno = 0, Sizing_SlNo = 0,  ForOrderBy_BeamNo = " & Str(Val(Common_Procedures.OrderBy_CodeToValue(txt_BeamNo1.Text))) & ", Gross_Weight = 0, Tare_Weight = 0, Net_Weight = 0, Noof_Pcs = 0, Meters_Pc = 0 , Warp_Meters = 0 " & _
                                    " Where Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Set_Code = '" & Trim(lbl_SetCode1.Text) & "' and Beam_No = '" & Trim(txt_BeamNo1.Text) & "'"
                Nr2 = cmd.ExecuteNonQuery()

                If Nr1 = 0 And Nr2 = 0 Then
                    cmd.CommandText = "Insert into Stock_SizedPavu_Processing_Details ( Reference_Code        ,                 Company_IdNo     ,                  Reference_No      ,                               for_OrderBy                              , Reference_Date,           Ledger_IdNo   ,         StockAt_IdNo    ,        Set_Code                  ,               Set_No           ,        setcode_forSelection  ,  Ends_Name,  count_idno ,         EndsCount_IdNo     , Mill_IdNo, Beam_Width_Idno, Sizing_SlNo, Sl_No,               Beam_No           ,                               ForOrderBy_BeamNo                          , Gross_Weight, Tare_Weight, Net_Weight, Noof_Pcs, Meters_Pc,                  Meters            , Warp_Meters,         Loom_Idno      ,   Beam_Knotting_Code   , Production_Meters, Close_Status, Pavu_Delivery_Code, Pavu_Delivery_Increment, DeliveryTo_Name ) " & _
                                      "            Values                             ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(Val(lbl_RefNo.Text)) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",   @OpeningDate, " & Str(Val(led_id)) & ", " & Str(Val(led_id)) & ", '" & Trim(lbl_SetCode1.Text) & "', '" & Trim(txt_SetNo1.Text) & "', '" & Trim(Selc_SetCode1) & "',     ''    ,   0         , " & Str(Val(EdsCnt_ID)) & ",     0    ,     0          ,     0      ,   1  , '" & Trim(txt_BeamNo1.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(txt_BeamNo1.Text))) & ",        0    ,      0     ,     0     ,    0    ,     0    , " & Str(Val(txt_Meters1.Text)) & " ,      0     , " & Str(Val(Lm_ID)) & ", '" & Trim(NewCode) & "',        0         ,       0     ,          ''        ,       0                ,       ''       ) "
                    cmd.ExecuteNonQuery()

                End If

            End If

            If Trim(lbl_SetCode2.Text) <> "" And Trim(txt_BeamNo2.Text) <> "" Then

                Nr1 = 0
                cmd.CommandText = "Update Stock_SizedPavu_Processing_Details Set Reference_Date = @OpeningDate, Ledger_IdNo = " & Str(Val(led_id)) & ",  Count_IdNo = 0 , EndsCount_IdNo = " & Str(Val(EdsCnt_ID)) & ", Mill_IdNo = 0, Beam_Width_Idno = 0, Sizing_SlNo = 0,  ForOrderBy_BeamNo = " & Str(Val(Common_Procedures.OrderBy_CodeToValue(txt_BeamNo2.Text))) & ", Gross_Weight = 0, Tare_Weight = 0, Net_Weight = 0, Noof_Pcs = 0, Meters_Pc = 0 , Meters = " & Str(Val(txt_Meters2.Text)) & ", Warp_Meters = 0 " & _
                                    " Where Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Set_Code = '" & Trim(lbl_SetCode2.Text) & "' and Beam_No = '" & Trim(txt_BeamNo2.Text) & "' and StockAt_IdNo  = " & Str(Val(led_id)) & " and Loom_Idno  = " & Str(Val(Lm_ID)) & " and Beam_Knotting_Code = '" & Trim(NewCode) & "'"
                Nr1 = cmd.ExecuteNonQuery()

                Nr2 = 0
                cmd.CommandText = "Update Stock_SizedPavu_Processing_Details Set Reference_Date = @OpeningDate, Count_IdNo = 0 , Mill_IdNo = 0, Beam_Width_Idno = 0, Sizing_SlNo = 0,  ForOrderBy_BeamNo = " & Str(Val(Common_Procedures.OrderBy_CodeToValue(txt_BeamNo2.Text))) & ", Gross_Weight = 0, Tare_Weight = 0, Net_Weight = 0, Noof_Pcs = 0, Meters_Pc = 0 , Warp_Meters = 0 " & _
                                    " Where Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Set_Code = '" & Trim(lbl_SetCode2.Text) & "' and Beam_No = '" & Trim(txt_BeamNo2.Text) & "'"
                Nr2 = cmd.ExecuteNonQuery()

                If Nr1 = 0 And Nr2 = 0 Then
                    cmd.CommandText = "Insert into Stock_SizedPavu_Processing_Details (            Reference_Code                  ,                 Company_IdNo     ,                  Reference_No      ,                               for_OrderBy                              , Reference_Date,           Ledger_IdNo   ,         StockAt_IdNo    ,        Set_Code                  ,               Set_No           ,        setcode_forSelection  ,  Ends_Name,  count_idno ,         EndsCount_IdNo     , Mill_IdNo, Beam_Width_Idno, Sizing_SlNo, Sl_No,               Beam_No           ,                               ForOrderBy_BeamNo                          , Gross_Weight, Tare_Weight, Net_Weight, Noof_Pcs, Meters_Pc,                  Meters            , Warp_Meters,         Loom_Idno      ,   Beam_Knotting_Code   , Production_Meters, Close_Status, Pavu_Delivery_Code, Pavu_Delivery_Increment, DeliveryTo_Name ) " & _
                                      "            Values                             ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(Val(lbl_RefNo.Text)) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",   @OpeningDate, " & Str(Val(led_id)) & ", " & Str(Val(led_id)) & ", '" & Trim(lbl_SetCode2.Text) & "', '" & Trim(txt_SetNo2.Text) & "', '" & Trim(Selc_SetCode2) & "',     ''    ,   0         , " & Str(Val(EdsCnt_ID)) & ",     0    ,     0          ,     0      ,   2  , '" & Trim(txt_BeamNo2.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(txt_BeamNo2.Text))) & ",        0    ,      0     ,     0     ,    0    ,     0    , " & Str(Val(txt_Meters2.Text)) & " ,      0     , " & Str(Val(Lm_ID)) & ", '" & Trim(NewCode) & "',        0         ,       0     ,          ''        ,       0                ,       ''       ) "
                    cmd.ExecuteNonQuery()

                End If

            End If


            Ledtype = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_Type", "(Ledger_IdNo = " & Str(Val(led_id)) & ")", , tr)

            vTotPvuMtrs = Val(txt_Meters1.Text) + Val(txt_Meters2.Text)

            vTotNoofBms = 0
            If Trim(txt_SetNo1.Text) <> "" And Trim(txt_BeamNo1.Text) <> "" Then vTotNoofBms = vTotNoofBms + 1
            If Trim(txt_SetNo2.Text) <> "" And Trim(txt_BeamNo2.Text) <> "" Then vTotNoofBms = vTotNoofBms + 1

            EntID = Trim(Pk_Condition) & Trim(lbl_RefNo.Text)
            Partcls = "Loom Opening : Loom No. " & Trim(lbl_RefNo.Text)
            PBlNo = ""

            If vTotPvuMtrs <> 0 Then

                Stk_DelvIdNo = 0 : Stk_RecIdNo = 0
                If Trim(UCase(Ledtype)) = "JOBWORKER" Then
                    Stk_DelvIdNo = 0
                    Stk_RecIdNo = led_id

                Else
                    Stk_DelvIdNo = led_id
                    Stk_RecIdNo = 0

                End If

                cmd.CommandText = "Insert into Stock_Pavu_Processing_Details (                  Reference_Code            ,                Company_IdNo      ,            Reference_No       ,                               for_OrderBy                              , Reference_Date,           DeliveryTo_Idno     ,          ReceivedFrom_Idno   ,             Cloth_Idno  ,        Entry_ID      ,    Party_Bill_No     ,     Particulars        , Sl_No,         EndsCount_IdNo     ,          Sized_Beam          ,              Meters           ) " & _
                                    "          Values                        ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",   @OpeningDate  , " & Str(Val(Stk_DelvIdNo)) & ", " & Str(Val(Stk_RecIdNo)) & ", " & Str(Val(Clo_ID)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "',   1  , " & Str(Val(EdsCnt_ID)) & ", " & Str(Val(vTotNoofBms)) & ", " & Str(Val(vTotPvuMtrs)) & " ) "
                cmd.ExecuteNonQuery()

            End If


            NoofKnotBmsInCD = 0
            da = New SqlClient.SqlDataAdapter("Select count(*) from Stock_SizedPavu_Processing_Details where Beam_Knotting_Code = '" & Trim(NewCode) & "'", con)
            da.SelectCommand.Transaction = tr
            dt2 = New DataTable
            da.Fill(dt2)
            If dt2.Rows.Count > 0 Then
                If IsDBNull(dt2.Rows(0)(0).ToString) = False Then
                    NoofKnotBmsInCD = Val(dt2.Rows(0)(0).ToString)
                End If
            End If
            dt2.Clear()

            If Val(NoofKnotBmsInCD) <> Val(NoofInpBmsInLom) Then
                Throw New ApplicationException("Invalid Knotting for this Code")
                Exit Sub
            End If

            NoofKnotBmsInLom = 0
            da = New SqlClient.SqlDataAdapter("Select count(*) from Stock_SizedPavu_Processing_Details where Loom_IdNo = " & Str(Val(Lm_ID)), con)
            da.SelectCommand.Transaction = tr
            dt2 = New DataTable
            da.Fill(dt2)

            If dt2.Rows.Count > 0 Then
                If IsDBNull(dt2.Rows(0)(0).ToString) = False Then
                    NoofKnotBmsInLom = Val(dt2.Rows(0)(0).ToString)
                End If
            End If
            dt2.Clear()

            If Val(NoofKnotBmsInLom) <> Val(NoofInpBmsInLom) Then
                Throw New ApplicationException("Invalid Knotting for this Loom")
                Exit Sub
            End If


            tr.Commit()

            move_record(lbl_RefNo.Text)

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
            If InStr(1, Trim(UCase(ex.Message)), Trim(UCase("PK_SizedPavu_Processing_Details"))) > 0 Then
                MessageBox.Show("Duplicate SetNo && BeamNo ", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            ElseIf InStr(1, Trim(UCase(ex.Message)), Trim(UCase("IX_Stock_SizedPavu_Processing_Details_2"))) > 0 Then
                MessageBox.Show("Duplicate SetNo && BeamNo ", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Else
                MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            End If

        Finally
            dt1.Dispose()
            da.Dispose()
            cmd.Dispose()
            tr.Dispose()

            If msk_Date.Enabled And msk_Date.Visible Then msk_Date.Focus()

        End Try

    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        '------ No Print
    End Sub

    Private Sub cbo_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( (Ledger_Type = 'WEAVER' and Own_Loom_Status = 1) or Ledger_Type = 'GODOWN' or Ledger_Type = 'JOBWORKER' or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PartyName, cbo_LoomNo, cbo_EndsCount, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( (Ledger_Type = 'WEAVER' and Own_Loom_Status = 1) or Ledger_Type = 'GODOWN' or Ledger_Type = 'JOBWORKER' or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
        If (e.KeyValue = 38 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
            If cbo_ClothSales_OrderNo.Visible And cbo_ClothSales_OrderNo.Enabled Then
                cbo_ClothSales_OrderNo.Focus()
            Else
                cbo_LoomNo.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PartyName, cbo_EndsCount, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( (Ledger_Type = 'WEAVER' and Own_Loom_Status = 1) or Ledger_Type = 'GODOWN' or Ledger_Type = 'JOBWORKER' or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_partyname_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
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

    Private Sub cbo_ClothName1_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ClothName1.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")
    End Sub

    Private Sub cbo_ClothName1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ClothName1.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_ClothName1, cbo_WidthType, Nothing, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")
        If (e.KeyValue = 40 And cbo_ClothName1.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If cbo_ClothName2.Enabled = True Then
                cbo_ClothName2.Focus()
            Else
                txt_SetNo1.Focus()
                'If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                '    save_record()
                'Else
                '    dtp_date.Focus()
                'End If
            End If
        End If
    End Sub

    Private Sub cbo_ClothName1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_ClothName1.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_ClothName1, Nothing, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")
        If Asc(e.KeyChar) = 13 Then
            If cbo_ClothName2.Enabled = True Then
                cbo_ClothName2.Focus()

            Else
                txt_SetNo1.Focus()
                'If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                '    save_record()
                'Else
                '    dtp_date.Focus()
                'End If

            End If
        End If
    End Sub

    Private Sub cbo_ClothName2_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ClothName2.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")
    End Sub

    Private Sub cbo_ClothName2_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ClothName2.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_ClothName2, cbo_ClothName1, Nothing, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")
        If (e.KeyValue = 40 And cbo_ClothName2.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If cbo_ClothName3.Enabled = True Then
                cbo_ClothName3.Focus()
            Else
                txt_SetNo1.Focus()
                'If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                '    save_record()
                'Else
                '    dtp_date.Focus()
                'End If
            End If
        End If
    End Sub

    Private Sub cbo_ClothName2_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_ClothName2.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_ClothName2, Nothing, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")
        If Asc(e.KeyChar) = 13 Then
            If cbo_ClothName3.Enabled = True Then
                cbo_ClothName3.Focus()

            Else
                txt_SetNo1.Focus()
                'If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                '    save_record()
                'Else
                '    dtp_date.Focus()
                'End If

            End If
        End If
    End Sub

    Private Sub cbo_ClothName3_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ClothName3.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")
    End Sub

    Private Sub cbo_ClothName3_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ClothName3.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_ClothName3, cbo_ClothName2, Nothing, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")
        If (e.KeyValue = 40 And cbo_ClothName3.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If cbo_ClothName4.Enabled = True Then
                cbo_ClothName4.Focus()

            Else
                txt_SetNo1.Focus()
                'If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                '    save_record()
                'Else
                '    dtp_date.Focus()
                'End If

            End If
        End If
    End Sub

    Private Sub cbo_ClothName3_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_ClothName3.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_ClothName3, Nothing, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")
        If Asc(e.KeyChar) = 13 Then
            If cbo_ClothName4.Enabled = True Then
                cbo_ClothName4.Focus()

            Else
                txt_SetNo1.Focus()
                'If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                '    save_record()
                'Else
                '    dtp_date.Focus()
                'End If

            End If
        End If
    End Sub

    Private Sub cbo_ClothName4_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ClothName4.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")
    End Sub

    Private Sub cbo_ClothName4_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ClothName4.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_ClothName4, cbo_ClothName3, txt_SetNo1, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")
        'If (e.KeyValue = 40 And cbo_ClothName4.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
        '    btn_save.Focus()
        '    If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
        '        save_record()
        '    Else
        '        dtp_date.Focus()
        '    End If
        'End If
    End Sub

    Private Sub cbo_ClothName4_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_ClothName4.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_ClothName4, txt_SetNo1, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")
        'If Asc(e.KeyChar) = 13 Then
        '    btn_save.Focus()
        '    If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
        '        save_record()
        '    Else
        '        dtp_date.Focus()
        '    End If
        'End If
    End Sub

    Private Sub cbo_WidthType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_WidthType.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_WidthType, cbo_EndsCount, cbo_ClothName1, "", "", "", "")
    End Sub

    Private Sub cbo_widthType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_WidthType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_WidthType, cbo_ClothName1, "", "", "", "")
        If Asc(e.KeyChar) = 13 Then
            cbo_WidthType_TextChanged(sender, e)
        End If
    End Sub

    Private Sub cbo_Shift_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Shift.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Shift_Head", "Shift_Name", "", "(Shift_IdNo)")
    End Sub

    Private Sub cbo_Shift_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Shift.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Shift, msk_Date, cbo_PartyName, "Shift_Head", "Shift_Name", "", "(Shift_IdNo = 0)")
    End Sub

    Private Sub cbo_Shift_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Shift.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Shift, cbo_PartyName, "Shift_Head", "Shift_Name", "", "(Shift_IdNo)")
    End Sub



    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, cbo_Filter_ClothName, cbo_Filter_LoomNo, "Ledger_AlaisHead", "Ledger_DisplayName", " (  Ledger_Type = 'WEAVER' OR Ledger_Type = 'JOBWORKER')", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, cbo_Filter_LoomNo, "Ledger_AlaisHead", "Ledger_DisplayName", " (  Ledger_Type = 'WEAVER' OR Ledger_Type = 'JOBWORKER')", "(Ledger_idno = 0)")
    End Sub


    Private Sub btn_filtershow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_filtershow.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim n As Integer
        Dim Led_IdNo As Integer, Clt_IdNo As Integer, Lom_IdNo As Integer
        Dim Condt As String = ""

        Try

            Condt = ""
            Led_IdNo = 0
            Clt_IdNo = 0
            Lom_IdNo = 0

            If IsDate(dtp_FilterFrom_date.Value) = True And IsDate(dtp_FilterTo_date.Value) = True Then
                Condt = "a.Beam_Knotting_Date between '" & Trim(Format(dtp_FilterFrom_date.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_FilterTo_date.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_FilterFrom_date.Value) = True Then
                Condt = "a.Beam_Knotting_Date = '" & Trim(Format(dtp_FilterFrom_date.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_FilterTo_date.Value) = True Then
                Condt = "a. Beam_Knotting_Date= '" & Trim(Format(dtp_FilterTo_date.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If

            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " (a.Ledger_Idno = " & Str(Val(Led_IdNo)) & ")"
            End If

            If Trim(cbo_Filter_ClothName.Text) <> "" Then
                Clt_IdNo = Common_Procedures.Cloth_NameToIdNo(con, cbo_Filter_ClothName.Text)
            End If
            If Val(Clt_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " (a.Cloth_Idno1 = " & Str(Val(Clt_IdNo)) & " or a.Cloth_Idno2 = " & Str(Val(Clt_IdNo)) & " or a.Cloth_Idno3 = " & Str(Val(Clt_IdNo)) & " or a.Cloth_Idno4 = " & Str(Val(Clt_IdNo)) & ")"
            End If

            If Trim(cbo_Filter_LoomNo.Text) <> "" Then
                Lom_IdNo = Common_Procedures.Loom_NameToIdNo(con, cbo_Filter_LoomNo.Text)
            End If
            If Val(Lom_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " (a.Loom_Idno = " & Str(Val(Lom_IdNo)) & ")"
            End If

            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name , c.Cloth_Name,  d.Loom_Name from Beam_Knotting_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo INNER JOIN Cloth_Head c ON a.Cloth_IdNo1 = c.Cloth_IdNo LEFT OUTER JOIN Loom_Head d ON a.Loom_IdNo = d.Loom_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Beam_Knotting_Code LIKE '%/" & Trim(EntFnYrCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Beam_Knotting_No", con)
            dt2 = New DataTable
            da.Fill(dt2)

            dgv_filter.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_filter.Rows.Add()

                    dgv_filter.Rows(n).Cells(0).Value = dt2.Rows(i).Item("Beam_Knotting_No").ToString
                    dgv_filter.Rows(n).Cells(1).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Beam_Knotting_Date").ToString), "dd-MM-yyyy")
                    dgv_filter.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_filter.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Cloth_Name").ToString
                    dgv_filter.Rows(n).Cells(4).Value = dt2.Rows(i).Item("Beam_No1").ToString
                    dgv_filter.Rows(n).Cells(5).Value = dt2.Rows(i).Item("Beam_No2").ToString



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
    Private Sub cbo_Filter_ClothName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_ClothName.KeyDown

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_ClothName, dtp_FilterTo_date, cbo_Filter_PartyName, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")
    End Sub

    Private Sub cbo_Filter_ClothName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_ClothName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_ClothName, cbo_Filter_PartyName, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")
    End Sub

    Private Sub cbo_Filter_LoomNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_LoomNo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_LoomNo, cbo_Filter_PartyName, btn_filtershow, "Loom_Head", "Loom_Name", "", "(Loom_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_LoomNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_LoomNo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_LoomNo, btn_filtershow, "Loom_Head", "Loom_Name", "", "(Loom_IdNo = 0 )")
    End Sub

    Private Sub dgv_filter_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_filter.DoubleClick
        Open_FilterEntry()
    End Sub


    Private Sub btn_filtershow_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles btn_filtershow.KeyPress
        If Asc(e.KeyChar) = 13 Then
            cbo_Filter_PartyName.Focus()
        End If
    End Sub


    Private Sub btn_closefilter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
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

    Private Sub cbo_ClothName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ClothName1.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Cloth_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_ClothName1.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If

    End Sub

    Private Sub cbo_EndsCount_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_EndsCount.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")
    End Sub

    Private Sub cbo_EndsCount_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_EndsCount.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_EndsCount, cbo_PartyName, cbo_WidthType, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")
    End Sub

    Private Sub cbo_EndsCount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_EndsCount.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_EndsCount, cbo_WidthType, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0 )")
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
        cbo_LoomNo.Tag = cbo_LoomNo.Text
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Loom_Head", "Loom_Name", "( Beam_Knotting_Code = '')", "(Loom_IdNo = 0)")
    End Sub

    Private Sub cbo_LoomNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_LoomNo.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_LoomNo, Nothing, Nothing, "Loom_Head", "Loom_Name", "( Beam_Knotting_Code = '')", "(Loom_IdNo = 0)")
        If (e.KeyValue = 38 And cbo_LoomNo.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
            If txt_Meters2.Enabled And txt_Meters2.Visible Then
                txt_Meters2.Focus()
            ElseIf txt_Meters1.Enabled And txt_Meters1.Visible Then
                txt_Meters1.Focus()
            End If
        End If

        If (e.KeyValue = 40 And cbo_LoomNo.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If cbo_ClothSales_OrderNo.Enabled And cbo_ClothSales_OrderNo.Visible Then
                cbo_ClothSales_OrderNo.Focus()
            ElseIf cbo_PartyName.Enabled And cbo_PartyName.Visible Then
                cbo_PartyName.Focus()
            End If
        End If

    End Sub

    Private Sub cbo_LoomNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_LoomNo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, sender, Nothing, "Loom_Head", "Loom_Name", "( Beam_Knotting_Code = '')", "(Loom_IdNo = 0 )")
        If Asc(e.KeyChar) = 13 Then
            If Trim(cbo_LoomNo.Text) <> "" Then
                If Trim(UCase(cbo_LoomNo.Tag)) <> Trim(UCase(cbo_LoomNo.Text)) Then
                    cbo_LoomNo.Tag = cbo_LoomNo.Text
                    get_Loom_Knotting_Details()
                    Enable_Disable_BeamNo_TextBox()
                End If
            End If
            If cbo_ClothSales_OrderNo.Enabled And cbo_ClothSales_OrderNo.Visible Then
                cbo_ClothSales_OrderNo.Focus()
            ElseIf cbo_PartyName.Enabled And cbo_PartyName.Visible Then
                cbo_PartyName.Focus()
            End If
        End If
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
        If Trim(cbo_LoomNo.Text) <> "" Then
            If Trim(UCase(cbo_LoomNo.Tag)) <> Trim(UCase(cbo_LoomNo.Text)) Then
                cbo_LoomNo.Tag = cbo_LoomNo.Text
                get_Loom_Knotting_Details()
                Enable_Disable_BeamNo_TextBox()
            End If
        End If
    End Sub

    Private Sub btn_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub cbo_WidthType_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_WidthType.TextChanged
        If Trim(UCase(cbo_WidthType.Text)) = "FOURTH" Then
            cbo_ClothName1.Enabled = True
            cbo_ClothName2.Enabled = True
            cbo_ClothName3.Enabled = True
            cbo_ClothName4.Enabled = True

        ElseIf Trim(UCase(cbo_WidthType.Text)) = "TRIPLE" Then
            cbo_ClothName1.Enabled = True
            cbo_ClothName2.Enabled = True
            cbo_ClothName3.Enabled = True

            cbo_ClothName4.Text = ""
            cbo_ClothName4.Enabled = False

        ElseIf Trim(UCase(cbo_WidthType.Text)) = "DOUBLE" Then
            cbo_ClothName3.Text = ""
            cbo_ClothName4.Text = ""
            cbo_ClothName2.Enabled = True
            cbo_ClothName3.Enabled = False
            cbo_ClothName4.Enabled = False

        Else

            cbo_ClothName2.Text = ""
            cbo_ClothName3.Text = ""
            cbo_ClothName4.Text = ""
            cbo_ClothName2.Enabled = False
            cbo_ClothName3.Enabled = False
            cbo_ClothName4.Enabled = False

        End If
    End Sub


    Private Sub txt_Meters2_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Meters2.KeyDown
        'If e.KeyValue = 40 Then
        '    btn_save.Focus()
        '    If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
        '        save_record()
        '    Else
        '        msk_Date.Focus()
        '    End If
        'End If
        'If (e.KeyValue = 38) Then txt_BeamNo2.Focus()
    End Sub

    Private Sub txt_Meters2_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Meters2.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        'If Asc(e.KeyChar) = 13 Then
        '    If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
        '        save_record()
        '    Else
        '        msk_Date.Focus()
        '    End If
        'End If
    End Sub

    Private Sub get_Loom_Knotting_Details()
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim movno As String = ""
        Dim NewCode As String = ""
        Dim LmNo As String = ""
        Dim Lm_ID As Integer = 0

        LmNo = Trim(cbo_LoomNo.Text)
        Lm_ID = Common_Procedures.Loom_NameToIdNo(con, LmNo)

        If Lm_ID = 0 Then Exit Sub

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(Val(Lm_ID)) & "/" & Trim(EntFnYrCode)

        Da1 = New SqlClient.SqlDataAdapter("select Beam_Knotting_No from Beam_Knotting_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Beam_Knotting_Code = '" & Trim(NewCode) & "' and Loom_IdNo = " & Str(Val(Lm_ID)), con)
        Dt1 = New DataTable
        Da1.Fill(Dt1)

        movno = ""
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1(0)(0)) = False Then
                movno = Dt1(0)(0).ToString
            End If
        End If
        Dt1.Clear()

        Dt1.Dispose()
        Da1.Dispose()

        If Val(movno) <> 0 Then
            move_record(movno)

        Else
            new_record()

            cbo_LoomNo.Text = LmNo
            cbo_LoomNo.Tag = cbo_LoomNo.Text

        End If

    End Sub

    Private Sub Enable_Disable_BeamNo_TextBox()
        Dim NoofInpBmsInLom As Integer = 0
        Dim Lm_ID As Integer = 0

        Lm_ID = Common_Procedures.Loom_NameToIdNo(con, cbo_LoomNo.Text)

        NoofInpBmsInLom = Common_Procedures.get_FieldValue(con, "Loom_Head", "Noof_Input_Beams", "(Loom_IdNo = " & Str(Val(Lm_ID)) & ")")
        If Val(NoofInpBmsInLom) <= 0 Then NoofInpBmsInLom = 1

        If NoofInpBmsInLom >= 2 Then

            txt_SetNo1.Enabled = True
            txt_BeamNo1.Enabled = True
            txt_Meters1.Enabled = True

            txt_SetNo2.Enabled = True
            txt_BeamNo2.Enabled = True
            txt_Meters2.Enabled = True

        Else

            txt_SetNo1.Enabled = True
            txt_BeamNo1.Enabled = True
            txt_Meters1.Enabled = True

            txt_SetNo2.Enabled = False
            txt_BeamNo2.Enabled = False
            txt_Meters2.Enabled = False

            txt_SetNo2.Text = ""
            txt_BeamNo2.Text = ""
            txt_Meters2.Text = ""
            lbl_SetCode2.Text = ""

        End If

    End Sub

    Private Sub txt_Meters1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Meters1.KeyDown
        If e.KeyValue = 40 Then
            If txt_SetNo2.Enabled = True Then
                txt_SetNo2.Focus()

            Else
                btn_save.Focus()
                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    save_record()
                Else
                    msk_Date.Focus()
                End If

            End If

        End If

        If (e.KeyValue = 38) Then txt_BeamNo1.Focus()

    End Sub

    Private Sub txt_Meters1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Meters1.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            If txt_SetNo2.Enabled = True Then
                txt_SetNo2.Focus()
            Else
                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    save_record()
                Else
                    msk_Date.Focus()
                End If

            End If

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
            msk_date.Text = Date.Today
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

    Private Sub cbo_ClothSales_OrderNo_Enter(sender As Object, e As EventArgs) Handles cbo_ClothSales_OrderNo.Enter
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "ClothSales_Order_Head", "ClothSales_OrderCode_forSelection", "", "(ClothSales_OrderCode_forSelection='')")
    End Sub

    Private Sub cbo_ClothSales_OrderNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ClothSales_OrderNo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, sender, cbo_LoomNo, cbo_PartyName, "ClothSales_Order_Head", "ClothSales_OrderCode_forSelection", "", "(ClothSales_OrderCode_forSelection='')")
    End Sub

    Private Sub cbo_ClothSales_OrderNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_ClothSales_OrderNo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, sender, cbo_PartyName, "ClothSales_Order_Head", "ClothSales_OrderCode_forSelection", "", "(ClothSales_OrderCode_forSelection='')")
    End Sub

    Private Sub txt_OnLoom_Fabric_Meters_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_OnLoom_Fabric_Meters.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                msk_Date.Focus()
            End If
        End If
    End Sub

    Private Sub txt_OnLoom_Fabric_Meters_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_OnLoom_Fabric_Meters.KeyDown
        If e.KeyValue = 40 Then
            btn_save.Focus()
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                msk_Date.Focus()
            End If
        End If
        If (e.KeyValue = 38) Then txt_Meters2.Focus()
    End Sub

    Private Sub txt_SetNo2_TextChanged(sender As Object, e As EventArgs) Handles txt_SetNo2.TextChanged

    End Sub
End Class