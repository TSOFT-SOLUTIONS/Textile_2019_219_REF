Public Class Sort_Change_Entry_Multi
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "SRCML-"
    Private NoCalc_Status As Boolean = False
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Private vCbo_ItmNm As String

    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl

    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
    Private prn_DetAr(50, 10) As String
    Private prn_DetMxIndx As Integer
    Private prn_NoofBmDets As Integer
    Private prn_DetSNo As Integer
    Private PrntCnt2ndPageSTS As Boolean = False
    Private prn_DetDt1 As New DataTable
    Private prn_PageNo1 As Integer
    Private prn_DetIndx1 As Integer
    Private prn_NoofBmDets1 As Integer
    Private prn_DetSNo1 As Integer
    Private prn_TotCopies As Integer = 0
    Private prn_Count As Integer = 0
    Private prn_HeadIndx As Integer
    Private prn_Prev_HeadIndx As Integer
    Private Prnt_HalfSheet_STS As Boolean = False
    Private vPrnt_2Copy_In_SinglePage As Integer = 0
    Public vmskOldText As String = ""
    Public vmskSelStrt As Integer = -1

    Private Enum DgvCol_Details As Integer
        SNO                 '0
        LOOM_NO             '1
        PARTY_NAME          '2
        KNOT_NO             '3
        ENDS_COUNT          '4
        SET_NO_1            '5
        BEAM_NO_1           '6
        SET_NO_2            '7
        BEAM_NO_2           '8
        TOTAL_MTRS          '9
        BAL_MTRS            '10
        BEAM_KNOTTING_CODE  '11
        SET_CODE_1          '12
        SET_CODE_2          '13
    End Enum

    Private Sub clear()
        New_Entry = False
        Insert_Entry = False


        pnl_Back.Enabled = True
        pnl_Selection.Visible = False
        chk_SelectAll.Checked = False

        lbl_RefNo.Text = ""
        lbl_RefNo.ForeColor = Color.Black
        msk_date.Text = ""
        dtp_Date.Text = ""
        cbo_ClothName1.Text = ""
        cbo_ClothName2.Text = ""
        cbo_ClothName3.Text = ""
        cbo_ClothName4.Text = ""


        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))

        If cbo_WidthType.Visible Then cbo_WidthType.Text = ""

        dgv_Details.Rows.Clear()
        dgv_Details.Rows.Add()
        Cbo_Grid_Loom_No.Text = ""


        If Filter_Status = False Then
            'dtp_Filter_Fromdate.Text = ""
            'dtp_Filter_ToDate.Text = ""
            'cbo_Filter_PartyName.Text = ""
            'cbo_Filter_EndsCountName.Text = ""

            'dgv_Filter_Details.Rows.Clear()
        End If
        'pnl_Delivery_Selection.Visible = False
        'lbl_Delivery_Code.Text = ""
        'cbo_Type.Text = "DIRECT"
    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox
        Dim Msktxbx As MaskedTextBox

        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Me.ActiveControl Is MaskedTextBox Or TypeOf Me.ActiveControl Is Button Then
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

        If Me.ActiveControl.Name <> Cbo_Grid_Loom_No.Name Then
            Cbo_Grid_Loom_No.Visible = False
        End If
        If Me.ActiveControl.Name <> dgv_Details.Name Then
            Grid_DeSelect()
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

    Private Sub Grid_DeSelect()
        On Error Resume Next
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False

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
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False

        'If Not IsNothing(dgv_Filter_Details.CurrentCell) Then dgv_Filter_Details.CurrentCell.Selected = False
    End Sub

    Private Sub Weaver_Pavu_Receipt_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Activated

        Try
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_ClothName1.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CLOTH" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_ClothName1.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_ClothName2.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CLOTH" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_ClothName2.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_ClothName3.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CLOTH" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_ClothName3.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_ClothName4.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CLOTH" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_ClothName4.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(Cbo_Grid_Loom_No.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LOOMNO" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                Cbo_Grid_Loom_No.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

                Me.Text = lbl_Heading.Text & "  -  " & lbl_Company.Text

                new_record()

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        FrmLdSTS = False

    End Sub

    Private Sub Weaver_Pavu_Receipt_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim dt5 As New DataTable
        Dim dt6 As New DataTable
        Dim dt7 As New DataTable
        Dim dt8 As New DataTable

        Me.Text = ""

        con.Open()

        da = New SqlClient.SqlDataAdapter("select Vechile_No from Weaver_Pavu_Receipt_Head order by Vechile_No", con)
        da.Fill(dt7)
        'cbo_VehicleNo.DataSource = dt7
        'cbo_VehicleNo.DisplayMember = "Vechile_No"

        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead Where (ledger_type = 'TRANSPORT' or Ledger_IdNo = 0) order by Ledger_DisplayName", con)
        da.Fill(dt2)
        'cbo_Transport.DataSource = dt2
        'cbo_Transport.DisplayMember = "Ledger_DisplayName"

        da = New SqlClient.SqlDataAdapter("select distinct(EndsCount_Name) from EndsCount_Head order by EndsCount_Name", con)
        da.Fill(dt3)
        'cbo_EndsCount.DataSource = dt3
        'cbo_EndsCount.DisplayMember = "EndsCount_Name"


        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or Ledger_Type = 'WEAVER' or Ledger_Type = 'SIZING' or Ledger_Type = 'JOBWORKER') and Close_status = 0 order by Ledger_DisplayName", con)
        da.Fill(dt8)
        'cbo_RecForm.DataSource = dt8
        'cbo_RecForm.DisplayMember = "Ledger_DisplayName"

        'lbl_Bobin.Visible = False
        'txt_NoOfBobin.Visible = False
        If Common_Procedures.settings.Weaver_Zari_Kuri_Entries_Status = 1 Then
            'lbl_Bobin.Visible = True
            'txt_NoOfBobin.Visible = True
        End If
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1235" Then '---- SOMANUR KALPANA COTTON (INDIA) PVT LTD (SOMANUR)  --TETILES
            dgv_Details.Columns(DgvCol_Details.Beam_No_1).HeaderText = "YARDS"

        End If

        If Common_Procedures.settings.Textile_Sizing_Delivery_receipt_Selection = 1 Then
            Label5.Location = New Point(240, 12)
            Label17.Location = New Point(272, 12)
            msk_date.Location = New Point(318, 8)
            dtp_Date.Location = New Point(435, 8)
            lbl_RefNo.Width = 124


        End If


        If Common_Procedures.settings.User_Modifications_Show_Status = 1 Then
            If Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_UserModification_Status = 1 Then

            End If
        End If
        cbo_WidthType.Items.Clear()
        cbo_WidthType.Items.Add("")
        cbo_WidthType.Items.Add("SINGLE")
        cbo_WidthType.Items.Add("DOUBLE")
        cbo_WidthType.Items.Add("TRIPLE")
        cbo_WidthType.Items.Add("FOURTH")

        'cbo_Type.Items.Clear()
        'cbo_Type.Items.Add(" ")
        'cbo_Type.Items.Add("DIRECT")
        'cbo_Type.Items.Add("DELIVERY")

        dtp_Date.Text = ""
        msk_date.Text = ""
        'pnl_Filter.Visible = False
        'pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        'pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2

        pnl_Selection.Visible = False
        pnl_Selection.Left = (Me.Width - pnl_Selection.Width) \ 2
        pnl_Selection.Top = (Me.Height - pnl_Selection.Height) \ 2

        'pnl_Delivery_Selection.Visible = False
        'pnl_Delivery_Selection.Left = (Me.Width - pnl_Delivery_Selection.Width) \ 2
        'pnl_Delivery_Selection.Top = (Me.Height - pnl_Delivery_Selection.Height) \ 2
        'pnl_Delivery_Selection.BringToFront()


        'cbo_WidthType.Visible = False
        'lbl_Widthtype.Visible = False
        'If Common_Procedures.settings.AutoLoom_PavuWidthWiseConsumption_IN_Delivery = 1 Then
        cbo_WidthType.Visible = True
        lbl_Widthtype.Visible = True
        'End If

        'pnl_OwnOrderSelection.Visible = False
        'pnl_OwnOrderSelection.Left = (Me.Width - pnl_OwnOrderSelection.Width) \ 2
        'pnl_OwnOrderSelection.Top = (Me.Height - pnl_OwnOrderSelection.Height) \ 2
        'pnl_OwnOrderSelection.BringToFront()



        'chk_Verified_Status.Visible = False
        If Common_Procedures.settings.Vefified_Status = 1 Then
            If Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_Verified_Status = 1 Then
                'chk_Verified_Status.Visible = True
                'lbl_verfied_sts.Visible = True
                'cbo_Verified_Sts.Visible = True


            End If

        Else
            'chk_Verified_Status.Visible = False
            'lbl_verfied_sts.Visible = False
            'cbo_Verified_Sts.Visible = False

        End If





        AddHandler msk_date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_WidthType.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ClothName1.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ClothName2.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ClothName3.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ClothName4.GotFocus, AddressOf ControlGotFocus
        AddHandler Cbo_Grid_Loom_No.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_save.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_close.GotFocus, AddressOf ControlGotFocus


        AddHandler msk_date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_WidthType.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ClothName1.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ClothName2.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ClothName3.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ClothName4.LostFocus, AddressOf ControlLostFocus
        AddHandler Cbo_Grid_Loom_No.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_save.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_close.LostFocus, AddressOf ControlLostFocus


        AddHandler msk_date.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler msk_date.KeyPress, AddressOf TextBoxControlKeyPress




        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True
        new_record()

    End Sub

    Private Sub Weaver_Pavu_Receipt_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub Weaver_Pavu_Receipt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress

        Try

            If Asc(e.KeyChar) = 27 Then

                'If pnl_Filter.Visible = True Then
                '    btn_Filter_Close_Click(sender, e)
                '    Exit Sub
                'ElseIf pnl_Selection.Visible = True Then
                '    btn_Close_Selection_Click(sender, e)
                '    Exit Sub
                'ElseIf pnl_Delivery_Selection.Visible = True Then
                '    btn_Close_Delivery_Selection_Click(sender, e)
                '    Exit Sub
                'Else
                Close_Form()
                'End If

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
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean

        Dim dgv1 As New DataGridView

        If ActiveControl.Name = dgv_Details.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            On Error Resume Next

            dgv1 = Nothing

            If ActiveControl.Name = dgv_Details.Name Then
                dgv1 = dgv_Details

            ElseIf dgv_Details.IsCurrentRowDirty = True Then
                dgv1 = dgv_Details

            Else
                dgv1 = dgv_Details

            End If

            If IsNothing(dgv1) = False Then

                With dgv1

                    If dgv1.Name = dgv_Details.Name Then

                        If keyData = Keys.Enter Or keyData = Keys.Down Then

                            If .CurrentCell.ColumnIndex >= 1 Then

                                If .CurrentCell.RowIndex = .RowCount - 1 Then

                                    If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                                        save_record()
                                    Else
                                        msk_date.Focus()
                                    End If

                                Else

                                    If dgv_Details.Columns(DgvCol_Details.SNO).ReadOnly = False Then
                                        .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(DgvCol_Details.SNO)
                                    Else
                                        .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(DgvCol_Details.LOOM_NO)
                                    End If

                                End If

                            Else

                                If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 0 And ((Trim(.CurrentRow.Cells(DgvCol_Details.SNO).Value) = "" Or Trim(.CurrentRow.Cells(DgvCol_Details.SNO).Value) = "0") And Val(.CurrentRow.Cells(DgvCol_Details.PARTY_NAME).Value) = 0) Then
                                    If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                                        save_record()
                                    Else
                                        msk_date.Focus()
                                    End If

                                ElseIf .CurrentCell.RowIndex = .RowCount - 2 And .CurrentCell.ColumnIndex >= 0 And ((Trim(.CurrentRow.Cells(DgvCol_Details.SNO).Value) = "" Or Trim(.CurrentRow.Cells(DgvCol_Details.SNO).Value) = "0") And Val(.CurrentRow.Cells(DgvCol_Details.PARTY_NAME).Value) = 0 And (Trim(.Rows(.RowCount - 1).Cells(DgvCol_Details.SNO).Value) = "" Or Trim(.Rows(.RowCount - 1).Cells(DgvCol_Details.SNO).Value) = "0")) And Val(.Rows(.RowCount - 1).Cells(DgvCol_Details.PARTY_NAME).Value) = 0 Then
                                    If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                                        save_record()
                                    Else
                                        msk_date.Focus()
                                    End If

                                Else
                                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                                End If


                            End If

                            Return True

                        ElseIf keyData = Keys.Up Then

                            If .CurrentCell.ColumnIndex <= 1 Then
                                If .CurrentCell.RowIndex = 0 Then
                                    If cbo_ClothName1.Enabled And cbo_ClothName1.Visible Then
                                        cbo_ClothName1.Focus()
                                    ElseIf cbo_ClothName2.Enabled And cbo_ClothName2.Visible Then
                                        cbo_ClothName2.Focus()
                                    ElseIf cbo_ClothName3.Enabled And cbo_ClothName3.Visible Then
                                        cbo_ClothName3.Focus()
                                    ElseIf cbo_ClothName4.Enabled And cbo_ClothName4.Visible Then
                                        cbo_ClothName4.Focus()
                                    Else
                                        msk_date.Focus()
                                    End If

                                Else
                                    .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(DgvCol_Details.LOOM_NO)

                                End If

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)

                            End If

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

    End Function

    Private Sub move_record(ByVal no As String)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim NewCode As String
        Dim n As Integer
        Dim SNo As Integer
        Dim LockSTS As Boolean = False

        Dim vLedNm As String



        If Val(no) = 0 Then Exit Sub

        clear()

        NewCode = Trim(Pk_Condition) & Val(lbl_Company.Tag) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try







            'da1 = New SqlClient.SqlDataAdapter("select a.*,  c.Cloth_Name, e.Cloth_Name as Cloth_Name2, f.Cloth_Name as Cloth_Name3, g.Cloth_Name as Cloth_Name4, d.Loom_Name from Sort_Change_Head a  INNER JOIN Cloth_Head c ON a.Display_Cloth_Idno1 = c.Cloth_IdNo LEFT OUTER JOIN Cloth_Head e ON a.Display_Cloth_Idno2 = e.Cloth_IdNo Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Sort_Change_Code = '" & Trim(NewCode) & "'", con)

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name, c.Cloth_Name, e.Cloth_Name as Cloth_Name2, f.Cloth_Name as Cloth_Name3, g.Cloth_Name as Cloth_Name4, d.Loom_Name from Sort_Change_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo INNER JOIN Cloth_Head c ON a.Display_Cloth_Idno1 = c.Cloth_IdNo LEFT OUTER JOIN Cloth_Head e ON a.Display_Cloth_Idno2 = e.Cloth_IdNo LEFT OUTER JOIN Cloth_Head f ON a.Display_Cloth_Idno3 = f.Cloth_IdNo LEFT OUTER JOIN Cloth_Head g ON a.Display_Cloth_Idno4 = g.Cloth_IdNo LEFT OUTER JOIN Loom_Head d ON a.Loom_IdNo = d.Loom_IdNo Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Sort_Change_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)
            'dt1.Rows(0).Item("Cloth_Name").ToString()

            If dt1.Rows.Count > 0 Then

                lbl_RefNo.Text = dt1.Rows(0).Item("Sort_Change_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Sort_Change_Date").ToString
                cbo_WidthType.Text = dt1.Rows(0).Item("Width_Type").ToString
                cbo_ClothName1.Text = Common_Procedures.Cloth_IdNoToName(con, Trim(dt1.Rows(0).Item("Cloth_idno1").ToString))
                cbo_ClothName2.Text = Common_Procedures.Cloth_IdNoToName(con, Trim(dt1.Rows(0).Item("Cloth_idno2").ToString))
                cbo_ClothName3.Text = Common_Procedures.Cloth_IdNoToName(con, Trim(dt1.Rows(0).Item("Cloth_idno3").ToString))
                cbo_ClothName4.Text = Common_Procedures.Cloth_IdNoToName(con, Trim(dt1.Rows(0).Item("Cloth_idno4").ToString))

                lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.ledger_name , c.Loom_Name, d.EndsCount_Name from Sort_change_multi_details a inner join ledger_head b on a.ledger_idno = b.ledger_idno INNER JOIN loom_Head c ON a.loom_IdNo = c.loom_IdNo  inner join EndsCount_Head d on a.EndsCount_IdNo = d.EndsCount_IdNo  where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.sort_change_Code = '" & Trim(NewCode) & "' Order by a.Sl_No", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                With dgv_Details

                    .Rows.Clear()

                    SNo = 0

                    If dt2.Rows.Count > 0 Then

                        For i = 0 To dt2.Rows.Count - 1

                            n = .Rows.Add()

                            SNo = SNo + 1

                            .Rows(n).Cells(DgvCol_Details.SNO).Value = Val(SNo)
                            .Rows(n).Cells(DgvCol_Details.LOOM_NO).Value = Trim(dt2.Rows(i).Item("Loom_Name").ToString)
                            .Rows(n).Cells(DgvCol_Details.PARTY_NAME).Value = Trim(dt2.Rows(i).Item("ledger_name").ToString)
                            .Rows(n).Cells(DgvCol_Details.KNOT_NO).Value = dt2.Rows(i).Item("Knot_No").ToString
                            .Rows(n).Cells(DgvCol_Details.ENDS_COUNT).Value = Trim(dt2.Rows(i).Item("EndsCount_Name").ToString)
                            .Rows(n).Cells(DgvCol_Details.SET_NO_1).Value = dt2.Rows(i).Item("Set_no1").ToString
                            .Rows(n).Cells(DgvCol_Details.BEAM_NO_1).Value = dt2.Rows(i).Item("Beam_No1").ToString
                            .Rows(n).Cells(DgvCol_Details.SET_NO_2).Value = dt2.Rows(i).Item("set_No2").ToString
                            .Rows(n).Cells(DgvCol_Details.BEAM_NO_2).Value = dt2.Rows(i).Item("Beam_No2").ToString
                            .Rows(n).Cells(DgvCol_Details.TOTAL_MTRS).Value = dt2.Rows(i).Item("Total_Meters").ToString
                            .Rows(n).Cells(DgvCol_Details.BAL_MTRS).Value = dt2.Rows(i).Item("bal_meters").ToString
                            .Rows(n).Cells(DgvCol_Details.BEAM_KNOTTING_CODE).Value = dt2.Rows(i).Item("Beam_Knotting_Code").ToString
                            .Rows(n).Cells(DgvCol_Details.SET_CODE_1).Value = dt2.Rows(i).Item("Set_Code1").ToString
                            .Rows(n).Cells(DgvCol_Details.SET_CODE_2).Value = dt2.Rows(i).Item("Set_Code2").ToString

                            'If Val(.Rows(n).Cells(DgvCol_Details.BEAM_KNOTTING_CODE).Value) > 0 And Val(.Rows(n).Cells(DgvCol_Details.BEAM_KNOTTING_CODE).Value) <> Val(.Rows(n).Cells(DgvCol_Details.SET_CODE_2).Value) Then

                            '    LockSTS = True

                            '    .Rows(n).Cells(DgvCol_Details.BAL_MTRS).Value = "1"

                            '    For j = 0 To .ColumnCount - 1
                            '        .Rows(n).Cells(j).Style.ForeColor = Color.Red
                            '        .Rows(n).Cells(j).Style.BackColor = Color.LightGray
                            '    Next

                            'End If

                        Next i

                    End If

                End With
                dt2.Clear()

                n = dgv_Details.Rows.Add()

            Else

                new_record()

            End If

            dt1.Clear()
            dt1.Dispose()
            da1.Dispose()



            Grid_Cell_DeSelect()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If msk_date.Visible And msk_date.Enabled Then msk_date.Focus()

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim Nr As Integer = 0
        Dim vOrdByNo As String = ""

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text)

        NewCode = Trim(Pk_Condition) & Val(lbl_Company.Tag) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.Weaver_Pavu_Rceipt_Entry, New_Entry, Me, con, "Weaver_Pavu_Receipt_Head", "Weaver_Pavu_Receipt_Code", NewCode, "Weaver_Pavu_Receipt_Date", "(Weaver_Pavu_Receipt_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        trans = con.BeginTransaction

        Try

            NewCode = Trim(Pk_Condition) & Val(lbl_Company.Tag) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.Transaction = trans

            cmd.CommandText = "Update Beam_Knotting_Head Set Sort_Change_No = b.Last_Sort_Change_No , Sort_Change_Code = b.Last_Sort_Change_Code, Width_Type = c.Width_Type  , Cloth_Idno1 = c.Cloth_Idno1 , Cloth_Idno2 = c.Cloth_Idno2 , Cloth_Idno3 = c.Cloth_Idno3 , Cloth_Idno4 = c.Cloth_Idno4 from Beam_Knotting_Head a INNER JOIN Sort_Change_Multi_Details b ON a.Beam_Knotting_Code = b.Beam_Knotting_Code and a.Sort_Change_Code = b.Sort_Change_Code and a.Loom_Idno = b.Loom_Idno INNER JOIN Sort_Change_Head c ON b.Sort_Change_Code = c.Sort_Change_Code Where a.Sort_Change_Code = '" & Trim(NewCode) & "' and a.Beam_RunOut_Code = ''"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Sort_Change_Multi_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sort_Change_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Sort_Change_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sort_Change_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()


            trans.Commit()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception
            trans.Rollback()
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally

            Dt1.Dispose()
            Da.Dispose()
            trans.Dispose()
            cmd.Dispose()

            If msk_date.Enabled = True And msk_date.Visible = True Then msk_date.Focus()

        End Try

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        If Filter_Status = False Then

            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable
            Dim dt2 As New DataTable
            Dim dt3 As New DataTable

            da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where  (Ledger_IdNo = 0 or Ledger_Type = 'WEAVER' or Ledger_Type = 'SIZING' or Ledger_Type = 'JOBWORKER')  order by Ledger_DisplayName", con)
            da.Fill(dt1)
            'cbo_Filter_PartyName.DataSource = dt1
            'cbo_Filter_PartyName.DisplayMember = "Ledger_DisplayName"


            da = New SqlClient.SqlDataAdapter("select distinct(EndsCount_Name) from EndsCount_Head order by EndsCount_Name", con)
            da.Fill(dt3)
            'cbo_Filter_EndsCountName.DataSource = dt3
            'cbo_Filter_EndsCountName.DisplayMember = "EndsCount_Name"

            'dtp_Filter_Fromdate.Text = ""
            'dtp_Filter_ToDate.Text = ""
            'cbo_Filter_PartyName.Text = ""

            'cbo_Filter_EndsCountName.Text = ""

            'cbo_Filter_PartyName.SelectedIndex = -1

            'cbo_Filter_EndsCountName.SelectedIndex = -1

            'dgv_Filter_Details.Rows.Clear()

        End If

        'pnl_Filter.Visible = True
        'pnl_Filter.Enabled = True
        'pnl_Filter.BringToFront()
        'pnl_Back.Enabled = False
        'If dtp_Filter_Fromdate.Enabled And dtp_Filter_Fromdate.Visible Then dtp_Filter_Fromdate.Focus()

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String

        Try


            da = New SqlClient.SqlDataAdapter("select top 1 Sort_Change_No from Sort_Change_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sort_Change_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Sort_Change_No", con)



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


            da = New SqlClient.SqlDataAdapter("select top 1 Sort_Change_No from Sort_Change_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sort_Change_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Sort_Change_No", con)

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



            da = New SqlClient.SqlDataAdapter("select top 1 Sort_Change_No from Sort_Change_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sort_Change_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Sort_Change_No desc", con)
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



            da = New SqlClient.SqlDataAdapter("select top 1 Sort_Change_No from Sort_CHange_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sort_Change_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Sort_Change_No desc", con)
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
        Dim dt1 As New DataTable
        Dim NewID As Integer = 0

        Try

            clear()

            New_Entry = True


            lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Sort_Change_Head", "Sort_Change_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

            lbl_RefNo.ForeColor = Color.Red

            msk_date.Text = Date.Today.ToShortDateString


            da = New SqlClient.SqlDataAdapter("select top 1 * from Sort_Change_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sort_Change_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Sort_Change_No desc", con)


            dt1 = New DataTable
            da.Fill(dt1)
            If dt1.Rows.Count > 0 Then
                If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)

                    If dt1.Rows(0).Item("Sort_Change_Date").ToString <> "" Then msk_date.Text = dt1.Rows(0).Item("Sort_Change_Date").ToString
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
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim RecCode As String

        Try

            inpno = InputBox("Enter Rec.No.", "FOR FINDING...")



            RecCode = Trim(Pk_Condition) & Val(lbl_Company.Tag) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Sort_Change_No from Sort_Change_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sort_Change_Code = '" & Trim(RecCode) & "'", con)
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
                MessageBox.Show("Rec No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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


        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.Weaver_Pavu_Rceipt_Entry, New_Entry, Me) = False Then Exit Sub

        Try

            inpno = InputBox("Enter New Rec No.", "FOR NEW RECEIPT INSERTION...")

            RecCode = Trim(Pk_Condition) & Val(lbl_Company.Tag) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)



            Da = New SqlClient.SqlDataAdapter("select Sort_Change_No from Sort_Change_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sort_Change_Code = '" & Trim(RecCode) & "'", con)
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
                    MessageBox.Show("Invalid Recc No", "DOES NOT INSERT NEW RECEIPT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim vForOrdByNo As Single = 0
        Dim Trans_ID As Integer = 0
        Dim KuPvu_EdsCnt_ID As Integer = 0
        Dim SzPvu_EdsCnt_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim Nr As Integer = 0
        Dim Partcls As String = ""
        Dim PBlNo As String = ""
        Dim vTotPvuBms As Single, vTotPvuMtrs As Single, vTotPvuPcs As Single, vTotPvuRctMtrs As Single, vTotPvuRctPcs As Single
        Dim Delv_ID As Integer = 0
        Dim Rec_ID As Integer = 0
        Dim Siz_ID As Integer = 0
        Dim vEdsCnt_ID As Integer = 0
        Dim EntID As String = ""
        Dim Bw_IdNo As Integer = 0
        Dim Pavu_DelvInc As Integer = 0
        Dim Ent_NoofUsed As Integer = 0
        Dim pCnt_ID As Integer = 0
        Dim pEds_Nm As Integer = 0
        Dim Mtr_Pc As Single = 0
        Dim Selc_SetCode As String = ""
        Dim Bm_CompID As Integer = 0
        Dim New_BmNo As String = ""
        Dim vTotPvuStk As Single = 0, vTotPvuStkAlLoomMtr As Single = 0
        Dim vPREV_SORTCHNGCODE As String, vPREV_SORTCHNGNO As String
        Dim vWdTyp As Single = 0
        Dim Delv_Ledtype As String = ""
        Dim Rec_Ledtype As String = ""
        Dim vPVUSTK_ENDSID As Integer = 0
        Dim vSELC_DCCODE As String = ""
        Dim Verified_STS As String = ""
        Dim vOrdByNo As String = ""
        Dim vCOMP_LEDIDNO As Integer = 0
        Dim vDELVLED_COMPIDNO As Integer = 0
        Dim Clo_ID As Integer = 0
        Dim Clo_ID2 As Integer = 0
        Dim Clo_ID3 As Integer = 0
        Dim Clo_ID4 As Integer = 0
        Dim Lm_ID As Integer = 0
        Dim VLed_Idno As Integer = 0

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text)


        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        NewCode = Trim(Pk_Condition) & Val(lbl_Company.Tag) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)


        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Weaver_Pavu_Rceipt_Entry, New_Entry, Me, con, "Weaver_Pavu_Receipt_Head", "Weaver_Pavu_Receipt_Code", NewCode, "Weaver_Pavu_Receipt_Date", "(Weaver_Pavu_Receipt_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Pavu_Receipt_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, Weaver_Pavu_Receipt_No desc", dtp_Date.Value.Date) = False Then Exit Sub


        If Common_Procedures.settings.Vefified_Status = 1 Then
            If Not (Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_Verified_Status = 1) Then
                NewCode = Trim(Pk_Condition) & Val(lbl_Company.Tag) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

                If Val(Common_Procedures.get_FieldValue(con, "Weaver_Pavu_Receipt_Head", "Verified_Status", "(Weaver_Pavu_Receipt_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "')")) = 1 Then
                    MessageBox.Show("Entry Already Verified", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If


        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(msk_date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()
            Exit Sub
        End If

        If Not (Convert.ToDateTime(msk_date.Text) >= Common_Procedures.Company_FromDate And Convert.ToDateTime(msk_date.Text) <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()
            Exit Sub
        End If

        Delv_ID = Val(Common_Procedures.CommonLedger.Godown_Ac)

        lbl_UserName.Text = Common_Procedures.User.IdNo

        Verified_STS = 0

        If cbo_WidthType.Visible And cbo_WidthType.Text = "" Then
            MessageBox.Show("Invalid Width Type", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_WidthType.Enabled And cbo_WidthType.Visible Then cbo_WidthType.Focus()
            Exit Sub
        End If

        With dgv_Details

            Dim Dup_LMNo As String = ""
            Dup_LMNo = ""

            For i = 0 To .RowCount - 1

                If Trim(dgv_Details.Rows(i).Cells(DgvCol_Details.BEAM_KNOTTING_CODE).Value) <> "" Then

                    If Trim(.Rows(i).Cells(DgvCol_Details.LOOM_NO).Value) = "" Then
                        MessageBox.Show("Invalid Loom No", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(DgvCol_Details.LOOM_NO)
                            .CurrentCell.Selected = True
                        End If
                        Exit Sub
                    End If

                    'If Trim(.Rows(i).Cells(DgvCol_Details.SET_CODE_1).Value) = "" Then
                    '    MessageBox.Show("Invalid Set Code 1", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    '    If .Enabled And .Visible Then
                    '        .Focus()
                    '        .CurrentCell = .Rows(i).Cells(DgvCol_Details.LOOM_NO)
                    '        .CurrentCell.Selected = True
                    '    End If
                    '    Exit Sub
                    'End If

                    'If Trim(.Rows(i).Cells(DgvCol_Details.PARTY_NAME).Value) = "" Then
                    '    MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    '    If .Enabled And .Visible Then
                    '        .Focus()
                    '        .CurrentCell = .Rows(i).Cells(DgvCol_Details.PARTY_NAME)
                    '        .CurrentCell.Selected = True
                    '    End If
                    '    Exit Sub
                    'End If

                    'If Trim(.Rows(i).Cells(DgvCol_Details.KNOT_NO).Value) = "" Then
                    '    MessageBox.Show("Invalid knot No", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    '    If .Enabled And .Visible Then
                    '        .Focus()
                    '        .CurrentCell = .Rows(i).Cells(DgvCol_Details.KNOT_NO)
                    '        .CurrentCell.Selected = True
                    '    End If
                    '    Exit Sub
                    'End If

                    'vEdsCnt_ID = Common_Procedures.EndsCount_NameToIdNo(con, .Rows(i).Cells(DgvCol_Details.ENDS_COUNT).Value)
                    'If Val(vEdsCnt_ID) = 0 Then
                    '    MessageBox.Show("Invalid Ends Count", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    '    If .Enabled And .Visible Then
                    '        .Focus()
                    '        .CurrentCell = .Rows(i).Cells(DgvCol_Details.ENDS_COUNT)
                    '        .CurrentCell.Selected = True
                    '    End If
                    '    Exit Sub
                    'End If

                    'If Val(.Rows(i).Cells(DgvCol_Details.Beam_No_1).Value) = 0 Then
                    '    MessageBox.Show("Invalid Beam No 1", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    '    If .Enabled And .Visible Then
                    '        .Focus()
                    '        .CurrentCell = .Rows(i).Cells(DgvCol_Details.Beam_No_1)
                    '        .CurrentCell.Selected = True
                    '    End If
                    '    Exit Sub
                    'End If

                    'If Val(.Rows(i).Cells(DgvCol_Details.Beam_No_2).Value) = 0 Then
                    '    MessageBox.Show("Invalid Beam No 2", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    '    If .Enabled And .Visible Then
                    '        .Focus()
                    '        .CurrentCell = .Rows(i).Cells(DgvCol_Details.BEAM_NO_2)
                    '        .CurrentCell.Selected = True
                    '    End If
                    '    Exit Sub
                    'End If


                End If

                If Trim(dgv_Details.Rows(i).Cells(DgvCol_Details.LOOM_NO).Value) <> "" Then

                    If InStr(1, Trim(UCase(Dup_LMNo)), "~" & Trim(UCase(.Rows(i).Cells(DgvCol_Details.LOOM_NO).Value)) & "~") > 0 Then
                        MessageBox.Show("Duplicate LoomNo ", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(DgvCol_Details.LOOM_NO)
                            .CurrentCell.Selected = True
                        End If
                        Exit Sub
                    End If

                    Dup_LMNo = Trim(Dup_LMNo) & "~" & Trim(UCase(.Rows(i).Cells(DgvCol_Details.LOOM_NO).Value)) & "~"

                End If



            Next

        End With

        TotalPavu_Calculation()

        vTotPvuBms = 0 : vTotPvuMtrs = 0 : vTotPvuPcs = 0 : vTotPvuRctMtrs = 0 : vTotPvuRctPcs = 0


        Delv_Ledtype = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_Type", "(Ledger_IdNo = " & Str(Val(Delv_ID)) & ")")
        Rec_Ledtype = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_Type", "(Ledger_IdNo = " & Str(Val(Rec_ID)) & ")")


        vSELC_DCCODE = ""

        Clo_ID = Common_Procedures.Cloth_NameToIdNo(con, Trim(cbo_ClothName1.Text))

        Clo_ID2 = Common_Procedures.Cloth_NameToIdNo(con, Trim(cbo_ClothName2.Text))

        Clo_ID3 = Common_Procedures.Cloth_NameToIdNo(con, Trim(cbo_ClothName3.Text))

        Clo_ID4 = Common_Procedures.Cloth_NameToIdNo(con, Trim(cbo_ClothName4.Text))


        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Pk_Condition) & Val(lbl_Company.Tag) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else
                lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Sort_Change_Head", "Sort_Change_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)


                NewCode = Trim(Pk_Condition) & Val(lbl_Company.Tag) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            vForOrdByNo = Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@EntryDate", Convert.ToDateTime(msk_date.Text))


            If New_Entry = True Then


                cmd.CommandText = "Insert into Sort_Change_Head   (     Sort_Change_Code,           Company_IdNo      ,        Sort_Change_No      ,               for_OrderBy                 ,           Sort_Change_Date  ,                          Width_Type   ,          Cloth_Idno1  ,         Cloth_Idno2  ,          Cloth_Idno3    ,          Cloth_Idno4    ,                   Entry_Type            ) " &
                            "      Values                       ('" & Trim(NewCode) & "', " & Val(lbl_Company.Tag) & ", '" & Trim(lbl_RefNo.Text) & "',          " & Str(Val(vForOrdByNo)) & ",                @EntryDate    ,     '" & Trim(cbo_WidthType.Text) & "',    " & Val(Clo_ID) & ",  " & Val(Clo_ID2) & ",     " & Val(Clo_ID3) & ",    " & Val(Clo_ID4) & " ,                     'Multi'            ) "
                cmd.ExecuteNonQuery()

            Else

                cmd.CommandText = "Update Beam_Knotting_Head Set Sort_Change_No = b.Last_Sort_Change_No , Sort_Change_Code = b.Last_Sort_Change_Code, Width_Type = c.Width_Type  , Cloth_Idno1 = c.Cloth_Idno1 , Cloth_Idno2 = c.Cloth_Idno2 , Cloth_Idno3 = c.Cloth_Idno3 , Cloth_Idno4 = c.Cloth_Idno4 from Beam_Knotting_Head a INNER JOIN Sort_Change_Multi_Details b ON a.Beam_Knotting_Code = b.Beam_Knotting_Code and a.Sort_Change_Code = b.Sort_Change_Code and a.Loom_Idno = b.Loom_Idno INNER JOIN Sort_Change_Head c ON b.Sort_Change_Code = c.Sort_Change_Code Where a.Sort_Change_Code = '" & Trim(NewCode) & "' and a.Beam_RunOut_Code = ''"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update sort_Change_Head set  Sort_Change_Date = @EntryDate , Width_Type = '" & Trim(cbo_WidthType.Text) & "' , Cloth_Idno1 = " & Val(Clo_ID) & " , Cloth_Idno2 = " & Val(Clo_ID2) & " , Cloth_Idno3 = " & Val(Clo_ID3) & " , Cloth_Idno4 = " & Val(Clo_ID4) & " WHERE Sort_Change_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If

            cmd.CommandText = "delete from Sort_Change_Multi_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Sort_change_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()


            With dgv_Details

                Sno = 0

                For i = 0 To dgv_Details.RowCount - 1

                    If Trim(.Rows(i).Cells(DgvCol_Details.LOOM_NO).Value) <> "" Then

                        Lm_ID = Common_Procedures.Loom_NameToIdNo(con, Trim(.Rows(i).Cells(DgvCol_Details.LOOM_NO).Value), tr)

                        If Val(Lm_ID) <> 0 Then

                            Sno = Sno + 1

                            VLed_Idno = Common_Procedures.Ledger_NameToIdNo(con, Trim(.Rows(i).Cells(DgvCol_Details.PARTY_NAME).Value), tr)
                            vEdsCnt_ID = Common_Procedures.EndsCount_NameToIdNo(con, Trim(.Rows(i).Cells(DgvCol_Details.ENDS_COUNT).Value), tr)


                            vPREV_SORTCHNGCODE = ""
                            vPREV_SORTCHNGNO = ""
                            Da = New SqlClient.SqlDataAdapter("select * from Beam_Knotting_Head Where Beam_Knotting_Code = '" & Trim(dgv_Details.Rows(i).Cells(DgvCol_Details.BEAM_KNOTTING_CODE).Value) & "'", con)
                            Da.SelectCommand.Transaction = tr
                            Dt1 = New DataTable
                            Da.Fill(Dt1)
                            If Dt1.Rows.Count > 0 Then
                                If IsDBNull(Dt1.Rows(0).Item("Sort_Change_Code").ToString) = False Then
                                    vPREV_SORTCHNGCODE = Trim(Dt1.Rows(0).Item("Sort_Change_Code").ToString)
                                End If
                                If IsDBNull(Dt1.Rows(0).Item("Sort_Change_No").ToString) = False Then
                                    vPREV_SORTCHNGCODE = Trim(Dt1.Rows(0).Item("Sort_Change_No").ToString)
                                End If
                            End If
                            Dt1.Clear()


                            cmd.CommandText = "insert into Sort_Change_Multi_Details (    Sort_change_Code ,              company_idno ,               Sort_Change_No    ,                     for_OrderBy ,    Sort_Change_Date ,                              Sl_No                       ,        Loom_idNo      ,         Ledger_idno        ,                          Knot_No                            ,                                  Set_No1                           ,                             Beam_No1                            ,                                 Set_No2                         ,                                   Beam_No2                    ,           EndsCount_IdNo ,                      Total_Meters                            ,                           Bal_Meters                         ,                              Beam_Knotting_Code                         ,                             Set_Code1                            ,                             Set_Code2                             ,        Last_Sort_Change_Code       ,           Last_Sort_Change_No    ) " &
                                                                         " values ( '" & Trim(NewCode) & "', " & Val(lbl_Company.Tag) & ", '" & Trim(lbl_RefNo.Text) & "' ,   " & Str(Val(vForOrdByNo)) & " ,         @EntryDate  ,    " & Val(.Rows(i).Cells(DgvCol_Details.SNO).Value) & " ,   " & Val(Lm_ID) & " ,      " & Val(VLed_Idno) & ",   " & Val(.Rows(i).Cells(DgvCol_Details.KNOT_NO).Value) & " ,      '" & Trim(.Rows(i).Cells(DgvCol_Details.SET_NO_1).Value) & "' ,   '" & Trim(.Rows(i).Cells(DgvCol_Details.BEAM_NO_1).Value) & "',   '" & Trim(.Rows(i).Cells(DgvCol_Details.SET_NO_2).Value) & "' , '" & Trim(.Rows(i).Cells(DgvCol_Details.BEAM_NO_2).Value) & "',  " & Val(vEdsCnt_ID) & " ,  " & Val(.Rows(i).Cells(DgvCol_Details.TOTAL_MTRS).Value) & ",    " & Val(.Rows(i).Cells(DgvCol_Details.BAL_MTRS).Value) & ",  '" & Trim(.Rows(i).Cells(DgvCol_Details.BEAM_KNOTTING_CODE).Value) & "',   '" & Trim(.Rows(i).Cells(DgvCol_Details.SET_CODE_1).Value) & "',   '" & Trim(.Rows(i).Cells(DgvCol_Details.SET_CODE_2).Value) & "' , '" & Trim(vPREV_SORTCHNGCODE) & "' , '" & Trim(vPREV_SORTCHNGNO) & "' ) "
                            Nr = cmd.ExecuteNonQuery()


                            If Trim(.Rows(i).Cells(DgvCol_Details.BEAM_KNOTTING_CODE).Value) <> "" Then

                                Nr = 0
                                cmd.CommandText = "Update Beam_Knotting_Head Set Sort_Change_No = '" & Trim(lbl_RefNo.Text) & "' , Sort_Change_Code = '" & Trim(NewCode) & "' , Width_Type = '" & Trim(cbo_WidthType.Text) & "' ,  Cloth_Idno1 = " & Str(Val(Clo_ID)) & ",  Cloth_Idno2 = " & Str(Val(Clo_ID2)) & ",  Cloth_Idno3 = " & Str(Val(Clo_ID3)) & ", Cloth_Idno4 = " & Str(Val(Clo_ID4)) & " Where Loom_Idno = " & Str(Lm_ID) & " and Beam_Knotting_Code = '" & Trim(.Rows(i).Cells(DgvCol_Details.BEAM_KNOTTING_CODE).Value) & "' and Beam_RunOut_Code = ''"
                                Nr = cmd.ExecuteNonQuery()
                                If Nr = 0 Then
                                    Throw New ApplicationException("Saving : These Beams already runnot")
                                    Exit Sub
                                End If
                                If Nr > 1 Then
                                    Throw New ApplicationException("Saving : Error in Beam Knotting Updation")
                                    Exit Sub
                                End If

                            End If

                        End If

                    End If

                Next

            End With

            Dt1.Clear()
            tr.Commit()

            Dt1.Dispose()
            Da.Dispose()

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)

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
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()

        End Try

    End Sub

    Private Sub cbo_RecForm_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        If Common_Procedures.settings.Textile_Sizing_Delivery_receipt_Selection = 1 Then
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'WEAVER' or Ledger_Type = 'GODOWN' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
        Else
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'WEAVER' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
        End If

    End Sub

    Private Sub cbo_RecForm_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Common_Procedures.UR.Ledr_Wea_Siz_Rw_Trans_JbWrk_Creation = Common_Procedures.UR.Weaver_Creation
            Common_Procedures.MDI_LedType = "WEAVER"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            'Common_Procedures.Master_Return.Control_Name = cbo_RecForm.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub
    Private Sub cbo_EndsCount_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_idno = 0)")

    End Sub
    Private Sub cbo_EndsCount_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        'Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_EndsCount, cbo_RecForm, txt_KuraiPavuBeam, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_idno = 0)")
    End Sub

    Private Sub cbo_EndsCount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        'Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_EndsCount, txt_KuraiPavuBeam, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_idno = 0)")
    End Sub

    Private Sub cbo_EndsCount_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New EndsCount_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            'Common_Procedures.Master_Return.Control_Name = cbo_EndsCount.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub
    Private Sub cbo_Transport_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")

    End Sub


    Private Sub cbo_Transport_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.Control = False And e.KeyValue = 17 Then
            Common_Procedures.UR.Ledr_Wea_Siz_Rw_Trans_JbWrk_Creation = Common_Procedures.UR.Ledger_Creation
            Common_Procedures.MDI_LedType = "TRANSPORT"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            'Common_Procedures.Master_Return.Control_Name = cbo_Transport.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub




    Private Sub btn_Filter_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        pnl_Back.Enabled = True
        'pnl_Filter.Visible = False
        Filter_Status = False
    End Sub

    Private Sub Open_FilterEntry()
        Dim movno As String

        On Error Resume Next

        'movno = Trim(dgv_Filter_Details.CurrentRow.Cells(0).Value)

        If Val(movno) <> 0 Then
            Filter_Status = True
            move_record(movno)
            pnl_Back.Enabled = True
            'pnl_Filter.Visible = False
        End If

    End Sub
    Private Sub btn_Filter_Show_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim n As Integer, i As Integer
        Dim Led_IdNo As Integer, Cnt_IdNo As Integer
        Dim Condt As String = ""
        Dim EdsCnt_IdNo As Integer, Mil_IdNo As Integer
        Dim Verfied_Sts As Integer = 0
        Try

            Condt = ""
            Led_IdNo = 0
            Cnt_IdNo = 0
            Mil_IdNo = 0
            EdsCnt_IdNo = 0

            'If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
            '    Condt = "a.Weaver_Pavu_Receipt_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            'ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
            '    Condt = "a.Weaver_Pavu_Receipt_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            'ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
            '    Condt = "a.Weaver_Pavu_Receipt_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            'End If

            'If Trim(cbo_Filter_PartyName.Text) <> "" Then
            '    Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            'End If

            'If Trim(cbo_Filter_EndsCountName.Text) <> "" Then
            '    Cnt_IdNo = Common_Procedures.EndsCount_NameToIdNo(con, cbo_Filter_EndsCountName.Text)
            'End If

            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.ReceivedFrom_IdNo = " & Str(Val(Led_IdNo)) & " "
            End If

            If Val(Cnt_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.EndsCount_IdNo = " & Str(Val(Cnt_IdNo)) & " "
            End If

            'If Trim(cbo_Verified_Sts.Text) = "YES" Then
            '    Verfied_Sts = 1
            '    Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Weaver_Pavu_Receipt_Code IN ( select z2.Weaver_Pavu_Receipt_Code from Weaver_Pavu_Receipt_Head z2 where z2.Verified_Status = " & Str(Val(Verfied_Sts)) & " )"
            'ElseIf Trim(cbo_Verified_Sts.Text) = "NO" Then
            '    Verfied_Sts = 0
            '    Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Weaver_Pavu_Receipt_Code IN ( select z2.Weaver_Pavu_Receipt_Code from Weaver_Pavu_Receipt_Head z2 where z2.Verified_Status = " & Str(Val(Verfied_Sts)) & " )"
            'End If


            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name , c.EndsCount_Name from Weaver_Pavu_Receipt_Head a INNER JOIN Weaver_Pavu_Receipt_Details d on a.Weaver_Pavu_Receipt_Code = d.Weaver_Pavu_Receipt_Code INNER JOIN Ledger_Head b on a.ReceivedFrom_IdNo = b.Ledger_IdNo LEFT OUTER JOIN EndsCount_Head c on d.EndsCount_IdNo = c.EndsCount_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_Pavu_Receipt_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Weaver_Pavu_Receipt_No", con)
            da.Fill(dt2)

            'dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    'n = dgv_Filter_Details.Rows.Add()

                    'dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    'dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Weaver_Pavu_Receipt_No").ToString
                    'dgv_Filter_Details.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Weaver_Pavu_Receipt_Date").ToString), "dd-MM-yyyy")
                    'dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    'dgv_Filter_Details.Rows(n).Cells(4).Value = Val(dt2.Rows(i).Item("Total_Pcs").ToString)
                    'dgv_Filter_Details.Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("Total_Meters").ToString), "########0.00")
                    'dgv_Filter_Details.Rows(n).Cells(6).Value = dt2.Rows(i).Item("EndsCount_Name").ToString
                    'dgv_Filter_Details.Rows(n).Cells(7).Value = Val(dt2.Rows(i).Item("Total_Rcpt_Pcs").ToString)
                    'dgv_Filter_Details.Rows(n).Cells(8).Value = Format(Val(dt2.Rows(i).Item("Total_Rcpt_Meters").ToString), "########0.00")

                Next i

            End If

            dt2.Clear()
            dt2.Dispose()
            da.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FILTER...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        'If dgv_Filter_Details.Visible And dgv_Filter_Details.Enabled Then dgv_Filter_Details.Focus()

    End Sub

    Private Sub dgv_Filter_Details_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs)
        Open_FilterEntry()
    End Sub

    Private Sub dgv_Filter_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyCode = 13 Then
            Open_FilterEntry()
        End If
    End Sub
    Private Sub dtp_Filter_Fromdate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Asc(e.KeyChar) = 13 Then
            'dtp_Filter_ToDate.Focus()
        End If
    End Sub

    Private Sub dtp_Filter_ToDate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Asc(e.KeyChar) = 13 Then
            'cbo_Filter_PartyName.Focus()
        End If
    End Sub

    Private Sub cbo_Filter_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( Ledger_Type = 'WEAVER' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        'Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, dtp_Filter_ToDate, cbo_Filter_EndsCountName, "Ledger_AlaisHead", "Ledger_DisplayName", "( Ledger_Type = 'WEAVER' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        'Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, cbo_Filter_EndsCountName, "Ledger_AlaisHead", "Ledger_DisplayName", "( Ledger_Type = 'WEAVER' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_EndsCountName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_idno = 0)")
    End Sub

    Private Sub cbo_Filter_CountName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        'Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_EndsCountName, cbo_Filter_PartyName, Nothing, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")
        'If e.KeyValue = 40 Or (e.Control = True And e.KeyValue = 40) Then
        '    If Common_Procedures.settings.CustomerCode = "1249" Then
        '        cbo_Verified_Sts.Focus()
        '    Else
        '        btn_Filter_Show.Focus()

        '    End If

        'End If
    End Sub

    Private Sub cbo_Filter_CountName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        'Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_EndsCountName, Nothing, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            'If Common_Procedures.settings.CustomerCode = "1249" Then
            '    cbo_Verified_Sts.Focus()
            'Else
            '    btn_Filter_Show.Focus()

            'End If
        End If
    End Sub





    Private Sub dgv_Sort_Change_Entry_Multi_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEnter
        Dim Vrect As Rectangle

        With dgv_Details
            If Val(.CurrentRow.Cells(DgvCol_Details.SNO).Value) = 0 Then
                .CurrentRow.Cells(DgvCol_Details.SNO).Value = .CurrentRow.Index + 1
            End If

            If .CurrentCell.ColumnIndex = DgvCol_Details.LOOM_NO Then



                If Cbo_Grid_Loom_No.Visible = False Or Cbo_Grid_Loom_No.Tag <> e.RowIndex Then


                    Vrect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    Cbo_Grid_Loom_No.Width = Vrect.Width
                    Cbo_Grid_Loom_No.Left = .Left + Vrect.Left
                    Cbo_Grid_Loom_No.Top = .Top + Vrect.Top
                    Cbo_Grid_Loom_No.Visible = True

                    Cbo_Grid_Loom_No.Text = .CurrentCell.Value


                    Cbo_Grid_Loom_No.Tag = Val(e.RowIndex)
                    Cbo_Grid_Loom_No.Visible = True

                    Cbo_Grid_Loom_No.BringToFront()
                    Cbo_Grid_Loom_No.Focus()
                    Cbo_Grid_Loom_No.DropDownHeight = 300

                End If
            Else
                Cbo_Grid_Loom_No.Visible = False

            End If







        End With


    End Sub

    Private Sub dgv_PavuDetails_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellLeave
        'With dgv_Details
        '    If .CurrentCell.ColumnIndex = DgvCol_Details.SET_NO_1 Then
        '        If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
        '            .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
        '        End If
        '    End If
        'End With
    End Sub

    Private Sub dgv_PavuDetails_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged
        On Error Resume Next
        If IsNothing(dgv_Details.CurrentCell) Then Exit Sub

        With dgv_Details
            If .Visible Then
                If .CurrentCell.ColumnIndex = DgvCol_Details.ENDS_COUNT Or .CurrentCell.ColumnIndex = DgvCol_Details.Beam_No_1 Or .CurrentCell.ColumnIndex = DgvCol_Details.Set_No_2 Or .CurrentCell.ColumnIndex = DgvCol_Details.Beam_No_2 Then
                    TotalPavu_Calculation()
                End If
            End If
        End With

    End Sub

    Private Sub dgv_PavuDetails_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Details.EditingControlShowing
        dgtxt_Details = CType(dgv_Details.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub
    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.Enter
        dgv_Details.EditingControl.BackColor = Color.Lime
        dgv_Details.EditingControl.ForeColor = Color.Blue
        dgtxt_Details.SelectAll()
    End Sub
    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Details.KeyPress
        On Error Resume Next
        If Val(dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells(DgvCol_Details.Bal_Mtrs).Value) = 1 Then
            e.Handled = True
        End If
        If dgv_Details.CurrentCell.ColumnIndex = DgvCol_Details.Set_No_2 Or dgv_Details.CurrentCell.ColumnIndex = DgvCol_Details.Beam_No_2 Then
            If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                e.Handled = True
            End If
        End If
    End Sub

    Private Sub dgv_PavuDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyUp
        Dim n As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_Details

                n = .CurrentRow.Index

                If Val(.Rows(n).Cells(DgvCol_Details.BEAM_KNOTTING_CODE).Value) > 0 And Val(.Rows(n).Cells(DgvCol_Details.BEAM_KNOTTING_CODE).Value) <> Val(.Rows(n).Cells(DgvCol_Details.SET_CODE_2).Value) Then
                    MessageBox.Show("Already this beam delivered to others", "DOES NOT DE-SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Exit Sub
                End If

                If n = .Rows.Count - 1 Then

                    For i = 0 To .Columns.Count - 1
                        .Rows(n).Cells(i).Value = ""
                    Next

                Else

                    .Rows.RemoveAt(n)

                End If

                For i = 0 To .Rows.Count - 1
                    .Rows(i).Cells(DgvCol_Details.SNO).Value = i + 1
                Next

            End With

            TotalPavu_Calculation()

        End If

    End Sub



    Private Sub TotalPavu_Calculation()
        Dim Sno As Integer
        Dim TotBms As Single, TotMtrs As Single, TotPcs As Single, TotRctMtrs As Single, TotRctPcs As Single

        Sno = 0
        TotBms = 0
        TotPcs = 0
        TotMtrs = 0
        TotRctPcs = 0
        TotRctMtrs = 0

        With dgv_Details
            For i = 0 To .RowCount - 1
                Sno = Sno + 1
                .Rows(i).Cells(DgvCol_Details.SNO).Value = Sno
                If Val(.Rows(i).Cells(DgvCol_Details.Beam_No_1).Value) <> 0 Then
                    TotBms = TotBms + 1

                    TotPcs = TotPcs + Val(.Rows(i).Cells(DgvCol_Details.ENDS_COUNT).Value)
                    TotMtrs = TotMtrs + Val(.Rows(i).Cells(DgvCol_Details.Beam_No_1).Value)

                    TotRctPcs = TotRctPcs + Val(.Rows(i).Cells(DgvCol_Details.Set_No_2).Value)
                    TotRctMtrs = TotRctMtrs + Val(.Rows(i).Cells(DgvCol_Details.Beam_No_2).Value)
                End If
            Next
        End With


    End Sub

    Private Sub btn_close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub btn_save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_print_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        print_record()
    End Sub
    Private Sub set_PaperSize_For_PrintDocument1()
        Dim I As Integer = 0
        Dim PpSzSTS As Boolean = False
        Dim ps As Printing.PaperSize

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1242" Then
            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                    PrintDocument1.DefaultPageSettings.PaperSize = ps
                    Exit For
                End If
            Next


        ElseIf Val(Common_Procedures.settings.Printing_For_HalfSheet_Set_Custom8X6_As_Default_PaperSize) = 1 Or Prnt_HalfSheet_STS = True Then

            Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8X6", 800, 600)
            PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
            PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1
            PrintDocument1.DefaultPageSettings.Landscape = False

        ElseIf Val(Common_Procedures.settings.Printing_For_HalfSheet_Set_A4_As_Default_PaperSize) = 1 Or Val(Common_Procedures.settings.Printing_For_FullSheet_Set_A4_As_Default_PaperSize) = 1 Or Val(vPrnt_2Copy_In_SinglePage) = 1 Then

            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                    PrintDocument1.DefaultPageSettings.PaperSize = ps
                    Exit For
                End If
            Next

        Else

            PpSzSTS = False

            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                If ps.Width = 800 And ps.Height = 600 Then
                    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                    PrintDocument1.DefaultPageSettings.PaperSize = ps
                    PpSzSTS = True
                    Exit For
                End If
            Next

            If PpSzSTS = False Then

                For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
                        ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                        PrintDocument1.DefaultPageSettings.PaperSize = ps
                        PpSzSTS = True
                        Exit For
                    End If
                Next

                If PpSzSTS = False Then
                    For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                        If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                            PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                            PrintDocument1.DefaultPageSettings.PaperSize = ps
                            Exit For
                        End If
                    Next
                End If

            End If

        End If

    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String
        Dim PpSzSTS As Boolean = False
        Dim I As Integer = 0
        Dim ps As Printing.PaperSize

        NewCode = Trim(Pk_Condition) & Val(lbl_Company.Tag) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.PrintEntry, Common_Procedures.UR.Weaver_Pavu_Rceipt_Entry, New_Entry) = False Then Exit Sub

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.* , d.EndsCount_Name , e.Ledger_Name as Trasport_Name from Weaver_Pavu_Receipt_Head a LEFT OUTER JOIN Ledger_Head b ON a.ReceivedFrom_IdNo = b.Ledger_IdNo LEFT OUTER JOIN EndsCount_Head d ON a.EndsCount_IdNo = d.EndsCount_IdNo LEFT OUTER JOIN Ledger_Head e ON a.Transport_IdNo = e.Ledger_IdNo  where a.Weaver_Pavu_Receipt_Code = '" & Trim(NewCode) & "'", con)
            da1.Fill(dt1)

            If dt1.Rows.Count <= 0 Then

                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub

            End If

            dt1.Dispose()
            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try





        prn_TotCopies = 1
        Prnt_HalfSheet_STS = False

        vPrnt_2Copy_In_SinglePage = Common_Procedures.settings.WeaverWagesPavuReceipt_print_2Copy_In_SinglePage

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1037" Then '---- Prakash Sizing (Somanur)

            Dim mymsgbox As New Tsoft_MessageBox("Select Paper Size to Print", "A4,HALF-SHEET,CANCEL", "FOR PRINTING...", "IF A4 is selected, 2 copies of dc will be printed in single A4 sheet," & Chr(13) & "If HALF-SHEET is selected 1 copy of dc will be printed in 8x6 paper size", MesssageBoxIcons.Questions, 2)
            mymsgbox.ShowDialog()

            If mymsgbox.MessageBoxResult = 1 Then
                vPrnt_2Copy_In_SinglePage = 1

            ElseIf mymsgbox.MessageBoxResult = 2 Then
                Prnt_HalfSheet_STS = True

                vPrnt_2Copy_In_SinglePage = 0

            Else

                Exit Sub

            End If

            prn_TotCopies = Val(InputBox("Enter No.of Copies", "FOR DELIVERY PRINTING...", "1"))
            If Val(prn_TotCopies) <= 0 Then
                Exit Sub
            End If

        End If


        set_PaperSize_For_PrintDocument1()
        'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '    'Debug.Print(ps.PaperName)
        '    If ps.Width = 800 And ps.Height = 600 Then
        '        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
        '        PrintDocument1.DefaultPageSettings.PaperSize = ps
        '        PpSzSTS = True
        '        Exit For
        '    End If
        'Next

        'If PpSzSTS = False Then
        '    For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '        If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
        '            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '            PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
        '            PrintDocument1.DefaultPageSettings.PaperSize = ps
        '            PpSzSTS = True
        '            Exit For
        '        End If
        '    Next

        '    If PpSzSTS = False Then
        '        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
        '                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
        '                PrintDocument1.DefaultPageSettings.PaperSize = ps
        '                Exit For
        '            End If
        '        Next
        '    End If

        'End If

        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then
            Try
                If Val(Common_Procedures.settings.Printing_Show_PrintDialogue) = 1 Then
                    PrintDialog1.PrinterSettings = PrintDocument1.PrinterSettings
                    If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                        PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings

                        'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                        '    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                        '    'Debug.Print(ps.PaperName)
                        '    If ps.Width = 800 And ps.Height = 600 Then
                        '        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                        '        PrintDocument1.DefaultPageSettings.PaperSize = ps
                        '        PpSzSTS = True
                        '        Exit For
                        '    End If
                        'Next

                        'If PpSzSTS = False Then
                        '    For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                        '        If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
                        '            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                        '            PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                        '            PrintDocument1.DefaultPageSettings.PaperSize = ps
                        '            PpSzSTS = True
                        '            Exit For
                        '        End If
                        '    Next

                        '    If PpSzSTS = False Then
                        '        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                        '            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                        '                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                        '                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                        '                PrintDocument1.DefaultPageSettings.PaperSize = ps
                        '                Exit For
                        '            End If
                        '        Next
                        '    End If

                        'End If
                        set_PaperSize_For_PrintDocument1()

                        PrintDocument1.Print()
                    End If

                Else
                    PrintDocument1.Print()

                End If
            Catch ex As Exception
                MessageBox.Show("The printing operation failed" & vbCrLf & ex.Message, "DOES NOT SHOW PRINT PREVIEW...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End Try


        Else
            Try

                Dim ppd As New PrintPreviewDialog

                ppd.Document = PrintDocument1

                ppd.WindowState = FormWindowState.Normal
                ppd.StartPosition = FormStartPosition.CenterScreen
                ppd.ClientSize = New Size(600, 600)

                ppd.ShowDialog()
                'If PageSetupDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                '    PrintDocument1.DefaultPageSettings = PageSetupDialog1.PageSettings
                '    ppd.ShowDialog()
                'End If

            Catch ex As Exception
                MsgBox("The printing operation failed" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "DOES NOT SHOW PRINT PREVIEW...")

            End Try

        End If

    End Sub

    Private Sub PrintDocument1_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument1.BeginPrint
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim NewCode As String


        NewCode = Trim(Pk_Condition) & Val(lbl_Company.Tag) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_PageNo = 0
        prn_NoofBmDets = 0
        prn_DetMxIndx = 0
        prn_HeadIndx = 0
        prn_Count = 0
        prn_Prev_HeadIndx = -100

        Erase prn_DetAr

        prn_DetAr = New String(50, 10) {}

        Try
            da1 = New SqlClient.SqlDataAdapter("select a.*, b.* , d.EndsCount_Name , e.Ledger_Name as Trasport_Name ,f.* from Weaver_Pavu_Receipt_Head a LEFT OUTER JOIN Ledger_Head b ON a.ReceivedFrom_IdNo = b.Ledger_IdNo LEFT OUTER JOIN EndsCount_Head d ON a.EndsCount_IdNo = d.EndsCount_IdNo LEFT OUTER JOIN Ledger_Head e ON a.Transport_IdNo = e.Ledger_IdNo  INNER JOIN Company_Head f ON a.Company_IdNo = f.Company_IdNo where a.Weaver_Pavu_Receipt_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("select a.* , d.EndsCount_Name from Weaver_Pavu_Receipt_Details a LEFT OUTER JOIN EndsCount_Head d ON a.EndsCount_idno = d.EndsCount_idno where Weaver_Pavu_Receipt_Code = '" & Trim(NewCode) & "' Order by a.Sl_No", con)
                prn_DetDt = New DataTable
                da2.Fill(prn_DetDt)

                If prn_DetDt.Rows.Count > 0 Then
                    For i = 0 To prn_DetDt.Rows.Count - 1
                        If Val(prn_DetDt.Rows(i).Item("Meters").ToString) <> 0 Then
                            prn_DetMxIndx = prn_DetMxIndx + 1
                            prn_DetAr(prn_DetMxIndx, 1) = Trim(prn_DetDt.Rows(i).Item("Set_No").ToString)
                            prn_DetAr(prn_DetMxIndx, 2) = Trim(prn_DetDt.Rows(i).Item("Beam_No").ToString)
                            prn_DetAr(prn_DetMxIndx, 3) = Val(prn_DetDt.Rows(i).Item("Pcs").ToString)
                            prn_DetAr(prn_DetMxIndx, 4) = Format(Val(prn_DetDt.Rows(i).Item("Meters").ToString), "#########0.00")
                            prn_DetAr(prn_DetMxIndx, 5) = Trim(Microsoft.VisualBasic.Left(prn_DetDt.Rows(i).Item("EndsCount_Name").ToString, 15))
                        End If
                    Next i
                End If

            Else
                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub PrintDocument1_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        If prn_HdDt.Rows.Count <= 0 Then Exit Sub
        'If Trim(UCase(Common_Procedures.settings.CompanyName)) = "" Then

        Printing_Format1(e)
        'End If
    End Sub

    Private Sub Printing_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim EntryCode As String
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim PCnt As Integer = 0, PrntCnt As Integer = 0
        Dim TpMargin As Single = 0
        Dim cnt As Integer = 0


        'PrintDocument pd = new PrintDocument();
        'pd.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("PaperA4", 840, 1180);
        'pd.Print();

        PrntCnt = 1


        If vPrnt_2Copy_In_SinglePage = 1 Then


            'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            '    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
            '    Debug.Print(ps.PaperName)
            '    If ps.Width = 800 And ps.Height = 600 Then
            '        PrintDocument1.DefaultPageSettings.PaperSize = ps
            '        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
            '        e.PageSettings.PaperSize = ps
            '        PpSzSTS = True
            '        Exit For
            '    End If
            'Next

            If PrntCnt2ndPageSTS = False Then
                PrntCnt = 2
            End If

        Else

            'If PpSzSTS = False Then
            '    For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            '        If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
            '            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
            '            PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
            '            PrintDocument1.DefaultPageSettings.PaperSize = ps
            '            e.PageSettings.PaperSize = ps
            '            PpSzSTS = True
            '            Exit For
            '        End If
            '    Next

            '    If PpSzSTS = False Then
            '        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            '            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
            '                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
            '                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
            '                PrintDocument1.DefaultPageSettings.PaperSize = ps
            '                e.PageSettings.PaperSize = ps
            '                Exit For
            '            End If
            '        Next
            '    End If

            'End If
            set_PaperSize_For_PrintDocument1()

        End If





        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 50
            .Right = 50
            .Top = 30
            .Bottom = 30
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

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

        e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        pFont = New Font("Calibri", 11, FontStyle.Regular)

        NoofItems_PerPage = 5

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = 65 : ClArr(2) = 55 : ClArr(3) = 50 : ClArr(4) = 75 : ClArr(5) = 120
        ClArr(6) = 65 : ClArr(7) = 55 : ClArr(8) = 50 : ClArr(9) = 75
        ClArr(10) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9))

        TxtHgt = 18.5 ' e.Graphics.MeasureString("A", pFont).Height  ' 20



        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_Prev_HeadIndx = prn_HeadIndx

        PrntCnt2ndPageSTS = False
        TpMargin = TMargin
        If Trim(Common_Procedures.settings.CustomerCode) = "1037" And vPrnt_2Copy_In_SinglePage = 1 Then
            PCnt = PCnt + 1
            PrntCnt = PrntCnt + 1
        End If
        For PCnt = 1 To PrntCnt

            If Val(vPrnt_2Copy_In_SinglePage) = 1 Then

                If PCnt = 1 Then
                    prn_PageNo1 = prn_PageNo

                    prn_DetIndx1 = prn_DetIndx
                    prn_DetSNo1 = prn_DetSNo
                    prn_NoofBmDets1 = prn_NoofBmDets
                    TpMargin = TMargin


                Else

                    prn_PageNo = prn_PageNo1
                    prn_NoofBmDets = prn_NoofBmDets1
                    prn_DetIndx = prn_DetIndx1
                    prn_DetSNo = prn_DetSNo1

                    TpMargin = 560 + TMargin  ' 600 + TMargin

                End If

            End If

            Try

                If prn_HdDt.Rows.Count > 0 Then

                    Printing_Format1_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TpMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                    NoofDets = 0

                    NoofItems_PerPage = 5
                    If Val(vPrnt_2Copy_In_SinglePage) = 1 Then
                        If prn_DetDt.Rows.Count > NoofItems_PerPage Then
                            NoofItems_PerPage = 30
                        End If
                    End If

                    CurY = CurY - 10

                    If prn_DetDt.Rows.Count > 0 Then

                        Do While prn_NoofBmDets < prn_DetMxIndx



                            If NoofDets >= NoofItems_PerPage Then

                                If Val(Common_Procedures.settings.WeaverWagesPavuDelivery_Print_2Copy_In_SinglePage) = 1 Then

                                    If PCnt = 2 And PrntCnt = 2 And PrntCnt2ndPageSTS = False Then

                                        CurY = CurY + TxtHgt

                                        Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                                        NoofDets = NoofDets + 1

                                        Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

                                        prn_DetIndx = prn_DetIndx + NoofItems_PerPage

                                        e.HasMorePages = True

                                        Return

                                    End If

                                Else
                                    CurY = CurY + TxtHgt

                                    Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                                    NoofDets = NoofDets + 1

                                    Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

                                    prn_DetIndx = prn_DetIndx + NoofItems_PerPage

                                    e.HasMorePages = True

                                    Return

                                End If
                            End If

                            prn_DetIndx = prn_DetIndx + 1

                            If PCnt <> 2 Then


                                If Val(prn_DetAr(prn_DetIndx, 4)) <> 0 Then

                                    If PCnt = 1 And NoofDets < NoofItems_PerPage Then

                                        CurY = CurY + TxtHgt
                                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 1)), LMargin + 10, CurY, 0, 0, pFont)
                                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 2)), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx, 3))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) - 10, CurY, 1, 0, pFont)
                                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx, 4)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 10, CurY, 1, 0, pFont)
                                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 5)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 10, CurY, 0, 0, pFont)

                                    End If

                                    prn_NoofBmDets = prn_NoofBmDets + 1

                                End If

                                If Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 4)) <> 0 Then

                                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 1)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + 12, CurY, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 2)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + 10, CurY, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 3))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 4)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) - 10, CurY, 1, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 5)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + 10, CurY, 0, 0, pFont)

                                    prn_NoofBmDets = prn_NoofBmDets + 1

                                End If

                                NoofDets = NoofDets + 1
                            End If

                            If PCnt = 2 Then

                                If Val(prn_DetAr(prn_DetIndx, 4)) <> 0 Then



                                    CurY = CurY + TxtHgt
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 1)), LMargin + 10, CurY, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 2)), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx, 3))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) - 10, CurY, 1, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx, 4)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 10, CurY, 1, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 5)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 10, CurY, 0, 0, pFont)



                                    prn_NoofBmDets = prn_NoofBmDets + 1

                                End If

                                If Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 4)) <> 0 Then

                                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 1)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + 12, CurY, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 2)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + 10, CurY, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 3))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 4)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) - 10, CurY, 1, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 5)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + 10, CurY, 0, 0, pFont)

                                    prn_NoofBmDets = prn_NoofBmDets + 1

                                End If

                                NoofDets = NoofDets + 1
                            End If
                        Loop

                    End If

                    Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)

                End If

            Catch ex As Exception

                MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End Try

            If Val(vPrnt_2Copy_In_SinglePage) = 1 Then
                If PCnt = 1 And PrntCnt = 2 And PrntCnt2ndPageSTS = False Then
                    If prn_DetDt.Rows.Count = cnt + 6 Then
                        PrntCnt2ndPageSTS = True
                        e.HasMorePages = True
                        cnt = 6
                        Return
                    End If
                End If
            End If

            PrntCnt2ndPageSTS = False

        Next PCnt

LOOP2:
        prn_Count = prn_Count + 1


        e.HasMorePages = False

        If Val(prn_TotCopies) > 1 Then
            If prn_Count < Val(prn_TotCopies) Then

                prn_DetIndx = 0
                'prn_DetSNo = 0
                prn_PageNo = 0
                prn_DetIndx = 0
                prn_NoofBmDets = 0


                e.HasMorePages = True
                Return

            End If

        End If


    End Sub

    Private Sub Printing_Format1_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim p1Font As Font
        Dim strHeight As Single
        Dim W1 As Single, N1 As Single, M1 As Single
        Dim Arr(300, 5) As String
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String

        PageNo = PageNo + 1

        CurY = TMargin

        If prn_DetMxIndx > (2 * NoofItems_PerPage) Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If

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

        CurY = CurY + TxtHgt - 12
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "PAVU RECEIPT", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight + 3
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        ' Try

        N1 = e.Graphics.MeasureString("TO   : ", pFont).Width
        W1 = e.Graphics.MeasureString("SET NO  :  ", pFont).Width

        M1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5)

        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 11, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, "TO :  M/s. " & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "REC.NO", LMargin + M1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
        p1Font = New Font("Calibri", 14, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Weaver_Pavu_Receipt_No").ToString, LMargin + M1 + W1 + 25, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)


        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + M1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Weaver_Pavu_Receipt_Date").ToString), "dd-MM-yyyy"), LMargin + M1 + W1 + 25, CurY, 0, 0, pFont)


        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + M1, LnAr(3), LMargin + M1, LnAr(2))
        e.Graphics.DrawLine(Pens.Black, LMargin + M1 + 4, LnAr(3), LMargin + M1 + 4, LnAr(2))

        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "SET NO", LMargin, CurY, 2, ClAr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "BEAM", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "PCS", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "ENDS COUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "SET NO", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 3, CurY, 2, ClAr(6), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "BEAM", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "PCS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "ENDS COUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, 2, ClAr(10), pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY

        ' Catch ex As Exception

        'MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OK, MessageBoxIcon.Error)

        '  End Try

    End Sub

    Private Sub Printing_Format1_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim Cmp_Name As String
        Dim I As Integer

        Try

            For I = NoofDets + 1 To NoofItems_PerPage
                CurY = CurY + TxtHgt
            Next

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(3))

            CurY = CurY + TxtHgt - 10

            If is_LastPage = True Then

                If (prn_DetMxIndx Mod (NoofItems_PerPage * 2)) <= NoofItems_PerPage Then


                    If Val(prn_HdDt.Rows(0).Item("Total_Beam").ToString) <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Beam").ToString), LMargin + ClAr(1) + ClAr(2) - 10, CurY, 1, 0, pFont)
                    End If

                    If Val(prn_HdDt.Rows(0).Item("Total_Pcs").ToString) <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Pcs").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 10, CurY, 1, 0, pFont)
                    End If
                    If Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString) <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
                    End If

                Else

                    If Val(prn_HdDt.Rows(0).Item("Total_Beam").ToString) <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Beam").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                    End If
                    If Val(prn_HdDt.Rows(0).Item("Total_Pcs").ToString) <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Pcs").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 10, CurY, 1, 0, pFont)
                    End If
                    If Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString) <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
                    End If

                End If

            End If

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 4, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 4, LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(3))

            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + 20, CurY, LMargin + ClAr(1) + ClAr(2) + 20, LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 5, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 5, LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 5, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 5, LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 50, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 50, LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + 50, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + 50, LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + 50, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + 50, LnAr(3))

            CurY = CurY + TxtHgt - 10
            If Trim(prn_HdDt.Rows(0).Item("Vechile_No").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "Vechile No  :  " & Trim(prn_HdDt.Rows(0).Item("Vechile_No").ToString), LMargin + 10, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(7) = CurY
            Common_Procedures.Print_To_PrintDocument(e, "Received The Beams As Per Above Details.", LMargin + 10, CurY + 5, 0, 0, pFont)
            CurY = CurY + TxtHgt
            If Val(Common_Procedures.User.IdNo) <> 1 Then
                Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
            End If
            CurY = CurY + TxtHgt


            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString

            Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 300, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 5, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin, LnAr(1))
            e.Graphics.DrawLine(Pens.Black, PageWidth, CurY, PageWidth, LnAr(1))

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub


    Private Sub txt_Freight_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyValue = 38 Then
            'cbo_Transport.Focus()
        End If
        If e.KeyValue = 40 Then
            If cbo_WidthType.Visible Then
                cbo_WidthType.Focus()
            Else
                If dgv_Details.Rows.Count > 0 Then
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(DgvCol_Details.Set_No_2)
                    dgv_Details.CurrentCell.Selected = True

                Else
                    'txt_Note.Focus()

                End If
            End If

        End If
    End Sub

    Private Sub txt_Freight_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
        If Asc(e.KeyChar) = 13 Then
            If cbo_WidthType.Visible Then
                cbo_WidthType.Focus()
            Else
                If dgv_Details.Rows.Count > 0 Then
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(DgvCol_Details.Set_No_2)
                    dgv_Details.CurrentCell.Selected = True

                Else
                    'txt_Note.Focus()

                End If
            End If

        End If
    End Sub

    Private Sub txt_Note_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyValue = 38 Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(DgvCol_Details.Set_No_2)
                dgv_Details.CurrentCell.Selected = True

            Else
                'txt_Freight.Focus()

            End If

        End If

        If e.KeyValue = 40 Then
            btn_save.Focus()
            'If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
            '    save_record()
            'Else
            '    msk_date.Focus()
            'End If
        End If

    End Sub

    Private Sub txt_Note_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                msk_date.Focus()
            End If
        End If
    End Sub

    Private Sub dgv_Selection_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Selection.CellClick
        Select_Pavu(e.RowIndex)
    End Sub

    Private Sub Select_Pavu(ByVal RwIndx As Integer)
        Dim i As Integer

        With dgv_Selection

            If .RowCount > 0 And RwIndx >= 0 Then

                If Val(.Rows(RwIndx).Cells(11).Value) > 0 Then
                    If Val(.Rows(RwIndx).Cells(10).Value) <> Val(.Rows(RwIndx).Cells(11).Value) Then
                        MessageBox.Show("Already this beam delivered to others", "DOES NOT DESELECT...", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        Exit Sub
                    End If
                End If

                .Rows(RwIndx).Cells(8).Value = (Val(.Rows(RwIndx).Cells(8).Value) + 1) Mod 2

                If Val(.Rows(RwIndx).Cells(8).Value) = 1 Then
                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
                    Next

                Else
                    .Rows(RwIndx).Cells(8).Value = ""
                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Black
                    Next

                End If

            End If

        End With



    End Sub

    Private Sub dgv_Selection_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Selection.KeyDown
        On Error Resume Next

        With dgv_Selection

            If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Space Then
                If .CurrentCell.RowIndex >= 0 Then
                    Select_Pavu(.CurrentCell.RowIndex)
                    e.Handled = True
                End If
            End If

            If e.KeyCode = Keys.Back Or e.KeyCode = Keys.Delete Then
                If .CurrentCell.RowIndex >= 0 Then
                    If Val(.Rows(.CurrentCell.RowIndex).Cells(8).Value) = 1 Then
                        Select_Pavu(.CurrentCell.RowIndex)
                        e.Handled = True
                    End If
                End If
            End If

        End With

    End Sub

    Private Sub btn_Close_Selection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Selection.Click
        Dim n As Integer
        Dim sno As Integer
        Dim EntPcs As Single = 0, EntMtrs As Single = 0
        Dim i As Integer
        Dim j As Integer

        With dgv_Details

            .Rows.Clear()

            For i = 0 To dgv_Selection.RowCount - 1

                If Val(dgv_Selection.Rows(i).Cells(8).Value) = 1 Then

                    EntPcs = 0 : EntMtrs = 0

                    If Val(dgv_Selection.Rows(i).Cells(12).Value) <> 0 Then
                        EntPcs = Val(dgv_Selection.Rows(i).Cells(12).Value)
                    Else
                        EntPcs = Val(dgv_Selection.Rows(i).Cells(3).Value)
                    End If

                    If Val(dgv_Selection.Rows(i).Cells(13).Value) <> 0 Then
                        EntMtrs = Val(dgv_Selection.Rows(i).Cells(13).Value)
                    Else
                        EntMtrs = Val(dgv_Selection.Rows(i).Cells(12).Value)
                    End If


                    n = .Rows.Add()

                    sno = sno + 1
                    .Rows(n).Cells(DgvCol_Details.SNO).Value = Val(sno)
                    .Rows(n).Cells(DgvCol_Details.LOOM_NO).Value = dgv_Selection.Rows(i).Cells(1).Value
                    .Rows(n).Cells(DgvCol_Details.PARTY_NAME).Value = dgv_Selection.Rows(i).Cells(2).Value
                    .Rows(n).Cells(DgvCol_Details.KNOT_NO).Value = dgv_Selection.Rows(i).Cells(6).Value
                    .Rows(n).Cells(DgvCol_Details.ENDS_COUNT).Value = Val(dgv_Selection.Rows(i).Cells(3).Value)
                    .Rows(n).Cells(DgvCol_Details.SET_NO_1).Value = Val(dgv_Selection.Rows(i).Cells(4).Value)
                    .Rows(n).Cells(DgvCol_Details.BEAM_NO_1).Value = Format(Val(dgv_Selection.Rows(i).Cells(5).Value), "#########0.00")

                    .Rows(n).Cells(DgvCol_Details.SET_NO_2).Value = Val(EntPcs)
                    .Rows(n).Cells(DgvCol_Details.BEAM_NO_2).Value = Format(Val(EntMtrs), "#########0.00")

                    .Rows(n).Cells(DgvCol_Details.TOTAL_MTRS).Value = dgv_Selection.Rows(i).Cells(7).Value

                    .Rows(n).Cells(DgvCol_Details.BAL_MTRS).Value = ""
                    .Rows(n).Cells(DgvCol_Details.BEAM_KNOTTING_CODE).Value = ""

                    If Val(dgv_Selection.Rows(i).Cells(11).Value) > 0 Then

                        If Val(dgv_Selection.Rows(i).Cells(10).Value) <> Val(dgv_Selection.Rows(i).Cells(11).Value) Then
                            .Rows(n).Cells(DgvCol_Details.BAL_MTRS).Value = "1"
                            For j = 0 To .ColumnCount - 1
                                .Rows(n).Cells(j).Style.ForeColor = Color.Red
                                .Rows(n).Cells(j).Style.BackColor = Color.LightGray
                            Next
                        Else
                            .Rows(n).Cells(DgvCol_Details.BAL_MTRS).Value = ""
                        End If

                        .Rows(n).Cells(DgvCol_Details.BEAM_KNOTTING_CODE).Value = dgv_Selection.Rows(i).Cells(11).Value

                    End If

                    .Rows(n).Cells(DgvCol_Details.SET_CODE_1).Value = dgv_Selection.Rows(i).Cells(9).Value
                    .Rows(n).Cells(DgvCol_Details.SET_CODE_2).Value = dgv_Selection.Rows(i).Cells(10).Value

                End If

            Next

        End With

        TotalPavu_Calculation()

        Grid_Cell_DeSelect()

        pnl_Back.Enabled = True
        pnl_Selection.Visible = False
        'If cbo_EndsCount.Enabled And cbo_EndsCount.Visible Then cbo_EndsCount.Focus()

    End Sub

    Private Sub dgtxt_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyUp
        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            dgv_PavuDetails_KeyUp(sender, e)
        End If
    End Sub
    Private Sub txt_SetNoSelection_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_SetNoSelection.KeyDown
        If (e.KeyValue = 40) Then
            txt_BeamNoSelection.Focus()
        End If
    End Sub

    Private Sub txt_SetNoSelection_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_SetNoSelection.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_BeamNoSelection.Focus()
        End If
    End Sub

    Private Sub txt_BeamNoSelection_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_BeamNoSelection.KeyDown
        If e.KeyValue = 40 Then
            If dgv_Selection.Rows.Count > 0 Then
                dgv_Selection.Focus()
                dgv_Selection.CurrentCell = dgv_Selection.Rows(0).Cells(0)
                dgv_Selection.CurrentCell.Selected = True
            End If
        End If
        If (e.KeyValue = 38) Then txt_SetNoSelection.Focus()
    End Sub

    Private Sub txt_BeamNoSelection_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_BeamNoSelection.KeyPress
        If Asc(e.KeyChar) = 13 Then

            If Trim(txt_BeamNoSelection.Text) <> "" Or Trim(txt_SetNoSelection.Text) <> "" Then
                btn_Set_Bm_selection_Click(sender, e)

            Else
                If dgv_Selection.Rows.Count > 0 Then
                    dgv_Selection.Focus()
                    dgv_Selection.CurrentCell = dgv_Selection.Rows(0).Cells(0)
                    dgv_Selection.CurrentCell.Selected = True
                End If

            End If

        End If
    End Sub

    Private Sub btn_Set_Bm_selection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Set_Bm_selection.Click
        Dim LtNo As String
        Dim PcsNo As String
        Dim i As Integer

        If Trim(txt_SetNoSelection.Text) <> "" Or Trim(txt_BeamNoSelection.Text) <> "" Then

            LtNo = Trim(txt_SetNoSelection.Text)
            PcsNo = Trim(txt_BeamNoSelection.Text)

            For i = 0 To dgv_Selection.Rows.Count - 1
                If Trim(UCase(LtNo)) = Trim(UCase(dgv_Selection.Rows(i).Cells(1).Value)) And Trim(UCase(PcsNo)) = Trim(UCase(dgv_Selection.Rows(i).Cells(2).Value)) Then
                    Call Select_Pavu(i)

                    dgv_Selection.CurrentCell = dgv_Selection.Rows(i).Cells(0)
                    If i >= 11 Then dgv_Selection.FirstDisplayedScrollingRowIndex = i - 10

                    Exit For

                End If
            Next

            txt_SetNoSelection.Text = ""
            txt_BeamNoSelection.Text = ""
            If txt_SetNoSelection.Enabled = True Then txt_SetNoSelection.Focus()

        End If
    End Sub

    Private Sub chk_SelectAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chk_SelectAll.CheckedChanged
        Dim i As Integer
        Dim J As Integer

        With dgv_Selection

            For i = 0 To .Rows.Count - 1
                .Rows(i).Cells(8).Value = ""
                For J = 0 To .ColumnCount - 1
                    .Rows(i).Cells(J).Style.ForeColor = Color.Black
                Next J
            Next i

            If chk_SelectAll.Checked = True Then
                For i = 0 To .Rows.Count - 1
                    Select_Pavu(i)
                Next i
            End If

            If .Rows.Count > 0 Then
                .Focus()
                .CurrentCell = .Rows(0).Cells(0)
                .CurrentCell.Selected = True
            End If

        End With

    End Sub

    Private Sub msk_date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles msk_date.KeyPress
        If Trim(UCase(e.KeyChar)) = "D" Then
            msk_date.Text = Date.Today
            msk_date.SelectionStart = 0
        End If

    End Sub



    Private Sub msk_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_date.KeyUp
        Dim vmRetTxt As String = ""
        Dim vmRetSelStrt As Integer = -1

        If e.KeyCode = 107 Then
            msk_date.Text = DateAdd("D", 1, Convert.ToDateTime(msk_date.Text))
            msk_date.SelectionStart = 0
        ElseIf e.KeyCode = 109 Then
            msk_date.Text = DateAdd("D", -1, Convert.ToDateTime(msk_date.Text))
            msk_date.SelectionStart = 0
        End If

        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            Common_Procedures.maskEdit_Date_ON_DelBackSpace(sender, e, vmskOldText, vmskSelStrt)
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
    End Sub

    Private Sub dtp_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Date.KeyDown
        If e.KeyCode = 40 Then
            e.Handled = True : e.SuppressKeyPress = True
            msk_date.Focus()
        End If

        If e.KeyCode = 38 Then
            e.Handled = True : e.SuppressKeyPress = True
            msk_date.Focus()
        End If
    End Sub

    Private Sub dtp_Date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_Date.KeyPress
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            msk_date.Focus()
        End If
    End Sub
    Private Sub dtp_Date_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_Date.TextChanged
        If IsDate(dtp_Date.Text) = True Then
            msk_date.Text = dtp_Date.Text
            msk_date.SelectionStart = 0
        End If
    End Sub
    Private Sub msk_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_date.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        If e.KeyCode = 40 Then
            e.Handled = True : e.SuppressKeyPress = True
            'txt_Party_DcNo.Focus()
        End If

        If e.KeyCode = 38 Then
            e.Handled = True : e.SuppressKeyPress = True
        End If

        vmskOldText = ""
        vmskSelStrt = -1
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            vmskOldText = msk_date.Text
            vmskSelStrt = msk_date.SelectionStart
        End If
    End Sub

    Private Sub cbo_WidthType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_WidthType.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")
    End Sub

    Private Sub cbo_WidthType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_WidthType.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_WidthType, msk_date, Nothing, "", "", "", "")

        If (e.KeyValue = 40 And cbo_WidthType.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then


            If cbo_ClothName1.Visible And cbo_ClothName1.Enabled Then
                cbo_ClothName1.Focus()


            End If

        End If


    End Sub

    Private Sub cbo_WidthType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_WidthType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_WidthType, Nothing, "", "", "", "")
        If Asc(e.KeyChar) = 13 Then

            If cbo_ClothName1.Visible And cbo_ClothName1.Enabled Then
                cbo_ClothName1.Focus()



            Else
                If dgv_Details.Rows.Count > 0 Then
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(DgvCol_Details.LOOM_NO)
                    dgv_Details.CurrentCell.Selected = True

                Else
                    msk_date.Focus()

                End If
            End If
        End If
    End Sub
    Private Sub btn_SMS_Click(sender As System.Object, e As System.EventArgs)
        Dim i As Integer = 0
        Dim smstxt As String = ""
        Dim PhNo As String = "", EndsCount As String = ""
        Dim Led_IdNo As Integer = 0, Endscount_IdNo As Integer = 0
        Dim SMS_SenderID As String = ""
        Dim SMS_Key As String = ""
        Dim SMS_RouteID As String = ""
        Dim SMS_Type As String = ""
        Dim BlNos As String = ""

        Try

            'Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_RecForm.Text)

            PhNo = Common_Procedures.get_FieldValue(con, "Ledger_Head", "MobileNo_Frsms", "(Ledger_IdNo = " & Str(Val(Led_IdNo)) & ")")

            'Endscount_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_EndsCount.Text)
            'EndsCount = ""
            'If Val(Endscount_IdNo) <> 0 Then
            '    EndsCount = Common_Procedures.get_FieldValue(con, "EndsCount_Name", "EndsCount_name", "(EndsCount_IdNo = " & Str(Val(Endscount_IdNo)) & ")")
            'End If

            ' If Trim(AgPNo) <> "" Then
            PhNo = Trim(PhNo) & IIf(Trim(PhNo) <> "", "", "")
            ' End If

            ' smstxt = Trim(cbo_.Text) & vbCrLf
            smstxt = smstxt & " Rec No : " & Trim(lbl_RefNo.Text) & vbCrLf
            smstxt = smstxt & " Date : " & Trim(msk_date.Text) & vbCrLf



            If dgv_Details.RowCount > 0 Then
                ' smstxt = smstxt & " Beam No: " & Trim((dgv_PavuDetails.Rows(0).Cells(3).Value())) & vbCrLf
                smstxt = smstxt & "Ends Count : " & Trim((dgv_Details.Rows(0).Cells(DgvCol_Details.KNOT_NO).Value())) & vbCrLf

            End If


            smstxt = smstxt & " " & vbCrLf
            smstxt = smstxt & " Thanks! " & vbCrLf
            smstxt = smstxt & Common_Procedures.Company_IdNoToName(con, Val(lbl_Company.Tag))

            SMS_SenderID = ""
            SMS_Key = ""
            SMS_RouteID = ""
            SMS_Type = ""

            Common_Procedures.get_SMS_Provider_Details(con, Val(lbl_Company.Tag), SMS_SenderID, SMS_Key, SMS_RouteID, SMS_Type)

            Sms_Entry.vSmsPhoneNo = Trim(PhNo)
            Sms_Entry.vSmsMessage = Trim(smstxt)

            Sms_Entry.SMSProvider_SenderID = SMS_SenderID
            Sms_Entry.SMSProvider_Key = SMS_Key
            Sms_Entry.SMSProvider_RouteID = SMS_RouteID
            Sms_Entry.SMSProvider_Type = SMS_Type

            Dim f1 As New Sms_Entry
            f1.MdiParent = MDIParent1
            f1.Show()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SEND SMS...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub btn_UserModification_Click(sender As Object, e As System.EventArgs)
        If Val(Common_Procedures.User.IdNo) = 1 Then
            Dim f1 As New User_Modifications
            f1.Entry_Name = Me.Name
            f1.Entry_PkValue = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            f1.ShowDialog()
        End If
    End Sub

    Private Sub cbo_Type_Enter(ByVal sender As Object, ByVal e As System.EventArgs)
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")
    End Sub

    Private Sub cbo_WidthType_TextChanged(sender As Object, e As EventArgs) Handles cbo_WidthType.TextChanged


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
            cbo_ClothName1.Enabled = True
            cbo_ClothName2.Enabled = True
            cbo_ClothName3.Enabled = False
            cbo_ClothName4.Enabled = False

        ElseIf Trim(UCase(cbo_WidthType.Text)) = "SINGLE" Then

            cbo_ClothName2.Text = ""
            cbo_ClothName3.Text = ""
            cbo_ClothName4.Text = ""
            cbo_ClothName1.Enabled = True
            cbo_ClothName2.Enabled = False
            cbo_ClothName3.Enabled = False
            cbo_ClothName4.Enabled = False


        Else
            cbo_ClothName1.Text = ""
            cbo_ClothName2.Text = ""
            cbo_ClothName3.Text = ""
            cbo_ClothName4.Text = ""
            cbo_ClothName1.Enabled = False
            cbo_ClothName2.Enabled = False
            cbo_ClothName3.Enabled = False
            cbo_ClothName4.Enabled = False

        End If
    End Sub

    Private Sub Cbo_Grid_Loom_No_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Cbo_Grid_Loom_No.KeyPress
        Dim vCOMPCONDT As String
        Dim vLOOMNOS_ENTERED As String

        vCOMPCONDT = ""
        vLOOMNOS_ENTERED = ""
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1155" Then
            vCOMPCONDT = "(Loom_CompanyIdno = " & Val(lbl_Company.Tag) & ")"
        End If

        vLOOMNOS_ENTERED = "'~AA~BB~CC~'"
        For I = 0 To dgv_Details.Rows.Count - 1
            If I <> dgv_Details.CurrentCell.RowIndex Then
                If Trim(dgv_Details.Rows(I).Cells(DgvCol_Details.LOOM_NO).Value) <> "" Then
                    vLOOMNOS_ENTERED = Trim(vLOOMNOS_ENTERED) & ", '" & Trim(dgv_Details.Rows(I).Cells(DgvCol_Details.LOOM_NO).Value) & "'"
                End If
            End If
        Next I
        vLOOMNOS_ENTERED = "Loom_Name NOT IN (" & Trim(vLOOMNOS_ENTERED) & ")"

        vCOMPCONDT = vCOMPCONDT & IIf(Trim(vCOMPCONDT) <> "", " AND ", "") & vLOOMNOS_ENTERED

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, sender, Nothing, "Loom_Head", "Loom_Name", vCOMPCONDT, "(Loom_IdNo = 0 )")
        If Asc(e.KeyChar) = 13 Then
            If Trim(Cbo_Grid_Loom_No.Text) <> "" And Trim(UCase(Cbo_Grid_Loom_No.Text)) <> Trim(UCase(Cbo_Grid_Loom_No.Tag)) Then
                btn_Selection_Click(sender, e)

            Else

                If dgv_Details.CurrentCell.RowIndex >= dgv_Details.Rows.Count - 1 Then
                    If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                        save_record()
                    Else
                        msk_date.Focus()
                    End If

                Else

                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex + 1).Cells(DgvCol_Details.LOOM_NO)
                    If Cbo_Grid_Loom_No.Visible = True Then
                        Cbo_Grid_Loom_No.BringToFront()
                        Cbo_Grid_Loom_No.Focus()
                    End If

                End If

            End If
        End If
    End Sub

    Private Sub Cbo_Grid_Loom_No_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_Grid_Loom_No.GotFocus
        Dim vCOMPCONDT As String
        Dim vLOOMNOS_ENTERED As String

        vCOMPCONDT = ""
        vLOOMNOS_ENTERED = ""
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1155" Then
            vCOMPCONDT = "(Loom_CompanyIdno = " & Val(lbl_Company.Tag) & ")"
        End If

        vLOOMNOS_ENTERED = "'~AA~BB~CC~'"
        For I = 0 To dgv_Details.Rows.Count - 1
            If I <> dgv_Details.CurrentCell.RowIndex Then
                If Trim(dgv_Details.Rows(I).Cells(DgvCol_Details.LOOM_NO).Value) <> "" Then
                    vLOOMNOS_ENTERED = Trim(vLOOMNOS_ENTERED) & ", '" & Trim(dgv_Details.Rows(I).Cells(DgvCol_Details.LOOM_NO).Value) & "'"
                End If
            End If
        Next I
        vLOOMNOS_ENTERED = "Loom_Name NOT IN (" & Trim(vLOOMNOS_ENTERED) & ")"

        vCOMPCONDT = vCOMPCONDT & IIf(Trim(vCOMPCONDT) <> "", " AND ", "") & vLOOMNOS_ENTERED


        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Loom_Head", "Loom_Name", vCOMPCONDT, "(Loom_IdNo = 0 )")
    End Sub

    Private Sub Cbo_Grid_Loom_No_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Cbo_Grid_Loom_No.KeyDown

        vcbo_KeyDwnVal = e.KeyValue

        Dim vCOMPCONDT As String
        Dim vLOOMNOS_ENTERED As String

        vCOMPCONDT = ""
        vLOOMNOS_ENTERED = ""
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1155" Then
            vCOMPCONDT = "(Loom_CompanyIdno = " & Val(lbl_Company.Tag) & ")"
        End If

        vLOOMNOS_ENTERED = "'~AA~BB~CC~'"
        For I = 0 To dgv_Details.Rows.Count - 1
            If I <> dgv_Details.CurrentCell.RowIndex Then
                If Trim(dgv_Details.Rows(I).Cells(DgvCol_Details.LOOM_NO).Value) <> "" Then
                    vLOOMNOS_ENTERED = Trim(vLOOMNOS_ENTERED) & ", '" & Trim(dgv_Details.Rows(I).Cells(DgvCol_Details.LOOM_NO).Value) & "'"
                End If
            End If
        Next I
        vLOOMNOS_ENTERED = "Loom_Name NOT IN (" & Trim(vLOOMNOS_ENTERED) & ")"

        vCOMPCONDT = vCOMPCONDT & IIf(Trim(vCOMPCONDT) <> "", " AND ", "") & vLOOMNOS_ENTERED

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, sender, Nothing, Nothing, "Loom_Head", "Loom_Name", vCOMPCONDT, "(Loom_IdNo = 0)")
        If (e.KeyValue = 38 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
            If dgv_Details.CurrentCell.RowIndex <= 0 Then
                If cbo_ClothName4.Visible And cbo_ClothName4.Enabled Then
                    cbo_ClothName4.Focus()
                ElseIf cbo_ClothName3.Visible And cbo_ClothName3.Enabled Then
                    cbo_ClothName3.Focus()
                ElseIf cbo_ClothName2.Visible And cbo_ClothName2.Enabled Then
                    cbo_ClothName2.Focus()
                Else
                    cbo_ClothName1.Focus()
                End If

            Else

                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex - 1).Cells(DgvCol_Details.LOOM_NO)
                If Cbo_Grid_Loom_No.Visible = True Then
                    Cbo_Grid_Loom_No.BringToFront()
                    Cbo_Grid_Loom_No.Focus()
                End If

            End If

        ElseIf (e.KeyValue = 40 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

            If dgv_Details.CurrentCell.RowIndex >= dgv_Details.Rows.Count - 1 Then
                btn_save.Focus()

            Else

                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex + 1).Cells(DgvCol_Details.LOOM_NO)
                If Cbo_Grid_Loom_No.Visible = True Then
                    Cbo_Grid_Loom_No.BringToFront()
                    Cbo_Grid_Loom_No.Focus()
                End If

            End If

        End If
    End Sub

    Private Sub cbo_ClothName1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_ClothName1.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_ClothName1, Nothing, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")

        If Asc(e.KeyChar) = 13 Then
            If cbo_ClothName2.Visible And cbo_ClothName2.Enabled = True Then
                cbo_ClothName2.Focus()




            Else
                If dgv_Details.Rows.Count > 0 Then
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(DgvCol_Details.LOOM_NO)
                    dgv_Details.CurrentCell.Selected = True

                Else
                    msk_date.Focus()

                End If
            End If
        End If
    End Sub

    Private Sub cbo_ClothName2_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_ClothName2.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_ClothName2, Nothing, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")

        If Asc(e.KeyChar) = 13 Then
            If cbo_ClothName3.Visible And cbo_ClothName3.Enabled = True Then

                cbo_ClothName3.Focus()
            Else
                If dgv_Details.Rows.Count > 0 Then
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(DgvCol_Details.LOOM_NO)
                    dgv_Details.CurrentCell.Selected = True

                Else
                    msk_date.Focus()

                End If
            End If
        End If
    End Sub

    Private Sub cbo_ClothName3_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_ClothName3.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_ClothName3, Nothing, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")

        If Asc(e.KeyChar) = 13 Then
            If cbo_ClothName4.Visible And cbo_ClothName4.Enabled = True Then
                cbo_ClothName4.Focus()
            Else
                If dgv_Details.Rows.Count > 0 Then
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(DgvCol_Details.LOOM_NO)
                    dgv_Details.CurrentCell.Selected = True

                Else
                    msk_date.Focus()

                End If
            End If
        End If
    End Sub

    Private Sub cbo_ClothName4_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_ClothName4.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_ClothName4, Nothing, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")

        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then

            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(DgvCol_Details.LOOM_NO)
            dgv_Details.CurrentCell.Selected = True

        End If
    End Sub

    Private Sub cbo_ClothName1_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_ClothName1.KeyDown

        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_ClothName1, cbo_WidthType, Nothing, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")


        If (e.KeyValue = 40 And cbo_ClothName1.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If cbo_ClothName2.Visible And cbo_ClothName2.Enabled = True Then
                cbo_ClothName2.Focus()

            Else
                If dgv_Details.Rows.Count > 0 Then
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(DgvCol_Details.LOOM_NO)
                    dgv_Details.CurrentCell.Selected = True
                End If
            End If
        End If

    End Sub

    Private Sub cbo_ClothName2_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_ClothName2.KeyDown

        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_ClothName2, cbo_ClothName1, Nothing, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")



        If (e.KeyValue = 40 And cbo_ClothName2.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

            If cbo_ClothName3.Visible And cbo_ClothName3.Enabled = True Then
                cbo_ClothName3.Focus()

            Else
                If dgv_Details.Rows.Count > 0 Then
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(DgvCol_Details.LOOM_NO)
                    dgv_Details.CurrentCell.Selected = True
                End If
            End If
        End If

    End Sub

    Private Sub cbo_ClothName3_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_ClothName3.KeyDown

        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_ClothName3, cbo_ClothName2, Nothing, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")



        If (e.KeyValue = 40 And cbo_ClothName3.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

            If cbo_ClothName4.Visible And cbo_ClothName4.Enabled = True Then
                cbo_ClothName4.Focus()

            Else
                If dgv_Details.Rows.Count > 0 Then
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(DgvCol_Details.LOOM_NO)
                    dgv_Details.CurrentCell.Selected = True
                End If
            End If


        End If
    End Sub

    Private Sub cbo_ClothName4_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_ClothName4.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_ClothName4, cbo_ClothName3, Nothing, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")

        If e.KeyCode = 38 Then
            cbo_ClothName3.Focus()

        ElseIf e.KeyCode = 40 Then

            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(DgvCol_Details.LOOM_NO)
            dgv_Details.CurrentCell.Selected = True

        End If
    End Sub


    Private Sub Cbo_Grid_Loom_No_TextChanged(sender As Object, e As EventArgs) Handles Cbo_Grid_Loom_No.TextChanged
        If IsNothing(dgv_Details.CurrentCell) Then Exit Sub

        If Cbo_Grid_Loom_No.Visible = True Then
            With dgv_Details

                If Val(Cbo_Grid_Loom_No.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = DgvCol_Details.LOOM_NO Then
                    .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(Cbo_Grid_Loom_No.Text)
                End If

            End With
        End If


    End Sub


    Private Sub btn_Selection_Click(sender As Object, e As EventArgs) Handles btn_Selection.Click
        Dim cmd As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Da2 As New SqlClient.SqlDataAdapter
        Dim Da3 As New SqlClient.SqlDataAdapter
        Dim Da4 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim Dt4 As New DataTable
        Dim Lm_ID As Integer, LMNO As String
        Dim NewCode As String = ""
        Dim Sno As Integer
        Dim m As Integer = 0, n As Integer = 0
        Dim vCRIMPPERC As String = 0
        Dim vPRODMTRS As String = 0

        LMNO = Cbo_Grid_Loom_No.Text
        Lm_ID = Common_Procedures.Loom_NameToIdNo(con, Cbo_Grid_Loom_No.Text)

        Da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name, c.Cloth_Name, c.Crimp_Percentage, d.EndsCount_Name, e.Count_Name ,j.Po_No from Beam_Knotting_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo INNER JOIN Cloth_Head c ON a.Cloth_IdNo1 = c.Cloth_IdNo INNER JOIN EndsCount_Head d ON a.EndsCount_IdNo = d.EndsCount_IdNo INNER JOIN Count_Head e ON c.Cloth_WeftCount_IdNo = e.Count_IdNo LEFT OUTER JOIN JobWork_Pavu_Receipt_Details j ON a.Set_Code1  = j.Set_Code and  a.Beam_no1  = j.Beam_No Where a.Loom_IdNo = " & Str(Val(Lm_ID)) & " and a.Beam_RunOut_Code = '' Order by a.Beam_Knotting_Date, a.for_OrderBy, a.Beam_Knotting_Code", con)
        Dt1 = New DataTable
        Da1.Fill(Dt1)

        With dgv_Details

            If Dt1.Rows.Count > 0 Then

                Sno = 0
                Sno = Sno + 1

                n = .CurrentCell.RowIndex

                .Rows(n).Cells(DgvCol_Details.SNO).Value = .Rows.Count - 1
                .Rows(n).Cells(DgvCol_Details.LOOM_NO).Value = Trim(LMNO)
                .Rows(n).Cells(DgvCol_Details.PARTY_NAME).Value = Dt1.Rows(0).Item("Ledger_Name").ToString
                .Rows(n).Cells(DgvCol_Details.KNOT_NO).Value = Dt1.Rows(0).Item("Beam_Knotting_No").ToString
                .Rows(n).Cells(DgvCol_Details.ENDS_COUNT).Value = Dt1.Rows(0).Item("EndsCount_Name").ToString
                .Rows(n).Cells(DgvCol_Details.BEAM_KNOTTING_CODE).Value = Dt1.Rows(0).Item("Beam_Knotting_Code").ToString
                .Rows(n).Cells(DgvCol_Details.SET_CODE_1).Value = Dt1.Rows(0).Item("Set_Code1").ToString
                .Rows(n).Cells(DgvCol_Details.SET_NO_1).Value = Dt1.Rows(0).Item("Set_No1").ToString
                .Rows(n).Cells(DgvCol_Details.BEAM_NO_1).Value = Dt1.Rows(0).Item("Beam_No1").ToString
                .Rows(n).Cells(DgvCol_Details.SET_CODE_2).Value = Dt1.Rows(0).Item("Set_Code2").ToString
                .Rows(n).Cells(DgvCol_Details.SET_NO_2).Value = Dt1.Rows(0).Item("Set_No2").ToString
                .Rows(n).Cells(DgvCol_Details.BEAM_NO_2).Value = Dt1.Rows(0).Item("Beam_No2").ToString
                .Rows(n).Cells(DgvCol_Details.TOTAL_MTRS).Value = 0
                .Rows(n).Cells(DgvCol_Details.BAL_MTRS).Value = 0

                'If Trim(Dt1.Rows(0).Item("Set_Code1").ToString) <> "" And Trim(Dt1.Rows(0).Item("Beam_No1").ToString) <> "" Then

                '    cmd.Connection = con
                '    cmd.CommandType = CommandType.StoredProcedure
                '    cmd.CommandText = "SP_get_Beam_Details_from_SizedPavu_Processing_Details"
                '    cmd.Parameters.Add("@setcode", SqlDbType.VarChar)
                '    cmd.Parameters("@setcode").Value = Trim(Dt1.Rows(0).Item("Set_Code1").ToString)
                '    cmd.Parameters.Add("@beamno", SqlDbType.VarChar)
                '    cmd.Parameters("@beamno").Value = Trim(Dt1.Rows(0).Item("Beam_No1").ToString)

                '    Da4 = New SqlClient.SqlDataAdapter(cmd)
                '    'Da4 = New SqlClient.SqlDataAdapter("Select Meters, Production_Meters from Stock_SizedPavu_Processing_Details Where Set_Code = '" & Trim(lbl_Pcs_SetCode1.Text) & "' and Beam_No = '" & Trim(lbl_Pcs_BeamNo1.Text) & "'", con)
                '    Dt4 = New DataTable
                '    Da4.Fill(Dt4)
                '    If Dt4.Rows.Count > 0 Then

                '        .Rows(n).Cells(DgvCol_Details.TOTAL_MTRS).Value = Dt4.Rows(0).Item("Meters").ToString

                '        vCRIMPPERC = Common_Procedures.Calculation_CrimpPercentage_On_BEAMRUNOUT(con, Trim(Dt1.Rows(0).Item("Set_Code1").ToString), Trim(Dt1.Rows(0).Item("Beam_No1").ToString), Val(Dt4.Rows(0).Item("Meters").ToString), vPRODMTRS)
                '        .Rows(n).Cells(DgvCol_Details.BAL_MTRS).Value = Format(Val(Dt4.Rows(0).Item("Meters").ToString) - Val(vPRODMTRS), "#########0.00")

                '    End If
                '    Dt4.Clear()
                'End If

                Da2 = New SqlClient.SqlDataAdapter("Select * from Stock_SizedPavu_Processing_Details Where Set_Code = '" & Trim(Dt1.Rows(0).Item("Set_Code1").ToString) & "' and Beam_No = '" & Trim(Dt1.Rows(0).Item("Beam_No1").ToString) & "'", con)
                Dt2 = New DataTable
                Da2.Fill(Dt2)
                If Dt2.Rows.Count > 0 Then
                    .Rows(n).Cells(DgvCol_Details.TOTAL_MTRS).Value = Dt2.Rows(0).Item("Meters").ToString
                    .Rows(n).Cells(DgvCol_Details.BAL_MTRS).Value = Format(Val(Dt2.Rows(0).Item("Meters").ToString) - Val(Dt2.Rows(0).Item("Production_Meters").ToString), "#########0.00")
                End If
                Dt2.Clear()


                If n = dgv_Details.Rows.Count - 1 Then
                    n = .Rows.Add()
                End If

                For i = 0 To .RowCount - 1
                    .Rows(i).Cells(DgvCol_Details.SNO).Value = i + 1
                Next i

                dgv_Details.Focus()
                If n = dgv_Details.Rows.Count - 1 Then
                    dgv_Details.CurrentCell = dgv_Details.Rows(n).Cells(DgvCol_Details.LOOM_NO)
                Else
                    dgv_Details.CurrentCell = dgv_Details.Rows(n + 1).Cells(DgvCol_Details.LOOM_NO)
                End If
                If Cbo_Grid_Loom_No.Visible = True Then
                    Cbo_Grid_Loom_No.BringToFront()
                    Cbo_Grid_Loom_No.Focus()
                End If

            Else

                Cbo_Grid_Loom_No.Text = ""

                n = .CurrentCell.RowIndex

                .Rows(n).Cells(DgvCol_Details.SNO).Value = .Rows.Count - 1
                .Rows(n).Cells(DgvCol_Details.LOOM_NO).Value = ""
                .Rows(n).Cells(DgvCol_Details.PARTY_NAME).Value = ""
                .Rows(n).Cells(DgvCol_Details.KNOT_NO).Value = ""
                .Rows(n).Cells(DgvCol_Details.ENDS_COUNT).Value = ""
                .Rows(n).Cells(DgvCol_Details.BEAM_KNOTTING_CODE).Value = ""
                .Rows(n).Cells(DgvCol_Details.SET_CODE_1).Value = ""
                .Rows(n).Cells(DgvCol_Details.SET_NO_1).Value = ""
                .Rows(n).Cells(DgvCol_Details.BEAM_NO_1).Value = ""
                .Rows(n).Cells(DgvCol_Details.SET_CODE_2).Value = ""
                .Rows(n).Cells(DgvCol_Details.SET_NO_2).Value = ""
                .Rows(n).Cells(DgvCol_Details.BEAM_NO_2).Value = ""
                .Rows(n).Cells(DgvCol_Details.TOTAL_MTRS).Value = ""
                .Rows(n).Cells(DgvCol_Details.BAL_MTRS).Value = ""

                For i = 0 To .RowCount - 1
                    .Rows(i).Cells(DgvCol_Details.SNO).Value = i + 1
                Next i

                dgv_Details.Focus()
                If n <= dgv_Details.Rows.Count - 1 Then
                    dgv_Details.CurrentCell = dgv_Details.Rows(n).Cells(DgvCol_Details.LOOM_NO)
                End If
                If Cbo_Grid_Loom_No.Visible = True Then
                    Cbo_Grid_Loom_No.BringToFront()
                    Cbo_Grid_Loom_No.Focus()
                End If

            End If

        End With

        'cbo_LoomNo.Tag = cbo_LoomNo.Text
        'cbo_Pcs_LastPiece_Status.Tag = cbo_Pcs_LastPiece_Status.Text

        Dt1.Dispose()
        Da1.Dispose()

        Dt2.Dispose()
        Da2.Dispose()

    End Sub

    Private Sub cbo_ClothName1_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ClothName1.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")
    End Sub

    Private Sub cbo_ClothName1_KeyUp(sender As Object, e As KeyEventArgs) Handles cbo_ClothName1.KeyUp
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

    Private Sub cbo_ClothName2_GotFocus(sender As Object, e As EventArgs) Handles cbo_ClothName2.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")
    End Sub

    Private Sub cbo_ClothName3_GotFocus(sender As Object, e As EventArgs) Handles cbo_ClothName3.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")
    End Sub

    Private Sub cbo_ClothName4_GotFocus(sender As Object, e As EventArgs) Handles cbo_ClothName4.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")
    End Sub

    Private Sub cbo_ClothName2_KeyUp(sender As Object, e As KeyEventArgs) Handles cbo_ClothName2.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Cloth_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_ClothName2.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_ClothName3_KeyUp(sender As Object, e As KeyEventArgs) Handles cbo_ClothName3.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Cloth_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_ClothName3.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_ClothName4_KeyUp(sender As Object, e As KeyEventArgs) Handles cbo_ClothName4.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Cloth_Creation
            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_ClothName4.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""
            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub Cbo_Grid_Loom_No_KeyUp(sender As Object, e As KeyEventArgs) Handles Cbo_Grid_Loom_No.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New LoomNo_Creation
            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = sender.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""
            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

End Class

