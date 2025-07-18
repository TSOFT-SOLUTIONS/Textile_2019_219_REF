Public Class Processed_Item_Purchase
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "PIPUR-"
    Private Pk_Condition2 As String = "PIPAC-"
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

    Private Sub clear()
        New_Entry = False
        Insert_Entry = False

        pnl_Back.Enabled = True
        pnl_Filter.Visible = False

        lbl_RefNo.Text = ""
        lbl_RefNo.ForeColor = Color.Black
        dtp_Date.Text = ""
        txt_RecNO.Text = ""
        cbo_Ledger.Text = ""
        cbo_PurchaseAccount.Text = ""
        cbo_TransportName.Text = ""
        cbo_VatAc.Text = ""
        txt_Freight.Text = ""
        txt_Note.Text = ""
        txt_CommPerc.Text = ""
        cbo_CommType.Text = "%"
        lbl_CommAmount.Text = ""
        txt_BillNo.Text = ""
        txt_VehicleNo.Text = ""
        cbo_Agent.Text = ""
        cbo_DelvAt.Text = Common_Procedures.Ledger_IdNoToName(con, Common_Procedures.CommonLedger.Godown_Ac)
        lbl_UserName.Text = "USER : " & Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)
        txt_GrossAmount.Text = ""
        txt_GrossAmount.Text = ""
        txt_DiscPerc.Text = ""
        lbl_DiscAmount.Text = ""
        lbl_AssessableValue.Text = ""
        cbo_VatAc.Text = ""
        cbo_TaxType.Text = "-NIL-"
        txt_TaxPerc.Text = ""
        lbl_TaxAmount.Text = ""
        txt_Freight.Text = ""
        txt_AddLess_AfterTax.Text = ""
        txt_AddLess_BeforeTax.Text = ""
        lbl_RoundOff.Text = ""
        lbl_NetAmount.Text = "0.00"

        dgv_Details.Rows.Clear()
        dgv_Details_Total.Rows.Clear()
        dgv_Details_Total.Rows.Add()

        Grid_Cell_DeSelect()

        cbo_ItemName.Visible = False
        cbo_Colour.Visible = False
        cbo_Unit.Visible = False

        cbo_ItemName.Tag = -1
        cbo_Unit.Tag = -1
        cbo_Colour.Tag = -1

        cbo_ItemName.Text = ""
        cbo_Colour.Text = ""
        cbo_Unit.Text = ""

    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox
        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Me.ActiveControl Is Button Then
            Me.ActiveControl.BackColor = Color.Lime
            Me.ActiveControl.ForeColor = Color.Blue
        End If

        If TypeOf Me.ActiveControl Is TextBox Then
            txtbx = Me.ActiveControl
            txtbx.SelectAll()
        ElseIf TypeOf Me.ActiveControl Is ComboBox Then
            combobx = Me.ActiveControl
            combobx.SelectAll()
        End If

        If Me.ActiveControl.Name <> cbo_Colour.Name Then
            cbo_Colour.Visible = False
        End If

        If Me.ActiveControl.Name <> cbo_Unit.Name Then
            cbo_Unit.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_ItemName.Name Then
            cbo_ItemName.Visible = False
        End If
        If Me.ActiveControl.Name <> dgv_Details_Total.Name Then
            Grid_Cell_DeSelect()
        End If

        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)

        On Error Resume Next

        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Then
                Prec_ActCtrl.BackColor = Color.White
                Prec_ActCtrl.ForeColor = Color.Black
            ElseIf TypeOf Prec_ActCtrl Is Button Then
                Prec_ActCtrl.BackColor = Color.FromArgb(5, 60, 110)
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

    Private Sub Grid_Cell_DeSelect()
        On Error Resume Next
        dgv_Details.CurrentCell.Selected = False
        dgv_Details_Total.CurrentCell.Selected = False
        dgv_Filter_Details.CurrentCell.Selected = False
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

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name ,c.Ledger_Name as Transport_Name,d.Ledger_Name as Agent_Name , e.Ledger_Name as VatAC_Name,f.Ledger_Name as PurAc_Name ,g.Ledger_Name as Delv_Name  from Item_Purchase_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Ledger_Head c ON a.Transport_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Ledger_Head d ON a.Agent_IdNo = d.Ledger_IdNo LEFT OUTER JOIN Ledger_Head e ON a.TaxAc_IdNo = e.Ledger_IdNo LEFT OUTER JOIN Ledger_Head f ON a.PurchaseAc_IdNo = f.Ledger_IdNo LEFT OUTER JOIN Ledger_Head g ON a.DeliveryTo_Idno = g.Ledger_IdNo Where a.Item_Purchase_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_RefNo.Text = dt1.Rows(0).Item("Item_Purchase_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Item_Purchase_Date").ToString
                txt_BillNo.Text = dt1.Rows(0).Item("Party_Bill_No").ToString
                cbo_Ledger.Text = dt1.Rows(0).Item("Ledger_Name").ToString
                cbo_PurchaseAccount.Text = dt1.Rows(0).Item("PurAc_Name").ToString
                cbo_DelvAt.Text = dt1.Rows(0).Item("Delv_Name").ToString
                txt_RecNO.Text = dt1.Rows(0).Item("Delivery_Receipt_No").ToString
                cbo_Agent.Text = dt1.Rows(0).Item("Agent_Name").ToString
                cbo_CommType.Text = dt1.Rows(0).Item("Agent_Commission_Type").ToString
                txt_CommPerc.Text = Val(dt1.Rows(0).Item("Agent_Commission_Rate").ToString)
                If Val(txt_CommPerc.Text) = 0 Then txt_CommPerc.Text = ""
                lbl_CommAmount.Text = Format(Val(dt1.Rows(0).Item("Agent_Commission_Amount").ToString), "#########0.00")
                If Val(lbl_CommAmount.Text) = 0 Then lbl_CommAmount.Text = ""
                cbo_TransportName.Text = dt1.Rows(0).Item("Transport_Name").ToString
                txt_VehicleNo.Text = dt1.Rows(0).Item("Vehicle_No").ToString
                lbl_UserName.Text = "USER : " & Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))



                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Processed_Item_Name, c.Colour_Name from Item_Purchase_Details a INNER JOIN Processed_Item_Head b ON a.Processed_Item_IdNo = b.Processed_Item_Idno LEFT OUTER JOIN Colour_Head c ON a.Colour_IdNo = c.Colour_IdNo Where a.Item_Purchase_Code = '" & Trim(NewCode) & "' Order by a.Sl_No", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_Details.Rows.Clear()
                SNo = 0

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_Details.Rows.Add()

                        SNo = SNo + 1
                        dgv_Details.Rows(n).Cells(0).Value = Val(SNo)
                        dgv_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Processed_Item_Name").ToString
                        dgv_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Party_Item_Name").ToString
                        dgv_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Colour_Name").ToString
                        dgv_Details.Rows(n).Cells(4).Value = dt2.Rows(i).Item("Purchase_Pcs").ToString
                        If Val(dgv_Details.Rows(n).Cells(4).Value) = 0 Then
                            dgv_Details.Rows(n).Cells(4).Value = ""
                        End If
                        dgv_Details.Rows(n).Cells(5).Value = dt2.Rows(i).Item("Purchase_Qty").ToString
                        If Val(dgv_Details.Rows(n).Cells(5).Value) = 0 Then
                            dgv_Details.Rows(n).Cells(5).Value = ""
                        End If
                        dgv_Details.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Meter_Qty").ToString), "########0.00")
                        If Val(dgv_Details.Rows(n).Cells(6).Value) = 0 Then
                            dgv_Details.Rows(n).Cells(6).Value = ""
                        End If
                        dgv_Details.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Meter").ToString), "########0.00")
                        If Val(dgv_Details.Rows(n).Cells(7).Value) = 0 Then
                            dgv_Details.Rows(n).Cells(7).Value = ""
                        End If
                        dgv_Details.Rows(n).Cells(8).Value = dt2.Rows(i).Item("Unit_Name").ToString
                        dgv_Details.Rows(n).Cells(9).Value = Format(Val(dt2.Rows(i).Item("Rate").ToString), "########0.00")
                        If Val(dgv_Details.Rows(n).Cells(9).Value) = 0 Then
                            dgv_Details.Rows(n).Cells(9).Value = ""
                        End If
                        dgv_Details.Rows(n).Cells(10).Value = Format(Val(dt2.Rows(i).Item("Amount").ToString), "########0.00")
                        If Val(dgv_Details.Rows(n).Cells(10).Value) = 0 Then
                            dgv_Details.Rows(n).Cells(10).Value = ""
                        End If

                    Next i

                End If

                If dgv_Details.RowCount = 0 Then dgv_Details.Rows.Add()

                With dgv_Details_Total
                    If .RowCount = 0 Then .Rows.Add()
                    .Rows(0).Cells(4).Value = Val(dt1.Rows(0).Item("Total_Pcs").ToString)
                    .Rows(0).Cells(5).Value = Val(dt1.Rows(0).Item("Total_Qty").ToString)
                    .Rows(0).Cells(7).Value = Format(Val(dt1.Rows(0).Item("Total_Meter").ToString), "########0.00")
                    .Rows(0).Cells(10).Value = Format(Val(dt1.Rows(0).Item("Gross_Amount").ToString), "########0.00")
                End With
                dt2.Clear()

                txt_GrossAmount.Text = dt1.Rows(0).Item("Gross_Amount").ToString
                If Val(txt_GrossAmount.Text) = 0 Then txt_GrossAmount.Text = ""

                txt_DiscPerc.Text = dt1.Rows(0).Item("Discount_Percentage").ToString
                If Val(txt_DiscPerc.Text) = 0 Then txt_DiscPerc.Text = ""

                lbl_DiscAmount.Text = dt1.Rows(0).Item("Discount_Amount").ToString
                If Val(lbl_DiscAmount.Text) = 0 Then lbl_DiscAmount.Text = ""

                txt_AddLess_BeforeTax.Text = dt1.Rows(0).Item("AddLess_BeforeTax_Amount").ToString
                If Val(txt_AddLess_BeforeTax.Text) = 0 Then txt_AddLess_BeforeTax.Text = ""

                lbl_AssessableValue.Text = dt1.Rows(0).Item("Assessable_Value").ToString
                If Val(lbl_AssessableValue.Text) = 0 Then lbl_AssessableValue.Text = ""

                cbo_VatAc.Text = dt1.Rows(0).Item("VatAC_Name").ToString
                cbo_TaxType.Text = dt1.Rows(0).Item("Tax_Type").ToString

                txt_TaxPerc.Text = Format(Val(dt1.Rows(0).Item("Tax_Percentage").ToString), "########0.00")
                If Val(txt_TaxPerc.Text) = 0 Then txt_TaxPerc.Text = ""

                lbl_TaxAmount.Text = Format(Val(dt1.Rows(0).Item("Tax_Amount").ToString), "########0.00")
                If Val(lbl_TaxAmount.Text) = 0 Then lbl_TaxAmount.Text = ""

                txt_Freight.Text = Format(Val(dt1.Rows(0).Item("Freight_Charge").ToString), "########0.00")
                If Val(txt_Freight.Text) = 0 Then txt_Freight.Text = ""

                txt_AddLess_AfterTax.Text = Format(Val(dt1.Rows(0).Item("AddLess_Amount").ToString), "########0.00")
                If Val(txt_AddLess_AfterTax.Text) = 0 Then txt_AddLess_AfterTax.Text = ""

                lbl_RoundOff.Text = Format(Val(dt1.Rows(0).Item("RoundOff_Amount").ToString), "########0.00")
                If Val(lbl_RoundOff.Text) = 0 Then lbl_RoundOff.Text = ""

                lbl_NetAmount.Text = Format(Val(dt1.Rows(0).Item("Net_Amount").ToString), "########0.00")
                If Val(lbl_NetAmount.Text) = 0 Then lbl_NetAmount.Text = ""

                txt_Note.Text = dt1.Rows(0).Item("Note").ToString

            End If

            dt1.Clear()

            Grid_Cell_DeSelect()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            dt1.Dispose()
            da1.Dispose()

            dt2.Dispose()
            da2.Dispose()

            If dtp_Date.Visible And dtp_Date.Enabled Then dtp_Date.Focus()

        End Try



    End Sub

    Private Sub Processed_Item_Purchase_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Ledger.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Ledger.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Agent.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "AGENT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Agent.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_TransportName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_TransportName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_ItemName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "GREYITEM" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_ItemName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Colour.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "COLOUR" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Colour.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Unit.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "UNIT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Unit.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_VatAc.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_VatAc.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_DelvAt.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_DelvAt.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

    Private Sub Processed_Item_Purchase_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        FrmLdSTS = True

        Me.Text = ""

        con.Open()

        cbo_ItemName.Visible = False
        cbo_Colour.Visible = False
        cbo_Unit.Visible = False

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2

        cbo_TaxType.Items.Clear()
        cbo_TaxType.Items.Add("-NIL-")
        cbo_TaxType.Items.Add("VAT")
        cbo_TaxType.Items.Add("CST")

        cbo_CommType.Items.Clear()
        cbo_CommType.Items.Add(" ")
        cbo_CommType.Items.Add("%")
        cbo_CommType.Items.Add("MTR")
        cbo_CommType.Items.Add("QTY")

        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Colour.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ItemName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Ledger.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Agent.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_DelvAt.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PurchaseAccount.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_TransportName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Unit.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_TaxType.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_VatAc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DiscPerc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_TaxPerc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Freight.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_AddLess_AfterTax.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_AddLess_BeforeTax.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_GrossAmount.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_filter_billNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_BillNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_CommPerc.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_CommType.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Note.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_RecNO.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_VehicleNo.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_DiscPerc.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_TaxType.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_TaxPerc.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Freight.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_AddLess_AfterTax.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_AddLess_BeforeTax.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_VatAc.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_GrossAmount.LostFocus, AddressOf ControlLostFocus


        AddHandler cbo_Filter_DelvAt.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus

        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Colour.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ItemName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Ledger.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Agent.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_DelvAt.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_PurchaseAccount.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_TransportName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Unit.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_filter_billNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_BillNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_CommPerc.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_CommType.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Note.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_RecNO.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_VehicleNo.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_Filter_DelvAt.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus

        AddHandler dtp_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_CommPerc.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Freight.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_RecNO.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_filter_billNo.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_TaxPerc.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_AddLess_AfterTax.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_AddLess_BeforeTax.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_BillNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_DiscPerc.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_CommPerc.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Freight.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_RecNO.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_filter_billNo.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler dtp_Filter_Fromdate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_ToDate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_BillNo.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler txt_DiscPerc.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_TaxPerc.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_AddLess_AfterTax.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_AddLess_BeforeTax.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_GrossAmount.KeyPress, AddressOf TextBoxControlKeyPress

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True
        new_record()

    End Sub

    Private Sub Processed_Item_Purchase_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub Processed_Item_Purchase_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

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
                                txt_GrossAmount.Focus()

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
                                txt_BillNo.Focus()

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 1)

                            End If

                        ElseIf .CurrentCell.ColumnIndex = 4 Then
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(1)


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

        '  If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.FP_Purchase_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.FP_Purchase_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.FP_purchase_Entry, New_Entry, Me, con, "Item_Purchase_Head", "Item_Purchase_Code", NewCode, "Item_Purchase_Date", "(Item_Purchase_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub







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

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.Transaction = trans

            If Common_Procedures.VoucherBill_Deletion(con, Trim(Pk_Condition) & Trim(NewCode), trans) = False Then
                Throw New ApplicationException("Error on Voucher Bill Deletion")
            End If

            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), trans)
            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition2) & Trim(NewCode), trans)

            cmd.CommandText = "delete from AgentCommission_Processing_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Item_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Item_Purchase_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Item_Purchase_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Item_Purchase_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Item_Purchase_Code = '" & Trim(NewCode) & "'"
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

        If dtp_Date.Enabled = True And dtp_Date.Visible = True Then dtp_Date.Focus()

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

            da = New SqlClient.SqlDataAdapter("select top 1 Item_Purchase_No from Item_Purchase_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Item_Purchase_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Item_Purchase_No", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 Item_Purchase_No from Item_Purchase_Head where for_orderby > " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Item_Purchase_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Item_Purchase_No", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 Item_Purchase_No from Item_Purchase_Head where for_orderby < " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Item_Purchase_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Item_Purchase_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 Item_Purchase_No from Item_Purchase_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Item_Purchase_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Item_Purchase_No desc", con)
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
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewID As Integer = 0

        Try

            clear()

            New_Entry = True

            lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Item_Purchase_Head", "Item_Purchase_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

            lbl_RefNo.ForeColor = Color.Red

            da1 = New SqlClient.SqlDataAdapter("select Top 1 a.*, e.Ledger_Name as VatAC_Name, f.Ledger_Name as PurAc_Name, g.Ledger_Name as Delv_Name from Item_Purchase_Head a LEFT OUTER JOIN Ledger_Head e ON a.TaxAc_IdNo = e.Ledger_IdNo LEFT OUTER JOIN Ledger_Head f ON a.PurchaseAc_IdNo = f.Ledger_IdNo LEFT OUTER JOIN Ledger_Head g ON a.DeliveryTo_Idno = g.Ledger_IdNo where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Item_Purchase_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by a.for_Orderby desc, a.Item_Purchase_No desc", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then
                If dt1.Rows(0).Item("PurAc_Name").ToString <> "" Then cbo_PurchaseAccount.Text = dt1.Rows(0).Item("PurAc_Name").ToString
                If dt1.Rows(0).Item("Delv_Name").ToString <> "" Then cbo_DelvAt.Text = dt1.Rows(0).Item("Delv_Name").ToString
                If dt1.Rows(0).Item("Tax_Type").ToString <> "" Then cbo_TaxType.Text = dt1.Rows(0).Item("Tax_Type").ToString
                If dt1.Rows(0).Item("Tax_Percentage").ToString <> "" Then txt_TaxPerc.Text = Val(dt1.Rows(0).Item("Tax_Percentage").ToString)
                If dt1.Rows(0).Item("VatAC_Name").ToString <> "" Then cbo_VatAc.Text = dt1.Rows(0).Item("VatAC_Name").ToString
                If dt1.Rows(0).Item("Agent_Commission_Type").ToString <> "" Then cbo_CommType.Text = dt1.Rows(0).Item("Agent_Commission_Type").ToString
            End If

            dt1.Clear()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            dt1.Dispose()
            da1.Dispose()

            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

        End Try

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim RecCode As String

        Try

            inpno = InputBox("Enter Dc.No.", "FOR FINDING...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Item_Purchase_No from Item_Purchase_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Item_Purchase_Code = '" & Trim(RecCode) & "'", con)
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
                MessageBox.Show("Dc No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.FP_Purchase_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.FP_Purchase_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.FP_purchase_Entry, New_Entry, Me) = False Then Exit Sub


        Try

            inpno = InputBox("Enter New Ref No.", "FOR NEW RECEIPT INSERTION...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Item_Purchase_No from Item_Purchase_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Item_Purchase_Code = '" & Trim(RecCode) & "'", con)
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
                    MessageBox.Show("Invalid Ref No", "DOES NOT INSERT NEW RECEIPT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim Led_ID As Integer = 0
        Dim Col_ID As Integer = 0
        Dim Itfp_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim Partcls As String = ""
        Dim EntID As String = ""
        Dim TxAc_ID As Integer = 0
        Dim PBlNo As String = ""
        Dim vTotPcs As Single, vTotMtrs As Single, vtotqty As Single
        Dim Proc_ID As Integer = 0
        Dim Lot_ID As Integer = 0
        Dim vTotWeight As Single
        Dim Tr_ID As Integer = 0, Ag_Id As Integer = 0, DelT_Id As Integer = 0
        Dim itgry_id As Integer = 0, PurAc_id As Integer = 0
        Dim vStkQty As Single = 0
        Dim ItmTyp As String = ""
        Dim VouBil As String = ""
        Dim YrnClthNm As String = ""
        Dim ProdType As String = ""
        Dim Usr_IDNo As Integer = 0


        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        '  If Common_Procedures.UserRight_Check(Common_Procedures.UR.FP_Purchase_Entry, New_Entry) = False Then Exit Sub
       


        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.FP_purchase_Entry, New_Entry, Me, con, "Item_Purchase_Head", "Item_Purchase_Code", NewCode, "Item_Purchase_Date", "(Item_Purchase_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Item_Purchase_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, Item_Purchase_No desc", dtp_Date.Value.Date) = False Then Exit Sub





      
        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(dtp_Date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
            Exit Sub
        End If

        If Not (dtp_Date.Value.Date >= Common_Procedures.Company_FromDate And dtp_Date.Value.Date <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
            Exit Sub
        End If

        Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
        If Led_ID = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
            Exit Sub
        End If

        If Trim(txt_BillNo.Text) = "" Then
            MessageBox.Show("Invalid Bill No", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If txt_BillNo.Enabled And txt_BillNo.Visible Then txt_BillNo.Focus()
            Exit Sub
        End If

        Tr_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_TransportName.Text)
        Ag_Id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Agent.Text)
        PurAc_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PurchaseAccount.Text)
        DelT_Id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_DelvAt.Text)
        If DelT_Id = 0 Then DelT_Id = 4
        TxAc_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_VatAc.Text)
        usr_idno = Common_Procedures.User_NameToIdNo(con1, lbl_UserName.Text)

        If PurAc_id = 0 And Val(CSng(lbl_NetAmount.Text)) <> 0 Then
            MessageBox.Show("Invalid Purchase A/c name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_PurchaseAccount.Enabled And cbo_PurchaseAccount.Visible Then cbo_PurchaseAccount.Focus()
            Exit Sub
        End If

        If Ag_Id = 0 And Val(lbl_CommAmount.Text) <> 0 Then
            MessageBox.Show("Invalid Agent name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Agent.Enabled And cbo_Agent.Visible Then cbo_Agent.Focus()
            Exit Sub
        End If

        With dgv_Details
            For i = 0 To dgv_Details.RowCount - 1
                If Val(.Rows(i).Cells(5).Value) <> 0 Or Val(.Rows(i).Cells(7).Value) <> 0 Then

                    If Trim(dgv_Details.Rows(i).Cells(1).Value) = "" Then
                        MessageBox.Show("Invalid Item Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dgv_Details.Enabled And dgv_Details.Visible Then
                            dgv_Details.Focus()
                            dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(1)

                        End If
                        Exit Sub
                    End If

                    ItmTyp = Common_Procedures.get_FieldValue(con, "Processed_Item_Head", "Processed_Item_Type", "( Processed_Item_Name = '" & Trim(dgv_Details.Rows(i).Cells(1).Value) & "' )")

                    If Trim(UCase(ItmTyp)) = "FP" Then

                        If Val(dgv_Details.Rows(i).Cells(5).Value) = 0 Then
                            MessageBox.Show("Invalid Quantity", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            If dgv_Details.Enabled And dgv_Details.Focus() Then
                                dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(7)
                                dgv_Details.Focus()
                            End If
                            Exit Sub
                        End If

                    Else

                        If Val(dgv_Details.Rows(i).Cells(7).Value) = 0 Then
                            MessageBox.Show("Invalid Meters", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            If dgv_Details.Enabled And dgv_Details.Focus() Then
                                dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(7)
                                dgv_Details.Focus()
                            End If
                            Exit Sub
                        End If

                    End If

                End If

            Next
        End With

        If TxAc_ID = 0 And Val(lbl_TaxAmount.Text) <> 0 Then
            MessageBox.Show("Invalid Tax A/c", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_VatAc.Enabled Then cbo_VatAc.Focus()
            Exit Sub
        End If

        If Val(lbl_TaxAmount.Text) <> 0 And (Trim(cbo_TaxType.Text) = "" Or Trim(cbo_TaxType.Text) = "-NIL-") Then
            MessageBox.Show("Invalid Tax Type", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_TaxType.Enabled And cbo_TaxType.Visible Then cbo_TaxType.Focus()
            Exit Sub
        End If


        Calculation_Grid_Total()

        vTotMtrs = 0 : vTotWeight = 0 : vTotPcs = 0 : vtotqty = 0

        If dgv_Details_Total.RowCount > 0 Then
            vTotPcs = Val(dgv_Details_Total.Rows(0).Cells(4).Value())
            vtotqty = Val(dgv_Details_Total.Rows(0).Cells(5).Value())
            vTotMtrs = Val(dgv_Details_Total.Rows(0).Cells(7).Value())
            vTotWeight = Val(dgv_Details_Total.Rows(0).Cells(10).Value())
        End If

        tr = con.BeginTransaction

        Try


            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Item_Purchase_Head", "Item_Purchase_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@PurchaseDate", dtp_Date.Value.Date)

            If New_Entry = True Then

                cmd.CommandText = "Insert into Item_Purchase_Head(Item_Purchase_Code     , Company_IdNo                     , Item_Purchase_No              ,                               for_OrderBy                              , Item_Purchase_Date,         Ledger_IdNo     ,  PurchaseAc_Idno      , DeliveryTo_Idno     ,       Delivery_Receipt_No     ,      Agent_Idno   ,          Agent_Commission_Type   ,         Agent_Commission_Rate ,         Agent_Commission_Amount      ,            Party_Bill_No       ,     Transport_IdNo,               Vehicle_No          ,               Note           ,            Total_Pcs     ,          Total_Qty       ,           Total_Meter     ,                  Gross_Amount         ,             Discount_Percentage    ,              Discount_Amount         ,              AddLess_BeforeTax_Amount       ,          Assessable_Value            , TaxAc_IdNo      ,           Tax_Type              ,             Tax_Percentage        ,               Tax_Amount            ,                 Freight_Charge    ,                 AddLess_Amount             ,             RoundOff_Amount        ,                     Net_Amount                      ,  User_idno)  " & _
                                                        " Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ", @PurchaseDate     , " & Str(Val(Led_ID)) & ", " & Val(PurAc_id) & " , " & Val(DelT_Id) & ", '" & Trim(txt_RecNO.Text) & "', " & Val(Ag_Id) & ", '" & Trim(cbo_CommType.Text) & "', " & Val(txt_CommPerc.Text) & ", " & Str(Val(lbl_CommAmount.Text)) & ", '" & Trim(txt_BillNo.Text) & "', " & Val(Tr_ID) & ", '" & Trim(txt_VehicleNo.Text) & "', '" & Trim(txt_Note.Text) & "', " & Str(Val(vTotPcs)) & ", " & Str(Val(vtotqty)) & ", " & Str(Val(vTotMtrs)) & ", " & Str(Val(txt_GrossAmount.Text)) & ", " & Str(Val(txt_DiscPerc.Text)) & ", " & Str(Val(lbl_DiscAmount.Text)) & ", " & Str(Val(txt_AddLess_BeforeTax.Text)) & ", " & Val(lbl_AssessableValue.Text) & ", " & Str(Val(TxAc_ID)) & ", '" & Trim(cbo_TaxType.Text) & "', " & Str(Val(txt_TaxPerc.Text)) & ", " & Str(Val(lbl_TaxAmount.Text)) & ", " & Str(Val(txt_Freight.Text)) & ", " & Str(Val(txt_AddLess_AfterTax.Text)) & ", " & Str(Val(lbl_RoundOff.Text)) & ", " & Str(Val(CSng(lbl_NetAmount.Text))) & ", " & Val(Usr_IDNo) & " )  "
                cmd.ExecuteNonQuery()

            Else
                cmd.CommandText = "Update Item_Purchase_Head set Item_Purchase_Date = @PurchaseDate, Ledger_IdNo = " & Val(Led_ID) & ", PurchaseAc_Idno = " & Val(PurAc_id) & " ,DeliveryTo_Idno = " & Val(DelT_Id) & ",Delivery_Receipt_No = '" & Trim(txt_RecNO.Text) & "', Agent_Idno = " & Val(Ag_Id) & ", Agent_Commission_Type = '" & Trim(cbo_CommType.Text) & "', Agent_Commission_Rate = " & Val(txt_CommPerc.Text) & ", Agent_Commission_Amount = " & Val(lbl_CommAmount.Text) & ", Vehicle_No = '" & Trim(txt_VehicleNo.Text) & "',Party_Bill_No = '" & Trim(txt_BillNo.Text) & "' , Discount_Percentage = " & Str(Val(txt_DiscPerc.Text)) & ", Discount_Amount = " & Str(Val(lbl_DiscAmount.Text)) & ", AddLess_BeforeTax_Amount = " & Str(Val(txt_AddLess_BeforeTax.Text)) & ", Assessable_Value = " & Val(lbl_AssessableValue.Text) & ", Transport_IdNo = " & Val(Tr_ID) & ", Freight_Charge = " & Val(txt_Freight.Text) & ", Note = '" & Trim(txt_Note.Text) & "', Total_Pcs = " & Val(vTotPcs) & ",Total_Qty = " & Val(vtotqty) & ", Total_Meter = " & Val(vTotMtrs) & ", Gross_Amount = " & Str(Val(txt_GrossAmount.Text)) & ", Tax_Type = '" & Trim(cbo_TaxType.Text) & "', Tax_Percentage = " & Str(Val(txt_TaxPerc.Text)) & ", Tax_Amount = " & Str(Val(lbl_TaxAmount.Text)) & ", TaxAc_IdNo = " & Str(Val(TxAc_ID)) & ", AddLess_Amount = " & Str(Val(txt_AddLess_AfterTax.Text)) & ", RoundOff_Amount = " & Str(Val(lbl_RoundOff.Text)) & ", Net_Amount = " & Str(Val(CSng(lbl_NetAmount.Text))) & ", User_idNo = " & Val(Usr_IDNo) & "  Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Item_Purchase_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If


            cmd.CommandText = "Delete from Item_Purchase_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Item_Purchase_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Item_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            Partcls = "Purc : Ref.No. " & Trim(lbl_RefNo.Text)
            PBlNo = Trim(lbl_RefNo.Text)
            EntID = Trim(Pk_Condition) & Trim(lbl_RefNo.Text)

            With dgv_Details
                Sno = 0
                YrnClthNm = ""
                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(5).Value) <> 0 Or Val(.Rows(i).Cells(7).Value) <> 0 Then

                        If Trim(YrnClthNm) = "" Then YrnClthNm = Trim(.Rows(i).Cells(1).Value)

                        Sno = Sno + 1
                        itgry_id = Common_Procedures.Processed_Item_NameToIdNo(con, .Rows(i).Cells(1).Value, tr)
                        Col_ID = Common_Procedures.Colour_NameToIdNo(con, .Rows(i).Cells(3).Value, tr)

                        ItmTyp = Common_Procedures.get_FieldValue(con, "Processed_Item_Head", "Processed_Item_Type", "( Processed_Item_IdNo = " & Str(Val(itgry_id)) & " )", , tr)

                        Sno = Sno + 1

                        cmd.CommandText = "Insert into Item_Purchase_Details(Item_Purchase_Code, Company_IdNo, Item_Purchase_No, for_OrderBy, Item_Purchase_Date,Sl_No,Processed_Item_Idno,Party_Item_Name,Colour_Idno,Purchase_Pcs,Purchase_Qty,Meter_Qty,Meter,Unit_Name,Rate,Amount ) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ", @PurchaseDate," & Str(Val(Sno)) & ", " & Str(Val(itgry_id)) & ", '" & Trim(.Rows(i).Cells(2).Value) & "', " & Val(Col_ID) & ", " & Val(.Rows(i).Cells(4).Value) & ",  " & Val(.Rows(i).Cells(5).Value) & "," & Val(.Rows(i).Cells(6).Value) & ", " & Val(.Rows(i).Cells(7).Value) & ", '" & Trim(.Rows(i).Cells(8).Value) & "', " & Str(Val(.Rows(i).Cells(9).Value)) & " ," & Str(Val(.Rows(i).Cells(10).Value)) & ")"
                        cmd.ExecuteNonQuery()

                        If Trim(UCase(ItmTyp)) = "FP" Then
                            vStkQty = Val(.Rows(i).Cells(5).Value)
                        Else
                            vStkQty = Val(.Rows(i).Cells(4).Value)
                        End If

                        cmd.CommandText = "Insert into Stock_Item_Processing_Details ( Reference_Code ,            Company_IdNo          ,            Reference_No    ,            For_OrderBy                                                 ,  Reference_Date     ,                   DeliveryTo_StockIdNo                     ,  ReceivedFrom_StockIdNo, Delivery_PartyIdNo, Received_PartyIdNo      ,  Entry_ID            , Party_Bill_No          , Particulars         ,            SL_No     ,             Item_IdNo       , Rack_IdNo            ,                       Quantity     ,                       Meters                        ) " & _
                                         " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "'      , " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",    @PurchaseDate  , " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & " ,     0                  ,          0        , " & Str(Val(Led_ID)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', " & Str(Val(Sno)) & ", " & Str(Val(itgry_id)) & "  , 0                    , " & Str(Math.Abs(Val(vStkQty))) & ", " & Str(Math.Abs(Val(.Rows(i).Cells(7).Value))) & " ) "
                        cmd.ExecuteNonQuery()

                    End If

                Next

            End With


            'AgentCommission Posting
            cmd.CommandText = "delete from AgentCommission_Processing_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            If Val(Ag_Id) <> 0 Then

                ItmTyp = Common_Procedures.get_FieldValue(con, "Processed_Item_Head", "Processed_Item_Type", "( Processed_Item_Name = '" & Trim(YrnClthNm) & "' )", , tr)

                If Trim(UCase(ItmTyp)) = "FP" Then
                    vStkQty = Val(vtotqty)
                    ProdType = "FP"

                Else
                    vStkQty = Val(vTotMtrs)
                    ProdType = "FABRIC"

                End If

                cmd.CommandText = "Insert into AgentCommission_Processing_Details (  Reference_Code   ,             Company_IdNo         ,            Reference_No       ,                               For_OrderBy                              , Reference_Date ,        Commission_For   ,     Ledger_IdNo    ,      Agent_IdNo   ,         Entry_ID     ,      Party_BillNo    ,       Particulars      ,      Yarn_Cloth_Name     ,         Bags_Meters      ,               Amount                ,              Commission_Type     ,       Commission_Rate              ,            Commission_Amount         ) " & _
                                                " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",   @PurchaseDate, '" & Trim(ProdType) & "', " & Str(Led_ID) & ", " & Str(Ag_Id) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', '" & Trim(YrnClthNm) & "', " & Str(Val(vStkQty)) & ", " & Str(Val(lbl_NetAmount.Text)) & ", '" & Trim(cbo_CommType.Text) & "', " & Str(Val(txt_CommPerc.Text)) & ", " & Str(Val(lbl_CommAmount.Text)) & ")"
                cmd.ExecuteNonQuery()

            End If



            Dim vLed_IdNos As String = "", vVou_Amts As String = "", ErrMsg As String = ""

            vLed_IdNos = Led_ID & "|" & PurAc_id & "|" & TxAc_ID
            vVou_Amts = Val(CSng(lbl_NetAmount.Text)) & "|" & -1 * (Val(CSng(lbl_NetAmount.Text)) - Val(lbl_TaxAmount.Text)) & "|" & -1 * Val(lbl_TaxAmount.Text)
            If Common_Procedures.Voucher_Updation(con, "Item.Purc", Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), Trim(lbl_RefNo.Text), dtp_Date.Value.Date, "Bill No : " & Trim(txt_BillNo.Text), vLed_IdNos, vVou_Amts, ErrMsg, tr) = False Then
                Throw New ApplicationException(ErrMsg)
            End If

            vLed_IdNos = Ag_Id & "|" & Val(Common_Procedures.CommonLedger.Agent_Commission_Ac)
            vVou_Amts = Val(lbl_CommAmount.Text) & "|" & -1 * Val(lbl_CommAmount.Text)
            If Common_Procedures.Voucher_Updation(con, "ItmPur.Comm", Val(lbl_Company.Tag), Trim(Pk_Condition2) & Trim(NewCode), Trim(lbl_RefNo.Text), dtp_Date.Value.Date, "Bill No : " & Trim(txt_BillNo.Text), vLed_IdNos, vVou_Amts, ErrMsg, tr) = False Then
                Throw New ApplicationException(ErrMsg)
            End If

            'Bill Posting
            VouBil = Common_Procedures.VoucherBill_Posting(con, Val(lbl_Company.Tag), dtp_Date.Text, Led_ID, Trim(txt_BillNo.Text), Ag_Id, Val(CSng(lbl_NetAmount.Text)), "CR", Trim(Pk_Condition) & Trim(NewCode), tr)
            If Trim(UCase(VouBil)) = "ERROR" Then
                Throw New ApplicationException("Error on Voucher Bill Posting")
            End If

            tr.Commit()

            Dt1.Dispose()
            Da.Dispose()
            If New_Entry = True Then new_record()



            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

    End Sub

    Private Sub Calculation_Grid_Total()
        Dim vTotPcs As Single, vTotMtrs As Single, vtotamt As Single, vtotqty As Single
        Dim i As Integer
        Dim sno As Integer

        If FrmLdSTS = True Then Exit Sub

        vTotPcs = 0 : vtotqty = 0 : vTotMtrs = 0 : vtotamt = 0 : sno = 0
        With dgv_Details
            For i = 0 To dgv_Details.Rows.Count - 1

                sno = sno + 1

                .Rows(i).Cells(0).Value = sno

                If Val(dgv_Details.Rows(i).Cells(5).Value) <> 0 Or Val(dgv_Details.Rows(i).Cells(7).Value) <> 0 Then
                    vTotPcs = vTotPcs + Val(dgv_Details.Rows(i).Cells(4).Value)
                    vtotqty = vtotqty + Val(dgv_Details.Rows(i).Cells(5).Value)
                    vTotMtrs = vTotMtrs + Val(dgv_Details.Rows(i).Cells(7).Value)
                    vtotamt = vtotamt + Val(dgv_Details.Rows(i).Cells(10).Value)
                End If
            Next
        End With
        If dgv_Details_Total.Rows.Count <= 0 Then dgv_Details_Total.Rows.Add()
        dgv_Details_Total.Rows(0).Cells(4).Value = Val(vTotPcs)
        dgv_Details_Total.Rows(0).Cells(5).Value = Val(vtotqty)
        dgv_Details_Total.Rows(0).Cells(7).Value = Format(Val(vTotMtrs), "#########0.00")
        dgv_Details_Total.Rows(0).Cells(10).Value = Format(Val(vtotamt), "#########0.00")

        If Val(vtotamt) <> 0 Then
            txt_GrossAmount.Text = Format(Val(vtotamt), "#########0.00")
        End If
        
        Calculation_AgentCommission()

        Calculation_NetAmount()

    End Sub

    Private Sub Calculation_AgentCommission()
        Dim AgCommAmt As Single = 0
        Dim TotQty As Integer = 0
        Dim TotMtrs As Integer = 0

        TotQty = 0 : TotMtrs = 0
        With dgv_Details_Total
            If .RowCount > 0 Then
                TotQty = Val(.Rows(0).Cells(5).Value)
                TotMtrs = Val(.Rows(0).Cells(7).Value)
            End If
        End With

        If Trim(UCase(cbo_CommType.Text)) = "MTR" Then
            AgCommAmt = Val(TotMtrs) * Val(txt_CommPerc.Text)
        ElseIf Trim(UCase(cbo_CommType.Text)) = "QTY" Then
            AgCommAmt = Val(TotQty) * Val(txt_CommPerc.Text)
        Else
            AgCommAmt = Val(txt_GrossAmount.Text) * Val(txt_CommPerc.Text) / 100
        End If

        lbl_CommAmount.Text = Format(Val(AgCommAmt), "#########0.00")

    End Sub

    Private Sub Calculation_NetAmount()
        Dim NtAmt As Single

        If FrmLdSTS = True Then Exit Sub

        lbl_DiscAmount.Text = Format(Val(txt_GrossAmount.Text) * Val(txt_DiscPerc.Text) / 100, "########0.00")

        lbl_AssessableValue.Text = Format(Val(txt_GrossAmount.Text) - Val(lbl_DiscAmount.Text) + Val(txt_AddLess_BeforeTax.Text), "########0.00")

        lbl_TaxAmount.Text = Format(Val(lbl_AssessableValue.Text) * Val(txt_TaxPerc.Text) / 100, "########0.00")

        NtAmt = Val(lbl_AssessableValue.Text) + Val(lbl_TaxAmount.Text) + Val(txt_Freight.Text) + Val(txt_AddLess_AfterTax.Text)

        lbl_NetAmount.Text = Format(Val(NtAmt), "#########0")
        lbl_NetAmount.Text = Common_Procedures.Currency_Format(Val(lbl_NetAmount.Text))

        lbl_RoundOff.Text = Format(Val(CSng(lbl_NetAmount.Text)) - Val(NtAmt), "#########0.00")

    End Sub

    Private Sub cbo_Ledger_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) and Verified_Status = 1)", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Ledger_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, txt_BillNo, cbo_PurchaseAccount, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) and Verified_Status = 1)", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Ledger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ledger.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Ledger, cbo_PurchaseAccount, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) and Verified_Status = 1 ) ", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Ledger_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New LedgerCreation_Processing

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Ledger.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub dgv_Details_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEndEdit
        dgv_Details_CellLeave(sender, e)
    End Sub

    Private Sub dgv_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim rect As Rectangle

        With dgv_Details
            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If

            If e.ColumnIndex = 1 Then

                If cbo_ItemName.Visible = False Or Val(cbo_ItemName.Tag) <> e.RowIndex Then

                    cbo_ItemName.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Processed_Item_Name from Processed_Item_Head where (Processed_Item_IdNo = 0 or Processed_Item_Type = 'GREY') order by Processed_item_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_ItemName.DataSource = Dt1
                    cbo_ItemName.DisplayMember = "Processed_Item_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_ItemName.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_ItemName.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top

                    cbo_ItemName.Width = rect.Width  ' .CurrentCell.Size.Width
                    cbo_ItemName.Height = rect.Height  ' rect.Height
                    cbo_ItemName.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_ItemName.Tag = Val(e.RowIndex)
                    cbo_ItemName.Visible = True

                    cbo_ItemName.BringToFront()
                    cbo_ItemName.Focus()

                End If

            Else

                cbo_ItemName.Visible = False

            End If

            If e.ColumnIndex = 3 Then

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


            End If

            If e.ColumnIndex = 8 Then

                If cbo_Unit.Visible = False Or Val(cbo_Unit.Tag) <> e.RowIndex Then

                    cbo_Unit.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Unit_Name from Unit_Head order by Unit_Name", con)
                    Dt3 = New DataTable
                    Da.Fill(Dt3)
                    cbo_Unit.DataSource = Dt3
                    cbo_Unit.DisplayMember = "Unit_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Unit.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_Unit.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                    cbo_Unit.Width = rect.Width  ' .CurrentCell.Size.Width
                    cbo_Unit.Height = rect.Height  ' rect.Height

                    cbo_Unit.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_Unit.Tag = Val(e.RowIndex)
                    cbo_Unit.Visible = True

                    cbo_Unit.BringToFront()
                    cbo_Unit.Focus()


                End If

            Else

                cbo_Unit.Visible = False

            End If


        End With
    End Sub

    Private Sub dgv_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellLeave
        With dgv_Details
            If e.ColumnIndex = 6 Or e.ColumnIndex = 7 Or e.ColumnIndex = 9 Or e.ColumnIndex = 10 Then
                If Val(.CurrentRow.Cells(e.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(e.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(e.ColumnIndex).Value), "#########0.00")
                Else
                    .CurrentRow.Cells(e.ColumnIndex).Value = ""
                End If
            End If

        End With

    End Sub

    Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged
        Dim q As Single = 0

        Try

            If FrmLdSTS = True Then Exit Sub

            With dgv_Details
                If .Visible Then
                    If .Rows.Count > 0 Then


                        If e.ColumnIndex = 4 Or e.ColumnIndex = 5 Or e.ColumnIndex = 6 Or e.ColumnIndex = 7 Or e.ColumnIndex = 8 Or e.ColumnIndex = 9 Then

                            If Val(e.ColumnIndex) = 5 Or Val(e.ColumnIndex) = 6 Then
                                .CurrentRow.Cells(7).Value = Format(Val(.CurrentRow.Cells(5).Value) * Val(.CurrentRow.Cells(6).Value), "#########0.00")
                            End If


                            If InStr(1, Trim(UCase(.CurrentRow.Cells(8).Value)), "MTR") > 0 Or InStr(1, Trim(UCase(.CurrentRow.Cells(8).Value)), "METER") > 0 Or InStr(1, Trim(UCase(.CurrentRow.Cells(5).Value)), "METRE") > 0 Then
                                q = Val(.CurrentRow.Cells(7).Value)

                            Else
                                q = Val(.CurrentRow.Cells(5).Value)

                            End If

                            .CurrentRow.Cells(10).Value = Format(Val(q) * Val(.CurrentRow.Cells(9).Value), "#########0.00")

                            Calculation_Grid_Total()

                        End If

                    End If

                End If

            End With

        Catch ex As Exception
            '---

        End Try

    End Sub

    Private Sub dgv_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Details.EditingControlShowing
        dgtxt_details = CType(dgv_Details.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_details.Enter
        dgv_Details.EditingControl.BackColor = Color.Lime
        dgv_Details.EditingControl.ForeColor = Color.Blue
        dgtxt_details.SelectAll()
    End Sub

    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_details.KeyPress
        Try
            With dgv_Details
                If .Visible Then
                    If .Rows.Count > 0 Then
                        If .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 6 Or .CurrentCell.ColumnIndex = 7 Or .CurrentCell.ColumnIndex = 9 Then

                            If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                                e.Handled = True
                            End If

                        End If

                    End If

                End If

            End With

        Catch ex As Exception
            '-----

        End Try

    End Sub

    Private Sub dgtxt_details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_details.KeyUp
        dgv_Details_KeyUp(sender, e)
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

                Calculation_Grid_Total()

            End With

        End If

    End Sub

    Private Sub dgv_Details_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Details.RowsAdded
        Dim n As Integer

        With dgv_Details
            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)
        End With
    End Sub

    Private Sub cbo_Unit_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Unit.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Unit_Head", "Unit_Name", "", "(Unit_Idno=0)")
    End Sub

    Private Sub cbo_unit_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Unit.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Unit, Nothing, Nothing, "Unit_Head", "Unit_Name", "", "(Unit_Idno=0)")
        With dgv_Details

            If (e.KeyValue = 38 And cbo_Unit.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_Unit.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
            End If

        End With

    End Sub

    Private Sub cbo_unit_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Unit.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Unit, Nothing, "Unit_Head", "Unit_Name", "", "(Unit_Idno=0)")
        If Asc(e.KeyChar) = 13 Then

            With dgv_Details

                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

            End With

        End If
    End Sub

    Private Sub cbo_unit_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Unit.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Unit_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Unit.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_unit_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Unit.TextChanged
        Try
            If cbo_Unit.Visible Then
                With dgv_Details
                    If Trim(cbo_Unit.Tag) = Trim(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 8 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Unit.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Colour_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Colour.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "colour_Head", "colour_Name", "", "(Colour_IdNo = 0)")

    End Sub

    Private Sub cbo_colour_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Colour.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Colour, cbo_ItemName, Nothing, "colour_Head", "colour_Name", "", "(Colour_IdNo = 0)")
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
                    If Val(cbo_Colour.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 3 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Colour.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_ItemName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ItemName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Processed_Item_Head", "Processed_Item_Name", "(Verified_Status = 1)", "(Processed_Item_IdNo = 0)")

    End Sub

    Private Sub cbo_ItemNameKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ItemName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_ItemName, Nothing, cbo_Colour, "Processed_Item_Head", "Processed_Item_Name", "(Verified_Status = 1)", "(Processed_Item_IdNo = 0)")
        With dgv_Details

            If (e.KeyValue = 38 And cbo_ItemName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                If .CurrentCell.RowIndex = 0 Then
                    txt_VehicleNo.Focus()
                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentRow.Index - 1).Cells(.ColumnCount - 1)
                End If
            End If

            If (e.KeyValue = 40 And cbo_ItemName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                If .CurrentCell.RowIndex = .Rows.Count - 1 And Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(1).Value) = "" Then
                    txt_GrossAmount.Focus()

                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(4)

                End If
            End If
        End With
    End Sub

    Private Sub cbo_ItemNameKeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_ItemName.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim paty_Nm As String
        Dim led_idno As Integer = 0
        Dim itm_idno As Integer = 0

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_ItemName, cbo_Colour, "Processed_Item_Head", "Processed_Item_Name", "(Verified_Status = 1)", "(Processed_Item_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then
            With dgv_Details
                .Rows(.CurrentRow.Index).Cells(2).Value = ""

                led_idno = Common_Procedures.Ledger_AlaisNameToIdNo(con, Trim(cbo_Ledger.Text))
                itm_idno = Common_Procedures.Processed_Item_NameToIdNo(con, Trim(.Rows(.CurrentRow.Index).Cells(1).Value))

                da = New SqlClient.SqlDataAdapter("select * from Ledger_ItemName_Details where Ledger_Idno = " & Str(Val(led_idno)) & " and Item_Idno =  " & Str(Val(itm_idno)), con)
                dt = New DataTable
                da.Fill(dt)

                paty_Nm = ""
                If dt.Rows.Count > 0 Then
                    If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                        paty_Nm = Trim(dt.Rows(0).Item("Party_ItemName").ToString)
                    End If
                End If
                dt.Clear()

                .Rows(.CurrentRow.Index).Cells(2).Value = Trim(paty_Nm)

                da = New SqlClient.SqlDataAdapter("select a.*, b.Unit_Name from Processed_Item_Head a LEFT OUTER JOIN Unit_Head b ON a.Unit_IdNo = b.Unit_IdNo where a.Processed_Item_IdNo = " & Str(Val(itm_idno)), con)
                dt = New DataTable
                da.Fill(dt)

                paty_Nm = ""
                If dt.Rows.Count > 0 Then
                    If IsDBNull(dt.Rows(0).Item("Meter_Qty")) = False Then
                        .Rows(.CurrentRow.Index).Cells(6).Value = Format(Val(dt.Rows(0).Item("Meter_Qty").ToString), "#######0.00")
                        If Val(.Rows(.CurrentRow.Index).Cells(6).Value) = 0 Then .Rows(.CurrentRow.Index).Cells(6).Value = ""
                    End If

                    If IsDBNull(dt.Rows(0).Item("Unit_Name")) = False Then
                        .Rows(.CurrentRow.Index).Cells(8).Value = dt.Rows(0).Item("Unit_Name").ToString
                    End If
                End If
                dt.Clear()

                dt.Dispose()
                da.Dispose()

                If .CurrentCell.RowIndex = .Rows.Count - 1 And Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(1).Value) = "" Then
                    txt_GrossAmount.Focus()

                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(4)

                End If

            End With

        End If
    End Sub

    Private Sub cbo_ItemNameKeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ItemName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Grey_Item_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_ItemName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_ItemNameTextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ItemName.TextChanged
        Try
            If cbo_ItemName.Visible Then
                With dgv_Details
                    If .Visible = True Then
                        If .Rows.Count > 0 Then
                            If Val(cbo_ItemName.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                                .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_ItemName.Text)
                            End If
                        End If
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
        On Error Resume Next
        dgv_Details.CurrentCell.Selected = True
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
                Condt = "a.Item_Purchase_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Item_Purchase_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Item_Purchase_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
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

            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name,C.Ledger_Name as Delv_Name from Item_Purchase_Head a INNER JOIN Ledger_Head b on a.Ledger_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Ledger_Head c ON a.DeliveryTo_Idno = c.Ledger_Idno where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Item_Purchase_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Item_Purchase_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()


                    dgv_Filter_Details.Rows(n).Cells(0).Value = dt2.Rows(i).Item("Item_Purchase_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(1).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Item_Purchase_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Party_Bill_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = dt2.Rows(i).Item("Delv_Name").ToString

                    dgv_Filter_Details.Rows(n).Cells(5).Value = Val(dt2.Rows(i).Item("Total_Pcs").ToString)
                    dgv_Filter_Details.Rows(n).Cells(6).Value = Val(dt2.Rows(i).Item("Total_Qty").ToString)
                    dgv_Filter_Details.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Total_Meter").ToString), "########0.00")
                    'dgv_Filter_Details.Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("Total_Weight").ToString), "########0.000")
                    'dgv_Filter_Details.Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("Net_Amount").ToString), "########0.000")

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

    Private Sub cbo_Filter_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) and Verified_Status = 1 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, txt_filter_billNo, cbo_Filter_DelvAt, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) and Verified_Status = 1)", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, cbo_Filter_DelvAt, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) and Verified_Status = 1and Verified_Status = 1) ", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_DelvAt_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_DelvAt.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", " (ledger_idno = 0 or Ledger_Type = 'GODOWN'  and Verified_Status = 1)", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_DelvAt_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_DelvAt.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_DelvAt, cbo_Filter_PartyName, btn_Filter_Show, "Ledger_AlaisHead", "Ledger_DisplayName", " (ledger_idno = 0 or Ledger_Type = 'GODOWN' and Verified_Status = 1 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_DelvAt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_DelvAt.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_DelvAt, btn_Filter_Show, "Ledger_AlaisHead", "Ledger_DisplayName", " (ledger_idno = 0 or Ledger_Type = 'GODOWN' and Verified_Status = 1) ", "(Ledger_idno = 0)")
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
        '----
    End Sub

    Private Sub txt_Note_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Note.KeyDown
        If e.KeyCode = 40 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                dtp_Date.Focus()
            End If
        End If

        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_Note_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Note.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                dtp_Date.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_DelvAt_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_DelvAt.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", " (ledger_idno = 0 or Ledger_Type = 'GODOWN' and Verified_Status = 1 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_DelvAt_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DelvAt.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_DelvAt, cbo_PurchaseAccount, txt_RecNO, "Ledger_AlaisHead", "Ledger_DisplayName", " (ledger_idno = 0 or Ledger_Type = 'GODOWN' and Verified_Status = 1 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_DelvAt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_DelvAt.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_DelvAt, txt_RecNO, "Ledger_AlaisHead", "Ledger_DisplayName", " (ledger_idno = 0 or Ledger_Type = 'GODOWN' and Verified_Status = 1 ) ", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_DelvAt_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DelvAt.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Common_Procedures.MDI_LedType = ""
            Dim f As New LedgerCreation_Processing

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_DelvAt.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_Agent_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Agent.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'AGENT' and Verified_Status = 1)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Agent_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Agent.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Agent, txt_RecNO, txt_CommPerc, "Ledger_AlaisHead", "Ledger_DisplayName", " (ledger_idno = 0 or Ledger_Type = 'AGENT' and Verified_Status = 1 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Agent_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Agent.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Agent, txt_CommPerc, "Ledger_AlaisHead", "Ledger_DisplayName", " (ledger_idno = 0 or Ledger_Type = 'AGENT' and Verified_Status = 1) ", "(Ledger_idno = 0)")
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

    Private Sub cbo_TransportName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_TransportName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", " ((ledger_idno = 0 or Ledger_Type = 'TRANSPORT') and Verified_Status = 1 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_TransportName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_TransportName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_TransportName, txt_CommPerc, txt_VehicleNo, "Ledger_AlaisHead", "Ledger_DisplayName", " ((ledger_idno = 0 or Ledger_Type = 'TRANSPORT') and Verified_Status = 1)", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Transportname_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_TransportName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_TransportName, txt_VehicleNo, "Ledger_AlaisHead", "Ledger_DisplayName", " ((ledger_idno = 0 or Ledger_Type = 'TRANSPORT') and Verified_Status = 1) ", "(Ledger_idno = 0)")

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

    Private Sub cbo_PurchaseAccount_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PurchaseAccount.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", " ((ledger_idno = 0 or AccountsGroup_IdNo = 27 ) and Verified_Status = 1)", "(Ledger_idno = 0)")

    End Sub

    Private Sub cbo_Purchaseaccount_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PurchaseAccount.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PurchaseAccount, cbo_Ledger, cbo_DelvAt, "Ledger_AlaisHead", "Ledger_DisplayName", " ((ledger_idno = 0 or AccountsGroup_IdNo = 27 ) and Verified_Status = 1)", "(Ledger_idno = 0)")

    End Sub

    Private Sub cbo_Purchaseaccount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PurchaseAccount.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PurchaseAccount, cbo_DelvAt, "Ledger_AlaisHead", "Ledger_DisplayName", " ((ledger_idno = 0 or AccountsGroup_IdNo = 27) and Verified_Status = 1) ", "(Ledger_idno = 0)")
    End Sub

    Private Sub txt_VehicleNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_VehicleNo.KeyDown
        If e.KeyCode = 40 Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)

            Else
                txt_DiscPerc.Focus()

            End If
        End If
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_VehicleNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_VehicleNo.KeyPress

        If Asc(e.KeyChar) = 13 Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)

            Else
                txt_DiscPerc.Focus()

            End If
        End If

    End Sub

    Private Sub txt_CommPerc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_CommPerc.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub cbo_PurchaseAccount_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PurchaseAccount.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            Common_Procedures.MDI_LedType = ""
            Dim f As New LedgerCreation_Processing

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_PurchaseAccount.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_TaxType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_TaxType.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_TaxType, cbo_VatAc, txt_TaxPerc, "", "", "", "")
    End Sub

    Private Sub cbo_TaxType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_TaxType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_TaxType, txt_TaxPerc, "", "", "", "")
    End Sub

    Private Sub cbo_TaxType_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_TaxType.TextChanged
        If Trim(UCase(cbo_TaxType.Text)) = "" Or Trim(UCase(cbo_TaxType.Text)) = "-NIL-" Then txt_TaxPerc.Text = ""
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
    Private Sub cbo_vataccount_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_VatAc.KeyUp
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

    Private Sub txt_TaxPerc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_TaxPerc.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_DiscPerc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_DiscPerc.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_AddLess_BeforeTax_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_AddLess_BeforeTax.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_AddLess_BeforeTax_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_AddLess_BeforeTax.TextChanged
        Calculation_NetAmount()
    End Sub

    Private Sub txt_AddLess_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_AddLess_AfterTax.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_AddLess_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_AddLess_AfterTax.TextChanged
        Calculation_NetAmount()
    End Sub

    Private Sub txt_Freight_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Freight.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_Freight_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Freight.TextChanged
        Calculation_NetAmount()
    End Sub

    Private Sub txt_TaxPerc_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_TaxPerc.TextChanged
        Calculation_NetAmount()
    End Sub

    Private Sub txt_DiscPerc_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_DiscPerc.TextChanged
        Calculation_NetAmount()
    End Sub

    Private Sub cbo_CommType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_CommType.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_CommType, txt_CommPerc, cbo_TransportName, "", "", "", "")
    End Sub

    Private Sub cbo_CommType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_CommType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_CommType, cbo_TransportName, "", "", "", "")
    End Sub

    Private Sub txt_CommPerc_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_CommPerc.TextChanged
        Calculation_AgentCommission()
    End Sub

    Private Sub cbo_CommType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbo_CommType.SelectedIndexChanged
        Calculation_AgentCommission()
    End Sub

    Private Sub txt_GrossAmount_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_GrossAmount.KeyDown
        If e.KeyCode = 38 Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)

            Else
                txt_VehicleNo.Focus()

            End If
        End If
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub txt_GrossAmount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_GrossAmount.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_GrossAmount_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_GrossAmount.TextChanged
        Calculation_NetAmount()
    End Sub

End Class