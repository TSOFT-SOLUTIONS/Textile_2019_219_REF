Public Class Weaver_Yarn_Receipt
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "WYNRC-"
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
    Private prn_TotCopies As Integer = 0
    Private prn_Count As Integer = 0
    Private prn_HeadIndx As Integer

    Private PrntCnt2ndPageSTS As Boolean = False
    Private prn_Prev_HeadIndx As Integer
    Private prn_PageNo1 As Integer
    Private prn_DetIndx1 As Integer
    Private prn_NoofBmDets1 As Integer
    Private prn_DetSNo1 As Integer


    Private dgv_ActiveCtrl_Name As String
    Private Prnt_HalfSheet_STS As Boolean = False
    Private vPrnt_2Copy_In_SinglePage As Integer = 0

    Public vmskOldText As String = ""
    Public vmskSelStrt As Integer = -1

    Private vCLO_CONDT As String = ""
    Private FnYearCode1 As String = ""
    Private FnYearCode2 As String = ""

    Private Sub clear()

        NoCalc_Status = True

        New_Entry = False
        Insert_Entry = False

        Pnl_Back.Enabled = True
        pnl_Filter.Visible = False


        lbl_RecNo.Text = ""
        lbl_RecNo.ForeColor = Color.Black

        msk_date.Text = ""
        dtp_Date.Text = ""
        cbo_Ledger.Text = ""
        cbo_TransportName.Text = ""
        cbo_Grid_CountName.Text = ""
        cbo_Grid_MillName.Text = ""
        cbo_Grid_YarnType.Text = ""
        cbo_Vechile.Text = ""
        cbo_Cloth.Text = ""

        cbo_Filter_CountName.Text = ""
        cbo_Filter_MillName.Text = ""
        cbo_Filter_PartyName.Text = ""
        txt_Party_DcNo.Text = ""
        txt_Empty_Beam.Text = ""
        txt_Empty_Gunnies.Text = ""
        txt_Empty_Cones.Text = ""
        txt_Freight.Text = ""
        cbo_DeliveryTo.Text = ""

        Grp_EWB.Visible = False
        txt_Eway_Bill_No.Text = ""
        txt_remarks.Text = ""
        txt_rate.Text = ""
        txt_EWBNo.Text = ""
        chk_Ewb_No_Sts.Checked = False
        chk_GSTTax_Invocie.Checked = True
        txt_Amount.Text = ""

        cbo_ClothSales_OrderCode_forSelection.Text = ""

        If cbo_weaving_job_no.Visible Then cbo_weaving_job_no.Text = ""

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1417" Then '---- SAKTHIVEL IMPEX (SOMANUR)
            cbo_DeliveryTo.Text = Common_Procedures.Ledger_IdNoToName(con, 4)
        End If




        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))
        dgv_YarnDetails.Rows.Clear()
        dgv_YarnDetails_Total.Rows.Clear()
        dgv_YarnDetails_Total.Rows.Add()

        pnl_Delivery_Selection.Visible = False
        cbo_Type.text = "DIRECT"
        lbl_Delivery_Code.Text = ""
        If Filter_Status = False Then
            dtp_Filter_Fromdate.Text = ""
            dtp_Filter_ToDate.Text = ""
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_CountName.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_CountName.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()
        End If

        Grid_Cell_DeSelect()

        dgv_ActiveCtrl_Name = ""
        cbo_Grid_CountName.Visible = False
        cbo_Grid_MillName.Visible = False
        cbo_Grid_YarnType.Visible = False

        NoCalc_Status = False
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

        If Me.ActiveControl.Name <> cbo_Grid_CountName.Name Then
            cbo_Grid_CountName.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Grid_MillName.Name Then
            cbo_Grid_MillName.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Grid_YarnType.Name Then
            cbo_Grid_YarnType.Visible = False
        End If

        If Me.ActiveControl.Name <> dgv_YarnDetails_Total.Name Then
            Grid_DeSelect()
        End If

        If Me.ActiveControl.Name <> dgv_YarnDetails.Name Then
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
        If Not IsNothing(dgv_YarnDetails.CurrentCell) Then dgv_YarnDetails.CurrentCell.Selected = False
        If Not IsNothing(dgv_YarnDetails_Total.CurrentCell) Then dgv_YarnDetails_Total.CurrentCell.Selected = False
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
        If Not IsNothing(dgv_YarnDetails.CurrentCell) Then dgv_YarnDetails.CurrentCell.Selected = False
        If Not IsNothing(dgv_YarnDetails_Total.CurrentCell) Then dgv_YarnDetails_Total.CurrentCell.Selected = False
        If Not IsNothing(dgv_Filter_Details.CurrentCell) Then dgv_Filter_Details.CurrentCell.Selected = False
    End Sub

    Private Sub Weaver_Yarn_Receipt_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Dim da As SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NoofComps As Integer
        Dim CompCondt As String

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Ledger.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Ledger.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_DeliveryTo.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_DeliveryTo.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If


            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_TransportName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "TRANSPORT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_TransportName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If


            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_YarnType.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "YARN" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_YarnType.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_CountName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "COUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_CountName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_MillName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "MILL" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_MillName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

    Private Sub Weaver_Yarn_Receipt_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub Weaver_Yarn_Receipt_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Me.Text = ""

        con.Open()

        cbo_Grid_CountName.Visible = False
        cbo_Grid_MillName.Visible = False
        cbo_Grid_YarnType.Visible = False

        dtp_Date.Text = ""
        msk_date.Text = ""

        cbo_Cloth.Visible = False
        lbl_Cloth.Visible = False
        If Val(Common_Procedures.settings.Weaver_YarnStock_InMeter_Status) = 1 Then
            dgv_YarnDetails.Columns(7).HeaderText = "METERS"
            lbl_Cloth.Visible = True
            cbo_Cloth.Visible = True
        End If

        If Common_Procedures.settings.Textile_Sizing_Delivery_receipt_Selection = 1 Then
            cbo_Type.Visible = True
            lbl_type_caption.Visible = True
            btn_Delivery_Selection.Visible = True
        End If
        btn_UserModification.Visible = False
        If Common_Procedures.settings.User_Modifications_Show_Status = 1 Then
            If Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_UserModification_Status = 1 Then
                btn_UserModification.Visible = True
            End If
        End If
        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2

        pnl_Selection.Visible = False
        pnl_Selection.Left = (Me.Width - pnl_Selection.Width) \ 2
        pnl_Selection.Top = (Me.Height - pnl_Selection.Height) \ 2
        pnl_Selection.BringToFront()

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1352" Then
            lbl_Cloth.Visible = True
            cbo_Cloth.Visible = True
        End If

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
        cbo_Type.Items.Clear()
        cbo_Type.Items.Add(" ")
        cbo_Type.Items.Add("DIRECT")
        cbo_Type.Items.Add("DELIVERY")

        If Trim(UCase(cbo_Type.Text)) = "DELIVERY" Then
            dgv_YarnDetails.AllowUserToAddRows = False
        End If

        pnl_Delivery_Selection.Visible = False
        pnl_Delivery_Selection.Left = (Me.Width - pnl_Delivery_Selection.Width) \ 2
        pnl_Delivery_Selection.Top = (Me.Height - pnl_Delivery_Selection.Height) \ 2
        pnl_Delivery_Selection.BringToFront()

        If Common_Procedures.settings.Show_Weaver_JobCard_Entry_STATUS = 1 Then
            lbl_weaving_job_no.Visible = True
            cbo_weaving_job_no.Visible = True

            lbl_weaving_job_no.Left = LBL_TRANSPORT.Left
            cbo_weaving_job_no.Left = cbo_TransportName.Left
            cbo_weaving_job_no.Width = cbo_TransportName.Width
            cbo_weaving_job_no.BackColor = Color.White

        End If

        If Common_Procedures.settings.Show_Sales_OrderNumber_in_ALLEntry_Status = 1 Then

            cbo_ClothSales_OrderCode_forSelection.Visible = True
            lbl_ClothSales_OrderCode_forSelection_Caption.Visible = True


            FnYearCode1 = ""
            FnYearCode2 = ""
            Common_Procedures.get_FnYearCode_of_Last_2_Years(FnYearCode1, FnYearCode2)

        Else

            cbo_ClothSales_OrderCode_forSelection.Visible = False
            lbl_ClothSales_OrderCode_forSelection_Caption.Visible = False

        End If


        AddHandler chk_Verified_Status.GotFocus, AddressOf ControlGotFocus
        AddHandler chk_Verified_Status.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Ledger.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_TransportName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Vechile.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_CountName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_MillName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_YarnType.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Empty_Beam.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Empty_Gunnies.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Empty_Cones.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Party_DcNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Freight.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_DeliveryTo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_rate.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_remarks.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Eway_Bill_No.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ClothSales_OrderCode_forSelection.GotFocus, AddressOf ControlGotFocus



        AddHandler cbo_Filter_CountName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_MillName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Cloth.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Type.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_weaving_job_no.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_Cloth.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Type.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Ledger.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_TransportName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Vechile.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_CountName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_MillName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_YarnType.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Empty_Beam.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Empty_Gunnies.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Empty_Cones.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Party_DcNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Freight.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_DeliveryTo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ClothSales_OrderCode_forSelection.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_rate.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_remarks.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Eway_Bill_No.LostFocus, AddressOf ControlLostFocus


        AddHandler cbo_Filter_CountName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_MillName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_weaving_job_no.LostFocus, AddressOf ControlLostFocus


        'AddHandler msk_date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Empty_Beam.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Empty_Gunnies.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Empty_Cones.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Party_DcNo.KeyDown, AddressOf TextBoxControlKeyDown

        'AddHandler msk_date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Empty_Beam.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Empty_Gunnies.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Empty_Cones.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Party_DcNo.KeyPress, AddressOf TextBoxControlKeyPress

        'Dgv_Details Columns- 35,100,85,270,75,80,105
        'Dgv_Details Columns- 35,80,70,240,70,80,85,90 After Add Thiri


        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True
        new_record()
    End Sub
    Private Sub Weaver_Yarn_Receipt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

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
                Else
                    Close_Form()

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
        Dim dgv1 As New DataGridView

        On Error Resume Next

        If ActiveControl.Name = dgv_YarnDetails.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            dgv1 = Nothing

            If ActiveControl.Name = dgv_YarnDetails.Name Then
                dgv1 = dgv_YarnDetails

            ElseIf dgv_YarnDetails.IsCurrentRowDirty = True Then
                dgv1 = dgv_YarnDetails

            ElseIf dgv_ActiveCtrl_Name = dgv_YarnDetails.Name Then
                dgv1 = dgv_YarnDetails
            End If

            If IsNothing(dgv1) = False Then

                With dgv1


                    If keyData = Keys.Enter Or keyData = Keys.Down Then
                        If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                btn_save.Focus()

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                            End If

                        Else
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then

                        If .CurrentCell.ColumnIndex <= 1 Then
                            If .CurrentCell.RowIndex = 0 Then
                                cbo_TransportName.Focus()

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 1)

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
    Private Sub move_record(ByVal no As String)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim NewCode As String
        Dim n As Integer
        Dim SNo As Integer

        If Val(no) = 0 Then Exit Sub

        clear()

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try
            da1 = New SqlClient.SqlDataAdapter("select a.* from Weaver_Yarn_Receipt_Head a Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_Yarn_Receipt_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_RecNo.Text = dt1.Rows(0).Item("Weaver_Yarn_Receipt_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Weaver_Yarn_Receipt_Date")
                msk_date.Text = dtp_Date.Text

                cbo_Ledger.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Ledger_IdNo").ToString))
                cbo_DeliveryTo.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("DeliveryTo_IdNo").ToString))
                cbo_TransportName.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Transport_IdNo").ToString))

                cbo_Vechile.Text = dt1.Rows(0).Item("Vechile_No").ToString


                txt_Party_DcNo.Text = dt1.Rows(0).Item("Party_DcNo").ToString

                If Val(dt1.Rows(0).Item("Empty_Beam").ToString) <> 0 Then
                    txt_Empty_Beam.Text = Val(dt1.Rows(0).Item("Empty_Beam").ToString)
                End If


                If Val(dt1.Rows(0).Item("Empty_Gunnies").ToString) <> 0 Then
                    txt_Empty_Gunnies.Text = Val(dt1.Rows(0).Item("Empty_Gunnies").ToString)
                End If

                If Val(dt1.Rows(0).Item("Empty_Cones").ToString) <> 0 Then
                    txt_Empty_Cones.Text = Val(dt1.Rows(0).Item("Empty_Cones").ToString)
                End If

                If Val(dt1.Rows(0).Item("Freight").ToString) <> 0 Then
                    txt_Freight.Text = Val(dt1.Rows(0).Item("Freight").ToString)
                End If

                cbo_Cloth.Text = Common_Procedures.Cloth_IdNoToName(con, Val(dt1.Rows(0).Item("Cloth_IdNo").ToString))
                lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))
                lbl_OrderNo.Text = dt1.Rows(0).Item("Our_Order_No").ToString
                lbl_OrderCode.Text = dt1.Rows(0).Item("Own_Order_Code").ToString
                lbl_Delivery_Code.Text = dt1.Rows(0).Item("Delivery_Code").ToString
                cbo_Type.Text = dt1.Rows(0).Item("Selection_type").ToString
                If Val(dt1.Rows(0).Item("Verified_Status").ToString) = 1 Then chk_Verified_Status.Checked = True

                cbo_weaving_job_no.Text = dt1.Rows(0).Item("Weaving_JobCode_forSelection").ToString

                txt_rate.Text = dt1.Rows(0).Item("Rate").ToString
                txt_Amount.Text = dt1.Rows(0).Item("Net_Amount").ToString
                txt_Eway_Bill_No.Text = dt1.Rows(0).Item("EwayBill_No").ToString
                txt_remarks.Text = dt1.Rows(0).Item("remarks").ToString

                txt_EWBNo.Text = dt1.Rows(0).Item("EwayBill_No").ToString


                cbo_ClothSales_OrderCode_forSelection.Text = dt1.Rows(0).Item("ClothSales_OrderCode_forSelection").ToString

                If Trim(txt_EWBNo.Text) <> "" Then

                    chk_Ewb_No_Sts.Checked = True
                Else
                    chk_Ewb_No_Sts.Checked = False
                End If

                If Val(dt1.Rows(0).Item("GST_Tax_Invoice_Status").ToString) = 1 Then chk_GSTTax_Invocie.Checked = True Else chk_GSTTax_Invocie.Checked = False


                dt2.Clear()

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Count_Name, c.Mill_Name from Weaver_Yarn_Receipt_Details a INNER JOIN Count_Head b on a.Count_IdNo = b.Count_IdNo INNER JOIN Mill_Head c on a.Mill_IdNo = c.Mill_IdNo where a.Weaver_Yarn_Receipt_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_YarnDetails.Rows.Clear()
                SNo = 0

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_YarnDetails.Rows.Add()

                        SNo = SNo + 1
                        dgv_YarnDetails.Rows(n).Cells(0).Value = Val(SNo)
                        dgv_YarnDetails.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Count_Name").ToString
                        dgv_YarnDetails.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Yarn_Type").ToString
                        dgv_YarnDetails.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Mill_Name").ToString
                        dgv_YarnDetails.Rows(n).Cells(4).Value = Val(dt2.Rows(i).Item("Bags").ToString)
                        dgv_YarnDetails.Rows(n).Cells(5).Value = Val(dt2.Rows(i).Item("Cones").ToString)
                        dgv_YarnDetails.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Weight").ToString), "########0.000")
                        dgv_YarnDetails.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Thiri").ToString), "########0.000")

                    Next i

                End If

                With dgv_YarnDetails_Total
                    If .RowCount = 0 Then .Rows.Add()
                    .Rows(0).Cells(4).Value = Val(dt1.Rows(0).Item("Total_Bags").ToString)
                    .Rows(0).Cells(5).Value = Val(dt1.Rows(0).Item("Total_Cones").ToString)
                    .Rows(0).Cells(6).Value = Format(Val(dt1.Rows(0).Item("Total_Weight").ToString), "########0.000")
                    .Rows(0).Cells(7).Value = Format(Val(dt1.Rows(0).Item("Total_Thiri").ToString), "########0.000")

                End With

                dt2.Clear()

                dt2.Dispose()
                da2.Dispose()

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

        Dim vOrdByNo As String = ""

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_RecNo.Text)


        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Weaver_Yarn_Rceipt_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Weaver_Yarn_Rceipt_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.Weaver_Yarn_Rceipt_Entry, New_Entry, Me, con, "Weaver_Yarn_Receipt_Head", "Weaver_Yarn_Receipt_Code", NewCode, "Weaver_Yarn_Receipt_Date", "(Weaver_Yarn_Receipt_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub


        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Pnl_Back.Enabled = False Then
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
            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "DELETE", "Weaver_Yarn_Receipt_head", "Weaver_Yarn_Receipt_Code", Val(lbl_Company.Tag), NewCode, lbl_RecNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "", "", "Weaver_Yarn_Receipt_Code, Company_IdNo, for_OrderBy", trans)
            Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "DELETE", "Weaver_Yarn_Receipt_Details", "Weaver_Yarn_Receipt_Code", Val(lbl_Company.Tag), NewCode, lbl_RecNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, " Yarn_Type, Mill_IdNo,count_idno, Bags, Cones, Weight ,Thiri ", "Sl_No", "Weaver_Yarn_Receipt_Code, For_OrderBy, Company_IdNo, Weaver_Yarn_Receipt_No, Weaver_Yarn_Receipt_Date, Ledger_Idno", trans)

            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), trans)

            cmd.CommandText = "Truncate Table TempTable_For_NegativeStock"
            cmd.ExecuteNonQuery()

            If Val(Common_Procedures.settings.Negative_Stock_Restriction_for_Yarn_Stock) = 1 Then

                cmd.CommandText = "Insert into TempTable_For_NegativeStock ( Stock_Type, Reference_Code, Reference_Date, Company_Idno, Ledger_Idno    , Count_IdNo, Yarn_Type, Mill_IdNo ) " &
                                      " Select                               'YARN'    , Reference_Code, Reference_Date, Company_IdNo, DeliveryTo_Idno, Count_IdNo, Yarn_Type, Mill_IdNo from Stock_Yarn_Processing_Details where Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If

            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Empty_BeamBagCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Weaver_Yarn_Receipt_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Yarn_Receipt_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Weaver_Yarn_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Yarn_Receipt_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Yarn_Delivery_Selections_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            If Val(Common_Procedures.settings.Negative_Stock_Restriction_for_Yarn_Stock) = 1 Then
                If Common_Procedures.Check_is_Negative_Stock_Status(con, trans) = True Then Exit Sub
            End If

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

            da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or Ledger_Type = 'JOBWORKER') order by Ledger_DisplayName", con)
            da.Fill(dt1)
            cbo_Filter_PartyName.DataSource = dt1
            cbo_Filter_PartyName.DisplayMember = "Ledger_DisplayName"

            da = New SqlClient.SqlDataAdapter("select count_name from count_head order by count_name", con)
            da.Fill(dt2)
            cbo_Filter_CountName.DataSource = dt2
            cbo_Filter_CountName.DisplayMember = "count_name"

            da = New SqlClient.SqlDataAdapter("select Mill_name from Mill_head order by Mill_name", con)
            da.Fill(dt2)
            cbo_Filter_MillName.DataSource = dt2
            cbo_Filter_MillName.DisplayMember = "Mill_name"


            dtp_Filter_Fromdate.Text = ""
            dtp_Filter_ToDate.Text = ""
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_CountName.Text = ""
            cbo_Filter_MillName.Text = ""

            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_CountName.SelectedIndex = -1
            cbo_Filter_MillName.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()

        End If

        pnl_Filter.Visible = True
        pnl_Filter.Enabled = True
        pnl_Filter.BringToFront()
        Pnl_Back.Enabled = False
        If dtp_Filter_Fromdate.Enabled And dtp_Filter_Fromdate.Visible Then dtp_Filter_Fromdate.Focus()

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String

        Try

            da = New SqlClient.SqlDataAdapter("select top 1 Weaver_Yarn_Receipt_No from Weaver_Yarn_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Yarn_Receipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Weaver_Yarn_Receipt_No", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 Weaver_Yarn_Receipt_No from Weaver_Yarn_Receipt_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Yarn_Receipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Weaver_Yarn_Receipt_No", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 Weaver_Yarn_Receipt_No from Weaver_Yarn_Receipt_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Yarn_Receipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Weaver_Yarn_Receipt_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 Weaver_Yarn_Receipt_No from Weaver_Yarn_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Yarn_Receipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Weaver_Yarn_Receipt_No desc", con)
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

            lbl_RecNo.Text = Common_Procedures.get_MaxCode(con, "Weaver_Yarn_Receipt_Head", "Weaver_Yarn_Receipt_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

            lbl_RecNo.ForeColor = Color.Red

            msk_date.Text = Date.Today.ToShortDateString
            da = New SqlClient.SqlDataAdapter("select top 1 * from Weaver_Yarn_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Yarn_Receipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Weaver_Yarn_Receipt_No desc", con)
            dt1 = New DataTable
            da.Fill(dt1)
            If dt1.Rows.Count > 0 Then
                If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)
                    If dt1.Rows(0).Item("Weaver_Yarn_Receipt_Date").ToString <> "" Then msk_date.Text = dt1.Rows(0).Item("Weaver_Yarn_Receipt_Date").ToString
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

            Da = New SqlClient.SqlDataAdapter("select Weaver_Yarn_Receipt_No from Weaver_Yarn_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Yarn_Receipt_Code = '" & Trim(RecCode) & "'", con)
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

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Weaver_Yarn_Rceipt_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Weaver_Yarn_Rceipt_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.Weaver_Yarn_Rceipt_Entry, New_Entry, Me) = False Then Exit Sub

        Try

            inpno = InputBox("Enter New Rec No.", "FOR NEW RECEIPT INSERTION...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Weaver_Yarn_Receipt_No from Weaver_Yarn_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Yarn_Receipt_Code = '" & Trim(RecCode) & "'", con)
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
                    MessageBox.Show("Invalid Rec No", "DOES NOT INSERT NEW RECEIPT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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
        Dim Led_ID As Integer = 0
        Dim Delv_ID As Integer = 0
        Dim Rec_ID As Integer = 0
        Dim Trans_ID As Integer = 0
        Dim Clo_ID As Integer = 0
        Dim EdsCnt_ID As Integer = 0
        Dim Sno As Integer = 0, YSno As Integer = 0
        Dim Partcls As String = ""
        Dim PBlNo As String = ""
        Dim YCnt_ID As Integer = 0
        Dim YMil_ID As Integer = 0
        Dim vTotYrnBags As Single, vTotYrnCones As Single, vTotYrnWeight As Single, vTotYrnThiri As Single
        Dim vENTDB_DelvToIDno As String = 0
        Dim EntID As String = ""
        Dim Thiri_val As Single = 0
        Dim Stock_Weight As Single = 0

        Dim Delv_Ledtype As String = ""
        Dim Rec_Ledtype As String = ""
        Dim OurOrd_No As String = ""
        Dim Verified_STS As Integer = 0

        Dim vOrdByNo As String = ""
        Dim vSELC_DCCODE As String = ""

        Dim Weaver_Job_Code As String = ""
        Dim vGST_Tax_Inv_Sts As Integer = 0



        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_RecNo.Text)


        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If


        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        ' If Common_Procedures.UserRight_Check(Common_Procedures.UR.Weaver_Yarn_Rceipt_Entry, New_Entry) = False Then Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Weaver_Yarn_Rceipt_Entry, New_Entry, Me, con, "Weaver_Yarn_Receipt_Head", "Weaver_Yarn_Receipt_Code", NewCode, "Weaver_Yarn_Receipt_Date", "(Weaver_Yarn_Receipt_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Yarn_Receipt_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, Weaver_Yarn_Receipt_No desc", dtp_Date.Value.Date) = False Then Exit Sub

        If Pnl_Back.Enabled = False Then
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

        Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
        If Led_ID = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
            Exit Sub
        End If
        Delv_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_DeliveryTo.Text)
        If Delv_ID = 0 Then Delv_ID = 4


        If Delv_ID = Led_ID Then
            MessageBox.Show("Invalid Party Name, Does not accept same name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_DeliveryTo.Enabled And cbo_DeliveryTo.Visible Then cbo_DeliveryTo.Focus()
            Exit Sub
        End If

        Verified_STS = 0
        If chk_Verified_Status.Checked = True Then Verified_STS = 1


        If Trim(lbl_OrderCode.Text) <> "" Then


            If Led_ID <> 0 Then

                Da = New SqlClient.SqlDataAdapter("select a.*,b.* from Own_Order_Head a INNER JOIN Own_order_Weaving_Details b ON a.Own_Order_Code =b.Own_Order_Code where a.Own_Order_Code = '" & Trim(lbl_OrderCode.Text) & "' and  b.Ledger_idno = " & Str(Val(Led_ID)), con)
                Dt1 = New DataTable
                Da.Fill(Dt1)
                If Dt1.Rows.Count > 0 Then
                    OurOrd_No = Dt1.Rows(0).Item("Order_No").ToString

                End If
            End If
            If Trim(OurOrd_No) <> Trim(lbl_OrderNo.Text) Then
                MessageBox.Show("Invalid Mismatch Of Order No for this Party Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
                Exit Sub
            End If
        End If
        Trans_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_TransportName.Text)
        Clo_ID = Common_Procedures.Cloth_NameToIdNo(con, cbo_Cloth.Text)

        If Trim(txt_Party_DcNo.Text) <> "" Then
            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            Da = New SqlClient.SqlDataAdapter("select * from Weaver_Yarn_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and DeliveryTo_IdNo = " & Str(Val(Delv_ID)) & " and Party_dcno = '" & Trim(txt_Party_DcNo.Text) & "' and Weaver_Yarn_Receipt_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' and Weaver_Yarn_Receipt_Code <> '" & Trim(NewCode) & "'", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                MessageBox.Show("Duplicate Party Dc No to this Party", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If txt_Party_DcNo.Enabled And txt_Party_DcNo.Visible Then txt_Party_DcNo.Focus()
                Exit Sub
            End If
            Dt1.Clear()
        End If
        For i = 0 To dgv_YarnDetails.RowCount - 1

            If Val(dgv_YarnDetails.Rows(i).Cells(6).Value) <> 0 Then

                YCnt_ID = Common_Procedures.Count_NameToIdNo(con, dgv_YarnDetails.Rows(i).Cells(1).Value)
                If Val(YCnt_ID) = 0 Then
                    MessageBox.Show("Invalid CountName", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(1)
                    dgv_YarnDetails.Focus()
                    Exit Sub
                End If

                If Trim(dgv_YarnDetails.Rows(i).Cells(2).Value) = "" Then
                    MessageBox.Show("Invalid Yarn Type", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(2)
                    dgv_YarnDetails.Focus()
                    Exit Sub
                End If

                YMil_ID = Common_Procedures.Mill_NameToIdNo(con, dgv_YarnDetails.Rows(i).Cells(3).Value)
                If Val(YMil_ID) = 0 Then
                    MessageBox.Show("Invalid Mill Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(3)
                    dgv_YarnDetails.Focus()
                    Exit Sub
                End If

            End If

        Next

        vTotYrnBags = 0 : vTotYrnCones = 0 : vTotYrnWeight = 0 : vTotYrnThiri = 0
        If dgv_YarnDetails_Total.RowCount > 0 Then
            vTotYrnBags = Val(dgv_YarnDetails_Total.Rows(0).Cells(4).Value())
            vTotYrnCones = Val(dgv_YarnDetails_Total.Rows(0).Cells(5).Value())
            vTotYrnWeight = Val(dgv_YarnDetails_Total.Rows(0).Cells(6).Value())
            If Val(Common_Procedures.settings.Weaver_YarnStock_InThiri_Status = 1) Or Val(Common_Procedures.settings.Weaver_YarnStock_InMeter_Status = 1) Then
                vTotYrnThiri = Val(dgv_YarnDetails_Total.Rows(0).Cells(7).Value())
            End If
        End If


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

                lbl_RecNo.Text = Common_Procedures.get_MaxCode(con, "Weaver_Yarn_Receipt_Head", "Weaver_Yarn_Receipt_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@EntryDate", Convert.ToDateTime(msk_date.Text))

            vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_RecNo.Text)

            cmd.CommandText = "Truncate Table TempTable_For_NegativeStock"
            cmd.ExecuteNonQuery()


            If New_Entry = True Then


                cmd.CommandText = "Insert into Weaver_Yarn_Receipt_Head(Weaver_Yarn_Receipt_Code, Company_IdNo, Weaver_Yarn_Receipt_No, for_OrderBy, Weaver_Yarn_Receipt_Date, Ledger_IdNo, Party_DcNo, Empty_Beam, Empty_Gunnies, Empty_Cones, Vechile_No,Transport_IdNo, Total_Bags, Total_Cones, Total_Weight  , Total_Thiri ,Freight ,Cloth_IdNo ,  User_IdNo , Our_Order_No     , Own_Order_Code ,DeliveryTo_IdNo, Verified_Status,Selection_type,Delivery_Code,Weaving_JobCode_forSelection,   Rate  , remarks  , EwayBill_No , Net_Amount  ,GST_Tax_Invoice_Status , ClothSales_OrderCode_forSelection) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RecNo.Text) & "', " & Str(Val(vOrdByNo)) & ", @EntryDate, " & Str(Val(Led_ID)) & ", '" & Trim(txt_Party_DcNo.Text) & "' ,  " & Str(Val(txt_Empty_Beam.Text)) & " ,  " & Str(Val(txt_Empty_Gunnies.Text)) & " ,  " & Str(Val(txt_Empty_Cones.Text)) & " , '" & Trim(cbo_Vechile.Text) & "', " & Str(Val(Trans_ID)) & " , " & Str(Val(vTotYrnBags)) & ", " & Str(Val(vTotYrnCones)) & ", " & Str(Val(vTotYrnWeight)) & " ," & Str(Val(vTotYrnThiri)) & " , " & Str(Val(txt_Freight.Text)) & " ," & Val(Clo_ID) & ", " & Val(Common_Procedures.User.IdNo) & ",'" & Trim(lbl_OrderNo.Text) & "' ,    '" & Trim(lbl_OrderCode.Text) & "'," & Str(Val(Delv_ID)) & ", " & Val(Verified_STS) & ",'" & Trim(cbo_Type.Text) & "','" & Trim(vSELC_DCCODE) & "','" & Trim(Weaver_Job_Code) & "'," & Val(txt_rate.Text) & ",'" & Trim(txt_remarks.Text) & "','" & Trim(txt_Eway_Bill_No.Text) & "'," & Val(txt_Amount.Text) & " , " & Str(Val(vGST_Tax_Inv_Sts)) & " , '" & Trim(cbo_ClothSales_OrderCode_forSelection.Text) & "' ) "
                cmd.ExecuteNonQuery()

            Else
                Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "OLD", "Weaver_Yarn_Receipt_head", "Weaver_Yarn_Receipt_Code", Val(lbl_Company.Tag), NewCode, lbl_RecNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Weaver_Yarn_Receipt_Code, Company_IdNo, for_OrderBy", tr)
                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "OLD", "Weaver_Yarn_Receipt_Details", "Weaver_Yarn_Receipt_Code", Val(lbl_Company.Tag), NewCode, lbl_RecNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, " Yarn_Type, Mill_IdNo,count_idno, Bags, Cones, Weight ,Thiri ", "Sl_No", "Weaver_Yarn_Receipt_Code, For_OrderBy, Company_IdNo, Weaver_Yarn_Receipt_No, Weaver_Yarn_Receipt_Date, Ledger_Idno", tr)

                If Val(Common_Procedures.settings.Negative_Stock_Restriction_for_Yarn_Stock) = 1 Then

                    vENTDB_DelvToIDno = Val(Common_Procedures.get_FieldValue(con, "Weaver_Yarn_Receipt_Head", "DeliveryTo_IdNo", "(Weaver_Yarn_Receipt_Code = '" & Trim(NewCode) & "')", , tr))

                    If Val(vENTDB_DelvToIDno) <> Val(Delv_ID) Then

                        cmd.CommandText = "Insert into TempTable_For_NegativeStock ( Stock_Type, Reference_Code, Reference_Date, Company_Idno, Ledger_Idno    , Count_IdNo, Yarn_Type, Mill_IdNo ) " &
                                            " Select                               'YARN'    , Reference_Code, Reference_Date, Company_IdNo, DeliveryTo_Idno, Count_IdNo, Yarn_Type, Mill_IdNo from Stock_Yarn_Processing_Details where Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                        cmd.ExecuteNonQuery()

                    End If

                End If

                cmd.CommandText = "Update Weaver_Yarn_Receipt_Head set Weaver_Yarn_Receipt_Date = @EntryDate, Ledger_IdNo = " & Str(Val(Led_ID)) & " , Party_DcNo = '" & Trim(txt_Party_DcNo.Text) & "' , Transport_IdNo = " & Str(Val(Trans_ID)) & ", Empty_Beam = " & Str(Val(txt_Empty_Beam.Text)) & " , Empty_Gunnies = " & Str(Val(txt_Empty_Gunnies.Text)) & " , Empty_Cones = " & Str(Val(txt_Empty_Cones.Text)) & " , Vechile_No = '" & Trim(cbo_Vechile.Text) & "' ,  Total_Bags = " & Str(Val(vTotYrnBags)) & ", Total_Cones = " & Str(Val(vTotYrnCones)) & ", Total_Weight = " & Str(Val(vTotYrnWeight)) & " ,Total_Thiri =  " & Str(Val(vTotYrnThiri)) & " ,  Freight = " & Str(Val(txt_Freight.Text)) & " , Cloth_IdNo = " & Val(Clo_ID) & ", User_IdNo = " & Val(Common_Procedures.User.IdNo) & " ,Our_Order_No = '" & Trim(lbl_OrderNo.Text) & "',Own_Order_Code = '" & Trim(lbl_OrderCode.Text) & "',DeliveryTo_IdNo = " & Str(Val(Delv_ID)) & ",Verified_Status= " & Val(Verified_STS) & " ,Selection_type='" & Trim(cbo_Type.Text) & "',Delivery_Code='" & Trim(vSELC_DCCODE) & "',Weaving_JobCode_forSelection = '" & Trim(Weaver_Job_Code) & "' , Rate =" & Val(txt_rate.Text) & " , EwayBill_No = '" & Trim(txt_Eway_Bill_No.Text) & "'  ,remarks = '" & Trim(txt_remarks.Text) & "' , Net_Amount = " & Val(txt_Amount.Text) & " ,  GST_Tax_Invoice_Status = " & Str(Val(vGST_Tax_Inv_Sts)) & " , ClothSales_OrderCode_forSelection = '" & Trim(cbo_ClothSales_OrderCode_forSelection.Text) & "'  Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Yarn_Receipt_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If
            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "NEW", "Weaver_Yarn_Receipt_head", "Weaver_Yarn_Receipt_Code", Val(lbl_Company.Tag), NewCode, lbl_RecNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Weaver_Yarn_Receipt_Code, Company_IdNo, for_OrderBy", tr)

            '    Delv_ID = Val(Common_Procedures.CommonLedger.Godown_Ac)
            Rec_ID = Led_ID

            EntID = Trim(Pk_Condition) & Trim(lbl_RecNo.Text)
            If Trim(txt_Party_DcNo.Text) <> "" Then
                Partcls = "Rcpt : P.DcNo. " & Trim(txt_Party_DcNo.Text)
                PBlNo = Trim(txt_Party_DcNo.Text)
            Else
                Partcls = "Rcpt : Rec.No. " & Trim(lbl_RecNo.Text)
                PBlNo = Trim(lbl_RecNo.Text)
            End If

            Delv_Ledtype = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_Type", "(Ledger_IdNo = " & Str(Val(Delv_ID)) & ")", , tr)
            Rec_Ledtype = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_Type", "(Ledger_IdNo = " & Str(Val(Rec_ID)) & ")", , tr)

            cmd.CommandText = "Delete from Weaver_Yarn_Receipt_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Yarn_Receipt_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Empty_BeamBagCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Yarn_Delivery_Selections_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            With dgv_YarnDetails
                Sno = 0
                YSno = 0
                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(6).Value) <> 0 Then

                        Sno = Sno + 1

                        YCnt_ID = Common_Procedures.Count_NameToIdNo(con, .Rows(i).Cells(1).Value, tr)

                        YMil_ID = Common_Procedures.Mill_NameToIdNo(con, .Rows(i).Cells(3).Value, tr)

                        Thiri_val = 0
                        If Val(Common_Procedures.settings.Weaver_YarnStock_InThiri_Status = 1) Or Val(Common_Procedures.settings.Weaver_YarnStock_InMeter_Status = 1) Then
                            If Val(.Rows(i).Cells(7).Value) <> 0 Then
                                Thiri_val = Val(.Rows(i).Cells(7).Value)
                            End If
                        End If

                        cmd.CommandText = "Insert into Weaver_Yarn_Receipt_Details(Weaver_Yarn_Receipt_Code, Company_IdNo, Weaver_Yarn_Receipt_No, for_OrderBy, Weaver_Yarn_Receipt_Date,  Sl_No,  Yarn_Type, Mill_IdNo,count_idno, Bags, Cones, Weight ,Thiri ) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RecNo.Text) & "', " & Str(Val(vOrdByNo)) & ", @EntryDate, " & Str(Val(Sno)) & ",  '" & Trim(.Rows(i).Cells(2).Value) & "', " & Str(Val(YMil_ID)) & ", " & Str(Val(YCnt_ID)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & ", " & Str(Val(.Rows(i).Cells(5).Value)) & ", " & Str(Val(.Rows(i).Cells(6).Value)) & " ," & Str(Val(Thiri_val)) & " )"
                        cmd.ExecuteNonQuery()

                        Stock_Weight = Val(.Rows(i).Cells(6).Value)
                        If Trim(UCase(Rec_Ledtype)) = "WEAVER" Then
                            If Val(Common_Procedures.settings.Weaver_YarnStock_InThiri_Status = 1) Or Val(Common_Procedures.settings.Weaver_YarnStock_InMeter_Status = 1) Then
                                Stock_Weight = 0
                                If Val(.Rows(i).Cells(7).Value) <> 0 Then
                                    Stock_Weight = Val(.Rows(i).Cells(7).Value)
                                End If
                            End If
                        End If

                        If Val(Stock_Weight) <> 0 Then
                            YSno = YSno + 1
                            cmd.CommandText = "Insert into Stock_Yarn_Processing_Details(Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Entry_ID, Particulars, Party_Bill_No, Sl_No, Count_IdNo, Yarn_Type, Mill_IdNo, Bags, Cones, Weight, DeliveryToIdno_ForParticulars, ReceivedFromIdno_ForParticulars , Cloth_IdNo ,Weaving_JobCode_forSelection , ClothSales_OrderCode_forSelection) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RecNo.Text) & "', " & Str(Val(vOrdByNo)) & ", @EntryDate, 0, " & Str(Val(Rec_ID)) & ", '" & Trim(EntID) & "', '" & Trim(Partcls) & "', '" & Trim(PBlNo) & "', " & Str(Val(YSno)) & ", " & Str(Val(YCnt_ID)) & ", '" & Trim(.Rows(i).Cells(2).Value) & "', " & Str(Val(YMil_ID)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & ", " & Str(Val(.Rows(i).Cells(5).Value)) & ", " & Str(Val(Stock_Weight)) & ", " & Str(Val(Delv_ID)) & ", 0 , " & Str(Val(Clo_ID)) & " , '" & Trim(Weaver_Job_Code) & "' , '" & Trim(cbo_ClothSales_OrderCode_forSelection.Text) & "' )"
                            cmd.ExecuteNonQuery()
                        End If

                        Stock_Weight = Val(.Rows(i).Cells(6).Value)
                        If Trim(UCase(Delv_Ledtype)) = "WEAVER" Then
                            If Val(Common_Procedures.settings.Weaver_YarnStock_InThiri_Status = 1) Then
                                Stock_Weight = 0
                                If Val(.Rows(i).Cells(7).Value) <> 0 Then
                                    Stock_Weight = Val(.Rows(i).Cells(7).Value)
                                End If
                            End If
                        End If

                        If Val(Stock_Weight) <> 0 Then
                            YSno = YSno + 1
                            cmd.CommandText = "Insert into Stock_Yarn_Processing_Details ( Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Entry_ID, Particulars, Party_Bill_No, Sl_No, Count_IdNo, Yarn_Type, Mill_IdNo, Bags, Cones, Weight, DeliveryToIdno_ForParticulars, ReceivedFromIdno_ForParticulars , Cloth_IdNo, Weaving_JobCode_forSelection ) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RecNo.Text) & "', " & Str(Val(vOrdByNo)) & ", @EntryDate, " & Str(Val(Delv_ID)) & ", 0, '" & Trim(EntID) & "', '" & Trim(Partcls) & "', '" & Trim(PBlNo) & "', " & Str(Val(YSno)) & ", " & Str(Val(YCnt_ID)) & ", '" & Trim(.Rows(i).Cells(2).Value) & "', " & Str(Val(YMil_ID)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & ", " & Str(Val(.Rows(i).Cells(5).Value)) & ", " & Str(Val(Stock_Weight)) & ", 0, " & Str(Val(Rec_ID)) & " , " & Str(Val(Clo_ID)) & " , '" & Trim(Weaver_Job_Code) & "' )"
                            cmd.ExecuteNonQuery()
                        End If

                    End If

                Next
                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "NEW", "Weaver_Yarn_Receipt_Details", "Weaver_Yarn_Receipt_Code", Val(lbl_Company.Tag), NewCode, lbl_RecNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, " Yarn_Type, Mill_IdNo,count_idno, Bags, Cones, Weight ,Thiri ", "Sl_No", "Weaver_Yarn_Receipt_Code, For_OrderBy, Company_IdNo, Weaver_Yarn_Receipt_No, Weaver_Yarn_Receipt_Date, Ledger_Idno", tr)

            End With

            If Val(txt_Empty_Beam.Text) <> 0 Or Val(txt_Empty_Cones.Text) <> 0 Or Val(txt_Empty_Gunnies.Text) <> 0 Or Val(vTotYrnBags) <> 0 Or Val(vTotYrnCones) <> 0 Then
                cmd.CommandText = "Insert into Stock_Empty_BeamBagCone_Processing_Details ( Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Entry_ID, Party_Bill_No, Particulars, Sl_No, Empty_Beam, Empty_Bags, Empty_Cones, Yarn_Bags, Yarn_Cones ) Values ( '" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RecNo.Text) & "', " & Str(Val(vOrdByNo)) & ", @EntryDate, " & Str(Val(Delv_ID)) & ", " & Str(Val(Rec_ID)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', 1, " & Str(Val(txt_Empty_Beam.Text)) & ", " & Str(Val(txt_Empty_Cones.Text)) & ", " & Str(Val(txt_Empty_Gunnies.Text)) & ", " & Str(Val(vTotYrnBags)) & ", " & Str(Val(vTotYrnCones)) & ")"
                cmd.ExecuteNonQuery()
            End If


            If Common_Procedures.settings.Textile_Sizing_Delivery_receipt_Selection = 1 And Trim(UCase(cbo_Type.Text)) = "DELIVERY" And Trim(vSELC_DCCODE) <> "" Then

                If Val(vTotYrnWeight) <> 0 Then

                    cmd.CommandText = "Insert into Yarn_Delivery_Selections_Processing_Details (             Reference_Code                 , Company_IdNo                       ,             Reference_No      ,           for_OrderBy     , Reference_Date,         Delivery_Code      ,               Delivery_No          ,      DeliveryTo_Idno     ,     ReceivedFrom_Idno   ,               Party_Dc_No          ,                    Total_Bags     ,            total_cones             ,                 Total_Weight        ) " &
                                        "           Values                                     ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & "  , '" & Trim(lbl_RecNo.Text) & "', " & Str(Val(vOrdByNo)) & ",   @EntryDate  ,'" & Trim(vSELC_DCCODE) & "', '" & Trim(txt_Party_DcNo.Text) & "', " & Str(Val(Delv_ID)) & ", " & Str(Val(Led_ID)) & ", '" & Trim(txt_Party_DcNo.Text) & "', " & Str(-1 * Val(vTotYrnBags)) & ", " & Str(-1 * Val(vTotYrnCones)) & ", " & Str(-1 * Val(vTotYrnWeight)) & ")"
                    cmd.ExecuteNonQuery()

                End If



            End If

            Dim vVou_LedIdNos As String = "", vVou_Amts As String = "", vVou_ErrMsg As String = ""

            vVou_LedIdNos = Trans_ID & "|" & Val(Common_Procedures.CommonLedger.Transport_Charges_Ac)
            vVou_Amts = Val(txt_Freight.Text) & "|" & -1 * Val(txt_Freight.Text)
            If Common_Procedures.Voucher_Updation(con, "Wea.YDelv", Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), Trim(lbl_RecNo.Text), Convert.ToDateTime(msk_date.Text), Partcls, vVou_LedIdNos, vVou_Amts, vVou_ErrMsg, tr, Common_Procedures.SoftwareTypes.Textile_Software) = False Then
                Throw New ApplicationException(vVou_ErrMsg)
            End If

            If Val(Common_Procedures.settings.Negative_Stock_Restriction_for_Yarn_Stock) = 1 Then

                cmd.CommandText = "Insert into TempTable_For_NegativeStock ( Stock_Type, Reference_Code, Reference_Date, Company_Idno, Ledger_Idno      , Count_IdNo, Yarn_Type, Mill_IdNo ) " &
                                      " Select                               'YARN'    , Reference_Code, Reference_Date, Company_IdNo, ReceivedFrom_Idno, Count_IdNo, Yarn_Type, Mill_IdNo from Stock_Yarn_Processing_Details where Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                If Common_Procedures.Check_is_Negative_Stock_Status(con, tr) = True Then Exit Sub

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

    Private Sub cbo_Ledger_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.GotFocus
        If Common_Procedures.settings.Textile_Sizing_Delivery_receipt_Selection = 1 Then
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1417" Then '---- SAKTHIVEL IMPEX (SOMANUR)
                Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'WEAVER' or Ledger_Type = 'GODOWN'  or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or  (Ledgers_CompanyIdNo <> 0 And Ledgers_CompanyIdNo <> " & Str(Val(lbl_Company.Tag)) & ") Or Show_In_All_Entry = 1 ) and  Ledger_IdNo <> 4 and Close_status = 0 )", "(Ledger_idno = 0)")
            Else
                Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'WEAVER' or Ledger_Type = 'GODOWN'  or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or  (Ledgers_CompanyIdNo <> 0 And Ledgers_CompanyIdNo <> " & Str(Val(lbl_Company.Tag)) & ") Or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
            End If


        Else
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'WEAVER' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")

        End If



    End Sub

    Private Sub cbo_Ledger_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        If Common_Procedures.settings.Textile_Sizing_Delivery_receipt_Selection = 1 Then
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1417" Then '---- SAKTHIVEL IMPEX (SOMANUR)
                Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, Nothing, txt_Party_DcNo, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'WEAVER' or Ledger_Type = 'GODOWN' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or  (Ledgers_CompanyIdNo <> 0 And Ledgers_CompanyIdNo <> " & Str(Val(lbl_Company.Tag)) & ") Or Show_In_All_Entry = 1 ) and  Ledger_IdNo <> 4 and Close_status = 0 )", "(Ledger_idno = 0)")
            Else
                Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, Nothing, txt_Party_DcNo, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'WEAVER' or Ledger_Type = 'GODOWN' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or  (Ledgers_CompanyIdNo <> 0 And Ledgers_CompanyIdNo <> " & Str(Val(lbl_Company.Tag)) & ") Or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
            End If


        Else
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, Nothing, txt_Party_DcNo, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'WEAVER' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")

        End If
        If (e.KeyValue = 38 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
            If cbo_Type.Visible And cbo_Type.Enabled Then
                cbo_Type.Focus()
            Else
                msk_date.Focus()
            End If
        End If

    End Sub

    Private Sub cbo_Ledger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ledger.KeyPress

        If Common_Procedures.settings.Textile_Sizing_Delivery_receipt_Selection = 1 Then
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1417" Then '---- SAKTHIVEL IMPEX (SOMANUR)
                Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Ledger, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'WEAVER' or Ledger_Type = 'GODOWN'  or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or  (Ledgers_CompanyIdNo <> 0 And Ledgers_CompanyIdNo <> " & Str(Val(lbl_Company.Tag)) & ") Or Show_In_All_Entry = 1 ) and  Ledger_IdNo <> 4 and Close_status = 0 )", "(Ledger_idno = 0)")
            Else
                Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Ledger, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'WEAVER' or Ledger_Type = 'GODOWN'  or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or  (Ledgers_CompanyIdNo <> 0 And Ledgers_CompanyIdNo <> " & Str(Val(lbl_Company.Tag)) & ") Or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
            End If


        Else
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Ledger, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'WEAVER' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")

        End If
        If Asc(e.KeyChar) = 13 Then

            If Common_Procedures.settings.Textile_Sizing_Delivery_receipt_Selection = 1 And Trim(cbo_Type.Text) = "DELIVERY" Then
                If MessageBox.Show("Do you want to select DeliveryDEtails ", "FOR DELIVERY SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                    btn_Delivery_Selection_Click(sender, e)
                Else
                    txt_Party_DcNo.Focus()
                End If
            Else
                txt_Party_DcNo.Focus()
            End If

            'If MessageBox.Show("Do you want to select order:", "FOR ORDER SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
            '    btn_Selection_Click(sender, e)
            'Else
            '    txt_Party_DcNo.Focus()
            'End If
        End If

    End Sub

    Private Sub cbo_Ledger_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Ledger_Creation

            Common_Procedures.MDI_LedType = "WEAVER"
            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Ledger.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub
    Private Sub cbo_TransportName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_TransportName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")

    End Sub
    Private Sub cbo_Transportname_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_TransportName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_TransportName, cbo_Vechile, txt_Freight, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Transportname_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_TransportName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_TransportName, txt_Freight, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Transport_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_TransportName.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Common_Procedures.UR.Ledr_Wea_Siz_Rw_Trans_JbWrk_Creation = Common_Procedures.UR.Ledger_Creation
            Common_Procedures.MDI_LedType = "TRANSPORT"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_TransportName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub
    Private Sub cbo_Vechile_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Vechile.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Weaver_Yarn_Delivery_Head", "Vechile_No", "", "")

    End Sub
    Private Sub cbo_Vechile_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Vechile.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Vechile, txt_Empty_Cones, cbo_TransportName, "Weaver_Yarn_Receipt_Head", "Vechile_No", "", "")
    End Sub

    Private Sub cbo_Vechile_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Vechile.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Vechile, cbo_TransportName, "Weaver_Yarn_Receipt_Head", "Vechile_No", "", "", False)
    End Sub

    Private Sub dgv_YarnDetails_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_YarnDetails.CellEndEdit
        TotalYarnTaken_Calculation()
        If dgv_YarnDetails.CurrentRow.Cells(2).Value = "MILL" Then
            If dgv_YarnDetails.CurrentCell.ColumnIndex = 4 Or dgv_YarnDetails.CurrentCell.ColumnIndex = 5 Then
                get_MillCount_Details()
            End If
        End If
    End Sub

    Private Sub dgv_YarnDetails_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_YarnDetails.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim rect As Rectangle

        With dgv_YarnDetails
            dgv_ActiveCtrl_Name = .Name
            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If

            If Trim(.CurrentRow.Cells(2).Value) = "" Then
                .CurrentRow.Cells(2).Value = "MILL"
            End If

            If e.ColumnIndex = 1 Then

                If cbo_Grid_CountName.Visible = False Or Val(cbo_Grid_CountName.Tag) <> e.RowIndex Then

                    cbo_Grid_CountName.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Count_Name from Count_Head order by Count_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Grid_CountName.DataSource = Dt1
                    cbo_Grid_CountName.DisplayMember = "Count_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_CountName.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_Grid_CountName.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top

                    cbo_Grid_CountName.Width = rect.Width  ' .CurrentCell.Size.Width
                    cbo_Grid_CountName.Height = rect.Height  ' rect.Height
                    cbo_Grid_CountName.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_Grid_CountName.Tag = Val(e.RowIndex)
                    cbo_Grid_CountName.Visible = True

                    cbo_Grid_CountName.BringToFront()
                    cbo_Grid_CountName.Focus()


                End If


            Else

                cbo_Grid_CountName.Visible = False

            End If

            If e.ColumnIndex = 2 Then

                If cbo_Grid_YarnType.Visible = False Or Val(cbo_Grid_YarnType.Tag) <> e.RowIndex Then

                    cbo_Grid_YarnType.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Yarn_Type from YarnType_Head order by Yarn_Type", con)
                    Dt2 = New DataTable
                    Da.Fill(Dt2)
                    cbo_Grid_YarnType.DataSource = Dt2
                    cbo_Grid_YarnType.DisplayMember = "Yarn_Type"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_YarnType.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_Grid_YarnType.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                    cbo_Grid_YarnType.Width = rect.Width  ' .CurrentCell.Size.Width
                    cbo_Grid_YarnType.Height = rect.Height  ' rect.Height

                    cbo_Grid_YarnType.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_Grid_YarnType.Tag = Val(e.RowIndex)
                    cbo_Grid_YarnType.Visible = True

                    cbo_Grid_YarnType.BringToFront()
                    cbo_Grid_YarnType.Focus()

                End If

            Else

                cbo_Grid_YarnType.Visible = False

            End If

            If e.ColumnIndex = 3 Then

                If cbo_Grid_MillName.Visible = False Or Val(cbo_Grid_MillName.Tag) <> e.RowIndex Then

                    cbo_Grid_MillName.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Mill_Name from Mill_Head order by Mill_Name", con)
                    Dt3 = New DataTable
                    Da.Fill(Dt3)
                    cbo_Grid_MillName.DataSource = Dt3
                    cbo_Grid_MillName.DisplayMember = "Mill_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_MillName.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_Grid_MillName.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                    cbo_Grid_MillName.Width = rect.Width  ' .CurrentCell.Size.Width
                    cbo_Grid_MillName.Height = rect.Height  ' rect.Height

                    cbo_Grid_MillName.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_Grid_MillName.Tag = Val(e.RowIndex)
                    cbo_Grid_MillName.Visible = True

                    cbo_Grid_MillName.BringToFront()
                    cbo_Grid_MillName.Focus()

                End If

            Else

                cbo_Grid_MillName.Visible = False

            End If

        End With
    End Sub

    Private Sub dgv_YarnDetails_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_YarnDetails.CellLeave
        With dgv_YarnDetails

            If .CurrentCell.ColumnIndex = 6 Or .CurrentCell.ColumnIndex = 7 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.000")
                End If
            End If
        End With
    End Sub

    Private Sub dgv_YarnDetails_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_YarnDetails.CellValueChanged
        On Error Resume Next
        If IsNothing(dgv_YarnDetails.CurrentCell) Then Exit Sub
        With dgv_YarnDetails
            If .Visible Then
                If e.ColumnIndex = 1 Or e.ColumnIndex = 6 Then
                    If Val(Common_Procedures.settings.Weaver_YarnStock_InThiri_Status = 1) Then
                        Thiri_Calculation()
                    ElseIf Val(Common_Procedures.settings.Weaver_YarnStock_InMeter_Status = 1) Then
                        Meter_Calculation()
                    End If
                End If
                If e.ColumnIndex = 4 Or e.ColumnIndex = 5 Or e.ColumnIndex = 6 Or e.ColumnIndex = 7 Then
                    TotalYarnTaken_Calculation()
                End If
            End If
        End With
    End Sub

    Private Sub dgv_YarnDetails_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_YarnDetails.EditingControlShowing
        dgtxt_Details = CType(dgv_YarnDetails.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgv_YarnDetails_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_YarnDetails.KeyDown

        With dgv_YarnDetails

            If e.KeyCode = Keys.Up Then
                If .CurrentCell.RowIndex = 0 Then
                    .CurrentCell.Selected = False
                    cbo_TransportName.Focus()
                End If
            End If
            If e.KeyCode = Keys.Left Then
                If .CurrentCell.RowIndex = 0 And .CurrentCell.ColumnIndex <= 0 Then
                    .CurrentCell.Selected = False
                    cbo_TransportName.Focus()
                    'SendKeys.Send("{RIGHT}")
                End If
            End If

            If e.KeyCode = Keys.Enter Then
                e.SuppressKeyPress = True
                e.Handled = True

                If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then

                    If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                        save_record()
                    End If

                Else
                    SendKeys.Send("{Tab}")

                End If


            End If

        End With


    End Sub

    Private Sub dgv_YarnDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_YarnDetails.KeyUp
        Dim i As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_YarnDetails

                .Rows.RemoveAt(.CurrentRow.Index)

                For i = 0 To .Rows.Count - 1
                    .Rows(i).Cells(0).Value = i + 1
                Next

            End With
        End If

    End Sub

    Private Sub dgv_YarnDetails_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_YarnDetails.LostFocus
        On Error Resume Next
        If Not IsNothing(dgv_YarnDetails.CurrentCell) Then dgv_YarnDetails.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_YarnDetails_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_YarnDetails.RowsAdded
        Dim n As Integer
        If IsNothing(dgv_YarnDetails.CurrentCell) Then Exit Sub
        With dgv_YarnDetails
            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)
            .Rows(n - 1).Cells(2).Value = "MILL"
        End With
    End Sub
    Private Sub get_MillCount_Details()
        Dim q As Single = 0
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim Cn_bag As Single
        Dim Wgt_Bag As Single
        Dim Wgt_Cn As Single
        Dim CntID As Integer
        Dim MilID As Integer

        CntID = Common_Procedures.Count_NameToIdNo(con, dgv_YarnDetails.Rows(dgv_YarnDetails.CurrentRow.Index).Cells(1).Value)
        MilID = Common_Procedures.Mill_NameToIdNo(con, dgv_YarnDetails.Rows(dgv_YarnDetails.CurrentRow.Index).Cells(3).Value)

        Wgt_Bag = 0 : Wgt_Cn = 0 : Cn_bag = 0

        If CntID <> 0 And MilID <> 0 Then

            Da = New SqlClient.SqlDataAdapter("select * from Mill_Count_Details where mill_idno = " & Str(Val(MilID)) & " and count_idno = " & Str(Val(CntID)), con)
            Da.Fill(Dt)
            With dgv_YarnDetails

                If Dt.Rows.Count > 0 Then
                    If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                        Wgt_Bag = Dt.Rows(0).Item("Weight_Bag").ToString
                        Wgt_Cn = Dt.Rows(0).Item("Weight_Cone").ToString
                        Cn_bag = Dt.Rows(0).Item("Cones_Bag").ToString
                    End If
                End If

                Dt.Clear()
                Dt.Dispose()
                Da.Dispose()

                If .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Then
                    If .CurrentCell.ColumnIndex = 4 Then
                        If Val(Cn_bag) <> 0 Then
                            .Rows(.CurrentRow.Index).Cells(5).Value = .Rows(.CurrentRow.Index).Cells(4).Value * Val(Cn_bag)
                        End If

                        If Val(Wgt_Bag) <> 0 Then
                            .Rows(.CurrentRow.Index).Cells(6).Value = Format(Val(.Rows(.CurrentRow.Index).Cells(4).Value) * Val(Wgt_Bag), "#########0.000")
                        End If

                    End If

                    If .CurrentCell.ColumnIndex = 5 Then
                        If Val(Wgt_Cn) <> 0 Then
                            .Rows(.CurrentRow.Index).Cells(6).Value = Format(.Rows(.CurrentRow.Index).Cells(5).Value * Val(Wgt_Cn), "##########0.000")
                        End If

                    End If

                End If

            End With

        End If

    End Sub

    Private Sub TotalYarnTaken_Calculation()
        Dim Sno As Integer
        Dim TotBags As Single, TotCones As Single, TotWeight As Single, TotThiri As Single

        Sno = 0
        TotBags = 0
        TotCones = 0
        TotWeight = 0
        TotThiri = 0
        With dgv_YarnDetails
            For i = 0 To .RowCount - 1
                Sno = Sno + 1
                .Rows(i).Cells(0).Value = Sno
                If Val(.Rows(i).Cells(6).Value) <> 0 Then
                    TotBags = TotBags + Val(.Rows(i).Cells(4).Value)
                    TotCones = TotCones + Val(.Rows(i).Cells(5).Value)
                    TotWeight = TotWeight + Val(.Rows(i).Cells(6).Value)
                    If Val(Common_Procedures.settings.Weaver_YarnStock_InThiri_Status = 1) Then
                        TotThiri = TotThiri + Val(.Rows(i).Cells(7).Value)
                    End If

                End If
            Next
        End With

        With dgv_YarnDetails_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(4).Value = Val(TotBags)
            .Rows(0).Cells(5).Value = Val(TotCones)
            .Rows(0).Cells(6).Value = Format(Val(TotWeight), "########0.000")
            If Val(Common_Procedures.settings.Weaver_YarnStock_InThiri_Status = 1) Then
                .Rows(0).Cells(7).Value = Format(Val(TotThiri), "########0.000")
            End If

        End With

    End Sub
    Private Sub cbo_Grid_YarnType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_YarnType.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "YarnType_Head", "Yarn_Type", "", "Yarn_Type")

    End Sub
    Private Sub cbo_Grid_YarnType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_YarnType.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_YarnType, cbo_Grid_CountName, cbo_Grid_MillName, "YarnType_Head", "Yarn_Type", "", "Yarn_Type")

        With dgv_YarnDetails

            If (e.KeyValue = 38 And cbo_Grid_YarnType.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_Grid_YarnType.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
            End If

        End With


    End Sub

    Private Sub cbo_Grid_YarnType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_YarnType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_YarnType, cbo_Grid_MillName, "YarnType_Head", "Yarn_Type", "", "Yarn_Type")
        If Asc(e.KeyChar) = 13 Then

            With dgv_YarnDetails

                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

            End With

        End If
    End Sub
    Private Sub cbo_Grid_YarnType_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_YarnType.TextChanged
        Try
            If cbo_Grid_YarnType.Visible Then

                If IsNothing(dgv_YarnDetails.CurrentCell) Then Exit Sub



                With dgv_YarnDetails
                    If Val(cbo_Grid_YarnType.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 2 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_YarnType.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub
    Private Sub cbo_Grid_MillName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_MillName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")

    End Sub
    Private Sub cbo_Grid_MillName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_MillName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_MillName, cbo_Grid_YarnType, Nothing, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")

        With dgv_YarnDetails

            If (e.KeyValue = 38 And cbo_Grid_MillName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_Grid_MillName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
            End If

        End With



    End Sub

    Private Sub cbo_Grid_MillName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_MillName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_MillName, Nothing, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then

            With dgv_YarnDetails

                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

            End With

        End If
    End Sub

    Private Sub cbo_Grid_MillName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_MillName.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New Mill_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_MillName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub
    Private Sub cbo_Grid_MillName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_MillName.TextChanged
        Try
            If cbo_Grid_MillName.Visible Then

                If IsNothing(dgv_YarnDetails.CurrentCell) Then Exit Sub



                With dgv_YarnDetails
                    If Val(cbo_Grid_MillName.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 3 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_MillName.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub
    Private Sub cbo_Grid_CountName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_CountName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

    End Sub
    Private Sub cbo_Grid_CountName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_CountName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_CountName, Nothing, cbo_Grid_YarnType, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

        With dgv_YarnDetails
            With dgv_YarnDetails
                If (e.KeyValue = 38 And cbo_Grid_CountName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

                    If cbo_ClothSales_OrderCode_forSelection.Visible = True Then
                        cbo_ClothSales_OrderCode_forSelection.Focus()
                    ElseIf cbo_weaving_job_no.Visible = True Then
                        cbo_weaving_job_no.Focus()

                    ElseIf cbo_Cloth.Visible = True Then
                        cbo_Cloth.Focus()

                    Else

                        If .CurrentCell.RowIndex = 0 Then
                            txt_Freight.Focus()
                        Else
                            .Focus()
                            .CurrentCell = .Rows(.CurrentRow.Index - 1).Cells(.ColumnCount - 1)
                        End If

                    End If
                End If

                If (e.KeyValue = 40 And cbo_Grid_CountName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                    '.Focus()
                    '.CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                    If .CurrentCell.RowIndex = .Rows.Count - 1 And Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(1).Value) = "" Then

                        txt_rate.Focus()
                    Else
                        .Focus()
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                    End If
                End If

            End With

        End With
    End Sub

    Private Sub cbo_Grid_CountName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_CountName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_CountName, Nothing, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then

            With dgv_YarnDetails
                If .CurrentCell.RowIndex = .Rows.Count - 1 And Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(1).Value) = "" Then
                    'If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    '    save_record()
                    'Else
                    '    .Focus()
                    '    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                    'End If

                    txt_rate.Focus()
                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                End If


            End With
        End If

    End Sub

    Private Sub cbo_Grid_CountName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_CountName.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New Count_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_CountName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_Grid_CountName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_CountName.TextChanged
        Try
            If cbo_Grid_CountName.Visible Then

                If IsNothing(dgv_YarnDetails.CurrentCell) Then Exit Sub



                With dgv_YarnDetails
                    If Val(cbo_Grid_CountName.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(1).Value = Trim(cbo_Grid_CountName.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub btn_close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub btn_save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub
    Private Sub Meter_Calculation()
        Dim q As Single = 0
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim Weft_Cons As Single
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim Clo_IdNo As Integer = 0
        Dim edscnt_id As Integer = 0
        Dim cnt_id As Integer = 0
        Dim mtrs As Single = 0

        Clo_IdNo = Common_Procedures.Cloth_NameToIdNo(con, cbo_Cloth.Text)
        Weft_Cons = 0

        Da = New SqlClient.SqlDataAdapter("select * from Cloth_Head where Cloth_idno = " & Str(Val(Clo_IdNo)), con)
        Da.Fill(Dt)
        With dgv_YarnDetails

            If Dt.Rows.Count > 0 Then
                If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                    Weft_Cons = Dt.Rows(0).Item("Weight_Meter_Weft").ToString
                End If
            End If

            Dt.Clear()
            Dt.Dispose()
            Da.Dispose()

            If Val(.Rows(.CurrentRow.Index).Cells(6).Value) <> 0 Then
                .Rows(.CurrentRow.Index).Cells(7).Value = Format(Val(.Rows(.CurrentRow.Index).Cells(6).Value) / Weft_Cons, "##########0.00")
            Else
                .Rows(.CurrentRow.Index).Cells(7).Value = Val(.Rows(.CurrentRow.Index).Cells(6).Value)
            End If

        End With
    End Sub
    Private Sub Thiri_Calculation()
        Dim q As Single = 0
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim count_val As Single
        Dim CntID As Integer

        CntID = Common_Procedures.Count_NameToIdNo(con, dgv_YarnDetails.Rows(dgv_YarnDetails.CurrentRow.Index).Cells(1).Value)

        count_val = 0

        'If Common_Procedures.settings.Weaver_YarnStock_InThiri_Status = 1 Then

        Da = New SqlClient.SqlDataAdapter("select (Resultant_Count) from Count_Head where count_idno = " & Str(Val(CntID)), con)
        Da.Fill(Dt)
        With dgv_YarnDetails

            If Dt.Rows.Count > 0 Then
                If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                    count_val = Dt.Rows(0).Item("Resultant_Count").ToString
                End If
            End If

            Dt.Clear()
            Dt.Dispose()
            Da.Dispose()

            'If .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 6 Then
            ' If .CurrentCell.ColumnIndex = 4 Then
            If Val(.Rows(.CurrentRow.Index).Cells(6).Value) <> 0 Then
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1278" Then   '----SRI VALLIMURUGAN SIZING MILLS
                    .Rows(.CurrentRow.Index).Cells(7).Value = Format(count_val * 11 / 50 * .Rows(.CurrentRow.Index).Cells(6).Value, "###########0")
                Else
                    .Rows(.CurrentRow.Index).Cells(7).Value = Format(count_val * 11 / 50 * .Rows(.CurrentRow.Index).Cells(6).Value, "##########0.000")
                End If

            Else
                .Rows(.CurrentRow.Index).Cells(7).Value = ""
            End If
            'End If

            'End If

        End With

        'End If

    End Sub
    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.Enter
        dgv_ActiveCtrl_Name = dgv_YarnDetails.Name
        dgv_YarnDetails.EditingControl.BackColor = Color.Lime
        dgv_YarnDetails.EditingControl.ForeColor = Color.Blue
        dgtxt_Details.SelectAll()
    End Sub
    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Details.KeyPress
        With dgv_YarnDetails
            If .Visible Then
                If .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 6 Then

                    If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                        e.Handled = True
                    End If

                End If
            End If
        End With
    End Sub

    Private Sub txt_Empty_Beam_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Empty_Beam.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub
    Private Sub btn_Filter_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter_Close.Click
        Pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        Filter_Status = False
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
                Condt = "a.Weaver_Yarn_Receipt_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Weaver_Yarn_Receipt_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Weaver_Yarn_Receipt_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If

            If Trim(cbo_Filter_CountName.Text) <> "" Then
                Cnt_IdNo = Common_Procedures.Count_NameToIdNo(con, cbo_Filter_CountName.Text)
            End If

            If Trim(cbo_Filter_MillName.Text) <> "" Then
                Mil_IdNo = Common_Procedures.Mill_NameToIdNo(con, cbo_Filter_MillName.Text)
            End If



            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.Ledger_IdNo = " & Str(Val(Led_IdNo))
            End If

            If Val(Cnt_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Weaver_Yarn_Receipt_Code IN (select z1.Weaver_Yarn_Receipt_Code from Weaver_Yarn_Receipt_Details z1 where z1.Count_IdNo = " & Str(Val(Cnt_IdNo)) & ")"
            End If

            If Val(Mil_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Weaver_Yarn_Receipt_Code IN (select z2.Weaver_Yarn_Receipt_Code from Weaver_Yarn_Receipt_Details z2 where z2.Mill_IdNo = " & Str(Val(Mil_IdNo)) & ")"
            End If

            If Trim(cbo_Verified_Sts.Text) = "YES" Then
                Verfied_Sts = 1
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Weaver_Yarn_Receipt_Code IN ( select z2.Weaver_Yarn_Receipt_Code from Weaver_Yarn_Receipt_Head z2 where z2.Verified_Status = " & Str(Val(Verfied_Sts)) & " )"
            ElseIf Trim(cbo_Verified_Sts.Text) = "NO" Then
                Verfied_Sts = 0
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Weaver_Yarn_Receipt_Code IN ( select z2.Weaver_Yarn_Receipt_Code from Weaver_Yarn_Receipt_Head z2 where z2.Verified_Status = " & Str(Val(Verfied_Sts)) & " )"
            End If

            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name from Weaver_Yarn_Receipt_Head a INNER JOIN Ledger_Head b on a.Ledger_IdNo = b.Ledger_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_Yarn_Receipt_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Weaver_Yarn_Receipt_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Weaver_Yarn_Receipt_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Weaver_Yarn_Receipt_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = Val(dt2.Rows(i).Item("Total_Bags").ToString)
                    dgv_Filter_Details.Rows(n).Cells(5).Value = Val(dt2.Rows(i).Item("Total_Cones").ToString)
                    dgv_Filter_Details.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Total_Weight").ToString), "########0.000")

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
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( Ledger_Type = 'WEAVER' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 ) ", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, dtp_Filter_ToDate, cbo_Filter_CountName, "Ledger_AlaisHead", "Ledger_DisplayName", "( Ledger_Type = 'WEAVER' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 ) ", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, cbo_Grid_CountName, "Ledger_AlaisHead", "Ledger_DisplayName", "( Ledger_Type = 'WEAVER' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 ) ", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_CountName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_CountName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_CountName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_CountName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_CountName, cbo_Filter_PartyName, cbo_Filter_MillName, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

    End Sub

    Private Sub cbo_Filter_CountName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_CountName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_CountName, cbo_Filter_MillName, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

    End Sub

    Private Sub Open_FilterEntry()
        Dim movno As String

        On Error Resume Next

        movno = Trim(dgv_Filter_Details.CurrentRow.Cells(1).Value)

        If Val(movno) <> 0 Then
            Filter_Status = True
            move_record(movno)
            Pnl_Back.Enabled = True
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
    Private Sub cbo_Filter_MillName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_MillName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")

    End Sub
    Private Sub cbo_Filter_MillName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_MillName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_MillName, cbo_Filter_CountName, btn_Filter_Show, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")
        If e.KeyValue = 40 Or (e.Control = True And e.KeyValue = 40) Then
            If Common_Procedures.settings.CustomerCode = "1249" Then
                cbo_Verified_Sts.Focus()
            Else
                btn_Filter_Show.Focus()

            End If

        End If
    End Sub

    Private Sub cbo_Filter_MillName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_MillName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_MillName, btn_Filter_Show, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")
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
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Verified_Sts, cbo_Filter_MillName, btn_Filter_Show, "", "", "", "")
    End Sub

    Private Sub cbo_Verified_Sts_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Verified_Sts.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Verified_Sts, btn_Filter_Show, "", "", "", "")
    End Sub


    Private Sub btn_Print_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        Common_Procedures.Print_OR_Preview_Status = 1
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
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.PrintEntry, Common_Procedures.UR.Weaver_Yarn_Rceipt_Entry, New_Entry) = False Then Exit Sub

        Try

            da1 = New SqlClient.SqlDataAdapter("select * from Weaver_Yarn_Receipt_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Yarn_Receipt_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
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

        vPrnt_2Copy_In_SinglePage = Common_Procedures.settings.WeaverWagesYarnReceipt_Print_2Copy_In_SinglePage

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
                MessageBox.Show("The printing operation failed" & vbCrLf & ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End Try


        Else

            Try

                Dim ppd As New PrintPreviewDialog

                ppd.Document = PrintDocument1

                ppd.WindowState = FormWindowState.Normal
                ppd.StartPosition = FormStartPosition.CenterScreen
                ppd.ClientSize = New Size(600, 600)

                ppd.ShowDialog()

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
        prn_DetSNo = 0
        prn_PageNo = 0
        PrntCnt2ndPageSTS = False
        prn_Prev_HeadIndx = -100
        prn_HeadIndx = 0
        prn_Count = 0

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.*, e.Transport_Name  from Weaver_Yarn_Receipt_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo  Left Outer JOIN Transport_Head e ON a.Transport_IdNo = e.Transport_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_Yarn_Receipt_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("select a.*,e.*, b.Mill_name, d.Count_name  from Weaver_Yarn_Receipt_Details a INNER JOIN Mill_Head b ON a.Mill_idno = b.Mill_idno LEFT OUTER JOIN Count_Head d ON a.Count_idno = d.Count_idno  LEFT OUTER JOIN itemgroup_head e ON d.itemgroup_idno = e.itemgroup_idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_Yarn_Receipt_Code = '" & Trim(NewCode) & "' Order by a.Sl_No", con)
                prn_DetDt = New DataTable
                da2.Fill(prn_DetDt)

            Else
                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub PrintDocument1_PrintPage(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage

        If prn_HdDt.Rows.Count <= 0 Then Exit Sub

        If Val(Common_Procedures.settings.Weaver_YarnStock_InThiri_Status = 1) Then
            Printing_Format2(e)
        Else
            Printing_Format1(e)
        End If

    End Sub

    Private Sub Printing_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim pFont As Font
        Dim ps As Printing.PaperSize
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim I As Integer
        Dim NoofItems_PerPage As Integer, NoofDets As Integer
        Dim TxtHgt As Single
        Dim PpSzSTS As Boolean = False
        Dim LnAr(15) As Single, ClAr(15) As Single
        Dim EntryCode As String
        Dim CurY As Single
        Dim ItmNm1 As String, ItmNm2 As String
        Dim PCnt As Integer = 0, PrntCnt As Integer = 0
        Dim TpMargin As Single = 0
        Dim Cnt As Integer

        PrntCnt = 1

        'If Val(Common_Procedures.settings.WeaverWagesYarnReceipt_Print_2Copy_In_SinglePage) = 1 Then
        '    For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '        ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '        If ps.Width = 800 And ps.Height = 600 Then
        '            PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
        '            PrintDocument1.DefaultPageSettings.PaperSize = ps
        '            e.PageSettings.PaperSize = ps
        '            PpSzSTS = True
        '            Exit For
        '        End If
        '    Next

        '    If PrntCnt2ndPageSTS = False Then
        '        PrntCnt = 2
        '    End If

        'Else
        '    If PpSzSTS = False Then

        '        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
        '                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
        '                PrintDocument1.DefaultPageSettings.PaperSize = ps
        '                e.PageSettings.PaperSize = ps
        '                PpSzSTS = True
        '                Exit For
        '            End If
        '        Next

        '        If PpSzSTS = False Then
        '            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
        '                    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '                    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
        '                    PrintDocument1.DefaultPageSettings.PaperSize = ps
        '                    e.PageSettings.PaperSize = ps
        '                    Exit For
        '                End If
        '            Next
        '        End If

        '    End If
        'End If



        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 30
            .Right = 50
            .Top = 30
            .Bottom = 30
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

        NoofItems_PerPage = 6

        Erase LnAr
        Erase ClAr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClAr(1) = Val(45) : ClAr(2) = 120 : ClAr(3) = 250 : ClAr(4) = 70 : ClAr(5) = 65 : ClAr(6) = 90
        ClAr(7) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6))

        TxtHgt = 18.5

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_Prev_HeadIndx = prn_HeadIndx
        If Trim(Common_Procedures.settings.CustomerCode) = "1037" And vPrnt_2Copy_In_SinglePage = 1 Then
            PCnt = PCnt + 1
            PrntCnt = PrntCnt + 1
        End If
        PrntCnt2ndPageSTS = False
        TpMargin = TMargin

        For PCnt = 1 To PrntCnt

            If Val(Common_Procedures.settings.WeaverWagesYarnReceipt_Print_2Copy_In_SinglePage) = 1 Then

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

                    Printing_Format1_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TpMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr)

                    NoofDets = 0

                    NoofItems_PerPage = 5
                    If Val(Common_Procedures.settings.WeaverWagesYarnReceipt_Print_2Copy_In_SinglePage) = 1 Then
                        If prn_DetDt.Rows.Count > NoofItems_PerPage Then
                            NoofItems_PerPage = 37
                        End If
                    End If


                    CurY = CurY - 10

                    If prn_DetDt.Rows.Count > 0 Then

                        Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                            If NoofDets >= NoofItems_PerPage Then

                                CurY = CurY + TxtHgt

                                Common_Procedures.Print_To_PrintDocument(e, "Continued...", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 10, CurY, 0, 0, pFont)

                                NoofDets = NoofDets + 1

                                Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, False)

                                e.HasMorePages = True
                                Return

                            End If

                            ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Mill_Name").ToString)
                            ItmNm2 = ""
                            If Len(ItmNm1) > 35 Then
                                For I = 15 To 1 Step -1
                                    If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                                Next I
                                If I = 0 Then I = 35
                                ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                                ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                            End If

                            CurY = CurY + TxtHgt
                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Sl_No").ToString), LMargin + 15, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Yarn_Type").ToString), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Count_Name").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Cones").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Weight").ToString), " #######0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            If Trim(ItmNm2) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If

                            prn_DetIndx = prn_DetIndx + 1

                        Loop

                    End If

                    Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True)

                End If

            Catch ex As Exception
                MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End Try


            If Val(Common_Procedures.settings.WeaverWagesYarnReceipt_Print_2Copy_In_SinglePage) = 1 Then
                If PCnt = 1 And PrntCnt = 2 And PrntCnt2ndPageSTS = False Then
                    If prn_DetDt.Rows.Count = Cnt + 6 Then
                        PrntCnt2ndPageSTS = True
                        e.HasMorePages = True
                        Cnt = 6
                        Return
                    End If
                End If
            End If

            PrntCnt2ndPageSTS = False

        Next PCnt

LOOP2:

        prn_Count = prn_Count + 1

        If Val(prn_TotCopies) > 1 Then

            If prn_Count < Val(prn_TotCopies) Then

                prn_DetIndx = 0
                prn_DetSNo = 0
                prn_PageNo = 0

                e.HasMorePages = True
                Return

            End If

        Else

            prn_HeadIndx = prn_HeadIndx + 1
            If prn_HeadIndx <= prn_HdDt.Rows.Count - 1 Then
                e.HasMorePages = True

            Else
                e.HasMorePages = False

            End If

            prn_DetDt.Clear()
            prn_PageNo = 0

            prn_DetIndx = 0
            prn_DetSNo = 0


        End If

    End Sub

    Private Sub Printing_Format1_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
        Dim strHeight As Single
        Dim C1 As Single
        Dim W1 As Single
        Dim S1 As Single

        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Mill_name, d.Count_name  from Weaver_Yarn_Receipt_Details a INNER JOIN Mill_Head b ON a.Mill_idno = b.Mill_idno LEFT OUTER JOIN Count_Head d ON a.Count_idno = d.Count_idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_Yarn_Receipt_Code = '" & Trim(EntryCode) & "' Order by a.Sl_No", con)
        da2.Fill(dt2)

        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        dt2.Clear()

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

        CurY = CurY + strHeight - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt - 5
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "YARN RECEIPT", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        C1 = ClAr(1) + ClAr(2) + ClAr(3)
        W1 = e.Graphics.MeasureString("DATE  : ", pFont).Width
        S1 = e.Graphics.MeasureString("TO  :  ", pFont).Width


        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "TO  :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_MainName").ToString, LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "REC.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Weaver_Yarn_Receipt_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        p1Font = New Font("Calibri", 14, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Weaver_Yarn_Receipt_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

        If Trim(prn_HdDt.Rows(0).Item("EwayBill_No").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "E-Way Bill No", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 50, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, (prn_HdDt.Rows(0).Item("EwayBill_No").ToString), LMargin + C1 + W1 + 60, CurY, 0, 0, pFont)
        End If

        If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "GSTIN", LMargin + S1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + S1 + W1 + 30, CurY, 0, 0, pFont)
            If Trim(prn_HdDt.Rows(0).Item("Vechile_No").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "Vehicle No ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ": ", LMargin + C1 + W1 + 50, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vechile_No").ToString, LMargin + C1 + W1 + 60, CurY, 0, 0, pFont)
            End If
        Else

            CurY = CurY + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("Pan_No").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "PAN", LMargin + S1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Pan_No").ToString, LMargin + S1 + W1 + 30, CurY, 0, 0, pFont)
            End If
            If Trim(prn_HdDt.Rows(0).Item("Vechile_No").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "Vehicle No ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ": ", LMargin + C1 + W1 + 50, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vechile_No").ToString, LMargin + C1 + W1 + 60, CurY, 0, 0, pFont)
            End If
        End If


        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))

        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "YARN TYPE", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "MILL NAME", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "COUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "BAG", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "CONE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)


        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY


    End Sub

    Private Sub Printing_Format1_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim i As Integer
        Dim Cmp_Name As String
        Dim p1Font As Font
        Dim W1, W2 As Single
        Dim C1 As Single
        Dim C2 As Single
        Dim vTxPerc As String = 0
        Dim vTxamt As String = 0
        Dim vNtAMt As String = 0
        Dim vSgst_amt As String = 0
        Dim vCgst_amt As String = 0
        Dim vIgst_amt As String = 0

        C1 = ClAr(1) + ClAr(2) + ClAr(3)
        C2 = ClAr(1) + ClAr(2)

        For i = NoofDets + 1 To NoofItems_PerPage
            CurY = CurY + TxtHgt
        Next

        W1 = e.Graphics.MeasureString("Empty Gunnies  :", pFont).Width
        W2 = e.Graphics.MeasureString("Empty Cones  :", pFont).Width

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(5) = CurY

        CurY = CurY + TxtHgt - 10
        If is_LastPage = True Then
            Common_Procedures.Print_To_PrintDocument(e, " TOTAL", LMargin + ClAr(1) + 30, CurY, 2, ClAr(4), pFont)

            Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Bags").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Cones").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString), " #######0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)

        End If

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(6) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))


        '----

        vTxPerc = 0
        vCgst_amt = 0
        vSgst_amt = 0
        vIgst_amt = 0
        If Val(prn_HdDt.Rows(0).Item("GST_Tax_Invoice_Status").ToString) = 1 Then
            If Val(prn_HdDt.Rows(0).Item("Company_State_IdNo").ToString) = Val(prn_HdDt.Rows(0).Item("Ledger_State_IdNo").ToString) Then
                vTxPerc = Format(Val(prn_DetDt.Rows(0).Item("item_gst_percentage").ToString) / 2, "############0.00")
                vCgst_amt = Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString) * vTxPerc / 100, "############0.00")
                vSgst_amt = Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString) * vTxPerc / 100, "############0.00")

            Else

                vTxPerc = prn_DetDt.Rows(0).Item("item_gst_percentage").ToString
                vIgst_amt = Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString) * Val(vTxPerc) / 100, "############0.00")

            End If
        End If
        '---


        CurY = CurY + 10
        If Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString) <> 0 Then
            Common_Procedures.Print_To_PrintDocument(e, "Empty Beam  ", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " : ", LMargin + W2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString), LMargin + W2 + 30, CurY, 0, 0, pFont)
        End If

        If Val(prn_HdDt.Rows(0).Item("Empty_Gunnies").ToString) <> 0 Then
            Common_Procedures.Print_To_PrintDocument(e, "Empty Gunnies  ", LMargin + C2 + 20, CurY, 2, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " : ", LMargin + C2 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Empty_Gunnies").ToString), LMargin + W1 + C2 + 30, CurY, 0, 0, pFont)
        End If

        If Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString) <> 0 And Val(prn_HdDt.Rows(0).Item("GST_Tax_Invoice_Status").ToString) = 1 Then
            Common_Procedures.Print_To_PrintDocument(e, "Taxable Value", LMargin + C1 - 40, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " : ", LMargin + C1 + W1 - 55, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Net_Amount").ToString, LMargin + W1 + C1 - 40, CurY, 0, 0, pFont)
        End If

        If Val(vCgst_amt) <> 0 Then
            Common_Procedures.Print_To_PrintDocument(e, "CGST @ " & Val(vTxPerc) & " %   : ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 10, CurY, 2, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + W1, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, vCgst_amt, PageWidth - 10, CurY, 1, 0, pFont)
        ElseIf Val(vIgst_amt) <> 0 Then
            Common_Procedures.Print_To_PrintDocument(e, "IGST @ " & Val(vTxPerc) & " %  : ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 25, CurY, 2, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + W1, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, vIgst_amt, PageWidth - 10, CurY, 1, 0, pFont)
        End If

        CurY = CurY + TxtHgt + 5

        If Val(prn_HdDt.Rows(0).Item("Empty_Cones").ToString) <> 0 Then
            Common_Procedures.Print_To_PrintDocument(e, "Empty Cones ", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " : ", LMargin + W2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Empty_Cones").ToString), LMargin + W2 + 30, CurY, 0, 0, pFont)
        End If

        'Common_Procedures.Print_To_PrintDocument(e, "Vehicle No  ", LMargin + C1 + 20, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, " : ", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vechile_No").ToString, LMargin + W1 + C1 + 30, CurY, 0, 0, pFont)

        If Val(vSgst_amt) <> 0 Then
            Common_Procedures.Print_To_PrintDocument(e, "SGST @ " & Val(vTxPerc) & " % ", LMargin + C1 - 40, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " : ", LMargin + C1 + W1 - 55, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, vSgst_amt, LMargin + W1 + C1 - 40, CurY, 0, 0, pFont)
        End If

        vNtAMt = Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString) + Val(vCgst_amt) + Val(vSgst_amt) + Val(vIgst_amt), "#############0")
        If Val(vNtAMt) <> 0 Then
            Common_Procedures.Print_To_PrintDocument(e, "Value of Goods  : ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 5, CurY, 2, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + W1, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(vNtAMt), "#############0.00"), PageWidth - 10, CurY, 1, 0, pFont)
        End If

        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        p1Font = New Font("Calibri", 12, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 20, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

        CurY = CurY + TxtHgt + 10

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

    End Sub

    Private Sub Printing_Format2(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim pFont As Font
        Dim ps As Printing.PaperSize
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim I As Integer
        Dim NoofItems_PerPage As Integer, NoofDets As Integer
        Dim TxtHgt As Single
        Dim PpSzSTS As Boolean = False
        Dim LnAr(15) As Single, ClAr(15) As Single
        Dim EntryCode As String
        Dim CurY As Single
        Dim ItmNm1 As String, ItmNm2 As String

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
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
            .Left = 30
            .Right = 30
            .Top = 30
            .Bottom = 30
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

        NoofItems_PerPage = 6

        Erase LnAr
        Erase ClAr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClAr(1) = Val(40) : ClAr(2) = 100 : ClAr(3) = 210 : ClAr(4) = 75 : ClAr(5) = 75 : ClAr(6) = 75 : ClAr(7) = 90
        ClAr(8) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7))

        TxtHgt = 18.5

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try
            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format2_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr)

                NoofDets = 0

                CurY = CurY - 10

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then

                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued...", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 10, CurY, 0, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, False)

                            e.HasMorePages = True
                            Return

                        End If

                        ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Mill_Name").ToString)
                        ItmNm2 = ""
                        If Len(ItmNm1) > 35 Then
                            For I = 15 To 1 Step -1
                                If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 35
                            ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                            ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                        End If

                        CurY = CurY + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Sl_No").ToString), LMargin + 15, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Yarn_Type").ToString), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Count_Name").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Cones").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Weight").ToString), " #######0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Thiri").ToString), " #######0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)

                        NoofDets = NoofDets + 1

                        If Trim(ItmNm2) <> "" Then
                            CurY = CurY + TxtHgt - 5
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                            NoofDets = NoofDets + 1
                        End If

                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If

                Printing_Format2_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format2_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
        Dim strHeight As Single
        Dim C1 As Single
        Dim W1 As Single
        Dim S1 As Single

        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Mill_name, d.Count_name  from Weaver_Yarn_Delivery_Details a INNER JOIN Mill_Head b ON a.Mill_idno = b.Mill_idno LEFT OUTER JOIN Count_Head d ON a.Count_idno = d.Count_idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_Yarn_Delivery_Code = '" & Trim(EntryCode) & "' Order by a.Sl_No", con)
        da2.Fill(dt2)

        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        dt2.Clear()

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

        CurY = CurY + strHeight - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt - 5
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "YARN RECEIPT", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        C1 = ClAr(1) + ClAr(2) + ClAr(3)
        W1 = e.Graphics.MeasureString("REC DATE  : ", pFont).Width
        S1 = e.Graphics.MeasureString("TO  :  ", pFont).Width


        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "TO  :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_MainName").ToString, LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "REC.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Weaver_Yarn_Receipt_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        p1Font = New Font("Calibri", 14, FontStyle.Bold)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Weaver_Yarn_Receipt_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))

        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "YARN TYPE", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "MILL NAME", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "COUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "BAG", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "CONE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "THIRI", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY


    End Sub

    Private Sub Printing_Format2_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim i As Integer
        Dim Cmp_Name As String
        Dim p1Font As Font
        Dim W1 As Single

        For i = NoofDets + 1 To NoofItems_PerPage
            CurY = CurY + TxtHgt
        Next

        W1 = e.Graphics.MeasureString(" Vehicle No :", pFont).Width

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(5) = CurY

        CurY = CurY + TxtHgt - 10
        If is_LastPage = True Then
            Common_Procedures.Print_To_PrintDocument(e, " TOTAL", LMargin + ClAr(1) + 30, CurY, 2, ClAr(4), pFont)

            Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Bags").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Cones").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString), " #######0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Thiri").ToString), " #######0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)

        End If

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(6) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(3))

        CurY = CurY + 10

        Common_Procedures.Print_To_PrintDocument(e, " Vehicle No : ", LMargin + 20, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vechile_No").ToString, LMargin + W1 + 30, CurY, 0, 0, pFont)
        If Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString) <> 0 Then
            Common_Procedures.Print_To_PrintDocument(e, "Empty Beam : " & Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString), PageWidth - 20, CurY, 1, 0, pFont)
        End If

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        p1Font = New Font("Calibri", 12, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 20, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

        CurY = CurY + TxtHgt + 10

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

    End Sub
    Private Sub txt_Empty_Cones_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Empty_Cones.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_Empty_Gunnies_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Empty_Gunnies.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_Freight_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Freight.KeyDown
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If (e.KeyValue = 40) Then

            If cbo_weaving_job_no.Enabled And cbo_weaving_job_no.Visible = True Then
                cbo_weaving_job_no.Focus()
            ElseIf cbo_ClothSales_OrderCode_forSelection.Visible = True Then
                cbo_ClothSales_OrderCode_forSelection.Focus()
            Else
                If dgv_YarnDetails.Rows.Count > 0 Then
                    dgv_YarnDetails.Focus()
                    dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(1)
                Else
                    btn_save.Focus()
                End If
            End If
        End If
    End Sub

    Private Sub txt_Freight_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Freight.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
        If Asc(e.KeyChar) = 13 Then

            If cbo_weaving_job_no.Enabled And cbo_weaving_job_no.Visible = True Then
                cbo_weaving_job_no.Focus()


            ElseIf cbo_ClothSales_OrderCode_forSelection.Visible = True Then
                cbo_ClothSales_OrderCode_forSelection.Focus()

            Else


                If dgv_YarnDetails.Rows.Count > 0 Then
                    dgv_YarnDetails.Focus()
                    dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(1)

                Else
                    btn_save.Focus()

                End If
            End If
        End If
    End Sub
    Private Sub dgtxt_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyUp
        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            dgv_YarnDetails_KeyUp(sender, e)
        End If
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
            ElseIf cbo_Ledger.Visible = True Then
                cbo_Ledger.Focus
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

    Private Sub cbo_Cloth_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Cloth.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "cloth_name", "", "(cloth_name)")

    End Sub

    Private Sub cbo_cloth_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Cloth.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Cloth, txt_Freight, Nothing, "Cloth_Head", "cloth_name", "", "(cloth_name)")
        If (e.KeyValue = 40 And cbo_Cloth.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If cbo_ClothSales_OrderCode_forSelection.Visible = True Then
                cbo_ClothSales_OrderCode_forSelection.Focus()
            Else
                dgv_YarnDetails.Focus()
                dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(1)
                dgv_YarnDetails.CurrentCell.Selected = True
            End If
        End If
    End Sub

    Private Sub cbo_cloth_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Cloth.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Cloth, Nothing, "Cloth_Head", "cloth_name", "", "(cloth_name)")

        If Asc(e.KeyChar) = 13 Then

            If cbo_ClothSales_OrderCode_forSelection.Visible = True Then
                cbo_ClothSales_OrderCode_forSelection.Focus()
            Else
                dgv_YarnDetails.Focus()
                dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(1)
                dgv_YarnDetails.CurrentCell.Selected = True
            End If



        End If

    End Sub

    Private Sub btn_Selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_OwnOrderSelection.Click
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim i As Integer, j As Integer, n As Integer, SNo As Integer
        Dim LedIdNo As Integer
        Dim NewCode As String
        Dim Ent_Rate As Single = 0
        Dim Ent_Wgt As Single = 0
        Dim Ent_Pcs As Single = 0
        Dim NR As Single = 0

        LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)

        'If LedIdNo = 0 Then
        '    MessageBox.Show("Invalid Party Name", "DOES NOT SELECT PAVU...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    If cbo_PartyName.Enabled And cbo_PartyName.Visible Then cbo_PartyName.Focus()
        '    Exit Sub
        'End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        With dgv_Selection
            If Val(LedIdNo) <> 0 Then

                .Rows.Clear()
                SNo = 0

                Da = New SqlClient.SqlDataAdapter("select a.*, b.Cloth_Name,c.*  from Own_Order_Head a INNER JOIN CLOTH_Head b ON  b.Cloth_IdNo = a.Cloth_Idno  INNER JOIN Own_Order_Weaving_Details c ON  c.Own_Order_Code = a.Own_Order_Code LEFT OUTER JOIN Weaver_Yarn_Receipt_Head d ON d.Own_Order_Code = a.Own_Order_Code  where a.Weaver_Yarn_Receipt_Code = '" & Trim(NewCode) & "'   and c.Ledger_IdNo = " & Val(LedIdNo) & " order by a.Own_Order_Date, a.for_orderby, a.Own_Order_No", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    For i = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()

                        Ent_Rate = 0


                        SNo = SNo + 1
                        .Rows(n).Cells(0).Value = Val(SNo)

                        .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Own_Order_No").ToString
                        .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Own_Order_Date").ToString), "dd-MM-yyyy")
                        .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Order_No").ToString


                        .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Cloth_Name").ToString

                        .Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(i).Item("Order_Meters").ToString), "#########0.000")
                        .Rows(n).Cells(6).Value = "1"
                        .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Own_Order_Code").ToString

                        For j = 0 To .ColumnCount - 1
                            .Rows(i).Cells(j).Style.ForeColor = Color.Red
                        Next

                    Next

                End If
                Dt1.Clear()

                Da = New SqlClient.SqlDataAdapter("select a.*, b.Cloth_Name from Own_Order_Head a INNER JOIN Cloth_Head b ON  b.Cloth_Idno = a.Cloth_Idno INNER JOIN Own_Order_Weaving_Details c ON  c.Own_Order_Code = a.Own_Order_Code   LEFT OUTER JOIN Weaver_Yarn_Receipt_Head d ON d.Weaver_Yarn_Receipt_Code = a.Own_Order_Code    where a.Weaver_Yarn_Receipt_Code = ''  and c.Ledger_IdNo = " & Val(LedIdNo) & " order by a.Own_Order_Date, a.for_orderby, a.Own_Order_No", con)
                Dt1 = New DataTable
                NR = Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    For i = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()

                        SNo = SNo + 1
                        .Rows(n).Cells(0).Value = Val(SNo)
                        .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("OWn_Order_No").ToString
                        .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Own_Order_Date").ToString), "dd-MM-yyyy")
                        .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Order_No").ToString


                        .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Cloth_Name").ToString

                        .Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(i).Item("Order_Meters").ToString), "#########0.000")
                        .Rows(n).Cells(6).Value = ""
                        .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Own_Order_Code").ToString

                    Next

                End If
                Dt1.Clear()
            Else
                .Rows.Clear()
                SNo = 0

                Da = New SqlClient.SqlDataAdapter("select a.*, b.Cloth_Name  from Own_Order_Head a INNER JOIN CLOTH_Head b ON  b.Cloth_IdNo = a.Cloth_Idno   LEFT OUTER JOIN Weaver_Yarn_Receipt_Head d ON d.Own_Order_Code = a.Own_Order_Code  where a.Weaver_Yarn_Receipt_Code = '" & Trim(NewCode) & "'   order by a.Own_Order_Date, a.for_orderby, a.Own_Order_No", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    For i = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()

                        Ent_Rate = 0


                        SNo = SNo + 1
                        .Rows(n).Cells(0).Value = Val(SNo)

                        .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Own_Order_No").ToString
                        .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Own_Order_Date").ToString), "dd-MM-yyyy")
                        .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Order_No").ToString

                        .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Cloth_Name").ToString

                        .Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(i).Item("Order_Meters").ToString), "#########0.000")
                        .Rows(n).Cells(6).Value = "1"
                        .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Own_Order_Code").ToString

                        For j = 0 To .ColumnCount - 1
                            .Rows(i).Cells(j).Style.ForeColor = Color.Red
                        Next

                    Next

                End If
                Dt1.Clear()

                Da = New SqlClient.SqlDataAdapter("select a.*, b.Cloth_Name from Own_Order_Head a INNER JOIN Cloth_Head b ON  b.Cloth_Idno = a.Cloth_Idno    LEFT OUTER JOIN Weaver_Yarn_Receipt_Head d ON d.Weaver_Yarn_Receipt_Code = a.Own_Order_Code    where a.Weaver_Yarn_Receipt_Code = ''   order by a.Own_Order_Date, a.for_orderby, a.Own_Order_No", con)
                Dt1 = New DataTable
                NR = Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    For i = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()

                        SNo = SNo + 1
                        .Rows(n).Cells(0).Value = Val(SNo)
                        .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Own_Order_No").ToString
                        .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Own_Order_Date").ToString), "dd-MM-yyyy")
                        .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Order_No").ToString

                        .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Cloth_Name").ToString

                        .Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(i).Item("Order_Meters").ToString), "#########0.000")
                        .Rows(n).Cells(6).Value = ""
                        .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Own_Order_Code").ToString

                    Next

                End If
                Dt1.Clear()
            End If
        End With

        pnl_Selection.Visible = True
        Pnl_Back.Enabled = False
        dgv_Selection.Focus()

    End Sub

    Private Sub dgv_Selection_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Selection.CellClick
        Select_Piece(e.RowIndex)
    End Sub

    Private Sub Select_Piece(ByVal RwIndx As Integer)
        Dim i As Integer

        With dgv_Selection

            If .RowCount > 0 And RwIndx >= 0 Then

                .Rows(RwIndx).Cells(6).Value = (Val(.Rows(RwIndx).Cells(6).Value) + 1) Mod 2

                If Val(.Rows(RwIndx).Cells(6).Value) = 1 Then

                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
                    Next

                Else
                    .Rows(RwIndx).Cells(6).Value = ""

                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Black
                    Next

                End If

            End If

        End With


    End Sub

    Private Sub dgv_Selection_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Selection.KeyDown
        Dim n As Integer

        On Error Resume Next

        If IsNothing(dgv_Selection.CurrentCell) Then Exit Sub

        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Space Then
            If dgv_Selection.CurrentCell.RowIndex >= 0 Then
                If IsNothing(dgv_Selection.CurrentCell) Then Exit Sub
                n = dgv_Selection.CurrentCell.RowIndex

                Select_Piece(n)

                e.Handled = True

            End If
        End If

    End Sub

    Private Sub btn_Close_Selection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Selection.Click
        Close_Receipt_Selection()
    End Sub

    Private Sub Close_Receipt_Selection()
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim n As Integer = 0
        Dim sno As Integer = 0
        Dim i As Integer = 0
        Dim j As Integer = 0

        lbl_OrderNo.Text = ""
        lbl_OrderCode.Text = ""

        For i = 0 To dgv_Selection.RowCount - 1

            If Val(dgv_Selection.Rows(i).Cells(6).Value) = 1 Then

                ' lbl_RecCode.Text = dgv_Selection.Rows(i).Cells(8).Value

                lbl_OrderNo.Text = dgv_Selection.Rows(i).Cells(3).Value
                lbl_OrderCode.Text = dgv_Selection.Rows(i).Cells(7).Value

            End If

        Next

        Pnl_Back.Enabled = True
        pnl_Selection.Visible = False

        If txt_Party_DcNo.Enabled And txt_Party_DcNo.Visible Then txt_Party_DcNo.Focus()

    End Sub
    Private Sub cbo_DeliveryTo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_DeliveryTo.GotFocus
        If Common_Procedures.settings.Textile_Sizing_Delivery_receipt_Selection = 1 Then
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1417" Then '---- SAKTHIVEL IMPEX (SOMANUR)
                Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'WEAVER' or Ledger_Type = 'SIZING' or Ledger_Type = 'GODOWN' or Ledger_Type = 'JOBWORKER' or Ledger_Type = 'REWINDING' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or  (Ledgers_CompanyIdNo <> 0 and Ledgers_CompanyIdNo <> " & Str(Val(lbl_Company.Tag)) & ") or Show_In_All_Entry = 1 ) and  Ledger_IdNo <> 4 and Close_status = 0 )", "(Ledger_idno = 0)")
            Else
                Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'WEAVER' or Ledger_Type = 'SIZING' or Ledger_Type = 'GODOWN' or Ledger_Type = 'JOBWORKER' or Ledger_Type = 'REWINDING' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or  (Ledgers_CompanyIdNo <> 0 and Ledgers_CompanyIdNo <> " & Str(Val(lbl_Company.Tag)) & ") or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
            End If
        Else
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'WEAVER' or Ledger_Type = 'SIZING' or Ledger_Type = 'GODOWN' or Ledger_Type = 'JOBWORKER' or Ledger_Type = 'REWINDING' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
        End If

    End Sub

    Private Sub cbo_DeliveryTo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DeliveryTo.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        If Common_Procedures.settings.Textile_Sizing_Delivery_receipt_Selection = 1 Then
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1417" Then '---- SAKTHIVEL IMPEX (SOMANUR)
                Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_DeliveryTo, txt_Party_DcNo, txt_Empty_Beam, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'WEAVER' or Ledger_Type = 'SIZING' or Ledger_Type = 'GODOWN' or Ledger_Type = 'JOBWORKER' or Ledger_Type = 'REWINDING' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or  (Ledgers_CompanyIdNo <> 0 and Ledgers_CompanyIdNo <> " & Str(Val(lbl_Company.Tag)) & ") or Show_In_All_Entry = 1 ) and  Ledger_IdNo <> 4 and Close_status = 0 )", "(Ledger_idno = 0)")
            Else
                Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_DeliveryTo, txt_Party_DcNo, txt_Empty_Beam, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'WEAVER' or Ledger_Type = 'SIZING' or Ledger_Type = 'GODOWN' or Ledger_Type = 'JOBWORKER' or Ledger_Type = 'REWINDING' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or  (Ledgers_CompanyIdNo <> 0 and Ledgers_CompanyIdNo <> " & Str(Val(lbl_Company.Tag)) & ") or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
            End If

        Else
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_DeliveryTo, txt_Party_DcNo, txt_Empty_Beam, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'WEAVER' or Ledger_Type = 'SIZING' or Ledger_Type = 'GODOWN' or Ledger_Type = 'JOBWORKER' or Ledger_Type = 'REWINDING' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
        End If

    End Sub

    Private Sub cbo_DeliveryTo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_DeliveryTo.KeyPress
        If Common_Procedures.settings.Textile_Sizing_Delivery_receipt_Selection = 1 Then
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1417" Then '---- SAKTHIVEL IMPEX (SOMANUR)
                Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_DeliveryTo, txt_Empty_Beam, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'WEAVER' or Ledger_Type = 'SIZING' or Ledger_Type = 'GODOWN' or Ledger_Type = 'JOBWORKER' or Ledger_Type = 'REWINDING' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or  (Ledgers_CompanyIdNo <> 0 and Ledgers_CompanyIdNo <> " & Str(Val(lbl_Company.Tag)) & ") or Show_In_All_Entry = 1 ) and  Ledger_IdNo <> 4 and Close_status = 0 )", "(Ledger_idno = 0)")
            Else
                Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_DeliveryTo, txt_Empty_Beam, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'WEAVER' or Ledger_Type = 'SIZING' or Ledger_Type = 'GODOWN' or Ledger_Type = 'JOBWORKER' or Ledger_Type = 'REWINDING' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or  (Ledgers_CompanyIdNo <> 0 and Ledgers_CompanyIdNo <> " & Str(Val(lbl_Company.Tag)) & ") or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
            End If

        Else
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_DeliveryTo, txt_Empty_Beam, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'WEAVER' or Ledger_Type = 'SIZING' or Ledger_Type = 'GODOWN' or Ledger_Type = 'JOBWORKER' or Ledger_Type = 'REWINDING' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
        End If

    End Sub

    Private Sub cbo_DeliveryTo_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DeliveryTo.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Common_Procedures.MDI_LedType = "WEAVER"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_DeliveryTo.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

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

            Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)

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
            If dgv_YarnDetails_Total.RowCount > 0 Then
                smstxt = smstxt & " WEIGHT: " & Val((dgv_YarnDetails.Rows(0).Cells(6).Value())) & vbCrLf


            End If

            If dgv_YarnDetails.RowCount > 0 Then
                ' smstxt = smstxt & " Beam No: " & Trim((dgv_PavuDetails.Rows(0).Cells(3).Value())) & vbCrLf
                '  smstxt = smstxt & "Ends Count : " & Trim((dgv_YarnDetails.Rows(0).Cells(3).Value())) & vbCrLf

                smstxt = smstxt & " BAGS  : " & Val(dgv_YarnDetails.Rows(0).Cells(4).Value()) & vbCrLf
                smstxt = smstxt & " MILL NAME  : " & Val(dgv_YarnDetails.Rows(0).Cells(3).Value()) & vbCrLf
                smstxt = smstxt & " COUNT: " & Val((dgv_YarnDetails.Rows(0).Cells(1).Value())) & vbCrLf


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
        Dim CMD As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Da2 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim i As Integer, j As Integer, n As Integer, SNo As Integer, Ledger_Party_idno As Integer = 0
        Dim Led_IdNo As Integer
        Dim NewCode As String = ""
        Dim CompIDCondt As String = ""
        Dim RcptBm_PavuInc As Integer = 0
        Dim vjoinTYP As String = ""

        If Trim(UCase(cbo_Type.Text)) <> "DELIVERY" Then Exit Sub

        CMD.Connection = con

        Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
        If Led_IdNo = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SELECT YARN DELIVERY...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
            Exit Sub
        End If

        CompIDCondt = "(a.Selection_CompanyIdno = " & Str(Val(lbl_Company.Tag)) & ")"

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Trim(UCase(cbo_Type.Text)) = "DELIVERY" Then

            With dgv_delivery_Selections

                .Rows.Clear()
                n = .Rows.Add()
                SNo = 0

                For i = 1 To 2

                    If i = 1 Then
                        '---editing
                        Da2 = New SqlClient.SqlDataAdapter("Select a.Delivery_Code, a.delivery_No, a.for_OrderBy, a.Reference_Date, a.Total_Bags as Bags , a.Total_Cones as Cones, a.Total_Weight as Weight , Dh.Ledger_Name as DeliveryTo from Yarn_Delivery_Selections_Processing_Details a INNER JOIN Ledger_Head Dh ON Dh.Ledger_IDno = a.DeliveryTo_IDno where ( a.Selection_ReceivedFromIdNo = " & Str(Val(Led_IdNo)) & " OR a.Selection_ledgerIdno = " & Str(Val(Led_IdNo)) & " ) and " & CompIDCondt & " and a.Delivery_Code = a.reference_code and a.Total_Weight > 0 and a.Delivery_Code IN (Select sq1.Delivery_Code from Yarn_Delivery_Selections_Processing_Details sq1 where sq1.reference_code = '" & Trim(NewCode) & "' )  ", con)
                        'Da2 = New SqlClient.SqlDataAdapter("Select a.Delivery_Code, a.delivery_No,  a.Total_Bags as Bags , a.Total_Cones as Cones, a.Total_Weight as Weight from Yarn_Delivery_Selections_Processing_Details a where   a.Selection_ledgerIdno =" & Str(Val(Led_IdNo)) & " and " & CompIDCondt & " and a.Delivery_Code = a.reference_code and a.Total_bags > 0 and a.Total_Weight > 0 and a.Delivery_Code IN (Select sq1.Delivery_Code from Yarn_Delivery_Selections_Processing_Details sq1 where sq1.reference_code = '" & Trim(NewCode) & "' )  ", con)
                    Else

                        CMD.CommandText = "truncate table " & Trim(Common_Procedures.EntryTempTable) & ""
                        CMD.ExecuteNonQuery()

                        Common_Procedures.get_YarnDelivery_Selection_Processing_Pending(con)

                        'new entry
                        Da2 = New SqlClient.SqlDataAdapter("Select a.Delivery_Code, a.delivery_No, a.for_OrderBy, a.Reference_Date, tET.meters1 as Bags , tET.int2 as Cones, tET.weight3 as Weight , Dh.Ledger_Name as DeliveryTo from Yarn_Delivery_Selections_Processing_Details a INNER JOIN " & Trim(Common_Procedures.EntryTempTable) & " tET ON tET.Name1 <> '' and tET.weight3 <> 0 and a.Delivery_Code = tET.Name1 INNER JOIN Ledger_Head Dh ON Dh.Ledger_IDno = a.DeliveryTo_IDno where a.Selection_CompanyIdno = " & Str(Val(lbl_Company.Tag)) & " and (a.Selection_ReceivedFromIdNo = " & Str(Val(Led_IdNo)) & " OR a.Selection_ledgerIdno = " & Str(Val(Led_IdNo)) & " ) and a.Total_Weight > 0 Order by a.Reference_Date DESC, a.for_OrderBy DESC, a.Delivery_Code DESC, a.Delivery_No DESC", con)
                        'Da2 = New SqlClient.SqlDataAdapter("Select a.Delivery_Code, a.delivery_No, SUM(a.Total_Bags) as Bags , SUM(a.Total_Cones) as 'Cones, SUM(a.Total_Weight) as Weight from Yarn_Delivery_Selections_Processing_Details a where ( a.Selection_ReceivedFromIdNo = " & Str(Val(Led_IdNo)) & " OR a.Selection_ledgerIdno = " & Str(Val(Led_IdNo)) & " ) and " & CompIDCondt & " and a.Delivery_Code NOT IN (Select sq1.Delivery_Code from Yarn_Delivery_Selections_Processing_Details sq1 where sq1.reference_code = '" & Trim(NewCode) & "' ) Group by a.Delivery_Code, a.Delivery_No Having sum(a.Total_Weight) > 0 Order by a.Delivery_Code DESC, a.Delivery_No DESC", con)
                        'Da2 = New SqlClient.SqlDataAdapter("Select a.Delivery_Code, a.delivery_No,  SUM(a.Total_Bags) as Bags , SUM(a.Total_Cones) as Cones, SUM(a.Total_Weight) as Weight from Yarn_Delivery_Selections_Processing_Details a where   a.Selection_ledgerIdno =" & Str(Val(Led_IdNo)) & " and " & CompIDCondt & " and a.Delivery_Code NOT IN (Select sq1.Delivery_Code from Yarn_Delivery_Selections_Processing_Details sq1 where sq1.reference_code = '" & Trim(NewCode) & "' ) Group by a.Delivery_Code, a.Delivery_No Having Sum(a.Total_bags) > 0  and  sum(a.Total_Weight) > 0 ", con)
                    End If
                    Dt2 = New DataTable
                    Da2.Fill(Dt2)

                    If Dt2.Rows.Count > 0 Then

                        For k = 0 To Dt2.Rows.Count - 1

                            If Val(Dt2.Rows(k).Item("Weight").ToString) > 0 Then

                                SNo = SNo + 1
                                n = .Rows.Add()

                                .Rows(n).Cells(0).Value = Val(SNo)
                                .Rows(n).Cells(1).Value = Dt2.Rows(k).Item("Delivery_No").ToString
                                .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt2.Rows(k).Item("Reference_Date").ToString), "dd-MM-yyyy")
                                .Rows(n).Cells(3).Value = Dt2.Rows(k).Item("Delivery_No").ToString
                                .Rows(n).Cells(4).Value = Dt2.Rows(k).Item("Bags").ToString
                                .Rows(n).Cells(5).Value = Dt2.Rows(k).Item("Cones").ToString
                                .Rows(n).Cells(6).Value = Dt2.Rows(k).Item("Weight").ToString
                                .Rows(n).Cells(8).Value = Trim(Dt2.Rows(k).Item("Delivery_Code").ToString)

                                If i = 1 Then

                                    .Rows(n).Cells(7).Value = 1
                                    For j = 0 To .ColumnCount - 1
                                        .Rows(n).Cells(j).Style.ForeColor = Color.Red
                                    Next

                                Else
                                    .Rows(n).Cells(7).Value = ""
                                    'For j = 0 To .ColumnCount - 1
                                    '    .Rows(k).Cells(j).Style.ForeColor = Color.Black
                                    'Next

                                End If

                                .Rows(n).Cells(9).Value = Trim(Dt2.Rows(k).Item("DeliveryTo").ToString)

                            End If
                        Next


                    End If
                    Dt2.Clear()


                Next

            End With

        End If

        pnl_Delivery_Selection.Visible = True
        Pnl_Back.Enabled = False

        If dgv_delivery_Selections.Rows.Count > 0 Then
            dgv_delivery_Selections.Focus()
            dgv_delivery_Selections.CurrentCell = dgv_delivery_Selections.Rows(0).Cells(0)
        End If


    End Sub

    Private Sub Close_Delivery_Selection()
        Dim CMD As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim i As Integer, n As Integer
        Dim sno As Integer = 0
        Dim Clo_IdNo As Integer = 0

        CMD.Connection = con


        sno = 0
        Clo_IdNo = 0

        lbl_Delivery_Code.Text = ""
        txt_Party_DcNo.Text = ""
        dgv_YarnDetails.Rows.Clear()

        For k = 0 To dgv_delivery_Selections.RowCount - 1

            If Val(dgv_delivery_Selections.Rows(k).Cells(7).Value) = 1 And Trim(dgv_delivery_Selections.Rows(k).Cells(8).Value) <> "" Then

                lbl_Delivery_Code.Text = Trim(dgv_delivery_Selections.Rows(k).Cells(8).Value)

                txt_Party_DcNo.Text = Trim(dgv_delivery_Selections.Rows(k).Cells(1).Value)

                cbo_DeliveryTo.Text = dgv_delivery_Selections.Rows(k).Cells(9).Value

                Da = New SqlClient.SqlDataAdapter("Select isnull(b.ledger_name,'') as deliveryatname from Yarn_Delivery_Selections_Processing_Details a LEFT OUTER JOIN ledger_head b ON b.ledger_idno <> 0 and b.ledger_idno = a.DeliveryAt_Idno Where a.Reference_Code = '" & Trim(dgv_delivery_Selections.Rows(k).Cells(8).Value) & "'", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)
                If Dt1.Rows.Count > 0 Then
                    If Trim(Dt1.Rows(0).Item("deliveryatname").ToString) <> "" Then
                        cbo_DeliveryTo.Text = Dt1.Rows(0).Item("deliveryatname").ToString
                    End If
                End If
                Dt1.Clear()


                CMD.CommandText = "truncate table " & Trim(Common_Procedures.EntryTempTable) & ""
                CMD.ExecuteNonQuery()

                Common_Procedures.get_YarnDelivery_Selection_Processing_Details(con, Trim(dgv_delivery_Selections.Rows(k).Cells(8).Value))

                Da = New SqlClient.SqlDataAdapter("Select int1 as Sl_No, Name1 as countname, name2 as Yarn_type, name3 as millname, meters1 as Bags, int2 as Cones, weight3 as Weight, weight4 as Thiri from " & Trim(Common_Procedures.EntryTempTable) & " Order by int1", con)
                'Da = New SqlClient.SqlDataAdapter("Select a.* from Weaver_Yarn_Delivery_Details a   where  'YNDLV-'+  a.Weaver_Yarn_Delivery_code =  '" & Trim(dgv_delivery_Selections.Rows(k).Cells(8).Value) & "' ", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)

                With dgv_YarnDetails

                    If Val(Dt1.Rows.Count <> 0) Then

                        For i = 0 To Dt1.Rows.Count - 1

                            n = dgv_YarnDetails.Rows.Add()

                            .Rows(n).Cells(0).Value = i + 1 ' Trim(Dt1.Rows(i).Item("Sl_No").ToString)
                            .Rows(n).Cells(1).Value = Trim(Dt1.Rows(i).Item("countname").ToString) ' Common_Procedures.Count_IdNoToName(con, Dt1.Rows(i).Item("count_idno").ToString)
                            .Rows(n).Cells(2).Value = Trim(Dt1.Rows(i).Item("Yarn_type").ToString)
                            .Rows(n).Cells(3).Value = Trim(Dt1.Rows(i).Item("millname").ToString) '  Common_Procedures.Mill_IdNoToName(con, Dt1.Rows(i).Item("mill_idno").ToString)
                            .Rows(n).Cells(4).Value = Val(Dt1.Rows(i).Item("Bags").ToString)
                            .Rows(n).Cells(5).Value = Val(Dt1.Rows(i).Item("Cones").ToString)
                            .Rows(n).Cells(6).Value = Val(Dt1.Rows(i).Item("Weight").ToString)
                            If Val(Dt1.Rows(i).Item("Thiri").ToString) <> 0 Then
                                .Rows(n).Cells(7).Value = Val(Dt1.Rows(i).Item("Thiri").ToString)
                            End If

                        Next

                    End If

                End With

                Exit For

            End If

        Next

        Dt1.Clear()
        Dt1.Dispose()
        Da.Dispose()

        Pnl_Back.Enabled = True
        pnl_Delivery_Selection.Visible = False

        If Trim(UCase(cbo_Type.Text)) = "DELIVERY" Then
            dgv_YarnDetails.AllowUserToAddRows = False
        End If

        txt_Party_DcNo.Focus()

    End Sub
    Private Sub btn_Close_Delivery_Selection_Click(sender As Object, e As EventArgs) Handles btn_Close_Delivery_Selection.Click
        Close_Delivery_Selection()
    End Sub
    Private Sub dgv_delivery_Selections_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgv_delivery_Selections.CellClick
        Select_Dc(e.RowIndex)
    End Sub

    Private Sub dgv_delivery_Selections_CellMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles dgv_delivery_Selections.CellMouseClick
        btn_Close_Delivery_Selection_Click(sender, e)
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

    Private Sub cbo_Type_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Type.Enter
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")
    End Sub

    Private Sub cbo_Type_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Type.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Type, msk_date, cbo_Ledger, "", "", "", "")
    End Sub

    Private Sub cbo_Type_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Type.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Type, cbo_Ledger, "", "", "", "")

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
            If dgv_YarnDetails.Rows.Count > 0 Then
                dgv_YarnDetails.Focus()
                dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(1)

            Else
                btn_save.Focus()

            End If
        End If
    End Sub

    Private Sub cbo_weaving_job_no_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_weaving_job_no.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_weaving_job_no, Nothing, Nothing, "Weaving_JobCard_Head", "Weaving_JobCode_forSelection", "", "(Ledger_IdNo = 0)")

        If (e.KeyCode = 40 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If dgv_YarnDetails.Rows.Count > 0 Then
                dgv_YarnDetails.Focus()
                dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(1)

            Else
                btn_save.Focus()


            End If
        End If

        If (e.KeyCode = 38 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
            txt_Freight.Focus()
        End If
    End Sub
    Private Sub btn_EWayBIll_Generation_Click(sender As Object, e As EventArgs) Handles btn_EWayBIll_Generation.Click
        btn_GENERATEEWB.Enabled = True
        Grp_EWB.Visible = True
        Grp_EWB.BringToFront()
        Grp_EWB.Left = (Me.Width - Pnl_Back.Width) / 2 + 160
        Grp_EWB.Top = (Me.Height - Pnl_Back.Height) / 2 + 150

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

        Dim da As New SqlClient.SqlDataAdapter("Select EwayBill_No from Weaver_Yarn_Receipt_Head where Weaver_Yarn_Receipt_Code = '" & NewCode & "'", con)
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
                         "  SELECT               'I'              , '6'             ,   'JOB WORK RETURNS'        ,    'CHL'    , a.Weaver_Yarn_Receipt_No , a.Weaver_Yarn_Receipt_Date     , L.Ledger_GSTINNo, L.Ledger_MainName   , L.Ledger_Address1 +  L.Ledger_Address2 , L.Ledger_Address3 + L.Ledger_Address4 , L.City_Town ," &
                         " L.PinCode     , TS.State_Code  ,TS.State_Code    , C.Company_GSTINNo, C.Company_Name , (case when a.DeliveryTo_IdNo = 4 then (C.Company_Address1+C.Company_Address2) when a.DeliveryTo_IdNo <> 0 then tDELV.Ledger_Address1+tDELV.Ledger_Address2 else C.Company_Address1+C.Company_Address2 end) as deliveryaddress1,  (case when a.DeliveryTo_IdNo = 4 then (c.Company_Address3+C.Company_Address4) when a.DeliveryTo_IdNo <> 0 then tDELV.Ledger_Address3+tDELV.Ledger_Address4 else  c.Company_Address3+C.Company_Address4 end) as deliveryaddress2, (case when a.DeliveryTo_IdNo = 4 then (c.Company_City) when a.DeliveryTo_IdNo <> 0 then tDELV.City_Town else c.Company_City end) as city_town_name, (case when a.DeliveryTo_IdNo = 4 then (c.Company_PinCode) when a.DeliveryTo_IdNo <> 0 then tDELV.Pincode else  c.Company_PinCode end) as pincodee,(case when a.DeliveryTo_IdNo = 4 then (FS.State_Code) when a.DeliveryTo_IdNo <> 0 then DLSC.State_Code ELSE  FS.State_Code END ),  (case when a.DeliveryTo_IdNo = 4 then (FS.State_Code) when a.DeliveryTo_IdNo <> 0 then DLSC.State_Code ELSE  FS.State_Code END   )  as actual_StateCode , " &
                         " 1                     , 0 , a.Net_Amount     , 0 ,  0 , 0   ,   0         ,0                   , t.Ledger_GSTINNo  , t.Ledger_Name ," &
                         " ''        ,''            ,  0        ,     '1'  AS TrMode ," &
                         " a.Vechile_No, 'R', '" & NewCode & "', (case when a.DeliveryTo_IdNo = 4 or a.DeliveryTo_IdNo = 0 then  c.Company_GSTINNo else tDELV.Ledger_GSTINNo end ) as ShippedTo_GSTIN, tDELV.Ledger_MainName as ShippedTo_LedgerName  from Weaver_Yarn_Receipt_Head a inner Join Company_Head C on a.Company_IdNo = C.Company_IdNo " &
                         " Inner Join Ledger_Head L ON a.Ledger_IdNo = L.Ledger_IdNo Left Outer Join Ledger_Head tDELV on a.DeliveryTo_IdNo <> 0 and a.DeliveryTo_IdNo = tDELV.Ledger_IdNo   left Outer Join Ledger_Head T on a.Transport_IdNo = T.Ledger_IdNo " &
                         " Left Outer Join State_Head FS On C.Company_State_IdNo = fs.State_IdNo left Outer Join State_Head TS on L.Ledger_State_IdNo = TS.State_IdNo  left Outer Join State_Head  DLSC ON tDELV.Ledger_State_IdNo = DLSC.State_IdNo  " &
                         " where a.Weaver_Yarn_Receipt_Code = '" & Trim(NewCode) & "'"
        CMD.ExecuteNonQuery()

        'CMD.CommandText = "Insert into EWB_Head ([SupplyType]  ,[SubSupplyType]  , [SubSupplyDesc]  ,[DocType]  ,	[EWBGenDocNo]                           ,[EWBDocDate]        ,[FromGSTIN]       ,[FromTradeName]  ,[FromAddress1]   ,[FromAddress2]     ,[FromPlace] ," &
        '                 "[FromPINCode]     ,	[FromStateCode] ,[ActualFromStateCode] ,[ToGSTIN]       ,[ToTradeName],[ToAddress1]      ,[ToAddress2]    ,[ToPlace]       ,[ToPINCode]       ,[ToStateCode] , [ActualToStateCode] ," &
        '                 "[TransactionType],[OtherValue]                       ,	[Total_value]       ,	[CGST_Value],[SGST_Value],[IGST_Value]     ,	[CessValue],[CessNonAdvolValue],[TransporterID]    ,[TransporterName]," &
        '                 "[TransportDOCNo] ,[TransportDOCDate]    ,[TotalInvValue]    ,[TransMode]             ," &
        '                 "[VehicleNo]      ,[VehicleType]   , [InvCode], [ShippedToGSTIN], [ShippedToTradeName] ) " &
        '                 " " &
        '                 " " &
        '                 "  SELECT               'O'              , '4'             ,   'JOB WORK RETURNS'              ,    'CHL'    , a.Weaver_Yarn_Receipt_No ,a.Weaver_Yarn_Receipt_Date          , C.Company_GSTINNo, C.Company_Name   ,C.Company_Address1+C.Company_Address2,c.Company_Address3+C.Company_Address4,C.Company_City ," &
        '                 " C.Company_PinCode    , FS.State_Code  ,FS.State_Code    ,L.Ledger_GSTINNo, L.Ledger_MainName, (case when a.DeliveryTo_IdNo <> 0 then tDELV.Ledger_Address1+tDELV.Ledger_Address2 else  L.Ledger_Address1+L.Ledger_Address2 end) as deliveryaddress1,  (case when a.DeliveryTo_IdNo <> 0 then tDELV.Ledger_Address3+tDELV.Ledger_Address4 else  L.Ledger_Address3+L.Ledger_Address4 end) as deliveryaddress2, (case when a.DeliveryTo_IdNo <> 0 then tDELV.City_Town else  L.City_Town end) as city_town_name, (case when a.DeliveryTo_IdNo <> 0 then tDELV.Pincode else  L.Pincode end) as pincodee, TS.State_Code, (case when a.DeliveryTo_IdNo <> 0 then TDCS.State_Code else TS.State_Code end) as actual_StateCode," &
        '                 " 1                     , 0 , a.Net_Amount     ,   0  ,  0  , 0   ,   0         ,0                   , t.Ledger_GSTINNo  , t.Ledger_Name ," &
        '                 " ''        ,''            ,  0        ,     '1'  AS TrMode ," &
        '                 " a.Vechile_No, 'R', '" & NewCode & "', tDELV.Ledger_GSTINNo as ShippedTo_GSTIN, tDELV.Ledger_MainName as ShippedTo_LedgerName from Weaver_Yarn_Receipt_Head a inner Join Company_Head C on a.Company_IdNo = C.Company_IdNo " &
        '                 " Inner Join Ledger_Head L ON a.DeliveryTo_IdNo = L.Ledger_IdNo Left Outer Join Ledger_Head tDELV on a.DeliveryTo_IdNo <> 0 and a.DeliveryTo_IdNo = tDELV.Ledger_IdNo left Outer Join Ledger_Head T on a.Transport_IdNo = T.Ledger_IdNo " &
        '                 " Left Outer Join State_Head FS On C.Company_State_IdNo = fs.State_IdNo left Outer Join State_Head TS on L.Ledger_State_IdNo = TS.State_IdNo  left Outer Join State_Head TDCS on tDELV.Ledger_State_IdNo = TDCS.State_IdNo " &
        '                  " where a.Weaver_Yarn_Receipt_Code = '" & Trim(NewCode) & "'"
        'CMD.ExecuteNonQuery()

        'vSgst = 

        'CMD.CommandText = " Update EWB_Head Set CGST_Value = ( (Total_value * 5 / 100 ) / 2 ) , SGST_Value = ( (Total_value * 5 / 100 ) / 2 ) , TotalInvValue = ( Total_value + SGST_Value + CGST_Value )  where InvCode = '" & Trim(NewCode) & "' "
        'CMD.ExecuteNonQuery()


        'CMD.CommandText = " Update EWB_Head Set TotalInvValue = ( Total_value + SGST_Value + CGST_Value )  where InvCode = '" & Trim(NewCode) & "' "
        'CMD.ExecuteNonQuery()

        Dim vCgst_Amt As String = 0
        Dim vSgst_Amt As String = 0
        Dim vIgst_AMt As String = 0
        Dim vTax_Perc As String = 0


        CMD.CommandText = "Delete from EWB_Details Where InvCode = '" & NewCode & "'"
        CMD.ExecuteNonQuery()

        Dim dt1 As New DataTable
        da = New SqlClient.SqlDataAdapter(" Select  I.Count_Name,  IG.ItemGroup_Name  , IG.Item_HSN_Code,( Case When ( Lh.Ledger_Type ='Weaver' or Lh.Show_In_All_Entry =1 ) and Lh.Ledger_GSTINNo <> '' Then (IG.Item_GST_Percentage ) else 0 end ) , sum(SD.WEIGHT * A.RATE) As TaxableAmt,sum(SD.Weight) as Qty, 1 , 'WGT' AS Units , tz.Company_State_IdNo , Lh.Ledger_State_Idno  ,a.GST_Tax_Invoice_Status  " &
                                          " from Weaver_Yarn_Receipt_Details SD Inner Join Weaver_Yarn_Receipt_Head a On a.Weaver_Yarn_Receipt_Code = sd.Weaver_Yarn_Receipt_Code Inner Join Count_Head I On SD.Count_IdNo = I.Count_IdNo Inner Join ItemGroup_Head IG on I.ItemGroup_IdNo = IG.ItemGroup_IdNo " &
                                          " INNER Join Ledger_Head Lh ON Lh.Ledger_Idno = a.Ledger_Idno   INNER JOIN Company_Head tz On tz.Company_Idno = a.Company_Idno  Where SD.Weaver_Yarn_Receipt_Code = '" & Trim(NewCode) & "' Group By " &
                                          " I.Count_Name,IG.ItemGroup_Name,IG.Item_HSN_Code,IG.ItemGroup_Name ,IG.Item_HSN_Code,IG.Item_GST_Percentage, Lh.Ledger_Type , Lh.Show_In_All_Entry ,Lh.Ledger_GSTINNo , tz.Company_State_IdNo , Lh.Ledger_State_Idno ,a.GST_Tax_Invoice_Status  ", con)

        '(case when a.DeliveryTo_IdNo<> 0 then a.DeliveryTo_IdNo else a.Ledger_Idno end )


        dt1 = New DataTable
        da.Fill(dt1)


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

            End If

            CMD.CommandText = "Insert into EWB_Details ([SlNo]                               , [Product_Name]           ,	[Product_Description]     ,	[HSNCode]                 ,	[Quantity]                                ,     [QuantityUnit] ,             Tax_Perc      ,	[CessRate]         ,	[CessNonAdvol]  ,	[TaxableAmount]               , InvCode      ,                   Cgst_Value  ,                       Sgst_Value ,                         Igst_Value) " &
                                  " values                 (" & dt1.Rows(I).Item(6).ToString & ",'" & dt1.Rows(I).Item(0) & "', '" & dt1.Rows(I).Item(1) & "', '" & dt1.Rows(I).Item(2) & "', " & dt1.Rows(I).Item(5).ToString & ",         'KGS'          ," & Val(vTax_Perc) & " , 0                  , 0                   ," & dt1.Rows(I).Item(4) & ",'" & NewCode & "',   '" & Str(Val(vCgst_Amt)) & "'        ,   '" & Str(Val(vSgst_Amt)) & "'     , '" & Str(Val(vIgst_AMt)) & "')"

            CMD.ExecuteNonQuery()

        Next

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
        EWB.GenerateEWB(NewCode, con, rtbEWBResponse, txt_EWBNo, "Weaver_Yarn_Receipt_Head", "EwayBill_No", "Weaver_Yarn_Receipt_Code", Pk_Condition)


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

        EWB.CancelEWB(txt_EWBNo.Text, NewCode, con, rtbEWBResponse, txt_EWBNo, "Weaver_Yarn_Receipt_Head", "EwayBill_No", "Weaver_Yarn_Receipt_Code")

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
        txt_Eway_Bill_No.Text = txt_EWBNo.Text
    End Sub

    Private Sub txt_rate_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_rate.KeyDown
        If e.KeyCode = 38 Then

            With dgv_YarnDetails
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(1)

            End With
        ElseIf e.KeyCode = 40 Then

            txt_Eway_Bill_No.Focus()

        End If
    End Sub

    Private Sub txt_rate_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_rate.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_Eway_Bill_No.Focus()

        End If
    End Sub
    Private Sub txt_Eway_Bill_No_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_Eway_Bill_No.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_remarks.Focus()

        End If
    End Sub

    Private Sub txt_Eway_Bill_No_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Eway_Bill_No.KeyDown
        If e.KeyCode = 38 Then

            txt_rate.Focus()
        ElseIf e.KeyCode = 40 Then

            txt_remarks.Focus()

        End If
    End Sub

    Private Sub txt_remarks_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_remarks.KeyDown
        If e.KeyCode = 38 Then
            txt_Eway_Bill_No.Focus()
        ElseIf e.KeyCode = 40 Then

            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                msk_date.Focus()
            End If
        End If
    End Sub

    Private Sub txt_remarks_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_remarks.KeyPress
        If Asc(e.KeyChar) = 13 Then

            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                msk_date.Focus()
            End If
        End If

    End Sub
    Private Sub txt_rate_TextChanged(sender As Object, e As System.EventArgs) Handles txt_rate.TextChanged

        Dim vTotWgt As String = ""

        vTotWgt = 0
        With dgv_YarnDetails_Total
            If .RowCount > 0 Then
                vTotWgt = Format(Val(.Rows(0).Cells(6).Value), "########0.000")
            End If
        End With

        txt_Amount.Text = Format(Val(vTotWgt) * Val(txt_rate.Text), "############0.000")
    End Sub

    Private Sub cbo_ClothSales_OrderCode_forSelection_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_ClothSales_OrderCode_forSelection.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, sender, Nothing, "ClothSales_Order_Head", "ClothSales_OrderCode_forSelection", vCLO_CONDT, "(ClothSales_Order_Code = '999999/00-00')")

        If Asc(e.KeyChar) = 13 Then
            dgv_YarnDetails.Focus()
            dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(1)
            dgv_YarnDetails.CurrentCell.Selected = True
        End If


    End Sub

    Private Sub cbo_ClothSales_OrderCode_forSelection_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_ClothSales_OrderCode_forSelection.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, sender, Nothing, Nothing, "ClothSales_Order_Head", "ClothSales_OrderCode_forSelection", vCLO_CONDT, "(ClothSales_Order_Code = '999999/00-00')")

        If (e.KeyValue = 38 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
            If cbo_weaving_job_no.Visible = True Then
                cbo_weaving_job_no.Focus()

            ElseIf cbo_Cloth.Visible = True Then
                cbo_Cloth.Focus()

            Else
                txt_Freight.Focus()
            End If
        End If


        If (e.KeyValue = 40 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            dgv_YarnDetails.Focus()
            dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(1)
            dgv_YarnDetails.CurrentCell.Selected = True
        End If




    End Sub

    Private Sub cbo_ClothSales_OrderCode_forSelection_Enter(sender As Object, e As EventArgs) Handles cbo_ClothSales_OrderCode_forSelection.Enter
        vCLO_CONDT = "(ClothSales_Order_Code LIKE '%/" & Trim(FnYearCode1) & "' or ClothSales_Order_Code LIKE '%/" & Trim(FnYearCode2) & "' and Order_Close_Status = 0 )"
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "ClothSales_Order_Head", "ClothSales_OrderCode_forSelection", vCLO_CONDT, "(ClothSales_Order_Code = '999999/00-00')")
    End Sub

    Private Sub cbo_Cloth_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_Cloth.SelectedIndexChanged

    End Sub

    Private Sub cbo_ClothSales_OrderCode_forSelection_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_ClothSales_OrderCode_forSelection.SelectedIndexChanged

    End Sub

    Private Sub cbo_ClothSales_OrderCode_forSelection_KeyUp(sender As Object, e As KeyEventArgs) Handles cbo_ClothSales_OrderCode_forSelection.KeyUp

    End Sub

    Private Sub cbo_Grid_CountName_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_Grid_CountName.SelectedIndexChanged

    End Sub

    Private Sub txt_Freight_TextChanged(sender As Object, e As EventArgs) Handles txt_Freight.TextChanged

    End Sub

    Private Sub msk_date_MaskInputRejected(sender As Object, e As MaskInputRejectedEventArgs) Handles msk_date.MaskInputRejected

    End Sub
End Class