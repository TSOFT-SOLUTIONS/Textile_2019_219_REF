Public Class Proforma_Bobin_Sales_GST
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "GBINV-"
    Private NoCalc_Status As Boolean = False
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl
    Private Print_PDF_Status As Boolean = False

    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
    Private prn_DetAr(50, 10) As String
    Private prn_DetMxIndx As Integer
    Private prn_NoofBmDets As Integer
    Private prn_DetSNo As Integer
    Private prn_Status As Integer

    Private dgv_LevColNo As Integer
    Public vmskOldText As String = ""
    Public vmskSelStrt As Integer = -1
    Public vmskOrdText As String = ""
    Public vmskOrdStrt As Integer = -1


    Public Sub New()
        ' This call is required by the designer.
        FrmLdSTS = True
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        clear()
    End Sub

    Private Enum DGVCol_BobinDetails
        SNO '0
        DC_No '1
        DC_Date '2
        Ends '3
        Count '4
        Bobin_Size '5
        NoOfBobin '6
        Jumpo '7
        NoOfCones '8
        Meters '9
        Weight '10
        RateFor '11
        Rate '12
        Amount '13
        Delivery_Code '14
        Delvery_SNo '15
        Discount_Percentage '16
        Discount_Value '17
        Taxable_Amount '18
        GST_Percentage '19
        HSN_Code '20
    End Enum

    Private Sub clear()

        NoCalc_Status = True

        New_Entry = False
        Insert_Entry = False

        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        pnl_Selection.Visible = False
        pnl_Selection.Visible = False
        pnl_Tax.Visible = False
        Print_PDF_Status = False
        vmskOldText = ""
        vmskSelStrt = -1


        lbl_InvoiceNo.Text = ""
        lbl_InvoiceNo.ForeColor = Color.Black
        lbl_RecCode.Text = ""
        msk_Date.Text = ""
        dtp_Date.Text = ""
        cbo_Ledger.Text = ""
        cbo_SalesAc.Text = ""

        txt_LrNo.Text = ""
        dtp_LrDate.Text = ""
        cbo_Transport.Text = ""

        cbo_VatAc.Text = ""
        txt_Note.Text = ""

        lbl_GrossAmount.Text = ""
        lbl_AssessableValue.Text = ""

        txt_DiscPerc.Text = ""
        lbl_DiscAmount.Text = ""
        cbo_TaxType.Text = "-NIL-"
        cbo_Type.Text = "DELIVERY"
        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))
        txt_TaxPerc.Text = ""
        lbl_TaxAmount.Text = ""

        txt_Packing.Text = ""
        txt_AddLess.Text = ""
        lbl_RoundOff.Text = ""
        lbl_NetAmount.Text = "0.00"
        lbl_AmountInWords.Text = "Rupees  :  "  ' "Amount In Words : "


        lbl_CGST_Amount.Text = ""
        lbl_SGST_Amount.Text = ""
        lbl_IGST_Amount.Text = ""
        lbl_TaxableValue.Text = ""
        txt_PoNo.Text = ""
        msk_PoDate.Text = ""

        dgv_Details.Rows.Clear()
        dgv_Details_Total.Rows.Clear()
        dgv_Details_Total.Rows.Add()

        dgv_Selection.Rows.Clear()

        If Filter_Status = False Then
            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()
        End If

        dgv_Details.Tag = ""
        dgv_LevColNo = -1
        ' cbo_GridEnds.Text = ""
        cbo_GridEnds.Visible = False
        cbo_GridEnds.Tag = -1
        'cbo_Grid_CountName.Text = ""
        cbo_Grid_CountName.Visible = False
        cbo_Grid_CountName.Tag = -1
        cbo_Grid_RateFor.Visible = False
        cbo_Grid_RateFor.Tag = -1
        Grid_Cell_DeSelect()
        cbo_Grid_BobinSize.Tag = -1
        cbo_Grid_BobinSize.Visible = False

        NoCalc_Status = False
        chk_NoAccountPosting.Checked = True
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

        NoCalc_Status = True

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name as PartyName from Proforma_BobinSales_Invoice_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.BobinSales_Invoice_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_InvoiceNo.Text = dt1.Rows(0).Item("BobinSales_Invoice_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("BobinDelivery_Invoice_Date").ToString
                msk_Date.Text = dtp_Date.Text
                cbo_Ledger.Text = dt1.Rows(0).Item("PartyName").ToString
                cbo_SalesAc.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("SalesAc_IdNo").ToString))
                txt_LrNo.Text = dt1.Rows(0).Item("Lr_No").ToString
                dtp_LrDate.Text = dt1.Rows(0).Item("Lr_Date").ToString
                cbo_Transport.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Transport_IdNo").ToString))
                cbo_Type.Text = dt1.Rows(0).Item("Selection_Type").ToString

                lbl_GrossAmount.Text = Format(Val(dt1.Rows(0).Item("Total_Amount").ToString), "#########0.00")
                txt_DiscPerc.Text = Val(dt1.Rows(0).Item("Discount_Percentage").ToString)
                lbl_DiscAmount.Text = Format(Val(dt1.Rows(0).Item("Discount_Amount").ToString), "#########0.00")
                lbl_AssessableValue.Text = Format(Val(dt1.Rows(0).Item("Assessable_Value").ToString), "#########0.00")
                cbo_TaxType.Text = dt1.Rows(0).Item("Tax_Type").ToString
                If Trim(cbo_TaxType.Text) = "" Then cbo_TaxType.Text = "-NIL-"
                txt_TaxPerc.Text = Val(dt1.Rows(0).Item("Tax_Percentage").ToString)
                lbl_TaxAmount.Text = Format(Val(dt1.Rows(0).Item("Tax_Amount").ToString), "#########0.00")

                txt_Packing.Text = Format(Val(dt1.Rows(0).Item("Packing_Amount").ToString), "#########0.00")
                txt_AddLess.Text = Format(Val(dt1.Rows(0).Item("AddLess_Amount").ToString), "#########0.00")
                lbl_RoundOff.Text = Format(Val(dt1.Rows(0).Item("RoundOff_Amount").ToString), "#########0.00")
                lbl_NetAmount.Text = Common_Procedures.Currency_Format(Val(dt1.Rows(0).Item("Net_Amount").ToString))

                cbo_VatAc.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("VatAc_IdNo").ToString))
                txt_Note.Text = dt1.Rows(0).Item("Note").ToString

                If Val(dt1.Rows(0).Item("NoAccountPosting").ToString) = 1 Then
                    chk_NoAccountPosting.Checked = True
                Else
                    chk_NoAccountPosting.Checked = False
                End If

                lbl_TaxableValue.Text = Format(Val(dt1.Rows(0).Item("Total_Taxable_Value").ToString), "#########0.00")
                lbl_CGST_Amount.Text = Format(Val(dt1.Rows(0).Item("Total_CGST_Amount").ToString), "#########0.00")
                lbl_SGST_Amount.Text = Format(Val(dt1.Rows(0).Item("Total_SGST_Amount").ToString), "#########0.00")
                lbl_IGST_Amount.Text = Format(Val(dt1.Rows(0).Item("Total_IGST_Amount").ToString), "#########0.00")

                txt_PoNo.Text = dt1.Rows(0).Item("Po_No").ToString
                dtp_PoDate.Text = dt1.Rows(0).Item("Po_Date").ToString
                msk_PoDate.Text = dtp_PoDate.Text

                lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))
                da2 = New SqlClient.SqlDataAdapter("Select a.* , b.EndsCount_Name  , c.Count_Name from Proforma_BobinSales_Invoice_Details  a LEFT OUTER JOIN EndsCount_Head b ON a.EndsCount_IdNo = b.EndsCount_IdNo LEFT OUTER JOIN Count_Head c ON a.Count_IdNo = c.Count_IdNo   Where a.BobinSales_Invoice_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                With dgv_Details

                    .Rows.Clear()
                    SNo = 0

                    If dt2.Rows.Count > 0 Then

                        For i = 0 To dt2.Rows.Count - 1

                            n = .Rows.Add()

                            SNo = SNo + 1
                            .Rows(n).Cells(DGVCol_BobinDetails.SNO).Value = Val(SNo)
                            .Rows(n).Cells(DGVCol_BobinDetails.DC_No).Value = dt2.Rows(i).Item("Bobin_Delivery_No").ToString
                            .Rows(n).Cells(DGVCol_BobinDetails.DC_Date).Value = dt2.Rows(i).Item("Bobin_Delivery_Date").ToString
                            .Rows(n).Cells(DGVCol_BobinDetails.Ends).Value = dt2.Rows(i).Item("EndsCount_Name").ToString
                            .Rows(n).Cells(DGVCol_BobinDetails.Count).Value = dt2.Rows(i).Item("count_name").ToString
                            .Rows(n).Cells(DGVCol_BobinDetails.Bobin_Size).Value = Common_Procedures.BobinSize_IdNoToName(con, dt2.Rows(i).Item("Bobin_Size_IdNo").ToString)

                            .Rows(n).Cells(DGVCol_BobinDetails.NoOfBobin).Value = Format(Val(dt2.Rows(i).Item("No_OF_Bobin").ToString), "########0")
                            .Rows(n).Cells(DGVCol_BobinDetails.Jumpo).Value = Format(Val(dt2.Rows(i).Item("No_OF_Jumpo").ToString), "########0")
                            .Rows(n).Cells(DGVCol_BobinDetails.NoOfCones).Value = Format(Val(dt2.Rows(i).Item("No_OF_Cones").ToString), "########0")

                            .Rows(n).Cells(DGVCol_BobinDetails.Meters).Value = Format(Val(dt2.Rows(i).Item("Meters").ToString), "########0.00")
                            .Rows(n).Cells(DGVCol_BobinDetails.Weight).Value = Format(Val(dt2.Rows(i).Item("Weights").ToString), "########0.000")
                            .Rows(n).Cells(DGVCol_BobinDetails.RateFor).Value = dt2.Rows(i).Item("Rate_For").ToString

                            .Rows(n).Cells(DGVCol_BobinDetails.Rate).Value = Format(Val(dt2.Rows(i).Item("Rate").ToString), "########0.00")
                            .Rows(n).Cells(DGVCol_BobinDetails.Amount).Value = Format(Val(dt2.Rows(i).Item("Amount").ToString), "########0.00")
                            .Rows(n).Cells(DGVCol_BobinDetails.Delivery_Code).Value = dt2.Rows(i).Item("Bobin_Jari_Delivery_Code").ToString

                            .Rows(n).Cells(DGVCol_BobinDetails.Discount_Percentage).Value = dt2.Rows(i).Item("Cash_Discount_Percentage").ToString
                            .Rows(n).Cells(DGVCol_BobinDetails.Discount_Value).Value = dt2.Rows(i).Item("Cash_Discount_Amount").ToString
                            .Rows(n).Cells(DGVCol_BobinDetails.Taxable_Amount).Value = dt2.Rows(i).Item("Taxable_Value").ToString
                            .Rows(n).Cells(DGVCol_BobinDetails.GST_Percentage).Value = dt2.Rows(i).Item("GST_Percentage").ToString
                            .Rows(n).Cells(DGVCol_BobinDetails.HSN_Code).Value = dt2.Rows(i).Item("HSN_Code").ToString
                        Next i

                    End If

                    If .RowCount = 0 Then .Rows.Add()

                End With

                With dgv_Details_Total
                    If .RowCount = 0 Then .Rows.Add()
                    .Rows(0).Cells(5).Value = Val(dt1.Rows(0).Item("Total_Bobin").ToString)
                    .Rows(0).Cells(6).Value = Val(dt1.Rows(0).Item("Total_Jumpo").ToString)
                    .Rows(0).Cells(7).Value = Val(dt1.Rows(0).Item("Total_Cones").ToString)

                    .Rows(0).Cells(8).Value = Format(Val(dt1.Rows(0).Item("Total_Meters").ToString), "########0.00")
                    .Rows(0).Cells(9).Value = Format(Val(dt1.Rows(0).Item("Total_Weights").ToString), "########0.000")

                    .Rows(0).Cells(12).Value = Format(Val(dt1.Rows(0).Item("Total_Amount").ToString), "########0.00")

                End With

                dt2.Dispose()
                da2.Dispose()

            End If

            dt1.Dispose()
            da1.Dispose()

            Grid_Cell_DeSelect()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        NoCalc_Status = False

        If msk_Date.Visible And msk_Date.Enabled Then msk_Date.Focus()

    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox
        Dim msktxbx As MaskedTextBox
        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Me.ActiveControl Is Button Or TypeOf Me.ActiveControl Is CheckBox Or TypeOf Me.ActiveControl Is MaskedTextBox Then
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
        'If Me.ActiveControl.Name <> dgv_Details.Name Then
        '    Common_Procedures.Hide_CurrentStock_Display()
        'End If
        If Me.ActiveControl.Name <> cbo_GridEnds.Name Then
            cbo_GridEnds.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Grid_CountName.Name Then
            cbo_Grid_CountName.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Grid_RateFor.Name Then
            cbo_Grid_RateFor.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Grid_BobinSize.Name Then
            cbo_Grid_BobinSize.Visible = False
        End If
        Grid_Cell_DeSelect()

        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        On Error Resume Next

        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Or TypeOf Prec_ActCtrl Is CheckBox Or TypeOf Prec_ActCtrl Is MaskedTextBox Then
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
        If e.KeyValue = 38 Then e.Handled = True : SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then e.Handled = True : SendKeys.Send("{TAB}")
    End Sub

    Private Sub TextBoxControlKeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        On Error Resume Next
        If Asc(e.KeyChar) = 13 Then e.Handled = True : SendKeys.Send("{TAB}")
    End Sub

    Private Sub Grid_Cell_DeSelect()
        On Error Resume Next
        dgv_Details.CurrentCell.Selected = False
        dgv_Details_Total.CurrentCell.Selected = False

        dgv_Selection.CurrentCell.Selected = False
        dgv_Filter_Details.CurrentCell.Selected = False
    End Sub

    Private Sub Bobin_Sales_Invoice_Entry_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Ledger.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Ledger.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_SalesAc.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_SalesAc.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_VatAc.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_VatAc.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Transport.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Transport.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_GridEnds.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "ENDSCOUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_GridEnds.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_CountName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "COUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_CountName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_BobinSize.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "BOBINSIZE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_BobinSize.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

    Private Sub Bobin_Sales_Invoice_Entry_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable

        Me.Text = ""

        con.Open()

        cbo_Grid_RateFor.Items.Clear()
        cbo_Grid_RateFor.Items.Add("MTRS")
        cbo_Grid_RateFor.Items.Add("KG")

        cbo_TaxType.Items.Clear()
        cbo_TaxType.Items.Add("-NIL-")
        cbo_TaxType.Items.Add("VAT")
        cbo_TaxType.Items.Add("CST")

        cbo_Type.Items.Clear()
        cbo_Type.Items.Add(" ")
        cbo_Type.Items.Add("DIRECT")
        cbo_Type.Items.Add("DELIVERY")

        'da = New SqlClient.SqlDataAdapter("select Ends_Name from Ends_Head order by Ends_Name", con)
        'da.Fill(dt1)
        'cbo_GridEnds.DataSource = dt1
        'cbo_GridEnds.DisplayMember = "Ends_Name"

        'da = New SqlClient.SqlDataAdapter("select Count_Name from Count_Head order by Count_Name", con)
        'da.Fill(dt2)
        'cbo_Grid_CountName.DataSource = dt2
        'cbo_Grid_CountName.DisplayMember = "Count_Name"

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2
        pnl_Filter.BringToFront()


        pnl_Selection.Visible = False
        pnl_Selection.Left = (Me.Width - pnl_Selection.Width) \ 2
        pnl_Selection.Top = (Me.Height - pnl_Selection.Height) \ 2
        pnl_Selection.BringToFront()


        pnl_Print.Visible = False
        pnl_Print.Left = (Me.Width - pnl_Print.Width) \ 2
        pnl_Print.Top = (Me.Height - pnl_Print.Height) \ 2
        pnl_Print.BringToFront()

        pnl_Tax.Visible = False
        pnl_Tax.Left = (Me.Width - pnl_Tax.Width) \ 2
        pnl_Tax.Top = ((Me.Height - pnl_Tax.Height) \ 2) - 100
        pnl_Tax.BringToFront()

        AddHandler msk_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Ledger.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_GridEnds.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_CountName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_RateFor.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_Type.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_SalesAc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_LrNo.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_LrDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Transport.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DiscPerc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_TaxPerc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Packing.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_AddLess.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_TaxType.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_VatAc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Note.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_Fromdate.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_ToDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus

        AddHandler btn_save.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_SendSMS.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_close.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print_VatBill.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print_NonVatBill.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print_PrePrint.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print_Cancel.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PoNo.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_PoDate.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_PoDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_BobinSize.GotFocus, AddressOf ControlGotFocus

        AddHandler msk_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Ledger.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_GridEnds.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_CountName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_RateFor.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_Type.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_SalesAc.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_LrNo.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_LrDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Transport.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DiscPerc.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_TaxType.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_TaxPerc.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Packing.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_AddLess.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_VatAc.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Note.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_Fromdate.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_ToDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus

        AddHandler btn_save.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_SendSMS.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_close.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print_VatBill.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print_NonVatBill.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print_Cancel.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print_PrePrint.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PoNo.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_PoDate.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_PoDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_BobinSize.LostFocus, AddressOf ControlLostFocus


        AddHandler msk_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_LrNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_LrDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_TaxPerc.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Packing.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_AddLess.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Note.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Note.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler msk_PoDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_PoDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_PoNo.KeyDown, AddressOf TextBoxControlKeyDown


        AddHandler msk_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_LrNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_LrDate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_DiscPerc.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_TaxPerc.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Packing.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_AddLess.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_Fromdate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_ToDate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Note.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler msk_PoDate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_PoDate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_PoNo.KeyPress, AddressOf TextBoxControlKeyPress

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True
        new_record()

    End Sub

    Private Sub Bobin_Sales_Invoice_Entry_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        'Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub Bobin_Sales_Invoice_Entry_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
                    Exit Sub

                    'ElseIf pnl_Selection.Visible = True Then
                    '    btn_Close_Selection_Click(sender, e)
                    '    Exit Sub

                ElseIf pnl_Print.Visible = True Then
                    btn_print_Close_Click(sender, e)
                    Exit Sub

                ElseIf pnl_Tax.Visible = True Then
                    btn_Tax_Close_Click(sender, e)
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
                        If .CurrentCell.ColumnIndex >= .ColumnCount - 9 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                txt_DiscPerc.Focus()

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(DGVCol_BobinDetails.DC_No)

                            End If

                        Else

                            If .CurrentCell.ColumnIndex >= 1 And .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = "" Then
                                txt_DiscPerc.Focus()
                            Else
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
                            End If

                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then

                        If .CurrentCell.ColumnIndex <= 1 Then
                            If .CurrentCell.RowIndex = 0 Then
                                cbo_Transport.Focus()

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(12)
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

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Invoice_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Invoice_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.Proforma_bobin_sales, New_Entry, Me, con, "Proforma_BobinSales_Invoice_Head", "BobinSales_Invoice_Code", NewCode, "BobinSales_Invoice_Date", "(BobinSales_Invoice_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub

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

            NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.Transaction = trans

            If Common_Procedures.VoucherBill_Deletion(con, Trim(NewCode), trans) = False Then
                Throw New ApplicationException("Error on Voucher Bill Deletion")
            End If

            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(NewCode), trans)

            cmd.CommandText = "Update Bobin_Jari_Delivery_Head set BobinSales_Invoice_Code = '', BobinSales_Invoice_Increment = BobinSales_Invoice_Increment - 1  Where BobinSales_Invoice_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Proforma_BobinSales_Invoice_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and BobinSales_Invoice_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "delete from Proforma_BobinSales_Invoice_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and BobinSales_Invoice_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Proforma_BobinSales_Invoice_GST_Tax_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and BobinSales_Invoice_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            'If Val(Common_Procedures.settings.NegativeStock_Restriction) = 1 Then

            '    If Common_Procedures.Check_is_Negative_Stock_Status(con, trans) = True Then Exit Sub

            'End If

            trans.Commit()

            Dt1.Dispose()
            Da.Dispose()
            trans.Dispose()
            cmd.Dispose()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception
            trans.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            If msk_Date.Visible And msk_Date.Enabled Then msk_Date.Focus()

        End Try

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        If Filter_Status = False Then

            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable

            da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or (Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) ) order by Ledger_DisplayName", con)
            da.Fill(dt1)
            cbo_Filter_PartyName.DataSource = dt1
            cbo_Filter_PartyName.DisplayMember = "Ledger_DisplayName"

            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()

        End If

        pnl_Filter.Visible = True
        pnl_Filter.Enabled = True
        pnl_Back.Enabled = False
        If dtp_Filter_Fromdate.Enabled And dtp_Filter_Fromdate.Visible Then dtp_Filter_Fromdate.Focus()

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String

        Try

            da = New SqlClient.SqlDataAdapter("select top 1 BobinSales_Invoice_No from Proforma_BobinSales_Invoice_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and BobinSales_Invoice_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Entry_VAT_GST_Type = 'GST' Order by for_Orderby, BobinSales_Invoice_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_InvoiceNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 BobinSales_Invoice_No from Proforma_BobinSales_Invoice_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and BobinSales_Invoice_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Entry_VAT_GST_Type = 'GST' Order by for_Orderby, BobinSales_Invoice_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_InvoiceNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 BobinSales_Invoice_No from Proforma_BobinSales_Invoice_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and BobinSales_Invoice_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Entry_VAT_GST_Type = 'GST' Order by for_Orderby desc, BobinSales_Invoice_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 BobinSales_Invoice_No from Proforma_BobinSales_Invoice_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and BobinSales_Invoice_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Entry_VAT_GST_Type = 'GST' Order by for_Orderby desc, BobinSales_Invoice_No desc", con)
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
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable

        Try
            clear()

            New_Entry = True

            lbl_InvoiceNo.Text = Common_Procedures.get_MaxCode(con, "Proforma_BobinSales_Invoice_Head", "BobinSales_Invoice_Code", "For_OrderBy", "Entry_VAT_GST_Type = 'GST'", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)
            lbl_InvoiceNo.ForeColor = Color.Red


            msk_Date.Text = Date.Today.ToShortDateString
            Da1 = New SqlClient.SqlDataAdapter("select Top 1 a.*, b.ledger_name as SalesAcName, c.ledger_name as TaxAcName from Proforma_BobinSales_Invoice_Head a LEFT OUTER JOIN Ledger_Head b ON a.SalesAc_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Ledger_Head c ON a.VatAc_IdNo = c.Ledger_IdNo where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.BobinSales_Invoice_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by a.for_Orderby desc, a.BobinSales_Invoice_No desc", con)
            Da1.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then
                If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)
                    If Dt1.Rows(0).Item("BobinSales_Invoice_Date").ToString <> "" Then msk_Date.Text = Dt1.Rows(0).Item("BobinSales_Invoice_Date").ToString
                End If
                If Dt1.Rows(0).Item("SalesAcName").ToString <> "" Then cbo_SalesAc.Text = Dt1.Rows(0).Item("SalesAcName").ToString
                If Dt1.Rows(0).Item("Tax_Type").ToString <> "" Then cbo_TaxType.Text = Dt1.Rows(0).Item("Tax_Type").ToString
                If Dt1.Rows(0).Item("Tax_Percentage").ToString <> "" Then txt_TaxPerc.Text = Val(Dt1.Rows(0).Item("Tax_Percentage").ToString)
                If Dt1.Rows(0).Item("TaxAcName").ToString <> "" Then cbo_VatAc.Text = Dt1.Rows(0).Item("TaxAcName").ToString
            End If

            Dt1.Clear()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            Dt1.Dispose()
            Da1.Dispose()

            If msk_Date.Enabled And msk_Date.Visible Then
                msk_Date.Focus()
                msk_Date.SelectionStart = 0
            End If


        End Try

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim InvCode As String

        Try

            inpno = InputBox("Enter Invoice No.", "FOR FINDING...")

            InvCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select BobinSales_Invoice_No from Proforma_BobinSales_Invoice_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and BobinSales_Invoice_Code = '" & Trim(InvCode) & "'", con)
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
                MessageBox.Show("Ivnvoice No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim InvCode As String

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Invoice_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Invoice_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.Proforma_bobin_sales, New_Entry, Me) = False Then Exit Sub

        Try

            inpno = InputBox("Enter New Invoice No.", "FOR NEW INVOICE NO. INSERTION...")

            InvCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select BobinSales_Invoice_No from Proforma_BobinSales_Invoice_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and BobinSales_Invoice_Code = '" & Trim(InvCode) & "'", con)
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
                    MessageBox.Show("Invalid Invoice No.", "DOES NOT INSERT NEW BILL...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_InvoiceNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT NEW BILL...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim Led_ID As Integer = 0
        Dim EndsCnt_ID As Integer = 0
        Dim Cnt_ID As Integer = 0

        Dim SalAc_ID As Integer = 0
        Dim Ends_ID As Integer = 0
        Dim PSalNm_ID As Integer = 0
        Dim Trans_ID As Integer

        Dim VatAc_ID As Integer = 0

        Dim Sno As Integer = 0
        Dim Partcls As String = ""
        Dim EntID As String = ""
        Dim Dup_FPname As String = ""
        Dim PBlNo As String = ""
        Dim vTotAmt As Single, vTotQty As Single, vTotMtrs As Single
        Dim vTotBbn As Single, vTotjmp As Single, vTotwgt As Single, vTotcns As Single
        Dim vBlsTotQty As Single, vBlsTotMtrs As Single
        Dim Nr As Long = 0
        Dim Cr_ID As Integer = 0
        Dim Dr_ID As Integer = 0
        Dim NtBl_STS As Integer = 0
        Dim noAccPost As Integer = 0
        Dim vGST_Tax_Inv_Sts As Integer = 0
        Dim vPurOrd As String = ""
        Dim BbnSz_Id As Integer = 0

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        ' If Common_Procedures.UserRight_Check(Common_Procedures.UR.Invoice_Entry, New_Entry) = False Then Exit Sub

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Proforma_bobin_sales, New_Entry, Me, con, "Proforma_BobinSales_Invoice_Head", "BobinSales_Invoice_Code", NewCode, "BobinSales_Invoice_Date", "(BobinSales_Invoice_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and BobinSales_Invoice_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, BobinSales_Invoice_No desc", dtp_Date.Value.Date) = False Then Exit Sub

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

        Trans_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Transport.Text)
        SalAc_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_SalesAc.Text)
        VatAc_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_VatAc.Text)
        lbl_UserName.Text = Common_Procedures.User.IdNo
        BbnSz_Id = Common_Procedures.BobinSize_NameToIdNo(con, cbo_Grid_BobinSize.Text)

        If SalAc_ID = 0 And Val(CSng(lbl_NetAmount.Text)) <> 0 Then
            MessageBox.Show("Invalid Sales A/c name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_SalesAc.Enabled And cbo_SalesAc.Visible Then cbo_SalesAc.Focus()
            Exit Sub
        End If

        noAccPost = 0
        If chk_NoAccountPosting.Checked = True Then noAccPost = 1

        With dgv_Details

            For i = 0 To .RowCount - 1

                If (Trim(.Rows(i).Cells(DGVCol_BobinDetails.Ends).Value) <> "" And Trim(.Rows(i).Cells(DGVCol_BobinDetails.Count).Value) <> "") Or (Val(.Rows(i).Cells(DGVCol_BobinDetails.NoOfCones).Value) <> 0 And Val(.Rows(i).Cells(DGVCol_BobinDetails.Meters).Value) <> 0) Then

                    Ends_ID = Common_Procedures.EndsCount_NameToIdNo(con, .Rows(i).Cells(DGVCol_BobinDetails.Ends).Value)
                    If Ends_ID = 0 Then
                        MessageBox.Show("Invalid EndsCount Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(DGVCol_BobinDetails.Ends)
                            If cbo_GridEnds.Visible And cbo_GridEnds.Enabled Then cbo_GridEnds.Focus()
                        End If
                        Exit Sub
                    End If

                    Cnt_ID = Common_Procedures.Count_NameToIdNo(con, .Rows(i).Cells(DGVCol_BobinDetails.Count).Value)
                    If Cnt_ID = 0 Then
                        MessageBox.Show("Invalid Count Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(DGVCol_BobinDetails.Count)
                            If cbo_Grid_CountName.Visible And cbo_Grid_CountName.Enabled Then cbo_Grid_CountName.Focus()
                        End If
                        Exit Sub
                    End If

                    'If Val(.Rows(i).Cells(7).Value) = 0 Then
                    '    MessageBox.Show("Invalid Meters", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    '    If .Enabled And .Visible Then
                    '        .Focus()
                    '        .CurrentCell = .Rows(i).Cells(7)
                    '    End If
                    '    Exit Sub
                    'End If
                    If Val(.Rows(i).Cells(DGVCol_BobinDetails.Weight).Value) = 0 Then
                        MessageBox.Show("Invalid Weight", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(DGVCol_BobinDetails.Weight)
                        End If
                        Exit Sub
                    End If

                    'FP_ID = Common_Procedures.Processed_Item_NameToIdNo(con, .Rows(i).Cells(1).Value)
                    'If FP_ID = 0 Then
                    '    MessageBox.Show("Invalid Finished Product Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    '    If .Enabled And .Visible Then
                    '        .Focus()
                    '        .CurrentCell = .Rows(i).Cells(1)
                    '        .CurrentCell.Selected = True
                    '    End If
                    '    Exit Sub
                    'End If

                    'If InStr(1, Trim(UCase(Dup_FPname)), "~" & Trim(UCase(.Rows(i).Cells(1).Value)) & "~") > 0 Then
                    '    MessageBox.Show("Duplicate FINISHED PRODUCT NAME ", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    '    If .Enabled And .Visible Then
                    '        .Focus()
                    '        .CurrentCell = .Rows(i).Cells(1)
                    '        .CurrentCell.Selected = True
                    '    End If
                    '    Exit Sub
                    'End If

                    'Dup_FPname = Trim(Dup_FPname) & "~" & Trim(UCase(.Rows(i).Cells(1).Value)) & "~"

                End If

            Next

        End With

        If VatAc_ID = 0 And Val(lbl_TaxAmount.Text) <> 0 Then
            MessageBox.Show("Invalid Vat A/c name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_VatAc.Enabled And cbo_VatAc.Visible Then cbo_VatAc.Focus()
            Exit Sub
        End If

        NoCalc_Status = False
        Total_Calculation()

        vTotAmt = 0 : vTotQty = 0 : vTotMtrs = 0
        vBlsTotQty = 0 : vBlsTotMtrs = 0
        vTotBbn = 0 : vTotjmp = 0 : vTotwgt = 0 : vTotcns = 0

        If dgv_Details_Total.RowCount > 0 Then

            vTotBbn = Val(dgv_Details_Total.Rows(0).Cells(5).Value())
            vTotjmp = Val(dgv_Details_Total.Rows(0).Cells(6).Value())
            vTotcns = Val(dgv_Details_Total.Rows(0).Cells(7).Value())

            vTotMtrs = Val(dgv_Details_Total.Rows(0).Cells(8).Value())
            vTotwgt = Val(dgv_Details_Total.Rows(0).Cells(9).Value())
            vTotAmt = Val(dgv_Details_Total.Rows(0).Cells(12).Value())

        End If

        'If Val(vTotQty) <> Val(vBlsTotQty) Or Val(vTotMtrs) <> Val(vBlsTotMtrs) Then
        '    MessageBox.Show("Mismatch of Quantity in Invoice and Bale Details", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
        '    Exit Sub
        'End If

        vPurOrd = ""
        If Trim(msk_PoDate.Text) <> "" Then
            If IsDate(msk_PoDate.Text) = True Then
                vPurOrd = Trim(msk_PoDate.Text)
            End If
        End If


        If Val(lbl_NetAmount.Text) = 0 Then lbl_NetAmount.Text = 0
        If Val(lbl_CGST_Amount.Text) = 0 Then lbl_CGST_Amount.Text = 0
        If Val(lbl_SGST_Amount.Text) = 0 Then lbl_SGST_Amount.Text = 0
        If Val(lbl_IGST_Amount.Text) = 0 Then lbl_IGST_Amount.Text = 0


        vGST_Tax_Inv_Sts = 0
        If chk_GSTTax_Invocie.Checked = True Then vGST_Tax_Inv_Sts = 1

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_InvoiceNo.Text = Common_Procedures.get_MaxCode(con, "Proforma_BobinSales_Invoice_Head", "BobinSales_Invoice_Code", "For_OrderBy", "Entry_VAT_GST_Type = 'GST'", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@InvoiceDate", dtp_Date.Value.Date)

            If New_Entry = True Then

                cmd.CommandText = "Insert into Proforma_BobinSales_Invoice_Head (Entry_VAT_GST_Type , BobinSales_Invoice_Code    ,               Company_IdNo       ,     BobinSales_Invoice_No    ,                     for_OrderBy                                            , BobinDelivery_Invoice_Date  ,          Ledger_IdNo    ,                   SalesAc_IdNo   ,           Lr_No              ,               Lr_Date          ,        Transport_IdNo     ,           Total_Meters     ,               Total_Amount            ,             Discount_Percentage    ,              Discount_Amount         ,              Assessable_Value    ,             Tax_Type            ,             Tax_Percentage        ,             Tax_Amount              ,           VatAc_IdNo      ,              Packing_Amount       ,              AddLess_Amount       ,               RoundOff_Amount      ,              Net_Amount             ,               Note           ,                   Selection_Type          , Total_Bobin               , Total_Jumpo                , Total_Cones                   ,    Total_Weights         ,        NoAccountPosting    ,           User_IdNo           ,          Total_Taxable_Value          ,       Total_CGST_Amount              ,        Total_SGST_Amount             ,       Total_IGST_Amount              ,     GST_Tax_Invoice_Status        , Po_No                         ,        Po_Date) " & _
                                    "   Values                       (  'GST'              , '" & Trim(NewCode) & "'    , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ",       @InvoiceDate    , " & Str(Val(Led_ID)) & ",  " & Str(Val(SalAc_ID)) & "       , '" & Trim(txt_LrNo.Text) & "', '" & Trim(dtp_LrDate.Text) & "', " & Str(Val(Trans_ID)) & ", " & Str(Val(vTotMtrs)) & " , " & Str(Val(lbl_GrossAmount.Text)) & ", " & Str(Val(txt_DiscPerc.Text)) & ", " & Str(Val(lbl_DiscAmount.Text)) & ", " & Str(Val(lbl_AssessableValue.Text)) & ", '" & Trim(cbo_TaxType.Text) & "', " & Str(Val(txt_TaxPerc.Text)) & ", " & Str(Val(lbl_TaxAmount.Text)) & ", " & Str(Val(VatAc_ID)) & ", " & Str(Val(txt_Packing.Text)) & ", " & Str(Val(txt_AddLess.Text)) & ", " & Str(Val(lbl_RoundOff.Text)) & ", " & Str(Val(CSng(lbl_NetAmount.Text))) & ",  '" & Trim(txt_Note.Text) & "',   '" & Trim(cbo_Type.Text) & "' , " & Str(Val(vTotBbn)) & ", " & Str(Val(vTotjmp)) & " ,  " & Str(Val(vTotcns)) & ", " & Str(Val(vTotwgt)) & "," & Str(Val(noAccPost)) & " , " & Val(lbl_UserName.Text) & "," & Str(Val(lbl_TaxableValue.Text)) & "," & Str(Val(lbl_CGST_Amount.Text)) & "," & Str(Val(lbl_SGST_Amount.Text)) & "," & Str(Val(lbl_IGST_Amount.Text)) & "," & Str(Val(vGST_Tax_Inv_Sts)) & " , '" & Trim(txt_PoNo.Text) & "',  '" & Trim(vPurOrd) & "') "
                cmd.ExecuteNonQuery()

            Else

                cmd.CommandText = "Update Proforma_BobinSales_Invoice_Head set Entry_VAT_GST_Type = 'GST', BobinDelivery_Invoice_Date = @InvoiceDate, Ledger_IdNo = " & Str(Val(Led_ID)) & ",  SalesAc_IdNo = " & Str(Val(SalAc_ID)) & ", Lr_No = '" & Trim(txt_LrNo.Text) & "', Lr_Date = '" & Trim(dtp_LrDate.Text) & "', Transport_IdNo = " & Str(Val(Trans_ID)) & ",  Total_Meters = " & Str(Val(vTotMtrs)) & ", Total_Amount = " & Str(Val(lbl_GrossAmount.Text)) & ", Discount_Percentage = " & Str(Val(txt_DiscPerc.Text)) & ", Discount_Amount = " & Str(Val(lbl_DiscAmount.Text)) & ", Assessable_Value = " & Str(Val(lbl_AssessableValue.Text)) & ", Tax_Type = '" & Trim(cbo_TaxType.Text) & "', Tax_Percentage = " & Str(Val(txt_TaxPerc.Text)) & ", Tax_Amount = " & Str(Val(lbl_TaxAmount.Text)) & ", VatAc_IdNo = " & Str(Val(VatAc_ID)) & ", Packing_Amount = " & Str(Val(txt_Packing.Text)) & ", AddLess_Amount = " & Str(Val(txt_AddLess.Text)) & ", RoundOff_Amount = " & Str(Val(lbl_RoundOff.Text)) & ", Selection_Type = '" & Trim(cbo_Type.Text) & "' , Net_Amount = " & Str(Val(CSng(lbl_NetAmount.Text))) & ",  Note = '" & Trim(txt_Note.Text) & "' , Total_Bobin = " & Str(Val(vTotBbn)) & " , Total_Jumpo = " & Str(Val(vTotjmp)) & "  , Total_Cones = " & Str(Val(vTotcns)) & "  , Total_Weights = " & Str(Val(vTotwgt)) & " ,NoAccountPosting = " & Str(Val(noAccPost)) & " , User_Idno = " & Val(lbl_UserName.Text) & " ,Total_Taxable_Value =" & Str(Val(lbl_TaxableValue.Text)) & " ,Total_CGST_Amount =" & Str(Val(lbl_CGST_Amount.Text)) & ",   Total_SGST_Amount =" & Str(Val(lbl_SGST_Amount.Text)) & "   , Total_IGST_Amount =" & Str(Val(lbl_IGST_Amount.Text)) & "  ,GST_Tax_Invoice_Status =" & Str(Val(vGST_Tax_Inv_Sts)) & ", Po_No = '" & Trim(txt_PoNo.Text) & "', Po_Date = '" & Trim(vPurOrd) & "' Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and BobinSales_Invoice_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                'cmd.CommandText = "Update Bobin_Jari_Delivery_Head set BobinSales_Invoice_Code = '' Where BobinSales_Invoice_Code = '" & Trim(NewCode) & "'"
                'cmd.ExecuteNonQuery()

            End If

            Partcls = "Bill : Inv.No. " & Trim(lbl_InvoiceNo.Text)
            PBlNo = Trim(lbl_InvoiceNo.Text)
            EntID = Trim(Pk_Condition) & Trim(lbl_InvoiceNo.Text)


            cmd.CommandText = "Delete from Proforma_BobinSales_Invoice_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and BobinSales_Invoice_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            With dgv_Details

                Sno = 0

                For i = 0 To .RowCount - 1

                    If (.Rows(i).Cells(DGVCol_BobinDetails.Ends).Value) <> "" Or (.Rows(i).Cells(DGVCol_BobinDetails.Count).Value) <> "" Then

                        Sno = Sno + 1

                        Ends_ID = Common_Procedures.EndsCount_NameToIdNo(con, .Rows(i).Cells(DGVCol_BobinDetails.Ends).Value, tr)
                        Cnt_ID = Common_Procedures.Count_NameToIdNo(con, .Rows(i).Cells(DGVCol_BobinDetails.Count).Value, tr)
                        BbnSz_Id = Common_Procedures.BobinSize_NameToIdNo(con, .Rows(i).Cells(DGVCol_BobinDetails.Bobin_Size).Value, tr)

                        cmd.CommandText = "Insert into Proforma_BobinSales_Invoice_Details ( BobinSales_Invoice_Code , Company_IdNo                     , BobinSales_Invoice_No             , for_OrderBy                                                                 , BobinDelivery_Invoice_Date  , Selection_Type               , Ledger_IdNo             ,          Sl_No       , Bobin_Delivery_No                                              , Bobin_Delivery_Date                                               , EndsCount_Idno            , Count_Idno                , No_OF_Bobin                                                          , No_OF_Jumpo                                                       , No_OF_Cones                                                           , Meters                                                             , Weights                                                            , Rate_For                                                          , Rate                                                            ,  Amount                                                             ,   Bobin_Jari_Delivery_Code                                              , Bobin_Jari_Delivery_Slno                                                , Cash_Discount_Percentage                                                        , Cash_Discount_Amount                                                      , Taxable_Value                                                              , GST_Percentage                                                             , HSN_Code                                                          ,     Bobin_Size_IdNo      ) " & _
                        "Values                                                            ('" & Trim(NewCode) & "'  , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & " , @InvoiceDate                , '" & Trim(cbo_Type.Text) & "', " & Str(Val(Led_ID)) & ", " & Str(Val(Sno)) & ", '" & Trim(.Rows(i).Cells(DGVCol_BobinDetails.DC_No).Value) & "', '" & Trim(.Rows(i).Cells(DGVCol_BobinDetails.DC_Date).Value) & "' , " & Str(Val(Ends_ID)) & " ,  " & Str(Val(Cnt_ID)) & " , " & Str(Val(.Rows(i).Cells(DGVCol_BobinDetails.NoOfBobin).Value)) & ", " & Str(Val(.Rows(i).Cells(DGVCol_BobinDetails.Jumpo).Value)) & " , " & Str(Val(.Rows(i).Cells(DGVCol_BobinDetails.NoOfCones).Value)) & " , " & Str(Val(.Rows(i).Cells(DGVCol_BobinDetails.Meters).Value)) & " , " & Str(Val(.Rows(i).Cells(DGVCol_BobinDetails.Weight).Value)) & " , '" & Trim(.Rows(i).Cells(DGVCol_BobinDetails.RateFor).Value) & "' , " & Str(Val(.Rows(i).Cells(DGVCol_BobinDetails.Rate).Value)) & ",  " & Str(Val(.Rows(i).Cells(DGVCol_BobinDetails.Amount).Value)) & " , '" & Trim(.Rows(i).Cells(DGVCol_BobinDetails.Delivery_Code).Value) & "' , " & Str(Val(.Rows(i).Cells(DGVCol_BobinDetails.Delvery_SNo).Value)) & " , " & Str(Val(.Rows(i).Cells(DGVCol_BobinDetails.Discount_Percentage).Value)) & " , " & Str(Val(.Rows(i).Cells(DGVCol_BobinDetails.Discount_Value).Value)) & ", " & Str(Val(.Rows(i).Cells(DGVCol_BobinDetails.Taxable_Amount).Value)) & " , " & Str(Val(.Rows(i).Cells(DGVCol_BobinDetails.GST_Percentage).Value)) & " , '" & Trim(.Rows(i).Cells(DGVCol_BobinDetails.HSN_Code).Value) & "'," & Str(Val(BbnSz_Id)) & ") "
                        cmd.ExecuteNonQuery()

                        'If Trim(UCase(cbo_Type.Text)) = "DELIVERY" Then

                        '    Nr = 0
                        '    cmd.CommandText = "Update Bobin_Jari_Delivery_Head set BobinSales_Invoice_Code = '" & Trim(NewCode) & "' Where Bobin_Jari_Delivery_Code = '" & Trim(.Rows(i).Cells(13).Value) & "'"
                        '    Nr = cmd.ExecuteNonQuery()
                        '    If Nr = 0 Then
                        '        Throw New ApplicationException("Mismatch of Delivery and Party Details")
                        '        Exit Sub
                        '    End If

                        'End If

                    End If

                Next

            End With

            '---Tax Details
            cmd.CommandText = "Delete from Proforma_BobinSales_Invoice_GST_Tax_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and BobinSales_Invoice_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            With dgv_Tax_Details

                Sno = 0
                For i = 0 To .Rows.Count - 1

                    If Val(.Rows(i).Cells(3).Value) <> 0 Or Val(.Rows(i).Cells(5).Value) <> 0 Or Val(.Rows(i).Cells(7).Value) <> 0 Then

                        Sno = Sno + 1

                        cmd.CommandText = "Insert into Proforma_BobinSales_Invoice_GST_Tax_Details   ( BobinSales_Invoice_Code  ,               Company_IdNo       ,      BobinSales_Invoice_No        ,                               for_OrderBy                                  , BobinDelivery_Invoice_Date  ,         Ledger_IdNo     ,            Sl_No     , HSN_Code                               ,Taxable_Amount                            ,CGST_Percentage                           ,CGST_Amount                               ,SGST_Percentage                            ,SGST_Amount                              ,IGST_Percentage                          ,IGST_Amount ) " & _
                                                "     Values                                (   '" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ",       @InvoiceDate          , " & Str(Val(Led_ID)) & ", " & Str(Val(Sno)) & ", '" & Trim(.Rows(i).Cells(1).Value) & "', " & Str(Val(.Rows(i).Cells(2).Value)) & ", " & Str(Val(.Rows(i).Cells(3).Value)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & "," & Str(Val(.Rows(i).Cells(5).Value)) & "  ," & Str(Val(.Rows(i).Cells(6).Value)) & "," & Str(Val(.Rows(i).Cells(7).Value)) & "," & Str(Val(.Rows(i).Cells(8).Value)) & ") "
                        cmd.ExecuteNonQuery()

                    End If

                Next i

            End With

            'If Trim(lbl_TaxAmount.Text) = "" Then lbl_TaxAmount.Text = 0

            'If chk_NoAccountPosting.Checked = False Then
            ''Dim vLed_IdNos As String = "", vVou_Amts As String = "", ErrMsg As String = ""

            ''vLed_IdNos = Led_ID & "|" & SalAc_ID & "|" & VatAc_ID

            ''vVou_Amts = -1 * Val(CSng(lbl_NetAmount.Text)) & "|" & (Val(CSng(lbl_NetAmount.Text)) - Val(CSng(lbl_TaxAmount.Text))) & "|" & Val(CSng(lbl_TaxAmount.Text))

            'vLed_IdNos = Led_ID & "|" & SalAc_ID & "|" & "24|25|26"

            'vVou_Amts = -1 * Val(CSng(lbl_NetAmount.Text)) & "|" & (Val(CSng(lbl_NetAmount.Text)) - Val(CSng(lbl_CGST_Amount.Text)) - Val(CSng(lbl_SGST_Amount.Text)) - Val(CSng(lbl_IGST_Amount.Text))) & "|" & Val(CSng(lbl_CGST_Amount.Text)) & "|" & Val(CSng(lbl_SGST_Amount.Text)) & "|" & Val(CSng(lbl_IGST_Amount.Text))

            'If Common_Procedures.Voucher_Updation(con, "Gst.Bob.Inv", Val(lbl_Company.Tag), Trim(NewCode), Trim(lbl_InvoiceNo.Text), dtp_Date.Value.Date, "Inv No : " & Trim(lbl_InvoiceNo.Text), vLed_IdNos, vVou_Amts, ErrMsg, tr,Common_Procedures.SoftwareTypes.Textile_Software) = False Then
            '    Throw New ApplicationException(ErrMsg)
            'End If

            'Dim VouBil As String = ""
            'VouBil = Common_Procedures.VoucherBill_Posting(con, Val(lbl_Company.Tag), dtp_Date.Value.Date, Led_ID, Trim(lbl_InvoiceNo.Text), 0, Val(CSng(lbl_NetAmount.Text)), "DR", Trim(NewCode), tr,Common_Procedures.SoftwareTypes.Textile_Software)
            'If Trim(UCase(VouBil)) = "ERROR" Then
            '    Throw New ApplicationException("Error on Voucher Bill Posting")
            'End If

            'Else

            'If Common_Procedures.VoucherBill_Deletion(con, Trim(NewCode), tr) = False Then
            '    Throw New ApplicationException("Error on Voucher Bill Deletion")
            'End If

            'Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(NewCode), tr)

            'End If



            tr.Commit()

            move_record(lbl_InvoiceNo.Text)

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception

            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)


        Finally

            Dt1.Dispose()
            Da.Dispose()
            cmd.Dispose()
            tr.Dispose()

            If msk_Date.Visible And msk_Date.Enabled Then msk_Date.Focus()

        End Try

    End Sub

    Private Sub cbo_Ledger_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Ledger_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, msk_Date, cbo_SalesAc, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Ledger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ledger.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Ledger, cbo_SalesAc, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14)", "(Ledger_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            cbo_SalesAc.Focus()
        End If
        'If Asc(e.KeyChar) = 13 And cbo_Type.Text = "DELIVERY" Then
        '    If MessageBox.Show("Do you want to select Delivery?", "FOR ORDER SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
        '        btn_EntrySelection_Click(sender, e)

        '    Else
        '        cbo_SalesAc.Focus()

        '    End If

        'End If
    End Sub

    Private Sub cbo_Transport_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Transport.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Transport_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Transport.KeyDown
        Try
            vcbo_KeyDwnVal = e.KeyValue
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Transport, msk_PoDate, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")

            If (e.KeyValue = 40 And cbo_Transport.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                If dgv_Details.Rows.Count > 0 Then
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(DGVCol_BobinDetails.DC_No)
                End If
            End If

        Catch ex As Exception
            '-----
        End Try
    End Sub

    Private Sub cbo_Transport_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Transport.KeyPress

        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Transport, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")

            If Asc(e.KeyChar) = 13 Then

                'If MessageBox.Show("Do you want to select Packing Sip?", "FOR BALE SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                '    btn_Selection_Click(sender, e)

                'Else
                If dgv_Details.Rows.Count > 0 Then
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(DGVCol_BobinDetails.DC_No)
                    dgv_Details.CurrentCell.Selected = True

                Else
                    txt_DiscPerc.Focus()

                End If

            End If

            'End If

        Catch ex As Exception
            '------
        End Try

    End Sub

    Private Sub cbo_SalesAc_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_SalesAc.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 28)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_SalesAc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_SalesAc.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_SalesAc, cbo_Ledger, txt_LrNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 28)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_SalesAc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_SalesAc.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_SalesAc, txt_LrNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 28 and Verified_Status = 1)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub btn_Filter_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter_Close.Click
        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        Filter_Status = False
    End Sub

    Private Sub btn_Filter_Show_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter_Show.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim n As Integer
        Dim Led_IdNo As Integer, Cnt_IdNo As Integer
        Dim Condt As String = ""

        Try

            Condt = ""
            Led_IdNo = 0
            Cnt_IdNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.BobinDelivery_Invoice_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.BobinDelivery_Invoice_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.BobinDelivery_Invoice_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If


            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.Ledger_IdNo = " & Str(Val(Led_IdNo))
            End If

            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name from Proforma_BobinSales_Invoice_Head a INNER JOIN Ledger_Head b on a.Ledger_IdNo = b.Ledger_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.BobinSales_Invoice_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.BobinSales_Invoice_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("BobinSales_Invoice_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("BobinDelivery_Invoice_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("Total_Meters").ToString), "########0.00")
                    dgv_Filter_Details.Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("Net_Amount").ToString), "########0.00")

                Next i

            End If

            dt2.Clear()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FILTER...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            dt2.Dispose()
            da.Dispose()

            If dgv_Filter_Details.Visible And dgv_Filter_Details.Enabled Then dgv_Filter_Details.Focus()

        End Try

    End Sub

    Private Sub cbo_Filter_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, dtp_Filter_ToDate, btn_Filter_Show, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ", "(Ledger_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then
            btn_Filter_Show_Click(sender, e)
        End If

    End Sub

    Private Sub Open_FilterEntry()
        Dim movno As String = ""

        Try
            movno = Trim(dgv_Filter_Details.CurrentRow.Cells(1).Value)

            If Val(movno) <> 0 Then
                Filter_Status = True
                move_record(movno)
                pnl_Back.Enabled = True
                pnl_Filter.Visible = False
            End If

        Catch ex As Exception
            '------
        End Try


    End Sub

    Private Sub dgv_Filter_Details_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Filter_Details.CellDoubleClick
        Open_FilterEntry()
    End Sub

    Private Sub dgv_Filter_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Filter_Details.KeyDown
        If e.KeyCode = 13 Then
            Open_FilterEntry()
        End If
    End Sub

    Private Sub dgv_Details_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEndEdit
        dgv_Details_CellLeave(sender, e)
    End Sub

    Private Sub dgv_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellLeave
        Try
            With dgv_Details
                If .CurrentCell.ColumnIndex = DGVCol_BobinDetails.Rate Or .CurrentCell.ColumnIndex = DGVCol_BobinDetails.Amount Or .CurrentCell.ColumnIndex = DGVCol_BobinDetails.Meters Then
                    If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                        .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                    End If
                End If
                If .CurrentCell.ColumnIndex = DGVCol_BobinDetails.Weight Then
                    If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                        .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.000")
                    End If
                End If
            End With
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT CHANGE VALUE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged
        Dim q As Single = 0

        If FrmLdSTS = True Then
            Exit Sub
        End If

        Try
            With dgv_Details
                If .Visible Then
                    If .Rows.Count > 0 Then
                        If e.ColumnIndex = DGVCol_BobinDetails.Meters Or e.ColumnIndex = DGVCol_BobinDetails.Weight Or e.ColumnIndex = DGVCol_BobinDetails.RateFor Or e.ColumnIndex = DGVCol_BobinDetails.Rate Then

                            If Trim(UCase(.CurrentRow.Cells(DGVCol_BobinDetails.RateFor).Value)) = "KG" Then
                                .CurrentRow.Cells(DGVCol_BobinDetails.Amount).Value = Format(Val(.CurrentRow.Cells(DGVCol_BobinDetails.Rate).Value) * Val(.CurrentRow.Cells(DGVCol_BobinDetails.Weight).Value), "#########0.00")
                            Else
                                .CurrentRow.Cells(DGVCol_BobinDetails.Amount).Value = Format(Val(.CurrentRow.Cells(DGVCol_BobinDetails.Rate).Value) * Val(.CurrentRow.Cells(DGVCol_BobinDetails.Meters).Value), "#########0.00")
                            End If

                            Total_Calculation()

                        End If

                    End If
                End If
            End With

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT CHANGE VALUE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    'Private Sub consumption_calculation(ByVal CurRow As Integer, ByVal CurCol As Integer)
    '    Dim da As New SqlClient.SqlDataAdapter
    '    Dim dt As New DataTable
    '    Dim da1 As New SqlClient.SqlDataAdapter
    '    Dim dt1 As New DataTable
    '    Dim cnt As Single = 0
    '    Dim Cnt_ID, EndsCnt_ID As Integer
    '    Dim conspn As Single = 0
    '    Dim ends As Single = 0

    '    Try

    '        With dgv_Details
    '            If .Visible Then
    '                If .Rows.Count > 0 Then
    '                    'If CurCol = 1 Or CurCol = 2 Then
    '                    EndsCnt_ID = 0
    '                    Cnt_ID = 0

    '                    If Trim(.Rows(CurRow).Cells(3).Value) <> "" Then

    '                        EndsCnt_ID = Common_Procedures.EndsCount_NameToIdNo(con, Trim(.Rows(CurRow).Cells(3).Value))

    '                    End If

    '                    If EndsCnt_ID <> 0 Then


    '                        da1 = New SqlClient.SqlDataAdapter("select a.* from EndsCount_Head a Where a.EndsCount_IdNo = " & Str(Val(EndsCnt_ID)), con)
    '                        dt1 = New DataTable
    '                        da1.Fill(dt1)

    '                        If dt1.Rows.Count > 0 Then
    '                            If IsDBNull(dt1.Rows(0)(0).ToString) = False Then
    '                                ends = Val(dt1.Rows(0).Item("Ends_Name").ToString)
    '                                Cnt_ID = Val(dt1.Rows(0).Item("Count_IdNo").ToString)
    '                            End If
    '                        End If

    '                        dt1.Dispose()
    '                        da1.Dispose()


    '                        da = New SqlClient.SqlDataAdapter("select a.* from Count_Head a Where a.Count_IdNo = " & Str(Val(Cnt_ID)), con)
    '                        dt = New DataTable
    '                        da.Fill(dt)

    '                        If dt.Rows.Count > 0 Then
    '                            If IsDBNull(dt.Rows(0)(0).ToString) = False Then
    '                                cnt = Format(Val(dt.Rows(0).Item("Resultant_Count").ToString), "#######0")
    '                            End If
    '                        End If

    '                        dt.Dispose()
    '                        da.Dispose()

    '                        'If Val(.Rows(CurRow).Cells(5).Value) <> 0 And Val(.Rows(CurRow).Cells(8).Value) <> 0 And Val(cnt) <> 0 And Val(ends) <> 0 Then

    '                        conspn = (Val(.Rows(CurRow).Cells(5).Value) * Val(.Rows(CurRow).Cells(8).Value) * Val(ends) * Val(cnt)) / 9000000
    '                        'If Val(.Rows(CurRow).Cells(9).Value) = 0 Then
    '                        .Rows(CurRow).Cells(9).Value = Format(Val(conspn), "#######0.000")
    '                        'End If

    '                        'End If

    '                    End If

    '                    'End If
    '                End If
    '            End If
    '        End With

    '        Total_Calculation()

    '    Catch ex As Exception
    '        '-----

    '    End Try
    'End Sub

    Private Sub dgv_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Details.EditingControlShowing
        Try
            dgtxt_Details = CType(dgv_Details.EditingControl, DataGridViewTextBoxEditingControl)
        Catch ex As Exception
            '----
        End Try

    End Sub

    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.Enter
        Try
            dgv_Details.EditingControl.BackColor = Color.Lime
            dgv_Details.EditingControl.ForeColor = Color.Blue
            dgtxt_Details.SelectAll()
        Catch ex As Exception
            '-----
        End Try

    End Sub

    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Details.KeyPress
        Try
            With dgv_Details
                If .Visible Then
                    If .Rows.Count > 0 Then
                        If .CurrentCell.ColumnIndex = DGVCol_BobinDetails.Ends Or .CurrentCell.ColumnIndex = DGVCol_BobinDetails.Count Or .CurrentCell.ColumnIndex = DGVCol_BobinDetails.NoOfBobin Or .CurrentCell.ColumnIndex = DGVCol_BobinDetails.Jumpo Or .CurrentCell.ColumnIndex = DGVCol_BobinDetails.NoOfCones Or .CurrentCell.ColumnIndex = DGVCol_BobinDetails.Meters Or .CurrentCell.ColumnIndex = DGVCol_BobinDetails.Weight Or .CurrentCell.ColumnIndex = DGVCol_BobinDetails.Rate Then
                            If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                                e.Handled = True
                            End If
                        End If
                    End If
                End If
            End With

        Catch ex As Exception
            '----
        End Try

    End Sub

    Private Sub cbo_Ledger_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyUp
        Try
            If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
                Common_Procedures.MDI_LedType = ""
                Dim f As New Ledger_Creation

                Common_Procedures.Master_Return.Form_Name = Me.Name
                Common_Procedures.Master_Return.Control_Name = cbo_Ledger.Name
                Common_Procedures.Master_Return.Return_Value = ""
                Common_Procedures.Master_Return.Master_Type = ""

                f.MdiParent = MDIParent1
                f.Show()
            End If
        Catch ex As Exception
            '---
        End Try

    End Sub

    Private Sub btn_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub dgv_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.LostFocus
        On Error Resume Next
        dgv_Details.CurrentCell.Selected = False
    End Sub

    Private Sub txt_AddLess_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_AddLess.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_AddLess_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_AddLess.LostFocus
        txt_AddLess.Text = Format(Val(txt_AddLess.Text), "#########0.00")
    End Sub

    Private Sub txt_AddLess_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_AddLess.TextChanged
        Total_Calculation()
    End Sub

    Private Sub txt_Packing_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Packing.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_Packing_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Packing.LostFocus
        txt_Packing.Text = Format(Val(txt_Packing.Text), "#########0.00")
    End Sub

    Private Sub txt_Packing_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_Packing.TextChanged
        Total_Calculation()
    End Sub

    Private Sub txt_TaxPerc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_TaxPerc.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_VatPerc_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_TaxPerc.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub txt_DiscPerc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_DiscPerc.KeyDown
        If e.KeyValue = 38 Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(DGVCol_BobinDetails.DC_No)
                dgv_Details.CurrentCell.Selected = True

            Else
                cbo_Transport.Focus()

            End If
        End If

        If e.KeyValue = 40 Then SendKeys.Send("{TAB}")

    End Sub

    Private Sub txt_DiscPerc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_DiscPerc.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_DiscPerc_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_DiscPerc.TextChanged
        If dgv_Details.Rows.Count > 0 Then
            For i = 0 To dgv_Details.Rows.Count - 1
                dgv_Details.Rows(i).Cells(DGVCol_BobinDetails.Discount_Percentage).Value = Val(txt_DiscPerc.Text)
            Next
        End If

        Total_Calculation()
    End Sub

    Private Sub txt_Note_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Note.KeyPress
        'If Asc(e.KeyChar) = 13 Then
        '    If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
        '        save_record()
        '    Else
        '        dtp_Date.Focus()
        '    End If
        'End If
    End Sub

    Private Sub Total_Calculation()
        Dim Sno As Integer
        Dim Totbob As Single, TotJum As Single, TotCns As Single
        Dim TotMtrs As Single, TotAmt As Single, TotWgt As Single
        Dim TotDisc As Single

        Try


            If NoCalc_Status = True Then Exit Sub

            GST_Calculation()

            Sno = 0
            TotMtrs = 0 : TotAmt = 0 : TotWgt = 0
            Totbob = 0 : TotJum = 0 : TotCns = 0 : TotDisc = 0

            With dgv_Details
                For i = 0 To .RowCount - 1
                    Sno = Sno + 1
                    .Rows(i).Cells(0).Value = Sno
                    If Trim(.Rows(i).Cells(DGVCol_BobinDetails.Ends).Value) <> "" Or Trim(.Rows(i).Cells(DGVCol_BobinDetails.Count).Value) <> "" Then

                        Totbob = Totbob + Val(.Rows(i).Cells(DGVCol_BobinDetails.NoOfBobin).Value)
                        TotJum = TotJum + Val(.Rows(i).Cells(DGVCol_BobinDetails.Jumpo).Value)
                        TotCns = TotCns + Val(.Rows(i).Cells(DGVCol_BobinDetails.NoOfCones).Value)

                        TotMtrs = TotMtrs + Val(.Rows(i).Cells(DGVCol_BobinDetails.Meters).Value)
                        TotWgt = TotWgt + Val(.Rows(i).Cells(DGVCol_BobinDetails.Weight).Value)

                        TotAmt = TotAmt + Val(.Rows(i).Cells(DGVCol_BobinDetails.Amount).Value)
                        TotDisc = TotDisc + Val(.Rows(i).Cells(DGVCol_BobinDetails.Discount_Value).Value)

                    End If

                Next

            End With

            lbl_GrossAmount.Text = Format(Val(TotAmt), "########0.00")
            lbl_DiscAmount.Text = Format(Val(TotDisc), "########0.00")

            With dgv_Details_Total
                If .RowCount = 0 Then .Rows.Add()
                .Rows(0).Cells(5).Value = Val(Totbob)
                .Rows(0).Cells(6).Value = Val(TotJum)
                .Rows(0).Cells(7).Value = Val(TotCns)

                .Rows(0).Cells(8).Value = Format(Val(TotMtrs), "########0.00")
                .Rows(0).Cells(9).Value = Format(Val(TotWgt), "########0.000")
                .Rows(0).Cells(12).Value = Format(Val(TotAmt), "########0.00")

            End With




            NetAmount_Calculation()

        Catch ex As Exception
            '----

        End Try

    End Sub

    Private Sub NetAmount_Calculation()
        Dim NtAmt As Single = 0

        Try

            If NoCalc_Status = True Then Exit Sub


            NtAmt = Val(lbl_GrossAmount.Text) + Val(lbl_CGST_Amount.Text) + Val(lbl_SGST_Amount.Text) + Val(lbl_IGST_Amount.Text) + Val(txt_AddLess.Text) + Val(txt_Packing.Text) - Val(lbl_DiscAmount.Text)


            lbl_NetAmount.Text = Format(Val(NtAmt), "#########0")
            lbl_NetAmount.Text = Common_Procedures.Currency_Format(Val(lbl_NetAmount.Text))

            lbl_RoundOff.Text = Format(Val(CSng(lbl_NetAmount.Text)) - Val(NtAmt), "#########0.00")

            lbl_AmountInWords.Text = "Rupees  :  "
            If Val(CSng(lbl_NetAmount.Text)) <> 0 Then
                lbl_AmountInWords.Text = "Rupees  :  " & Common_Procedures.Rupees_Converstion(Val(CSng(lbl_NetAmount.Text)))
            End If

        Catch ex As Exception
            '------

        End Try

    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.PrintEntry, Common_Procedures.UR.Proforma_bobin_sales, New_Entry) = False Then Exit Sub

        pnl_Print.Visible = True
        pnl_Back.Enabled = False
        If btn_Print_VatBill.Enabled And btn_Print_VatBill.Visible Then
            btn_Print_VatBill.Focus()
        End If
    End Sub

    Public Sub printing_invoice()
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String
        'Dim ps As Printing.PaperSize

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select * from Proforma_BobinSales_Invoice_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and BobinSales_Invoice_Code = '" & Trim(NewCode) & "'", con)
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

        If prn_Status = 1 Then

            Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8X6", 800, 600)
            PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
            PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1

            'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            '    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
            '        ps = PrintDocument1.PrinterSettings.PaperSizes(I)
            '        PrintDocument1.DefaultPageSettings.PaperSize = ps
            '        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
            '        Exit For
            '    End If
            'Next

        Else
            Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8X12", 800, 1200)
            PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
            PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1

        End If

        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then
            Try
                If Print_PDF_Status = True Then
                    '--This is actual & correct 
                    PrintDocument1.DocumentName = "Invoice"
                    PrintDocument1.PrinterSettings.PrinterName = "doPDF v7"
                    PrintDocument1.PrinterSettings.PrintFileName = "c:\Invoice.pdf"
                    PrintDocument1.Print()

                Else

                    PrintDocument1.Print()

                    'PrintDialog1.PrinterSettings = PrintDocument1.PrinterSettings
                    'If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                    '    PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings
                    '    PrintDocument1.Print()
                    'End If
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
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim NewCode As String
        Dim W1 As Single = 0

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.*, d.Ledger_Name as TransportName, f.Ledger_Name as SalesAcc_Name  ,SH.* ,Lsh.State_Name as Ledger_State_Name ,Lsh.State_Code as Ledger_State_Code from Proforma_BobinSales_Invoice_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo LEFT OUTER JOIN State_Head SH ON b.Company_State_IdNo =SH.State_IdNo  INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN State_Head Lsh ON c.Ledger_State_IdNo = Lsh.State_IdNo LEFT OUTER JOIN Ledger_Head d ON d.Ledger_IdNo = a.Transport_IdNo LEFT OUTER JOIN Ledger_Head f ON f.Ledger_IdNo =a.SalesAc_IdNo  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.BobinSales_Invoice_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("Select a.* , B.ENDSCOUNT_Name , C.COUNT_Name from Proforma_BobinSales_Invoice_Details a LEFT OUTER JOIN ENDSCOUNT_Head b oN A.EndsCount_Idno = B.EndsCount_Idno LEFT OUTER JOIN COUNT_Head C oN A.Count_Idno = C.Count_Idno  Where a.BobinSales_Invoice_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
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
        If prn_HdDt.Rows.Count <= 0 Then Exit Sub
        If prn_Status = 1 Then
            Printing_Format1(e)
        ElseIf prn_Status = 3 Then
            Printing_Format3(e)
        Else
            Printing_GST_Format1(e)
            ' Printing_Format2(e)
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
        Dim SNo As Integer

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

            Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8X6", 800, 600)
            PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
            PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1

            'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            '    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
            '        ps = PrintDocument1.PrinterSettings.PaperSizes(I)
            '        PrintDocument1.DefaultPageSettings.PaperSize = ps
            '        e.PageSettings.PaperSize = ps
            '        PpSzSTS = True
            '        Exit For
            '    End If
            'Next

            'If PpSzSTS = False Then
            '    For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            '        If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
            '            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
            '            PrintDocument1.DefaultPageSettings.PaperSize = ps
            '            e.PageSettings.PaperSize = ps
            '            Exit For
            '        End If
            '    Next
            'End If

        End If

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 30
            .Right = 30
            .Top = 15 ' 30
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

        If Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then
            NoofItems_PerPage = 8
        ElseIf Val(prn_HdDt.Rows(0).Item("Packing_Amount").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("Tax_Amount").ToString) <> 0 Then
            NoofItems_PerPage = 8
        Else
            NoofItems_PerPage = 9
        End If


        Erase LnAr
        Erase ClAr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClAr(1) = Val(35) : ClAr(2) = 120 : ClAr(3) = 135 : ClAr(4) = 100 : ClAr(5) = 100 : ClAr(6) = 70
        ClAr(7) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6))

        TxtHgt = 18

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try
            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format1_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr)

                NoofDets = 0

                CurY = CurY - 10

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then

                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued...", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 10, CurY, 0, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, False)

                            e.HasMorePages = True
                            Return

                        End If

                        If Trim(prn_DetDt.Rows(prn_DetIndx).Item("Endscount_Name").ToString) <> "" Then
                            ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Endscount_Name").ToString)
                        Else
                            ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("count_Name").ToString)
                        End If

                        ItmNm2 = ""
                        If Len(ItmNm1) > 25 Then
                            For I = 15 To 1 Step -1
                                If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 25
                            ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                            ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                        End If

                        CurY = CurY + TxtHgt
                        SNo = SNo + 1
                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(SNo)), LMargin + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Bobin_Delivery_No").ToString, LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Bobin_Delivery_Date").ToString, LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 10, CurY, 0, 0, pFont)
                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Meters").ToString) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Meters").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                        Else
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Weights").ToString), "########0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)

                        End If

                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Rate").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)

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

        e.HasMorePages = False

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
        Dim S1, s2 As Single
        Dim NewCode As String

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("Select a.* , B.ENDSCOUNT_Name , C.COUNT_Name from Proforma_BobinSales_Invoice_Details a LEFT OUTER JOIN ENDSCOUNT_Head b oN A.EndsCount_Idno = B.EndsCount_Idno LEFT OUTER JOIN COUNT_Head C oN A.Count_Idno = C.Count_Idno  Where a.BobinSales_Invoice_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
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
        Common_Procedures.Print_To_PrintDocument(e, "JOBWORK FINAL DELIVERY CHALLAN", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4)
        W1 = e.Graphics.MeasureString("ORDER NO : ", pFont).Width
        'w2 = e.Graphics.MeasureString("DESP.TO : ", pFont).Width
        'S1 = e.Graphics.MeasureString("TO  :  ", pFont).Width
        s2 = e.Graphics.MeasureString("TRANSPORT :  ", pFont).Width

        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "TO  :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "DC.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("BobinSales_Invoice_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt
        pFont = New Font("Calibri", 9, FontStyle.Regular)
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        p1Font = New Font("Calibri", 14, FontStyle.Bold)

        CurY = CurY + TxtHgt

        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        pFont = New Font("Calibri", 12, FontStyle.Regular)
        Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("BobinDelivery_Invoice_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

        'CurY = CurY + TxtHgt
        'Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

        'CurY = CurY + TxtHgt
        'Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        pFont = New Font("Calibri", 9, FontStyle.Regular)
        CurY = CurY + TxtHgt
        If Trim(prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, " TIN NO. : " & prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        End If
        pFont = New Font("Calibri", 12, FontStyle.Regular)
        'If prn_HdDt.Rows(0).Item("Party_OrderNo").ToString <> "" Then
        '    Common_Procedures.Print_To_PrintDocument(e, "ORDER NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Party_OrderNo").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)
        'End If

        'CurY = CurY + TxtHgt + 20
        'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        'LnAr(3) = CurY

        'CurY = CurY + TxtHgt - 10
        ''Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Delivery_Address1").ToString, LMargin + C1 + w2 + 30, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "VECHILE NO  ", LMargin + C1 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + C1 + s2 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vechile_No").ToString, LMargin + C1 + s2 + 30, CurY, 0, 0, pFont)

        '' CurY = CurY + TxtHgt
        'Common_Procedures.Print_To_PrintDocument(e, "TRANSPORT ", LMargin + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + s2 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("TransportName").ToString, LMargin + s2 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))

        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "DC.NO", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "DC.DATE", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "ENDS/", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "COUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY + TxtHgt, 2, ClAr(4), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "METER/", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY + TxtHgt, 2, ClAr(5), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "RATE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)

        CurY = CurY + TxtHgt + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY

    End Sub

    Private Sub Printing_Format1_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim i As Integer
        Dim Cmp_Name As String
        Dim p1Font As Font
        Dim W1 As Single = 0, W2 As Single = 0
        Dim C1 As Single = 0
        Dim s2 As Single = 0
        Dim vprn_BlNos As String = ""

        Dim cmd As New SqlClient.SqlCommand
        Dim Da As SqlClient.SqlDataAdapter
        Dim Dt1 As DataTable
        Dim NetBal As Single, PreBal As Single
        Dim Amt_OpBal As Single
        Dim Cmp_Cond As String
        Dim Emp_Bob As Integer, EmpBob_Par As Integer


        For i = NoofDets + 1 To NoofItems_PerPage
            CurY = CurY + TxtHgt
        Next

        C1 = ClAr(1) + ClAr(2) + ClAr(3)
        W1 = e.Graphics.MeasureString("TOTAL BOBIN : ", pFont).Width
        'w2 = e.Graphics.MeasureString("DESP.TO : ", pFont).Width
        'S1 = e.Graphics.MeasureString("TO  :  ", pFont).Width
        s2 = e.Graphics.MeasureString("TOTAL BOBIN :  ", pFont).Width

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(5) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))

        If Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then
            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "DISCOUNT ", LMargin + C1 - 30, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + s2 + C1 - 30, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Discount_Amount").ToString, LMargin + s2 + C1 - 10, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "ADDLESS ", LMargin + C1 + 230, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + s2 + 240 + C1, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("AddLess_Amount").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
        End If

        If Val(prn_HdDt.Rows(0).Item("Packing_Amount").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("Tax_Amount").ToString) <> 0 Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "PACKING ", LMargin + C1 - 30, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + s2 + C1 - 30, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Packing_Amount").ToString, LMargin + s2 + C1 - 10, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "TAX ", LMargin + C1 + 230, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + s2 + 240 + C1, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Tax_Amount").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
        End If

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "TOTAL METERS ", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + s2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Total_Meters").ToString, LMargin + s2 + 30, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, "TOTAL WEIGHT ", LMargin + C1 - 30, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + s2 + C1 - 30, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Total_Weights").ToString, LMargin + s2 + C1 - 10, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, "NET AMOUNT ", LMargin + C1 + 230, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + s2 + 240 + C1, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Net_Amount").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(6) = CurY

        'vprn_BlNos = ""
        'For i = 0 To prn_DetDt.Rows.Count - 1
        '    If Trim(prn_DetDt.Rows(i).Item("Bales_Nos").ToString) <> "" Then
        '        vprn_BlNos = Trim(vprn_BlNos) & IIf(Trim(vprn_BlNos) <> "", ", ", "") & prn_DetDt.Rows(i).Item("Bales_Nos").ToString
        '    End If
        'Next
        'Common_Procedures.Print_To_PrintDocument(e, "BALES NOS : " & vprn_BlNos, LMargin + 10, CurY, 0, 0, pFont)

        '----   Opening Balance for Amount

        Cmp_Cond = ""
        If Val(Common_Procedures.settings.EntrySelection_Combine_AllCompany) = 0 Then
            Cmp_Cond = Cmp_Cond & IIf(Cmp_Cond <> "", " and", "") & " a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and "
        End If

        cmd.Connection = con

        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@CompFromDate", Common_Procedures.Company_FromDate)
        cmd.Parameters.AddWithValue("@SalesDate", prn_HdDt.Rows(0).Item("BobinDelivery_Invoice_Date"))

        Amt_OpBal = 0

        cmd.CommandText = "select sum(a.voucher_amount) as Op_Balance from voucher_details a where " & Cmp_Cond & " a.ledger_idno = " & Str(Val(prn_HdDt.Rows(0).Item("Ledger_IdNo"))) & " and a.voucher_date < @CompFromDate"
        Da = New SqlClient.SqlDataAdapter(cmd)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0).Item("Op_Balance").ToString) = False Then Amt_OpBal = -1 * Val(Dt1.Rows(0).Item("Op_Balance").ToString)
        End If
        Dt1.Clear()

        cmd.CommandText = "select sum(a.voucher_amount) as Op_Balance from voucher_details a, voucher_head b where " & Cmp_Cond & " a.ledger_idno = " & Str(Val(prn_HdDt.Rows(0).Item("Ledger_IdNo"))) & " and a.voucher_date between @CompFromDate and @SalesDate and ( b.entry_identification NOT LIKE '" & Trim(Pk_Condition) & "%' or b.entry_identification is Null ) and a.voucher_code = b.voucher_code and a.company_idno = b.company_idno"
        Da = New SqlClient.SqlDataAdapter(cmd)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0).Item("Op_Balance").ToString) = False Then Amt_OpBal = Amt_OpBal - Val(Dt1.Rows(0).Item("Op_Balance").ToString)
        End If
        Dt1.Clear()

        cmd.CommandText = "select sum(a.net_amount) as Inv_OpBalance from Proforma_BobinSales_Invoice_Head a Where " & Cmp_Cond & " a.ledger_idno = " & Str(Val(prn_HdDt.Rows(0).Item("Ledger_IdNo"))) & " and ( (a.BobinDelivery_Invoice_Date >= @CompFromDate and a.BobinDelivery_Invoice_Date < @SalesDate) or ( a.BobinDelivery_Invoice_Date = @SalesDate and a.for_orderby < " & Str(Val(prn_HdDt.Rows(0).Item("for_orderby"))) & " ) ) "
        Da = New SqlClient.SqlDataAdapter(cmd)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0).Item("Inv_OpBalance").ToString) = False Then Amt_OpBal = Amt_OpBal + Val(Dt1.Rows(0).Item("Inv_OpBalance").ToString)
        End If
        Dt1.Clear()

        Cmp_Cond = ""
        If Trim(UCase(Common_Procedures.User.Type)) <> "UNACCOUNT" Then
            Cmp_Cond = "Company_Type <> 'UNACCOUNT'"
        End If
        If Val(Common_Procedures.settings.EntrySelection_Combine_AllCompany) = 0 Then
            Cmp_Cond = Cmp_Cond & IIf(Cmp_Cond <> "", " and", "") & " a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & "  "
        End If

        cmd.CommandText = "Truncate table " & Trim(Common_Procedures.ReportTempSubTable) & ""
        cmd.ExecuteNonQuery()

        '---Opening

        cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempSubTable) & "(int1) Select sum(Empty_Bobin) from Stock_Empty_BeamBagCone_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON a.DeliveryTo_Idno <> 0 and a.DeliveryTo_Idno = tP.Ledger_IdNo Where " & Cmp_Cond & IIf(Trim(Cmp_Cond) <> "", " and ", "") & " a.DeliveryTo_Idno = " & Str(Val(prn_HdDt.Rows(0).Item("Ledger_IdNo").ToString)) & " and a.Reference_Date <= @SalesDate and Empty_Bobin <> 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempSubTable) & "(int1) Select -1*sum(Empty_Bobin) from Stock_Empty_BeamBagCone_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON a.ReceivedFrom_Idno <> 0 and a.ReceivedFrom_Idno = tP.Ledger_IdNo Where " & Cmp_Cond & IIf(Trim(Cmp_Cond) <> "", " and ", "") & " a.ReceivedFrom_Idno = " & Str(Val(prn_HdDt.Rows(0).Item("Ledger_IdNo").ToString)) & " and a.Reference_Date <= @SalesDate and Empty_Bobin <> 0"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempSubTable) & "(int2) Select sum(EmptyBobin_Party) from Stock_Empty_BeamBagCone_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON a.ReceivedFrom_Idno <> 0 and a.ReceivedFrom_Idno = tP.Ledger_IdNo Where " & Cmp_Cond & IIf(Trim(Cmp_Cond) <> "", " and ", "") & " a.ReceivedFrom_Idno = " & Str(Val(prn_HdDt.Rows(0).Item("Ledger_IdNo").ToString)) & " and a.Reference_Date <= @SalesDate and EmptyBobin_Party <> 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempSubTable) & "(int2) Select -1*sum(EmptyBobin_Party) from Stock_Empty_BeamBagCone_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON a.DeliveryTo_Idno <> 0 and a.DeliveryTo_Idno = tP.Ledger_IdNo Where " & Cmp_Cond & IIf(Trim(Cmp_Cond) <> "", " and ", "") & " a.DeliveryTo_Idno = " & Str(Val(prn_HdDt.Rows(0).Item("Ledger_IdNo").ToString)) & " and a.Reference_Date <= @SalesDate and EmptyBobin_Party <> 0"
        cmd.ExecuteNonQuery()

        Emp_Bob = 0
        EmpBob_Par = 0
        Da = New SqlClient.SqlDataAdapter("select sum(int1) as Empty_Bobin, sum(int2) as EmptyBobin_Party from " & Trim(Common_Procedures.ReportTempSubTable) & "", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0).Item("Empty_Bobin").ToString) = False Then Emp_Bob = Val(Dt1.Rows(0).Item("Empty_Bobin").ToString)
            If IsDBNull(Dt1.Rows(0).Item("EmptyBobin_Party").ToString) = False Then EmpBob_Par = Val(Dt1.Rows(0).Item("EmptyBobin_Party").ToString)
        End If
        Dt1.Clear()

        W1 = e.Graphics.MeasureString("Our Bobin  (With Party)               :  ", pFont).Width
        W2 = e.Graphics.MeasureString("Previous Balance  : ", pFont).Width

        CurY = CurY + TxtHgt

        Common_Procedures.Print_To_PrintDocument(e, "Our Bobin  (With Party) : " & Trim(Val(Emp_Bob)), LMargin + 10, CurY, 0, 0, pFont)

        PreBal = Amt_OpBal
        Common_Procedures.Print_To_PrintDocument(e, "Previous Balance", LMargin + W1 + 15, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + W2 + 15, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(Format(Val(PreBal), "#########0.00")), LMargin + W1 + W2 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "Party Bobin (In Godown) : " & Trim(Val(EmpBob_Par)), LMargin + 10, CurY, 0, 0, pFont)


        NetBal = Amt_OpBal + Val(prn_HdDt.Rows(0).Item("Net_amount").ToString)
        Common_Procedures.Print_To_PrintDocument(e, "Net Balance", LMargin + W1 + 15, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + W2 + 15, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(Format(Val(NetBal), "#########0.00")), LMargin + W1 + W2 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        p1Font = New Font("Calibri", 12, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

        CurY = CurY + TxtHgt + 10

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

    End Sub

    Private Sub Printing_Format2(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim cmd As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim EntryCode As String
        Dim NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim LnAr(15) As Single, ClArr(15) As Single
        'Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim W1 As Single = 0
        Dim SNo As Integer

        Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8X12", 800, 1200)
        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
        PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1

        'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
        '        ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '        PrintDocument1.DefaultPageSettings.PaperSize = ps
        '        e.PageSettings.PaperSize = ps
        '        PpSzSTS = True
        '        Exit For
        '    End If
        'Next

        'If PpSzSTS = False Then
        '    For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '        If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
        '            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '            PrintDocument1.DefaultPageSettings.PaperSize = ps
        '            e.PageSettings.PaperSize = ps
        '            Exit For
        '        End If
        '    Next
        'End If

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 10
            .Right = 40
            .Top = 40
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

        NoofItems_PerPage = 6  '8 ' 6

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = 50 : ClArr(2) = 275 : ClArr(3) = 130 : ClArr(4) = 130
        ClArr(5) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4))

        'ClArr(1) = Val(50) : ClArr(2) = 60 : ClArr(3) = 220 : ClArr(4) = 130 : ClArr(5) = 130
        'ClArr(6) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5))

        TxtHgt = 18.5 ' 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format2_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                ' W1 = e.Graphics.MeasureString("No.of Beams  : ", pFont).Width

                NoofDets = 0

                CurY = CurY - 10

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then
                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_Format2_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

                            e.HasMorePages = True
                            Return

                        End If

                        prn_DetSNo = prn_DetSNo + 1

                        'ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Cloth_Description").ToString)
                        'If Trim(ItmNm1) = "" Then
                        '    ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("cloth_Name").ToString)
                        'End If
                        'ItmNm2 = ""
                        'If Len(ItmNm1) > 45 Then
                        '    For I = 45 To 1 Step -1
                        '        If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                        '    Next I
                        '    If I = 0 Then I = 45
                        '    ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                        '    ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                        'End If

                        CurY = CurY + TxtHgt

                        SNo = SNo + 1
                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(SNo)), LMargin + 10, CurY, 0, 0, pFont)

                        If prn_DetDt.Rows(prn_DetIndx).Item("EndsCount_Name").ToString <> "" Then
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("EndsCount_Name").ToString, LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                        Else
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Count_Name").ToString, LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                        End If

                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Weights").ToString), "#########0.000"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) - 10, CurY, 1, 0, pFont)

                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Rate").ToString), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Amount").ToString), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)

                        NoofDets = NoofDets + 1

                        'If Trim(ItmNm2) <> "" Then
                        '    CurY = CurY + TxtHgt - 5
                        '    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                        '    NoofDets = NoofDets + 1
                        'End If


                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If


                Printing_Format2_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)

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
        Dim strHeight As Single
        Dim C1 As Single, W1 As Single, S1 As Single
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
        Dim NewCode As String

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        PageNo = PageNo + 1

        Cmp_Name = ""

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("Select a.* , B.ENDSCOUNT_Name , C.COUNT_Name from Proforma_BobinSales_Invoice_Details a LEFT OUTER JOIN ENDSCOUNT_Head b oN A.EndsCount_Idno = B.EndsCount_Idno LEFT OUTER JOIN COUNT_Head C oN A.Count_Idno = C.Count_Idno  Where a.BobinSales_Invoice_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
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
        CurY = CurY + TxtHgt
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "INVOICE", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try
            C1 = ClAr(1) + ClAr(2) + ClAr(3)
            W1 = e.Graphics.MeasureString("BILL NO   : ", pFont).Width
            S1 = e.Graphics.MeasureString("TO     :    ", pFont).Width

            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "TO  :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "INVOICE NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("BobinSales_Invoice_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 14, FontStyle.Bold)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "DATE ", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("BobinDelivery_Invoice_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, " TIN NO. : " & prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

            'e.Graphics.DrawLine(Pens.Black, LMargin + C1, LnAr(3), LMargin + C1, LnAr(2))
            CurY = CurY + TxtHgt - 5
            Common_Procedures.Print_To_PrintDocument(e, "Order No : " & Trim(prn_HdDt.Rows(0).Item("BobinSales_Invoice_No").ToString), LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Order Date : " & (Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("BobinDelivery_Invoice_Date").ToString), "dd-MM-yyyy").ToString), LMargin + C1 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Transport Name : " & Trim(prn_HdDt.Rows(0).Item("Transport_Name").ToString), LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "L.R No : " & Trim(prn_HdDt.Rows(0).Item("Lr_No").ToString), LMargin + C1 + 10, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PARTICULARS", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "RATE", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format2_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As SqlClient.SqlDataAdapter
        Dim Dt1 As DataTable
        Dim p1Font As Font
        Dim I As Integer
        Dim Cmp_Name As String
        Dim W1 As Single = 0
        Dim BmsInWrds As String
        Dim vprn_BlNos As String = ""
        Dim BankNm1 As String = ""
        Dim BankNm2 As String = ""
        Dim BankNm3 As String = ""
        Dim BankNm4 As String = ""
        Dim BInc As Integer
        Dim BnkDetAr() As String
        Dim NetBal As Single, PreBal As Single
        Dim Amt_OpBal As Single
        Dim Cmp_Cond As String
        Dim Emp_Bob As Integer, EmpBob_Par As Integer

        ' W1 = e.Graphics.MeasureString("No.of Beams  : ", pFont).Width

        Try

            For I = NoofDets + 1 To NoofItems_PerPage

                CurY = CurY + TxtHgt

                prn_DetIndx = prn_DetIndx + 1

            Next

            CurY = CurY + TxtHgt + 50
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Total_Weights").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Total_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)


            CurY = CurY + TxtHgt

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(7) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(2))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(4))

            Erase BnkDetAr
            If Trim(prn_HdDt.Rows(0).Item("Company_Bank_Ac_Details").ToString) <> "" Then
                BnkDetAr = Split(Trim(prn_HdDt.Rows(0).Item("Company_Bank_Ac_Details").ToString), ",")

                BInc = -1

                BInc = BInc + 1
                If UBound(BnkDetAr) >= BInc Then
                    BankNm1 = Trim(BnkDetAr(BInc))
                End If

                BInc = BInc + 1
                If UBound(BnkDetAr) >= BInc Then
                    BankNm2 = Trim(BnkDetAr(BInc))
                End If

                BInc = BInc + 1
                If UBound(BnkDetAr) >= BInc Then
                    BankNm3 = Trim(BnkDetAr(BInc))
                End If

                BInc = BInc + 1
                If UBound(BnkDetAr) >= BInc Then
                    BankNm4 = Trim(BnkDetAr(BInc))
                End If

            End If

            '----   Opening Balance for Amount

            Cmp_Cond = ""
            If Val(Common_Procedures.settings.EntrySelection_Combine_AllCompany) = 0 Then
                Cmp_Cond = Cmp_Cond & IIf(Cmp_Cond <> "", " and", "") & " a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and "
            End If

            cmd.Connection = con

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@CompFromDate", Common_Procedures.Company_FromDate)
            cmd.Parameters.AddWithValue("@SalesDate", prn_HdDt.Rows(0).Item("BobinDelivery_Invoice_Date"))

            Amt_OpBal = 0

            cmd.CommandText = "select sum(a.voucher_amount) as Op_Balance from voucher_details a where " & Cmp_Cond & " a.ledger_idno = " & Str(Val(prn_HdDt.Rows(0).Item("Ledger_IdNo"))) & " and a.voucher_date < @CompFromDate"
            Da = New SqlClient.SqlDataAdapter(cmd)
            Dt1 = New DataTable
            Da.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                If IsDBNull(Dt1.Rows(0).Item("Op_Balance").ToString) = False Then Amt_OpBal = -1 * Val(Dt1.Rows(0).Item("Op_Balance").ToString)
            End If
            Dt1.Clear()

            cmd.CommandText = "select sum(a.voucher_amount) as Op_Balance from voucher_details a, voucher_head b where " & Cmp_Cond & " a.ledger_idno = " & Str(Val(prn_HdDt.Rows(0).Item("Ledger_IdNo"))) & " and a.voucher_date between @CompFromDate and @SalesDate and ( b.entry_identification NOT LIKE '" & Trim(Pk_Condition) & "%' or b.entry_identification is Null ) and a.voucher_code = b.voucher_code and a.company_idno = b.company_idno"
            Da = New SqlClient.SqlDataAdapter(cmd)
            Dt1 = New DataTable
            Da.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                If IsDBNull(Dt1.Rows(0).Item("Op_Balance").ToString) = False Then Amt_OpBal = Amt_OpBal - Val(Dt1.Rows(0).Item("Op_Balance").ToString)
            End If
            Dt1.Clear()

            cmd.CommandText = "select sum(a.net_amount) as Inv_OpBalance from Proforma_BobinSales_Invoice_Head a Where " & Cmp_Cond & " a.ledger_idno = " & Str(Val(prn_HdDt.Rows(0).Item("Ledger_IdNo"))) & " and ( (a.BobinDelivery_Invoice_Date >= @CompFromDate and a.BobinDelivery_Invoice_Date < @SalesDate) or ( a.BobinDelivery_Invoice_Date = @SalesDate and a.for_orderby < " & Str(Val(prn_HdDt.Rows(0).Item("for_orderby"))) & " ) ) "
            Da = New SqlClient.SqlDataAdapter(cmd)
            Dt1 = New DataTable
            Da.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                If IsDBNull(Dt1.Rows(0).Item("Inv_OpBalance").ToString) = False Then Amt_OpBal = Amt_OpBal + Val(Dt1.Rows(0).Item("Inv_OpBalance").ToString)
            End If
            Dt1.Clear()

            Cmp_Cond = ""
            If Trim(UCase(Common_Procedures.User.Type)) <> "UNACCOUNT" Then
                Cmp_Cond = "Company_Type <> 'UNACCOUNT'"
            End If
            If Val(Common_Procedures.settings.EntrySelection_Combine_AllCompany) = 0 Then
                Cmp_Cond = Cmp_Cond & IIf(Cmp_Cond <> "", " and", "") & " a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & "  "
            End If

            cmd.CommandText = "Truncate table " & Trim(Common_Procedures.ReportTempSubTable) & ""
            cmd.ExecuteNonQuery()

            '---Opening

            cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempSubTable) & "(int1) Select sum(Empty_Bobin) from Stock_Empty_BeamBagCone_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON a.DeliveryTo_Idno <> 0 and a.DeliveryTo_Idno = tP.Ledger_IdNo Where " & Cmp_Cond & IIf(Trim(Cmp_Cond) <> "", " and ", "") & " a.DeliveryTo_Idno = " & Str(Val(prn_HdDt.Rows(0).Item("Ledger_IdNo").ToString)) & " and a.Reference_Date <= @SalesDate and Empty_Bobin <> 0"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempSubTable) & "(int1) Select -1*sum(Empty_Bobin) from Stock_Empty_BeamBagCone_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON a.ReceivedFrom_Idno <> 0 and a.ReceivedFrom_Idno = tP.Ledger_IdNo Where " & Cmp_Cond & IIf(Trim(Cmp_Cond) <> "", " and ", "") & " a.ReceivedFrom_Idno = " & Str(Val(prn_HdDt.Rows(0).Item("Ledger_IdNo").ToString)) & " and a.Reference_Date <= @SalesDate and Empty_Bobin <> 0"
            cmd.ExecuteNonQuery()


            cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempSubTable) & "(int2) Select sum(EmptyBobin_Party) from Stock_Empty_BeamBagCone_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON a.ReceivedFrom_Idno <> 0 and a.ReceivedFrom_Idno = tP.Ledger_IdNo Where " & Cmp_Cond & IIf(Trim(Cmp_Cond) <> "", " and ", "") & " a.ReceivedFrom_Idno = " & Str(Val(prn_HdDt.Rows(0).Item("Ledger_IdNo").ToString)) & " and a.Reference_Date <= @SalesDate and EmptyBobin_Party <> 0"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempSubTable) & "(int2) Select -1*sum(EmptyBobin_Party) from Stock_Empty_BeamBagCone_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON a.DeliveryTo_Idno <> 0 and a.DeliveryTo_Idno = tP.Ledger_IdNo Where " & Cmp_Cond & IIf(Trim(Cmp_Cond) <> "", " and ", "") & " a.DeliveryTo_Idno = " & Str(Val(prn_HdDt.Rows(0).Item("Ledger_IdNo").ToString)) & " and a.Reference_Date <= @SalesDate and EmptyBobin_Party <> 0"
            cmd.ExecuteNonQuery()


            Emp_Bob = 0
            EmpBob_Par = 0
            Da = New SqlClient.SqlDataAdapter("select sum(int1) as Empty_Bobin, sum(int2) as EmptyBobin_Party from " & Trim(Common_Procedures.ReportTempSubTable) & "", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                If IsDBNull(Dt1.Rows(0).Item("Empty_Bobin").ToString) = False Then Emp_Bob = Val(Dt1.Rows(0).Item("Empty_Bobin").ToString)
                If IsDBNull(Dt1.Rows(0).Item("EmptyBobin_Party").ToString) = False Then EmpBob_Par = Val(Dt1.Rows(0).Item("EmptyBobin_Party").ToString)
            End If
            Dt1.Clear()

            'CurY = CurY + TxtHgt
            'p1Font = New Font("Calibri", 11, FontStyle.Bold)
            'Common_Procedures.Print_To_PrintDocument(e, BankNm1, LMargin + 10, CurY, 0, 0, p1Font)
            'If Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) <> 0 Then
            '    Common_Procedures.Print_To_PrintDocument(e, "Frieght", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Freight_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            'End If

            CurY = CurY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, "Our Bobin  (With Party) : " & Trim(Val(Emp_Bob)), LMargin + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            If Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Discount " & Trim(prn_HdDt.Rows(0).Item("Discount_Percentage").ToString) & "%" & " (-) ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("Discount_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Party Bobin (In Godown) : " & Trim(Val(EmpBob_Par)), LMargin + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            If Val(prn_HdDt.Rows(0).Item("Tax_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Tax " & Trim(prn_HdDt.Rows(0).Item("Tax_Percentage").ToString) & "%", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("Tax_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
            End If


            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "AddLess", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, BankNm1, LMargin + 10, CurY, 0, 0, p1Font)
            If Val(prn_HdDt.Rows(0).Item("RoundOff_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "RoundOff", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("RoundOff_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, BankNm2, LMargin + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, PageWidth, CurY)
            LnAr(8) = CurY
            Common_Procedures.Print_To_PrintDocument(e, BankNm3, LMargin + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, BankNm4, LMargin + 10, CurY, 0, 0, p1Font)

            Common_Procedures.Print_To_PrintDocument(e, "Grand Total", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("Net_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(7))
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(9) = CurY
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(4))

            CurY = CurY + TxtHgt - 5

            BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))
            BmsInWrds = Replace(Trim(LCase(BmsInWrds)), "", "")

            Common_Procedures.Print_To_PrintDocument(e, "Rupees            : " & BmsInWrds & " ", LMargin + 10, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            CurY = CurY + 10
            p1Font = New Font("Calibri", 12, FontStyle.Underline)
            Common_Procedures.Print_To_PrintDocument(e, "Term & Conditions : ", LMargin + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt + 10
            Common_Procedures.Print_To_PrintDocument(e, "Goods Once sold will not be refundable", LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Our responsibility ceases as soon as the goods leave our premises", LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Interest will be charged at 21% from the date of purchase", LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Payment by Cheques subject to realisation", LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Subject to Tirupur jurisdiction ", LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(10) = CurY

            'If Val(Common_Procedures.User.IdNo) <> 1 Then
            '    Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
            'End If

            CurY = CurY + TxtHgt
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

            W1 = e.Graphics.MeasureString("Previous Balance  : ", pFont).Width

            'CurY = CurY + TxtHgt

            If is_LastPage = True Then
                PreBal = Amt_OpBal
                Common_Procedures.Print_To_PrintDocument(e, "Previous Balance", LMargin + 15, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 15, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Trim(Format(Val(PreBal), "#########0.00")), LMargin + W1 + 30, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt + 5
            If is_LastPage = True Then
                NetBal = Amt_OpBal + Val(prn_HdDt.Rows(0).Item("Net_amount").ToString)
                Common_Procedures.Print_To_PrintDocument(e, "Net Balance", LMargin + 15, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 15, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Trim(Format(Val(NetBal), "#########0.00")), LMargin + W1 + 30, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            'CurY = CurY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 20, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 350, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            'Common_Procedures.Print_To_PrintDocument(e, "For ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 30, CurY, 1, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "AUTHORISED SIGNATORY ", PageWidth - 5, CurY, 1, 0, pFont)
            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format3(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim cmd As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim EntryCode As String
        Dim NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single = 0, CurX As Single = 0, TxtHgt As Single = 0
        Dim LnAr(15) As Single, ClArr(15) As Single
        'Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim W1 As Single = 0

        Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 9X12", 900, 1200)
        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
        PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1
        PrintDocument1.DefaultPageSettings.Landscape = False

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 0
            .Right = 0
            .Top = 0
            .Bottom = 0

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


        ' ''========================================================================================================================
        ' ''-------------------  PRE PRINT POINTS STARTS  ----------------------------------
        ' ''========================================================================================================================

        ''Dim pFont1 As Font
        ''pFont1 = New Font("Calibri", 8, FontStyle.Regular)

        ''For I = 100 To 1100 Step 300

        ''    CurY = I
        ''    For J = 1 To 850 Step 40

        ''        CurX = J
        ''        Common_Procedures.Print_To_PrintDocument(e, CurX, CurX, CurY - 20, 0, 0, pFont1)
        ''        Common_Procedures.Print_To_PrintDocument(e, "|", CurX, CurY, 0, 0, pFont)

        ''        CurX = J + 20
        ''        Common_Procedures.Print_To_PrintDocument(e, "|", CurX, CurY, 0, 0, pFont)
        ''        Common_Procedures.Print_To_PrintDocument(e, "|", CurX, CurY + 20, 0, 0, pFont)
        ''        Common_Procedures.Print_To_PrintDocument(e, CurX, CurX, CurY + 40, 0, 0, pFont1)

        ''    Next

        ''Next

        ''For I = 200 To 800 Step 250

        ''    CurX = I
        ''    For J = 1 To 1200 Step 40

        ''        CurY = J
        ''        Common_Procedures.Print_To_PrintDocument(e, "-", CurX, CurY, 0, 0, pFont)
        ''        Common_Procedures.Print_To_PrintDocument(e, "   " & CurY, CurX, CurY, 0, 0, pFont1)

        ''        CurY = J + 20
        ''        Common_Procedures.Print_To_PrintDocument(e, "--", CurX, CurY, 0, 0, pFont)
        ''        Common_Procedures.Print_To_PrintDocument(e, "   " & CurY, CurX, CurY, 0, 0, pFont1)

        ''    Next

        ''Next

        ''e.HasMorePages = False

        ''Exit Sub

        ' ''========================================================================================================================
        ' ''-------------------  PRE PRINT POINTS ENDS  ----------------------------------
        ' ''========================================================================================================================



        NoofItems_PerPage = 7

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = 50 : ClArr(2) = 275 : ClArr(3) = 130 : ClArr(4) = 130
        ClArr(5) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4))

        TxtHgt = 19 ' 18.5 ' e.Graphics.MeasureString("A", pFont).Height  

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format3_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                NoofDets = 0

                CurY = TMargin + 400

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then

                            CurX = LMargin + 740
                            CurY = CurY + TxtHgt
                            Common_Procedures.Print_To_PrintDocument(e, "Continued....", CurX, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1
                            Printing_Format3_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

                            e.HasMorePages = True

                            Return

                        End If

                        prn_DetSNo = prn_DetSNo + 1

                        CurY = CurY + TxtHgt

                        CurX = LMargin + 40
                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetDt.Rows(prn_DetIndx).Item("Sl_No").ToString)), CurX, CurY, 0, 0, pFont)

                        CurX = LMargin + 100
                        If prn_DetDt.Rows(prn_DetIndx).Item("EndsCount_Name").ToString <> "" Then
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("EndsCount_Name").ToString, CurX, CurY, 0, 0, pFont)
                        Else
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Count_Name").ToString, CurX, CurY, 0, 0, pFont)
                        End If

                        CurX = LMargin + 440
                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Meters").ToString) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Meters").ToString), "#########0.00"), CurX, CurY, 1, 0, pFont)
                        Else
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Weights").ToString), "#########0.000"), CurX, CurY, 1, 0, pFont)
                        End If

                        'Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Weights").ToString), "#########0.000"), CurX, CurY, 1, 0, pFont)

                        CurX = LMargin + 580
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Rate").ToString), "#########0.00"), CurX, CurY, 1, 0, pFont)

                        CurX = LMargin + 740
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Amount").ToString), "#########0.00"), CurX, CurY, 1, 0, pFont)

                        NoofDets = NoofDets + 1

                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If


                Printing_Format3_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)

            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format3_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim strHeight As Single = 0, strWidth As Single = 0
        Dim CurX As Single = 0
        Dim C1 As Single = 0, W1 As Single = 0, S1 As Single = 0
        Dim NewCode As String

        Try

            NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            PageNo = PageNo + 1

            da2 = New SqlClient.SqlDataAdapter("Select a.* , b.EndsCount_Name, c.Count_Name from Proforma_BobinSales_Invoice_Details a LEFT OUTER JOIN ENDSCOUNT_Head b oN a.EndsCount_Idno = b.EndsCount_Idno LEFT OUTER JOIN COUNT_Head c oN a.Count_Idno = c.Count_Idno Where a.BobinSales_Invoice_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
            dt2 = New DataTable
            da2.Fill(dt2)
            If dt2.Rows.Count > NoofItems_PerPage Then
                CurY = TMargin
                CurX = LMargin + 740
                Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), CurX, CurY, 1, 0, pFont)
            End If
            dt2.Clear()

            CurX = LMargin + 100
            CurY = TMargin + 180
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "M/s." & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, CurX, CurY, 0, 0, p1Font)

            CurX = LMargin + 100
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, CurX, CurY, 0, 0, pFont)

            CurX = LMargin + 100
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, CurX, CurY, 0, 0, pFont)

            CurX = LMargin + 100
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, CurX, CurY, 0, 0, pFont)

            CurX = LMargin + 100
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, CurX, CurY, 0, 0, pFont)

            If Trim(prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString) <> "" Then
                CurX = LMargin + 100
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, " TIN NO. : " & prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString, CurX, CurY, 0, 0, pFont)
            End If

            CurX = LMargin + 620
            CurY = TMargin + 190
            p1Font = New Font("Calibri", 14, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("BobinSales_Invoice_No").ToString, CurX, CurY, 0, 0, p1Font)

            CurX = LMargin + 620
            CurY = TMargin + 230
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("BobinDelivery_Invoice_Date").ToString), "dd-MM-yyyy").ToString, CurX, CurY, 0, 0, pFont)


            CurX = LMargin + 180
            CurY = TMargin + 315
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("BobinSales_Invoice_No").ToString), CurX, CurY, 0, 0, pFont)
            CurX = LMargin + 600
            Common_Procedures.Print_To_PrintDocument(e, (Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("BobinDelivery_Invoice_Date").ToString), "dd-MM-yyyy").ToString), CurX, CurY, 0, 0, pFont)

            CurX = LMargin + 180
            CurY = TMargin + 340
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Transport_Name").ToString), CurX, CurY, 0, 0, pFont)
            CurX = LMargin + 600
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Lr_No").ToString), CurX, CurY, 0, 0, pFont)
            If IsDBNull(prn_HdDt.Rows(0).Item("Lr_Date").ToString) = False Then
                If Trim(prn_HdDt.Rows(0).Item("Lr_Date").ToString) <> "" Then
                    If IsDate(prn_HdDt.Rows(0).Item("Lr_Date").ToString) = True Then
                        strWidth = e.Graphics.MeasureString(Trim(prn_HdDt.Rows(0).Item("Lr_No").ToString), pFont).Width
                        CurX = CurX + strWidth + 15
                        Common_Procedures.Print_To_PrintDocument(e, "Dt. " & Trim(prn_HdDt.Rows(0).Item("Lr_Date").ToString), CurX, CurY, 0, 0, pFont)
                    End If
                End If
            End If


        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format3_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim cmd As New SqlClient.SqlCommand
        'Dim Da As SqlClient.SqlDataAdapter
        'Dim Dt1 As DataTable
        'Dim p1Font As Font
        'Dim I As Integer
        Dim p1Font As Font
        Dim W1 As Single = 0
        Dim BmsInWrds As String
        Dim vprn_BlNos As String = ""
        Dim NetBal As Single = 0, PreBal As Single = 0
        Dim Amt_OpBal As Single = 0
        Dim Cmp_Cond As String = ""
        Dim Emp_Bob As Integer = 0, EmpBob_Par As Integer = 0
        Dim CurX As Single = 0

        Try

            'For I = NoofDets + 1 To NoofItems_PerPage

            '    CurY = CurY + TxtHgt

            '    prn_DetIndx = prn_DetIndx + 1

            'Next

            'CurY = CurY + TxtHgt + 50
            'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            'LnAr(6) = CurY

            CurX = LMargin + 440
            CurY = TMargin + 600
            '  Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Total_Weights").ToString), CurX, CurY, 1, 0, pFont)
            CurX = LMargin + 740
            Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Total_Amount").ToString), CurX, CurY, 1, 0, pFont)

            ' ''----   Opening Balance for Amount

            ''Cmp_Cond = ""
            ''If Val(Common_Procedures.settings.EntrySelection_Combine_AllCompany) = 0 Then
            ''    Cmp_Cond = Cmp_Cond & IIf(Cmp_Cond <> "", " and", "") & " a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and "
            ''End If

            ''cmd.Connection = con

            ''cmd.Parameters.Clear()
            ''cmd.Parameters.AddWithValue("@CompFromDate", Common_Procedures.Company_FromDate)
            ''cmd.Parameters.AddWithValue("@SalesDate", prn_HdDt.Rows(0).Item("BobinDelivery_Invoice_Date"))

            ''Amt_OpBal = 0

            ''cmd.CommandText = "select sum(a.voucher_amount) as Op_Balance from voucher_details a where " & Cmp_Cond & " a.ledger_idno = " & Str(Val(prn_HdDt.Rows(0).Item("Ledger_IdNo"))) & " and a.voucher_date < @CompFromDate"
            ''Da = New SqlClient.SqlDataAdapter(cmd)
            ''Dt1 = New DataTable
            ''Da.Fill(Dt1)
            ''If Dt1.Rows.Count > 0 Then
            ''    If IsDBNull(Dt1.Rows(0).Item("Op_Balance").ToString) = False Then Amt_OpBal = -1 * Val(Dt1.Rows(0).Item("Op_Balance").ToString)
            ''End If
            ''Dt1.Clear()

            ''cmd.CommandText = "select sum(a.voucher_amount) as Op_Balance from voucher_details a, voucher_head b where " & Cmp_Cond & " a.ledger_idno = " & Str(Val(prn_HdDt.Rows(0).Item("Ledger_IdNo"))) & " and a.voucher_date between @CompFromDate and @SalesDate and ( b.entry_identification NOT LIKE '" & Trim(Pk_Condition) & "%' or b.entry_identification is Null ) and a.voucher_code = b.voucher_code and a.company_idno = b.company_idno"
            ''Da = New SqlClient.SqlDataAdapter(cmd)
            ''Dt1 = New DataTable
            ''Da.Fill(Dt1)
            ''If Dt1.Rows.Count > 0 Then
            ''    If IsDBNull(Dt1.Rows(0).Item("Op_Balance").ToString) = False Then Amt_OpBal = Amt_OpBal - Val(Dt1.Rows(0).Item("Op_Balance").ToString)
            ''End If
            ''Dt1.Clear()

            ''cmd.CommandText = "select sum(a.net_amount) as Inv_OpBalance from BobinSales_Invoice_Head a Where " & Cmp_Cond & " a.ledger_idno = " & Str(Val(prn_HdDt.Rows(0).Item("Ledger_IdNo"))) & " and ( (a.BobinDelivery_Invoice_Date >= @CompFromDate and a.BobinDelivery_Invoice_Date < @SalesDate) or ( a.BobinDelivery_Invoice_Date = @SalesDate and a.for_orderby < " & Str(Val(prn_HdDt.Rows(0).Item("for_orderby"))) & " ) ) "
            ''Da = New SqlClient.SqlDataAdapter(cmd)
            ''Dt1 = New DataTable
            ''Da.Fill(Dt1)
            ''If Dt1.Rows.Count > 0 Then
            ''    If IsDBNull(Dt1.Rows(0).Item("Inv_OpBalance").ToString) = False Then Amt_OpBal = Amt_OpBal + Val(Dt1.Rows(0).Item("Inv_OpBalance").ToString)
            ''End If
            ''Dt1.Clear()

            ''Cmp_Cond = ""
            ''If Trim(UCase(Common_Procedures.User.Type)) <> "UNACCOUNT" Then
            ''    Cmp_Cond = "Company_Type <> 'UNACCOUNT'"
            ''End If
            ''If Val(Common_Procedures.settings.EntrySelection_Combine_AllCompany) = 0 Then
            ''    Cmp_Cond = Cmp_Cond & IIf(Cmp_Cond <> "", " and", "") & " a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & "  "
            ''End If

            ''cmd.CommandText = "Truncate table " & Trim(Common_Procedures.ReportTempSubTable) & ""
            ''cmd.ExecuteNonQuery()

            ' ''---Opening

            ''cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempSubTable) & "(int1) Select sum(Empty_Bobin) from Stock_Empty_BeamBagCone_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON a.DeliveryTo_Idno <> 0 and a.DeliveryTo_Idno = tP.Ledger_IdNo Where " & Cmp_Cond & IIf(Trim(Cmp_Cond) <> "", " and ", "") & " a.DeliveryTo_Idno = " & Str(Val(prn_HdDt.Rows(0).Item("Ledger_IdNo").ToString)) & " and a.Reference_Date <= @SalesDate and Empty_Bobin <> 0"
            ''cmd.ExecuteNonQuery()
            ''cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempSubTable) & "(int1) Select -1*sum(Empty_Bobin) from Stock_Empty_BeamBagCone_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON a.ReceivedFrom_Idno <> 0 and a.ReceivedFrom_Idno = tP.Ledger_IdNo Where " & Cmp_Cond & IIf(Trim(Cmp_Cond) <> "", " and ", "") & " a.ReceivedFrom_Idno = " & Str(Val(prn_HdDt.Rows(0).Item("Ledger_IdNo").ToString)) & " and a.Reference_Date <= @SalesDate and Empty_Bobin <> 0"
            ''cmd.ExecuteNonQuery()


            ''cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempSubTable) & "(int2) Select sum(EmptyBobin_Party) from Stock_Empty_BeamBagCone_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON a.ReceivedFrom_Idno <> 0 and a.ReceivedFrom_Idno = tP.Ledger_IdNo Where " & Cmp_Cond & IIf(Trim(Cmp_Cond) <> "", " and ", "") & " a.ReceivedFrom_Idno = " & Str(Val(prn_HdDt.Rows(0).Item("Ledger_IdNo").ToString)) & " and a.Reference_Date <= @SalesDate and EmptyBobin_Party <> 0"
            ''cmd.ExecuteNonQuery()
            ''cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempSubTable) & "(int2) Select -1*sum(EmptyBobin_Party) from Stock_Empty_BeamBagCone_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON a.DeliveryTo_Idno <> 0 and a.DeliveryTo_Idno = tP.Ledger_IdNo Where " & Cmp_Cond & IIf(Trim(Cmp_Cond) <> "", " and ", "") & " a.DeliveryTo_Idno = " & Str(Val(prn_HdDt.Rows(0).Item("Ledger_IdNo").ToString)) & " and a.Reference_Date <= @SalesDate and EmptyBobin_Party <> 0"
            ''cmd.ExecuteNonQuery()


            ''Emp_Bob = 0
            ''EmpBob_Par = 0
            ''Da = New SqlClient.SqlDataAdapter("select sum(int1) as Empty_Bobin, sum(int2) as EmptyBobin_Party from " & Trim(Common_Procedures.ReportTempSubTable) & "", con)
            ''Dt1 = New DataTable
            ''Da.Fill(Dt1)
            ''If Dt1.Rows.Count > 0 Then
            ''    If IsDBNull(Dt1.Rows(0).Item("Empty_Bobin").ToString) = False Then Emp_Bob = Val(Dt1.Rows(0).Item("Empty_Bobin").ToString)
            ''    If IsDBNull(Dt1.Rows(0).Item("EmptyBobin_Party").ToString) = False Then EmpBob_Par = Val(Dt1.Rows(0).Item("EmptyBobin_Party").ToString)
            ''End If
            ''Dt1.Clear()


            'Common_Procedures.Print_To_PrintDocument(e, "Our Bobin  (With Party) : " & Trim(Val(Emp_Bob)), LMargin + 10, CurY, 0, 0, pFont)
            If Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString) <> 0 Then
                CurX = LMargin + 580
                CurY = TMargin + 650
                Common_Procedures.Print_To_PrintDocument(e, "Discount " & Trim(prn_HdDt.Rows(0).Item("Discount_Percentage").ToString) & "%" & " (-) ", CurX, CurY, 1, 0, pFont)
                CurX = LMargin + 740
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Discount_Amount").ToString), CurX, CurY, 1, 0, pFont)
            End If

            'Common_Procedures.Print_To_PrintDocument(e, "Party Bobin (In Godown) : " & Trim(Val(EmpBob_Par)), LMargin + 10, CurY, 0, 0, pFont)
            If Val(prn_HdDt.Rows(0).Item("Tax_Amount").ToString) <> 0 Then
                CurX = LMargin + 580
                CurY = TMargin + 670
                Common_Procedures.Print_To_PrintDocument(e, "Tax " & Trim(prn_HdDt.Rows(0).Item("Tax_Percentage").ToString) & "%" & " (+) ", CurX, CurY, 1, 0, pFont)
                CurX = LMargin + 740
                Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("Tax_Amount").ToString), CurX, CurY, 1, 0, pFont)
            End If


            If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then
                CurX = LMargin + 580
                CurY = TMargin + 690
                Common_Procedures.Print_To_PrintDocument(e, "AddLess", CurX, CurY, 1, 0, pFont)
                CurX = LMargin + 740
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString), CurX, CurY, 1, 0, pFont)
            End If

            If Val(prn_HdDt.Rows(0).Item("RoundOff_Amount").ToString) <> 0 Then
                CurX = LMargin + 580
                CurY = TMargin + 710
                'Common_Procedures.Print_To_PrintDocument(e, "RoundOff", curx, CurY, 1, 0, pFont)
                CurX = LMargin + 740
                Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("RoundOff_Amount").ToString), CurX, CurY, 1, 0, pFont)
            End If


            CurX = LMargin + 740
            CurY = TMargin + 760
            p1Font = New Font("Calibri", 14, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("Net_Amount").ToString), CurX, CurY, 1, 0, p1Font)

            BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))

            CurX = LMargin + 120
            CurY = TMargin + 800
            Common_Procedures.Print_To_PrintDocument(e, BmsInWrds, CurX, CurY, 0, 0, pFont)


            'W1 = e.Graphics.MeasureString("Previous Balance  : ", pFont).Width

            'If is_LastPage = True Then
            '    PreBal = Amt_OpBal
            '    Common_Procedures.Print_To_PrintDocument(e, "Previous Balance", LMargin + 15, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 15, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, Trim(Format(Val(PreBal), "#########0.00")), LMargin + W1 + 30, CurY, 0, 0, pFont)
            'End If

            'CurY = CurY + TxtHgt + 5
            'If is_LastPage = True Then
            '    NetBal = Amt_OpBal + Val(prn_HdDt.Rows(0).Item("Net_amount").ToString)
            '    Common_Procedures.Print_To_PrintDocument(e, "Net Balance", LMargin + 15, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 15, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, Trim(Format(Val(NetBal), "#########0.00")), LMargin + W1 + 30, CurY, 0, 0, pFont)
            'End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_VatAc_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_VatAc.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 12)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_VatAc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_VatAc.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_VatAc, txt_TaxPerc, txt_Note, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 12)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_VatAc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_VatAc.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_VatAc, txt_Note, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 12)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Transport_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Transport.KeyUp
        Try
            If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

                Common_Procedures.MDI_LedType = "TRANSPORT"
                Dim f As New Ledger_Creation

                Common_Procedures.Master_Return.Form_Name = Me.Name
                Common_Procedures.Master_Return.Control_Name = cbo_Transport.Name
                Common_Procedures.Master_Return.Return_Value = ""
                Common_Procedures.Master_Return.Master_Type = ""

                f.MdiParent = MDIParent1
                f.Show()
            End If
        Catch ex As Exception
            '----
        End Try

    End Sub

    Private Sub cbo_TaxType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_TaxType.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")

    End Sub

    Private Sub cbo_TaxType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_TaxType.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_TaxType, txt_Packing, txt_TaxPerc, "", "", "", "")
    End Sub

    Private Sub cbo_TaxType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_TaxType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_TaxType, txt_TaxPerc, "", "", "", "")
    End Sub

    Private Sub cbo_TaxType_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_TaxType.TextChanged
        If Trim(UCase(cbo_TaxType.Text)) = "" Or Trim(UCase(cbo_TaxType.Text)) = "-NIL-" Then txt_TaxPerc.Text = ""
    End Sub

    Private Sub cbo_Type_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Type.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")
    End Sub

    Private Sub cbo_Type_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Type.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Type, cbo_Ledger, cbo_SalesAc, "", "", "", "")
    End Sub

    Private Sub cbo_Type_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Type.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Type, cbo_SalesAc, "", "", "", "")

        If Asc(e.KeyChar) = 13 And cbo_Type.Text = "DELIVERY" Then
            If MessageBox.Show("Do you want to select Delivery?", "FOR ORDER SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                btn_EntrySelection_Click(sender, e)

            Else
                cbo_SalesAc.Focus()

            End If

        End If

    End Sub

    Private Sub btn_EntrySelection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_EntrySelection.Click
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim i As Integer, j As Integer, n As Integer, SNo As Integer
        Dim LedIdNo As Integer
        Dim NewCode As String
        Dim CompIDCondt As String
        Dim Ent_Bls As Single = 0
        Dim Ent_BlNos As String = ""
        Dim Ent_Pcs As Single = 0
        Dim Ent_Mtrs As Single = 0
        Dim Ent_Rate As Single = 0

        If Trim(UCase(cbo_Type.Text)) <> "DELIVERY" Then Exit Sub

        LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)

        If LedIdNo = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SELECT ORDER...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
            Exit Sub
        End If

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        CompIDCondt = "(a.company_idno = " & Str(Val(lbl_Company.Tag)) & ")"
        If Val(Common_Procedures.settings.EntrySelection_Combine_AllCompany) = 1 Then
            CompIDCondt = ""
        End If

        With dgv_Selection

            'lbl_Heading_Selection.Text = "RECEIPT SELECTION"

            .Rows.Clear()
            SNo = 0

            Da = New SqlClient.SqlDataAdapter("select a.*, b.* from Bobin_Jari_Delivery_Head a INNER JOIN Ledger_Head B ON A.Ledger_idno = b.Ledger_idno  where b.Ledger_IdNo  = " & Str(Val(LedIdNo)) & " and a.BobinSales_Invoice_Code = '" & Trim(NewCode) & "' order by a.Bobin_Jari_Delivery_Date, a.for_orderby, a.Bobin_Jari_Delivery_No", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()

                    SNo = SNo + 1
                    .Rows(n).Cells(0).Value = Val(SNo)

                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Bobin_Jari_Delivery_No").ToString
                    .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Bobin_Jari_Delivery_Date").ToString), "dd-MM-yyyy")
                    .Rows(n).Cells(3).Value = Val(Dt1.Rows(i).Item("Total_Bobins").ToString)
                    .Rows(n).Cells(4).Value = Val(Dt1.Rows(i).Item("Total_Jumbos").ToString)
                    .Rows(n).Cells(5).Value = Val(Dt1.Rows(i).Item("Total_Cones").ToString)

                    .Rows(n).Cells(6).Value = Format(Val(Dt1.Rows(i).Item("Total_Meters").ToString), "########0.00")
                    .Rows(n).Cells(7).Value = Format(Val(Dt1.Rows(i).Item("Total_Weight").ToString), "########0.000")

                    .Rows(n).Cells(8).Value = "1"
                    .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("Bobin_Jari_Delivery_Code").ToString

                    For j = 0 To .ColumnCount - 1
                        .Rows(i).Cells(j).Style.ForeColor = Color.Red
                    Next

                Next
            End If


            Da = New SqlClient.SqlDataAdapter("select a.*,b.* from Bobin_Jari_Delivery_Head a INNER JOIN Ledger_Head B ON A.Ledger_idno = b.Ledger_idno  where b.Ledger_IdNo  = " & Str(Val(LedIdNo)) & " and a.BobinSales_Invoice_Code = '' order by a.Bobin_Jari_Delivery_Date, a.for_orderby, a.Bobin_Jari_Delivery_No", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()

                    SNo = SNo + 1
                    .Rows(n).Cells(0).Value = Val(SNo)

                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Bobin_Jari_Delivery_No").ToString
                    .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Bobin_Jari_Delivery_Date").ToString), "dd-MM-yyyy")
                    .Rows(n).Cells(3).Value = Val(Dt1.Rows(i).Item("Total_Bobins").ToString)
                    .Rows(n).Cells(4).Value = Val(Dt1.Rows(i).Item("Total_Jumbos").ToString)
                    .Rows(n).Cells(5).Value = Val(Dt1.Rows(i).Item("Total_Cones").ToString)

                    .Rows(n).Cells(6).Value = Format(Val(Dt1.Rows(i).Item("Total_Meters").ToString), "########0.00")
                    .Rows(n).Cells(7).Value = Format(Val(Dt1.Rows(i).Item("Total_Weight").ToString), "########0.000")

                    .Rows(n).Cells(8).Value = ""
                    .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("Bobin_Jari_Delivery_Code").ToString

                    For j = 0 To .ColumnCount - 1
                        .Rows(i).Cells(j).Style.ForeColor = Color.Black
                    Next

                Next
            End If
        End With

        pnl_Selection.Visible = True
        pnl_Back.Enabled = False
        dgv_Selection.Focus()
        If dgv_Selection.Rows.Count > 0 Then
            dgv_Selection.CurrentCell = dgv_Selection.Rows(0).Cells(0)
            dgv_Selection.CurrentCell.Selected = True
        End If

    End Sub

    'Private Sub dgv_Selection_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Selection.CellClick
    'Select_Order(e.RowIndex)
    'End Sub

    'Private Sub Select_Order(ByVal RwIndx As Integer)
    '    Dim i As Integer

    '    With dgv_Selection

    '        If .RowCount > 0 And RwIndx >= 0 Then

    '            .Rows(RwIndx).Cells(8).Value = (Val(.Rows(RwIndx).Cells(8).Value) + 1) Mod 2

    '            If Val(.Rows(RwIndx).Cells(8).Value) = 1 Then

    '                For i = 0 To .ColumnCount - 1
    '                    .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
    '                Next


    '            Else
    '                .Rows(RwIndx).Cells(8).Value = ""

    '                For i = 0 To .ColumnCount - 1
    '                    .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Black
    '                Next

    '            End If

    '        End If

    '    End With

    'End Sub

    'Private Sub dgv_Selection_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Selection.KeyDown
    '    Dim n As Integer

    '    Try
    '        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Space Then
    '            If dgv_Selection.CurrentCell.RowIndex >= 0 Then

    '                n = dgv_Selection.CurrentCell.RowIndex

    '                Select_Order(n)

    '                e.Handled = True

    '            End If
    '        End If

    '    Catch ex As Exception
    '        '------
    '    End Try


    'End Sub

    'Private Sub btn_Close_Selection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_EntrySelection.Click
    '    Close_Order_Selection()
    'End Sub

    'Private Sub Close_Order_Selection()
    '    Dim Da1 As New SqlClient.SqlDataAdapter
    '    Dim Dt1 As New DataTable
    '    Dim Da2 As New SqlClient.SqlDataAdapter
    '    Dim Dt2 As New DataTable
    '    Dim Da3 As New SqlClient.SqlDataAdapter
    '    Dim Dt3 As New DataTable
    '    Dim Da4 As New SqlClient.SqlDataAdapter
    '    Dim Dt4 As New DataTable

    '    Dim n As Integer, i As Integer, j As Integer
    '    Dim SNo As Integer

    '    If Trim(UCase(cbo_Type.Text)) = "DELIVERY" Then

    '        dgv_Details.Rows.Clear()

    '        For i = 0 To dgv_Selection.Rows.Count - 1

    '            If Val(dgv_Selection.Rows(i).Cells(8).Value) = 1 Then

    '                Da1 = New SqlClient.SqlDataAdapter("select a.* , d.Count_Name from Bobin_Jari_Delivery_Jari_Details a INNER JOIN Count_Head d ON a.Count_IdNo = d.Count_IdNo  where a.Bobin_Jari_Delivery_Code = '" & Trim(dgv_Selection.Rows(i).Cells(9).Value) & "' Order by a.Sl_No", con)
    '                Dt1 = New DataTable
    '                Da1.Fill(Dt1)

    '                If Dt1.Rows.Count > 0 Then

    '                    For j = 0 To Dt1.Rows.Count - 1
    '                        SNo = SNo + 1

    '                        n = dgv_Details.Rows.Add()
    '                        dgv_Details.Rows(n).Cells(0).Value = Val(SNo)

    '                        dgv_Details.Rows(n).Cells(1).Value = Dt1.Rows(j).Item("Bobin_Jari_Delivery_No").ToString
    '                        dgv_Details.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(j).Item("Bobin_Jari_Delivery_Date").ToString), "dd-MM-yyyy")
    '                        'dgv_Details.Rows(n).Cells(3).Value = Dt1.Rows(j).Item("EndsCount_Name").ToString
    '                        dgv_Details.Rows(n).Cells(4).Value = Dt1.Rows(j).Item("Count_Name").ToString
    '                        'dgv_Details.Rows(n).Cells(5).Value = Val(Dt1.Rows(j).Item("Bobins").ToString)
    '                        dgv_Details.Rows(n).Cells(6).Value = Val(Dt1.Rows(j).Item("Noof_Jumbos").ToString)
    '                        dgv_Details.Rows(n).Cells(7).Value = Val(Dt1.Rows(j).Item("Noof_Cones").ToString)
    '                        'dgv_Details.Rows(n).Cells(8).Value = Format(Val(Dt1.Rows(j).Item("Meters").ToString), "#########0.00")
    '                        dgv_Details.Rows(n).Cells(9).Value = Format(Val(Dt1.Rows(j).Item("Weight").ToString), "#########0.000")
    '                        dgv_Details.Rows(n).Cells(10).Value = "KG"
    '                        dgv_Details.Rows(n).Cells(11).Value = ""
    '                        dgv_Details.Rows(n).Cells(13).Value = Dt1.Rows(j).Item("Bobin_Jari_Delivery_Code").ToString
    '                        dgv_Details.Rows(n).Cells(14).Value = Dt1.Rows(j).Item("Bobin_Jari_Delivery_Jari_Slno").ToString

    '                        Da3 = New SqlClient.SqlDataAdapter("select a.* from Proforma_BobinSales_Invoice_Details a  where a.Bobin_Jari_Delivery_Code = '" & Trim(Dt1.Rows(j).Item("Bobin_Jari_Delivery_Code").ToString) & "' and Bobin_Jari_Delivery_Slno = " & Str(Dt1.Rows(j).Item("Bobin_Jari_Delivery_Jari_Slno").ToString) & "", con)
    '                        Dt3 = New DataTable
    '                        Da3.Fill(Dt3)

    '                        If Dt3.Rows.Count > 0 Then
    '                            If Trim(Dt3.Rows(0).Item("Rate_For").ToString) <> "" Then
    '                                dgv_Details.Rows(n).Cells(10).Value = (Dt3.Rows(0).Item("Rate_For").ToString)
    '                            End If

    '                            If Val(Dt3.Rows(0).Item("Rate").ToString) <> 0 Then
    '                                dgv_Details.Rows(n).Cells(11).Value = Format(Val(Dt3.Rows(0).Item("Rate").ToString), "#########0.00")
    '                            End If
    '                            If Val(Dt3.Rows(0).Item("Amount").ToString) <> 0 Then
    '                                dgv_Details.Rows(n).Cells(12).Value = Format(Val(Dt3.Rows(0).Item("Amount").ToString), "#########0.00")
    '                            End If

    '                        End If
    '                        Dt3.Clear()
    '                    Next

    '                End If
    '                Dt1.Clear()

    '                Da2 = New SqlClient.SqlDataAdapter("select B.* , c.EndsCount_Name  from Bobin_Jari_Delivery_Bobin_Details b INNER JOIN EndsCount_Head c ON b.EndsCount_IdNo = c.EndsCount_IdNo  where b.Bobin_Jari_Delivery_Code = '" & Trim(dgv_Selection.Rows(i).Cells(9).Value) & "' Order by b.Sl_No", con)
    '                Dt2 = New DataTable
    '                Da2.Fill(Dt2)

    '                If Dt2.Rows.Count > 0 Then

    '                    For j = 0 To Dt2.Rows.Count - 1
    '                        SNo = SNo + 1

    '                        n = dgv_Details.Rows.Add()
    '                        dgv_Details.Rows(n).Cells(0).Value = Val(SNo)

    '                        dgv_Details.Rows(n).Cells(1).Value = Dt2.Rows(j).Item("Bobin_Jari_Delivery_No").ToString
    '                        dgv_Details.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt2.Rows(j).Item("Bobin_Jari_Delivery_Date").ToString), "dd-MM-yyyy")
    '                        dgv_Details.Rows(n).Cells(3).Value = Dt2.Rows(j).Item("EndsCount_Name").ToString
    '                        dgv_Details.Rows(n).Cells(5).Value = Val(Dt2.Rows(j).Item("Bobins").ToString)
    '                        dgv_Details.Rows(n).Cells(8).Value = Format(Val(Dt2.Rows(j).Item("Meters").ToString), "#########0.00")
    '                        dgv_Details.Rows(n).Cells(10).Value = "MTRS"
    '                        dgv_Details.Rows(n).Cells(13).Value = Dt2.Rows(j).Item("Bobin_Jari_Delivery_Code").ToString
    '                        dgv_Details.Rows(n).Cells(14).Value = Dt2.Rows(j).Item("Bobin_Jari_Delivery_Bobin_Slno").ToString

    '                        Da4 = New SqlClient.SqlDataAdapter("select a.* from Proforma_BobinSales_Invoice_Details a Where a.Bobin_Jari_Delivery_Code = '" & Trim(Dt2.Rows(j).Item("Bobin_Jari_Delivery_Code").ToString) & "' and Bobin_Jari_Delivery_Slno = " & Str(Val(Dt2.Rows(j).Item("Bobin_Jari_Delivery_Bobin_Slno").ToString)) & "", con)
    '                        Dt4 = New DataTable
    '                        Da4.Fill(Dt4)

    '                        If Dt4.Rows.Count > 0 Then
    '                            If Trim(Dt4.Rows(0).Item("Rate_For").ToString) <> "" Then
    '                                dgv_Details.Rows(n).Cells(10).Value = (Dt4.Rows(0).Item("Rate_For").ToString)
    '                            End If
    '                            If Val(Dt4.Rows(0).Item("Rate").ToString) <> 0 Then
    '                                dgv_Details.Rows(n).Cells(11).Value = Format(Val(Dt4.Rows(0).Item("Rate").ToString), "#########0.00")
    '                            End If
    '                            If Val(Dt4.Rows(0).Item("Amount").ToString) <> 0 Then
    '                                dgv_Details.Rows(n).Cells(12).Value = Format(Val(Dt4.Rows(0).Item("Amount").ToString), "#########0.00")
    '                            End If
    '                        End If
    '                        Dt4.Clear()

    '                        consumption_calculation(n, 8)

    '                    Next

    '                End If
    '                Dt2.Clear()

    '            End If

    '        Next

    '        'End If

    '    End If

    '    Total_Calculation()

    '    pnl_Back.Enabled = True
    '    pnl_Selection.Visible = False
    '    If cbo_SalesAc.Enabled And cbo_SalesAc.Visible Then cbo_SalesAc.Focus()

    'End Sub

    Private Sub cbo_SalesAc_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_SalesAc.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            Common_Procedures.MDI_LedType = ""
            Dim f As New LedgerCreation_Processing

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_SalesAc.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_VatAc_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_VatAc.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            Common_Procedures.MDI_LedType = ""
            Dim f As New LedgerCreation_Processing

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_VatAc.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub btn_SendSMS_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_SendSMS.Click
        Dim smstxt As String = ""
        Dim PhNo As String = ""
        Dim Led_IdNo As Integer = 0
        Dim SMS_SenderID As String = ""
        Dim SMS_Key As String = ""
        Dim SMS_RouteID As String = ""
        Dim SMS_Type As String = ""

        Try

            Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
            'If Led_IdNo  = 0 Then Exit Sub

            PhNo = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_MobileNo", "(Ledger_IdNo = " & Str(Val(Led_IdNo)) & ")")

            smstxt = "Inv No : " & Trim(lbl_InvoiceNo.Text) & Chr(13)
            smstxt = smstxt & "DATE : " & Trim(dtp_Date.Text) & Chr(13)
            smstxt = smstxt & "Lr No : " & Trim(txt_LrNo.Text) & Chr(13)
            smstxt = smstxt & "Bill Amount : " & Trim(lbl_NetAmount.Text) & Chr(13)
            smstxt = smstxt & " " & Chr(13)
            smstxt = smstxt & "Thanks! " & Chr(13)
            smstxt = smstxt & Common_Procedures.Company_IdNoToName(con, Val(lbl_Company.Tag))

            SMS_SenderID = ""
            SMS_Key = ""
            SMS_RouteID = ""
            SMS_Type = ""

            Common_Procedures.get_SMS_Provider_Details(con, Val(lbl_Company.Tag), SMS_SenderID, SMS_Key, SMS_RouteID, SMS_Type)

            Sms_Entry.SMSProvider_SenderID = SMS_SenderID
            Sms_Entry.SMSProvider_Key = SMS_Key
            Sms_Entry.SMSProvider_RouteID = SMS_RouteID
            Sms_Entry.SMSProvider_Type = SMS_Type

            Sms_Entry.vSmsPhoneNo = Trim(PhNo)
            Sms_Entry.vSmsMessage = Trim(smstxt)

            Dim f1 As New Sms_Entry
            f1.MdiParent = MDIParent1
            f1.Show()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SEND SMS...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub btn_Print_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        Print_PDF_Status = False
        print_record()
    End Sub

    Private Sub cbo_GridEnds_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_GridEnds.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "EndsCount_Head", "EndsCount_Name", "(Close_Status=0)", "(EndsCount_IdNo = 0)")
    End Sub


    Private Sub cbo_Ends_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_GridEnds.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_GridEnds, Nothing, Nothing, "EndsCount_Head", "EndsCount_Name", "(Close_Status=0)", "(EndsCount_IdNo = 0)")
        With dgv_Details

            If (e.KeyValue = 38 And cbo_GridEnds.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_GridEnds.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
            End If

        End With


    End Sub

    Private Sub cbo_Ends_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_GridEnds.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_GridEnds, Nothing, "EndsCount_Head", "EndsCount_Name", "(Close_Status=0)", "(EndsCount_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            With dgv_Details

                .Focus()
                .Rows(.CurrentCell.RowIndex).Cells.Item(DGVCol_BobinDetails.Ends).Value = Trim(cbo_GridEnds.Text)
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

            End With

        End If


    End Sub


    Private Sub cbo_Ends_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_GridEnds.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New EndsCount_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_GridEnds.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_GridEnds_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_GridEnds.TextChanged
        Try
            If cbo_GridEnds.Visible Then
                With dgv_Details
                    If Val(cbo_GridEnds.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = DGVCol_BobinDetails.Ends Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_GridEnds.Text)
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


    ' Private Sub cbo_Grid_RateFor_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_RateFor.GotFocus
    'vCbo_ItmNm = Trim(cbo_Grid_RateFor.Text)
    '  End Sub

    Private Sub cbo_Grid_RateFor_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_RateFor.KeyDown
        Dim dep_idno As Integer = 0

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_RateFor, Nothing, Nothing, "", "", "", "")


        With dgv_Details

            If (e.KeyValue = 38 And cbo_Grid_RateFor.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                If Val(.CurrentCell.RowIndex) <= 0 Then
                    'cbo_Transport.Focus()
                    .Focus()
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentRow.Index - 1).Cells(.CurrentCell.ColumnIndex + 1)

                End If

            End If

            If (e.KeyValue = 40 And cbo_Grid_RateFor.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
            End If

        End With
    End Sub

    Private Sub cbo_Grid_RateFor_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_RateFor.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_RateFor, Nothing, "", "", "", "")

        If Asc(e.KeyChar) = 13 Then
            With dgv_Details

                .Focus()
                .Rows(.CurrentCell.RowIndex).Cells.Item(DGVCol_BobinDetails.RateFor).Value = Trim(cbo_Grid_RateFor.Text)
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

            End With

        End If

    End Sub

    Private Sub cbo_Grid_RateFor_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_RateFor.TextChanged
        Try
            If cbo_Grid_RateFor.Visible Then
                With dgv_Details
                    If Val(cbo_Grid_RateFor.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = DGVCol_BobinDetails.RateFor Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_RateFor.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_Grid_CountName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_CountName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_CountName, Nothing, Nothing, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
        With dgv_Details

            If (e.KeyValue = 38 And cbo_Grid_CountName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_Grid_CountName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
            End If

        End With
    End Sub

    Private Sub cbo_Grid_CountName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_CountName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_CountName, Nothing, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            With dgv_Details

                .Focus()
                .Rows(.CurrentCell.RowIndex).Cells.Item(DGVCol_BobinDetails.Count).Value = Trim(cbo_Grid_CountName.Text)
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

            End With

        End If
    End Sub

    Private Sub cbo_Grid_CountName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_CountName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Count_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_CountName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub


    Private Sub dgv_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim Dt4 As New DataTable
        Dim Dt5 As New DataTable
        Dim rect As Rectangle

        With dgv_Details

            If Val(.CurrentRow.Cells(DGVCol_BobinDetails.DC_No).Value) = 0 Then
                .CurrentRow.Cells(DGVCol_BobinDetails.SNO).Value = .CurrentRow.Index + 1
            End If


            If e.ColumnIndex = DGVCol_BobinDetails.Ends Then

                If cbo_GridEnds.Visible = False Or Val(cbo_GridEnds.Tag) <> e.RowIndex Then

                    'cbo_GridEnds.Tag = -1
                    'Da = New SqlClient.SqlDataAdapter("select EndsCount_Name from EndsCount_Head order by EndsCount_Name", con)
                    'Dt1 = New DataTable
                    'Da.Fill(Dt1)
                    'cbo_GridEnds.DataSource = Dt1
                    'cbo_GridEnds.DisplayMember = "EndsCount_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_GridEnds.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_GridEnds.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                    cbo_GridEnds.Width = rect.Width  ' .CurrentCell.Size.Width
                    cbo_GridEnds.Height = rect.Height  ' rect.Height

                    cbo_GridEnds.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_GridEnds.Tag = Val(e.RowIndex)
                    cbo_GridEnds.Visible = True

                    cbo_GridEnds.BringToFront()
                    cbo_GridEnds.Focus()

                End If

            Else

                cbo_GridEnds.Visible = False


            End If

            If e.ColumnIndex = DGVCol_BobinDetails.Count Then

                If cbo_Grid_CountName.Visible = False Or Val(cbo_Grid_CountName.Tag) <> e.RowIndex Then

                    'cbo_Grid_CountName.Tag = -1
                    'Da = New SqlClient.SqlDataAdapter("select Count_Name from Count_Head order by Count_Name", con)
                    'Dt2 = New DataTable
                    'Da.Fill(Dt2)
                    'cbo_Grid_CountName.DataSource = Dt2
                    'cbo_Grid_CountName.DisplayMember = "Count_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_CountName.Left = .Left + rect.Left
                    cbo_Grid_CountName.Top = .Top + rect.Top

                    cbo_Grid_CountName.Width = rect.Width
                    cbo_Grid_CountName.Height = rect.Height
                    cbo_Grid_CountName.Text = .CurrentCell.Value

                    cbo_Grid_CountName.Tag = Val(e.RowIndex)
                    cbo_Grid_CountName.Visible = True

                    cbo_Grid_CountName.BringToFront()
                    cbo_Grid_CountName.Focus()

                End If

            Else
                cbo_Grid_CountName.Visible = False

            End If


            If e.ColumnIndex = DGVCol_BobinDetails.Bobin_Size Then

                If cbo_Grid_BobinSize.Visible = False Or Val(cbo_Grid_BobinSize.Tag) <> e.RowIndex Then

                    cbo_Grid_BobinSize.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("SELECT Bobin_Size_Name FROM Bobin_Size_Head ORDER BY Bobin_Size_Name", con)
                    Dt3 = New DataTable
                    Da.Fill(Dt3)
                    cbo_Grid_BobinSize.DataSource = Dt3
                    cbo_Grid_BobinSize.DisplayMember = "Bobin_Size_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_BobinSize.Left = .Left + rect.Left
                    cbo_Grid_BobinSize.Top = .Top + rect.Top

                    cbo_Grid_BobinSize.Width = rect.Width
                    cbo_Grid_BobinSize.Height = rect.Height
                    cbo_Grid_BobinSize.Text = .CurrentCell.Value

                    cbo_Grid_BobinSize.Tag = Val(e.RowIndex)
                    cbo_Grid_BobinSize.Visible = True

                    cbo_Grid_BobinSize.BringToFront()
                    cbo_Grid_BobinSize.Focus()

                End If

            Else
                cbo_Grid_BobinSize.Visible = False

            End If


            If e.ColumnIndex = DGVCol_BobinDetails.RateFor Then

                If cbo_Grid_RateFor.Visible = False Or Val(cbo_Grid_RateFor.Tag) <> e.RowIndex Then

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_RateFor.Left = .Left + rect.Left
                    cbo_Grid_RateFor.Top = .Top + rect.Top

                    cbo_Grid_RateFor.Width = rect.Width
                    cbo_Grid_RateFor.Height = rect.Height
                    cbo_Grid_RateFor.Text = .CurrentCell.Value

                    cbo_Grid_RateFor.Tag = Val(e.RowIndex)
                    cbo_Grid_RateFor.Visible = True

                    cbo_Grid_RateFor.BringToFront()
                    cbo_Grid_RateFor.Focus()

                End If

            Else
                cbo_Grid_RateFor.Visible = False

            End If

        End With

    End Sub

    Private Sub dgv_Details_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Details.RowsAdded
        Dim n As Integer
        If FrmLdSTS = True Then
            Exit Sub
        End If
        With dgv_Details
            n = .RowCount
            .Rows(n - 1).Cells(DGVCol_BobinDetails.SNO).Value = Val(n)
        End With
    End Sub
    Private Sub btn_Print_Cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_Cancel.Click
        btn_print_Close_Click(sender, e)
    End Sub

    Private Sub btn_print_Close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Print.Click
        pnl_Back.Enabled = True
        pnl_Print.Visible = False
    End Sub

    Private Sub btn_Print_NonVatBill_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_NonVatBill.Click
        prn_Status = 1
        printing_invoice()
        btn_print_Close_Click(sender, e)
    End Sub

    Private Sub btn_Print_VatBill_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_VatBill.Click
        prn_Status = 2
        printing_invoice()
        btn_print_Close_Click(sender, e)
    End Sub

    Private Sub btn_Print_PrePrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_PrePrint.Click
        prn_Status = 3
        printing_invoice()
        btn_print_Close_Click(sender, e)
    End Sub

    Private Sub chk_NoAccountPosting_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles chk_NoAccountPosting.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()
            Else
                dtp_Date.Focus()
            End If
        End If
    End Sub
    Private Sub msk_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_Date.KeyDown
        vcbo_KeyDwnVal = e.KeyValue

        'If e.KeyCode = 40 Then
        '    e.Handled = True : e.SuppressKeyPress = True
        '    cbo_Ledger.Focus()
        'End If

        If e.KeyCode = 38 Then
            txt_Note.Focus()
        End If

        vmskOldText = ""
        vmskSelStrt = -1
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            vmskOldText = msk_Date.Text
            vmskSelStrt = msk_Date.SelectionStart
        End If

    End Sub

    Private Sub msk_Date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles msk_Date.KeyPress
        If Trim(UCase(e.KeyChar)) = "D" Then
            msk_Date.Text = Date.Today
            msk_Date.SelectionStart = 0
        End If
        'If Asc(e.KeyChar) = 13 Then
        '    e.Handled = True
        '    cbo_Ledger.Focus()
        'End If

    End Sub


    Private Sub msk_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_Date.KeyUp
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

    Private Sub dtp_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Date.KeyDown
        If e.KeyCode = 40 Then
            e.Handled = True : e.SuppressKeyPress = True
            msk_Date.Focus()
        End If

        If e.KeyCode = 38 Then
            e.Handled = True : e.SuppressKeyPress = True
            msk_Date.Focus()
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

    Private Sub btn_Tax_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Tax.Click
        pnl_Back.Enabled = False
        pnl_Tax.Visible = True
        pnl_Tax.Focus()
    End Sub

    Private Sub btn_Tax_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Tax_Close.Click
        pnl_Tax.Visible = False
        pnl_Back.Enabled = True

    End Sub





    Private Sub cbo_Ledger_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.LostFocus
        If Trim(UCase(cbo_Ledger.Tag)) <> Trim(UCase(cbo_Ledger.Text)) Then
            cbo_Ledger.Tag = cbo_Ledger.Text
            GST_Calculation()
        End If
    End Sub

    Private Sub cbo_PartyName_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbo_Ledger.SelectedIndexChanged
        Total_Calculation()
    End Sub

    Private Sub chk_GSTTax_Invocie_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chk_GSTTax_Invocie.CheckedChanged
        Total_Calculation()
    End Sub

    Private Sub GST_Calculation()
        Dim Ledger_State_Code As String = ""
        Dim Company_State_Code As String = ""
        Dim CGST_Per As Single = 0, SGST_Per As Single = 0, IGST_Per As Single = 0, GST_Per As Single = 0
        Dim HSN_Code As String = ""
        Dim Taxable_Amount As Double = 0
        Dim Led_IdNo As Integer = 0

        Try

            If FrmLdSTS = True Or NoCalc_Status = True Then Exit Sub

            With dgv_Details

                If dgv_Details.Rows.Count > 0 Then

                    For RowIndx = 0 To dgv_Details.Rows.Count - 1

                        ' .Rows(RowIndx).Cells(19).Value = ""
                        ' .Rows(RowIndx).Cells(20).Value = ""
                        ' .Rows(RowIndx).Cells(21).Value = ""
                        ' .Rows(RowIndx).Cells(22).Value = ""
                        .Rows(RowIndx).Cells(DGVCol_BobinDetails.Taxable_Amount).Value = ""  ' Taxable value
                        .Rows(RowIndx).Cells(DGVCol_BobinDetails.GST_Percentage).Value = ""  ' GST %
                        .Rows(RowIndx).Cells(DGVCol_BobinDetails.HSN_Code).Value = ""  ' HSN code

                        If Trim(.Rows(RowIndx).Cells(DGVCol_BobinDetails.DC_No).Value) <> "" Or Val(.Rows(RowIndx).Cells(DGVCol_BobinDetails.Count).Value) = 0 Or Val(.Rows(RowIndx).Cells(DGVCol_BobinDetails.Weight).Value) = 0 Or Val(.Rows(RowIndx).Cells(DGVCol_BobinDetails.Amount).Value) = 0 Then

                            HSN_Code = ""
                            GST_Per = 0
                            Get_GST_Percentage_From_ClothGroup(Trim(.Rows(RowIndx).Cells(DGVCol_BobinDetails.Ends).Value), Trim(.Rows(RowIndx).Cells(DGVCol_BobinDetails.Count).Value), HSN_Code, GST_Per)

                            'CGST_Per = GST_Per / 2
                            'SGST_Per = GST_Per / 2
                            'IGST_Per = GST_Per


                            '--Cash discount
                            .Rows(RowIndx).Cells(DGVCol_BobinDetails.Discount_Percentage).Value = Format(Val(txt_DiscPerc.Text), "########0.00")
                            .Rows(RowIndx).Cells(DGVCol_BobinDetails.Discount_Value).Value = Format(Val(.Rows(RowIndx).Cells(DGVCol_BobinDetails.Amount).Value) * (Val(.Rows(RowIndx).Cells(DGVCol_BobinDetails.Discount_Percentage).Value) / 100), "########0.00")

                            '-- Taxable value = amount -  cash disc
                            Taxable_Amount = Val(.Rows(RowIndx).Cells(DGVCol_BobinDetails.Amount).Value) - Val(.Rows(RowIndx).Cells(DGVCol_BobinDetails.Discount_Value).Value)

                            .Rows(RowIndx).Cells(DGVCol_BobinDetails.Taxable_Amount).Value = Format(Val(Taxable_Amount), "##########0.00")
                            .Rows(RowIndx).Cells(DGVCol_BobinDetails.GST_Percentage).Value = Format(Val(GST_Per), "########0.00")
                            .Rows(RowIndx).Cells(DGVCol_BobinDetails.HSN_Code).Value = Trim(HSN_Code)

                        End If

                    Next RowIndx

                    Get_HSN_CodeWise_Tax_Details()

                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT DO GST CALCULATION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Get_HSN_CodeWise_Tax_Details()
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim Sno As Integer = 0
        Dim n As Integer = 0
        Dim Led_IdNo As Integer = 0
        Dim AssVal_Pack_Frgt_Ins_Amt As String = ""
        Dim InterStateStatus As Boolean = False

        Try

            If FrmLdSTS = True Or NoCalc_Status = True Then Exit Sub

            cmd.Connection = con

            cmd.CommandText = "Truncate table " & Trim(Common_Procedures.EntryTempTable) & ""
            cmd.ExecuteNonQuery()

            If chk_GSTTax_Invocie.Checked = True Then

                AssVal_Pack_Frgt_Ins_Amt = Format(Val(txt_Packing.Text) + Val(txt_AddLess.Text), "#########0.00")

                With dgv_Details

                    If .Rows.Count > 0 Then
                        For i = 0 To .Rows.Count - 1
                            If Trim(.Rows(i).Cells(DGVCol_BobinDetails.DC_No).Value) <> "" And Val(.Rows(i).Cells(DGVCol_BobinDetails.Taxable_Amount).Value) <> 0 And Trim(.Rows(i).Cells(DGVCol_BobinDetails.HSN_Code).Value) <> "" Then
                                cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempTable) & " (                    Name1                            ,                  Currency1                                            ,                       Currency2                                                                             ) " & _
                                "Values                   ( '" & Trim(.Rows(i).Cells(DGVCol_BobinDetails.HSN_Code).Value) & "' , " & Val(.Rows(i).Cells(DGVCol_BobinDetails.GST_Percentage).Value) & " ,  " & Str(Val(.Rows(i).Cells(DGVCol_BobinDetails.Taxable_Amount).Value) + Val(AssVal_Pack_Frgt_Ins_Amt)) & " ) "
                                cmd.ExecuteNonQuery()

                                AssVal_Pack_Frgt_Ins_Amt = 0

                            End If
                        Next

                    End If

                End With

            End If


            With dgv_Tax_Details

                .Rows.Clear()
                Sno = 0

                da = New SqlClient.SqlDataAdapter("select Name1 as HSN_Code, Currency1 as GST_Percentage, sum(Currency2) as TaxableAmount from " & Trim(Common_Procedures.EntryTempTable) & " group by name1, Currency1 Having sum(Currency2) <> 0 order by Name1, Currency1", con)
                dt = New DataTable
                da.Fill(dt)

                If dt.Rows.Count > 0 Then

                    Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
                    InterStateStatus = Common_Procedures.Is_InterState_Party(con, Val(lbl_Company.Tag), Led_IdNo)

                    For i = 0 To dt.Rows.Count - 1

                        n = .Rows.Add()

                        Sno = Sno + 1

                        .Rows(n).Cells(0).Value = Sno
                        .Rows(n).Cells(1).Value = dt.Rows(i).Item("HSN_Code").ToString

                        .Rows(n).Cells(2).Value = Format(Val(dt.Rows(i).Item("TaxableAmount").ToString), "############0.00")
                        If Val(.Rows(n).Cells(2).Value) = 0 Then .Rows(n).Cells(2).Value = ""

                        If InterStateStatus = True Then
                            .Rows(n).Cells(7).Value = Format(Val(dt.Rows(i).Item("GST_Percentage").ToString), "#############0.00")
                            If Val(.Rows(n).Cells(7).Value) = 0 Then .Rows(n).Cells(7).Value = ""

                        Else

                            .Rows(n).Cells(3).Value = Format(Val(dt.Rows(i).Item("GST_Percentage").ToString) / 2, "############0.00")
                            If Val(.Rows(n).Cells(3).Value) = 0 Then .Rows(n).Cells(3).Value = ""

                            .Rows(n).Cells(5).Value = Format(Val(dt.Rows(i).Item("GST_Percentage").ToString) / 2, "############0.00")
                            If Val(.Rows(n).Cells(5).Value) = 0 Then .Rows(n).Cells(5).Value = ""

                        End If

                        .Rows(n).Cells(4).Value = Format(Val(.Rows(n).Cells(2).Value) * Val(.Rows(n).Cells(3).Value) / 100, "#############0.00")
                        If Val(.Rows(n).Cells(4).Value) = 0 Then .Rows(n).Cells(4).Value = ""

                        .Rows(n).Cells(6).Value = Format(Val(.Rows(n).Cells(2).Value) * Val(.Rows(n).Cells(5).Value) / 100, "#############0.00")
                        If Val(.Rows(n).Cells(6).Value) = 0 Then .Rows(n).Cells(6).Value = ""

                        .Rows(n).Cells(8).Value = Format(Val(.Rows(n).Cells(2).Value) * Val(.Rows(n).Cells(7).Value) / 100, "#############0.00")
                        If Val(.Rows(n).Cells(8).Value) = 0 Then .Rows(n).Cells(8).Value = ""

                    Next

                End If
                dt.Clear()

                dt.Dispose()
                da.Dispose()

            End With

            Total_Tax_Calculation()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            dt.Dispose()
            da.Dispose()

        End Try
    End Sub

    Private Sub Total_Tax_Calculation()
        Dim Sno As Integer
        Dim TotAss_Val As Single
        Dim TotCGST_amt As Single
        Dim TotSGST_amt As Double
        Dim TotIGST_amt As Double

        If FrmLdSTS = True Or NoCalc_Status = True Then Exit Sub

        Sno = 0
        TotAss_Val = 0 : TotCGST_amt = 0 : TotSGST_amt = 0 : TotIGST_amt = 0

        With dgv_Tax_Details
            For i = 0 To .RowCount - 1
                Sno = Sno + 1
                .Rows(i).Cells(0).Value = Sno
                If Val(.Rows(i).Cells(2).Value) <> 0 Then

                    TotAss_Val = TotAss_Val + Val(.Rows(i).Cells(2).Value())
                    TotCGST_amt = TotCGST_amt + Val(.Rows(i).Cells(4).Value())
                    TotSGST_amt = TotSGST_amt + Val(.Rows(i).Cells(6).Value())
                    TotIGST_amt = TotIGST_amt + Val(.Rows(i).Cells(8).Value())


                End If

            Next i

        End With



        With dgv_Tax_Total_Details
            If .RowCount = 0 Then .Rows.Add()

            .Rows(0).Cells(2).Value = Format(Val(TotAss_Val), "##########0.00")
            .Rows(0).Cells(4).Value = Format(Val(TotCGST_amt), "##########0.00")
            .Rows(0).Cells(6).Value = Format(Val(TotSGST_amt), "##########0.00")
            .Rows(0).Cells(8).Value = Format(Val(TotIGST_amt), "##########0.00")

        End With

        lbl_TaxableValue.Text = Format(Val(TotAss_Val), "##########0.00")
        lbl_CGST_Amount.Text = IIf(Val(TotCGST_amt) <> 0, Format(Val(TotCGST_amt), "##########0.00"), "")
        lbl_SGST_Amount.Text = IIf(Val(TotSGST_amt) <> 0, Format(Val(TotSGST_amt), "##########0.00"), "")
        lbl_IGST_Amount.Text = IIf(Val(TotIGST_amt) <> 0, Format(Val(TotIGST_amt), "##########0.00"), "")

    End Sub
    Private Sub Get_GST_Percentage_From_ClothGroup(ByVal EndsCount As String, ByVal ClothName As String, ByRef HSN_Code As String, ByRef GST_PerCent As Single)
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        Try

            HSN_Code = ""
            GST_PerCent = 0

            If Trim(ClothName) <> "" Then
                da = New SqlClient.SqlDataAdapter("select a.* from ItemGroup_Head a INNER JOIN Count_Head b ON a.ItemGroup_IdNo = b.ItemGroup_IdNo Where b.Count_Name ='" & Trim(ClothName) & "'", con)
                dt = New DataTable
                da.Fill(dt)

                If dt.Rows.Count > 0 Then
                    If IsDBNull(dt.Rows(0).Item("Item_HSN_Code").ToString) = False Then
                        HSN_Code = Trim(dt.Rows(0).Item("Item_HSN_Code").ToString)
                    End If
                    If IsDBNull(dt.Rows(0).Item("Item_GST_Percentage").ToString) = False Then
                        GST_PerCent = Val(dt.Rows(0).Item("Item_GST_Percentage").ToString)
                    End If
                End If
                dt.Clear()
                dt.Dispose()
                da.Dispose()

            ElseIf Trim(EndsCount) <> "" Then
                da = New SqlClient.SqlDataAdapter("select a.* from ItemGroup_Head a INNER JOIN EndsCount_Head b ON a.ItemGroup_IdNo = b.ItemGroup_IdNo Where b.EndsCount_Name ='" & Trim(EndsCount) & "'", con)
                dt = New DataTable
                da.Fill(dt)

                If dt.Rows.Count > 0 Then
                    If IsDBNull(dt.Rows(0).Item("Item_HSN_Code").ToString) = False Then
                        HSN_Code = Trim(dt.Rows(0).Item("Item_HSN_Code").ToString)
                    End If
                    If IsDBNull(dt.Rows(0).Item("Item_GST_Percentage").ToString) = False Then
                        GST_PerCent = Val(dt.Rows(0).Item("Item_GST_Percentage").ToString)
                    End If
                End If
                dt.Clear()
                dt.Dispose()
                da.Dispose()
            End If





        Catch ex As Exception
            '---MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            dt.Dispose()
            da.Dispose()

        End Try
    End Sub

    Private Sub dgtxt_Details_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.TextChanged
        Try
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells(dgv_Details.CurrentCell.ColumnIndex).Value = Trim(dgtxt_Details.Text)

            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Printing_GST_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim cmd As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim EntryCode As String
        Dim NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim LnAr(15) As Single, ClArr(15) As Single
        'Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim W1 As Single = 0
        Dim SNo As Integer
        Dim vMtrs_ForPrint As String = ""
        Dim vRate_ForPrint As String = ""

        Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8X12", 800, 1200)
        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
        PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1

        'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
        '        ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '        PrintDocument1.DefaultPageSettings.PaperSize = ps
        '        e.PageSettings.PaperSize = ps
        '        PpSzSTS = True
        '        Exit For
        '    End If
        'Next

        'If PpSzSTS = False Then
        '    For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '        If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
        '            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '            PrintDocument1.DefaultPageSettings.PaperSize = ps
        '            e.PageSettings.PaperSize = ps
        '            Exit For
        '        End If
        '    Next
        'End If

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 10
            .Right = 40
            .Top = 40
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

        NoofItems_PerPage = 8  '8 ' 6

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = 50 : ClArr(2) = 210 : ClArr(3) = 80 : ClArr(4) = 60 : ClArr(5) = 95 : ClArr(6) = 80 : ClArr(7) = 70
        ClArr(8) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7))

        'ClArr(1) = 50 : ClArr(2) = 260 : ClArr(3) = 80 : ClArr(4) = 60 : ClArr(5) = 80 : ClArr(6) = 100
        'ClArr(7) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6))

        'ClArr(1) = Val(50) : ClArr(2) = 60 : ClArr(3) = 220 : ClArr(4) = 130 : ClArr(5) = 130
        'ClArr(6) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5))

        TxtHgt = 18.5 ' 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        EntryCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_GST_Format1_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                ' W1 = e.Graphics.MeasureString("No.of Beams  : ", pFont).Width

                NoofDets = 0

                CurY = CurY - 10

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then
                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_GST_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

                            e.HasMorePages = True
                            Return

                        End If

                        prn_DetSNo = prn_DetSNo + 1

                        'ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Cloth_Description").ToString)
                        'If Trim(ItmNm1) = "" Then
                        '    ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("cloth_Name").ToString)
                        'End If
                        'ItmNm2 = ""
                        'If Len(ItmNm1) > 45 Then
                        '    For I = 45 To 1 Step -1
                        '        If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                        '    Next I
                        '    If I = 0 Then I = 45
                        '    ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                        '    ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                        'End If

                        CurY = CurY + TxtHgt

                        SNo = SNo + 1
                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(SNo)), LMargin + 10, CurY, 0, 0, pFont)

                        If prn_DetDt.Rows(prn_DetIndx).Item("EndsCount_Name").ToString <> "" Then
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("EndsCount_Name").ToString, LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                        Else
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Count_Name").ToString, LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                        End If
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("HSN_Code").ToString, LMargin + ClArr(1) + ClArr(2) + 5, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("GST_Percentage").ToString) & " %", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 5, CurY, 1, 0, pFont)

                        vMtrs_ForPrint = Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Meters").ToString), "#########0.00")
                        vRate_ForPrint = Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Rate").ToString), "#########0.00")
                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Ledger_IdNo").ToString) = 308 Then
                            vMtrs_ForPrint = Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Meters").ToString) * 2, "#########0.00")
                            If Trim(UCase(prn_DetDt.Rows(prn_DetIndx).Item("Rate_For").ToString)) = "MTRS" Then
                                vRate_ForPrint = Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Amount").ToString) / Val(vMtrs_ForPrint), "#########0.00")
                            End If
                        End If

                        Common_Procedures.Print_To_PrintDocument(e, IIf(Val(vMtrs_ForPrint) <> 0, Format(Val(vMtrs_ForPrint), "#########0.00"), ""), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 5, CurY, 1, 0, pFont)

                        Common_Procedures.Print_To_PrintDocument(e, IIf(Val(prn_DetDt.Rows(prn_DetIndx).Item("Weights").ToString) <> 0, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Weights").ToString), "#########0.000"), ""), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 5, CurY, 1, 0, pFont)

                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(vRate_ForPrint), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 5, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Amount").ToString), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 5, CurY, 1, 0, pFont)

                        NoofDets = NoofDets + 1

                        'If Trim(ItmNm2) <> "" Then
                        '    CurY = CurY + TxtHgt - 5
                        '    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                        '    NoofDets = NoofDets + 1
                        'End If


                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If


                Printing_GST_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)

            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_GST_Format1_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim strHeight As Single
        Dim C1 As Single, W1 As Single, S1 As Single
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
        Dim Cmp_State As String, Cmp_StateCode As String, Cmp_GSTIN_No As String, Cmp_PanNo As String, Cmp_EMail As String
        Dim NewCode As String
        Dim CurY1 As Single = 0

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        PageNo = PageNo + 1

        Cmp_Name = ""

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("Select a.* , B.ENDSCOUNT_Name , C.COUNT_Name from Proforma_BobinSales_Invoice_Details a LEFT OUTER JOIN ENDSCOUNT_Head b oN A.EndsCount_Idno = B.EndsCount_Idno LEFT OUTER JOIN COUNT_Head C oN A.Count_Idno = C.Count_Idno  Where a.BobinSales_Invoice_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
        da2.Fill(dt2)

        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        dt2.Clear()

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_PanNo = "" : Cmp_EMail = ""
        Cmp_State = "" : Cmp_StateCode = "" : Cmp_GSTIN_No = ""

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
        If Trim(prn_HdDt.Rows(0).Item("State_Name").ToString) <> "" Then
            Cmp_State = "STATE : " & prn_HdDt.Rows(0).Item("State_Name").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("State_Code").ToString) <> "" Then
            Cmp_StateCode = "CODE :" & prn_HdDt.Rows(0).Item("State_Code").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString) <> "" Then
            Cmp_GSTIN_No = "GSTIN :" & prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
        End If
        CurY = CurY + TxtHgt - 10

        If Trim(Common_Procedures.settings.CustomerCode) = "1116" Then
            e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Lourdu_matha_tex_logo, Drawing.Image), LMargin + 10, CurY, 110, 110)
        End If
        p1Font = New Font("Calibri", 18, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)

        If Cmp_State <> "" Then
            CurY = CurY + TxtHgt - 1
            Common_Procedures.Print_To_PrintDocument(e, Cmp_State & "  " & Cmp_StateCode, LMargin, CurY, 2, PrintWidth, pFont)
        End If

        If Cmp_GSTIN_No <> "" Then
            CurY = CurY + TxtHgt - 1
            Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, LMargin, CurY, 2, PrintWidth, pFont)
        End If


        CurY = CurY + TxtHgt
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "PROFORMA INVOICE", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        If Cmp_PhNo <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin + 10, CurY, 0, 0, pFont)
        End If
        If Cmp_EMail <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, Cmp_EMail, PrintWidth - 2, CurY, 1, 0, pFont)
        End If


        CurY = CurY + strHeight
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try
            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4)
            W1 = e.Graphics.MeasureString("TRANSPORT  ", pFont).Width
            S1 = e.Graphics.MeasureString("TO     :    ", pFont).Width

            CurY1 = CurY

            'left side

            CurY = CurY + TxtHgt - 10
            p1Font = New Font("Calibri", 12, FontStyle.Bold Or FontStyle.Underline)
            Common_Procedures.Print_To_PrintDocument(e, "BILLED TO : ", LMargin + 10, CurY, 0, 0, p1Font)
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            CurY = CurY + TxtHgt + 5
            Common_Procedures.Print_To_PrintDocument(e, "M/s." & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + S1 + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 14, FontStyle.Bold)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("Ledger_PhoneNo").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, " PHONE : " & prn_HdDt.Rows(0).Item("Ledger_PhoneNo").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            End If
            CurY = CurY + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString & "  CODE : " & prn_HdDt.Rows(0).Item("Ledger_State_Code").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, " GSTIN : " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            End If


            'right side
            CurY1 = CurY1 + TxtHgt - 10
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "INVOICE NO", LMargin + C1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("BobinSales_Invoice_No").ToString, LMargin + C1 + W1 + 30, CurY1, 0, 0, p1Font)

            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "DATE ", LMargin + C1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("BobinDelivery_Invoice_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY1, 0, 0, pFont)

            CurY1 = CurY1 + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY1, PageWidth, CurY1)
            LnAr(3) = CurY1

            CurY1 = CurY1 - 15
            If Trim(prn_HdDt.Rows(0).Item("Po_No").ToString) <> "" Then
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "ORDER NO", LMargin + C1 + 10, CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Po_No").ToString, LMargin + C1 + W1 + 30, CurY1, 0, 0, pFont)
            End If
            If Trim(prn_HdDt.Rows(0).Item("Po_Date").ToString) <> "" Then
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "ORDER DATE", LMargin + C1 + 10, CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, (Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Po_Date").ToString), "dd-MM-yyyy").ToString), LMargin + C1 + W1 + 30, CurY1, 0, 0, pFont)
            End If
            If Trim(prn_HdDt.Rows(0).Item("TransportName").ToString) <> "" Then
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "TRNASPORT", LMargin + C1 + 10, CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("TransportName").ToString, LMargin + C1 + W1 + 30, CurY1, 0, 0, pFont)
            End If
            If Trim(prn_HdDt.Rows(0).Item("Lr_No").ToString) <> "" Then
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "LR. NO.", LMargin + C1 + 10, CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Lr_No").ToString, LMargin + C1 + W1 + 30, CurY1, 0, 0, pFont)
            End If

            If CurY1 > CurY Then CurY = CurY1


            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PARTICULARS", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "HSN CODE", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "GST %", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "METER", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "RATE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_GST_Format1_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As SqlClient.SqlDataAdapter
        Dim Dt1 As DataTable
        Dim p1Font As Font
        Dim I As Integer
        Dim Cmp_Name As String
        Dim W1 As Single = 0
        Dim BmsInWrds As String
        Dim vprn_BlNos As String = ""
        Dim BankNm1 As String = ""
        Dim BankNm2 As String = ""
        Dim BankNm3 As String = ""
        Dim BankNm4 As String = ""
        Dim BInc As Integer
        Dim BnkDetAr() As String
        Dim Amt_OpBal As Single
        Dim Cmp_Cond As String
        Dim Emp_Bob As Integer, EmpBob_Par As Integer
        Dim CurY1 As Single = 0
        Dim CHk_Details_Cnt As Integer = 0

        ' W1 = e.Graphics.MeasureString("No.of Beams  : ", pFont).Width

        Try

            CHk_Details_Cnt = Get_HSN_CODE_Count()

            For I = NoofDets + 1 To NoofItems_PerPage

                CurY = CurY + TxtHgt

                prn_DetIndx = prn_DetIndx + 1

            Next

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Total_Meters").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Total_Weights").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Total_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)


            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(7) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(2))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(4))

            Erase BnkDetAr
            If Trim(prn_HdDt.Rows(0).Item("Company_Bank_Ac_Details").ToString) <> "" Then
                BnkDetAr = Split(Trim(prn_HdDt.Rows(0).Item("Company_Bank_Ac_Details").ToString), ",")

                BInc = -1

                BInc = BInc + 1
                If UBound(BnkDetAr) >= BInc Then
                    BankNm1 = Trim(BnkDetAr(BInc))
                End If

                BInc = BInc + 1
                If UBound(BnkDetAr) >= BInc Then
                    BankNm2 = Trim(BnkDetAr(BInc))
                End If

                BInc = BInc + 1
                If UBound(BnkDetAr) >= BInc Then
                    BankNm3 = Trim(BnkDetAr(BInc))
                End If

                BInc = BInc + 1
                If UBound(BnkDetAr) >= BInc Then
                    BankNm4 = Trim(BnkDetAr(BInc))
                End If

            End If

            '----   Opening Balance for Amount

            Cmp_Cond = ""
            If Val(Common_Procedures.settings.EntrySelection_Combine_AllCompany) = 0 Then
                Cmp_Cond = Cmp_Cond & IIf(Cmp_Cond <> "", " and", "") & " a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and "
            End If

            cmd.Connection = con

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@CompFromDate", Common_Procedures.Company_FromDate)
            cmd.Parameters.AddWithValue("@SalesDate", prn_HdDt.Rows(0).Item("BobinDelivery_Invoice_Date"))

            Amt_OpBal = 0

            cmd.CommandText = "select sum(a.voucher_amount) as Op_Balance from voucher_details a where " & Cmp_Cond & " a.ledger_idno = " & Str(Val(prn_HdDt.Rows(0).Item("Ledger_IdNo"))) & " and a.voucher_date < @CompFromDate"
            Da = New SqlClient.SqlDataAdapter(cmd)
            Dt1 = New DataTable
            Da.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                If IsDBNull(Dt1.Rows(0).Item("Op_Balance").ToString) = False Then Amt_OpBal = -1 * Val(Dt1.Rows(0).Item("Op_Balance").ToString)
            End If
            Dt1.Clear()

            cmd.CommandText = "select sum(a.voucher_amount) as Op_Balance from voucher_details a, voucher_head b where " & Cmp_Cond & " a.ledger_idno = " & Str(Val(prn_HdDt.Rows(0).Item("Ledger_IdNo"))) & " and a.voucher_date between @CompFromDate and @SalesDate and ( b.entry_identification NOT LIKE '" & Trim(Pk_Condition) & "%' or b.entry_identification is Null ) and a.voucher_code = b.voucher_code and a.company_idno = b.company_idno"
            Da = New SqlClient.SqlDataAdapter(cmd)
            Dt1 = New DataTable
            Da.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                If IsDBNull(Dt1.Rows(0).Item("Op_Balance").ToString) = False Then Amt_OpBal = Amt_OpBal - Val(Dt1.Rows(0).Item("Op_Balance").ToString)
            End If
            Dt1.Clear()

            cmd.CommandText = "select sum(a.net_amount) as Inv_OpBalance from Proforma_BobinSales_Invoice_Head a Where " & Cmp_Cond & " a.ledger_idno = " & Str(Val(prn_HdDt.Rows(0).Item("Ledger_IdNo"))) & " and ( (a.BobinDelivery_Invoice_Date >= @CompFromDate and a.BobinDelivery_Invoice_Date < @SalesDate) or ( a.BobinDelivery_Invoice_Date = @SalesDate and a.for_orderby < " & Str(Val(prn_HdDt.Rows(0).Item("for_orderby"))) & " ) ) "
            Da = New SqlClient.SqlDataAdapter(cmd)
            Dt1 = New DataTable
            Da.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                If IsDBNull(Dt1.Rows(0).Item("Inv_OpBalance").ToString) = False Then Amt_OpBal = Amt_OpBal + Val(Dt1.Rows(0).Item("Inv_OpBalance").ToString)
            End If
            Dt1.Clear()

            Cmp_Cond = ""
            If Trim(UCase(Common_Procedures.User.Type)) <> "UNACCOUNT" Then
                Cmp_Cond = "Company_Type <> 'UNACCOUNT'"
            End If
            If Val(Common_Procedures.settings.EntrySelection_Combine_AllCompany) = 0 Then
                Cmp_Cond = Cmp_Cond & IIf(Cmp_Cond <> "", " and", "") & " a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & "  "
            End If

            cmd.CommandText = "Truncate table " & Trim(Common_Procedures.ReportTempSubTable) & ""
            cmd.ExecuteNonQuery()

            '---Opening

            cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempSubTable) & "(int1) Select sum(Empty_Bobin) from Stock_Empty_BeamBagCone_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON a.DeliveryTo_Idno <> 0 and a.DeliveryTo_Idno = tP.Ledger_IdNo Where " & Cmp_Cond & IIf(Trim(Cmp_Cond) <> "", " and ", "") & " a.DeliveryTo_Idno = " & Str(Val(prn_HdDt.Rows(0).Item("Ledger_IdNo").ToString)) & " and a.Reference_Date <= @SalesDate and Empty_Bobin <> 0"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempSubTable) & "(int1) Select -1*sum(Empty_Bobin) from Stock_Empty_BeamBagCone_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON a.ReceivedFrom_Idno <> 0 and a.ReceivedFrom_Idno = tP.Ledger_IdNo Where " & Cmp_Cond & IIf(Trim(Cmp_Cond) <> "", " and ", "") & " a.ReceivedFrom_Idno = " & Str(Val(prn_HdDt.Rows(0).Item("Ledger_IdNo").ToString)) & " and a.Reference_Date <= @SalesDate and Empty_Bobin <> 0"
            cmd.ExecuteNonQuery()


            cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempSubTable) & "(int2) Select sum(EmptyBobin_Party) from Stock_Empty_BeamBagCone_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON a.ReceivedFrom_Idno <> 0 and a.ReceivedFrom_Idno = tP.Ledger_IdNo Where " & Cmp_Cond & IIf(Trim(Cmp_Cond) <> "", " and ", "") & " a.ReceivedFrom_Idno = " & Str(Val(prn_HdDt.Rows(0).Item("Ledger_IdNo").ToString)) & " and a.Reference_Date <= @SalesDate and EmptyBobin_Party <> 0"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempSubTable) & "(int2) Select -1*sum(EmptyBobin_Party) from Stock_Empty_BeamBagCone_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON a.DeliveryTo_Idno <> 0 and a.DeliveryTo_Idno = tP.Ledger_IdNo Where " & Cmp_Cond & IIf(Trim(Cmp_Cond) <> "", " and ", "") & " a.DeliveryTo_Idno = " & Str(Val(prn_HdDt.Rows(0).Item("Ledger_IdNo").ToString)) & " and a.Reference_Date <= @SalesDate and EmptyBobin_Party <> 0"
            cmd.ExecuteNonQuery()


            Emp_Bob = 0
            EmpBob_Par = 0
            Da = New SqlClient.SqlDataAdapter("select sum(int1) as Empty_Bobin, sum(int2) as EmptyBobin_Party from " & Trim(Common_Procedures.ReportTempSubTable) & "", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                If IsDBNull(Dt1.Rows(0).Item("Empty_Bobin").ToString) = False Then Emp_Bob = Val(Dt1.Rows(0).Item("Empty_Bobin").ToString)
                If IsDBNull(Dt1.Rows(0).Item("EmptyBobin_Party").ToString) = False Then EmpBob_Par = Val(Dt1.Rows(0).Item("EmptyBobin_Party").ToString)
            End If
            Dt1.Clear()

            'CurY = CurY + TxtHgt
            'p1Font = New Font("Calibri", 11, FontStyle.Bold)
            'Common_Procedures.Print_To_PrintDocument(e, BankNm1, LMargin + 10, CurY, 0, 0, p1Font)
            'If Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) <> 0 Then
            '    Common_Procedures.Print_To_PrintDocument(e, "Frieght", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Freight_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            'End If

            CurY1 = CurY
            'Left Side

            '  CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "Our Bobin  (With Party) : " & Trim(Val(Emp_Bob)), LMargin + 10, CurY, 0, 0, pFont)

            '   CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "Party Bobin (In Godown) : " & Trim(Val(EmpBob_Par)), LMargin + 10, CurY, 0, 0, pFont)

            ' CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, BankNm1, LMargin + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, BankNm2, LMargin + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, BankNm3, LMargin + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, BankNm4, LMargin + 10, CurY, 0, 0, p1Font)

            'Right side

            'CurY1 = CurY1 - 10
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            If Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString) <> 0 Then
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Discount " & Trim(prn_HdDt.Rows(0).Item("Discount_Percentage").ToString) & " %" & " (-) ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY1, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("Discount_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY1, 1, 0, pFont)
            End If

            If Val(prn_HdDt.Rows(0).Item("Packing_Amount").ToString) <> 0 Then
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Packing (+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY1, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("Packing_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY1, 1, 0, pFont)
            End If

            If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "AddLess " & IIf(Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) > 0, "(+)", "(-)"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY1, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY1, 1, 0, pFont)
            End If

            If Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("Packing_Amount").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then
                CurY1 = CurY1 + TxtHgt
                e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY1, PageWidth, CurY1)
                LnAr(8) = CurY1
            End If

            CurY1 = CurY1 - 10
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            If Val(prn_HdDt.Rows(0).Item("Total_Taxable_Value").ToString) <> 0 Then
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Taxable Value", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY1, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("Total_Taxable_Value").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY1, 1, 0, pFont)
            End If

            If Val(prn_HdDt.Rows(0).Item("Total_CGST_Amount").ToString) <> 0 Then
                CurY1 = CurY1 + TxtHgt

                If CHk_Details_Cnt = 1 Then
                    Common_Procedures.Print_To_PrintDocument(e, "CGST " & Val(prn_DetDt.Rows(0).Item("GST_Percentage").ToString) / 2 & " % (+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY1, 1, 0, pFont)
                Else
                    Common_Procedures.Print_To_PrintDocument(e, "CGST (+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY1, 1, 0, pFont)
                End If
                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Total_CGST_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY1, 1, 0, pFont)
            End If
            If Val(prn_HdDt.Rows(0).Item("Total_SGST_Amount").ToString) <> 0 Then
                CurY1 = CurY1 + TxtHgt

                If CHk_Details_Cnt = 1 Then

                    Common_Procedures.Print_To_PrintDocument(e, "SGST " & Val(prn_DetDt.Rows(0).Item("GST_Percentage").ToString) / 2 & " % (+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY1, 1, 0, pFont)

                Else
                    Common_Procedures.Print_To_PrintDocument(e, "SGST (+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY1, 1, 0, pFont)

                End If
                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Total_SGST_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY1, 1, 0, pFont)
            End If
            If Val(prn_HdDt.Rows(0).Item("Total_IGST_Amount").ToString) <> 0 Then
                CurY1 = CurY1 + TxtHgt

                If CHk_Details_Cnt = 1 Then
                    Common_Procedures.Print_To_PrintDocument(e, "IGST " & Val(prn_DetDt.Rows(0).Item("GST_Percentage").ToString) & " % (+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY1, 1, 0, pFont)
                Else
                    Common_Procedures.Print_To_PrintDocument(e, "IGST (+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY1, 1, 0, pFont)
                End If
                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Total_IGST_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY1, 1, 0, pFont)
            End If

            CurY1 = CurY1 + TxtHgt
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            If Val(prn_HdDt.Rows(0).Item("RoundOff_Amount").ToString) <> 0 Then

                Common_Procedures.Print_To_PrintDocument(e, "RoundOff " & IIf(Val(prn_HdDt.Rows(0).Item("RoundOff_Amount").ToString) > 0, "(+)", "(-)"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY1, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("RoundOff_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY1, 1, 0, pFont)
            End If


            If CurY1 > CurY Then CurY = CurY1

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, PageWidth, CurY)
            LnAr(8) = CurY

            CurY = CurY + TxtHgt - 10
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Net Amount", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, " " & Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(7))
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(9) = CurY
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(4))

            CurY = CurY + TxtHgt - 10

            BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))
            BmsInWrds = Replace(Trim(LCase(BmsInWrds)), "", "")

            BmsInWrds = Replace(BmsInWrds, LCase(Microsoft.VisualBasic.Left(BmsInWrds, 1)), UCase(Microsoft.VisualBasic.Left(BmsInWrds, 1)), 1, 1)

            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Amount Chargeable (In words) : " & BmsInWrds & " ", LMargin + 10, CurY, 0, 0, p1Font)
            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(10) = CurY

            '=========


            Printing_GST_HSN_Details_Format1(e, CurY, LMargin, PageWidth, PrintWidth, LnAr(10))

            '=====
            CurY = CurY + 10
            p1Font = New Font("Calibri", 10, FontStyle.Underline Or FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Terms & Conditions : ", LMargin + 10, CurY, 0, 0, p1Font)

            p1Font = New Font("Calibri", 9, FontStyle.Regular)
            CurY = CurY + TxtHgt + 5
            Common_Procedures.Print_To_PrintDocument(e, "1.Goods Once sold will not be refundable", LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "2.Our responsibility ceases as soon as the goods leave our premises", PageWidth / 2, CurY, 0, 0, p1Font)


            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "3.Interest will be charged at 21% from the date of purchase", LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "4.Payment by Cheques subject to realisation", PageWidth / 2, CurY, 0, 0, p1Font)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "5.Subject to Tirupur jurisdiction ", LMargin + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(10) = CurY

            'If Val(Common_Procedures.User.IdNo) <> 1 Then
            '    Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
            'End If

            CurY = CurY + TxtHgt - 10
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

            W1 = e.Graphics.MeasureString("Previous Balance  : ", pFont).Width

            'CurY = CurY + TxtHgt

            'If is_LastPage = True Then
            '    PreBal = Amt_OpBal
            '    Common_Procedures.Print_To_PrintDocument(e, "Previous Balance", LMargin + 15, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 15, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, Trim(Format(Val(PreBal), "#########0.00")), LMargin + W1 + 110, CurY, 1, 0, pFont)
            'End If

            CurY = CurY + TxtHgt + 5
            'If is_LastPage = True Then
            '    NetBal = Amt_OpBal + Val(prn_HdDt.Rows(0).Item("Net_amount").ToString)
            '    Common_Procedures.Print_To_PrintDocument(e, "Net Balance", LMargin + 15, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 15, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, Trim(Format(Val(NetBal), "#########0.00")), LMargin + W1 + 110, CurY, 1, 0, pFont)
            'End If

            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            'CurY = CurY + TxtHgt
            'CurY = CurY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 20, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 350, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            'Common_Procedures.Print_To_PrintDocument(e, "For ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 30, CurY, 1, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "AUTHORISED SIGNATORY ", PageWidth - 5, CurY, 1, 0, pFont)
            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Function Get_HSN_CODE_Count() As Integer
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim NewCode As String = ""
        Dim Cnt As Integer = 0
        Try
            NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            da = New SqlClient.SqlDataAdapter("select count(HSN_Code) as HsnCnt from Proforma_BobinSales_Invoice_GST_Tax_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and BobinSales_Invoice_Code = '" & Trim(NewCode) & "' ", con)
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    Cnt = Trim(dt.Rows(0)(0).ToString)
                End If
            End If

            dt.Clear()
            dt.Dispose()
            da.Dispose()


        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        Get_HSN_CODE_Count = Cnt

    End Function

    Private Sub dtp_Date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_Date.KeyPress
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            msk_Date.Focus()
        End If
    End Sub

    Private Sub btn_Pdf_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Pdf.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        Print_PDF_Status = True
        print_record()
    End Sub
    Private Sub Printing_GST_HSN_Details_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByRef CurY As Single, ByVal LMargin As Integer, ByVal PageWidth As Integer, ByVal PrintWidth As Double, ByVal LnAr As Single)
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim EntryCode As String
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font, p1Font As Font
        Dim TxtHgt As Single
        Dim SubClAr(15) As Single
        Dim ItmNm1 As String, ItmNm2 As String
        Dim SNo As Integer = 0
        Dim NoofItems_Increment As Integer
        Dim Ttl_TaxAmt As Double, Ttl_CGst As Double, Ttl_Sgst As Double, Ttl_igst As Double
        Dim LnAr2 As Single
        Dim BmsInWrds As String
        Dim Cnt As Integer = 0
        Try
            pFont = New Font("Calibri", 10, FontStyle.Regular)


            NoofItems_PerPage = 3 ' 5

            Ttl_TaxAmt = 0 : Ttl_CGst = 0 : Ttl_Sgst = 0

            Erase SubClAr

            SubClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

            SubClAr(1) = 140 : SubClAr(2) = 130 : SubClAr(3) = 60 : SubClAr(4) = 95 : SubClAr(5) = 60 : SubClAr(6) = 90 : SubClAr(7) = 60
            SubClAr(8) = PageWidth - (LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7))

            TxtHgt = 18.75 ' 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20


            Common_Procedures.Print_To_PrintDocument(e, "HSN/SAC", LMargin, CurY + 5, 2, SubClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "TAXABLE AMOUNT", LMargin + SubClAr(1), CurY + 5, 2, SubClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "CGST", LMargin + SubClAr(1) + SubClAr(2), CurY, 2, SubClAr(3) + SubClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "SGST", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4), CurY, 2, SubClAr(5) + SubClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "IGST", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6), CurY, 2, SubClAr(7) + SubClAr(8), pFont)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + SubClAr(1) + SubClAr(2), CurY, PageWidth, CurY)
            LnAr2 = CurY
            CurY = CurY + 5
            Common_Procedures.Print_To_PrintDocument(e, "%", LMargin + SubClAr(1) + SubClAr(2), CurY, 2, SubClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Amount", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3), CurY, 2, SubClAr(4), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "%", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4), CurY, 2, SubClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Amount", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5), CurY, 2, SubClAr(6), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "%", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6), CurY, 2, SubClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Amount", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7), CurY, 2, SubClAr(8), pFont)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            EntryCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)


            Da = New SqlClient.SqlDataAdapter("select * from Proforma_BobinSales_Invoice_GST_Tax_Details where BobinSales_Invoice_Code = '" & Trim(EntryCode) & "'", con)
            Dt = New DataTable
            Da.Fill(Dt)

            If Dt.Rows.Count > 0 Then

                Cnt = Dt.Rows.Count

                prn_DetIndx = 0
                NoofDets = 0
                NoofItems_Increment = 0
                CurY = CurY - 20

                Do While prn_DetIndx <= Dt.Rows.Count - 1

                    ItmNm1 = Trim(Dt.Rows(prn_DetIndx).Item("HSN_Code").ToString)

                    ItmNm2 = ""
                    If Len(ItmNm1) > 40 Then
                        For I = 35 To 1 Step -1
                            If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                        Next I
                        If I = 0 Then I = 40
                        ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                        ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                    End If



                    CurY = CurY + TxtHgt + 3

                    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Dt.Rows(prn_DetIndx).Item("Taxable_Amount").ToString) <> 0, Common_Procedures.Currency_Format(Val(Dt.Rows(prn_DetIndx).Item("Taxable_Amount").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Dt.Rows(prn_DetIndx).Item("CGST_Percentage").ToString) <> 0, Common_Procedures.Currency_Format(Val(Dt.Rows(prn_DetIndx).Item("CGST_Percentage").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Dt.Rows(prn_DetIndx).Item("CGST_Amount").ToString) <> 0, Common_Procedures.Currency_Format(Val(Dt.Rows(prn_DetIndx).Item("CGST_Amount").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) - 5, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Dt.Rows(prn_DetIndx).Item("SGST_Percentage").ToString) <> 0, Common_Procedures.Currency_Format(Val(Dt.Rows(prn_DetIndx).Item("SGST_Percentage").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Dt.Rows(prn_DetIndx).Item("SGST_Amount").ToString) <> 0, Common_Procedures.Currency_Format(Val(Dt.Rows(prn_DetIndx).Item("SGST_Amount").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) - 5, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Dt.Rows(prn_DetIndx).Item("IGST_Percentage").ToString) <> 0, Common_Procedures.Currency_Format(Val(Dt.Rows(prn_DetIndx).Item("IGST_Percentage").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Dt.Rows(prn_DetIndx).Item("IGST_Amount").ToString) <> 0, Common_Procedures.Currency_Format(Val(Dt.Rows(prn_DetIndx).Item("IGST_Amount").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8) - 5, CurY, 1, 0, pFont)

                    NoofItems_Increment = NoofItems_Increment + 1

                    NoofDets = NoofDets + 1



                    Ttl_TaxAmt = Ttl_TaxAmt + Val(Dt.Rows(prn_DetIndx).Item("Taxable_Amount").ToString)
                    Ttl_CGst = Ttl_CGst + Val(Dt.Rows(prn_DetIndx).Item("CGST_Amount").ToString)
                    Ttl_Sgst = Ttl_Sgst + Val(Dt.Rows(prn_DetIndx).Item("SGST_Amount").ToString)
                    Ttl_igst = Ttl_igst + Val(Dt.Rows(prn_DetIndx).Item("IGST_Amount").ToString)
                    prn_DetIndx = prn_DetIndx + 1
                Loop

            End If

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            CurY = CurY + TxtHgt - 15
            Common_Procedures.Print_To_PrintDocument(e, "Total", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Ttl_TaxAmt) <> 0, Common_Procedures.Currency_Format(Val(Ttl_TaxAmt)), ""), LMargin + SubClAr(1) + SubClAr(2) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Ttl_CGst) <> 0, Common_Procedures.Currency_Format(Val(Ttl_CGst)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) - 5, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Ttl_Sgst) <> 0, Common_Procedures.Currency_Format(Val(Ttl_Sgst)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) - 5, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Ttl_igst) <> 0, Common_Procedures.Currency_Format(Val(Ttl_igst)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8) - 5, CurY, 1, 0, pFont)
            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            e.Graphics.DrawLine(Pens.Black, LMargin + SubClAr(1), CurY, LMargin + SubClAr(1), LnAr)
            e.Graphics.DrawLine(Pens.Black, LMargin + SubClAr(1) + SubClAr(2), CurY, LMargin + SubClAr(1) + SubClAr(2), LnAr)
            e.Graphics.DrawLine(Pens.Black, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3), LnAr2)
            e.Graphics.DrawLine(Pens.Black, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4), LnAr)
            e.Graphics.DrawLine(Pens.Black, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5), LnAr2)

            e.Graphics.DrawLine(Pens.Black, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6), LnAr)
            e.Graphics.DrawLine(Pens.Black, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7), LnAr2)


            CurY = CurY + 5
            p1Font = New Font("Calibri", 12, FontStyle.Regular)
            BmsInWrds = ""
            If (Val(Ttl_CGst) + Val(Ttl_Sgst) + Val(Ttl_igst)) <> 0 Then
                BmsInWrds = Common_Procedures.Rupees_Converstion(Val(Ttl_CGst) + Val(Ttl_Sgst) + Val(Ttl_igst))
                BmsInWrds = Replace(Trim(BmsInWrds), "", "")
            End If

            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Tax Amount(In Words) : " & BmsInWrds & " ", LMargin + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)


            If Cnt = 1 Then
                CurY = CurY + TxtHgt
                CurY = CurY + TxtHgt
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try



    End Sub

    Private Sub msk_PoDate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_PoDate.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        vmskOrdText = ""
        vmskOrdStrt = -1
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            vmskOrdText = msk_PoDate.Text
            vmskOrdStrt = msk_PoDate.SelectionStart
        End If

    End Sub

    Private Sub msk_PoDate_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_PoDate.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            msk_PoDate.Text = Date.Today
            msk_PoDate.SelectionStart = msk_PoDate.Text.Length
        End If
        If IsDate(msk_PoDate.Text) = True Then
            If e.KeyCode = 107 Then
                msk_PoDate.Text = DateAdd("D", 1, Convert.ToDateTime(msk_PoDate.Text))
            ElseIf e.KeyCode = 109 Then
                msk_PoDate.Text = DateAdd("D", -1, Convert.ToDateTime(msk_PoDate.Text))
            End If
        End If
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            Common_Procedures.maskEdit_Date_ON_DelBackSpace(sender, e, vmskOrdText, vmskOrdStrt)
        End If
    End Sub

    Private Sub dtp_FromDate_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        msk_PoDate.Text = dtp_PoDate.Text
    End Sub

    Private Sub dtp_FromDate_Enter(ByVal sender As Object, ByVal e As System.EventArgs)
        msk_PoDate.Focus()
        msk_PoDate.SelectionStart = 0
    End Sub

    Private Sub dtp_PoDate_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_PoDate.Enter
        msk_PoDate.Focus()
        msk_PoDate.SelectionStart = 0
    End Sub

    Private Sub dtp_PoDate_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_PoDate.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            dtp_PoDate.Text = Date.Today
        End If
    End Sub

    Private Sub dtp_PoDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_PoDate.TextChanged
        If IsDate(dtp_PoDate.Text) = True Then

            msk_PoDate.Text = dtp_PoDate.Text
            msk_PoDate.SelectionStart = 0
        End If
    End Sub

    Private Sub dtp_PoDate_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_PoDate.ValueChanged
        msk_PoDate.Text = dtp_PoDate.Text
    End Sub

    Private Sub cbo_Grid_CountName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_CountName.TextChanged
        Try
            If cbo_Grid_CountName.Visible Then
                With dgv_Details
                    If Val(cbo_Grid_CountName.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = DGVCol_BobinDetails.Count Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_CountName.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Grid_BobinSize_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_BobinSize.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Bobin_Size_Head", "Bobin_Size_Name", "", "(Bobin_Size_IdNo = 0)")
    End Sub

    Private Sub cbo_Grid_BobinSize_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_BobinSize.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_BobinSize, Nothing, Nothing, "Bobin_Size_Head", "Bobin_Size_Name", "", "(Bobin_Size_IdNo = 0)")
        With dgv_Details
            If e.KeyCode = 38 And cbo_Grid_BobinSize.DroppedDown = False Or (e.Control = True And e.KeyCode = 38) Then
                If .CurrentCell.ColumnIndex <= 1 Then
                    cbo_Transport.Focus()
                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
                End If
            End If
            If e.KeyCode = 40 And cbo_Grid_BobinSize.DroppedDown = False Or (e.Control = True And e.KeyCode = 40) Then
                If .CurrentCell.ColumnIndex >= 1 And .Rows(.CurrentRow.Index).Cells.Item(.CurrentCell.ColumnIndex).Value = "" Then
                    txt_DiscPerc.Focus()
                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
                End If
            End If
        End With
    End Sub

    Private Sub cbo_Grid_BobinSize_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_BobinSize.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_BobinSize, Nothing, "Bobin_Size_Head", "Bobin_Size_Name", "", "(Bobin_Size_IdNo = 0)")
        With dgv_Details
            If Asc(e.KeyChar) = 13 Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

            End If
        End With
    End Sub

    Private Sub cbo_Grid_BobinSize_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_BobinSize.KeyUp
        If e.KeyCode = 17 And e.Control = False Then

            Dim F As New Bobin_Size_Creation
            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_BobinSize.Name
            Common_Procedures.Master_Return.Master_Type = ""
            Common_Procedures.Master_Return.Return_Value = ""

            F.MdiParent = MDIParent1
            F.Show()
        End If
    End Sub

    Private Sub cbo_Grid_BobinSize_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_BobinSize.TextChanged
        Try
            With dgv_Details
                If cbo_Grid_BobinSize.Visible = True Then
                    If Val(cbo_Grid_BobinSize.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = DGVCol_BobinDetails.Bobin_Size Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_BobinSize.Text)
                    End If
                End If
            End With
        Catch ex As Exception

        End Try
    End Sub
End Class