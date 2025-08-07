Public Class Doffing_Entry_Format2
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private MovSTS As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "PCDOF-"
    Private prn_HdDt As New DataTable
    Private prn_PageNo As Integer
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double

    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl
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
        pnl_Selection.Visible = False

        vmskOldText = ""
        vmskSelStrt = -1

        lbl_RollNo.Text = ""
        lbl_RollNo.ForeColor = Color.Black
        msk_Date.Text = ""
        dtp_date.Text = ""
        lbl_Count.Text = ""
        lbl_PartyName.Text = ""
        lbl_PartyName.Tag = ""
        cbo_ClothName.Text = ""
        cbo_ClothName.Tag = ""
        lbl_WidthType.Text = ""
        lbl_EndsCount.Text = ""

        lbl_KnotNo.Text = ""
        lbl_KnotCode.Text = ""

        lbl_SetCode1.Text = ""
        lbl_SetNo1.Text = ""
        lbl_TotMtrs1.Text = ""
        lbl_TotMtrs2.Text = ""
        lbl_SetCode2.Text = ""
        lbl_SetNo2.Text = ""
        cbo_LoomNo.Text = ""
        cbo_LoomNo.Tag = ""
        lbl_BeamNo1.Text = ""
        lbl_BeamNo2.Text = ""
        lbl_BeamConsPavu.Text = ""
        lbl_BalMtrs1.Text = ""
        lbl_BalMtrs2.Text = ""
        Label1.Text = Common_Procedures.settings.ClothReceipt_LotNo_OR_RollNo_Text

        dgv_Details.Rows.Clear()
        dgv_Details_Total.Rows.Clear()
        dgv_Details_Total.Rows.Add()

        Grid_Cell_DeSelect()

        cbo_LoomNo.Enabled = True
        cbo_LoomNo.BackColor = Color.White

        cbo_ClothName.Enabled = True
        cbo_ClothName.BackColor = Color.White


    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox
        Dim msktxbx As MaskedTextBox
        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Me.ActiveControl Is MaskedTextBox Then
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

        If Me.ActiveControl.Name <> cbo_ClothName.Name Then
            cbo_ClothName.Visible = False
        End If

        If Me.ActiveControl.Name <> dgv_Details.Name Then
            Grid_Cell_DeSelect()
        End If

        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)

        On Error Resume Next

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

    Private Sub Grid_Cell_DeSelect()
        On Error Resume Next
        dgv_Details.CurrentCell.Selected = False
        dgv_Details_Total.CurrentCell.Selected = False
        dgv_filter.CurrentCell.Selected = False
    End Sub

    Private Sub move_record(ByVal no As String)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim da3 As New SqlClient.SqlDataAdapter
        Dim dt3 As New DataTable

        Dim NewCode As String
        Dim LockSTS As Boolean = False
        Dim BmRunOutCd As String = ""
        Dim SNO, N, nr As Integer

        MovSTS = True

        If Val(no) = 0 Then Exit Sub

        clear()

        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

            da1 = New SqlClient.SqlDataAdapter("Select a.*, b.Ledger_Name, c.Cloth_Name, d.EndsCount_Name, e.Count_Name, f.Loom_Name from Weaver_Cloth_Receipt_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo INNER JOIN Cloth_Head c ON a.Cloth_IdNo = c.Cloth_IdNo LEFT OUTER JOIN EndsCount_Head d ON a.EndsCount_IdNo = d.EndsCount_IdNo LEFT OUTER JOIN Count_Head e ON a.Count_IdNo = e.Count_IdNo LEFT OUTER JOIN Loom_Head f ON a.Loom_IdNo = f.Loom_IdNo Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "' and a.Receipt_Type = 'L'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then
                lbl_RollNo.Text = dt1.Rows(0).Item("Weaver_ClothReceipt_No").ToString
                dtp_date.Text = dt1.Rows(0).Item("Weaver_ClothReceipt_Date").ToString
                msk_Date.Text = dtp_Date.Text
                lbl_PartyName.Text = dt1.Rows(0).Item("Ledger_Name").ToString
                cbo_LoomNo.Text = dt1.Rows(0).Item("Loom_Name").ToString

                lbl_KnotCode.Text = dt1.Rows(0).Item("Beam_Knotting_Code").ToString
                lbl_KnotNo.Text = dt1.Rows(0).Item("Beam_Knotting_No").ToString

                cbo_ClothName.Text = dt1.Rows(0).Item("Cloth_Name").ToString
                lbl_WidthType.Text = dt1.Rows(0).Item("Width_Type").ToString
                lbl_EndsCount.Text = dt1.Rows(0).Item("EndsCount_Name").ToString
                lbl_Count.Text = dt1.Rows(0).Item("Count_Name").ToString

                lbl_SetCode1.Text = dt1.Rows(0).Item("Set_Code1").ToString
                lbl_SetNo1.Text = dt1.Rows(0).Item("Set_No1").ToString
                lbl_BeamNo1.Text = dt1.Rows(0).Item("Beam_No1").ToString
                lbl_BalMtrs1.Text = dt1.Rows(0).Item("Balance_Meters1").ToString

                lbl_TotMtrs1.Text = ""
                da2 = New SqlClient.SqlDataAdapter("Select * from Stock_SizedPavu_Processing_Details Where Set_Code = '" & Trim(lbl_SetCode1.Text) & "' and Beam_No = '" & Trim(lbl_BeamNo1.Text) & "'", con)
                dt2 = New DataTable
                da2.Fill(dt2)
                If dt2.Rows.Count > 0 Then
                    lbl_TotMtrs1.Text = dt2.Rows(0).Item("Meters").ToString
                End If
                dt2.Clear()

                lbl_SetCode2.Text = dt1.Rows(0).Item("Set_Code2").ToString
                lbl_SetNo2.Text = dt1.Rows(0).Item("Set_No2").ToString
                lbl_BeamNo2.Text = dt1.Rows(0).Item("Beam_No2").ToString
                lbl_BalMtrs2.Text = dt1.Rows(0).Item("Balance_Meters2").ToString
                lbl_TotMtrs2.Text = ""
                da2 = New SqlClient.SqlDataAdapter("Select * from Stock_SizedPavu_Processing_Details Where Set_Code = '" & Trim(lbl_SetCode2.Text) & "' and Beam_No = '" & Trim(lbl_BeamNo2.Text) & "'", con)
                dt2 = New DataTable
                da2.Fill(dt2)
                If dt2.Rows.Count > 0 Then
                    lbl_TotMtrs2.Text = dt2.Rows(0).Item("Meters").ToString
                End If
                dt2.Clear()

                da3 = New SqlClient.SqlDataAdapter("select a.* , B.Cloth_Name from Weaver_ClothReceipt_Piece_Details a  Left Outer Join Cloth_Head b ON A.Cloth_Idno = b.Cloth_Idno where a.Weaver_ClothReceipt_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)
                dt3 = New DataTable
                nr = da3.Fill(dt3)

                With dgv_Details

                    .Rows.Clear()
                    SNo = 0

                    If dt3.Rows.Count > 0 Then

                        For i = 0 To dt3.Rows.Count - 1

                            N = .Rows.Add()

                            .Rows(N).Cells(0).Value = dt3.Rows(i).Item("Piece_No").ToString
                            .Rows(N).Cells(1).Value = dt3.Rows(i).Item("Cloth_Name").ToString
                            .Rows(N).Cells(2).Value = Format(Val(dt3.Rows(i).Item("Crimp_Percentage").ToString), "########0.00")
                            .Rows(N).Cells(3).Value = Format(Val(dt3.Rows(i).Item("ReceiptMeters_Receipt").ToString), "########0.00")
                            .Rows(N).Cells(4).Value = Format(Val(dt3.Rows(i).Item("ConsumedPavu_Receipt").ToString), "########0.00")
                            .Rows(N).Cells(5).Value = Format(Val(dt3.Rows(i).Item("ConsumedYarn_Receipt").ToString), "########0.000")

                        Next i

                    End If

                    dt3.Dispose()
                    da3.Dispose()

                    N = .Rows.Count - 1
                    If (Trim(.Rows(N).Cells(1).Value) = "" And Val(.Rows(N).Cells(3).Value) <> 0) Or (.Rows(N).Cells(1).Value = Nothing And .Rows(N).Cells(3).Value = Nothing) Then
                        .Rows(N).Cells(0).Value = ""
                    End If

                End With

                With dgv_Details_Total
                    If .RowCount = 0 Then .Rows.Add()

                    .Rows(0).Cells(3).Value = Format(Val(dt1.Rows(0).Item("ReceiptMeters_Receipt").ToString), "########0.00")
                    .Rows(0).Cells(4).Value = Format(Val(dt1.Rows(0).Item("ConsumedPavu_Receipt").ToString), "########0.00")
                    .Rows(0).Cells(5).Value = Format(Val(dt1.Rows(0).Item("ConsumedYarn_Receipt").ToString), "########0.000")

                End With

                'txt_DoffMtrs.Text = dt1.Rows(0).Item("ReceiptMeters_Receipt").ToString
                'txt_CrimpPerc.Text = dt1.Rows(0).Item("Crimp_Percentage").ToString
                'lbl_ConsPavu.Text = dt1.Rows(0).Item("ConsumedPavu_Receipt").ToString
                'lbl_ConsWeftYarn.Text = dt1.Rows(0).Item("ConsumedYarn_Receipt").ToString

                lbl_BeamConsPavu.Text = dt1.Rows(0).Item("BeamConsumption_Meters").ToString

                BmRunOutCd = Common_Procedures.get_FieldValue(con, "Beam_Knotting_Head", "Beam_RunOut_Code", "(Beam_Knotting_Code = '" & Trim(lbl_KnotCode.Text) & "')")
                If BmRunOutCd <> "" Then
                    LockSTS = True
                End If

                If IsDBNull(dt1.Rows(0).Item("Weaver_Piece_Checking_Code").ToString) = False Then
                    If Trim(dt1.Rows(0).Item("Weaver_Piece_Checking_Code").ToString) <> "" Then
                        LockSTS = True
                    End If
                End If

                If LockSTS = True Then
                    cbo_LoomNo.Enabled = False
                    cbo_LoomNo.BackColor = Color.LightGray

                    cbo_ClothName.Enabled = False
                    cbo_ClothName.BackColor = Color.LightGray

                End If

            Else
                new_record()

            End If

            dt1.Dispose()
            da1.Dispose()

            Grid_Cell_DeSelect()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If msk_Date.Visible And msk_Date.Enabled Then msk_Date.Focus()
        MovSTS = False
    End Sub

    Private Sub Doffing_Entry_Format2_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(lbl_PartyName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                lbl_PartyName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_ClothName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CLOTH" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_ClothName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(lbl_EndsCount.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "ENDSCOUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                lbl_EndsCount.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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
            'MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        FrmLdSTS = False

    End Sub

    Private Sub Doffing_Entry_Format2_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim dt5 As New DataTable
        Dim dt6 As New DataTable
        Me.Text = ""
        Label1.Text = Common_Procedures.settings.ClothReceipt_LotNo_OR_RollNo_Text

        con.Open()


        Da = New SqlClient.SqlDataAdapter("select Loom_Name from Loom_head order by Loom_Name", con)
        Da.Fill(Dt2)
        cbo_LoomNo.DataSource = Dt2
        cbo_LoomNo.DisplayMember = "Loom_Name"

        'Da = New SqlClient.SqlDataAdapter("select Cloth_Name from Cloth_Head order by Cloth_Name", con)
        'Da.Fill(dt3)
        'cbo_ClothName.DataSource = dt3
        'cbo_ClothName.DisplayMember = "Cloth_Name"


        pnl_filter.Visible = False
        pnl_filter.Left = (Me.Width - pnl_filter.Width) \ 2
        pnl_filter.Top = ((Me.Height - pnl_filter.Height) \ 2) + 20


        AddHandler msk_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_date.GotFocus, AddressOf ControlGotFocus
        AddHandler lbl_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler lbl_Count.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ClothName.GotFocus, AddressOf ControlGotFocus
        AddHandler lbl_EndsCount.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_LoomNo.GotFocus, AddressOf ControlGotFocus
        AddHandler lbl_KnotNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_ClothName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_LoomNo.GotFocus, AddressOf ControlGotFocus

        AddHandler msk_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler lbl_Count.LostFocus, AddressOf ControlLostFocus
        AddHandler lbl_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ClothName.LostFocus, AddressOf ControlLostFocus
        AddHandler lbl_EndsCount.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_LoomNo.LostFocus, AddressOf ControlLostFocus
        AddHandler lbl_KnotNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_ClothName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_LoomNo.LostFocus, AddressOf ControlLostFocus

        AddHandler msk_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_FilterFrom_date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_FilterTo_date.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler msk_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_FilterFrom_date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_FilterTo_date.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_filterpono.KeyPress, AddressOf TextBoxControlKeyPress

        pnl_Selection.Visible = False
        pnl_Selection.Left = (Me.Width - pnl_Selection.Width) \ 2
        pnl_Selection.Top = (Me.Height - pnl_Selection.Height) \ 2

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        dgv_filter.RowTemplate.Height = 27

        Filter_Status = False
        FrmLdSTS = True
        new_record()

    End Sub


    Private Sub Doffing_Entry_Format2_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub Doffing_Entry_Format2_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        Try

            If Asc(e.KeyChar) = 27 Then

                If pnl_filter.Visible = True Then
                    btn_closefilter_Click(sender, e)
                    Exit Sub

                ElseIf pnl_Selection.Visible = True Then
                    btn_Close_Selection_Click(sender, e)
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
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim NewCode As String = ""
        Dim DelvSts As Integer = 0
        Dim BmRunOutCd As String

        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If
        If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Inhouse_Doffing_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Inhouse_Doffing_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub


        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RollNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Da = New SqlClient.SqlDataAdapter("select * from Weaver_Cloth_Receipt_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "'", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then

            If IsDBNull(Dt1.Rows(0).Item("Weaver_Piece_Checking_Code").ToString) = False Then
                If Trim(Dt1.Rows(0).Item("Weaver_Piece_Checking_Code").ToString) <> "" Then
                    MessageBox.Show("Already Piece Checking Prepared", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If

            BmRunOutCd = Common_Procedures.get_FieldValue(con, "Beam_Knotting_Head", "Beam_RunOut_Code", "(Beam_Knotting_Code = '" & Trim(Dt1.Rows(0).Item("Beam_Knotting_Code").ToString) & "')")
            If Trim(BmRunOutCd) <> "" Then
                MessageBox.Show("Already this knotting, was runout", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If

            Da = New SqlClient.SqlDataAdapter("Select Close_Status from Stock_SizedPavu_Processing_Details Where Set_Code = '" & Trim(Dt1.Rows(0).Item("Set_Code1").ToString) & "' and Beam_No = '" & Trim(Dt1.Rows(0).Item("Beam_No1").ToString) & "'", con)
            Dt2 = New DataTable
            Da.Fill(Dt2)
            If Dt2.Rows.Count > 0 Then
                If IsDBNull(Dt2.Rows(0).Item("Close_Status").ToString) = False Then
                    If Val(Dt2.Rows(0).Item("Close_Status").ToString) <> 0 Then
                        Throw New ApplicationException("Already this Beams was Closed")
                        Exit Sub
                    End If
                End If
            End If
            Dt2.Clear()

            Da = New SqlClient.SqlDataAdapter("Select Close_Status from Stock_SizedPavu_Processing_Details Where Set_Code = '" & Trim(Dt1.Rows(0).Item("Set_Code2").ToString) & "' and Beam_No = '" & Trim(Dt1.Rows(0).Item("Beam_No2").ToString) & "'", con)
            Dt2 = New DataTable
            Da.Fill(Dt2)
            If Dt2.Rows.Count > 0 Then
                If IsDBNull(Dt2.Rows(0).Item("Close_Status").ToString) = False Then
                    If Val(Dt2.Rows(0).Item("Close_Status").ToString) <> 0 Then
                        Throw New ApplicationException("Already this Beams was Closed")
                        Exit Sub
                    End If
                End If
            End If
            Dt2.Clear()

        End If
        Dt1.Clear()

        tr = con.BeginTransaction

        Try

            cmd.Connection = con
            cmd.Transaction = tr


            cmd.CommandText = "Update Beam_Knotting_Head set Production_Meters = a.Production_Meters - b.ReceiptMeters_Receipt from Beam_Knotting_Head a, Weaver_Cloth_Receipt_Head b where b.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and b.Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "' and a.Beam_Knotting_Code = b.Beam_Knotting_Code"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Update Stock_SizedPavu_Processing_Details set Production_Meters = a.Production_Meters - b.BeamConsumption_Meters from Stock_SizedPavu_Processing_Details a, Weaver_Cloth_Receipt_Head b where b.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and b.Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "' and a.set_code = b.Set_code1 and a.Beam_No = b.Beam_No1"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Update Stock_SizedPavu_Processing_Details set Production_Meters = a.Production_Meters - b.BeamConsumption_Meters from Stock_SizedPavu_Processing_Details a, Weaver_Cloth_Receipt_Head b where b.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and b.Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "' and a.set_code = b.Set_code2 and a.Beam_No = b.Beam_No2"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Weaver_ClothReceipt_Piece_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Piece_Checking_Code =  '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Pavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Cloth_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Weaver_Cloth_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "'"
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
            Dim dt2 As New DataTable
            Dim dt3 As New DataTable

            da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or Ledger_Type = 'GODOWN' or Ledger_Type = 'WEAVER' or Ledger_Type = 'JOBWORKER') order by Ledger_DisplayName", con)
            da.Fill(dt1)
            cbo_Filter_PartyName.DataSource = dt1
            cbo_Filter_PartyName.DisplayMember = "Ledger_DisplayName"

            da = New SqlClient.SqlDataAdapter("select Loom_Name from Loom_head order by Loom_Name", con)
            da.Fill(dt2)
            cbo_Filter_LoomNo.DataSource = dt2
            cbo_Filter_LoomNo.DisplayMember = "Loom_Name"

            da = New SqlClient.SqlDataAdapter("select Cloth_Name from Cloth_Head order by Cloth_Name", con)
            da.Fill(dt3)
            cbo_Filter_ClothName.DataSource = dt3
            cbo_Filter_ClothName.DisplayMember = "Cloth_Name"


            dtp_FilterFrom_date.Text = ""
            dtp_FilterTo_date.Text = ""
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

    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
        Dim dgv1 As New DataGridView

        On Error Resume Next

        If ActiveControl.Name = dgv_Details.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            dgv1 = Nothing

            If ActiveControl.Name = dgv_Details.Name Then
                dgv1 = dgv_Details

            ElseIf dgv_Details.IsCurrentRowDirty = True Then
                dgv1 = dgv_Details

            ElseIf Pnl_Back.Enabled = True Then
                dgv1 = dgv_Details

            End If

            If IsNothing(dgv1) = False Then

                With dgv1

                    If keyData = Keys.Enter Or keyData = Keys.Down Then
                        If .CurrentCell.ColumnIndex >= .ColumnCount - 3 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes Then
                                    save_record()
                                Else
                                    msk_Date.Focus()
                                End If

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                            End If

                        Else
                            If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then
                                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes Then
                                    save_record()
                                Else
                                    msk_Date.Focus()
                                End If


                            Else
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                            End If

                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then

                        If .CurrentCell.ColumnIndex <= 1 Then
                            If .CurrentCell.RowIndex = 0 Then
                                If cbo_LoomNo.Enabled Then cbo_LoomNo.Focus() Else cbo_LoomNo.Focus()

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 3)

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
    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String, inpno As String
        Dim NewCode As String

        If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Inhouse_Doffing_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Inhouse_Doffing_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        Try

            inpno = InputBox("Enter New Roll.No.", "FOR INSERTION...")

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.CommandText = "select Weaver_ClothReceipt_No from Weaver_Cloth_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "'"
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
                    MessageBox.Show("Invalid Roll.No", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_RollNo.Text = Trim(UCase(inpno))

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
            cmd.CommandText = "select top 1 Weaver_ClothReceipt_No from Weaver_Cloth_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Receipt_Type = 'L' Order by for_Orderby, Weaver_ClothReceipt_No"
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
            cmd.CommandText = "select top 1 Weaver_ClothReceipt_No from Weaver_Cloth_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Receipt_Type = 'L' Order by for_Orderby desc, Weaver_ClothReceipt_No desc"
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RollNo.Text))

            cmd.Connection = con
            cmd.CommandText = "select top 1 Weaver_ClothReceipt_No from Weaver_Cloth_Receipt_Head where for_orderby > " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Receipt_Type = 'L' Order by for_Orderby, Weaver_ClothReceipt_No"
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RollNo.Text))

            cmd.Connection = con
            cmd.CommandText = "select top 1 Weaver_ClothReceipt_No from Weaver_Cloth_Receipt_Head where for_orderby < " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and  Weaver_ClothReceipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Receipt_Type = 'L' Order by for_Orderby desc, Weaver_ClothReceipt_No desc"
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

            da = New SqlClient.SqlDataAdapter("select max(for_orderby) from Weaver_Cloth_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' ", con)
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


            ' dtp_date.Text = Date.Today.ToShortDateString
            da = New SqlClient.SqlDataAdapter("select top 1 * from Weaver_ClothReceipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Weaver_ClothReceipt_No desc", con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)

                    If dt.Rows(0).Item("Weaver_ClothReceipt_Date").ToString <> "" Then msk_Date.Text = dt.Rows(0).Item("Weaver_ClothReceipt_Date").ToString
                End If
            End If
            dt.Clear()
            If msk_Date.Enabled And msk_Date.Visible Then
                msk_Date.Focus()
                msk_Date.SelectionStart = 0
            End If


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

            inpno = InputBox("Enter Roll.No", "FOR FINDING...")

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.CommandText = "select Weaver_ClothReceipt_No from Weaver_Cloth_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "'"
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
                MessageBox.Show("Roll.No. Does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim tr As SqlClient.SqlTransaction
        Dim da As SqlClient.SqlDataAdapter
        Dim da1 As SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim dt5 As New DataTable
        Dim NewCode As String = ""
        Dim NewNo As Long = 0
        Dim nr As Long = 0
        Dim vTotDofMtr As Single, vTotConPav As Single, vTotConYrn As Single
        Dim led_id As Integer = 0, Delv_ID As Integer = 0, Rec_ID As Integer = 0
        Dim Clo_ID As Integer = 0
        Dim CloTH_IDNO As Integer = 0
        Dim EdsCnt_ID As Integer = 0
        Dim WftCnt_ID As Integer = 0
        Dim Lm_ID As Integer = 0
        Dim Partcls As String, PBlNo As String, EntID As String
        Dim PcsChkCode As String
        Dim PavuConsMtrs As Single = 0
        Dim NoofInpBmsInLom As Integer
        Dim Old_Loom_Idno As Integer
        Dim Old_SetCd1 As String, Old_Beam1 As String
        Dim Old_SetCd2 As String, Old_Beam2 As String
        Dim OrdByNo As Single = 0
        Dim sno As Integer
        Dim StkOf_IdNo As Integer = 0
        Dim Led_Type As String = 0

        If pnl_back.Enabled = False Then
            MessageBox.Show("Close all other windows", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Common_Procedures.UserRight_Check(Common_Procedures.UR.Inhouse_Doffing_Entry, New_Entry) = False Then Exit Sub

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(msk_Date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If msk_Date.Enabled Then msk_Date.Focus()
            Exit Sub
        End If


        If Not (Convert.ToDateTime(msk_Date.Text) >= Common_Procedures.Company_FromDate And Convert.ToDateTime(msk_Date.Text) <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If msk_Date.Enabled Then msk_Date.Focus()
            Exit Sub
        End If


        Lm_ID = Common_Procedures.Loom_NameToIdNo(con, cbo_LoomNo.Text)
        If Lm_ID = 0 Then
            MessageBox.Show("Invalid Loom No", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_LoomNo.Enabled Then cbo_LoomNo.Focus()
            Exit Sub
        End If

        led_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, lbl_PartyName.Text)
        If led_id = 0 Then
            MessageBox.Show("Invalid Ledger Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_LoomNo.Enabled Then cbo_LoomNo.Focus()
            Exit Sub
        End If

        If Trim(lbl_WidthType.Text) = "" Then
            MessageBox.Show("Invalid Width Type", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_ClothName.Enabled Then cbo_ClothName.Focus()
            Exit Sub
        End If

        EdsCnt_ID = Common_Procedures.EndsCount_NameToIdNo(con, lbl_EndsCount.Text)
        If Val(EdsCnt_ID) = 0 Then
            MessageBox.Show("Invalid Ends/Count", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_ClothName.Enabled Then cbo_ClothName.Focus()
            Exit Sub
        End If
        WftCnt_ID = Common_Procedures.Count_NameToIdNo(con, lbl_Count.Text)
        If Val(WftCnt_ID) = 0 Then
            MessageBox.Show("Invalid Weft Count", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_ClothName.Enabled Then cbo_ClothName.Focus()
            Exit Sub
        End If

        NoofInpBmsInLom = Common_Procedures.get_FieldValue(con, "Loom_Head", "Noof_Input_Beams", "(Loom_IdNo = " & Str(Val(Lm_ID)) & ")")
        If Val(NoofInpBmsInLom) = 0 Then NoofInpBmsInLom = 1

        If NoofInpBmsInLom = 1 Then
            If Trim(lbl_BeamNo1.Text) = "" And Trim(lbl_BeamNo2.Text) = "" Then
                MessageBox.Show("Invalid Beams", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                If cbo_LoomNo.Enabled Then cbo_LoomNo.Focus()
                Exit Sub
            End If

            If Val(lbl_BalMtrs1.Text) = 0 And Val(lbl_BalMtrs2.Text) = 0 Then
                MessageBox.Show("Invalid Beam Meters", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                If cbo_LoomNo.Enabled Then cbo_LoomNo.Focus()
                Exit Sub
            End If

        Else
            If Trim(lbl_BeamNo1.Text) = "" Or Trim(lbl_BeamNo2.Text) = "" Then
                MessageBox.Show("Invalid Beams", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                If cbo_LoomNo.Enabled Then cbo_LoomNo.Focus()
                Exit Sub
            End If

            If Val(lbl_BalMtrs1.Text) = 0 Or Val(lbl_BalMtrs2.Text) = 0 Then
                MessageBox.Show("Invalid Beam Meters", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                If cbo_LoomNo.Enabled Then cbo_LoomNo.Focus()
                Exit Sub
            End If

        End If

        For i = 0 To dgv_Details.RowCount - 1

            If Val(dgv_Details.Rows(i).Cells(3).Value) <> 0 Or Trim(dgv_Details.Rows(i).Cells(1).Value) <> "" Then

                CloTH_IDNO = Common_Procedures.Cloth_NameToIdNo(con, dgv_Details.Rows(i).Cells(1).Value)
                If CloTH_IDNO = 0 Then
                    MessageBox.Show("Invalid Cloth Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    If cbo_ClothName.Enabled Then cbo_ClothName.Focus()
                    Exit Sub
                End If

                If Val(dgv_Details.Rows(i).Cells(3).Value) = 0 Then
                    MessageBox.Show("Invalid Doffing Meters", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    If dgv_Details.Enabled And dgv_Details.Visible Then
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(3)
                        dgv_Details.CurrentCell.Selected = True
                    End If
                    Exit Sub

                End If

            End If

        Next
        vTotDofMtr = 0 : vTotConPav = 0 : vTotConYrn = 0

        If dgv_Details_Total.RowCount > 0 Then
            vTotDofMtr = Val(dgv_Details_Total.Rows(0).Cells(3).Value())
            vTotConPav = Val(dgv_Details_Total.Rows(0).Cells(4).Value())
            vTotConYrn = Val(dgv_Details_Total.Rows(0).Cells(5).Value())
        End If
        'If Val(txt_DoffMtrs.Text) = 0 Then
        '    MessageBox.Show("Invalid Meters", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    If txt_DoffMtrs.Enabled Then txt_DoffMtrs.Focus()
        '    Exit Sub
        'End If

        'If Val(s2d_Mtrs.GetValue) > Val(s2d_BalMtrs1.GetValue) Or Val(s2d_Mtrs.GetValue) > Val(s2d_BalMtrs2.GetValue) Then
        '    MessageBox.Show("Roll Meters greater than Beam Meters", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    If s2d_Mtrs.Enabled Then s2d_Mtrs.SetFocus()
        '    Exit Sub
        'End If

        'Call ConsumedPavu_Calculation()
        'Call ConsumedYarn_Calculation()

        PcsChkCode = ""
        Old_Loom_Idno = 0
        Old_SetCd1 = ""
        Old_Beam1 = ""
        Old_SetCd2 = ""
        Old_Beam2 = ""

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RollNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_RollNo.Text = Common_Procedures.get_MaxCode(con, "Weaver_Cloth_Receipt_Head", "Weaver_ClothReceipt_Code", "for_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RollNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            OrdByNo = Val(Common_Procedures.OrderBy_CodeToValue(lbl_RollNo.Text))

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@EntryDate", dtp_date.Value.Date)

            If New_Entry = True Then

                cmd.CommandText = "Insert into Weaver_Cloth_Receipt_Head ( Receipt_Type, Weaver_ClothReceipt_Code,           Company_IdNo      ,        Weaver_ClothReceipt_No  ,     for_OrderBy     , Weaver_ClothReceipt_Date,    Ledger_IdNo     ,       Loom_IdNo   ,             Width_Type            ,           Beam_Knotting_Code     ,       Beam_Knotting_No         ,     Cloth_Idno     ,       EndsCount_Idno  ,     Count_IdNo        ,              Beam_No1           ,              Set_Code1           ,              Set_No1           ,          Balance_Meters1      ,               Beam_No2          ,               Set_Code2          ,             Set_No2            ,            Balance_Meters2    , Folding_Receipt, Folding, Total_Receipt_Pcs, noof_pcs,      BeamConsumption_Meters      ,              Weaver_Piece_Checking_Code, Weaver_Piece_Checking_Increment , ReceiptMeters_Receipt , Receipt_Meters                , ConsumedPavu_Receipt             , Consumed_Pavu             , ConsumedYarn_Receipt         ,  Consumed_Yarn  ) " & _
                                  "            Values                    (      'L'    ,  '" & Trim(NewCode) & "', " & Val(lbl_Company.Tag) & ", '" & Trim(lbl_RollNo.Text) & "', " & Val(OrdByNo) & ",         @EntryDate      , " & Val(led_id) & ", " & Val(Lm_ID) & ", '" & Trim(lbl_WidthType.Text) & "', '" & Trim(lbl_KnotCode.Text) & "', '" & Trim(lbl_KnotNo.Text) & "', " & Val(Clo_ID) & ", " & Val(EdsCnt_ID) & ", " & Val(WftCnt_ID) & ", '" & Trim(lbl_BeamNo1.Text) & "', '" & Trim(lbl_SetCode1.Text) & "', '" & Trim(lbl_SetNo1.Text) & "', " & Val(lbl_BalMtrs1.Text) & ", '" & Trim(lbl_BeamNo2.Text) & "', '" & Trim(lbl_SetCode2.Text) & "', '" & Trim(lbl_SetNo2.Text) & "', " & Val(lbl_BalMtrs2.Text) & ",      100       ,   100  ,       1          ,    1    ,   " & Str(Val(lbl_BeamConsPavu.Text)) & ",                ''          ,             0           ,      " & Str(Val(vTotDofMtr)) & " ,  " & Str(Val(vTotDofMtr)) & ",  " & Str(Val(vTotConPav)) & "  ," & Str(Val(vTotConPav)) & "  , " & Str(Val(vTotConYrn)) & "  , " & Str(Val(vTotConYrn)) & "  ) "
                cmd.ExecuteNonQuery()

            Else

                da = New SqlClient.SqlDataAdapter("Select * from Weaver_Cloth_Receipt_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "'", con)
                da.SelectCommand.Transaction = tr
                dt1 = New DataTable
                da.Fill(dt1)
                If dt1.Rows.Count > 0 Then

                    If IsDBNull(dt1.Rows(0).Item("Weaver_Piece_Checking_Code").ToString) = False Then
                        If Trim(dt1.Rows(0).Item("Weaver_Piece_Checking_Code").ToString) <> "" Then
                            PcsChkCode = Trim(dt1.Rows(0).Item("Weaver_Piece_Checking_Code").ToString)
                            Throw New ApplicationException("Already Piece Checking Prepared")
                            Exit Sub
                        End If
                    End If

                    Old_Loom_Idno = Val(dt1.Rows(0).Item("Loom_IdNo").ToString)
                    Old_SetCd1 = dt1.Rows(0).Item("set_code1").ToString
                    Old_Beam1 = dt1.Rows(0).Item("beam_no1").ToString
                    Old_SetCd2 = dt1.Rows(0).Item("set_code2").ToString
                    Old_Beam2 = dt1.Rows(0).Item("beam_no2").ToString

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

                End If
                dt1.Clear()

                cmd.CommandText = "Update Beam_Knotting_Head set Production_Meters = a.Production_Meters - b.ReceiptMeters_Receipt from Beam_Knotting_Head a, Weaver_Cloth_Receipt_Head b where b.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and b.Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "' and a.Beam_Knotting_Code = b.Beam_Knotting_Code"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update Stock_SizedPavu_Processing_Details set Production_Meters = a.Production_Meters - b.BeamConsumption_Meters from Stock_SizedPavu_Processing_Details a, Weaver_Cloth_Receipt_Head b where b.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and b.Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "' and a.set_code = b.Set_code1 and a.Beam_No = b.Beam_No1"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update Stock_SizedPavu_Processing_Details set Production_Meters = a.Production_Meters - b.BeamConsumption_Meters from Stock_SizedPavu_Processing_Details a, Weaver_Cloth_Receipt_Head b where b.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and b.Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "' and a.set_code = b.Set_code2 and a.Beam_No = b.Beam_No2"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update Weaver_Cloth_Receipt_Head set Receipt_Type = 'L', Weaver_ClothReceipt_Date = @EntryDate, Ledger_IdNo = " & Str(Val(led_id)) & ", Loom_IdNo = " & Str(Val(Lm_ID)) & ", Width_Type = '" & Trim(lbl_WidthType.Text) & "', Beam_Knotting_Code = '" & Trim(lbl_KnotCode.Text) & "', Beam_Knotting_No = '" & Trim(lbl_KnotNo.Text) & "', Cloth_Idno = " & Str(Val(Clo_ID)) & ", EndsCount_IdNo = " & Val(EdsCnt_ID) & ", Count_IdNo = " & Val(WftCnt_ID) & ", set_Code1 = '" & Trim(lbl_SetCode1.Text) & "', set_No1 = '" & Trim(lbl_SetNo1.Text) & "', Beam_No1 = '" & Trim(lbl_BeamNo1.Text) & "', Balance_Meters1 = " & Str(Val(lbl_BalMtrs1.Text)) & ", set_Code2 = '" & Trim(lbl_SetCode2.Text) & "', set_no2 = '" & Trim(lbl_SetNo2.Text) & "', Beam_No2 = '" & Trim(lbl_BeamNo2.Text) & "', Balance_Meters2 = " & Str(Val(lbl_BalMtrs2.Text)) & ", BeamConsumption_Meters = " & Str(Val(lbl_BeamConsPavu.Text)) & "  ,  ReceiptMeters_Receipt =   " & Str(Val(vTotDofMtr)) & "    , ConsumedPavu_Receipt =  " & Str(Val(vTotConPav)) & "   ,ConsumedYarn_Receipt =  " & Str(Val(vTotConYrn)) & "  , Receipt_Meters =   " & Str(Val(vTotDofMtr)) & "    , Consumed_Pavu =  " & Str(Val(vTotConPav)) & "   ,Consumed_Yarn =  " & Str(Val(vTotConYrn)) & " Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If

            EntID = Trim(Pk_Condition) & Trim(lbl_RollNo.Text)
            Partcls = "Doff : Roll.No. " & Trim(lbl_RollNo.Text)
            PBlNo = Trim(lbl_RollNo.Text)

            led_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, lbl_PartyName.Text, tr)
            da1 = New SqlClient.SqlDataAdapter("Select a.* from Ledger_Head a Where a.Ledger_IdNo = " & Val(led_id) & "", con)
            da1.SelectCommand.Transaction = tr
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then
                Led_type = dt1.Rows(0).Item("Ledger_Type").ToString
            End If

            dt1.Dispose()
            da1.Dispose()

            stkof_idno = 0
            If Led_type = "JOBWORKER" Then
                stkof_idno = led_id
            Else
                stkof_idno = Val(Common_Procedures.CommonLedger.Godown_Ac)
            End If

            With dgv_Details
                sno = 0
                For i = 0 To .RowCount - 1
                    If Val(.Rows(i).Cells(3).Value) <> 0 Then

                        sno = sno + 1

                        CloTH_IDNO = Common_Procedures.Cloth_NameToIdNo(con, .Rows(i).Cells(1).Value, tr)
                        nr = 0
                        cmd.CommandText = "Update Weaver_ClothReceipt_Piece_Details set  Weaver_ClothReceipt_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "',  Weaver_ClothReceipt_No = '" & Trim(lbl_RollNo.Text) & "', Weaver_ClothReceipt_Date = @EntryDate , Piece_No =  '" & Trim(.Rows(i).Cells(0).Value) & "', Lot_Code = '" & Trim(NewCode) & "',Lot_No = '" & Trim(lbl_RollNo.Text) & "' , Sl_No = " & Str(Val(sno)) & ", PieceNo_OrderBy = " & Str(Val(Common_Procedures.OrderBy_CodeToValue(Val(.Rows(i).Cells(0).Value)))) & ", Cloth_IdNo = " & Str(Val(CloTH_IDNO)) & ", Crimp_Percentage = " & Str(Val(.Rows(i).Cells(2).Value)) & ",  ReceiptMeters_Receipt = " & Str(Val(.Rows(i).Cells(3).Value)) & ", ConsumedPavu_Receipt = " & Str(Val(.Rows(i).Cells(4).Value)) & ", ConsumedYarn_Receipt = " & Str(Val(.Rows(i).Cells(5).Value)) & " , Ledger_Idno =  " & Val(led_id) & " , StockOff_IdNo = " & Val(stkof_idno) & " , Create_Status = 1 where Weaver_ClothReceipt_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Piece_No = '" & Trim(.Rows(i).Cells(0).Value) & "'"
                        nr = cmd.ExecuteNonQuery()

                        If nr = 0 Then
                            cmd.CommandText = "Insert into Weaver_ClothReceipt_Piece_Details ( Weaver_ClothReceipt_Code ,             Company_IdNo         ,     Weaver_ClothReceipt_No          ,                for_orderby                                                  , Weaver_ClothReceipt_Date              ,       Piece_No                            , Lot_Code                 ,     Lot_No                      , PieceNo_OrderBy                                                                 ,  Sl_No            ,  Cloth_IdNo             ,         Crimp_Percentage                  , ReceiptMeters_Receipt                     ,      ConsumedPavu_Receipt                 ,        ConsumedYarn_Receipt           , Ledger_Idno          , StockOff_IdNo           , Create_Status ) " & _
                                                    "     Values                             (   '" & Trim(Pk_Condition) & Trim(NewCode) & "' , " & Str(Val(lbl_Company.Tag)) & ",  '" & Trim(lbl_RollNo.Text) & "',      " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RollNo.Text))) & " , @EntryDate   ,   '" & Trim(.Rows(i).Cells(0).Value) & "',  '" & Trim(NewCode) & "'   , '" & Trim(lbl_RollNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(.Rows(i).Cells(0).Value))) & " ," & Str(Val(sno)) & ", " & Str(Val(CloTH_IDNO)) & ", " & Str(Val(.Rows(i).Cells(2).Value)) & " , " & Str(Val(.Rows(i).Cells(3).Value)) & " , " & Str(Val(.Rows(i).Cells(4).Value)) & " ," & Str(Val(.Rows(i).Cells(5).Value)) & ",  " & Val(led_id) & " , " & Val(stkof_idno) & "  , 1 ) "
                            cmd.ExecuteNonQuery()
                        End If

                    End If
                Next
            End With

            nr = 0
            cmd.CommandText = "Update Beam_Knotting_Head set Production_Meters = Production_Meters + " & Str(Val(vTotDofMtr)) & " where Loom_IdNo = " & Str(Val(Lm_ID)) & " and Beam_Knotting_Code = '" & Trim(lbl_KnotCode.Text) & "' and Ledger_IdNo = " & Str(Val(led_id))
            nr = cmd.ExecuteNonQuery
            If nr = 0 Then
                Throw New ApplicationException("Mismatch of Loom Knotting && Party")
                Exit Sub
            End If

            If Trim(lbl_SetCode1.Text) <> "" And Trim(lbl_BeamNo1.Text) <> "" Then
                cmd.CommandText = "Update Stock_SizedPavu_Processing_Details set Production_Meters = Production_Meters + " & Str(Val(lbl_BeamConsPavu.Text)) & " where set_code = '" & Trim(lbl_SetCode1.Text) & "' and beam_no = '" & Trim(lbl_BeamNo1.Text) & "'"
                cmd.ExecuteNonQuery()
            End If

            If Trim(lbl_SetCode2.Text) <> "" And Trim(lbl_BeamNo2.Text) <> "" Then
                cmd.CommandText = "Update Stock_SizedPavu_Processing_Details set Production_Meters = Production_Meters + " & Str(Val(lbl_BeamConsPavu.Text)) & " where set_code = '" & Trim(lbl_SetCode2.Text) & "' and beam_no = '" & Trim(lbl_BeamNo2.Text) & "'"
                cmd.ExecuteNonQuery()
            End If

            If Trim(PcsChkCode) = "" Then

                cmd.CommandText = "Delete from Stock_Pavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Delete from Stock_Cloth_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                If Val(vTotConPav) <> 0 Then
                    cmd.CommandText = "Insert into Stock_Pavu_Processing_Details ( Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Cloth_Idno, Entry_ID, Party_Bill_No, Particulars, Sl_No, EndsCount_IdNo, Sized_Beam, Meters) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RollNo.Text) & "', " & Str(Val(OrdByNo)) & ", @EntryDate, 0, " & Str(Val(led_id)) & ", " & Str(Val(Clo_ID)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', 1, " & Str(Val(EdsCnt_ID)) & ", 0, " & Str(Val(vTotConPav)) & " )"
                    cmd.ExecuteNonQuery()
                End If

                If Val(vTotConYrn) <> 0 Then
                    cmd.CommandText = "Insert into Stock_Yarn_Processing_Details ( Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Entry_ID, Particulars, Party_Bill_No, Sl_No, Count_IdNo, Yarn_Type, Mill_IdNo, Bags, Cones, Weight) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RollNo.Text) & "', " & Str(Val(OrdByNo)) & ", @EntryDate, 0, " & Str(Val(led_id)) & ", '" & Trim(EntID) & "', '" & Trim(Partcls) & "', '" & Trim(PBlNo) & "', 1, " & Str(Val(WftCnt_ID)) & ", 'MILL', 0, 0, 0, " & Str(Val(vTotConYrn)) & " )"
                    cmd.ExecuteNonQuery()
                End If

                If Val(vTotDofMtr) <> 0 Then

                    Delv_ID = 0 : Rec_ID = 0
                    If Val(led_id) = Val(Common_Procedures.CommonLedger.Godown_Ac) Then
                        Delv_ID = Val(Common_Procedures.CommonLedger.Godown_Ac)
                        Rec_ID = 0

                    Else
                        Delv_ID = Val(Common_Procedures.CommonLedger.Godown_Ac)
                        Rec_ID = Val(led_id)

                    End If

                    cmd.CommandText = "Insert into Stock_Cloth_Processing_Details (   Reference_Code     ,             Company_IdNo         ,             Reference_No       ,     for_OrderBy          , Reference_Date,     DeliveryTo_Idno      ,       ReceivedFrom_Idno ,         Entry_ID     ,      Party_Bill_No   ,      Particulars       , Sl_No,          Cloth_Idno     , Folding,             UnChecked_Meters       ,  Meters_Type1, Meters_Type2, Meters_Type3, Meters_Type4, Meters_Type5 ,  StockOff_IdNo  ) " & _
                                                "    Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RollNo.Text) & "', " & Str(Val(OrdByNo)) & ",    @EntryDate , " & Str(Val(Delv_ID)) & ", " & Str(Val(Rec_ID)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "',   1  , " & Str(Val(Clo_ID)) & ",   100  , " & Str(Val(vTotDofMtr)) & ",       0      ,       0     ,       0     ,       0     ,       0             , " & Val(stkof_idno) & "  ) "
                    cmd.ExecuteNonQuery()

                End If

            End If


            cmd.CommandText = "Truncate Table EntryTemp"
            cmd.ExecuteNonQuery()

            If New_Entry = False Then
                '----- Editing
                cmd.CommandText = "Insert Into EntryTemp(Name1, Name2, Meters1) select Set_Code, Beam_No, Production_Meters from Stock_SizedPavu_Processing_Details where Set_Code = '" & Trim(Old_SetCd1) & "' and Beam_No = '" & Trim(Old_Beam1) & "'"
                cmd.ExecuteNonQuery()
                cmd.CommandText = "Insert Into EntryTemp(Name1, Name2, Meters1) select Set_Code, Beam_No, Production_Meters from Stock_SizedPavu_Processing_Details where Set_Code = '" & Trim(Old_SetCd2) & "' and Beam_No = '" & Trim(Old_Beam2) & "'"
                cmd.ExecuteNonQuery()
                cmd.CommandText = "Insert Into EntryTemp(Name1, Name2, Meters1) select '" & Trim(Old_SetCd1) & "', '" & Trim(Old_Beam1) & "', -1*BeamConsumption_Meters from Weaver_Cloth_Receipt_Head where (Set_Code1 = '" & Trim(Old_SetCd1) & "' and Beam_No1 = '" & Trim(Old_Beam1) & "') OR (Set_Code2 = '" & Trim(Old_SetCd1) & "' and Beam_No2 = '" & Trim(Old_Beam1) & "')"
                cmd.ExecuteNonQuery()
                cmd.CommandText = "Insert Into EntryTemp(Name1, Name2, Meters1) select '" & Trim(Old_SetCd2) & "', '" & Trim(Old_Beam2) & "', -1*BeamConsumption_Meters from Weaver_Cloth_Receipt_Head where (Set_Code1 = '" & Trim(Old_SetCd2) & "' and Beam_No1 = '" & Trim(Old_Beam2) & "') OR (Set_Code2 = '" & Trim(Old_SetCd2) & "' and Beam_No2 = '" & Trim(Old_Beam2) & "')"
                cmd.ExecuteNonQuery()
            End If

            ''----- Saving
            cmd.CommandText = "Insert Into EntryTemp(Name1, Name2, Meters1) select Set_Code, Beam_No, Production_Meters from Stock_SizedPavu_Processing_Details where Set_Code = '" & Trim(lbl_SetCode1.Text) & "' and Beam_No = '" & Trim(lbl_BeamNo1.Text) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Insert Into EntryTemp(Name1, Name2, Meters1) select Set_Code, Beam_No, Production_Meters from Stock_SizedPavu_Processing_Details where Set_Code = '" & Trim(lbl_SetCode2.Text) & "' and Beam_No = '" & Trim(lbl_BeamNo2.Text) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Insert Into EntryTemp(Name1, Name2, Meters1) select '" & Trim(lbl_SetCode1.Text) & "', '" & Trim(lbl_BeamNo1.Text) & "', -1*BeamConsumption_Meters from Weaver_Cloth_Receipt_Head where (Set_Code1 = '" & Trim(lbl_SetCode1.Text) & "' and Beam_No1 = '" & Trim(lbl_BeamNo1.Text) & "') OR (Set_Code2 = '" & Trim(lbl_SetCode1.Text) & "' and Beam_No2 = '" & Trim(lbl_BeamNo1.Text) & "')"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Insert Into EntryTemp(Name1, Name2, Meters1) select '" & Trim(lbl_SetCode2.Text) & "', '" & Trim(lbl_BeamNo2.Text) & "', -1*BeamConsumption_Meters from Weaver_Cloth_Receipt_Head where (Set_Code2 = '" & Trim(lbl_SetCode2.Text) & "' and Beam_No2 = '" & Trim(lbl_BeamNo2.Text) & "') OR (Set_Code2 = '" & Trim(lbl_SetCode2.Text) & "' and Beam_No2 = '" & Trim(lbl_BeamNo2.Text) & "')"
            cmd.ExecuteNonQuery()

            da = New SqlClient.SqlDataAdapter("select Name1, Name2, sum(Meters1) as ProdMtrs from EntryTemp Group by Name1, Name2 having sum(Meters1) <> 0 Order by Name1, Name2", con)
            da.SelectCommand.Transaction = tr
            dt2 = New DataTable
            da.Fill(dt2)
            If dt2.Rows.Count > 0 Then
                If IsDBNull(dt2.Rows(0).Item("ProdMtrs").ToString) = False Then
                    If Val(dt2.Rows(0).Item("ProdMtrs").ToString) <> 0 Then
                        Throw New ApplicationException("Invalid Editing : Mismatch of Production Meters")
                        Exit Sub
                    End If
                End If
            End If
            dt2.Clear()


            '----- Saving
            cmd.CommandText = "Truncate Table EntryTemp"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Insert Into EntryTemp(Meters1) select Production_Meters from Beam_Knotting_Head where Beam_Knotting_Code = '" & Trim(lbl_KnotCode.Text) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Insert Into EntryTemp(Meters1) select -1*ReceiptMeters_Receipt from Weaver_Cloth_Receipt_Head where  Beam_Knotting_Code = '" & Trim(lbl_KnotCode.Text) & "'"
            cmd.ExecuteNonQuery()

            da = New SqlClient.SqlDataAdapter("select sum(Meters1) as ProdMtrs from EntryTemp having sum(Meters1) <> 0", con)
            da.SelectCommand.Transaction = tr
            dt2 = New DataTable
            da.Fill(dt2)
            If dt2.Rows.Count > 0 Then
                If IsDBNull(dt2.Rows(0)(0).ToString) = False Then
                    If Val(dt2.Rows(0)(0).ToString) <> 0 Then
                        Throw New ApplicationException("Invalid Editing : Mismatch of Production Meters")
                        Exit Sub
                    End If
                End If
            End If
            dt2.Clear()

            tr.Commit()



            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)
            If Val(Common_Procedures.settings.OnSave_MoveTo_NewEntry_Status) = 1 Then
                If New_Entry = True Then
                    new_record()
                Else
                    move_record(lbl_RollNo.Text)
                End If
            Else
                move_record(lbl_RollNo.Text)
            End If

        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If msk_Date.Visible And msk_Date.Enabled Then msk_Date.Focus()

    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        '------ No Print
    End Sub
    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Details.KeyPress
        With dgv_Details
            If .Visible Then
                If .Rows.Count > 0 Then
                    If .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3 Then
                        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                            e.Handled = True
                        End If
                    End If
                End If
            End If
        End With
    End Sub

    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.Enter
        dgv_Details.EditingControl.BackColor = Color.Lime
        dgv_Details.EditingControl.ForeColor = Color.Blue
        dgtxt_Details.SelectAll()
    End Sub

    Private Sub dgv_Details_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEndEdit
        dgv_Details_CellLeave(sender, e)
    End Sub

    Private Sub dgv_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim rect As Rectangle
        Dim n As Integer = 0

        With dgv_Details
            If Trim(.CurrentRow.Cells(0).Value) = "" Then
                n = .RowCount
                If Common_Procedures.settings.ClothReceipt_PieceNo_Concept = "1,2,3" Then
                    .Rows(e.RowIndex).Cells(0).Value = Val(e.RowIndex) + 1
                Else
                    .Rows(e.RowIndex).Cells(0).Value = Chr(65 + e.RowIndex)
                End If
                '.CurrentRow.Cells(0).Value = Chr(65 + n)
                '.CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If

            If e.ColumnIndex = 1 Then

                If cbo_ClothName.Visible = False Or Val(cbo_ClothName.Tag) <> e.RowIndex Then

                    'Da = New SqlClient.SqlDataAdapter("select ClothType_Name from ClothType_Head where ClothType_IdNo Between 0 to 5 order by ClothType_Name", con)

                    cbo_ClothName.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Cloth_Name from Cloth_Head order by Cloth_Name", con)
                    Dt2 = New DataTable
                    Da.Fill(Dt2)
                    cbo_ClothName.DataSource = Dt2
                    cbo_ClothName.DisplayMember = "Cloth_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_ClothName.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_ClothName.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                    cbo_ClothName.Width = rect.Width  ' .CurrentCell.Size.Width
                    cbo_ClothName.Height = rect.Height  ' rect.Height

                    cbo_ClothName.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_ClothName.Tag = Val(e.RowIndex)
                    cbo_ClothName.Visible = True

                    cbo_ClothName.BringToFront()
                    cbo_ClothName.Focus()

                End If

            Else
                cbo_ClothName.Visible = False

            End If
        End With
    End Sub

    Private Sub dgv_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellLeave
        With dgv_Details

            If .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 4 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                End If
            End If

            If .CurrentCell.ColumnIndex = 5 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.000")
                Else
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
                End If
            End If

        End With

    End Sub

    Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged
        Try
            If FrmLdSTS = True Then Exit Sub
            If MovSTS = True Then Exit Sub

            With dgv_Details
                If .Visible Then
                    If .Rows.Count > 0 Then
                        If e.ColumnIndex = 2 Or e.ColumnIndex = 3 Then

                            ConsumedPavu_Calculation()
                            ConsumedYarn_Calculation()
                            Total_Calculation()

                        End If
                    End If
                End If
            End With

        Catch ex As Exception
            '------

        End Try
    End Sub

    Private Sub dgv_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Details.EditingControlShowing
        dgtxt_Details = CType(dgv_Details.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgv_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyUp
        Dim i As Integer
        Dim n As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            With dgv_Details
                n = .CurrentRow.Index
                If .CurrentCell.RowIndex = .Rows.Count - 1 Then
                    For i = 0 To .ColumnCount - 1
                        .Rows(n).Cells(i).Value = ""
                    Next
                Else
                    .Rows.RemoveAt(n)

                End If
                Total_Calculation()
            End With
        End If
    End Sub

    Private Sub dgv_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.LostFocus
        On Error Resume Next
        dgv_Details.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_Details_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Details.RowsAdded
        With dgv_Details
            If Common_Procedures.settings.ClothReceipt_PieceNo_Concept = "1,2,3" Then
                .Rows(e.RowIndex).Cells(0).Value = Val(e.RowIndex) + 1
            Else
                .Rows(e.RowIndex).Cells(0).Value = Chr(65 + e.RowIndex)
            End If
        End With
    End Sub

    Private Sub Total_Calculation()
        Dim Sno As Integer
        Dim TotMtrs As Single, TotConsPav As Single, TotConsYrn As Single

        Sno = -1
        TotMtrs = 0
        TotConsPav = 0
        TotConsYrn = 0

        With dgv_Details
            For i = 0 To .RowCount - 1
                Sno = Sno + 1

                If Trim(.Rows(i).Cells(1).Value) <> "" Or Val(.Rows(i).Cells(3).Value) <> 0 Then

                    TotMtrs = TotMtrs + Val(.Rows(i).Cells(3).Value)
                    TotConsPav = TotConsPav + Val(.Rows(i).Cells(4).Value)
                    TotConsYrn = TotConsYrn + Val(.Rows(i).Cells(5).Value)

                End If
            Next
        End With

        With dgv_Details_Total

            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(3).Value = Format(Val(TotMtrs), "#########0.00")
            .Rows(0).Cells(4).Value = Format(Val(TotConsPav), "#########0.00")
            .Rows(0).Cells(5).Value = Format(Val(TotConsYrn), "#########0.000")

        End With

    End Sub

    Private Sub cbo_ClothName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ClothName.GotFocus
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Lm_ID, Clth1, Clth2, Clth3 As Integer
        Dim NewCode As String = ""

        Lm_ID = Common_Procedures.Loom_NameToIdNo(con, cbo_LoomNo.Text)

        Clth1 = 0 : Clth2 = 0 : Clth3 = 0
        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RollNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Da1 = New SqlClient.SqlDataAdapter("select a.* from Beam_Knotting_Head a Where a.Loom_IdNo = " & Str(Val(Lm_ID)) & " and a.Beam_RunOut_Code = ''", con)
        Dt1 = New DataTable
        Da1.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then
            Clth1 = Dt1.Rows(0).Item("Cloth_Idno1").ToString
            Clth2 = Dt1.Rows(0).Item("Cloth_Idno2").ToString
            Clth3 = Dt1.Rows(0).Item("Cloth_Idno3").ToString
        End If
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "(Cloth_idno = " & Str(Val(Clth1)) & " or Cloth_idno = " & Str(Val(Clth2)) & " or Cloth_idno = " & Str(Val(Clth3)) & ")", "(Cloth_idno = 0)")

    End Sub
    Private Sub cbo_ClothName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ClothName.KeyDown
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Lm_ID, Clth1, Clth2, Clth3 As Integer
        Dim NewCode As String = ""

        Lm_ID = Common_Procedures.Loom_NameToIdNo(con, cbo_LoomNo.Text)

        Clth1 = 0 : Clth2 = 0 : Clth3 = 0
        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RollNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Da1 = New SqlClient.SqlDataAdapter("select a.* from Beam_Knotting_Head a Where a.Loom_IdNo = " & Str(Val(Lm_ID)) & " and a.Beam_RunOut_Code = ''", con)
        Dt1 = New DataTable
        Da1.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then
            Clth1 = Dt1.Rows(0).Item("Cloth_Idno1").ToString
            Clth2 = Dt1.Rows(0).Item("Cloth_Idno2").ToString
            Clth3 = Dt1.Rows(0).Item("Cloth_Idno3").ToString
        End If

        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_ClothName, Nothing, Nothing, "Cloth_Head", "Cloth_Name", "(Cloth_idno = " & Str(Val(Clth1)) & " or Cloth_idno = " & Str(Val(Clth2)) & " or Cloth_idno = " & Str(Val(Clth3)) & ")", "(Cloth_idno = 0)")
        With dgv_Details

            If (e.KeyValue = 38 And cbo_ClothName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                If .CurrentRow.Index <= 0 Then
                    If cbo_LoomNo.Enabled Then cbo_LoomNo.Focus() Else cbo_LoomNo.Focus()
                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.CurrentCell.ColumnIndex - 1)
                    .CurrentCell.Selected = True
                End If
            End If

            If (e.KeyValue = 40 And cbo_ClothName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                If .CurrentCell.RowIndex = .Rows.Count - 1 And Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(1).Value) = "" Then

                    If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                        save_record()
                    Else
                        msk_Date.Focus()
                    End If

                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

                End If

            End If

        End With

    End Sub

    Private Sub cbo_ClothName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_ClothName.KeyPress
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Da2 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Lm_ID, Clth1, Clth2, Clth3 As Integer
        Dim NewCode As String = ""
        Dim crimp As Single = 0
        Dim clth_idno As Integer = 0

        Lm_ID = Common_Procedures.Loom_NameToIdNo(con, cbo_LoomNo.Text)

        Clth1 = 0 : Clth2 = 0 : Clth3 = 0
        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RollNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Da1 = New SqlClient.SqlDataAdapter("select a.* from Beam_Knotting_Head a Where a.Loom_IdNo = " & Str(Val(Lm_ID)) & " and a.Beam_RunOut_Code = '' Order by a.Beam_Knotting_Date, a.for_OrderBy, a.Beam_Knotting_Code", con)
        Dt1 = New DataTable
        Da1.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then
            Clth1 = Dt1.Rows(0).Item("Cloth_Idno1").ToString
            Clth2 = Dt1.Rows(0).Item("Cloth_Idno2").ToString
            Clth3 = Dt1.Rows(0).Item("Cloth_Idno3").ToString
        End If

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_ClothName, Nothing, "Cloth_Head", "Cloth_Name", "(Cloth_idno = " & Str(Val(Clth1)) & " or Cloth_idno = " & Str(Val(Clth2)) & " or Cloth_idno = " & Str(Val(Clth3)) & ")", "(Cloth_idno = 0)")

        If Asc(e.KeyChar) = 13 Then

            With dgv_Details

                If Trim(cbo_ClothName.Text) <> "" Then

                    clth_idno = Common_Procedures.Cloth_NameToIdNo(con, Trim(cbo_ClothName.Text))

                    Da2 = New SqlClient.SqlDataAdapter("select a.* from Cloth_Head a Where a.Cloth_IdNo = " & Str(Val(clth_idno)), con)
                    Dt2 = New DataTable
                    Da2.Fill(Dt2)

                    crimp = 0
                    If Dt2.Rows.Count > 0 Then
                        If IsDBNull(Dt2.Rows(0)(0).ToString) = False Then
                            crimp = Val(Dt2.Rows(0).Item("Crimp_Percentage").ToString)
                        End If
                    End If

                    Dt2.Dispose()
                    Da2.Dispose()

                    If Val(crimp) <> 0 Then .Rows(.CurrentRow.Index).Cells(2).Value = Format(Val(crimp), "#########0.00")

                End If

                If .CurrentCell.RowIndex = .Rows.Count - 1 And Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(1).Value) = "" Then
                    If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                        save_record()
                    Else
                        msk_Date.Focus()
                    End If
                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

                End If

            End With

        End If
    End Sub


    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, cbo_Filter_ClothName, cbo_Filter_LoomNo, "Ledger_AlaisHead", "Ledger_DisplayName", " (  Ledger_Type = 'GODOWN' or Ledger_Type = 'WEAVER' OR Ledger_Type = 'JOBWORKER')", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, cbo_Filter_LoomNo, "Ledger_AlaisHead", "Ledger_DisplayName", " (  Ledger_Type = 'GODOWN' or Ledger_Type = 'WEAVER' OR Ledger_Type = 'JOBWORKER')", "(Ledger_idno = 0)")
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
                Condt = "a.Weaver_ClothReceipt_Date between '" & Trim(Format(dtp_FilterFrom_date.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_FilterTo_date.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_FilterFrom_date.Value) = True Then
                Condt = "a.Weaver_ClothReceipt_Date = '" & Trim(Format(dtp_FilterFrom_date.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_FilterTo_date.Value) = True Then
                Condt = "a. Weaver_ClothReceipt_Date= '" & Trim(Format(dtp_FilterTo_date.Value, "MM/dd/yyyy")) & "' "
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
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " (a.Cloth_Idno = " & Str(Val(Clt_IdNo)) & ")"
            End If

            If Trim(cbo_Filter_LoomNo.Text) <> "" Then
                Lom_IdNo = Common_Procedures.Loom_NameToIdNo(con, cbo_Filter_LoomNo.Text)
            End If
            If Val(Lom_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " (a.Loom_Idno = " & Str(Val(Lom_IdNo)) & ")"
            End If

            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name , c.Cloth_Name,  d.Loom_Name from Weaver_Cloth_Receipt_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo INNER JOIN Cloth_Head c ON a.Cloth_IdNo = c.Cloth_IdNo   LEFT OUTER JOIN Loom_Head d ON a.Loom_IdNo = d.Loom_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_ClothReceipt_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Weaver_ClothReceipt_No", con)
            dt2 = New DataTable
            da.Fill(dt2)

            dgv_filter.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_filter.Rows.Add()

                    dgv_filter.Rows(n).Cells(0).Value = dt2.Rows(i).Item("Weaver_ClothReceipt_No").ToString
                    dgv_filter.Rows(n).Cells(1).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Weaver_ClothReceipt_Date").ToString), "dd-MM-yyyy")
                    dgv_filter.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_filter.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Cloth_Name").ToString
                    dgv_filter.Rows(n).Cells(4).Value = dt2.Rows(i).Item("Loom_Name").ToString
                    dgv_filter.Rows(n).Cells(5).Value = dt2.Rows(i).Item("Beam_No1").ToString
                    dgv_filter.Rows(n).Cells(6).Value = dt2.Rows(i).Item("Beam_No2").ToString



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
        'If Asc(e.KeyChar) = 13 Then
        '    If Trim(UCase(cbo_LoomNo.Text)) = "" Then
        '        If MessageBox.Show("Do you want to select  :", "FOR  SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
        '            btn_Selection_Click(sender, e)
        '        Else
        '            cbo_WidthType.Focus()
        '        End If

        '    Else
        '        cbo_WidthType.Focus()

        '    End If

        'End If

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

    Private Sub cbo_LoomNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_LoomNo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Loom_Head", "Loom_Name", "", "(Loom_IdNo = 0 )")
    End Sub

    Private Sub cbo_LoomNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_LoomNo.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_LoomNo, msk_Date, cbo_ClothName, "Loom_Head", "Loom_Name", "", "(Loom_IdNo = 0)")
    End Sub

    Private Sub cbo_LoomNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_LoomNo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_LoomNo, Nothing, "Loom_Head", "Loom_Name", "", "(Loom_IdNo = 0 )")
        If Asc(e.KeyChar) = 13 Then
            If Trim(cbo_LoomNo.Text) <> "" And (Trim(UCase(cbo_LoomNo.Text)) <> Trim(UCase(cbo_LoomNo.Tag)) Or Trim(lbl_KnotCode.Text) = "") Then
                btn_Selection_Click(sender, e)
            End If
            If cbo_ClothName.Enabled And cbo_ClothName.Visible Then cbo_ClothName.Focus()
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

    Private Sub btn_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub cbo_Count_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Count_Creation
            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = lbl_Count.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""
            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub ConsumedPavu_Calculation()
        Dim CloID As Integer
        Dim ConsPavu As Single
        Dim LmID As Integer
        Dim NoofBeams As Integer = 0

        CloID = Common_Procedures.Cloth_NameToIdNo(con, cbo_ClothName.Text)
        LmID = Common_Procedures.Loom_NameToIdNo(con, cbo_LoomNo.Text)

        ConsPavu = Common_Procedures.get_Pavu_Consumption(con, CloID, LmID, dgv_Details.CurrentRow.Cells(3).Value, Trim(lbl_WidthType.Text), , dgv_Details.CurrentRow.Cells(2).Value)

        dgv_Details.CurrentRow.Cells(4).Value = Format(ConsPavu, "#########0.00")

        If Trim(lbl_BeamNo1.Text) <> "" And Trim(lbl_BeamNo2.Text) <> "" Then
            NoofBeams = 2
        Else
            NoofBeams = 1
        End If
        If Val(NoofBeams) = 0 Then NoofBeams = 1

        lbl_BeamConsPavu.Text = Format(Val(dgv_Details.CurrentRow.Cells(4).Value) / NoofBeams, "#########0.00")

    End Sub

    Private Sub ConsumedYarn_Calculation()
        Dim CloID As Integer
        Dim ConsYarn As Single
        'Dim WgtMtr As Single

        CloID = Common_Procedures.Cloth_NameToIdNo(con, cbo_ClothName.Text)

        ConsYarn = Common_Procedures.get_Weft_ConsumedYarn(con, CloID, Val(dgv_Details.CurrentRow.Cells(3).Value))

        ''WgtMtr = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Weight_Meter_Weft", "(cloth_idno = " & Str(Val(CloID)) & ")"))
        ''ConsYarn = Val(txt_Meters.Text) * Val(WgtMtr)

        dgv_Details.CurrentRow.Cells(5).Value = Format(ConsYarn, "#########0.000")

    End Sub

    Private Sub txt_DoffMtrs_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        ConsumedPavu_Calculation()
        ConsumedYarn_Calculation()
    End Sub

    Private Sub txt_CrimpPerc_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        ConsumedPavu_Calculation()
    End Sub

    Private Sub btn_Selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Selection.Click
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Da2 As New SqlClient.SqlDataAdapter
        Dim Da3 As New SqlClient.SqlDataAdapter
        Dim Da4 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim Dt4 As New DataTable
        Dim Lm_ID As Integer
        Dim NewCode As String = ""

        Lm_ID = Common_Procedures.Loom_NameToIdNo(con, cbo_LoomNo.Text)
        If Val(Lm_ID) = 0 Then
            MessageBox.Show("Invalid Loom NO", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_LoomNo.Enabled Then cbo_LoomNo.Focus()
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RollNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Da1 = New SqlClient.SqlDataAdapter("Select a.*, b.Ledger_Name, c.Cloth_Name, d.EndsCount_Name, e.Count_Name, f.Loom_Name from Weaver_Cloth_Receipt_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo INNER JOIN Cloth_Head c ON a.Cloth_IdNo = c.Cloth_IdNo LEFT OUTER JOIN EndsCount_Head d ON a.EndsCount_IdNo = d.EndsCount_IdNo LEFT OUTER JOIN Count_Head e ON a.Count_IdNo = e.Count_IdNo LEFT OUTER JOIN Loom_Head f ON a.Loom_IdNo = f.Loom_IdNo Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "' and a.Loom_IdNo = " & Str(Val(Lm_ID)) & " and a.Receipt_Type = 'L'", con)
        Dt1 = New DataTable
        Da1.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then

            lbl_PartyName.Text = Dt1.Rows(0).Item("Ledger_Name").ToString

            lbl_KnotCode.Text = Dt1.Rows(0).Item("Beam_Knotting_Code").ToString
            lbl_KnotNo.Text = Dt1.Rows(0).Item("Beam_Knotting_No").ToString

            'cbo_ClothName.Text = Dt1.Rows(0).Item("Cloth_Name").ToString
            lbl_WidthType.Text = Dt1.Rows(0).Item("Width_Type").ToString
            lbl_EndsCount.Text = Dt1.Rows(0).Item("EndsCount_Name").ToString
            lbl_Count.Text = Dt1.Rows(0).Item("Count_Name").ToString

            lbl_SetCode1.Text = Dt1.Rows(0).Item("Set_Code1").ToString
            lbl_SetNo1.Text = Dt1.Rows(0).Item("Set_No1").ToString
            lbl_BeamNo1.Text = Dt1.Rows(0).Item("Beam_No1").ToString
            lbl_BalMtrs1.Text = Dt1.Rows(0).Item("Balance_Meters1").ToString

            lbl_TotMtrs1.Text = ""
            Da2 = New SqlClient.SqlDataAdapter("Select * from Stock_SizedPavu_Processing_Details Where Set_Code = '" & Trim(lbl_SetCode1.Text) & "' and Beam_No = '" & Trim(lbl_BeamNo1.Text) & "'", con)
            Dt2 = New DataTable
            Da2.Fill(Dt2)
            If Dt2.Rows.Count > 0 Then
                lbl_TotMtrs1.Text = Dt2.Rows(0).Item("Meters").ToString
            End If
            Dt2.Clear()

            lbl_SetCode2.Text = Dt1.Rows(0).Item("Set_Code2").ToString
            lbl_SetNo2.Text = Dt1.Rows(0).Item("Set_No2").ToString
            lbl_BeamNo2.Text = Dt1.Rows(0).Item("Beam_No2").ToString
            lbl_BalMtrs2.Text = Dt1.Rows(0).Item("Balance_Meters2").ToString
            lbl_TotMtrs2.Text = ""
            Da2 = New SqlClient.SqlDataAdapter("Select * from Stock_SizedPavu_Processing_Details Where Set_Code = '" & Trim(lbl_SetCode2.Text) & "' and Beam_No = '" & Trim(lbl_BeamNo2.Text) & "'", con)
            Dt2 = New DataTable
            Da2.Fill(Dt2)
            If Dt2.Rows.Count > 0 Then
                lbl_TotMtrs2.Text = Dt2.Rows(0).Item("Meters").ToString
            End If
            Dt2.Clear()
            dgv_Details.Rows.Clear()


            'txt_DoffMtrs.Text = Dt1.Rows(0).Item("Doff_Meters").ToString
            'txt_CrimpPerc.Text = Dt1.Rows(0).Item("Crimp_Percentage").ToString
            'lbl_ConsPavu.Text = Dt1.Rows(0).Item("ConsumedPavu_Receipt").ToString
            'lbl_ConsWeftYarn.Text = Dt1.Rows(0).Item("ConsumedYarn_Receipt").ToString

        Else

            Da3 = New SqlClient.SqlDataAdapter("select top 1 a.*, b.Ledger_Name, c.Cloth_Name, c.Crimp_Percentage, d.EndsCount_Name, e.Count_Name from Beam_Knotting_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo INNER JOIN Cloth_Head c ON a.Cloth_IdNo1 = c.Cloth_IdNo INNER JOIN EndsCount_Head d ON a.EndsCount_IdNo = d.EndsCount_IdNo INNER JOIN Count_Head e ON c.Cloth_WeftCount_IdNo = e.Count_IdNo Where a.Loom_IdNo = " & Str(Val(Lm_ID)) & " and a.Beam_RunOut_Code = '' Order by a.Beam_Knotting_Date desc, a.for_OrderBy desc, a.Beam_Knotting_Code desc", con)
            Dt3 = New DataTable
            Da3.Fill(Dt3)

            If Dt3.Rows.Count > 0 Then
                lbl_PartyName.Text = Dt3.Rows(0).Item("Ledger_Name").ToString

                lbl_KnotCode.Text = Dt3.Rows(0).Item("Beam_Knotting_Code").ToString
                lbl_KnotNo.Text = Dt3.Rows(0).Item("Beam_Knotting_No").ToString

                'cbo_ClothName.Text = Dt3.Rows(0).Item("Cloth_Name").ToString
                lbl_WidthType.Text = Dt3.Rows(0).Item("Width_Type").ToString
                lbl_EndsCount.Text = Dt3.Rows(0).Item("EndsCount_Name").ToString
                lbl_Count.Text = Dt3.Rows(0).Item("Count_Name").ToString

                lbl_SetCode1.Text = Dt3.Rows(0).Item("Set_Code1").ToString
                lbl_SetNo1.Text = Dt3.Rows(0).Item("Set_No1").ToString
                lbl_BeamNo1.Text = Dt3.Rows(0).Item("Beam_No1").ToString

                lbl_TotMtrs1.Text = ""
                lbl_BalMtrs1.Text = ""
                Da4 = New SqlClient.SqlDataAdapter("Select * from Stock_SizedPavu_Processing_Details Where Set_Code = '" & Trim(lbl_SetCode1.Text) & "' and Beam_No = '" & Trim(lbl_BeamNo1.Text) & "'", con)
                Dt4 = New DataTable
                Da4.Fill(Dt4)
                If Dt4.Rows.Count > 0 Then
                    lbl_TotMtrs1.Text = Dt4.Rows(0).Item("Meters").ToString
                    lbl_BalMtrs1.Text = Format(Val(Dt4.Rows(0).Item("Meters").ToString) - Val(Dt3.Rows(0).Item("Production_Meters").ToString), "#########0.00")
                End If
                Dt4.Clear()

                lbl_SetCode2.Text = Dt3.Rows(0).Item("Set_Code2").ToString
                lbl_SetNo2.Text = Dt3.Rows(0).Item("Set_No2").ToString
                lbl_BeamNo2.Text = Dt3.Rows(0).Item("Beam_No2").ToString
                lbl_BalMtrs2.Text = ""
                lbl_TotMtrs2.Text = ""
                Da4 = New SqlClient.SqlDataAdapter("Select * from Stock_SizedPavu_Processing_Details Where Set_Code = '" & Trim(lbl_SetCode2.Text) & "' and Beam_No = '" & Trim(lbl_BeamNo2.Text) & "'", con)
                Dt4 = New DataTable
                Da4.Fill(Dt4)
                If Dt4.Rows.Count > 0 Then
                    lbl_TotMtrs2.Text = Dt4.Rows(0).Item("Meters").ToString
                    lbl_BalMtrs2.Text = Format(Val(Dt4.Rows(0).Item("Meters").ToString) - Val(Dt3.Rows(0).Item("Production_Meters").ToString), "#########0.00")
                End If
                Dt4.Clear()

                'txt_DoffMtrs.Text = dt3.Rows(0).Item("Doff_Meters").ToString
                'txt_CrimpPerc.Text = Dt3.Rows(0).Item("Crimp_Percentage").ToString
                'lbl_ConsPavu.Text = dt3.Rows(0).Item("ConsumedPavu_Receipt").ToString
                'lbl_ConsWeftYarn.Text = dt3.Rows(0).Item("ConsumedYarn_Receipt").ToString

            End If
            Dt3.Clear()
        End If

        cbo_LoomNo.Tag = cbo_LoomNo.Text

        Dt1.Dispose()
        Da1.Dispose()

        Dt2.Dispose()
        Da2.Dispose()

        Dt3.Dispose()
        Da3.Dispose()

        Dt4.Dispose()
        Da4.Dispose()

        If dgv_Details.Rows.Count > 0 Then

            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
            dgv_Details.CurrentCell.Selected = True
        End If

    End Sub

    Private Sub btn_Close_Selection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Selection.Click
        pnl_back.Enabled = True
        pnl_Selection.Visible = False
        If cbo_ClothName.Enabled And cbo_ClothName.Visible Then cbo_ClothName.Focus()
    End Sub

    Private Sub cbo_ClothName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ClothName.TextChanged
        Try
            If cbo_ClothName.Visible Then
                With dgv_Details
                    If Val(cbo_ClothName.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_ClothName.Text)

                        ConsumedPavu_Calculation()
                        ConsumedYarn_Calculation()
                        Total_Calculation()
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
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

End Class