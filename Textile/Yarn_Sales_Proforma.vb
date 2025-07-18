Public Class Yarn_Sales_Proforma
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "YNPSL-"
    Private Pk_Condition2 As String = "YSAGC-"
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
    Private prn_Status As Integer = 0
    Private prn_InpOpts As String = ""
    Private prn_OriDupTri As String = ""
    Private prn_Count As Integer

    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        clear()
    End Sub

    Private Sub clear()

        NoCalc_Status = True
        chk_Verified_Status.Checked = False

        New_Entry = False
        Insert_Entry = False

        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        pnl_Selection.Visible = False
        lbl_InvoiceNo.Text = ""
        lbl_InvoiceNo.ForeColor = Color.Black

        msk_date.Text = ""
        dtp_Date.Text = ""
        cbo_PartyName.Text = ""

        cbo_Agent.Text = ""
        Cbo_SalesAc.Text = ""
        txt_Address2.Text = ""

        txt_Address3.Text = ""

        cbo_VehicleNo.Text = ""
        txt_Address1.Text = ""
        txt_CommRate.Text = ""
        cbo_CommType.Text = "BAG"
        txt_CommAmount.Text = ""

        lbl_GrossAmount.Text = ""

        txt_DiscPerc.Text = ""
        lbl_DiscAmount.Text = ""

        lbl_AssessableValue.Text = ""

        cbo_VatAc.Text = ""

        cbo_TaxType.Text = "-NIL-"
        txt_TaxPerc.Text = ""
        lbl_TaxAmount.Text = ""

        txt_Freight_Name.Text = "Freight"
        txt_FreightAmount.Text = ""
        txt_Packing_Name.Text = "Packing"
        txt_Packing.Text = ""
        txt_AddLessAmount.Text = "Add/Less"
        txt_AddLessAmount.Text = ""
        lbl_RoundOff.Text = ""
        lbl_NetAmount.Text = "0.00"
        lbl_AmountInWords.Text = "Rupees  :  "

        cbo_Transport.Text = ""
        txt_YarnDescription.Text = ""
        txt_LrNo.Text = ""
        msk_Lr_Date.Text = ""

        txt_Chess_Name.Text = "Cess"
        txt_ChessPerc.Text = ""
        lbl_ChessAmount.Text = ""
        txt_ExtraTax_Name.Text = "Extra Tax"
        txt_ExtraTaxPerc.Text = ""
        lbl_ExtraTax_Amount.Text = ""

        txt_DespatchFrom.Text = ""
        txt_DespatchTo.Text = ""
        txt_LotNo.Text = ""
        txt_LC_No.Text = ""
        msk_LC_Date.Text = ""
        dtp_LC_Date.Text = ""
        txt_OrderNo.Text = ""
        msk_OrderDate.Text = ""
        dtp_OrderDate.Text = ""
        cbo_Type.Text = "DIRECT"
        txt_DateAndTime_Invoice.Text = ""
        txt_TimeofRemoval_Goods.Text = ""

        txt_Terms_Details1.Text = ""
        txt_Terms_Details2.Text = ""
        txt_Terms_Details3.Text = ""
        txt_Terms_Details4.Text = ""
        txt_Terms_Details5.Text = ""
        txt_Terms_Details6.Text = ""
        txt_Terms_Details7.Text = ""

        txt_labelCaption1.Text = ""
        txt_labelCaption2.Text = ""
        txt_LabelCaption3.Text = ""
        txt_LabelCaption4.Text = ""
        txt_LabelCaption5.Text = ""
        txt_LabelCaption6.Text = ""
        txt_LabelCaption7.Text = ""

        chk_C_FormSales.Checked = False
        cbo_SalesType.Text = ""

        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))

     
        dgv_Details.Rows.Clear()
        dgv_Details_Total.Rows.Clear()
        dgv_Details_Total.Rows.Add()

        If Filter_Status = False Then
            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_CountName.Text = ""
            txt_Address1.Text = ""
            cbo_Filter_CountName.SelectedIndex = -1
            cbo_Filter_PartyName.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()
        End If

        Grid_Cell_DeSelect()

        cbo_Grid_CountName.Visible = False
        cbo_Grid_MillName.Visible = False

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
            Msktxbx.SelectAll()
        End If

        If Me.ActiveControl.Name <> cbo_Grid_CountName.Name Then
            cbo_Grid_CountName.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Grid_MillName.Name Then
            cbo_Grid_MillName.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Grid_RateFor.Name Then
            cbo_Grid_RateFor.Visible = False
        End If

        Grid_Cell_DeSelect()

        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        On Error Resume Next
        If IsNothing(Prec_ActCtrl) = False Then
            Prec_ActCtrl.BackColor = Color.White
            Prec_ActCtrl.ForeColor = Color.Black
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
        Dim I As Integer

        If Val(no) = 0 Then Exit Sub

        clear()

        NoCalc_Status = True

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.* from Yarn_Sales_Prroforma_Head a Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Yarn_Sales_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_InvoiceNo.Text = dt1.Rows(0).Item("Yarn_Sales_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Yarn_Sales_Date").ToString
                msk_date.Text = dtp_Date.Text
                cbo_Type.Text = dt1.Rows(0).Item("Entry_Type").ToString
                cbo_PartyName.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Ledger_IdNo").ToString))
                Cbo_SalesAc.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("PurchaseAc_IdNo").ToString))
                txt_Address1.Text = dt1.Rows(0).Item("Delivery_Address1").ToString
                txt_Address2.Text = dt1.Rows(0).Item("Delivery_Address2").ToString
                txt_Address3.Text = dt1.Rows(0).Item("Delivery_Address3").ToString
                cbo_Agent.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Agent_IdNo").ToString))


                txt_CommRate.Text = Val(dt1.Rows(0).Item("Agent_Commission_Rate").ToString)
                cbo_CommType.Text = dt1.Rows(0).Item("Agent_Commission_Type").ToString
                txt_CommAmount.Text = dt1.Rows(0).Item("Agent_Commission_Commission").ToString
                txt_YarnDescription.Text = dt1.Rows(0).Item("Yarn_Description").ToString
                cbo_Transport.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Transport_IdNo").ToString))
                cbo_VehicleNo.Text = dt1.Rows(0).Item("Vehicle_No").ToString

                lbl_GrossAmount.Text = Format(Val(dt1.Rows(0).Item("Total_Amount").ToString), "#########0.00")
                txt_DiscPerc.Text = Val(dt1.Rows(0).Item("Discount_Percentage").ToString)
                lbl_DiscAmount.Text = Format(Val(dt1.Rows(0).Item("Discount_Amount").ToString), "#########0.00")
                lbl_AssessableValue.Text = Format(Val(dt1.Rows(0).Item("Assessable_Value").ToString), "#########0.00")
                cbo_TaxType.Text = dt1.Rows(0).Item("Tax_Type").ToString
                If Trim(cbo_TaxType.Text) = "" Then cbo_TaxType.Text = "-NIL-"
                txt_TaxPerc.Text = Val(dt1.Rows(0).Item("Tax_Percentage").ToString)
                lbl_TaxAmount.Text = Format(Val(dt1.Rows(0).Item("Tax_Amount").ToString), "#########0.00")

                If IsDBNull(dt1.Rows(0).Item("Freight_Name").ToString) = False Then
                    If Trim(dt1.Rows(0).Item("Freight_Name").ToString) <> "" Then
                        txt_Freight_Name.Text = dt1.Rows(0).Item("Freight_Name").ToString
                    End If
                End If
                txt_FreightAmount.Text = Format(Val(dt1.Rows(0).Item("Freight_Amount").ToString), "#########0.00")
                If IsDBNull(dt1.Rows(0).Item("Packing_Name").ToString) = False Then
                    If Trim(dt1.Rows(0).Item("Packing_Name").ToString) <> "" Then
                        txt_Packing_Name.Text = dt1.Rows(0).Item("Packing_Name").ToString
                    End If
                End If
                txt_Packing.Text = Format(Val(dt1.Rows(0).Item("Packing_Amount").ToString), "#########0.00")
                If IsDBNull(dt1.Rows(0).Item("AddLess_Name").ToString) = False Then
                    If Trim(dt1.Rows(0).Item("AddLess_Name").ToString) <> "" Then
                        txt_AddLess_Name.Text = dt1.Rows(0).Item("AddLess_Name").ToString
                    End If
                End If
                txt_AddLessAmount.Text = Format(Val(dt1.Rows(0).Item("AddLess_Amount").ToString), "#########0.00")
                lbl_RoundOff.Text = Format(Val(dt1.Rows(0).Item("RoundOff_Amount").ToString), "#########0.00")
                lbl_NetAmount.Text = Common_Procedures.Currency_Format(Val(dt1.Rows(0).Item("Net_Amount").ToString))

                cbo_VatAc.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("TaxAc_IdNo").ToString))

                txt_LotNo.Text = Trim(dt1.Rows(0).Item("Lot_No").ToString)
                txt_LrNo.Text = dt1.Rows(0).Item("Lr_No").ToString
                msk_Lr_Date.Text = dt1.Rows(0).Item("Lr_Date").ToString

                If IsDBNull(dt1.Rows(0).Item("Chess_Name").ToString) = False Then
                    If Trim(dt1.Rows(0).Item("Chess_Name").ToString) <> "" Then
                        txt_Chess_Name.Text = dt1.Rows(0).Item("Chess_Name").ToString
                    End If
                End If

                txt_ChessPerc.Text = Val(dt1.Rows(0).Item("Chess_Percentage").ToString)
                lbl_ChessAmount.Text = Format(Val(dt1.Rows(0).Item("Chess_Amount").ToString), "#########0.00")

                If IsDBNull(dt1.Rows(0).Item("ExtraTax_Name").ToString) = False Then
                    If Trim(dt1.Rows(0).Item("ExtraTax_Name").ToString) <> "" Then
                        txt_ExtraTax_Name.Text = dt1.Rows(0).Item("ExtraTax_Name").ToString
                    End If
                End If
                txt_ExtraTaxPerc.Text = Val(dt1.Rows(0).Item("ExtraTax_Percentage").ToString)
                lbl_ExtraTax_Amount.Text = Format(Val(dt1.Rows(0).Item("ExtraTax_Amount").ToString), "#########0.00")
                lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))

                msk_LC_Date.Text = dt1.Rows(0).Item("LC_Date").ToString


                msk_OrderDate.Text = dt1.Rows(0).Item("Order_Date").ToString


                txt_LC_No.Text = dt1.Rows(0).Item("LC_No").ToString
                txt_OrderNo.Text = dt1.Rows(0).Item("Order_No").ToString
                txt_DespatchFrom.Text = dt1.Rows(0).Item("Despatch_From").ToString
                txt_DespatchTo.Text = dt1.Rows(0).Item("Despatch_To").ToString
                cbo_Through.Text = dt1.Rows(0).Item("Through_Name").ToString
                txt_DateAndTime_Invoice.Text = dt1.Rows(0).Item("Date_of_Inv_Preparation").ToString
                txt_TimeofRemoval_Goods.Text = dt1.Rows(0).Item("Time_Of_Removal_Of_Goods").ToString

                txt_Terms_Details1.Text = dt1.Rows(0).Item("Terms_Details1").ToString
                txt_Terms_Details2.Text = dt1.Rows(0).Item("Terms_Details2").ToString
                txt_Terms_Details3.Text = dt1.Rows(0).Item("Terms_Details3").ToString
                txt_Terms_Details4.Text = dt1.Rows(0).Item("Terms_Details4").ToString
                txt_Terms_Details5.Text = dt1.Rows(0).Item("Terms_Details5").ToString
                txt_Terms_Details6.Text = dt1.Rows(0).Item("Terms_Details6").ToString
                txt_Terms_Details7.Text = dt1.Rows(0).Item("Terms_Details7").ToString

                txt_labelCaption1.Text = dt1.Rows(0).Item("Terms_Label1").ToString
                txt_labelCaption2.Text = dt1.Rows(0).Item("Terms_Label2").ToString
                txt_LabelCaption3.Text = dt1.Rows(0).Item("Terms_Label3").ToString
                txt_LabelCaption4.Text = dt1.Rows(0).Item("Terms_Label4").ToString
                txt_LabelCaption5.Text = dt1.Rows(0).Item("Terms_Label5").ToString
                txt_LabelCaption6.Text = dt1.Rows(0).Item("Terms_Label6").ToString
                txt_LabelCaption7.Text = dt1.Rows(0).Item("Terms_Label7").ToString

                cbo_SalesType.Text = dt1.Rows(0).Item("Sale_Type").ToString
                chk_C_FormSales.Checked = IIf(Val(dt1.Rows(0).Item("C_Form_Sales_Status").ToString) = 1, True, False)
                cbo_PackingType.Text = dt1.Rows(0).Item("Packing_Type").ToString

                If Val(dt1.Rows(0).Item("Verified_Status").ToString) = 1 Then chk_Verified_Status.Checked = True


                da2 = New SqlClient.SqlDataAdapter("Select a.*, b.Mill_Name, c.Count_name from Yarn_Sales_Proforma_Details a INNER JOIN Mill_Head b ON a.Mill_IdNo = b.Mill_IdNo INNER JOIN Count_Head c ON a.Count_IdNo = c.Count_IdNo Where a.Yarn_Sales_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                With dgv_Details

                    .Rows.Clear()
                    SNo = 0

                    If dt2.Rows.Count > 0 Then

                        For i = 0 To dt2.Rows.Count - 1

                            n = .Rows.Add()

                            SNo = SNo + 1

                            .Rows(n).Cells(0).Value = Val(SNo)
                            .Rows(n).Cells(1).Value = dt2.Rows(i).Item("Count_Name").ToString
                            .Rows(n).Cells(2).Value = dt2.Rows(i).Item("Mill_Name").ToString
                            .Rows(n).Cells(3).Value = Val(dt2.Rows(i).Item("Bags").ToString)
                            .Rows(n).Cells(4).Value = Val(dt2.Rows(i).Item("Cones").ToString)
                            .Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("Weight").ToString), "########0.000")
                            .Rows(n).Cells(6).Value = dt2.Rows(i).Item("Rate_For").ToString
                            .Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Rate").ToString), "########0.00")
                            .Rows(n).Cells(8).Value = Format(Val(dt2.Rows(i).Item("Amount").ToString), "########0.00")
                            .Rows(n).Cells(9).Value = dt2.Rows(I).Item("Bag_No").ToString
                            .Rows(n).Cells(10).Value = dt2.Rows(I).Item("Delivery_No").ToString
                            .Rows(n).Cells(11).Value = dt2.Rows(I).Item("Weaver_Sales_Yarn_Delivery_Code").ToString
                            .Rows(n).Cells(12).Value = Val(dt2.Rows(I).Item("Weaver_Sales_Yarn_Delivery_Detail_SlNo").ToString)
                        Next i

                    End If

                    If .RowCount = 0 Then .Rows.Add()

                    For I = 0 To .Rows.Count - 1
                        .Rows(I).Cells(0).Value = I + 1
                    Next

                End With

                With dgv_Details_Total
                    If .RowCount = 0 Then .Rows.Add()
                    .Rows(0).Cells(3).Value = Val(dt1.Rows(0).Item("Total_Bags").ToString)
                    .Rows(0).Cells(4).Value = Val(dt1.Rows(0).Item("Total_Cones").ToString)
                    .Rows(0).Cells(5).Value = Format(Val(dt1.Rows(0).Item("Total_Weight").ToString), "########0.00")
                    .Rows(0).Cells(8).Value = Format(Val(dt1.Rows(0).Item("Total_Amount").ToString), "########0.00")
                End With


            End If

            Grid_Cell_DeSelect()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            dt1.Dispose()
            da1.Dispose()

            dt2.Dispose()
            da2.Dispose()

            If msk_date.Visible And msk_date.Enabled Then msk_date.Focus()

        End Try

        NoCalc_Status = False

    End Sub

    Private Sub Yarn_Sales_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Activated

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_PartyName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_PartyName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Agent.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "AGENT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Agent.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(Cbo_SalesAc.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                Cbo_SalesAc.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_VatAc.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_VatAc.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_CountName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "COUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_CountName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_MillName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "MILL" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_MillName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Transport.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Transport.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

    Private Sub Yarn_Sales_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
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
        cbo_CommType.Items.Clear()
        cbo_CommType.Items.Add("BAG")
        cbo_CommType.Items.Add("%")

        cbo_Grid_RateFor.Items.Clear()
        cbo_Grid_RateFor.Items.Add("BAG")
        cbo_Grid_RateFor.Items.Add("KG")

        cbo_TaxType.Items.Clear()
        cbo_TaxType.Items.Add("-NIL-")
        cbo_TaxType.Items.Add("VAT")
        cbo_TaxType.Items.Add("CST")

        cbo_Type.Items.Clear()
        'cbo_Type.Items.Add("")
        cbo_Type.Items.Add("DIRECT")
        ' cbo_Type.Items.Add("DELIVERY")

        cbo_Through.Items.Clear()
        cbo_Through.Items.Add(" ")
        cbo_Through.Items.Add("DIRECT")
        cbo_Through.Items.Add("BANK")
        cbo_Through.Items.Add("AGENT")

        cbo_SalesType.Items.Clear()
        'cbo_SalesType.Items.Add("V.A.T.SALE")
        'cbo_SalesType.Items.Add("C.S.T.SALE")
        cbo_SalesType.Items.Add("E1.SALE")

        cbo_PackingType.Items.Clear()
        cbo_PackingType.Items.Add("BAG")
        cbo_PackingType.Items.Add("CHIPPAM")


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


        pnl_Terms.Visible = False
        pnl_Terms.Left = (Me.Width - pnl_Terms.Width) \ 2
        pnl_Terms.Top = (Me.Height - pnl_Terms.Height) \ 2
        pnl_Terms.BringToFront()


        chk_Verified_Status.Visible = False
        If Common_Procedures.settings.Vefified_Status = 1 Then
            If Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_Verified_Status = 1 Then chk_Verified_Status.Visible = True
        End If




        btn_UserModification.Visible = False
        If Common_Procedures.settings.User_Modifications_Show_Status = 1 Then
            If Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_UserModification_Status = 1 Then
                btn_UserModification.Visible = True
            End If
        End If

        If Common_Procedures.settings.CustomerCode = "1333" Then
            Label1.Text = "PROFORMA INVOICE"

        End If


        AddHandler chk_Verified_Status.GotFocus, AddressOf ControlGotFocus
        AddHandler chk_Verified_Status.LostFocus, AddressOf ControlLostFocus



        AddHandler msk_date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Agent.GotFocus, AddressOf ControlGotFocus
        AddHandler Cbo_SalesAc.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Type.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_CountName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_MillName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_RateFor.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_VehicleNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Through.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_Lr_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_LrNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_LC_No.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_OrderNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DespatchTo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DespatchFrom.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_LC_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_OrderDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_SalesType.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Terms_Details1.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Terms_Details2.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Terms_Details3.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Terms_Details4.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Terms_Details5.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Terms_Details6.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Terms_Details7.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_labelCaption1.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_labelCaption2.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_LabelCaption3.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_LabelCaption4.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_LabelCaption5.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_LabelCaption6.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_LabelCaption7.GotFocus, AddressOf ControlGotFocus


        AddHandler txt_Address1.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Address2.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Address3.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_CommRate.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_CommAmount.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DiscPerc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_TaxPerc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Freight_Name.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_FreightAmount.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Packing_Name.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Packing.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_AddLess_Name.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_AddLessAmount.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_TaxType.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_VatAc.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Transport.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_YarnDescription.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_Fromdate.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_ToDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_CountName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Chess_Name.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_ExtraTax_Name.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_ChessPerc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_ExtraTaxPerc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DateAndTime_Invoice.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_TimeofRemoval_Goods.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_LotNo.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_ChessPerc.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_ExtraTaxPerc.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_Lr_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_LrNo.LostFocus, AddressOf ControlLostFocus


        AddHandler msk_date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Through.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Type.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Agent.LostFocus, AddressOf ControlLostFocus
        AddHandler Cbo_SalesAc.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_CountName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_MillName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_RateFor.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_VehicleNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Address1.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Address2.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Address3.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_CommAmount.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_CommRate.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DiscPerc.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_TaxType.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_TaxPerc.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Freight_Name.LostFocus, AddressOf ControlLostFocus1
        AddHandler txt_FreightAmount.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Packing_Name.LostFocus, AddressOf ControlLostFocus1
        AddHandler txt_Packing.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_AddLess_Name.LostFocus, AddressOf ControlLostFocus1
        AddHandler txt_AddLessAmount.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_VatAc.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Transport.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_YarnDescription.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_Fromdate.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_ToDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_CountName.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Chess_Name.LostFocus, AddressOf ControlLostFocus1
        AddHandler txt_ExtraTax_Name.LostFocus, AddressOf ControlLostFocus1
        AddHandler txt_DateAndTime_Invoice.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_TimeofRemoval_Goods.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_LC_No.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_OrderNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DespatchTo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DespatchFrom.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_LC_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_OrderDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_SalesType.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Terms_Details1.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Terms_Details2.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Terms_Details3.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Terms_Details4.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Terms_Details5.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Terms_Details6.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Terms_Details7.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_labelCaption1.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_labelCaption2.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_LabelCaption3.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_LabelCaption4.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_LabelCaption5.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_LabelCaption6.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_LabelCaption7.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_LotNo.LostFocus, AddressOf ControlLostFocus


        'AddHandler msk_date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Address1.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Address2.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Address3.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_TaxPerc.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Packing_Name.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Packing.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Freight_Name.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_FreightAmount.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_AddLess_Name.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_YarnDescription.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Chess_Name.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_ChessPerc.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_ExtraTax_Name.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_ExtraTaxPerc.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Terms_Details1.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Terms_Details2.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Terms_Details3.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Terms_Details4.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Terms_Details5.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Terms_Details6.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Terms_Details7.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_LotNo.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler msk_date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Address1.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Address2.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Address3.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler txt_Chess_Name.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_ChessPerc.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_ExtraTax_Name.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_ExtraTaxPerc.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler txt_DiscPerc.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_TaxPerc.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Freight_Name.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_FreightAmount.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Packing_Name.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Packing.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_AddLess_Name.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_Fromdate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_ToDate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Terms_Details1.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Terms_Details2.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Terms_Details3.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Terms_Details4.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Terms_Details5.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Terms_Details6.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Terms_Details7.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_LotNo.KeyPress, AddressOf TextBoxControlKeyPress

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1118" Then '---- Kasthuri Textiles (COIMBATORE)
            cbo_PackingType.Visible = True
            lbl_PackingType.Visible = True
            txt_TimeofRemoval_Goods.Visible = False
            txt_DateAndTime_Invoice.Visible = False
            lbl_TimeOfRemoval.Visible = False
            lbl_TimeOfInvoie.Visible = False
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1111" Then '---- VELAN SPINNING MILLS
            chk_C_FormSales.Text = "Form- F"
        Else
            chk_C_FormSales.Text = "C Form Sales"
        End If

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True
        new_record()

    End Sub

    Private Sub Yarn_Sales_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub Yarn_Sales_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress

        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
                    Exit Sub

                ElseIf pnl_Print.Visible = True Then
                    btn_print_Close_Click(sender, e)
                    Exit Sub


                ElseIf pnl_Selection.Visible = True Then
                    btn_Close_Selection_Click(sender, e)
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

            Else
                dgv1 = dgv_Details

            End If

            With dgv1

                If keyData = Keys.Enter Or keyData = Keys.Down Then

                    If .CurrentCell.ColumnIndex >= .ColumnCount - 4 Then
                        If .CurrentCell.RowIndex = .RowCount - 1 Then
                            txt_DiscPerc.Focus()

                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                        End If
                    ElseIf .CurrentCell.ColumnIndex = 7 Then
                        If UCase(Trim(cbo_Type.Text)) = "DELIVERY" Then
                            If .CurrentCell.ColumnIndex >= 7 And .CurrentCell.RowIndex = .RowCount - 1 Then

                                txt_DiscPerc.Focus()

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(3)



                            End If
                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(9)

                        End If
                    Else

                        If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then
                            txt_DiscPerc.Focus()

                        Else
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                        End If

                    End If

                    Return True

                ElseIf keyData = Keys.Up Then
                    If .CurrentCell.ColumnIndex <= 1 Then
                        If .CurrentCell.RowIndex = 0 Then
                            txt_CommAmount.Focus()

                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(7)

                        End If
                    ElseIf .CurrentCell.ColumnIndex = 3 Then
                        If UCase(Trim(cbo_Type.Text)) = "DELIVERY" Then
                            If .CurrentCell.ColumnIndex = 3 And .CurrentCell.RowIndex = 0 Then
                                msk_OrderDate.Focus()
                            Else
                                .CurrentCell = .Rows(.CurrentRow.Index - 1).Cells(7)
                            End If
                        Else
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(2)
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

    End Function

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim vOrdByNo As String = ""

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text)

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Yarn_Sales_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Yarn_Sales_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub
        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.Yarn_Proforma_Sales_Entry, New_Entry, Me, con, "Yarn_Sales_Prroforma_Head", "Yarn_Sales_Code", NewCode, "Yarn_Sales_Date", "(Yarn_Sales_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub


        If Common_Procedures.settings.Vefified_Status = 1 Then
            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            If Val(Common_Procedures.get_FieldValue(con, "Yarn_Sales_Prroforma_Head", "Verified_Status", "(Yarn_Sales_Code = '" & Trim(NewCode) & "')")) = 1 Then
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

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.Transaction = trans

            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "DELETE", "Yarn_Sales_Prroforma_Head", "Yarn_Sales_Code", Val(lbl_Company.Tag), NewCode, lbl_InvoiceNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Yarn_Sales_Code, Company_IdNo, for_OrderBy", trans)
            Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "DELETE", "Yarn_Sales_Proforma_Details", "Yarn_Sales_Code", Val(lbl_Company.Tag), NewCode, lbl_InvoiceNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "Count_IdNo,Mill_IdNo,Bags,Cones,Weight,Rate_For,Rate,Amount,Bag_No,Delivery_No,Weaver_Sales_Yarn_Delivery_Code,Weaver_Sales_Yarn_Delivery_Detail_SlNo", "Sl_No", "Yarn_Sales_Code, For_OrderBy, Company_IdNo, Yarn_Sales_No,Yarn_Sales_Date, Ledger_Idno", trans)


            If Common_Procedures.VoucherBill_Deletion(con, Trim(Pk_Condition) & Trim(NewCode), trans) = False Then
                Throw New ApplicationException("Error on Voucher Bill Deletion")
            End If

            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), trans)
            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition2) & Trim(NewCode), trans)

            If Trim(UCase(cbo_Type.Text)) = "DELIVERY" Then
                cmd.CommandText = "Update Weaver_Sales_Yarn_Delivery_Details set Sales_Bag = a.Sales_Bag - (b.Bags) , Sales_Cone = a.Sales_Cone - (b.Cones) , Sales_Weight = a.Sales_Weight - (b.Weight ) from Weaver_Sales_Yarn_Delivery_Details a, Yarn_Sales_Proforma_Details b Where b.Yarn_Sales_Code = '" & Trim(NewCode) & "' and a.Weaver_Sales_Yarn_Delivery_Code = b.Weaver_Sales_Yarn_Delivery_Code and a.Weaver_Sales_Yarn_Delivery_Detail_SlNo = b.Weaver_Sales_Yarn_Delivery_Detail_SlNo"
                cmd.ExecuteNonQuery()
            End If

            cmd.CommandText = "delete from AgentCommission_Processing_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Empty_BeamBagCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Yarn_Sales_Proforma_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Sales_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "delete from Yarn_Sales_Prroforma_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Sales_Code = '" & Trim(NewCode) & "'"
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
            da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or (Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) ) order by Ledger_DisplayName", con)
            da.Fill(dt1)
            cbo_Filter_PartyName.DataSource = dt1
            cbo_Filter_PartyName.DisplayMember = "Ledger_DisplayName"

            da = New SqlClient.SqlDataAdapter("select Count_Name from Count_Head where (Count_IdNo = 0 ) order by Count_Name", con)
            da.Fill(dt2)
            cbo_Filter_CountName.DataSource = dt2
            cbo_Filter_CountName.DisplayMember = "Count_Name"


            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_CountName.Text = ""

            cbo_Filter_CountName.SelectedIndex = -1
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

            da = New SqlClient.SqlDataAdapter("select top 1 Yarn_Sales_No from Yarn_Sales_Prroforma_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Sales_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Yarn_Sales_No", con)
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
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_InvoiceNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Yarn_Sales_No from Yarn_Sales_Prroforma_Head where for_orderby > " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Sales_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Yarn_Sales_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_InvoiceNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Yarn_Sales_No from Yarn_Sales_Prroforma_Head where for_orderby < " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Sales_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Yarn_Sales_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 Yarn_Sales_No from Yarn_Sales_Prroforma_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Sales_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Yarn_Sales_No desc", con)
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

        Finally
            dt.Dispose()
            da.Dispose()

        End Try

    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable

        Try
            clear()

            New_Entry = True

            lbl_InvoiceNo.Text = Common_Procedures.get_MaxCode(con, "Yarn_Sales_Prroforma_Head", "Yarn_Sales_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)
            lbl_InvoiceNo.ForeColor = Color.Red

            msk_date.Text = Date.Today.ToShortDateString
            Da1 = New SqlClient.SqlDataAdapter("select Top 1 a.*, b.ledger_name as PurchaseAcName, c.ledger_name as TaxAcName from Yarn_Sales_Prroforma_Head a LEFT OUTER JOIN Ledger_Head b ON a.PurchaseAc_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Ledger_Head c ON a.TaxAc_IdNo = c.Ledger_IdNo where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Yarn_Sales_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by a.for_Orderby desc, a.Yarn_Sales_No desc", con)
            Da1.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then
                If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)
                    If Dt1.Rows(0).Item("Yarn_Sales_Date").ToString <> "" Then msk_date.Text = Dt1.Rows(0).Item("Yarn_Sales_Date").ToString
                End If
                If Dt1.Rows(0).Item("PurchaseAcName").ToString <> "" Then Cbo_SalesAc.Text = Dt1.Rows(0).Item("PurchaseAcName").ToString
                If Dt1.Rows(0).Item("Tax_Type").ToString <> "" Then cbo_TaxType.Text = Dt1.Rows(0).Item("Tax_Type").ToString
                If Dt1.Rows(0).Item("Tax_Percentage").ToString <> "" Then txt_TaxPerc.Text = Val(Dt1.Rows(0).Item("Tax_Percentage").ToString)
                If Dt1.Rows(0).Item("TaxAcName").ToString <> "" Then cbo_VatAc.Text = Dt1.Rows(0).Item("TaxAcName").ToString

                If IsDBNull(Dt1.Rows(0).Item("Freight_Name").ToString) = False Then
                    If Trim(Dt1.Rows(0).Item("Freight_Name").ToString) <> "" Then txt_Freight_Name.Text = Dt1.Rows(0).Item("Freight_Name").ToString
                End If
                If IsDBNull(Dt1.Rows(0).Item("Packing_Name").ToString) = False Then
                    If Trim(Dt1.Rows(0).Item("Packing_Name").ToString) <> "" Then txt_Packing_Name.Text = Dt1.Rows(0).Item("Packing_Name").ToString
                End If
                If IsDBNull(Dt1.Rows(0).Item("AddLess_Name").ToString) = False Then
                    If Trim(Dt1.Rows(0).Item("AddLess_Name").ToString) <> "" Then txt_AddLess_Name.Text = Dt1.Rows(0).Item("AddLess_Name").ToString
                End If
                If IsDBNull(Dt1.Rows(0).Item("Chess_Name").ToString) = False Then
                    If Trim(Dt1.Rows(0).Item("Chess_Name").ToString) <> "" Then txt_Chess_Name.Text = Dt1.Rows(0).Item("Chess_Name").ToString
                End If
                If IsDBNull(Dt1.Rows(0).Item("ExtraTax_Name").ToString) = False Then
                    If Trim(Dt1.Rows(0).Item("ExtraTax_Name").ToString) <> "" Then txt_ExtraTax_Name.Text = Dt1.Rows(0).Item("ExtraTax_Name").ToString
                End If

                Da1 = New SqlClient.SqlDataAdapter("Select a.* from Yarn_Sales_Proforma_Details a Where a.Yarn_Sales_Code = '" & Trim(Dt1.Rows(0).Item("Yarn_Sales_Code").ToString) & "' Order by a.sl_no", con)
                Dt2 = New DataTable
                Da1.Fill(Dt2)

                If Dt2.Rows.Count > 0 Then

                    If dgv_Details.Rows.Count > 0 Then
                        dgv_Details.Rows(0).Cells(6).Value = Dt2.Rows(0).Item("Rate_For").ToString
                    End If

                End If

                Dt2.Clear()

            End If

            Dt1.Clear()


            txt_DateAndTime_Invoice.Text = Format(dtp_Date.Value, "dd/MM/yyyy hh:mm tt")
            txt_TimeofRemoval_Goods.Text = Format(DateAdd(DateInterval.Minute, 10, dtp_Date.Value), "dd/MM/yyyy hh:mm tt")

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            Dt1.Dispose()
            Dt2.Dispose()
            Da1.Dispose()

            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()

        End Try

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim RefCode As String

        Try

            inpno = InputBox("Enter Invoice No.", "FOR FINDING...")

            RefCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Yarn_Sales_No from Yarn_Sales_Prroforma_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Sales_Code = '" & Trim(RefCode) & "'", con)
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
                MessageBox.Show("Invoice No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            Dt.Dispose()
            Da.Dispose()

        End Try

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim InvCode As String

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Yarn_Sales_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Yarn_Sales_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.Yarn_Proforma_Sales_Entry, New_Entry, Me) = False Then Exit Sub


        Try

            inpno = InputBox("Enter New Invoice No.", "FOR NEW Ref NO. INSERTION...")

            InvCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Yarn_Sales_No from Yarn_Sales_Prroforma_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Sales_Code = '" & Trim(InvCode) & "'", con)
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
                    MessageBox.Show("Invalid Invoice No.", "DOES NOT INSERT NEW Ref...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_InvoiceNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT NEW BILL...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            Dt.Dispose()
            Da.Dispose()

        End Try

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim SaAc_ID As Integer = 0
        Dim Rck_IdNo As Integer = 0
        Dim Fp_Id As Integer = 0
        Dim vLrDt As String = ""
        Dim vLCDt As String = ""
        Dim vOrdrDt As String = ""
        Dim Led_ID As Integer = 0
        Dim Cnt_ID As Integer = 0
        Dim Mill_ID As Integer = 0
        Dim Del_ID As Integer = 0
        Dim Rec_ID As Integer = 0
        Dim Agt_Idno As Integer = 0
        Dim TxAc_ID As Integer = 0
        Dim Unt_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim EntID As String = ""
        Dim PBlNo As String = ""
        Dim Partcls As String = ""
        Dim vTotCns As Single, vTotBgs As Single, vTotWght As Single
        Dim Cr_ID As Integer = 0
        Dim Dr_ID As Integer = 0
        Dim Usr_ID As Integer = 0
        Dim Trans_ID As Integer
        Dim VouBil As String = ""
        Dim YrnClthNm As String = ""
        Dim vVou_BlAmt As Double = 0
        Dim Dc_Cde As String = ""
        Dim Dc_SlNo As Integer = 0, Nr As Integer = 0
        Dim Dc_No As String = ""
        Dim C_Sts As Integer = 0

        Dim Verified_STS As String = ""

        Dim vOrdByNo As String = ""

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text)


        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        'If Common_Procedures.UserRight_Check(Common_Procedures.UR.Yarn_Sales_Entry, New_Entry) = False Then Exit Sub
        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Yarn_Proforma_Sales_Entry, New_Entry, Me, con, "Yarn_Sales_Prroforma_Head", "Yarn_Sales_Code", NewCode, "Yarn_Sales_Date", "(Yarn_Sales_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Sales_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, Yarn_Sales_No desc", dtp_Date.Value.Date) = False Then Exit Sub


        If Common_Procedures.settings.Vefified_Status = 1 Then
            If Not (Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_Verified_Status = 1) Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

                If Val(Common_Procedures.get_FieldValue(con, "Yarn_Sales_Prroforma_Head", "Verified_Status", "(Yarn_Sales_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "')")) = 1 Then
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

        Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyName.Text)
        If Val(Led_ID) = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_PartyName.Enabled And cbo_PartyName.Visible Then cbo_PartyName.Focus()
            Exit Sub
        End If

        Agt_Idno = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Agent.Text)

        SaAc_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, Cbo_SalesAc.Text)
        TxAc_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_VatAc.Text)
        Trans_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Transport.Text)
        lbl_UserName.Text = Common_Procedures.User.IdNo


        If SaAc_ID = 0 And Val(CSng(lbl_NetAmount.Text)) <> 0 Then
            MessageBox.Show("Invalid Sales A/c name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If Cbo_SalesAc.Enabled And Cbo_SalesAc.Visible Then Cbo_SalesAc.Focus()
            Exit Sub
        End If

        With dgv_Details

            For i = 0 To .RowCount - 1

                If Val(.Rows(i).Cells(3).Value) <> 0 Or Val(.Rows(i).Cells(5).Value) <> 0 Then

                    Cnt_ID = Common_Procedures.Count_NameToIdNo(con, .Rows(i).Cells(1).Value)
                    If Cnt_ID = 0 Then
                        MessageBox.Show("Invalid Count Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(1)
                        End If
                        Exit Sub
                    End If

                    Mill_ID = Common_Procedures.Mill_NameToIdNo(con, .Rows(i).Cells(2).Value)
                    If Mill_ID = 0 Then
                        MessageBox.Show("Invalid Mill Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(2)
                        End If
                        Exit Sub
                    End If

                    If Val(.Rows(i).Cells(5).Value) = 0 Then
                        MessageBox.Show("Invalid Weight", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(5)
                        End If
                        Exit Sub
                    End If

                End If

            Next

        End With

        If TxAc_ID = 0 And Val(lbl_TaxAmount.Text) <> 0 Then
            MessageBox.Show("Invalid Tax A/c name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_VatAc.Enabled And cbo_VatAc.Visible Then cbo_VatAc.Focus()
            Exit Sub
        End If

        If Val(lbl_TaxAmount.Text) <> 0 And (Trim(cbo_TaxType.Text) = "" Or Trim(cbo_TaxType.Text) = "-NIL-") Then
            MessageBox.Show("Invalid Tax Type", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_TaxType.Enabled And cbo_TaxType.Visible Then cbo_TaxType.Focus()
            Exit Sub
        End If

        vLrDt = ""
        If Trim(msk_Lr_Date.Text) <> "" Then
            If IsDate(msk_Lr_Date.Text) = True Then
                vLrDt = Trim(msk_Lr_Date.Text)
            End If
        End If

        vLCDt = ""
        If Trim(msk_LC_Date.Text) <> "" Then
            If IsDate(msk_LC_Date.Text) = True Then
                vLCDt = Trim(msk_LC_Date.Text)
            End If
        End If

        vOrdrDt = ""
        If Trim(msk_OrderDate.Text) <> "" Then
            If IsDate(msk_OrderDate.Text) = True Then
                vOrdrDt = Trim(msk_OrderDate.Text)
            End If
        End If

        C_Sts = 0
        If chk_C_FormSales.Checked = True Then
            C_Sts = 1
        End If

        Verified_STS = 0
        If chk_Verified_Status.Checked = True Then Verified_STS = 1


        NoCalc_Status = False
        Total_Calculation()

        vTotCns = 0 : vTotBgs = 0 : vTotWght = 0

        If dgv_Details_Total.RowCount > 0 Then
            vTotBgs = Val(dgv_Details_Total.Rows(0).Cells(3).Value())
            vTotCns = Val(dgv_Details_Total.Rows(0).Cells(4).Value())
            vTotWght = Val(dgv_Details_Total.Rows(0).Cells(5).Value())
        End If

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_InvoiceNo.Text = Common_Procedures.get_MaxCode(con, "Yarn_Sales_Prroforma_Head", "Yarn_Sales_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@YarnDate", Convert.ToDateTime(msk_date.Text))


            If New_Entry = True Then
                cmd.CommandText = "Insert into Yarn_Sales_Prroforma_Head (       Yarn_Sales_Code       ,               Company_IdNo       ,           Yarn_Sales_No           ,                               for_OrderBy                                  , Yarn_Sales_Date   ,      Ledger_IdNo        ,  Vehicle_No                        ,         Agent_IdNo        ,  PurchaseAc_IdNo    ,        Delivery_Address1         ,             Delivery_Address2   ,            Delivery_Address3     ,   Agent_Commission_Rate       ,         Agent_Commission_Type    ,   Agent_Commission_Commission   ,       Total_Bags     ,          Total_Cones    ,          Total_Weight     ,               Total_Amount            ,             Discount_Percentage    ,              Discount_Amount         ,              Assessable_Value             ,           TaxAc_IdNo      ,              Tax_Type          ,             Tax_Percentage        ,             Tax_Amount              ,                  Freight_Name        ,                 Freight_Amount          ,               Packing_Name           ,             Packing_Amount        ,               AddLess_Name           ,                 AddLess_Amount          ,               RoundOff_Amount      ,                  Net_Amount               ,         Transport_IdNo    ,              Yarn_Description           ,            Lr_No             ,          Lr_Date     ,                Chess_Name           ,           Chess_Percentage          ,           Chess_Amount                ,          ExtraTax_Name                 ,        ExtraTax_Percentage              ,  ExtraTax_Amount                         ,   User_IdNo                   ,     Despatch_From                   ,      Despatch_To                  ,       LC_No                  ,LC_Date              ,       Order_No                 ,Order_Date            ,    Entry_Type               ,  Through_Name                    ,Date_of_Inv_Preparation                          ,Time_Of_Removal_Of_Goods                ,Sale_Type                          ,C_Form_Sales_Status , Terms_Details1                          ,Terms_Details2                        ,Terms_Details3                         ,Terms_Details4                         ,Terms_Details5                         ,Terms_Details6                         ,Terms_Details7                         ,Terms_Label1                          ,Terms_Label2                          ,Terms_Label3                          ,Terms_Label4                          ,Terms_Label5                          ,Terms_Label6                          ,Terms_Label7                 ,Packing_Type                        ,Lot_No                      ,Verified_Status) " & _
                                    "     Values                  (   '" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ",      @YarnDate    , " & Str(Val(Led_ID)) & ", '" & Trim(cbo_VehicleNo.Text) & "' , " & Str(Val(Agt_Idno)) & ", " & Val(SaAc_ID) & ", '" & Trim(txt_Address1.Text) & "','" & Trim(txt_Address2.Text) & "', '" & Trim(txt_Address3.Text) & "', " & Val(txt_CommRate.Text) & ", '" & Trim(cbo_CommType.Text) & "', " & Val(txt_CommAmount.Text) & ",  " & Val(vTotBgs) & "," & Str(Val(vTotCns)) & ", " & Str(Val(vTotWght)) & ", " & Str(Val(lbl_GrossAmount.Text)) & ", " & Str(Val(txt_DiscPerc.Text)) & ", " & Str(Val(lbl_DiscAmount.Text)) & ", " & Str(Val(lbl_AssessableValue.Text)) & ", " & Str(Val(TxAc_ID)) & ", '" & Trim(cbo_TaxType.Text) & "', " & Str(Val(txt_TaxPerc.Text)) & ", " & Str(Val(lbl_TaxAmount.Text)) & ", '" & Trim(txt_Freight_Name.Text) & "', " & Str(Val(txt_FreightAmount.Text)) & ", '" & Trim(txt_Packing_Name.Text) & "', " & Str(Val(txt_Packing.Text)) & ", '" & Trim(txt_AddLess_Name.Text) & "', " & Str(Val(txt_AddLessAmount.Text)) & ", " & Str(Val(lbl_RoundOff.Text)) & ", " & Str(Val(CSng(lbl_NetAmount.Text))) & ", " & Str(Val(Trans_ID)) & ",'" & Trim(txt_YarnDescription.Text) & "' , '" & Trim(txt_LrNo.Text) & "', '" & Trim(vLrDt) & "' , '" & Trim(txt_Chess_Name.Text) & "', " & Str(Val(txt_ChessPerc.Text)) & ", " & Str(Val(lbl_ChessAmount.Text)) & ", '" & Trim(txt_ExtraTax_Name.Text) & "' , " & Str(Val(txt_ExtraTaxPerc.Text)) & " ," & Str(Val(lbl_ExtraTax_Amount.Text)) & ", " & Val(lbl_UserName.Text) & ",'" & Trim(txt_DespatchFrom.Text) & "','" & Trim(txt_DespatchTo.Text) & "','" & Trim(txt_LC_No.Text) & "','" & Trim(vLCDt) & "','" & Trim(txt_OrderNo.Text) & "','" & Trim(vOrdrDt) & "','" & Trim(cbo_Type.Text) & "', '" & Trim(cbo_Through.Text) & "','" & Trim(txt_DateAndTime_Invoice.Text) & "' ,'" & Trim(txt_TimeofRemoval_Goods.Text) & "','" & Trim(cbo_SalesType.Text) & "' ," & Val(C_Sts) & " ,   '" & Trim(txt_Terms_Details1.Text) & "','" & Trim(txt_Terms_Details2.Text) & "','" & Trim(txt_Terms_Details3.Text) & "','" & Trim(txt_Terms_Details4.Text) & "','" & Trim(txt_Terms_Details5.Text) & "','" & Trim(txt_Terms_Details6.Text) & "','" & Trim(txt_Terms_Details7.Text) & "','" & Trim(txt_labelCaption1.Text) & "','" & Trim(txt_labelCaption2.Text) & "','" & Trim(txt_LabelCaption3.Text) & "','" & Trim(txt_LabelCaption4.Text) & "','" & Trim(txt_LabelCaption5.Text) & "','" & Trim(txt_LabelCaption6.Text) & "','" & Trim(txt_LabelCaption7.Text) & "','" & Trim(cbo_PackingType.Text) & "','" & Trim(txt_LotNo.Text) & "', " & Val(Verified_STS) & ") "
                cmd.ExecuteNonQuery()

            Else

                Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "OLD", "Yarn_Sales_Prroforma_Head", "Yarn_Sales_Code", Val(lbl_Company.Tag), NewCode, lbl_InvoiceNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Yarn_Sales_Code, Company_IdNo, for_OrderBy", tr)
                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "OLD", "Yarn_Sales_Proforma_Details", "Yarn_Sales_Code", Val(lbl_Company.Tag), NewCode, lbl_InvoiceNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "Count_IdNo,Mill_IdNo,Bags,Cones,Weight,Rate_For,Rate,Amount,Bag_No,Delivery_No,Weaver_Sales_Yarn_Delivery_Code,Weaver_Sales_Yarn_Delivery_Detail_SlNo", "Sl_No", "Yarn_Sales_Code, For_OrderBy, Company_IdNo, Yarn_Sales_No,Yarn_Sales_Date, Ledger_Idno", tr)


                cmd.CommandText = "Update Yarn_Sales_Prroforma_Head set Yarn_Sales_Date = @YarnDate, Ledger_IdNo = " & Str(Val(Led_ID)) & ", Agent_IdNo = " & Str(Val(Agt_Idno)) & ", PurchaseAc_IdNo = " & Str(Val(SaAc_ID)) & ", Vehicle_No = '" & Trim(cbo_VehicleNo.Text) & "', Delivery_Address1 = '" & Trim(txt_Address1.Text) & "', Delivery_Address2 ='" & Trim(txt_Address2.Text) & "' ,Delivery_Address3 = '" & Trim(txt_Address3.Text) & "', Agent_Commission_Rate = " & Val(txt_CommRate.Text) & ", Agent_Commission_Type = '" & Trim(cbo_CommType.Text) & "', Agent_Commission_Commission =" & Val(txt_CommAmount.Text) & ", Total_Bags = " & Val(vTotBgs) & ",Total_Cones  = " & Str(Val(vTotCns)) & ", Total_Weight = " & Str(Val(vTotWght)) & ", Total_Amount = " & Str(Val(lbl_GrossAmount.Text)) & ", Discount_Percentage = " & Str(Val(txt_DiscPerc.Text)) & ", Discount_Amount = " & Str(Val(lbl_DiscAmount.Text)) & ", Assessable_Value = " & Str(Val(lbl_AssessableValue.Text)) & ", Tax_Type = '" & Trim(cbo_TaxType.Text) & "', Tax_Percentage = " & Str(Val(txt_TaxPerc.Text)) & ", Tax_Amount = " & Str(Val(lbl_TaxAmount.Text)) & ", TaxAc_IdNo = " & Str(Val(TxAc_ID)) & ", Lr_No = '" & Trim(txt_LrNo.Text) & "'  , Lr_Date  = '" & Trim(vLrDt) & "', Freight_Name = '" & Trim(txt_Freight_Name.Text) & "', Freight_Amount = " & Str(Val(txt_FreightAmount.Text)) & ", Packing_Name = '" & Trim(txt_Packing_Name.Text) & "', Packing_Amount = " & Str(Val(txt_Packing.Text)) & ", AddLess_Name = '" & Trim(txt_AddLess_Name.Text) & "', AddLess_Amount = " & Str(Val(txt_AddLessAmount.Text)) & ", Chess_Name = '" & Trim(txt_Chess_Name.Text) & "' ,  Chess_Percentage = " & Str(Val(txt_ChessPerc.Text)) & ",Chess_Amount = " & Str(Val(lbl_ChessAmount.Text)) & ",  ExtraTax_Name = '" & Trim(txt_ExtraTax_Name.Text) & "' ,  ExtraTax_Percentage = " & Str(Val(txt_ExtraTaxPerc.Text)) & ",ExtraTax_Amount = " & Str(Val(lbl_ExtraTax_Amount.Text)) & ",RoundOff_Amount = " & Str(Val(lbl_RoundOff.Text)) & ", Net_Amount = " & Str(Val(CSng(lbl_NetAmount.Text))) & ", Transport_IdNo  = " & Str(Val(Trans_ID)) & ", Yarn_Description = '" & Trim(txt_YarnDescription.Text) & "',User_IdNo = " & Val(lbl_UserName.Text) & ", Despatch_From ='" & Trim(txt_DespatchFrom.Text) & "',Despatch_To ='" & Trim(txt_DespatchTo.Text) & "',LC_No ='" & Trim(txt_LC_No.Text) & "',LC_Date ='" & Trim(vLCDt) & "',Order_No ='" & Trim(txt_OrderNo.Text) & "',Order_Date ='" & Trim(vOrdrDt) & "', Entry_Type = '" & Trim(cbo_Type.Text) & "',Through_Name= '" & Trim(cbo_Through.Text) & "' ,Date_of_Inv_Preparation   ='" & Trim(txt_DateAndTime_Invoice.Text) & "' ,Time_Of_Removal_Of_Goods ='" & Trim(txt_TimeofRemoval_Goods.Text) & "',Sale_Type = '" & Trim(cbo_SalesType.Text) & "',C_Form_Sales_Status = " & Val(C_Sts) & ",Terms_Details1  = '" & Trim(txt_Terms_Details1.Text) & "' ,Terms_Details2 ='" & Trim(txt_Terms_Details2.Text) & "' ,Terms_Details3 ='" & Trim(txt_Terms_Details3.Text) & "' ,Terms_Details4  ='" & Trim(txt_Terms_Details4.Text) & "'  ,Terms_Details5  ='" & Trim(txt_Terms_Details5.Text) & "' ,Terms_Details6 ='" & Trim(txt_Terms_Details6.Text) & "' ,Terms_Details7  ='" & Trim(txt_Terms_Details7.Text) & "'  ,Terms_Label1 ='" & Trim(txt_labelCaption1.Text) & "' ,Terms_Label2  ='" & Trim(txt_labelCaption2.Text) & "'  ,Terms_Label3 ='" & Trim(txt_LabelCaption3.Text) & "' ,Terms_Label4  ='" & Trim(txt_LabelCaption4.Text) & "' ,Terms_Label5  ='" & Trim(txt_LabelCaption5.Text) & "'  ,Terms_Label6 ='" & Trim(txt_LabelCaption6.Text) & "'  ,Terms_Label7 ='" & Trim(txt_LabelCaption7.Text) & "',Packing_Type ='" & Trim(cbo_PackingType.Text) & "' ,Lot_No = '" & Trim(txt_LotNo.Text) & "',Verified_Status= " & Val(Verified_STS) & " Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Sales_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                If Trim(UCase(cbo_Type.Text)) = "DELIVERY" Then
                    cmd.CommandText = "Update Weaver_Sales_Yarn_Delivery_Details set Sales_Bag = a.Sales_Bag - (b.Bags) , Sales_Cone = a.Sales_Cone - (b.Cones) , Sales_Weight = a.Sales_Weight - (b.Weight ) from Weaver_Sales_Yarn_Delivery_Details a, Yarn_Sales_Proforma_Details b Where b.Yarn_Sales_Code = '" & Trim(NewCode) & "' and a.Weaver_Sales_Yarn_Delivery_Code = b.Weaver_Sales_Yarn_Delivery_Code and a.Weaver_Sales_Yarn_Delivery_Detail_SlNo = b.Weaver_Sales_Yarn_Delivery_Detail_SlNo"
                    cmd.ExecuteNonQuery()
                End If

            End If
            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "NEW", "Yarn_Sales_Prroforma_Head", "Yarn_Sales_Code", Val(lbl_Company.Tag), NewCode, lbl_InvoiceNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Yarn_Sales_Code, Company_IdNo, for_OrderBy", tr)
           
            EntID = Trim(Pk_Condition) & Trim(lbl_InvoiceNo.Text)
            PBlNo = Trim(lbl_InvoiceNo.Text)
            Partcls = "Sales : Inv No. " & Trim(lbl_InvoiceNo.Text)

            Rec_ID = Common_Procedures.CommonLedger.Godown_Ac
            Del_ID = 0

            cmd.CommandText = "Delete from Yarn_Sales_Proforma_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Sales_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Empty_BeamBagCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            With dgv_Details

                Sno = 0
                YrnClthNm = ""
                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(5).Value) <> 0 Then

                        Sno = Sno + 1
                        If Trim(UCase(cbo_Type.Text)) = "DELIVERY" Then
                            Dc_Cde = Trim(.Rows(i).Cells(11).Value)
                            Dc_No = Trim(.Rows(i).Cells(10).Value)
                            Dc_SlNo = Val(.Rows(i).Cells(12).Value)
                        Else
                            Dc_Cde = ""
                            Dc_No = ""
                            Dc_SlNo = 0
                        End If

                        Cnt_ID = Common_Procedures.Count_NameToIdNo(con, .Rows(i).Cells(1).Value, tr)
                        Mill_ID = Common_Procedures.Mill_NameToIdNo(con, .Rows(i).Cells(2).Value, tr)

                        If Trim(YrnClthNm) = "" Then YrnClthNm = Trim(.Rows(i).Cells(1).Value) & "/" & Trim(.Rows(i).Cells(2).Value)

                        cmd.CommandText = "Insert into Yarn_Sales_Proforma_Details ( Yarn_Sales_Code ,               Company_IdNo       ,   Yarn_Sales_No                                  ,                     for_OrderBy                                            ,              Yarn_Sales_Date,             Sl_No     ,              Count_IdNo         ,          Mill_IdNo       ,                     Bags            ,                 Cones                ,                        Weight         ,                   Rate_For                       ,                     Rate                 ,                  Amount          ,      Bag_No                          ,    Delivery_No          ,Weaver_Sales_Yarn_Delivery_Code,Weaver_Sales_Yarn_Delivery_Detail_SlNo) " & _
                                            "     Values                 (   '" & Trim(NewCode) & "'      , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ",       @YarnDate            ,  " & Str(Val(Sno)) & ", " & Str(Val(Cnt_ID)) & ", " & Str(Val(Mill_ID)) & ", " & Str(Val(.Rows(i).Cells(3).Value)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & ", " & Str(Val(.Rows(i).Cells(5).Value)) & ", '" & Trim(.Rows(i).Cells(6).Value) & "', " & Str(Val(.Rows(i).Cells(7).Value)) & ", " & Str(Val(.Rows(i).Cells(8).Value)) & ",'" & Trim(.Rows(i).Cells(9).Value) & "', '" & Trim(Dc_No) & "'  ,'" & Trim(Dc_Cde) & "'         ,   " & Val(Dc_SlNo) & ") "
                        cmd.ExecuteNonQuery()
                    
                        If Trim(UCase(cbo_Type.Text)) = "DELIVERY" Then
                            Nr = 0
                            cmd.CommandText = "Update Weaver_Sales_Yarn_Delivery_Details set  Sales_Bag = Sales_Bag + " & Str(Val(.Rows(i).Cells(3).Value)) & " , Sales_Cone = Sales_Cone + " & Str(Val(.Rows(i).Cells(4).Value)) & "  ,  Sales_Weight = Sales_Weight + " & Str(Val(.Rows(i).Cells(5).Value)) & "     Where Weaver_Sales_Yarn_Delivery_Code = '" & Trim(Dc_Cde) & "' and Weaver_Sales_Yarn_Delivery_Detail_SlNo = " & Str(Val(Dc_SlNo)) & " and DeliveryTo_idNo = " & Str(Val(Led_ID))
                            Nr = cmd.ExecuteNonQuery()

                            If Nr = 0 Then
                                Throw New ApplicationException("Mismatch of Delivery and Party Details")
                            End If

                        Else
                            cmd.CommandText = "Insert into Stock_Yarn_Processing_Details ( Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Entry_ID, Particulars, Party_Bill_No, Sl_No, Count_IdNo, Yarn_Type, Mill_IdNo, Bags, Cones, Weight, DeliveryToIdno_ForParticulars, ReceivedFromIdno_ForParticulars  ) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ", @YarnDate, " & Str(Val(Del_ID)) & ", " & Str(Val(Rec_ID)) & ", '" & Trim(EntID) & "', '" & Trim(Partcls) & "', '" & Trim(PBlNo) & "', " & Str(Val(Sno)) & ", " & Str(Val(Cnt_ID)) & ", 'MILL', " & Str(Val(Mill_ID)) & ", " & Str(Val(.Rows(i).Cells(3).Value)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & ", " & Str(Val(.Rows(i).Cells(5).Value)) & ", " & Str(Val(Led_ID)) & ", " & Str(Val(Rec_ID)) & " )"
                            cmd.ExecuteNonQuery()

                        End If


                    End If

                Next
                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "NEW", "Yarn_Sales_Proforma_Details", "Yarn_Sales_Code", Val(lbl_Company.Tag), NewCode, lbl_InvoiceNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "Count_IdNo,Mill_IdNo,Bags,Cones,Weight,Rate_For,Rate,Amount,Bag_No,Delivery_No,Weaver_Sales_Yarn_Delivery_Code,Weaver_Sales_Yarn_Delivery_Detail_SlNo", "Sl_No", "Yarn_Sales_Code, For_OrderBy, Company_IdNo, Yarn_Sales_No,Yarn_Sales_Date, Ledger_Idno", tr)

            End With

            If Val(vTotBgs) <> 0 Or Val(vTotCns) <> 0 Then
                cmd.CommandText = "Insert into Stock_Empty_BeamBagCone_Processing_Details(Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Entry_ID, Party_Bill_No, Sl_No, Beam_Width_IdNo, Empty_Beam, Empty_Bags, Empty_Cones, Particulars) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ", @YarnDate, " & Str(Val(Del_ID)) & ", " & Str(Val(Rec_ID)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', 1, 0, 0, " & Str(Val(vTotBgs)) & ", " & Str(Val(vTotCns)) & ", '" & Trim(Partcls) & "')"
                cmd.ExecuteNonQuery()
            End If


            'AgentCommission Posting
            cmd.CommandText = "delete from AgentCommission_Processing_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            If Val(Agt_Idno) <> 0 Then

                cmd.CommandText = "Insert into AgentCommission_Processing_Details (  Reference_Code   ,             Company_IdNo         ,            Reference_No       ,                               For_OrderBy                              , Reference_Date, Commission_For,     Ledger_IdNo     ,      Agent_IdNo      ,         Entry_ID     ,      Party_BillNo    ,       Particulars      ,      Yarn_Cloth_Name     ,         Bags_Meters       ,               Amount               ,              Commission_Type      ,       Commission_Rate              ,            Commission_Amount         ) " & _
                                                " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ",   @YarnDate   ,     'YARN'    , " & Str(Led_ID) & ", " & Str(Agt_Idno) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', '" & Trim(YrnClthNm) & "', " & Str(Val(vTotBgs)) & ", " & Str(Val(lbl_NetAmount.Text)) & ", '" & Trim(cbo_CommType.Text) & "', " & Str(Val(txt_CommRate.Text)) & ", " & Str(Val(txt_CommAmount.Text)) & ")"
                cmd.ExecuteNonQuery()

            End If

            'Dim vLed_IdNos As String = "", vVou_Amts As String = "", ErrMsg As String = ""

            'If Val(CSng(lbl_NetAmount.Text)) <> 0 Then
            '    vLed_IdNos = Led_ID & "|" & SaAc_ID & "|" & TxAc_ID
            '    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1023" Then '---- Manoj Kumar Spining (Perumanallur)
            '        vVou_Amts = -1 * (Val(CSng(lbl_NetAmount.Text)) - Val(txt_CommAmount.Text)) & "|" & (Val(CSng(lbl_NetAmount.Text)) - Val(txt_CommAmount.Text) - Val(lbl_TaxAmount.Text)) & "|" & Val(lbl_TaxAmount.Text)
            '    Else
            '        vVou_Amts = -1 * Val(CSng(lbl_NetAmount.Text)) & "|" & (Val(CSng(lbl_NetAmount.Text)) - Val(lbl_TaxAmount.Text)) & "|" & Val(lbl_TaxAmount.Text)
            '    End If

            '    If Common_Procedures.Voucher_Updation(con, "Yarn.Sales", Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), Trim(lbl_InvoiceNo.Text), Convert.ToDateTime(msk_date.Text), "Bill No : " & Trim(lbl_InvoiceNo.Text), vLed_IdNos, vVou_Amts, ErrMsg, tr) = False Then
            '        Throw New ApplicationException(ErrMsg)
            '    End If
            'End If

            'Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition2) & Trim(NewCode), tr)

            'If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1023" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1035" Then '---- Kalaimagal Textiles (Avinashi) Then '---- Manoj Kumar Spining (Perumanallur)
            '    If Val(txt_CommAmount.Text) <> 0 Then
            '        vLed_IdNos = Agt_Idno & "|" & Val(Common_Procedures.CommonLedger.Agent_Commission_Ac)
            '        vVou_Amts = Val(txt_CommAmount.Text) & "|" & -1 * Val(txt_CommAmount.Text)
            '        If Common_Procedures.Voucher_Updation(con, "YrnSal.AgComm", Val(lbl_Company.Tag), Trim(Pk_Condition2) & Trim(NewCode), Trim(lbl_InvoiceNo.Text), Convert.ToDateTime(msk_date.Text), "Bill No : " & Trim(lbl_InvoiceNo.Text), vLed_IdNos, vVou_Amts, ErrMsg, tr) = False Then
            '            Throw New ApplicationException(ErrMsg)
            '        End If
            '    End If
            'End If

            ''---Bill Posting
            'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1023" Then '---- Manoj Kumar Spining (Perumanallur)
            '    vVou_BlAmt = (Val(CSng(lbl_NetAmount.Text)) - Val(txt_CommAmount.Text))
            'Else
            '    vVou_BlAmt = Val(CSng(lbl_NetAmount.Text))
            'End If
            'VouBil = Common_Procedures.VoucherBill_Posting(con, Val(lbl_Company.Tag), msk_date.Text, Led_ID, Trim(lbl_InvoiceNo.Text), Agt_Idno, Val(vVou_BlAmt), "DR", Trim(Pk_Condition) & Trim(NewCode), tr)
            'If Trim(UCase(VouBil)) = "ERROR" Then
            '    Throw New ApplicationException("Error on Voucher Bill Posting")
            'End If

            tr.Commit()

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

            If Val(Common_Procedures.settings.OnSave_MoveTo_NewEntry_Status) = 1 Then
                If New_Entry = True Then
                    new_record()
                Else
                    move_record(lbl_InvoiceNo.Text)
                End If

            Else
                move_record(lbl_InvoiceNo.Text)

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

            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()


        End Try

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

        CntID = Common_Procedures.Count_NameToIdNo(con, dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(1).Value)
        MilID = Common_Procedures.Mill_NameToIdNo(con, dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(2).Value)

        Wgt_Bag = 0 : Wgt_Cn = 0 : Cn_bag = 0

        If CntID <> 0 And MilID <> 0 Then

            Da = New SqlClient.SqlDataAdapter("select * from Mill_Count_Details where mill_idno = " & Str(Val(MilID)) & " and count_idno = " & Str(Val(CntID)), con)
            Da.Fill(Dt)
            With dgv_Details

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

                If .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 4 Then


                    'If .CurrentCell.ColumnIndex = 4 Then
                    '    If Val(Wgt_Cn) <> 0 Then
                    '        .Rows(.CurrentRow.Index).Cells(5).Value = Format(.Rows(.CurrentRow.Index).Cells(4).Value * Val(Wgt_Cn), "##########0.000")
                    '    End If

                    'End If
                    If .CurrentCell.ColumnIndex = 3 Then
                        If Val(Cn_bag) <> 0 Then
                            .Rows(.CurrentRow.Index).Cells(4).Value = .Rows(.CurrentRow.Index).Cells(3).Value * Val(Cn_bag)
                        End If

                        If Val(Wgt_Bag) <> 0 Then
                            .Rows(.CurrentRow.Index).Cells(5).Value = Format(Val(.Rows(.CurrentRow.Index).Cells(3).Value) * Val(Wgt_Bag), "#########0.000")
                        End If

                    End If
                End If

            End With

        End If

    End Sub

    Private Sub cbo_Transport_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Transport.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")

    End Sub

    'Private Sub Weight_Calculation()
    '    Dim n As Integer
    '    Dim dt As New DataTable
    '    With dgv_Details
    '        If dt.Rows.Count > 0 Then
    '            .Rows.Clear()
    '            For i = 0 To dt.Rows.Count - 1

    '                n = .Rows.Add()
    '                If Val(dgv_Details.Rows(0).Cells(3).Value) <> 0 Then
    '                    .Rows(n).Cells(6).Value = Val(dgv_Details.Rows(0).Cells(3).Value) * Val(dgv_Details.Rows(0).Cells(6).Value)
    '                End If
    '                If Val(dgv_Details.Rows(0).Cells(4).Value) <> 0 Then
    '                    .Rows(n).Cells(6).Value = Val(dgv_Details.Rows(0).Cells(4).Value) * Val(dgv_Details.Rows(0).Cells(6).Value)
    '                End If

    '            Next i
    '        End If
    '    End With
    '    'If Val(txt_Cones_Bag.Text) <> 0 Then
    '    '    txt_Cones.Text = Val(txt_Bags.Text) * Val(txt_Cones_Bag.Text)
    '    'End If
    '    'If Val(txt_Weight_Cone.Text) <> 0 Then
    '    '    txt_Weight.Text = Format(Val(txt_Cones.Text) * Val(txt_Weight_Cone.Text), "#########0.000")
    '    'End If
    'End Sub
    Private Sub cbo_Transport_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Transport.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Transport, txt_YarnDescription, cbo_VehicleNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_Transport_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Transport.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Transport, cbo_VehicleNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_IdNo = 0)")

    End Sub
    Private Sub cbo_Party_Name_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PartyName, cbo_Type, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_IdNo = 0)")
        If (e.KeyValue = 40 And cbo_PartyName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

            If cbo_Type.Text = "DELIVERY" Then

                If MessageBox.Show("Do you want to select Delivery  :", "FOR WEAVER YARN DELIVERY SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                    btn_Selection_Click(sender, e)
                End If

            Else

                Cbo_SalesAc.Focus()

            End If
        End If
    End Sub

    Private Sub cbo_Party_Name_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PartyName, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            If cbo_Type.Text = "DELIVERY" Then

                If MessageBox.Show("Do you want to select Delivery :", "FOR WEAVER YARN DELIVERY SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                    btn_Selection_Click(sender, e)
                End If

            Else

                Cbo_SalesAc.Focus()

            End If

        End If
    End Sub
    Private Sub cbo_PartyName_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyName.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
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
    Private Sub cbo_Vechile_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_VehicleNo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Yarn_Sales_Prroforma_Head", "Vehicle_No", "", "")
    End Sub

    Private Sub cbo_VehicleNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_VehicleNo.KeyDown

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_VehicleNo, cbo_Transport, txt_DespatchFrom, "Yarn_Sales_Prroforma_Head", "Vehicle_No", "", "")

        'Try
        '    With cbo_VehicleNo
        '        If e.KeyValue = 40 And .DroppedDown = False Then

        '            dgv_Details.Focus()
        '            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
        '            dgv_Details.CurrentCell.Selected = True

        '        ElseIf e.KeyValue <> 13 And .DroppedDown = False Then
        '            .DroppedDown = True
        '        End If
        '    End With

        'Catch ex As Exception
        '    MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        'End Try
    End Sub

    Private Sub cbo_VehicleNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_VehicleNo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_VehicleNo, txt_DespatchFrom, "Yarn_Sales_Prroforma_Head", "Vehicle_No", "", "", False)
        'If Asc(e.KeyChar) = 13 Then
        '    dgv_Details.Focus()
        '    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
        '    dgv_Details.CurrentCell.Selected = True
        'End If
    End Sub

    Private Sub Cbo_SalesAc_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_SalesAc.GotFocus

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 28)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_PurchaseAc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Cbo_SalesAc.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, Cbo_SalesAc, cbo_PartyName, txt_Address1, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 28)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_PurchaseAc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Cbo_SalesAc.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, Cbo_SalesAc, txt_Address1, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 28)", "(Ledger_IdNo = 0)")
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
        Dim Led_IdNo As Integer, Del_IdNo As Integer, Cnt_IdNo As Integer
        Dim Condt As String = ""

        Try

            Condt = ""
            Del_IdNo = 0
            Cnt_IdNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Yarn_Sales_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Yarn_Sales_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Yarn_Sales_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If
            If Trim(cbo_Filter_CountName.Text) <> "" Then
                Cnt_IdNo = Common_Procedures.Count_NameToIdNo(con, cbo_Filter_CountName.Text)
            End If


            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Ledger_IdNo = " & Str(Val(Led_IdNo)) & " "
            End If
            If Val(Cnt_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " b.Count_Idno = " & Str(Val(Cnt_IdNo)) & " "
            End If



            da = New SqlClient.SqlDataAdapter("select a.*, b.*, c.Ledger_Name as PartyName, d.Count_Name ,e.Mill_Name from Yarn_Sales_Prroforma_Head a INNER JOIN Yarn_Sales_Proforma_Details b ON a.Yarn_Sales_Code = b.Yarn_Sales_Code INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Count_Head d ON b.Count_Idno = d.Count_IdNo LEFT OUTER JOIN Mill_Head e ON b.Mill_IdNo = e.Mill_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Yarn_Sales_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Yarn_Sales_No", con)
            'da = New SqlClient.SqlDataAdapter("select a.*, b.*, c.Ledger_Name as Delv_Name from Yarn_Sales_Prroforma_Head a INNER JOIN Yarn_Sales_Proforma_Details b ON a.Yarn_Sales_Code = b.Yarn_Sales_Code LEFT OUTER JOIN Ledger_Head c ON a.DeliveryTo_Idno = c.Ledger_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Yarn_Sales_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Yarn_Sales_No", con)
            dt2 = New DataTable
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    'dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(0).Value = dt2.Rows(i).Item("Yarn_Sales_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(1).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Yarn_Sales_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("PartyName").ToString
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Mill_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = dt2.Rows(i).Item("Count_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(5).Value = Val(dt2.Rows(i).Item("Bags").ToString)
                    dgv_Filter_Details.Rows(n).Cells(6).Value = Val(dt2.Rows(i).Item("Cones").ToString)
                    dgv_Filter_Details.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Weight").ToString), "########0.000")
                    dgv_Filter_Details.Rows(n).Cells(8).Value = Format(Val(dt2.Rows(i).Item("Net_Amount").ToString), "########0.00")

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
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        ' Common_Procedures.get_CashPartyName_From_All_Entries(con)
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, cbo_Filter_CountName, btn_Filter_Show, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            btn_Filter_Show_Click(sender, e)
        End If


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

    Private Sub dgv_Details_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEndEdit
        dgv_Details_CellLeave(sender, e)
        If dgv_Details.CurrentCell.ColumnIndex = 3 Or dgv_Details.CurrentCell.ColumnIndex = 4 Then
            get_MillCount_Details()
        End If
    End Sub

    Private Sub dgv_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim rect As Rectangle

        With dgv_Details

            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If

            If e.ColumnIndex = 1 Then

                If cbo_Grid_CountName.Visible = False And Trim(UCase(cbo_Type.Text)) <> "DELIVERY" Or Val(cbo_Grid_CountName.Tag) <> e.RowIndex Then

                    cbo_Grid_CountName.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Count_Name from Count_Head order by Count_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Grid_CountName.DataSource = Dt1
                    cbo_Grid_CountName.DisplayMember = "Count_Name"

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

            If e.ColumnIndex = 2 Then

                If cbo_Grid_MillName.Visible = False And Trim(UCase(cbo_Type.Text)) <> "DELIVERY" Or Val(cbo_Grid_MillName.Tag) <> e.RowIndex Then

                    cbo_Grid_MillName.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Mill_Name from Mill_Head order by Mill_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Grid_MillName.DataSource = Dt1
                    cbo_Grid_MillName.DisplayMember = "Mill_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_MillName.Left = .Left + rect.Left
                    cbo_Grid_MillName.Top = .Top + rect.Top

                    cbo_Grid_MillName.Width = rect.Width
                    cbo_Grid_MillName.Height = rect.Height
                    cbo_Grid_MillName.Text = .CurrentCell.Value

                    cbo_Grid_MillName.Tag = Val(e.RowIndex)
                    cbo_Grid_MillName.Visible = True

                    cbo_Grid_MillName.BringToFront()
                    cbo_Grid_MillName.Focus()

                End If

            Else
                cbo_Grid_MillName.Visible = False

            End If

            If e.ColumnIndex = 6 Then

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

    Private Sub dgv_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellLeave
        With dgv_Details
            If .CurrentCell.ColumnIndex = 5 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.000")
                Else
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
                End If
            End If

            If .CurrentCell.ColumnIndex = 7 Or .CurrentCell.ColumnIndex = 8 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                Else
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
                End If
            End If
        End With
    End Sub

    Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged
        On Error Resume Next

        If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
        With dgv_Details
            If .Visible Then
                If .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 6 Or .CurrentCell.ColumnIndex = 7 Then

                    Amount_Calculation(e.RowIndex, e.ColumnIndex)

                End If
            End If
        End With

    End Sub

    Private Sub dgv_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Details.EditingControlShowing
        dgtxt_Details = CType(dgv_Details.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub
    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.Enter
        dgv_Details.EditingControl.BackColor = Color.Lime
        dgv_Details.EditingControl.ForeColor = Color.Blue
        dgtxt_Details.SelectAll()
    End Sub

    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Details.KeyPress
        Try
            With dgv_Details
                If .Visible Then
                    If .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 7 Then

                        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                            e.Handled = True
                        End If

                    End If
                End If
            End With

        Catch ex As Exception
            '--
        End Try

    End Sub

    Private Sub dgv_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyDown
        With dgv_Details

            If e.KeyCode = Keys.Up Then
                If .CurrentCell.ColumnIndex <= 1 Then
                    If .CurrentCell.RowIndex = 0 Then
                        txt_CommAmount.Focus()
                    Else
                        .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 1)
                    End If
                End If
            End If

            If e.KeyCode = Keys.Right Then
                If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then
                    If .CurrentCell.RowIndex >= .Rows.Count - 1 Then
                        txt_DiscPerc.Focus()
                    Else
                        .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)
                    End If
                End If
            End If

        End With

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

                For i = 0 To .Rows.Count - 1
                    .Rows(i).Cells(0).Value = i + 1
                Next

            End With

        End If
    End Sub

    Private Sub dgv_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.LostFocus
        On Error Resume Next
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_Details_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Details.RowsAdded
        Dim n As Integer = 0

        If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
        With dgv_Details
            .Rows(e.RowIndex).Cells(0).Value = Val(e.RowIndex) + 1
            If e.RowIndex > 0 Then
                .Rows(e.RowIndex).Cells(6).Value = Trim(UCase(.Rows(e.RowIndex - 1).Cells(6).Value))
            Else
                .Rows(e.RowIndex).Cells(6).Value = "BAG"
            End If
            'n = .RowCount
            '.Rows(n - 1).Cells(0).Value = Val(n)
        End With
    End Sub

    Private Sub cbo_Ledger_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyName.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
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

    Private Sub btn_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub txt_AddLess_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_AddLessAmount.KeyDown
        If e.KeyCode = 40 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()
            Else
                msk_date.Focus()
            End If
        End If
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_AddLess_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_AddLessAmount.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()
            Else
                msk_date.Focus()
            End If

        End If
    End Sub

    Private Sub txt_AddLess_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_AddLessAmount.LostFocus
        txt_AddLessAmount.Text = Format(Val(txt_AddLessAmount.Text), "#########0.00")
    End Sub

    Private Sub txt_AddLess_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_AddLessAmount.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub txt_Freight_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_FreightAmount.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Packing_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_FreightAmount.LostFocus
        txt_FreightAmount.Text = Format(Val(txt_FreightAmount.Text), "#########0.00")
    End Sub

    Private Sub txt_Packing_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_FreightAmount.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub txt_TaxPerc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_TaxPerc.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_VatPerc_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_TaxPerc.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub txt_DiscPerc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_DiscPerc.KeyDown
        If e.KeyValue = 38 Then
            If Trim(UCase(cbo_Type.Text)) = "DELIVERY" Then
                If dgv_Details.Rows.Count > 0 Then
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(3)
                Else
                    msk_OrderDate.Focus()
                End If
            Else
                If dgv_Details.Rows.Count > 0 Then
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
                Else
                    msk_OrderDate.Focus()
                End If
            End If

        End If

        If e.KeyValue = 40 Then SendKeys.Send("{TAB}")

    End Sub

    Private Sub txt_DiscPerc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_DiscPerc.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            cbo_VatAc.Focus()
        End If
    End Sub

    Private Sub txt_DiscPerc_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_DiscPerc.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub txt_Yarn_Description_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_YarnDescription.KeyPress
        If Asc(e.KeyChar) = 13 Then
            'If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
            '    save_record()
            'Else
            '    msk_date.Focus()
            'End If
            cbo_Transport.Focus()
        End If
    End Sub

    Private Sub Amount_Calculation(ByVal CurRow As Integer, ByVal CurCol As Integer)
        On Error Resume Next

        With dgv_Details
            If .Visible Then
                If CurCol = 3 Or CurCol = 5 Or CurCol = 6 Or CurCol = 7 Then

                    If Trim(UCase(.Rows(CurRow).Cells(6).Value)) = "BAG" Then
                        .Rows(CurRow).Cells(8).Value = Format(Val(.Rows(CurRow).Cells(3).Value) * Val(.Rows(CurRow).Cells(7).Value), "#########0.00")
                    Else
                        .Rows(CurRow).Cells(8).Value = Format(Val(.Rows(CurRow).Cells(5).Value) * Val(.Rows(CurRow).Cells(7).Value), "#########0.00")
                    End If

                    Total_Calculation()

                End If
            End If
        End With

    End Sub

    Private Sub Total_Calculation()
        Dim Sno As Integer
        Dim TotBgs As Single
        Dim TotCns As Single
        Dim TotWgt As Single
        Dim TotAmt As Single

        If NoCalc_Status = True Then Exit Sub

        Sno = 0
        TotBgs = 0 : TotCns = 0 : TotWgt = 0 : TotAmt = 0

        With dgv_Details
            For i = 0 To .RowCount - 1
                Sno = Sno + 1
                .Rows(i).Cells(0).Value = Sno
                If Trim(.Rows(i).Cells(1).Value) <> "" And (Val(.Rows(i).Cells(3).Value) <> 0 Or Val(.Rows(i).Cells(6).Value) <> 0) Then

                    TotBgs = TotBgs + Val(.Rows(i).Cells(3).Value)
                    TotCns = TotCns + Val(.Rows(i).Cells(4).Value)
                    TotWgt = TotWgt + Val(.Rows(i).Cells(5).Value)
                    TotAmt = TotAmt + Val(.Rows(i).Cells(8).Value)

                End If

            Next

        End With

        lbl_GrossAmount.Text = Format(Val(TotAmt), "########0.00")

        With dgv_Details_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(3).Value = Val(TotBgs)
            .Rows(0).Cells(4).Value = Val(TotCns)
            .Rows(0).Cells(5).Value = Format(Val(TotWgt), "########0.000")
            .Rows(0).Cells(8).Value = Format(Val(TotAmt), "########0.00")
        End With

        Agent_Commission_Calculation()

        NetAmount_Calculation()

    End Sub

    Private Sub Agent_Commission_Calculation()
        Dim AgCommAmt As Single = 0
        Dim TotBags As Integer = 0

        With dgv_Details_Total
            If .RowCount > 0 Then
                TotBags = Val(.Rows(0).Cells(3).Value)
            End If
        End With

        If Trim(UCase(cbo_CommType.Text)) = "%" Then
            AgCommAmt = Val(lbl_GrossAmount.Text) * Val(txt_CommRate.Text) / 100
        Else
            AgCommAmt = Val(TotBags) * Val(txt_CommRate.Text)
        End If

        txt_CommAmount.Text = Format(Val(AgCommAmt), "#########0.00")

    End Sub

    Private Sub NetAmount_Calculation()
        Dim NtAmt As Single

        If NoCalc_Status = True Then Exit Sub

        lbl_DiscAmount.Text = Format(Val(lbl_GrossAmount.Text) * Val(txt_DiscPerc.Text) / 100, "########0.00")

        lbl_AssessableValue.Text = Format(Val(lbl_GrossAmount.Text) - Val(lbl_DiscAmount.Text), "########0.00")

        lbl_TaxAmount.Text = Format(Val(lbl_AssessableValue.Text) * Val(txt_TaxPerc.Text) / 100, "########0.00")

        lbl_ChessAmount.Text = Format(Val(lbl_AssessableValue.Text) * Val(txt_ChessPerc.Text) / 100, "########0.00")

        lbl_ExtraTax_Amount.Text = Format(Val(lbl_AssessableValue.Text) * Val(txt_ExtraTaxPerc.Text) / 100, "########0.00")

        NtAmt = Val(lbl_AssessableValue.Text) + Val(lbl_TaxAmount.Text) + Val(lbl_ChessAmount.Text) + Val(lbl_ExtraTax_Amount.Text) + Val(txt_FreightAmount.Text) + Val(txt_Packing.Text) + Val(txt_AddLessAmount.Text)

        lbl_NetAmount.Text = Format(Val(NtAmt), "##########0")
        lbl_NetAmount.Text = Common_Procedures.Currency_Format(Val(lbl_NetAmount.Text))

        lbl_RoundOff.Text = Format(Val(CSng(lbl_NetAmount.Text)) - Val(NtAmt), "#########0.00")

        lbl_AmountInWords.Text = "Rupees  :  "
        If Val(CSng(lbl_NetAmount.Text)) <> 0 Then
            lbl_AmountInWords.Text = "Rupees  :  " & Common_Procedures.Rupees_Converstion(Val(CSng(lbl_NetAmount.Text)))
        End If

    End Sub

    Private Sub btn_Print_Cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_Cancel.Click
        btn_print_Close_Click(sender, e)
    End Sub


    Private Sub btn_print_Close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Print.Click

        pnl_Back.Enabled = True
        pnl_Print.Visible = False
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.PrintEntry, Common_Procedures.UR.Yarn_Proforma_Sales_Entry, New_Entry) = False Then Exit Sub



        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1027" Then
            pnl_Print.Visible = True
            pnl_Back.Enabled = False
            If btn_Print_oRDINARY.Enabled And btn_Print_oRDINARY.Visible Then
                btn_Print_oRDINARY.Focus()
            End If
        Else
            ' Print_Selection()
            ' btn_print_Close_Click(sender, e)


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

                    ppd.WindowState = FormWindowState.Maximized
                    ppd.StartPosition = FormStartPosition.CenterScreen
                    'ppd.ClientSize = New Size(600, 600)
                    ppd.PrintPreviewControl.AutoZoom = True
                    ppd.PrintPreviewControl.Zoom = 1
                    ppd.ShowDialog()

                Catch ex As Exception
                    MsgBox("The printing operation failed" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "DOES NOT SHOW PRINT PREVIEW...")

                End Try

            End If


            pnl_Back.Enabled = True
            pnl_Print.Visible = False
        End If

    End Sub

    Private Sub Print_Selection()

        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String
        Dim ps As Printing.PaperSize
        Dim PpSzSTS As Boolean = False


        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select * from Yarn_Sales_Prroforma_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Sales_Code = '" & Trim(NewCode) & "'", con)
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

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1111" Then ' -----------------Velan Spinning Mills 
            prn_InpOpts = ""
            prn_InpOpts = InputBox("Select    -   0. None" & Chr(13) & "                  1. Original" & Chr(13) & "                  2. Duplicate" & Chr(13) & "                  3. Triplicate" & Chr(13) & "                  4. HO Copy           5. All", "FOR INVOICE PRINTING...", "123")
            prn_InpOpts = Replace(Trim(prn_InpOpts), "5", "1234")

        Else
            prn_InpOpts = ""
            prn_InpOpts = InputBox("Select    -   0. None" & Chr(13) & "                  1. Original" & Chr(13) & "                  2. Duplicate" & Chr(13) & "                  3. Triplicate" & Chr(13) & "                  4. Extra Copy" & Space(10) & "                  5. All", "FOR INVOICE PRINTING...", "123")
            prn_InpOpts = Replace(Trim(prn_InpOpts), "5", "1234")

        End If



        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                PpSzSTS = True
                Exit For
            End If
        Next

        If PpSzSTS = False Then
            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                    PrintDocument1.DefaultPageSettings.PaperSize = ps
                    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
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

                        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
                                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                                PrintDocument1.DefaultPageSettings.PaperSize = ps
                                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                                PpSzSTS = True
                                Exit For
                            End If
                        Next

                        If PpSzSTS = False Then
                            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                                    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                                    PrintDocument1.DefaultPageSettings.PaperSize = ps
                                    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                                    Exit For
                                End If
                            Next
                        End If

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
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim NewCode As String
        Dim W1 As Single = 0

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0

        prn_Count = 0

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.*,d.Ledger_Name as TransportName,e.Ledger_Name as Agent_Name from Yarn_Sales_Prroforma_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Ledger_Head d ON d.Ledger_IdNo =a.Transport_IdNo  LEFT OUTER JOIN Ledger_Head e ON e.Ledger_IdNo = a.Agent_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Yarn_Sales_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.* , c.Mill_Name  from Yarn_Sales_Proforma_Details a INNER JOIN Count_Head b ON a.Count_IdNo = b.Count_IdNo LEFT OUTER JOIN Mill_Head c ON a.Mill_IdNo = c.Mill_IdNo where a.Yarn_Sales_Code = '" & Trim(NewCode) & "' Order by a.for_orderby, a.Yarn_Sales_No", con)
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



        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1046" Then '--Sri Mahalakshmi Mills (Textile)

            Printing_Format3(e)

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1111" Then  ' --- velan Spinning Mills

            Printing_Format4(e)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1118" Then  ' --- Kasthuri Tex (Coimbatore)

            Printing_Format5(e)

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1027" Then

            If prn_Status = 1 Then
                Printing_Format1(e)
            Else
                Printing_Format2(e)
            End If

        Else

            Printing_Format1(e)

        End If

    End Sub

    Private Sub Printing_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
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
        Dim ItmNm1 As String, ItmNm2 As String
        Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim W1 As Single = 0
        Dim SNo As Integer

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
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
                    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                    e.PageSettings.PaperSize = ps
                    Exit For
                End If
            Next
        End If

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 40
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

        NoofItems_PerPage = 5
8:

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = Val(50) : ClArr(2) = 120 : ClArr(3) = 220 : ClArr(4) = 75 : ClArr(5) = 110 : ClArr(6) = 75
        ClArr(7) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6))

        TxtHgt = 18.4 ' 18.5 ' 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format1_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)


                ' W1 = e.Graphics.MeasureString("No.of Beams  : ", pFont).Width

                NoofDets = 0

                CurY = CurY - 10

                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Yarn_Description").ToString, LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then
                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

                            e.HasMorePages = True
                            Return

                        End If

                        prn_DetSNo = prn_DetSNo + 1


                        ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Mill_Name").ToString)
                        ItmNm2 = ""
                        If Len(ItmNm1) > 25 Then
                            For I = 25 To 1 Step -1
                                If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 25
                            ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                            ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                        End If

                        CurY = CurY + TxtHgt

                        SNo = SNo + 1
                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(SNo)), LMargin + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Count_Name").ToString, LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Mill_Name").ToString, LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 5, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Weight").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 5, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Rate").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 5, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Amount").ToString), "#########0.00"), PageWidth - 5, CurY, 1, 0, pFont)

                        NoofDets = NoofDets + 1

                        If Trim(ItmNm2) <> "" Then
                            CurY = CurY + TxtHgt - 5
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                            NoofDets = NoofDets + 1
                        End If




                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If


                Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)

                If Trim(prn_InpOpts) <> "" Then
                    If prn_Count < Len(Trim(prn_InpOpts)) Then


                        If Val(prn_InpOpts) <> "0" Then
                            prn_DetIndx = 0
                            prn_DetSNo = 0
                            prn_PageNo = 0

                            e.HasMorePages = True
                            Return
                        End If

                    End If
                End If

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
        Dim strHeight As Single
        Dim C1 As Single, C2 As Single, W1 As Single, S1 As Single
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String, Cmp_PanNo As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
        Dim S As String

        PageNo = PageNo + 1

        prn_Count = prn_Count + 1

        prn_OriDupTri = ""
        If Trim(prn_InpOpts) <> "" Then
            If prn_Count <= Len(Trim(prn_InpOpts)) Then

                S = Mid$(Trim(prn_InpOpts), prn_Count, 1)

                If Val(S) = 1 Then
                    prn_OriDupTri = "ORIGINAL"
                ElseIf Val(S) = 2 Then
                    prn_OriDupTri = "DUPLICATE"
                ElseIf Val(S) = 3 Then
                    prn_OriDupTri = "TRIPLICATE"
                Else
                    If Trim(prn_InpOpts) <> "0" And Val(prn_InpOpts) = "0" And Len(Trim(prn_InpOpts)) > 4 Then
                        prn_OriDupTri = Trim(prn_InpOpts)
                    End If
                End If

            End If
        End If
        CurY = TMargin

        If Trim(prn_OriDupTri) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If

        '  CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name, c.Ledger_Name as Transport_Name from Yarn_Sales_Prroforma_Head a  INNER JOIN Ledger_Head b ON b.Ledger_IdNo = a.Ledger_Idno LEFT OUTER JOIN Ledger_Head c ON c.Ledger_IdNo = a.Transport_IdNo  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Yarn_Sales_Code = '" & Trim(EntryCode) & "' Order by a.For_OrderBy", con)
        da2.Fill(dt2)
        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        dt2.Clear()

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY


        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1065" Then '---- Logu textile

        '    If InStr(1, Trim(UCase(Cmp_Name)), "GANAPATHY") > 0 Or InStr(1, Trim(UCase(Cmp_Name)), "GANAPATHI") > 0 Then                                    '---- Ganapathy Spinning textile
        '        e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.GSM_LOGO, Drawing.Image), LMargin + 20, CurY, 112, 80)
        '    ElseIf InStr(1, Trim(UCase(Cmp_Name)), "LOGU") > 0 Or InStr(1, Trim(UCase(Cmp_Name)), "LOGA") > 0 Then                                          '---- Logu textile
        '        e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Company_Logo_LogaTex, Drawing.Image), LMargin + 20, CurY, 112, 80)
        '    End If

        'End If

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_PanNo = ""

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
        If Trim(prn_HdDt.Rows(0).Item("Company_PanNo").ToString) <> "" Then
            Cmp_PanNo = "PAN NO.: " & prn_HdDt.Rows(0).Item("Company_PanNo").ToString
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1065" Then '---- Logu textile

            If InStr(1, Trim(UCase(Cmp_Name)), "GANAPATHY") > 0 Or InStr(1, Trim(UCase(Cmp_Name)), "GANAPATHI") > 0 Then                                    '---- Ganapathy Spinning textile
                e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.GSM_LOGO, Drawing.Image), LMargin + 15, CurY + 5, 112, 80)
            ElseIf InStr(1, Trim(UCase(Cmp_Name)), "LOGU") > 0 Or InStr(1, Trim(UCase(Cmp_Name)), "LOGA") > 0 Then                                          '---- Logu textile
                e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Company_Logo_LogaTex, Drawing.Image), LMargin + 20, CurY + 5, 112, 80)
            End If

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
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1084" Then
            Common_Procedures.Print_To_PrintDocument(e, Cmp_PanNo, LMargin, CurY, 2, PrintWidth, pFont)
        End If
        Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "PROFORMA INVOICE", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try

            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 90
            W1 = e.Graphics.MeasureString("INVOICE NO     : ", pFont).Width
            S1 = e.Graphics.MeasureString("TO     :    ", pFont).Width

            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "TO  :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "Invoice No", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Yarn_Sales_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 14, FontStyle.Bold)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Invoice Date", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Yarn_Sales_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, " Tin No : " & prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

            C2 = ClAr(1) + ClAr(2) + ClAr(3) + 25

            e.Graphics.DrawLine(Pens.Black, LMargin + C1, LnAr(3), LMargin + C1, LnAr(2))
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "DELIVERY ADDRESS", LMargin + 10, CurY, 0, 0, p1Font)
            If Trim(prn_HdDt.Rows(0).Item("Agent_Name").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "Agent Name", LMargin + C2 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Agent_Name").ToString, LMargin + C2 + W1 + 30, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("Delivery_Address1").ToString), LMargin + S1 + 10, CurY, 0, 0, pFont)
            If Trim(prn_HdDt.Rows(0).Item("TransportName").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "Transport Name", LMargin + C2 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("TransportName").ToString, LMargin + C2 + W1 + 30, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Delivery_Address2").ToString), LMargin + S1 + 10, CurY, 0, 0, pFont)
            If Trim(prn_HdDt.Rows(0).Item("Lr_No").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "LR No", LMargin + C2 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Lr_No").ToString, LMargin + C2 + W1 + 30, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Delivery_Address3").ToString), LMargin + S1 + 10, CurY, 0, 0, pFont)
            If Trim(prn_HdDt.Rows(0).Item("Lr_Date").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "LR Date", LMargin + C2 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Lr_Date").ToString, LMargin + C2 + W1 + 30, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "Vehicle No", LMargin + C2 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vehicle_No").ToString, LMargin + C2 + W1 + 30, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "COUNT", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PARTICULARS", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, " BAG", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "TOT WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "RATE/KG ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format1_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim I As Integer
        Dim Cmp_Name As String
        Dim C1 As Single, W1 As Single
        Dim BInc As Integer
        Dim BnkDetAr() As String
        Dim BmsInWrds As String
        Dim vprn_BlNos As String = ""
        Dim BankNm1 As String = ""
        Dim BankNm2 As String = ""
        Dim BankNm3 As String = ""
        Dim BankNm4 As String = ""
        ' W1 = e.Graphics.MeasureString("No.of Beams  : ", pFont).Width
        p1Font = New Font("Calibri", 12, FontStyle.Bold)

        Try

            For I = NoofDets + 1 To NoofItems_PerPage

                CurY = CurY + TxtHgt

                prn_DetIndx = prn_DetIndx + 1

            Next

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + ClAr(2) + 30, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Total_Bags").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Total_Weight").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Total_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)


            CurY = CurY + TxtHgt

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(7) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))
            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 50
            W1 = e.Graphics.MeasureString("DISCOUNT    : ", pFont).Width

            ' If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1065" Then '---- LOGUtEX

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
            ' End If


            If Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Discount", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "( - )", LMargin + C1 + W1 + 7, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, BankNm1, LMargin + 10, CurY, 0, 0, p1Font)

            If Val(prn_HdDt.Rows(0).Item("Tax_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Tax_Type").ToString & " " & Val(prn_HdDt.Rows(0).Item("Tax_Percentage").ToString) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 30, CurY, 1, 0, pFont)
                'Common_Procedures.Print_To_PrintDocument(e, "VAT. 5 % ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 20, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "( + )", LMargin + C1 + W1 + 7, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Val(prn_HdDt.Rows(0).Item("Tax_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, BankNm2, LMargin + 10, CurY, 0, 0, p1Font)
            If Val(prn_HdDt.Rows(0).Item("Chess_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Chess_Name").ToString & " " & Val(prn_HdDt.Rows(0).Item("Chess_Percentage").ToString) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 30, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "( + )", LMargin + C1 + W1 + 7, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Val(prn_HdDt.Rows(0).Item("Chess_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, BankNm3, LMargin + 10, CurY, 0, 0, p1Font)
            If Val(prn_HdDt.Rows(0).Item("ExtraTax_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("ExtraTax_Name").ToString & " " & Val(prn_HdDt.Rows(0).Item("ExtraTax_Percentage").ToString) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 30, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "( + )", LMargin + C1 + W1 + 7, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Val(prn_HdDt.Rows(0).Item("ExtraTax_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, BankNm4, LMargin + 10, CurY, 0, 0, p1Font)

            If Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Freight_Name").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 30, CurY, 1, 0, pFont)
                'Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Freight_Name").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "( + )", LMargin + C1 + W1 + 7, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Freight_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            End If
            If Val(prn_HdDt.Rows(0).Item("Packing_Amount").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Packing_Name").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 30, CurY, 1, 0, pFont)
                'Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Packing_Name").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "( + )", LMargin + C1 + W1 + 7, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Packing_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            End If

            If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("AddLess_Name").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 30, CurY, 1, 0, pFont)
                'Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("AddLess_Name").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "( + )", LMargin + C1 + W1 + 7, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            End If

            If Val(prn_HdDt.Rows(0).Item("RoundOff_Amount").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "RoundOff ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "( + )", LMargin + C1 + W1 + 7, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("RoundOff_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, PageWidth, CurY)
            LnAr(8) = CurY
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "E & O.E", LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Total", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("Net_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(9) = CurY
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))
            CurY = CurY + TxtHgt
            BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))
            BmsInWrds = Replace(Trim(LCase(BmsInWrds)), "", "")

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1065" Then '---- Logu textile
                BmsInWrds = Trim(UCase(BmsInWrds))
            End If

            Common_Procedures.Print_To_PrintDocument(e, "Rupees            : " & BmsInWrds & " ", LMargin + 10, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            CurY = CurY + 10
            p1Font = New Font("Calibri", 12, FontStyle.Underline)
            Common_Procedures.Print_To_PrintDocument(e, "Term & Conditions : ", LMargin + 10, CurY, 0, 0, p1Font)

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1065" Then '---- Logu textile

                CurY = CurY + TxtHgt + 10
                Common_Procedures.Print_To_PrintDocument(e, "1. Interest will be Charged at 24% P.A for the overdue payments from the Date of Invoice. ", LMargin + 10, CurY, 0, 0, pFont)

                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "2. We are not responsible for any delay , Loss Or Damage During the Transport.", LMargin + 10, CurY, 0, 0, pFont)

                CurY = CurY + TxtHgt
                If Val(Common_Procedures.User.IdNo) <> 1 Then
                    Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
                End If

                'CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "3. Quality Complaint Will be accepted only in Grey Stage for Fabrics and Cotton Yarn Stage for Yarns. ", LMargin + 10, CurY, 0, 0, pFont)

                ' Common_Procedures.Print_To_PrintDocument(e, "For ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 30, CurY, 1, 0, p1Font)
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "4. Subject to Palladam jurisdiction Only.", LMargin + 10, CurY, 0, 0, pFont)

                CurY = CurY + TxtHgt + 10
                Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
                p1Font = New Font("Calibri", 12, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)
                CurY = CurY + TxtHgt
                CurY = CurY + TxtHgt
            Else

                CurY = CurY + TxtHgt + 10
                Common_Procedures.Print_To_PrintDocument(e, "1. We are responsible for the quality of yarn only;If any running fault or quality  ", LMargin + 10, CurY, 0, 0, pFont)

                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "   defect noted in yarn please inform with firat fabric roll at once.We will", LMargin + 10, CurY, 0, 0, pFont)
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "   accept only one roll at defect otherwise we do not hold ourself responsible. ", LMargin + 10, CurY, 0, 0, pFont)

                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "2. Our responsibility ceases when goods leave our permises.", LMargin + 10, CurY, 0, 0, pFont)

                CurY = CurY + TxtHgt
                ' If Val(Common_Procedures.User.IdNo) <> 1 Then
                '  Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
                'End If

                ' CurY = CurY + TxtHgt
                Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
                p1Font = New Font("Calibri", 12, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)
                Common_Procedures.Print_To_PrintDocument(e, "3. Subject to Tirupur jurisdiction. ", LMargin + 10, CurY, 0, 0, pFont)

                ' Common_Procedures.Print_To_PrintDocument(e, "For ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 30, CurY, 1, 0, p1Font)
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "4. Interest at the rate of 24% will be charge from the due date.", LMargin + 10, CurY, 0, 0, pFont)

                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "5. All payment should be made by A/C payer cheque or draft.", LMargin + 10, CurY, 0, 0, pFont)

            End If

            CurY = CurY + TxtHgt + 10

            Common_Procedures.Print_To_PrintDocument(e, "AUTHORISED SIGNATORY ", PageWidth - 5, CurY, 1, 0, pFont)
            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

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
        Dim CurY As Single = 0, CurX As Single = 0, TxtHgt As Single = 0
        Dim LnAr(15) As Single, ClArr(15) As Single
        'Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim W1 As Single = 0
        Dim ItmNm1 As String, ItmNm2 As String
        Dim I As Integer

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

                Printing_Format2_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                NoofDets = 0

                CurY = TMargin + 400
                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then

                            CurX = LMargin + 740
                            CurY = CurY + TxtHgt
                            Common_Procedures.Print_To_PrintDocument(e, "Continued....", CurX, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1
                            Printing_Format2_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

                            e.HasMorePages = True

                            Return

                        End If

                        prn_DetSNo = prn_DetSNo + 1


                        ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Count_Name").ToString) & "-" & (prn_DetDt.Rows(prn_DetIndx).Item("Mill_Name").ToString)


                        ItmNm2 = ""
                        If Len(ItmNm1) > 35 Then
                            For I = 35 To 1 Step -1
                                If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 35
                            ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                            ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                        End If

                        CurY = CurY + TxtHgt

                        CurX = LMargin + 40
                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetDt.Rows(prn_DetIndx).Item("Sl_No").ToString)), CurX, CurY, 0, 0, pFont)

                        CurX = LMargin + 100
                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), CurX, CurY, 0, 0, pFont)


                        CurX = LMargin + 440
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Weight").ToString), "#########0.000"), CurX, CurY, 1, 0, pFont)

                        'Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Weights").ToString), "#########0.000"), CurX, CurY, 1, 0, pFont)

                        CurX = LMargin + 580
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Rate").ToString), "#########0.00"), CurX, CurY, 1, 0, pFont)
                        CurX = LMargin + 740
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Amount").ToString), "#########0.00"), CurX, CurY, 1, 0, pFont)
                        NoofDets = NoofDets + 1

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
        Dim strHeight As Single = 0, strWidth As Single = 0
        Dim CurX As Single = 0
        Dim C1 As Single = 0, W1 As Single = 0, S1 As Single = 0
        Dim NewCode As String

        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            PageNo = PageNo + 1

            'da2 = New SqlClient.SqlDataAdapter("Select a.* , b.EndsCount_Name, c.Count_Name from BobinSales_Invoice_Details a LEFT OUTER JOIN ENDSCOUNT_Head b oN a.EndsCount_Idno = b.EndsCount_Idno LEFT OUTER JOIN COUNT_Head c oN a.Count_Idno = c.Count_Idno Where a.BobinSales_Invoice_Code = '" & Trim(NewCode) & "' Order by a.sl_no", Con)
            'dt2 = New DataTable
            'da2.Fill(dt2)
            'If dt2.Rows.Count > NoofItems_PerPage Then
            '    CurY = TMargin
            '    CurX = LMargin + 740
            '    Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), CurX, CurY, 1, 0, pFont)
            'End If
            'dt2.Clear()

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
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Yarn_Sales_No").ToString, CurX, CurY, 0, 0, p1Font)

            CurX = LMargin + 620
            CurY = TMargin + 230
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Yarn_Sales_Date").ToString), "dd-MM-yyyy").ToString, CurX, CurY, 0, 0, pFont)


            'CurX = LMargin + 180
            'CurY = TMargin + 315
            'Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Yarn_Sales_No").ToString), CurX, CurY, 0, 0, pFont)
            'CurX = LMargin + 600
            'Common_Procedures.Print_To_PrintDocument(e, (Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Yarn_Sales_Date").ToString), "dd-MM-yyyy").ToString), CurX, CurY, 0, 0, pFont)

            CurX = LMargin + 180
            CurY = TMargin + 340
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Transport_Name").ToString), CurX, CurY, 0, 0, pFont)
            'CurX = LMargin + 600
            'Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Lr_No").ToString), CurX, CurY, 0, 0, pFont)
            'If IsDBNull(prn_HdDt.Rows(0).Item("Lr_Date").ToString) = False Then
            '    If Trim(prn_HdDt.Rows(0).Item("Lr_Date").ToString) <> "" Then
            '        If IsDate(prn_HdDt.Rows(0).Item("Lr_Date").ToString) = True Then
            '            strWidth = e.Graphics.MeasureString(Trim(prn_HdDt.Rows(0).Item("Lr_No").ToString), pFont).Width
            '            CurX = CurX + strWidth + 15
            '            Common_Procedures.Print_To_PrintDocument(e, "Dt. " & Trim(prn_HdDt.Rows(0).Item("Lr_Date").ToString), CurX, CurY, 0, 0, pFont)
            '        End If
            '    End If
            'End If


        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format2_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
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
        '  Dim I As Integer


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
            CurX = LMargin + 440
            Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Total_Weight").ToString), CurX, CurY, 1, 0, pFont)
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
            If Val(prn_HdDt.Rows(0).Item("Tax_Amount").ToString) <> 0 Then
                CurX = LMargin + 580
                CurY = TMargin + 650
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Tax_Type").ToString & " " & Val(prn_HdDt.Rows(0).Item("Tax_Percentage").ToString) & " %", CurX, CurY, 1, 0, pFont)
                'Common_Procedures.Print_To_PrintDocument(e, "Tax Amount " & "VAT. 5 % " & " (+) ", CurX, CurY, 1, 0, pFont)
                CurX = LMargin + 740
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Tax_Amount").ToString), CurX, CurY, 1, 0, pFont)
            End If

            'Common_Procedures.Print_To_PrintDocument(e, "Party Bobin (In Godown) : " & Trim(Val(EmpBob_Par)), LMargin + 10, CurY, 0, 0, pFont)
            If Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) <> 0 Then
                CurX = LMargin + 580
                CurY = TMargin + 670
                Common_Procedures.Print_To_PrintDocument(e, "Freight", CurX, CurY, 1, 0, pFont)
                CurX = LMargin + 740
                Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("Freight_Amount").ToString), CurX, CurY, 1, 0, pFont)
            End If


            If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then
                CurX = LMargin + 580
                CurY = TMargin + 690
                Common_Procedures.Print_To_PrintDocument(e, "Add/Less", CurX, CurY, 1, 0, pFont)
                CurX = LMargin + 740
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString), CurX, CurY, 1, 0, pFont)
            End If

            '  TtAmt = Format(Val(prn_HdDt.Rows(0).Item("total_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("Freight").ToString) + Val(prn_HdDt.Rows(0).Item("Insurance").ToString) + Val(prn_HdDt.Rows(0).Item("Packing_amount").ToString) - Val(prn_HdDt.Rows(0).Item("Trade_Discount_Perc").ToString) - Val(prn_HdDt.Rows(0).Item("Cash_Discount_Perc").ToString), "#########0.00")
            '
            If Val(prn_HdDt.Rows(0).Item("RoundOff_Amount").ToString) <> 0 Then
                CurX = LMargin + 580
                CurY = TMargin + 710
                'Common_Procedures.Print_To_PrintDocument(e, "RoundOff", curx, CurY, 1, 0, pFont)
                CurX = LMargin + 740
                Common_Procedures.Print_To_PrintDocument(e, " " & Format(Val(prn_HdDt.Rows(0).Item("RoundOff_Amount").ToString), "########0.00"), CurX, CurY, 1, 0, pFont)
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

    Private Sub cbo_Agent_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Agent.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'AGENT')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Agent_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Agent.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Agent, txt_Address3, txt_CommRate, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'AGENT')", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_Agent_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Agent.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Agent, txt_CommRate, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'AGENT')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_VatAc_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_VatAc.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 12)", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_VatAc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_VatAc.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_VatAc, txt_DiscPerc, cbo_TaxType, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 12)", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_VatAc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_VatAc.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_VatAc, cbo_TaxType, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 12)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Agent_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Agent.KeyUp

        If e.Control = False And e.KeyValue = 17 Then

            Dim f As New Agent_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Agent.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_SalesAc_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Cbo_SalesAc.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Common_Procedures.MDI_LedType = ""
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = Cbo_SalesAc.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_VatAc_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_VatAc.KeyUp
        If e.Control = False And e.KeyValue = 17 Then

            Common_Procedures.MDI_LedType = ""
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_VatAc.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_TaxType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_TaxType.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")

    End Sub

    Private Sub cbo_TaxType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_TaxType.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_TaxType, cbo_VatAc, txt_TaxPerc, "", "", "", "")

    End Sub

    Private Sub cbo_TaxType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_TaxType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_TaxType, txt_TaxPerc, "", "", "", "")
    End Sub

    Private Sub cbo_TaxType_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_TaxType.TextChanged
        If Trim(UCase(cbo_TaxType.Text)) = "" Or Trim(UCase(cbo_TaxType.Text)) = "-NIL-" Then txt_TaxPerc.Text = ""
    End Sub

    Private Sub cbo_Grid_ItemName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_CountName.GotFocus
        vCbo_ItmNm = Trim(cbo_Grid_CountName.Text)

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

    End Sub

    Private Sub cbo_Grid_ItemName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_CountName.KeyDown
        Dim dep_idno As Integer = 0

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_CountName, Nothing, Nothing, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

        With dgv_Details

            If (e.KeyValue = 38 And cbo_Grid_CountName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

                If Val(.CurrentCell.RowIndex) <= 0 Then
                    If txt_TimeofRemoval_Goods.Visible Then
                        txt_TimeofRemoval_Goods.Focus()
                    Else
                        msk_Lr_Date.Focus()
                    End If
                    'dgv_Details.Focus()
                    'dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
                    'dgv_Details.CurrentCell.Selected = True

                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(7)
                    .CurrentCell.Selected = True

                End If
            End If

            If (e.KeyValue = 40 And cbo_Grid_CountName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                If .CurrentCell.RowIndex = .Rows.Count - 1 And Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(1).Value) = "" Then
                    txt_DiscPerc.Focus()

                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

                End If

            End If

        End With

    End Sub

    Private Sub cbo_Grid_ItemName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_CountName.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        'Dim Cn_bag As Integer
        'Dim Wgt_Bag As Integer
        'Dim Wgt_Cn As Integer
        'Dim mill_idno As Integer = 0

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_CountName, Nothing, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            With dgv_Details

                .Focus()
                .Rows(.CurrentCell.RowIndex).Cells.Item(1).Value = Trim(cbo_Grid_CountName.Text)
                If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then
                    txt_DiscPerc.Focus()
                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                End If

            End With

        End If
        'If Asc(e.KeyChar) = 13 Then

        '    With dgv_Details

        '        If Val(.Rows(.CurrentRow.Index).Cells(3).Value) = 0 Or Trim(.Rows(.CurrentRow.Index).Cells(4).Value) = 0 Or Trim(UCase(vCbo_ItmNm)) <> Trim(UCase(cbo_Grid_CountName.Text)) Then

        '            mill_idno_idno = Common_Procedures.Processed_Item_NameToIdNo(con, Trim(cbo_Grid_CountName.Text))

        '            da = New SqlClient.SqlDataAdapter("select a.Meter_Qty, b.unit_name from Processed_Item_Head a left outer join unit_head b on a.unit_idno = b.unit_idno Where a.Processed_Item_IdNo = " & Str(Val(Itm_idno)), con)
        '            dt = New DataTable
        '            da.Fill(dt)

        '            Mtr_Qty = 0
        '            Unt_nm = ""
        '            If dt.Rows.Count > 0 Then
        '                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
        '                    Mtr_Qty = Val(dt.Rows(0).Item("Meter_Qty").ToString)
        '                    Unt_nm = Trim(dt.Rows(0).Item("unit_name").ToString)
        '                End If
        '            End If

        '            dt.Dispose()
        '            da.Dispose()

        '            If Val(Mtr_Qty) <> 0 Then .Rows(.CurrentRow.Index).Cells(4).Value = Format(Val(Mtr_Qty), "#########0.00")
        '            .Rows(dgv_Details.CurrentRow.Index).Cells(6).Value = Trim(Unt_nm)

        '        End If

        '        .Rows(.CurrentCell.RowIndex).Cells.Item(1).Value = Trim(cbo_Grid_CountName.Text)

        '        If .CurrentCell.RowIndex = .Rows.Count - 1 And Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(1).Value) = "" Then
        '            txt_DiscPerc.Focus()

        '        Else
        '            .Focus()
        '            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

        '        End If

        '    End With

        'End If

    End Sub

    Private Sub cbo_Grid_ItemName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_CountName.KeyUp

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


    Private Sub cbo_Grid_ItemName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_CountName.TextChanged
        Try
            If cbo_Grid_CountName.Visible Then
                If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
                With dgv_Details
                    If Val(cbo_Grid_CountName.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_CountName.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Grid_millName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_MillName.GotFocus
        vCbo_ItmNm = Trim(cbo_Grid_MillName.Text)

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")

    End Sub

    Private Sub cbo_Grid_RackNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_MillName.KeyDown

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_MillName, Nothing, Nothing, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")

        With dgv_Details

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

    Private Sub cbo_Grid_RackNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_MillName.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_MillName, Nothing, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            With dgv_Details

                .Focus()
                .Rows(.CurrentCell.RowIndex).Cells.Item(2).Value = Trim(cbo_Grid_MillName.Text)
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

            End With

        End If

    End Sub

    Private Sub cbo_Grid_RackNo_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_MillName.KeyUp
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

    Private Sub cbo_Grid_MillName_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_MillName.LostFocus


        'If Trim(UCase(cbo_Grid_MillName.Tag)) <> Trim(UCase(cbo_Grid_MillName.Text)) Then
        '    get_MillCount_Details()
        'End If
    End Sub

    Private Sub cbo_Grid_RackNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_MillName.TextChanged
        Try
            If cbo_Grid_MillName.Visible Then
                If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
                With dgv_Details
                    If Val(cbo_Grid_MillName.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 2 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_MillName.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Grid_RateFor_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_RateFor.GotFocus
        vCbo_ItmNm = Trim(cbo_Grid_RateFor.Text)
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")

    End Sub

    Private Sub cbo_Grid_RateFor_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_RateFor.KeyDown
        Dim dep_idno As Integer = 0

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_RateFor, Nothing, Nothing, "", "", "", "")


        With dgv_Details

            If (e.KeyValue = 38 And cbo_Grid_RateFor.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
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
                .Rows(.CurrentCell.RowIndex).Cells.Item(6).Value = Trim(cbo_Grid_RateFor.Text)
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

            End With

        End If

    End Sub

    Private Sub cbo_Filter_CountName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_CountName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

    End Sub
    Private Sub cbo_Filter_CountName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_CountName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_CountName, dtp_Filter_ToDate, cbo_Filter_PartyName, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

    End Sub

    Private Sub cbo_Filter_CountName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_CountName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_CountName, cbo_Filter_PartyName, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

    End Sub


    Private Sub txt_CommRate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_CommRate.KeyDown
        If e.KeyValue = 38 Then cbo_Agent.Focus()
        If e.KeyValue = 40 Then
            txt_YarnDescription.Focus()
        End If
    End Sub

    Private Sub txt_Commbag_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_CommRate.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            txt_YarnDescription.Focus()
        End If
    End Sub

    Private Sub cbo_CommType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_CommType.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")

    End Sub

    Private Sub cbo_CommType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_CommType.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_CommType, txt_CommRate, Nothing, "", "", "", "")
        If e.KeyValue = 40 Then
            txt_YarnDescription.Focus()
        End If

    End Sub

    Private Sub cbo_CommType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_CommType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_CommType, Nothing, "", "", "", "")
        If Asc(e.KeyChar) = 13 Then
            txt_YarnDescription.Focus()
        End If
    End Sub


    Private Sub txt_CommRate_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_CommRate.TextChanged
        Agent_Commission_Calculation()
    End Sub


    Private Sub cbo_Grid_RateFor_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_RateFor.TextChanged
        Try
            If cbo_Grid_RateFor.Visible Then
                If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
                With dgv_Details
                    If Val(cbo_Grid_RateFor.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 6 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_RateFor.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub lbl_NetAmount_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbl_NetAmount.TextChanged
        lbl_AmountInWords.Text = "Rupees  :  "
        If Val(CSng(lbl_NetAmount.Text)) <> 0 Then
            lbl_AmountInWords.Text = "Rupees  :  " & Common_Procedures.Rupees_Converstion(Val(CSng(lbl_NetAmount.Text)))
        End If
    End Sub

    Private Sub cbo_CommType_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_CommType.TextChanged
        Agent_Commission_Calculation()
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

    Private Sub btn_Print_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        print_record()
    End Sub
    Private Sub dgtxt_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyUp
        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            dgv_Details_KeyUp(sender, e)
        End If
    End Sub

    Private Sub msk_date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles msk_date.KeyPress
        If Trim(UCase(e.KeyChar)) = "D" Then
            msk_date.Text = Date.Today
            msk_date.SelectionStart = 0
        End If
    End Sub

    Private Sub msk_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_date.KeyUp
        'If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
        '    msk_date.Text = Date.Today
        'End If
        If IsDate(msk_date.Text) = True Then
            If e.KeyCode = 107 Then
                msk_date.Text = DateAdd("D", 1, Convert.ToDateTime(msk_date.Text))
            ElseIf e.KeyCode = 109 Then
                msk_date.Text = DateAdd("D", -1, Convert.ToDateTime(msk_date.Text))
            End If
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

    Private Sub dtp_Date_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_Date.ValueChanged
        If IsDate(dtp_Date.Text) = True Then
            msk_date.Text = dtp_Date.Text
        End If
    End Sub

    Private Sub msk_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_date.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        If e.KeyValue = 38 Then e.Handled = True : e.SuppressKeyPress = True : txt_AddLessAmount.Focus()
        If e.KeyValue = 40 Then e.Handled = True : e.SuppressKeyPress = True : cbo_PartyName.Focus()
    End Sub

    Private Sub btn_Print_oRDINARY_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Print_oRDINARY.Click
        prn_Status = 1
        Print_Selection()
        btn_print_Close_Click(sender, e)
    End Sub

    Private Sub btn_Print_PrePrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Print_PrePrint.Click
        prn_Status = 2
        Print_Selection()
        btn_print_Close_Click(sender, e)
    End Sub

    Private Sub txt_LrNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_LrNo.KeyDown

        Try
            With txt_LrNo
                If e.KeyValue = 40 Then
                    msk_Lr_Date.Focus()
                End If

                If e.KeyValue = 38 Then
                    cbo_VehicleNo.Focus()
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub


    Private Sub txt_Chess_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_ChessPerc.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_extraTaxPerc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_ExtraTaxPerc.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_Chess_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_ChessPerc.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub txt_extraTaxPerc_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_ExtraTaxPerc.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub txt_LrNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_LrNo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            msk_Lr_Date.Focus()
        End If
    End Sub

    Private Sub txt_Packing_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Packing.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Packing_TextChanged1(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Packing.TextChanged
        NetAmount_Calculation()
    End Sub
    Private Sub Printing_Format3(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
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
        Dim ItmNm1 As String, ItmNm2 As String
        Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim W1 As Single = 0
        Dim SNo As Integer

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
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
                    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                    e.PageSettings.PaperSize = ps
                    Exit For
                End If
            Next
        End If

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 40
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

        NoofItems_PerPage = 7
8:

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = Val(50) : ClArr(2) = 100 : ClArr(3) = 220 : ClArr(4) = 75 : ClArr(5) = 110 : ClArr(6) = 75
        ClArr(7) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6))

        TxtHgt = 18.4 ' 18.5 ' 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format3_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)


                ' W1 = e.Graphics.MeasureString("No.of Beams  : ", pFont).Width

                NoofDets = 0

                CurY = CurY - 10

                If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1046" Then

                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Yarn_Description").ToString, LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)

                End If

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then
                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_Format3_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

                            e.HasMorePages = True
                            Return

                        End If

                        prn_DetSNo = prn_DetSNo + 1

                        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1046" Then
                            ItmNm1 = Trim(prn_HdDt.Rows(0).Item("Yarn_Description").ToString)
                        Else
                            ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Mill_Name").ToString)
                        End If


                        ItmNm2 = ""
                        If Len(ItmNm1) > 25 Then
                            For I = 25 To 1 Step -1
                                If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 25
                            ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                            ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                        End If

                        CurY = CurY + TxtHgt

                        SNo = SNo + 1
                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(SNo)), LMargin + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Count_Name").ToString, LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Mill_Name").ToString, LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 5, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Weight").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 5, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Rate").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 5, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Amount").ToString), "#########0.00"), PageWidth - 5, CurY, 1, 0, pFont)

                        NoofDets = NoofDets + 1

                        If Trim(ItmNm2) <> "" Then
                            CurY = CurY + TxtHgt - 5
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                            NoofDets = NoofDets + 1
                        End If




                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If


                Printing_Format3_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)

                If Trim(prn_InpOpts) <> "" Then
                    If prn_Count < Len(Trim(prn_InpOpts)) Then


                        If Val(prn_InpOpts) <> "0" Then
                            prn_DetIndx = 0
                            prn_DetSNo = 0
                            prn_PageNo = 0

                            e.HasMorePages = True
                            Return
                        End If

                    End If
                End If

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
        Dim strHeight As Single
        Dim C1 As Single, C2 As Single, W1 As Single, S1 As Single
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String, Cmp_PanNo As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
        Dim Cmp_Email As String
        Dim S As String

        PageNo = PageNo + 1

        prn_Count = prn_Count + 1

        prn_OriDupTri = ""
        If Trim(prn_InpOpts) <> "" Then
            If prn_Count <= Len(Trim(prn_InpOpts)) Then

                S = Mid$(Trim(prn_InpOpts), prn_Count, 1)

                If Val(S) = 1 Then
                    prn_OriDupTri = "ORIGINAL"
                ElseIf Val(S) = 2 Then
                    prn_OriDupTri = "DUPLICATE"
                ElseIf Val(S) = 3 Then
                    prn_OriDupTri = "TRIPLICATE"
                Else
                    If Trim(prn_InpOpts) <> "0" And Val(prn_InpOpts) = "0" And Len(Trim(prn_InpOpts)) > 4 Then
                        prn_OriDupTri = Trim(prn_InpOpts)
                    End If
                End If

            End If
        End If
        CurY = TMargin

        If Trim(prn_OriDupTri) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If

        '  CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name, c.Ledger_Name as Transport_Name from Yarn_Sales_Prroforma_Head a  INNER JOIN Ledger_Head b ON b.Ledger_IdNo = a.Ledger_Idno LEFT OUTER JOIN Ledger_Head c ON c.Ledger_IdNo = a.Transport_IdNo  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Yarn_Sales_Code = '" & Trim(EntryCode) & "' Order by a.For_OrderBy", con)
        da2.Fill(dt2)
        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        dt2.Clear()

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY



        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_PanNo = "" : Cmp_Email = ""

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
        If Trim(prn_HdDt.Rows(0).Item("Company_PanNo").ToString) <> "" Then
            Cmp_PanNo = "PAN NO.: " & prn_HdDt.Rows(0).Item("Company_PanNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
            Cmp_Email = "EMAIL: " & prn_HdDt.Rows(0).Item("Company_EMail").ToString
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1065" Then '---- Logu textile

            If InStr(1, Trim(UCase(Cmp_Name)), "GANAPATHY") > 0 Or InStr(1, Trim(UCase(Cmp_Name)), "GANAPATHI") > 0 Then                                    '---- Ganapathy Spinning textile
                e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.GSM_LOGO, Drawing.Image), LMargin + 15, CurY + 5, 112, 80)
            ElseIf InStr(1, Trim(UCase(Cmp_Name)), "LOGU") > 0 Or InStr(1, Trim(UCase(Cmp_Name)), "LOGA") > 0 Then                                          '---- Logu textile
                e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Company_Logo_LogaTex, Drawing.Image), LMargin + 20, CurY + 5, 112, 80)
            End If

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

        p1Font = New Font("Calibri", 9, FontStyle.Regular)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, p1Font)
        CurY = CurY + TxtHgt - 1

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1084" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1046" Then
            Common_Procedures.Print_To_PrintDocument(e, Cmp_PanNo, LMargin, CurY, 2, PrintWidth, p1Font)
        End If
        CurY = CurY + TxtHgt - 1
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1046" Then
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Email, LMargin, CurY, 2, PrintWidth, p1Font)
        End If

        CurY = CurY + TxtHgt
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, " INVOICE", LMargin, CurY, 2, PrintWidth, p1Font)

        Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY + 5, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY + 5, 1, 0, pFont)

        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


        CurY = CurY + strHeight
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try

            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 90
            W1 = e.Graphics.MeasureString("INVOICE NO     : ", pFont).Width
            S1 = e.Graphics.MeasureString("TO     :    ", pFont).Width

            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "TO  :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "Invoice No", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Yarn_Sales_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 14, FontStyle.Bold)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Invoice Date", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Yarn_Sales_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, " TIN NO.: " & prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

            C2 = ClAr(1) + ClAr(2) + ClAr(3) + 25

            e.Graphics.DrawLine(Pens.Black, LMargin + C1, LnAr(3), LMargin + C1, LnAr(2))

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "Agent Name", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Agent_Name").ToString, LMargin + W1 + 30, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Despatch From", LMargin + C2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Despatch_From").ToString), LMargin + C2 + W1 + 30, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Order No.", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Order_No").ToString), LMargin + W1 + 30, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Despatch To", LMargin + C2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Despatch_To").ToString), LMargin + C2 + W1 + 30, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "LR NO.", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Lr_No").ToString, LMargin + W1 + 30, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Transport Name", LMargin + C2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("TransportName").ToString, LMargin + C2 + W1 + 30, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "LC NO.", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("LC_No").ToString, LMargin + W1 + 30, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Vehicle No", LMargin + C2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vehicle_No").ToString, LMargin + C2 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "COUNT", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PARTICULARS", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, " BAG", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "TOT WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "RATE/KG ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format3_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim I As Integer
        Dim Cmp_Name As String
        Dim C1 As Single, W1 As Single
        Dim BInc As Integer
        Dim BnkDetAr() As String
        Dim BmsInWrds As String
        Dim vprn_BlNos As String = ""
        Dim BankNm1 As String = ""
        Dim BankNm2 As String = ""
        Dim BankNm3 As String = ""
        Dim BankNm4 As String = ""
        Dim Tot_Pcks As Single = 0
        Dim Tot_Wgt As Single = 0
        Dim Tot_Amt As Single = 0
        ' W1 = e.Graphics.MeasureString("No.of Beams  : ", pFont).Width
        p1Font = New Font("Calibri", 12, FontStyle.Bold)

        Try

            For I = NoofDets + 1 To NoofItems_PerPage

                CurY = CurY + TxtHgt

                prn_DetIndx = prn_DetIndx + 1

            Next


            CurY = CurY + TxtHgt - 10

            If Val(prn_HdDt.Rows(0).Item("Total_Bags").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, IIf(Val(prn_HdDt.Rows(0).Item("Total_Bags").ToString) > 1, "BAG NO. : 1 TO " & Val(prn_HdDt.Rows(0).Item("Total_Bags").ToString), "BAG NO. : 1 "), LMargin + 160, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + ClAr(2) + 30, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Total_Bags").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Total_Weight").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Total_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)


            CurY = CurY + TxtHgt

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(7) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))
            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 50
            W1 = e.Graphics.MeasureString("DISCOUNT    : ", pFont).Width

            ' If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1065" Then '---- LOGUtEX

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
            ' End If


            If Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Discount", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 30, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "( - )", LMargin + C1 + W1, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, BankNm1, LMargin + 10, CurY, 0, 0, p1Font)

            If Val(prn_HdDt.Rows(0).Item("Tax_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Tax_Type").ToString & " " & Val(prn_HdDt.Rows(0).Item("Tax_Percentage").ToString) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 30, CurY, 0, 0, pFont)
                'Common_Procedures.Print_To_PrintDocument(e, "VAT. 5 % ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 20, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "( + )", LMargin + C1 + W1, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Tax_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, BankNm2, LMargin + 10, CurY, 0, 0, p1Font)
            If Val(prn_HdDt.Rows(0).Item("Chess_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Chess_Name").ToString & " " & Val(prn_HdDt.Rows(0).Item("Chess_Percentage").ToString) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 30, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "( + )", LMargin + C1 + W1, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Chess_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, BankNm3, LMargin + 10, CurY, 0, 0, p1Font)
            If Val(prn_HdDt.Rows(0).Item("ExtraTax_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("ExtraTax_Name").ToString & " " & Val(prn_HdDt.Rows(0).Item("ExtraTax_Percentage").ToString) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 30, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "( + )", LMargin + C1 + W1, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("ExtraTax_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, BankNm4, LMargin + 10, CurY, 0, 0, p1Font)

            If Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Freight_Name").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 30, CurY, 0, 0, pFont)
                'Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Freight_Name").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "( + )", LMargin + C1 + W1, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format((prn_HdDt.Rows(0).Item("Freight_Amount").ToString), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            End If
            If Val(prn_HdDt.Rows(0).Item("Packing_Amount").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Packing_Name").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 30, CurY, 0, 0, pFont)
                'Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Packing_Name").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "( + )", LMargin + C1 + W1, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Packing_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            End If

            If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("AddLess_Name").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 30, CurY, 0, 0, pFont)
                'Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("AddLess_Name").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "( + )", LMargin + C1 + W1, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            End If

            If Val(prn_HdDt.Rows(0).Item("RoundOff_Amount").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "RoundOff ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 30, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "( + )", LMargin + C1 + W1, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " " & Format((prn_HdDt.Rows(0).Item("RoundOff_Amount").ToString), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, PageWidth, CurY)
            LnAr(8) = CurY
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "E & O.E", LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Total", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 30, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("Net_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(9) = CurY
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))
            CurY = CurY + TxtHgt
            BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))
            BmsInWrds = Replace(Trim(LCase(BmsInWrds)), "", "")

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1065" Then '---- Logu textile
                BmsInWrds = Trim(UCase(BmsInWrds))
            End If

            BmsInWrds = Replace(BmsInWrds, LCase((Microsoft.VisualBasic.Left(BmsInWrds, 1))), UCase((Microsoft.VisualBasic.Left(BmsInWrds, 1))), 1)

            Common_Procedures.Print_To_PrintDocument(e, "Rupees            : " & BmsInWrds & " ", LMargin + 10, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            CurY = CurY + 10
            p1Font = New Font("Calibri", 12, FontStyle.Underline)
            Common_Procedures.Print_To_PrintDocument(e, "Term & Conditions : ", LMargin + 10, CurY, 0, 0, p1Font)

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1065" Then '---- Logu textile

                CurY = CurY + TxtHgt + 10
                Common_Procedures.Print_To_PrintDocument(e, "1. Interest will be Charged at 24% P.A for the overdue payments from the Date of Invoice. ", LMargin + 10, CurY, 0, 0, pFont)

                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "2. We are not responsible for any delay , Loss Or Damage During the Transport.", LMargin + 10, CurY, 0, 0, pFont)

                CurY = CurY + TxtHgt
                If Val(Common_Procedures.User.IdNo) <> 1 Then
                    Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
                End If

                'CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "3. Quality Complaint Will be accepted only in Grey Stage for Fabrics and Cotton Yarn Stage for Yarns. ", LMargin + 10, CurY, 0, 0, pFont)

                ' Common_Procedures.Print_To_PrintDocument(e, "For ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 30, CurY, 1, 0, p1Font)
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "4. Subject to Palladam jurisdiction Only.", LMargin + 10, CurY, 0, 0, pFont)

                CurY = CurY + TxtHgt + 10
                Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
                p1Font = New Font("Calibri", 12, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)
                CurY = CurY + TxtHgt
                CurY = CurY + TxtHgt
            Else

                CurY = CurY + TxtHgt + 10
                Common_Procedures.Print_To_PrintDocument(e, "1. We are responsible for the quality of yarn only;If any running fault or quality  ", LMargin + 10, CurY, 0, 0, pFont)

                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "   defect noted in yarn please inform with firat fabric roll at once.We will", LMargin + 10, CurY, 0, 0, pFont)
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "   accept only one roll at defect otherwise we do not hold ourself responsible. ", LMargin + 10, CurY, 0, 0, pFont)

                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "2. Our responsibility ceases when goods leave our permises.", LMargin + 10, CurY, 0, 0, pFont)

                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "3. Subject to Tirupur jurisdiction. ", LMargin + 10, CurY, 0, 0, pFont)

                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "4. Interest at the rate of 24% will be charge from the due date.", LMargin + 10, CurY, 0, 0, pFont)
                Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
                p1Font = New Font("Calibri", 12, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "5. All payment should be made by A/C payer cheque or draft.", LMargin + 10, CurY, 0, 0, pFont)

            End If

            CurY = CurY + TxtHgt + 10
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "AUTHORISED SIGNATORY ", PageWidth - 5, CurY, 1, 0, pFont)
            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub txt_DespatchFrom_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_DespatchFrom.KeyDown
        If e.KeyCode = 38 Then
            cbo_VehicleNo.Focus()
        End If
        If e.KeyCode = 40 Then
            txt_LrNo.Focus()
        End If
    End Sub
    Private Sub txt_DespatchFrom_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_DespatchFrom.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_LrNo.Focus()
        End If
    End Sub

    Private Sub txt_DespatchTo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_DespatchTo.KeyDown
        If e.KeyCode = 38 Then
            cbo_Through.Focus()
        End If
        If e.KeyCode = 40 Then
            txt_LC_No.Focus()
        End If
    End Sub
    Private Sub txt_DespatchTo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_DespatchTo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_LC_No.Focus()
        End If
    End Sub

    Private Sub txt_LC_No_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_LC_No.KeyDown
        If e.KeyCode = 38 Then
            txt_DespatchFrom.Focus()
        End If
        If e.KeyCode = 40 Then
            msk_LC_Date.Focus()
        End If
    End Sub
    Private Sub txt_LC_No_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_LC_No.KeyPress
        If Asc(e.KeyChar) = 13 Then
            msk_LC_Date.Focus()
        End If
    End Sub

    Private Sub txt_OrderNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_OrderNo.KeyDown
        If e.KeyCode = 38 Then
            If cbo_PackingType.Visible And cbo_PackingType.Enabled Then
                cbo_PackingType.Focus()
            ElseIf txt_LotNo.Visible And txt_LotNo.Enabled Then
                txt_LotNo.Focus()
            Else
                msk_LC_Date.Focus()
            End If
            txt_DespatchTo.Focus()
        End If
        If e.KeyCode = 40 Then
            msk_OrderDate.Focus()
        End If
    End Sub
    Private Sub txt_OrderNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_OrderNo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            msk_OrderDate.Focus()
        End If
    End Sub

    Private Sub msk_LC_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_LC_Date.KeyDown
        If e.KeyCode = 38 Then
            txt_LC_No.Focus()
        End If
        If e.KeyCode = 40 Then
            If txt_LotNo.Visible And txt_LotNo.Enabled Then
                txt_LotNo.Focus()
            ElseIf cbo_PackingType.Visible And cbo_PackingType.Enabled Then
                cbo_PackingType.Focus()
            Else
                txt_OrderNo.Focus()
            End If


        End If
    End Sub
    Private Sub msk_LC_Date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles msk_LC_Date.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If txt_LotNo.Visible And txt_LotNo.Enabled Then
                txt_LotNo.Focus()
            ElseIf cbo_PackingType.Visible And cbo_PackingType.Enabled Then
                cbo_PackingType.Focus()
            Else
                txt_OrderNo.Focus()
            End If

        End If
    End Sub
    Private Sub msk_OrderDate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_OrderDate.KeyDown
        If e.KeyValue = 40 Then
            If txt_DateAndTime_Invoice.Visible And txt_DateAndTime_Invoice.Enabled Then
                txt_DateAndTime_Invoice.Focus()
            Else
                If Trim(UCase(cbo_Type.Text)) = "DELIVERY" Then

                    If dgv_Details.Rows.Count > 0 Then
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(3)
                    Else
                        txt_DiscPerc.Focus()
                    End If
                Else
                    If dgv_Details.Rows.Count > 0 Then
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
                    Else
                        txt_DiscPerc.Focus()
                    End If
                End If
            End If

        End If

        If e.KeyValue = 38 Then
            txt_OrderNo.Focus()
        End If
    End Sub
    Private Sub msk_OrderDate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles msk_OrderDate.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If txt_DateAndTime_Invoice.Visible And txt_DateAndTime_Invoice.Enabled Then
                txt_DateAndTime_Invoice.Focus()
            Else
                If Trim(UCase(cbo_Type.Text)) = "DELIVERY" Then
                    If dgv_Details.Rows.Count > 0 Then
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(3)
                    Else
                        txt_DiscPerc.Focus()
                    End If
                Else
                    If dgv_Details.Rows.Count > 0 Then
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
                    Else
                        txt_DiscPerc.Focus()
                    End If
                End If

            End If

        End If
    End Sub

    Private Sub msk_Lr_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_Lr_Date.KeyDown

        If e.KeyValue = 40 Then
            cbo_Through.Focus()
        End If

        If e.KeyValue = 38 Then
            msk_Lr_Date.Focus()
        End If

    End Sub

    Private Sub msk_Lr_Date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles msk_Lr_Date.KeyPress
        If Asc(e.KeyChar) = 13 Then
            cbo_Through.Focus()
        End If
    End Sub
    Private Sub cbo_Type_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Type.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")

    End Sub

    Private Sub cbo_Type_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Type.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Type, msk_date, cbo_PartyName, "", "", "", "")

    End Sub

    Private Sub cbo_Type_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Type.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_TaxType, cbo_PartyName, "", "", "", "")
        If Asc(e.KeyChar) = 13 Then
            cbo_PartyName.Focus()
        End If
    End Sub
    Private Sub cbo_Type_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Type.TextChanged
        If Trim(UCase(cbo_Type.Text)) <> "DELIVERY" Then
            dgv_Details.AllowUserToAddRows = True
        Else
            dgv_Details.AllowUserToAddRows = False
        End If
    End Sub

    Private Sub btn_Selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Selection.Click
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim i As Integer, j As Integer, n As Integer, SNo As Integer
        Dim LedIdNo As Integer
        Dim NewCode As String
        Dim CompIDCondt As String
        Dim Ent_Bag As Single = 0
        Dim Ent_Wgt As Single = 0
        Dim Ent_Cone As Single = 0
        Dim Ent_Exc As Single = 0

        Dim nr As Single = 0

        LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyName.Text)

        If LedIdNo = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SELECT DELIVERY...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_PartyName.Enabled And cbo_PartyName.Visible Then cbo_PartyName.Focus()
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)


        CompIDCondt = "(a.company_idno = " & Str(Val(lbl_Company.Tag)) & ")"
        If Trim(UCase(Common_Procedures.settings.CompanyName)) = "-----~~~" Then
            CompIDCondt = ""
        End If


        With dgv_Selection

            .Rows.Clear()
            SNo = 0

            Da = New SqlClient.SqlDataAdapter("select a.* , b.* ,  e.Ledger_Name as Transportname,h.Bags As Ent_Bag,  h.Weight As Ent_Wgt, h.Cones As Ent_COne, g.Count_Name , I.Mill_Name   from Weaver_Sales_Yarn_Delivery_Head a INNER JOIN Weaver_Sales_Yarn_Delivery_Details b ON a.Weaver_Sales_Yarn_Delivery_Code = b.Weaver_Sales_Yarn_Delivery_Code INNER JOIN Count_Head g ON g.Count_Idno = b.Count_IdNo  LEFT OUTER JOIN Mill_Head i ON b.Mill_IdNo = i.Mill_IdNo   LEFT OUTER JOIN Ledger_Head e ON a.Transport_IdNo = e.Ledger_IdNo LEFT OUTER JOIN Yarn_Sales_Proforma_Details h ON h.Yarn_Sales_Code = '" & Trim(NewCode) & "' and b.Weaver_Sales_Yarn_Delivery_Code = h.Weaver_Sales_Yarn_Delivery_Code and b.Weaver_Sales_Yarn_Delivery_Detail_SlNo = h.Weaver_Sales_Yarn_Delivery_Detail_SlNo Where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.DeliveryTo_idNo = " & Str(Val(LedIdNo)) & " and ((b.Weight - b.Sales_Weight ) > 0 or h.Weight > 0 ) order by a.Weaver_Sales_Yarn_Delivery_Date, a.for_orderby, a.Weaver_Sales_Yarn_Delivery_No", con)
            Dt1 = New DataTable
            nr = Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()


                    Ent_Bag = 0
                    Ent_Wgt = 0
                    Ent_Cone = 0

                    If IsDBNull(Dt1.Rows(i).Item("Ent_Bag").ToString) = False Then
                        Ent_Bag = Val(Dt1.Rows(i).Item("Ent_Bag").ToString)
                    End If
                    If IsDBNull(Dt1.Rows(i).Item("Ent_Cone").ToString) = False Then
                        Ent_Cone = Val(Dt1.Rows(i).Item("Ent_Cone").ToString)
                    End If

                    If IsDBNull(Dt1.Rows(i).Item("Ent_Wgt").ToString) = False Then
                        Ent_Wgt = Val(Dt1.Rows(i).Item("Ent_Wgt").ToString)
                    End If



                    SNo = SNo + 1
                    .Rows(n).Cells(0).Value = Val(SNo)
                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Weaver_Sales_Yarn_Delivery_No").ToString
                    .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Weaver_Sales_Yarn_Delivery_Date").ToString), "dd-MM-yyyy")
                    .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Count_Name").ToString
                    .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Yarn_Type").ToString
                    .Rows(n).Cells(5).Value = Dt1.Rows(i).Item("Mill_Name").ToString
                    .Rows(n).Cells(6).Value = Format(Val(Dt1.Rows(i).Item("Bags").ToString) - Val(Dt1.Rows(i).Item("Sales_Bag").ToString) + Val(Ent_Bag), "#########0.00")
                    .Rows(n).Cells(7).Value = Format(Val(Dt1.Rows(i).Item("Cones").ToString) - Val(Dt1.Rows(i).Item("Sales_Cone").ToString) + Val(Ent_Cone), "#########0.00")
                    .Rows(n).Cells(8).Value = Format(Val(Dt1.Rows(i).Item("Weight").ToString) - Val(Dt1.Rows(i).Item("Sales_Weight").ToString) + Val(Ent_Wgt), "#########0.000")
                    .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("Bag_No").ToString
                    If Ent_Wgt > 0 Then
                        .Rows(n).Cells(10).Value = "1"
                        For j = 0 To .ColumnCount - 1
                            .Rows(n).Cells(j).Style.ForeColor = Color.Red
                        Next

                    Else
                        .Rows(n).Cells(10).Value = ""

                    End If
                    .Rows(n).Cells(11).Value = Dt1.Rows(i).Item("TransportName").ToString
                    .Rows(n).Cells(12).Value = Dt1.Rows(i).Item("Weaver_Sales_Yarn_Delivery_Code").ToString
                    .Rows(n).Cells(13).Value = Dt1.Rows(i).Item("Weaver_Sales_Yarn_Delivery_Detail_SlNo").ToString
                    .Rows(n).Cells(14).Value = Dt1.Rows(i).Item("Vechile_No").ToString
                    .Rows(n).Cells(15).Value = Ent_Bag
                    .Rows(n).Cells(16).Value = Ent_Cone
                    .Rows(n).Cells(17).Value = Ent_Wgt


                Next

            End If
            Dt1.Clear()

        End With

        pnl_Selection.Visible = True
        pnl_Back.Enabled = False
        '  pnl_Back.Visible = False
        dgv_Selection.Focus()

    End Sub

    Private Sub dgv_Selection_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Selection.CellClick
        Select_Piece(e.RowIndex)
    End Sub

    Private Sub Select_Piece(ByVal RwIndx As Integer)
        Dim i As Integer

        With dgv_Selection

            If .RowCount > 0 And RwIndx >= 0 Then

                .Rows(RwIndx).Cells(10).Value = (Val(.Rows(RwIndx).Cells(10).Value) + 1) Mod 2

                If Val(.Rows(RwIndx).Cells(10).Value) = 1 Then

                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
                    Next


                Else
                    .Rows(RwIndx).Cells(10).Value = ""

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

        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Space Then
            If dgv_Selection.CurrentCell.RowIndex >= 0 Then

                n = dgv_Selection.CurrentCell.RowIndex

                Select_Piece(n)

                e.Handled = True

            End If
        End If
    End Sub

    Private Sub btn_Close_Selection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Selection.Click
        YarnDelivery_Selection()
    End Sub

    Private Sub YarnDelivery_Selection()
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim n As Integer = 0
        Dim sno As Integer = 0
        Dim i As Integer = 0
        Dim j As Integer = 0

        dgv_Details.Rows.Clear()

        For i = 0 To dgv_Selection.RowCount - 1

            If Val(dgv_Selection.Rows(i).Cells(10).Value) = 1 Then


                n = dgv_Details.Rows.Add()
                sno = sno + 1
                dgv_Details.Rows(n).Cells(0).Value = Val(sno)
                dgv_Details.Rows(n).Cells(1).Value = dgv_Selection.Rows(i).Cells(3).Value
                dgv_Details.Rows(n).Cells(2).Value = dgv_Selection.Rows(i).Cells(5).Value
                dgv_Details.Rows(n).Cells(9).Value = dgv_Selection.Rows(i).Cells(9).Value
                dgv_Details.Rows(n).Cells(10).Value = dgv_Selection.Rows(i).Cells(1).Value
                cbo_VehicleNo.Text = dgv_Selection.Rows(i).Cells(14).Value
                cbo_Transport.Text = dgv_Selection.Rows(i).Cells(11).Value
                dgv_Details.Rows(n).Cells(11).Value = dgv_Selection.Rows(i).Cells(12).Value
                dgv_Details.Rows(n).Cells(12).Value = dgv_Selection.Rows(i).Cells(13).Value

                If Val(dgv_Selection.Rows(i).Cells(15).Value) <> 0 Then
                    dgv_Details.Rows(n).Cells(3).Value = dgv_Selection.Rows(i).Cells(15).Value
                Else
                    dgv_Details.Rows(n).Cells(3).Value = dgv_Selection.Rows(i).Cells(6).Value
                End If

                If Val(dgv_Selection.Rows(i).Cells(16).Value) <> 0 Then
                    dgv_Details.Rows(n).Cells(4).Value = dgv_Selection.Rows(i).Cells(16).Value
                Else
                    dgv_Details.Rows(n).Cells(4).Value = dgv_Selection.Rows(i).Cells(7).Value
                End If

                If Val(dgv_Selection.Rows(i).Cells(17).Value) <> 0 Then
                    dgv_Details.Rows(n).Cells(5).Value = dgv_Selection.Rows(i).Cells(17).Value
                Else
                    dgv_Details.Rows(n).Cells(5).Value = dgv_Selection.Rows(i).Cells(8).Value
                End If


            End If

        Next

        Total_Calculation()

        pnl_Back.Enabled = True
        '  pnl_Back.Visible = True
        pnl_Selection.Visible = False
        If Cbo_SalesAc.Enabled And Cbo_SalesAc.Visible Then Cbo_SalesAc.Focus()

    End Sub

    Private Sub Printing_Format4(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim cmd As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim EntryCode As String
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font, p1Font As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ItmNm1 As String, ItmNm2 As String
        Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim W1 As Single = 0
        Dim SNo As Integer
        Dim BagNo1 As String = "", BagNo2 As String = ""

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
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
                    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                    e.PageSettings.PaperSize = ps
                    Exit For
                End If
            Next
        End If

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 20
            .Right = 50
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

        NoofItems_PerPage = 9
8:

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = Val(40) : ClArr(2) = 300 : ClArr(3) = 100 : ClArr(4) = 100 : ClArr(5) = 90
        ClArr(6) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5))

        TxtHgt = 18.4 ' 18.5 ' 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format4_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)


                ' W1 = e.Graphics.MeasureString("No.of Beams  : ", pFont).Width

                NoofDets = 0

                CurY = CurY - 10

                ' CurY = CurY + TxtHgt
                '  Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Yarn_Description").ToString, LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then
                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_Format4_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

                            e.HasMorePages = True
                            Return

                        End If

                        prn_DetSNo = prn_DetSNo + 1


                        ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Count_Name").ToString) & "  " & Trim(prn_DetDt.Rows(prn_DetIndx).Item("Count_Description").ToString)
                        ItmNm2 = ""
                        If Len(ItmNm1) > 30 Then
                            For I = 30 To 1 Step -1
                                If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 30
                            ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                            ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                        End If

                        If Trim(prn_DetDt.Rows(prn_DetIndx).Item("Bag_No").ToString) <> "" Then
                            BagNo1 = "BAG NOs. : " & prn_DetDt.Rows(prn_DetIndx).Item("Bag_No").ToString() & ""
                            BagNo2 = ""
                            If Len(BagNo1) > 40 Then
                                For I = 40 To 1 Step -1
                                    If Mid$(Trim(BagNo1), I, 1) = " " Or Mid$(Trim(BagNo1), I, 1) = "," Or Mid$(Trim(BagNo1), I, 1) = "." Or Mid$(Trim(BagNo1), I, 1) = "-" Or Mid$(Trim(BagNo1), I, 1) = "/" Or Mid$(Trim(BagNo1), I, 1) = "_" Or Mid$(Trim(BagNo1), I, 1) = "\" Or Mid$(Trim(BagNo1), I, 1) = "[" Or Mid$(Trim(BagNo1), I, 1) = "]" Or Mid$(Trim(BagNo1), I, 1) = "{" Or Mid$(Trim(BagNo1), I, 1) = "}" Then Exit For
                                Next I
                                If I = 0 Then I = 40
                                BagNo2 = Microsoft.VisualBasic.Right(Trim(BagNo1), Len(BagNo1) - I)
                                BagNo1 = Microsoft.VisualBasic.Left(Trim(BagNo1), I - 1)
                            End If
                        End If


                        CurY = CurY + TxtHgt

                        SNo = SNo + 1
                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(SNo)), LMargin + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString), "#######0") & " Bags", LMargin + ClArr(1) + ClArr(2) + 15, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Weight").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Rate").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 5, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Amount").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)


                        NoofDets = NoofDets + 1


                        If Trim(ItmNm2) <> "" Then
                            CurY = CurY + TxtHgt - 5
                            NoofDets = NoofDets + 1
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                        End If

                        p1Font = New Font("Calibri", 10, FontStyle.Regular)

                        If Trim(BagNo1) <> "" Then
                            CurY = CurY + TxtHgt + TxtHgt
                            NoofDets = NoofDets + 2
                            Common_Procedures.Print_To_PrintDocument(e, BagNo1, LMargin + ClArr(1) + 10, CurY, 0, 0, p1Font)
                            If Trim(BagNo2) = "" Then CurY = CurY + TxtHgt : NoofDets = NoofDets + 1
                        End If

                        W1 = e.Graphics.MeasureString("BAG NOs. : ", p1Font).Width

                        If Trim(BagNo2) <> "" Then
                            CurY = CurY + TxtHgt - 5
                            NoofDets = NoofDets + 1
                            Common_Procedures.Print_To_PrintDocument(e, BagNo2, LMargin + ClArr(1) + 10, CurY, 0, 0, p1Font)
                            CurY = CurY + TxtHgt : NoofDets = NoofDets + 1
                        End If





                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If

                If Trim(prn_HdDt.Rows(0).Item("Lot_No").ToString) <> "" Then
                    CurY = CurY + TxtHgt - 5
                    NoofDets = NoofDets + 1
                    Common_Procedures.Print_To_PrintDocument(e, "LOT NO. : " & Trim(prn_HdDt.Rows(0).Item("Lot_No").ToString), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)

                End If
                CurY = CurY + 10

                p1Font = New Font("Calibri", 10, FontStyle.Bold Or FontStyle.Underline)
                If Val(prn_HdDt.Rows(0).Item("C_Form_Sales_Status").ToString) <> 0 Then
                    CurY = CurY + TxtHgt
                    NoofDets = NoofDets + 1
                    Common_Procedures.Print_To_PrintDocument(e, "Consignment Sales Against Form-F", LMargin + ClArr(1) + 10, CurY, 0, 0, p1Font)

                End If

                Printing_Format4_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)

                If Trim(prn_InpOpts) <> "" Then
                    If prn_Count < Len(Trim(prn_InpOpts)) Then


                        If Val(prn_InpOpts) <> "0" Then
                            prn_DetIndx = 0
                            prn_DetSNo = 0
                            prn_PageNo = 0

                            e.HasMorePages = True
                            Return
                        End If

                    End If
                End If

            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format4_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim strHeight As Single
        Dim C1 As Single, C2 As Single, W1 As Single, S1 As Single
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String, Cmp_PanNo As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_Email As String
        Dim S As String
        Dim CurY1 As Single = 0

        PageNo = PageNo + 1

        prn_Count = prn_Count + 1

        prn_OriDupTri = ""
        If Trim(prn_InpOpts) <> "" Then
            If prn_Count <= Len(Trim(prn_InpOpts)) Then

                S = Mid$(Trim(prn_InpOpts), prn_Count, 1)

                If Val(S) = 1 Then
                    prn_OriDupTri = "ORIGINAL"
                ElseIf Val(S) = 2 Then
                    prn_OriDupTri = "DUPLICATE"
                ElseIf Val(S) = 3 Then
                    prn_OriDupTri = "TRIPLICATE"
                ElseIf Val(S) = 4 Then
                    prn_OriDupTri = "HO COPY"
                Else
                    If Trim(prn_InpOpts) <> "0" And Val(prn_InpOpts) = "0" And Len(Trim(prn_InpOpts)) > 4 Then
                        prn_OriDupTri = Trim(prn_InpOpts)
                    End If
                End If

            End If
        End If
        CurY = TMargin

        If Trim(prn_OriDupTri) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If

        '  CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name, c.Ledger_Name as Transport_Name from Yarn_Sales_Prroforma_Head a  INNER JOIN Ledger_Head b ON b.Ledger_IdNo = a.Ledger_Idno LEFT OUTER JOIN Ledger_Head c ON c.Ledger_IdNo = a.Transport_IdNo  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Yarn_Sales_Code = '" & Trim(EntryCode) & "' Order by a.For_OrderBy", con)
        da2.Fill(dt2)
        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        dt2.Clear()

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY



        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = "" : Cmp_Email = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_PanNo = ""

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString
        Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Factory_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Factory_Address2").ToString & " " & prn_HdDt.Rows(0).Item("Company_Factory_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Factory_Address4").ToString

        If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "PHONE NO.:" & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
            Cmp_TinNo = "TIN NO. : " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString) <> "" Then
            Cmp_CstNo = "CST NO. :" & prn_HdDt.Rows(0).Item("Company_CstNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
            Cmp_Email = "EMAIL: " & prn_HdDt.Rows(0).Item("Company_EMail").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_PanNo").ToString) <> "" Then
            Cmp_PanNo = "PAN NO.: " & prn_HdDt.Rows(0).Item("Company_PanNo").ToString
        End If


        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1111" Then '-----Velan Spinning mills
            e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.vsm_logo, Drawing.Image), LMargin + 5, CurY + 5, 100, 100)
        End If


        'CurY = CurY + TxtHgt - 15
        'p1Font = New Font("Calibri", 16, FontStyle.Bold)
        'Common_Procedures.Print_To_PrintDocument(e, " INVOICE", LMargin, CurY, 2, PrintWidth, p1Font)
        'strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


        CurY = CurY + TxtHgt - 5
        p1Font = New Font("Calibri", 22, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        p1Font = New Font("Calibri", 9, FontStyle.Regular)
        CurY = CurY + strHeight - 1
        Common_Procedures.Print_To_PrintDocument(e, "Regd. Off: " & Cmp_Add2, LMargin, CurY, 2, PrintWidth, p1Font)

        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, "Mill Add.: " & Cmp_Add1, LMargin, CurY, 2, PrintWidth, p1Font)

        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, p1Font)

        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Email, LMargin, CurY, 2, PrintWidth, p1Font)

        'CurY = CurY + TxtHgt - 1
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_PanNo, LMargin, CurY, 2, PrintWidth, p1Font)

        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, p1Font)

        'CurY = CurY + TxtHgt - 15
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "PROFORMA INVOICE", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


        CurY = CurY + strHeight
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try

            C1 = ClAr(1) + ClAr(2) + ClAr(3) + 50
            W1 = e.Graphics.MeasureString("INVOICE NO    : ", pFont).Width
            S1 = e.Graphics.MeasureString("TO     :    ", pFont).Width

            CurY1 = CurY

            '---Left Sidr

            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "TO  :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)

            p1Font = New Font("Calibri", 11, FontStyle.Regular)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, p1Font)


            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, p1Font)


            CurY = CurY + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, " Tin No : " & prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString, LMargin + S1 + 10, CurY, 0, 0, p1Font)
            End If

            '---Right Side
            CurY1 = CurY1 + TxtHgt


            Common_Procedures.Print_To_PrintDocument(e, "Invoice No", LMargin + C1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY1, 0, 0, pFont)
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Yarn_Sales_No").ToString, LMargin + C1 + W1 + 30, CurY1, 0, 0, p1Font)

            p1Font = New Font("Calibri", 11, FontStyle.Regular)
            CurY1 = CurY1 + TxtHgt + 5
            Common_Procedures.Print_To_PrintDocument(e, "Invoice Date", LMargin + C1 + 10, CurY1, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY1, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Yarn_Sales_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY1, 0, 0, p1Font)

            If Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString) <> "" Then
                CurY1 = CurY1 + TxtHgt + 5
                Common_Procedures.Print_To_PrintDocument(e, "Vehicle No.", LMargin + C1 + 10, CurY1, 0, 0, p1Font)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY1, 0, 0, p1Font)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vehicle_No").ToString, LMargin + C1 + W1 + 30, CurY1, 0, 0, p1Font)
            End If

            If Trim(prn_HdDt.Rows(0).Item("Through_Name").ToString) <> "" Then
                CurY1 = CurY1 + TxtHgt + 5
                Common_Procedures.Print_To_PrintDocument(e, "Documn Thro", LMargin + C1 + 10, CurY1, 0, 0, p1Font)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY1, 0, 0, p1Font)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Through_Name").ToString, LMargin + C1 + W1 + 30, CurY1, 0, 0, p1Font)
            End If

            If CurY1 > CurY Then CurY = CurY1

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

            C2 = ClAr(1) + ClAr(2) + ClAr(3)

            e.Graphics.DrawLine(Pens.Black, LMargin + C1, LnAr(3), LMargin + C1, LnAr(2))
            CurY = CurY + 5

            p1Font = New Font("Calibri", 10, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, "Name of the Commodity : Cotton Yarn", LMargin + 10, CurY, 0, 0, p1Font)

            p1Font = New Font("Calibri", 9, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, "100 COTTON YARN DUTY EXEMPTED AS PER OUR CENTRAL ", PageWidth - 10, CurY, 1, 0, p1Font)
            CurY = CurY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, "GOVERNMENT BUDGET UNDER NOTIFICATION NO : 30/2004", PageWidth - 10, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY

            CurY = CurY + TxtHgt - 5
            Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "DESCRIPTION & SPECIFICATION OF GOODS", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "NO. OF BAGS", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "TOTAL KGS", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "RATE/KG", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)


            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format4_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim I As Integer
        Dim Cmp_Name As String
        Dim C1 As Single, W1 As Single
        Dim BInc As Integer
        Dim BnkDetAr() As String
        Dim BmsInWrds As String
        Dim vprn_BlNos As String = ""
        Dim BankNm1 As String = ""
        Dim BankNm2 As String = ""
        Dim BankNm3 As String = ""
        Dim BankNm4 As String = ""
        Dim vprn_DcNos As String = ""
        Dim DcNo1 As String = "", DcNo2 As String = ""
        Dim Rps1 As String = "", Rps2 As String = ""
        Dim CurY1 As Single = 0

        W1 = e.Graphics.MeasureString("No.of Beams                    : ", pFont).Width
        p1Font = New Font("Calibri", 12, FontStyle.Bold)

        Try

            For I = NoofDets + 1 To NoofItems_PerPage

                CurY = CurY + TxtHgt

                prn_DetIndx = prn_DetIndx + 1

            Next

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, PageWidth, CurY)
            LnAr(6) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(4))

            CurY = CurY + TxtHgt - 10

            Common_Procedures.Print_To_PrintDocument(e, "Net Assessable Value", LMargin + ClAr(1) + ClAr(2) + 50, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Total_Amount").ToString), PageWidth - 10, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(7) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(4))

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(4))

            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10
            W1 = e.Graphics.MeasureString("DISCOUNT                            : ", pFont).Width

            'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1111" Then '---- velan spinning mills

            'Erase BnkDetAr
            'If Trim(prn_HdDt.Rows(0).Item("Company_Bank_Ac_Details").ToString) <> "" Then
            '    BnkDetAr = Split(Trim(prn_HdDt.Rows(0).Item("Company_Bank_Ac_Details").ToString), ",")

            '    BInc = -1

            '    BInc = BInc + 1
            '    If UBound(BnkDetAr) >= BInc Then
            '        BankNm1 = Trim(BnkDetAr(BInc))
            '    End If

            '    BInc = BInc + 1
            '    If UBound(BnkDetAr) >= BInc Then
            '        BankNm2 = Trim(BnkDetAr(BInc))
            '    End If

            '    BInc = BInc + 1
            '    If UBound(BnkDetAr) >= BInc Then
            '        BankNm3 = Trim(BnkDetAr(BInc))
            '    End If

            '    BInc = BInc + 1
            '    If UBound(BnkDetAr) >= BInc Then
            '        BankNm4 = Trim(BnkDetAr(BInc))
            '    End If

            'End If
            'End If
            'CurY1 = CurY

            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "  " & Trim(BankNm1), LMargin + 30, CurY, 0, 0, p1Font)
            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "  " & Trim(BankNm2), LMargin + 30, CurY, 0, 0, p1Font)
            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "  " & Trim(BankNm3), LMargin + 30, CurY, 0, 0, p1Font)
            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "  " & Trim(BankNm4), LMargin + 30, CurY, 0, 0, p1Font)


            CurY1 = CurY

            p1Font = New Font("Calibri", 10, FontStyle.Bold Or FontStyle.Underline)
            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "DELIVERY ADDRESS", LMargin + 10, CurY, 0, 0, p1Font)

            p1Font = New Font("Calibri", 10, FontStyle.Regular)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "  " & Trim(prn_HdDt.Rows(0).Item("Delivery_Address1").ToString), LMargin + 30, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "  " & Trim(prn_HdDt.Rows(0).Item("Delivery_Address2").ToString), LMargin + 30, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "  " & Trim(prn_HdDt.Rows(0).Item("Delivery_Address3").ToString), LMargin + 30, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth - (ClAr(6) + ClAr(5) + ClAr(4)), CurY)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Date & Time of Pre of Inv", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Date_of_Inv_Preparation").ToString), LMargin + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            If prn_DetDt.Rows.Count > 0 Then
                DcNo1 = "" : DcNo2 = ""
                For I = 0 To prn_DetDt.Rows.Count - 1
                    If Trim((prn_DetDt.Rows(I).Item("Delivery_No").ToString)) <> "" Then
                        If InStr(1, Trim(UCase(DcNo2)), "~" & Trim(prn_DetDt.Rows(I).Item("Delivery_No").ToString)) = 0 Then

                            DcNo1 = Trim(DcNo1) & IIf(Trim(DcNo1) <> "", " ,", "") & Trim(prn_DetDt.Rows(I).Item("Delivery_No").ToString)

                            DcNo2 = Trim(DcNo2) & "~" & Trim(prn_DetDt.Rows(I).Item("Delivery_No").ToString) & "~"
                        End If

                    End If
                Next
                DcNo2 = ""
                If Len(DcNo1) > 15 Then
                    For I = 15 To 1 Step -1
                        If Mid$(Trim(DcNo1), I, 1) = " " Or Mid$(Trim(DcNo1), I, 1) = "," Or Mid$(Trim(DcNo1), I, 1) = "." Or Mid$(Trim(DcNo1), I, 1) = "-" Or Mid$(Trim(DcNo1), I, 1) = "/" Or Mid$(Trim(DcNo1), I, 1) = "_" Or Mid$(Trim(DcNo1), I, 1) = "(" Or Mid$(Trim(DcNo1), I, 1) = ")" Or Mid$(Trim(DcNo1), I, 1) = "\" Or Mid$(Trim(DcNo1), I, 1) = "[" Or Mid$(Trim(DcNo1), I, 1) = "]" Or Mid$(Trim(DcNo1), I, 1) = "{" Or Mid$(Trim(DcNo1), I, 1) = "}" Then Exit For
                    Next I
                    If I = 0 Then I = 15
                    DcNo2 = Microsoft.VisualBasic.Right(Trim(DcNo1), Len(DcNo1) - I)
                    DcNo1 = Microsoft.VisualBasic.Left(Trim(DcNo1), I - 1)
                End If

                Common_Procedures.Print_To_PrintDocument(e, "DC No.", LMargin + 10, CurY, 0, 0, p1Font)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, p1Font)
                Common_Procedures.Print_To_PrintDocument(e, DcNo1, LMargin + W1 + 30, CurY, 0, 0, p1Font)

            End If


            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Time Of Removal Of Goods", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Time_Of_Removal_Of_Goods").ToString), LMargin + W1 + 30, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Rupees (In Words)", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 5


            BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))
            Rps1 = BmsInWrds
            Rps2 = ""
            If Len(Rps1) > 50 Then
                For I = 50 To 1 Step -1
                    If Mid$(Trim(Rps1), I, 1) = " " Or Mid$(Trim(Rps1), I, 1) = "," Or Mid$(Trim(Rps1), I, 1) = "." Or Mid$(Trim(Rps1), I, 1) = "-" Or Mid$(Trim(Rps1), I, 1) = "/" Or Mid$(Trim(Rps1), I, 1) = "_" Or Mid$(Trim(Rps1), I, 1) = "(" Or Mid$(Trim(Rps1), I, 1) = ")" Or Mid$(Trim(Rps1), I, 1) = "\" Or Mid$(Trim(Rps1), I, 1) = "[" Or Mid$(Trim(Rps1), I, 1) = "]" Or Mid$(Trim(Rps1), I, 1) = "{" Or Mid$(Trim(Rps1), I, 1) = "}" Then Exit For
                Next I
                If I = 0 Then I = 50
                Rps2 = Microsoft.VisualBasic.Right(Trim(Rps1), Len(Rps1) - I)
                Rps1 = Microsoft.VisualBasic.Left(Trim(Rps1), I - 1)
            End If
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, " " & Rps1 & " ", LMargin + 10, CurY, 0, 0, p1Font)

            If Rps2 <> "" Then
                CurY = CurY + TxtHgt + 5
                Common_Procedures.Print_To_PrintDocument(e, " " & Rps2 & " ", LMargin + 10, CurY, 0, 0, p1Font)
            End If

            ' Common_Procedures.Print_To_PrintDocument(e, "E & O.E", LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)


            '----Right Side


            If Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString) <> 0 Then
                CurY1 = CurY1 + TxtHgt - 10
                Common_Procedures.Print_To_PrintDocument(e, "Discount", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 10, CurY1, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "( - )", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY1, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString), "########0.00"), PageWidth - 10, CurY1, 1, 0, pFont)
            End If

            If Val(prn_HdDt.Rows(0).Item("Tax_Amount").ToString) <> 0 Then
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Tax_Type").ToString & " " & Val(prn_HdDt.Rows(0).Item("Tax_Percentage").ToString) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 10, CurY1, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "( + )", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY1, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Tax_Amount").ToString), "#########0.00"), PageWidth - 10, CurY1, 1, 0, pFont)
            End If

            If Val(prn_HdDt.Rows(0).Item("Chess_Amount").ToString) <> 0 Then
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Chess_Name").ToString & " " & Val(prn_HdDt.Rows(0).Item("Chess_Percentage").ToString) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 10, CurY1, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "( + )", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY1, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Chess_Amount").ToString), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY1, 1, 0, pFont)
            End If

            If Val(prn_HdDt.Rows(0).Item("ExtraTax_Amount").ToString) <> 0 Then
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("ExtraTax_Name").ToString & " " & Val(prn_HdDt.Rows(0).Item("ExtraTax_Percentage").ToString) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 10, CurY1, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "( + )", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY1, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("ExtraTax_Amount").ToString), "##########0.00"), PageWidth - 10, CurY1, 1, 0, pFont)
            End If

            If Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) <> 0 Then
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Freight_Name").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 10, CurY1, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "( + )", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY1, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString), "#########0.00"), PageWidth - 10, CurY1, 1, 0, pFont)
            End If
            If Val(prn_HdDt.Rows(0).Item("Packing_Amount").ToString) <> 0 Then
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Packing_Name").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 10, CurY1, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "( + )", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY1, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Packing_Amount").ToString), "#######0.00"), PageWidth - 10, CurY1, 1, 0, pFont)
            End If
            If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("AddLess_Name").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 10, CurY1, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "( + )", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY1, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString), "#########0.00"), PageWidth - 10, CurY1, 1, 0, pFont)
            End If


            If Val(prn_HdDt.Rows(0).Item("RoundOff_Amount").ToString) <> 0 Then
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "RoundOff ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 10, CurY1, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY1, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " " & Format(Val(prn_HdDt.Rows(0).Item("RoundOff_Amount").ToString), "#########0.00"), PageWidth - 10, CurY1, 1, 0, pFont)
            End If



            If CurY1 > CurY Then CurY = CurY1


            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, PageWidth, CurY)
            LnAr(8) = CurY
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(4))

            p1Font = New Font("Calibri", 15, FontStyle.Bold)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Net Amount", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 40, CurY, 1, 0, p1Font)

            p1Font = New Font("Calibri", 15, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("Net_Amount").ToString), PageWidth - 10, CurY, 1, 0, p1Font)


            CurY = CurY + TxtHgt + 15
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(9) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(7))

            CurY1 = 0
            CurY1 = CurY


            CurY = CurY + 10
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Term & Conditions : ", LMargin + 10, CurY, 0, 0, p1Font)


            CurY = CurY + TxtHgt + 10
            Common_Procedures.Print_To_PrintDocument(e, "1. Subject to Tirupur Jurisdiction Only ", LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "2. Goods once sold cannot be taken back.", LMargin + 10, CurY, 0, 0, pFont)


            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1111" Then '---- velan spinning mills

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
            End If



            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(BankNm1), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 10, CurY1, 0, 0, p1Font)
            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(BankNm2), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 10, CurY1, 0, 0, p1Font)
            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(BankNm3), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 10, CurY1, 0, 0, p1Font)
            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(BankNm4), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 10, CurY1, 0, 0, p1Font)

            If CurY1 > CurY Then CurY = CurY1



            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(9))

            'CurY = CurY + TxtHgt - 5
            'If Val(Common_Procedures.User.IdNo) <> 1 Then
            '    Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
            'End If


            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "Certified that the particulars given ", PageWidth - 30, CurY, 1, 0, pFont)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "above are true and correct ", PageWidth - 50, CurY, 1, 0, pFont)
            CurY = CurY + TxtHgt
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            p1Font = New Font("Calibri", 14, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt

            CurY = CurY + TxtHgt



            Common_Procedures.Print_To_PrintDocument(e, "Prepared ", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Checked ", LMargin + 250, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Authorised Signatory ", PageWidth - 5, CurY, 1, 0, pFont)
            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub
    Private Sub cbo_Through_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Through.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")

    End Sub
    Private Sub cbo_Through_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Through.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Through, msk_Lr_Date, txt_DespatchTo, "", "", "", "")
    End Sub

    Private Sub cbo_Through_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Through.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Through, txt_DespatchTo, "", "", "", "")
    End Sub

    Private Sub txt_DateAndTime_Invoice_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_DateAndTime_Invoice.KeyDown
        If e.KeyValue = 38 Then
            e.Handled = True
            msk_OrderDate.Focus()
        End If

        If e.KeyValue = 40 Then
            txt_TimeofRemoval_Goods.Focus()
            e.Handled = True
        End If

    End Sub

    Private Sub txt_DateAndTime_Invoice_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_DateAndTime_Invoice.KeyPress
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            txt_TimeofRemoval_Goods.Focus()
        End If

    End Sub

    Private Sub Printing_Format5(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
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
        Dim ItmNm1 As String, ItmNm2 As String
        Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim W1 As Single = 0
        Dim SNo As Integer

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
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
                    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                    e.PageSettings.PaperSize = ps
                    Exit For
                End If
            Next
        End If

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 40
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

        NoofItems_PerPage = 5
8:

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        '50,120
        ClArr(1) = 0 : ClArr(2) = 0 : ClArr(3) = 340 : ClArr(4) = 75 : ClArr(5) = 110 : ClArr(6) = 75
        ClArr(7) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6))

        TxtHgt = 18.4 ' 18.5 ' 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format5_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)


                ' W1 = e.Graphics.MeasureString("No.of Beams  : ", pFont).Width

                NoofDets = 0

                CurY = CurY - 10

                'CurY = CurY + TxtHgt
                'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Yarn_Description").ToString, LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then
                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

                            e.HasMorePages = True
                            Return

                        End If

                        prn_DetSNo = prn_DetSNo + 1


                        'ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Mill_Name").ToString)
                        ItmNm1 = Trim(prn_HdDt.Rows(0).Item("Yarn_Description").ToString)
                        ItmNm2 = ""
                        If Len(ItmNm1) > 25 Then
                            For I = 25 To 1 Step -1
                                If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 25
                            ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                            ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                        End If

                        CurY = CurY + TxtHgt

                        SNo = SNo + 1
                        '  Common_Procedures.Print_To_PrintDocument(e, Trim(Val(SNo)), LMargin + 10, CurY, 0, 0, pFont)
                        ' Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Count_Name").ToString, LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Mill_Name").ToString, LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 5, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Weight").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 5, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Rate").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 5, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Amount").ToString), "#########0.00"), PageWidth - 5, CurY, 1, 0, pFont)

                        NoofDets = NoofDets + 1

                        If Trim(ItmNm2) <> "" Then
                            CurY = CurY + TxtHgt - 5
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                            NoofDets = NoofDets + 1
                        End If




                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If


                Printing_Format5_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)

                If Trim(prn_InpOpts) <> "" Then
                    If prn_Count < Len(Trim(prn_InpOpts)) Then


                        If Val(prn_InpOpts) <> "0" Then
                            prn_DetIndx = 0
                            prn_DetSNo = 0
                            prn_PageNo = 0

                            e.HasMorePages = True
                            Return
                        End If

                    End If
                End If

            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format5_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim strHeight As Single
        Dim C1 As Single, C2 As Single, W1 As Single, S1 As Single
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String, Cmp_PanNo As String, Cmp_Email As String, Cmp_Fax As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
        Dim S As String
        Dim CurY1 As Double = 0

        PageNo = PageNo + 1

        prn_Count = prn_Count + 1

        prn_OriDupTri = ""
        If Trim(prn_InpOpts) <> "" Then
            If prn_Count <= Len(Trim(prn_InpOpts)) Then

                S = Mid$(Trim(prn_InpOpts), prn_Count, 1)

                If Val(S) = 1 Then
                    prn_OriDupTri = "ORIGINAL"
                ElseIf Val(S) = 2 Then
                    prn_OriDupTri = "DUPLICATE"
                ElseIf Val(S) = 3 Then
                    prn_OriDupTri = "TRIPLICATE"
                ElseIf Val(S) = 4 Then
                    prn_OriDupTri = "EXTRA COPY"
                Else
                    If Trim(prn_InpOpts) <> "0" And Val(prn_InpOpts) = "0" And Len(Trim(prn_InpOpts)) > 4 Then
                        prn_OriDupTri = Trim(prn_InpOpts)
                    End If
                End If

            End If
        End If
        CurY = TMargin

        If Trim(prn_OriDupTri) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If

        '  CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name, c.Ledger_Name as Transport_Name from Yarn_Sales_Prroforma_Head a  INNER JOIN Ledger_Head b ON b.Ledger_IdNo = a.Ledger_Idno LEFT OUTER JOIN Ledger_Head c ON c.Ledger_IdNo = a.Transport_IdNo  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Yarn_Sales_Code = '" & Trim(EntryCode) & "' Order by a.For_OrderBy", con)
        da2.Fill(dt2)
        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        dt2.Clear()
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY



        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = "" : Cmp_Email = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_PanNo = "" : Cmp_Fax = ""

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString
        Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Factory_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Factory_Address2").ToString & " " & prn_HdDt.Rows(0).Item("Company_Factory_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Factory_Address4").ToString

        If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "PHONE NO.:" & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_FaxNo").ToString) <> "" Then
            Cmp_Fax = "FAX:" & prn_HdDt.Rows(0).Item("Company_FaxNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
            Cmp_TinNo = "TIN NO. : " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString) <> "" Then
            Cmp_CstNo = "CST NO. :" & prn_HdDt.Rows(0).Item("Company_CstNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
            Cmp_Email = "EMAIL: " & prn_HdDt.Rows(0).Item("Company_EMail").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_PanNo").ToString) <> "" Then
            Cmp_PanNo = "PAN NO.: " & prn_HdDt.Rows(0).Item("Company_PanNo").ToString
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
        If Cmp_Fax <> "" Then
            If Cmp_PhNo <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo & ", " & Cmp_Fax, LMargin, CurY, 2, PrintWidth, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, Cmp_Fax, LMargin, CurY, 2, PrintWidth, pFont)
            End If
        Else
            Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
        End If



        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)

        ' CurY = CurY + TxtHgt
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, " PROFORMA INVOICE", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try

            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 90
            W1 = e.Graphics.MeasureString("INVOICE NO     : ", pFont).Width
            S1 = e.Graphics.MeasureString("TO     :    ", pFont).Width

            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "TO  :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "Invoice No", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Yarn_Sales_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 14, FontStyle.Bold)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Invoice Date", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Yarn_Sales_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, " Tin No : " & prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

            C2 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 5



            e.Graphics.DrawLine(Pens.Black, LMargin + C1, LnAr(3), LMargin + C1, LnAr(2))

            CurY1 = CurY - 10
            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "DELIVERY ADDRESS", LMargin + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("Delivery_Address1").ToString), LMargin + S1 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Delivery_Address2").ToString), LMargin + S1 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Delivery_Address3").ToString), LMargin + S1 + 10, CurY, 0, 0, pFont)


            '---

            If Trim(prn_HdDt.Rows(0).Item("Agent_Name").ToString) <> "" Then
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Agent Name", LMargin + C2 + 10, CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Agent_Name").ToString, LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)
            End If

            If Trim(prn_HdDt.Rows(0).Item("TransportName").ToString) <> "" Then
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Transport Name", LMargin + C2 + 10, CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("TransportName").ToString, LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)
            End If

            '  If Trim(prn_HdDt.Rows(0).Item("Lr_No").ToString) <> "" Then
            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "LR No", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Lr_No").ToString, LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)
            'End If

            ' If Trim(prn_HdDt.Rows(0).Item("Lr_Date").ToString) <> "" Then
            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "LR Date", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Lr_Date").ToString, LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)
            '  End If

            If Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString) <> "" Then
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Vehicle No", LMargin + C2 + 10, CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vehicle_No").ToString, LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)
            End If

            If CurY1 > CurY Then CurY = CurY1

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY

            CurY = CurY + TxtHgt
            '  Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin, CurY, 2, ClAr(1), pFont)
            '  Common_Procedures.Print_To_PrintDocument(e, "COUNT", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "DESCRIPTION OF GOODS", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1118" Then '---- Kasthuri Textiles (COIMBATORE)
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Packing_Type").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "BAG", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            End If

            Common_Procedures.Print_To_PrintDocument(e, "TOT WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "RATE/KG ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format5_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim I As Integer
        Dim Cmp_Name As String
        Dim C1 As Single, W1 As Single
        Dim BInc As Integer
        Dim BnkDetAr() As String
        Dim BmsInWrds As String
        Dim vprn_BlNos As String = ""
        Dim BankNm1 As String = ""
        Dim BankNm2 As String = ""
        Dim BankNm3 As String = ""
        Dim BankNm4 As String = ""
        Dim CurY1 As Double = 0
        Dim ItmNm As String = ""
        Dim ItmNm1 As String = ""

        ' W1 = e.Graphics.MeasureString("No.of Beams  : ", pFont).Width
        p1Font = New Font("Calibri", 12, FontStyle.Bold)

        Try

            For I = NoofDets + 1 To NoofItems_PerPage

                CurY = CurY + TxtHgt

                prn_DetIndx = prn_DetIndx + 1

            Next

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + ClAr(2) + 30, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Total_Bags").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Total_Weight").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Total_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)


            CurY = CurY + TxtHgt

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(7) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))
            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 50
            W1 = e.Graphics.MeasureString("DISCOUNT    : ", pFont).Width





            CurY1 = CurY
            '---Left Side

            p1Font = New Font("Calibri", 10, FontStyle.Regular Or FontStyle.Underline)
            If Trim(prn_HdDt.Rows(0).Item("Terms_Details1").ToString) <> "" Then
                ItmNm1 = ""
                ItmNm = ""
                ItmNm = Trim(prn_HdDt.Rows(0).Item("Terms_Details1").ToString)
                If Len(ItmNm) > 70 Then
                    For I = 70 To 1 Step -1
                        If Mid$(Trim(ItmNm), I, 1) = " " Or Mid$(Trim(ItmNm), I, 1) = "," Or Mid$(Trim(ItmNm), I, 1) = "." Or Mid$(Trim(ItmNm), I, 1) = "-" Or Mid$(Trim(ItmNm), I, 1) = "/" Or Mid$(Trim(ItmNm), I, 1) = "_" Or Mid$(Trim(ItmNm), I, 1) = "(" Or Mid$(Trim(ItmNm), I, 1) = ")" Or Mid$(Trim(ItmNm), I, 1) = "\" Or Mid$(Trim(ItmNm), I, 1) = "[" Or Mid$(Trim(ItmNm), I, 1) = "]" Or Mid$(Trim(ItmNm), I, 1) = "{" Or Mid$(Trim(ItmNm), I, 1) = "}" Then Exit For
                    Next I
                    If I = 0 Then I = 70
                    ItmNm1 = Microsoft.VisualBasic.Right(Trim(ItmNm), Len(ItmNm) - I)
                    ItmNm = Microsoft.VisualBasic.Left(Trim(ItmNm), I - 1)
                End If
                If Trim(prn_HdDt.Rows(0).Item("Terms_Label1").ToString) <> "" Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Terms_Label1").ToString, LMargin + 10, CurY, 0, 0, p1Font)
                    'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 5, CurY, 0, 0, pFont)
                End If
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, ItmNm, LMargin + 10, CurY, 0, 0, pFont)
                If ItmNm1 <> "" Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, ItmNm1, LMargin + 10, CurY, 0, 0, pFont)
                End If
            End If

            If Trim(prn_HdDt.Rows(0).Item("Terms_Details2").ToString) <> "" Then
                ItmNm1 = ""
                ItmNm = ""
                ItmNm = Trim(prn_HdDt.Rows(0).Item("Terms_Details2").ToString)
                If Len(ItmNm) > 70 Then
                    For I = 70 To 1 Step -1
                        If Mid$(Trim(ItmNm), I, 1) = " " Or Mid$(Trim(ItmNm), I, 1) = "," Or Mid$(Trim(ItmNm), I, 1) = "." Or Mid$(Trim(ItmNm), I, 1) = "-" Or Mid$(Trim(ItmNm), I, 1) = "/" Or Mid$(Trim(ItmNm), I, 1) = "_" Or Mid$(Trim(ItmNm), I, 1) = "(" Or Mid$(Trim(ItmNm), I, 1) = ")" Or Mid$(Trim(ItmNm), I, 1) = "\" Or Mid$(Trim(ItmNm), I, 1) = "[" Or Mid$(Trim(ItmNm), I, 1) = "]" Or Mid$(Trim(ItmNm), I, 1) = "{" Or Mid$(Trim(ItmNm), I, 1) = "}" Then Exit For
                    Next I
                    If I = 0 Then I = 70
                    ItmNm1 = Microsoft.VisualBasic.Right(Trim(ItmNm), Len(ItmNm) - I)
                    ItmNm = Microsoft.VisualBasic.Left(Trim(ItmNm), I - 1)
                End If
                If Trim(prn_HdDt.Rows(0).Item("Terms_Label2").ToString) <> "" Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Terms_Label2").ToString, LMargin + 10, CurY, 0, 0, p1Font)
                    'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 5, CurY, 0, 0, pFont)
                End If
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, ItmNm, LMargin + 10, CurY, 0, 0, pFont)
                If ItmNm1 <> "" Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, ItmNm1, LMargin + 10, CurY, 0, 0, pFont)
                End If
            End If

            If Trim(prn_HdDt.Rows(0).Item("Terms_Details3").ToString) <> "" Then
                ItmNm1 = ""
                ItmNm = ""
                ItmNm = Trim(prn_HdDt.Rows(0).Item("Terms_Details3").ToString)
                If Len(ItmNm) > 70 Then
                    For I = 70 To 1 Step -1
                        If Mid$(Trim(ItmNm), I, 1) = " " Or Mid$(Trim(ItmNm), I, 1) = "," Or Mid$(Trim(ItmNm), I, 1) = "." Or Mid$(Trim(ItmNm), I, 1) = "-" Or Mid$(Trim(ItmNm), I, 1) = "/" Or Mid$(Trim(ItmNm), I, 1) = "_" Or Mid$(Trim(ItmNm), I, 1) = "(" Or Mid$(Trim(ItmNm), I, 1) = ")" Or Mid$(Trim(ItmNm), I, 1) = "\" Or Mid$(Trim(ItmNm), I, 1) = "[" Or Mid$(Trim(ItmNm), I, 1) = "]" Or Mid$(Trim(ItmNm), I, 1) = "{" Or Mid$(Trim(ItmNm), I, 1) = "}" Then Exit For
                    Next I
                    If I = 0 Then I = 70
                    ItmNm1 = Microsoft.VisualBasic.Right(Trim(ItmNm), Len(ItmNm) - I)
                    ItmNm = Microsoft.VisualBasic.Left(Trim(ItmNm), I - 1)
                End If
                If Trim(prn_HdDt.Rows(0).Item("Terms_Label3").ToString) <> "" Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Terms_Label3").ToString, LMargin + 10, CurY, 0, 0, p1Font)
                    'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 5, CurY, 0, 0, pFont)
                End If
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, ItmNm, LMargin + 10, CurY, 0, 0, pFont)
                If ItmNm1 <> "" Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, ItmNm1, LMargin + 10, CurY, 0, 0, pFont)
                End If
            End If
            If Trim(prn_HdDt.Rows(0).Item("Terms_Details4").ToString) <> "" Then
                ItmNm1 = ""
                ItmNm = ""
                ItmNm = Trim(prn_HdDt.Rows(0).Item("Terms_Details4").ToString)
                If Len(ItmNm) > 70 Then
                    For I = 70 To 1 Step -1
                        If Mid$(Trim(ItmNm), I, 1) = " " Or Mid$(Trim(ItmNm), I, 1) = "," Or Mid$(Trim(ItmNm), I, 1) = "." Or Mid$(Trim(ItmNm), I, 1) = "-" Or Mid$(Trim(ItmNm), I, 1) = "/" Or Mid$(Trim(ItmNm), I, 1) = "_" Or Mid$(Trim(ItmNm), I, 1) = "(" Or Mid$(Trim(ItmNm), I, 1) = ")" Or Mid$(Trim(ItmNm), I, 1) = "\" Or Mid$(Trim(ItmNm), I, 1) = "[" Or Mid$(Trim(ItmNm), I, 1) = "]" Or Mid$(Trim(ItmNm), I, 1) = "{" Or Mid$(Trim(ItmNm), I, 1) = "}" Then Exit For
                    Next I
                    If I = 0 Then I = 70
                    ItmNm1 = Microsoft.VisualBasic.Right(Trim(ItmNm), Len(ItmNm) - I)
                    ItmNm = Microsoft.VisualBasic.Left(Trim(ItmNm), I - 1)
                End If
                If Trim(prn_HdDt.Rows(0).Item("Terms_Label4").ToString) <> "" Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Terms_Label4").ToString, LMargin + 10, CurY, 0, 0, p1Font)
                    'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 5, CurY, 0, 0, pFont)
                End If
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, ItmNm, LMargin + 10, CurY, 0, 0, pFont)
                If ItmNm1 <> "" Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, ItmNm1, LMargin + 10, CurY, 0, 0, pFont)
                End If
            End If

            If Trim(prn_HdDt.Rows(0).Item("Terms_Details5").ToString) <> "" Then
                ItmNm1 = ""
                ItmNm = ""
                ItmNm = Trim(prn_HdDt.Rows(0).Item("Terms_Details5").ToString)
                If Len(ItmNm) > 70 Then
                    For I = 70 To 1 Step -1
                        If Mid$(Trim(ItmNm), I, 1) = " " Or Mid$(Trim(ItmNm), I, 1) = "," Or Mid$(Trim(ItmNm), I, 1) = "." Or Mid$(Trim(ItmNm), I, 1) = "-" Or Mid$(Trim(ItmNm), I, 1) = "/" Or Mid$(Trim(ItmNm), I, 1) = "_" Or Mid$(Trim(ItmNm), I, 1) = "(" Or Mid$(Trim(ItmNm), I, 1) = ")" Or Mid$(Trim(ItmNm), I, 1) = "\" Or Mid$(Trim(ItmNm), I, 1) = "[" Or Mid$(Trim(ItmNm), I, 1) = "]" Or Mid$(Trim(ItmNm), I, 1) = "{" Or Mid$(Trim(ItmNm), I, 1) = "}" Then Exit For
                    Next I
                    If I = 0 Then I = 70
                    ItmNm1 = Microsoft.VisualBasic.Right(Trim(ItmNm), Len(ItmNm) - I)
                    ItmNm = Microsoft.VisualBasic.Left(Trim(ItmNm), I - 1)
                End If
                If Trim(prn_HdDt.Rows(0).Item("Terms_Label5").ToString) <> "" Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Terms_Label5").ToString, LMargin + 10, CurY, 0, 0, p1Font)
                    'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 5, CurY, 0, 0, pFont)
                End If
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, ItmNm, LMargin + 10, CurY, 0, 0, pFont)
                If ItmNm1 <> "" Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, ItmNm1, LMargin + 10, CurY, 0, 0, pFont)
                End If
            End If
            If Trim(prn_HdDt.Rows(0).Item("Terms_Details6").ToString) <> "" Then
                ItmNm1 = ""
                ItmNm = ""
                ItmNm = Trim(prn_HdDt.Rows(0).Item("Terms_Details6").ToString)
                If Len(ItmNm) > 70 Then
                    For I = 70 To 1 Step -1
                        If Mid$(Trim(ItmNm), I, 1) = " " Or Mid$(Trim(ItmNm), I, 1) = "," Or Mid$(Trim(ItmNm), I, 1) = "." Or Mid$(Trim(ItmNm), I, 1) = "-" Or Mid$(Trim(ItmNm), I, 1) = "/" Or Mid$(Trim(ItmNm), I, 1) = "_" Or Mid$(Trim(ItmNm), I, 1) = "(" Or Mid$(Trim(ItmNm), I, 1) = ")" Or Mid$(Trim(ItmNm), I, 1) = "\" Or Mid$(Trim(ItmNm), I, 1) = "[" Or Mid$(Trim(ItmNm), I, 1) = "]" Or Mid$(Trim(ItmNm), I, 1) = "{" Or Mid$(Trim(ItmNm), I, 1) = "}" Then Exit For
                    Next I
                    If I = 0 Then I = 70
                    ItmNm1 = Microsoft.VisualBasic.Right(Trim(ItmNm), Len(ItmNm) - I)
                    ItmNm = Microsoft.VisualBasic.Left(Trim(ItmNm), I - 1)
                End If
                If Trim(prn_HdDt.Rows(0).Item("Terms_Label6").ToString) <> "" Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Terms_Label6").ToString, LMargin + 10, CurY, 0, 0, p1Font)
                    'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 5, CurY, 0, 0, pFont)
                End If
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, ItmNm, LMargin + 10, CurY, 0, 0, pFont)
                If ItmNm1 <> "" Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, ItmNm1, LMargin + 10, CurY, 0, 0, pFont)
                End If
            End If

            If Trim(prn_HdDt.Rows(0).Item("Terms_Details7").ToString) <> "" Then
                ItmNm1 = ""
                ItmNm = ""
                ItmNm = Trim(prn_HdDt.Rows(0).Item("Terms_Details7").ToString)
                If Len(ItmNm) > 70 Then
                    For I = 70 To 1 Step -1
                        If Mid$(Trim(ItmNm), I, 1) = " " Or Mid$(Trim(ItmNm), I, 1) = "," Or Mid$(Trim(ItmNm), I, 1) = "." Or Mid$(Trim(ItmNm), I, 1) = "-" Or Mid$(Trim(ItmNm), I, 1) = "/" Or Mid$(Trim(ItmNm), I, 1) = "_" Or Mid$(Trim(ItmNm), I, 1) = "(" Or Mid$(Trim(ItmNm), I, 1) = ")" Or Mid$(Trim(ItmNm), I, 1) = "\" Or Mid$(Trim(ItmNm), I, 1) = "[" Or Mid$(Trim(ItmNm), I, 1) = "]" Or Mid$(Trim(ItmNm), I, 1) = "{" Or Mid$(Trim(ItmNm), I, 1) = "}" Then Exit For
                    Next I
                    If I = 0 Then I = 70
                    ItmNm1 = Microsoft.VisualBasic.Right(Trim(ItmNm), Len(ItmNm) - I)
                    ItmNm = Microsoft.VisualBasic.Left(Trim(ItmNm), I - 1)
                End If
                If Trim(prn_HdDt.Rows(0).Item("Terms_Label7").ToString) <> "" Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Terms_Label7").ToString, LMargin + 10, CurY, 0, 0, p1Font)
                    'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 5, CurY, 0, 0, pFont)
                End If
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, ItmNm, LMargin + 10, CurY, 0, 0, pFont)
                If ItmNm1 <> "" Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, ItmNm1, LMargin + 10, CurY, 0, 0, pFont)
                End If
            End If

            CurY = CurY + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("C_Form_Sales_Status").ToString) <> 0 Then
                p1Font = New Font("Calibri", 10, FontStyle.Regular Or FontStyle.Underline)
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Sales Against: ""C"" Form", LMargin + 10, CurY, 0, 0, p1Font)
            End If

            If Trim(prn_HdDt.Rows(0).Item("Sale_Type").ToString) = "E1.SALE" Then
                p1Font = New Font("Calibri", 10, FontStyle.Regular Or FontStyle.Underline)
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "E-1 SALES", LMargin + 10, CurY, 0, 0, p1Font)
            End If


            p1Font = New Font("Calibri", 12, FontStyle.Bold)
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
            ' End If
            CurY = CurY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, BankNm1, LMargin + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, BankNm2, LMargin + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, BankNm3, LMargin + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, BankNm4, LMargin + 10, CurY, 0, 0, p1Font)




            '--Right Side

            If Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString) <> 0 Then
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Discount", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 30, CurY1, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "( - )", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 5, CurY1, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY1, 1, 0, pFont)
            End If

            If Val(prn_HdDt.Rows(0).Item("Tax_Amount").ToString) <> 0 Then
                CurY1 = CurY1 + TxtHgt
                If Trim(prn_HdDt.Rows(0).Item("Sale_Type").ToString) = "C.S.T.SALE" Then
                    Common_Procedures.Print_To_PrintDocument(e, "CST " & " " & Val(prn_HdDt.Rows(0).Item("Tax_Percentage").ToString) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 30, CurY1, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, "( + )", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 5, CurY1, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Tax_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY1, 1, 0, pFont)
                ElseIf Trim(prn_HdDt.Rows(0).Item("Sale_Type").ToString) = "E1.SALE" Then
                    Common_Procedures.Print_To_PrintDocument(e, "CST " & " " & Val(prn_HdDt.Rows(0).Item("Tax_Percentage").ToString) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 30, CurY1, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, "( + )", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 5, CurY1, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Tax_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY1, 1, 0, pFont)
                Else
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Tax_Type").ToString & " " & Val(prn_HdDt.Rows(0).Item("Tax_Percentage").ToString) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 30, CurY1, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, "( + )", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 5, CurY1, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Tax_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY1, 1, 0, pFont)
                End If
            End If

            'If Val(prn_HdDt.Rows(0).Item("Tax_Amount").ToString) <> 0 Then
            '    CurY1 = CurY1 + TxtHgt
            '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Tax_Type").ToString & " " & Val(prn_HdDt.Rows(0).Item("Tax_Percentage").ToString) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 30, CurY1, 1, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, "( + )", LMargin + C1 + W1 + 10, CurY1, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, "" & Val(prn_HdDt.Rows(0).Item("Tax_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY1, 1, 0, pFont)
            'End If
            If Val(prn_HdDt.Rows(0).Item("Chess_Amount").ToString) <> 0 Then
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Chess_Name").ToString & " " & Val(prn_HdDt.Rows(0).Item("Chess_Percentage").ToString) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 30, CurY1, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "( + )", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 5, CurY1, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Chess_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY1, 1, 0, pFont)
            End If
            If Val(prn_HdDt.Rows(0).Item("ExtraTax_Amount").ToString) <> 0 Then
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("ExtraTax_Name").ToString & " " & Val(prn_HdDt.Rows(0).Item("ExtraTax_Percentage").ToString) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 30, CurY1, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "( + )", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 5, CurY1, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("ExtraTax_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY1, 1, 0, pFont)
            End If

            If Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) <> 0 Then
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Freight_Name").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 30, CurY1, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "( + )", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 5, CurY1, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY1, 1, 0, pFont)
            End If

            If Val(prn_HdDt.Rows(0).Item("Packing_Amount").ToString) <> 0 Then
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Packing_Name").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 30, CurY1, 1, 0, pFont)
                'Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Packing_Name").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "( + )", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 5, CurY1, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Packing_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY1, 1, 0, pFont)
            End If

            If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("AddLess_Name").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 30, CurY1, 1, 0, pFont)
                'Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("AddLess_Name").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "( + )", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 5, CurY1, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY1, 1, 0, pFont)
            End If

            If Val(prn_HdDt.Rows(0).Item("RoundOff_Amount").ToString) <> 0 Then
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "RoundOff ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 30, CurY1, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "( + )", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 5, CurY1, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " " & Format(Val(prn_HdDt.Rows(0).Item("RoundOff_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY1, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, PageWidth, CurY)
            LnAr(8) = CurY
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "E & O.E", LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Total", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("Net_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(9) = CurY
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))
            CurY = CurY + TxtHgt
            BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))
            BmsInWrds = Replace(Trim(LCase(BmsInWrds)), "", "")

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1065" Then '---- Logu textile
                BmsInWrds = Trim(UCase(BmsInWrds))
            End If

            Common_Procedures.Print_To_PrintDocument(e, "Rupees            : " & BmsInWrds & " ", LMargin + 10, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            CurY = CurY + 10
            p1Font = New Font("Calibri", 12, FontStyle.Underline)
            Common_Procedures.Print_To_PrintDocument(e, "Term & Conditions : ", LMargin + 10, CurY, 0, 0, p1Font)

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1118" Then '---- Kasthuri textile 

                CurY = CurY + TxtHgt + 10
                Common_Procedures.Print_To_PrintDocument(e, "Interest at the rate 24% per annum will be charged on overdue bills.", LMargin + 10, CurY, 0, 0, pFont)

                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Our responsibility ceases once the goods leave our premises.", LMargin + 10, CurY, 0, 0, pFont)

                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Form XX (See Rule 26,(13))", LMargin + 10, CurY, 0, 0, pFont)

                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Subject to " & Trim(Common_Procedures.settings.Jurisdiction) & " Jurisdiction Only.", LMargin + 10, CurY, 0, 0, pFont)


                CurY = CurY + TxtHgt + 10
                Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
                p1Font = New Font("Calibri", 12, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)
                CurY = CurY + TxtHgt
                CurY = CurY + TxtHgt
            Else

                CurY = CurY + TxtHgt + 10
                Common_Procedures.Print_To_PrintDocument(e, "1. We are responsible for the quality of yarn only;If any running fault or quality  ", LMargin + 10, CurY, 0, 0, pFont)

                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "   defect noted in yarn please inform with firat fabric roll at once.We will", LMargin + 10, CurY, 0, 0, pFont)
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "   accept only one roll at defect otherwise we do not hold ourself responsible. ", LMargin + 10, CurY, 0, 0, pFont)

                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "2. Our responsibility ceases when goods leave our permises.", LMargin + 10, CurY, 0, 0, pFont)

                CurY = CurY + TxtHgt
                ' If Val(Common_Procedures.User.IdNo) <> 1 Then
                '  Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
                'End If

                ' CurY = CurY + TxtHgt
                Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
                p1Font = New Font("Calibri", 12, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)
                Common_Procedures.Print_To_PrintDocument(e, "3. Subject to Tirupur jurisdiction. ", LMargin + 10, CurY, 0, 0, pFont)

                ' Common_Procedures.Print_To_PrintDocument(e, "For ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 30, CurY, 1, 0, p1Font)
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "4. Interest at the rate of 24% will be charge from the due date.", LMargin + 10, CurY, 0, 0, pFont)

                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "5. All payment should be made by A/C payer cheque or draft.", LMargin + 10, CurY, 0, 0, pFont)

            End If

            CurY = CurY + TxtHgt + 10

            Common_Procedures.Print_To_PrintDocument(e, "AUTHORISED SIGNATORY ", PageWidth - 5, CurY, 1, 0, pFont)
            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub



    Private Sub btn_CloseTerms_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_CloseTerms.Click
        pnl_Back.Enabled = True
        pnl_Terms.Visible = False
        dgv_Details.Focus()
        dgv_Details.CurrentCell.Selected = True
    End Sub

    Private Sub btn_Terms_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Terms.Click
        pnl_Terms.Visible = True
        pnl_Terms.BringToFront()
        pnl_Back.Enabled = False
        txt_Terms_Details1.Focus()
    End Sub
    Private Sub txt_TimeofRemoval_Goods_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_TimeofRemoval_Goods.KeyDown
        If e.KeyValue = 40 Then
            chk_C_FormSales.Focus()
        End If

        If e.KeyValue = 38 Then
            txt_DateAndTime_Invoice.Focus()
        End If
    End Sub
    Private Sub txt_TimeofRemoval_Goods_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_TimeofRemoval_Goods.KeyPress
        If Asc(e.KeyChar) = 13 Then
            chk_C_FormSales.Focus()
        End If
    End Sub
    Private Sub chk_C_FormSales_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles chk_C_FormSales.KeyDown
        If e.KeyValue = 40 Then
            cbo_SalesType.Focus()
        End If

        If e.KeyValue = 38 Then
            txt_TimeofRemoval_Goods.Focus()
        End If
    End Sub
    Private Sub chk_C_FormSales_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles chk_C_FormSales.KeyPress
        If Asc(e.KeyChar) = 13 Then
            cbo_SalesType.Focus()
        End If
    End Sub
    Private Sub cbo_SalesType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_SalesType.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_SalesType, Nothing, Nothing, "", "", "", "")

        If (e.KeyValue = 40 And cbo_SalesType.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If Trim(UCase(cbo_Type.Text)) = "DELIVERY" Then
                If dgv_Details.Rows.Count > 0 Then
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(3)
                Else
                    txt_DiscPerc.Focus()
                End If
            Else
                If dgv_Details.Rows.Count > 0 Then
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
                Else
                    txt_DiscPerc.Focus()
                End If
            End If
        End If
        If e.KeyValue = 38 Then
            chk_C_FormSales.Focus()
        End If
    End Sub

    Private Sub cbo_SalesType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_SalesType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_SalesType, Nothing, "", "", "", "")
        If Asc(e.KeyChar) = 13 Then
            If Trim(UCase(cbo_Type.Text)) = "DELIVERY" Then
                If dgv_Details.Rows.Count > 0 Then
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(3)
                Else
                    txt_DiscPerc.Focus()
                End If
            Else
                If dgv_Details.Rows.Count > 0 Then
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
                Else
                    txt_DiscPerc.Focus()
                End If
            End If
        End If
    End Sub

    Private Sub Yarn_Sales_Proforma_InputLanguageChanging(ByVal sender As Object, ByVal e As System.Windows.Forms.InputLanguageChangingEventArgs) Handles MyBase.InputLanguageChanging

    End Sub

    Private Sub cbo_Verified_Sts_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Verified_Sts.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")

    End Sub
    Private Sub cbo_Verified_Sts_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Verified_Sts.KeyDown

    End Sub

    Private Sub cbo_Verified_Sts_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Verified_Sts.KeyPress

    End Sub

    Private Sub Cbo_SalesAc_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Cbo_SalesAc.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, Cbo_SalesAc, cbo_PartyName, txt_Address1, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 28)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub Cbo_SalesAc_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles Cbo_SalesAc.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, Cbo_SalesAc, txt_Address1, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 28)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub dtp_Lrdate_Enter(sender As Object, e As System.EventArgs) Handles dtp_Lrdate.Enter
        msk_Lr_Date.Focus()
        msk_Lr_Date.SelectionStart = 0
    End Sub

    Private Sub dtp_Lrdate_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles dtp_Lrdate.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
    End Sub

    Private Sub dtp_Lrdate_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles dtp_Lrdate.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            dtp_Lrdate.Text = Date.Today
        End If
    End Sub

    Private Sub dtp_Lrdate_ValueChanged(sender As Object, e As System.EventArgs) Handles dtp_Lrdate.ValueChanged
        msk_Lr_Date.Text = dtp_Lrdate.Text
    End Sub

    Private Sub dtp_LC_Date_Enter(sender As Object, e As System.EventArgs) Handles dtp_LC_Date.Enter
        msk_LC_Date.Focus()
        msk_LC_Date.SelectionStart = 0
    End Sub

    Private Sub dtp_LC_Date_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles dtp_LC_Date.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
    End Sub

    Private Sub dtp_LC_Date_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles dtp_LC_Date.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            dtp_LC_Date.Text = Date.Today
        End If
    End Sub

    Private Sub dtp_LC_Date_ValueChanged(sender As Object, e As System.EventArgs) Handles dtp_LC_Date.ValueChanged
        msk_LC_Date.Text = dtp_LC_Date.Text
    End Sub

    Private Sub dtp_OrderDate_Enter(sender As Object, e As System.EventArgs) Handles dtp_OrderDate.Enter
        msk_OrderDate.Focus()
        msk_OrderDate.SelectionStart = 0
    End Sub

    Private Sub dtp_OrderDate_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles dtp_OrderDate.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
    End Sub

    Private Sub dtp_OrderDate_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles dtp_OrderDate.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            dtp_OrderDate.Text = Date.Today
        End If
    End Sub

    Private Sub dtp_OrderDate_ValueChanged(sender As Object, e As System.EventArgs) Handles dtp_OrderDate.ValueChanged
        msk_OrderDate.Text = dtp_OrderDate.Text
    End Sub

    Private Sub dgtxt_Details_TextChanged(sender As Object, e As System.EventArgs) Handles dgtxt_Details.TextChanged
        Try
            If Not IsNothing(dgv_Details.CurrentCell) Then

                With dgv_Details
                    If .Rows.Count <> 0 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(dgtxt_Details.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub btn_UserModification_Click(sender As System.Object, e As System.EventArgs) Handles btn_UserModification.Click
        If Val(Common_Procedures.User.IdNo) = 1 Then
            Dim f1 As New User_Modifications
            f1.Entry_Name = Me.Name
            f1.Entry_PkValue = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            f1.ShowDialog()
        End If
    End Sub

    Private Sub dgv_Filter_Details_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles dgv_Filter_Details.CellValueChanged

    End Sub
End Class