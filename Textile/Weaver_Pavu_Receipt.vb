Public Class Weaver_Pavu_Receipt
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "PVREC-"
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

    Private vCLO_CONDT As String = ""
    Private FnYearCode1 As String = ""
    Private FnYearCode2 As String = ""

    Private Sub clear()
        New_Entry = False
        Insert_Entry = False

        chk_Verified_Status.Checked = False

        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        pnl_Selection.Visible = False
        chk_SelectAll.Checked = False
        Grp_EWB.Visible = False

        lbl_RecNo.Text = ""
        lbl_RecNo.ForeColor = Color.Black
        msk_date.Text = ""
        dtp_Date.Text = ""
        txt_KuraiPavuBeam.Text = ""
        txt_KuraiPavuMeters.Text = ""
        txt_Freight.Text = ""
        txt_Party_DcNo.Text = ""
        txt_Note.Text = ""
        txt_NoOfBobin.Text = ""

        cbo_EndsCount.Text = ""
        cbo_Transport.Text = ""
        cbo_VehicleNo.Text = ""
        cbo_RecForm.Text = ""

        txt_EWBNo.Text = ""
        chk_GSTTax_Invocie.Checked = True
        txt_Amount.Text = ""
        txt_rate.Text = ""


        cbo_weaving_job_no.Text = ""
        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))

        If cbo_WidthType.Visible Then cbo_WidthType.Text = ""

        dgv_PavuDetails.Rows.Clear()
        dgv_PavuDetails_Total.Rows.Clear()
        dgv_PavuDetails_Total.Rows.Add()

        cbo_RecForm.Enabled = True
        cbo_RecForm.BackColor = Color.White

        cbo_ClothSales_OrderCode_forSelection.Text = ""

        If Filter_Status = False Then
            dtp_Filter_Fromdate.Text = ""
            dtp_Filter_ToDate.Text = ""
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_EndsCountName.Text = ""

            dgv_Filter_Details.Rows.Clear()
        End If
        pnl_Delivery_Selection.Visible = False
        lbl_Delivery_Code.Text = ""
        cbo_Type.Text = "DIRECT"
    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox
        Dim Msktxbx As MaskedTextBox
        On Error Resume Next

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

        If Me.ActiveControl.Name <> dgv_PavuDetails_Total.Name Then
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
            End If
        End If

    End Sub

    Private Sub Grid_DeSelect()
        On Error Resume Next
        If Not IsNothing(dgv_PavuDetails.CurrentCell) Then dgv_PavuDetails.CurrentCell.Selected = False
        If Not IsNothing(dgv_PavuDetails_Total.CurrentCell) Then dgv_PavuDetails_Total.CurrentCell.Selected = False
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
        If Not IsNothing(dgv_PavuDetails.CurrentCell) Then dgv_PavuDetails.CurrentCell.Selected = False
        If Not IsNothing(dgv_PavuDetails_Total.CurrentCell) Then dgv_PavuDetails_Total.CurrentCell.Selected = False
        If Not IsNothing(dgv_Filter_Details.CurrentCell) Then dgv_Filter_Details.CurrentCell.Selected = False
    End Sub

    Private Sub Weaver_Pavu_Receipt_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Dim da As SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NoofComps As Integer
        Dim CompCondt As String

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_EndsCount.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "ENDSCOUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_EndsCount.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Transport.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Transport.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_RecForm.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_RecForm.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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
        cbo_VehicleNo.DataSource = dt7
        cbo_VehicleNo.DisplayMember = "Vechile_No"

        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead Where (ledger_type = 'TRANSPORT' or Ledger_IdNo = 0) order by Ledger_DisplayName", con)
        da.Fill(dt2)
        cbo_Transport.DataSource = dt2
        cbo_Transport.DisplayMember = "Ledger_DisplayName"

        da = New SqlClient.SqlDataAdapter("select distinct(EndsCount_Name) from EndsCount_Head order by EndsCount_Name", con)
        da.Fill(dt3)
        cbo_EndsCount.DataSource = dt3
        cbo_EndsCount.DisplayMember = "EndsCount_Name"


        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or Ledger_Type = 'WEAVER' or Ledger_Type = 'SIZING' or Ledger_Type = 'JOBWORKER') and Close_status = 0 order by Ledger_DisplayName", con)
        da.Fill(dt8)
        cbo_RecForm.DataSource = dt8
        cbo_RecForm.DisplayMember = "Ledger_DisplayName"

        lbl_Bobin.Visible = False
        txt_NoOfBobin.Visible = False
        If Common_Procedures.settings.Weaver_Zari_Kuri_Entries_Status = 1 Then
            lbl_Bobin.Visible = True
            txt_NoOfBobin.Visible = True
        End If
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1235" Then '---- SOMANUR KALPANA COTTON (INDIA) PVT LTD (SOMANUR)  --TETILES
            dgv_PavuDetails.Columns(6).HeaderText = "YARDS"

        End If

        If Common_Procedures.settings.Textile_Sizing_Delivery_receipt_Selection = 1 Then

            cbo_Type.Visible = True

            lbl_type_caption.Visible = True
            btn_Delivery_Selection.Visible = True

            Label5.Location = New Point(240, 12)
            Label17.Location = New Point(272, 12)
            msk_date.Location = New Point(318, 8)
            dtp_Date.Location = New Point(435, 8)
            lbl_RecNo.Width = 124


        End If

        btn_UserModification.Visible = False
        If Common_Procedures.settings.User_Modifications_Show_Status = 1 Then
            If Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_UserModification_Status = 1 Then
                btn_UserModification.Visible = True
            End If
        End If
        cbo_WidthType.Items.Clear()
        cbo_WidthType.Items.Add("")
        cbo_WidthType.Items.Add("SINGLE")
        cbo_WidthType.Items.Add("DOUBLE")
        cbo_WidthType.Items.Add("TRIPLE")
        cbo_WidthType.Items.Add("FOURTH")

        cbo_Type.Items.Clear()
        cbo_Type.Items.Add(" ")
        cbo_Type.Items.Add("DIRECT")
        cbo_Type.Items.Add("DELIVERY")

        dtp_Date.Text = ""
        msk_date.Text = ""
        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2

        pnl_Selection.Visible = False
        pnl_Selection.Left = (Me.Width - pnl_Selection.Width) \ 2
        pnl_Selection.Top = (Me.Height - pnl_Selection.Height) \ 2

        pnl_Delivery_Selection.Visible = False
        pnl_Delivery_Selection.Left = (Me.Width - pnl_Delivery_Selection.Width) \ 2
        pnl_Delivery_Selection.Top = (Me.Height - pnl_Delivery_Selection.Height) \ 2
        pnl_Delivery_Selection.BringToFront()


        cbo_WidthType.Visible = False
        lbl_Widthtype.Visible = False
        Label18.Visible = False
        If Common_Procedures.settings.AutoLoom_PavuWidthWiseConsumption_IN_Delivery = 1 Then
            cbo_WidthType.Visible = True
            lbl_Widthtype.Visible = True
            Label18.Visible = True

        End If

        pnl_OwnOrderSelection.Visible = False
        pnl_OwnOrderSelection.Left = (Me.Width - pnl_OwnOrderSelection.Width) \ 2
        pnl_OwnOrderSelection.Top = (Me.Height - pnl_OwnOrderSelection.Height) \ 2
        pnl_OwnOrderSelection.BringToFront()

        lbl_weaving_job_no.Visible = False
        cbo_weaving_job_no.Visible = False

        chk_Verified_Status.Visible = False
        If Common_Procedures.settings.Vefified_Status = 1 Then
            If Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_Verified_Status = 1 Then
                chk_Verified_Status.Visible = True
                lbl_verfied_sts.Visible = True
                cbo_Verified_Sts.Visible = True


            End If
        Else
            chk_Verified_Status.Visible = False
            lbl_verfied_sts.Visible = False
            cbo_Verified_Sts.Visible = False
        End If

        If Common_Procedures.settings.Show_Weaver_JobCard_Entry_STATUS = 1 Then
            lbl_weaving_job_no.Visible = True
            cbo_weaving_job_no.Visible = True

            cbo_Transport.Width = cbo_EndsCount.Width
            cbo_weaving_job_no.BackColor = Color.White

        End If


        If Common_Procedures.settings.Show_Sales_OrderNumber_in_ALLEntry_Status = 1 Then

            cbo_ClothSales_OrderCode_forSelection.Visible = True
            cbo_ClothSales_OrderCode_forSelection.BackColor = Color.White
            lbl_ClothSales_OrderCode_forSelection_Caption.Visible = True


            FnYearCode1 = ""
            FnYearCode2 = ""
            Common_Procedures.get_FnYearCode_of_Last_2_Years(FnYearCode1, FnYearCode2)


            cbo_WidthType.Width = cbo_EndsCount.Width


        Else

            cbo_ClothSales_OrderCode_forSelection.Visible = False
            lbl_ClothSales_OrderCode_forSelection_Caption.Visible = False

        End If




        AddHandler chk_Verified_Status.GotFocus, AddressOf ControlGotFocus
        AddHandler chk_Verified_Status.LostFocus, AddressOf ControlLostFocus


        AddHandler txt_rate.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_RecForm.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Transport.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_VehicleNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_EndsCount.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_KuraiPavuBeam.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_KuraiPavuMeters.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Freight.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Party_DcNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Note.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_NoOfBobin.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_WidthType.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ClothSales_OrderCode_forSelection.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_EndsCountName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Type.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_weaving_job_no.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_rate.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_RecForm.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Transport.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_VehicleNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_EndsCount.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_KuraiPavuBeam.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_KuraiPavuMeters.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Freight.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Party_DcNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Note.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_WidthType.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ClothSales_OrderCode_forSelection.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_EndsCountName.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_NoOfBobin.LostFocus, AddressOf ControlLostFocus


        AddHandler cbo_Type.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_weaving_job_no.LostFocus, AddressOf ControlLostFocus

        'AddHandler msk_date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_KuraiPavuBeam.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_KuraiPavuMeters.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Party_DcNo.KeyDown, AddressOf TextBoxControlKeyDown

        'AddHandler msk_date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_KuraiPavuBeam.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_KuraiPavuMeters.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Party_DcNo.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_NoOfBobin.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_NoOfBobin.KeyPress, AddressOf TextBoxControlKeyPress


        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True
        new_record()

    End Sub

    Private Sub Weaver_Pavu_Receipt_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub Weaver_Pavu_Receipt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
                    Exit Sub

                ElseIf pnl_Selection.Visible = True Then
                    btn_Close_Selection_Click(sender, e)
                    Exit Sub
                ElseIf pnl_Delivery_Selection.Visible = True Then
                    btn_Close_Delivery_Selection_Click(sender, e)
                    Exit Sub
                ElseIf Grp_EWB.Visible = True Then
                    btn_Close_EWB_Click(sender, e)
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

    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
        Dim dgv1 As New DataGridView

        On Error Resume Next

        If ActiveControl.Name = dgv_PavuDetails.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            dgv1 = Nothing

            If ActiveControl.Name = dgv_PavuDetails.Name Then
                dgv1 = dgv_PavuDetails

            ElseIf dgv_PavuDetails.IsCurrentRowDirty = True Then
                dgv1 = dgv_PavuDetails

            ElseIf pnl_Back.Enabled = True Then
                dgv1 = dgv_PavuDetails

            End If

            If IsNothing(dgv1) = False Then

                With dgv1

                    If keyData = Keys.Enter Or keyData = Keys.Down Then
                        If .CurrentCell.ColumnIndex >= 8 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                txt_Note.Focus()

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(7)

                            End If

                        Else

                            If .CurrentCell.ColumnIndex < 7 Then
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(7)

                            Else
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                            End If

                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then

                        If .CurrentCell.ColumnIndex <= 7 Then
                            If .CurrentCell.RowIndex = 0 Then

                                If cbo_ClothSales_OrderCode_forSelection.Visible = True Then
                                    cbo_ClothSales_OrderCode_forSelection.Focus()

                                Else
                                    txt_Freight.Focus()

                                End If




                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(8)

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

    Private Sub move_record(ByVal no As String)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim NewCode As String
        Dim n As Integer
        Dim SNo As Integer
        Dim LockSTS As Boolean = False

        If Val(no) = 0 Then Exit Sub

        clear()

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name as RecFromName, c.Ledger_Name as TransportName, d.EndsCount_Name from Weaver_Pavu_Receipt_Head a INNER JOIN Ledger_Head b ON a.ReceivedFrom_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Ledger_Head c ON a.Transport_IdNo = c.Ledger_IdNo LEFT OUTER JOIN EndsCount_Head d ON a.EndsCount_IdNo = d.EndsCount_IdNo Where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_Pavu_Receipt_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_RecNo.Text = dt1.Rows(0).Item("Weaver_Pavu_Receipt_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Weaver_Pavu_Receipt_Date").ToString
                msk_date.Text = dtp_Date.Text
                If Val(dt1.Rows(0).Item("Empty_Beam").ToString) <> 0 Then
                    txt_KuraiPavuBeam.Text = Val(dt1.Rows(0).Item("Empty_Beam").ToString)
                End If
                If Val(dt1.Rows(0).Item("Pavu_Meters").ToString) <> 0 Then
                    txt_KuraiPavuMeters.Text = Val(dt1.Rows(0).Item("Pavu_Meters").ToString)
                End If
                txt_NoOfBobin.Text = dt1.Rows(0).Item("Empty_Bobin").ToString

                cbo_EndsCount.Text = dt1.Rows(0).Item("EndsCount_Name").ToString
                cbo_VehicleNo.Text = dt1.Rows(0).Item("Vechile_No").ToString
                cbo_Transport.Text = dt1.Rows(0).Item("TransportName").ToString
                cbo_RecForm.Text = dt1.Rows(0).Item("RecFromName").ToString
                If Val(dt1.Rows(0).Item("Freight").ToString) <> 0 Then
                    txt_Freight.Text = Val(dt1.Rows(0).Item("Freight").ToString)
                End If
                cbo_WidthType.Text = dt1.Rows(0).Item("Width_Type").ToString

                cbo_weaving_job_no.Text = dt1.Rows(0).Item("Weaving_JobCode_forSelection").ToString

                txt_Party_DcNo.Text = dt1.Rows(0).Item("Party_DcNo").ToString
                txt_Note.Text = dt1.Rows(0).Item("Note").ToString
                lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))

                If Val(dt1.Rows(0).Item("Verified_Status").ToString) = 1 Then chk_Verified_Status.Checked = True
                cbo_Type.Text = dt1.Rows(0).Item("Selection_type").ToString

                lbl_Delivery_Code.Text = Trim(dt1.Rows(0).Item("Delivery_Code").ToString)

                cbo_ClothSales_OrderCode_forSelection.Text = dt1.Rows(0).Item("ClothSales_OrderCode_forSelection").ToString


                txt_EWBNo.Text = dt1.Rows(0).Item("EwayBill_No").ToString

                If Trim(txt_EWBNo.Text) <> "" Then

                    chk_Ewb_No_Sts.Checked = True
                Else
                    chk_Ewb_No_Sts.Checked = False
                End If

                If Val(dt1.Rows(0).Item("GST_Tax_Invoice_Status").ToString) = 1 Then chk_GSTTax_Invocie.Checked = True Else chk_GSTTax_Invocie.Checked = False
                txt_rate.Text = dt1.Rows(0).Item("Rate").ToString
                txt_Amount.Text = dt1.Rows(0).Item("Net_Amount").ToString


                da2 = New SqlClient.SqlDataAdapter("Select a.*, b.Pavu_Delivery_Increment, c.EndsCount_Name, d.Beam_Width_Name from Weaver_Pavu_Receipt_Details a INNER JOIN Stock_SizedPavu_Processing_Details b ON a.Set_Code = b.Set_Code and (case when a.New_Receipt_BeamNo <> '' then a.New_Receipt_BeamNo else a.Beam_No end) = b.Beam_No INNER JOIN EndsCount_Head c ON a.EndsCount_IdNo = c.EndsCount_IdNo LEFT OUTER JOIN Beam_Width_Head d ON a.Beam_Width_Idno = d.Beam_Width_Idno where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_Pavu_Receipt_Code = '" & Trim(NewCode) & "' Order by a.Sl_No", con)
                'da2 = New SqlClient.SqlDataAdapter("Select a.*, b.Pavu_Delivery_Increment, c.EndsCount_Name, d.Beam_Width_Name from Weaver_Pavu_Receipt_Details a INNER JOIN Stock_SizedPavu_Processing_Details b ON a.Set_Code = b.Set_Code and a.Beam_No = b.Beam_No INNER JOIN EndsCount_Head c ON a.EndsCount_IdNo = c.EndsCount_IdNo LEFT OUTER JOIN Beam_Width_Head d ON a.Beam_Width_Idno = d.Beam_Width_Idno where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_Pavu_Receipt_Code = '" & Trim(NewCode) & "' Order by a.Sl_No", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                With dgv_PavuDetails

                    .Rows.Clear()

                    SNo = 0

                    If dt2.Rows.Count > 0 Then

                        For i = 0 To dt2.Rows.Count - 1

                            n = .Rows.Add()

                            SNo = SNo + 1
                            .Rows(n).Cells(0).Value = Val(SNo)
                            .Rows(n).Cells(1).Value = dt2.Rows(i).Item("Set_No").ToString
                            .Rows(n).Cells(2).Value = dt2.Rows(i).Item("Beam_No").ToString
                            .Rows(n).Cells(3).Value = dt2.Rows(i).Item("EndsCount_Name").ToString
                            If Val(dt2.Rows(i).Item("Pcs").ToString) <> 0 Then
                                .Rows(n).Cells(4).Value = dt2.Rows(i).Item("Pcs").ToString
                            End If
                            If Val(dt2.Rows(i).Item("Meters_Pc").ToString) <> 0 Then
                                .Rows(n).Cells(5).Value = dt2.Rows(i).Item("Meters_Pc").ToString
                            End If
                            If Val(dt2.Rows(i).Item("Meters").ToString) <> 0 Then
                                .Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Meters").ToString), "########0.00")
                            End If
                            If Val(dt2.Rows(i).Item("Rcpt_Pcs").ToString) <> 0 Then
                                .Rows(n).Cells(7).Value = dt2.Rows(i).Item("Rcpt_Pcs").ToString
                            End If
                            If Val(dt2.Rows(i).Item("Rcpt_Meters").ToString) <> 0 Then
                                .Rows(n).Cells(8).Value = Format(Val(dt2.Rows(i).Item("Rcpt_Meters").ToString), "########0.00")
                            End If
                            .Rows(n).Cells(9).Value = dt2.Rows(i).Item("Beam_Width_Name").ToString
                            .Rows(n).Cells(10).Value = ""
                            .Rows(n).Cells(11).Value = dt2.Rows(i).Item("Noof_Used").ToString
                            .Rows(n).Cells(12).Value = dt2.Rows(i).Item("set_code").ToString
                            .Rows(n).Cells(13).Value = dt2.Rows(i).Item("Pavu_Delivery_Increment").ToString

                            If Val(.Rows(n).Cells(11).Value) > 0 And Val(.Rows(n).Cells(11).Value) <> Val(.Rows(n).Cells(13).Value) Then

                                LockSTS = True

                                .Rows(n).Cells(10).Value = "1"

                                For j = 0 To .ColumnCount - 1
                                    .Rows(n).Cells(j).Style.ForeColor = Color.Red
                                    .Rows(n).Cells(j).Style.BackColor = Color.LightGray
                                Next

                            End If

                        Next i

                    End If

                End With

                With dgv_PavuDetails_Total
                    If .RowCount = 0 Then .Rows.Add()
                    .Rows(0).Cells(2).Value = Val(dt1.Rows(0).Item("Total_Beam").ToString)
                    .Rows(0).Cells(4).Value = Val(dt1.Rows(0).Item("Total_Pcs").ToString)
                    .Rows(0).Cells(6).Value = Format(Val(dt1.Rows(0).Item("Total_Meters").ToString), "########0.00")
                    .Rows(0).Cells(7).Value = Val(dt1.Rows(0).Item("Total_Rcpt_Pcs").ToString)
                    .Rows(0).Cells(8).Value = Format(Val(dt1.Rows(0).Item("Total_Rcpt_Meters").ToString), "########0.00")
                End With

                dt2.Clear()

            End If

            dt1.Clear()
            dt1.Dispose()
            da1.Dispose()

            If LockSTS = True Then
                cbo_RecForm.Enabled = False
                cbo_RecForm.BackColor = Color.LightGray
            End If

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

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_RecNo.Text)

        '   If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Weaver_Pavu_Rceipt_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Weaver_Pavu_Rceipt_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub
        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.Weaver_Pavu_Rceipt_Entry, New_Entry, Me, con, "Weaver_Pavu_Receipt_Head", "Weaver_Pavu_Receipt_Code", NewCode, "Weaver_Pavu_Receipt_Date", "(Weaver_Pavu_Receipt_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub

        If Common_Procedures.settings.Vefified_Status = 1 Then
            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            If Val(Common_Procedures.get_FieldValue(con, "Weaver_Pavu_Receipt_Head", "Verified_Status", "(Weaver_Pavu_Receipt_Code = '" & Trim(NewCode) & "')")) = 1 Then
                MessageBox.Show("Entry Already Verified", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub
            End If
        End If


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

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.Transaction = trans
            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "DELETE", "Weaver_Pavu_Receipt_head", "Weaver_Pavu_Receipt_Code", Val(lbl_Company.Tag), NewCode, lbl_RecNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "", "", "Weaver_Pavu_Receipt_Code, Company_IdNo, for_OrderBy", trans)
            Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "DELETE", "Weaver_Pavu_Receipt_Details", "Weaver_Pavu_Receipt_Code", Val(lbl_Company.Tag), NewCode, lbl_RecNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, " Set_No,Beam_No,EndsCount_IdNo,Pcs,Meters_Pc,Meters,Rcpt_Pcs,Rcpt_Meters,Beam_Width_Idno  ,Noof_Used,Set_Code,New_Receipt_BeamNo", "Sl_No", "Weaver_Pavu_Receipt_Code, For_OrderBy, Company_IdNo, Weaver_Pavu_Receipt_No, Weaver_Pavu_Receipt_Date, Ledger_Idno", trans)

            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), trans)

            Da = New SqlClient.SqlDataAdapter("Select * from Weaver_Pavu_Receipt_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Pavu_Receipt_Code = '" & Trim(NewCode) & "'", con)
            Da.SelectCommand.Transaction = trans
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then
                For i = 0 To Dt1.Rows.Count - 1

                    If Val(Dt1.Rows(i).Item("Meters").ToString) = Val(Dt1.Rows(i).Item("Rcpt_Meters").ToString) Then

                        Nr = 0
                        cmd.CommandText = "update Stock_SizedPavu_Processing_Details set " _
                                  & " StockAt_IdNo = " & Str(Val(Dt1.Rows(i).Item("ReceivedFrom_IdNo").ToString)) & ", " _
                                  & " Pavu_Delivery_Increment = Pavu_Delivery_Increment - 1 " _
                                  & " Where " _
                                  & " StockAt_IdNo = " & Str(Val(Dt1.Rows(i).Item("DeliveryTo_IdNo").ToString)) & " and " _
                                  & " Set_Code = '" & Trim(Dt1.Rows(i).Item("Set_Code").ToString) & "' and " _
                                  & " beam_no = '" & Trim(Dt1.Rows(i).Item("Beam_No").ToString) & "' and " _
                                  & " Pavu_Delivery_Increment = " & Str(Val(Dt1.Rows(i).Item("Noof_Used").ToString)) & " and " _
                                  & " Pavu_Delivery_Code = ''"
                        Nr = cmd.ExecuteNonQuery
'----[THANGES] - check pavu selection - it has some problem, i think
                        If Common_Procedures.settings.Textile_Sizing_Delivery_receipt_Selection = 0 Then
                            If Nr = 0 Then
                                Throw New ApplicationException("Some Beams Delivered to Others - BeamNo : " & Trim(Dt1.Rows(i).Item("Beam_No").ToString))
                                Exit Sub
                            End If
                        End If


                    Else

                            cmd.CommandText = "update Stock_SizedPavu_Processing_Details set " _
                                  & " StockAt_IdNo = " & Str(Val(Dt1.Rows(i).Item("ReceivedFrom_IdNo").ToString)) & ", " _
                                  & " Pavu_Delivery_Increment = Pavu_Delivery_Increment - 1, " _
                                  & " Pavu_Delivery_Code = '' " _
                                  & " Where " _
                                  & " Pavu_Delivery_Code = '" & Trim(NewCode) & "' and " _
                                  & " StockAt_IdNo = 0 and " _
                                  & " Set_Code = '" & Trim(Dt1.Rows(i).Item("Set_Code").ToString) & "' and " _
                                  & " beam_no = '" & Trim(Dt1.Rows(i).Item("Beam_No").ToString) & "' and " _
                                  & " Pavu_Delivery_Increment = " & Str(Val(Dt1.Rows(i).Item("Noof_Used").ToString))
                        cmd.ExecuteNonQuery()

                        Nr = 0
                        cmd.CommandText = "Delete from Stock_SizedPavu_Processing_Details " _
                                  & " Where " _
                                  & " Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and " _
                                  & " StockAt_IdNo = " & Str(Val(Dt1.Rows(i).Item("DeliveryTo_IdNo").ToString)) & " and " _
                                  & " Set_Code = '" & Trim(Dt1.Rows(i).Item("Set_Code").ToString) & "' and " _
                                  & " beam_no = '" & Trim(Dt1.Rows(i).Item("New_Receipt_BeamNo").ToString) & "' and " _
                                  & " Pavu_Delivery_Increment = " & Str(Val(Dt1.Rows(i).Item("Noof_Used").ToString)) & " and " _
                                  & " Pavu_Delivery_Code = ''"
                        Nr = cmd.ExecuteNonQuery()
'----[THANGES] - check pavu selection - it has some problem, i think
                        If Common_Procedures.settings.Textile_Sizing_Delivery_receipt_Selection = 0 Then
                            If Nr = 0 Then
                                Throw New ApplicationException("Some Beams Delivered to Others - BeamNo : " & Trim(Dt1.Rows(i).Item("New_Receipt_BeamNo").ToString))
                                Exit Sub
                            End If
                        End If


                    End If

                Next

            End If
            Dt1.Clear()

            cmd.CommandText = "Delete from Stock_Pavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Empty_BeamBagCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Weaver_Pavu_Receipt_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Pavu_Receipt_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "delete from Weaver_Pavu_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Pavu_Receipt_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Pavu_Delivery_Selections_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
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
            cbo_Filter_PartyName.DataSource = dt1
            cbo_Filter_PartyName.DisplayMember = "Ledger_DisplayName"


            da = New SqlClient.SqlDataAdapter("select distinct(EndsCount_Name) from EndsCount_Head order by EndsCount_Name", con)
            da.Fill(dt3)
            cbo_Filter_EndsCountName.DataSource = dt3
            cbo_Filter_EndsCountName.DisplayMember = "EndsCount_Name"

            dtp_Filter_Fromdate.Text = ""
            dtp_Filter_ToDate.Text = ""
            cbo_Filter_PartyName.Text = ""

            cbo_Filter_EndsCountName.Text = ""

            cbo_Filter_PartyName.SelectedIndex = -1

            cbo_Filter_EndsCountName.SelectedIndex = -1

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

            da = New SqlClient.SqlDataAdapter("select top 1 Weaver_Pavu_Receipt_No from Weaver_Pavu_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Pavu_Receipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Weaver_Pavu_Receipt_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RecNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Weaver_Pavu_Receipt_No from Weaver_Pavu_Receipt_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Pavu_Receipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Weaver_Pavu_Receipt_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RecNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Weaver_Pavu_Receipt_No from Weaver_Pavu_Receipt_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Pavu_Receipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Weaver_Pavu_Receipt_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 Weaver_Pavu_Receipt_No from Weaver_Pavu_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Pavu_Receipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Weaver_Pavu_Receipt_No desc", con)
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

            lbl_RecNo.Text = Common_Procedures.get_MaxCode(con, "Weaver_Pavu_Receipt_Head", "Weaver_Pavu_Receipt_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

            lbl_RecNo.ForeColor = Color.Red


            msk_date.Text = Date.Today.ToShortDateString
            da = New SqlClient.SqlDataAdapter("select top 1 * from Weaver_Pavu_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Pavu_Receipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Weaver_Pavu_Receipt_No desc", con)
            dt1 = New DataTable
            da.Fill(dt1)
            If dt1.Rows.Count > 0 Then
                If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)
                    If dt1.Rows(0).Item("Weaver_Pavu_Receipt_Date").ToString <> "" Then msk_date.Text = dt1.Rows(0).Item("Weaver_Pavu_Receipt_Date").ToString
                End If
                If Val(dt1.Rows(0).Item("GST_Tax_Invoice_Status").ToString) = 1 Then chk_GSTTax_Invocie.Checked = True Else chk_GSTTax_Invocie.Checked = False

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

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Weaver_Pavu_Receipt_No from Weaver_Pavu_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Pavu_Receipt_Code = '" & Trim(RecCode) & "'", con)
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

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Weaver_Pavu_Rceipt_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Weaver_Pavu_Rceipt_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.Weaver_Pavu_Rceipt_Entry, New_Entry, Me) = False Then Exit Sub

        Try

            inpno = InputBox("Enter New Rec No.", "FOR NEW RECEIPT INSERTION...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Weaver_Pavu_Receipt_No from Weaver_Pavu_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Pavu_Receipt_Code = '" & Trim(RecCode) & "'", con)
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
                    lbl_RecNo.Text = Trim(UCase(inpno))

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
        Dim Stock_In As String
        Dim mtrspcs As Double
        Dim dt2 As New DataTable
        Dim vTotPvuStk As Single = 0, vTotPvuStkAlLoomMtr As Single = 0
        Dim Stk_DelvMtr As Single, Stk_RecMtr As Single
        Dim vWdTyp As Single = 0
        Dim Delv_Ledtype As String = ""
        Dim Rec_Ledtype As String = ""
        Dim vPVUSTK_ENDSID As Integer = 0
        Dim vSELC_DCCODE As String = ""
        Dim Verified_STS As String = ""
        Dim vOrdByNo As String = ""
        Dim vCOMP_LEDIDNO As Integer = 0
        Dim vDELVLED_COMPIDNO As Integer = 0

        Dim Weaver_Job_Code As String = ""
        Dim vGST_Tax_Inv_Sts As Integer = 0


        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_RecNo.Text)


        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If


        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        '  If Common_Procedures.UserRight_Check(Common_Procedures.UR.Weaver_Pavu_Rceipt_Entry, New_Entry) = False Then Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Weaver_Pavu_Rceipt_Entry, New_Entry, Me, con, "Weaver_Pavu_Receipt_Head", "Weaver_Pavu_Receipt_Code", NewCode, "Weaver_Pavu_Receipt_Date", "(Weaver_Pavu_Receipt_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Pavu_Receipt_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, Weaver_Pavu_Receipt_No desc", dtp_Date.Value.Date) = False Then Exit Sub


        If Common_Procedures.settings.Vefified_Status = 1 Then
            If Not (Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_Verified_Status = 1) Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

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

        Rec_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_RecForm.Text)
        If Rec_ID = 0 Then
            MessageBox.Show("Invalid Delivery Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_RecForm.Enabled And cbo_RecForm.Visible Then cbo_RecForm.Focus()
            Exit Sub
        End If

        Delv_ID = Val(Common_Procedures.CommonLedger.Godown_Ac)

        Trans_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Transport.Text)
        KuPvu_EdsCnt_ID = Common_Procedures.EndsCount_NameToIdNo(con, cbo_EndsCount.Text)
        lbl_UserName.Text = Common_Procedures.User.IdNo

        If KuPvu_EdsCnt_ID = 0 And Val(txt_KuraiPavuMeters.Text) <> 0 Then
            MessageBox.Show("Invalid Ends Count", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_EndsCount.Enabled And cbo_EndsCount.Visible Then cbo_EndsCount.Focus()
            Exit Sub
        End If

        Verified_STS = 0
        If chk_Verified_Status.Checked = True Then Verified_STS = 1


        If cbo_WidthType.Visible And cbo_WidthType.Text = "" Then
            MessageBox.Show("Invalid Width Type", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_WidthType.Enabled And cbo_WidthType.Visible Then cbo_WidthType.Focus()
            Exit Sub
        End If

        If Trim(txt_Party_DcNo.Text) <> "" Then
            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            Da = New SqlClient.SqlDataAdapter("select * from Weaver_Pavu_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and DeliveryTo_IdNo = " & Str(Val(Delv_ID)) & " and Party_dcno = '" & Trim(txt_Party_DcNo.Text) & "' and Weaver_Pavu_Receipt_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' and Weaver_Pavu_Receipt_Code <> '" & Trim(NewCode) & "'", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                MessageBox.Show("Duplicate Party Dc No to this Party", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If txt_Party_DcNo.Enabled And txt_Party_DcNo.Visible Then txt_Party_DcNo.Focus()
                Exit Sub
            End If
            Dt1.Clear()
        End If

        With dgv_PavuDetails

            For i = 0 To .RowCount - 1

                If Val(dgv_PavuDetails.Rows(i).Cells(6).Value) <> 0 Or Val(dgv_PavuDetails.Rows(i).Cells(8).Value) <> 0 Then

                    If Trim(.Rows(i).Cells(1).Value) = "" Then
                        MessageBox.Show("Invalid Set No", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(1)
                            .CurrentCell.Selected = True
                        End If
                        Exit Sub
                    End If

                    If Trim(.Rows(i).Cells(12).Value) = "" Then
                        MessageBox.Show("Invalid Set Code", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(1)
                            .CurrentCell.Selected = True
                        End If
                        Exit Sub
                    End If

                    If Trim(.Rows(i).Cells(2).Value) = "" Then
                        MessageBox.Show("Invalid Beam No", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(2)
                            .CurrentCell.Selected = True
                        End If
                        Exit Sub
                    End If

                    If Trim(.Rows(i).Cells(3).Value) = "" Then
                        MessageBox.Show("Invalid Ends Count", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(3)
                            .CurrentCell.Selected = True
                        End If
                        Exit Sub
                    End If

                    vEdsCnt_ID = Common_Procedures.EndsCount_NameToIdNo(con, .Rows(i).Cells(3).Value)
                    If Val(vEdsCnt_ID) = 0 Then
                        MessageBox.Show("Invalid Ends Count", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(3)
                            .CurrentCell.Selected = True
                        End If
                        Exit Sub
                    End If

                    If Val(.Rows(i).Cells(6).Value) = 0 Then
                        MessageBox.Show("Invalid Beam Meters", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(6)
                            .CurrentCell.Selected = True
                        End If
                        Exit Sub
                    End If

                    If Val(.Rows(i).Cells(8).Value) = 0 Then
                        MessageBox.Show("Invalid Receipt Meters", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(8)
                            .CurrentCell.Selected = True
                        End If
                        Exit Sub
                    End If

                End If

            Next

        End With

        TotalPavu_Calculation()

        vTotPvuBms = 0 : vTotPvuMtrs = 0 : vTotPvuPcs = 0 : vTotPvuRctMtrs = 0 : vTotPvuRctPcs = 0
        If dgv_PavuDetails_Total.RowCount > 0 Then
            vTotPvuBms = Val(dgv_PavuDetails_Total.Rows(0).Cells(2).Value())
            vTotPvuPcs = Val(dgv_PavuDetails_Total.Rows(0).Cells(4).Value())
            vTotPvuMtrs = Val(dgv_PavuDetails_Total.Rows(0).Cells(6).Value())

            vTotPvuRctPcs = Val(dgv_PavuDetails_Total.Rows(0).Cells(7).Value())
            vTotPvuRctMtrs = Val(dgv_PavuDetails_Total.Rows(0).Cells(8).Value())

        End If

        Delv_Ledtype = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_Type", "(Ledger_IdNo = " & Str(Val(Delv_ID)) & ")")
        Rec_Ledtype = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_Type", "(Ledger_IdNo = " & Str(Val(Rec_ID)) & ")")


        vSELC_DCCODE = ""
        If Common_Procedures.settings.Textile_Sizing_Delivery_receipt_Selection = 1 And Trim(cbo_Type.Text) = "DELIVERY" Then
            vSELC_DCCODE = Trim(lbl_Delivery_Code.Text)
        End If

        Weaver_Job_Code = ""

        If Trim(cbo_weaving_job_no.Text) <> "" Then
            Weaver_Job_Code = Trim(cbo_weaving_job_no.Text)
        End If

        vGST_Tax_Inv_Sts = 0
        If chk_GSTTax_Invocie.Checked = True Then vGST_Tax_Inv_Sts = 1

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_RecNo.Text = Common_Procedures.get_MaxCode(con, "Weaver_Pavu_Receipt_Head", "Weaver_Pavu_Receipt_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            vForOrdByNo = Val(Common_Procedures.OrderBy_CodeToValue(lbl_RecNo.Text))

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@EntryDate", Convert.ToDateTime(msk_date.Text))


            If New_Entry = True Then
                cmd.CommandText = "Insert into Weaver_Pavu_Receipt_Head ( Weaver_Pavu_Receipt_Code,             Company_IdNo         ,      Weaver_Pavu_Receipt_No   ,             for_OrderBy     , Weaver_Pavu_Receipt_Date,         DeliveryTo_IdNo  ,      ReceivedFrom_IdNo  ,             Empty_Beam         ,           Pavu_Meters         ,             EndsCount_IdNo       ,               Party_DcNo           ,               Vechile_No          ,          Transport_Idno   ,                 Freight           ,            Note              ,            Total_Beam       ,           Total_Pcs         ,             Total_Meters     ,               Total_Rcpt_Pcs    ,            Total_Rcpt_Meters                 , Empty_Bobin                        , Width_Type                       ,            User_IdNo            ,   Verified_Status    ,Selection_type,Delivery_Code , Weaving_JobCode_forSelection                                     ,           EwayBill_No         ,        GST_Tax_Invoice_Status     ,            Rate            , Net_Amount                  ,  ClothSales_OrderCode_forSelection       ) " &
                                                "                Values (  '" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RecNo.Text) & "', " & Str(Val(vForOrdByNo)) & ",          @EntryDate     , " & Str(Val(Delv_ID)) & ", " & Str(Val(Rec_ID)) & ", " & Val(txt_KuraiPavuBeam.Text) & ", " & Val(txt_KuraiPavuMeters.Text) & ", " & Str(Val(KuPvu_EdsCnt_ID)) & ", '" & Trim(txt_Party_DcNo.Text) & "', '" & Trim(cbo_VehicleNo.Text) & "', " & Str(Val(Trans_ID)) & ", " & Str(Val(txt_Freight.Text)) & ", '" & Trim(txt_Note.Text) & "', " & Str(Val(vTotPvuBms)) & ", " & Str(Val(vTotPvuPcs)) & ", " & Str(Val(vTotPvuMtrs)) & ",  " & Str(Val(vTotPvuRctPcs)) & ", " & Str(Val(vTotPvuRctMtrs)) & " ," & Str(Val(txt_NoOfBobin.Text)) & ",'" & Trim(cbo_WidthType.Text) & "', " & Val(lbl_UserName.Text) & " , " & Val(Verified_STS) & ",'" & Trim(cbo_Type.Text) & "','" & Trim(vSELC_DCCODE) & "' , '" & Trim(Weaver_Job_Code) & "' , '" & Trim(txt_EWBNo.Text) & "', " & Str(Val(vGST_Tax_Inv_Sts)) & " ," & Val(txt_rate.Text) & ", " & Val(txt_Amount.Text) & " , '" & Trim(cbo_ClothSales_OrderCode_forSelection.Text) & "'        ) "
                cmd.ExecuteNonQuery()

            Else
                Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "OLD", "Weaver_Pavu_Receipt_head", "Weaver_Pavu_Receipt_Code", Val(lbl_Company.Tag), NewCode, lbl_RecNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Weaver_Pavu_Receipt_Code, Company_IdNo, for_OrderBy", tr)
                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "OLD", "Weaver_Pavu_Receipt_Details", "Weaver_Pavu_Receipt_Code", Val(lbl_Company.Tag), NewCode, lbl_RecNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, " Set_No,Beam_No,EndsCount_IdNo,Pcs,Meters_Pc,Meters,Rcpt_Pcs,Rcpt_Meters,Beam_Width_Idno  ,Noof_Used,Set_Code,New_Receipt_BeamNo", "Sl_No", "Weaver_Pavu_Receipt_Code, For_OrderBy, Company_IdNo, Weaver_Pavu_Receipt_No, Weaver_Pavu_Receipt_Date, Ledger_Idno", tr)


                cmd.CommandText = "Update Weaver_Pavu_Receipt_Head set Weaver_Pavu_Receipt_Date = @EntryDate, DeliveryTo_IdNo = " & Str(Val(Delv_ID)) & ", ReceivedFrom_IdNo = " & Str(Val(Rec_ID)) & ", Party_DcNo = '" & Trim(txt_Party_DcNo.Text) & "', Empty_Beam = " & Str(Val(txt_KuraiPavuBeam.Text)) & ", Pavu_Meters = " & Str(Val(txt_KuraiPavuMeters.Text)) & ",  EndsCount_IdNo = " & Str(Val(KuPvu_EdsCnt_ID)) & ", Vechile_No = '" & Trim(cbo_VehicleNo.Text) & "', Transport_Idno = " & Str(Val(Trans_ID)) & ", Freight = " & Str(Val(txt_Freight.Text)) & ", Note = '" & Trim(txt_Note.Text) & "', Total_Beam = " & Str(Val(vTotPvuBms)) & ", Total_Pcs = " & Str(Val(vTotPvuPcs)) & ", Total_Meters = " & Str(Val(vTotPvuMtrs)) & ", Total_Rcpt_Pcs = " & Str(Val(vTotPvuRctPcs)) & ", Total_Rcpt_Meters = " & Str(Val(vTotPvuRctMtrs)) & " , Empty_Bobin = " & Str(Val(txt_NoOfBobin.Text)) & " ,Width_Type ='" & Trim(cbo_WidthType.Text) & "', User_IdNo = " & Val(lbl_UserName.Text) & ",Verified_Status= " & Val(Verified_STS) & " ,Selection_type='" & Trim(cbo_Type.Text) & "',Delivery_Code='" & Trim(vSELC_DCCODE) & "' ,  Weaving_JobCode_forSelection = '" & Trim(Weaver_Job_Code) & "' ,EwayBill_No = '" & Trim(txt_EWBNo.Text) & "'  ,  GST_Tax_Invoice_Status = " & Str(Val(vGST_Tax_Inv_Sts)) & " , Rate =" & Val(txt_rate.Text) & " , Net_Amount = " & Val(txt_Amount.Text) & " , ClothSales_OrderCode_forSelection = '" & Trim(cbo_ClothSales_OrderCode_forSelection.Text) & "' Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Pavu_Receipt_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                Da = New SqlClient.SqlDataAdapter("Select * from Weaver_Pavu_Receipt_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Pavu_Receipt_Code = '" & Trim(NewCode) & "'", con)
                Da.SelectCommand.Transaction = tr
                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then
                    For i = 0 To Dt1.Rows.Count - 1

                        cmd.CommandText = "update Stock_SizedPavu_Processing_Details set " _
                                  & " StockAt_IdNo = " & Str(Val(Dt1.Rows(i).Item("ReceivedFrom_IdNo").ToString)) & ", " _
                                  & " Pavu_Delivery_Increment = Pavu_Delivery_Increment - 1 " _
                                  & " Where " _
                                  & " StockAt_IdNo = " & Str(Val(Dt1.Rows(i).Item("DeliveryTo_IdNo").ToString)) & " and " _
                                  & " Set_Code = '" & Trim(Dt1.Rows(i).Item("Set_Code").ToString) & "' and " _
                                  & " beam_no = '" & Trim(Dt1.Rows(i).Item("Beam_No").ToString) & "' and " _
                                  & " Pavu_Delivery_Increment = " & Str(Val(Dt1.Rows(i).Item("Noof_Used").ToString)) & " and " _
                                  & " Pavu_Delivery_Code = ''"
                        cmd.ExecuteNonQuery()

                        cmd.CommandText = "update Stock_SizedPavu_Processing_Details set " _
                                  & " StockAt_IdNo = " & Str(Val(Dt1.Rows(i).Item("ReceivedFrom_IdNo").ToString)) & ", " _
                                  & " Pavu_Delivery_Increment = Pavu_Delivery_Increment - 1, " _
                                  & " Pavu_Delivery_Code = '' " _
                                  & " Where " _
                                  & " Pavu_Delivery_Code = '" & Trim(NewCode) & "' and " _
                                  & " StockAt_IdNo = 0 and " _
                                  & " Set_Code = '" & Trim(Dt1.Rows(i).Item("Set_Code").ToString) & "' and " _
                                  & " beam_no = '" & Trim(Dt1.Rows(i).Item("Beam_No").ToString) & "' and " _
                                  & " Pavu_Delivery_Increment = " & Str(Val(Dt1.Rows(i).Item("Noof_Used").ToString))
                        cmd.ExecuteNonQuery()

                        cmd.CommandText = "Delete from Stock_SizedPavu_Processing_Details " _
                                  & " Where " _
                                  & " Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and " _
                                  & " StockAt_IdNo = " & Str(Val(Dt1.Rows(i).Item("DeliveryTo_IdNo").ToString)) & " and " _
                                  & " Set_Code = '" & Trim(Dt1.Rows(i).Item("Set_Code").ToString) & "' and " _
                                  & " beam_no = '" & Trim(Dt1.Rows(i).Item("New_Receipt_BeamNo").ToString) & "' and " _
                                  & " Pavu_Delivery_Increment = " & Str(Val(Dt1.Rows(i).Item("Noof_Used").ToString)) & " and " _
                                  & " Pavu_Delivery_Code = ''"
                        cmd.ExecuteNonQuery()

                    Next
                End If
                Dt1.Clear()

            End If
            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "NEW", "Weaver_Pavu_Receipt_head", "Weaver_Pavu_Receipt_Code", Val(lbl_Company.Tag), NewCode, lbl_RecNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Weaver_Pavu_Receipt_Code, Company_IdNo, for_OrderBy", tr)
            
            EntID = Trim(Pk_Condition) & Trim(lbl_RecNo.Text)
            Partcls = "Rcpt : Rec.No. " & Trim(lbl_RecNo.Text)
            PBlNo = Trim(lbl_RecNo.Text)

            cmd.CommandText = "truncate table " & Trim(Common_Procedures.EntryTempTable) & ""
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Weaver_Pavu_Receipt_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Pavu_Receipt_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Pavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Empty_BeamBagCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()


            cmd.CommandText = "Delete from Pavu_Delivery_Selections_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()





            If Val(txt_KuraiPavuMeters.Text) <> 0 And Val(KuPvu_EdsCnt_ID) <> 0 Then
                cmd.CommandText = "insert into " & Trim(Common_Procedures.EntryTempTable) & "(Int1, Int2, Meters1) values (" & Str(Val(KuPvu_EdsCnt_ID)) & ", " & Str(Val(txt_KuraiPavuBeam.Text)) & ", " & Str(Val(txt_KuraiPavuMeters.Text)) & ")"
                cmd.ExecuteNonQuery()
            End If

            With dgv_PavuDetails

                Sno = 0

                For i = 0 To dgv_PavuDetails.RowCount - 1

                    If Val(.Rows(i).Cells(6).Value) <> 0 And Val(.Rows(i).Cells(8).Value) <> 0 Then

                        Sno = Sno + 1

                        SzPvu_EdsCnt_ID = Common_Procedures.EndsCount_NameToIdNo(con, .Rows(i).Cells(3).Value, tr)
                        Bw_IdNo = Common_Procedures.BeamWidth_NameToIdNo(con, .Rows(i).Cells(9).Value, tr)

                        Ent_NoofUsed = 0
                        New_BmNo = ""
                        If Trim(cbo_Type.Text) <> "DELIVERY" Then


                            If Val(.Rows(i).Cells(6).Value) = Val(.Rows(i).Cells(8).Value) Then
                                '--[THANGES] - on saving pavu selection has some problem

                                If Val(.Rows(i).Cells(11).Value) = 0 Or (Val(.Rows(i).Cells(11).Value) > 0 And Val(.Rows(i).Cells(11).Value) = Val(.Rows(i).Cells(13).Value)) Then

                                    Nr = 0
                                    cmd.CommandText = "update Stock_SizedPavu_Processing_Details set StockAt_IdNo = " & Str(Val(Delv_ID)) & ", Pavu_Delivery_Increment = Pavu_Delivery_Increment + 1 " &
                                                            " Where  Set_Code = '" & Trim(.Rows(i).Cells(12).Value) & "' and Beam_No = '" & Trim(.Rows(i).Cells(2).Value) & "' and StockAt_IdNo = " & Str(Val(Rec_ID))
                                    Nr = cmd.ExecuteNonQuery()

                                    If Nr <> 1 Then
                                        Throw New ApplicationException("Invalid SizedBeam Details" & vbCrLf & "Mismath Party Name and Beam Details - BeamNo : " & Trim(.Rows(i).Cells(2).Value))
                                        Exit Sub
                                    End If

                                    Ent_NoofUsed = Val(Common_Procedures.get_FieldValue(con, "Stock_SizedPavu_Processing_Details", "Pavu_Delivery_Increment", "(Set_Code = '" & Trim(.Rows(i).Cells(12).Value) & "' and Beam_No = '" & Trim(.Rows(i).Cells(2).Value) & "' )", , tr))

                                Else

                                    Ent_NoofUsed = Val(.Rows(i).Cells(11).Value)

                                End If

                            Else

                                Nr = 0
                                cmd.CommandText = "update Stock_SizedPavu_Processing_Details set Pavu_Delivery_Code = '" & Trim(NewCode) & "', StockAt_IdNo = 0, Pavu_Delivery_Increment = Pavu_Delivery_Increment + 1 " &
                                                        " Where  Set_Code = '" & Trim(.Rows(i).Cells(12).Value) & "' and Beam_No = '" & Trim(.Rows(i).Cells(2).Value) & "' and StockAt_IdNo = " & Str(Val(Rec_ID)) & " and Pavu_Delivery_Code = ''"
                                Nr = cmd.ExecuteNonQuery()

                                If Nr <> 1 Then
                                    Throw New ApplicationException("Invalid SizedBeam Details" & vbCrLf & "Mismath Party Name and Beam Details - BeamNo : " & Trim(.Rows(i).Cells(2).Value))
                                    Exit Sub
                                End If

                                Ent_NoofUsed = Val(Common_Procedures.get_FieldValue(con, "Stock_SizedPavu_Processing_Details", "Pavu_Delivery_Increment", "(Set_Code = '" & Trim(.Rows(i).Cells(12).Value) & "' and Beam_No = '" & Trim(.Rows(i).Cells(2).Value) & "')", , tr))

                                Siz_ID = Val(Common_Procedures.get_FieldValue(con, "Stock_SizedPavu_Processing_Details", "Ledger_IdNo", "(Set_Code = '" & Trim(.Rows(i).Cells(12).Value) & "' and Beam_No = '" & Trim(.Rows(i).Cells(2).Value) & "' )", , tr))
                                Selc_SetCode = Common_Procedures.get_FieldValue(con, "Stock_SizedPavu_Processing_Details", "setcode_forSelection", "(Set_Code = '" & Trim(.Rows(i).Cells(12).Value) & "' and Beam_No = '" & Trim(.Rows(i).Cells(2).Value) & "' )", , tr)

                                pCnt_ID = Val(Common_Procedures.get_FieldValue(con, "EndsCount_Head", "Count_IdNo", "(EndsCount_IdNo = " & Str(Val(SzPvu_EdsCnt_ID)) & ")", , tr))
                                pEds_Nm = Val(Common_Procedures.get_FieldValue(con, "EndsCount_Head", "Ends_Name", "(EndsCount_IdNo = " & Str(Val(SzPvu_EdsCnt_ID)) & ")", , tr))

                                Mtr_Pc = 0
                                If Val(.Rows(i).Cells(7).Value) <> 0 Then
                                    Mtr_Pc = Val(.Rows(i).Cells(8).Value) / Val(.Rows(i).Cells(7).Value)
                                End If

                                New_BmNo = Trim(.Rows(i).Cells(2).Value) & "-R"

                                Nr = 0
                                cmd.CommandText = "update Stock_SizedPavu_Processing_Details set Reference_Date = @EntryDate " &
                                                        " Where  Set_Code = '" & Trim(.Rows(i).Cells(12).Value) & "' and Beam_No = '" & Trim(New_BmNo) & "'"
                                Nr = cmd.ExecuteNonQuery()

                                If Nr = 0 Then
                                    cmd.CommandText = "Insert into Stock_SizedPavu_Processing_Details ( Reference_Code,                Company_IdNo      ,              Reference_No     ,             for_OrderBy      , Reference_Date,           Ledger_IdNo   ,         StockAt_IdNo     ,                    Set_Code             ,                    Set_No              ,   setcode_forSelection      ,          Ends_Name     ,            count_idno    ,             EndsCount_IdNo       ,         Beam_Width_Idno           ,                   Sl_No      ,           Beam_No       ,                               ForOrderBy_BeamNo                                 ,                      Noof_Pcs            ,            Meters_Pc    ,                      Meters              , Pavu_Delivery_Code,     Pavu_Delivery_Increment,      Weaving_JobCode_forSelection    ) " &
                                                            " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RecNo.Text) & "', " & Str(Val(vForOrdByNo)) & ",   @EntryDate  , " & Str(Val(Siz_ID)) & ", " & Str(Val(Delv_ID)) & ", '" & Trim(.Rows(i).Cells(12).Value) & "', '" & Trim(.Rows(i).Cells(1).Value) & "', '" & Trim(Selc_SetCode) & "', '" & Trim(pEds_Nm) & "', " & Str(Val(pCnt_ID)) & ", " & Str(Val(SzPvu_EdsCnt_ID)) & "    ,     " & Str(Val(Bw_IdNo)) & "     ,         " & Str(Val(Sno)) & ", '" & Trim(New_BmNo) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(.Rows(i).Cells(2).Value))) & ", " & Str(Val(.Rows(i).Cells(7).Value)) & ", " & Str(Val(Mtr_Pc)) & ", " & Str(Val(.Rows(i).Cells(8).Value)) & ",         ''        , " & Str(Val(Ent_NoofUsed)) & " , '" & Trim(Weaver_Job_Code) & "' ) "
                                    cmd.ExecuteNonQuery()
                                End If

                            End If

                        End If

                        cmd.CommandText = "Insert into Weaver_Pavu_Receipt_Details (Weaver_Pavu_Receipt_Code,              Company_IdNo        ,     Weaver_Pavu_Receipt_No    ,            for_OrderBy       , Weaver_Pavu_Receipt_Date,       DeliveryTo_IdNo    ,      ReceivedFrom_IdNo   ,           Sl_No      ,                    Set_No              ,                    Beam_No             ,              EndsCount_IdNo       ,                      Pcs                  ,                      Meters_Pc           ,                      Meters              ,                      Rcpt_Pcs            ,                      Rcpt_Meters         ,         Beam_Width_Idno  ,             Noof_Used         ,                    Set_Code             ,   New_Receipt_BeamNo     ) " & _
                                                    "          Values  (  '" & Trim(NewCode) & "'           , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RecNo.Text) & "', " & Str(Val(vForOrdByNo)) & ",       @EntryDate        , " & Str(Val(Delv_ID)) & ",  " & Str(Val(Rec_ID)) & ", " & Str(Val(Sno)) & ", '" & Trim(.Rows(i).Cells(1).Value) & "', '" & Trim(.Rows(i).Cells(2).Value) & "',  " & Str(Val(SzPvu_EdsCnt_ID)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & " , " & Str(Val(.Rows(i).Cells(5).Value)) & ", " & Str(Val(.Rows(i).Cells(6).Value)) & ", " & Str(Val(.Rows(i).Cells(7).Value)) & ", " & Str(Val(.Rows(i).Cells(8).Value)) & ", " & Str(Val(Bw_IdNo)) & ", " & Str(Val(Ent_NoofUsed)) & ", '" & Trim(.Rows(i).Cells(12).Value) & "', '" & Trim(New_BmNo) & "' ) "
                        cmd.ExecuteNonQuery()

                        vPVUSTK_ENDSID = SzPvu_EdsCnt_ID
                        If Trim(LCase(Common_Procedures.settings.CustomerCode)) = "1087" Or Trim(LCase(Common_Procedures.settings.CustomerCode)) = "1155" Then
                            If KuPvu_EdsCnt_ID <> 0 Then
                                vPVUSTK_ENDSID = KuPvu_EdsCnt_ID
                            End If
                        End If

                        cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempTable) & "(Int1, Int2, Meters1) values (" & Str(Val(vPVUSTK_ENDSID)) & ", 1, " & Str(Val(.Rows(i).Cells(8).Value)) & ")"
                        cmd.ExecuteNonQuery()

                    End If

                Next
                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "NEW", "Weaver_Pavu_Receipt_Details", "Weaver_Pavu_Receipt_Code", Val(lbl_Company.Tag), NewCode, lbl_RecNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, " Set_No,Beam_No,EndsCount_IdNo,Pcs,Meters_Pc,Meters,Rcpt_Pcs,Rcpt_Meters,Beam_Width_Idno  ,Noof_Used,Set_Code,New_Receipt_BeamNo", "Sl_No", "Weaver_Pavu_Receipt_Code, For_OrderBy, Company_IdNo, Weaver_Pavu_Receipt_No, Weaver_Pavu_Receipt_Date, Ledger_Idno", tr)

            End With

            If Common_Procedures.settings.Textile_Sizing_Delivery_receipt_Selection = 1 And Trim(cbo_Type.Text) = "DELIVERY" And Trim(vSELC_DCCODE) <> "" Then


                cmd.CommandText = "Insert into Pavu_Delivery_Selections_Processing_Details ( Reference_Code                 , Company_IdNo                       , Reference_No                      , for_OrderBy                                                            , Reference_Date    ,    Delivery_Code                              ,     Delivery_No                  , DeliveryTo_Idno            , ReceivedFrom_Idno             ,     Party_Dc_No                         , Beam_Width_IdNo        , Total_Beams                          , Total_Pcs                                  , Total_Meters                    ,     Selection_Ledgeridno          ,Selection_CompanyIdno             ) " &
                        " Values                                              ('" & Trim(Pk_Condition) & Trim(NewCode) & "'          , " & Str(Val(lbl_Company.Tag)) & "  , '" & Trim(lbl_RecNo.Text) & "'      , " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RecNo.Text))) & ", @EntryDate        ,'" & Trim(vSELC_DCCODE) & "'   , '" & Trim(txt_Party_DcNo.Text) & "'    ," & Str(Val(Delv_ID)) & "    , " & Str(Val(Rec_ID)) & "  , '" & Trim(txt_Party_DcNo.Text) & "'   ,  " & Val(Bw_IdNo) & "     , " & Str(-1 * (Val(vTotPvuBms)) + Val(txt_KuraiPavuBeam.Text)) & "        , " & Str(-1 * (Val(vTotPvuRctPcs))) & "                , " & Str(-1 * Val(vTotPvuRctMtrs) + Val(txt_KuraiPavuMeters.Text)) & " ," & Str(Val(Rec_ID)) & "," & Str(Val(lbl_Company.Tag)) & "  )"
                cmd.ExecuteNonQuery()
            End If


            Da = New SqlClient.SqlDataAdapter("select Int1 as PavuEndsCount_IdNo, sum(Int2) as PavuBeam, sum(Meters1) as PavuMeters from " & Trim(Common_Procedures.EntryTempTable) & " group by Int1 having sum(Int2) <> 0 or sum(Meters1) <> 0", con)
            Da.SelectCommand.Transaction = tr
            Dt1 = New DataTable
            Da.Fill(Dt1)

            Sno = 0
            If Dt1.Rows.Count > 0 Then
                For i = 0 To Dt1.Rows.Count - 1


                    vTotPvuMtrs = 0
                    vTotPvuMtrs = Str(Val(Dt1.Rows(i).Item("PavuMeters").ToString))

                    Stock_In = ""
                    mtrspcs = 0

                    Da = New SqlClient.SqlDataAdapter("Select Stock_In , Meters_Pcs from  EndsCount_Head Where EndsCount_IdNo = " & Str(Val(Dt1.Rows(i).Item("PavuEndsCount_IdNo").ToString)), con)
                    Da.SelectCommand.Transaction = tr
                    dt2 = New DataTable
                    Da.Fill(dt2)
                    If dt2.Rows.Count > 0 Then
                        Stock_In = dt2.Rows(0)("Stock_In").ToString
                        mtrspcs = Val(dt2.Rows(0)("Meters_Pcs").ToString)
                    End If
                    dt2.Clear()

                    If Trim(UCase(Stock_In)) = "PCS" Then
                        If Val(mtrspcs) = 0 Then mtrspcs = 1
                        vTotPvuStk = vTotPvuMtrs / mtrspcs

                        Stk_DelvMtr = vTotPvuStk
                        Stk_RecMtr = vTotPvuStk

                    Else

                        If Common_Procedures.settings.AutoLoom_PavuWidthWiseConsumption_IN_Delivery = 1 Then
                            If Trim(UCase(cbo_WidthType.Text)) = "FOURTH" Then
                                vWdTyp = 2
                            ElseIf Trim(UCase(cbo_WidthType.Text)) = "TRIPLE" Then
                                vWdTyp = 1.5
                            ElseIf Trim(UCase(cbo_WidthType.Text)) = "DOUBLE" Then
                                vWdTyp = 1
                            Else
                                vWdTyp = 0.5
                            End If

                            vTotPvuStkAlLoomMtr = vTotPvuMtrs * vWdTyp

                            If Trim(UCase(Delv_Ledtype)) = "WEAVER" Then
                                Stk_DelvMtr = vTotPvuStkAlLoomMtr
                            Else
                                Stk_DelvMtr = vTotPvuMtrs
                            End If

                            If Trim(UCase(Rec_Ledtype)) = "WEAVER" Then
                                Stk_RecMtr = vTotPvuStkAlLoomMtr
                            Else
                                Stk_RecMtr = vTotPvuMtrs
                            End If
                        Else

                            vTotPvuStk = vTotPvuMtrs
                            Stk_DelvMtr = vTotPvuMtrs
                            Stk_RecMtr = vTotPvuMtrs

                        End If
                    End If

                    Sno = Sno + 1
                    cmd.CommandText = "Insert into Stock_Pavu_Processing_Details(Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno,DeliveryToIdno_ForParticulars,ReceivedFromIdno_ForParticulars, Entry_ID, Party_Bill_No, Particulars, Sl_No, EndsCount_IdNo, Sized_Beam, Meters,Weaving_JobCode_forSelection , ClothSales_OrderCode_forSelection) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RecNo.Text) & "', " & Str(Val(vForOrdByNo)) & ", @EntryDate, " & Str(Val(Delv_ID)) & ", 0," & Str(Val(Delv_ID)) & "," & Str(Val(Rec_ID)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', " & Str(Val(Sno)) & ", " & Str(Val(Dt1.Rows(i).Item("PavuEndsCount_IdNo").ToString)) & ", " & Str(Val(Dt1.Rows(i).Item("PavuBeam").ToString)) & ", " & Str(Val(Stk_DelvMtr)) & ", '" & Trim(Weaver_Job_Code) & "' , '" & Trim(cbo_ClothSales_OrderCode_forSelection.Text) & "' )"
                    cmd.ExecuteNonQuery()

                    Sno = Sno + 1
                    cmd.CommandText = "Insert into Stock_Pavu_Processing_Details(Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno,DeliveryToIdno_ForParticulars,ReceivedFromIdno_ForParticulars, Entry_ID, Party_Bill_No, Particulars, Sl_No, EndsCount_IdNo, Sized_Beam, Meters,Weaving_JobCode_forSelection , ClothSales_OrderCode_forSelection) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RecNo.Text) & "', " & Str(Val(vForOrdByNo)) & ", @EntryDate, 0, " & Str(Val(Rec_ID)) & "," & Str(Val(Delv_ID)) & "," & Str(Val(Rec_ID)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', " & Str(Val(Sno)) & ", " & Str(Val(Dt1.Rows(i).Item("PavuEndsCount_IdNo").ToString)) & ", " & Str(Val(Dt1.Rows(i).Item("PavuBeam").ToString)) & ", " & Str(Val(Stk_RecMtr)) & ", '" & Trim(Weaver_Job_Code) & "' , '" & Trim(cbo_ClothSales_OrderCode_forSelection.Text) & "')"
                    cmd.ExecuteNonQuery()

                Next
            End If
            Dt1.Clear()

            Dim Empty_Bms As Integer
            Empty_Bms = 0
            ' If Common_Procedures.settings.Weaver_Zari_Kuri_Entries_Status = 1 Then
            '  Empty_Bms = Val(txt_KuraiPavuBeam.Text)
            ' Else
            Empty_Bms = Val(txt_KuraiPavuBeam.Text) + Val(vTotPvuBms)
            '  End If

            If Val(txt_KuraiPavuBeam.Text) <> 0 Or Val(vTotPvuBms) <> 0 Then
                cmd.CommandText = "Insert into Stock_Empty_BeamBagCone_Processing_Details(Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Party_Bill_No, Entry_ID, Particulars, Sl_No, Beam_Width_IdNo, Empty_Beam, Pavu_Beam , Empty_Bobin) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RecNo.Text) & "', " & Str(Val(vForOrdByNo)) & ", @EntryDate, " & Str(Val(Delv_ID)) & ", " & Str(Val(Rec_ID)) & ", '" & Trim(PBlNo) & "', '" & Trim(EntID) & "', '" & Trim(Partcls) & "', 1, 0, 0, " & Str(Val(Empty_Bms)) & " , " & Str(Val(txt_NoOfBobin.Text)) & ")"
                cmd.ExecuteNonQuery()
            End If

            Dim vLed_IdNos As String = "", vVou_Amts As String = "", ErrMsg As String = ""

            vLed_IdNos = Trans_ID & "|" & Val(Common_Procedures.CommonLedger.Transport_Charges_Ac)
            vVou_Amts = Val(txt_Freight.Text) & "|" & -1 * Val(txt_Freight.Text)
            If Common_Procedures.Voucher_Updation(con, "WP.Rcpt", Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), Trim(lbl_RecNo.Text), Convert.ToDateTime(msk_date.Text), Partcls, vLed_IdNos, vVou_Amts, ErrMsg, tr, Common_Procedures.SoftwareTypes.Textile_Software) = False Then
                Throw New ApplicationException(ErrMsg)
                Exit Sub
            End If



            tr.Commit()

            Dt1.Dispose()
            Da.Dispose()

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)

            If Val(Common_Procedures.settings.OnSave_MoveTo_NewEntry_Status) = 1 Then
                If New_Entry = True Then
                    new_record()
                Else
                    move_record(lbl_RecNo.Text)
                End If
            Else
                move_record(lbl_RecNo.Text)
            End If

        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()

        End Try

    End Sub

    Private Sub cbo_RecForm_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_RecForm.GotFocus
        If Common_Procedures.settings.Textile_Sizing_Delivery_receipt_Selection = 1 Then
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'WEAVER' or Ledger_Type = 'GODOWN' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
        Else
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'WEAVER' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
        End If

    End Sub

    Private Sub cbo_RecForm_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_RecForm.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        If Common_Procedures.settings.Textile_Sizing_Delivery_receipt_Selection = 1 Then
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_RecForm, txt_Party_DcNo, cbo_EndsCount, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'WEAVER' or Ledger_Type = 'GODOWN' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
        Else
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_RecForm, txt_Party_DcNo, cbo_EndsCount, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'WEAVER' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
        End If

    End Sub

    Private Sub cbo_RecForm_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_RecForm.KeyPress
        If Common_Procedures.settings.Textile_Sizing_Delivery_receipt_Selection = 1 Then
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_RecForm, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'WEAVER' or Ledger_Type = 'GODOWN'  or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")

        Else
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_RecForm, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'WEAVER'  or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")

        End If

        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to select Pavu :", "FOR PAVU SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                If Common_Procedures.settings.Textile_Sizing_Delivery_receipt_Selection = 1 And Trim(cbo_Type.Text = "DELIVERY") Then
                    btn_Delivery_Selection_Click(sender, e)
                Else
                    btn_Selection_Click(sender, e)
                End If


            Else
                cbo_EndsCount.Focus()

            End If

        End If

    End Sub

    Private Sub cbo_RecForm_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_RecForm.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Common_Procedures.UR.Ledr_Wea_Siz_Rw_Trans_JbWrk_Creation = Common_Procedures.UR.WEAVER_Creation
            Common_Procedures.MDI_LedType = "WEAVER"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_RecForm.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub
    Private Sub cbo_EndsCount_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_EndsCount.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_idno = 0)")

    End Sub
    Private Sub cbo_EndsCount_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_EndsCount.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_EndsCount, cbo_RecForm, txt_KuraiPavuBeam, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_idno = 0)")
    End Sub

    Private Sub cbo_EndsCount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_EndsCount.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_EndsCount, txt_KuraiPavuBeam, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_idno = 0)")
    End Sub

    Private Sub cbo_EndsCount_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_EndsCount.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New EndsCount_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_EndsCount.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub
    Private Sub cbo_Transport_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Transport.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")

    End Sub
    Private Sub cbo_Transport_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Transport.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Transport, cbo_VehicleNo, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")

        If (e.KeyCode = 40 And cbo_Transport.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If cbo_weaving_job_no.Enabled And cbo_weaving_job_no.Visible = True Then
                cbo_weaving_job_no.Focus()
            Else
                txt_Freight.Focus()
            End If
        End If


    End Sub

    Private Sub cbo_Transport_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Transport.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Transport, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then
            If cbo_weaving_job_no.Enabled And cbo_weaving_job_no.Visible = True Then
                cbo_weaving_job_no.Focus()
            Else
                txt_Freight.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_Transport_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Transport.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Common_Procedures.UR.Ledr_Wea_Siz_Rw_Trans_JbWrk_Creation = Common_Procedures.UR.Ledger_Creation
            Common_Procedures.MDI_LedType = "TRANSPORT"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Transport.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub
    Private Sub cbo_VehicleNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_VehicleNo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Weaver_Pavu_Receipt_Head", "Vechile_No", "", "")

    End Sub
    Private Sub cbo_Vechile_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_VehicleNo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_VehicleNo, txt_KuraiPavuMeters, cbo_Transport, "Weaver_Pavu_Receipt_Head", "Vechile_No", "", "")


    End Sub

    Private Sub cbo_Vechile_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_VehicleNo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_VehicleNo, cbo_Transport, "Weaver_Pavu_Receipt_Head", "Vechile_No", "", "", False)
    End Sub

    Private Sub txt_KuraiPavuMeters_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_KuraiPavuMeters.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_KuraiPavuBeam_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_KuraiPavuBeam.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub btn_Filter_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter_Close.Click
        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        Filter_Status = False
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
    Private Sub btn_Filter_Show_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter_Show.Click
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

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Weaver_Pavu_Receipt_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Weaver_Pavu_Receipt_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Weaver_Pavu_Receipt_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If

            If Trim(cbo_Filter_EndsCountName.Text) <> "" Then
                Cnt_IdNo = Common_Procedures.EndsCount_NameToIdNo(con, cbo_Filter_EndsCountName.Text)
            End If

            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.ReceivedFrom_IdNo = " & Str(Val(Led_IdNo)) & " "
            End If

            If Val(Cnt_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.EndsCount_IdNo = " & Str(Val(Cnt_IdNo)) & " "
            End If

            If Trim(cbo_Verified_Sts.Text) = "YES" Then
                Verfied_Sts = 1
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Weaver_Pavu_Receipt_Code IN ( select z2.Weaver_Pavu_Receipt_Code from Weaver_Pavu_Receipt_Head z2 where z2.Verified_Status = " & Str(Val(Verfied_Sts)) & " )"
            ElseIf Trim(cbo_Verified_Sts.Text) = "NO" Then
                Verfied_Sts = 0
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Weaver_Pavu_Receipt_Code IN ( select z2.Weaver_Pavu_Receipt_Code from Weaver_Pavu_Receipt_Head z2 where z2.Verified_Status = " & Str(Val(Verfied_Sts)) & " )"
            End If


            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name , c.EndsCount_Name from Weaver_Pavu_Receipt_Head a INNER JOIN Weaver_Pavu_Receipt_Details d on a.Weaver_Pavu_Receipt_Code = d.Weaver_Pavu_Receipt_Code INNER JOIN Ledger_Head b on a.ReceivedFrom_IdNo = b.Ledger_IdNo LEFT OUTER JOIN EndsCount_Head c on d.EndsCount_IdNo = c.EndsCount_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_Pavu_Receipt_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Weaver_Pavu_Receipt_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Weaver_Pavu_Receipt_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Weaver_Pavu_Receipt_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = Val(dt2.Rows(i).Item("Total_Pcs").ToString)
                    dgv_Filter_Details.Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("Total_Meters").ToString), "########0.00")
                    dgv_Filter_Details.Rows(n).Cells(6).Value = dt2.Rows(i).Item("EndsCount_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(7).Value = Val(dt2.Rows(i).Item("Total_Rcpt_Pcs").ToString)
                    dgv_Filter_Details.Rows(n).Cells(8).Value = Format(Val(dt2.Rows(i).Item("Total_Rcpt_Meters").ToString), "########0.00")

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
    Private Sub dtp_Filter_Fromdate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_Filter_Fromdate.KeyPress
        If Asc(e.KeyChar) = 13 Then
            dtp_Filter_ToDate.Focus()
        End If
    End Sub

    Private Sub dtp_Filter_ToDate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_Filter_ToDate.KeyPress
        If Asc(e.KeyChar) = 13 Then
            cbo_Filter_PartyName.Focus()
        End If
    End Sub

    Private Sub cbo_Filter_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( Ledger_Type = 'WEAVER' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, dtp_Filter_ToDate, cbo_Filter_EndsCountName, "Ledger_AlaisHead", "Ledger_DisplayName", "( Ledger_Type = 'WEAVER' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, cbo_Filter_EndsCountName, "Ledger_AlaisHead", "Ledger_DisplayName", "( Ledger_Type = 'WEAVER' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_EndsCountName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_EndsCountName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_idno = 0)")
    End Sub

    Private Sub cbo_Filter_CountName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_EndsCountName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_EndsCountName, cbo_Filter_PartyName, Nothing, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")
        If e.KeyValue = 40 Or (e.Control = True And e.KeyValue = 40) Then
            If Common_Procedures.settings.CustomerCode = "1249" Then
                cbo_Verified_Sts.Focus()
            Else
                btn_Filter_Show.Focus()

            End If

        End If
    End Sub

    Private Sub cbo_Filter_CountName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_EndsCountName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_EndsCountName, Nothing, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            If Common_Procedures.settings.CustomerCode = "1249" Then
                cbo_Verified_Sts.Focus()
            Else
                btn_Filter_Show.Focus()

            End If
        End If
    End Sub




    Private Sub cbo_Verified_Sts_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Verified_Sts.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")

    End Sub
    Private Sub cbo_Verified_Sts_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Verified_Sts.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Verified_Sts, cbo_Filter_EndsCountName, btn_Filter_Show, "", "", "", "")
    End Sub

    Private Sub cbo_Verified_Sts_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Verified_Sts.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Verified_Sts, btn_Filter_Show, "", "", "", "")
    End Sub

    Private Sub dgv_PavuDetails_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_PavuDetails.CellEnter


        With dgv_PavuDetails
            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If
        End With
    End Sub

    Private Sub dgv_PavuDetails_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_PavuDetails.CellLeave
        With dgv_PavuDetails
            If .CurrentCell.ColumnIndex = 5 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                End If
            End If
        End With
    End Sub

    Private Sub dgv_PavuDetails_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_PavuDetails.CellValueChanged
        On Error Resume Next
        If IsNothing(dgv_PavuDetails.CurrentCell) Then Exit Sub

        With dgv_PavuDetails
            If .Visible Then
                If .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 6 Or .CurrentCell.ColumnIndex = 7 Or .CurrentCell.ColumnIndex = 8 Then
                    TotalPavu_Calculation()
                End If
            End If
        End With
    End Sub

    Private Sub dgv_PavuDetails_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_PavuDetails.EditingControlShowing
        dgtxt_Details = CType(dgv_PavuDetails.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub
    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.Enter
        dgv_PavuDetails.EditingControl.BackColor = Color.Lime
        dgv_PavuDetails.EditingControl.ForeColor = Color.Blue
        dgtxt_Details.SelectAll()
    End Sub
    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Details.KeyPress
        On Error Resume Next
        If Val(dgv_PavuDetails.Rows(dgv_PavuDetails.CurrentCell.RowIndex).Cells(10).Value) = 1 Then
            e.Handled = True
        End If
        If dgv_PavuDetails.CurrentCell.ColumnIndex = 7 Or dgv_PavuDetails.CurrentCell.ColumnIndex = 8 Then
            If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                e.Handled = True
            End If
        End If
    End Sub

    Private Sub dgv_PavuDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_PavuDetails.KeyUp
        Dim n As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_PavuDetails

                n = .CurrentRow.Index

                If Val(.Rows(n).Cells(11).Value) > 0 And Val(.Rows(n).Cells(11).Value) <> Val(.Rows(n).Cells(13).Value) Then
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
                    .Rows(i).Cells(0).Value = i + 1
                Next

            End With

            TotalPavu_Calculation()

        End If

    End Sub

    Private Sub dgv_PavuDetails_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_PavuDetails.RowsAdded
        Dim n As Integer
        If IsNothing(dgv_PavuDetails.CurrentCell) Then Exit Sub

        With dgv_PavuDetails
            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)
        End With
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

        With dgv_PavuDetails
            For i = 0 To .RowCount - 1
                Sno = Sno + 1
                .Rows(i).Cells(0).Value = Sno
                If Val(.Rows(i).Cells(6).Value) <> 0 Then
                    TotBms = TotBms + 1

                    TotPcs = TotPcs + Val(.Rows(i).Cells(4).Value)
                    TotMtrs = TotMtrs + Val(.Rows(i).Cells(6).Value)

                    TotRctPcs = TotRctPcs + Val(.Rows(i).Cells(7).Value)
                    TotRctMtrs = TotRctMtrs + Val(.Rows(i).Cells(8).Value)
                End If
            Next
        End With

        With dgv_PavuDetails_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(2).Value = Val(TotBms)

            .Rows(0).Cells(4).Value = Val(TotPcs)
            .Rows(0).Cells(6).Value = Format(Val(TotMtrs), "########0.00")

            .Rows(0).Cells(7).Value = Val(TotRctPcs)
            .Rows(0).Cells(8).Value = Format(Val(TotRctMtrs), "########0.00")

        End With

        Amount_Calculation()

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

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
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


        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

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






            da1 = New SqlClient.SqlDataAdapter("select a.*, b.* , d.EndsCount_Name , e.Ledger_Name as Trasport_Name,IG.Item_GST_Percentage , f.* from Weaver_Pavu_Receipt_Head a LEFT OUTER JOIN Ledger_Head b ON a.ReceivedFrom_IdNo = b.Ledger_IdNo LEFT OUTER JOIN EndsCount_Head d ON a.EndsCount_IdNo = d.EndsCount_IdNo LEFT OUTER JOIN Ledger_Head e ON a.Transport_IdNo = e.Ledger_IdNo  INNER JOIN Company_Head f ON a.Company_IdNo = f.Company_IdNo  LEFT OUTER JOIN Count_Head Ch On Ch.Count_Idno = d.Count_Idno " &
                                                     " LEFT OUTER JOIN ItemGroup_Head IG on Ch.ItemGroup_IdNo = IG.ItemGroup_IdNo where a.Weaver_Pavu_Receipt_Code = '" & Trim(NewCode) & "'", con)
            ' da1 = New SqlClient.SqlDataAdapter("select a.*, b.* , d.EndsCount_Name , e.Ledger_Name as Trasport_Name ,f.* from Weaver_Pavu_Receipt_Head a LEFT OUTER JOIN Ledger_Head b ON a.ReceivedFrom_IdNo = b.Ledger_IdNo LEFT OUTER JOIN EndsCount_Head d ON a.EndsCount_IdNo = d.EndsCount_IdNo LEFT OUTER JOIN Ledger_Head e ON a.Transport_IdNo = e.Ledger_IdNo  INNER JOIN Company_Head f ON a.Company_IdNo = f.Company_IdNo   where a.Weaver_Pavu_Receipt_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("select a.* , d.EndsCount_Name , IG.Item_GST_Percentage as GST_Percentage   from Weaver_Pavu_Receipt_Details a LEFT OUTER JOIN EndsCount_Head d ON a.EndsCount_idno = d.EndsCount_idno  INNER JOIN Count_Head Ch On Ch.Count_Idno = d.Count_Idno " &
                                                     " Inner Join ItemGroup_Head IG on Ch.ItemGroup_IdNo = IG.ItemGroup_IdNo  where Weaver_Pavu_Receipt_Code = '" & Trim(NewCode) & "' Order by a.Sl_No", con)
                ' da2 = New SqlClient.SqlDataAdapter("select a.* , d.EndsCount_Name from Weaver_Pavu_Receipt_Details a LEFT OUTER JOIN EndsCount_Head d ON a.EndsCount_idno = d.EndsCount_idno where Weaver_Pavu_Receipt_Code = '" & Trim(NewCode) & "' Order by a.Sl_No", con)
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
                            prn_DetAr(prn_DetMxIndx, 6) = prn_DetDt.Rows(i).Item("GST_Percentage").ToString
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

        TxtHgt = 17.5 '18.5 ' e.Graphics.MeasureString("A", pFont).Height  ' 20



        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

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
        Dim C1 As Single, C2 As Single, C3 As Single
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
        C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 20

        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 11, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, "TO :  M/s. " & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "REC.NO", LMargin + M1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + 10, CurY, 0, 0, pFont)
        p1Font = New Font("Calibri", 14, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Weaver_Pavu_Receipt_No").ToString, LMargin + C1 + 25, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + M1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Weaver_Pavu_Receipt_Date").ToString), "dd-MM-yyyy"), LMargin + C1 + 25, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)

        If Trim(prn_HdDt.Rows(0).Item("EwayBill_No").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "E-WAY BILL NO", LMargin + M1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, (prn_HdDt.Rows(0).Item("EwayBill_No").ToString), LMargin + C1 + 25, CurY, 0, 0, pFont)
        End If

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)

        If Trim(prn_HdDt.Rows(0).Item("Vechile_No").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "VEHICLE NO", LMargin + M1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, (prn_HdDt.Rows(0).Item("Vechile_No").ToString), LMargin + C1 + 25, CurY, 0, 0, pFont)
        End If


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

            CurY = CurY + TxtHgt - 15

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

            CurY = CurY + TxtHgt + 5
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
            If Trim(prn_HdDt.Rows(0).Item("transport_Idno").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "Transport :  " & Common_Procedures.Ledger_IdNoToName(con, prn_HdDt.Rows(0).Item("transport_Idno").ToString), LMargin + 10, CurY, 0, 0, pFont)
            End If


            ' --------- GST CALCULATION PART ------------ '  START 

            Dim vTxPerc As String = 0
            Dim vTxamt As String = 0
            Dim vNtAMt As String = 0
            Dim vSgst_amt As String = 0
            Dim vCgst_amt As String = 0
            Dim vIgst_amt As String = 0
            Dim VGST_STS As Integer = 0
            Dim C1 As Single
            Dim W1 As Single
            ' Dim C2 As Single

            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 20
            W1 = e.Graphics.MeasureString(" Vehicle No :", pFont).Width

            ' C2 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 10


            vTxPerc = 0
            vCgst_amt = 0
            vSgst_amt = 0
            vIgst_amt = 0

            VGST_STS = Val(prn_HdDt.Rows(0).Item("GST_Tax_Invoice_Status").ToString)

            If Val(VGST_STS) = 1 Then

                If Val(prn_HdDt.Rows(0).Item("Company_State_IdNo").ToString) = Val(prn_HdDt.Rows(0).Item("Ledger_State_IdNo").ToString) Then

                    If prn_DetAr(prn_DetMxIndx, 5) <> "" Then
                        'Rcpt_Meters
                        vTxPerc = Format(Val(prn_DetAr(prn_DetIndx, 6)) / 2, "############0.00")
                    Else
                        vTxPerc = Format(Val(prn_HdDt.Rows(0).Item("Item_GST_Percentage").ToString) / 2, "############0.00")
                    End If


                    vCgst_amt = Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString) * vTxPerc / 100, "############0.00")
                    vSgst_amt = Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString) * vTxPerc / 100, "############0.00")

                Else
                    If prn_DetAr(prn_DetMxIndx, 5) <> "" Then
                        'Rcpt_Meters
                        vTxPerc = Format(Val(prn_DetAr(prn_DetIndx, 6)), "############0.00")
                    Else
                        vTxPerc = Format(Val(prn_HdDt.Rows(0).Item("Item_GST_Percentage").ToString), "############0.00")
                    End If


                    vIgst_amt = Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString) * Val(vTxPerc) / 100, "############0.00")

                End If
            End If

            ' --------- GST CALCULATION PART ------------ '  END 


            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            If Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString) <> 0 And Val(VGST_STS) = 1 Then

                Common_Procedures.Print_To_PrintDocument(e, "Taxable Value", LMargin + C1 - 20, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " : ", LMargin + C1 + W1, CurY, 2, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Net_Amount").ToString, LMargin + C1 + W1 + 20, CurY, 2, 0, pFont)

            Else
                Common_Procedures.Print_To_PrintDocument(e, "Value of Goods", LMargin + C1 - 20, CurY, 0, 0, p1Font)
                Common_Procedures.Print_To_PrintDocument(e, " : ", LMargin + C1 + W1, CurY, 2, 0, p1Font)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Net_Amount").ToString, LMargin + C1 + W1 + 20, CurY, 2, 0, p1Font)

            End If
            CurY = CurY + TxtHgt
            If Val(vCgst_amt) <> 0 Then

                Common_Procedures.Print_To_PrintDocument(e, "CGST " & vTxPerc & "  %  ", LMargin + C1 - 20, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " : ", LMargin + C1 + W1, CurY, 2, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, vCgst_amt, LMargin + C1 + W1 + 20, CurY, 2, 0, pFont)

            ElseIf Val(vIgst_amt) <> 0 Then

                Common_Procedures.Print_To_PrintDocument(e, "IGST " & vTxPerc & "  %  ", LMargin + C1 - 20, CurY + 10, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " : ", LMargin + C1 + W1, CurY + 10, 2, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, vIgst_amt, LMargin + C1 + W1 + 20, CurY + 10, 2, 0, pFont)

            End If
            CurY = CurY + TxtHgt
            If Val(vSgst_amt) <> 0 Then

                Common_Procedures.Print_To_PrintDocument(e, "SGST " & vTxPerc & "  %  ", LMargin + C1 - 20, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " : ", LMargin + C1 + W1, CurY, 2, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, vSgst_amt, LMargin + C1 + W1 + 20, CurY, 2, 0, pFont)

            End If
            CurY = CurY + TxtHgt

            If Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString) <> 0 And Val(VGST_STS) = 1 Then
                vTxamt = Val(vCgst_amt) + Val(vSgst_amt) + Val(vIgst_amt)
                vNtAMt = Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString) + vTxamt, "###########0.00")

                Common_Procedures.Print_To_PrintDocument(e, "Value of Goods", LMargin + C1 - 20, CurY, 0, 0, p1Font)
                Common_Procedures.Print_To_PrintDocument(e, " : ", LMargin + C1 + W1, CurY, 2, 0, p1Font)
                Common_Procedures.Print_To_PrintDocument(e, vNtAMt, LMargin + C1 + W1 + 20, CurY, 2, 0, p1Font)

            End If


            CurY = CurY + TxtHgt + 5
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


    Private Sub txt_Freight_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Freight.KeyDown
        If e.KeyValue = 38 Then

            If cbo_weaving_job_no.Enabled And cbo_weaving_job_no.Visible = True Then
                cbo_weaving_job_no.Focus()
            Else
                cbo_Transport.Focus()
            End If
        End If
        If e.KeyValue = 40 Then
            If cbo_WidthType.Visible Then
                cbo_WidthType.Focus()

            ElseIf cbo_ClothSales_OrderCode_forSelection.Visible = True Then
                cbo_ClothSales_OrderCode_forSelection.Focus()

            Else
                If dgv_PavuDetails.Rows.Count > 0 Then
                    dgv_PavuDetails.Focus()
                    dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(0).Cells(7)
                    dgv_PavuDetails.CurrentCell.Selected = True

                Else
                    txt_Note.Focus()

                End If
            End If

        End If
    End Sub

    Private Sub txt_Freight_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Freight.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
        If Asc(e.KeyChar) = 13 Then
            If cbo_WidthType.Visible Then
                cbo_WidthType.Focus()

            ElseIf cbo_ClothSales_OrderCode_forSelection.Visible = True Then
                cbo_ClothSales_OrderCode_forSelection.Focus()

            Else
                If dgv_PavuDetails.Rows.Count > 0 Then
                    dgv_PavuDetails.Focus()
                    dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(0).Cells(7)
                    dgv_PavuDetails.CurrentCell.Selected = True

                Else
                    txt_Note.Focus()

                End If
            End If

        End If
    End Sub

    Private Sub txt_Note_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Note.KeyDown
        If e.KeyValue = 38 Then
            If dgv_PavuDetails.Rows.Count > 0 Then
                dgv_PavuDetails.Focus()
                dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(0).Cells(7)
                dgv_PavuDetails.CurrentCell.Selected = True

            Else
                txt_Freight.Focus()

            End If

        End If

        If e.KeyValue = 40 Then
            'If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
            '    save_record()
            'Else
            '    msk_date.Focus()
            'End If
            txt_rate.Focus()
        End If

    End Sub

    Private Sub txt_Note_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Note.KeyPress
        If Asc(e.KeyChar) = 13 Then
            'If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
            '    save_record()
            'Else
            '    msk_date.Focus()
            'End If

            txt_rate.Focus()

        End If
    End Sub

    Private Sub btn_Selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Selection.Click
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim i As Integer, j As Integer, n As Integer, SNo As Integer
        Dim Led_IdNo As Integer
        Dim NewCode As String = ""
        Dim CompIDCondt As String = ""
        Dim RcptBm_PavuInc As Integer

        Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_RecForm.Text)
        If Led_IdNo = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SELECT PAVU...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_RecForm.Enabled And cbo_RecForm.Visible Then cbo_RecForm.Focus()
            Exit Sub
        End If

        CompIDCondt = "(a.company_idno = " & Str(Val(lbl_Company.Tag)) & ")"
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1155" Then
        If Common_Procedures.settings.EntrySelection_Combine_AllCompany = 1 Then
            CompIDCondt = ""
            If Trim(UCase(Common_Procedures.User.Type)) = "ACCOUNT" Then
                CompIDCondt = "(Company_Type <> 'UNACCOUNT')"
            End If
        End If
        End If



        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        With dgv_Selection
            chk_SelectAll.Checked = False
            .Rows.Clear()
            SNo = 0

            Da = New SqlClient.SqlDataAdapter("select a.noof_used as Ent_NoofUsed, a.Rcpt_Pcs, a.Rcpt_Meters, a.New_Receipt_BeamNo, b.*, c.Pavu_Delivery_Increment as New_Receipt_Beam_Pavu_Delv_Inc, d.EndsCount_Name, e.Beam_Width_Name from Weaver_Pavu_Receipt_Details a INNER JOIN Company_Head tZ ON a.company_idno = tZ.company_idno INNER JOIN Stock_SizedPavu_Processing_Details b ON a.Set_Code = b.Set_Code and a.Beam_No = b.Beam_No LEFT OUTER JOIN Stock_SizedPavu_Processing_Details c ON c.Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and a.Set_Code = c.Set_Code and a.New_Receipt_BeamNo = c.Beam_No INNER JOIN EndsCount_Head d ON a.EndsCount_IdNo = d.EndsCount_IdNo LEFT OUTER JOIN Beam_Width_Head e ON b.Beam_Width_Idno = e.Beam_Width_Idno where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.Weaver_Pavu_Receipt_Code = '" & Trim(NewCode) & "' order by a.for_orderby, a.Set_Code, b.ForOrderBy_BeamNo, a.Beam_No, a.sl_no", con)
            'Da = New SqlClient.SqlDataAdapter("select a.noof_used as Ent_NoofUsed, a.Rcpt_Pcs, a.Rcpt_Meters, b.*, c.EndsCount_Name, d.Beam_Width_Name from Weaver_Pavu_Receipt_Details a INNER JOIN Stock_SizedPavu_Processing_Details b ON a.Set_Code = b.Set_Code and a.Beam_No = b.Beam_No INNER JOIN EndsCount_Head c ON a.EndsCount_IdNo = c.EndsCount_IdNo LEFT OUTER JOIN Beam_Width_Head d ON b.Beam_Width_Idno = d.Beam_Width_Idno where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.Weaver_Pavu_Receipt_Code = '" & Trim(NewCode) & "' order by a.for_orderby, a.Set_Code, b.ForOrderBy_BeamNo, a.Beam_No, a.sl_no", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    RcptBm_PavuInc = Dt1.Rows(i).Item("Pavu_Delivery_Increment").ToString
                    If Val(Dt1.Rows(i).Item("Meters").ToString) <> Val(Dt1.Rows(i).Item("Rcpt_Meters").ToString) Then
                        If Trim(Dt1.Rows(i).Item("New_Receipt_BeamNo").ToString) <> "" Then
                            If IsDBNull(Dt1.Rows(i).Item("New_Receipt_Beam_Pavu_Delv_Inc").ToString) = False Then
                                RcptBm_PavuInc = Val(Dt1.Rows(i).Item("New_Receipt_Beam_Pavu_Delv_Inc").ToString)
                            End If
                        End If
                    End If

                    n = .Rows.Add()

                    SNo = SNo + 1
                    .Rows(n).Cells(0).Value = Val(SNo)
                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Set_No").ToString
                    .Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Beam_No").ToString
                    .Rows(n).Cells(3).Value = Val(Dt1.Rows(i).Item("Noof_Pcs").ToString)
                    .Rows(n).Cells(4).Value = Val(Dt1.Rows(i).Item("Meters_Pc").ToString)
                    .Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(i).Item("Meters").ToString), "#########0.00")
                    .Rows(n).Cells(6).Value = Dt1.Rows(i).Item("EndsCount_Name").ToString
                    .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Beam_Width_Name").ToString
                    .Rows(n).Cells(8).Value = "1"
                    .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("Set_Code").ToString
                    .Rows(n).Cells(10).Value = RcptBm_PavuInc
                    .Rows(n).Cells(11).Value = Dt1.Rows(i).Item("Ent_NoofUsed").ToString
                    .Rows(n).Cells(12).Value = Dt1.Rows(i).Item("Rcpt_Pcs").ToString
                    .Rows(n).Cells(13).Value = Dt1.Rows(i).Item("Rcpt_Meters").ToString

                    For j = 0 To .ColumnCount - 1
                        .Rows(i).Cells(j).Style.ForeColor = Color.Red
                        If Val(.Rows(n).Cells(10).Value) <> Val(.Rows(n).Cells(11).Value) Then
                            .Rows(i).Cells(j).Style.BackColor = Color.LightGray
                        End If
                    Next

                Next

            End If
            Dt1.Clear()

            Da = New SqlClient.SqlDataAdapter("select a.*, b.EndsCount_Name, c.Beam_Width_Name from Stock_SizedPavu_Processing_Details a INNER JOIN Company_Head tZ ON a.company_idno = tZ.company_idno INNER JOIN EndsCount_Head b ON a.EndsCount_IdNo = b.EndsCount_IdNo LEFT OUTER JOIN Beam_Width_Head c ON a.Beam_Width_Idno = c.Beam_Width_Idno Where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.StockAt_IdNo = " & Str(Val(Led_IdNo)) & " and  (a.Pavu_Delivery_Code = '' and a.Beam_Knotting_Code = '' and a.Close_Status = 0) order by a.for_orderby, a.Set_Code, a.ForOrderBy_BeamNo, a.Beam_No, a.sl_no", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()

                    SNo = SNo + 1
                    .Rows(n).Cells(0).Value = Val(SNo)
                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Set_No").ToString
                    .Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Beam_No").ToString
                    .Rows(n).Cells(3).Value = Val(Dt1.Rows(i).Item("Noof_Pcs").ToString)
                    .Rows(n).Cells(4).Value = Val(Dt1.Rows(i).Item("Meters_Pc").ToString)
                    .Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(i).Item("Meters").ToString), "#########0.00")
                    .Rows(n).Cells(6).Value = Dt1.Rows(i).Item("EndsCount_Name").ToString
                    .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Beam_Width_Name").ToString
                    .Rows(n).Cells(8).Value = ""
                    .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("Set_Code").ToString
                    .Rows(n).Cells(10).Value = Dt1.Rows(i).Item("Pavu_Delivery_Increment").ToString
                    .Rows(n).Cells(11).Value = -9999
                    .Rows(n).Cells(12).Value = 0
                    .Rows(n).Cells(13).Value = 0

                Next

            End If
            Dt1.Clear()

        End With

        pnl_Selection.Visible = True
        pnl_Back.Enabled = False
        If dgv_Selection.Rows.Count > 0 Then
            dgv_Selection.Focus()
            dgv_Selection.CurrentCell = dgv_Selection.Rows(0).Cells(0)
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

        With dgv_PavuDetails

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
                        EntMtrs = Val(dgv_Selection.Rows(i).Cells(5).Value)
                    End If


                    n = .Rows.Add()

                    sno = sno + 1
                    .Rows(n).Cells(0).Value = Val(sno)
                    .Rows(n).Cells(1).Value = dgv_Selection.Rows(i).Cells(1).Value
                    .Rows(n).Cells(2).Value = dgv_Selection.Rows(i).Cells(2).Value
                    .Rows(n).Cells(3).Value = dgv_Selection.Rows(i).Cells(6).Value
                    .Rows(n).Cells(4).Value = Val(dgv_Selection.Rows(i).Cells(3).Value)
                    .Rows(n).Cells(5).Value = Val(dgv_Selection.Rows(i).Cells(4).Value)
                    .Rows(n).Cells(6).Value = Format(Val(dgv_Selection.Rows(i).Cells(5).Value), "#########0.00")

                    .Rows(n).Cells(7).Value = Val(EntPcs)
                    .Rows(n).Cells(8).Value = Format(Val(EntMtrs), "#########0.00")

                    .Rows(n).Cells(9).Value = dgv_Selection.Rows(i).Cells(7).Value

                    .Rows(n).Cells(10).Value = ""
                    .Rows(n).Cells(11).Value = ""

                    If Val(dgv_Selection.Rows(i).Cells(11).Value) > 0 Then

                        If Val(dgv_Selection.Rows(i).Cells(10).Value) <> Val(dgv_Selection.Rows(i).Cells(11).Value) Then
                            .Rows(n).Cells(10).Value = "1"
                            For j = 0 To .ColumnCount - 1
                                .Rows(n).Cells(j).Style.ForeColor = Color.Red
                                .Rows(n).Cells(j).Style.BackColor = Color.LightGray
                            Next
                        Else
                            .Rows(n).Cells(10).Value = ""
                        End If

                        .Rows(n).Cells(11).Value = dgv_Selection.Rows(i).Cells(11).Value

                    End If

                    .Rows(n).Cells(12).Value = dgv_Selection.Rows(i).Cells(9).Value
                    .Rows(n).Cells(13).Value = dgv_Selection.Rows(i).Cells(10).Value

                End If

            Next

        End With

        TotalPavu_Calculation()

        Grid_Cell_DeSelect()

        pnl_Back.Enabled = True
        pnl_Selection.Visible = False
        If cbo_EndsCount.Enabled And cbo_EndsCount.Visible Then cbo_EndsCount.Focus()

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
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            If cbo_Type.Visible = True Then
                cbo_Type.Focus()
            Else
                txt_Party_DcNo.Focus()
            End If
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

        If IsDate(msk_Date.Text) = True Then
            If Microsoft.VisualBasic.DateAndTime.Day(Convert.ToDateTime(msk_Date.Text)) <= 31 Or Microsoft.VisualBasic.DateAndTime.Month(Convert.ToDateTime(msk_Date.Text)) <= 31 Then
                If Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_Date.Text)) <= 2050 And Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_Date.Text)) >= 2000 Then
                    dtp_Date.Value = Convert.ToDateTime(msk_Date.Text)
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
            txt_Party_DcNo.Focus()
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
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_WidthType, Nothing, Nothing, "", "", "", "")
        If e.KeyValue = 38 Then
            txt_Freight.Focus()
        End If
        If e.KeyValue = 40 Then

            If dgv_PavuDetails.Rows.Count > 0 Then
                dgv_PavuDetails.Focus()
                dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(0).Cells(7)
                dgv_PavuDetails.CurrentCell.Selected = True

            Else
                txt_Note.Focus()
            End If

        End If
    End Sub

    Private Sub cbo_WidthType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_WidthType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_WidthType, Nothing, "", "", "", "")
        If Asc(e.KeyChar) = 13 Then

            If dgv_PavuDetails.Rows.Count > 0 Then
                dgv_PavuDetails.Focus()
                dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(0).Cells(7)
                dgv_PavuDetails.CurrentCell.Selected = True

            Else
                txt_Note.Focus()

            End If

        End If
    End Sub
    Private Sub btn_SMS_Click(sender As System.Object, e As System.EventArgs) Handles btn_SMS.Click
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

            Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_RecForm.Text)

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
            smstxt = smstxt & " Rec No : " & Trim(lbl_RecNo.Text) & vbCrLf
            smstxt = smstxt & " Date : " & Trim(msk_date.Text) & vbCrLf

            'If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1044" Then
            '    If Trim(cbo_Transport.Text) <> "" Then
            '        smstxt = smstxt & " Transport : " & Trim(cbo_Transport.Text) & vbCrLf
            '    End If

            'End If
            If dgv_PavuDetails_Total.RowCount > 0 Then
                smstxt = smstxt & " Total Meters: " & Val((dgv_PavuDetails_Total.Rows(0).Cells(6).Value())) & vbCrLf

                smstxt = smstxt & " Beam  : " & Val(dgv_PavuDetails_Total.Rows(0).Cells(2).Value()) & vbCrLf
            End If

            If dgv_PavuDetails.RowCount > 0 Then
                ' smstxt = smstxt & " Beam No: " & Trim((dgv_PavuDetails.Rows(0).Cells(3).Value())) & vbCrLf
                smstxt = smstxt & "Ends Count : " & Trim((dgv_PavuDetails.Rows(0).Cells(3).Value())) & vbCrLf

            End If
            'smstxt = smstxt & " Ends Count : " & Trim(EndsCount) & vbCrLf
            'smstxt = smstxt & " Tax Amount : " & Val(lbl_CGST_Amount.Text) + Val(lbl_SGST_Amount.Text) + Val(lbl_IGST_Amount.Text) & vbCrLf
            'smstxt = smstxt & " Net Amount : " & Trim(lbl_Net_Amt.Text) & vbCrLf

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

    Private Sub btn_UserModification_Click(sender As Object, e As System.EventArgs) Handles btn_UserModification.Click
        If Val(Common_Procedures.User.IdNo) = 1 Then
            Dim f1 As New User_Modifications
            f1.Entry_Name = Me.Name
            f1.Entry_PkValue = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            f1.ShowDialog()
        End If
    End Sub


    Private Sub Delivery_Selection()
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Da2 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim i As Integer, j As Integer, n As Integer, SNo As Integer, Ledger_Party_idno As Integer
        Dim Led_IdNo As Integer
        Dim NewCode As String = ""
        Dim CompIDCondt As String = ""
        Dim RcptBm_PavuInc As Integer
        Dim vjoinTYP As String


        Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_RecForm.Text)
        If Led_IdNo = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SELECT PAVU...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_RecForm.Enabled And cbo_RecForm.Visible Then cbo_RecForm.Focus()
            Exit Sub
        End If

        CompIDCondt = "(a.Selection_CompanyIdno = " & Str(Val(lbl_Company.Tag)) & ")"

        'If Common_Procedures.settings.Textile_Sizing_Delivery_receipt_Selection = 1 Then

        '    Da = New SqlClient.SqlDataAdapter("Select Company_idno,* from Company_Head where Sizing_to_ledgeridno= " & Str(Val(Led_IdNo)) & " ", con)
        '    Dt1 = New DataTable
        '    Da.Fill(Dt1)
        '    If Dt1.Rows.Count > 0 Then

        '        CompIDCondt = "(a.Ledger_Company_idno = " & Str(Val(lbl_Company.Tag)) & ")"
        '    Else
        '        CompIDCondt = "(a.Company_idno = " & Str(Val(lbl_Company.Tag)) & ")"

        '    End If
        'Else
        '    CompIDCondt = "(a.Company_idno = " & Str(Val(lbl_Company.Tag)) & ")"
        'End If


        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)


        If Trim(UCase(cbo_Type.Text)) = "DELIVERY" Then


            With dgv_delivery_Selections

                .Rows.Clear()
                n = .Rows.Add()
                SNo = 0

                For i = 1 To 2


                    If i = 1 Then
                        '---editing
                        Da2 = New SqlClient.SqlDataAdapter("Select a.Delivery_Code, a.delivery_No,  a.Total_Beams as Beams , a.Total_pcs as Pcs, a.Total_meters as meters from Pavu_Delivery_Selections_Processing_Details a where   a.Selection_ledgerIdno =" & Str(Val(Led_IdNo)) & " and " & CompIDCondt & " and a.Delivery_Code = a.reference_code and a.Total_meters > 0 and a.Delivery_Code IN (Select sq1.Delivery_Code from Pavu_Delivery_Selections_Processing_Details sq1 where sq1.reference_code = '" & Trim(NewCode) & "' )  ", con)
                        'Da2 = New SqlClient.SqlDataAdapter("Select a.Delivery_Code, a.delivery_No,  a.Total_Beams as Beams , a.Total_pcs as Pcs, a.Total_meters as meters from Pavu_Delivery_Selections_Processing_Details a where   a.Ledger_idno =" & Str(Val(Led_IdNo)) & " and a.Delivery_Code IN (Select a.Delivery_Code from Pavu_Delivery_Selections_Processing_Details sq1 where sq1.reference_code = '" & Trim(NewCode) & "' ) ", con)
                    Else
                        'new entry
                        Da2 = New SqlClient.SqlDataAdapter("Select a.Delivery_Code, a.delivery_No,  SUM(a.Total_Beams) as Beams , SUM(a.Total_pcs) as Pcs, SUM(a.Total_meters) as meters from Pavu_Delivery_Selections_Processing_Details a where   a.Selection_ledgerIdno =" & Str(Val(Led_IdNo)) & " and " & CompIDCondt & " and a.Delivery_Code NOT IN (Select sq1.Delivery_Code from Pavu_Delivery_Selections_Processing_Details sq1 where sq1.reference_code = '" & Trim(NewCode) & "' ) Group by a.Delivery_Code, a.Delivery_No Having Sum(a.Total_meters) > 0  ", con)
                    End If


                    Dt2 = New DataTable


                    Da2.Fill(Dt2)

                    If Dt2.Rows.Count > 0 Then

                        For k = 0 To Dt2.Rows.Count - 1

                            If Val(Dt2.Rows(k).Item("meters").ToString) > 0 Then

                                SNo = SNo + 1
                                n = .Rows.Add()

                                .Rows(n).Cells(0).Value = Val(SNo)
                                .Rows(n).Cells(1).Value = Dt2.Rows(k).Item("Delivery_No").ToString
                                '.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Empty_BeamBagCone_Delivery_Date").ToString), "dd-MM-yyyy")
                                .Rows(n).Cells(3).Value = Dt2.Rows(k).Item("Delivery_No").ToString
                                .Rows(n).Cells(4).Value = Dt2.Rows(k).Item("Beams").ToString
                                .Rows(n).Cells(5).Value = Dt2.Rows(k).Item("Pcs").ToString
                                .Rows(n).Cells(6).Value = Dt2.Rows(k).Item("Meters").ToString
                                .Rows(n).Cells(8).Value = Trim(Dt2.Rows(k).Item("Delivery_Code").ToString)
                                If i = 1 Then

                                    .Rows(n).Cells(7).Value = 1
                                    'For j = 0 To .ColumnCount - 1
                                    '    .Rows(k).Cells(j).Style.ForeColor = Color.Red
                                    'Next

                                Else
                                    .Rows(n).Cells(7).Value = ""
                                    'For j = 0 To .ColumnCount - 1
                                    '    .Rows(k).Cells(j).Style.ForeColor = Color.Black
                                    'Next

                                End If


                            End If
                        Next


                    End If
                    Dt2.Clear()


                Next









            End With


        End If
        pnl_Delivery_Selection.Visible = True
        pnl_Back.Enabled = False
        dgv_delivery_Selections.Focus()

    End Sub


    Private Sub Close_Delivery_Selection()
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim i As Integer, n As Integer
        Dim sno As Integer = 0
        Dim Clo_IdNo As Integer = 0


        sno = 0
        Clo_IdNo = 0

        lbl_Delivery_Code.Text = ""
        txt_Party_DcNo.Text = ""
        dgv_PavuDetails.Rows.Clear()

        For i = 0 To dgv_delivery_Selections.RowCount - 1


            If Val(dgv_delivery_Selections.Rows.Count > 0) Then
                lbl_Delivery_Code.Text = Trim(dgv_delivery_Selections.Rows(i).Cells(8).Value)
                'txt_KuraiPavuMeters.Text = Trim(dgv_delivery_Selections.Rows(i).Cells(6).Value)
                txt_Party_DcNo.Text = Trim(dgv_delivery_Selections.Rows(i).Cells(1).Value)
            End If


            If Val(dgv_delivery_Selections.Rows(i).Cells(7).Value) = 1 Then
                Da = New SqlClient.SqlDataAdapter("Select h.Pavu_Meters,h.Empty_Beam,a.sl_no,a.set_no,Beam_no, c.EndsCount_Name,a.pcs,a.Meters_Pc ,a.Meters , d.Beam_Width_Name,a.* from Weaver_Pavu_Delivery_Details a  inner join Weaver_Pavu_Delivery_head h on a.Weaver_Pavu_Delivery_code=h.Weaver_Pavu_Delivery_code INNER JOIN EndsCount_Head c ON a.EndsCount_IdNo = c.EndsCount_IdNo LEFT OUTER JOIN Beam_Width_Head d ON a.Beam_Width_Idno = d.Beam_Width_Idno where 'WPVDC-'+ a.Weaver_Pavu_Delivery_code ='" & Trim(dgv_delivery_Selections.Rows(i).Cells(8).Value) & "' ", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)



                With dgv_PavuDetails


                    If Val(Dt1.Rows.Count <> 0) Then

                        For j = 0 To Dt1.Rows.Count - 1

                            n = .Rows.Add()

                            .Rows(n).Cells(0).Value = Trim(Dt1.Rows(j).Item("Sl_No").ToString)
                            .Rows(n).Cells(1).Value = Trim(Dt1.Rows(j).Item("Set_no").ToString)
                            .Rows(n).Cells(2).Value = Trim(Dt1.Rows(j).Item("Beam_no").ToString)
                            .Rows(n).Cells(3).Value = Trim(Dt1.Rows(j).Item("EndsCount_Name").ToString)
                            .Rows(n).Cells(4).Value = Val(Dt1.Rows(j).Item("pcs").ToString)
                            .Rows(n).Cells(5).Value = Val(Dt1.Rows(j).Item("Meters_Pc").ToString)
                            .Rows(n).Cells(6).Value = Val(Dt1.Rows(j).Item("Meters").ToString)
                            .Rows(n).Cells(7).Value = Val(Dt1.Rows(j).Item("pcs").ToString)

                            .Rows(n).Cells(8).Value = Val(Dt1.Rows(j).Item("Meters").ToString)

                            .Rows(n).Cells(12).Value = Trim(Dt1.Rows(j).Item("Set_Code").ToString)

                            txt_KuraiPavuMeters.Text = Val(Dt1.Rows(j).Item("pavu_Meters").ToString)

                            txt_KuraiPavuBeam.Text = Val(Dt1.Rows(j).Item("Empty_Beam").ToString)


                        Next
                    End If

                End With
                Dt1.Clear()

                Exit For
            End If

        Next

        pnl_Back.Enabled = True
        pnl_Delivery_Selection.Visible = False

    End Sub

    Private Sub btn_Close_Delivery_Selection_Click(sender As Object, e As EventArgs) Handles btn_Close_Delivery_Selection.Click
        Close_Delivery_Selection()
    End Sub
    Private Sub cbo_Type_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Type.Enter
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")
    End Sub

    Private Sub cbo_Type_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Type.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Type, msk_date, txt_Party_DcNo, "", "", "", "")
    End Sub

    Private Sub cbo_Type_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Type.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Type, txt_Party_DcNo, "", "", "", "")


        If Asc(e.KeyChar) = 13 Then
            If Trim(cbo_Type.Text) = "DELIVERY" Then
                btn_Delivery_Selection.Visible = True
            Else
                btn_Delivery_Selection.Visible = False

            End If
        End If

    End Sub

    Private Sub dgv_delivery_Selections_Click(sender As Object, e As EventArgs) Handles dgv_delivery_Selections.Click

    End Sub


    Private Sub Select_Dc(ByVal RwIndx As Integer)
        Dim i As Integer




        With dgv_delivery_Selections

            If .RowCount > 0 And RwIndx >= 0 Then

                For i = 0 To .Rows.Count - 1
                    .Rows(i).Cells(7).Value = ""
                Next

                .Rows(RwIndx).Cells(7).Value = 1

                If Val(.Rows(RwIndx).Cells(7).Value) = 1 Then

                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
                    Next


                Else
                    .Rows(RwIndx).Cells(7).Value = ""

                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Black
                    Next

                End If


                Close_Delivery_Selection()
            End If
        End With
    End Sub



    Private Sub dgv_delivery_Selections_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgv_delivery_Selections.CellClick
        Select_Dc(e.RowIndex)
    End Sub
    Private Sub dgv_delivery_Selections_CellMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles dgv_delivery_Selections.CellMouseClick
        btn_Close_Delivery_Selection_Click(sender, e)
    End Sub

    Private Sub dgv_delivery_Selections_KeyDown(sender As Object, e As KeyEventArgs) Handles dgv_delivery_Selections.KeyDown
        On Error Resume Next

        With dgv_delivery_Selections

            If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Space Then
                If .CurrentCell.RowIndex >= 0 Then
                    Select_Dc(.CurrentCell.RowIndex)
                    e.Handled = True
                End If
            End If

            If e.KeyCode = Keys.Back Or e.KeyCode = Keys.Delete Then
                If .CurrentCell.RowIndex >= 0 Then
                    If Val(.Rows(.CurrentCell.RowIndex).Cells(7).Value) = 1 Then
                        Select_Dc(.CurrentCell.RowIndex)
                        e.Handled = True
                    End If
                End If
            End If

        End With


    End Sub

    Private Sub btn_Delivery_Selection_Click(sender As Object, e As EventArgs) Handles btn_Delivery_Selection.Click
        Delivery_Selection()

    End Sub

    Private Sub cbo_weaving_job_no_GotFocus(sender As Object, e As EventArgs) Handles cbo_weaving_job_no.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Weaving_JobCard_Head", "Weaving_JobCode_forSelection", "", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_weaving_job_no_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_weaving_job_no.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_weaving_job_no, Nothing, "Weaving_JobCard_Head", "Weaving_JobCode_forSelection", "", "(Ledger_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            txt_Freight.Focus()
        End If
    End Sub

    Private Sub cbo_weaving_job_no_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_weaving_job_no.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_weaving_job_no, Nothing, Nothing, "Weaving_JobCard_Head", "Weaving_JobCode_forSelection", "", "(Ledger_IdNo = 0)")

        If (e.KeyCode = 40 And cbo_weaving_job_no.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            txt_Freight.Focus()
        End If
        If (e.KeyCode = 38 And cbo_weaving_job_no.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
            cbo_Transport.Focus()
        End If
    End Sub
    Private Sub btn_EWayBIll_Generation_Click(sender As Object, e As EventArgs) Handles btn_EWayBIll_Generation.Click
        btn_GENERATEEWB.Enabled = True
        Grp_EWB.Visible = True
        Grp_EWB.BringToFront()
        Grp_EWB.Left = (Me.Width - pnl_Back.Width) / 2 + 160
        Grp_EWB.Top = (Me.Height - pnl_Back.Height) / 2 + 150

    End Sub
    Private Sub btn_CheckConnectivity_Click(sender As Object, e As EventArgs) Handles btn_CheckConnectivity.Click
        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()

        'Dim einv As New eInvoice(Val(lbl_Company.Tag))
        'einv.GetAuthToken(rtbEWBResponse)

        Dim ewb As New EWB(Val(lbl_Company.Tag))
        EWB.GetAuthToken(rtbEWBResponse)
    End Sub

    Private Sub btn_GENERATEEWB_Click(sender As Object, e As EventArgs) Handles btn_GENERATEEWB.Click

        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable

        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()

        '   Dim NewCode As String = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        Dim NewCode As String = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Dim da As New SqlClient.SqlDataAdapter("Select EwayBill_No  FROM  Weaver_Pavu_Receipt_Head where Weaver_Pavu_Receipt_Code = '" & NewCode & "'", con)
        Dim dt As New DataTable

        da.Fill(dt)

        If dt.Rows.Count = 0 Then
            MessageBox.Show("Please Save the Invoice before proceeding to generate EWB", "Please SAVE", MessageBoxButtons.OKCancel)
            dt.Clear()
            Exit Sub
        End If

        If Not IsDBNull(dt.Rows(0).Item(0)) Then
            If Len(Trim(dt.Rows(0).Item(0))) > 0 Then
                MessageBox.Show("EWB has been generated for this invoice already", "Redundant Request", MessageBoxButtons.OKCancel)
                dt.Clear()
                Exit Sub
            End If
        End If

        dt.Clear()

        Dim CMD As New SqlClient.SqlCommand
        CMD.Connection = con


        CMD.CommandText = "Delete from EWB_Head Where InvCode = '" & NewCode & "'"
        CMD.ExecuteNonQuery()

        CMD.CommandText = "Insert into EWB_Head ([SupplyType]  ,[SubSupplyType]  , [SubSupplyDesc]  ,[DocType]  ,	[EWBGenDocNo]                           ,[EWBDocDate]        ,[FromGSTIN]       ,[FromTradeName]  ,[FromAddress1]   ,[FromAddress2]     ,[FromPlace] ," &
                         "[FromPINCode]     ,	[FromStateCode] ,[ActualFromStateCode] ,[ToGSTIN]       ,[ToTradeName],[ToAddress1]      ,[ToAddress2]    ,[ToPlace]       ,[ToPINCode]       ,[ToStateCode] , [ActualToStateCode] ," &
                         "[TransactionType],[OtherValue]                       ,	[Total_value]       ,	[CGST_Value],[SGST_Value],[IGST_Value]     ,	[CessValue],[CessNonAdvolValue],[TransporterID]    ,[TransporterName]," &
                         "[TransportDOCNo] ,[TransportDOCDate]    ,[TotalInvValue]    ,[TransMode]             ," &
                         "[VehicleNo]      ,[VehicleType]   , [InvCode], [ShippedToGSTIN], [ShippedToTradeName] ) " &
                         " " &
                         " " &
                         "  SELECT               'I'              , '6'             ,   'JOB WORK RETURNS'        ,    'CHL'    , a.Weaver_Pavu_Receipt_No , a.Weaver_Pavu_Receipt_Date     , L.Ledger_GSTINNo, L.Ledger_MainName   , L.Ledger_Address1 +  L.Ledger_Address2 , L.Ledger_Address3 + L.Ledger_Address4 , L.City_Town ," &
                         " L.PinCode     , TS.State_Code  ,TS.State_Code    , C.Company_GSTINNo, C.Company_Name , (case when a.DeliveryTo_IdNo = 4 then (C.Company_Address1+C.Company_Address2) when a.DeliveryTo_IdNo <> 0 then tDELV.Ledger_Address1+tDELV.Ledger_Address2 else C.Company_Address1+C.Company_Address2 end) as deliveryaddress1,  (case when a.DeliveryTo_IdNo = 4 then (c.Company_Address3+C.Company_Address4) when a.DeliveryTo_IdNo <> 0 then tDELV.Ledger_Address3+tDELV.Ledger_Address4 else  c.Company_Address3+C.Company_Address4 end) as deliveryaddress2, (case when a.DeliveryTo_IdNo = 4 then (c.Company_City) when a.DeliveryTo_IdNo <> 0 then tDELV.City_Town else c.Company_City end) as city_town_name, (case when a.DeliveryTo_IdNo = 4 then (c.Company_PinCode) when a.DeliveryTo_IdNo <> 0 then tDELV.Pincode else  c.Company_PinCode end) as pincodee,(case when a.DeliveryTo_IdNo = 4 then (FS.State_Code) when a.DeliveryTo_IdNo <> 0 then DLSC.State_Code ELSE  FS.State_Code END ),  (case when a.DeliveryTo_IdNo = 4 then (FS.State_Code) when a.DeliveryTo_IdNo <> 0 then DLSC.State_Code ELSE  FS.State_Code END   )  as actual_StateCode , " &
                         " 1                     , 0 , a.Net_Amount     , 0 ,  0 , 0   ,   0         ,0                   , t.Ledger_GSTINNo  , t.Ledger_Name ," &
                         " ''        ,''            ,  0        ,     '1'  AS TrMode ," &
                         " a.Vechile_No, 'R', '" & NewCode & "', (case when a.DeliveryTo_IdNo = 4 or a.DeliveryTo_IdNo = 0 then  c.Company_GSTINNo else tDELV.Ledger_GSTINNo end ) as ShippedTo_GSTIN, tDELV.Ledger_MainName as ShippedTo_LedgerName  from Weaver_Pavu_Receipt_Head a inner Join Company_Head C on a.Company_IdNo = C.Company_IdNo " &
                         " Inner Join Ledger_Head L ON a.ReceivedFrom_IdNo = L.Ledger_IdNo Left Outer Join Ledger_Head tDELV on a.DeliveryTo_IdNo <> 0 and a.DeliveryTo_IdNo = tDELV.Ledger_IdNo   left Outer Join Ledger_Head T on a.Transport_IdNo = T.Ledger_IdNo " &
                         " Left Outer Join State_Head FS On C.Company_State_IdNo = fs.State_IdNo left Outer Join State_Head TS on L.Ledger_State_IdNo = TS.State_IdNo  left Outer Join State_Head  DLSC ON tDELV.Ledger_State_IdNo = DLSC.State_IdNo  " &
                         " where a.Weaver_Pavu_Receipt_Code = '" & Trim(NewCode) & "'"
        CMD.ExecuteNonQuery()



        Dim vCgst_Amt As String = 0
        Dim vSgst_Amt As String = 0
        Dim vIgst_AMt As String = 0
        Dim vTax_Perc As String = 0


        CMD.CommandText = "Delete from EWB_Details Where InvCode = '" & NewCode & "'"
        CMD.ExecuteNonQuery()

        ''------------------


        da = New SqlClient.SqlDataAdapter(" Select  I.EndsCount_Name, (I.EndsCount_Name + ' - WARP'  ) , IG.Item_HSN_Code , IG.Item_GST_Percentage , sum(PD.Rcpt_Meters * SD.Rate) As TaxableAmt,sum(PD.Rcpt_Meters) as Qty, 1 , 'MTR' AS Units  , tz.Company_State_IdNo , Lh.Ledger_State_Idno  , SD.GST_Tax_Invoice_Status " &
                                          " from Weaver_Pavu_Receipt_Head SD Inner Join Weaver_Pavu_Receipt_DETAILS Pd On Pd.Weaver_Pavu_Receipt_Code = Sd.Weaver_Pavu_Receipt_Code  Inner Join EndsCount_Head I On PD.EndsCount_IdNo = I.EndsCount_IdNo INNER JOIN Count_Head Ch On Ch.Count_Idno = I.Count_Idno " &
                                          " Inner Join ItemGroup_Head IG on Ch.ItemGroup_IdNo = IG.ItemGroup_IdNo  INNER Join Ledger_Head Lh ON Lh.Ledger_Idno =  SD.ReceivedFrom_IdNo   INNER JOIN Company_Head tz On tz.Company_Idno = SD.Company_Idno   Where SD.Weaver_Pavu_Receipt_Code = '" & Trim(NewCode) & "' Group By " &
                                          " I.EndsCount_Name,IG.ItemGroup_Name,IG.Item_HSN_Code, IG.Item_GST_Percentage , tz.Company_State_IdNo , Lh.Ledger_State_Idno  , SD.GST_Tax_Invoice_Status  ", con)

        Dim DT1 As New DataTable
        DT1 = New DataTable
        da.Fill(dt1)


        If dt1.Rows.Count > 0 Then
            For I = 0 To dt1.Rows.Count - 1

                vTax_Perc = 0

                If Val(dt1.Rows(I).Item("GST_Tax_Invoice_Status")) = 1 Then
                    If dt1.Rows(I).Item("Company_State_IdNo") = dt1.Rows(I).Item("Ledger_State_Idno") Then

                        If Val(dt1.Rows(I).Item(3).ToString) <> 0 Then
                            vCgst_Amt = ((dt1.Rows(I).Item(4) * Val(dt1.Rows(I).Item(3).ToString) / 100) / 2)
                            vSgst_Amt = vCgst_Amt
                            vIgst_AMt = 0
                        Else
                            vCgst_Amt = 0
                            vSgst_Amt = 0
                            vIgst_AMt = 0
                        End If
                    Else

                        If Val(dt1.Rows(I).Item(3).ToString) <> 0 Then
                            vIgst_AMt = (dt1.Rows(I).Item(4) * Val(dt1.Rows(I).Item(3).ToString) / 100)
                            vCgst_Amt = 0
                            vSgst_Amt = 0
                        Else
                            vIgst_AMt = 0
                            vCgst_Amt = 0
                            vSgst_Amt = 0
                        End If

                    End If

                    vTax_Perc = dt1.Rows(I).Item(3).ToString


                Else

                    vIgst_AMt = 0
                    vCgst_Amt = 0
                    vSgst_Amt = 0
                    vTax_Perc = 0

                End If


                CMD.CommandText = "Insert into EWB_Details ([SlNo]                               , [Product_Name]           ,	[Product_Description]     ,	[HSNCode]                 ,	[Quantity]                    ,     [QuantityUnit] ,             Tax_Perc      ,	[CessRate]       ,	[CessNonAdvol]  ,	[TaxableAmount]               , InvCode      ,                   Cgst_Value  ,                       Sgst_Value ,                         Igst_Value) " &
                      " values                 (" & dt1.Rows(I).Item(6).ToString & ",'" & dt1.Rows(I).Item(0) & "', '" & dt1.Rows(I).Item(1) & "', '" & dt1.Rows(I).Item(2) & "', " & dt1.Rows(I).Item(5).ToString & ",         'KGS'          ," & Val(vTax_Perc) & " ,          0          , 0                   ," & dt1.Rows(I).Item(4) & ",'" & NewCode & "',   '" & Str(Val(vCgst_Amt)) & "'        ,   '" & Str(Val(vSgst_Amt)) & "'     , '" & Str(Val(vIgst_AMt)) & "')"

                CMD.ExecuteNonQuery()

            Next
        End If

        DT1.Clear()
        da.Dispose()


        da = New SqlClient.SqlDataAdapter(" Select I.EndsCount_Name, (I.EndsCount_Name + ' - WARP'  ) , IG.Item_HSN_Code , IG.Item_GST_Percentage , (sum(SD.Pavu_Meters)*SD.Rate) As TaxableAmt, sum(SD.Pavu_Meters) as Qty, 201 as SlNo, 'MTR' AS Units  , tz.Company_State_IdNo , Lh.Ledger_State_Idno  , SD.GST_Tax_Invoice_Status  " &
                                          " from Weaver_Pavu_Receipt_Head SD Inner Join EndsCount_Head I On SD.EndsCount_IdNo = I.EndsCount_IdNo INNER JOIN Count_Head Ch On Ch.Count_Idno = I.Count_Idno " &
                                          " Inner Join ItemGroup_Head IG on Ch.ItemGroup_IdNo = IG.ItemGroup_IdNo  INNER Join Ledger_Head Lh ON Lh.Ledger_Idno =  SD.ReceivedFrom_IdNo   INNER JOIN Company_Head tz On tz.Company_Idno = SD.Company_Idno  Where SD.Weaver_Pavu_Receipt_Code = '" & Trim(NewCode) & "' and SD.Pavu_Meters > 0 Group By " &
                                          " I.EndsCount_Name,IG.ItemGroup_Name,IG.Item_HSN_Code, IG.Item_GST_Percentage,SD.Rate  , tz.Company_State_IdNo , Lh.Ledger_State_Idno  , SD.GST_Tax_Invoice_Status  ", con)
        DT1 = New DataTable
        da.Fill(dt1)
        If dt1.Rows.Count > 0 Then
            For I = 0 To dt1.Rows.Count - 1

                vTax_Perc = 0

                If Val(dt1.Rows(I).Item("GST_Tax_Invoice_Status")) = 1 Then
                    If dt1.Rows(I).Item("Company_State_IdNo") = dt1.Rows(I).Item("Ledger_State_Idno") Then

                        If Val(dt1.Rows(I).Item(3).ToString) <> 0 Then
                            vCgst_Amt = ((dt1.Rows(I).Item(4) * Val(dt1.Rows(I).Item(3).ToString) / 100) / 2)
                            vSgst_Amt = vCgst_Amt
                            vIgst_AMt = 0
                        Else
                            vCgst_Amt = 0
                            vSgst_Amt = 0
                            vIgst_AMt = 0
                        End If
                    Else

                        If Val(dt1.Rows(I).Item(3).ToString) <> 0 Then
                            vIgst_AMt = (dt1.Rows(I).Item(4) * Val(dt1.Rows(I).Item(3).ToString) / 100)
                            vCgst_Amt = 0
                            vSgst_Amt = 0
                        Else
                            vIgst_AMt = 0
                            vCgst_Amt = 0
                            vSgst_Amt = 0
                        End If

                    End If

                    vTax_Perc = dt1.Rows(I).Item(3).ToString


                Else

                    vIgst_AMt = 0
                    vCgst_Amt = 0
                    vSgst_Amt = 0
                    vTax_Perc = 0

                End If

                CMD.CommandText = "Insert into EWB_Details ([SlNo]                               , [Product_Name]           ,	[Product_Description]     ,	[HSNCode]                 ,	[Quantity]                    ,     [QuantityUnit] ,             Tax_Perc      ,	[CessRate]       ,	[CessNonAdvol]  ,	[TaxableAmount]               , InvCode      ,                   Cgst_Value  ,                       Sgst_Value ,                         Igst_Value) " &
                      " values                 (" & dt1.Rows(I).Item(6).ToString & ",'" & dt1.Rows(I).Item(0) & "', '" & dt1.Rows(I).Item(1) & "', '" & dt1.Rows(I).Item(2) & "', " & dt1.Rows(I).Item(5).ToString & ",         'KGS'          ," & Val(vTax_Perc) & " ,          0          , 0                   ," & dt1.Rows(I).Item(4) & ",'" & NewCode & "',   '" & Str(Val(vCgst_Amt)) & "'        ,   '" & Str(Val(vSgst_Amt)) & "'     , '" & Str(Val(vIgst_AMt)) & "')"
                CMD.ExecuteNonQuery()

            Next
        End If




        ''-------------


        da1 = New SqlClient.SqlDataAdapter(" Select  * from EWB_Details Ewd  Where Ewd.InvCode = '" & Trim(NewCode) & "' and (Ewd.Cgst_Value <> 0 or Ewd.Sgst_Value <> 0 or Ewd.Igst_Value <> 0) ", con)
        dt2 = New DataTable
        da1.Fill(dt2)

        If dt2.Rows.Count > 0 Then

            If dt2.Rows(0).Item("Igst_Value") <> 0 Then

                CMD.CommandText = " Update EWB_Head Set IGST_Value = (select sum(Ed.Igst_Value) from EWB_Details Ed  where Ed.InvCode = '" & Trim(NewCode) & "' and Ed.Igst_Value <> 0) "
                CMD.ExecuteNonQuery()
            Else
                CMD.CommandText = " Update EWB_Head Set CGST_Value = (select sum(Ed.Cgst_Value) from EWB_Details Ed  where Ed.InvCode = '" & Trim(NewCode) & "' and Ed.Cgst_Value <> 0 ) "
                CMD.ExecuteNonQuery()

                CMD.CommandText = " Update EWB_Head Set SGST_Value = (select sum(Ed.Sgst_Value) from EWB_Details Ed where Ed.InvCode = '" & Trim(NewCode) & "' and Ed.Sgst_Value <> 0) "
                CMD.ExecuteNonQuery()
            End If

        End If

        dt2.Clear()


        btn_GENERATEEWB.Enabled = False

        Dim ewb As New EWB(Val(lbl_Company.Tag))
        EWB.GenerateEWB(NewCode, con, rtbEWBResponse, txt_EWBNo, "Weaver_Pavu_Receipt_Head", "EwayBill_No", "Weaver_Pavu_Receipt_Code", Pk_Condition)


    End Sub
    Private Sub btn_PRINTEWB_Click(sender As Object, e As EventArgs) Handles btn_PRINTEWB.Click
        Dim ewb As New EWB(Val(lbl_Company.Tag))
        EWB.PrintEWB(txt_EWBNo.Text, rtbEWBResponse, 0)
    End Sub

    Private Sub btn_CancelEWB_1_Click(sender As Object, e As EventArgs) Handles btn_CancelEWB_1.Click
        'Dim NewCode As String = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Dim NewCode As String = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        Dim c As Integer = MsgBox("Are You Sure To Cancel This EWB ? ", vbYesNo)

        If c = vbNo Then Exit Sub

        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()

        Dim ewb As New EWB(Val(lbl_Company.Tag))

        EWB.CancelEWB(txt_EWBNo.Text, NewCode, con, rtbEWBResponse, txt_EWBNo, "Weaver_Pavu_Receipt_Head", "EwayBill_No", "Weaver_Pavu_Receipt_Code")

    End Sub

    Private Sub btn_Close_EWB_Click(sender As Object, e As EventArgs) Handles btn_Close_EWB.Click
        Grp_EWB.Visible = False
    End Sub
    Private Sub btn_Detail_PRINTEWB_Click(sender As Object, e As EventArgs) Handles btn_Detail_PRINTEWB.Click
        Dim ewb As New EWB(Val(lbl_Company.Tag))
        EWB.PrintEWB(txt_EWBNo.Text, rtbEWBResponse, 1)
    End Sub
    Private Sub txt_EWBNo_TextChanged(sender As Object, e As EventArgs) Handles txt_EWBNo.TextChanged
        With chk_Ewb_No_Sts
            If Trim(txt_EWBNo.Text) <> "" Then
                .Checked = True
            Else
                .Checked = False
            End If
        End With
    End Sub
    Private Sub txt_rate_TextChanged(sender As Object, e As System.EventArgs) Handles txt_rate.TextChanged
        Amount_Calculation()
    End Sub
    Private Sub Amount_Calculation()

        Dim vTotMtrs As String = ""

        vTotMtrs = 0
        With dgv_PavuDetails_Total
            If .RowCount > 0 Then
                vTotMtrs = Format(Val(.Rows(0).Cells(8).Value), "########0.000")
            End If
        End With
        If Val(vTotMtrs) <> 0 Then
            txt_Amount.Text = 0
            txt_Amount.Text = Format(Val(vTotMtrs) * Val(txt_rate.Text), "############0.00")
        ElseIf Val(txt_KuraiPavuMeters.Text) <> 0 Then
            txt_Amount.Text = 0
            txt_Amount.Text = Format(Val(txt_KuraiPavuMeters.Text) * Val(txt_rate.Text), "############0.00")
        End If
    End Sub
    Private Sub txt_rate_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_rate.KeyDown
        If e.KeyCode = 38 Then
            txt_Note.Focus()
        ElseIf e.KeyCode = 40 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                msk_date.Focus()
            End If
        End If
    End Sub

    Private Sub txt_rate_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_rate.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                msk_date.Focus()
            End If
        End If
    End Sub

    Private Sub txt_KuraiPavuMeters_TextChanged(sender As Object, e As EventArgs) Handles txt_KuraiPavuMeters.TextChanged
        Amount_Calculation()

    End Sub

    Private Sub dgtxt_Details_TextChanged(sender As Object, e As EventArgs) Handles dgtxt_Details.TextChanged
        Try
            With dgv_PavuDetails
                If .Rows.Count <> 0 Then
                    .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(dgtxt_Details.Text)
                End If
            End With

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_ClothSales_OrderCode_forSelection_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_ClothSales_OrderCode_forSelection.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, sender, Nothing, "ClothSales_Order_Head", "ClothSales_OrderCode_forSelection", vCLO_CONDT, "(ClothSales_Order_Code = '999999/00-00')")

        If Asc(e.KeyChar) = 13 Then
            If dgv_PavuDetails.Rows.Count > 0 Then
                dgv_PavuDetails.Focus()
                dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(0).Cells(7)
                dgv_PavuDetails.CurrentCell.Selected = True

            Else
                txt_Note.Focus()

            End If
        End If


    End Sub

    Private Sub cbo_ClothSales_OrderCode_forSelection_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_ClothSales_OrderCode_forSelection.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, sender, Nothing, Nothing, "ClothSales_Order_Head", "ClothSales_OrderCode_forSelection", vCLO_CONDT, "(ClothSales_Order_Code = '999999/00-00')")

        If (e.KeyCode = 38 And cbo_ClothSales_OrderCode_forSelection.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

            If cbo_WidthType.Visible = True Then
                cbo_WidthType.Focus()
            ElseIf txt_Freight.Visible = True Then
                txt_Freight.Focus()
            ElseIf cbo_weaving_job_no.Visible = True Then
                cbo_weaving_job_no.Focus()
            Else
                cbo_Transport.Focus()
            End If

        End If



        If (e.KeyCode = 40 And cbo_ClothSales_OrderCode_forSelection.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If dgv_PavuDetails.Rows.Count > 0 Then
                dgv_PavuDetails.Focus()
                dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(0).Cells(7)
                dgv_PavuDetails.CurrentCell.Selected = True

            Else
                txt_Note.Focus()

            End If
        End If


    End Sub

    Private Sub cbo_ClothSales_OrderCode_forSelection_Enter(sender As Object, e As EventArgs) Handles cbo_ClothSales_OrderCode_forSelection.Enter
        vCLO_CONDT = "(ClothSales_Order_Code LIKE '%/" & Trim(FnYearCode1) & "' or ClothSales_Order_Code LIKE '%/" & Trim(FnYearCode2) & "' and Order_Close_Status = 0 )"
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "ClothSales_Order_Head", "ClothSales_OrderCode_forSelection", vCLO_CONDT, "(ClothSales_Order_Code = '999999/00-00')")
    End Sub

    Private Sub txt_Freight_TextChanged(sender As Object, e As EventArgs) Handles txt_Freight.TextChanged

    End Sub

    Private Sub cbo_ClothSales_OrderCode_forSelection_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_ClothSales_OrderCode_forSelection.SelectedIndexChanged

    End Sub

    Private Sub cbo_WidthType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_WidthType.SelectedIndexChanged

    End Sub

    Private Sub lbl_ClothSales_OrderCode_forSelection_Caption_Click(sender As Object, e As EventArgs) Handles lbl_ClothSales_OrderCode_forSelection_Caption.Click

    End Sub
End Class

