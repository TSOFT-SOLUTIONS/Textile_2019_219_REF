Imports System.IO
Public Class JobWork_ConversionBill_Multi_Entry
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "JBILL-"
    Private Pk_Condition2 As String = "JAGCB-"
    Private PkCondition_JWTDS As String = "JWMTD-"

    Private NoCalc_Status As Boolean = False
    Private Print_PDF_Status As Boolean = False
    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl

    Private prn_OriDupTri_Count As String = ""

    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
    Private prn_DetAr(50, 10) As String
    Private prn_DetMxIndx As Integer
    Private prn_NoofBmDets As Integer
    Private prn_DetSNo As Integer
    Private prn_TotMtrs As Single = 0
    Private prn_pcs As Single = 0
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Public vmskOldText As String = ""
    Public vmskSelStrt As Integer = -1
    Private prn_InpOpts As String = ""
    Private prn_OriDupTri As String = ""
    Private prn_Count As Integer
    Private Prn_Cnt_Temp As Integer = 0
    Private Prn_Cnt_TEMP1 As Integer = 0

    Private Enum dgvCol_Details As Integer

        Slno
        DC_No
        DC_Date
        ClothName
        Pcs
        Dc_Mtr
        Actual_Mtr
        Type1_Meter
        Type2_Meter
        Type3_Meter
        Type4_Meter
        Type5_Meter
        Total_Meter
        JobWork_Piece_DeliveryCode
        Sound_Rate
        Sound_Amount
        Seconds_Rate
        Seconds_Amount
        Bits_Rate
        Bits_Amount
        Reject_Rate
        Reject_Amount
        Others_Rate
        Others_Amount
        Total_Amount
        Po_No

    End Enum

    Private Sub clear()
        New_Entry = False
        Insert_Entry = False
        Print_PDF_Status = False
        NoCalc_Status = True

        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        pnl_Selection.Visible = False

        vmskOldText = ""
        vmskSelStrt = -1

        txt_BillPrefixNo.Text = ""
        lbl_BillNo.Text = ""
        lbl_BillNo.ForeColor = Color.Black
        msk_Date.Text = ""
        dtp_Date.Text = ""
        cbo_Ledger.Text = ""

        txt_LrNo.Text = ""
        dtp_LrDate.Text = ""

        txt_DespatchTo.Text = ""
        cbo_Transport.Text = ""
        cbo_VehicleNo.Text = ""
        txt_FormJJNo.Text = ""

        txt_com_per.Text = ""
        txt_CommAmt.Text = ""
        cbo_Agent.Text = ""
        cbo_Type.Text = "DIRECT"
        cbo_Type.Enabled = True
        cbo_InvoiceSufixNo.Text = ""
        cbo_DeliveryTo.Text = ""

        lbl_Meters_Type1.Text = ""
        lbl_Meters_Type2.Text = ""
        lbl_Meters_Type3.Text = ""
        lbl_Meters_Type4.Text = ""
        lbl_Meters_Type5.Text = ""

        txt_Rate_Type1.Text = ""
        txt_Rate_Type2.Text = ""
        txt_Rate_Type3.Text = ""
        txt_Rate_Type4.Text = ""
        txt_Rate_Type5.Text = ""

        lbl_Amount_Type1.Text = ""
        lbl_Amount_Type2.Text = ""
        lbl_Amount_Type3.Text = ""
        lbl_Amount_Type4.Text = ""
        lbl_Amount_Type5.Text = ""

        lbl_TotalAmount.Text = ""
        txt_Freight.Text = ""
        txt_AddLess.Text = ""
        lbl_RoundOff.Text = ""
        lbl_NetAmount.Text = ""

        txt_CGST_Perc.Text = "2.5"
        txt_SGST_Perc.Text = "2.5"
        txt_IGST_Perc.Text = ""
        lbl_TaxableValue.Text = ""
        lbl_CGST_Amount.Text = ""
        lbl_SGST_Amount.Text = ""
        lbl_IGST_Amount.Text = ""

        txt_Add_Less_Packing_Caption.Text = ""
        txt_Freight_Caption.Text = ""

        txt_Tds.Text = ""

        pic_IRN_QRCode_Image.BackgroundImage = Nothing
        txt_IR_No.Text = ""

        If Filter_Status = False Then
            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()
        End If

        cbo_Ledger.Tag = ""

        dgv_Details.Rows.Clear()
        dgv_Details_Total.Rows.Clear()
        dgv_Details_Total.Rows.Add()

        Grid_Cell_DeSelect()

        NoCalc_Status = False

    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox
        Dim Msktxbx As MaskedTextBox

        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Me.ActiveControl Is MaskedTextBox Or TypeOf Me.ActiveControl Is Button Then
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

        If Me.ActiveControl.Name <> cbo_Grid_ClothName.Name Then
            cbo_Grid_ClothName.Visible = False
        End If


        If Me.ActiveControl.Name <> dgv_Details.Name Then
            Grid_Cell_DeSelect()
        End If

        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)

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

        NoCalc_Status = True

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        'Try

        da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name as PartyName, c.ledger_Name as TransportName , del.Ledger_Name as Delivery_Name from JobWork_ConversionBill_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Ledger_Head c ON a.Transport_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Ledger_Head del ON a.Delivery_Idno = del.Ledger_IdNo Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.JobWork_ConversionBill_Code = '" & Trim(NewCode) & "'", con)
        dt1 = New DataTable
        da1.Fill(dt1)

        If dt1.Rows.Count > 0 Then

            txt_BillPrefixNo.Text = dt1.Rows(0).Item("JobWork_ConversionBill_PrefixNo").ToString
            lbl_BillNo.Text = dt1.Rows(0).Item("JobWork_ConversionBill_RefNo").ToString
            cbo_InvoiceSufixNo.Text = dt1.Rows(0).Item("JobWork_ConversionBill_SuffixNo").ToString
            dtp_Date.Text = dt1.Rows(0).Item("JobWork_ConversionBill_Date").ToString
            msk_Date.Text = dtp_Date.Text
            cbo_Type.Text = dt1.Rows(0).Item("Entry_Type").ToString
            cbo_Type.Enabled = False
            cbo_Ledger.Text = dt1.Rows(0).Item("PartyName").ToString

            txt_LrNo.Text = dt1.Rows(0).Item("Lr_No").ToString
            ' dtp_LrDate.Text = dt1.Rows(0).Item("Lr_Date").ToString
            cbo_DeliveryTo.Text = dt1.Rows(0).Item("delivery_name").ToString
            txt_DespatchTo.Text = dt1.Rows(0).Item("Despatch_To").ToString

            cbo_Transport.Text = dt1.Rows(0).Item("TransportName").ToString
            cbo_VehicleNo.Text = dt1.Rows(0).Item("Vehicle_No").ToString
            txt_FormJJNo.Text = dt1.Rows(0).Item("FormJJ_No").ToString

            lbl_Meters_Type1.Text = Format(Val(dt1.Rows(0).Item("Total_ClothType1_Meters").ToString), "#########0.00")
            lbl_Meters_Type2.Text = Format(Val(dt1.Rows(0).Item("Total_ClothType2_Meters").ToString), "#########0.00")
            lbl_Meters_Type3.Text = Format(Val(dt1.Rows(0).Item("Total_ClothType3_Meters").ToString), "#########0.00")
            lbl_Meters_Type4.Text = Format(Val(dt1.Rows(0).Item("Total_ClothType4_Meters").ToString), "#########0.00")
            lbl_Meters_Type5.Text = Format(Val(dt1.Rows(0).Item("Total_ClothType5_Meters").ToString), "#########0.00")

            txt_Rate_Type1.Text = Format(Val(dt1.Rows(0).Item("Rate_ClothType1").ToString), "#########0.00")
            txt_Rate_Type2.Text = Format(Val(dt1.Rows(0).Item("Rate_ClothType2").ToString), "#########0.00")
            txt_Rate_Type3.Text = Format(Val(dt1.Rows(0).Item("Rate_ClothType3").ToString), "#########0.00")
            txt_Rate_Type4.Text = Format(Val(dt1.Rows(0).Item("Rate_ClothType4").ToString), "#########0.00")
            txt_Rate_Type5.Text = Format(Val(dt1.Rows(0).Item("Rate_ClothType5").ToString), "#########0.00")

            lbl_Amount_Type1.Text = Format(Val(dt1.Rows(0).Item("Amount_ClothType1").ToString), "#########0.00")
            lbl_Amount_Type2.Text = Format(Val(dt1.Rows(0).Item("Amount_ClothType2").ToString), "#########0.00")
            lbl_Amount_Type3.Text = Format(Val(dt1.Rows(0).Item("Amount_ClothType3").ToString), "#########0.00")
            lbl_Amount_Type4.Text = Format(Val(dt1.Rows(0).Item("Amount_ClothType4").ToString), "#########0.00")
            lbl_Amount_Type5.Text = Format(Val(dt1.Rows(0).Item("Amount_ClothType5").ToString), "#########0.00")

            lbl_TotalAmount.Text = Format(Val(dt1.Rows(0).Item("Gross_Amount").ToString), "#########0.00")
            txt_Freight.Text = Format(Val(dt1.Rows(0).Item("Freight_Amount").ToString), "#########0.00")
            txt_AddLess.Text = Format(Val(dt1.Rows(0).Item("AddLess_Amount").ToString), "#########0.00")
            lbl_RoundOff.Text = Format(Val(dt1.Rows(0).Item("RoundOff_Amount").ToString), "#########0.00")
            lbl_NetAmount.Text = Format(Val(dt1.Rows(0).Item("Net_Amount").ToString), "#########0.00")

            cbo_Agent.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Agent_IdNo").ToString))
            txt_com_per.Text = dt1.Rows(0).Item("Agent_Comm_Perc").ToString
            txt_CommAmt.Text = dt1.Rows(0).Item("Agent_Comm_Total").ToString

            lbl_TaxableValue.Text = dt1.Rows(0).Item("Total_Taxable_Amount").ToString
            txt_CGST_Perc.Text = dt1.Rows(0).Item("CGST_Percentage").ToString
            lbl_CGST_Amount.Text = dt1.Rows(0).Item("CGST_Amount").ToString
            txt_SGST_Perc.Text = dt1.Rows(0).Item("SGST_Percentage").ToString
            lbl_SGST_Amount.Text = dt1.Rows(0).Item("SGST_Amount").ToString
            txt_IGST_Perc.Text = dt1.Rows(0).Item("IGST_Percentage").ToString
            lbl_IGST_Amount.Text = dt1.Rows(0).Item("IGST_Amount").ToString

            txt_Tds.Text = dt1.Rows(0).Item("Tds_Perc").ToString
            lbl_Tds_Amount.Text = dt1.Rows(0).Item("Tds_Amount").ToString
            lbl_BillAmount.Text = Format(Val(dt1.Rows(0).Item("Bill_Amount").ToString), "#########0.00")

            txt_IR_No.Text = Trim(dt1.Rows(0).Item("E_Invoice_IRNO").ToString)
            txt_eInvoiceNo.Text = Trim(dt1.Rows(0).Item("E_Invoice_IRNO").ToString)
            If Not IsDBNull(dt1.Rows(0).Item("E_Invoice_ACK_No")) Then txt_eInvoiceAckNo.Text = Trim(dt1.Rows(0).Item("E_Invoice_ACK_No").ToString)
            If Not IsDBNull(dt1.Rows(0).Item("E_Invoice_ACK_Date")) Then
                If IsDate(dt1.Rows(0).Item("E_Invoice_ACK_Date")) Then
                    If Year(dt1.Rows(0).Item("E_Invoice_ACK_Date")) <> 1900 Then
                        txt_eInvoiceAckDate.Text = Format(Convert.ToDateTime(dt1.Rows(0).Item("E_Invoice_ACK_Date")), "dd-MM-yyyy hh:mm tt").ToString
                    End If

                End If
            End If
            If Trim(txt_IR_No.Text) <> "" Then
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

            If dt1.Rows(0).Item("AddLess_Caption_Name").ToString <> "" Then txt_Add_Less_Packing_Caption.Text = dt1.Rows(0).Item("AddLess_Caption_Name").ToString
            If dt1.Rows(0).Item("Freight_Caption_Name").ToString <> "" Then txt_Freight_Caption.Text = dt1.Rows(0).Item("Freight_Caption_Name").ToString


            da2 = New SqlClient.SqlDataAdapter("Select a.*, b.cloth_name from JobWork_ConversionBill_Details a, cloth_head b Where a.JobWork_ConversionBill_Code = '" & Trim(NewCode) & "' and a.cloth_idno = b.cloth_idno Order by a.JobWork_ConversionBill_Date, a.For_OrderBy, a.JobWork_ConversionBill_RefNo", con)
            dt2 = New DataTable
            da2.Fill(dt2)

            dgv_Details.Rows.Clear()
            SNo = 0

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Details.Rows.Add()

                    SNo = SNo + 1
                    dgv_Details.Rows(n).Cells(dgvCol_Details.Slno).Value = Val(SNo)
                    dgv_Details.Rows(n).Cells(dgvCol_Details.DC_No).Value = dt2.Rows(i).Item("Delivery_No").ToString
                    dgv_Details.Rows(n).Cells(dgvCol_Details.DC_Date).Value = dt2.Rows(i).Item("Delivery_Date").ToString
                    'dgv_Details.Rows(n).Cells(dgvCol_Details.DC_Date).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Delivery_Date").ToString), "dd-MM-yyyy").ToString
                    dgv_Details.Rows(n).Cells(dgvCol_Details.ClothName).Value = dt2.Rows(i).Item("Cloth_Name").ToString
                    dgv_Details.Rows(n).Cells(dgvCol_Details.Pcs).Value = Format(Val(dt2.Rows(i).Item("Pcs").ToString), "########0.00")
                    dgv_Details.Rows(n).Cells(dgvCol_Details.Dc_Mtr).Value = Format(Val(dt2.Rows(i).Item("Delivery_Meters").ToString), "########0.00")
                    dgv_Details.Rows(n).Cells(dgvCol_Details.Actual_Mtr).Value = Format(Val(dt2.Rows(i).Item("Actual_Meters").ToString), "########0.00")
                    dgv_Details.Rows(n).Cells(dgvCol_Details.Type1_Meter).Value = Format(Val(dt2.Rows(i).Item("Type1_Meters").ToString), "########0.00")
                    dgv_Details.Rows(n).Cells(dgvCol_Details.Type2_Meter).Value = Format(Val(dt2.Rows(i).Item("Type2_Meters").ToString), "########0.00")
                    dgv_Details.Rows(n).Cells(dgvCol_Details.Type3_Meter).Value = Format(Val(dt2.Rows(i).Item("Type3_Meters").ToString), "########0.00")
                    dgv_Details.Rows(n).Cells(dgvCol_Details.Type4_Meter).Value = Format(Val(dt2.Rows(i).Item("Type4_Meters").ToString), "########0.00")
                    dgv_Details.Rows(n).Cells(dgvCol_Details.Type5_Meter).Value = Format(Val(dt2.Rows(i).Item("Type5_Meters").ToString), "########0.00")
                    dgv_Details.Rows(n).Cells(dgvCol_Details.Total_Meter).Value = Format(Val(dt2.Rows(i).Item("Total_Meters").ToString), "########0.00")
                    dgv_Details.Rows(n).Cells(dgvCol_Details.JobWork_Piece_DeliveryCode).Value = dt2.Rows(i).Item("JobWork_Piece_Delivery_Code").ToString

                    dgv_Details.Rows(n).Cells(dgvCol_Details.Sound_Rate).Value = Format(Val(dt2.Rows(i).Item("Sound_Rate").ToString), "########0.00")
                    dgv_Details.Rows(n).Cells(dgvCol_Details.Sound_Amount).Value = Format(Val(dt2.Rows(i).Item("Sound_Amount").ToString), "########0.00")
                    dgv_Details.Rows(n).Cells(dgvCol_Details.Seconds_Rate).Value = Format(Val(dt2.Rows(i).Item("Seconds_Rate").ToString), "########0.00")
                    dgv_Details.Rows(n).Cells(dgvCol_Details.Seconds_Amount).Value = Format(Val(dt2.Rows(i).Item("Seconds_Amount").ToString), "########0.00")
                    dgv_Details.Rows(n).Cells(dgvCol_Details.Bits_Rate).Value = Format(Val(dt2.Rows(i).Item("Bits_Rate").ToString), "########0.00")
                    dgv_Details.Rows(n).Cells(dgvCol_Details.Bits_Amount).Value = Format(Val(dt2.Rows(i).Item("Bits_Amount").ToString), "########0.00")
                    dgv_Details.Rows(n).Cells(dgvCol_Details.Reject_Rate).Value = Format(Val(dt2.Rows(i).Item("Reject_Rate").ToString), "########0.00")
                    dgv_Details.Rows(n).Cells(dgvCol_Details.Reject_Amount).Value = Format(Val(dt2.Rows(i).Item("Reject_Amount").ToString), "########0.00")
                    dgv_Details.Rows(n).Cells(dgvCol_Details.Others_Rate).Value = Format(Val(dt2.Rows(i).Item("Others_Rate").ToString), "########0.00")
                    dgv_Details.Rows(n).Cells(dgvCol_Details.Others_Amount).Value = Format(Val(dt2.Rows(i).Item("Others_Amount").ToString), "########0.00")
                    dgv_Details.Rows(n).Cells(dgvCol_Details.Total_Amount).Value = Format(Val(dt2.Rows(i).Item("Total_Amount").ToString), "########0.00")
                    dgv_Details.Rows(n).Cells(dgvCol_Details.Po_No).Value = (dt2.Rows(i).Item("Po_No").ToString)

                Next i

            End If

            With dgv_Details_Total
                If .RowCount = 0 Then .Rows.Add()
                .Rows(0).Cells(4).Value = Val(dt1.Rows(0).Item("Total_Rolls").ToString)
                .Rows(0).Cells(5).Value = Format(Val(dt1.Rows(0).Item("Total_Delivery_Meters").ToString), "########0.00")
                .Rows(0).Cells(6).Value = Format(Val(dt1.Rows(0).Item("Total_Actual_Meters").ToString), "########0.00")
                .Rows(0).Cells(7).Value = Format(Val(dt1.Rows(0).Item("Total_ClothType1_Meters").ToString), "########0.00")
                .Rows(0).Cells(8).Value = Format(Val(dt1.Rows(0).Item("Total_ClothType2_Meters").ToString), "########0.00")
                .Rows(0).Cells(9).Value = Format(Val(dt1.Rows(0).Item("Total_ClothType3_Meters").ToString), "########0.00")
                .Rows(0).Cells(10).Value = Format(Val(dt1.Rows(0).Item("Total_ClothType4_Meters").ToString), "########0.00")
                .Rows(0).Cells(11).Value = Format(Val(dt1.Rows(0).Item("Total_ClothType5_Meters").ToString), "########0.00")
                .Rows(0).Cells(12).Value = Format(Val(dt1.Rows(0).Item("Total_Meters").ToString), "########0.00")
            End With

            dt2.Clear()
            dt2.Dispose()
            da2.Dispose()

        End If

        dt1.Clear()
        dt1.Dispose()
        da1.Dispose()

        Grid_Cell_DeSelect()

        'Catch ex As Exception
        '    MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        'End Try

        NoCalc_Status = False

        If msk_Date.Visible And msk_Date.Enabled Then msk_Date.Focus()

    End Sub




    Private Sub JobWork_Bill_Entry_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated


        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Ledger.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Ledger.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Transport.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Transport.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_ClothName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CLOTH NAME" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_ClothName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

    Private Sub JobWork_Bill_Entry_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable

        Me.Text = ""

        dgv_Details.Columns(dgvCol_Details.Type1_Meter).HeaderText = Trim(UCase(Common_Procedures.ClothType.Type1))
        dgv_Details.Columns(dgvCol_Details.Type2_Meter).HeaderText = Trim(UCase(Common_Procedures.ClothType.Type2))
        dgv_Details.Columns(dgvCol_Details.Type3_Meter).HeaderText = Trim(UCase(Common_Procedures.ClothType.Type3))
        dgv_Details.Columns(dgvCol_Details.Type4_Meter).HeaderText = Trim(UCase(Common_Procedures.ClothType.Type4))
        dgv_Details.Columns(dgvCol_Details.Type5_Meter).HeaderText = Trim(UCase(Common_Procedures.ClothType.Type5))

        lbl_ClothType1.Text = Trim(UCase(Common_Procedures.ClothType.Type1))
        lbl_ClothType2.Text = Trim(UCase(Common_Procedures.ClothType.Type2))
        lbl_ClothType3.Text = Trim(UCase(Common_Procedures.ClothType.Type3))
        lbl_ClothType4.Text = Trim(UCase(Common_Procedures.ClothType.Type4))
        lbl_ClothType5.Text = Trim(UCase(Common_Procedures.ClothType.Type5))

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1238" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1239" Then '---- KALAIMAGAL TEX
            txt_Add_Less_Packing_Caption.Text = "Trade Discount"
        Else
            txt_Add_Less_Packing_Caption.Text = "Add/Less"
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1186" Then

            dgv_Details.Columns(dgvCol_Details.Type4_Meter).Visible = True
            dgv_Details.Columns(dgvCol_Details.Type5_Meter).Visible = True
            dgv_Details.Columns(dgvCol_Details.Reject_Rate).Visible = True
            dgv_Details.Columns(dgvCol_Details.Others_Rate).Visible = True

            txt_LrNo.Enabled = False
            dtp_LrDate.Enabled = False
            cbo_Agent.Enabled = False
            txt_com_per.Enabled = False
            txt_CommAmt.Enabled = False
            txt_AddLess.Enabled = False
            txt_Freight.Enabled = False



        End If


        con.Open()

        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or Ledger_Type = 'JOBWORKER') order by Ledger_DisplayName", con)
        da.Fill(dt1)
        cbo_Ledger.DataSource = dt1
        cbo_Ledger.DisplayMember = "Ledger_DisplayName"

        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or Ledger_Type = 'TRANSPORT') order by Ledger_DisplayName", con)
        da.Fill(dt2)
        cbo_Transport.DataSource = dt2
        cbo_Transport.DisplayMember = "Ledger_DisplayName"


        cbo_Type.Items.Clear()
        cbo_Type.Items.Add(" ")
        cbo_Type.Items.Add("DIRECT")
        cbo_Type.Items.Add("DELIVERY")

        cbo_InvoiceSufixNo.Items.Clear()
        cbo_InvoiceSufixNo.Items.Add("")
        cbo_InvoiceSufixNo.Items.Add("/" & Common_Procedures.FnYearCode)
        cbo_InvoiceSufixNo.Items.Add("/" & Year(Common_Procedures.Company_FromDate) & "-" & Year(Common_Procedures.Company_ToDate))

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2

        pnl_Selection.Visible = False
        pnl_Selection.Left = (Me.Width - pnl_Selection.Width) \ 2
        pnl_Selection.Top = (Me.Height - pnl_Selection.Height) \ 2

        btn_UserModification.Visible = False
        If Common_Procedures.settings.User_Modifications_Show_Status = 1 Then
            If Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_UserModification_Status = 1 Then
                btn_UserModification.Visible = True
            End If
        End If


        AddHandler txt_BillPrefixNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_InvoiceSufixNo.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Agent.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Transport.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_LrNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DespatchTo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Type.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_VehicleNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_FormJJNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_ClothName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Ledger.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_com_per.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_CommAmt.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler lbl_Meters_Type5.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Rate_Type1.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Rate_Type2.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Rate_Type3.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Rate_Type4.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Rate_Type5.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Freight.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_AddLess.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_CGST_Perc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_SGST_Perc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_IGST_Perc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_IR_No.Enter, AddressOf ControlGotFocus
        AddHandler cbo_DeliveryTo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Tds.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_DeliveryTo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_BillPrefixNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_InvoiceSufixNo.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Type.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_VehicleNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_FormJJNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_ClothName.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_LrNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DespatchTo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Transport.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Ledger.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_com_per.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_CommAmt.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Agent.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Rate_Type1.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Rate_Type2.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Rate_Type3.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Rate_Type4.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Rate_Type5.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Freight.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_AddLess.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_CGST_Perc.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_SGST_Perc.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_IGST_Perc.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_IR_No.Leave, AddressOf ControlLostFocus
        AddHandler txt_Tds.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_BillPrefixNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler msk_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_com_per.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler lbl_Meters_Type2.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler lbl_Meters_Type3.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler lbl_Meters_Type4.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler lbl_Meters_Type5.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_CGST_Perc.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_SGST_Perc.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_AddLess.KeyDown, AddressOf TextBoxControlKeyDown
        '   AddHandler txt_IR_No.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_BillPrefixNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler msk_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_com_per.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler lbl_Meters_Type2.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler lbl_Meters_Type3.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler lbl_Meters_Type4.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler lbl_Meters_Type5.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_CGST_Perc.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_SGST_Perc.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_AddLess.KeyPress, AddressOf TextBoxControlKeyPress
        ' AddHandler txt_IR_No.KeyPress, AddressOf TextBoxControlKeyPress

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True
        new_record()

    End Sub

    Private Sub JobWork_Bill_Entry_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub JobWork_Bill_Entry_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

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
                        If .CurrentCell.ColumnIndex >= .ColumnCount - 2 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                If Trim(UCase(cbo_Type.Text)) = "DIRECT" Then
                                    txt_Rate_Type1.Focus()
                                Else
                                    txt_Rate_Type1.Focus()
                                End If

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                            End If

                        ElseIf .CurrentCell.ColumnIndex = dgvCol_Details.Dc_Mtr Then

                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCol_Details.Type1_Meter)

                        ElseIf .CurrentCell.ColumnIndex = dgvCol_Details.Type4_Meter Then
                            If .Columns(dgvCol_Details.Type5_Meter).Visible = True Then
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCol_Details.Type5_Meter)
                            Else
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCol_Details.Sound_Rate)
                            End If


                        ElseIf .CurrentCell.ColumnIndex = dgvCol_Details.Type5_Meter Then

                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCol_Details.Sound_Rate)


                        ElseIf .CurrentCell.ColumnIndex = dgvCol_Details.Sound_Rate Then
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCol_Details.Seconds_Rate)


                        ElseIf .CurrentCell.ColumnIndex = dgvCol_Details.Seconds_Rate Then

                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCol_Details.Bits_Rate)

                        ElseIf .CurrentCell.ColumnIndex = dgvCol_Details.Bits_Rate Then

                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCol_Details.Reject_Rate)

                        ElseIf .CurrentCell.ColumnIndex = dgvCol_Details.Reject_Rate Then

                            If .Columns(dgvCol_Details.Others_Rate).Visible = True Then
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCol_Details.Others_Rate)
                            Else
                                If Common_Procedures.settings.CustomerCode = "1186" Then
                                    save_record()
                                Else
                                    cbo_Agent.Focus()
                                End If
                            End If


                        ElseIf .CurrentCell.ColumnIndex = dgvCol_Details.Others_Rate Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                If Common_Procedures.settings.CustomerCode = "1186" Then
                                    save_record()
                                Else
                                    cbo_Agent.Focus()
                                End If

                            Else

                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(dgvCol_Details.Seconds_Rate)


                            End If

                        Else

                            If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then
                                If Trim(UCase(cbo_Type.Text)) = "DIRECT" Then
                                    'txt_Rate_Type1.Focus()

                                    If Common_Procedures.settings.CustomerCode = "1186" Then
                                        save_record()

                                    Else
                                        cbo_Agent.Focus()
                                    End If

                                Else
                                    If Common_Procedures.settings.CustomerCode = "1186" Then
                                        save_record()
                                    Else
                                        cbo_Agent.Focus()
                                    End If

                                End If

                            Else

                                If .CurrentCell.ColumnIndex + 1 <= .Columns.Count - 1 Then
                                    If .Columns(.CurrentCell.ColumnIndex + 1).Visible = True Then
                                        .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
                                    Else

                                    End If

                                Else
                                    cbo_Agent.Focus()

                                End If



                            End If
                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then

                        If .CurrentCell.ColumnIndex <= dgvCol_Details.DC_No Then
                            If .CurrentCell.RowIndex = 0 Then
                                ' txt_FormJJNo.Focus()
                                txt_IR_No.Focus()

                            Else

                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(dgvCol_Details.Dc_Mtr)

                            End If

                        ElseIf .CurrentCell.ColumnIndex = dgvCol_Details.Type1_Meter Then
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details.Dc_Mtr)

                        ElseIf .CurrentCell.ColumnIndex = dgvCol_Details.Sound_Rate Then
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details.Type4_Meter)

                        ElseIf .CurrentCell.ColumnIndex = dgvCol_Details.Seconds_Rate Then
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCol_Details.Sound_Rate)


                        ElseIf .CurrentCell.ColumnIndex = dgvCol_Details.Bits_Rate Then

                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCol_Details.Seconds_Rate)

                        ElseIf .CurrentCell.ColumnIndex = dgvCol_Details.Reject_Rate Then

                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCol_Details.Bits_Rate)

                        ElseIf .CurrentCell.ColumnIndex = dgvCol_Details.Others_Rate Then

                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCol_Details.Reject_Rate)

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
        Dim vOrdByNo As String = ""

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_BillNo.Text)

        '    If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.JobWork_Conversion_Bill, "~L~") = 0 And InStr(Common_Procedures.UR.JobWork_Conversion_Bill, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub
        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_BillNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)


        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.Jobwork_Conversion_Bill_Entry, New_Entry, Me, con, "JobWork_ConversionBill_Head", "JobWork_ConversionBill_Code", NewCode, "JobWork_ConversionBill_Date", "(JobWork_ConversionBill_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub







        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        trans = con.BeginTransaction

        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_BillNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            If Common_Procedures.VoucherBill_Deletion(con, Trim(Pk_Condition) & Trim(NewCode), trans) = False Then
                Throw New ApplicationException("Error on Voucher Bill Deletion")
            End If

            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), trans)
            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition2) & Trim(NewCode), trans)

            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(PkCondition_JWTDS) & Trim(NewCode), trans)

            cmd.Connection = con
            cmd.Transaction = trans
            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "DELETE", "JobWork_ConversionBill_Head", "JobWork_ConversionBill_Code", Val(lbl_Company.Tag), NewCode, lbl_BillNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "", "", "JobWork_ConversionBill_Code, Company_IdNo, for_OrderBy", trans)

            Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "DELETE", "JobWork_ConversionBill_Details", "JobWork_ConversionBill_Code", Val(lbl_Company.Tag), NewCode, lbl_BillNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, " Delivery_No,Delivery_Date,Cloth_idNo,Pcs,Delivery_Meters,Actual_Meters,Type1_Meters,Type2_Meters,Type3_Meters,Type4_Meters,Type5_Meters, Total_Meters,JobWork_Piece_Delivery_Code  ", "Sl_No", "JobWork_ConversionBill_Code, For_OrderBy, Company_IdNo, JobWork_ConversionBill_No, JobWork_ConversionBill_Date, Ledger_Idno", trans)

            cmd.CommandText = "delete from AgentCommission_Processing_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            If Trim(UCase(cbo_Type.Text)) = "DELIVERY" Then
                cmd.CommandText = "Update JobWork_Piece_Delivery_Head set JobWork_Bill_Code = '', JobWork_Bill_Increment = JobWork_Bill_Increment - 1, JobWork_Bill_Date = Null Where JobWork_Bill_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()
            End If

            cmd.CommandText = "Delete from Voucher_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Entry_Identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Delete from Voucher_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Entry_Identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from JobWork_ConversionBill_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and JobWork_ConversionBill_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from JobWork_ConversionBill_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and JobWork_ConversionBill_Code = '" & Trim(NewCode) & "'"
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

        If msk_Date.Enabled = True And msk_Date.Visible = True Then msk_Date.Focus()

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        If Filter_Status = False Then

            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable

            da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or Ledger_Type = 'JOBWORKER') order by Ledger_DisplayName", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 JobWork_ConversionBill_RefNo from JobWork_ConversionBill_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and JobWork_ConversionBill_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, JobWork_ConversionBill_RefNo", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_BillNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 JobWork_ConversionBill_RefNo from JobWork_ConversionBill_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and JobWork_ConversionBill_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, JobWork_ConversionBill_RefNo", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_BillNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 JobWork_ConversionBill_RefNo from JobWork_ConversionBill_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and JobWork_ConversionBill_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, JobWork_ConversionBill_RefNo desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 JobWork_ConversionBill_RefNo from JobWork_ConversionBill_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and JobWork_ConversionBill_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, JobWork_ConversionBill_RefNo desc", con)
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

            lbl_BillNo.Text = Common_Procedures.get_MaxCode(con, "JobWork_ConversionBill_Head", "JobWork_ConversionBill_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

            lbl_BillNo.ForeColor = Color.Red

            msk_Date.Text = Date.Today.ToShortDateString
            da = New SqlClient.SqlDataAdapter("select top 1 * from JobWork_ConversionBill_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and JobWork_ConversionBill_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, JobWork_ConversionBill_RefNo desc", con)
            dt1 = New DataTable
            da.Fill(dt1)
            If dt1.Rows.Count > 0 Then
                If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)
                    If dt1.Rows(0).Item("JobWork_ConversionBill_Date").ToString <> "" Then msk_Date.Text = dt1.Rows(0).Item("JobWork_ConversionBill_Date").ToString
                End If
                If dt1.Rows(0).Item("JobWork_ConversionBill_PrefixNo").ToString <> "" Then txt_BillPrefixNo.Text = dt1.Rows(0).Item("JobWork_ConversionBill_PrefixNo").ToString
                If dt1.Rows(0).Item("JobWork_ConversionBill_SuffixNo").ToString <> "" Then cbo_InvoiceSufixNo.Text = dt1.Rows(0).Item("JobWork_ConversionBill_SuffixNo").ToString
                If dt1.Rows(0).Item("Entry_Type").ToString <> "" Then cbo_Type.Text = dt1.Rows(0).Item("Entry_Type").ToString
                If dt1.Rows(0).Item("CGST_Percentage").ToString <> "" Then txt_CGST_Perc.Text = dt1.Rows(0).Item("CGST_Percentage").ToString
                If dt1.Rows(0).Item("SGST_Percentage").ToString <> "" Then txt_SGST_Perc.Text = dt1.Rows(0).Item("SGST_Percentage").ToString
                If dt1.Rows(0).Item("IGST_Percentage").ToString <> "" Then txt_IGST_Perc.Text = dt1.Rows(0).Item("IGST_Percentage").ToString

                If dt1.Rows(0).Item("AddLess_Caption_Name").ToString <> "" Then txt_Add_Less_Packing_Caption.Text = dt1.Rows(0).Item("AddLess_Caption_Name").ToString
                If dt1.Rows(0).Item("Freight_Caption_Name").ToString <> "" Then txt_Freight_Caption.Text = dt1.Rows(0).Item("Freight_Caption_Name").ToString

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
        Dim InvCode As String

        Try

            inpno = InputBox("Enter Bill No.", "FOR FINDING...")

            InvCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select JobWork_ConversionBill_RefNo from JobWork_ConversionBill_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and JobWork_ConversionBill_Code = '" & Trim(InvCode) & "'", con)
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
                MessageBox.Show("Bill No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.JobWork_Conversion_Bill, "~L~") = 0 And InStr(Common_Procedures.UR.JobWork_Conversion_Bill, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.Jobwork_Conversion_Bill_Entry, New_Entry, Me) = False Then Exit Sub





        Try

            inpno = InputBox("Enter New Bill No.", "FOR NEW BILL INSERTION...")

            InvCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select JobWork_ConversionBill_RefNo from JobWork_ConversionBill_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and JobWork_ConversionBill_Code = '" & Trim(InvCode) & "'", con)
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
                    MessageBox.Show("Invalid Bill No.", "DOES NOT INSERT NEW BILL...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_BillNo.Text = Trim(UCase(inpno))

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
        Dim SalAc_ID As Integer = 18
        Dim Clo_ID As Integer = 0
        Dim Trans_ID As Integer
        Dim Sno As Integer = 0
        Dim Partcls As String = ""
        Dim PBlNo As String = ""
        Dim vTotRls As Single, vTotDcMtrs As Single, vTotActMtrs As Single, vTotMtrs1 As Single, vTotMtrs2 As Single
        Dim vTotMtrs3 As Single, vTotMtrs4 As Single, vTotMtrs5 As Single, vNtTtMtrs As Single
        Dim Nr As Long
        Dim consyarn As Single = 0
        Dim PavuConsMtrs As Single = 0
        Dim Lm_ID As Integer = 0
        Dim vWdth_Typ As String = ""
        Dim Cr_ID As Integer = 0
        Dim Clth_ID As Integer = 0
        Dim Dr_ID As Integer = 0
        Dim Agt_Idno As Integer = 0
        Dim YrnClthNm As String = ""
        Dim vEntBillNo As String = ""
        Dim Dc_Cd As String = ""
        Dim vOrdBy As String = ""
        Dim EntID As String = ""
        Dim vOrdByNo As String = ""
        Dim vEInvAckDate As String = ""
        Dim lckdt As Date
        Dim dat As Date
        Dim Delivery_ID As Integer = 0
        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_BillNo.Text)


        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1253" Then '--- vm mills
            lckdt = #8/20/2025#

            If IsDate(Common_Procedures.settings.Sdd) = True Then
                dat = Common_Procedures.settings.Sdd
            End If

            If DateDiff("d", lckdt.ToShortDateString, dat.ToShortDateString) > 0 Then
                MessageBox.Show("Run-time error '463': " & Chr(13) & Chr(13) & "Class not registered on local machine", "DOES NOT SAVE", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error)
                Me.Close()
                Application.Exit()
            End If

        End If


        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_BillNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Jobwork_Conversion_Bill_Entry, New_Entry, Me, con, "JobWork_ConversionBill_Head", "JobWork_ConversionBill_Code", NewCode, "JobWork_ConversionBill_Date", "(JobWork_ConversionBill_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and JobWork_ConversionBill_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, JobWork_ConversionBill_RefNo desc", dtp_Date.Value.Date) = False Then Exit Sub


        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        '  If Common_Procedures.UserRight_Check(Common_Procedures.UR.JobWork_Conversion_Bill, New_Entry) = False Then Exit Sub

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
        Agt_Idno = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Agent.Text)

        Delivery_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_DeliveryTo.Text)

        For i = 0 To dgv_Details.RowCount - 1

            If Trim(dgv_Details.Rows(i).Cells(dgvCol_Details.DC_No).Value) <> "" And Val(dgv_Details.Rows(i).Cells(dgvCol_Details.Dc_Mtr).Value) <> 0 Then
                If Trim(UCase(cbo_Type.Text)) = "DELIVERY" Then
                    'If Val(dgv_Details.Rows(i).Cells(6).Value) <= 0 Then
                    '    MessageBox.Show("Invalid Actual Meters", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    '    dgv_Details.Focus()
                    '    dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(6)
                    '    dgv_Details.CurrentCell.Selected = True
                    '    Exit Sub
                    'End If
                End If
                If Val(dgv_Details.Rows(i).Cells(dgvCol_Details.Type1_Meter).Value) <= 0 And Val(dgv_Details.Rows(i).Cells(dgvCol_Details.Type2_Meter).Value) <= 0 And Val(dgv_Details.Rows(i).Cells(dgvCol_Details.Type3_Meter).Value) <= 0 And Val(dgv_Details.Rows(i).Cells(dgvCol_Details.Type4_Meter).Value) <= 0 And Val(dgv_Details.Rows(i).Cells(dgvCol_Details.Type5_Meter).Value) <= 0 Then
                    MessageBox.Show("Invalid Checking Meters", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(dgvCol_Details.Type1_Meter)
                    dgv_Details.CurrentCell.Selected = True
                    Exit Sub
                End If

            End If

        Next

        NoCalc_Status = False
        Total_Calculation()

        vTotRls = 0 : vTotDcMtrs = 0 : vTotActMtrs = 0
        vTotMtrs1 = 0 : vTotMtrs2 = 0 : vTotMtrs3 = 0
        vTotMtrs4 = 0 : vTotMtrs5 = 0 : vNtTtMtrs = 0

        If dgv_Details_Total.RowCount > 0 Then
            vTotRls = Val(dgv_Details_Total.Rows(0).Cells(4).Value())
            vTotDcMtrs = Val(dgv_Details_Total.Rows(0).Cells(5).Value())
            vTotActMtrs = Val(dgv_Details_Total.Rows(0).Cells(6).Value())
            vTotMtrs1 = Val(dgv_Details_Total.Rows(0).Cells(7).Value())
            'vTotMtrs2 = Val(dgv_Details_Total.Rows(0).Cells(8).Value())
            'vTotMtrs3 = Val(dgv_Details_Total.Rows(0).Cells(9).Value())
            'vTotMtrs4 = Val(dgv_Details_Total.Rows(0).Cells(10).Value())
            'vTotMtrs5 = Val(dgv_Details_Total.Rows(0).Cells(11).Value())
            'vNtTtMtrs = Val(dgv_Details_Total.Rows(0).Cells(12).Value())
        End If

        vNtTtMtrs = Val(lbl_Meters_Type1.Text) + Val(lbl_Meters_Type2.Text) + Val(lbl_Meters_Type3.Text) + Val(lbl_Meters_Type4.Text) + Val(lbl_Meters_Type5.Text)

        tr = con.BeginTransaction

        Try

        If Insert_Entry = True Or New_Entry = False Then

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_BillNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Else

            lbl_BillNo.Text = Common_Procedures.get_MaxCode(con, "JobWork_ConversionBill_Head", "JobWork_ConversionBill_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_BillNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        End If


        vOrdBy = Common_Procedures.OrderBy_CodeToValue(lbl_BillNo.Text)
            vEntBillNo = Trim(txt_BillPrefixNo.Text) & Trim(lbl_BillNo.Text) & Trim(cbo_InvoiceSufixNo.Text)

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@BillDate", Convert.ToDateTime(msk_Date.Text))

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


            If New_Entry = True Then

                cmd.CommandText = "Insert into JobWork_ConversionBill_Head ( JobWork_ConversionBill_Code ,               Company_IdNo       ,   JobWork_ConversionBill_PrefixNo    ,  JobWork_ConversionBill_SuffixNo        ,  JobWork_ConversionBill_No ,  JobWork_ConversionBill_RefNo  ,        for_OrderBy     , JobWork_ConversionBill_Date,          Ledger_IdNo    ,          Sales_Ac_Idno    ,           Lr_No              ,             Lr_Date            ,         Transport_IdNo    ,              Vehicle_No           ,              FormJJ_No           ,              Despatch_To           ,        Total_Rolls       ,     Total_Delivery_Meters   ,      Total_Actual_Meters     ,             Total_ClothType1_Meters    ,             Total_ClothType2_Meters    ,             Total_ClothType3_Meters    ,             Total_ClothType4_Meters    ,             Total_ClothType5_Meters    ,           Total_Meters      ,            Rate_ClothType1          ,             Rate_ClothType2          ,             Rate_ClothType3          ,              Rate_ClothType4         ,              Rate_ClothType5         ,              Amount_ClothType1         ,               Amount_ClothType2        ,              Amount_ClothType3         ,               Amount_ClothType4        ,               Amount_ClothType5        ,               Gross_Amount            ,                Freight_Amount     ,              AddLess_Amount       ,              RoundOff_Amount       ,                      Net_Amount            , Agent_IdNo               , Agent_Comm_Perc                           , Agent_Comm_Total             ,  Entry_Type                 ,Total_Taxable_Amount                   ,CGST_Percentage                     ,CGST_Amount                           ,SGST_Percentage                     ,SGST_Amount                           ,IGST_Percentage                     ,             IGST_Amount                   , E_Invoice_IRNO  ,   E_Invoice_QR_Image   ,        E_Invoice_ACK_No  ,                         E_Invoice_ACK_Date ,                                            Delivery_Idno         ,           Tds_Perc      ,                             Tds_Amount            ,               Bill_Amount       ,                AddLess_Caption_Name                     ,                Freight_Caption_Name ) " &
                                    "   Values                             (   '" & Trim(NewCode) & "'   , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(txt_BillPrefixNo.Text) & "', '" & Trim(cbo_InvoiceSufixNo.Text) & "' , '" & Trim(vEntBillNo) & "' , '" & Trim(lbl_BillNo.Text) & "'," & Str(Val(vOrdBy)) & ",       @BillDate            , " & Str(Val(Led_ID)) & ", " & Str(Val(SalAc_ID)) & ", '" & Trim(txt_LrNo.Text) & "', '" & Trim(dtp_LrDate.Text) & "', " & Str(Val(Trans_ID)) & ", '" & Trim(cbo_VehicleNo.Text) & "', '" & Trim(txt_FormJJNo.Text) & "', '" & Trim(txt_DespatchTo.Text) & "', " & Str(Val(vTotRls)) & ", " & Str(Val(vTotDcMtrs)) & ", " & Str(Val(vTotActMtrs)) & ", " & Str(Val(lbl_Meters_Type1.Text)) & ", " & Str(Val(lbl_Meters_Type2.Text)) & ", " & Str(Val(lbl_Meters_Type3.Text)) & ", " & Str(Val(lbl_Meters_Type4.Text)) & ", " & Str(Val(lbl_Meters_Type5.Text)) & ", " & Str(Val(vNtTtMtrs)) & ", " & Str(Val(txt_Rate_Type1.Text)) & ", " & Str(Val(txt_Rate_Type2.Text)) & ", " & Str(Val(txt_Rate_Type3.Text)) & ", " & Str(Val(txt_Rate_Type4.Text)) & ", " & Str(Val(txt_Rate_Type5.Text)) & ", " & Str(Val(lbl_Amount_Type1.Text)) & ", " & Str(Val(lbl_Amount_Type2.Text)) & ", " & Str(Val(lbl_Amount_Type3.Text)) & ", " & Str(Val(lbl_Amount_Type4.Text)) & ", " & Str(Val(lbl_Amount_Type5.Text)) & ", " & Str(Val(lbl_TotalAmount.Text)) & ", " & Str(Val(txt_Freight.Text)) & ", " & Str(Val(txt_AddLess.Text)) & ", " & Str(Val(lbl_RoundOff.Text)) & ", " & Str(Val(CSng(lbl_NetAmount.Text))) & " , " & Str(Val(Agt_Idno)) & ",  " & Str(Val(txt_com_per.Text)) & ",  " & Str(Val(txt_CommAmt.Text)) & ",'" & Trim(cbo_Type.Text) & "'," & Str(Val(lbl_TaxableValue.Text)) & "," & Str(Val(txt_CGST_Perc.Text)) & "," & Str(Val(lbl_CGST_Amount.Text)) & "," & Str(Val(txt_SGST_Perc.Text)) & "," & Str(Val(lbl_SGST_Amount.Text)) & "," & Str(Val(txt_IGST_Perc.Text)) & "," & Str(Val(lbl_IGST_Amount.Text)) & " ,  '" & Trim(txt_IR_No.Text) & "' ,     @QrCode , '" & txt_eInvoiceAckNo.Text & "'  ,  " & IIf(Trim(vEInvAckDate) <> "", "@EInvoiceAckDate", "Null") & "," & Str(Val(Delivery_ID)) & "  ,  " & Str(Val(txt_Tds.Text)) & ",  " & Str(Val(lbl_Tds_Amount.Text)) & " , " & Str(Val(lbl_BillAmount.Text)) & " ,  '" & Trim(txt_Add_Less_Packing_Caption.Text) & "'   , '" & Trim(txt_Freight_Caption.Text) & "' ) "
                cmd.ExecuteNonQuery()

            Else
                Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "OLD", "JobWork_ConversionBill_Head", "JobWork_ConversionBill_Code", Val(lbl_Company.Tag), NewCode, lbl_BillNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "JobWork_ConversionBill_Code, Company_IdNo, for_OrderBy", tr)

                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "OLD", "JobWork_ConversionBill_Details", "JobWork_ConversionBill_Code", Val(lbl_Company.Tag), NewCode, lbl_BillNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, " Delivery_No,Delivery_Date,Cloth_idNo,Pcs,Delivery_Meters,Actual_Meters,Type1_Meters,Type2_Meters,Type3_Meters,Type4_Meters,Type5_Meters, Total_Meters,JobWork_Piece_Delivery_Code  ", "Sl_No", "JobWork_ConversionBill_Code, For_OrderBy, Company_IdNo, JobWork_ConversionBill_No, JobWork_ConversionBill_Date, Ledger_Idno", tr)

                cmd.CommandText = "Update JobWork_ConversionBill_Head Set JobWork_ConversionBill_PrefixNo  =  '" & Trim(txt_BillPrefixNo.Text) & "' ,  JobWork_ConversionBill_SuffixNo ='" & Trim(cbo_InvoiceSufixNo.Text) & "' , JobWork_ConversionBill_RefNo = '" & Trim(lbl_BillNo.Text) & "'  ,  JobWork_ConversionBill_No =  '" & Trim(vEntBillNo) & "', JobWork_ConversionBill_Date = @BillDate, Ledger_IdNo = " & Str(Val(Led_ID)) & ", Sales_Ac_Idno = " & Str(Val(SalAc_ID)) & ", Lr_No = '" & Trim(txt_LrNo.Text) & "', Lr_Date = '" & Trim(dtp_LrDate.Text) & "', Transport_IdNo = " & Str(Val(Trans_ID)) & ", Vehicle_No = '" & Trim(cbo_VehicleNo.Text) & "', FormJJ_No = '" & Trim(txt_FormJJNo.Text) & "', Despatch_To = '" & Trim(txt_DespatchTo.Text) & "', Total_Rolls = " & Str(Val(vTotRls)) & ", Total_Delivery_Meters = " & Str(Val(vTotDcMtrs)) & ", Total_Actual_Meters = " & Str(Val(vTotActMtrs)) & ", Total_ClothType1_Meters = " & Str(Val(lbl_Meters_Type1.Text)) & ", Total_ClothType2_Meters = " & Str(Val(lbl_Meters_Type2.Text)) & ", Total_ClothType3_Meters = " & Str(Val(lbl_Meters_Type3.Text)) & ", Total_ClothType4_Meters = " & Str(Val(lbl_Meters_Type4.Text)) & ", Total_ClothType5_Meters = " & Str(Val(lbl_Meters_Type5.Text)) & ", Total_Meters = " & Str(Val(vNtTtMtrs)) & ", Rate_ClothType1 = " & Str(Val(txt_Rate_Type1.Text)) & ", Rate_ClothType2 = " & Str(Val(txt_Rate_Type2.Text)) & ", Rate_ClothType3 = " & Str(Val(txt_Rate_Type3.Text)) & ", Rate_ClothType4 = " & Str(Val(txt_Rate_Type4.Text)) & ", Rate_ClothType5 = " & Str(Val(txt_Rate_Type5.Text)) & ", Amount_ClothType1 = " & Str(Val(lbl_Amount_Type1.Text)) & ", Amount_ClothType2 = " & Str(Val(lbl_Amount_Type2.Text)) & ", Amount_ClothType3 = " & Str(Val(lbl_Amount_Type3.Text)) & ",  Agent_IdNo = " & Str(Val(Agt_Idno)) & " ,Agent_Comm_Perc =  " & Str(Val(txt_com_per.Text)) & " , Agent_Comm_Total = " & Str(Val(txt_CommAmt.Text)) & ", Amount_ClothType4 = " & Str(Val(lbl_Amount_Type4.Text)) & ", Amount_ClothType5 = " & Str(Val(lbl_Amount_Type5.Text)) & ", Gross_Amount = " & Str(Val(lbl_TotalAmount.Text)) & ", Freight_Amount = " & Str(Val(txt_Freight.Text)) & ", AddLess_Amount = " & Str(Val(txt_AddLess.Text)) & ", RoundOff_Amount = " & Str(Val(lbl_RoundOff.Text)) & ", Net_Amount = " & Str(Val(CSng(lbl_NetAmount.Text))) & ",Entry_Type = '" & Trim(cbo_Type.Text) & "', Total_Taxable_Amount = " & Str(Val(lbl_TaxableValue.Text)) & ",CGST_Percentage =" & Str(Val(txt_CGST_Perc.Text)) & ",CGST_Amount = " & Str(Val(lbl_CGST_Amount.Text)) & ",SGST_Percentage = " & Str(Val(txt_SGST_Perc.Text)) & ",SGST_Amount =" & Str(Val(lbl_SGST_Amount.Text)) & ",IGST_Percentage =" & Str(Val(txt_IGST_Perc.Text)) & ",IGST_Amount = " & Str(Val(lbl_IGST_Amount.Text)) & " ,  E_Invoice_IRNO = '" & Trim(txt_IR_No.Text) & "' , E_Invoice_QR_Image =  @QrCode ,  E_Invoice_ACK_No = '" & txt_eInvoiceAckNo.Text & "' , E_Invoice_ACK_Date = " & IIf(Trim(vEInvAckDate) <> "", "@EInvoiceAckDate", "Null") & " ,Delivery_Idno=" & Str(Val(Delivery_ID)) & " , Tds_Perc =  " & Str(Val(txt_Tds.Text)) & " , Tds_Amount =  " & Str(Val(lbl_Tds_Amount.Text)) & " , Bill_Amount = " & Str(Val(lbl_BillAmount.Text)) & "  , AddLess_Caption_Name  = '" & Trim(txt_Add_Less_Packing_Caption.Text) & "'   ,  Freight_Caption_Name = '" & Trim(txt_Freight_Caption.Text) & "'  Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and JobWork_ConversionBill_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                If Trim(UCase(cbo_Type.Text)) = "DELIVERY" Then

                    cmd.CommandText = "Update JobWork_Piece_Delivery_Head set JobWork_Bill_Code = '', JobWork_Bill_Increment = JobWork_Bill_Increment - 1, JobWork_Bill_Date = Null Where JobWork_Bill_Code = '" & Trim(NewCode) & "'"
                    cmd.ExecuteNonQuery()
                End If

            End If


            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "NEW", "JobWork_ConversionBill_Head", "JobWork_ConversionBill_Code", Val(lbl_Company.Tag), NewCode, lbl_BillNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "JobWork_ConversionBill_Code, Company_IdNo, for_OrderBy", tr)


            cmd.CommandText = "Delete from JobWork_ConversionBill_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and JobWork_ConversionBill_Code = '" & Trim(NewCode) & "' "
            cmd.ExecuteNonQuery()

            Sno = 0
            For i = 0 To dgv_Details.RowCount - 1
                Sno = Sno + 1

                Clth_ID = Common_Procedures.Cloth_NameToIdNo(con, dgv_Details.Rows(i).Cells(dgvCol_Details.ClothName).Value, tr)

                If Val(Clth_ID) <> 0 And (Val(dgv_Details.Rows(i).Cells(dgvCol_Details.Dc_Mtr).Value) <> 0 Or Val(dgv_Details.Rows(i).Cells(dgvCol_Details.Total_Meter).Value) <> 0) Then
                    Dc_Cd = ""
                    If Trim(UCase(cbo_Type.Text)) = "DELIVERY" Then

                        Dc_Cd = Trim(dgv_Details.Rows(i).Cells(dgvCol_Details.JobWork_Piece_DeliveryCode).Value)
                    Else
                        Dc_Cd = ""

                    End If




                    cmd.CommandText = "Insert into JobWork_ConversionBill_Details ( JobWork_ConversionBill_Code ,               Company_IdNo       ,  JobWork_ConversionBill_No, JobWork_ConversionBill_RefNo    ,        for_OrderBy     ,  JobWork_ConversionBill_Date,       Ledger_IdNo   ,         Sl_No     ,          Delivery_No                              ,                                    Delivery_Date                    ,                             Cloth_idNo         ,                     Pcs                                            ,                            Delivery_Meters                      ,                                          Actual_Meters                     ,                                   Type1_Meters          ,                                                 Type2_Meters                         ,                                          Type3_Meters                    ,                                                Type4_Meters                            ,                                           Type5_Meters                 ,                                                Total_Meters                                           ,   JobWork_Piece_Delivery_Code ,                      Sound_Rate   ,                                                                            Sound_Amount   ,                                                           Seconds_Rate  ,                                                           Seconds_Amount ,                                                                                 Bits_Rate  ,                                                                       Bits_Amount  ,                                                          Reject_Rate  ,                                                                         Reject_Amount ,                                                             Others_Rate  ,                                                                      Others_Amount  ,                                                        Total_Amount                                   ,                               Po_No                                    ) " &
                                        "     Values                 (   '" & Trim(NewCode) & "'      , " & Str(Val(lbl_Company.Tag)) & "          , '" & Trim(vEntBillNo) & "',  '" & Trim(lbl_BillNo.Text) & "'," & Str(Val(vOrdBy)) & ",    @BillDate                , " & Val(Led_ID) & " , " & Str(Val(Sno)) & ",'" & Trim(dgv_Details.Rows(i).Cells(dgvCol_Details.DC_No).Value) & "','" & Trim(dgv_Details.Rows(i).Cells(dgvCol_Details.DC_Date).Value) & "' ," & Str(Val(Clth_ID)) & ",  " & Str(Val(dgv_Details.Rows(i).Cells(dgvCol_Details.Pcs).Value)) & ", " & Str(Val(dgv_Details.Rows(i).Cells(dgvCol_Details.Dc_Mtr).Value)) & ", " & Str(Val(dgv_Details.Rows(i).Cells(dgvCol_Details.Actual_Mtr).Value)) & ", " & Val(dgv_Details.Rows(i).Cells(dgvCol_Details.Type1_Meter).Value) & ", " & Str(Val(dgv_Details.Rows(i).Cells(dgvCol_Details.Type2_Meter).Value)) & ", " & Str(Val(dgv_Details.Rows(i).Cells(dgvCol_Details.Type3_Meter).Value)) & " ," & Str(Val(dgv_Details.Rows(i).Cells(dgvCol_Details.Type4_Meter).Value)) & " ," & Str(Val(dgv_Details.Rows(i).Cells(dgvCol_Details.Type5_Meter).Value)) & "  ," & Str(Val(dgv_Details.Rows(i).Cells(dgvCol_Details.Total_Meter).Value)) & " ,     '" & Trim(Dc_Cd) & "'   ,    " & Str(Val(dgv_Details.Rows(i).Cells(dgvCol_Details.Sound_Rate).Value)) & " ,  " & Str(Val(dgv_Details.Rows(i).Cells(dgvCol_Details.Sound_Amount).Value)) & " , " & Str(Val(dgv_Details.Rows(i).Cells(dgvCol_Details.Seconds_Rate).Value)) & " , " & Str(Val(dgv_Details.Rows(i).Cells(dgvCol_Details.Seconds_Amount).Value)) & " ," & Str(Val(dgv_Details.Rows(i).Cells(dgvCol_Details.Bits_Rate).Value)) & " ," & Str(Val(dgv_Details.Rows(i).Cells(dgvCol_Details.Bits_Amount).Value)) & " ," & Str(Val(dgv_Details.Rows(i).Cells(dgvCol_Details.Reject_Rate).Value)) & " ," & Str(Val(dgv_Details.Rows(i).Cells(dgvCol_Details.Reject_Amount).Value)) & " ," & Str(Val(dgv_Details.Rows(i).Cells(dgvCol_Details.Others_Rate).Value)) & " ," & Str(Val(dgv_Details.Rows(i).Cells(dgvCol_Details.Others_Amount).Value)) & " ," & Str(Val(dgv_Details.Rows(i).Cells(dgvCol_Details.Total_Amount).Value)) & ",'" & Trim(dgv_Details.Rows(i).Cells(dgvCol_Details.Po_No).Value) & "'  ) "
                cmd.ExecuteNonQuery()

                    If Trim(UCase(cbo_Type.Text)) = "DELIVERY" Then
                        Nr = 0
                        cmd.CommandText = "Update JobWork_Piece_Delivery_Head set JobWork_Bill_Code = '" & Trim(NewCode) & "', JobWork_Bill_Increment = JobWork_Bill_Increment + 1, JobWork_Bill_Date = @BillDate Where JobWork_Piece_Delivery_Code = '" & Trim(Dc_Cd) & "' and Ledger_IdNo = " & Str(Val(Led_ID))
                        Nr = cmd.ExecuteNonQuery()

                        If Nr = 0 Then
                            MessageBox.Show("Invalid Delivery Details - Mismatch of details", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            tr.Rollback()
                            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
                            Exit Sub
                        End If

                    End If

                End If

            Next
            Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "NEW", "JobWork_ConversionBill_Details", "JobWork_ConversionBill_Code", Val(lbl_Company.Tag), NewCode, lbl_BillNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, " Delivery_No,Delivery_Date,Cloth_idNo,Pcs,Delivery_Meters,Actual_Meters,Type1_Meters,Type2_Meters,Type3_Meters,Type4_Meters,Type5_Meters, Total_Meters,JobWork_Piece_Delivery_Code  ", "Sl_No", "JobWork_ConversionBill_Code, For_OrderBy, Company_IdNo, JobWork_ConversionBill_No, JobWork_ConversionBill_Date, Ledger_Idno", tr)

            EntID = Trim(Pk_Condition) & Trim(vEntBillNo)

            'AgentCommission Posting
            cmd.CommandText = "delete from AgentCommission_Processing_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            If Val(Agt_Idno) <> 0 Then
                cmd.CommandText = "Insert into AgentCommission_Processing_Details (  Reference_Code   ,             Company_IdNo         ,            Reference_No   ,         For_OrderBy     , Reference_Date, Commission_For,     Ledger_IdNo    ,           Agent_IdNo      ,         Entry_ID     ,      Party_BillNo    ,       Particulars      ,      Yarn_Cloth_Name     ,         Bags_Meters       ,               Amount              ,             Commission_Type      ,       Commission_Rate              ,            Commission_Amount     ) " &
                                                " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(vEntBillNo) & "', " & Str(Val(vOrdBy)) & ",   @BillDate,     'CLOTH'   , " & Str(Led_ID) & ", " & Str(Val(Agt_Idno)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', '" & Trim(YrnClthNm) & "', " & Str(Val(vTotActMtrs)) & "," & Str(Val(CSng(lbl_NetAmount.Text))) & ", 'PICK', " & Str(Val(txt_com_per.Text)) & ", " & Str(Val(txt_CommAmt.Text)) & ")"
                Nr = cmd.ExecuteNonQuery()
            End If




            Dim vLed_IdNos As String = "", vVou_Amts As String = "", ErrMsg As String = ""
            Dim AcPos_ID As Integer = 0

        'Dim vNetAmt As String = Format(Val(CSng(lbl_NetAmount.Text)), "#############0.00")

        Dim vNetAmt As String = Format(Val(CSng(lbl_BillAmount.Text)), "#############0.00")
        Dim vCGSTAmt As String = Format(Val(CSng(lbl_CGST_Amount.Text)), "#############0.00")
        Dim vSGSTAmt As String = Format(Val(CSng(lbl_SGST_Amount.Text)), "#############0.00")
        Dim vIGSTAmt As String = Format(Val(CSng(lbl_IGST_Amount.Text)), "#############0.00")

        '---GST
        vLed_IdNos = Led_ID & "|" & SalAc_ID & "|24|25|26"
            vVou_Amts = -1 * Val(vNetAmt) & "|" & Val(vNetAmt) - (Val(vCGSTAmt) + Val(vSGSTAmt) + Val(vIGSTAmt)) & "|" & Val(vCGSTAmt) & "|" & Val(vSGSTAmt) & "|" & Val(vIGSTAmt)

        If Common_Procedures.Voucher_Updation(con, "Jw.Invoice", Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), Trim(vEntBillNo), Convert.ToDateTime(msk_Date.Text), "Cnv.Bill No : " & Trim(vEntBillNo) & ", Mtrs : " & Trim(Format(Val(vTotMtrs1), "#########0.00")), vLed_IdNos, vVou_Amts, ErrMsg, tr, Common_Procedures.SoftwareTypes.Textile_Software) = False Then
            Throw New ApplicationException(ErrMsg)
        End If


        Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition2) & Trim(NewCode), tr)

        vLed_IdNos = Agt_Idno & "|" & Val(Common_Procedures.CommonLedger.Agent_Commission_Ac)
        vVou_Amts = Val(txt_CommAmt.Text) & "|" & -1 * Val(txt_CommAmt.Text)
        If Common_Procedures.Voucher_Updation(con, "Ag.Comm", Val(lbl_Company.Tag), Trim(Pk_Condition2) & Trim(NewCode), Trim(vEntBillNo), Convert.ToDateTime(dtp_Date.Text), "Cnv.Bill No : " & Trim(vEntBillNo) & ", Mtrs : " & Trim(Format(Val(vTotMtrs1), "#########0.00")), vLed_IdNos, vVou_Amts, ErrMsg, tr, Common_Procedures.SoftwareTypes.Textile_Software) = False Then
            Throw New ApplicationException(ErrMsg)
        End If


        Dim VouBil As String = ""
        VouBil = Common_Procedures.VoucherBill_Posting(con, Val(lbl_Company.Tag), Convert.ToDateTime(msk_Date.Text), Led_ID, Trim(lbl_BillNo.Text), Agt_Idno, Format(Val(CSng(lbl_NetAmount.Text)), "#########0.00"), "DR", Trim(Pk_Condition) & Trim(NewCode), tr, Common_Procedures.SoftwareTypes.Textile_Software)
        If Trim(UCase(VouBil)) = "ERROR" Then
                Throw New ApplicationException("Error on Voucher Bill Posting")
            End If

        'Cr_ID = SalAc_ID
        'Dr_ID = Led_ID

        'cmd.CommandText = "Delete from Voucher_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Entry_Identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
        'cmd.ExecuteNonQuery()
        'cmd.CommandText = "Delete from Voucher_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Entry_Identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
        'cmd.ExecuteNonQuery()

        'cmd.CommandText = "Insert into Voucher_Head (                 Voucher_Code               , For_OrderByCode, Company_IdNo, Voucher_No, For_OrderBy, Voucher_Type, Voucher_Date, Debtor_Idno, Creditor_Idno, Total_VoucherAmount, Narration, Indicate, Year_For_Report, Entry_Identification, Voucher_Receipt_Code) " & _
        '                    "          Values       ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(vOrdBy)) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_BillNo.Text) & "', " & Str(Val(vOrdBy)) & ", 'Jw.Invoice', @BillDate, " & Str(Val(Cr_ID)) & ", " & Str(Val(Dr_ID)) & ", " & Str(Val(lbl_NetAmount.Text)) & ", 'Bill No. : " & Trim(vEntBillNo) & "', 1, " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(Pk_Condition) & Trim(NewCode) & "', '')"
        'cmd.ExecuteNonQuery()

        'cmd.CommandText = "Insert into Voucher_Details(Voucher_Code, For_OrderByCode, Company_IdNo, Voucher_No, For_OrderBy, Voucher_Type, Voucher_Date, SL_No, Ledger_IdNo, Voucher_Amount, Narration, Year_For_Report, Entry_Identification ) " & _
        '                  " Values             ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(vOrdBy)) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_BillNo.Text) & "', " & Str(Val(vOrdBy)) & ", 'Jw.Invoice', @BillDate, 1, " & Str(Val(Cr_ID)) & ", " & Str(Val(lbl_NetAmount.Text)) & ", 'Bill No. : " & Trim(vEntBillNo) & "', " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(Pk_Condition) & Trim(NewCode) & "' )"
        'cmd.ExecuteNonQuery()

        'cmd.CommandText = "Insert into Voucher_Details(Voucher_Code, For_OrderByCode, Company_IdNo, Voucher_No, For_OrderBy, Voucher_Type, Voucher_Date, SL_No, Ledger_IdNo, Voucher_Amount, Narration, Year_For_Report, Entry_Identification ) " & _
        '                  " Values             ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(vOrdBy)) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_BillNo.Text) & "', " & Str(Val(vOrdBy)) & ", 'Jw.Invoice', @BillDate, 2, " & Str(Val(Dr_ID)) & ", " & Str(-1 * Val(lbl_NetAmount.Text)) & ", 'Bill No. : " & Trim(vEntBillNo) & "', " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(Pk_Condition) & Trim(NewCode) & "' )"
        'cmd.ExecuteNonQuery()


        '--Tds A/c Posting
        Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(PkCondition_JWTDS) & Trim(NewCode), tr)
        vLed_IdNos = ""
        vVou_Amts = ""
        ErrMsg = ""


        vLed_IdNos = Val(Common_Procedures.CommonLedger.TDS_Receivable_Ac) & "|" & Led_ID
        vVou_Amts = -1 * Val(CSng(lbl_Tds_Amount.Text)) & "|" & Val(CSng(lbl_Tds_Amount.Text))

        If Common_Procedures.Voucher_Updation(con, "JobWrk.Tds", Val(lbl_Company.Tag), Trim(PkCondition_JWTDS) & Trim(NewCode), Trim(vEntBillNo), msk_Date.Text, "Bill No : " & Trim(vEntBillNo), vLed_IdNos, vVou_Amts, ErrMsg, tr, Common_Procedures.SoftwareTypes.Textile_Software) = False Then
            Throw New ApplicationException(ErrMsg)
        End If

        tr.Commit()

            Dt1.Dispose()
            Da.Dispose()

            move_record(lbl_BillNo.Text)

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            If msk_Date.Enabled And msk_Date.Visible Then msk_Date.Focus()

        End Try

    End Sub

    Private Sub dtp_Date_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Grid_Cell_DeSelect()
    End Sub

    Private Sub dtp_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyCode = 40 Then e.Handled = True : SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then e.Handled = True : txt_AddLess.Focus() ' SendKeys.Send("+{TAB}")
    End Sub

    Private Sub dtp_Date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
    End Sub
    Private Sub cbo_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_IdNo = 0 or  Ledger_Type = 'JOBWORKER')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, cbo_Type, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(ledger_idno = 0 or Ledger_Type = 'JOBWORKER' )", "(Ledger_IdNo = 0)")
        If (e.KeyValue = 40 And cbo_Ledger.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If Trim(UCase(cbo_Type.Text)) = "DELIVERY" Then


                If MessageBox.Show("Do you want to select delivery :", "FOR DELIVERY SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                    btn_Selection_Click(sender, e)

                Else
                    If Common_Procedures.settings.CustomerCode = "1186" Then
                        cbo_DeliveryTo.Focus()

                    Else
                        txt_LrNo.Focus()
                    End If


                End If
            Else
                If Common_Procedures.settings.CustomerCode = "1186" Then
                    cbo_DeliveryTo.Focus()
                Else
                    txt_LrNo.Focus()
                End If
            End If
        End If
    End Sub

    Private Sub cbo_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ledger.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Ledger, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(ledger_idno = 0 or Ledger_Type = 'JOBWORKER' )", "(Ledger_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            If Trim(UCase(cbo_Type.Text)) = "DELIVERY" Then


                If MessageBox.Show("Do you want to select delivery :", "FOR DELIVERY SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                    btn_Selection_Click(sender, e)

                Else
                    If Common_Procedures.settings.CustomerCode = "1186" Then
                        cbo_DeliveryTo.Focus()
                    Else
                        txt_LrNo.Focus()
                    End If

                End If
            Else
                If Common_Procedures.settings.CustomerCode = "1186" Then
                    cbo_DeliveryTo.Focus()
                Else
                    txt_LrNo.Focus()
                End If

            End If
            'Get_HSN_CodeWise_Tax_Details()

        End If
    End Sub


    Public Sub Get_vehicle_from_Transport()

        If Common_Procedures.settings.CustomerCode <> "1186" Then Exit Sub

        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim transport_id As Integer
        transport_id = Common_Procedures.Ledger_NameToIdNo(con, cbo_Transport.Text)
        Da = New SqlClient.SqlDataAdapter("select vehicle_no from ledger_head where ledger_idno=" & Str(Val(transport_id)) & "", con)
        Dt = New DataTable
        Da.Fill(Dt)
        If Dt.Rows.Count <> 0 Then
            cbo_VehicleNo.Text = Dt.Rows(0).Item("vehicle_no").ToString


        End If
        Dt.Clear()
    End Sub
    Private Sub cbo_Transport_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Transport.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")

        Get_vehicle_from_Transport()
    End Sub

    Private Sub cbo_Transport_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Transport.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Transport, cbo_DeliveryTo, cbo_VehicleNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")

        Get_vehicle_from_Transport()
    End Sub

    Private Sub cbo_Transport_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Transport.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Transport, cbo_VehicleNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")

        Get_vehicle_from_Transport()
    End Sub

    Private Sub cbo_Vechile_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_VehicleNo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "JobWork_ConversionBill_Head", "Vehicle_No", "", "")
    End Sub

    Private Sub cbo_Vechile_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_VehicleNo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_VehicleNo, cbo_Transport, txt_FormJJNo, "JobWork_ConversionBill_Head", "Vehicle_No", "", "")
        'If (e.KeyValue = 40 And cbo_VehicleNo.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
        '    If Trim(UCase(cbo_Type.Text)) = "DELIVERY" Then

        '        txt_Rate_Type1.Focus()
        '    Else
        '        If dgv_Details.RowCount > 0 Then


        '            dgv_Details.Focus()
        '            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
        '        Else
        '            txt_Rate_Type1.Focus()
        '        End If

        '    End If
        'End If
    End Sub

    Private Sub cbo_Vechile_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_VehicleNo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_VehicleNo, txt_FormJJNo, "JobWork_ConversionBill_Head", "Vehicle_No", "", "", False)
        'If Asc(e.KeyChar) = 13 Then
        '    If Trim(UCase(cbo_Type.Text)) = "DELIVERY" Then
        '        txt_Rate_Type1.Focus()
        '    Else
        '        If dgv_Details.RowCount > 0 Then
        '            dgv_Details.Focus()
        '            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
        '        Else
        '            txt_Rate_Type1.Focus()
        '        End If

        '    End If
        'End If
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
                Condt = "a.JobWork_ConversionBill_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.JobWork_ConversionBill_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.JobWork_ConversionBill_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If


            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.Ledger_IdNo = " & Str(Val(Led_IdNo))
            End If

            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name from JobWork_ConversionBill_Head a INNER JOIN Ledger_Head b on a.Ledger_IdNo = b.Ledger_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.JobWork_ConversionBill_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.JobWork_ConversionBill_RefNo", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("JobWork_ConversionBill_RefNo").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("JobWork_ConversionBill_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = Val(dt2.Rows(i).Item("Total_Rolls").ToString)
                    dgv_Filter_Details.Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("Total_Meters").ToString), "########0.00")
                    dgv_Filter_Details.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Net_Amount").ToString), "########0.00")

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
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_IdNo = 0 or  Ledger_Type = 'JOBWORKER' or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, dtp_Filter_ToDate, btn_Filter_Show, "Ledger_AlaisHead", "Ledger_DisplayName", "(ledger_idno = 0 or Ledger_Type = 'JOBWORKER' or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, btn_Filter_Show, "Ledger_AlaisHead", "Ledger_DisplayName", "(ledger_idno = 0 or Ledger_Type = 'JOBWORKER' or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")
    End Sub



    Private Sub Open_FilterEntry()
        Dim movno As String

        On Error Resume Next

        movno = Trim(dgv_Filter_Details.CurrentRow.Cells(1).Value)

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
        On Error Resume Next
        With dgv_Details
            If .Visible Then
                If .CurrentCell.ColumnIndex >= dgvCol_Details.Pcs And .CurrentCell.ColumnIndex <= dgvCol_Details.Total_Meter Then
                    Total_Calculation()
                End If
            End If
        End With
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


            If e.ColumnIndex = dgvCol_Details.ClothName Then

                If cbo_Grid_ClothName.Visible = False And Trim(UCase(cbo_Type.Text)) <> "DELIVERY" Or Val(cbo_Grid_ClothName.Tag) <> e.RowIndex Then

                    cbo_Grid_ClothName.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Cloth_Name from Cloth_Head order by Cloth_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Grid_ClothName.DataSource = Dt1
                    cbo_Grid_ClothName.DisplayMember = "Cloth_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_ClothName.Left = .Left + rect.Left
                    cbo_Grid_ClothName.Top = .Top + rect.Top

                    cbo_Grid_ClothName.Width = rect.Width
                    cbo_Grid_ClothName.Height = rect.Height
                    cbo_Grid_ClothName.Text = .CurrentCell.Value

                    cbo_Grid_ClothName.Tag = Val(e.RowIndex)
                    cbo_Grid_ClothName.Visible = True

                    cbo_Grid_ClothName.BringToFront()
                    cbo_Grid_ClothName.Focus()

                End If

            Else
                cbo_Grid_ClothName.Visible = False

            End If


        End With

    End Sub

    Private Sub dgv_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellLeave
        With dgv_Details
            If .CurrentCell.ColumnIndex >= dgvCol_Details.Dc_Mtr And .CurrentCell.ColumnIndex <= dgvCol_Details.Total_Meter And .CurrentCell.ColumnIndex <= dgvCol_Details.Sound_Amount And .CurrentCell.ColumnIndex <= dgvCol_Details.Seconds_Amount And .CurrentCell.ColumnIndex <= dgvCol_Details.Bits_Amount And .CurrentCell.ColumnIndex <= dgvCol_Details.Reject_Amount And .CurrentCell.ColumnIndex <= dgvCol_Details.Others_Amount Then
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
        If Not IsNothing(dgv_Details.CurrentCell) Then
            With dgv_Details
                If .Visible Then

                    If e.ColumnIndex = dgvCol_Details.Dc_Mtr Or e.ColumnIndex = dgvCol_Details.Sound_Rate Or e.ColumnIndex = dgvCol_Details.Seconds_Rate Or e.ColumnIndex = dgvCol_Details.Bits_Rate Or e.ColumnIndex = dgvCol_Details.Reject_Rate Or e.ColumnIndex = dgvCol_Details.Others_Rate Then

                        .Rows(e.RowIndex).Cells(dgvCol_Details.Sound_Amount).Value = Format(Val(.Rows(e.RowIndex).Cells(dgvCol_Details.Type1_Meter).Value) * Val(.Rows(e.RowIndex).Cells(dgvCol_Details.Sound_Rate).Value), "###########0.00")
                        .Rows(e.RowIndex).Cells(dgvCol_Details.Seconds_Amount).Value = Format(Val(.Rows(e.RowIndex).Cells(dgvCol_Details.Type2_Meter).Value) * Val(.Rows(e.RowIndex).Cells(dgvCol_Details.Seconds_Rate).Value), "###########0.00")
                        .Rows(e.RowIndex).Cells(dgvCol_Details.Bits_Amount).Value = Format(Val(.Rows(e.RowIndex).Cells(dgvCol_Details.Type3_Meter).Value) * Val(.Rows(e.RowIndex).Cells(dgvCol_Details.Bits_Rate).Value), "###########0.00")
                        .Rows(e.RowIndex).Cells(dgvCol_Details.Reject_Amount).Value = Format(Val(.Rows(e.RowIndex).Cells(dgvCol_Details.Type4_Meter).Value) * Val(.Rows(e.RowIndex).Cells(dgvCol_Details.Reject_Rate).Value), "###########0.00")
                        .Rows(e.RowIndex).Cells(dgvCol_Details.Others_Amount).Value = Format(Val(.Rows(e.RowIndex).Cells(dgvCol_Details.Type5_Meter).Value) * Val(.Rows(e.RowIndex).Cells(dgvCol_Details.Others_Rate).Value), "###########0.00")
                        'If Common_Procedures.settings.CustomerCode = "1186" Then
                        '    .Rows(e.RowIndex).Cells(dgvCol_Details.Total_Amount).Value = Format(Val(.Rows(e.RowIndex).Cells(dgvCol_Details.Total_Meter ).Value) * Val(.Rows(e.RowIndex).Cells(dgvCol_Details.Sound_Rate ).Value), "###########0.00")

                        'Else
                        .Rows(e.RowIndex).Cells(dgvCol_Details.Total_Amount).Value = Format(Val(.Rows(e.RowIndex).Cells(dgvCol_Details.Sound_Amount).Value) + Val(.Rows(e.RowIndex).Cells(dgvCol_Details.Seconds_Amount).Value) + Val(.Rows(e.RowIndex).Cells(dgvCol_Details.Bits_Amount).Value) + Val(.Rows(e.RowIndex).Cells(dgvCol_Details.Reject_Amount).Value) + Val(.Rows(e.RowIndex).Cells(dgvCol_Details.Others_Amount).Value), "###########0.00")

                        'End If

                    End If

                    If .CurrentCell.ColumnIndex >= dgvCol_Details.Pcs Or .CurrentCell.ColumnIndex <= dgvCol_Details.Total_Meter Or .CurrentCell.ColumnIndex <= dgvCol_Details.Sound_Amount Or .CurrentCell.ColumnIndex <= dgvCol_Details.Seconds_Amount Or .CurrentCell.ColumnIndex <= dgvCol_Details.Bits_Amount Or .CurrentCell.ColumnIndex <= dgvCol_Details.Reject_Amount Or .CurrentCell.ColumnIndex <= dgvCol_Details.Others_Amount Then
                        Total_Calculation()
                    End If
                End If
            End With
        End If

    End Sub

    Private Sub dgv_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Details.EditingControlShowing
        dgtxt_Details = CType(dgv_Details.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgv_Details_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Details.RowsAdded
        Dim n As Integer = 0

        Try
            With dgv_Details

                n = .RowCount
                .Rows(n - 1).Cells(0).Value = Val(n)

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS ROWS ADD....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub




    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.Enter
        dgv_Details.EditingControl.BackColor = Color.Lime
    End Sub

    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Details.KeyPress
        If dgv_Details.CurrentCell.ColumnIndex = dgvCol_Details.DC_Date Or dgv_Details.CurrentCell.ColumnIndex = dgvCol_Details.Sound_Rate Or dgv_Details.CurrentCell.ColumnIndex = dgvCol_Details.Seconds_Rate Or dgv_Details.CurrentCell.ColumnIndex = dgvCol_Details.Bits_Rate Or dgv_Details.CurrentCell.ColumnIndex = dgvCol_Details.Reject_Rate Or dgv_Details.CurrentCell.ColumnIndex = dgvCol_Details.Others_Rate Then
            If Common_Procedures.Accept_NegativeNumbers(Asc(e.KeyChar)) = 0 Then
                e.Handled = True
            End If

        ElseIf dgv_Details.CurrentCell.ColumnIndex <> dgvCol_Details.DC_No Then
            If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                e.Handled = True
            End If

        End If
    End Sub

    Private Sub dgv_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyUp
        Dim n As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_Details

                NoCalc_Status = True

                n = .CurrentRow.Index

                If .Rows.Count = 1 Then
                    For i = 0 To .Columns.Count - 1
                        .Rows(n).Cells(i).Value = ""
                    Next

                Else
                    .Rows.RemoveAt(n)

                End If

                For i = 0 To .Rows.Count - 1
                    .Rows(i).Cells(dgvCol_Details.Slno).Value = i + 1
                Next

            End With

            NoCalc_Status = False
            Total_Calculation()

        End If

    End Sub

    Private Sub cbo_Ledger_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Common_Procedures.MDI_LedType = "JOBWORKER"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Ledger.Name
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


    Private Sub btn_Selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Selection.Click
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim i As Integer, j As Integer, n As Integer, SNo As Integer
        Dim LedIdNo As Integer
        Dim NewCode As String
        Dim Da2 As New SqlClient.SqlDataAdapter
        Dim Dt2 As New DataTable
        Dim vPCSCHK_CONDT As String = ""
        Dim Ent_DcMtr As String = ""
        Dim Ent_ActulMtr As String = ""
        Dim Ent_Mtrs_1 As Single = 0
        Dim Ent_Mtrs_2 As Single = 0
        Dim Ent_Mtrs_3 As Single = 0
        Dim Ent_Mtrs_4 As Single = 0
        Dim Ent_Mtrs_5 As Single = 0

        Dim Ent_Sound_Rate As Single = 0
        Dim Ent_Bit_Rate As Single = 0
        Dim Ent_Second_Rate As Single = 0
        Dim Ent_Reject_Rate As Single = 0
        Dim Ent_Other_Rate As Single = 0

        Dim vSELCCode As String = ""

        Dim vFOLDPERC As String = 0, vDCMTR_100FOLDMTRS As String = 0
        Dim vTYPE1_100FOLDMTRS As String = 0, vTYPE2_100FOLDMTRS As String = 0, vTYPE3_100FOLDMTRS As String = 0, vTYPE4_100FOLDMTRS As String = 0, vTYPE5_100FOLDMTRS As String = 0


        LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)

        If LedIdNo = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SELECT PIECE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_BillNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)



        vPCSCHK_CONDT = " isnull(a.JobWork_Inspection_Code,'') <> '' "
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1608" Then '---- SAMANTH TEXTILES (SOMANUR)
            vPCSCHK_CONDT = " isnull(a.JobWork_Inspection_Code,'') <> '-00000/CLOSE' "
        End If

        With dgv_Selection

            .Rows.Clear()

            SNo = 0


            For K = 1 To 2

                vSELCCode = ""
                If K = 1 Then
                    vSELCCode = Trim(NewCode)
                End If

                Da = New SqlClient.SqlDataAdapter("select a.jobwork_piece_delivery_code, a.jobwork_piece_delivery_No,a.jobwork_piece_delivery_Date,a.folding,a.Inspection_Folding,count(j.Sl_no) as Rolls,sum(j.cloth_Type1_meters) as sounds,sum(j.cloth_Type2_meters) as seconds,sum(j.cloth_Type3_meters) as BIts,sum(j.cloth_Type4_meters) as Reject,sum(j.cloth_Type5_meters) as Others,sum(j.cloth_Type1_meters+j.cloth_Type2_meters+j.cloth_Type3_meters+j.cloth_Type4_meters+j.cloth_Type5_meters) as meters, sum(j.Meters) as pcsdelv_meters, b.Cloth_Name,b.Wages_For_Type1 as Sound_Rate,b. Wages_For_Type2 as Seconds_Rate,b.Wages_For_Type3 as Bits_Rate,b.Wages_For_Type4 as Other_Rate,b.Wages_For_Type5 as Reject_Rate,j.Po_no,  " &
                                                " JD.Delivery_Meters AS Delv_mtr , JD.Actual_Meters as Actul_Mtr ,JD.Type1_Meters as Typ_Mtr_1,JD.Type2_Meters as Typ_Mtr_2,JD.Type3_Meters as Typ_Mtr_3,JD.Type4_Meters as Typ_Mtr_4,JD.Type5_Meters as Typ_Mtr_5,JD.Sound_Rate AS Soud_Rate,JD.Seconds_Rate AS Sec_Rate,JD.Bits_Rate AS Bit_Rate,JD.Reject_Rate AS Rejct_Rate,JD.Others_Rate AS Othr_Rate  " &
                                                " from JobWork_Piece_Delivery_Head a  " &
                                                " INNER JOIN Cloth_Head b ON a.Cloth_IdNo = b.Cloth_IdNo " &
                                                " LEFT OUTER JOIN Ledger_Head del ON a.Delivery_Idno = del.Ledger_IdNo  " &
                                                " lEFT OUTER JOIN JobWork_Piece_Delivery_details J on j.jobwork_piece_delivery_code= a.jobwork_piece_delivery_code " &
                                                " LEFT OUTER JOIN JobWork_ConversionBill_Details JD ON JD.JobWork_ConversionBill_CODE = '" & Trim(NewCode) & "' and J.jobwork_piece_delivery_code = Jd.jobwork_piece_delivery_code and j.Po_No = jd.Po_No  " &
                                                " Where a.JobWork_Bill_Code = '" & Trim(vSELCCode) & "' and " & vPCSCHK_CONDT & " and a.ledger_Idno = " & Str(Val(LedIdNo)) & " " &
                                                " group by j.po_no, b.cloth_name, b.Wages_For_Type1, b.Wages_For_Type2, b.Wages_For_Type3, b.Wages_For_Type4, b.Wages_For_Type5, a.Total_Delivery_Meters, a.jobwork_piece_delivery_No, a.jobwork_piece_delivery_Date, a.jobwork_piece_delivery_code,a.folding,a.Inspection_Folding, JD.Delivery_Meters, JD.Actual_Meters, JD.Type1_Meters, JD.Type2_Meters, JD.Type3_Meters, JD.Type4_Meters, JD.Type5_Meters, " &
                                                " JD.Sound_Rate, JD.Seconds_Rate, JD.Bits_Rate, JD.Reject_Rate,JD.Others_Rate ", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)
                If Dt1.Rows.Count > 0 Then

                    For i = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()

                        SNo = SNo + 1


                        Ent_DcMtr = ""
                        Ent_ActulMtr = ""
                        Ent_Mtrs_1 = 0
                        Ent_Mtrs_2 = 0
                        Ent_Mtrs_3 = 0
                        Ent_Mtrs_4 = 0
                        Ent_Mtrs_5 = 0

                        Ent_Sound_Rate = 0
                        Ent_Bit_Rate = 0
                        Ent_Second_Rate = 0
                        Ent_Reject_Rate = 0
                        Ent_Other_Rate = 0


                        If IsDBNull(Dt1.Rows(i).Item("Delv_mtr").ToString) = False Then
                            Ent_DcMtr = Dt1.Rows(i).Item("Delv_mtr").ToString
                        End If
                        If IsDBNull(Dt1.Rows(i).Item("Actul_Mtr").ToString) = False Then
                            Ent_ActulMtr = Val(Dt1.Rows(i).Item("Actul_Mtr").ToString)
                        End If
                        If IsDBNull(Dt1.Rows(i).Item("Typ_Mtr_1").ToString) = False Then
                            Ent_Mtrs_1 = Val(Dt1.Rows(i).Item("Typ_Mtr_1").ToString)
                        End If
                        If IsDBNull(Dt1.Rows(i).Item("Typ_Mtr_2").ToString) = False Then
                            Ent_Mtrs_2 = Val(Dt1.Rows(i).Item("Typ_Mtr_2").ToString)
                        End If
                        If IsDBNull(Dt1.Rows(i).Item("Typ_Mtr_3").ToString) = False Then
                            Ent_Mtrs_3 = Val(Dt1.Rows(i).Item("Typ_Mtr_3").ToString)
                        End If
                        If IsDBNull(Dt1.Rows(i).Item("Typ_Mtr_4").ToString) = False Then
                            Ent_Mtrs_4 = Val(Dt1.Rows(i).Item("Typ_Mtr_4").ToString)
                        End If
                        If IsDBNull(Dt1.Rows(i).Item("Typ_Mtr_5").ToString) = False Then
                            Ent_Mtrs_5 = Val(Dt1.Rows(i).Item("Typ_Mtr_5").ToString)
                        End If


                        If IsDBNull(Dt1.Rows(i).Item("Soud_Rate").ToString) = False Then
                            Ent_Sound_Rate = Val(Dt1.Rows(i).Item("Soud_Rate").ToString)
                        End If
                        If IsDBNull(Dt1.Rows(i).Item("Sec_Rate").ToString) = False Then
                            Ent_Second_Rate = Val(Dt1.Rows(i).Item("Sec_Rate").ToString)
                        End If
                        If IsDBNull(Dt1.Rows(i).Item("Bit_Rate").ToString) = False Then
                            Ent_Bit_Rate = Val(Dt1.Rows(i).Item("Bit_Rate").ToString)
                        End If
                        If IsDBNull(Dt1.Rows(i).Item("Rejct_Rate").ToString) = False Then
                            Ent_Reject_Rate = Val(Dt1.Rows(i).Item("Rejct_Rate").ToString)
                        End If
                        If IsDBNull(Dt1.Rows(i).Item("Othr_Rate").ToString) = False Then
                            Ent_Other_Rate = Val(Dt1.Rows(i).Item("Othr_Rate").ToString)
                        End If


                        .Rows(n).Cells(0).Value = Val(SNo)
                        .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("JobWork_Piece_Delivery_No").ToString
                        .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("JobWork_Piece_Delivery_Date").ToString), "dd-MM-yyyy").ToString
                        .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Cloth_Name").ToString
                        .Rows(n).Cells(4).Value = Val(Dt1.Rows(i).Item("Rolls").ToString)
                        .Rows(n).Cells(7).Value = ""
                        If Trim(vSELCCode) <> "" Then
                            .Rows(n).Cells(7).Value = "1"
                            For j = 0 To .ColumnCount - 1
                                .Rows(n).Cells(j).Style.ForeColor = Color.Red
                            Next

                        End If

                        .Rows(n).Cells(8).Value = Dt1.Rows(i).Item("JobWork_Piece_Delivery_Code").ToString

                        dgv_Selection.Rows(n).Cells(20).Value = Dt1.Rows(i).Item("Po_no").ToString

                        If Val(Ent_DcMtr) <> 0 Or Val(Ent_Mtrs_1) <> 0 Or Val(Ent_Sound_Rate) <> 0 Or Val(Ent_Mtrs_2) <> 0 Or Val(Ent_Second_Rate) <> 0 Or Val(Ent_Mtrs_3) <> 0 Or Val(Ent_Bit_Rate) <> 0 Or Val(Ent_Mtrs_4) <> 0 Or Val(Ent_Reject_Rate) <> 0 Or Val(Ent_Mtrs_5) <> 0 Or Val(Ent_Other_Rate) <> 0 Then

                            .Rows(n).Cells(5).Value = Val(Ent_DcMtr)
                            .Rows(n).Cells(6).Value = Val(Ent_Mtrs_1) + Val(Ent_Mtrs_2) + Val(Ent_Mtrs_3) + Val(Ent_Mtrs_4) + Val(Ent_Mtrs_5)
                            .Rows(n).Cells(9).Value = Val(Ent_Mtrs_1)
                            .Rows(n).Cells(10).Value = Val(Ent_Mtrs_2)
                            .Rows(n).Cells(11).Value = Val(Ent_Mtrs_3)
                            .Rows(n).Cells(12).Value = Val(Ent_Mtrs_4)
                            .Rows(n).Cells(13).Value = Val(Ent_Mtrs_5)

                            dgv_Selection.Rows(n).Cells(15).Value = Val(Ent_Sound_Rate)
                            dgv_Selection.Rows(n).Cells(16).Value = Val(Ent_Second_Rate)
                            dgv_Selection.Rows(n).Cells(17).Value = Val(Ent_Bit_Rate)
                            dgv_Selection.Rows(n).Cells(18).Value = Val(Ent_Reject_Rate)
                            dgv_Selection.Rows(n).Cells(19).Value = Val(Ent_Other_Rate)

                        Else

                            vFOLDPERC = Dt1.Rows(i).Item("Inspection_Folding").ToString
                            If Val(vFOLDPERC) = 0 Then vFOLDPERC = 100

                            vDCMTR_100FOLDMTRS = Format(Val(Dt1.Rows(i).Item("pcsdelv_meters").ToString) * Val(vFOLDPERC) / 100, "#########0.00")
                            vTYPE1_100FOLDMTRS = Format(Val(Dt1.Rows(i).Item("Sounds").ToString) * Val(vFOLDPERC) / 100, "#########0.00")
                            vTYPE2_100FOLDMTRS = Format(Val(Dt1.Rows(i).Item("Seconds").ToString) * Val(vFOLDPERC) / 100, "#########0.00")
                            vTYPE3_100FOLDMTRS = Format(Val(Dt1.Rows(i).Item("Bits").ToString) * Val(vFOLDPERC) / 100, "#########0.00")
                            vTYPE4_100FOLDMTRS = Format(Val(Dt1.Rows(i).Item("Reject").ToString) * Val(vFOLDPERC) / 100, "#########0.00")
                            vTYPE5_100FOLDMTRS = Format(Val(Dt1.Rows(i).Item("Others").ToString) * Val(vFOLDPERC) / 100, "#########0.00")

                            .Rows(n).Cells(5).Value = Format(Val(vDCMTR_100FOLDMTRS), "#########0.00")
                            .Rows(n).Cells(6).Value = Format(Val(vTYPE1_100FOLDMTRS) + Val(vTYPE2_100FOLDMTRS) + Val(vTYPE3_100FOLDMTRS) + Val(vTYPE4_100FOLDMTRS) + Val(vTYPE5_100FOLDMTRS), "#########0.00")
                            .Rows(n).Cells(9).Value = Format(Val(vTYPE1_100FOLDMTRS), "#########0.00")
                            .Rows(n).Cells(10).Value = Format(Val(vTYPE2_100FOLDMTRS), "#########0.00")
                            .Rows(n).Cells(11).Value = Format(Val(vTYPE3_100FOLDMTRS), "#########0.00")
                            .Rows(n).Cells(12).Value = Format(Val(vTYPE4_100FOLDMTRS), "#########0.00")
                            .Rows(n).Cells(13).Value = Format(Val(vTYPE5_100FOLDMTRS), "#########0.00")

                            dgv_Selection.Rows(n).Cells(15).Value = Dt1.Rows(i).Item("Sound_Rate").ToString
                            dgv_Selection.Rows(n).Cells(16).Value = Dt1.Rows(i).Item("Seconds_Rate").ToString
                            dgv_Selection.Rows(n).Cells(17).Value = Dt1.Rows(i).Item("Bits_Rate").ToString
                            dgv_Selection.Rows(n).Cells(18).Value = Dt1.Rows(i).Item("Reject_Rate").ToString
                            dgv_Selection.Rows(n).Cells(19).Value = Dt1.Rows(i).Item("Other_Rate").ToString

                        End If

                    Next

                End If

            Next K

            Dt1.Clear()

        End With

        pnl_Selection.Visible = True
        pnl_Back.Enabled = False
        dgv_Selection.Focus()

    End Sub

    Private Sub dgv_Selection_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Selection.CellClick
        Select_Dc(e.RowIndex)
    End Sub

    Private Sub Select_Dc(ByVal RwIndx As Integer)
        Dim i As Integer

        With dgv_Selection

            If .RowCount > 0 And RwIndx >= 0 Then

                .Rows(RwIndx).Cells(7).Value = (Val(.Rows(RwIndx).Cells(7).Value) + 1) Mod 2

                If Val(.Rows(RwIndx).Cells(7).Value) = 0 Then .Rows(RwIndx).Cells(7).Value = ""

                If Val(.Rows(RwIndx).Cells(7).Value) = 0 Then
                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Black
                    Next
                    .DefaultCellStyle.SelectionForeColor = Color.Black

                Else

                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
                    Next
                    .DefaultCellStyle.SelectionForeColor = Color.Red

                End If

            End If

        End With

    End Sub

    Private Sub dgv_Selection_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Selection.KeyDown
        On Error Resume Next

        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Space Then
            If dgv_Selection.CurrentCell.RowIndex >= 0 Then
                e.Handled = True
                Select_Dc(dgv_Selection.CurrentCell.RowIndex)
            End If
        End If
    End Sub


    Private Sub btn_Close_Selection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Selection.Click
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim i As Integer, n As Integer
        Dim sno As Integer = 0
        Dim Clo_IdNo As Integer = 0
        Dim Da2 As New SqlClient.SqlDataAdapter
        Dim Dt2 As New DataTable
        Dim NewCode As String


        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_BillNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        dgv_Details.Rows.Clear()

        NoCalc_Status = True

        sno = 0
        Clo_IdNo = 0

        For i = 0 To dgv_Selection.RowCount - 1

            If Val(dgv_Selection.Rows(i).Cells(7).Value) = 1 Then

                'If Val(Clo_IdNo) = 0 Then Clo_IdNo = Common_Procedures.Cloth_NameToIdNo(con, dgv_Selection.Rows(i).Cells(3).Value)

                n = dgv_Details.Rows.Add()

                sno = sno + 1
                dgv_Details.Rows(n).Cells(dgvCol_Details.Slno).Value = dgv_Selection.Rows(i).Cells(0).Value
                dgv_Details.Rows(n).Cells(dgvCol_Details.DC_No).Value = dgv_Selection.Rows(i).Cells(1).Value
                dgv_Details.Rows(n).Cells(dgvCol_Details.DC_Date).Value = dgv_Selection.Rows(i).Cells(2).Value
                dgv_Details.Rows(n).Cells(dgvCol_Details.ClothName).Value = dgv_Selection.Rows(i).Cells(3).Value
                dgv_Details.Rows(n).Cells(dgvCol_Details.Pcs).Value = dgv_Selection.Rows(i).Cells(4).Value
                dgv_Details.Rows(n).Cells(dgvCol_Details.Dc_Mtr).Value = dgv_Selection.Rows(i).Cells(5).Value
                dgv_Details.Rows(n).Cells(dgvCol_Details.Type1_Meter).Value = dgv_Selection.Rows(i).Cells(9).Value
                dgv_Details.Rows(n).Cells(dgvCol_Details.Type2_Meter).Value = dgv_Selection.Rows(i).Cells(10).Value
                dgv_Details.Rows(n).Cells(dgvCol_Details.Type3_Meter).Value = dgv_Selection.Rows(i).Cells(11).Value
                dgv_Details.Rows(n).Cells(dgvCol_Details.Type4_Meter).Value = dgv_Selection.Rows(i).Cells(12).Value
                dgv_Details.Rows(n).Cells(dgvCol_Details.Type5_Meter).Value = dgv_Selection.Rows(i).Cells(13).Value

                If Val(dgv_Details.Rows(n).Cells(dgvCol_Details.Type1_Meter).Value) = 0 And Val(dgv_Details.Rows(n).Cells(dgvCol_Details.Type2_Meter).Value) = 0 And Val(dgv_Details.Rows(n).Cells(dgvCol_Details.Type3_Meter).Value) = 0 And Val(dgv_Details.Rows(n).Cells(dgvCol_Details.Type4_Meter).Value) = 0 And Val(dgv_Details.Rows(n).Cells(dgvCol_Details.Type5_Meter).Value) = 0 Then
                    dgv_Details.Rows(n).Cells(dgvCol_Details.Type1_Meter).Value = dgv_Details.Rows(n).Cells(dgvCol_Details.Dc_Mtr).Value
                End If

                ' dgv_Details.Rows(n).Cells(dgvCol_Details.Actual_Mtr).Value = Dt2.Rows(i).Item("").ToString
                'dgv_Details.Rows(n).Cells(dgvCol_Details.Total_Meter).Value = Dt2.Rows(i).Item("").ToString
                dgv_Details.Rows(n).Cells(dgvCol_Details.JobWork_Piece_DeliveryCode).Value = dgv_Selection.Rows(i).Cells(8).Value
                dgv_Details.Rows(n).Cells(dgvCol_Details.Po_No).Value = dgv_Selection.Rows(i).Cells(20).Value
                dgv_Details.Rows(n).Cells(dgvCol_Details.Sound_Rate).Value = dgv_Selection.Rows(i).Cells(15).Value

                dgv_Details.Rows(n).Cells(dgvCol_Details.Seconds_Rate).Value = dgv_Selection.Rows(i).Cells(16).Value
                dgv_Details.Rows(n).Cells(dgvCol_Details.Bits_Rate).Value = dgv_Selection.Rows(i).Cells(17).Value
                dgv_Details.Rows(n).Cells(dgvCol_Details.Reject_Rate).Value = dgv_Selection.Rows(i).Cells(18).Value
                dgv_Details.Rows(n).Cells(dgvCol_Details.Others_Rate).Value = dgv_Selection.Rows(i).Cells(19).Value

            End If

        Next i


        NoCalc_Status = False

        Total_Calculation()

        Grid_Cell_DeSelect()

        pnl_Back.Enabled = True
        pnl_Selection.Visible = False
        If Common_Procedures.settings.CustomerCode = "1186" Then
            cbo_DeliveryTo.Focus()
        Else
            txt_LrNo.Focus()
        End If

    End Sub


    Private Sub dgv_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.LostFocus
        On Error Resume Next
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
    End Sub

    Private Sub txt_Rate_Type1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then
            If Trim(UCase(cbo_Type.Text)) = "DELIVERY" Then
                txt_FormJJNo.Focus()

            Else
                If dgv_Details.RowCount > 0 Then
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCol_Details.DC_No)
                Else
                    txt_FormJJNo.Focus()
                End If

            End If
        End If
    End Sub

    Private Sub txt_Rate_Type1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub txt_Rate_Type2_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_Rate_Type2_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub txt_Rate_Type3_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_Rate_Type3_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub txt_Rate_Type4_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_Rate_Type4_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub txt_Rate_Type5_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyCode = 40 Then cbo_Agent.Focus()
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_Rate_Type5_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Asc(e.KeyChar) = 13 Then cbo_Agent.Focus()
    End Sub

    Private Sub txt_AddLess_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_AddLess.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub txt_Freight_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_Freight.TextChanged
        NetAmount_Calculation()
    End Sub




    Private Sub txt_Rate_Type1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        NetAmount_Calculation()
    End Sub

    Private Sub txt_Rate_Type2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        NetAmount_Calculation()
    End Sub

    Private Sub txt_Rate_Type3_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        NetAmount_Calculation()
    End Sub

    Private Sub txt_Rate_Type4_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        NetAmount_Calculation()
    End Sub

    Private Sub txt_Rate_Type5_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        NetAmount_Calculation()
    End Sub

    Private Sub txt_DespatchTo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_DespatchTo.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_DespatchTo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_DespatchTo.KeyPress
        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub dtp_LrDate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_LrDate.KeyDown
        If e.KeyCode = 40 Then e.SuppressKeyPress = True : SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then e.SuppressKeyPress = True : SendKeys.Send("+{TAB}")
    End Sub

    Private Sub dtp_LrDate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_LrDate.KeyPress
        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub txt_LrNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_LrNo.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_LrNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_LrNo.KeyPress
        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub txt_Freight_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Freight.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}") ' SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_Freight_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Freight.KeyPress
        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub txt_AddLess_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_AddLess.KeyDown

    End Sub

    Private Sub txt_AddLess_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_AddLess.KeyPress
        If Asc(e.KeyChar) = 13 Then
            'If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
            '    save_record()
            'Else
            '    msk_Date.Focus()
            'End If
            '  txt_CGST_Perc.Focus()

        End If
    End Sub
    Private Sub btn_EMail_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_EMail.Click
        Dim Led_IdNo As Integer
        Dim MailTxt As String

        Try

            Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)

            MailTxt = "INVOICE " & vbCrLf & vbCrLf
            MailTxt = MailTxt & "Invoice No.-" & Trim(lbl_BillNo.Text) & vbCrLf & "Date-" & Trim(dtp_Date.Text)
            MailTxt = MailTxt & vbCrLf & "Lr No.-" & Trim(txt_LrNo.Text) & IIf(Trim(txt_LrNo.Text) <> "", " Dt.", "")
            MailTxt = MailTxt & vbCrLf & "Value-" & Trim(lbl_NetAmount.Text)

            EMAIL_Entry.vMailID = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_Mail", "(Ledger_IdNo = " & Str(Val(Led_IdNo)) & ")")
            EMAIL_Entry.vSubJect = "Invocie : " & Trim(lbl_BillNo.Text)
            EMAIL_Entry.vMessage = Trim(MailTxt)

            Dim f1 As New EMAIL_Entry
            f1.MdiParent = MDIParent1
            f1.Show()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SEND MAIL...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub btn_SMS_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_SMS.Click
        Dim i As Integer = 0
        Dim smstxt As String = ""
        Dim PhNo As String = ""
        Dim Led_IdNo As Integer = 0
        Dim SMS_SenderID As String = ""
        Dim SMS_Key As String = ""
        Dim SMS_RouteID As String = ""
        Dim SMS_Type As String = ""


        Try

            Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)

            PhNo = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_MobileNo", "(Ledger_IdNo = " & Str(Val(Led_IdNo)) & ")")

            smstxt = "INVOICE " & vbCrLf & vbCrLf
            smstxt = smstxt & "Invoice No.-" & Trim(lbl_BillNo.Text) & vbCrLf & "Date-" & Trim(dtp_Date.Text)
            smstxt = smstxt & vbCrLf & "Lr No.-" & Trim(txt_LrNo.Text) & IIf(Trim(txt_LrNo.Text) <> "", " Dt.", "")
            smstxt = smstxt & vbCrLf & "Value-" & Trim(lbl_NetAmount.Text)

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
    Private Sub Total_Calculation()
        Dim Sno As Integer
        Dim TotRls As Single, TotDcMtrs As Single, TotActMtrs As Single
        Dim TotMtrs1 As Single, TotMtrs2 As Single, TotMtrs3 As Single
        Dim TotMtrs4 As Single, TotMtrs5 As Single, NtTtMtrs As Single
        Dim TotAmt As String = 0
        Dim Typ1_Amt As String, Typ2_Amt As String, Typ3_Amt As String, Typ4_Amt As String, Typ5_Amt As String
        If NoCalc_Status = True Then Exit Sub

        Sno = 0
        TotRls = 0
        TotDcMtrs = 0 : TotActMtrs = 0
        TotMtrs1 = 0 : TotMtrs2 = 0 : TotMtrs3 = 0
        TotMtrs4 = 0 : TotMtrs5 = 0 : NtTtMtrs = 0
        TotAmt = 0


        Typ1_Amt = 0 : Typ2_Amt = 0 : Typ3_Amt = 0 : Typ4_Amt = 0 : Typ5_Amt = 0

        With dgv_Details
            For i = 0 To .RowCount - 1
                Sno = Sno + 1
                .Rows(i).Cells(dgvCol_Details.Slno).Value = Sno
                If Trim(.Rows(i).Cells(dgvCol_Details.DC_No).Value) <> "" And Val(.Rows(i).Cells(dgvCol_Details.Dc_Mtr).Value) <> 0 Then

                    .Rows(i).Cells(dgvCol_Details.Total_Meter).Value = Val(.Rows(i).Cells(dgvCol_Details.Type1_Meter).Value) + Val(.Rows(i).Cells(dgvCol_Details.Type2_Meter).Value) + Val(.Rows(i).Cells(dgvCol_Details.Type3_Meter).Value) + Val(.Rows(i).Cells(dgvCol_Details.Type4_Meter).Value) + Val(.Rows(i).Cells(dgvCol_Details.Type5_Meter).Value)
                    'If Common_Procedures.settings.CustomerCode = "1186" Then
                    '.Rows(i).Cells(dgvCol_Details.Total_Amount).Value = Val(.Rows(i).Cells(dgvCol_Details.Total_Meter).Value) * Val(.Rows(i).Cells(dgvCol_Details.Sound_Rate).Value)

                    'Else
                    .Rows(i).Cells(dgvCol_Details.Total_Amount).Value = Val(.Rows(i).Cells(dgvCol_Details.Sound_Amount).Value) + Val(.Rows(i).Cells(dgvCol_Details.Seconds_Amount).Value) + Val(.Rows(i).Cells(dgvCol_Details.Bits_Amount).Value) + Val(.Rows(i).Cells(dgvCol_Details.Reject_Amount).Value) + Val(.Rows(i).Cells(dgvCol_Details.Others_Amount).Value)

                    'End If

                    TotRls = TotRls + 1
                    TotDcMtrs = TotDcMtrs + Val(.Rows(i).Cells(dgvCol_Details.Dc_Mtr).Value)
                    TotActMtrs = TotActMtrs + Val(.Rows(i).Cells(dgvCol_Details.Actual_Mtr).Value)
                    TotMtrs1 = TotMtrs1 + Val(.Rows(i).Cells(dgvCol_Details.Type1_Meter).Value)
                    TotMtrs2 = TotMtrs2 + Val(.Rows(i).Cells(dgvCol_Details.Type2_Meter).Value)
                    TotMtrs3 = TotMtrs3 + Val(.Rows(i).Cells(dgvCol_Details.Type3_Meter).Value)
                    TotMtrs4 = TotMtrs4 + Val(.Rows(i).Cells(dgvCol_Details.Type4_Meter).Value)
                    TotMtrs5 = TotMtrs5 + Val(.Rows(i).Cells(dgvCol_Details.Type5_Meter).Value)
                    NtTtMtrs = NtTtMtrs + Val(.Rows(i).Cells(dgvCol_Details.Total_Meter).Value)

                    TotAmt = TotAmt + Val(.Rows(i).Cells(dgvCol_Details.Total_Amount).Value)





                End If

            Next

        End With

        With dgv_Details_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(4).Value = Val(TotRls)
            .Rows(0).Cells(5).Value = Format(Val(TotDcMtrs), "########0.00")
            .Rows(0).Cells(6).Value = Format(Val(TotActMtrs), "########0.00")
            .Rows(0).Cells(7).Value = Format(Val(TotMtrs1), "########0.00")
            .Rows(0).Cells(8).Value = Format(Val(TotMtrs2), "########0.00")
            .Rows(0).Cells(9).Value = Format(Val(TotMtrs3), "########0.00")
            .Rows(0).Cells(10).Value = Format(Val(TotMtrs4), "########0.00")
            .Rows(0).Cells(11).Value = Format(Val(TotMtrs5), "########0.00")
            .Rows(0).Cells(12).Value = Format(Val(NtTtMtrs), "########0.00")

            .Rows(0).Cells(24).Value = Format(Val(TotAmt), "########0.00")

        End With

        lbl_Meters_Type1.Text = Format(Val(TotMtrs1), "########0.00")
        lbl_Meters_Type2.Text = Format(Val(TotMtrs2), "########0.00")
        lbl_Meters_Type3.Text = Format(Val(TotMtrs3), "########0.00")
        lbl_Meters_Type4.Text = Format(Val(TotMtrs4), "########0.00")
        lbl_Meters_Type5.Text = Format(Val(TotMtrs5), "########0.00")


        lbl_TotalAmount.Text = Format(Val(TotAmt), "########0.00")

        TdsCommision_Calculation()

        ' NetAmount_Calculation()

    End Sub

    Private Sub NetAmount_Calculation()
        Dim NtAmt As Single

        If NoCalc_Status = True Then Exit Sub

        'lbl_Amount_Type1.Text = Format(Val(lbl_Meters_Type1.Text) * Val(txt_Rate_Type1.Text), "########0.00")
        'lbl_Amount_Type2.Text = Format(Val(lbl_Meters_Type2.Text) * Val(txt_Rate_Type2.Text), "########0.00")
        'lbl_Amount_Type3.Text = Format(Val(lbl_Meters_Type3.Text) * Val(txt_Rate_Type3.Text), "########0.00")
        'lbl_Amount_Type4.Text = Format(Val(lbl_Meters_Type4.Text) * Val(txt_Rate_Type4.Text), "########0.00")
        'lbl_Amount_Type5.Text = Format(Val(lbl_Meters_Type5.Text) * Val(txt_Rate_Type5.Text), "########0.00")


        'lbl_TotalAmount.Text = Format(Val(lbl_Amount_Type1.Text) + Val(lbl_Amount_Type2.Text) + Val(lbl_Amount_Type3.Text) + Val(lbl_Amount_Type4.Text) + Val(lbl_Amount_Type5.Text), "########0.00")

        lbl_TaxableValue.Text = Format(Val(lbl_TotalAmount.Text) + Val(txt_Freight.Text) + Val(txt_AddLess.Text) + Val(txt_CommAmt.Text), "#############0.00")

        'GST
        If chk_TaxAmount_RoundOff_STS.Checked = True Then
            lbl_CGST_Amount.Text = Format(Val(lbl_TaxableValue.Text) * Val(txt_CGST_Perc.Text) / 100, "##########0")
            lbl_SGST_Amount.Text = Format(Val(lbl_TaxableValue.Text) * Val(txt_SGST_Perc.Text) / 100, "##########0")
            lbl_IGST_Amount.Text = Format(Val(lbl_TaxableValue.Text) * Val(txt_IGST_Perc.Text) / 100, "##########0")
        Else
            lbl_CGST_Amount.Text = Format(Val(lbl_TaxableValue.Text) * Val(txt_CGST_Perc.Text) / 100, "##########0.00")
            lbl_SGST_Amount.Text = Format(Val(lbl_TaxableValue.Text) * Val(txt_SGST_Perc.Text) / 100, "##########0.00")
            lbl_IGST_Amount.Text = Format(Val(lbl_TaxableValue.Text) * Val(txt_IGST_Perc.Text) / 100, "##########0.00")
        End If

        Dim vBillAmt As String = 0

        vBillAmt = Val(lbl_TaxableValue.Text) + Val(lbl_CGST_Amount.Text) + Val(lbl_SGST_Amount.Text) + Val(lbl_IGST_Amount.Text)

        lbl_BillAmount.Text = Format(Val(vBillAmt), "#########0")
        lbl_BillAmount.Text = Format(Val(lbl_BillAmount.Text), "#########0.00")

        lbl_RoundOff.Text = Format(Val(lbl_BillAmount.Text) - Val(vBillAmt), "#########0.00")



        NtAmt = Val(lbl_BillAmount.Text) - Val(lbl_Tds_Amount.Text)

        lbl_NetAmount.Text = Format(Val(NtAmt), "#########0")

        lbl_NetAmount.Text = Common_Procedures.Currency_Format(Val(CSng(lbl_NetAmount.Text)))

    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String
        Dim ps As Printing.PaperSize

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_BillNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.PrintEntry, Common_Procedures.UR.Jobwork_Conversion_Bill_Entry, New_Entry) = False Then Exit Sub

        Try

            da1 = New SqlClient.SqlDataAdapter("select * from JobWork_ConversionBill_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and JobWork_ConversionBill_Code = '" & Trim(NewCode) & "'", con)
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

        prn_InpOpts = ""
        prn_InpOpts = InputBox("Select    -   0. None" & Chr(13) & "                  1. Original" & Chr(13) & "                  2. Duplicate" & Chr(13) & "                  3. Triplicate" & Chr(13) & "                  4. Extra Copy" & Space(10) & "                  5. All", "FOR INVOICE PRINTING...", "123")
        prn_InpOpts = Replace(Trim(prn_InpOpts), "5", "1234")


        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                Exit For
            End If
        Next

        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then

            Try

                If Print_PDF_Status = True Then
                    '--This is actual & correct 
                    PrintDocument1.DocumentName = "Invoice"
                    PrintDocument1.PrinterSettings.PrinterName = "doPDF v7"
                    PrintDocument1.PrinterSettings.PrintFileName = "c:\Invoice.pdf"
                    PrintDocument1.Print()

                Else
                    If Common_Procedures.settings.Printing_Show_PrintDialogue = 1 Then
                        PrintDialog1.PrinterSettings = PrintDocument1.PrinterSettings
                        If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                            PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings
                            PrintDocument1.Print()
                        End If

                    Else
                        PrintDocument1.Print()

                    End If

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
        Print_PDF_Status = False
    End Sub

    Private Sub PrintDocument1_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument1.BeginPrint
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim NewCode As String
        Dim W1 As Single = 0

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_BillNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0
        prn_TotMtrs = 0
        prn_Count = 0
        Prn_Cnt_Temp = 0
        Prn_Cnt_TEMP1 = 0
        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.*,f.*,d.Ledger_Name as TransportName,e.*,fh.*,SH.* ,Lsh.State_Name as Ledger_State_Name ,Lsh.State_code as Ledger_State_Code  , f.Ledger_mainName as DeliveryTo_LedgerName, f.Ledger_Address1 as DeliveryTo_LedgerAddress1, f.Ledger_Address2 as DeliveryTo_LedgerAddress2, f.Ledger_Address3 as DeliveryTo_LedgerAddress3, f.Ledger_Address4 as DeliveryTo_LedgerAddress4, f.Ledger_GSTinNo as DeliveryTo_LedgerGSTinNo, f.Ledger_pHONENo as DeliveryTo_LedgerPhoneNo, f.Pan_No as DeliveryTo_PanNo,f.Ledger_State_IdNo , Dsh.State_Name as DeliveryTo_State_Name, Dsh.State_Code as DeliveryTo_State_Code from JobWork_ConversionBill_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo LEFT OUTER JOIN State_Head SH ON b.Company_State_IdNo =SH.State_IdNo  INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN State_Head Lsh ON c.Ledger_State_IdNo = Lsh.State_IdNo LEFT OUTER JOIN Ledger_Head d ON d.Ledger_IdNo =a.Transport_IdNo  LEFT OUTER JOIN Ledger_Head f ON a.Delivery_IdNo = f.ledger_IdNo Left outer join state_head dsh on f.ledger_state_idno=dsh.state_idno  LEFT OUTER JOIN JobWork_Piece_Delivery_Head e ON A.JobWork_ConversionBill_Code = e.JobWork_Bill_Code LEFT OUTER JOIN Cloth_Head fh ON e.Cloth_IdNo = fh.Cloth_IdNO where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.JobWork_ConversionBill_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("Select a.*, b.cloth_name, b.weave,b.Cloth_Description ,IG.* from JobWork_ConversionBill_Details a, cloth_head b  ,ItemGroup_Head IG Where a.JobWork_ConversionBill_Code = '" & Trim(NewCode) & "' and a.cloth_idno = b.cloth_idno AND B.ItemGroup_IdNo = IG.ItemGroup_IdNo Order by a.JobWork_ConversionBill_Date, a.For_OrderBy, a.JobWork_ConversionBill_RefNo", con)
                'da2 = New SqlClient.SqlDataAdapter("select a.*, b.cloth_name, b.Cloth_Description from JobWork_Piece_Delivery_Head a INNER JOIN Cloth_Head b ON a.Cloth_IdNo = b.cloth_IdNo where a.JobWork_Bill_Code = '" & Trim(NewCode) & "' Order by a.for_orderby, a.JobWork_Piece_Delivery_No", con)
                prn_DetDt = New DataTable
                da2.Fill(prn_DetDt)

            Else
                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If
            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub PrintDocument1_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        If prn_HdDt.Rows.Count <= 0 Then Exit Sub

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1224" Then
            Printing_Format3(e)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1186" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1420" Or Common_Procedures.settings.CustomerCode = "1423" Or Common_Procedures.settings.CustomerCode = "1547" Or Common_Procedures.settings.CustomerCode = "1608" Then
            Printing_Format_1186(e)
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
        Dim SNo As Integer = 0

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                e.PageSettings.PaperSize = ps
                Exit For
            End If
        Next

        With PrintDocument1.DefaultPageSettings.Margins
            If Trim(Common_Procedures.settings.CustomerCode) = "1139" Then
                .Left = 50 ' 40
                .Right = 45
                .Top = 20  ' 40
                .Bottom = 55
            Else
                .Left = 20 ' 40
                .Right = 55
                .Top = 20  ' 40
                .Bottom = 50
            End If
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

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1021" Then
            NoofItems_PerPage = 6
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1033" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1238" Then '---- Rajeswari Weaving (Karumanthapatti) - Somanur Sizing
            NoofItems_PerPage = 11
        ElseIf Trim(Common_Procedures.settings.CustomerCode) = "1139" Then
            NoofItems_PerPage = 14
        Else
            NoofItems_PerPage = 12  '6  '8 ' 6
        End If

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1238" Then
            ClArr(1) = Val(40) : ClArr(2) = 0 : ClArr(3) = 300 : ClArr(4) = 70 : ClArr(5) = 70 : ClArr(6) = 90 : ClArr(7) = 80
            ClArr(8) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7))
            TxtHgt = 18 ' 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20
        Else
            ClArr(1) = Val(50) : ClArr(2) = 70 : ClArr(3) = 210 : ClArr(4) = 80 : ClArr(5) = 70 : ClArr(6) = 90 : ClArr(7) = 80
            ClArr(8) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7))
            TxtHgt = 18.25 ' 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20
        End If


        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_BillNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)


        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format1_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                ' W1 = e.Graphics.MeasureString("No.of Beams  : ", pFont).Width

                NoofDets = 0
                'prn_TotMtrs = 0

                CurY = CurY - 10

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If NoofDets >= (NoofItems_PerPage - 1) Then
                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

                            e.HasMorePages = True
                            Return

                        End If


                        ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("cloth_Name").ToString)
                        ItmNm2 = ""
                        If Len(ItmNm1) > 22 Then
                            For I = 22 To 1 Step -1
                                If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 22
                            ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                            ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                        End If

                        'CurY = CurY + TxtHgt

                        'prn_DetSNo = prn_DetSNo + 1
                        'Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetSNo)), LMargin + 10, CurY, 0, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Delivery_No").ToString, LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)

                        'Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Type1_Meters").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 10, CurY, 1, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_DetIndx).Item("Rate_ClothType1").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)

                        'Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Type1_Meters").ToString) * Val(prn_HdDt.Rows(prn_DetIndx).Item("Rate_ClothType1").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

                        'NoofDets = NoofDets + 1

                        'If Trim(ItmNm2) <> "" Then
                        '    CurY = CurY + TxtHgt - 5
                        '    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                        '    NoofDets = NoofDets + 1
                        'End If

                        'prn_TotMtrs = prn_TotMtrs + Val(prn_DetDt.Rows(prn_DetIndx).Item("Type1_Meters").ToString)

                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Type1_Meters").ToString) > 0 And Val(prn_DetDt.Rows(prn_DetIndx).Item("Sound_rate").ToString) <> 0 Then
                            CurY = CurY + TxtHgt

                            prn_DetSNo = prn_DetSNo + 1
                            Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetSNo)), LMargin + 10, CurY, 0, 0, pFont)
                            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1238" Then
                                If Val(ClArr(2)) <> 0 Then
                                    Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Delivery_No").ToString, LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                                    'p1Font = New Font("Calibri", 9, FontStyle.Regular)
                                    'Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Delivery_No").ToString & "/" & prn_DetDt.Rows(prn_DetIndx).Item("Delivery_Date").ToString, LMargin + ClArr(1) + 10, CurY, 0, 0, p1Font)
                                End If
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1) & " " & Common_Procedures.ClothType.Type1, LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                            Else
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Delivery_No").ToString, LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                            End If

                            Common_Procedures.Print_To_PrintDocument(e, "998821", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 5, CurY, 0, 0, pFont)
                            'Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Item_HSN_Code").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 5, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Item_GST_Percentage").ToString & IIf(prn_DetDt.Rows(prn_DetIndx).Item("Item_GST_Percentage").ToString <> "", " %", ""), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)

                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Type1_Meters").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("SOund_rate").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
                            ' Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Type1_Meters").ToString) * Val(prn_HdDt.Rows(0).Item("SOunds_rate").ToString), "#########0.00")
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("SOund_Amount").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            If Trim(ItmNm2) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If

                            prn_TotMtrs = prn_TotMtrs + Val(prn_DetDt.Rows(prn_DetIndx).Item("Type1_Meters").ToString)

                        End If


                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Type2_Meters").ToString) > 0 And Val(prn_DetDt.Rows(prn_DetIndx).Item("Seconds_rate").ToString) <> 0 Then
                            CurY = CurY + TxtHgt

                            prn_DetSNo = prn_DetSNo + 1
                            Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetSNo)), LMargin + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Delivery_No").ToString, LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                            'Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.ClothType.Type2, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)

                            Common_Procedures.Print_To_PrintDocument(e, "998821", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 5, CurY, 0, 0, pFont)
                            'Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Item_HSN_Code").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 5, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Item_GST_Percentage").ToString & IIf(prn_DetDt.Rows(prn_DetIndx).Item("Item_GST_Percentage").ToString <> "", " %", ""), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)


                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Type2_Meters").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Seconds_rate").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)

                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Seconds_Amount").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            If Trim(ItmNm2) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If

                            prn_TotMtrs = prn_TotMtrs + Val(prn_DetDt.Rows(prn_DetIndx).Item("Type2_Meters").ToString)

                        End If

                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Type3_Meters").ToString) > 0 And Val(prn_DetDt.Rows(prn_DetIndx).Item("bits_rate").ToString) <> 0 Then
                            CurY = CurY + TxtHgt

                            prn_DetSNo = prn_DetSNo + 1
                            Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetSNo)), LMargin + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Delivery_No").ToString, LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                            'Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.ClothType.Type3, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)

                            Common_Procedures.Print_To_PrintDocument(e, "998821", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 5, CurY, 0, 0, pFont)
                            'Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Item_HSN_Code").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 5, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Item_GST_Percentage").ToString & IIf(prn_DetDt.Rows(prn_DetIndx).Item("Item_GST_Percentage").ToString <> "", " %", ""), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)

                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Type3_Meters").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("bits_rate").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)

                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("bits_amount").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            If Trim(ItmNm2) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If

                            prn_TotMtrs = prn_TotMtrs + Val(prn_DetDt.Rows(prn_DetIndx).Item("Type3_Meters").ToString)

                        End If

                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Type4_Meters").ToString) > 0 And Val(prn_DetDt.Rows(prn_DetIndx).Item("Reject_rate").ToString) <> 0 Then
                            CurY = CurY + TxtHgt

                            prn_DetSNo = prn_DetSNo + 1
                            Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetSNo)), LMargin + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Delivery_No").ToString, LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                            '  Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.ClothType.Type4, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)

                            Common_Procedures.Print_To_PrintDocument(e, "998821", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 5, CurY, 0, 0, pFont)
                            'Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Item_HSN_Code").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 5, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Item_GST_Percentage").ToString & IIf(prn_DetDt.Rows(prn_DetIndx).Item("Item_GST_Percentage").ToString <> "", " %", ""), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)


                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Type4_Meters").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Reject_rate").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)

                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Reject_Amount").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            If Trim(ItmNm2) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If

                            prn_TotMtrs = prn_TotMtrs + Val(prn_DetDt.Rows(prn_DetIndx).Item("Type4_Meters").ToString)

                        End If

                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Type5_Meters").ToString) > 0 And Val(prn_DetDt.Rows(prn_DetIndx).Item("Others_rate").ToString) <> 0 Then
                            CurY = CurY + TxtHgt

                            prn_DetSNo = prn_DetSNo + 1
                            Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetSNo)), LMargin + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Delivery_No").ToString, LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                            ' Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.ClothType.Type5, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)


                            Common_Procedures.Print_To_PrintDocument(e, "998821", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 5, CurY, 0, 0, pFont)
                            'Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Item_HSN_Code").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 5, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Item_GST_Percentage").ToString & IIf(prn_DetDt.Rows(prn_DetIndx).Item("Item_GST_Percentage").ToString <> "", " %", ""), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)


                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Type5_Meters").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Others_rate").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)

                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Others_AMount").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            If Trim(ItmNm2) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If

                            prn_TotMtrs = prn_TotMtrs + Val(prn_DetDt.Rows(prn_DetIndx).Item("Type5_Meters").ToString)

                        End If




                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If


                Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)

                If Trim(prn_InpOpts) <> "" Then
                    If Prn_Cnt_TEMP1 < Len(Trim(prn_InpOpts)) Then


                        If Val(prn_InpOpts) <> "0" Then
                            prn_DetIndx = 0
                            prn_DetSNo = 0

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
        Dim C1 As Single, W1 As Single, S1 As Single
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String, Cmp_EMail As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_PanNo As String
        Dim Cmp_State As String, Cmp_StateCode As String, Cmp_GSTIN_No As String
        Dim I As Integer = 0
        Dim FrmJJ1 As String = ""
        Dim FrmJJ2 As String = ""
        Dim CurY1 As Single = 0
        Dim S As String = ""
        Dim vDcNoDt As String = ""
        Dim vDupDcNoDt As String = ""

        Dim ItmNm1 As String = ""
        Dim ItmNm2 As String = ""

        PageNo = PageNo + 1

        prn_Count = prn_Count + 1

        prn_OriDupTri = ""
        If Trim(prn_InpOpts) <> "" Then
            If Prn_Cnt_TEMP1 <= Len(Trim(prn_InpOpts)) Then

                ' S = Mid$(Trim(prn_InpOpts), prn_Count, 1)

                ' If PageNo <= 1 Then
                Prn_Cnt_Temp = Prn_Cnt_Temp + 1
                S = Mid$(Trim(prn_InpOpts), Prn_Cnt_Temp, 1)
                Prn_Cnt_TEMP1 = Prn_Cnt_TEMP1 + 1
                'End If

                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1018" Then '---- M.K Textiles (Palladam)
                    If Val(S) = 1 Then
                        prn_OriDupTri = "ORIGINAL"
                    ElseIf Val(S) = 2 Then
                        prn_OriDupTri = "TRANSPORT COPY"
                    ElseIf Val(S) = 3 Then
                        prn_OriDupTri = "TRIPLICATE"
                    ElseIf Val(S) = 4 Then
                        prn_OriDupTri = "EXTRA COPY"
                    Else
                        If Trim(prn_InpOpts) <> "0" And Val(prn_InpOpts) = "0" And Len(Trim(prn_InpOpts)) > 3 Then
                            prn_OriDupTri = Trim(prn_InpOpts)
                        End If
                    End If

                Else
                    If Val(S) = 1 Then
                        prn_OriDupTri = "ORIGINAL"
                    ElseIf Val(S) = 2 Then
                        prn_OriDupTri = "DUPLICATE"
                    ElseIf Val(S) = 3 Then
                        prn_OriDupTri = "TRIPLICATE"
                    ElseIf Val(S) = 4 Then
                        prn_OriDupTri = "EXTRA COPY"
                    Else
                        If Trim(prn_InpOpts) <> "0" And Val(prn_InpOpts) = "0" And Len(Trim(prn_InpOpts)) > 3 Then
                            prn_OriDupTri = Trim(prn_InpOpts)
                        End If
                    End If

                End If

            End If
        End If

        Cmp_Name = ""
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1021" Then

            CurY = TMargin + 185

            If Trim(prn_OriDupTri) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
            End If

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(1) = CurY

            CurY = CurY + 10
            p1Font = New Font("Calibri", 16, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "INVOICE", LMargin, CurY, 2, PrintWidth, p1Font)
            strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

            CurY = CurY + strHeight + 25
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(2) = CurY

        Else

            CurY = TMargin

            da2 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name, c.Ledger_Name as Transport_Name from JobWork_ConversionBill_Head a  INNER JOIN Ledger_Head b ON b.Ledger_IdNo = a.Ledger_Idno LEFT OUTER JOIN Ledger_Head c ON c.Ledger_IdNo = a.Transport_IdNo  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.JobWork_ConversionBill_Code = '" & Trim(EntryCode) & "' Order by a.For_OrderBy", con)
            da2.Fill(dt2)
            If dt2.Rows.Count > NoofItems_PerPage Then
                Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
            End If
            dt2.Clear()

            If Trim(prn_OriDupTri) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
            End If
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(1) = CurY

            Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
            Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_PanNo = ""
            Cmp_State = "" : Cmp_StateCode = "" : Cmp_GSTIN_No = "" : Cmp_EMail = ""
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            If Trim(prn_HdDt.Rows(0).Item("Company_Factory_Address1").ToString) <> "" Or Trim(prn_HdDt.Rows(0).Item("Company_Factory_Address2").ToString) <> "" Or Trim(prn_HdDt.Rows(0).Item("Company_Factory_Address3").ToString) <> "" Or Trim(prn_HdDt.Rows(0).Item("Company_Factory_Address4").ToString) <> "" Then
                Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString
                Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Factory_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Factory_Address2").ToString & " " & prn_HdDt.Rows(0).Item("Company_Factory_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Factory_Address4").ToString

            Else
                Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
                Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString
            End If

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
                Cmp_EMail = "EMAIL ID : " & prn_HdDt.Rows(0).Item("Company_EMail").ToString
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
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1238" Then
                p1Font = New Font("Cambria", 25, FontStyle.Bold)
            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1152" Then
                p1Font = New Font("Elephant", 22, FontStyle.Bold)
            Else
                p1Font = New Font("Calibri", 14, FontStyle.Bold)
            End If
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin + 10, CurY, 0, PrintWidth, p1Font)
            strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height
            'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1033" Then '---- Rajeswari Weaving (Karumanthapatti) - Somanur Sizing
            '    e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Companylogo_Rajeswari_Weaving_Mills, Drawing.Image), LMargin + 15, CurY, 190, 90)
            'End If

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1186" Then '---- UNITED WEAVES (PALLADAM)
              e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.united_weaves_logo_png, Drawing.Image), PageWidth - 150, CurY, 120, 100)
            End If


            If Trim(prn_HdDt.Rows(0).Item("E_Invoice_IRNO").ToString) <> "" Then

                If IsDBNull(prn_HdDt.Rows(0).Item("E_Invoice_QR_Image")) = False Then
                    Dim imageData As Byte() = DirectCast(prn_HdDt.Rows(0).Item("E_Invoice_QR_Image"), Byte())
                    If Not imageData Is Nothing Then
                        Using ms As New MemoryStream(imageData, 0, imageData.Length)
                            ms.Write(imageData, 0, imageData.Length)

                            If imageData.Length > 0 Then

                                pic_IRN_QRCode_Image_forPrinting.BackgroundImage = Image.FromStream(ms)

                                e.Graphics.DrawImage(DirectCast(pic_IRN_QRCode_Image_forPrinting.BackgroundImage, Drawing.Image), PageWidth - 75, CurY - 5, 65, 65)

                            End If

                        End Using

                    End If

                End If

            End If

            CurY = CurY + strHeight - 1
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin + 10, CurY, 0, PrintWidth, pFont)

            CurY = CurY + TxtHgt - 1
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin + 10, CurY, 0, PrintWidth, pFont)
            ' CurY = CurY + TxtHgt - 1
            ' Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)

            If Cmp_State <> "" Then
                CurY = CurY + TxtHgt - 1
                Common_Procedures.Print_To_PrintDocument(e, Cmp_State & "  " & Cmp_StateCode, LMargin + 10, CurY, 0, PrintWidth, pFont)
            End If
            If Trim(Common_Procedures.settings.CustomerCode) = "1274" Then
                If Cmp_GSTIN_No <> "" Then
                    pFont = New Font("CALIBRI", 10, FontStyle.Bold)
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, LMargin + 10, CurY + 15, 0, PrintWidth, pFont)
                End If
            Else
                If Cmp_GSTIN_No <> "" Then
                    CurY = CurY + TxtHgt - 1
                    Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, LMargin + 10, CurY, 0, PrintWidth, pFont)
                End If
            End If

            CurY = CurY + TxtHgt + 10
            ' p1Font = New Font("Calibri", 16, FontStyle.Bold)
            ' Common_Procedures.Print_To_PrintDocument(e, "JOBWORK INVOICE", LMargin, CurY, 2, PrintWidth, p1Font)
            strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

            pFont = New Font("CALIBRI", 10, FontStyle.Regular)

            If Cmp_PhNo <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo & " / " & Cmp_EMail, LMargin + 10, CurY, 0, 0, pFont)
            End If
            'If Cmp_EMail <> "" Then
            '    Common_Procedures.Print_To_PrintDocument(e, Cmp_EMail, PageWidth - 270, CurY, 0, 0, pFont)
            'End If


            '----------






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
                p1Font = New Font("Calibri", 10, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "IRN : " & Trim(ItmNm1), LMargin + 10, CurY, 0, 0, p1Font)
                Common_Procedures.Print_To_PrintDocument(e, "Ack. No : " & prn_HdDt.Rows(0).Item("E_Invoice_ACK_No").ToString, PrintWidth - 270, CurY, 0, 0, p1Font)

                If Trim(ItmNm2) <> "" Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, "             " & Trim(ItmNm2), LMargin, CurY, 0, 0, p1Font)
                    Common_Procedures.Print_To_PrintDocument(e, "Ack. Date : " & prn_HdDt.Rows(0).Item("E_Invoice_ACK_Date").ToString, PrintWidth - 270, CurY, 0, 0, p1Font)
                End If


            End If

            CurY = CurY + TxtHgt + 10
            p1Font = New Font("Calibri", 16, FontStyle.Bold)
            If Trim(Common_Procedures.settings.CustomerCode) = "1274" Then
                Common_Procedures.Print_To_PrintDocument(e, "CONVERSION BILL", LMargin, CurY, 2, PrintWidth, p1Font)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "JOBWORK INVOICE", LMargin, CurY, 2, PrintWidth, p1Font)
            End If


            CurY = CurY + strHeight
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(2) = CurY
        End If


        Try
            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5)
            W1 = e.Graphics.MeasureString("FORMJJ NO   : ", pFont).Width
            S1 = e.Graphics.MeasureString("TO     :    ", pFont).Width

            CurY1 = CurY
            'Left side


            CurY1 = CurY1 + TxtHgt
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "TO  :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_MainName").ToString, LMargin + 10, CurY1, 0, 0, p1Font)

            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY1, 0, 0, pFont)
            p1Font = New Font("Calibri", 14, FontStyle.Bold)

            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY1, 0, 0, pFont)

            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY1, 0, 0, pFont)

            If Trim(prn_HdDt.Rows(0).Item("Ledger_Address4").ToString) <> "" Then
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY1, 0, 0, pFont)
            End If

            CurY1 = CurY1 + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("Ledger_PhoneNo").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, " PHONE : " & prn_HdDt.Rows(0).Item("Ledger_PhoneNo").ToString, LMargin + S1 + 10, CurY1, 0, 0, pFont)
            End If
            CurY1 = CurY1 + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString & "  CODE : " & prn_HdDt.Rows(0).Item("Ledger_State_Code").ToString, LMargin + S1 + 10, CurY1, 0, 0, pFont)
            End If

            pFont = New Font("CALIBRI", 10, FontStyle.Bold)
            CurY1 = CurY1 + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, " GSTIN : " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + S1 + 10, CurY1, 0, 0, pFont)
            End If


            pFont = New Font("CALIBRI", 10, FontStyle.Regular)
            'Right Side
            CurY = CurY + TxtHgt

            p1Font = New Font("Elephant", 9, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "BILL NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            'If Common_Procedures.settings.CustomerCode = "1139" Then
            '    Common_Procedures.Print_To_PrintDocument(e, "J-" & prn_HdDt.Rows(0).Item("JobWork_ConversionBill_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)
            'Else
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("JobWork_ConversionBill_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)
            ' End If

            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("JobWork_ConversionBill_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)


            If prn_HdDt.Rows(0).Item("FormJJ_No").ToString <> "" Then

                CurY = CurY + TxtHgt
                FrmJJ1 = Trim(prn_HdDt.Rows(prn_DetIndx).Item("FormJJ_No").ToString)
                FrmJJ2 = ""
                If Len(FrmJJ1) > 17 Then
                    For I = 17 To 1 Step -1
                        If Mid$(Trim(FrmJJ1), I, 1) = " " Or Mid$(Trim(FrmJJ1), I, 1) = "," Or Mid$(Trim(FrmJJ1), I, 1) = "." Or Mid$(Trim(FrmJJ1), I, 1) = "-" Or Mid$(Trim(FrmJJ1), I, 1) = "/" Or Mid$(Trim(FrmJJ1), I, 1) = "_" Or Mid$(Trim(FrmJJ1), I, 1) = "(" Or Mid$(Trim(FrmJJ1), I, 1) = ")" Or Mid$(Trim(FrmJJ1), I, 1) = "\" Or Mid$(Trim(FrmJJ1), I, 1) = "[" Or Mid$(Trim(FrmJJ1), I, 1) = "]" Or Mid$(Trim(FrmJJ1), I, 1) = "{" Or Mid$(Trim(FrmJJ1), I, 1) = "}" Then Exit For
                    Next I
                    If I = 0 Then I = 17
                    FrmJJ2 = Microsoft.VisualBasic.Right(Trim(FrmJJ1), Len(FrmJJ1) - I)
                    FrmJJ1 = Microsoft.VisualBasic.Left(Trim(FrmJJ1), I - 1)
                End If

                Common_Procedures.Print_To_PrintDocument(e, "FORMJJ NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, FrmJJ1, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

                If Trim(FrmJJ2) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, FrmJJ2, LMargin + C1 + W1 + 30, CurY + TxtHgt - 3, 0, 0, pFont)
                End If

            End If


            '---------


            If CurY1 > CurY Then CurY = CurY1

            CurY = CurY + TxtHgt + 6
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + C1, LnAr(3), LMargin + C1, LnAr(2))

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1238" Then

                da2 = New SqlClient.SqlDataAdapter("Select a.* from JobWork_ConversionBill_Details a Where a.JobWork_ConversionBill_Code = '" & Trim(EntryCode) & "' Order by a.sl_No", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_Details.Rows.Clear()

                vDcNoDt = ""
                vDupDcNoDt = ""
                If dt2.Rows.Count > 0 Then

                    For I = 0 To dt2.Rows.Count - 1

                        If InStr(1, Trim(UCase(vDupDcNoDt)), "~" & Trim(UCase(dt2.Rows(I).Item("Delivery_No").ToString)) & "~") = 0 Then
                            vDcNoDt = vDcNoDt & IIf(Trim(vDcNoDt) <> "", ",  ", "") & Trim(dt2.Rows(I).Item("Delivery_No").ToString) & "/" & dt2.Rows(I).Item("Delivery_Date").ToString
                            vDupDcNoDt = vDupDcNoDt & "~" & Trim(dt2.Rows(I).Item("Delivery_No").ToString) & "~"
                        End If

                    Next I

                End If

                CurY = CurY + TxtHgt - 10
                p1Font = New Font("Calibri", 9, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, "Dc No & Date : " & Trim(vDcNoDt), LMargin + 10, CurY, 0, 0, p1Font)
                CurY = CurY + 10

            End If

            CurY = CurY + TxtHgt - 10
            'Common_Procedures.Print_To_PrintDocument(e, "Transport Name : " & Trim(prn_HdDt.Rows(0).Item("TransportName").ToString), LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Despatch To : " & Trim(prn_HdDt.Rows(0).Item("Despatch_To").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 0, 0, pFont)

            'CurY = CurY + TxtHgt + 6
            Common_Procedures.Print_To_PrintDocument(e, "Vehicle No   : " & Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString), LMargin + 10, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt + 6
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY

            pFont = New Font("Calibri", 10, FontStyle.Bold)
            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)
            If Val(ClAr(2)) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "DC.NO", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            End If
            Common_Procedures.Print_To_PrintDocument(e, "DESCRIPTION OF GOODS", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "HSN/SAC", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "GST %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "METER", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "RATE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)

            CurY = CurY + TxtHgt + 5
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
        Dim W1 As Single = 0
        Dim BmsInWrds As String
        Dim vprn_BlNos As String = ""
        Dim BankNm1 As String = ""
        Dim BankNm2 As String = ""
        Dim BankNm3 As String = ""
        Dim BankNm4 As String = ""
        Dim BInc As Integer
        Dim BnkDetAr() As String
        Dim CurY1 As Single = 0

        ' W1 = e.Graphics.MeasureString("No.of Beams  : ", pFont).Width

        Try

            For I = NoofDets + 1 To NoofItems_PerPage - 1

                CurY = CurY + TxtHgt

                prn_DetIndx = prn_DetIndx + 1

            Next

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

            pFont = New Font("Calibri", 10, FontStyle.Regular)
            CurY = CurY + TxtHgt - 10
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(Format(Val(prn_TotMtrs), "########0.00")), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Gross_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                prn_TotMtrs = 0
            End If

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
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))

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

            CurY1 = CurY
            If is_LastPage = True Then
                'Left Side
                CurY1 = CurY1 + TxtHgt
                p1Font = New Font("Calibri", 11, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, BankNm1, LMargin + 10, CurY1, 0, 0, p1Font)
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, BankNm2, LMargin + 10, CurY1, 0, 0, p1Font)
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, BankNm3, LMargin + 10, CurY1, 0, 0, p1Font)
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, BankNm4, LMargin + 10, CurY1, 0, 0, p1Font)

            End If


            'Right Side
            CurY = CurY + TxtHgt - 10
            If is_LastPage = True Then
                If Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Freight_Caption_Name").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Freight_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
                End If
            End If

            CurY = CurY + TxtHgt
            If is_LastPage = True Then
                If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then
                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1238" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1239" Then '---- KALAIMAGAL TEX
                        Common_Procedures.Print_To_PrintDocument(e, "Trade Discount", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                    Else
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("AddLess_Caption_Name").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                    End If
                    Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
                End If

                If Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then
                    CurY = CurY + TxtHgt + 5
                    e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, PageWidth, CurY)
                End If
            End If


            CurY = CurY - 10

            CurY = CurY + TxtHgt
            If is_LastPage = True Then
                If Val(prn_HdDt.Rows(0).Item("Total_Taxable_Amount").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, "Taxable Value", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("Total_Taxable_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
                End If
            End If
            CurY = CurY + TxtHgt
            If is_LastPage = True Then
                If Val(prn_HdDt.Rows(0).Item("CGST_Amount").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, "CGST " & Val(prn_HdDt.Rows(0).Item("CGST_Percentage").ToString) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("CGST_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
                End If
            End If
            CurY = CurY + TxtHgt
            If is_LastPage = True Then
                If Val(prn_HdDt.Rows(0).Item("SGST_Amount").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, "SGST " & Val(prn_HdDt.Rows(0).Item("SGST_Percentage").ToString) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("SGST_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
                End If
            End If
            CurY = CurY + TxtHgt
            If is_LastPage = True Then
                If Val(prn_HdDt.Rows(0).Item("IGST_Amount").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, "IGST " & Val(prn_HdDt.Rows(0).Item("IGST_Percentage").ToString) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("IGST_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
                End If
            End If





            CurY = CurY + TxtHgt
            If is_LastPage = True Then
                p1Font = New Font("Calibri", 13, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "(Textile manufactring service (Weaving) )", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 20, CurY, 1, 0, p1Font)
            End If

            If is_LastPage = True Then
                If Val(prn_HdDt.Rows(0).Item("RoundOff_Amount").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, "RoundOff", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("RoundOff_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
                End If

            End If


            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, PageWidth, CurY)
            LnAr(8) = CurY
            CurY = CurY + TxtHgt - 10
            p1Font = New Font("Calibri", 13, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "E & OE", LMargin + 10, CurY, 0, 0, p1Font)

            If is_LastPage = True Then
                pFont = New Font("Calibri", 12, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "NET AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                p1Font = New Font("Calibri", 12, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, " " & Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, p1Font)
            End If

            pFont = New Font("Calibri", 10, FontStyle.Regular)
            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(7))
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(9) = CurY
            ' e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))

            CurY = CurY + TxtHgt - 5

            If is_LastPage = True Then
                BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))

                p1Font = New Font("Calibri", 12, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "Rupees  : " & BmsInWrds & " ", LMargin + 10, CurY, 0, 0, p1Font)
            End If

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            CurY = CurY + 5
            p1Font = New Font("Calibri", 12, FontStyle.Underline)
            Common_Procedures.Print_To_PrintDocument(e, "Term & Conditions : ", LMargin + 10, CurY, 0, 0, p1Font)



            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1162" Then
                CurY = CurY + TxtHgt + 6
                Common_Procedures.Print_To_PrintDocument(e, "Goods Once Sold Cannot Be Taken Back ", LMargin + 10, CurY, 0, 0, pFont)
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Overdue Interest will be charged at 24% from The invoice date ", LMargin + 10, CurY, 0, 0, pFont)
            Else
                CurY = CurY + TxtHgt + 10
                Common_Procedures.Print_To_PrintDocument(e, "Overdue Interest will be charged at 24% from The invoice date ", LMargin + 10, CurY, 0, 0, pFont)
            End If


            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "We are not responsible for any loss or damage in transit", LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "We will not accept any claim after processing of goods", LMargin + 10, CurY, 0, 0, pFont)

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1162" Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Any Claim or Damage Can Be Accepted If Intimated Within 7 Working days From Date Of Delivery", LMargin + 10, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            If Trim(Common_Procedures.settings.CustomerCode) = "1162" Then
                Common_Procedures.Print_To_PrintDocument(e, "Subject to " & Trim(Common_Procedures.settings.Jurisdiction) & " jurisdiction ", LMargin + 10, CurY, 0, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "Subject to Tirupur jurisdiction ", LMargin + 10, CurY, 0, 0, pFont)
            End If




            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(10) = CurY


            'If Val(Common_Procedures.User.IdNo) <> 1 Then
            '    Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
            'End If

            CurY = CurY + TxtHgt - 10
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1152" Then
                p1Font = New Font("Elephant", 12, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)
            Else
                p1Font = New Font("Calibri", 12, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)
            End If

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1162" Then
                CurY = CurY + TxtHgt
                e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.vijay_tex_Sign2, Drawing.Image), PageWidth - 110, CurY, 90, 55)
            End If

            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            '        CurY = CurY + TxtHgt

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


    Private Sub Printing_Format_1186(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
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
        Dim ItmNm1 As String, ItmNm2 As String, ItmNm3 As String
        Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim W1 As Single = 0
        Dim SNo As Integer = 0

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                e.PageSettings.PaperSize = ps
                Exit For
            End If
        Next

        With PrintDocument1.DefaultPageSettings.Margins
            If Trim(Common_Procedures.settings.CustomerCode) = "1139" Then
                .Left = 50 ' 40
                .Right = 45
                .Top = 20  ' 40
                .Bottom = 55
            Else
                .Left = 20 ' 40
                .Right = 55
                .Top = 20  ' 40
                .Bottom = 50
            End If
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pFont = New Font("Calibri", 9, FontStyle.Regular)

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

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1021" Then
            NoofItems_PerPage = 6
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1033" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1238" Then '---- Rajeswari Weaving (Karumanthapatti) - Somanur Sizing
            NoofItems_PerPage = 11
        ElseIf Trim(Common_Procedures.settings.CustomerCode) = "1139" Then
            NoofItems_PerPage = 14
        ElseIf Trim(Common_Procedures.settings.CustomerCode) = "1420" Or Common_Procedures.settings.CustomerCode = "1423" Then 'CYBER TEXTILES (INDIA) PRIVATE LIMITED
            NoofItems_PerPage = 7
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1608" Then '---- SAMANTH TEXTILES (SOMANUR)
            NoofItems_PerPage = 5
        Else
            NoofItems_PerPage =6 ' 10  '6  '8 ' 6
        End If

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1238" Then
            ClArr(1) = Val(40) : ClArr(2) = 0 : ClArr(3) = 300 : ClArr(4) = 70 : ClArr(5) = 70 : ClArr(6) = 70 : ClArr(7) = 90 : ClArr(8) = 80
            ClArr(9) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8))
            TxtHgt = 18 ' 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        Else

            If prn_DetDt.Rows(prn_DetIndx).Item("po_no").ToString <> "" Then
                ClArr(1) = Val(35) : ClArr(2) = 55 : ClArr(3) = 200 : ClArr(4) = 90 : ClArr(5) = 60 : ClArr(6) = 60 : ClArr(7) = 85 : ClArr(8) = 70
                ClArr(9) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8))
            Else
                ClArr(1) = Val(35) : ClArr(2) = 55 : ClArr(3) = 200 : ClArr(4) = 90 : ClArr(5) = 60 : ClArr(6) = 60 : ClArr(7) = 85 : ClArr(8) = 70
                ClArr(9) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8))
            End If

            If Trim(Common_Procedures.settings.CustomerCode) = "1420" Or Common_Procedures.settings.CustomerCode = "1423" Then
                TxtHgt = 17.5  '18.25 ' 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20
            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1608" Then '---- SAMANTH TEXTILES (SOMANUR)
                TxtHgt = 18.1
            Else
                TxtHgt = 18.25 ' 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20
            End If

        End If



        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_BillNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)


        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format_1186_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                ' W1 = e.Graphics.MeasureString("No.of Beams  : ", pFont).Width

                NoofDets = 0
                'prn_TotMtrs = 0

                CurY = CurY - 10

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If NoofDets >= (NoofItems_PerPage - 1) Then
                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_Format_1186_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

                            e.HasMorePages = True
                            Return

                        End If


                        ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("cloth_Name").ToString) & Trim(prn_DetDt.Rows(prn_DetIndx).Item("wEAVE").ToString)
                        ItmNm2 = ""
                        ItmNm3 = ""
                        If Len(ItmNm1) > 30 Then
                            For I = 30 To 1 Step -1
                                If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 30
                            ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                            ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                        End If
                        If Len(ItmNm2) > 30 Then
                            For I = 30 To 1 Step -1
                                If Mid$(Trim(ItmNm2), I, 1) = " " Or Mid$(Trim(ItmNm2), I, 1) = "," Or Mid$(Trim(ItmNm2), I, 1) = "." Or Mid$(Trim(ItmNm2), I, 1) = "-" Or Mid$(Trim(ItmNm2), I, 1) = "/" Or Mid$(Trim(ItmNm2), I, 1) = "_" Or Mid$(Trim(ItmNm2), I, 1) = "(" Or Mid$(Trim(ItmNm2), I, 1) = ")" Or Mid$(Trim(ItmNm2), I, 1) = "\" Or Mid$(Trim(ItmNm2), I, 1) = "[" Or Mid$(Trim(ItmNm2), I, 1) = "]" Or Mid$(Trim(ItmNm2), I, 1) = "{" Or Mid$(Trim(ItmNm2), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 30
                            ItmNm3 = Microsoft.VisualBasic.Right(Trim(ItmNm2), Len(ItmNm2) - I)
                            ItmNm2 = Microsoft.VisualBasic.Left(Trim(ItmNm2), I - 1)
                        End If


                        'CurY = CurY + TxtHgt

                        'prn_DetSNo = prn_DetSNo + 1
                        'Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetSNo)), LMargin + 10, CurY, 0, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Delivery_No").ToString, LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)

                        'Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Type1_Meters").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 10, CurY, 1, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_DetIndx).Item("Rate_ClothType1").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)

                        'Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Type1_Meters").ToString) * Val(prn_HdDt.Rows(prn_DetIndx).Item("Rate_ClothType1").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

                        'NoofDets = NoofDets + 1

                        'If Trim(ItmNm2) <> "" Then
                        '    CurY = CurY + TxtHgt - 5
                        '    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                        '    NoofDets = NoofDets + 1
                        'End If

                        'prn_TotMtrs = prn_TotMtrs + Val(prn_DetDt.Rows(prn_DetIndx).Item("Type1_Meters").ToString)

                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Type1_Meters").ToString) > 0 And Val(prn_DetDt.Rows(prn_DetIndx).Item("Sound_rate").ToString) > 0 Then
                            CurY = CurY + TxtHgt

                            prn_DetSNo = prn_DetSNo + 1
                            Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetSNo)), LMargin + 10, CurY, 0, 0, pFont)
                            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1238" Then
                                If Val(ClArr(2)) <> 0 Then
                                    Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Delivery_No").ToString, LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                                    'p1Font = New Font("Calibri", 9, FontStyle.Regular)
                                    'Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Delivery_No").ToString & "/" & prn_DetDt.Rows(prn_DetIndx).Item("Delivery_Date").ToString, LMargin + ClArr(1) + 10, CurY, 0, 0, p1Font)
                                End If
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1) & " " & Common_Procedures.ClothType.Type1, LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                            Else
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Delivery_No").ToString, LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                            End If

                            If prn_DetDt.Rows(prn_DetIndx).Item("po_no").ToString <> "" Then
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("po_no").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 5, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("pcs").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, "998821", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + 2, CurY, 0, 0, pFont)

                            Else

                                Common_Procedures.Print_To_PrintDocument(e, "998821", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)
                                'Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Item_HSN_Code").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 5, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("pcs").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Item_GST_Percentage").ToString & IIf(prn_DetDt.Rows(prn_DetIndx).Item("Item_GST_Percentage").ToString <> "", " %", ""), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)

                            End If

                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Type1_Meters").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Sound_Rate").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)
                            ' Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Type1_Meters").ToString) * Val(prn_HdDt.Rows(0).Item("SOunds_rate").ToString), "#########0.00")
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Sound_Amount").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            If Trim(ItmNm2) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If

                            prn_TotMtrs = prn_TotMtrs + Val(prn_DetDt.Rows(prn_DetIndx).Item("Type1_Meters").ToString)
                            prn_pcs = prn_pcs + Val(prn_DetDt.Rows(prn_DetIndx).Item("pcs").ToString)

                        End If


                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Type2_Meters").ToString) > 0 And Val(prn_DetDt.Rows(prn_DetIndx).Item("Seconds_rate").ToString) > 0 Then
                            CurY = CurY + TxtHgt

                            prn_DetSNo = prn_DetSNo + 1
                            Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetSNo)), LMargin + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Delivery_No").ToString, LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                            'Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.ClothType.Type2, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)

                            If prn_DetDt.Rows(prn_DetIndx).Item("po_no").ToString <> "" Then
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("po_no").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 5, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, " ", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 2, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, "998821", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + 2, CurY, 0, 0, pFont)
                            Else

                                Common_Procedures.Print_To_PrintDocument(e, "998821", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)
                                'Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Item_HSN_Code").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 5, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, " ", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 2, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Item_GST_Percentage").ToString & IIf(prn_DetDt.Rows(prn_DetIndx).Item("Item_GST_Percentage").ToString <> "", " %", ""), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)

                            End If

                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Type2_Meters").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Seconds_rate").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)

                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Seconds_Amount").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            If Trim(ItmNm2) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If

                            prn_TotMtrs = prn_TotMtrs + Val(prn_DetDt.Rows(prn_DetIndx).Item("Type2_Meters").ToString)

                        End If

                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Type3_Meters").ToString) > 0 And Val(prn_DetDt.Rows(prn_DetIndx).Item("bits_rate").ToString) > 0 Then
                            CurY = CurY + TxtHgt

                            prn_DetSNo = prn_DetSNo + 1
                            Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetSNo)), LMargin + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Delivery_No").ToString, LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                            'Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.ClothType.Type3, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)

                            If prn_DetDt.Rows(prn_DetIndx).Item("po_no").ToString <> "" Then
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("po_no").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 5, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, " ", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 2, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, "998821", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + 2, CurY, 0, 0, pFont)
                            Else

                                Common_Procedures.Print_To_PrintDocument(e, "998821", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)
                                'Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Item_HSN_Code").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 5, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, " ", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 2, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Item_GST_Percentage").ToString & IIf(prn_DetDt.Rows(prn_DetIndx).Item("Item_GST_Percentage").ToString <> "", " %", ""), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)

                            End If
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Type3_Meters").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("bits_rate").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)

                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("bits_amount").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            If Trim(ItmNm2) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If

                            prn_TotMtrs = prn_TotMtrs + Val(prn_DetDt.Rows(prn_DetIndx).Item("Type3_Meters").ToString)

                        End If

                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Type4_Meters").ToString) > 0 And Val(prn_DetDt.Rows(prn_DetIndx).Item("Reject_rate").ToString) > 0 Then
                            CurY = CurY + TxtHgt

                            prn_DetSNo = prn_DetSNo + 1
                            Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetSNo)), LMargin + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Delivery_No").ToString, LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                            '  Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.ClothType.Type4, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)

                            If prn_DetDt.Rows(prn_DetIndx).Item("po_no").ToString <> "" Then
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("po_no").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 5, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, " ", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 2, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, "998821", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + 2, CurY, 0, 0, pFont)
                            Else

                                Common_Procedures.Print_To_PrintDocument(e, "998821", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)
                                'Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Item_HSN_Code").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 5, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, " ", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 2, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Item_GST_Percentage").ToString & IIf(prn_DetDt.Rows(prn_DetIndx).Item("Item_GST_Percentage").ToString <> "", " %", ""), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)

                            End If
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Type4_Meters").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Reject_rate").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)

                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Reject_Amount").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            If Trim(ItmNm2) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If

                            prn_TotMtrs = prn_TotMtrs + Val(prn_DetDt.Rows(prn_DetIndx).Item("Type4_Meters").ToString)

                        End If

                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Type5_Meters").ToString) > 0 And Val(prn_DetDt.Rows(prn_DetIndx).Item("Others_rate").ToString) > 0 Then
                            CurY = CurY + TxtHgt

                            prn_DetSNo = prn_DetSNo + 1
                            Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetSNo)), LMargin + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Delivery_No").ToString, LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                            ' Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.ClothType.Type5, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)


                            If prn_DetDt.Rows(prn_DetIndx).Item("po_no").ToString <> "" Then
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("po_no").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 5, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, " ", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 2, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, "998821", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + 2, CurY, 0, 0, pFont)
                            Else

                                Common_Procedures.Print_To_PrintDocument(e, "998821", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)
                                'Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Item_HSN_Code").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 5, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, " ", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 2, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Item_GST_Percentage").ToString & IIf(prn_DetDt.Rows(prn_DetIndx).Item("Item_GST_Percentage").ToString <> "", " %", ""), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)

                            End If

                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Type5_Meters").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Others_rate").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)

                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Others_AMount").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            If Trim(ItmNm2) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If

                            prn_TotMtrs = prn_TotMtrs + Val(prn_DetDt.Rows(prn_DetIndx).Item("Type5_Meters").ToString)


                        End If




                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If


                Printing_Format_1186_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)

                If Trim(prn_InpOpts) <> "" Then
                    If Prn_Cnt_TEMP1 < Len(Trim(prn_InpOpts)) Then


                        If Val(prn_InpOpts) <> "0" Then
                            prn_DetIndx = 0
                            prn_DetSNo = 0

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

    Private Sub Printing_Format_1186_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim strHeight As Single, strWidth As Single = 0, CurX As Single = 0
        Dim C1 As Single, W1 As Single, S1 As Single
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String, Cmp_EMail As String, Cmp_Add3 As String, City As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_PanNo As String
        Dim Cmp_State As String, Cmp_StateCode As String, Cmp_GSTIN_No As String
        Dim I As Integer = 0
        Dim FrmJJ1 As String = ""
        Dim FrmJJ2 As String = ""
        Dim CurY1 As Single = 0
        Dim S As String = ""
        Dim vDcNoDt As String = ""
        Dim vDupDcNoDt As String = ""

        Dim ItmNm1 As String = ""
        Dim ItmNm2 As String = ""

        PageNo = PageNo + 1

        prn_Count = prn_Count + 1

        prn_OriDupTri = ""
        If Trim(prn_InpOpts) <> "" Then
            If Prn_Cnt_TEMP1 <= Len(Trim(prn_InpOpts)) Then

                ' S = Mid$(Trim(prn_InpOpts), prn_Count, 1)

                ' If PageNo <= 1 Then
                Prn_Cnt_Temp = Prn_Cnt_Temp + 1
                S = Mid$(Trim(prn_InpOpts), Prn_Cnt_Temp, 1)
                Prn_Cnt_TEMP1 = Prn_Cnt_TEMP1 + 1
                'End If

                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1018" Then '---- M.K Textiles (Palladam)
                    If Val(S) = 1 Then
                        prn_OriDupTri = "ORIGINAL"
                    ElseIf Val(S) = 2 Then
                        prn_OriDupTri = "TRANSPORT COPY"
                    ElseIf Val(S) = 3 Then
                        prn_OriDupTri = "TRIPLICATE"
                    ElseIf Val(S) = 4 Then
                        prn_OriDupTri = "EXTRA COPY"
                    Else
                        If Trim(prn_InpOpts) <> "0" And Val(prn_InpOpts) = "0" And Len(Trim(prn_InpOpts)) > 3 Then
                            prn_OriDupTri = Trim(prn_InpOpts)
                        End If
                    End If

                Else
                    If Val(S) = 1 Then
                        prn_OriDupTri = "ORIGINAL"
                    ElseIf Val(S) = 2 Then
                        prn_OriDupTri = "DUPLICATE"
                    ElseIf Val(S) = 3 Then
                        prn_OriDupTri = "TRIPLICATE"
                    ElseIf Val(S) = 4 Then
                        prn_OriDupTri = "EXTRA COPY"
                    Else
                        If Trim(prn_InpOpts) <> "0" And Val(prn_InpOpts) = "0" And Len(Trim(prn_InpOpts)) > 3 Then
                            prn_OriDupTri = Trim(prn_InpOpts)
                        End If
                    End If

                End If

            End If
        End If

        Cmp_Name = ""
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1021" Then

            CurY = TMargin + 185

            If Trim(prn_OriDupTri) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
            End If

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(1) = CurY

            CurY = CurY + 10
            p1Font = New Font("Calibri", 16, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "INVOICE", LMargin, CurY, 2, PrintWidth, p1Font)
            strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

            CurY = CurY + strHeight + 25
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(2) = CurY

        Else

            CurY = TMargin

            da2 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name, c.Ledger_Name as Transport_Name from JobWork_ConversionBill_Head a  INNER JOIN Ledger_Head b ON b.Ledger_IdNo = a.Ledger_Idno LEFT OUTER JOIN Ledger_Head c ON c.Ledger_IdNo = a.Transport_IdNo  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.JobWork_ConversionBill_Code = '" & Trim(EntryCode) & "' Order by a.For_OrderBy", con)
            da2.Fill(dt2)
            If dt2.Rows.Count > NoofItems_PerPage Then
                Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
            End If
            dt2.Clear()

            If Trim(prn_OriDupTri) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
            End If
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(1) = CurY

            Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
            Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_PanNo = ""
            Cmp_State = "" : Cmp_StateCode = "" : Cmp_GSTIN_No = "" : Cmp_EMail = ""
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString

            Cmp_Add1 = "Regd. Off : " & prn_HdDt.Rows(0).Item("Company_Address1").ToString
            Cmp_Add2 = "Factory : " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
            Cmp_Add3 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString

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
                Cmp_EMail = "EMAIL ID : " & prn_HdDt.Rows(0).Item("Company_EMail").ToString
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

            If Trim(prn_HdDt.Rows(0).Item("Company_City").ToString) <> "" Then
                City = "" & prn_HdDt.Rows(0).Item("Company_City").ToString
            End If

            CurY = CurY + TxtHgt - 10
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1238" Then
                p1Font = New Font("Cambria", 25, FontStyle.Bold)
            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1152" Then
                p1Font = New Font("Elephant", 22, FontStyle.Bold)
            Else
                p1Font = New Font("Calibri", 14, FontStyle.Bold)
            End If
            Dim vcompalign As Integer
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1186" Then
                vcompalign = 0
            Else
                vcompalign = 2
            End If

            Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin + 10, CurY, vcompalign, PrintWidth, p1Font)
            strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1186" Then
                e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.united_weaves_logo_png, Drawing.Image), PageWidth - 150, CurY, 120, 100)

            Else

                If Trim(prn_HdDt.Rows(0).Item("Company_logo_Image").ToString) <> "" Then

                    If IsDBNull(prn_HdDt.Rows(0).Item("Company_logo_Image")) = False Then

                        Dim imageData As Byte() = DirectCast(prn_HdDt.Rows(0).Item("Company_logo_Image"), Byte())
                        If Not imageData Is Nothing Then
                            Using ms As New MemoryStream(imageData, 0, imageData.Length)
                                ms.Write(imageData, 0, imageData.Length)
                                If imageData.Length > 0 Then
                                    e.Graphics.DrawImage(DirectCast(Image.FromStream(ms), Drawing.Image), LMargin + 20, CurY + 5, 120, 100)
                                End If
                            End Using

                        End If

                    End If

                End If

            End If



            If Trim(prn_HdDt.Rows(0).Item("E_Invoice_IRNO").ToString) <> "" Then

                If IsDBNull(prn_HdDt.Rows(0).Item("E_Invoice_QR_Image")) = False Then
                    Dim imageData As Byte() = DirectCast(prn_HdDt.Rows(0).Item("E_Invoice_QR_Image"), Byte())
                    If Not imageData Is Nothing Then
                        Using ms As New MemoryStream(imageData, 0, imageData.Length)
                            ms.Write(imageData, 0, imageData.Length)

                            If imageData.Length > 0 Then

                                pic_IRN_QRCode_Image_forPrinting.BackgroundImage = Image.FromStream(ms)

                                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1608" Then '---- SAMANTH TEXTILES (SOMANUR)
                                    e.Graphics.DrawImage(DirectCast(pic_IRN_QRCode_Image_forPrinting.BackgroundImage, Drawing.Image), PageWidth - 135, CurY + 5, 120, 100)
                                Else
                                    e.Graphics.DrawImage(DirectCast(pic_IRN_QRCode_Image_forPrinting.BackgroundImage, Drawing.Image), PageWidth - 300, CurY, 80, 80)
                                End If


                            End If

                        End Using

                    End If

                End If

            End If

            CurY = CurY + strHeight - 1
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin + 10, CurY, vcompalign, PrintWidth, pFont)

            CurY = CurY + TxtHgt - 1
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin + 10, CurY, vcompalign, PrintWidth, pFont)
            CurY = CurY + TxtHgt - 1
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Add3 & ",  " & City, LMargin + 10, CurY, vcompalign, PrintWidth, pFont)

            If Cmp_State <> "" Then
                CurY = CurY + TxtHgt - 1
                Common_Procedures.Print_To_PrintDocument(e, Cmp_State & "  " & Cmp_StateCode, LMargin + 10, CurY, vcompalign, PrintWidth, pFont)
            End If
            If Trim(Common_Procedures.settings.CustomerCode) = "1274" Then
                If Cmp_GSTIN_No <> "" Then
                    pFont = New Font("CALIBRI", 10, FontStyle.Bold)
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, LMargin + 10, CurY + 15, vcompalign, PrintWidth, pFont)
                End If
            Else
                If Cmp_GSTIN_No <> "" Then
                    CurY = CurY + TxtHgt - 1
                    Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No & "  / " & Cmp_PanNo, LMargin + 10, CurY, vcompalign, PrintWidth, pFont)
                End If
            End If

            CurY = CurY + TxtHgt + 10
            p1Font = New Font("Calibri", 16, FontStyle.Bold)
            'Common_Procedures.Print_To_PrintDocument(e, "JOBWORK INVOICE", LMargin, CurY, 2, PrintWidth, p1Font)
            strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

            pFont = New Font("CALIBRI", 10, FontStyle.Regular)

            If Cmp_PhNo <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo & "  / " & Cmp_EMail, LMargin + 10, CurY, vcompalign, PrintWidth, pFont)
            End If
            'If Cmp_EMail <> "" Then
            '    Common_Procedures.Print_To_PrintDocument(e, Cmp_EMail, PageWidth - 270, CurY, 0, 0, pFont)
            'End If


            '----------


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

                CurY = CurY + TxtHgt
                e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

                CurY = CurY + TxtHgt + 2
                p1Font = New Font("Calibri", 10, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "IRN : " & Trim(ItmNm1), LMargin + 10, CurY, 0, 0, p1Font)
                Common_Procedures.Print_To_PrintDocument(e, "Ack. No : " & prn_HdDt.Rows(0).Item("E_Invoice_ACK_No").ToString, PrintWidth - 270, CurY, 0, 0, p1Font)


                If Trim(ItmNm2) <> "" Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, "             " & Trim(ItmNm2), LMargin, CurY, 0, 0, p1Font)
                    Common_Procedures.Print_To_PrintDocument(e, "Ack. Date : " & prn_HdDt.Rows(0).Item("E_Invoice_ACK_Date").ToString, PrintWidth - 270, CurY, 0, 0, p1Font)
                End If


            End If

            CurY = CurY + TxtHgt + 10
            p1Font = New Font("Calibri", 18, FontStyle.Bold)

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(15) = CurY
            CurY = CurY + 5
            If Trim(Common_Procedures.settings.CustomerCode) = "1274" Then
                Common_Procedures.Print_To_PrintDocument(e, "CONVERSION BILL", LMargin, CurY, 2, PrintWidth, p1Font)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "JOBWORK INVOICE", LMargin, CurY, 2, PrintWidth, p1Font)
            End If
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "BILL No. : " & prn_HdDt.Rows(0).Item("JobWork_ConversionBill_No").ToString, LMargin + 10, CurY + 3, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "DATE : " & Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("JobWork_ConversionBill_Date").ToString), "dd-MM-yyyy").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 10, CurY + 3, 0, 0, p1Font)

            CurY = CurY + strHeight + 5
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + 70, CurY, LMargin + ClAr(1) + ClAr(2) + 70, LnAr(15))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(15))
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(2) = CurY

        End If


        Try
            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4)
            W1 = e.Graphics.MeasureString("FORM JJ  : ", pFont).Width
            S1 = e.Graphics.MeasureString("TO     :    ", pFont).Width

            CurY1 = CurY
            'Left side


            CurY1 = CurY1 + TxtHgt - 12
            p1Font = New Font("Calibri", 10, FontStyle.Bold Or FontStyle.Italic Or FontStyle.Underline)
            Common_Procedures.Print_To_PrintDocument(e, "BILLED TO  :", LMargin + 10, CurY1, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "DELIVERY TO : ", LMargin + C1 + 10, CurY1, 0, 0, p1Font)

            CurY1 = CurY1 + TxtHgt
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "M/s." & prn_HdDt.Rows(0).Item("Ledger_MainName").ToString, LMargin + 10, CurY1, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "M/s." & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerName").ToString, LMargin + C1 + 10, CurY1, 0, 0, p1Font)
            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress1").ToString, LMargin + C1 + 30, CurY1, 0, 0, pFont)
            p1Font = New Font("Calibri", 14, FontStyle.Bold)

            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress2").ToString, LMargin + C1 + 30, CurY1, 0, 0, pFont)
            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress3").ToString, LMargin + C1 + 30, CurY1, 0, 0, pFont)
            If Trim(prn_HdDt.Rows(0).Item("Ledger_Address4").ToString) <> "" Then
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress4").ToString, LMargin + C1 + 30, CurY1, 0, 0, pFont)
            End If

            CurY1 = CurY1 + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("Ledger_PhoneNo").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, " PHONE : " & prn_HdDt.Rows(0).Item("Ledger_PhoneNo").ToString, LMargin + S1 + 10, CurY1, 0, 0, pFont)
            End If
            If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerPhoneNo").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, " PHONE : " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerPhoneNo").ToString, LMargin + C1 + 30, CurY1, 0, 0, pFont)
            End If
            CurY1 = CurY1 + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, " STATE :   " & prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString & "        CODE : " & prn_HdDt.Rows(0).Item("Ledger_State_Code").ToString, LMargin + S1 + 10, CurY1, 0, 0, pFont)
            End If
            Common_Procedures.Print_To_PrintDocument(e, " STATE :   " & prn_HdDt.Rows(0).Item("DeliveryTo_State_Name").ToString & "        CODE : " & prn_HdDt.Rows(0).Item("DeliveryTo_State_Code").ToString, LMargin + C1 + 30, CurY1, 0, 0, pFont)

            pFont = New Font("CALIBRI", 10, FontStyle.Bold)
            CurY1 = CurY1 + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, " GSTIN : " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + S1 + 10, CurY1, 0, 0, pFont)
            End If


            If Trim(prn_HdDt.Rows(0).Item("Pan_No").ToString) <> "" Then
                strWidth = e.Graphics.MeasureString("GSTIN : " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, pFont).Width
                CurX = LMargin + S1 + 10 + strWidth
                Common_Procedures.Print_To_PrintDocument(e, "      PAN : " & prn_HdDt.Rows(0).Item("Pan_No").ToString, CurX, CurY1, 0, PrintWidth, pFont)
            End If


            If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerGSTinNo").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, " GSTIN : " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerGSTinNo").ToString, LMargin + C1 + 30, CurY1, 0, 0, pFont)
            End If

            If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_PanNo").ToString) <> "" Then
                strWidth = e.Graphics.MeasureString("GSTIN : " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerGSTinNo").ToString, pFont).Width
                CurX = LMargin + C1 + 30 + strWidth
                Common_Procedures.Print_To_PrintDocument(e, "      PAN : " & prn_HdDt.Rows(0).Item("DeliveryTo_PanNo").ToString, CurX, CurY1, 0, PrintWidth, pFont)
            End If
            pFont = New Font("CALIBRI", 10, FontStyle.Regular)
            'Right Side
            CurY = CurY + TxtHgt

            p1Font = New Font("Elephant", 9, FontStyle.Bold)
            'Common_Procedures.Print_To_PrintDocument(e, "BILL NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            ''If Common_Procedures.settings.CustomerCode = "1139" Then
            ''    Common_Procedures.Print_To_PrintDocument(e, "J-" & prn_HdDt.Rows(0).Item("JobWork_ConversionBill_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)
            ''Else
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("JobWork_ConversionBill_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)
            '' End If

            'p1Font = New Font("Calibri", 12, FontStyle.Bold)
            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("JobWork_ConversionBill_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)


            If prn_HdDt.Rows(0).Item("FormJJ_No").ToString <> "" Then

                CurY = CurY + TxtHgt
                FrmJJ1 = Trim(prn_HdDt.Rows(prn_DetIndx).Item("FormJJ_No").ToString)
                FrmJJ2 = ""
                If Len(FrmJJ1) > 17 Then
                    For I = 17 To 1 Step -1
                        If Mid$(Trim(FrmJJ1), I, 1) = " " Or Mid$(Trim(FrmJJ1), I, 1) = "," Or Mid$(Trim(FrmJJ1), I, 1) = "." Or Mid$(Trim(FrmJJ1), I, 1) = "-" Or Mid$(Trim(FrmJJ1), I, 1) = "/" Or Mid$(Trim(FrmJJ1), I, 1) = "_" Or Mid$(Trim(FrmJJ1), I, 1) = "(" Or Mid$(Trim(FrmJJ1), I, 1) = ")" Or Mid$(Trim(FrmJJ1), I, 1) = "\" Or Mid$(Trim(FrmJJ1), I, 1) = "[" Or Mid$(Trim(FrmJJ1), I, 1) = "]" Or Mid$(Trim(FrmJJ1), I, 1) = "{" Or Mid$(Trim(FrmJJ1), I, 1) = "}" Then Exit For
                    Next I
                    If I = 0 Then I = 17
                    FrmJJ2 = Microsoft.VisualBasic.Right(Trim(FrmJJ1), Len(FrmJJ1) - I)
                    FrmJJ1 = Microsoft.VisualBasic.Left(Trim(FrmJJ1), I - 1)
                End If

                'Common_Procedures.Print_To_PrintDocument(e, "E-Way Bill No", LMargin + C1 + 10, CurY, 0, 0, pFont)
                'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                'Common_Procedures.Print_To_PrintDocument(e, FrmJJ1, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

                'If Trim(FrmJJ2) <> "" Then
                '    Common_Procedures.Print_To_PrintDocument(e, FrmJJ2, LMargin + C1 + W1 + 30, CurY + TxtHgt - 3, 0, 0, pFont)
                'End If

            End If


            '---------


            If CurY1 > CurY Then CurY = CurY1

            CurY = CurY + TxtHgt + 6
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + C1, LnAr(3), LMargin + C1, LnAr(2))

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1238" Then

                da2 = New SqlClient.SqlDataAdapter("Select a.* from JobWork_ConversionBill_Details a Where a.JobWork_ConversionBill_Code = '" & Trim(EntryCode) & "' Order by a.sl_No", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_Details.Rows.Clear()

                vDcNoDt = ""
                vDupDcNoDt = ""
                If dt2.Rows.Count > 0 Then

                    For I = 0 To dt2.Rows.Count - 1

                        If InStr(1, Trim(UCase(vDupDcNoDt)), "~" & Trim(UCase(dt2.Rows(I).Item("Delivery_No").ToString)) & "~") = 0 Then
                            vDcNoDt = vDcNoDt & IIf(Trim(vDcNoDt) <> "", ",  ", "") & Trim(dt2.Rows(I).Item("Delivery_No").ToString) & "/" & dt2.Rows(I).Item("Delivery_Date").ToString
                            vDupDcNoDt = vDupDcNoDt & "~" & Trim(dt2.Rows(I).Item("Delivery_No").ToString) & "~"
                        End If

                    Next I

                End If

                CurY = CurY + TxtHgt - 10
                p1Font = New Font("Calibri", 9, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, "Dc No & Date : " & Trim(vDcNoDt), LMargin + 10, CurY, 0, 0, p1Font)
                CurY = CurY + 10

            End If

            CurY = CurY + TxtHgt - 10
            'Common_Procedures.Print_To_PrintDocument(e, "Transport Name : " & Trim(prn_HdDt.Rows(0).Item("TransportName").ToString), LMargin + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "Delivery Address : " & Trim(prn_HdDt.Rows(0).Item("Despatch_To").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 0, 0, pFont)

            'CurY = CurY + TxtHgt + 6 
            Common_Procedures.Print_To_PrintDocument(e, "E-way Bill No : " & FrmJJ1, LMargin + C1 + 10, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Vehicle No   : " & Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString), LMargin + 10, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt + 6

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY
            e.Graphics.DrawLine(Pens.Black, LMargin + C1, LnAr(4), LMargin + C1, LnAr(3))
            pFont = New Font("Calibri", 10, FontStyle.Bold)
            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)
            If Val(ClAr(2)) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "DC No.", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            End If
            Common_Procedures.Print_To_PrintDocument(e, "DESCRIPTION OF GOODS", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)

            If prn_DetDt.Rows(prn_DetIndx).Item("po_no").ToString <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "PO No.", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
                Common_Procedures.Print_To_PrintDocument(e, "No. Of", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
                Common_Procedures.Print_To_PrintDocument(e, "PCS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY + 15, 2, ClAr(5), pFont)
                Common_Procedures.Print_To_PrintDocument(e, "HSN/SAC", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            Else

                Common_Procedures.Print_To_PrintDocument(e, "HSN/SAC", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
                Common_Procedures.Print_To_PrintDocument(e, "No. Of", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
                Common_Procedures.Print_To_PrintDocument(e, "PCS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY + 15, 2, ClAr(5), pFont)
                Common_Procedures.Print_To_PrintDocument(e, "GST %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)

            End If
            Common_Procedures.Print_To_PrintDocument(e, "QTY.MTRS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "RATE/MTR", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT (Rs.)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)

            CurY = CurY + TxtHgt + 15
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub Printing_Format_1186_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim p2Font As Font
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
        Dim CurY1 As Single = 0
        Dim vBNKNAME As String
        Dim vBNKBRANCH As String
        Dim vBNKACNO As String
        Dim vBNKIFSC As String


        ' W1 = e.Graphics.MeasureString("No.of Beams  : ", pFont).Width

        Try

            For I = NoofDets + 1 To NoofItems_PerPage - 1

                CurY = CurY + TxtHgt

                prn_DetIndx = prn_DetIndx + 1

            Next

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

            pFont = New Font("Calibri", 10, FontStyle.Regular)
            CurY = CurY + TxtHgt - 10
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(Format(Val(prn_TotMtrs), "#######0.00")), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(Format(Val(prn_pcs))), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Gross_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
                prn_TotMtrs = 0
                prn_pcs = 0
            End If

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

            Dim Len1 As Integer = 0
            p1Font = New Font("Calibri", 9, FontStyle.Underline Or FontStyle.Bold)
            Len1 = e.Graphics.MeasureString("Amount Rs : ", pFont).Width

            CurY1 = CurY
            If is_LastPage = True Then
                '---Left Side   

                CurY1 = CurY1 + 4

                p1Font = New Font("Calibri", 12, FontStyle.Bold Or FontStyle.Underline)
                p2Font = New Font("Calibri", 10, FontStyle.Bold Or FontStyle.Underline)
                'p1Font = New Font("Calibri", 11, FontStyle.Bold Or FontStyle.Underline)
                'p2Font = New Font("Calibri", 9, FontStyle.Bold Or FontStyle.Underline)
                Common_Procedures.Print_To_PrintDocument(e, "BANK DETAILS", LMargin + 10, CurY1, 0, 0, p1Font)
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1186" Then
                    Common_Procedures.Print_To_PrintDocument(e, "PAYMENT DETAILS  : ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 10, CurY1, 0, 0, p2Font)
                End If


                vBNKNAME = ""
                vBNKBRANCH = ""
                vBNKACNO = ""
                vBNKIFSC = ""

                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1186" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1420" Or Common_Procedures.settings.CustomerCode = "1423" Or Common_Procedures.settings.CustomerCode = "1547" Then
                    vBNKNAME = BankNm1
                    vBNKBRANCH = BankNm2
                    vBNKACNO = BankNm3
                    vBNKIFSC = BankNm4
                Else
                    vBNKNAME = BankNm1
                    vBNKBRANCH = BankNm2
                    vBNKACNO = BankNm3
                    vBNKIFSC = BankNm4
                End If

                CurY1 = CurY1 + TxtHgt + 10
                p1Font = New Font("Calibri", 11, FontStyle.Bold)
                p2Font = New Font("Calibri", 9, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "BANK NAME      :  " & vBNKNAME, LMargin + 10, CurY1, 0, 0, p2Font)
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1186" Then
                    Common_Procedures.Print_To_PrintDocument(e, "Chq No  ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 10, CurY1, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":____________", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 10 + Len1 + 10, CurY1, 0, 0, pFont)
                End If

                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "ACCOUNT NO    :  " & vBNKACNO, LMargin + 10, CurY1, 0, 0, p2Font)
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1186" Then
                    Common_Procedures.Print_To_PrintDocument(e, "Bank ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 10, CurY1, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":____________", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 10 + Len1 + 10, CurY1, 0, 0, pFont)
                End If


                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "BRANCH NAME :  " & vBNKBRANCH, LMargin + 10, CurY1, 0, 0, p2Font)
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1186" Then
                    Common_Procedures.Print_To_PrintDocument(e, "Date   ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 10, CurY1, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":____________", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 10 + Len1 + 10, CurY1, 0, 0, pFont)
                End If

                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "IFSC CODE          :  " & vBNKIFSC, LMargin + 10, CurY1, 0, 0, p2Font)
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1186" Then
                    Common_Procedures.Print_To_PrintDocument(e, "Amount Rs.  ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 10, CurY1, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":____________", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 10 + Len1 + 10, CurY1, 0, 0, pFont)
                End If


            End If


            'Right Side

            CurY = CurY + TxtHgt - 10
            If is_LastPage = True Then
                If Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Freight_Caption_Name").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Freight_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
                End If
            End If

            CurY = CurY + TxtHgt
            If is_LastPage = True Then
                If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then
                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1238" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1239" Then '---- KALAIMAGAL TEX
                        Common_Procedures.Print_To_PrintDocument(e, "Trade Discount", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                    Else
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("AddLess_Caption_Name").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                    End If
                    Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
                End If

                If Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then
                    CurY = CurY + TxtHgt + 5
                    e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, PageWidth, CurY)
                End If
            End If


            CurY = CurY - 10

            CurY = CurY + TxtHgt
            If is_LastPage = True Then
                If Val(prn_HdDt.Rows(0).Item("Total_Taxable_Amount").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, "Total Assessable Value", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("Total_Taxable_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
                End If
            End If
            CurY = CurY + TxtHgt
            If is_LastPage = True Then
                If Val(prn_HdDt.Rows(0).Item("CGST_Amount").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, "CGST " & Val(prn_HdDt.Rows(0).Item("CGST_Percentage").ToString) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("CGST_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
                End If
            End If
            CurY = CurY + TxtHgt
            If is_LastPage = True Then
                If Val(prn_HdDt.Rows(0).Item("SGST_Amount").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, "SGST " & Val(prn_HdDt.Rows(0).Item("SGST_Percentage").ToString) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("SGST_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
                End If
            End If
            CurY = CurY + TxtHgt
            If is_LastPage = True Then
                If Val(prn_HdDt.Rows(0).Item("IGST_Amount").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, "IGST " & Val(prn_HdDt.Rows(0).Item("IGST_Percentage").ToString) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("IGST_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
                End If
            End If

            CurY = CurY + TxtHgt

            If is_LastPage = True Then
                If Val(prn_HdDt.Rows(0).Item("RoundOff_Amount").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, "RoundOff", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("RoundOff_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
                End If

            End If


            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, PageWidth, CurY)
            LnAr(8) = CurY
            'CurY = CurY + TxtHgt - 10
            'p1Font = New Font("Calibri", 13, FontStyle.Bold)
            'Common_Procedures.Print_To_PrintDocument(e, "(Textile manufacturing service (Weaving) )", LMargin + 10, CurY, 0, 0, p1Font)

            If is_LastPage = True Then
                pFont = New Font("Calibri", 12, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "NET AMOUNT (Rs.)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                p1Font = New Font("Calibri", 12, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, " " & Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Bill_Amount").ToString)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, p1Font)
            End If

            pFont = New Font("Calibri", 10, FontStyle.Regular)
            CurY = CurY + TxtHgt + 10
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1186" Then
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(7))
            End If
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(7))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(7))
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(9) = CurY
            ' e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))

            CurY = CurY + TxtHgt - 5

            If is_LastPage = True Then
                BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))
                'BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Bill_Amount").ToString))

                p1Font = New Font("Calibri", 12, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "Rupees  : " & BmsInWrds & " ", LMargin + 10, CurY, 0, 0, p1Font)
            End If
            Dim D1 As Single = 0
            ' W1 = e.Graphics.MeasureString("No.of Beams  : ", pFont).Width
            D1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5)
            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(15) = CurY
            CurY = CurY + 5
            p1Font = New Font("Calibri", 12, FontStyle.Underline)
            Common_Procedures.Print_To_PrintDocument(e, "Term & Conditions : ", LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "Goods Description : ", LMargin + D1 + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt + 10
            Common_Procedures.Print_To_PrintDocument(e, "1. Overdue Interest will be charged at 24% from the Invoice date. ", LMargin + 10, CurY, 0, 0, pFont)


            Common_Procedures.Print_To_PrintDocument(e, "1. Uncalendered Grey Fabrics. ", LMargin + D1 + 5, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "2. Our risk & responsibility ceases on goods leaving our factory.", LMargin + 10, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "2. Goods Not for Sale. Goods Sent after Job  ", LMargin + D1 + 5, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "3. Goods are supplied under our firm conditions.", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Work Compeletion. ", LMargin + D1 + 5, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + D1, CurY, PageWidth, CurY)
            Common_Procedures.Print_To_PrintDocument(e, "4. Subject to Tirupur jurisdiction. ", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Tax Is Payable On Reverse Charge : YES / NO", LMargin + D1 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, "5. E.O & E . ", LMargin + 10, CurY, 0, 0, pFont)
            e.Graphics.DrawLine(Pens.Black, LMargin + D1, CurY, PageWidth, CurY)
            'Common_Procedures.Print_To_PrintDocument(e, "WOVEN FABRIC AND RETURNED AFTER", LMargin + D1 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt - 10

            Common_Procedures.Print_To_PrintDocument(e, "WOVEN FABRIC AND RETURNED AFTER", LMargin + D1 + 10, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "CONVERSION", LMargin + D1 + 10, CurY, 0, 0, pFont)


            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(15))
            CurY = CurY + TxtHgt

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(15))
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)


            CurY = CurY + 5
            pFont = New Font("Calibri", 9, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, "Certified that particulars given above are true and correct and the amount indicated represents the price actually charged and that ", LMargin + 10, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "there is no flow of additional consideration directly or indirectly from the buyer.", LMargin + 10, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(10) = CurY
            pFont = New Font("Calibri", 10, FontStyle.Regular)

            'If Val(Common_Procedures.User.IdNo) <> 1 Then
            '    Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
            'End If

            CurY = CurY + TxtHgt - 10
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1152" Then
                p1Font = New Font("Elephant", 12, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)
            Else
                p1Font = New Font("Calibri", 12, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)
            End If

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1162" Then
                CurY = CurY + TxtHgt
                e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.vijay_tex_Sign2, Drawing.Image), PageWidth - 110, CurY, 90, 55)

            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1186" Then

                CurY = CurY + TxtHgt
                e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.UNITED_WEAVES_SIGN, Drawing.Image), PageWidth - 110, CurY, 90, 55)


            End If

            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            '        CurY = CurY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, "Prepared By", LMargin + 20, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Checked By ", LMargin + 350, CurY, 0, 0, pFont)
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

    Private Sub btn_Print_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        Print_PDF_Status = False
        print_record()
    End Sub

    Private Sub btn_PDF_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_PDF.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        Print_PDF_Status = True
        print_record()
        Print_PDF_Status = False
    End Sub
    Private Sub cbo_Agent_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Agent.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'AGENT')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Agent_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Agent.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Agent, txt_Rate_Type5, txt_com_per, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'AGENT')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Agent_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Agent.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim dt1 As New DataTable
        Dim New_Rate As Double = 0
        Dim clth_pick As Double = 0
        Dim chk_meter As Double = 0
        Dim Emp_idno As Integer = 0
        Dim Clth_idno As Integer = 0

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Agent, txt_com_per, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'AGENT')", "(Ledger_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            Emp_idno = Common_Procedures.Ledger_NameToIdNo(con, Trim(cbo_Agent.Text))

            da = New SqlClient.SqlDataAdapter("select a.* from ledger_head a Where a.Ledger_IdNo = " & Str(Val(Emp_idno)), con)
            da.Fill(dt)

            New_Rate = 0
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    New_Rate = Val(dt.Rows(0).Item("Cloth_Comm_Meter").ToString)
                End If
            End If

            dt.Dispose()
            da.Dispose()

            txt_com_per.Text = Val(New_Rate)

            wages_calculation()
        End If
    End Sub
    Private Sub wages_calculation()
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim dt1 As New DataTable
        Dim New_Rate As Double = 0
        Dim clth_pick As Double = 0
        Dim chk_meter As Double = 0
        Dim Emp_idno As Integer = 0
        Dim Clth_idno As Integer = 0
        Dim Clth_name As String = ""

        If dgv_Details.RowCount > 0 Then
            Clth_name = dgv_Details.Rows(0).Cells(dgvCol_Details.ClothName).Value
        End If

        Clth_idno = Common_Procedures.Cloth_NameToIdNo(con, Trim(Clth_name))

        da = New SqlClient.SqlDataAdapter("select a.* from Cloth_Head a Where a.Cloth_Idno = " & Str(Val(Clth_idno)), con)
        da.Fill(dt1)

        clth_pick = 0
        If dt1.Rows.Count > 0 Then
            If IsDBNull(dt1.Rows(0)(0).ToString) = False Then
                clth_pick = Val(dt1.Rows(0).Item("Cloth_Pick").ToString)
            End If
        End If

        dt1.Dispose()
        da.Dispose()

        chk_meter = Val(dgv_Details_Total.Rows(0).Cells(dgvCol_Details.Actual_Mtr).Value)

        txt_CommAmt.Text = Val(clth_pick * Val(txt_com_per.Text) * chk_meter)

    End Sub

    Private Sub cbo_Agent_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Agent.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Common_Procedures.MDI_LedType = "AGENT"
            Dim f As New Agent_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Agent.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub


    Private Sub txt_com_per_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_com_per.KeyPress

        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_CommAmt_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_CommAmt.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}") ' 
    End Sub
    Private Sub txt_Comm_Calc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_CommAmt.KeyPress

        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub



    Private Sub cbo_Grid_ClothName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_ClothName.GotFocus


        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")

    End Sub

    Private Sub cbo_Grid_ClothName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_ClothName.KeyDown



        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_ClothName, Nothing, Nothing, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")

        With dgv_Details

            If (e.KeyValue = 38 And cbo_Grid_ClothName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)

            End If

            If (e.KeyValue = 40 And cbo_Grid_ClothName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then


                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)



            End If

        End With
    End Sub

    Private Sub cbo_Grid_ClothName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_ClothName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_ClothName, Nothing, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            e.Handled = True

            With dgv_Details
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCol_Details.Pcs)


            End With

        End If
    End Sub

    Private Sub cbo_Grid_ClothName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_ClothName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Cloth_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_ClothName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub
    Private Sub cbo_Grid_ClothName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_ClothName.TextChanged
        Try
            If cbo_Grid_ClothName.Visible Then
                With dgv_Details
                    If Val(cbo_Grid_ClothName.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = dgvCol_Details.ClothName Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_ClothName.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub
    Private Sub cbo_Type_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Type.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")

    End Sub

    Private Sub cbo_Type_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Type.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Type, msk_Date, cbo_Ledger, "", "", "", "")

    End Sub

    Private Sub cbo_Type_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Type.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Type, cbo_Ledger, "", "", "", "")


    End Sub
    Private Sub cbo_Type_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Type.TextChanged
        If Trim(UCase(cbo_Type.Text)) = "DELIVERY" Then
            dgv_Details.AllowUserToAddRows = False
            'dgv_Details.ReadOnly = True

            'dgv_Details.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            dgv_Details.Columns(dgvCol_Details.DC_No).ReadOnly = False
            dgv_Details.Columns(dgvCol_Details.DC_Date).ReadOnly = False
            dgv_Details.Columns(dgvCol_Details.Dc_Mtr).ReadOnly = False
            dgv_Details.Columns(dgvCol_Details.ClothName).ReadOnly = False
            dgv_Details.Columns(dgvCol_Details.Type1_Meter).ReadOnly = False
            dgv_Details.Columns(dgvCol_Details.Type2_Meter).ReadOnly = False
            dgv_Details.Columns(dgvCol_Details.Type3_Meter).ReadOnly = False
            dgv_Details.Columns(dgvCol_Details.Type4_Meter).ReadOnly = False
            dgv_Details.Columns(dgvCol_Details.Type5_Meter).ReadOnly = False

            dgv_Details.Columns(dgvCol_Details.Sound_Rate).ReadOnly = False
            dgv_Details.Columns(dgvCol_Details.Seconds_Rate).ReadOnly = False
            dgv_Details.Columns(dgvCol_Details.Bits_Rate).ReadOnly = False
            dgv_Details.Columns(dgvCol_Details.Reject_Rate).ReadOnly = False
            dgv_Details.Columns(dgvCol_Details.Others_Rate).ReadOnly = False
        Else
            dgv_Details.AllowUserToAddRows = True
            dgv_Details.ReadOnly = False
            dgv_Details.SelectionMode = DataGridViewSelectionMode.CellSelect


        End If
    End Sub





    Private Sub cbo_Transport_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Transport.KeyUp
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

    End Sub

    Private Sub txt_FormJJNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_FormJJNo.KeyDown
        If e.KeyValue = 38 Or (e.Control = True And e.KeyValue = 38) Then
            cbo_VehicleNo.Focus()
        End If
        If (e.KeyValue = 40 And cbo_VehicleNo.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            'If Trim(UCase(cbo_Type.Text)) = "DELIVERY" Then
            '    txt_Rate_Type1.Focus()
            'Else
            '    If dgv_Details.RowCount > 0 Then
            '        dgv_Details.Focus()
            '        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
            '    Else
            '        txt_Rate_Type1.Focus()
            '    End If
            'End If
            txt_IR_No.Focus()
        End If
    End Sub

    Private Sub txt_FormJJNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_FormJJNo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            'If Trim(UCase(cbo_Type.Text)) = "DELIVERY" Then
            '    txt_Rate_Type1.Focus()
            'Else
            '    If dgv_Details.RowCount > 0 Then
            '        dgv_Details.Focus()
            '        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
            '    Else
            '        txt_Rate_Type1.Focus()
            '    End If
            'End If
            txt_IR_No.Focus()
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

    Private Sub txt_CGST_Perc_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_CGST_Perc.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub txt_SGST_Perc_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_SGST_Perc.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub txt_IGST_Perc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_IGST_Perc.KeyDown
        If e.KeyCode = 40 Then btn_save.Focus()
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_IGST_Perc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_IGST_Perc.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()
            Else
                msk_Date.Focus()
            End If
        End If
    End Sub

    Private Sub txt_IGST_Perc_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_IGST_Perc.TextChanged
        NetAmount_Calculation()
    End Sub




    Private Sub Printing_Format3(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim cmd As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim EntryCode As String
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font
        Dim vFontName As String = ""
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim LnAr(15) As Single, ClAr(15) As Single
        Dim ItmNm1 As String, ItmNm2 As String
        Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim W1 As Single = 0
        Dim SNo As Integer = 0
        Dim flperc As Single = 0
        Dim flmtr As Single = 0
        Dim fmtr As Single = 0
        Dim VechDesc1 As String = "", VechDesc2 As String = ""
        Dim vNoofHsnCodes As Integer = 0
        Dim vLine_Pen As Pen



        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1016" Then '---- Rajendra Textiles (Somanur)
            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
                    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                    PrintDocument1.DefaultPageSettings.PaperSize = ps
                    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                    e.PageSettings.PaperSize = ps
                    Exit For
                End If
            Next

        Else
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

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1016" Then '---- Rajendra Textiles (Somanur)
            With PrintDocument1.DefaultPageSettings.Margins
                .Left = 10
                .Right = 65
                .Top = 50 ' 60
                .Bottom = 40
                LMargin = .Left
                RMargin = .Right
                TMargin = .Top
                BMargin = .Bottom
            End With

        Else
            With PrintDocument1.DefaultPageSettings.Margins
                .Left = 30 ' 40
                .Right = 50
                .Top = 30 ' 20 '40 '50 ' 60
                .Bottom = 40
                LMargin = .Left
                RMargin = .Right
                TMargin = .Top
                BMargin = .Bottom
            End With

        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1229" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1152" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1151" Then '----Star Fabric Mills (Thekkalur)
            vFontName = "Cambria"
        Else
            vFontName = "Calibri"
        End If
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1151" Then
            pFont = New Font(vFontName, 9, FontStyle.Bold)
        Else
            pFont = New Font(vFontName, 9, FontStyle.Bold)
        End If

        pFont = New Font(vFontName, 9, FontStyle.Bold)
        'pFont = New Font(vFontName, 9, FontStyle.Bold)

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
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1044" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1307" Then
            NoofItems_PerPage = 6 ' 4 
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1140" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1084" Then
            NoofItems_PerPage = 15 ' 4 
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1257" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1203" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1211" Then
            NoofItems_PerPage = 8
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1235" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1151" Then
            NoofItems_PerPage = 12
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1258" Then
            NoofItems_PerPage = 7
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1266" Then
            NoofItems_PerPage = 8
        Else
            NoofItems_PerPage = 7
        End If

        EntryCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_BillNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)


        Erase LnAr
        Erase ClAr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}


        ClAr(1) = 30 : ClAr(2) = 50 : ClAr(3) = 200 : ClAr(4) = 60 : ClAr(5) = 60 : ClAr(6) = 80 : ClAr(7) = 80 : ClAr(8) = 60
        ClAr(9) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8))
        TxtHgt = e.Graphics.MeasureString("A", pFont).Height
        TxtHgt = 16




        vLine_Pen = New Pen(Color.Black, 2)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format3_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr, vLine_Pen, vFontName)

                ' W1 = e.Graphics.MeasureString("No.of Beams  : ", pFont).Width

                NoofDets = 0
                'prn_TotMtrs = 0

                CurY = CurY - 10

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If NoofDets >= (NoofItems_PerPage - 1) Then
                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_Format3_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, False)

                            e.HasMorePages = True
                            Return

                        End If


                        ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("cloth_Name").ToString)
                        ItmNm2 = ""
                        If Len(ItmNm1) > 22 Then
                            For I = 22 To 1 Step -1
                                If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 22
                            ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                            ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                        End If


                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Type1_Meters").ToString) > 0 And Val(prn_HdDt.Rows(0).Item("Rate_ClothType1").ToString) > 0 Then
                            CurY = CurY + TxtHgt

                            prn_DetSNo = prn_DetSNo + 1
                            Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetSNo)), LMargin + 10, CurY, 0, 0, pFont)
                            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1238" Then
                                If Val(ClAr(2)) <> 0 Then
                                    Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Delivery_No").ToString, LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                                    'p1Font = New Font("Calibri", 9, FontStyle.Regular)
                                    'Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Delivery_No").ToString & "/" & prn_DetDt.Rows(prn_DetIndx).Item("Delivery_Date").ToString, LMargin + clar(1) + 10, CurY, 0, 0, p1Font)
                                End If
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1) & " " & Common_Procedures.ClothType.Type1, LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
                            Else
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Delivery_No").ToString, LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
                            End If

                            Common_Procedures.Print_To_PrintDocument(e, "998821", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 5, CurY, 0, 0, pFont)
                            'Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Item_HSN_Code").ToString), LMargin + clar(1) + clar(2) + clar(3) + 5, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Item_GST_Percentage").ToString & IIf(prn_DetDt.Rows(prn_DetIndx).Item("Item_GST_Percentage").ToString <> "", " %", ""), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)

                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Type1_Meters").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Rate_ClothType1").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(0).Item("PCS").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)

                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Type1_Meters").ToString) * Val(prn_HdDt.Rows(0).Item("Rate_ClothType1").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            If Trim(ItmNm2) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If

                            prn_TotMtrs = prn_TotMtrs + Val(prn_DetDt.Rows(prn_DetIndx).Item("Type1_Meters").ToString)

                        End If

                        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1152" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1238" Then '----J.P.R Textile (PALLADAM) or AMMAN TEX (VELAYUTHAMPALAYAM)

                            If Val(prn_DetDt.Rows(prn_DetIndx).Item("Type2_Meters").ToString) > 0 Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1238" Then
                                CurY = CurY + TxtHgt

                                prn_DetSNo = prn_DetSNo + 1
                                Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetSNo)), LMargin + 10, CurY, 0, 0, pFont)
                                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1238" Then
                                    If Val(ClAr(2)) <> 0 Then
                                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Delivery_No").ToString, LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                                    End If
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1) & " " & Common_Procedures.ClothType.Type2, LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
                                Else
                                    Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Delivery_No").ToString, LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
                                End If

                                Common_Procedures.Print_To_PrintDocument(e, "998821", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 5, CurY, 0, 0, pFont)
                                'Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Item_HSN_Code").ToString), LMargin + clar(1) + clar(2) + clar(3) + 5, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Item_GST_Percentage").ToString & IIf(prn_DetDt.Rows(prn_DetIndx).Item("Item_GST_Percentage").ToString <> "", " %", ""), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)


                                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Type2_Meters").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Rate_ClothType2").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(0).Item("PCS").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)

                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Type2_Meters").ToString) * Val(prn_HdDt.Rows(0).Item("Rate_ClothType2").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

                                NoofDets = NoofDets + 1

                                If Trim(ItmNm2) <> "" Then
                                    CurY = CurY + TxtHgt - 5
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
                                    NoofDets = NoofDets + 1
                                End If

                                prn_TotMtrs = prn_TotMtrs + Val(prn_DetDt.Rows(prn_DetIndx).Item("Type2_Meters").ToString)

                            End If

                            If Val(prn_DetDt.Rows(prn_DetIndx).Item("Type3_Meters").ToString) > 0 Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1238" Then
                                CurY = CurY + TxtHgt

                                prn_DetSNo = prn_DetSNo + 1
                                Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetSNo)), LMargin + 10, CurY, 0, 0, pFont)
                                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1238" Then
                                    If Val(ClAr(2)) <> 0 Then
                                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Delivery_No").ToString, LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                                        'p1Font = New Font("Calibri", 9, FontStyle.Regular)
                                        'Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Delivery_No").ToString & "/" & prn_DetDt.Rows(prn_DetIndx).Item("Delivery_Date").ToString, LMargin + clar(1) + 10, CurY, 0, 0, p1Font)
                                    End If
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1) & " " & Common_Procedures.ClothType.Type3, LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
                                Else
                                    Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Delivery_No").ToString, LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
                                End If

                                Common_Procedures.Print_To_PrintDocument(e, "998821", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 5, CurY, 0, 0, pFont)
                                'Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Item_HSN_Code").ToString), LMargin + clar(1) + clar(2) + clar(3) + 5, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Item_GST_Percentage").ToString & IIf(prn_DetDt.Rows(prn_DetIndx).Item("Item_GST_Percentage").ToString <> "", " %", ""), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)

                                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Type3_Meters").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(0).Item("PCS").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)

                                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Rate_ClothType3").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)

                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Type3_Meters").ToString) * Val(prn_HdDt.Rows(0).Item("Rate_ClothType3").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

                                NoofDets = NoofDets + 1

                                If Trim(ItmNm2) <> "" Then
                                    CurY = CurY + TxtHgt - 5
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
                                    NoofDets = NoofDets + 1
                                End If

                                prn_TotMtrs = prn_TotMtrs + Val(prn_DetDt.Rows(prn_DetIndx).Item("Type3_Meters").ToString)

                            End If

                            If Val(prn_DetDt.Rows(prn_DetIndx).Item("Type4_Meters").ToString) > 0 Then
                                CurY = CurY + TxtHgt

                                prn_DetSNo = prn_DetSNo + 1
                                Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetSNo)), LMargin + 10, CurY, 0, 0, pFont)
                                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1238" Then
                                    If Val(ClAr(2)) <> 0 Then
                                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Delivery_No").ToString, LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                                        'p1Font = New Font("Calibri", 9, FontStyle.Regular)
                                        'Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Delivery_No").ToString & "/" & prn_DetDt.Rows(prn_DetIndx).Item("Delivery_Date").ToString, LMargin + clar(1) + 10, CurY, 0, 0, p1Font)
                                    End If
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1) & " " & Common_Procedures.ClothType.Type4, LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
                                Else
                                    Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Delivery_No").ToString, LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
                                End If

                                Common_Procedures.Print_To_PrintDocument(e, "998821", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 5, CurY, 0, 0, pFont)
                                'Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Item_HSN_Code").ToString), LMargin + clar(1) + clar(2) + clar(3) + 5, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Item_GST_Percentage").ToString & IIf(prn_DetDt.Rows(prn_DetIndx).Item("Item_GST_Percentage").ToString <> "", " %", ""), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)


                                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Type4_Meters").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(0).Item("PCS").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)

                                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Rate_ClothType4").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)

                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Type4_Meters").ToString) * Val(prn_HdDt.Rows(0).Item("Rate_ClothType4").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

                                NoofDets = NoofDets + 1

                                If Trim(ItmNm2) <> "" Then
                                    CurY = CurY + TxtHgt - 5
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
                                    NoofDets = NoofDets + 1
                                End If

                                prn_TotMtrs = prn_TotMtrs + Val(prn_DetDt.Rows(prn_DetIndx).Item("Type4_Meters").ToString)

                            End If

                            If Val(prn_DetDt.Rows(prn_DetIndx).Item("Type5_Meters").ToString) > 0 Then
                                CurY = CurY + TxtHgt

                                prn_DetSNo = prn_DetSNo + 1
                                Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetSNo)), LMargin + 10, CurY, 0, 0, pFont)
                                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1238" Then
                                    If Val(ClAr(2)) <> 0 Then
                                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Delivery_No").ToString, LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                                        'p1Font = New Font("Calibri", 9, FontStyle.Regular)
                                        'Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Delivery_No").ToString & "/" & prn_DetDt.Rows(prn_DetIndx).Item("Delivery_Date").ToString, LMargin + clar(1) + 10, CurY, 0, 0, p1Font)
                                    End If
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1) & " " & Common_Procedures.ClothType.Type5, LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
                                Else
                                    Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Delivery_No").ToString, LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
                                End If
                                'Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Delivery_No").ToString, LMargin + clar(1) + 10, CurY, 0, 0, pFont)
                                'Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + clar(1) + clar(2) + 10, CurY, 0, 0, pFont)

                                Common_Procedures.Print_To_PrintDocument(e, "998821", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 5, CurY, 0, 0, pFont)
                                'Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Item_HSN_Code").ToString), LMargin + clar(1) + clar(2) + clar(3) + 5, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Item_GST_Percentage").ToString & IIf(prn_DetDt.Rows(prn_DetIndx).Item("Item_GST_Percentage").ToString <> "", " %", ""), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)


                                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Type5_Meters").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(0).Item("PCS").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)

                                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Rate_ClothType5").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)

                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Type5_Meters").ToString) * Val(prn_HdDt.Rows(0).Item("Rate_ClothType5").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

                                NoofDets = NoofDets + 1

                                If Trim(ItmNm2) <> "" Then
                                    CurY = CurY + TxtHgt - 5
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
                                    NoofDets = NoofDets + 1
                                End If

                                prn_TotMtrs = prn_TotMtrs + Val(prn_DetDt.Rows(prn_DetIndx).Item("Type5_Meters").ToString)

                            End If

                        Else

                            If Val(prn_DetDt.Rows(prn_DetIndx).Item("Type2_Meters").ToString) > 0 And Val(prn_HdDt.Rows(0).Item("Rate_ClothType2").ToString) > 0 Then
                                CurY = CurY + TxtHgt

                                prn_DetSNo = prn_DetSNo + 1
                                Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetSNo)), LMargin + 10, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Delivery_No").ToString, LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
                                'Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.ClothType.Type2, LMargin + clar(1) + clar(2) + clar(3) + 10, CurY, 0, 0, pFont)

                                Common_Procedures.Print_To_PrintDocument(e, "998821", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 5, CurY, 0, 0, pFont)
                                'Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Item_HSN_Code").ToString), LMargin + clar(1) + clar(2) + clar(3) + 5, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Item_GST_Percentage").ToString & IIf(prn_DetDt.Rows(prn_DetIndx).Item("Item_GST_Percentage").ToString <> "", " %", ""), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)


                                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Type2_Meters").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(0).Item("PCS").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)

                                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Rate_ClothType2").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)

                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Type2_Meters").ToString) * Val(prn_HdDt.Rows(0).Item("Rate_ClothType2").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

                                NoofDets = NoofDets + 1

                                If Trim(ItmNm2) <> "" Then
                                    CurY = CurY + TxtHgt - 5
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
                                    NoofDets = NoofDets + 1
                                End If

                                prn_TotMtrs = prn_TotMtrs + Val(prn_DetDt.Rows(prn_DetIndx).Item("Type2_Meters").ToString)

                            End If

                            If Val(prn_DetDt.Rows(prn_DetIndx).Item("Type3_Meters").ToString) > 0 And Val(prn_HdDt.Rows(0).Item("Rate_ClothType3").ToString) > 0 Then
                                CurY = CurY + TxtHgt

                                prn_DetSNo = prn_DetSNo + 1
                                Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetSNo)), LMargin + 10, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Delivery_No").ToString, LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
                                'Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.ClothType.Type3, LMargin + clar(1) + clar(2) + clar(3) + 10, CurY, 0, 0, pFont)

                                Common_Procedures.Print_To_PrintDocument(e, "998821", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 5, CurY, 0, 0, pFont)
                                'Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Item_HSN_Code").ToString), LMargin + clar(1) + clar(2) + clar(3) + 5, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Item_GST_Percentage").ToString & IIf(prn_DetDt.Rows(prn_DetIndx).Item("Item_GST_Percentage").ToString <> "", " %", ""), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)

                                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Type3_Meters").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(0).Item("PCS").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)

                                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Rate_ClothType3").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)

                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Type3_Meters").ToString) * Val(prn_HdDt.Rows(0).Item("Rate_ClothType3").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

                                NoofDets = NoofDets + 1

                                If Trim(ItmNm2) <> "" Then
                                    CurY = CurY + TxtHgt - 5
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
                                    NoofDets = NoofDets + 1
                                End If

                                prn_TotMtrs = prn_TotMtrs + Val(prn_DetDt.Rows(prn_DetIndx).Item("Type3_Meters").ToString)

                            End If

                            If Val(prn_DetDt.Rows(prn_DetIndx).Item("Type4_Meters").ToString) > 0 And Val(prn_HdDt.Rows(0).Item("Rate_ClothType4").ToString) > 0 Then
                                CurY = CurY + TxtHgt

                                prn_DetSNo = prn_DetSNo + 1
                                Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetSNo)), LMargin + 10, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Delivery_No").ToString, LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
                                '  Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.ClothType.Type4, LMargin + clar(1) + clar(2) + clar(3) + 10, CurY, 0, 0, pFont)

                                Common_Procedures.Print_To_PrintDocument(e, "998821", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 5, CurY, 0, 0, pFont)
                                'Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Item_HSN_Code").ToString), LMargin + clar(1) + clar(2) + clar(3) + 5, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Item_GST_Percentage").ToString & IIf(prn_DetDt.Rows(prn_DetIndx).Item("Item_GST_Percentage").ToString <> "", " %", ""), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)


                                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Type4_Meters").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(0).Item("PCS").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)

                                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Rate_ClothType4").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)

                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Type4_Meters").ToString) * Val(prn_HdDt.Rows(0).Item("Rate_ClothType4").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

                                NoofDets = NoofDets + 1

                                If Trim(ItmNm2) <> "" Then
                                    CurY = CurY + TxtHgt - 5
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
                                    NoofDets = NoofDets + 1
                                End If

                                prn_TotMtrs = prn_TotMtrs + Val(prn_DetDt.Rows(prn_DetIndx).Item("Type4_Meters").ToString)

                            End If

                            If Val(prn_DetDt.Rows(prn_DetIndx).Item("Type5_Meters").ToString) > 0 And Val(prn_HdDt.Rows(0).Item("Rate_ClothType5").ToString) > 0 Then
                                CurY = CurY + TxtHgt

                                prn_DetSNo = prn_DetSNo + 1
                                Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetSNo)), LMargin + 10, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Delivery_No").ToString, LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
                                ' Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.ClothType.Type5, LMargin + clar(1) + clar(2) + clar(3) + 10, CurY, 0, 0, pFont)


                                Common_Procedures.Print_To_PrintDocument(e, "998821", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 5, CurY, 0, 0, pFont)
                                'Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Item_HSN_Code").ToString), LMargin + clar(1) + clar(2) + clar(3) + 5, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Item_GST_Percentage").ToString & IIf(prn_DetDt.Rows(prn_DetIndx).Item("Item_GST_Percentage").ToString <> "", " %", ""), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)


                                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Type5_Meters").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(0).Item("PCS").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Rate_ClothType5").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)

                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Type5_Meters").ToString) * Val(prn_HdDt.Rows(0).Item("Rate_ClothType5").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

                                NoofDets = NoofDets + 1

                                If Trim(ItmNm2) <> "" Then
                                    CurY = CurY + TxtHgt - 5
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
                                    NoofDets = NoofDets + 1
                                End If

                                prn_TotMtrs = prn_TotMtrs + Val(prn_DetDt.Rows(prn_DetIndx).Item("Type5_Meters").ToString)

                            End If
                        End If



                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If


                Printing_Format3_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True)

                If Trim(prn_InpOpts) <> "" Then
                    If prn_Count < Len(Trim(prn_InpOpts)) Then


                        If Val(prn_InpOpts) <> "0" Then
                            prn_DetIndx = 0
                            prn_DetSNo = 0

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





    Private Sub Printing_Format3_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByRef NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal vLine_Pen As Pen, ByVal vFontName As String)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim strHeight As Single = 0, strWidth As Single = 0
        Dim C1 As Single, W1, W2, W3 As Single, S1, S2, S3 As Single
        Dim Cmp_Name, Desc As String, Cmp_Add1 As String, Cmp_Add2 As String, Cmp_PanNo As String, Cmp_PanCap As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_EMail As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim CurX As Single = 0, CurY1 As Single = 0, CurY2 As Single = 0
        Dim Inv_No As String = ""
        Dim InvSubNo As String = ""
        Dim Rate_PCMETER As String = ""
        Dim vPgNo_TXT As String = ""
        Dim vDcNoDt As String = ""
        Dim vDupDcNoDt As String = ""
        PageNo = PageNo + 1


        CurY = TMargin

        Dim br = New SolidBrush(Color.FromArgb(255, 0, 0))


        prn_Count = prn_Count + 1

        prn_OriDupTri = ""
        prn_OriDupTri_Count = 0
        If Trim(prn_InpOpts) <> "" Then
            If prn_Count <= Len(Trim(prn_InpOpts)) Then

                prn_OriDupTri_Count = Mid$(Trim(prn_InpOpts), prn_Count, 1)

                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1018" Then '---- M.K Textiles (Palladam)
                    If Val(prn_OriDupTri_Count) = 1 Then
                        prn_OriDupTri = "ORIGINAL"
                    ElseIf Val(prn_OriDupTri_Count) = 2 Then
                        prn_OriDupTri = "TRANSPORT COPY"
                    ElseIf Val(prn_OriDupTri_Count) = 3 Then
                        prn_OriDupTri = "TRIPLICATE"
                    ElseIf Val(prn_OriDupTri_Count) = 4 Then
                        prn_OriDupTri = "EXTRA COPY"
                    Else
                        If Trim(prn_InpOpts) <> "0" And Val(prn_InpOpts) = "0" And Len(Trim(prn_InpOpts)) > 3 Then
                            prn_OriDupTri = Trim(prn_InpOpts)
                        End If
                    End If

                Else
                    If Val(prn_OriDupTri_Count) = 1 Then
                        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1169" Then
                            prn_OriDupTri = "ORIGINAL FOR BUYER"
                        Else
                            prn_OriDupTri = "ORIGINAL FOR RECEIPIENT"
                        End If

                    ElseIf Val(prn_OriDupTri_Count) = 2 Then
                        prn_OriDupTri = "DUPLICATE FOR TRANSPORTER"
                    ElseIf Val(prn_OriDupTri_Count) = 3 Then
                        prn_OriDupTri = "TRIPLICATE FOR SUPPLIER"
                    ElseIf Val(prn_OriDupTri_Count) = 4 Then
                        prn_OriDupTri = "EXTRA COPY"
                    Else
                        If Trim(prn_InpOpts) <> "0" And Val(prn_InpOpts) = "0" And Len(Trim(prn_InpOpts)) > 3 Then
                            prn_OriDupTri = Trim(prn_InpOpts)
                        End If
                    End If

                End If

            End If
        End If


        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1018" Then '---- M.K Textiles (Palladam)
        p1Font = New Font(vFontName, 14, FontStyle.Bold)
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1044" Then
            Common_Procedures.Print_To_PrintDocument(e, "GST TAX INVOICE", LMargin, CurY - TxtHgt - 5, 2, PrintWidth, p1Font)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1151" Then '---- BHARATHI TEXTILE (TIRUPUR)
            Common_Procedures.Print_To_PrintDocument(e, "TAX INVOICE CLOTH", LMargin, CurY - TxtHgt - 5, 2, PrintWidth, p1Font)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1060" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1185" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1187" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1257" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1258" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1296" Then '---- Selvanayaki Textiles (Karumanthapatti)
            Common_Procedures.Print_To_PrintDocument(e, "TAX INVOICE", LMargin, CurY - TxtHgt - 5, 2, PrintWidth, p1Font, Brushes.Red)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1283" Then
            Common_Procedures.Print_To_PrintDocument(e, "TAX INVOICE", LMargin, CurY - TxtHgt - 5, 2, PrintWidth, p1Font, Brushes.Blue)
        Else
            Common_Procedures.Print_To_PrintDocument(e, "JOB WORK INVOICE", LMargin, CurY - TxtHgt - 5, 2, PrintWidth, p1Font)
        End If
        '  End If
        If Trim(prn_OriDupTri) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If

        e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
        'e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1229" Then '----Star Fabric Mills (Thekkalur)
            p1Font = New Font(vFontName, 8, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, "*** shree ***", LMargin, CurY + 5, 2, PrintWidth, p1Font)
            CurY = CurY + 5
        End If
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1239" Then '---- Sri Sathis Textile (Mangalam-VelayuthamPalayam)
            p1Font = New Font(vFontName, 8, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, "Sri Selvanayaki Amman Thunai", LMargin, CurY + 5, 2, PrintWidth, p1Font)
            CurY = CurY + 5
        End If

        Desc = ""
        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_PanNo = "" : Cmp_EMail = "" : Cmp_PanCap = ""
        Cmp_StateCap = "" : Cmp_StateNm = "" : Cmp_StateCode = "" : Cmp_GSTIN_Cap = "" : Cmp_GSTIN_No = ""

        Desc = prn_HdDt.Rows(0).Item("Company_Description").ToString
        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString

        If Trim(prn_HdDt.Rows(0).Item("Company_Factory_Address1").ToString) <> "" Or Trim(prn_HdDt.Rows(0).Item("Company_Factory_Address2").ToString) <> "" Or Trim(prn_HdDt.Rows(0).Item("Company_Factory_Address3").ToString) <> "" Or Trim(prn_HdDt.Rows(0).Item("Company_Factory_Address4").ToString) <> "" Then
            Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString
            Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Factory_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Factory_Address2").ToString & " " & prn_HdDt.Rows(0).Item("Company_Factory_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Factory_Address4").ToString

        Else
            Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
            Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString

        End If

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
        If Trim(prn_HdDt.Rows(0).Item("State_Name").ToString) <> "" Then
            Cmp_StateCap = "STATE : "
            Cmp_StateNm = prn_HdDt.Rows(0).Item("State_Name").ToString
            If Trim(prn_HdDt.Rows(0).Item("State_Code").ToString) <> "" Then
                Cmp_StateNm = Cmp_StateNm & "   CODE : " & prn_HdDt.Rows(0).Item("State_Code").ToString
            End If
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString) <> "" Then
            Cmp_GSTIN_Cap = "GSTIN : "
            Cmp_GSTIN_No = prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
        End If

        CurY = CurY + TxtHgt - 5
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1018" Then '---- M.K Textiles (Palladam)
            If Val(lbl_Company.Tag) = 1 Then
                e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Company_Logo_MK, Drawing.Image), LMargin + 20, CurY, 112, 80)

            End If

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1065" Then '---- Logu textile
            If InStr(1, Trim(UCase(Cmp_Name)), "GANAPATHY") > 0 Or InStr(1, Trim(UCase(Cmp_Name)), "GANAPATHI") > 0 Then                                    '---- Ganapathy Spinning textile
                e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.GSM_LOGO, Drawing.Image), LMargin + 20, CurY, 112, 80)
            ElseIf InStr(1, Trim(UCase(Cmp_Name)), "LOGU") > 0 Or InStr(1, Trim(UCase(Cmp_Name)), "LOGA") > 0 Then                                          '---- Logu textile
                e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Company_Logo_LogaTex, Drawing.Image), LMargin + 20, CurY, 112, 80)
            End If

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1060" Then '---- Selvanayaki Textiles (Karumanthapatti)
            e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Company_Logo_Selvanayaki_Kpati, Drawing.Image), LMargin + 20, CurY - 10, 120, 90)

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1098" Then '---- Bannari amman textiles
            e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.REVISED_LOGO_7___2_, Drawing.Image), LMargin + 20, CurY - 10, 130, 110)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1040" Then '---- m.s textiles
            e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.ms_logo_2, Drawing.Image), LMargin + 20, CurY - 10, 130, 110)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1029" Then                   '---- Arul Kumaran Textiles (Somanur)
            e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Company_Logo_ArulKumaran, Drawing.Image), LMargin + 20, CurY - 5, 100, 90)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then '---- Kalaimagal Textiles (Palladam)
            e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.KmtOe, Drawing.Image), LMargin + 10, CurY - 5, 120, 100)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1045" Then '---- Kesavalogu textiles
            e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.KeasavLogu, Drawing.Image), LMargin + 10, CurY - 5, 120, 100)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1152" Then '---- j.p.r TEXTILES 
            If InStr(1, Trim(UCase(Cmp_Name)), "JPR") > 0 Or InStr(1, Trim(UCase(Cmp_Name)), "J.P.R") > 0 Then
                e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.JPR_LOGO2, Drawing.Image), LMargin + 10, CurY - 10, 150, 110)
            End If
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1169" Then '---- GANESHA TEXTILES
            e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.GANESH_TEX, Drawing.Image), LMargin + 5, CurY - 15, 160, 115)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1173" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1140" Then '---- S.P TEXTILES & ANATHARAJA
            e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.SPT, Drawing.Image), LMargin + 5, CurY - 10, 140, 100)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1185" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1187" Then '----  SAKTHI VINAYAGA TEXTILES
            e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Shakthi_Vinayaka, Drawing.Image), LMargin + 5, CurY - 5, 115, 115)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1136" Then '---- ps TEXTILES
            e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.PS_LOGO, Drawing.Image), LMargin + 10, CurY - 15, 115, 115)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1257" Then '----  senthil murugan TEXTILES
            e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Shakthi_Vinayaka, Drawing.Image), LMargin + 5, CurY - 5, 115, 115)
            e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.MURUGAN1, Drawing.Image), PageWidth - 125, CurY - 5, 115, 115)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1258" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "3000" Then
            e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.MURUGAN1, Drawing.Image), LMargin + 10, CurY - 5, 115, 115)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1213" Then '----
            If Val(lbl_Company.Tag) = 1 Then
                e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.ammanswami, Drawing.Image), LMargin + 10, CurY, 112, 110)
            Else
                e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.MURUGAN1, Drawing.Image), LMargin + 10, CurY, 112, 110)
            End If
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1214" Then '----
            e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Vinayakar_cholatx, Drawing.Image), LMargin + 10, CurY + 10, 112, 110)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1229" Then '----Star Fabric Mills (Thekkalur)
            e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Company_Logo_Star_1229, Drawing.Image), LMargin + 10, CurY, 112, 110)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1238" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1239" Then                   '---- Amman Tex (Velayuthampalayam)    and Sri Sathis Textiles(velayuthampalayam)
            e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Company_Logo_AmmanTex, Drawing.Image), LMargin + 20, CurY - 5, 100, 90)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1249" Then
            e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Company_Logo_Vaipav, Drawing.Image), LMargin + 20, CurY - 5, 100, 90)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1326" Then
            e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Company_Logo_GS_ELECTRONICS, Drawing.Image), LMargin + 10, CurY - 2, 110, 90)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1328" Then
            If InStr(1, Trim(UCase(Cmp_Name)), "KARTHIK") > 0 Then
                e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Company_Logo_KarthikTex, Drawing.Image), LMargin + 20, CurY - 3, 140, 90)
            End If
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1331" Then
            e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Company_Logo_SubaSri_Textile, Drawing.Image), LMargin + 10, CurY - 2, 110, 90)
        ElseIf Trim(Common_Procedures.settings.CustomerCode) = "1033" Then
            If Trim(Cmp_Name) = "RAJESWARI WEAVING MILL" Then
                e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Rajeswari_Weaving_Logo, Drawing.Image), LMargin + 10, CurY, 180, 70)
            ElseIf Trim(Cmp_Name) = "RAJESWARI WOVENS" Then
                e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Rajeswari_Wovens_Logo, Drawing.Image), LMargin + 10, CurY, 180, 70)
            End If
        End If

        'Dim br = New SolidBrush(Color.FromArgb(255, 0, 0))

        p1Font = New Font(vFontName, 18, FontStyle.Bold)
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1060" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1185" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1187" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1257" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1258" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1296" Then '---- Selvanayaki Textiles (Karumanthapatti)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font, Brushes.Red)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1283" Then
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font, Brushes.Blue)

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1213" Then
            p1Font = New Font(vFontName, 24, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font, br)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1214" Then
            br = New SolidBrush(Color.FromArgb(249, 99, 40))
            p1Font = New Font(vFontName, 22, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font, br)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1303" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "3000" Then
            p1Font = New Font(vFontName, 25, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1152" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1159" Then
            p1Font = New Font("Elephant", 22, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        Else
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        End If

        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1213" Then
            CurY = CurY + strHeight - 15
        Else
            CurY = CurY + strHeight - 7
        End If

        If Desc <> "" Then
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1060" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1185" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1187" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1257" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1258" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1296" Then '---- Selvanayaki Textiles (Karumanthapatti)
                Common_Procedures.Print_To_PrintDocument(e, Desc, LMargin, CurY, 2, PrintWidth, pFont, Brushes.Green)
            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1213" Then
                Common_Procedures.Print_To_PrintDocument(e, Desc, LMargin, CurY, 2, PrintWidth, pFont, Brushes.Gray)
            Else
                Common_Procedures.Print_To_PrintDocument(e, Desc, LMargin, CurY, 2, PrintWidth, pFont)
            End If
        End If

        br = New SolidBrush(Color.FromArgb(0, 150, 0))

        strWidth = e.Graphics.MeasureString(Trim(Cmp_Add1 & " " & Cmp_Add2), p1Font).Width
        p1Font = New Font(vFontName, 11, FontStyle.Bold)

        If PrintWidth > strWidth And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1258" Then
            If Trim(Cmp_Add1 & " " & Cmp_Add2) <> "" Then
                CurY = CurY + TxtHgt - 1
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1060" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1185" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1187" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1257" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1258" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1296" Then '---- Selvanayaki Textiles (Karumanthapatti)
                    Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_Add1 & " " & Cmp_Add2), LMargin, CurY, 2, PrintWidth, pFont, Brushes.Green)
                ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1213" Then
                    Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_Add1 & " " & Cmp_Add2), LMargin, CurY - 5, 2, PrintWidth, p1Font, br)
                Else
                    Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_Add1 & " " & Cmp_Add2), LMargin, CurY, 2, PrintWidth, pFont)
                End If
            End If

            NoofItems_PerPage = NoofItems_PerPage - 1

        Else

            If Cmp_Add1 <> "" Then
                CurY = CurY + TxtHgt - 1
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1060" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1185" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1187" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1257" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1258" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1296" Then '---- Selvanayaki Textiles (Karumanthapatti)
                    Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont, Brushes.Green)
                ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1213" Then
                    Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY - 10, 2, PrintWidth, p1Font, br)
                Else
                    Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)
                End If

            End If
            If Cmp_Add2 <> "" Then
                CurY = CurY + TxtHgt - 1
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1060" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1185" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1187" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1257" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1258" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1296" Then '---- Selvanayaki Textiles (Karumanthapatti)
                    Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont, Brushes.Green)
                ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1213" Then
                    Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY - 5, 2, PrintWidth, p1Font, br)

                Else
                    Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
                End If
            End If

        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1018" Then '---- M.K Textiles (Palladam)

            If Cmp_StateNm <> "" Then
                CurY = CurY + TxtHgt - 1
                Common_Procedures.Print_To_PrintDocument(e, Cmp_StateNm & "  " & Cmp_StateCode, LMargin, CurY, 2, PrintWidth, pFont)
            End If
            If Cmp_EMail <> "" Then
                CurY = CurY + TxtHgt - 1
                Common_Procedures.Print_To_PrintDocument(e, Cmp_EMail, LMargin, CurY, 2, PrintWidth, pFont)
            End If
            If Cmp_GSTIN_No <> "" Then
                CurY = CurY + TxtHgt - 1
                p1Font = New Font(vFontName, 12, FontStyle.Bold)
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "3000" Then
                    p1Font = New Font(vFontName, 15, FontStyle.Bold)
                    Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_Cap & Cmp_GSTIN_No, LMargin, CurY, 2, PrintWidth, p1Font)
                Else
                    p1Font = New Font(vFontName, 12, FontStyle.Bold)
                    Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_Cap & Cmp_GSTIN_No, LMargin, CurY, 2, PrintWidth, p1Font)
                End If
            End If
            If Cmp_PhNo <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin + 10, CurY, 0, 0, pFont)
            End If

        Else

            CurY = CurY + TxtHgt
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1060" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1185" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1187" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1257" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1258" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1296" Then '---- Selvanayaki Textiles (Karumanthapatti)
                Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PhNo & " / " & Cmp_EMail), LMargin, CurY, 2, PrintWidth, pFont, Brushes.Green)
            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1213" Then
                Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PhNo & " / " & Cmp_EMail), LMargin, CurY, 2, PrintWidth, p1Font, br)
            Else
                Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PhNo & " / " & Cmp_EMail), LMargin, CurY, 2, PrintWidth, pFont)
            End If

            CurY = CurY + TxtHgt

            p1Font = New Font(vFontName, 11, FontStyle.Bold)
            strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_GSTIN_Cap), p1Font).Width
            strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_StateNm & "     " & Cmp_GSTIN_Cap & Cmp_GSTIN_No & "     " & Cmp_PanCap & Cmp_PanNo), pFont).Width
            If PrintWidth > strWidth Then
                CurX = LMargin + (PrintWidth - strWidth) / 2
            Else
                CurX = LMargin
            End If

            p1Font = New Font(vFontName, 10, FontStyle.Bold)
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1060" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1185" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1187" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1257" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1258" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1296" Then '---- Selvanayaki Textiles (Karumanthapatti)
                Common_Procedures.Print_To_PrintDocument(e, Cmp_StateCap, CurX, CurY, 0, 0, p1Font, Brushes.Green)
            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1213" Then
                Common_Procedures.Print_To_PrintDocument(e, Cmp_StateCap, CurX, CurY + 5, 0, 0, p1Font, br)

            Else
                Common_Procedures.Print_To_PrintDocument(e, Cmp_StateCap, CurX, CurY, 0, 0, p1Font)
            End If

            strWidth = e.Graphics.MeasureString(Cmp_StateCap, p1Font).Width
            CurX = CurX + strWidth
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1060" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1185" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1187" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1257" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1258" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1296" Then '---- Selvanayaki Textiles (Karumanthapatti)
                Common_Procedures.Print_To_PrintDocument(e, Cmp_StateNm, CurX, CurY, 0, 0, pFont, Brushes.Green)
            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1213" Then
                Common_Procedures.Print_To_PrintDocument(e, Cmp_StateNm, CurX, CurY + 5, 0, 0, p1Font, br)

            Else
                Common_Procedures.Print_To_PrintDocument(e, Cmp_StateNm, CurX, CurY, 0, 0, pFont)
            End If


            strWidth = e.Graphics.MeasureString(Cmp_StateNm, pFont).Width
            p1Font = New Font(vFontName, 12, FontStyle.Bold)
            CurX = CurX + strWidth
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1060" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1185" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1187" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1257" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1258" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1296" Then '---- Selvanayaki Textiles (Karumanthapatti)
                Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_GSTIN_Cap, CurX, CurY - 3, 0, PrintWidth, p1Font, Brushes.Green)
            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1213" Then
                Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_GSTIN_Cap, CurX, CurY + 2, 0, PrintWidth, p1Font, br)

            Else
                Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_GSTIN_Cap, CurX, CurY - 3, 0, PrintWidth, p1Font)
            End If

            strWidth = e.Graphics.MeasureString("     " & Cmp_GSTIN_Cap, p1Font).Width
            CurX = CurX + strWidth
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1060" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1185" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1187" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1257" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1258" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1296" Then '---- Selvanayaki Textiles (Karumanthapatti)
                Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, CurX, CurY - 3, 0, 0, p1Font, Brushes.Green)
            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1213" Then
                Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, CurX, CurY + 2, 0, 0, p1Font, br)

            Else
                Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, CurX, CurY - 3, 0, 0, p1Font)
            End If

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1044" Then
                strWidth = e.Graphics.MeasureString(Cmp_GSTIN_No, p1Font).Width
                p1Font = New Font(vFontName, 10, FontStyle.Bold)
                CurX = CurX + strWidth
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1060" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1185" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1187" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1257" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1258" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1296" Then '---- Selvanayaki Textiles (Karumanthapatti)
                    Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_PanCap, CurX, CurY, 0, PrintWidth, p1Font, Brushes.Green)
                ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1213" Then
                    Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_PanCap, CurX, CurY + 5, 0, PrintWidth, p1Font, br)

                Else
                    Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_PanCap, CurX, CurY, 0, PrintWidth, p1Font)
                End If
                strWidth = e.Graphics.MeasureString("     " & Cmp_PanCap, p1Font).Width
                CurX = CurX + strWidth
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1060" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1185" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1187" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1257" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1258" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1296" Then '---- Selvanayaki Textiles (Karumanthapatti)
                    Common_Procedures.Print_To_PrintDocument(e, Cmp_PanNo, CurX, CurY, 0, 0, pFont, Brushes.Green)
                ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1213" Then
                    Common_Procedures.Print_To_PrintDocument(e, Cmp_PanNo, CurX, CurY + 5, 0, 0, p1Font, br)

                Else
                    Common_Procedures.Print_To_PrintDocument(e, Cmp_PanNo, CurX, CurY, 0, 0, pFont)
                End If

                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1249" Then '---- Vaipav Textile

                    If Trim(prn_HdDt.Rows(0).Item("Company_CinNo").ToString()) <> "" Then
                        CurY = CurY + TxtHgt
                        p1Font = New Font(vFontName, 10, FontStyle.Bold)
                        Common_Procedures.Print_To_PrintDocument(e, " CIN : " & prn_HdDt.Rows(0).Item("Company_CinNo").ToString(), LMargin, CurY, 2, PrintWidth, p1Font)
                    End If

                End If
            End If

        End If

        CurY = CurY + TxtHgt - 10
        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try

            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 30
            W1 = e.Graphics.MeasureString("DATE & TIME OF   SUPPLY ", pFont).Width
            S1 = e.Graphics.MeasureString("TO", pFont).Width  ' e.Graphics.MeasureString("Details of Receiver | Billed to     :", pFont).Width

            W2 = e.Graphics.MeasureString("DESPATCH   TO   : ", pFont).Width
            S2 = e.Graphics.MeasureString("TRANSPORTATION   MODE", pFont).Width

            W3 = e.Graphics.MeasureString("INVOICE   DATE", pFont).Width
            S3 = e.Graphics.MeasureString("REVERSE CHARGE   (YES/NO) ", pFont).Width

            CurY = CurY + 10
            p1Font = New Font(vFontName, 12, FontStyle.Bold)
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1151" Then '---- BHARATHI TEXTILE (TIRUPUR)
                Common_Procedures.Print_To_PrintDocument(e, "INVOICE NO", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W3 + 10, CurY, 0, 0, pFont)
            End If

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1155" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1204" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1277" Then '---- K.R.G Textiles (Somanur)

                'Inv_No = prn_HdDt.Rows(0).Item("ClothSales_Invoice_RefNo").ToString
                'InvSubNo = Replace(Trim(Inv_No), Trim(Val(Inv_No)), "")

                'If prn_HdDt.Rows(0).Item("Invoice_PrefixNo").ToString <> "" Then
                '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Invoice_PrefixNo").ToString & "-" & Trim(Format(Val(Inv_No), "######000")) & Trim(InvSubNo) & "/" & Trim(Year(Common_Procedures.Company_FromDate)) & "-" & Trim(Microsoft.VisualBasic.Right(Year(Common_Procedures.Company_ToDate), 2)), LMargin + W3 + 30, CurY, 0, 0, p1Font)
                'Else
                '    Common_Procedures.Print_To_PrintDocument(e, Trim(Format(Val(Inv_No), "######000")) & Trim(InvSubNo) & "/" & Trim(Year(Common_Procedures.Company_FromDate)) & "-" & Trim(Microsoft.VisualBasic.Right(Year(Common_Procedures.Company_ToDate), 2)), LMargin + W3 + 30, CurY, 0, 0, p1Font)
                'End If

                'ElseIf Val(prn_HdDt.Rows(0).Item("ClothSales_InvoiceNo_LeadingZeros_Status").ToString) = 1 Then

                '    Inv_No = prn_HdDt.Rows(0).Item("ClothSales_Invoice_RefNo").ToString
                '    InvSubNo = Replace(Trim(Inv_No), Trim(Val(Inv_No)), "")

                '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Invoice_PrefixNo").ToString & Trim(Format(Val(Inv_No), "######0000")) & Trim(InvSubNo) & prn_HdDt.Rows(0).Item("Invoice_SuffixNo").ToString, LMargin + W3 + 30, CurY, 0, 0, p1Font)

            Else

                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1151" Then '---- BHARATHI TEXTILE (TIRUPUR)
                    W3 = e.Graphics.MeasureString("REVERSE CHARGE (YES/NO) ", pFont).Width
                    Common_Procedures.Print_To_PrintDocument(e, "REVERSE CHARGE (YES/NO)", LMargin + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W3 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin + W3 + 30, CurY, 0, 0, pFont)

                Else
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("JobWork_ConversionBill_no").ToString, LMargin + W3 + 30, CurY, 0, 0, p1Font)

                End If

            End If

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1151" Then '---- BHARATHI TEXTILE (TIRUPUR)
                W3 = e.Graphics.MeasureString("INVOICE       DATE", pFont).Width

                Common_Procedures.Print_To_PrintDocument(e, "INVOICE NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S3, CurY, 0, 0, pFont)

                p1Font = New Font(vFontName, 13, FontStyle.Bold)
                If prn_HdDt.Rows(0).Item("JobWork_ConversionBill_PrefixNo").ToString <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("JobWork_ConversionBill_PrefixNo").ToString & "-" & prn_HdDt.Rows(0).Item("JobWork_ConversionBill_No").ToString, LMargin + C1 + S3 + 10, CurY, 0, 0, p1Font)
                Else
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("JobWork_ConversionBill_No").ToString, LMargin + C1 + S3 + 10, CurY, 0, 0, p1Font)
                End If

                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "INVOICE DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S3, CurY, 0, 0, pFont)
                p1Font = New Font(vFontName, 13, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("JobWork_ConversionBill_No").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + S3 + 10, CurY, 0, 0, p1Font)

                If Trim(prn_HdDt.Rows(0).Item("FormJJ_No").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, "E-WAY BILL.NO ", LMargin + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W3 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("FormJJ_No").ToString, LMargin + W3 + 30, CurY, 0, 0, pFont)
                End If

            Else

                Common_Procedures.Print_To_PrintDocument(e, "REVERSE CHARGE (YES/NO)", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S3 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin + C1 + S3 + 30, CurY, 0, 0, pFont)


                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "INVOICE DATE", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W3 + 10, CurY, 0, 0, pFont)
                p1Font = New Font(vFontName, 11, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("JobWork_ConversionBill_Date").ToString), "dd-MM-yyyy").ToString, LMargin + W3 + 30, CurY, 0, 0, p1Font)

                If Trim(prn_HdDt.Rows(0).Item("FormJJ_No").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, "E-WAY BILL NO. ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S3 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("FormJJ_No").ToString, LMargin + C1 + S3 + 30, CurY, 0, 0, pFont)
                End If
            End If

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
            'LnAr(2) = CurY

            CurY1 = CurY
            CurY2 = CurY

            '---left side

            CurY1 = CurY1 + 10
            p1Font = New Font(vFontName, 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "DETAILS OF RECEIVER (BILLED TO) :", LMargin + 10, CurY1, 0, 0, p1Font)

            strHeight = e.Graphics.MeasureString("A", p1Font).Height
            CurY1 = CurY1 + strHeight
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1211" Then
                p1Font = New Font(vFontName, 12, FontStyle.Bold)
            Else
                p1Font = New Font(vFontName, 11, FontStyle.Bold)
            End If
            Common_Procedures.Print_To_PrintDocument(e, "M/s. " & prn_HdDt.Rows(0).Item("Ledger_mainName").ToString, LMargin + S1 + 10, CurY1, 0, 0, p1Font)

            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY1, 0, 0, pFont)

            'If Trim(prn_HdDt.Rows(0).Item("Ledger_Address2").ToString) <> "" Then
            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY1, 0, 0, pFont)
            'End If

            'If Trim(prn_HdDt.Rows(0).Item("Ledger_Address3").ToString) <> "" Then
            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY1, 0, 0, pFont)
            'End If

            'If Trim(prn_HdDt.Rows(0).Item("Ledger_Address4").ToString) <> "" Then
            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY1, 0, 0, pFont)
            'End If


            '--Right Side

            CurY2 = CurY2 + 10
            p1Font = New Font(vFontName, 10, FontStyle.Bold)
            If (prn_HdDt.Rows(0).Item("Ledger_mainName").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "DETAILS OF CONSIGNEE (SHIPPED TO) :", LMargin + C1 + 10, CurY2, 0, 0, p1Font)

                strHeight = e.Graphics.MeasureString("A", p1Font).Height
                CurY2 = CurY2 + strHeight
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1211" Then '---SREE SAMY TEXTILES
                    p1Font = New Font(vFontName, 12, FontStyle.Bold)
                Else
                    p1Font = New Font(vFontName, 11, FontStyle.Bold)
                End If
                Common_Procedures.Print_To_PrintDocument(e, "M/s. " & prn_HdDt.Rows(0).Item("Ledger_mainName").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, p1Font)

            End If
            CurY2 = CurY2 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, pFont)

            If Trim(prn_HdDt.Rows(0).Item("Ledger_Address2").ToString) <> "" Then
                CurY2 = CurY2 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, pFont)
            End If

            If Trim(prn_HdDt.Rows(0).Item("Ledger_Address3").ToString) <> "" Then
                CurY2 = CurY2 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, pFont)
            End If

            If Trim(prn_HdDt.Rows(0).Item("Ledger_Address4").ToString) <> "" Then
                CurY2 = CurY2 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, pFont)
            End If

            CurY1 = IIf(CurY1 > CurY2, CurY1, CurY2)
            CurY1 = CurY1 + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY1, PageWidth, CurY1)



            CurY1 = CurY1 + TxtHgt - 15
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1211" Then '---SREE SAMY TEXTILES
                p1Font = New Font(vFontName, 12, FontStyle.Bold)
            Else
                p1Font = New Font(vFontName, 10, FontStyle.Bold)
            End If
            If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "GSTIN : " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + S1 + 10, CurY1, 0, 0, p1Font)
            End If
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1044" Then
                If Trim(prn_HdDt.Rows(0).Item("Pan_No").ToString) <> "" Then
                    strWidth = e.Graphics.MeasureString("GSTIN : " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, p1Font).Width
                    CurX = LMargin + S1 + 10 + strWidth
                    Common_Procedures.Print_To_PrintDocument(e, "      PAN : " & prn_HdDt.Rows(0).Item("Pan_No").ToString, CurX, CurY1, 0, PrintWidth, p1Font)
                End If
            End If

            If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "GSTIN : " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + C1 + S1 + 10, CurY1, 0, 0, p1Font)
            End If
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1044" Then
                If Trim(prn_HdDt.Rows(0).Item("Pan_No").ToString) <> "" Then
                    strWidth = e.Graphics.MeasureString("GSTIN : " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, p1Font).Width
                    CurX = LMargin + C1 + S1 + 10 + strWidth
                    Common_Procedures.Print_To_PrintDocument(e, "      PAN : " & prn_HdDt.Rows(0).Item("Pan_No").ToString, CurX, CurY1, 0, PrintWidth, p1Font)
                End If
            End If

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1140" Then
                CurY1 = CurY1 + TxtHgt
                e.Graphics.DrawLine(Pens.Black, LMargin, CurY1, PageWidth, CurY1)
            End If
            LnAr(10) = CurY1

            CurY1 = CurY1 + TxtHgt - 15
            If Trim(prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString) <> "" Then
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1140" Then
                    Common_Procedures.Print_To_PrintDocument(e, "STATE : " & prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString, LMargin + S1 + 10, CurY1, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, "CODE : " & prn_HdDt.Rows(0).Item("Ledger_State_Code").ToString, LMargin + C1 - 100, CurY1, 0, 0, pFont)
                Else
                    Common_Procedures.Print_To_PrintDocument(e, "STATE CODE : " & prn_HdDt.Rows(0).Item("Ledger_State_Code").ToString, LMargin + C1 - 100, CurY1, 0, 0, pFont)
                End If
            End If
            If Trim(prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString) <> "" Then
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1140" Then
                    Common_Procedures.Print_To_PrintDocument(e, "STATE : " & prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString, LMargin + C1 + S1 + 10, CurY1, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, " CODE : " & prn_HdDt.Rows(0).Item("Ledger_State_Code").ToString, PageWidth - 100, CurY1, 0, 0, pFont)
                Else
                    Common_Procedures.Print_To_PrintDocument(e, " STATE CODE : " & prn_HdDt.Rows(0).Item("Ledger_State_Code").ToString, PageWidth - 100, CurY1, 0, 0, pFont)
                End If

            End If



            CurY = IIf(CurY1 > CurY2, CurY1, CurY2)


            CurY = CurY + TxtHgt

            e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(vLine_Pen, LMargin + C1, CurY, LMargin + C1, LnAr(2))
            LnAr(3) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + C1 - 110, LnAr(10), LMargin + C1 - 110, LnAr(3))
            e.Graphics.DrawLine(Pens.Black, PageWidth - 110, LnAr(10), PageWidth - 110, LnAr(3))


            W2 = e.Graphics.MeasureString("DOCUMENT THROUGH   : ", pFont).Width
            S2 = e.Graphics.MeasureString("DATE & TIME OF SUPPLY  :", pFont).Width

            '--Right Side
            CurY = CurY + 10
            'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1316" Then '---- NITHYABHARATH TEXTILE (P) LTD (PALLADAM)
            '    If Val(prn_OriDupTri_Count) <> 2 Then
            '        Common_Procedures.Print_To_PrintDocument(e, "AGENT NAME ", LMargin + 10, CurY, 0, 0, pFont)
            '        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 10, CurY, 0, 0, pFont)
            '        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Agent_Name").ToString, LMargin + W2 + 30, CurY, 0, 0, pFont)
            '    End If
            'Else
            '    'Common_Procedures.Print_To_PrintDocument(e, "AGENT NAME ", LMargin + 10, CurY, 0, 0, pFont)
            '    'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 10, CurY, 0, 0, pFont)
            '    'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Agent_Name").ToString, LMargin + W2 + 30, CurY, 0, 0, pFont)
            'End If

            Common_Procedures.Print_To_PrintDocument(e, "DELIVERY MODE ", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "TRANSPORT", LMargin + W2 + 30, CurY, 0, 0, pFont)



            Common_Procedures.Print_To_PrintDocument(e, "TRANSPORTATION MODE", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "ROAD", LMargin + C1 + S2 + 30, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt
            'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1140" Then
            '    Common_Procedures.Print_To_PrintDocument(e, "NO OF BALES", LMargin + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Bales").ToString), "########0"), LMargin + W2 + 30, CurY, 0, 0, pFont)

            'Else

            '    Common_Procedures.Print_To_PrintDocument(e, "ORDER NO", LMargin + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Party_OrderNo").ToString, LMargin + W2 + 30, CurY, 0, 0, pFont)
            '    If Trim(prn_HdDt.Rows(0).Item("Party_OrderDate").ToString) <> "" Then
            '        strWidth = e.Graphics.MeasureString(prn_HdDt.Rows(0).Item("Party_OrderNo").ToString, pFont).Width
            '        Common_Procedures.Print_To_PrintDocument(e, "Date : " & prn_HdDt.Rows(0).Item("Party_OrderDate").ToString, LMargin + W2 + 30 + strWidth + 15, CurY, 0, 0, pFont)
            '    End If
            'End If



            Common_Procedures.Print_To_PrintDocument(e, "VEHICLE NO", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vehicle_No").ToString, LMargin + W2 + 30, CurY, 0, 0, pFont)

            Dim vprn_BlNos As String = ""
            CurY = CurY + TxtHgt
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1140" Then

                vprn_BlNos = ""
                For I = 0 To prn_DetDt.Rows.Count - 1
                    If Trim(prn_DetDt.Rows(I).Item("Bales_Nos").ToString) <> "" Then
                        vprn_BlNos = Trim(vprn_BlNos) & IIf(Trim(vprn_BlNos) <> "", ", ", "") & prn_DetDt.Rows(I).Item("Bales_Nos").ToString
                    End If
                Next
                Common_Procedures.Print_To_PrintDocument(e, "BALE NOS", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, vprn_BlNos, LMargin + W2 + 30, CurY, 0, 0, pFont)

            Else
                'Common_Procedures.Print_To_PrintDocument(e, "DC NO", LMargin + 10, CurY, 0, 0, pFont)
                'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 10, CurY, 0, 0, pFont)
                'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Dc_No").ToString, LMargin + W2 + 30, CurY, 0, 0, pFont)
                'If Trim(prn_HdDt.Rows(0).Item("Dc_Date").ToString) <> "" Then
                '    strWidth = e.Graphics.MeasureString(prn_HdDt.Rows(0).Item("Dc_No").ToString, pFont).Width
                '    Common_Procedures.Print_To_PrintDocument(e, "Date : " & prn_HdDt.Rows(0).Item("Dc_Date").ToString, LMargin + W2 + 30 + strWidth + 15, CurY, 0, 0, pFont)
                'End If
            End If


            Common_Procedures.Print_To_PrintDocument(e, "TRANSPORT NAME", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("TransportName").ToString, LMargin + C1 + S2 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt

            'Common_Procedures.Print_To_PrintDocument(e, "LR NO.", LMargin + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Lr_No").ToString, LMargin + W2 + 30, CurY, 0, 0, pFont)
            'If Trim(prn_HdDt.Rows(0).Item("Lr_Date").ToString) <> "" Then
            '    strWidth = e.Graphics.MeasureString(prn_HdDt.Rows(0).Item("Lr_No").ToString, pFont).Width
            '    Common_Procedures.Print_To_PrintDocument(e, "Date : " & prn_HdDt.Rows(0).Item("Lr_Date").ToString, LMargin + W2 + 30 + strWidth + 15, CurY, 0, 0, pFont)
            'End If

            'Common_Procedures.Print_To_PrintDocument(e, "DATE & TIME OF SUPPLY", LMargin + C1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S2 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Date_And_Time_Of_Supply").ToString, LMargin + C1 + S2 + 30, CurY, 0, 0, pFont)

            'CurY = CurY + TxtHgt
            'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1140" Then
            '    Common_Procedures.Print_To_PrintDocument(e, "BALE/BUNDLE WEIGHT", LMargin + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Bale_Weight").ToString, LMargin + W2 + 30, CurY, 0, 0, pFont)
            'Else
            '    Common_Procedures.Print_To_PrintDocument(e, "DOCUMENT THROUGH", LMargin + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Through_Name").ToString, LMargin + W2 + 30, CurY, 0, 0, pFont)
            'End If


            'Common_Procedures.Print_To_PrintDocument(e, "PLACE OF SUPPLY", LMargin + C1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S2 + 10, CurY, 0, 0, pFont)

            'If Trim(prn_HdDt.Rows(0).Item("Despatch_To").ToString) <> "" Then
            '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Despatch_To").ToString, LMargin + C1 + S2 + 30, CurY, 0, 0, pFont)
            'Else
            '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Company_State_Name").ToString, LMargin + C1 + S2 + 30, CurY, 0, 0, pFont)
            'End If


            'If Trim(prn_HdDt.Rows(0).Item("Lc_No").ToString) <> "" Then
            '    CurY = CurY + TxtHgt
            '    Common_Procedures.Print_To_PrintDocument(e, "LC NO", LMargin + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Lc_No").ToString, LMargin + W2 + 30, CurY, 0, 0, pFont)
            '    If Trim(prn_HdDt.Rows(0).Item("Lc_Date").ToString) <> "" Then
            '        strWidth = e.Graphics.MeasureString(prn_HdDt.Rows(0).Item("Lc_No").ToString, pFont).Width
            '        Common_Procedures.Print_To_PrintDocument(e, "Date : " & prn_HdDt.Rows(0).Item("Lc_Date").ToString, LMargin + W2 + 150, CurY, 0, 0, pFont)
            '    End If
            'End If

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(vLine_Pen, LMargin + C1, CurY, LMargin + C1, LnAr(3))
            LnAr(4) = CurY

            '---Table Headings
            CurY = CurY + 10
            pFont = New Font("Calibri", 10, FontStyle.Bold)

            CurY = CurY + TxtHgt - 10

            Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)
            If Val(ClAr(2)) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "DC.NO", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            End If
            Common_Procedures.Print_To_PrintDocument(e, "DESCRIPTION OF GOODS", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "HSN/SAC", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "GST %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "METER", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "NO.OF ROLLS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "RATE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)
            CurY = CurY + 10
            Common_Procedures.Print_To_PrintDocument(e, "/BALES", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)

            CurY = CurY + TxtHgt + 15
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1044" Then
                CurY = CurY + 10
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1229" Then '----Star Fabric Mills (Thekkalur)
                    p1Font = New Font(vFontName, 7, FontStyle.Bold)
                Else
                    p1Font = New Font(vFontName, 8, FontStyle.Bold)
                End If
                'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1316" Then '---- NITHYABHARATH TEXTILE (P) LTD (PALLADAM)
                '    If Val(prn_OriDupTri_Count) <> 2 Then
                '        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Cloth_Details").ToString, LMargin + ClAr(1) + 5, CurY, 0, 0, p1Font)
                '    End If
                'Else
                '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Cloth_Details").ToString, LMargin + ClAr(1) + 5, CurY, 0, 0, p1Font)
                'End If

            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub Printing_Format3_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
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
        Dim CurY1 As Single = 0

        ' W1 = e.Graphics.MeasureString("No.of Beams  : ", pFont).Width

        Try

            For I = NoofDets + 1 To NoofItems_PerPage - 1

                CurY = CurY + TxtHgt

                prn_DetIndx = prn_DetIndx + 1

            Next

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

            pFont = New Font("Calibri", 10, FontStyle.Regular)
            CurY = CurY + TxtHgt - 10
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(Format(Val(prn_TotMtrs), "########0.00")), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Rate_ClothType1").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)

                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Gross_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
                prn_TotMtrs = 0
            End If

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
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(4))
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

            CurY1 = CurY
            If is_LastPage = True Then
                'Left Side
                CurY1 = CurY1 + TxtHgt
                p1Font = New Font("Calibri", 11, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, BankNm1, LMargin + 10, CurY1, 0, 0, p1Font)
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, BankNm2, LMargin + 10, CurY1, 0, 0, p1Font)
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, BankNm3, LMargin + 10, CurY1, 0, 0, p1Font)
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, BankNm4, LMargin + 10, CurY1, 0, 0, p1Font)

            End If


            'Right Side
            CurY = CurY + TxtHgt - 10
            If is_LastPage = True Then
                If Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Freight_Caption_Name").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Freight_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
                End If
            End If

            CurY = CurY + TxtHgt
            If is_LastPage = True Then
                If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then
                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1238" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1239" Then '---- KALAIMAGAL TEX
                        Common_Procedures.Print_To_PrintDocument(e, "Trade Discount", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                    Else
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("AddLess_Caption_Name").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                    End If
                    Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
                End If

                If Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then
                    CurY = CurY + TxtHgt + 5
                    e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, PageWidth, CurY)
                End If
            End If


            CurY = CurY - 10

            CurY = CurY + TxtHgt
            If is_LastPage = True Then
                If Val(prn_HdDt.Rows(0).Item("Total_Taxable_Amount").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, "Taxable Value", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("Total_Taxable_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
                End If
            End If
            CurY = CurY + TxtHgt
            If is_LastPage = True Then
                If Val(prn_HdDt.Rows(0).Item("CGST_Amount").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, "CGST " & Val(prn_HdDt.Rows(0).Item("CGST_Percentage").ToString) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("CGST_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
                End If
            End If
            CurY = CurY + TxtHgt
            If is_LastPage = True Then
                If Val(prn_HdDt.Rows(0).Item("SGST_Amount").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, "SGST " & Val(prn_HdDt.Rows(0).Item("SGST_Percentage").ToString) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("SGST_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
                End If
            End If
            CurY = CurY + TxtHgt
            If is_LastPage = True Then
                If Val(prn_HdDt.Rows(0).Item("IGST_Amount").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, "IGST " & Val(prn_HdDt.Rows(0).Item("IGST_Percentage").ToString) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("IGST_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
                End If
            End If





            CurY = CurY + TxtHgt
            If is_LastPage = True Then
                p1Font = New Font("Calibri", 13, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "(Textile manufactring service (Weaving) )", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 20, CurY, 1, 0, p1Font)
            End If

            If is_LastPage = True Then
                If Val(prn_HdDt.Rows(0).Item("RoundOff_Amount").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, "RoundOff", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("RoundOff_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
                End If

            End If


            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, PageWidth, CurY)
            LnAr(8) = CurY
            CurY = CurY + TxtHgt - 10
            p1Font = New Font("Calibri", 13, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "E & OE", LMargin + 10, CurY, 0, 0, p1Font)

            If is_LastPage = True Then
                pFont = New Font("Calibri", 12, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "NET AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                p1Font = New Font("Calibri", 12, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, " " & Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, p1Font)
            End If

            pFont = New Font("Calibri", 10, FontStyle.Regular)
            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(7))
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(9) = CurY
            ' e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))

            CurY = CurY + TxtHgt - 5

            If is_LastPage = True Then
                BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))

                p1Font = New Font("Calibri", 12, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "Rupees  : " & BmsInWrds & " ", LMargin + 10, CurY, 0, 0, p1Font)
            End If

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            CurY = CurY + 5
            p1Font = New Font("Calibri", 12, FontStyle.Underline)
            Common_Procedures.Print_To_PrintDocument(e, "Term & Conditions : ", LMargin + 10, CurY, 0, 0, p1Font)



            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1162" Then
                CurY = CurY + TxtHgt + 6
                Common_Procedures.Print_To_PrintDocument(e, "Goods Once Sold Cannot Be Taken Back ", LMargin + 10, CurY, 0, 0, pFont)
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Overdue Interest will be charged at 24% from The invoice date ", LMargin + 10, CurY, 0, 0, pFont)
            Else
                CurY = CurY + TxtHgt + 10
                Common_Procedures.Print_To_PrintDocument(e, "Overdue Interest will be charged at 24% from The invoice date ", LMargin + 10, CurY, 0, 0, pFont)
            End If


            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "We are not responsible for any loss or damage in transit", LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "We will not accept any claim after processing of goods", LMargin + 10, CurY, 0, 0, pFont)

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1162" Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Any Claim or Damage Can Be Accepted If Intimated Within 7 Working days From Date Of Delivery", LMargin + 10, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            If Trim(Common_Procedures.settings.CustomerCode) = "1162" Then
                Common_Procedures.Print_To_PrintDocument(e, "Subject to " & Trim(Common_Procedures.settings.Jurisdiction) & " jurisdiction ", LMargin + 10, CurY, 0, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "Subject to Coimbatore jurisdiction ", LMargin + 10, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(10) = CurY



            CurY = CurY + 5

            p1Font = New Font("Calibri", 7, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Certified that the Particulars given above are true and correct and the amount indicated represents the price actually charged and that there is no flow additional consideration", PageWidth - 10, CurY, 1, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "directly or indirectly from the buyer", LMargin + 20, CurY + 10, 0, 0, p1Font)


            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)


            LnAr(10) = CurY



            'If Val(Common_Procedures.User.IdNo) <> 1 Then
            '    Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
            'End If

            CurY = CurY + TxtHgt - 5
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1152" Then
                p1Font = New Font("Elephant", 12, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)
            Else
                p1Font = New Font("Calibri", 12, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)
            End If

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1162" Then
                CurY = CurY + TxtHgt
                e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.vijay_tex_Sign2, Drawing.Image), PageWidth - 110, CurY, 90, 55)
            End If

            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            '        CurY = CurY + TxtHgt

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

    Private Sub btn_UserModification_Click(sender As Object, e As System.EventArgs) Handles btn_UserModification.Click
        If Val(Common_Procedures.User.IdNo) = 1 Then
            Dim f1 As New User_Modifications
            f1.Entry_Name = Me.Name
            f1.Entry_PkValue = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_BillNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            f1.ShowDialog()
        End If
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

    Private Sub txt_IR_No_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_IR_No.KeyDown
        If e.KeyValue = 38 Or (e.Control = True And e.KeyValue = 38) Then
            txt_FormJJNo.Focus()
        End If
        If (e.KeyValue = 40 And cbo_VehicleNo.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If Trim(UCase(cbo_Type.Text)) = "DELIVERY" Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCol_Details.Seconds_Rate)
            Else
                If dgv_Details.RowCount > 0 Then
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCol_Details.DC_No)
                Else
                    txt_Rate_Type1.Focus()
                End If
            End If
        End If
    End Sub

    Private Sub txt_IR_No_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_IR_No.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If Trim(UCase(cbo_Type.Text)) = "DELIVERY" Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCol_Details.Seconds_Rate)
            Else
                If dgv_Details.RowCount > 0 Then
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCol_Details.DC_No)
                Else
                    txt_Rate_Type1.Focus()
                End If
            End If
        End If
    End Sub

    Private Sub btn_EInvoice_Generation_Click(sender As Object, e As EventArgs) Handles btn_EInvoice_Generation.Click
        'rtbeInvoiceResponse.Text = ""
        'txt_EWBNo.Text = txt_Electronic_RefNo.Text
        grp_EInvoice.Visible = True
        grp_EInvoice.Left = (Me.Width - grp_EInvoice.Width) / 2
        grp_EInvoice.Top = (Me.Height - grp_EInvoice.Height) / 2
    End Sub
    Private Sub btn_CheckConnectivity1_Click(sender As Object, e As EventArgs) Handles btn_CheckConnectivity1.Click

        Dim einv As New eInvoice(Val(lbl_Company.Tag))
        einv.GetAuthToken(rtbeInvoiceResponse)
        'rtbeInvoiceResponse.Text = einv.AuthTokenReturnMsg
    End Sub

    Private Sub btn_Generate_eInvoice_Click(sender As Object, e As EventArgs) Handles btn_Generate_eInvoice.Click

        'Dim NewCode As String = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_BillNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Dim NewCode As String = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_BillNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Dim vDescription As String = ""
        Dim Cmd As New SqlClient.SqlCommand
        Cmd.Connection = con
        Cmd.CommandText = "Select count(*) from JobWork_ConversionBill_Head Where JobWork_ConversionBill_Code = '" & Trim(NewCode) & "'"

        Dim c As Int16 = Cmd.ExecuteScalar

        If c <= 0 Then
            MsgBox("Please Save the Invoice Before Generating IRN ", vbOKOnly, "Save")
            Exit Sub
        End If

        Cmd.CommandText = "Select count(*) from JobWork_ConversionBill_Head Where JobWork_ConversionBill_Code = '" & Trim(NewCode) & "' and Len(E_Invoice_IRNO) > 0"
        c = Cmd.ExecuteScalar

        If c > 0 Then
            Dim k As Integer = MsgBox("An IRN Has been Generated already for this Invoice. Do you want to Delete the Previous IRN ?", vbYesNo, "IRN Generated")
            If k = vbNo Then
                MsgBox("Cannot Create a New IRN When there is an IRN generated already !", vbOKOnly, "Duplicate IRN ")
                Exit Sub
            Else

            End If
        End If

        Dim tr As SqlClient.SqlTransaction

        tr = con.BeginTransaction
        Cmd.Transaction = tr

        Try


            Cmd.CommandText = "Delete from e_Invoice_Head  where Ref_Sales_Code = '" & Trim(NewCode) & "'"
            Cmd.ExecuteNonQuery()

            Cmd.CommandText = "Delete from e_Invoice_Details  where Ref_Sales_Code = '" & Trim(NewCode) & "'"
            Cmd.ExecuteNonQuery()

            Cmd.CommandText = "Insert into e_Invoice_Head Select JobWork_ConversionBill_No, JobWork_ConversionBill_Date, Ledger_IdNo, Ledger_IdNo, " &
                              " Total_Taxable_Amount, CGST_Amount, SGST_Amount, IGST_Amount , " &
                              " 0, 0, (RoundOff_Amount), Net_Amount, '" & Trim(NewCode) & "', 0 , 0  from JobWork_ConversionBill_Head where JobWork_ConversionBill_Code = '" & Trim(NewCode) & "'"
            Cmd.ExecuteNonQuery()

            Cmd.CommandText = "truncate table " & Trim(Common_Procedures.EntryTempTable) & ""
            Cmd.ExecuteNonQuery()


            vDescription = "(Textile manufactring service (Weaving) )"

            Cmd.CommandText = "Insert into e_Invoice_Details  " &
                               " Select a.Sl_No, 0 as IsServc, '" & Trim(vDescription) & "' as producDescription  , '998821' as HSN_Code , '' as batchdetails, a.Total_Meters, 'MTR' as UOM, 0 as Rate , (a.Total_Amount + (CASE WHEN a.sl_no = 1 then (b.Freight_Amount + b.AddLess_Amount) else 0 end ) ), 0 as DiscountAmount, " &
                               " (a.Total_Amount + (CASE WHEN a.sl_no = 1 then (b.Freight_Amount + b.AddLess_Amount) else 0 end ) ), (CASE WHEN b.IGST_Amount <> 0 then b.IGST_Percentage else ( b.SGST_Percentage + b.CGST_Percentage ) end ) as GST_Percentage , 0 AS SgstAmt, 0 AS CgstAmt, 0 AS igstAmt, 0 AS CesRt, 0 AS CesAmt, 0 AS CesNonAdvlAmt, 0 AS StateCesRt, 0 AS StateCesAmt, 0 as StateCesNonAdvlAmt, " &
                               " 0 as OthChrg, 0 as TotItemVal, '' as AttributesDetails, '" & Trim(NewCode) & "' " &
                               " from JobWork_ConversionBill_Details a INNER JOIN JobWork_ConversionBill_Head b  ON a.JobWork_ConversionBill_Code =  b.JobWork_ConversionBill_Code" &
                               " inner join Cloth_head C on a.Cloth_IdNo = c.Cloth_IdNo " &
                               " Where a.JobWork_ConversionBill_Code = '" & Trim(NewCode) & "'"
            Cmd.ExecuteNonQuery()

            'Cmd.CommandText = "Insert into e_Invoice_Details  " &
            '                   " Select a.Sl_No, 0 as IsServc, '" & Trim(vDescription) & "' as producDescription  , '998821' as HSN_Code , '' as batchdetails, a.Total_Meters, 'MTR' as UOM, 0 as Rate , (b.Gross_Amount + (CASE WHEN a.sl_no = 1 then (b.Freight_Amount + b.AddLess_Amount) else 0 end ) ), 0 as DiscountAmount, " &
            '                   " b.Total_Taxable_Amount, (CASE WHEN b.IGST_Amount <> 0 then b.IGST_Percentage else ( b.SGST_Percentage + b.CGST_Percentage ) end ) as GST_Percentage , 0 AS SgstAmt, 0 AS CgstAmt, 0 AS igstAmt, 0 AS CesRt, 0 AS CesAmt, 0 AS CesNonAdvlAmt, 0 AS StateCesRt, 0 AS StateCesAmt, 0 as StateCesNonAdvlAmt, " &
            '                   " 0 as OthChrg, 0 as TotItemVal, '' as AttributesDetails, '" & Trim(NewCode) & "' " &
            '                   " from JobWork_ConversionBill_Details a INNER JOIN JobWork_ConversionBill_Head b  ON a.JobWork_ConversionBill_Code =  b.JobWork_ConversionBill_Code" &
            '                   " inner join Cloth_head C on a.Cloth_IdNo = c.Cloth_IdNo " &
            '                   " Where a.JobWork_ConversionBill_Code = '" & Trim(NewCode) & "'"
            'Cmd.ExecuteNonQuery()


            'Cmd.CommandText = "Insert into e_Invoice_Details  " &
            '                   " Select 1 as Sl_No, 1 as IsServc, '" & Trim(vDescription) & "' as producDescription  , '998821' as HSN_Code , '' as batchdetails, b.Total_ClothType1_Meters, 'MTR' as UOM, b.Rate_ClothType1 as Rate , (b.Amount_ClothType1 + b.Freight_Amount + b.AddLess_Amount ), 0 as DiscountAmount, " &
            '                   " (b.Amount_ClothType1 + b.Freight_Amount + b.AddLess_Amount ), (CASE WHEN b.IGST_Amount <> 0 then b.IGST_Percentage else ( b.SGST_Percentage + b.CGST_Percentage ) end ) as GST_Percentage , 0 AS SgstAmt, 0 AS CgstAmt, 0 AS igstAmt, 0 AS CesRt, 0 AS CesAmt, 0 AS CesNonAdvlAmt, 0 AS StateCesRt, 0 AS StateCesAmt, 0 as StateCesNonAdvlAmt, " &
            '                   " 0 as OthChrg, 0 as TotItemVal, '' as AttributesDetails, '" & Trim(NewCode) & "' " &
            '                   " from JobWork_ConversionBill_Head b Where b.JobWork_ConversionBill_Code = '" & Trim(NewCode) & "' and b.Total_ClothType1_Meters > 0 and (b.Amount_ClothType1 + b.Freight_Amount + b.AddLess_Amount ) > 0"

            'Cmd.ExecuteNonQuery()

            tr.Commit()

            'Exit Sub

            'rtbeInvoiceResponse.Text = einv.AuthTokenReturnMsg

        Catch ex As Exception

            tr.Rollback()
            MsgBox(ex.Message + " Cannot Generate IRN.", vbOKOnly, "Error !")

            Exit Sub

        End Try

        Dim einv As New eInvoice(Val(lbl_Company.Tag))
        einv.GenerateIRN(Val(lbl_Company.Tag), NewCode, con, rtbeInvoiceResponse, pic_IRN_QRCode_Image, txt_eInvoiceNo, txt_eInvoiceAckNo, txt_eInvoiceAckDate, txt_eInvoice_CancelStatus, "JobWork_ConversionBill_Head", "JobWork_ConversionBill_Code", Pk_Condition)

    End Sub

    Private Sub btn_Close_eInvoice_Click(sender As Object, e As EventArgs) Handles btn_Close_eInvoice.Click
        grp_EInvoice.Visible = False
    End Sub

    Private Sub btn_Delete_eInvoice_Click(sender As Object, e As EventArgs) Handles btn_Delete_eInvoice.Click

        Dim NewCode As String = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_BillNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        Dim einv As New eInvoice(Val(lbl_Company.Tag))
        einv.CancelIRNByIRN(txt_eInvoiceNo.Text, rtbeInvoiceResponse, "JobWork_ConversionBill_Head", "JobWork_ConversionBill_Code", con, txt_eInvoice_CancelStatus, NewCode, "DataEntry Mistake")

    End Sub
    Private Sub btn_Refresh_eInvoice_Info_Click(sender As Object, e As EventArgs)

        Threading.Thread.Sleep(10000)

        Dim NewCode As String = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_BillNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        Dim einv As New eInvoice(Val(lbl_Company.Tag))
        einv.RefresheInvoiceInfoByIRN(txt_eInvoiceNo.Text, Val(lbl_Company.Tag), NewCode, con, rtbeInvoiceResponse, pic_IRN_QRCode_Image, txt_eInvoiceNo, txt_eInvoiceAckNo, txt_eInvoiceAckDate, txt_eInvoice_CancelStatus, "JobWork_ConversionBill_Head", "JobWork_ConversionBill_Code")

    End Sub
    Private Sub txt_eInvoiceNo_TextChanged(sender As Object, e As EventArgs) Handles txt_eInvoiceNo.TextChanged
        txt_IR_No.Text = txt_eInvoiceNo.Text
    End Sub

    Private Sub dgtxt_Details_TextChanged(sender As Object, e As EventArgs) Handles dgtxt_Details.TextChanged
        Try
            With dgv_Details
                If .Rows.Count <> 0 Then
                    .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(dgtxt_Details.Text)
                End If
            End With

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub
    Private Sub txt_com_per_TextChanged(sender As Object, e As EventArgs) Handles txt_com_per.TextChanged
        AgentCommision_Calculation()
        NetAmount_Calculation()

    End Sub
    Private Sub AgentCommision_Calculation()
        Dim tlamt As String = 0
        Dim tlmtr As String = 0

        Dim vInvNetMtr As String = 0
        Dim flperc As String = 0
        Dim flmtr As String = 0



        tlamt = 0
        If dgv_Details_Total.Rows.Count > 0 Then
            tlamt = Format(Val(lbl_TotalAmount.Text), "#########0.00")
        End If


        tlamt = Format(Val(tlamt), "#########0.00")


        txt_CommAmt.Text = Format(Val(tlamt) * Val(txt_com_per.Text) / 100, "########0.00")
    End Sub


    Private Sub cbo_DeliveryTo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_DeliveryTo.GotFocus
        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'SIZING' or Ledger_Type = 'WEAVER' or Ledger_Type = 'JOBWORKER'or Ledger_Type = 'REWINDING'  or (Ledger_Type = '' ) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_DeliveryTo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DeliveryTo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_DeliveryTo, dtp_LrDate, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'SIZING' or Ledger_Type = 'WEAVER' or Ledger_Type = 'JOBWORKER'or Ledger_Type = 'REWINDING'  or (Ledger_Type = '' ) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
        If (e.KeyCode = 38) Then
            If Common_Procedures.settings.CustomerCode = "1186" Then
                cbo_Ledger.Focus()


            Else
                cbo_Transport.Focus()

            End If
        End If


    End Sub

    Private Sub cbo_DeliveryTo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_DeliveryTo.KeyPress
        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_DeliveryTo, cbo_Transport, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'SIZING' or Ledger_Type = 'WEAVER' or Ledger_Type = 'JOBWORKER'or Ledger_Type = 'REWINDING'  or (Ledger_Type = '' ) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_DeliveryTo_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DeliveryTo.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Common_Procedures.MDI_LedType = ""
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_DeliveryTo.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub
    Private Sub Get_GST_Percentage_From_ClothGroup(ByVal ClothName As String, ByRef HSN_Code As String, ByRef GST_PerCent As Single)
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim vClothIdNo As Integer = 0
        Dim Led_IdNo As Integer = 0
        Dim AssVal_Pack_Frgt_Ins_Amt As String = ""
        Dim InterStateStatus As Boolean = False
        vClothIdNo = Common_Procedures.Cloth_NameToIdNo(con, ClothName)

        Try
            Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)


            InterStateStatus = Common_Procedures.Is_InterState_Party(con, Val(lbl_Company.Tag), Led_IdNo)

            HSN_Code = ""
            GST_PerCent = 0



            da = New SqlClient.SqlDataAdapter("select a.* from ItemGroup_Head a INNER JOIN Cloth_Head b ON a.ItemGroup_IdNo = b.ItemGroup_IdNo Where b.Cloth_idno = " & Val(vClothIdNo), con)
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


        Catch ex As Exception
            '---MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            dt.Dispose()
            da.Dispose()

        End Try
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

    Private Sub Get_HSN_CodeWise_Tax_Details()
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim Sno As Integer = 0
        Dim n As Integer = 0
        Dim Led_IdNo As Integer, vClothIdNo As Integer

        Dim AssVal_Pack_Frgt_Ins_Amt As String = ""
        Dim InterStateStatus As Boolean = False
        Dim OnAc_ID As Integer = 0

        vClothIdNo = Common_Procedures.Cloth_NameToIdNo(con, cbo_Grid_ClothName.Text)

        Try

            If FrmLdSTS = True Or NoCalc_Status = True Then Exit Sub

            cmd.Connection = con

            cmd.CommandText = "Truncate table " & Trim(Common_Procedures.EntryTempTable) & ""
            cmd.ExecuteNonQuery()




            'With dgv_Tax_Details

            Sno = 0

            da = New SqlClient.SqlDataAdapter("select a.* from ItemGroup_Head a INNER JOIN Cloth_Head b ON a.ItemGroup_IdNo = b.ItemGroup_IdNo Where b.Cloth_idno = " & Val(vClothIdNo), con)
            dt = New DataTable
                da.Fill(dt)

                If dt.Rows.Count > 0 Then


                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)


                InterStateStatus = Common_Procedures.Is_InterState_Party(con, Val(lbl_Company.Tag), Led_IdNo)

                    For i = 0 To dt.Rows.Count - 1

                    '  n = .Rows.Add()

                    Sno = Sno + 1




                    If InterStateStatus = True Then
                        txt_IGST_Perc.Text = Format(Val(dt.Rows(0).Item("GST_Percentage").ToString), "#############0.00")
                    Else

                        txt_CGST_Perc.Text = Format(Val(dt.Rows(0).Item("GST_Percentage").ToString) / 2, "############0.00")

                        txt_SGST_Perc.Text = Format(Val(dt.Rows(0).Item("GST_Percentage").ToString) / 2, "############0.00")


                    End If




                Next

                End If
                dt.Clear()

                dt.Dispose()
                da.Dispose()

            'End With


        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            dt.Dispose()
            da.Dispose()

        End Try
    End Sub

    Private Sub PrintDialog1_Disposed(sender As Object, e As EventArgs) Handles PrintDialog1.Disposed

    End Sub
    Private Sub TdsCommision_Calculation()
        Dim tlamt As Double = 0
        Dim tdsamt As Double = 0
        Dim Totamt As Double = 0
        If FrmLdSTS = True Then Exit Sub
        If NoCalc_Status = True Then Exit Sub

        tdsamt = Format((Val(lbl_TaxableValue.Text)) * Val(txt_Tds.Text) / 100, "########0")

        lbl_Tds_Amount.Text = Format(Val(tdsamt), "########0.00")

        NetAmount_Calculation()

    End Sub
    Private Sub txt_Tds_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Tds.KeyDown

        If e.KeyCode = 38 Then
            txt_IR_No.Focus()
        End If

        If e.KeyCode = 40 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()
            Else
                msk_Date.Focus()
            End If
        End If

    End Sub

    Private Sub txt_Tds_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_Tds.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If

        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()
            Else
                msk_Date.Focus()
            End If
        End If
    End Sub
    Private Sub txt_Tds_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Tds.TextChanged
        TdsCommision_Calculation()
    End Sub

    Private Sub btn_Close_Selection2_Click(sender As Object, e As EventArgs) Handles btn_Close_Selection2.Click
        btn_Close_Selection_Click(sender, e)
    End Sub

    Private Sub chk_SelectAll_CheckedChanged(sender As Object, e As EventArgs) Handles chk_SelectAll.CheckedChanged
        Dim i, j As Integer



        If chk_SelectAll.Checked = True Then
            For i = 0 To dgv_Selection.Rows.Count - 1
                dgv_Selection.Rows(i).Cells(7).Value = ""
                Select_Dc(i)
            Next i

        Else

            For i = 0 To dgv_Selection.Rows.Count - 1
                dgv_Selection.Rows(i).Cells(7).Value = ""
                For j = 0 To dgv_Selection.ColumnCount - 1
                    dgv_Selection.Rows(i).Cells(j).Style.ForeColor = Color.Black
                Next
            Next i

        End If

    End Sub

    Private Sub dgv_Selection_CellEnter(sender As Object, e As DataGridViewCellEventArgs) Handles dgv_Selection.CellEnter
        With sender
            If Val(.Rows(e.RowIndex).Cells(7).Value) = 0 Then
                .DefaultCellStyle.SelectionForeColor = Color.Black
            Else
                .DefaultCellStyle.SelectionForeColor = Color.Red
            End If
        End With
    End Sub

End Class
