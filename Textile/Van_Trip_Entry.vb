Public Class Van_Trip_Entry
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)

    Private Insert_Entry As Boolean = False
    Private FrmLdSTS As Boolean = False
    Private new_entry As Boolean = False
    Private Prec_ActCtrl As Control
    Public vmskOldText As String = ""
    Public vmskSelStrt As Integer = -1
    Private Filter_Status As Boolean = False
    Private vcbo_KeyDwnVal As Double

    Private dgv_ActiveCtrl_Name As String

    Private TrnTo_DbName As String = ""

    Private Enum dgvCol_DelvDetails As Integer
        SlNo
        Dcno
        PartyName
        Particulars
        Yarn_Bag
        Pavu_Beams
        Empty_Beams
        Quality
        Pcs
        Meter
        Weight
        Entry_type
        Dc_Entry_Code
        Load_UnLoad_Status

        Yarn_Bag_Rate_for_Kgs
        Yarn_Loading_Rate
        Yarn_Unloading_Rate
        Pavu_Beam_Loading_Rate
        Pavu_Beam_unloading_Rate
        Empty_Beam_Loading_Rate
        Empty_Beam_Unloading_Rate
        Cloth_Rate_for_Kgs
        Cloth_Loading_Rate
        Cloth_UnLoading_Rate
        Yarn_Loading_Amount
        Yarn_Unloading_Amount
        Pavu_Beam_Loading_Amount
        Pavu_Beam_unloading_Amount
        Empty_Beam_Loading_Amount
        Empty_Beam_Unloading_Amount
        Cloth_Loading_Amount
        Cloth_UnLoading_Amount
        Cloth_Weight_Per_Meter
        Yarn_Weight_Per_Bag

    End Enum

    Private Enum dgvCol_RcptDetails As Integer
        SlNo
        Recno
        P_Dcno
        PartyName
        Particulars
        Yarn_Bag
        Pavu_Beams
        Empty_Beams
        Quality
        Pcs
        Meter
        Weight
        Entry_type
        Receipt_Entry_Code
        Load_UnLoad_Status

        Yarn_Bag_Rate_for_Kgs
        Yarn_Loading_Rate
        Yarn_Unloading_Rate
        Pavu_Beam_Loading_Rate
        Pavu_Beam_unloading_Rate
        Empty_Beam_Loading_Rate
        Empty_Beam_Unloading_Rate
        Cloth_Rate_for_Kgs
        Cloth_Loading_Rate
        Cloth_UnLoading_Rate
        Yarn_Loading_Amount
        Yarn_Unloading_Amount
        Pavu_Beam_Loading_Amount
        Pavu_Beam_unloading_Amount
        Empty_Beam_Loading_Amount
        Empty_Beam_Unloading_Amount
        Cloth_Loading_Amount
        Cloth_UnLoading_Amount
        Cloth_Weight_Per_Meter
        Yarn_Weight_Per_Bag

    End Enum

    Private Enum dgvCol_Selection As Integer
        SlNo
        Dcno
        PartydcNo
        PartyName
        Particulars
        Yarn_Bag
        Pavu_Beams
        Empty_Beams
        Quality
        Pcs
        Meter
        Weight
        STS
        Entry_Type
        Dc_Receipt_Entry_Code
        Load_UnLoad_Status
        Cloth_Weight_Per_Meter
        Yarn_Weight_Per_Bag

    End Enum

    Public Sub New()
        FrmLdSTS = True
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub


    Private Sub Van_Trip_Entry_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Try

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
            '----------------------------

        End Try

        FrmLdSTS = False

    End Sub

    Private Sub Van_Trip_Entry_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()

    End Sub

    Private Sub Van_Trip_Entry_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        Try

            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
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

    Private Sub Van_Trip_Entry_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Text = ""

        con.Open()

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2
        pnl_Filter.BringToFront()

        pnl_Selection.Visible = False
        pnl_Selection.Left = (Me.Width - pnl_Selection.Width) \ 2
        pnl_Selection.Top = (Me.Height - pnl_Selection.Height) \ 2
        pnl_Selection.BringToFront()

        AddHandler txt_van_bill_no.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_address.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_drivername.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_loadman.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Transport.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_vehicle.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_date.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Save.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_close.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_start_time.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_End_time.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_Fromdate.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_ToDate.GotFocus, AddressOf ControlGotFocus

        AddHandler dtp_Filter_Fromdate.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_ToDate.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_van_bill_no.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_address.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_drivername.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_loadman.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Transport.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_vehicle.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_date.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Save.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_close.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_start_time.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_End_time.LostFocus, AddressOf ControlLostFocus

        AddHandler dtp_Filter_Fromdate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_ToDate.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler txt_Freight_Charges.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Freight_Charges.LostFocus, AddressOf ControlLostFocus


        'If Common_Procedures.settings.Combine_Textile_Sizing_Software_Status = 1 Then
        '    TrnTo_DbName = Common_Procedures.get_Company_TextileDataBaseName(Trim(Val(Common_Procedures.CompGroupIdNo)))
        'Else
        '    TrnTo_DbName = Common_Procedures.get_Company_DataBaseName(Trim(Val(Common_Procedures.CompGroupIdNo)))
        'End If


        FrmLdSTS = True
        new_record()





    End Sub





    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        If FrmLdSTS = True Then Exit Sub

        Dim txtbx As TextBox
        Dim combobx As ComboBox
        Dim Msktxbx As MaskedTextBox

        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Me.ActiveControl Is MaskedTextBox Or TypeOf Me.ActiveControl Is Button Then
            Me.ActiveControl.BackColor = Color.Lime ' Color.MistyRose ' Color.PaleGreen
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

            Msktxbx.Select()

            Msktxbx.SelectionStart = 0
        End If

        If Me.ActiveControl.Name <> dgv_DeliveryDetails_Total.Name Then
            Grid_Cell_DeSelect()
        End If
        If Me.ActiveControl.Name <> dgv_ReceiptDetails_Total.Name Then
            Grid_Cell_DeSelect()
        End If
        Prec_ActCtrl = Me.ActiveControl

    End Sub



    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        If FrmLdSTS = True Then Exit Sub

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



    Private Sub ControlLostFocus2(ByVal sender As Object, ByVal e As System.EventArgs)

        On Error Resume Next

        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is Button Then
                Prec_ActCtrl.BackColor = Color.FromArgb(255, 90, 90)
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


    Public Sub clear()

        new_entry = False
        pnl_back.Enabled = True
        pnl_Selection.Visible = False

        msk_date.Text = dtp_date.Text

        lbl_Ref_no.Text = ""
        lbl_Ref_no.ForeColor = Color.Black
        cbo_Transport.Text = ""
        cbo_address.Text = ""
        cbo_drivername.Text = ""
        cbo_loadman.Text = ""
        cbo_Transport.Text = ""
        cbo_vehicle.Text = ""
        lbl_Loading_charges.Text = ""
        lbl_unloading_charges.Text = ""

        txt_van_bill_no.Text = ""
        dtp_start_time.Text = ""
        dtp_End_time.Text = ""
        txt_Freight_Charges.Text = ""

        If Filter_Status = False Then
            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            'cbo_Filter_BuyerName.Text = ""
            'cbo_Filter_BuyerName.SelectedIndex = -1


            dgv_Filter_Details.Rows.Clear()
        End If

        cbo_vehicle.Enabled = True
        cbo_vehicle.BackColor = Color.White

        cbo_address.Enabled = True
        cbo_address.BackColor = Color.White

        cbo_loadman.Enabled = True
        cbo_loadman.BackColor = Color.White

        cbo_drivername.Enabled = True
        cbo_drivername.BackColor = Color.White

        dgv_DeliveryDetails.Rows.Clear()
        dgv_DeliveryDetails_Total.Rows.Clear()
        dgv_DeliveryDetails_Total.Rows.Add()



        dgv_ReceiptDetails.Rows.Clear()
        dgv_ReceiptDetails_Total.Rows.Clear()
        dgv_ReceiptDetails_Total.Rows.Add()

        msk_date.Focus()

    End Sub
    Public Sub move_record(ByVal No As String)
        Dim da As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt As DataTable = New DataTable
        Dim dt2 As DataTable = New DataTable
        Dim da3 As New SqlClient.SqlDataAdapter
        Dim dt3 As DataTable = New DataTable
        Dim NewCode As String = ""
        Dim SNo, n As Integer
        Dim LockSTS As Boolean = False
        Dim SQL As String = ""


        If Val(No) = 0 Then Exit Sub


        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(No) & "/" & Trim(Common_Procedures.FnYearCode)

        clear()
        da = New SqlClient.SqlDataAdapter("Select a.*, b.ledger_name as Transport from Van_Trip_Head a, ledger_head b where a.Ref_Code = '" & Trim(NewCode) & "' and a.Transport_idno = b.ledger_idno", con)
        da.Fill(dt)


        If dt.Rows.Count > 0 Then
            lbl_Ref_no.Text = dt.Rows(0).Item("Ref_No").ToString
            txt_van_bill_no.Text = dt.Rows(0).Item("Van_Bill_No").ToString

            txt_Freight_Charges.Text = dt.Rows(0).Item("freight_charges").ToString

            dtp_date.Text = dt.Rows(0).Item("Date").ToString
            msk_date.Text = dt.Rows(0).Item("Date").ToString

            cbo_address.Text = dt.Rows(0).Item("Address").ToString
            cbo_drivername.Text = dt.Rows(0).Item("Driver_Name").ToString
            cbo_Transport.Text = Common_Procedures.Ledger_IdNoToName(con, dt.Rows(0).Item("Transport_IdNo").ToString)
            cbo_loadman.Text = dt.Rows(0).Item("Loadman_Name").ToString
            cbo_vehicle.Text = dt.Rows(0).Item("Vehicle_No").ToString

            dtp_start_time.Text = dt.Rows(0).Item("Start_Time").ToString
            dtp_End_time.Text = dt.Rows(0).Item("End_Time").ToString

            lbl_Loading_charges.Text = Format(Val(dt.Rows(0).Item("loading_charges").ToString), "##########0.00")
            lbl_unloading_charges.Text = Format(Val(dt.Rows(0).Item("unloading_Charges").ToString), "##########0.00")


            da2 = New SqlClient.SqlDataAdapter("select a.* from Van_Trip_Details a " &
                                               " INNER JOIN Ledger_Head b on b.Ledger_IdNo = a.Partyname_IdNo " &
                                               " where a.Ref_Code = '" & Trim(NewCode) &
                                               "' and (entry_type = 'YNDLV-DELIVERY' or entry_type = 'PVDLV-DELIVERY') " &
                                               " Order by a.Ref_no", con)
            dt2 = New DataTable
            da2.Fill(dt2)

            dgv_DeliveryDetails.Rows.Clear()
            SNo = 0

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_DeliveryDetails.Rows.Add()

                    SNo = SNo + 1
                    dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.SlNo).Value = Val(SNo)
                    dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Dcno).Value = dt2.Rows(i).Item("DC_No").ToString
                    dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.PartyName).Value = Common_Procedures.Ledger_IdNoToName(con, dt2.Rows(i).Item("Partyname_IdNo").ToString)
                    dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Particulars).Value = dt2.Rows(i).Item("particulars").ToString

                    dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Yarn_Bag).Value = Val(dt2.Rows(i).Item("Yarn_Bag").ToString)
                    If Val(dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Yarn_Bag).Value) = 0 Then dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Yarn_Bag).Value = ""

                    dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Pavu_Beams).Value = Val(dt2.Rows(i).Item("Pavu_beam").ToString)
                    If Val(dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Pavu_Beams).Value) = 0 Then dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Pavu_Beams).Value = ""

                    dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Empty_Beams).Value = Val(dt2.Rows(i).Item("Empty_beam").ToString)
                    If Val(dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Empty_Beams).Value) = 0 Then dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Empty_Beams).Value = ""

                    dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Quality).Value = dt2.Rows(i).Item("Cloth_Name").ToString

                    dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Pcs).Value = Val(dt2.Rows(i).Item("Pcs").ToString)
                    If Val(dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Pcs).Value) = 0 Then dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Pcs).Value = ""

                    dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Meter).Value = Val(dt2.Rows(i).Item("Meter").ToString)
                    If Val(dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Meter).Value) = 0 Then dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Meter).Value = ""

                    dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Weight).Value = Format(Val(dt2.Rows(i).Item("Weight").ToString), "########0.000")
                    If Val(dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Weight).Value) = 0 Then dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Weight).Value = ""

                    dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Entry_type).Value = dt2.Rows(i).Item("Entry_Type").ToString

                    dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Dc_Entry_Code).Value = dt2.Rows(i).Item("Dc_Code").ToString

                    dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Load_UnLoad_Status).Value = Val(dt2.Rows(i).Item("load_unload_status").ToString)

                    dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Yarn_Bag_Rate_for_Kgs).Value = Val(dt2.Rows(i).Item("Yarn_Bag_Rate_for_Kgs").ToString)
                    dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Yarn_Loading_Rate).Value = Val(dt2.Rows(i).Item("Yarn_Loading_Rate").ToString)
                    dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Yarn_Unloading_Rate).Value = Val(dt2.Rows(i).Item("Yarn_Unloading_rate").ToString)
                    dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Pavu_Beam_Loading_Rate).Value = Val(dt2.Rows(i).Item("Pavu_Beam_Loading_rate").ToString)
                    dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Pavu_Beam_unloading_Rate).Value = Val(dt2.Rows(i).Item("Pavu_Beam_unloading_Rate").ToString)
                    dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Empty_Beam_Loading_Rate).Value = Val(dt2.Rows(i).Item("Empty_Beam_Loading_Rate").ToString)
                    dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Empty_Beam_Unloading_Rate).Value = Val(dt2.Rows(i).Item("Empty_Beam_Unloading_Rate").ToString)
                    dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Cloth_Rate_for_Kgs).Value = Val(dt2.Rows(i).Item("Cloth_Rate_for_Kgs").ToString)
                    dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Cloth_Loading_Rate).Value = Val(dt2.Rows(i).Item("Cloth_Loading_Rate").ToString)
                    dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Cloth_UnLoading_Rate).Value = Val(dt2.Rows(i).Item("Cloth_UnLoading_Rate").ToString)

                    dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Yarn_Loading_Amount).Value = Val(dt2.Rows(i).Item("Yarn_Loading_Amount").ToString)
                    dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Yarn_Unloading_Amount).Value = Val(dt2.Rows(i).Item("Yarn_Unloading_Amount").ToString)
                    dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Pavu_Beam_Loading_Amount).Value = Val(dt2.Rows(i).Item("Pavu_Beam_Loading_Amount").ToString)
                    dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Pavu_Beam_unloading_Amount).Value = Val(dt2.Rows(i).Item("Pavu_Beam_Unloading_Amount").ToString)
                    dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Empty_Beam_Loading_Amount).Value = Val(dt2.Rows(i).Item("Empty_Beam_Loading_Amount").ToString)
                    dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Empty_Beam_Unloading_Amount).Value = Val(dt2.Rows(i).Item("Empty_Beam_Unloading_Amount").ToString)
                    dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Cloth_Loading_Amount).Value = Val(dt2.Rows(i).Item("Cloth_Loading_Amount").ToString)
                    dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Cloth_UnLoading_Amount).Value = Val(dt2.Rows(i).Item("Cloth_UnLoading_Amount").ToString)

                    dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Cloth_Weight_Per_Meter).Value = Val(dt2.Rows(i).Item("Cloth_Weight_Per_Meter").ToString)
                    dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Yarn_Weight_Per_Bag).Value = Val(dt2.Rows(i).Item("Yarn_Weight_Per_Bag").ToString)


                Next i

            End If
            dt2.Clear()

            With dgv_DeliveryDetails_Total
                If .RowCount = 0 Then .Rows.Add()
                .Rows(0).Cells(4).Value = Val(dt.Rows(0).Item("Total_yarn_Bags").ToString)
                .Rows(0).Cells(5).Value = Val(dt.Rows(0).Item("Total_pavu_Beams").ToString)
                .Rows(0).Cells(6).Value = Val(dt.Rows(0).Item("Total_Empty_Beams").ToString)
                .Rows(0).Cells(8).Value = Val(dt.Rows(0).Item("Total_Pcs").ToString)
                .Rows(0).Cells(9).Value = Val(dt.Rows(0).Item("Total_Meters").ToString)
                .Rows(0).Cells(10).Value = Format(Val(dt.Rows(0).Item("Total_weight").ToString), "########0.000")
            End With

            dt2.Dispose()
            da2.Dispose()


            SQL = "  select a.* from Van_Trip_Details a INNER JOIN Ledger_Head b on b.Ledger_IdNo = a.Partyname_IdNo " &
                    " where a.Ref_Code = '" & Trim(NewCode) & "' and (entry_type = 'EBREC-RECEIPT' or entry_type = 'EBREC-RECEIPT-BY-SIZING' ) " &
                    "         UNION ALL " &
                    " select a.* from Van_Trip_Details a   INNER JOIN  " & TrnTo_DbName & "..Ledger_Head b on b.Ledger_IdNo = a.Partyname_IdNo " &
                    " where a.Ref_Code = '" & Trim(NewCode) & "' and entry_type = 'FBREC-RECEIPT'  " &
                    " Order by Ref_no "

            da3 = New SqlClient.SqlDataAdapter(SQL, con)
            dt3 = New DataTable
            da3.Fill(dt3)

            dgv_ReceiptDetails.Rows.Clear()
            SNo = 0


            If dt3.Rows.Count > 0 Then

                For i = 0 To dt3.Rows.Count - 1

                    n = dgv_ReceiptDetails.Rows.Add()

                    SNo = SNo + 1
                    dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.SlNo).Value = Val(SNo)
                    dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.P_Dcno).Value = dt3.Rows(i).Item("party_DC_No").ToString
                    dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Recno).Value = dt3.Rows(i).Item("Recpt_No").ToString

                    If Trim(dt3.Rows(i).Item("Entry_Type").ToString).ToUpper = "FBREC-RECEIPT" Or Trim(dt3.Rows(i).Item("Entry_Type").ToString).ToUpper = "EBREC-RECEIPT" Or Trim(dt3.Rows(i).Item("Entry_Type").ToString).ToUpper = "EBREC-RECEIPT-BY-SIZING" Then
                        dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.PartyName).Value = Common_Procedures.Ledger_IdNoToName(con, dt3.Rows(i).Item("Partyname_IdNo").ToString, Nothing, TrnTo_DbName)
                    Else
                        dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.PartyName).Value = Common_Procedures.Vendor_IdNoToName(con, dt3.Rows(i).Item("Partyname_IdNo").ToString)
                    End If

                    dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Particulars).Value = dt3.Rows(i).Item("particulars").ToString

                    dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Yarn_Bag).Value = Val(dt3.Rows(i).Item("Yarn_Bag").ToString)
                    If Val(dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Yarn_Bag).Value) = 0 Then dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Yarn_Bag).Value = ""

                    dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Pavu_Beams).Value = Val(dt3.Rows(i).Item("Pavu_beam").ToString)
                    If Val(dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Pavu_Beams).Value) = 0 Then dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Pavu_Beams).Value = ""

                    dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Empty_Beams).Value = Val(dt3.Rows(i).Item("Empty_beam").ToString)
                    If Val(dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Empty_Beams).Value) = 0 Then dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Empty_Beams).Value = ""

                    dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Quality).Value = dt3.Rows(i).Item("Cloth_Name").ToString

                    dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Pcs).Value = Val(dt3.Rows(i).Item("Pcs").ToString)
                    If Val(dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Pcs).Value) = 0 Then dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Pcs).Value = ""

                    dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Meter).Value = Val(dt3.Rows(i).Item("Meter").ToString)
                    If Val(dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Meter).Value) = 0 Then dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Meter).Value = ""

                    dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Weight).Value = Format(Val(dt3.Rows(i).Item("Weight").ToString), "########0.000")
                    If Val(dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Weight).Value) = 0 Then dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Weight).Value = ""

                    dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Entry_type).Value = dt3.Rows(i).Item("Entry_Type").ToString

                    dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Receipt_Entry_Code).Value = dt3.Rows(i).Item("Dc_Code").ToString

                    dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Load_UnLoad_Status).Value = Val(dt3.Rows(i).Item("load_unload_status").ToString)

                    dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Yarn_Bag_Rate_for_Kgs).Value = Val(dt3.Rows(i).Item("Yarn_Bag_Rate_for_Kgs").ToString)
                    dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Yarn_Loading_Rate).Value = Val(dt3.Rows(i).Item("Yarn_Loading_Rate").ToString)
                    dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Yarn_Unloading_Rate).Value = Val(dt3.Rows(i).Item("Yarn_Unloading_rate").ToString)
                    dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Pavu_Beam_Loading_Rate).Value = Val(dt3.Rows(i).Item("Pavu_Beam_Loading_rate").ToString)
                    dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Pavu_Beam_unloading_Rate).Value = Val(dt3.Rows(i).Item("Pavu_Beam_unloading_Rate").ToString)
                    dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Empty_Beam_Loading_Rate).Value = Val(dt3.Rows(i).Item("Empty_Beam_Loading_Rate").ToString)
                    dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Empty_Beam_Unloading_Rate).Value = Val(dt3.Rows(i).Item("Empty_Beam_Unloading_Rate").ToString)
                    dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Cloth_Rate_for_Kgs).Value = Val(dt3.Rows(i).Item("Cloth_Rate_for_Kgs").ToString)
                    dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Cloth_Loading_Rate).Value = Val(dt3.Rows(i).Item("Cloth_Loading_Rate").ToString)
                    dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Cloth_UnLoading_Rate).Value = Val(dt3.Rows(i).Item("Cloth_UnLoading_Rate").ToString)

                    dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Yarn_Loading_Amount).Value = Val(dt3.Rows(i).Item("Yarn_Loading_Amount").ToString)
                    dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Yarn_Unloading_Amount).Value = Val(dt3.Rows(i).Item("Yarn_Unloading_Amount").ToString)
                    dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Pavu_Beam_Loading_Amount).Value = Val(dt3.Rows(i).Item("Pavu_Beam_Loading_Amount").ToString)
                    dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Pavu_Beam_unloading_Amount).Value = Val(dt3.Rows(i).Item("Pavu_Beam_Unloading_Amount").ToString)
                    dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Empty_Beam_Loading_Amount).Value = Val(dt3.Rows(i).Item("Empty_Beam_Loading_Amount").ToString)
                    dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Empty_Beam_Unloading_Amount).Value = Val(dt3.Rows(i).Item("Empty_Beam_Unloading_Amount").ToString)
                    dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Cloth_Loading_Amount).Value = Val(dt3.Rows(i).Item("Cloth_Loading_Amount").ToString)
                    dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Cloth_UnLoading_Amount).Value = Val(dt3.Rows(i).Item("Cloth_UnLoading_Amount").ToString)

                    dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Cloth_Weight_Per_Meter).Value = Val(dt3.Rows(i).Item("Cloth_Weight_Per_Meter").ToString)
                    dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Yarn_Weight_Per_Bag).Value = Val(dt3.Rows(i).Item("Yarn_Weight_Per_Bag").ToString)

                Next i

            End If

            dt3.Clear()

            With dgv_ReceiptDetails_Total
                If .RowCount = 0 Then .Rows.Add()
                .Rows(0).Cells(5).Value = Val(dt.Rows(0).Item("Total_Recpt_yarn_Bags").ToString)
                .Rows(0).Cells(6).Value = Val(dt.Rows(0).Item("Total_Recpt_pavu_Beams").ToString)
                .Rows(0).Cells(7).Value = Val(dt.Rows(0).Item("Total_Recpt_Empty_Beams").ToString)
                .Rows(0).Cells(9).Value = Val(dt.Rows(0).Item("Total_Recpt_Pcs").ToString)
                .Rows(0).Cells(10).Value = Val(dt.Rows(0).Item("Total_Recpt_Meters").ToString)
                .Rows(0).Cells(11).Value = Format(Val(dt.Rows(0).Item("Total_Recpt_weight").ToString), "########0.000")
            End With

            dt3.Dispose()
            da3.Dispose()

        End If

        TotalVan_Calculation()

        dt.Clear()
        dt.Dispose()
        da.Dispose()

        Grid_Cell_DeSelect()

        If (msk_date.Enabled And msk_date.Visible) Then msk_date.Focus()


    End Sub



    Public Sub delete_record() Implements Interface_MDIActions.delete_record

        Dim cmd As New SqlClient.SqlCommand
        Dim tr As SqlClient.SqlTransaction
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim NewCode As String = ""


        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.VanTrip_Entry, new_entry, Me) = False Then Exit Sub

        If MessageBox.Show("Do you want to Delete ?", "FOR DELETION....", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

        If new_entry = True Then
            MessageBox.Show("This is new entry", "DOES NOT DELETION....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If




        tr = con.BeginTransaction

        Try
            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_Ref_no.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            cmd.Connection = con

            cmd.Transaction = tr

            cmd.CommandText = "Update SizSoft_Yarn_Delivery_Head set van_trip_code = '' Where van_trip_code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Update Weaver_Pavu_Delivery_Head set van_trip_code = '' Where van_trip_code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Update Empty_BeamBagCone_Receipt_Head set van_trip_code = '' Where van_trip_code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Update " & TrnTo_DbName & "..Weaver_Cloth_Receipt_Head  set van_trip_code_Textile = '' Where van_trip_code_Textile = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Van_Trip_Head where Ref_No ='" & Trim(lbl_Ref_no.Text) & "' "
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Van_Trip_Details where Ref_No ='" & Trim(lbl_Ref_no.Text) & "' "
            cmd.ExecuteNonQuery()

            tr.Commit()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "For DELETION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "DOES Not DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            cmd.Dispose()
            If msk_date.Visible And msk_date.Enabled Then msk_date.Focus()

        End Try



    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        If Filter_Status = False Then

            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            'cbo_Filter_BuyerName.Text = ""
            'cbo_Filter_BuyerName.SelectedIndex = -1

            dgv_Filter_Details.Rows.Clear()

        End If

        pnl_Filter.Visible = True
        pnl_Filter.Enabled = True
        pnl_Filter.BringToFront()
        pnl_back.Enabled = False
        If dtp_Filter_Fromdate.Enabled And dtp_Filter_Fromdate.Visible Then dtp_Filter_Fromdate.Focus()

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        Dim InvCode As String = ""
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String


        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Pavu_Delivery_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.sales, "~I~") = 0 Then MessageBox.Show("You have No Rights To Insert", "DOES Not INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub


        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.VanTrip_Entry, new_entry, Me) = False Then Exit Sub

        Try

            inpno = InputBox("Enter New Ref.No.", "For New REF NO. INSERTION...")

            InvCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("Select Ref_No from Van_trip_Head where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " And Ref_Code = '" & Trim(InvCode) & "'", con)
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
                If Val(inpno) = 0 Then
                    MessageBox.Show("Invalid ReF.No.", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_Ref_no.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT ...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            Dt.Dispose()
            Da.Dispose()

        End Try








    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record

        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As String = ""

        Try
            da = New SqlClient.SqlDataAdapter("select top 1 Ref_No from Van_Trip_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Ref_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "%' Order by for_orderby , Ref_No ", con)
            dt = New DataTable
            da.Fill(dt)
            movid = ""

            If (dt.Rows.Count > 0) Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movid = dt.Rows(0)(0).ToString

                End If
            End If
            If Val(movid) <> 0 Then move_record(movid)

            dt.Dispose()
            da.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DO NOT SAVE......", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As String = ""

        Try
            da = New SqlClient.SqlDataAdapter("select top 1 Ref_No from Van_trip_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Ref_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "%' Order by for_orderby desc, Ref_No desc", con)
            dt = New DataTable
            da.Fill(dt)
            movid = ""

            If (dt.Rows.Count > 0) Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movid = dt.Rows(0)(0).ToString


                End If
            End If
            If Val(movid) <> 0 Then move_record(movid)

            dt.Dispose()
            da.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DO NOT SAVE......", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record


        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""
        Dim OrdByNo As String = ""

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_Ref_no.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Ref_No from Van_trip_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Ref_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Ref_No", con)
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
        Dim movid As String = ""

        Dim OrdByNo As Single = 0
        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_Ref_no.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Ref_No from Van_Trip_Head where for_orderby < " & Str(Val(OrdByNo)) & " and Company_idno = " & Str(Val(lbl_Company.Tag)) & " and Ref_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "%' Order by for_orderby desc, Ref_No desc", con)

            dt = New DataTable
            da.Fill(dt)

            movid = ""

            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movid = dt.Rows(0)(0).ToString
                End If
            End If

            If Val(movid) <> 0 Then move_record(movid)

            dt.Dispose()
            da.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try



    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record


        clear()

        new_entry = True

        lbl_Ref_no.ForeColor = Color.Red
        lbl_Ref_no.Text = Common_Procedures.get_MaxCode(con, "van_Trip_Head", "Ref_Code", "for_orderby", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String

        Try

            inpno = InputBox("Enter Ref No.", "FOR FINDING...")

            movno = inpno

            If Val(movno) <> 0 Then
                move_record(movno)
            Else
                MessageBox.Show("Ref No. does Not exists", "DOES Not FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES Not FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            Dt.Dispose()
            Da.Dispose()

            If dtp_date.Enabled And dtp_date.Visible Then dtp_date.Focus()

        End Try

    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        '-----
    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim vrSurnm As String = ""
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim vforOrdby As String = ""
        Dim vTrans_IdNo As Integer = 0
        Dim vPar_IdNo As Integer = 0
        Dim sno As Integer = 0
        Dim Nr As Integer = 0
        Dim vcLTH_IdNo As Integer = 0
        Dim vTotYrnBags As Single, vTotPvuBeams As Single, vTotEmptBeams As Single, vTotpcs As Single, vTotmtrs As Single, vTotWgt As Single
        Dim RTotYrnBags As Single, RTotPvuBeams As Single, RTotEmptBeams As Single, RTotpcs As Single, RTotmtrs As Single, RTotWgt As Single


        If pnl_back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES Not SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.VanTrip_Entry, new_entry, Me) = False Then Exit Sub


        If Trim(cbo_vehicle.Text) = "" Then
            MessageBox.Show("Invalid Vehicle No.", "DO NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            cbo_vehicle.Focus()
            Exit Sub
        End If

        vTrans_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Transport.Text)
        If vTrans_IdNo = 0 Then
            MessageBox.Show("Invalid Transport name", "DO NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            cbo_Transport.Focus()
            Exit Sub
        End If


        For i = 0 To dgv_DeliveryDetails.RowCount - 1

            If Val(dgv_DeliveryDetails.Rows(i).Cells(dgvCol_DelvDetails.Yarn_Bag).Value) <> 0 Or Val(dgv_DeliveryDetails.Rows(i).Cells(dgvCol_DelvDetails.Pavu_Beams).Value) <> 0 Or Val(dgv_DeliveryDetails.Rows(i).Cells(dgvCol_DelvDetails.Empty_Beams).Value) <> 0 Or Val(dgv_DeliveryDetails.Rows(i).Cells(dgvCol_DelvDetails.Pcs).Value) <> 0 Or Val(dgv_DeliveryDetails.Rows(i).Cells(dgvCol_DelvDetails.Meter).Value) <> 0 Or Val(dgv_DeliveryDetails.Rows(i).Cells(dgvCol_DelvDetails.Weight).Value) <> 0 Then

                If Trim(dgv_DeliveryDetails.Rows(i).Cells(dgvCol_DelvDetails.PartyName).Value) = "" Then

                    MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                    dgv_DeliveryDetails.CurrentCell = dgv_DeliveryDetails.Rows(0).Cells(dgvCol_DelvDetails.PartyName)

                    dgv_DeliveryDetails.Focus()

                    Exit Sub

                End If



                If Val(dgv_DeliveryDetails.Rows(i).Cells(dgvCol_DelvDetails.Pcs).Value) <> 0 Then

                    If Trim(dgv_DeliveryDetails.Rows(i).Cells(dgvCol_DelvDetails.Quality).Value) = "" Then

                        MessageBox.Show("Invalid Quality", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                        dgv_DeliveryDetails.CurrentCell = dgv_DeliveryDetails.Rows(0).Cells(dgvCol_DelvDetails.PartyName)

                        dgv_DeliveryDetails.Focus()

                        Exit Sub

                    End If

                End If

            End If

        Next

        For i = 0 To dgv_ReceiptDetails.RowCount - 1

            If Val(dgv_ReceiptDetails.Rows(i).Cells(dgvCol_RcptDetails.Yarn_Bag).Value) <> 0 Or Val(dgv_ReceiptDetails.Rows(i).Cells(dgvCol_RcptDetails.Pavu_Beams).Value) <> 0 Or Val(dgv_ReceiptDetails.Rows(i).Cells(dgvCol_RcptDetails.Empty_Beams).Value) <> 0 Or Val(dgv_ReceiptDetails.Rows(i).Cells(dgvCol_RcptDetails.Pcs).Value) <> 0 Or Val(dgv_ReceiptDetails.Rows(i).Cells(dgvCol_RcptDetails.Meter).Value) <> 0 Or Val(dgv_ReceiptDetails.Rows(i).Cells(dgvCol_RcptDetails.Weight).Value) <> 0 Then

                If Trim(dgv_ReceiptDetails.Rows(i).Cells(dgvCol_RcptDetails.PartyName).Value) = "" Then

                    MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                    dgv_ReceiptDetails.CurrentCell = dgv_ReceiptDetails.Rows(0).Cells(dgvCol_RcptDetails.PartyName)

                    dgv_ReceiptDetails.Focus()

                    Exit Sub

                End If

                If Val(dgv_ReceiptDetails.Rows(i).Cells(dgvCol_RcptDetails.Pcs).Value) <> 0 Then

                    If Trim(dgv_ReceiptDetails.Rows(i).Cells(dgvCol_RcptDetails.Quality).Value) = "" Then

                        MessageBox.Show("Invalid Quality", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                        dgv_ReceiptDetails.CurrentCell = dgv_ReceiptDetails.Rows(0).Cells(dgvCol_RcptDetails.PartyName)

                        dgv_ReceiptDetails.Focus()

                        Exit Sub

                    End If

                End If


            End If

        Next

        vTotYrnBags = 0 : vTotmtrs = 0 : vTotpcs = 0 : vTotWgt = 0 : vTotPvuBeams = 0 : vTotEmptBeams = 0
        If dgv_DeliveryDetails_Total.RowCount > 0 Then

            vTotYrnBags = Val(dgv_DeliveryDetails_Total.Rows(0).Cells(dgvCol_DelvDetails.Yarn_Bag).Value)
            vTotPvuBeams = Val(dgv_DeliveryDetails_Total.Rows(0).Cells(dgvCol_DelvDetails.Pavu_Beams).Value)
            vTotEmptBeams = Val(dgv_DeliveryDetails_Total.Rows(0).Cells(dgvCol_DelvDetails.Empty_Beams).Value)

            vTotpcs = Val(dgv_DeliveryDetails_Total.Rows(0).Cells(dgvCol_DelvDetails.Pcs).Value)
            vTotmtrs = Val(dgv_DeliveryDetails_Total.Rows(0).Cells(dgvCol_DelvDetails.Meter).Value)
            vTotWgt = Val(dgv_DeliveryDetails_Total.Rows(0).Cells(dgvCol_DelvDetails.Weight).Value)

        End If



        RTotYrnBags = 0 : RTotmtrs = 0 : RTotpcs = 0 : RTotWgt = 0 : RTotPvuBeams = 0 : RTotEmptBeams = 0
        If dgv_ReceiptDetails_Total.RowCount > 0 Then
            RTotYrnBags = Val(dgv_ReceiptDetails_Total.Rows(0).Cells(dgvCol_RcptDetails.Yarn_Bag).Value)
            RTotPvuBeams = Val(dgv_ReceiptDetails_Total.Rows(0).Cells(dgvCol_RcptDetails.Pavu_Beams).Value)
            RTotEmptBeams = Val(dgv_ReceiptDetails_Total.Rows(0).Cells(dgvCol_RcptDetails.Empty_Beams).Value)

            RTotpcs = Val(dgv_ReceiptDetails_Total.Rows(0).Cells(dgvCol_RcptDetails.Pcs).Value)
            RTotmtrs = Val(dgv_ReceiptDetails_Total.Rows(0).Cells(dgvCol_RcptDetails.Meter).Value)
            RTotWgt = Val(dgv_ReceiptDetails_Total.Rows(0).Cells(dgvCol_RcptDetails.Weight).Value)

        End If


        trans = con.BeginTransaction
        Try
            cmd.Connection = con

            cmd.Transaction = trans

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@EntryDate", Convert.ToDateTime(msk_date.Text))
            cmd.Parameters.AddWithValue("@StartTime", Convert.ToDateTime(dtp_start_time.Text))
            cmd.Parameters.AddWithValue("@EndTime", Convert.ToDateTime(dtp_End_time.Text))

            If Insert_Entry = True Or new_entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_Ref_no.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_Ref_no.Text = Common_Procedures.get_MaxCode(con, "Van_Trip_Head", "Ref_Code", "for_orderby", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, trans)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_Ref_no.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            vforOrdby = Val(Common_Procedures.OrderBy_CodeToValue(lbl_Ref_no.Text))

            If new_entry = True Then


                cmd.CommandText = "insert into Van_Trip_Head                 (       Ref_Code    ,                 Company_IdNo     ,                Ref_No          ,            for_OrderBy     , Date,        Transport_IdNo    ,                      Vehicle_No         ,             Driver_name          ,               Loadman_name       ,               Address                   , Van_bill_No                            , loading_charges                           ,      unloading_Charges                        , freight_Charges                          ,start_time  ,   End_time  ,   Total_YArn_Bags         ,Total_pavu_beams       ,   Total_Empty_beams       ,Total_pcs          ,Total_meters            ,Total_Weight,   Total_Recpt_Yarn_Bags         ,Total_Recpt_pavu_beams       ,   Total_Recpt_Empty_beams       ,Total_Recpt_pcs          ,Total_Recpt_meters            ,Total_Recpt_Weight) " &
                                    " Values                                ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ",  '" & Trim(lbl_Ref_no.Text) & "', " & Str(Val(vforOrdby)) & ",  @EntryDate , " & Val(vTrans_IdNo) & ", '" & Trim(cbo_vehicle.Text) & "', '" & Trim(cbo_drivername.Text) & "', '" & Trim(cbo_loadman.Text) & "', '" & Trim(cbo_address.Text) & "'  ,   '" & Trim(txt_van_bill_no.Text) & "'  , " & Str(Val(lbl_Loading_charges.Text)) & "     , " & Str(Val(lbl_unloading_charges.Text)) & ",   " & Str(Val(txt_Freight_Charges.Text)) & ",   @StartTime  ,@EndTime   ,    " & vTotYrnBags & "     ," & vTotPvuBeams & "   ,  " & vTotEmptBeams & "   ,  " & vTotpcs & "  ,   " & vTotmtrs & "    ,  " & vTotWgt & ",  " & RTotYrnBags & "     ," & RTotPvuBeams & "   ,  " & RTotEmptBeams & "   ,  " & RTotpcs & "  ,   " & RTotmtrs & "    ,  " & RTotWgt & ")"

                cmd.ExecuteNonQuery()

            Else

                cmd.CommandText = ("update Van_Trip_Head set Date = @EntryDate, Transport_IdNo = " & Val(vTrans_IdNo) & ", Vehicle_No = '" & Trim(cbo_vehicle.Text) & "' ,Driver_name= '" & Trim(cbo_drivername.Text) & "',Loadman_name='" & Trim(cbo_loadman.Text) & "',Address='" & Trim(cbo_address.Text) & "',  Van_bill_No    ='" & Trim(txt_van_bill_no.Text) & "' , loading_charges    =" & Str(Val(lbl_Loading_charges.Text)) & "  ,      unloading_Charges=" & Str(Val(lbl_unloading_charges.Text)) & "  , freight_Charges=" & Str(Val(txt_Freight_Charges.Text)) & " ,start_time =@StartTime  ,   End_time =@EndTime  ,Total_YArn_Bags=    " & vTotYrnBags & "     ,Total_pavu_beams=" & vTotPvuBeams & "   , Total_Empty_beams= " & vTotEmptBeams & "   ,Total_pcs=  " & vTotpcs & "  ,  Total_meters= " & vTotmtrs & "    , Total_Weight= " & vTotWgt & ", Total_Recpt_Yarn_bags=" & RTotYrnBags & " ,Total_Recpt_pavu_beams=" & RTotPvuBeams & "   , Total_Recpt_Empty_beams = " & RTotEmptBeams & "   , Total_Recpt_Pcs =" & RTotpcs & "  , Total_Recpt_meters=  " & RTotmtrs & "    ,Total_Recpt_Weight=  " & RTotWgt & " where Ref_Code = '" & Trim(NewCode) & "'")
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update Empty_BeamBagCone_Receipt_Head set van_trip_code = '' Where van_trip_code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update Weaver_Pavu_Delivery_Head set van_trip_code = '' Where van_trip_code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update SizSoft_Yarn_Delivery_Head set van_trip_code = '' Where van_trip_code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update  " & TrnTo_DbName & "..Weaver_Cloth_Receipt_Head set van_trip_code_Textile = '' Where van_trip_code_Textile = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If

            cmd.CommandText = "Delete from Van_trip_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Ref_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            With dgv_DeliveryDetails
                sno = 0

                For i = 0 To .RowCount - 1

                    If Trim(.Rows(i).Cells(dgvCol_DelvDetails.Dcno).Value) <> "" Then

                        sno = sno + 1



                        If Trim(UCase(.Rows(i).Cells(dgvCol_DelvDetails.Entry_type).Value)) = "PVDLV-DELIVERY" Or Trim(UCase(.Rows(i).Cells(dgvCol_DelvDetails.Entry_type).Value)) = "YNDLV-DELIVERY" Then
                            vPar_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, .Rows(i).Cells(dgvCol_DelvDetails.PartyName).Value, trans)
                        Else
                            vPar_IdNo = Common_Procedures.Vendor_AlaisNameToIdNo(con, .Rows(i).Cells(dgvCol_DelvDetails.PartyName).Value, trans)
                        End If



                        cmd.CommandText = "Insert into Van_trip_Details (          Ref_Code      ,               Company_IdNo        ,           Ref_No               ,                               for_OrderBy                                ,          Partyname_IdNo    ,            Sl_No      ,                    Dc_No                                       ,   Recpt_No , party_Dc_No ,                     Party_Vendor_Name                               ,                      particulars                                       ,                      Cloth_name                                    ,                       Yarn_bag                                     ,                       Pavu_Beam                                        ,                       Empty_beam                                       ,                      pcs                                       ,                      meter                                       ,                      Weight                                       ,                      Entry_Type                                       ,                    Dc_code                                             ,                      load_unload_status                                       ,                      Yarn_Bag_Rate_for_Kgs                                       ,                      Yarn_Loading_Rate                                       ,                      Yarn_Unloading_rate                                       ,                      Pavu_Beam_Loading_rate                                        ,                       Pavu_Beam_unloading_Rate                                       ,                      Empty_Beam_Loading_Rate                                        ,                      Empty_Beam_Unloading_Rate                                       ,                      Cloth_Rate_for_Kgs                                       ,                      Cloth_Loading_Rate                                      ,                      Cloth_UnLoading_Rate                                       ,                      Yarn_Loading_Amount                                        ,                      Yarn_Unloading_Amount                                         ,                      Pavu_Beam_Loading_Amount                                       ,                      Pavu_Beam_Unloading_Amount                                        ,                      Empty_Beam_Loading_Amount                                        ,                      Empty_Beam_Unloading_Amount                                      ,                      Cloth_Loading_Amount                                       ,                     Cloth_UnLoading_Amount                                         ,                      Cloth_Weight_Per_Meter                                       ,                      Yarn_Weight_Per_Bag                                       ) " &
                                            "          Values           ( '" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & " , '" & Trim(lbl_Ref_no.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_Ref_no.Text))) & " , " & Str(Val(vPar_IdNo)) & ", " & Str(Val(sno)) & " , '" & Trim(.Rows(i).Cells(dgvCol_DelvDetails.Dcno).Value) & "'  ,     ''     ,      ''     ,  '" & Trim(.Rows(i).Cells(dgvCol_DelvDetails.PartyName).Value) & "' ,   '" & Trim(.Rows(i).Cells(dgvCol_DelvDetails.Particulars).Value) & "' ,   '" & Trim(.Rows(i).Cells(dgvCol_DelvDetails.Quality).Value) & "' ,  " & Str(Val(.Rows(i).Cells(dgvCol_DelvDetails.Yarn_Bag).Value)) & ", " & Str(Val(.Rows(i).Cells(dgvCol_DelvDetails.Pavu_Beams).Value)) & " ,  " & Str(Val(.Rows(i).Cells(dgvCol_DelvDetails.Empty_Beams).Value)) & ", " & Str(Val(.Rows(i).Cells(dgvCol_DelvDetails.Pcs).Value)) & " , " & Str(Val(.Rows(i).Cells(dgvCol_DelvDetails.Meter).Value)) & " , " & Str(Val(.Rows(i).Cells(dgvCol_DelvDetails.Weight).Value)) & " ,   '" & Trim(.Rows(i).Cells(dgvCol_DelvDetails.Entry_type).Value) & "' , '" & Trim(.Rows(i).Cells(dgvCol_DelvDetails.Dc_Entry_Code).Value) & "' , " & Str(Val(.Rows(i).Cells(dgvCol_DelvDetails.Load_UnLoad_Status).Value)) & " , " & Str(Val(.Rows(i).Cells(dgvCol_DelvDetails.Yarn_Bag_Rate_for_Kgs).Value)) & " , " & Str(Val(.Rows(i).Cells(dgvCol_DelvDetails.Yarn_Loading_Rate).Value)) & " , " & Str(Val(.Rows(i).Cells(dgvCol_DelvDetails.Yarn_Unloading_Rate).Value)) & " , " & Str(Val(.Rows(i).Cells(dgvCol_DelvDetails.Pavu_Beam_Loading_Rate).Value)) & "  , " & Str(Val(.Rows(i).Cells(dgvCol_DelvDetails.Pavu_Beam_unloading_Rate).Value)) & "  , " & Str(Val(.Rows(i).Cells(dgvCol_DelvDetails.Empty_Beam_Loading_Rate).Value)) & "  , " & Str(Val(.Rows(i).Cells(dgvCol_DelvDetails.Empty_Beam_Unloading_Rate).Value)) & " , " & Str(Val(.Rows(i).Cells(dgvCol_DelvDetails.Cloth_Rate_for_Kgs).Value)) & " , " & Str(Val(.Rows(i).Cells(dgvCol_DelvDetails.Cloth_Loading_Rate).Value)) & ", " & Str(Val(.Rows(i).Cells(dgvCol_DelvDetails.Cloth_UnLoading_Rate).Value)) & " , " & Str(Val(.Rows(i).Cells(dgvCol_DelvDetails.Yarn_Loading_Amount).Value)) & "  , " & Str(Val(.Rows(i).Cells(dgvCol_DelvDetails.Yarn_Unloading_Amount).Value)) & "   , " & Str(Val(.Rows(i).Cells(dgvCol_DelvDetails.Pavu_Beam_Loading_Amount).Value)) & " , " & Str(Val(.Rows(i).Cells(dgvCol_DelvDetails.Pavu_Beam_unloading_Amount).Value)) & "  , " & Str(Val(.Rows(i).Cells(dgvCol_DelvDetails.Empty_Beam_Loading_Amount).Value)) & "  , " & Str(Val(.Rows(i).Cells(dgvCol_DelvDetails.Empty_Beam_Unloading_Amount).Value)) & ", " & Str(Val(.Rows(i).Cells(dgvCol_DelvDetails.Cloth_Loading_Amount).Value)) & " , " & Str(Val(.Rows(i).Cells(dgvCol_DelvDetails.Cloth_UnLoading_Amount).Value)) & "  , " & Str(Val(.Rows(i).Cells(dgvCol_DelvDetails.Cloth_Weight_Per_Meter).Value)) & " , " & Str(Val(.Rows(i).Cells(dgvCol_DelvDetails.Yarn_Weight_Per_Bag).Value)) & " ) "
                        cmd.ExecuteNonQuery()


                        If Trim(UCase(.Rows(i).Cells(dgvCol_DelvDetails.Entry_type).Value)) = "PVDLV-DELIVERY" Then
                            cmd.CommandText = "Update Weaver_Pavu_Delivery_Head set van_trip_code = '" & Trim(NewCode) & "' Where Weaver_Pavu_Delivery_Code = '" & Trim(.Rows(i).Cells(dgvCol_DelvDetails.Dc_Entry_Code).Value) & "' and Weaver_Pavu_Delivery_Date = @EntryDate "
                            cmd.ExecuteNonQuery()
                        ElseIf Trim(UCase(.Rows(i).Cells(dgvCol_DelvDetails.Entry_type).Value)) = "YNDLV-DELIVERY" Then
                            cmd.CommandText = "Update SizSoft_Yarn_Delivery_Head set van_trip_code = '" & Trim(NewCode) & "' Where Yarn_Delivery_Code = '" & Trim(.Rows(i).Cells(dgvCol_DelvDetails.Dc_Entry_Code).Value) & "'  and Yarn_Delivery_Date = @EntryDate"
                            cmd.ExecuteNonQuery()
                        End If

                    End If

                Next

            End With



            With dgv_ReceiptDetails

                For i = 0 To .RowCount - 1
                    If Trim(.Rows(i).Cells(dgvCol_RcptDetails.Recno).Value) <> "" Then
                        sno = sno + 1


                        If Trim(UCase(.Rows(i).Cells(dgvCol_RcptDetails.Entry_type).Value)) = "FBREC-RECEIPT" Or Trim(UCase(.Rows(i).Cells(dgvCol_RcptDetails.Entry_type).Value)) = "EBREC-RECEIPT" Or Trim(UCase(.Rows(i).Cells(dgvCol_RcptDetails.Entry_type).Value)) = "EBREC-RECEIPT-BY-SIZING" Then
                            vPar_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, .Rows(i).Cells(dgvCol_RcptDetails.PartyName).Value, trans, TrnTo_DbName)
                        Else
                            vPar_IdNo = Common_Procedures.Vendor_AlaisNameToIdNo(con, .Rows(i).Cells(dgvCol_RcptDetails.PartyName).Value, trans)
                        End If

                        cmd.CommandText = "Insert into Van_trip_Details (          Ref_Code        ,               Company_IdNo        ,           Ref_No                ,                               for_OrderBy                                 ,         Partyname_IdNo     ,    Sl_No                 ,  Dc_No ,                     Recpt_no                                     ,                    party_Dc_No                                   ,                      Party_Vendor_Name                             ,                        particulars                                       ,                       Cloth_name                                    ,                    Yarn_bag                                        ,                     Pavu_Beam                                        ,                    Empty_beam                                         ,                      pcs                                       ,                      meter                                        ,                       Weight                                        ,                      Entry_Type                                       ,                    Dc_code                                                  ,                      load_unload_status                                       ,                      Yarn_Bag_Rate_for_Kgs                                       ,                      Yarn_Loading_Rate                                       ,                      Yarn_Unloading_rate                                       ,                      Pavu_Beam_Loading_rate                                        ,                       Pavu_Beam_unloading_Rate                                       ,                      Empty_Beam_Loading_Rate                                        ,                      Empty_Beam_Unloading_Rate                                       ,                      Cloth_Rate_for_Kgs                                       ,                      Cloth_Loading_Rate                                      ,                      Cloth_UnLoading_Rate                                       ,                      Yarn_Loading_Amount                                        ,                      Yarn_Unloading_Amount                                         ,                      Pavu_Beam_Loading_Amount                                       ,                      Pavu_Beam_Unloading_Amount                                        ,                      Empty_Beam_Loading_Amount                                        ,                      Empty_Beam_Unloading_Amount                                      ,                      Cloth_Loading_Amount                                       ,                     Cloth_UnLoading_Amount                                         ,                      Cloth_Weight_Per_Meter                                       ,                      Yarn_Weight_Per_Bag                                       ) " &
                                            "          Values           (   '" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & "  , '" & Trim(lbl_Ref_no.Text) & "' ," & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_Ref_no.Text))) & "  , " & Str(Val(vPar_IdNo)) & ",  " & Str(Val(sno)) & "   ,     '' ,  '" & Trim(.Rows(i).Cells(dgvCol_RcptDetails.Recno).Value) & "'  , '" & Trim(.Rows(i).Cells(dgvCol_RcptDetails.P_Dcno).Value) & "'  , '" & Trim(.Rows(i).Cells(dgvCol_RcptDetails.PartyName).Value) & "' ,     '" & Trim(.Rows(i).Cells(dgvCol_RcptDetails.Particulars).Value) & "' ,    '" & Trim(.Rows(i).Cells(dgvCol_RcptDetails.Quality).Value) & "' , " & Str(Val(.Rows(i).Cells(dgvCol_RcptDetails.Yarn_Bag).Value)) & ", " & Str(Val(.Rows(i).Cells(dgvCol_RcptDetails.Pavu_Beams).Value)) & ", " & Str(Val(.Rows(i).Cells(dgvCol_RcptDetails.Empty_Beams).Value)) & ", " & Str(Val(.Rows(i).Cells(dgvCol_RcptDetails.Pcs).Value)) & " , " & Str(Val(.Rows(i).Cells(dgvCol_RcptDetails.Meter).Value)) & "   , " & Str(Val(.Rows(i).Cells(dgvCol_RcptDetails.Weight).Value)) & "  ,   '" & Trim(.Rows(i).Cells(dgvCol_RcptDetails.Entry_type).Value) & "' , '" & Trim(.Rows(i).Cells(dgvCol_RcptDetails.Receipt_Entry_Code).Value) & "' , " & Str(Val(.Rows(i).Cells(dgvCol_RcptDetails.Load_UnLoad_Status).Value)) & " , " & Str(Val(.Rows(i).Cells(dgvCol_RcptDetails.Yarn_Bag_Rate_for_Kgs).Value)) & " , " & Str(Val(.Rows(i).Cells(dgvCol_RcptDetails.Yarn_Loading_Rate).Value)) & " , " & Str(Val(.Rows(i).Cells(dgvCol_RcptDetails.Yarn_Unloading_Rate).Value)) & " , " & Str(Val(.Rows(i).Cells(dgvCol_RcptDetails.Pavu_Beam_Loading_Rate).Value)) & "  , " & Str(Val(.Rows(i).Cells(dgvCol_RcptDetails.Pavu_Beam_unloading_Rate).Value)) & "  , " & Str(Val(.Rows(i).Cells(dgvCol_RcptDetails.Empty_Beam_Loading_Rate).Value)) & "  , " & Str(Val(.Rows(i).Cells(dgvCol_RcptDetails.Empty_Beam_Unloading_Rate).Value)) & " , " & Str(Val(.Rows(i).Cells(dgvCol_RcptDetails.Cloth_Rate_for_Kgs).Value)) & " , " & Str(Val(.Rows(i).Cells(dgvCol_RcptDetails.Cloth_Loading_Rate).Value)) & ", " & Str(Val(.Rows(i).Cells(dgvCol_RcptDetails.Cloth_UnLoading_Rate).Value)) & " , " & Str(Val(.Rows(i).Cells(dgvCol_RcptDetails.Yarn_Loading_Amount).Value)) & "  , " & Str(Val(.Rows(i).Cells(dgvCol_RcptDetails.Yarn_Unloading_Amount).Value)) & "   , " & Str(Val(.Rows(i).Cells(dgvCol_RcptDetails.Pavu_Beam_Loading_Amount).Value)) & " , " & Str(Val(.Rows(i).Cells(dgvCol_RcptDetails.Pavu_Beam_unloading_Amount).Value)) & "  , " & Str(Val(.Rows(i).Cells(dgvCol_RcptDetails.Empty_Beam_Loading_Amount).Value)) & "  , " & Str(Val(.Rows(i).Cells(dgvCol_RcptDetails.Empty_Beam_Unloading_Amount).Value)) & ", " & Str(Val(.Rows(i).Cells(dgvCol_RcptDetails.Cloth_Loading_Amount).Value)) & " , " & Str(Val(.Rows(i).Cells(dgvCol_RcptDetails.Cloth_UnLoading_Amount).Value)) & "  , " & Str(Val(.Rows(i).Cells(dgvCol_RcptDetails.Cloth_Weight_Per_Meter).Value)) & " , " & Str(Val(.Rows(i).Cells(dgvCol_RcptDetails.Yarn_Weight_Per_Bag).Value)) & " ) "
                        cmd.ExecuteNonQuery()

                        If Trim(UCase(.Rows(i).Cells(dgvCol_RcptDetails.Entry_type).Value)) = "EBREC-RECEIPT-BY-SIZING" Then
                            cmd.CommandText = "Update SizSoft_Empty_BeamBagCone_Receipt_Head set van_trip_code = '" & Trim(NewCode) & "' Where Empty_BeamBagCone_Receipt_Code = '" & Trim(.Rows(i).Cells(dgvCol_RcptDetails.Receipt_Entry_Code).Value) & "' and Empty_BeamBagCone_Receipt_Date = @EntryDate"
                            cmd.ExecuteNonQuery()
                        ElseIf Trim(UCase(.Rows(i).Cells(dgvCol_RcptDetails.Entry_type).Value)) = "EBREC-RECEIPT" Then
                            cmd.CommandText = "Update Empty_BeamBagCone_Receipt_Head set van_trip_code = '" & Trim(NewCode) & "' Where Empty_BeamBagCone_Receipt_Code = '" & Trim(.Rows(i).Cells(dgvCol_RcptDetails.Receipt_Entry_Code).Value) & "' and Empty_BeamBagCone_Receipt_Date = @EntryDate"
                            cmd.ExecuteNonQuery()
                        ElseIf Trim(UCase(.Rows(i).Cells(dgvCol_RcptDetails.Entry_type).Value)) = "FBREC-RECEIPT" Then
                            cmd.CommandText = "Update  " & TrnTo_DbName & "..Weaver_Cloth_Receipt_Head set van_trip_code_Textile = '" & Trim(NewCode) & "' Where weaver_clothreceipt_code = '" & Trim(.Rows(i).Cells(dgvCol_RcptDetails.Receipt_Entry_Code).Value) & "'  and Weaver_ClothReceipt_Date = @EntryDate"
                            cmd.ExecuteNonQuery()
                        End If

                    End If

                Next
            End With

            trans.Commit()
            MessageBox.Show("Saved Successfully", "FOR SAVING", MessageBoxButtons.OK, MessageBoxIcon.Information)

            If (new_entry = True) Then
                new_record()
            Else
                move_record(lbl_Ref_no.Text)

            End If

        Catch ex As Exception
            trans.Rollback()
            If InStr(1, Trim(LCase(ex.Message)), Trim(LCase("PK_Van_Trip_Head"))) > 0 Then
                MessageBox.Show("Duplicate Entry", "Do Not Save", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Else

                MessageBox.Show(ex.Message, "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If
        Finally
            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()

        End Try

    End Sub

    Private Sub msk_CallDate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_date.KeyDown

        If e.KeyCode = 40 Then
            e.Handled = True : e.SuppressKeyPress = True
            cbo_vehicle.Focus()
        End If

        If e.KeyCode = 38 Then
            e.Handled = True : e.SuppressKeyPress = True
            txt_Freight_Charges.Focus()
        End If

        'vmskOldText = ""
        'vmskSelStrt = -1
        'If e.KeyCode = 46 Or e.KeyCode = 8 Then
        '    vmskOldText = msk_Date.Text
        '    vmskSelStrt = msk_Date.SelectionStart
        'End If
        vcbo_KeyDwnVal = e.KeyValue


        vmskOldText = ""
        vmskSelStrt = -1
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            vmskOldText = msk_date.Text
            vmskSelStrt = msk_date.SelectionStart
        End If

    End Sub



    Private Sub msk_Date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles msk_date.KeyPress
        If Trim(UCase(e.KeyChar)) = "D" Then
            msk_Date.Text = Date.Today
            msk_date.SelectionStart = 0

        End If
        If Asc(e.KeyChar) = 13 Then
            cbo_vehicle.Focus()
        End If




    End Sub

    Private Sub msk_CallDate_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_date.KeyUp
        Dim vmRetTxt As String = ""
        Dim vmRetSelStrt As Integer = -1

        If e.KeyCode = 107 Then
            msk_Date.Text = DateAdd("D", 1, Convert.ToDateTime(msk_Date.Text))
            msk_Date.SelectionStart = 0
        ElseIf e.KeyCode = 109 Then
            msk_Date.Text = DateAdd("D", -1, Convert.ToDateTime(msk_Date.Text))
            msk_Date.SelectionStart = 0
        End If

        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            Common_Procedures.maskEdit_Date_ON_DelBackSpace(sender, e, vmskOldText, vmskSelStrt)
        End If

    End Sub

    Private Sub msk_CallDate_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles msk_date.LostFocus
        If IsDate(msk_Date.Text) = True Then
            If Microsoft.VisualBasic.DateAndTime.Day(Convert.ToDateTime(msk_Date.Text)) <= 31 Or Microsoft.VisualBasic.DateAndTime.Month(Convert.ToDateTime(msk_Date.Text)) <= 31 Then
                If Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_Date.Text)) <= 2050 And Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_Date.Text)) >= 2000 Then
                    dtp_Date.Value = Convert.ToDateTime(msk_Date.Text)
                End If
            End If
        End If
    End Sub


    Private Sub dtp_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_date.KeyDown
        If e.KeyCode = 40 Then
            e.Handled = True : e.SuppressKeyPress = True
            cbo_vehicle.Focus()
        End If
        If e.KeyCode = 38 Then
            e.Handled = True : e.SuppressKeyPress = True
            msk_Date.Focus()
        End If
    End Sub

    Private Sub dtp_Date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_date.KeyPress
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            cbo_vehicle.Focus()
        End If
    End Sub

    Private Sub dtp_Date_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_date.TextChanged
        If IsDate(dtp_Date.Text) = True Then
            msk_Date.Text = dtp_Date.Text
            msk_Date.SelectionStart = 0
        End If
    End Sub





    Private Sub cbo_Filter_Transport_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_VehicleNo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Transport_Head", "Transport_Name", "", "(Transport_Idno = 0)")

    End Sub

    Private Sub cbo_Filter_Transport_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_VehicleNo.KeyDown




        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_VehicleNo, dtp_Filter_ToDate, dtp_date, "Van_Trip_Head", "", "", "(Ref_No = 0)")

        If (e.KeyValue = 38 And cbo_Transport.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
            dtp_Filter_ToDate.Focus()
        End If

        If (e.KeyValue = 40 And cbo_Transport.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            btn_Filter_Show.Focus()
        End If
    End Sub

    Private Sub cbo_Filter_Transport_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_VehicleNo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_VehicleNo, dgv_Filter_Details, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')  ", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_Transport_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Transport.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT') ", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_Transport_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Transport.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Transport, cbo_vehicle, txt_van_bill_no, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT') ", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_Transport_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Transport.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Transport, txt_van_bill_no, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")

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

    'Private Sub Grid_Cell_DeSelect()
    '    On Error Resume Next


    'End Sub


    Private Sub btn_save_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btn_Save.Click
        save_record()

    End Sub

    Private Sub btn_Close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()

    End Sub


    Private Sub dgv_Filter_Details_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Filter_Details.CellDoubleClick
        Open_FilterEntry()
    End Sub

    Private Sub dgv_Filter_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Filter_Details.KeyDown
        If e.KeyCode = 13 Then
            Open_FilterEntry()
        End If
    End Sub


    Private Sub Open_FilterEntry()

        Dim movno As String

        On Error Resume Next

        movno = Trim(dgv_Filter_Details.CurrentRow.Cells(0).Value)

        If Val(movno) <> 0 Then
            Filter_Status = True
            move_record(movno)
            pnl_back.Enabled = True
            pnl_Filter.Visible = False
        End If

    End Sub

    Private Sub cbo_vehicle_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_vehicle.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_vehicle, msk_date, cbo_Transport, "vehicle_Head", "Vehicle_No", "", "(vehicle_idno = 0)")

        If e.KeyValue = 38 Then
            msk_date.Focus()
        End If

    End Sub

    Private Sub cbo_vehicle_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_vehicle.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "vehicle_Head", "Vehicle_No", "", "(vehicle_idno = 0)")
    End Sub


    Private Sub cbo_vehicle_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_vehicle.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_vehicle, cbo_Transport, "vehicle_Head", "Vehicle_No", "", "(vehicle_idno = 0)")
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to select Delivery", "FOR DELIVERY SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                btn_Selection_Click(sender, e)

            Else
                cbo_Transport.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_drivername_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_drivername.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_drivername, txt_van_bill_no, cbo_loadman, "Van_Trip_Head", "Driver_Name", "", "Driver_Name")



        If (e.KeyValue = 38 And cbo_drivername.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

        End If
    End Sub

    Private Sub cbo_drivername_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_drivername.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_drivername, cbo_loadman, "Van_Trip_Head", "Driver_Name", "", "", False)

    End Sub

    Private Sub cbo_loadman_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_loadman.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_loadman, cbo_drivername, dtp_start_time, "", "", "", "")
    End Sub

    Private Sub cbo_loadman_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_loadman.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_loadman, dtp_start_time, "", "", "", "", False)

    End Sub

    Private Sub cbo_address_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_address.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_address, dtp_End_time, txt_Freight_Charges, "", "", "", "")

        'If e.KeyValue = 40 Then
        '    If MessageBox.Show("Do you want to save? ", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
        '        save_record()
        '    Else
        '        dtp_date.Focus()
        '    End If
        'End If

        'If (e.KeyValue = 38) Then
        '    dtp_End_time.Focus()


        'End If
    End Sub


    Private Sub cbo_address_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_address.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_address, txt_Freight_Charges, "", "", "", "", False)

    End Sub

    Private Sub btn_Filter_Close_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btn_Filter_Close.Click
        pnl_back.Enabled = True
        pnl_Filter.Visible = False
        Filter_Status = False
    End Sub

    Private Sub btn_Filter_Show_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter_Show.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim n As Integer
        Dim Led_IdNo As Integer
        Dim Condt As String = ""

        Try

            Condt = ""
            Led_IdNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_VehicleNo.Text) <> "" Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " (a.Vehicle_No = '" & Trim(cbo_Filter_VehicleNo.Text) & "')"
            End If


            da = New SqlClient.SqlDataAdapter("select a.*, b.Transport_Name as Transport from Van_trip_Head a LEFT OUTER JOIN Transport_Head b on a.Transport_Idno = b.Transport_IdNo where a.company_IdNo = " & Val(lbl_Company.Tag) & "    and  a.Ref_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Ref_No", con)
            'da = New SqlClient.SqlDataAdapter("select a.*, b.Transport_Name as Transport from Van_trip_Head a INNER JOIN Transport_Head b on a.Transport_Idno = b.Transport_IdNo where a.company_IdNo = " & Val(lbl_Company.Tag) & "    and  a.Ref_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Ref_No", con)
            dt = New DataTable
            da.Fill(dt)

            dgv_Filter_Details.Rows.Clear()

            If dt.Rows.Count > 0 Then

                For i = 0 To dt.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = dt.Rows(i).Item("Ref_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(1).Value = Format(Convert.ToDateTime(dt.Rows(i).Item("Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(2).Value = dt.Rows(i).Item("Vehicle_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt.Rows(i).Item("Transport").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = dt.Rows(i).Item("Van_bill_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(5).Value = dt.Rows(i).Item("Driver_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(6).Value = dt.Rows(i).Item("Loadman_name").ToString
                    dgv_Filter_Details.Rows(n).Cells(7).Value = dt.Rows(i).Item("Loading_charges").ToString
                    dgv_Filter_Details.Rows(n).Cells(8).Value = dt.Rows(i).Item("unLoading_charges").ToString
                    dgv_Filter_Details.Rows(n).Cells(9).Value = dt.Rows(i).Item("Freight_charges").ToString
                    dgv_Filter_Details.Rows(n).Cells(10).Value = dt.Rows(i).Item("Address").ToString
                Next i

            End If

            dt.Clear()
            dt.Dispose()
            da.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FILTER...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If dgv_Filter_Details.Visible And dgv_Filter_Details.Enabled Then dgv_Filter_Details.Focus()

    End Sub




    Private Sub Grid_Cell_DeSelect()
        If FrmLdSTS = True Then Exit Sub
        On Error Resume Next

        If IsNothing(dgv_DeliveryDetails.CurrentCell) Then Exit Sub
        If IsNothing(dgv_DeliveryDetails_Total.CurrentCell) Then Exit Sub
        If IsNothing(dgv_ReceiptDetails.CurrentCell) Then Exit Sub
        If IsNothing(dgv_ReceiptDetails_Total.CurrentCell) Then Exit Sub
        If IsNothing(dgv_Filter_Details.CurrentCell) Then Exit Sub

        dgv_DeliveryDetails.CurrentCell.Selected = False
        dgv_DeliveryDetails_Total.CurrentCell.Selected = False
        dgv_ReceiptDetails.CurrentCell.Selected = False
        dgv_ReceiptDetails_Total.CurrentCell.Selected = False
        dgv_Filter_Details.CurrentCell.Selected = False
    End Sub


    'Protected Overrides Function ProcessCmdKey_Alt(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
    '    Dim dgv1 As New DataGridView


    '    If ActiveControl.Name = dgv_Van_Trip_delivery_Details.Name Or ActiveControl.Name = dgv_receipt_details.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

    '        dgv1 = Nothing

    '        If ActiveControl.Name = dgv_Van_Trip_delivery_Details.Name Then
    '            dgv1 = dgv_Van_Trip_delivery_Details

    '        ElseIf ActiveControl.Name = dgv_receipt_details.Name Then
    '            dgv1 = dgv_receipt_details


    '        ElseIf dgv_Van_Trip_delivery_Details.IsCurrentRowDirty = True Then
    '            dgv1 = dgv_Van_Trip_delivery_Details


    '        ElseIf dgv_receipt_details.IsCurrentRowDirty = True Then
    '            dgv1 = dgv_receipt_details

    '        ElseIf dgv_ActiveCtrl_Name = dgv_Van_Trip_delivery_Details.Name Then
    '            dgv1 = dgv_Van_Trip_delivery_Details


    '        ElseIf dgv_ActiveCtrl_Name = dgv_receipt_details.Name Then
    '            dgv1 = dgv_receipt_details

    '        End If

    '        If IsNothing(dgv1) = False Then

    '            With dgv1
    '                If dgv1.Name = dgv_Van_Trip_delivery_Details.Name Then
    '                    If keyData = Keys.Enter Or keyData = Keys.Down Then
    '                        If .CurrentCell.ColumnIndex >= dgvCol_DelvDetails.Weight Then
    '                            If .CurrentCell.RowIndex = .RowCount - 1 Then
    '                                ' dgv_receipt_details.Focus()

    '                            Else
    '                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(dgvCol_DelvDetails.Dcno)


    '                            End If

    '                        ElseIf .CurrentCell.ColumnIndex <= 1 Then


    '                            If Trim(.Rows(.CurrentRow.Index).Cells(dgvCol_DelvDetails.Dcno).Value) = "" Then




    '                                dgv_receipt_details.Focus()
    '                                dgv_receipt_details.CurrentCell = dgv_receipt_details.Rows(0).Cells(dgvCol_RcptDetails.Recno)





    '                            Else

    '                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
    '                            End If
    '                        Else



    '                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)


    '                        End If




    '                        Return True




    '                    ElseIf keyData = Keys.Up Then

    '                        If .CurrentCell.ColumnIndex <= 1 Then
    '                            If .CurrentCell.RowIndex = 0 Then
    '                                cbo_loadman.Focus()
    '                            Else
    '                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 1)

    '                            End If

    '                        Else
    '                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)

    '                        End If

    '                        Return True

    '                    Else

    '                        Return MyBase.ProcessCmdKey(msg, keyData)

    '                    End If
    '                ElseIf dgv1.Name = dgv_receipt_details.Name Then
    '                    If keyData = Keys.Enter Or keyData = Keys.Down Then
    '                        If .CurrentCell.ColumnIndex >= dgvCol_RcptDetails.Weight Then
    '                            If .CurrentCell.RowIndex = .RowCount - 1 Then

    '                                dtp_start_time.Focus()



    '                            Else
    '                                '.CurrentCell = .Rows(.CurrentRow.Index + 1).Cells(1)
    '                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(dgvCol_RcptDetails.Recno)


    '                            End If

    '                        ElseIf .CurrentCell.ColumnIndex <= 1 Then

    '                            If Trim(.Rows(.CurrentRow.Index).Cells(1).Value) = "" Then

    '                                dtp_start_time.Focus()

    '                            Else

    '                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

    '                            End If

    '                        Else
    '                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
    '                        End If

    '                        Return True

    '                    ElseIf keyData = Keys.Up Then

    '                        If .CurrentCell.ColumnIndex <= 1 Then

    '                            If .CurrentCell.RowIndex = 0 Then





    '                                With dgv_Van_Trip_delivery_Details
    '                                    .Focus()

    '                                    .CurrentCell = .Rows(0).Cells(1)


    '                                End With

    '                            Else
    '                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 1)


    '                            End If

    '                        Else
    '                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)

    '                        End If
    '                        Return True

    '                    End If

    '                End If
    '            End With

    '        Else

    '            Return MyBase.ProcessCmdKey(msg, keyData)

    '        End If
    '    Else

    '        Return MyBase.ProcessCmdKey(msg, keyData)

    '    End If
    '    Return MyBase.ProcessCmdKey(msg, keyData)




    'End Function


    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
        Dim dgv1 As New DataGridView


        If ActiveControl.Name = dgv_DeliveryDetails.Name Or ActiveControl.Name = dgv_ReceiptDetails.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            dgv1 = Nothing

            If ActiveControl.Name = dgv_DeliveryDetails.Name Then
                dgv1 = dgv_DeliveryDetails

            ElseIf ActiveControl.Name = dgv_ReceiptDetails.Name Then
                dgv1 = dgv_ReceiptDetails


            ElseIf dgv_DeliveryDetails.IsCurrentRowDirty = True Then
                dgv1 = dgv_DeliveryDetails


            ElseIf dgv_ReceiptDetails.IsCurrentRowDirty = True Then
                dgv1 = dgv_ReceiptDetails

            ElseIf dgv_ActiveCtrl_Name = dgv_DeliveryDetails.Name Then
                dgv1 = dgv_DeliveryDetails


            ElseIf dgv_ActiveCtrl_Name = dgv_ReceiptDetails.Name Then
                dgv1 = dgv_ReceiptDetails

            End If

            If IsNothing(dgv1) = False Then


                With dgv1

                    If dgv1.Name = dgv_DeliveryDetails.Name Then
                        ' FIRST GRID DELIVERY DETAILS
                        If keyData = Keys.Enter Then
                            txt_Freight_Charges.Focus()
                            Return True

                        Else
                            Return MyBase.ProcessCmdKey(msg, keyData)

                        End If

                    ElseIf dgv1.Name = dgv_ReceiptDetails.Name Then
                        ' SECOND GRID RECEIPT DETAILS
                        If keyData = Keys.Enter Then
                            txt_Freight_Charges.Focus()
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


    Private Sub dgv_DeliveryDetails_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_DeliveryDetails.CellEnter
        If FrmLdSTS = True Then Exit Sub

        With dgv_DeliveryDetails

            If Val(.CurrentRow.Cells(dgvCol_DelvDetails.SlNo).Value) = 0 Then
                .CurrentRow.Cells(dgvCol_DelvDetails.SlNo).Value = .CurrentRow.Index + 1
            End If

        End With
    End Sub

    Private Sub dgv_DeliveryDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_DeliveryDetails.KeyUp
        Dim i, N As Integer

        If FrmLdSTS = True Then Exit Sub

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_DeliveryDetails

                N = .CurrentRow.Index

                If .Rows.Count = 1 Then
                    For i = 0 To .Columns.Count - 1
                        .Rows(N).Cells(i).Value = ""
                    Next

                Else

                    .Rows.RemoveAt(N)

                End If

                For i = 0 To .Rows.Count - 1
                    .Rows(i).Cells(0).Value = i + 1
                Next

            End With

        End If
    End Sub

    Private Sub dgv_DeliveryDetails_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_DeliveryDetails.LostFocus
        If FrmLdSTS = True Then Exit Sub
        On Error Resume Next
        dgv_DeliveryDetails.CurrentCell.Selected = False
    End Sub


    Private Sub TotalVan_Calculation()

        If FrmLdSTS = True Then Exit Sub

        Dim Sno As Integer
        Dim TotYrnBags As String, TotPvuBeams As String, TotEmptBeams As String, Totpcs As String, Totmtrs As String, TotWgt As String
        Dim rTotYrnBags As String, rTotPvuBeams As String, rTotEmptBeams As String, rTotpcs As String, rTotmtrs As String, rTotWgt As String

        Dim TotYrnLoadAmt As String, TotYrnUnLoadAmt As String
        Dim TotClothLoadAmt As String, TotClothUnLoadAmt As String
        Dim TotPavuLoadAmt As String, TotPavuUnLoadAmt As String
        Dim TotEbeamLoadAmt As String, TotEbeamUnLoadAmt As String

        Sno = 0
        TotYrnBags = 0 : TotPvuBeams = 0 : TotEmptBeams = 0 : Totpcs = 0 : Totmtrs = 0 : TotWgt = 0
        rTotYrnBags = 0 : rTotPvuBeams = 0 : rTotEmptBeams = 0 : rTotpcs = 0 : rTotmtrs = 0 : rTotWgt = 0

        TotYrnLoadAmt = 0 : TotYrnUnLoadAmt = 0
        TotClothLoadAmt = 0 : TotClothUnLoadAmt = 0
        TotPavuLoadAmt = 0 : TotPavuUnLoadAmt = 0
        TotEbeamLoadAmt = 0 : TotEbeamUnLoadAmt = 0

        With dgv_DeliveryDetails
            For i = 0 To .RowCount - 1
                TotYrnBags = Val(TotYrnBags) + Val(.Rows(i).Cells(dgvCol_DelvDetails.Yarn_Bag).Value)
                TotPvuBeams = Val(TotPvuBeams) + Val(.Rows(i).Cells(dgvCol_DelvDetails.Pavu_Beams).Value)
                TotEmptBeams = Val(TotEmptBeams) + Val(.Rows(i).Cells(dgvCol_DelvDetails.Empty_Beams).Value)
                Totpcs = Val(Totpcs) + Val(.Rows(i).Cells(dgvCol_DelvDetails.Pcs).Value)
                Totmtrs = Val(Totmtrs) + Val(.Rows(i).Cells(dgvCol_DelvDetails.Meter).Value)
                TotWgt = Val(TotWgt) + Val(.Rows(i).Cells(dgvCol_DelvDetails.Weight).Value)

                TotYrnLoadAmt = Val(TotYrnLoadAmt) + Val(.Rows(i).Cells(dgvCol_DelvDetails.Yarn_Loading_Amount).Value)
                TotYrnUnLoadAmt = Val(TotYrnUnLoadAmt) + Val(.Rows(i).Cells(dgvCol_DelvDetails.Yarn_Unloading_Amount).Value)
                TotClothLoadAmt = Val(TotClothLoadAmt) + Val(.Rows(i).Cells(dgvCol_DelvDetails.Cloth_Loading_Amount).Value)
                TotClothUnLoadAmt = Val(TotClothUnLoadAmt) + Val(.Rows(i).Cells(dgvCol_DelvDetails.Cloth_UnLoading_Amount).Value)
                TotPavuLoadAmt = Val(TotPavuLoadAmt) + Val(.Rows(i).Cells(dgvCol_DelvDetails.Pavu_Beam_Loading_Amount).Value)
                TotPavuUnLoadAmt = Val(TotPavuUnLoadAmt) + Val(.Rows(i).Cells(dgvCol_DelvDetails.Pavu_Beam_unloading_Amount).Value)
                TotEbeamLoadAmt = Val(TotEbeamLoadAmt) + Val(.Rows(i).Cells(dgvCol_DelvDetails.Empty_Beam_Loading_Amount).Value)
                TotEbeamUnLoadAmt = Val(TotEbeamUnLoadAmt) + Val(.Rows(i).Cells(dgvCol_DelvDetails.Empty_Beam_Unloading_Amount).Value)

            Next

        End With

        With dgv_DeliveryDetails_Total

            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(dgvCol_DelvDetails.Yarn_Bag).Value = Val(TotYrnBags)
            .Rows(0).Cells(dgvCol_DelvDetails.Pavu_Beams).Value = Val(TotPvuBeams)
            .Rows(0).Cells(dgvCol_DelvDetails.Empty_Beams).Value = Val(TotEmptBeams)
            .Rows(0).Cells(dgvCol_DelvDetails.Pcs).Value = Val(Totpcs)
            .Rows(0).Cells(dgvCol_DelvDetails.Meter).Value = Format(Val(Totmtrs), "########0.00")
            .Rows(0).Cells(dgvCol_DelvDetails.Weight).Value = Format(Val(TotWgt), "########0.000")

            .Rows(0).Cells(dgvCol_DelvDetails.Yarn_Loading_Amount).Value = Val(TotYrnLoadAmt)
            .Rows(0).Cells(dgvCol_DelvDetails.Yarn_Unloading_Amount).Value = Val(TotYrnUnLoadAmt)
            .Rows(0).Cells(dgvCol_DelvDetails.Cloth_Loading_Amount).Value = Val(TotClothLoadAmt)
            .Rows(0).Cells(dgvCol_DelvDetails.Cloth_UnLoading_Amount).Value = Val(TotClothUnLoadAmt)
            .Rows(0).Cells(dgvCol_DelvDetails.Pavu_Beam_Loading_Amount).Value = Val(TotPavuLoadAmt)
            .Rows(0).Cells(dgvCol_DelvDetails.Pavu_Beam_unloading_Amount).Value = Val(TotPavuUnLoadAmt)
            .Rows(0).Cells(dgvCol_DelvDetails.Empty_Beam_Loading_Amount).Value = Val(TotEbeamLoadAmt)
            .Rows(0).Cells(dgvCol_DelvDetails.Empty_Beam_Unloading_Amount).Value = Val(TotEbeamUnLoadAmt)

        End With

        lbl_Loading_charges.Text = Format(Val(TotYrnLoadAmt) + Val(TotClothLoadAmt) + Val(TotPavuLoadAmt) + Val(TotEbeamLoadAmt), "#######0.00")
        lbl_unloading_charges.Text = Format(Val(TotYrnUnLoadAmt) + Val(TotClothUnLoadAmt) + Val(TotPavuUnLoadAmt) + Val(TotEbeamUnLoadAmt), "#######0.00")

        TotYrnLoadAmt = 0 : TotYrnUnLoadAmt = 0
        TotClothLoadAmt = 0 : TotClothUnLoadAmt = 0
        TotPavuLoadAmt = 0 : TotPavuUnLoadAmt = 0
        TotEbeamLoadAmt = 0 : TotEbeamUnLoadAmt = 0

        With dgv_ReceiptDetails

            For i = 0 To .RowCount - 1

                rTotYrnBags = Val(rTotYrnBags) + Val(.Rows(i).Cells(dgvCol_RcptDetails.Yarn_Bag).Value)
                rTotPvuBeams = Val(rTotPvuBeams) + Val(.Rows(i).Cells(dgvCol_RcptDetails.Pavu_Beams).Value)
                rTotEmptBeams = Val(rTotEmptBeams) + Val(.Rows(i).Cells(dgvCol_RcptDetails.Empty_Beams).Value)
                rTotpcs = Val(rTotpcs) + Val(.Rows(i).Cells(dgvCol_RcptDetails.Pcs).Value)
                rTotmtrs = Val(rTotmtrs) + Val(.Rows(i).Cells(dgvCol_RcptDetails.Meter).Value)
                rTotWgt = Val(rTotWgt) + Val(.Rows(i).Cells(dgvCol_RcptDetails.Weight).Value)

                TotYrnLoadAmt = Val(TotYrnLoadAmt) + Val(.Rows(i).Cells(dgvCol_RcptDetails.Yarn_Loading_Amount).Value)
                TotYrnUnLoadAmt = Val(TotYrnUnLoadAmt) + Val(.Rows(i).Cells(dgvCol_RcptDetails.Yarn_Unloading_Amount).Value)
                TotClothLoadAmt = Val(TotClothLoadAmt) + Val(.Rows(i).Cells(dgvCol_RcptDetails.Cloth_Loading_Amount).Value)
                TotClothUnLoadAmt = Val(TotClothUnLoadAmt) + Val(.Rows(i).Cells(dgvCol_RcptDetails.Cloth_UnLoading_Amount).Value)
                TotPavuLoadAmt = Val(TotPavuLoadAmt) + Val(.Rows(i).Cells(dgvCol_RcptDetails.Pavu_Beam_Loading_Amount).Value)
                TotPavuUnLoadAmt = Val(TotPavuUnLoadAmt) + Val(.Rows(i).Cells(dgvCol_RcptDetails.Pavu_Beam_unloading_Amount).Value)
                TotEbeamLoadAmt = Val(TotEbeamLoadAmt) + Val(.Rows(i).Cells(dgvCol_RcptDetails.Empty_Beam_Loading_Amount).Value)
                TotEbeamUnLoadAmt = Val(TotEbeamUnLoadAmt) + Val(.Rows(i).Cells(dgvCol_RcptDetails.Empty_Beam_Unloading_Amount).Value)

            Next

        End With

        With dgv_ReceiptDetails_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(dgvCol_RcptDetails.Yarn_Bag).Value = Val(rTotYrnBags)
            .Rows(0).Cells(dgvCol_RcptDetails.Pavu_Beams).Value = Val(rTotPvuBeams)
            .Rows(0).Cells(dgvCol_RcptDetails.Empty_Beams).Value = Val(rTotEmptBeams)
            .Rows(0).Cells(dgvCol_RcptDetails.Pcs).Value = Val(rTotpcs)
            .Rows(0).Cells(dgvCol_RcptDetails.Meter).Value = Format(Val(rTotmtrs), "########0.00")
            .Rows(0).Cells(dgvCol_RcptDetails.Weight).Value = Format(Val(rTotWgt), "########0.000")

            .Rows(0).Cells(dgvCol_RcptDetails.Yarn_Loading_Amount).Value = Val(TotYrnLoadAmt)
            .Rows(0).Cells(dgvCol_RcptDetails.Yarn_Unloading_Amount).Value = Val(TotYrnUnLoadAmt)
            .Rows(0).Cells(dgvCol_RcptDetails.Cloth_Loading_Amount).Value = Val(TotClothLoadAmt)
            .Rows(0).Cells(dgvCol_RcptDetails.Cloth_UnLoading_Amount).Value = Val(TotClothUnLoadAmt)
            .Rows(0).Cells(dgvCol_RcptDetails.Pavu_Beam_Loading_Amount).Value = Val(TotPavuLoadAmt)
            .Rows(0).Cells(dgvCol_RcptDetails.Pavu_Beam_unloading_Amount).Value = Val(TotPavuUnLoadAmt)
            .Rows(0).Cells(dgvCol_RcptDetails.Empty_Beam_Loading_Amount).Value = Val(TotEbeamLoadAmt)
            .Rows(0).Cells(dgvCol_RcptDetails.Empty_Beam_Unloading_Amount).Value = Val(TotEbeamUnLoadAmt)

        End With

        lbl_Loading_charges.Text = Format(Val(lbl_Loading_charges.Text) + Val(TotYrnLoadAmt) + Val(TotClothLoadAmt) + Val(TotPavuLoadAmt) + Val(TotEbeamLoadAmt), "#######0.00")
        lbl_unloading_charges.Text = Format(Val(lbl_unloading_charges.Text) + Val(TotYrnUnLoadAmt) + Val(TotClothUnLoadAmt) + Val(TotPavuUnLoadAmt) + Val(TotEbeamUnLoadAmt), "#######0.00")

    End Sub



    Private Sub dgv_DeliveryDetails_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_DeliveryDetails.RowsAdded
        Dim n As Integer
        If FrmLdSTS = True Then Exit Sub
        With dgv_DeliveryDetails
            n = .RowCount
            .Rows(n - 1).Cells(dgvCol_DelvDetails.SlNo).Value = Val(n)
        End With
    End Sub


    Private Sub txt_van_bill_no_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_van_bill_no.KeyDown
        If e.KeyValue = 38 Then
            cbo_Transport.Focus()
        ElseIf e.KeyValue = 40 Then
            cbo_drivername.Focus()
        End If

    End Sub

    Private Sub txt_van_bill_no_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_van_bill_no.KeyPress
        If Asc(e.KeyChar) = 13 Then
            cbo_drivername.Focus()
        End If
    End Sub


    Private Sub dgv_ReceiptDetails_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_ReceiptDetails.CellEnter
        If FrmLdSTS = True Then Exit Sub

        With dgv_ReceiptDetails
            If Val(.CurrentRow.Cells(dgvCol_RcptDetails.SlNo).Value) = 0 Then
                .CurrentRow.Cells(dgvCol_RcptDetails.SlNo).Value = .CurrentRow.Index + 1
            End If
        End With

    End Sub

    Private Sub dgv_ReceiptDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_ReceiptDetails.KeyUp
        Dim i, N As Integer

        If FrmLdSTS = True Then Exit Sub

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_ReceiptDetails

                N = .CurrentRow.Index

                If .Rows.Count = 1 Then
                    For i = 0 To .Columns.Count - 1
                        .Rows(N).Cells(i).Value = ""
                    Next

                Else

                    .Rows.RemoveAt(N)

                End If

                For i = 0 To .Rows.Count - 1
                    .Rows(i).Cells(0).Value = i + 1
                Next

            End With

        End If

    End Sub

    Private Sub dgv_ReceiptDetails_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_ReceiptDetails.LostFocus
        If FrmLdSTS = True Then Exit Sub
        On Error Resume Next
        If IsNothing(dgv_ReceiptDetails.CurrentCell) Then Exit Sub
        dgv_ReceiptDetails.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_ReceiptDetails_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_ReceiptDetails.RowsAdded
        Dim n As Integer
        If FrmLdSTS = True Then Exit Sub
        With dgv_ReceiptDetails
            n = .RowCount
            .Rows(n - 1).Cells(dgvCol_RcptDetails.SlNo).Value = Val(n)
        End With
    End Sub

    Private Sub dtp_start_time_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_start_time.KeyDown
        If (e.KeyValue = 38) Then
            cbo_loadman.Focus()
        End If

        If (e.KeyValue = 40) Then
            dtp_End_time.Focus()

        End If
    End Sub

    Private Sub btn_Selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Selection.Click
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable

        Dim Da2 As New SqlClient.SqlDataAdapter
        Dim Dt2 As New DataTable

        Dim i As Integer, j As Integer, n As Integer, SNo As Integer
        Dim Trans_IdNo As Integer
        Dim NewCode As String = ""
        Dim DelNo As String = ""
        Dim Ent_Rate As Single = 0
        Dim Ent_Wgt As Single = 0
        Dim Ent_Pcs As Single = 0
        Dim NR As Single = 0
        Dim cmd As New SqlClient.SqlCommand
        Dim SQL As String = ""
        Dim tempcalc As Single = 0
        Dim loopcount As Integer = 0


        cmd.Connection = con

        Trans_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Transport.Text)

        If Trim(cbo_vehicle.Text) = "" Then
            MessageBox.Show("Invalid Vehicle No", "DOES NOT SELECT DELIVERY/RECEIPT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_vehicle.Enabled And cbo_vehicle.Visible Then cbo_vehicle.Focus()
            Exit Sub
        End If

        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@VanTripDate", Convert.ToDateTime(msk_date.Text))


        With dgv_Selection

            .Rows.Clear()
            SNo = 0

            For loopcount = 1 To 2

                NewCode = ""
                If loopcount = 1 Then
                    NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_Ref_no.Text) & "/" & Trim(Common_Procedures.FnYearCode)
                End If


                '----- YARN DELIVERY  

                cmd.CommandText = "Select a.*, b.*, v.ledger_Name , c.Count_Name, m.mill_name from SizSoft_Yarn_Delivery_Head a " &
                            " INNER JOIN SizSoft_Yarn_Delivery_Details b ON A.Yarn_Delivery_Code = B.Yarn_Delivery_Code  " &
                            " INNER JOIN Count_Head c ON c.Count_IdNo = b.Count_Idno " &
                            " INNER JOIN Ledger_Head v ON v.ledger_IdNo = a.Vendor_IdNo " &
                            " LEFT OUTER JOIN Mill_Head m ON m.Mill_IdNo = b.Mill_IdNo " &
                            " where a.van_trip_code = '" & Trim(NewCode) & "' and upper(replace(a.Vehicle_No,' ','')) = upper(replace('" & Trim(cbo_vehicle.Text) & "',' ','')) and " &
                            " a.Yarn_Delivery_Date = @VanTripDate and a.Vendor_IdNo <> 0  " &
                            " order by a.Yarn_Delivery_Date, a.for_orderby, a.Yarn_Delivery_No"
                Da = New SqlClient.SqlDataAdapter(cmd)
                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then
                    DelNo = ""
                    For i = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()

                        SNo = SNo + 1
                        .Rows(n).Cells(dgvCol_Selection.SlNo).Value = Val(SNo)
                        .Rows(n).Cells(dgvCol_Selection.Dcno).Value = Dt1.Rows(i).Item("Yarn_Delivery_No").ToString
                        .Rows(n).Cells(dgvCol_Selection.PartydcNo).Value = ""
                        .Rows(n).Cells(dgvCol_Selection.PartyName).Value = Dt1.Rows(i).Item("ledger_Name").ToString  ' Common_Procedures.Vendor_IdNoToName(con, Val(Dt1.Rows(i).Item("Vendor_IdNo").ToString))
                        .Rows(n).Cells(dgvCol_Selection.Particulars).Value = Dt1.Rows(i).Item("Count_Name").ToString & " - " & Dt1.Rows(i).Item("Yarn_Type").ToString & " - " & Dt1.Rows(i).Item("Mill_Name").ToString
                        .Rows(n).Cells(dgvCol_Selection.Yarn_Bag).Value = Format(Val(Dt1.Rows(i).Item("Bags").ToString), "#######0")
                        .Rows(n).Cells(dgvCol_Selection.Pavu_Beams).Value = ""
                        .Rows(n).Cells(dgvCol_Selection.Empty_Beams).Value = ""
                        .Rows(n).Cells(dgvCol_Selection.Quality).Value = ""
                        .Rows(n).Cells(dgvCol_Selection.Pcs).Value = ""
                        .Rows(n).Cells(dgvCol_Selection.Meter).Value = ""
                        .Rows(n).Cells(dgvCol_Selection.Weight).Value = "" 'Format(Val(Dt1.Rows(i).Item("Weight").ToString), "#######0.000") '"" ' 

                        .Rows(n).Cells(dgvCol_Selection.STS).Value = ""
                        If Trim(NewCode) <> "" Then
                            .Rows(n).Cells(dgvCol_Selection.STS).Value = "1"
                        End If

                        .Rows(n).Cells(dgvCol_Selection.Entry_Type).Value = "YNDLV-DELIVERY"
                        .Rows(n).Cells(dgvCol_Selection.Dc_Receipt_Entry_Code).Value = Dt1.Rows(i).Item("Yarn_Delivery_Code").ToString
                        .Rows(n).Cells(dgvCol_Selection.Load_UnLoad_Status).Value = Dt1.Rows(i).Item("Loaded_by_Our_Employee").ToString
                        .Rows(n).Cells(dgvCol_Selection.Cloth_Weight_Per_Meter).Value = 0
                        .Rows(n).Cells(dgvCol_Selection.Yarn_Weight_Per_Bag).Value = 0
                        If Val(Dt1.Rows(i).Item("Bags").ToString) <> 0 Then
                            .Rows(n).Cells(dgvCol_Selection.Yarn_Weight_Per_Bag).Value = Format(Val(Dt1.Rows(i).Item("Weight").ToString) / Val(Dt1.Rows(i).Item("Bags").ToString), "#######0.000")
                        End If

                        If Val(.Rows(n).Cells(dgvCol_Selection.STS).Value) = 1 Then
                            For j = 0 To .ColumnCount - 1
                                .Rows(n).Cells(dgvCol_Selection.STS).Style.ForeColor = Color.Red
                            Next
                        End If

                    Next

                End If
                Dt1.Clear()

                '----- Weaver PAVU DELIVERY 

                cmd.CommandText = "Select a.Weaver_Pavu_Delivery_Code, a.Weaver_Pavu_Delivery_Date, a.for_orderby, a.Weaver_Pavu_Delivery_No, v.ledger_name, b.Set_No, Eh.Ends_Name, c.Count_Name, a.Loaded_by_Our_Employee, count(b.Beam_No) as Noof_Beams " &
                                    " from Weaver_Pavu_Delivery_Head a  " &
                                    " Left Outer JOIN Weaver_Pavu_Delivery_Details b ON a.Weaver_Pavu_Delivery_Code = b.Weaver_Pavu_Delivery_Code  " &
                                    " INNER JOIN Ledger_Head v ON v.Ledger_IdNo = a.DeliveryTo_IdNo " &
                                    " LEFT OUTER JOIN EndsCount_Head Eh ON Eh.Endscount_Idno = b.Endscount_Idno " &
                                    " LEFT OUTER JOIN Count_Head c ON c.Count_IdNo = Eh.Count_IdNo " &
                                    " where a.van_trip_code = '" & Trim(NewCode) & "' and upper(replace(a.Vehicle_No,' ','')) = upper(replace('" & Trim(cbo_vehicle.Text) & "',' ','')) and " &
                                    " a.Weaver_Pavu_Delivery_Date = @VanTripDate and a.DeliveryTo_IdNo <> 0 " &
                                    "Group by a.Weaver_Pavu_Delivery_Code, a.Weaver_Pavu_Delivery_Date, a.for_orderby, a.Weaver_Pavu_Delivery_No, v.ledger_name, b.Set_No, Eh.Ends_Name, c.Count_Name, a.Loaded_by_Our_Employee " &
                                    " Order by a.Weaver_Pavu_Delivery_Date, a.for_orderby, a.Weaver_Pavu_Delivery_No "
                Da = New SqlClient.SqlDataAdapter(cmd)
                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then
                    DelNo = ""
                    For i = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()

                        SNo = SNo + 1
                        .Rows(n).Cells(dgvCol_Selection.SlNo).Value = Val(SNo)
                        .Rows(n).Cells(dgvCol_Selection.Dcno).Value = Dt1.Rows(i).Item("Weaver_Pavu_Delivery_No").ToString
                        .Rows(n).Cells(dgvCol_Selection.PartydcNo).Value = ""
                        .Rows(n).Cells(dgvCol_Selection.PartyName).Value = Dt1.Rows(i).Item("ledger_name").ToString ' Common_Procedures.Vendor_IdNoToName(con, Val(Dt1.Rows(i).Item("Vendor_IdNo").ToString)) ' Dt1.Rows(i).Item("Ledger_MainName").ToString
                        .Rows(n).Cells(dgvCol_Selection.Particulars).Value = Dt1.Rows(i).Item("Set_No").ToString & " - " & Dt1.Rows(i).Item("Ends_Name").ToString & "/" & Dt1.Rows(i).Item("Count_Name").ToString
                        .Rows(n).Cells(dgvCol_Selection.Yarn_Bag).Value = ""
                        .Rows(n).Cells(dgvCol_Selection.Pavu_Beams).Value = Val(Dt1.Rows(i).Item("Noof_Beams").ToString)
                        .Rows(n).Cells(dgvCol_Selection.Empty_Beams).Value = ""
                        .Rows(n).Cells(dgvCol_Selection.Quality).Value = ""
                        .Rows(n).Cells(dgvCol_Selection.Pcs).Value = ""
                        .Rows(n).Cells(dgvCol_Selection.Meter).Value = "" ' Dt1.Rows(i).Item("Meters").ToString
                        .Rows(n).Cells(dgvCol_Selection.Weight).Value = ""

                        .Rows(n).Cells(dgvCol_Selection.STS).Value = ""
                        If Trim(NewCode) <> "" Then
                            .Rows(n).Cells(dgvCol_Selection.STS).Value = "1"
                        End If

                        .Rows(n).Cells(dgvCol_Selection.Entry_Type).Value = "PVDLV-DELIVERY"
                        .Rows(n).Cells(dgvCol_Selection.Dc_Receipt_Entry_Code).Value = Dt1.Rows(i).Item("Weaver_Pavu_Delivery_Code").ToString
                        .Rows(n).Cells(dgvCol_Selection.Load_UnLoad_Status).Value = Dt1.Rows(i).Item("Loaded_by_Our_Employee").ToString
                        .Rows(n).Cells(dgvCol_Selection.Cloth_Weight_Per_Meter).Value = 0
                        .Rows(n).Cells(dgvCol_Selection.Yarn_Weight_Per_Bag).Value = 0

                        If Val(.Rows(n).Cells(dgvCol_Selection.STS).Value) = 1 Then
                            For j = 0 To .ColumnCount - 1
                                .Rows(n).Cells(dgvCol_Selection.STS).Style.ForeColor = Color.Red
                            Next
                        End If

                    Next

                End If
                Dt1.Clear()


                '----- EMPTY BEAM RECEIPT(TEXTILE-MODULE)  -  EDITING

                SQL = "select a.* , v.Ledger_Name as vendor_name, bw.Beam_Width_Name from Empty_BeamBagCone_Receipt_Head a " &
                            " INNER JOIN Ledger_Head v ON v.Ledger_IdNo = a.Ledger_IdNo " &
                            " LEFT OUTER JOIN Beam_Width_Head bw ON bw.Beam_Width_IdNo = a.Beam_Width_IdNo  " &
                            " WHERE a.van_trip_code = '" & Trim(NewCode) & "' and upper(replace(a.Vehicle_No,' ','')) = upper(replace('" & Trim(cbo_vehicle.Text) & "',' ','')) and " &
                            " a.Empty_BeamBagCone_Receipt_Date = @VanTripDate and a.Ledger_IdNo <> 0 " &
                            " ORDER BY a.Empty_BeamBagCone_Receipt_Date, a.for_orderby, a.Empty_BeamBagCone_Receipt_No"

                cmd.CommandText = SQL
                Da = New SqlClient.SqlDataAdapter(cmd)
                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then
                    DelNo = ""
                    For i = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()

                        SNo = SNo + 1
                        .Rows(n).Cells(dgvCol_Selection.SlNo).Value = Val(SNo)
                        .Rows(n).Cells(dgvCol_Selection.Dcno).Value = Dt1.Rows(i).Item("Empty_BeamBagCone_Receipt_No").ToString
                        .Rows(n).Cells(dgvCol_Selection.PartydcNo).Value = Dt1.Rows(i).Item("party_dcno").ToString
                        .Rows(n).Cells(dgvCol_Selection.PartyName).Value = Dt1.Rows(i).Item("vendor_name").ToString ' Common_Procedures.Vendor_IdNoToName(con, Dt1.Rows(i).Item("Vendor_Idno").ToString)  '  Dt1.Rows(i).Item("Ledger_MainName").ToString
                        .Rows(n).Cells(dgvCol_Selection.Particulars).Value = Dt1.Rows(i).Item("Beam_Width_Name").ToString
                        .Rows(n).Cells(dgvCol_Selection.Yarn_Bag).Value = ""
                        .Rows(n).Cells(dgvCol_Selection.Pavu_Beams).Value = ""
                        .Rows(n).Cells(dgvCol_Selection.Empty_Beams).Value = Format(Val(Dt1.Rows(i).Item("Empty_Beam").ToString), "#######0")
                        .Rows(n).Cells(dgvCol_Selection.Quality).Value = ""
                        .Rows(n).Cells(dgvCol_Selection.Pcs).Value = ""
                        .Rows(n).Cells(dgvCol_Selection.Meter).Value = ""
                        .Rows(n).Cells(dgvCol_Selection.Weight).Value = ""

                        .Rows(n).Cells(dgvCol_Selection.STS).Value = ""
                        If Trim(NewCode) <> "" Then
                            .Rows(n).Cells(dgvCol_Selection.STS).Value = "1"
                        End If

                        .Rows(n).Cells(dgvCol_Selection.Entry_Type).Value = "EBREC-RECEIPT"
                        .Rows(n).Cells(dgvCol_Selection.Dc_Receipt_Entry_Code).Value = Dt1.Rows(i).Item("Empty_BeamBagCone_Receipt_Code").ToString
                        .Rows(n).Cells(dgvCol_Selection.Load_UnLoad_Status).Value = Dt1.Rows(i).Item("UnLoaded_by_Our_employee").ToString
                        .Rows(n).Cells(dgvCol_Selection.Cloth_Weight_Per_Meter).Value = 0
                        .Rows(n).Cells(dgvCol_Selection.Yarn_Weight_Per_Bag).Value = 0

                        If Val(.Rows(n).Cells(dgvCol_Selection.STS).Value) = 1 Then
                            For j = 0 To .ColumnCount - 1
                                .Rows(n).Cells(dgvCol_Selection.STS).Style.ForeColor = Color.Red
                            Next
                        End If

                    Next

                End If
                Dt1.Clear()


                '----- EMPTY BEAM RECEIPT(SIZING-MODULE)  -  EDITING

                SQL = "select a.* , v.Ledger_Name as vendor_name, bw.Beam_Width_Name from SizSoft_Empty_BeamBagCone_Receipt_Head a " &
                            " INNER JOIN SizSoft_Empty_BeamBagCone_Receipt_Details b ON b.sl_no = 1 and b.Empty_BeamBagCone_Receipt_Code = a.Empty_BeamBagCone_Receipt_Code and b.Empty_Beam <> 0 " &
                            " INNER JOIN Ledger_Head v ON v.Ledger_IdNo = b.Vendor_Idno " &
                            " LEFT OUTER JOIN Beam_Width_Head bw ON bw.Beam_Width_IdNo = a.Beam_Width_IdNo " &
                            " WHERE a.van_trip_code = '" & Trim(NewCode) & "' and upper(replace(a.Vehicle_No,' ','')) = upper(replace('" & Trim(cbo_vehicle.Text) & "',' ','')) and " &
                            " a.Empty_BeamBagCone_Receipt_Date = @VanTripDate and a.Ledger_IdNo <> 0 " &
                            " ORDER BY a.Empty_BeamBagCone_Receipt_Date, a.for_orderby, a.Empty_BeamBagCone_Receipt_No"

                cmd.CommandText = SQL
                Da = New SqlClient.SqlDataAdapter(cmd)
                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then
                    DelNo = ""
                    For i = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()

                        SNo = SNo + 1
                        .Rows(n).Cells(dgvCol_Selection.SlNo).Value = Val(SNo)
                        .Rows(n).Cells(dgvCol_Selection.Dcno).Value = Dt1.Rows(i).Item("Empty_BeamBagCone_Receipt_Code").ToString
                        If Trim(Dt1.Rows(i).Item("Party_DcNo").ToString) <> "" Then
                            .Rows(n).Cells(dgvCol_Selection.PartydcNo).Value = Dt1.Rows(i).Item("Party_DcNo").ToString
                        Else
                            .Rows(n).Cells(dgvCol_Selection.PartydcNo).Value = Dt1.Rows(i).Item("Empty_BeamBagCone_Receipt_No").ToString
                        End If
                        .Rows(n).Cells(dgvCol_Selection.PartyName).Value = Dt1.Rows(i).Item("vendor_name").ToString
                        .Rows(n).Cells(dgvCol_Selection.Particulars).Value = Dt1.Rows(i).Item("Beam_Width_Name").ToString
                        .Rows(n).Cells(dgvCol_Selection.Yarn_Bag).Value = ""
                        .Rows(n).Cells(dgvCol_Selection.Pavu_Beams).Value = ""
                        .Rows(n).Cells(dgvCol_Selection.Empty_Beams).Value = Format(Val(Dt1.Rows(i).Item("Empty_Beam").ToString), "#######0")
                        .Rows(n).Cells(dgvCol_Selection.Quality).Value = ""
                        .Rows(n).Cells(dgvCol_Selection.Pcs).Value = ""
                        .Rows(n).Cells(dgvCol_Selection.Meter).Value = ""
                        .Rows(n).Cells(dgvCol_Selection.Weight).Value = ""
                        .Rows(n).Cells(dgvCol_Selection.STS).Value = ""
                        If Trim(NewCode) <> "" Then
                            .Rows(n).Cells(dgvCol_Selection.STS).Value = "1"
                        End If

                        .Rows(n).Cells(dgvCol_Selection.Entry_Type).Value = "EBREC-RECEIPT-BY-SIZING"
                        .Rows(n).Cells(dgvCol_Selection.Dc_Receipt_Entry_Code).Value = Dt1.Rows(i).Item("Empty_BeamBagCone_Receipt_Code").ToString
                        .Rows(n).Cells(dgvCol_Selection.Load_UnLoad_Status).Value = Dt1.Rows(i).Item("UnLoaded_by_Our_employee").ToString
                        .Rows(n).Cells(dgvCol_Selection.Cloth_Weight_Per_Meter).Value = 0
                        .Rows(n).Cells(dgvCol_Selection.Yarn_Weight_Per_Bag).Value = 0

                        If Val(.Rows(n).Cells(dgvCol_Selection.STS).Value) = 1 Then
                            For j = 0 To .ColumnCount - 1
                                .Rows(n).Cells(dgvCol_Selection.STS).Style.ForeColor = Color.Red
                            Next
                        End If

                    Next

                End If
                Dt1.Clear()

                '----- CLOTH RECEIPT  

                cmd.CommandText = "Select a.* , L.Ledger_Name, cl.Cloth_Name, cl.Weight_Meter_Warp, cl.Weight_Meter_Weft " &
                                    " FROM  Weaver_Cloth_Receipt_Head a  " &
                                    " LEFT OUTER JOIN Ledger_Head L ON L.Ledger_IdNo = a.Ledger_Idno     " &
                                    " LEFT OUTER JOIN Cloth_Head cl on a.cloth_idno = cl.cloth_idno   " &
                                    " WHERE a.van_trip_code_Textile = '" & Trim(NewCode) & "'  and upper(replace(a.Vehicle_No,' ','')) = upper(replace('" & Trim(cbo_vehicle.Text) & "',' ','')) and " &
                                    " a.Weaver_ClothReceipt_Date = @VanTripDate  " &
                                    " ORDER BY a.Weaver_ClothReceipt_Date, a.for_orderby, a.Weaver_ClothReceipt_No "
                Da = New SqlClient.SqlDataAdapter(cmd)
                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then
                    DelNo = ""
                    For i = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()

                        SNo = SNo + 1

                        .Rows(n).Cells(dgvCol_Selection.SlNo).Value = Val(SNo)
                        .Rows(n).Cells(dgvCol_Selection.Dcno).Value = Dt1.Rows(i).Item("Weaver_ClothReceipt_No").ToString
                        .Rows(n).Cells(dgvCol_Selection.PartydcNo).Value = Dt1.Rows(i).Item("party_dcno").ToString
                        .Rows(n).Cells(dgvCol_Selection.PartyName).Value = Dt1.Rows(i).Item("Ledger_Name").ToString
                        .Rows(n).Cells(dgvCol_Selection.Particulars).Value = Dt1.Rows(i).Item("Cloth_Name").ToString
                        .Rows(n).Cells(dgvCol_Selection.Yarn_Bag).Value = ""
                        .Rows(n).Cells(dgvCol_Selection.Pavu_Beams).Value = ""
                        .Rows(n).Cells(dgvCol_Selection.Empty_Beams).Value = ""
                        .Rows(n).Cells(dgvCol_Selection.Quality).Value = Dt1.Rows(i).Item("Cloth_Name").ToString
                        .Rows(n).Cells(dgvCol_Selection.Pcs).Value = Val(Dt1.Rows(i).Item("noof_pcs").ToString)
                        .Rows(n).Cells(dgvCol_Selection.Meter).Value = Dt1.Rows(i).Item("ReceiptMeters_Receipt").ToString

                        .Rows(n).Cells(dgvCol_Selection.Cloth_Weight_Per_Meter).Value = Val(Val(Dt1.Rows(i).Item("Weight_Meter_Warp").ToString) + Val(Dt1.Rows(i).Item("Weight_Meter_Weft").ToString))

                        .Rows(n).Cells(dgvCol_Selection.Weight).Value = Format(Val(Dt1.Rows(i).Item("ReceiptMeters_Receipt").ToString) * Val(.Rows(n).Cells(dgvCol_Selection.Cloth_Weight_Per_Meter).Value), "#######0.000")

                        .Rows(n).Cells(dgvCol_Selection.STS).Value = ""
                        If Trim(NewCode) <> "" Then
                            .Rows(n).Cells(dgvCol_Selection.STS).Value = "1"
                        End If

                        .Rows(n).Cells(dgvCol_Selection.Entry_Type).Value = "FBREC-RECEIPT"
                        .Rows(n).Cells(dgvCol_Selection.Dc_Receipt_Entry_Code).Value = Dt1.Rows(i).Item("Weaver_ClothReceipt_Code").ToString
                        .Rows(n).Cells(dgvCol_Selection.Load_UnLoad_Status).Value = Dt1.Rows(i).Item("UnLoaded_by_Our_employee").ToString
                        .Rows(n).Cells(dgvCol_Selection.Yarn_Weight_Per_Bag).Value = 0

                        If Val(.Rows(n).Cells(dgvCol_Selection.STS).Value) = 1 Then
                            For j = 0 To .ColumnCount - 1
                                .Rows(n).Cells(dgvCol_Selection.STS).Style.ForeColor = Color.Red
                            Next
                        End If

                    Next

                End If

                Dt1.Clear()

            Next loopcount

        End With

        pnl_Selection.Visible = True
        pnl_back.Enabled = False
        dgv_Selection.Focus()

    End Sub

    Private Sub btn_Close_Selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Close_Selection.Click
        Close_Delivery_Selection()
    End Sub

    Private Sub dgv_Selection_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Selection.CellClick
        Select_Delivery(e.RowIndex)
    End Sub

    Private Sub dgv_Selection_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Selection.KeyDown
        Dim n As Integer

        On Error Resume Next

        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Space Then
            If dgv_Selection.CurrentCell.RowIndex >= 0 Then

                n = dgv_Selection.CurrentCell.RowIndex

                Select_Delivery(n)

                e.Handled = True

            End If
        End If

    End Sub

    Private Sub Select_Delivery(ByVal RwIndx As Integer)

        Dim i As Integer

        With dgv_Selection

            If .RowCount > 0 And RwIndx >= 0 Then

                .Rows(RwIndx).Cells(dgvCol_Selection.STS).Value = (Val(.Rows(RwIndx).Cells(dgvCol_Selection.STS).Value) + 1) Mod 2

                If Val(.Rows(RwIndx).Cells(dgvCol_Selection.STS).Value) = 1 Then

                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
                    Next


                Else
                    .Rows(RwIndx).Cells(dgvCol_Selection.STS).Value = ""

                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Black
                    Next

                End If

            End If

        End With
    End Sub

    Private Sub Close_Delivery_Selection()
        If FrmLdSTS = True Then Exit Sub

        Dim n As Integer = 0
        Dim sno As Integer = 0
        Dim i As Integer = 0
        Dim j As Integer = 0

        Dim ClothKgs As String
        Dim YrnLoadRate As String, YrnUnLoadRate As String
        Dim ClothLoadRate As String, ClothUnLoadRate As String
        Dim PavuLoadRate As String, PavuUnLoadRate As String
        Dim EbeamLoadRate As String, EbeamUnLoadRate As String

        'Dim LoadRate As Single, UnLoadRate As Single
        'Dim Total_LoadRate As Single, Total_UnLoadRate As Single

        Dim SQL As String = ""
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As DataTable = New DataTable
        Dim vAmt As String = 0
        Dim vVechi_IdNo As Integer = 0
        Dim vLoadRate As String = 0, vUnLoadRate As String = 0


        dgv_DeliveryDetails.Rows.Clear()
        dgv_ReceiptDetails.Rows.Clear()

        vVechi_IdNo = Val(Common_Procedures.Vehicle_NameToIdNo(con, cbo_vehicle.Text))


        ClothKgs = 0
        YrnLoadRate = 0
        YrnUnLoadRate = 0
        ClothLoadRate = 0
        ClothUnLoadRate = 0
        PavuLoadRate = 0
        PavuUnLoadRate = 0
        EbeamLoadRate = 0
        EbeamUnLoadRate = 0

        SQL = "Select * from Loading_unloading_Rate_Head where vehicle_idno = " & Str(Val(vVechi_IdNo))
        da = New SqlClient.SqlDataAdapter(Sql, con)
        dt = New DataTable
        da.Fill(dt)

        If dt.Rows.Count > 0 Then

            ClothKgs = Val(dt.Rows(0).Item("Cloth_Kgs").ToString)
            ClothLoadRate = Val(dt.Rows(0).Item("Cloth_Loading_Rate").ToString)
            ClothUnLoadRate = Val(dt.Rows(0).Item("Cloth_UnLoading_Rate").ToString)
            PavuLoadRate = Val(dt.Rows(0).Item("Pavu_Beam_Loading_rate").ToString)
            PavuUnLoadRate = Val(dt.Rows(0).Item("Pavu_Beam_unloading_Rate").ToString)
            EbeamLoadRate = Val(dt.Rows(0).Item("Empty_Beam_Loading_Rate").ToString)
            EbeamUnLoadRate = Val(dt.Rows(0).Item("Empty_Beam_Unloading_Rate").ToString)

        End If
        dt.Clear()

        'If Val(YrnKgs) = 0 Then YrnKgs = 1
        If Val(ClothKgs) = 0 Then ClothKgs = 1

        For i = 0 To dgv_Selection.RowCount - 1



            SQL = "Select * from Loading_unloading_Details  where " & Val(dgv_Selection.Rows(i).Cells(dgvCol_Selection.Yarn_Weight_Per_Bag).Value) & " BETWEEN From_weight AND To_Weight  and vehicle_idno = " & Str(Val(vVechi_IdNo))
            da = New SqlClient.SqlDataAdapter(SQL, con)
            dt = New DataTable
            da.Fill(dt)

            If dt.Rows.Count > 0 Then

                YrnLoadRate = Val(dt.Rows(0).Item("Loading_charges").ToString)
                YrnUnLoadRate = Val(dt.Rows(0).Item("UnLoading_charges").ToString)

            End If
            dt.Clear()


            If Val(dgv_Selection.Rows(i).Cells(dgvCol_Selection.STS).Value) = 1 Then

                If Trim(UCase(dgv_Selection.Rows(i).Cells(dgvCol_Selection.Entry_type).Value.ToString)) = "YNDLV-DELIVERY" Or Trim(UCase(dgv_Selection.Rows(i).Cells(dgvCol_Selection.Entry_type).Value.ToString)) = "PVDLV-DELIVERY" Then

                    n = dgv_DeliveryDetails.Rows.Add()
                    sno = sno + 1

                    dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.SlNo).Value = Val(sno)
                    dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Dcno).Value = dgv_Selection.Rows(i).Cells(dgvCol_Selection.Dcno).Value
                    dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.PartyName).Value = dgv_Selection.Rows(i).Cells(dgvCol_Selection.PartyName).Value
                    dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Particulars).Value = dgv_Selection.Rows(i).Cells(dgvCol_Selection.Particulars).Value

                    dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Yarn_Bag).Value = dgv_Selection.Rows(i).Cells(dgvCol_Selection.Yarn_Bag).Value
                    If Val(dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Yarn_Bag).Value) = 0 Then dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Yarn_Bag).Value = ""

                    dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Pavu_Beams).Value = dgv_Selection.Rows(i).Cells(dgvCol_Selection.Pavu_Beams).Value
                    If Val(dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Pavu_Beams).Value) = 0 Then dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Pavu_Beams).Value = ""

                    dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Empty_Beams).Value = dgv_Selection.Rows(i).Cells(dgvCol_Selection.Empty_Beams).Value
                    If Val(dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Empty_Beams).Value) = 0 Then dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Empty_Beams).Value = ""

                    dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Quality).Value = dgv_Selection.Rows(i).Cells(dgvCol_Selection.Quality).Value

                    dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Pcs).Value = dgv_Selection.Rows(i).Cells(dgvCol_Selection.Pcs).Value
                    If Val(dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Pcs).Value) = 0 Then dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Pcs).Value = ""

                    dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Meter).Value = dgv_Selection.Rows(i).Cells(dgvCol_Selection.Meter).Value
                    If Val(dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Meter).Value) = 0 Then dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Meter).Value = ""

                    dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Weight).Value = dgv_Selection.Rows(i).Cells(dgvCol_Selection.Weight).Value

                    dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Entry_type).Value = dgv_Selection.Rows(i).Cells(dgvCol_Selection.Entry_type).Value
                    dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Dc_Entry_Code).Value = dgv_Selection.Rows(i).Cells(dgvCol_Selection.Dc_Receipt_Entry_Code).Value
                    dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Load_UnLoad_Status).Value = dgv_Selection.Rows(i).Cells(dgvCol_Selection.Load_UnLoad_Status).Value

                    dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Yarn_Bag_Rate_for_Kgs).Value = 0
                    dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Yarn_Loading_Rate).Value = Val(YrnLoadRate)
                    dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Yarn_Unloading_Rate).Value = Val(YrnUnLoadRate)
                    dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Pavu_Beam_Loading_Rate).Value = Val(PavuLoadRate)
                    dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Pavu_Beam_unloading_Rate).Value = Val(PavuUnLoadRate)
                    dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Empty_Beam_Loading_Rate).Value = Val(EbeamLoadRate)
                    dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Empty_Beam_Unloading_Rate).Value = Val(EbeamUnLoadRate)
                    dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Cloth_Rate_for_Kgs).Value = Val(ClothKgs)
                    dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Cloth_Loading_Rate).Value = Val(ClothLoadRate)
                    dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Cloth_UnLoading_Rate).Value = Val(ClothUnLoadRate)

                    dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Yarn_Weight_Per_Bag).Value = dgv_Selection.Rows(i).Cells(dgvCol_Selection.Yarn_Weight_Per_Bag).Value

                    vLoadRate = 0

                    If Trim(UCase(dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Entry_type).Value)) = Trim(UCase("YNDLV-DELIVERY")) Then

                        If Val(dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Load_UnLoad_Status).Value) <> 0 Then
                            vLoadRate = 0
                        Else
                            vLoadRate = Val(dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Yarn_Loading_Rate).Value)
                        End If

                    Else
                        vLoadRate = Val(dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Yarn_Loading_Rate).Value)

                    End If

                    dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Yarn_Loading_Rate).Value = Val(vLoadRate)

                    vAmt = 0
                    'If Val(dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Yarn_Bag_Rate_for_Kgs).Value) <> 0 Then
                    vLoadRate = Val(dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Yarn_Loading_Rate).Value)
                    'vLoadRate = Format(Val(dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Yarn_Loading_Rate).Value) / Val(dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Yarn_Bag_Rate_for_Kgs).Value) * Val(dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Yarn_Weight_Per_Bag).Value), "##########0.00")
                    vAmt = Val(dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Yarn_Bag).Value) * Val(vLoadRate)
                    'End If
                    dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Yarn_Loading_Amount).Value = Format(Val(vAmt), "##########0.00")

                    vAmt = 0
                    'If Val(dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Yarn_Bag_Rate_for_Kgs).Value) <> 0 Then
                    vUnLoadRate = Val(dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Yarn_Unloading_Rate).Value)
                    'vUnLoadRate = Format(Val(dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Yarn_Unloading_Rate).Value) / Val(dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Yarn_Bag_Rate_for_Kgs).Value) * Val(dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Yarn_Weight_Per_Bag).Value), "##########0.00")
                    vAmt = Val(dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Yarn_Bag).Value) * Val(vUnLoadRate)
                    'End If
                    dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Yarn_Unloading_Amount).Value = Format(Val(vAmt), "##########0.00")


                    vLoadRate = 0

                    If Trim(UCase(dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Entry_type).Value)) = Trim(UCase("PVDLV-DELIVERY")) Then

                        If Val(dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Load_UnLoad_Status).Value) <> 0 Then
                            vLoadRate = 0
                        Else
                            vLoadRate = Val(dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Pavu_Beam_Loading_Rate).Value)
                        End If

                    Else
                        vLoadRate = Val(dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Pavu_Beam_Loading_Rate).Value)

                    End If

                    dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Pavu_Beam_Loading_Rate).Value = Val(vLoadRate)

                    vAmt = Val(dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Pavu_Beams).Value) * Val(dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Pavu_Beam_Loading_Rate).Value)
                    dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Pavu_Beam_Loading_Amount).Value = Format(Val(vAmt), "##########0.00")

                    vAmt = Val(dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Pavu_Beams).Value) * Val(dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Pavu_Beam_unloading_Rate).Value)
                    dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Pavu_Beam_unloading_Amount).Value = Format(Val(vAmt), "##########0.00")


                    vAmt = Val(dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Empty_Beams).Value) * Val(dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Empty_Beam_Loading_Rate).Value)
                    dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Empty_Beam_Loading_Amount).Value = Format(Val(vAmt), "##########0.00")

                    vAmt = Val(dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Empty_Beams).Value) * Val(dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Empty_Beam_Unloading_Rate).Value)
                    dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Empty_Beam_Unloading_Amount).Value = Format(Val(vAmt), "##########0.00")


                    dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Cloth_Loading_Amount).Value = 0
                    dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Cloth_UnLoading_Amount).Value = 0
                    dgv_DeliveryDetails.Rows(n).Cells(dgvCol_DelvDetails.Cloth_Weight_Per_Meter).Value = 0





                Else   '----------------------------------------------------------------------------------------------------------

                    n = dgv_ReceiptDetails.Rows.Add()
                    sno = sno + 1

                    dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.SlNo).Value = Val(sno)
                    dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Recno).Value = dgv_Selection.Rows(i).Cells(dgvCol_Selection.Dcno).Value
                    dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.P_Dcno).Value = dgv_Selection.Rows(i).Cells(dgvCol_Selection.PartydcNo).Value
                    dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.PartyName).Value = dgv_Selection.Rows(i).Cells(dgvCol_Selection.PartyName).Value
                    dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Particulars).Value = dgv_Selection.Rows(i).Cells(dgvCol_Selection.Particulars).Value

                    dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Yarn_Bag).Value = dgv_Selection.Rows(i).Cells(dgvCol_Selection.Yarn_Bag).Value
                    If Val(dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Yarn_Bag).Value) = 0 Then dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Yarn_Bag).Value = ""

                    dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Pavu_Beams).Value = dgv_Selection.Rows(i).Cells(dgvCol_Selection.Pavu_Beams).Value
                    If Val(dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Pavu_Beams).Value) = 0 Then dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Pavu_Beams).Value = ""

                    dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Empty_Beams).Value = dgv_Selection.Rows(i).Cells(dgvCol_Selection.Empty_Beams).Value
                    If Val(dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Empty_Beams).Value) = 0 Then dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Empty_Beams).Value = ""

                    dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Quality).Value = dgv_Selection.Rows(i).Cells(dgvCol_Selection.Quality).Value

                    dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Pcs).Value = Val(dgv_Selection.Rows(i).Cells(dgvCol_Selection.Pcs).Value)
                    If Val(dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Pcs).Value) = 0 Then dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Pcs).Value = ""

                    dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Meter).Value = dgv_Selection.Rows(i).Cells(dgvCol_Selection.Meter).Value
                    If Val(dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Meter).Value) = 0 Then dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Meter).Value = ""

                    dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Weight).Value = dgv_Selection.Rows(i).Cells(dgvCol_Selection.Weight).Value
                    If Val(dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Weight).Value) = 0 Then dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Weight).Value = ""

                    dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Entry_type).Value = dgv_Selection.Rows(i).Cells(dgvCol_Selection.Entry_Type).Value
                    dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Receipt_Entry_Code).Value = dgv_Selection.Rows(i).Cells(dgvCol_Selection.Dc_Receipt_Entry_Code).Value
                    dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Load_UnLoad_Status).Value = dgv_Selection.Rows(i).Cells(dgvCol_Selection.Load_UnLoad_Status).Value


                    dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Yarn_Bag_Rate_for_Kgs).Value = 0
                    dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Yarn_Loading_Rate).Value = Val(YrnLoadRate)
                    dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Yarn_Unloading_Rate).Value = Val(YrnUnLoadRate)
                    dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Pavu_Beam_Loading_Rate).Value = Val(PavuLoadRate)
                    dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Pavu_Beam_unloading_Rate).Value = Val(PavuUnLoadRate)
                    dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Empty_Beam_Loading_Rate).Value = Val(EbeamLoadRate)
                    dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Empty_Beam_Unloading_Rate).Value = Val(EbeamUnLoadRate)
                    dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Cloth_Rate_for_Kgs).Value = Val(ClothKgs)
                    dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Cloth_Loading_Rate).Value = Val(ClothLoadRate)
                    dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Cloth_UnLoading_Rate).Value = Val(ClothUnLoadRate)

                    dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Yarn_Weight_Per_Bag).Value = dgv_Selection.Rows(i).Cells(dgvCol_Selection.Yarn_Weight_Per_Bag).Value

                    vAmt = 0
                    If Val(dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Yarn_Bag_Rate_for_Kgs).Value) <> 0 Then
                        vLoadRate = Format(Val(dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Yarn_Loading_Rate).Value) / Val(dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Yarn_Bag_Rate_for_Kgs).Value) * Val(dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Yarn_Weight_Per_Bag).Value), "##########0.00")
                        vAmt = Val(dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Yarn_Bag).Value) * Val(vLoadRate)
                    End If
                    dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Yarn_Loading_Amount).Value = Format(Val(vAmt), "##########0.00")

                    vAmt = 0
                    If Val(dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Yarn_Bag_Rate_for_Kgs).Value) <> 0 Then
                        vUnLoadRate = Format(Val(dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Yarn_Unloading_Rate).Value) / Val(dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Yarn_Bag_Rate_for_Kgs).Value) * Val(dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Yarn_Weight_Per_Bag).Value), "##########0.00")
                        vAmt = Val(dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Yarn_Bag).Value) * Val(vUnLoadRate)
                    End If
                    dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Yarn_Unloading_Amount).Value = Format(Val(vAmt), "##########0.00")

                    vAmt = Val(dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Pavu_Beams).Value) * Val(dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Pavu_Beam_Loading_Rate).Value)
                    dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Pavu_Beam_Loading_Amount).Value = Format(Val(vAmt), "##########0.00")

                    vAmt = Val(dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Pavu_Beams).Value) * Val(dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Pavu_Beam_unloading_Rate).Value)
                    dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Pavu_Beam_unloading_Amount).Value = Format(Val(vAmt), "##########0.00")

                    vAmt = Val(dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Empty_Beams).Value) * Val(dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Empty_Beam_Loading_Rate).Value)
                    dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Empty_Beam_Loading_Amount).Value = Format(Val(vAmt), "##########0.00")

                    vUnLoadRate = 0
                    If Trim(UCase(dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Entry_type).Value)) = Trim(UCase("EBREC-RECEIPT")) Or Trim(UCase(dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Entry_type).Value)) = Trim(UCase("EBREC-RECEIPT-BY-SIZING")) Then

                        If Val(dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Load_UnLoad_Status).Value) <> 0 Then
                            vUnLoadRate = 0
                        Else
                            vUnLoadRate = Val(dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Empty_Beam_Unloading_Rate).Value)
                        End If

                    Else
                        vUnLoadRate = Val(dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Empty_Beam_Unloading_Rate).Value)

                    End If
                    dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Empty_Beam_Unloading_Rate).Value = Val(vUnLoadRate)

                    vAmt = Val(dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Empty_Beams).Value) * Val(dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Empty_Beam_Unloading_Rate).Value)
                    dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Empty_Beam_Unloading_Amount).Value = Format(Val(vAmt), "##########0.00")


                    vAmt = 0
                    If Val(dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Cloth_Rate_for_Kgs).Value) <> 0 Then
                        vAmt = Val(dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Weight).Value) * (Val(dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Cloth_Loading_Rate).Value) / Val(dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Cloth_Rate_for_Kgs).Value))
                    End If
                    dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Cloth_Loading_Amount).Value = Format(Val(vAmt), "##########0.00")

                    vUnLoadRate = 0
                    If Trim(UCase(dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Entry_type).Value)) = Trim(UCase("FBREC-RECEIPT")) Then

                        If Val(dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Load_UnLoad_Status).Value) <> 0 Then
                            vUnLoadRate = 0
                        Else
                            vUnLoadRate = Val(dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Cloth_UnLoading_Rate).Value)
                        End If

                    Else
                        vUnLoadRate = Val(dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Cloth_UnLoading_Rate).Value)

                    End If
                    dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Cloth_UnLoading_Rate).Value = Val(vUnLoadRate)

                    vAmt = 0
                    If Val(dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Cloth_Rate_for_Kgs).Value) <> 0 Then
                        vAmt = Val(dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Weight).Value) * (Val(dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Cloth_UnLoading_Rate).Value) / Val(dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Cloth_Rate_for_Kgs).Value))
                    End If
                    dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Cloth_UnLoading_Amount).Value = Format(Val(vAmt), "##########0.00")

                    dgv_ReceiptDetails.Rows(n).Cells(dgvCol_RcptDetails.Cloth_Weight_Per_Meter).Value = dgv_Selection.Rows(i).Cells(dgvCol_Selection.Cloth_Weight_Per_Meter).Value



                End If

            End If

        Next

        TotalVan_Calculation()

        pnl_back.Enabled = True
        pnl_Selection.Visible = False
        Grid_Cell_DeSelect()

        cbo_Transport.Focus()

    End Sub

    Private Sub txt_Freight_Charges_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Freight_Charges.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()

            Else
                msk_date.Focus()
            End If

        End If


        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If

    End Sub

    Private Sub txt_Freight_Charges_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Freight_Charges.KeyDown
        If e.KeyValue = 40 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()

            Else
                msk_date.Focus()
            End If

        End If

        If (e.KeyValue = 38) Then

            cbo_address.Focus()

        End If
    End Sub


    
    Private Sub dtp_start_time_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_start_time.KeyPress
        If Asc(e.KeyChar) = 13 Then
            dtp_End_time.Focus()
        End If
    End Sub

    Private Sub dtp_End_time_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_End_time.KeyDown
        If (e.KeyValue = 38) Then
            dtp_start_time.Focus()
        ElseIf (e.KeyValue = 40) Then
            cbo_address.Focus()
        End If
    End Sub

    Private Sub dtp_End_time_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_End_time.KeyPress
        If Asc(e.KeyChar) = 13 Then
            cbo_address.Focus()
        End If
    End Sub

    Private Sub cbo_vehicle_KeyUp(sender As Object, e As KeyEventArgs) Handles cbo_vehicle.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Common_Procedures.MDI_LedType = ""
            Dim f As New VehicleNo_Creation


            f.MdiParent = MDIParent1
            f.Show()
        End If

    End Sub
    Private Sub cbo_drivername_GotFocus(sender As Object, e As EventArgs) Handles cbo_drivername.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Van_Trip_Head", "Driver_Name", "", "Driver_Name")
    End Sub
End Class
