Imports System.IO
Public Class Sizing_YarnDelivery_Entry
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "SYDEL-"
    Private Pk_Condition_Tex As String = "SSYDC-"
    Private Prec_ActCtrl As New Control
    Private vCbo_ItmNm As String
    Private vcbo_KeyDwnVal As Double
    Private TrnTo_DbName As String = ""
    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl
    Private PrntCnt2ndPageSTS As Boolean = False
    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private sum_Total_Amount As Single
    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
    Private prn_DetSNo As Integer
    Private prn_Status As Integer
    Private prn_TotCopies As Integer = 0
    Private prn_Count As Integer = 0
    Private SaveAll_STS As Boolean = False
    Private fs As FileStream
    Private sw As StreamWriter
    Private LastNo As String = ""
    Private Hz1 As Integer, Hz2 As Integer, Vz1 As Integer, Vz2 As Integer
    Private Corn1 As Integer, Corn2 As Integer, Corn3 As Integer, Corn4 As Integer
    Private LfCon As Integer, RgtCon As Integer


    ' PRAKASH    SIZING 
    Private Prnt_HalfSheet_STS As Boolean = False
    Private vPrnt_2Copy_In_SinglePage As Integer = 0
    ' PRAKASH    SIZING 


    Private Print_PDF_Status As Boolean = False
    Private EMAIL_Status As Boolean = False
    Private WHATSAPP_Status As Boolean = False

    Private Sub clear()

        New_Entry = False
        Insert_Entry = False

        pnl_Back.Enabled = True
        pnl_Filter.Visible = False

        lbl_DcNo.Text = ""
        lbl_DcNo.ForeColor = Color.Black
        chk_Loaded.Checked = False
        chk_Loaded.Visible = False
        dtp_Date.Text = ""
        txt_BookNo.Text = ""
        cbo_Ledger.Text = ""
        cbo_VendorName.Text = ""
        txt_EmptyBags.Text = ""
        txt_EmptyCones.Text = ""
        txt_TexDcNo.Text = ""
        cbo_bagType.Text = ""
        cbo_coneType.Text = ""
        cbo_DeliveryTo.Text = ""
        txt_ElectronicRefNo.Text = ""
        txt_DateAndTimeOFSupply.Text = ""
        txt_Approx_Value.Text = ""
        txt_EmptyBeam.Text = ""
        cbo_BeamWidth.Text = ""
        cbo_Transport.Text = ""
        cbo_VehicleNo.Text = ""
        txt_DeliveryAt.Text = ""
        txt_Remarks.Text = ""
        dtp_Time.Text = ""
        lbl_AvailableStock.Tag = 0
        lbl_AvailableStock.Text = ""
        cbo_Delivered.Text = ""
        txt_kg_Rate.Text = ""
        txt_SlNo.Text = ""
        cbo_CountName.Text = ""
        cbo_YarnType.Text = "MILL"
        cbo_MillName.Text = ""
        cbo_bagType.Text = ""
        cbo_grdBagType.Text = ""
        txt_Bags.Text = ""
        cbo_SetNo.Text = ""
        txt_Cones.Text = ""
        txt_Weight.Text = ""
        txt_GWeight.Text = ""
        txt_Rate.Text = ""
        txt_Amount.Text = ""
        cbo_Godown.Text = ""

        cbo_Det_Location.Text = ""
        txt_DetLotNo.Text = ""

        chk_Printed.Checked = False
        chk_Printed.Enabled = False
        chk_Printed.Visible = False

        If Filter_Status = False Then
            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_CountName.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_CountName.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()
        End If

        cbo_Ledger.Tag = ""
        cbo_YarnType.Tag = ""
        cbo_CountName.Tag = ""
        cbo_MillName.Tag = ""
        cbo_grdBagType.Tag = ""

        If Common_Procedures.settings.CustomerCode = "1282" Then
            chk_Loaded.Visible = True
        Else
            chk_Loaded.Visible = False

        End If

        dgv_Details.Rows.Clear()
        dgv_Details_Total.Rows.Clear()
        dgv_Details_Total.Rows.Add()

        Grid_Cell_DeSelect()

        txt_SlNo.Text = "1"
        cbo_YarnType.Text = "MILL"
        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))
        dtp_Date.Enabled = True
        dtp_Date.BackColor = Color.White

        cbo_Transport.Enabled = True
        cbo_Transport.BackColor = Color.White

        cbo_Ledger.Enabled = True
        cbo_Ledger.BackColor = Color.White

        cbo_VehicleNo.Enabled = True
        cbo_VehicleNo.BackColor = Color.White
        txt_EmptyBeam.Enabled = True
        txt_EmptyBeam.BackColor = Color.White
    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox

        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Then
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

        'If Me.ActiveControl.Name <> cbo_ItemName.Name Then
        '    cbo_ItemName.Visible = False
        'End If
        'If Me.ActiveControl.Name <> cbo_PackingType.Name Then
        '    cbo_PackingType.Visible = False
        'End If

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
        If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
        If IsNothing(dgv_Details_Total.CurrentCell) Then Exit Sub
        If IsNothing(dgv_Filter_Details.CurrentCell) Then Exit Sub

        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details_Total.CurrentCell.Selected = False
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Filter_Details.CurrentCell.Selected = False
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
        Dim vLedID As Integer = 0
        Dim vCntID As String = 0
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        If Val(no) = 0 Then Exit Sub

        clear()

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name as PartyName, c.Transport_Name, d.Beam_Width_Name,  glh.Ledger_Name as Godown_Name from SizingSoft_Yarn_Delivery_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Transport_Head c ON a.Transport_IdNo = c.Transport_IdNo LEFT OUTER JOIN Beam_Width_Head d ON a.Beam_Width_IdNo = d.Beam_Width_IdNo LEFT OUTER JOIN ledger_Head glh ON a.WareHouse_IdNo = glh.Ledger_IdNo Where a.Yarn_Delivery_Code = '" & Trim(NewCode) & "'", con)
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then
                lbl_DcNo.Text = dt1.Rows(0).Item("Yarn_Delivery_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Yarn_Delivery_Date").ToString

                cbo_Ledger.Text = dt1.Rows(0).Item("PartyName").ToString
                txt_BookNo.Text = dt1.Rows(0).Item("Book_No").ToString
                txt_TexDcNo.Text = dt1.Rows(0).Item("Textile_Dc_No").ToString

                txt_EmptyBeam.Text = Val(dt1.Rows(0).Item("Empty_Beam").ToString)
                cbo_BeamWidth.Text = dt1.Rows(0).Item("Beam_Width_Name").ToString
                txt_EmptyBags.Text = dt1.Rows(0).Item("Empty_Bags").ToString
                txt_EmptyCones.Text = dt1.Rows(0).Item("Empty_Cones").ToString

                cbo_Transport.Text = dt1.Rows(0).Item("Transport_Name").ToString

                cbo_VehicleNo.Text = dt1.Rows(0).Item("Vehicle_No").ToString
                cbo_Delivered.Text = dt1.Rows(0).Item("Delivered_By").ToString

                txt_DeliveryAt.Text = dt1.Rows(0).Item("Delivery_At").ToString
                cbo_Godown.Text = dt1.Rows(0).Item("Godown_Name").ToString

                dtp_Time.Text = (dt1.Rows(0).Item("Entry_Time_Text").ToString)
                txt_Remarks.Text = dt1.Rows(0).Item("Remarks").ToString
                cbo_bagType.Text = Common_Procedures.Bag_Type_IdNoToName(con, dt1.Rows(0).Item("Bag_Type_Idno").ToString)
                cbo_coneType.Text = Common_Procedures.Conetype_IdNoToName(con, dt1.Rows(0).Item("Cone_Type_Idno").ToString)
                txt_ElectronicRefNo.Text = dt1.Rows(0).Item("Electronic_Reference_No").ToString
                txt_DateAndTimeOFSupply.Text = dt1.Rows(0).Item("Date_And_Time_Of_Supply").ToString
                txt_kg_Rate.Text = dt1.Rows(0).Item("Kg_Rate").ToString
                txt_Approx_Value.Text = Format(Val(dt1.Rows(0).Item("approx_Value").ToString), "############0.00")

                '  cbo_DeliveryTo.Text = Common_Procedures.Delivery_IdNoToName(con, Val(dt1.Rows(0).Item("DeliveryTo_IdNo").ToString))
                cbo_DeliveryTo.Text = Common_Procedures.Despatch_IdNoToName(con, Val(dt1.Rows(0).Item("DeliveryTo_IdNo").ToString))

                If Trim(cbo_DeliveryTo.Text) = "" Then
                    cbo_DeliveryTo.Text = txt_DeliveryAt.Text
                End If
                cbo_VendorName.Text = Common_Procedures.Vendor_IdNoToName(con, Val(dt1.Rows(0).Item("Vendor_IdNo").ToString))
                lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))

                vLedID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
                'Bw_id = Common_Procedures.BeamWidth_NameToIdNo(con, cbo_BeamWidth.Text)


                chk_Printed.Checked = False
                chk_Printed.Enabled = False
                chk_Printed.Visible = False
                If Val(dt1.Rows(0).Item("PrintOut_Status").ToString) = 1 Then
                    chk_Printed.Checked = True
                    chk_Printed.Visible = True
                    If Val(Common_Procedures.User.IdNo) = 1 Then
                        chk_Printed.Enabled = True
                    End If
                End If
                If IsDBNull(dt1.Rows(0).Item("Gate_Pass_Code").ToString) = False Then
                    If Trim(dt1.Rows(0).Item("Gate_Pass_Code").ToString) <> "" Then
                        LockSTS = True
                    End If
                End If
                If Val(dt1.Rows(0).Item("Loaded_By_Our_Employee").ToString) = 1 Then chk_Loaded.Checked = True

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Count_Name, c.Mill_Name from SizingSoft_Yarn_Delivery_Details a INNER JOIN Count_Head b on a.Count_IdNo = b.Count_IdNo INNER JOIN Mill_Head c on a.Mill_IdNo = c.Mill_IdNo where a.Yarn_Delivery_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                da2.Fill(dt2)

                dgv_Details.Rows.Clear()
                SNo = 0

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_Details.Rows.Add()

                        SNo = SNo + 1
                        dgv_Details.Rows(n).Cells(0).Value = Val(SNo)
                        dgv_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Count_Name").ToString
                        dgv_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Yarn_Type").ToString
                        dgv_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("setcode_forSelection").ToString
                        dgv_Details.Rows(n).Cells(4).Value = dt2.Rows(i).Item("Mill_Name").ToString
                        dgv_Details.Rows(n).Cells(5).Value = Val(dt2.Rows(i).Item("Bags").ToString)
                        dgv_Details.Rows(n).Cells(6).Value = Val(dt2.Rows(i).Item("Cones").ToString)
                        dgv_Details.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Weight").ToString), "########0.000")
                        dgv_Details.Rows(n).Cells(8).Value = dt2.Rows(i).Item("Weaver_Yarn_Requirement_No").ToString
                        dgv_Details.Rows(n).Cells(9).Value = dt2.Rows(i).Item("Weaver_Yarn_Requirement_Code").ToString
                        dgv_Details.Rows(n).Cells(10).Value = dt2.Rows(i).Item("Weaver_Yarn_Requirement_Details_Slno").ToString

                        dgv_Details.Rows(n).Cells(11).Value = Common_Procedures.Bag_Type_IdNoToName(con, Val(dt2.Rows(i).Item("BagType_Idno").ToString()))
                        dgv_Details.Rows(n).Cells(15).Value = Common_Procedures.Ledger_IdNoToName(con, Val(dt2.Rows(i).Item("Location_IdNo").ToString))

                        dgv_Details.Rows(n).Cells(16).Value = dt2.Rows(i).Item("Lot_No").ToString

                        If Common_Procedures.settings.CustomerCode = "1288" Then
                            dgv_Details.Rows(n).Cells(12).Value = dt2.Rows(i).Item("rate").ToString
                            dgv_Details.Rows(n).Cells(13).Value = dt2.Rows(i).Item("amount").ToString
                            dgv_Details.Rows(n).Cells(14).Value = Format(Val(dt2.Rows(i).Item("GrossWeight").ToString), "########0.000")

                        End If

                        vCntID = Common_Procedures.Count_NameToIdNo(con, dgv_Details.Rows(n).Cells(1).Value)

                        'vCntID = dgv_Details.Rows(n).Cells(1).Value

                        If IsDBNull(dt1.Rows(0).Item("Gate_Pass_Code").ToString) = False Then
                            If Trim(dt1.Rows(0).Item("Gate_Pass_Code").ToString) <> "" Then
                                For j = 0 To dgv_Details.ColumnCount - 1
                                    dgv_Details.Rows(n).Cells(j).Style.BackColor = Color.LightGray
                                Next j
                                LockSTS = True
                            End If
                        End If

                        da = New SqlClient.SqlDataAdapter("Select a.Total_bags ,a.Total_Weight  from Yarn_Delivery_Selections_Processing_Details a where  a.Reference_Code<>'" & Trim(Pk_Condition) & Trim(NewCode) & "' and a.Delivery_Code='" & Trim(Pk_Condition) & Trim(NewCode) & "' and a.Total_weight < 0", con)
                        dt = New DataTable
                        da.Fill(dt)
                        If dt.Rows.Count > 0 Then

                            For j = 0 To dgv_Details.ColumnCount - 1
                                dgv_Details.Rows(n).Cells(j).Style.BackColor = Color.LightGray
                            Next j
                            LockSTS = True

                        End If
                        dt.Clear()

                    Next i

                End If




                lbl_AvailableStock.Text = Common_Procedures.get_Yarn_CurrentStock(con, Val(lbl_Company.Tag), vLedID, vCntID)

                        SNo = SNo + 1
                        txt_SlNo.Text = Val(SNo)

                        With dgv_Details_Total
                            If .RowCount = 0 Then .Rows.Add()
                            .Rows(0).Cells(5).Value = Val(dt1.Rows(0).Item("Total_Bags").ToString)
                            .Rows(0).Cells(6).Value = Val(dt1.Rows(0).Item("Total_Cones").ToString)
                            .Rows(0).Cells(7).Value = Format(Val(dt1.Rows(0).Item("Total_Weight").ToString), "########0.000")
                        End With

                        dt2.Clear()
                        dt2.Dispose()
                        da2.Dispose()

                    End If

                    dt1.Clear()
            dt1.Dispose()
            da1.Dispose()
            If LockSTS = True Then

                dtp_Date.Enabled = False
                dtp_Date.BackColor = Color.LightGray

                cbo_Transport.Enabled = False
                cbo_Transport.BackColor = Color.LightGray

                cbo_Ledger.Enabled = False
                cbo_Ledger.BackColor = Color.LightGray

                cbo_VehicleNo.Enabled = False
                cbo_VehicleNo.BackColor = Color.LightGray

                txt_EmptyBeam.Enabled = False
                txt_EmptyBeam.BackColor = Color.LightGray



            End If
            Grid_Cell_DeSelect()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        If Trim(Common_Procedures.settings.CustomerCode) = "1112" Then '----VENUS SIZING
            If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
        Else
            If dtp_Date.Visible And dtp_Date.Enabled Then dtp_Date.Focus()
        End If

    End Sub

    Private Sub YarnDelivery_Entry_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Activated

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And
                Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Ledger.Name)) And
                Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And
                Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Ledger.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_DeliveryTo.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "DELIVERY" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_DeliveryTo.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Transport.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "TRANSPORT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Transport.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_CountName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "COUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_CountName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_MillName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "MILL" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_MillName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_BeamWidth.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "BEAMWIDTH" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_BeamWidth.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_VendorName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "VENDOR" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_VendorName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Godown.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "GODOWN" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Godown.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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
            'MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        FrmLdSTS = False


    End Sub

    Private Sub YarnDelivery_Entry_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
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
        Dim dt12 As New DataTable

        Me.Text = ""

        con.Open()

        btn_Selection.Visible = False

        Panel2.Enabled = True

        dgv_Details.EditMode = DataGridViewEditMode.EditProgrammatically
        dgv_Details.SelectionMode = DataGridViewSelectionMode.FullRowSelect

        If Common_Procedures.settings.Combine_Textile_Sizing_Software_Status = 1 Then
            TrnTo_DbName = Common_Procedures.get_Company_TextileDataBaseName(Trim(Val(Common_Procedures.CompGroupIdNo)))
            'btn_Selection.Visible = True
            ''Panel2.Enabled = False
            ''dgv_Details.EditMode = DataGridViewEditMode.EditOnEnter
            ''dgv_Details.SelectionMode = DataGridViewSelectionMode.CellSelect
            'cbo_Ledger.Width = cbo_Ledger.Width - btn_Selection.Width - 20

        Else
            TrnTo_DbName = Common_Procedures.get_Company_DataBaseName(Trim(Val(Common_Procedures.CompGroupIdNo)))
            btn_Selection.Visible = False

        End If
        Label23.Visible = True

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1036" Then '---- Kalaimagal Sizing (Avinashi)
            lbl_Del_Vendor.Text = "Delivery To"
            cbo_DeliveryTo.Visible = True
            cbo_VendorName.Visible = False
            txt_DeliveryAt.Visible = False
            cbo_DeliveryTo.BringToFront()
            Label23.Visible = False
            'lbl_Del_Vendor.Text = "Delivery To"
            'cbo_DeliveryTo.Visible = False
            'cbo_VendorName.Visible = False
            'txt_DeliveryAt.Visible = True
            'txt_DeliveryAt.BringToFront()
        ElseIf Common_Procedures.settings.Textile_Sizing_Delivery_receipt_Selection = 1 Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Or Trim(Common_Procedures.settings.CustomerCode) = "1282" Or Trim(Common_Procedures.settings.CustomerCode) = "1288" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1263" Then
            lbl_Del_Vendor.Text = "Vendor"
            cbo_DeliveryTo.Visible = False
            txt_DeliveryAt.Visible = False
            cbo_VendorName.Visible = True
            cbo_VendorName.BringToFront()
        Else
            lbl_Del_Vendor.Text = "Delivery To"
            cbo_DeliveryTo.Visible = True
            cbo_VendorName.Visible = False
            txt_DeliveryAt.Visible = False
            cbo_DeliveryTo.BringToFront()
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1282" Then '--- BRT Sizing (Somanur)
            If Val(Common_Procedures.settings.Multi_Godown_Status) = 1 Then
                lbl_Godown_Caption.Visible = True
                cbo_Godown.Visible = True
                cbo_coneType.Width = 122
            End If
        End If

        btn_SaveAll.Visible = False
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then
            btn_SaveAll.Visible = True
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1288" Then '--- BRT Sizing (Somanur)
            If Val(Common_Procedures.settings.Multi_Godown_Status) = 1 Then
                lbl_Godown_Caption.Visible = True
                cbo_Godown.Visible = True
                cbo_Godown.Top = cbo_bagType.Top
                cbo_Godown.Width = cbo_bagType.Width
                cbo_Godown.Location = cbo_bagType.Location

                lbl_Godown_Caption.Top = Label23.Top
                lbl_Godown_Caption.Width = Label23.Width
                lbl_Godown_Caption.Location = Label23.Location

            End If
            txt_Rate.Visible = True
            lbl_Rate_Caption.Visible = True
            txt_Amount.Visible = True
            lbl_Amount_Caption.Visible = True

            lbl_GWT.Visible = True
            txt_GWeight.Visible = True

        Else

            txt_Rate.Visible = False
            lbl_Rate_Caption.Visible = False
            txt_Amount.Visible = False
            lbl_Amount_Caption.Visible = False
            txt_Weight.Top = txt_Rate.Top
            txt_Weight.Left = txt_Rate.Left

            lbl_Weight_Caption.Top = lbl_Rate_Caption.Top
            lbl_Weight_Caption.Left = lbl_Rate_Caption.Left

            lbl_GWT.Visible = False
            txt_GWeight.Visible = False

        End If

        da = New SqlClient.SqlDataAdapter("select a.Ledger_DisplayName from Ledger_AlaisHead a, ledger_head b where (a.Ledger_IdNo = 0 or b.AccountsGroup_IdNo = 10 and a.Close_Status = 0 ) and a.Ledger_IdNo = b.Ledger_IdNo order by a.Ledger_DisplayName", con)
        da.Fill(dt1)
        cbo_Ledger.DataSource = dt1
        cbo_Ledger.DisplayMember = "Ledger_DisplayName"

        da = New SqlClient.SqlDataAdapter("select count_name from Count_Head order by count_name", con)
        da.Fill(dt2)
        cbo_CountName.DataSource = dt2
        cbo_CountName.DisplayMember = "count_name"

        da = New SqlClient.SqlDataAdapter("select mill_name from Mill_Head order by mill_name", con)
        da.Fill(dt3)
        cbo_MillName.DataSource = dt3
        cbo_MillName.DisplayMember = "mill_name"

        da = New SqlClient.SqlDataAdapter("select Transport_Name from Transport_Head order by Transport_Name", con)
        da.Fill(dt4)
        cbo_Transport.DataSource = dt4
        cbo_Transport.DisplayMember = "Transport_Name"

        da = New SqlClient.SqlDataAdapter("select Yarn_Type from YarnType_Head order by Yarn_Type", con)
        da.Fill(dt5)
        cbo_YarnType.DataSource = dt5
        cbo_YarnType.DisplayMember = "Yarn_Type"

        da = New SqlClient.SqlDataAdapter("select Beam_Width_Name from Beam_Width_Head order by Beam_Width_Name", con)
        da.Fill(dt6)
        cbo_BeamWidth.DataSource = dt6
        cbo_BeamWidth.DisplayMember = "Beam_Width_Name"

        da = New SqlClient.SqlDataAdapter("select distinct(Vehicle_No) from SizingSoft_Yarn_Delivery_Head order by Vehicle_No", con)
        da.Fill(dt7)
        cbo_VehicleNo.DataSource = dt7
        cbo_VehicleNo.DisplayMember = "Vehicle_No"

        da = New SqlClient.SqlDataAdapter("select distinct(Delivered_By) from SizingSoft_Yarn_Delivery_Head order by Delivered_By", con)
        da.Fill(dt11)
        cbo_Delivered.DataSource = dt11
        cbo_Delivered.DisplayMember = "Delivered_By"

        da = New SqlClient.SqlDataAdapter("select distinct(setcode_forSelection) from Stock_BabyCone_Processing_Details order by setcode_forSelection", con)
        da.Fill(dt8)
        cbo_SetNo.DataSource = dt8
        cbo_SetNo.DisplayMember = "setcode_forSelection"

        da = New SqlClient.SqlDataAdapter("select Bag_Type_Name from Bag_Type_Head order by Bag_Type_Name", con)
        da.Fill(dt9)
        cbo_bagType.DataSource = dt9
        cbo_bagType.DisplayMember = "Bag_Type_Name"

        da = New SqlClient.SqlDataAdapter("select Cone_Type_Name from Cone_Type_Head order by Cone_Type_Name", con)
        da.Fill(dt10)
        cbo_coneType.DataSource = dt10
        cbo_coneType.DisplayMember = "Cone_Type_Name"

        da = New SqlClient.SqlDataAdapter("select Bag_Type_name from Bag_Type_Head order by Bag_Type_name", con)
        da.Fill(dt12)
        cbo_grdBagType.DataSource = dt12
        cbo_grdBagType.DisplayMember = "Bag_Type_name"

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2
        pnl_Filter.BringToFront()

        pnl_Selection.Visible = False
        pnl_Selection.Left = (Me.Width - pnl_Selection.Width) \ 2
        pnl_Selection.Top = (Me.Height - pnl_Selection.Height) \ 2
        pnl_Selection.BringToFront()

        pnl_Print.Visible = False
        pnl_Print.BringToFront()
        pnl_Print.Left = (Me.Width - pnl_Print.Width) \ 2
        pnl_Print.Top = (Me.Height - pnl_Print.Height) \ 2

        Pnl_DosPrint.Visible = False
        Pnl_DosPrint.BringToFront()
        Pnl_DosPrint.Left = (Me.Width - Pnl_DosPrint.Width) \ 2
        Pnl_DosPrint.Top = (Me.Height - Pnl_DosPrint.Height) \ 2

        btn_UserModification.Visible = False
        If Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_UserModification_Status = 1 Then
            btn_UserModification.Visible = True
        End If

        chk_Printed.Enabled = False
        If Val(Common_Procedures.User.IdNo) = 1 Then
            chk_Printed.Enabled = True
        End If


        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Ledger.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_VendorName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_CountName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_bagType.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_MillName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_SetNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_YarnType.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_BookNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Bags.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_TexDcNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_grdBagType.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_coneType.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_ElectronicRefNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_DeliveryTo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DateAndTimeOFSupply.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Approx_Value.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Delivered.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Godown.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_kg_Rate.GotFocus, AddressOf ControlGotFocus


        AddHandler txt_Cones.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Rate.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Amount.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Weight.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DeliveryAt.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Remarks.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_SlNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_BeamWidth.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_VehicleNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_CountName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Transport.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_EmptyBags.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_EmptyBeam.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_EmptyCones.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_Fromdate.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_ToDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_MillName.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_ElectronicRefNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DateAndTimeOFSupply.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Approx_Value.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_DeliveryTo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Godown.LostFocus, AddressOf ControlLostFocus

        AddHandler btn_save.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Add.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_close.GotFocus, AddressOf ControlGotFocus

        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_TexDcNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Ledger.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_VendorName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_CountName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_MillName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_grdBagType.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_SetNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_YarnType.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_BookNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Bags.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Cones.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Rate.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Amount.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_Weight.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_CountName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_VehicleNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DeliveryAt.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Remarks.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_SlNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_BeamWidth.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Transport.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_EmptyBags.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_EmptyBeam.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_EmptyCones.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_bagType.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_coneType.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Delivered.LostFocus, AddressOf ControlLostFocus

        AddHandler dtp_Filter_Fromdate.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_ToDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_Filter_MillName.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_save.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Add.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_close.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_kg_Rate.LostFocus, AddressOf ControlLostFocus

        'AddHandler dtp_Date.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler txt_BookNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_TexDcNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_ElectronicRefNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Approx_Value.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Bags.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Rate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Rate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Cones.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Weight.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_EmptyBags.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_EmptyCones.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_SlNo.KeyDown, AddressOf TextBoxControlKeyDown
        ' AddHandler txt_Remarks.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_DeliveryAt.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler dtp_Time.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_BookNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Bags.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_TexDcNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_ElectronicRefNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Cones.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_EmptyBags.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Approx_Value.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_EmptyBeam.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_EmptyCones.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_DeliveryAt.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_Fromdate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_ToDate.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler txt_GWeight.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_GWeight.LostFocus, AddressOf ControlLostFocus
        'AddHandler txt_GWeight.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler txt_GWeight.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler cbo_Det_Location.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Det_Location.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DetLotNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DetLotNo.LostFocus, AddressOf ControlLostFocus

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1163" Then
        dtp_Time.Visible = True
        'End If

        Filter_Status = False
        FrmLdSTS = True

        ' Detail Grid Showing  for KKP only
        If Common_Procedures.settings.Yarn_Stock_LotNo_wise_Status Then
            lbl_DetLocation.Visible = True
            cbo_Det_Location.Visible = True
            lbl_DetLotNo.Visible = True
            txt_DetLotNo.Visible = True
        Else
            lbl_DetLocation.Visible = False
            cbo_Det_Location.Visible = False
            lbl_DetLotNo.Visible = False
            txt_DetLotNo.Visible = False
        End If

        new_record()

    End Sub

    Private Sub YarnDelivery_Entry_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub YarnDelivery_Entry_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress

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

                ElseIf Pnl_DosPrint.Visible = True Then
                    btn_Close_DosPrint_Click(sender, e)
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

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim vOrdByNo As String = ""
        Dim vDbName As String = ""
        Dim LedIdNo As Integer
        Dim TexComp_ID As String = 0
        Dim UID As Single = 0
        Dim vUsrNm As String = "", vAcPwd As String = "", vUnAcPwd As String = ""
        Dim Da2 As New SqlClient.SqlDataAdapter
        Dim Dt2 As New DataTable

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1036" Then '----- KALAIMAGAL TEXTILES (AVINASHI)
            Common_Procedures.Password_Input = ""
            Dim g As New Admin_Password
            g.ShowDialog()

            UID = 1
            Common_Procedures.get_Admin_Name_PassWord_From_DB(vUsrNm, vAcPwd, vUnAcPwd)

            vAcPwd = Common_Procedures.Decrypt(Trim(vAcPwd), Trim(Common_Procedures.UserCreation_AcPassWord.passPhrase) & Trim(Val(UID)) & Trim(UCase(vUsrNm)), Trim(Common_Procedures.UserCreation_AcPassWord.saltValue) & Trim(Val(UID)) & Trim(UCase(vUsrNm)))
            vUnAcPwd = Common_Procedures.Decrypt(Trim(vUnAcPwd), Trim(Common_Procedures.UserCreation_UnAcPassWord.passPhrase) & Trim(Val(UID)) & Trim(UCase(vUsrNm)), Trim(Common_Procedures.UserCreation_UnAcPassWord.saltValue) & Trim(Val(UID)) & Trim(UCase(vUsrNm)))

            If Trim(Common_Procedures.Password_Input) <> Trim(vAcPwd) And Trim(Common_Procedures.Password_Input) <> Trim(vUnAcPwd) Then
                MessageBox.Show("Invalid Admin Password", "ADMIN PASSWORD FAILED...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub
            End If
        End If

        If Trim(TrnTo_DbName) <> "" Then
            vDbName = Trim(TrnTo_DbName) & ".."
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.ENTRY_SIZING_JOBWORK_MODULE_YARN_DELIVERY, New_Entry, Me, con, "SizingSoft_Yarn_Delivery_Head", "Yarn_Delivery_Code", NewCode, "Yarn_Delivery_Date", "(Yarn_Delivery_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub


        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If
        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Da = New SqlClient.SqlDataAdapter("select * from SizingSoft_Yarn_Delivery_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Delivery_Code = '" & Trim(NewCode) & "'", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then

            If IsDBNull(Dt1.Rows(0).Item("Gate_Pass_Code").ToString) = False Then
                If Trim(Dt1.Rows(0).Item("Gate_Pass_Code").ToString) <> "" Then
                    MessageBox.Show("Already Gate Pass Prepared", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If

        End If
        Dt1.Clear()
        Dt1.Dispose()
        Da.Dispose()


        Da2 = New SqlClient.SqlDataAdapter("Select a.Total_bags ,a.Total_Weight  from Yarn_Delivery_Selections_Processing_Details a where  a.Reference_Code<>'" & Trim(Pk_Condition) & Trim(NewCode) & "' and a.Delivery_Code='" & Trim(Pk_Condition) & Trim(NewCode) & "'and a.Total_weight < 0", con)
        Dt2 = New DataTable
        Da2.Fill(Dt2)
        If Dt2.Rows.Count > 0 Then
            If Val(Dt2.Rows(0).Item("Total_weight").ToString) < 0 Then
                MessageBox.Show("Already Receipt Prepared", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub
            End If
        End If
        Dt2.Clear()


        tr = con.BeginTransaction

        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text)

            cmd.Connection = con
            cmd.Transaction = tr

            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "DELETE", "SizingSoft_Yarn_Delivery_Head", "Yarn_Delivery_Code", Val(lbl_Company.Tag), NewCode, lbl_DcNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "", "", "Yarn_Delivery_Code, Company_IdNo, for_OrderBy", tr)
            Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "DELETE", "SizingSoft_Yarn_Delivery_Details", "Yarn_Delivery_Code", Val(lbl_Company.Tag), NewCode, lbl_DcNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "Count_IdNo, Yarn_Type, SetCode_ForSelection, Mill_IdNo, Bags, Cones, Weight", "Sl_No", "Yarn_Delivery_Code, For_OrderBy, Company_IdNo, Yarn_Delivery_No, Yarn_Delivery_Date, Ledger_Idno", tr)

            If Common_Procedures.settings.Combine_Textile_Sizing_Software_Status = 1 Then
                LedIdNo = Common_Procedures.get_FieldValue(con, "SizingSoft_Yarn_Delivery_Head", "ledger_idno", "(Yarn_Delivery_Code = '" & Trim(NewCode) & "')", , tr)
                TexComp_ID = Common_Procedures.get_FieldValue(con, "ledger_head", "Textile_To_CompanyIdNo", "(ledger_idno = " & Str(Val(LedIdNo)) & ")", , tr)
                If Val(TexComp_ID) <> 0 Then
                    'cmd.CommandText = "Update " & Trim(vDbName) & "Weaver_Yarn_Requirement_Details set Delivery_Bag = a.Delivery_Bag - (b.Bags) , Delivery_Cone = a.Delivery_Cone - (b.Cones) , Delivery_Weight = a.Delivery_Weight - (b.Weight ) from " & Trim(vDbName) & "Weaver_Yarn_Requirement_Details a, SizingSoft_Yarn_Delivery_Details b Where b.Yarn_Delivery_Code = '" & Trim(NewCode) & "' and a.Weaver_Yarn_Requirement_Code = b.Weaver_Yarn_Requirement_Code and a.Weaver_Yarn_Requirement_Details_SlNo = b.Weaver_Yarn_Requirement_Details_SlNo "
                    'cmd.ExecuteNonQuery()

                    cmd.CommandText = "delete from " & Trim(vDbName) & "SizSoft_SizingSoft_Yarn_Delivery_Details where Yarn_Delivery_Code = '" & Trim(NewCode) & "'"
                    cmd.ExecuteNonQuery()

                    cmd.CommandText = "delete from " & Trim(vDbName) & "SizSoft_SizingSoft_Yarn_Delivery_Head where Yarn_Delivery_Code = '" & Trim(NewCode) & "'"
                    cmd.ExecuteNonQuery()

                    cmd.CommandText = "Delete from " & Trim(vDbName) & "Stock_Yarn_Processing_Details Where Reference_Code = '" & Trim(Pk_Condition_Tex) & Trim(NewCode) & "'"
                    cmd.ExecuteNonQuery()

                End If
            End If

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1163" Then

                Da = New SqlClient.SqlDataAdapter("Select * from SizingSoft_Yarn_Delivery_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Delivery_Code = '" & Trim(NewCode) & "' and yarn_type = 'BABY' and setcode_forSelection <> ''", con)
                Da.SelectCommand.Transaction = tr
                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    For i = 0 To Dt1.Rows.Count - 1

                        cmd.CommandText = "Update Stock_BabyCone_Processing_Details set Delivered_Bags = Delivered_Bags - " & Str(Val(Dt1.Rows(i).Item("Bags").ToString)) & ", Delivered_Cones = Delivered_Cones - " & Str(Val(Dt1.Rows(i).Item("cones").ToString)) & ", Delivered_Weight = Delivered_Weight - " & Str(Val(Dt1.Rows(i).Item("Weight").ToString)) & " Where setcode_forSelection = '" & Trim(Dt1.Rows(i).Item("setcode_forSelection").ToString) & "'"
                        'cmd.CommandText = "Update Stock_BabyCone_Processing_Details set Delivered_Bags = Delivered_Bags - " & Str(Val(Dt1.Rows(i).Item("Bags").ToString)) & ", Delivered_Cones = Delivered_Cones - " & Str(Val(Dt1.Rows(i).Item("cones").ToString)) & ", Delivered_Weight = Delivered_Weight - " & Str(Val(Dt1.Rows(i).Item("Weight").ToString)) & " Where setcode_forSelection = '" & Trim(Dt1.Rows(i).Item("setcode_forSelection").ToString) & "' and Company_IdNo = " & Str(Val(Dt1.Rows(i).Item("Company_IdNo").ToString))
                        cmd.ExecuteNonQuery()

                    Next i

                End If
                Dt1.Clear()

            End If

            cmd.CommandText = "Delete from Stock_WasteMaterial_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Empty_BeamBagCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from SizingSoft_Yarn_Delivery_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Delivery_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from SizingSoft_Yarn_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Delivery_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Yarn_Delivery_Selections_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            tr.Commit()

            Dt1.Dispose()
            Da.Dispose()
            tr.Dispose()
            cmd.Dispose()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        If dtp_Date.Enabled = True And dtp_Date.Visible = True Then dtp_Date.Focus()

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        If Filter_Status = False Then

            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable
            Dim dt2 As New DataTable
            Dim dt3 As New DataTable

            da = New SqlClient.SqlDataAdapter("select a.Ledger_DisplayName from Ledger_AlaisHead a, ledger_head b where (a.Ledger_IdNo = 0 or b.AccountsGroup_IdNo = 10) and a.Ledger_IdNo = b.Ledger_IdNo order by a.Ledger_DisplayName", con)
            da.Fill(dt1)
            cbo_Filter_PartyName.DataSource = dt1
            cbo_Filter_PartyName.DisplayMember = "Ledger_DisplayName"

            da = New SqlClient.SqlDataAdapter("select Count_name from Count_Head order by count_name", con)
            da.Fill(dt2)
            cbo_Filter_CountName.DataSource = dt2
            cbo_Filter_CountName.DisplayMember = "count_name"

            da = New SqlClient.SqlDataAdapter("select Mill_name from Mill_Head order by Mill_name", con)
            da.Fill(dt3)
            cbo_Filter_MillName.DataSource = dt3
            cbo_Filter_MillName.DisplayMember = "Mill_name"

            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate   '.Date.ToShortDateString
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate   '.Date.ToShortDateString
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_CountName.Text = ""
            cbo_Filter_MillName.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_CountName.SelectedIndex = -1
            cbo_Filter_MillName.SelectedIndex = -1
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

            da = New SqlClient.SqlDataAdapter("select top 1 Yarn_Delivery_No from SizingSoft_Yarn_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Yarn_Delivery_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_DcNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Yarn_Delivery_No from SizingSoft_Yarn_Delivery_Head where for_orderby > " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Yarn_Delivery_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_DcNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Yarn_Delivery_No from SizingSoft_Yarn_Delivery_Head where for_orderby < " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Yarn_Delivery_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 Yarn_Delivery_No from SizingSoft_Yarn_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Yarn_Delivery_No desc", con)
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
        Dim dt As New DataTable
        Dim NewID As Integer = 0

        Try
            clear()

            New_Entry = True

            lbl_DcNo.Text = Common_Procedures.get_MaxCode(con, "SizingSoft_Yarn_Delivery_Head", "Yarn_Delivery_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

            lbl_DcNo.ForeColor = Color.Red

            dtp_Time.Text = Format(Now, "hh:mm tt").ToString


        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        dt.Dispose()
        da.Dispose()

        If Trim(Common_Procedures.settings.CustomerCode) = "1112" Then '----VENUS SIZING
            If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
        Else
            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
        End If

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim RecCode As String

        Try

            inpno = InputBox("Enter Receipt No.", "FOR FINDING...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Yarn_Delivery_No from SizingSoft_Yarn_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Delivery_Code = '" & Trim(RecCode) & "'", con)
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
                MessageBox.Show("Receipt No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

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

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.ENTRY_SIZING_JOBWORK_MODULE_YARN_DELIVERY, New_Entry, Me) = False Then Exit Sub

        Try

            inpno = InputBox("Enter New Receipt No.", "FOR NEW RECEIPT INSERTION...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Yarn_Delivery_No from SizingSoft_Yarn_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Delivery_Code = '" & Trim(RecCode) & "'", con)
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
                    MessageBox.Show("Invalid Receipt No", "DOES NOT INSERT NEW RECEIPT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_DcNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT NEW RECEIPT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim Dt1 As New DataTable
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim led_id As Integer = 0
        Dim trans_id As Integer = 0
        Dim Del_ID As Integer = 0
        Dim Bw_id As Integer = 0
        Dim Cnt_ID As Integer = 0
        Dim Mil_ID As Integer = 0
        Dim Bag_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim Partcls As String = ""
        Dim PBlNo As String = ""
        Dim vTotYrnBags As Single, vTotYrnCones As Single, vTotYrnWeight As Single
        Dim vSetCd As String, vSetNo As String, vStCdSel As String
        Dim Nr As Long = 0
        Dim Bg_Id As Integer = 0
        Dim Gd_Id As Integer = 0
        Dim Con_Id As Integer = 0
        Dim WstBg_Id As Integer = 0
        Dim WstCn_Id As Integer = 0
        Dim VndrNm_Id As Integer = 0
        Dim vOrdByNo As String = ""
        Dim vYrnPartcls As String = ""
        Dim TexComp_ID As String = 0
        Dim TexLed_ID As String = 0
        Dim vEntLedIdNo As String = 0
        Dim TexCnt_iD As String = 0
        Dim TexMil_iD As String = 0
        Dim TexVnd_iD As String = 0
        Dim EntID As String = ""
        Dim vNewFrmTYpe As String = ""
        Dim vDbName As String = ""
        Dim CurStk As Single = 0
        Dim vLedID As Integer = 0
        Dim vCntID As Integer = 0
        Dim LocDet_ID As Integer = 0
        Dim Close_STS As Single = 0

        Dim Posting_for_Status As Boolean = False
        Dim vCOMP_LEDIDNO As Integer = 0
        Dim vDELVLED_COMPIDNO As Integer = 0
        Dim vSELC_RCVDIDNO As Integer
        Dim vREC_Ledtype As String = ""
        Dim vDELV_Ledtype As String = ""
        Dim vDELVAT_IDNO As Integer = 0





        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If




        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(dtp_Date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
            Exit Sub
        End If

        If Not (dtp_Date.Value.Date >= Common_Procedures.Company_FromDate And dtp_Date.Value.Date <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
            Exit Sub
        End If

        Close_STS = 0
        If chk_Loaded.Checked = True Then Close_STS = 1
        If Trim(TrnTo_DbName) <> "" Then
            vDbName = Trim(TrnTo_DbName) & ".."
        End If

        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()


        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.ENTRY_SIZING_JOBWORK_MODULE_YARN_DELIVERY, New_Entry, Me, con, "SizingSoft_Yarn_Delivery_Head", "Yarn_Delivery_Code", NewCode, "Yarn_Delivery_Date", "(Yarn_Delivery_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Delivery_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, Yarn_Delivery_No desc", dtp_Date.Value.Date) = False Then Exit Sub
        led_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
        If led_id = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
            Exit Sub
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1282" Or Trim(Common_Procedures.settings.CustomerCode) = "1288" Then
            If Trim(cbo_VehicleNo.Text) = "" Then
                MessageBox.Show("Invalid Vehicle No", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_VehicleNo.Enabled And cbo_VehicleNo.Visible Then cbo_VehicleNo.Focus()
                Exit Sub
            End If
        End If


        If Trim(cbo_VehicleNo.Text) <> "" Then
            cbo_VehicleNo.Text = Common_Procedures.Vehicle_Number_Remove_Unwanted_Spaces(Trim(cbo_VehicleNo.Text))
        End If

        If dtp_Time.Visible Then
            If New_Entry = True Then
                If Trim(dtp_Time.Text) = "" Or IsDate(dtp_Time.Text) = False Then
                    dtp_Time.Text = Format(Now, "Short Time").ToString
                End If
            End If
            If Trim(dtp_Time.Text) = "" Then
                MessageBox.Show("Invalid Time", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
                Exit Sub
            End If

            If IsDate(dtp_Time.Text) = False Then
                MessageBox.Show("Invalid Time", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
                Exit Sub
            End If
        Else
            If New_Entry = True Or Trim(dtp_Time.Text) = "" Or IsDate(dtp_Time.Text) = False Then
                dtp_Time.Value = Now
            End If
        End If

        trans_id = Common_Procedures.Transport_NameToIdNo(con, cbo_Transport.Text)

        Bw_id = Common_Procedures.BeamWidth_NameToIdNo(con, cbo_BeamWidth.Text)
        Bg_Id = Common_Procedures.BagType_NameToIdNo(con, cbo_bagType.Text)
        Gd_Id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Godown.Text)
        Con_Id = Common_Procedures.ConeType_NameToIdNo(con, cbo_coneType.Text)
        'Del_ID = Common_Procedures.Delivery_AlaisNameToIdNo(con, cbo_DeliveryTo.Text)

        Del_ID = Common_Procedures.Despatch_NameToIdNo(con, cbo_DeliveryTo.Text)

        VndrNm_Id = Common_Procedures.Vendor_AlaisNameToIdNo(con, cbo_VendorName.Text)

        If Del_ID = 0 Then
            cbo_DeliveryTo.Text = cbo_Ledger.Text
            Del_ID = Common_Procedures.Delivery_AlaisNameToIdNo(con, cbo_DeliveryTo.Text)
        End If

        For i = 0 To dgv_Details.RowCount - 1

            If Val(dgv_Details.Rows(i).Cells(7).Value) <> 0 Then

                Cnt_ID = Common_Procedures.Count_NameToIdNo(con, Trim(dgv_Details.Rows(i).Cells(1).Value))
                If Cnt_ID = 0 Then
                    MessageBox.Show("Invalid Count Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    If cbo_CountName.Enabled And cbo_CountName.Visible Then cbo_CountName.Focus()
                    Exit Sub
                End If


                If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1006" Then
                    If Trim(dgv_Details.Rows(i).Cells(2).Value) = "" Then
                        MessageBox.Show("Invalid Yarn Type", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If cbo_YarnType.Enabled And cbo_YarnType.Visible Then cbo_YarnType.Focus()
                        Exit Sub
                    End If
                End If

                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1102" Then
                    Bw_id = Common_Procedures.BeamWidth_NameToIdNo(con, cbo_BeamWidth.Text)
                    If Val(txt_EmptyBeam.Text) <> 0 And Bw_id = 0 Then
                        MessageBox.Show("Invalid Beam Width", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If cbo_BeamWidth.Enabled And cbo_BeamWidth.Visible Then cbo_BeamWidth.Focus()
                        Exit Sub
                    End If
                End If



                If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1034" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1163" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1288" Then
                    If Trim(UCase(dgv_Details.Rows(i).Cells(2).Value)) = "BABY" And Trim(dgv_Details.Rows(i).Cells(3).Value) = "" Then
                        MessageBox.Show("Invalid Set No", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If cbo_SetNo.Enabled And cbo_SetNo.Visible Then cbo_SetNo.Focus()
                        Exit Sub
                    End If
                End If



                Mil_ID = Common_Procedures.Mill_NameToIdNo(con, Trim(dgv_Details.Rows(i).Cells(4).Value))
                If Mil_ID = 0 Then
                    MessageBox.Show("Invalid Mill Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    If cbo_MillName.Enabled And cbo_MillName.Visible Then cbo_MillName.Focus()
                    Exit Sub
                End If


                If Common_Procedures.settings.CustomerCode = "1288" Then
                    Dim l As Integer = Common_Procedures.Ledger_AlaisNameToIdNo(con, Trim(dgv_Details.Rows(i).Cells(15).Value))
                    If l = 0 Then
                        MessageBox.Show("Invalid Location Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If cbo_Det_Location.Enabled And cbo_Det_Location.Visible Then cbo_Det_Location.Focus()
                        Exit Sub
                    End If
                End If

                Bag_ID = Common_Procedures.BagType_NameToIdNo(con, Trim(dgv_Details.Rows(i).Cells(11).Value))

                If Common_Procedures.settings.CustomerCode = "1282" And Trim(dgv_Details.Rows(i).Cells(2).Value.ToString.ToUpper) = "MILL" Then

                    Da = New SqlClient.SqlDataAdapter("select * from Yarn_Receipt_Details where Ledger_IdNo = " & Str(Val(led_id)) & " and mill_idno = " & Str(Val(Mil_ID)) & " and count_idno = " & Str(Val(Cnt_ID)), con)
                    'Da = New SqlClient.SqlDataAdapter("select * from Mill_Count_Details where mill_idno = " & Str(Val(Mil_ID)) & " and count_idno = " & Str(Val(Cnt_ID)), con)
                    Da.Fill(Dt)

                    If Val(Dt.Rows.Count) = 0 Then
                        MessageBox.Show("Invalid Mill Name Or Count Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If cbo_MillName.Enabled And cbo_MillName.Visible Then cbo_MillName.Focus()
                        Exit Sub
                    End If

                    Dt.Clear()
                    Dt.Dispose()
                    Da.Dispose()
                End If

            End If

        Next

        vTotYrnBags = 0 : vTotYrnCones = 0 : vTotYrnWeight = 0
        If dgv_Details_Total.RowCount > 0 Then
            vTotYrnBags = Val(dgv_Details_Total.Rows(0).Cells(5).Value())
            vTotYrnCones = Val(dgv_Details_Total.Rows(0).Cells(6).Value())
            vTotYrnWeight = Val(dgv_Details_Total.Rows(0).Cells(7).Value())
        End If
        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Da = New SqlClient.SqlDataAdapter("select * from SizingSoft_Yarn_Delivery_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Delivery_Code = '" & Trim(NewCode) & "'", con)
        'Dt1 = New DataTable
        Da.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then

            If IsDBNull(Dt1.Rows(0).Item("Gate_Pass_Code").ToString) = False Then
                If Trim(Dt1.Rows(0).Item("Gate_Pass_Code").ToString) <> "" Then
                    MessageBox.Show("Already Gate Pass Prepared", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If

        End If
        Dt1.Clear()
        Dt1.Dispose()
        Da.Dispose()

        vLedID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
        'If Common_Procedures.settings.CustomerCode <> "1006" And Common_Procedures.settings.CustomerCode <> "1036" Then

        '    If dgv_Details.Rows.Count > 0 Then
        '        For i = 0 To dgv_Details.Rows.Count - 1
        '            vLedID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
        '            vCntID = Common_Procedures.Count_NameToIdNo(con, dgv_Details.Rows(i).Cells(1).Value)
        '            CurStk = Common_Procedures.get_Yarn_CurrentStock(con, Val(lbl_Company.Tag), led_id, vCntID)
        '            If Val(CurStk) <= 0 Then
        '                MessageBox.Show("Invalid Stock : Current Stock is " & Trim(Format(Val(CurStk), "#########0.000")), "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '                Exit Sub
        '            End If
        '        Next
        '    End If
        'End If

        vCOMP_LEDIDNO = 0
        vDELVLED_COMPIDNO = 0

        If Common_Procedures.settings.Textile_Sizing_Delivery_receipt_Selection = 1 Then

            vCOMP_LEDIDNO = Common_Procedures.get_FieldValue(con, "Company_Head", "Sizing_To_LedgerIdNo", "(Company_idno = " & Str(Val(lbl_Company.Tag)) & ")")
            vDELVLED_COMPIDNO = Val(Common_Procedures.get_FieldValue(con, "Company_Head", "Company_idno", "(Sizing_To_LedgerIdNo = " & Str(Val(vLedID)) & ")"))

        End If
        Amount_Calultation()

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_DcNo.Text = Common_Procedures.get_MaxCode(con, "SizingSoft_Yarn_Delivery_Head", "Yarn_Delivery_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@DcDate", dtp_Date.Value.Date)

            vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text)




            If New_Entry = True Then

                If Trim(txt_DateAndTimeOFSupply.Text) = "" Then txt_DateAndTimeOFSupply.Text = Format(Now, "dd-MM-yyyy hh:mm tt")

                cmd.CommandText = "Insert into SizingSoft_Yarn_Delivery_Head(        User_IdNo     ,   Yarn_Delivery_Code    ,                 Company_IdNo      ,          Yarn_Delivery_No     ,                                     for_OrderBy                        , Yarn_Delivery_Date,               Book_No          ,              Textile_Dc_No      ,          Ledger_IdNo     ,                 Empty_Bags           ,                 Empty_Cones           ,       Beam_Width_IdNo   ,                 Empty_Beam           ,          Transport_IdNo    ,               Vehicle_No           ,               Delivery_At           ,               Remarks            ,              Total_Bags       ,               Total_Cones      ,              Total_Weight       ,            Bag_Type_Idno ,         Cone_Type_Idno   ,               Electronic_Reference_No    ,               Date_And_Time_Of_Supply        , DeliveryTO_Idno     ,             Approx_Value           ,         Entry_Time_Text       ,         Vendor_Idno    ,               Delivered_By         ,     WareHouse_IdNo,   Loaded_By_Our_Employee , Kg_Rate  ) " &
                                  "Values                        (" & Str(Common_Procedures.User.IdNo) & " , '" & Trim(NewCode) & "' , " & Str(Val(lbl_Company.Tag)) & " , '" & Trim(lbl_DcNo.Text) & "' , " & Str(Val(vOrdByNo)) & " ,            @DcDate, '" & Trim(txt_BookNo.Text) & "','" & Trim(txt_TexDcNo.Text) & "' , " & Str(Val(led_id)) & " , " & Str(Val(txt_EmptyBags.Text)) & " , " & Str(Val(txt_EmptyCones.Text)) & " , " & Str(Val(Bw_id)) & " , " & Str(Val(txt_EmptyBeam.Text)) & " , " & Str(Val(trans_id)) & " , '" & Trim(cbo_VehicleNo.Text) & "' , '" & Trim(cbo_Godown.Text) & "' , '" & Trim(txt_Remarks.Text) & "' , " & Str(Val(vTotYrnBags)) & " , " & Str(Val(vTotYrnCones)) & " , " & Str(Val(vTotYrnWeight)) & " , " & Str(Val(Bg_Id)) & "  , " & Str(Val(Con_Id)) & " , '" & Trim(txt_ElectronicRefNo.Text) & "' , '" & Trim(txt_DateAndTimeOFSupply.Text) & "' , " & Val(Del_ID) & " , " & Val(txt_Approx_Value.Text) & " , '" & Trim(dtp_Time.Text) & "' , " & Val(VndrNm_Id) & " , '" & Trim(cbo_Delivered.Text) & "' , " & Val(Gd_Id) & ",                                    " & Val(Close_STS) & "  ,   " & Val(txt_kg_Rate.Text) & ")"
                cmd.ExecuteNonQuery()

            Else

                Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "OLD", "SizingSoft_Yarn_Delivery_Head", "Yarn_Delivery_Code", Val(lbl_Company.Tag), NewCode, lbl_DcNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Yarn_Delivery_Code, Company_IdNo, for_OrderBy", tr)
                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "OLD", "SizingSoft_Yarn_Delivery_Details", "Yarn_Delivery_Code", Val(lbl_Company.Tag), NewCode, lbl_DcNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "Count_IdNo, Yarn_Type, SetCode_ForSelection, Mill_IdNo, Bags, Cones, Weight", "Sl_No", "Yarn_Delivery_Code, For_OrderBy, Company_IdNo, Yarn_Delivery_No, Yarn_Delivery_Date, Ledger_Idno", tr)

                '  POSTING FOR TEXTILE ===================================================  DELETE

                '  POSTING FOR TEXTILE ===================================================  STATUS
                Posting_for_Status = False
                If Common_Procedures.settings.Combine_Textile_Sizing_Software_Status = 1 Then

                    vEntLedIdNo = Common_Procedures.get_FieldValue(con, "SizingSoft_Yarn_Delivery_Head", "ledger_idno", "(Yarn_Delivery_Code = '" & Trim(NewCode) & "')", , tr)
                    TexComp_ID = Common_Procedures.get_FieldValue(con, "ledger_head", "Textile_To_CompanyIdNo", "(ledger_idno = " & Str(Val(vEntLedIdNo)) & ")", , tr)

                    If Val(TexComp_ID) <> 0 Then
                        TexLed_ID = Common_Procedures.get_FieldValue(con, "company_head", "Textile_To_SizingIdNo", "(company_idno = " & Str(Val(lbl_Company.Tag)) & ")", , tr)
                        TexVnd_iD = Val(Common_Procedures.get_FieldValue(con, "Vendor_head", "Textile_To_WeaverIdNo", "(Vendor_idno = " & Str(Val(VndrNm_Id)) & ")", , tr))
                        If TexVnd_iD <> 0 And TexLed_ID <> 0 Then
                            Posting_for_Status = True
                        End If
                    End If
                End If
                '  POSTING FOR TEXTILE ===================================================  STATUS

                If Posting_for_Status Then
                    Nr = 0
                    cmd.CommandText = "Delete from " & Trim(vDbName) & "SizSoft_Yarn_Delivery_Head Where Yarn_Delivery_Code = '" & Trim(NewCode) & "'"
                    Nr = cmd.ExecuteNonQuery()

                    Nr = 0
                    cmd.CommandText = "Delete from " & Trim(vDbName) & "SizSoft_Yarn_Delivery_Details Where Yarn_Delivery_Code = '" & Trim(NewCode) & "'"
                    Nr = cmd.ExecuteNonQuery()

                    Nr = 0
                    cmd.CommandText = "Delete from " & Trim(vDbName) & "Stock_Yarn_Processing_Details Where Reference_Code = '" & Trim(Pk_Condition_Tex) & Trim(NewCode) & "'"
                    Nr = cmd.ExecuteNonQuery()
                End If
                '  POSTING FOR TEXTILE ===================================================  DELETE


                cmd.CommandText = "Update SizingSoft_Yarn_Delivery_Head set User_IdNo = " & Str(Common_Procedures.User.IdNo) & ",Yarn_Delivery_Date = @DcDate, Book_No = '" & Trim(txt_BookNo.Text) & "',Textile_Dc_No = '" & Trim(txt_TexDcNo.Text) & "',  Bag_Type_Idno = " & Str(Val(Bg_Id)) & "  , Cone_Type_Idno = " & Str(Val(Con_Id)) & " ,  Ledger_IdNo = " & Str(Val(led_id)) & ", Empty_Bags = " & Str(Val(txt_EmptyBags.Text)) & ", Empty_Cones = " & Str(Val(txt_EmptyCones.Text)) & ", Beam_Width_IdNo = " & Str(Val(Bw_id)) & ", Entry_Time_Text = '" & Trim(dtp_Time.Text) & "' , Empty_Beam = " & Str(Val(txt_EmptyBeam.Text)) & ", Transport_IdNo = " & Str(Val(trans_id)) & ", Vehicle_No = '" & Trim(cbo_VehicleNo.Text) & "', Delivery_At = '" & Trim(cbo_Godown.Text) & "', Remarks = '" & Trim(txt_Remarks.Text) & "', Total_Bags = " & Str(Val(vTotYrnBags)) & ", Total_Cones = " & Str(Val(vTotYrnCones)) & ", Total_Weight = " & Str(Val(vTotYrnWeight)) & ",Electronic_Reference_No = '" & Trim(txt_ElectronicRefNo.Text) & "',Date_And_Time_Of_Supply = '" & Trim(txt_DateAndTimeOFSupply.Text) & "',DeliveryTo_IdNo = " & Val(Del_ID) & " , Approx_Value = " & Val(txt_Approx_Value.Text) & ", Vendor_Idno  = " & Val(VndrNm_Id) & ", Delivered_By = '" & Trim(cbo_Delivered.Text) & "',WareHouse_IdNo = " & Val(Gd_Id) & ",Loaded_By_Our_Employee=" & Val(Close_STS) & " , Kg_Rate = " & Val(txt_kg_Rate.Text) & " Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Delivery_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()
                cmd.CommandText = "Delete from Stock_WasteMaterial_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1163" Then

                    Da = New SqlClient.SqlDataAdapter("Select * from SizingSoft_Yarn_Delivery_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Delivery_Code = '" & Trim(NewCode) & "' and yarn_type = 'BABY' and setcode_forSelection <> ''", con)
                    Da.SelectCommand.Transaction = tr
                    Dt1 = New DataTable
                    Da.Fill(Dt1)

                    If Dt1.Rows.Count > 0 Then

                        For i = 0 To Dt1.Rows.Count - 1


                            Nr = 0
                            cmd.CommandText = "Update Stock_BabyCone_Processing_Details set Delivered_Bags = Delivered_Bags - " & Str(Val(Dt1.Rows(i).Item("Bags").ToString)) & ", Delivered_Cones = Delivered_Cones - " & Str(Val(Dt1.Rows(i).Item("cones").ToString)) & ", Delivered_Weight = Delivered_Weight - " & Str(Val(Dt1.Rows(i).Item("Weight").ToString)) & " Where setcode_forSelection = '" & Trim(Dt1.Rows(i).Item("setcode_forSelection").ToString) & "'"
                            Nr = cmd.ExecuteNonQuery()

                        Next i

                    End If

                    Dt1.Clear()

                End If

            End If

            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "NEW", "SizingSoft_Yarn_Delivery_Head", "Yarn_Delivery_Code", Val(lbl_Company.Tag), NewCode, lbl_DcNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Yarn_Delivery_Code, Company_IdNo, for_OrderBy", tr)

            '  POSTING FOR TEXTILE ===================================================  HEADER

            '  POSTING FOR TEXTILE ===================================================  STATUS
            Posting_for_Status = False
            If Common_Procedures.settings.Combine_Textile_Sizing_Software_Status = 1 Then

                vEntLedIdNo = Common_Procedures.get_FieldValue(con, "SizingSoft_Yarn_Delivery_Head", "ledger_idno", "(Yarn_Delivery_Code = '" & Trim(NewCode) & "')", , tr)
                TexComp_ID = Common_Procedures.get_FieldValue(con, "ledger_head", "Textile_To_CompanyIdNo", "(ledger_idno = " & Str(Val(vEntLedIdNo)) & ")", , tr)

                If Val(TexComp_ID) <> 0 Then


                    If VndrNm_Id = 0 Then
                        cbo_VendorName.Focus()
                        Throw New ApplicationException("Invalid Vendor Name.")
                    End If




                    TexLed_ID = Common_Procedures.get_FieldValue(con, "company_head", "Textile_To_SizingIdNo", "(company_idno = " & Str(Val(lbl_Company.Tag)) & ")", , tr)
                    TexVnd_iD = Val(Common_Procedures.get_FieldValue(con, "Vendor_head", "Textile_To_WeaverIdNo", "(Vendor_idno = " & Str(Val(VndrNm_Id)) & ")", , tr))
                    If TexVnd_iD <> 0 And TexLed_ID <> 0 Then
                        Posting_for_Status = True
                    End If
                End If
            End If

            '  POSTING FOR TEXTILE ===================================================  STATUS

            If Posting_for_Status Then

                cmd.CommandText = "Insert into " & Trim(vDbName) & "SizSoft_Yarn_Delivery_Head (     User_IdNo       ,    Yarn_Delivery_Code  ,             Company_IdNo   ,        Yarn_Delivery_No      ,                                         for_OrderBy,            Yarn_Delivery_Date,           Book_No,                Textile_Dc_No ,                 Ledger_IdNo,                         Empty_Bags,                            Empty_Cones,               Beam_Width_IdNo,                   Empty_Beam,                       Transport_IdNo,                  Vehicle_No,                      Delivery_At,                         Remarks,                        Total_Bags,                   Total_Cones,                     Total_Weight ,                 Bag_Type_Idno ,          Cone_Type_Idno ,              Electronic_Reference_No   ,               Date_And_Time_Of_Supply       ,        DeliveryTO_Idno,         Approx_Value ,                   Entry_Time_Text,             Vendor_Idno,                 Delivered_By,              WareHouse_IdNo    ,  Loaded_by_Our_employee) " &
                                    "                               Values                     (" & Str(Common_Procedures.User.IdNo) & ", '" & Trim(NewCode) & "', " & Str(Val(TexComp_ID)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(vOrdByNo)) & ",    @DcDate        , '" & Trim(txt_BookNo.Text) & "','" & Trim(txt_TexDcNo.Text) & "', " & Str(Val(TexLed_ID)) & ", " & Str(Val(txt_EmptyBags.Text)) & ", " & Str(Val(txt_EmptyCones.Text)) & ", " & Str(Val(Bw_id)) & ", " & Str(Val(txt_EmptyBeam.Text)) & ", " & Str(Val(trans_id)) & ", '" & Trim(cbo_VehicleNo.Text) & "', '" & Trim(txt_DeliveryAt.Text) & "', '" & Trim(txt_Remarks.Text) & "', " & Str(Val(vTotYrnBags)) & ", " & Str(Val(vTotYrnCones)) & ", " & Str(Val(vTotYrnWeight)) & " , " & Str(Val(Bg_Id)) & "  ," & Str(Val(Con_Id)) & ",'" & Trim(txt_ElectronicRefNo.Text) & "','" & Trim(txt_DateAndTimeOFSupply.Text) & "'," & Val(Del_ID) & "," & Val(txt_Approx_Value.Text) & ",'" & Trim(dtp_Time.Text) & "'," & Val(TexVnd_iD) & ",'" & Trim(cbo_Delivered.Text) & "', " & Val(Gd_Id) & ",  " & Val(Close_STS) & "  )"
                cmd.ExecuteNonQuery()

            End If
            '  POSTING FOR TEXTILE ===================================================  HEADER



            If Val(Common_Procedures.settings.StatementPrint_BookNo_IN_Stock_Particulars_Status) = 1 Then
                Partcls = "Delv : Dc.No. " & Trim(lbl_DcNo.Text)
                PBlNo = Trim(txt_BookNo.Text)
            Else
                Partcls = "Delv : Dc.No. " & Trim(lbl_DcNo.Text)
                PBlNo = Trim(lbl_DcNo.Text)
            End If

            If Val(Bg_Id) <> 0 Then

                Da = New SqlClient.SqlDataAdapter("select a.* from Waste_Head a Where a.Bag_Type_Idno = " & Str(Val(Bg_Id)), con)
                Da.SelectCommand.Transaction = tr
                Dt = New DataTable
                Da.Fill(Dt)

                WstBg_Id = 0
                If Dt.Rows.Count > 0 Then
                    If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                        WstBg_Id = Val(Dt.Rows(0).Item("Packing_Idno").ToString)
                    End If
                End If

                cmd.CommandText = "Insert into Stock_WasteMaterial_Processing_Details ( Reference_Code,             Company_IdNo         ,           Reference_No        ,                               For_OrderBy                              , Reference_Date,        Ledger_IdNo      ,      Party_Bill_No   ,           Sl_No      ,          Waste_IdNo  ,      Quantity     ,     Rate ,  Amount ) " &
                                                "   Values  ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(vOrdByNo)) & ",    @DcDate   , " & Str(Val(led_id)) & ", '" & Trim(PBlNo) & "', 1, " & Str(Val(WstBg_Id)) & ", " & Str(-1 * Val(vTotYrnBags)) & ",  0   , 0 )"
                cmd.ExecuteNonQuery()

            End If

            If Val(Con_Id) <> 0 Then

                Da = New SqlClient.SqlDataAdapter("select a.* from Waste_Head a Where a.Cone_Type_Idno = " & Str(Val(Con_Id)), con)
                Da.SelectCommand.Transaction = tr
                Dt = New DataTable
                Da.Fill(Dt)

                WstCn_Id = 0
                If Dt.Rows.Count > 0 Then
                    If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                        WstCn_Id = Val(Dt.Rows(0).Item("Packing_Idno").ToString)
                    End If
                End If

                Dt.Dispose()
                Da.Dispose()

                cmd.CommandText = "Insert into Stock_WasteMaterial_Processing_Details ( Reference_Code,             Company_IdNo         ,           Reference_No        ,                               For_OrderBy                              , Reference_Date,        Ledger_IdNo      ,      Party_Bill_No   ,           Sl_No      ,          Waste_IdNo  ,      Quantity     ,     Rate ,  Amount ) " &
                                                "   Values  ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(vOrdByNo)) & ",    @DcDate   , " & Str(Val(led_id)) & ", '" & Trim(PBlNo) & "', 2 , " & Str(Val(WstCn_Id)) & ", " & Str(-1 * Val(vTotYrnCones)) & ",      0   , 0       )"
                cmd.ExecuteNonQuery()

            End If

            cmd.CommandText = "Delete from SizingSoft_Yarn_Delivery_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Delivery_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            ''---Only for save all status
            ''cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = 'YNDLV-" & Trim(NewCode) & "'"
            ''cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Empty_BeamBagCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Yarn_Delivery_Selections_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            Sno = 0
            For i = 0 To dgv_Details.RowCount - 1

                If Val(dgv_Details.Rows(i).Cells(7).Value) <> 0 Then

                    Cnt_ID = Common_Procedures.Count_NameToIdNo(con, Trim(dgv_Details.Rows(i).Cells(1).Value), tr)
                    Mil_ID = Common_Procedures.Mill_NameToIdNo(con, Trim(dgv_Details.Rows(i).Cells(4).Value), tr)
                    Bag_ID = Common_Procedures.BagType_NameToIdNo(con, Trim(dgv_Details.Rows(i).Cells(11).Value), tr)
                    LocDet_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, Trim(dgv_Details.Rows(i).Cells(15).Value), tr)


                    vStCdSel = ""
                    If Trim(UCase(dgv_Details.Rows(i).Cells(2).Value)) = "BABY" Then
                        vStCdSel = Trim(dgv_Details.Rows(i).Cells(3).Value)
                    End If

                    Sno = Sno + 1

                    cmd.CommandText = "Insert into SizingSoft_Yarn_Delivery_Details ( Yarn_Delivery_Code, Company_IdNo, Yarn_Delivery_No, for_OrderBy, Yarn_Delivery_Date, Ledger_IdNo, Sl_No, Count_IdNo, Yarn_Type, SetCode_ForSelection, Mill_IdNo, Bags, Cones, Weight, Weaver_Yarn_Requirement_No, Weaver_Yarn_Requirement_Code, Weaver_Yarn_Requirement_Details_Slno,rate,amount,bagtype_idno,GrossWeight,Location_IdNo,Lot_No ) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(vOrdByNo)) & ", @DcDate, " & Str(Val(led_id)) & ", " & Str(Val(Sno)) & ", " & Str(Val(Cnt_ID)) & ", '" & Trim(dgv_Details.Rows(i).Cells(2).Value) & "', '" & Trim(vStCdSel) & "', " & Str(Val(Mil_ID)) & ", " & Str(Val(dgv_Details.Rows(i).Cells(5).Value)) & ", " & Str(Val(dgv_Details.Rows(i).Cells(6).Value)) & ", " & Str(Val(dgv_Details.Rows(i).Cells(7).Value)) & ", '" & Trim(dgv_Details.Rows(i).Cells(8).Value) & "', '" & Trim(dgv_Details.Rows(i).Cells(9).Value) & "', " & Str(Val(dgv_Details.Rows(i).Cells(10).Value)) & "," & Val(dgv_Details.Rows(i).Cells(12).Value) & "," & Val(dgv_Details.Rows(i).Cells(13).Value) & "," & Val(Bag_ID) & "," & Val(dgv_Details.Rows(i).Cells(14).Value) & "," & Str(Val(LocDet_ID)) & ",'" & Trim(dgv_Details.Rows(i).Cells(16).Value) & "' )"
                    cmd.ExecuteNonQuery()




                    'vSetCd = ""
                    'vSetNo = ""
                    'If Common_Procedures.settings.Textile_Sizing_Delivery_receipt_Selection = 1 And vCOMP_LEDIDNO <> 0 Then
                    '    cmd.CommandText = "Insert into Yarn_Delivery_Selections_Processing_Details ( Reference_Code                 , Company_IdNo                       , Reference_No                      , for_OrderBy                                                            , Reference_Date    ,    Delivery_Code                              ,     Delivery_No                  , DeliveryTo_Idno            , ReceivedFrom_Idno             ,     Party_Dc_No                                 , Total_Bags                          , total_cones                                  , Total_Weight                                     ,    Selection_Ledgeridno       ,          Selection_CompanyIdno      ) " &
                    '                                                             " Values          ('" & Trim(Pk_Condition) & Trim(NewCode) & "'          , " & Str(Val(lbl_Company.Tag)) & "  , '" & Trim(lbl_DcNo.Text) & "'      , " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text))) & ", @DcDate        ,'" & Trim(Pk_Condition) & Trim(NewCode) & "'   , '" & Trim(lbl_DcNo.Text) & "'    ," & Str(Val(vLedID)) & "    , " & Str(Val(vLedID)) & "  , '" & Trim(txt_TexDcNo.Text) & "'      , " & Str(Val(vTotYrnBags)) & "        , " & Str(Val(vTotYrnCones)) & "                , " & Str(Val(vTotYrnWeight)) & "         ," & Str(Val(vCOMP_LEDIDNO)) & ", " & Str(Val(vDELVLED_COMPIDNO)) & " ) "
                    '    cmd.ExecuteNonQuery()
                    'End If
                    If Trim(UCase(dgv_Details.Rows(i).Cells(2).Value)) = "BABY" And Trim(Trim(dgv_Details.Rows(i).Cells(3).Value)) <> "" Then

                        Da = New SqlClient.SqlDataAdapter("select a.set_code, a.set_no from Specification_Head a where a.setcode_forSelection = '" & Trim(Trim(dgv_Details.Rows(i).Cells(3).Value)) & "'", con)
                        Da.SelectCommand.Transaction = tr
                        Dt1 = New DataTable
                        Da.Fill(Dt1)
                        If Dt1.Rows.Count > 0 Then
                            vSetCd = Dt1.Rows(0).Item("set_code").ToString
                            vSetNo = Dt1.Rows(0).Item("set_no").ToString
                        End If
                        Dt1.Clear()

                        If Trim(vSetCd) = "" Then
                            Da = New SqlClient.SqlDataAdapter("select a.set_code, a.set_no from Stock_BabyCone_Processing_Details a where a.setcode_forSelection = '" & Trim(Trim(dgv_Details.Rows(i).Cells(3).Value)) & "'", con)
                            Da.SelectCommand.Transaction = tr
                            Dt1 = New DataTable
                            Da.Fill(Dt1)
                            If Dt1.Rows.Count > 0 Then
                                vSetCd = Dt1.Rows(0).Item("set_code").ToString
                                vSetNo = Dt1.Rows(0).Item("set_no").ToString
                            End If
                            Dt1.Clear()
                        End If

                        Nr = 0
                        cmd.CommandText = "Update Stock_BabyCone_Processing_Details set Delivered_Bags = Delivered_Bags + " & Str(Val(dgv_Details.Rows(i).Cells(5).Value)) & ", Delivered_Cones = Delivered_Cones + " & Str(Val(dgv_Details.Rows(i).Cells(6).Value)) & ", Delivered_Weight = Delivered_Weight + " & Str(Val(dgv_Details.Rows(i).Cells(7).Value)) & " Where setcode_forSelection = '" & Trim(dgv_Details.Rows(i).Cells(3).Value) & "' and Ledger_IdNo = " & Str(Val(led_id)) & " and Count_IdNo = " & Str(Val(Cnt_ID)) & " and Mill_IdNo = " & Str(Val(Mil_ID))
                        Nr = cmd.ExecuteNonQuery()

                        If Nr = 0 Then
                            MessageBox.Show("Invalid Baby cone Details - Mismatch of details", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                            tr.Rollback()
                            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
                            Exit Sub
                        End If

                    End If

                    vYrnPartcls = Partcls
                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1263" Then '---- Meenashi Sizing (Somanur)
                        vYrnPartcls = vYrnPartcls & ",  Mill :  " & Trim(dgv_Details.Rows(i).Cells(4).Value)
                    End If

                    cmd.CommandText = "Insert into Stock_Yarn_Processing_Details(  SoftwareType_IdNo  ,                                                Reference_Code            ,              Company_IdNo         ,          Reference_No         ,                                  for_OrderBy                           , Reference_Date,       DeliveryTo_Idno    , ReceivedFrom_Idno,     Party_Bill_No     ,          Sl_No        ,          Count_IdNo      ,                            Yarn_Type               ,             Mill_IdNo    ,                        Bags                          ,                         Cones                        ,                              Weight                 ,               Particulars   , Posting_For,          Set_Code      ,            Set_No      ,     WareHouse_IdNo    ,Lot_No ) " &
                                                        "Values  (" & Str(Val(Common_Procedures.SoftwareTypes.Sizing_Software)) & " , '" & Trim(Pk_Condition) & Trim(NewCode) & "' , " & Str(Val(lbl_Company.Tag)) & " , '" & Trim(lbl_DcNo.Text) & "' , " & Str(Val(vOrdByNo)) & " ,     @DcDate   , " & Str(Val(led_id)) & " ,          0       , '" & Trim(PBlNo) & "' , " & Str(Val(Sno)) & " , " & Str(Val(Cnt_ID)) & " , '" & Trim(dgv_Details.Rows(i).Cells(2).Value) & "' , " & Str(Val(Mil_ID)) & " , " & Str(Val(dgv_Details.Rows(i).Cells(5).Value)) & " , " & Str(Val(dgv_Details.Rows(i).Cells(6).Value)) & " , " & Str(Val(dgv_Details.Rows(i).Cells(7).Value)) & ", '" & Trim(vYrnPartcls) & "' ,  'DELIVERY', '" & Trim(vSetCd) & "' , '" & Trim(vSetNo) & "' , " & Str(Val(LocDet_ID)) & ",'" & Trim(dgv_Details.Rows(i).Cells(16).Value) & "')"
                    cmd.ExecuteNonQuery()

                    'If Common_Procedures.settings.CustomerCode <> "1006" And Common_Procedures.settings.CustomerCode <> "1036" And Common_Procedures.settings.CustomerCode <> "1220" Then
                    '    'vLedID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
                    '    'vCntID = Common_Procedures.Count_NameToIdNo(con, dgv_Details.Rows(i).Cells(1).Value)
                    '    CurStk = Common_Procedures.get_Yarn_CurrentStock(con, Val(lbl_Company.Tag), led_id, Cnt_ID, tr)
                    '    If Val(CurStk) <= 0 Then
                    '        'MessageBox.Show("Invalid Stock : Current Stock is " & Trim(Format(Val(CurStk), "#########0.000")), "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    '        Throw New ApplicationException("Invalid Stock : Current Stock is " & Trim(Format(Val(CurStk), "#########0.000")) & " DOES NOT SAVE...")
                    '        Exit Sub
                    '    End If
                    'End If

                    If Common_Procedures.settings.CustomerCode <> "1006" And Common_Procedures.settings.CustomerCode <> "1036" And Common_Procedures.settings.CustomerCode <> "1078" And Common_Procedures.settings.CustomerCode <> "1087" And Common_Procedures.settings.CustomerCode <> "1220" And Common_Procedures.settings.CustomerCode <> "1351" And Common_Procedures.settings.CustomerCode <> "1042" And Common_Procedures.settings.CustomerCode <> "1015" And Common_Procedures.settings.CustomerCode <> "1346" Then

                        'vLedID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
                        'vCntID = Common_Procedures.Count_NameToIdNo(con, dgv_Details.Rows(i).Cells(1).Value)

                        'If Common_Procedures.settings.CustomerCode = "1282" Then
                        '    CurStk = Common_Procedures.get_Yarn_CurrentStock(con, Val(lbl_Company.Tag), led_id, Cnt_ID, Mil_ID, tr)
                        'Else
                        CurStk = Common_Procedures.get_Yarn_CurrentStock(con, Val(lbl_Company.Tag), led_id, Cnt_ID, 0, tr)
                        'End If

                        If Val(CurStk) < 0 Then
                            Throw New ApplicationException("Invalid Stock : Current Stock is " & Trim(Format(Val(CurStk), "#########0.000")))
                            Exit Sub
                        End If

                    End If


                    '  POSTING FOR TEXTILE ===================================================  DETAIL
                    If Posting_for_Status Then

                        TexCnt_iD = Common_Procedures.get_FieldValue(con, "count_head", "Textile_To_CountIdNo", "(count_idno = " & Str(Val(Cnt_ID)) & ")", , tr)
                        If Val(TexCnt_iD) = 0 Then
                            vNewFrmTYpe = "COUNT"
                            Throw New ApplicationException("Invalid Textile Count Name" & Chr(13) & "Select ``Textile_Count_Name``  in  Count_Creation  for  " & dgv_Details.Rows(i).Cells(1).Value)
                            Exit Sub
                        End If

                        TexMil_iD = Common_Procedures.get_FieldValue(con, "Mill_head", "Textile_To_MillIdNo", "(Mill_idno = " & Str(Val(Mil_ID)) & ")", , tr)
                        If Val(TexMil_iD) = 0 Then
                            vNewFrmTYpe = "MILL"
                            Throw New ApplicationException("Invalid Textile Mill Name" & Chr(13) & "Select ``Textile_Mill_Name``  in  Mill_Creation  for  " & dgv_Details.Rows(i).Cells(4).Value)
                            Exit Sub
                        End If

                        cmd.CommandText = "Insert into " & Trim(vDbName) & "SizSoft_Yarn_Delivery_Details ( Yarn_Delivery_Code, Company_IdNo, Yarn_Delivery_No, for_OrderBy, Yarn_Delivery_Date, Ledger_IdNo, Sl_No, Count_IdNo, Yarn_Type, SetCode_ForSelection, Mill_IdNo, Bags, Cones, Weight, Weaver_Yarn_Requirement_No, Weaver_Yarn_Requirement_Code, Weaver_Yarn_Requirement_Details_Slno ) Values ('" & Trim(NewCode) & "', " & Str(Val(TexComp_ID)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(vOrdByNo)) & ", @DcDate, " & Str(Val(TexLed_ID)) & ", " & Str(Val(Sno)) & ", " & Str(Val(TexCnt_iD)) & ", '" & Trim(dgv_Details.Rows(i).Cells(2).Value) & "', '" & Trim(vStCdSel) & "', " & Str(Val(TexMil_iD)) & ", " & Str(Val(dgv_Details.Rows(i).Cells(5).Value)) & ", " & Str(Val(dgv_Details.Rows(i).Cells(6).Value)) & ", " & Str(Val(dgv_Details.Rows(i).Cells(7).Value)) & ",'" & Trim(dgv_Details.Rows(i).Cells(8).Value) & "','" & Trim(dgv_Details.Rows(i).Cells(9).Value) & "'," & Str(Val(dgv_Details.Rows(i).Cells(10).Value)) & " )"
                        cmd.ExecuteNonQuery()

                        cmd.CommandText = "Insert into  " & Trim(vDbName) & "Stock_Yarn_Processing_Details (       SoftwareType_IdNo  ,                            Reference_Code                    ,               Company_IdNo     ,             Reference_No     ,                               for_OrderBy                             , Reference_Date,      DeliveryTo_Idno       ,         ReceivedFrom_Idno    ,                              Entry_ID                       ,        Particulars     ,     Party_Bill_No    ,            Sl_No      ,          Count_IdNo       ,                               Yarn_Type           ,           Mill_IdNo        ,                                 Bags                ,                                 Cones               ,                                 Weight              , DeliveryToIdno_ForParticulars,  ReceivedFromIdno_ForParticulars  ) " &
                                                                              "Values (" & Str(Val(Common_Procedures.SoftwareTypes.Sizing_Software)) & " ,  '" & Trim(Pk_Condition_Tex) & Trim(NewCode) & "', " & Str(Val(TexComp_ID)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(vOrdByNo)) & ",    @DcDate    , " & Str(Val(TexVnd_iD)) & ",  " & Str(Val(TexLed_ID)) & " , '" & Trim(Trim(Pk_Condition_Tex) & Trim(lbl_DcNo.Text)) & "', '" & Trim(Partcls) & "', '" & Trim(PBlNo) & "', " & Str(Val(Sno)) & ", " & Str(Val(TexCnt_iD)) & ", '" & Trim(dgv_Details.Rows(i).Cells(2).Value) & "', " & Str(Val(TexMil_iD)) & ", " & Str(Val(dgv_Details.Rows(i).Cells(5).Value)) & ", " & Str(Val(dgv_Details.Rows(i).Cells(6).Value)) & ", " & Str(Val(dgv_Details.Rows(i).Cells(7).Value)) & ", " & Str(Val(TexVnd_iD)) & "  ,       " & Str(Val(TexLed_ID)) & " ) "
                        cmd.ExecuteNonQuery()

                    End If
                    '  POSTING FOR TEXTILE ===================================================  DETAIL

                End If

            Next

            Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "NEW", "SizingSoft_Yarn_Delivery_Details", "Yarn_Delivery_Code", Val(lbl_Company.Tag), NewCode, lbl_DcNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "Count_IdNo, Yarn_Type, SetCode_ForSelection, Mill_IdNo, Bags, Cones, Weight", "Sl_No", "Yarn_Delivery_Code, For_OrderBy, Company_IdNo, Yarn_Delivery_No, Yarn_Delivery_Date, Ledger_Idno", tr)

            If Val(txt_EmptyBeam.Text) <> 0 Or Val(txt_EmptyBags.Text) <> 0 Or Val(txt_EmptyCones.Text) <> 0 Or Val(vTotYrnBags) <> 0 Or Val(vTotYrnCones) <> 0 Then
                cmd.CommandText = "Insert into Stock_Empty_BeamBagCone_Processing_Details(SoftwareType_IdNo, Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Party_Bill_No, Sl_No, Beam_Width_IdNo, Empty_Beam, Empty_Bags, Empty_Cones, Particulars) Values (" & Str(Val(Common_Procedures.SoftwareTypes.Sizing_Software)) & " , '" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(vOrdByNo)) & ", @DcDate, " & Str(Val(led_id)) & ", 0, '" & Trim(PBlNo) & "', 1, " & Str(Val(Bw_id)) & ", " & Str(Val(txt_EmptyBeam.Text)) & ", " & Str(Val(txt_EmptyBags.Text) + Val(vTotYrnBags)) & ", " & Str(Val(txt_EmptyCones.Text) + Val(vTotYrnCones)) & ", '" & Trim(Partcls) & "' )"
                cmd.ExecuteNonQuery()
            End If

            vDELVLED_COMPIDNO = 0
            If Common_Procedures.settings.Textile_Sizing_Delivery_receipt_Selection = 1 Then
                vDELVLED_COMPIDNO = Common_Procedures.Ledger_IdNoToCompanyIdNo(con, Str(Val(led_id)), tr)
            End If

            If Common_Procedures.settings.Textile_Sizing_Delivery_receipt_Selection = 1 And vDELVLED_COMPIDNO <> 0 Then

                vCOMP_LEDIDNO = Common_Procedures.Company_IdnoToSizingLedgerIdNo(con, Str(Val(lbl_Company.Tag)), tr)

                vSELC_RCVDIDNO = 0
                'vREC_Ledtype = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_Type", "(Ledger_IdNo = " & Str(Val(Rec_ID)) & ")", , tr)
                'If Trim(UCase(vREC_Ledtype)) = "GODOWN" Or Trim(UCase(vREC_Ledtype)) = "WEAVER" Then
                '    vSELC_RCVDIDNO = Rec_ID
                'Else
                vSELC_RCVDIDNO = vCOMP_LEDIDNO
                'End If

                vDELVAT_IDNO = 0
                vDELV_Ledtype = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_Type", "(Ledger_IdNo = " & Str(Val(VndrNm_Id)) & ")", , tr)
                If Trim(UCase(vDELV_Ledtype)) = "GODOWN" Or Trim(UCase(vDELV_Ledtype)) = "WEAVER" Or Trim(UCase(vDELV_Ledtype)) <> "" Then
                    vDELVAT_IDNO = led_id
                Else
                    vDELVAT_IDNO = 0
                End If

                cmd.CommandText = "Insert into Yarn_Delivery_Selections_Processing_Details ( Reference_Code                             , Company_IdNo                       , Reference_No                      , for_OrderBy                , Reference_Date ,    Delivery_Code                            ,     Delivery_No                  , DeliveryTo_Idno        ,         ReceivedFrom_Idno       ,         DeliveryAt_Idno       ,     Party_Dc_No                 , Total_Bags                   , total_cones                   , Total_Weight                   ,          Selection_CompanyIdno      ,         Selection_Ledgeridno   ,      Selection_ReceivedFromIdNo  ) " &
                                                                         " Values          ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & "  , '" & Trim(lbl_DcNo.Text) & "'      , " & Str(Val(vOrdByNo)) & ", @DcDate        , '" & Trim(Pk_Condition) & Trim(NewCode) & "', '" & Trim(lbl_DcNo.Text) & "'    ," & Str(Val(vLedID)) & ", " & Str(Val(vSELC_RCVDIDNO)) & ", " & Str(Val(vDELVAT_IDNO)) & ", '" & Trim(txt_TexDcNo.Text) & "', " & Str(Val(vTotYrnBags)) & ", " & Str(Val(vTotYrnCones)) & ", " & Str(Val(vTotYrnWeight)) & ", " & Str(Val(vDELVLED_COMPIDNO)) & ", " & Str(Val(vCOMP_LEDIDNO)) & ", " & Str(Val(vSELC_RCVDIDNO)) & " ) "
                cmd.ExecuteNonQuery()

                'cmd.CommandText = "Insert into Yarn_Delivery_Selections_Processing_Details (                  Reference_Code            ,                 Company_IdNo     ,            Reference_No      ,         for_OrderBy       , Reference_Date,                  Delivery_Code              ,           Delivery_No        ,       DeliveryTo_Idno    ,     ReceivedFrom_Idno   ,         DeliveryAt_Idno       ,               Party_Dc_No          ,              Total_Bags      ,          total_cones          ,              Total_Weight      ,          Selection_CompanyIdno      ,         Selection_Ledgeridno  ,      Selection_ReceivedFromIdNo  ) " &
                '                    "           Values                                     ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(vOrdByNo)) & ", @DcDate       , '" & Trim(Pk_Condition) & Trim(NewCode) & "', '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(Delv_ID)) & ", " & Str(Val(Rec_ID)) & ", " & Str(Val(vDELVAT_IDNO)) & ", '" & Trim(txt_Party_DcNo.Text) & "', " & Str(Val(vTotYrnBags)) & ", " & Str(Val(vTotYrnCones)) & ", " & Str(Val(vTotYrnWeight)) & ", " & Str(Val(vDELVLED_COMPIDNO)) & ", " & Str(Val(vCOMP_LEDIDNO)) & ", " & Str(Val(vSELC_RCVDIDNO)) & " ) "
                'cmd.ExecuteNonQuery()

            End If

            'vSetCd = ""
            'vSetNo = ""
            'If Common_Procedures.settings.Textile_Sizing_Delivery_receipt_Selection = 1 And vCOMP_LEDIDNO <> 0 Then
            '    cmd.CommandText = "Insert into Yarn_Delivery_Selections_Processing_Details ( Reference_Code                 , Company_IdNo                       , Reference_No                      , for_OrderBy                                                            , Reference_Date    ,    Delivery_Code                              ,     Delivery_No                  , DeliveryTo_Idno            , ReceivedFrom_Idno             ,     Party_Dc_No                                 , Total_Bags                          , total_cones                                  , Total_Weight                                     ,    Selection_Ledgeridno       ,          Selection_CompanyIdno      ) " &
            '                                                             " Values          ('" & Trim(Pk_Condition) & Trim(NewCode) & "'          , " & Str(Val(lbl_Company.Tag)) & "  , '" & Trim(lbl_DcNo.Text) & "'      , " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text))) & ", @DcDate        ,'" & Trim(Pk_Condition) & Trim(NewCode) & "'   , '" & Trim(lbl_DcNo.Text) & "'    ," & Str(Val(vLedID)) & "    , " & Str(Val(vLedID)) & "  , '" & Trim(txt_TexDcNo.Text) & "'      , " & Str(Val(vTotYrnBags)) & "        , " & Str(Val(vTotYrnCones)) & "                , " & Str(Val(vTotYrnWeight)) & "         ," & Str(Val(vCOMP_LEDIDNO)) & ", " & Str(Val(vDELVLED_COMPIDNO)) & " ) "
            '    cmd.ExecuteNonQuery()
            'End If

            If Val(Common_Procedures.User.IdNo) = 1 Then
                If chk_Printed.Visible = True Then
                    If chk_Printed.Enabled = True Then
                        Update_PrintOut_Status(tr)
                    End If
                End If
            End If

            tr.Commit()



            If SaveAll_STS <> True Then
                MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1282" Then '---- BRT SIZING (SOMANUR)
                If New_Entry = True Then
                    If TexComp_ID <> 0 Then
                        Send_SMS()
                    End If
                End If
            End If

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

            Timer1.Enabled = False
            SaveAll_STS = False

            If InStr(1, Trim(LCase(ex.Message)), Trim(LCase("ck_stock_babycone_processing_details"))) > 0 Then
                MessageBox.Show("Invalid Baby cone Details - Delivery Qty greater than production Qty", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            Else
                MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If

        Finally
            Dt.Dispose()
            Dt1.Dispose()
            Da.Dispose()

            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()



            If Trim(UCase(vNewFrmTYpe)) = "COUNT" Then
                Dim f1 As New Count_Creation '(Cnt_ID)

                Common_Procedures.Master_Return.Form_Name = ""
                Common_Procedures.Master_Return.Control_Name = ""
                Common_Procedures.Master_Return.Return_Value = ""
                Common_Procedures.Master_Return.Master_Type = ""

                f1.MdiParent = MDIParent1
                f1.Show()

            ElseIf Trim(UCase(vNewFrmTYpe)) = "MILL" Then
                Dim f2 As New Mill_Creation '(Mil_ID)

                Common_Procedures.Master_Return.Form_Name = ""
                Common_Procedures.Master_Return.Control_Name = ""
                Common_Procedures.Master_Return.Return_Value = ""
                Common_Procedures.Master_Return.Master_Type = ""

                f2.MdiParent = MDIParent1
                f2.Show()

            ElseIf Trim(UCase(vNewFrmTYpe)) = "VENDOR" Then
                Dim f2 As New Vendor_Creation '(VndrNm_Id)

                Common_Procedures.Master_Return.Form_Name = ""
                Common_Procedures.Master_Return.Control_Name = ""
                Common_Procedures.Master_Return.Return_Value = ""
                Common_Procedures.Master_Return.Master_Type = ""

                f2.MdiParent = MDIParent1
                f2.Show()

            End If

        End Try

    End Sub

    Private Sub txt_Bags_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Bags.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

    End Sub

    Private Sub txt_Cones_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Cones.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

    End Sub


    Private Sub txt_EmptyBeam_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_EmptyBeam.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

    End Sub

    Private Sub txt_SlNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_SlNo.GotFocus
        '---Show_Yarn_CurrentStock()
    End Sub

    Private Sub txt_SlNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_SlNo.KeyPress
        Dim i As Integer

        If Asc(e.KeyChar) = 13 Then

            If Val(txt_SlNo.Text) = 0 Then
                txt_EmptyBeam.Focus()

            Else

                With dgv_Details

                    For i = 0 To .Rows.Count - 1
                        If Val(dgv_Details.Rows(i).Cells(0).Value) = Val(txt_SlNo.Text) Then

                            cbo_CountName.Text = .Rows(i).Cells(1).Value
                            cbo_YarnType.Text = .Rows(i).Cells(2).Value
                            cbo_SetNo.Text = .Rows(i).Cells(3).Value
                            cbo_MillName.Text = .Rows(i).Cells(4).Value
                            txt_Bags.Text = Val(.Rows(i).Cells(5).Value)
                            txt_Cones.Text = Val(.Rows(i).Cells(6).Value)
                            txt_Weight.Text = Format(Val(.Rows(i).Cells(7).Value), "########0.000")

                            Exit For

                        End If

                    Next

                End With

                SendKeys.Send("{TAB}")

            End If

        End If
    End Sub

    Private Sub cbo_YarnType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_YarnType.GotFocus
        With cbo_YarnType

            If Trim(cbo_YarnType.Text) = "" Then cbo_YarnType.Text = "MILL"
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "YarnType_Head", "Yarn_Type", "", "(Yarn_type <> '')")

        End With
        cbo_YarnType.Tag = cbo_YarnType.Text
        '---Show_Yarn_CurrentStock()
    End Sub

    Private Sub cbo_YarnType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_YarnType.KeyDown
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_YarnType, cbo_CountName, Nothing, "YarnType_Head", "Yarn_type", "", "(Yarn_type <> '')")
            With cbo_YarnType
                If e.KeyValue = 40 And .DroppedDown = False Then
                    e.Handled = True
                    If Trim(UCase(cbo_YarnType.Text)) = "BABY" Then
                        cbo_SetNo.Focus()

                    Else
                        cbo_MillName.Focus()

                    End If
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_YarnType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_YarnType.KeyPress

        Try

            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_YarnType, cbo_MillName, "YarnType_Head", "Yarn_Type", "", "(Yarn_type <> '')")

            If Asc(e.KeyChar) = 13 Then
                If Trim(UCase(cbo_YarnType.Text)) = "BABY" Then
                    cbo_SetNo.Focus()
                Else
                    cbo_MillName.Focus()
                End If
            End If


        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

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
        Dim Led_IdNo As Integer, Cnt_IdNo As Integer, Mil_IdNo As Integer
        Dim Condt As String = ""

        Try

            Condt = ""
            Led_IdNo = 0
            Cnt_IdNo = 0
            Mil_IdNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Yarn_Delivery_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Yarn_Delivery_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Yarn_Delivery_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If

            If Trim(cbo_Filter_CountName.Text) <> "" Then
                Cnt_IdNo = Common_Procedures.Count_NameToIdNo(con, cbo_Filter_CountName.Text)
            End If

            If Trim(cbo_Filter_MillName.Text) <> "" Then
                Mil_IdNo = Common_Procedures.Mill_NameToIdNo(con, cbo_Filter_MillName.Text)
            End If

            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.Ledger_IdNo = " & Str(Val(Led_IdNo))
            End If

            If Val(Cnt_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Yarn_Delivery_Code IN (select z1.Yarn_Delivery_Code from SizingSoft_Yarn_Delivery_Details z1 where z1.Count_IdNo = " & Str(Val(Cnt_IdNo)) & ") "
            End If

            If Val(Mil_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Yarn_Delivery_Code IN (select z2.Yarn_Delivery_Code from SizingSoft_Yarn_Delivery_Details z2 where z2.Mill_IdNo = " & Str(Val(Mil_IdNo)) & ") "
            End If

            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name from SizingSoft_Yarn_Delivery_Head a INNER JOIN Ledger_Head b on a.Ledger_IdNo = b.Ledger_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Yarn_Delivery_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Yarn_Delivery_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Yarn_Delivery_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Yarn_Delivery_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = Val(dt2.Rows(i).Item("Total_Bags").ToString)
                    dgv_Filter_Details.Rows(n).Cells(5).Value = Val(dt2.Rows(i).Item("Total_Cones").ToString)
                    dgv_Filter_Details.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Total_Weight").ToString), "########0.000")

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

    Private Sub Open_FilterEntry()
        Dim movno As String

        On Error Resume Next

        If IsNothing(dgv_Filter_Details.CurrentCell) Then Exit Sub

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

    Private Sub cbo_BeamWidth_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_BeamWidth.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Beam_Width_Head", "Beam_Width_Name", "", "(Beam_Width_IdNo = 0)")
    End Sub




    Private Sub cbo_BeamWidth_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_BeamWidth.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Beam_Width_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_BeamWidth.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_Transport_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Transport.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Transport_Head", "Transport_Name", "", "(Transport_IdNo = 0)")
    End Sub

    Private Sub cbo_Transport_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Transport.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Transport_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Transport.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub




    Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged
        On Error Resume Next
        If IsNothing(dgv_Details.CurrentCell) Then Exit Sub

        With dgv_Details
            If .Visible Then
                If .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 6 Or .CurrentCell.ColumnIndex = 7 Then
                    Total_Calculation()
                End If
            End If
        End With
    End Sub



    Private Sub dgv_Details_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.DoubleClick

        If Trim(dgv_Details.CurrentRow.Cells(1).Value) <> "" Then

            txt_SlNo.Text = Val(dgv_Details.CurrentRow.Cells(0).Value)
            cbo_CountName.Text = Trim(dgv_Details.CurrentRow.Cells(1).Value)
            cbo_YarnType.Text = Trim(dgv_Details.CurrentRow.Cells(2).Value)
            cbo_SetNo.Text = dgv_Details.CurrentRow.Cells(3).Value
            cbo_MillName.Text = dgv_Details.CurrentRow.Cells(4).Value
            txt_Bags.Text = Val(dgv_Details.CurrentRow.Cells(5).Value)
            txt_Cones.Text = Val(dgv_Details.CurrentRow.Cells(6).Value)
            txt_Weight.Text = Format(Val(dgv_Details.CurrentRow.Cells(7).Value), "########0.000")

            cbo_grdBagType.Text = dgv_Details.CurrentRow.Cells(11).Value
            txt_Rate.Text = Format(Val(dgv_Details.CurrentRow.Cells(12).Value), "########0.00")
            txt_Amount.Text = Format(Val(dgv_Details.CurrentRow.Cells(13).Value), "########0.00")
            txt_GWeight.Text = Format(Val(dgv_Details.CurrentRow.Cells(14).Value), "########0.000")

            cbo_Det_Location.Text = dgv_Details.CurrentRow.Cells(15).Value
            txt_DetLotNo.Text = dgv_Details.CurrentRow.Cells(16).Value

            If txt_SlNo.Enabled And txt_SlNo.Visible Then txt_SlNo.Focus()

        End If

    End Sub

    Private Sub dgv_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyDown
        vcbo_KeyDwnVal = e.KeyValue

    End Sub

    Private Sub dgv_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyUp
        Dim n As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_Details

                n = .CurrentRow.Index
                .Rows.RemoveAt(n)

                For i = 0 To .Rows.Count - 1
                    .Rows(i).Cells(0).Value = i + 1
                Next

            End With

            Total_Calculation()

            txt_SlNo.Text = dgv_Details.Rows.Count + 1
            cbo_CountName.Text = ""
            cbo_YarnType.Text = "MILL"
            cbo_SetNo.Text = ""
            cbo_MillName.Text = ""
            txt_Bags.Text = ""
            txt_Cones.Text = ""
            txt_Weight.Text = ""

            If cbo_CountName.Enabled And cbo_CountName.Visible Then cbo_CountName.Focus()

        End If

    End Sub

    Private Sub cbo_Ledger_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.GotFocus

        If Common_Procedures.settings.Textile_Sizing_Delivery_receipt_Selection = 1 Then
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( AccountsGroup_IdNo = 10 or Ledger_Type <> 'WEAVER' or Ledger_Type <> 'SIZING' or Ledger_Type <> 'GODOWN' or Ledger_Type <> 'JOBWORKER' or Ledger_Type <> 'REWINDING' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or  (Ledgers_CompanyIdNo <> 0 And Ledgers_CompanyIdNo <> " & Str(Val(lbl_Company.Tag)) & ") Or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
            'Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", " Ledger_Type = 'GODOWN' OR (AccountsGroup_IdNo = 10 and Close_Status = 0 )", "(Ledger_IdNo = 0)")
        Else
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 and Close_Status = 0 )", "(Ledger_IdNo = 0)")
        End If

    End Sub

    Private Sub cbo_Ledger_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyUp
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
    End Sub


    Private Sub cbo_MillName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_MillName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")
        '---Show_Yarn_CurrentStock()
    End Sub

    Private Sub cbo_MillName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_MillName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue

        Try
            With cbo_MillName
                If e.KeyValue = 38 And .DroppedDown = False Then
                    e.Handled = True
                    If Trim(UCase(cbo_YarnType.Text)) = "BABY" Then
                        cbo_SetNo.Focus()

                    Else
                        cbo_YarnType.Focus()

                    End If

                    'SendKeys.Send("+{TAB}")
                ElseIf e.KeyValue = 40 And .DroppedDown = False Then
                    e.Handled = True
                    cbo_grdBagType.Focus()
                    'SendKeys.Send("{TAB}")
                ElseIf e.KeyValue <> 13 And e.KeyValue <> 17 And e.KeyValue <> 27 And .DroppedDown = False Then
                    .DroppedDown = True
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
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

    Private Sub btn_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Close_Form()
    End Sub

    Private Sub btn_Add_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Add.Click
        Dim n As Integer
        Dim MtchSTS As Boolean

        If Trim(cbo_CountName.Text) = "" Then
            MessageBox.Show("Invalid Count Name", "DOES NOT ADD...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_CountName.Enabled And cbo_CountName.Visible Then cbo_CountName.Focus()
            Exit Sub
        End If

        If Common_Procedures.settings.CustomerCode <> "1006" Then
            If Trim(cbo_YarnType.Text) = "" Then
                MessageBox.Show("Invalid Yarn Type", "DOES NOT ADD...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_YarnType.Enabled And cbo_YarnType.Visible Then cbo_YarnType.Focus()
                Exit Sub
            End If
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1034" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1163" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1288" Then
            If Trim(UCase(cbo_YarnType.Text)) = "BABY" And Trim(cbo_SetNo.Text) = "" Then
                MessageBox.Show("Invalid Set No", "DOES NOT ADD...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_SetNo.Enabled And cbo_SetNo.Visible Then cbo_SetNo.Focus()
                Exit Sub
            End If
        End If

        If Trim(cbo_MillName.Text) = "" Then
            MessageBox.Show("Invalid MIll Name", "DOES NOT ADD...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_MillName.Enabled And cbo_MillName.Visible Then cbo_MillName.Focus()
            Exit Sub
        End If

        If Val(txt_Weight.Text) = 0 Then
            MessageBox.Show("Invalid Weight", "DOES NOT ADD...", MessageBoxButtons.OK)
            If txt_Weight.Enabled And txt_Weight.Visible Then txt_Weight.Focus()
            Exit Sub
        End If

        MtchSTS = False

        With dgv_Details

            For i = 0 To .Rows.Count - 1
                If Val(dgv_Details.Rows(i).Cells(0).Value) = Val(txt_SlNo.Text) Then

                    .Rows(i).Cells(1).Value = cbo_CountName.Text
                    .Rows(i).Cells(2).Value = cbo_YarnType.Text
                    .Rows(i).Cells(3).Value = cbo_SetNo.Text
                    .Rows(i).Cells(4).Value = cbo_MillName.Text
                    .Rows(i).Cells(5).Value = Val(txt_Bags.Text)
                    .Rows(i).Cells(6).Value = Val(txt_Cones.Text)
                    .Rows(i).Cells(7).Value = Format(Val(txt_Weight.Text), "########0.000")

                    .Rows(i).Cells(11).Value = cbo_grdBagType.Text
                    .Rows(i).Cells(12).Value = Format(Val(txt_Rate.Text), "########0.00")
                    .Rows(i).Cells(13).Value = Format(Val(txt_Amount.Text), "########0.00")
                    .Rows(i).Cells(14).Value = Format(Val(txt_GWeight.Text), "########0.000")

                    .Rows(i).Cells(15).Value = cbo_Det_Location.Text
                    .Rows(i).Cells(16).Value = txt_DetLotNo.Text

                    .Rows(i).Selected = True

                    MtchSTS = True

                    If i >= 8 Then .FirstDisplayedScrollingRowIndex = i - 7

                    Exit For

                End If

            Next

            If MtchSTS = False Then

                n = .Rows.Add()
                .Rows(n).Cells(0).Value = txt_SlNo.Text
                .Rows(n).Cells(1).Value = cbo_CountName.Text
                .Rows(n).Cells(2).Value = cbo_YarnType.Text
                .Rows(n).Cells(3).Value = cbo_SetNo.Text
                .Rows(n).Cells(4).Value = cbo_MillName.Text
                .Rows(n).Cells(5).Value = Val(txt_Bags.Text)
                .Rows(n).Cells(6).Value = Val(txt_Cones.Text)
                .Rows(n).Cells(7).Value = Format(Val(txt_Weight.Text), "########0.000")

                .Rows(n).Cells(11).Value = cbo_grdBagType.Text
                .Rows(n).Cells(12).Value = Format(Val(txt_Rate.Text), "########0.00")
                .Rows(n).Cells(13).Value = Format(Val(txt_Amount.Text), "########0.00")
                .Rows(n).Cells(14).Value = Format(Val(txt_GWeight.Text), "########0.000")

                .Rows(n).Cells(15).Value = cbo_Det_Location.Text
                .Rows(n).Cells(16).Value = txt_DetLotNo.Text

                .Rows(n).Selected = True

                If n >= 8 Then .FirstDisplayedScrollingRowIndex = n - 7

            End If

        End With

        Total_Calculation()

        txt_SlNo.Text = dgv_Details.Rows.Count + 1
        cbo_CountName.Text = ""
        cbo_YarnType.Text = "MILL"
        cbo_SetNo.Text = ""
        cbo_MillName.Text = ""
        txt_Bags.Text = ""
        txt_Rate.Text = ""
        cbo_grdBagType.Text = ""
        txt_Amount.Text = ""
        txt_Cones.Text = ""
        txt_Weight.Text = ""
        txt_GWeight.Text = ""
        cbo_Det_Location.Text = ""
        txt_DetLotNo.Text = ""


        If cbo_CountName.Enabled And cbo_CountName.Visible Then cbo_CountName.Focus()

    End Sub

    Private Sub btn_Delete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Delete.Click
        Dim n As Integer
        Dim MtchSTS As Boolean

        MtchSTS = False

        With dgv_Details

            For i = 0 To .Rows.Count - 1
                If Val(dgv_Details.Rows(i).Cells(0).Value) = Val(txt_SlNo.Text) Then

                    .Rows.RemoveAt(i)

                    MtchSTS = True

                    Exit For

                End If

            Next

            If MtchSTS = True Then
                For i = 0 To .Rows.Count - 1
                    .Rows(n).Cells(0).Value = i + 1
                Next
            End If

        End With

        Total_Calculation()

        txt_SlNo.Text = dgv_Details.Rows.Count + 1
        cbo_CountName.Text = ""
        cbo_YarnType.Text = "MILL"
        cbo_SetNo.Text = ""
        cbo_MillName.Text = ""
        txt_Bags.Text = ""
        txt_Cones.Text = ""
        txt_Weight.Text = ""
        txt_Rate.Text = ""
        txt_Amount.Text = ""
        txt_GWeight.Text = ""
        cbo_Det_Location.Text = ""
        txt_DetLotNo.Text = ""



        If cbo_CountName.Enabled And cbo_CountName.Visible Then cbo_CountName.Focus()

    End Sub



    Private Sub txt_Bags_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_Bags.TextChanged
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim CntID As Integer
        Dim MilID As Integer
        Dim Cns_Bg As Single, Wt_Cn As Single

        CntID = Common_Procedures.Count_NameToIdNo(con, cbo_CountName.Text)
        MilID = Common_Procedures.Mill_NameToIdNo(con, cbo_MillName.Text)

        If CntID <> 0 And MilID <> 0 And Trim(UCase(cbo_YarnType.Text)) = "MILL" Then

            Cns_Bg = 0 : Wt_Cn = 0
            Da = New SqlClient.SqlDataAdapter("select * from Mill_Count_Details where mill_idno = " & Str(Val(MilID)) & " and count_idno = " & Str(Val(CntID)), con)
            Da.Fill(Dt)

            If Dt.Rows.Count > 0 Then
                Cns_Bg = Val(Dt.Rows(0).Item("Cones_Bag").ToString)
                Wt_Cn = Val(Dt.Rows(0).Item("Weight_Cone").ToString)
            End If

            Dt.Clear()
            Dt.Dispose()
            Da.Dispose()

            If Val(Cns_Bg) <> 0 Then
                txt_Cones.Text = Val(txt_Bags.Text) * Val(Cns_Bg)
            End If
            If Val(Wt_Cn) <> 0 Then
                txt_Weight.Text = Format(Val(txt_Cones.Text) * Val(Wt_Cn), "#########0.000")
            End If

        End If
    End Sub

    Private Sub txt_Cones_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_Cones.TextChanged
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim CntID As Integer
        Dim MilID As Integer
        Dim Cns_Bg As Single, Wt_Cn As Single

        CntID = Common_Procedures.Count_NameToIdNo(con, cbo_CountName.Text)
        MilID = Common_Procedures.Mill_NameToIdNo(con, cbo_MillName.Text)

        If CntID <> 0 And MilID <> 0 And Trim(UCase(cbo_YarnType.Text)) = "MILL" Then

            Cns_Bg = 0 : Wt_Cn = 0
            Da = New SqlClient.SqlDataAdapter("select * from Mill_Count_Details where mill_idno = " & Str(Val(MilID)) & " and count_idno = " & Str(Val(CntID)), con)
            Da.Fill(Dt)

            If Dt.Rows.Count > 0 Then
                Cns_Bg = Val(Dt.Rows(0).Item("Cones_Bag").ToString)
                Wt_Cn = Val(Dt.Rows(0).Item("Weight_Cone").ToString)
            End If

            Dt.Clear()
            Dt.Dispose()
            Da.Dispose()

            If Val(Wt_Cn) <> 0 Then
                txt_Weight.Text = Format(Val(txt_Cones.Text) * Val(Wt_Cn), "#########0.000")
            End If

        End If
    End Sub

    Private Sub cbo_CountName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_CountName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
    End Sub

    Private Sub cbo_CountName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_CountName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Sizing_Count_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_CountName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If

    End Sub

    Private Sub cbo_CountName_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_CountName.LostFocus
        Show_Yarn_CurrentStock()
    End Sub

    Private Sub cbo_Transport_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Transport.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Transport, cbo_Delivered, cbo_VehicleNo, "Transport_Head", "Transport_Name", "", "(Transport_IdNo = 0)")
    End Sub

    Private Sub cbo_Transport_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Transport.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Transport, cbo_VehicleNo, "Transport_Head", "Transport_Name", "", "(Transport_IdNo = 0)")
    End Sub

    Private Sub cbo_SetNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_SetNo.GotFocus
        Dim NewCode As String
        Dim Led_ID As Integer, Cnt_ID As Integer
        Dim Condt As String
        Dim Cmp_Cond As String = ""

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)

        Cnt_ID = Common_Procedures.Count_NameToIdNo(con, cbo_CountName.Text)

        Cmp_Cond = ""
        If Val(Common_Procedures.settings.StatementPrint_InStock_Combine_AllCompany) = 0 Then
            Cmp_Cond = "a.Company_IdNo = " & Str(Val(lbl_Company.Tag))
        End If

        Condt = "( " & Cmp_Cond & IIf(Trim(Cmp_Cond) <> "", " and ", "") & " a.Ledger_IdNo = " & Str(Val(Led_ID)) & " and a.Count_IdNo = " & Str(Val(Cnt_ID)) & " and (  (a.Baby_Weight - a.Delivered_Weight) > 0 or (a.setcode_forSelection IN (select z.setcode_forSelection from SizingSoft_Yarn_Delivery_Details z where z.company_idno = " & Str(Val(lbl_Company.Tag)) & " and z.Yarn_Delivery_Code = '" & Trim(NewCode) & "') ) ) )"
        'Condt = "( a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Ledger_IdNo = " & Str(Val(Led_ID)) & " and a.Count_IdNo = " & Str(Val(Cnt_ID)) & " and (  (a.Baby_Weight - a.Delivered_Weight) > 0 or (a.setcode_forSelection IN (select z.setcode_forSelection from SizingSoft_Yarn_Delivery_Details z where z.company_idno = " & Str(Val(lbl_Company.Tag)) & " and z.Yarn_Delivery_Code = '" & Trim(NewCode) & "') ) ) )"

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Stock_BabyCone_Processing_Details a", "setcode_forSelection", Condt, "(Reference_Code = '')")

        cbo_SetNo.Tag = cbo_SetNo.Text

    End Sub

    Private Sub cbo_SetNo_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_SetNo.LostFocus
        If Trim(UCase(cbo_SetNo.Text)) <> Trim(UCase(cbo_SetNo.Tag)) Then
            get_BabyCone_Details()
        End If
    End Sub

    Private Sub cbo_setno_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_SetNo.KeyDown
        Dim NewCode As String
        Dim Led_ID As Integer, Cnt_ID As Integer
        Dim Condt As String
        Dim Cmp_Cond As String


        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)

        Cnt_ID = Common_Procedures.Count_NameToIdNo(con, cbo_CountName.Text)

        Cmp_Cond = ""
        If Val(Common_Procedures.settings.StatementPrint_InStock_Combine_AllCompany) = 0 Then
            Cmp_Cond = "a.Company_IdNo = " & Str(Val(lbl_Company.Tag))
        End If

        Condt = "( " & Cmp_Cond & IIf(Trim(Cmp_Cond) <> "", " and ", "") & " a.Ledger_IdNo = " & Str(Val(Led_ID)) & " and a.Count_IdNo = " & Str(Val(Cnt_ID)) & " and (  (a.Baby_Weight - a.Delivered_Weight) > 0 or (a.setcode_forSelection IN (select z.setcode_forSelection from SizingSoft_Yarn_Delivery_Details z where z.company_idno = " & Str(Val(lbl_Company.Tag)) & " and z.Yarn_Delivery_Code = '" & Trim(NewCode) & "') ) ) )"
        'Condt = "( a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Ledger_IdNo = " & Str(Val(Led_ID)) & " and a.Count_IdNo = " & Str(Val(Cnt_ID)) & " and (  (a.Baby_Weight - a.Delivered_Weight) > 0 or (a.setcode_forSelection IN (select z.setcode_forSelection from SizingSoft_Yarn_Delivery_Details z where z.company_idno = " & Str(Val(lbl_Company.Tag)) & " and z.Yarn_Delivery_Code = '" & Trim(NewCode) & "') ) ) )"

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_SetNo, cbo_YarnType, cbo_MillName, "Stock_BabyCone_Processing_Details", "setcode_forSelection", Condt, "(Reference_Code = '')")

    End Sub

    Private Sub cbo_setno_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_SetNo.KeyPress
        Dim NewCode As String
        Dim Led_ID As Integer, Cnt_ID As Integer
        Dim Condt As String
        Dim Cmp_Cond As String = ""

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)

        Cnt_ID = Common_Procedures.Count_NameToIdNo(con, cbo_CountName.Text)

        Cmp_Cond = ""
        If Val(Common_Procedures.settings.StatementPrint_InStock_Combine_AllCompany) = 0 Then
            Cmp_Cond = "a.Company_IdNo = " & Str(Val(lbl_Company.Tag))
        End If

        Condt = "( " & Cmp_Cond & IIf(Trim(Cmp_Cond) <> "", " and ", "") & " a.Ledger_IdNo = " & Str(Val(Led_ID)) & " and a.Count_IdNo = " & Str(Val(Cnt_ID)) & " and (  (a.Baby_Weight - a.Delivered_Weight) > 0 or (a.setcode_forSelection IN (select z.setcode_forSelection from SizingSoft_Yarn_Delivery_Details z where z.company_idno = " & Str(Val(lbl_Company.Tag)) & " and z.Yarn_Delivery_Code = '" & Trim(NewCode) & "') ) ) )"
        'Condt = "( a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Ledger_IdNo = " & Str(Val(Led_ID)) & " and a.Count_IdNo = " & Str(Val(Cnt_ID)) & " and (  (a.Baby_Weight - a.Delivered_Weight) > 0 or (a.setcode_forSelection IN (select z.setcode_forSelection from SizingSoft_Yarn_Delivery_Details z where z.company_idno = " & Str(Val(lbl_Company.Tag)) & " and z.Yarn_Delivery_Code = '" & Trim(NewCode) & "') ) ) )"

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_SetNo, cbo_MillName, "Stock_BabyCone_Processing_Details a", "setcode_forSelection", Condt, "(Reference_Code = '')")

    End Sub

    Private Sub cbo_Ledger_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        If Common_Procedures.settings.Textile_Sizing_Delivery_receipt_Selection = 1 Then
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, dtp_Date, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "Ledger_Type = 'GODOWN' OR (AccountsGroup_IdNo = 10 and Close_Status = 0 )", "(Ledger_IdNo = 0)")
        Else
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, dtp_Date, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 and Close_Status = 0 )", "(Ledger_IdNo = 0)")
        End If


        If e.KeyCode = 40 And cbo_Ledger.DroppedDown = False Or (e.Control = True And e.KeyCode) = 40 Then
            If Trim(Common_Procedures.settings.CustomerCode) = "1112" Then '----VENUS SIZING
                If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
            Else
                If txt_BookNo.Enabled And txt_BookNo.Visible Then txt_BookNo.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_Ledger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ledger.KeyPress
        Dim TexStk_iD As String = 0
        Dim LedIdNo As Integer = 0
        If Common_Procedures.settings.Textile_Sizing_Delivery_receipt_Selection = 1 Then
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Ledger, dtp_Date, "Ledger_AlaisHead", "Ledger_DisplayName", "Ledger_Type = 'GODOWN' OR (AccountsGroup_IdNo = 10 and Close_Status = 0 )", "(Ledger_IdNo = 0)")
        Else
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Ledger, dtp_Date, "Ledger_AlaisHead", "Ledger_DisplayName", " (AccountsGroup_IdNo = 10 and Close_Status = 0 )", "(Ledger_IdNo = 0)")
        End If

        If Asc(e.KeyChar) = 13 Then
            'If Common_Procedures.settings.Combine_Textile_Sizing_Software_Status = 1 Then

            '    LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text, , TrnTo_DbName)
            '    TexStk_iD = Val(Common_Procedures.get_FieldValue(con, "ledger_head", "Textile_To_CompanyIdNo", "(ledger_idno = " & Str(Val(LedIdNo)) & ")"))
            '    If Val(TexStk_iD) <> 0 Then

            '        If MessageBox.Show("Do you want to select Requirement:", "FOR REQUIREMENT SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
            '            btn_Selection_Click(sender, e)

            '        Else
            '            txt_BookNo.Focus()

            '        End If

            '    Else
            '        txt_BookNo.Focus()

            '    End If

            'Else

            '    txt_BookNo.Focus()

            'End If
            If Trim(Common_Procedures.settings.CustomerCode) = "1112" Then '----VENUS SIZING
                If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
            Else
                If txt_BookNo.Enabled And txt_BookNo.Visible Then txt_BookNo.Focus()
            End If

        End If
    End Sub

    Private Sub cbo_Filter_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, dtp_Filter_ToDate, cbo_Filter_CountName, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, cbo_Filter_CountName, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_CountName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_CountName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_CountName, txt_SlNo, Nothing, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
        If (e.KeyValue = 40 And cbo_CountName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If Trim(cbo_CountName.Text) <> "" Then
                cbo_YarnType.Focus()
            Else
                txt_EmptyBeam.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_Filter_CountName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_CountName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_CountName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_CountName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_CountName, cbo_Filter_PartyName, cbo_Filter_MillName, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_CountName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_CountName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_CountName, cbo_Filter_MillName, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
    End Sub

    Private Sub cbo_VehicleNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_VehicleNo.GotFocus
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim trans_id As Integer = 0

        If Trim(cbo_VehicleNo.Text) = "" And Trim(cbo_Transport.Text) <> "" Then

            trans_id = Common_Procedures.Transport_NameToIdNo(con, cbo_Transport.Text)

            Try

                If trans_id <> 0 Then
                    da1 = New SqlClient.SqlDataAdapter("select top 1 * from SizingSoft_Yarn_Delivery_Head where Transport_IdNo = " & Str(Val(trans_id)) & " Order by Yarn_Delivery_Date desc, for_Orderby desc, Yarn_Delivery_No desc", con)
                    dt1 = New DataTable
                    da1.Fill(dt1)

                    If dt1.Rows.Count > 0 Then
                        cbo_VehicleNo.Text = dt1.Rows(0).Item("Vehicle_No").ToString
                    End If

                    dt1.Clear()
                    dt1.Dispose()
                    da1.Dispose()
                End If

            Catch ex As Exception
                MessageBox.Show(ex.Message, "INVALID VEHICLE DETAILS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End Try

        End If
    End Sub

    Private Sub cbo_VehicleNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_VehicleNo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_VehicleNo, Nothing, "", "", "", "", False)
        If Asc(e.KeyChar) = 13 Then
            If cbo_VendorName.Visible = True Then
                cbo_VendorName.Focus()
            ElseIf txt_DeliveryAt.Visible = True Then
                txt_DeliveryAt.Focus()
            Else
                cbo_DeliveryTo.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_VehicleNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_VehicleNo.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_VehicleNo, cbo_Transport, Nothing, "", "", "", "")

        If (e.KeyValue = 40 And cbo_VehicleNo.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Or Trim(Common_Procedures.settings.CustomerCode) = "1282" Or Trim(Common_Procedures.settings.CustomerCode) = "1288" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1263" Then
                cbo_VendorName.Focus()
            Else
                cbo_DeliveryTo.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_CountName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_CountName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_CountName, Nothing, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            If Trim(cbo_CountName.Text) <> "" Then
                cbo_YarnType.Focus()
            Else
                'txt_EmptyBeam.Focus()
                If txt_EmptyBeam.Enabled And txt_EmptyBeam.Visible Then
                    txt_EmptyBeam.Focus()
                Else
                    txt_EmptyBags.Focus()
                End If
            End If
        End If
    End Sub
    Private Sub cbo_Beamwidth_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_BeamWidth.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_BeamWidth, txt_EmptyBeam, txt_EmptyBags, "Beam_Width_Head", "Beam_Width_Name", "", "(Beam_Width_IdNo = 0)")
    End Sub

    Private Sub cbo_BeamWidth_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_BeamWidth.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_BeamWidth, txt_EmptyBags, "Beam_Width_Head", "Beam_Width_Name", "", "(Beam_Width_IdNo = 0)")
    End Sub


    Private Sub cbo_MillName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_MillName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_MillName, cbo_grdBagType, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")

    End Sub

    Private Sub cbo_Filter_MillName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_MillName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_MillName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_MillName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_MillName, cbo_Filter_CountName, btn_Filter_Show, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")

    End Sub

    Private Sub cbo_Filter_MillName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_MillName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_MillName, btn_Filter_Show, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            btn_Filter_Show_Click(sender, e)
        End If
    End Sub

    Private Sub txt_EmptyCones_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_EmptyCones.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_EmptyBags_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_EmptyBags.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If


    End Sub

    Private Sub txt_Weight_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Weight.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_Weight_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Weight.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            If Common_Procedures.settings.CustomerCode = "1288" Then
                txt_GWeight.Focus()
            Else
                'SendKeys.Send("{TAB}")
                btn_Add_Click(sender, e)
            End If

            'SendKeys.Send("{TAB}")
        End If
    End Sub


    Private Sub txt_GWeight_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_GWeight.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_GWeight_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_GWeight.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

        If cbo_Det_Location.Visible Then
            cbo_Det_Location.Focus()
        Else
            If Asc(e.KeyChar) = 13 Then
                btn_Add_Click(sender, e)
                'SendKeys.Send("{TAB}")
            End If
        End If

    End Sub

    Private Sub txt_EmptyBeam_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_EmptyBeam.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then cbo_CountName.Focus() ' SendKeys.Send("+{TAB}")
    End Sub


    Private Sub txt_Remarks_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Remarks.KeyDown
        If (e.KeyValue = 38) Then

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1263" Or Trim(Common_Procedures.settings.CustomerCode) = "1282" Or Trim(Common_Procedures.settings.CustomerCode) = "1288" Then
                cbo_VendorName.Focus()

            Else

                cbo_DeliveryTo.Focus()
            End If

        End If
        If (e.KeyValue = 40) Then
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1282" Then

                chk_Loaded.Focus()
            Else
                If MessageBox.Show("Do you want to save? ", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                    save_record()

                End If

            End If

        End If
    End Sub

    Private Sub chk_Loaded_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles chk_Loaded.KeyDown
        If e.KeyCode = 38 Then
            txt_Remarks.Focus()

        End If
        If e.KeyCode = 40 Then
            If MessageBox.Show("Do you want to save? ", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()

            End If

        End If

    End Sub


    Private Sub chk_Loaded_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles chk_Loaded.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save? ", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()

            End If
        End If
    End Sub

    Private Sub txt_Remarks_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Remarks.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1282" Then

                chk_Loaded.Focus()
            Else
                If MessageBox.Show("Do you want to save? ", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                    save_record()

                End If

            End If

        End If

    End Sub
    Private Sub Show_Yarn_CurrentStock()
        Dim vCntID As Integer
        Dim vLedID As Integer
        Dim CurStk As Decimal
        Dim Vdate As Date


        If Trim(cbo_Ledger.Text) <> "" And Trim(cbo_CountName.Text) <> "" Then
            vLedID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
            vCntID = Common_Procedures.Count_NameToIdNo(con, cbo_CountName.Text)
            Vdate = dtp_Date.Value
            ' CurStk = 0
            If Val(cbo_Ledger.Tag) <> Val(vLedID) Or Val(lbl_AvailableStock.Tag) <> Val(vCntID) Then
                lbl_AvailableStock.Tag = 0
                lbl_AvailableStock.Text = ""
                CurStk = 0
                If Val(vLedID) <> 0 And Val(vCntID) <> 0 Then
                    CurStk = Common_Procedures.get_Yarn_CurrentStock(con, Val(lbl_Company.Tag), vLedID, vCntID)
                    cbo_Ledger.Tag = Val(vLedID)
                    lbl_AvailableStock.Tag = Val(vCntID)
                    lbl_AvailableStock.Text = Format(Val(CurStk), "#########0.000")
                End If
            End If

        Else
            cbo_Ledger.Tag = 0
            'lbl_AvailableStock.Tag = 0
            'lbl_AvailableStock.Text = ""

        End If
    End Sub

    Private Sub get_BabyCone_Details()
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim CntID As Integer
        Dim NewCode As String
        Dim Ent_Bgs As Integer, Ent_Cns As Integer
        Dim Ent_Wgt As Single


        CntID = Common_Procedures.Count_NameToIdNo(con, cbo_CountName.Text)

        If CntID <> 0 And Trim(cbo_SetNo.Text) <> "" And Trim(UCase(cbo_YarnType.Text)) = "BABY" Then

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select a.*, b.mill_name, c.bags as DelvEnt_Bags, c.cones as DelvEnt_cones, c.Weight as DelvEnt_Weight from Stock_BabyCone_Processing_Details a INNER JOIN mill_head b ON  a.mill_idno = b.mill_idno LEFT OUTER JOIN SizingSoft_Yarn_Delivery_Details c ON c.Yarn_Delivery_Code = '" & Trim(NewCode) & "' and c.yarn_type = 'BABY' and a.SetCode_ForSelection = c.SetCode_ForSelection where a.setcode_forSelection = '" & Trim(cbo_SetNo.Text) & "' and a.count_idno = " & Str(Val(CntID)), con)
            Da.Fill(Dt)

            If Dt.Rows.Count > 0 Then

                Ent_Bgs = 0 : Ent_Cns = 0 : Ent_Wgt = 0

                If IsDBNull(Dt.Rows(0).Item("DelvEnt_Bags").ToString) = False Then Ent_Bgs = Val(Dt.Rows(0).Item("DelvEnt_Bags").ToString)
                If IsDBNull(Dt.Rows(0).Item("DelvEnt_cones").ToString) = False Then Ent_Cns = Val(Dt.Rows(0).Item("DelvEnt_cones").ToString)
                If IsDBNull(Dt.Rows(0).Item("DelvEnt_Weight").ToString) = False Then Ent_Wgt = Val(Dt.Rows(0).Item("DelvEnt_Weight").ToString)

                cbo_MillName.Text = Dt.Rows(0).Item("Mill_Name").ToString
                txt_Bags.Text = (Val(Dt.Rows(0).Item("Baby_Bags").ToString) - Val(Dt.Rows(0).Item("Delivered_Bags").ToString) + Ent_Bgs)
                txt_Cones.Text = (Val(Dt.Rows(0).Item("Baby_Cones").ToString) - Val(Dt.Rows(0).Item("Delivered_Cones").ToString) + Ent_Cns)
                txt_Weight.Text = (Val(Dt.Rows(0).Item("Baby_Weight").ToString) - Val(Dt.Rows(0).Item("Delivered_Weight").ToString) + Ent_Wgt)

            End If

            Dt.Clear()
            Dt.Dispose()
            Da.Dispose()

        End If

    End Sub

    Private Sub Total_Calculation()
        Dim Sno As Integer
        Dim TotBags As Single, TotCones As Single, TotWeight As Single

        Sno = 0
        TotBags = 0
        TotCones = 0
        TotWeight = 0
        With dgv_Details
            For i = 0 To .RowCount - 1
                Sno = Sno + 1
                .Rows(i).Cells(0).Value = Sno
                If Val(.Rows(i).Cells(7).Value) <> 0 Then
                    TotBags = TotBags + Val(.Rows(i).Cells(5).Value)
                    TotCones = TotCones + Val(.Rows(i).Cells(6).Value)
                    TotWeight = TotWeight + Val(.Rows(i).Cells(7).Value)
                End If
            Next
        End With

        With dgv_Details_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(5).Value = Val(TotBags)
            .Rows(0).Cells(6).Value = Val(TotCones)
            .Rows(0).Cells(7).Value = Format(Val(TotWeight), "########0.000")
        End With

    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.PrintEntry, Common_Procedures.UR.ENTRY_SIZING_JOBWORK_MODULE_YARN_DELIVERY, New_Entry) = False Then Exit Sub
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1034" Then '---- Asia Sizing (Palladam)
            pnl_Print.Visible = True
            pnl_Back.Enabled = False
            If btn_Print_Preprint.Enabled And btn_Print_Preprint.Visible Then
                btn_Print_Preprint.Focus()
            End If

        ElseIf Val(Common_Procedures.settings.Dos_Printing) = 1 Then
            Pnl_DosPrint.Visible = True
            pnl_Back.Enabled = False
            If Btn_DosPrint.Enabled And Btn_DosPrint.Visible Then
                Btn_DosPrint.Focus()
            End If



        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1037" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1038" Then '---- Prakash sizing

            Dim mymsgbox As New Tsoft_MessageBox("Select Paper Size to Print", "A4,HALF-SHEET,CANCEL", "FOR DELIVERY PRINTING...", "IF A4 is selected, 2 copies of dc will be printed in single A4 sheet," & Chr(13) & "If HALF-SHEET is selected 1 copy of dc will be printed in 8x6 paper size", MesssageBoxIcons.Questions, 2)
            mymsgbox.ShowDialog()

            If mymsgbox.MessageBoxResult = 1 Then
                vPrnt_2Copy_In_SinglePage = 1

            ElseIf mymsgbox.MessageBoxResult = 2 Then
                Prnt_HalfSheet_STS = True
                vPrnt_2Copy_In_SinglePage = 0

            Else

                Exit Sub

            End If

            'prn_TotCopies = Val(InputBox("Enter No.of Copies", "FOR DELIVERY PRINTING...", "2"))
            'If Val(prn_TotCopies) <= 0 Then
            '    Exit Sub
            'End If

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
                    MessageBox.Show("The printing operation failed" & vbCrLf & ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                End Try


            Else
                Try

                    Dim ppd As New PrintPreviewDialog

                    ppd.Document = PrintDocument1

                    ppd.WindowState = FormWindowState.Maximized
                    ppd.StartPosition = FormStartPosition.CenterScreen
                    'ppd.ClientSize = New Size(600, 600)
                    ppd.PrintPreviewControl.AutoZoom = True
                    ppd.PrintPreviewControl.Zoom = 1.0

                    ppd.ShowDialog()

                Catch ex As Exception
                    MsgBox("The printing operation failed" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "DOES NOT SHOW PRINT PREVIEW...")

                End Try

            End If


        Else
            printing_invoice()

        End If

    End Sub



    Private Sub set_PaperSize_For_PrintDocument1()
        Dim I As Integer = 0
        Dim PpSzSTS As Boolean = False
        Dim ps As Printing.PaperSize

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1242" Then
            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                    PrintDocument1.DefaultPageSettings.PaperSize = ps
                    Exit For
                End If
            Next


        ElseIf Val(Common_Procedures.settings.Printing_For_HalfSheet_Set_Custom8X6_As_Default_PaperSize) = 1 Or Prnt_HalfSheet_STS = True Then

            Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8X6", 800, 600)
            PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
            PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1
            PrintDocument1.DefaultPageSettings.Landscape = False

        ElseIf Val(Common_Procedures.settings.Printing_For_HalfSheet_Set_A4_As_Default_PaperSize) = 1 Or Val(Common_Procedures.settings.Printing_For_FullSheet_Set_A4_As_Default_PaperSize) = 1 Or Val(vPrnt_2Copy_In_SinglePage) = 1 Then

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

                For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
                        ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                        PrintDocument1.DefaultPageSettings.PaperSize = ps
                        PpSzSTS = True
                        Exit For
                    End If
                Next

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

    Private Sub printing_invoice()
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String
        Dim PpSzSTS As Boolean = False
        Dim I As Integer = 0
        Dim ps As Printing.PaperSize
        Dim inpno As String = ""


        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.* from SizingSoft_Yarn_Delivery_Head a LEFT OUTER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo where a.Yarn_Delivery_Code = '" & Trim(NewCode) & "'", con)
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

        prn_TotCopies = 1
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1282" Then
            inpno = InputBox("Enter No.of Copies", "FOR PRINTING...", 4)
            prn_TotCopies = Val(inpno)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1263" Then '---- Meenashi Sizing (Somanur)
            inpno = InputBox("Enter No.of Copies", "FOR PRINTING...", 3)
            prn_TotCopies = Val(inpno)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1288" Then '---- kkp
            inpno = InputBox("Enter No.of Copies", "FOR PRINTING...", 1)
            prn_TotCopies = Val(inpno)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1034" Then '---- Asia Sizing (palladam)
            prn_TotCopies = 2
        End If
        If Val(prn_TotCopies) <= 0 Then
            Exit Sub
        End If

        'Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8X6", 800, 600)
        'PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
        'PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1

        If Common_Procedures.settings.CustomerCode = "1288" Then
            PrintDocument1.DefaultPageSettings.Landscape = True
        Else
            PrintDocument1.DefaultPageSettings.Landscape = False
        End If


        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1034" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1033" Then
            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                'Debug.Print(ps.PaperName)
                If ps.Width = 800 And ps.Height = 600 Then
                    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                    PrintDocument1.DefaultPageSettings.PaperSize = ps
                    PpSzSTS = True
                    Exit For
                End If
            Next

            If Common_Procedures.settings.CustomerCode = "1282" Then

                For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A5 Then
                        ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                        PrintDocument1.DefaultPageSettings.PaperSize = ps
                        PpSzSTS = True
                        Exit For
                    End If
                Next

                'If PpSzSTS = False Then
                '    For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                '        If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
                '            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                '            PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                '            PrintDocument1.DefaultPageSettings.PaperSize = ps
                '            PpSzSTS = True
                '            Exit For
                '        End If
                '    Next
            Else
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

        Else

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


        'If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then
        '    Try
        '        If Val(Common_Procedures.settings.Printing_Show_PrintDialogue) = 1 Then
        '            PrintDialog1.PrinterSettings = PrintDocument1.PrinterSettings
        '            If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
        '                PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings
        '                PrintDocument1.Print()
        '            End If
        '            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1034" Then


        '                For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '                    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '                    'Debug.Print(ps.PaperName)
        '                    If ps.Width = 800 And ps.Height = 600 Then
        '                        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
        '                        PrintDocument1.DefaultPageSettings.PaperSize = ps
        '                        PpSzSTS = True
        '                        Exit For
        '                    End If
        '                Next

        '                If PpSzSTS = False Then
        '                    For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '                        If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
        '                            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '                            PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
        '                            PrintDocument1.DefaultPageSettings.PaperSize = ps
        '                            PpSzSTS = True
        '                            Exit For
        '                        End If
        '                    Next

        '                    If PpSzSTS = False Then
        '                        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '                            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
        '                                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '                                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
        '                                PrintDocument1.DefaultPageSettings.PaperSize = ps
        '                                Exit For
        '                            End If
        '                        Next
        '                    End If

        '                End If
        '            Else
        '                If PpSzSTS = False Then
        '                    For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '                        If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
        '                            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '                            PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
        '                            PrintDocument1.DefaultPageSettings.PaperSize = ps
        '                            Exit For
        '                        End If
        '                    Next
        '                End If
        '            End If

        '        Else
        '            PrintDocument1.Print()

        '        End If


        '    Catch ex As Exception
        '        MessageBox.Show("The printing operation failed" & vbCrLf & ex.Message, "DOES NOT SHOW PRINT PREVIEW...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        '    End Try


        'Else

        '    Try

        '        Dim ppd As New PrintPreviewDialog

        '        ppd.Document = PrintDocument1

        '        ppd.WindowState = FormWindowState.Maximized
        '        ppd.StartPosition = FormStartPosition.CenterScreen
        '        'ppd.ClientSize = New Size(600, 600)

        '        AddHandler ppd.Shown, AddressOf PrintPreview_Shown
        '        ppd.ShowDialog()
        '        'If PageSetupDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
        '        '    PrintDocument1.DefaultPageSettings = PageSetupDialog1.PageSettings
        '        '    ppd.ShowDialog()
        '        'End If

        '    Catch ex As Exception
        '        MsgBox("The printing operation failed" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "DOES NOT SHOW PRINT PREVIEW...")

        '    End Try

        'End If










        '--------------------------------------------------------



        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then
            Try

                If Print_PDF_Status = True Then
                    '--This is actual & correct 

                    PrintDocument1.DocumentName = "Invoice"
                    PrintDocument1.PrinterSettings.PrinterName = "doPDF v7"
                    PrintDocument1.PrinterSettings.PrintFileName = "c:\Invoice.pdf"
                    PrintDocument1.Print()

                Else

                    If Val(Common_Procedures.settings.Printing_Show_PrintDialogue) = 1 Then
                        PrintDialog1.PrinterSettings = PrintDocument1.PrinterSettings
                        If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                            PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings

                            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1

                                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                                    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                                    PrintDocument1.DefaultPageSettings.PaperSize = ps
                                    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                                    Exit For
                                End If


                            Next

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


        '--------------------------------------------------------





    End Sub

    Private Sub PrintDocument1_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument1.BeginPrint
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim NewCode As String

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0
        prn_Count = 0
        Try

            da1 = New SqlClient.SqlDataAdapter("Select a.*, b.*, c.*, Csh.State_Name as Company_State_Name, Csh.State_Code as Company_State_Code, Lsh.State_Name as Ledger_State_Name, Lsh.State_Code as Ledger_State_Code,d.* ,f.Ledger_MainName as DelName , f.Ledger_Address1 as DelAdd1 ,f.Ledger_Address2 as DelAdd2, f.Ledger_Address3 as DelAdd3 ,f.Ledger_Address4 as DelAdd4,f.Ledger_GSTinNo as DelGSTinNo,DSH.State_Name as DelState_Name ,DSH.State_Code as Delivery_State_Code, vh.Vendor_Name,vh.* from SizingSoft_Yarn_Delivery_Head a INNER JOIN Company_Head b ON a.Company_IdNo <> 0 and a.Company_IdNo = b.Company_IdNo  LEFT OUTER JOIN State_Head Csh ON b.Company_State_IdNo = csh.State_IdNo INNER JOIN Ledger_Head c ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = c.Ledger_IdNo  LEFT OUTER JOIN State_Head Lsh ON c.ledger_State_IdNo = Lsh.State_IdNo  LEFT OUTER JOIN Transport_Head d ON a.Transport_IdNo = d.Transport_IdNo  LEFT OUTER JOIN Delivery_Party_Head f ON a.DeliveryTo_IdNo = f.Ledger_IdNo LEFT OUTER JOIN State_HEad DSH on f.Ledger_State_IdNo = DSH.State_IdNo left outer join Vendor_Head vh ON vh.Vendor_IdNo = a.Vendor_IdNo  where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Yarn_Delivery_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Mill_Name, c.*, d.set_no from SizingSoft_Yarn_Delivery_Details a INNER JOIN Mill_Head b on a.Mill_IdNo = b.Mill_IdNo INNER JOIN Count_Head c on a.Count_IdNo = c.Count_IdNo LEFT OUTER JOIN Specification_Head d ON a.SetCode_ForSelection <> '' and a.SetCode_ForSelection = d.setcode_forSelection where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Yarn_Delivery_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                prn_DetDt = New DataTable
                da2.Fill(prn_DetDt)

                da2.Dispose()

            Else
                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If

            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub PrintDocument1_EndPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument1.EndPrint

        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then
            chk_Printed.Checked = True
            Update_PrintOut_Status()
        End If

    End Sub

    Private Sub PrintDocument1_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        If prn_HdDt.Rows.Count <= 0 Then Exit Sub
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1034" Then '---- Asia Sizing (Palladam)
            If prn_Status = 1 Then
                'If Trim(UCase(Common_Procedures.settings.CompanyName)) = "" Then
                Printing_Format1(e)
            Else
                Printing_Format2(e)
            End If 'End 
        ElseIf Val(Common_Procedures.settings.YarnDelivery_Print_2Copy_In_SinglePage) = 1 Then
            Printing_Format4(e)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1102" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1144" Then '---- Ganesh karthik Sizing (Somanur)
            Printing_Format5(e)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1282" Then
            Printing_Format6(e)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1288" Then '-----KKP SPINNING MILLS PVT. LTD
            Printing_Format7(e)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then '-----KKP SPINNING MILLS PVT. LTD
            Printing_Format1087(e)
        Else
            Printing_Format1(e)

        End If
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
        Dim ItmNm1 As String, ItmNm2 As String
        'Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim PS As Printing.PaperSize
        Dim PCnt As Integer = 0, PrntCnt As Integer = 0

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1037" Or Common_Procedures.settings.CustomerCode = "1038" Then
            PrntCnt = 1
            If Val(vPrnt_2Copy_In_SinglePage) = 1 Then
                If PrntCnt2ndPageSTS = False Then
                    PrntCnt = 2
                End If
            End If

            set_PaperSize_For_PrintDocument1()
        Else
            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                    PS = PrintDocument1.PrinterSettings.PaperSizes(I)
                    PrintDocument1.DefaultPageSettings.PaperSize = PS
                    e.PageSettings.PaperSize = PS
                    Exit For
                End If
            Next
        End If



        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 20  ' 50 
            .Right = 55
            .Top = 20   '30
            .Bottom = 35 ' 30
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

        If Common_Procedures.settings.CustomerCode = "1155" Then
            NoofItems_PerPage = 3 ' 6 ' 5
        Else
        NoofItems_PerPage = 5 ' 6 ' 5
        End If

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = 40
        ClArr(2) = 60 : ClArr(3) = 80 : ClArr(4) = 210 : ClArr(5) = 100 : ClArr(6) = 70 : ClArr(7) = 70
        ClArr(8) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7))

        If Common_Procedures.settings.CustomerCode = "1155" Then
            TxtHgt = 17.5
        Else
        TxtHgt = 18.5
        End If

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format1_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                Try

                    NoofDets = 0

                    CurY = CurY - 10

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
                            If Len(ItmNm1) > 18 Then
                                For I = 18 To 1 Step -1
                                    If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                                Next I
                                If I = 0 Then I = 18
                                ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                                ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                            End If


                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Sl_No").ToString), LMargin + 25, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Yarn_Type").ToString, LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                            If IsDBNull(prn_DetDt.Rows(prn_DetIndx).Item("Set_no").ToString) = False Then
                                If Trim(prn_DetDt.Rows(prn_DetIndx).Item("Set_no").ToString) <> "" Then
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Set_no").ToString), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                                End If
                            End If
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Count_Name").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 10, CurY, 0, 0, pFont)
                            If Val(prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString) <> 0 Then
                                Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)
                            End If
                            If Val(prn_DetDt.Rows(prn_DetIndx).Item("Cones").ToString) <> 0 Then
                                Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Cones").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
                            End If
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Weight").ToString), "########0.000"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            If Trim(ItmNm2) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If

                            prn_DetIndx = prn_DetIndx + 1

                        Loop

                    End If

                    Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)

                Catch ex As Exception

                    MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                End Try

            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        prn_Count = prn_Count + 1

        e.HasMorePages = False

        If Val(prn_TotCopies) > 1 Then
            If prn_Count < Val(prn_TotCopies) Then

                prn_DetIndx = 0
                prn_PageNo = 0
                prn_DetIndx = 0
                prn_PageNo = 0
                'prn_NoofBmDets = 0


                e.HasMorePages = True
                Return

            End If

        End If


    End Sub

    Private Sub Printing_Format1_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_Email As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim p1Font As Font
        Dim strHeight As Single = 0, strWidth As Single = 0
        Dim W1 As Single, C1 As Single, S1 As Single, K1 As Single
        Dim CurX As Single = 0
        Dim Gst_dt As Date
        Dim Entry_dt As Date
        Dim Led_Name As String, Led_Add1 As String, Led_Add2 As String, Led_Add3 As String, Led_Add4 As String
        Dim Led_GstNo As String, Led_TinNo As String
        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Mill_Name, c.Count_Name  from SizingSoft_Yarn_Delivery_Details a INNER JOIN Mill_Head b on a.Mill_IdNo = b.Mill_IdNo LEFT OUTER JOIN Count_Head c on a.Count_IdNo = c.Count_IdNo where a.Yarn_Delivery_Code = '" & Trim(EntryCode) & "' Order by a.sl_no", con)
        da2.Fill(dt2)
        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        dt2.Clear()


        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_Email = ""
        Cmp_StateCap = "" : Cmp_StateNm = "" : Cmp_StateCode = "" : Cmp_GSTIN_Cap = "" : Cmp_GSTIN_No = ""

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1015" Then
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString
            Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address2").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString

        Else

            If Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString) <> "" Then
                Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
                Cmp_Name = Replace(Cmp_Name, Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString), "")
                Cmp_Add1 = Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString)
                Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString

            Else
                Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
                Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
                Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString

            End If

        End If
        'Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        'Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
        'Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString
        If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "PHONE NO.:" & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
            Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_panNo").ToString) <> "" Then
            Cmp_CstNo = "PAN NO.: " & prn_HdDt.Rows(0).Item("Company_panNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
            Cmp_Email = "E-mail : " & Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString)
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




        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 18, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        If Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString) <> "" Then
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Else
            p1Font = New Font("Calibri", 9, FontStyle.Regular)
        End If
        CurY = CurY + strHeight
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, p1Font)

        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height
        CurY = CurY + strHeight
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)

        'CurY = CurY + strHeight
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1 & " " & Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)

        Gst_dt = #7/1/2017#
        Entry_dt = dtp_Date.Value

        If DateDiff("d", Gst_dt.ToShortDateString, Entry_dt.ToShortDateString) < 0 Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt - 1
            Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt - 13  ' 10

        Else

            CurY = CurY + TxtHgt

            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_GSTIN_Cap), p1Font).Width
            strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_StateNm & "     " & Cmp_GSTIN_Cap & Cmp_GSTIN_No & Cmp_CstNo), pFont).Width
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
            Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No & "     " & Cmp_CstNo, CurX, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PhNo & "   " & Cmp_Email), LMargin, CurY, 2, PrintWidth, pFont)
            'CurY = CurY + TxtHgt - 1
            'Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, Cmp_Email, PageWidth - 10, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt - 1

        End If
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)
        Led_Name = "" : Led_Add1 = "" : Led_Add2 = "" : Led_Add3 = "" : Led_Add4 = "" : Led_GstNo = "" : Led_TinNo = ""

        If Trim(UCase(Common_Procedures.User.Type)) = "UNACCOUNT" And (Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1263") Then
            Led_Name = prn_HdDt.Rows(0).Item("Ledger_MainName").ToString : Led_Add1 = "" : Led_Add2 = "" : Led_Add3 = "" : Led_Add4 = "" : Led_GstNo = "" : Led_TinNo = ""
        Else
            Led_Name = prn_HdDt.Rows(0).Item("Ledger_MainName").ToString
            Led_Add1 = prn_HdDt.Rows(0).Item("Ledger_Address1").ToString
            Led_Add2 = prn_HdDt.Rows(0).Item("Ledger_Address2").ToString
            Led_Add3 = prn_HdDt.Rows(0).Item("Ledger_Address3").ToString '& "," & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString
            Led_Add4 = prn_HdDt.Rows(0).Item("Ledger_Address4").ToString

            If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
                Led_GstNo = "GSTIN : " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString
            End If
            If Trim(prn_HdDt.Rows(0).Item("pan_no").ToString) <> "" Then
                Led_TinNo = " PAN NO  " & Trim(prn_HdDt.Rows(0).Item("pan_no").ToString)
            End If
        End If
        p1Font = New Font("Calibri", 14, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "YARN DELIVERY NOTE", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        'CurY = CurY + TxtHgt

        CurY = CurY + strHeight + 5 ' + 150
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try

            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4)
            W1 = e.Graphics.MeasureString("BOOK NO  : ", pFont).Width
            K1 = e.Graphics.MeasureString("SIZING BOOK NO : ", pFont).Width
            S1 = e.Graphics.MeasureString("TO    :", pFont).Width

            CurY = CurY + TxtHgt - 5
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "TO  :  " & "M/s. " & Led_Name, LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "DC.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + K1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Yarn_Delivery_No").ToString, LMargin + C1 + K1 + 25, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 9, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, Led_Add1, LMargin + S1 + 10, CurY, 0, 0, p1Font)
            p1Font = New Font("Calibri", 14, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + K1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Yarn_Delivery_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + K1 + 25, CurY, 0, 0, pFont)
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1078" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1087" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1087" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1112" Then '---- Kalaimagal Sizing (Palladam)
                If Trim(prn_HdDt.Rows(0).Item("Entry_Time_Text").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, "TIME : " & Trim(prn_HdDt.Rows(0).Item("Entry_Time_Text").ToString), PageWidth - 10, CurY, 1, 0, pFont)
                End If
            End If

            'CurY = CurY + TxtHgt
            ' e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY + 30, PageWidth + 20, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY + 20, PageWidth, CurY + 20)
            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 10, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, Led_Add2, LMargin + S1 + 10, CurY, 0, 0, p1Font)
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1155" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1263" Then
                If Trim(prn_HdDt.Rows(0).Item("Vendor_Name").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, "DELIVERY TO ", LMargin + C1 + 10, CurY + 5, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + K1 + 10, CurY + 5, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vendor_Name").ToString, LMargin + C1 + K1 + 30, CurY + 5, 0, 0, pFont)
            End If

            Else
                If Trim(prn_HdDt.Rows(0).Item("DelName").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, "DELIVERY TO", LMargin + C1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + K1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DelName").ToString, LMargin + C1 + K1 + 30, CurY, 0, 0, pFont)
                End If
            End If
            'If Trim(prn_HdDt.Rows(0).Item("Book_No").ToString) <> "" Then
            '    Common_Procedures.Print_To_PrintDocument(e, "SIZING BOOK NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + K1 + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Book_No").ToString, LMargin + C1 + K1 + 30, CurY, 0, 0, pFont)
            'Else
            '    If Trim(prn_HdDt.Rows(0).Item("Delivery_At").ToString) <> "" Then
            '        Common_Procedures.Print_To_PrintDocument(e, "DELIVERY AT", LMargin + C1 + 10, CurY, 0, 0, pFont)
            '        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + K1 + 10, CurY, 0, 0, pFont)
            '        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Delivery_At").ToString, LMargin + C1 + K1 + 30, CurY, 0, 0, pFont)
            '    End If
            'End If

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Led_Add3, LMargin + S1 + 10, CurY, 0, 0, p1Font)

            If Trim(prn_HdDt.Rows(0).Item("Vendor_Address1").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vendor_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Vendor_Address2").ToString, LMargin + C1 + 10, CurY + 5, 0, 0, pFont)
            End If
                CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Led_Add4, LMargin + S1 + 10, CurY, 0, 0, p1Font)
            If Trim(prn_HdDt.Rows(0).Item("Vendor_Address3").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vendor_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Vendor_Address4").ToString, LMargin + C1 + 10, CurY + 5, 0, 0, pFont)
            End If

            'If Trim(prn_HdDt.Rows(0).Item("Textile_Dc_No").ToString) <> "" Then
            '    Common_Procedures.Print_To_PrintDocument(e, "TEXTILE DC NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + K1 + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Textile_Dc_No").ToString, LMargin + C1 + K1 + 30, CurY, 0, 0, pFont)
            'End If
            'If Common_Procedures.settings.CustomerCode = "1112" Then
            '    CurY = CurY + TxtHgt
            '    p1Font = New Font("Calibri", 11, FontStyle.Bold)
            '    Common_Procedures.Print_To_PrintDocument(e, "(For Jobwork Only, Not For Sale)", LMargin + C1 + 10, CurY, 0, 0, p1Font)
            'End If
            CurY = CurY + TxtHgt
                If Trim(prn_HdDt.Rows(0).Item("ledger_GSTinNo").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, Led_GstNo & Led_TinNo, LMargin + S1 + 10, CurY, 0, 0, pFont)
                End If
                If Trim(prn_HdDt.Rows(0).Item("GST_No").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "GSTIN : " & prn_HdDt.Rows(0).Item("GST_No").ToString & Led_TinNo, LMargin + C1 + 10, CurY + 5, 0, 0, pFont)
            End If


            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + C1, LnAr(3), LMargin + C1, LnAr(2))

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "TYPE", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "SET NO", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "MILL NAME", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "COUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "BAGS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "CONES", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format1_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim I As Integer
        Dim Cmp_Name As String
        Dim W1 As Single, C1 As Single, S1 As Single, K1 As Single
        Dim Del_Add1 As String = "", Del_Add2 As String = ""
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        Dim vTxamt As String = 0
        Dim vNtAMt As String = 0
        Dim vSgst_amt As String = 0
        Dim vCgst_amt As String = 0

        Try
            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4)
            W1 = e.Graphics.MeasureString("BOOK NO  : ", pFont).Width
            K1 = e.Graphics.MeasureString("SIZING BOOK NO : ", pFont).Width
            S1 = e.Graphics.MeasureString("TO    :", pFont).Width

            For I = NoofDets + 1 To NoofItems_PerPage
                CurY = CurY + TxtHgt
            Next

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

            CurY = CurY + TxtHgt - 10
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, " TOTAL", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)

                If Val(prn_HdDt.Rows(0).Item("Total_Bags").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Bags").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                End If
                If Val(prn_HdDt.Rows(0).Item("Total_Cones").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Cones").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                End If
                If Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString), "#########0.000"), PageWidth - 10, CurY, 1, 0, pFont)
                End If
            End If

            CurY = CurY + TxtHgt - 15

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))


            CurY = CurY + TxtHgt - 10

            Common_Procedures.Print_To_PrintDocument(e, "Through : " & Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString), LMargin + 10, CurY, 0, 0, pFont)

            'Common_Procedures.Print_To_PrintDocument(e, "VENDOR NAME", LMargin + C1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + K1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vendor_Name").ToString, LMargin + C1 + K1 + 25, CurY, 0, 0, pFont)
            ' CurY = CurY + TxtHgt
            'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1155" Then
            '    p1Font = New Font("Calibri", 9, FontStyle.Bold)
            '    If Common_Procedures.Vendor_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("Vendor_IdNo").ToString)) <> "" Then

            '        da1 = New SqlClient.SqlDataAdapter("select a.* from Vendor_head a where  a.Vendor_IdNO = " & Val(prn_HdDt.Rows(0).Item("Vendor_IdNo").ToString) & " ", con)
            '        dt = New DataTable
            '        da1.Fill(dt)

            '        If dt.Rows.Count > 0 Then

            '            Del_Add1 = dt.Rows(0).Item("Vendor_Address1").ToString & "," & dt.Rows(0).Item("Vendor_Address2").ToString
            '            Del_Add2 = dt.Rows(0).Item("Vendor_Address3").ToString & "," & dt.Rows(0).Item("Vendor_Address4").ToString

            '        End If
            '        Common_Procedures.Print_To_PrintDocument(e, "DELIVERY TO   : " & Common_Procedures.Vendor_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("Vendor_IdNo").ToString)), LMargin + 10, CurY, 0, 0, p1Font)
            '        CurY = CurY + TxtHgt
            '        Common_Procedures.Print_To_PrintDocument(e, " " & Trim(Del_Add1), LMargin + 30, CurY, 0, 0, pFont)
            '        CurY = CurY + TxtHgt
            '        Common_Procedures.Print_To_PrintDocument(e, " " & Trim(Del_Add2), LMargin + 30, CurY, 0, 0, pFont)
            '    End If
            'End If

            If Val(prn_HdDt.Rows(0).Item("Kg_Rate").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, " Rate/Kg : " & Trim(prn_HdDt.Rows(0).Item("Kg_Rate").ToString), PageWidth - 510, CurY, 0, 0, pFont)
            End If

            If Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, " Empty Beam : " & Trim(prn_HdDt.Rows(0).Item("Empty_Beam").ToString), PageWidth - 400, CurY, 0, 0, pFont)
            End If

            If Val(prn_HdDt.Rows(0).Item("Empty_Bags").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, " Empty Bags : " & Trim(prn_HdDt.Rows(0).Item("Empty_Bags").ToString), PageWidth - 280, CurY, 0, 0, pFont)
            End If

            If Val(prn_HdDt.Rows(0).Item("Empty_Cones").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, " Empty Cones : " & Trim(prn_HdDt.Rows(0).Item("Empty_Cones").ToString), PageWidth - 150, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt

            If Val(prn_HdDt.Rows(0).Item("approx_Value").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, " Taxable Amount : " & Trim(prn_HdDt.Rows(0).Item("approx_Value").ToString), LMargin + 10, CurY, 0, 0, pFont)
            End If

            If Val(prn_HdDt.Rows(0).Item("approx_Value").ToString) <> 0 Then

                vCgst_amt = Format((Val(prn_HdDt.Rows(0).Item("approx_Value").ToString) * 2.5 / 100), "############0")
                vSgst_amt = Format((Val(prn_HdDt.Rows(0).Item("approx_Value").ToString) * 2.5 / 100), "############0")


                Common_Procedures.Print_To_PrintDocument(e, " CGST 2.5 % : " & vCgst_amt, PageWidth - 560, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " SGST 2.5 % : " & vSgst_amt, PageWidth - 420, CurY, 0, 0, pFont)
            End If

            If Val(prn_HdDt.Rows(0).Item("approx_Value").ToString) <> 0 Then
                vTxamt = Val(vCgst_amt) + Val(vSgst_amt) 'Format((Val(prn_HdDt.Rows(0).Item("approx_Value").ToString) * 5 / 100), "############0.00")
                Common_Procedures.Print_To_PrintDocument(e, " Tax Amount : " & vTxamt, PageWidth - 280, CurY, 0, 0, pFont)
            End If

            If Val(vTxamt) <> 0 Then
                vNtAMt = Format(Val(prn_HdDt.Rows(0).Item("approx_Value").ToString) + vTxamt, "###########0.00")
                Common_Procedures.Print_To_PrintDocument(e, " Net Amount : " & vNtAMt, PageWidth - 150, CurY, 0, 0, pFont)
            End If



            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(7) = CurY

            CurY = CurY + TxtHgt
            If Val(Common_Procedures.User.IdNo) <> 1 Then
                Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
            End If
            CurY = CurY + TxtHgt

            If Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString) <> "" Then
                Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
                Cmp_Name = Replace(Cmp_Name, Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString), "")

            Else
                Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString

            End If

            Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 20, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 250, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, LMargin + PageWidth - 75, CurY, 1, 0, p1Font)

            'e.Graphics.DrawLine(Pens.Black, PageWidth, CurY, PageWidth, LnAr(1))

            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub
    Private Sub Printing_Format2(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim pFont As Font, pFont1 As Font, p1Font As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurX As Single = 0
        Dim CurY As Single, TxtHgt As Single
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ps As Printing.PaperSize
        Dim NoofItems_PerPage As Integer
        Dim AmtInWrds As String = ""
        Dim PrnHeading As String = ""
        Dim ItmNm1 As String, ItmNm2 As String
        Dim I As Integer, NoofDets As Integer
        Dim time As String = ""

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                'PageSetupDialog1.PageSettings.PaperSize = ps
                Exit For
            End If
        Next

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 0 ' 65
            .Right = 0 ' 50
            .Top = 25 ' 65
            .Bottom = 0 ' 50
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pFont = New Font("Calibri", 11, FontStyle.Regular)
        pFont1 = New Font("Calibri", 8, FontStyle.Regular)

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

        TxtHgt = 20 ' e.Graphics.MeasureString("A", pFont).Height  ' 20
        NoofItems_PerPage = 5

        Try

            'For I = 100 To 1100 Step 300

            '    CurY = I
            '    For J = 1 To 850 Step 40

            '        CurX = J
            '        Common_Procedures.Print_To_PrintDocument(e, CurX, CurX, CurY - 20, 0, 0, pFont1)
            '        Common_Procedures.Print_To_PrintDocument(e, "|", CurX, CurY, 0, 0, pFont)

            '        CurX = J + 20
            '        Common_Procedures.Print_To_PrintDocument(e, "|", CurX, CurY, 0, 0, pFont)
            '        Common_Procedures.Print_To_PrintDocument(e, "|", CurX, CurY + 20, 0, 0, pFont)
            '        Common_Procedures.Print_To_PrintDocument(e, CurX, CurX, CurY + 40, 0, 0, pFont1)

            '    Next

            'Next

            'For I = 200 To 800 Step 250

            '    CurX = I
            '    For J = 1 To 1200 Step 40

            '        CurY = J
            '        Common_Procedures.Print_To_PrintDocument(e, "-", CurX, CurY, 0, 0, pFont)
            '        Common_Procedures.Print_To_PrintDocument(e, "   " & CurY, CurX, CurY, 0, 0, pFont1)

            '        CurY = J + 20
            '        Common_Procedures.Print_To_PrintDocument(e, "--", CurX, CurY, 0, 0, pFont)
            '        Common_Procedures.Print_To_PrintDocument(e, "   " & CurY, CurX, CurY, 0, 0, pFont1)

            '    Next

            'Next

            'e.HasMorePages = False

            CurX = LMargin + 340
            CurY = TMargin + 100
            p1Font = New Font("Calibri", 14, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "YARN DELIVERY NOTE", CurX, CurY, 0, 0, p1Font)


            CurX = LMargin + 80
            CurY = TMargin + 140
            p1Font = New Font("Calibri", 14, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "NO : " & prn_HdDt.Rows(0).Item("Yarn_Delivery_No").ToString, CurX, CurY, 0, 0, p1Font)

            CurX = LMargin + 340
            Common_Procedures.Print_To_PrintDocument(e, "DATE : " & Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Yarn_Delivery_Date").ToString), "dd-MM-yyyy"), CurX, CurY, 0, 0, p1Font)

            'time = System.DateTime.Now

            'CurX = LMargin + 580
            'Common_Procedures.Print_To_PrintDocument(e, "TIME : " & Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Yarn_Receipt_Date").ToString), "h:mm:ss tt"), CurX, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            CurX = LMargin + 30
            e.Graphics.DrawLine(Pens.Black, CurX, CurY, CurX + 730, CurY)

            CurX = LMargin + 65 ' 40  '150
            CurY = TMargin + 180 ' 122 ' 100
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "To M/s. " & prn_HdDt.Rows(0).Item("Ledger_MainName").ToString, CurX, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, CurX + 20, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("Ledger_Address2").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, CurX + 20, CurY, 0, 0, pFont)
            End If

            CurX = LMargin + 300 ' 40  '150
            CurY = TMargin + 240 ' 122 ' 100
            Common_Procedures.Print_To_PrintDocument(e, "We have delivered the following", CurX, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            CurX = LMargin + 30
            e.Graphics.DrawLine(Pens.Black, CurX, CurY, CurX + 730, CurY)

            CurX = LMargin + 65 ' 40  '150
            CurY = TMargin + 265 ' 122 ' 100
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Count", CurX, CurY, 0, 0, p1Font)

            CurX = LMargin + 180 ' 40  '150
            Common_Procedures.Print_To_PrintDocument(e, "Name of the Mill ", CurX, CurY, 0, 0, p1Font)

            CurX = LMargin + 440 ' 40  '150
            Common_Procedures.Print_To_PrintDocument(e, "Bags", CurX, CurY, 0, 0, p1Font)

            CurX = LMargin + 565 ' 40  '150
            Common_Procedures.Print_To_PrintDocument(e, "Cones", CurX, CurY, 0, 0, p1Font)

            CurX = LMargin + 675 ' 40  '150
            Common_Procedures.Print_To_PrintDocument(e, "Weight", CurX, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            CurX = LMargin + 30
            e.Graphics.DrawLine(Pens.Black, CurX, CurY, CurX + 730, CurY)

            Try

                NoofDets = 0

                CurY = 300 ' 370

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1


                        ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Mill_Name").ToString)
                        ItmNm2 = ""
                        If Len(ItmNm1) > 30 Then
                            For I = 6 To 1 Step -1
                                If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 30
                            ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                            ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                        End If


                        CurY = CurY + TxtHgt

                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Count_Name").ToString), LMargin + 65, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + 185, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString), LMargin + 550, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Cones").ToString), "########0"), LMargin + 660, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Weight").ToString), "########0.000"), LMargin + 780, CurY, 1, 0, pFont)

                        NoofDets = NoofDets + 1

                        If Trim(ItmNm2) <> "" Or Trim(ItmNm2) <> "" Then
                            CurY = CurY + TxtHgt
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), 115, CurY, 0, 0, pFont)


                            NoofDets = NoofDets + 1
                        End If

                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If

            Catch ex As Exception

                MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End Try

            For I = NoofDets + 1 To NoofItems_PerPage
                CurY = CurY + TxtHgt
            Next

            CurY = TMargin + 390
            e.Graphics.DrawLine(Pens.Black, LMargin + 30, CurY, LMargin + 790, CurY)

            CurX = LMargin + 200
            CurY = TMargin + 400
            Common_Procedures.Print_To_PrintDocument(e, "TOTAL", CurX, CurY, 0, 0, pFont)

            CurX = LMargin + 550
            CurY = TMargin + 400

            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Bags").ToString), "########0"), CurX, CurY, 1, 0, pFont)

            CurX = LMargin + 660

            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Cones").ToString), "########0"), CurX, CurY, 1, 0, pFont)
            CurX = LMargin + 780
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString), "########0.000"), CurX, CurY, 1, 0, pFont)

            'CurY = TMargin + 440
            'e.Graphics.DrawLine(Pens.Black, LMargin + 60, CurY, LMargin + 790, CurY)

            CurY = TMargin + 440
            e.Graphics.DrawLine(Pens.Black, LMargin + 170, CurY, LMargin + 170, TMargin + 260)
            e.Graphics.DrawLine(Pens.Black, LMargin + 430, CurY, LMargin + 430, TMargin + 260)
            e.Graphics.DrawLine(Pens.Black, LMargin + 560, CurY, LMargin + 560, TMargin + 260)
            e.Graphics.DrawLine(Pens.Black, LMargin + 670, CurY, LMargin + 670, TMargin + 260)

            CurX = LMargin + 200
            CurY = TMargin + 450
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString) & "    Duplicate for Book No . B1", CurX, CurY, 0, 0, pFont)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub dgv_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.LostFocus
        On Error Resume Next
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
    End Sub

    Private Sub btn_Print_Cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_Cancel.Click
        btn_print_Close_Click(sender, e)
    End Sub

    Private Sub btn_print_Close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Print.Click
        pnl_Back.Enabled = True
        pnl_Print.Visible = False
    End Sub

    Private Sub btn_Print_Invoice_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_Invoice.Click
        prn_Status = 1
        printing_invoice()
        btn_print_Close_Click(sender, e)
    End Sub

    Private Sub btn_Print_Preprint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_Preprint.Click
        prn_Status = 2
        printing_invoice()
        btn_print_Close_Click(sender, e)
    End Sub




    Private Sub cbo_bagType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_bagType.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_bagType, txt_TexDcNo, cbo_coneType, "Bag_Type_Head", "Bag_Type_Name", "", "(Bag_Type_Idno = 0)")

    End Sub

    Private Sub cbo_bagType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_bagType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_bagType, cbo_coneType, "Bag_Type_Head", "Bag_Type_Name", "", "(Bag_Type_Idno = 0)")

    End Sub

    Private Sub cbo_bagType_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_bagType.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New Bag_Type_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_bagType.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub
    Private Sub cbo_coneType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_coneType.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_coneType, dtp_Time, Nothing, "Cone_Type_Head", "Cone_Type_Name", "", "(Cone_Type_Idno = 0)")
        If e.KeyCode = 40 And cbo_coneType.DroppedDown = False Or (e.Control = True And e.KeyCode = 40) Then
            If cbo_Godown.Visible = True Then
                cbo_Godown.Focus()
            Else
                txt_ElectronicRefNo.Focus()
            End If
        End If
        If e.KeyCode = 38 And cbo_coneType.DroppedDown = False Or (e.Control = True And e.KeyCode = 38) Then
            txt_TexDcNo.Focus()
        End If
    End Sub

    Private Sub cbo_coneType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_coneType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_coneType, Nothing, "Cone_Type_Head", "Cone_Type_Name", "", "(Cone_Type_Idno = 0)")
        If Asc(e.KeyChar) = 13 Then
            If cbo_Godown.Visible = True Then
                cbo_Godown.Focus()
            Else
                txt_ElectronicRefNo.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_coneType_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_coneType.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New ConeType_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_coneType.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub
    Private Sub Printing_Format3_DosPrint()
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim NewCode As String = ""
        Dim PrnTxt As String = ""
        Dim LnCnt As Integer = 0

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.*, d.Beam_Width_Name from SizingSoft_Yarn_Delivery_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo  LEFT OUTER JOIN Beam_Width_Head d ON a.Beam_Width_IdNo = d.Beam_Width_IdNo Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Yarn_Delivery_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Mill_Name, c.Count_Name  from SizingSoft_Yarn_Delivery_Details a INNER JOIN Mill_Head b on a.Mill_IdNo = b.Mill_IdNo INNER JOIN Count_Head c on a.Count_IdNo = c.Count_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Yarn_Delivery_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                prn_DetDt = New DataTable
                da2.Fill(prn_DetDt)

            Else
                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If

            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try


        Get_DosLineDetails()

        LnCnt = 0

        fs = New FileStream(Common_Procedures.Dos_Printing_FileName_Path, FileMode.Create)
        sw = New StreamWriter(fs, System.Text.Encoding.Default)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format3_DosPrint_PageHeader(LnCnt)

                prn_DetIndx = 0
                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        PrnTxt = Chr(Vz1) & Space(1) & Trim(prn_DetDt.Rows(prn_DetIndx).Item("Sl_No").ToString) & Space(5 - Len(Trim(Val(prn_DetDt.Rows(prn_DetIndx).Item("Sl_No").ToString)))) & Chr(Vz2) & Space(1) & Trim(prn_DetDt.Rows(prn_DetIndx).Item("Yarn_Type").ToString) & Space(7 - Len(Trim(prn_DetDt.Rows(prn_DetIndx).Item("Yarn_Type").ToString))) & Chr(Vz2) & Space(1) & Trim(Microsoft.VisualBasic.Left(prn_DetDt.Rows(prn_DetIndx).Item("Mill_Name").ToString, 22)) & Space(22 - Len(Microsoft.VisualBasic.Left(Trim(prn_DetDt.Rows(prn_DetIndx).Item("Mill_Name").ToString), 22))) & Chr(Vz2) & Space(1) & Trim(Microsoft.VisualBasic.Left(prn_DetDt.Rows(prn_DetIndx).Item("Count_Name").ToString, 6)) & Space(6 - Len(Microsoft.VisualBasic.Left(Trim(prn_DetDt.Rows(prn_DetIndx).Item("Count_Name").ToString), 6))) & Chr(Vz2) & Space(7 - Len(Trim(Val(prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString)))) & Val(prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString) & Space(1) & Chr(Vz2) & Space(8 - Len(Trim(Val(prn_DetDt.Rows(prn_DetIndx).Item("Cones").ToString)))) & Val(prn_DetDt.Rows(prn_DetIndx).Item("Cones").ToString) & Space(1) & Chr(Vz2) & Space(10 - Len(Trim(Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Weight").ToString), "########0.000")))) & Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Weight").ToString), "########0.000") & Space(1) & Chr(Vz1)
                        sw.WriteLine(PrnTxt)

                        LnCnt = LnCnt + 1

                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If

                Printing_Format3_DosPrint_PageFooter(LnCnt)

                'w.Close()
                'w.Dispose()

                If Val(Common_Procedures.Print_OR_Preview_Status) = 2 Then
                    Dim p1 As New System.Diagnostics.Process
                    p1.EnableRaisingEvents = False
                    p1.StartInfo.FileName = Common_Procedures.Dos_PrintPreView_BatchFileName_Path
                    p1.StartInfo.WindowStyle = ProcessWindowStyle.Maximized
                    p1.Start()
                Else
                    Dim p2 As New System.Diagnostics.Process
                    p2.EnableRaisingEvents = False
                    p2.StartInfo.FileName = Common_Procedures.Dos_Print_BatchFileName_Path
                    p2.StartInfo.CreateNoWindow = True
                    p2.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
                    p2.Start()
                End If

            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            sw.Close()
            fs.Close()
            sw.Dispose()
            fs.Dispose()


        End Try

    End Sub

    Public Sub Printing_Format3_DosPrint_PageHeader(ByRef LnCnt As Integer)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String, Cmp_Add3 As String, Cmp_Add4 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
        Dim PrnTxt As String = ""

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = "" : Cmp_Add3 = "" : Cmp_Add4 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = ""

        Try

            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString
            Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address2").ToString
            Cmp_Add3 = prn_HdDt.Rows(0).Item("Company_Address3").ToString
            Cmp_Add4 = prn_HdDt.Rows(0).Item("Company_Address4").ToString

            If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
                Cmp_PhNo = prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
            End If
            If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
                Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
            End If
            If Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString) <> "" Then
                Cmp_CstNo = "CST NO.: " & prn_HdDt.Rows(0).Item("Company_CstNo").ToString
            End If

            PrnTxt = Chr(27) & "@" & Chr(18) & Chr(27) & "P" & Chr(27) & "t1" & Chr(27) & "2" & Chr(27) & "x0"
            sw.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1

            PrnTxt = Trim(ChrW(Corn1)) & StrDup(78, Chr(Hz1)) & ChrW(Corn2)
            sw.WriteLine(PrnTxt)
            'PrnTxt = Chr(Corn1) & StrDup(78, Chr(Hz1)) & Chr(Corn2)
            'sw.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1

            PrnTxt = Chr(Vz1) & Space(39 - Len(Trim(Cmp_Name))) & Chr(14) & Chr(27) & "E" & Trim(Cmp_Name) & Chr(27) & "F" & Chr(20) & Space(39 - Len(Trim(Cmp_Name))) & Chr(Vz1)
            sw.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1

            If Len(Trim(Cmp_Add1 & " " & Cmp_Add2)) > 78 Then
                PrnTxt = Chr(Vz1) & Space(78) & Chr(Vz1) & Chr(13) & Space(2) & Chr(15) & Space(65 - ((Len((Cmp_Add1) & " " & (Cmp_Add2)) / 2) + 0.1)) & Trim(Cmp_Add1 & " " & Cmp_Add2) & Chr(18)
                sw.WriteLine(PrnTxt)
                LnCnt = LnCnt + 1
                PrnTxt = Chr(Vz1) & Space(78) & Chr(Vz1) & Chr(13) & Space(2) & Chr(15) & Space(65 - ((Len((Cmp_Add3) & " " & (Cmp_Add4)) / 2) + 0.1)) & Trim(Cmp_Add3 & " " & Cmp_Add4) & Chr(18)
                sw.WriteLine(PrnTxt)
                LnCnt = LnCnt + 1

            Else
                PrnTxt = Chr(Vz1) & Space(39 - ((Len((Cmp_Add1) & " " & (Cmp_Add2)) / 2) + 0.1)) & Trim(Cmp_Add1 & " " & Cmp_Add2) & Space(39 - ((Len(Cmp_Add1 & " " & Cmp_Add2) / 2) + 0.1)) & Space(Len(Cmp_Add1 & " " & Cmp_Add2) Mod 2) & Chr(Vz1)
                sw.WriteLine(PrnTxt)
                LnCnt = LnCnt + 1
                PrnTxt = Chr(Vz1) & Space(39 - ((Len((Cmp_Add3) & " " & (Cmp_Add4)) / 2) + 0.1)) & Trim(Cmp_Add3 & " " & Cmp_Add4) & Space(39 - ((Len(Cmp_Add3 & " " & Cmp_Add4) / 2) + 0.1)) & Space(Len(Cmp_Add3 & " " & Cmp_Add4) Mod 2) & Chr(Vz1)
                sw.WriteLine(PrnTxt)
                LnCnt = LnCnt + 1

            End If

            PrnTxt = Chr(Vz1) & Space(35 - Math.Round((Len(Cmp_PhNo) / 2) + 0.1)) & "Phone : " & Trim(Cmp_PhNo) & Space(35 - Math.Round((Len(Cmp_PhNo) / 2) + 0.1)) & Space(Len(Cmp_PhNo) Mod 2) & Chr(Vz1)
            sw.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1

            PrnTxt = Chr(Vz1) & Space(78) & Chr(Vz1)
            sw.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1

            PrnTxt = Chr(Vz1) & Space(22) & Chr(14) & Chr(27) & "E" & "YARN DELIVERY NOTE" & Chr(27) & "F" & Chr(20) & Space(22) & Chr(Vz1)
            sw.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1

            PrnTxt = Chr(LfCon) & StrDup(39, Chr(Hz2)) & Chr(194) & StrDup(38, Chr(Hz2)) & Chr(RgtCon)
            sw.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1

            PrnTxt = Chr(Vz1) & Space(1) & "From : " & Space(31) & Chr(Vz2) & Space(38) & Chr(Vz1)
            sw.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1
            PrnTxt = Chr(Vz1) & Space(4) & "M/s." & Trim(prn_HdDt.Rows(0).Item("Ledger_MainName").ToString) & Space(31 - Len(Trim(prn_HdDt.Rows(0).Item("Ledger_MainName").ToString))) & Chr(Vz2) & Space(1) & "DC NO  : " & Trim(prn_HdDt.Rows(0).Item("Yarn_Delivery_No").ToString) & Space(28 - Len(Trim(prn_HdDt.Rows(0).Item("Yarn_Delivery_No").ToString))) & Chr(Vz1)
            sw.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1
            PrnTxt = Chr(Vz1) & Space(4) & Trim(prn_HdDt.Rows(0).Item("Ledger_Address1").ToString) & Space(35 - Len(Trim(prn_HdDt.Rows(0).Item("Ledger_Address1").ToString))) & Chr(Vz2) & Space(38) & Chr(Vz1)
            sw.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1
            PrnTxt = Chr(Vz1) & Space(4) & Trim(prn_HdDt.Rows(0).Item("Ledger_Address2").ToString) & Space(35 - Len(Trim(prn_HdDt.Rows(0).Item("Ledger_Address2").ToString))) & Chr(Vz2) & Space(1) & "DATE   : " & Trim(Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Yarn_Delivery_Date").ToString), "dd-MM-yyyy").ToString) & Space(28 - Len(Trim(Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Yarn_Delivery_Date").ToString), "dd-MM-yyyy").ToString))) & Chr(Vz1)
            sw.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1
            PrnTxt = Chr(Vz1) & Space(4) & Trim(prn_HdDt.Rows(0).Item("Ledger_Address3").ToString) & Space(35 - Len(Trim(prn_HdDt.Rows(0).Item("Ledger_Address3").ToString))) & Chr(Vz2) & Space(38) & Chr(Vz1)
            sw.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1
            PrnTxt = Chr(Vz1) & Space(4) & Trim(prn_HdDt.Rows(0).Item("Ledger_Address4").ToString) & Space(35 - Len(Trim(prn_HdDt.Rows(0).Item("Ledger_Address4").ToString))) & Chr(Vz2) & Space(38) & Chr(Vz1) '  & "PARTY DC.NO : " & Trim(prn_HdDt.Rows(0).Item("Book_No").ToString) & Space(23 - Len(Trim(prn_HdDt.Rows(0).Item("Book_No").ToString))) & Chr(Vz1)
            sw.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1
            'SUB HEADING
            PrnTxt = Chr(LfCon) & StrDup(6, Chr(Hz2)) & Chr(194) & StrDup(8, Chr(Hz2)) & Chr(194) & StrDup(23, Chr(Hz2)) & Chr(197) & StrDup(7, Chr(Hz2)) & Chr(194) & StrDup(8, Chr(Hz2)) & Chr(194) & StrDup(9, Chr(Hz2)) & Chr(194) & StrDup(11, Chr(Hz2)) & Chr(RgtCon)
            sw.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1

            PrnTxt = Chr(Vz1) & " S.No " & Chr(Vz2) & "  TYPE  " & Chr(Vz2) & "       MILL NAME       " & Chr(Vz2) & " COUNT " & Chr(Vz2) & "  BAGS  " & Chr(Vz2) & "  CONES  " & Chr(Vz2) & "   WEIGHT  " & Chr(Vz1)
            sw.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1
            PrnTxt = Chr(LfCon) & StrDup(6, Chr(Hz2)) & Chr(197) & StrDup(8, Chr(Hz2)) & Chr(197) & StrDup(23, Chr(Hz2)) & Chr(197) & StrDup(7, Chr(Hz2)) & Chr(197) & StrDup(8, Chr(Hz2)) & Chr(197) & StrDup(9, Chr(Hz2)) & Chr(197) & StrDup(11, Chr(Hz2)) & Chr(RgtCon)
            sw.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1

        Catch ex As Exception
            sw.Close()
            sw.Dispose()
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format3_DosPrint_PageFooter(ByRef LnCnt As Integer)
        Dim EBm_Txt As String = ""
        Dim EBm_Wdth As String = ""
        Dim Cmp_Name As String = ""
        Dim PrnTxt As String = ""

        Try

            For J = prn_DetIndx + 1 To 5
                PrnTxt = Chr(Vz1) & Space(6) & Chr(Vz2) & Space(8) & Chr(Vz2) & Space(23) & Chr(Vz2) & Space(7) & Chr(Vz2) & Space(8) & Chr(Vz2) & Space(9) & Chr(Vz2) & Space(11) & Chr(Vz1)
                sw.WriteLine(PrnTxt)
                LnCnt = LnCnt + 1
            Next J


            PrnTxt = Chr(LfCon) & StrDup(6, Chr(Hz2)) & Chr(197) & StrDup(8, Chr(Hz2)) & Chr(197) & StrDup(23, Chr(Hz2)) & Chr(197) & StrDup(7, Chr(Hz2)) & Chr(197) & StrDup(8, Chr(Hz2)) & Chr(197) & StrDup(9, Chr(Hz2)) & Chr(197) & StrDup(11, Chr(Hz2)) & Chr(RgtCon)
            sw.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1

            PrnTxt = Chr(Vz1) & Space(6) & Chr(Vz2) & Space(8) & Chr(Vz2) & " TOTAL" & Space(17) & Chr(Vz2) & Space(7) & Chr(Vz2) & Space(7 - Len(Trim(prn_HdDt.Rows(0).Item("Total_Bags").ToString))) & Val(prn_HdDt.Rows(0).Item("Total_Bags").ToString) & Space(1) & Chr(Vz2) & Space(8 - Len(Trim(prn_HdDt.Rows(0).Item("Total_Cones").ToString))) & Val(prn_HdDt.Rows(0).Item("Total_Cones").ToString) & Space(1) & Chr(Vz2) & Space(10 - Len(Trim(Format(Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString), "#########0.000")))) & Format(Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString), "#########0.000") & Space(1) & Chr(Vz1)
            sw.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1

            PrnTxt = Chr(LfCon) & StrDup(6, Chr(Hz2)) & Chr(193) & StrDup(8, Chr(Hz2)) & Chr(193) & StrDup(23, Chr(Hz2)) & Chr(193) & StrDup(7, Chr(Hz2)) & Chr(193) & StrDup(8, Chr(Hz2)) & Chr(193) & StrDup(9, Chr(Hz2)) & Chr(193) & StrDup(11, Chr(Hz2)) & Chr(RgtCon)
            sw.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1

            EBm_Txt = "" : EBm_Wdth = ""
            If Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString) <> 0 Then EBm_Txt = "EMPTY BEAM : " & Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString)
            If Trim(prn_HdDt.Rows(0).Item("Beam_Width_Name").ToString) <> "" Then EBm_Wdth = "BEAM WIDTH  : " & Trim(prn_HdDt.Rows(0).Item("Beam_Width_Name").ToString)

            PrnTxt = Chr(Vz1) & Space(1) & "Through     : " & Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString) & Space(36 - Len(Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString))) & Space(2) & Trim(EBm_Txt) & Space(25 - Len(Trim(EBm_Txt))) & Chr(Vz1)
            sw.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1

            PrnTxt = Chr(Vz1) & Space(1) & Trim(EBm_Wdth) & Space(75 - Len(Trim(EBm_Wdth))) & Space(2) & Chr(Vz1)
            sw.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1

            PrnTxt = Chr(LfCon) & StrDup(78, Chr(Hz2)) & Chr(RgtCon)
            sw.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1

            PrnTxt = Chr(Vz1) & Space(78) & Chr(Vz1)
            sw.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1

            PrnTxt = Chr(Vz1) & Space(78) & Chr(Vz1)
            sw.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1

            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString

            PrnTxt = Chr(Vz1) & " Receiver Signature   " & "Prepared By " & Space(39 - Len(Microsoft.VisualBasic.Left(Trim(Cmp_Name), 39))) & "For " & Cmp_Name & Space(1) & Chr(Vz1)
            sw.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1

            PrnTxt = Chr(Corn3) & StrDup(78, Chr(Hz1)) & Chr(Corn4)
            sw.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1

            For I = LnCnt + 1 To 36
                PrnTxt = ""
                sw.WriteLine(PrnTxt)
                LnCnt = LnCnt + 1
            Next

        Catch ex As Exception
            sw.Close()
            sw.Dispose()
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub btn_Close_DosPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_DosPrint.Click
        pnl_Back.Enabled = True
        Pnl_DosPrint.Visible = False
    End Sub

    Private Sub Btn_DosCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Btn_DosCancel.Click
        btn_Close_DosPrint_Click(sender, e)
    End Sub

    Private Sub Btn_DosPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Btn_DosPrint.Click
        prn_Status = 1
        Printing_Format3_DosPrint()
        btn_Close_DosPrint_Click(sender, e)
    End Sub

    Private Sub Btn_LaserPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Btn_LaserPrint.Click
        prn_Status = 2
        printing_invoice()
        btn_Close_DosPrint_Click(sender, e)
    End Sub
    Private Sub Get_DosLineDetails()
        Hz1 = Common_Procedures.Dos_DottedLines.Hz1
        Hz2 = Common_Procedures.Dos_DottedLines.Hz2
        Vz1 = Common_Procedures.Dos_DottedLines.Vz1
        Vz2 = Common_Procedures.Dos_DottedLines.Vz2
        Corn1 = Common_Procedures.Dos_DottedLines.Corn1
        Corn2 = Common_Procedures.Dos_DottedLines.Corn2
        Corn3 = Common_Procedures.Dos_DottedLines.Corn3
        Corn4 = Common_Procedures.Dos_DottedLines.Corn4
        LfCon = Common_Procedures.Dos_DottedLines.LfCon
        RgtCon = Common_Procedures.Dos_DottedLines.RgtCon
    End Sub

    Private Sub btn_Print_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        print_record()
    End Sub
    Private Sub Printing_Format4(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim EntryCode As String
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ItmNm1 As String, ItmNm2 As String
        'Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim PS As Printing.PaperSize
        Dim PCnt As Integer = 0, PrntCnt As Integer = 0
        Dim TpMargin As Single = 0


        PrntCnt = 1

        If Val(Common_Procedures.settings.YarnDelivery_Print_2Copy_In_SinglePage) = 1 Then
            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                    PS = PrintDocument1.PrinterSettings.PaperSizes(I)
                    PrintDocument1.DefaultPageSettings.PaperSize = PS
                    e.PageSettings.PaperSize = PS
                    Exit For
                End If
            Next
        Else

            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                PS = PrintDocument1.PrinterSettings.PaperSizes(I)
                Debug.Print(PS.PaperName)
                If PS.Width = 800 And PS.Height = 600 Then
                    PrintDocument1.DefaultPageSettings.PaperSize = PS
                    e.PageSettings.PaperSize = PS
                    PpSzSTS = True
                    Exit For
                End If
            Next
            If PpSzSTS = False Then
                For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
                        PS = PrintDocument1.PrinterSettings.PaperSizes(I)
                        PrintDocument1.DefaultPageSettings.PaperSize = PS
                        e.PageSettings.PaperSize = PS
                        PpSzSTS = True
                        Exit For
                    End If
                Next

                If PpSzSTS = False Then
                    For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                        If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                            PS = PrintDocument1.PrinterSettings.PaperSizes(I)
                            PrintDocument1.DefaultPageSettings.PaperSize = PS
                            e.PageSettings.PaperSize = PS
                            Exit For
                        End If
                    Next
                End If

            End If
        End If


        If PrntCnt2ndPageSTS = False Then
            PrntCnt = 2
        End If

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 10
            .Right = 50
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
        If PrintDocument1.DefaultPageSettings.Landscape = True Then
            With PrintDocument1.DefaultPageSettings.PaperSize
                PrintWidth = .Height - TMargin - BMargin
                PrintHeight = .Width - RMargin - LMargin
                PageWidth = .Height - TMargin
                PageHeight = .Width - RMargin
            End With
        End If



        NoofItems_PerPage = 5 ' 6 ' 5

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = 40
        ClArr(2) = 60 : ClArr(3) = 80 : ClArr(4) = 210 : ClArr(5) = 100 : ClArr(6) = 70 : ClArr(7) = 70
        ClArr(8) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7))

        TxtHgt = 18.5 ' 18.8  ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        PrntCnt2ndPageSTS = False
        TpMargin = TMargin

        For PCnt = 1 To PrntCnt
            If Val(Common_Procedures.settings.YarnDelivery_Print_2Copy_In_SinglePage) = 1 Then
                If PCnt = 1 Then
                    prn_PageNo = 0

                    prn_DetIndx = 0
                    prn_DetSNo = 0
                    ' prn_NoofBmDets = 0
                    TpMargin = TMargin

                Else

                    prn_PageNo = 0
                    ' prn_NoofBmDets = 0
                    prn_DetIndx = 0
                    prn_DetSNo = 0

                    TpMargin = 560 + TMargin  ' 600 + TMargin

                End If
            End If

            Try

                If prn_HdDt.Rows.Count > 0 Then

                    Printing_Format4_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TpMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                    Try

                        NoofDets = 0

                        CurY = CurY - 10

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

                                ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Mill_Name").ToString)
                                ItmNm2 = ""
                                If Len(ItmNm1) > 18 Then
                                    For I = 18 To 1 Step -1
                                        If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                                    Next I
                                    If I = 0 Then I = 18
                                    ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                                    ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                                End If


                                CurY = CurY + TxtHgt

                                Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Sl_No").ToString), LMargin + 25, CurY, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Yarn_Type").ToString, LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                                If IsDBNull(prn_DetDt.Rows(prn_DetIndx).Item("Set_no").ToString) = False Then
                                    If Trim(prn_DetDt.Rows(prn_DetIndx).Item("Set_no").ToString) <> "" Then
                                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Set_no").ToString), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                                    End If
                                End If
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Count_Name").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 10, CurY, 0, 0, pFont)
                                If Val(prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString) <> 0 Then
                                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)
                                End If
                                If Val(prn_DetDt.Rows(prn_DetIndx).Item("Cones").ToString) <> 0 Then
                                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Cones").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
                                End If
                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Weight").ToString), "########0.000"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)

                                NoofDets = NoofDets + 1

                                If Trim(ItmNm2) <> "" Then
                                    CurY = CurY + TxtHgt - 5
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)
                                    NoofDets = NoofDets + 1
                                End If

                                prn_DetIndx = prn_DetIndx + 1

                            Loop

                        End If

                        Printing_Format4_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)

                    Catch ex As Exception

                        MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                    End Try

                End If

            Catch ex As Exception

                MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End Try
            If Val(Common_Procedures.settings.YarnDelivery_Print_2Copy_In_SinglePage) = 1 Then
                If PCnt = 1 And PrntCnt = 2 And PrntCnt2ndPageSTS = False Then
                    If prn_DetDt.Rows.Count > 5 Then
                        PrntCnt2ndPageSTS = True
                        e.HasMorePages = True
                        Return
                    End If
                End If
            End If
            PrntCnt2ndPageSTS = False

        Next PCnt
LOOP2:

        prn_Count = prn_Count + 1

        e.HasMorePages = False

        If Val(prn_TotCopies) > 1 Then
            If prn_Count < Val(prn_TotCopies) Then

                prn_DetIndx = 0
                prn_PageNo = 0
                prn_DetIndx = 0
                prn_PageNo = 0
                'prn_NoofBmDets = 0


                e.HasMorePages = True
                Return

            End If

        End If


    End Sub

    Private Sub Printing_Format4_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_Email As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim p1Font As Font
        Dim strHeight As Single = 0, strWidth As Single = 0
        Dim W1 As Single, C1 As Single, S1 As Single, K1 As Single
        Dim CurX As Single = 0
        Dim Gst_dt As Date
        Dim Entry_dt As Date

        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Mill_Name, c.Count_Name  from SizingSoft_Yarn_Delivery_Details a INNER JOIN Mill_Head b on a.Mill_IdNo = b.Mill_IdNo LEFT OUTER JOIN Count_Head c on a.Count_IdNo = c.Count_IdNo where a.Yarn_Delivery_Code = '" & Trim(EntryCode) & "' Order by a.sl_no", con)
        da2.Fill(dt2)
        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        dt2.Clear()


        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_Email = ""
        Cmp_StateCap = "" : Cmp_StateNm = "" : Cmp_StateCode = "" : Cmp_GSTIN_Cap = "" : Cmp_GSTIN_No = ""

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1015" Then
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString
            Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address2").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString

        Else

            If Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString) <> "" Then
                Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
                Cmp_Name = Replace(Cmp_Name, Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString), "")
                Cmp_Add1 = Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString)
                Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString

            Else
                Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
                Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
                Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString

            End If

        End If
        'Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        'Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
        'Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString
        If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "PHONE NO.:" & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
            Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString) <> "" Then
            Cmp_CstNo = "CST NO.: " & prn_HdDt.Rows(0).Item("Company_CstNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
            Cmp_Email = "E-mail : " & Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString)
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




        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 18, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        If Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString) <> "" Then
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Else
            p1Font = New Font("Calibri", 9, FontStyle.Regular)
        End If
        CurY = CurY + strHeight
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, p1Font)

        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height
        CurY = CurY + strHeight
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)

        'CurY = CurY + strHeight
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1 & " " & Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)

        Gst_dt = #7/1/2017#
        Entry_dt = dtp_Date.Value

        If DateDiff("d", Gst_dt.ToShortDateString, Entry_dt.ToShortDateString) < 0 Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt - 1
            Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt - 13  ' 10

        Else

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
            Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PhNo & "   " & Cmp_Email), LMargin, CurY, 2, PrintWidth, pFont)
            'CurY = CurY + TxtHgt - 1
            'Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, Cmp_Email, PageWidth - 10, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt - 1

        End If
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)

        'CurY = CurY + TxtHgt - 13  ' 10
        p1Font = New Font("Calibri", 14, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "YARN DELIVERY NOTE", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        'CurY = CurY + TxtHgt

        CurY = CurY + strHeight + 5 ' + 150
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try

            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4)
            W1 = e.Graphics.MeasureString("BOOK NO  : ", pFont).Width
            K1 = e.Graphics.MeasureString("SIZING BOOK NO : ", pFont).Width
            S1 = e.Graphics.MeasureString("TO    :", pFont).Width

            CurY = CurY + TxtHgt - 5
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "TO  :  " & "M/s. " & prn_HdDt.Rows(0).Item("Ledger_MainName").ToString, LMargin + 10, CurY, 0, 0, p1Font)

            Common_Procedures.Print_To_PrintDocument(e, "DC.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + K1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Yarn_Delivery_No").ToString, LMargin + C1 + K1 + 25, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 9, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, p1Font)
            p1Font = New Font("Calibri", 14, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY + 8, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + K1 + 10, CurY + 8, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Yarn_Delivery_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + K1 + 25, CurY + 8, 0, 0, pFont)

            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 9, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, p1Font)
            If Trim(prn_HdDt.Rows(0).Item("Book_No").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "SIZING BOOK NO", LMargin + C1 + 10, CurY + 14, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + K1 + 10, CurY + 14, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Book_No").ToString, LMargin + C1 + K1 + 30, CurY + 14, 0, 0, pFont)
            Else
                If Trim(prn_HdDt.Rows(0).Item("Delivery_At").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, "DELIVERY AT", LMargin + C1 + 10, CurY + 20, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + K1 + 10, CurY + 20, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Delivery_At").ToString, LMargin + C1 + K1 + 30, CurY + 20, 0, 0, pFont)
                End If
            End If

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, p1Font)
            If Trim(prn_HdDt.Rows(0).Item("Textile_Dc_No").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "TEXTILE DC NO", LMargin + C1 + 10, CurY + 20, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + K1 + 10, CurY + 20, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Textile_Dc_No").ToString, LMargin + C1 + K1 + 30, CurY + 20, 0, 0, pFont)
            End If


            CurY = CurY + TxtHgt
            If DateDiff("d", Gst_dt.ToShortDateString, Entry_dt.ToShortDateString) < 0 Then
                If Trim(prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, " TIN NO : " & Trim(prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString), LMargin + S1 + 10, CurY, 0, 0, pFont)
                End If

            Else
                If Trim(prn_HdDt.Rows(0).Item("ledger_GSTinNo").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, " GSTIN NO : " & Trim(prn_HdDt.Rows(0).Item("ledger_GSTinNo").ToString), LMargin + S1 + 10, CurY, 0, 0, pFont)
                End If

            End If


            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + C1, LnAr(3), LMargin + C1, LnAr(2))

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "TYPE", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "SET NO", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "MILL NAME", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "COUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "BAGS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "CONES", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format4_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim I As Integer
        Dim Cmp_Name As String

        Try

            For I = NoofDets + 1 To NoofItems_PerPage
                CurY = CurY + TxtHgt
            Next

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

            CurY = CurY + TxtHgt - 10
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, " TOTAL", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)

                If Val(prn_HdDt.Rows(0).Item("Total_Bags").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Bags").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                End If
                If Val(prn_HdDt.Rows(0).Item("Total_Cones").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Cones").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                End If
                If Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString), "#########0.000"), PageWidth - 10, CurY, 1, 0, pFont)
                End If
            End If

            CurY = CurY + TxtHgt - 15

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))

            CurY = CurY + TxtHgt - 5

            Common_Procedures.Print_To_PrintDocument(e, "Through : " & Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString), LMargin + 10, CurY, 0, 0, pFont)
            If Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, " Empty Beam : " & Trim(prn_HdDt.Rows(0).Item("Empty_Beam").ToString), PageWidth - 430, CurY, 0, 0, pFont)
            End If
            If Val(prn_HdDt.Rows(0).Item("Empty_Bags").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, " Empty Bags : " & Trim(prn_HdDt.Rows(0).Item("Empty_Bags").ToString), PageWidth - 280, CurY, 0, 0, pFont)
            End If
            If Val(prn_HdDt.Rows(0).Item("Empty_Cones").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, " Empty Cones : " & Trim(prn_HdDt.Rows(0).Item("Empty_Cones").ToString), PageWidth - 150, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(7) = CurY

            CurY = CurY + TxtHgt
            If Val(Common_Procedures.User.IdNo) <> 1 Then
                Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
            End If
            CurY = CurY + TxtHgt

            If Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString) <> "" Then
                Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
                Cmp_Name = Replace(Cmp_Name, Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString), "")

            Else
                Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString

            End If

            Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 20, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 250, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, LMargin + PageWidth - 75, CurY, 1, 0, p1Font)

            'e.Graphics.DrawLine(Pens.Black, PageWidth, CurY, PageWidth, LnAr(1))

            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format5(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim EntryCode As String
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ItmNm1 As String, ItmNm2 As String
        'Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim PS As Printing.PaperSize

        Dim PCnt As Integer = 0, PrntCnt As Integer = 0
        Dim TpMargin As Single = 0


        PrntCnt = 1
        If Val(Common_Procedures.settings.YarnDelivery_Print_2Copy_In_SinglePage) = 1 Then
            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                    PS = PrintDocument1.PrinterSettings.PaperSizes(I)
                    PrintDocument1.DefaultPageSettings.PaperSize = PS
                    e.PageSettings.PaperSize = PS
                    Exit For
                End If
            Next


        Else

            'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            '    PS = PrintDocument1.PrinterSettings.PaperSizes(I)
            '    Debug.Print(PS.PaperName)
            '    If PS.Width = 800 And PS.Height = 600 Then
            '        PrintDocument1.DefaultPageSettings.PaperSize = PS
            '        e.PageSettings.PaperSize = PS
            '        PpSzSTS = True
            '        Exit For
            '    End If
            'Next

            'If PpSzSTS = False Then

            If PpSzSTS = False Then
                For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                        PS = PrintDocument1.PrinterSettings.PaperSizes(I)
                        PrintDocument1.DefaultPageSettings.PaperSize = PS
                        e.PageSettings.PaperSize = PS
                        Exit For
                    End If
                Next


            Else
                For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A5 Then
                        PS = PrintDocument1.PrinterSettings.PaperSizes(I)
                        PrintDocument1.DefaultPageSettings.PaperSize = PS
                        e.PageSettings.PaperSize = PS
                        PpSzSTS = True
                        Exit For
                    End If
                Next
            End If

            'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            '    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
            '        PS = PrintDocument1.PrinterSettings.PaperSizes(I)
            '        PrintDocument1.DefaultPageSettings.PaperSize = PS
            '        e.PageSettings.PaperSize = PS
            '        PpSzSTS = True
            '        Exit For
            '    End If
            'Next



        End If
        'End If
        If Val(Common_Procedures.settings.YarnDelivery_Print_2Copy_In_SinglePage) = 1 Then
            If PrntCnt2ndPageSTS = False Then
                PrntCnt = 2
            End If
        End If


        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 10
            .Right = 30
            .Top = 30
            .Bottom = 30
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pFont = New Font("Calibri", 8, FontStyle.Regular)

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


        If (Trim(UCase(Common_Procedures.settings.CustomerCode))) = "1288" Then
            NoofItems_PerPage = 32 ' 6 ' 5
        Else
            NoofItems_PerPage = 4 ' 6 ' 5
        End If


        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = 40
        ClArr(2) = 60 : ClArr(3) = 80 : ClArr(4) = 210 : ClArr(5) = 100 : ClArr(6) = 70 : ClArr(7) = 70
        ClArr(8) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7))

        TxtHgt = 18 ' 18.8  ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        PrntCnt2ndPageSTS = False
        TpMargin = TMargin

        For PCnt = 1 To PrntCnt
            If Val(Common_Procedures.settings.YarnDelivery_Print_2Copy_In_SinglePage) = 1 Then
                If PCnt = 1 Then
                    prn_PageNo = 0

                    prn_DetIndx = 0
                    prn_DetSNo = 0
                    ' prn_NoofBmDets = 0
                    TpMargin = TMargin

                Else

                    prn_PageNo = 0
                    ' prn_NoofBmDets = 0
                    prn_DetIndx = 0
                    prn_DetSNo = 0

                    TpMargin = 580 + TMargin  ' 600 + TMargin

                End If
            End If
            Try

                If prn_HdDt.Rows.Count > 0 Then

                    Printing_Format5_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TpMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                    Try

                        NoofDets = 0

                        CurY = CurY - 10

                        If prn_DetDt.Rows.Count > 0 Then

                            Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                                If NoofDets >= NoofItems_PerPage Then
                                    CurY = CurY + TxtHgt

                                    Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                                    NoofDets = NoofDets + 1

                                    Printing_Format5_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

                                    e.HasMorePages = True
                                    Return

                                End If

                                prn_DetSNo = prn_DetSNo + 1

                                ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Mill_Name").ToString)
                                ItmNm2 = ""
                                If Len(ItmNm1) > 18 Then
                                    For I = 18 To 1 Step -1
                                        If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                                    Next I
                                    If I = 0 Then I = 18
                                    ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                                    ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                                End If


                                CurY = CurY + TxtHgt

                                Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Sl_No").ToString), LMargin + 25, CurY, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Yarn_Type").ToString, LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                                If IsDBNull(prn_DetDt.Rows(prn_DetIndx).Item("Set_No").ToString) = False Then
                                    If Trim(prn_DetDt.Rows(prn_DetIndx).Item("Set_No").ToString) <> "" Then
                                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Set_no").ToString), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                                    End If
                                End If
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Count_Name").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 10, CurY, 0, 0, pFont)




                                If Val(prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString) <> 0 Then
                                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)
                                End If
                                If Val(prn_DetDt.Rows(prn_DetIndx).Item("Cones").ToString) <> 0 Then
                                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Cones").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
                                End If
                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Weight").ToString), "########0.000"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)

                                NoofDets = NoofDets + 1

                                If Trim(ItmNm2) <> "" Then
                                    CurY = CurY + TxtHgt - 5
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)
                                    NoofDets = NoofDets + 1
                                End If

                                prn_DetIndx = prn_DetIndx + 1

                            Loop

                        End If

                        Printing_Format5_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)

                    Catch ex As Exception

                        MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                    End Try

                End If

            Catch ex As Exception

                MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End Try
            If Val(Common_Procedures.settings.YarnDelivery_Print_2Copy_In_SinglePage) = 1 Then
                If PCnt = 1 And PrntCnt = 2 And PrntCnt2ndPageSTS = False Then
                    If prn_DetDt.Rows.Count > 4 Then
                        PrntCnt2ndPageSTS = True
                        e.HasMorePages = True
                        Return
                    End If
                End If
            End If
            PrntCnt2ndPageSTS = False

        Next PCnt
LOOP2:

        prn_Count = prn_Count + 1

        e.HasMorePages = False

        If Val(prn_TotCopies) > 1 Then
            If prn_Count < Val(prn_TotCopies) Then

                prn_DetIndx = 0
                prn_PageNo = 0
                prn_DetIndx = 0
                prn_PageNo = 0
                'prn_NoofBmDets = 0


                e.HasMorePages = True
                Return

            End If

        End If


    End Sub

    Private Sub Printing_Format5_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String, Cmp_Add3 As String, Cmp_Add4 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_Email As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim p1Font As Font
        Dim strHeight As Single = 0, strWidth As Single = 0
        Dim W1 As Single, C1 As Single, S1 As Single, K1 As Single, M1 As Single
        Dim CurX As Single = 0
        Dim Hsn_Code As String = ""
        Dim Led_Name As String, Led_Add1 As String, Led_Add2 As String, Led_Add3 As String, Led_Add4 As String
        Dim Led_GstNo As String

        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Mill_Name, c.Count_Name  from SizingSoft_Yarn_Delivery_Details a INNER JOIN Mill_Head b on a.Mill_IdNo = b.Mill_IdNo LEFT OUTER JOIN Count_Head c on a.Count_IdNo = c.Count_IdNo where a.Yarn_Delivery_Code = '" & Trim(EntryCode) & "' and c.Count_Name = '" & Trim(dgv_Details.Rows(0).Cells(1).Value) & "' Order by a.sl_no", con)
        da2.Fill(dt2)
        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)

        End If
        dt2.Clear()
        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Mill_Name, c.*  from SizingSoft_Yarn_Delivery_Details a INNER JOIN Mill_Head b on a.Mill_IdNo = b.Mill_IdNo LEFT OUTER JOIN Count_Head c on a.Count_IdNo = c.Count_IdNo where a.Yarn_Delivery_Code = '" & Trim(EntryCode) & "' and c.Count_Name = '" & Trim(dgv_Details.Rows(0).Cells(1).Value) & "' Order by a.sl_no", con)
        da2.Fill(dt3)
        If dt3.Rows.Count > 0 Then

            Hsn_Code = dt3.Rows(0).Item("Count_Hsn_Code").ToString
        End If
        dt3.Clear()

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = "" : Cmp_Add3 = "" : Cmp_Add4 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_Email = ""
        Cmp_StateCap = "" : Cmp_StateNm = "" : Cmp_StateCode = "" : Cmp_GSTIN_Cap = "" : Cmp_GSTIN_No = ""
        Led_Name = "" : Led_Add1 = "" : Led_Add2 = "" : Led_Add3 = "" : Led_Add4 = "" : Led_GstNo = ""

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString
        Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address2").ToString
        Cmp_Add3 = prn_HdDt.Rows(0).Item("Company_Address3").ToString
        Cmp_Add4 = prn_HdDt.Rows(0).Item("Company_Address4").ToString

        Led_Name = prn_HdDt.Rows(0).Item("Ledger_MainName").ToString
        Led_Add1 = prn_HdDt.Rows(0).Item("Ledger_Address1").ToString
        Led_Add2 = prn_HdDt.Rows(0).Item("Ledger_Address2").ToString
        Led_Add3 = prn_HdDt.Rows(0).Item("Ledger_Address3").ToString
        Led_Add4 = prn_HdDt.Rows(0).Item("Ledger_Address4").ToString

        If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "PHONE NO.:" & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
            Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString) <> "" Then
            Cmp_CstNo = "CST NO.: " & prn_HdDt.Rows(0).Item("Company_CstNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
            Cmp_Email = "E-mail : " & Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString)
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
            Cmp_GSTIN_No = "GSTIN : " & prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then

            Led_GstNo = "GSTIN : " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString
        End If


        CurY = CurY + TxtHgt - 10
        M1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4)
        If (Common_Procedures.settings.CustomerCode) = "1282" Then
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Else
            p1Font = New Font("Calibri", 14, FontStyle.Bold)
        End If
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin + 10, CurY, 0, 0, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height
        Common_Procedures.Print_To_PrintDocument(e, "YARN SIZED TO :", LMargin + M1 + 10, CurY, 0, 0, pFont)

        ' p1Font = New Font("Calibri", 9, FontStyle.Regular)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "M/s." & Led_Name, LMargin + M1 + 10, CurY, 0, 0, p1Font)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Led_Add1, LMargin + M1 + 10, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add3, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Led_Add2, LMargin + M1 + 10, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add4, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Led_Add3, LMargin + M1 + 10, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Led_Add4, LMargin + M1 + 10, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Led_GstNo, LMargin + M1 + 10, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY
        e.Graphics.DrawLine(Pens.Black, LMargin + M1, LnAr(2), LMargin + M1, LnAr(1))


        CurY = CurY + TxtHgt - 12
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "YARN DELIVERY NOTE", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


        Try

            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4)
            W1 = e.Graphics.MeasureString("BOOK NO            : ", pFont).Width
            K1 = e.Graphics.MeasureString("SIZING BOOK NO : ", pFont).Width
            S1 = e.Graphics.MeasureString("TO    :", pFont).Width

            CurY = CurY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, "DC.NO", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 9, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("yarn_Delivery_No").ToString, LMargin + W1 + 25, CurY, 0, 0, p1Font)



            Common_Procedures.Print_To_PrintDocument(e, "VAN NO", LMargin + M1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vehicle_No").ToString, LMargin + M1 + W1 + 25, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, "DATE & TIME", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, (prn_HdDt.Rows(0).Item("Date_And_Time_Of_Supply").ToString).ToString, LMargin + W1 + 25, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Entry_Time_Text").ToString, PageWidth, CurY, 1, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "APPROX VALUE", LMargin + M1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Approx_Value").ToString, LMargin + M1 + W1 + 25, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, "HSN CODE", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, (Hsn_Code), LMargin + W1 + 25, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "E.REF", LMargin + M1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Electronic_Reference_No").ToString, LMargin + M1 + W1 + 25, CurY, 0, 0, pFont)

            If Common_Procedures.settings.CustomerCode <> "1102" Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "SAC CODE", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " 998821", LMargin + W1 + 25, CurY, 0, 0, pFont)

                Common_Procedures.Print_To_PrintDocument(e, "TRANSPORT", LMargin + M1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Transport_Name").ToString, LMargin + M1 + W1 + 25, CurY, 0, 0, pFont)

            End If
            If Common_Procedures.settings.CustomerCode <> "1102" Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "SIZING BOOK NO", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Book_No").ToString, LMargin + W1 + 25, CurY, 0, 0, pFont)

                Common_Procedures.Print_To_PrintDocument(e, "TEXTILE DC NO", LMargin + M1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Textile_Dc_No").ToString, LMargin + M1 + W1 + 25, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY


            CurY = CurY + TxtHgt - 12
            Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "TYPE", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "SET NO", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "MILL NAME", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "COUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "BAGS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "CONES", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format5_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim I As Integer
        Dim Cmp_Name As String
        Dim C1 As Single
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Del_Add1 As String = "", Del_Add2 As String = "", nGST_No As String = ""
        Try

            For I = NoofDets + 1 To NoofItems_PerPage
                CurY = CurY + TxtHgt
            Next

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

            CurY = CurY + TxtHgt - 10
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, " TOTAL", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)

                If Val(prn_HdDt.Rows(0).Item("Total_Bags").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Bags").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                End If
                If Val(prn_HdDt.Rows(0).Item("Total_Cones").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Cones").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                End If
                If Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString), "#########0.000"), PageWidth - 10, CurY, 1, 0, pFont)
                End If
            End If


            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))
            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4)
            CurY = CurY + TxtHgt - 12

            p1Font = New Font("Calibri", 10, FontStyle.Bold)

            If Trim(Common_Procedures.settings.CustomerCode) = "1282" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1288" Then

                If Common_Procedures.Vendor_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("Vendor_IdNo").ToString)) <> "" Then
                    da = New SqlClient.SqlDataAdapter("SELECT a.* From Vendor_Head a Where a.Vendor_IdNo = " & Val(prn_HdDt.Rows(0).Item("Vendor_IdNo").ToString) & " ", con)
                    dt = New DataTable
                    da.Fill(dt)


                    If dt.Rows.Count > 0 Then
                        Del_Add1 = dt.Rows(0).Item("Vendor_address1").ToString & "," & dt.Rows(0).Item("Vendor_Address2").ToString
                        Del_Add2 = dt.Rows(0).Item("Vendor_address3").ToString & "," & dt.Rows(0).Item("Vendor_Address4").ToString
                        If Trim(dt.Rows(0).Item("GST_No").ToString) <> "" Then nGST_No = "GSTIN : " & dt.Rows(0).Item("GST_No").ToString
                    End If


                    Common_Procedures.Print_To_PrintDocument(e, "DELIVERY TO   : " & Common_Procedures.Vendor_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("Vendor_IdNo").ToString)), LMargin + 10, CurY, 0, 0, pFont)
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, " " & Trim(Del_Add1), LMargin + 30, CurY, 0, 0, pFont)
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, " " & Trim(Del_Add2), LMargin + 30, CurY, 0, 0, pFont)
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, " " & Trim(nGST_No), LMargin + 30, CurY, 0, 0, pFont)


                    'Del_Add1 = prn_HdDt.Rows(0).Item("Ledger_Address1").ToString & "," & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString
                    'Del_Add2 = prn_HdDt.Rows(0).Item("Ledger_Address3").ToString & "," & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString


                    'Common_Procedures.Print_To_PrintDocument(e, "DELIVERY TO   : " & prn_HdDt.Rows(0).Item("Ledger_MainName").ToString, LMargin + 10, CurY, 0, 0, pFont)
                    'CurY = CurY + TxtHgt
                    'Common_Procedures.Print_To_PrintDocument(e, " " & Trim(Del_Add1), LMargin + 30, CurY, 0, 0, pFont)
                    'CurY = CurY + TxtHgt
                    'Common_Procedures.Print_To_PrintDocument(e, " " & Trim(Del_Add2), LMargin + 30, CurY, 0, 0, pFont)

                Else

                    Del_Add1 = prn_HdDt.Rows(0).Item("DelAdd1").ToString & "," & prn_HdDt.Rows(0).Item("DelAdd2").ToString
                    Del_Add2 = prn_HdDt.Rows(0).Item("DelAdd3").ToString & "," & prn_HdDt.Rows(0).Item("DelAdd4").ToString

                    Common_Procedures.Print_To_PrintDocument(e, "DELIVERY TO   : " & prn_HdDt.Rows(0).Item("DelName").ToString, LMargin + 10, CurY, 0, 0, pFont)
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, " " & Trim(Del_Add1), LMargin + 30, CurY, 0, 0, pFont)
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, " " & Trim(Del_Add2), LMargin + 30, CurY, 0, 0, pFont)
                End If

            Else

                Del_Add1 = prn_HdDt.Rows(0).Item("DelAdd1").ToString & "," & prn_HdDt.Rows(0).Item("DelAdd2").ToString
                Del_Add2 = prn_HdDt.Rows(0).Item("DelAdd3").ToString & "," & prn_HdDt.Rows(0).Item("DelAdd4").ToString


                Common_Procedures.Print_To_PrintDocument(e, "DELIVERY TO   : " & prn_HdDt.Rows(0).Item("DelName").ToString, LMargin + 10, CurY, 0, 0, p1Font)
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, " " & Trim(Del_Add1), LMargin + 30, CurY, 0, 0, pFont)
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, " " & Trim(Del_Add2), LMargin + 30, CurY, 0, 0, pFont)
                If Trim(prn_HdDt.Rows(0).Item("DelGSTinNo").ToString) <> "" Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, " GSTIN " & prn_HdDt.Rows(0).Item("DelGSTinNo").ToString, LMargin + 10, CurY, 0, 0, pFont)
                End If

            End If

            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            CurY = CurY + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("Remarks").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, " Remarks : " & Trim(prn_HdDt.Rows(0).Item("Remarks").ToString), LMargin + 10, CurY, 0, 0, p1Font)
            End If


            'Common_Procedures.Print_To_PrintDocument(e, "DELIVERY TO   : " & prn_HdDt.Rows(0).Item("DelName").ToString, LMargin + 10, CurY, 0, 0, pFont)
            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelAdd1").ToString, LMargin + 30, CurY, 0, 0, pFont)
            If Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, " Empty Beam : " & Trim(prn_HdDt.Rows(0).Item("Empty_Beam").ToString), LMargin + C1 + 10, CurY, 0, 0, pFont)
            End If
            'Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelAdd2").ToString, LMargin + 30, CurY, 0, 0, pFont)
            If Val(prn_HdDt.Rows(0).Item("Empty_Bags").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, " Empty Bags : " & Trim(prn_HdDt.Rows(0).Item("Empty_Bags").ToString), LMargin + C1 + 10, CurY, 0, 0, pFont)
            End If

            'Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelAdd3").ToString, LMargin + 30, CurY, 0, 0, pFont)
            If Val(prn_HdDt.Rows(0).Item("Empty_Cones").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, " Empty Cones : " & Trim(prn_HdDt.Rows(0).Item("Empty_Cones").ToString), LMargin + C1 + 10, CurY, 0, 0, pFont)
            End If
            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelAdd4").ToString, LMargin + 30, CurY, 0, 0, pFont)



            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "This is to certify that there is no sale indicated in this delivery and the yarn sized is returned back to party after warping and sizing job work.", LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(7) = CurY

            CurY = CurY + TxtHgt
            If Val(Common_Procedures.User.IdNo) <> 1 Then
                Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
            End If
            CurY = CurY + TxtHgt

            If Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString) <> "" Then
                Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
                Cmp_Name = Replace(Cmp_Name, Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString), "")

            Else
                Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString

            End If

            If Trim(Common_Procedures.settings.CustomerCode) = "1288" Then
                Common_Procedures.Print_To_PrintDocument(e, "Receiver's ", LMargin + 20, CurY, 0, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 20, CurY, 0, 0, pFont)
            End If

            If Trim(Common_Procedures.settings.CustomerCode) = "1288" Then
                Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 120, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "Checked By  ", LMargin + 250, CurY, 0, 0, pFont)

            ElseIf Trim(Common_Procedures.settings.CustomerCode) = "1282" Then
                Common_Procedures.Print_To_PrintDocument(e, "Checked By ", LMargin + 250, CurY, 0, 0, pFont)

            Else
                Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 250, CurY, 0, 0, pFont)
            End If



            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, LMargin + PageWidth - 75, CurY, 1, 0, p1Font)

            'e.Graphics.DrawLine(Pens.Black, PageWidth, CurY, PageWidth, LnAr(1))

            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Cbo_DelTo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_DeliveryTo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Sales_DeliveryAddress_Head", "Party_Name", "", "(Party_IdNo = 0)")
        '  Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Delivery_Party_Head", "Ledger_Name", "", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub Cbo_DelTo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DeliveryTo.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_DeliveryTo, cbo_VehicleNo, txt_Remarks, "Sales_DeliveryAddress_Head", "Party_Name", "", "(Party_IdNo = 0)")
        ' Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_DeliveryTo, cbo_VehicleNo, txt_Remarks, "Delivery_Party_Head", "Ledger_Name", "", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub Cbo_DelTo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_DeliveryTo.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_DeliveryTo, txt_Remarks, "Sales_DeliveryAddress_Head", "Party_Name", "", "(Party_IdNo = 0)")
        ' Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_DeliveryTo, txt_Remarks, "Delivery_Party_Head", "Ledger_Name", "", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub Cbo_DelTo_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DeliveryTo.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Common_Procedures.MDI_LedType = ""
            Dim f As New Delivery_Party_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_DeliveryTo.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub txt_DateAndTimeOFSupply_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_DateAndTimeOFSupply.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then txt_ElectronicRefNo.Focus()
    End Sub

    Private Sub txt_DateAndTimeOFSupply_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_DateAndTimeOFSupply.KeyPress
        If Asc(e.KeyChar) = 13 Then
            cbo_CountName.Focus()
        End If
    End Sub

    Private Sub btn_SMS_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_SMS.Click
        Send_SMS()
    End Sub

    Private Sub cbo_VendorName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_VendorName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Vendor_AlaisHead", "Vendor_DisplayName", "", "(Vendor_IdNo = 0)")
    End Sub

    Private Sub cbo_VendorName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_VendorName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_VendorName, cbo_VehicleNo, txt_Remarks, "Vendor_AlaisHead", "Vendor_DisplayName", "", "(Vendor_IdNo = 0)")
    End Sub

    Private Sub cbo_VendorName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_VendorName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_VendorName, txt_Remarks, "Vendor_AlaisHead", "Vendor_DisplayName", "", "(Vendor_IdNo = 0)")
    End Sub

    Private Sub cbo_VendorName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_VendorName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            Dim f As New Vendor_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_VendorName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub btn_UserModification_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_UserModification.Click
        If Val(Common_Procedures.User.IdNo) = 1 Then
            Dim f1 As New User_Modifications
            f1.Entry_Name = Me.Name
            f1.Entry_PkValue = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            f1.ShowDialog()
        End If
    End Sub

    Public Sub New()

        FrmLdSTS = True
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Private Sub cbo_Delivered_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Delivered.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Delivered, txt_Approx_Value, cbo_Transport, "SizingSoft_Yarn_Delivery_Head", "Delivered_By", "", "")
    End Sub

    Private Sub cbo_Delivered_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Delivered.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Delivered, cbo_Transport, "SizingSoft_Yarn_Delivery_Head", "Delivered_By", "", "", False)
    End Sub


    Private Sub Update_PrintOut_Status(Optional ByVal sqltr As SqlClient.SqlTransaction = Nothing)
        Dim cmd As New SqlClient.SqlCommand
        Dim NewCode As String = ""
        Dim vPrnSTS As Integer = 0


        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            If IsNothing(sqltr) = False Then
                cmd.Transaction = sqltr
            End If

            vPrnSTS = 0
            If chk_Printed.Checked = True Then
                vPrnSTS = 1
            End If

            cmd.CommandText = "Update SizingSoft_Yarn_Delivery_Head set PrintOut_Status = " & Str(Val(vPrnSTS)) & " where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Delivery_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            If chk_Printed.Checked = True Then
                chk_Printed.Visible = True
                If Val(Common_Procedures.User.IdNo) = 1 Then
                    chk_Printed.Enabled = True
                End If
            End If

            cmd.Dispose()

        Catch ex As Exception
            MsgBox(ex.Message)

        End Try

    End Sub

    Private Sub PrintPreview_Shown(ByVal sender As Object, ByVal e As System.EventArgs)
        'Capture the click events for the toolstrip in the dialog box when the dialog is shown
        Dim ts As ToolStrip = CType(sender.Controls(1), ToolStrip)
        AddHandler ts.ItemClicked, AddressOf PrintPreview_Toolstrip_ItemClicked
    End Sub

    Private Sub PrintPreview_Toolstrip_ItemClicked(ByVal sender As Object, ByVal e As ToolStripItemClickedEventArgs)
        'If it is the print button that was clicked: run the printdialog
        If LCase(e.ClickedItem.Name) = LCase("printToolStripButton") Then

            Try
                chk_Printed.Checked = True
                chk_Printed.Visible = True
                Update_PrintOut_Status()

            Catch ex As Exception
                MsgBox("Print Error: " & ex.Message)

            End Try
        End If
    End Sub

    Private Sub cbo_godown_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Godown.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'GODOWN')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_godown_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Godown.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Godown, cbo_coneType, txt_ElectronicRefNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_type ='GODOWN')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_godown_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Godown.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Godown, txt_ElectronicRefNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_type ='GODOWN')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_godown_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Godown.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Common_Procedures.MDI_LedType = "GODOWN"
            Dim f As New Ledger_Creation
            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_bagType.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""
            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub btn_Selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Selection.Click
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim i As Integer = 0, j As Integer = 0, n As Integer = 0, SNo As Integer = 0
        Dim LedIdNo As Integer = 0, CmpIdNo As String = 0
        Dim NewCode As String = ""
        Dim CompIDCondt As String = ""
        Dim Ent_Bag As Single = 0
        Dim Ent_Wgt As Single = 0
        Dim Ent_Cone As Single = 0
        Dim Ent_Exc As Single = 0
        Dim TexStk_iD As String = 0
        Dim nr As Single = 0
        Dim vDbName As String = ""

        Exit Sub

        If Trim(TrnTo_DbName) <> "" Then
            vDbName = Trim(TrnTo_DbName) & ".."
        End If

        LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text, , TrnTo_DbName)
        If LedIdNo = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SELECT DELIVERY...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
            Exit Sub
        End If

        TexStk_iD = Common_Procedures.get_FieldValue(con, "ledger_head", "Textile_To_CompanyIdNo", "(ledger_idno = " & Str(Val(LedIdNo)) & ")")
        If Val(TexStk_iD) = 0 Then Exit Sub


        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)


        CompIDCondt = "(a.company_idno = " & Str(Val(lbl_Company.Tag)) & ")"
        If Trim(UCase(Common_Procedures.settings.CompanyName)) = "-----~~~" Then
            CompIDCondt = ""
        End If


        With dgv_Selection

            .Rows.Clear()
            SNo = 0

            Da = New SqlClient.SqlDataAdapter("select  a.*, h.Bags As Ent_Bag,  h.Weight As Ent_Wgt,h.Cones As Ent_COne   from " & Trim(vDbName) & "Weaver_Yarn_Requirement_Details a   LEFT OUTER JOIN SizingSoft_Yarn_Delivery_Details h ON h.Yarn_Delivery_Code = '" & Trim(NewCode) & "' and a.Weaver_Yarn_Requirement_Code = h.Weaver_Yarn_Requirement_Code and a.Weaver_Yarn_Requirement_Details_SlNo = h.Weaver_Yarn_Requirement_Details_SlNo Where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.DeliveryTo_IdNo = " & Str(Val(LedIdNo)) & " and ((a.Weight - a.Delivery_Weight ) > 0 or h.Weight > 0 ) order by a.Weaver_Yarn_Requirement_Date, a.for_orderby, a.Weaver_Yarn_Requirement_No", con)
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
                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Weaver_Yarn_Requirement_No").ToString
                    .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Weaver_Yarn_Requirement_Date").ToString), "dd-MM-yyyy")
                    .Rows(n).Cells(3).Value = Common_Procedures.Count_IdNoToName(con, Val(Dt1.Rows(i).Item("Count_IdNo").ToString), , TrnTo_DbName)
                    .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Yarn_Type").ToString
                    .Rows(n).Cells(5).Value = Common_Procedures.Mill_IdNoToName(con, Val(Dt1.Rows(i).Item("Mill_IdNo").ToString), , TrnTo_DbName)
                    .Rows(n).Cells(6).Value = Format(Val(Dt1.Rows(i).Item("Bags").ToString) - Val(Dt1.Rows(i).Item("Delivery_Bag").ToString) + Val(Ent_Bag), "#########0.00")
                    .Rows(n).Cells(7).Value = Format(Val(Dt1.Rows(i).Item("Cones").ToString) - Val(Dt1.Rows(i).Item("Delivery_Cone").ToString) + Val(Ent_Cone), "#########0.00")
                    .Rows(n).Cells(8).Value = Format(Val(Dt1.Rows(i).Item("Weight").ToString) - Val(Dt1.Rows(i).Item("Delivery_Weight").ToString) + Val(Ent_Wgt), "#########0.000")

                    If Ent_Wgt > 0 Then
                        .Rows(n).Cells(9).Value = "1"
                        For j = 0 To .ColumnCount - 1
                            .Rows(n).Cells(j).Style.ForeColor = Color.Red
                        Next

                    Else
                        .Rows(n).Cells(9).Value = ""

                    End If

                    .Rows(n).Cells(10).Value = Dt1.Rows(i).Item("Weaver_Yarn_Requirement_Code").ToString
                    .Rows(n).Cells(11).Value = Dt1.Rows(i).Item("Weaver_Yarn_Requirement_Details_SlNo").ToString

                    .Rows(n).Cells(12).Value = Ent_Bag
                    .Rows(n).Cells(13).Value = Ent_Cone
                    .Rows(n).Cells(14).Value = Ent_Wgt
                    ' .Rows(n).Cells(16).Value = Ent_Exc

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

                .Rows(RwIndx).Cells(9).Value = (Val(.Rows(RwIndx).Cells(9).Value) + 1) Mod 2

                If Val(.Rows(RwIndx).Cells(9).Value) = 1 Then

                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
                    Next


                Else
                    .Rows(RwIndx).Cells(9).Value = ""

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

            If Val(dgv_Selection.Rows(i).Cells(9).Value) = 1 Then


                n = dgv_Details.Rows.Add()
                sno = sno + 1
                dgv_Details.Rows(n).Cells(0).Value = Val(sno)
                dgv_Details.Rows(n).Cells(1).Value = dgv_Selection.Rows(i).Cells(3).Value
                dgv_Details.Rows(n).Cells(2).Value = dgv_Selection.Rows(i).Cells(4).Value
                ' dgv_Details.Rows(n).Cells(3).Value = dgv_Selection.Rows(i).Cells(4).Value
                dgv_Details.Rows(n).Cells(4).Value = dgv_Selection.Rows(i).Cells(5).Value

                ' cbo_TransportName.Text = dgv_Selection.Rows(i).Cells(10).Value
                dgv_Details.Rows(n).Cells(8).Value = dgv_Selection.Rows(i).Cells(1).Value
                dgv_Details.Rows(n).Cells(9).Value = dgv_Selection.Rows(i).Cells(10).Value
                dgv_Details.Rows(n).Cells(10).Value = dgv_Selection.Rows(i).Cells(11).Value
                If Val(dgv_Selection.Rows(i).Cells(12).Value) <> 0 Then
                    dgv_Details.Rows(n).Cells(5).Value = dgv_Selection.Rows(i).Cells(12).Value
                Else
                    dgv_Details.Rows(n).Cells(5).Value = dgv_Selection.Rows(i).Cells(6).Value
                End If

                If Val(dgv_Selection.Rows(i).Cells(13).Value) <> 0 Then
                    dgv_Details.Rows(n).Cells(6).Value = dgv_Selection.Rows(i).Cells(13).Value
                Else
                    dgv_Details.Rows(n).Cells(6).Value = dgv_Selection.Rows(i).Cells(7).Value
                End If

                If Val(dgv_Selection.Rows(i).Cells(14).Value) <> 0 Then
                    dgv_Details.Rows(n).Cells(7).Value = dgv_Selection.Rows(i).Cells(14).Value
                Else
                    dgv_Details.Rows(n).Cells(7).Value = dgv_Selection.Rows(i).Cells(8).Value
                End If


            End If

        Next

        Total_Calculation()

        pnl_Back.Enabled = True
        '  pnl_Back.Visible = True
        pnl_Selection.Visible = False
        If txt_BookNo.Enabled And txt_BookNo.Visible Then txt_BookNo.Focus()

    End Sub

    Private Sub Printing_Format6(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim EntryCode As String
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ItmNm1 As String, ItmNm2 As String
        Dim CntNm1 As String, CntNm2 As String
        'Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim PS As Printing.PaperSize

        Dim PCnt As Integer = 0, PrntCnt As Integer = 0
        Dim TpMargin As Single = 0


        PrntCnt = 1
        If Val(Common_Procedures.settings.YarnDelivery_Print_2Copy_In_SinglePage) = 1 Then
            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                    PS = PrintDocument1.PrinterSettings.PaperSizes(I)
                    PrintDocument1.DefaultPageSettings.PaperSize = PS
                    e.PageSettings.PaperSize = PS
                    Exit For
                End If
            Next


        Else

            'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            '    PS = PrintDocument1.PrinterSettings.PaperSizes(I)
            '    Debug.Print(PS.PaperName)
            '    If PS.Width = 800 And PS.Height = 600 Then
            '        PrintDocument1.DefaultPageSettings.PaperSize = PS
            '        e.PageSettings.PaperSize = PS
            '        PpSzSTS = True
            '        Exit For
            '    End If
            'Next

            'If PpSzSTS = False Then

            'If PpSzSTS = False Then
            '    For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            '        If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
            '            PS = PrintDocument1.PrinterSettings.PaperSizes(I)
            '            PrintDocument1.DefaultPageSettings.PaperSize = PS
            '            e.PageSettings.PaperSize = PS
            '            Exit For
            '        End If
            '    Next


            'Else
            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A5 Then
                    PS = PrintDocument1.PrinterSettings.PaperSizes(I)
                    PrintDocument1.DefaultPageSettings.PaperSize = PS
                    e.PageSettings.PaperSize = PS
                    PpSzSTS = True
                    Exit For
                End If
            Next
            'End If

            'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            '    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
            '        PS = PrintDocument1.PrinterSettings.PaperSizes(I)
            '        PrintDocument1.DefaultPageSettings.PaperSize = PS
            '        e.PageSettings.PaperSize = PS
            '        PpSzSTS = True
            '        Exit For
            '    End If
            'Next



        End If
        'End If
        If Val(Common_Procedures.settings.YarnDelivery_Print_2Copy_In_SinglePage) = 1 Then
            If PrntCnt2ndPageSTS = False Then
                PrntCnt = 2
            End If
        End If


        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 10
            .Right = 42
            .Top = 30
            .Bottom = 30
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pFont = New Font("Calibri", 8, FontStyle.Regular)

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


        If (Trim(UCase(Common_Procedures.settings.CustomerCode))) = "1288" Then
            NoofItems_PerPage = 32 ' 6 ' 5
        Else
            NoofItems_PerPage = 15 ' 6 ' 5
        End If


        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = 30 : ClArr(2) = 45 : ClArr(3) = 65 : ClArr(4) = 140 : ClArr(5) = 100 : ClArr(6) = 40 : ClArr(7) = 40
        ClArr(8) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7))

        TxtHgt = 18 ' 18.8  ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        PrntCnt2ndPageSTS = False
        TpMargin = TMargin

        For PCnt = 1 To PrntCnt
            If Val(Common_Procedures.settings.YarnDelivery_Print_2Copy_In_SinglePage) = 1 Then
                If PCnt = 1 Then
                    prn_PageNo = 0

                    prn_DetIndx = 0
                    prn_DetSNo = 0
                    ' prn_NoofBmDets = 0
                    TpMargin = TMargin

                Else

                    prn_PageNo = 0
                    ' prn_NoofBmDets = 0
                    prn_DetIndx = 0
                    prn_DetSNo = 0

                    TpMargin = 580 + TMargin  ' 600 + TMargin

                End If
            End If
            Try

                If prn_HdDt.Rows.Count > 0 Then

                    Printing_Format6_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TpMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                    Try

                        NoofDets = 0

                        CurY = CurY - 10

                        If prn_DetDt.Rows.Count > 0 Then

                            Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                                If NoofDets >= NoofItems_PerPage Then
                                    CurY = CurY + TxtHgt

                                    Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                                    NoofDets = NoofDets + 1

                                    Printing_Format6_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

                                    e.HasMorePages = True
                                    Return

                                End If

                                prn_DetSNo = prn_DetSNo + 1

                                ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Mill_Name").ToString)
                                ItmNm2 = ""
                                If Len(ItmNm1) > 18 Then
                                    For I = 18 To 1 Step -1
                                        If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                                    Next I
                                    If I = 0 Then I = 18
                                    ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                                    ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                                End If


                                CntNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Count_Name").ToString)
                                CntNm2 = ""
                                If Len(CntNm1) > 10 Then
                                    For I = 18 To 1 Step -1
                                        If Mid$(Trim(CntNm1), I, 1) = " " Or Mid$(Trim(CntNm1), I, 1) = "," Or Mid$(Trim(CntNm1), I, 1) = "." Or Mid$(Trim(CntNm1), I, 1) = "-" Or Mid$(Trim(CntNm1), I, 1) = "/" Or Mid$(Trim(CntNm1), I, 1) = "_" Or Mid$(Trim(CntNm1), I, 1) = "(" Or Mid$(Trim(CntNm1), I, 1) = ")" Or Mid$(Trim(CntNm1), I, 1) = "\" Or Mid$(Trim(CntNm1), I, 1) = "[" Or Mid$(Trim(CntNm1), I, 1) = "]" Or Mid$(Trim(CntNm1), I, 1) = "{" Or Mid$(Trim(CntNm1), I, 1) = "}" Then Exit For
                                    Next I
                                    If I = 0 Then I = 10
                                    CntNm2 = Microsoft.VisualBasic.Right(Trim(CntNm1), Len(CntNm1) - I)
                                    CntNm1 = Microsoft.VisualBasic.Left(Trim(CntNm1), I - 1)
                                End If


                                CurY = CurY + TxtHgt

                                Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Sl_No").ToString), LMargin + 25, CurY, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Yarn_Type").ToString, LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                                If IsDBNull(prn_DetDt.Rows(prn_DetIndx).Item("Set_No").ToString) = False Then
                                    If Trim(prn_DetDt.Rows(prn_DetIndx).Item("Set_No").ToString) <> "" Then
                                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Set_no").ToString), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                                    End If
                                End If
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)
                                'Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Count_Name").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 10, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Trim(CntNm1), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 10, CurY, 0, 0, pFont)
                                If Val(prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString) <> 0 Then
                                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)
                                End If
                                If Val(prn_DetDt.Rows(prn_DetIndx).Item("Cones").ToString) <> 0 Then
                                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Cones").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
                                End If
                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Weight").ToString), "########0.000"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)

                                NoofDets = NoofDets + 1

                                If Trim(ItmNm2) <> "" Then
                                    CurY = CurY + TxtHgt - 5
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(CntNm2), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 10, CurY, 0, 0, pFont)
                                    NoofDets = NoofDets + 1
                                End If



                                prn_DetIndx = prn_DetIndx + 1

                            Loop

                        End If

                        Printing_Format6_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)

                    Catch ex As Exception

                        MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                    End Try

                End If

            Catch ex As Exception

                MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End Try
            If Val(Common_Procedures.settings.YarnDelivery_Print_2Copy_In_SinglePage) = 1 Then
                If PCnt = 1 And PrntCnt = 2 And PrntCnt2ndPageSTS = False Then
                    If prn_DetDt.Rows.Count > 4 Then
                        PrntCnt2ndPageSTS = True
                        e.HasMorePages = True
                        Return
                    End If
                End If
            End If
            PrntCnt2ndPageSTS = False

        Next PCnt
LOOP2:

        prn_Count = prn_Count + 1

        e.HasMorePages = False

        If Val(prn_TotCopies) > 1 Then
            If prn_Count < Val(prn_TotCopies) Then

                prn_DetIndx = 0
                prn_PageNo = 0
                prn_DetIndx = 0
                prn_PageNo = 0
                'prn_NoofBmDets = 0


                e.HasMorePages = True
                Return

            End If

        End If


    End Sub

    Private Sub Printing_Format6_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim pFontBold As Font = New Font("Calibri", 8, FontStyle.Bold)
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String, Cmp_Add3 As String, Cmp_Add4 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_Email As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim p1Font As Font
        Dim strHeight As Single = 0, strWidth As Single = 0
        Dim W1 As Single, C1 As Single, S1 As Single, K1 As Single, M1 As Single
        Dim CurX As Single = 0
        Dim Hsn_Code As String = ""
        Dim Led_Name As String, Led_Add1 As String, Led_Add2 As String, Led_Add3 As String, Led_Add4 As String
        Dim Led_GstNo As String

        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Mill_Name, c.Count_Name  from SizingSoft_Yarn_Delivery_Details a INNER JOIN Mill_Head b on a.Mill_IdNo = b.Mill_IdNo LEFT OUTER JOIN Count_Head c on a.Count_IdNo = c.Count_IdNo where a.Yarn_Delivery_Code = '" & Trim(EntryCode) & "' and c.Count_Name = '" & Trim(dgv_Details.Rows(0).Cells(1).Value) & "' Order by a.sl_no", con)
        da2.Fill(dt2)
        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)

        End If
        dt2.Clear()
        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Mill_Name, c.*  from SizingSoft_Yarn_Delivery_Details a INNER JOIN Mill_Head b on a.Mill_IdNo = b.Mill_IdNo LEFT OUTER JOIN Count_Head c on a.Count_IdNo = c.Count_IdNo where a.Yarn_Delivery_Code = '" & Trim(EntryCode) & "' and c.Count_Name = '" & Trim(dgv_Details.Rows(0).Cells(1).Value) & "' Order by a.sl_no", con)
        da2.Fill(dt3)
        If dt3.Rows.Count > 0 Then

            Hsn_Code = dt3.Rows(0).Item("Count_Hsn_Code").ToString
        End If
        dt3.Clear()

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = "" : Cmp_Add3 = "" : Cmp_Add4 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_Email = ""
        Cmp_StateCap = "" : Cmp_StateNm = "" : Cmp_StateCode = "" : Cmp_GSTIN_Cap = "" : Cmp_GSTIN_No = ""
        Led_Name = "" : Led_Add1 = "" : Led_Add2 = "" : Led_Add3 = "" : Led_Add4 = "" : Led_GstNo = ""

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString
        Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address2").ToString
        Cmp_Add3 = prn_HdDt.Rows(0).Item("Company_Address3").ToString
        Cmp_Add4 = prn_HdDt.Rows(0).Item("Company_Address4").ToString

        Led_Name = prn_HdDt.Rows(0).Item("Ledger_MainName").ToString
        Led_Add1 = prn_HdDt.Rows(0).Item("Ledger_Address1").ToString
        Led_Add2 = prn_HdDt.Rows(0).Item("Ledger_Address2").ToString
        Led_Add3 = prn_HdDt.Rows(0).Item("Ledger_Address3").ToString
        Led_Add4 = prn_HdDt.Rows(0).Item("Ledger_Address4").ToString

        If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "PHONE NO.:" & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
            Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString) <> "" Then
            Cmp_CstNo = "CST NO.: " & prn_HdDt.Rows(0).Item("Company_CstNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
            Cmp_Email = "E-mail : " & Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString)
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
            Cmp_GSTIN_No = "GSTIN : " & prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then

            Led_GstNo = "GSTIN : " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString
        End If


        CurY = CurY + TxtHgt - 10
        M1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 30
        If (Common_Procedures.settings.CustomerCode) = "1282" Then
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Else
            p1Font = New Font("Calibri", 14, FontStyle.Bold)
        End If
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin + 10, CurY, 0, 0, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height
        Common_Procedures.Print_To_PrintDocument(e, "YARN SIZED TO :", LMargin + M1 + 10, CurY, 0, 0, p1Font)

        ' p1Font = New Font("Calibri", 9, FontStyle.Regular)
        p1Font = New Font("Calibri", 8, FontStyle.Bold)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin + 10, CurY, 0, 0, p1Font)
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "M/s." & Led_Name, LMargin + M1 + 10, CurY, 0, 0, p1Font)
        CurY = CurY + TxtHgt
        p1Font = New Font("Calibri", 8, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, Led_Add1, LMargin + M1 + 10, CurY, 0, 0, p1Font)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add3, LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, Led_Add2, LMargin + M1 + 10, CurY, 0, 0, p1Font)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add4, LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, Led_Add3, LMargin + M1 + 10, CurY, 0, 0, p1Font)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, Led_Add4, LMargin + M1 + 10, CurY, 0, 0, p1Font)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, Led_GstNo, LMargin + M1 + 10, CurY, 0, 0, p1Font)
        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY
        e.Graphics.DrawLine(Pens.Black, LMargin + M1, LnAr(2), LMargin + M1, LnAr(1))


        CurY = CurY + TxtHgt - 12
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "YARN DELIVERY NOTE", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


        Try

            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4)
            W1 = e.Graphics.MeasureString("BOOK NO            : ", pFont).Width
            K1 = e.Graphics.MeasureString("SIZING BOOK NO : ", pFont).Width
            S1 = e.Graphics.MeasureString("TO    :", pFont).Width

            CurY = CurY + TxtHgt

            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "DC.NO", LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("yarn_Delivery_No").ToString, LMargin + W1 + 25, CurY, 0, 0, p1Font)



            Common_Procedures.Print_To_PrintDocument(e, "Vehicle No", LMargin + M1 + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vehicle_No").ToString, LMargin + M1 + W1 + 25, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + 10, CurY, 0, 0, pFontBold)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFontBold)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("yarn_Delivery_Date").ToString), "dd-MM-yyyy"), LMargin + W1 + 25, CurY, 0, 0, pFontBold)
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Entry_Time_Text").ToString, PageWidth, CurY, 1, 0, pFont)

            'Common_Procedures.Print_To_PrintDocument(e, "APPROX VALUE", LMargin + M1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Approx_Value").ToString, LMargin + M1 + W1 + 25, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "TIME", LMargin + M1 + 10, CurY, 0, 0, pFontBold)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFontBold)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Entry_Time_Text").ToString, LMargin + M1 + W1 + 25, CurY, 0, 0, pFontBold)

            CurY = CurY + TxtHgt

            'Common_Procedures.Print_To_PrintDocument(e, "HSN CODE", LMargin + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, (Hsn_Code), LMargin + W1 + 25, CurY, 0, 0, pFont)

            'Common_Procedures.Print_To_PrintDocument(e, "E.REF", LMargin + M1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Electronic_Reference_No").ToString, LMargin + M1 + W1 + 25, CurY, 0, 0, pFont)
            'CurY = CurY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, "SAC CODE", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " 998821", LMargin + W1 + 25, CurY, 0, 0, pFont)

            'Common_Procedures.Print_To_PrintDocument(e, "TRANSPORT", LMargin + M1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Transport_Name").ToString, LMargin + M1 + W1 + 25, CurY, 0, 0, pFont)

            'CurY = CurY + TxtHgt

            'Common_Procedures.Print_To_PrintDocument(e, "SIZING BOOK NO", LMargin + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Book_No").ToString, LMargin + W1 + 25, CurY, 0, 0, pFont)

            'Common_Procedures.Print_To_PrintDocument(e, "TEXTILE DC NO", LMargin + M1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Textile_Dc_No").ToString, LMargin + M1 + W1 + 25, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY


            CurY = CurY + TxtHgt - 12
            Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "TYPE", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "SET NO", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "MILL NAME", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "COUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "BAGS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "CONES", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format6_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim I As Integer
        Dim Cmp_Name As String
        Dim C1 As Single
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Del_Add1 As String = "", Del_Add2 As String = "", nGST_No As String = ""
        Dim Cmp_UserName As String = "", Cmp_Divi As String = ""
        Dim Rmks1 As String
        Dim Rmks2 As String

        Try

            For I = NoofDets + 1 To NoofItems_PerPage
                CurY = CurY + TxtHgt
            Next

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

            CurY = CurY + TxtHgt - 10

            p1Font = New Font("Calibri", 9, FontStyle.Bold)

            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, " TOTAL", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), p1Font)

                If Val(prn_HdDt.Rows(0).Item("Total_Bags").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Bags").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, p1Font)
                End If
                If Val(prn_HdDt.Rows(0).Item("Total_Cones").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Cones").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, p1Font)
                End If
                If Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString), "#########0.000"), PageWidth - 10, CurY, 1, 0, p1Font)
                End If
            End If


            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))
            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4)
            CurY = CurY + TxtHgt - 12

            p1Font = New Font("Calibri", 10, FontStyle.Bold)

            If Trim(Common_Procedures.settings.CustomerCode) = "1282" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1288" Then

                If Common_Procedures.Vendor_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("Vendor_IdNo").ToString)) <> "" Then
                    da = New SqlClient.SqlDataAdapter("SELECT a.* From Vendor_Head a Where a.Vendor_IdNo = " & Val(prn_HdDt.Rows(0).Item("Vendor_IdNo").ToString) & " ", con)
                    dt = New DataTable
                    da.Fill(dt)


                    If dt.Rows.Count > 0 Then
                        Del_Add1 = dt.Rows(0).Item("Vendor_address1").ToString & "," & dt.Rows(0).Item("Vendor_Address2").ToString
                        Del_Add2 = dt.Rows(0).Item("Vendor_address3").ToString & "," & dt.Rows(0).Item("Vendor_Address4").ToString
                        If Trim(dt.Rows(0).Item("GST_No").ToString) <> "" Then nGST_No = "GSTIN : " & dt.Rows(0).Item("GST_No").ToString
                    End If


                    Common_Procedures.Print_To_PrintDocument(e, "DELIVERY TO   : " & Common_Procedures.Vendor_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("Vendor_IdNo").ToString)), LMargin + 10, CurY, 0, 0, p1Font)
                    p1Font = New Font("Calibri", 8, FontStyle.Bold)
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, " " & Trim(Del_Add1), LMargin + 30, CurY, 0, 0, p1Font)
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, " " & Trim(Del_Add2), LMargin + 30, CurY, 0, 0, p1Font)
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, " " & Trim(nGST_No), LMargin + 30, CurY, 0, 0, p1Font)


                    'Del_Add1 = prn_HdDt.Rows(0).Item("Ledger_Address1").ToString & "," & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString
                    'Del_Add2 = prn_HdDt.Rows(0).Item("Ledger_Address3").ToString & "," & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString


                    'Common_Procedures.Print_To_PrintDocument(e, "DELIVERY TO   : " & prn_HdDt.Rows(0).Item("Ledger_MainName").ToString, LMargin + 10, CurY, 0, 0, pFont)
                    'CurY = CurY + TxtHgt
                    'Common_Procedures.Print_To_PrintDocument(e, " " & Trim(Del_Add1), LMargin + 30, CurY, 0, 0, pFont)
                    'CurY = CurY + TxtHgt
                    'Common_Procedures.Print_To_PrintDocument(e, " " & Trim(Del_Add2), LMargin + 30, CurY, 0, 0, pFont)

                Else

                    Del_Add1 = prn_HdDt.Rows(0).Item("DelAdd1").ToString & "," & prn_HdDt.Rows(0).Item("DelAdd2").ToString
                    Del_Add2 = prn_HdDt.Rows(0).Item("DelAdd3").ToString & "," & prn_HdDt.Rows(0).Item("DelAdd4").ToString

                    Common_Procedures.Print_To_PrintDocument(e, "DELIVERY TO   : " & prn_HdDt.Rows(0).Item("DelName").ToString, LMargin + 10, CurY, 0, 0, pFont)
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, " " & Trim(Del_Add1), LMargin + 30, CurY, 0, 0, pFont)
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, " " & Trim(Del_Add2), LMargin + 30, CurY, 0, 0, pFont)
                End If

            Else

                Del_Add1 = prn_HdDt.Rows(0).Item("DelAdd1").ToString & "," & prn_HdDt.Rows(0).Item("DelAdd2").ToString
                Del_Add2 = prn_HdDt.Rows(0).Item("DelAdd3").ToString & "," & prn_HdDt.Rows(0).Item("DelAdd4").ToString


                Common_Procedures.Print_To_PrintDocument(e, "DELIVERY TO   : " & prn_HdDt.Rows(0).Item("DelName").ToString, LMargin + 10, CurY, 0, 0, p1Font)
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, " " & Trim(Del_Add1), LMargin + 30, CurY, 0, 0, pFont)
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, " " & Trim(Del_Add2), LMargin + 30, CurY, 0, 0, pFont)
                If Trim(prn_HdDt.Rows(0).Item("DelGSTinNo").ToString) <> "" Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, " GSTIN " & prn_HdDt.Rows(0).Item("DelGSTinNo").ToString, LMargin + 10, CurY, 0, 0, pFont)
                End If

            End If


            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            CurY = CurY + TxtHgt



            Rmks1 = Trim(prn_HdDt.Rows(0).Item("Remarks").ToString)
            Rmks2 = ""
            If Len(Rmks1) > 70 Then
                For I = 70 To 1 Step -1
                    If Mid$(Trim(Rmks1), I, 1) = " " Or Mid$(Trim(Rmks1), I, 1) = "," Or Mid$(Trim(Rmks1), I, 1) = "." Or Mid$(Trim(Rmks1), I, 1) = "-" Or Mid$(Trim(Rmks1), I, 1) = "/" Or Mid$(Trim(Rmks1), I, 1) = "_" Or Mid$(Trim(Rmks1), I, 1) = "(" Or Mid$(Trim(Rmks1), I, 1) = ")" Or Mid$(Trim(Rmks1), I, 1) = "\" Or Mid$(Trim(Rmks1), I, 1) = "[" Or Mid$(Trim(Rmks1), I, 1) = "]" Or Mid$(Trim(Rmks1), I, 1) = "{" Or Mid$(Trim(Rmks1), I, 1) = "}" Then Exit For
                Next I
                If I = 0 Then I = 70
                Rmks2 = Microsoft.VisualBasic.Right(Trim(Rmks1), Len(Rmks1) - I)
                Rmks1 = Microsoft.VisualBasic.Left(Trim(Rmks1), I - 1)
            End If

            If Trim(prn_HdDt.Rows(0).Item("Remarks").ToString) <> "" Then
                e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
                'Common_Procedures.Print_To_PrintDocument(e, " Remarks : " & Trim(prn_HdDt.Rows(0).Item("Remarks").ToString), LMargin + 10, CurY, 0, 0, p1Font)
                Common_Procedures.Print_To_PrintDocument(e, "Remarks : " & Rmks1, LMargin + 10, CurY, 0, 0, p1Font)

                If Trim(Rmks2) <> "" Then
                    CurY = CurY + TxtHgt - 5
                    Common_Procedures.Print_To_PrintDocument(e, Trim(Rmks2), LMargin + 10, CurY, 0, 0, p1Font)
                    NoofDets = NoofDets + 1
                End If


                CurY = CurY + TxtHgt
                e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            End If


            'Common_Procedures.Print_To_PrintDocument(e, "DELIVERY TO   : " & prn_HdDt.Rows(0).Item("DelName").ToString, LMargin + 10, CurY, 0, 0, pFont)
            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelAdd1").ToString, LMargin + 30, CurY, 0, 0, pFont)
            If Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, " Empty Beam : " & Trim(prn_HdDt.Rows(0).Item("Empty_Beam").ToString), LMargin + C1 + 10, CurY, 0, 0, pFont)
            End If
            'Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelAdd2").ToString, LMargin + 30, CurY, 0, 0, pFont)
            If Val(prn_HdDt.Rows(0).Item("Empty_Bags").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, " Empty Bags : " & Trim(prn_HdDt.Rows(0).Item("Empty_Bags").ToString), LMargin + C1 + 10, CurY, 0, 0, pFont)
            End If

            'Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelAdd3").ToString, LMargin + 30, CurY, 0, 0, pFont)
            If Val(prn_HdDt.Rows(0).Item("Empty_Cones").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, " Empty Cones : " & Trim(prn_HdDt.Rows(0).Item("Empty_Cones").ToString), LMargin + C1 + 10, CurY, 0, 0, pFont)
            End If
            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelAdd4").ToString, LMargin + 30, CurY, 0, 0, pFont)


            p1Font = New Font("Calibri", 6.5, FontStyle.Regular)
            'CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "This is to certify that there is no sale indicated in this delivery and the yarn sized is returned back to party after warping and sizing job work.", LMargin + 5, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(7) = CurY

            '
            If Val(Common_Procedures.User.IdNo) <> 1 Then
                Cmp_UserName = Trim(Common_Procedures.User.Name)
                ' Cmp_UserName = Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
            End If
            If Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString) <> "" Then
                Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
                'Cmp_Name = Replace(Cmp_Name, Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString), "")
            Else
                Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            End If


            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, LMargin + PageWidth - 20, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt - 5
            Cmp_Divi = Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Divi, PageWidth - 60, CurY, 1, 0, p1Font)


            CurY = CurY + TxtHgt - 5
            Common_Procedures.Print_To_PrintDocument(e, Cmp_UserName, PageWidth - 60, CurY, 1, 0, p1Font)


            CurY = CurY + TxtHgt
            If Trim(Common_Procedures.settings.CustomerCode) = "1288" Then
                Common_Procedures.Print_To_PrintDocument(e, "Receiver's ", LMargin + 20, CurY, 0, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 20, CurY, 0, 0, pFont)
            End If

            If Trim(Common_Procedures.settings.CustomerCode) = "1288" Then
                Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 120, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "Checked By  ", LMargin + 250, CurY, 0, 0, pFont)

            ElseIf Trim(Common_Procedures.settings.CustomerCode) = "1282" Then
                Common_Procedures.Print_To_PrintDocument(e, "Checked By ", LMargin + 200, CurY, 0, 0, pFont)

            Else
                Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 250, CurY, 0, 0, pFont)
            End If





            'e.Graphics.DrawLine(Pens.Black, PageWidth, CurY, PageWidth, LnAr(1))

            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format7(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim EntryCode As String
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ItmNm1 As String, ItmNm2 As String
        Dim CtmNm1 As String, CtmNm2 As String
        'Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim PS As Printing.PaperSize

        Dim PCnt As Integer = 0, PrntCnt As Integer = 0
        Dim TpMargin As Single = 0


        PrntCnt = 1
        If Val(Common_Procedures.settings.YarnDelivery_Print_2Copy_In_SinglePage) = 1 Then
            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                    PS = PrintDocument1.PrinterSettings.PaperSizes(I)
                    PrintDocument1.DefaultPageSettings.PaperSize = PS
                    e.PageSettings.PaperSize = PS
                    Exit For
                End If
            Next


        Else

            If PpSzSTS = False Then
                For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                        PS = PrintDocument1.PrinterSettings.PaperSizes(I)
                        PrintDocument1.DefaultPageSettings.PaperSize = PS
                        e.PageSettings.PaperSize = PS
                        Exit For
                    End If
                Next


            Else
                For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A5 Then
                        PS = PrintDocument1.PrinterSettings.PaperSizes(I)
                        PrintDocument1.DefaultPageSettings.PaperSize = PS
                        e.PageSettings.PaperSize = PS
                        PpSzSTS = True
                        Exit For
                    End If
                Next
            End If

        End If

        If Val(Common_Procedures.settings.YarnDelivery_Print_2Copy_In_SinglePage) = 1 Then
            If PrntCnt2ndPageSTS = False Then
                PrntCnt = 2
            End If
        End If

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 10
            .Right = 30
            .Top = 30
            .Bottom = 30
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pFont = New Font("Calibri", 8, FontStyle.Regular)

        e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        With PrintDocument1.DefaultPageSettings.PaperSize
            PrintWidth = .Width - RMargin - LMargin
            PrintHeight = .Height - TMargin - BMargin
            PageWidth = .Width - RMargin
            PageHeight = .Height - BMargin
        End With

        If Common_Procedures.settings.CustomerCode = "1288" Then
            PrintDocument1.DefaultPageSettings.Landscape = True
        End If

        If PrintDocument1.DefaultPageSettings.Landscape = True Then
            With PrintDocument1.DefaultPageSettings.PaperSize
                PrintWidth = .Height - TMargin - BMargin
                PrintHeight = .Width - RMargin - LMargin
                PageWidth = .Height - TMargin
                PageHeight = .Width - RMargin
            End With
        End If


        If (Trim(UCase(Common_Procedures.settings.CustomerCode))) = "1288" Then
            NoofItems_PerPage = 13 ' 6 ' 5
        Else
            NoofItems_PerPage = 4 ' 6 ' 5
        End If

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = 30
        ClArr(2) = 40 : ClArr(3) = 40 : ClArr(4) = 250 : ClArr(5) = 160 : ClArr(6) = 60 : ClArr(7) = 60 : ClArr(8) = 60
        ClArr(9) = 80 : ClArr(10) = 40
        'ClArr(11) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10))

        ClArr(11) = 100

        ClArr(12) = 95
        ClArr(13) = 95
        'ClArr(13) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) + ClArr(11) + ClArr(12))






        TxtHgt = 18 ' 18.8  ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        PrntCnt2ndPageSTS = False
        TpMargin = TMargin

        For PCnt = 1 To PrntCnt
            If Val(Common_Procedures.settings.YarnDelivery_Print_2Copy_In_SinglePage) = 1 Then
                If PCnt = 1 Then
                    prn_PageNo = 0

                    prn_DetIndx = 0
                    prn_DetSNo = 0
                    ' prn_NoofBmDets = 0
                    TpMargin = TMargin

                Else

                    prn_PageNo = 0
                    ' prn_NoofBmDets = 0
                    prn_DetIndx = 0
                    prn_DetSNo = 0

                    TpMargin = 580 + TMargin  ' 600 + TMargin

                End If
            End If


            Try

                If prn_HdDt.Rows.Count > 0 Then

                    Printing_Format7_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TpMargin, BMargin, PageWidth - 50, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                    Try

                        NoofDets = 0

                        CurY = CurY - 10

                        If prn_DetDt.Rows.Count > 0 Then

                            sum_Total_Amount = 0

                            Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                                If NoofDets >= NoofItems_PerPage Then
                                    CurY = CurY + TxtHgt

                                    Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                                    NoofDets = NoofDets + 1

                                    Printing_Format7_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth - 50, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

                                    e.HasMorePages = True
                                    Return

                                End If

                                prn_DetSNo = prn_DetSNo + 1

                                ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Mill_Name").ToString)
                                ItmNm2 = ""
                                If Len(ItmNm1) > 35 Then
                                    For I = 35 To 1 Step -1
                                        If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                                    Next I
                                    If I = 0 Then I = 35
                                    ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                                    ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                                End If

                                CtmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Count_Name").ToString)
                                CtmNm2 = ""
                                If Len(CtmNm1) > 10 Then
                                    For I = 10 To 1 Step -1
                                        If Mid$(Trim(CtmNm1), I, 1) = " " Or Mid$(Trim(CtmNm1), I, 1) = "," Or Mid$(Trim(CtmNm1), I, 1) = "." Or Mid$(Trim(CtmNm1), I, 1) = "-" Or Mid$(Trim(CtmNm1), I, 1) = "/" Or Mid$(Trim(CtmNm1), I, 1) = "_" Or Mid$(Trim(CtmNm1), I, 1) = "(" Or Mid$(Trim(CtmNm1), I, 1) = ")" Or Mid$(Trim(CtmNm1), I, 1) = "\" Or Mid$(Trim(CtmNm1), I, 1) = "[" Or Mid$(Trim(CtmNm1), I, 1) = "]" Or Mid$(Trim(CtmNm1), I, 1) = "{" Or Mid$(Trim(CtmNm1), I, 1) = "}" Then Exit For
                                    Next I
                                    If I = 0 Then I = 10
                                    CtmNm2 = Microsoft.VisualBasic.Right(Trim(CtmNm1), Len(CtmNm1) - I)
                                    CtmNm1 = Microsoft.VisualBasic.Left(Trim(CtmNm1), I - 1)
                                End If


                                CurY = CurY + TxtHgt



                                Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Sl_No").ToString), LMargin + 25, CurY, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Yarn_Type").ToString, LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                                If IsDBNull(prn_DetDt.Rows(prn_DetIndx).Item("Set_No").ToString) = False Then
                                    If Trim(prn_DetDt.Rows(prn_DetIndx).Item("Set_No").ToString) <> "" Then
                                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Set_no").ToString), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                                    End If
                                End If
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)

                                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Count_Name").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 5, CurY, 0, 0, pFont)

                                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Count_Hsn_Code").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + 10, CurY, 0, 0, pFont)

                                If Val(prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString) <> 0 Then
                                    Dim infor As String = Val(prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString) & " " & Common_Procedures.Bag_Type_IdNoToName(con, prn_DetDt.Rows(prn_DetIndx).Item("BagType_IdNo"))
                                    Common_Procedures.Print_To_PrintDocument(e, infor, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
                                    'Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("BagsRate").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
                                End If
                                If Val(prn_DetDt.Rows(prn_DetIndx).Item("Cones").ToString) <> 0 Then
                                    Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Cones").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)
                                End If

                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Weight").ToString), "########0.000"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) - 10, CurY, 1, 0, pFont)

                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("rate").ToString), "##0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) - 10, CurY, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Amount").ToString), "########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) + ClArr(11) - 10, CurY, 1, 0, pFont)

                                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Lot_No").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) + ClArr(11) + 10, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Ledger_IdNoToName(con, Val(prn_DetDt.Rows(prn_DetIndx).Item("Location_IdNo").ToString)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) + ClArr(11) + ClArr(12) + 10, CurY, 0, 0, pFont)




                                sum_Total_Amount += Val(prn_DetDt.Rows(prn_DetIndx).Item("Amount").ToString)


                                NoofDets = NoofDets + 1

                                'If Trim(ItmNm2) <> "" Or Trim(CtmNm2) <> "" Then
                                '    CurY = CurY + TxtHgt - 5
                                '    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)
                                '    NoofDets = NoofDets + 1
                                '    Common_Procedures.Print_To_PrintDocument(e, Trim(CtmNm2), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 5, CurY, 0, 0, pFont)
                                '    NoofDets = NoofDets + 1
                                'End If

                                prn_DetIndx = prn_DetIndx + 1

                            Loop

                        End If

                        Printing_Format7_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth - 50, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)

                    Catch ex As Exception

                        MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                    End Try

                End If

            Catch ex As Exception

                MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End Try
            If Val(Common_Procedures.settings.YarnDelivery_Print_2Copy_In_SinglePage) = 1 Then
                If PCnt = 1 And PrntCnt = 2 And PrntCnt2ndPageSTS = False Then
                    If prn_DetDt.Rows.Count > 4 Then
                        PrntCnt2ndPageSTS = True
                        e.HasMorePages = True
                        Return
                    End If
                End If
            End If
            PrntCnt2ndPageSTS = False

        Next PCnt
LOOP2:

        prn_Count = prn_Count + 1

        e.HasMorePages = False

        If Val(prn_TotCopies) > 1 Then
            If prn_Count < Val(prn_TotCopies) Then

                prn_DetIndx = 0
                prn_PageNo = 0
                prn_DetIndx = 0
                prn_PageNo = 0
                'prn_NoofBmDets = 0


                e.HasMorePages = True
                Return

            End If

        End If


    End Sub

    Private Sub Printing_Format7_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String, Cmp_Add3 As String, Cmp_Add4 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_Email As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim p1Font As Font
        Dim strHeight As Single = 0, strWidth As Single = 0
        Dim W1 As Single, C1 As Single, S1 As Single, K1 As Single, M1 As Single
        Dim CurX As Single = 0
        Dim Hsn_Code As String = ""
        Dim Led_Name As String, Led_Add1 As String, Led_Add2 As String, Led_Add3 As String, Led_Add4 As String
        Dim Led_GstNo As String

        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Mill_Name, c.Count_Name  from SizingSoft_Yarn_Delivery_Details a INNER JOIN Mill_Head b on a.Mill_IdNo = b.Mill_IdNo LEFT OUTER JOIN Count_Head c on a.Count_IdNo = c.Count_IdNo where a.Yarn_Delivery_Code = '" & Trim(EntryCode) & "' and c.Count_Name = '" & Trim(dgv_Details.Rows(0).Cells(1).Value) & "' Order by a.sl_no", con)
        da2.Fill(dt2)
        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)

        End If
        dt2.Clear()
        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Mill_Name, c.*  from SizingSoft_Yarn_Delivery_Details a INNER JOIN Mill_Head b on a.Mill_IdNo = b.Mill_IdNo LEFT OUTER JOIN Count_Head c on a.Count_IdNo = c.Count_IdNo where a.Yarn_Delivery_Code = '" & Trim(EntryCode) & "' and c.Count_Name = '" & Trim(dgv_Details.Rows(0).Cells(1).Value) & "' Order by a.sl_no", con)
        da2.Fill(dt3)
        If dt3.Rows.Count > 0 Then

            Hsn_Code = dt3.Rows(0).Item("Count_Hsn_Code").ToString
        End If
        dt3.Clear()

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = "" : Cmp_Add3 = "" : Cmp_Add4 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_Email = ""
        Cmp_StateCap = "" : Cmp_StateNm = "" : Cmp_StateCode = "" : Cmp_GSTIN_Cap = "" : Cmp_GSTIN_No = ""
        Led_Name = "" : Led_Add1 = "" : Led_Add2 = "" : Led_Add3 = "" : Led_Add4 = "" : Led_GstNo = ""

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString
        Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address2").ToString
        Cmp_Add3 = prn_HdDt.Rows(0).Item("Company_Address3").ToString
        Cmp_Add4 = prn_HdDt.Rows(0).Item("Company_Address4").ToString

        Led_Name = prn_HdDt.Rows(0).Item("Ledger_MainName").ToString
        Led_Add1 = prn_HdDt.Rows(0).Item("Ledger_Address1").ToString
        Led_Add2 = prn_HdDt.Rows(0).Item("Ledger_Address2").ToString
        Led_Add3 = prn_HdDt.Rows(0).Item("Ledger_Address3").ToString
        Led_Add4 = prn_HdDt.Rows(0).Item("Ledger_Address4").ToString

        If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "PHONE NO.:" & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
            Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString) <> "" Then
            Cmp_CstNo = "CST NO.: " & prn_HdDt.Rows(0).Item("Company_CstNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
            Cmp_Email = "E-mail : " & Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString)
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
            Cmp_GSTIN_No = "GSTIN : " & prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then

            Led_GstNo = "GSTIN : " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString
        End If


        CurY = CurY + TxtHgt - 10
        M1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6)
        If (Common_Procedures.settings.CustomerCode) = "1282" Then
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Else
            p1Font = New Font("Calibri", 14, FontStyle.Bold)
        End If
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin + 10, CurY, 0, 0, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height
        Common_Procedures.Print_To_PrintDocument(e, "YARN SIZED TO :", LMargin + M1 + 10, CurY, 0, 0, pFont)

        ' p1Font = New Font("Calibri", 9, FontStyle.Regular)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "M/s." & Led_Name, LMargin + M1 + 10, CurY, 0, 0, p1Font, , True, PageWidth)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Led_Add1, LMargin + M1 + 10, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add3, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Led_Add2, LMargin + M1 + 10, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add4, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Led_Add3, LMargin + M1 + 10, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Led_Add4, LMargin + M1 + 10, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Led_GstNo, LMargin + M1 + 10, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY
        e.Graphics.DrawLine(Pens.Black, LMargin + M1, LnAr(2), LMargin + M1, LnAr(1))


        CurY = CurY + TxtHgt - 12
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "YARN DELIVERY NOTE", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


        Try

            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4)
            W1 = e.Graphics.MeasureString("BOOK NO            : ", pFont).Width
            K1 = e.Graphics.MeasureString("SIZING BOOK NO : ", pFont).Width
            S1 = e.Graphics.MeasureString("TO    :", pFont).Width

            CurY = CurY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, "DC.NO", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 9, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("yarn_Delivery_No").ToString, LMargin + W1 + 25, CurY, 0, 0, p1Font)



            Common_Procedures.Print_To_PrintDocument(e, "VAN NO", LMargin + M1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vehicle_No").ToString, LMargin + M1 + W1 + 25, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, "DATE & TIME", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, (prn_HdDt.Rows(0).Item("Date_And_Time_Of_Supply").ToString).ToString, LMargin + W1 + 25, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Entry_Time_Text").ToString, PageWidth, CurY, 1, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "APPROX VALUE", LMargin + M1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Approx_Value").ToString, LMargin + M1 + W1 + 25, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, "HSN CODE", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, (Hsn_Code), LMargin + W1 + 25, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "E.REF", LMargin + M1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Electronic_Reference_No").ToString, LMargin + M1 + W1 + 25, CurY, 0, 0, pFont)

            If Common_Procedures.settings.CustomerCode <> "1102" Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "SAC CODE", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " 998821", LMargin + W1 + 25, CurY, 0, 0, pFont)

                Common_Procedures.Print_To_PrintDocument(e, "TRANSPORT", LMargin + M1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Transport_Name").ToString, LMargin + M1 + W1 + 25, CurY, 0, 0, pFont)

            End If
            If Common_Procedures.settings.CustomerCode <> "1102" Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "SIZING BOOK NO", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Book_No").ToString, LMargin + W1 + 25, CurY, 0, 0, pFont)

                Common_Procedures.Print_To_PrintDocument(e, "TEXTILE DC NO", LMargin + M1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Textile_Dc_No").ToString, LMargin + M1 + W1 + 25, CurY, 0, 0, pFont)
            End If


            If Common_Procedures.settings.CustomerCode <> "1102" Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "LOCATION", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
                'Trim(Common_Procedures.Ledger_IdNoToName(con, prn_HdDt.Rows(0).Item("Location_IdNo")))
                Common_Procedures.Print_To_PrintDocument(e, Trim(Common_Procedures.Ledger_IdNoToName(con, prn_HdDt.Rows(0).Item("WareHouse_IdNo"))), LMargin + W1 + 25, CurY, 0, 0, pFont)
            End If




            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY


            CurY = CurY + TxtHgt - 12
            Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "TYPE", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "SET NO", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "MILL NAME", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "COUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "HSN CODE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "BAGS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "CONES", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "RATE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, 2, ClAr(10), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, 2, ClAr(11), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "LOTNO", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), CurY, 2, ClAr(10) + ClAr(11), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "LOCATION", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), CurY, 2, ClAr(10), pFont)


            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format7_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim I As Integer
        Dim Cmp_Name As String
        Dim C1 As Single
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Del_Add1 As String = "", Del_Add2 As String = "", nGST_No As String = ""
        Try

            For I = NoofDets + 1 To NoofItems_PerPage
                CurY = CurY + TxtHgt
            Next

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

            CurY = CurY + TxtHgt - 10
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, " TOTAL", LMargin + ClAr(3) + ClAr(4) - 40, CurY, 2, ClAr(4), pFont)

                If Val(prn_HdDt.Rows(0).Item("Total_Bags").ToString) <> 0 Then

                    'Dim infor As String = Val(prn_HdDt.Rows(0).Item("Total_Bags").ToString) & " " & Common_Procedures.Bag_Type_IdNoToName(con, prn_HdDt.Rows(0).Item("BagType_IdNo"))
                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Bags").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                End If
                If Val(prn_HdDt.Rows(0).Item("Total_Cones").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Cones").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                End If
                'sum_Total_Amount

                If Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString), "#######0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)

                End If



                If Val(sum_Total_Amount) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(sum_Total_Amount.ToString), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) - 10, CurY, 1, 0, pFont)
                    sum_Total_Amount = 0
                End If
            End If


            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

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

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), LnAr(3))

            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4)
            CurY = CurY + TxtHgt - 12

            p1Font = New Font("Calibri", 10, FontStyle.Bold)

            If Trim(Common_Procedures.settings.CustomerCode) = "1282" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1288" Then

                If Common_Procedures.Vendor_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("Vendor_IdNo").ToString)) <> "" Then
                    da = New SqlClient.SqlDataAdapter("SELECT a.* From Vendor_Head a Where a.Vendor_IdNo = " & Val(prn_HdDt.Rows(0).Item("Vendor_IdNo").ToString) & " ", con)
                    dt = New DataTable
                    da.Fill(dt)


                    If dt.Rows.Count > 0 Then
                        Del_Add1 = dt.Rows(0).Item("Vendor_address1").ToString & "," & dt.Rows(0).Item("Vendor_Address2").ToString
                        Del_Add2 = dt.Rows(0).Item("Vendor_address3").ToString & "," & dt.Rows(0).Item("Vendor_Address4").ToString
                        If Trim(dt.Rows(0).Item("GST_No").ToString) <> "" Then nGST_No = "GSTIN : " & dt.Rows(0).Item("GST_No").ToString
                    End If


                    Common_Procedures.Print_To_PrintDocument(e, "DELIVERY TO   : " & Common_Procedures.Vendor_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("Vendor_IdNo").ToString)), LMargin + 10, CurY, 0, 0, pFont)
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, " " & Trim(Del_Add1), LMargin + 30, CurY, 0, 0, pFont)
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, " " & Trim(Del_Add2), LMargin + 30, CurY, 0, 0, pFont)
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, " " & Trim(nGST_No), LMargin + 30, CurY, 0, 0, pFont)

                Else

                    Del_Add1 = prn_HdDt.Rows(0).Item("DelAdd1").ToString & "," & prn_HdDt.Rows(0).Item("DelAdd2").ToString
                    Del_Add2 = prn_HdDt.Rows(0).Item("DelAdd3").ToString & "," & prn_HdDt.Rows(0).Item("DelAdd4").ToString

                    Common_Procedures.Print_To_PrintDocument(e, "DELIVERY TO   : " & prn_HdDt.Rows(0).Item("DelName").ToString, LMargin + 10, CurY, 0, 0, pFont)
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, " " & Trim(Del_Add1), LMargin + 30, CurY, 0, 0, pFont)
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, " " & Trim(Del_Add2), LMargin + 30, CurY, 0, 0, pFont)
                End If

            Else

                Del_Add1 = prn_HdDt.Rows(0).Item("DelAdd1").ToString & "," & prn_HdDt.Rows(0).Item("DelAdd2").ToString
                Del_Add2 = prn_HdDt.Rows(0).Item("DelAdd3").ToString & "," & prn_HdDt.Rows(0).Item("DelAdd4").ToString


                Common_Procedures.Print_To_PrintDocument(e, "DELIVERY TO   : " & prn_HdDt.Rows(0).Item("DelName").ToString, LMargin + 10, CurY, 0, 0, p1Font)
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, " " & Trim(Del_Add1), LMargin + 30, CurY, 0, 0, pFont)
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, " " & Trim(Del_Add2), LMargin + 30, CurY, 0, 0, pFont)
                If Trim(prn_HdDt.Rows(0).Item("DelGSTinNo").ToString) <> "" Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, " GSTIN " & prn_HdDt.Rows(0).Item("DelGSTinNo").ToString, LMargin + 10, CurY, 0, 0, pFont)
                End If

            End If

            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            CurY = CurY + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("Remarks").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, " Remarks : " & Trim(prn_HdDt.Rows(0).Item("Remarks").ToString), LMargin + 10, CurY, 0, 0, p1Font)
            End If


            'Common_Procedures.Print_To_PrintDocument(e, "DELIVERY TO   : " & prn_HdDt.Rows(0).Item("DelName").ToString, LMargin + 10, CurY, 0, 0, pFont)
            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelAdd1").ToString, LMargin + 30, CurY, 0, 0, pFont)
            If Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, " Empty Beam : " & Trim(prn_HdDt.Rows(0).Item("Empty_Beam").ToString), LMargin + C1 + 10, CurY, 0, 0, pFont)
            End If
            'Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelAdd2").ToString, LMargin + 30, CurY, 0, 0, pFont)
            If Val(prn_HdDt.Rows(0).Item("Empty_Bags").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, " Empty Bags : " & Trim(prn_HdDt.Rows(0).Item("Empty_Bags").ToString), LMargin + C1 + 10, CurY, 0, 0, pFont)
            End If

            'Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelAdd3").ToString, LMargin + 30, CurY, 0, 0, pFont)
            If Val(prn_HdDt.Rows(0).Item("Empty_Cones").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, " Empty Cones : " & Trim(prn_HdDt.Rows(0).Item("Empty_Cones").ToString), LMargin + C1 + 10, CurY, 0, 0, pFont)
            End If
            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelAdd4").ToString, LMargin + 30, CurY, 0, 0, pFont)



            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "This is to certify that there is no sale indicated in this delivery and the yarn sized is returned back to party after warping and sizing job work.", LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(7) = CurY

            CurY = CurY + TxtHgt
            If Val(Common_Procedures.User.IdNo) <> 1 Then
                Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
            End If
            CurY = CurY + TxtHgt

            If Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString) <> "" Then
                Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
                Cmp_Name = Replace(Cmp_Name, Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString), "")

            Else
                Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString

            End If

            If Trim(Common_Procedures.settings.CustomerCode) = "1288" Then
                Common_Procedures.Print_To_PrintDocument(e, "Receiver's ", LMargin + 20, CurY, 0, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 20, CurY, 0, 0, pFont)
            End If

            If Trim(Common_Procedures.settings.CustomerCode) = "1288" Then
                Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 120, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "Checked By  ", LMargin + 250, CurY, 0, 0, pFont)

            ElseIf Trim(Common_Procedures.settings.CustomerCode) = "1282" Then
                Common_Procedures.Print_To_PrintDocument(e, "Checked By ", LMargin + 250, CurY, 0, 0, pFont)

            Else
                Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 250, CurY, 0, 0, pFont)
            End If



            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, LMargin + PageWidth - 75, CurY, 1, 0, p1Font)

            'e.Graphics.DrawLine(Pens.Black, PageWidth, CurY, PageWidth, LnAr(1))

            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Send_SMS()
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim NewCode As String

        Dim i As Integer = 0
        Dim smstxt As String = ""
        Dim PhNo As String = ""
        Dim Led_IdNo As Integer = 0

        Dim SMS_SenderID As String = ""
        Dim SMS_Key As String = ""
        Dim SMS_RouteID As String = ""
        Dim SMS_Type As String = ""
        Dim Cmp_Typ As String = ""
        Dim VndrNm_Id As Integer = 0


        Try

            Cmp_Typ = Trim(Common_Procedures.get_FieldValue(con, "Company_Head", "Company_Type", ""))

            If Common_Procedures.settings.CustomerCode = "1282" Then
                VndrNm_Id = Common_Procedures.Vendor_AlaisNameToIdNo(con, cbo_VendorName.Text)
                If VndrNm_Id <> 0 Then
                    PhNo = Common_Procedures.get_FieldValue(con, "Vendor_head", "Vendor_PhoneNo", "(Vendor_IdNo = " & Str(Val(VndrNm_Id)) & ")")
                End If

            Else
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
                'If Led_IdNo  = 0 Then Exit Sub

                PhNo = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_MobileNo", "(Ledger_IdNo = " & Str(Val(Led_IdNo)) & ")")

            End If


            smstxt = " YARN DELIVERY" & vbCrLf

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1102" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1144" Then       '---- Ganesh karthik Sizing (Somanur)
                smstxt = smstxt & vbCrLf
            End If

            If Common_Procedures.settings.CustomerCode = "1282" Then
                If Trim(cbo_VendorName.Text) <> "" Then
                    smstxt = smstxt & "VENOR NAME-" & Trim(cbo_VendorName.Text) & vbCrLf
                End If
            End If

            smstxt = smstxt & "DC.NO-" & Trim(lbl_DcNo.Text) & vbCrLf & "DATE-" & Trim(dtp_Date.Text)

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            da2 = New SqlClient.SqlDataAdapter("select a.*, b.Count_Name, c.Mill_Name from SizingSoft_Yarn_Delivery_Details a INNER JOIN Count_Head b on a.Count_IdNo = b.Count_IdNo INNER JOIN Mill_Head c on a.Mill_IdNo = c.Mill_IdNo where a.Yarn_Delivery_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
            dt2 = New DataTable
            da2.Fill(dt2)

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1102" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1144" Then       '---- Ganesh karthik Sizing (Somanur)
                        smstxt = smstxt & vbCrLf
                    End If
                    '  smstxt = smstxt & vbCrLf
                    smstxt = smstxt & vbCrLf & "Count : " & Trim(dt2.Rows(i).Item("Count_Name").ToString)
                    smstxt = smstxt & vbCrLf & "Mill : " & Trim(dt2.Rows(i).Item("Mill_Name").ToString)

                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1102" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1144" Then       '---- Ganesh karthik Sizing (Somanur)
                        smstxt = smstxt & vbCrLf & "Type : " & Trim(dt2.Rows(i).Item("Yarn_Type").ToString)
                    End If

                    If Val(dt2.Rows(i).Item("Bags").ToString) <> 0 Then
                        smstxt = smstxt & vbCrLf & "Bags : " & Trim(Val(dt2.Rows(i).Item("Bags").ToString))
                    End If

                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1102" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1144" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1282" Then       '---- Ganesh karthik Sizing (Somanur)
                        If Val(dt2.Rows(i).Item("Cones").ToString) <> 0 Then
                            smstxt = smstxt & vbCrLf & "Cones : " & Trim(dt2.Rows(i).Item("Cones").ToString)
                        End If
                    End If


                    smstxt = smstxt & vbCrLf & "Weight : " & Trim(Format(Val(dt2.Rows(i).Item("Weight").ToString), "########0.000"))

                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1102" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1144" Then       '---- Ganesh karthik Sizing (Somanur)
                        If Trim(UCase(Cmp_Typ)) = "UNACCOUNT" Then
                            smstxt = ""
                            smstxt = "DELIVERY " & vbCrLf
                            smstxt = smstxt & vbCrLf & "Bags : " & Trim(Val(dt2.Rows(i).Item("Bags").ToString))
                        End If
                    End If

                Next i

            End If
            dt2.Clear()

            smstxt = smstxt & vbCrLf & vbCrLf & "Thanks! " & vbCrLf
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1102" Then
                smstxt = smstxt & "GKT SIZING "
            Else '
                smstxt = smstxt & Common_Procedures.Company_IdNoToName(con, Val(lbl_Company.Tag))
            End If


            SMS_SenderID = ""
            SMS_Key = ""
            SMS_RouteID = ""
            SMS_Type = ""

            Common_Procedures.get_SMS_Provider_Details(con, Val(lbl_Company.Tag), SMS_SenderID, SMS_Key, SMS_RouteID, SMS_Type)

            If Common_Procedures.settings.CustomerCode = "1102" Then
                Sms_Entry.vSmsPhoneNo = Trim(PhNo) & "," & "9361188135"
            ElseIf Common_Procedures.settings.CustomerCode = "1282" Then
                Sms_Entry.vSmsPhoneNo = Trim(PhNo) & ",9344415141,9965575141"
            Else
                Sms_Entry.vSmsPhoneNo = Trim(PhNo)
            End If

            Sms_Entry.vSmsMessage = Trim(smstxt)

            Sms_Entry.SMSProvider_SenderID = SMS_SenderID
            Sms_Entry.SMSProvider_Key = SMS_Key
            Sms_Entry.SMSProvider_RouteID = SMS_RouteID
            Sms_Entry.SMSProvider_Type = SMS_Type

            Dim f1 As New Sms_Entry
            f1.MdiParent = MDIParent1
            f1.Show()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SEND SMS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub




    Private Sub cbo_grdBagType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_grdBagType.GotFocus

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Bag_Type_Head", "Bag_Type_Name", "", "(Bag_Type_IdNo = 0)")
    End Sub

    Private Sub cbo_grdBagType_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_grdBagType.KeyDown
        vcbo_KeyDwnVal = e.KeyValue

        Try
            With cbo_grdBagType
                If e.KeyValue = 38 And .DroppedDown = False Then
                    e.Handled = True
                    cbo_MillName.Focus()
                    'SendKeys.Send("+{TAB}")
                ElseIf e.KeyValue = 40 And .DroppedDown = False Then
                    e.Handled = True
                    txt_Bags.Focus()
                    'SendKeys.Send("{TAB}")
                ElseIf e.KeyValue <> 13 And e.KeyValue <> 17 And e.KeyValue <> 27 And .DroppedDown = False Then
                    .DroppedDown = True
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_grdBagType_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_grdBagType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_grdBagType, txt_Bags, "Bag_Type_Head", "Bag_Type_Name", "", "(Bag_Type_Idno = 0)")
    End Sub

    Private Sub cbo_grdBagType_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_grdBagType.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New Bag_Type_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_grdBagType.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If

    End Sub

    Private Sub txt_Rate_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Rate.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

    End Sub

    Private Sub txt_Rate_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_Rate.TextChanged
        update_amount()
    End Sub
    Private Sub update_amount()
        Dim amt As Double = 0
        Dim rate As Double = 0
        Dim wt As Double = 0

        If Val(txt_Rate.Text) <> 0 Then
            rate = Val(txt_Rate.Text)
        End If

        If Val(txt_Weight.Text) <> 0 Then
            wt = Val(txt_Weight.Text)
        End If

        txt_Amount.Text = Format(rate * wt, "#########0.00")
    End Sub

    Private Sub txt_Weight_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_Weight.TextChanged
        update_amount()
    End Sub

    Private Sub dtp_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Date.KeyDown
        If e.KeyCode = 38 Then
            If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
        End If
        If e.KeyCode = 40 Then
            If Trim(Common_Procedures.settings.CustomerCode) = "1112" Then '----VENUS SIZING
                If txt_BookNo.Enabled And txt_BookNo.Visible Then txt_BookNo.Focus()
            Else
                If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
            End If
        End If
    End Sub

    Private Sub dtp_Date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_Date.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If Trim(Common_Procedures.settings.CustomerCode) = "1112" Then '----VENUS SIZING
                If txt_BookNo.Enabled And txt_BookNo.Visible Then txt_BookNo.Focus()
            Else
                If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
            End If
        End If

    End Sub

    Private Sub btn_SaveAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_SaveAll.Click
        Dim pwd As String = ""

        Dim g As New Password
        g.ShowDialog()

        pwd = Trim(Common_Procedures.Password_Input)

        If Trim(UCase(pwd)) <> "TSSA7417" Then
            MessageBox.Show("Invalid Password", "FAILED...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        SaveAll_STS = True

        LastNo = ""
        movelast_record()

        LastNo = lbl_DcNo.Text

        movefirst_record()
        Timer1.Enabled = True


    End Sub

    Private Sub txt_BookNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_BookNo.KeyDown
        If e.KeyCode = 38 Then
            If Trim(Common_Procedures.settings.CustomerCode) = "1112" Then '----VENUS SIZING
                If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
            Else
                If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
            End If
        End If
        If e.KeyCode = 40 Then
            If Trim(Common_Procedures.settings.CustomerCode) = "1112" Then '----VENUS SIZING
                If txt_TexDcNo.Enabled And txt_TexDcNo.Visible Then txt_TexDcNo.Focus()
            Else
                If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
            End If
        End If
    End Sub

    Private Sub btn_EWayBIll_Generation_Click(sender As Object, e As EventArgs) Handles btn_EWayBIll_Generation.Click
        btn_GENERATEEWB.Enabled = True
        Grp_EWB.Visible = True
        Grp_EWB.BringToFront()
        Grp_EWB.Left = (Me.Width - pnl_Back.Width) / 2 + 160
        Grp_EWB.Top = (Me.Height - pnl_Back.Height) / 2 + 150
    End Sub

    Private Sub btn_Close_EWB_Click(sender As Object, e As EventArgs) Handles btn_Close_EWB.Click
        Grp_EWB.Visible = False
    End Sub

    Private Sub btn_CheckConnectivity_Click(sender As Object, e As EventArgs) Handles btn_CheckConnectivity.Click
        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()

        'Dim einv As New eInvoice(Val(lbl_Company.Tag))
        'einv.GetAuthToken(rtbEWBResponse)

        Dim ewb As New EWB(Val(lbl_Company.Tag))
        EWB.GetAuthToken(rtbEWBResponse)
    End Sub

    Private Sub btn_GENERATEEWB_Click(sender As Object, e As EventArgs) Handles btn_GENERATEEWB.Click
        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()

        '   Dim NewCode As String = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        Dim NewCode As String = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)


        'If Val(txt_Rate.Text) = 0 Then
        '    MessageBox.Show("Invalid Rate", "DOES NOT GENERATE EWB...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '    If txt_Rate.Enabled And txt_Rate.Visible Then txt_Rate.Focus()
        '    Exit Sub
        'End If

        Dim da As New SqlClient.SqlDataAdapter("Select Electronic_Reference_No from SizingSoft_Yarn_Delivery_Head where Yarn_Delivery_Code = '" & NewCode & "'", con)
        Dim dt As New DataTable

        da.Fill(dt)

        If dt.Rows.Count = 0 Then
            MessageBox.Show("Please Save the Invoice before proceeding to generate EWB", "Please SAVE", MessageBoxButtons.OKCancel)
            dt.Clear()
            Exit Sub
        End If

        If Not IsDBNull(dt.Rows(0).Item(0)) Then
            If Len(Trim(dt.Rows(0).Item(0))) > 0 Then
                MessageBox.Show("EWB has been generated for this invoice already", "Redundant Request", MessageBoxButtons.OKCancel)
                dt.Clear()
                Exit Sub
            End If
        End If

        dt.Clear()

        Dim CMD As New SqlClient.SqlCommand
        CMD.Connection = con



        CMD.CommandText = "Delete from EWB_Head Where InvCode = '" & NewCode & "'"
        CMD.ExecuteNonQuery()



        CMD.CommandText = "Insert into EWB_Head ([SupplyType]  ,[SubSupplyType]  , [SubSupplyDesc]  ,[DocType]  ,	[EWBGenDocNo]                           ,[EWBDocDate]        ,[FromGSTIN]       ,[FromTradeName]  ,[FromAddress1]   ,[FromAddress2]     ,[FromPlace] ," &
                         "[FromPINCode]     ,	[FromStateCode] ,[ActualFromStateCode] ,[ToGSTIN]       ,[ToTradeName],[ToAddress1]      ,[ToAddress2]    ,[ToPlace]       ,[ToPINCode]       ,[ToStateCode] , [ActualToStateCode] ," &
                         "[TransactionType],[OtherValue]                       ,	[Total_value]       ,	[CGST_Value],[SGST_Value],[IGST_Value]     ,	[CessValue],[CessNonAdvolValue],[TransporterID]    ,[TransporterName]," &
                         "[TransportDOCNo] ,[TransportDOCDate]    ,[TotalInvValue]    ,[TransMode]             ," &
                         "[VehicleNo]      ,[VehicleType]   , [InvCode], [ShippedToGSTIN], [ShippedToTradeName] ) " &
                         " " &
                         " " &
                         "  SELECT               'O'              , '4'             ,   'JOB WORK'              ,    'CHL'    , a.Yarn_Delivery_No ,a.Yarn_Delivery_Date          , C.Company_GSTINNo, C.Company_Name   ,C.Company_Address1+C.Company_Address2,c.Company_Address3+C.Company_Address4,C.Company_City ," &
                         " C.Company_PinCode    , FS.State_Code  ,FS.State_Code    ,L.Ledger_GSTINNo, L.Ledger_MainName, (case when a.DeliveryTo_IdNo <> 0 then tDELV.Ledger_Address1+tDELV.Ledger_Address2 else  L.Ledger_Address1+L.Ledger_Address2 end) as deliveryaddress1,  (case when a.DeliveryTo_IdNo <> 0 then tDELV.Ledger_Address3+tDELV.Ledger_Address4 else  L.Ledger_Address3+L.Ledger_Address4 end) as deliveryaddress2, (case when a.DeliveryTo_IdNo <> 0 then tDELV.City_Town else  L.City_Town end) as city_town_name, (case when a.DeliveryTo_IdNo <> 0 then tDELV.Pincode else  L.Pincode end) as pincodee, TS.State_Code, (case when a.DeliveryTo_IdNo <> 0 then TDCS.State_Code else TS.State_Code end) as actual_StateCode," &
                         " 1                     , 0 , a.approx_Value     , 0 ,  0 , 0   ,   0         ,0                   , t.Ledger_GSTINNo  , t.Ledger_Name ," &
                         " ''        ,''            ,  0        ,     '1'  AS TrMode ," &
                         " a.Vehicle_No, 'R', '" & NewCode & "', tDELV.Ledger_GSTINNo as ShippedTo_GSTIN, tDELV.Ledger_MainName as ShippedTo_LedgerName from SizingSoft_Yarn_Delivery_Head a inner Join Company_Head C on a.Company_IdNo = C.Company_IdNo " &
                         " Inner Join Ledger_Head L ON a.DeliveryTo_IdNo = L.Ledger_IdNo Left Outer Join Ledger_Head tDELV on a.DeliveryTo_IdNo <> 0 and a.DeliveryTo_IdNo = tDELV.Ledger_IdNo left Outer Join Ledger_Head T on a.Transport_IdNo = T.Ledger_IdNo " &
                         " Left Outer Join State_Head FS On C.Company_State_IdNo = fs.State_IdNo left Outer Join State_Head TS on L.Ledger_State_IdNo = TS.State_IdNo  left Outer Join State_Head TDCS on tDELV.Ledger_State_IdNo = TDCS.State_IdNo " &
                          " where a.Yarn_Delivery_Code = '" & Trim(NewCode) & "'"
        CMD.ExecuteNonQuery()

        'vSgst = 

        CMD.CommandText = " Update EWB_Head Set CGST_Value = ( (Total_value * 5 / 100 ) / 2 ) , SGST_Value = ( (Total_value * 5 / 100 ) / 2 ) , TotalInvValue = ( Total_value + SGST_Value + CGST_Value )  where InvCode = '" & Trim(NewCode) & "' "
        CMD.ExecuteNonQuery()


        CMD.CommandText = " Update EWB_Head Set TotalInvValue = ( Total_value + SGST_Value + CGST_Value )  where InvCode = '" & Trim(NewCode) & "' "
        CMD.ExecuteNonQuery()


        CMD.CommandText = "Delete from EWB_Details Where InvCode = '" & NewCode & "'"
        CMD.ExecuteNonQuery()

        Dim vPARTICULARS_FIELDNAME As String = ""
        If Trim(Common_Procedures.settings.CustomerCode) = "1234" Then
            vPARTICULARS_FIELDNAME = "(c.Count_Name )"
        Else
            vPARTICULARS_FIELDNAME = "( I.Count_Name + ' - ' + IG.ItemGroup_Name )"
        End If

        Dim dt1 As New DataTable


        da = New SqlClient.SqlDataAdapter(" Select  I.Count_Name, IG.ItemGroup_Name ,IG.Item_HSN_Code,( Case When Lh.Ledger_Type ='Weaver' and Lh.Ledger_GSTINNo <> '' Then (IG.Item_GST_Percentage ) else 0 end ) , sum(a.approx_Value) As TaxableAmt,sum(a.Total_Weight) as Qty, 1 , 'WGT' AS Units " &
                                          " from SizingSoft_Yarn_Delivery_Details SD Inner Join SizingSoft_Yarn_Delivery_Head a On a.Yarn_Delivery_Code = sd.Yarn_Delivery_Code Inner Join Count_Head I On SD.Count_IdNo = I.Count_IdNo Inner Join ItemGroup_Head IG on I.ItemGroup_IdNo = IG.ItemGroup_IdNo " &
                                          " INNER Join Ledger_Head Lh ON Lh.Ledger_Idno = a.DeliveryTo_IdNo INNER JOIN Company_Head tz On tz.Company_Idno = a.Company_Idno  Where SD.Yarn_Delivery_Code = '" & Trim(NewCode) & "' Group By " &
                                          " I.Count_Name,IG.ItemGroup_Name,IG.Item_HSN_Code,IG.ItemGroup_Name ,IG.Item_HSN_Code,IG.Item_GST_Percentage,Lh.Ledger_Type, Lh.Ledger_GSTINNo", con)
        dt1 = New DataTable
        da.Fill(dt1)

        'da = New SqlClient.SqlDataAdapter(" Select  I.Count_Name, ( I.Count_Name + ' - ' + replace(IG.ItemGroup_Name,IG.Item_HSN_Code,'') ) ,IG.Item_HSN_Code,( Case When Lh.Ledger_Type ='Weaver' and Lh.Ledger_GSTINNo <> '' Then (IG.Item_GST_Percentage ) else 0 end ) , sum(a.Amount) As TaxableAmt,sum(a.Total_Weight) as Qty, 1 , 'WGT' AS Units " &
        '                                  " from Weaver_Yarn_Delivery_Details SD Inner Join Weaver_Yarn_Delivery_Head a On a.Weaver_Yarn_Delivery_Code = sd.Weaver_Yarn_Delivery_Code Inner Join Count_Head I On SD.Count_IdNo = I.Count_IdNo Inner Join ItemGroup_Head IG on I.ItemGroup_IdNo = IG.ItemGroup_IdNo " &
        '                                  " INNER Join Ledger_Head Lh ON Lh.Ledger_Idno = a.DeliveryTo_IdNo INNER JOIN Company_Head tz On tz.Company_Idno = a.Company_Idno  Where SD.Weaver_Yarn_Delivery_Code = '" & Trim(NewCode) & "' Group By " &
        '                                  " I.Count_Name,IG.ItemGroup_Name,IG.Item_HSN_Code,IG.ItemGroup_Name ,IG.Item_HSN_Code,IG.Item_GST_Percentage,Lh.Ledger_Type, Lh.Ledger_GSTINNo", con)
        'dt1 = New DataTable
        'da.Fill(dt1)


        For I = 0 To dt1.Rows.Count - 1

            CMD.CommandText = "Insert into EWB_Details ([SlNo]                               , [Product_Name]           ,	[Product_Description]     ,	[HSNCode]                 ,	[Quantity]                                ,[QuantityUnit] ,  Tax_Perc                         ,	[CessRate]         ,	[CessNonAdvol]  ,	[TaxableAmount]               ,InvCode) " &
                              " values                 (" & dt1.Rows(I).Item(6).ToString & ",'" & dt1.Rows(I).Item(0) & "', '" & dt1.Rows(I).Item(1) & "', '" & dt1.Rows(I).Item(2) & "', " & dt1.Rows(I).Item(5).ToString & ",'MTR'          ," & dt1.Rows(I).Item(3).ToString & ", 0                  , 0                   ," & dt1.Rows(I).Item(4) & ",'" & NewCode & "')"

            CMD.ExecuteNonQuery()

        Next

        btn_GENERATEEWB.Enabled = False

        Dim ewb As New EWB(Val(lbl_Company.Tag))
        EWB.GenerateEWB(NewCode, con, rtbEWBResponse, txt_EWBNo, "SizingSoft_Yarn_Delivery_Head", "EWave_Bill_No", "Yarn_Delivery_Code", Pk_Condition)

    End Sub

    Private Sub btn_PRINTEWB_Click(sender As Object, e As EventArgs) Handles btn_PRINTEWB.Click
        Dim ewb As New EWB(Val(lbl_Company.Tag))
        EWB.PrintEWB(txt_ElectronicRefNo.Text, rtbEWBResponse, 0)
    End Sub

    Private Sub btn_Detail_PRINTEWB_Click(sender As Object, e As EventArgs) Handles btn_Detail_PRINTEWB.Click
        Dim ewb As New EWB(Val(lbl_Company.Tag))
        EWB.PrintEWB(txt_ElectronicRefNo.Text, rtbEWBResponse, 1)
    End Sub

    Private Sub btn_CancelEWB_1_Click(sender As Object, e As EventArgs) Handles btn_CancelEWB_1.Click

        Dim NewCode As String = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        Dim c As Integer = MsgBox("Are You Sure To Cancel This EWB ? ", vbYesNo)

        If c = vbNo Then Exit Sub

        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()

        Dim ewb As New EWB(Val(lbl_Company.Tag))

        EWB.CancelEWB(txt_ElectronicRefNo.Text, NewCode, con, rtbEWBResponse, txt_EWBNo, "SizingSoft_Yarn_Delivery_Head", "EWave_Bill_No", "Yarn_Delivery_Code")

    End Sub

    Private Sub txt_EWBNo_TextChanged(sender As Object, e As EventArgs) Handles txt_EWBNo.TextChanged
        txt_ElectronicRefNo.Text = txt_EWBNo.Text
    End Sub

    Private Sub txt_BookNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_BookNo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            'If Trim(Common_Procedures.settings.CustomerCode) = "1112" Then '----VENUS SIZING
            '    If txt_TexDcNo.Enabled And txt_TexDcNo.Visible Then txt_TexDcNo.Focus()
            'Else
            '    If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
            'End If
            If txt_TexDcNo.Enabled And txt_TexDcNo.Visible Then txt_TexDcNo.Focus()
        End If
    End Sub



    Private Sub cbo_Det_Location_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Det_Location.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'GODOWN')", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_Det_Location_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Det_Location.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Common_Procedures.UR.Ledr_Wea_Siz_Rw_Trans_JbWrk_Creation = Common_Procedures.UR.Ledger_Creation
            Common_Procedures.MDI_LedType = ""
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Det_Location.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_Det_Location_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Det_Location.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Det_Location, txt_DetLotNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'GODOWN')", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub btn_PDF_Click(sender As Object, e As EventArgs) Handles btn_PDF.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        Print_PDF_Status = True
        EMAIL_Status = False
        WHATSAPP_Status = False
        print_record()
        'Print_PDF_Status = False
    End Sub

    Private Sub cbo_Det_Location_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Det_Location.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Det_Location, txt_DetLotNo, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'GODOWN')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub txt_DetLotNo_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_DetLotNo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            btn_Add_Click(sender, e)
        End If
    End Sub


    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        save_record()
        If Trim(UCase(LastNo)) = Trim(UCase(lbl_DcNo.Text)) Then
            Timer1.Enabled = False
            SaveAll_STS = False
            MessageBox.Show("All entries saved sucessfully", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Else
            movenext_record()

        End If
    End Sub

    Private Sub txt_kg_Rate_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_kg_Rate.KeyDown
        If e.KeyCode = 38 Then
            txt_Remarks.Focus()
        End If

        If e.KeyValue = 40 Then
            txt_Approx_Value.Focus()
        End If

    End Sub

    Private Sub txt_kg_Rate_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_kg_Rate.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_Approx_Value.Focus()
        End If
    End Sub


    Private Sub Amount_Calultation()

        Dim nTotWgt As String

        nTotWgt = 0
        If dgv_Details_Total.RowCount > 0 Then
            nTotWgt = Val(dgv_Details_Total.Rows(0).Cells(7).Value())
        End If

        If Val(txt_kg_Rate.Text) <> 0 Then
            txt_Approx_Value.Text = Format(Val(nTotWgt) * Val(txt_kg_Rate.Text), "########0.00")
        End If


    End Sub

    Private Sub txt_kg_Rate_TextChanged(sender As Object, e As EventArgs) Handles txt_kg_Rate.TextChanged
        Amount_Calultation()
    End Sub

    Private Sub Printing_Format1087(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim EntryCode As String
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ItmNm1 As String, ItmNm2 As String
        'Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim PS As Printing.PaperSize
        Dim PCnt As Integer = 0, PrntCnt As Integer = 0

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1037" Or Common_Procedures.settings.CustomerCode = "1038" Then
            PrntCnt = 1
            If Val(vPrnt_2Copy_In_SinglePage) = 1 Then
                If PrntCnt2ndPageSTS = False Then
                    PrntCnt = 2
                End If
            End If

            set_PaperSize_For_PrintDocument1()
        Else
            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                    PS = PrintDocument1.PrinterSettings.PaperSizes(I)
                    PrintDocument1.DefaultPageSettings.PaperSize = PS
                    e.PageSettings.PaperSize = PS
                    Exit For
                End If
            Next
        End If



        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 20  ' 50 
            .Right = 55
            .Top = 20   '30
            .Bottom = 35 ' 30
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pFont = New Font("Arial", 8, FontStyle.Regular)

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

        If Common_Procedures.settings.CustomerCode = "1155" Then
            NoofItems_PerPage = 3 ' 6 ' 5
        Else
            NoofItems_PerPage = 5 ' 6 ' 5
        End If

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = 40
        ClArr(2) = 60 : ClArr(3) = 80 : ClArr(4) = 210 : ClArr(5) = 100 : ClArr(6) = 70 : ClArr(7) = 70
        ClArr(8) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7))

        If Common_Procedures.settings.CustomerCode = "1155" Then
            TxtHgt = 17.5
        Else
            TxtHgt = 18.5
        End If

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format1087_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                Try

                    NoofDets = 0

                    CurY = CurY - 10

                    If prn_DetDt.Rows.Count > 0 Then

                        Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                            If NoofDets >= NoofItems_PerPage Then
                                CurY = CurY + TxtHgt

                                Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                                NoofDets = NoofDets + 1

                                Printing_Format1087_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

                                e.HasMorePages = True
                                Return

                            End If

                            prn_DetSNo = prn_DetSNo + 1

                            ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Mill_Name").ToString)
                            ItmNm2 = ""
                            If Len(ItmNm1) > 18 Then
                                For I = 18 To 1 Step -1
                                    If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                                Next I
                                If I = 0 Then I = 18
                                ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                                ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                            End If


                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Sl_No").ToString), LMargin + 25, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Yarn_Type").ToString, LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                            If IsDBNull(prn_DetDt.Rows(prn_DetIndx).Item("Set_no").ToString) = False Then
                                If Trim(prn_DetDt.Rows(prn_DetIndx).Item("Set_no").ToString) <> "" Then
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Set_no").ToString), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                                End If
                            End If
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Count_Name").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 10, CurY, 0, 0, pFont)
                            If Val(prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString) <> 0 Then
                                Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)
                            End If
                            If Val(prn_DetDt.Rows(prn_DetIndx).Item("Cones").ToString) <> 0 Then
                                Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Cones").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
                            End If
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Weight").ToString), "########0.000"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            If Trim(ItmNm2) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If

                            prn_DetIndx = prn_DetIndx + 1

                        Loop

                    End If

                    Printing_Format1087_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)

                Catch ex As Exception

                    MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                End Try

            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        prn_Count = prn_Count + 1

        e.HasMorePages = False

        If Val(prn_TotCopies) > 1 Then
            If prn_Count < Val(prn_TotCopies) Then

                prn_DetIndx = 0
                prn_PageNo = 0
                prn_DetIndx = 0
                prn_PageNo = 0
                'prn_NoofBmDets = 0


                e.HasMorePages = True
                Return

            End If

        End If


    End Sub

    Private Sub Printing_Format1087_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_Email As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim p1Font As Font
        Dim strHeight As Single = 0, strWidth As Single = 0
        Dim W1 As Single, C1 As Single, S1 As Single, K1 As Single
        Dim CurX As Single = 0
        Dim Gst_dt As Date
        Dim Entry_dt As Date
        Dim Led_Name As String, Led_Add1 As String, Led_Add2 As String, Led_Add3 As String, Led_Add4 As String
        Dim Led_GstNo As String, Led_TinNo As String
        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Mill_Name, c.Count_Name  from SizingSoft_Yarn_Delivery_Details a INNER JOIN Mill_Head b on a.Mill_IdNo = b.Mill_IdNo LEFT OUTER JOIN Count_Head c on a.Count_IdNo = c.Count_IdNo where a.Yarn_Delivery_Code = '" & Trim(EntryCode) & "' Order by a.sl_no", con)
        da2.Fill(dt2)
        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        dt2.Clear()


        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_Email = ""
        Cmp_StateCap = "" : Cmp_StateNm = "" : Cmp_StateCode = "" : Cmp_GSTIN_Cap = "" : Cmp_GSTIN_No = ""

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1015" Then
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString
            Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address2").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString

        Else

            If Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString) <> "" Then
                Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
                Cmp_Name = Replace(Cmp_Name, Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString), "")
                Cmp_Add1 = Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString)
                Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString

            Else
                Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
                Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
                Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString

            End If

        End If
        'Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        'Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
        'Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString
        If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "PHONE NO.:" & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
            Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_panNo").ToString) <> "" Then
            Cmp_CstNo = "PAN NO.: " & prn_HdDt.Rows(0).Item("Company_panNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
            Cmp_Email = "E-mail : " & Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString)
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




        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Americana Std", 20, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, StrConv(Cmp_Name, VbStrConv.ProperCase), LMargin, CurY, 2, PrintWidth, p1Font)

        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        If Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString) <> "" Then
            p1Font = New Font("Arial", 8, FontStyle.Bold)
        Else
            p1Font = New Font("Arial", 8, FontStyle.Regular)
        End If
        CurY = CurY + strHeight
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, p1Font)

        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height
        CurY = CurY + strHeight
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)

        'CurY = CurY + strHeight
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1 & " " & Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)

        Gst_dt = #7/1/2017#
        Entry_dt = dtp_Date.Value

        If DateDiff("d", Gst_dt.ToShortDateString, Entry_dt.ToShortDateString) < 0 Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt - 1
            Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt - 13  ' 10

        Else

            CurY = CurY + TxtHgt

            p1Font = New Font("Arial", 8, FontStyle.Bold)
            strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_GSTIN_Cap), p1Font).Width
            strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_StateNm & "     " & Cmp_GSTIN_Cap & Cmp_GSTIN_No & Cmp_CstNo), pFont).Width
            If PrintWidth > strWidth Then
                CurX = LMargin + (PrintWidth - strWidth) / 2
            Else
                CurX = LMargin
            End If

            p1Font = New Font("Arial", 8, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_StateCap, CurX, CurY, 0, 0, p1Font)
            strWidth = e.Graphics.MeasureString(Cmp_StateCap, p1Font).Width
            CurX = CurX + strWidth
            Common_Procedures.Print_To_PrintDocument(e, Cmp_StateNm, CurX, CurY, 0, 0, pFont)

            strWidth = e.Graphics.MeasureString(Cmp_StateNm, pFont).Width
            p1Font = New Font("Arial", 8, FontStyle.Bold)
            CurX = CurX + strWidth
            Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_GSTIN_Cap, CurX, CurY, 0, PrintWidth, p1Font)
            strWidth = e.Graphics.MeasureString("     " & Cmp_GSTIN_Cap, p1Font).Width
            CurX = CurX + strWidth
            Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No & "     " & Cmp_CstNo, CurX, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PhNo & "   " & Cmp_Email), LMargin, CurY, 2, PrintWidth, pFont)
            'CurY = CurY + TxtHgt - 1
            'Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, Cmp_Email, PageWidth - 10, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt - 1

        End If
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)
        Led_Name = "" : Led_Add1 = "" : Led_Add2 = "" : Led_Add3 = "" : Led_Add4 = "" : Led_GstNo = "" : Led_TinNo = ""

        If Trim(UCase(Common_Procedures.User.Type)) = "UNACCOUNT" And (Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1263") Then
            Led_Name = prn_HdDt.Rows(0).Item("Ledger_MainName").ToString : Led_Add1 = "" : Led_Add2 = "" : Led_Add3 = "" : Led_Add4 = "" : Led_GstNo = "" : Led_TinNo = ""
        Else
            Led_Name = prn_HdDt.Rows(0).Item("Ledger_MainName").ToString
            Led_Add1 = prn_HdDt.Rows(0).Item("Ledger_Address1").ToString
            Led_Add2 = prn_HdDt.Rows(0).Item("Ledger_Address2").ToString
            Led_Add3 = prn_HdDt.Rows(0).Item("Ledger_Address3").ToString '& "," & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString
            Led_Add4 = prn_HdDt.Rows(0).Item("Ledger_Address4").ToString

            If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
                Led_GstNo = "GSTIN : " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString
            End If
            If Trim(prn_HdDt.Rows(0).Item("pan_no").ToString) <> "" Then
                Led_TinNo = " PAN NO  " & Trim(prn_HdDt.Rows(0).Item("pan_no").ToString)
            End If
        End If
        p1Font = New Font("Arial", 9, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "YARN DELIVERY NOTE", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        'CurY = CurY + TxtHgt

        CurY = CurY + strHeight + 5 ' + 150
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try

            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4)
            W1 = e.Graphics.MeasureString("BOOK NO  : ", pFont).Width
            K1 = e.Graphics.MeasureString("SIZING BOOK NO : ", pFont).Width
            S1 = e.Graphics.MeasureString("TO    :", pFont).Width

            CurY = CurY + TxtHgt - 5
            p1Font = New Font("Arial", 9, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "TO  :  " & "M/s. " & Led_Name, LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "DC.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + K1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Yarn_Delivery_No").ToString, LMargin + C1 + K1 + 25, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            p1Font = New Font("Arial", 7, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, Led_Add1, LMargin + S1 + 10, CurY, 0, 0, p1Font)
            p1Font = New Font("Arial", 8, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + K1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Yarn_Delivery_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + K1 + 25, CurY, 0, 0, pFont)
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1078" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1087" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1087" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1112" Then '---- Kalaimagal Sizing (Palladam)
                If Trim(prn_HdDt.Rows(0).Item("Entry_Time_Text").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, "TIME : " & Trim(prn_HdDt.Rows(0).Item("Entry_Time_Text").ToString), PageWidth - 10, CurY, 1, 0, pFont)
                End If
            End If

            'CurY = CurY + TxtHgt
            ' e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY + 30, PageWidth + 20, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY + 20, PageWidth, CurY + 20)
            CurY = CurY + TxtHgt
            p1Font = New Font("Arial", 8, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, Led_Add2, LMargin + S1 + 10, CurY, 0, 0, p1Font)
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1155" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1263" Then
                If Trim(prn_HdDt.Rows(0).Item("Vendor_Name").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, "DELIVERY TO ", LMargin + C1 + 10, CurY + 5, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + K1 + 10, CurY + 5, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vendor_Name").ToString, LMargin + C1 + K1 + 30, CurY + 5, 0, 0, pFont)
                End If

            Else
                If Trim(prn_HdDt.Rows(0).Item("DelName").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, "DELIVERY TO", LMargin + C1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + K1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DelName").ToString, LMargin + C1 + K1 + 30, CurY, 0, 0, pFont)
                End If
            End If
            'If Trim(prn_HdDt.Rows(0).Item("Book_No").ToString) <> "" Then
            '    Common_Procedures.Print_To_PrintDocument(e, "SIZING BOOK NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + K1 + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Book_No").ToString, LMargin + C1 + K1 + 30, CurY, 0, 0, pFont)
            'Else
            '    If Trim(prn_HdDt.Rows(0).Item("Delivery_At").ToString) <> "" Then
            '        Common_Procedures.Print_To_PrintDocument(e, "DELIVERY AT", LMargin + C1 + 10, CurY, 0, 0, pFont)
            '        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + K1 + 10, CurY, 0, 0, pFont)
            '        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Delivery_At").ToString, LMargin + C1 + K1 + 30, CurY, 0, 0, pFont)
            '    End If
            'End If

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Led_Add3, LMargin + S1 + 10, CurY, 0, 0, p1Font)

            If Trim(prn_HdDt.Rows(0).Item("Vendor_Address1").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vendor_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Vendor_Address2").ToString, LMargin + C1 + 10, CurY + 5, 0, 0, pFont)
            End If
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Led_Add4, LMargin + S1 + 10, CurY, 0, 0, p1Font)
            If Trim(prn_HdDt.Rows(0).Item("Vendor_Address3").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vendor_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Vendor_Address4").ToString, LMargin + C1 + 10, CurY + 5, 0, 0, pFont)
            End If

            'If Trim(prn_HdDt.Rows(0).Item("Textile_Dc_No").ToString) <> "" Then
            '    Common_Procedures.Print_To_PrintDocument(e, "TEXTILE DC NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + K1 + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Textile_Dc_No").ToString, LMargin + C1 + K1 + 30, CurY, 0, 0, pFont)
            'End If
            'If Common_Procedures.settings.CustomerCode = "1112" Then
            '    CurY = CurY + TxtHgt
            '    p1Font = New Font("Calibri", 11, FontStyle.Bold)
            '    Common_Procedures.Print_To_PrintDocument(e, "(For Jobwork Only, Not For Sale)", LMargin + C1 + 10, CurY, 0, 0, p1Font)
            'End If
            CurY = CurY + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("ledger_GSTinNo").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, Led_GstNo & Led_TinNo, LMargin + S1 + 10, CurY, 0, 0, pFont)
            End If
            If Trim(prn_HdDt.Rows(0).Item("GST_No").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "GSTIN : " & prn_HdDt.Rows(0).Item("GST_No").ToString & Led_TinNo, LMargin + C1 + 10, CurY + 5, 0, 0, pFont)
            End If


            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + C1, LnAr(3), LMargin + C1, LnAr(2))

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "TYPE", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "SET NO", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "MILL NAME", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "COUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "BAGS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "CONES", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format1087_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim I As Integer
        Dim Cmp_Name As String
        Dim W1 As Single, C1 As Single, S1 As Single, K1 As Single
        Dim Del_Add1 As String = "", Del_Add2 As String = ""
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        Dim vTxamt As String = 0
        Dim vNtAMt As String = 0
        Dim vSgst_amt As String = 0
        Dim vCgst_amt As String = 0

        Try
            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4)
            W1 = e.Graphics.MeasureString("BOOK NO  : ", pFont).Width
            K1 = e.Graphics.MeasureString("SIZING BOOK NO : ", pFont).Width
            S1 = e.Graphics.MeasureString("TO    :", pFont).Width

            For I = NoofDets + 1 To NoofItems_PerPage
                CurY = CurY + TxtHgt
            Next

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

            CurY = CurY + TxtHgt - 10
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, " TOTAL", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)

                If Val(prn_HdDt.Rows(0).Item("Total_Bags").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Bags").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                End If
                If Val(prn_HdDt.Rows(0).Item("Total_Cones").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Cones").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                End If
                If Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString), "#########0.000"), PageWidth - 10, CurY, 1, 0, pFont)
                End If
            End If

            CurY = CurY + TxtHgt - 15

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))


            CurY = CurY + TxtHgt - 10

            Common_Procedures.Print_To_PrintDocument(e, "Through : " & Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString), LMargin + 10, CurY, 0, 0, pFont)

            'Common_Procedures.Print_To_PrintDocument(e, "VENDOR NAME", LMargin + C1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + K1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vendor_Name").ToString, LMargin + C1 + K1 + 25, CurY, 0, 0, pFont)
            ' CurY = CurY + TxtHgt
            'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1155" Then
            '    p1Font = New Font("Calibri", 9, FontStyle.Bold)
            '    If Common_Procedures.Vendor_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("Vendor_IdNo").ToString)) <> "" Then

            '        da1 = New SqlClient.SqlDataAdapter("select a.* from Vendor_head a where  a.Vendor_IdNO = " & Val(prn_HdDt.Rows(0).Item("Vendor_IdNo").ToString) & " ", con)
            '        dt = New DataTable
            '        da1.Fill(dt)

            '        If dt.Rows.Count > 0 Then

            '            Del_Add1 = dt.Rows(0).Item("Vendor_Address1").ToString & "," & dt.Rows(0).Item("Vendor_Address2").ToString
            '            Del_Add2 = dt.Rows(0).Item("Vendor_Address3").ToString & "," & dt.Rows(0).Item("Vendor_Address4").ToString

            '        End If
            '        Common_Procedures.Print_To_PrintDocument(e, "DELIVERY TO   : " & Common_Procedures.Vendor_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("Vendor_IdNo").ToString)), LMargin + 10, CurY, 0, 0, p1Font)
            '        CurY = CurY + TxtHgt
            '        Common_Procedures.Print_To_PrintDocument(e, " " & Trim(Del_Add1), LMargin + 30, CurY, 0, 0, pFont)
            '        CurY = CurY + TxtHgt
            '        Common_Procedures.Print_To_PrintDocument(e, " " & Trim(Del_Add2), LMargin + 30, CurY, 0, 0, pFont)
            '    End If
            'End If

            If Val(prn_HdDt.Rows(0).Item("Kg_Rate").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, " Rate/Kg : " & Trim(prn_HdDt.Rows(0).Item("Kg_Rate").ToString), PageWidth - 510, CurY, 0, 0, pFont)
            End If

            If Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, " Empty Beam : " & Trim(prn_HdDt.Rows(0).Item("Empty_Beam").ToString), PageWidth - 400, CurY, 0, 0, pFont)
            End If

            If Val(prn_HdDt.Rows(0).Item("Empty_Bags").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, " Empty Bags : " & Trim(prn_HdDt.Rows(0).Item("Empty_Bags").ToString), PageWidth - 280, CurY, 0, 0, pFont)
            End If

            If Val(prn_HdDt.Rows(0).Item("Empty_Cones").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, " Empty Cones : " & Trim(prn_HdDt.Rows(0).Item("Empty_Cones").ToString), PageWidth - 150, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt

            If Val(prn_HdDt.Rows(0).Item("approx_Value").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, " Taxable Amount : " & Trim(prn_HdDt.Rows(0).Item("approx_Value").ToString), LMargin + 10, CurY, 0, 0, pFont)
            End If

            If Val(prn_HdDt.Rows(0).Item("approx_Value").ToString) <> 0 Then

                vCgst_amt = Format((Val(prn_HdDt.Rows(0).Item("approx_Value").ToString) * 2.5 / 100), "############0")
                vSgst_amt = Format((Val(prn_HdDt.Rows(0).Item("approx_Value").ToString) * 2.5 / 100), "############0")


                Common_Procedures.Print_To_PrintDocument(e, " CGST 2.5 % : " & vCgst_amt, PageWidth - 560, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " SGST 2.5 % : " & vSgst_amt, PageWidth - 420, CurY, 0, 0, pFont)
            End If

            If Val(prn_HdDt.Rows(0).Item("approx_Value").ToString) <> 0 Then
                vTxamt = Val(vCgst_amt) + Val(vSgst_amt) 'Format((Val(prn_HdDt.Rows(0).Item("approx_Value").ToString) * 5 / 100), "############0.00")
                Common_Procedures.Print_To_PrintDocument(e, " Tax Amount : " & vTxamt, PageWidth - 280, CurY, 0, 0, pFont)
            End If

            If Val(vTxamt) <> 0 Then
                vNtAMt = Format(Val(prn_HdDt.Rows(0).Item("approx_Value").ToString) + vTxamt, "###########0.00")
                Common_Procedures.Print_To_PrintDocument(e, " Net Amount : " & vNtAMt, PageWidth - 150, CurY, 0, 0, pFont)
            End If



            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(7) = CurY

            CurY = CurY + TxtHgt
            If Val(Common_Procedures.User.IdNo) <> 1 Then
                Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
            End If
            CurY = CurY + TxtHgt

            If Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString) <> "" Then
                Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
                Cmp_Name = Replace(Cmp_Name, Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString), "")

            Else
                Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString

            End If

            Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 20, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 250, CurY, 0, 0, pFont)
            p1Font = New Font("Arial", 8, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, LMargin + PageWidth - 75, CurY, 1, 0, p1Font)

            'e.Graphics.DrawLine(Pens.Black, PageWidth, CurY, PageWidth, LnAr(1))

            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub


End Class