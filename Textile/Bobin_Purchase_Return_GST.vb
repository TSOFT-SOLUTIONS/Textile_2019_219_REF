Public Class Bobin_Purchase_Return_GST

    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "GBPRT-"
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
        cbo_PurchaseAccount.Text = ""
        cbo_TransportName.Text = ""
        txt_Freight.Text = ""
        txt_Narration.Text = ""
        txt_CommBag.Text = ""
        txt_CommBag.Text = ""
        txt_Billamount.Text = ""
        txt_BillNo.Text = ""
        txt_Accesablevalue.Text = ""
        txt_TaxPerc.Text = ""
        txt_AddLess.Text = ""
        cbo_Agent.Text = ""

        cbo_vataccount.Text = ""
        cbo_rateFor.Text = "MTRS"
        txt_BillDate.Text = ""
        lbl_GrossAmount.Text = ""
        txt_SGST_Percentage.Text = ""
        txt_Igst_Perc.Text = ""
        txt_CGST_Percentage.Text = ""
        lbl_CGST_Amount.Text = ""
        lbl_IGSTAmount.Text = ""
        lbl_SGST_Amount.Text = ""
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



            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name ,c.Ledger_Name as Transport_Name,d.Ledger_Name as Agent_Name , e.Ledger_Name as VatAC_Name,f.Ledger_Name as PurAc_Name ,g.Ledger_Name as Delv_Name  from Bobin_Purchase_Return_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Ledger_Head c ON a.Transport_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Ledger_Head d ON a.Agent_IdNo = d.Ledger_IdNo LEFT OUTER JOIN Ledger_Head e ON a.VatAc_IdNo = e.Ledger_IdNo LEFT OUTER JOIN Ledger_Head f ON a.PurchaseAc_IdNo = f.Ledger_IdNo LEFT OUTER JOIN Ledger_Head g ON a.DeliveryTo_Idno = g.Ledger_IdNo Where a.Bobin_Purchase_Return_Code = '" & Trim(NewCode) & "'", con)
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then
                lbl_OrderNo.Text = dt1.Rows(0).Item("Bobin_Purchase_Return_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Bobin_Purchase_Return_Date").ToString
                msk_Date.Text = dtp_Date.Text
                cbo_Ledger.Text = dt1.Rows(0).Item("Ledger_Name").ToString
                cbo_PurchaseAccount.Text = dt1.Rows(0).Item("PurAc_Name").ToString

                cbo_Agent.Text = dt1.Rows(0).Item("Agent_Name").ToString
                txt_CommBag.Text = dt1.Rows(0).Item("Agent_Commission_Bag").ToString
                txt_AddLess.Text = dt1.Rows(0).Item("AddLess_Amount").ToString
                txt_BillNo.Text = dt1.Rows(0).Item("Party_Bill_No").ToString
                txt_BillDate.Text = dt1.Rows(0).Item("PARTY_BILL_DATE").ToString
                lbl_GrossAmount.Text = Format(Val(dt1.Rows(0).Item("Gross_Amount").ToString), "########0.00")
                txt_Billamount.Text = dt1.Rows(0).Item("Net_Amount").ToString
                txt_Accesablevalue.Text = dt1.Rows(0).Item("Access_Value").ToString
                cbo_TransportName.Text = dt1.Rows(0).Item("Transport_Name").ToString
                cbo_TaxType.Text = dt1.Rows(0).Item("Tax_Type").ToString
                txt_TaxPerc.Text = Format(Val(dt1.Rows(0).Item("Tax_Perc").ToString), "########0.00")
                lbl_TaxAmount.Text = Format(Val(dt1.Rows(0).Item("Tax_Amount").ToString), "########0.00")
                cbo_vataccount.Text = dt1.Rows(0).Item("VatAc_Name").ToString
                txt_Freight.Text = Format(Val(dt1.Rows(0).Item("Freight_Charge").ToString), "########0.00")
                txt_Narration.Text = dt1.Rows(0).Item("Narration").ToString
                txt_CGST_Percentage.Text = dt1.Rows(0).Item("CGST_Percentage").ToString
                lbl_CGST_Amount.Text = dt1.Rows(0).Item("CGST_Amount").ToString
                txt_SGST_Percentage.Text = dt1.Rows(0).Item("SGST_Percentage").ToString
                lbl_SGST_Amount.Text = dt1.Rows(0).Item("SGST_Amount").ToString
                txt_Igst_Perc.Text = dt1.Rows(0).Item("IGST_Percentage").ToString
                lbl_IGSTAmount.Text = dt1.Rows(0).Item("IGST_Amount").ToString
                cbo_rateFor.Text = dt1.Rows(0).Item("Rate_For").ToString

                da2 = New SqlClient.SqlDataAdapter("select a.*, B.Colour_Name  from Bobin_Purchase_Return_Details a LEFT OUTER JOIN Colour_Head b ON a.Colour_IdNo = b.Colour_IdNo where a.Bobin_Purchase_Return_Code = '" & Trim(NewCode) & "' Order by Sl_No", con)
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
                        dgv_Details.Rows(n).Cells(DgvCol_BobinDetails.NofReel).Value = Val(dt2.Rows(i).Item("reel").ToString)
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

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_TransportName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
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

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_vataccount.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_vataccount.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_PurchaseAccount.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_PurchaseAccount.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

        da = New SqlClient.SqlDataAdapter("select a.Ledger_DisplayName from Ledger_AlaisHead a, ledger_head b where b.AccountsGroup_IdNo = 12 and a.Ledger_IdNo = b.Ledger_IdNo order by a.Ledger_DisplayName", con)
        da.Fill(dt3)
        cbo_vataccount.DataSource = dt3
        cbo_vataccount.DisplayMember = "Ledger_DisplayName"

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

        da = New SqlClient.SqlDataAdapter("select a.Ledger_DisplayName from Ledger_AlaisHead a, ledger_head b where b.AccountsGroup_IdNo = 27 and a.Ledger_IdNo = b.Ledger_IdNo order by a.Ledger_DisplayName", con)
        da.Fill(dt7)
        cbo_PurchaseAccount.DataSource = dt7
        cbo_PurchaseAccount.DisplayMember = "Ledger_DisplayName"

        da = New SqlClient.SqlDataAdapter("select Colour_Name from Colour_Head order by Colour_Name", con)
        da.Fill(dt10)
        cbo_Colour.DataSource = dt10
        cbo_Colour.DisplayMember = "Colour_Name"

        da = New SqlClient.SqlDataAdapter("select Bobin_Size_Name from Bobin_Size_Head order by Bobin_Size_Name", con)
        da.Fill(dt11)
        cbo_BobinSize.DataSource = dt11
        cbo_BobinSize.DisplayMember = "Bobin_Size_Name"

        cbo_TaxType.Items.Clear()
        cbo_TaxType.Items.Add("-NIL-")
        cbo_TaxType.Items.Add("VAT")
        cbo_TaxType.Items.Add("CST")

        cbo_rateFor.Items.Clear()
        cbo_rateFor.Items.Add("")
        cbo_rateFor.Items.Add("MTRS")
        cbo_rateFor.Items.Add("REEL")

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

        AddHandler cbo_PurchaseAccount.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_TransportName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_vataccount.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_rateFor.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_TaxType.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_filter_billNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_SGST_Percentage.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_CGST_Percentage.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Igst_Perc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Accesablevalue.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Billamount.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_BillNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_BillDate.GotFocus, AddressOf ControlGotFocus
        AddHandler lbl_GrossAmount.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_CommBag.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Freight.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Narration.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_TaxPerc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_AddLess.GotFocus, AddressOf ControlGotFocus

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
        AddHandler cbo_rateFor.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_PurchaseAccount.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_TransportName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_vataccount.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_TaxType.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_filter_billNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Accesablevalue.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Billamount.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_BillDate.LostFocus, AddressOf ControlLostFocus
        AddHandler lbl_GrossAmount.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_BillNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_CommBag.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Freight.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Narration.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_TaxPerc.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_AddLess.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Igst_Perc.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_SGST_Percentage.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_CGST_Percentage.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_Filter_DelvAt.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus

        AddHandler dtp_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Accesablevalue.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Billamount.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler msk_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_BillDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler lbl_GrossAmount.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_AddLess.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Freight.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_TaxPerc.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_filter_billNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_CGST_Percentage.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_SGST_Percentage.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Igst_Perc.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler msk_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Accesablevalue.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Billamount.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_BillDate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler lbl_GrossAmount.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_BillNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_AddLess.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Freight.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler txt_TaxPerc.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_filter_billNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_CGST_Percentage.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_SGST_Percentage.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Igst_Perc.KeyPress, AddressOf TextBoxControlKeyPress

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
                                txt_BillNo.Focus()

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
                                txt_AddLess.Focus()

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

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Bobin_Purchase_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Bobin_Purchase_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

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
            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "DELETE", "Bobin_Purchase_Return_Head", "Bobin_Purchase_Return_Code", Val(lbl_Company.Tag), NewCode, lbl_OrderNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "", "", "Bobin_Purchase_Return_Code, Company_IdNo, for_OrderBy", trans)

            Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "DELETE", "Bobin_Purchase_Return_Details", "Bobin_Purchase_Return_Code", Val(lbl_Company.Tag), NewCode, lbl_OrderNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "EndsCount_Idno, Mill_IdNo, Colour_IdNo, Bobins, Bobin_Size_IdNo, Meter_Bobin, Meters, Meter_Reel, Reel, Rate, Amount", "Sl_No", "Bobin_Purchase_Return_Code, For_OrderBy, Company_IdNo, Bobin_Purchase_Return_No, Bobin_Purchase_Return_Date, Ledger_Idno", trans)

            If Common_Procedures.VoucherBill_Deletion(con, Trim(Pk_Condition) & Trim(NewCode), trans) = False Then
                Throw New ApplicationException("Error on Voucher Bill Deletion")
            End If

            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), trans)
            ' Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition2) & Trim(NewCode), trans)

            cmd.CommandText = "Delete from Stock_Pavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Empty_BeamBagCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Bobin_Purchase_Return_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Bobin_Purchase_Return_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Bobin_Purchase_Return_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Bobin_Purchase_Return_Code = '" & Trim(NewCode) & "'"
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

            da = New SqlClient.SqlDataAdapter("select top 1 Bobin_Purchase_Return_No from Bobin_Purchase_Return_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Bobin_Purchase_Return_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Bobin_Purchase_Return_Code LIKE '" & Trim(Pk_Condition) & "%' Order by for_Orderby, Bobin_Purchase_Return_No", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 Bobin_Purchase_Return_No from Bobin_Purchase_Return_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Bobin_Purchase_Return_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Bobin_Purchase_Return_Code LIKE '" & Trim(Pk_Condition) & "%' Order by for_Orderby, Bobin_Purchase_Return_No", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 Bobin_Purchase_Return_No from Bobin_Purchase_Return_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Bobin_Purchase_Return_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Bobin_Purchase_Return_Code LIKE '" & Trim(Pk_Condition) & "%' Order by for_Orderby desc, Bobin_Purchase_Return_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 Bobin_Purchase_Return_No from Bobin_Purchase_Return_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Bobin_Purchase_Return_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Bobin_Purchase_Return_Code LIKE '" & Trim(Pk_Condition) & "%' Order by for_Orderby desc, Bobin_Purchase_Return_No desc", con)
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

            lbl_OrderNo.Text = Common_Procedures.get_MaxCode(con, "Bobin_Purchase_Return_Head", "Bobin_Purchase_Return_Code", "For_OrderBy", "( Bobin_Purchase_Return_Code LIKE '" & Trim(Pk_Condition) & "%' )", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

            lbl_OrderNo.ForeColor = Color.Red

            msk_Date.Text = Date.Today.ToShortDateString
            da = New SqlClient.SqlDataAdapter("select top 1 * from Bobin_Purchase_Return_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Bobin_Purchase_Return_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "'  and Bobin_Purchase_Return_Code LIKE '" & Trim(Pk_Condition) & "%' Order by for_Orderby desc, Bobin_Purchase_Return_No desc", con)
            dt1 = New DataTable
            da.Fill(dt1)
            If dt1.Rows.Count > 0 Then
                If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)
                    If dt1.Rows(0).Item("Bobin_Purchase_Return_Date").ToString <> "" Then msk_Date.Text = dt1.Rows(0).Item("Bobin_Purchase_Return_Date").ToString
                End If
                txt_CGST_Percentage.Text = dt1.Rows(0).Item("CGST_Percentage").ToString
                txt_SGST_Percentage.Text = dt1.Rows(0).Item("SGST_Percentage").ToString
                txt_Igst_Perc.Text = dt1.Rows(0).Item("IGST_Percentage").ToString
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

            Da = New SqlClient.SqlDataAdapter("select Bobin_Purchase_Return_No from Bobin_Purchase_Return_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Bobin_Purchase_Return_Code = '" & Trim(RecCode) & "'", con)
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

            Da = New SqlClient.SqlDataAdapter("select Bobin_Purchase_Return_No from Bobin_Purchase_Return_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Bobin_Purchase_Return_Code = '" & Trim(RecCode) & "'", con)
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

        ' If Common_Procedures.UserRight_Check(Common_Procedures.UR.Bobin_Purchase_Entry, New_Entry) = False Then Exit Sub

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
        PurAc_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PurchaseAccount.Text)

        TxAc_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_vataccount.Text)

        Delv_ID = Led_ID
        Rec_Id = Val(Common_Procedures.CommonLedger.Godown_Ac)


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

        vatac_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_vataccount.Text)
        'If vatac_id = 0 Then
        '    MessageBox.Show("Invalid Vat A/c", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    If cbo_vataccount.Enabled Then cbo_vataccount.Focus()
        '    Exit Sub
        'End If


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

                lbl_OrderNo.Text = Common_Procedures.get_MaxCode(con, "Bobin_Purchase_Return_Head", "Bobin_Purchase_Return_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_OrderNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@PurchaseDate", dtp_Date.Value.Date)

            If New_Entry = True Then
                cmd.CommandText = "Insert into Bobin_Purchase_Return_Head (Bobin_Purchase_Return_Code, Company_IdNo                     , Bobin_Purchase_Return_No               , for_OrderBy                                                              , Bobin_Purchase_Return_Date, Ledger_IdNo             , PurchaseAc_Idno       , DeliveryTo_Idno      , Agent_Idno         , Agent_Commission_Bag          , Party_Bill_No                   , PARTY_BILL_DATE                   , Gross_Amount                      , Net_Amount                       , Access_Value                         , VatAc_Idno            , Tax_Type                        , Tax_Perc                     , Tax_Amount                      , AddLess_Amount                , Transport_IdNo         , Freight_Charge                    , Narration                          , Total_Bobin          , Total_Meters          , Total_Amount        , CGST_Percentage                            , CGST_Amount                            , SGST_Percentage                           , SGST_Amount                           , IGST_Percentage                    , IGST_Amount                          , Rate_For                       , Total_Reel            ) " & _
                "Values                                           ('" & Trim(NewCode) & "'   , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_OrderNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_OrderNo.Text))) & ", @PurchaseDate      , " & Str(Val(Led_ID)) & ", " & Val(PurAc_id) & " , " & Val(DelT_Id) & " , " & Val(Ag_Id) & " , " & Val(txt_CommBag.Text) & " , '" & Trim(txt_BillNo.Text) & "' , '" & Trim(txt_BillDate.Text) & "' , " & Val(lbl_GrossAmount.Text) & " , " & Val(txt_Billamount.Text) & " , " & Val(txt_Accesablevalue.Text) & " , " & Val(vatac_id) & " , '" & Trim(cbo_TaxType.Text) & "', " & Val(txt_TaxPerc.Text) & ", " & Val(lbl_TaxAmount.Text) & " , " & Val(txt_AddLess.Text) & " , " & Str(Val(Tr_ID)) & ", " & Str(Val(txt_Freight.Text)) & ",  '" & Trim(txt_Narration.Text) & "', " & Val(vTotBns) & " , " & Val(vTotMtrs) & " , " & Val(vTotamt) & ", " & Str(Val(txt_CGST_Percentage.Text)) & " , " & Str(Val(lbl_CGST_Amount.Text)) & " , " & Str(Val(txt_SGST_Percentage.Text)) & ", " & Str(Val(lbl_SGST_Amount.Text)) & "," & Str(Val(txt_Igst_Perc.Text)) & ", " & Str(Val(lbl_IGSTAmount.Text)) & ",'" & Trim(cbo_rateFor.Text) & "', " & Val(vTotReel) & " )"
                cmd.ExecuteNonQuery()

            Else
                Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "OLD", "Bobin_Purchase_Return_Head", "Bobin_Purchase_Return_Code", Val(lbl_Company.Tag), NewCode, lbl_OrderNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Bobin_Purchase_Return_Code, Company_IdNo, for_OrderBy", tr)

                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "OLD", "Bobin_Purchase_Return_Details", "Bobin_Purchase_Return_Code", Val(lbl_Company.Tag), NewCode, lbl_OrderNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "EndsCount_Idno, Mill_IdNo, Colour_IdNo, Bobins, Bobin_Size_IdNo, Meter_Bobin, Meters, Meter_Reel, Reel, Rate, Amount", "Sl_No", "Bobin_Purchase_Return_Code, For_OrderBy, Company_IdNo, Bobin_Purchase_Return_No, Bobin_Purchase_Return_Date, Ledger_Idno", tr)

                cmd.CommandText = "Update Bobin_Purchase_Return_Head set Bobin_Purchase_Return_Date = @PurchaseDate, Ledger_IdNo = " & Val(Led_ID) & ", PurchaseAc_Idno = " & Val(PurAc_id) & " ,DeliveryTo_Idno = " & Val(DelT_Id) & ", Agent_Idno = " & Val(Ag_Id) & ",Agent_Commission_Bag = " & Val(txt_CommBag.Text) & ",AddLess_Amount = " & Val(txt_AddLess.Text) & ",Party_Bill_No = '" & Trim(txt_BillNo.Text) & "', PARTY_BILL_DATE ='" & Trim(txt_BillDate.Text) & "' ,Gross_Amount = " & Val(lbl_GrossAmount.Text) & ", Net_Amount = " & Val(txt_Billamount.Text) & " , Access_Value = " & Val(txt_Accesablevalue.Text) & ",VatAc_Idno = " & Val(vatac_id) & " ,Tax_Type ='" & Trim(cbo_TaxType.Text) & "', Tax_Perc = " & Val(txt_TaxPerc.Text) & ",Tax_Amount = " & Val(lbl_TaxAmount.Text) & " , Transport_IdNo = " & Val(Tr_ID) & ", Freight_Charge = " & Val(txt_Freight.Text) & ", Narration = '" & Trim(txt_Narration.Text) & "',Total_Bobin = " & Val(vTotBns) & "  ,Total_Meters = " & Val(vTotMtrs) & " ,Total_Amount = " & (vTotamt) & " , CGST_Percentage =" & Str(Val(txt_CGST_Percentage.Text)) & " ,CGST_Amount =" & Str(Val(lbl_CGST_Amount.Text)) & " ,SGST_Percentage =" & Str(Val(txt_SGST_Percentage.Text)) & ",SGST_Amount =" & Str(Val(lbl_SGST_Amount.Text)) & ",IGST_Percentage =" & Str(Val(txt_Igst_Perc.Text)) & ",IGST_Amount =" & Str(Val(lbl_IGSTAmount.Text)) & ", Rate_For = '" & Trim(cbo_rateFor.Text) & "',Total_reel = " & Val(vTotReel) & "  Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Bobin_Purchase_Return_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If
            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "NEW", "Bobin_Purchase_Return_Head", "Bobin_Purchase_Return_Code", Val(lbl_Company.Tag), NewCode, lbl_OrderNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Bobin_Purchase_Return_Code, Company_IdNo, for_OrderBy", tr)

          
            cmd.CommandText = "Delete from Bobin_Purchase_Return_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Bobin_Purchase_Return_Code = '" & Trim(NewCode) & "'"
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

                        Sno = Sno + 1

                        cmd.CommandText = "Insert into Bobin_Purchase_Return_Details(Bobin_Purchase_Return_Code, Company_IdNo                     , Bobin_Purchase_Return_No               , for_OrderBy                                                              , Bobin_Purchase_Return_Date, Sl_No                , EndsCount_Idno       , Mill_IdNo           , Colour_IdNo         , Bobins                                                            , Bobin_Size_IdNo            , Meter_Bobin                                                      , Meters                                                        , Meter_Reel                                                      , Reel                                                                , Rate                                                        , Amount                                                             ) " & _
                        "Values                                                     ('" & Trim(NewCode) & "'   , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_OrderNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_OrderNo.Text))) & ", @PurchaseDate      , " & Str(Val(Sno)) & ",  " & Val(Ends_ID) & ", " & Val(Mill_ID) & ", " & Val(Clr_ID) & " , " & Val(.Rows(i).Cells(DgvCol_BobinDetails.NoOfBobins).Value) & " ,  " & Val(BobinSize_ID) & " , " & Val(.Rows(i).Cells(DgvCol_BobinDetails.MeterBobin).Value) & ", " & Val(.Rows(i).Cells(DgvCol_BobinDetails.Meters).Value) & " , " & Val(.Rows(i).Cells(DgvCol_BobinDetails.MeterReel).Value) & ", " & Str(Val(.Rows(i).Cells(DgvCol_BobinDetails.NofReel).Value)) & " , " & Val(.Rows(i).Cells(DgvCol_BobinDetails.Rate).Value) & " , " & Str(Val(.Rows(i).Cells(DgvCol_BobinDetails.Amount).Value)) & " )"
                        cmd.ExecuteNonQuery()

                        cmd.CommandText = "Insert into Stock_Pavu_Processing_Details ( Reference_Code                              , Company_IdNo                      , Reference_No                    , for_OrderBy                                                              , Reference_Date, DeliveryTo_Idno , ReceivedFrom_Idno       , StockOf_IdNo, Entry_ID             , Party_Bill_No        , Particulars            , Sl_No                , EndsCount_IdNo           , Colour_IdNo             , Mill_idNo            , Meters_Bobin                                                           , Bobins                                                                 , Meters                                                             , Bobin_Size_IdNo                ) " & _
                        "Values                                                      ('" & Trim(Pk_Condition) & Trim(NewCode) & "' , " & Str(Val(lbl_Company.Tag)) & " , '" & Trim(lbl_OrderNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_OrderNo.Text))) & ", @PurchaseDate ,       0         , " & Str(Val(Rec_Id)) & ", 0           , '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', " & Str(Val(Sno)) & ", " & Str(Val(Ends_ID)) & ", " & Str(Val(Clr_ID)) & ", " & Val(Mill_ID) & " , " & Str(Val(.Rows(i).Cells(DgvCol_BobinDetails.MeterBobin).Value)) & " , " & Str(Val(.Rows(i).Cells(DgvCol_BobinDetails.NoOfBobins).Value)) & " , " & Str(Val(.Rows(i).Cells(DgvCol_BobinDetails.Meters).Value)) & " , " & Str(Val(BobinSize_ID)) & " )"
                        cmd.ExecuteNonQuery()

                    End If

                Next
                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "NEW", "Bobin_Purchase_Return_Details", "Bobin_Purchase_Return_Code", Val(lbl_Company.Tag), NewCode, lbl_OrderNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "EndsCount_Idno, Mill_IdNo, Colour_IdNo, Bobins, Bobin_Size_IdNo, Meter_Bobin, Meters, Meter_Reel, Reel, Rate, Amount", "Sl_No", "Bobin_Purchase_Return_Code, For_OrderBy, Company_IdNo, Bobin_Purchase_Return_No, Bobin_Purchase_Return_Date, Ledger_Idno", tr)

            End With

            If Val(vTotBns) <> 0 Or Val(vTotMtrs) <> 0 Then
                cmd.CommandText = "Insert into Stock_Empty_BeamBagCone_Processing_Details(Reference_Code                              , Company_IdNo                     , Reference_No                    , for_OrderBy                                                              , Reference_Date, DeliveryTo_Idno , ReceivedFrom_Idno       , Party_Bill_No        , Particulars            , Sl_No , Empty_Cones, Empty_Bobin              , EmptyBobin_Party, Empty_Jumbo) " & _
                "Values                                                                  ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_OrderNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_OrderNo.Text))) & ", @PurchaseDate , 0               , " & Str(Val(Rec_Id)) & ", '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', 1     , 0          , " & Str(Val(vTotBns)) & ", 0               , 0          )"
                cmd.ExecuteNonQuery()
            End If


            'AgentCommission Posting
            cmd.CommandText = "delete from AgentCommission_Processing_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            Dim vLed_IdNos As String = "", vVou_Amts As String = "", ErrMsg As String = ""
            If Val(CSng(txt_Billamount.Text)) <> 0 Then
                vLed_IdNos = Led_ID & "|" & PurAc_id & "|24|25|26"
                vVou_Amts = Val(CSng(txt_Billamount.Text)) & "|" & -1 * (Val(CSng(txt_Billamount.Text)) - Val(lbl_CGST_Amount.Text) - Val(lbl_SGST_Amount.Text) - Val(lbl_IGSTAmount.Text)) & "|" & -1 * Val(lbl_CGST_Amount.Text) & "|" & -1 * Val(lbl_SGST_Amount.Text) & "|" & -1 * Val(lbl_IGSTAmount.Text)
                If Common_Procedures.Voucher_Updation(con, "Bobin.Purc.RetGst", Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), Trim(lbl_OrderNo.Text), dtp_Date.Text, "Bill No : " & Trim(txt_BillNo.Text), vLed_IdNos, vVou_Amts, ErrMsg, tr, Common_Procedures.SoftwareTypes.Textile_Software) = False Then
                    Throw New ApplicationException(ErrMsg)
                End If
            End If

            'Bill Posting
            VouBil = Common_Procedures.VoucherBill_Posting(con, Val(lbl_Company.Tag), dtp_Date.Text, Led_ID, Trim(txt_BillNo.Text), Ag_Id, Val(CSng(txt_Billamount.Text)), "DR", Trim(Pk_Condition) & Trim(NewCode), tr, Common_Procedures.SoftwareTypes.Textile_Software)
            If Trim(UCase(VouBil)) = "ERROR" Then
                Throw New ApplicationException("Error on Voucher Bill Posting")
            End If

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

        lbl_GrossAmount.Text = Format(Val(vtotamt), "########0.00")

    End Sub

    Private Sub cbo_Ledger_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) )  or Show_In_All_Entry = 1 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Ledger_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, msk_Date, cbo_PurchaseAccount, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) )  or Show_In_All_Entry = 1 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Ledger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ledger.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Ledger, cbo_PurchaseAccount, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) )  or Show_In_All_Entry = 1 )", "(Ledger_idno = 0)")
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
                    If Trim(UCase(cbo_rateFor.Text)) <> "REEL" Then
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

                        .Rows(e.RowIndex).Cells(DgvCol_BobinDetails.NofReel).Value = Format((Val(.Rows(e.RowIndex).Cells(DgvCol_BobinDetails.Meters).Value) * Val(Ends)) / Val(.Rows(e.RowIndex).Cells(DgvCol_BobinDetails.MeterReel).Value), "#########0")
                    End If

                End If

                If e.ColumnIndex = DgvCol_BobinDetails.NofReel Or e.ColumnIndex = DgvCol_BobinDetails.Rate Then
                    If Trim(UCase(cbo_rateFor.Text)) = "REEL" Then
                        .Rows(e.RowIndex).Cells(DgvCol_BobinDetails.Amount).Value = Format(Val(.Rows(e.RowIndex).Cells(DgvCol_BobinDetails.NofReel).Value) * Val(.Rows(e.RowIndex).Cells(DgvCol_BobinDetails.Rate).Value), "############0.00")
                    End If
                End If

            End If

        End With
    End Sub

    Private Sub Amount_Calculation(ByVal CurRow As Integer, ByVal CurCol As Integer)
        On Error Resume Next

        'Try
        If FrmLdSTS = True Then Exit Sub
        With dgv_Details
            If .Visible Then
                If .Rows.Count > 0 Then
                    If CurCol = DgvCol_BobinDetails.Meters Or CurCol = DgvCol_BobinDetails.NofReel Or CurCol = DgvCol_BobinDetails.Rate Then
                        If Trim(UCase(cbo_rateFor.Text)) = "MTRS" Then
                            .Rows(CurRow).Cells(DgvCol_BobinDetails.Amount).Value = Format(Val(.Rows(CurRow).Cells(DgvCol_BobinDetails.Meters).Value) * Val(.Rows(CurRow).Cells(DgvCol_BobinDetails.Rate).Value), "###########0.00")
                        Else
                            .Rows(CurRow).Cells(DgvCol_BobinDetails.Amount).Value = Format(Val(.Rows(CurRow).Cells(DgvCol_BobinDetails.NofReel).Value) * Val(.Rows(CurRow).Cells(DgvCol_BobinDetails.Rate).Value), "############0.00")
                        End If

                        Total_Calculation()

                    End If

                End If
            End If

        End With

        'Catch ex As Exception
        '    MessageBox.Show(ex.Message, "AMOUNT CALCULATION....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        'End Try


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
                    txt_CommBag.Focus()
                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentRow.Index - 1).Cells(.ColumnCount - 1)
                End If
            End If

            If (e.KeyValue = 40 And cbo_Ends.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                If .CurrentCell.RowIndex = .Rows.Count - 1 And Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(1).Value) = "" Then
                    txt_BillNo.Focus()

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
                    txt_BillNo.Focus()

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

    Private Sub cbo_vataccount_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_vataccount.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_vataccount, txt_AddLess, txt_Billamount, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_idno = 0 or AccountsGroup_IdNo = 12) ", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_vataccount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_vataccount.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_vataccount, txt_Billamount, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_idno = 0 or AccountsGroup_IdNo = 12) ", "(Ledger_idno = 0)")
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
                Condt = "a.Bobin_Purchase_Return_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Bobin_Purchase_Return_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Bobin_Purchase_Return_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
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

            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name,C.Ledger_Name as Delv_Name from Bobin_Purchase_Return_Head a INNER JOIN Ledger_Head b on a.Ledger_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Ledger_Head c ON a.DeliveryTo_Idno = c.Ledger_Idno where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Bobin_Purchase_Return_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Bobin_Purchase_Return_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()


                    dgv_Filter_Details.Rows(n).Cells(0).Value = dt2.Rows(i).Item("Bobin_Purchase_Return_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(1).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Bobin_Purchase_Return_Date").ToString), "dd-MM-yyyy")
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

    Private Sub cbo_TaxType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_TaxType.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_TaxType, txt_Accesablevalue, txt_TaxPerc, "", "", "", "")
    End Sub

    Private Sub cbo_TaxType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_TaxType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_TaxType, txt_TaxPerc, "", "", "", "")
    End Sub

    Private Sub cbo_TaxType_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_TaxType.TextChanged
        If Trim(UCase(cbo_TaxType.Text)) = "" Or Trim(UCase(cbo_TaxType.Text)) = "-NIL-" Then txt_TaxPerc.Text = ""
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


    End Sub





    Private Sub txt_Note_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Narration.KeyDown
        If e.KeyCode = 40 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                msk_Date.Focus()
            End If
        End If

        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_Note_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Narration.KeyPress
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
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Agent, cbo_PurchaseAccount, txt_CommBag, "Ledger_AlaisHead", "Ledger_DisplayName", " (ledger_idno = 0 or Ledger_Type = 'AGENT' )", "(Ledger_idno = 0)")

    End Sub

    Private Sub cbo_Agent_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Agent.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Agent, txt_CommBag, "Ledger_AlaisHead", "Ledger_DisplayName", " (ledger_idno = 0 or Ledger_Type = 'AGENT') ", "(Ledger_idno = 0)")

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
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_TransportName, txt_Billamount, txt_Freight, "Ledger_AlaisHead", "Ledger_DisplayName", " (ledger_idno = 0 or Ledger_Type = 'TRANSPORT' )", "(Ledger_idno = 0)")


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

    Private Sub cbo_Purchaseaccount_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PurchaseAccount.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PurchaseAccount, cbo_Ledger, cbo_Agent, "Ledger_AlaisHead", "Ledger_DisplayName", " (ledger_idno = 0 or AccountsGroup_IdNo = 27 )", "(Ledger_idno = 0)")

    End Sub

    Private Sub cbo_Purchaseaccount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PurchaseAccount.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PurchaseAccount, cbo_Agent, "Ledger_AlaisHead", "Ledger_DisplayName", " (ledger_idno = 0 or AccountsGroup_IdNo = 27) ", "(Ledger_idno = 0)")
    End Sub

    Private Sub txt_BillNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_BillNo.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(DgvCol_BobinDetails.Ends)

            Else
                cbo_rateFor.Focus()

            End If
        End If
    End Sub


    Private Sub txt_commBag_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_CommBag.KeyDown

        If e.KeyCode = 40 Then
            cbo_rateFor.Focus()
        End If
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_CommBag_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_CommBag.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
        If Asc(e.KeyChar) = 13 Then
            cbo_rateFor.Focus()
        End If
    End Sub

    Private Sub txt_VatAmount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_TaxPerc.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_TaxPerc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_TaxPerc.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_TaxPerc_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_TaxPerc.TextChanged
        lbl_TaxAmount.Text = Format(Val(txt_Accesablevalue.Text) * Val(txt_TaxPerc.Text) / 100, "########0.00")
    End Sub

    Private Sub txt_Billamount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Billamount.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_Accesablevalue_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Accesablevalue.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub cbo_vataccount_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_vataccount.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            Common_Procedures.UR.Ledr_Wea_Siz_Rw_Trans_JbWrk_Creation = Common_Procedures.UR.Ledger_Creation
            Common_Procedures.MDI_LedType = ""
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_vataccount.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_PurchaseAccount_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PurchaseAccount.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            Common_Procedures.UR.Ledr_Wea_Siz_Rw_Trans_JbWrk_Creation = Common_Procedures.UR.Ledger_Creation
            Common_Procedures.MDI_LedType = ""
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_PurchaseAccount.Name
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






    Private Sub txt_AddLess_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_AddLess.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
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

    Private Sub txt_SGST_Percentage_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_SGST_Percentage.TextChanged

        Amount_Calculation()
    End Sub

    Private Sub txt_CGST_Percentage_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_CGST_Percentage.TextChanged


        Amount_Calculation()
    End Sub

    Private Sub txt_Igst_Perc_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Igst_Perc.TextChanged



        Amount_Calculation()
    End Sub

    Private Sub Amount_Calculation()
        Dim billamt As Double = 0

        txt_Accesablevalue.Text = Val(lbl_GrossAmount.Text) + Val(txt_AddLess.Text)

        lbl_CGST_Amount.Text = Format(Val(txt_Accesablevalue.Text) * Val(txt_CGST_Percentage.Text) / 100, "##########0.00")
        lbl_SGST_Amount.Text = Format(Val(txt_Accesablevalue.Text) * Val(txt_SGST_Percentage.Text) / 100, "##########0.00")
        lbl_IGSTAmount.Text = Format(Val(txt_Accesablevalue.Text) * Val(txt_Igst_Perc.Text) / 100, "##########0.00")


        billamt = Format(Val(txt_Accesablevalue.Text) + Val(lbl_CGST_Amount.Text) + Val(lbl_SGST_Amount.Text) + Val(lbl_IGSTAmount.Text), "###########0")

        txt_Billamount.Text = Format(billamt, "############0.00")

    End Sub

    Private Sub txt_AddLess_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_AddLess.TextChanged
        Amount_Calculation()
    End Sub

    Private Sub lbl_GrossAmount_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbl_GrossAmount.TextChanged
        Amount_Calculation()
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

    Private Sub cbo_rateFor_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_rateFor.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_rateFor, txt_CommBag, Nothing, "", "", "", "")
        If (e.KeyValue = 40 And cbo_rateFor.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(DgvCol_BobinDetails.Ends)

            Else
                txt_BillNo.Focus()

            End If
        End If
    End Sub

    Private Sub cbo_rateFor_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_rateFor.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_rateFor, Nothing, "", "", "", "")
        If Asc(e.KeyChar) = 13 Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(DgvCol_BobinDetails.Ends)

            Else
                txt_BillNo.Focus()

            End If
        End If
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
                    txt_BillNo.Focus()
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
                    txt_BillNo.Focus()
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
            If cbo_BobinSize.Visible Then
                With dgv_Details
                    If Val(cbo_BobinSize.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = DgvCol_BobinDetails.BobinSize Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_BobinSize.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_rateFor_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_rateFor.TextChanged
        Amount_Calculation()
        Total_Calculation()
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