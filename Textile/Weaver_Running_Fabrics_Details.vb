Public Class Weaver_Running_Fabrics_Details_Entry
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private dgv_ActiveCtrl_Name As String

    Private NoCalc_Status As Boolean = False
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double

    Private dgvDet_CboBx_ColNos_Arr As Integer() = {1}

    Public vmskOldText As String = ""
    Public vmskSelStrt As Integer = -1

    Public Sub New()
        FrmLdSTS = True
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Private Sub clear()
        NoCalc_Status = True
        New_Entry = False
        Insert_Entry = False

        pnl_Back.Enabled = True
        pnl_Filter.Visible = False

        lbl_IdNo.Text = ""
        lbl_IdNo.ForeColor = Color.Black

        vmskOldText = ""
        vmskSelStrt = -1

        dtp_date.Text = ""




        Msk_StartDate.Text = ""
        dtp_StartDate.Text = ""
        Msk_EndDate.Text = ""
        dtp_Enddate.Text = ""
        txt_Particulars.Text = ""



        chk_CloseStatus.Checked = False


        cbo_WeaverName.Text = Trim(lbl_Weaver.Text)
        cbo_WeaverName.Enabled = False
        ' cbo_WeaverName.Enabled = True
        ' cbo_WeaverName.BackColor = Color.White


        dgv_Count_details.Rows.Clear()
        dgv_Count_details.Rows.Add()

        dgv_EndsCount_Details.Rows.Clear()
        dgv_EndsCount_Details.Rows.Add()

        dgv_Cloth_Details.Rows.Clear()
        dgv_Cloth_Details.Rows.Add()




        If Filter_Status = False Then
            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1

            dgv_Filter_Details.Rows.Clear()
        End If



        Grid_Cell_DeSelect()

        NoCalc_Status = False

    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox
        Dim mskdtxbx As MaskedTextBox
        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Prec_ActCtrl Is MaskedTextBox Then
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
            mskdtxbx = Me.ActiveControl
            mskdtxbx.SelectionStart = 0
        End If

        If Me.ActiveControl.Name <> Cbo_Grid_CountName.Name Then
            Cbo_Grid_CountName.Visible = False
            Cbo_Grid_CountName.Tag = -100
        End If

        If Me.ActiveControl.Name <> Cbo_Grid_EndsCountName.Name Then
            Cbo_Grid_EndsCountName.Visible = False
            Cbo_Grid_EndsCountName.Tag = -100
        End If

        If Me.ActiveControl.Name <> cbo_Grid_ClothName.Name Then
            cbo_Grid_ClothName.Visible = False
            cbo_Grid_ClothName.Tag = -100
        End If

        Grid_Cell_DeSelect()

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

    Private Sub ControlLostFocus1(ByVal sender As Object, ByVal e As System.EventArgs)

        On Error Resume Next

        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Then
                Prec_ActCtrl.BackColor = Color.LightSkyBlue
                Prec_ActCtrl.ForeColor = Color.Blue
            End If
        End If

    End Sub

    Private Sub Grid_DeSelect()
        On Error Resume Next
        If Not IsNothing(dgv_Count_details.CurrentCell) Then dgv_Count_details.CurrentCell.Selected = False

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
        If Not IsNothing(dgv_Count_details.CurrentCell) Then dgv_Count_details.CurrentCell.Selected = False
        If Not IsNothing(dgv_EndsCount_Details.CurrentCell) Then dgv_EndsCount_Details.CurrentCell.Selected = False
        If Not IsNothing(dgv_Cloth_Details.CurrentCell) Then dgv_Cloth_Details.CurrentCell.Selected = False
        If Not IsNothing(dgv_Filter_Details.CurrentCell) Then dgv_Filter_Details.CurrentCell.Selected = False
    End Sub

    Private Sub Weaver_Running_Fabrics_Details_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_WeaverName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_WeaverName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(Cbo_Grid_CountName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                Cbo_Grid_CountName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(Cbo_Grid_EndsCountName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                Cbo_Grid_EndsCountName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_ClothName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_ClothName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""



            If FrmLdSTS = True Then

                lbl_Weaver.Text = ""
                lbl_Weaver.Tag = 0
                Common_Procedures.CompIdNo = 0
                Common_Procedures.VWeavIdno = 0

                Me.Text = ""

                lbl_Weaver.Text = Common_Procedures.get_Weaver_From_WeaverSelection(con)
                lbl_Weaver.Tag = Val(Common_Procedures.VWeavIdno)

                cbo_WeaverName.Text = Trim(lbl_Weaver.Text)

                new_record()

            End If





        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        FrmLdSTS = False

    End Sub

    Private Sub Weaver_Running_Fabrics_Details_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub Weaver_Running_Fabrics_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
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
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub Weaver_Running_Fabrics_Details_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load

        FrmLdSTS = True
        Me.Text = ""

        con.Open()

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2
        pnl_Filter.BringToFront()

        AddHandler dtp_date.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_WeaverName.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_Grid_ClothName.GotFocus, AddressOf ControlGotFocus
        AddHandler Cbo_Grid_CountName.GotFocus, AddressOf ControlGotFocus
        AddHandler Cbo_Grid_EndsCountName.GotFocus, AddressOf ControlGotFocus


        AddHandler dtp_Filter_Fromdate.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_ToDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus


        AddHandler btn_save.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_close.GotFocus, AddressOf ControlGotFocus


        AddHandler dtp_date.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_date.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_WeaverName.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_Grid_ClothName.LostFocus, AddressOf ControlLostFocus
        AddHandler Cbo_Grid_CountName.LostFocus, AddressOf ControlLostFocus
        AddHandler Cbo_Grid_EndsCountName.LostFocus, AddressOf ControlLostFocus


        AddHandler dtp_Filter_Fromdate.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_ToDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus

        AddHandler btn_save.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_close.LostFocus, AddressOf ControlLostFocus


        AddHandler msk_date.KeyDown, AddressOf TextBoxControlKeyDown


        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler msk_date.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler dtp_Filter_Fromdate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_ToDate.KeyPress, AddressOf TextBoxControlKeyPress


        AddHandler Msk_StartDate.GotFocus, AddressOf ControlGotFocus
        AddHandler Msk_EndDate.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Particulars.GotFocus, AddressOf ControlGotFocus

        AddHandler Msk_StartDate.LostFocus, AddressOf ControlLostFocus
        AddHandler Msk_EndDate.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Particulars.LostFocus, AddressOf ControlLostFocus


        lbl_Weaver.Text = ""
        lbl_Weaver.Tag = 0
        lbl_Weaver.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True
        new_record()

    End Sub

    Private Sub Close_Form()

        Try

            lbl_Weaver.Tag = 0
            lbl_Weaver.Text = ""
            Me.Text = ""
            Common_Procedures.VWeavIdno = 0


            lbl_Weaver.Text = Common_Procedures.Show_WeaverSelection_On_FormClose(con)

            lbl_Weaver.Tag = Val(Common_Procedures.VWeavIdno)

            Me.Text = lbl_Weaver.Text


            If Val(Common_Procedures.VWeavIdno) = 0 Then

                Me.Close()

            Else

                new_record()

            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
        Dim dgv1 As New DataGridView


        If ActiveControl.Name = dgv_Count_details.Name Or ActiveControl.Name = dgv_EndsCount_Details.Name Or ActiveControl.Name = dgv_Cloth_Details.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            dgv1 = Nothing

            If ActiveControl.Name = dgv_Count_details.Name Then
                dgv1 = dgv_Count_details

            ElseIf ActiveControl.Name = dgv_EndsCount_Details.Name Then
                dgv1 = dgv_EndsCount_Details

            ElseIf ActiveControl.Name = dgv_Cloth_Details.Name Then
                dgv1 = dgv_Cloth_Details

            ElseIf dgv_Count_details.IsCurrentRowDirty = True Then
                dgv1 = dgv_Count_details

            ElseIf dgv_EndsCount_Details.IsCurrentRowDirty = True Then
                dgv1 = dgv_EndsCount_Details

            ElseIf dgv_Cloth_Details.IsCurrentRowDirty = True Then
                dgv1 = dgv_Cloth_Details

            ElseIf dgv_ActiveCtrl_Name = dgv_Count_details.Name Then
                dgv1 = dgv_Count_details

            ElseIf dgv_ActiveCtrl_Name = dgv_EndsCount_Details.Name Then
                dgv1 = dgv_EndsCount_Details

            ElseIf dgv_ActiveCtrl_Name = dgv_Cloth_Details.Name Then
                dgv1 = dgv_Cloth_Details

            End If

            If IsNothing(dgv1) = False Then

                With dgv1

                    If dgv1.Name = dgv_Cloth_Details.Name Then



                        If keyData = Keys.Enter Or keyData = Keys.Down Or keyData = Keys.Up Then

                            Common_Procedures.DGVGrid_Cursor_Focus(dgv1, keyData, txt_Particulars, dgv_Cloth_Details, dgvDet_CboBx_ColNos_Arr, Nothing, dtp_date)

                            Return True

                        Else
                            Return MyBase.ProcessCmdKey(msg, keyData)

                        End If

                    ElseIf dgv1.Name = dgv_Count_details.Name Then

                        If keyData = Keys.Enter Or keyData = Keys.Down Or keyData = Keys.Up Then

                            Common_Procedures.DGVGrid_Cursor_Focus(dgv1, keyData, dgv_Cloth_Details, dgv_EndsCount_Details, dgvDet_CboBx_ColNos_Arr, Nothing, dtp_date)

                            Return True

                        Else
                            Return MyBase.ProcessCmdKey(msg, keyData)

                        End If

                    ElseIf dgv1.Name = dgv_EndsCount_Details.Name Then


                        If keyData = Keys.Enter Or keyData = Keys.Down Or keyData = Keys.Up Then

                            Common_Procedures.DGVGrid_Cursor_Focus(dgv1, keyData, dgv_Count_details, btn_save, dgvDet_CboBx_ColNos_Arr, Nothing, dtp_date)

                            Return True

                        Else
                            Return MyBase.ProcessCmdKey(msg, keyData)

                        End If
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

        Return MyBase.ProcessCmdKey(msg, keyData)

    End Function

    Private Sub move_record(ByVal idno As String)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim da3 As New SqlClient.SqlDataAdapter
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim n As Integer
        Dim SNo As Integer
        Dim LockSTS As Boolean = False

        If Val(idno) = 0 Then Exit Sub

        clear()

        NoCalc_Status = True



        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.ledger_name as weavername from Weaver_Running_Fabrics_Head a INNER JOIN Ledger_Head b ON a.Weaver_idno = b.ledger_idno where a.Weaver_IdNo = " & Str(Val(lbl_Weaver.Tag)) & " and a.Weaver_Running_Fabrics_IdNo = " & Str(Val(idno)), con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_IdNo.Text = dt1.Rows(0).Item("Weaver_Running_Fabrics_IdNo").ToString

                msk_date.Text = dt1.Rows(0).Item("Weaver_Running_Fabrics_date").ToString
                cbo_WeaverName.Text = dt1.Rows(0).Item("weavername").ToString
                Msk_StartDate.Text = dt1.Rows(0).Item("StartDate").ToString
                Msk_EndDate.Text = dt1.Rows(0).Item("EndDate").ToString
                txt_Particulars.Text = dt1.Rows(0).Item("Particulars").ToString

                If Val(dt1.Rows(0).Item("Close_Status").ToString) = 1 Then chk_CloseStatus.Checked = True

                da2 = New SqlClient.SqlDataAdapter("Select a.*, b.count_name from Weaver_Running_Fabrics_Count_Details a INNER JOIN Count_Head b On a.Count_idno = b.Count_idno Where a.Weaver_Running_Fabrics_IdNo = " & Str(Val(idno)) & " Order by a.sl_no", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                With dgv_Count_details

                    .Rows.Clear()
                    SNo = 0

                    If dt2.Rows.Count > 0 Then

                        For i = 0 To dt2.Rows.Count - 1

                            n = .Rows.Add()

                            SNo = SNo + 1

                            .Rows(n).Cells(0).Value = Val(SNo)
                            .Rows(n).Cells(1).Value = dt2.Rows(i).Item("count_name").ToString

                        Next i

                    End If
                    n = .Rows.Add()

                End With

                NoCalc_Status = False

                NoCalc_Status = True
                da2 = New SqlClient.SqlDataAdapter("Select a.*, b.EndsCount_name from Weaver_Running_Fabrics_EndsCount_Details a INNER JOIN EndsCount_Head b On a.EndsCount_idno = b.EndsCount_idno Where a.Weaver_Running_Fabrics_IdNo = " & Str(Val(idno)) & " Order by a.sl_no", con)
                dt3 = New DataTable
                da2.Fill(dt3)

                With dgv_EndsCount_Details

                    .Rows.Clear()
                    SNo = 0

                    If dt3.Rows.Count > 0 Then

                        For i = 0 To dt3.Rows.Count - 1

                            n = .Rows.Add()

                            SNo = SNo + 1

                            .Rows(n).Cells(0).Value = Val(SNo)
                            .Rows(n).Cells(1).Value = dt3.Rows(i).Item("EndsCount_name").ToString

                        Next i


                    End If
                    n = .Rows.Add()
                End With
                NoCalc_Status = False

                NoCalc_Status = True
                da2 = New SqlClient.SqlDataAdapter("Select a.*, b.Cloth_name from Weaver_Running_Fabrics_Cloth_Details a INNER JOIN Cloth_Head b On a.Cloth_idno = b.Cloth_idno Where a.Weaver_Running_Fabrics_IdNo = " & Str(Val(idno)) & " Order by a.sl_no", con)
                dt4 = New DataTable
                da2.Fill(dt4)

                With dgv_Cloth_Details

                    .Rows.Clear()
                    SNo = 0

                    If dt4.Rows.Count > 0 Then

                        For i = 0 To dt4.Rows.Count - 1

                            n = .Rows.Add()

                            SNo = SNo + 1

                            .Rows(n).Cells(0).Value = Val(SNo)
                            .Rows(n).Cells(1).Value = dt4.Rows(i).Item("Cloth_name").ToString

                        Next i



                    End If
                    n = .Rows.Add()
                End With


                NoCalc_Status = False

                NoCalc_Status = True

            else

                new_record()

            End If

            Grid_Cell_DeSelect()


            cbo_WeaverName.Enabled = False
            cbo_WeaverName.BackColor = Color.LightGray

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES Not MOVE RECORDS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally

            dt1.Dispose()
            da1.Dispose()

            dt2.Dispose()
            da2.Dispose()

            If msk_date.Enabled = True And msk_date.Visible = True Then msk_date.Focus()
            If dtp_date.Enabled = True And dtp_date.Visible = True Then dtp_date.Focus()


        End Try

        NoCalc_Status = False

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Fabric_Delivery_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Fabric_Delivery_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights To Delete", "DOES Not DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub

        If Val(lbl_Weaver.Tag) = 0 Then
            MessageBox.Show("Invalid Weaver Selection", "DOES Not DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES Not DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If MessageBox.Show("Do you want To Delete?", "For DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This Is New Entry", "DOES Not DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If


        trans = con.BeginTransaction

        Try


            cmd.Connection = con
            cmd.Transaction = trans

            cmd.CommandText = "delete from Weaver_Running_Fabrics_EndsCount_Details where Weaver_IdNo = " & Str(Val(lbl_Weaver.Tag)) & " And Weaver_Running_Fabrics_IdNo = " & Str(Val(lbl_IdNo.Text))
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Weaver_Running_Fabrics_Count_Details where Weaver_IdNo = " & Str(Val(lbl_Weaver.Tag)) & " And Weaver_Running_Fabrics_IdNo = " & Str(Val(lbl_IdNo.Text))
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Weaver_Running_Fabrics_Cloth_Details where Weaver_IdNo = " & Str(Val(lbl_Weaver.Tag)) & " And Weaver_Running_Fabrics_IdNo = " & Str(Val(lbl_IdNo.Text))
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Weaver_Running_Fabrics_Head where Weaver_IdNo = " & Str(Val(lbl_Weaver.Tag)) & " And Weaver_Running_Fabrics_IdNo =  " & Str(Val(lbl_IdNo.Text))
            cmd.ExecuteNonQuery()



            trans.Commit()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "For DELETION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception
            trans.Rollback()
            MessageBox.Show(ex.Message, "DOES Not DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally

            Dt1.Dispose()
            Da.Dispose()
            trans.Dispose()
            cmd.Dispose()
            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()
        End Try

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        Exit Sub

        If Filter_Status = False Then

            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1


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
        Dim movno As String

        Try

            da = New SqlClient.SqlDataAdapter("Select min(Weaver_Running_Fabrics_IdNo) from Weaver_Running_Fabrics_Head Where Weaver_IdNo = " & Str(Val(lbl_Weaver.Tag)) & " and Weaver_Running_Fabrics_IdNo <> 0", con)
            dt = New DataTable
            da.Fill(dt)

            movno = ""
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = Trim(dt.Rows(0)(0).ToString)
                End If
            End If

            dt.Clear()

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "For MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            dt.Dispose()
            da.Dispose()

        End Try

    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""
        Dim OrdByNo As Single = 0

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_IdNo.Text))

            da = New SqlClient.SqlDataAdapter("Select min(Weaver_Running_Fabrics_IdNo) from Weaver_Running_Fabrics_Head Where  Weaver_IdNo = " & Str(Val(lbl_Weaver.Tag)) & " and Weaver_Running_Fabrics_IdNo > " & Str(Val(lbl_IdNo.Text)) & " And Weaver_Running_Fabrics_IdNo <> 0", con)
            dt = New DataTable
            da.Fill(dt)

            movno = ""
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = dt.Rows(0)(0).ToString
                End If
            End If

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "For MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            dt.Dispose()
            da.Dispose()

        End Try

    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""
        Dim OrdByNo As Single = 0

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_IdNo.Text))

            da = New SqlClient.SqlDataAdapter("Select max(Weaver_Running_Fabrics_IdNo) from Weaver_Running_Fabrics_Head Where  Weaver_IdNo = " & Str(Val(lbl_Weaver.Tag)) & " and Weaver_Running_Fabrics_IdNo < " & Str(Val(lbl_IdNo.Text)) & " And Weaver_Running_Fabrics_IdNo <> 0", con)
            dt = New DataTable
            da.Fill(dt)

            movno = ""
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = dt.Rows(0)(0).ToString
                End If
            End If

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "For MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            dt.Dispose()
            da.Dispose()

        End Try

    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""

        Try
            da = New SqlClient.SqlDataAdapter("Select max(Weaver_Running_Fabrics_IdNo) from Weaver_Running_Fabrics_Head Where  Weaver_IdNo = " & Str(Val(lbl_Weaver.Tag)) & " and Weaver_Running_Fabrics_IdNo <> 0", con)
            dt = New DataTable
            da.Fill(dt)

            movno = ""
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = dt.Rows(0)(0).ToString
                End If
            End If

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "For MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            dt.Dispose()
            da.Dispose()

        End Try

    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable

        Try
            clear()

            New_Entry = True
            lbl_IdNo.ForeColor = Color.Red

            lbl_IdNo.Text = Common_Procedures.get_MaxIdNo(con, "Weaver_Running_Fabrics_Head", "Weaver_Running_Fabrics_IdNo", "")


        Catch ex As Exception
            MessageBox.Show(ex.Message, "For New RECORD...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()

        End Try

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String


        Try

            inpno = InputBox("Enter Ref No.", "For FINDING...")

            Da = New SqlClient.SqlDataAdapter("Select Weaver_Running_Fabrics_IdNo from Weaver_Running_Fabrics_Head where Weaver_IdNo = " & Str(Val(lbl_Weaver.Tag)) & " And Weaver_Running_Fabrics_IdNo = " & Str(Val(inpno)), con)
            Dt = New DataTable
            Da.Fill(Dt)

            movno = ""
            If Dt.Rows.Count > 0 Then
                If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                    movno = Trim(Dt.Rows(0)(0).ToString)
                End If
            End If

            Dt.Clear()

            If Val(movno) <> 0 Then
                move_record(movno)

            Else
                MessageBox.Show("Invoice No. does Not exists", "DOES Not FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES Not FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            Dt.Dispose()
            Da.Dispose()

        End Try

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        '-----
    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim tr As SqlClient.SqlTransaction
        Dim Led_ID As Integer = 0
        Dim OnAc_ID As Integer = 0
        Dim VaAc_ID As Integer = 0
        Dim PrnPl_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim Slno As Integer = 0
        Dim Sno1 As Integer = 0
        Dim Nr As Integer = 0
        Dim vCount_ID As Integer = 0
        Dim vEndsCount_ID As Integer = 0
        Dim vCloth_ID As Integer = 0
        Dim vPRTCULRS_for_SELEC As String = ""
        Dim vCLOSESTS As Integer




        '   If Common_Procedures.UserRight_Check(Common_Procedures.UR.Fabric_Receipt_Entry, New_Entry) = False Then Exit Sub

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES Not SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Val(lbl_Weaver.Tag) = 0 Then
            MessageBox.Show("Invalid Weaver Selection", "DOES Not SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(msk_date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES Not SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()
            Exit Sub
        End If

        If IsDate(Msk_StartDate.Text) = False Then
            MessageBox.Show("Invalid Start Date", "DOES Not SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If Msk_StartDate.Enabled And Msk_StartDate.Visible Then Msk_StartDate.Focus()
            Exit Sub
        End If

        If IsDate(Msk_EndDate.Text) = False Then
            MessageBox.Show("Invalid End Date", "DOES Not SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If Msk_EndDate.Enabled And Msk_EndDate.Visible Then Msk_EndDate.Focus()
            Exit Sub
        End If


        Led_ID = Val(lbl_Weaver.Tag)

        'cbo_WeaverName.Text = Trim(lbl_Weaver.Text)
        'cbo_WeaverName.Enabled = False
        'Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_WeaverName.Text)

        If Led_ID = 0 Then
            MessageBox.Show("Invalid Weaver Name", "DOES Not SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_WeaverName.Enabled Then cbo_WeaverName.Focus()
            Exit Sub
        End If

        NoCalc_Status = False

        vCLOSESTS = 0
        If chk_CloseStatus.Checked = True Then vCLOSESTS = 1


        vPRTCULRS_for_SELEC = Trim(Msk_StartDate.Text) & " To " & Trim(Msk_EndDate.Text) & " / " & Trim(txt_Particulars.Text)

        tr = con.BeginTransaction

        Try


            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@InvDate", Convert.ToDateTime(msk_date.Text))
            cmd.Parameters.AddWithValue("@StartDate", Convert.ToDateTime(Msk_StartDate.Text))
            cmd.Parameters.AddWithValue("@EndDate", Convert.ToDateTime(Msk_EndDate.Text))


            If New_Entry = True Then

                lbl_IdNo.Text = Common_Procedures.get_MaxIdNo(con, "Weaver_Running_Fabrics_Head ", "Weaver_Running_Fabrics_IdNo", "", tr)

                cmd.CommandText = "Insert into Weaver_Running_Fabrics_Head (    Weaver_Running_Fabrics_IdNo  ,                 Weaver_IdNo    , Weaver_Running_Fabrics_date, StartDate  ,  EndDate  ,             Particulars              ,        Close_Status    ,       datetime_text           ,         Startdate_text             ,         Enddate_text            , StartDate_EndDate_Particulars_for_Selection ) " &
                                  " Values                                 ( " & Str(Val(lbl_IdNo.Text)) & ", " & Str(Val(lbl_Weaver.Tag)) & ",       @InvDate             ,  @StartDate,   @EndDate, '" & Trim(txt_Particulars.Text) & "' ,  " & Val(vCLOSESTS) & ", '" & Trim(msk_date.Text) & "' , '" & Trim(Msk_StartDate.Text) & "' , '" & Trim(Msk_EndDate.Text) & "',      '" & Trim(vPRTCULRS_for_SELEC) & "'    ) "
                cmd.ExecuteNonQuery()

            Else

                cmd.CommandText = "Update Weaver_Running_Fabrics_Head Set Weaver_Running_Fabrics_date = @InvDate, Weaver_IdNo = " & Str(Val(lbl_Weaver.Tag)) & " , StartDate=@StartDate,EndDate=@EndDate  , Particulars  ='" & Trim(txt_Particulars.Text) & "' , Close_Status = " & Val(vCLOSESTS) & " ,datetime_text ='" & Trim(msk_date.Text) & "',  Startdate_text ='" & Trim(Msk_StartDate.Text) & "' , Enddate_text ='" & Trim(Msk_EndDate.Text) & "' , StartDate_EndDate_Particulars_for_Selection = '" & Trim(vPRTCULRS_for_SELEC) & "' Where Weaver_IdNo = " & Str(Val(lbl_Weaver.Tag)) & " And Weaver_Running_Fabrics_IdNo = " & Str(Val(lbl_IdNo.Text))
                cmd.ExecuteNonQuery()


            End If


            cmd.CommandText = "delete from Weaver_Running_Fabrics_Cloth_Details where Weaver_IdNo = " & Str(Val(lbl_Weaver.Tag)) & " and Weaver_Running_Fabrics_IdNo = " & Str(Val(lbl_IdNo.Text))
            cmd.ExecuteNonQuery()


            cmd.CommandText = "delete from Weaver_Running_Fabrics_EndsCount_Details where Weaver_IdNo = " & Str(Val(lbl_Weaver.Tag)) & " and Weaver_Running_Fabrics_IdNo = " & Str(Val(lbl_IdNo.Text))
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Weaver_Running_Fabrics_Count_Details where Weaver_IdNo = " & Str(Val(lbl_Weaver.Tag)) & " and Weaver_Running_Fabrics_IdNo =" & Str(Val(lbl_IdNo.Text))
            cmd.ExecuteNonQuery()


            With dgv_Count_details

                Sno = 0

                For i = 0 To .RowCount - 1

                    If Trim(.Rows(i).Cells(1).Value) <> "" Then

                        Sno = Sno + 1

                        vCount_ID = Common_Procedures.Count_NameToIdNo(con, .Rows(i).Cells(1).Value, tr)

                        cmd.CommandText = "Insert into Weaver_Running_Fabrics_Count_Details (   Weaver_Running_Fabrics_IdNo   ,  Weaver_Running_Fabrics_date ,                 Weaver_IdNo       ,            Sl_No      ,            Count_IdNo        ) " &
                                                  " Values                                  ( " & Str(Val(lbl_IdNo.Text)) & " ,      @InvDate                ,  " & Str(Val(lbl_Weaver.Tag)) & " , " & Str(Val(Sno)) & " ,  " & Str(Val(vCount_ID)) & " ) "
                        cmd.ExecuteNonQuery()

                    End If

                Next

            End With
            With dgv_EndsCount_Details
                Slno = 0
                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(1).Value) <> 0 Then

                        vEndsCount_ID = Common_Procedures.EndsCount_NameToIdNo(con, .Rows(i).Cells(1).Value, tr)

                        Slno = Slno + 1

                        cmd.CommandText = "Insert into Weaver_Running_Fabrics_EndsCount_Details (   Weaver_Running_Fabrics_IdNo   ,  Weaver_Running_Fabrics_date ,                 Weaver_IdNo       ,            Sl_No       ,            EndsCount_IdNo        ) " &
                                                  " Values                                      ( " & Str(Val(lbl_IdNo.Text)) & " ,      @InvDate                ,  " & Str(Val(lbl_Weaver.Tag)) & " , " & Str(Val(Slno)) & " ,  " & Str(Val(vEndsCount_ID)) & " ) "
                        cmd.ExecuteNonQuery()


                    End If

                Next
            End With


            With dgv_Cloth_Details

                Sno1 = 0

                For i = 0 To .RowCount - 1

                    If Trim(.Rows(i).Cells(1).Value) <> "" Then

                        Sno1 = Sno1 + 1
                        vCloth_ID = Common_Procedures.Cloth_NameToIdNo(con, .Rows(i).Cells(1).Value, tr)

                        cmd.CommandText = "Insert into Weaver_Running_Fabrics_Cloth_Details (   Weaver_Running_Fabrics_IdNo   ,  Weaver_Running_Fabrics_date ,                 Weaver_IdNo       ,            Sl_No       ,            Cloth_IdNo        ) " &
                                                  " Values                                  ( " & Str(Val(lbl_IdNo.Text)) & " ,      @InvDate                ,  " & Str(Val(lbl_Weaver.Tag)) & " , " & Str(Val(Sno1)) & " ,  " & Str(Val(vCloth_ID)) & " ) "
                        cmd.ExecuteNonQuery()

                    End If

                Next

            End With



            tr.Commit()


            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

            If New_Entry = False Then
                move_record(lbl_IdNo.Text)
            Else
                new_record()
            End If

        Catch ex As Exception
            tr.Rollback()


            If InStr(1, Trim(LCase(ex.Message)), Trim(LCase("IX_Weaver_Running_Fabrics_Head_001"))) > 0 Then
                MessageBox.Show("Duplicate FromDate/ToDate/Particulars", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            ElseIf InStr(1, Trim(LCase(ex.Message)), Trim(LCase("PK_Weaver_Running_Fabrics_EndsCount_Details"))) > 0 Then
                MessageBox.Show("Duplicate Ends/Count Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            ElseIf InStr(1, Trim(LCase(ex.Message)), Trim(LCase("PK_Weaver_Running_Fabrics_Count_Details"))) > 0 Then
                MessageBox.Show("Duplicate Count Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            ElseIf InStr(1, Trim(LCase(ex.Message)), Trim(LCase("PK_Weaver_Running_Fabrics_Cloth_Details"))) > 0 Then
                MessageBox.Show("Duplicate Cloth Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Else
                MessageBox.Show(ex.Message, "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            End If




        Finally
            Dt1.Dispose()
            Da.Dispose()
            cmd.Dispose()
            tr.Dispose()

            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()

        End Try

    End Sub


    Public Sub print_record() Implements Interface_MDIActions.print_record
        '---
    End Sub



    Private Sub btn_save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
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
        Dim n As Integer
        Dim Led_IdNo As Integer, Prnt_IdNo As Integer
        Dim Condt As String = ""


        Try

            Condt = ""
            Led_IdNo = 0
            Prnt_IdNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Weaver_Running_Fabrics_date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Weaver_Running_Fabrics_date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Weaver_Running_Fabrics_date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If


            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If


            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " (a.Weaver_idno = " & Str(Val(Led_IdNo)) & ")"
            End If


            da = New SqlClient.SqlDataAdapter("select a.* ,c.Ledger_Name from Weaver_Running_Fabrics_Head a  INNER join Ledger_head c on a.Weaver_idno = c.Ledger_idno where a.Weaver_IdNo =" & Str(Val(lbl_Weaver.Tag)) & " " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.Weaver_Running_Fabrics_date, a.for_orderby, a.Weaver_Running_Fabrics_IdNo", con)
            dt2 = New DataTable
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = dt2.Rows(i).Item("Weaver_Running_Fabrics_IdNo").ToString
                    dgv_Filter_Details.Rows(n).Cells(1).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Weaver_Running_Fabrics_date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(3).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("StartDate").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(4).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("EndDate").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(5).Value = dt2.Rows(i).Item("Particulars").ToString

                Next i

            End If

            dt2.Clear()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FILTER...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            dt2.Dispose()
            da.Dispose()

            If dgv_Filter_Details.Visible And dgv_Filter_Details.Enabled Then dgv_Filter_Details.Focus()

        End Try

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


    Private Sub cbo_Filter_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'WEAVER' or Ledger_Type = 'JOBWORKER'  or Show_In_All_Entry = 1) and Close_status = 0 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, Nothing, btn_save, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'WEAVER' or Ledger_Type = 'JOBWORKER'  or Show_In_All_Entry = 1) and Close_status = 0 )", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, btn_save, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'WEAVER' or Ledger_Type = 'JOBWORKER'  or Show_In_All_Entry = 1) and Close_status = 0 )", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_WeaverName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_WeaverName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'WEAVER' or Ledger_Type = 'JOBWORKER'  or Show_In_All_Entry = 1) and Close_status = 0 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_WeaverName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_WeaverName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_WeaverName, dtp_date, Msk_StartDate, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'WEAVER' or Ledger_Type = 'JOBWORKER'  or Show_In_All_Entry = 1) and Close_status = 0 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_WeaverName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_WeaverName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_WeaverName, Msk_StartDate, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'WEAVER' or Ledger_Type = 'JOBWORKER'  or Show_In_All_Entry = 1) and Close_status = 0 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_WeaverName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_WeaverName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Common_Procedures.MDI_LedType = "WEAVER"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_WeaverName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub dgv_Count_Details_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Count_details.CellEndEdit
        dgv_Count_Details_CellLeave(sender, e)
    End Sub

    Private Sub dgv_Count_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Count_details.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Rect As Rectangle
        Dim LedID As Integer = 0
        Dim CloID As Integer = 0
        With dgv_Count_details

            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If

            If e.ColumnIndex = 1 Then

                If Cbo_Grid_CountName.Visible = False Or Val(Cbo_Grid_CountName.Tag) <> e.RowIndex Then

                    Cbo_Grid_CountName.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Count_Name from Count_Head order by Count_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    Cbo_Grid_CountName.DataSource = Dt1
                    Cbo_Grid_CountName.DisplayMember = "Count_Name"

                    Rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    Cbo_Grid_CountName.Left = .Left + Rect.Left
                    Cbo_Grid_CountName.Top = .Top + Rect.Top

                    Cbo_Grid_CountName.Width = Rect.Width
                    Cbo_Grid_CountName.Height = Rect.Height
                    Cbo_Grid_CountName.Text = .CurrentCell.Value

                    Cbo_Grid_CountName.Tag = Val(e.RowIndex)
                    Cbo_Grid_CountName.Visible = True

                    Cbo_Grid_CountName.BringToFront()
                    Cbo_Grid_CountName.Focus()



                End If

            Else
                Cbo_Grid_CountName.Visible = False

            End If

        End With
    End Sub

    Private Sub dgv_Count_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Count_details.CellLeave
        On Error Resume Next
        If FrmLdSTS = True Then Exit Sub
        With dgv_Count_details
            If IsNothing(.CurrentCell) Then Exit Sub
            If .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                Else
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
                End If
            End If
        End With
    End Sub



    Private Sub dgv_Count_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Count_details.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
    End Sub

    Private Sub dgv_Count_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Count_details.KeyUp
        Dim n As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            With dgv_Count_details

                n = .CurrentRow.Index

                If n = .Rows.Count - 1 Then
                    For i = 0 To .ColumnCount - 1
                        .Rows(n).Cells(i).Value = ""
                    Next

                Else
                    .Rows.RemoveAt(n)

                End If

            End With

        End If

    End Sub

    Private Sub dgv_Count_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Count_details.LostFocus
        On Error Resume Next
        If IsNothing(dgv_Count_details.CurrentCell) Then Exit Sub
        dgv_Count_details.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_Count_Details_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Count_details.RowsAdded
        Dim n As Integer
        If FrmLdSTS = True Then Exit Sub
        With dgv_Count_details
            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)
        End With
    End Sub






    Private Sub msk_date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_date.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        vmskOldText = ""
        vmskSelStrt = -1
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            vmskOldText = msk_date.Text
            vmskSelStrt = msk_date.SelectionStart
        End If
    End Sub


    Private Sub msk_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_date.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            msk_date.Text = Date.Today
        End If
        If IsDate(msk_date.Text) = True Then
            If e.KeyCode = 107 Then
                msk_date.Text = DateAdd("D", 1, Convert.ToDateTime(msk_date.Text))
            ElseIf e.KeyCode = 109 Then
                msk_date.Text = DateAdd("D", -1, Convert.ToDateTime(msk_date.Text))
            End If
        End If
        If e.KeyCode = 46 Or e.KeyCode = 8 Then

            Common_Procedures.maskEdit_Date_ON_DelBackSpace(sender, e, vmskOldText, vmskSelStrt)
        End If
    End Sub
    Private Sub dtp_Date_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtp_date.ValueChanged
        msk_date.Text = dtp_date.Text
    End Sub

    Private Sub dtp_Date_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_date.Enter
        msk_date.Focus()
        msk_date.SelectionStart = 0
    End Sub




    Private Sub dgv_EndsCount_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_EndsCount_Details.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Da3 As New SqlClient.SqlDataAdapter
        Dim Dt3 As New DataTable
        Dim Dt4 As New DataTable
        Dim Dt5 As New DataTable
        Dim Rect As Rectangle

        With dgv_EndsCount_Details

            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If


            If e.ColumnIndex = 1 Then

                If Cbo_Grid_EndsCountName.Visible = False Or Val(Cbo_Grid_EndsCountName.Tag) <> e.RowIndex Then

                    Cbo_Grid_EndsCountName.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select EndsCount_Name from EndsCount_Head order by EndsCount_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    Cbo_Grid_EndsCountName.DataSource = Dt1
                    Cbo_Grid_EndsCountName.DisplayMember = "EndsCount_Name"

                    Rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    Cbo_Grid_EndsCountName.Left = .Left + Rect.Left
                    Cbo_Grid_EndsCountName.Top = .Top + Rect.Top

                    Cbo_Grid_EndsCountName.Width = Rect.Width
                    Cbo_Grid_EndsCountName.Height = Rect.Height
                    Cbo_Grid_EndsCountName.Text = .CurrentCell.Value

                    Cbo_Grid_EndsCountName.Tag = Val(e.RowIndex)
                    Cbo_Grid_EndsCountName.Visible = True

                    Cbo_Grid_EndsCountName.BringToFront()
                    Cbo_Grid_EndsCountName.Focus()



                End If

            Else
                Cbo_Grid_EndsCountName.Visible = False

            End If

        End With

    End Sub


    Private Sub dgv_EndsCount_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_EndsCount_Details.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
    End Sub

    Private Sub dgv_EndsCount_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_EndsCount_Details.KeyUp
        Dim n As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            With dgv_EndsCount_Details

                n = .CurrentRow.Index

                If n = .Rows.Count - 1 Then
                    For i = 0 To .ColumnCount - 1
                        .Rows(n).Cells(i).Value = ""
                    Next

                Else
                    .Rows.RemoveAt(n)

                End If

            End With

        End If

    End Sub

    Private Sub dgv_EndsCount_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_EndsCount_Details.LostFocus
        On Error Resume Next
        If IsNothing(dgv_EndsCount_Details.CurrentCell) Then Exit Sub
        dgv_EndsCount_Details.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_EndsCount_Details_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_EndsCount_Details.RowsAdded
        Dim n As Integer

        With dgv_EndsCount_Details
            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)
        End With
    End Sub


    Private Sub dgv_Cloth_Details_CellEnter(sender As Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Cloth_Details.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Rect As Rectangle
        Dim LedID As Integer = 0
        Dim CloID As Integer = 0
        With dgv_Cloth_Details

            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If

            If e.ColumnIndex = 1 Then

                If cbo_Grid_ClothName.Visible = False Or Val(cbo_Grid_ClothName.Tag) <> e.RowIndex Then

                    cbo_Grid_ClothName.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Cloth_Name from Cloth_Head order by Cloth_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Grid_ClothName.DataSource = Dt1
                    cbo_Grid_ClothName.DisplayMember = "Cloth_Name"

                    Rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_ClothName.Left = .Left + Rect.Left
                    cbo_Grid_ClothName.Top = .Top + Rect.Top

                    cbo_Grid_ClothName.Width = Rect.Width
                    cbo_Grid_ClothName.Height = Rect.Height
                    cbo_Grid_ClothName.Text = .CurrentCell.Value

                    cbo_Grid_ClothName.Tag = Val(e.RowIndex)
                    cbo_Grid_ClothName.Visible = True

                    cbo_Grid_ClothName.BringToFront()
                    cbo_Grid_ClothName.Focus()



                End If

            Else
                cbo_Grid_ClothName.Visible = False

            End If

        End With
    End Sub


    Private Sub dgv_Cloth_Details_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles dgv_Cloth_Details.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
    End Sub

    Private Sub dgv_Cloth_Details_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles dgv_Cloth_Details.KeyUp
        Dim n As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            With dgv_Cloth_Details

                n = .CurrentRow.Index

                If n = .Rows.Count - 1 Then
                    For i = 0 To .ColumnCount - 1
                        .Rows(n).Cells(i).Value = ""
                    Next

                Else
                    .Rows.RemoveAt(n)

                End If



            End With

        End If
    End Sub

    Private Sub dgv_Cloth_Details_LostFocus(sender As Object, e As System.EventArgs) Handles dgv_Cloth_Details.LostFocus
        On Error Resume Next
        If Not IsNothing(dgv_Cloth_Details.CurrentCell) Then dgv_Cloth_Details.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_Cloth_Details_RowsAdded(sender As Object, e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Cloth_Details.RowsAdded
        Dim n As Integer
        If FrmLdSTS = True Then Exit Sub

        If IsNothing(dgv_Cloth_Details.CurrentCell) Then Exit Sub
        With dgv_Cloth_Details
            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)
        End With
    End Sub



    Private Sub cbo_Grid_COuntName_GotFocus(sender As Object, e As System.EventArgs) Handles Cbo_Grid_CountName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
    End Sub

    Private Sub cbo_Grid_COuntName_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Cbo_Grid_CountName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, Cbo_Grid_CountName, Nothing, Nothing, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

        With dgv_Count_details

            If (e.KeyValue = 38 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                If .CurrentCell.RowIndex <= 0 Then

                    If dgv_Cloth_Details.Rows.Count = 0 Then dgv_Cloth_Details.Rows.Add()
                    dgv_Cloth_Details.Focus()
                    dgv_Cloth_Details.CurrentCell = dgv_Cloth_Details.Rows(0).Cells(1)

                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(1)

                End If

            ElseIf (e.KeyValue = 40 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                If .CurrentRow.Index = .Rows.Count - 1 Then

                    If Trim(.Rows(.CurrentRow.Index).Cells(1).Value) = "" Then
                        If dgv_EndsCount_Details.Rows.Count = 0 Then dgv_EndsCount_Details.Rows.Add()
                        dgv_EndsCount_Details.Focus()
                        dgv_EndsCount_Details.CurrentCell = dgv_EndsCount_Details.Rows(0).Cells(1)

                    Else
                        .Rows.Add()
                        .Focus()
                        .CurrentCell = .Rows(.CurrentRow.Index + 1).Cells(1)

                    End If



                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentRow.Index + 1).Cells(1)

                End If

            End If

        End With
    End Sub

    Private Sub Cbo_Grid_CountName_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles Cbo_Grid_CountName.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        Dim vCloRate As Single = 0
        Dim trpt_Idno As Integer = 0

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, Cbo_Grid_CountName, Nothing, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            e.Handled = True

            With dgv_Count_details

                If .CurrentRow.Index = .Rows.Count - 1 Then

                    If Trim(.Rows(.CurrentRow.Index).Cells(1).Value) = "" Then
                        If dgv_EndsCount_Details.Rows.Count = 0 Then dgv_EndsCount_Details.Rows.Add()
                        dgv_EndsCount_Details.Focus()
                        dgv_EndsCount_Details.CurrentCell = dgv_EndsCount_Details.Rows(0).Cells(1)

                    Else

                        .Rows.Add()
                        .Focus()
                        .CurrentCell = .Rows(.CurrentRow.Index + 1).Cells(1)

                    End If

                Else

                    .CurrentCell = .Rows(.CurrentRow.Index + 1).Cells(1)

                End If

            End With

        End If
    End Sub

    Private Sub cbo_Grid_COuntName_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Cbo_Grid_CountName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Count_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = Cbo_Grid_CountName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub
    '-----

    Private Sub cbo_Grid_EndsCOuntName_GotFocus(sender As Object, e As System.EventArgs) Handles Cbo_Grid_EndsCountName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")
    End Sub

    Private Sub cbo_Grid_EndsCOuntName_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Cbo_Grid_EndsCountName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, Cbo_Grid_EndsCountName, Nothing, Nothing, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")

        With dgv_EndsCount_Details


            If (e.KeyValue = 38 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

                If .CurrentCell.RowIndex <= 0 Then
                    If dgv_Count_details.Rows.Count = 0 Then dgv_Count_details.Rows.Add()
                    dgv_Count_details.Focus()
                    dgv_Count_details.CurrentCell = dgv_Count_details.Rows(0).Cells(1)

                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(1)

                End If

            ElseIf (e.KeyValue = 40 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                If .CurrentRow.Index = .Rows.Count - 1 Then

                    If Trim(.Rows(.CurrentRow.Index).Cells(1).Value) = "" Then
                        If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                            save_record()
                        Else
                            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus() Else Msk_StartDate.Focus()
                        End If

                    Else
                        .Rows.Add()
                        .Focus()
                        .CurrentCell = .Rows(.CurrentRow.Index + 1).Cells(1)

                    End If


                Else

                    .CurrentCell = .Rows(.CurrentRow.Index + 1).Cells(1)

                End If

            End If


        End With

    End Sub

    Private Sub cbo_Grid_EndsCOuntName_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles Cbo_Grid_EndsCountName.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim vCloRate As Single = 0
        Dim trpt_Idno As Integer = 0

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, Cbo_Grid_EndsCountName, Nothing, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            e.Handled = True

            With dgv_EndsCount_Details

                If .CurrentRow.Index = .Rows.Count - 1 Then

                    If Trim(.Rows(.CurrentRow.Index).Cells(1).Value) = "" Then
                        If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                            save_record()
                        Else
                            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus() Else Msk_StartDate.Focus()
                        End If

                    Else
                        .Rows.Add()
                        .Focus()
                        .CurrentCell = .Rows(.CurrentRow.Index + 1).Cells(1)

                    End If


                Else

                    .CurrentCell = .Rows(.CurrentRow.Index + 1).Cells(1)

                End If

            End With

        End If

    End Sub

    Private Sub cbo_Grid_EndsCOuntName_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Cbo_Grid_EndsCountName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New EndsCount_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = Cbo_Grid_EndsCountName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub


    Private Sub cbo_Grid_ClothName_GotFocus(sender As Object, e As System.EventArgs) Handles cbo_Grid_ClothName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")
    End Sub

    Private Sub cbo_Grid_ClothName_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_ClothName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_ClothName, Nothing, Nothing, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")

        With dgv_Cloth_Details

            If (e.KeyValue = 38 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                If .CurrentCell.RowIndex <= 0 Then
                    txt_Particulars.Focus()

                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(1)

                End If

            ElseIf (e.KeyValue = 40 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                If .CurrentRow.Index = .Rows.Count - 1 Then

                    If Trim(.Rows(.CurrentRow.Index).Cells(1).Value) = "" Then
                        If dgv_Count_details.Rows.Count = 0 Then dgv_Count_details.Rows.Add()
                        dgv_Count_details.Focus()
                        dgv_Count_details.CurrentCell = dgv_Count_details.Rows(0).Cells(1)

                    Else
                        .Rows.Add()
                        .Focus()
                        .CurrentCell = .Rows(.CurrentRow.Index + 1).Cells(1)

                    End If



                Else

                    .CurrentCell = .Rows(.CurrentRow.Index + 1).Cells(1)

                End If

            End If

        End With
    End Sub

    Private Sub cbo_Grid_ClothName_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_ClothName.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        Dim vCloRate As Single = 0
        Dim trpt_Idno As Integer = 0

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_ClothName, Nothing, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            e.Handled = True

            With dgv_Cloth_Details

                If .CurrentRow.Index = .Rows.Count - 1 Then

                    If Trim(.Rows(.CurrentRow.Index).Cells(1).Value) = "" Then
                        If dgv_Count_details.Rows.Count = 0 Then dgv_Count_details.Rows.Add()
                        dgv_Count_details.Focus()
                        dgv_Count_details.CurrentCell = dgv_Count_details.Rows(0).Cells(1)

                    Else
                        .Rows.Add()
                        .Focus()
                        .CurrentCell = .Rows(.CurrentRow.Index + 1).Cells(1)

                    End If



                Else

                    .CurrentCell = .Rows(.CurrentRow.Index + 1).Cells(1)

                End If

            End With

        End If

    End Sub

    Private Sub cbo_Grid_ClothName_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_ClothName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Cloth_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_ClothName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub




    Private Sub Cbo_Grid_CountName_TextChanged(sender As Object, e As System.EventArgs) Handles Cbo_Grid_CountName.TextChanged
        Try
            If Cbo_Grid_CountName.Visible Then

                If IsNothing(dgv_Count_details.CurrentCell) Then Exit Sub

                With dgv_Count_details
                    If Val(Cbo_Grid_CountName.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(Cbo_Grid_CountName.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub Cbo_Grid_EndsCountName_TextChanged(sender As Object, e As System.EventArgs) Handles Cbo_Grid_EndsCountName.TextChanged
        Try
            If Cbo_Grid_EndsCountName.Visible Then

                If IsNothing(dgv_EndsCount_Details.CurrentCell) Then Exit Sub

                With dgv_EndsCount_Details
                    If Val(Cbo_Grid_EndsCountName.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(Cbo_Grid_EndsCountName.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub
    Private Sub cbo_Grid_ClothName_TextChanged(sender As Object, e As System.EventArgs) Handles cbo_Grid_ClothName.TextChanged
        Try

            If FrmLdSTS = True Then Exit Sub

            If cbo_Grid_ClothName.Visible Then

                If IsNothing(dgv_Cloth_Details.CurrentCell) Then Exit Sub

                With dgv_Cloth_Details
                    If Val(cbo_Grid_ClothName.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_ClothName.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub Msk_StartDate_KeyDown(sender As Object, e As KeyEventArgs) Handles Msk_StartDate.KeyDown
        If e.KeyValue = 38 Then
            e.Handled = True
            e.SuppressKeyPress = True
            If cbo_WeaverName.Enabled And cbo_WeaverName.Visible Then
                cbo_WeaverName.Focus()
            Else
                msk_date.Focus()
            End If

        ElseIf e.KeyValue = 40 Then
            e.Handled = True
            e.SuppressKeyPress = True
            Msk_EndDate.Focus()

        End If
    End Sub

    Private Sub Msk_StartDate_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Msk_StartDate.KeyPress

        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            Msk_EndDate.Focus()
        End If
    End Sub

    Private Sub Msk_EndDate_KeyDown(sender As Object, e As KeyEventArgs) Handles Msk_EndDate.KeyDown

        If e.KeyValue = 38 Then
            e.Handled = True
            e.SuppressKeyPress = True
            Msk_StartDate.Focus()

        ElseIf e.KeyValue = 40 Then
            e.Handled = True
            e.SuppressKeyPress = True
            txt_Particulars.Focus()


        End If
    End Sub

    Private Sub Msk_EndDate_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Msk_EndDate.KeyPress

        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            txt_Particulars.Focus()

        End If
    End Sub

    Private Sub txt_Particulars_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Particulars.KeyDown

        If e.KeyValue = 38 Then
            e.Handled = True
            e.SuppressKeyPress = True
            Msk_EndDate.Focus()

        ElseIf e.KeyValue = 40 Then
            e.Handled = True
            e.SuppressKeyPress = True
            If dgv_Cloth_Details.Rows.Count <= 0 Then dgv_Cloth_Details.Rows.Add()
            dgv_Cloth_Details.Focus()
            dgv_Cloth_Details.CurrentCell = dgv_Cloth_Details.Rows(0).Cells(1)

        End If



    End Sub

    Private Sub txt_Particulars_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_Particulars.KeyPress
        If Asc(e.KeyChar) = 13 Then

            If dgv_Cloth_Details.Rows.Count > 0 Then
                dgv_Cloth_Details.Focus()
                dgv_Cloth_Details.CurrentCell = dgv_Cloth_Details.Rows(0).Cells(1)
            End If

        End If

    End Sub

    Private Sub dtp_StartDate_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtp_StartDate.ValueChanged
        Msk_StartDate.Text = dtp_StartDate.Text
    End Sub

    Private Sub dtp_StartDate_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_StartDate.Enter
        Msk_StartDate.Focus()
        Msk_StartDate.SelectionStart = 0
    End Sub
    Private Sub dtp_Enddate_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtp_Enddate.ValueChanged
        Msk_EndDate.Text = dtp_Enddate.Text
    End Sub

    Private Sub dtp_Enddate_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_Enddate.Enter
        Msk_EndDate.Focus()
        Msk_EndDate.SelectionStart = 0
    End Sub


End Class