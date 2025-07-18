Public Class Roll_Packing_1352
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "RLPCK-"
    Private NoCalc_Status As Boolean = False
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Private vCbo_ItmNm As String

    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl

    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
    Private prn_HdIndx As Integer
    Private prn_DetSNo As Integer
    Private prn_Count As Integer = 0
    Public vmskOldText As String = ""
    Public vmskSelStrt As Integer = -1

    Private dgvDet_CboBx_ColNos_Arr As Integer() = {0}

    Private Enum dgvCol_Details As Integer
        SlNo
        LotNo
        PcsNo
        Loom_No
        Totalmeters
        Roll_No

        P4
        p3
        p2
        P1 'pass_Meter

        TotalPoints 'points

        Per_100ML ' Points_Pass_Meter

        fabric_grade 'Grade

        Warp_Lot_No ' less_meter
        Weft_lot_No ' Type

        Net_Weight
        gross_Weight 'wgt_mtr

        fabric_defect_Details ' reject_meter

        lot_code
        pcs_party_name
        pcs_cloth_name
        buyer_offer_code
        roll_code
        bale_delivery_code



    End Enum


    Private Enum dgvCol_Selection As Integer
        SNO
        LOT_NO
        PCS_NO
        CLOTH_TYPE
        METERS
        WEIGHT
        wgt_mtr
        BUYER_OFFER_NO
        BUYER_REF_NO
        PARTY_PCS_NO
        ROLL_NO
        STS
        lot_code
        pcs_party_name
        pcs_cloth_name
        pass_mtr
        less_mtr
        rejection_mtr
        points
        points_pass_mtr
        grade
        Buyer_Offer_Code
        Roll_Code
        Bale_Delivery_Code
        loom_no

    End Enum



    Private Sub clear()

        NoCalc_Status = True

        New_Entry = False
        Insert_Entry = False

        Pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        pnl_Selection.Visible = False
        pnl_Print.Visible = False

        lbl_RefNo.Text = ""
        lbl_RefNo.ForeColor = Color.Black

        vmskOldText = ""
        vmskSelStrt = -1
        msk_date.Text = ""
        dtp_Date.Text = ""
        ' cbo_Cloth.Text = ""
        cbo_ClothType.Text = Common_Procedures.ClothType_IdNoToName(con, 1)
        ' cbo_Cloth.Text = ""
        cbo_Bale_Bundle.Text = "ROLL"
        cbo_Type.Text = "PCS"
        txt_LotSelction.Text = ""
        txt_PcsSelction.Text = ""
        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))

        txt_Folding.Text = 100
        txt_Note.Text = ""
        chk_SelectAll.Checked = False

        txt_Tareweight.Text = "3"

        dgv_Details.Rows.Clear()
        dgv_Details_Total.Rows.Clear()
        dgv_Details_Total.Rows.Add()


        If Filter_Status = False Then
            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_Party.Text = ""
            cbo_Filter_Cloth.Text = ""
            txt_Filter_BuyerRefNo.Text = ""
            txt_Filter_LotNo.Text = ""
            txt_Filter_PcsNo.Text = ""
            txt_Filter_RollNo.Text = ""
            cbo_Filter_Party.SelectedIndex = -1
            cbo_Filter_Cloth.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()
        End If

        cbo_PartyName.Enabled = True
        cbo_PartyName.BackColor = Color.White

        cbo_Cloth.Enabled = True
        cbo_Cloth.BackColor = Color.White

        cbo_ClothType.Enabled = True
        cbo_ClothType.BackColor = Color.White

        txt_Folding.Enabled = True
        txt_Folding.BackColor = Color.White

        btn_Selection.Enabled = True

        Grid_Cell_DeSelect()
        NoCalc_Status = False

    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox
        Dim Msktxbx As MaskedTextBox
        On Error Resume Next

        If FrmLdSTS = True Then Exit Sub

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Me.ActiveControl Is CheckBox Or TypeOf Me.ActiveControl Is Button Or TypeOf Me.ActiveControl Is MaskedTextBox Then
            Me.ActiveControl.BackColor = Color.SpringGreen
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
            Msktxbx.SelectAll()
        End If

        If Me.ActiveControl.Name <> dgv_Details_Total.Name Then
            Grid_DeSelect()
        End If

        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)

        On Error Resume Next

        If FrmLdSTS = True Then Exit Sub

        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Or TypeOf Prec_ActCtrl Is CheckBox Or TypeOf Prec_ActCtrl Is MaskedTextBox Then
                Prec_ActCtrl.BackColor = Color.White
                Prec_ActCtrl.ForeColor = Color.Black
            ElseIf TypeOf Prec_ActCtrl Is Button Then
                Prec_ActCtrl.BackColor = Color.FromArgb(2, 57, 111)
                Prec_ActCtrl.ForeColor = Color.White
            End If
        End If

    End Sub

    Private Sub Grid_DeSelect()
        On Error Resume Next
        If Not IsNothing(dgv_Details.CurrentCell) Then  dgv_Details.CurrentCell.Selected = False
        If Not IsNothing(dgv_Details_Total.CurrentCell) Then dgv_Details_Total.CurrentCell.Selected = False
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
        If Not IsNothing(dgv_Details_Total.CurrentCell) Then dgv_Details_Total.CurrentCell.Selected = False
        If Not IsNothing(dgv_Filter_Details.CurrentCell) Then dgv_Filter_Details.CurrentCell.Selected = False
    End Sub

    Private Sub Packing_Slip_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_PartyName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_PartyName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Cloth.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CLOTH" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Cloth.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_ClothType.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CLOTH TYPE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_ClothType.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            Common_Procedures.Master_Return.Form_Name = ""
            Common_Procedures.Master_Return.Control_Name = ""
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

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

    Private Sub Packing_Slip_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub Packing_Slip_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
                    Exit Sub

                ElseIf pnl_Selection.Visible = True Then
                    btn_Close_Selection_Click(sender, e)
                    Exit Sub

                ElseIf pnl_Print.Visible = True Then
                    btn_print_Close_Click(sender, e)
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

    Private Sub Packing_Slip_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable

        FrmLdSTS = True

        ' dgv_Details.Columns(1).HeaderText = Trim(UCase(Common_Procedures.settings.ClothReceipt_LotNo_OR_RollNo_Text))
        ' dgv_Selection.Columns(1).HeaderText = Trim(UCase(Common_Procedures.settings.ClothReceipt_LotNo_OR_RollNo_Text))

        Me.Text = ""

        con.Open()

        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or Ledger_Type = 'GODOWN' or Ledger_Type = 'JOBWORKER' or Show_In_All_Entry = 1) order by Ledger_DisplayName", con)
        da.Fill(dt3)
        cbo_PartyName.DataSource = dt3
        cbo_PartyName.DisplayMember = "Ledger_DisplayName"

        da = New SqlClient.SqlDataAdapter("select Cloth_Name from Cloth_Head order by Cloth_Name", con)
        da.Fill(dt1)
        cbo_Cloth.DataSource = dt1
        cbo_Cloth.DisplayMember = "Cloth_Name"

        da = New SqlClient.SqlDataAdapter("select ClothType_Name from ClothType_Head order by ClothType_Name", con)
        da.Fill(dt2)
        cbo_ClothType.DataSource = dt2
        cbo_ClothType.DisplayMember = "ClothType_Name"

        dtp_Date.Text = ""
        msk_date.Text = ""

        cbo_Bale_Bundle.Items.Clear()
        cbo_Bale_Bundle.Items.Add("BALE")
        cbo_Bale_Bundle.Items.Add("BUNDLE")
        cbo_Bale_Bundle.Items.Add("ROLL")


        cbo_Type.Items.Clear()
        cbo_Type.Items.Add("PCS")
        cbo_Type.Items.Add("BUYER-OFFER")


        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2

        pnl_Selection.Visible = False
        pnl_Selection.Left = (Me.Width - pnl_Selection.Width) \ 2
        pnl_Selection.Top = (Me.Height - pnl_Selection.Height) \ 2
        pnl_Selection.BringToFront()

        pnl_Print.Visible = False
        pnl_Print.Left = (Me.Width - pnl_Print.Width) \ 2
        pnl_Print.Top = (Me.Height - pnl_Print.Height) \ 2
        pnl_Print.BringToFront()

        btn_UserModification.Visible = False
        If Common_Procedures.settings.User_Modifications_Show_Status = 1 Then
            If Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_UserModification_Status = 1 Then
                btn_UserModification.Visible = True
            End If
        End If

        AddHandler msk_date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Cloth.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ClothType.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Bale_Bundle.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Folding.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Note.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_Party.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_Cloth.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Filter_BuyerRefNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Filter_LotNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Filter_PcsNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Filter_RollNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PrintFrom.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PrintTo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_save.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_close.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print_Ok.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print_Cancel.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_LotSelction.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PcsSelction.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Tareweight.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Cloth.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ClothType.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Bale_Bundle.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Folding.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Note.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_Party.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_Cloth.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Filter_BuyerRefNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Filter_LotNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Filter_PcsNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Filter_RollNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PrintFrom.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PrintTo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Tareweight.LostFocus, AddressOf ControlLostFocus

        AddHandler btn_save.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_close.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print_Ok.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print_Cancel.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_LotSelction.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PcsSelction.LostFocus, AddressOf ControlLostFocus

        AddHandler msk_date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Filter_LotNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Filter_PcsNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Filter_BuyerRefNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Tareweight.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler msk_date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_PrintFrom.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Filter_LotNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Filter_PcsNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Filter_BuyerRefNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Tareweight.KeyPress, AddressOf TextBoxControlKeyPress

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True
        new_record()

    End Sub


    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
        Dim I As Integer = 0
        Dim dgv1 As New DataGridView

        If ActiveControl.Name = dgv_Details.Name Or ActiveControl.Name = dgv_Details.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            'On Error Resume Next

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

                    If keyData = Keys.Enter Or keyData = Keys.Down Or keyData = Keys.Up Then

                        Common_Procedures.DGVGrid_Cursor_Focus(dgv1, keyData, msk_date, txt_Note, dgvDet_CboBx_ColNos_Arr, dgtxt_Details, msk_date)

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
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub move_record(ByVal no As String)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim da3 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim NewCode As String
        Dim n As Integer
        Dim SNo As Integer
        Dim LockSTS As Boolean = False
        Dim vBaleDelvCd As String = ""

        If Val(no) = 0 Then Exit Sub

        clear()

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try
            da1 = New SqlClient.SqlDataAdapter("select a.* from Roll_Packing_Head a Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Roll_Packing_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_RefNo.Text = dt1.Rows(0).Item("Roll_Packing_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Roll_Packing_Date").ToString
                msk_date.Text = dtp_Date.Text
                cbo_PartyName.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Ledger_IdNo").ToString))
                cbo_Cloth.Text = Common_Procedures.Cloth_IdNoToName(con, Val(dt1.Rows(0).Item("Cloth_IdNo").ToString))
                cbo_ClothType.Text = Common_Procedures.ClothType_IdNoToName(con, Val(dt1.Rows(0).Item("ClothType_IdNo").ToString))
                cbo_Bale_Bundle.Text = dt1.Rows(0).Item("Bale_Bundle").ToString
                cbo_Type.Text = dt1.Rows(0).Item("Pcs_BufferOffer_Type").ToString
                txt_Note.Text = dt1.Rows(0).Item("Note").ToString
                txt_Folding.Text = Val(dt1.Rows(0).Item("Folding").ToString)
                txt_Tareweight.Text = Format(Val(dt1.Rows(0).Item("Tare_Weight").ToString), "########0.000")
                lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))

                LockSTS = False

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.clothtype_name as Pcs_ClothTypeName, c.Ledger_Name as Pcs_PartyName, d.cloth_name as Pcs_ClothName from Roll_Packing_Details a INNER JOIN ClothType_Head b ON a.ClothType_IdNo <> 0 and a.ClothType_IdNo = b.ClothType_IdNo LEFT OUTER JOIN Ledger_Head c ON a.Pcs_PartyIdNo <> 0 and a.Pcs_PartyIdNo = c.Ledger_Idno LEFT OUTER JOIN Cloth_Head d ON a.Pcs_Cloth_IdNo <> 0 and a.Pcs_Cloth_IdNo = d.Cloth_Idno where a.Roll_Packing_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_Details.Rows.Clear()
                SNo = 0

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_Details.Rows.Add()

                        SNo = SNo + 1



                        dgv_Details.Rows(n).Cells(dgvCol_Details.SlNo).Value = Val(SNo)
                        dgv_Details.Rows(n).Cells(dgvCol_Details.LotNo).Value = dt2.Rows(i).Item("Lot_No").ToString
                        dgv_Details.Rows(n).Cells(dgvCol_Details.PcsNo).Value = dt2.Rows(i).Item("Pcs_NO").ToString

                        dgv_Details.Rows(n).Cells(dgvCol_Details.Totalmeters).Value = Val(dt2.Rows(i).Item("Meters").ToString)
                        If Val(dgv_Details.Rows(n).Cells(dgvCol_Details.Totalmeters).Value) = 0 Then dgv_Details.Rows(n).Cells(dgvCol_Details.Totalmeters).Value = ""

                        dgv_Details.Rows(n).Cells(dgvCol_Details.Roll_No).Value = dt2.Rows(i).Item("Roll_No").ToString

                        dgv_Details.Rows(n).Cells(dgvCol_Details.Net_Weight).Value = Val(dt2.Rows(i).Item("Weight").ToString)
                        If Val(dgv_Details.Rows(n).Cells(dgvCol_Details.Net_Weight).Value) = 0 Then dgv_Details.Rows(n).Cells(dgvCol_Details.Net_Weight).Value = ""

                        dgv_Details.Rows(n).Cells(dgvCol_Details.gross_Weight).Value = Val(dt2.Rows(i).Item("Gross_Weight").ToString)
                        If Val(dgv_Details.Rows(n).Cells(dgvCol_Details.gross_Weight).Value) = 0 Then dgv_Details.Rows(n).Cells(dgvCol_Details.gross_Weight).Value = ""

                        dgv_Details.Rows(n).Cells(dgvCol_Details.P4).Value = Val(dt2.Rows(i).Item("P4").ToString)
                        If Val(dgv_Details.Rows(n).Cells(dgvCol_Details.P4).Value) = 0 Then dgv_Details.Rows(n).Cells(dgvCol_Details.P4).Value = ""

                        dgv_Details.Rows(n).Cells(dgvCol_Details.p3).Value = Val(dt2.Rows(i).Item("p3").ToString)
                        If Val(dgv_Details.Rows(n).Cells(dgvCol_Details.p3).Value) = 0 Then dgv_Details.Rows(n).Cells(dgvCol_Details.p3).Value = ""

                        dgv_Details.Rows(n).Cells(dgvCol_Details.p2).Value = Val(dt2.Rows(i).Item("P2").ToString)
                        If Val(dgv_Details.Rows(n).Cells(dgvCol_Details.p2).Value) = 0 Then dgv_Details.Rows(n).Cells(dgvCol_Details.p2).Value = ""

                        dgv_Details.Rows(n).Cells(dgvCol_Details.P1).Value = Val(dt2.Rows(i).Item("P1").ToString)
                        If Val(dgv_Details.Rows(n).Cells(dgvCol_Details.P1).Value) = 0 Then dgv_Details.Rows(n).Cells(dgvCol_Details.P1).Value = ""

                        dgv_Details.Rows(n).Cells(dgvCol_Details.TotalPoints).Value = Val(dt2.Rows(i).Item("Points").ToString)
                        If Val(dgv_Details.Rows(n).Cells(dgvCol_Details.TotalPoints).Value) = 0 Then dgv_Details.Rows(n).Cells(dgvCol_Details.TotalPoints).Value = ""

                        dgv_Details.Rows(n).Cells(dgvCol_Details.Per_100ML).Value = Val(dt2.Rows(i).Item("Point_Per_PassMeter").ToString)
                        If Val(dgv_Details.Rows(n).Cells(dgvCol_Details.Per_100ML).Value) = 0 Then dgv_Details.Rows(n).Cells(dgvCol_Details.Per_100ML).Value = ""

                        dgv_Details.Rows(n).Cells(dgvCol_Details.Warp_Lot_No).Value = Trim(dt2.Rows(i).Item("Warp_Lot_no").ToString)
                        dgv_Details.Rows(n).Cells(dgvCol_Details.Weft_lot_No).Value = Trim(dt2.Rows(i).Item("Weft_lot_no").ToString)

                        dgv_Details.Rows(n).Cells(dgvCol_Details.fabric_defect_Details).Value = Trim(dt2.Rows(i).Item("Fabric_defect_penalty_point").ToString)
                       
                        dgv_Details.Rows(n).Cells(dgvCol_Details.fabric_grade).Value = dt2.Rows(i).Item("Grade").ToString

                        dgv_Details.Rows(n).Cells(dgvCol_Details.lot_code).Value = dt2.Rows(i).Item("lot_code").ToString
                        dgv_Details.Rows(n).Cells(dgvCol_Details.pcs_party_name).Value = dt2.Rows(i).Item("Pcs_PartyName").ToString
                        dgv_Details.Rows(n).Cells(dgvCol_Details.pcs_cloth_name).Value = dt2.Rows(i).Item("Pcs_ClothName").ToString

                        dgv_Details.Rows(n).Cells(dgvCol_Details.buyer_offer_code).Value = dt2.Rows(i).Item("Buyer_Offer_Code").ToString
                        dgv_Details.Rows(n).Cells(dgvCol_Details.roll_code).Value = dt2.Rows(i).Item("Roll_Code").ToString

                        vBaleDelvCd = ""
                        da3 = New SqlClient.SqlDataAdapter("Select a.* from Packing_Slip_Head a Where a.Packing_Slip_Code = '" & Trim(dt2.Rows(i).Item("Roll_Code").ToString) & "'", con)
                        dt3 = New DataTable
                        da3.Fill(dt3)
                        If dt3.Rows.Count > 0 Then
                            If IsDBNull(dt3.Rows(0).Item("Delivery_Code").ToString) = False Then
                                vBaleDelvCd = dt3.Rows(0).Item("Delivery_Code").ToString
                            End If

                        End If
                        dt3.Clear()

                        dgv_Details.Rows(n).Cells(dgvCol_Details.bale_delivery_code).Value = vBaleDelvCd

                        If Val(dt2.Rows(i).Item("Loom_IdNo").ToString) <> 0 Then
                            dgv_Details.Rows(n).Cells(dgvCol_Details.Loom_No).Value = Common_Procedures.Loom_IdNoToName(con, Val(dt2.Rows(i).Item("Loom_IdNo").ToString))
                        Else
                            dgv_Details.Rows(n).Cells(dgvCol_Details.Loom_No).Value = dt2.Rows(i).Item("Loom_No").ToString
                        End If

                        If Trim(vBaleDelvCd) <> "" Then
                            For J = 0 To dgv_Details.ColumnCount - 1
                                dgv_Details.Rows(n).Cells(J).Style.BackColor = Color.LightGray
                                dgv_Details.Rows(n).Cells(J).Style.ForeColor = Color.Red
                            Next
                            LockSTS = True
                        End If

                    Next i

                End If

                With dgv_Details_Total

                    If .RowCount = 0 Then .Rows.Add()
                    '.Rows(0).Cells(dgvCol_Details.PcsNo).Value = Val(dt1.Rows(0).Item("Total_Pcs").ToString)
                    .Rows(0).Cells(dgvCol_Details.Totalmeters).Value = Format(Val(dt1.Rows(0).Item("Total_Meters").ToString), "########0.00")
                    .Rows(0).Cells(dgvCol_Details.net_Weight).Value = Format(Val(dt1.Rows(0).Item("Total_Weight").ToString), "########0.000")

                    .Rows(0).Cells(dgvCol_Details.P1).Value = Val(dt1.Rows(0).Item("P1").ToString)
                    .Rows(0).Cells(dgvCol_Details.p2).Value = Val(dt1.Rows(0).Item("P2").ToString)
                    .Rows(0).Cells(dgvCol_Details.p3).Value = Val(dt1.Rows(0).Item("P3").ToString)
                    .Rows(0).Cells(dgvCol_Details.P4).Value = Val(dt1.Rows(0).Item("P4").ToString)
                    .Rows(0).Cells(dgvCol_Details.Per_100ML).Value =Val(dt1.Rows(0).Item("Total_100L_Mtrs").ToString)
                    .Rows(0).Cells(dgvCol_Details.gross_Weight).Value = Format(Val(dt1.Rows(0).Item("Total_Gross_weight").ToString), "########0.000")
                    .Rows(0).Cells(dgvCol_Details.TotalPoints).Value = Val(dt1.Rows(0).Item("total_points").ToString)

                End With

                dt2.Clear()
                dt2.Dispose()
                da2.Dispose()

            End If

            dt1.Clear()
            dt1.Dispose()
            da1.Dispose()

            If LockSTS = True Then

                cbo_PartyName.Enabled = False
                cbo_PartyName.BackColor = Color.LightGray

                cbo_Cloth.Enabled = False
                cbo_Cloth.BackColor = Color.LightGray

                cbo_ClothType.Enabled = False
                cbo_ClothType.BackColor = Color.LightGray

                txt_Folding.Enabled = False
                txt_Folding.BackColor = Color.LightGray

                'btn_Selection.Enabled = False

            End If

            Grid_Cell_DeSelect()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

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

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text)

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.ClothSales_Roll_Packing_Entry, New_Entry, Me, con, "Roll_Packing_Head", "Roll_Packing_Code", NewCode, "Roll_Packing_Date", "(Roll_Packing_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub

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

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Da = New SqlClient.SqlDataAdapter("select count(*) from Packing_Slip_Head Where Roll_Packing_Code = '" & Trim(NewCode) & "' and Packing_Slip_Code LIKE '" & Trim(Pk_Condition) & "%' and Delivery_Code <> ''", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                If Val(Dt1.Rows(0)(0).ToString) > 0 Then
                    MessageBox.Show("Already some rolls delivered/invoiced", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If
        Dt1.Clear()

        trans = con.BeginTransaction

        Try

            cmd.Connection = con
            cmd.Transaction = trans
            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "DELETE", "Roll_packing_head", "Roll_packing_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "", "", "Roll_packing_Code, Company_IdNo, for_OrderBy", trans)
            Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "DELETE", "Roll_packing_Details", "Roll_packing_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "Lot_No,Pcs_No,Weft_lot_no,Meters,Weight, Weight_Meter  ,Gross_Weight,p4,p3,Party_PieceNo,Pass_Meters,p1,P2,Warp_lot_no,Fabric_defect_penalty_point,Points,Point_Per_PassMeter,Grade,Roll_No,Lot_Code,Pcs_PartyIdNo,Pcs_Cloth_IdNo,Buyer_Offer_Code,Roll_Code,Loom_IdNo,Loom_No", "Sl_No", "Roll_packing_Code, For_OrderBy, Company_IdNo, Roll_packing_No, Roll_packing_Date, Ledger_Idno", trans)


            Da = New SqlClient.SqlDataAdapter("select * from Roll_Packing_Details Where Roll_Packing_Code = '" & Trim(NewCode) & "' Order by sl_no ", con)
            Da.SelectCommand.Transaction = trans
            Dt1 = New DataTable
            Da.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                For i = 0 To Dt1.Rows.Count - 1

                    cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type1 = '', PackingSlip_Inc_Type1 = PackingSlip_Inc_Type1 - 1, Roll_No_Type1 = '' Where PackingSlip_Code_Type1 = '" & Trim(Dt1.Rows(i).Item("Roll_Code").ToString) & "'"
                    cmd.ExecuteNonQuery()

                    cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type2 = '', PackingSlip_Inc_Type2 = PackingSlip_Inc_Type2 - 1, Roll_No_Type2 = '' Where PackingSlip_Code_Type2 = '" & Trim(Dt1.Rows(i).Item("Roll_Code").ToString) & "'"
                    cmd.ExecuteNonQuery()

                    cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type3 = '', PackingSlip_Inc_Type3 = PackingSlip_Inc_Type3 - 1, Roll_No_Type3 = '' Where PackingSlip_Code_Type3 = '" & Trim(Dt1.Rows(i).Item("Roll_Code").ToString) & "'"
                    cmd.ExecuteNonQuery()

                    cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type4 = '', PackingSlip_Inc_Type4 = PackingSlip_Inc_Type4 - 1, Roll_No_Type4 = '' Where PackingSlip_Code_Type4 = '" & Trim(Dt1.Rows(i).Item("Roll_Code").ToString) & "'"
                    cmd.ExecuteNonQuery()

                    cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type5 = '', PackingSlip_Inc_Type5 = PackingSlip_Inc_Type5 - 1, Roll_No_Type5 = '' Where PackingSlip_Code_Type5 = '" & Trim(Dt1.Rows(i).Item("Roll_Code").ToString) & "'"
                    cmd.ExecuteNonQuery()

                    cmd.CommandText = "delete from Packing_Slip_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Packing_Slip_Code = '" & Trim(Dt1.Rows(i).Item("Roll_Code").ToString) & "' and Roll_Packing_Code = '" & Trim(NewCode) & "'"
                    cmd.ExecuteNonQuery()

                    cmd.CommandText = "delete from Packing_Slip_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Packing_Slip_Code = '" & Trim(Dt1.Rows(i).Item("Roll_Code").ToString) & "' and Roll_Packing_Code = '" & Trim(NewCode) & "'"
                    cmd.ExecuteNonQuery()

                Next i
            End If
            Dt1.Clear()

            cmd.CommandText = "delete from Roll_Packing_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Roll_Packing_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Roll_Packing_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Roll_Packing_Code = '" & Trim(NewCode) & "'"
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

            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_Party.Text = ""
            cbo_Filter_Cloth.Text = ""
            txt_Filter_BuyerRefNo.Text = ""
            txt_Filter_LotNo.Text = ""
            txt_Filter_PcsNo.Text = ""
            txt_Filter_RollNo.Text = ""

            cbo_Filter_Party.SelectedIndex = -1
            cbo_Filter_Cloth.SelectedIndex = -1
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

            da = New SqlClient.SqlDataAdapter("select top 1 Roll_Packing_No from Roll_Packing_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Roll_Packing_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Roll_Packing_Code like '" & Trim(Pk_Condition) & "%' Order by for_Orderby, Roll_Packing_No", con)
            dt = New DataTable
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
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""
        Dim OrdByNo As Single = 0

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Roll_Packing_No from Roll_Packing_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Roll_Packing_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Roll_Packing_Code like '" & Trim(Pk_Condition) & "%' Order by for_Orderby, Roll_Packing_No", con)
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
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""
        Dim OrdByNo As Single = 0

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Roll_Packing_No from Roll_Packing_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Roll_Packing_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Roll_Packing_Code like '" & Trim(Pk_Condition) & "%' Order by for_Orderby desc, Roll_Packing_No desc", con)
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
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""

        Try
            da = New SqlClient.SqlDataAdapter("select top 1 Roll_Packing_No from Roll_Packing_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Roll_Packing_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Roll_Packing_Code like '" & Trim(Pk_Condition) & "%' Order by for_Orderby desc, Roll_Packing_No desc", con)
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
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewID As Integer = 0

        Try
            clear()

            New_Entry = True

            lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Roll_Packing_Head", "Roll_Packing_Code", "For_OrderBy", "(Roll_Packing_Code like '" & Trim(Pk_Condition) & "%')", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

            lbl_RefNo.ForeColor = Color.Red


            msk_date.Text = Date.Today.ToShortDateString
            da = New SqlClient.SqlDataAdapter("select top 1 * from Roll_Packing_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Roll_Packing_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Roll_Packing_Code like '" & Trim(Pk_Condition) & "%' Order by for_Orderby desc, Roll_Packing_No desc", con)
            dt1 = New DataTable
            da.Fill(dt1)
            If dt1.Rows.Count > 0 Then
                If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)
                    If dt1.Rows(0).Item("Roll_Packing_Date").ToString <> "" Then msk_date.Text = dt1.Rows(0).Item("Roll_Packing_Date").ToString
                End If

                cbo_PartyName.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Ledger_IdNo").ToString))
                cbo_Cloth.Text = Common_Procedures.Cloth_IdNoToName(con, Val(dt1.Rows(0).Item("Cloth_IdNo").ToString))
                cbo_ClothType.Text = Common_Procedures.ClothType_IdNoToName(con, Val(dt1.Rows(0).Item("ClothType_IdNo").ToString))
                If dt1.Rows(0).Item("Bale_Bundle").ToString <> "" Then
                    cbo_Bale_Bundle.Text = dt1.Rows(0).Item("Bale_Bundle").ToString
                End If
                If dt1.Rows(0).Item("Pcs_BufferOffer_Type").ToString <> "" Then
                    cbo_Type.Text = dt1.Rows(0).Item("Pcs_BufferOffer_Type").ToString
                End If

                txt_Folding.Text = Format(Val(dt1.Rows(0).Item("Folding").ToString), "########0.00")
                txt_Tareweight.Text = Format(Val(dt1.Rows(0).Item("Tare_Weight").ToString), "########0.000")

            End If
            dt1.Clear()


            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

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

            inpno = InputBox("Enter Ref.No.", "FOR FINDING...")

            RecCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Roll_Packing_No from Roll_Packing_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Roll_Packing_Code = '" & Trim(RecCode) & "'", con)
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
                MessageBox.Show("Ref No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim RecCode As String

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.PackinSlip_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.PackinSlip_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.ClothSales_Roll_Packing_Entry, New_Entry, Me) = False Then Exit Sub

        Try

            inpno = InputBox("Enter New Ref No.", "FOR NEW RECEIPT INSERTION...")

            RecCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Roll_Packing_No from Roll_Packing_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Roll_Packing_Code = '" & Trim(RecCode) & "'", con)
            Dt = New DataTable
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
                    MessageBox.Show("Invalid Roll No", "DOES NOT INSERT NEW REF NO...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_RefNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT NEW REF No...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim vNewRollCode As String = ""
        Dim vRollNo As String = ""
        Dim Clth_ID As Integer = 0
        Dim Clthty_ID As Integer = 0
        Dim dCloTyp_ID As Integer = 0
        Dim dClo_ID As Integer = 0
        Dim Trans_ID As Integer = 0
        Dim led_id As Integer = 0
        Dim EdsCnt_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim Partcls As String = ""
        Dim PBlNo As String = ""
        Dim vTotMtrs As Single, vTotPcs As Single, vTotWgt As Single
        Dim EntID As String = ""
        Dim dparty_ID As Integer = 0
        Dim vTotPassMtrs As Single = 0
        Dim vTotLessMtrs As Single = 0
        Dim vTotRejMtrs As Single = 0
        Dim vTotPts As Single = 0
        Dim Nr As Long = 0
        Dim vLmIdNo As Integer = 0
        Dim vLmNo As String = ""
        Dim vWgt_per_Mtr_fab As String = ""
        Dim vWgt_Mtr_MIn As String = ""
        Dim vWgt_Mtr_MAx As String = ""

        Dim vOrdByNo As String = ""

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text)


        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        'If Common_Procedures.UserRight_Check(Common_Procedures.UR.PackinSlip_Entry, New_Entry) = False Then Exit Sub

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.ClothSales_Roll_Packing_Entry, New_Entry, Me, con, "Roll_Packing_Head", "Roll_Packing_Code", NewCode, "Roll_Packing_Date", "(Roll_Packing_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Roll_Packing_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, Roll_Packing_No desc", dtp_Date.Value.Date) = False Then Exit Sub

        If Pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(msk_date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()
            Exit Sub
        End If

        If Not (Convert.ToDateTime(msk_date.Text) >= Common_Procedures.Company_FromDate And Convert.ToDateTime(msk_date.Text) <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()
            Exit Sub
        End If

        led_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyName.Text)
        If led_id = 0 Then
            MessageBox.Show("Invalid Ledger Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_PartyName.Enabled Then cbo_PartyName.Focus()
            Exit Sub
        End If

        Clth_ID = Common_Procedures.Cloth_NameToIdNo(con, cbo_Cloth.Text)
        If Clth_ID = 0 Then
            MessageBox.Show("Invalid Cloth Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Cloth.Enabled And cbo_Cloth.Visible Then cbo_Cloth.Focus()
            Exit Sub
        End If

        Clthty_ID = Common_Procedures.ClothType_NameToIdNo(con, cbo_ClothType.Text)
        If Clthty_ID = 0 Then
            MessageBox.Show("Invalid Cloth Type Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_ClothType.Enabled And cbo_ClothType.Visible Then cbo_ClothType.Focus()
            Exit Sub
        End If


        vWgt_Mtr_MIn = Common_Procedures.get_FieldValue(con, "cloth_head", "Weight_Meter_Min", "(cloth_idno = " & Str(Val(Clth_ID)) & ")")
        vWgt_Mtr_MAx = Common_Procedures.get_FieldValue(con, "cloth_head", "Weight_Meter_Max", "(cloth_idno = " & Str(Val(Clth_ID)) & ")")

        With dgv_Details

            For i = 0 To .RowCount - 1

                If Val(.Rows(i).Cells(dgvCol_Details.Totalmeters).Value) <> 0 Then

                    'If Trim(.Rows(i).Cells(7).Value) = "" Then
                    '    MessageBox.Show("Invalid Party Pcs.No", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    '    If .Enabled And .Visible Then
                    '        .Focus()
                    '        .CurrentCell = .Rows(i).Cells(7)
                    '    End If
                    '    Exit Sub
                    'End If

                    'If Trim(UCase(cbo_Type.Text)) = "BUYER-OFFER" Then
                    '    If Val(.Rows(i).Cells(dgvCol_Details.P3).Value) = 0 And Val(.Rows(i).Cells(dgvCol_Details.P2).Value) = 0 And Val(.Rows(i).Cells(dgvCol_Details.P1).Value) = 0 Then
                    '        MessageBox.Show("Invalid Pass/Less/Rejection Meters", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    '        If .Enabled And .Visible Then
                    '            .Focus()
                    '            .CurrentCell = .Rows(i).Cells(dgvCol_Details.P3)
                    '        End If
                    '        Exit Sub
                    '    End If
                    'End If

                    If Trim(.Rows(i).Cells(dgvCol_Details.Roll_No).Value) = "" Then
                        MessageBox.Show("Invalid Roll No", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(dgvCol_Details.Roll_No)
                        End If
                        Exit Sub
                    End If

                    If Val(.Rows(i).Cells(dgvCol_Details.Roll_No).Value) = 0 Then
                        MessageBox.Show("Invalid Roll No", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(dgvCol_Details.Roll_No)
                        End If
                        Exit Sub
                    End If

                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1352" Then '----Eminent Textile (Rajapalayam)
                        If Val(.Rows(i).Cells(dgvCol_Details.Net_Weight).Value) = 0 Then
                            MessageBox.Show("Invalid Net Weight", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                            If .Enabled And .Visible Then
                                .Focus()
                                .CurrentCell = .Rows(i).Cells(dgvCol_Details.Net_Weight)
                            End If
                            Exit Sub
                        End If
                    End If

                    If Val(.Rows(i).Cells(dgvCol_Details.Net_Weight).Value) <> 0 Then

                        If Val(vWgt_Mtr_MIn) <> 0 And Val(vWgt_Mtr_MAx) <> 0 Then

                            vWgt_per_Mtr_fab = Format(Val(.Rows(i).Cells(dgvCol_Details.Net_Weight).Value) / Val(.Rows(i).Cells(dgvCol_Details.Totalmeters).Value), "##########.000")
                            If Val(vWgt_per_Mtr_fab) < Val(vWgt_Mtr_MIn) Or Val(vWgt_per_Mtr_fab) > Val(vWgt_Mtr_MAx) Then
                                MessageBox.Show("Invalid Weight/Meter = " & vWgt_per_Mtr_fab & Chr(13) & "Min = " & vWgt_Mtr_MIn & "         Max = " & vWgt_Mtr_MAx, "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                                If .Enabled And .Visible Then
                                    .Focus()
                                    .CurrentCell = .Rows(i).Cells(dgvCol_Details.Roll_No)
                                End If
                                Exit Sub
                            End If

                        End If

                    End If


                End If

            Next

        End With

        vTotMtrs = 0 : vTotPcs = 0 : vTotWgt = 0
        vTotPassMtrs = 0 : vTotLessMtrs = 0 : vTotRejMtrs = 0 : vTotPts = 0
        If dgv_Details_Total.RowCount > 0 Then
            vTotMtrs = Val(dgv_Details_Total.Rows(0).Cells(dgvCol_Details.Totalmeters).Value())
            vTotPcs = Val(dgv_Details_Total.Rows(0).Cells(dgvCol_Details.P1).Value())
            vTotWgt = Val(dgv_Details_Total.Rows(0).Cells(dgvCol_Details.net_Weight).Value())

            vTotPassMtrs = Val(dgv_Details_Total.Rows(0).Cells(dgvCol_Details.P4).Value())
            vTotLessMtrs = Val(dgv_Details_Total.Rows(0).Cells(dgvCol_Details.p3).Value())
            vTotRejMtrs = Val(dgv_Details_Total.Rows(0).Cells(dgvCol_Details.p2).Value())
            vTotPts = Val(dgv_Details_Total.Rows(0).Cells(dgvCol_Details.TotalPoints).Value())

        End If

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Roll_Packing_Head", "Roll_Packing_Code", "For_OrderBy", "(Roll_Packing_Code like '" & Trim(Pk_Condition) & "%')", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@EntryDate", Convert.ToDateTime(msk_date.Text))

            If New_Entry = True Then

                cmd.CommandText = "Insert into Roll_Packing_Head (   Roll_Packing_Code   ,                 Company_IdNo     ,           Roll_Packing_No     ,                               for_OrderBy                              , Roll_Packing_Date,    Pcs_BufferOffer_Type      ,        Ledger_IdNo      ,             Cloth_IdNo   ,          ClothType_IdNo    ,                 Folding           ,               Bale_Bundle           ,  p1                        ,         Total_Meters       ,         Total_Weight      ,        p4                     ,          P3                   ,       p2                     ,         Note           ,                           User_IdNo            ,total_gross_Weight                                                               ,Total_points              ,Total_100L_mtrs ,  Tare_weight) " & _
                                    "          Values            ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",    @EntryDate    , '" & Trim(cbo_Type.Text) & "', " & Str(Val(led_id)) & ", " & Str(Val(Clth_ID)) & ", " & Str(Val(Clthty_ID)) & ", " & Str(Val(txt_Folding.Text)) & ", '" & Trim(cbo_Bale_Bundle.Text) & "', " & Str(Val(vTotPcs)) & ", " & Str(Val(vTotMtrs)) & " , " & Str(Val(vTotWgt)) & " , " & Str(Val(vTotPassMtrs)) & " , " & Str(Val(vTotLessMtrs)) & " , " & Str(Val(vTotRejMtrs)) & " , '" & Trim(txt_Note.Text) & "', " & Val(Common_Procedures.User.IdNo) & ", " & Val(dgv_Details_Total.Rows(0).Cells(dgvCol_Details.gross_Weight).Value()) & "," & Str(Val(vTotPts)) & "," & Val(dgv_Details_Total.Rows(0).Cells(dgvCol_Details.Per_100ML).Value()) & ",   " & Str(Val(txt_Tareweight.Text)) & " ) "
                cmd.ExecuteNonQuery()

            Else
                Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "OLD", "Roll_packing_head", "Roll_packing_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Roll_packing_Code, Company_IdNo, for_OrderBy", tr)
                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "OLD", "Roll_packing_Details", "Roll_packing_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "Lot_No,Pcs_No,Weft_lot_no,Meters,Weight, Weight_Meter  ,Gross_Weight,p4,p3,Party_PieceNo,Pass_Meters,p1,P2,Warp_lot_no,Fabric_defect_penalty_point,Points,Point_Per_PassMeter,Grade,Roll_No,Lot_Code,Pcs_PartyIdNo,Pcs_Cloth_IdNo,Buyer_Offer_Code,Roll_Code,Loom_IdNo,Loom_No", "Sl_No", "Roll_packing_Code, For_OrderBy, Company_IdNo, Roll_packing_No, Roll_packing_Date, Ledger_Idno", tr)

                cmd.CommandText = "Update Roll_Packing_Head set Roll_Packing_Date = @EntryDate, Ledger_IdNo = " & Str(Val(led_id)) & " , Pcs_BufferOffer_Type = '" & Trim(cbo_Type.Text) & "' , Cloth_IdNo = " & Str(Val(Clth_ID)) & " , ClothType_IdNo = " & Str(Val(Clthty_ID)) & " , Folding = " & Str(Val(txt_Folding.Text)) & ", Bale_Bundle = '" & Trim(cbo_Bale_Bundle.Text) & "', p1 = " & Str(Val(vTotPcs)) & ", Total_Meters = " & Str(Val(vTotMtrs)) & " , Total_Weight = " & Str(Val(vTotWgt)) & " , p4 = " & Str(Val(vTotPassMtrs)) & " , p3 = " & Str(Val(vTotLessMtrs)) & " , p2 = " & Str(Val(vTotRejMtrs)) & " , Total_Points = " & Str(Val(vTotPts)) & " , Note = '" & Trim(txt_Note.Text) & "' , User_IdNo = " & Val(Common_Procedures.User.IdNo) & " ,total_gross_Weight = " & Val(dgv_Details_Total.Rows(0).Cells(dgvCol_Details.gross_Weight).Value()) & " , Total_100L_mtrs=" & Val(dgv_Details_Total.Rows(0).Cells(dgvCol_Details.Per_100ML).Value()) & ",Tare_weight= " & Str(Val(txt_Tareweight.Text)) & " Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Roll_Packing_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()


                Da = New SqlClient.SqlDataAdapter("select * from Roll_Packing_Details Where Roll_Packing_Code = '" & Trim(NewCode) & "' Order by sl_no ", con)
                Da.SelectCommand.Transaction = tr
                Dt1 = New DataTable
                Da.Fill(Dt1)
                If Dt1.Rows.Count > 0 Then

                    For i = 0 To Dt1.Rows.Count - 1

                        cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type1 = '', PackingSlip_Inc_Type1 = PackingSlip_Inc_Type1 - 1, Roll_No_Type1 = '' Where PackingSlip_Code_Type1 = '" & Trim(Dt1.Rows(i).Item("Roll_Code").ToString) & "'"
                        cmd.ExecuteNonQuery()

                        cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type2 = '', PackingSlip_Inc_Type2 = PackingSlip_Inc_Type2 - 1, Roll_No_Type2 = '' Where PackingSlip_Code_Type2 = '" & Trim(Dt1.Rows(i).Item("Roll_Code").ToString) & "'"
                        cmd.ExecuteNonQuery()

                        cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type3 = '', PackingSlip_Inc_Type3 = PackingSlip_Inc_Type3 - 1, Roll_No_Type3 = '' Where PackingSlip_Code_Type3 = '" & Trim(Dt1.Rows(i).Item("Roll_Code").ToString) & "'"
                        cmd.ExecuteNonQuery()

                        cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type4 = '', PackingSlip_Inc_Type4 = PackingSlip_Inc_Type4 - 1, Roll_No_Type4 = '' Where PackingSlip_Code_Type4 = '" & Trim(Dt1.Rows(i).Item("Roll_Code").ToString) & "'"
                        cmd.ExecuteNonQuery()

                        cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type5 = '', PackingSlip_Inc_Type5 = PackingSlip_Inc_Type5 - 1, Roll_No_Type5 = '' Where PackingSlip_Code_Type5 = '" & Trim(Dt1.Rows(i).Item("Roll_Code").ToString) & "'"
                        cmd.ExecuteNonQuery()

                    Next i

                End If
                Dt1.Clear()

            End If
            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "NEW", "Roll_packing_head", "Roll_packing_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Roll_packing_Code, Company_IdNo, for_OrderBy", tr)
          
            EntID = Trim(Pk_Condition) & Trim(lbl_RefNo.Text)
            If Trim(lbl_RefNo.Text) <> "" Then
                Partcls = "Delv : Dc.No. " & Trim(lbl_RefNo.Text)
            End If
            PBlNo = Trim(lbl_RefNo.Text)

            cmd.CommandText = "Delete from Roll_Packing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Roll_Packing_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Packing_Slip_Details From Packing_Slip_Details a, Packing_Slip_Head b Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Roll_Packing_Code = '" & Trim(NewCode) & "' and b.Delivery_Code = '' and a.Packing_Slip_Code = b.Packing_Slip_Code and a.Roll_Packing_Code = b.Roll_Packing_Code"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Packing_Slip_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Roll_Packing_Code = '" & Trim(NewCode) & "' and Delivery_Code = ''"
            cmd.ExecuteNonQuery()

            With dgv_Details
                Sno = 0

                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(4).Value) <> 0 Then

                        dCloTyp_ID = 1 'Common_Procedures.ClothType_NameToIdNo(con, .Rows(i).Cells(dgvCol_Details.Weft_lot_no).Value, tr)
                        dparty_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, .Rows(i).Cells(dgvCol_Details.pcs_party_name).Value, tr)
                        dClo_ID = Common_Procedures.Cloth_NameToIdNo(con, .Rows(i).Cells(dgvCol_Details.pcs_cloth_name).Value, tr)

                        vLmNo = .Rows(i).Cells(dgvCol_Details.Loom_No).Value
                        vLmIdNo = Common_Procedures.Loom_NameToIdNo(con, .Rows(i).Cells(dgvCol_Details.Loom_No).Value, tr)

                        vNewRollCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "\" & Trim(.Rows(i).Cells(dgvCol_Details.Roll_No).Value) & "/" & Trim(Common_Procedures.FnYearCode)
                        'vNewRollCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(.Rows(i).Cells(16).Value) & "/" & Trim(Common_Procedures.FnYearCode)
                        .Rows(i).Cells(dgvCol_Details.roll_code).Value = vNewRollCode
                        vRollNo = Trim(.Rows(i).Cells(dgvCol_Details.Roll_No).Value)

                        'If Trim(.Rows(i).Cells(16).Value) = "3144" Then
                        '    Debug.Print(Trim(.Rows(i).Cells(16).Value))
                        'End If


                        Sno = Sno + 1

                        'cmd.CommandText = "Insert into Roll_Packing_Details (     Roll_Packing_Code   ,                 Company_IdNo     ,          Roll_Packing_No     ,                               for_OrderBy                              , Roll_Packing_Date ,        Ledger_IdNo      ,           Cloth_IdNo      ,          ClothType_IdNo     ,                  Folding           ,              Sl_No    ,                     Lot_No              ,                    Pcs_No              ,        Pcs_ClothType_IdNo   ,                      Meters              ,                      Weight              ,                      Weight_Meter        ,                    Buyer_Offer_No       ,                    Buyer_RefNo          ,                    Party_PieceNo        ,                      Pass_Meters          ,                      Less_Meters          ,                      Reject_Meters        ,                      Points               ,                    Point_Per_PassMeter    ,                    Grade                 ,                    Roll_No               ,                    Lot_Code              ,          Pcs_PartyIdNo      ,       Pcs_Cloth_IdNo     ,                Buyer_Offer_Code          ,                    Roll_Code             ,             Loom_IdNo     ,          Loom_No       ) " & _
                        '                    "          Values               ( '" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",     @EntryDate    , " & Str(Val(led_id)) & ",  " & Str(Val(Clth_ID)) & ",  " & Str(Val(Clthty_ID)) & ",  " & Str(Val(txt_Folding.Text)) & ",  " & Str(Val(Sno)) & ",  '" & Trim(.Rows(i).Cells(dgvCol_Details.LotNo).Value) & "', '" & Trim(.Rows(i).Cells(dgvCol_Details.PcsNo).Value) & "', " & Str(Val(dCloTyp_ID)) & ", " & Str(Val(.Rows(i).Cells(dgvCol_Details.Totalmeters).Value)) & ", " & Str(Val(.Rows(i).Cells(dgvCol_Details.net_Weight).Value)) & ", " & Str(Val(.Rows(i).Cells(dgvCol_Details.Gross_Weight).Value)) & ", '" & Trim(.Rows(i).Cells(dgvCol_Details.P4).Value) & "' , '" & Trim(.Rows(i).Cells(dgvCol_Details.P3).Value) & "' , '" & Trim(.Rows(i).Cells(dgvCol_Details.PcsNo).Value) & "' , " & Str(Val(.Rows(i).Cells(dgvCol_Details.P1).Value)) & ", " & Str(Val(.Rows(i).Cells(dgvCol_Details.Warp_Lot_no).Value)) & ", " & Str(Val(.Rows(i).Cells(dgvCol_Details.Fabric_Defect_details).Value)) & ", " & Str(Val(.Rows(i).Cells(dgvCol_Details.TotalPoints).Value)) & ", " & Str(Val(.Rows(i).Cells(dgvCol_Details.Per_100ML).Value)) & ", '" & Trim(.Rows(i).Cells(dgvCol_Details.fabric_grade).Value) & "' , '" & Trim(.Rows(i).Cells(dgvCol_Details.Roll_No).Value) & "' , '" & Trim(.Rows(i).Cells(dgvCol_Details.lot_code).Value) & "' , " & Str(Val(dparty_ID)) & " , " & Str(Val(dClo_ID)) & ", '" & Trim(.Rows(i).Cells(dgvCol_Details.buyer_offer_code).Value) & "' , '" & Trim(.Rows(i).Cells(dgvCol_Details.roll_code).Value) & "' , " & Str(Val(vLmIdNo)) & " , '" & Trim(vLmNo) & "'  ) "

                        cmd.CommandText = "Insert into Roll_Packing_Details (     Roll_Packing_Code   ,                 Company_IdNo     ,          Roll_Packing_No     ,                               for_OrderBy                              , Roll_Packing_Date ,        Ledger_IdNo      ,           Cloth_IdNo      ,          ClothType_IdNo     ,                  Folding           ,              Sl_No    ,                     Lot_No                                 ,                    Pcs_No                               ,                   Weft_lot_no                                  ,                      Meters                                       ,                      Weight                                        , Weight_Meter  ,                            Gross_Weight                            ,                      p4                                   ,                       p3                                  ,                    Party_PieceNo                           ,                   Pass_Meters                                          ,           p1                                             ,                       P2                                 ,                      Warp_lot_no                                ,                    Fabric_defect_penalty_point                            ,                      Points                                       ,                    Point_Per_PassMeter                          ,                    Grade                                       ,                    Roll_No                                      ,                    Lot_Code                                   ,          Pcs_PartyIdNo      ,       Pcs_Cloth_IdNo     ,                          Buyer_Offer_Code                             ,                    Roll_Code                                   ,             Loom_IdNo     ,          Loom_No       ) " & _
                                            "          Values               ( '" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",     @EntryDate    , " & Str(Val(led_id)) & ",  " & Str(Val(Clth_ID)) & ",  " & Str(Val(Clthty_ID)) & ",  " & Str(Val(txt_Folding.Text)) & ",  " & Str(Val(Sno)) & ",  '" & Trim(.Rows(i).Cells(dgvCol_Details.LotNo).Value) & "', '" & Trim(.Rows(i).Cells(dgvCol_Details.PcsNo).Value) & "', '" & Trim(.Rows(i).Cells(dgvCol_Details.Weft_lot_No).Value) & "', " & Str(Val(.Rows(i).Cells(dgvCol_Details.Totalmeters).Value)) & ", " & Str(Val(.Rows(i).Cells(dgvCol_Details.Net_Weight).Value)) & "  ,    0          , " & Str(Val(.Rows(i).Cells(dgvCol_Details.gross_Weight).Value)) & ", " & Str(Val(.Rows(i).Cells(dgvCol_Details.P4).Value)) & " , " & Str(Val(.Rows(i).Cells(dgvCol_Details.p3).Value)) & " , '" & Trim(.Rows(i).Cells(dgvCol_Details.PcsNo).Value) & "' ,    " & Str(Val(.Rows(i).Cells(dgvCol_Details.Totalmeters).Value)) & "  , " & Str(Val(.Rows(i).Cells(dgvCol_Details.P1).Value)) & ", " & Str(Val(.Rows(i).Cells(dgvCol_Details.p2).Value)) & ", '" & Trim(.Rows(i).Cells(dgvCol_Details.Warp_Lot_No).Value) & "', '" & Trim(.Rows(i).Cells(dgvCol_Details.fabric_defect_Details).Value) & "', " & Str(Val(.Rows(i).Cells(dgvCol_Details.TotalPoints).Value)) & ", " & Str(Val(.Rows(i).Cells(dgvCol_Details.Per_100ML).Value)) & ", '" & Trim(.Rows(i).Cells(dgvCol_Details.fabric_grade).Value) & "' , '" & Trim(.Rows(i).Cells(dgvCol_Details.Roll_No).Value) & "' , '" & Trim(.Rows(i).Cells(dgvCol_Details.lot_code).Value) & "' , " & Str(Val(dparty_ID)) & " , " & Str(Val(dClo_ID)) & ", '" & Trim(.Rows(i).Cells(dgvCol_Details.buyer_offer_code).Value) & "' , '" & Trim(.Rows(i).Cells(dgvCol_Details.roll_code).Value) & "' , " & Str(Val(vLmIdNo)) & " , '" & Trim(vLmNo) & "'  ) "
                        cmd.ExecuteNonQuery()

                        Nr = 0
                        cmd.CommandText = "Update Packing_Slip_Head set Packing_Slip_Date = @EntryDate, Ledger_IdNo = " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & " , Roll_Packing_Party_IdNo = " & Str(Val(led_id)) & " , Note = '" & Trim(txt_Note.Text) & "' , User_IdNo = " & Val(lbl_UserName.Text) & " Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Roll_Packing_Code = '" & Trim(NewCode) & "'  and Packing_Slip_Code = '" & Trim(vNewRollCode) & "'"
                        Nr = cmd.ExecuteNonQuery()
                        If Nr = 0 Then
                            cmd.CommandText = "Insert into Packing_Slip_Head ( Roll_Packing_Code     ,       Packing_Slip_Code     ,               Company_IdNo       ,                     Packing_Slip_No                         ,                               for_OrderBy                                                            , Packing_Slip_Date,                                             Ledger_IdNo    ,    Roll_Packing_Party_IdNo,     Pcs_BufferOffer_Type     ,             Cloth_IdNo   ,            ClothType_IdNo  ,              Bale_Bundle            ,                  Folding           , Total_Pcs ,                      Total_Meters                                  ,                      Total_Weight                                 ,                      Total_GrossWeight                              ,               Note           ,             User_IdNo          ) " & _
                                                "          Values            ('" & Trim(NewCode) & "', '" & Trim(vNewRollCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(.Rows(i).Cells(dgvCol_Details.Roll_No).Value) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(.Rows(i).Cells(dgvCol_Details.Roll_No).Value))) & ",      @EntryDate  ,  " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & ", " & Str(Val(led_id)) & "  , '" & Trim(cbo_Type.Text) & "', " & Str(Val(Clth_ID)) & ", " & Str(Val(Clthty_ID)) & ", '" & Trim(cbo_Bale_Bundle.Text) & "',  " & Str(Val(txt_Folding.Text)) & ",     1     , " & Str(Val(.Rows(i).Cells(dgvCol_Details.Totalmeters).Value)) & " , " & Str(Val(.Rows(i).Cells(dgvCol_Details.Net_Weight).Value)) & " , " & Str(Val(.Rows(i).Cells(dgvCol_Details.gross_Weight).Value)) & " , '" & Trim(txt_Note.Text) & "', " & Val(lbl_UserName.Text) & " ) "
                            cmd.ExecuteNonQuery()
                        End If

                        vWgt_per_Mtr_fab = Format(Val(.Rows(i).Cells(dgvCol_Details.Net_Weight).Value) / Val(.Rows(i).Cells(dgvCol_Details.Totalmeters).Value), "##########.000")

                        Nr = 0
                        cmd.CommandText = "Update Packing_Slip_Details set Packing_Slip_Date = @EntryDate Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Roll_Packing_Code = '" & Trim(NewCode) & "'  and Packing_Slip_Code = '" & Trim(vNewRollCode) & "' and Lot_Code = '" & Trim(.Rows(i).Cells(17).Value) & "' and Pcs_No = '" & Trim(.Rows(i).Cells(2).Value) & "'"
                        Nr = cmd.ExecuteNonQuery()
                        If Nr = 0 Then
                            cmd.CommandText = "Insert into Packing_Slip_Details (     Roll_Packing_Code  ,       Packing_Slip_Code       ,           Company_IdNo           ,                           Packing_Slip_No                    ,                               for_OrderBy                                                           , Packing_Slip_Date,          Cloth_IdNo      ,                  Folding           ,  Sl_No ,                     Lot_No                                 ,                    Pcs_No                                 ,           ClothType_IdNo    ,                      Meters                                       ,                      Weight                                      ,                      Gross_Weight                                  ,              Weight_Meter          ,            Party_IdNo       ,                    Lot_Code                                   ,             Loom_IdNo     ,          Loom_No      ) " & _
                                                "           Values              ( '" & Trim(NewCode) & "', '" & Trim(vNewRollCode) & "'  , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(.Rows(i).Cells(dgvCol_Details.Roll_No).Value) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(.Rows(i).Cells(dgvCol_Details.Roll_No).Value))) & ",     @EntryDate   , " & Str(Val(dClo_ID)) & ",  " & Str(Val(txt_Folding.Text)) & ",    1   ,  '" & Trim(.Rows(i).Cells(dgvCol_Details.LotNo).Value) & "', '" & Trim(.Rows(i).Cells(dgvCol_Details.PcsNo).Value) & "', " & Str(Val(dCloTyp_ID)) & ", " & Str(Val(.Rows(i).Cells(dgvCol_Details.Totalmeters).Value)) & ", " & Str(Val(.Rows(i).Cells(dgvCol_Details.Net_Weight).Value)) & ", " & Str(Val(.Rows(i).Cells(dgvCol_Details.gross_Weight).Value)) & ", " & Str(Val(vWgt_per_Mtr_fab)) & " , " & Str(Val(dparty_ID)) & " , '" & Trim(.Rows(i).Cells(dgvCol_Details.lot_code).Value) & "' , " & Str(Val(vLmIdNo)) & " , '" & Trim(vLmNo) & "' ) "
                            cmd.ExecuteNonQuery()
                        End If


                        If dCloTyp_ID = 1 Then
                            Nr = 0
                            cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type1 = '" & Trim(vNewRollCode) & "', PackingSlip_Inc_Type1 = PackingSlip_Inc_Type1 + 1, Roll_No_Type1 = '" & Trim(.Rows(i).Cells(dgvCol_Details.Roll_No).Value) & "' Where lot_code = '" & Trim(.Rows(i).Cells(dgvCol_Details.lot_code).Value) & "' and Piece_No = '" & Trim(.Rows(i).Cells(dgvCol_Details.PcsNo).Value) & "' and PackingSlip_Code_Type1 = ''"
                            Nr = cmd.ExecuteNonQuery()
                            If Nr = 0 Then
                                Throw New ApplicationException("Mismatch of Piece Details")
                                Exit Sub
                            End If

                        ElseIf dCloTyp_ID = 2 Then
                            Nr = 0
                            cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type2 = '" & Trim(vNewRollCode) & "', PackingSlip_Inc_Type2 = PackingSlip_Inc_Type2 + 1, Roll_No_Type2 = '" & Trim(.Rows(i).Cells(dgvCol_Details.Roll_No).Value) & "' Where lot_code = '" & Trim(.Rows(i).Cells(dgvCol_Details.lot_code).Value) & "' and Piece_No = '" & Trim(.Rows(i).Cells(dgvCol_Details.PcsNo).Value) & "' and PackingSlip_Code_Type2 = ''"
                            Nr = cmd.ExecuteNonQuery()
                            If Nr = 0 Then
                                Throw New ApplicationException("Mismatch of Piece Details")
                                Exit Sub
                            End If

                        ElseIf dCloTyp_ID = 3 Then
                            Nr = 0
                            cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type3 = '" & Trim(vNewRollCode) & "', PackingSlip_Inc_Type3 = PackingSlip_Inc_Type3 + 1, Roll_No_Type3 = '" & Trim(.Rows(i).Cells(dgvCol_Details.Roll_No).Value) & "' Where lot_code = '" & Trim(.Rows(i).Cells(dgvCol_Details.lot_code).Value) & "' and Piece_No = '" & Trim(.Rows(i).Cells(dgvCol_Details.PcsNo).Value) & "' and PackingSlip_Code_Type3 = ''"
                            Nr = cmd.ExecuteNonQuery()
                            If Nr = 0 Then
                                Throw New ApplicationException("Mismatch of Piece Details")
                                Exit Sub
                            End If

                        ElseIf dCloTyp_ID = 4 Then
                            Nr = 0
                            cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type4 = '" & Trim(vNewRollCode) & "', PackingSlip_Inc_Type4 = PackingSlip_Inc_Type4 + 1, Roll_No_Type4 = '" & Trim(.Rows(i).Cells(dgvCol_Details.Roll_No).Value) & "' Where lot_code = '" & Trim(.Rows(i).Cells(dgvCol_Details.lot_code).Value) & "' and Piece_No = '" & Trim(.Rows(i).Cells(dgvCol_Details.PcsNo).Value) & "' and PackingSlip_Code_Type4 = ''"
                            Nr = cmd.ExecuteNonQuery()
                            If Nr = 0 Then
                                Throw New ApplicationException("Mismatch of Piece Details")
                                Exit Sub
                            End If

                        ElseIf dCloTyp_ID = 5 Then
                            Nr = 0
                            cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type5 = '" & Trim(vNewRollCode) & "', PackingSlip_Inc_Type5 = PackingSlip_Inc_Type5 + 1, Roll_No_Type5 = '" & Trim(.Rows(i).Cells(dgvCol_Details.Roll_No).Value) & "' Where lot_code = '" & Trim(.Rows(i).Cells(dgvCol_Details.lot_code).Value) & "' and Piece_No = '" & Trim(.Rows(i).Cells(dgvCol_Details.PcsNo).Value) & "' and PackingSlip_Code_Type5 = ''"
                            Nr = cmd.ExecuteNonQuery()
                            If Nr = 0 Then
                                Throw New ApplicationException("Mismatch of Piece Details")
                                Exit Sub
                            End If

                        End If

                    End If

                Next
                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "DELETE", "Roll_packing_Details", "Roll_packing_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "Lot_No,Pcs_No,Weft_lot_no,Meters,Weight, Weight_Meter  ,Gross_Weight,p4,p3,Party_PieceNo,Pass_Meters,p1,P2,Warp_lot_no,Fabric_defect_penalty_point,Points,Point_Per_PassMeter,Grade,Roll_No,Lot_Code,Pcs_PartyIdNo,Pcs_Cloth_IdNo,Buyer_Offer_Code,Roll_Code,Loom_IdNo,Loom_No", "Sl_No", "Roll_packing_Code, For_OrderBy, Company_IdNo, Roll_packing_No, Roll_packing_Date, Ledger_Idno", tr)

            End With

            tr.Commit()

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
            If InStr(1, Trim(UCase(ex.Message)), Trim(UCase("PK_Packing_Slip_Head"))) > 0 Then
                MessageBox.Show("Duplicate Roll No in this Entry - " & Trim(vRollNo), "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            ElseIf InStr(1, Trim(UCase(ex.Message)), Trim(UCase("PK_Packing_Slip_Details"))) > 0 Then
                MessageBox.Show("Duplicate Roll No in this Entry - " & Trim(vRollNo), "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Else
                MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            End If


        Finally
            Dt1.Dispose()
            Da.Dispose()
            tr.Dispose()
            cmd.Dispose()

            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()

        End Try

    End Sub

    Private Sub Total_Calculation()
        Dim Sno As Integer
        Dim TotMtrs As Single, TotPcs As Single, TotWgt As Single, Totgrswgt As Single
        Dim TotPassMtrs As Single, tot_100Lmtr As Single, p1 As Single, TotPts As Single, p2 As Single, p3 As Single, p4 As Single




        Try
            If FrmLdSTS = True Then Exit Sub

            Sno = 0
            TotPcs = 0
            TotMtrs = 0
            TotWgt = 0
            TotPassMtrs = 0
            tot_100Lmtr = 0

            TotPts = 0

 


            With dgv_Details
                For i = 0 To .RowCount - 1
                    Sno = Sno + 1
                    .Rows(i).Cells(0).Value = Sno





  

                    If Val(.Rows(i).Cells(dgvCol_Details.Totalmeters).Value) <> 0 Then

                        ' TotPcs = TotPcs + 1
                        TotMtrs = TotMtrs + Val(.Rows(i).Cells(dgvCol_Details.Totalmeters).Value)
                        TotWgt = TotWgt + Val(.Rows(i).Cells(dgvCol_Details.Net_Weight).Value)
                        tot_100Lmtr = tot_100Lmtr + Trim(Format(Val(.Rows(i).Cells(dgvCol_Details.Per_100ML).Value), "#######0"))
                        TotPts = TotPts + Val(.Rows(i).Cells(dgvCol_Details.TotalPoints).Value)
                        p1 = p1 + Val(.Rows(i).Cells(dgvCol_Details.P1).Value)
                        p2 = p2 + Val(.Rows(i).Cells(dgvCol_Details.p2).Value)
                        p3 = p3 + Val(.Rows(i).Cells(dgvCol_Details.p3).Value)
                        p4 = p4 + Val(.Rows(i).Cells(dgvCol_Details.P4).Value)

                        Totgrswgt = Totgrswgt + Val(.Rows(i).Cells(dgvCol_Details.gross_Weight).Value)


                    End If
                Next




            End With

            tot_100Lmtr = Format(Val(tot_100Lmtr / Sno), "#####0")

            With dgv_Details_Total
                If .RowCount = 0 Then .Rows.Add()
                '.Rows(0).Cells(dgvCol_Details.PcsNo).Value = Val(TotPcs)
                .Rows(0).Cells(dgvCol_Details.Totalmeters).Value = Format(Val(TotMtrs), "########0.00")
                .Rows(0).Cells(dgvCol_Details.Net_Weight).Value = Format(Val(TotWgt), "########0.000")
                .Rows(0).Cells(dgvCol_Details.Per_100ML).Value = Trim(Val(tot_100Lmtr))

                .Rows(0).Cells(dgvCol_Details.P1).Value = Val(p1)
                '             
                .Rows(0).Cells(dgvCol_Details.p2).Value = Val(p2)
                .Rows(0).Cells(dgvCol_Details.p3).Value = Val(p3)
                .Rows(0).Cells(dgvCol_Details.P4).Value = Val(p4)

                .Rows(0).Cells(dgvCol_Details.TotalPoints).Value = Val(TotPts)
                .Rows(0).Cells(dgvCol_Details.gross_Weight).Value = Val(Totgrswgt)
            End With

        Catch ex As Exception
            '---
        End Try
    End Sub

    Private Sub cbo_Cloth_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Cloth.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "", "Cloth_Name")

    End Sub
    Private Sub cbo_Cloth_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Cloth.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Cloth, cbo_Type, cbo_ClothType, "Cloth_Head", "Cloth_Name", "", "Cloth_Name")

    End Sub

    Private Sub cbo_Cloth_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Cloth.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Cloth, cbo_ClothType, "Cloth_Head", "Cloth_Name", "", "Cloth_Name")
    End Sub

    Private Sub cbo_Cloth_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Cloth.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New Cloth_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Cloth.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""
            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_ClothType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ClothType.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "ClothType_Head", "ClothType_Name", "", "ClothType_Name")

    End Sub
    Private Sub cbo_ClothType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ClothType.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_ClothType, cbo_Cloth, cbo_Bale_Bundle, "ClothType_Head", "ClothType_Name", "", "ClothType_Name")

    End Sub

    Private Sub cbo_ClothType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_ClothType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_ClothType, cbo_Bale_Bundle, "ClothType_Head", "ClothType_Name", "", "ClothType_Name")
    End Sub

    Private Sub cbo_Bale_Bundle_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Bale_Bundle.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")

    End Sub
    Private Sub cbo_Bale_Bundle_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Bale_Bundle.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Bale_Bundle, cbo_ClothType, txt_Folding, "", "", "", "")

    End Sub

    Private Sub cbo_Bale_Bundle_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Bale_Bundle.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Bale_Bundle, txt_Folding, "", "", "", "")
    End Sub

    Private Sub txt_Folding_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Folding.KeyDown
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCol_Details.Roll_No)
                dgv_Details.CurrentCell.Selected = True
            Else
                txt_Tareweight.Focus()
                '  txt_Note.Focus()
            End If
        End If
    End Sub

    Private Sub txt_Folding_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Folding.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
        If Asc(e.KeyChar) = 13 Then

            If MessageBox.Show("Do you want to select Piece", "FOR PIECE SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                btn_Selection_Click(sender, e)

            Else

                If dgv_Details.Rows.Count > 0 Then
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCol_Details.Roll_No)
                    dgv_Details.CurrentCell.Selected = True
                Else
                    '  txt_Note.Focus()
                    txt_Tareweight.Focus()
                End If

            End If

        End If
    End Sub

    Private Sub txt_Note_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Note.KeyDown
        If (e.KeyValue = 40) Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                msk_date.Focus()
            End If
        End If

        If (e.KeyValue = 38) Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCol_Details.Roll_No)
                dgv_Details.CurrentCell.Selected = True
            Else
                ' txt_Folding.Focus()
                txt_Tareweight.Focus()
            End If
        End If

    End Sub

    Private Sub txt_Note_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Note.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                msk_date.Focus()
            End If
        End If
    End Sub

    Private Sub dgv_Details_CellEnter(ByVal sender As Object, ByVal e As DataGridViewCellEventArgs) Handles dgv_Details.CellEnter
        Dim vTare_Wgt As String = 0

        Try

            If FrmLdSTS = True Then Exit Sub

            With dgv_Details
                If .Visible Then
                    If .Rows.Count > 0 Then

                        If e.ColumnIndex = dgvCol_Details.Roll_No Then
                            If Trim(.Rows(e.RowIndex).Cells(dgvCol_Details.bale_delivery_code).Value) <> "" Then
                                .Rows(e.RowIndex).Cells(e.ColumnIndex).ReadOnly = True
                            Else
                                .Rows(e.RowIndex).Cells(e.ColumnIndex).ReadOnly = False
                            End If
                        End If

                        If e.ColumnIndex = dgvCol_Details.gross_Weight Then
                            If Val(.Rows(e.RowIndex).Cells(dgvCol_Details.gross_Weight).Value) = 0 And Val(.Rows(e.RowIndex).Cells(dgvCol_Details.Net_Weight).Value) <> 0 Then
                                vTare_Wgt = Val(txt_Tareweight.Text)
                                .Rows(e.RowIndex).Cells(dgvCol_Details.gross_Weight).Value = Val(Val(.Rows(e.RowIndex).Cells(dgvCol_Details.Net_Weight).Value) + Val(vTare_Wgt))
                            End If
                        End If

                        If e.ColumnIndex = dgvCol_Details.fabric_grade Then

                            If Trim(.Rows(e.RowIndex).Cells(dgvCol_Details.fabric_grade).Value) = "" Then
                                If Val(.Rows(e.RowIndex).Cells(dgvCol_Details.Per_100ML).Value) >= 22 Then
                                    .Rows(e.RowIndex).Cells(dgvCol_Details.fabric_grade).Value = "D"
                                ElseIf Val(.Rows(e.RowIndex).Cells(dgvCol_Details.Per_100ML).Value) >= 15 Then
                                    .Rows(e.RowIndex).Cells(dgvCol_Details.fabric_grade).Value = "C"
                                ElseIf Val(.Rows(e.RowIndex).Cells(dgvCol_Details.Per_100ML).Value) >= 8 Then
                                    .Rows(e.RowIndex).Cells(dgvCol_Details.fabric_grade).Value = "B"
                                Else
                                    .Rows(e.RowIndex).Cells(dgvCol_Details.fabric_grade).Value = "A"
                                End If
                            End If




                        End If

                    End If
                End If
            End With

        Catch ex As Exception
            '---

        End Try

    End Sub

    Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged
        Dim Vp1 As Single, Vp2 As Single, Vp3 As Single, Vp4 As Single

        Dim Vmtr As Single, Vpoint As Single

        Dim vAvg_Points As String = 0
        Dim vTare_Wgt As String = 0
        Dim vGrs_Wgt As String = 0



        Try

            If FrmLdSTS = True Then Exit Sub
            If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
            With dgv_Details

                If .Visible Then

                    If .Rows.Count > 0 Then



                        If e.ColumnIndex = dgvCol_Details.Totalmeters Or e.ColumnIndex = dgvCol_Details.TotalPoints Or e.ColumnIndex = dgvCol_Details.Net_Weight Or e.ColumnIndex = dgvCol_Details.P1 Or e.ColumnIndex = dgvCol_Details.p2 Or e.ColumnIndex = dgvCol_Details.p3 Or e.ColumnIndex = dgvCol_Details.P4 Then

                            Vp1 = Val(.Rows(e.RowIndex).Cells(dgvCol_Details.P1).Value)
                            Vp2 = Val(.Rows(e.RowIndex).Cells(dgvCol_Details.p2).Value) * 2
                            Vp3 = Val(.Rows(e.RowIndex).Cells(dgvCol_Details.p3).Value) * 3
                            Vp4 = Val(.Rows(e.RowIndex).Cells(dgvCol_Details.P4).Value) * 4

                            .Rows(e.RowIndex).Cells(dgvCol_Details.TotalPoints).Value = Val(Vp1 + Vp2 + Vp3 + Vp4)

                            Vmtr = Val(.Rows(e.RowIndex).Cells(dgvCol_Details.Totalmeters).Value)
                            Vpoint = Val(.Rows(e.RowIndex).Cells(dgvCol_Details.TotalPoints).Value)

                            vAvg_Points = Format((Vpoint / Vmtr) * 100, "########0.0")
                            .Rows(e.RowIndex).Cells(dgvCol_Details.Per_100ML).Value = Val(vAvg_Points)

                            If Val(.Rows(e.RowIndex).Cells(dgvCol_Details.Net_Weight).Value) <> 0 Then
                                vTare_Wgt = Val(txt_Tareweight.Text)
                                vGrs_Wgt = Val(Val(.Rows(e.RowIndex).Cells(dgvCol_Details.Net_Weight).Value) + Val(vTare_Wgt))
                                If Val(vGrs_Wgt) <> 0 Then
                                    .Rows(e.RowIndex).Cells(dgvCol_Details.gross_Weight).Value = Common_Procedures.Meter_RoundOff(Val(vGrs_Wgt))
                                Else
                                    .Rows(e.RowIndex).Cells(dgvCol_Details.gross_Weight).Value = ""
                                End If
                            End If

                            Total_Calculation()

                        End If

                        If e.ColumnIndex = dgvCol_Details.Per_100ML Then
                            If Val(.Rows(e.RowIndex).Cells(dgvCol_Details.Per_100ML).Value) >= 22 Then
                                .Rows(e.RowIndex).Cells(dgvCol_Details.fabric_grade).Value = "D"
                            ElseIf Val(.Rows(e.RowIndex).Cells(dgvCol_Details.Per_100ML).Value) >= 15 Then
                                .Rows(e.RowIndex).Cells(dgvCol_Details.fabric_grade).Value = "C"
                            ElseIf Val(.Rows(e.RowIndex).Cells(dgvCol_Details.Per_100ML).Value) >= 8 Then
                                .Rows(e.RowIndex).Cells(dgvCol_Details.fabric_grade).Value = "B"
                            Else
                                .Rows(e.RowIndex).Cells(dgvCol_Details.fabric_grade).Value = "A"
                            End If
                        End If

                    End If
                End If
                
            End With

        Catch ex As Exception
            MsgBox(ex.Message)

            '---

        End Try
    End Sub

    Private Sub dgv_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Details.EditingControlShowing
        dgtxt_Details = CType(dgv_Details.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgv_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyUp
        Dim i As Integer = 0
        Dim n As Integer = 0
        Dim S As String = ""

        Try

            If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

                With dgv_Details

                    n = .CurrentRow.Index

                    If Trim(.Rows(n).Cells(dgvCol_Details.bale_delivery_code).Value) = "" Then

                        If .Rows.Count <= 1 Then
                            For i = 0 To .ColumnCount - 1
                                .Rows(n).Cells(i).Value = ""
                            Next

                        Else
                            .Rows.RemoveAt(n)

                        End If

                        For i = 0 To .Rows.Count - 1
                            .Rows(i).Cells(0).Value = i + 1
                        Next

                        Total_Calculation()

                    Else

                        S = ""
                        If Trim(.Rows(n).Cells(dgvCol_Details.bale_delivery_code).Value) <> "" Then
                            S = " Already Rolls Delivered/Invoiced = " & Trim(.Rows(n).Cells(dgvCol_Details.bale_delivery_code).Value)
                        End If
                        MessageBox.Show(S, "DOES NOT REMOVE THIS ROLL...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        Exit Sub

                    End If

                End With

            End If

        Catch ex As Exception
            '------------

        End Try

    End Sub

    Private Sub dgv_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.LostFocus
        On Error Resume Next
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_Details_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Details.RowsAdded
        Dim n As Integer = 0

        Try
            If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
            With dgv_Details
                n = .RowCount
                .Rows(n - 1).Cells(dgvCol_Details.SlNo).Value = Val(n)
            End With
        Catch ex As Exception
            '---
        End Try
    End Sub

    Private Sub btn_close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub btn_save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.Enter
        dgv_Details.EditingControl.BackColor = Color.Lime
        dgv_Details.EditingControl.ForeColor = Color.Blue
        dgtxt_Details.SelectAll()
    End Sub

    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Details.KeyPress
        With dgv_Details
            If .Visible Then
                If .CurrentCell.ColumnIndex = dgvCol_Details.P1 Or .CurrentCell.ColumnIndex = dgvCol_Details.p2 Or .CurrentCell.ColumnIndex = dgvCol_Details.p3 Or .CurrentCell.ColumnIndex = dgvCol_Details.P4 Or .CurrentCell.ColumnIndex = dgvCol_Details.Net_Weight Or .CurrentCell.ColumnIndex = dgvCol_Details.gross_Weight Then
                    If Common_Procedures.Accept_NumericPositiveOnly(Asc(e.KeyChar)) = 0 Then
                        e.Handled = True
                    End If
                End If
            End If
        End With
    End Sub

    Private Sub dgtxt_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyUp
        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            dgv_Details_KeyUp(sender, e)
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
        Dim Led_IdNo As Integer = 0
        Dim Clo_IdNo As Integer = 0
        Dim ClTyp_IdNo As Integer = 0
        Dim Condt As String = ""

        Try

            Condt = ""
            Led_IdNo = 0
            Clo_IdNo = 0
            ClTyp_IdNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Roll_Packing_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Roll_Packing_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Roll_Packing_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_Party.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_Party.Text)
            End If
            If Trim(cbo_Filter_Cloth.Text) <> "" Then
                Clo_IdNo = Common_Procedures.Cloth_NameToIdNo(con, cbo_Filter_Cloth.Text)
            End If

            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Ledger_IdNo = " & Str(Val(Led_IdNo))
            End If
            If Val(Clo_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Cloth_IdNo = " & Str(Val(Clo_IdNo))
            End If

            If Trim(txt_Filter_LotNo.Text) <> "" Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Roll_Packing_Code IN (select z1.Roll_Packing_Code from Roll_Packing_Details z1 where z1.Lot_No = '" & Trim(txt_Filter_LotNo.Text) & "') "
            End If
            If Trim(txt_Filter_PcsNo.Text) <> "" Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Roll_Packing_Code IN (select z2.Roll_Packing_Code from Roll_Packing_Details z2 where z2.Pcs_No = '" & Trim(txt_Filter_PcsNo.Text) & "') "
            End If
            If Trim(txt_Filter_RollNo.Text) <> "" Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Roll_Packing_Code IN (select z3.Roll_Packing_Code from Roll_Packing_Details z3 where z3.Roll_No = '" & Trim(txt_Filter_RollNo.Text) & "') "
            End If
            If Trim(txt_Filter_BuyerRefNo.Text) <> "" Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Roll_Packing_Code IN (select z4.Roll_Packing_Code from Roll_Packing_Details z4 where z4.Buyer_RefNo = '" & Trim(txt_Filter_BuyerRefNo.Text) & "') "
            End If

            da = New SqlClient.SqlDataAdapter("select a.*, b.Cloth_name, c.Ledger_name from Roll_Packing_Head a INNER JOIN Cloth_Head b on a.cloth_idno <> 0 and a.cloth_idno = b.cloth_idno INNER JOIN Ledger_Head c on a.ledger_idno <> 0 and a.ledger_idno = c.ledger_idno where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Roll_Packing_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "'  and a.Roll_Packing_Code LIKE '" & Trim(Pk_Condition) & "%' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Roll_Packing_No", con)
            dt2 = New DataTable
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Roll_Packing_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Roll_Packing_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Ledger_name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = dt2.Rows(i).Item("Cloth_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(5).Value = Val(dt2.Rows(i).Item("Total_Pcs").ToString)
                    dgv_Filter_Details.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Total_Meters").ToString), "########0.00")
                    dgv_Filter_Details.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Total_Weight").ToString), "########0.000")

                Next i

            End If

            dt2.Clear()
            dt2.Dispose()
            da.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FILTER...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        If dgv_Filter_Details.Visible And dgv_Filter_Details.Enabled Then dgv_Filter_Details.Focus()

    End Sub

    Private Sub dtp_Filter_Fromdate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Filter_Fromdate.KeyDown
        On Error Resume Next
        If e.KeyValue = 38 Then e.Handled = True ': SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then e.Handled = True : SendKeys.Send("{TAB}")
    End Sub


    Private Sub dtp_Filter_Fromdate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_Filter_Fromdate.KeyPress
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            dtp_Filter_ToDate.Focus()
        End If

    End Sub

    Private Sub dtp_Filter_ToDate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Filter_ToDate.KeyDown
        On Error Resume Next
        If e.KeyValue = 38 Then e.Handled = True : SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then e.Handled = True : SendKeys.Send("{TAB}")
    End Sub

    Private Sub dtp_Filter_ToDate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_Filter_ToDate.KeyPress

        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            cbo_Filter_Party.Focus()
        End If
    End Sub

    Private Sub cbo_Filter_Cloth_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_Cloth.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "", "Cloth_Name")
    End Sub

    Private Sub cbo_Filter_Cloth_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_Cloth.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_Cloth, cbo_Filter_Party, txt_Filter_BuyerRefNo, "Cloth_Head", "Cloth_Name", "", "Cloth_Name")
    End Sub

    Private Sub cbo_Filter_Cloth_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_Cloth.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_Cloth, txt_Filter_BuyerRefNo, "Cloth_Head", "Cloth_Name", "", "Cloth_Name")

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

    Private Sub chk_SelectAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chk_SelectAll.CheckedChanged
        Dim i As Integer
        Dim J As Integer

        With dgv_Selection

            For i = 0 To .Rows.Count - 1
                .Rows(i).Cells(dgvCol_Details.Warp_Lot_no).Value = ""
                For J = 0 To .ColumnCount - 1
                    .Rows(i).Cells(J).Style.ForeColor = Color.Black
                Next J
            Next i

            If chk_SelectAll.Checked = True Then
                For i = 0 To .Rows.Count - 1
                    Select_Piece(i)
                Next i
            End If

            If .Rows.Count > 0 Then
                .Focus()
                .CurrentCell = .Rows(0).Cells(dgvCol_Details.SlNo)
                .CurrentCell.Selected = True
            End If

        End With

    End Sub

    Private Sub btn_Selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Selection.Click
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim i As Integer, j As Integer, n As Integer, SNo As Integer
        Dim CloIdNo As Integer, CloTypIdNo As Integer
        Dim NewCode As String
        Dim CompIDCondt As String
        Dim StkOf_IdNo As Integer = 0
        Dim led_id As Integer = 0
        Dim Cnt_GrpIdNos As String = ""
        Dim Cnt_IdNo As Integer, Cnt_UndIdNo As Integer
        Dim Cnt_Cond As String = ""
        Dim BuyrOffrCondt As String = ""
        Dim PcsMtrs As Single = 0
        Dim vBaleDelvCd As String = ""
        Dim vLmIdNo As Long = 0
        Dim vLmNo As String = ""


        StkOf_IdNo = Common_Procedures.CommonLedger.Godown_Ac

        led_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyName.Text)

        CloIdNo = Common_Procedures.Cloth_NameToIdNo(con, cbo_Cloth.Text)
        If CloIdNo = 0 Then
            MessageBox.Show("Invalid Cloth Name", "DOES NOT SELECT PAVU...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Cloth.Enabled And cbo_Cloth.Visible Then cbo_Cloth.Focus()
            Exit Sub
        End If

        CloTypIdNo = Common_Procedures.ClothType_NameToIdNo(con, cbo_ClothType.Text)
        If CloTypIdNo = 0 Then
            MessageBox.Show("Invalid Cloth Type", "DOES NOT SELECT PAVU...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_ClothType.Enabled And cbo_ClothType.Visible Then cbo_ClothType.Focus()
            Exit Sub
        End If

        If Val(txt_Folding.Text) = 0 Then
            MessageBox.Show("Invalid Folding", "DOES NOT SELECT PAVU...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If txt_Folding.Enabled And txt_Folding.Visible Then txt_Folding.Focus()
            Exit Sub
        End If

        CompIDCondt = "(a.company_idno = " & Str(Val(lbl_Company.Tag)) & ")"
        If Common_Procedures.settings.EntrySelection_Combine_AllCompany = 1 Then
            CompIDCondt = ""
            If Trim(UCase(Common_Procedures.User.Type)) = "ACCOUNT" Then
                CompIDCondt = "(tZ.Company_Type <> 'UNACCOUNT')"
            End If
        End If


        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)


        Cnt_IdNo = CloIdNo

        Cnt_UndIdNo = Val(Cnt_IdNo)

        Da = New SqlClient.SqlDataAdapter("select * from Cloth_head where Cloth_idno = " & Str(Val(Cnt_UndIdNo)), con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0).Item("Cloth_StockUnder_IdNo").ToString) = False Then
                If Val(Dt1.Rows(0).Item("Cloth_StockUnder_IdNo").ToString) <> 0 Then Cnt_UndIdNo = Val(Dt1.Rows(0).Item("Cloth_StockUnder_IdNo").ToString)
            End If
        End If
        Dt1.Clear()

        Da = New SqlClient.SqlDataAdapter("select * from Cloth_head where Cloth_StockUnder_IdNo = " & Str(Val(Cnt_UndIdNo)), con)
        Dt1 = New DataTable
        Da.Fill(Dt1)

        Cnt_GrpIdNos = ""
        If Dt1.Rows.Count > 0 Then
            For i = 0 To Dt1.Rows.Count - 1
                Cnt_GrpIdNos = Trim(Cnt_GrpIdNos) & IIf(Trim(Cnt_GrpIdNos) <> "", ", ", "") & Trim(Val(Dt1.Rows(i).Item("Cloth_IdNo")))
            Next
        End If
        If Trim(Cnt_GrpIdNos) <> "" Then
            Cnt_GrpIdNos = "(" & Cnt_GrpIdNos & ")"
        Else
            Cnt_GrpIdNos = "(" & Trim(Val(Cnt_IdNo)) & ")"
        End If

        Cnt_Cond = "(a.Cloth_idno = " & Str(Cnt_IdNo) & " or a.Cloth_idno IN " & Trim(Cnt_GrpIdNos) & ")"

        With dgv_Selection
            chk_SelectAll.Checked = False
            .Rows.Clear()
            SNo = 0

            If CloTypIdNo = 1 Then

                Da = New SqlClient.SqlDataAdapter("select a.* , c.Ledger_Name, d.cloth_name from Weaver_ClothReceipt_Piece_Details a INNER JOIN Company_Head tZ ON a.company_idno = tZ.company_idno  LEFT OUTER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Cloth_Head d ON a.Cloth_IdNo <> 0 and a.Cloth_IdNo = d.Cloth_IdNo Where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.Type1_Meters <> 0 and a.PackingSlip_Code_Type1 IN (select z1.Roll_Code from Roll_Packing_Details z1 where z1.Roll_Packing_Code = '" & Trim(NewCode) & "') and " & Cnt_Cond & IIf(Cnt_Cond <> "", " and ", " ") & " a.Folding = " & Str(Val(txt_Folding.Text)) & " and  (a.StockOff_IdNo = 4 or a.StockOff_IdNo = 5)  order by a.Buyer_RefNo_Type1, a.BuyerOffer_No_Type1, a.BuyerOffer_Party_PieceNo_Type1, a.Weaver_ClothReceipt_Date, a.for_orderby, a.Weaver_ClothReceipt_No, a.PieceNo_OrderBy, a.Piece_No", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    For i = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()

                        SNo = SNo + 1
                        .Rows(n).Cells(dgvCol_Selection.SNO).Value = Val(SNo)

                        .Rows(n).Cells(dgvCol_Selection.LOT_NO).Value = Dt1.Rows(i).Item("Lot_No").ToString
                        .Rows(n).Cells(dgvCol_Selection.PCS_NO).Value = Dt1.Rows(i).Item("Piece_No").ToString
                        .Rows(n).Cells(dgvCol_Selection.CLOTH_TYPE).Value = Common_Procedures.ClothType.Type1

                        .Rows(n).Cells(dgvCol_Selection.METERS).Value = Format(Val(Dt1.Rows(i).Item("Type1_Meters").ToString), "#########.00")

                        PcsMtrs = Val(Dt1.Rows(i).Item("Type1_Meters").ToString) - Val(Dt1.Rows(i).Item("BuyerOffer_Reject_Meters_Type1").ToString)
                        'PcsMtrs = Val(Dt1.Rows(i).Item("Type1_Meters").ToString) - Val(Dt1.Rows(i).Item("BuyerOffer_Less_Meters_Type1").ToString) - Val(Dt1.Rows(i).Item("BuyerOffer_Reject_Meters_Type1").ToString)

                        .Rows(n).Cells(dgvCol_Selection.WEIGHT).Value = Format(Val(PcsMtrs) * Val(Dt1.Rows(i).Item("Weight_Meter").ToString), "#########0.000")
                        .Rows(n).Cells(dgvCol_Selection.wgt_mtr).Value = Val(Dt1.Rows(i).Item("Weight_Meter").ToString)

                        .Rows(n).Cells(dgvCol_Selection.Buyer_Offer_No).Value = Dt1.Rows(i).Item("BuyerOffer_No_Type1").ToString
                        .Rows(n).Cells(dgvCol_Selection.Buyer_Ref_No).Value = Dt1.Rows(i).Item("Buyer_RefNo_Type1").ToString
                        .Rows(n).Cells(dgvCol_Selection.PARTY_PCS_NO).Value = Dt1.Rows(i).Item("BuyerOffer_Party_PieceNo_Type1").ToString
                        .Rows(n).Cells(dgvCol_Selection.Roll_No).Value = Dt1.Rows(i).Item("Roll_No_Type1").ToString

                        .Rows(n).Cells(dgvCol_Selection.STS).Value = "1"  '--STS

                        .Rows(n).Cells(dgvCol_Selection.lot_code).Value = Dt1.Rows(i).Item("Lot_Code").ToString
                        .Rows(n).Cells(dgvCol_Selection.pcs_party_name).Value = Dt1.Rows(i).Item("Ledger_Name").ToString
                        .Rows(n).Cells(dgvCol_Selection.pcs_cloth_name).Value = Dt1.Rows(i).Item("Cloth_Name").ToString

                        If Trim(UCase(cbo_Type.Text)) = "BUYER-OFFER" Then
                            .Rows(n).Cells(dgvCol_Selection.pass_mtr).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Passed_Meters_Type1").ToString)
                        Else
                            .Rows(n).Cells(dgvCol_Selection.pass_mtr).Value = Val(Dt1.Rows(i).Item("Type1_Meters").ToString)
                        End If
                        .Rows(n).Cells(dgvCol_Selection.less_mtr).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Less_Meters_Type1").ToString)
                        .Rows(n).Cells(dgvCol_Selection.rejection_mtr).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Reject_Meters_Type1").ToString)
                        .Rows(n).Cells(dgvCol_Selection.points).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Points_Type1").ToString)
                        .Rows(n).Cells(dgvCol_Selection.points_pass_mtr).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Points_Per_PassMeter_Type1").ToString)
                        .Rows(n).Cells(dgvCol_Selection.grade).Value = Dt1.Rows(i).Item("BuyerOffer_Grade_Type1").ToString

                        .Rows(n).Cells(dgvCol_Selection.Buyer_Offer_Code).Value = Dt1.Rows(i).Item("BuyerOffer_Code_Type1").ToString
                        .Rows(n).Cells(dgvCol_Selection.Roll_Code).Value = Dt1.Rows(i).Item("PackingSlip_Code_Type1").ToString


                        vBaleDelvCd = ""
                        If IsDBNull(Dt1.Rows(i).Item("PackingSlip_Code_Type1").ToString) = False Then
                            If Trim(Dt1.Rows(i).Item("PackingSlip_Code_Type1").ToString) <> "" Then
                                Da = New SqlClient.SqlDataAdapter("Select a.* from Packing_Slip_Head a Where a.Packing_Slip_Code = '" & Trim(Dt1.Rows(i).Item("PackingSlip_Code_Type1").ToString) & "'", con)
                                Dt2 = New DataTable
                                Da.Fill(Dt2)
                                If Dt2.Rows.Count > 0 Then
                                    If IsDBNull(Dt2.Rows(0).Item("Delivery_Code").ToString) = False Then
                                        vBaleDelvCd = Dt2.Rows(0).Item("Delivery_Code").ToString
                                    End If
                                End If
                                Dt2.Clear()
                            End If
                        End If
                        .Rows(n).Cells(dgvCol_Selection.Bale_Delivery_Code).Value = vBaleDelvCd

                        vLmIdNo = 0
                        If IsDBNull(Dt1.Rows(i).Item("Loom_IdNo").ToString) = False Then
                            vLmIdNo = Val(Dt1.Rows(i).Item("Loom_IdNo").ToString)
                        End If

                        vLmNo = ""
                        If vLmIdNo <> 0 Then
                            vLmNo = Common_Procedures.Loom_IdNoToName(con, vLmIdNo)

                        Else
                            If IsDBNull(Dt1.Rows(i).Item("Loom_No").ToString) = False Then
                                vLmNo = Dt1.Rows(i).Item("Loom_No").ToString
                            End If

                        End If

                        .Rows(n).Cells(dgvCol_Selection.loom_no).Value = vLmNo


                        If Trim(vBaleDelvCd) <> "" Then
                            For j = 0 To .ColumnCount - 1
                                .Rows(n).Cells(j).Style.BackColor = Color.LightGray
                                .Rows(n).Cells(j).Style.ForeColor = Color.Red
                            Next
                        Else
                            For j = 0 To .ColumnCount - 1
                                .Rows(i).Cells(j).Style.ForeColor = Color.Red
                            Next

                        End If

                    Next

                End If
                Dt1.Clear()

                BuyrOffrCondt = "(a.BuyerOffer_Code_Type1 = '')"
                If Trim(UCase(cbo_Type.Text)) = "BUYER-OFFER" Then
                    BuyrOffrCondt = "(a.BuyerOffer_Code_Type1 <> '' and a.BuyerOffer_Code_Type1 IN  (SELECT tz1.Buyer_Offer_Code FROM Buyer_Offer_Head tz1 where tz1.Ledger_IdNo = " & Str(Val(led_id)) & " ) )"
                End If

                Da = New SqlClient.SqlDataAdapter("select a.*, c.Ledger_Name, d.Cloth_Name from Weaver_ClothReceipt_Piece_Details a INNER JOIN Company_Head tZ ON a.company_idno = tZ.company_idno LEFT OUTER JOIN Ledger_Head c ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = c.Ledger_IdNo  LEFT OUTER JOIN Cloth_Head d ON a.Cloth_IdNo <> 0 and a.Cloth_IdNo = d.Cloth_IdNo where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.Type1_Meters <> 0 and a.PackingSlip_Code_Type1 = '' and " & Cnt_Cond & IIf(Cnt_Cond <> "", " and ", " ") & BuyrOffrCondt & IIf(BuyrOffrCondt <> "", " and ", " ") & " a.Folding = " & Str(Val(txt_Folding.Text)) & " and  (a.StockOff_IdNo = 4 or a.StockOff_IdNo = 5)  order by a.Buyer_RefNo_Type1, a.BuyerOffer_No_Type1, a.BuyerOffer_Party_PieceNo_Type1, a.Weaver_ClothReceipt_Date, a.for_orderby, a.Weaver_ClothReceipt_No, a.PieceNo_OrderBy, a.Piece_No", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    For i = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()

                        SNo = SNo + 1
                        .Rows(n).Cells(dgvCol_Selection.SNO).Value = Val(SNo)

                        .Rows(n).Cells(dgvCol_Selection.LOT_NO).Value = Dt1.Rows(i).Item("Lot_No").ToString
                        .Rows(n).Cells(dgvCol_Selection.PCS_NO).Value = Dt1.Rows(i).Item("Piece_No").ToString
                        .Rows(n).Cells(dgvCol_Selection.CLOTH_TYPE).Value = Common_Procedures.ClothType.Type1

                        .Rows(n).Cells(dgvCol_Selection.METERS).Value = Format(Val(Dt1.Rows(i).Item("Type1_Meters").ToString), "#########.00")

                        PcsMtrs = Val(Dt1.Rows(i).Item("Type1_Meters").ToString) - Val(Dt1.Rows(i).Item("BuyerOffer_Reject_Meters_Type1").ToString)
                        'PcsMtrs = Val(Dt1.Rows(i).Item("Type1_Meters").ToString) - Val(Dt1.Rows(i).Item("BuyerOffer_Less_Meters_Type1").ToString) - Val(Dt1.Rows(i).Item("BuyerOffer_Reject_Meters_Type1").ToString)

                        .Rows(n).Cells(dgvCol_Selection.WEIGHT).Value = Format(Val(PcsMtrs) * Val(Dt1.Rows(i).Item("Weight_Meter").ToString), "#########0.000")
                        .Rows(n).Cells(dgvCol_Selection.wgt_mtr).Value = Val(Dt1.Rows(i).Item("Weight_Meter").ToString)

                        .Rows(n).Cells(dgvCol_Selection.BUYER_OFFER_NO).Value = Dt1.Rows(i).Item("BuyerOffer_No_Type1").ToString
                        .Rows(n).Cells(dgvCol_Selection.BUYER_REF_NO).Value = Dt1.Rows(i).Item("Buyer_RefNo_Type1").ToString
                        .Rows(n).Cells(dgvCol_Selection.PARTY_PCS_NO).Value = Dt1.Rows(i).Item("BuyerOffer_Party_PieceNo_Type1").ToString
                        .Rows(n).Cells(dgvCol_Selection.ROLL_NO).Value = ""  'ROLL NO

                        .Rows(n).Cells(dgvCol_Selection.STS).Value = ""  'STS

                        .Rows(n).Cells(dgvCol_Selection.lot_code).Value = Dt1.Rows(i).Item("Lot_Code").ToString
                        .Rows(n).Cells(dgvCol_Selection.pcs_party_name).Value = Dt1.Rows(i).Item("Ledger_Name").ToString
                        .Rows(n).Cells(dgvCol_Selection.pcs_cloth_name).Value = Dt1.Rows(i).Item("Cloth_Name").ToString

                        If Trim(UCase(cbo_Type.Text)) = "BUYER-OFFER" Then
                            .Rows(n).Cells(dgvCol_Selection.pass_mtr).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Passed_Meters_Type1").ToString)
                        Else
                            .Rows(n).Cells(dgvCol_Selection.pass_mtr).Value = Val(Dt1.Rows(i).Item("Type1_Meters").ToString)
                        End If

                        .Rows(n).Cells(dgvCol_Selection.less_mtr).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Less_Meters_Type1").ToString)
                        .Rows(n).Cells(dgvCol_Selection.rejection_mtr).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Reject_Meters_Type1").ToString)
                        .Rows(n).Cells(dgvCol_Selection.points).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Points_Type1").ToString)
                        .Rows(n).Cells(dgvCol_Selection.points_pass_mtr).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Points_Per_PassMeter_Type1").ToString)
                        .Rows(n).Cells(dgvCol_Selection.grade).Value = Dt1.Rows(i).Item("BuyerOffer_Grade_Type1").ToString

                        .Rows(n).Cells(dgvCol_Selection.Buyer_Offer_Code).Value = Dt1.Rows(i).Item("BuyerOffer_Code_Type1").ToString
                        .Rows(n).Cells(dgvCol_Selection.Roll_Code).Value = ""
                        .Rows(n).Cells(dgvCol_Selection.Bale_Delivery_Code).Value = ""

                        vLmIdNo = 0
                        If IsDBNull(Dt1.Rows(i).Item("Loom_IdNo").ToString) = False Then
                            vLmIdNo = Val(Dt1.Rows(i).Item("Loom_IdNo").ToString)
                        End If

                        vLmNo = ""
                        If vLmIdNo <> 0 Then
                            vLmNo = Common_Procedures.Loom_IdNoToName(con, vLmIdNo)

                        Else
                            If IsDBNull(Dt1.Rows(i).Item("Loom_No").ToString) = False Then
                                vLmNo = Dt1.Rows(i).Item("Loom_No").ToString
                            End If

                        End If

                        .Rows(n).Cells(dgvCol_Selection.loom_no).Value = vLmNo

                    Next

                End If
                Dt1.Clear()

            End If

            If CloTypIdNo = 1 Or CloTypIdNo = 2 Then

                Da = New SqlClient.SqlDataAdapter("select a.* , C.Ledger_Name, d.cloth_name from Weaver_ClothReceipt_Piece_Details a INNER JOIN Company_Head tZ ON a.company_idno = tZ.company_idno  LEFT OUTER JOIN Ledger_Head c ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Cloth_Head d ON a.Cloth_IdNo <> 0 and a.Cloth_IdNo = d.Cloth_IdNo where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.Type2_Meters <> 0 and a.PackingSlip_Code_Type2  IN (select z1.Roll_Code from Roll_Packing_Details z1 where z1.Roll_Packing_Code = '" & Trim(NewCode) & "') and " & Cnt_Cond & IIf(Cnt_Cond <> "", " and ", " ") & "  a.Folding = " & Str(Val(txt_Folding.Text)) & "   and  (a.StockOff_IdNo = 4 or a.StockOff_IdNo = 5)  order by a.Buyer_RefNo_Type2, a.BuyerOffer_No_Type2, a.BuyerOffer_Party_PieceNo_Type2, a.Weaver_ClothReceipt_Date, a.for_orderby, a.Weaver_ClothReceipt_No, a.PieceNo_OrderBy, a.Piece_No", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    For i = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()

                        SNo = SNo + 1
                        .Rows(n).Cells(dgvCol_Selection.SNO).Value = Val(SNo)

                        .Rows(n).Cells(dgvCol_Selection.LOT_NO).Value = Dt1.Rows(i).Item("Lot_No").ToString
                        .Rows(n).Cells(dgvCol_Selection.PCS_NO).Value = Dt1.Rows(i).Item("Piece_No").ToString
                        .Rows(n).Cells(dgvCol_Selection.CLOTH_TYPE).Value = Common_Procedures.ClothType.Type2

                        .Rows(n).Cells(dgvCol_Selection.METERS).Value = Format(Val(Dt1.Rows(i).Item("Type2_Meters").ToString), "#########.00")

                        PcsMtrs = Val(Dt1.Rows(i).Item("Type2_Meters").ToString) - Val(Dt1.Rows(i).Item("BuyerOffer_Reject_Meters_Type2").ToString)
                        'PcsMtrs = Val(Dt1.Rows(i).Item("Type2_Meters").ToString) - Val(Dt1.Rows(i).Item("BuyerOffer_Less_Meters_Type2").ToString) - Val(Dt1.Rows(i).Item("BuyerOffer_Reject_Meters_Type2").ToString)

                        .Rows(n).Cells(dgvCol_Selection.WEIGHT).Value = Format(Val(PcsMtrs) * Val(Dt1.Rows(i).Item("Weight_Meter").ToString), "#########0.000")
                        .Rows(n).Cells(dgvCol_Selection.wgt_mtr).Value = Val(Dt1.Rows(i).Item("Weight_Meter").ToString)

                        .Rows(n).Cells(dgvCol_Selection.BUYER_OFFER_NO).Value = Dt1.Rows(i).Item("BuyerOffer_No_Type2").ToString
                        .Rows(n).Cells(dgvCol_Selection.BUYER_REF_NO).Value = Dt1.Rows(i).Item("Buyer_RefNo_Type2").ToString
                        .Rows(n).Cells(dgvCol_Selection.PARTY_PCS_NO).Value = Dt1.Rows(i).Item("BuyerOffer_Party_PieceNo_Type2").ToString
                        .Rows(n).Cells(dgvCol_Selection.ROLL_NO).Value = Dt1.Rows(i).Item("Roll_No_Type2").ToString

                        .Rows(n).Cells(dgvCol_Selection.STS).Value = "1"  '--STS

                        .Rows(n).Cells(dgvCol_Selection.lot_code).Value = Dt1.Rows(i).Item("Lot_Code").ToString
                        .Rows(n).Cells(dgvCol_Selection.pcs_party_name).Value = Dt1.Rows(i).Item("Ledger_Name").ToString
                        .Rows(n).Cells(dgvCol_Selection.pcs_cloth_name).Value = Dt1.Rows(i).Item("Cloth_Name").ToString

                        If Trim(UCase(cbo_Type.Text)) = "BUYER-OFFER" Then
                            .Rows(n).Cells(dgvCol_Selection.pass_mtr).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Passed_Meters_Type2").ToString)
                        Else
                            .Rows(n).Cells(dgvCol_Selection.pass_mtr).Value = Val(Dt1.Rows(i).Item("Type2_Meters").ToString)
                        End If
                        .Rows(n).Cells(dgvCol_Selection.less_mtr).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Less_Meters_Type2").ToString)
                        .Rows(n).Cells(dgvCol_Selection.rejection_mtr).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Reject_Meters_Type2").ToString)
                        .Rows(n).Cells(dgvCol_Selection.points).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Points_Type2").ToString)
                        .Rows(n).Cells(dgvCol_Selection.points_pass_mtr).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Points_Per_PassMeter_Type2").ToString)
                        .Rows(n).Cells(dgvCol_Selection.grade).Value = Dt1.Rows(i).Item("BuyerOffer_Grade_Type2").ToString

                        .Rows(n).Cells(dgvCol_Selection.Buyer_Offer_Code).Value = Dt1.Rows(i).Item("BuyerOffer_Code_Type2").ToString
                        .Rows(n).Cells(dgvCol_Selection.Roll_Code).Value = Dt1.Rows(i).Item("PackingSlip_Code_Type2").ToString


                        vBaleDelvCd = ""
                        If IsDBNull(Dt1.Rows(i).Item("PackingSlip_Code_Type2").ToString) = False Then
                            If Trim(Dt1.Rows(i).Item("PackingSlip_Code_Type2").ToString) <> "" Then
                                Da = New SqlClient.SqlDataAdapter("Select a.* from Packing_Slip_Head a Where a.Packing_Slip_Code = '" & Trim(Dt1.Rows(i).Item("PackingSlip_Code_Type2").ToString) & "'", con)
                                Dt2 = New DataTable
                                Da.Fill(Dt2)
                                If Dt2.Rows.Count > 0 Then
                                    If IsDBNull(Dt2.Rows(0).Item("Delivery_Code").ToString) = False Then
                                        vBaleDelvCd = Dt2.Rows(0).Item("Delivery_Code").ToString
                                    End If
                                End If
                                Dt2.Clear()
                            End If
                        End If
                        .Rows(n).Cells(dgvCol_Selection.Bale_Delivery_Code).Value = vBaleDelvCd

                        vLmIdNo = 0
                        If IsDBNull(Dt1.Rows(i).Item("Loom_IdNo").ToString) = False Then
                            vLmIdNo = Val(Dt1.Rows(i).Item("Loom_IdNo").ToString)
                        End If

                        vLmNo = ""
                        If vLmIdNo <> 0 Then
                            vLmNo = Common_Procedures.Loom_IdNoToName(con, vLmIdNo)

                        Else
                            If IsDBNull(Dt1.Rows(i).Item("Loom_No").ToString) = False Then
                                vLmNo = Dt1.Rows(i).Item("Loom_No").ToString
                            End If

                        End If

                        .Rows(n).Cells(dgvCol_Selection.loom_no).Value = vLmNo

                        If Trim(vBaleDelvCd) <> "" Then
                            For j = 0 To .ColumnCount - 1
                                .Rows(n).Cells(j).Style.BackColor = Color.LightGray
                                .Rows(n).Cells(j).Style.ForeColor = Color.Red
                            Next
                        Else
                            For j = 0 To .ColumnCount - 1
                                .Rows(i).Cells(j).Style.ForeColor = Color.Red
                            Next

                        End If

                    Next

                End If
                Dt1.Clear()

                BuyrOffrCondt = "(a.BuyerOffer_Code_Type2 = '')"
                If Trim(UCase(cbo_Type.Text)) = "BUYER-OFFER" Then
                    BuyrOffrCondt = "(a.BuyerOffer_Code_Type2 <> '' and a.BuyerOffer_Code_Type2 IN  (SELECT tz1.Buyer_Offer_Code FROM Buyer_Offer_Head tz1 where tz1.Ledger_IdNo = " & Str(Val(led_id)) & " ) )"
                End If

                Da = New SqlClient.SqlDataAdapter("select a.* , c.Ledger_Name, d.cloth_name from Weaver_ClothReceipt_Piece_Details a INNER JOIN Company_Head tZ ON a.company_idno = tZ.company_idno  LEFT OUTER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Cloth_Head d ON a.Cloth_IdNo <> 0 and a.Cloth_IdNo = d.Cloth_IdNo where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.Type2_Meters <> 0 and a.PackingSlip_Code_Type2 = ''  and " & Cnt_Cond & IIf(Cnt_Cond <> "", " and ", " ") & BuyrOffrCondt & IIf(BuyrOffrCondt <> "", " and ", " ") & " a.Folding = " & Str(Val(txt_Folding.Text)) & "  and  (a.StockOff_IdNo = 4 or a.StockOff_IdNo = 5)  order by a.Buyer_RefNo_Type2, a.BuyerOffer_No_Type2, a.BuyerOffer_Party_PieceNo_Type2, a.Weaver_ClothReceipt_Date, a.for_orderby, a.Weaver_ClothReceipt_No, a.PieceNo_OrderBy, a.Piece_No", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    For i = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()

                        SNo = SNo + 1
                        .Rows(n).Cells(dgvCol_Selection.SNO).Value = Val(SNo)

                        .Rows(n).Cells(dgvCol_Selection.LOT_NO).Value = Dt1.Rows(i).Item("Lot_No").ToString
                        .Rows(n).Cells(dgvCol_Selection.PCS_NO).Value = Dt1.Rows(i).Item("Piece_No").ToString
                        .Rows(n).Cells(dgvCol_Selection.CLOTH_TYPE).Value = Common_Procedures.ClothType.Type2

                        .Rows(n).Cells(dgvCol_Selection.METERS).Value = Format(Val(Dt1.Rows(i).Item("Type2_Meters").ToString), "#########.00")

                        PcsMtrs = Val(Dt1.Rows(i).Item("Type2_Meters").ToString) - Val(Dt1.Rows(i).Item("BuyerOffer_Reject_Meters_Type2").ToString)
                        'PcsMtrs = Val(Dt1.Rows(i).Item("Type2_Meters").ToString) - Val(Dt1.Rows(i).Item("BuyerOffer_Less_Meters_Type2").ToString) - Val(Dt1.Rows(i).Item("BuyerOffer_Reject_Meters_Type2").ToString)

                        .Rows(n).Cells(dgvCol_Selection.WEIGHT).Value = Format(Val(PcsMtrs) * Val(Dt1.Rows(i).Item("Weight_Meter").ToString), "#########0.000")
                        .Rows(n).Cells(dgvCol_Selection.wgt_mtr).Value = Val(Dt1.Rows(i).Item("Weight_Meter").ToString)

                        .Rows(n).Cells(dgvCol_Selection.BUYER_OFFER_NO).Value = Dt1.Rows(i).Item("BuyerOffer_No_Type2").ToString
                        .Rows(n).Cells(dgvCol_Selection.BUYER_REF_NO).Value = Dt1.Rows(i).Item("Buyer_RefNo_Type2").ToString
                        .Rows(n).Cells(dgvCol_Selection.PARTY_PCS_NO).Value = Dt1.Rows(i).Item("BuyerOffer_Party_PieceNo_Type2").ToString
                        .Rows(n).Cells(dgvCol_Selection.ROLL_NO).Value = ""  'ROLL NO

                        .Rows(n).Cells(dgvCol_Selection.STS).Value = ""  'STS

                        .Rows(n).Cells(dgvCol_Selection.lot_code).Value = Dt1.Rows(i).Item("Lot_Code").ToString
                        .Rows(n).Cells(dgvCol_Selection.pcs_party_name).Value = Dt1.Rows(i).Item("Ledger_Name").ToString
                        .Rows(n).Cells(dgvCol_Selection.pcs_cloth_name).Value = Dt1.Rows(i).Item("Cloth_Name").ToString

                        If Trim(UCase(cbo_Type.Text)) = "BUYER-OFFER" Then
                            .Rows(n).Cells(dgvCol_Selection.pass_mtr).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Passed_Meters_Type2").ToString)
                        Else
                            .Rows(n).Cells(dgvCol_Selection.pass_mtr).Value = Val(Dt1.Rows(i).Item("Type2_Meters").ToString)
                        End If
                        .Rows(n).Cells(dgvCol_Selection.less_mtr).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Less_Meters_Type2").ToString)
                        .Rows(n).Cells(dgvCol_Selection.rejection_mtr).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Reject_Meters_Type2").ToString)
                        .Rows(n).Cells(dgvCol_Selection.points).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Points_Type2").ToString)
                        .Rows(n).Cells(dgvCol_Selection.points_pass_mtr).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Points_Per_PassMeter_Type2").ToString)
                        .Rows(n).Cells(dgvCol_Selection.grade).Value = Dt1.Rows(i).Item("BuyerOffer_Grade_Type2").ToString

                        .Rows(n).Cells(dgvCol_Selection.Buyer_Offer_Code).Value = Dt1.Rows(i).Item("BuyerOffer_Code_Type2").ToString
                        .Rows(n).Cells(dgvCol_Selection.Roll_Code).Value = ""
                        .Rows(n).Cells(dgvCol_Selection.Bale_Delivery_Code).Value = ""

                        vLmIdNo = 0
                        If IsDBNull(Dt1.Rows(i).Item("Loom_IdNo").ToString) = False Then
                            vLmIdNo = Val(Dt1.Rows(i).Item("Loom_IdNo").ToString)
                        End If

                        vLmNo = ""
                        If vLmIdNo <> 0 Then
                            vLmNo = Common_Procedures.Loom_IdNoToName(con, vLmIdNo)

                        Else
                            If IsDBNull(Dt1.Rows(i).Item("Loom_No").ToString) = False Then
                                vLmNo = Dt1.Rows(i).Item("Loom_No").ToString
                            End If

                        End If

                        .Rows(n).Cells(dgvCol_Selection.loom_no).Value = vLmNo

                    Next

                End If
                Dt1.Clear()
            End If

            If CloTypIdNo = 3 Then

                Da = New SqlClient.SqlDataAdapter("select a.* , c.Ledger_Name, d.cloth_name from Weaver_ClothReceipt_Piece_Details a INNER JOIN Company_Head tZ ON a.company_idno = tZ.company_idno   LEFT OUTER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Cloth_Head d ON a.Cloth_IdNo <> 0 and a.Cloth_IdNo = d.Cloth_IdNo where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.Type3_Meters <> 0 and a.PackingSlip_Code_Type3  IN (select z1.Roll_Code from Roll_Packing_Details z1 where z1.Roll_Packing_Code = '" & Trim(NewCode) & "') and " & Cnt_Cond & IIf(Cnt_Cond <> "", " and ", " ") & "  a.Folding = " & Str(Val(txt_Folding.Text)) & "  and  (a.StockOff_IdNo = 4 or a.StockOff_IdNo = 5)  order by a.Buyer_RefNo_Type3, a.BuyerOffer_No_Type3, a.BuyerOffer_Party_PieceNo_Type3, a.Weaver_ClothReceipt_Date, a.for_orderby, a.Weaver_ClothReceipt_No, a.PieceNo_OrderBy, a.Piece_No", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    For i = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()

                        SNo = SNo + 1
                        .Rows(n).Cells(dgvCol_Selection.SNO).Value = Val(SNo)

                        .Rows(n).Cells(dgvCol_Selection.LOT_NO).Value = Dt1.Rows(i).Item("Lot_No").ToString
                        .Rows(n).Cells(dgvCol_Selection.PCS_NO).Value = Dt1.Rows(i).Item("Piece_No").ToString
                        .Rows(n).Cells(dgvCol_Selection.CLOTH_TYPE).Value = Common_Procedures.ClothType.Type3

                        .Rows(n).Cells(dgvCol_Selection.METERS).Value = Format(Val(Dt1.Rows(i).Item("Type3_Meters").ToString), "#########.00")

                        PcsMtrs = Val(Dt1.Rows(i).Item("Type3_Meters").ToString) - Val(Dt1.Rows(i).Item("BuyerOffer_Reject_Meters_Type3").ToString)
                        'PcsMtrs = Val(Dt1.Rows(i).Item("Type3_Meters").ToString) - Val(Dt1.Rows(i).Item("BuyerOffer_Less_Meters_Type3").ToString) - Val(Dt1.Rows(i).Item("BuyerOffer_Reject_Meters_Type3").ToString)

                        .Rows(n).Cells(dgvCol_Selection.WEIGHT).Value = Format(Val(PcsMtrs) * Val(Dt1.Rows(i).Item("Weight_Meter").ToString), "#########0.000")
                        .Rows(n).Cells(dgvCol_Selection.wgt_mtr).Value = Val(Dt1.Rows(i).Item("Weight_Meter").ToString)

                        .Rows(n).Cells(dgvCol_Selection.BUYER_OFFER_NO).Value = Dt1.Rows(i).Item("BuyerOffer_No_Type3").ToString
                        .Rows(n).Cells(dgvCol_Selection.BUYER_REF_NO).Value = Dt1.Rows(i).Item("Buyer_RefNo_Type3").ToString
                        .Rows(n).Cells(dgvCol_Selection.PARTY_PCS_NO).Value = Dt1.Rows(i).Item("BuyerOffer_Party_PieceNo_Type3").ToString
                        .Rows(n).Cells(dgvCol_Selection.ROLL_NO).Value = Dt1.Rows(i).Item("Roll_No_Type3").ToString

                        .Rows(n).Cells(dgvCol_Selection.STS).Value = "1"  '--STS

                        .Rows(n).Cells(dgvCol_Selection.lot_code).Value = Dt1.Rows(i).Item("Lot_Code").ToString
                        .Rows(n).Cells(dgvCol_Selection.pcs_party_name).Value = Dt1.Rows(i).Item("Ledger_Name").ToString
                        .Rows(n).Cells(dgvCol_Selection.pcs_cloth_name).Value = Dt1.Rows(i).Item("Cloth_Name").ToString

                        If Trim(UCase(cbo_Type.Text)) = "BUYER-OFFER" Then
                            .Rows(n).Cells(dgvCol_Selection.pass_mtr).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Passed_Meters_Type3").ToString)
                        Else
                            .Rows(n).Cells(dgvCol_Selection.pass_mtr).Value = Val(Dt1.Rows(i).Item("Type3_Meters").ToString)
                        End If
                        .Rows(n).Cells(dgvCol_Selection.less_mtr).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Less_Meters_Type3").ToString)
                        .Rows(n).Cells(dgvCol_Selection.rejection_mtr).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Reject_Meters_Type3").ToString)
                        .Rows(n).Cells(dgvCol_Selection.points).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Points_Type3").ToString)
                        .Rows(n).Cells(dgvCol_Selection.points_pass_mtr).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Points_Per_PassMeter_Type3").ToString)
                        .Rows(n).Cells(dgvCol_Selection.grade).Value = Dt1.Rows(i).Item("BuyerOffer_Grade_Type3").ToString

                        .Rows(n).Cells(dgvCol_Selection.Buyer_Offer_Code).Value = Dt1.Rows(i).Item("BuyerOffer_Code_Type3").ToString
                        .Rows(n).Cells(dgvCol_Selection.Roll_Code).Value = Dt1.Rows(i).Item("PackingSlip_Code_Type3").ToString


                        vBaleDelvCd = ""
                        If IsDBNull(Dt1.Rows(i).Item("PackingSlip_Code_Type3").ToString) = False Then
                            If Trim(Dt1.Rows(i).Item("PackingSlip_Code_Type3").ToString) <> "" Then
                                Da = New SqlClient.SqlDataAdapter("Select a.* from Packing_Slip_Head a Where a.Packing_Slip_Code = '" & Trim(Dt1.Rows(i).Item("PackingSlip_Code_Type3").ToString) & "'", con)
                                Dt2 = New DataTable
                                Da.Fill(Dt2)
                                If Dt2.Rows.Count > 0 Then
                                    If IsDBNull(Dt2.Rows(0).Item("Delivery_Code").ToString) = False Then
                                        vBaleDelvCd = Dt2.Rows(0).Item("Delivery_Code").ToString
                                    End If
                                End If
                                Dt2.Clear()
                            End If
                        End If
                        .Rows(n).Cells(dgvCol_Selection.Bale_Delivery_Code).Value = vBaleDelvCd

                        vLmIdNo = 0
                        If IsDBNull(Dt1.Rows(i).Item("Loom_IdNo").ToString) = False Then
                            vLmIdNo = Val(Dt1.Rows(i).Item("Loom_IdNo").ToString)
                        End If

                        vLmNo = ""
                        If vLmIdNo <> 0 Then
                            vLmNo = Common_Procedures.Loom_IdNoToName(con, vLmIdNo)

                        Else
                            If IsDBNull(Dt1.Rows(i).Item("Loom_No").ToString) = False Then
                                vLmNo = Dt1.Rows(i).Item("Loom_No").ToString
                            End If

                        End If

                        .Rows(n).Cells(dgvCol_Selection.loom_no).Value = vLmNo

                        If Trim(vBaleDelvCd) <> "" Then
                            For j = 0 To .ColumnCount - 1
                                .Rows(n).Cells(j).Style.BackColor = Color.LightGray
                                .Rows(n).Cells(j).Style.ForeColor = Color.Red
                            Next
                        Else
                            For j = 0 To .ColumnCount - 1
                                .Rows(i).Cells(j).Style.ForeColor = Color.Red
                            Next

                        End If

                    Next

                End If
                Dt1.Clear()

                BuyrOffrCondt = "(a.BuyerOffer_Code_Type3 = '')"
                If Trim(UCase(cbo_Type.Text)) = "BUYER-OFFER" Then
                    BuyrOffrCondt = "(a.BuyerOffer_Code_Type3 <> '' and a.BuyerOffer_Code_Type3 IN  (SELECT tz1.Buyer_Offer_Code FROM Buyer_Offer_Head tz1 where tz1.Ledger_IdNo = " & Str(Val(led_id)) & " ) )"
                End If

                Da = New SqlClient.SqlDataAdapter("select a.* ,C.Ledger_Name, d.Cloth_Name from Weaver_ClothReceipt_Piece_Details a INNER JOIN Company_Head tZ ON a.company_idno = tZ.company_idno   LEFT OUTER JOIN Ledger_Head c ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = c.Ledger_IdNo  LEFT OUTER JOIN Cloth_Head d ON a.cloth_IdNo <> 0 and a.cloth_IdNo = d.cloth_IdNo where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.Type3_Meters <> 0 and a.PackingSlip_Code_Type3 = ''  and " & Cnt_Cond & IIf(Cnt_Cond <> "", " and ", " ") & BuyrOffrCondt & IIf(BuyrOffrCondt <> "", " and ", " ") & "  a.Folding = " & Str(Val(txt_Folding.Text)) & "  and  (a.StockOff_IdNo = 4 or a.StockOff_IdNo = 5)  order by a.Buyer_RefNo_Type3, a.BuyerOffer_No_Type3, a.BuyerOffer_Party_PieceNo_Type3, a.Weaver_ClothReceipt_Date, a.for_orderby, a.Weaver_ClothReceipt_No, a.PieceNo_OrderBy, a.Piece_No", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    For i = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()

                        SNo = SNo + 1
                        .Rows(n).Cells(dgvCol_Selection.SNO).Value = Val(SNo)

                        .Rows(n).Cells(dgvCol_Selection.LOT_NO).Value = Dt1.Rows(i).Item("Lot_No").ToString
                        .Rows(n).Cells(dgvCol_Selection.PCS_NO).Value = Dt1.Rows(i).Item("Piece_No").ToString
                        .Rows(n).Cells(dgvCol_Selection.CLOTH_TYPE).Value = Common_Procedures.ClothType.Type3

                        .Rows(n).Cells(dgvCol_Selection.METERS).Value = Format(Val(Dt1.Rows(i).Item("Type3_Meters").ToString), "#########.00")

                        PcsMtrs = Val(Dt1.Rows(i).Item("Type3_Meters").ToString) - Val(Dt1.Rows(i).Item("BuyerOffer_Reject_Meters_Type3").ToString)
                        'PcsMtrs = Val(Dt1.Rows(i).Item("Type3_Meters").ToString) - Val(Dt1.Rows(i).Item("BuyerOffer_Less_Meters_Type3").ToString) - Val(Dt1.Rows(i).Item("BuyerOffer_Reject_Meters_Type3").ToString)

                        .Rows(n).Cells(dgvCol_Selection.WEIGHT).Value = Format(Val(PcsMtrs) * Val(Dt1.Rows(i).Item("Weight_Meter").ToString), "#########0.000")
                        .Rows(n).Cells(dgvCol_Selection.wgt_mtr).Value = Val(Dt1.Rows(i).Item("Weight_Meter").ToString)

                        .Rows(n).Cells(dgvCol_Selection.BUYER_OFFER_NO).Value = Dt1.Rows(i).Item("BuyerOffer_No_Type3").ToString
                        .Rows(n).Cells(dgvCol_Selection.BUYER_REF_NO).Value = Dt1.Rows(i).Item("Buyer_RefNo_Type3").ToString
                        .Rows(n).Cells(dgvCol_Selection.PARTY_PCS_NO).Value = Dt1.Rows(i).Item("BuyerOffer_Party_PieceNo_Type3").ToString
                        .Rows(n).Cells(dgvCol_Selection.ROLL_NO).Value = ""  'ROLL NO

                        .Rows(n).Cells(dgvCol_Selection.STS).Value = ""  'STS

                        .Rows(n).Cells(dgvCol_Selection.lot_code).Value = Dt1.Rows(i).Item("Lot_Code").ToString
                        .Rows(n).Cells(dgvCol_Selection.pcs_party_name).Value = Dt1.Rows(i).Item("Ledger_Name").ToString
                        .Rows(n).Cells(dgvCol_Selection.pcs_cloth_name).Value = Dt1.Rows(i).Item("Cloth_Name").ToString

                        If Trim(UCase(cbo_Type.Text)) = "BUYER-OFFER" Then
                            .Rows(n).Cells(dgvCol_Selection.pass_mtr).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Passed_Meters_Type3").ToString)
                        Else
                            .Rows(n).Cells(dgvCol_Selection.pass_mtr).Value = Val(Dt1.Rows(i).Item("Type3_Meters").ToString)
                        End If
                        .Rows(n).Cells(dgvCol_Selection.less_mtr).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Less_Meters_Type3").ToString)
                        .Rows(n).Cells(dgvCol_Selection.rejection_mtr).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Reject_Meters_Type3").ToString)
                        .Rows(n).Cells(dgvCol_Selection.points).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Points_Type3").ToString)
                        .Rows(n).Cells(dgvCol_Selection.points_pass_mtr).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Points_Per_PassMeter_Type3").ToString)
                        .Rows(n).Cells(dgvCol_Selection.grade).Value = Dt1.Rows(i).Item("BuyerOffer_Grade_Type3").ToString

                        .Rows(n).Cells(dgvCol_Selection.Buyer_Offer_Code).Value = Dt1.Rows(i).Item("BuyerOffer_Code_Type3").ToString
                        .Rows(n).Cells(dgvCol_Selection.Roll_Code).Value = ""
                        .Rows(n).Cells(dgvCol_Selection.Bale_Delivery_Code).Value = ""

                        vLmIdNo = 0
                        If IsDBNull(Dt1.Rows(i).Item("Loom_IdNo").ToString) = False Then
                            vLmIdNo = Val(Dt1.Rows(i).Item("Loom_IdNo").ToString)
                        End If

                        vLmNo = ""
                        If vLmIdNo <> 0 Then
                            vLmNo = Common_Procedures.Loom_IdNoToName(con, vLmIdNo)

                        Else
                            If IsDBNull(Dt1.Rows(i).Item("Loom_No").ToString) = False Then
                                vLmNo = Dt1.Rows(i).Item("Loom_No").ToString
                            End If

                        End If

                        .Rows(n).Cells(dgvCol_Selection.loom_no).Value = vLmNo

                    Next

                End If
                Dt1.Clear()
            End If

            If CloTypIdNo = 4 Then

                Da = New SqlClient.SqlDataAdapter("select a.* , C.Ledger_Name, d.cloth_name from Weaver_ClothReceipt_Piece_Details a INNER JOIN Company_Head tZ ON a.company_idno = tZ.company_idno  LEFT OUTER JOIN Ledger_Head c ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Cloth_Head d ON d.Cloth_IdNo <> 0 and a.Cloth_IdNo = d.Cloth_IdNo where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.Type4_Meters <> 0 and a.PackingSlip_Code_Type4  IN (select z1.Roll_Code from Roll_Packing_Details z1 where z1.Roll_Packing_Code = '" & Trim(NewCode) & "') and " & Cnt_Cond & IIf(Cnt_Cond <> "", " and ", " ") & "  a.Folding = " & Str(Val(txt_Folding.Text)) & "  and  (a.StockOff_IdNo = 4 or a.StockOff_IdNo = 5)  order by a.Buyer_RefNo_Type4, a.BuyerOffer_No_Type4, a.BuyerOffer_Party_PieceNo_Type4, a.Weaver_ClothReceipt_Date, a.for_orderby, a.Weaver_ClothReceipt_No, a.PieceNo_OrderBy, a.Piece_No", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    For i = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()

                        SNo = SNo + 1
                        .Rows(n).Cells(dgvCol_Selection.SNO).Value = Val(SNo)

                        .Rows(n).Cells(dgvCol_Selection.LOT_NO).Value = Dt1.Rows(i).Item("Lot_No").ToString
                        .Rows(n).Cells(dgvCol_Selection.PCS_NO).Value = Dt1.Rows(i).Item("Piece_No").ToString
                        .Rows(n).Cells(dgvCol_Selection.CLOTH_TYPE).Value = Common_Procedures.ClothType.Type4

                        .Rows(n).Cells(dgvCol_Selection.METERS).Value = Format(Val(Dt1.Rows(i).Item("Type4_Meters").ToString), "#########.00")

                        PcsMtrs = Val(Dt1.Rows(i).Item("Type4_Meters").ToString) - Val(Dt1.Rows(i).Item("BuyerOffer_Reject_Meters_Type4").ToString)
                        'PcsMtrs = Val(Dt1.Rows(i).Item("Type4_Meters").ToString) - Val(Dt1.Rows(i).Item("BuyerOffer_Less_Meters_Type4").ToString) - Val(Dt1.Rows(i).Item("BuyerOffer_Reject_Meters_Type4").ToString)

                        .Rows(n).Cells(dgvCol_Selection.WEIGHT).Value = Format(Val(PcsMtrs) * Val(Dt1.Rows(i).Item("Weight_Meter").ToString), "#########0.000")
                        .Rows(n).Cells(dgvCol_Selection.wgt_mtr).Value = Val(Dt1.Rows(i).Item("Weight_Meter").ToString)

                        .Rows(n).Cells(dgvCol_Selection.BUYER_OFFER_NO).Value = Dt1.Rows(i).Item("BuyerOffer_No_Type4").ToString
                        .Rows(n).Cells(dgvCol_Selection.BUYER_REF_NO).Value = Dt1.Rows(i).Item("Buyer_RefNo_Type3").ToString
                        .Rows(n).Cells(dgvCol_Selection.PARTY_PCS_NO).Value = Dt1.Rows(i).Item("BuyerOffer_Party_PieceNo_Type4").ToString
                        .Rows(n).Cells(dgvCol_Selection.ROLL_NO).Value = Dt1.Rows(i).Item("Roll_No_Type4").ToString

                        .Rows(n).Cells(dgvCol_Selection.STS).Value = "1"  '--STS

                        .Rows(n).Cells(dgvCol_Selection.lot_code).Value = Dt1.Rows(i).Item("Lot_Code").ToString
                        .Rows(n).Cells(dgvCol_Selection.pcs_party_name).Value = Dt1.Rows(i).Item("Ledger_Name").ToString
                        .Rows(n).Cells(dgvCol_Selection.pcs_cloth_name).Value = Dt1.Rows(i).Item("Cloth_Name").ToString

                        If Trim(UCase(cbo_Type.Text)) = "BUYER-OFFER" Then
                            .Rows(n).Cells(dgvCol_Selection.pass_mtr).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Passed_Meters_Type4").ToString)
                        Else
                            .Rows(n).Cells(dgvCol_Selection.pass_mtr).Value = Val(Dt1.Rows(i).Item("Type4_Meters").ToString)
                        End If
                        .Rows(n).Cells(dgvCol_Selection.less_mtr).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Less_Meters_Type4").ToString)
                        .Rows(n).Cells(dgvCol_Selection.rejection_mtr).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Reject_Meters_Type4").ToString)
                        .Rows(n).Cells(dgvCol_Selection.points).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Points_Type4").ToString)
                        .Rows(n).Cells(dgvCol_Selection.points_pass_mtr).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Points_Per_PassMeter_Type4").ToString)
                        .Rows(n).Cells(dgvCol_Selection.grade).Value = Dt1.Rows(i).Item("BuyerOffer_Grade_Type4").ToString

                        .Rows(n).Cells(dgvCol_Selection.Buyer_Offer_Code).Value = Dt1.Rows(i).Item("BuyerOffer_Code_Type4").ToString
                        .Rows(n).Cells(dgvCol_Selection.Roll_Code).Value = Dt1.Rows(i).Item("PackingSlip_Code_Type4").ToString


                        vBaleDelvCd = ""
                        If IsDBNull(Dt1.Rows(i).Item("PackingSlip_Code_Type4").ToString) = False Then
                            If Trim(Dt1.Rows(i).Item("PackingSlip_Code_Type4").ToString) <> "" Then
                                Da = New SqlClient.SqlDataAdapter("Select a.* from Packing_Slip_Head a Where a.Packing_Slip_Code = '" & Trim(Dt1.Rows(i).Item("PackingSlip_Code_Type4").ToString) & "'", con)
                                Dt2 = New DataTable
                                Da.Fill(Dt2)
                                If Dt2.Rows.Count > 0 Then
                                    If IsDBNull(Dt2.Rows(0).Item("Delivery_Code").ToString) = False Then
                                        vBaleDelvCd = Dt2.Rows(0).Item("Delivery_Code").ToString
                                    End If
                                End If
                                Dt2.Clear()
                            End If
                        End If
                        .Rows(n).Cells(dgvCol_Selection.Bale_Delivery_Code).Value = vBaleDelvCd

                        vLmIdNo = 0
                        If IsDBNull(Dt1.Rows(i).Item("Loom_IdNo").ToString) = False Then
                            vLmIdNo = Val(Dt1.Rows(i).Item("Loom_IdNo").ToString)
                        End If

                        vLmNo = ""
                        If vLmIdNo <> 0 Then
                            vLmNo = Common_Procedures.Loom_IdNoToName(con, vLmIdNo)

                        Else
                            If IsDBNull(Dt1.Rows(i).Item("Loom_No").ToString) = False Then
                                vLmNo = Dt1.Rows(i).Item("Loom_No").ToString
                            End If

                        End If

                        .Rows(n).Cells(dgvCol_Selection.loom_no).Value = vLmNo

                        If Trim(vBaleDelvCd) <> "" Then
                            For j = 0 To .ColumnCount - 1
                                .Rows(n).Cells(j).Style.BackColor = Color.LightGray
                                .Rows(n).Cells(j).Style.ForeColor = Color.Red
                            Next
                        Else
                            For j = 0 To .ColumnCount - 1
                                .Rows(i).Cells(j).Style.ForeColor = Color.Red
                            Next

                        End If

                    Next

                End If
                Dt1.Clear()

                BuyrOffrCondt = "(a.BuyerOffer_Code_Type4 = '')"
                If Trim(UCase(cbo_Type.Text)) = "BUYER-OFFER" Then
                    BuyrOffrCondt = "(a.BuyerOffer_Code_Type4 <> '' and a.BuyerOffer_Code_Type4 IN  (SELECT tz1.Buyer_Offer_Code FROM Buyer_Offer_Head tz1 where tz1.Ledger_IdNo = " & Str(Val(led_id)) & " ) )"
                End If

                Da = New SqlClient.SqlDataAdapter("select a.* , C.Ledger_Name, d.cloth_name from Weaver_ClothReceipt_Piece_Details a INNER JOIN Company_Head tZ ON a.company_idno = tZ.company_idno   LEFT OUTER JOIN Ledger_Head c ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Cloth_Head d ON a.Cloth_IdNo <> 0 and a.Cloth_IdNo = d.Cloth_IdNo where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.Type4_Meters <> 0 and a.PackingSlip_Code_Type4 = ''  and " & Cnt_Cond & IIf(Cnt_Cond <> "", " and ", " ") & BuyrOffrCondt & IIf(BuyrOffrCondt <> "", " and ", " ") & "  a.Folding = " & Str(Val(txt_Folding.Text)) & "  and  (a.StockOff_IdNo = 4 or a.StockOff_IdNo = 5)  order by a.Buyer_RefNo_Type4, a.BuyerOffer_No_Type4, a.BuyerOffer_Party_PieceNo_Type4, a.Weaver_ClothReceipt_Date, a.for_orderby, a.Weaver_ClothReceipt_No, a.PieceNo_OrderBy, a.Piece_No", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    For i = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()

                        SNo = SNo + 1
                        .Rows(n).Cells(dgvCol_Selection.SNO).Value = Val(SNo)

                        .Rows(n).Cells(dgvCol_Selection.LOT_NO).Value = Dt1.Rows(i).Item("Lot_No").ToString
                        .Rows(n).Cells(dgvCol_Selection.PCS_NO).Value = Dt1.Rows(i).Item("Piece_No").ToString
                        .Rows(n).Cells(dgvCol_Selection.CLOTH_TYPE).Value = Common_Procedures.ClothType.Type4

                        .Rows(n).Cells(dgvCol_Selection.METERS).Value = Format(Val(Dt1.Rows(i).Item("Type4_Meters").ToString), "#########.00")

                        PcsMtrs = Val(Dt1.Rows(i).Item("Type4_Meters").ToString) - Val(Dt1.Rows(i).Item("BuyerOffer_Reject_Meters_Type4").ToString)
                        'PcsMtrs = Val(Dt1.Rows(i).Item("Type4_Meters").ToString) - Val(Dt1.Rows(i).Item("BuyerOffer_Less_Meters_Type4").ToString) - Val(Dt1.Rows(i).Item("BuyerOffer_Reject_Meters_Type4").ToString)

                        .Rows(n).Cells(dgvCol_Selection.WEIGHT).Value = Format(Val(PcsMtrs) * Val(Dt1.Rows(i).Item("Weight_Meter").ToString), "#########0.000")
                        .Rows(n).Cells(dgvCol_Selection.wgt_mtr).Value = Val(Dt1.Rows(i).Item("Weight_Meter").ToString)

                        .Rows(n).Cells(dgvCol_Selection.BUYER_OFFER_NO).Value = Dt1.Rows(i).Item("BuyerOffer_No_Type4").ToString
                        .Rows(n).Cells(dgvCol_Selection.BUYER_REF_NO).Value = Dt1.Rows(i).Item("Buyer_RefNo_Type4").ToString
                        .Rows(n).Cells(dgvCol_Selection.PARTY_PCS_NO).Value = Dt1.Rows(i).Item("BuyerOffer_Party_PieceNo_Type4").ToString
                        .Rows(n).Cells(dgvCol_Selection.ROLL_NO).Value = ""  'ROLL NO

                        .Rows(n).Cells(dgvCol_Selection.STS).Value = ""  'STS

                        .Rows(n).Cells(dgvCol_Selection.lot_code).Value = Dt1.Rows(i).Item("Lot_Code").ToString
                        .Rows(n).Cells(dgvCol_Selection.pcs_party_name).Value = Dt1.Rows(i).Item("Ledger_Name").ToString
                        .Rows(n).Cells(dgvCol_Selection.pcs_cloth_name).Value = Dt1.Rows(i).Item("Cloth_Name").ToString

                        If Trim(UCase(cbo_Type.Text)) = "BUYER-OFFER" Then
                            .Rows(n).Cells(dgvCol_Selection.pass_mtr).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Passed_Meters_Type4").ToString)
                        Else
                            .Rows(n).Cells(dgvCol_Selection.pass_mtr).Value = Val(Dt1.Rows(i).Item("Type4_Meters").ToString)
                        End If
                        .Rows(n).Cells(dgvCol_Selection.less_mtr).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Less_Meters_Type4").ToString)
                        .Rows(n).Cells(dgvCol_Selection.rejection_mtr).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Reject_Meters_Type4").ToString)
                        .Rows(n).Cells(dgvCol_Selection.points).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Points_Type4").ToString)
                        .Rows(n).Cells(dgvCol_Selection.points_pass_mtr).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Points_Per_PassMeter_Type4").ToString)
                        .Rows(n).Cells(dgvCol_Selection.grade).Value = Dt1.Rows(i).Item("BuyerOffer_Grade_Type4").ToString

                        .Rows(n).Cells(dgvCol_Selection.Buyer_Offer_Code).Value = Dt1.Rows(i).Item("BuyerOffer_Code_Type4").ToString
                        .Rows(n).Cells(dgvCol_Selection.Roll_Code).Value = ""
                        .Rows(n).Cells(dgvCol_Selection.Bale_Delivery_Code).Value = ""

                        vLmIdNo = 0
                        If IsDBNull(Dt1.Rows(i).Item("Loom_IdNo").ToString) = False Then
                            vLmIdNo = Val(Dt1.Rows(i).Item("Loom_IdNo").ToString)
                        End If

                        vLmNo = ""
                        If vLmIdNo <> 0 Then
                            vLmNo = Common_Procedures.Loom_IdNoToName(con, vLmIdNo)

                        Else
                            If IsDBNull(Dt1.Rows(i).Item("Loom_No").ToString) = False Then
                                vLmNo = Dt1.Rows(i).Item("Loom_No").ToString
                            End If

                        End If

                        .Rows(n).Cells(dgvCol_Selection.loom_no).Value = vLmNo

                    Next

                End If
                Dt1.Clear()
            End If
            If CloTypIdNo = 5 Then

                Da = New SqlClient.SqlDataAdapter("select a.*, c.Ledger_Name, d.cloth_name from Weaver_ClothReceipt_Piece_Details a INNER JOIN Company_Head tZ ON a.company_idno = tZ.company_idno  LEFT OUTER JOIN Ledger_Head c ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Cloth_Head d ON a.Cloth_IdNo <> 0 and a.Cloth_IdNo = d.Cloth_IdNo where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.Type5_Meters <> 0 and a.PackingSlip_Code_Type5  IN (select z1.Roll_Code from Roll_Packing_Details z1 where z1.Roll_Packing_Code = '" & Trim(NewCode) & "') and " & Cnt_Cond & IIf(Cnt_Cond <> "", " and ", " ") & "  a.Folding = " & Str(Val(txt_Folding.Text)) & " and  (a.StockOff_IdNo = 4 or a.StockOff_IdNo = 5)  order by a.Buyer_RefNo_Type5, a.BuyerOffer_No_Type5, a.BuyerOffer_Party_PieceNo_Type5, a.Weaver_ClothReceipt_Date, a.for_orderby, a.Weaver_ClothReceipt_No, a.PieceNo_OrderBy, a.Piece_No", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    For i = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()

                        SNo = SNo + 1
                        .Rows(n).Cells(dgvCol_Selection.SNO).Value = Val(SNo)

                        .Rows(n).Cells(dgvCol_Selection.LOT_NO).Value = Dt1.Rows(i).Item("Lot_No").ToString
                        .Rows(n).Cells(dgvCol_Selection.PCS_NO).Value = Dt1.Rows(i).Item("Piece_No").ToString
                        .Rows(n).Cells(dgvCol_Selection.CLOTH_TYPE).Value = Common_Procedures.ClothType.Type5

                        .Rows(n).Cells(dgvCol_Selection.METERS).Value = Format(Val(Dt1.Rows(i).Item("Type5_Meters").ToString), "#########.00")

                        PcsMtrs = Val(Dt1.Rows(i).Item("Type5_Meters").ToString) - Val(Dt1.Rows(i).Item("BuyerOffer_Reject_Meters_Type5").ToString)
                        'PcsMtrs = Val(Dt1.Rows(i).Item("Type5_Meters").ToString) - Val(Dt1.Rows(i).Item("BuyerOffer_Less_Meters_Type5").ToString) - Val(Dt1.Rows(i).Item("BuyerOffer_Reject_Meters_Type5").ToString)

                        .Rows(n).Cells(dgvCol_Selection.WEIGHT).Value = Format(Val(PcsMtrs) * Val(Dt1.Rows(i).Item("Weight_Meter").ToString), "#########0.000")
                        .Rows(n).Cells(dgvCol_Selection.wgt_mtr).Value = Val(Dt1.Rows(i).Item("Weight_Meter").ToString)

                        .Rows(n).Cells(dgvCol_Selection.BUYER_OFFER_NO).Value = Dt1.Rows(i).Item("BuyerOffer_No_Type5").ToString
                        .Rows(n).Cells(dgvCol_Selection.BUYER_REF_NO).Value = Dt1.Rows(i).Item("Buyer_RefNo_Type5").ToString
                        .Rows(n).Cells(dgvCol_Selection.PARTY_PCS_NO).Value = Dt1.Rows(i).Item("BuyerOffer_Party_PieceNo_Type5").ToString
                        .Rows(n).Cells(dgvCol_Selection.ROLL_NO).Value = Dt1.Rows(i).Item("Roll_No_Type5").ToString

                        .Rows(n).Cells(dgvCol_Selection.STS).Value = "1"  '--STS

                        .Rows(n).Cells(dgvCol_Selection.lot_code).Value = Dt1.Rows(i).Item("Lot_Code").ToString
                        .Rows(n).Cells(dgvCol_Selection.pcs_party_name).Value = Dt1.Rows(i).Item("Ledger_Name").ToString
                        .Rows(n).Cells(dgvCol_Selection.pcs_cloth_name).Value = Dt1.Rows(i).Item("Cloth_Name").ToString

                        If Trim(UCase(cbo_Type.Text)) = "BUYER-OFFER" Then
                            .Rows(n).Cells(dgvCol_Selection.pass_mtr).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Passed_Meters_Type5").ToString)
                        Else
                            .Rows(n).Cells(dgvCol_Selection.pass_mtr).Value = Val(Dt1.Rows(i).Item("Type5_Meters").ToString)
                        End If
                        .Rows(n).Cells(dgvCol_Selection.less_mtr).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Less_Meters_Type5").ToString)
                        .Rows(n).Cells(dgvCol_Selection.rejection_mtr).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Reject_Meters_Type5").ToString)
                        .Rows(n).Cells(dgvCol_Selection.points).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Points_Type5").ToString)
                        .Rows(n).Cells(dgvCol_Selection.points_pass_mtr).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Points_Per_PassMeter_Type5").ToString)
                        .Rows(n).Cells(dgvCol_Selection.grade).Value = Dt1.Rows(i).Item("BuyerOffer_Grade_Type5").ToString

                        .Rows(n).Cells(dgvCol_Selection.Buyer_Offer_Code).Value = Dt1.Rows(i).Item("BuyerOffer_Code_Type5").ToString
                        .Rows(n).Cells(dgvCol_Selection.Roll_Code).Value = Dt1.Rows(i).Item("PackingSlip_Code_Type5").ToString


                        vBaleDelvCd = ""
                        If IsDBNull(Dt1.Rows(i).Item("PackingSlip_Code_Type5").ToString) = False Then
                            If Trim(Dt1.Rows(i).Item("PackingSlip_Code_Type5").ToString) <> "" Then
                                Da = New SqlClient.SqlDataAdapter("Select a.* from Packing_Slip_Head a Where a.Packing_Slip_Code = '" & Trim(Dt1.Rows(i).Item("PackingSlip_Code_Type5").ToString) & "'", con)
                                Dt2 = New DataTable
                                Da.Fill(Dt2)
                                If Dt2.Rows.Count > 0 Then
                                    If IsDBNull(Dt2.Rows(0).Item("Delivery_Code").ToString) = False Then
                                        vBaleDelvCd = Dt2.Rows(0).Item("Delivery_Code").ToString
                                    End If
                                End If
                                Dt2.Clear()
                            End If
                        End If
                        .Rows(n).Cells(dgvCol_Selection.Bale_Delivery_Code).Value = vBaleDelvCd

                        vLmIdNo = 0
                        If IsDBNull(Dt1.Rows(i).Item("Loom_IdNo").ToString) = False Then
                            vLmIdNo = Val(Dt1.Rows(i).Item("Loom_IdNo").ToString)
                        End If

                        vLmNo = ""
                        If vLmIdNo <> 0 Then
                            vLmNo = Common_Procedures.Loom_IdNoToName(con, vLmIdNo)

                        Else
                            If IsDBNull(Dt1.Rows(i).Item("Loom_No").ToString) = False Then
                                vLmNo = Dt1.Rows(i).Item("Loom_No").ToString
                            End If

                        End If

                        .Rows(n).Cells(dgvCol_Selection.loom_no).Value = vLmNo

                        If Trim(vBaleDelvCd) <> "" Then
                            For j = 0 To .ColumnCount - 1
                                .Rows(n).Cells(j).Style.BackColor = Color.LightGray
                                .Rows(n).Cells(j).Style.ForeColor = Color.Red
                            Next
                        Else
                            For j = 0 To .ColumnCount - 1
                                .Rows(i).Cells(j).Style.ForeColor = Color.Red
                            Next

                        End If

                    Next

                End If
                Dt1.Clear()

                BuyrOffrCondt = "(a.BuyerOffer_Code_Type5 = '')"
                If Trim(UCase(cbo_Type.Text)) = "BUYER-OFFER" Then
                    BuyrOffrCondt = "(a.BuyerOffer_Code_Type5 <> '' and a.BuyerOffer_Code_Type5 IN  (SELECT tz1.Buyer_Offer_Code FROM Buyer_Offer_Head tz1 where tz1.Ledger_IdNo = " & Str(Val(led_id)) & " ) )"
                End If

                Da = New SqlClient.SqlDataAdapter("select a.*, c.Ledger_Name, d.cloth_name from Weaver_ClothReceipt_Piece_Details a INNER JOIN Company_Head tZ ON a.company_idno = tZ.company_idno  LEFT OUTER JOIN Ledger_Head c ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Cloth_Head d ON a.Cloth_IdNo <> 0 and a.Cloth_IdNo = d.Cloth_IdNo where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.Type5_Meters <> 0 and a.PackingSlip_Code_Type5 = '' and " & Cnt_Cond & IIf(Cnt_Cond <> "", " and ", " ") & BuyrOffrCondt & IIf(BuyrOffrCondt <> "", " and ", " ") & "  a.Folding = " & Str(Val(txt_Folding.Text)) & "  and  (a.StockOff_IdNo = 4 or a.StockOff_IdNo = 5)  order by a.Buyer_RefNo_Type5, a.BuyerOffer_No_Type5, a.BuyerOffer_Party_PieceNo_Type5, a.Weaver_ClothReceipt_Date, a.for_orderby, a.Weaver_ClothReceipt_No, a.PieceNo_OrderBy, a.Piece_No", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    For i = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()

                        SNo = SNo + 1

                        .Rows(n).Cells(dgvCol_Selection.SNO).Value = Val(SNo)

                        .Rows(n).Cells(dgvCol_Selection.LOT_NO).Value = Dt1.Rows(i).Item("Lot_No").ToString
                        .Rows(n).Cells(dgvCol_Selection.PCS_NO).Value = Dt1.Rows(i).Item("Piece_No").ToString
                        .Rows(n).Cells(dgvCol_Selection.CLOTH_TYPE).Value = Common_Procedures.ClothType.Type5

                        .Rows(n).Cells(dgvCol_Selection.METERS).Value = Format(Val(Dt1.Rows(i).Item("Type5_Meters").ToString), "#########.00")

                        PcsMtrs = Val(Dt1.Rows(i).Item("Type5_Meters").ToString) - Val(Dt1.Rows(i).Item("BuyerOffer_Reject_Meters_Type5").ToString)
                        'PcsMtrs = Val(Dt1.Rows(i).Item("Type5_Meters").ToString) - Val(Dt1.Rows(i).Item("BuyerOffer_Less_Meters_Type5").ToString) - Val(Dt1.Rows(i).Item("BuyerOffer_Reject_Meters_Type5").ToString)

                        .Rows(n).Cells(dgvCol_Selection.WEIGHT).Value = Format(Val(PcsMtrs) * Val(Dt1.Rows(i).Item("Weight_Meter").ToString), "#########0.000")
                        .Rows(n).Cells(dgvCol_Selection.wgt_mtr).Value = Val(Dt1.Rows(i).Item("Weight_Meter").ToString)

                        .Rows(n).Cells(dgvCol_Selection.BUYER_OFFER_NO).Value = Dt1.Rows(i).Item("BuyerOffer_No_Type5").ToString
                        .Rows(n).Cells(dgvCol_Selection.BUYER_REF_NO).Value = Dt1.Rows(i).Item("Buyer_RefNo_Type5").ToString
                        .Rows(n).Cells(dgvCol_Selection.PARTY_PCS_NO).Value = Dt1.Rows(i).Item("BuyerOffer_Party_PieceNo_Type5").ToString
                        .Rows(n).Cells(dgvCol_Selection.ROLL_NO).Value = ""  'ROLL NO

                        .Rows(n).Cells(dgvCol_Selection.STS).Value = ""  'STS

                        .Rows(n).Cells(dgvCol_Selection.lot_code).Value = Dt1.Rows(i).Item("Lot_Code").ToString
                        .Rows(n).Cells(dgvCol_Selection.pcs_party_name).Value = Dt1.Rows(i).Item("Ledger_Name").ToString
                        .Rows(n).Cells(dgvCol_Selection.pcs_cloth_name).Value = Dt1.Rows(i).Item("Cloth_Name").ToString

                        If Trim(UCase(cbo_Type.Text)) = "BUYER-OFFER" Then
                            .Rows(n).Cells(dgvCol_Selection.pass_mtr).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Passed_Meters_Type5").ToString)
                        Else
                            .Rows(n).Cells(dgvCol_Selection.pass_mtr).Value = Val(Dt1.Rows(i).Item("Type5_Meters").ToString)
                        End If
                        .Rows(n).Cells(dgvCol_Selection.less_mtr).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Less_Meters_Type4").ToString)
                        .Rows(n).Cells(dgvCol_Selection.rejection_mtr).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Reject_Meters_Type5").ToString)
                        .Rows(n).Cells(dgvCol_Selection.points).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Points_Type5").ToString)
                        .Rows(n).Cells(dgvCol_Selection.points_pass_mtr).Value = Val(Dt1.Rows(i).Item("BuyerOffer_Points_Per_PassMeter_Type5").ToString)
                        .Rows(n).Cells(dgvCol_Selection.grade).Value = Dt1.Rows(i).Item("BuyerOffer_Grade_Type5").ToString

                        .Rows(n).Cells(dgvCol_Selection.Buyer_Offer_Code).Value = Dt1.Rows(i).Item("BuyerOffer_Code_Type5").ToString
                        .Rows(n).Cells(dgvCol_Selection.Roll_Code).Value = ""
                        .Rows(n).Cells(dgvCol_Selection.Bale_Delivery_Code).Value = ""

                        vLmIdNo = 0
                        If IsDBNull(Dt1.Rows(i).Item("Loom_IdNo").ToString) = False Then
                            vLmIdNo = Val(Dt1.Rows(i).Item("Loom_IdNo").ToString)
                        End If

                        vLmNo = ""
                        If vLmIdNo <> 0 Then
                            vLmNo = Common_Procedures.Loom_IdNoToName(con, vLmIdNo)

                        Else
                            If IsDBNull(Dt1.Rows(i).Item("Loom_No").ToString) = False Then
                                vLmNo = Dt1.Rows(i).Item("Loom_No").ToString
                            End If

                        End If

                        .Rows(n).Cells(dgvCol_Selection.loom_no).Value = vLmNo

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

    Private Sub dgv_Selection_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Selection.KeyDown
        Dim n As Integer

        On Error Resume Next

        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Space Then
            If dgv_Selection.CurrentCell.RowIndex >= 0 Then

                n = dgv_Selection.CurrentCell.RowIndex

                Select_Piece(n)

                e.Handled = True

            End If
        End If
    End Sub

    Private Sub Select_Piece(ByVal RwIndx As Integer)
        Dim i As Integer = 0
        Dim S As String = ""

        With dgv_Selection

            If .RowCount > 0 And RwIndx >= 0 Then

                If Trim(dgv_Selection.Rows(RwIndx).Cells(dgvCol_Selection.Bale_Delivery_Code).Value) = "" Then

                    .Rows(RwIndx).Cells(dgvCol_Selection.STS).Value = (Val(.Rows(RwIndx).Cells(dgvCol_Selection.STS).Value) + 1) Mod 2

                    If Val(.Rows(RwIndx).Cells(dgvCol_Selection.STS).Value) = 1 Then

                        For i = 0 To .ColumnCount - 1
                            .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
                        Next

                    Else

                        .Rows(RwIndx).Cells(dgvCol_Selection.STS).Value = ""

                        For i = 0 To .ColumnCount - 1
                            .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Blue
                        Next

                    End If

                Else
                    S = ""
                    If Trim(dgv_Selection.Rows(RwIndx).Cells(dgvCol_Selection.Bale_Delivery_Code).Value) <> "" Then
                        S = "Already this rolls was baled/delivered = " & Trim(dgv_Selection.Rows(RwIndx).Cells(23).Value)
                    End If

                    MessageBox.Show(S, "DOES NOT DE-SELECT THIS ROLL...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                    Exit Sub

                End If

            End If

        End With

    End Sub

    Private Sub btn_Close_Selection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Selection.Click
        Piece_Selection()
    End Sub

    Private Sub Piece_Selection()
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim n As Integer = 0
        Dim sno As Integer = 0
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim vTare_Wgt As String = 0
        Dim vGrs_Wgt As String = 0
        Dim NewCode As String = ""

        Pnl_Back.Enabled = True
        dgv_Details.Rows.Clear()

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        sno = 0
        For i = 0 To dgv_Selection.RowCount - 1

            If Val(dgv_Selection.Rows(i).Cells(dgvCol_Selection.STS).Value) = 1 Then

                n = dgv_Details.Rows.Add()

                sno = sno + 1
                dgv_Details.Rows(n).Cells(dgvCol_Details.SlNo).Value = sno
                dgv_Details.Rows(n).Cells(dgvCol_Details.LotNo).Value = dgv_Selection.Rows(i).Cells(dgvCol_Selection.LOT_NO).Value
                dgv_Details.Rows(n).Cells(dgvCol_Details.PcsNo).Value = dgv_Selection.Rows(i).Cells(dgvCol_Selection.PCS_NO).Value
                dgv_Details.Rows(n).Cells(dgvCol_Details.Totalmeters).Value = Val(dgv_Selection.Rows(i).Cells(dgvCol_Selection.METERS).Value)

                dgv_Details.Rows(n).Cells(dgvCol_Details.Net_Weight).Value = dgv_Selection.Rows(i).Cells(dgvCol_Selection.WEIGHT).Value
                dgv_Details.Rows(n).Cells(dgvCol_Details.Net_Weight).Value = Common_Procedures.Meter_RoundOff(Val(dgv_Details.Rows(n).Cells(dgvCol_Details.Net_Weight).Value))

                If Val(dgv_Details.Rows(n).Cells(dgvCol_Details.Net_Weight).Value) <> 0 Then
                    dgv_Details.Rows(n).Cells(dgvCol_Details.Net_Weight).ReadOnly = True
                Else
                    dgv_Details.Rows(n).Cells(dgvCol_Details.Net_Weight).ReadOnly = False
                    dgv_Details.Rows(n).Cells(dgvCol_Details.Net_Weight).Value = ""
                End If

                vGrs_Wgt = ""
                If Val(dgv_Details.Rows(n).Cells(dgvCol_Details.Net_Weight).Value) <> 0 Then
                    vTare_Wgt = Val(txt_Tareweight.Text)
                    vGrs_Wgt = Val(Val(dgv_Details.Rows(n).Cells(dgvCol_Details.Net_Weight).Value) + Val(vTare_Wgt))
                End If

                If Val(vGrs_Wgt) <> 0 Then
                    dgv_Details.Rows(n).Cells(dgvCol_Details.gross_Weight).Value = Common_Procedures.Meter_RoundOff(Val(vGrs_Wgt))
                Else
                    dgv_Selection.Rows(i).Cells(dgvCol_Details.gross_Weight).Value = ""
                End If



  
                'dgv_Details.Rows(n).Cells(dgvCol_Details.P4).Value = dgv_Selection.Rows(i).Cells(dgvCol_Selection.BUYER_OFFER_NO).Value
                ' dgv_Details.Rows(n).Cells(dgvCol_Details.P3).Value = dgv_Selection.Rows(i).Cells(dgvCol_Selection.BUYER_REF_NO).Value
                ' dgv_Details.Rows(n).Cells(dgvCol_Details.P2).Value = dgv_Selection.Rows(i).Cells(dgvCol_Selection.PARTY_PCS_NO).Value
                ' dgv_Details.Rows(n).Cells(dgvCol_Details.P1).Value = dgv_Selection.Rows(i).Cells(dgvCol_Selection.pass_mtr).Value
                '  dgv_Details.Rows(n).Cells(dgvCol_Details.Warp_Lot_no).Value = dgv_Selection.Rows(i).Cells(dgvCol_Selection.less_mtr).Value
                ' dgv_Details.Rows(n).Cells(dgvCol_Details.Fabric_Defect_details).Value = dgv_Selection.Rows(i).Cells(dgvCol_Selection.rejection_mtr).Value

                dgv_Details.Rows(n).Cells(dgvCol_Details.TotalPoints).Value = dgv_Selection.Rows(i).Cells(dgvCol_Selection.points).Value
                If Val(dgv_Details.Rows(n).Cells(dgvCol_Details.TotalPoints).Value) = 0 Then dgv_Details.Rows(n).Cells(dgvCol_Details.TotalPoints).Value = ""

                dgv_Details.Rows(n).Cells(dgvCol_Details.Per_100ML).Value = dgv_Selection.Rows(i).Cells(dgvCol_Selection.points_pass_mtr).Value
                If Val(dgv_Details.Rows(n).Cells(dgvCol_Details.Per_100ML).Value) = 0 Then dgv_Details.Rows(n).Cells(dgvCol_Details.Per_100ML).Value = ""

                dgv_Details.Rows(n).Cells(dgvCol_Details.fabric_grade).Value = dgv_Selection.Rows(i).Cells(dgvCol_Selection.grade).Value
                dgv_Details.Rows(n).Cells(dgvCol_Details.Roll_No).Value = dgv_Selection.Rows(i).Cells(dgvCol_Selection.ROLL_NO).Value
                dgv_Details.Rows(n).Cells(dgvCol_Details.lot_code).Value = dgv_Selection.Rows(i).Cells(dgvCol_Selection.lot_code).Value
                dgv_Details.Rows(n).Cells(dgvCol_Details.pcs_party_name).Value = dgv_Selection.Rows(i).Cells(dgvCol_Selection.pcs_party_name).Value
                dgv_Details.Rows(n).Cells(dgvCol_Details.pcs_cloth_name).Value = dgv_Selection.Rows(i).Cells(dgvCol_Selection.pcs_cloth_name).Value
                dgv_Details.Rows(n).Cells(dgvCol_Details.buyer_offer_code).Value = dgv_Selection.Rows(i).Cells(dgvCol_Selection.Buyer_Offer_Code).Value
                dgv_Details.Rows(n).Cells(dgvCol_Details.roll_code).Value = dgv_Selection.Rows(i).Cells(dgvCol_Selection.Roll_Code).Value
                dgv_Details.Rows(n).Cells(dgvCol_Details.bale_delivery_code).Value = dgv_Selection.Rows(i).Cells(dgvCol_Selection.Bale_Delivery_Code).Value
                dgv_Details.Rows(n).Cells(dgvCol_Details.Loom_No).Value = dgv_Selection.Rows(i).Cells(dgvCol_Selection.loom_no).Value


                da2 = New SqlClient.SqlDataAdapter("Select a.* from Roll_Packing_Details a Where a.Roll_Packing_Code = '" & Trim(NewCode) & "' and a.Roll_Code = '" & Trim(dgv_Details.Rows(n).Cells(dgvCol_Details.roll_code).Value) & "' and a.lot_code  = '" & Trim(dgv_Details.Rows(n).Cells(dgvCol_Details.lot_code).Value) & "' and a.Pcs_No = '" & Trim(dgv_Details.Rows(n).Cells(dgvCol_Details.PcsNo).Value) & "'  Order by a.sl_no", con)
                dt2 = New DataTable
                da2.Fill(dt2)
                If dt2.Rows.Count > 0 Then

                    dgv_Details.Rows(n).Cells(dgvCol_Details.P4).Value = Val(dt2.Rows(0).Item("P4").ToString)
                    If Val(dgv_Details.Rows(n).Cells(dgvCol_Details.P4).Value) = 0 Then dgv_Details.Rows(n).Cells(dgvCol_Details.P4).Value = ""

                    dgv_Details.Rows(n).Cells(dgvCol_Details.p3).Value = Val(dt2.Rows(0).Item("p3").ToString)
                    If Val(dgv_Details.Rows(n).Cells(dgvCol_Details.p3).Value) = 0 Then dgv_Details.Rows(n).Cells(dgvCol_Details.p3).Value = ""

                    dgv_Details.Rows(n).Cells(dgvCol_Details.p2).Value = Val(dt2.Rows(0).Item("P2").ToString)
                    If Val(dgv_Details.Rows(n).Cells(dgvCol_Details.p2).Value) = 0 Then dgv_Details.Rows(n).Cells(dgvCol_Details.p2).Value = ""

                    dgv_Details.Rows(n).Cells(dgvCol_Details.P1).Value = Val(dt2.Rows(0).Item("P1").ToString)
                    If Val(dgv_Details.Rows(n).Cells(dgvCol_Details.P1).Value) = 0 Then dgv_Details.Rows(n).Cells(dgvCol_Details.P1).Value = ""


                    'dgv_Details.Rows(n).Cells(dgvCol_Details.TotalPoints).Value = Val(dt2.Rows(0).Item("Points").ToString)
                    'If Val(dgv_Details.Rows(n).Cells(dgvCol_Details.TotalPoints).Value) = 0 Then dgv_Details.Rows(n).Cells(dgvCol_Details.TotalPoints).Value = ""

                    'dgv_Details.Rows(n).Cells(dgvCol_Details.Per_100ML).Value = Val(dt2.Rows(0).Item("Point_Per_PassMeter").ToString)
                    'If Val(dgv_Details.Rows(n).Cells(dgvCol_Details.Per_100ML).Value) = 0 Then dgv_Details.Rows(n).Cells(dgvCol_Details.Per_100ML).Value = ""

                    dgv_Details.Rows(n).Cells(dgvCol_Details.Warp_Lot_No).Value = Trim(dt2.Rows(0).Item("Warp_Lot_no").ToString)
                    dgv_Details.Rows(n).Cells(dgvCol_Details.Weft_lot_No).Value = Trim(dt2.Rows(0).Item("Weft_lot_no").ToString)

                    dgv_Details.Rows(n).Cells(dgvCol_Details.fabric_defect_Details).Value = Trim(dt2.Rows(0).Item("Fabric_defect_penalty_point").ToString)

                    If Val(dt2.Rows(i).Item("Loom_IdNo").ToString) <> 0 Then
                        dgv_Details.Rows(n).Cells(dgvCol_Details.Loom_No).Value = Common_Procedures.Loom_IdNoToName(con, Val(dt2.Rows(i).Item("Loom_IdNo").ToString))
                    Else
                        dgv_Details.Rows(n).Cells(dgvCol_Details.Loom_No).Value = dt2.Rows(i).Item("Loom_No").ToString
                    End If
                    dgv_Details.Rows(n).Cells(dgvCol_Details.fabric_grade).Value = dt2.Rows(i).Item("Grade").ToString

                End If
                dt2.Clear()


                If Trim(dgv_Details.Rows(n).Cells(dgvCol_Details.bale_delivery_code).Value) <> "" Then
                    For j = 0 To dgv_Details.ColumnCount - 1
                        dgv_Details.Rows(n).Cells(j).Style.BackColor = Color.LightGray
                        dgv_Details.Rows(n).Cells(j).Style.ForeColor = Color.Red
                    Next
                End If

            End If

        Next i

        Total_Calculation()

        Pnl_Back.Enabled = True
        pnl_Selection.Visible = False
        If dgv_Details.Rows.Count > 0 Then
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCol_Details.Loom_No)
            dgv_Details.CurrentCell.Selected = True
        Else
            ' txt_Note.Focus()
            txt_Tareweight.Focus()
        End If
        'If txt_Note.Enabled And txt_Note.Visible Then txt_Note.Focus()

    End Sub

    Private Sub btn_Print_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        print_record()
    End Sub

    Private Sub btn_print_Close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Print.Click
        Pnl_Back.Enabled = True
        pnl_Print.Visible = False
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        Dim prtFrm As Single = 0
        Dim prtTo As Single = 0
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim Condt As String = ""
        Dim PpSzSTS As Boolean = False
        'Dim ps As Printing.PaperSize
        Dim NewCode As String


        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.PrintEntry, Common_Procedures.UR.ClothSales_Roll_Packing_Entry, New_Entry) = False Then Exit Sub


        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.* from Roll_Packing_Head a Where a.Roll_Packing_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count <= 0 Then

                MessageBox.Show("No Entry Found", "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub

            End If

            dt1.Dispose()
            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

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
                ppd.ClientSize = New Size(600, 600)

                ppd.ShowDialog()

            Catch ex As Exception
                MsgBox("The printing operation failed" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "DOES NOT SHOW PRINT PREVIEW...")

            End Try

        End If
    End Sub

    Private Sub btn_Print_Cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_Cancel.Click
        btn_print_Close_Click(sender, e)
    End Sub


    Private Sub txt_PrintFrom_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_PrintFrom.KeyDown
        If e.KeyCode = Keys.Down Then
            txt_PrintTo.Focus()
        End If
    End Sub

    Private Sub txt_PrintTo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_PrintTo.KeyDown
        If e.KeyCode = Keys.Down Then
            btn_Print_Ok.Focus()
        End If
        If e.KeyCode = Keys.Up Then
            txt_PrintFrom.Focus()
        End If
    End Sub



    Private Sub PrintDocument1_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument1.BeginPrint
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim NewCode As String = ""

        prn_HdDt.Clear()
        prn_DetDt.Clear()

        prn_PageNo = 0
        prn_HdIndx = 0
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_Count = 1

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, tZ.*, c.Cloth_Name , ct.clothType_name ,E.* from Roll_Packing_Head a INNER JOIN Company_head tZ ON a.company_idno = tZ.Company_Idno INNER JOIN Cloth_Head c ON a.Cloth_IdNo = c.Cloth_IdNo LEFT OUTER JOIN ClothType_Head ct ON a.ClothType_IdNo = ct.ClothType_IdNo  LEFT OUTER JOIN LEDGER_Head E ON A.Ledger_IdNo = E.Ledger_IdNo  Where a.Roll_Packing_Code = '" & Trim(NewCode) & "' Order by a.Roll_Packing_Date, a.for_OrderBy, a.Roll_Packing_No, a.Roll_Packing_Code", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("select a.* from Roll_Packing_Details a where a.Roll_Packing_Code = '" & Trim(NewCode) & "'  order by a.Roll_No, a.Sl_No", con)
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

    Private Sub PrintDocument1_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        Dim NewCode As String = ""
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim InvNo As String = "", InvDate As String = "", PONo As String = "", PODate As String = "", FabLotNo As String = ""
        Dim Range1_100_200_Perc As String = "", Range2_200_300_Perc As String = "", Range3_300_400_Perc As String = "", Range4_abv400_Perc As String = ""


        If prn_HdDt.Rows.Count <= 0 Then Exit Sub

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        da1 = New SqlClient.SqlDataAdapter("select a.* from ClothSales_Invoice_Head a INNER JOIN Packing_Slip_Head b ON a.ClothSales_Invoice_Code = b.Delivery_Code INNER JOIN Roll_Packing_Head c ON b.Roll_Packing_Code = c.Roll_Packing_Code Where c.Roll_Packing_Code = '" & Trim(NewCode) & "'", con)
        'da1 = New SqlClient.SqlDataAdapter("select a.* from ClothSales_Invoice_Head a Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.ClothSales_Invoice_Code = '" & Trim(NewCode) & "' and a.Tax_Type = 'GST'", con)
        dt1 = New DataTable
        da1.Fill(dt1)
        If dt1.Rows.Count > 0 Then
            InvNo = dt1.Rows(0).Item("ClothSales_Invoice_No").ToString
            InvDate = Format(Convert.ToDateTime(dt1.Rows(0).Item("ClothSales_Invoice_Date").ToString), "dd-MM-yyyy").ToString
            PONo = dt1.Rows(0).Item("Party_OrderNo").ToString
            PODate = dt1.Rows(0).Item("Party_OrderDate").ToString
            FabLotNo = dt1.Rows(0).Item("Fabric_Lot_No").ToString
        End If
        dt1.Clear()

        Common_Procedures.Printing_RollPacking_Format1(PrintDocument1, e, con, Me.Name, NewCode, prn_HdDt, prn_DetDt, prn_DetSNo, prn_PageNo, prn_Count, prn_HdIndx, prn_DetIndx, InvNo, InvDate, PONo, PODate, FabLotNo, Range1_100_200_Perc, Range2_200_300_Perc, Range3_300_400_Perc, Range4_abv400_Perc)

        'Printing_Format1(e)

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
        Dim LnAr(21) As Single, ClAr(21) As Single
        Dim EntryCode As String
        Dim CurY As Single
        Dim npCS As Integer = "1"
      
        Dim Detail1 As String = ""

        Dim Detail2 As String = ""
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
            .Left = 20
            .Right = 35
            .Top = 30
            .Bottom = 30
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pFont = New Font("Calibri", 7, FontStyle.Regular)

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

        NoofItems_PerPage = 27

        Erase LnAr
        Erase ClAr

        LnAr = New Single(21) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(21) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClAr(1) = 35 : ClAr(2) = 45 : ClAr(3) = 45 : ClAr(4) = 38 : ClAr(5) = 50 : ClAr(6) = 30 : ClAr(7) = 30 : ClAr(8) = 30 : ClAr(9) = 30 : ClAr(10) = 50 : ClAr(11) = 45 : ClAr(12) = 35 : ClAr(13) = 32 : ClAr(14) = 32 : ClAr(15) = 55 : ClAr(16) = 60
        ClAr(17) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15) + ClAr(16))

        TxtHgt = 18.5

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try
            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format1_PageHeader(e, EntryCode, TxtHgt, prn_DetSNo, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr)

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

                 
                        If Trim(prn_DetDt.Rows(prn_DetIndx).Item("Fabric_defect_penalty_point").ToString) <> "" Then
                            Detail1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Fabric_defect_penalty_point").ToString)
                     
                        End If

                        Detail2 = ""
                        If Len(Detail1) > 25 Then
                            For I = 25 To 1 Step -1
                                If Mid$(Trim(Detail1), I, 1) = " " Or Mid$(Trim(Detail1), I, 1) = "," Or Mid$(Trim(Detail1), I, 1) = "." Or Mid$(Trim(Detail1), I, 1) = "-" Or Mid$(Trim(Detail1), I, 1) = "/" Or Mid$(Trim(Detail1), I, 1) = "_" Or Mid$(Trim(Detail1), I, 1) = "(" Or Mid$(Trim(Detail1), I, 1) = ")" Or Mid$(Trim(Detail1), I, 1) = "\" Or Mid$(Trim(Detail1), I, 1) = "[" Or Mid$(Trim(Detail1), I, 1) = "]" Or Mid$(Trim(Detail1), I, 1) = "{" Or Mid$(Trim(Detail1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 25
                            Detail2 = Microsoft.VisualBasic.Right(Trim(Detail1), Len(Detail1) - I)
                            Detail1 = Microsoft.VisualBasic.Left(Trim(Detail1), I - 1)
                        End If

                        prn_DetSNo = prn_DetSNo + 1

                        CurY = CurY + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetSNo)), LMargin + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Roll_No").ToString), LMargin + ClAr(1) + ClAr(2) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetDt.Rows(prn_DetIndx).Item("Meters")).ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, (prn_DetDt.Rows(prn_DetIndx).Item("Lot_No").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, (prn_DetDt.Rows(prn_DetIndx).Item("Loom_No").ToString) & "/" & (prn_DetDt.Rows(prn_DetIndx).Item("Pcs_No").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetDt.Rows(prn_DetIndx).Item("p4").ToString)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetDt.Rows(prn_DetIndx).Item("p3").ToString)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetDt.Rows(prn_DetIndx).Item("p2").ToString)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetDt.Rows(prn_DetIndx).Item("p1").ToString)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)

                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetDt.Rows(prn_DetIndx).Item("Points").ToString)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) - 10, CurY, 1, 0, pFont)

                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetDt.Rows(prn_DetIndx).Item("Point_Per_PassMeter").ToString)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Grade").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetDt.Rows(prn_DetIndx).Item("Warp_lot_no").ToString)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetDt.Rows(prn_DetIndx).Item("Weft_lot_no").ToString)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) - 10, CurY, 1, 0, pFont)

                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetDt.Rows(prn_DetIndx).Item("Weight").ToString)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetDt.Rows(prn_DetIndx).Item("Gross_Weight").ToString)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15) + ClAr(16) - 10, CurY, 1, 0, pFont)





                        Common_Procedures.Print_To_PrintDocument(e, Detail1, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15) + ClAr(16) + 10, CurY, 2, 0, pFont)
                        If Detail2 <> "" Then
                            CurY = CurY + TxtHgt - 5
                            Common_Procedures.Print_To_PrintDocument(e, Detail2, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15) + ClAr(16) + 10, CurY, 2, 0, pFont)
                            NoofDets = NoofDets + 1
                        End If

                        'Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Grade").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + 10, CurY, 0, 0, pFont)

                        NoofDets = NoofDets + 1

                        prn_DetIndx = prn_DetIndx + 1
                        prn_DetSNo = prn_DetSNo
                    Loop

                End If

                Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format1_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal prn_DetSNo As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2, da3 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim p1Font As Font
        Dim NewCode As String = ""
        Dim Y1 As Single = 0, Y2 As Single = 0
        Dim Cmp_Add As String = ""
        Dim C1 As Single = 0, C11, C22, c33 As Single, Range1, Range2, Range3, Range4, no_of_rolls As Single

        Dim Cmp_Name, Po_No_date As String, Inv_No As String, Inv_date As String
        Dim fabric_Lotno As String, total_mtr As String, Cmp_EMail As String

        PageNo = PageNo + 1

        CurY = TMargin + 30
      

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)


        C11 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4)
        C22 = ClAr(1) + ClAr(2) + ClAr(3)
        c33 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12)
        p1Font = New Font("Calibri", 15, FontStyle.Bold)
        prn_Count = prn_Count + 1

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin, CurY, 2, PrintWidth, p1Font)
        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(11) = CurY
        Po_No_date = ""

        Cmp_Name = "" : Inv_No = "" : Inv_date = ""
        fabric_Lotno = "" : total_mtr = "" : Cmp_EMail = ""


        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString

        total_mtr = prn_HdDt.Rows(0).Item("Total_Meters").ToString


        If Trim(prn_HdDt.Rows(0).Item("Cloth_Name").ToString) <> "" Then
            Cmp_EMail = "MAIL ID : " & prn_HdDt.Rows(0).Item("Company_EMail").ToString
        End If

        Common_Procedures.Print_To_PrintDocument(e, "PACKING LIST CUM SELF INSPECTION ABSTRACT REPORT", LMargin, CurY, 2, PrintWidth, p1Font)
        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(13) = CurY
        ' CurY = CurY + TxtHgt
        p1Font = New Font("Calibri", 10, FontStyle.Bold)
        Y1 = CurY + 0.5
        Y2 = CurY + TxtHgt
        Common_Procedures.FillRegionRectangle(e, LMargin, Y1, C22 - 15 + ClAr(4), Y2)

        Common_Procedures.Print_To_PrintDocument(e, "Company Name", LMargin + 10, CurY, 0, 0, p1Font)

        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin + C11 - 15, CurY, 0, 0, p1Font)
        Y1 = CurY + 0.5
        Y2 = CurY + TxtHgt
        Common_Procedures.FillRegionRectangle(e, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), Y1, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15) - 5, Y2)

        Common_Procedures.Print_To_PrintDocument(e, "Loom Type & Selvedge", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), CurY, 2, ClAr(12), p1Font)

        Common_Procedures.Print_To_PrintDocument(e, "Airjet", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15), CurY, 2, ClAr(16), p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "open", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15) + ClAr(16), CurY, 2, ClAr(17), p1Font)

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(14) = CurY
        'CurY = CurY + TxtHgtLoom type & Selvedge
        Y1 = CurY + 0.5
        Y2 = CurY + TxtHgt
        Common_Procedures.FillRegionRectangle(e, LMargin, Y1, C22 - 15 + ClAr(4), Y2)


        Common_Procedures.Print_To_PrintDocument(e, "PO No & Date", LMargin + 10, CurY, 0, 0, p1Font)

        Common_Procedures.Print_To_PrintDocument(e, Po_No_date, LMargin + C11 - 15, CurY, 0, 0, p1Font)
        Y1 = CurY + 0.5
        Y2 = CurY + TxtHgt
        Common_Procedures.FillRegionRectangle(e, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), Y1, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15) - 5, Y2)

        Common_Procedures.Print_To_PrintDocument(e, "Break-up", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), CurY, 2, ClAr(12), p1Font)

        Common_Procedures.Print_To_PrintDocument(e, "Qty in Mtrs", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15), CurY, 2, ClAr(16), p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "% of Rolls", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15) + ClAr(16), CurY, 2, ClAr(17), p1Font)


        CurY = CurY + TxtHgt


        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(15) = CurY
        ' CurY = CurY + TxtHgt
        Y1 = CurY + 0.5
        Y2 = CurY + TxtHgt
        Common_Procedures.FillRegionRectangle(e, LMargin, Y1, C22 - 15 + ClAr(4), Y2)

        Common_Procedures.Print_To_PrintDocument(e, "TC|Construct|Weave", LMargin + 10, CurY, 0, 0, p1Font)

        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Cloth_Name").ToString) & Trim(prn_HdDt.Rows(0).Item("Clothtype_Name").ToString), LMargin + C11 - 15, CurY, 0, 0, p1Font)

        Y1 = CurY + 0.5
        Y2 = CurY + TxtHgt
        Common_Procedures.FillRegionRectangle(e, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), Y1, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15) - 5, Y2)

        Common_Procedures.Print_To_PrintDocument(e, "100-200 Mtrs", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), CurY, 2, ClAr(12), p1Font)
        da2 = New SqlClient.SqlDataAdapter("SELECT SUM(a.meters) as range1 FROM Roll_Packing_Details a WHERE  a.meters between 100 AND 200 And Roll_packing_Code='" & Trim(NewCode) & "'", con)
        da2.Fill(dt2)
        Range1 = Val(dt2.Rows(0).Item("range1").ToString)
        Common_Procedures.Print_To_PrintDocument(e, Range1, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15), CurY, 2, ClAr(16), p1Font)

        Common_Procedures.Print_To_PrintDocument(e, Format(Val((Range1 / total_mtr) * 100), "#######0"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15) + ClAr(16), CurY, 2, ClAr(17), p1Font)

        dt2.Clear()


        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(16) = CurY

        Y1 = CurY + 0.5
        Y2 = CurY + TxtHgt
        Common_Procedures.FillRegionRectangle(e, LMargin, Y1, C22 - 15 + ClAr(4), Y2)

        Common_Procedures.Print_To_PrintDocument(e, "Invoice no", LMargin + 10, CurY, 0, 0, p1Font)

        Common_Procedures.Print_To_PrintDocument(e, Inv_No, LMargin + C11 - 15, CurY, 0, 0, p1Font)
        Y1 = CurY + 0.5
        Y2 = CurY + TxtHgt
        Common_Procedures.FillRegionRectangle(e, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), Y1, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15) - 5, Y2)


        Common_Procedures.Print_To_PrintDocument(e, "201-300 Mtrs", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), CurY, 2, ClAr(12), p1Font)



        da2 = New SqlClient.SqlDataAdapter("SELECT SUM(a.meters) as range2 FROM Roll_Packing_Details a WHERE  a.meters between 201 AND 300 And Roll_packing_Code='" & Trim(NewCode) & "'", con)
        da2.Fill(dt2)
        Range2 = Val(dt2.Rows(0).Item("range2").ToString)
        Common_Procedures.Print_To_PrintDocument(e, Range2, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15), CurY, 2, ClAr(16), p1Font)
        Common_Procedures.Print_To_PrintDocument(e, Format(Val((Range2 / total_mtr) * 100), "#######0"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15) + ClAr(16), CurY, 2, ClAr(17), p1Font)

        dt2.Clear()
        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(17) = CurY
        Y1 = CurY + 0.5
        Y2 = CurY + TxtHgt
        Common_Procedures.FillRegionRectangle(e, LMargin, Y1, C22 - 15 + ClAr(4), Y2)

        Common_Procedures.Print_To_PrintDocument(e, "Invoice Date", LMargin + 10, CurY, 0, 0, p1Font)

        Common_Procedures.Print_To_PrintDocument(e, Inv_date, LMargin + C11 - 15, CurY, 0, 0, p1Font)
        Y1 = CurY + 0.5
        Y2 = CurY + TxtHgt
        Common_Procedures.FillRegionRectangle(e, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), Y1, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15) - 5, Y2)


        Common_Procedures.Print_To_PrintDocument(e, "301-400 Mtrs", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), CurY, 2, ClAr(12), p1Font)
        da2 = New SqlClient.SqlDataAdapter("SELECT SUM(a.meters) as range3 FROM Roll_Packing_Details a WHERE  a.meters between 301 AND 400 And Roll_packing_Code='" & Trim(NewCode) & "'", con)
        da2.Fill(dt2)
        Range3 = Val(dt2.Rows(0).Item("range3").ToString)
        Common_Procedures.Print_To_PrintDocument(e, Range3, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15), CurY, 2, ClAr(16), p1Font)
        Common_Procedures.Print_To_PrintDocument(e, Format(Val((Range3 / total_mtr) * 100), "#######0"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15) + ClAr(16), CurY, 2, ClAr(17), p1Font)

        dt2.Clear()
        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(18) = CurY
        Y1 = CurY + 0.5
        Y2 = CurY + TxtHgt
        Common_Procedures.FillRegionRectangle(e, LMargin, Y1, C22 - 15 + ClAr(4), Y2)


        Common_Procedures.Print_To_PrintDocument(e, "Fabric Lot no", LMargin + 10, CurY, 0, 0, p1Font)

        Common_Procedures.Print_To_PrintDocument(e, fabric_Lotno, LMargin + C11 - 15, CurY, 0, 0, p1Font)

        Y1 = CurY + 0.5
        Y2 = CurY + TxtHgt
        Common_Procedures.FillRegionRectangle(e, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), Y1, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15) - 5, Y2)

        Common_Procedures.Print_To_PrintDocument(e, "> 401 Mtrs", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), CurY, 2, ClAr(12), p1Font)
        da2 = New SqlClient.SqlDataAdapter("SELECT SUM(a.meters) as range4 FROM Roll_Packing_Details a WHERE  a.meters  >=401 And Roll_packing_Code='" & Trim(NewCode) & "'", con)
        da2.Fill(dt2)
        Range4 = Val(dt2.Rows(0).Item("range4").ToString)
        Common_Procedures.Print_To_PrintDocument(e, Range4, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15), CurY, 2, ClAr(16), p1Font)
        Common_Procedures.Print_To_PrintDocument(e, Format(Val((Range4 / total_mtr) * 100), "#######0"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15) + ClAr(16), CurY, 2, ClAr(17), p1Font)

        dt2.Clear()
        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(19) = CurY
        Y1 = CurY + 0.5
        Y2 = CurY + TxtHgt
        Common_Procedures.FillRegionRectangle(e, LMargin, Y1, C22 - 15 + ClAr(4), Y2)


        Common_Procedures.Print_To_PrintDocument(e, "Total no of rolls : ", LMargin + 10, CurY, 0, 0, p1Font)
        da3 = New SqlClient.SqlDataAdapter("SELECT COUNT(*) No_of_Rolls FROM Roll_Packing_Details  WHERE Roll_packing_Code='" & Trim(NewCode) & "'", con)
        da3.Fill(dt3)
        no_of_rolls = Val(dt3.Rows(0).Item("No_of_Rolls").ToString)


        Common_Procedures.Print_To_PrintDocument(e, no_of_rolls, LMargin + C11 - 15, CurY, 0, 0, p1Font)

        dt3.Clear()





       e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15) - 5, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15) - 5, LnAr(13))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15) + ClAr(16) + 20, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15) + ClAr(16) + 20, LnAr(13))
        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C22 + 20, CurY, LMargin + C22 + 20, LnAr(13))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), LnAr(13))





        Try
            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            Y1 = CurY + 0.5
            Y2 = CurY + TxtHgt - 10 + TxtHgt + 10
            Common_Procedures.FillRegionRectangle(e, LMargin, Y1, PageWidth, Y2)

            LnAr(10) = CurY
            CurY = CurY + 10
            pFont = New Font("Calibri", 8, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "GRAND TOTAL", LMargin + 10, CurY, 2, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 10, CurY, 1, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_HdDt.Rows(0).Item("p4").ToString)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_HdDt.Rows(0).Item("p3").ToString)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_HdDt.Rows(0).Item("p2").ToString)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_HdDt.Rows(0).Item("p1").ToString)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_HdDt.Rows(0).Item("Total_Points").ToString)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + +ClAr(8) + ClAr(9) + ClAr(10) - 10, CurY, 1, 0, p1Font)

            Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_HdDt.Rows(0).Item("Total_100L_mtrs").ToString)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + +ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) - 10, CurY, 1, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_HdDt.Rows(0).Item("Total_weight").ToString)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + +ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15) - 10, CurY, 1, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_HdDt.Rows(0).Item("Total_gross_weight").ToString)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + +ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15) + ClAr(16) - 10, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(2) = CurY

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "S.NO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Roll", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "No", LMargin + ClAr(1), CurY + TxtHgt, 2, ClAr(2), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Tot", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Mtrs", LMargin + ClAr(1) + ClAr(2), CurY + TxtHgt, 2, ClAr(3), pFont)


            Common_Procedures.Print_To_PrintDocument(e, "Loom", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "No", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY + TxtHgt, 2, ClAr(4), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Supplier", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "piece/Ref", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY + TxtHgt, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "no", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY + TxtHgt + TxtHgt, 2, ClAr(5), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "4", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "3", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "2", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "1", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Total", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, 2, ClAr(10), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "points", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY + TxtHgt, 2, ClAr(10), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "P.Per", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, 2, ClAr(11), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "100L", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY + TxtHgt, 2, ClAr(11), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "mtrs", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY + TxtHgt + TxtHgt, 2, ClAr(11), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Fabric", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), CurY, 2, ClAr(12), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Grade", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), CurY + TxtHgt, 2, ClAr(12), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Warp", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), CurY, 2, ClAr(13), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "lot", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), CurY + TxtHgt, 2, ClAr(13), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "no", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), CurY + TxtHgt + TxtHgt, 2, ClAr(13), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Weft", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13), CurY, 2, ClAr(14), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "lot", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13), CurY + TxtHgt, 2, ClAr(14), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "no", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13), CurY + TxtHgt + TxtHgt, 2, ClAr(14), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Net", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14), CurY, 2, ClAr(15), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Weight", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14), CurY + TxtHgt, 2, ClAr(15), pFont)


            Common_Procedures.Print_To_PrintDocument(e, "Gross", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15), CurY, 2, ClAr(16), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Weight", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15), CurY + TxtHgt, 2, ClAr(16), pFont)


            Common_Procedures.Print_To_PrintDocument(e, "Fabric Defects & Penalty", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15) + ClAr(16), CurY, 2, ClAr(17), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "point Details", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15) + ClAr(16), CurY + TxtHgt, 2, ClAr(17), pFont)

            CurY = CurY + TxtHgt + TxtHgt + 20
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY


            ' e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(10))


        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format1_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim i As Integer
        Dim p1Font As Font
        Dim W1, W2 As Single
        Dim C1 As Single
        Dim da2, da3 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable

        Dim NewCode As String = ""

        Dim Cmp_Add As String = ""
        Dim Range3 As Single

        C1 = ClAr(1) + ClAr(2) + ClAr(3)
        For i = NoofDets + 1 To NoofItems_PerPage
            CurY = CurY + TxtHgt
        Next

        W1 = e.Graphics.MeasureString("Empty Gunnies  :", pFont).Width
        W2 = e.Graphics.MeasureString("Empty Cones  :", pFont).Width
       
        ' no_of_Rolls = Val(dt3.Rows.Count.ToString + 1)
        da2 = New SqlClient.SqlDataAdapter("SELECT SUM(a.meters) as range3 FROM Roll_Packing_Details a WHERE  a.meters between 301 AND 400 And Roll_packing_Code='" & Trim(NewCode) & "'", con)
        da2.Fill(dt2)
        Range3 = Val(dt2.Rows(0).Item("range3").ToString)



        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(6) = CurY
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(2))

        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(10))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(10))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(10))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(10))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(10))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(10))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(10))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(10))

        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), LnAr(10))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), LnAr(10))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), LnAr(10))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13), LnAr(10))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14), LnAr(10))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15), LnAr(10))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15) + ClAr(16), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15) + ClAr(16), LnAr(10))

        pFont = New Font("Calibri", 8, FontStyle.Bold Or FontStyle.Underline)
        p1Font = New Font("Calibri", 7, FontStyle.Bold)
        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "Suppliers Acknowledgement ", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "*PL-Packing list ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "Commands ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13), CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "*RI-Random Inspections ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) + ClAr(14) + ClAr(15), CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), CurY)
        LnAr(18) = CurY
        Common_Procedures.Print_To_PrintDocument(e, "This PL fabric rolls are 100% free from hanging & Uncut threads", LMargin + ClAr(1), CurY, 0, 0, p1Font)
        CurY = CurY + TxtHgt - 5

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), CurY)
        Common_Procedures.Print_To_PrintDocument(e, "There Will not be any negative observation in EPI,PPI greay width & yarn count", LMargin + ClAr(1), CurY, 0, 0, p1Font)
        CurY = CurY + TxtHgt - 5

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), CurY)
        LnAr(17) = CurY
        Common_Procedures.Print_To_PrintDocument(e, "1st Line inspection report attached in rolls", LMargin + ClAr(1), CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "Yes", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, 2, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "Datas", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), CurY, 2, 0, p1Font)
        CurY = CurY + TxtHgt - 5


        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), CurY)
        Common_Procedures.Print_To_PrintDocument(e, "Is This PL contains 99% A Grade Fabric rolls", LMargin + ClAr(1), CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "Yes", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, 2, 0, p1Font)


        Common_Procedures.Print_To_PrintDocument(e, "100", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), CurY, 2, 0, p1Font)

        CurY = CurY + TxtHgt - 5


        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), CurY)
        Common_Procedures.Print_To_PrintDocument(e, "Is This  PL contains 95% -400 mtrsroll Length", LMargin + ClAr(1), CurY, 0, 0, p1Font)
        If (Range3 > 95) Then
            Common_Procedures.Print_To_PrintDocument(e, "Yes", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, 2, 0, p1Font)
        Else
            Common_Procedures.Print_To_PrintDocument(e, "No", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, 2, 0, p1Font)

        End If
        Common_Procedures.Print_To_PrintDocument(e, Val(Range3), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), CurY, 2, 0, p1Font)

        CurY = CurY + TxtHgt - 5


        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), CurY)
        Common_Procedures.Print_To_PrintDocument(e, "Is This PL Falls Under Avg. 12 Points/100 M", LMargin + ClAr(1), CurY, 0, 0, p1Font)
        If (Trim(Val(prn_HdDt.Rows(0).Item("Total_100L_mtrs").ToString)) > 12) Then
            Common_Procedures.Print_To_PrintDocument(e, "Yes", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, 2, 0, p1Font)
        Else
            Common_Procedures.Print_To_PrintDocument(e, "No", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, 2, 0, p1Font)

        End If
        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_HdDt.Rows(0).Item("Total_100L_mtrs").ToString)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + +ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), CurY, 2, 0, p1Font)

        CurY = CurY + TxtHgt - 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), CurY)
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), LnAr(17), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), CurY)

        Common_Procedures.Print_To_PrintDocument(e, "Is This PL Fabric gone under Feld random inspection", LMargin + ClAr(1), CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "Yes", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + 20, CurY, 0, 0, p1Font)
        CurY = CurY + TxtHgt - 5


        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), CurY)
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), LnAr(17), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY)
        Common_Procedures.Print_To_PrintDocument(e, "*If yes thenattach the random inspection reports", LMargin + ClAr(1), CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "Attached", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + 20, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt - 5

        p1Font = New Font("Calibri", 7, FontStyle.Bold)

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13), CurY)
        LnAr(19) = CurY
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), LnAr(18), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), LnAr(19))

        Common_Procedures.Print_To_PrintDocument(e, "Declaration :", LMargin + 5, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "If any inferior fabric quality found in grey or after processing as a fabrics supplier we whole", LMargin + ClAr(1) + ClAr(2) - 10, CurY, 0, 0, p1Font)
        CurY = CurY + 15
        Common_Procedures.Print_To_PrintDocument(e, " responsible for total value loss right from Grey/Transportion,dyeing/Colouring & Printing", LMargin + ClAr(1) + ClAr(2) - 10, CurY, 0, 0, p1Font)
        CurY = CurY + 15
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13), CurY)

        CurY = CurY + TxtHgt
        p1Font = New Font("Calibri", 9, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "Supplier authorized signatory ", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "ICIL Plant Random Inspection Team ", PageWidth - 50, CurY, 1, 0, p1Font)

        CurY = CurY + TxtHgt

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13), LnAr(6))

        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

    End Sub

    Private Sub cbo_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'GODOWN' or Ledger_Type = 'JOBWORKER' or (Ledger_Type = '' and AccountsGroup_IdNo IN (select tZ1.AccountsGroup_IdNo from AccountsGroup_Head tZ1 Where tZ1.Parent_Idno LIKE '%~10~4~%' or tZ1.Parent_Idno LIKE '%~14~11~%' ) ) or Show_In_All_Entry = 1 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PartyName, msk_date, cbo_Type, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'GODOWN' or Ledger_Type = 'JOBWORKER' or (Ledger_Type = '' and AccountsGroup_IdNo IN (select tZ1.AccountsGroup_IdNo from AccountsGroup_Head tZ1 Where tZ1.Parent_Idno LIKE '%~10~4~%' or tZ1.Parent_Idno LIKE '%~14~11~%' ) ) or Show_In_All_Entry = 1 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PartyName, cbo_Type, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'GODOWN' or Ledger_Type = 'JOBWORKER' or (Ledger_Type = '' and AccountsGroup_IdNo IN (select tZ1.AccountsGroup_IdNo from AccountsGroup_Head tZ1 Where tZ1.Parent_Idno LIKE '%~10~4~%' or tZ1.Parent_Idno LIKE '%~14~11~%' ) ) or Show_In_All_Entry = 1 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_PartyName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyName.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Common_Procedures.UR.Ledr_Wea_Siz_Rw_Trans_JbWrk_Creation = Common_Procedures.UR.Ledger_Creation
            Common_Procedures.MDI_LedType = ""
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_PartyName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub txt_PcsSelction_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_PcsSelction.KeyDown
        If e.KeyValue = 40 Then
            If dgv_Selection.Rows.Count > 0 Then
                dgv_Selection.Focus()
                dgv_Selection.CurrentCell = dgv_Selection.Rows(0).Cells(0)
                dgv_Selection.CurrentCell.Selected = True
            End If
        End If
        If (e.KeyValue = 38) Then txt_LotSelction.Focus()

    End Sub

    Private Sub txt_PcsSelction_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_PcsSelction.KeyPress
        If Asc(e.KeyChar) = 13 Then

            If Trim(txt_PcsSelction.Text) <> "" Or Trim(txt_PcsSelction.Text) <> "" Then
                btn_lot_Pcs_selection_Click(sender, e)

            Else
                If dgv_Selection.Rows.Count > 0 Then
                    dgv_Selection.Focus()
                    dgv_Selection.CurrentCell = dgv_Selection.Rows(0).Cells(0)
                    dgv_Selection.CurrentCell.Selected = True
                End If

            End If

        End If
    End Sub

    Private Sub txt_LotSelction_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_LotSelction.KeyDown
        If (e.KeyValue = 40) Then
            txt_PcsSelction.Focus()
        End If
    End Sub

    Private Sub txt_LotSelction_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_LotSelction.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_PcsSelction.Focus()
        End If
    End Sub

    Private Sub btn_lot_Pcs_selection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_lot_Pcs_selection.Click
        Dim LtNo As String
        Dim PcsNo As String
        Dim i As Integer

        If Trim(txt_LotSelction.Text) <> "" Or Trim(txt_PcsSelction.Text) <> "" Then

            LtNo = Trim(txt_LotSelction.Text)
            PcsNo = Trim(txt_PcsSelction.Text)

            For i = 0 To dgv_Selection.Rows.Count - 1
                If Trim(UCase(LtNo)) = Trim(UCase(dgv_Selection.Rows(i).Cells(1).Value)) And Trim(UCase(PcsNo)) = Trim(UCase(dgv_Selection.Rows(i).Cells(2).Value)) Then
                    Call Select_Piece(i)
                    dgv_Selection.CurrentCell = dgv_Selection.Rows(i).Cells(0)
                    If i >= 11 Then dgv_Selection.FirstDisplayedScrollingRowIndex = i - 10
                    Exit For
                End If
            Next

            txt_LotSelction.Text = ""
            txt_PcsSelction.Text = ""
            If txt_LotSelction.Enabled = True Then txt_LotSelction.Focus()

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
    Private Sub msk_Date_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles msk_date.LostFocus

        If IsDate(msk_date.Text) = True Then
            If Microsoft.VisualBasic.DateAndTime.Day(Convert.ToDateTime(msk_date.Text)) <= 31 Or Microsoft.VisualBasic.DateAndTime.Month(Convert.ToDateTime(msk_date.Text)) <= 31 Then
                If Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_date.Text)) <= 2050 And Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_date.Text)) >= 2000 Then
                    dtp_Date.Value = Convert.ToDateTime(msk_date.Text)
                End If
            End If

        End If
    End Sub

    Private Sub dtp_Date_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_Date.TextChanged
        If IsDate(dtp_Date.Text) = True Then
            msk_date.Text = dtp_Date.Text
        End If
    End Sub

    Private Sub msk_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_date.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        vmskOldText = ""
        vmskSelStrt = -1
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            vmskOldText = msk_date.Text
            vmskSelStrt = msk_date.SelectionStart
        End If

    End Sub

    Private Sub cbo_Type_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Type.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")
    End Sub

    Private Sub cbo_Type_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Type.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Type, cbo_PartyName, cbo_Cloth, "", "", "", "")
    End Sub

    Private Sub cbo_Type_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Type.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Type, cbo_Cloth, "", "", "", "")
    End Sub

    Private Sub dgtxt_Details_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles dgtxt_Details.TextChanged
        Try
            With dgv_Details
                If .CurrentCell.RowIndex >= 0 And .CurrentCell.ColumnIndex >= 0 Then
                    .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex).Value = Trim(dgtxt_Details.Text)
                End If
            End With

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Filter_Party_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_Party.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'GODOWN' or Ledger_Type = 'JOBWORKER' or (Ledger_Type = '' and AccountsGroup_IdNo IN (select tZ1.AccountsGroup_IdNo from AccountsGroup_Head tZ1 Where tZ1.Parent_Idno LIKE '%~10~4~%' or tZ1.Parent_Idno LIKE '%~14~11~%' ) ) or Show_In_All_Entry = 1 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_Party_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_Party.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_Party, dtp_Filter_ToDate, cbo_Filter_Cloth, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'GODOWN' or Ledger_Type = 'JOBWORKER' or (Ledger_Type = '' and AccountsGroup_IdNo IN (select tZ1.AccountsGroup_IdNo from AccountsGroup_Head tZ1 Where tZ1.Parent_Idno LIKE '%~10~4~%' or tZ1.Parent_Idno LIKE '%~14~11~%' ) ) or Show_In_All_Entry = 1 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_Party_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_Party.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_Party, cbo_Filter_Cloth, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'GODOWN' or Ledger_Type = 'JOBWORKER' or (Ledger_Type = '' and AccountsGroup_IdNo IN (select tZ1.AccountsGroup_IdNo from AccountsGroup_Head tZ1 Where tZ1.Parent_Idno LIKE '%~10~4~%' or tZ1.Parent_Idno LIKE '%~14~11~%' ) ) or Show_In_All_Entry = 1 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub txt_Filter_RollNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Filter_RollNo.KeyDown
        If e.KeyValue = 38 Then e.Handled = True : SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then e.Handled = True : SendKeys.Send("{TAB}")
    End Sub

    Private Sub txt_Filter_RollNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Filter_RollNo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            btn_Filter_Show.Focus()
            btn_Filter_Show_Click(sender, e)
        End If
    End Sub

    Private Sub txt_Tareweight_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Tareweight.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_Tareweight_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Tareweight.TextChanged
        Total_Calculation()
    End Sub

    Private Sub btn_UserModification_Click(sender As Object, e As System.EventArgs) Handles btn_UserModification.Click
        If Val(Common_Procedures.User.IdNo) = 1 Then
            Dim f1 As New User_Modifications
            f1.Entry_Name = Me.Name
            f1.Entry_PkValue = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            f1.ShowDialog()
        End If
    End Sub

    Private Sub dgv_Details_ChangeUICues(sender As Object, e As UICuesEventArgs) Handles dgv_Details.ChangeUICues

    End Sub
End Class