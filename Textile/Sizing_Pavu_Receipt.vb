
Imports System.IO

'Imports Microsoft.Vbe.Interop
Public Class Sizing_Pavu_Receipt
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private MovSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "SZPRC-"
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Private WithEvents dgtxt_PavuDetails As New DataGridViewTextBoxEditingControl
    Private WithEvents dgtxt_YarnDetails As New DataGridViewTextBoxEditingControl
    Private dgv_ActiveCtrl_Name As String

    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
    Private prn_DetAr(500, 10) As String
    Private prn_DetMxIndx As Integer
    Private prn_NoofBmDets As Integer
    Private prn_DetSNo As Integer

    Public vmskOldText As String = ""
    Public vmskSelStrt As Integer = -1
    Private vEndscount_tag As String = ""
    Private vEnds_Name As String = ""
    Private vResultant_Count As String = ""
    Private vCLO_CONDT As String = ""
    Private FnYearCode1 As String = ""
    Private FnYearCode2 As String = ""

    Private fs As FileStream
    Private sw As StreamWriter
    Private Enum DgvCol_details As Integer
        SLNO
        BEAM_NO
        ENDS_COUNT
        PCS
        MTR_PCS
        METERS
        BEAM_WIDTH
        BEAM_TYPE
        SIZING_WEIGHT
        STS
        FABRIC_METERS
    End Enum
    Private Enum DgvFilter_ColDetails As Integer
        SL_NO
        REC_NO
        SET_NO
        REC_DATE
        PARTY_NAME
        ENDS_COUNT
        NO_OF_BEAMS
        TOTAL_METERS
    End Enum
    Private Enum DgvDeliverySelec_ColDetails As Integer
        S_NO
        DC_NO
        DC_DATE
        PARTY_DC_NO
        BEAM_NO
        PCS
        METERS
        STS
        DELIVERY_CODE
    End Enum

    Private Enum DgvSelec_ColDetails As Integer
        S_NO
        REF_NO
        ORDER_DATE
        ORDER_NO
        QUALITY
        ORDER_METERS
        STS
        PROCESS_RECEIPT_CODE
    End Enum


    Private Sub clear()
        New_Entry = False
        Insert_Entry = False
        MovSTS = False
        pnl_Selection.Visible = False
        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        lbl_NewSTS.Visible = False

        chk_Verified_Status.Checked = False
        txt_InvoicePrefixNo.Text = ""
        lbl_RefNo.Text = ""
        lbl_RefNo.ForeColor = Color.Black

        msk_date.Text = ""
        dtp_Date.Text = ""
        txt_PartyDcNo.Text = ""
        txt_SetNo.Text = ""
        cbo_Sizing.Text = ""
        cbo_Sizing.Tag = ""
        cbo_DeliveryTo.Text = Common_Procedures.Ledger_IdNoToName(con, Val(Common_Procedures.CommonLedger.Godown_Ac))
        cbo_DeliveryTo.Tag = ""
        cbo_EndsCount.Text = ""
        txt_PcsLength.Text = ""
        lbl_OrderCode.Text = ""
        lbl_OrderNo.Text = ""
        txt_TotalBobin.Text = ""
        txt_TotalPavu.Text = ""
        txt_Remarks.Text = ""
        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))

        cbo_TransportName.Text = ""
        txt_Freight.Text = ""
        txt_Freight_For.Text = ""
        lbl_Total_FabricMeters.Text = ""


        If cbo_Weaving_JobCardNo.Visible Then cbo_Weaving_JobCardNo.Text = ""
        If cbo_Sizing_JobCardNo.Visible Then cbo_Sizing_JobCardNo.Text = ""

        If cbo_WidthType.Visible Then cbo_WidthType.Text = ""


        cbo_MillName.Text = ""
        txt_Warp_LotNo.Text = ""

        vEndscount_tag = 0
        vEnds_Name = 0
        vResultant_Count = 0
        txt_PickUp_Perc.Text = ""

        lbl_UserName_CreatedBy.Text = ""
        lbl_UserName_ModifiedBy.Text = ""
        chk_Calculate_Sizing_Wgt_Auto.Checked = False
        dgv_PavuDetails.Columns(DgvCol_details.SIZING_WEIGHT).ReadOnly = False
        cbo_ClothSales_OrderCode_forSelection.Text = ""


        dgv_PavuDetails.Rows.Clear()
        dgv_PavuDetails.Rows.Add()

        dgv_PavuDetails_Total.Rows.Clear()
        dgv_PavuDetails_Total.Rows.Add()

        cbo_Sizing.Enabled = True
        cbo_Sizing.BackColor = Color.White

        cbo_DeliveryTo.Enabled = True
        cbo_DeliveryTo.BackColor = Color.White

        txt_SetNo.Enabled = True
        txt_SetNo.BackColor = Color.White

        cbo_EndsCount.Enabled = True
        cbo_EndsCount.BackColor = Color.White

        cbo_TransportName.Enabled = True
        cbo_TransportName.BackColor = Color.White

        txt_Freight.Enabled = True
        txt_Freight.BackColor = Color.White
        If Common_Procedures.settings.CustomerCode = "1186" Then
            Label1.Text = "SIZING WARP RECEIPT"
            Label110.Visible = False
            lbl_OrderNo.Visible = False

        End If

        If Filter_Status = False Then
            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_PartyName.Text = ""

            txt_Filter_SetNo.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1

            dgv_Filter_Details.Rows.Clear()
        End If

        Grid_Cell_DeSelect()

        dgv_ActiveCtrl_Name = ""

        pnl_Delivery_Selection.Visible = False
        lbl_Delivery_Code.Text = ""
        cbo_Type.Text = "DIRECT"

        cbo_Grid_BeamType.Text = ""
        cbo_Grid_Beam_width.Text = ""
        cbo_Grid_BeamType.Tag = -1
        cbo_Grid_Beam_width.Tag = -1

    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox
        Dim Msktxbx As MaskedTextBox
        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Me.ActiveControl Is CheckBox Or TypeOf Me.ActiveControl Is MaskedTextBox Then
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
        If Me.ActiveControl.Name <> cbo_Grid_Beam_width.Name Then
            cbo_Grid_Beam_width.Visible = False
        End If

        If Me.ActiveControl.Name <> cbo_Grid_BeamType.Name Then
            cbo_Grid_BeamType.Visible = False
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

    Private Sub move_record(ByVal no As String)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim NewCode As String
        Dim n As Integer = 0
        Dim I As Integer = 0, J As Integer = 0
        Dim SNo As Integer
        Dim LockSTS As Boolean = False

        If Val(no) = 0 Then Exit Sub

        clear()

        MovSTS = True

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name as SizingName, c.Ledger_Name as DeliveryTo_Name, d.EndsCount_Name from Sizing_Pavu_Receipt_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo  INNER JOIN Ledger_Head c ON a.DeliveryTo_IdNo = c.Ledger_IdNo INNER JOIN EndsCount_Head d ON a.EndsCount_IdNo = d.EndsCount_IdNo Where a.Sizing_Pavu_Receipt_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)
            If dt1.Rows.Count > 0 Then
                txt_InvoicePrefixNo.Text = dt1.Rows(0).Item("Invoice_PrefixNo").ToString
                lbl_RefNo.Text = dt1.Rows(0).Item("Sizing_Pavu_Receipt_RefNo").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Sizing_Pavu_Receipt_Date").ToString
                msk_date.Text = dtp_Date.Text
                cbo_Sizing.Text = dt1.Rows(0).Item("SizingName").ToString

                If Val(dt1.Rows(0).Item("created_useridno").ToString) <> 0 Then
                    If IsDate(dt1.Rows(0).Item("created_DateTime").ToString) = True And Trim(dt1.Rows(0).Item("created_DateTime_Text").ToString) <> "" Then
                        lbl_UserName_CreatedBy.Text = "Created by " & Trim(UCase(Common_Procedures.User_IdNoToName(con, Val(dt1.Rows(0).Item("created_useridno").ToString)))) & " @ " & Trim(dt1.Rows(0).Item("created_DateTime_Text").ToString)
                    Else
                        lbl_UserName_CreatedBy.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con, Val(dt1.Rows(0).Item("created_useridno").ToString))))
                    End If
                End If
                If Val(dt1.Rows(0).Item("Last_modified_useridno").ToString) <> 0 Then
                    If IsDate(dt1.Rows(0).Item("Last_modified_DateTime").ToString) = True And Trim(dt1.Rows(0).Item("Last_modified_DateTime_Text").ToString) <> "" Then
                        lbl_UserName_ModifiedBy.Text = "Last Modified by " & Trim(UCase(Common_Procedures.User_IdNoToName(con, Val(dt1.Rows(0).Item("Last_modified_useridno").ToString)))) & " @ " & Trim(dt1.Rows(0).Item("Last_modified_DateTime_Text").ToString)
                    End If
                End If

                cbo_DeliveryTo.Text = dt1.Rows(0).Item("DeliveryTo_Name").ToString
                txt_PartyDcNo.Text = dt1.Rows(0).Item("Party_DcNo").ToString
                txt_SetNo.Text = dt1.Rows(0).Item("Set_No").ToString
                cbo_EndsCount.Text = dt1.Rows(0).Item("EndsCount_Name").ToString
                txt_PcsLength.Text = dt1.Rows(0).Item("Pcs_Length").ToString
                txt_vehicle.Text = dt1.Rows(0).Item("vehicle_no").ToString
                txt_TotalBobin.Text = dt1.Rows(0).Item("Total_Bobin").ToString
                txt_TotalPavu.Text = dt1.Rows(0).Item("Total_Pavu").ToString

                txt_Remarks.Text = dt1.Rows(0).Item("Remarks").ToString
                cbo_Type.Text = dt1.Rows(0).Item("Selection_type").ToString

                lbl_Delivery_Code.Text = Trim(dt1.Rows(0).Item("Delivery_Code").ToString)

                cbo_Weaving_JobCardNo.Text = dt1.Rows(0).Item("Weaving_JobCode_forSelection").ToString
                cbo_Sizing_JobCardNo.Text = dt1.Rows(0).Item("Sizing_JobCode_forSelection").ToString

                If Val(dt1.Rows(0).Item("Verified_Status").ToString) = 1 Then chk_Verified_Status.Checked = True

                If Trim(dt1.Rows(0).Item("Sizing_Specification_Code").ToString) <> "" Then
                    LockSTS = True
                End If
                lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))
                lbl_OrderNo.Text = dt1.Rows(0).Item("Our_Order_No").ToString
                lbl_OrderCode.Text = dt1.Rows(0).Item("Own_Order_Code").ToString

                cbo_TransportName.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Transport_IdNo").ToString))

                If Val(dt1.Rows(0).Item("Freight").ToString) <> 0 Then
                    txt_Freight.Text = Format(Val(dt1.Rows(0).Item("Freight").ToString), "########0.00")
                End If

                txt_Freight_For.Text = Format(Val(dt1.Rows(0).Item("Freight_For_Bale").ToString), "#########0.00")

                cbo_WidthType.Text = dt1.Rows(0).Item("Width_Type").ToString
                lbl_Total_FabricMeters.Text = dt1.Rows(0).Item("Total_WidthType_Mtrs").ToString

                cbo_MillName.Text = Common_Procedures.Mill_IdNoToName(con, Val(dt1.Rows(0).Item("Mill_IdNo").ToString))
                txt_Warp_LotNo.Text = dt1.Rows(0).Item("Warp_LotNo").ToString

                txt_PickUp_Perc.Text = Format(Val(dt1.Rows(0).Item("Sizing_PickUp_Percentage").ToString), "#########0.00")

                If Val(dt1.Rows(0).Item("Auto_Sizing_Weight_Calculation_Status").ToString) = 1 Then chk_Calculate_Sizing_Wgt_Auto.Checked = True

                cbo_ClothSales_OrderCode_forSelection.Text = Trim(dt1.Rows(0).Item("ClothSales_OrderCode_forSelection").ToString)


                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Pavu_Delivery_Code, b.Pavu_Delivery_Increment, b.Beam_Knotting_Code, b.Loom_Idno, b.Production_Meters, b.Close_Status , d.EndsCount_Name ,BW.Beam_Width_Name from Sizing_Pavu_Receipt_Details a, Stock_SizedPavu_Processing_Details b INNER JOIN EndsCount_Head d ON b.EndsCount_IdNo = d.EndsCount_IdNo LEFT OUTER JOIN Beam_Width_Head BW ON b.Beam_Width_IdNo = BW.Beam_Width_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Sizing_Pavu_Receipt_Code = '" & Trim(NewCode) & "' and b.Reference_Code = '" & Trim(Pk_Condition) & "' + a.Sizing_Pavu_Receipt_Code and a.Set_Code = b.Set_Code and a.Beam_No = b.Beam_No Order by a.Sl_No", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_PavuDetails.Rows.Clear()
                SNo = 0

                If dt2.Rows.Count > 0 Then

                    For I = 0 To dt2.Rows.Count - 1

                        n = dgv_PavuDetails.Rows.Add()

                        SNo = SNo + 1
                        dgv_PavuDetails.Rows(n).Cells(DgvCol_details.SLNO).Value = Val(SNo)
                        dgv_PavuDetails.Rows(n).Cells(DgvCol_details.BEAM_NO).Value = dt2.Rows(I).Item("Beam_No").ToString
                        dgv_PavuDetails.Rows(n).Cells(DgvCol_details.PCS).Value = Val(dt2.Rows(I).Item("Pcs").ToString)
                        If Val(dgv_PavuDetails.Rows(n).Cells(DgvCol_details.PCS).Value) = 0 Then dgv_PavuDetails.Rows(n).Cells(DgvCol_details.PCS).Value = ""
                        dgv_PavuDetails.Rows(n).Cells(DgvCol_details.MTR_PCS).Value = Format(Val(dt2.Rows(I).Item("Meters_Pc").ToString), "########0.00")
                        If Common_Procedures.settings.Weaver_Zari_Kuri_Entries_Status = 1 Then
                            dgv_PavuDetails.Rows(n).Cells(DgvCol_details.METERS).Value = Format(Val(dt2.Rows(I).Item("SizedBeam_Meters").ToString), "########0.000")
                        Else
                            dgv_PavuDetails.Rows(n).Cells(DgvCol_details.METERS).Value = Format(Val(dt2.Rows(I).Item("SizedBeam_Meters").ToString), "########0.00")
                        End If
                        dgv_PavuDetails.Rows(n).Cells(DgvCol_details.STS).Value = ""
                        dgv_PavuDetails.Rows(n).Cells(DgvCol_details.FABRIC_METERS).Value = Val(dt2.Rows(I).Item("Fabric_Meters").ToString)

                        dgv_PavuDetails.Rows(n).Cells(DgvCol_details.SIZING_WEIGHT).Value = Format(Val(dt2.Rows(I).Item("Sizing_Weight").ToString), "########0.000")

                        dgv_PavuDetails.Rows(n).Cells(DgvCol_details.ENDS_COUNT).Value = dt2.Rows(I).Item("EndsCount_Name").ToString

                        dgv_PavuDetails.Rows(n).Cells(DgvCol_details.BEAM_WIDTH).Value = dt2.Rows(I).Item("Beam_Width_Name").ToString

                        dgv_PavuDetails.Rows(n).Cells(DgvCol_details.BEAM_TYPE).Value = Common_Procedures.LoomType_IdNoToName(con, Val(dt2.Rows(I).Item("LoomType_Idno").ToString))

                        If Trim(dt2.Rows(I).Item("Pavu_Delivery_Code").ToString) <> "" Or Val(dt2.Rows(I).Item("Pavu_Delivery_Increment").ToString) <> 0 Or Trim(dt2.Rows(I).Item("Beam_Knotting_Code").ToString) <> "" Or Val(dt2.Rows(I).Item("Production_Meters").ToString) <> 0 Or Val(dt2.Rows(I).Item("Close_Status").ToString) <> 0 Then
                            dgv_PavuDetails.Rows(n).Cells(DgvCol_details.STS).Value = "1"
                            For J = 0 To dgv_PavuDetails.ColumnCount - 1
                                dgv_PavuDetails.Rows(n).Cells(J).Style.BackColor = Color.LightGray
                                dgv_PavuDetails.Rows(n).Cells(J).Style.ForeColor = Color.Red
                            Next
                            LockSTS = True
                        End If



                    Next I

                End If
                dt2.Clear()

                TotalPavu_Calculation()

                'With dgv_PavuDetails_Total
                '    If .RowCount = 0 Then .Rows.Add()
                '    .Rows(0).Cells(1).Value = Val(dt1.Rows(0).Item("Total_Beam").ToString)
                '    .Rows(0).Cells(3).Value = Format(Val(dt1.Rows(0).Item("Total_Meters").ToString), "########0.00")
                'End With

            End If
            dgv_PavuDetails.Rows.Add()
            dt1.Clear()


            If LockSTS = True Then
                cbo_Sizing.Enabled = False
                cbo_Sizing.BackColor = Color.LightGray

                cbo_DeliveryTo.Enabled = False
                cbo_DeliveryTo.BackColor = Color.LightGray

                txt_SetNo.Enabled = False
                txt_SetNo.BackColor = Color.LightGray

                cbo_EndsCount.Enabled = False
                cbo_EndsCount.BackColor = Color.LightGray

                cbo_TransportName.Enabled = False
                cbo_TransportName.BackColor = Color.LightGray

                txt_Freight.Enabled = False
                txt_Freight.BackColor = Color.LightGray

            End If

            Grid_Cell_DeSelect()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            dt1.Dispose()
            da1.Dispose()
            dt2.Dispose()
            da2.Dispose()

            MovSTS = False

            If msk_date.Visible And msk_date.Enabled Then msk_date.Focus()

        End Try

    End Sub

    Private Sub Grid_Cell_DeSelect()
        On Error Resume Next
        If Not IsNothing(dgv_PavuDetails.CurrentCell) Then dgv_PavuDetails.CurrentCell.Selected = False
    End Sub


    Private Sub Sizing_Pavu_Receipt_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Sizing.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Sizing.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_DeliveryTo.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_DeliveryTo.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_EndsCount.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "ENDSCOUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_EndsCount.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_EndsCount.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "ENDSCOUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_EndsCount.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_TransportName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "TRANSPORT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_TransportName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_MillName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "MILL" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_MillName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_Beam_width.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "BEAMWIDTH" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_Beam_width.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_BeamType.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LOOMTYPE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_BeamType.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

    Private Sub Sizing_Pavu_Receipt_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub Sizing_Pavu_Receipt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
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

    Private Sub Sizing_Pavu_Receipt_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Text = ""

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1428" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1464" Then '---- SAKTHI VINAYAGA TEXTILES  (ERODE-PALLIPALAYAM)
            Label1.Text = "SIZEDBEAM RECEIPT FROM SIZING"
        End If

        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()

        cbo_Grid_EndsCount.Visible = False
        lbl_PickUp_Perc_Caption.Visible = False
        txt_PickUp_Perc.Visible = False
        chk_Calculate_Sizing_Wgt_Auto.Visible = False

        dgv_PavuDetails.Columns(DgvCol_details.SIZING_WEIGHT).Visible = False
        dgv_PavuDetails_Total.Columns(DgvCol_details.SIZING_WEIGHT).Visible = False

        dtp_Date.Text = ""
        msk_date.Text = ""
        txt_vehicle.Text = ""
        txt_PartyDcNo.Text = ""
        txt_SetNo.Text = ""
        cbo_Sizing.Text = ""
        cbo_Sizing.Tag = ""
        cbo_DeliveryTo.Text = ""
        cbo_DeliveryTo.Tag = ""
        cbo_EndsCount.Text = ""

        cbo_EndsCount.Text = ""

        btn_BarcodePrint.Visible = False


        If Common_Procedures.settings.Weaver_Zari_Kuri_Entries_Status = 1 Then
            dgv_PavuDetails.Columns(DgvCol_details.METERS).HeaderText = "MTR Or WGT"
            lbl_TotalBobin_Caption.Visible = True
            lbl_TotalPavu_Caption.Visible = True
            txt_TotalBobin.Visible = True
            txt_TotalPavu.Visible = True
        End If

        If Common_Procedures.settings.Pavu_Stock_Maintenance_in_Weight_Status = 1 Then

            lbl_PickUp_Perc_Caption.Visible = True
            txt_PickUp_Perc.Visible = True
            chk_Calculate_Sizing_Wgt_Auto.Visible = True

            dgv_PavuDetails.Columns(DgvCol_details.SIZING_WEIGHT).Visible = True
            dgv_PavuDetails_Total.Columns(DgvCol_details.SIZING_WEIGHT).Visible = True

            lbl_PickUp_Perc_Caption.BackColor = Color.SkyBlue
            txt_PickUp_Perc.BackColor = Color.White
            chk_Calculate_Sizing_Wgt_Auto.BackColor = Color.SkyBlue

            dgv_PavuDetails.Top = txt_PickUp_Perc.Bottom + 5
            dgv_PavuDetails_Total.Top = dgv_PavuDetails.Bottom - 3

            dgv_PavuDetails.Columns(DgvCol_details.BEAM_NO).Width = 80
            dgv_PavuDetails.Columns(DgvCol_details.ENDS_COUNT).Width = 140
            dgv_PavuDetails.Columns(DgvCol_details.PCS).Width = 45
            dgv_PavuDetails.Columns(DgvCol_details.MTR_PCS).Width = 60
            dgv_PavuDetails.Columns(DgvCol_details.METERS).Width = 80
            dgv_PavuDetails.Columns(DgvCol_details.STS).Width = 40

            dgv_PavuDetails_Total.Columns(DgvCol_details.BEAM_NO).Width = dgv_PavuDetails.Columns(DgvCol_details.BEAM_NO).Width
            dgv_PavuDetails_Total.Columns(DgvCol_details.ENDS_COUNT).Width = dgv_PavuDetails.Columns(DgvCol_details.ENDS_COUNT).Width
            dgv_PavuDetails_Total.Columns(DgvCol_details.PCS).Width = dgv_PavuDetails.Columns(DgvCol_details.PCS).Width
            dgv_PavuDetails_Total.Columns(DgvCol_details.MTR_PCS).Width = dgv_PavuDetails.Columns(DgvCol_details.MTR_PCS).Width
            dgv_PavuDetails_Total.Columns(DgvCol_details.METERS).Width = dgv_PavuDetails.Columns(DgvCol_details.METERS).Width
            dgv_PavuDetails_Total.Columns(DgvCol_details.STS).Width = dgv_PavuDetails.Columns(DgvCol_details.STS).Width


        End If


        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1235" Then '---- SOMANUR KALPANA COTTON (INDIA) PVT LTD (SOMANUR)  --TETILES
            dgv_PavuDetails.Columns(DgvCol_details.METERS).HeaderText = "YARDS"
        End If

        lbl_FreightFor_Caption.Visible = False
        txt_Freight_For.Visible = False

        If Common_Procedures.settings.CustomerCode = "1381" Or Common_Procedures.settings.CustomerCode = "1382" Or Common_Procedures.settings.CustomerCode = "1387" Then  'KRS Tex 'Bagavathi Weaving mills
            lbl_FreightFor_Caption.Visible = True
            txt_Freight_For.Visible = True
            txt_Freight_For.BackColor = Color.White

        Else
            cbo_TransportName.Width = cbo_DeliveryTo.Width
            'cbo_EndsCount.Width = cbo_DeliveryTo.Width
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1464" Then '---- MANI OMEGA FABRICS (THIRUCHENKODU)
            cbo_MillName.Visible = True
            cbo_MillName.BackColor = Color.White
            cbo_MillName.Left = txt_Remarks.Left
            cbo_MillName.Top = txt_Remarks.Top
            cbo_MillName.Width = txt_Remarks.Width
            lbl_Remarks_Caption.Text = "Mill Name"

            txt_Warp_LotNo.Visible = True
            txt_Warp_LotNo.BackColor = Color.White
            txt_Warp_LotNo.Left = txt_TotalPavu.Left
            txt_Warp_LotNo.Top = txt_TotalPavu.Top
            txt_Warp_LotNo.Width = txt_TotalPavu.Width
            lbl_TotalPavu_Caption.Visible = True
            lbl_TotalPavu_Caption.Text = "Yarn Lot No."

        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1608" Then
            btn_BarcodePrint.Visible = True
        End If


        btn_Selection.Visible = False
        If Common_Procedures.settings.Internal_Order_Entry_Status = 1 Then
            btn_Selection.Visible = True
        End If

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2

        btn_UserModification.Visible = False
        If Common_Procedures.settings.User_Modifications_Show_Status = 1 Then
            If Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_UserModification_Status = 1 Then
                btn_UserModification.Visible = True
            End If
        End If

        pnl_Selection.Visible = False
        pnl_Selection.Left = (Me.Width - pnl_Selection.Width) \ 2
        pnl_Selection.Top = (Me.Height - pnl_Selection.Height) \ 2
        pnl_Selection.BringToFront()

        If Common_Procedures.settings.Textile_Sizing_Delivery_receipt_Selection = 1 Then
            cbo_Type.Visible = True
            lbl_Type_Caption.Visible = True
            cbo_Type.Items.Clear()
            cbo_Type.Items.Add(" ")
            cbo_Type.Items.Add("DIRECT")
            cbo_Type.Items.Add("DELIVERY")
            btn_Delivery_Selection.Visible = True

        Else
            cbo_Sizing.Width = cbo_DeliveryTo.Width

        End If



        cbo_Verified_Sts.Items.Clear()
        cbo_Verified_Sts.Items.Add("")
        cbo_Verified_Sts.Items.Add("YES")
        cbo_Verified_Sts.Items.Add("NO")

        cbo_WidthType.Items.Clear()
        cbo_WidthType.Items.Add("")
        cbo_WidthType.Items.Add("SINGLE")
        cbo_WidthType.Items.Add("DOUBLE")
        cbo_WidthType.Items.Add("TRIPLE")
        cbo_WidthType.Items.Add("FOURTH")

        cbo_WidthType.Items.Add("SINGLE FABRIC FROM 1 BEAM")
        cbo_WidthType.Items.Add("SINGLE FABRIC FROM 2 BEAMS")

        cbo_WidthType.Items.Add("DOUBLE FABRIC FROM 1 BEAM")
        cbo_WidthType.Items.Add("DOUBLE FABRIC FROM 2 BEAMS")

        cbo_WidthType.Items.Add("TRIPLE FABRIC FROM 1 BEAM")
        cbo_WidthType.Items.Add("TRIPLE FABRIC FROM 2 BEAMS")

        cbo_WidthType.Items.Add("FOUR FABRIC FROM 1 BEAM")
        cbo_WidthType.Items.Add("FOUR FABRIC FROM 2 BEAMS")

        lbl_Weaving_JobCardNo_Caption.Visible = False
        cbo_Weaving_JobCardNo.Visible = False
        lbl_Sizing_JobCardNo_Caption.Visible = False
        cbo_Sizing_JobCardNo.Visible = False

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1428" And Common_Procedures.settings.AutoLoom_PavuWidthWiseConsumption_IN_Delivery = 1 Then     '---- SAKTHI VINAYAGA TEXTILES  (ERODE-PALLIPALAYAM)

            lbl_Widthtype_Caption.Visible = True
            cbo_WidthType.Visible = True
            lbl_Total_FabricMeters_Caption.Visible = False
            lbl_Total_FabricMeters.Visible = False

            dgv_PavuDetails.Columns(DgvCol_details.MTR_PCS).Visible = False
            dgv_PavuDetails.Columns(DgvCol_details.FABRIC_METERS).Visible = True

            dgv_PavuDetails_Total.Columns(DgvCol_details.MTR_PCS).Visible = False
            dgv_PavuDetails_Total.Columns(DgvCol_details.FABRIC_METERS).Visible = True

        End If


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

        If Common_Procedures.settings.Show_Weaver_JobCard_Entry_STATUS = 1 Then

            lbl_Weaving_JobCardNo_Caption.Visible = True
            cbo_Weaving_JobCardNo.Visible = True
            cbo_Weaving_JobCardNo.BackColor = Color.White

            lbl_Weaving_JobCardNo_Caption.Left = lbl_FreightFor_Caption.Left
            lbl_Weaving_JobCardNo_Caption.Top = lbl_FreightFor_Caption.Top

            cbo_Weaving_JobCardNo.Left = txt_Freight_For.Left
            cbo_Weaving_JobCardNo.Top = txt_Freight_For.Top
            cbo_Weaving_JobCardNo.Width = txt_Freight_For.Width

            cbo_TransportName.Width = txt_PartyDcNo.Width
            'cbo_EndsCount.Width = txt_PartyDcNo.Width

        Else

            lbl_Weaving_JobCardNo_Caption.Visible = False
            cbo_Weaving_JobCardNo.Visible = False

        End If


        If Common_Procedures.settings.Show_Sizing_JobCard_Entry_Status = 1 Then
            lbl_Sizing_JobCardNo_Caption.Visible = True
            cbo_Sizing_JobCardNo.Visible = True
            cbo_Sizing_JobCardNo.BackColor = Color.White

            lbl_Sizing_JobCardNo_Caption.Left = lbl_SetNo_Caption.Left
            lbl_Sizing_JobCardNo_Caption.Top = lbl_DeliveryTo_Caption.Top

            cbo_Sizing_JobCardNo.Left = txt_SetNo.Left
            cbo_Sizing_JobCardNo.Top = cbo_DeliveryTo.Top
            cbo_Sizing_JobCardNo.Width = txt_SetNo.Width

            cbo_DeliveryTo.Width = txt_PartyDcNo.Width

        Else

            lbl_Sizing_JobCardNo_Caption.Visible = False
            cbo_Sizing_JobCardNo.Visible = False

        End If

        cbo_ClothSales_OrderCode_forSelection.Visible = False
        lbl_ClothSales_OrderCode_forSelection_Caption.Visible = False

        If Common_Procedures.settings.Show_Sales_OrderNumber_in_ALLEntry_Status = 1 Then

            cbo_TransportName.Width = txt_PartyDcNo.Width

            cbo_ClothSales_OrderCode_forSelection.Visible = True
            cbo_ClothSales_OrderCode_forSelection.BackColor = Color.White
            lbl_ClothSales_OrderCode_forSelection_Caption.Visible = True

            FnYearCode1 = ""
            FnYearCode2 = ""
            Common_Procedures.get_FnYearCode_of_Last_2_Years(FnYearCode1, FnYearCode2)


            lbl_ClothSales_OrderCode_forSelection_Caption.Left = lbl_FreightFor_Caption.Left

            cbo_ClothSales_OrderCode_forSelection.Left = txt_Freight_For.Left
            cbo_ClothSales_OrderCode_forSelection.Top = txt_Freight_For.Top
            cbo_ClothSales_OrderCode_forSelection.Width = txt_Freight_For.Width
            cbo_ClothSales_OrderCode_forSelection.Height = txt_Freight_For.Height

        End If

        AddHandler txt_InvoicePrefixNo.GotFocus, AddressOf ControlGotFocus
        AddHandler chk_Verified_Status.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_date.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Sizing.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_DeliveryTo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_EndsCount.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PcsLength.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PartyDcNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_SetNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Filter_SetNo.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_Fromdate.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_ToDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_EndsCount.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_save.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_close.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_TotalBobin.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_TotalPavu.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Remarks.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_TransportName.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Freight.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Freight_For.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_vehicle.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_WidthType.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_MillName.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Warp_LotNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Weaving_JobCardNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Sizing_JobCardNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_EndsCount.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PickUp_Perc.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_BeamType.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_Beam_width.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ClothSales_OrderCode_forSelection.GotFocus, AddressOf ControlGotFocus


        AddHandler chk_Verified_Status.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Remarks.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_date.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Sizing.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_DeliveryTo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_EndsCount.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Filter_SetNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PcsLength.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PartyDcNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_SetNo.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_Fromdate.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_ToDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_EndsCount.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_save.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_close.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_TotalPavu.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_TotalBobin.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_TransportName.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Freight.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Freight_For.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_vehicle.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_WidthType.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_MillName.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Warp_LotNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Weaving_JobCardNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Sizing_JobCardNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_EndsCount.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PickUp_Perc.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_BeamType.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_Beam_width.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ClothSales_OrderCode_forSelection.LostFocus, AddressOf ControlLostFocus

        'AddHandler txt_vehicle.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler msk_date.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler txt_PartyDcNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_SetNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_TotalBobin.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_TotalPavu.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler txt_Freight_For.KeyDown, AddressOf TextBoxControlKeyDown


        'AddHandler txt_vehicle.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler msk_date.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_PartyDcNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_SetNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_Fromdate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_ToDate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_TotalBobin.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_TotalPavu.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_Freight_For.KeyPress, AddressOf TextBoxControlKeyPress


        AddHandler txt_InvoicePrefixNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_InvoicePrefixNo.KeyPress, AddressOf TextBoxControlKeyPress

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        pnl_Delivery_Selection.Visible = False
        pnl_Delivery_Selection.Left = (Me.Width - pnl_Delivery_Selection.Width) \ 2
        pnl_Delivery_Selection.Top = (Me.Height - pnl_Delivery_Selection.Height) \ 2
        pnl_Delivery_Selection.BringToFront()


        Filter_Status = False
        FrmLdSTS = True
        new_record()
    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
        Dim dgv1 As New DataGridView
        Dim i As Integer
        Dim vLASTCOL As Integer = -1
        Dim vCURRCOL As Integer = -1

        If ActiveControl.Name = dgv_PavuDetails.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            On Error Resume Next

            dgv1 = Nothing
            If ActiveControl.Name = dgv_PavuDetails.Name Then
                dgv1 = dgv_PavuDetails

            ElseIf dgv_PavuDetails.IsCurrentRowDirty = True Then
                dgv1 = dgv_PavuDetails

            ElseIf dgv_ActiveCtrl_Name = dgv_PavuDetails.Name Then
                dgv1 = dgv_PavuDetails

            End If

            If IsNothing(dgv1) = True Then
                Return MyBase.ProcessCmdKey(msg, keyData)
                Exit Function
            End If

            With dgv1


                If dgv1.Name = dgv_PavuDetails.Name Then


                    If .Columns(DgvCol_details.SIZING_WEIGHT).Visible Then
                        vLASTCOL = DgvCol_details.SIZING_WEIGHT '.Columns.Count - 2
                    Else
                        vLASTCOL = DgvCol_details.BEAM_TYPE '.Columns.Count - 3
                    End If

                    vCURRCOL = .CurrentCell.ColumnIndex

                    If keyData = Keys.Enter Or keyData = Keys.Down Then

LOOP1:

                        If vCURRCOL >= vLASTCOL Then

                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                .Rows.Add()
                                'dgv_YarnDetails.Focus()
                                'dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(1)

                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(DgvCol_details.BEAM_NO)

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(DgvCol_details.BEAM_NO)

                            End If

                        Else

                            If .CurrentCell.RowIndex = .RowCount - 1 And vCURRCOL >= 1 And ((vCURRCOL <> 1 And Trim(.CurrentRow.Cells(DgvCol_details.BEAM_NO).Value) = "") Or (vCURRCOL = 1 And Trim(dgtxt_PavuDetails.Text) = "")) Then
                                For i = 0 To .Columns.Count - 1
                                    .Rows(.CurrentCell.RowIndex).Cells(i).Value = ""
                                Next

                                If cbo_WidthType.Visible = True And cbo_WidthType.Enabled = True Then
                                    cbo_WidthType.Focus()
                                ElseIf cbo_MillName.Visible And cbo_MillName.Enabled Then
                                    cbo_MillName.Focus()
                                Else
                                    txt_Remarks.Focus()
                                End If

                                'If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                                '    save_record()
                                'Else
                                '    msk_date.Focus()
                                'End If

                            Else

                                vCURRCOL = vCURRCOL + 1
                                If .Columns(vCURRCOL).Visible = True And .Columns(vCURRCOL).ReadOnly = False Then
                                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(vCURRCOL)
                                Else
                                    GoTo LOOP1
                                End If



                            End If


                        End If

                        Return True


                    ElseIf keyData = Keys.Up Then
LOOP2:
                        If .CurrentCell.ColumnIndex <= 1 Then
                            If .CurrentCell.RowIndex = 0 Then
                                ' cbo_EndsCount.Focus()
                                If txt_PickUp_Perc.Visible And Enabled Then
                                    txt_PickUp_Perc.Focus()
                                Else
                                    txt_Freight.Focus()
                                End If
                                'txt_PcsLength.Focus()

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(vLASTCOL)

                            End If

                        Else
                            vCURRCOL = vCURRCOL - 1
                            If .Columns(vCURRCOL).Visible = True And .Columns(vCURRCOL).ReadOnly = False Then
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(vCURRCOL)
                            Else
                                GoTo LOOP2
                            End If
                            '.CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)

                        End If

                        Return True

                    Else
                        Return MyBase.ProcessCmdKey(msg, keyData)

                    End If

                End If

            End With

        Else

            Return MyBase.ProcessCmdKey(msg, keyData)

        End If

    End Function

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

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""

        Dim vOrdByNo As String = ""

        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text)

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.Sizing_pavu_Receipt_Entry, New_Entry, Me, con, "Sizing_Pavu_Receipt_Head", "Sizing_Pavu_Receipt_Code", NewCode, "Sizing_Pavu_Receipt_Date", "(Sizing_Pavu_Receipt_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub



        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Sizing_Specification_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Sizing_Specification_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        If Common_Procedures.settings.Vefified_Status = 1 Then

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            If Val(Common_Procedures.get_FieldValue(con, "Sizing_Pavu_Receipt_Head", "Verified_Status", "(Sizing_Pavu_Receipt_Code = '" & Trim(NewCode) & "')")) = 1 Then
                MessageBox.Show("Entry Already Verified", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub
            End If
        End If

        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> DialogResult.Yes Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Da = New SqlClient.SqlDataAdapter("select count(*) from Sizing_Pavu_Receipt_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Sizing_Pavu_Receipt_Code = '" & Trim(NewCode) & "' and Sizing_Specification_Code <> ''", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                If Val(Dt1.Rows(0)(0).ToString) > 0 Then
                    MessageBox.Show("Already Sizing specifiation Prepared", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If
        Dt1.Clear()

        Da = New SqlClient.SqlDataAdapter("select count(*) from Stock_SizedPavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and  ( Pavu_Delivery_Code <> '' or Pavu_Delivery_Increment <> 0 or Beam_Knotting_Code <> '' or Loom_Idno <> 0 or Production_Meters <> 0 or Close_Status <> 0 )", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                If Val(Dt1.Rows(0)(0).ToString) > 0 Then
                    MessageBox.Show("Already some Pavu Delivered", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If
        Dt1.Clear()

        trans = con.BeginTransaction

        Try

            cmd.Connection = con
            cmd.Transaction = trans
            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "DELETE", "Sizing_Pavu_Receipt_head", "Sizing_Pavu_Receipt_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "", "", "Sizing_Pavu_Receipt_Code, Company_IdNo, for_OrderBy", trans)
            Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "DELETE", "Sizing_Pavu_Receipt_Details", "Sizing_Pavu_Receipt_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, " Beam_No,Pcs,Meters_Pc,Meters", "Sl_No", "Sizing_Pavu_Receipt_Code, For_OrderBy, Company_IdNo, Sizing_Pavu_Receipt_No, Sizing_Pavu_Receipt_Date, Ledger_Idno", trans)

            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), trans)

            cmd.CommandText = "Delete from Stock_Empty_BeamBagCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_SizedPavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Pavu_Delivery_Code = '' and Pavu_Delivery_Increment = 0 and Beam_Knotting_Code = '' and Loom_Idno = 0"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Pavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Empty_BeamBagCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Pavu_Delivery_Selections_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Sizing_Pavu_Receipt_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sizing_Pavu_Receipt_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Sizing_Pavu_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sizing_Pavu_Receipt_Code = '" & Trim(NewCode) & "'"
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

        If msk_date.Enabled = True And msk_date.Visible = True Then msk_date.Focus()

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()
        If Filter_Status = False Then

            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable
            Dim dt2 As New DataTable
            Dim dt3 As New DataTable

            da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or Ledger_Type = 'SIZING') order by Ledger_DisplayName", con)
            da.Fill(dt1)
            cbo_Filter_PartyName.DataSource = dt1
            cbo_Filter_PartyName.DisplayMember = "Ledger_DisplayName"

            da = New SqlClient.SqlDataAdapter("select distinct(EndsCount_Name) from EndsCount_Head order by EndsCount_Name", con)
            da.Fill(dt3)
            cbo_Filter_EndsCount.DataSource = dt3
            cbo_Filter_EndsCount.DisplayMember = "EndsCount_Name"

            'dtp_Filter_Fromdate.Text = ""
            'dtp_Filter_ToDate.Text = ""

            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate

            cbo_Filter_PartyName.Text = ""

            cbo_Filter_EndsCount.Text = ""

            txt_Filter_SetNo.Text = ""

            cbo_Filter_PartyName.SelectedIndex = -1

            cbo_Filter_EndsCount.SelectedIndex = -1

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
        Dim movno As String

        Try

            con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
            con.Open()
            da = New SqlClient.SqlDataAdapter("select top 1 Sizing_Pavu_Receipt_RefNo from Sizing_Pavu_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sizing_Pavu_Receipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Sizing_Pavu_Receipt_RefNo", con)
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

            con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
            con.Open()
            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Sizing_Pavu_Receipt_RefNo from Sizing_Pavu_Receipt_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sizing_Pavu_Receipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Sizing_Pavu_Receipt_RefNo", con)
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

            con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
            con.Open()
            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Sizing_Pavu_Receipt_RefNo from Sizing_Pavu_Receipt_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sizing_Pavu_Receipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Sizing_Pavu_Receipt_RefNo desc", con)
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
            con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
            con.Open()
            da = New SqlClient.SqlDataAdapter("select top 1 Sizing_Pavu_Receipt_RefNo from Sizing_Pavu_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sizing_Pavu_Receipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Sizing_Pavu_Receipt_RefNo desc", con)
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
            con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
            con.Open()
            clear()

            New_Entry = True
            lbl_NewSTS.Visible = True

            lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Sizing_Pavu_Receipt_Head", "Sizing_Pavu_Receipt_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

            lbl_RefNo.ForeColor = Color.Red


            msk_date.Text = Date.Today.ToShortDateString
            da = New SqlClient.SqlDataAdapter("select top 1 * from Sizing_Pavu_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sizing_Pavu_Receipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Sizing_Pavu_Receipt_RefNo desc", con)
            dt1 = New DataTable
            da.Fill(dt1)
            If dt1.Rows.Count > 0 Then
                If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)
                    If dt1.Rows(0).Item("Sizing_Pavu_Receipt_Date").ToString <> "" Then msk_date.Text = dt1.Rows(0).Item("Sizing_Pavu_Receipt_Date").ToString
                End If


                If IsDBNull(dt1.Rows(0).Item("Invoice_PrefixNo").ToString) = False Then
                    If dt1.Rows(0).Item("Invoice_PrefixNo").ToString <> "" Then txt_InvoicePrefixNo.Text = dt1.Rows(0).Item("Invoice_PrefixNo").ToString
                End If

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

            con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
            con.Open()
            inpno = InputBox("Enter Rec.No.", "FOR FINDING...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Sizing_Pavu_Receipt_RefNo from Sizing_Pavu_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sizing_Pavu_Receipt_Code = '" & Trim(RecCode) & "'", con)
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

        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()
        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Sizing_Specification_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Sizing_Specification_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.Sizing_pavu_Receipt_Entry, New_Entry, Me) = False Then Exit Sub

        Try

            inpno = InputBox("Enter New Ref No.", "FOR NEW REF INSERTION...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Sizing_Pavu_Receipt_RefNo from Sizing_Pavu_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sizing_Pavu_Receipt_Code = '" & Trim(RecCode) & "'", con)
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
                    lbl_RefNo.Text = Trim(UCase(inpno))

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
        Dim dt2 As New DataTable
        Dim Da4 As New SqlClient.SqlDataAdapter
        Dim Da5 As New SqlClient.SqlDataAdapter
        Dim Dt4 As New DataTable
        Dim Dt5 As New DataTable
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim Led_ID As Integer = 0
        Dim DelvTo_ID As Integer = 0
        Dim EdsCnt_ID As Integer = 0, vDET_EdsCnt_ID As Integer = 0
        Dim Nr As Integer = 0
        Dim Sno As Integer = 0, I As Integer, K As Integer
        Dim Partcls As String = "", YrnPartcls As String = ""
        Dim PBlNo As String = ""
        Dim EntID As String = ""
        Dim vTotPvuBms As Single = 0
        Dim vTotPvuMtrs As Single = 0
        Dim vTotFABMtrs As String = 0
        Dim vTotPvuStk As Single = 0
        Dim Dup_SetNoBmNo As String = ""
        Dim Mtr_Pc As Single = 0
        Dim pCnt_ID As Integer = 0
        Dim pEds_Nm As String = ""
        Dim vSetCd As String = ""
        Dim Selc_SetCode As String = ""
        Dim VouBil As String = ""
        Dim Del_ID As Integer = 0, Rec_ID As Integer = 0
        Dim Stock_In As String = ""
        Dim mtrspcs As Single = 0
        Dim StkAt_IdNo As Integer = 0
        Dim Empty_Bms As Integer = 0
        Dim Usr_ID As Integer = 0
        Dim OurOrd_No As String = ""
        Dim Trans_ID As Integer = 0
        Dim Verified_STS As String = ""
        Dim vOrdByNo As String = ""
        Dim vInvoNo As String = ""
        Dim vSELC_DCCODE As String = ""
        Dim vSTKPAVUMTRS As String = 0
        Dim vMILL_IdNo As Integer
        Dim EndsCnt_ID_Header As Integer = 0
        Dim vCREATED_DTTM_TXT As String = ""
        Dim vMODIFIED_DTTM_TXT As String = ""
        Dim Weaver_Job_Code As String = ""
        Dim Auto_Sizing_Wgt_Cal_STS As Integer = 0
        Dim vTOT_Sizing_Wgt As String = ""

        Dim vLoomType_Idno As Integer = 0
        Dim Bw_ID As Integer = 0

        Dim vLoomType_Id_Stck_pst_frstlne As Integer = 0
        Dim Bw_Id_Stck_pst_frstlne As Integer = 0


        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text)

        vInvoNo = Trim(txt_InvoicePrefixNo.Text) & Trim(lbl_RefNo.Text)
        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        If Val(Common_Procedures.User.IdNo) = 0 Then
            MessageBox.Show("Invalid User Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If


        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        ' If Common_Procedures.UserRight_Check(Common_Procedures.UR.Sizing_Specification_Entry, New_Entry) = False Then Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Sizing_pavu_Receipt_Entry, New_Entry, Me, con, "Sizing_Pavu_Receipt_Head", "Sizing_Pavu_Receipt_Code", NewCode, "Sizing_Pavu_Receipt_Date", "(Sizing_Pavu_Receipt_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Sizing_Pavu_Receipt_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, Sizing_Pavu_Receipt_RefNo desc", dtp_Date.Value.Date) = False Then Exit Sub

        If Common_Procedures.settings.Vefified_Status = 1 Then
            If Not (Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_Verified_Status = 1) Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

                If Val(Common_Procedures.get_FieldValue(con, "Sizing_Pavu_Receipt_Head", "Verified_Status", "(Sizing_Pavu_Receipt_Code = '" & Trim(NewCode) & "')")) = 1 Then
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

        Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Sizing.Text)
        If Led_ID = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Sizing.Enabled And cbo_Sizing.Visible Then cbo_Sizing.Focus()
            Exit Sub
        End If
        If Trim(lbl_OrderCode.Text) <> "" Then


            If Led_ID <> 0 Then

                Da = New SqlClient.SqlDataAdapter("select a.*,b.* from Own_Order_Head a INNER JOIN Own_order_Sizing_Details b ON a.Own_Order_Code =b.Own_Order_Code where a.Own_Order_Code = '" & Trim(lbl_OrderCode.Text) & "' and  b.Ledger_idno = " & Str(Val(Led_ID)), con)
                Dt1 = New DataTable
                Da.Fill(Dt1)
                If Dt1.Rows.Count > 0 Then
                    OurOrd_No = Dt1.Rows(0).Item("Order_No").ToString

                End If
            End If
            If Trim(OurOrd_No) <> Trim(lbl_OrderNo.Text) Then
                MessageBox.Show("Invalid Mismatch Of Order No for this Party Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_Sizing.Enabled And cbo_Sizing.Visible Then cbo_Sizing.Focus()
                Exit Sub
            End If
        End If

        Verified_STS = 0
        If chk_Verified_Status.Checked = True Then Verified_STS = 1

        If Trim(txt_SetNo.Text) = "" Then
            MessageBox.Show("Invalid Set No", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If txt_SetNo.Enabled And txt_SetNo.Visible Then txt_SetNo.Focus()
            Exit Sub
        End If
        lbl_UserName.Text = Common_Procedures.User.IdNo
        vSetCd = Trim(Val(lbl_Company.Tag)) & "-" & Trim(txt_SetNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        Selc_SetCode = Trim(txt_SetNo.Text) & "/" & Trim(Common_Procedures.FnYearCode) & "/" & Trim(Val(lbl_Company.Tag))
        Usr_ID = Common_Procedures.User_NameToIdNo(con1, lbl_UserName.Text)
        DelvTo_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_DeliveryTo.Text)
        If DelvTo_ID = 0 Then
            DelvTo_ID = Val(Common_Procedures.CommonLedger.Godown_Ac)
            cbo_DeliveryTo.Text = Common_Procedures.Ledger_IdNoToName(con, DelvTo_ID)
            'MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            'If cbo_Sizing.Enabled And cbo_Sizing.Visible Then cbo_Sizing.Focus()
            'Exit Sub
        End If

        Trans_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_TransportName.Text)

        If Trim(txt_PartyDcNo.Text) <> "" Then
            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            Da = New SqlClient.SqlDataAdapter("select * from Sizing_Pavu_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sizing_Pavu_Receipt_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' and Ledger_IdNo = " & Str(Val(Led_ID)) & " and Party_DcNo = '" & Trim(txt_PartyDcNo.Text) & "' and Sizing_Pavu_Receipt_Code <> '" & Trim(NewCode) & "'", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                MessageBox.Show("Duplicate Party Dc.No to this Party", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If txt_PartyDcNo.Enabled And txt_PartyDcNo.Visible Then txt_PartyDcNo.Focus()
                Exit Sub
            End If
            Dt1.Clear()
        End If
        With dgv_PavuDetails

            For I = 0 To .RowCount - 1

                If Val(.Rows(I).Cells(DgvCol_details.METERS).Value) <> 0 Then

                    If Trim(.Rows(I).Cells(DgvCol_details.BEAM_NO).Value) = "" Then
                        MessageBox.Show("Invalid Beam No", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(I).Cells(DgvCol_details.BEAM_NO)
                            .CurrentCell.Selected = True
                        End If
                        Exit Sub
                    End If

                    If InStr(1, Trim(.Rows(I).Cells(DgvCol_details.BEAM_NO).Value), " ") > 0 Then
                        MessageBox.Show("Invalid Beam No, Spaces not allowed in SetNo", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(I).Cells(DgvCol_details.BEAM_NO)
                            .CurrentCell.Selected = True
                        End If
                        Exit Sub
                    End If

                    If InStr(1, Trim(UCase(Dup_SetNoBmNo)), "~" & Trim(UCase(.Rows(I).Cells(DgvCol_details.BEAM_NO).Value)) & "~") > 0 Then
                        MessageBox.Show("Duplicate BeamNo ", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(I).Cells(DgvCol_details.BEAM_NO)
                            .CurrentCell.Selected = True
                        End If
                        Exit Sub
                    End If

                    Dup_SetNoBmNo = Trim(Dup_SetNoBmNo) & "~" & Trim(UCase(.Rows(I).Cells(DgvCol_details.BEAM_NO).Value)) & "~"

                    If Val(.Rows(0).Cells(DgvCol_details.MTR_PCS).Value) <> 0 Then
                        txt_PcsLength.Text = Val(.Rows(0).Cells(DgvCol_details.MTR_PCS).Value)
                    End If


                    If Trim(.Rows(I).Cells(DgvCol_details.ENDS_COUNT).Value) = "" Then
                        MessageBox.Show("Invalid Ends Count", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(I).Cells(DgvCol_details.ENDS_COUNT)
                            .CurrentCell.Selected = True
                        End If
                        Exit Sub
                    End If

                End If

            Next I

        End With

        EndsCnt_ID_Header = 0
        If cbo_EndsCount.Visible Then
            EndsCnt_ID_Header = Common_Procedures.EndsCount_NameToIdNo(con, cbo_EndsCount.Text)
        End If
        If EndsCnt_ID_Header = 0 Then
            EndsCnt_ID_Header = Common_Procedures.EndsCount_NameToIdNo(con, dgv_PavuDetails.Rows(0).Cells(DgvCol_details.ENDS_COUNT).Value)
        End If


        If cbo_WidthType.Visible Then
            If Trim(cbo_WidthType.Text) = "" Then
                MessageBox.Show("Invalid Width TYpe", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_WidthType.Enabled And cbo_WidthType.Visible Then cbo_WidthType.Focus()
                Exit Sub
            End If
        End If

        vMILL_IdNo = Common_Procedures.Mill_NameToIdNo(con, cbo_MillName.Text)

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1464" Then '---- MANI OMEGA FABRICS (THIRUCHENKODU)

            If cbo_MillName.Visible Then
                If vMILL_IdNo = 0 Then
                    MessageBox.Show("Invalid Mill Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    If cbo_MillName.Enabled And cbo_MillName.Visible Then cbo_MillName.Focus()
                    Exit Sub
                End If
            End If

            If txt_Warp_LotNo.Visible Then
                If Trim(txt_Warp_LotNo.Text) = "" Then
                    MessageBox.Show("Invalid Warp LotNo.", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    If txt_Warp_LotNo.Enabled And txt_Warp_LotNo.Visible Then txt_Warp_LotNo.Focus()
                    Exit Sub
                End If
            End If

        End If

        TotalPavu_Calculation()

        vTotPvuBms = 0 : vTotPvuMtrs = 0 : vTotFABMtrs = 0 : vTOT_Sizing_Wgt = 0
        If dgv_PavuDetails_Total.RowCount > 0 Then
            vTotPvuBms = Val(dgv_PavuDetails_Total.Rows(0).Cells(DgvCol_details.BEAM_NO).Value())
            vTotPvuMtrs = Val(dgv_PavuDetails_Total.Rows(0).Cells(DgvCol_details.METERS).Value())
            vTotFABMtrs = Val(dgv_PavuDetails_Total.Rows(0).Cells(DgvCol_details.FABRIC_METERS).Value())
            vTOT_Sizing_Wgt = Val(dgv_PavuDetails_Total.Rows(0).Cells(DgvCol_details.SIZING_WEIGHT).Value())
        End If


        If Common_Procedures.settings.Textile_Sizing_Delivery_receipt_Selection = 1 And Trim(cbo_Type.Text) = "DELIVERY" Then
            vSELC_DCCODE = Trim(lbl_Delivery_Code.Text)
        End If

        Weaver_Job_Code = ""
        If Trim(cbo_Weaving_JobCardNo.Text) <> "" Then
            Weaver_Job_Code = Trim(cbo_Weaving_JobCardNo.Text)
        End If

        Auto_Sizing_Wgt_Cal_STS = 0
        If chk_Calculate_Sizing_Wgt_Auto.Checked = True Then Auto_Sizing_Wgt_Cal_STS = 1

        If Common_Procedures.settings.Sales_OrderNumber_compulsory_in_ALLEntry_Status = 1 Then
            If Trim(cbo_ClothSales_OrderCode_forSelection.Text) = "" Then
                MessageBox.Show("Invalid Sales Order No", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_ClothSales_OrderCode_forSelection.Enabled And cbo_ClothSales_OrderCode_forSelection.Visible Then cbo_ClothSales_OrderCode_forSelection.Focus()
                Exit Sub
            End If
        End If

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Sizing_Pavu_Receipt_Head", "Sizing_Pavu_Receipt_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@EntryDate", Convert.ToDateTime(msk_date.Text))


            vCREATED_DTTM_TXT = ""
            vMODIFIED_DTTM_TXT = ""

            vCREATED_DTTM_TXT = Trim(Format(Now, "dd-MM-yyyy hh:mm tt"))
            cmd.Parameters.AddWithValue("@createddatetime", Now)


            vMODIFIED_DTTM_TXT = Trim(Format(Now, "dd-MM-yyyy hh:mm tt"))
            cmd.Parameters.AddWithValue("@modifieddatetime", Now)

            If New_Entry = True Then

                cmd.CommandText = "Insert into Sizing_Pavu_Receipt_Head ( Sizing_Pavu_Receipt_Code,            Company_IdNo          , Sizing_Pavu_Receipt_No ,   Sizing_Pavu_Receipt_refNo   ,                 Invoice_PrefixNo               ,                               for_OrderBy                              , Sizing_Pavu_Receipt_Date,          Ledger_IdNo    ,       DeliveryTo_IdNo      ,           Party_DcNo              ,               Set_No          ,          EndsCount_IdNo    ,             Pcs_Length         ,            Total_Beam       ,           Total_Meters       ,                  Total_Pavu         ,             Total_Bobin              ,              User_idNo         ,                 Our_Order_No     ,                Own_Order_Code      ,         Verified_Status   ,               Remarks            ,           Transport_IdNo  ,                   Freight           ,            Freight_For_Bale            ,              Vehicle_No         ,     Selection_type           ,          Delivery_Code       ,           Width_Type              ,        Total_WidthType_Mtrs                  ,              Mill_IdNo      ,                 Warp_LotNo         , Weaving_JobCode_forSelection          , Sizing_JobCode_forSelection               ,     Auto_Sizing_Weight_Calculation_Status  ,         Sizing_PickUp_Percentage       ,          Total_Sizing_Weight       ,              ClothSales_OrderCode_forSelection              ,       created_useridno           ,   created_DateTime,          created_DateTime_Text    , Last_modified_useridno, Last_modified_DateTime, Last_modified_DateTime_Text ) " &
                                    " Values                            ( '" & Trim(NewCode) & "' , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(vInvoNo) & "', '" & Trim(lbl_RefNo.Text) & "', '" & Trim(UCase(txt_InvoicePrefixNo.Text)) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",        @EntryDate       , " & Str(Val(Led_ID)) & ", " & Str(Val(DelvTo_ID)) & ", '" & Trim(txt_PartyDcNo.Text) & "', '" & Trim(txt_SetNo.Text) & "', " & Str(Val(EndsCnt_ID_Header)) & ", " & Val(txt_PcsLength.Text) & ", " & Str(Val(vTotPvuBms)) & ", " & Str(Val(vTotPvuMtrs)) & ", " & Str(Val(txt_TotalPavu.Text)) & ", " & Str(Val(txt_TotalBobin.Text)) & ",  " & Val(lbl_UserName.Text) & ", '" & Trim(lbl_OrderNo.Text) & "' ,  '" & Trim(lbl_OrderCode.Text) & "', " & Val(Verified_STS) & " , '" & Trim(txt_Remarks.Text) & "' , " & Str(Val(Trans_ID)) & ",  " & Str(Val(txt_Freight.Text)) & " ,  " & Str(Val(txt_Freight_For.Text)) & ", '" & Trim(txt_vehicle.Text) & "', '" & Trim(cbo_Type.Text) & "', '" & Trim(vSELC_DCCODE) & "' , '" & Trim(cbo_WidthType.Text) & "', " & Str(Val(lbl_Total_FabricMeters.Text)) & ", " & Str(Val(vMILL_IdNo)) & ", '" & Trim(txt_Warp_LotNo.Text) & "','" & Trim(Weaver_Job_Code) & "', '" & Trim(cbo_Sizing_JobCardNo.Text) & "' ,  " & Str(Val(Auto_Sizing_Wgt_Cal_STS)) & " , " & Str(Val(txt_PickUp_Perc.Text)) & " , " & Str(Val(vTOT_Sizing_Wgt)) & "  ,  '" & Trim(cbo_ClothSales_OrderCode_forSelection.Text) & "' ,       " & Str(Val(Common_Procedures.User.IdNo)) & ",  @createddatetime ,  '" & Trim(vCREATED_DTTM_TXT) & "',              0        ,     NUll              ,          ''       ) "
                cmd.ExecuteNonQuery()

            Else

                Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "OLD", "Sizing_Pavu_Receipt_head", "Sizing_Pavu_Receipt_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Sizing_Pavu_Receipt_Code, Company_IdNo, for_OrderBy", tr)
                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "OLD", "Sizing_Pavu_Receipt_Details", "Sizing_Pavu_Receipt_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, " Beam_No,Pcs,Meters_Pc,Meters", "Sl_No", "Sizing_Pavu_Receipt_Code, For_OrderBy, Company_IdNo, Sizing_Pavu_Receipt_No, Sizing_Pavu_Receipt_Date, Ledger_Idno", tr)

                If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1490" Then '---- LAKSHMI SARASWATHI EXPORTS (THIRUCHENCODE)

                    Da = New SqlClient.SqlDataAdapter("select count(*) from Sizing_Pavu_Receipt_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Sizing_Pavu_Receipt_Code = '" & Trim(NewCode) & "' and Sizing_Specification_Code <> ''", con)
                    Da.SelectCommand.Transaction = tr
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    If Dt1.Rows.Count > 0 Then
                        If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                            If Val(Dt1.Rows(0)(0).ToString) > 0 Then
                                Throw New ApplicationException("Already Sizing specifiation Prepared")
                                Exit Sub
                            End If
                        End If
                    End If
                    Dt1.Clear()

                End If

                cmd.CommandText = "Update Sizing_Pavu_Receipt_Head set Invoice_PrefixNo = '" & Trim(UCase(txt_InvoicePrefixNo.Text)) & "' ,  Sizing_Pavu_Receipt_refNo =   '" & Trim(lbl_RefNo.Text) & "',Sizing_Pavu_Receipt_No='" & Trim(vInvoNo) & "' , Sizing_Pavu_Receipt_Date = @EntryDate, Ledger_IdNo = " & Str(Val(Led_ID)) & ", DeliveryTo_IdNo = " & Str(Val(DelvTo_ID)) & ", Party_DcNo = '" & Trim(txt_PartyDcNo.Text) & "',  Set_No = '" & Trim(txt_SetNo.Text) & "', EndsCount_IdNo = " & Str(Val(EndsCnt_ID_Header)) & ", Total_Pavu=  " & Str(Val(txt_TotalPavu.Text)) & ", Total_Bobin = " & Str(Val(txt_TotalBobin.Text)) & " , Pcs_Length = '" & Trim(txt_PcsLength.Text) & "', Total_Beam = " & Str(Val(vTotPvuBms)) & " , Total_Meters =  " & Str(Val(vTotPvuMtrs)) & " ,User_idNo = " & Val(lbl_UserName.Text) & " ,Our_Order_No = '" & Trim(lbl_OrderNo.Text) & "',Own_Order_Code = '" & Trim(lbl_OrderCode.Text) & "',Verified_Status= " & Val(Verified_STS) & ",Remarks='" & Trim(txt_Remarks.Text) & "' ,  Transport_IdNo = " & Str(Val(Trans_ID)) & ", Freight = " & Str(Val(txt_Freight.Text)) & " , Freight_For_Bale = " & Str(Val(txt_Freight_For.Text)) & ",Vehicle_No='" & Trim(txt_vehicle.Text) & "' ,Selection_type='" & Trim(cbo_Type.Text) & "',Delivery_Code='" & Trim(vSELC_DCCODE) & "' ,  Width_Type = '" & Trim(cbo_WidthType.Text) & "'  , Total_WidthType_Mtrs = " & Str(Val(lbl_Total_FabricMeters.Text)) & " , Mill_IdNo = " & Str(Val(vMILL_IdNo)) & ", Warp_LotNo = '" & Trim(txt_Warp_LotNo.Text) & "',Weaving_JobCode_forSelection = '" & Trim(Weaver_Job_Code) & "',Sizing_JobCode_forSelection = '" & Trim(cbo_Sizing_JobCardNo.Text) & "' , Auto_Sizing_Weight_Calculation_Status  =" & Str(Val(Auto_Sizing_Wgt_Cal_STS)) & " , Sizing_PickUp_Percentage = " & Str(Val(txt_PickUp_Perc.Text)) & " ,Total_Sizing_Weight = " & Str(Val(vTOT_Sizing_Wgt)) & "  , ClothSales_OrderCode_forSelection = '" & Trim(cbo_ClothSales_OrderCode_forSelection.Text) & "' , Last_modified_useridno = " & Str(Val(Common_Procedures.User.IdNo)) & ", Last_modified_DateTime = @modifieddatetime, Last_modified_DateTime_Text = '" & Trim(vMODIFIED_DTTM_TXT) & "'  Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Sizing_Pavu_Receipt_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If

            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "NEW", "Sizing_Pavu_Receipt_head", "Sizing_Pavu_Receipt_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Sizing_Pavu_Receipt_Code, Company_IdNo, for_OrderBy", tr)

            EntID = Trim(Pk_Condition) & Trim(lbl_RefNo.Text)
            Partcls = "Pavu Rcpt : Set No. " & Trim(txt_SetNo.Text)
            PBlNo = Trim(txt_SetNo.Text)

            cmd.CommandText = "Delete from Stock_Pavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Empty_BeamBagCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_SizedPavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Pavu_Delivery_Code = '' and Pavu_Delivery_Increment = 0 and Beam_Knotting_Code = '' and Production_Meters = 0 and Close_Status = 0"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Sizing_Pavu_Receipt_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Sizing_Pavu_Receipt_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Pavu_Delivery_Selections_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Truncate table " & Trim(Common_Procedures.EntryTempTable)
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Truncate table " & Trim(Common_Procedures.EntryTempSubTable)
            cmd.ExecuteNonQuery()

            StkAt_IdNo = Val(DelvTo_ID)

            With dgv_PavuDetails
                Sno = 0
                For I = 0 To dgv_PavuDetails.RowCount - 1

                    If Val(dgv_PavuDetails.Rows(I).Cells(DgvCol_details.METERS).Value) <> 0 Then

                        Sno = Sno + 1

                        Mtr_Pc = Format(Val(.Rows(I).Cells(DgvCol_details.MTR_PCS).Value), "#########0.00")

                        vDET_EdsCnt_ID = Common_Procedures.EndsCount_NameToIdNo(con, .Rows(I).Cells(DgvCol_details.ENDS_COUNT).Value, tr)
                        'pCnt_ID = Val(Common_Procedures.get_FieldValue(con, "EndsCount_Head", "Count_IdNo", "(EndsCount_IdNo = " & Str(Val(vDET_EdsCnt_ID)) & ")", , tr))
                        'pEds_Nm = Val(Common_Procedures.get_FieldValue(con, "EndsCount_Head", "Ends_Name", "(EndsCount_IdNo = " & Str(Val(vDET_EdsCnt_ID)) & ")", , tr))

                        Bw_ID = Common_Procedures.BeamWidth_NameToIdNo(con, .Rows(I).Cells(DgvCol_details.BEAM_WIDTH).Value, tr)
                        vLoomType_Idno = Common_Procedures.LoomType_NameToIdNo(con, .Rows(I).Cells(DgvCol_details.BEAM_TYPE).Value, tr)

                        pEds_Nm = 0
                        pCnt_ID = 0
                        Stock_In = ""
                        Da = New SqlClient.SqlDataAdapter("Select Ends_Name, Count_IdNo, Stock_In from EndsCount_Head Where EndsCount_IdNo = " & Str(Val(vDET_EdsCnt_ID)), con)
                        Da.SelectCommand.Transaction = tr
                        dt2 = New DataTable
                        Da.Fill(dt2)
                        If dt2.Rows.Count > 0 Then
                            pEds_Nm = dt2.Rows(0)("Ends_Name").ToString
                            pCnt_ID = dt2.Rows(0)("Count_IdNo").ToString
                            Stock_In = dt2.Rows(0)("Stock_In").ToString
                        End If
                        Da.Dispose()
                        dt2.Clear()
                        dt2.Dispose()

                        vSTKPAVUMTRS = .Rows(I).Cells(DgvCol_details.METERS).Value

                        If Trim(UCase(Stock_In)) = "WEIGHT" And dgv_PavuDetails.Columns(DgvCol_details.SIZING_WEIGHT).Visible = True Then

                            vSTKPAVUMTRS = .Rows(I).Cells(DgvCol_details.SIZING_WEIGHT).Value

                            If Val(vSTKPAVUMTRS) = 0 Then
                                Throw New ApplicationException("Invalid Sizing Weight." & Chr(13) & "This Endscount : " & Trim(.Rows(I).Cells(DgvCol_details.ENDS_COUNT).Value) & " , Stock is Maintained in  Weight, by Endscount Creation,Please Check it")
                                Exit Sub
                            End If

                        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1428" And Common_Procedures.settings.AutoLoom_PavuWidthWiseConsumption_IN_Delivery = 1 Then    '---- SAKTHI VINAYAGA TEXTILES  (ERODE-PALLIPALAYAM)
                            vSTKPAVUMTRS = .Rows(I).Cells(DgvCol_details.FABRIC_METERS).Value
                        End If

                        cmd.CommandText = "Insert into Sizing_Pavu_Receipt_Details (  Sizing_Pavu_Receipt_Code ,              Company_IdNo        , Sizing_Pavu_Receipt_No  ,    Sizing_Pavu_Receipt_RefNo  ,           for_OrderBy     , Sizing_Pavu_Receipt_Date ,          Ledger_IdNo     ,               Set_No          ,         Set_Code      ,             Sl_No    ,                    Beam_No                                  ,                      Pcs                                  ,          Meters_Pc       ,               Meters           ,                       SizedBeam_Meters                        ,                       Fabric_Meters                                    ,               EndsCount_IdNo      ,                      Sizing_Weight                                    , Beam_Width_IdNo     ,         LoomType_Idno ) " &
                                                        "    Values                ( '" & Trim(NewCode) & "'   , " & Str(Val(lbl_Company.Tag)) & ",  '" & Trim(vInvoNo) & "', '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(vOrdByNo)) & ",   @EntryDate             ,  " & Str(Val(Led_ID)) & ", '" & Trim(txt_SetNo.Text) & "', '" & Trim(vSetCd) & "', " & Str(Val(Sno)) & ", '" & Trim(.Rows(I).Cells(DgvCol_details.BEAM_NO).Value) & "', " & Str(Val(.Rows(I).Cells(DgvCol_details.PCS).Value)) & ",  " & Str(Val(Mtr_Pc)) & ",  " & Str(Val(vSTKPAVUMTRS)) & ",  " & Str(Val(.Rows(I).Cells(DgvCol_details.METERS).Value)) & ",  " & Str(Val(.Rows(I).Cells(DgvCol_details.FABRIC_METERS).Value)) & "  ,  " & Str(Val(vDET_EdsCnt_ID)) & " , " & Str(Val(.Rows(I).Cells(DgvCol_details.SIZING_WEIGHT).Value)) & "  , " & Val(Bw_ID) & "  ,  " & Str(Val(vLoomType_Idno)) & " ) "
                        cmd.ExecuteNonQuery()

                        Nr = 0
                        cmd.CommandText = "Update Stock_SizedPavu_Processing_Details set Reference_Date = @EntryDate, Sl_No = " & Str(Val(Sno)) & ", Ends_Name = '" & Trim(pEds_Nm) & "', count_idno = " & Str(Val(pCnt_ID)) & ", EndsCount_IdNo = " & Str(Val(vDET_EdsCnt_ID)) & ", Width_Type = '" & Trim(cbo_WidthType.Text) & "' , Mill_IdNo = " & Str(Val(vMILL_IdNo)) & ", Warp_LotNo = '" & Trim(txt_Warp_LotNo.Text) & "', Meters = " & Str(Val(vSTKPAVUMTRS)) & " , SizedBeam_Meters = " & Str(Val(.Rows(I).Cells(DgvCol_details.METERS).Value)) & ", Fabric_Meters = " & Str(Val(.Rows(I).Cells(DgvCol_details.FABRIC_METERS).Value)) & ", Weaving_JobCode_forSelection = '" & Trim(Weaver_Job_Code) & "' , Beam_Width_Idno  = " & Val(Bw_ID) & "  ,  LoomType_Idno =  " & Str(Val(vLoomType_Idno)) & " , ClothSales_OrderCode_forSelection  = '" & Trim(cbo_ClothSales_OrderCode_forSelection.Text) & "'  " &
                                                " Where Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Set_Code = '" & Trim(vSetCd) & "' and Beam_No = '" & Trim(.Rows(I).Cells(DgvCol_details.BEAM_NO).Value) & "' "
                        Nr = cmd.ExecuteNonQuery()

                        'cmd.CommandText = "Update Stock_SizedPavu_Processing_Details set Reference_Date = @EntryDate, Sl_No = " & Str(Val(Sno)) & ", Width_Type = '" & Trim(cbo_WidthType.Text) & "' , Mill_IdNo = " & Str(Val(vMILL_IdNo)) & ", Warp_LotNo = '" & Trim(txt_Warp_LotNo.Text) & "', Meters = " & Str(Val(vSTKPAVUMTRS)) & " , SizedBeam_Meters = " & Str(Val(.Rows(i).Cells(DgvCol_details.METERS).Value)) & ", Fabric_Meters = " & Str(Val(.Rows(i).Cells(DgvCol_details.FABRIC_METERS).Value)) & ", Weaving_JobCode_forSelection = '" & Trim(Weaver_Job_Code) & "' " &
                        '                    " Where Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Set_Code = '" & Trim(vSetCd) & "' and Beam_No = '" & Trim(.Rows(i).Cells(DgvCol_details.BEAM_NO).Value) & "'"
                        'Nr = cmd.ExecuteNonQuery()


                        If Nr = 0 Then
                            cmd.CommandText = "Insert into Stock_SizedPavu_Processing_Details (                     Reference_Code          ,              Company_IdNo        ,    Reference_No        ,     for_OrderBy           ,    Reference_Date,         Ledger_IdNo     ,           StockAt_IdNo      ,         Set_Code      ,           Set_No              ,    setcode_forSelection     ,      Ends_Name         ,     count_idno           ,               EndsCount_IdNo        ,              Mill_IdNo       , Beam_Width_Idno, Sizing_SlNo,         Sl_No        ,                                                        Beam_No             ,                                               ForOrderBy_BeamNo                   ,                           Gross_Weight, Tare_Weight, Net_Weight,                      Noof_Pcs                             ,          Meters_Pc      ,              Meters            ,                       SizedBeam_Meters    ,                       Fabric_Meters       ,                                                        Width_Type           ,               Warp_LotNo           ,         Weaving_JobCode_forSelection   ,               LoomType_Idno          ,             ClothSales_OrderCode_forSelection  ) " &
                                                            "    Values                           ( '" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(vInvoNo) & "', " & Str(Val(vOrdByNo)) & ",      @EntryDate  , " & Str(Val(Led_ID)) & ", " & Str(Val(StkAt_IdNo)) & ", '" & Trim(vSetCd) & "', '" & Trim(txt_SetNo.Text) & "', '" & Trim(Selc_SetCode) & "', '" & Trim(pEds_Nm) & "', " & Str(Val(pCnt_ID)) & ", " & Str(Val(vDET_EdsCnt_ID)) & ", " & Str(Val(vMILL_IdNo)) & " ,      " & Val(Bw_ID) & "      ,      0     , " & Str(Val(Sno)) & ", '" & Trim(.Rows(I).Cells(DgvCol_details.BEAM_NO).Value) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(.Rows(I).Cells(DgvCol_details.BEAM_NO).Value))) & " ,      0      ,       0    ,      0          , " & Str(Val(.Rows(I).Cells(DgvCol_details.PCS).Value)) & ", " & Str(Val(Mtr_Pc)) & ", " & Str(Val(vSTKPAVUMTRS)) & " ,  " & Str(Val(.Rows(I).Cells(DgvCol_details.METERS).Value)) & ",  " & Str(Val(.Rows(I).Cells(DgvCol_details.FABRIC_METERS).Value)) & ", '" & Trim(cbo_WidthType.Text) & "' , '" & Trim(txt_Warp_LotNo.Text) & "', '" & Trim(Weaver_Job_Code) & "'   ,  " & Str(Val(vLoomType_Idno)) & "  , '" & Trim(cbo_ClothSales_OrderCode_forSelection.Text) & "' ) "
                            cmd.ExecuteNonQuery()
                        End If

                        cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempTable) & "(Int1, Int2, Meters1, Weight1) Values (" & Str(Val(vDET_EdsCnt_ID)) & ", 1, " & Str(Val(.Rows(I).Cells(DgvCol_details.METERS).Value)) & ", " & Str(Val(vSTKPAVUMTRS)) & ")"
                        cmd.ExecuteNonQuery()

                        cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempSubTable) & "(Int1, Int2, Int3 ) Values (" & Str(Val(Bw_ID)) & "," & Str(Val(vLoomType_Idno)) & ", 1 )"
                        cmd.ExecuteNonQuery()

                    End If

                Next
                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "NEW", "Sizing_Pavu_Receipt_Details", "Sizing_Pavu_Receipt_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, " Beam_No,Pcs,Meters_Pc,Meters", "Sl_No", "Sizing_Pavu_Receipt_Code, For_OrderBy, Company_IdNo, Sizing_Pavu_Receipt_No, Sizing_Pavu_Receipt_Date, Ledger_Idno", tr)

            End With


            Dim Stk_DelvMtr As String = 0, Stk_RecMtr As String = 0
            Dim Delv_Ledtype As String = ""
            Dim Rec_Ledtype As String = ""
            Dim vPVUSTK_DelVID As Integer
            Dim vPvuBMS As String = 0
            Dim vPvuMtrs As String = 0
            Dim vSTKPVUQTY As String = 0

            Del_ID = Val(DelvTo_ID)
            Rec_ID = Val(Led_ID)

            Delv_Ledtype = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_Type", "(Ledger_IdNo = " & Str(Val(Del_ID)) & ")", , tr)
            Rec_Ledtype = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_Type", "(Ledger_IdNo = " & Str(Val(Rec_ID)) & ")", , tr)

            If Val(vTotPvuMtrs) <> 0 Then

                Da4 = New SqlClient.SqlDataAdapter("Select Int1 as endscount_idno, sum(int2) as pavu_beams , sum(Meters1) as pavu_meters , sum(Weight1) as pavu_stock_qty from  " & Trim(Common_Procedures.EntryTempTable) & " group by Int1 Having sum(Weight1) <> 0 ", con)
                Da4.SelectCommand.Transaction = tr
                Dt4 = New DataTable
                Da4.Fill(Dt4)
                If Dt4.Rows.Count > 0 Then
                    For K = 0 To Dt4.Rows.Count - 1

                        EdsCnt_ID = Val(Dt4.Rows(K).Item("endscount_idno").ToString)
                        vPvuBMS = Val(Dt4.Rows(K).Item("pavu_beams").ToString)
                        vPvuMtrs = Val(Dt4.Rows(K).Item("pavu_meters").ToString)
                        vSTKPVUQTY = Val(Dt4.Rows(K).Item("pavu_stock_qty").ToString)

                        Stock_In = ""
                        mtrspcs = 0
                        Da = New SqlClient.SqlDataAdapter("Select Stock_In , Meters_Pcs from EndsCount_Head Where EndsCount_IdNo = " & Str(Val(EdsCnt_ID)), con)
                        Da.SelectCommand.Transaction = tr
                        dt2 = New DataTable
                        Da.Fill(dt2)
                        If dt2.Rows.Count > 0 Then
                            Stock_In = dt2.Rows(0)("Stock_In").ToString
                            mtrspcs = Val(dt2.Rows(0)("Meters_Pcs").ToString)
                        End If
                        dt2.Clear()


                        vPVUSTK_DelVID = Del_ID
                        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1075" Then '---- JR TEX ( STANLEY ) ( MS FABRICS ) (SULUR)   (or)   J.R TEX ( STANLEY ) ( M.S FABRICS ) (SULUR)
                            If Trim(UCase(Delv_Ledtype)) = "JOBWORKER" Then
                                vPVUSTK_DelVID = 0
                            End If
                        End If

                        If (Trim(UCase(Stock_In)) = "PCS" Or Trim(UCase(Stock_In)) = "WEIGHT") Or Common_Procedures.settings.AutoLoom_PavuWidthWiseConsumption_IN_Delivery = 1 Then

                            Stk_DelvMtr = 0
                            Stk_RecMtr = 0
                            If Trim(UCase(Stock_In)) = "PCS" Then
                                If Val(mtrspcs) = 0 Then mtrspcs = 1
                                Stk_DelvMtr = Format(Val(vSTKPVUQTY) / mtrspcs, "#########0.00")
                                Stk_RecMtr = Format(Val(vSTKPVUQTY) / mtrspcs, "#########0.00")

                            Else

                                If Trim(UCase(Delv_Ledtype)) = "WEAVER" Then
                                    Stk_DelvMtr = Val(vSTKPVUQTY)
                                Else
                                    Stk_DelvMtr = Val(vPvuMtrs)
                                End If

                                If Trim(UCase(Rec_Ledtype)) = "WEAVER" Then
                                    Stk_RecMtr = Val(vSTKPVUQTY)
                                Else
                                    Stk_RecMtr = Val(vPvuMtrs)
                                End If

                            End If


                            If vPVUSTK_DelVID <> 0 Then
                                cmd.CommandText = "Insert into Stock_Pavu_Processing_Details (                    Reference_Code           ,                 Company_IdNo     ,           Reference_No ,          for_OrderBy      , Reference_Date,      DeliveryTo_Idno            , ReceivedFrom_Idno,        Entry_ID      ,     Party_Bill_No    ,        Particulars     ,           Sl_No      ,          EndsCount_IdNo    ,             Sized_Beam   ,               Meters        ,  DeliveryToIdno_ForParticulars ,    ReceivedFromIdno_ForParticulars,   Weaving_JobCode_forSelection  ,         Sizing_JobCode_forSelection        ,                ClothSales_OrderCode_forSelection          ) " &
                                " Values                                                     ( '" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(vInvoNo) & "', " & Str(Val(vOrdByNo)) & ",   @EntryDate  , " & Str(Val(vPVUSTK_DelVID)) & ",      0           , '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "',  " & Str(Val(K)) & " , " & Str(Val(EdsCnt_ID)) & ", " & Str(Val(vPvuBMS)) & ", " & Str(Val(Stk_DelvMtr)) & ",     " & Str(Val(Del_ID)) & "  ,         " & Str(Val(Rec_ID)) & "  ,  '" & Trim(Weaver_Job_Code) & "', '" & Trim(cbo_Sizing_JobCardNo.Text) & "' , '" & Trim(cbo_ClothSales_OrderCode_forSelection.Text) & "' ) "
                                cmd.ExecuteNonQuery()
                            End If

                            cmd.CommandText = "Insert into Stock_Pavu_Processing_Details (                    Reference_Code           ,                 Company_IdNo     ,           Reference_No ,          for_OrderBy      , Reference_Date, DeliveryTo_Idno ,     ReceivedFrom_Idno   ,        Entry_ID      ,     Party_Bill_No    ,        Particulars     ,           Sl_No          ,          EndsCount_IdNo    ,             Sized_Beam   ,               Meters        ,  DeliveryToIdno_ForParticulars ,    ReceivedFromIdno_ForParticulars ,  Weaving_JobCode_forSelection   ,         Sizing_JobCode_forSelection            ,                ClothSales_OrderCode_forSelection         ) " &
                            " Values                                                     ( '" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(vInvoNo) & "', " & Str(Val(vOrdByNo)) & ",   @EntryDate  ,       0         , " & Str(Val(Rec_ID)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "',  " & Str(Val(K + 200)) & " , " & Str(Val(EdsCnt_ID)) & ", " & Str(Val(vPvuBMS)) & ", " & Str(Val(Stk_RecMtr)) & ",     " & Str(Val(Del_ID)) & "   ,         " & Str(Val(Rec_ID)) & "   , '" & Trim(Weaver_Job_Code) & "' , '" & Trim(cbo_Sizing_JobCardNo.Text) & "' , '" & Trim(cbo_ClothSales_OrderCode_forSelection.Text) & "'  ) "
                            cmd.ExecuteNonQuery()

                        Else

                            cmd.CommandText = "Insert into Stock_Pavu_Processing_Details (                    Reference_Code           ,                 Company_IdNo     ,           Reference_No ,          for_OrderBy      , Reference_Date,           DeliveryTo_Idno       ,     ReceivedFrom_Idno   ,        Entry_ID      ,     Party_Bill_No    ,        Particulars     ,            Sl_No      ,          EndsCount_IdNo    ,             Sized_Beam   ,               Meters      ,  DeliveryToIdno_ForParticulars ,    ReceivedFromIdno_ForParticulars ,    Weaving_JobCode_forSelection,         Sizing_JobCode_forSelection       ,                  ClothSales_OrderCode_forSelection        ) " &
                            "                     Values                                 ( '" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(vInvoNo) & "', " & Str(Val(vOrdByNo)) & ",   @EntryDate  , " & Str(Val(vPVUSTK_DelVID)) & ", " & Str(Val(Rec_ID)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "',   " & Str(Val(K)) & " , " & Str(Val(EdsCnt_ID)) & ", " & Str(Val(vPvuBMS)) & ", " & Str(Val(vPvuMtrs)) & ",     " & Str(Val(Del_ID)) & "   ,         " & Str(Val(Rec_ID)) & "   , '" & Trim(Weaver_Job_Code) & "', '" & Trim(cbo_Sizing_JobCardNo.Text) & "' , '" & Trim(cbo_ClothSales_OrderCode_forSelection.Text) & "') "
                            cmd.ExecuteNonQuery()

                        End If



                    Next

                End If
                Dt4.Clear()

            End If


            If Common_Procedures.settings.Textile_Sizing_Delivery_receipt_Selection = 1 And Trim(cbo_Type.Text) = "DELIVERY" And Trim(vSELC_DCCODE) <> "" Then
                If Val(vTotPvuMtrs) <> 0 Then
                    cmd.CommandText = "Insert into Pavu_Delivery_Selections_Processing_Details (                 Reference_Code             ,             Company_IdNo         , Reference_No                  ,          for_OrderBy      , Reference_Date,         Delivery_Code       ,             Delivery_No           ,       DeliveryTo_Idno      ,      ReceivedFrom_Idno  ,            Party_Dc_No            , Beam_Width_IdNo,       Total_Beams                , Total_Pcs,           Total_Meters             ) " &
                    "           Values                                     ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(vOrdByNo)) & ",   @EntryDate  , '" & Trim(vSELC_DCCODE) & "', '" & Trim(txt_PartyDcNo.Text) & "', " & Str(Val(DelvTo_ID)) & ", " & Str(Val(Rec_ID)) & ", '" & Trim(txt_PartyDcNo.Text) & "',     0          , " & Str(-1 * Val(vTotPvuBms)) & ",     0    , " & Str(-1 * Val(vTotPvuMtrs)) & " ) "
                    cmd.ExecuteNonQuery()
                End If
            End If


            vLoomType_Id_Stck_pst_frstlne = 0
            Bw_Id_Stck_pst_frstlne = 0

            If txt_TotalPavu.Visible And Val(txt_TotalPavu.Text) <> 0 Then


                Bw_Id_Stck_pst_frstlne = Common_Procedures.BeamWidth_NameToIdNo(con, dgv_PavuDetails.Rows(0).Cells(DgvCol_details.BEAM_WIDTH).Value)
                vLoomType_Id_Stck_pst_frstlne = Common_Procedures.LoomType_NameToIdNo(con, dgv_PavuDetails.Rows(0).Cells(DgvCol_details.BEAM_TYPE).Value)

                'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1018" Then '---- M.K Textiles (Palladam)
                cmd.CommandText = "Insert into Stock_Empty_BeamBagCone_Processing_Details ( Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Entry_ID, Party_Bill_No, Particulars, Sl_No, Beam_Width_IdNo, Pavu_Beam  , LoomType_Idno) Values ( '" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(vInvoNo) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ", @EntryDate, " & Str(Val(Del_ID)) & ", 0, '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', 2, " & Str(Val(Bw_Id_Stck_pst_frstlne)) & ", " & Str(Val(Val(txt_TotalPavu.Text))) & " , " & Str(Val(vLoomType_Id_Stck_pst_frstlne)) & ")"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Insert into Stock_Empty_BeamBagCone_Processing_Details ( Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Entry_ID, Party_Bill_No, Particulars, Sl_No, Beam_Width_IdNo, Empty_Beam  , LoomType_Idno) Values ( '" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(vInvoNo) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ", @EntryDate, 0, " & Str(Val(Rec_ID)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', 3, " & Str(Val(Bw_Id_Stck_pst_frstlne)) & ", " & Str(Val(Val(txt_TotalPavu.Text))) & " , " & Str(Val(vLoomType_Id_Stck_pst_frstlne)) & ")"
                cmd.ExecuteNonQuery()

                'Else
                '    cmd.CommandText = "Insert into Stock_Empty_BeamBagCone_Processing_Details ( Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Entry_ID, Party_Bill_No, Particulars, Sl_No, Beam_Width_IdNo, Pavu_Beam , Empty_Beam , Empty_Bobin ) Values ( '" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ", @EntryDate, " & Str(Val(Del_ID)) & ", " & Str(Val(Rec_ID)) & " , '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', 2, 0, " & Str(Val(Empty_Bms)) & "," & Str(Val(Empty_Bms)) & " , " & Str(Val(txt_TotalBobin.Text)) & ")"
                '    Nr = cmd.ExecuteNonQuery()
                'End If

            Else

                If Val(vTotPvuBms) <> 0 Then

                    Dim vLOOMTYPE_ID = 0
                    Dim vBEAMWIDTH_ID = 0
                    Dim vTOTBMS = 0

                    Da5 = New SqlClient.SqlDataAdapter("Select Int1 as Widthtype_idno, int2 as LoomType_Idno , Sum(int3) as pavu_beams  from  " & Trim(Common_Procedures.EntryTempSubTable) & " group by Int1 , Int2  Having sum(int3) <> 0 ORDER BY INT1 ", con)
                    Da5.SelectCommand.Transaction = tr
                    Dt5 = New DataTable
                    Da5.Fill(Dt5)

                    vLOOMTYPE_ID = 0
                    vBEAMWIDTH_ID = 0
                    vTOTBMS = 0

                    If Dt5.Rows.Count > 0 Then

                        For L = 0 To Dt5.Rows.Count - 1

                            vBEAMWIDTH_ID = Val(Dt5.Rows(L).Item("Widthtype_idno").ToString)
                            vLOOMTYPE_ID = Val(Dt5.Rows(L).Item("LoomType_Idno").ToString)
                            vTOTBMS = Val(Dt5.Rows(L).Item("pavu_beams").ToString)

                            cmd.CommandText = "Insert into Stock_Empty_BeamBagCone_Processing_Details ( Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Entry_ID, Party_Bill_No, Particulars, Sl_No, Beam_Width_IdNo, Pavu_Beam   , LoomType_Idno ) Values ( '" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(vInvoNo) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ", @EntryDate, " & Str(Val(Del_ID)) & ", 0, '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', " & Val(L) & "," & Str(Val(vBEAMWIDTH_ID)) & " , " & Str(Val(vTOTBMS)) & " ," & Str(Val(vLOOMTYPE_ID)) & ")"
                            cmd.ExecuteNonQuery()

                            cmd.CommandText = "Insert into Stock_Empty_BeamBagCone_Processing_Details ( Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Entry_ID, Party_Bill_No, Particulars, Sl_No, Beam_Width_IdNo, Empty_Beam  , LoomType_Idno ) Values ( '" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(vInvoNo) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ", @EntryDate, 0, " & Str(Val(Rec_ID)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', " & Val(L + 200) & ", " & Str(Val(vBEAMWIDTH_ID)) & ", " & Str(Val(vTOTBMS)) & " , " & Str(Val(vLOOMTYPE_ID)) & ")"
                            cmd.ExecuteNonQuery()

                        Next L
                    End If

                End If
            End If

            ' --- command  date 2024-11-12

            'Empty_Bms = 0
            'If Common_Procedures.settings.Weaver_Zari_Kuri_Entries_Status = 1 Then
            'Empty_Bms = Val(txt_TotalPavu.Text)


            'Else
            '    Empty_Bms = Val(vTotPvuBms)
            'End If

            'If Val(Empty_Bms) <> 0 Or Val(txt_TotalBobin.Text) <> 0 Then
            '    'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1018" Then '---- M.K Textiles (Palladam)
            '    cmd.CommandText = "Insert into Stock_Empty_BeamBagCone_Processing_Details ( Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Entry_ID, Party_Bill_No, Particulars, Sl_No, Beam_Width_IdNo, Pavu_Beam ) Values ( '" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(vInvoNo) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ", @EntryDate, " & Str(Val(Del_ID)) & ", 0, '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', 2, 0, " & Str(Val(Empty_Bms)) & ")"
            '    cmd.ExecuteNonQuery()

            '    cmd.CommandText = "Insert into Stock_Empty_BeamBagCone_Processing_Details ( Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Entry_ID, Party_Bill_No, Particulars, Sl_No, Beam_Width_IdNo, Empty_Beam ) Values ( '" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(vInvoNo) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ", @EntryDate, 0, " & Str(Val(Rec_ID)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', 3, 0, " & Str(Val(Empty_Bms)) & ")"
            '    cmd.ExecuteNonQuery()
            '    'Else
            '    '    cmd.CommandText = "Insert into Stock_Empty_BeamBagCone_Processing_Details ( Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Entry_ID, Party_Bill_No, Particulars, Sl_No, Beam_Width_IdNo, Pavu_Beam , Empty_Beam , Empty_Bobin ) Values ( '" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ", @EntryDate, " & Str(Val(Del_ID)) & ", " & Str(Val(Rec_ID)) & " , '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', 2, 0, " & Str(Val(Empty_Bms)) & "," & Str(Val(Empty_Bms)) & " , " & Str(Val(txt_TotalBobin.Text)) & ")"
            '    '    Nr = cmd.ExecuteNonQuery()
            '    'End If

            'End If

            Dim vVou_LedIdNos As String = "", vVou_Amts As String = "", vVou_ErrMsg As String = ""

            vVou_LedIdNos = Trans_ID & "|" & Val(Common_Procedures.CommonLedger.Transport_Charges_Ac)
            vVou_Amts = Val(txt_Freight.Text) & "|" & -1 * Val(txt_Freight.Text)
            If Common_Procedures.Voucher_Updation(con, "Siz.Pavu.Rec", Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), Trim(vInvoNo), Convert.ToDateTime(msk_date.Text), Partcls, vVou_LedIdNos, vVou_Amts, vVou_ErrMsg, tr, Common_Procedures.SoftwareTypes.Textile_Software) = False Then
                Throw New ApplicationException(vVou_ErrMsg)
            End If

            tr.Commit()

            Dt1.Dispose()
            Da.Dispose()



            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)

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
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()

        End Try

    End Sub


    Private Sub dtp_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Date.KeyDown
        If e.KeyCode = 40 Then e.Handled = True : SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then
            e.Handled = True

            'dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(0)
            'dgv_YarnDetails.CurrentCell.Selected = True
            'dgv_YarnDetails.Focus()

            ' SendKeys.Send("+{TAB}")
        End If

    End Sub

    Private Sub cbo_Sizing_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Sizing.GotFocus
        If Common_Procedures.settings.Textile_Sizing_Delivery_receipt_Selection = 1 Then
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(ledger_idno = 0 or Ledger_Type = 'SIZING' or Ledger_Type = 'GODOWN' or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")
        Else
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(ledger_idno = 0 or Ledger_Type = 'SIZING' or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")
        End If

    End Sub

    Private Sub cbo_Sizing_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Sizing.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        If Common_Procedures.settings.Textile_Sizing_Delivery_receipt_Selection = 1 Then
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Sizing, msk_date, txt_PartyDcNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(ledger_idno = 0 or Ledger_Type = 'GODOWN' or Ledger_Type = 'SIZING'  or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")
        Else
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Sizing, msk_date, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(ledger_idno = 0 or Ledger_Type = 'SIZING'  or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")
        End If

        If (e.KeyCode = 40 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

            txt_PartyDcNo.Focus()
        End If




    End Sub

    Private Sub cbo_Sizing_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Sizing.KeyPress
        If Common_Procedures.settings.Textile_Sizing_Delivery_receipt_Selection = 1 Then
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Sizing, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(ledger_idno = 0  or Ledger_Type = 'GODOWN' or Ledger_Type = 'SIZING'  or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")

        Else
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Sizing, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(ledger_idno = 0 or Ledger_Type = 'SIZING'  or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")

        End If

        If Asc(e.KeyChar) = 13 Then

            If Common_Procedures.settings.Internal_Order_Entry_Status = 1 Then
                If MessageBox.Show("Do you want to select Internal Order:", "FOR INTERNAL ORDER SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                    btn_Selection_Click(sender, e)

                Else
                    txt_PartyDcNo.Focus()

                End If

            ElseIf cbo_Type.Visible = True Then

                cbo_Type.Focus()
            Else
                txt_PartyDcNo.Focus()

            End If

        End If

    End Sub

    Private Sub cbo_Sizing_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Sizing.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Common_Procedures.UR.Ledr_Wea_Siz_Rw_Trans_JbWrk_Creation = Common_Procedures.UR.Ledger_Creation
            Common_Procedures.MDI_LedType = "SIZING"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Sizing.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_DeliveryTo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_DeliveryTo.GotFocus
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1019" Then '---- SUBHAM Textiles (Somanur)
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'GODOWN')", "(Ledger_IdNo = 0)")
        Else
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'WEAVER' or Ledger_Type = 'GODOWN'  or Ledger_Type = 'JOBWORKER' or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")
        End If

    End Sub

    Private Sub cbo_DeliveryTo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DeliveryTo.KeyDown
        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_DeliveryTo, txt_SetNo, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'WEAVER' or Ledger_Type = 'GODOWN'  or Ledger_Type = 'JOBWORKER' or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")

        If (e.KeyValue = 40 And cbo_DeliveryTo.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If cbo_Sizing_JobCardNo.Enabled And cbo_Sizing_JobCardNo.Visible = True Then
                cbo_Sizing_JobCardNo.Focus()
            Else
                cbo_TransportName.Focus()
                'cbo_EndsCount.Focus()
            End If
        End If

    End Sub

    Private Sub cbo_DeliveryTo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_DeliveryTo.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_DeliveryTo, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'WEAVER' or Ledger_Type = 'GODOWN'  or Ledger_Type = 'JOBWORKER' or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then
            If cbo_Sizing_JobCardNo.Enabled And cbo_Sizing_JobCardNo.Visible = True Then
                cbo_Sizing_JobCardNo.Focus()
            Else
                cbo_TransportName.Focus()

                'cbo_EndsCount.Focus()
            End If
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

    Private Sub cbo_EndsCount_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_EndsCount.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")
    End Sub

    Private Sub cbo_EndsCount_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_EndsCount.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_EndsCount, Nothing, Nothing, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")
        If (e.KeyValue = 40 And cbo_EndsCount.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

            If txt_Freight_For.Enabled = True And txt_Freight_For.Visible = True Then
                txt_Freight_For.Focus()
            ElseIf cbo_Weaving_JobCardNo.Enabled = True And cbo_Weaving_JobCardNo.Visible = True Then
                cbo_Weaving_JobCardNo.Focus()
            Else
                cbo_TransportName.Focus()
            End If

        End If

        If (e.KeyValue = 38 And cbo_EndsCount.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
            If cbo_Sizing_JobCardNo.Enabled And cbo_Sizing_JobCardNo.Visible = True Then
                cbo_Sizing_JobCardNo.Focus()
            Else
                cbo_DeliveryTo.Focus()
            End If
        End If

    End Sub

    Private Sub cbo_EndsCountName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_EndsCount.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_EndsCount, Nothing, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            If txt_Freight_For.Enabled = True And txt_Freight_For.Visible = True Then
                txt_Freight_For.Focus()
            ElseIf cbo_Weaving_JobCardNo.Enabled = True And cbo_Weaving_JobCardNo.Visible = True Then
                cbo_Weaving_JobCardNo.Focus()
            Else
                cbo_TransportName.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_EndsCount_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_EndsCount.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New EndsCount_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_EndsCount.Name
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

    Private Sub btn_Filter_Show_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter_Show.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim n As Integer, i As Integer
        Dim Led_IdNo As Integer, Cnt_IdNo As Integer
        Dim Condt As String = ""
        Dim EdsCnt_IdNo As Integer, Mil_IdNo As Integer
        Dim NewCode As String = ""

        Try

            Condt = ""
            Led_IdNo = 0
            Cnt_IdNo = 0
            Mil_IdNo = 0
            EdsCnt_IdNo = 0
            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Sizing_Pavu_Receipt_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Sizing_Pavu_Receipt_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Sizing_Pavu_Receipt_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If


            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If



            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.Ledger_IdNo = " & Str(Val(Led_IdNo))
            End If

            If Val(Cnt_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Sizing_Pavu_Receipt_Code IN (select z1.Sizing_Pavu_Receipt_Code from Sizing_SpecificationYarn_Details z1 where z1.Count_IdNo = " & Str(Val(Cnt_IdNo)) & ")"
            End If

            If Val(Mil_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Sizing_Pavu_Receipt_Code IN (select z1.Sizing_Pavu_Receipt_Code from Sizing_SpecificationYarn_Details z1 where z1.Mill_IdNo = " & Str(Val(Mil_IdNo)) & ")"
            End If

            If Val(EdsCnt_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.EndsCount_IdNo = " & Str(Val(EdsCnt_IdNo))
            End If

            If Trim(txt_Filter_SetNo.Text) <> "" Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Set_No = '" & Trim(txt_Filter_SetNo.Text) & "'"
            End If


            If cbo_Verified_Sts.Visible = True And Trim(cbo_Verified_Sts.Text) <> "" Then

                If Trim(cbo_Verified_Sts.Text) = "YES" Then
                    Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Verified_Status = 1 "
                ElseIf Trim(cbo_Verified_Sts.Text) = "NO" Then
                    Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Verified_Status = 0 "
                End If

            End If

            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name as SizingName , c.*  from Sizing_Pavu_Receipt_Head a INNER JOIN Ledger_Head b on a.Ledger_IdNo = b.Ledger_IdNo LEFT OUTER JOIN EndsCount_Head c ON c.EndsCount_IdNo = a.EndsCount_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Sizing_Pavu_Receipt_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.Sizing_Pavu_Receipt_date, a.for_orderby, a.Sizing_Pavu_Receipt_RefNo", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(DgvFilter_ColDetails.SL_NO).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(DgvFilter_ColDetails.REC_NO).Value = dt2.Rows(i).Item("Sizing_Pavu_Receipt_RefNo").ToString
                    dgv_Filter_Details.Rows(n).Cells(DgvFilter_ColDetails.SET_NO).Value = dt2.Rows(i).Item("Set_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(DgvFilter_ColDetails.REC_DATE).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Sizing_Pavu_Receipt_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(DgvFilter_ColDetails.PARTY_NAME).Value = dt2.Rows(i).Item("SizingName").ToString
                    dgv_Filter_Details.Rows(n).Cells(DgvFilter_ColDetails.ENDS_COUNT).Value = dt2.Rows(i).Item("EndsCount_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(DgvFilter_ColDetails.NO_OF_BEAMS).Value = Val(dt2.Rows(i).Item("Total_Beam").ToString)
                    dgv_Filter_Details.Rows(n).Cells(DgvFilter_ColDetails.TOTAL_METERS).Value = Format(Val(dt2.Rows(i).Item("Total_Meters").ToString), "########0.00")

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
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_IdNo = 0 or Ledger_Type = 'SIZING' or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")
    End Sub


    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, dtp_Filter_ToDate, cbo_Filter_EndsCount, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_IdNo = 0 or Ledger_Type = 'SIZING' )", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, cbo_Filter_EndsCount, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_IdNo = 0 or Ledger_Type = 'SIZING' )", "(Ledger_IdNo = 0)")

    End Sub


    Private Sub Open_FilterEntry()
        Dim movno As String

        On Error Resume Next

        movno = Trim(dgv_Filter_Details.CurrentRow.Cells(DgvFilter_ColDetails.REC_NO).Value)

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

    Private Sub dgv_PavuDetails_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_PavuDetails.CellEndEdit
        dgv_PavuDetails_CellLeave(sender, e)
    End Sub

    Private Sub dgv_PavuDetails_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_PavuDetails.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim rect As Rectangle

        With dgv_PavuDetails

            dgv_ActiveCtrl_Name = .Name

            If Val(.CurrentRow.Cells(DgvCol_details.SLNO).Value) = 0 Then
                .CurrentRow.Cells(DgvCol_details.SLNO).Value = .CurrentRow.Index + 1
            End If

            If e.RowIndex > 0 And e.ColumnIndex = DgvCol_details.BEAM_NO Then
                If Val(.CurrentRow.Cells(DgvCol_details.BEAM_NO).Value) = 0 And e.RowIndex = .RowCount - 1 Then
                    If Val(.Rows(e.RowIndex - 1).Cells(DgvCol_details.BEAM_NO).Value) <> 0 Then
                        .CurrentRow.Cells(DgvCol_details.BEAM_NO).Value = Val(.Rows(e.RowIndex - 1).Cells(DgvCol_details.BEAM_NO).Value) + 1
                        .CurrentRow.Cells(DgvCol_details.ENDS_COUNT).Value = Trim(.Rows(e.RowIndex - 1).Cells(DgvCol_details.ENDS_COUNT).Value)
                        .CurrentRow.Cells(DgvCol_details.PCS).Value = .Rows(e.RowIndex - 1).Cells(DgvCol_details.PCS).Value
                        .CurrentRow.Cells(DgvCol_details.MTR_PCS).Value = .Rows(e.RowIndex - 1).Cells(DgvCol_details.MTR_PCS).Value
                        .CurrentRow.Cells(DgvCol_details.METERS).Value = .Rows(e.RowIndex - 1).Cells(DgvCol_details.METERS).Value

                        .CurrentRow.Cells(DgvCol_details.BEAM_WIDTH).Value = .Rows(e.RowIndex - 1).Cells(DgvCol_details.BEAM_WIDTH).Value
                        .CurrentRow.Cells(DgvCol_details.BEAM_TYPE).Value = .Rows(e.RowIndex - 1).Cells(DgvCol_details.BEAM_TYPE).Value


                    End If
                End If
                If e.ColumnIndex = DgvCol_details.BEAM_NO And e.RowIndex = .RowCount - 1 And Val(.CurrentRow.Cells(DgvCol_details.PCS).Value) = 0 And Val(.CurrentRow.Cells(DgvCol_details.MTR_PCS).Value) = 0 Then
                    If Val(.Rows(e.RowIndex - 1).Cells(DgvCol_details.BEAM_NO).Value) <> 0 Then
                        .CurrentRow.Cells(DgvCol_details.BEAM_NO).Value = Val(.Rows(e.RowIndex - 1).Cells(DgvCol_details.BEAM_NO).Value) + 1
                        .CurrentRow.Cells(DgvCol_details.ENDS_COUNT).Value = Trim(.Rows(e.RowIndex - 1).Cells(DgvCol_details.ENDS_COUNT).Value)
                        .CurrentRow.Cells(DgvCol_details.PCS).Value = .Rows(e.RowIndex - 1).Cells(DgvCol_details.PCS).Value
                        .CurrentRow.Cells(DgvCol_details.MTR_PCS).Value = .Rows(e.RowIndex - 1).Cells(DgvCol_details.MTR_PCS).Value
                        .CurrentRow.Cells(DgvCol_details.METERS).Value = .Rows(e.RowIndex - 1).Cells(DgvCol_details.METERS).Value

                        .CurrentRow.Cells(DgvCol_details.BEAM_WIDTH).Value = .Rows(e.RowIndex - 1).Cells(DgvCol_details.BEAM_WIDTH).Value
                        .CurrentRow.Cells(DgvCol_details.BEAM_TYPE).Value = .Rows(e.RowIndex - 1).Cells(DgvCol_details.BEAM_TYPE).Value

                    End If
                End If
            End If

            If e.ColumnIndex = DgvCol_details.ENDS_COUNT Then

                If .CurrentCell.RowIndex > 0 And Trim(.CurrentRow.Cells(DgvCol_details.ENDS_COUNT).Value) = "" Then
                    .CurrentRow.Cells(DgvCol_details.ENDS_COUNT).Value = Trim(.Rows(e.RowIndex - 1).Cells(DgvCol_details.ENDS_COUNT).Value)
                End If
                If Val(dgv_PavuDetails.Rows(dgv_PavuDetails.CurrentCell.RowIndex).Cells(DgvCol_details.STS).Value) <> 0 Then
                    cbo_Grid_EndsCount.Enabled = True ' False
                    cbo_Grid_EndsCount.BackColor = Color.LightGray
                Else
                    cbo_Grid_EndsCount.Enabled = True
                    cbo_Grid_EndsCount.BackColor = Color.White
                End If

                If cbo_Grid_EndsCount.Visible = False Or Val(cbo_Grid_EndsCount.Tag) <> e.RowIndex Then

                    cbo_Grid_EndsCount.Tag = -1

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_EndsCount.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_Grid_EndsCount.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top

                    cbo_Grid_EndsCount.Width = rect.Width  ' .CurrentCell.Size.Width
                    cbo_Grid_EndsCount.Height = rect.Height  ' rect.Height
                    cbo_Grid_EndsCount.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_Grid_EndsCount.Tag = Val(e.RowIndex)
                    cbo_Grid_EndsCount.Visible = True

                    cbo_Grid_EndsCount.BringToFront()
                    cbo_Grid_EndsCount.Focus()


                End If

            Else

                cbo_Grid_EndsCount.Visible = False

            End If


            If e.ColumnIndex = DgvCol_details.BEAM_WIDTH Then

                If cbo_Grid_Beam_width.Visible = False Or Val(cbo_Grid_Beam_width.Tag) <> e.RowIndex Then

                    'dgv_ActCtrlName = dgv_PavuDetails.Name

                    cbo_Grid_Beam_width.Tag = -1
                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_Beam_width.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_Grid_Beam_width.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                    cbo_Grid_Beam_width.Width = rect.Width  ' .CurrentCell.Size.Width
                    cbo_Grid_Beam_width.Height = rect.Height  ' rect.Height

                    cbo_Grid_Beam_width.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_Grid_Beam_width.Tag = Val(e.RowIndex)
                    cbo_Grid_Beam_width.Visible = True

                    cbo_Grid_Beam_width.BringToFront()
                    cbo_Grid_Beam_width.Focus()

                End If

            Else

                cbo_Grid_Beam_width.Visible = False

            End If

            If e.ColumnIndex = DgvCol_details.BEAM_TYPE Then

                If cbo_Grid_BeamType.Visible = False Or Val(cbo_Grid_BeamType.Tag) <> e.RowIndex Then

                    'dgv_ActCtrlName = dgv_PavuDetails.Name

                    cbo_Grid_BeamType.Tag = -1
                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_BeamType.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_Grid_BeamType.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                    cbo_Grid_BeamType.Width = rect.Width  ' .CurrentCell.Size.Width
                    cbo_Grid_BeamType.Height = rect.Height  ' rect.Height

                    cbo_Grid_BeamType.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_Grid_BeamType.Tag = Val(e.RowIndex)
                    cbo_Grid_BeamType.Visible = True

                    cbo_Grid_BeamType.BringToFront()
                    cbo_Grid_BeamType.Focus()

                End If

            Else

                cbo_Grid_BeamType.Visible = False

            End If


        End With
    End Sub

    Private Sub dgv_PavuDetails_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_PavuDetails.CellLeave
        With dgv_PavuDetails
            If .CurrentCell.ColumnIndex = DgvCol_details.METERS Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    If Common_Procedures.settings.Weaver_Zari_Kuri_Entries_Status = 1 Then
                        .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.000")
                    Else
                        .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")

                    End If
                Else
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
                End If
            End If
        End With
    End Sub

    Private Sub dgv_PavuDetails_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_PavuDetails.CellValueChanged
        Try

            If FrmLdSTS = True Then Exit Sub
            If MovSTS = True Then Exit Sub

            If IsNothing(dgv_PavuDetails.CurrentCell) Then Exit Sub

            With dgv_PavuDetails
                If .Visible Then
                    If e.ColumnIndex = DgvCol_details.PCS Or e.ColumnIndex = DgvCol_details.MTR_PCS Then
                        If Common_Procedures.settings.Weaver_Zari_Kuri_Entries_Status = 1 Then
                            .CurrentRow.Cells(DgvCol_details.METERS).Value = Format(Val(.CurrentRow.Cells(DgvCol_details.PCS).Value) * Val(.CurrentRow.Cells(DgvCol_details.MTR_PCS).Value), "#########0.000")
                        Else
                            .CurrentRow.Cells(DgvCol_details.METERS).Value = Format(Val(.CurrentRow.Cells(DgvCol_details.PCS).Value) * Val(.CurrentRow.Cells(DgvCol_details.MTR_PCS).Value), "#########0.00")
                        End If
                    End If

                    If e.ColumnIndex = DgvCol_details.PCS Or e.ColumnIndex = DgvCol_details.METERS Or (e.ColumnIndex = DgvCol_details.ENDS_COUNT And chk_Calculate_Sizing_Wgt_Auto.Checked = True) Then
                        TotalPavu_Calculation()
                    End If
                    If (.CurrentCell.ColumnIndex = DgvCol_details.PCS Or .CurrentCell.ColumnIndex = DgvCol_details.METERS) And Val(.CurrentCell.Value) <> 0 Then
                        If .CurrentRow.Index = .Rows.Count - 1 Then
                            .Rows.Add()
                        End If
                    End If

                End If
            End With

        Catch ex As Exception
            '----

        End Try

    End Sub


    Private Sub dgv_PavuDetails_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_PavuDetails.EditingControlShowing
        dgtxt_PavuDetails = CType(dgv_PavuDetails.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgv_PavuDetails_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_PavuDetails.GotFocus
        '--
    End Sub

    Private Sub dgv_PavuDetails_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_PavuDetails.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
    End Sub

    Private Sub dgv_PavuDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_PavuDetails.KeyUp
        Dim n As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_PavuDetails

                If Val(.Rows(.CurrentRow.Index).Cells(DgvCol_details.STS).Value) = 0 Then

                    n = .CurrentRow.Index

                    If .Rows.Count = 1 Then
                        For i = 0 To .Columns.Count - 1
                            .Rows(n).Cells(i).Value = ""
                        Next

                    Else

                        .Rows.RemoveAt(n)

                    End If

                    TotalPavu_Calculation()

                Else
                    MessageBox.Show("Already Pavu delivered", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub

                End If

            End With

        End If

    End Sub

    Private Sub dgv_PavuDetails_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_PavuDetails.LostFocus
        On Error Resume Next
        If Not IsNothing(dgv_PavuDetails.CurrentCell) Then dgv_PavuDetails.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_PavuDetails_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_PavuDetails.RowsAdded
        Dim n As Integer
        If IsNothing(dgv_PavuDetails.CurrentCell) Then Exit Sub
        With dgv_PavuDetails
            n = .RowCount
            .Rows(n - 1).Cells(DgvCol_details.SLNO).Value = Val(n)
        End With
    End Sub

    Private Sub TotalPavu_Calculation()
        Dim Sno As Integer
        Dim TotBms As Single, TotPcs As Single, TotMtrs As Single
        Dim vFABMTRS As String, vTOT_FABMTRS As String, vSIZING_WGT As String, vTOT_SIZING_WGT As String

        If FrmLdSTS = True Then Exit Sub

        Sno = 0
        TotBms = 0
        TotPcs = 0
        TotMtrs = 0
        vTOT_FABMTRS = 0
        vSIZING_WGT = 0
        vTOT_SIZING_WGT = 0
        With dgv_PavuDetails
            For i = 0 To .RowCount - 1
                Sno = Sno + 1
                .Rows(i).Cells(DgvCol_details.SLNO).Value = Sno
                vFABMTRS = 0
                If Val(.Rows(i).Cells(DgvCol_details.METERS).Value) <> 0 Then
                    TotBms = TotBms + 1
                    TotPcs = TotPcs + Val(.Rows(i).Cells(DgvCol_details.PCS).Value)
                    TotMtrs = TotMtrs + Val(.Rows(i).Cells(DgvCol_details.METERS).Value)
                    vFABMTRS = Calculate_Fabric_Meters(Val(.Rows(i).Cells(DgvCol_details.METERS).Value))

                    If chk_Calculate_Sizing_Wgt_Auto.Checked = True Then
                        vSIZING_WGT = Sizing_Weight_Calculation_AutoMatic(vCurrow:=i, Val(.Rows(i).Cells(DgvCol_details.METERS).Value))
                    Else
                        vSIZING_WGT = Val(.Rows(i).Cells(DgvCol_details.SIZING_WEIGHT).Value)
                    End If

                    vTOT_SIZING_WGT = Format(Val(vTOT_SIZING_WGT) + Val(vSIZING_WGT), "##########0.000")

                End If
                .Rows(i).Cells(DgvCol_details.FABRIC_METERS).Value = vFABMTRS
                vTOT_FABMTRS = Format(Val(vTOT_FABMTRS) + Val(.Rows(i).Cells(DgvCol_details.FABRIC_METERS).Value), "##########0.00")

                '.Rows(i).Cells(DgvCol_details.SIZING_WEIGHT).Value = vSIZING_WGT
                'vTOT_SIZING_WGT = Format(Val(vTOT_SIZING_WGT) + Val(.Rows(i).Cells(DgvCol_details.SIZING_WEIGHT).Value), "##########0.000")

            Next
        End With

        With dgv_PavuDetails_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(DgvCol_details.BEAM_NO).Value = Val(TotBms)
            .Rows(0).Cells(DgvCol_details.PCS).Value = Val(TotPcs)
            If Common_Procedures.settings.Weaver_Zari_Kuri_Entries_Status = 1 Then
                .Rows(0).Cells(DgvCol_details.METERS).Value = Format(Val(TotMtrs), "########0.000")
            Else
                .Rows(0).Cells(DgvCol_details.METERS).Value = Format(Val(TotMtrs), "########0.00")
            End If
            .Rows(0).Cells(DgvCol_details.FABRIC_METERS).Value = Format(Val(vTOT_FABMTRS), "########0.00")

            .Rows(0).Cells(DgvCol_details.SIZING_WEIGHT).Value = Format(Val(vTOT_SIZING_WGT), "########0.000")

        End With

        lbl_Total_FabricMeters.Text = Format(Val(vTOT_FABMTRS), "##########0.00")

    End Sub
    Private Sub btn_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub
    Private Sub cbo_Filter_EndsCount_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_EndsCount.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_EndsCountName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_EndsCount.KeyDown
        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_EndsCount, dtp_Filter_ToDate, txt_Filter_SetNo, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")

    End Sub

    Private Sub cbo_Filter_EndsCountName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_EndsCount.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_EndsCount, txt_Filter_SetNo, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")
        'If Asc(e.KeyChar) = 13 Then
        '    btn_Filter_Show_Click(sender, e)
        'End If
    End Sub


    Private Sub txt_Filter_SetNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Filter_SetNo.KeyDown
        If e.KeyValue = 40 Or (e.Control = True And e.KeyValue = 40) Then
            If Common_Procedures.settings.CustomerCode = "1249" Then
                cbo_Verified_Sts.Focus()
            Else
                btn_Filter_Show.Focus()

            End If

        End If
        If e.KeyCode = 38 Then cbo_Filter_EndsCount.Focus()
    End Sub

    Private Sub txt_Filter_SetNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Filter_SetNo.KeyPress
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
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Verified_Sts, txt_Filter_SetNo, btn_Filter_Show, "", "", "", "")
    End Sub

    Private Sub cbo_Verified_Sts_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Verified_Sts.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Verified_Sts, btn_Filter_Show, "", "", "", "")
    End Sub


    Private Sub txt_PcsLength_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_PcsLength.KeyDown
        If e.KeyCode = 40 Then
            If dgv_PavuDetails.Rows.Count > 0 Then
                dgv_PavuDetails.Focus()
                dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(0).Cells(DgvCol_details.BEAM_NO)
                dgv_PavuDetails.CurrentCell.Selected = True
            Else
                btn_save.Focus()
            End If
        End If

        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_PcsLength_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_PcsLength.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            If dgv_PavuDetails.Rows.Count > 0 Then
                dgv_PavuDetails.Focus()
                dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(0).Cells(DgvCol_details.BEAM_NO)
                dgv_PavuDetails.CurrentCell.Selected = True
            Else
                btn_save.Focus()
            End If
        End If
    End Sub

    Private Sub dgtxt_PavuDetails_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_PavuDetails.Enter
        dgv_ActiveCtrl_Name = dgv_PavuDetails.Name
        dgv_PavuDetails.EditingControl.BackColor = Color.Lime
        dgv_PavuDetails.EditingControl.ForeColor = Color.Blue
        dgv_PavuDetails.SelectAll()
    End Sub

    Private Sub dgtxt_PavuDetails_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_PavuDetails.KeyDown
        If Val(dgv_PavuDetails.Rows(dgv_PavuDetails.CurrentCell.RowIndex).Cells(DgvCol_details.STS).Value) <> 0 Then
            e.SuppressKeyPress = True
            e.Handled = True
        End If
    End Sub

    Private Sub dgtxt_PavuDetails_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_PavuDetails.KeyPress
        If Val(dgv_PavuDetails.Rows(dgv_PavuDetails.CurrentCell.RowIndex).Cells(DgvCol_details.STS).Value) <> 0 Then
            e.Handled = True
        Else
            If dgv_PavuDetails.CurrentCell.ColumnIndex = DgvCol_details.PCS Or dgv_PavuDetails.CurrentCell.ColumnIndex = DgvCol_details.MTR_PCS Or dgv_PavuDetails.CurrentCell.ColumnIndex = DgvCol_details.SIZING_WEIGHT Then
                If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
            End If
        End If

    End Sub

    Private Sub dgtxt_PavuDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_PavuDetails.KeyUp
        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            dgv_PavuDetails_KeyUp(sender, e)
        End If
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String
        Dim PpSzSTS As Boolean = False
        Dim I As Integer = 0
        Dim ps As Printing.PaperSize

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.PrintEntry, Common_Procedures.UR.Sizing_pavu_Receipt_Entry, New_Entry) = False Then Exit Sub

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.* , d.EndsCount_Name from Sizing_Pavu_Receipt_Head a LEFT OUTER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo LEFT OUTER JOIN EndsCount_Head d ON a.EndsCount_IdNo = d.EndsCount_IdNo  where a.Sizing_Pavu_Receipt_Code = '" & Trim(NewCode) & "'", con)
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

        set_PaperSize_For_PrintDocument1()

        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then

            Try
                If Val(Common_Procedures.settings.Printing_Show_PrintDialogue) = 1 Then
                    PrintDialog1.PrinterSettings = PrintDocument1.PrinterSettings
                    If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                        PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings

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


        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_PageNo = 0
        prn_NoofBmDets = 0
        prn_DetMxIndx = 0
        Erase prn_DetAr

        prn_DetAr = New String(500, 10) {}

        Try
            da1 = New SqlClient.SqlDataAdapter("select a.*, b.* , d.EndsCount_Name ,f.* from Sizing_Pavu_Receipt_Head a LEFT OUTER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo LEFT OUTER JOIN EndsCount_Head d ON a.EndsCount_IdNo = d.EndsCount_IdNo INNER JOIN Company_Head f ON a.Company_IdNo = f.Company_IdNo where a.Sizing_Pavu_Receipt_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("select a.* from Sizing_Pavu_Receipt_Details a where Sizing_Pavu_Receipt_Code = '" & Trim(NewCode) & "' Order by a.Sl_No", con)
                prn_DetDt = New DataTable
                da2.Fill(prn_DetDt)

                If prn_DetDt.Rows.Count > 0 Then
                    For i = 0 To prn_DetDt.Rows.Count - 1
                        If Val(prn_DetDt.Rows(i).Item("Meters").ToString) <> 0 Then
                            prn_DetMxIndx = prn_DetMxIndx + 1
                            prn_DetAr(prn_DetMxIndx, 1) = Trim(prn_DetDt.Rows(i).Item("Set_No").ToString)
                            prn_DetAr(prn_DetMxIndx, 2) = Trim(prn_DetDt.Rows(i).Item("Beam_No").ToString)
                            prn_DetAr(prn_DetMxIndx, 3) = Val(prn_DetDt.Rows(i).Item("Pcs").ToString)
                            prn_DetAr(prn_DetMxIndx, 4) = Format(Val(prn_DetDt.Rows(i).Item("Meters").ToString), "#########0.00")
                            prn_DetAr(prn_DetMxIndx, 5) = Trim(Microsoft.VisualBasic.Left(prn_HdDt.Rows(0).Item("EndsCount_Name").ToString, 15))
                        End If
                    Next i
                End If

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
        'If Trim(UCase(Common_Procedures.settings.CompanyName)) = "" Then
        Printing_Format1(e)
        'End If
    End Sub

    Private Sub Printing_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim EntryCode As String
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False


        set_PaperSize_For_PrintDocument1()

        'PrintDocument pd = new PrintDocument();
        'pd.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("PaperA4", 840, 1180);
        'pd.Print();

        'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '    Debug.Print(ps.PaperName)
        '    If ps.Width = 800 And ps.Height = 600 Then
        '        PrintDocument1.DefaultPageSettings.PaperSize = ps
        '        e.PageSettings.PaperSize = ps
        '        PpSzSTS = True
        '        Exit For
        '    End If
        'Next

        'If PpSzSTS = False Then
        '    For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '        If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
        '            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '            PrintDocument1.DefaultPageSettings.PaperSize = ps
        '            e.PageSettings.PaperSize = ps
        '            PpSzSTS = True
        '            Exit For
        '        End If
        '    Next

        '    If PpSzSTS = False Then
        '        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
        '                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '                PrintDocument1.DefaultPageSettings.PaperSize = ps
        '                e.PageSettings.PaperSize = ps
        '                Exit For
        '            End If
        '        Next
        '    End If

        'End If

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 50
            .Right = 50
            .Top = 30
            .Bottom = 30
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

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

        e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        pFont = New Font("Calibri", 11, FontStyle.Regular)

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1408" Then
            NoofItems_PerPage = 35
        Else
            NoofItems_PerPage = 5
        End If

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = 65 : ClArr(2) = 55 : ClArr(3) = 45 : ClArr(4) = 75 : ClArr(5) = 130
        ClArr(6) = 65 : ClArr(7) = 55 : ClArr(8) = 50 : ClArr(9) = 75
        ClArr(10) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9))

        TxtHgt = 18.5 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        ' Try

        If prn_HdDt.Rows.Count > 0 Then

            Printing_Format1_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

            NoofDets = 0

            CurY = CurY - 10

            If prn_DetDt.Rows.Count > 0 Then

                Do While prn_NoofBmDets < prn_DetMxIndx

                    If NoofDets >= NoofItems_PerPage Then

                        CurY = CurY + TxtHgt

                        Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                        NoofDets = NoofDets + 1

                        Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

                        prn_DetIndx = prn_DetIndx + NoofItems_PerPage

                        e.HasMorePages = True

                        Return

                    End If

                    prn_DetIndx = prn_DetIndx + 1

                    CurY = CurY + TxtHgt

                    If Val(prn_DetAr(prn_DetIndx, 4)) <> 0 Then

                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 1)), LMargin + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 2)), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx, 3))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx, 4)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 5)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 10, CurY, 0, 0, pFont)

                        prn_NoofBmDets = prn_NoofBmDets + 1

                    End If

                    If Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 4)) <> 0 Then

                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 1)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + 12, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 2)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 3))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 4)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 5)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + 10, CurY, 0, 0, pFont)

                        prn_NoofBmDets = prn_NoofBmDets + 1

                    End If

                    NoofDets = NoofDets + 1

                Loop

            End If

            Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)

        End If

        ' Catch ex As Exception

        ' MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        'End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format1_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim p1Font As Font
        Dim strHeight As Single
        Dim W1 As Single, N1 As Single, M1 As Single
        Dim Arr(300, 5) As String
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String

        PageNo = PageNo + 1

        CurY = TMargin

        If prn_DetMxIndx > (2 * NoofItems_PerPage) Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If

        CurY = TMargin
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

        If Trim(prn_HdDt.Rows(0).Item("Company_logo_Image").ToString) <> "" Then

            If IsDBNull(prn_HdDt.Rows(0).Item("Company_logo_Image")) = False Then

                Dim imageData As Byte() = DirectCast(prn_HdDt.Rows(0).Item("Company_logo_Image"), Byte())
                If Not imageData Is Nothing Then
                    Using ms As New MemoryStream(imageData, 0, imageData.Length)
                        ms.Write(imageData, 0, imageData.Length)

                        If imageData.Length > 0 Then

                            '.BackgroundImage = Image.FromStream(ms)

                            ' e.Graphics.DrawImage(DirectCast(pic_IRN_QRCode_Image_forPrinting.BackgroundImage, Drawing.Image), PageWidth - 108, CurY + 10, 90, 90)
                            e.Graphics.DrawImage(DirectCast(Image.FromStream(ms), Drawing.Image), LMargin + 10, CurY + 5, 110, 100)

                        End If

                    End Using

                End If

            End If

        End If


        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 18, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt - 12
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "PAVU RECEIPT", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight + 3
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try

            N1 = e.Graphics.MeasureString("TO   : ", pFont).Width
            W1 = e.Graphics.MeasureString("Party DC.NO :  ", pFont).Width

            M1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5)

            CurY = CurY + TxtHgt - 10
            p1Font = New Font("Calibri", 11, FontStyle.Bold)

            Common_Procedures.Print_To_PrintDocument(e, "From :  M/s. " & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "REC.NO", LMargin + M1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 14, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Pavu_Receipt_No").ToString, LMargin + M1 + W1 + 25, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Party DC.NO", LMargin + M1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Party_DcNo").ToString, LMargin + M1 + W1 + 25, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + M1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Sizing_Pavu_Receipt_Date").ToString), "dd-MM-yyyy"), LMargin + M1 + W1 + 25, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + M1, LnAr(3), LMargin + M1, LnAr(2))
            e.Graphics.DrawLine(Pens.Black, LMargin + M1 + 4, LnAr(3), LMargin + M1 + 4, LnAr(2))

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "SET NO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "BEAM", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PCS", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "ENDS COUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "SET NO", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 3, CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "BEAM", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PCS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "ENDS COUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, 2, ClAr(10), pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try


    End Sub

    Private Sub Printing_Format1_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim Cmp_Name As String
        Dim I As Integer

        Try

            For I = NoofDets + 1 To NoofItems_PerPage
                CurY = CurY + TxtHgt
            Next

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(3))

            CurY = CurY + TxtHgt - 10

            If is_LastPage = True Then

                If (prn_DetMxIndx Mod (NoofItems_PerPage * 2)) <= NoofItems_PerPage Then


                    If Val(prn_HdDt.Rows(0).Item("Total_Beam").ToString) <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Beam").ToString), LMargin + ClAr(1) + ClAr(2) - 10, CurY, 1, 0, pFont)
                    End If

                    If Val(prn_HdDt.Rows(0).Item("Total_Pcs").ToString) <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Pcs").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 10, CurY, 1, 0, pFont)
                    End If
                    If Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString) <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
                    End If

                Else

                    If Val(prn_HdDt.Rows(0).Item("Total_Beam").ToString) <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Beam").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                    End If
                    If Val(prn_HdDt.Rows(0).Item("Total_Pcs").ToString) <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Pcs").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 10, CurY, 1, 0, pFont)
                    End If
                    If Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString) <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
                    End If

                End If

            End If

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 4, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 4, LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(3))

            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + 20, CurY, LMargin + ClAr(1) + ClAr(2) + 20, LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 5, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 5, LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 5, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 5, LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 50, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 50, LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + 50, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + 50, LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + 50, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + 50, LnAr(3))
            If Trim(prn_HdDt.Rows(0).Item("vehicle_no").ToString) <> "" Then
                CurY = CurY + TxtHgt + 10
                Common_Procedures.Print_To_PrintDocument(e, "Vehicle No  :  " & Trim(prn_HdDt.Rows(0).Item("vehicle_no").ToString), LMargin + 10, CurY, 0, 0, pFont)
            End If
            CurY = CurY + TxtHgt - 10
            If Trim(prn_HdDt.Rows(0).Item("Remarks").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "Remarks  :  " & Trim(prn_HdDt.Rows(0).Item("Remarks").ToString), LMargin + 10, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(7) = CurY
            Common_Procedures.Print_To_PrintDocument(e, "Received The Beams As Per Above Details.", LMargin + 10, CurY + 5, 0, 0, pFont)
            CurY = CurY + TxtHgt
            If Val(Common_Procedures.User.IdNo) <> 1 Then
                Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
            End If
            CurY = CurY + TxtHgt


            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString

            Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 300, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 5, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin, LnAr(1))
            e.Graphics.DrawLine(Pens.Black, PageWidth, CurY, PageWidth, LnAr(1))

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub msk_date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles msk_date.KeyPress
        If Trim(UCase(e.KeyChar)) = "D" Then
            msk_date.Text = Date.Today
            msk_date.SelectionStart = 0
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
    Private Sub dtp_Date_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_Date.TextChanged
        If IsDate(dtp_Date.Text) = True Then
            msk_date.Text = dtp_Date.Text
            msk_date.SelectionStart = 0
        End If
    End Sub
    Private Sub msk_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_date.KeyDown
        vcbo_KeyDwnVal = e.KeyValue

        If e.KeyCode = 38 Then
            e.Handled = True
        End If

        vmskOldText = ""
        vmskSelStrt = -1
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            vmskOldText = msk_date.Text
            vmskSelStrt = msk_date.SelectionStart
        End If
    End Sub

    Private Sub txt_TotalBobin_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_TotalBobin.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_TotalPavu_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_TotalPavu.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub
    Private Sub btn_Selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Selection.Click
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim i As Integer, j As Integer, n As Integer, SNo As Integer
        Dim LedIdNo As Integer
        Dim NewCode As String
        Dim Ent_Rate As Single = 0
        Dim Ent_Wgt As Single = 0
        Dim Ent_Pcs As Single = 0
        Dim NR As Single = 0

        LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Sizing.Text)

        'If LedIdNo = 0 Then
        '    MessageBox.Show("Invalid Party Name", "DOES NOT SELECT PAVU...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    If cbo_PartyName.Enabled And cbo_PartyName.Visible Then cbo_PartyName.Focus()
        '    Exit Sub
        'End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        With dgv_Selection
            If Val(LedIdNo) <> 0 Then

                .Rows.Clear()
                SNo = 0

                Da = New SqlClient.SqlDataAdapter("select a.*, b.Cloth_Name,c.*  from Own_Order_Head a INNER JOIN CLOTH_Head b ON  b.Cloth_IdNo = a.Cloth_Idno  INNER JOIN Own_Order_Sizing_Details c ON  c.Own_Order_Code = a.Own_Order_Code LEFT OUTER JOIN Sizing_Pavu_Receipt_Head d ON d.Own_Order_Code = a.Own_Order_Code  where a.Sizing_Pavu_Receipt_Code = '" & Trim(NewCode) & "'   and c.Ledger_IdNo = " & Val(LedIdNo) & " order by a.Own_Order_Date, a.for_orderby, a.Own_Order_No", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    For i = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()

                        Ent_Rate = 0


                        SNo = SNo + 1
                        .Rows(n).Cells(DgvSelec_ColDetails.S_NO).Value = Val(SNo)
                        .Rows(n).Cells(DgvSelec_ColDetails.REF_NO).Value = Dt1.Rows(i).Item("Own_Order_No").ToString
                        .Rows(n).Cells(DgvSelec_ColDetails.ORDER_DATE).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Own_Order_Date").ToString), "dd-MM-yyyy")
                        .Rows(n).Cells(DgvSelec_ColDetails.ORDER_NO).Value = Dt1.Rows(i).Item("Order_No").ToString
                        .Rows(n).Cells(DgvSelec_ColDetails.QUALITY).Value = Dt1.Rows(i).Item("Cloth_Name").ToString
                        .Rows(n).Cells(DgvSelec_ColDetails.ORDER_METERS).Value = Format(Val(Dt1.Rows(i).Item("Order_Meters").ToString), "#########0.000")
                        .Rows(n).Cells(DgvSelec_ColDetails.STS).Value = "1"
                        .Rows(n).Cells(DgvSelec_ColDetails.PROCESS_RECEIPT_CODE).Value = Dt1.Rows(i).Item("Own_Order_Code").ToString

                        For j = 0 To .ColumnCount - 1
                            .Rows(i).Cells(j).Style.ForeColor = Color.Red
                        Next

                    Next

                End If
                Dt1.Clear()

                Da = New SqlClient.SqlDataAdapter("select a.*, b.Cloth_Name from Own_Order_Head a INNER JOIN Cloth_Head b ON  b.Cloth_Idno = a.Cloth_Idno INNER JOIN Own_Order_Sizing_Details c ON  c.Own_Order_Code = a.Own_Order_Code   LEFT OUTER JOIN Sizing_Pavu_Receipt_Head d ON d.Sizing_Pavu_Receipt_Code = a.Own_Order_Code    where a.Sizing_Pavu_Receipt_Code = ''  and c.Ledger_IdNo = " & Val(LedIdNo) & " order by a.Own_Order_Date, a.for_orderby, a.Own_Order_No", con)
                Dt1 = New DataTable
                NR = Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    For i = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()

                        SNo = SNo + 1
                        .Rows(n).Cells(DgvSelec_ColDetails.S_NO).Value = Val(SNo)
                        .Rows(n).Cells(DgvSelec_ColDetails.REF_NO).Value = Dt1.Rows(i).Item("OWn_Order_No").ToString
                        .Rows(n).Cells(DgvSelec_ColDetails.ORDER_DATE).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Own_Order_Date").ToString), "dd-MM-yyyy")
                        .Rows(n).Cells(DgvSelec_ColDetails.ORDER_NO).Value = Dt1.Rows(i).Item("Order_No").ToString
                        .Rows(n).Cells(DgvSelec_ColDetails.QUALITY).Value = Dt1.Rows(i).Item("Cloth_Name").ToString
                        .Rows(n).Cells(DgvSelec_ColDetails.ORDER_METERS).Value = Format(Val(Dt1.Rows(i).Item("Order_Meters").ToString), "#########0.000")
                        .Rows(n).Cells(DgvSelec_ColDetails.STS).Value = ""
                        .Rows(n).Cells(DgvSelec_ColDetails.PROCESS_RECEIPT_CODE).Value = Dt1.Rows(i).Item("Own_Order_Code").ToString

                        '.Rows(n).Cells(0).Value = Val(SNo)
                        '.Rows(n).Cells(1).Value = Dt1.Rows(i).Item("OWn_Order_No").ToString
                        '.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Own_Order_Date").ToString), "dd-MM-yyyy")
                        '.Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Order_No").ToString
                        '.Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Cloth_Name").ToString
                        '.Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(i).Item("Order_Meters").ToString), "#########0.000")
                        '.Rows(n).Cells(6).Value = ""
                        '.Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Own_Order_Code").ToString

                    Next

                End If
                Dt1.Clear()
            Else
                .Rows.Clear()
                SNo = 0

                Da = New SqlClient.SqlDataAdapter("select a.*, b.Cloth_Name  from Own_Order_Head a INNER JOIN CLOTH_Head b ON  b.Cloth_IdNo = a.Cloth_Idno   LEFT OUTER JOIN Sizing_Pavu_Receipt_Head d ON d.Own_Order_Code = a.Own_Order_Code  where a.Sizing_Pavu_Receipt_Code = '" & Trim(NewCode) & "'   order by a.Own_Order_Date, a.for_orderby, a.Own_Order_No", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    For i = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()

                        Ent_Rate = 0


                        SNo = SNo + 1
                        .Rows(n).Cells(DgvSelec_ColDetails.S_NO).Value = Val(SNo)
                        .Rows(n).Cells(DgvSelec_ColDetails.REF_NO).Value = Dt1.Rows(i).Item("Own_Order_No").ToString
                        .Rows(n).Cells(DgvSelec_ColDetails.ORDER_DATE).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Own_Order_Date").ToString), "dd-MM-yyyy")
                        .Rows(n).Cells(DgvSelec_ColDetails.ORDER_NO).Value = Dt1.Rows(i).Item("Order_No").ToString
                        .Rows(n).Cells(DgvSelec_ColDetails.QUALITY).Value = Dt1.Rows(i).Item("Cloth_Name").ToString
                        .Rows(n).Cells(DgvSelec_ColDetails.ORDER_METERS).Value = Format(Val(Dt1.Rows(i).Item("Order_Meters").ToString), "#########0.000")
                        .Rows(n).Cells(DgvSelec_ColDetails.STS).Value = "1"
                        .Rows(n).Cells(DgvSelec_ColDetails.PROCESS_RECEIPT_CODE).Value = Dt1.Rows(i).Item("Own_Order_Code").ToString

                        For j = 0 To .ColumnCount - 1
                            .Rows(i).Cells(j).Style.ForeColor = Color.Red
                        Next

                    Next

                End If
                Dt1.Clear()

                Da = New SqlClient.SqlDataAdapter("select a.*, b.Cloth_Name from Own_Order_Head a INNER JOIN Cloth_Head b ON  b.Cloth_Idno = a.Cloth_Idno    LEFT OUTER JOIN Sizing_Pavu_Receipt_Head d ON d.Sizing_Pavu_Receipt_Code = a.Own_Order_Code    where a.Sizing_Pavu_Receipt_Code = ''   order by a.Own_Order_Date, a.for_orderby, a.Own_Order_No", con)
                Dt1 = New DataTable
                NR = Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    For i = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()

                        SNo = SNo + 1

                        .Rows(n).Cells(DgvSelec_ColDetails.S_NO).Value = Val(SNo)
                        .Rows(n).Cells(DgvSelec_ColDetails.REF_NO).Value = Dt1.Rows(i).Item("Own_Order_No").ToString
                        .Rows(n).Cells(DgvSelec_ColDetails.ORDER_DATE).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Own_Order_Date").ToString), "dd-MM-yyyy")
                        .Rows(n).Cells(DgvSelec_ColDetails.ORDER_NO).Value = Dt1.Rows(i).Item("Order_No").ToString
                        .Rows(n).Cells(DgvSelec_ColDetails.QUALITY).Value = Dt1.Rows(i).Item("Cloth_Name").ToString
                        .Rows(n).Cells(DgvSelec_ColDetails.ORDER_METERS).Value = Format(Val(Dt1.Rows(i).Item("Order_Meters").ToString), "#########0.000")
                        .Rows(n).Cells(DgvSelec_ColDetails.STS).Value = ""
                        .Rows(n).Cells(DgvSelec_ColDetails.PROCESS_RECEIPT_CODE).Value = Dt1.Rows(i).Item("Own_Order_Code").ToString

                    Next

                End If
                Dt1.Clear()
            End If
        End With

        pnl_Selection.Visible = True
        pnl_Back.Enabled = False
        dgv_Selection.Focus()

    End Sub

    Private Sub dgv_Selection_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Selection.CellClick
        Select_Piece(e.RowIndex)
    End Sub

    Private Sub Select_Piece(ByVal RwIndx As Integer)
        Dim i As Integer

        With dgv_Selection

            If .RowCount > 0 And RwIndx >= 0 Then

                .Rows(RwIndx).Cells(DgvSelec_ColDetails.STS).Value = (Val(.Rows(RwIndx).Cells(DgvSelec_ColDetails.STS).Value) + 1) Mod 2

                If Val(.Rows(RwIndx).Cells(DgvSelec_ColDetails.STS).Value) = 1 Then

                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
                    Next

                Else
                    .Rows(RwIndx).Cells(DgvSelec_ColDetails.STS).Value = ""

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

            If Val(dgv_Selection.Rows(i).Cells(DgvSelec_ColDetails.STS).Value) = 1 Then

                ' lbl_RecCode.Text = dgv_Selection.Rows(i).Cells(8).Value

                lbl_OrderNo.Text = dgv_Selection.Rows(i).Cells(DgvSelec_ColDetails.ORDER_NO).Value
                lbl_OrderCode.Text = dgv_Selection.Rows(i).Cells(DgvSelec_ColDetails.PROCESS_RECEIPT_CODE).Value

            End If

        Next

        pnl_Back.Enabled = True
        pnl_Selection.Visible = False

        If txt_PartyDcNo.Enabled And txt_PartyDcNo.Visible Then txt_PartyDcNo.Focus()


    End Sub

    Private Sub dgtxt_PavuDetails_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_PavuDetails.TextChanged
        Try
            With dgv_PavuDetails
                If .Rows.Count <> 0 Then
                    .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(dgtxt_PavuDetails.Text)
                End If
            End With

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub btn_UserModification_Click(sender As Object, e As System.EventArgs) Handles btn_UserModification.Click
        If Val(Common_Procedures.User.IdNo) = 1 Then
            Dim f1 As New User_Modifications
            f1.Entry_Name = Me.Name
            f1.Entry_PkValue = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            f1.ShowDialog()
        End If
    End Sub

    Private Sub txt_Remarks_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles txt_Remarks.KeyDown
        If (e.KeyValue = 38) Or (e.Control = True And e.KeyValue = 38) Then
            If cbo_WidthType.Visible And cbo_WidthType.Enabled Then
                cbo_WidthType.Focus()
            ElseIf txt_PickUp_Perc.Visible And Enabled Then
                txt_PickUp_Perc.Focus()

            Else
                txt_Freight.Focus()
            End If
        End If


        If (e.KeyValue = 40) Or (e.Control = True And e.KeyValue = 40) Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                msk_date.Focus()
            End If
        End If
    End Sub

    Private Sub txt_Remarks_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Remarks.KeyPress
        If Asc(e.KeyChar) = 13 Then

            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                msk_date.Focus()
            End If

        End If
    End Sub

    Private Sub txt_Freight_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles txt_Freight.KeyDown
        If e.KeyValue = 38 Then
            txt_vehicle.Focus()
        End If
        If (e.KeyValue = 40) Then
            If txt_PickUp_Perc.Visible And Enabled Then
                txt_PickUp_Perc.Focus()
            ElseIf dgv_PavuDetails.Rows.Count > 0 Then
                dgv_PavuDetails.Focus()
                dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(0).Cells(DgvCol_details.BEAM_NO)

            Else
                btn_save.Focus()

            End If
        End If

    End Sub

    Private Sub txt_Freight_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Freight.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
        If Asc(e.KeyChar) = 13 Then
            If txt_PickUp_Perc.Visible And Enabled Then
                txt_PickUp_Perc.Focus()
            ElseIf dgv_PavuDetails.Rows.Count > 0 Then
                dgv_PavuDetails.Focus()
                dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(0).Cells(DgvCol_details.BEAM_NO)

            Else
                btn_save.Focus()

            End If
        End If
    End Sub
    Public Sub Get_vehicle_from_Transport()

        If Common_Procedures.settings.CustomerCode <> "1186" Then Exit Sub

        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim transport_id As Integer

        transport_id = Common_Procedures.Ledger_NameToIdNo(con, cbo_TransportName.Text)
        Da = New SqlClient.SqlDataAdapter("select vehicle_no from ledger_head where ledger_idno=" & Str(Val(transport_id)) & "", con)
        Dt = New DataTable
        Da.Fill(Dt)
        If Dt.Rows.Count <> 0 Then
            txt_vehicle.Text = Dt.Rows(0).Item("vehicle_no").ToString
        End If
        Dt.Clear()

    End Sub

    Private Sub cbo_TransportName_GotFocus(sender As Object, e As System.EventArgs) Handles cbo_TransportName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
        Get_vehicle_from_Transport()
    End Sub

    Private Sub cbo_TransportName_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles cbo_TransportName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_TransportName, Nothing, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
        If e.KeyValue = 38 Then
            If cbo_Sizing_JobCardNo.Enabled And cbo_Sizing_JobCardNo.Visible = True Then
                cbo_Sizing_JobCardNo.Focus()
            Else
                cbo_DeliveryTo.Focus()
            End If
        End If

        If (e.KeyValue = 40 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If cbo_Weaving_JobCardNo.Visible And cbo_Weaving_JobCardNo.Enabled Then
                cbo_Weaving_JobCardNo.Focus()
            ElseIf txt_Freight_For.Visible And txt_Freight_For.Enabled Then
                txt_Freight_For.Focus()
            ElseIf cbo_ClothSales_OrderCode_forSelection.Visible And cbo_ClothSales_OrderCode_forSelection.Enabled Then
                cbo_ClothSales_OrderCode_forSelection.Focus()
            Else
                txt_vehicle.Focus()
            End If
        End If

        Get_vehicle_from_Transport()
    End Sub

    Private Sub cbo_TransportName_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_TransportName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_TransportName, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            If cbo_Weaving_JobCardNo.Visible And cbo_Weaving_JobCardNo.Enabled Then
                cbo_Weaving_JobCardNo.Focus()
            ElseIf txt_Freight_For.Visible And txt_Freight_For.Enabled Then
                txt_Freight_For.Focus()
            ElseIf cbo_ClothSales_OrderCode_forSelection.Visible And cbo_ClothSales_OrderCode_forSelection.Enabled Then
                cbo_ClothSales_OrderCode_forSelection.Focus()
            Else

                txt_vehicle.Focus()
            End If
        End If
        Get_vehicle_from_Transport()
    End Sub

    Private Sub cbo_TransportName_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles cbo_TransportName.KeyUp
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
    Private Sub FreightFor_Calculation()

        With dgv_PavuDetails_Total
            If .Rows.Count > 0 Then
                If Val(.Rows(0).Cells(DgvCol_details.BEAM_NO).Value) <> 0 Then

                    txt_Freight.Text = Format(Val(.Rows(0).Cells(DgvCol_details.BEAM_NO).Value()) * Val(txt_Freight_For.Text), "########0.00")
                End If
            End If
        End With

    End Sub

    Private Sub txt_FreightFor_TextChanged(sender As Object, e As System.EventArgs) Handles txt_Freight_For.TextChanged
        FreightFor_Calculation()
    End Sub

    Private Sub dgv_PavuDetails_Total_CellValueChanged(sender As Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_PavuDetails_Total.CellValueChanged

        With dgv_PavuDetails_Total
            FreightFor_Calculation()
        End With

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        print_record()
    End Sub

    Private Sub Delivery_Selection()
        Dim Cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Da2 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim i As Integer, j As Integer, n As Integer, SNo As Integer, Ledger_Party_idno As Integer
        Dim Led_IdNo As Integer
        Dim NewCode As String = ""
        Dim CompIDCondt As String = ""
        Dim RcptBm_PavuInc As Integer
        Dim vjoinTYP As String

        If Trim(UCase(cbo_Type.Text)) <> "DELIVERY" Then Exit Sub

        Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Sizing.Text)
        If Led_IdNo = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SELECT PAVU...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Sizing.Enabled And cbo_Sizing.Visible Then cbo_Sizing.Focus()
            Exit Sub
        End If

        CompIDCondt = "(a.Selection_CompanyIdno = " & Str(Val(lbl_Company.Tag)) & ")"



        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)


        If Trim(UCase(cbo_Type.Text)) = "DELIVERY" Then


            With dgv_delivery_Selections

                .Rows.Clear()
                n = .Rows.Add()
                SNo = 0

                For i = 1 To 2


                    If i = 1 Then
                        '---editing
                        Da2 = New SqlClient.SqlDataAdapter("Select a.Delivery_Code, a.delivery_No,  a.Total_Beams as Beams , a.Total_pcs as Pcs, a.Total_meters as meters from Pavu_Delivery_Selections_Processing_Details a where ( a.Selection_ReceivedFromIdNo = " & Str(Val(Led_IdNo)) & " OR a.Selection_ledgerIdno = " & Str(Val(Led_IdNo)) & " ) and  " & CompIDCondt & " and a.Delivery_Code = a.reference_code and a.Total_meters > 0 and a.Delivery_Code IN (Select sq1.Delivery_Code from Pavu_Delivery_Selections_Processing_Details sq1 where sq1.reference_code = '" & Trim(NewCode) & "' )  ", con)
                        'Da2 = New SqlClient.SqlDataAdapter("Select a.Delivery_Code, a.delivery_No,  a.Total_Beams as Beams , a.Total_pcs as Pcs, a.Total_meters as meters from Pavu_Delivery_Selections_Processing_Details a where   a.Ledger_idno =" & Str(Val(Led_IdNo)) & " and a.Delivery_Code IN (Select a.Delivery_Code from Pavu_Delivery_Selections_Processing_Details sq1 where sq1.reference_code = '" & Trim(NewCode) & "' ) ", con)
                    Else
                        Cmd.Connection = con

                        Cmd.CommandText = "truncate table " & Trim(Common_Procedures.EntryTempTable) & ""
                        Cmd.ExecuteNonQuery()

                        Common_Procedures.get_PavuDelivery_Selection_Processing_Pending(con)
                        'new entry
                        Da2 = New SqlClient.SqlDataAdapter("Select a.Delivery_Code, a.delivery_No, a.for_OrderBy, a.Reference_Date, tET.int1 as Beams, tET.meters2 as Pcs, tET.weight3 as meters from Pavu_Delivery_Selections_Processing_Details a INNER JOIN " & Trim(Common_Procedures.EntryTempTable) & " tET ON tET.Name1 <> '' and tET.weight3 <> 0 and a.Delivery_Code = tET.Name1 where a.Selection_CompanyIdno = " & Str(Val(lbl_Company.Tag)) & " and (a.Selection_ReceivedFromIdNo = " & Str(Val(Led_IdNo)) & " OR a.Selection_ledgerIdno = " & Str(Val(Led_IdNo)) & " ) and a.Total_meters > 0 Order by a.Reference_Date DESC, a.for_OrderBy DESC, a.Delivery_Code DESC, a.Delivery_No DESC", con)
                        'Da2 = New SqlClient.SqlDataAdapter("Select a.Delivery_Code, a.delivery_No,  SUM(a.Total_Beams) as Beams , SUM(a.Total_pcs) as Pcs, SUM(a.Total_meters) as meters from Pavu_Delivery_Selections_Processing_Details a where   a.Selection_ledgerIdno =" & Str(Val(Led_IdNo)) & " and " & CompIDCondt & " and a.Delivery_Code NOT IN (Select sq1.Delivery_Code from Pavu_Delivery_Selections_Processing_Details sq1 where sq1.reference_code = '" & Trim(NewCode) & "' ) Group by a.Delivery_Code, a.Delivery_No Having Sum(a.Total_meters) > 0  ", con)
                    End If


                    Dt2 = New DataTable


                    Da2.Fill(Dt2)

                    If Dt2.Rows.Count > 0 Then

                        For k = 0 To Dt2.Rows.Count - 1

                            If Val(Dt2.Rows(k).Item("meters").ToString) > 0 Then

                                SNo = SNo + 1
                                n = .Rows.Add()

                                .Rows(n).Cells(DgvDeliverySelec_ColDetails.S_NO).Value = Val(SNo)
                                .Rows(n).Cells(DgvDeliverySelec_ColDetails.DC_NO).Value = Dt2.Rows(k).Item("Delivery_No").ToString
                                '.Rows(n).Cells(DgvDeliverySelec_ColDetails.DC_DATE).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Empty_BeamBagCone_Delivery_Date").ToString), "dd-MM-yyyy")
                                .Rows(n).Cells(DgvDeliverySelec_ColDetails.PARTY_DC_NO).Value = Dt2.Rows(k).Item("Delivery_No").ToString
                                .Rows(n).Cells(DgvDeliverySelec_ColDetails.BEAM_NO).Value = Dt2.Rows(k).Item("Beams").ToString
                                .Rows(n).Cells(DgvDeliverySelec_ColDetails.PCS).Value = Dt2.Rows(k).Item("Pcs").ToString
                                .Rows(n).Cells(DgvDeliverySelec_ColDetails.METERS).Value = Dt2.Rows(k).Item("Meters").ToString
                                .Rows(n).Cells(DgvDeliverySelec_ColDetails.DELIVERY_CODE).Value = Trim(Dt2.Rows(k).Item("Delivery_Code").ToString)
                                If i = 1 Then

                                    .Rows(n).Cells(DgvDeliverySelec_ColDetails.STS).Value = 1
                                    'For j = 0 To .ColumnCount - 1
                                    '    .Rows(k).Cells(j).Style.ForeColor = Color.Red
                                    'Next

                                Else
                                    .Rows(n).Cells(DgvDeliverySelec_ColDetails.STS).Value = ""
                                    'For j = 0 To .ColumnCount - 1
                                    '    .Rows(k).Cells(j).Style.ForeColor = Color.Black
                                    'Next

                                End If


                            End If
                        Next


                    End If
                    Dt2.Clear()


                Next








            End With


        End If
        pnl_Delivery_Selection.Visible = True
        pnl_Back.Enabled = False
        dgv_delivery_Selections.Focus()

    End Sub

    Private Sub Close_Delivery_Selection()
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim i As Integer, n As Integer
        Dim sno As Integer = 0
        Dim Clo_IdNo As Integer = 0


        sno = 0
        Clo_IdNo = 0

        lbl_Delivery_Code.Text = ""
        txt_PartyDcNo.Text = ""
        dgv_PavuDetails.Rows.Clear()

        For k = 0 To dgv_delivery_Selections.RowCount - 1


            If Val(dgv_delivery_Selections.Rows.Count > 0) Then
                lbl_Delivery_Code.Text = Trim(dgv_delivery_Selections.Rows(i).Cells(DgvDeliverySelec_ColDetails.DELIVERY_CODE).Value)
                'txt_KuraiPavuMeters.Text = Trim(dgv_delivery_Selections.Rows(i).Cells(6).Value)
                txt_PartyDcNo.Text = Trim(dgv_delivery_Selections.Rows(i).Cells(DgvDeliverySelec_ColDetails.DC_NO).Value)
            End If


            If Val(dgv_delivery_Selections.Rows(k).Cells(DgvDeliverySelec_ColDetails.STS).Value) = 1 Then
                Da = New SqlClient.SqlDataAdapter("Select a.sl_no,a.set_no,Beam_no,NoOf_Pcs,a.NoOf_Pcs,a.Meters_Pc ,a.Meters,a.Ends_name,a.Count_idno ,a.* from Sizing_Pavu_Delivery_Details a   where 'PVDLV-'+  a.Pavu_Delivery_code =  '" & Trim(dgv_delivery_Selections.Rows(k).Cells(DgvDeliverySelec_ColDetails.DELIVERY_CODE).Value) & "' ", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)
                With dgv_PavuDetails

                    dgv_PavuDetails.Rows.Clear()

                    If Val(Dt1.Rows.Count <> 0) Then

                        For i = 0 To Dt1.Rows.Count - 1

                            n = .Rows.Add()


                            .Rows(n).Cells(DgvCol_details.SLNO).Value = Trim(Dt1.Rows(i).Item("Sl_No").ToString)
                            .Rows(n).Cells(DgvCol_details.BEAM_NO).Value = Trim(Dt1.Rows(i).Item("Beam_no").ToString)
                            .Rows(n).Cells(DgvCol_details.PCS).Value = Trim(Dt1.Rows(i).Item("NoOf_Pcs").ToString)
                            .Rows(n).Cells(DgvCol_details.MTR_PCS).Value = Trim(Dt1.Rows(i).Item("Meters_Pc").ToString)
                            .Rows(n).Cells(DgvCol_details.METERS).Value = Val(Dt1.Rows(i).Item("Meters").ToString)

                            txt_SetNo.Text = Trim(Dt1.Rows(i).Item("set_no").ToString)
                            cbo_EndsCount.Text = Trim(Dt1.Rows(i).Item("Ends_Name").ToString) & "/" & Trim(Common_Procedures.Count_IdNoToName(con, Val(Dt1.Rows(i).Item("count_idno").ToString)))


                        Next
                    End If

                End With
                Dt1.Clear()


                'Da = New SqlClient.SqlDataAdapter("Select h.Pavu_Meters,h.Empty_Beam,a.sl_no,a.set_no,Beam_no, c.EndsCount_Name,a.pcs,a.Meters_Pc ,a.Meters , d.Beam_Width_Name,a.* from Weaver_Pavu_Delivery_Details a  inner join Weaver_Pavu_Delivery_head h on a.Weaver_Pavu_Delivery_code=h.Weaver_Pavu_Delivery_code INNER JOIN EndsCount_Head c ON a.EndsCount_IdNo = c.EndsCount_IdNo LEFT OUTER JOIN Beam_Width_Head d ON a.Beam_Width_Idno = d.Beam_Width_Idno where 'WPVDC-'+ a.Weaver_Pavu_Delivery_code ='" & Trim(dgv_delivery_Selections.Rows(k).Cells(8).Value) & "' ", con)
                'Dt1 = New DataTable
                'Da.Fill(Dt1)



                'With dgv_PavuDetails


                '    If Val(Dt1.Rows.Count <> 0) Then

                '        For j = 0 To Dt1.Rows.Count - 1

                '            n = .Rows.Add()


                '            .Rows(n).Cells(0).Value = Trim(Dt1.Rows(j).Item("Sl_No").ToString)
                '            .Rows(n).Cells(1).Value = Trim(Dt1.Rows(j).Item("Beam_no").ToString)
                '            .Rows(n).Cells(2).Value = Trim(Dt1.Rows(j).Item("pcs").ToString)
                '            .Rows(n).Cells(3).Value = Trim(Dt1.Rows(j).Item("Meters_Pc").ToString)
                '            .Rows(n).Cells(4).Value = Val(Dt1.Rows(j).Item("Meters").ToString)

                '            txt_SetNo.Text = Trim(Dt1.Rows(j).Item("set_no").ToString)
                '            cbo_EndsCount.Text = Trim(Dt1.Rows(j).Item("EndsCount_Name").ToString) '& "/" & Trim(Common_Procedures.Count_IdNoToName(con, Val(Dt1.Rows(i).Item("count_idno").ToString)))



                '        Next
                '    End If

                'End With
                'Dt1.Clear()



                Exit For
            End If

        Next
        pnl_Back.Enabled = True
        pnl_Delivery_Selection.Visible = False
        If Trim(UCase(cbo_Type.Text)) = "DELIVERY" Then
            dgv_PavuDetails.AllowUserToAddRows = False
        End If
    End Sub
    Private Sub btn_Close_Delivery_Selection_Click(sender As Object, e As EventArgs) Handles btn_Close_Delivery_Selection.Click

        Close_Delivery_Selection()

    End Sub
    Private Sub cbo_Type_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Type.Enter
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")

    End Sub

    Private Sub cbo_Type_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Type.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Type, msk_date, txt_PartyDcNo, "", "", "", "")
    End Sub

    Private Sub cbo_Type_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Type.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Type, Nothing, "", "", "", "")
        If Asc(e.KeyChar) = 13 Then
            If Val(Common_Procedures.settings.Textile_Sizing_Delivery_receipt_Selection) = 1 And Trim(cbo_Type.Text = "DELIVERY") Then
                Delivery_Selection()
            Else
                txt_PartyDcNo.Focus()

            End If




        End If
    End Sub

    Private Sub dgv_delivery_Selections_Click(sender As Object, e As EventArgs) Handles dgv_delivery_Selections.Click

    End Sub
    Private Sub Select_Pavu(ByVal RwIndx As Integer)
        Dim i As Integer





        With dgv_delivery_Selections

            If .RowCount > 0 And RwIndx >= 0 Then

                For i = 0 To .Rows.Count - 1
                    .Rows(i).Cells(DgvDeliverySelec_ColDetails.STS).Value = ""
                Next

                .Rows(RwIndx).Cells(DgvDeliverySelec_ColDetails.STS).Value = 1

                If Val(.Rows(RwIndx).Cells(DgvDeliverySelec_ColDetails.STS).Value) = 1 Then

                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
                    Next


                Else
                    .Rows(RwIndx).Cells(DgvDeliverySelec_ColDetails.STS).Value = ""

                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Black
                    Next

                End If


                Close_Delivery_Selection()
                TotalPavu_Calculation()
            End If

        End With
    End Sub

    Private Sub dgv_delivery_Selections_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgv_delivery_Selections.CellClick
        Select_Pavu(e.RowIndex)
    End Sub

    Private Sub dgv_delivery_Selections_CellMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles dgv_delivery_Selections.CellMouseClick
        btn_Close_Delivery_Selection_Click(sender, e)
        TotalPavu_Calculation()

    End Sub

    'Private Sub dgv_delivery_Selections_CellEnter(sender As Object, e As DataGridViewCellEventArgs) Handles dgv_delivery_Selections.CellEnter
    '    btn_Close_Delivery_Selection_Click(sender, e)
    '    TotalPavu_Calculation()
    'End Sub

    Private Sub dgv_delivery_Selections_KeyDown(sender As Object, e As KeyEventArgs) Handles dgv_delivery_Selections.KeyDown
        On Error Resume Next

        With dgv_delivery_Selections

            If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Space Then
                If .CurrentCell.RowIndex >= 0 Then
                    Select_Pavu(.CurrentCell.RowIndex)
                    e.Handled = True
                End If
            End If

            If e.KeyCode = Keys.Back Or e.KeyCode = Keys.Delete Then
                If .CurrentCell.RowIndex >= 0 Then
                    If Val(.Rows(.CurrentCell.RowIndex).Cells(DgvDeliverySelec_ColDetails.STS).Value) = 1 Then
                        Select_Pavu(.CurrentCell.RowIndex)
                        e.Handled = True
                    End If
                End If
            End If

        End With


    End Sub

    Private Sub btn_Delivery_Selection_Click(sender As Object, e As EventArgs) Handles btn_Delivery_Selection.Click
        Delivery_Selection()

    End Sub

    Private Sub cbo_WidthType_GotFocus(sender As Object, e As EventArgs) Handles cbo_WidthType.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")
    End Sub

    Private Sub cbo_WidthType_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_WidthType.KeyDown

        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_WidthType, Nothing, Nothing, "", "", "", "")
        If (e.KeyValue = 38 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
            If txt_PickUp_Perc.Visible And Enabled Then
                txt_PickUp_Perc.Focus()
            Else
                txt_Freight.Focus()
            End If
        End If

        If (e.KeyValue = 40 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If cbo_MillName.Visible And cbo_MillName.Enabled Then
                cbo_MillName.Focus()
            Else
                txt_Remarks.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_WidthType_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_WidthType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_WidthType, Nothing, "", "", "", "")
        If Asc(e.KeyChar) = 13 Then
            If cbo_MillName.Visible And cbo_MillName.Enabled Then
                cbo_MillName.Focus()
            Else
                txt_Remarks.Focus()
            End If
        End If

    End Sub

    Private Sub cbo_WidthType_TextChanged(sender As Object, e As EventArgs) Handles cbo_WidthType.TextChanged
        TotalPavu_Calculation()
        'Dim vWidthVal As Integer = 0

        'Dim vTotPvuMtrs As Single = 0
        'Dim vTotPvuStk As Single = 0
        'Dim vNoofBeams As Integer = 0
        'Dim vWdTyp As Single = 0
        'Dim vTotPvuStkAlLoomMtr As Single = 0
        'Dim vCrmp_Mtrs As String = 0

        'Dim Stk_DelvMtr As Single, Stk_RecMtr As Single
        'Dim Delv_Ledtype As String = ""
        'Dim Rec_Ledtype As String = ""


        'vTotPvuMtrs = 0
        'If dgv_PavuDetails_Total.RowCount > 0 Then
        '    vTotPvuMtrs = Val(dgv_PavuDetails_Total.Rows(0).Cells(4).Value())
        'End If

        'lbl_TotMeters.Text = ""


        ''vWidthVal = 0
        ''If Trim(UCase(cbo_WidthType.Text)) = "SINGLE FABRIC FROM 1 BEAM" Then
        ''    vWidthVal = 1
        ''ElseIf Trim(UCase(cbo_WidthType.Text)) = "SINGLE FABRIC FROM 2 BEAMS" Then
        ''    vWidthVal = 2
        ''End If

        ''lbl_TotMeters.Text = Format(Val(vTotPvuMtr) * Val(vWidthVal), "##########0.00")


        ''----------------------------------------



        'If Common_Procedures.settings.AutoLoom_PavuWidthWiseConsumption_IN_Delivery = 1 Then

        '    vNoofBeams = 2
        '    If Trim(cbo_WidthType.Text) <> "" Then
        '        If InStr(1, Trim(UCase(cbo_WidthType.Text)), "1 BEAM") > 0 Then
        '            vNoofBeams = 1
        '        ElseIf InStr(1, Trim(UCase(cbo_WidthType.Text)), "2 BEAM") > 0 Then
        '            vNoofBeams = 2
        '        End If
        '    End If

        '    vWdTyp = 0
        '    If Trim(UCase(cbo_WidthType.Text)) = "FOURTH" Or InStr(1, Trim(UCase(cbo_WidthType.Text)), "FOURTH") > 0 Or InStr(1, Trim(UCase(cbo_WidthType.Text)), "FOUR") > 0 Then
        '        vWdTyp = 4
        '    ElseIf Trim(UCase(cbo_WidthType.Text)) = "TRIPLE" Or InStr(1, Trim(UCase(cbo_WidthType.Text)), "TRIPLE") > 0 Then
        '        vWdTyp = 3
        '    ElseIf Trim(UCase(cbo_WidthType.Text)) = "DOUBLE" Or InStr(1, Trim(UCase(cbo_WidthType.Text)), "DOUBLE") > 0 Then
        '        vWdTyp = 2
        '    ElseIf Trim(UCase(cbo_WidthType.Text)) = "SINGLE" Or InStr(1, Trim(UCase(cbo_WidthType.Text)), "SINGLE") > 0 Then
        '        vWdTyp = 1
        '    End If

        '    vTotPvuStkAlLoomMtr = Format(vTotPvuMtrs / vNoofBeams * vWdTyp, "###########0.00")

        '    lbl_TotMeters.Text = Format(Val(vTotPvuStkAlLoomMtr), "##########0.00")




        '    If Trim(UCase(Delv_Ledtype)) = "WEAVER" Then
        '        Stk_DelvMtr = vTotPvuStkAlLoomMtr
        '    Else
        '        Stk_DelvMtr = vTotPvuMtrs
        '    End If

        '    If Trim(UCase(Rec_Ledtype)) = "WEAVER" Then
        '        Stk_RecMtr = vTotPvuStkAlLoomMtr
        '    Else
        '        Stk_RecMtr = vTotPvuMtrs
        '    End If

        'Else

        '    vTotPvuStk = vTotPvuMtrs

        '    Stk_DelvMtr = vTotPvuMtrs
        '    Stk_RecMtr = vTotPvuMtrs

        'End If


    End Sub


    Private Function Calculate_Fabric_Meters(vTotPavuMtrs As String) As String
        Dim vWidthVal As Integer = 0
        Dim vTotPvuMtrs As Single = 0
        Dim vTotPvuStk As Single = 0
        Dim vNoofBeams As Integer = 0
        Dim vDEFBMS As Integer = 0
        Dim vWdTyp As Single = 0
        Dim vTotPvuStkAlLoomMtr As String = 0
        Dim vFABMTRS As String = 0


        vFABMTRS = 0
        If Common_Procedures.settings.AutoLoom_PavuWidthWiseConsumption_IN_Delivery = 1 Then

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1428" Then '---- SAKTHI VINAYAGA TEXTILES  (ERODE-PALLIPALAYAM)
                vDEFBMS = 1
            Else
                vDEFBMS = 2
            End If

            vNoofBeams = vDEFBMS
            vWdTyp = vDEFBMS
            If Trim(cbo_WidthType.Text) <> "" Then
                If InStr(1, Trim(UCase(cbo_WidthType.Text)), "1 BEAM") > 0 Then
                    vNoofBeams = 1
                ElseIf InStr(1, Trim(UCase(cbo_WidthType.Text)), "2 BEAM") > 0 Then
                    vNoofBeams = 2
                End If

                If Trim(UCase(cbo_WidthType.Text)) = "FOURTH" Or InStr(1, Trim(UCase(cbo_WidthType.Text)), "FOURTH") > 0 Or InStr(1, Trim(UCase(cbo_WidthType.Text)), "FOUR") > 0 Then
                    vWdTyp = 4
                ElseIf Trim(UCase(cbo_WidthType.Text)) = "TRIPLE" Or InStr(1, Trim(UCase(cbo_WidthType.Text)), "TRIPLE") > 0 Then
                    vWdTyp = 3
                ElseIf Trim(UCase(cbo_WidthType.Text)) = "DOUBLE" Or InStr(1, Trim(UCase(cbo_WidthType.Text)), "DOUBLE") > 0 Then
                    vWdTyp = 2
                ElseIf Trim(UCase(cbo_WidthType.Text)) = "SINGLE" Or InStr(1, Trim(UCase(cbo_WidthType.Text)), "SINGLE") > 0 Then
                    vWdTyp = 1
                End If

            End If

            vTotPvuStkAlLoomMtr = Format(vTotPavuMtrs / vNoofBeams * vWdTyp, "###########0.00")

            vFABMTRS = Format(Val(vTotPvuStkAlLoomMtr), "##########0.00")

        Else

            vFABMTRS = Format(Val(vTotPavuMtrs), "##########0.00")

        End If

        Calculate_Fabric_Meters = vFABMTRS

    End Function

    Private Sub cbo_MillName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_MillName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")
    End Sub

    Private Sub cbo_MillName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_MillName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, sender, cbo_WidthType, txt_Warp_LotNo, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")

        If (e.KeyCode = 38 And sender.DroppedDown = False) Or (e.Control = True And e.KeyCode = 38) Then

            If cbo_WidthType.Visible = True Then
                cbo_WidthType.Focus()
            Else
                txt_Freight.Focus()
            End If

        End If

    End Sub

    Private Sub cbo_MillName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_MillName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, sender, txt_Warp_LotNo, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")
    End Sub

    Private Sub cbo_MillName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_MillName.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New Mill_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = sender.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If

    End Sub

    Private Sub txt_Warp_LotNo_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Warp_LotNo.KeyDown
        On Error Resume Next
        If e.KeyValue = 38 Then e.Handled = True : SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then e.Handled = True : SendKeys.Send("{TAB}")
    End Sub

    Private Sub txt_Warp_LotNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Warp_LotNo.KeyPress

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1464" Then
            If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        End If

        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                msk_date.Focus()
            End If
        End If

    End Sub

    Private Sub set_PaperSize_For_PrintDocument1()
        Dim I As Integer = 0
        Dim PpSzSTS As Boolean = False
        Dim ps As Printing.PaperSize

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1242" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1408" Then
            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                    PrintDocument1.DefaultPageSettings.PaperSize = ps
                    Exit For
                End If
            Next


        ElseIf Val(Common_Procedures.settings.Printing_For_HalfSheet_Set_Custom8X6_As_Default_PaperSize) = 1 Then

            Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8X6", 800, 600)
            PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
            PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1
            PrintDocument1.DefaultPageSettings.Landscape = False

        ElseIf Val(Common_Procedures.settings.Printing_For_HalfSheet_Set_A4_As_Default_PaperSize) = 1 Or Val(Common_Procedures.settings.Printing_For_FullSheet_Set_A4_As_Default_PaperSize) = 1 Then

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

                'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                '    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
                '        ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                '        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                '        PrintDocument1.DefaultPageSettings.PaperSize = ps
                '        PpSzSTS = True
                '        Exit For
                '    End If
                'Next

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


    Private Sub cbo_weaving_job_no_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_Weaving_JobCardNo.KeyPress
        Dim Led_idno As Integer = 0

        Led_idno = 0
        If Trim(cbo_DeliveryTo.Text) <> "" Then
            Led_idno = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_DeliveryTo.Text)
        End If
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Weaving_JobCardNo, txt_vehicle, "Weaving_JobCard_Head", "Weaving_JobCode_forSelection", "(ledger_idno = " & Str(Val(Led_idno)) & ")", "(Weaving_JobCard_Code = '')")

    End Sub

    Private Sub cbo_weaving_job_no_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_Weaving_JobCardNo.KeyDown
        Dim Led_idno As Integer = 0

        Led_idno = 0
        If Trim(cbo_DeliveryTo.Text) <> "" Then
            Led_idno = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_DeliveryTo.Text)
        End If

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Weaving_JobCardNo, cbo_TransportName, txt_vehicle, "Weaving_JobCard_Head", "Weaving_JobCode_forSelection", "(ledger_idno = " & Str(Val(Led_idno)) & ")", "(Weaving_JobCard_Code = '')")
    End Sub

    Private Sub cbo_weaving_job_no_GotFocus(sender As Object, e As EventArgs) Handles cbo_Weaving_JobCardNo.GotFocus
        Dim Led_idno As Integer = 0

        Led_idno = 0
        If Trim(cbo_DeliveryTo.Text) <> "" Then
            Led_idno = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_DeliveryTo.Text)
        End If

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Weaving_JobCard_Head", "Weaving_JobCode_forSelection", "(ledger_idno = " & Str(Val(Led_idno)) & ")", "(Weaving_JobCard_Code = '')")

    End Sub


    Private Sub txt_PartyDcNo_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_PartyDcNo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_SetNo.Focus()
        End If
    End Sub

    Private Sub txt_PartyDcNo_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_PartyDcNo.KeyDown
        If e.KeyCode = 40 Then
            txt_SetNo.Focus()
        End If


        If e.KeyCode = 38 Then
            cbo_Sizing.Focus()
        End If

    End Sub

    Private Sub cbo_jobcardno_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_Sizing_JobCardNo.KeyPress
        Dim Led_idno As Integer = 0

        If Trim(cbo_Sizing.Text) <> "" Then
            Led_idno = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Sizing.Text)
        End If
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Sizing_JobCardNo, cbo_TransportName, "Sizing_JobCard_Head", "Sizing_JobCode_forSelection", "ledger_idno = " & Str(Val(Led_idno)) & "", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_jobcardno_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_Sizing_JobCardNo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Sizing_JobCardNo, cbo_DeliveryTo, cbo_TransportName, "Sizing_JobCard_Head", "Sizing_JobCode_forSelection", "", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_jobcardno_GotFocus(sender As Object, e As EventArgs) Handles cbo_Sizing_JobCardNo.GotFocus
        Dim Led_idno As Integer = 0

        Led_idno = 0
        If Trim(cbo_Sizing.Text) <> "" Then
            Led_idno = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Sizing.Text)
        End If


        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Sizing_JobCard_Head", "Sizing_JobCode_forSelection", "(ledger_idno = " & Str(Val(Led_idno)) & ")", "(Ledger_IdNo = 0)")
    End Sub
    Private Sub cbo_Grid_EndsCount_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_EndsCount.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")
    End Sub
    Private Sub cbo_Grid_EndsCount_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_EndsCount.KeyDown
        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_EndsCount, Nothing, Nothing, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")
        With dgv_PavuDetails
            If e.KeyCode = 38 And cbo_Grid_EndsCount.DroppedDown = False Or (e.Control = True And e.KeyCode = 38) Then
                If .Visible Then
                    e.Handled = True
                    e.SuppressKeyPress = True
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)
                    .CurrentCell.Selected = True
                End If
            End If

            If e.KeyCode = 40 And cbo_Grid_EndsCount.DroppedDown = False Or (e.Control = True And e.KeyCode = 40) Then
                If .Visible Then
                    e.Handled = True
                    e.SuppressKeyPress = True
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(DgvCol_details.PCS)
                    .CurrentCell.Selected = True
                End If
            End If

        End With
    End Sub

    Private Sub cbo_Grid_EndsCount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_EndsCount.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_EndsCount, Nothing, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")

        With dgv_PavuDetails

            If .Visible Then
                If Asc(e.KeyChar) = 13 Then
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(DgvCol_details.PCS)
                    .CurrentCell.Selected = True
                End If
            End If

        End With

    End Sub
    Private Sub cbo_Grid_EndsCount_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_EndsCount.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New EndsCount_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_EndsCount.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub
    Private Sub cbo_Grid_EndsCount_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_EndsCount.TextChanged
        If IsNothing(dgv_PavuDetails.CurrentCell) Then Exit Sub

        Try
            With dgv_PavuDetails
                If cbo_Grid_EndsCount.Visible = True Then

                    If Val(cbo_Grid_EndsCount.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = DgvCol_details.ENDS_COUNT Then
                        .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_EndsCount.Text)
                    End If

                End If

            End With
        Catch ex As Exception

        End Try
    End Sub

    Private Sub txt_vehicle_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_vehicle.KeyDown
        If e.KeyCode = 38 Then
            If cbo_Weaving_JobCardNo.Visible And cbo_Weaving_JobCardNo.Enabled Then
                cbo_Weaving_JobCardNo.Focus()
            ElseIf txt_Freight_For.Visible And txt_Freight_For.Enabled Then
                txt_Freight_For.Focus()
            ElseIf cbo_ClothSales_OrderCode_forSelection.Visible And cbo_ClothSales_OrderCode_forSelection.Enabled Then
                cbo_ClothSales_OrderCode_forSelection.Focus()
            Else
                cbo_TransportName.Focus()
            End If
        ElseIf e.KeyCode = 40 Then

            txt_Freight.Focus()
        End If
    End Sub

    Private Sub txt_vehicle_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_vehicle.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_Freight.Focus()
        End If
    End Sub

    Private Sub txt_Freight_For_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Freight_For.KeyDown
        If e.KeyCode = 38 Then
            cbo_TransportName.Focus()
        ElseIf e.KeyCode = 40 Then
            txt_vehicle.Focus()
        End If
    End Sub

    Private Sub txt_Freight_For_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_Freight_For.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_vehicle.Focus()
        End If
    End Sub
    Private Sub txt_PickUp_Perc_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles txt_PickUp_Perc.KeyDown
        If e.KeyValue = 38 Then
            txt_Freight.Focus()
        End If
        If (e.KeyValue = 40) Then
            If dgv_PavuDetails.Rows.Count > 0 Then
                dgv_PavuDetails.Focus()
                dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(0).Cells(DgvCol_details.BEAM_NO)

            Else
                btn_save.Focus()
            End If
        End If

    End Sub

    Private Sub txt_PickUp_Perc_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txt_PickUp_Perc.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
        If Asc(e.KeyChar) = 13 Then
            If dgv_PavuDetails.Rows.Count > 0 Then
                dgv_PavuDetails.Focus()
                dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(0).Cells(DgvCol_details.BEAM_NO)

            Else
                btn_save.Focus()
            End If
        End If
    End Sub
    Private Function Sizing_Weight_Calculation_AutoMatic(ByVal vCurrow As Integer, ByVal Pavu_Meters As String) As String

        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        Dim Warp_Wgt = ""
        Dim Sizing_Wgt = ""
        Dim Pickup = ""
        Dim Ends = ""
        Dim Endscnt_ID = 0


        Warp_Wgt = 0
        Sizing_Wgt = 0
        Ends = 0
        Pickup = 0

        Try
            With dgv_PavuDetails

                If chk_Calculate_Sizing_Wgt_Auto.Checked = True Then

                    Endscnt_ID = Common_Procedures.EndsCount_NameToIdNo(con, dgv_PavuDetails.Rows(vCurrow).Cells(DgvCol_details.ENDS_COUNT).Value)


                    If Val(Endscnt_ID) = 0 Then Exit Function


                    If Val(Endscnt_ID) <> Val(vEndscount_tag) Then

                        vEndscount_tag = Endscnt_ID

                        vEnds_Name = 0
                        vResultant_Count = 0

                        da = New SqlClient.SqlDataAdapter("Select a.Ends_Name,b.Resultant_Count From Endscount_Head a Inner Join Count_Head b on a.Count_Idno = b.Count_Idno Where Endscount_Idno = " & Val(Endscnt_ID) & " ", con)
                        da.Fill(dt)

                        If dt.Rows.Count > 0 Then

                            vEnds_Name = dt.Rows(0)(0).ToString
                            vResultant_Count = dt.Rows(0)(1).ToString

                        End If

                    End If

                    ' WARP WEIGHT FORMULA =  (ends * pavumeter * 0.00059  ) / count

                    Warp_Wgt = 0
                    If Val(vResultant_Count) <> 0 Then
                        Warp_Wgt = (Val(vEnds_Name) * Val(Pavu_Meters) * 0.00059) / Val(vResultant_Count)
                    End If


                    'SIZING WEIGHT FORMULA =  WARP WEIGHT + PICKUP%

                    Pickup = 0
                    If Val(txt_PickUp_Perc.Text) <> 0 Then
                        Pickup = (Val(Warp_Wgt) * Val(txt_PickUp_Perc.Text)) / 100
                    End If

                    Sizing_Wgt = Val(Warp_Wgt) + Val(Pickup)

                    .Rows(vCurrow).Cells(DgvCol_details.SIZING_WEIGHT).Value = Format(Val(Sizing_Wgt), "########0.000")


                    Sizing_Weight_Calculation_AutoMatic = Val(.Rows(vCurrow).Cells(DgvCol_details.SIZING_WEIGHT).Value)

                End If

            End With


        Catch ex As Exception

            MessageBox.Show(ex.Message, "GET SIZING WEIGHT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try


    End Function

    Private Sub chk_Calculate_Sizing_Wgt_Auto_CheckedChanged(sender As Object, e As EventArgs) Handles chk_Calculate_Sizing_Wgt_Auto.CheckedChanged
        If chk_Calculate_Sizing_Wgt_Auto.Checked = True Then
            dgv_PavuDetails.Columns(DgvCol_details.SIZING_WEIGHT).ReadOnly = True
        Else
            dgv_PavuDetails.Columns(DgvCol_details.SIZING_WEIGHT).ReadOnly = False
        End If
        TotalPavu_Calculation()

    End Sub

    Private Sub txt_PickUp_Perc_TextChanged(sender As Object, e As EventArgs) Handles txt_PickUp_Perc.TextChanged
        TotalPavu_Calculation()
    End Sub
    Private Sub cbo_Grid_Beam_width_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Beam_width.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Beam_Width_head", "Beam_Width_name", "", "(Beam_Width_Idno = 0)")

    End Sub

    Private Sub cbo_Grid_Beam_width_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Beam_width.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_Beam_width, Nothing, Nothing, "Beam_Width_head", "Beam_Width_name", "", "(Beam_Width_Idno = 0)")
        With dgv_PavuDetails

            If (e.KeyValue = 38 And cbo_Grid_Beam_width.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
                .CurrentCell.Selected = True
            End If


            If (e.KeyValue = 40 And cbo_Grid_Beam_width.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
                .CurrentCell.Selected = True
            End If

        End With
    End Sub

    Private Sub cbo_Grid_Beam_width_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_Beam_width.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_Beam_width, Nothing, "Beam_Width_head", "Beam_Width_name", "", "(Beam_Width_Idno = 0)")
        If Asc(e.KeyChar) = 13 Then

            With dgv_PavuDetails
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
                .CurrentCell.Selected = True

            End With

        End If
    End Sub

    Private Sub cbo_Grid_Beam_width_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Beam_width.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Beam_Width_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_Beam_width.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If

    End Sub
    Private Sub cbo_Grid_Beam_width_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Beam_width.TextChanged
        If IsNothing(dgv_PavuDetails.CurrentCell) Then Exit Sub

        Try
            If cbo_Grid_Beam_width.Visible Then
                With dgv_PavuDetails
                    If Val(cbo_Grid_Beam_width.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = DgvCol_details.BEAM_WIDTH Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_Beam_width.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub
    Private Sub cbo_Grid_BeamType_GotFocus(sender As Object, e As EventArgs) Handles cbo_Grid_BeamType.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "LoomTYpe_Head", "LoomType_Name", "", "(LoomType_IdNo = 0)")
    End Sub
    Private Sub cbo_Grid_BeamType_TextChanged(sender As Object, e As EventArgs) Handles cbo_Grid_BeamType.TextChanged
        If IsNothing(dgv_PavuDetails.CurrentCell) Then Exit Sub

        Try
            If cbo_Grid_BeamType.Visible Then
                With dgv_PavuDetails
                    If Val(cbo_Grid_BeamType.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = DgvCol_details.BEAM_TYPE Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_BeamType.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Grid_BeamType_KeyUp(sender As Object, e As KeyEventArgs) Handles cbo_Grid_BeamType.KeyUp

        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New LoomType_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_BeamType.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If


    End Sub

    Private Sub cbo_Grid_BeamType_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_Grid_BeamType.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_BeamType, "", "LoomType_Head", "LoomType_Name", "", "(LoomType_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            With dgv_PavuDetails

                If dgv_PavuDetails.Columns(DgvCol_details.SIZING_WEIGHT).Visible Then
                    .Focus()
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(DgvCol_details.SIZING_WEIGHT)
                    .CurrentCell.Selected = True
                Else
                    If .CurrentCell.RowIndex = .RowCount - 1 Then

                        .Rows.Add()
                        .Focus()
                        dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(dgv_PavuDetails.CurrentRow.Index + 1).Cells(DgvCol_details.BEAM_NO)
                        .CurrentCell.Selected = True
                    Else
                        .Focus()
                        dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(dgv_PavuDetails.CurrentRow.Index + 1).Cells(DgvCol_details.BEAM_NO)
                        .CurrentCell.Selected = True
                    End If

                End If
            End With

        End If


    End Sub

    Private Sub cbo_Grid_BeamType_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_Grid_BeamType.KeyDown

        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_BeamType, cbo_Grid_Beam_width, "", "LoomTYpe_Head", "LoomTYpe_Name", "", "(LoomTYpe_IdNo = 0)")

        With dgv_PavuDetails

            If (e.KeyValue = 38 And cbo_Grid_BeamType.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(DgvCol_details.BEAM_WIDTH)
                .CurrentCell.Selected = True
            End If


            If (e.KeyValue = 40 And cbo_Grid_BeamType.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                If dgv_PavuDetails.Columns(DgvCol_details.SIZING_WEIGHT).Visible Then
                    .Focus()
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(DgvCol_details.SIZING_WEIGHT)
                    .CurrentCell.Selected = True
                Else
                    If .CurrentCell.RowIndex = .RowCount - 1 Then

                        .Rows.Add()
                        .Focus()
                        dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(dgv_PavuDetails.CurrentRow.Index + 1).Cells(DgvCol_details.BEAM_NO)
                        .CurrentCell.Selected = True
                    Else
                        .Focus()
                        dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(dgv_PavuDetails.CurrentRow.Index + 1).Cells(DgvCol_details.BEAM_NO)
                        .CurrentCell.Selected = True
                    End If

                End If


            End If

        End With


    End Sub
    Private Sub cbo_Order_Indent_Enter(sender As Object, e As EventArgs) Handles cbo_ClothSales_OrderCode_forSelection.Enter


        vCLO_CONDT = "(ClothSales_Order_Code LIKE '%/" & Trim(FnYearCode1) & "' or ClothSales_Order_Code LIKE '%/" & Trim(FnYearCode2) & "' and Order_Close_Status = 0 )"

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "ClothSales_Order_Head", "ClothSales_OrderCode_forSelection", vCLO_CONDT, "(ClothSales_Order_Code = '999999/00-00')")

    End Sub

    Private Sub cbo_ClothSales_OrderCode_forSelection_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_ClothSales_OrderCode_forSelection.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, sender, cbo_TransportName, txt_vehicle, "ClothSales_Order_Head", "ClothSales_OrderCode_forSelection", vCLO_CONDT, "(ClothSales_Order_Code = '999999/00-00')")
        'If (e.KeyCode = 40 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
        '    If Trim(Common_Procedures.settings.CustomerCode) = "1267" Then
        '        txt_Duedays.Focus()
        '    Else
        '        txt_BillNo.Focus()
        '    End If
        'End If
        'If (e.KeyCode = 38 And sender.droppeddown = False) Or (e.Control = True And e.KeyValue = 38) Then
        '    If Trim(Common_Procedures.settings.CustomerCode) = "1267" Then
        '        cbo_Delvat.Focus()
        '    Else
        '        cbo_PurchaseAc.Focus()
        '    End If
        'End If
    End Sub

    Private Sub cbo_ClothSales_OrderCode_forSelection_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_ClothSales_OrderCode_forSelection.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, sender, txt_vehicle, "ClothSales_Order_Head", "ClothSales_OrderCode_forSelection", vCLO_CONDT, "(ClothSales_Order_Code = '999999/00-00')")

        'If Asc(e.KeyChar) = 13 Then
        '    'If Trim(Common_Procedures.settings.CustomerCode) = "1267" Then
        '    'txt_Duedays.Focus()
        '    'Else
        '    '    txt_BillNo.Focus()
        '    'End If
        'End If
    End Sub

    Private Sub btn_BarcodePrint_Click(sender As Object, e As EventArgs) Handles btn_BarcodePrint.Click
        'Common_Procedures.Print_OR_Preview_Status = 0
        'Prn_BarcodeSticker = True
        Printing_BarCode_Sticker_Format1_1608_DosPrint()
    End Sub
    Private Sub Printing_BarCode_Sticker_Format1_1608_DosPrint()
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim NewCode As String = ""
        Dim PrnTxt As String = ""
        Dim LnCnt As Integer = 0
        Dim I As Integer = 0
        Dim NoofItems_PerPage As Integer
        Dim vBarCode As String = ""
        Dim vFldMtrs As String = "", vPcs As String = ""
        Dim ItmNm1 As String, ItmNm2 As String
        Dim vYrCode As String = ""
        Dim prtFrm As String = ""
        Dim prtTo As String = ""
        Dim Condt As String = ""
        Dim prn_DetAr(,) As String

        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0

        Erase prn_DetAr
        prn_DetAr = New String(100, 10) {}

        Try
            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            da2 = New SqlClient.SqlDataAdapter("select a.*,b.*,lh.Ledger_Name,d.Ends_Name from Sizing_Pavu_Receipt_Details a inner join Sizing_Pavu_Receipt_Head b on a.Sizing_Pavu_Receipt_Code = b.Sizing_Pavu_Receipt_Code inner Join Ledger_Head lh ON b.Ledger_IdNo = lh.Ledger_IdNo LEFT OUTER JOIN EndsCount_Head d ON a.EndsCount_IdNo = d.EndsCount_IdNo  where a.Sizing_Pavu_Receipt_Code = '" & Trim(NewCode) & "' Order by a.Sl_No", con)
            prn_DetDt = New DataTable
            da2.Fill(prn_DetDt)

            If prn_DetDt.Rows.Count > 0 Then

                For I = 0 To prn_DetDt.Rows.Count - 1

                    prn_DetIndx = prn_DetIndx + 1

                    prn_DetAr(prn_DetIndx, 0) = Trim(prn_DetDt.Rows(I).Item("Ledger_Name").ToString)
                    prn_DetAr(prn_DetIndx, 1) = Trim(prn_DetDt.Rows(I).Item("Set_No").ToString)
                    prn_DetAr(prn_DetIndx, 2) = Trim(prn_DetDt.Rows(I).Item("Beam_No").ToString)
                    prn_DetAr(prn_DetIndx, 3) = Trim(prn_DetDt.Rows(I).Item("Ends_Name").ToString)
                    prn_DetAr(prn_DetIndx, 4) = Format(Val(prn_DetDt.Rows(I).Item("SizedBeam_Meters").ToString), "##########0.00")
                    prn_DetAr(prn_DetIndx, 5) = Trim(prn_DetDt.Rows(I).Item("Set_Code").ToString) & "/" & prn_DetDt.Rows(I).Item("Beam_No").ToString

                Next

                Common_Procedures.Printing_BarCode_Sticker_Format1_1608_DosPrint(prn_DetDt, prn_DetAr, prn_DetIndx)
            Else

                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub

            End If

            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        'fs = New FileStream(Common_Procedures.Dos_Printing_FileName_Path, FileMode.Create)
        'sw = New StreamWriter(fs, System.Text.Encoding.Default)


        'Try

        '    If prn_DetDt.Rows.Count > 0 Then

        '        Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

        '            vFldMtrs = Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("SizedBeam_Meters").ToString), "##########0.00")

        '            vBarCode = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Set_Code").ToString) & "/" & prn_DetDt.Rows(prn_DetIndx).Item("Beam_No").ToString


        '            If Val(vFldMtrs) <> 0 Then



        '                ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Ledger_Name").ToString)


        '                ItmNm2 = ""
        '                If Len(ItmNm1) > 20 Then
        '                    For I = 20 To 1 Step -1
        '                        If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
        '                    Next I
        '                    If I = 0 Then I = 20

        '                    ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I + 1)
        '                    ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
        '                End If

        '                ItmNm1 = Replace(ItmNm1, """", """""")
        '                ItmNm2 = Replace(ItmNm2, """", """""")

        '                PrnTxt = "SIZE 82.5 mm, 40 mm"
        '                sw.WriteLine(PrnTxt)
        '                PrnTxt = "DIRECTION 0,0"
        '                sw.WriteLine(PrnTxt)
        '                PrnTxt = "REFERENCE 0,0"
        '                sw.WriteLine(PrnTxt)
        '                PrnTxt = "OFFSET 0 mm"
        '                sw.WriteLine(PrnTxt)
        '                PrnTxt = "SET PEEL OFF"
        '                sw.WriteLine(PrnTxt)
        '                PrnTxt = "SET CUTTER OFF"
        '                sw.WriteLine(PrnTxt)
        '                PrnTxt = "SET PARTIAL_CUTTER OFF"
        '                sw.WriteLine(PrnTxt)
        '                PrnTxt = "SET TEAR ON"
        '                sw.WriteLine(PrnTxt)
        '                PrnTxt = "CLS"
        '                sw.WriteLine(PrnTxt)
        '                PrnTxt = "CODEPAGE 1252"
        '                sw.WriteLine(PrnTxt)

        '                PrnTxt = "TEXT 612,274,""ROMAN.TTF"",180,1,12,""SIZING"""
        '                sw.WriteLine(PrnTxt)
        '                PrnTxt = "BOX 33,13,633,305,3"
        '                sw.WriteLine(PrnTxt)
        '                PrnTxt = "TEXT 612,163,""ROMAN.TTF"",180,1,12,""BEAM NO"""
        '                sw.WriteLine(PrnTxt)
        '                PrnTxt = "TEXT 612,112,""ROMAN.TTF"",180,1,12,""ENDS"""
        '                sw.WriteLine(PrnTxt)
        '                PrnTxt = "TEXT 613,63,""ROMAN.TTF"",180,1,12,""METER"""
        '                sw.WriteLine(PrnTxt)
        '                PrnTxt = "TEXT 472,296,""0"",180,31,22,"":"""
        '                sw.WriteLine(PrnTxt)
        '                PrnTxt = "TEXT 464,168,""ROMAN.TTF"",180,1,14,"":"""
        '                sw.WriteLine(PrnTxt)
        '                PrnTxt = "TEXT 464,115,""ROMAN.TTF"",180,1,14,"":"""
        '                sw.WriteLine(PrnTxt)
        '                PrnTxt = "TEXT 464,62,""ROMAN.TTF"",180,1,14,"":"""
        '                sw.WriteLine(PrnTxt)

        '                PrnTxt = "TEXT 440,163,""ROMAN.TTF"",180,1,12,""" & Trim(prn_DetDt.Rows(prn_DetIndx).Item("Beam_No").ToString) & """"
        '                sw.WriteLine(PrnTxt)
        '                PrnTxt = "TEXT 441,112,""ROMAN.TTF"",180,1,12,""" & Trim(prn_DetDt.Rows(prn_DetIndx).Item("Ends_Name").ToString) & """"
        '                sw.WriteLine(PrnTxt)
        '                PrnTxt = "TEXT 441,59,""ROMAN.TTF"",180,1,12,""" & Trim(Val(vFldMtrs)) & """"
        '                sw.WriteLine(PrnTxt)
        '                PrnTxt = "QRCODE 228,215,L,7,A,180,M2,S7,""" & Trim(UCase(vBarCode)) & """"
        '                sw.WriteLine(PrnTxt)

        '                PrnTxt = "TEXT 222,50,""ROMAN.TTF"",180,1,12,""" & Trim(UCase(vBarCode)) & """"
        '                sw.WriteLine(PrnTxt)
        '                PrnTxt = "TEXT 612,215,""ROMAN.TTF"",180,1,12,""SETNO"""
        '                sw.WriteLine(PrnTxt)
        '                PrnTxt = "TEXT 464,217,""ROMAN.TTF"",180,1,14,"":"""
        '                sw.WriteLine(PrnTxt)
        '                PrnTxt = "TEXT 440,213,""ROMAN.TTF"",180,1,12,""" & Trim(prn_DetDt.Rows(prn_DetIndx).Item("Set_No").ToString) & """"
        '                sw.WriteLine(PrnTxt)
        '                PrnTxt = "TEXT 442,293,""ROMAN.TTF"",180,1,11,""" & Trim(ItmNm1) & """"
        '                sw.WriteLine(PrnTxt)
        '                PrnTxt = "TEXT 442,256,""ROMAN.TTF"",180,1,11,""" & Trim(ItmNm2) & """"
        '                sw.WriteLine(PrnTxt)

        '                PrnTxt = "PRINT 1,1"
        '                sw.WriteLine(PrnTxt)

        '            End If

        '            prn_DetIndx = prn_DetIndx + 1

        '        Loop

        '    End If

        '    sw.Close()
        '    fs.Close()
        '    sw.Dispose()
        '    fs.Dispose()

        '    If Val(Common_Procedures.Print_OR_Preview_Status) = 2 Then
        '        Dim p1 As New System.Diagnostics.Process
        '        p1.EnableRaisingEvents = False
        '        p1.StartInfo.FileName = Common_Procedures.Dos_PrintPreView_BatchFileName_Path
        '        p1.StartInfo.WindowStyle = ProcessWindowStyle.Maximized
        '        p1.Start()

        '    Else
        '        Dim p2 As New System.Diagnostics.Process
        '        p2.EnableRaisingEvents = False
        '        p2.StartInfo.FileName = Common_Procedures.Dos_Print_BatchFileName_Path
        '        p2.StartInfo.CreateNoWindow = True
        '        p2.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
        '        p2.Start()

        '    End If

        '    MessageBox.Show("BarCode Sticker Printed", "FOR BARCODE STICKER PRINTING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        'Catch ex As Exception
        '    MessageBox.Show(ex.Message, "DOES NOT PRINT BARCODE STICKER...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)


        'Finally

        '    Try
        '        sw.Close()
        '        fs.Close()
        '        sw.Dispose()
        '        fs.Dispose()
        '    Catch ex As Exception
        '        '-----

        '    End Try

        'End Try

    End Sub
End Class