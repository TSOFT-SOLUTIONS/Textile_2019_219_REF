Public Class Bobin_Purchase_Order_Entry

    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "BPORD-"
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
    Private prn_DetSNo As Integer

    Private vPrn_PvuEdsCnt As String
    Private vPrn_PvuTotBms As Integer
    Private vPrn_PvuTotMtrs As Single
    Private vPrn_PvuSetNo As String
    Private vPrn_PvuBmNos1 As String
    Private vPrn_PvuBmNos2 As String
    Private vPrn_PvuBmNos3 As String
    Private vPrn_PvuBmNos4 As String
    Private WithEvents dgtxt_details As New DataGridViewTextBoxEditingControl
    Public vmskOldText As String = ""
    Public vmskSelStrt As Integer = -1
    Private Prn_Sts As Integer = 0


    Public Sub New()
        FrmLdSTS = True
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Private Enum DgvCol_BobinDetails
        SNo '0
        Ends '1
        MillName '2
        Colour '3
        NoOfBobins '4
        BobinSize '5
        MeterBobin '6
        Meters '7
        MeterReel '8
        NofReel '9
        Rate '10
        Amount '11
    End Enum

    Private Sub clear()
        New_Entry = False
        Insert_Entry = False

        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        vmskOldText = ""
        vmskSelStrt = -1


        lbl_OrderNo.Text = ""
        lbl_OrderNo.ForeColor = Color.Black
        msk_Date.Text = ""
        dtp_Date.Text = ""

        cbo_Ledger.Text = ""
        cbo_TransportName.Text = ""
        txt_Freight.Text = ""
        txt_Remarks.Text = ""
        txt_ExcersiceTerms.Text = ""

        txt_PaymentTerms.Text = ""
        cbo_Agent.Text = ""
        cbo_RateFor.Text = ""


        txt_DeliveryTerms.Text = ""
        cbo_Colour.Text = ""
        cbo_Colour.Tag = -1
        dgv_Details.Rows.Clear()


        Grid_DeSelect()


        cbo_Ends.Visible = False
        cbo_Colour.Visible = False
        cbo_MillName.Visible = False
        cbo_BobinSize.Visible = False
        cbo_Ends.Tag = -1
        cbo_Ends.Text = ""
        cbo_MillName.Tag = -1
        cbo_MillName.Text = ""
        dgv_Details.Rows.Clear()
        dgv_Details_Total.Rows.Clear()
        dgv_Details_Total.Rows.Add()

    End Sub

    Private Sub Grid_DeSelect()
        On Error Resume Next
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
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
        ElseIf TypeOf Me.ActiveControl Is ComboBox Then
            combobx = Me.ActiveControl
            combobx.SelectAll()
        ElseIf TypeOf Me.ActiveControl Is MaskedTextBox Then
            msktxbx = Me.ActiveControl
            msktxbx.SelectionStart = 0
        End If

        If Me.ActiveControl.Name <> cbo_Ends.Name Then
            cbo_Ends.Visible = False
        End If

        If Me.ActiveControl.Name <> cbo_Colour.Name Then
            cbo_Colour.Visible = False
        End If

        If Me.ActiveControl.Name <> cbo_BobinSize.Name Then
            cbo_BobinSize.Visible = False
        End If

        If Me.ActiveControl.Name <> cbo_MillName.Name Then
            cbo_MillName.Visible = False
        End If

        If Me.ActiveControl.Name <> dgv_Details_Total.Name Then
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

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try


            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name , c.Ledger_Name as Transport_Name, d.Ledger_Name as Agent_Name from Bobin_PO_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Ledger_Head c ON a.Transport_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Ledger_Head d ON a.Agent_IdNo = d.Ledger_IdNo Where a.Bobin_PO_Code = '" & Trim(NewCode) & "'", con)
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then
                lbl_OrderNo.Text = dt1.Rows(0).Item("Bobin_PO_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Bobin_PO_Date").ToString
                msk_Date.Text = dtp_Date.Text
                cbo_Ledger.Text = dt1.Rows(0).Item("Ledger_Name").ToString
                cbo_Agent.Text = dt1.Rows(0).Item("Agent_Name").ToString
                cbo_TransportName.Text = dt1.Rows(0).Item("Transport_Name").ToString
                cbo_RateFor.Text = dt1.Rows(0).Item("Rate_For").ToString

                txt_PaymentTerms.Text = dt1.Rows(0).Item("Payment_Terms").ToString
                txt_ExcersiceTerms.Text = dt1.Rows(0).Item("Exercise_Terms").ToString
                txt_DeliveryTerms.Text = dt1.Rows(0).Item("Delivery_Terms").ToString

                txt_Freight.Text = Format(Val(dt1.Rows(0).Item("Freight_Charge").ToString), "########0.00")
                txt_Remarks.Text = dt1.Rows(0).Item("Narration").ToString

                lbl_NetAmount.Text = dt1.Rows(0).Item("Net_Amount").ToString

                da2 = New SqlClient.SqlDataAdapter("select a.*, B.Colour_Name  from Bobin_PO_Details a LEFT OUTER JOIN Colour_Head b ON a.Colour_IdNo = b.Colour_IdNo where a.Bobin_PO_Code = '" & Trim(NewCode) & "' Order by Sl_No", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_Details.Rows.Clear()
                SNo = 0

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_Details.Rows.Add()

                        SNo = SNo + 1
                        dgv_Details.Rows(n).Cells(DgvCol_BobinDetails.SNo).Value = Val(SNo)
                        dgv_Details.Rows(n).Cells(DgvCol_BobinDetails.Ends).Value = Common_Procedures.EndsCount_IdNoToName(con, dt2.Rows(i).Item("EndsCount_Idno").ToString)
                        dgv_Details.Rows(n).Cells(DgvCol_BobinDetails.MillName).Value = Common_Procedures.Mill_IdNoToName(con, dt2.Rows(i).Item("Mill_Idno").ToString)
                        dgv_Details.Rows(n).Cells(DgvCol_BobinDetails.Colour).Value = dt2.Rows(i).Item("Colour_Name").ToString
                        dgv_Details.Rows(n).Cells(DgvCol_BobinDetails.NoOfBobins).Value = dt2.Rows(i).Item("Bobins").ToString
                        dgv_Details.Rows(n).Cells(DgvCol_BobinDetails.BobinSize).Value = Common_Procedures.BobinSize_IdNoToName(con, dt2.Rows(i).Item("Bobin_Size_IdNo").ToString)
                        dgv_Details.Rows(n).Cells(DgvCol_BobinDetails.MeterBobin).Value = dt2.Rows(i).Item("Meter_Bobin").ToString
                        dgv_Details.Rows(n).Cells(DgvCol_BobinDetails.Meters).Value = Format(Val(dt2.Rows(i).Item("Meters").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(DgvCol_BobinDetails.MeterReel).Value = Format(Val(dt2.Rows(i).Item("Meter_Reel").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(DgvCol_BobinDetails.NofReel).Value = Val(dt2.Rows(i).Item("Reel").ToString)
                        dgv_Details.Rows(n).Cells(DgvCol_BobinDetails.Rate).Value = Format(Val(dt2.Rows(i).Item("Rate").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(DgvCol_BobinDetails.Amount).Value = Format(Val(dt2.Rows(i).Item("Amount").ToString), "########0.000")

                    Next i

                End If

                If dgv_Details.RowCount = 0 Then dgv_Details.Rows.Add()
                With dgv_Details_Total
                    If .RowCount = 0 Then .Rows.Add()
                    .Rows(0).Cells(4).Value = dt1.Rows(0).Item("Total_Bobin").ToString
                    .Rows(0).Cells(7).Value = Format(Val(dt1.Rows(0).Item("Total_Meters").ToString), "########0.00")
                    .Rows(0).Cells(9).Value = dt1.Rows(0).Item("Total_Reel").ToString
                    .Rows(0).Cells(11).Value = Format(Val(dt1.Rows(0).Item("Total_Amount").ToString), "########0.000")
                End With

                Grid_DeSelect()

                dt2.Clear()
                dt2.Dispose()
                da2.Dispose()

            End If

            dt1.Clear()
            dt1.Dispose()
            da1.Dispose()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If msk_Date.Visible And msk_Date.Enabled Then msk_Date.Focus()

    End Sub

    Private Sub Bobin_Purchase_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Dim dt1 As New DataTable

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_MillName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "MILL" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Ledger.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Ledger.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_MillName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Agent.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "AGENT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Agent.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_TransportName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "TRANSPORT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_TransportName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Ends.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "ENDSCOUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Ends.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Colour.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "COLOUR" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Colour.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_BobinSize.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "BOBINSIZE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_BobinSize.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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
            MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        FrmLdSTS = False

    End Sub

    Private Sub Bobin_Purchase_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim dt5 As New DataTable
        Dim dt6 As New DataTable
        Dim dt7 As New DataTable
        Dim dt8 As New DataTable
        Dim dt9 As New DataTable
        Dim dt10 As New DataTable
        Dim dt11 As New DataTable

        Me.Text = ""

        con.Open()



        da = New SqlClient.SqlDataAdapter("select EndsCount_Name from EndsCount_Head where Cotton_Polyester_Jari <>'COTTON' order by EndsCount_Name", con)
        da.Fill(dt2)
        cbo_Ends.DataSource = dt2
        cbo_Ends.DisplayMember = "EndsCount_Name"

        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead Where (ledger_type = 'AGENT' or Ledger_IdNo = 0) order by Ledger_DisplayName", con)
        da.Fill(dt4)
        cbo_Agent.DataSource = dt4
        cbo_Agent.DisplayMember = "Ledger_DisplayName"

        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead Where (ledger_type = 'TRANSPORT' or Ledger_IdNo = 0) order by Ledger_DisplayName", con)
        da.Fill(dt5)
        cbo_TransportName.DataSource = dt5
        cbo_TransportName.DisplayMember = "Ledger_DisplayName"

        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where ( (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) )  or Show_In_All_Entry = 1 ) order by Ledger_DisplayName", con)
        da.Fill(dt6)
        cbo_Ledger.DataSource = dt6
        cbo_Ledger.DisplayMember = "Ledger_DisplayName"

        da = New SqlClient.SqlDataAdapter("select Colour_Name from Colour_Head order by Colour_Name", con)
        da.Fill(dt10)
        cbo_Colour.DataSource = dt10
        cbo_Colour.DisplayMember = "Colour_Name"

        da = New SqlClient.SqlDataAdapter("select Bobin_Size_Name from Bobin_Size_Head order by Bobin_Size_Name", con)
        da.Fill(dt11)
        cbo_BobinSize.DataSource = dt11
        cbo_BobinSize.DisplayMember = "Bobin_Size_Name"

        cbo_RateFor.Items.Clear()
        cbo_RateFor.Items.Add("")
        cbo_RateFor.Items.Add("METERS")
        cbo_RateFor.Items.Add("REEL")

        cbo_Ends.Visible = False

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2

        btn_UserModification.Visible = False
        If Common_Procedures.settings.User_Modifications_Show_Status = 1 Then
            If Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_UserModification_Status = 1 Then
                btn_UserModification.Visible = True
            End If
        End If

        AddHandler msk_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Ends.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_MillName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Colour.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_BobinSize.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Ledger.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Agent.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_TransportName.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_filter_billNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DeliveryTerms.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_ExcersiceTerms.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Freight.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Remarks.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PaymentTerms.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_RateFor.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_DelvAt.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus

        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Ends.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Colour.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_BobinSize.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_MillName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Ledger.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Agent.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_TransportName.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_filter_billNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_ExcersiceTerms.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Freight.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Remarks.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PaymentTerms.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_RateFor.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DeliveryTerms.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_Filter_DelvAt.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus

        AddHandler dtp_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_ExcersiceTerms.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler msk_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_PaymentTerms.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Freight.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_filter_billNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_DeliveryTerms.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler msk_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_ExcersiceTerms.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler txt_PaymentTerms.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Freight.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_filter_billNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_DeliveryTerms.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler dtp_Filter_Fromdate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_ToDate.KeyPress, AddressOf TextBoxControlKeyPress

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True
        new_record()

    End Sub

    Private Sub Bobin_Purchase_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub Bobin_Purchase_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
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

    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
        Dim dgv1 As New DataGridView

        On Error Resume Next

        If ActiveControl.Name = dgv_Details.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            dgv1 = Nothing

            If ActiveControl.Name = dgv_Details.Name Then
                dgv1 = dgv_Details

            ElseIf dgv_Details.IsCurrentRowDirty = True Then
                dgv1 = dgv_Details
            ElseIf pnl_Back.Enabled = True Then
                dgv1 = dgv_Details

            End If

            If IsNothing(dgv1) = False Then

                With dgv1


                    If keyData = Keys.Enter Or keyData = Keys.Down Then
                        If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                txt_PaymentTerms.Focus()

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(DgvCol_BobinDetails.Ends)

                            End If

                        Else
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then

                        If .CurrentCell.ColumnIndex <= 1 Then
                            If .CurrentCell.RowIndex = 0 Then
                                txt_PaymentTerms.Focus()

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

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim Qa As Windows.Forms.DialogResult
        Dim vOrdByNo As String = ""

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_OrderNo.Text)
        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Bobin_Purchase_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Bobin_Purchase_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        Qa = MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
        If Qa = Windows.Forms.DialogResult.No Or Qa = Windows.Forms.DialogResult.Cancel Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        trans = con.BeginTransaction

        Try

            NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_OrderNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.Transaction = trans

            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "DELETE", "Bobin_PO_Head", "Bobin_PO_Code", Val(lbl_Company.Tag), NewCode, lbl_OrderNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "", "", "Bobin_PO_Code, Company_IdNo, for_OrderBy", trans)

            Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "DELETE", "Bobin_PO_Details", "Bobin_PO_Code", Val(lbl_Company.Tag), NewCode, lbl_OrderNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "EndsCount_Idno, Mill_IdNo,Colour_IdNo,Bobins, Bobin_Size_IdNo, Meter_Bobin, Meters, Meter_Reel, Reel, Rate, Amount", "Sl_No", "Bobin_PO_Code, For_OrderBy, Company_IdNo, Bobin_PO_No, Bobin_PO_Date, Ledger_Idno", trans)

            If Common_Procedures.VoucherBill_Deletion(con, Trim(Pk_Condition) & Trim(NewCode), trans) = False Then
                Throw New ApplicationException("Error on Voucher Bill Deletion")
            End If

            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), trans)
            ' Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition2) & Trim(NewCode), trans)

            cmd.CommandText = "Delete from Stock_Pavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Empty_BeamBagCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Bobin_PO_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Bobin_PO_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Bobin_PO_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Bobin_PO_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            trans.Commit()

            Dt1.Dispose()
            Da.Dispose()
            trans.Dispose()
            cmd.Dispose()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            trans.Rollback()
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

            da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 ) order by Ledger_DisplayName", con)
            da.Fill(dt1)
            cbo_Filter_PartyName.DataSource = dt1
            cbo_Filter_PartyName.DisplayMember = "Ledger_DisplayName"

            da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead Where (ledger_type = 'GODOWN' or Ledger_IdNo = 0) order by Ledger_DisplayName", con)
            da.Fill(dt2)
            cbo_Filter_DelvAt.DataSource = dt2
            cbo_Filter_DelvAt.DisplayMember = "Ledger_DisplayName"


            dtp_Filter_Fromdate.Text = ""
            dtp_Filter_ToDate.Text = ""
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_DelvAt.Text = ""
            txt_filter_billNo.Text = ""

            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_DelvAt.SelectedIndex = -1
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
        Dim movno As String = ""

        Try

            da = New SqlClient.SqlDataAdapter("select top 1 Bobin_PO_No from Bobin_PO_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Bobin_PO_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Bobin_PO_Code LIKE '" & Trim(Pk_Condition) & "%' Order by for_Orderby, Bobin_PO_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_OrderNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Bobin_PO_No from Bobin_PO_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Bobin_PO_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Bobin_PO_Code LIKE '" & Trim(Pk_Condition) & "%' Order by for_Orderby, Bobin_PO_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_OrderNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Bobin_PO_No from Bobin_PO_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Bobin_PO_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Bobin_PO_Code LIKE '" & Trim(Pk_Condition) & "%' Order by for_Orderby desc, Bobin_PO_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 Bobin_PO_No from Bobin_PO_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Bobin_PO_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Bobin_PO_Code LIKE '" & Trim(Pk_Condition) & "%' Order by for_Orderby desc, Bobin_PO_No desc", con)
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

            lbl_OrderNo.Text = Common_Procedures.get_MaxCode(con, "Bobin_PO_Head", "Bobin_PO_Code", "For_OrderBy", "( Bobin_PO_Code LIKE '" & Trim(Pk_Condition) & "%' )", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

            lbl_OrderNo.ForeColor = Color.Red

            msk_Date.Text = Date.Today.ToShortDateString
            da = New SqlClient.SqlDataAdapter("select top 1 * from Bobin_PO_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Bobin_PO_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "'  and Bobin_PO_Code LIKE '" & Trim(Pk_Condition) & "%' Order by for_Orderby desc, Bobin_PO_No desc", con)
            dt1 = New DataTable
            da.Fill(dt1)
            If dt1.Rows.Count > 0 Then
                If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)
                    If dt1.Rows(0).Item("Bobin_PO_Date").ToString <> "" Then msk_Date.Text = dt1.Rows(0).Item("Bobin_PO_Date").ToString
                End If
            End If
            dt1.Clear()


            If msk_Date.Enabled And msk_Date.Visible Then msk_Date.Focus()

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

            inpno = InputBox("Enter Ref.No.", "FOR FINDING...")

            RecCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Bobin_PO_No from Bobin_PO_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Bobin_PO_Code = '" & Trim(RecCode) & "'", con)
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
                MessageBox.Show("Ref No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Bobin_Purchase_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Bobin_Purchase_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        Try

            inpno = InputBox("Enter New Ref No.", "FOR NEW REF INSERTION...")

            RecCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Bobin_PO_No from Bobin_PO_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Bobin_PO_Code = '" & Trim(RecCode) & "'", con)
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
                    MessageBox.Show("Invalid Ref No", "DOES NOT INSERT NEW REF...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_OrderNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT NEW REF...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim Led_ID As Integer = 0
        Dim Ends_ID As Integer = 0
        Dim Mill_ID As Integer = 0
        Dim Itfp_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim Partcls As String = ""
        Dim EntID As String = ""
        Dim Clr_ID As Integer = 0
        Dim BobinSize_ID As Integer = 0

        Dim PBlNo As String = ""
        Dim vTotBns As Single, vTotMtrs As Single, vTotReel As Single
        Dim Proc_ID As Integer = 0
        Dim Lot_ID As Integer = 0
        Dim vTotamt As Single
        Dim Tr_ID As Integer = 0, Ag_Id As Integer = 0, DelT_Id As Integer = 0
        Dim itgry_id As Integer = 0, vatac_id As Integer = 0, PurAc_id As Integer = 0
        Dim Delv_ID As Integer = 0, Rec_Id As Integer = 0
        Dim TxAc_ID As Integer = 0
        Dim VouBil As String = ""
        Dim vOrdByNo As String = ""

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_OrderNo.Text)

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        'If Common_Procedures.UserRight_Check(Common_Procedures.UR.Bobin_Purchase_Entry, New_Entry) = False Then Exit Sub

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
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

        Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
        If Led_ID = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
            Exit Sub
        End If

        Tr_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_TransportName.Text)
        Ag_Id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Agent.Text)


        Delv_ID = Val(Common_Procedures.CommonLedger.Godown_Ac)
        Rec_Id = Led_ID


        With dgv_Details
            For i = 0 To dgv_Details.RowCount - 1

                If Val(.Rows(i).Cells(DgvCol_BobinDetails.Colour).Value) <> 0 Or Val(.Rows(i).Cells(DgvCol_BobinDetails.MeterBobin).Value) <> 0 Or Val(.Rows(i).Cells(DgvCol_BobinDetails.MeterReel).Value) <> 0 Or Val(.Rows(i).Cells(DgvCol_BobinDetails.Rate).Value) <> 0 Then

                    If Trim(dgv_Details.Rows(i).Cells(DgvCol_BobinDetails.Ends).Value) = "" Then
                        MessageBox.Show("Invalid Ends Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dgv_Details.Enabled And dgv_Details.Visible Then
                            dgv_Details.Focus()
                            dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(DgvCol_BobinDetails.Ends)
                        End If
                        Exit Sub
                    End If
                    If Val(dgv_Details.Rows(i).Cells(DgvCol_BobinDetails.Meters).Value) = 0 Then
                        MessageBox.Show("Invalid Meters..", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dgv_Details.Enabled And dgv_Details.Visible Then
                            dgv_Details.Focus()
                            dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(DgvCol_BobinDetails.Meters)
                            Exit Sub
                        End If
                    End If
                End If

            Next
        End With


        Total_Calculation()

        vTotMtrs = 0 : vTotamt = 0 : vTotBns = 0

        If dgv_Details_Total.RowCount > 0 Then
            vTotBns = Val(dgv_Details_Total.Rows(0).Cells(4).Value())
            vTotMtrs = Val(dgv_Details_Total.Rows(0).Cells(7).Value())
            vTotReel = Val(dgv_Details_Total.Rows(0).Cells(9).Value())
            vTotamt = Val(dgv_Details_Total.Rows(0).Cells(11).Value())
        End If


        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_OrderNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_OrderNo.Text = Common_Procedures.get_MaxCode(con, "Bobin_PO_Head", "Bobin_PO_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_OrderNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@PurchaseDate", dtp_Date.Value.Date)

            If New_Entry = True Then
                cmd.CommandText = "Insert into Bobin_PO_Head (Bobin_PO_Code          , Company_IdNo                     , Bobin_PO_No                     , for_OrderBy                                                              , Bobin_PO_Date , Ledger_IdNo             , PurchaseAc_Idno       , Agent_Idno         , Net_Amount                                 , Transport_IdNo         , Freight_Charge                    , Narration                       , Total_Bobin          , Total_Meters          , Total_Amount        , Total_Reel            ,  Delivery_Terms                       , Payment_Terms                         ,  Exercise_Terms                        ,   Rate_For                     ) " & _
                "Values                                      ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_OrderNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_OrderNo.Text))) & ", @PurchaseDate , " & Str(Val(Led_ID)) & ", " & Val(PurAc_id) & " , " & Val(Ag_Id) & " , " & Str(Val(CSng(lbl_NetAmount.Text))) & " , " & Str(Val(Tr_ID)) & ", " & Str(Val(txt_Freight.Text)) & ", '" & Trim(txt_Remarks.Text) & "', " & Val(vTotBns) & " , " & Val(vTotMtrs) & " , " & Val(vTotamt) & ", " & Val(vTotReel) & " ,'" & Trim(txt_DeliveryTerms.Text) & "' , '" & Trim(txt_PaymentTerms.Text) & "' , '" & Trim(txt_ExcersiceTerms.Text) & "','" & Trim(cbo_RateFor.Text) & "')"
                cmd.ExecuteNonQuery()

            Else
                Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "OLD", "Bobin_PO_Head", "Bobin_PO_Code", Val(lbl_Company.Tag), NewCode, lbl_OrderNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Bobin_PO_Code, Company_IdNo, for_OrderBy", tr)

                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "OLD", "Bobin_PO_Details", "Bobin_PO_Code", Val(lbl_Company.Tag), NewCode, lbl_OrderNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "EndsCount_Idno, Mill_IdNo,Colour_IdNo,Bobins, Bobin_Size_IdNo, Meter_Bobin, Meters, Meter_Reel, Reel, Rate, Amount", "Sl_No", "Bobin_PO_Code, For_OrderBy, Company_IdNo, Bobin_PO_No, Bobin_PO_Date, Ledger_Idno", tr)


                cmd.CommandText = "Update Bobin_PO_Head set Bobin_PO_Date = @PurchaseDate, Ledger_IdNo = " & Val(Led_ID) & ", PurchaseAc_Idno = " & Val(PurAc_id) & " , DeliveryTo_Idno = " & Val(DelT_Id) & ", Agent_Idno = " & Val(Ag_Id) & ", Net_Amount = " & Val(lbl_NetAmount.Text) & " ,Transport_IdNo = " & Val(Tr_ID) & ", Freight_Charge = " & Val(txt_Freight.Text) & ", Narration = '" & Trim(txt_Remarks.Text) & "' , Delivery_Terms = '" & Trim(txt_DeliveryTerms.Text) & "' , Payment_Terms = '" & Trim(txt_PaymentTerms.Text) & "' , Exercise_Terms = '" & Trim(txt_ExcersiceTerms.Text) & "', Total_Bobin = " & Val(vTotBns) & " , Total_Meters = " & Val(vTotMtrs) & " , Total_Amount = " & (vTotamt) & " , Total_Reel = " & Val(vTotReel) & " , Rate_For = '" & Trim(cbo_RateFor.Text) & "' Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Bobin_PO_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If
            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "NEW", "Bobin_PO_Head", "Bobin_PO_Code", Val(lbl_Company.Tag), NewCode, lbl_OrderNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Bobin_PO_Code, Company_IdNo, for_OrderBy", tr)

            cmd.CommandText = "Delete from Bobin_PO_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Bobin_PO_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Pavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Empty_BeamBagCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            Partcls = "BobinPurc : Ref.No. " & Trim(lbl_OrderNo.Text)
            PBlNo = Trim(lbl_OrderNo.Text)
            EntID = Trim(Pk_Condition) & Trim(lbl_OrderNo.Text)


            With dgv_Details
                Sno = 0
                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(DgvCol_BobinDetails.Colour).Value) <> 0 Or Val(.Rows(i).Cells(DgvCol_BobinDetails.Meters).Value) <> 0 Or Val(.Rows(i).Cells(DgvCol_BobinDetails.NofReel).Value) <> 0 Then

                        Sno = Sno + 1

                        Ends_ID = Common_Procedures.EndsCount_NameToIdNo(con, .Rows(i).Cells(DgvCol_BobinDetails.Ends).Value, tr)
                        Mill_ID = Common_Procedures.Mill_NameToIdNo(con, .Rows(i).Cells(DgvCol_BobinDetails.MillName).Value, tr)
                        Clr_ID = Common_Procedures.Colour_NameToIdNo(con, .Rows(i).Cells(DgvCol_BobinDetails.Colour).Value, tr)
                        BobinSize_ID = Common_Procedures.BobinSize_NameToIdNo(con, .Rows(i).Cells(DgvCol_BobinDetails.BobinSize).Value, tr)

                        cmd.CommandText = "Insert into Bobin_PO_Details(Bobin_PO_Code          , Company_IdNo                     , Bobin_PO_No                     , for_OrderBy                                                              , Bobin_PO_Date , Sl_No                , EndsCount_Idno       , Mill_IdNo           , Colour_IdNo         , Bobins                                                            , Bobin_Size_IdNo            , Meter_Bobin                                                      , Meters                                                        , Meter_Reel                                                      , Reel                                                                , Rate                                                        , Amount                                                             ) " & _
                        "Values                                        ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_OrderNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_OrderNo.Text))) & ", @PurchaseDate , " & Str(Val(Sno)) & ",  " & Val(Ends_ID) & ", " & Val(Mill_ID) & ", " & Val(Clr_ID) & " , " & Val(.Rows(i).Cells(DgvCol_BobinDetails.NoOfBobins).Value) & " ,  " & Val(BobinSize_ID) & " , " & Val(.Rows(i).Cells(DgvCol_BobinDetails.MeterBobin).Value) & ", " & Val(.Rows(i).Cells(DgvCol_BobinDetails.Meters).Value) & " , " & Val(.Rows(i).Cells(DgvCol_BobinDetails.MeterReel).Value) & ", " & Str(Val(.Rows(i).Cells(DgvCol_BobinDetails.NofReel).Value)) & " , " & Val(.Rows(i).Cells(DgvCol_BobinDetails.Rate).Value) & " , " & Str(Val(.Rows(i).Cells(DgvCol_BobinDetails.Amount).Value)) & " )"
                        cmd.ExecuteNonQuery()

                        cmd.CommandText = "Insert into Stock_Pavu_Processing_Details ( Reference_Code, Company_IdNo                     , Reference_No                    , for_OrderBy                                                              , Reference_Date, DeliveryTo_Idno          , ReceivedFrom_Idno , StockOf_IdNo , Entry_ID             , Party_Bill_No        , Particulars            , Sl_No                , EndsCount_IdNo           , Colour_IdNo             , Mill_idNo            , Meters_Bobin                                                           , Bobins                                                                 , Meters                                                             , Bobin_Size_IdNo                ) " & _
                        "Values                        ( '" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_OrderNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_OrderNo.Text))) & ", @PurchaseDate , " & Str(Val(Delv_ID)) & ", 0                 , 0            , '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', " & Str(Val(Sno)) & ", " & Str(Val(Ends_ID)) & ", " & Str(Val(Clr_ID)) & ", " & Val(Mill_ID) & " , " & Str(Val(.Rows(i).Cells(DgvCol_BobinDetails.MeterBobin).Value)) & " , " & Str(Val(.Rows(i).Cells(DgvCol_BobinDetails.NoOfBobins).Value)) & " , " & Str(Val(.Rows(i).Cells(DgvCol_BobinDetails.Meters).Value)) & " , " & Str(Val(BobinSize_ID)) & " )"
                        cmd.ExecuteNonQuery()


                    End If

                Next

                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "NEW", "Bobin_PO_Details", "Bobin_PO_Code", Val(lbl_Company.Tag), NewCode, lbl_OrderNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "EndsCount_Idno, Mill_IdNo,Colour_IdNo,Bobins, Bobin_Size_IdNo, Meter_Bobin, Meters, Meter_Reel, Reel, Rate, Amount", "Sl_No", "Bobin_PO_Code, For_OrderBy, Company_IdNo, Bobin_PO_No, Bobin_PO_Date, Ledger_Idno", tr)

            End With

            If Val(vTotBns) <> 0 Or Val(vTotMtrs) <> 0 Then
                cmd.CommandText = "Insert into Stock_Empty_BeamBagCone_Processing_Details(Reference_Code, Company_IdNo                     , Reference_No                    , for_OrderBy                                                              , Reference_Date, DeliveryTo_Idno          , ReceivedFrom_Idno , Party_Bill_No        , Particulars            , Sl_No , Empty_Cones, Empty_Bobin              , EmptyBobin_Party, Empty_Jumbo) " & _
                "Values                                    ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_OrderNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_OrderNo.Text))) & ", @PurchaseDate , " & Str(Val(Delv_ID)) & ", 0                 , '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', 1     , 0          , " & Str(Val(vTotBns)) & ", 0               , 0          )"
                cmd.ExecuteNonQuery()
            End If

            'AgentCommission Posting
            cmd.CommandText = "delete from AgentCommission_Processing_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            tr.Commit()

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)

            If Val(Common_Procedures.settings.OnSave_MoveTo_NewEntry_Status) = 1 Then
                If New_Entry = True Then
                    new_record()
                Else
                    move_record(lbl_OrderNo.Text)
                End If
            Else
                move_record(lbl_OrderNo.Text)
            End If


        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally

            Dt1.Dispose()
            Da.Dispose()
            cmd.Dispose()
            tr.Dispose()
            Dt1.Clear()
            If msk_Date.Enabled And msk_Date.Visible Then msk_Date.Focus()

        End Try
    End Sub

    Private Sub Total_Calculation()
         Dim vTotBns As Single, vTotMtrs As Single, vtotamt As Single, vtotreel As Single
        Dim i As Integer
        Dim sno As Integer
        If FrmLdSTS = True Then Exit Sub
        vTotBns = 0 : vTotMtrs = 0 : vtotamt = 0 : sno = 0 : vtotreel = 0
        With dgv_Details
            For i = 0 To dgv_Details.Rows.Count - 1

                sno = sno + 1

                .Rows(i).Cells(DgvCol_BobinDetails.SNo).Value = sno

                If Val(dgv_Details.Rows(i).Cells(DgvCol_BobinDetails.NoOfBobins).Value) <> 0 Or Val(dgv_Details.Rows(i).Cells(DgvCol_BobinDetails.Meters).Value) <> 0 Or Val(dgv_Details.Rows(i).Cells(DgvCol_BobinDetails.NofReel).Value) <> 0 Or Val(dgv_Details.Rows(i).Cells(DgvCol_BobinDetails.Amount).Value) <> 0 Then

                    .Rows(i).Cells(DgvCol_BobinDetails.NofReel).Value = Val(dgv_Details.Rows(i).Cells(DgvCol_BobinDetails.Meters).Value) * Val(dgv_Details.Rows(i).Cells(DgvCol_BobinDetails.MeterReel).Value)

                    vTotBns = vTotBns + Val(dgv_Details.Rows(i).Cells(DgvCol_BobinDetails.NoOfBobins).Value)

                    vTotMtrs = vTotMtrs + Val(dgv_Details.Rows(i).Cells(DgvCol_BobinDetails.Meters).Value)
                    vtotreel = vtotreel + Val(dgv_Details.Rows(i).Cells(DgvCol_BobinDetails.NofReel).Value)
                    vtotamt = vtotamt + Val(dgv_Details.Rows(i).Cells(DgvCol_BobinDetails.Amount).Value)
                End If
            Next
        End With

        If dgv_Details_Total.Rows.Count <= 0 Then dgv_Details_Total.Rows.Add()
        dgv_Details_Total.Rows(0).Cells(4).Value = Val(vTotBns)
        dgv_Details_Total.Rows(0).Cells(7).Value = Format(Val(vTotMtrs), "#########0.00")
        dgv_Details_Total.Rows(0).Cells(9).Value = Val(vtotreel)
        dgv_Details_Total.Rows(0).Cells(11).Value = Format(Val(vtotamt), "#########0.000")

        lbl_NetAmount.Text = Format(Val(Val(vtotamt) + Val(txt_Freight.Text)), "########0.00")


    End Sub

    Private Sub cbo_Ledger_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) )  or Show_In_All_Entry = 1 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Ledger_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, msk_Date, cbo_RateFor, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) )  or Show_In_All_Entry = 1 )", "(Ledger_idno = 0)")
        'With dgv_Details
        '    If e.KeyCode = 40 And cbo_Ledger.DroppedDown = False Or (e.Control = True And e.KeyCode = 40) Then
        '        If .Visible = True Then
        '            .Focus()
        '            .CurrentCell = .CurrentRow.Cells(1).Value
        '            .CurrentCell.Selected = True
        '        Else
        '            txt_PaymentTerms.Focus()
        '        End If
        '    End If
        'End With
    End Sub

    Private Sub cbo_Ledger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ledger.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Ledger, cbo_RateFor, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) )  or Show_In_All_Entry = 1 )", "(Ledger_idno = 0)")

    End Sub

    Private Sub cbo_Ledger_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Ledger.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub dgv_Details_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEndEdit
        With dgv_Details

            If .CurrentCell.ColumnIndex = DgvCol_BobinDetails.Meters Or .CurrentCell.ColumnIndex = DgvCol_BobinDetails.Rate Or .CurrentCell.ColumnIndex = DgvCol_BobinDetails.MeterBobin Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                End If
            End If
            If .CurrentCell.ColumnIndex = DgvCol_BobinDetails.NofReel Or .CurrentCell.ColumnIndex = DgvCol_BobinDetails.Amount Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                End If
            End If
            Total_Calculation()

        End With
    End Sub

    Private Sub dgv_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim Dt4 As New DataTable
        Dim rect As Rectangle

        With dgv_Details
            If Val(.CurrentRow.Cells(DgvCol_BobinDetails.SNo).Value) = 0 Then
                .CurrentRow.Cells(DgvCol_BobinDetails.SNo).Value = .CurrentRow.Index + 1
            End If


            If e.ColumnIndex = DgvCol_BobinDetails.Ends Then

                If cbo_Ends.Visible = False Or Val(cbo_Ends.Tag) <> e.RowIndex Then

                    cbo_Ends.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select EndsCount_Name from EndsCount_Head order by EndsCount_Name", con)
                    Dt3 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Ends.DataSource = Dt1
                    cbo_Ends.DisplayMember = "EndsCount_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Ends.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_Ends.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                    cbo_Ends.Width = rect.Width  ' .CurrentCell.Size.Width
                    cbo_Ends.Height = rect.Height  ' rect.Height

                    cbo_Ends.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_Ends.Tag = Val(e.RowIndex)
                    cbo_Ends.Visible = True

                    cbo_Ends.BringToFront()
                    cbo_Ends.Focus()

                End If

            Else
                cbo_Ends.Visible = False
            End If


            If e.ColumnIndex = DgvCol_BobinDetails.MillName Then

                If cbo_MillName.Visible = False Or Val(cbo_MillName.Tag) <> e.RowIndex Then

                    cbo_MillName.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Mill_Name from Mill_Head order by Mill_Name", con)
                    Dt3 = New DataTable
                    Da.Fill(Dt3)
                    cbo_MillName.DataSource = Dt3
                    cbo_MillName.DisplayMember = "Mill_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_MillName.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_MillName.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                    cbo_MillName.Width = rect.Width  ' .CurrentCell.Size.Width
                    cbo_MillName.Height = rect.Height  ' rect.Height

                    cbo_MillName.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_MillName.Tag = Val(e.RowIndex)
                    cbo_MillName.Visible = True

                    cbo_MillName.BringToFront()
                    cbo_MillName.Focus()

                End If

            Else

                cbo_MillName.Visible = False
                'cbo_Grid_MillName.Tag = -1
                'cbo_Grid_MillName.Text = ""

            End If

            If e.ColumnIndex = DgvCol_BobinDetails.Colour Then

                If cbo_Colour.Visible = False Or Val(cbo_Colour.Tag) <> e.RowIndex Then

                    cbo_Colour.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Colour_Name from Colour_Head order by Colour_Name", con)
                    Dt3 = New DataTable
                    Da.Fill(Dt3)
                    cbo_Colour.DataSource = Dt3
                    cbo_Colour.DisplayMember = "Colour_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Colour.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_Colour.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                    cbo_Colour.Width = rect.Width  ' .CurrentCell.Size.Width
                    cbo_Colour.Height = rect.Height  ' rect.Height

                    cbo_Colour.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_Colour.Tag = Val(e.RowIndex)
                    cbo_Colour.Visible = True

                    cbo_Colour.BringToFront()
                    cbo_Colour.Focus()

                End If

            Else

                cbo_Colour.Visible = False
                'cbo_Grid_MillName.Tag = -1
                'cbo_Grid_MillName.Text = ""

            End If


            If e.ColumnIndex = DgvCol_BobinDetails.BobinSize Then

                If cbo_BobinSize.Visible = False Or Val(cbo_BobinSize.Tag) <> e.RowIndex Then

                    cbo_BobinSize.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Bobin_Size_Name from Bobin_Size_Head order by Bobin_Size_Name", con)
                    Dt4 = New DataTable
                    Da.Fill(Dt4)
                    cbo_BobinSize.DataSource = Dt4
                    cbo_BobinSize.DisplayMember = "Bobin_Size_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_BobinSize.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_BobinSize.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                    cbo_BobinSize.Width = rect.Width  ' .CurrentCell.Size.Width
                    cbo_BobinSize.Height = rect.Height  ' rect.Height

                    cbo_BobinSize.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_BobinSize.Tag = Val(e.RowIndex)
                    cbo_BobinSize.Visible = True

                    cbo_BobinSize.BringToFront()
                    cbo_BobinSize.Focus()

                End If

            Else
                cbo_BobinSize.Visible = False
            End If


        End With
    End Sub

    Private Sub dgv_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellLeave
        With dgv_Details

            If .CurrentCell.ColumnIndex = DgvCol_BobinDetails.Meters Or .CurrentCell.ColumnIndex = DgvCol_BobinDetails.Rate Or .CurrentCell.ColumnIndex = DgvCol_BobinDetails.MeterBobin Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                End If
            End If
            If .CurrentCell.ColumnIndex = DgvCol_BobinDetails.NofReel Or .CurrentCell.ColumnIndex = DgvCol_BobinDetails.Amount Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                End If
            End If
        End With
    End Sub

    Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged
        Dim i As Integer
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Ends_Idno As Integer = 0
        Dim vTotMtrs As Single
        Dim Ends As Single
        Dim ratefr As String = ""

        On Error Resume Next
        If FrmLdSTS = True Then Exit Sub
        If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
        With dgv_Details

            If .Visible Then

                If e.ColumnIndex = DgvCol_BobinDetails.NoOfBobins Or e.ColumnIndex = DgvCol_BobinDetails.MeterBobin Or e.ColumnIndex = DgvCol_BobinDetails.Rate Then
                    .Rows(e.RowIndex).Cells(DgvCol_BobinDetails.Meters).Value = Val(.Rows(e.RowIndex).Cells(DgvCol_BobinDetails.NoOfBobins).Value) * Val(.Rows(e.RowIndex).Cells(DgvCol_BobinDetails.MeterBobin).Value)
                    If Trim(UCase(cbo_RateFor.Text)) <> "REEL" Then
                        .Rows(e.RowIndex).Cells(DgvCol_BobinDetails.Amount).Value = Format(Val(.Rows(e.RowIndex).Cells(DgvCol_BobinDetails.Meters).Value) * Val(.Rows(e.RowIndex).Cells(DgvCol_BobinDetails.Rate).Value), "###########0.00")
                    End If

                End If

                If e.ColumnIndex = DgvCol_BobinDetails.Ends Or e.ColumnIndex = DgvCol_BobinDetails.Meters Or e.ColumnIndex = DgvCol_BobinDetails.MeterReel Then
                    If Val(.Rows(e.RowIndex).Cells(DgvCol_BobinDetails.MeterReel).Value) <> 0 Then

                        If Trim(.Rows(e.RowIndex).Cells(DgvCol_BobinDetails.Ends).Value) <> "" Then
                            Ends_Idno = Common_Procedures.EndsCount_NameToIdNo(con, Trim(.Rows(e.RowIndex).Cells(DgvCol_BobinDetails.Ends).Value))

                            da = New SqlClient.SqlDataAdapter("select a.Ends_Name from EndsCount_Head a  Where a.EndsCount_IdNo = " & Str(Val(Ends_Idno)), con)
                            dt = New DataTable
                            da.Fill(dt)

                            Ends = 0

                            If dt.Rows.Count > 0 Then
                                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                                    Ends = Val(dt.Rows(0).Item("Ends_Name").ToString)
                                End If
                            End If

                            dt.Dispose()
                            da.Dispose()
                        End If

                        .Rows(e.RowIndex).Cells(DgvCol_BobinDetails.NofReel).Value = Format(Val(Val(.Rows(e.RowIndex).Cells(DgvCol_BobinDetails.Meters).Value) * Val(Ends)) / Val(.Rows(e.RowIndex).Cells(DgvCol_BobinDetails.MeterReel).Value), "#########0.00")

                    End If

                End If

                If e.ColumnIndex = DgvCol_BobinDetails.NofReel Or e.ColumnIndex = DgvCol_BobinDetails.Rate Then
                    If Trim(UCase(cbo_RateFor.Text)) = "REEL" Then
                        .Rows(e.RowIndex).Cells(DgvCol_BobinDetails.Amount).Value = Format(Val(.Rows(e.RowIndex).Cells(DgvCol_BobinDetails.NofReel).Value) * Val(.Rows(e.RowIndex).Cells(DgvCol_BobinDetails.Rate).Value), "############0.00")
                    End If
                End If

            End If

        End With
    End Sub

    Private Sub Amount_Calculation(ByVal CurRow As Integer, ByVal CurCol As Integer)
        Dim NtAmt As Single = 0


        If FrmLdSTS = True Then Exit Sub
        With dgv_Details
            If .Visible Then
                If .Rows.Count > 0 Then
                    If CurCol = DgvCol_BobinDetails.Meters Or CurCol = DgvCol_BobinDetails.NofReel Or CurCol = DgvCol_BobinDetails.Rate Then
                        If Trim(UCase(cbo_RateFor.Text)) = "METERS" Then
                            .Rows(CurRow).Cells(DgvCol_BobinDetails.Amount).Value = Format(Val(.Rows(CurRow).Cells(DgvCol_BobinDetails.Meters).Value) * Val(.Rows(CurRow).Cells(DgvCol_BobinDetails.Rate).Value), "###########0.00")
                        Else
                            .Rows(CurRow).Cells(DgvCol_BobinDetails.Amount).Value = Format(Val(.Rows(CurRow).Cells(DgvCol_BobinDetails.NofReel).Value) * Val(.Rows(CurRow).Cells(DgvCol_BobinDetails.Rate).Value), "############0.00")
                        End If

                        Total_Calculation()

                    End If

                End If
            End If

        End With

    End Sub

    Private Sub dgv_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Details.EditingControlShowing
        dgtxt_details = CType(dgv_Details.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_details.Enter
        dgv_Details.EditingControl.BackColor = Color.Lime
    End Sub

    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_details.KeyPress
        With dgv_Details

            If Val(dgv_Details.CurrentCell.ColumnIndex.ToString) = DgvCol_BobinDetails.NoOfBobins Or Val(dgv_Details.CurrentCell.ColumnIndex.ToString) = DgvCol_BobinDetails.Meters Or Val(dgv_Details.CurrentCell.ColumnIndex.ToString) = DgvCol_BobinDetails.MeterBobin Or Val(dgv_Details.CurrentCell.ColumnIndex.ToString) = DgvCol_BobinDetails.MeterReel Or Val(dgv_Details.CurrentCell.ColumnIndex.ToString) = DgvCol_BobinDetails.NofReel Then

                If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                    e.Handled = True
                End If
            End If

        End With

    End Sub

    Private Sub dgv_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyUp
        Dim n As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_Details

                n = .CurrentRow.Index

                If .Rows.Count = 1 Then
                    For i = 0 To .Columns.Count - 1
                        .Rows(n).Cells(i).Value = ""
                    Next

                Else

                    .Rows.RemoveAt(n)

                End If

                For i = 0 To .Rows.Count - 1
                    .Rows(i).Cells(DgvCol_BobinDetails.SNo).Value = i + 1
                Next

            End With

            Total_Calculation()

        End If

    End Sub

    Private Sub dgv_Details_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Details.RowsAdded
        Dim n As Integer
        If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
        With dgv_Details
            n = .RowCount
            .Rows(n - 1).Cells(DgvCol_BobinDetails.SNo).Value = Val(n)
        End With
    End Sub

    Private Sub cbo_ends_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ends.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ends, Nothing, Nothing, "EndsCount_Head", "EndsCount_Name", "(Cotton_Polyester_Jari<>'COTTON') and (Close_Status=0) ", "(EndsCount_IdNo = 0)")
        With dgv_Details

            If (e.KeyValue = 38 And cbo_Ends.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                If .CurrentCell.RowIndex = 0 Then
                    cbo_Ledger.Focus()
                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentRow.Index - 1).Cells(.ColumnCount - 1)
                End If
            End If

            If (e.KeyValue = 40 And cbo_Ends.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                If .CurrentCell.RowIndex = .Rows.Count - 1 And Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(1).Value) = "" Then
                    txt_PaymentTerms.Focus()

                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

                End If
            End If
        End With
    End Sub

    Private Sub cbo_Ends_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ends.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Ends, Nothing, "EndsCount_Head", "EndsCount_Name", "(Cotton_Polyester_Jari<>'COTTON') and (Close_Status=0) ", "(EndsCount_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            With dgv_Details
                If .CurrentCell.RowIndex = .Rows.Count - 1 And Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(1).Value) = "" Then
                    txt_PaymentTerms.Focus()

                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                End If
            End With

        End If
    End Sub


    Private Sub cbo_Ends_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ends.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New EndsCount_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Ends.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_Ends_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ends.TextChanged
        Try
            If cbo_Ends.Visible Then
                With dgv_Details
                    If Val(cbo_Ends.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = DgvCol_BobinDetails.Ends Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Ends.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btn_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub dgv_Details_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.GotFocus
        dgv_Details.Focus()
        'dgv_Details.CurrentCell.Selected = True
    End Sub

    Private Sub btn_Filter_Show_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter_Show.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim n As Integer, i As Integer
        Dim Led_IdNo As Integer, Delat_IdNo As Integer
        Dim Condt As String = ""


        Try

            Condt = ""
            Led_IdNo = 0
            Delat_IdNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Bobin_PO_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Bobin_PO_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Bobin_PO_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If

            If Trim(cbo_Filter_DelvAt.Text) <> "" Then
                Delat_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_DelvAt.Text)
            End If

            If Val(Delat_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.DeliveryTo_Idno = " & Str(Val(Delat_IdNo))
            End If


            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.Ledger_Idno = " & Str(Val(Led_IdNo))
            End If

            If Trim(txt_filter_billNo.Text) <> "" Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.Party_Bill_No = '" & Trim(txt_filter_billNo.Text) & "'"
            End If

            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name,C.Ledger_Name as Delv_Name from Bobin_PO_Head a INNER JOIN Ledger_Head b on a.Ledger_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Ledger_Head c ON a.DeliveryTo_Idno = c.Ledger_Idno where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Bobin_PO_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Bobin_PO_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()


                    dgv_Filter_Details.Rows(n).Cells(0).Value = dt2.Rows(i).Item("Bobin_PO_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(1).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Bobin_PO_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Party_Bill_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = dt2.Rows(i).Item("Delv_Name").ToString

                    'dgv_Filter_Details.Rows(n).Cells(5).Value = Val(dt2.Rows(i).Item("Total_Pcs").ToString)
                    'dgv_Filter_Details.Rows(n).Cells(6).Value = Val(dt2.Rows(i).Item("Total_Qty").ToString)
                    'dgv_Filter_Details.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Total_Meter").ToString), "########0.00")
                    'dgv_Filter_Details.Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("Total_Weight").ToString), "########0.000")
                    dgv_Filter_Details.Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("Net_Amount").ToString), "########0.000")

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

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, txt_filter_billNo, cbo_Filter_DelvAt, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, cbo_Filter_DelvAt, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) ", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_DelvAt_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_DelvAt.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_DelvAt, cbo_Filter_PartyName, btn_Filter_Show, "Ledger_AlaisHead", "Ledger_DisplayName", " (ledger_idno = 0 or Ledger_Type = 'GODOWN' )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_DelvAt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_DelvAt.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_DelvAt, btn_Filter_Show, "Ledger_AlaisHead", "Ledger_DisplayName", " (ledger_idno = 0 or Ledger_Type = 'GODOWN' ) ", "(Ledger_idno = 0)")
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

    Public Sub print_record() Implements Interface_MDIActions.print_record
        pnl_Back.Enabled = False
        Print_Selection()
    End Sub

    Private Sub txt_Note_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Remarks.KeyDown
        If e.KeyCode = 40 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                msk_Date.Focus()
            End If
        End If

        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_Note_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Remarks.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                msk_Date.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_Agent_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Agent.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Agent, txt_DeliveryTerms, cbo_TransportName, "Ledger_AlaisHead", "Ledger_DisplayName", " (ledger_idno = 0 or Ledger_Type = 'AGENT' )", "(Ledger_idno = 0)")

    End Sub

    Private Sub cbo_Agent_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Agent.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Agent, cbo_TransportName, "Ledger_AlaisHead", "Ledger_DisplayName", " (ledger_idno = 0 or Ledger_Type = 'AGENT') ", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Agent_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Agent.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            Dim f As New Agent_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Agent.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub btn_Filter_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter_Close.Click
        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        Filter_Status = False
    End Sub

    Private Sub cbo_TransportName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_TransportName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_TransportName, cbo_Agent, txt_Freight, "Ledger_AlaisHead", "Ledger_DisplayName", " (ledger_idno = 0 or Ledger_Type = 'TRANSPORT' )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Transportname_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_TransportName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_TransportName, txt_Freight, "Ledger_AlaisHead", "Ledger_DisplayName", " (ledger_idno = 0 or Ledger_Type = 'TRANSPORT') ", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_TransportName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_TransportName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
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

    Private Sub txt_Freight_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Freight.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_PaymentTerms_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_PaymentTerms.KeyPress
        
    End Sub

    Private Sub cbo_colour_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Colour.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Colour, Nothing, Nothing, "colour_Head", "colour_Name", "", "(Colour_IdNo = 0)")
        With dgv_Details

            If (e.KeyValue = 38 And cbo_Colour.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_Colour.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
            End If

        End With
    End Sub

    Private Sub cbo_colour_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Colour.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Colour, Nothing, "colour_Head", "colour_Name", "", "(Colour_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then

            With dgv_Details

                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

            End With

        End If
    End Sub

    Private Sub cbo_colour_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Colour.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Color_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Colour.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_Colour_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Colour.TextChanged
        Try
            If cbo_Colour.Visible Then
                With dgv_Details
                    If Val(cbo_Colour.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = DgvCol_BobinDetails.Colour Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Colour.Text)
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

    Private Sub txt_PaymentTerms_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_PaymentTerms.TextChanged

    End Sub

    Private Sub cbo_MillName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_MillName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")
    End Sub

    Private Sub cbo_MillName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_MillName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_MillName, Nothing, Nothing, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")
        With dgv_Details

            If (e.KeyValue = 38 And cbo_MillName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_MillName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
            End If

        End With
    End Sub

    Private Sub cbo_MillName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_MillName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_MillName, Nothing, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then

            With dgv_Details

                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

            End With

        End If
    End Sub

    Private Sub cbo_MillName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_MillName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Mill_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_MillName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_MillName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_MillName.TextChanged
        Try
            If cbo_MillName.Visible Then
                With dgv_Details
                    If Val(cbo_MillName.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = DgvCol_BobinDetails.MillName Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_MillName.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_BobinSize_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_BobinSize.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Bobin_Size_Head", "Bobin_Size_Name", "", "(Bobin_Size_IdNo = 0)")
    End Sub

    Private Sub cbo_BobinSize_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_BobinSize.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_BobinSize, Nothing, Nothing, "Bobin_Size_Head", "Bobin_Size_Name", "", "(Bobin_Size_IdNo = 0)")

        With dgv_Details
            If e.KeyCode = 38 And cbo_BobinSize.DroppedDown = False Or (e.Control = True And e.KeyCode = 38) Then
                If .Visible Then
                    .Focus()
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
                    .CurrentCell.Selected = True
                End If
            End If
            If e.KeyCode = 40 And cbo_BobinSize.DroppedDown = False Or (e.Control = True And e.KeyCode = 40) Then
                If .Visible Then
                    .Focus()
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
                    .CurrentCell.Selected = True
                Else
                    txt_PaymentTerms.Focus()
                End If
            End If
        End With

    End Sub

    Private Sub cbo_BobinSize_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_BobinSize.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_BobinSize, Nothing, "Bobin_Size_Head", "Bobin_Size_Name", "", "(Bobin_Size_IdNo = 0)")
        With dgv_Details
            If Asc(e.KeyChar) = 13 Then
                If .Visible Then
                    .Focus()
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
                    .CurrentCell.Selected = True
                Else
                    txt_PaymentTerms.Focus()
                End If
            End If
        End With

    End Sub

    Private Sub cbo_BobinSize_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_BobinSize.KeyUp
        If e.KeyCode = 17 And e.Control = False Then
            Dim g As New Bobin_Size_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_BobinSize.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            g.MdiParent = MDIParent1
            g.Show()
        End If
    End Sub

    Private Sub cbo_BobinSize_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_BobinSize.TextChanged
        Try
            If FrmLdSTS = True Then Exit Sub
            If cbo_BobinSize.Visible Then
                With dgv_Details
                    If IsNothing(.CurrentCell) Then Exit Sub
                    If Val(cbo_BobinSize.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = DgvCol_BobinDetails.BobinSize Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_BobinSize.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_RateFor_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_RateFor.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")
    End Sub

    Private Sub cbo_RateFor_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_RateFor.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_RateFor, cbo_Ledger, Nothing, "", "", "", "")
        With dgv_Details
            If e.KeyCode = 40 And cbo_RateFor.DroppedDown = False Or (e.Control = True And e.KeyCode = 40) Then
                If .Visible Then
                    .Focus()
                    .CurrentCell = .CurrentRow.Cells(1).Value
                    .CurrentCell.Selected = True
                Else
                    txt_PaymentTerms.Focus()
                End If
            End If
        End With

    End Sub

    Private Sub cbo_RateFor_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_RateFor.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_RateFor, Nothing, "", "", "", "")
        With dgv_Details
            If Asc(e.KeyChar) = 13 Then
                If .Visible Then
                    .Focus()
                    .CurrentCell = .Rows(0).Cells(1)
                    .CurrentCell.Selected = True
                Else
                    txt_PaymentTerms.Focus()
                End If
            End If
        End With
    End Sub

    Private Sub cbo_RateFor_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_RateFor.TextChanged
        'Amount_Calculation(e.CurRow, e.CurCol)
        Total_Calculation()
    End Sub

    Private Sub btn_Print_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        pnl_Back.Enabled = False
    End Sub


    Private Sub Print_Selection()
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim ps As Printing.PaperSize
        Dim PpSzSTS As Boolean = False
        Dim NewCode As String = ""


        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_OrderNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select * from Bobin_PO_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Bobin_PO_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count <= 0 Then
                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub
            End If

            dt1.Dispose()
            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try

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

        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then

            Try

                If Val(Common_Procedures.settings.Printing_Show_PrintDialogue) = 1 Then
                    PrintDialog1.PrinterSettings = PrintDocument1.PrinterSettings
                    If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then

                        PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings

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

                    PrintDocument1.Print()

                End If

            Catch ex As Exception
                MessageBox.Show("The printing operation failed" & vbCrLf & ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End Try

        Else
            Try

                Dim ppd As New PrintPreviewDialog

                ppd.Document = PrintDocument1
                ppd.WindowState = FormWindowState.Maximized
                ppd.StartPosition = FormStartPosition.CenterScreen
                ppd.PrintPreviewControl.AutoZoom = True
                ppd.PrintPreviewControl.Zoom = 1.0

                ppd.ShowDialog()

            Catch ex As Exception
                MsgBox("The printing operation failed" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "DOES NOT SHOW PRINT PREVIEW...")

            End Try

        End If
    End Sub

    Private Sub PrintDocument1_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument1.BeginPrint
        Dim da As New SqlClient.SqlDataAdapter
        Dim Da2 As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim NewCode As String
        Dim cmd As New SqlClient.SqlCommand

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_OrderNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Rows.Clear()
        prn_DetDt.Rows.Clear()
        prn_DetIndx = 0
        prn_PageNo = 0
        prn_DetSNo = 0

        cmd.Connection = con

        Try

            da = New SqlClient.SqlDataAdapter("SELECT BPH.*, CH.*, CSH.State_Name as Company_State_Name, LH.* , LSH.State_Name as Ledger_State_Name, LAH.Ledger_Name, LTH.Ledger_Name,LDH.Ledger_Name,CSH.State_Code as Company_State_Code,LSH.State_Code as Ledger_State_Code,LAH.Ledger_Name AS Agent_Name,LTH.Ledger_Name AS Transport_Name FROM BOBIN_PO_HEAD BPH INNER JOIN Company_Head CH ON BPH.Company_IdNo = CH.Company_IdNo INNER JOIN Ledger_Head LH ON BPH.Ledger_IdNo = LH.Ledger_IdNo LEFT OUTER JOIN State_Head CSH ON CH.Company_State_IdNo = CSH.State_IdNo LEFT OUTER JOIN State_Head LSH ON LH.Ledger_State_IdNo = LSH.State_IdNo  LEFT OUTER JOIN Ledger_Head LAH ON BPH.Agent_IdNo = LAH.Ledger_IdNo LEFT OUTER JOIN Ledger_Head LTH ON BPH.Transport_idno = LTH.Ledger_IdNo LEFT OUTER JOIN Ledger_Head LDH ON BPH.DeliveryTo_Idno = LDH.Ledger_IdNo WHERE BPH.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " AND BPH.Bobin_PO_Code ='" & Trim(Pk_Condition) & Trim(NewCode) & "' ORDER BY BPH.For_OrderBy, BPH.Bobin_PO_No", con)
            dt = New DataTable
            da.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then
                Da2 = New SqlClient.SqlDataAdapter("SELECT BPD.*,ECH.EndsCount_Name  ,CLH.Colour_Name,MH.Mill_Name,BSH.Bobin_Size_Name FROM BOBIN_PO_DETAILS BPD INNER JOIN EndsCount_Head ECH ON BPD.EndsCount_Idno = ECH.EndsCount_IdNo LEFT OUTER JOIN Colour_Head CLH ON BPD.COLOUR_IDNO = CLH.Colour_IdNo INNER JOIN Mill_Head MH ON BPD.Mill_IdNo = MH.Mill_IdNo LEFT OUTER JOIN Bobin_Size_Head BSH ON BPD.Bobin_Size_IdNo = BSH.Bobin_Size_IdNo WHERE BPD.Bobin_PO_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' ORDER BY BPD.For_OrderBy,BPD.Bobin_PO_No", con)
                dt = New DataTable
                Da2.Fill(prn_DetDt)
            Else

                MessageBox.Show("This is New Entry", "DOES NOT PRINT)", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                cbo_Ledger.Focus()
                Exit Sub
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Private Sub PrintDocument1_PrintPage(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        If prn_HdDt.Rows.Count < 0 Then Exit Sub

        Printing_FormatGST(e)
    End Sub

    Private Sub Printing_FormatGST(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim cmd As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim EntryCode As String
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim Mill1 As String, Mill2 As String
        Dim Ends1 As String, Ends2 As String
        Dim Clr1 As String, Clr2 As String
        Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim W1 As Single = 0
        Dim SNo As Integer


        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                e.PageSettings.PaperSize = ps
                Exit For
            End If
        Next

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 20
            .Right = 50
            .Top = 20
            .Bottom = 40
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

        NoofItems_PerPage = 18 ' 6

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = Val(30) : ClArr(2) = 120 : ClArr(3) = 160 : ClArr(4) = 100 : ClArr(5) = 80 : ClArr(6) = 80 : ClArr(7) = 70
        ClArr(8) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7))

        TxtHgt = 18.5

        EntryCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_OrderNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_FormatGST_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                NoofDets = 0

                CurY = CurY - 10

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then
                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_FormatGST_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

                            e.HasMorePages = True
                            Return

                        End If

                        prn_DetSNo = prn_DetSNo + 1

                        Mill1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Mill_Name").ToString)
                        Mill2 = ""
                        If Len(Mill1) > 20 Then
                            For I = 20 To 1 Step -1
                                If Mid$(Trim(Mill1), I, 1) = " " Or Mid$(Trim(Mill1), I, 1) = "," Or Mid$(Trim(Mill1), I, 1) = "." Or Mid$(Trim(Mill1), I, 1) = "-" Or Mid$(Trim(Mill1), I, 1) = "/" Or Mid$(Trim(Mill1), I, 1) = "_" Or Mid$(Trim(Mill1), I, 1) = "(" Or Mid$(Trim(Mill1), I, 1) = ")" Or Mid$(Trim(Mill1), I, 1) = "\" Or Mid$(Trim(Mill1), I, 1) = "[" Or Mid$(Trim(Mill1), I, 1) = "]" Or Mid$(Trim(Mill1), I, 1) = "{" Or Mid$(Trim(Mill1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 20
                            Mill2 = Microsoft.VisualBasic.Right(Trim(Mill1), Len(Mill1) - I)
                            Mill1 = Microsoft.VisualBasic.Left(Trim(Mill1), I - 1)
                        End If

                        Ends1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("EndsCount_Name").ToString)
                        Ends2 = ""
                        If Len(Ends1) > 16 Then
                            For I = 16 To 1 Step -1
                                If Mid$(Trim(Ends1), I, 1) = " " Or Mid$(Trim(Ends1), I, 1) = "," Or Mid$(Trim(Ends1), I, 1) = "." Or Mid$(Trim(Ends1), I, 1) = "-" Or Mid$(Trim(Ends1), I, 1) = "/" Or Mid$(Trim(Ends1), I, 1) = "_" Or Mid$(Trim(Ends1), I, 1) = "(" Or Mid$(Trim(Ends1), I, 1) = ")" Or Mid$(Trim(Ends1), I, 1) = "\" Or Mid$(Trim(Ends1), I, 1) = "[" Or Mid$(Trim(Ends1), I, 1) = "]" Or Mid$(Trim(Ends1), I, 1) = "{" Or Mid$(Trim(Ends1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 16
                            Ends2 = Microsoft.VisualBasic.Right(Trim(Ends1), Len(Ends1) - I)
                            Ends1 = Microsoft.VisualBasic.Left(Trim(Ends1), I - 1)
                        End If

                        Clr1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Colour_Name").ToString)
                        Clr2 = ""
                        If Len(Clr1) > 12 Then
                            For I = 10 To 1 Step -1
                                If Mid$(Trim(Clr1), I, 1) = " " Or Mid$(Trim(Clr1), I, 1) = "," Or Mid$(Trim(Clr1), I, 1) = "." Or Mid$(Trim(Clr1), I, 1) = "-" Or Mid$(Trim(Clr1), I, 1) = "/" Or Mid$(Trim(Clr1), I, 1) = "_" Or Mid$(Trim(Clr1), I, 1) = "(" Or Mid$(Trim(Clr1), I, 1) = ")" Or Mid$(Trim(Clr1), I, 1) = "\" Or Mid$(Trim(Clr1), I, 1) = "[" Or Mid$(Trim(Clr1), I, 1) = "]" Or Mid$(Trim(Clr1), I, 1) = "{" Or Mid$(Trim(Clr1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 12
                            Clr2 = Microsoft.VisualBasic.Right(Trim(Clr1), Len(Clr1) - I)
                            Clr1 = Microsoft.VisualBasic.Left(Trim(Clr1), I - 1)
                        End If


                        CurY = CurY + TxtHgt
                        SNo = SNo + 1
                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetDt.Rows(prn_DetIndx).Item("Sl_No")).ToString), LMargin + ClArr(1) - 2, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(Ends1), LMargin + ClArr(1) + 5, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(Mill1), LMargin + ClArr(1) + ClArr(2) + 5, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(Clr1), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 5, CurY, 0, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Bobin_Size_Name").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 5, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Bobins").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 5, CurY, 1, 0, pFont)
                        If Trim(prn_HdDt.Rows(0).Item("Rate_For").ToString) = "REEL" Then
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Reel").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 5, CurY, 1, 0, pFont)
                        Else
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Meters").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 5, CurY, 1, 0, pFont)
                        End If
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Rate").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 5, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Amount").ToString), "#########0.00"), PageWidth - 5, CurY, 1, 0, pFont)


                        If Trim(Mill2) <> "" Or Trim(Ends2) <> "" Or Trim(Clr2) <> "" Then
                            CurY = CurY + TxtHgt - 5
                            If Trim(Ends2) <> "" Then
                                Common_Procedures.Print_To_PrintDocument(e, Trim(Ends2), LMargin + ClArr(1) + 5, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If
                            If Trim(Mill2) <> "" Then
                                Common_Procedures.Print_To_PrintDocument(e, Trim(Mill2), LMargin + ClArr(1) + ClArr(2) + 5, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If
                            If Trim(Clr2) <> "" Then
                                Common_Procedures.Print_To_PrintDocument(e, Trim(Clr2), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 5, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If
                        End If
                        NoofDets = NoofDets + 1
                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If

                Printing_FormatGST_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_FormatGST_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim strHeight As Single = 0, strWidth As Single = 0
        Dim C1 As Single, C2 As Single, W1 As Single, S1 As Single
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String, Cmp_PanNo As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_EMail As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim CurY1 As Single = 0, CurX As Single = 0
        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("SELECT BPH.*, CH.*, CSH.State_Name as Company_State_Name, LH.* , LSH.State_Name as Ledger_State_Name, LAH.Ledger_Name, LTH.Ledger_Name,LDH.Ledger_Name,CSH.State_Code as Company_State_Code,LSH.State_Code as Ledger_State_Code,LAH.Ledger_Name AS Agent_Name,LTH.Ledger_Name AS Transport_Name FROM BOBIN_PO_HEAD BPH INNER JOIN Company_Head CH ON BPH.Company_IdNo = CH.Company_IdNo INNER JOIN Ledger_Head LH ON BPH.Ledger_IdNo = LH.Ledger_IdNo LEFT OUTER JOIN State_Head CSH ON CH.Company_State_IdNo = CSH.State_IdNo LEFT OUTER JOIN State_Head LSH ON LH.Ledger_State_IdNo = LSH.State_IdNo  LEFT OUTER JOIN Ledger_Head LAH ON BPH.Agent_IdNo = LAH.Ledger_IdNo LEFT OUTER JOIN Ledger_Head LTH ON BPH.Transport_idno = LTH.Ledger_IdNo LEFT OUTER JOIN Ledger_Head LDH ON BPH.DeliveryTo_Idno = LDH.Ledger_IdNo WHERE BPH.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and BPH.Bobin_PO_Code = '" & Trim(EntryCode) & "' ORDER BY BPH.For_OrderBy, BPH.Bobin_PO_No", con)
        da2.Fill(dt2)

        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        dt2.Clear()

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_PanNo = "" : Cmp_EMail = ""
        Cmp_StateCap = "" : Cmp_StateNm = "" : Cmp_StateCode = "" : Cmp_GSTIN_Cap = "" : Cmp_GSTIN_No = ""

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
        Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString
        If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "PHONE NO.:" & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_PanNo").ToString) <> "" Then
            Cmp_PanNo = "PAN NO.: " & prn_HdDt.Rows(0).Item("Company_PanNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
            Cmp_EMail = "Email : " & prn_HdDt.Rows(0).Item("Company_EMail").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_State_Name").ToString) <> "" Then
            Cmp_StateCap = "STATE : "
            Cmp_StateNm = prn_HdDt.Rows(0).Item("Company_State_Name").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_State_Code").ToString) <> "" Then
            Cmp_StateCode = "CODE :" & prn_HdDt.Rows(0).Item("Company_State_Code").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString) <> "" Then
            Cmp_GSTIN_Cap = "GSTIN : "
            Cmp_GSTIN_No = prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
        End If


        CurY = CurY + TxtHgt - 15
        p1Font = New Font("Calibri", 18, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt

        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_GSTIN_Cap), p1Font).Width
        strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_StateNm & "     " & Cmp_GSTIN_Cap & Cmp_GSTIN_No), pFont).Width
        If PrintWidth > strWidth Then
            CurX = LMargin + (PrintWidth - strWidth) / 2
        Else
            CurX = LMargin
        End If

        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_StateCap, CurX, CurY, 0, 0, p1Font)
        strWidth = e.Graphics.MeasureString(Cmp_StateCap, p1Font).Width
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, Cmp_StateNm, CurX, CurY, 0, 0, pFont)

        strWidth = e.Graphics.MeasureString(Cmp_StateNm, pFont).Width
        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_GSTIN_Cap, CurX, CurY, 0, PrintWidth, p1Font)
        strWidth = e.Graphics.MeasureString("     " & Cmp_GSTIN_Cap, p1Font).Width
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, CurX, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PhNo & "   " & Cmp_EMail), LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "BOBIN PURCHASE RECEIPT (GST)", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try

            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5)
            C2 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5)
            W1 = e.Graphics.MeasureString("Reverse Charge (Y/N) : ", pFont).Width
            S1 = e.Graphics.MeasureString("TO       :    ", pFont).Width

            CurY = CurY + TxtHgt - 10
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "FROM :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "Purchase No", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Bobin_PO_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 14, FontStyle.Bold)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Purchase Date", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Bobin_PO_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, " GSTIN : " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            End If
            
            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + C1, LnAr(3), LMargin + C1, LnAr(2))

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "Agent Name", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Agent_Name").ToString, LMargin + W1 + 30, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Transport Name", LMargin + C2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Transport_Name").ToString, LMargin + C2 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY

            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin, CurY, 2, ClAr(1), p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "ENDS", LMargin + ClAr(1), CurY, 2, ClAr(2), p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "MILL NAME", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "COLOUR", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "BOBINS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), p1Font)
            If Trim(prn_HdDt.Rows(0).Item("Rate_For").ToString) = "REEL" Then
                Common_Procedures.Print_To_PrintDocument(e, "REELS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, +ClAr(6), p1Font)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, +ClAr(6), p1Font)
            End If
            Common_Procedures.Print_To_PrintDocument(e, "RATE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), p1Font)

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_FormatGST_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim I As Integer
        Dim Cmp_Name As String
        Dim C1 As Single, W1 As Single
        Dim vTaxPerc As Single = 0
        Dim BmsInWrds As String

        ' W1 = e.Graphics.MeasureString("No.of Beams  : ", pFont).Width

        Try

            For I = NoofDets + 1 To NoofItems_PerPage

                CurY = CurY + TxtHgt

                prn_DetIndx = prn_DetIndx + 1

            Next

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 30, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "" & Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Total_Reel").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Total_Amount").ToString), PageWidth - 10, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(7) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(4))


            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5)
            W1 = e.Graphics.MeasureString("DISCOUNT    : ", pFont).Width

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "Payment Terms", LMargin + 5, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Payment_Terms").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 1, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Freight", LMargin + C1 + 10, CurY, 0, 0, pFont)
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Freight_Charge").ToString), PageWidth - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt + 5
            Common_Procedures.Print_To_PrintDocument(e, "Delivery Terms", LMargin + 5, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Delivery_Terms").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 1, 0, pFont)

            'CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Excise Terms", LMargin + C1 + 10, CurY, 0, 0, pFont)
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Exercise_Terms").ToString), PageWidth - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, PageWidth, CurY)

            'CurY = CurY + TxtHgt - 10
            'If Val(prn_HdDt.Rows(0).Item("CGST_Amount").ToString) <> 0 Then
            '    Common_Procedures.Print_To_PrintDocument(e, "Add : CGST  @ " & Format(Val(txt_CGST_Percentage.Text), "##########0.0") & " %", LMargin + C1 + 10, CurY, 0, 0, pFont)
            '    If is_LastPage = True Then
            '        Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("CGST_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            '    End If

            'Else
            '    Common_Procedures.Print_To_PrintDocument(e, "Add : CGST  @ ", LMargin + C1 + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)

            'End If
            'CurY = CurY + TxtHgt

            'If Val(prn_HdDt.Rows(0).Item("SGST_Amount").ToString) <> 0 Then
            '    Common_Procedures.Print_To_PrintDocument(e, "Add : SGST  @ " & Format(Val(txt_SGST_Percentage.Text), "##########0.0") & " %", LMargin + C1 + 10, CurY, 0, 0, pFont)
            '    If is_LastPage = True Then
            '        Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("SGST_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            '    End If

            'Else
            '    Common_Procedures.Print_To_PrintDocument(e, "Add : SGST  @ ", LMargin + C1 + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)
            'End If

            'CurY = CurY + TxtHgt
            'If Val(prn_HdDt.Rows(0).Item("IGST_Amount").ToString) <> 0 Then
            '    Common_Procedures.Print_To_PrintDocument(e, "Add : IGST  @ " & Format(Val(txt_Igst_Perc.Text), "##########0.0") & " %", LMargin + C1 + 10, CurY, 0, 0, pFont)
            '    If is_LastPage = True Then
            '        Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("IGST_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            '    End If

            'Else
            '    Common_Procedures.Print_To_PrintDocument(e, "Add : IGST  @ ", LMargin + C1 + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)
            'End If

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, PageWidth, CurY)
            LnAr(8) = CurY

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "E & O.E", LMargin + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Net Amount", LMargin + C1 + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("Net_Amount").ToString), PageWidth - 10, CurY, 1, 0, p1Font)

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, PageWidth, CurY)
            LnAr(8) = CurY

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(9) = CurY
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(4))
            CurY = CurY + TxtHgt - 10
            BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))
            BmsInWrds = Replace(Trim(UCase(BmsInWrds)), "", "")

            Common_Procedures.Print_To_PrintDocument(e, "RUPEES            : " & BmsInWrds & " ", LMargin + 5, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            'CurY = CurY + 10
            'p1Font = New Font("Calibri", 12, FontStyle.Underline)
            'Common_Procedures.Print_To_PrintDocument(e, "Term & Conditions : ", LMargin + 10, CurY, 0, 0, p1Font)

            'CurY = CurY + TxtHgt + 10
            'Common_Procedures.Print_To_PrintDocument(e, "1. We are responsible for the quality of yarn only;If any running fault or quality  ", LMargin + 10, CurY, 0, 0, pFont)

            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "   defect noted in yarn please inform with firat fabric roll at once.We will", LMargin + 10, CurY, 0, 0, pFont)
            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "   accept only one roll at defect otherwise we do not hold ourself responsible. ", LMargin + 10, CurY, 0, 0, pFont)

            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "2. Our responsibility ceases when goods leave our permises.", LMargin + 10, CurY, 0, 0, pFont)

            'CurY = CurY + TxtHgt
            'If Val(Common_Procedures.User.IdNo) <> 1 Then
            '    Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
            'End If

            CurY = CurY + TxtHgt - 10
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt + 10
            Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 20, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 250, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "AUTHORISED SIGNATORY ", PageWidth - 5, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub btn_UserModification_Click(sender As Object, e As System.EventArgs) Handles btn_UserModification.Click
        If Val(Common_Procedures.User.IdNo) = 1 Then
            Dim f1 As New User_Modifications
            f1.Entry_Name = Me.Name
            f1.Entry_PkValue = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_OrderNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            f1.ShowDialog()
        End If
    End Sub
End Class