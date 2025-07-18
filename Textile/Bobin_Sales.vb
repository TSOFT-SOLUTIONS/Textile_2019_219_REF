Imports System.Drawing.Printing
Imports System.IO
Imports System.Windows.Forms.DataVisualization.Charting
Public Class Bobin_Sales
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "BSALS-"
    Private Pk_Condition1 As String = "BSLFR-"
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
    Private prn_DetSNo As Integer
    Private NoCalc_Status As Boolean
    Private vPrn_PvuEdsCnt As String
    Private vPrn_PvuTotBms As Integer
    Private vPrn_PvuTotMtrs As Single
    Private vPrn_PvuSetNo As String
    Private vPrn_PvuBmNos1 As String
    Private vPrn_PvuBmNos2 As String
    Private vPrn_PvuBmNos3 As String
    Private vPrn_PvuBmNos4 As String
    Private WithEvents dgtxt_BobinDetails As New DataGridViewTextBoxEditingControl
    Private WithEvents dgtxt_KuriDetails As New DataGridViewTextBoxEditingControl
    Private dgv_ActCtrlName As String = ""
    Private dgv_LevColNo As Integer
    Private Balance_Bobin As Double = 0
    Private Balance_Amount As String = ""
    Public vmskOldText As String = ""
    Public vmskSelStrt As Integer = -1

    Private prn_OriDupTri As String = ""
    Private prn_InpOpts As String = ""
    Private prn_Count As Integer

    Dim prn_GST_Perc As Single
    Dim prn_CGST_Amount As Double
    Dim prn_SGST_Amount As Double
    Dim prn_IGST_Amount As Double

    Private Polister_STS As Integer = 0


    Private Sub clear()
        New_Entry = False
        Insert_Entry = False

        NoCalc_Status = True

        pnl_Back.Enabled = True
        pnl_Filter.Visible = False

        vmskOldText = ""
        vmskSelStrt = -1
        txt_InvoicePrefixNo.Text = ""
        lbl_DcNo.Text = ""
        lbl_DcNo.ForeColor = Color.Black
        msk_Date.Text = ""
        dtp_Date.Text = ""
        lbl_Net_Amt.Text = ""
        cbo_Ledger.Text = ""
        cbo_VechileNo.Text = ""
        cbo_Transport.Text = ""
        txt_Freight.Text = ""
        txt_PartyBobin.Text = ""
        txt_OurBobin.Text = ""
        txt_Remarks.Text = ""
        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))
        dgv_BobinDetails.Rows.Clear()

        cbo_SalesAcc.Text = Common_Procedures.Ledger_IdNoToName(con, 22)

        Grid_DeSelect()
        chk_Polister.Checked = False

        cbo_BobinColour.Visible = False
        cbo_BobinColour.Tag = -1

        cbo_BobinColour.Text = ""

        lbl_grid_GstPerc.Text = ""
        lbl_Grid_HSNCode.Text = ""
        lbl_CGST_Amount.Text = ""
        lbl_AssessableValue.Text = ""
        lbl_SGST_Amount.Text = ""
        lbl_IGST_Amount.Text = ""
        txt_Freight.Text = ""
        txt_AddLess.Text = ""
        cbo_ItemGroup.Text = ""
        cbo_Type.Text = "GST"
        'dgv_Details.Tag = ""
        'dgv_LevColNo = -1

        lbl_RoundOff.Text = ""

        txt_eInvoiceNo.Text = ""
        txt_eInvoiceAckNo.Text = ""
        txt_eInvoiceAckDate.Text = ""
        txt_eInvoice_CancelStatus.Text = ""
        txt_EInvoiceCancellationReson.Text = ""
        pic_IRN_QRCode_Image.BackgroundImage = Nothing

        txt_eWayBill_No.Text = ""
        txt_EWB_Date.Text = ""
        txt_EWB_ValidUpto.Text = ""
        txt_EWB_Cancel_Status.Text = ""
        txt_EWB_Canellation_Reason.Text = ""

        txt_eWayBill_No.Enabled = True
        rtbeInvoiceResponse.Text = ""

        cbo_Ledger.Enabled = True
        cbo_Ledger.BackColor = Color.White

        cbo_BobinColour.Enabled = True
        cbo_BobinColour.BackColor = Color.White

        dgv_BobinDetails.ReadOnly = False

        dgv_ActCtrlName = ""
        NoCalc_Status = False
    End Sub

    Private Sub Grid_DeSelect()
        On Error Resume Next
        If Not IsNothing(dgv_BobinDetails.CurrentCell) Then dgv_BobinDetails.CurrentCell.Selected = False
        'dgv_BobinDetails.CurrentCell.Selected = False

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
        ElseIf TypeOf Me.ActiveControl Is MaskedTextBox Then
            msktxbx = Me.ActiveControl
            msktxbx.SelectionStart = 0
        ElseIf TypeOf Me.ActiveControl Is ComboBox Then
            combobx = Me.ActiveControl
            combobx.SelectAll()
        End If

        If Me.ActiveControl.Name <> cbo_BobinColour.Name Then
            cbo_BobinColour.Visible = False
        End If

        If Me.ActiveControl.Name <> dgv_BobinDetails.Name Then
            Grid_DeSelect()
        End If


        'If Me.ActiveControl.Name <> dgv_BobinDetails.Name Then
        '    Common_Procedures.Hide_CurrentStock_Display()
        'End If

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
        If FrmLdSTS = True Then Exit Sub
        If Not IsNothing(dgv_BobinDetails.CurrentCell) Then dgv_BobinDetails.CurrentCell.Selected = False
        If Not IsNothing(dgv_BobinDetails_Total.CurrentCell) Then dgv_BobinDetails_Total.CurrentCell.Selected = False
        If Not IsNothing(dgv_Filter_Details.CurrentCell) Then dgv_Filter_Details.CurrentCell.Selected = False


        'dgv_BobinDetails.CurrentCell.Selected = False
        'dgv_BobinDetails_Total.CurrentCell.Selected = False
        'dgv_Filter_Details.CurrentCell.Selected = False

    End Sub

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
        NoCalc_Status = True
        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name from Bobin_Sales_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo   Where a.Bobin_Sales_Code = '" & Trim(NewCode) & "'", con)
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then
                txt_InvoicePrefixNo.Text = dt1.Rows(0).Item("Invoice_PrefixNo").ToString
                lbl_DcNo.Text = dt1.Rows(0).Item("Bobin_Sales_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Bobin_Sales_Date").ToString
                msk_Date.Text = dtp_Date.Text
                cbo_Ledger.Text = dt1.Rows(0).Item("Ledger_Name").ToString
                cbo_VechileNo.Text = dt1.Rows(0).Item("Vechile_No").ToString
                txt_Freight.Text = Format(Val(dt1.Rows(0).Item("Freight").ToString), "########0.00")
                cbo_Transport.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Transport_IdNo").ToString))
                txt_PartyBobin.Text = Format(Val(dt1.Rows(0).Item("Party_Bobin").ToString), "########0.00")
                txt_OurBobin.Text = Format(Val(dt1.Rows(0).Item("OurOwn_Bobin").ToString), "########0.00")
                txt_Remarks.Text = dt1.Rows(0).Item("Remarks").ToString
                cbo_SalesAcc.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("SalesAc_IdNo").ToString))

                txt_AddLess.Text = Format(Val(dt1.Rows(0).Item("AddLess_BeforeTax").ToString), "########0.00")
                lbl_AssessableValue.Text = Format(Val(dt1.Rows(0).Item("Total_Taxable_Value").ToString), "########0.00")
                lbl_CGST_Amount.Text = Format(Val(dt1.Rows(0).Item("Total_CGST_Amount").ToString), "########0.00")
                lbl_SGST_Amount.Text = Format(Val(dt1.Rows(0).Item("Total_SGST_Amount").ToString), "########0.00")
                lbl_IGST_Amount.Text = Format(Val(dt1.Rows(0).Item("Total_IGST_Amount").ToString), "########0.00")
                lbl_grid_GstPerc.Text = Format(Val(dt1.Rows(0).Item("GST_Percentage").ToString), "########0.00")
                lbl_Grid_HSNCode.Text = dt1.Rows(0).Item("HSN_Code").ToString
                cbo_Type.Text = dt1.Rows(0).Item("Entry_VAT_GST_Type").ToString
                cbo_ItemGroup.Text = Common_Procedures.ItemGroup_IdNoToName(con, Val(dt1.Rows(0).Item("Item_Group_id").ToString))
                lbl_Net_Amt.Text = Format(Val(dt1.Rows(0).Item("Net_Amount").ToString), "########0.00")

                lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))
                If Val(dt1.Rows(0).Item("Polister_Status").ToString) = 1 Then chk_Polister.Checked = True Else chk_Polister.Checked = False

                lbl_RoundOff.Text = Format(Val(dt1.Rows(0).Item("RoundOff_Amount").ToString), "#########0.00")

                txt_eInvoiceNo.Text = Trim(dt1.Rows(0).Item("E_Invoice_IRNO").ToString)
                txt_eInvoiceNo.Text = Trim(dt1.Rows(0).Item("E_Invoice_IRNO").ToString)
                If Not IsDBNull(dt1.Rows(0).Item("E_Invoice_ACK_No")) Then txt_eInvoiceAckNo.Text = Trim(dt1.Rows(0).Item("E_Invoice_ACK_No").ToString)
                If Not IsDBNull(dt1.Rows(0).Item("E_Invoice_ACK_Date")) Then
                    If IsDate(dt1.Rows(0).Item("E_Invoice_ACK_Date")) Then
                        If Year(dt1.Rows(0).Item("E_Invoice_ACK_Date")) <> 1900 Then
                            txt_eInvoiceAckDate.Text = Format(Convert.ToDateTime(dt1.Rows(0).Item("E_Invoice_ACK_Date")), "dd-MM-yyyy hh:mm tt").ToString
                        End If
                    End If
                End If
                If Trim(txt_eInvoiceNo.Text) <> "" Then
                    If Not IsDBNull(dt1.Rows(0).Item("E_Invoice_Cancelled_Status")) Then txt_eInvoice_CancelStatus.Text = IIf(dt1.Rows(0).Item("E_Invoice_Cancelled_Status") = True, "Cancelled", "Active")
                End If
                If IsDBNull(dt1.Rows(0).Item("E_Invoice_QR_Image")) = False Then
                    Dim imageData As Byte() = DirectCast(dt1.Rows(0).Item("E_Invoice_QR_Image"), Byte())
                    If Not imageData Is Nothing Then
                        Using ms As New MemoryStream(imageData, 0, imageData.Length)
                            ms.Write(imageData, 0, imageData.Length)
                            If imageData.Length > 0 Then
                                pic_IRN_QRCode_Image.BackgroundImage = Image.FromStream(ms)
                            End If
                        End Using
                    End If
                End If

                If Not IsDBNull(dt1.Rows(0).Item("E_Invoice_Cancellation_Reason")) Then txt_EInvoiceCancellationReson.Text = Trim(dt1.Rows(0).Item("E_Invoice_Cancellation_Reason").ToString)

                If Not IsDBNull(Trim(dt1.Rows(0).Item("E_Invoice_Cancelled_Status"))) Then
                    If dt1.Rows(0).Item("E_Invoice_Cancelled_Status") = True Then
                        txt_eInvoice_CancelStatus.Text = "Cancelled"
                    Else
                        txt_eInvoice_CancelStatus.Text = "Active"
                    End If
                End If

                If Not IsDBNull(dt1.Rows(0).Item("EWB_No")) Then txt_eWayBill_No.Text = Trim(dt1.Rows(0).Item("EWB_No").ToString)
                If Not IsDBNull(dt1.Rows(0).Item("EWB_Date")) Then txt_EWB_Date.Text = Trim(dt1.Rows(0).Item("EWB_Date").ToString)
                If Not IsDBNull(dt1.Rows(0).Item("EWB_Valid_Upto")) Then txt_EWB_ValidUpto.Text = Trim(dt1.Rows(0).Item("EWB_Valid_Upto").ToString)
                If Not IsDBNull(dt1.Rows(0).Item("EWB_Cancelled")) Then
                    If dt1.Rows(0).Item("EWB_Cancelled") = True Then
                        txt_EWB_Cancel_Status.Text = "Cancelled"
                    Else
                        txt_EWB_Cancel_Status.Text = "Active"
                    End If
                End If

                If Not IsDBNull(dt1.Rows(0).Item("E_Invoice_Cancellation_Reason")) Then txt_EWB_Canellation_Reason.Text = Trim(dt1.Rows(0).Item("E_Invoice_Cancellation_Reason").ToString)



                'If IsDBNull(dt1.Rows(0).Item("BobinSales_Invoice_Code").ToString) = False Then
                '    If Trim(dt1.Rows(0).Item("BobinSales_Invoice_Code").ToString) <> "" Then LockSTS = True
                'End If

                da2 = New SqlClient.SqlDataAdapter("select a.*,  c.Colour_Name from Bobin_Sales_Bobin_Details a LEFT OUTER JOIN Colour_Head c ON a.Colour_IdNo = c.Colour_IdNo  Where a.Bobin_Sales_Code = '" & Trim(NewCode) & "' Order by a.Sl_No", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_BobinDetails.Rows.Clear()
                SNo = 0

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_BobinDetails.Rows.Add()

                        SNo = SNo + 1
                        dgv_BobinDetails.Rows(n).Cells(0).Value = Val(SNo)
                        dgv_BobinDetails.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Ends").ToString
                        dgv_BobinDetails.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Colour_Name").ToString

                        dgv_BobinDetails.Rows(n).Cells(3).Value = Val(dt2.Rows(i).Item("Bobins").ToString)
                        dgv_BobinDetails.Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("Meter_Bobin").ToString), "########0.00")
                        dgv_BobinDetails.Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("METERS").ToString), "########0.00")

                        dgv_BobinDetails.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Meter_Reel").ToString), "########0.00")
                        dgv_BobinDetails.Rows(n).Cells(7).Value = Val(dt2.Rows(i).Item("reel").ToString)
                        dgv_BobinDetails.Rows(n).Cells(9).Value = Format(Val(dt2.Rows(i).Item("Weight").ToString), "########0.00")

                        dgv_BobinDetails.Rows(n).Cells(10).Value = Format(Val(dt2.Rows(i).Item("Rate").ToString), "########0.00")
                        dgv_BobinDetails.Rows(n).Cells(11).Value = Format(Val(dt2.Rows(i).Item("Amount").ToString), "########0.00")

                        ' dgv_BobinDetails.Rows(n).Cells(7).Value = dt2.Rows(i).Item("Bobin_Jari_Sales_Invoice_Code").ToString
                        ' dgv_BobinDetails.Rows(n).Cells(8).Value = dt2.Rows(i).Item("Bobin_Sales_Bobin_Slno").ToString

                        'If Val(dgv_KuriDetails.Rows(n).Cells(7).Value) <> 0 Then
                        '    For j = 0 To dgv_KuriDetails.ColumnCount - 1
                        '        dgv_KuriDetails.Rows(n).Cells(j).Style.BackColor = Color.LightGray
                        '    Next j
                        '    LockSTS = True
                        'End If
                    Next i

                End If
                dt2.Clear()

                With dgv_BobinDetails_Total
                    If .RowCount = 0 Then .Rows.Add()
                    .Rows(0).Cells(3).Value = Val(dt1.Rows(0).Item("Total_Bobins").ToString)
                    .Rows(0).Cells(5).Value = Format(Val(dt1.Rows(0).Item("Total_Meters").ToString), "########0.00")
                    .Rows(0).Cells(7).Value = Val(dt1.Rows(0).Item("Total_Reels").ToString)
                    .Rows(0).Cells(9).Value = Format(Val(dt1.Rows(0).Item("Total_Weight").ToString), "########0.00")
                    .Rows(0).Cells(11).Value = Format(Val(dt1.Rows(0).Item("Total_Amount").ToString), "########0.00")
                    'lbl_Net_Amt.Text = Format(Val(dt1.Rows(0).Item("net_Amount").ToString), "########0.00")
                End With

            End If
            dt1.Clear()

            Grid_DeSelect()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            dgv_ActCtrlName = ""
            dt1.Dispose()
            da1.Dispose()
            dt2.Dispose()
            da2.Dispose()

        End Try

        NoCalc_Status = False
        If msk_Date.Visible And msk_Date.Enabled Then msk_Date.Focus()

    End Sub

    Private Sub Bobin_Sales_Delivery_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Activated

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Ledger.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Ledger.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Transport.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Transport.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_BobinColour.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "BORDER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_BobinColour.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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
            '----MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        FrmLdSTS = False

    End Sub

    Private Sub Bobin_Sales_Delivery_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Me.Text = ""

        con.Open()


        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 ) order by Ledger_DisplayName", con)
        da.Fill(dt2)
        cbo_Ledger.DataSource = dt2
        cbo_Ledger.DisplayMember = "Ledger_DisplayName"


        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or Ledger_Type = 'TRANSPORT') order by Ledger_DisplayName", con)
        da.Fill(dt3)
        cbo_Transport.DataSource = dt3
        cbo_Transport.DisplayMember = "Ledger_DisplayName"


        'da = New SqlClient.SqlDataAdapter("select distinct(Vechile_No) from Bobin_Sales_Head order by Vechile_No", con)
        'da.Fill(dt4)
        'cbo_VechileNo.DataSource = dt4
        'cbo_VechileNo.DisplayMember = "Vechile_No"

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2

        cbo_Type.Items.Clear()
        cbo_Type.Items.Add(" ")
        cbo_Type.Items.Add("GST")
        cbo_Type.Items.Add("NO TAX")

        AddHandler msk_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_BobinColour.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_SalesAcc.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Ledger.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_VechileNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Transport.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Freight.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PartyBobin.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_OurBobin.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Remarks.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_EndsName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Type.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ItemGroup.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Freight.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_AddLess.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_InvoicePrefixNo.GotFocus, AddressOf ControlGotFocus

        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_BobinColour.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Ledger.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Transport.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_VechileNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Freight.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PartyBobin.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_OurBobin.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Remarks.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_SalesAcc.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_EndsName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Type.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ItemGroup.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ItemGroup.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Freight.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_AddLess.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_InvoicePrefixNo.LostFocus, AddressOf ControlLostFocus

        AddHandler msk_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_PartyBobin.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler txt_Remarks.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler txt_Freight.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_AddLess.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler msk_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_PartyBobin.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_Fromdate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_ToDate.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_Remarks.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_Freight.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_AddLess.KeyPress, AddressOf TextBoxControlKeyPress

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True
        new_record()

    End Sub

    Private Sub Bobin_Sales_Delivery_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
        'Common_Procedures.Hide_CurrentStock_Display()
    End Sub

    Private Sub Bobin_Sales_Delivery_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress

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

        If ActiveControl.Name = dgv_BobinDetails.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            dgv1 = Nothing

            If ActiveControl.Name = dgv_BobinDetails.Name Then
                dgv1 = dgv_BobinDetails

            ElseIf dgv_BobinDetails.IsCurrentRowDirty = True Then
                dgv1 = dgv_BobinDetails

            ElseIf Trim(UCase(dgv_ActCtrlName)) = Trim(UCase(dgv_BobinDetails.Name.ToString)) Then
                dgv1 = dgv_BobinDetails

            End If

            If IsNothing(dgv1) = False Then

                With dgv1

                    If keyData = Keys.Enter Or keyData = Keys.Down Then
                        If .CurrentCell.ColumnIndex >= .ColumnCount - 10 Then

                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                If dgv1.Name = dgv_BobinDetails.Name Then
                                    txt_PartyBobin.Focus()
                                Else
                                    txt_Freight.Focus()
                                End If

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
                                If dgv1.Name = dgv_BobinDetails.Name Then
                                    txt_Freight.Focus()
                                Else
                                    If dgv_BobinDetails.Rows.Count > 0 Then
                                        dgv_BobinDetails.Focus()
                                        dgv_BobinDetails.CurrentCell = dgv_BobinDetails.Rows(0).Cells(1)
                                        dgv_BobinDetails.CurrentCell.Selected = True
                                    Else
                                        txt_Freight.Focus()
                                    End If

                                End If

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 4)

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

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Processing_Receipt_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Processing_Receipt_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If


        'Da = New SqlClient.SqlDataAdapter("select BobinSales_Invoice_Code from Bobin_Sales_Head Where Bobin_Sales_Code = '" & Trim(NewCode) & "'", con)
        'Dt1 = New DataTable
        'Da.Fill(Dt1)

        'If Dt1.Rows.Count > 0 Then
        '    If IsDBNull(Dt1.Rows(0).Item("BobinSales_Invoice_Code").ToString) = False Then
        '        If Trim(Dt1.Rows(0).Item("BobinSales_Invoice_Code").ToString) <> "" Then
        '            MessageBox.Show("Already Invoiced", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '            Exit Sub

        '        End If
        '    End If
        'End If
        Dt1.Clear()

        trans = con.BeginTransaction

        Try

            cmd.Connection = con
            cmd.Transaction = trans


            If Common_Procedures.VoucherBill_Deletion(con, Trim(Pk_Condition) & Trim(NewCode), trans) = False Then
                Throw New ApplicationException("Error on Voucher Bill Deletion")
            End If

            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), trans)
            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition1) & Trim(NewCode), trans)

            'cmd.CommandText = "Delete from Stock_Pavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            'cmd.ExecuteNonQuery()

            'cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            'cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Empty_BeamBagCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Bobin_Sales_Bobin_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Bobin_Sales_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            'cmd.CommandText = "Delete from Bobin_Sales_Jari_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Bobin_Sales_Code = '" & Trim(NewCode) & "'"
            'cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Bobin_Sales_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Bobin_Sales_Code = '" & Trim(NewCode) & "'"
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

            da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead order by Ledger_DisplayName", con)
            da.Fill(dt1)
            cbo_Filter_PartyName.DataSource = dt1
            cbo_Filter_PartyName.DisplayMember = "Ledger_DisplayName"

            da = New SqlClient.SqlDataAdapter("select EndsCount_name from EndsCount_head order by EndsCount_name", con)
            da.Fill(dt2)
            cbo_Filter_EndsName.DataSource = dt2
            cbo_Filter_EndsName.DisplayMember = "EndsCount_name"


            dtp_Filter_Fromdate.Text = ""
            dtp_Filter_ToDate.Text = ""
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_EndsName.Text = ""

            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_EndsName.SelectedIndex = -1
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

            da = New SqlClient.SqlDataAdapter("select top 1 Bobin_Sales_No from Bobin_Sales_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Bobin_Sales_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Bobin_Sales_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_DcNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Bobin_Sales_No from Bobin_Sales_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Bobin_Sales_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Bobin_Sales_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_DcNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Bobin_Sales_No from Bobin_Sales_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Bobin_Sales_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Bobin_Sales_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 Bobin_Sales_No from Bobin_Sales_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Bobin_Sales_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Bobin_Sales_No desc", con)
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

            lbl_DcNo.Text = Common_Procedures.get_MaxCode(con, "Bobin_Sales_Head", "Bobin_Sales_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

            lbl_DcNo.ForeColor = Color.Red

            msk_Date.Text = Date.Today.ToShortDateString
            da = New SqlClient.SqlDataAdapter("select top 1 * from Bobin_Sales_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Bobin_Sales_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Bobin_Sales_No desc", con)
            dt1 = New DataTable
            da.Fill(dt1)
            If dt1.Rows.Count > 0 Then
                If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)
                    If dt1.Rows(0).Item("Bobin_Sales_Date").ToString <> "" Then msk_Date.Text = dt1.Rows(0).Item("Bobin_Sales_Date").ToString
                    If dt1.Rows(0).Item("Invoice_PrefixNo").ToString <> "" Then txt_InvoicePrefixNo.Text = dt1.Rows(0).Item("Invoice_PrefixNo").ToString

                End If
            End If
            dt1.Clear()


            If msk_Date.Enabled And msk_Date.Visible Then
                msk_Date.Focus()
                msk_Date.SelectionStart = 0
            End If

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

            inpno = InputBox("Enter Dc.No.", "FOR FINDING...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Bobin_Sales_No from Bobin_Sales_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Bobin_Sales_Code = '" & Trim(RecCode) & "'", con)
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

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Processing_Receipt_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Processing_Receipt_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        Try

            inpno = InputBox("Enter New Dc No.", "FOR NEW DELIVERY INSERTION...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Bobin_Sales_No from Bobin_Sales_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Bobin_Sales_Code = '" & Trim(RecCode) & "'", con)
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
                    MessageBox.Show("Invalid Dc No", "DOES NOT INSERT NEW DELIVERY...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_DcNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT NEW DELIVERY...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim Led_ID As Integer = 0
        Dim Ens_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim Partcls As String = ""
        Dim EntID As String = ""
        Dim Cnt_ID As Integer = 0
        Dim Delv_ID As Integer = 0
        Dim Rec_ID As Integer = 0
        Dim Siz_ID As Integer = 0
        Dim Clr_ID As Integer = 0
        Dim BthSz_ID As Integer = 0
        Dim Trans_ID As Integer = 0
        Dim vEdsCnt_ID As Integer = 0
        Dim ItemGrpID As Integer = 0
        Dim PBlNo As String = ""
        Dim vTotBbns As Single, vTotMtrs As Single
        Dim vTotReel As Single, vTotamt As Single, vTotWgt As Single
        Dim Nr As Integer = 0
        Dim vOrdByNo As Single = 0
        Dim noStockpost As Integer = 0
        Dim SlAc_ID As Integer = 0
        Dim vEInvAckDate As String = ""


        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        ' If Common_Procedures.UserRight_Check(Common_Procedures.UR.Processing_Receipt_Entry, New_Entry) = False Then Exit Sub

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

        SlAc_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_SalesAcc.Text)
        If SlAc_ID = 0 Then
            MessageBox.Show("Invalid Sales A/c", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_SalesAcc.Enabled Then cbo_SalesAcc.Focus()
            Exit Sub
        End If

        ItemGrpID = Common_Procedures.ItemGroup_NameToIdNo(con, cbo_ItemGroup.Text)
        Delv_ID = 0  ' Led_ID

        Rec_ID = Val(Common_Procedures.CommonLedger.Godown_Ac)

        Trans_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Transport.Text)






        Dim eiCancel As String = "0"
        If txt_eInvoice_CancelStatus.Text = "Cancelled" Then
            eiCancel = "1"
        End If
        Dim EWBCancel As String = "0"
        If txt_EWB_Cancel_Status.Text = "Cancelled" Then
            EWBCancel = "1"
        End If

        With dgv_BobinDetails

            For i = 0 To dgv_BobinDetails.RowCount - 1

                If Val(.Rows(i).Cells(4).Value) <> 0 Or Val(.Rows(i).Cells(6).Value) <> 0 And Val(.Rows(i).Cells(10).Value) <> 0 Then

                    If Trim(.Rows(i).Cells(2).Value) = "" Then
                        MessageBox.Show("Invalid Ends/Count", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(1)
                        End If
                        Exit Sub
                    End If

                    If Val(.Rows(i).Cells(5).Value) = 0 Then
                        MessageBox.Show("Invalid Meters..", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled Then .Focus()
                        .CurrentCell = .Rows(0).Cells(6)
                        Exit Sub
                    End If

                    'If Val(.Rows(i).Cells(7).Value) = 0 Then
                    '    MessageBox.Show("Invalid Reel..", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    '    If .Enabled Then .Focus()
                    '    .CurrentCell = .Rows(0).Cells(8)
                    '    Exit Sub
                    'End If

                    'If Val(.Rows(i).Cells(9).Value) = 0 Then
                    '    MessageBox.Show("Invalid Weights..", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    '    If .Enabled Then .Focus()
                    '    .CurrentCell = .Rows(0).Cells(9)
                    '    Exit Sub
                    'End If

                End If

            Next i

        End With
        ' lbl_UserName.Text = Common_Procedures.User.IdNo
        NoCalc_Status = False
        Total_Calculation()

        vTotBbns = 0 : vTotMtrs = 0 : vTotReel = 0 : vTotamt = 0 : vTotWgt = 0
        If dgv_BobinDetails_Total.RowCount > 0 Then
            vTotBbns = Val(dgv_BobinDetails_Total.Rows(0).Cells(3).Value())
            vTotMtrs = Val(dgv_BobinDetails_Total.Rows(0).Cells(5).Value())
            vTotReel = Val(dgv_BobinDetails_Total.Rows(0).Cells(7).Value())
            vTotWgt = Val(dgv_BobinDetails_Total.Rows(0).Cells(9).Value())
            vTotamt = Val(dgv_BobinDetails_Total.Rows(0).Cells(11).Value())

        End If

        Polister_STS = 0
        If chk_Polister.Checked = True Then Polister_STS = 1

        'If (Val(txt_OurBobin.Text) + Val(txt_PartyBobin.Text)) <> Val(vTotBbns) Then
        '    MessageBox.Show("Invalid Bobins..", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    If txt_PartyBobin.Enabled Then txt_PartyBobin.Focus()
        '    Exit Sub
        'End If

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_DcNo.Text = Common_Procedures.get_MaxCode(con, "Bobin_Sales_Head", "Bobin_Sales_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            'Da = New SqlClient.SqlDataAdapter("select count(*) from Bobin_Sales_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Bobin_Sales_Code = '" & Trim(NewCode) & "' and BobinSales_Invoice_Code <> ''", con)
            'Da.SelectCommand.Transaction = tr
            'Dt1 = New DataTable
            'Da.Fill(Dt1)
            'If Dt1.Rows.Count > 0 Then
            '    If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
            '        If Val(Dt1.Rows(0)(0).ToString) > 0 Then
            '            Throw New ApplicationException("Already Invoiced")
            '            Exit Sub
            '        End If
            '    End If
            'End If
            'Dt1.Clear()

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@EntryDate", dtp_Date.Value.Date)

            vEInvAckDate = ""
            If Trim(txt_eInvoiceAckDate.Text) <> "" Then
                If IsDate(txt_eInvoiceAckDate.Text) = True Then
                    If Year(CDate(txt_eInvoiceAckDate.Text)) <> 1900 Then
                        vEInvAckDate = Trim(txt_eInvoiceAckDate.Text)
                    End If

                End If
            End If
            If Trim(vEInvAckDate) <> "" Then
                cmd.Parameters.AddWithValue("@EInvoiceAckDate", Convert.ToDateTime(vEInvAckDate))
            End If


            Dim ms As New MemoryStream()
            If IsNothing(pic_IRN_QRCode_Image.BackgroundImage) = False Then
                Dim bitmp As New Bitmap(pic_IRN_QRCode_Image.BackgroundImage)
                bitmp.Save(ms, Drawing.Imaging.ImageFormat.Jpeg)
            End If
            Dim data As Byte() = ms.GetBuffer()
            Dim p As New SqlClient.SqlParameter("@QrCode", SqlDbType.Image)
            p.Value = data
            cmd.Parameters.Add(p)
            ms.Dispose()


            vOrdByNo = Val(Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text))


            If New_Entry = True Then

                cmd.CommandText = "Insert into Bobin_Sales_Head ( Bobin_Sales_Code      ,           Company_IdNo           ,             Bobin_Sales_No   ,   for_OrderBy        , Bobin_Sales_Date , Invoice_PrefixNo                                 , Ledger_IdNo             ,       Vechile_No                 ,           Freight                ,       Transport_IdNo     ,       Total_Bobins         ,           Total_Meters    ,           Total_Reels       ,   Total_Amount           ,       Total_Weight         ,       Party_Bobin                    ,       OurOwn_Bobin                 ,               Remarks           ,       SalesAc_IdNo  ,           User_IdNo                     ,             AddLess_BeforeTax     ,           Total_Taxable_Value             ,                Total_CGST_Amount     ,                 Total_SGST_Amount    ,         Total_IGST_Amount            ,       Entry_VAT_GST_Type    , HSN_Code                            ,  GST_Percentage                       , Item_Group_id         ,       Net_Amount                         ,   Polister_Status             ,                RoundOff_Amount       ,             E_Invoice_IRNO          ,   E_Invoice_QR_Image ,         E_Invoice_ACK_No          ,                 E_Invoice_ACK_Date )" &
                "Values                                         ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(vOrdByNo) & ", @EntryDate       , '" & Trim(UCase(txt_InvoicePrefixNo.Text)) & "'  , " & Str(Val(Led_ID)) & ",'" & Trim(cbo_VechileNo.Text) & "'," & Str(Val(txt_Freight.Text)) & "," & Str(Val(Trans_ID)) & ", " & Str(Val(vTotBbns)) & " , " & Str(Val(vTotMtrs)) & ",  " & Str(Val(vTotReel)) & " , " & Str(Val(vTotamt)) & ",  " & Str(Val(vTotWgt)) & " , " & Str(Val(txt_PartyBobin.Text)) & ", " & Str(Val(txt_OurBobin.Text)) & ", '" & Trim(txt_Remarks.Text) & "', " & Val(SlAc_ID) & "," & Val(Common_Procedures.User.IdNo) & " ," & Str(Val(txt_AddLess.Text)) & " , " & Str(Val(lbl_AssessableValue.Text)) & "," & Str(Val(lbl_CGST_Amount.Text)) & "," & Str(Val(lbl_SGST_Amount.Text)) & "," & Str(Val(lbl_IGST_Amount.Text)) & ",'" & Trim(cbo_Type.Text) & "','" & Trim(lbl_Grid_HSNCode.Text) & "'," & Str(Val(lbl_grid_GstPerc.Text)) & ", " & Val(ItemGrpID) & ",  " & Str(Val(CSng(lbl_Net_Amt.Text))) & ", " & Str(Val(Polister_STS)) & ", " & Str(Val(lbl_RoundOff.Text)) & "  , '" & Trim(txt_eInvoiceNo.Text) & "' ,         @QrCode      ,  '" & txt_eInvoiceAckNo.Text & "' , " & IIf(Trim(vEInvAckDate) <> "", "@EInvoiceAckDate", "Null") & ")"
                cmd.ExecuteNonQuery()

            Else
                cmd.CommandText = "Update Bobin_Sales_Head set Bobin_Sales_Date = @EntryDate, Ledger_IdNo = " & Val(Led_ID) & ", Vechile_No = '" & Trim(cbo_VechileNo.Text) & "', Freight = " & Str(Val(txt_Freight.Text)) & ",Invoice_PrefixNo = '" & Trim(UCase(txt_InvoicePrefixNo.Text)) & "' , Transport_IdNo = " & Str(Val(Trans_ID)) & ", Total_Bobins = " & Val(vTotBbns) & " , Total_Meters = " & Val(vTotMtrs) & ", SalesAc_IdNo = " & Val(SlAc_ID) & " ,  Total_Reels = " & Val(vTotReel) & ", Total_Amount = " & Val(vTotamt) & ", Total_Weight = " & Val(vTotWgt) & ", Party_Bobin = " & Str(Val(txt_PartyBobin.Text)) & " , OurOwn_Bobin = " & Str(Val(txt_OurBobin.Text)) & ", Remarks = '" & Trim(txt_Remarks.Text) & "' , User_Idno = " & Val(Common_Procedures.User.IdNo) & " ,AddLess_BeforeTax=" & Str(Val(txt_AddLess.Text)) & ",Total_Taxable_Value=" & Str(Val(lbl_AssessableValue.Text)) & ",Total_CGST_Amount=" & Str(Val(lbl_CGST_Amount.Text)) & ",Total_SGST_Amount=" & Str(Val(lbl_SGST_Amount.Text)) & ",Polister_Status=" & Str(Val(Polister_STS)) & ",Total_IGST_Amount=" & Str(Val(lbl_IGST_Amount.Text)) & ",Entry_VAT_GST_Type='" & Trim(cbo_Type.Text) & "',HSN_Code='" & Trim(lbl_Grid_HSNCode.Text) & "' , GST_Percentage=" & Str(Val(lbl_grid_GstPerc.Text)) & " , Item_Group_id =" & Val(ItemGrpID) & ",Net_Amount=" & Str(Val(CSng(lbl_Net_Amt.Text))) & " , RoundOff_Amount = " & Str(Val(lbl_RoundOff.Text)) & " ,  E_Invoice_IRNO = '" & Trim(txt_eInvoiceNo.Text) & "' , E_Invoice_QR_Image =  @QrCode  , E_Invoice_ACK_No = '" & txt_eInvoiceAckNo.Text & "' , E_Invoice_ACK_Date = " & IIf(Trim(vEInvAckDate) <> "", "@EInvoiceAckDate", "Null") & " " &
                ",E_Invoice_Cancelled_Status = " & eiCancel.ToString & " ,  E_Invoice_Cancellation_Reason = '" & txt_EInvoiceCancellationReson.Text & "' , EWB_No = '" & txt_eWayBill_No.Text & "',EWB_Date = '" & txt_EWB_Date.Text & "',EWB_Valid_Upto = '" & txt_EWB_ValidUpto.Text & "' , EWB_Cancelled = " & EWBCancel.ToString & " , EWBCancellation_Reason = '" & txt_EWB_Canellation_Reason.Text & "'  Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " And Bobin_Sales_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If

            If Trim(Common_Procedures.settings.CustomerCode) = "1135" Then
                Partcls = "BobDelv : Dc.No. " & Trim(lbl_DcNo.Text) & ", No Of Reels : " & Val(vTotReel)
                PBlNo = Trim(lbl_DcNo.Text)
                EntID = Trim(Pk_Condition) & Trim(lbl_DcNo.Text)

            Else
                Partcls = "BobDelv : Dc.No. " & Trim(lbl_DcNo.Text)
                PBlNo = Trim(lbl_DcNo.Text)
                EntID = Trim(Pk_Condition) & Trim(lbl_DcNo.Text)

            End If

            cmd.CommandText = "Delete from Bobin_Sales_Bobin_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Bobin_Sales_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            With dgv_BobinDetails
                Sno = 0
                For i = 0 To .RowCount - 1

                    If Trim(.Rows(i).Cells(1).Value) <> "" And Val(.Rows(i).Cells(5).Value) <> 0 Then

                        Sno = Sno + 1

                        Clr_ID = Common_Procedures.Colour_NameToIdNo(con, .Rows(i).Cells(1).Value, tr)


                        Nr = 0
                        cmd.CommandText = "Update  Bobin_Sales_Bobin_Details set Bobin_Sales_Date = @EntryDate , Sl_No  = " & Str(Val(Sno)) & " , Ends =" & Val(.Rows(i).Cells(2).Value) & "  , Colour_IdNo = " & Str(Val(Clr_ID)) & "   , Bobins = " & Val(.Rows(i).Cells(3).Value) & " , Meter_Bobin = " & Val(.Rows(i).Cells(4).Value) & " , Meters = " & Val(.Rows(i).Cells(5).Value) & " , Meter_Reel = " & Val(.Rows(i).Cells(6).Value) & " , Reel = " & Val(.Rows(i).Cells(7).Value) & " , Weight=" & Str(Val(.Rows(i).Cells(9).Value)) & ",Rate = " & Val(.Rows(i).Cells(10).Value) & " , Amount = " & Val(.Rows(i).Cells(11).Value) & " , Bobin_Jari_Sales_Invoice_Code = '" & Trim(.Rows(i).Cells(12).Value) & "'  where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Bobin_Sales_Code = '" & Trim(NewCode) & "'  and Sl_No = " & Val(Sno) & " "
                        Nr = cmd.ExecuteNonQuery()

                        If Nr = 0 Then

                            cmd.CommandText = "Insert into Bobin_Sales_Bobin_Details ( Bobin_Sales_Code      ,           Company_IdNo           ,   Bobin_Sales_No              ,       for_OrderBy     , Bobin_Sales_Date  ,       Sl_No           ,        Ends                       ,           Colour_IdNo     ,           Bobins                  ,       Meter_Bobin                     ,                       Meters              ,                   Meter_Reel          ,               Reel                         ,              Weight                   ,               Rate                    ,                       Amount              ,           Bobin_Jari_Sales_Invoice_Code  )" &
                            "Values                                                  ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(vOrdByNo) & ", @EntryDate          ," & Str(Val(Sno)) & ",  " & Val(.Rows(i).Cells(2).Value) & ", " & Str(Val(Clr_ID)) & " ,  " & Val(.Rows(i).Cells(3).Value) & ", " & Val(.Rows(i).Cells(4).Value) & ", " & Str(Val(.Rows(i).Cells(5).Value)) & "," & Str(Val(.Rows(i).Cells(6).Value)) & "," & Str(Val(.Rows(i).Cells(7).Value)) & "," & Str(Val(.Rows(i).Cells(9).Value)) & ", " & Str(Val(.Rows(i).Cells(10).Value)) & "," & Str(Val(.Rows(i).Cells(11).Value)) & ",'" & Trim(.Rows(i).Cells(12).Value) & "' )"
                            cmd.ExecuteNonQuery()

                        End If


                    End If

                Next

            End With

            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition1) & Trim(NewCode), tr)

            Dim vLed_IdNos As String = "", vVou_Amts As String = "", ErrMsg As String = ""
            Dim AcPos_ID As Integer = 0


            AcPos_ID = Led_ID


            vLed_IdNos = AcPos_ID & "|" & SlAc_ID

            vVou_Amts = -1 * Val(CSng(lbl_Net_Amt.Text)) & "|" & (Val(CSng(lbl_Net_Amt.Text)))

            If Common_Procedures.Voucher_Updation(con, "GST-Bobin.Sale", Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), Trim(lbl_DcNo.Text), Convert.ToDateTime(dtp_Date.Text), "Dc No : " & Trim(lbl_DcNo.Text) & ", Mtrs : " & Trim(Format(Val(vTotMtrs), "#########0.00")) & ", No of Reels : " & Format(Val(vTotReel), "##########0.00"), vLed_IdNos, vVou_Amts, ErrMsg, tr, Common_Procedures.SoftwareTypes.OE_Software) = False Then
                Throw New ApplicationException(ErrMsg)
            End If

            'vLed_IdNos = Trans_ID & "|" & Val(Common_Procedures.CommonLedger.Transport_Charges_Ac)
            'vVou_Amts = Val(txt_Freight.Text) & "|" & -1 * Val(txt_Freight.Text)
            'If Common_Procedures.Voucher_Updation(con, "Bobin.Dc.Frgt", Val(lbl_Company.Tag), Trim(Pk_Condition1) & Trim(NewCode), Trim(lbl_DcNo.Text), Convert.ToDateTime(dtp_Date.Text), Partcls, vLed_IdNos, vVou_Amts, ErrMsg, tr,Common_Procedures.SoftwareTypes.OE_Software) = False Then
            '    Throw New ApplicationException(ErrMsg)
            'End If

            Dim VouBil As String = ""
            VouBil = Common_Procedures.VoucherBill_Posting(con, Val(lbl_Company.Tag), Convert.ToDateTime(dtp_Date.Text), AcPos_ID, Trim(lbl_DcNo.Text), 0, Val(CSng(lbl_Net_Amt.Text)), "DR", Trim(Pk_Condition) & Trim(NewCode), tr, Common_Procedures.SoftwareTypes.OE_Software)
            If Trim(UCase(VouBil)) = "ERROR" Then
                Throw New ApplicationException("Error on Voucher Bill Posting")
            End If

            cmd.CommandText = "Delete from Stock_Empty_BeamBagCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()


            'If Val(txt_OurBobin.Text) <> 0 Or Val(txt_PartyBobin.Text) <> 0 Then
            '    cmd.CommandText = "Insert into Stock_Empty_BeamBagCone_Processing_Details ( Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Entry_ID, Party_Bill_No, Particulars, Sl_No, Empty_Cones, Empty_Bobin, EmptyBobin_Party, Empty_Jumbo) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(vOrdByNo) & ", @EntryDate, " & Str(Val(Led_ID)) & ", 0, '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', 1, 0, " & Str(Val(txt_OurBobin.Text)) & ", " & Str(Val(txt_PartyBobin.Text)) & ", 0)"
            '    cmd.ExecuteNonQuery()
            'End If

            If Val(txt_OurBobin.Text) <> 0 Or Val(txt_PartyBobin.Text) <> 0 Or Val(vTotBbns) <> 0 Then
                cmd.CommandText = "Insert into Stock_Empty_BeamBagCone_Processing_Details ( Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Entry_ID, Party_Bill_No, Particulars, Sl_No, Empty_Cones, Empty_Bobin, EmptyBobin_Party, Empty_Jumbo) " &
                                    " Values                                             ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(vOrdByNo) & ", @EntryDate, " & Str(Val(Led_ID)) & ", 0, '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', 1, 0, " & Str(Val(txt_OurBobin.Text) + Val(vTotBbns)) & ", " & Str(Val(txt_PartyBobin.Text)) & ", 0)"
                Nr = cmd.ExecuteNonQuery()
            End If

            tr.Commit()

            Dt1.Dispose()
            Da.Dispose()


            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)

            If Val(Common_Procedures.settings.OnSave_MoveTo_NewEntry_Status) = 1 Then
                If New_Entry = True Then
                    new_record()
                Else
                    move_record(lbl_DcNo.Text)
                End If
            Else
                move_record(lbl_DcNo.Text)
            End If


        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If msk_Date.Visible And msk_Date.Enabled Then msk_Date.Focus()

    End Sub

    Private Sub Total_Calculation()
        Dim vTotBbnS As Single, vTotMtrs As Single, vTotReel As Single, vTotAmt As Single, vTotWgt As Single
        Dim i As Integer
        Dim sno As Integer
        Dim BlAmt As Double
        Dim AssAmt As Single = 0
        Dim CGSTAmt As Single = 0
        Dim SGSTAmt As Single = 0
        Dim IGSTAmt As Single = 0
        Dim Ledger_State_Code As String = ""
        Dim Company_State_Code As String = ""
        Dim Led_IdNo As Integer
        Dim vStrNetAmt As String = ""

        If NoCalc_Status = True Then Exit Sub


        Try

            vTotBbnS = 0 : vTotMtrs = 0 : vTotReel = 0 : vTotAmt = 0 : vTotWgt = 0

            With dgv_BobinDetails

                For i = 0 To .Rows.Count - 1

                    sno = sno + 1

                    .Rows(i).Cells(0).Value = sno

                    If Val(.Rows(i).Cells(5).Value) <> 0 Then

                        vTotBbnS = vTotBbnS + Val(.Rows(i).Cells(3).Value)
                        vTotMtrs = vTotMtrs + Val(.Rows(i).Cells(5).Value)
                        vTotReel = vTotReel + Val(.Rows(i).Cells(7).Value)
                        ' vTotWgt = vTotWgt + Val(.Rows(i).Cells(9).Value)
                        vTotAmt = vTotAmt + Val(.Rows(i).Cells(11).Value)

                    End If
                Next

            End With

            If dgv_BobinDetails_Total.Rows.Count <= 0 Then dgv_BobinDetails_Total.Rows.Add()
            dgv_BobinDetails_Total.Rows(0).Cells(3).Value = Val(vTotBbnS)
            dgv_BobinDetails_Total.Rows(0).Cells(5).Value = Format(Val(vTotMtrs), "#########0.00")

            If chk_Polister.Checked = False Then
                dgv_BobinDetails_Total.Rows(0).Cells(7).Value = Val(vTotReel)
            End If
            '  dgv_BobinDetails_Total.Rows(0).Cells(9).Value = Val(vTotWgt)
            dgv_BobinDetails_Total.Rows(0).Cells(11).Value = Format(Val(vTotAmt), "#########0.00")

            lbl_AssessableValue.Text = Format(Val(vTotAmt) + Val(txt_AddLess.Text) + Val(txt_Freight.Text), "#########0.00")

            AssAmt = Val(lbl_AssessableValue.Text)

            lbl_CGST_Amount.Text = 0
            lbl_SGST_Amount.Text = 0
            lbl_IGST_Amount.Text = 0

            If Trim(cbo_Type.Text) = "GST" Then

                Led_IdNo = Val(Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_IdNo", "Ledger_Name = '" & Trim(cbo_Ledger.Text) & "'"))
                Get_State_Code(Led_IdNo, Ledger_State_Code, Company_State_Code)

                'lbl_grid_GstPerc.Text = 0

                'lbl_Grid_HsnCode.Text = ""
                lbl_Grid_HSNCode.Text = Common_Procedures.get_FieldValue(con, "ItemGroup_Head", "Item_HSN_Code", "ItemGroup_Name = '" & Trim(cbo_ItemGroup.Text) & "'")

                lbl_grid_GstPerc.Text = Val(Common_Procedures.get_FieldValue(con, "ItemGroup_Head", "Item_GST_Percentage", "ItemGroup_Name = '" & Trim(cbo_ItemGroup.Text) & "'"))


                If Trim(Company_State_Code) = Trim(Ledger_State_Code) Then
                    '-CGST 
                    lbl_CGST_Amount.Text = Format(Val(lbl_AssessableValue.Text) * (Val(lbl_grid_GstPerc.Text) / 2) / 100, "#########0.00")
                    '-SGST 
                    lbl_SGST_Amount.Text = Format(Val(lbl_AssessableValue.Text) * (Val(lbl_grid_GstPerc.Text) / 2) / 100, "#########0.00")

                ElseIf Trim(Company_State_Code) <> Trim(Ledger_State_Code) Then
                    '-IGST 
                    lbl_IGST_Amount.Text = Format(Val(lbl_AssessableValue.Text) * Val(lbl_grid_GstPerc.Text) / 100, "#########0.00")

                End If

            End If

            BlAmt = Val(lbl_AssessableValue.Text) + Val(lbl_CGST_Amount.Text) + Val(lbl_SGST_Amount.Text) + Val(lbl_IGST_Amount.Text)

            ' lbl_Net_Amt.Text = Format(Val(BlAmt), "#########0.00")
            lbl_Net_Amt.Text = Format(Val(BlAmt), "#########0")


            vStrNetAmt = Format(Val(BlAmt), "##########0.00")

            lbl_Net_Amt.Text = Common_Procedures.Currency_Format(Val(CSng(lbl_Net_Amt.Text)))

            lbl_RoundOff.Text = Format(Val(CSng(lbl_Net_Amt.Text)) - Val(vStrNetAmt), "#########0.00")


            If Val(lbl_RoundOff.Text) = 0 Then lbl_RoundOff.Text = ""



        Catch ex As Exception
            '----
        End Try
    End Sub


    Private Sub Meters_Calculation()
        Dim i As Integer
        Dim sno As Integer
        Dim vtotMtrs As Single
        Dim vtotReel As Single
        Dim vtotAmt As Single
        Dim vMeterPrReel As Single, vWeightPrReel As Single
        Dim Led_Id As Integer = 0
        Dim Colour_Id As Integer = 0

        Try
            vtotMtrs = 0 : sno = 0 : vtotReel = 0 : vtotAmt = 0
            With dgv_BobinDetails

                For i = 0 To dgv_BobinDetails.Rows.Count - 1

                    If .CurrentRow.Cells(1).Value <> "" And .CurrentRow.Cells(3).Value <> "" Then
                        sno = sno + 1
                        .Rows(i).Cells(0).Value = sno

                        ' TOTAL METERS CALCULATION
                        vtotMtrs = Val(dgv_BobinDetails.Rows(i).Cells(3).Value) * Val(dgv_BobinDetails.Rows(i).Cells(4).Value)
                        dgv_BobinDetails.Rows(i).Cells(5).Value = Format(Val(vtotMtrs), "#########0.00")

                        ' GET METER/REEL & RATE/REEL FROM LEDGER TABLE
                        If chk_Polister.Checked = False Then
                            Led_Id = Common_Procedures.Ledger_NameToIdNo(con, cbo_Ledger.Text)
                            vMeterPrReel = Val(Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_MeterPrReel", "ledger_idno = " & Val(Led_Id) & ""))
                            vWeightPrReel = Val(Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_WeightPrReel", "ledger_idno = " & Val(Led_Id) & ""))

                            '  Colour_Id = Common_Procedures.Colour_NameToIdNo(con, Val(.Rows(i).Cells(1).Value))

                            dgv_BobinDetails.Rows(0).Cells(6).Value = Format(Val(vMeterPrReel), "#########0.00")
                            dgv_BobinDetails.Rows(0).Cells(8).Value = Format(Val(vWeightPrReel), "#########0.00")
                            'dgv_BobinDetails.Rows(0).Cells(10).Value = Format(Val(vRatePrReel), "#########0.00")
                        End If

                    End If

                Next
            End With
            Total_Calculation()

        Catch ex As Exception
            '-----
        End Try

    End Sub

    Private Sub cbo_Ledger_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) or Show_In_All_Entry = 1) ", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Ledger_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, msk_Date, cbo_SalesAcc, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) or Show_In_All_Entry = 1) ", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Ledger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ledger.KeyPress
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Ledger, cbo_SalesAcc, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) or Show_In_All_Entry = 1) ", "(Ledger_idno = 0)")
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
            '--------
        End Try

    End Sub

    Private Sub dgv_BobinDetails_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_BobinDetails.CellEndEdit
        dgv_BobinDetails_CellLeave(sender, e)
        'Try
        '    With dgv_BobinDetails

        '        If .CurrentCell.ColumnIndex = 3 Then
        '            If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
        '                .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
        '            End If
        '        End If

        '        'Meters_Calculation()
        '        'Total_Calculation()

        '    End With

        'Catch ex As Exception
        '    '-----
        'End Try
    End Sub

    Private Sub dgv_BobinDetails_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_BobinDetails.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim Dt4 As New DataTable
        Dim rect As Rectangle

        Try

            With dgv_BobinDetails

                dgv_ActCtrlName = .Name.ToString

                If Val(.CurrentRow.Cells(0).Value) = 0 Then
                    .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
                End If



                If e.ColumnIndex = 1 Then

                    If cbo_BobinColour.Visible = False Or Val(cbo_BobinColour.Tag) <> e.RowIndex Then

                        cbo_BobinColour.Tag = -1
                        Da = New SqlClient.SqlDataAdapter("select Colour_Name from Colour_Head order by Colour_Name", con)
                        Dt2 = New DataTable
                        Da.Fill(Dt2)
                        cbo_BobinColour.DataSource = Dt2
                        cbo_BobinColour.DisplayMember = "Colour_Name"

                        rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                        cbo_BobinColour.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                        cbo_BobinColour.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                        cbo_BobinColour.Width = rect.Width  ' .CurrentCell.Size.Width
                        cbo_BobinColour.Height = rect.Height  ' rect.Height

                        cbo_BobinColour.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                        cbo_BobinColour.Tag = Val(e.RowIndex)
                        cbo_BobinColour.Visible = True

                        cbo_BobinColour.BringToFront()
                        cbo_BobinColour.Focus()

                    End If

                Else

                    'cbo_Grid_MillName.Tag = -1
                    'cbo_Grid_MillName.Text = ""
                    cbo_BobinColour.Visible = False

                End If


            End With

        Catch ex As Exception
            '-----

        End Try

    End Sub

    Private Sub dgv_BobinDetails_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_BobinDetails.CellLeave

        Try
            With dgv_BobinDetails
                If .Visible = True Then

                    If .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 6 Then
                        If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                            .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                        End If
                    End If

                End If

            End With

        Catch ex As Exception
            '-----
        End Try

    End Sub

    Private Sub dgv_BobinDetails_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_BobinDetails.CellValueChanged

        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Ends_Idno As Integer = 0
        Dim Tot_Meters As Single
        Dim Ends As Single
        Dim MeterPrReel As Single
        Dim NoOfReels As Single

        Try
            If IsNothing(dgv_BobinDetails.CurrentCell) Then Exit Sub
            With dgv_BobinDetails

                If .Visible Then

                    If .Rows.Count > 0 Then

                        If .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 2 Then
                            Meters_Calculation()
                        End If

                        If .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 6 Then

                            If Val(.CurrentRow.Cells(5).Value) <> 0 Then

                                If chk_Polister.Checked = False Then
                                    Tot_Meters = Val(.Rows(.CurrentCell.RowIndex).Cells(5).Value)
                                    Ends = Val(.Rows(.CurrentCell.RowIndex).Cells(2).Value)
                                    MeterPrReel = Val(.Rows(.CurrentCell.RowIndex).Cells(6).Value)

                                    'NO OF REEL CALCULATION
                                    NoOfReels = Tot_Meters * Ends / MeterPrReel
                                    .Rows(.CurrentCell.RowIndex).Cells(7).Value = Format(Val(NoOfReels), "#########0")
                                    'WEIGHT CALCULATION
                                    'Weight = NoOfReels * Val(.Rows(0).Cells(8).Value)
                                    '.Rows(.CurrentCell.RowIndex).Cells(9).Value = Format(Val(Weight), "#########0")
                                Else
                                    Tot_Meters = Val(.Rows(.CurrentCell.RowIndex).Cells(4).Value)
                                    Ends = Val(.Rows(.CurrentCell.RowIndex).Cells(2).Value)
                                    MeterPrReel = Val(.Rows(.CurrentCell.RowIndex).Cells(6).Value)

                                    'NO OF REEL CALCULATION
                                    NoOfReels = Tot_Meters * Ends * (MeterPrReel / 100) * (10 / 100)

                                    .Rows(.CurrentCell.RowIndex).Cells(10).Value = Format(Val(NoOfReels), "#########0.00")

                                    'WEIGHT CALCULATION
                                    ' Weight = NoOfReels * Val(.Rows(0).Cells(8).Value)
                                    ' .Rows(.CurrentCell.RowIndex).Cells(9).Value = Format(Val(Weight), "#########0")
                                End If

                            End If

                            Total_Calculation()

                            If chk_Polister.Checked = False Then
                                .Rows(.CurrentCell.RowIndex).Cells(11).Value = Format(Val(.Rows(.CurrentCell.RowIndex).Cells(7).Value) * Val(.Rows(.CurrentCell.RowIndex).Cells(10).Value), "#########0.00")
                            Else
                                .Rows(.CurrentCell.RowIndex).Cells(11).Value = Format(Val(.Rows(.CurrentCell.RowIndex).Cells(3).Value) * Val(.Rows(.CurrentCell.RowIndex).Cells(10).Value), "#########0.00")
                            End If

                        End If

                    End If

                End If

            End With

        Catch ex As Exception
            '-----
        End Try
    End Sub

    Private Sub dgv_BobinDetails_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_BobinDetails.EditingControlShowing
        Try
            dgtxt_BobinDetails = Nothing
            If dgv_BobinDetails.CurrentCell.ColumnIndex > 2 Then
                dgtxt_BobinDetails = CType(dgv_BobinDetails.EditingControl, DataGridViewTextBoxEditingControl)
            End If

        Catch ex As Exception
            '----
        End Try
    End Sub

    Private Sub dgtxt_BobinDetails_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_BobinDetails.Enter
        Try
            dgv_ActCtrlName = dgv_BobinDetails.Name
            dgv_BobinDetails.EditingControl.BackColor = Color.PaleGreen
            dgv_BobinDetails.EditingControl.ForeColor = Color.Blue
            dgtxt_BobinDetails.SelectAll()
        Catch ex As Exception
            '----
        End Try
    End Sub

    Private Sub dgtxt_BobinDetails_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_BobinDetails.KeyPress

        Try
            With dgv_BobinDetails
                If .Visible Then

                    If Val(dgv_BobinDetails.CurrentCell.ColumnIndex.ToString) = 3 Or Val(dgv_BobinDetails.CurrentCell.ColumnIndex.ToString) = 4 Or Val(dgv_BobinDetails.CurrentCell.ColumnIndex.ToString) = 5 Or Val(dgv_BobinDetails.CurrentCell.ColumnIndex.ToString) = 8 Or Val(dgv_BobinDetails.CurrentCell.ColumnIndex.ToString) = 7 Then

                        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                            e.Handled = True
                        End If
                    End If

                End If

            End With

        Catch ex As Exception
            '-----

        End Try

    End Sub

    Private Sub dgv_BobinDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_BobinDetails.KeyUp
        Dim n As Integer = 0

        Try
            If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

                With dgv_BobinDetails

                    n = .CurrentRow.Index

                    If .Rows.Count = 1 Then
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

                Total_Calculation()

            End If

        Catch ex As Exception
            '------
        End Try

    End Sub

    Private Sub dgv_Details_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_BobinDetails.RowsAdded
        Dim n As Integer = 0

        Try
            With dgv_BobinDetails
                n = .RowCount
                .Rows(n - 1).Cells(0).Value = Val(n)
            End With

        Catch ex As Exception
            '-----

        End Try
    End Sub

    Private Sub dgv_BobinDetails_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_BobinDetails.LostFocus
        On Error Resume Next
        dgv_BobinDetails.CurrentCell.Selected = False
    End Sub

    Private Sub cbo_Colour_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_BobinColour.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Colour_Head", "Colour_Name", "", "(Colour_IdNo = 0)")
    End Sub

    Private Sub cbo_BobinColour_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_BobinColour.KeyDown
        Dim dep_idno As Integer = 0

        Try
            vcbo_KeyDwnVal = e.KeyValue

            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_BobinColour, Nothing, Nothing, "Colour_Head", "Colour_Name", "", "(Colour_IdNo = 0)")
            With dgv_BobinDetails

                If (e.KeyValue = 38 And cbo_BobinColour.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                    If .CurrentCell.RowIndex = 0 Then
                        cbo_VechileNo.Focus()
                    Else
                        .Focus()
                        .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
                    End If
                End If

                If (e.KeyValue = 40 And cbo_BobinColour.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                    If .CurrentCell.RowIndex = .Rows.Count - 1 And Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(1).Value) = "" Then
                        txt_Freight.Focus()
                    Else
                        .Focus()
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

                    End If
                End If

            End With

        Catch ex As Exception
            '-----

        End Try

    End Sub

    Private Sub cbo_BobinColour_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_BobinColour.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim vmeterPrReel As Single = 0
        Dim vRatePrReel As Single = 0
        Dim Led_Id As Integer = 0
        Dim Colour_Id As Integer = 0

        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_BobinColour, Nothing, "Colour_Head", "Colour_Name", "", "(Colour_IdNo = 0)")

            If Asc(e.KeyChar) = 13 Then
                With dgv_BobinDetails
                    With dgv_BobinDetails

                        .Rows(.CurrentCell.RowIndex).Cells.Item(1).Value = Trim(cbo_BobinColour.Text)
                        If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then
                            txt_Freight.Focus()
                        Else
                            .Focus()
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                        End If

                        Led_Id = Common_Procedures.Ledger_NameToIdNo(con, Trim(cbo_Ledger.Text))
                        Colour_Id = Common_Procedures.Colour_NameToIdNo(con, Trim(dgv_BobinDetails.Rows(.CurrentCell.RowIndex).Cells(1).Value))

                        vRatePrReel = Val(Common_Procedures.get_FieldValue(con, "Ledger_Rate_Details", "RATE", "ledger_idno = " & Val(Led_Id) & " and Colour_IdNo=" & Val(Colour_Id) & ""))
                        vmeterPrReel = Val(Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_MeterPrReel", "ledger_idno = " & Val(Led_Id) & ""))
                        If chk_Polister.Checked = True Then
                            .Rows(.CurrentCell.RowIndex).Cells(6).Value = vRatePrReel
                        Else
                            .Rows(.CurrentCell.RowIndex).Cells(6).Value = vmeterPrReel
                            .Rows(.CurrentCell.RowIndex).Cells(10).Value = vRatePrReel
                        End If

                    End With
                End With
            End If

        Catch ex As Exception
            '-----
        End Try

    End Sub

    Private Sub cbo_BorderName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_BobinColour.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Color_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_BobinColour.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_BobinColour_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_BobinColour.SelectedIndexChanged
        Dim vmeterPrReel As Single = 0
        Dim vRatePrReel As Single = 0
        Dim Led_Id As Integer = 0
        Dim Colour_Id As Integer = 0


        Try
            With dgv_BobinDetails

                If dgv_BobinDetails.Visible Then
                    If cbo_BobinColour.Visible = True Then

                        Led_Id = Common_Procedures.Ledger_NameToIdNo(con, Trim(cbo_Ledger.Text))
                        Colour_Id = Common_Procedures.Colour_NameToIdNo(con, Trim(dgv_BobinDetails.Rows(.CurrentCell.RowIndex).Cells(1).Value))

                        vRatePrReel = Val(Common_Procedures.get_FieldValue(con, "Ledger_Rate_Details", "RATE", "ledger_idno = " & Val(Led_Id) & " and Colour_IdNo=" & Val(Colour_Id) & ""))
                        vmeterPrReel = Val(Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_MeterPrReel", "ledger_idno = " & Val(Led_Id) & ""))
                        If chk_Polister.Checked = True Then
                            .Rows(.CurrentCell.RowIndex).Cells(6).Value = vRatePrReel
                        Else
                            .Rows(.CurrentCell.RowIndex).Cells(6).Value = vmeterPrReel
                            .Rows(.CurrentCell.RowIndex).Cells(10).Value = vRatePrReel
                        End If

                    End If

                End If
            End With

        Catch ex As Exception

        End Try



        Total_Calculation()
    End Sub

    Private Sub cbo_BorderName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_BobinColour.TextChanged
        Dim vmeterPrReel As Single = 0
        Dim vRatePrReel As Single = 0
        Dim Led_Id As Integer = 0
        Dim Colour_Id As Integer = 0

        Try
            If cbo_BobinColour.Visible Then
                With dgv_BobinDetails
                    If .Rows.Count > 0 Then
                        If Val(cbo_BobinColour.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 2 Then
                            .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_BobinColour.Text)
                        End If
                    End If

                    Led_Id = Common_Procedures.Ledger_NameToIdNo(con, Trim(cbo_Ledger.Text))
                    Colour_Id = Common_Procedures.Colour_NameToIdNo(con, Trim(dgv_BobinDetails.Rows(.CurrentCell.RowIndex).Cells(1).Value))

                    vRatePrReel = Val(Common_Procedures.get_FieldValue(con, "Ledger_Rate_Details", "RATE", "ledger_idno = " & Val(Led_Id) & " and Colour_IdNo=" & Val(Colour_Id) & ""))
                    vmeterPrReel = Val(Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_MeterPrReel", "ledger_idno = " & Val(Led_Id) & ""))
                    If chk_Polister.Checked = True Then
                        .Rows(.CurrentCell.RowIndex).Cells(6).Value = vRatePrReel
                    Else
                        .Rows(.CurrentCell.RowIndex).Cells(6).Value = vmeterPrReel
                        .Rows(.CurrentCell.RowIndex).Cells(10).Value = vRatePrReel
                    End If

                End With
            End If

            Total_Calculation()

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Vechile_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_VechileNo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_VechileNo, Nothing, Nothing, "Bobin_Sales_Head", "Vechile_No", "", "")
        Try
            If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
            If e.KeyValue = 40 Then
                If dgv_BobinDetails.Rows.Count > 0 Then
                    dgv_BobinDetails.Focus()
                    dgv_BobinDetails.CurrentCell = dgv_BobinDetails.Rows(0).Cells(1)
                    dgv_BobinDetails.CurrentCell.Selected = True

                Else
                    txt_PartyBobin.Focus()

                End If
            End If

        Catch ex As Exception
            '---
        End Try
    End Sub

    Private Sub cbo_Vechile_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_VechileNo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_VechileNo, Nothing, "Bobin_Sales_Head", "Vechile_No", "", "", False)
        Try
            If Asc(e.KeyChar) = 13 Then
                If dgv_BobinDetails.Rows.Count > 0 Then
                    dgv_BobinDetails.Focus()
                    dgv_BobinDetails.CurrentCell = dgv_BobinDetails.Rows(0).Cells(1)
                    dgv_BobinDetails.CurrentCell.Selected = True

                Else
                    txt_PartyBobin.Focus()

                End If
            End If

        Catch ex As Exception
            '--
        End Try
    End Sub

    Private Sub cbo_Transport_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Transport.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Transport, cbo_ItemGroup, cbo_VechileNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
        'If e.KeyValue = 40 And cbo_Transport.DroppedDown = False Then
        '    If dgv_BobinDetails.Rows.Count > 0 Then
        '        dgv_BobinDetails.Focus()
        '        dgv_BobinDetails.CurrentCell = dgv_BobinDetails.Rows(0).Cells(1)
        '        dgv_BobinDetails.CurrentCell.Selected = True

        '    Else
        '        txt_PartyBobin.Focus()

        '    End If
        'End If
    End Sub

    Private Sub cbo_Transport_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Transport.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Transport, cbo_VechileNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
        'If Asc(e.KeyChar) = 13 And cbo_Transport.DroppedDown = False Then
        '    If dgv_BobinDetails.Rows.Count > 0 Then
        '        dgv_BobinDetails.Focus()
        '        dgv_BobinDetails.CurrentCell = dgv_BobinDetails.Rows(0).Cells(1)
        '        dgv_BobinDetails.CurrentCell.Selected = True

        '    Else
        '        txt_PartyBobin.Focus()

        '    End If
        'End If
    End Sub

    Private Sub cbo_Transport_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Transport.KeyUp
        If e.Control = False And e.KeyValue = 17 Then

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

    Private Sub btn_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub btn_Filter_Show_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter_Show.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim n As Integer, i As Integer
        Dim Led_IdNo As Integer, proc_IdNo As Integer
        Dim Condt As String = ""


        Try

            Condt = ""
            Led_IdNo = 0
            proc_IdNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Bobin_Sales_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Bobin_Sales_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Bobin_Sales_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If

            If Trim(cbo_Filter_EndsName.Text) <> "" Then
                proc_IdNo = Common_Procedures.Process_NameToIdNo(con, cbo_Filter_EndsName.Text)
            End If

            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.Ledger_IdNo = " & Str(Val(Led_IdNo))
            End If




            If Trim(cbo_Filter_EndsName.Text) <> "" Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Bobin_Sales_Head IN (select z1.Bobin_Sales_Head from Bobin_Sales_Bobin_Details z1 where z1.Ends = '" & Trim(cbo_Filter_EndsName.Text) & "')"
            End If

            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name from Bobin_Sales_Head a INNER JOIN Ledger_Head b on a.Ledger_IdNo = b.Ledger_IdNo  where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Bobin_Sales_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Bobin_Sales_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()


                    dgv_Filter_Details.Rows(n).Cells(0).Value = dt2.Rows(i).Item("Bobin_Sales_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(1).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Bobin_Sales_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(3).Value = Val(dt2.Rows(i).Item("Total_Bobins").ToString)
                    dgv_Filter_Details.Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("Total_Meters").ToString), "########0.00")


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
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, cbo_Filter_EndsName, btn_Filter_Show, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) )", "(Ledger_idno = 0)")

    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, btn_Filter_Show, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) ", "(Ledger_idno = 0)")
    End Sub


    Private Sub cbo_Filter_EndsName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_EndsName.KeyDown

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_EndsName, dtp_Filter_ToDate, cbo_Filter_PartyName, "EndsCount_Head", "EndsCount_name", "", "(endsCount_iDNO = 0)")

    End Sub

    Private Sub cbo_Filter_ProcessName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_EndsName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_EndsName, cbo_Filter_PartyName, "endsCount_Head", "EndsCount_name", "", "(EndsCount_iDNO = 0)")
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


    Private Sub btn_Print_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        print_record()
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String
        Dim ps As Printing.PaperSize
        Dim PpSzSTS As Boolean = False


        prn_InpOpts = ""
        prn_InpOpts = InputBox("Select    -   0. None" & Chr(13) & "                  1. Original" & Chr(13) & "                  2. Duplicate" & Chr(13) & "                  3. Triplicate" & Chr(13) & "                  4. HO Copy           5. All", "FOR INVOICE PRINTING...", "1234")
        prn_InpOpts = Replace(Trim(prn_InpOpts), "5", "1234")


        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select * from Bobin_Sales_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Bobin_Sales_Code = '" & Trim(NewCode) & "'", con)
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
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dtbl1 As New DataTable
        Dim nr As Integer = 0
        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0
        prn_Count = 0

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.*, d.Ledger_Name as TransportName,lsh.State_Code as Ledger_State_Code,Lsh.State_Name as Ledger_State_Name, Csh.State_Name as Company_State_Name, csh.State_Code as Company_State_Code, f.Ledger_Name as DeliveryTo_LedgerName, f.Ledger_Address1 as DeliveryTo_LedgerAddress1, f.Ledger_Address2 as DeliveryTo_LedgerAddress2, f.Ledger_Address3 as DeliveryTo_LedgerAddress3, f.Ledger_Address4 as DeliveryTo_LedgerAddress4, f.Ledger_GSTinNo as DeliveryTo_LedgerGSTinNo, f.Ledger_pHONENo as DeliveryTo_LedgerPhoneNo, f.Pan_No as DeliveryTo_PanNo, Dsh.State_Name as DeliveryTo_State_Name, Dsh.State_Code as DeliveryTo_State_Code  from Bobin_Sales_Head a " &
                                               "INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo " &
                                               "INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo " &
                                                "INNER JOIN State_Head lsh ON c.Ledger_State_IdNo = lsh.State_IdNo " &
                                                "INNER JOIN State_Head csh ON b.Company_State_IdNo = csh.State_IdNo " &
                                               " LEFT OUTER JOIN Ledger_Head f ON (case when a.DeliveryTo_IdNo <> 0 then a.DeliveryTo_IdNo else a.Ledger_IdNo end) = f.Ledger_IdNo LEFT OUTER JOIN State_Head Dsh ON f.Ledger_State_IdNo = Dsh.State_IdNo" &
                                               " Left outer JOIN Ledger_Head d ON a.Transport_IdNo = d.Ledger_IdNo  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Bobin_Sales_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("select a.* , c.Colour_name from Bobin_Sales_Bobin_Details a  LEFT OUTER JOIN Colour_Head c ON a.Colour_idno = c.Colour_idno  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Bobin_Sales_Code = '" & Trim(NewCode) & "' Order by a.Sl_No", con)
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

        If chk_Polister.Checked = True Then
            Printing_Delivery_Format_GST1(e)



        Else
            Printing_Delivery_Format_GST2(e)
        End If

    End Sub

    'Private Sub Printing_Delivery_Format_GST2(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
    '    Dim p2Font As Font
    '    'Dim ps As Printing.PaperSize
    '    Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
    '    Dim PrintWidth As Single, PrintHeight As Single
    '    Dim PageWidth As Single, PageHeight As Single
    '    Dim I As Integer
    '    Dim NoofItems_PerPage As Integer, NoofDets As Integer
    '    Dim TxtHgt As Single
    '    Dim PpSzSTS As Boolean = False
    '    Dim LnAr(15) As Single, ClAr(15) As Single
    '    Dim EntryCode As String
    '    Dim CurY As Single
    '    Dim ps As Printing.PaperSize
    '    Dim ItmNm1 As String, ItmNm2 As String
    '    Dim SNo As Integer
    '    Dim vLine_Pen As Pen

    '    For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
    '        If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
    '            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
    '            PrintDocument1.DefaultPageSettings.PaperSize = ps
    '            PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
    '            Exit For
    '        End If
    '    Next

    '    'Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8X6", 800, 600)
    '    'PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
    '    'PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1

    '    With PrintDocument1.DefaultPageSettings.Margins
    '        .Left = 30 ' 30
    '        .Right = 50
    '        .Top = 20 ' 30
    '        .Bottom = 30
    '        LMargin = .Left
    '        RMargin = .Right
    '        TMargin = .Top
    '        BMargin = .Bottom
    '    End With

    '    p2Font = New Font("Calibri", 11, FontStyle.Regular)

    '    e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

    '    With PrintDocument1.DefaultPageSettings.PaperSize
    '        PrintWidth = .Width - RMargin - LMargin
    '        PrintHeight = .Height - TMargin - BMargin
    '        PageWidth = .Width - RMargin
    '        PageHeight = .Height - BMargin
    '    End With

    '    NoofItems_PerPage = 15   ' 7

    '    Erase LnAr
    '    Erase ClAr

    '    LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
    '    ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}


    '    ClAr(1) = 35        'SNO
    '    ClAr(2) = 100       'PARTICULARS

    '    ClAr(3) = 145        'ENDS
    '    ClAr(4) = 55       'Bobins
    '    ClAr(5) = 75       'Meter_Bobin
    '    ClAr(6) = 0       'Meters

    '    ClAr(7) = 77        'HSN CODE
    '    ClAr(8) = 43        'GST %

    '    ClAr(9) = 60       'REEL
    '    ClAr(10) = 55     'RATE
    '    ClAr(11) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10)) 'AMOUNT

    '    TxtHgt = 17.75 ' 18

    '    EntryCode = Trim((Pk_Condition)) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

    '    vLine_Pen = New Pen(Color.Black, 2)

    '    Try
    '        If prn_HdDt.Rows.Count > 0 Then

    '            Printing_Delivery_Format_GST2_PageHeader(e, EntryCode, TxtHgt, p2Font, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr, vLine_Pen)

    '            NoofDets = 0

    '            CurY = CurY - 10

    '            If prn_DetDt.Rows.Count > 0 Then

    '                Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

    '                    If NoofDets >= NoofItems_PerPage Then

    '                        CurY = CurY + TxtHgt

    '                        Common_Procedures.Print_To_PrintDocument(e, "Continued...", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, p2Font)

    '                        NoofDets = NoofDets + 1

    '                        Printing_Delivery_Format_GST2_PageFooter(e, EntryCode, TxtHgt, p2Font, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, False)

    '                        NoofDets = 0
    '                        e.HasMorePages = True

    '                        Return

    '                    End If

    '                    ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Colour_name").ToString)
    '                    ItmNm2 = ""
    '                    If Len(ItmNm1) > 25 Then
    '                        For I = 15 To 1 Step -1
    '                            If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
    '                        Next I
    '                        If I = 0 Then I = 25
    '                        ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
    '                        ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
    '                    End If

    '                    CurY = CurY + TxtHgt
    '                    SNo = SNo + 1
    '                    Common_Procedures.Print_To_PrintDocument(e, Trim(Val(SNo)), LMargin + 10, CurY, 0, 0, p2Font)
    '                    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClAr(1) + 10, CurY, 0, 0, p2Font)
    '                    'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("HSN_Code").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 2, CurY, 1, 0,  p2Font)
    '                    'Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("GST_Percentage").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 5, CurY, 1, 0,  p2Font)

    '                    'Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("EndscOUNT_Name").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 5, CurY, 0, 0,  p2Font)
    '                    'Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Bobins").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 5, CurY, 1, 0,  p2Font)
    '                    'Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Meter_Bobin").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 5, CurY, 1, 0,  p2Font)
    '                    '' Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Meters").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 5, CurY, 1, 0,  p2Font)

    '                    '----------
    '                    Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Ends").ToString, LMargin + ClAr(1) + ClAr(2) + 5, CurY, 0, 0, p2Font)
    '                    Common_Procedures.Print_To_PrintDocument(e, (Val(prn_DetDt.Rows(prn_DetIndx).Item("Bobins").ToString)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 5, CurY, 1, 0, p2Font)

    '                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Meter_Bobin").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 5, CurY, 1, 0, p2Font)

    '                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("HSN_Code").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 5, CurY, 1, 0, p2Font)
    '                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("GST_Percentage").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 3, CurY, 1, 0, p2Font)

    '                    '----------
    '                    p2Font = New Font("Calibri", 11, FontStyle.Bold)
    '                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("REEL").ToString), "########0"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 5, CurY, 1, 0, p2Font)
    '                    p2Font = New Font("Calibri", 11, FontStyle.Regular)

    '                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Rate").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) - 5, CurY, 1, 0, p2Font)
    '                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) - 5, CurY, 1, 0, p2Font)


    '                    NoofDets = NoofDets + 1

    '                    If Trim(ItmNm2) <> "" Then
    '                        CurY = CurY + TxtHgt - 5
    '                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClAr(1) + 10, CurY, 0, 0, p2Font)
    '                        NoofDets = NoofDets + 1
    '                    End If

    '                    prn_DetIndx = prn_DetIndx + 1

    '                Loop

    '            End If

    '            Printing_Delivery_Format_GST2_PageFooter(e, EntryCode, TxtHgt, p2Font, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True)

    '            If Trim(prn_InpOpts) <> "" Then
    '                If prn_Count < Len(Trim(prn_InpOpts)) Then

    '                    If Val(prn_InpOpts) <> "0" Then
    '                        prn_DetIndx = 0
    '                        prn_DetSNo = 0
    '                        prn_PageNo = 0

    '                        e.HasMorePages = True
    '                        Return
    '                    End If

    '                End If

    '            End If
    '        End If

    '    Catch ex As Exception
    '        MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

    '    End Try

    '    e.HasMorePages = False

    'End Sub

    'Private Sub Printing_Delivery_Format_GST2_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal p2Font As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal vLine_Pen As Pen)
    '    Dim CurX As Single = 0, CurY1 As Single = 0, CurY2 As Single = 0
    '    Dim da2 As New SqlClient.SqlDataAdapter
    '    Dim dt2 As New DataTable
    '    Dim p1Font As Font
    '    Dim strHeight As Single = 0, strWidth As Single = 0
    '    Dim C1 As Single, W1, W2, W3 As Single, S1, S2, S3 As Single
    '    Dim Cmp_Name, Desc As String, Cmp_Add1 As String, Cmp_Add2 As String, Cmp_PanNo As String, Cmp_PanCap As String
    '    Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_EMail As String
    '    Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
    '    Dim S As String
    '    Dim Inv_No As String = ""
    '    Dim InvSubNo As String = ""

    '    PageNo = PageNo + 1
    '    prn_Count = prn_Count + 1

    '    prn_OriDupTri = ""
    '    If Trim(prn_InpOpts) <> "" Then
    '        If prn_Count <= Len(Trim(prn_InpOpts)) Then

    '            S = Mid$(Trim(prn_InpOpts), prn_Count, 1)

    '            If Val(S) = 1 Then
    '                prn_OriDupTri = "ORIGINAL FOR RECEIPIENT"
    '            ElseIf Val(S) = 2 Then
    '                prn_OriDupTri = "DUPLICATE FOR TRANSPORTER"
    '            ElseIf Val(S) = 3 Then
    '                prn_OriDupTri = "TRIPLICATE FOR SUPPLIER"
    '            ElseIf Val(S) = 4 Then
    '                prn_OriDupTri = "EXTRA COPY"
    '            Else
    '                If Trim(prn_InpOpts) <> "0" And Val(prn_InpOpts) = "0" And Len(Trim(prn_InpOpts)) > 3 Then
    '                    prn_OriDupTri = Trim(prn_InpOpts)
    '                End If
    '            End If

    '        End If
    '    End If



    '    CurY = TMargin + 2

    '    p1Font = New Font("Calibri", 14, FontStyle.Bold)
    '    Common_Procedures.Print_To_PrintDocument(e, "TAX INVOICE", LMargin, CurY - TxtHgt - 5, 2, PrintWidth, p1Font)

    '    '  End If
    '    If Trim(prn_OriDupTri) <> "" Then
    '        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), PageWidth - 10, CurY - TxtHgt, 1, 0, p2Font)
    '    End If

    '    e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
    '    'e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
    '    LnAr(1) = CurY
    '    Desc = ""
    '    Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
    '    Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_PanNo = "" : Cmp_EMail = "" : Cmp_PanCap = ""
    '    Cmp_StateCap = "" : Cmp_StateNm = "" : Cmp_StateCode = "" : Cmp_GSTIN_Cap = "" : Cmp_GSTIN_No = ""

    '    Desc = prn_HdDt.Rows(0).Item("Company_Description").ToString
    '    Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
    '    Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
    '    Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString

    '    If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
    '        Cmp_PhNo = "Phone : " & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
    '    End If
    '    If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
    '        Cmp_TinNo = "TIN NO : " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
    '    End If
    '    If Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString) <> "" Then
    '        Cmp_CstNo = "CST NO : " & prn_HdDt.Rows(0).Item("Company_CstNo").ToString
    '    End If
    '    If Trim(prn_HdDt.Rows(0).Item("Company_PanNo").ToString) <> "" Then
    '        Cmp_PanCap = "PAN : "
    '        Cmp_PanNo = prn_HdDt.Rows(0).Item("Company_PanNo").ToString
    '    End If
    '    If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
    '        Cmp_EMail = "Email : " & prn_HdDt.Rows(0).Item("Company_EMail").ToString
    '    End If
    '    If Trim(prn_HdDt.Rows(0).Item("Company_State_Name").ToString) <> "" Then
    '        Cmp_StateCap = "STATE : "
    '        Cmp_StateNm = prn_HdDt.Rows(0).Item("Company_State_Name").ToString
    '        If Trim(prn_HdDt.Rows(0).Item("Company_State_Code").ToString) <> "" Then
    '            Cmp_StateNm = Cmp_StateNm & "   CODE : " & prn_HdDt.Rows(0).Item("Company_State_Code").ToString
    '        End If
    '    End If
    '    If Trim(prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString) <> "" Then
    '        Cmp_GSTIN_Cap = "GSTIN : "
    '        Cmp_GSTIN_No = prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
    '    End If

    '    'p1Font = New Font("Calibri", 14, FontStyle.Bold)
    '    'If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1018" Then '---- M.K Textiles (Palladam)
    '    '    p1Font = New Font("Calibri", 12, FontStyle.Bold)
    '    '    Common_Procedures.Print_To_PrintDocument(e, "INVOICE", LMargin, CurY, 2, PrintWidth, p1Font)
    '    'End If

    '    CurY = CurY + TxtHgt - 15
    '    strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


    '    p1Font = New Font("Calibri", 18, FontStyle.Bold)
    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
    '    strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


    '    CurY = CurY + strHeight - 7
    '    If Desc <> "" Then
    '        Common_Procedures.Print_To_PrintDocument(e, Desc, LMargin, CurY, 2, PrintWidth, p2Font)
    '    End If

    '    strWidth = e.Graphics.MeasureString(Trim(Cmp_Add1 & " " & Cmp_Add2), p1Font).Width
    '    If PrintWidth > strWidth Then
    '        If Trim(Cmp_Add1 & " " & Cmp_Add2) <> "" Then
    '            CurY = CurY + TxtHgt - 1
    '            Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_Add1 & " " & Cmp_Add2), LMargin, CurY, 2, PrintWidth, p2Font)
    '        End If

    '        NoofItems_PerPage = NoofItems_PerPage - 1

    '    Else

    '        If Cmp_Add1 <> "" Then
    '            CurY = CurY + TxtHgt - 1
    '            Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, p2Font)
    '        End If
    '        If Cmp_Add2 <> "" Then
    '            CurY = CurY + TxtHgt - 1
    '            Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, p2Font)
    '        End If

    '    End If


    '    CurY = CurY + TxtHgt
    '    Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PhNo & " / " & Cmp_EMail), LMargin, CurY, 2, PrintWidth, p2Font)

    '    CurY = CurY + TxtHgt

    '    p1Font = New Font("Calibri", 11, FontStyle.Bold)
    '    strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_GSTIN_Cap), p1Font).Width
    '    strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_StateNm & "     " & Cmp_GSTIN_Cap & Cmp_GSTIN_No & "     " & Cmp_PanCap & Cmp_PanNo), p2Font).Width
    '    If PrintWidth > strWidth Then
    '        CurX = LMargin + (PrintWidth - strWidth) / 2
    '    Else
    '        CurX = LMargin
    '    End If

    '    p1Font = New Font("Calibri", 10, FontStyle.Bold)
    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_StateCap, CurX, CurY, 0, 0, p1Font)
    '    strWidth = e.Graphics.MeasureString(Cmp_StateCap, p1Font).Width
    '    CurX = CurX + strWidth
    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_StateNm, CurX, CurY, 0, 0, p2Font)

    '    strWidth = e.Graphics.MeasureString(Cmp_StateNm, p2Font).Width
    '    p1Font = New Font("Calibri", 10, FontStyle.Bold)
    '    CurX = CurX + strWidth
    '    Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_GSTIN_Cap, CurX, CurY, 0, PrintWidth, p1Font)
    '    strWidth = e.Graphics.MeasureString("     " & Cmp_GSTIN_Cap, p1Font).Width
    '    CurX = CurX + strWidth
    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, CurX, CurY, 0, 0, p2Font)

    '    If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1044" Then
    '        strWidth = e.Graphics.MeasureString(Cmp_GSTIN_No, p2Font).Width
    '        p1Font = New Font("Calibri", 10, FontStyle.Bold)
    '        CurX = CurX + strWidth
    '        Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_PanCap, CurX, CurY, 0, PrintWidth, p1Font)
    '        strWidth = e.Graphics.MeasureString("     " & Cmp_PanCap, p1Font).Width
    '        CurX = CurX + strWidth
    '        Common_Procedures.Print_To_PrintDocument(e, Cmp_PanNo, CurX, CurY, 0, 0, p2Font)
    '    End If


    '    CurY = CurY + TxtHgt
    '    e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
    '    LnAr(2) = CurY


    '    C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 30
    '    W1 = e.Graphics.MeasureString("DATE & TIME OF   SUPPLY ", p2Font).Width
    '    S1 = e.Graphics.MeasureString("TO", p2Font).Width  ' e.Graphics.MeasureString("Details of Receiver | Billed to     :",  p2Font).Width

    '    W2 = e.Graphics.MeasureString("DESPATCH   TO   : ", p2Font).Width
    '    S2 = e.Graphics.MeasureString("TRANSPORTATION   MODE", p2Font).Width

    '    W3 = e.Graphics.MeasureString("INVOICE   DATE", p2Font).Width
    '    S3 = e.Graphics.MeasureString("REVERSE CHARGE   (YES/NO) ", p2Font).Width

    '    CurY = CurY + 10
    '    p1Font = New Font("Calibri", 12, FontStyle.Bold)
    '    Common_Procedures.Print_To_PrintDocument(e, "INVOICE NO", LMargin + 10, CurY, 0, 0, p2Font)
    '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W3 + 10, CurY, 0, 0, p2Font)


    '    If prn_HdDt.Rows(0).Item("Invoice_PrefixNo").ToString <> "" Then
    '        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Invoice_PrefixNo").ToString & "-" & prn_HdDt.Rows(0).Item("Bobin_Sales_No").ToString, LMargin + W3 + 30, CurY, 0, 0, p1Font)
    '    Else
    '        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Bobin_Sales_No").ToString, LMargin + W3 + 30, CurY, 0, 0, p1Font)
    '    End If


    '    Common_Procedures.Print_To_PrintDocument(e, "REVERSE CHARGE (YES/NO)", LMargin + C1 + 10, CurY, 0, 0, p2Font)
    '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S3 + 10, CurY, 0, 0, p2Font)
    '    Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin + C1 + S3 + 30, CurY, 0, 0, p2Font)


    '    CurY = CurY + TxtHgt
    '    Common_Procedures.Print_To_PrintDocument(e, "INVOICE DATE", LMargin + 10, CurY, 0, 0, p2Font)
    '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W3 + 10, CurY, 0, 0, p2Font)
    '    p2Font = New Font("Calibri", 11, FontStyle.Regular)
    '    Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Bobin_Sales_Date").ToString), "dd-MM-yyyy").ToString, LMargin + W3 + 30, CurY, 0, 0, p1Font)

    '    'If Trim(prn_HdDt.Rows(0).Item("Electronic_Reference_No").ToString) <> "" Then
    '    '    Common_Procedures.Print_To_PrintDocument(e, "ELECTRONIC REF.NO ", LMargin + C1 + 10, CurY, 0, 0,  p2Font)
    '    '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S3 + 10, CurY, 0, 0,  p2Font)
    '    '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Electronic_Reference_No").ToString, LMargin + C1 + S3 + 30, CurY, 0, 0,  p2Font)
    '    'End If

    '    CurY = CurY + TxtHgt
    '    e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
    '    'LnAr(2) = CurY

    '    CurY1 = CurY
    '    CurY2 = CurY

    '    '---left side

    '    CurY1 = CurY1 + 10
    '    p1Font = New Font("Calibri", 10, FontStyle.Bold)
    '    Common_Procedures.Print_To_PrintDocument(e, "DETAILS OF RECEIVER (BILLED TO) :", LMargin + 10, CurY1, 0, 0, p1Font)

    '    strHeight = e.Graphics.MeasureString("A", p1Font).Height
    '    CurY1 = CurY1 + strHeight
    '    p1Font = New Font("Calibri", 11, FontStyle.Bold)
    '    Common_Procedures.Print_To_PrintDocument(e, "M/s. " & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + S1 + 10, CurY1, 0, 0, p1Font)

    '    CurY1 = CurY1 + TxtHgt
    '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY1, 0, 0, p2Font)

    '    CurY1 = CurY1 + TxtHgt
    '    If Trim(prn_HdDt.Rows(0).Item("Ledger_Address2").ToString) <> "" Then

    '        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY1, 0, 0, p2Font)
    '    End If
    '    CurY1 = CurY1 + TxtHgt
    '    If Trim(prn_HdDt.Rows(0).Item("Ledger_Address3").ToString) <> "" Then

    '        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY1, 0, 0, p2Font)
    '    End If

    '    If Trim(prn_HdDt.Rows(0).Item("Ledger_Address4").ToString) <> "" Then
    '        CurY1 = CurY1 + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY1, 0, 0, p2Font)
    '    End If
    '    CurY1 = CurY1 + TxtHgt
    '    If Trim(prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString) <> "" Then
    '        '  CurY1 = CurY1 + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString & "  CODE : " & prn_HdDt.Rows(0).Item("Ledger_State_Code").ToString, LMargin + S1 + 10, CurY1, 0, 0, p2Font)
    '    End If

    '    CurY1 = CurY1 + TxtHgt
    '    If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
    '        Common_Procedures.Print_To_PrintDocument(e, "GSTIN : " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + S1 + 10, CurY1, 0, 0, p2Font)
    '    End If
    '    If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1044" Then
    '        If Trim(prn_HdDt.Rows(0).Item("Pan_No").ToString) <> "" Then
    '            strWidth = e.Graphics.MeasureString("GSTIN : " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, p2Font).Width
    '            CurX = LMargin + S1 + 10 + strWidth
    '            Common_Procedures.Print_To_PrintDocument(e, "      PAN : " & prn_HdDt.Rows(0).Item("Pan_No").ToString, CurX, CurY1, 0, PrintWidth, p2Font)
    '        End If
    '    End If



    '    '--Right Side

    '    CurY2 = CurY2 + 10
    '    p1Font = New Font("Calibri", 10, FontStyle.Bold)
    '    Common_Procedures.Print_To_PrintDocument(e, "DETAILS OF CONSIGNEE (SHIPPED TO) :", LMargin + C1 + 10, CurY2, 0, 0, p1Font)

    '    strHeight = e.Graphics.MeasureString("A", p1Font).Height
    '    CurY2 = CurY2 + strHeight
    '    p1Font = New Font("Calibri", 11, FontStyle.Bold)
    '    Common_Procedures.Print_To_PrintDocument(e, "M/s. " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerName").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, p1Font)

    '    p2Font = New Font("Calibri", 10, FontStyle.Regular)
    '    CurY2 = CurY2 + TxtHgt
    '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress1").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, p2Font)
    '    CurY2 = CurY2 + TxtHgt
    '    If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress2").ToString) <> "" Then
    '        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress2").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, p2Font)
    '    End If
    '    CurY2 = CurY2 + TxtHgt
    '    If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress3").ToString) <> "" Then
    '        ' CurY2 = CurY2 + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress3").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, p2Font)
    '    End If

    '    If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress4").ToString) <> "" Then
    '        CurY2 = CurY2 + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress4").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, p2Font)
    '    End If
    '    CurY2 = CurY2 + TxtHgt
    '    If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_State_Name").ToString) <> "" Then
    '        ' CurY2 = CurY2 + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_State_Name").ToString & "  CODE : " & prn_HdDt.Rows(0).Item("DeliveryTo_State_Code").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, p2Font)
    '    End If

    '    CurY2 = CurY2 + TxtHgt
    '    If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerGSTinNo").ToString) <> "" Then
    '        Common_Procedures.Print_To_PrintDocument(e, "GSTIN : " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerGSTinNo").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, p2Font)
    '    End If
    '    If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1044" Then
    '        If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_PanNo").ToString) <> "" Then
    '            strWidth = e.Graphics.MeasureString("GSTIN : " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerGSTinNo").ToString, p2Font).Width
    '            CurX = LMargin + C1 + S1 + 10 + strWidth
    '            Common_Procedures.Print_To_PrintDocument(e, "      PAN : " & prn_HdDt.Rows(0).Item("DeliveryTo_PanNo").ToString, CurX, CurY2, 0, PrintWidth, p2Font)
    '        End If
    '    End If




    '    CurY = IIf(CurY1 > CurY2, CurY1, CurY2)


    '    CurY = CurY + TxtHgt

    '    e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
    '    e.Graphics.DrawLine(vLine_Pen, LMargin + C1, CurY, LMargin + C1, LnAr(2))
    '    LnAr(3) = CurY



    '    W2 = e.Graphics.MeasureString("DOCUMENT THROUGH   : ", p2Font).Width
    '    S2 = e.Graphics.MeasureString("DATE & TIME OF SUPPLY  :", p2Font).Width

    '    '--Right Side
    '    CurY = CurY + 10
    '    Common_Procedures.Print_To_PrintDocument(e, "TRANSPORT ", LMargin + 10, CurY, 0, 0, p2Font)
    '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 10, CurY, 0, 0, p2Font)
    '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("TransportName").ToString, LMargin + W2 + 30, CurY, 0, 0, p2Font)


    '    Common_Procedures.Print_To_PrintDocument(e, "VEHICLE NO", LMargin + C1 + 10, CurY, 0, 0, p2Font)
    '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S2 + 10, CurY, 0, 0, p2Font)
    '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vechile_No").ToString, LMargin + C1 + S2 + 30, CurY, 0, 0, p2Font)


    '    'CurY = CurY + TxtHgt
    '    'Common_Procedures.Print_To_PrintDocument(e, "ORDER NO", LMargin + 10, CurY, 0, 0,  p2Font)
    '    'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 10, CurY, 0, 0,  p2Font)
    '    'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Party_OrderNo").ToString, LMargin + W2 + 30, CurY, 0, 0,  p2Font)
    '    'If Trim(prn_HdDt.Rows(0).Item("Party_OrderDate").ToString) <> "" Then
    '    '    strWidth = e.Graphics.MeasureString(prn_HdDt.Rows(0).Item("Party_OrderNo").ToString,  p2Font).Width
    '    '    Common_Procedures.Print_To_PrintDocument(e, "Date : " & prn_HdDt.Rows(0).Item("Party_OrderDate").ToString, LMargin + W2 + strWidth + 60, CurY, 0, 0,  p2Font)
    '    'End If



    '    'CurY = CurY + TxtHgt
    '    'Common_Procedures.Print_To_PrintDocument(e, "DC NO", LMargin + 10, CurY, 0, 0,  p2Font)
    '    'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 10, CurY, 0, 0,  p2Font)
    '    'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Dc_No").ToString, LMargin + W2 + 30, CurY, 0, 0,  p2Font)
    '    'If Trim(prn_HdDt.Rows(0).Item("Dc_Date").ToString) <> "" Then
    '    '    strWidth = e.Graphics.MeasureString(prn_HdDt.Rows(0).Item("Dc_No").ToString,  p2Font).Width
    '    '    Common_Procedures.Print_To_PrintDocument(e, "Date : " & prn_HdDt.Rows(0).Item("Dc_Date").ToString, LMargin + strWidth + W1 + 60, CurY, 0, 0,  p2Font)
    '    'End If

    '    'Common_Procedures.Print_To_PrintDocument(e, "TRANSPORT NAME", LMargin + C1 + 10, CurY, 0, 0,  p2Font)
    '    'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S2 + 10, CurY, 0, 0,  p2Font)
    '    'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("TransportName").ToString, LMargin + C1 + S2 + 30, CurY, 0, 0,  p2Font)


    '    '' CurY = CurY + TxtHgt
    '    'Common_Procedures.Print_To_PrintDocument(e, "TRANSPORTATION MODE", LMargin + C1 + 10, CurY, 0, 0,  p2Font)
    '    'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S2 + 10, CurY, 0, 0,  p2Font)
    '    'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Transport_Mode").ToString, LMargin + C1 + S2 + 30, CurY, 0, 0,  p2Font)
    '    ''Common_Procedures.Print_To_PrintDocument(e, "TRANSPORTATION MODE", LMargin + 10, CurY, 0, 0,  p2Font)
    '    ''Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 10, CurY, 0, 0,  p2Font)
    '    ''Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Transport_Mode").ToString, LMargin + W2 + 30, CurY, 0, 0,  p2Font)


    '    ''Common_Procedures.Print_To_PrintDocument(e, "DATE & TIME OF SUPPLY", LMargin + C1 + 10, CurY, 0, 0,  p2Font)
    '    ''Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S2 + 10, CurY, 0, 0,  p2Font)
    '    ''Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Date_And_Time_Of_Supply").ToString, LMargin + C1 + S2 + 30, CurY, 0, 0,  p2Font)

    '    'CurY = CurY + TxtHgt
    '    'Common_Procedures.Print_To_PrintDocument(e, "DOCUMENT THROUGH", LMargin + 10, CurY, 0, 0,  p2Font)
    '    'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 10, CurY, 0, 0,  p2Font)
    '    'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Through_Name").ToString, LMargin + W2 + 30, CurY, 0, 0,  p2Font)

    '    'Common_Procedures.Print_To_PrintDocument(e, "PLACE OF SUPPLY", LMargin + C1 + 10, CurY, 0, 0,  p2Font)
    '    'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S2 + 10, CurY, 0, 0,  p2Font)
    '    'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_State_Name").ToString, LMargin + C1 + S2 + 30, CurY, 0, 0,  p2Font)


    '    'If Trim(prn_HdDt.Rows(0).Item("Lc_No").ToString) <> "" Then
    '    '    CurY = CurY + TxtHgt
    '    '    Common_Procedures.Print_To_PrintDocument(e, "LC NO", LMargin + 10, CurY, 0, 0,  p2Font)
    '    '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 10, CurY, 0, 0,  p2Font)
    '    '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Lc_No").ToString, LMargin + W2 + 30, CurY, 0, 0,  p2Font)
    '    '    If Trim(prn_HdDt.Rows(0).Item("Lc_Date").ToString) <> "" Then
    '    '        strWidth = e.Graphics.MeasureString(prn_HdDt.Rows(0).Item("Lc_No").ToString,  p2Font).Width
    '    '        Common_Procedures.Print_To_PrintDocument(e, "Date : " & prn_HdDt.Rows(0).Item("Lc_Date").ToString, LMargin + strWidth + W2 + 60, CurY, 0, 0,  p2Font)
    '    '    End If
    '    'End If

    '    CurY = CurY + TxtHgt + 10
    '    e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '    LnAr(3) = CurY

    '    e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))

    '    CurY = CurY + TxtHgt - 10
    '    Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), p2Font)

    '    Common_Procedures.Print_To_PrintDocument(e, "PARTICULARS", LMargin + ClAr(1), CurY, 2, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), p2Font)

    '    e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY + TxtHgt, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY + TxtHgt)
    '    LnAr(10) = CurY
    '    CurY = CurY + TxtHgt + 10
    '    Common_Procedures.Print_To_PrintDocument(e, "COLOUR", LMargin + ClAr(1), CurY, 2, ClAr(2), p2Font)

    '    'Common_Procedures.Print_To_PrintDocument(e, "HSN CODE", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3),  p2Font)

    '    'Common_Procedures.Print_To_PrintDocument(e, "GST %", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4),  p2Font)

    '    'Common_Procedures.Print_To_PrintDocument(e, "ENDS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5),  p2Font)

    '    'Common_Procedures.Print_To_PrintDocument(e, "BOBINS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6),  p2Font)

    '    'Common_Procedures.Print_To_PrintDocument(e, "METER/", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7),  p2Font)
    '    'Common_Procedures.Print_To_PrintDocument(e, "BOBINS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY + TxtHgt, 2, ClAr(7),  p2Font)

    '    '---------
    '    Common_Procedures.Print_To_PrintDocument(e, "ENDS ", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), p2Font)

    '    Common_Procedures.Print_To_PrintDocument(e, "BOBINS ", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), p2Font)

    '    Common_Procedures.Print_To_PrintDocument(e, "METER/", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), p2Font)
    '    Common_Procedures.Print_To_PrintDocument(e, "BOBINS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY + TxtHgt, 2, ClAr(5), p2Font)

    '    CurY = CurY - 20

    '    Common_Procedures.Print_To_PrintDocument(e, "HSN CODE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), p2Font)

    '    Common_Procedures.Print_To_PrintDocument(e, "GST %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), p2Font)
    '    '---------

    '    Common_Procedures.Print_To_PrintDocument(e, "TOTAL/", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), p2Font)
    '    Common_Procedures.Print_To_PrintDocument(e, "REEL", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY + TxtHgt, 2, ClAr(9), p2Font)

    '    Common_Procedures.Print_To_PrintDocument(e, "RATE/", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, 2, ClAr(10), p2Font)
    '    Common_Procedures.Print_To_PrintDocument(e, "REEL", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY + TxtHgt, 2, ClAr(10), p2Font)

    '    Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, 2, ClAr(11), p2Font)

    '    CurY = CurY + TxtHgt + TxtHgt + 20
    '    e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '    LnAr(4) = CurY

    'End Sub

    'Private Sub Printing_Delivery_Format_GST2_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal p2Font As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
    '    Dim i As Integer
    '    Dim Cmp_Name As String
    '    Dim W1 As Single = 0
    '    Dim C1 As Single = 0
    '    Dim ItmNm1 As String = ""
    '    Dim s2 As Single = 0
    '    Dim vprn_BlNos As String = ""
    '    Dim SubClAr(15) As Single
    '    Dim p1Font As Font, p3Font As Font
    '    Dim rndoff As Double, TtAmt As Double
    '    Dim BInc As Integer
    '    Dim BnkDetAr() As String
    '    Dim BmsInWrds As String
    '    Dim BankNm1 As String = ""
    '    Dim BankNm2 As String = ""
    '    Dim BankNm3 As String = ""
    '    Dim BankNm4 As String = ""
    '    Dim CurY1 As Single = 0
    '    Dim vNoofHsnCodes As Integer = 0
    '    Dim vTaxPerc As Single = 0

    '    For i = NoofDets + 1 To NoofItems_PerPage
    '        CurY = CurY + TxtHgt
    '    Next

    '    C1 = ClAr(1) + ClAr(2) + ClAr(3) - 30
    '    W1 = e.Graphics.MeasureString("TOTAL BOBIN : ", p2Font).Width
    '    'w2 = e.Graphics.MeasureString("DESP.TO : ",  p2Font).Width
    '    'S1 = e.Graphics.MeasureString("TO  :  ",  p2Font).Width
    '    s2 = e.Graphics.MeasureString("TOTAL BOBIN :  ", p2Font).Width

    '    CurY = CurY + TxtHgt
    '    e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '    LnAr(5) = CurY

    '    e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
    '    e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(10) + 20)
    '    e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(10) + 20)
    '    e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(10) + 20)
    '    e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(10) + 20)
    '    e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
    '    e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))
    '    e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(3))
    '    e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(3))
    '    e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), LnAr(3))


    '    Erase BnkDetAr
    '    If Trim(prn_HdDt.Rows(0).Item("Company_Bank_Ac_Details").ToString) <> "" Then
    '        BnkDetAr = Split(Trim(prn_HdDt.Rows(0).Item("Company_Bank_Ac_Details").ToString), ",")

    '        BInc = -1

    '        BInc = BInc + 1
    '        If UBound(BnkDetAr) >= BInc Then
    '            BankNm1 = Trim(BnkDetAr(BInc))
    '        End If

    '        BInc = BInc + 1
    '        If UBound(BnkDetAr) >= BInc Then
    '            BankNm2 = Trim(BnkDetAr(BInc))
    '        End If

    '        BInc = BInc + 1
    '        If UBound(BnkDetAr) >= BInc Then
    '            BankNm3 = Trim(BnkDetAr(BInc))
    '        End If

    '        BInc = BInc + 1
    '        If UBound(BnkDetAr) >= BInc Then
    '            BankNm4 = Trim(BnkDetAr(BInc))
    '        End If

    '    End If

    '    Balance_Calculation()

    '    CurY = CurY + TxtHgt - 10
    '    Common_Procedures.Print_To_PrintDocument(e, "TOTAL BOBIN ", LMargin + 10, CurY, 0, 0, p2Font)
    '    Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + s2, CurY, 0, 0, p2Font)
    '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Total_Bobins").ToString, LMargin + s2 + 10, CurY, 0, 0, p2Font)

    '    'Common_Procedures.Print_To_PrintDocument(e, "TOTAL METER ", LMargin + ClAr(1) + ClAr(2) + 20, CurY, 0, 0, p2Font)
    '    'Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + s2 + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, p2Font)
    '    'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Total_Meters").ToString, LMargin + s2 + ClAr(1) + ClAr(2) + 20, CurY, 0, 0, p2Font)

    '    Common_Procedures.Print_To_PrintDocument(e, "BALANCE BOBIN", LMargin + ClAr(1) + ClAr(2) + 30, CurY, 0, 0, p2Font)
    '    Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + s2 + ClAr(1) + ClAr(2) + 40, CurY, 0, 0, p2Font)
    '    Common_Procedures.Print_To_PrintDocument(e, Balance_Bobin, LMargin + s2 + ClAr(1) + ClAr(2) + 50, CurY, 0, 0, p2Font)



    '    Common_Procedures.Print_To_PrintDocument(e, "TOTAL REEL ", LMargin + C1 + ClAr(4) + 20, CurY, 0, 0, p2Font)
    '    Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + s2 + C1 + ClAr(4) + 5, CurY, 0, 0, p2Font)
    '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Total_Reels").ToString, LMargin + s2 + C1 + +ClAr(4) + 15, CurY, 0, 0, p2Font)

    '    CurY1 = CurY + 5
    '    CurY1 = CurY1 + TxtHgt
    '    e.Graphics.DrawLine(Pens.Black, LMargin, CurY1, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY1)
    '    CurY1 = CurY1 - 15
    '    p3Font = New Font("Calibri", 10, FontStyle.Regular)
    '    If BankNm1 <> "" Then
    '        CurY1 = CurY1 + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, BankNm1, LMargin + 10, CurY1, 0, 0, p3Font)
    '    End If
    '    If BankNm2 <> "" Then
    '        CurY1 = CurY1 + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, BankNm2, LMargin + 10, CurY1, 0, 0, p3Font)
    '    End If
    '    If BankNm3 <> "" Then
    '        CurY1 = CurY1 + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, BankNm3, LMargin + 10, CurY1, 0, 0, p3Font)
    '    End If
    '    If BankNm4 <> "" Then
    '        CurY1 = CurY1 + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, BankNm4, LMargin + 10, CurY1, 0, 0, p3Font)
    '    End If
    '    CurY1 = CurY1 + TxtHgt + 8
    '    e.Graphics.DrawLine(Pens.Black, LMargin, CurY1, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY1)

    '    Common_Procedures.Print_To_PrintDocument(e, "TOTAL AMOUNT :   ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 2, CurY, 0, 0, p2Font)
    '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Total_Amount").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) - 10, CurY, 1, 0, p2Font)


    '    'CurY = CurY + TxtHgt + 10
    '    If is_LastPage = True Then

    '        If Val(prn_HdDt.Rows(0).Item("Freight").ToString) <> 0 Then
    '            CurY = CurY + TxtHgt
    '            Common_Procedures.Print_To_PrintDocument(e, "Freight", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 2, CurY, 0, 0, p2Font)
    '            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Freight").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, p2Font)
    '        End If
    '    End If

    '    If is_LastPage = True Then

    '        If Val(prn_HdDt.Rows(0).Item("AddLess_BeforeTax").ToString) <> 0 Then
    '            CurY = CurY + TxtHgt
    '            Common_Procedures.Print_To_PrintDocument(e, "Add/Less", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 2, CurY, 0, 0, p2Font)
    '            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("AddLess_BeforeTax").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, p2Font)
    '        End If
    '    End If
    '    'CurY = CurY + TxtHgt

    '    CurY = CurY + TxtHgt - 10
    '    '-------------------------------------------------------------------

    '    prn_CGST_Amount = prn_HdDt.Rows(0).Item("Total_CGst_Amount").ToString
    '    prn_SGST_Amount = prn_HdDt.Rows(0).Item("Total_SGst_Amount").ToString
    '    prn_IGST_Amount = prn_HdDt.Rows(0).Item("Total_IGST_Amount").ToString

    '    prn_GST_Perc = Val(prn_HdDt.Rows(0).Item("GST_Percentage").ToString)


    '    If Val(prn_CGST_Amount) <> 0 Or Val(prn_SGST_Amount) <> 0 Or Val(prn_IGST_Amount) <> 0 Then

    '        If Val(prn_HdDt.Rows(0).Item("Freight").ToString) <> 0 Then
    '            CurY = CurY + TxtHgt
    '            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, PageWidth, CurY)
    '        Else
    '            CurY = CurY + 10
    '        End If

    '        CurY = CurY + TxtHgt - 10
    '        If is_LastPage = True Then
    '            p1Font = New Font("Calibri", 10, FontStyle.Bold)
    '            Common_Procedures.Print_To_PrintDocument(e, "Taxable Value", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 2, CurY, 0, 0, p2Font)
    '            '  Common_Procedures.Print_To_PrintDocument(e, "Taxable Value", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, 1, 0,  p2Font)
    '            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Taxable_Value").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, p1Font)
    '        End If
    '    End If
    '    CurY = CurY + TxtHgt
    '    If Val(prn_CGST_Amount) <> 0 Then

    '        If is_LastPage = True Then
    '            Common_Procedures.Print_To_PrintDocument(e, "CGST @ " & Trim(Val(prn_GST_Perc / 2)) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 2, CurY, 0, 0, p2Font)

    '        Else
    '            Common_Procedures.Print_To_PrintDocument(e, "CGST ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 2, CurY, 0, 0, p2Font)
    '        End If
    '        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_CGST_Amount), "########0.00"), PageWidth - 10, CurY, 1, 0, p2Font)

    '    End If
    '    CurY = CurY + TxtHgt
    '    If Val(prn_SGST_Amount) <> 0 Then

    '        If is_LastPage = True Then
    '            Common_Procedures.Print_To_PrintDocument(e, "SGST @ " & Trim(Val(prn_GST_Perc / 2)) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 2, CurY, 0, 0, p2Font)

    '        Else
    '            Common_Procedures.Print_To_PrintDocument(e, "SGST ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 2, CurY, 0, 0, p2Font)
    '        End If
    '        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_SGST_Amount), "########0.00"), PageWidth - 10, CurY, 1, 0, p2Font)

    '    End If

    '    CurY = CurY + TxtHgt
    '    If Val(prn_IGST_Amount) <> 0 Then

    '        If is_LastPage = True Then
    '            Common_Procedures.Print_To_PrintDocument(e, "IGST @ " & Trim(Val(prn_GST_Perc)) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 2, CurY, 0, 0, p2Font)
    '            ' Common_Procedures.Print_To_PrintDocument(e, "IGST @ " & Trim(Val(prn_GST_Perc)) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, 1, 0,  p2Font)
    '        Else
    '            Common_Procedures.Print_To_PrintDocument(e, "IGST ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 2, CurY, 0, 0, p2Font)
    '        End If
    '        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_IGST_Amount), "########0.00"), PageWidth - 10, CurY, 1, 0, p2Font)

    '    End If

    '    '***** GST END *****
    '    TtAmt = Format(Val(prn_HdDt.Rows(0).Item("Total_Taxable_Value").ToString) + Val(prn_IGST_Amount) + Val(prn_SGST_Amount) + Val(prn_CGST_Amount), "#########0.00")

    '    rndoff = 0
    '    rndoff = Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString) - Val(TtAmt)

    '    CurY = CurY + TxtHgt
    '    If Val(rndoff) <> 0 Then
    '        If Val(rndoff) >= 0 Then
    '            Common_Procedures.Print_To_PrintDocument(e, "ROUND OFF (+) ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 0, 0, p2Font)

    '            '  Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 20, CurY, 0, 0,  p2Font)
    '        Else
    '            Common_Procedures.Print_To_PrintDocument(e, "ROUND OFF (-)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 0, 0, p2Font)

    '            'Common_Procedures.Print_To_PrintDocument(e, "(-)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 20, CurY, 0, 0,  p2Font)
    '        End If
    '        Common_Procedures.Print_To_PrintDocument(e, " " & Format(Math.Abs(Val(rndoff)), "########0.00"), PageWidth - 10, CurY, 1, 0, p2Font)
    '    End If

    '    p1Font = New Font("Calibri", 13, FontStyle.Bold)
    '    CurY = CurY + TxtHgt
    '    p3Font = New Font("Calibri", 12, FontStyle.Regular)
    '    Common_Procedures.Print_To_PrintDocument(e, "E & OE", LMargin + 10, CurY, 0, 0, p3Font)

    '    CurY = CurY + TxtHgt + 8
    '    e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '    LnAr(6) = CurY

    '    CurY = CurY + TxtHgt - 10
    '    'Common_Procedures.Print_To_PrintDocument(e, "BALANCE BOBIN ", LMargin + 10, CurY, 0, 0,  p2Font)
    '    'Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + s2 + 30, CurY, 0, 0,  p2Font)
    '    'Common_Procedures.Print_To_PrintDocument(e, Balance_Bobin, LMargin + s2 + 60, CurY, 0, 0,  p2Font)
    '    Common_Procedures.Print_To_PrintDocument(e, "NET AMOUNT ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 2, CurY, 0, 0, p1Font)
    '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Net_Amount").ToString, PageWidth - 10, CurY, 1, 0, p1Font)

    '    'Common_Procedures.Print_To_PrintDocument(e, "BALANCE AMOUNT ", LMargin + C1, CurY, 0, 0,  p2Font)
    '    'Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + s2 + C1 + 30, CurY, 0, 0,  p2Font)
    '    'Common_Procedures.Print_To_PrintDocument(e, Balance_Amount, LMargin + s2 + C1 + 70, CurY, 0, 0,  p2Font)

    '    '  CurY = CurY + TxtHgt

    '    CurY = CurY + TxtHgt + 10
    '    e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '    LnAr(9) = CurY
    '    e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(4))

    '    CurY = CurY + 5
    '    BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))
    '    BmsInWrds = Replace(Trim(BmsInWrds), "", "")

    '    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1065" Then '---- Logu textile
    '        BmsInWrds = Trim(UCase(BmsInWrds))
    '    End If

    '    p1Font = New Font("Calibri", 10, FontStyle.Bold)
    '    Common_Procedures.Print_To_PrintDocument(e, "Amount Chargeable(In Words)  : " & BmsInWrds & " ", LMargin + 10, CurY, 0, 0, p1Font)
    '    CurY = CurY + TxtHgt + 5
    '    e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '    LnAr(10) = CurY

    '    '=============GST SUMMARY============

    '    '  vNoofHsnCodes = get_GST_Noof_HSN_Codes_For_Printing(EntryCode)

    '    ''    ' Printing_GST_HSN_Details_Format1(e, EntryCode, TxtHgt, p2Font, LMargin, PageWidth, PrintWidth, CurY, LnAr(10), Pens.Black)



    '    '==========================

    '    CurY = CurY + TxtHgt - 15
    '    p1Font = New Font("Calibri", 9, FontStyle.Underline Or FontStyle.Bold)
    '    Common_Procedures.Print_To_PrintDocument(e, "Term & Conditions : ", LMargin + 10, CurY, 0, 0, p1Font)

    '    CurY = CurY + TxtHgt

    '    p2Font = New Font("Webdings", 8, FontStyle.Regular)
    '    p1Font = New Font("Calibri", 8, FontStyle.Bold)


    '    ''1
    '    'If Val(prn_HdDt.Rows(0).Item("Gr_Time").ToString) <> 0 Then
    '    '    Common_Procedures.Print_To_PrintDocument(e, "=", LMargin + 10, CurY, 0, 0, p2Font)
    '    '    Common_Procedures.Print_To_PrintDocument(e, "Overdue Interest will be charged at 24% from The  " & Trim(prn_HdDt.Rows(0).Item("Gr_Date").ToString), LMargin + 25, CurY, 0, 0, p1Font)
    '    'Else
    '    Common_Procedures.Print_To_PrintDocument(e, "=", LMargin + 10, CurY, 0, 0, p2Font)
    '    Common_Procedures.Print_To_PrintDocument(e, "Overdue Interest will be charged at 24% from The invoice date. ", LMargin + 25, CurY, 0, 0, p1Font)
    '    'End If
    '    '3
    '    Common_Procedures.Print_To_PrintDocument(e, "=", PrintWidth / 2 + 10, CurY, 0, 0, p2Font)
    '    Common_Procedures.Print_To_PrintDocument(e, "We will not accept any claim after processing of goods.", PrintWidth / 2 + 25, CurY, 0, 0, p1Font)

    '    '2
    '    CurY = CurY + TxtHgt
    '    Common_Procedures.Print_To_PrintDocument(e, "=", LMargin + 10, CurY, 0, 0, p2Font)
    '    Common_Procedures.Print_To_PrintDocument(e, "We are not responsible for any loss or damage in transit.", LMargin + 25, CurY, 0, 0, p1Font)
    '    '4
    '    Common_Procedures.Print_To_PrintDocument(e, "=", PrintWidth / 2 + 10, CurY, 0, 0, p2Font)
    '    Common_Procedures.Print_To_PrintDocument(e, "Subject to " & Trim(Common_Procedures.settings.Jurisdiction) & " jurisdiction. ", PrintWidth / 2 + 25, CurY, 0, 0, p1Font)


    '    CurY = CurY + TxtHgt
    '    e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '    LnAr(10) = CurY

    '    'If Val(Common_Procedures.User.IdNo) <> 1 Then
    '    '    Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 20, CurY + 40, 0, 0,  p2Font)
    '    'End If

    '    CurY = CurY + 5
    '    Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
    '    p1Font = New Font("Calibri", 7, FontStyle.Bold)
    '    Common_Procedures.Print_To_PrintDocument(e, "Certified that the Particulars given above are true and correct", PageWidth - 10, CurY, 1, 0, p1Font)
    '    CurY = CurY + TxtHgt - 5
    '    p1Font = New Font("Calibri", 12, FontStyle.Bold)
    '    p3Font = New Font("Calibri", 12, FontStyle.Regular)
    '    Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 10, CurY, 1, 0, p1Font)
    '    CurY = CurY + TxtHgt
    '    CurY = CurY + TxtHgt
    '    CurY = CurY + TxtHgt
    '    '   CurY = CurY + TxtHgt
    '    Common_Procedures.Print_To_PrintDocument(e, "PREPARED BY", LMargin + 20, CurY, 0, 0, p3Font)
    '    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1135" Then
    '        Common_Procedures.Print_To_PrintDocument(e, "RECEIVER SIGNATURE", LMargin + 250, CurY, 0, 0, p3Font)
    '    Else
    '        Common_Procedures.Print_To_PrintDocument(e, "CHECKED BY", LMargin + 250, CurY, 0, 0, p3Font)
    '    End If
    '    p1Font = New Font("Calibri", 12, FontStyle.Bold)

    '    Common_Procedures.Print_To_PrintDocument(e, "AUTHORISED SIGNATORY ", PageWidth - 10, CurY, 1, 0, p3Font)
    '    CurY = CurY + TxtHgt + 10

    '    e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '    e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
    '    e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)


    '    'Catch ex As Exception

    '    '    MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

    '    'End Try

    'End Sub
    Private Sub Printing_GST_HSN_Details_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Integer, ByVal PageWidth As Integer, ByVal PrintWidth As Double, ByRef CurY As Single, ByRef TopLnYAxis As Single, ByVal vLine_Pen As Pen)
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim I As Integer = 0
        Dim p1Font As Font
        Dim SubClAr(15) As Single
        Dim ItmNm1 As String = "", ItmNm2 As String = ""
        Dim SNo As Integer = 0
        Dim Ttl_TaxAmt As Double, Ttl_CGst As Double, Ttl_Sgst As Double, Ttl_igst As Double
        Dim LnAr2 As Single
        Dim BmsInWrds As String = ""

        Try

            Ttl_TaxAmt = 0 : Ttl_CGst = 0 : Ttl_Sgst = 0

            Erase SubClAr

            SubClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

            SubClAr(1) = 110 : SubClAr(2) = 130 : SubClAr(3) = 48 : SubClAr(4) = 90 : SubClAr(5) = 48 : SubClAr(6) = 90 : SubClAr(7) = 48 : SubClAr(8) = 90
            SubClAr(9) = PageWidth - (LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8))

            CurY = CurY + 5
            pFont = New Font("Calibri", 9, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "HSN CODE", LMargin, CurY + 15, 2, SubClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "TAXABLE AMOUNT", LMargin + SubClAr(1), CurY + 15, 2, SubClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "CGST", LMargin + SubClAr(1) + SubClAr(2), CurY, 2, SubClAr(3) + SubClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "SGST", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4), CurY, 2, SubClAr(5) + SubClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "IGST", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6), CurY, 2, SubClAr(7) + SubClAr(8), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8), CurY, 2, SubClAr(9), pFont)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(vLine_Pen, LMargin + SubClAr(1) + SubClAr(2), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8), CurY)
            LnAr2 = CurY
            CurY = CurY + 5
            Common_Procedures.Print_To_PrintDocument(e, "%", LMargin + SubClAr(1) + SubClAr(2), CurY, 2, SubClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3), CurY, 2, SubClAr(4), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "%", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4), CurY, 2, SubClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5), CurY, 2, SubClAr(6), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "%", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6), CurY, 2, SubClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7), CurY, 2, SubClAr(8), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "TAX AMOUNT", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8), CurY, 2, SubClAr(9), pFont)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)


            CurY = CurY - 15

            CurY = CurY + TxtHgt + 3
            pFont = New Font("Calibri", 9, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("HSN_Code").ToString), LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, IIf(Val(prn_HdDt.Rows(0).Item("Total_Taxable_Value").ToString) <> 0, Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Total_Taxable_Value").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, IIf(Val(prn_HdDt.Rows(0).Item("Total_CGST_Amount").ToString) <> 0, Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("GST_Percentage").ToString) / 2), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) - 10, CurY, 1, 0, pFont)
            pFont = New Font("Calibri", 9, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, IIf(Val(prn_HdDt.Rows(0).Item("Total_CGST_Amount").ToString) <> 0, Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Total_CGST_Amount").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) - 5, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, IIf(Val(prn_HdDt.Rows(0).Item("Total_SGST_Amount").ToString) <> 0, Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("GST_Percentage").ToString) / 2), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) - 10, CurY, 1, 0, pFont)
            pFont = New Font("Calibri", 9, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, IIf(Val(prn_HdDt.Rows(0).Item("Total_SGST_Amount").ToString) <> 0, Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Total_SGST_Amount").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) - 5, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, IIf(Val(prn_HdDt.Rows(0).Item("Total_IGST_Amount").ToString) <> 0, Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("GST_Percentage").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) - 10, CurY, 1, 0, pFont)
            pFont = New Font("Calibri", 9, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, IIf(Val(prn_HdDt.Rows(0).Item("Total_IGST_Amount").ToString) <> 0, Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Total_IGST_Amount").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8) - 5, CurY, 1, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Total_CGST_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("Total_SGST_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("Total_IGST_Amount").ToString)), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8) + SubClAr(9) - 5, CurY, 1, 0, pFont)
            pFont = New Font("Calibri", 9, FontStyle.Regular)

            Ttl_TaxAmt = Ttl_TaxAmt + Val(prn_HdDt.Rows(0).Item("Total_Taxable_Value").ToString)
            Ttl_CGst = Ttl_CGst + Val(prn_HdDt.Rows(0).Item("Total_CGST_Amount").ToString)
            Ttl_Sgst = Ttl_Sgst + Val(prn_HdDt.Rows(0).Item("Total_SGST_Amount").ToString)
            Ttl_igst = Ttl_igst + Val(prn_HdDt.Rows(0).Item("Total_IGST_Amount").ToString)


            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
            CurY = CurY + TxtHgt - 15
            pFont = New Font("Calibri", 9, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Total", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Ttl_TaxAmt) <> 0, Common_Procedures.Currency_Format(Val(Ttl_TaxAmt)), ""), LMargin + SubClAr(1) + SubClAr(2) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Ttl_CGst) <> 0, Common_Procedures.Currency_Format(Val(Ttl_CGst)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) - 5, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Ttl_Sgst) <> 0, Common_Procedures.Currency_Format(Val(Ttl_Sgst)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) - 5, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Ttl_igst) <> 0, Common_Procedures.Currency_Format(Val(Ttl_igst)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8) - 5, CurY, 1, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(Val(Ttl_CGst) + Val(Ttl_Sgst) + Val(Ttl_igst)), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8) + SubClAr(9) - 5, CurY, 1, 0, pFont)
            pFont = New Font("Calibri", 9, FontStyle.Regular)
            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)

            e.Graphics.DrawLine(vLine_Pen, LMargin + SubClAr(1), CurY, LMargin + SubClAr(1), TopLnYAxis)
            e.Graphics.DrawLine(vLine_Pen, LMargin + SubClAr(1) + SubClAr(2), CurY, LMargin + SubClAr(1) + SubClAr(2), TopLnYAxis)
            e.Graphics.DrawLine(vLine_Pen, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3), LnAr2)
            e.Graphics.DrawLine(vLine_Pen, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4), TopLnYAxis)
            e.Graphics.DrawLine(vLine_Pen, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5), LnAr2)

            e.Graphics.DrawLine(vLine_Pen, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6), TopLnYAxis)
            e.Graphics.DrawLine(vLine_Pen, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7), LnAr2)

            e.Graphics.DrawLine(vLine_Pen, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8), TopLnYAxis)

            CurY = CurY + 5
            BmsInWrds = ""
            If (Val(Ttl_CGst) + Val(Ttl_Sgst) + Val(Ttl_igst)) <> 0 Then
                BmsInWrds = Common_Procedures.Rupees_Converstion(Val(Ttl_CGst) + Val(Ttl_Sgst) + Val(Ttl_igst))
            End If

            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Tax Amount(In Words) : " & BmsInWrds & " ", LMargin + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub
    Private Sub Printing_Delivery_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim pFont As Font
        'Dim ps As Printing.PaperSize
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
        Dim ps As Printing.PaperSize

        ''Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8X6", 800, 600)
        ''PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
        ''PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1
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
            .Left = 30
            .Right = 40
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

        NoofItems_PerPage = 5  ' 7

        Erase LnAr
        Erase ClAr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        'ClAr(1) = 35
        'ClAr(2) = 120
        'ClAr(3) = 130
        'ClAr(4) = 60
        'ClAr(5) = 70
        'ClAr(6) = 85
        'ClAr(7) = 65
        'ClAr(8) = 70
        'ClAr(9) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8))

        ClAr(1) = 35        'SNO
        ClAr(2) = 100       'PARTICULARS
        ClAr(3) = 70        'HSN CODE
        ClAr(4) = 50        'GST %
        ClAr(5) = 55        'ENDS
        ClAr(6) = 55       'Bobins
        ClAr(7) = 65     'Meter_Bobin
        ClAr(8) = 60       'REEL
        ClAr(9) = 90       'WEIGHT
        ClAr(10) = 70      'RATE
        ClAr(11) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10)) 'AMOUNT

        TxtHgt = 17.75 ' 18

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try
            If prn_HdDt.Rows.Count > 0 Then

                Printing_Delivery_Format1_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr)

                NoofDets = 0

                CurY = CurY - 10

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then

                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued...", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_Delivery_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, False)

                            NoofDets = 0
                            e.HasMorePages = True

                            Return

                        End If

                        ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Colour_name").ToString)
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
                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)

                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("HSN_Code").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 5, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("GST_Percentage").ToString), "#########.##"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 5, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Ends").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Bobins").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 5, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Meter_Bobin").ToString), "#########.##"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 5, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("REEL").ToString), "########.##"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 5, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("WEIGHT").ToString), "########0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 5, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Rate").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) - 5, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) - 5, CurY, 1, 0, pFont)


                        NoofDets = NoofDets + 1

                        If Trim(ItmNm2) <> "" Then
                            CurY = CurY + TxtHgt - 5
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                            NoofDets = NoofDets + 1
                        End If

                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If

                Printing_Delivery_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True)
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

    Private Sub Printing_Delivery_Format1_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_EMail As String, Cmp_PanNo As String = "", Cmp_PanCap As String = ""
        Dim Cmp_StateCap As String = "", Cmp_StateNm As String = "", Cmp_StateCode As String = "", Cmp_GSTIN_Cap As String = "", Cmp_GSTIN_No As String = ""
        Dim strHeight As Single
        Dim C1 As Single
        Dim W1 As Single
        Dim S1, s2 As Single
        Dim S As String
        Dim strWidth As Single = 0
        Dim CurX As Single = 0, CurY1 As Single = 0, CurY2 As Single = 0

        PageNo = PageNo + 1
        prn_Count = prn_Count + 1

        '----------------------------------------------------------------
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
        '----------------------------------------------------------------

        da2 = New SqlClient.SqlDataAdapter("select a.* from " & Trim(Common_Procedures.EntryTempTable) & " a ", con)
        da2.Fill(dt2)
        If dt2.Rows.Count > NoofItems_PerPage Then
            '  Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        dt2.Clear()
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "BOBIN INVOICE", LMargin, CurY - TxtHgt, 2, PrintWidth, p1Font)

        CurY = CurY + TxtHgt - 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = "" : Cmp_PanNo = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_EMail = ""

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
        Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString
        If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
            Cmp_EMail = "MAIL ID : " & prn_HdDt.Rows(0).Item("Company_EMail").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_PanNo").ToString) <> "" Then
            Cmp_PanCap = "PAN : "
            Cmp_PanNo = prn_HdDt.Rows(0).Item("Company_PanNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "PHONE NO.:" & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
        End If
        'Cmp_StateCap = Common_Procedures.State_IdNoToName(con, Trim(prn_HdDt.Rows(0).Item("Company_State_Idno").ToString))
        'Cmp_StateNm = "STATE : " & Cmp_StateCap & "  "
        'Cmp_GSTIN_No = "GSTIN : " & prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString & " "

        If Trim(Common_Procedures.State_IdNoToName(con, Trim(prn_HdDt.Rows(0).Item("Company_State_Idno").ToString))) <> "" Then
            Cmp_StateCap = "STATE : "
            Cmp_StateNm = Common_Procedures.State_IdNoToName(con, Trim(prn_HdDt.Rows(0).Item("Company_State_Idno").ToString))
            'If Trim(prn_HdDt.Rows(0).Item("Company_State_Code").ToString) <> "" Then
            '    Cmp_StateNm = Cmp_StateNm & "   CODE : " & prn_HdDt.Rows(0).Item("Company_State_Code").ToString
            'End If
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString) <> "" Then
            Cmp_GSTIN_Cap = "GSTIN : "
            Cmp_GSTIN_No = prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
        End If

        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 18, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


        CurY = CurY + strHeight - 1
        p1Font = New Font("Calibri", 9, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1 & " " & Cmp_Add2, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        'CurY = CurY + TxtHgt - 1
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + strHeight - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, p1Font)

        CurY = CurY + TxtHgt
        ''Common_Procedures.Print_To_PrintDocument(e, "STATE CODE :" & prn_HdDt.Rows(0).Item("Company_State_Code").ToString & " ", LMargin + 5, CurY, 0, PrintWidth, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_StateNm & " ", LMargin + 5, CurY, 0, PrintWidth, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, PageWidth - 10, CurY, 1, 0, pFont)

        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_GSTIN_Cap), p1Font).Width
        strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_StateNm & "     " & Cmp_GSTIN_Cap & Cmp_GSTIN_No & "     " & Cmp_PanCap & Cmp_PanNo), pFont).Width
        If PrintWidth > strWidth Then
            CurX = LMargin + (PrintWidth - strWidth) / 2
        Else
            CurX = LMargin
        End If

        p1Font = New Font("Calibri", 10, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_StateCap, CurX, CurY, 0, 0, p1Font)
        strWidth = e.Graphics.MeasureString(Cmp_StateCap, p1Font).Width
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, Cmp_StateNm, CurX, CurY, 0, 0, pFont)

        strWidth = e.Graphics.MeasureString(Cmp_StateNm, pFont).Width
        p1Font = New Font("Calibri", 10, FontStyle.Bold)
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_GSTIN_Cap, CurX, CurY, 0, PrintWidth, p1Font)
        strWidth = e.Graphics.MeasureString("     " & Cmp_GSTIN_Cap, p1Font).Width
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, CurX, CurY, 0, 0, pFont)

        strWidth = e.Graphics.MeasureString(Cmp_GSTIN_No, pFont).Width
        p1Font = New Font("Calibri", 10, FontStyle.Bold)
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_PanCap, CurX, CurY, 0, PrintWidth, p1Font)
        strWidth = e.Graphics.MeasureString("     " & Cmp_PanCap, p1Font).Width
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PanNo, CurX, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        '  Common_Procedures.Print_To_PrintDocument(e, Cmp_panno, LMargin + 5, CurY, 0, PrintWidth, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_EMail, LMargin, CurY, 2, PrintWidth, pFont)


        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) '+ ClAr(7)
        'C1 = ClAr(1) + ClAr(2) + ClAr(3)
        W1 = e.Graphics.MeasureString("ORDER NO : ", pFont).Width
        'w2 = e.Graphics.MeasureString("DESP.TO : ", pFont).Width
        'S1 = e.Graphics.MeasureString("TO  :  ", pFont).Width
        s2 = e.Graphics.MeasureString("TRANSPORT :  ", pFont).Width

        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "TO  :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_mainName").ToString, LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "INV.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Bobin_Sales_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Bobin_Sales_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        p1Font = New Font("Calibri", 14, FontStyle.Bold)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "VECHILE NO  ", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vechile_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "GSTIN  NO  :" & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString & " ", LMargin + S1 + 10, CurY, 0, 0, pFont)
        p1Font = New Font("Calibri", 14, FontStyle.Bold)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "STATE : " & Common_Procedures.State_IdNoToName(con, Trim(prn_HdDt.Rows(0).Item("Ledger_State_Idno").ToString)) & "     " & "STATE CODE :" & prn_HdDt.Rows(0).Item("Ledger_State_Code").ToString & " ", LMargin + S1 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "STATE CODE :" & prn_HdDt.Rows(0).Item("Ledger_State_Code").ToString & " ", LMargin + ClAr(1) + ClAr(2), CurY, 0, 0, pFont)
        p1Font = New Font("Calibri", 14, FontStyle.Bold)


        'CurY = CurY + TxtHgt
        'Common_Procedures.Print_To_PrintDocument(e, "STATE CODE :" & prn_HdDt.Rows(0).Item("Ledger_State_Code").ToString & " ", LMargin + S1 + 10, CurY, 0, 0, pFont)

        p1Font = New Font("Calibri", 14, FontStyle.Bold)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))

        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "PARTICULARS", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "HSN CODE", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "GST %", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "ENDS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "COUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY + TxtHgt, 2, ClAr(5), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "BOBINS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "METER/", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "BOBIN", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY + TxtHgt, 2, ClAr(7), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "REEL", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY + TxtHgt, 2, ClAr(8), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY + TxtHgt, 2, ClAr(9), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "RATE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, 2, ClAr(10), pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "REEL", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY + TxtHgt, 2, ClAr(10), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, 2, ClAr(11), pFont)

        CurY = CurY + TxtHgt + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY

    End Sub

    Private Sub Printing_Delivery_Format1_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim i As Integer
        Dim Cmp_Name As String
        Dim p1Font As Font
        Dim W1 As Single = 0
        Dim C1 As Single = 0
        Dim s2 As Single = 0
        Dim vprn_BlNos As String = ""

        For i = NoofDets + 1 To NoofItems_PerPage
            CurY = CurY + TxtHgt
        Next

        C1 = ClAr(1) + ClAr(2) + ClAr(3) - 30
        W1 = e.Graphics.MeasureString("TOTAL BOBIN : ", pFont).Width
        'w2 = e.Graphics.MeasureString("DESP.TO : ", pFont).Width
        'S1 = e.Graphics.MeasureString("TO  :  ", pFont).Width
        s2 = e.Graphics.MeasureString("TOTAL BOBIN :  ", pFont).Width

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)


        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Bobins").ToString), "########0"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 5, CurY, 1, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString), "####0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 50, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Total_Amount").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) - 10, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(5) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), LnAr(3))

        'CurY = CurY + TxtHgt + 8
        'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        'CurY = CurY + TxtHgt + 10
        'If is_LastPage = True Then

        '    If Val(prn_HdDt.Rows(0).Item("Freight").ToString) <> 0 Then
        '        CurY = CurY + TxtHgt
        '        Common_Procedures.Print_To_PrintDocument(e, "Freight", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) - 10, CurY, 1, 0, pFont)
        '        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Freight").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
        '    End If
        'End If
        ''CurY = CurY + TxtHgt

        'If is_LastPage = True Then
        '    If Val(prn_HdDt.Rows(0).Item("AddLess_BeforeTax").ToString) <> 0 Then
        '        CurY = CurY + TxtHgt
        '        Common_Procedures.Print_To_PrintDocument(e, "AddLess", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) - 10, CurY, 1, 0, pFont)
        '        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("AddLess_BeforeTax").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
        '    End If
        'End If
        'CurY = CurY + TxtHgt - 10
        '-------------------------------------------------------------------

        prn_CGST_Amount = prn_HdDt.Rows(0).Item("Total_CGst_Amount").ToString
        prn_SGST_Amount = prn_HdDt.Rows(0).Item("Total_SGst_Amount").ToString
        prn_IGST_Amount = prn_HdDt.Rows(0).Item("Total_IGST_Amount").ToString

        prn_GST_Perc = Val(Common_Procedures.get_FieldValue(con, "ItemGroup_Head", "Item_GST_Percentage", "ItemGroup_Name = '" & Trim(cbo_ItemGroup.Text) & "'"))


        If Val(prn_CGST_Amount) <> 0 Or Val(prn_SGST_Amount) <> 0 Or Val(prn_IGST_Amount) <> 0 Then

            'If Val(prn_HdDt.Rows(0).Item("Freight").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("AddLess_BeforeTax").ToString) <> 0 Then
            '    CurY = CurY + TxtHgt
            '    e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, PageWidth, CurY)
            'Else
            '    CurY = CurY + 10
            'End If

            CurY = CurY + TxtHgt - 10
            If is_LastPage = True Then
                p1Font = New Font("Calibri", 12, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "Taxable Value :", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, 1, 0, p1Font)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Taxable_Value").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, p1Font)
            End If
        End If

        If Val(prn_CGST_Amount) <> 0 Then
            CurY = CurY + TxtHgt
            If is_LastPage = True Then
                If prn_CGST_Amount <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, "CGST @ " & Trim(Val(prn_GST_Perc / 2)) & " % :", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, 1, 0, pFont)
                Else
                    Common_Procedures.Print_To_PrintDocument(e, "CGST ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                End If
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_CGST_Amount), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            End If
        End If

        If Val(prn_SGST_Amount) <> 0 Then
            CurY = CurY + TxtHgt
            If is_LastPage = True Then
                If prn_SGST_Amount <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, "SGST @ " & Trim(Val(prn_GST_Perc / 2)) & " % :", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, 1, 0, pFont)
                Else
                    Common_Procedures.Print_To_PrintDocument(e, "SGST ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                End If
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_SGST_Amount), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            End If
        End If

        If Val(prn_IGST_Amount) <> 0 Then
            CurY = CurY + TxtHgt
            If is_LastPage = True Then
                If prn_IGST_Amount <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, "IGST @ " & Trim(Val(prn_GST_Perc)) & " % :", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, 1, 0, pFont)
                Else
                    Common_Procedures.Print_To_PrintDocument(e, "IGST ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                End If
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_IGST_Amount), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            End If
        End If
        '***** GST END *****



        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "NET AMOUNT :   ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, 1, 0, p1Font)
        'Common_Procedures.Print_To_PrintDocument(e, "NET AMOUNT :   " & prn_HdDt.Rows(0).Item("Net_Amount").ToString, PageWidth - 10, CurY, 1, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Net_Amount").ToString, PageWidth - 10, CurY, 1, 0, p1Font)


        CurY = CurY + TxtHgt + 8
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(6) = CurY

        'CurY = CurY + TxtHgt - 10
        'Common_Procedures.Print_To_PrintDocument(e, "BALANCE BOBIN ", LMargin + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + s2 + 30, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, Balance_Bobin, LMargin + s2 + 60, CurY, 0, 0, pFont)

        'Common_Procedures.Print_To_PrintDocument(e, "BALANCE AMOUNT ", LMargin + C1, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + s2 + C1 + 30, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, Balance_Amount, LMargin + s2 + C1 + 70, CurY, 0, 0, pFont)

        'CurY = CurY + TxtHgt + 2
        'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        LnAr(6) = CurY
        Common_Procedures.Print_To_PrintDocument(e, "( " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(prn_HdDt.Rows(0).Item("User_IdNo").ToString)))) & " )", LMargin, CurY + 10, 2, PageWidth, p1Font)

        CurY = CurY + TxtHgt + 30

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        p1Font = New Font("Calibri", 12, FontStyle.Bold)



        Common_Procedures.Print_To_PrintDocument(e, "Receiver Signature", LMargin + 5, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "Prepared By", LMargin, CurY, 2, PageWidth, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

        CurY = CurY + TxtHgt + 10

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

    End Sub

    Private Sub Balance_Calculation()
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewNo As Integer = 0
        Dim Led_ID As Integer = 0
        Dim cmd As New SqlClient.SqlCommand
        Dim Dt As New DataTable
        Dim Dtbl1 As New DataTable
        Dim Bal As Decimal = 0
        Dim Amt As Double = 0, BillPend As Double = 0
        Dim count As String = ""
        Dim eNDS As String = ""

        Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)


        '-----------BALANCE

        da = New SqlClient.SqlDataAdapter("select  sum(a.voucher_amount) as amount from voucher_details a WHERE a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.ledger_idno = " & Str(Val(Led_ID)) & " ", con)
        Dtbl1 = New DataTable
        da.Fill(Dtbl1)

        Balance_Amount = ""
        Bal = 0
        If Dtbl1.Rows.Count > 0 Then
            For i = 0 To Dtbl1.Rows.Count - 1
                Amt = Val(Dtbl1.Rows(i).Item("amount").ToString)
                Balance_Amount = Trim(Format(Math.Abs(Val(Amt)), "#########0.00")) & IIf(Val(Amt) >= 0, " Cr", " Dr")

                Amt = Val(Dtbl1.Rows(i).Item("amount").ToString)
                Balance_Amount = Trim(Format(Math.Abs(Val(Amt)), "#########0.00")) & IIf(Val(Amt) >= 0, " Cr", " Dr")
            Next i
        End If


        '-------- Empty Bobin
        cmd.Connection = con

        cmd.CommandText = "Truncate table " & Trim(Common_Procedures.ReportTempSubTable) & ""
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Truncate table " & Trim(Common_Procedures.ReportTempTable) & ""
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempSubTable) & "(Int1, Name1, Int2) Select a.DeliveryTo_Idno, tP.Ledger_Name,  sum(a.Empty_BObin) from Stock_Empty_BeamBagCone_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON a.DeliveryTo_Idno <> 0 and a.DeliveryTo_Idno = tP.Ledger_IdNo Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and tP.ledger_idno = " & Str(Val(Led_ID)) & " and  (a.Empty_BObin) <> 0 group by a.DeliveryTo_Idno, tP.Ledger_Name having sum(a.Empty_BObin) <> 0 "
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempSubTable) & "(Int1, Name1, Int2) Select a.ReceivedFrom_Idno, tP.Ledger_Name,  -1*sum(a.Empty_BObin) from Stock_Empty_BeamBagCone_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON a.ReceivedFrom_Idno <> 0 and a.ReceivedFrom_Idno = tP.Ledger_IdNo Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and tP.ledger_idno = " & Str(Val(Led_ID)) & " and (a.Empty_BObin) <> 0 group by a.ReceivedFrom_Idno, tP.Ledger_Name having sum(a.Empty_BObin) <> 0 "
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & "(Int1, Name1, Int2) Select Int1, Name1,  sum(Int2) from " & Trim(Common_Procedures.ReportTempSubTable) & " group by Int1, Name1  having sum(Int2) <> 0 "
        cmd.ExecuteNonQuery()

        Balance_Bobin = 0

        da = New SqlClient.SqlDataAdapter("select Int1, name1, Int2 from " & Trim(Common_Procedures.ReportTempTable) & " ", con)
        Dtbl1 = New DataTable
        da.Fill(Dtbl1)

        If Dtbl1.Rows.Count > 0 Then
            For i = 0 To Dtbl1.Rows.Count - 1
                Balance_Bobin = Val(Dtbl1.Rows(i).Item("Int2").ToString)
            Next i
        End If
        Dt.Dispose()
        da.Dispose()
    End Sub

    Private Sub btn_Filter_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter_Close.Click
        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        Filter_Status = False
    End Sub

    Private Sub txt_Remarks_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Remarks.KeyDown
        If e.KeyCode = 40 Then
            If MessageBox.Show("Do you want to Save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()
            Else
                msk_Date.Focus()
            End If
        End If
    End Sub

    Private Sub txt_Remarks_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Remarks.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to Save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()
            Else
                msk_Date.Focus()
            End If
        End If
    End Sub

    Private Sub txt_PartyBobin_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_PartyBobin.KeyDown
        If e.KeyValue = 38 Then
            If dgv_BobinDetails.Rows.Count > 0 Then
                dgv_BobinDetails.Focus()
                dgv_BobinDetails.CurrentCell = dgv_BobinDetails.Rows(0).Cells(1)
                dgv_BobinDetails.CurrentCell.Selected = True

            Else
                txt_Freight.Focus()

            End If
        End If

        If e.KeyValue = 40 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub txt_PartyBobin_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_PartyBobin.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub



    Private Sub txt_OutBobin_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_OurBobin.KeyDown
        Try
            If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
            If e.KeyValue = 40 Then

                txt_Remarks.Focus()


            End If

        Catch ex As Exception
            '----
        End Try
    End Sub

    Private Sub txt_OutBobin_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_OurBobin.KeyPress
        Try
            If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
            If Asc(e.KeyChar) = 13 Then

                txt_Remarks.Focus()


            End If

        Catch ex As Exception
            '--
        End Try
    End Sub

    Private Sub cbo_KuriCount_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
    End Sub

    Private Sub dgtxt_BobinDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_BobinDetails.KeyUp
        dgv_BobinDetails_KeyUp(sender, e)
    End Sub

    Private Sub chk_NoStockPosting_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()
            Else
                dtp_Date.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_SalesAcc_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_SalesAcc.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 28)", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_SalesAcc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_SalesAcc.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_SalesAcc, cbo_Ledger, cbo_Type, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 28)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_SalesAcc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_SalesAcc.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_SalesAcc, cbo_Type, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 28)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_SalesAcc_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_SalesAcc.KeyUp
        If e.Control = False And e.KeyValue = 17 Then

            Common_Procedures.MDI_LedType = ""
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_SalesAcc.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
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
    Private Sub cbo_ItemGroup_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ItemGroup.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "ItemGroup_Head", "ItemGroup_Name", "", "(Cloth_IdNo = 0)")
    End Sub

    Private Sub cbo_ItemGroup_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ItemGroup.KeyDown

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_ItemGroup, Nothing, cbo_Transport, "ItemGroup_Head", "ItemGroup_Name", "", "(ItemGroup_IdNo = 0)")

        If (e.KeyValue = 38 And cbo_ItemGroup.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            cbo_Type.Focus()
        End If


    End Sub

    Private Sub cbo_ItemGroup_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_ItemGroup.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_ItemGroup, cbo_Transport, "ItemGroup_Head", "ItemGroup_Name", "", "(ItemGroup_IdNo = 0)")
    End Sub
    Private Sub cbo_ItemGroup_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ItemGroup.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New ItemGroup_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_ItemGroup.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub
    Private Sub cbo_Type_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Type.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")
    End Sub
    Private Sub cbo_Type_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Type.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Type, cbo_SalesAcc, cbo_ItemGroup, "", "", "", "")
    End Sub

    Private Sub cbo_Type_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Type.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Type, cbo_ItemGroup, "", "", "", "")
    End Sub

    Private Sub Get_State_Code(ByVal Ledger_IDno As Integer, ByRef Ledger_State_Code As String, ByRef Company_State_Code As String)

        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        Try

            da = New SqlClient.SqlDataAdapter("Select * from Ledger_Head a LEFT OUTER JOIN State_Head b ON a.Ledger_State_IdNo = b.State_IdNo where a.Ledger_IdNo = " & Str(Val(Ledger_IDno)), con)
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0).Item("State_Code").ToString) = False Then
                    Ledger_State_Code = Trim(dt.Rows(0).Item("State_Code").ToString)
                End If

            End If
            dt.Clear()
            dt.Dispose()
            da.Dispose()

            da = New SqlClient.SqlDataAdapter("Select * from Company_Head a LEFT OUTER JOIN State_Head b ON a.Company_State_IdNo = b.State_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)), con)
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0).Item("State_Code").ToString) = False Then
                    Company_State_Code = Trim(dt.Rows(0).Item("State_Code").ToString)
                End If
            End If
            dt.Clear()
            dt.Dispose()
            da.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            dt.Dispose()
            da.Dispose()

        End Try
    End Sub

    Private Sub txt_Freight_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Freight.KeyDown
        If e.KeyValue = 38 Then
            If dgv_BobinDetails.Rows.Count > 0 Then
                dgv_BobinDetails.Focus()
                dgv_BobinDetails.CurrentCell = dgv_BobinDetails.Rows(0).Cells(1)
                dgv_BobinDetails.CurrentCell.Selected = True

            Else
                cbo_VechileNo.Focus()

            End If
        End If

        If e.KeyValue = 40 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub txt_Freight_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Freight.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

        If Asc(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub cbo_ItemGroup_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ItemGroup.SelectedIndexChanged
        Total_Calculation()
    End Sub

    Private Sub txt_Freight_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Freight.TextChanged
        Total_Calculation()
    End Sub

    Private Sub txt_AddLess_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_AddLess.TextChanged
        Total_Calculation()
    End Sub

    Private Sub cbo_Ledger_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.LostFocus
        Total_Calculation()
    End Sub

    Private Sub cbo_Ledger_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.SelectedIndexChanged
        'Total_Calculation()
    End Sub

    Private Sub dgtxt_BobinDetails_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_BobinDetails.TextChanged
        Try
            With dgv_BobinDetails
                If .Rows.Count <> 0 Then
                    .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(dgtxt_BobinDetails.Text)
                End If
            End With

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub txt_InvoicePrefixNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_InvoicePrefixNo.KeyDown
        On Error Resume Next
        ' If e.KeyValue = 38 Then txt_Packing.Focus()
        If e.KeyValue = 40 Then msk_Date.Focus()
    End Sub
    Private Sub Printing_Delivery_Format_GST1(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim p2Font As Font
        'Dim ps As Printing.PaperSize
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
        Dim ps As Printing.PaperSize
        Dim ItmNm1 As String, ItmNm2 As String
        Dim SNo As Integer
        Dim vLine_Pen As Pen

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                Exit For
            End If
        Next

        'Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8X6", 800, 600)
        'PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
        'PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 60 ' 30
            .Right = 40
            .Top = 20 ' 30
            .Bottom = 30
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        p2Font = New Font("Calibri", 11, FontStyle.Regular)

        e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        With PrintDocument1.DefaultPageSettings.PaperSize
            PrintWidth = .Width - RMargin - LMargin
            PrintHeight = .Height - TMargin - BMargin
            PageWidth = .Width - RMargin
            PageHeight = .Height - BMargin
        End With

        NoofItems_PerPage = 12 '15   ' 7

        Erase LnAr
        Erase ClAr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}


        ClAr(1) = 35        'SNO
        ClAr(2) = 100       'PARTICULARS

        ClAr(3) = 145        'ENDS
        ClAr(4) = 55       'Bobins
        ClAr(5) = 75       'Meter_Bobin
        ClAr(6) = 0       'Meters

        ClAr(7) = 77        'HSN CODE
        ClAr(8) = 50        'GST %

        ClAr(9) = 0       'REEL
        ClAr(10) = 85     'RATE
        ClAr(11) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10)) 'AMOUNT

        TxtHgt = 17.75 ' 18

        EntryCode = Trim((Pk_Condition)) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        vLine_Pen = New Pen(Color.Black, 2)

        Try
            If prn_HdDt.Rows.Count > 0 Then

                Printing_Delivery_Format_GST1_PageHeader(e, EntryCode, TxtHgt, p2Font, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr, vLine_Pen)

                NoofDets = 0

                CurY = CurY - 10

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then

                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued...", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, p2Font)

                            NoofDets = NoofDets + 1

                            Printing_Delivery_Format_GST1_PageFooter(e, EntryCode, TxtHgt, p2Font, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, False)

                            NoofDets = 0
                            e.HasMorePages = True

                            Return

                        End If

                        ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Colour_name").ToString)
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
                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(SNo)), LMargin + 10, CurY, 0, 0, p2Font)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClAr(1) + 10, CurY, 0, 0, p2Font)
                        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("HSN_Code").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 2, CurY, 1, 0,  p2Font)
                        'Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("GST_Percentage").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 5, CurY, 1, 0,  p2Font)

                        'Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("EndscOUNT_Name").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 5, CurY, 0, 0,  p2Font)
                        'Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Bobins").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 5, CurY, 1, 0,  p2Font)
                        'Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Meter_Bobin").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 5, CurY, 1, 0,  p2Font)
                        '' Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Meters").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 5, CurY, 1, 0,  p2Font)

                        '----------
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Ends").ToString, LMargin + ClAr(1) + ClAr(2) + 5, CurY, 0, 0, p2Font)
                        Common_Procedures.Print_To_PrintDocument(e, (Val(prn_DetDt.Rows(prn_DetIndx).Item("Bobins").ToString)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 5, CurY, 1, 0, p2Font)

                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Meter_Bobin").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 5, CurY, 1, 0, p2Font)

                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("HSN_Code").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 5, CurY, 1, 0, p2Font)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("GST_Percentage").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 3, CurY, 1, 0, p2Font)

                        '----------
                        ' p2Font = New Font("Calibri", 11, FontStyle.Bold)
                        '  Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("REEL").ToString), "########0"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 5, CurY, 1, 0, p2Font)
                        p2Font = New Font("Calibri", 11, FontStyle.Regular)

                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Rate").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) - 5, CurY, 1, 0, p2Font)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) - 5, CurY, 1, 0, p2Font)

                        NoofDets = NoofDets + 1

                        If Trim(ItmNm2) <> "" Then
                            CurY = CurY + TxtHgt - 5
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClAr(1) + 10, CurY, 0, 0, p2Font)
                            NoofDets = NoofDets + 1
                        End If

                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If

                Printing_Delivery_Format_GST1_PageFooter(e, EntryCode, TxtHgt, p2Font, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True)

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

    Private Sub Printing_Delivery_Format_GST1_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal p2Font As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal vLine_Pen As Pen)
        Dim CurX As Single = 0, CurY1 As Single = 0, CurY2 As Single = 0
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim strHeight As Single = 0, strWidth As Single = 0
        Dim C1 As Single, W1, W2, W3 As Single, S1, S2, S3 As Single
        Dim Cmp_Name, Desc As String, Cmp_Add1 As String, Cmp_Add2 As String, Cmp_PanNo As String, Cmp_PanCap As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_EMail As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim S As String
        Dim Inv_No As String = ""
        Dim InvSubNo As String = ""
        Dim ItmNm1 As String = ""
        Dim ItmNm2 As String = ""
        Dim ItmNm3 As String = ""
        Dim I As Integer = 0
        Dim br2 As SolidBrush

        PageNo = PageNo + 1
        prn_Count = prn_Count + 1

        prn_OriDupTri = ""
        If Trim(prn_InpOpts) <> "" Then
            If prn_Count <= Len(Trim(prn_InpOpts)) Then

                S = Mid$(Trim(prn_InpOpts), prn_Count, 1)

                If Val(S) = 1 Then
                    prn_OriDupTri = "ORIGINAL FOR RECEIPIENT"
                ElseIf Val(S) = 2 Then
                    prn_OriDupTri = "DUPLICATE FOR TRANSPORTER"
                ElseIf Val(S) = 3 Then
                    prn_OriDupTri = "TRIPLICATE FOR SUPPLIER"
                ElseIf Val(S) = 4 Then
                    prn_OriDupTri = "EXTRA COPY"
                Else
                    If Trim(prn_InpOpts) <> "0" And Val(prn_InpOpts) = "0" And Len(Trim(prn_InpOpts)) > 3 Then
                        prn_OriDupTri = Trim(prn_InpOpts)
                    End If
                End If

            End If
        End If



        CurY = TMargin + 2

        p1Font = New Font("Calibri", 14, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "TAX INVOICE", LMargin, CurY - TxtHgt - 5, 2, PrintWidth, p1Font)

        '  End If
        If Trim(prn_OriDupTri) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), PageWidth - 10, CurY - TxtHgt, 1, 0, p2Font)
        End If

        e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
        'e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY
        Desc = ""
        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_PanNo = "" : Cmp_EMail = "" : Cmp_PanCap = ""
        Cmp_StateCap = "" : Cmp_StateNm = "" : Cmp_StateCode = "" : Cmp_GSTIN_Cap = "" : Cmp_GSTIN_No = ""

        Desc = prn_HdDt.Rows(0).Item("Company_Description").ToString
        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
        Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString

        If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "Phone : " & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
            Cmp_TinNo = "TIN NO : " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString) <> "" Then
            Cmp_CstNo = "CST NO : " & prn_HdDt.Rows(0).Item("Company_CstNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_PanNo").ToString) <> "" Then
            Cmp_PanCap = "PAN : "
            Cmp_PanNo = prn_HdDt.Rows(0).Item("Company_PanNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
            Cmp_EMail = "Email : " & prn_HdDt.Rows(0).Item("Company_EMail").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_State_Name").ToString) <> "" Then
            Cmp_StateCap = "STATE : "
            Cmp_StateNm = prn_HdDt.Rows(0).Item("Company_State_Name").ToString
            If Trim(prn_HdDt.Rows(0).Item("Company_State_Code").ToString) <> "" Then
                Cmp_StateNm = Cmp_StateNm & "   CODE : " & prn_HdDt.Rows(0).Item("Company_State_Code").ToString
            End If
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString) <> "" Then
            Cmp_GSTIN_Cap = "GSTIN : "
            Cmp_GSTIN_No = prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
        End If

        'p1Font = New Font("Calibri", 14, FontStyle.Bold)
        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1018" Then '---- M.K Textiles (Palladam)
        '    p1Font = New Font("Calibri", 12, FontStyle.Bold)
        '    Common_Procedures.Print_To_PrintDocument(e, "INVOICE", LMargin, CurY, 2, PrintWidth, p1Font)
        'End If

        CurY = CurY + TxtHgt - 15
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


        p1Font = New Font("Calibri", 18, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


        If Trim(prn_HdDt.Rows(0).Item("E_Invoice_IRNO").ToString) <> "" Then
            If IsDBNull(prn_HdDt.Rows(0).Item("E_Invoice_QR_Image")) = False Then
                Dim imageData As Byte() = DirectCast(prn_HdDt.Rows(0).Item("E_Invoice_QR_Image"), Byte())
                If Not imageData Is Nothing Then
                    Using ms As New MemoryStream(imageData, 0, imageData.Length)
                        ms.Write(imageData, 0, imageData.Length)

                        If imageData.Length > 0 Then

                            pic_IRN_QRCode_Image_forPrinting.BackgroundImage = Image.FromStream(ms)

                            e.Graphics.DrawImage(DirectCast(pic_IRN_QRCode_Image_forPrinting.BackgroundImage, Drawing.Image), PageWidth - 90, CurY, 80, 80)
                        End If

                    End Using

                End If

            End If

        End If




        CurY = CurY + strHeight - 7
        If Desc <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, Desc, LMargin, CurY, 2, PrintWidth, p2Font)
        End If

        strWidth = e.Graphics.MeasureString(Trim(Cmp_Add1 & " " & Cmp_Add2), p1Font).Width
        If PrintWidth > strWidth Then
            If Trim(Cmp_Add1 & " " & Cmp_Add2) <> "" Then
                CurY = CurY + TxtHgt - 1
                Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_Add1 & " " & Cmp_Add2), LMargin, CurY, 2, PrintWidth, p2Font)
            End If

            NoofItems_PerPage = NoofItems_PerPage - 1

        Else

            If Cmp_Add1 <> "" Then
                CurY = CurY + TxtHgt - 1
                Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, p2Font)
            End If
            If Cmp_Add2 <> "" Then
                CurY = CurY + TxtHgt - 1
                Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, p2Font)
            End If

        End If


        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PhNo & " / " & Cmp_EMail), LMargin, CurY, 2, PrintWidth, p2Font)

        CurY = CurY + TxtHgt

        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_GSTIN_Cap), p1Font).Width
        strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_StateNm & "     " & Cmp_GSTIN_Cap & Cmp_GSTIN_No & "     " & Cmp_PanCap & Cmp_PanNo), p2Font).Width
        If PrintWidth > strWidth Then
            CurX = LMargin + (PrintWidth - strWidth) / 2
        Else
            CurX = LMargin
        End If

        p1Font = New Font("Calibri", 10, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_StateCap, CurX, CurY, 0, 0, p1Font)
        strWidth = e.Graphics.MeasureString(Cmp_StateCap, p1Font).Width
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, Cmp_StateNm, CurX, CurY, 0, 0, p2Font)

        strWidth = e.Graphics.MeasureString(Cmp_StateNm, p2Font).Width
        p1Font = New Font("Calibri", 10, FontStyle.Bold)
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_GSTIN_Cap, CurX, CurY, 0, PrintWidth, p1Font)
        strWidth = e.Graphics.MeasureString("     " & Cmp_GSTIN_Cap, p1Font).Width
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, CurX, CurY, 0, 0, p2Font)

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1044" Then
            strWidth = e.Graphics.MeasureString(Cmp_GSTIN_No, p2Font).Width
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            CurX = CurX + strWidth
            Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_PanCap, CurX, CurY, 0, PrintWidth, p1Font)
            strWidth = e.Graphics.MeasureString("     " & Cmp_PanCap, p1Font).Width
            CurX = CurX + strWidth
            Common_Procedures.Print_To_PrintDocument(e, Cmp_PanNo, CurX, CurY, 0, 0, p2Font)
        End If


        If Trim(prn_HdDt.Rows(0).Item("E_Invoice_IRNO").ToString) <> "" Then
            ItmNm1 = Trim(prn_HdDt.Rows(0).Item("E_Invoice_IRNO").ToString)

            ItmNm2 = ""
            If Len(ItmNm1) > 35 Then
                For I = 35 To 1 Step -1
                    If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                Next I
                If I = 0 Then I = 35

                ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
            End If

            CurY = CurY + TxtHgt + 2

            e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "IRN : " & Trim(ItmNm1), LMargin, CurY, 0, 0, p1Font, br2)
            Common_Procedures.Print_To_PrintDocument(e, "Ack. No : " & prn_HdDt.Rows(0).Item("E_Invoice_ACK_No").ToString, PrintWidth - 10, CurY, 1, 0, p1Font, br2)

            If Trim(ItmNm2) <> "" Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "          " & Trim(ItmNm2), LMargin, CurY, 0, 0, p1Font)
                Common_Procedures.Print_To_PrintDocument(e, "Ack. Date : " & prn_HdDt.Rows(0).Item("E_Invoice_ACK_Date").ToString, PrintWidth - 10, CurY, 1, 0, p1Font, br2)
            End If


        End If


        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY


        C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 30
        W1 = e.Graphics.MeasureString("DATE & TIME OF   SUPPLY ", p2Font).Width
        S1 = e.Graphics.MeasureString("TO", p2Font).Width  ' e.Graphics.MeasureString("Details of Receiver | Billed to     :",  p2Font).Width

        W2 = e.Graphics.MeasureString("DESPATCH   TO   : ", p2Font).Width
        S2 = e.Graphics.MeasureString("TRANSPORTATION   MODE", p2Font).Width

        W3 = e.Graphics.MeasureString("INVOICE   DATE", p2Font).Width
        S3 = e.Graphics.MeasureString("REVERSE CHARGE   (YES/NO) ", p2Font).Width

        CurY = CurY + 10
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "INVOICE NO", LMargin + 10, CurY, 0, 0, p2Font)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W3 + 10, CurY, 0, 0, p2Font)


        If prn_HdDt.Rows(0).Item("Invoice_PrefixNo").ToString <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Invoice_PrefixNo").ToString & "-" & prn_HdDt.Rows(0).Item("Bobin_Sales_No").ToString, LMargin + W3 + 30, CurY, 0, 0, p1Font)
        Else
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Bobin_Sales_No").ToString, LMargin + W3 + 30, CurY, 0, 0, p1Font)
        End If


        Common_Procedures.Print_To_PrintDocument(e, "REVERSE CHARGE (YES/NO)", LMargin + C1 + 10, CurY, 0, 0, p2Font)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S3 + 10, CurY, 0, 0, p2Font)
        Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin + C1 + S3 + 30, CurY, 0, 0, p2Font)


        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "INVOICE DATE", LMargin + 10, CurY, 0, 0, p2Font)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W3 + 10, CurY, 0, 0, p2Font)
        p2Font = New Font("Calibri", 11, FontStyle.Regular)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Bobin_Sales_Date").ToString), "dd-MM-yyyy").ToString, LMargin + W3 + 30, CurY, 0, 0, p1Font)

        If Trim(prn_HdDt.Rows(0).Item("EWB_No").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "EWAYBILL NO ", LMargin + C1 + 10, CurY, 0, 0, p2Font)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S3 + 10, CurY, 0, 0, p2Font)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("EWB_No").ToString, LMargin + C1 + S3 + 30, CurY, 0, 0, p2Font)
        End If

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
        'LnAr(2) = CurY

        CurY1 = CurY
        CurY2 = CurY

        '---left side

        CurY1 = CurY1 + 10
        p1Font = New Font("Calibri", 10, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "DETAILS OF RECEIVER (BILLED TO) :", LMargin + 10, CurY1, 0, 0, p1Font)

        strHeight = e.Graphics.MeasureString("A", p1Font).Height
        CurY1 = CurY1 + strHeight
        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "M/s. " & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + S1 + 10, CurY1, 0, 0, p1Font)

        CurY1 = CurY1 + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY1, 0, 0, p2Font)

        CurY1 = CurY1 + TxtHgt
        If Trim(prn_HdDt.Rows(0).Item("Ledger_Address2").ToString) <> "" Then

            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY1, 0, 0, p2Font)
        End If
        CurY1 = CurY1 + TxtHgt
        If Trim(prn_HdDt.Rows(0).Item("Ledger_Address3").ToString) <> "" Then

            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY1, 0, 0, p2Font)
        End If

        If Trim(prn_HdDt.Rows(0).Item("Ledger_Address4").ToString) <> "" Then
            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY1, 0, 0, p2Font)
        End If
        CurY1 = CurY1 + TxtHgt
        If Trim(prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString) <> "" Then
            '  CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString & "  CODE : " & prn_HdDt.Rows(0).Item("Ledger_State_Code").ToString, LMargin + S1 + 10, CurY1, 0, 0, p2Font)
        End If

        CurY1 = CurY1 + TxtHgt
        If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "GSTIN : " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + S1 + 10, CurY1, 0, 0, p2Font)
        End If
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1044" Then
            If Trim(prn_HdDt.Rows(0).Item("Pan_No").ToString) <> "" Then
                strWidth = e.Graphics.MeasureString("GSTIN : " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, p2Font).Width
                CurX = LMargin + S1 + 10 + strWidth
                Common_Procedures.Print_To_PrintDocument(e, "      PAN : " & prn_HdDt.Rows(0).Item("Pan_No").ToString, CurX, CurY1, 0, PrintWidth, p2Font)
            End If
        End If



        '--Right Side

        CurY2 = CurY2 + 10
        p1Font = New Font("Calibri", 10, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "DETAILS OF CONSIGNEE (SHIPPED TO) :", LMargin + C1 + 10, CurY2, 0, 0, p1Font)

        strHeight = e.Graphics.MeasureString("A", p1Font).Height
        CurY2 = CurY2 + strHeight
        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "M/s. " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerName").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, p1Font)

        p2Font = New Font("Calibri", 10, FontStyle.Regular)
        CurY2 = CurY2 + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress1").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, p2Font)
        CurY2 = CurY2 + TxtHgt
        If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress2").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress2").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, p2Font)
        End If
        CurY2 = CurY2 + TxtHgt
        If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress3").ToString) <> "" Then
            ' CurY2 = CurY2 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress3").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, p2Font)
        End If

        If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress4").ToString) <> "" Then
            CurY2 = CurY2 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress4").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, p2Font)
        End If
        CurY2 = CurY2 + TxtHgt
        If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_State_Name").ToString) <> "" Then
            ' CurY2 = CurY2 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_State_Name").ToString & "  CODE : " & prn_HdDt.Rows(0).Item("DeliveryTo_State_Code").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, p2Font)
        End If

        CurY2 = CurY2 + TxtHgt
        If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerGSTinNo").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "GSTIN : " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerGSTinNo").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, p2Font)
        End If
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1044" Then
            If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_PanNo").ToString) <> "" Then
                strWidth = e.Graphics.MeasureString("GSTIN : " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerGSTinNo").ToString, p2Font).Width
                CurX = LMargin + C1 + S1 + 10 + strWidth
                Common_Procedures.Print_To_PrintDocument(e, "      PAN : " & prn_HdDt.Rows(0).Item("DeliveryTo_PanNo").ToString, CurX, CurY2, 0, PrintWidth, p2Font)
            End If
        End If




        CurY = IIf(CurY1 > CurY2, CurY1, CurY2)


        CurY = CurY + TxtHgt

        e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
        e.Graphics.DrawLine(vLine_Pen, LMargin + C1, CurY, LMargin + C1, LnAr(2))
        LnAr(3) = CurY



        W2 = e.Graphics.MeasureString("DOCUMENT THROUGH   : ", p2Font).Width
        S2 = e.Graphics.MeasureString("DATE & TIME OF SUPPLY  :", p2Font).Width

        '--Right Side
        CurY = CurY + 10
        Common_Procedures.Print_To_PrintDocument(e, "TRANSPORT ", LMargin + 10, CurY, 0, 0, p2Font)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 10, CurY, 0, 0, p2Font)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("TransportName").ToString, LMargin + W2 + 30, CurY, 0, 0, p2Font)


        Common_Procedures.Print_To_PrintDocument(e, "VEHICLE NO", LMargin + C1 + 10, CurY, 0, 0, p2Font)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S2 + 10, CurY, 0, 0, p2Font)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vechile_No").ToString, LMargin + C1 + S2 + 30, CurY, 0, 0, p2Font)


        'CurY = CurY + TxtHgt
        'Common_Procedures.Print_To_PrintDocument(e, "ORDER NO", LMargin + 10, CurY, 0, 0,  p2Font)
        'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 10, CurY, 0, 0,  p2Font)
        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Party_OrderNo").ToString, LMargin + W2 + 30, CurY, 0, 0,  p2Font)
        'If Trim(prn_HdDt.Rows(0).Item("Party_OrderDate").ToString) <> "" Then
        '    strWidth = e.Graphics.MeasureString(prn_HdDt.Rows(0).Item("Party_OrderNo").ToString,  p2Font).Width
        '    Common_Procedures.Print_To_PrintDocument(e, "Date : " & prn_HdDt.Rows(0).Item("Party_OrderDate").ToString, LMargin + W2 + strWidth + 60, CurY, 0, 0,  p2Font)
        'End If



        'CurY = CurY + TxtHgt
        'Common_Procedures.Print_To_PrintDocument(e, "DC NO", LMargin + 10, CurY, 0, 0,  p2Font)
        'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 10, CurY, 0, 0,  p2Font)
        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Dc_No").ToString, LMargin + W2 + 30, CurY, 0, 0,  p2Font)
        'If Trim(prn_HdDt.Rows(0).Item("Dc_Date").ToString) <> "" Then
        '    strWidth = e.Graphics.MeasureString(prn_HdDt.Rows(0).Item("Dc_No").ToString,  p2Font).Width
        '    Common_Procedures.Print_To_PrintDocument(e, "Date : " & prn_HdDt.Rows(0).Item("Dc_Date").ToString, LMargin + strWidth + W1 + 60, CurY, 0, 0,  p2Font)
        'End If

        'Common_Procedures.Print_To_PrintDocument(e, "TRANSPORT NAME", LMargin + C1 + 10, CurY, 0, 0,  p2Font)
        'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S2 + 10, CurY, 0, 0,  p2Font)
        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("TransportName").ToString, LMargin + C1 + S2 + 30, CurY, 0, 0,  p2Font)


        '' CurY = CurY + TxtHgt
        'Common_Procedures.Print_To_PrintDocument(e, "TRANSPORTATION MODE", LMargin + C1 + 10, CurY, 0, 0,  p2Font)
        'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S2 + 10, CurY, 0, 0,  p2Font)
        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Transport_Mode").ToString, LMargin + C1 + S2 + 30, CurY, 0, 0,  p2Font)
        ''Common_Procedures.Print_To_PrintDocument(e, "TRANSPORTATION MODE", LMargin + 10, CurY, 0, 0,  p2Font)
        ''Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 10, CurY, 0, 0,  p2Font)
        ''Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Transport_Mode").ToString, LMargin + W2 + 30, CurY, 0, 0,  p2Font)


        ''Common_Procedures.Print_To_PrintDocument(e, "DATE & TIME OF SUPPLY", LMargin + C1 + 10, CurY, 0, 0,  p2Font)
        ''Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S2 + 10, CurY, 0, 0,  p2Font)
        ''Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Date_And_Time_Of_Supply").ToString, LMargin + C1 + S2 + 30, CurY, 0, 0,  p2Font)

        'CurY = CurY + TxtHgt
        'Common_Procedures.Print_To_PrintDocument(e, "DOCUMENT THROUGH", LMargin + 10, CurY, 0, 0,  p2Font)
        'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 10, CurY, 0, 0,  p2Font)
        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Through_Name").ToString, LMargin + W2 + 30, CurY, 0, 0,  p2Font)

        'Common_Procedures.Print_To_PrintDocument(e, "PLACE OF SUPPLY", LMargin + C1 + 10, CurY, 0, 0,  p2Font)
        'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S2 + 10, CurY, 0, 0,  p2Font)
        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_State_Name").ToString, LMargin + C1 + S2 + 30, CurY, 0, 0,  p2Font)


        'If Trim(prn_HdDt.Rows(0).Item("Lc_No").ToString) <> "" Then
        '    CurY = CurY + TxtHgt
        '    Common_Procedures.Print_To_PrintDocument(e, "LC NO", LMargin + 10, CurY, 0, 0,  p2Font)
        '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 10, CurY, 0, 0,  p2Font)
        '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Lc_No").ToString, LMargin + W2 + 30, CurY, 0, 0,  p2Font)
        '    If Trim(prn_HdDt.Rows(0).Item("Lc_Date").ToString) <> "" Then
        '        strWidth = e.Graphics.MeasureString(prn_HdDt.Rows(0).Item("Lc_No").ToString,  p2Font).Width
        '        Common_Procedures.Print_To_PrintDocument(e, "Date : " & prn_HdDt.Rows(0).Item("Lc_Date").ToString, LMargin + strWidth + W2 + 60, CurY, 0, 0,  p2Font)
        '    End If
        'End If

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))

        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), p2Font)

        Common_Procedures.Print_To_PrintDocument(e, "PARTICULARS", LMargin + ClAr(1), CurY, 2, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), p2Font)

        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY + TxtHgt, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY + TxtHgt)
        LnAr(10) = CurY
        CurY = CurY + TxtHgt + 10
        Common_Procedures.Print_To_PrintDocument(e, "COLOUR", LMargin + ClAr(1), CurY, 2, ClAr(2), p2Font)

        'Common_Procedures.Print_To_PrintDocument(e, "HSN CODE", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3),  p2Font)

        'Common_Procedures.Print_To_PrintDocument(e, "GST %", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4),  p2Font)

        'Common_Procedures.Print_To_PrintDocument(e, "ENDS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5),  p2Font)

        'Common_Procedures.Print_To_PrintDocument(e, "BOBINS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6),  p2Font)

        'Common_Procedures.Print_To_PrintDocument(e, "METER/", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7),  p2Font)
        'Common_Procedures.Print_To_PrintDocument(e, "BOBINS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY + TxtHgt, 2, ClAr(7),  p2Font)

        '---------
        Common_Procedures.Print_To_PrintDocument(e, "ENDS ", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), p2Font)

        Common_Procedures.Print_To_PrintDocument(e, "BOBINS ", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), p2Font)

        Common_Procedures.Print_To_PrintDocument(e, "METER/", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), p2Font)
        Common_Procedures.Print_To_PrintDocument(e, "BOBINS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY + TxtHgt, 2, ClAr(5), p2Font)

        CurY = CurY - 20

        Common_Procedures.Print_To_PrintDocument(e, "HSN CODE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), p2Font)

        Common_Procedures.Print_To_PrintDocument(e, "GST %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), p2Font)
        '---------

        'Common_Procedures.Print_To_PrintDocument(e, "TOTAL/", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), p2Font)
        'Common_Procedures.Print_To_PrintDocument(e, "REEL", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY + TxtHgt, 2, ClAr(9), p2Font)

        Common_Procedures.Print_To_PrintDocument(e, "RATE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, 2, ClAr(10), p2Font)
        '   Common_Procedures.Print_To_PrintDocument(e, "REEL", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY + TxtHgt, 2, ClAr(10), p2Font)

        Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, 2, ClAr(11), p2Font)

        CurY = CurY + TxtHgt + TxtHgt + 20
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY

    End Sub

    Private Sub Printing_Delivery_Format_GST1_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal p2Font As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim i As Integer
        Dim Cmp_Name As String
        Dim W1 As Single = 0
        Dim C1 As Single = 0
        Dim ItmNm1 As String = ""
        Dim s2 As Single = 0
        Dim vprn_BlNos As String = ""
        Dim SubClAr(15) As Single
        Dim p1Font As Font, p3Font As Font
        Dim rndoff As Double, TtAmt As Double
        Dim BInc As Integer
        Dim BnkDetAr() As String
        Dim BmsInWrds As String
        Dim BankNm1 As String = ""
        Dim BankNm2 As String = ""
        Dim BankNm3 As String = ""
        Dim BankNm4 As String = ""
        Dim CurY1 As Single = 0
        Dim vNoofHsnCodes As Integer = 0
        Dim vTaxPerc As Single = 0

        For i = NoofDets + 1 To NoofItems_PerPage
            CurY = CurY + TxtHgt
        Next

        C1 = ClAr(1) + ClAr(2) + ClAr(3) - 30
        W1 = e.Graphics.MeasureString("TOTAL BOBIN : ", p2Font).Width
        'w2 = e.Graphics.MeasureString("DESP.TO : ",  p2Font).Width
        'S1 = e.Graphics.MeasureString("TO  :  ",  p2Font).Width
        s2 = e.Graphics.MeasureString("TOTAL BOBIN :  ", p2Font).Width

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(5) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(10) + 20)
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(10) + 20)
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(10) + 20)
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(10) + 20)
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), LnAr(3))


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

        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "TOTAL BOBIN ", LMargin + 10, CurY, 0, 0, p2Font)
        Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + s2 + 10, CurY, 0, 0, p2Font)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Total_Bobins").ToString, LMargin + s2 + 30, CurY, 0, 0, p2Font)

        Common_Procedures.Print_To_PrintDocument(e, "TOTAL REEL ", LMargin + C1 + 10, CurY, 0, 0, p2Font)
        Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + s2 + C1 + 10, CurY, 0, 0, p2Font)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Total_Reels").ToString, LMargin + s2 + C1 + 20, CurY, 0, 0, p2Font)

        CurY1 = CurY + 5
        CurY1 = CurY1 + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY1, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY1)
        CurY1 = CurY1 - 15
        p3Font = New Font("Calibri", 10, FontStyle.Regular)
        If BankNm1 <> "" Then
            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, BankNm1, LMargin + 10, CurY1, 0, 0, p3Font)
        End If
        If BankNm2 <> "" Then
            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, BankNm2, LMargin + 10, CurY1, 0, 0, p3Font)
        End If
        If BankNm3 <> "" Then
            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, BankNm3, LMargin + 10, CurY1, 0, 0, p3Font)
        End If
        If BankNm4 <> "" Then
            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, BankNm4, LMargin + 10, CurY1, 0, 0, p3Font)
        End If
        CurY1 = CurY1 + TxtHgt + 8
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY1, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY1)

        Common_Procedures.Print_To_PrintDocument(e, "TOTAL AMOUNT :   ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 2, CurY, 0, 0, p2Font)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Total_Amount").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) - 10, CurY, 1, 0, p2Font)
        Balance_Calculation()

        'CurY = CurY + TxtHgt + 10
        If is_LastPage = True Then

            If Val(prn_HdDt.Rows(0).Item("Freight").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Freight", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 2, CurY, 0, 0, p2Font)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Freight").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, p2Font)
            End If
        End If

        If is_LastPage = True Then

            If Val(prn_HdDt.Rows(0).Item("AddLess_BeforeTax").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Add/Less", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 2, CurY, 0, 0, p2Font)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("AddLess_BeforeTax").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, p2Font)
            End If
        End If
        'CurY = CurY + TxtHgt

        CurY = CurY + TxtHgt - 10
        '-------------------------------------------------------------------

        prn_CGST_Amount = prn_HdDt.Rows(0).Item("Total_CGst_Amount").ToString
        prn_SGST_Amount = prn_HdDt.Rows(0).Item("Total_SGst_Amount").ToString
        prn_IGST_Amount = prn_HdDt.Rows(0).Item("Total_IGST_Amount").ToString

        prn_GST_Perc = Val(prn_HdDt.Rows(0).Item("GST_Percentage").ToString)


        If Val(prn_CGST_Amount) <> 0 Or Val(prn_SGST_Amount) <> 0 Or Val(prn_IGST_Amount) <> 0 Then

            If Val(prn_HdDt.Rows(0).Item("Freight").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, PageWidth, CurY)
            Else
                CurY = CurY + 10
            End If

            CurY = CurY + TxtHgt - 10
            If is_LastPage = True Then
                p1Font = New Font("Calibri", 10, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "Taxable Value", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 2, CurY, 0, 0, p2Font)
                '  Common_Procedures.Print_To_PrintDocument(e, "Taxable Value", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, 1, 0,  p2Font)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Taxable_Value").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, p1Font)
            End If
        End If
        CurY = CurY + TxtHgt
        If Val(prn_CGST_Amount) <> 0 Then

            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, "CGST @ " & Trim(Val(prn_GST_Perc / 2)) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 2, CurY, 0, 0, p2Font)

            Else
                Common_Procedures.Print_To_PrintDocument(e, "CGST ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 2, CurY, 0, 0, p2Font)
            End If
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_CGST_Amount), "########0.00"), PageWidth - 10, CurY, 1, 0, p2Font)

        End If
        CurY = CurY + TxtHgt
        If Val(prn_SGST_Amount) <> 0 Then

            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, "SGST @ " & Trim(Val(prn_GST_Perc / 2)) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 2, CurY, 0, 0, p2Font)

            Else
                Common_Procedures.Print_To_PrintDocument(e, "SGST ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 2, CurY, 0, 0, p2Font)
            End If
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_SGST_Amount), "########0.00"), PageWidth - 10, CurY, 1, 0, p2Font)

        End If

        CurY = CurY + TxtHgt
        If Val(prn_IGST_Amount) <> 0 Then

            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, "IGST @ " & Trim(Val(prn_GST_Perc)) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 2, CurY, 0, 0, p2Font)
                ' Common_Procedures.Print_To_PrintDocument(e, "IGST @ " & Trim(Val(prn_GST_Perc)) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, 1, 0,  p2Font)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "IGST ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 2, CurY, 0, 0, p2Font)
            End If
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_IGST_Amount), "########0.00"), PageWidth - 10, CurY, 1, 0, p2Font)

        End If

        '***** GST END *****
        TtAmt = Format(Val(prn_HdDt.Rows(0).Item("Total_Taxable_Value").ToString) + Val(prn_IGST_Amount) + Val(prn_SGST_Amount) + Val(prn_CGST_Amount), "#########0.00")

        rndoff = 0
        rndoff = Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString) - Val(TtAmt)

        CurY = CurY + TxtHgt
        If Val(rndoff) <> 0 Then
            If Val(rndoff) >= 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "ROUND OFF (+) ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 0, 0, p2Font)

                '  Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 20, CurY, 0, 0,  p2Font)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "ROUND OFF (-)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 0, 0, p2Font)

                'Common_Procedures.Print_To_PrintDocument(e, "(-)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 20, CurY, 0, 0,  p2Font)
            End If
            Common_Procedures.Print_To_PrintDocument(e, " " & Format(Math.Abs(Val(rndoff)), "########0.00"), PageWidth - 10, CurY, 1, 0, p2Font)
        End If

        p1Font = New Font("Calibri", 13, FontStyle.Bold)
        CurY = CurY + TxtHgt
        p3Font = New Font("Calibri", 12, FontStyle.Regular)
        Common_Procedures.Print_To_PrintDocument(e, "E & OE", LMargin + 10, CurY, 0, 0, p3Font)

        CurY = CurY + TxtHgt + 8
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(6) = CurY

        CurY = CurY + TxtHgt - 10
        'Common_Procedures.Print_To_PrintDocument(e, "BALANCE BOBIN ", LMargin + 10, CurY, 0, 0,  p2Font)
        'Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + s2 + 30, CurY, 0, 0,  p2Font)
        'Common_Procedures.Print_To_PrintDocument(e, Balance_Bobin, LMargin + s2 + 60, CurY, 0, 0,  p2Font)
        Common_Procedures.Print_To_PrintDocument(e, "NET AMOUNT ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 2, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Net_Amount").ToString, PageWidth - 10, CurY, 1, 0, p1Font)

        'Common_Procedures.Print_To_PrintDocument(e, "BALANCE AMOUNT ", LMargin + C1, CurY, 0, 0,  p2Font)
        'Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + s2 + C1 + 30, CurY, 0, 0,  p2Font)
        'Common_Procedures.Print_To_PrintDocument(e, Balance_Amount, LMargin + s2 + C1 + 70, CurY, 0, 0,  p2Font)

        '  CurY = CurY + TxtHgt

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(9) = CurY
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(4))

        CurY = CurY + 5
        BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))
        BmsInWrds = Replace(Trim(BmsInWrds), "", "")

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1065" Then '---- Logu textile
            BmsInWrds = Trim(UCase(BmsInWrds))
        End If

        p1Font = New Font("Calibri", 10, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "Amount Chargeable(In Words)  : " & BmsInWrds & " ", LMargin + 10, CurY, 0, 0, p1Font)
        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(10) = CurY

        '=============GST SUMMARY============

        '  vNoofHsnCodes = get_GST_Noof_HSN_Codes_For_Printing(EntryCode)

        ''    ' Printing_GST_HSN_Details_Format1(e, EntryCode, TxtHgt, p2Font, LMargin, PageWidth, PrintWidth, CurY, LnAr(10), Pens.Black)



        '==========================

        CurY = CurY + TxtHgt - 15
        p1Font = New Font("Calibri", 9, FontStyle.Underline Or FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "Term & Conditions : ", LMargin + 10, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt

        p2Font = New Font("Webdings", 8, FontStyle.Regular)
        p1Font = New Font("Calibri", 8, FontStyle.Bold)


        ''1
        'If Val(prn_HdDt.Rows(0).Item("Gr_Time").ToString) <> 0 Then
        '    Common_Procedures.Print_To_PrintDocument(e, "=", LMargin + 10, CurY, 0, 0, p2Font)
        '    Common_Procedures.Print_To_PrintDocument(e, "Overdue Interest will be charged at 24% from The  " & Trim(prn_HdDt.Rows(0).Item("Gr_Date").ToString), LMargin + 25, CurY, 0, 0, p1Font)
        'Else
        Common_Procedures.Print_To_PrintDocument(e, "=", LMargin + 10, CurY, 0, 0, p2Font)
        Common_Procedures.Print_To_PrintDocument(e, "Overdue Interest will be charged at 24% from The invoice date. ", LMargin + 25, CurY, 0, 0, p1Font)
        'End If
        '3
        Common_Procedures.Print_To_PrintDocument(e, "=", PrintWidth / 2 + 10, CurY, 0, 0, p2Font)
        Common_Procedures.Print_To_PrintDocument(e, "We will not accept any claim after processing of goods.", PrintWidth / 2 + 25, CurY, 0, 0, p1Font)

        '2
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "=", LMargin + 10, CurY, 0, 0, p2Font)
        Common_Procedures.Print_To_PrintDocument(e, "We are not responsible for any loss or damage in transit.", LMargin + 25, CurY, 0, 0, p1Font)
        '4
        Common_Procedures.Print_To_PrintDocument(e, "=", PrintWidth / 2 + 10, CurY, 0, 0, p2Font)
        Common_Procedures.Print_To_PrintDocument(e, "Subject to " & Trim(Common_Procedures.settings.Jurisdiction) & " jurisdiction. ", PrintWidth / 2 + 25, CurY, 0, 0, p1Font)


        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(10) = CurY

        'If Val(Common_Procedures.User.IdNo) <> 1 Then
        '    Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 20, CurY + 40, 0, 0,  p2Font)
        'End If

        CurY = CurY + 5
        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        p1Font = New Font("Calibri", 7, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "Certified that the Particulars given above are true and correct", PageWidth - 10, CurY, 1, 0, p1Font)
        CurY = CurY + TxtHgt - 5
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        p3Font = New Font("Calibri", 12, FontStyle.Regular)
        Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 10, CurY, 1, 0, p1Font)
        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt
        '   CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "Prepared By", LMargin + 20, CurY, 0, 0, p3Font)
        Common_Procedures.Print_To_PrintDocument(e, "Checked By ", LMargin + 250, CurY, 0, 0, p3Font)
        p1Font = New Font("Calibri", 12, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, "AUTHORISED SIGNATORY ", PageWidth - 10, CurY, 1, 0, p3Font)
        CurY = CurY + TxtHgt + 10

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)


        'Catch ex As Exception

        '    MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        'End Try

    End Sub

    Private Sub dgv_BobinDetails_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_BobinDetails.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
    End Sub

    Private Sub btn_EInvoice_Generation_Click(sender As Object, e As EventArgs) Handles btn_EInvoice_Generation.Click

        btn_Generate_eInvoice.Enabled = True
        btn_Generate_EWB_IRN.Enabled = True

        grp_EInvoice.Visible = True
        grp_EInvoice.BringToFront()
        grp_EInvoice.Left = (Me.Width - grp_EInvoice.Width) / 2
        grp_EInvoice.Top = (Me.Height - grp_EInvoice.Height) / 2
        btn_CheckConnectivity1.Enabled = False
        btn_CheckConnectivity1.Visible = False

    End Sub

    Private Sub btn_Generate_eInvoice_Click(sender As Object, e As EventArgs) Handles btn_Generate_eInvoice.Click
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim vCLONAME As String = ""
        Dim vIS_SERVC_STS As Integer = 0

        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()


        Dim NewCode As String = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Dim Cmd As New SqlClient.SqlCommand
        Cmd.Connection = con
        Cmd.CommandText = "Select count(*) from Bobin_Sales_Head Where Bobin_Sales_Code = '" & Trim(NewCode) & "'"

        Dim c As Int16 = Cmd.ExecuteScalar

        If c <= 0 Then
            MsgBox("Please Save the Invoice Before Generating IRN ", vbOKOnly, "Save")
            Exit Sub
        End If

        Cmd.CommandText = "Select count(*) from Bobin_Sales_Head Where Bobin_Sales_Code = '" & Trim(NewCode) & "' and Len(E_Invoice_IRNO) > 0"
        c = Cmd.ExecuteScalar

        If c > 0 Then
            Dim k As Integer = MsgBox("An IRN Has been Generated already for this Invoice. Do you want to Delete the Previous IRN ?", vbYesNo, "IRN Generated")
            If k = vbNo Then
                MsgBox("Cannot Create a New IRN When there is an IRN generated already !", vbOKOnly, "Duplicate IRN ")
                Exit Sub
            Else

            End If
        End If

        For i = 0 To dgv_BobinDetails.RowCount - 1

            If Val(dgv_BobinDetails.Rows(i).Cells(3).Value) <> 0 Or Val(dgv_BobinDetails.Rows(i).Cells(4).Value) <> 0 Then

                If Val(dgv_BobinDetails.Rows(i).Cells(10).Value) = 0 Or Val(dgv_BobinDetails.Rows(i).Cells(11).Value) = 0 Then
                    MessageBox.Show("Invalid Rate / Amount", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    If dgv_BobinDetails.Enabled And dgv_BobinDetails.Visible Then
                        dgv_BobinDetails.Focus()
                        dgv_BobinDetails.CurrentCell = dgv_BobinDetails.Rows(i).Cells(10)
                    End If
                    Exit Sub
                End If
            End If

        Next

        If Val(lbl_Net_Amt.Text) = 0 Then
            MessageBox.Show("Invalid Amount", "DOES NOT GENERATE EWB...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If txt_AddLess.Enabled And txt_AddLess.Visible Then txt_AddLess.Focus()
            Exit Sub
        End If
        Dim tr As SqlClient.SqlTransaction

        tr = con.BeginTransaction
        Cmd.Transaction = tr

        Try


            Cmd.CommandText = "Delete from e_Invoice_Head  where Ref_Sales_Code = '" & Trim(NewCode) & "'"
            Cmd.ExecuteNonQuery()

            Cmd.CommandText = "Delete from e_Invoice_Details  where Ref_Sales_Code = '" & Trim(NewCode) & "'"
            Cmd.ExecuteNonQuery()

            'Cmd.CommandText = "Insert into e_Invoice_Head (e_Invoice_No,         e_Invoice_date , Buyer_IdNo , Consignee_IdNo ,   Assessable_Value  ,    CGST          ,       SGST       ,       IGST       , Cess , State_Cess ,       Round_Off                                      , Nett_Invoice_Value ,       Ref_Sales_Code   ,              Other_Charges                                , Dispatcher_Idno  ) " &
            '                    " Select        ClothSales_Invoice_No , ClothSales_Invoice_Date, Ledger_IdNo, DeliveryTo_IdNo, Total_Taxable_Amount, Total_CGST_Amount, Total_SGST_Amount, Total_IGST_Amount,  0   ,       0    , (RoundOff_Invoice_Value_Before_TCS + RoundOff_Amount),      Net_Amount    , '" & Trim(NewCode) & "', (ISNULL(TCS_Amount,0)+ ISNULL(AddLess,0)) as OtherCharges ,  Dispatcher_IdNo   from ClothSales_Invoice_Head where ClothSales_Invoice_Code = '" & Trim(NewCode) & "'"
            'Cmd.ExecuteNonQuery()


            Cmd.CommandText = "Insert into e_Invoice_Head (e_Invoice_No,         e_Invoice_date , Buyer_IdNo , Consignee_IdNo ,   Assessable_Value  ,    CGST          ,       SGST       ,       IGST       , Cess , State_Cess ,       Round_Off  , Nett_Invoice_Value ,       Ref_Sales_Code   ,              Other_Charges                  ,      Dispatcher_Idno  ) " &
                                " Select                Bobin_Sales_No ,        Bobin_Sales_Date, Ledger_IdNo, Ledger_IdNo    , Total_Taxable_Value , Total_CGST_Amount, Total_SGST_Amount, Total_IGST_Amount,  0   ,       0    ,  RoundOff_Amount ,      Net_Amount    , '" & Trim(NewCode) & "',                  0 as OtherCharges          ,             0   from Bobin_Sales_Head where Bobin_Sales_Code = '" & Trim(NewCode) & "'"
            Cmd.ExecuteNonQuery()


            Cmd.CommandText = "truncate table " & Trim(Common_Procedures.EntryTempTable) & ""
            Cmd.ExecuteNonQuery()


            vIS_SERVC_STS = 0
            da2 = New SqlClient.SqlDataAdapter("Select a.HSN_Code from Bobin_Sales_Head a Where a.Bobin_Sales_Code = '" & Trim(NewCode) & "' ", con)
            'da2 = New SqlClient.SqlDataAdapter("Select a.HSN_Code from ClothSales_Invoice_Details a Where a.ClothSales_Invoice_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
            da2.SelectCommand.Transaction = tr
            dt2 = New DataTable
            da2.Fill(dt2)
            If dt2.Rows.Count > 0 Then
                If IsDBNull(dt2.Rows(0).Item("HSN_CODE").ToString) = False Then

                    If Microsoft.VisualBasic.Left(Trim(dt2.Rows(0).Item("HSN_CODE").ToString), 2) = "99" Then

                        vIS_SERVC_STS = 1

                    End If
                End If

            End If
            dt2.Clear()




            Cmd.CommandText = " Insert into e_Invoice_Details (Sl_No,   IsService   ,          Product_Description      ,   HSN_Code,  Batch_Details    ,            Quantity          ,     Unit    , Unit_Price ,               Total_Amount                                       ,        Discount    ,                                  Assessable_Amount                                           ,   GST_Rate      , SGST_Amount , IGST_Amount , CGST_Amount , Cess_rate , Cess_Amount, CessNonAdvlAmount , State_Cess_Rate , State_Cess_Amount , StateCessNonAdvlAmount , Other_Charge, Total_Item_Value,   AttributesDetails    ,         Ref_Sales_Code  ) " &
                               "  Select                     a.Sl_No,           0   ,    c.Colour_Name , b.HSN_Code, '' as batchdetails,          a.reel as qty       , 'NOS' as UOM,   a.Rate   , (a.Amount + (CASE WHEN a.sl_no = 1 then (b.Freight + b.AddLess_BeforeTax) else 0 end ) ) as Total_Amount, 0 as DiscountAmount,  ( (a.Amount + (CASE WHEN a.sl_no = 1 then ( b.Freight+b.AddLess_BeforeTax) else 0 end ) ) ) as Assessable_Amount, b.GST_Percentage, Total_SGST_Amount AS SgstAmt, Total_IGST_Amount AS igstAmt, Total_CGST_Amount AS cgstAmt, 0 AS CesRt, 0 AS CesAmt, 0 AS CesNonAdvlAmt, 0 AS StateCesRt , 0 AS StateCesAmt  , 0 as StateCesNonAdvlAmt, 0 as OthChrg, 0 as TotItemVal , '' as AttributesDetails, '" & Trim(NewCode) & "' " &
                               " from Bobin_Sales_Bobin_Details a INNER JOIN Bobin_Sales_Head b  ON a.Bobin_Sales_Code = b.Bobin_Sales_Code INNER JOIN Colour_head c ON A.Colour_IdNo = c.Colour_IdNo " &
                               " Where a.Bobin_Sales_Code = '" & Trim(NewCode) & "' Order by a.Sl_No"
            Cmd.ExecuteNonQuery()


            'Cmd.CommandText = " Insert into e_Invoice_Details " &
            '                   " Select a.Sl_No, " & vIS_SERVC_STS & " as IsServc, " & vCLONAME & " , a.HSN_Code, '' as batchdetails, a.Fold_Meter, 'MTR' as UOM, a.Rate, (a.Amount + (CASE WHEN a.sl_no = 1 then (b.Packing_Amount+b.Freight+b.Insurance+b.Certificate_charges) else 0 end ) ) as Total_Amount, (CASE WHEN a.sl_no = 1 then (b.Trade_Discount_Perc+b.Cash_Discount_Perc) ELSE 0 END ) as DiscountAmount, " &
            '                   " ( (a.Amount + (CASE WHEN a.sl_no = 1 then (b.Packing_Amount + b.Freight + b.Insurance + b.Certificate_charges - b.Trade_Discount_Perc - b.Cash_Discount_Perc) else 0 end ) ) ) as Assessable_Amount, a.GST_Percentage, 0 AS SgstAmt, 0 AS CgstAmt, 0 AS igstAmt, 0 AS CesRt, 0 AS CesAmt, 0 AS CesNonAdvlAmt, 0 AS StateCesRt, 0 AS StateCesAmt, 0 as StateCesNonAdvlAmt, " &
            '                   " 0 as OthChrg, 0 as TotItemVal, '' as AttributesDetails, '" & Trim(NewCode) & "' " &
            '                   " from ClothSales_Invoice_Details a INNER JOIN ClothSales_Invoice_Head b  ON a.ClothSales_Invoice_Code =  b.ClothSales_Invoice_Code INNER JOIN Cloth_Head c ON A.Cloth_IdNo = c.Cloth_IdNo " &
            '                   " Where a.ClothSales_Invoice_Code = '" & Trim(NewCode) & "' Order by a.Sl_No"
            'Cmd.ExecuteNonQuery()

            'Cmd.CommandText = "Insert into e_Invoice_Details  " &
            '                   " Select a.Sl_No, 0 as IsServc, (CASE WHEN b.Cloth_Details <> '' THEN b.Cloth_Details ELSE (CASE WHEN c.Cloth_Description <> '' THEN c.Cloth_Description ELSE c.Cloth_Name END) END) as producDescription , a.HSN_Code, '' as batchdetails, a.Meters, 'MTR' as UOM, a.Rate, (a.Amount + (CASE WHEN a.sl_no = 1 then (b.Packing_Amount+b.Freight+b.Insurance+b.Certificate_charges) else 0 end ) ) as Total_Amount, (CASE WHEN a.sl_no = 1 then (b.Trade_Discount_Perc+b.Cash_Discount_Perc) ELSE 0 END ) as DiscountAmount, " &
            '                   " ( (a.Amount + (CASE WHEN a.sl_no = 1 then (b.Packing_Amount + b.Freight + b.Insurance + b.Certificate_charges - b.Trade_Discount_Perc - b.Cash_Discount_Perc) else 0 end ) ) ) as Assessable_Amount, a.GST_Percentage, 0 AS SgstAmt, 0 AS CgstAmt, 0 AS igstAmt, 0 AS CesRt, 0 AS CesAmt, 0 AS CesNonAdvlAmt, 0 AS StateCesRt, 0 AS StateCesAmt, 0 as StateCesNonAdvlAmt, " &
            '                   " 0 as OthChrg, 0 as TotItemVal, '' as AttributesDetails, '" & Trim(NewCode) & "' " &
            '                   " from ClothSales_Invoice_Details a INNER JOIN ClothSales_Invoice_Head b  ON a.ClothSales_Invoice_Code =  b.ClothSales_Invoice_Code INNER JOIN Cloth_Head c ON A.Cloth_IdNo = c.Cloth_IdNo " &
            '                   " Where a.ClothSales_Invoice_Code = '" & Trim(NewCode) & "'"
            'Cmd.ExecuteNonQuery()


            tr.Commit()

            'Exit Sub

            'rtbeInvoiceResponse.Text = einv.AuthTokenReturnMsg

        Catch ex As Exception

            tr.Rollback()
            MessageBox.Show(ex.Message + " Cannot Generate IRN.", "DOES NOT GENERATE IRN...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            Exit Sub

        End Try

        btn_Generate_eInvoice.Enabled = False

        Dim einv As New eInvoice(Val(lbl_Company.Tag))
        einv.GenerateIRN(Val(lbl_Company.Tag), NewCode, con, rtbeInvoiceResponse, pic_IRN_QRCode_Image, txt_eInvoiceNo, txt_eInvoiceAckNo, txt_eInvoiceAckDate, txt_eInvoice_CancelStatus, "Bobin_Sales_Head", "Bobin_Sales_Code", Pk_Condition)

    End Sub

    Private Sub lbl_Net_Amt_Click(sender As Object, e As EventArgs) Handles lbl_Net_Amt.Click

    End Sub

    Private Sub btn_Get_QR_Code_Click(sender As Object, e As EventArgs) Handles btn_Get_QR_Code.Click
        Dim CMD As New SqlClient.SqlCommand
        CMD.Connection = con

        CMD.CommandText = "DELETE FROM " & Common_Procedures.CompanyDetailsDataBaseName & "..e_Invoice_refresh where IRN = '" & txt_eInvoiceNo.Text & "'"
        CMD.ExecuteNonQuery()

        CMD.CommandText = " INSERT INTO " & Common_Procedures.CompanyDetailsDataBaseName & "..e_Invoice_Refresh ([IRN] ,[ACK_No] , [DOC_No] , [SEARCH_BY]  , [COMPANY_IDNO],[Update_Table] ,[Update_table_Unique_Code],[COMPANYGROUP_IDNO] ) VALUES " &
                          "('" & txt_eInvoiceNo.Text & "' ,'','','I'," & Val(Common_Procedures.CompIdNo).ToString & ",'Bobin_Sales_Head', 'E_Invoice_IRNO'," & Val(Common_Procedures.CompGroupIdNo).ToString & ")"
        CMD.ExecuteNonQuery()

        Shell(Application.StartupPath & "\Refresh_IRN.EXE")
    End Sub

    Private Sub btn_refresh_Click(sender As Object, e As EventArgs) Handles btn_refresh.Click

        Dim NewCode As String = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Dim da As New SqlClient.SqlDataAdapter("Select E_Invoice_QR_Image,E_Invoice_IRNO,E_Invoice_ACK_No,E_Invoice_ACK_Date,E_Invoice_Cancelled_Status FROM  Bobin_Sales_Head WHERE Bobin_Sales_Code = '" & NewCode & "'", con)

        Dim DT As New DataTable

        da.Fill(DT)

        If DT.Rows.Count > 0 Then

            pic_IRN_QRCode_Image.BackgroundImage = Nothing
            txt_eInvoiceAckNo.Text = ""
            txt_eInvoiceAckDate.Text = ""
            txt_eInvoice_CancelStatus.Text = ""

            txt_eInvoiceNo.Text = Trim(DT.Rows(0).Item("E_Invoice_IRNO").ToString)
            If Not IsDBNull(DT.Rows(0).Item("E_Invoice_ACK_No")) Then txt_eInvoiceAckNo.Text = Trim(DT.Rows(0).Item("E_Invoice_ACK_No").ToString)
            If Not IsDBNull(DT.Rows(0).Item("E_Invoice_ACK_Date")) Then txt_eInvoiceAckDate.Text = Trim(DT.Rows(0).Item("E_Invoice_ACK_Date").ToString)
            If Not IsDBNull(DT.Rows(0).Item("E_Invoice_Cancelled_Status")) Then txt_eInvoice_CancelStatus.Text = IIf(DT.Rows(0).Item("E_Invoice_Cancelled_Status") = True, "Cancelled", "Active")

            If IsDBNull(DT.Rows(0).Item("E_Invoice_QR_Image")) = False Then
                Dim imageData As Byte() = DirectCast(DT.Rows(0).Item("E_Invoice_QR_Image"), Byte())
                If Not imageData Is Nothing Then
                    Using ms As New MemoryStream(imageData, 0, imageData.Length)
                        ms.Write(imageData, 0, imageData.Length)
                        If imageData.Length > 0 Then

                            pic_IRN_QRCode_Image.BackgroundImage = Image.FromStream(ms)

                        End If
                    End Using
                End If
            End If

        End If


    End Sub

    Private Sub btn_Delete_eInvoice_Click(sender As Object, e As EventArgs) Handles btn_Delete_eInvoice.Click

        If Len(Trim(txt_EInvoiceCancellationReson.Text)) = 0 Then
            MsgBox("Please provode the reason for cancellation", vbOKCancel, "Provide Reason !")
            Exit Sub
        End If

        Dim NewCode As String = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        Dim einv As New eInvoice(Val(lbl_Company.Tag))
        einv.CancelIRNByIRN(txt_eInvoiceNo.Text, rtbeInvoiceResponse, "Bobin_Sales_Head", "Bobin_Sales_Code", con, txt_eInvoice_CancelStatus, NewCode, txt_EInvoiceCancellationReson.Text)

    End Sub

    Private Sub Btn_Qr_Code_Add_Click(sender As Object, e As EventArgs) Handles Btn_Qr_Code_Add.Click
        OpenFileDialog1.FileName = ""
        If OpenFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            pic_IRN_QRCode_Image.BackgroundImage = Image.FromFile(OpenFileDialog1.FileName)
        End If
    End Sub

    Private Sub Btn_Qr_Code_Close_Click(sender As Object, e As EventArgs) Handles Btn_Qr_Code_Close.Click
        pic_IRN_QRCode_Image.BackgroundImage = Nothing
    End Sub

    Private Sub txt_eInvoiceNo_TextChanged(sender As Object, e As EventArgs) Handles txt_eInvoiceNo.TextChanged

    End Sub

    Private Sub btn_Close_eInvoice_Click(sender As Object, e As EventArgs) Handles btn_Close_eInvoice.Click
        grp_EInvoice.Visible = False
    End Sub

    Private Sub btn_Generate_EWB_IRN_Click(sender As Object, e As EventArgs) Handles btn_Generate_EWB_IRN.Click
        Dim NewCode As String = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Dim Cmd As New SqlClient.SqlCommand
        Cmd.Connection = con
        Cmd.CommandText = "Select count(*) from Bobin_Sales_Bobin_Details Where Bobin_Sales_Code = '" & NewCode & "'"
        Dim c As Int16 = Cmd.ExecuteScalar

        If c <= 0 Then
            MsgBox("Please Save the Invoice Before Generating IRN ", vbOKOnly, "Save")
            Exit Sub
        End If

        Cmd.CommandText = "Select count(*) from Bobin_Sales_Head Where Bobin_Sales_Code = '" & NewCode & "' and (Len(EWB_No) >0 or Len(E_Invoice_IRNO) = 0 OR E_Invoice_IRNO IS NULL )"
        c = Cmd.ExecuteScalar

        If c > 0 Then
            'Dim k As Integer = MsgBox("EWB Has been Generated already for this Invoice. Do you want to Delete the Previous IRN ?", vbYesNo, "IRN Generated")
            'If k = vbNo Then
            MsgBox("Cannot Create a New EWB When there is an EWB generated already and/or an IRN has not been generated!", vbOKOnly, "Duplicate EWB ")
            Exit Sub
            'Else
            'End If
        End If

        Dim tr As SqlClient.SqlTransaction

        tr = con.BeginTransaction
        Cmd.Transaction = tr

        Try

            Cmd.CommandText = "Delete from EWB_By_IRN  where InvCode = '" & NewCode & "'"
            Cmd.ExecuteNonQuery()


            Cmd.CommandText = "Insert into EWB_By_IRN  (	[IRN]                ,	[TransID]        ,	[TransMode] ,	[TransDocNo] ,[TransDocDate]  ,	[VehicleNo]  , [Distance],	[VehType]  ,	[TransName]         , [InvCode]   , Company_Idno   , Company_Pincode  , Shipped_To_Idno ,  Shipped_To_Pincode )   Select A.E_Invoice_IRNO  ,  t.Ledger_GSTINNo,        '1'    ,        ''   ,  ''     ,       a.Vechile_No     ,  L.Distance ,      'R'    ,  t.Ledger_Mainname     ,'" & NewCode & "' , a.Company_Idno  , tz.Company_Pincode  ,  a.Ledger_idno ,  L.Pincode    " &
                                                       " from Bobin_Sales_Head a INNER JOIN Company_Head tz on tz.Company_idno = a.Company_Idno INNER JOIN Ledger_Head L on a.Ledger_IdNo = L.Ledger_IdNo LEFT OUTER JOIN Ledger_Head T on a.Transport_IdNo = T.Ledger_IdNo  Where a.Bobin_Sales_Code = '" & NewCode & "'"

            Cmd.ExecuteNonQuery()

            tr.Commit()

            'Exit Sub

            'rtbeInvoiceResponse.Text = einv.AuthTokenReturnMsg

        Catch ex As Exception

            tr.Rollback()
            MsgBox(ex.Message + " Cannot Generate IRN.", vbOKOnly, "Error !")

            Exit Sub

        End Try

        btn_Generate_EWB_IRN.Enabled = False


        Dim einv As New eInvoice(Val(lbl_Company.Tag))
        einv.GenerateEWBByIRN(NewCode, rtbeInvoiceResponse, txt_eWayBill_No, txt_EWB_Date, txt_EWB_ValidUpto, con, "Bobin_Sales_Head", "Bobin_Sales_Code", txt_EWB_Canellation_Reason, txt_EWB_Cancel_Status, Pk_Condition)

        Cmd.CommandText = "DELETE FROM EWB_By_IRN WHERE INVCODE = '" & NewCode & "'"
        Cmd.ExecuteNonQuery()
    End Sub

    Private Sub btn_Print_EWB_Click(sender As Object, e As EventArgs) Handles btn_Print_EWB.Click
        Dim ewb As New EWB(Val(lbl_Company.Tag))
        EWB.PrintEWB(txt_eWayBill_No.Text, rtbeInvoiceResponse, 0, txt_eInvoiceNo.Text)
    End Sub

    Private Sub btn_Detail_PRINT_EWB_Click(sender As Object, e As EventArgs) Handles btn_Detail_PRINT_EWB.Click
        Dim ewb As New EWB(Val(lbl_Company.Tag))
        EWB.PrintEWB(txt_eWayBill_No.Text, rtbeInvoiceResponse, 1, Trim(txt_eInvoiceNo.Text))
    End Sub

    Private Sub btn_Cancel_EWB_Click(sender As Object, e As EventArgs) Handles btn_Cancel_EWB.Click
        If Len(Trim(txt_EWB_Canellation_Reason.Text)) = 0 Then
            MsgBox("Please provode the reason for cancellation", vbOKCancel, "Provide Reason !")
            Exit Sub
        End If

        Dim NewCode As String = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()

        Dim Ewb As New EWB(Val(lbl_Company.Tag))

        EWB.CancelEWB(txt_eWayBill_No.Text, NewCode, con, rtbeInvoiceResponse, txt_eWayBill_No, "Bobin_Sales_Head", "EWB_No", "Bobin_Sales_Code")
    End Sub


    Private Sub Printing_Delivery_Format_GST2(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim p2Font As Font
        'Dim ps As Printing.PaperSize
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
        Dim ps As Printing.PaperSize
        Dim ItmNm1 As String, ItmNm2 As String
        Dim SNo As Integer
        Dim vLine_Pen As Pen

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                Exit For
            End If
        Next

        'Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8X6", 800, 600)
        'PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
        'PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 30 ' 30
            .Right = 50
            .Top = 20 ' 30
            .Bottom = 30
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        p2Font = New Font("Calibri", 11, FontStyle.Regular)

        e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        With PrintDocument1.DefaultPageSettings.PaperSize
            PrintWidth = .Width - RMargin - LMargin
            PrintHeight = .Height - TMargin - BMargin
            PageWidth = .Width - RMargin
            PageHeight = .Height - BMargin
        End With

        NoofItems_PerPage = 12   ' 7

        Erase LnAr
        Erase ClAr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}


        ClAr(1) = 35        'SNO
        ClAr(2) = 100       'PARTICULARS

        ClAr(3) = 145        'ENDS
        ClAr(4) = 55       'Bobins
        ClAr(5) = 75       'Meter_Bobin
        ClAr(6) = 0       'Meters

        ClAr(7) = 77        'HSN CODE
        ClAr(8) = 43        'GST %

        ClAr(9) = 60       'REEL
        ClAr(10) = 55     'RATE
        ClAr(11) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10)) 'AMOUNT

        TxtHgt = 17.75 ' 18

        EntryCode = Trim((Pk_Condition)) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        vLine_Pen = New Pen(Color.Black, 2)

        Try
            If prn_HdDt.Rows.Count > 0 Then

                Printing_Delivery_Format_GST2_PageHeader(e, EntryCode, TxtHgt, p2Font, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr, vLine_Pen)

                NoofDets = 0

                CurY = CurY - 10

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then

                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued...", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, p2Font)

                            NoofDets = NoofDets + 1

                            Printing_Delivery_Format_GST2_PageFooter(e, EntryCode, TxtHgt, p2Font, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, False)

                            NoofDets = 0
                            e.HasMorePages = True

                            Return

                        End If

                        ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Colour_name").ToString)
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
                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(SNo)), LMargin + 10, CurY, 0, 0, p2Font)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClAr(1) + 10, CurY, 0, 0, p2Font)
                        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("HSN_Code").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 2, CurY, 1, 0,  p2Font)
                        'Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("GST_Percentage").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 5, CurY, 1, 0,  p2Font)

                        'Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("EndscOUNT_Name").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 5, CurY, 0, 0,  p2Font)
                        'Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Bobins").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 5, CurY, 1, 0,  p2Font)
                        'Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Meter_Bobin").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 5, CurY, 1, 0,  p2Font)
                        '' Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Meters").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 5, CurY, 1, 0,  p2Font)

                        '----------
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Ends").ToString, LMargin + ClAr(1) + ClAr(2) + 5, CurY, 0, 0, p2Font)
                        Common_Procedures.Print_To_PrintDocument(e, (Val(prn_DetDt.Rows(prn_DetIndx).Item("Bobins").ToString)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 5, CurY, 1, 0, p2Font)

                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Meter_Bobin").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 5, CurY, 1, 0, p2Font)

                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("HSN_Code").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 5, CurY, 1, 0, p2Font)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("GST_Percentage").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 3, CurY, 1, 0, p2Font)

                        '----------
                        p2Font = New Font("Calibri", 11, FontStyle.Bold)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("REEL").ToString), "########0"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 5, CurY, 1, 0, p2Font)
                        p2Font = New Font("Calibri", 11, FontStyle.Regular)

                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Rate").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) - 5, CurY, 1, 0, p2Font)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) - 5, CurY, 1, 0, p2Font)


                        NoofDets = NoofDets + 1

                        If Trim(ItmNm2) <> "" Then
                            CurY = CurY + TxtHgt - 5
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClAr(1) + 10, CurY, 0, 0, p2Font)
                            NoofDets = NoofDets + 1
                        End If

                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If

                Printing_Delivery_Format_GST2_PageFooter(e, EntryCode, TxtHgt, p2Font, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True)

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

    Private Sub Printing_Delivery_Format_GST2_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal p2Font As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal vLine_Pen As Pen)
        Dim CurX As Single = 0, CurY1 As Single = 0, CurY2 As Single = 0
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim strHeight As Single = 0, strWidth As Single = 0
        Dim C1 As Single, W1, W2, W3 As Single, S1, S2, S3 As Single
        Dim Cmp_Name, Desc As String, Cmp_Add1 As String, Cmp_Add2 As String, Cmp_PanNo As String, Cmp_PanCap As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_EMail As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim S As String
        Dim Inv_No As String = ""
        Dim InvSubNo As String = ""
        Dim ItmNm1 As String = ""
        Dim ItmNm2 As String = ""
        Dim ItmNm3 As String = ""
        Dim I As Integer = 0
        Dim br2 As SolidBrush

        vLine_Pen = New Pen(Color.Black, 2)

        PageNo = PageNo + 1
        prn_Count = prn_Count + 1

        prn_OriDupTri = ""
        If Trim(prn_InpOpts) <> "" Then
            If prn_Count <= Len(Trim(prn_InpOpts)) Then

                S = Mid$(Trim(prn_InpOpts), prn_Count, 1)

                If Val(S) = 1 Then
                    prn_OriDupTri = "ORIGINAL FOR RECEIPIENT"
                ElseIf Val(S) = 2 Then
                    prn_OriDupTri = "DUPLICATE FOR TRANSPORTER"
                ElseIf Val(S) = 3 Then
                    prn_OriDupTri = "TRIPLICATE FOR SUPPLIER"
                ElseIf Val(S) = 4 Then
                    prn_OriDupTri = "EXTRA COPY"
                Else
                    If Trim(prn_InpOpts) <> "0" And Val(prn_InpOpts) = "0" And Len(Trim(prn_InpOpts)) > 3 Then
                        prn_OriDupTri = Trim(prn_InpOpts)
                    End If
                End If

            End If
        End If



        CurY = TMargin + 2

        p1Font = New Font("Calibri", 14, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "TAX INVOICE", LMargin, CurY - TxtHgt - 5, 2, PrintWidth, p1Font)






        '  End If
        If Trim(prn_OriDupTri) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), PageWidth - 10, CurY - TxtHgt, 1, 0, p2Font)
        End If




        'If Trim(prn_HdDt.Rows(0).Item("E_Invoice_IRNO").ToString) <> "" Then

        '    Dim vIRN_Width As Single = 0

        '    vIRN_Width = e.Graphics.MeasureString("Ack.  Date    ", p1Font).Width

        '    ItmNm1 = Trim(prn_HdDt.Rows(0).Item("E_Invoice_IRNO").ToString)
        '    ItmNm2 = ""
        '    ItmNm3 = ""
        '    If Len(ItmNm1) > 50 Then
        '        For I = 50 To 1 Step -1
        '            If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
        '        Next I
        '        If I = 0 Then I = 50
        '        ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
        '        ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I)
        '    End If
        '    If Len(ItmNm2) > 50 Then
        '        For I = 50 To 1 Step -1
        '            If Mid$(Trim(ItmNm2), I, 1) = " " Or Mid$(Trim(ItmNm2), I, 1) = "," Or Mid$(Trim(ItmNm2), I, 1) = "." Or Mid$(Trim(ItmNm2), I, 1) = "-" Or Mid$(Trim(ItmNm2), I, 1) = "/" Or Mid$(Trim(ItmNm2), I, 1) = "_" Or Mid$(Trim(ItmNm2), I, 1) = "(" Or Mid$(Trim(ItmNm2), I, 1) = ")" Or Mid$(Trim(ItmNm2), I, 1) = "\" Or Mid$(Trim(ItmNm2), I, 1) = "[" Or Mid$(Trim(ItmNm2), I, 1) = "]" Or Mid$(Trim(ItmNm2), I, 1) = "{" Or Mid$(Trim(ItmNm2), I, 1) = "}" Then Exit For
        '        Next I
        '        If I = 0 Then I = 50
        '        ItmNm3 = Microsoft.VisualBasic.Right(Trim(ItmNm2), Len(ItmNm2) - I)
        '        ItmNm2 = Microsoft.VisualBasic.Left(Trim(ItmNm2), I)
        '    End If


        '    p1Font = New Font("Calibri", 10, FontStyle.Bold)
        '    Common_Procedures.Print_To_PrintDocument(e, "IRN No", LMargin + 10, CurY + 5, 0, 0, p1Font, br2)
        '    Common_Procedures.Print_To_PrintDocument(e, ": ", LMargin + vIRN_Width, CurY + 5, 0, 0, p1Font, br2)
        '    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + vIRN_Width + 10, CurY + 5, 0, 0, p1Font, br2)


        '    If Trim(ItmNm2) <> "" Then
        '        CurY = CurY + TxtHgt
        '        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + vIRN_Width + 10, CurY + 5, 0, 0, p1Font, br2)
        '    End If


        '    If Trim(prn_HdDt.Rows(0).Item("E_Invoice_IRNO").ToString) <> "" Then
        '        If IsDBNull(prn_HdDt.Rows(0).Item("E_Invoice_QR_Image")) = False Then
        '            Dim imageData As Byte() = DirectCast(prn_HdDt.Rows(0).Item("E_Invoice_QR_Image"), Byte())
        '            If Not imageData Is Nothing Then
        '                Using ms As New MemoryStream(imageData, 0, imageData.Length)
        '                    ms.Write(imageData, 0, imageData.Length)

        '                    If imageData.Length > 0 Then

        '                        pic_IRN_QRCode_Image_forPrinting.BackgroundImage = Image.FromStream(ms)

        '                        e.Graphics.DrawImage(DirectCast(pic_IRN_QRCode_Image_forPrinting.BackgroundImage, Drawing.Image), PageWidth - 90, CurY, 80, 80)
        '                    End If

        '                End Using

        '            End If

        '        End If

        '    End If

        '    CurY = CurY + TxtHgt
        '    Common_Procedures.Print_To_PrintDocument(e, "Ack. No  ", LMargin + 10, CurY + 5, 0, 0, p1Font, br2)
        '    Common_Procedures.Print_To_PrintDocument(e, ": ", LMargin + vIRN_Width, CurY + 5, 0, 0, p1Font, br2)
        '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("E_Invoice_ACK_No").ToString, LMargin + vIRN_Width + 10, CurY + 5, 0, 0, p1Font, br2)

        '    CurY = CurY + TxtHgt

        '    Common_Procedures.Print_To_PrintDocument(e, "ACK Date", LMargin + 10, CurY + 5, 0, 0, p1Font, br2)
        '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + vIRN_Width, CurY + 5, 0, 0, p1Font, br2)
        '    If Trim(prn_HdDt.Rows(0).Item("E_Invoice_ACK_Date").ToString) <> "" Then
        '        If IsDate(prn_HdDt.Rows(0).Item("E_Invoice_ACK_Date").ToString) = True Then
        '            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("E_Invoice_ACK_Date").ToString), "dd-MM-yyyy hh:mm tt").ToString, LMargin + vIRN_Width + 10, CurY + 5, 0, 0, p1Font, br2)
        '        End If
        '    End If

        '    CurY = CurY + TxtHgt


        '    Common_Procedures.Print_To_PrintDocument(e, "E-way Bill No ", LMargin + 10, CurY + 5, 0, 0, p1Font, br2)
        '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + vIRN_Width, CurY + 5, 0, 0, p1Font, br2)
        '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("EWB_No").ToString, LMargin + vIRN_Width + 10, CurY + 5, 0, 0, p1Font, br2)

        '    CurY = CurY + TxtHgt
        '    CurY = CurY + TxtHgt
        '    CurY = CurY + TxtHgt


        'End If

        e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
        'e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY
        Desc = ""
        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_PanNo = "" : Cmp_EMail = "" : Cmp_PanCap = ""
        Cmp_StateCap = "" : Cmp_StateNm = "" : Cmp_StateCode = "" : Cmp_GSTIN_Cap = "" : Cmp_GSTIN_No = ""

        Desc = prn_HdDt.Rows(0).Item("Company_Description").ToString
        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
        Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString

        If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "Phone : " & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
            Cmp_TinNo = "TIN NO : " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString) <> "" Then
            Cmp_CstNo = "CST NO : " & prn_HdDt.Rows(0).Item("Company_CstNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_PanNo").ToString) <> "" Then
            Cmp_PanCap = "PAN : "
            Cmp_PanNo = prn_HdDt.Rows(0).Item("Company_PanNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
            Cmp_EMail = "Email : " & prn_HdDt.Rows(0).Item("Company_EMail").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_State_Name").ToString) <> "" Then
            Cmp_StateCap = "STATE : "
            Cmp_StateNm = prn_HdDt.Rows(0).Item("Company_State_Name").ToString
            If Trim(prn_HdDt.Rows(0).Item("Company_State_Code").ToString) <> "" Then
                Cmp_StateNm = Cmp_StateNm & "   CODE : " & prn_HdDt.Rows(0).Item("Company_State_Code").ToString
            End If
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString) <> "" Then
            Cmp_GSTIN_Cap = "GSTIN : "
            Cmp_GSTIN_No = prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
        End If




        'p1Font = New Font("Calibri", 14, FontStyle.Bold)
        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1018" Then '---- M.K Textiles (Palladam)
        '    p1Font = New Font("Calibri", 12, FontStyle.Bold)
        '    Common_Procedures.Print_To_PrintDocument(e, "INVOICE", LMargin, CurY, 2, PrintWidth, p1Font)
        'End If

        CurY = CurY + TxtHgt - 15
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


        p1Font = New Font("Calibri", 18, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height



        If Trim(prn_HdDt.Rows(0).Item("E_Invoice_IRNO").ToString) <> "" Then
            If IsDBNull(prn_HdDt.Rows(0).Item("E_Invoice_QR_Image")) = False Then
                Dim imageData As Byte() = DirectCast(prn_HdDt.Rows(0).Item("E_Invoice_QR_Image"), Byte())
                If Not imageData Is Nothing Then
                    Using ms As New MemoryStream(imageData, 0, imageData.Length)
                        ms.Write(imageData, 0, imageData.Length)

                        If imageData.Length > 0 Then

                            pic_IRN_QRCode_Image_forPrinting.BackgroundImage = Image.FromStream(ms)

                            e.Graphics.DrawImage(DirectCast(pic_IRN_QRCode_Image_forPrinting.BackgroundImage, Drawing.Image), PageWidth - 90, CurY, 80, 80)
                        End If

                    End Using

                End If

            End If

        End If



        CurY = CurY + strHeight - 7
        If Desc <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, Desc, LMargin, CurY, 2, PrintWidth, p2Font)
        End If

        strWidth = e.Graphics.MeasureString(Trim(Cmp_Add1 & " " & Cmp_Add2), p1Font).Width
        If PrintWidth > strWidth Then
            If Trim(Cmp_Add1 & " " & Cmp_Add2) <> "" Then
                CurY = CurY + TxtHgt - 1
                Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_Add1 & " " & Cmp_Add2), LMargin, CurY, 2, PrintWidth, p2Font)
            End If

            NoofItems_PerPage = NoofItems_PerPage - 1

        Else

            If Cmp_Add1 <> "" Then
                CurY = CurY + TxtHgt - 1
                Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, p2Font)
            End If
            If Cmp_Add2 <> "" Then
                CurY = CurY + TxtHgt - 1
                Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, p2Font)
            End If

        End If


        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PhNo & " / " & Cmp_EMail), LMargin, CurY, 2, PrintWidth, p2Font)

        CurY = CurY + TxtHgt

        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_GSTIN_Cap), p1Font).Width
        strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_StateNm & "     " & Cmp_GSTIN_Cap & Cmp_GSTIN_No & "     " & Cmp_PanCap & Cmp_PanNo), p2Font).Width
        If PrintWidth > strWidth Then
            CurX = LMargin + (PrintWidth - strWidth) / 2
        Else
            CurX = LMargin
        End If

        p1Font = New Font("Calibri", 10, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_StateCap, CurX, CurY, 0, 0, p1Font)
        strWidth = e.Graphics.MeasureString(Cmp_StateCap, p1Font).Width
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, Cmp_StateNm, CurX, CurY, 0, 0, p2Font)

        strWidth = e.Graphics.MeasureString(Cmp_StateNm, p2Font).Width
        p1Font = New Font("Calibri", 10, FontStyle.Bold)
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_GSTIN_Cap, CurX, CurY, 0, PrintWidth, p1Font)
        strWidth = e.Graphics.MeasureString("     " & Cmp_GSTIN_Cap, p1Font).Width
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, CurX, CurY, 0, 0, p2Font)

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1044" Then
            strWidth = e.Graphics.MeasureString(Cmp_GSTIN_No, p2Font).Width
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            CurX = CurX + strWidth
            Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_PanCap, CurX, CurY, 0, PrintWidth, p1Font)
            strWidth = e.Graphics.MeasureString("     " & Cmp_PanCap, p1Font).Width
            CurX = CurX + strWidth
            Common_Procedures.Print_To_PrintDocument(e, Cmp_PanNo, CurX, CurY, 0, 0, p2Font)
        End If


        If Trim(prn_HdDt.Rows(0).Item("E_Invoice_IRNO").ToString) <> "" Then
            ItmNm1 = Trim(prn_HdDt.Rows(0).Item("E_Invoice_IRNO").ToString)

            ItmNm2 = ""
            If Len(ItmNm1) > 35 Then
                For I = 35 To 1 Step -1
                    If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                Next I
                If I = 0 Then I = 35

                ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
            End If

            CurY = CurY + TxtHgt + 2

            e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "IRN : " & Trim(ItmNm1), LMargin, CurY, 0, 0, p1Font, br2)
            Common_Procedures.Print_To_PrintDocument(e, "Ack. No : " & prn_HdDt.Rows(0).Item("E_Invoice_ACK_No").ToString, PrintWidth - 10, CurY, 1, 0, p1Font, br2)

            If Trim(ItmNm2) <> "" Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "          " & Trim(ItmNm2), LMargin, CurY, 0, 0, p1Font)
                Common_Procedures.Print_To_PrintDocument(e, "Ack. Date : " & prn_HdDt.Rows(0).Item("E_Invoice_ACK_Date").ToString, PrintWidth - 10, CurY, 1, 0, p1Font, br2)
            End If


        End If


        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 30
        W1 = e.Graphics.MeasureString("DATE & TIME OF   SUPPLY ", p2Font).Width
        S1 = e.Graphics.MeasureString("TO", p2Font).Width  ' e.Graphics.MeasureString("Details of Receiver | Billed to     :",  p2Font).Width

        W2 = e.Graphics.MeasureString("DESPATCH   TO   : ", p2Font).Width
        S2 = e.Graphics.MeasureString("TRANSPORTATION   MODE", p2Font).Width

        W3 = e.Graphics.MeasureString("INVOICE   DATE", p2Font).Width
        S3 = e.Graphics.MeasureString("REVERSE CHARGE   (YES/NO) ", p2Font).Width

        CurY = CurY + 10
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "INVOICE NO", LMargin + 10, CurY, 0, 0, p2Font)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W3 + 10, CurY, 0, 0, p2Font)


        If prn_HdDt.Rows(0).Item("Invoice_PrefixNo").ToString <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Invoice_PrefixNo").ToString & "-" & prn_HdDt.Rows(0).Item("Bobin_Sales_No").ToString, LMargin + W3 + 30, CurY, 0, 0, p1Font)
        Else
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Bobin_Sales_No").ToString, LMargin + W3 + 30, CurY, 0, 0, p1Font)
        End If


        Common_Procedures.Print_To_PrintDocument(e, "REVERSE CHARGE (YES/NO)", LMargin + C1 + 10, CurY, 0, 0, p2Font)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S3 + 10, CurY, 0, 0, p2Font)
        Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin + C1 + S3 + 30, CurY, 0, 0, p2Font)


        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "INVOICE DATE", LMargin + 10, CurY, 0, 0, p2Font)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W3 + 10, CurY, 0, 0, p2Font)
        p2Font = New Font("Calibri", 11, FontStyle.Regular)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Bobin_Sales_Date").ToString), "dd-MM-yyyy").ToString, LMargin + W3 + 30, CurY, 0, 0, p1Font)

        If Trim(prn_HdDt.Rows(0).Item("EWB_No").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "EWAYBILL NO ", LMargin + C1 + 10, CurY, 0, 0, p2Font)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S3 + 10, CurY, 0, 0, p2Font)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("EWB_No").ToString, LMargin + C1 + S3 + 30, CurY, 0, 0, p2Font)
        End If

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
        'LnAr(2) = CurY

        CurY1 = CurY
        CurY2 = CurY

        '---left side

        CurY1 = CurY1 + 10
        p1Font = New Font("Calibri", 10, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "DETAILS OF RECEIVER (BILLED TO) :", LMargin + 10, CurY1, 0, 0, p1Font)

        strHeight = e.Graphics.MeasureString("A", p1Font).Height
        CurY1 = CurY1 + strHeight
        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "M/s. " & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + S1 + 10, CurY1, 0, 0, p1Font)

        CurY1 = CurY1 + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY1, 0, 0, p2Font)

        CurY1 = CurY1 + TxtHgt
        If Trim(prn_HdDt.Rows(0).Item("Ledger_Address2").ToString) <> "" Then

            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY1, 0, 0, p2Font)
        End If
        CurY1 = CurY1 + TxtHgt
        If Trim(prn_HdDt.Rows(0).Item("Ledger_Address3").ToString) <> "" Then

            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY1, 0, 0, p2Font)
        End If

        If Trim(prn_HdDt.Rows(0).Item("Ledger_Address4").ToString) <> "" Then
            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY1, 0, 0, p2Font)
        End If
        CurY1 = CurY1 + TxtHgt
        If Trim(prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString) <> "" Then
            '  CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString & "  CODE : " & prn_HdDt.Rows(0).Item("Ledger_State_Code").ToString, LMargin + S1 + 10, CurY1, 0, 0, p2Font)
        End If

        CurY1 = CurY1 + TxtHgt
        If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "GSTIN : " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + S1 + 10, CurY1, 0, 0, p2Font)
        End If
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1044" Then
            If Trim(prn_HdDt.Rows(0).Item("Pan_No").ToString) <> "" Then
                strWidth = e.Graphics.MeasureString("GSTIN : " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, p2Font).Width
                CurX = LMargin + S1 + 10 + strWidth
                Common_Procedures.Print_To_PrintDocument(e, "      PAN : " & prn_HdDt.Rows(0).Item("Pan_No").ToString, CurX, CurY1, 0, PrintWidth, p2Font)
            End If
        End If



        '--Right Side

        CurY2 = CurY2 + 10
        p1Font = New Font("Calibri", 10, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "DETAILS OF CONSIGNEE (SHIPPED TO) :", LMargin + C1 + 10, CurY2, 0, 0, p1Font)

        strHeight = e.Graphics.MeasureString("A", p1Font).Height
        CurY2 = CurY2 + strHeight
        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "M/s. " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerName").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, p1Font)

        p2Font = New Font("Calibri", 10, FontStyle.Regular)
        CurY2 = CurY2 + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress1").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, p2Font)
        CurY2 = CurY2 + TxtHgt
        If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress2").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress2").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, p2Font)
        End If
        CurY2 = CurY2 + TxtHgt
        If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress3").ToString) <> "" Then
            ' CurY2 = CurY2 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress3").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, p2Font)
        End If

        If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress4").ToString) <> "" Then
            CurY2 = CurY2 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress4").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, p2Font)
        End If
        CurY2 = CurY2 + TxtHgt
        If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_State_Name").ToString) <> "" Then
            ' CurY2 = CurY2 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_State_Name").ToString & "  CODE : " & prn_HdDt.Rows(0).Item("DeliveryTo_State_Code").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, p2Font)
        End If

        CurY2 = CurY2 + TxtHgt
        If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerGSTinNo").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "GSTIN : " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerGSTinNo").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, p2Font)
        End If
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1044" Then
            If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_PanNo").ToString) <> "" Then
                strWidth = e.Graphics.MeasureString("GSTIN : " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerGSTinNo").ToString, p2Font).Width
                CurX = LMargin + C1 + S1 + 10 + strWidth
                Common_Procedures.Print_To_PrintDocument(e, "      PAN : " & prn_HdDt.Rows(0).Item("DeliveryTo_PanNo").ToString, CurX, CurY2, 0, PrintWidth, p2Font)
            End If
        End If




        CurY = IIf(CurY1 > CurY2, CurY1, CurY2)


        CurY = CurY + TxtHgt

        e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
        e.Graphics.DrawLine(vLine_Pen, LMargin + C1, CurY, LMargin + C1, LnAr(2))
        LnAr(3) = CurY



        W2 = e.Graphics.MeasureString("DOCUMENT THROUGH   : ", p2Font).Width
        S2 = e.Graphics.MeasureString("DATE & TIME OF SUPPLY  :", p2Font).Width

        '--Right Side
        CurY = CurY + 10
        Common_Procedures.Print_To_PrintDocument(e, "TRANSPORT ", LMargin + 10, CurY, 0, 0, p2Font)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 10, CurY, 0, 0, p2Font)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("TransportName").ToString, LMargin + W2 + 30, CurY, 0, 0, p2Font)


        Common_Procedures.Print_To_PrintDocument(e, "VEHICLE NO", LMargin + C1 + 10, CurY, 0, 0, p2Font)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S2 + 10, CurY, 0, 0, p2Font)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vechile_No").ToString, LMargin + C1 + S2 + 30, CurY, 0, 0, p2Font)


        'CurY = CurY + TxtHgt
        'Common_Procedures.Print_To_PrintDocument(e, "ORDER NO", LMargin + 10, CurY, 0, 0,  p2Font)
        'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 10, CurY, 0, 0,  p2Font)
        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Party_OrderNo").ToString, LMargin + W2 + 30, CurY, 0, 0,  p2Font)
        'If Trim(prn_HdDt.Rows(0).Item("Party_OrderDate").ToString) <> "" Then
        '    strWidth = e.Graphics.MeasureString(prn_HdDt.Rows(0).Item("Party_OrderNo").ToString,  p2Font).Width
        '    Common_Procedures.Print_To_PrintDocument(e, "Date : " & prn_HdDt.Rows(0).Item("Party_OrderDate").ToString, LMargin + W2 + strWidth + 60, CurY, 0, 0,  p2Font)
        'End If



        'CurY = CurY + TxtHgt
        'Common_Procedures.Print_To_PrintDocument(e, "DC NO", LMargin + 10, CurY, 0, 0,  p2Font)
        'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 10, CurY, 0, 0,  p2Font)
        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Dc_No").ToString, LMargin + W2 + 30, CurY, 0, 0,  p2Font)
        'If Trim(prn_HdDt.Rows(0).Item("Dc_Date").ToString) <> "" Then
        '    strWidth = e.Graphics.MeasureString(prn_HdDt.Rows(0).Item("Dc_No").ToString,  p2Font).Width
        '    Common_Procedures.Print_To_PrintDocument(e, "Date : " & prn_HdDt.Rows(0).Item("Dc_Date").ToString, LMargin + strWidth + W1 + 60, CurY, 0, 0,  p2Font)
        'End If

        'Common_Procedures.Print_To_PrintDocument(e, "TRANSPORT NAME", LMargin + C1 + 10, CurY, 0, 0,  p2Font)
        'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S2 + 10, CurY, 0, 0,  p2Font)
        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("TransportName").ToString, LMargin + C1 + S2 + 30, CurY, 0, 0,  p2Font)


        '' CurY = CurY + TxtHgt
        'Common_Procedures.Print_To_PrintDocument(e, "TRANSPORTATION MODE", LMargin + C1 + 10, CurY, 0, 0,  p2Font)
        'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S2 + 10, CurY, 0, 0,  p2Font)
        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Transport_Mode").ToString, LMargin + C1 + S2 + 30, CurY, 0, 0,  p2Font)
        ''Common_Procedures.Print_To_PrintDocument(e, "TRANSPORTATION MODE", LMargin + 10, CurY, 0, 0,  p2Font)
        ''Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 10, CurY, 0, 0,  p2Font)
        ''Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Transport_Mode").ToString, LMargin + W2 + 30, CurY, 0, 0,  p2Font)


        ''Common_Procedures.Print_To_PrintDocument(e, "DATE & TIME OF SUPPLY", LMargin + C1 + 10, CurY, 0, 0,  p2Font)
        ''Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S2 + 10, CurY, 0, 0,  p2Font)
        ''Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Date_And_Time_Of_Supply").ToString, LMargin + C1 + S2 + 30, CurY, 0, 0,  p2Font)

        'CurY = CurY + TxtHgt
        'Common_Procedures.Print_To_PrintDocument(e, "DOCUMENT THROUGH", LMargin + 10, CurY, 0, 0,  p2Font)
        'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 10, CurY, 0, 0,  p2Font)
        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Through_Name").ToString, LMargin + W2 + 30, CurY, 0, 0,  p2Font)

        'Common_Procedures.Print_To_PrintDocument(e, "PLACE OF SUPPLY", LMargin + C1 + 10, CurY, 0, 0,  p2Font)
        'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S2 + 10, CurY, 0, 0,  p2Font)
        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_State_Name").ToString, LMargin + C1 + S2 + 30, CurY, 0, 0,  p2Font)


        'If Trim(prn_HdDt.Rows(0).Item("Lc_No").ToString) <> "" Then
        '    CurY = CurY + TxtHgt
        '    Common_Procedures.Print_To_PrintDocument(e, "LC NO", LMargin + 10, CurY, 0, 0,  p2Font)
        '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 10, CurY, 0, 0,  p2Font)
        '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Lc_No").ToString, LMargin + W2 + 30, CurY, 0, 0,  p2Font)
        '    If Trim(prn_HdDt.Rows(0).Item("Lc_Date").ToString) <> "" Then
        '        strWidth = e.Graphics.MeasureString(prn_HdDt.Rows(0).Item("Lc_No").ToString,  p2Font).Width
        '        Common_Procedures.Print_To_PrintDocument(e, "Date : " & prn_HdDt.Rows(0).Item("Lc_Date").ToString, LMargin + strWidth + W2 + 60, CurY, 0, 0,  p2Font)
        '    End If
        'End If

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))

        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), p2Font)

        Common_Procedures.Print_To_PrintDocument(e, "PARTICULARS", LMargin + ClAr(1), CurY, 2, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), p2Font)

        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY + TxtHgt, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY + TxtHgt)
        LnAr(10) = CurY
        CurY = CurY + TxtHgt + 10
        Common_Procedures.Print_To_PrintDocument(e, "COLOUR", LMargin + ClAr(1), CurY, 2, ClAr(2), p2Font)

        'Common_Procedures.Print_To_PrintDocument(e, "HSN CODE", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3),  p2Font)

        'Common_Procedures.Print_To_PrintDocument(e, "GST %", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4),  p2Font)

        'Common_Procedures.Print_To_PrintDocument(e, "ENDS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5),  p2Font)

        'Common_Procedures.Print_To_PrintDocument(e, "BOBINS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6),  p2Font)

        'Common_Procedures.Print_To_PrintDocument(e, "METER/", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7),  p2Font)
        'Common_Procedures.Print_To_PrintDocument(e, "BOBINS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY + TxtHgt, 2, ClAr(7),  p2Font)

        '---------
        Common_Procedures.Print_To_PrintDocument(e, "ENDS ", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), p2Font)

        Common_Procedures.Print_To_PrintDocument(e, "BOBINS ", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), p2Font)

        Common_Procedures.Print_To_PrintDocument(e, "METER/", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), p2Font)
        Common_Procedures.Print_To_PrintDocument(e, "BOBINS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY + TxtHgt, 2, ClAr(5), p2Font)

        CurY = CurY - 20

        Common_Procedures.Print_To_PrintDocument(e, "HSN CODE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), p2Font)

        Common_Procedures.Print_To_PrintDocument(e, "GST %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), p2Font)
        '---------

        Common_Procedures.Print_To_PrintDocument(e, "TOTAL/", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), p2Font)
        Common_Procedures.Print_To_PrintDocument(e, "REEL", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY + TxtHgt, 2, ClAr(9), p2Font)

        Common_Procedures.Print_To_PrintDocument(e, "RATE/", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, 2, ClAr(10), p2Font)
        Common_Procedures.Print_To_PrintDocument(e, "REEL", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY + TxtHgt, 2, ClAr(10), p2Font)

        Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, 2, ClAr(11), p2Font)

        CurY = CurY + TxtHgt + TxtHgt + 20
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY

    End Sub

    Private Sub Printing_Delivery_Format_GST2_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal p2Font As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim i As Integer
        Dim Cmp_Name As String
        Dim W1 As Single = 0
        Dim C1 As Single = 0
        Dim ItmNm1 As String = ""
        Dim s2 As Single = 0
        Dim vprn_BlNos As String = ""
        Dim SubClAr(15) As Single
        Dim p1Font As Font, p3Font As Font
        Dim rndoff As Double, TtAmt As Double
        Dim BInc As Integer
        Dim BnkDetAr() As String
        Dim BmsInWrds As String
        Dim BankNm1 As String = ""
        Dim BankNm2 As String = ""
        Dim BankNm3 As String = ""
        Dim BankNm4 As String = ""
        Dim CurY1 As Single = 0
        Dim vNoofHsnCodes As Integer = 0
        Dim vTaxPerc As Single = 0

        For i = NoofDets + 1 To NoofItems_PerPage
            CurY = CurY + TxtHgt
        Next

        C1 = ClAr(1) + ClAr(2) + ClAr(3) - 30
        W1 = e.Graphics.MeasureString("TOTAL BOBIN : ", p2Font).Width
        'w2 = e.Graphics.MeasureString("DESP.TO : ",  p2Font).Width
        'S1 = e.Graphics.MeasureString("TO  :  ",  p2Font).Width
        s2 = e.Graphics.MeasureString("TOTAL BOBIN :  ", p2Font).Width

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(5) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(10) + 20)
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(10) + 20)
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(10) + 20)
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(10) + 20)
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), LnAr(3))


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

        Balance_Calculation()

        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "TOTAL BOBIN ", LMargin + 10, CurY, 0, 0, p2Font)
        Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + s2, CurY, 0, 0, p2Font)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Total_Bobins").ToString, LMargin + s2 + 10, CurY, 0, 0, p2Font)

        'Common_Procedures.Print_To_PrintDocument(e, "TOTAL METER ", LMargin + ClAr(1) + ClAr(2) + 20, CurY, 0, 0, p2Font)
        'Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + s2 + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, p2Font)
        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Total_Meters").ToString, LMargin + s2 + ClAr(1) + ClAr(2) + 20, CurY, 0, 0, p2Font)

        Common_Procedures.Print_To_PrintDocument(e, "BALANCE BOBIN", LMargin + ClAr(1) + ClAr(2) + 30, CurY, 0, 0, p2Font)
        Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + s2 + ClAr(1) + ClAr(2) + 40, CurY, 0, 0, p2Font)
        Common_Procedures.Print_To_PrintDocument(e, Balance_Bobin, LMargin + s2 + ClAr(1) + ClAr(2) + 50, CurY, 0, 0, p2Font)



        Common_Procedures.Print_To_PrintDocument(e, "TOTAL REEL ", LMargin + C1 + ClAr(4) + 20, CurY, 0, 0, p2Font)
        Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + s2 + C1 + ClAr(4) + 5, CurY, 0, 0, p2Font)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Total_Reels").ToString, LMargin + s2 + C1 + +ClAr(4) + 15, CurY, 0, 0, p2Font)

        CurY1 = CurY + 5
        CurY1 = CurY1 + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY1, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY1)
        CurY1 = CurY1 - 15
        p3Font = New Font("Calibri", 10, FontStyle.Regular)
        If BankNm1 <> "" Then
            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, BankNm1, LMargin + 10, CurY1, 0, 0, p3Font)
        End If
        If BankNm2 <> "" Then
            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, BankNm2, LMargin + 10, CurY1, 0, 0, p3Font)
        End If
        If BankNm3 <> "" Then
            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, BankNm3, LMargin + 10, CurY1, 0, 0, p3Font)
        End If
        If BankNm4 <> "" Then
            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, BankNm4, LMargin + 10, CurY1, 0, 0, p3Font)
        End If
        CurY1 = CurY1 + TxtHgt + 8
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY1, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY1)

        Common_Procedures.Print_To_PrintDocument(e, "TOTAL AMOUNT :   ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 2, CurY, 0, 0, p2Font)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Total_Amount").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) - 10, CurY, 1, 0, p2Font)


        'CurY = CurY + TxtHgt + 10
        If is_LastPage = True Then

            If Val(prn_HdDt.Rows(0).Item("Freight").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Freight", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 2, CurY, 0, 0, p2Font)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Freight").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, p2Font)
            End If
        End If

        If is_LastPage = True Then

            If Val(prn_HdDt.Rows(0).Item("AddLess_BeforeTax").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Add/Less", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 2, CurY, 0, 0, p2Font)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("AddLess_BeforeTax").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, p2Font)
            End If
        End If
        'CurY = CurY + TxtHgt

        CurY = CurY + TxtHgt - 10
        '-------------------------------------------------------------------

        prn_CGST_Amount = prn_HdDt.Rows(0).Item("Total_CGst_Amount").ToString
        prn_SGST_Amount = prn_HdDt.Rows(0).Item("Total_SGst_Amount").ToString
        prn_IGST_Amount = prn_HdDt.Rows(0).Item("Total_IGST_Amount").ToString

        prn_GST_Perc = Val(prn_HdDt.Rows(0).Item("GST_Percentage").ToString)


        If Val(prn_CGST_Amount) <> 0 Or Val(prn_SGST_Amount) <> 0 Or Val(prn_IGST_Amount) <> 0 Then

            If Val(prn_HdDt.Rows(0).Item("Freight").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, PageWidth, CurY)
            Else
                CurY = CurY + 10
            End If

            CurY = CurY + TxtHgt - 10
            If is_LastPage = True Then
                p1Font = New Font("Calibri", 10, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "Taxable Value", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 2, CurY, 0, 0, p2Font)
                '  Common_Procedures.Print_To_PrintDocument(e, "Taxable Value", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, 1, 0,  p2Font)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Taxable_Value").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, p1Font)
            End If
        End If
        CurY = CurY + TxtHgt
        If Val(prn_CGST_Amount) <> 0 Then

            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, "CGST @ " & Trim(Val(prn_GST_Perc / 2)) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 2, CurY, 0, 0, p2Font)

            Else
                Common_Procedures.Print_To_PrintDocument(e, "CGST ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 2, CurY, 0, 0, p2Font)
            End If
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_CGST_Amount), "########0.00"), PageWidth - 10, CurY, 1, 0, p2Font)

        End If
        CurY = CurY + TxtHgt
        If Val(prn_SGST_Amount) <> 0 Then

            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, "SGST @ " & Trim(Val(prn_GST_Perc / 2)) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 2, CurY, 0, 0, p2Font)

            Else
                Common_Procedures.Print_To_PrintDocument(e, "SGST ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 2, CurY, 0, 0, p2Font)
            End If
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_SGST_Amount), "########0.00"), PageWidth - 10, CurY, 1, 0, p2Font)

        End If

        CurY = CurY + TxtHgt
        If Val(prn_IGST_Amount) <> 0 Then

            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, "IGST @ " & Trim(Val(prn_GST_Perc)) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 2, CurY, 0, 0, p2Font)
                ' Common_Procedures.Print_To_PrintDocument(e, "IGST @ " & Trim(Val(prn_GST_Perc)) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, 1, 0,  p2Font)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "IGST ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 2, CurY, 0, 0, p2Font)
            End If
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_IGST_Amount), "########0.00"), PageWidth - 10, CurY, 1, 0, p2Font)

        End If

        '***** GST END *****
        TtAmt = Format(Val(prn_HdDt.Rows(0).Item("Total_Taxable_Value").ToString) + Val(prn_IGST_Amount) + Val(prn_SGST_Amount) + Val(prn_CGST_Amount), "#########0.00")

        rndoff = 0
        rndoff = Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString) - Val(TtAmt)

        CurY = CurY + TxtHgt
        If Val(rndoff) <> 0 Then
            If Val(rndoff) >= 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "ROUND OFF (+) ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 0, 0, p2Font)

                '  Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 20, CurY, 0, 0,  p2Font)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "ROUND OFF (-)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 0, 0, p2Font)

                'Common_Procedures.Print_To_PrintDocument(e, "(-)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 20, CurY, 0, 0,  p2Font)
            End If
            Common_Procedures.Print_To_PrintDocument(e, " " & Format(Math.Abs(Val(rndoff)), "########0.00"), PageWidth - 10, CurY, 1, 0, p2Font)
        End If

        p1Font = New Font("Calibri", 13, FontStyle.Bold)
        CurY = CurY + TxtHgt
        p3Font = New Font("Calibri", 12, FontStyle.Regular)
        Common_Procedures.Print_To_PrintDocument(e, "E & OE", LMargin + 10, CurY, 0, 0, p3Font)

        CurY = CurY + TxtHgt + 8
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(6) = CurY

        CurY = CurY + TxtHgt - 10
        'Common_Procedures.Print_To_PrintDocument(e, "BALANCE BOBIN ", LMargin + 10, CurY, 0, 0,  p2Font)
        'Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + s2 + 30, CurY, 0, 0,  p2Font)
        'Common_Procedures.Print_To_PrintDocument(e, Balance_Bobin, LMargin + s2 + 60, CurY, 0, 0,  p2Font)
        Common_Procedures.Print_To_PrintDocument(e, "NET AMOUNT ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 2, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Net_Amount").ToString, PageWidth - 10, CurY, 1, 0, p1Font)

        'Common_Procedures.Print_To_PrintDocument(e, "BALANCE AMOUNT ", LMargin + C1, CurY, 0, 0,  p2Font)
        'Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + s2 + C1 + 30, CurY, 0, 0,  p2Font)
        'Common_Procedures.Print_To_PrintDocument(e, Balance_Amount, LMargin + s2 + C1 + 70, CurY, 0, 0,  p2Font)

        '  CurY = CurY + TxtHgt

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(9) = CurY
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(4))

        CurY = CurY + 5
        BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))
        BmsInWrds = Replace(Trim(BmsInWrds), "", "")

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1065" Then '---- Logu textile
            BmsInWrds = Trim(UCase(BmsInWrds))
        End If

        p1Font = New Font("Calibri", 10, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "Amount Chargeable(In Words)  : " & BmsInWrds & " ", LMargin + 10, CurY, 0, 0, p1Font)
        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(10) = CurY

        '=============GST SUMMARY============

        '  vNoofHsnCodes = get_GST_Noof_HSN_Codes_For_Printing(EntryCode)

        ''    ' Printing_GST_HSN_Details_Format1(e, EntryCode, TxtHgt, p2Font, LMargin, PageWidth, PrintWidth, CurY, LnAr(10), Pens.Black)



        '==========================

        CurY = CurY + TxtHgt - 15
        p1Font = New Font("Calibri", 9, FontStyle.Underline Or FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "Term & Conditions : ", LMargin + 10, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt

        p2Font = New Font("Webdings", 8, FontStyle.Regular)
        p1Font = New Font("Calibri", 8, FontStyle.Bold)


        ''1
        'If Val(prn_HdDt.Rows(0).Item("Gr_Time").ToString) <> 0 Then
        '    Common_Procedures.Print_To_PrintDocument(e, "=", LMargin + 10, CurY, 0, 0, p2Font)
        '    Common_Procedures.Print_To_PrintDocument(e, "Overdue Interest will be charged at 24% from The  " & Trim(prn_HdDt.Rows(0).Item("Gr_Date").ToString), LMargin + 25, CurY, 0, 0, p1Font)
        'Else
        Common_Procedures.Print_To_PrintDocument(e, "=", LMargin + 10, CurY, 0, 0, p2Font)
        Common_Procedures.Print_To_PrintDocument(e, "Overdue Interest will be charged at 24% from The invoice date. ", LMargin + 25, CurY, 0, 0, p1Font)
        'End If
        '3
        Common_Procedures.Print_To_PrintDocument(e, "=", PrintWidth / 2 + 10, CurY, 0, 0, p2Font)
        Common_Procedures.Print_To_PrintDocument(e, "We will not accept any claim after processing of goods.", PrintWidth / 2 + 25, CurY, 0, 0, p1Font)

        '2
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "=", LMargin + 10, CurY, 0, 0, p2Font)
        Common_Procedures.Print_To_PrintDocument(e, "We are not responsible for any loss or damage in transit.", LMargin + 25, CurY, 0, 0, p1Font)
        '4
        Common_Procedures.Print_To_PrintDocument(e, "=", PrintWidth / 2 + 10, CurY, 0, 0, p2Font)
        Common_Procedures.Print_To_PrintDocument(e, "Subject to " & Trim(Common_Procedures.settings.Jurisdiction) & " jurisdiction. ", PrintWidth / 2 + 25, CurY, 0, 0, p1Font)


        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(10) = CurY

        'If Val(Common_Procedures.User.IdNo) <> 1 Then
        '    Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 20, CurY + 40, 0, 0,  p2Font)
        'End If

        CurY = CurY + 5
        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        p1Font = New Font("Calibri", 7, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "Certified that the Particulars given above are true and correct", PageWidth - 10, CurY, 1, 0, p1Font)
        CurY = CurY + TxtHgt - 5
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        p3Font = New Font("Calibri", 12, FontStyle.Regular)
        Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 10, CurY, 1, 0, p1Font)
        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt
        '   CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "PREPARED BY", LMargin + 20, CurY, 0, 0, p3Font)
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1135" Then
            Common_Procedures.Print_To_PrintDocument(e, "RECEIVER SIGNATURE", LMargin + 250, CurY, 0, 0, p3Font)
        Else
            Common_Procedures.Print_To_PrintDocument(e, "CHECKED BY", LMargin + 250, CurY, 0, 0, p3Font)
        End If
        p1Font = New Font("Calibri", 12, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, "AUTHORISED SIGNATORY ", PageWidth - 10, CurY, 1, 0, p3Font)
        CurY = CurY + TxtHgt + 10

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)


        'Catch ex As Exception

        '    MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        'End Try

    End Sub



End Class