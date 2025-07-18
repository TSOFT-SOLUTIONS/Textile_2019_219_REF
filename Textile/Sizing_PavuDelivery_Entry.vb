Imports System.IO
Public Class Sizing_PavuDelivery_Entry
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "PVDLV-"
    Private Pk_Condition_Tex As String = "SSPDC-"
    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl
    Private Prec_ActCtrl As New Control
    Private vCbo_ItmNm As String
    Private vcbo_KeyDwnVal As Double
    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private cnt As Integer = 0
    Private prn_DetIndx As Integer
    Private prn_DetDt1 As New DataTable
    Private prn_PageNo1 As Integer
    Private prn_DetIndx1 As Integer
    Private prn_DetAr(50, 10) As String
    Private prn_DetMxIndx As Integer
    Private prn_NoofBmDets As Integer
    Private prn_NoofBmDets1 As Integer
    Private prn_Status As Integer
    Private Prev_kyData As Keys
    Private prn_TotCopies As Integer = 0
    Private prn_Count As Integer = 0
    Private PrntCnt2ndPageSTS As Boolean = False
    Private TrnTo_DbName As String = ""

    Private pth As String
    Private pth2 As String
    Private PrnTxt As String = ""
    Private a() As String
    Private fs As FileStream
    Private r As StreamReader
    Private w As StreamWriter
    Private prn_DetSNo As Integer
    Private prn_DetSNo1 As Integer
    Private Hz1 As Integer, Hz2 As Integer, Vz1 As Integer, Vz2 As Integer
    Private Corn1 As Integer, Corn2 As Integer, Corn3 As Integer, Corn4 As Integer
    Private LfCon As Integer, RgtCon As Integer
    Private LnCnt As Integer = 0, CenCon As Integer
    Private CenDwn As Integer, CenUp As Integer

    Private SaveAll_STS As Boolean = False
    Private LastNo As String = ""

    Private Print_PDF_Status As Boolean = False
    Private EMAIL_Status As Boolean = False
    Private WHATSAPP_Status As Boolean = False
    Private Sub clear()
        New_Entry = False
        Insert_Entry = False

        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        pnl_Selection.Visible = False

        lbl_DcNo.Text = ""
        lbl_DcNo.ForeColor = Color.Black

        dtp_Date.Text = ""
        txt_BookNo.Text = ""
        txt_SetNo.Text = ""
        cbo_Ledger.Text = ""
        lbl_CountName.Text = ""
        cbo_DeliveryTo.Text = ""
        cbo_Grid_Vendor.Text = ""
        cbo_VendorName.Text = ""
        txt_Rate.Text = ""
        lbl_value.Text = ""
        lbl_Ends.Text = ""
        cbo_Transport.Text = ""
        cbo_MillName.Text = ""

        lbl_UserName_CreatedBy.Text = ""
        lbl_UserName_ModifiedBy.Text = ""

        Print_PDF_Status = False

        cbo_Type.Text = "SPEC"
        If (Common_Procedures.settings.CustomerCode) = "1282" Then
            cbo_Type.Text = "DIRECT"
        End If

        cbo_VehicleNo.Text = ""
        cbo_Delivered.Text = ""
        txt_Remarks.Text = ""
        txt_ElectronicRefNo.Text = ""
        txt_DateAndTimeOFSupply.Text = ""
        dtp_Time.Text = ""
        cbo_Grid_Count.Text = ""

        chk_Printed.Checked = False
        chk_Printed.Enabled = False
        chk_Printed.Visible = False
        chk_Loaded.Checked = False

        If Filter_Status = False Then
            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate   '.Date.ToShortDateString
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate   '.Date.ToShortDateString
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_CountName.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_CountName.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()
        End If

        cbo_Ledger.Tag = ""

        dgv_Details.Rows.Clear()
        dgv_Details.Rows.Add()
        dgv_Details_Total.Rows.Clear()
        dgv_Details_Total.Rows.Add()
        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))

        dtp_Date.Enabled = True
        dtp_Date.BackColor = Color.White

        cbo_Transport.Enabled = True
        cbo_Transport.BackColor = Color.White

        cbo_Ledger.Enabled = True
        cbo_Ledger.BackColor = Color.White

        cbo_VehicleNo.Enabled = True
        cbo_VehicleNo.BackColor = Color.White
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

        If Me.ActiveControl.Name <> cbo_Grid_Count.Name Then
            cbo_Grid_Count.Visible = False
        End If

        Grid_Cell_DeSelect()

        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        On Error Resume Next
        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is Button Then
                If Prec_ActCtrl.Name = btn_SMS.Name Then
                    Prec_ActCtrl.BackColor = Color.DimGray
                Else
                    Prec_ActCtrl.BackColor = Color.FromArgb(41, 57, 85)
                End If
                Prec_ActCtrl.ForeColor = Color.White

            Else
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

    Private Sub Grid_Cell_DeSelect()

        On Error Resume Next

        If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
        If IsNothing(dgv_Details_Total.CurrentCell) Then Exit Sub
        If IsNothing(dgv_Filter_Details.CurrentCell) Then Exit Sub

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
        Dim LockSTS As Boolean = False
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        If Val(no) = 0 Then Exit Sub

        clear()

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name as PartyName, c.Transport_Name, mh.Mill_Name from Sizing_Pavu_Delivery_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Transport_Head c ON a.Transport_IdNo = c.Transport_IdNo LEFT OUTER JOIN Mill_Head mh ON a.Mill_IdNo = mh.Mill_IdNo Where a.Pavu_Delivery_Code = '" & Trim(NewCode) & "'", con)
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then
                lbl_DcNo.Text = dt1.Rows(0).Item("Pavu_Delivery_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Pavu_Delivery_Date").ToString

                cbo_Ledger.Text = dt1.Rows(0).Item("PartyName").ToString
                txt_BookNo.Text = dt1.Rows(0).Item("Book_No").ToString

                lbl_Ends.Text = dt1.Rows(0).Item("Ends_Name").ToString

                lbl_CountName.Text = dt1.Rows(0).Item("Count_Name").ToString


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

                cbo_Transport.Text = dt1.Rows(0).Item("Transport_Name").ToString
                cbo_DeliveryTo.Text = Common_Procedures.Despatch_IdNoToName(con, Val(dt1.Rows(0).Item("DeliveryTo_IdNo").ToString))
                cbo_VendorName.Text = Common_Procedures.Vendor_IdNoToName(con, Val(dt1.Rows(0).Item("Vendor_IdNo").ToString))
                cbo_VehicleNo.Text = dt1.Rows(0).Item("Vehicle_No").ToString
                cbo_Delivered.Text = dt1.Rows(0).Item("Delivered_By").ToString
                cbo_MillName.Text = dt1.Rows(0).Item("Mill_Name").ToString

                txt_Remarks.Text = dt1.Rows(0).Item("Remarks").ToString
                Cbo_RateFor.Text = dt1.Rows(0).Item("Rate_For").ToString
                txt_ElectronicRefNo.Text = dt1.Rows(0).Item("Electronic_Reference_No").ToString
                txt_DateAndTimeOFSupply.Text = dt1.Rows(0).Item("Date_And_Time_Of_Supply").ToString
                txt_Rate.Text = dt1.Rows(0).Item("Rate").ToString
                lbl_value.Text = dt1.Rows(0).Item("Approx_Value").ToString
                If IsDBNull(dt1.Rows(0).Item("Invoice_Selection_Type").ToString) = False Then
                    If Trim(dt1.Rows(0).Item("Invoice_Selection_Type").ToString) <> "" Then
                        cbo_Type.Text = dt1.Rows(0).Item("Invoice_Selection_Type").ToString
                    End If
                End If

                dtp_Time.Text = (dt1.Rows(0).Item("Entry_Time_Text").ToString)
                lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))

                If IsDBNull(dt1.Rows(0).Item("Gate_Pass_Code").ToString) = False Then
                    If Trim(dt1.Rows(0).Item("Gate_Pass_Code").ToString) <> "" Then
                        LockSTS = True
                    End If
                End If
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
                If Val(dt1.Rows(0).Item("Loaded_By_Our_Employee").ToString) = 1 Then chk_Loaded.Checked = True


                If Trim(cbo_Type.Text) = "DIRECT" Then
                    da2 = New SqlClient.SqlDataAdapter("select a.* , b.Count_Name, 0 as Tex_Pavu_Delivery_Increment,  '' as Tex_Beam_Knotting_Code , 0 as Tex_Production_Meters, 0 as Tex_Close_Status from Sizing_Pavu_Delivery_Details a LEFT OUTER JOIN Count_Head b ON a.Count_IdNo = b.Count_IdNo where a.Pavu_Delivery_Code = '" & Trim(NewCode) & "' Order by a.For_OrderBy, a.Set_No, a.ForOrderBy_BeamNo, a.Beam_No", con)
                Else
                    da2 = New SqlClient.SqlDataAdapter("select a.* , b.Count_Name, stt.sort_no, tTEXSPP.Pavu_Delivery_Increment as Tex_Pavu_Delivery_Increment,  tTEXSPP.Beam_Knotting_Code as Tex_Beam_Knotting_Code , tTEXSPP.Production_Meters as Tex_Production_Meters, tTEXSPP.Close_Status as Tex_Close_Status  from Stock_SizedPavu_Processing_Details a LEFT OUTER JOIN Specification_Head stt ON a.Set_Code = stt.set_code  LEFT OUTER JOIN Count_Head b ON a.Count_IdNo = b.Count_IdNo LEFT OUTER JOIN Stock_SizedPavu_Processing_Details tTEXSPP ON tTEXSPP.Reference_Code LIKE '" & Trim(Pk_Condition_Tex) & "%' and tTEXSPP.Selection_From_ReferenceCode = '" & Trim(Pk_Condition) & "'  + a.Pavu_Delivery_Code and tTEXSPP.beam_no = a.beam_no Where a.Pavu_Delivery_Code = '" & Trim(NewCode) & "' Order by a.For_OrderBy, a.Set_No, a.ForOrderBy_BeamNo, a.Beam_No", con)

                End If
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_Details.Rows.Clear()
                SNo = 0

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_Details.Rows.Add()

                        SNo = SNo + 1
                        dgv_Details.Rows(n).Cells(0).Value = Val(SNo)
                        dgv_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Set_No").ToString
                        dgv_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Beam_No").ToString
                        dgv_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Noof_Pcs").ToString
                        dgv_Details.Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("Meters").ToString), "########0.00")

                        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1263" Then '---- Sri Meenakshi Sizing (Somanur)
                            dgv_Details.Rows(n).Cells(5).Value = Common_Procedures.Vendor_IdNoToName(con, dt2.Rows(i).Item("Vendor_Idno").ToString)
                        Else
                            dgv_Details.Rows(n).Cells(5).Value = dt2.Rows(i).Item("DeliveryTo_Name").ToString
                        End If

                        dgv_Details.Rows(n).Cells(6).Value = dt2.Rows(i).Item("set_code").ToString
                        dgv_Details.Rows(n).Cells(7).Value = dt2.Rows(i).Item("Ends_Name").ToString
                        dgv_Details.Rows(n).Cells(8).Value = dt2.Rows(i).Item("Count_Name").ToString
                        dgv_Details.Rows(n).Cells(9).Value = dt2.Rows(i).Item("Meters_pc").ToString
                        dgv_Details.Rows(n).Cells(10).Value = dt2.Rows(i).Item("Net_Weight").ToString

                        dgv_Details.Rows(n).Cells(11).Value = Common_Procedures.Mill_IdNoToName(con, Val(dt2.Rows(i).Item("Mill_IdNo").ToString))
                        dgv_Details.Rows(n).Cells(12).Value = dt2.Rows(i).Item("Sort_No").ToString

                        dgv_Details.Rows(n).Cells(13).Value = ""
                        If Val(dt2.Rows(i).Item("Tex_Pavu_Delivery_Increment").ToString) <> 0 Or Trim(dt2.Rows(i).Item("Tex_Beam_Knotting_Code").ToString) <> "" Or Val(dt2.Rows(i).Item("Tex_Production_Meters").ToString) <> 0 Or Val(dt2.Rows(i).Item("Tex_Close_Status").ToString) <> 0 Then
                            dgv_Details.Rows(n).Cells(13).Value = "1"

                            For J = 0 To dgv_Details.ColumnCount - 1
                                dgv_Details.Rows(n).Cells(J).Style.BackColor = Color.LightGray
                                dgv_Details.Rows(n).Cells(J).Style.ForeColor = Color.Red
                            Next
                            LockSTS = True
                        End If







                    Next i

                End If




                With dgv_Details_Total
                    If .RowCount = 0 Then .Rows.Add()
                    .Rows(0).Cells(2).Value = Val(dt1.Rows(0).Item("Total_Beam").ToString)
                    .Rows(0).Cells(3).Value = Val(dt1.Rows(0).Item("Total_Pcs").ToString)
                    .Rows(0).Cells(4).Value = Format(Val(dt1.Rows(0).Item("Total_Meters").ToString), "########0.00")
                    .Rows(0).Cells(9).Value = Format(Val(dt1.Rows(0).Item("Sizing_Tot_Weight").ToString), "#########0.000")
                End With

                dt2.Clear()
                dt2.Dispose()
                da2.Dispose()

            End If

            dt1.Clear()
            dt1.Dispose()
            da1.Dispose()

            da = New SqlClient.SqlDataAdapter("Select a.Total_meters ,a.Total_beams  from Pavu_Delivery_Selections_Processing_Details a where  a.Reference_Code<>'" & Trim(Pk_Condition) & Trim(NewCode) & "' and a.Delivery_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and  a.Total_meters < 0", con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If Val(dt.Rows(0).Item("Total_meters").ToString) < 0 Then
                    For j = 0 To dgv_Details.ColumnCount - 1
                        dgv_Details.Rows(n).Cells(j).Style.BackColor = Color.LightGray
                    Next j
                    LockSTS = True
                End If
            End If
            dt.Clear()

            If LockSTS = True Then

                dtp_Date.Enabled = False
                dtp_Date.BackColor = Color.LightGray

                cbo_Transport.Enabled = False
                cbo_Transport.BackColor = Color.LightGray

                cbo_Ledger.Enabled = False
                cbo_Ledger.BackColor = Color.LightGray

                cbo_VehicleNo.Enabled = False
                cbo_VehicleNo.BackColor = Color.LightGray

            End If

            Grid_Cell_DeSelect()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        If dtp_Date.Visible And dtp_Date.Enabled Then dtp_Date.Focus()

    End Sub

    Private Sub PavuDelivery_Entry_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Ledger.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Ledger.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_DeliveryTo.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "DELIVERY" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_DeliveryTo.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Transport.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "TRANSPORT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Transport.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_Vendor.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "VENDOR" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_Vendor.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_VendorName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "VENDOR" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_VendorName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_Count.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "COUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_Count.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

    Private Sub PavuDelivery_Entry_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim dt5 As New DataTable

        Me.Text = ""

        con.Open()
        'dgv_Details.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        If Common_Procedures.settings.Combine_Textile_Sizing_Software_Status = 1 Then
            TrnTo_DbName = Common_Procedures.get_Company_TextileDataBaseName(Trim(Val(Common_Procedures.CompGroupIdNo)))
            btn_Selection.Visible = True
            'Panel2.Enabled = False
            'dgv_Details.EditMode = DataGridViewEditMode.EditOnEnter
            'dgv_Details.SelectionMode = DataGridViewSelectionMode.CellSelect
            cbo_Ledger.Width = cbo_Ledger.Width - btn_Selection.Width - 20
        Else
            TrnTo_DbName = Common_Procedures.get_Company_DataBaseName(Trim(Val(Common_Procedures.CompGroupIdNo)))
            btn_Selection.Visible = False
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1282" Then
            txt_ElectronicRefNo.Size = New Size(203, 23)
            lbl_millname_caption.Visible = True
            cbo_MillName.Visible = True
            chk_Loaded.Visible = True
        Else
            txt_ElectronicRefNo.Size = New Size(431, 23)
            lbl_millname_caption.Visible = False
            cbo_MillName.Visible = False
            chk_Loaded.Visible = False
        End If


        lbl_Ends.Visible = True
        lbl_CountName.Visible = True
        Label12.Visible = True
        Label21.Visible = True
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1102" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1144" Then
            lbl_Ends.Visible = False
            lbl_CountName.Visible = False
            Label12.Visible = False
            Label21.Visible = False
        End If
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1155" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Or Trim(Common_Procedures.settings.CustomerCode) = "1282" Or Trim(Common_Procedures.settings.CustomerCode) = "1288" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1263" Then
            lbl_Del_Vendor.Text = "Vendor"
            cbo_DeliveryTo.Visible = False
            cbo_VendorName.Visible = True
            cbo_VendorName.BringToFront()
            cbo_VendorName.Width = cbo_DeliveryTo.Width
            cbo_VendorName.BackColor = Color.White
            dgv_Details.Columns(5).ReadOnly = True
        Else
            lbl_Del_Vendor.Text = "Delivery To"
            cbo_DeliveryTo.Visible = True
            cbo_VendorName.Visible = False
            cbo_DeliveryTo.BringToFront()
            dgv_Details.Columns(5).ReadOnly = False
        End If


        lbl_typeCaption.Visible = False
        cbo_Type.Visible = False
        btn_Selection.Visible = True
        If (Common_Procedures.settings.CustomerCode) = "1282" Then
            'lbl_typeCaption.Visible = True
            'cbo_Type.Visible = True
            btn_Selection.Visible = False
            cbo_Ledger.Width = txt_BookNo.Width
        Else
            cbo_Ledger.Width = txt_BookNo.Width - btn_Selection.Width - 20
        End If

        btn_SaveAll.Visible = False
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then
            btn_SaveAll.Visible = True
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1417" Then
            lbl_Del_Vendor.Text = "Vendor"
            cbo_DeliveryTo.Visible = False
            cbo_VendorName.Visible = True
            cbo_VendorName.BringToFront()
            cbo_VendorName.Width = cbo_DeliveryTo.Width
            cbo_VendorName.BackColor = Color.White
        End If

        cbo_Type.Items.Clear()
        cbo_Type.Items.Add(" ")
        cbo_Type.Items.Add("DIRECT")
        cbo_Type.Items.Add("SPEC")


        Cbo_RateFor.Items.Clear()
        Cbo_RateFor.Items.Add("")
        Cbo_RateFor.Items.Add("METERS")
        Cbo_RateFor.Items.Add("WEIGHT")

        Pnl_DosPrint.Visible = False
        Pnl_DosPrint.BringToFront()
        Pnl_DosPrint.Left = (Me.Width - Pnl_DosPrint.Width) \ 2
        Pnl_DosPrint.Top = (Me.Height - Pnl_DosPrint.Height) \ 2

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2

        pnl_Selection.Visible = False
        pnl_Selection.Left = (Me.Width - pnl_Selection.Width) \ 2
        pnl_Selection.Top = (Me.Height - pnl_Selection.Height) \ 2

        pnl_Print.Visible = False
        pnl_Print.BringToFront()
        pnl_Print.Left = (Me.Width - pnl_Print.Width) \ 2
        pnl_Print.Top = (Me.Height - pnl_Print.Height) \ 2

        ' btn_UserModification.Visible = False
        chk_Printed.Enabled = False
        If Val(Common_Procedures.User.IdNo) = 1 Then
            'btn_UserModification.Visible = True
            chk_Printed.Enabled = True
        End If


        btn_UserModification.Visible = False
        If Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_UserModification_Status = 1 Then
            btn_UserModification.Visible = True
        End If


        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Ledger.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_VendorName.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_BookNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_ElectronicRefNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_DeliveryTo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DateAndTimeOFSupply.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Rate.GotFocus, AddressOf ControlGotFocus
        AddHandler lbl_value.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Transport.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_VehicleNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Remarks.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_SetNo.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_Fromdate.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_ToDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_CountName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_EndsName.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Filter_Show.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Filter_Close.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_close.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_save.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print_Cancel.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print_Invoice.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print_Preprint.GotFocus, AddressOf ControlGotFocus
        AddHandler Btn_DosPrint.GotFocus, AddressOf ControlGotFocus
        AddHandler Btn_LaserPrint.GotFocus, AddressOf ControlGotFocus
        AddHandler Btn_DosCancel.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_Vendor.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Delivered.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Type.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_Count.GotFocus, AddressOf ControlGotFocus
        AddHandler Cbo_RateFor.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_MillName.GotFocus, AddressOf ControlGotFocus


        AddHandler txt_SetNo.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Ledger.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_VendorName.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_BookNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_ElectronicRefNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DateAndTimeOFSupply.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Rate.LostFocus, AddressOf ControlLostFocus
        AddHandler lbl_value.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_DeliveryTo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Transport.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_VehicleNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Remarks.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_Vendor.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Delivered.LostFocus, AddressOf ControlLostFocus
        AddHandler Cbo_RateFor.LostFocus, AddressOf ControlLostFocus


        AddHandler dtp_Filter_Fromdate.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_ToDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_CountName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_EndsName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_Count.LostFocus, AddressOf ControlLostFocus


        AddHandler btn_save.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_close.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Filter_Show.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Filter_Close.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print_Cancel.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print_Invoice.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print_Preprint.LostFocus, AddressOf ControlLostFocus
        AddHandler Btn_DosPrint.LostFocus, AddressOf ControlLostFocus
        AddHandler Btn_LaserPrint.LostFocus, AddressOf ControlLostFocus
        AddHandler Btn_DosCancel.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Type.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_MillName.LostFocus, AddressOf ControlLostFocus


        AddHandler txt_DateAndTimeOFSupply.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_BookNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_ElectronicRefNo.KeyDown, AddressOf TextBoxControlKeyDown
        ' AddHandler txt_Approx_Value.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Remarks.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler dtp_Filter_Fromdate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_ToDate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_DateAndTimeOFSupply.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_Rate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler lbl_value.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_BookNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Remarks.KeyPress, AddressOf TextBoxControlKeyPress

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1163" Then
            dtp_Time.Visible = True
        End If

        Filter_Status = False
        FrmLdSTS = True

        If Common_Procedures.settings.CustomerCode = "1288" Then
            dgv_Details.Columns(11).Visible = True
            dgv_Details.Columns(12).Visible = True
        Else
            dgv_Details.Columns(11).Visible = False
            dgv_Details.Columns(12).Visible = False
        End If

        new_record()

    End Sub

    Private Sub PavuDelivery_Entry_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub PavuDelivery_Entry_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
                    Exit Sub

                ElseIf pnl_Selection.Visible = True Then
                    btn_Close_Selection_Click(sender, e)
                    Exit Sub

                ElseIf pnl_Print.Visible = True Then
                    btn_print_Close_Click(sender, e)
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

    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
        Dim dgv1 As New DataGridView
        Dim Pr_kyData As Keys

        If ActiveControl.Name = dgv_Details.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            On Error Resume Next

            dgv1 = Nothing

            If ActiveControl.Name = dgv_Details.Name Then
                dgv1 = dgv_Details

            ElseIf dgv_Details.IsCurrentRowDirty = True Then
                dgv1 = dgv_Details

            Else
                dgv1 = dgv_Details

            End If

            If IsNothing(dgv1) = True Then
                Return MyBase.ProcessCmdKey(msg, keyData)
                Exit Function
            End If

            Pr_kyData = Prev_kyData
            Prev_kyData = keyData

            With dgv1

                If (keyData = Keys.Enter Or keyData = Keys.Down Or keyData = 131085) Then

                    If Pr_kyData = 131089 Then
                        cbo_Transport.Focus()

                    ElseIf .CurrentCell.ColumnIndex >= 9 Then
                        If .CurrentCell.RowIndex = .RowCount - 1 Then
                            cbo_Transport.Focus()

                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)
                        End If






                    ElseIf Trim(cbo_Type.Text) = "DIRECT" And .CurrentCell.ColumnIndex = 5 Then

                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(7)

                    ElseIf Trim(cbo_Type.Text) <> "DIRECT" And .CurrentCell.ColumnIndex >= 5 Then

                        If .CurrentCell.RowIndex = .RowCount - 1 Then
                            cbo_Transport.Focus()


                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(5)
                            ' .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)
                        End If



                    Else

                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

                    End If

                    Return True


                ElseIf keyData = Keys.Up Then

                    If .CurrentCell.ColumnIndex <= .ColumnCount Then
                        If .CurrentCell.RowIndex = 0 And .CurrentCell.ColumnIndex = 1 Then
                            'txt_ElectronicRefNo
                            cbo_MillName.Focus()

                        ElseIf Trim(cbo_Type.Text) = "DIRECT" And .CurrentCell.ColumnIndex = 7 Then
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(5)

                        ElseIf Trim(cbo_Type.Text) <> "DIRECT" And .CurrentCell.ColumnIndex = 5 Then
                            .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(5)

                        ElseIf .CurrentCell.ColumnIndex = 1 Then
                            .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(9)
                        Else

                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)

                        End If

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
        Dim vDbName As String = ""
        Dim LedIdNo As Integer = 0
        Dim TexStk_iD As Integer = 0
        Dim UID As Single
        Dim vUsrNm As String = "", vAcPwd As String = "", vUnAcPwd As String = ""
        Dim vOrdByNo As String = 0
        Dim Nr As Long
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


        vDbName = ""
        If Trim(TrnTo_DbName) <> "" Then
            vDbName = Trim(TrnTo_DbName) & ".."
        End If
        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.ENTRY_SIZING_JOBWORK_MODULE_PAVU_DELIVERY, New_Entry, Me, con, "Sizing_Pavu_Delivery_Head", "Pavu_Delivery_Code", NewCode, "Pavu_Delivery_Date", "(Pavu_Delivery_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub


        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If
        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Da = New SqlClient.SqlDataAdapter("select * from Sizing_Pavu_Delivery_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Pavu_Delivery_Code = '" & Trim(NewCode) & "'", con)
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




        Da2 = New SqlClient.SqlDataAdapter("Select a.Total_meters ,a.Total_beams  from Pavu_Delivery_Selections_Processing_Details a where  a.Reference_Code<>'" & Trim(Pk_Condition) & Trim(NewCode) & "' and a.Delivery_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and  a.Total_meters < 0", con)
        Dt2 = New DataTable
        Da2.Fill(dt2)
        If dt2.Rows.Count > 0 Then
            MessageBox.Show("Already Pavu Receipt Prepared", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If
        dt2.Clear()




        trans = con.BeginTransaction

        'Try

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.Transaction = trans

            vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text)

            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "DELETE", "Sizing_Pavu_Delivery_Head", "Pavu_Delivery_Code", Val(lbl_Company.Tag), NewCode, lbl_DcNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "", "", "Pavu_Delivery_Code, Company_IdNo, for_OrderBy", trans)
            Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "DELETE", "Sizing_Pavu_Delivery_Details", "Pavu_Delivery_Code", Val(lbl_Company.Tag), NewCode, lbl_DcNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "Set_No, Beam_No, Noof_Pcs, Meters, DeliveryTo_Name, Ends_Name, Count_IdNo, Meters_Pc, Mill_IdNo, Sort_No", "Sl_No", "Pavu_Delivery_Code, For_OrderBy, Company_IdNo, Pavu_Delivery_No, Pavu_Delivery_Date, Ledger_IdNo, Set_Code", trans)

        If Common_Procedures.settings.AUTOPOSTING_OF_SIZINGSOFTWARE_PAVUDELIVERY_AS_TEXTILESOFWTARE_PAVURECEIPT = 1 Or Common_Procedures.settings.Combine_Textile_Sizing_Software_Status = 1 Then

            Dim TexComp_ID As Integer
            Dim vEntLedIdNo As Integer
            Dim TexLed_ID As Integer

            TexComp_ID = 0
            TexLed_ID = 0
            If Common_Procedures.settings.AUTOPOSTING_OF_SIZINGSOFTWARE_PAVUDELIVERY_AS_TEXTILESOFWTARE_PAVURECEIPT = 1 Then
                TexComp_ID = Val(lbl_Company.Tag)
                TexLed_ID = Common_Procedures.get_FieldValue(con, "company_head", "Textile_Unit_LedgerIdNo", "(company_idno = " & Str(Val(lbl_Company.Tag)) & ")", , trans)
                vEntLedIdNo = Common_Procedures.get_FieldValue(con, "Sizing_Pavu_Delivery_Head", "ledger_idno", "(Pavu_Delivery_Code = '" & Trim(NewCode) & "')", , trans)

            ElseIf Common_Procedures.settings.Combine_Textile_Sizing_Software_Status = 1 Then

                vEntLedIdNo = Common_Procedures.get_FieldValue(con, "Sizing_Pavu_Delivery_Head", "ledger_idno", "(Pavu_Delivery_Code = '" & Trim(NewCode) & "')", , trans)
                TexComp_ID = Common_Procedures.get_FieldValue(con, "ledger_head", "Textile_To_CompanyIdNo", "(ledger_idno = " & Str(Val(vEntLedIdNo)) & ")", , trans)


            End If

            If (vEntLedIdNo <> 0 And TexLed_ID <> 0 And TexLed_ID = vEntLedIdNo And Common_Procedures.settings.AUTOPOSTING_OF_SIZINGSOFTWARE_PAVUDELIVERY_AS_TEXTILESOFWTARE_PAVURECEIPT = 1) Or (Val(TexComp_ID) <> 0 And Common_Procedures.settings.Combine_Textile_Sizing_Software_Status = 1) Then

                Da = New SqlClient.SqlDataAdapter("select * from Stock_SizedPavu_Processing_Details Where  Reference_Code = '" & Trim(Pk_Condition_Tex) & Trim(NewCode) & "' and (Pavu_Delivery_Code <> '' or Pavu_Delivery_Increment <> 0 or Beam_Knotting_Code <> '' or Production_Meters <> 0 or Close_Status <> 0)", con)
                Da.SelectCommand.Transaction = trans
                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then
                    Throw New ApplicationException("Invalid Deletion : Already this Warp Beam used in Textile")
                    Exit Sub
                End If
                Dt1.Clear()

                Nr = 0
                cmd.CommandText = "delete from " & Trim(vDbName) & "SizSoft_Pavu_Delivery_Head where Pavu_Delivery_Code = '" & Trim(NewCode) & "' and ISNULL(Sizing_Specification_Code, '') = ''"
                Nr = cmd.ExecuteNonQuery()
                If Nr = 0 Then
                    Throw New ApplicationException("Invalid Editing : Already Specification entered against this Pavu Delivery in Textile Software")
                    Exit Sub
                End If
                cmd.CommandText = "delete from " & Trim(vDbName) & "SizSoft_Pavu_Delivery_Details where Pavu_Delivery_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()
                cmd.CommandText = "Delete from " & Trim(vDbName) & "Stock_Pavu_Processing_Details Where Reference_Code = '" & Trim(Pk_Condition_Tex) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()
                cmd.CommandText = "Delete from " & Trim(vDbName) & "Stock_SizedPavu_Processing_Details Where Reference_Code = '" & Trim(Pk_Condition_Tex) & Trim(NewCode) & "' and Pavu_Delivery_Code = '' and Pavu_Delivery_Increment = 0 and Beam_Knotting_Code = '' and Production_Meters = 0 and Close_Status = 0"
                cmd.ExecuteNonQuery()

            End If

        End If


        cmd.CommandText = "Delete from Pavu_Delivery_Selections_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Update Stock_SizedPavu_Processing_Details set Pavu_Delivery_Code = '', Pavu_Delivery_Increment = Pavu_Delivery_Increment - 1, DeliveryTo_Name = '' Where Pavu_Delivery_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()


            cmd.CommandText = "delete from Sizing_Pavu_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Pavu_Delivery_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            trans.Commit()

            Dt1.Dispose()
            Da.Dispose()
            trans.Dispose()
            cmd.Dispose()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        'Catch ex As Exception
        '    trans.Rollback()
        '    MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        'Finally
        '    Dt1.Dispose()
        '    Da.Dispose()

        'End Try

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

            da = New SqlClient.SqlDataAdapter("select distinct(Ends_Name) from Stock_SizedPavu_Processing_Details order by Ends_Name", con)
            da.Fill(dt3)
            cbo_Filter_EndsName.DataSource = dt3
            cbo_Filter_EndsName.DisplayMember = "Ends_Name"

            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate   '.Date.ToShortDateString
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate   '.Date.ToShortDateString
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_CountName.Text = ""
            cbo_Filter_EndsName.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_CountName.SelectedIndex = -1
            cbo_Filter_EndsName.SelectedIndex = -1
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

            da = New SqlClient.SqlDataAdapter("select top 1 Pavu_Delivery_No from Sizing_Pavu_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Pavu_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Pavu_Delivery_No", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 Pavu_Delivery_No from Sizing_Pavu_Delivery_Head where for_orderby > " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Pavu_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Pavu_Delivery_No", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 Pavu_Delivery_No from Sizing_Pavu_Delivery_Head where for_orderby < " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Pavu_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Pavu_Delivery_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 Pavu_Delivery_No from Sizing_Pavu_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Pavu_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Pavu_Delivery_No desc", con)
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

            lbl_DcNo.Text = Common_Procedures.get_MaxCode(con, "Sizing_Pavu_Delivery_Head", "Pavu_Delivery_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)
            dtp_Time.Text = Format(Now, "hh:mm tt").ToString

            lbl_DcNo.ForeColor = Color.Red
            If Trim(txt_DateAndTimeOFSupply.Text) = "" Then txt_DateAndTimeOFSupply.Text = Format(Now, "hh:mm tt")
            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        dt.Dispose()
        da.Dispose()

        If dtp_Date.Visible And dtp_Date.Enabled Then dtp_Date.Focus()

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim RecCode As String

        Try

            inpno = InputBox("Enter Dc.No.", "FOR FINDING...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Pavu_Delivery_No from Sizing_Pavu_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Pavu_Delivery_Code = '" & Trim(RecCode) & "'", con)
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
                MessageBox.Show("Dc No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

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

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.ENTRY_SIZING_JOBWORK_MODULE_PAVU_DELIVERY, New_Entry, Me) = False Then Exit Sub

        Try

            inpno = InputBox("Enter New Dc No.", "FOR NEW DC INSERTION...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Pavu_Delivery_No from Sizing_Pavu_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Pavu_Delivery_Code = '" & Trim(RecCode) & "'", con)
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
                    MessageBox.Show("Invalid Dc No", "DOES NOT INSERT NEW DC...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

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
        Dim Dt1 As New DataTable
        Dim Da2 As New SqlClient.SqlDataAdapter
        Dim Dt2 As New DataTable
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim led_id As Integer = 0
        Dim mill_det_Id As Integer = 0
        Dim trans_id As Integer = 0
        Dim Bw_id As Integer = 0
        Dim Cnt_ID As Integer = 0
        Dim Mill_ID As Integer = 0
        Dim Del_ID As Integer = 0
        Dim Mil_ID As Integer = 0
        Dim Sno As Integer = 0, vTEX_Pvu_SNO As Integer = 0
        Dim Partcls As String = ""
        Dim PBlNo As String = ""
        Dim EntID As String = ""
        Dim vTotBms As Single, vTotPcs As Single, vTotMtrs As Single, nTotWegt As Single
        Dim Nr As Long
        Dim Vec_No As String = ""
        Dim Vndr_Id As Integer = 0
        Dim VndrNm_Id As Integer = 0
        Dim vSetDte As Date
        Dim TexComp_ID As String = 0
        Dim TexLed_ID As String = 0, SizLed_ID As Integer = 0
        Dim TexVnd_ID As String = 0
        Dim vEntLedIdNo As String = 0
        Dim TexCnt_iD As String = 0
        Dim vSetCd As String = ""
        Dim Selc_SetCode As String = ""
        Dim TexEdsCnt_ID As String = 0
        Dim vEdsCnt_ID As String = 0
        Dim Mtr_Pc As String = 0
        Dim vNewFrmTYpe As String = ""
        Dim Close_STS As Single = 0
        Dim SQL As String = ""
        Dim vDup_SetBmNo As String = ""
        Dim vOrdByNo As String = 0
        Dim vDbName As String = ""
        Dim vDup_SetNo As String = ""
        Dim vCOMP_LEDIDNO As Integer = 0
        Dim vDELVLED_COMPIDNO As Integer = 0
        Dim vSELC_RCVDIDNO As Integer
        Dim vREC_Ledtype As String = ""
        Dim vDELV_Ledtype As String = ""
        Dim vDELVAT_IDNO As Integer = 0
        Dim vENTDB_DelvToIDno As String = 0
        Dim vCREATED_DTTM_TXT As String = ""
        Dim vMODIFIED_DTTM_TXT As String = ""

        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()




        vDbName = ""
        If Trim(TrnTo_DbName) <> "" Then
            vDbName = Trim(TrnTo_DbName) & ".."
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Close_STS = 0
        If chk_Loaded.Checked = True Then Close_STS = 1

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Val(Common_Procedures.User.IdNo) = 0 Then
            MessageBox.Show("Invalid User Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.ENTRY_SIZING_JOBWORK_MODULE_PAVU_DELIVERY, New_Entry, Me, con, "Sizing_Pavu_Delivery_Head", "Pavu_Delivery_Date", "(Pavu_Delivery_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        Da2 = New SqlClient.SqlDataAdapter("Select a.Total_bags ,a.Total_Weight  from Yarn_Delivery_Selections_Processing_Details a where  a.Reference_Code <> '" & Trim(Pk_Condition) & Trim(NewCode) & "' and a.Delivery_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and a.Total_weight < 0 ", con)
        Dt2 = New DataTable
        Da2.Fill(Dt2)
        If Dt2.Rows.Count > 0 Then
            If Val(Dt2.Rows(0).Item("Total_weight").ToString) < 0 Then
                MessageBox.Show("Already Pavu Receipt Prepared", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub
            End If
        End If
        Dt2.Clear()

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

        led_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
        If led_id = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
            Exit Sub
        End If

        Mill_ID = Common_Procedures.Mill_NameToIdNo(con, cbo_MillName.Text)


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
        Del_ID = Common_Procedures.Despatch_NameToIdNo(con, cbo_DeliveryTo.Text)
        VndrNm_Id = Common_Procedures.Vendor_AlaisNameToIdNo(con, cbo_VendorName.Text)


        If Trim(UCase(cbo_Type.Text)) = "" Or (Trim(UCase(cbo_Type.Text)) <> "DIRECT") Then
            cbo_Type.Text = "SPEC"
        End If


        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1102" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1144" Then '---- Ganesh karthik Sizing (Somanur)
            If Del_ID = 0 Then

                cbo_DeliveryTo.Text = cbo_Ledger.Text
                Del_ID = Common_Procedures.Delivery_AlaisNameToIdNo(con, cbo_DeliveryTo.Text)
            End If

        End If

        vDup_SetNo = ""

        For i = 0 To dgv_Details.RowCount - 1

            If Val(dgv_Details.Rows(i).Cells(4).Value) <> 0 Then

                If Trim(dgv_Details.Rows(i).Cells(1).Value) = "" Then
                    MessageBox.Show("Invalid Set No", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If

                If Trim(dgv_Details.Rows(i).Cells(6).Value) = "" And Trim(UCase(cbo_Type.Text)) <> "DIRECT" Then
                    MessageBox.Show("Invalid Set Code", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If

                If Trim(dgv_Details.Rows(i).Cells(2).Value) = "" Then
                    ' Beam No is not Entered
                    MessageBox.Show("Invalid Beam No", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    If dgv_Details.Enabled And dgv_Details.Visible Then dgv_Details.Focus()
                    Exit Sub

                Else
                    ' ========================    DUPLICATE SETNO AND BEAM NO VALIDATION ====================================================================
                    If InStr(1, Trim(UCase(vDup_SetBmNo)), "~" & Trim(UCase(dgv_Details.Rows(i).Cells(2).Value)) & "~") > 0 Then
                        MessageBox.Show("Duplicate BeamNo(" & Trim(dgv_Details.Rows(i).Cells(2).Value) & ")", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(2)
                        dgv_Details.Focus()
                        Exit Sub
                    End If

                    If Trim(UCase(cbo_Type.Text)) = "DIRECT" Then

                        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

                        ' Beam No is entered
                        'SQL = "select * from Sizing_Pavu_Delivery_Details where Set_No = '" & Trim(dgv_Details.Rows(i).Cells(1).Value) & _
                        '                    "' and Beam_No = '" & Trim(dgv_Details.Rows(i).Cells(2).Value) & _
                        '                    "' and Pavu_Delivery_Code <> '" & Trim(NewCode) & _
                        '                    "' and Pavu_Delivery_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "'"
                        SQL = "select * from Sizing_Pavu_Delivery_Details where " &
                                           " Beam_No = '" & Trim(dgv_Details.Rows(i).Cells(2).Value) &
                                           "' and Pavu_Delivery_Code <> '" & Trim(NewCode) &
                                           "' and Pavu_Delivery_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "'"

                        Da = New SqlClient.SqlDataAdapter(SQL, con)
                        Dt1 = New DataTable
                        Da.Fill(Dt1)
                        If Dt1.Rows.Count > 0 Then
                            MessageBox.Show("Duplicate BeamNo(" & Trim(dgv_Details.Rows(i).Cells(2).Value) & ") - DC No - " & Trim(Dt1.Rows(0).Item("Pavu_Delivery_No").ToString) & " ", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                            If dgv_Details.Enabled And dgv_Details.Visible Then dgv_Details.Focus()
                            Exit Sub
                        End If
                        Dt1.Clear()
                        Dt1.Dispose()
                        Da.Dispose()
                    End If
                    ' ========================    DUPLICATE SETNO AND BEAM NO VALIDATION ====================================================================
                End If

                vDup_SetBmNo = Trim(vDup_SetBmNo) & "~" & Trim(dgv_Details.Rows(i).Cells(2).Value) & "~"
                'vDup_SetBmNo = Trim(vDup_SetBmNo) & "~" & Trim(UCase(dgv_Details.Rows(i).Cells(1).Value)) & "|" & Trim(dgv_Details.Rows(i).Cells(2).Value) & "~"

                If Trim(UCase(cbo_Type.Text)) <> "DIRECT" Then

                    vSetDte = #1/1/2000#
                    Da = New SqlClient.SqlDataAdapter("select * from Stock_SizedPavu_Processing_Details where Set_Code = '" & Trim(dgv_Details.Rows(i).Cells(6).Value) & "'", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    If Dt1.Rows.Count > 0 Then
                        vSetDte = Dt1.Rows(0).Item("reference_date")

                        If DateDiff(DateInterval.Day, vSetDte, dtp_Date.Value.Date) < 0 Then
                            MessageBox.Show("Invoice Date - Delivery Date Should not less than Set Date " & Chr(13) & "(Set No : " & Trim(dgv_Details.Rows(i).Cells(1).Value) & "     Date : " & vSetDte.ToShortDateString & ")", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                            If dtp_Date.Enabled = True And dtp_Date.Visible = True Then dtp_Date.Focus()
                            Exit Sub
                        End If

                    End If
                    Dt1.Clear()

                End If


                If Common_Procedures.settings.AUTOPOSTING_OF_SIZINGSOFTWARE_PAVUDELIVERY_AS_TEXTILESOFWTARE_PAVURECEIPT = 1 Then

                    If Trim(vDup_SetNo) = "" Then
                        vDup_SetNo = Trim(dgv_Details.Rows(i).Cells(6).Value)

                    Else

                        If Trim(UCase(vDup_SetNo)) <> Trim(UCase(dgv_Details.Rows(i).Cells(6).Value)) Then
                            MessageBox.Show("Do not select multiple SetNo", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                            dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(2)
                            dgv_Details.Focus()
                            Exit Sub
                        End If

                    End If

                End If

            End If

        Next

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1102" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1144" Then '---- Ganesh karthik Sizing (Somanur)
            If dgv_Details.Rows.Count > 18 Then
                MessageBox.Show("Does Not Select More than 18 BeamNo", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
                Exit Sub
            End If
        End If
        vTotBms = 0 : vTotPcs = 0 : vTotMtrs = 0 : nTotWegt = 0
        If dgv_Details_Total.RowCount > 0 Then
            vTotBms = Val(dgv_Details_Total.Rows(0).Cells(2).Value())
            vTotPcs = Val(dgv_Details_Total.Rows(0).Cells(3).Value())
            vTotMtrs = Val(dgv_Details_Total.Rows(0).Cells(4).Value())
            nTotWegt = Val(dgv_Details_Total.Rows(0).Cells(9).Value())
        End If
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1102" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1144" Then '---- Ganesh karthik Sizing (Somanur)
            If Trim(cbo_VehicleNo.Text) = "" Then
                MessageBox.Show("Invalid Vehicle No", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_VehicleNo.Enabled And cbo_VehicleNo.Visible Then cbo_VehicleNo.Focus()
                Exit Sub
            End If
            'If Val(txt_Approx_Value.Text) = 0 Then
            '    MessageBox.Show("Invalid Approx Value", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            '    If txt_Approx_Value.Enabled And txt_Approx_Value.Visible Then txt_Approx_Value.Focus()
            '    Exit Sub
            'End If


        End If

        Vec_No = Trim(cbo_VehicleNo.Text)
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1102" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1144" Then '---- Ganesh karthik Sizing (Somanur)
            Vec_No = Trim(cbo_VehicleNo.Text)
            ' Vec_No = IIf(IsDBNull(cbo_VehicleNo.Text), "", "")
            Vec_No = Vec_No.Replace(" ", "")
            Vec_No = (UCase(Vec_No))
        End If
        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Da = New SqlClient.SqlDataAdapter("select * from Sizing_Pavu_Delivery_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Pavu_Delivery_Code = '" & Trim(NewCode) & "'", con)
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



        tr = con.BeginTransaction

        'Try

        If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_DcNo.Text = Common_Procedures.get_MaxCode(con, "Sizing_Pavu_Delivery_Head", "Pavu_Delivery_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@DcDate", dtp_Date.Value.Date)

            vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text)

        vCREATED_DTTM_TXT = ""
        vMODIFIED_DTTM_TXT = ""

        vCREATED_DTTM_TXT = Trim(Format(Now, "dd-MM-yyyy hh:mm tt"))
        cmd.Parameters.AddWithValue("@createddatetime", Now)


        vMODIFIED_DTTM_TXT = Trim(Format(Now, "dd-MM-yyyy hh:mm tt"))
        cmd.Parameters.AddWithValue("@modifieddatetime", Now)


        If New_Entry = True Then

            If Trim(txt_DateAndTimeOFSupply.Text) = "" Then txt_DateAndTimeOFSupply.Text = Format(Now, "hh:mm tt")
            cmd.CommandText = "Insert into Sizing_Pavu_Delivery_Head(  User_IdNo , Pavu_Delivery_Code    ,  Company_IdNo                    , Pavu_Delivery_No             , for_OrderBy               , Pavu_Delivery_Date, Book_No                        , Ledger_IdNo             , Count_Name                        , Ends_Name                      , Transport_IdNo            , Vehicle_No            , Remarks                         , Total_Beam               , Total_Pcs                , Total_Meters              , Electronic_Reference_No                , Date_And_Time_Of_Supply                     , DeliveryTO_Idno  , Approx_Value              , Entry_Time_Text             ,   Vendor_Idno        , Rate                      ,   Delivered_By                   , Invoice_Selection_Type              ,Rate_For                        ,Sizing_Tot_Weight     ,            Mill_IdNo        , Loaded_by_Our_Employee  ,       created_useridno           ,   created_DateTime,          created_DateTime_Text    , Last_modified_useridno, Last_modified_DateTime, Last_modified_DateTime_Text )  " &
                               "                 Values (" & Str(Common_Procedures.User.IdNo) & ",'" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(vOrdByNo)) & ", @DcDate           , '" & Trim(txt_BookNo.Text) & "', " & Str(Val(led_id)) & ", '" & Trim(lbl_CountName.Text) & "', " & Str(Val(lbl_Ends.Text)) & ", " & Str(Val(trans_id)) & ", '" & Trim(Vec_No) & "', '" & Trim(txt_Remarks.Text) & "', " & Str(Val(vTotBms)) & ", " & Str(Val(vTotPcs)) & ", " & Str(Val(vTotMtrs)) & ",'" & Trim(txt_ElectronicRefNo.Text) & "','" & Trim(txt_DateAndTimeOFSupply.Text) & "'," & Val(Del_ID) & "," & Val(lbl_value.Text) & ",'" & Trim(dtp_Time.Text) & "'," & Val(VndrNm_Id) & ", " & Val(txt_Rate.Text) & ",'" & Trim(cbo_Delivered.Text) & "', '" & Trim(UCase(cbo_Type.Text)) & "','" & Trim(Cbo_RateFor.Text) & "', " & Val(nTotWegt) & ", " & Val(Mill_ID) & "      ," & Val(Close_STS) & ",       " & Str(Val(Common_Procedures.User.IdNo)) & ",  @createddatetime ,  '" & Trim(vCREATED_DTTM_TXT) & "',              0        ,     NUll              ,          ''    ) "
            cmd.ExecuteNonQuery()

        Else

            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "OLD", "Sizing_Pavu_Delivery_Head", "Pavu_Delivery_Code", Val(lbl_Company.Tag), NewCode, lbl_DcNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Pavu_Delivery_Code, Company_IdNo, for_OrderBy", tr)
            Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "OLD", "Sizing_Pavu_Delivery_Details", "Pavu_Delivery_Code", Val(lbl_Company.Tag), NewCode, lbl_DcNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "Set_No, Beam_No, Noof_Pcs, Meters, DeliveryTo_Name, Ends_Name, Count_IdNo, Meters_Pc, Mill_IdNo, Sort_No", "Sl_No", "Pavu_Delivery_Code, For_OrderBy, Company_IdNo, Pavu_Delivery_No, Pavu_Delivery_Date, Ledger_IdNo, Set_Code", tr)



            cmd.CommandText = "Update Stock_SizedPavu_Processing_Details set Pavu_Delivery_Code = '', Pavu_Delivery_Increment = Pavu_Delivery_Increment - 1, DeliveryTo_Name = '' Where Pavu_Delivery_Code = '" & Trim(NewCode) & "' and Company_IdNo = " & Str(Val(lbl_Company.Tag))
            cmd.ExecuteNonQuery()

            If Common_Procedures.settings.AUTOPOSTING_OF_SIZINGSOFTWARE_PAVUDELIVERY_AS_TEXTILESOFWTARE_PAVURECEIPT = 1 Or Common_Procedures.settings.Combine_Textile_Sizing_Software_Status = 1 Then

                TexComp_ID = 0
                TexLed_ID = 0
                If Common_Procedures.settings.AUTOPOSTING_OF_SIZINGSOFTWARE_PAVUDELIVERY_AS_TEXTILESOFWTARE_PAVURECEIPT = 1 Then
                    TexComp_ID = Val(lbl_Company.Tag)
                    TexLed_ID = Common_Procedures.get_FieldValue(con, "company_head", "Textile_Unit_LedgerIdNo", "(company_idno = " & Str(Val(lbl_Company.Tag)) & ")", , tr)
                    vEntLedIdNo = Common_Procedures.get_FieldValue(con, "Sizing_Pavu_Delivery_Head", "ledger_idno", "(Pavu_Delivery_Code = '" & Trim(NewCode) & "')", , tr)

                ElseIf Common_Procedures.settings.Combine_Textile_Sizing_Software_Status = 1 Then
                    vEntLedIdNo = Common_Procedures.get_FieldValue(con, "Sizing_Pavu_Delivery_Head", "ledger_idno", "(Pavu_Delivery_Code = '" & Trim(NewCode) & "')", , tr)
                    TexComp_ID = Common_Procedures.get_FieldValue(con, "ledger_head", "Textile_To_CompanyIdNo", "(ledger_idno = " & Str(Val(vEntLedIdNo)) & ")", , tr)


                End If

                If (vEntLedIdNo <> 0 And TexLed_ID <> 0 And TexLed_ID = vEntLedIdNo And Common_Procedures.settings.AUTOPOSTING_OF_SIZINGSOFTWARE_PAVUDELIVERY_AS_TEXTILESOFWTARE_PAVURECEIPT = 1) Or (Val(TexComp_ID) <> 0 And Common_Procedures.settings.Combine_Textile_Sizing_Software_Status = 1) Then

                    cmd.CommandText = "Delete from " & Trim(vDbName) & "SizSoft_Pavu_Delivery_Head Where Pavu_Delivery_Code = '" & Trim(NewCode) & "' and ISNULL(Sizing_Specification_Code, '') = ''"
                    Nr = cmd.ExecuteNonQuery()
                    If Nr = 0 Then
                        Throw New ApplicationException("Invalid Editing : Already Specification entered against this Pavu Delivery in Textile Software")
                        Exit Sub
                    End If
                    cmd.CommandText = "Delete from " & Trim(vDbName) & "SizSoft_Pavu_Delivery_Details Where Pavu_Delivery_Code = '" & Trim(NewCode) & "'"
                    Nr = cmd.ExecuteNonQuery()
                    cmd.CommandText = "Delete from " & Trim(vDbName) & "Stock_Pavu_Processing_Details Where  Reference_Code = '" & Trim(Pk_Condition_Tex) & Trim(NewCode) & "'"
                    Nr = cmd.ExecuteNonQuery()
                    cmd.CommandText = "Delete from " & Trim(vDbName) & "Stock_SizedPavu_Processing_Details Where Reference_Code = '" & Trim(Pk_Condition_Tex) & Trim(NewCode) & "' and Pavu_Delivery_Code = '' and Pavu_Delivery_Increment = 0 and Beam_Knotting_Code = '' and Production_Meters = 0 and Close_Status = 0"
                    Nr = cmd.ExecuteNonQuery()
                End If

            End If

            cmd.CommandText = "Update Sizing_Pavu_Delivery_Head set User_IdNo =" & Str(Common_Procedures.User.IdNo) & " , Pavu_Delivery_Date = @DcDate, Book_No = '" & Trim(txt_BookNo.Text) & "', Ledger_IdNo = " & Str(Val(led_id)) & ", Count_Name = '" & Trim(lbl_CountName.Text) & "', Ends_Name = " & Str(Val(lbl_Ends.Text)) & ", Transport_IdNo = " & Str(Val(trans_id)) & ", Vehicle_No = '" & Trim(Vec_No) & "', Entry_Time_Text = '" & Trim(dtp_Time.Text) & "', Remarks = '" & Trim(txt_Remarks.Text) & "', Total_Beam = " & Str(Val(vTotBms)) & ", Total_Pcs = " & Str(Val(vTotPcs)) & ", Total_Meters = " & Str(Val(vTotMtrs)) & ",Electronic_Reference_No = '" & Trim(txt_ElectronicRefNo.Text) & "',Date_And_Time_Of_Supply = '" & Trim(txt_DateAndTimeOFSupply.Text) & "',DeliveryTo_IdNo = " & Val(Del_ID) & " , Approx_Value = " & Val(lbl_value.Text) & " ,  Vendor_IdNo = " & Val(VndrNm_Id) & ", Rate = " & Val(txt_Rate.Text) & ", Delivered_By = '" & Trim(cbo_Delivered.Text) & "', Invoice_Selection_Type = '" & Trim(UCase(cbo_Type.Text)) & "' , Rate_For = '" & Trim(Cbo_RateFor.Text) & "',Sizing_Tot_Weight =  " & Val(nTotWegt) & ",Mill_IdNo = " & Trim(Mill_ID) & ",Loaded_by_Our_Employee=" & Val(Close_STS) & " , Last_modified_useridno = " & Str(Val(Common_Procedures.User.IdNo)) & ", Last_modified_DateTime = @modifieddatetime, Last_modified_DateTime_Text = '" & Trim(vMODIFIED_DTTM_TXT) & "' Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Pavu_Delivery_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Pavu_Delivery_Selections_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()


        End If


        Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "NEW", "Sizing_Pavu_Delivery_Head", "Pavu_Delivery_Code", Val(lbl_Company.Tag), NewCode, lbl_DcNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Pavu_Delivery_Code, Company_IdNo, for_OrderBy", tr)

            EntID = Trim(Pk_Condition_Tex) & Trim(lbl_DcNo.Text)
            Partcls = "Siz.Pavu Delv : Dc No. " & Trim(lbl_DcNo.Text)
            PBlNo = Trim(lbl_DcNo.Text)

            TexComp_ID = 0
            TexLed_ID = 0
            TexVnd_ID = 0
            SizLed_ID = 0

            If Common_Procedures.settings.AUTOPOSTING_OF_SIZINGSOFTWARE_PAVUDELIVERY_AS_TEXTILESOFWTARE_PAVURECEIPT = 1 Or Common_Procedures.settings.Combine_Textile_Sizing_Software_Status = 1 Then

                If Common_Procedures.settings.AUTOPOSTING_OF_SIZINGSOFTWARE_PAVUDELIVERY_AS_TEXTILESOFWTARE_PAVURECEIPT = 1 Then
                    TexComp_ID = Val(lbl_Company.Tag)
                ElseIf Common_Procedures.settings.Combine_Textile_Sizing_Software_Status = 1 Then
                    TexComp_ID = Common_Procedures.get_FieldValue(con, "ledger_head", "Textile_To_CompanyIdNo", "(ledger_idno = " & Str(Val(led_id)) & ")", , tr)
                End If

                TexLed_ID = 0
                SizLed_ID = 0
                If Common_Procedures.settings.AUTOPOSTING_OF_SIZINGSOFTWARE_PAVUDELIVERY_AS_TEXTILESOFWTARE_PAVURECEIPT = 1 Then
                    TexLed_ID = Common_Procedures.get_FieldValue(con, "company_head", "Textile_Unit_LedgerIdNo", "(company_idno = " & Str(Val(lbl_Company.Tag)) & ")", , tr)
                    SizLed_ID = Common_Procedures.get_FieldValue(con, "company_head", "Sizing_Unit_LedgerIdNo", "(company_idno = " & Str(Val(lbl_Company.Tag)) & ")", , tr)
                ElseIf Common_Procedures.settings.Combine_Textile_Sizing_Software_Status = 1 Then
                    TexLed_ID = Common_Procedures.get_FieldValue(con, "company_head", "Textile_To_SizingIdNo", "(company_idno = " & Str(Val(lbl_Company.Tag)) & ")", , tr)
                End If

                If (TexLed_ID <> 0 And TexLed_ID = led_id And Common_Procedures.settings.AUTOPOSTING_OF_SIZINGSOFTWARE_PAVUDELIVERY_AS_TEXTILESOFWTARE_PAVURECEIPT = 1) Or (Val(TexComp_ID) <> 0 And Common_Procedures.settings.Combine_Textile_Sizing_Software_Status = 1) Then

                    If Val(TexLed_ID) = 0 Then
                        Throw New ApplicationException("Invalid Textile Sizing Name" & Chr(13) & "Select ``Textile_Sizing_Name``  in  Company_Creation  for  " & lbl_Company.Text)
                        Exit Sub
                    End If

                    TexVnd_ID = 0
                    If Common_Procedures.settings.AUTOPOSTING_OF_SIZINGSOFTWARE_PAVUDELIVERY_AS_TEXTILESOFWTARE_PAVURECEIPT = 1 Then
                        TexVnd_ID = VndrNm_Id
                    ElseIf Common_Procedures.settings.Combine_Textile_Sizing_Software_Status = 1 Then
                        TexVnd_ID = Common_Procedures.get_FieldValue(con, "Vendor_head", "Textile_To_WeaverIdNo", "(Vendor_idno = " & Val(VndrNm_Id) & ")", , tr)

                        If Val(TexVnd_ID) = 0 Then
                            vNewFrmTYpe = "VENDOR"
                            Throw New ApplicationException("Invalid Textile Weaver Name" & Chr(13) & "Select ``Textile_Weaver_Name``  in  Vendor_Creation  for  " & cbo_VendorName.Text)
                            Exit Sub
                        End If

                    End If

                Dim vFirst_EdsCntID As String
                Dim vFirst_StNo As String

                    vFirst_StNo = ""
                    vFirst_EdsCntID = 0

                    If dgv_Details.Rows.Count > 0 Then

                        vFirst_StNo = Trim(dgv_Details.Rows(0).Cells(1).Value)

                        Cnt_ID = Common_Procedures.Count_NameToIdNo(con, dgv_Details.Rows(0).Cells(8).Value, tr)
                        vFirst_EdsCntID = Common_Procedures.get_FieldValue(con, "EndsCount_Head", "EndsCount_IdNo", "(Ends_Name = " & Str(Val(dgv_Details.Rows(0).Cells(7).Value)) & " and Count_IdNo = " & Str(Val(Cnt_ID)) & ")", , tr)

                    End If


                    cmd.CommandText = "Insert into " & Trim(vDbName) & "SizSoft_Pavu_Delivery_Head (                     User_IdNo          ,    Pavu_Delivery_Code  ,        Company_IdNo         ,    Pavu_Delivery_No          ,             for_OrderBy   , Pavu_Delivery_Date,               Book_No          ,         Sizing_IdNo        ,          Ledger_IdNo       ,               Count_Name          ,                 Ends_Name      , Transport_IdNo  ,           Vehicle_No  ,               Remarks           ,              Total_Beam  ,         Total_Pcs        ,           Total_Meters    ,        Electronic_Reference_No          ,          Date_And_Time_Of_Supply            ,   DeliveryTO_Idno  ,   Approx_Value             ,        Entry_Time_Text       ,   Vendor_Idno         ,        Rate               ,               Delivered_By        ,          Invoice_Selection_Type     ,            Rate_For             ,   Sizing_Tot_Weight  ,             First_SetNo    ,       First_EndsCount_IdNo   )  " &
                                      "                                 Values                     (" & Str(Common_Procedures.User.IdNo) & ", '" & Trim(NewCode) & "', " & Str(Val(TexComp_ID)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(vOrdByNo)) & ",     @DcDate       , '" & Trim(txt_BookNo.Text) & "', " & Str(Val(SizLed_ID)) & ", " & Str(Val(TexLed_ID)) & ", '" & Trim(lbl_CountName.Text) & "', " & Str(Val(lbl_Ends.Text)) & ",     0           , '" & Trim(Vec_No) & "', '" & Trim(txt_Remarks.Text) & "', " & Str(Val(vTotBms)) & ", " & Str(Val(vTotPcs)) & ", " & Str(Val(vTotMtrs)) & ", '" & Trim(txt_ElectronicRefNo.Text) & "', '" & Trim(txt_DateAndTimeOFSupply.Text) & "',           0        , " & Val(lbl_value.Text) & ", '" & Trim(dtp_Time.Text) & "', " & Val(TexVnd_ID) & ", " & Val(txt_Rate.Text) & ", '" & Trim(cbo_Delivered.Text) & "', '" & Trim(UCase(cbo_Type.Text)) & "', '" & Trim(Cbo_RateFor.Text) & "', " & Val(nTotWegt) & ", '" & Trim(vFirst_StNo) & "', " & Val(vFirst_EdsCntID) & " ) "
                    cmd.ExecuteNonQuery()


                End If

            End If

            cmd.CommandText = "Delete from Sizing_Pavu_Delivery_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Pavu_Delivery_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

        For i = 0 To dgv_Details.RowCount - 1

            Sno = 0
            vTEX_Pvu_SNO = 0

            If Val(dgv_Details.Rows(i).Cells(4).Value) <> 0 Then

                Sno = Sno + 1

                Cnt_ID = Common_Procedures.Count_NameToIdNo(con, dgv_Details.Rows(i).Cells(8).Value, tr)
                mill_det_Id = Common_Procedures.Mill_NameToIdNo(con, dgv_Details.Rows(i).Cells(11).Value, tr)

                cmd.CommandText = "Insert into Sizing_Pavu_Delivery_Details (   Pavu_Delivery_Code   ,            Company_IdNo            ,        Pavu_Delivery_No       ,             for_OrderBy        , Pavu_Delivery_Date,          Ledger_IdNo    ,            Sl_No     ,          Set_No                                   ,     Beam_No                                      ,           Noof_Pcs                              ,  Meters                                           , DeliveryTo_Name                                    ,        Set_Code                                    ,    Ends_Name                                      , Count_IdNo              ,                   Meters_Pc               ,Mill_IdNo,Sort_No     ) " &
                                      "Values                            ('" & Trim(NewCode) & "' , " & Str(Val(lbl_Company.Tag)) & "  , '" & Trim(lbl_DcNo.Text) & "' , " & Str(Val(vOrdByNo)) & "     , @DcDate            , " & Str(Val(led_id)) & ", " & Str(Val(Sno)) & ", '" & Trim(dgv_Details.Rows(i).Cells(1).Value) & "','" & Trim(dgv_Details.Rows(i).Cells(2).Value) & "', " & Val(dgv_Details.Rows(i).Cells(3).Value) & " , " & Val(dgv_Details.Rows(i).Cells(4).Value) & "   , '" & Trim(dgv_Details.Rows(i).Cells(5).Value) & "' , '" & Trim(dgv_Details.Rows(i).Cells(6).Value) & "' , '" & Trim(dgv_Details.Rows(i).Cells(7).Value) & "', " & Str(Val(Cnt_ID)) & ", " & Val(dgv_Details.Rows(i).Cells(9).Value) & "," & Val(mill_det_Id) & ",'" & Trim(dgv_Details.Rows(i).Cells(12).Value) & "')"
                cmd.ExecuteNonQuery()

                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "NEW", "Sizing_Pavu_Delivery_Details", "Pavu_Delivery_Code", Val(lbl_Company.Tag), NewCode, lbl_DcNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "Set_No, Beam_No, Noof_Pcs, Meters, DeliveryTo_Name, Ends_Name, Count_IdNo, Meters_Pc, Mill_IdNo, Sort_No", "Sl_No", "Pavu_Delivery_Code, For_OrderBy, Company_IdNo, Pavu_Delivery_No, Pavu_Delivery_Date, Ledger_IdNo, Set_Code", tr)

                If Trim(UCase(cbo_Type.Text)) <> "DIRECT" Then

                    Nr = 0
                    cmd.CommandText = "Update Stock_SizedPavu_Processing_Details set Pavu_Delivery_Code = '" & Trim(NewCode) & "', Pavu_Delivery_Increment = Pavu_Delivery_Increment + 1, DeliveryTo_Name = '" & Trim(dgv_Details.Rows(i).Cells(5).Value) & "' Where Set_Code ='" & Trim(dgv_Details.Rows(i).Cells(6).Value) & "' and Beam_No = '" & Trim(dgv_Details.Rows(i).Cells(2).Value) & "' and (Ledger_IdNo = " & Str(Val(led_id)) & " or Ledger_IdNo = " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & ")"
                    Nr = cmd.ExecuteNonQuery()

                    If Nr = 0 Then
                        MessageBox.Show("Invalid Pavu Details - Mismatch of details", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        tr.Rollback()
                        If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
                        Exit Sub
                    End If

                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1263" Then
                        Vndr_Id = Common_Procedures.Vendor_AlaisNameToIdNo(con, dgv_Details.Rows(i).Cells(5).Value, tr)
                        cmd.CommandText = "Update Stock_SizedPavu_Processing_Details Set Vendor_idno =  " & Str(Val(Vndr_Id)) & " Where Set_Code ='" & Trim(dgv_Details.Rows(i).Cells(6).Value) & "' and Beam_No = '" & Trim(dgv_Details.Rows(i).Cells(2).Value) & "' and (Ledger_IdNo = " & Str(Val(led_id)) & " or Ledger_IdNo = " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & ")"
                        cmd.ExecuteNonQuery()
                    End If

                End If

                If Common_Procedures.settings.AUTOPOSTING_OF_SIZINGSOFTWARE_PAVUDELIVERY_AS_TEXTILESOFWTARE_PAVURECEIPT = 1 Or Common_Procedures.settings.Combine_Textile_Sizing_Software_Status = 1 Then

                    If Common_Procedures.settings.AUTOPOSTING_OF_SIZINGSOFTWARE_PAVUDELIVERY_AS_TEXTILESOFWTARE_PAVURECEIPT = 1 Then
                        TexComp_ID = Val(lbl_Company.Tag)
                    ElseIf Common_Procedures.settings.Combine_Textile_Sizing_Software_Status = 1 Then
                        TexComp_ID = Common_Procedures.get_FieldValue(con, "ledger_head", "Textile_To_CompanyIdNo", "(ledger_idno = " & Str(Val(led_id)) & ")", , tr)
                    End If


                    If (TexLed_ID <> 0 And TexLed_ID = led_id And Common_Procedures.settings.AUTOPOSTING_OF_SIZINGSOFTWARE_PAVUDELIVERY_AS_TEXTILESOFWTARE_PAVURECEIPT = 1) Or (Val(TexComp_ID) <> 0 And Common_Procedures.settings.Combine_Textile_Sizing_Software_Status = 1) Then

                        If Common_Procedures.settings.AUTOPOSTING_OF_SIZINGSOFTWARE_PAVUDELIVERY_AS_TEXTILESOFWTARE_PAVURECEIPT = 1 Then
                            TexCnt_iD = Cnt_ID
                        ElseIf Common_Procedures.settings.Combine_Textile_Sizing_Software_Status = 1 Then
                            TexCnt_iD = Common_Procedures.get_FieldValue(con, "count_head", "Textile_To_CountIdNo", "(count_idno = " & Str(Val(Cnt_ID)) & ")", , tr)
                        End If

                        If Val(TexCnt_iD) = 0 Then
                            vNewFrmTYpe = "COUNT"
                            Throw New ApplicationException("Invalid Textile Count Name" & Chr(13) & "Select ``Textile_Count_Name``  in  Count_Creation  for  " & dgv_Details.Rows(i).Cells(8).Value)
                            Exit Sub
                        End If

                        If Common_Procedures.settings.AUTOPOSTING_OF_SIZINGSOFTWARE_PAVUDELIVERY_AS_TEXTILESOFWTARE_PAVURECEIPT = 1 Then
                            TexEdsCnt_ID = Common_Procedures.get_FieldValue(con, "EndsCount_Head", "EndsCount_IdNo", "(Ends_Name = " & Str(Val(dgv_Details.Rows(i).Cells(7).Value)) & " and Count_IdNo = " & Str(Val(Cnt_ID)) & ")", , tr)
                        ElseIf Common_Procedures.settings.Combine_Textile_Sizing_Software_Status = 1 Then
                            TexEdsCnt_ID = Common_Procedures.get_FieldValue(con, "EndsCount_Head", "Textile_To_EndsCountIdNo", "(Ends_Name = " & Str(Val(dgv_Details.Rows(i).Cells(7).Value)) & " and Count_IdNo = " & Str(Val(Cnt_ID)) & ")", , tr)
                        End If

                        If Val(TexEdsCnt_ID) = 0 Then
                            vEdsCnt_ID = Common_Procedures.get_FieldValue(con, "EndsCount_Head", "EndsCount_IdNo", "(Ends_Name = " & Str(Val(dgv_Details.Rows(i).Cells(7).Value)) & " and Count_IdNo = " & Str(Val(Cnt_ID)) & ")", , tr)
                            vNewFrmTYpe = "ENDSCOUNT"
                            If Val(vEdsCnt_ID) = 0 Then
                                Throw New ApplicationException("Invalid EndsCount Name" & Chr(13) & "Create New ``EndsCount_Name``  in  EndsCount_Creation  for  " & dgv_Details.Rows(i).Cells(7).Value & "/" & dgv_Details.Rows(i).Cells(8).Value)
                            Else
                                Throw New ApplicationException("Invalid Textile EndsCount Name" & Chr(13) & "Select ``Textile_EndsCount_Name``  in  EndsCount_Creation  for  " & dgv_Details.Rows(i).Cells(7).Value & "/" & dgv_Details.Rows(i).Cells(8).Value)
                            End If
                            Exit Sub
                        End If


                        vSetCd = Trim(dgv_Details.Rows(i).Cells(6).Value)
                        If Trim(vSetCd) = "" Then
                            vSetCd = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(dgv_Details.Rows(i).Cells(1).Value) & "/" & Trim(Common_Procedures.FnYearCode)
                            Selc_SetCode = Trim(dgv_Details.Rows(i).Cells(1).Value) & "/" & Trim(Common_Procedures.FnYearCode) & "/" & Trim(Val(lbl_Company.Tag)) & "/" & Trim(Pk_Condition)
                        Else
                            Selc_SetCode = Common_Procedures.get_FieldValue(con, "Stock_SizedPavu_Processing_Details", "setcode_forSelection", "(set_code = '" & Trim(vSetCd) & "')", , tr)
                        End If

                        vSetCd = Trim(Pk_Condition_Tex) & Trim(vSetCd)
                        Selc_SetCode = Trim(Selc_SetCode) & "/" & Trim(Pk_Condition_Tex)


                        cmd.CommandText = "Insert into " & Trim(vDbName) & "SizSoft_Pavu_Delivery_Details ( Pavu_Delivery_Code      ,            Company_IdNo       ,    Pavu_Delivery_No             ,         for_OrderBy        , Pavu_Delivery_Date,          Ledger_IdNo       ,            Sl_No     ,                               Set_No              ,                               Beam_No             ,                             Noof_Pcs            ,                             Meters             ,                               DeliveryTo_Name      ,                               Set_Code             ,                               Ends_Name           ,          Count_IdNo        ,                             Meters_Pc           ) " &
                                                "                                Values                       ( '" & Trim(NewCode) & "' , " & Str(Val(TexComp_ID)) & "  , '" & Trim(lbl_DcNo.Text) & "'   , " & Str(Val(vOrdByNo)) & " ,  @DcDate          , " & Str(Val(TexLed_ID)) & ", " & Str(Val(Sno)) & ", '" & Trim(dgv_Details.Rows(i).Cells(1).Value) & "', '" & Trim(dgv_Details.Rows(i).Cells(2).Value) & "', " & Val(dgv_Details.Rows(i).Cells(3).Value) & " , " & Val(dgv_Details.Rows(i).Cells(4).Value) & ", '" & Trim(dgv_Details.Rows(i).Cells(5).Value) & "' , '" & Trim(dgv_Details.Rows(i).Cells(6).Value) & "' , '" & Trim(dgv_Details.Rows(i).Cells(7).Value) & "', " & Str(Val(TexCnt_iD)) & ", " & Val(dgv_Details.Rows(i).Cells(9).Value) & " ) "
                        cmd.ExecuteNonQuery()

                        Nr = 0
                        cmd.CommandText = "Update " & Trim(vDbName) & "Stock_SizedPavu_Processing_Details set SoftwareType_IdNo = " & Str(Val(Common_Procedures.SoftwareTypes.Textile_Software)) & " ,  Reference_Date = @DcDate, Sl_No = " & Str(Val(Sno)) & ", Vendor_IdNo = " & Str(Val(TexVnd_ID)) & " " &
                                                " Where Reference_Code = '" & Trim(Pk_Condition_Tex) & Trim(NewCode) & "' and Selection_From_ReferenceCode = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Beam_No = '" & Trim(dgv_Details.Rows(i).Cells(2).Value) & "'"
                        Nr = cmd.ExecuteNonQuery()

                        Mtr_Pc = 0
                        If Val(dgv_Details.Rows(i).Cells(3).Value) <> 0 Then
                            Mtr_Pc = Format(Val(dgv_Details.Rows(i).Cells(4).Value) / Val(dgv_Details.Rows(i).Cells(3).Value), "")
                        End If

                        If Nr = 0 Then

                            cmd.CommandText = "Insert into " & Trim(vDbName) & "Stock_SizedPavu_Processing_Details (                     SoftwareType_IdNo                             ,                      Reference_Code              ,              Selection_From_ReferenceCode   ,              Company_IdNo   ,               Reference_No   ,         for_OrderBy       , Reference_Date,         Ledger_IdNo        ,                             StockAt_IdNo                  ,         Set_Code      ,                               Set_No              ,    setcode_forSelection     ,                               Ends_Name           ,     count_idno             ,         EndsCount_IdNo       , Mill_IdNo, Beam_Width_Idno, Sizing_SlNo,         Sl_No        ,                                  Beam_No             ,                                  ForOrderBy_BeamNo                                       , Gross_Weight, Tare_Weight, Net_Weight,                              Noof_Pcs               ,          Meters_Pc      ,                                 Meters               ,           Vendor_IdNo       ) " &
                                                            "    Values                                                (" & Str(Val(Common_Procedures.SoftwareTypes.Textile_Software)) & " ,  '" & Trim(Pk_Condition_Tex) & Trim(NewCode) & "', '" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(TexComp_ID)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(vOrdByNo)) & ",   @DcDate     , " & Str(Val(TexLed_ID)) & ", " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & ", '" & Trim(vSetCd) & "', '" & Trim(dgv_Details.Rows(i).Cells(1).Value) & "', '" & Trim(Selc_SetCode) & "', '" & Trim(dgv_Details.Rows(i).Cells(7).Value) & "', " & Str(Val(TexCnt_iD)) & ", " & Str(Val(TexEdsCnt_ID)) & ",     0    ,      0         ,      0     , " & Str(Val(Sno)) & ", '" & Trim(dgv_Details.Rows(i).Cells(2).Value) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(dgv_Details.Rows(i).Cells(2).Value))) & ",      0      ,       0    ,      0    , " & Str(Val(dgv_Details.Rows(i).Cells(3).Value)) & ", " & Str(Val(Mtr_Pc)) & ", " & Str(Val(dgv_Details.Rows(i).Cells(4).Value)) & " , " & Str(Val(TexVnd_ID)) & " ) "
                            cmd.ExecuteNonQuery()

                        End If

                        Nr = 0
                        cmd.CommandText = "Update " & Trim(vDbName) & "Stock_Pavu_Processing_Details set Reference_Date = @DcDate, Sized_Beam = Sized_Beam + 1, Meters = Meters  + " & Str(Val(dgv_Details.Rows(i).Cells(4).Value)) & " " &
                                                " Where Reference_Code = '" & Trim(Pk_Condition_Tex) & Trim(NewCode) & "' and EndsCount_IdNo = " & Str(Val(TexEdsCnt_ID))
                        Nr = cmd.ExecuteNonQuery()

                        If Nr = 0 Then
                            vTEX_Pvu_SNO = vTEX_Pvu_SNO + 1
                            cmd.CommandText = "Insert into " & Trim(vDbName) & "Stock_Pavu_Processing_Details (                   Reference_Code                ,                 Company_IdNo     ,             Reference_No     ,              for_OrderBy  , Reference_Date,                                       DeliveryTo_Idno     ,     ReceivedFrom_Idno      ,        Entry_ID      ,      Party_Bill_No   ,        Particulars     ,               Sl_No            ,         EndsCount_IdNo        , Sized_Beam  ,                                 Meters               ,                      DeliveryToIdno_ForParticulars          ,   ReceivedFromIdno_ForParticulars ) " &
                                                    "           Values                                            ( '" & Trim(Pk_Condition_Tex) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(vOrdByNo)) & ", @DcDate       , " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & ", " & Str(Val(SizLed_ID)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', " & Str(Val(vTEX_Pvu_SNO)) & " , " & Str(Val(TexEdsCnt_ID)) & ",      1      , " & Str(Val(dgv_Details.Rows(i).Cells(4).Value)) & " ,   " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & ",       " & Str(Val(SizLed_ID)) & " )"
                            cmd.ExecuteNonQuery()
                        End If

                    End If

                End If

            End If

        Next



        If Common_Procedures.settings.Textile_Sizing_Delivery_receipt_Selection = 1 Then

            vDELVLED_COMPIDNO = Common_Procedures.Ledger_IdNoToCompanyIdNo(con, Str(Val(led_id)), tr)

            If vDELVLED_COMPIDNO <> 0 Then

                vCOMP_LEDIDNO = Common_Procedures.Company_IdnoToSizingLedgerIdNo(con, Str(Val(lbl_Company.Tag)), tr)

                'vSELC_RCVDIDNO = 0
                'vREC_Ledtype = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_Type", "(Ledger_IdNo = " & Str(Val(led_id)) & ")", , tr)
                'If Trim(UCase(vREC_Ledtype)) = "GODOWN" Or Trim(UCase(vREC_Ledtype)) = "WEAVER" Then
                '    vSELC_RCVDIDNO = led_id
                'Else
                vSELC_RCVDIDNO = vCOMP_LEDIDNO
                'End If

                vDELVAT_IDNO = 0
                If cbo_VendorName.Visible = True Then
                    If Trim(cbo_VendorName.Text) <> "" Then
                        vDELVAT_IDNO = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_VendorName.Text, tr)
                    End If
                End If

                cmd.CommandText = "Insert into Pavu_Delivery_Selections_Processing_Details (                 Reference_Code              ,                 Company_IdNo     ,         Reference_No         ,           for_OrderBy     , Reference_Date ,                  Delivery_Code              ,          Delivery_No         ,        DeliveryTo_Idno  ,        ReceivedFrom_Idno       ,      Party_Dc_No             , Beam_Width_IdNo,        Total_Beams       ,            Total_Pcs       ,         Total_Meters      ,          Selection_CompanyIdno      ,      Selection_Ledgeridno      ,     Selection_ReceivedFromIdNo  ) " &
                                    "           Values                                     ( '" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(vOrdByNo)) & ",    @DcDate     , '" & Trim(Pk_Condition) & Trim(NewCode) & "', '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(led_id)) & ", " & Str(Val(vCOMP_LEDIDNO)) & ", '" & Trim(lbl_DcNo.Text) & "',        0       , " & Str(Val(vTotBms)) & ",   " & Str(Val(vTotPcs)) & ", " & Str(Val(vTotMtrs)) & ", " & Str(Val(vDELVLED_COMPIDNO)) & " , " & Str(Val(vCOMP_LEDIDNO)) & ", " & Str(Val(vSELC_RCVDIDNO)) & ") "
                cmd.ExecuteNonQuery()

                'cmd.CommandText = "Insert into Yarn_Delivery_Selections_Processing_Details (                  Reference_Code            ,                 Company_IdNo     ,            Reference_No      ,         for_OrderBy       , Reference_Date    ,                  Delivery_Code             ,           Delivery_No        ,       DeliveryTo_Idno    ,     ReceivedFrom_Idno   ,         DeliveryAt_Idno       ,               Party_Dc_No          ,              Total_Bags      ,          total_cones          ,              Total_Weight      ,          Selection_CompanyIdno      ,         Selection_Ledgeridno  ,      Selection_ReceivedFromIdNo  ) " &
                '                        "           Values                                     ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(vOrdByNo)) & ", @EntryDate        ,'" & Trim(Pk_Condition) & Trim(NewCode) & "', '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(Delv_ID)) & ", " & Str(Val(Rec_ID)) & ", " & Str(Val(vDELVAT_IDNO)) & ", '" & Trim(txt_Party_DcNo.Text) & "', " & Str(Val(vTotYrnBags)) & ", " & Str(Val(vTotYrnCones)) & ", " & Str(Val(vTotYrnWeight)) & ", " & Str(Val(vDELVLED_COMPIDNO)) & ", " & Str(Val(vCOMP_LEDIDNO)) & ", " & Str(Val(vSELC_RCVDIDNO)) & " ) "
                'cmd.ExecuteNonQuery()

            End If

        End If



        'vCOMP_LEDIDNO = 0
        'vDELVLED_COMPIDNO = 0

        'If Common_Procedures.settings.Textile_Sizing_Delivery_receipt_Selection = 1 Then

        '    vCOMP_LEDIDNO = Common_Procedures.get_FieldValue(con, "Company_Head", "Sizing_To_LedgerIdNo", "(Company_idno = " & Str(Val(lbl_Company.Tag)) & ")")
        '    vDELVLED_COMPIDNO = Val(Common_Procedures.get_FieldValue(con, "Company_Head", "Company_idno", "(Sizing_To_LedgerIdNo = " & Str(Val(Del_ID)) & ")"))

        'End If

        'If Common_Procedures.settings.Textile_Sizing_Delivery_receipt_Selection = 1 And vDELVLED_COMPIDNO <> 0 Then
        '    cmd.CommandText = "Insert into Pavu_Delivery_Selections_Processing_Details (                 Reference_Code              ,                 Company_IdNo     ,         Reference_No         ,                               for_OrderBy                             , Reference_Date ,    Delivery_Code                            ,     Delivery_No              ,        DeliveryTo_Idno  ,     ReceivedFrom_Idno   , EndsCount_Idno,     Party_Dc_No              , Beam_Width_IdNo,        Total_Beams       ,            Total_Pcs       ,         Total_Meters      ,                Set_No         , Set_Code,    Selection_Ledgeridno       ,          Selection_CompanyIdno      ) " &
        '                        "           Values                                     ( '" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text))) & ",    @DcDate     , '" & Trim(Pk_Condition) & Trim(NewCode) & "', '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(led_id)) & ", " & Str(Val(led_id)) & ",    0          , '" & Trim(lbl_DcNo.Text) & "',        0       , " & Str(Val(vTotBms)) & ",   " & Str(Val(vTotPcs)) & ", " & Str(Val(vTotMtrs)) & ", '" & Trim(txt_SetNo.Text) & "',    ''   ," & Str(Val(vCOMP_LEDIDNO)) & ", " & Str(Val(vDELVLED_COMPIDNO)) & " ) "
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

        Dt1.Dispose()

        Da.Dispose()


        If SaveAll_STS <> True Then
            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
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


        'Catch ex As Exception
        '    tr.Rollback()
        '    Timer1.Enabled = False
        '    SaveAll_STS = False

        '    MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)


        'Finally
        '    If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

        '    If Trim(UCase(vNewFrmTYpe)) = "COUNT" Then
        '        Dim f1 As New Count_Creation '(Cnt_ID)

        '        Common_Procedures.Master_Return.Form_Name = ""
        '        Common_Procedures.Master_Return.Control_Name = ""
        '        Common_Procedures.Master_Return.Return_Value = ""
        '        Common_Procedures.Master_Return.Master_Type = ""

        '        f1.MdiParent = MDIParent1
        '        f1.Show()

        '    ElseIf Trim(UCase(vNewFrmTYpe)) = "ENDSCOUNT" Then
        '        Dim f2 As New EndsCount_Creation '((vEdsCnt_ID))

        '        Common_Procedures.Master_Return.Form_Name = ""
        '        Common_Procedures.Master_Return.Control_Name = ""
        '        Common_Procedures.Master_Return.Return_Value = ""
        '        Common_Procedures.Master_Return.Master_Type = ""

        '        f2.MdiParent = MDIParent1
        '        f2.Show()

        '    ElseIf Trim(UCase(vNewFrmTYpe)) = "VENDOR" Then
        '        Dim f2 As New Vendor_Creation '(VndrNm_Id)

        '        Common_Procedures.Master_Return.Form_Name = ""
        '        Common_Procedures.Master_Return.Control_Name = ""
        '        Common_Procedures.Master_Return.Return_Value = ""
        '        Common_Procedures.Master_Return.Master_Type = ""

        '        f2.MdiParent = MDIParent1
        '        f2.Show()

        '    End If

        'End Try

    End Sub

    Private Sub cbo_Ledger_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.GotFocus


        If Common_Procedures.settings.Textile_Sizing_Delivery_receipt_Selection = 1 Then
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "Ledger_Type = 'GODOWN' or (AccountsGroup_IdNo = 10 and Close_Status = 0 )", "(Ledger_IdNo = 0)")
        Else
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 and Close_Status = 0 )", "(Ledger_IdNo = 0)")
        End If


    End Sub

    Private Sub cbo_Ledger_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        If Common_Procedures.settings.Textile_Sizing_Delivery_receipt_Selection = 1 Then
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, dtp_Date, cbo_Type, "Ledger_AlaisHead", "Ledger_DisplayName", "Ledger_Type = 'GODOWN' or (AccountsGroup_IdNo = 10 and Close_Status = 0 )", "(Ledger_IdNo = 0)")
        Else
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, dtp_Date, cbo_Type, "Ledger_AlaisHead", "Ledger_DisplayName", " (AccountsGroup_IdNo = 10 and Close_Status = 0 )", "(Ledger_IdNo = 0)")
        End If

    End Sub

    Private Sub cbo_Ledger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ledger.KeyPress
        If Common_Procedures.settings.Textile_Sizing_Delivery_receipt_Selection = 1 Then
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Ledger, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "Ledger_Type = 'GODOWN' or (AccountsGroup_IdNo = 10 and Close_Status = 0 )", "(Ledger_IdNo = 0)")

        Else
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Ledger, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 and Close_Status = 0 )", "(Ledger_IdNo = 0)")

        End If

        If Asc(e.KeyChar) = 13 Then
            If Trim(UCase(cbo_Type.Text)) <> "DIRECT" Then
                If MessageBox.Show("Do you want to select Pavu :", "FOR PAVU SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                    btn_Selection_Click(sender, e)

                Else
                    txt_BookNo.Focus()

                End If

            Else
                txt_BookNo.Focus()

            End If

        End If

    End Sub

    Private Sub cbo_VehicleNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_VehicleNo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_VehicleNo, txt_Remarks, "Sizing_Pavu_Delivery_Head", "Vehicle_No", "", "", False)
        'If Asc(e.KeyChar) = 13 Then
        '    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Then
        '        cbo_VendorName.Focus()
        '    Else
        '        Cbo_RateFor.Focus()
        '    End If
        'End If
    End Sub

    Private Sub cbo_VehicleNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_VehicleNo.KeyDown

        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_VehicleNo, cbo_DeliveryTo, txt_Remarks, "Sizing_Pavu_Delivery_Head", "Vehicle_No", "", "")
        If (e.KeyValue = 40 And cbo_VehicleNo.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Or Trim(Common_Procedures.settings.CustomerCode) = "1282" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1288" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1263" Then
                txt_Remarks.Focus()
            End If
        End If

        If (e.KeyValue = 38 And cbo_VehicleNo.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Or Trim(Common_Procedures.settings.CustomerCode) = "1282" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1288" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1263" Then
                cbo_VendorName.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_Transport_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Transport.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Transport_Head", "Transport_Name", "", "(Transport_IdNo = 0)")
    End Sub

    Private Sub cbo_Transport_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Transport.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Transport, txt_ElectronicRefNo, cbo_Delivered, "Transport_Head", "Transport_Name", "", "(Transport_IdNo = 0)")
        If (e.KeyValue = 38) Then

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1036" Then '---- Kalaimagal Sizing (Avinashi)
                txt_BookNo.Focus()

            Else
                If dgv_Details.RowCount > 0 Then
                    dgv_Details.Focus()
                    If Trim(cbo_Type.Text) <> "DIRECT" Then
                        dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.RowCount - 1).Cells(5)
                        dgv_Details.CurrentCell.Selected = True
                    Else
                        dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.RowCount - 1).Cells(9)
                        dgv_Details.CurrentCell.Selected = True
                    End If


                Else
                    txt_BookNo.Focus()

                End If

            End If

        End If
    End Sub


    Private Sub cbo_Transport_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Transport.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Transport, cbo_Delivered, "Transport_Head", "Transport_Name", "", "(Transport_IdNo = 0)")
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
                Condt = "a.Pavu_Delivery_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Pavu_Delivery_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Pavu_Delivery_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If

            If Trim(cbo_Filter_CountName.Text) <> "" Then
                Cnt_IdNo = Common_Procedures.Count_NameToIdNo(con, cbo_Filter_CountName.Text)
            End If

            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.Ledger_IdNo = " & Str(Val(Led_IdNo))
            End If

            If Val(Cnt_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Pavu_Delivery_Code IN (select z1.Pavu_Delivery_Code from Stock_SizedPavu_Processing_Details z1 where z1.Count_IdNo = " & Str(Val(Cnt_IdNo)) & ")"
            End If

            If Trim(cbo_Filter_EndsName.Text) <> "" Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Pavu_Delivery_Code IN (select z2.Pavu_Delivery_Code from Stock_SizedPavu_Processing_Details z2 where z2.Ends_Name = '" & Trim(cbo_Filter_EndsName.Text) & "')"
            End If

            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name from Sizing_Pavu_Delivery_Head a INNER JOIN Ledger_Head b on a.Ledger_IdNo = b.Ledger_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Pavu_Delivery_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Pavu_Delivery_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Pavu_Delivery_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Pavu_Delivery_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = Val(dt2.Rows(i).Item("Total_Beam").ToString)
                    dgv_Filter_Details.Rows(n).Cells(5).Value = Val(dt2.Rows(i).Item("Total_Pcs").ToString)
                    dgv_Filter_Details.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Total_Meters").ToString), "########0.00")

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

    Private Sub cbo_Filter_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 )", "(Ledger_IdNo = 0)")
    End Sub


    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, dtp_Filter_ToDate, cbo_Filter_CountName, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 )", "(Ledger_IdNo = 0)")
        'Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, txt_RecNo, txt_BillNo, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_idno = 0)")
    End Sub



    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, cbo_Filter_CountName, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 )", "(Ledger_IdNo = 0)")
        'Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Ledger, txt_BillNo, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_CountName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_CountName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_CountName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_CountName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_CountName, cbo_Filter_PartyName, cbo_Filter_EndsName, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_CountName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_CountName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_CountName, cbo_Filter_EndsName, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_EndsName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_EndsName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Stock_SizedPavu_Processing_Details", "Ends_Name", "", "")
    End Sub


    Private Sub cbo_Filter_MillName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_EndsName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_EndsName, cbo_Filter_CountName, btn_Filter_Show, "Stock_SizedPavu_Processing_Details", "Ends_Name", "", "")

    End Sub

    Private Sub cbo_Filter_MillName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_EndsName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_EndsName, Nothing, "Stock_SizedPavu_Processing_Details", "Ends_Name", "", "")
        If Asc(e.KeyChar) = 13 Then
            btn_Filter_Show_Click(sender, e)
        End If
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

    Private Sub dgv_Details_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEndEdit
        'With dgv_Details
        '    If .CurrentCell.RowIndex = .RowCount - 1 Then
        '        cbo_Transport.Focus()
        '    End If 
        'End With
    End Sub

    Private Sub dgv_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim Dt4 As New DataTable
        Dim Rect As Rectangle

        With dgv_Details

            ' dgv_ActCtrlName = .Name.ToString

            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1282" Then '---- BRT SIZING
                If e.RowIndex > 0 And e.ColumnIndex = 1 Then
                    If Val(.CurrentRow.Cells(1).Value) = 0 And e.RowIndex = .RowCount - 1 Then
                        .CurrentRow.Cells(1).Value = .Rows(e.RowIndex - 1).Cells(1).Value
                        .CurrentRow.Cells(7).Value = .Rows(e.RowIndex - 1).Cells(7).Value
                        .CurrentRow.Cells(8).Value = .Rows(e.RowIndex - 1).Cells(8).Value
                    End If
                End If
            End If

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1263" Then '---- Sri Meenakshi Sizing (Somanur)

                If e.ColumnIndex = 5 Then

                    If cbo_Grid_Vendor.Visible = False Or Val(cbo_Grid_Vendor.Tag) <> e.RowIndex Then

                        'dgv_ActCtrlName = dgv_Details.Name

                        cbo_Grid_Vendor.Tag = -1
                        Da = New SqlClient.SqlDataAdapter("select Vendor_Name from Vendor_Head Order by Vendor_Name", con)
                        Dt1 = New DataTable
                        Da.Fill(Dt1)
                        cbo_Grid_Vendor.DataSource = Dt1
                        cbo_Grid_Vendor.DisplayMember = "Vendor_Name"

                        Rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                        cbo_Grid_Vendor.Left = .Left + Rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                        cbo_Grid_Vendor.Top = .Top + Rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                        cbo_Grid_Vendor.Width = Rect.Width  ' .CurrentCell.Size.Width
                        cbo_Grid_Vendor.Height = Rect.Height  ' rect.Height

                        cbo_Grid_Vendor.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                        cbo_Grid_Vendor.Tag = Val(e.RowIndex)
                        cbo_Grid_Vendor.Visible = True

                        cbo_Grid_Vendor.BringToFront()
                        cbo_Grid_Vendor.Focus()
                    End If
                Else

                    cbo_Grid_Vendor.Visible = False

                End If
            End If


            If e.ColumnIndex = 8 And Trim(cbo_Type.Text) = "DIRECT" Then

                If cbo_Grid_Count.Visible = False Or Val(cbo_Grid_Count.Tag) <> e.RowIndex Then

                    'dgv_ActCtrlName = dgv_Details.Name

                    cbo_Grid_Count.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Count_Name from Count_Head Order by Count_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Grid_Count.DataSource = Dt1
                    cbo_Grid_Count.DisplayMember = "Count_Name"

                    Rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_Count.Left = .Left + Rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_Grid_Count.Top = .Top + Rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                    cbo_Grid_Count.Width = Rect.Width  ' .CurrentCell.Size.Width
                    cbo_Grid_Count.Height = Rect.Height  ' rect.Height

                    cbo_Grid_Count.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_Grid_Count.Tag = Val(e.RowIndex)
                    cbo_Grid_Count.Visible = True

                    cbo_Grid_Count.BringToFront()
                    cbo_Grid_Count.Focus()
                Else

                    cbo_Grid_Count.Visible = False
                End If
            End If

        End With
    End Sub

    Private Sub cbo_Vendor_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Vendor.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Vendor_AlaisHead", "Vendor_DisplayName", "(AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14)", "(Vendor_IdNo = 0)")
    End Sub

    Private Sub cbo_Vendor_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Vendor.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_Vendor, Nothing, Nothing, "Vendor_AlaisHead", "Vendor_DisplayName", "(AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Vendor_IdNo = 0)")
        With dgv_Details

            If (e.KeyValue = 38 And cbo_Grid_Vendor.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_Grid_Vendor.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(7)

            End If

        End With
    End Sub

    Private Sub cbo_Vendor_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_Vendor.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_Vendor, Nothing, "Vendor_AlaisHead", "Vendor_DisplayName", "(AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Vendor_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then

            With dgv_Details
                .Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(7)

            End With

        End If

    End Sub

    Private Sub cbo_Vendor_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Vendor.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Vendor_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_Vendor.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If

    End Sub

    Private Sub cbo_Vendor_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Vendor.TextChanged
        Try
            If cbo_Grid_Vendor.Visible Then
                If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
                With dgv_Details
                    If Val(cbo_Grid_Vendor.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 5 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_Vendor.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub dgv_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellLeave
        With dgv_Details
            If .CurrentCell.ColumnIndex = 1 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0")
                End If
            End If
        End With
    End Sub

    Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged
        Try
            If IsNothing(dgv_Details.CurrentCell) Then Exit Sub

            If IsNothing(dgv_Details.CurrentCell) Then Exit Sub

            With dgv_Details
                If .Visible Then
                    If .CurrentCell.ColumnIndex = 4 Then
                        Total_Calculation()
                    End If
                End If
            End With

        Catch ex As Exception
            '-----
        End Try

    End Sub

    Private Sub dgv_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Details.EditingControlShowing
        dgtxt_Details = CType(dgv_Details.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.Enter
        Try
            dgv_Details.EditingControl.BackColor = Color.Lime
            dgv_Details.EditingControl.ForeColor = Color.Blue
            dgtxt_Details.SelectAll()
        Catch ex As Exception
            '--
        End Try

    End Sub

    Private Sub dgv_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyUp

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_Details

                If Val(.Rows(.CurrentRow.Index).Cells(13).Value) = 0 Then

                    If .CurrentRow.Index = .RowCount - 1 Then
                        For i = 1 To .Columns.Count - 1
                            .Rows(.CurrentRow.Index).Cells(i).Value = ""
                        Next

                    Else
                        .Rows.RemoveAt(.CurrentRow.Index)

                    End If

                    Total_Calculation()

                End If

            End With

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

    Private Sub txt_ElectronicRefNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_ElectronicRefNo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1036" Then '---- Kalaimagal Sizing (Avinashi)
                cbo_Transport.Focus()
            Else
                If cbo_Type.Text <> "DIRECT" Then
                    If dgv_Details.Rows.Count > 0 Then
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(5)
                        dgv_Details.CurrentCell.Selected = True
                    Else
                        cbo_Transport.Focus()
                    End If
                Else

                    If dgv_Details.Rows.Count > 0 Then
                        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1282" Then
                            cbo_MillName.Focus()
                        Else
                            dgv_Details.Focus()
                            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
                            dgv_Details.CurrentCell.Selected = True
                        End If
                    Else
                        cbo_Transport.Focus()
                    End If

                End If
            End If

        End If

        ' End If
    End Sub

    'Private Sub txt_Remarks_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Remarks.KeyDown
    '    'If e.KeyCode = 40 Then btn_save.Focus() ' SendKeys.Send("{TAB}")
    '    'If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    'End Sub

    'Private Sub txt_Remarks_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Remarks.KeyPress
    '    If Asc(e.KeyChar) = 13 Then
    '        Cbo_RateFor.Focus()
    '        'If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
    '        '    save_record()
    '        'Else
    '        '    dtp_Date.Focus()
    '        'End If
    '    End If
    'End Sub

    Private Sub Total_Calculation()
        Dim Sno As Integer
        Dim TotBms As Single, TotPcs As Single, TotMtrs As Single, nTotWght As Single

        Sno = 0
        TotBms = 0
        TotPcs = 0
        TotMtrs = 0
        nTotWght = 0

        With dgv_Details
            For i = 0 To .Rows.Count - 1
                Sno = Sno + 1
                .Rows(i).Cells(0).Value = Val(Sno)
                If Val(.Rows(i).Cells(4).Value) <> 0 Then
                    TotBms = TotBms + 1
                    TotPcs = TotPcs + Val(.Rows(i).Cells(3).Value)
                    TotMtrs = TotMtrs + Val(.Rows(i).Cells(4).Value)
                    nTotWght = nTotWght + Val(.Rows(i).Cells(10).Value)
                End If
            Next
        End With

        With dgv_Details_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(2).Value = Val(TotBms)
            .Rows(0).Cells(3).Value = Val(TotPcs)
            .Rows(0).Cells(4).Value = Format(Val(TotMtrs), "########0.00")
            .Rows(0).Cells(9).Value = Format(Val(nTotWght), "########0.000")
        End With

        Amount_Calultation()

    End Sub

    Private Sub btn_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Close_Form()
    End Sub

    Private Sub btn_Close_Selection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Selection.Click
        Dim n As Integer
        Dim sno As Integer
        Dim EdsNm As String, DupEdsNm As String
        Dim CntNm As String, DupCntNm As String

        dgv_Details.Rows.Clear()

        EdsNm = "" : DupEdsNm = ""
        CntNm = "" : DupCntNm = ""

        pnl_Back.Enabled = True

        For i = 0 To dgv_Selection.RowCount - 1

            If Val(dgv_Selection.Rows(i).Cells(5).Value) = 1 Then

                n = dgv_Details.Rows.Add()

                sno = sno + 1
                dgv_Details.Rows(n).Cells(0).Value = Val(sno)
                dgv_Details.Rows(n).Cells(1).Value = dgv_Selection.Rows(i).Cells(1).Value
                dgv_Details.Rows(n).Cells(2).Value = dgv_Selection.Rows(i).Cells(2).Value
                dgv_Details.Rows(n).Cells(3).Value = dgv_Selection.Rows(i).Cells(3).Value
                dgv_Details.Rows(n).Cells(4).Value = Format(Val(dgv_Selection.Rows(i).Cells(4).Value), "#########0.00")
                dgv_Details.Rows(n).Cells(5).Value = dgv_Selection.Rows(i).Cells(7).Value
                dgv_Details.Rows(n).Cells(6).Value = dgv_Selection.Rows(i).Cells(6).Value
                dgv_Details.Rows(n).Cells(7).Value = dgv_Selection.Rows(i).Cells(8).Value
                dgv_Details.Rows(n).Cells(8).Value = dgv_Selection.Rows(i).Cells(9).Value
                dgv_Details.Rows(n).Cells(9).Value = dgv_Selection.Rows(i).Cells(10).Value
                dgv_Details.Rows(n).Cells(10).Value = dgv_Selection.Rows(i).Cells(11).Value

                dgv_Details.Rows(n).Cells(11).Value = dgv_Selection.Rows(i).Cells(12).Value
                dgv_Details.Rows(n).Cells(12).Value = dgv_Selection.Rows(i).Cells(13).Value

                If InStr(1, Trim(LCase(DupEdsNm)), "~" & Trim(LCase(dgv_Selection.Rows(i).Cells(8).Value)) & "~") = 0 Then
                    EdsNm = Trim(EdsNm) & IIf(Trim(EdsNm) <> "", ", ", "") & Trim(dgv_Selection.Rows(i).Cells(8).Value)
                    DupEdsNm = Trim(DupEdsNm) & "~" & Trim(dgv_Selection.Rows(i).Cells(8).Value) & "~"
                End If

                If InStr(1, Trim(LCase(DupCntNm)), "~" & Trim(LCase(dgv_Selection.Rows(i).Cells(9).Value)) & "~") = 0 Then
                    CntNm = Trim(CntNm) & IIf(Trim(CntNm) <> "", ", ", "") & Trim(dgv_Selection.Rows(i).Cells(9).Value)
                    DupCntNm = Trim(DupCntNm) & "~" & Trim(dgv_Selection.Rows(i).Cells(9).Value) & "~"
                End If

            End If

        Next

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1102" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1144" Then '---- Ganesh karthik Sizing (Somanur)
            If dgv_Details.Rows.Count > 18 Then
                MessageBox.Show("Does Not Select More than 18 BeamNo", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If txt_BookNo.Enabled And txt_BookNo.Visible Then txt_BookNo.Focus()
                Exit Sub
            End If
        End If
        lbl_Ends.Text = Trim(EdsNm)
        lbl_CountName.Text = Trim(CntNm)

        Total_Calculation()

        pnl_Back.Enabled = True
        pnl_Selection.Visible = False
        If txt_BookNo.Enabled And txt_BookNo.Visible Then txt_BookNo.Focus()

    End Sub


    Private Sub btn_Selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Selection.Click
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim i As Integer, j As Integer, n As Integer, SNo As Integer
        Dim LedNo As Integer
        Dim NewCode As String
        Dim RptCondt As String = ""
        Dim CompIDCondt As String

        If Trim(UCase(cbo_Type.Text)) = "DIRECT" Then Exit Sub


        LedNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)

        If LedNo = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SELECT PAVU...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
            Exit Sub
        End If

        If txt_SetNo.Visible = True And Trim(txt_SetNo.Text) <> "" Then
            RptCondt = Trim(RptCondt) & IIf(Trim(RptCondt) <> "", " and ", "") & " a.Set_No = '" & Trim(txt_SetNo.Text) & " '"
        End If

        CompIDCondt = "(a.company_idno = " & Str(Val(lbl_Company.Tag)) & ")"
        If Trim(UCase(Common_Procedures.settings.CompanyName)) = "-----~~~" Then
            CompIDCondt = ""
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        With dgv_Selection

            .Rows.Clear()
            chk_SelectAll.Checked = False

            SNo = 0

            Dim vKuraiPvu_STkID As Integer = 0

            vKuraiPvu_STkID = LedNo

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1263" Then
                'If LedNo = 101 Then
                vKuraiPvu_STkID = Common_Procedures.CommonLedger.Godown_Ac
                'End If
            End If

            'da2 = New SqlClient.SqlDataAdapter("select a.* , b.Count_Name, stt.sort_no, tTEXSPP.Pavu_Delivery_Increment as Tex_Pavu_Delivery_Increment,  tTEXSPP.Beam_Knotting_Code as Tex_Beam_Knotting_Code , tTEXSPP.Production_Meters as Tex_Production_Meters, tTEXSPP.Close_Status as Tex_Close_Status  from Stock_SizedPavu_Processing_Details a LEFT OUTER JOIN Specification_Head stt ON a.Set_Code = stt.set_code  LEFT OUTER JOIN Count_Head b ON a.Count_IdNo = b.Count_IdNo LEFT OUTER JOIN Stock_SizedPavu_Processing_Details tTEXSPP ON tTEXSPP.Reference_Code LIKE '" & Trim(Pk_Condition_Tex) & "%' and tTEXSPP.Selection_From_ReferenceCode = '" & Trim(Pk_Condition) & "'  + a.Pavu_Delivery_Code and tTEXSPP.beam_no = a.beam_no Where a.Pavu_Delivery_Code = '" & Trim(NewCode) & "' Order by a.For_OrderBy, a.Set_No, a.ForOrderBy_BeamNo, a.Beam_No", con)

            Da = New SqlClient.SqlDataAdapter("select a.*, b.Count_Name,m.Mill_Name ,stt.sort_no, tTEXSPP.Pavu_Delivery_Increment as Tex_Pavu_Delivery_Increment,  tTEXSPP.Beam_Knotting_Code as Tex_Beam_Knotting_Code , tTEXSPP.Production_Meters as Tex_Production_Meters, tTEXSPP.Close_Status as Tex_Close_Status   from Stock_SizedPavu_Processing_Details a LEFT OUTER JOIN Specification_Head stt ON a.Set_Code = stt.set_code LEFT OUTER JOIN Count_Head b ON a.Count_IdNo = b.Count_IdNo LEFT OUTER JOIN Mill_Head m ON a.Mill_IdNo = m.mill_IdNo   LEFT OUTER JOIN Stock_SizedPavu_Processing_Details tTEXSPP ON  tTEXSPP.SoftwareType_IdNo = " & Str(Val(Common_Procedures.SoftwareTypes.Textile_Software)) & " and tTEXSPP.Reference_Code LIKE '" & Trim(Pk_Condition_Tex) & "%' and tTEXSPP.Selection_From_ReferenceCode = '" & Trim(Pk_Condition) & "'  + a.Pavu_Delivery_Code and tTEXSPP.beam_no = a.beam_no where  " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.Pavu_Delivery_Code = '" & Trim(NewCode) & "' and  " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " (a.ledger_Idno = " & Str(Val(LedNo)) & " or a.ledger_Idno = " & Str(Val(vKuraiPvu_STkID)) & ")  and a.SoftwareType_IdNo = " & Str(Val(Common_Procedures.SoftwareTypes.Sizing_Software)) & " order by a.for_orderby, a.Set_Code, a.ForOrderBy_BeamNo, a.Beam_No, a.sl_no", con)
            'Da = New SqlClient.SqlDataAdapter("select a.*, b.Count_Name,m.Mill_Name ,stt.sort_no from Stock_SizedPavu_Processing_Details a LEFT OUTER JOIN Specification_Head stt ON a.Set_Code = stt.set_code LEFT OUTER JOIN Count_Head b ON a.Count_IdNo = b.Count_IdNo LEFT OUTER JOIN Mill_Head m ON a.Mill_IdNo = m.mill_IdNo  where  " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.Pavu_Delivery_Code = '" & Trim(NewCode) & "' and  " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " (a.ledger_Idno = " & Str(Val(LedNo)) & " or a.ledger_Idno = " & Str(Val(vKuraiPvu_STkID)) & ") order by a.for_orderby, a.Set_Code, a.ForOrderBy_BeamNo, a.Beam_No, a.sl_no", con)

            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()

                    SNo = SNo + 1
                    .Rows(n).Cells(0).Value = Val(SNo)
                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Set_No").ToString
                    .Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Beam_No").ToString
                    .Rows(n).Cells(3).Value = Val(Dt1.Rows(i).Item("Noof_Pcs").ToString)
                    .Rows(n).Cells(4).Value = Format(Val(Dt1.Rows(i).Item("Meters").ToString), "#########0.00")
                    .Rows(n).Cells(5).Value = "1"
                    .Rows(n).Cells(6).Value = Dt1.Rows(i).Item("Set_Code").ToString

                    If Trim(Dt1.Rows(i).Item("DeliveryTo_Name").ToString) <> "" Then
                        .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("DeliveryTo_Name").ToString

                    Else
                        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1163" Then '---- Guru Sizing (Somanur)
                            If Val(Dt1.Rows(i).Item("Vendor_Idno").ToString) <> 0 Then
                                .Rows(n).Cells(7).Value = Common_Procedures.Vendor_IdNoToName(con, Val(Dt1.Rows(i).Item("Vendor_Idno").ToString))
                            End If
                        End If

                    End If

                    'm.Mill_Name ,stt.sort_no

                    .Rows(n).Cells(8).Value = Dt1.Rows(i).Item("Ends_Name").ToString
                    .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("Count_Name").ToString
                    .Rows(n).Cells(10).Value = Dt1.Rows(i).Item("METERS_PC").ToString
                    .Rows(n).Cells(11).Value = Dt1.Rows(i).Item("Net_Weight").ToString
                    .Rows(n).Cells(12).Value = Dt1.Rows(i).Item("Mill_Name").ToString
                    .Rows(n).Cells(13).Value = Dt1.Rows(i).Item("sort_no").ToString

                    For j = 0 To .ColumnCount - 1
                        .Rows(i).Cells(j).Style.ForeColor = Color.Red
                    Next

                    .Rows(n).Cells(14).Value = ""
                    If Val(Dt1.Rows(i).Item("Tex_Pavu_Delivery_Increment").ToString) <> 0 Or Trim(Dt1.Rows(i).Item("Tex_Beam_Knotting_Code").ToString) <> "" Or Val(Dt1.Rows(i).Item("Tex_Production_Meters").ToString) <> 0 Or Val(Dt1.Rows(i).Item("Tex_Close_Status").ToString) <> 0 Then

                        .Rows(n).Cells(14).Value = "1"

                        For j = 0 To dgv_Details.ColumnCount - 1
                            .Rows(n).Cells(j).Style.BackColor = Color.LightGray
                            .Rows(n).Cells(j).Style.ForeColor = Color.Red
                        Next

                    End If

                Next

            End If
            Dt1.Clear()

            'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Then
            '    Da = New SqlClient.SqlDataAdapter("select a.*, b.Count_Name from Stock_SizedPavu_Processing_Details a LEFT OUTER JOIN Count_Head b ON a.Count_IdNo = b.Count_IdNo where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.Pavu_Delivery_Code = '' and  " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " (a.ledger_Idno = " & Str(Val(LedNo)) & ")  order by a.for_orderby, a.Set_Code, a.ForOrderBy_BeamNo, a.Beam_No, a.sl_no", con)
            'Else
            Da = New SqlClient.SqlDataAdapter("select a.*, b.Count_Name,m.Mill_Name ,stt.sort_no from Stock_SizedPavu_Processing_Details a LEFT OUTER JOIN Specification_Head stt ON a.Set_Code = stt.set_code  LEFT OUTER JOIN Count_Head b ON a.Count_IdNo = b.Count_IdNo LEFT OUTER JOIN Mill_Head m ON a.Mill_IdNo = m.mill_IdNo where " & RptCondt & IIf(RptCondt <> "", " and ", "") & " a.Pavu_Delivery_Code = '' and  " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " (a.ledger_Idno = " & Str(Val(LedNo)) & " Or a.ledger_Idno = " & Str(Val(vKuraiPvu_STkID)) & ") and a.SoftwareType_IdNo = " & Str(Val(Common_Procedures.SoftwareTypes.Sizing_Software)) & " order by a.for_orderby, a.Set_Code, a.ForOrderBy_BeamNo, a.Beam_No, a.sl_no", con)
            'End If

            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()

                    SNo = SNo + 1
                    .Rows(n).Cells(0).Value = Val(SNo)
                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Set_No").ToString
                    .Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Beam_No").ToString
                    .Rows(n).Cells(3).Value = Val(Dt1.Rows(i).Item("Noof_Pcs").ToString)
                    .Rows(n).Cells(4).Value = Format(Val(Dt1.Rows(i).Item("Meters").ToString), "#########0.00")
                    .Rows(n).Cells(5).Value = ""
                    .Rows(n).Cells(6).Value = Dt1.Rows(i).Item("Set_Code").ToString

                    If Trim(Dt1.Rows(i).Item("DeliveryTo_Name").ToString) <> "" Then
                        .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("DeliveryTo_Name").ToString

                    Else
                        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1163" Then '---- Guru Sizing (Somanur)
                            If Val(Dt1.Rows(i).Item("Vendor_Idno").ToString) <> 0 Then
                                .Rows(n).Cells(7).Value = Common_Procedures.Vendor_IdNoToName(con, Val(Dt1.Rows(i).Item("Vendor_Idno").ToString))
                            End If
                        End If

                    End If

                    .Rows(n).Cells(8).Value = Dt1.Rows(i).Item("Ends_Name").ToString
                    .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("Count_Name").ToString
                    .Rows(n).Cells(10).Value = Dt1.Rows(i).Item("METERS_PC").ToString
                    .Rows(n).Cells(11).Value = Dt1.Rows(i).Item("Net_Weight").ToString
                    .Rows(n).Cells(12).Value = Dt1.Rows(i).Item("Mill_Name").ToString
                    .Rows(n).Cells(13).Value = Dt1.Rows(i).Item("sort_no").ToString
                    .Rows(n).Cells(13).Value = ""

                Next

            End If
            Dt1.Clear()

        End With

        pnl_Selection.Visible = True
        pnl_Back.Enabled = False
        dgv_Selection.Focus()

        cnt = 0
    End Sub

    Private Sub dgv_Selection_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Selection.CellClick
        Select_Pavu(e.RowIndex)
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1102" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1144" Then '---- Ganesh karthik Sizing (Somanur)
            If cnt > 18 Then
                MessageBox.Show("Does Not Select More than 18 BeamNo", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If txt_BookNo.Enabled And txt_BookNo.Visible Then txt_BookNo.Focus()
                Exit Sub
            End If
        End If
    End Sub

    Private Sub Select_Pavu(ByVal RwIndx As Integer)
        Dim i As Integer

        With dgv_Selection

            If .RowCount > 0 And RwIndx >= 0 Then

                If Val(.Rows(RwIndx).Cells(14).Value) <> 0 Then

                    MessageBox.Show("Invalid Selection : Could Not De-Select this Beam" & Chr(13) & "Alredy Delivered in Textile", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub

                Else

                    .Rows(RwIndx).Cells(5).Value = (Val(.Rows(RwIndx).Cells(5).Value) + 1) Mod 2

                    If Val(.Rows(RwIndx).Cells(5).Value) = 0 Then
                        .Rows(RwIndx).Cells(5).Value = ""
                        For i = 0 To .ColumnCount - 1
                            .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Blue
                        Next

                    Else

                        For i = 0 To .ColumnCount - 1
                            .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
                        Next

                    End If

                    If Val(dgv_Selection.CurrentRow.Cells(5).Value) = 1 Then
                        cnt = cnt + 1
                    Else
                        cnt = cnt - 1
                    End If

                End If

            End If

        End With

    End Sub

    Private Sub chk_SelectAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chk_SelectAll.CheckedChanged
        Dim i As Integer
        Dim J As Integer

        With dgv_Selection

            For i = 0 To .Rows.Count - 1
                .Rows(i).Cells(5).Value = ""
                For J = 0 To .ColumnCount - 1
                    .Rows(i).Cells(J).Style.ForeColor = Color.Blue
                Next J
            Next i

            If chk_SelectAll.Checked = True Then
                For i = 0 To .Rows.Count - 1
                    Select_Pavu(i)
                Next i
            End If

            If .Rows.Count > 0 Then
                .Focus()
                .CurrentCell = .Rows(0).Cells(0)
                .CurrentCell.Selected = True
            End If

        End With

    End Sub


    Private Sub dgv_Selection_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Selection.KeyDown
        On Error Resume Next
        vcbo_KeyDwnVal = e.KeyValue
        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Space Then
            If dgv_Selection.CurrentCell.RowIndex >= 0 Then
                Select_Pavu(dgv_Selection.CurrentCell.RowIndex)
                e.Handled = True
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1102" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1144" Then '---- Ganesh karthik Sizing (Somanur)
                    If cnt > 18 Then
                        MessageBox.Show("Does Not Select More than 18 BeamNo", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If txt_BookNo.Enabled And txt_BookNo.Visible Then txt_BookNo.Focus()
                        Exit Sub
                    End If

                End If
            End If
        End If
    End Sub

    Private Sub dgv_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.LostFocus
        On Error Resume Next
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
    End Sub

    Private Sub btn_Print_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        Print_PDF_Status = False
        print_record()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        dgv_Details.Rows.Add()
    End Sub

    Private Sub dtp_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Date.KeyDown
        If e.KeyValue = 38 Then e.Handled = True : txt_Remarks.Focus() ' SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then e.Handled = True : SendKeys.Send("{TAB}")
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.PrintEntry, Common_Procedures.UR.ENTRY_SIZING_JOBWORK_MODULE_PAVU_DELIVERY, New_Entry) = False Then Exit Sub
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1034" Then '---- Asia Sizing (Palladam)
            pnl_Print.Visible = True
            pnl_Back.Enabled = False
            If btn_Print_Preprint.Enabled And btn_Print_Preprint.Visible Then
                btn_Print_Preprint.Focus()
            End If

        ElseIf Common_Procedures.settings.Dos_Printing = 1 Then
            Pnl_DosPrint.Visible = True
            pnl_Back.Enabled = False
            If Btn_DosPrint.Enabled And Btn_DosPrint.Visible Then
                Btn_DosPrint.Focus()
            End If

        Else
            printing_invoice()

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

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.* from Sizing_Pavu_Delivery_Head a LEFT OUTER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo where a.Pavu_Delivery_Code = '" & Trim(NewCode) & "'", con)
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

        prn_TotCopies = 1
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1263" Then  '---- Meenashi Sizing (Somanur)
            inpno = InputBox("Enter No.of Copies", "FOR PRINTING...", 3)
            prn_TotCopies = Val(inpno)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1282" Then ' ---------brt
            inpno = InputBox("Enter No.of Copies", "FOR PRINTING...", 4)
            prn_TotCopies = Val(inpno)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1288" Then '---------kkp
            inpno = InputBox("Enter No.of Copies", "FOR PRINTING...", 1)
            prn_TotCopies = Val(inpno)
        End If


        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1034" Then '---- Asia Sizing (palladam)
            prn_TotCopies = 2
        End If

        If Val(prn_TotCopies) <= 0 Then
            Exit Sub
        End If


        If Common_Procedures.settings.CustomerCode = "1288" Then
            PrintDocument1.DefaultPageSettings.Landscape = True
        Else
            PrintDocument1.DefaultPageSettings.Landscape = False
        End If



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


        '-----------------------------------

        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then
            Try

                If Print_PDF_Status = True Then
                    '--This is actual & correct 

                    PrintDocument1.DocumentName = "Document"
                    PrintDocument1.PrinterSettings.PrinterName = "doPDF v7"
                    PrintDocument1.PrinterSettings.PrintFileName = "c:\Document.pdf"
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







    End Sub

    Private Sub PrintDocument1_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument1.BeginPrint
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim NewCode As String


        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_PageNo = 0
        prn_NoofBmDets = 0
        prn_DetMxIndx = 0
        prn_Count = 0
        Erase prn_DetAr

        prn_DetAr = New String(50, 10) {}

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.*, VH.* , Csh.State_Name as Company_State_Name, Csh.State_Code as Company_State_Code, Lsh.State_Name as Ledger_State_Name, Lsh.State_Code as Ledger_State_Code,d.*,f.Ledger_MainName as DelName , f.Ledger_Address1 as DelAdd1 ,f.Ledger_Address2 as DelAdd2, f.Ledger_Address3 as DelAdd3 ,f.Ledger_Address4 as DelAdd4,f.Ledger_GSTinNo as DelGSTinNo,DSH.State_Name as DelState_Name ,DSH.State_Code as Delivery_State_Code, MH.Mill_Name from Sizing_Pavu_Delivery_Head a INNER JOIN Company_Head b ON a.Company_IdNo <> 0 and a.Company_IdNo = b.Company_IdNo  LEFT OUTER JOIN State_Head Csh ON b.Company_State_IdNo = csh.State_IdNo INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo  LEFT OUTER JOIN State_Head Lsh ON c.ledger_State_IdNo = Lsh.State_IdNo LEFT OUTER JOIN Transport_Head d ON a.Transport_IdNo = d.Transport_IdNo  LEFT OUTER JOIN Delivery_Party_Head f ON a.DeliveryTo_IdNo = f.Ledger_IdNo LEFT OUTER JOIN State_HEad DSH on f.Ledger_State_IdNo = DSH.State_IdNo  LEFT OUTER JOIN Mill_Head MH ON a.Mill_IdNo = mh.Mill_IdNo INNER JOIN Vendor_Head VH on a.Vendor_IdNo = VH.Vendor_IdNo where a.Pavu_Delivery_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then
                If Trim(prn_HdDt.Rows(0).Item("Invoice_Selection_Type").ToString) = "DIRECT" Then
                    da2 = New SqlClient.SqlDataAdapter("select a.*,b.* from Sizing_Pavu_Delivery_Details a LEFT OUTER JOIN Count_Head b ON a.Count_IdNo = b.Count_IdNo where Pavu_Delivery_Code = '" & Trim(NewCode) & "' Order by ForOrderBy_BeamNo, sl_no", con)
                Else
                    da2 = New SqlClient.SqlDataAdapter("select a.*,b.*,stt.Sort_NO, tM.Mill_Name AS set_Millname from Stock_SizedPavu_Processing_Details a LEFT OUTER JOIN Specification_Head stt ON a.Set_Code = stt.set_code LEFT OUTER JOIN Count_Head b ON a.Count_IdNo = b.Count_IdNo LEFT OUTER JOIN Mill_Head tM ON a.Mill_IdNo = tM.Mill_IdNo where Pavu_Delivery_Code = '" & Trim(NewCode) & "' Order by ForOrderBy_BeamNo, sl_no", con)
                End If
                'prn_DetDt = New DataTable
                da2.Fill(prn_DetDt)

                If prn_DetDt.Rows.Count > 0 Then
                    For i = 0 To prn_DetDt.Rows.Count - 1
                        If Val(prn_DetDt.Rows(i).Item("Meters").ToString) <> 0 Then
                            prn_DetMxIndx = prn_DetMxIndx + 1
                            prn_DetAr(prn_DetMxIndx, 1) = Trim(Val(prn_DetMxIndx))
                            prn_DetAr(prn_DetMxIndx, 2) = Trim(prn_DetDt.Rows(i).Item("Beam_No").ToString)
                            prn_DetAr(prn_DetMxIndx, 3) = Val(prn_DetDt.Rows(i).Item("Noof_Pcs").ToString)
                            prn_DetAr(prn_DetMxIndx, 4) = Format(Val(prn_DetDt.Rows(i).Item("Meters").ToString), "#########0.00")
                            prn_DetAr(prn_DetMxIndx, 5) = Trim(Microsoft.VisualBasic.Left(prn_DetDt.Rows(i).Item("DeliveryTo_Name").ToString, 15))
                            prn_DetAr(prn_DetMxIndx, 6) = Trim(prn_DetDt.Rows(i).Item("Set_No").ToString)
                            prn_DetAr(prn_DetMxIndx, 7) = Trim(prn_DetDt.Rows(i).Item("Ends_Name").ToString)
                            prn_DetAr(prn_DetMxIndx, 8) = Trim(prn_DetDt.Rows(i).Item("Count_Name").ToString)
                            prn_DetAr(prn_DetMxIndx, 9) = Trim(prn_DetDt.Rows(i).Item("meters_pc").ToString)
                        End If
                    Next i
                End If

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
                Printing_Format5(e)
            Else
                Printing_Format2(e)
            End If 'End 

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1102" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1144" Then  'Or Trim(Common_Procedures.settings.CustomerCode) = "1282"'---- Ganesh karthik Sizing (Somanur)
            Printing_Format6(e)

        ElseIf Common_Procedures.settings.Dos_Printing = 1 Then
            If prn_Status = 1 Then
                Printing_Format3()

            ElseIf Val(Common_Procedures.settings.PavuDelivery_Print_2Copy_In_SinglePage) = 1 Then
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1102" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1144" Then '---- Ganesh karthik Sizing (Somanur)
                    Printing_Format6(e)
                Else
                    Printing_Format5(e)
                End If

            Else
                Printing_Format5(e)

            End If

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1288" Then
            Printing_Format7(e)

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1282" Then
            Printing_Format8(e)

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1263" Then
            Printing_Format9(e)

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1155" Then
            Printing_Format10(e)

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then
            Printing_Format1087(e)

        Else

            Printing_Format5(e)

        End If

    End Sub

    Private Sub Printing_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
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
        Dim PS As Printing.PaperSize

        'Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8X6", 800, 600)
        'PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
        'PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1

        ''PrintDocument pd = new PrintDocument();
        ''pd.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("PaperA4", 840, 1180);
        ''pd.Print();

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

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 20  ' 50 
            .Right = 55
            .Top = 35   '30
            .Bottom = 35 ' 30
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

        NoofItems_PerPage = 5

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = Val(30) : ClArr(2) = 55 : ClArr(3) = 75 : ClArr(4) = 85 : ClArr(5) = 120
        ClArr(6) = 35 : ClArr(7) = 55 : ClArr(8) = 75 : ClArr(9) = 85
        ClArr(10) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9))

        TxtHgt = 18.25 ' 18.5 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

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

                            Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx, 1))), LMargin + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 2)), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx, 3))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx, 4)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 5)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 10, CurY, 0, 0, pFont)

                            prn_NoofBmDets = prn_NoofBmDets + 1

                        End If

                        If Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 4)) <> 0 Then

                            Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 1))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + 12, CurY, 0, 0, pFont)
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

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        'e.HasMorePages = False
        prn_Count = prn_Count + 1

        e.HasMorePages = False

        If Val(prn_TotCopies) > 1 Then
            If prn_Count < Val(prn_TotCopies) Then

                prn_DetIndx = 0
                prn_PageNo = 0
                prn_DetIndx = 0
                prn_PageNo = 0
                prn_NoofBmDets = 0


                e.HasMorePages = True
                Return

            End If

        End If

    End Sub

    Private Sub Printing_Format1_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim da3 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim p1Font As Font
        Dim vSetNo As String
        Dim vMill_Nm As String
        Dim strHeight As Single
        Dim W1 As Single, N1 As Single, M1 As Single
        Dim i As Integer, k As Integer
        Dim Arr(300, 5) As String
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_Email As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim Gst_dt As Date
        Dim Entry_dt As Date
        Dim CurX As Single = 0
        Dim strWidth As Single = 0

        PageNo = PageNo + 1

        CurY = TMargin

        If prn_DetMxIndx > (2 * NoofItems_PerPage) Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If

        da2 = New SqlClient.SqlDataAdapter("select DISTINCT(setcode_forSelection) from Stock_SizedPavu_Processing_Details where Pavu_Delivery_Code = '" & Trim(EntryCode) & "' Order by setcode_forSelection", con)
        dt3 = New DataTable
        da2.Fill(dt3)

        vSetNo = ""
        If dt3.Rows.Count > 0 Then
            For i = 0 To dt3.Rows.Count - 1
                k = InStr(1, dt3.Rows(i).Item("setcode_forSelection").ToString, "/")
                vSetNo = vSetNo & IIf(Trim(vSetNo) <> "", ", ", "") & Microsoft.VisualBasic.Left(dt3.Rows(i).Item("setcode_forSelection").ToString, k - 1)
            Next i
        End If
        dt3.Dispose()

        da3 = New SqlClient.SqlDataAdapter("select b.mill_name from Stock_SizedPavu_Processing_Details a LEFT OUTER JOIN Mill_Head b ON a.Mill_IdNo = b.Mill_IdNo where a.Pavu_Delivery_Code = '" & Trim(EntryCode) & "'", con)
        dt4 = New DataTable
        da3.Fill(dt4)

        vMill_Nm = ""
        If dt4.Rows.Count > 0 Then
            vMill_Nm = dt4.Rows(0).Item("mill_name").ToString
        End If
        dt4.Dispose()

        CurY = TMargin
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
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
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

        Gst_dt = #7/1/2017#
        Entry_dt = dtp_Date.Value

        If DateDiff("d", Gst_dt.ToShortDateString, Entry_dt.ToShortDateString) < 0 Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)

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
            CurY = CurY + TxtHgt

        End If


        CurY = CurY + TxtHgt - 12
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "SIZED BEAM DELIVERY NOTE", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight + 3
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        ' Try

        N1 = e.Graphics.MeasureString("TO   : ", pFont).Width
        W1 = e.Graphics.MeasureString("SET NO  :  ", pFont).Width

        M1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7)

        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 11, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, "TO :  M/s. " & prn_HdDt.Rows(0).Item("Ledger_MainName").ToString, LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "DC.NO", LMargin + M1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
        p1Font = New Font("Calibri", 14, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Pavu_Delivery_No").ToString, LMargin + M1 + W1 + 25, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "DATE : " & Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Pavu_Delivery_Date").ToString), "dd-MM-yyyy").ToString, PageWidth - 10, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "SET NO", LMargin + M1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, vSetNo, LMargin + M1 + W1 + 25, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "ENDS", LMargin + M1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ends_Name").ToString, LMargin + M1 + W1 + 25, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "COUNT", LMargin + M1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Count_Name").ToString, LMargin + M1 + W1 + 25, CurY, 0, 0, pFont)

        If DateDiff("d", Gst_dt.ToShortDateString, Entry_dt.ToShortDateString) < 0 Then

            CurY = CurY + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, " TIN NO : " & prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)
            End If

        Else

            CurY = CurY + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("ledger_GSTinNo").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, " GSTIN NO : " & prn_HdDt.Rows(0).Item("ledger_GSTinNo").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)
            End If

        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1078" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1112" Then '---- Kalaimagal Sizing (Palladam)
            Common_Procedures.Print_To_PrintDocument(e, "MILL", LMargin + M1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(vMill_Nm), LMargin + M1 + W1 + 25, CurY, 0, 0, pFont)
        End If

        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + M1, LnAr(3), LMargin + M1, LnAr(2))

        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin, CurY, 2, ClAr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "BEAM", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "PCS", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "DELIVERY TO", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 3, CurY, 2, ClAr(6), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "BEAM", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "PCS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "DELIVERY TO", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, 2, ClAr(10), pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY

        ' Catch ex As Exception

        'MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        '  End Try

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
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Pcs").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
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


            CurY = CurY + TxtHgt - 10

            Common_Procedures.Print_To_PrintDocument(e, "Vehicle No  :  " & Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString), LMargin + 10, CurY, 0, 0, pFont)
            If Trim(prn_HdDt.Rows(0).Item("Book_No").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "Book No  :  " & Trim(prn_HdDt.Rows(0).Item("Book_No").ToString), PageWidth - 200, CurY, 0, 0, pFont)
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
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 5, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin, LnAr(1))
            e.Graphics.DrawLine(Pens.Black, PageWidth, CurY, PageWidth, LnAr(1))

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format2(ByRef e As System.Drawing.Printing.PrintPageEventArgs)  '---Asia Sizing preprint
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
        Dim I As Integer, NoofDets As Integer
        Dim time As String = ""
        Dim EntryCode As String

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
            .Top = 0 ' 65
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
        NoofItems_PerPage = 8


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

        CurX = LMargin + 65 ' 40  '150
        CurY = TMargin + 80 ' 122 ' 100
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "To M/s. " & prn_HdDt.Rows(0).Item("Ledger_MainName").ToString, CurX, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, CurX + 20, CurY, 0, 0, pFont)

        CurX = LMargin + 420
        CurY = TMargin + 80
        p1Font = New Font("Calibri", 14, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "PAVU DELIVERY NOTE", CurX, CurY, 0, 0, p1Font)

        CurX = LMargin + 80
        CurY = TMargin + 140
        p1Font = New Font("Calibri", 14, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "DEL NO : " & prn_HdDt.Rows(0).Item("Pavu_Delivery_No").ToString, CurX, CurY, 0, 0, p1Font)

        CurX = LMargin + 500
        Common_Procedures.Print_To_PrintDocument(e, "DATE : " & Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Pavu_Delivery_Date").ToString), "dd-MM-yyyy"), CurX, CurY, 0, 0, p1Font)

        CurX = LMargin + 80
        CurY = TMargin + 160
        p1Font = New Font("Calibri", 14, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "SET NO : " & prn_DetDt.Rows(0).Item("Set_No").ToString, CurX, CurY, 0, 0, p1Font)

        CurX = LMargin + 300
        Common_Procedures.Print_To_PrintDocument(e, "ENDS : " & prn_HdDt.Rows(0).Item("Ends_Name").ToString, CurX, CurY, 0, 0, p1Font)

        CurX = LMargin + 500
        Common_Procedures.Print_To_PrintDocument(e, "COUNT : " & prn_HdDt.Rows(0).Item("Count_Name").ToString, CurX, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt
        CurX = LMargin + 60
        e.Graphics.DrawLine(Pens.Black, CurX, CurY, CurX + 730, CurY)

        CurX = LMargin + 65 ' 40  '150
        CurY = TMargin + 190 ' 122 ' 100
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "S.No", CurX, CurY, 0, 0, p1Font)

        CurX = LMargin + 120 ' 40  '150
        Common_Procedures.Print_To_PrintDocument(e, "Beam No", CurX, CurY, 0, 0, p1Font)

        CurX = LMargin + 260 ' 40  '150
        Common_Procedures.Print_To_PrintDocument(e, "Pcs", CurX, CurY, 0, 0, p1Font)

        CurX = LMargin + 320 ' 40  '150
        Common_Procedures.Print_To_PrintDocument(e, "Meters", CurX, CurY, 0, 0, p1Font)

        CurX = LMargin + 420
        Common_Procedures.Print_To_PrintDocument(e, "S.No", CurX, CurY, 0, 0, p1Font)

        CurX = LMargin + 480 ' 40  '150
        Common_Procedures.Print_To_PrintDocument(e, "Beam No", CurX, CurY, 0, 0, p1Font)

        CurX = LMargin + 620 ' 40  '150
        Common_Procedures.Print_To_PrintDocument(e, "Pcs", CurX, CurY, 0, 0, p1Font)

        CurX = LMargin + 680 ' 40  '150
        Common_Procedures.Print_To_PrintDocument(e, "Meters", CurX, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt
        CurX = LMargin + 60
        e.Graphics.DrawLine(Pens.Black, CurX, CurY, CurX + 730, CurY)

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try
            If prn_HdDt.Rows.Count > 0 Then

                NoofDets = 0

                CurY = 200 ' 370

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_NoofBmDets < prn_DetMxIndx

                        If NoofDets >= NoofItems_PerPage Then

                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            prn_DetIndx = prn_DetIndx + NoofItems_PerPage

                            e.HasMorePages = True

                            CurY = TMargin + 440
                            e.Graphics.DrawLine(Pens.Black, LMargin + 115, CurY, LMargin + 115, TMargin + 160 + TxtHgt)
                            e.Graphics.DrawLine(Pens.Black, LMargin + 255, CurY, LMargin + 255, TMargin + 160 + TxtHgt)
                            e.Graphics.DrawLine(Pens.Black, LMargin + 315, CurY, LMargin + 315, TMargin + 160 + TxtHgt)
                            e.Graphics.DrawLine(Pens.Black, LMargin + 415, CurY, LMargin + 415, TMargin + 160 + TxtHgt)
                            e.Graphics.DrawLine(Pens.Black, LMargin + 475, CurY, LMargin + 475, TMargin + 160 + TxtHgt)
                            e.Graphics.DrawLine(Pens.Black, LMargin + 615, CurY, LMargin + 615, TMargin + 160 + TxtHgt)
                            e.Graphics.DrawLine(Pens.Black, LMargin + 675, CurY, LMargin + 675, TMargin + 160 + TxtHgt)


                            Return

                        End If

                        prn_DetIndx = prn_DetIndx + 1

                        CurY = CurY + TxtHgt

                        If Val(prn_DetAr(prn_DetIndx, 4)) <> 0 Then

                            Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx, 1))), LMargin + 65, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 2)), LMargin + 125, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx, 3))), LMargin + 310, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx, 4)), "#########0.00"), LMargin + 410, CurY, 1, 0, pFont)

                            prn_NoofBmDets = prn_NoofBmDets + 1

                        End If

                        If Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 4)) <> 0 Then

                            Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 1))), LMargin + 425, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 2)), LMargin + 485, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 3))), LMargin + 670, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 4)), "#########0.00"), LMargin + 770, CurY, 1, 0, pFont)

                            prn_NoofBmDets = prn_NoofBmDets + 1

                        End If

                        NoofDets = NoofDets + 1

                    Loop

                End If

                CurY = TMargin + 390
                e.Graphics.DrawLine(Pens.Black, LMargin + 60, CurY, LMargin + 790, CurY)

                CurX = LMargin + 120
                CurY = TMargin + 400
                Common_Procedures.Print_To_PrintDocument(e, "TOTAL", CurX, CurY, 0, 0, pFont)

                CurX = LMargin + 550
                CurY = TMargin + 400

                If (prn_DetMxIndx Mod (NoofItems_PerPage * 2)) <= NoofItems_PerPage Then

                    If Val(prn_HdDt.Rows(0).Item("Total_Pcs").ToString) <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Pcs").ToString), LMargin + 310, CurY, 1, 0, pFont)
                    End If

                    If Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString) <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString), "#########0.00"), LMargin + 410, CurY, 1, 0, pFont)
                    End If

                Else

                    If Val(prn_HdDt.Rows(0).Item("Total_Pcs").ToString) <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Pcs").ToString), LMargin + 670, CurY, 1, 0, pFont)
                    End If

                    If Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString) <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString), "#########0.00"), LMargin + 770, CurY, 1, 0, pFont)
                    End If

                End If

                CurY = TMargin + 440
                e.Graphics.DrawLine(Pens.Black, LMargin + 115, CurY, LMargin + 115, TMargin + 160 + TxtHgt)
                e.Graphics.DrawLine(Pens.Black, LMargin + 255, CurY, LMargin + 255, TMargin + 160 + TxtHgt)
                e.Graphics.DrawLine(Pens.Black, LMargin + 315, CurY, LMargin + 315, TMargin + 160 + TxtHgt)
                e.Graphics.DrawLine(Pens.Black, LMargin + 415, CurY, LMargin + 415, TMargin + 160 + TxtHgt)
                e.Graphics.DrawLine(Pens.Black, LMargin + 475, CurY, LMargin + 475, TMargin + 160 + TxtHgt)
                e.Graphics.DrawLine(Pens.Black, LMargin + 615, CurY, LMargin + 615, TMargin + 160 + TxtHgt)
                e.Graphics.DrawLine(Pens.Black, LMargin + 675, CurY, LMargin + 675, TMargin + 160 + TxtHgt)

            End If

            CurY = TMargin + 440
            e.Graphics.DrawLine(Pens.Black, LMargin + 60, CurY, LMargin + 790, CurY)

            CurX = LMargin + 200
            CurY = TMargin + 450
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString), CurX, CurY, 0, 0, pFont)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

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

    Private Sub txt_SetNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_SetNo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            btn_Selection_Click(sender, e)

        End If
    End Sub
    Private Sub btn_Close_DosPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_DosPrint.Click
        pnl_Back.Enabled = True
        Pnl_DosPrint.Visible = False
    End Sub

    Private Sub Btn_DosPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Btn_DosPrint.Click
        prn_Status = 1
        Printing_Format3()
        btn_Close_DosPrint_Click(sender, e)
    End Sub

    Private Sub Btn_LaserPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Btn_LaserPrint.Click
        prn_Status = 2
        printing_invoice()
        btn_Close_DosPrint_Click(sender, e)
    End Sub

    Private Sub Get_DosLoneDetails()
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

        CenCon = Common_Procedures.Dos_DottedLines.CenCon
        CenDwn = Common_Procedures.Dos_DottedLines.CenDwn
        CenUp = Common_Procedures.Dos_DottedLines.CenUp

    End Sub

    Private Sub Printing_Format3()
        Dim J As Integer
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim NewCode As String
        ' Dim NoofDets As Integer, NoofItems_PerPage As Integer
        Dim BmAr(300, 5) As String
        Dim u9 As Single = 0, Tb As Single = 0, tp As Single = 0, u10 As Single = 0
        Dim i As Integer = 0
        Dim tm As Single = 0
        '  Dim Dc_No As String, StNo As String
        Dim Yy As Integer
        Dim L1 As Integer = 0
        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.* from Sizing_Pavu_Delivery_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo where a.Pavu_Delivery_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                u9 = 2
                J = 0

                da2 = New SqlClient.SqlDataAdapter("select a.* ,C.Count_Name from Stock_SizedPavu_Processing_Details a  LEFT OUTER JOIN Count_Head C ON a.Count_IdNo = C.Count_IdNo where Pavu_Delivery_Code = '" & Trim(NewCode) & "' Order by ForOrderBy_BeamNo, sl_no", con)
                prn_DetDt = New DataTable
                da2.Fill(prn_DetDt)
                Erase BmAr
                BmAr = New String(500, 50) {}

                If prn_DetDt.Rows.Count > 0 Then
                    For i = 0 To prn_DetDt.Rows.Count - 1
                        If Val(prn_DetDt.Rows(i).Item("Meters").ToString) <> 0 Then
                            J = J + 1
                            BmAr(u9 - 1, 0) = Trim(J)
                            BmAr(u9 - 1, 1) = Trim(prn_DetDt.Rows(i).Item("Set_No").ToString)
                            BmAr(u9 - 1, 2) = Trim(prn_DetDt.Rows(i).Item("Ends_Name").ToString)
                            BmAr(u9 - 1, 3) = Trim(prn_DetDt.Rows(i).Item("Count_Name").ToString)
                            BmAr(u9 - 1, 4) = Trim(prn_DetDt.Rows(i).Item("Beam_No").ToString)
                            BmAr(u9 - 1, 5) = Val(prn_DetDt.Rows(i).Item("Noof_Pcs").ToString)
                            BmAr(u9 - 1, 6) = Format(Val(prn_DetDt.Rows(i).Item("Meters").ToString), "#########0.00")
                            BmAr(u9 - 1, 7) = Trim(Microsoft.VisualBasic.Left(prn_DetDt.Rows(i).Item("DeliveryTo_Name").ToString, 16))
                            Tb = Tb + 1 : tp = tp + Val(prn_DetDt.Rows(i).Item("Noof_Pcs").ToString) : tm = tm + Val(prn_DetDt.Rows(i).Item("Meters").ToString)

                            u9 = u9 + 1
                        End If
                    Next i
                End If
            Else

                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            End If

            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try


        Get_DosLoneDetails()
        LnCnt = 0

        pth = Common_Procedures.Dos_Printing_FileName_Path

        'If File.Exists(pth) = False Then
        '    fs = New FileStream(pth, FileMode.Create)
        '    w = New StreamWriter(fs, System.Text.Encoding.Default)
        '    w.Close()
        '    fs.Close()
        '    w.Dispose()
        '    fs.Dispose()
        'End If

        fs = New FileStream(pth, FileMode.Create)
        w = New StreamWriter(fs, System.Text.Encoding.Default)


        Try




            If Tb > 8 Then
                Yy = Tb + 2  ' 16 + 29
            Else
                Yy = 8
            End If
            L1 = 0
LOOP1:
            ' u10 = L1


            Printing_Format3_PageHeader()

            For u10 = L1 + 1 To Yy

                L1 = L1 + 1

                PrnTxt = Chr(Vz1) & Trim(BmAr(u10, 0)) & Space(6 - Len(BmAr(u10, 0))) & Chr(Vz2) &
                                    Trim(BmAr(u10, 1)) & Space(6 - Len(BmAr(u10, 1))) & Chr(Vz2) &
                                    Trim(BmAr(u10, 2)) & Space(7 - Len(BmAr(u10, 2))) & Chr(Vz2) &
                                    Trim(BmAr(u10, 3)) & Space(5 - Len(BmAr(u10, 3))) & Chr(Vz2) &
                                    Trim(BmAr(u10, 4)) & Space(7 - Len(BmAr(u10, 4))) & Chr(Vz2) &
                                    Space(5 - Len(BmAr(u10, 5))) & Trim(BmAr(u10, 5)) & Chr(Vz1) &
                                    Space(12 - Len(BmAr(u10, 6))) & Trim(BmAr(u10, 6)) & Chr(Vz1) &
                                    Trim(BmAr(u10, 7)) & Space(23 - Len(BmAr(u10, 7))) & Chr(Vz2)
                w.WriteLine(PrnTxt)
                LnCnt = LnCnt + 1


            Next u10

            PrnTxt = Chr(LfCon) & StrDup(6, Chr(Hz2)) & Chr(194) &
                                  StrDup(6, Chr(Hz2)) & Chr(194) &
                                  StrDup(7, Chr(Hz2)) & Chr(194) &
                                  StrDup(5, Chr(Hz2)) & Chr(194) &
                                  StrDup(7, Chr(Hz2)) & Chr(194) &
                                  StrDup(5, Chr(Hz2)) & Chr(194) &
                                  StrDup(12, Chr(Hz2)) & Chr(194) &
                                  StrDup(23, Chr(Hz2)) & Chr(RgtCon)

            w.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1

            If Tb <= Yy Then

                PrnTxt = Chr(Vz1) & Space(6) &
                         Chr(Vz2) & Space(6) &
                         Chr(Vz2) & Space(7) &
                         Chr(Vz2) & Space(5) &
                         Chr(Vz2) & Trim(Tb) & Space(7 - Len(Trim(Tb))) &
                         Chr(Vz2) & Space(5 - Len(Trim(tp))) & Trim(Trim(tp)) &
                         Chr(Vz1) & Space(12 - Len(Trim(tm))) & Trim(tm) &
                         Chr(Vz2) & Space(23) & Chr(Vz2)
                w.WriteLine(PrnTxt)
                LnCnt = LnCnt + 1

            Else


                PrnTxt = Chr(Vz1) & Space(6) &
                         Chr(Vz2) & Space(7) &
                         Chr(Vz2) & Space(5) &
                         Chr(Vz2) & Space(7) &
                         Chr(Vz2) & Space(5) &
                         Chr(Vz1) & Space(12) &
                         Chr(Vz2) & Space(23 - Len("Continued..")) & "Continued.." & Chr(Vz2)

                w.WriteLine(PrnTxt)
                LnCnt = LnCnt + 1

                Printing_Format3_PageFooter()
                If L1 = 8 Then Yy = 16

                GoTo LOOP1

                'PrnTxt = Chr(Vz1) & Space(6) & Chr(Vz2) & Space(15) & Chr(Vz2) & Space(11) & Chr(Vz2) & Space(8) & Chr(Vz2) & Space(11) & Chr(Vz1) & Space(22) & Chr(Vz2)
                'w.WriteLine(PrnTxt)
                'LnCnt = LnCnt + 1
            End If


            Printing_Format3_PageFooter()

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

            'MessageBox.Show("Printed Sucessfully!!!", "PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            w.Close()
            fs.Close()
            w.Dispose()
            fs.Dispose()

        End Try

    End Sub

    Public Sub Printing_Format3_PageHeader()
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String, Cmp_Add3 As String, Cmp_Add4 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String


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

            '   If File.Exists(pth) = True Then w = New StreamWriter(fs, System.Text.Encoding.Default) 'w = New StreamWriter(pth)

            PrnTxt = Chr(Corn1) & StrDup(78, Chr(Hz1)) & Chr(Corn2)
            w.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1

            PrnTxt = Chr(27) & "@" & Chr(18) & Chr(27) & "P" & Chr(27) & "t1" & Chr(27) & "2" & Chr(27) & "x0"
            LnCnt = LnCnt + 1

            'PrnTxt = ""
            'w.WriteLine(PrnTxt)
            'LnCnt = LnCnt + 1

            PrnTxt = Chr(Vz1) & Space(39 - Len(Cmp_Name)) & Chr(14) & Chr(27) & "E" & Cmp_Name & Chr(27) & "F" & Chr(20) & Space(39 - Len(Cmp_Name)) & Chr(Vz1)
            w.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1

            If Len(Trim(Cmp_Add1 & " " & Cmp_Add2)) > 78 Then

                PrnTxt = Chr(Vz1) & Space(78) & Chr(Vz1) & Chr(13) & Space(2) & Chr(15) & Space(65 - ((Len((Cmp_Add1) & " " & (Cmp_Add2)) / 2) + 0.1)) & Trim(Cmp_Add1 & " " & Cmp_Add2) & Chr(18)
                w.WriteLine(PrnTxt)
                LnCnt = LnCnt + 1
                PrnTxt = Chr(Vz1) & Space(78) & Chr(Vz1) & Chr(13) & Space(2) & Chr(15) & Space(65 - ((Len((Cmp_Add3) & " " & (Cmp_Add4)) / 2) + 0.1)) & Trim(Cmp_Add3 & " " & Cmp_Add4) & Chr(18)
                w.WriteLine(PrnTxt)
                LnCnt = LnCnt + 1
            Else
                PrnTxt = Chr(Vz1) & Space(39 - ((Len((Cmp_Add1) & " " & (Cmp_Add2)) / 2) + 0.1)) & Trim(Cmp_Add1 & " " & Cmp_Add2) & Space(39 - ((Len(Cmp_Add1 & " " & Cmp_Add2) / 2) + 0.1)) & Space(Len(Cmp_Add1 & " " & Cmp_Add2) Mod 2) & Chr(Vz1)
                w.WriteLine(PrnTxt)
                LnCnt = LnCnt + 1
                PrnTxt = Chr(Vz1) & Space(39 - ((Len((Cmp_Add3) & " " & (Cmp_Add4)) / 2) + 0.1)) & Trim(Cmp_Add3 & " " & Cmp_Add4) & Space(39 - ((Len(Cmp_Add3 & " " & Cmp_Add4) / 2) + 0.1)) & Space(Len(Cmp_Add3 & " " & Cmp_Add4) Mod 2) & Chr(Vz1)
                w.WriteLine(PrnTxt)
                LnCnt = LnCnt + 1
            End If

            PrnTxt = Chr(Vz1) & Space(35 - Math.Round((Len(Cmp_PhNo) / 2) + 0.1)) & "Phone : " & Trim(Cmp_PhNo) & Space(35 - Math.Round((Len(Cmp_PhNo) / 2) + 0.1)) & Space(Len(Cmp_PhNo) Mod 2) & Chr(Vz1)
            w.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1
            PrnTxt = Chr(Vz1) & Space(78) & Chr(Vz1)
            w.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1

            PrnTxt = Chr(Vz1) & Space(15) & Chr(14) & Chr(27) & "E" & "SIZED BEAM DELIVERY NOTE" & Chr(27) & "F" & Chr(20) & Space(15) & Chr(Vz1)
            w.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1

            '39 ,38
            PrnTxt = Chr(LfCon) & StrDup(39, Chr(Hz2)) & Chr(194) & StrDup(38, Chr(Hz2)) & Chr(RgtCon)
            w.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1


            PrnTxt = Chr(Vz1) & Space(1) & "To : " & Space(33) & Chr(Vz2) & Space(38) & Chr(Vz1)
            w.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1

            PrnTxt = Chr(Vz1) & Space(4) & "M/s." & Trim(prn_HdDt.Rows(0).Item("Ledger_MainName").ToString) & Space(31 - Len(Trim(prn_HdDt.Rows(0).Item("Ledger_MainName").ToString))) & Chr(Vz2) & Space(1) & "DC NO  : " & Trim(prn_HdDt.Rows(0).Item("Pavu_Delivery_No").ToString) & Space(28 - Len(Trim(prn_HdDt.Rows(0).Item("Pavu_Delivery_No").ToString))) & Chr(Vz1)
            w.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1

            PrnTxt = Chr(Vz1) & Space(4) & Trim(prn_HdDt.Rows(0).Item("Ledger_Address1").ToString) & Space(35 - Len(Trim(prn_HdDt.Rows(0).Item("Ledger_Address1").ToString))) & Chr(Vz1) & Space(1) & "DATE   : " & Trim(Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Pavu_Delivery_Date").ToString), "dd-MM-yyyy")) & Space(28 - Len(Trim(Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Pavu_Delivery_Date").ToString), "dd-MM-yyyy")))) & Chr(Vz1)
            w.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1

            If Trim(prn_HdDt.Rows(0).Item("Ledger_Address2").ToString) <> "" Then
                PrnTxt = Chr(Vz1) & Space(4) & Trim(prn_HdDt.Rows(0).Item("Ledger_Address2").ToString) & Space(35 - Len(Trim(prn_HdDt.Rows(0).Item("Ledger_Address2").ToString))) & Chr(Vz1) & Space(38) & Chr(Vz1)
                w.WriteLine(PrnTxt)
                LnCnt = LnCnt + 1
            End If
            If Trim(prn_HdDt.Rows(0).Item("Ledger_Address3").ToString) <> "" Then
                PrnTxt = Chr(Vz1) & Space(4) & Trim(prn_HdDt.Rows(0).Item("Ledger_Address3").ToString) & Space(35 - Len(Trim(prn_HdDt.Rows(0).Item("Ledger_Address3").ToString))) & Chr(Vz1) & Space(38) & Chr(Vz1)
                w.WriteLine(PrnTxt)
                LnCnt = LnCnt + 1
            End If
            If Trim(prn_HdDt.Rows(0).Item("Ledger_Address4").ToString) <> "" Then
                PrnTxt = Chr(Vz1) & Space(4) & Trim(prn_HdDt.Rows(0).Item("Ledger_Address4").ToString) & Space(35 - Len(Trim(prn_HdDt.Rows(0).Item("Ledger_Address4").ToString))) & Chr(Vz1) & Space(38) & Chr(Vz1)
                w.WriteLine(PrnTxt)
                LnCnt = LnCnt + 1
            End If


            'SUB HEADING

            PrnTxt = Chr(LfCon) & StrDup(6, Chr(Hz2)) & Chr(194) &
                                  StrDup(6, Chr(Hz2)) & Chr(194) &
                                  StrDup(7, Chr(Hz2)) & Chr(194) &
                                  StrDup(5, Chr(Hz2)) & Chr(194) &
                                  StrDup(7, Chr(Hz2)) & Chr(194) &
                                  StrDup(5, Chr(Hz2)) & Chr(194) &
                                  StrDup(12, Chr(Hz2)) & Chr(194) &
                                  StrDup(23, Chr(Hz2)) & Chr(RgtCon)

            'PrnTxt = Chr(LfCon) & StrDup(2, Chr(Hz2)) & Chr(194) & StrDup(5, Chr(Hz2)) & Chr(194) & StrDup(5, Chr(Hz2)) & Chr(194) & StrDup(6, Chr(Hz2)) & Chr(194) & StrDup(17, Chr(Hz2)) & Chr(CenCon) & StrDup(2, Chr(Hz2)) & Chr(194) & StrDup(5, Chr(Hz2)) & Chr(194) & StrDup(5, Chr(Hz2)) & Chr(194) & StrDup(6, Chr(Hz2)) & Chr(194) & StrDup(16, Chr(Hz2)) & Chr(RgtCon)
            w.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1

            PrnTxt = Chr(Vz1) & " S.NO " &
                     Chr(Vz2) & "SET NO" &
                     Chr(Vz2) & " ENDS  " &
                     Chr(Vz2) & "COUNT" &
                     Chr(Vz2) & " BEAM  " &
                     Chr(Vz2) & " PCS " &
                     Chr(Vz2) & "   METER    " &
                     Chr(Vz2) & "      DELIVERY TO      " & Chr(Vz1)

            'PrnTxt = Chr(Vz1) & "NO" & Chr(Vz2) & " BEAM" & Chr(Vz2) & " PCS " & Chr(Vz2) & "METERS" & Chr(Vz2) & "   DELIVERY TO   " & Chr(Vz1) & "NO" & Chr(Vz2) & " BEAM" & Chr(Vz2) & " PCS " & Chr(Vz2) & "METERS" & Chr(Vz2) & "   DELIVERY TO  " & Chr(Vz1)
            w.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1

            PrnTxt = Chr(LfCon) & StrDup(6, Chr(Hz2)) & Chr(194) &
                                  StrDup(6, Chr(Hz2)) & Chr(194) &
                                  StrDup(7, Chr(Hz2)) & Chr(194) &
                                  StrDup(5, Chr(Hz2)) & Chr(194) &
                                  StrDup(7, Chr(Hz2)) & Chr(194) &
                                  StrDup(5, Chr(Hz2)) & Chr(194) &
                                  StrDup(12, Chr(Hz2)) & Chr(194) &
                                  StrDup(23, Chr(Hz2)) & Chr(RgtCon)

            ' PrnTxt = Chr(LfCon) & StrDup(6, Chr(Hz2)) & Chr(194) & StrDup(10, Chr(Hz2)) & Chr(194) & StrDup(7, Chr(Hz2)) & Chr(194) & StrDup(7, Chr(Hz2)) & Chr(194) & StrDup(11, Chr(Hz2)) & Chr(194) & StrDup(17, Chr(Hz2)) & Chr(194) & StrDup(6, Chr(Hz2)) & Chr(194) & StrDup(7, Chr(Hz2)) & Chr(RgtCon)

            ' PrnTxt = Chr(LfCon) & StrDup(2, Chr(Hz2)) & Chr(197) & StrDup(5, Chr(Hz2)) & Chr(197) & StrDup(5, Chr(Hz2)) & Chr(197) & StrDup(6, Chr(Hz2)) & Chr(197) & StrDup(17, Chr(Hz2)) & Chr(CenCon) & StrDup(2, Chr(Hz2)) & Chr(197) & StrDup(5, Chr(Hz2)) & Chr(197) & StrDup(5, Chr(Hz2)) & Chr(197) & StrDup(6, Chr(Hz2)) & Chr(197) & StrDup(16, Chr(Hz2)) & Chr(RgtCon)
            w.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1

        Catch ex As Exception
            w.Close()
            w.Dispose()
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format3_PageFooter()
        Dim EBm_Txt As String = ""
        Dim EBm_Wdth As String = ""
        Dim Cmp_Name As String = ""

        Try

            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString


            ' PrnTxt = Chr(LfCon) & StrDup(6, Chr(Hz2)) & Chr(194) & StrDup(10, Chr(Hz2)) & Chr(194) & StrDup(7, Chr(Hz2)) & Chr(194) & StrDup(7, Chr(Hz2)) & Chr(194) & StrDup(11, Chr(Hz2)) & Chr(194) & StrDup(17, Chr(Hz2)) & Chr(194) & StrDup(6, Chr(Hz2)) & Chr(194) & StrDup(7, Chr(Hz2)) & Chr(RgtCon)

            PrnTxt = Chr(LfCon) & StrDup(6, Chr(Hz2)) & Chr(193) &
                                  StrDup(6, Chr(Hz2)) & Chr(193) &
                                  StrDup(7, Chr(Hz2)) & Chr(193) &
                                  StrDup(5, Chr(Hz2)) & Chr(193) &
                                  StrDup(7, Chr(Hz2)) & Chr(CenUp) &
                                  StrDup(5, Chr(Hz2)) & Chr(194) &
                                  StrDup(12, Chr(Hz2)) & Chr(194) &
                                  StrDup(23, Chr(Hz2)) & Chr(RgtCon)
            w.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1

            PrnTxt = Chr(Vz1) & Space(1) & "Through     : " & Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString) & Space(36 - Len(Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString))) & Space(2) & Trim(EBm_Txt) & Space(25 - Len(Trim(EBm_Txt))) & Chr(Vz1)
            w.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1
            PrnTxt = Chr(LfCon) & StrDup(78, Chr(Hz2)) & Chr(RgtCon)
            w.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1

            PrnTxt = Chr(Vz1) & Space(78) & Chr(Vz1)
            w.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1
            PrnTxt = Chr(Vz1) & Space(78) & Chr(Vz1)
            w.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1
            PrnTxt = Chr(Vz1) & " Signature of the Receiver    " & Space(43 - Len(Cmp_Name)) & "For " & Cmp_Name & Space(1) & Chr(Vz1)
            w.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1

            PrnTxt = Chr(Corn3) & StrDup(78, Chr(Hz1)) & Chr(Corn4)
            w.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1


            PrnTxt = ""
            w.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1
            PrnTxt = ""

            w.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1

            PrnTxt = ""
            w.WriteLine(PrnTxt)
            LnCnt = LnCnt + 1

            'For I = LnCnt To 36
            '    PrnTxt = ""
            '    w.WriteLine(PrnTxt)
            '    LnCnt = LnCnt + 1
            'Next


        Catch ex As Exception
            w.Close()
            w.Dispose()
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub
    Private Sub Printing_Format4(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
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
        Dim PS As Printing.PaperSize
        Dim PCnt As Integer = 0, PrntCnt As Integer = 0
        Dim TpMargin As Single = 0
        Dim Cnt As Integer = 0

        PrntCnt = 1

        If Val(Common_Procedures.settings.PavuDelivery_Print_2Copy_In_SinglePage) = 1 Then
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






        NoofItems_PerPage = 5 ' 6


        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = Val(30) : ClArr(2) = 55 : ClArr(3) = 75 : ClArr(4) = 85 : ClArr(5) = 120
        ClArr(6) = 35 : ClArr(7) = 55 : ClArr(8) = 75 : ClArr(9) = 85
        ClArr(10) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9))

        TxtHgt = 18.25 ' 18.5 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        PrntCnt2ndPageSTS = False
        TpMargin = TMargin

        For PCnt = 1 To PrntCnt
            If Val(Common_Procedures.settings.PavuDelivery_Print_2Copy_In_SinglePage) = 1 Then
                If PCnt = 1 Then
                    prn_PageNo1 = prn_PageNo
                    prn_DetIndx1 = prn_DetIndx
                    prn_DetSNo1 = prn_DetSNo
                    prn_NoofBmDets1 = prn_NoofBmDets
                    TpMargin = TMargin

                Else

                    prn_PageNo = prn_PageNo1
                    prn_NoofBmDets = prn_NoofBmDets1
                    prn_DetIndx = prn_DetIndx1
                    prn_DetSNo = prn_DetSNo1

                    TpMargin = 580 + TMargin  ' 600 + TMargin

                End If


            End If
            Try

                If prn_HdDt.Rows.Count > 0 Then

                    Printing_Format4_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TpMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                    NoofDets = 0

                    CurY = CurY - 10

                    If prn_DetDt.Rows.Count > 0 Then

                        Do While prn_NoofBmDets < prn_DetMxIndx

                            If NoofDets >= NoofItems_PerPage Then
                                If Val(Common_Procedures.settings.PavuDelivery_Print_2Copy_In_SinglePage) = 1 Then
                                    If PCnt = 2 And PrntCnt = 2 And PrntCnt2ndPageSTS = False Then
                                        CurY = CurY + TxtHgt

                                        Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                                        NoofDets = NoofDets + 1
                                        prn_NoofBmDets = prn_NoofBmDets + 1

                                        Printing_Format4_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

                                        prn_DetIndx = prn_DetIndx + NoofItems_PerPage

                                        e.HasMorePages = True

                                        Return
                                    End If
                                Else

                                    CurY = CurY + TxtHgt

                                    Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                                    NoofDets = NoofDets + 1
                                    prn_NoofBmDets = prn_NoofBmDets + 1

                                    Printing_Format4_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

                                    prn_DetIndx = prn_DetIndx + NoofItems_PerPage

                                    e.HasMorePages = True

                                    Return

                                End If
                            End If

                            prn_DetIndx = prn_DetIndx + 1


                            If PCnt <> 2 Then


                                If Val(prn_DetAr(prn_DetIndx, 4)) <> 0 Then
                                    If PCnt = 1 And NoofDets < NoofItems_PerPage Then
                                        CurY = CurY + TxtHgt
                                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx, 1))), LMargin + 10, CurY, 0, 0, pFont)
                                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 2)), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx, 3))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) - 10, CurY, 1, 0, pFont)
                                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx, 4)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 10, CurY, 1, 0, pFont)
                                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 5)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 10, CurY, 0, 0, pFont)
                                    End If
                                    prn_NoofBmDets = prn_NoofBmDets + 1

                                End If

                                If Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 4)) <> 0 Then
                                    If PCnt = 1 And NoofDets < NoofItems_PerPage Then
                                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 1))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + 12, CurY, 0, 0, pFont)
                                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 2)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + 10, CurY, 0, 0, pFont)
                                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 3))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)
                                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 4)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) - 10, CurY, 1, 0, pFont)
                                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 5)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + 10, CurY, 0, 0, pFont)
                                    End If
                                    prn_NoofBmDets = prn_NoofBmDets + 1

                                End If

                                NoofDets = NoofDets + 1
                            End If

                            If PCnt = 2 Then


                                If Val(prn_DetAr(prn_DetIndx, 4)) <> 0 Then
                                    CurY = CurY + TxtHgt
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx, 1))), LMargin + 10, CurY, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 2)), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx, 3))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) - 10, CurY, 1, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx, 4)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 10, CurY, 1, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 5)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 10, CurY, 0, 0, pFont)

                                    prn_NoofBmDets = prn_NoofBmDets + 1

                                End If

                                If Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 4)) <> 0 Then

                                    Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 1))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + 12, CurY, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 2)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + 10, CurY, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 3))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 4)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) - 10, CurY, 1, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 5)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + 10, CurY, 0, 0, pFont)

                                    prn_NoofBmDets = prn_NoofBmDets + 1

                                End If

                                NoofDets = NoofDets + 1
                            End If
                        Loop

                    End If

                    Printing_Format4_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)

                End If

            Catch ex As Exception

                MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End Try
            If Val(Common_Procedures.settings.PavuDelivery_Print_2Copy_In_SinglePage) = 1 Then

                If PCnt = 1 And PrntCnt = 2 And PrntCnt2ndPageSTS = False Then
                    If prn_DetDt.Rows.Count = Cnt + 10 Then
                        PrntCnt2ndPageSTS = True
                        e.HasMorePages = True
                        Cnt = 10
                        Return
                    End If
                End If
            End If

            PrntCnt2ndPageSTS = False

        Next PCnt
LOOP10:


        prn_Count = prn_Count + 1

        e.HasMorePages = False

        If Val(prn_TotCopies) > 1 Then
            If prn_Count < Val(prn_TotCopies) Then

                prn_DetIndx = 0
                prn_PageNo = 0
                prn_DetIndx = 0
                prn_PageNo = 0
                prn_NoofBmDets = 0


                e.HasMorePages = True
                Return

            End If

        End If

    End Sub

    Private Sub Printing_Format4_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TpMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim da3 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim p1Font As Font
        Dim vSetNo As String
        Dim vMill_Nm As String
        Dim strHeight As Single
        Dim W1 As Single, N1 As Single, M1 As Single
        Dim i As Integer, k As Integer
        Dim Arr(300, 5) As String
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_Email As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim Gst_dt As Date
        Dim Entry_dt As Date
        Dim CurX As Single = 0
        Dim strWidth As Single = 0

        PageNo = PageNo + 1

        CurY = TpMargin

        If prn_DetMxIndx > (2 * NoofItems_PerPage) Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If

        da2 = New SqlClient.SqlDataAdapter("select DISTINCT(setcode_forSelection) from Stock_SizedPavu_Processing_Details where Pavu_Delivery_Code = '" & Trim(EntryCode) & "' Order by setcode_forSelection", con)
        dt3 = New DataTable
        da2.Fill(dt3)

        vSetNo = ""
        If dt3.Rows.Count > 0 Then
            For i = 0 To dt3.Rows.Count - 1
                k = InStr(1, dt3.Rows(i).Item("setcode_forSelection").ToString, "/")
                vSetNo = vSetNo & IIf(Trim(vSetNo) <> "", ", ", "") & Microsoft.VisualBasic.Left(dt3.Rows(i).Item("setcode_forSelection").ToString, k - 1)
            Next i
        End If
        dt3.Dispose()

        da3 = New SqlClient.SqlDataAdapter("select b.mill_name from Stock_SizedPavu_Processing_Details a LEFT OUTER JOIN Mill_Head b ON a.Mill_IdNo = b.Mill_IdNo where a.Pavu_Delivery_Code = '" & Trim(EntryCode) & "'", con)
        dt4 = New DataTable
        da3.Fill(dt4)

        vMill_Nm = ""
        If dt4.Rows.Count > 0 Then
            vMill_Nm = dt4.Rows(0).Item("mill_name").ToString
        End If
        dt4.Dispose()

        CurY = TpMargin
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
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
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

        Gst_dt = #7/1/2017#
        Entry_dt = dtp_Date.Value

        If DateDiff("d", Gst_dt.ToShortDateString, Entry_dt.ToShortDateString) < 0 Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)

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
            CurY = CurY + TxtHgt

        End If


        CurY = CurY + TxtHgt - 12
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "SIZED BEAM DELIVERY NOTE", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight + 3
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        ' Try

        N1 = e.Graphics.MeasureString("TO   : ", pFont).Width
        W1 = e.Graphics.MeasureString("SET NO  :  ", pFont).Width

        M1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7)

        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 11, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, "TO :  M/s. " & prn_HdDt.Rows(0).Item("Ledger_MainName").ToString, LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "DC.NO", LMargin + M1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
        p1Font = New Font("Calibri", 14, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Pavu_Delivery_No").ToString, LMargin + M1 + W1 + 25, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "DATE : " & Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Pavu_Delivery_Date").ToString), "dd-MM-yyyy").ToString, PageWidth - 10, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "SET NO", LMargin + M1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, vSetNo, LMargin + M1 + W1 + 25, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "ENDS", LMargin + M1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ends_Name").ToString, LMargin + M1 + W1 + 25, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "COUNT", LMargin + M1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Count_Name").ToString, LMargin + M1 + W1 + 25, CurY, 0, 0, pFont)

        If DateDiff("d", Gst_dt.ToShortDateString, Entry_dt.ToShortDateString) < 0 Then

            CurY = CurY + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, " TIN NO : " & prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)
            End If

        Else

            CurY = CurY + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("ledger_GSTinNo").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, " GSTIN NO : " & prn_HdDt.Rows(0).Item("ledger_GSTinNo").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)
            End If

        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1078" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1112" Then '---- Kalaimagal Sizing (Palladam)
            Common_Procedures.Print_To_PrintDocument(e, "MILL", LMargin + M1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(vMill_Nm), LMargin + M1 + W1 + 25, CurY, 0, 0, pFont)
        End If

        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + M1, LnAr(3), LMargin + M1, LnAr(2))

        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin, CurY, 2, ClAr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "BEAM", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "PCS", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "DELIVERY TO", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 3, CurY, 2, ClAr(6), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "BEAM", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "PCS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "DELIVERY TO", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, 2, ClAr(10), pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY

        ' Catch ex As Exception

        'MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        '  End Try

    End Sub

    Private Sub Printing_Format4_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
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
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Pcs").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
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


            CurY = CurY + TxtHgt - 10

            Common_Procedures.Print_To_PrintDocument(e, "Vehicle No  :  " & Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString), LMargin + 10, CurY, 0, 0, pFont)
            If Trim(prn_HdDt.Rows(0).Item("Book_No").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "Book No  :  " & Trim(prn_HdDt.Rows(0).Item("Book_No").ToString), PageWidth - 200, CurY, 0, 0, pFont)
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
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 5, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin, LnAr(1))
            e.Graphics.DrawLine(Pens.Black, PageWidth, CurY, PageWidth, LnAr(1))

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format5(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim EntryCode As String
        Dim NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim cnt As Integer = 0
        Dim LnAr(20) As Single, ClArr(15) As Single
        'Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim PS As Printing.PaperSize
        Dim PCnt As Integer = 0, PrntCnt As Integer = 0
        Dim TpMargin As Single = 0
        Dim Cnt1 As Integer = 0


        PrntCnt = 1

        If Val(Common_Procedures.settings.PavuDelivery_Print_2Copy_In_SinglePage) = 1 Then
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

        pFont = New Font("Calibri", 10, FontStyle.Regular)

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

        NoofItems_PerPage = 11 ' 6

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1263" Then '---- Asia Sizing (Palladam)
            ClArr(1) = Val(25) : ClArr(2) = 50 : ClArr(3) = 60 : ClArr(4) = 55 : ClArr(5) = 50 : ClArr(6) = 70 : ClArr(7) = 70
            ClArr(8) = 30 : ClArr(9) = 50 : ClArr(10) = 60 : ClArr(11) = 55 : ClArr(12) = 50 : ClArr(13) = 70
            ClArr(14) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) + ClArr(11) + ClArr(12) + ClArr(13))
        Else
            ClArr(1) = Val(25) : ClArr(2) = 40 : ClArr(3) = 55 : ClArr(4) = 55 : ClArr(5) = 40 : ClArr(6) = 70 : ClArr(7) = 100
            ClArr(8) = 30 : ClArr(9) = 40 : ClArr(10) = 55 : ClArr(11) = 55 : ClArr(12) = 40 : ClArr(13) = 70
            ClArr(14) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) + ClArr(11) + ClArr(12) + ClArr(13))
        End If

        TxtHgt = 17.2 ' 18.5 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        PrntCnt2ndPageSTS = False
        TpMargin = TMargin

        For PCnt = 1 To PrntCnt
            If Val(Common_Procedures.settings.PavuDelivery_Print_2Copy_In_SinglePage) = 1 Then
                If PCnt = 1 Then
                    prn_PageNo1 = prn_PageNo

                    prn_DetIndx1 = prn_DetIndx
                    prn_DetSNo1 = prn_DetSNo
                    prn_NoofBmDets1 = prn_NoofBmDets
                    TpMargin = TMargin

                Else

                    prn_PageNo = prn_PageNo1
                    prn_NoofBmDets = prn_NoofBmDets1
                    prn_DetIndx = prn_DetIndx1
                    prn_DetSNo = prn_DetSNo1

                    TpMargin = 560 + TMargin  ' 600 + TMargin

                End If
            End If

            Try

                If prn_HdDt.Rows.Count > 0 Then

                    Printing_Format5_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TpMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                    NoofDets = 0

                    CurY = CurY - 10

                    If prn_DetDt.Rows.Count > 0 Then


                        Do While prn_NoofBmDets < prn_DetMxIndx
                            If NoofDets >= NoofItems_PerPage Then
                                If Val(Common_Procedures.settings.PavuDelivery_Print_2Copy_In_SinglePage) = 1 Then

                                    If PCnt = 2 And PrntCnt = 2 And PrntCnt2ndPageSTS = False Then


                                        CurY = CurY + TxtHgt

                                        Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                                        NoofDets = NoofDets + 1

                                        Printing_Format5_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, PCnt, False)

                                        prn_DetIndx = prn_DetIndx + NoofItems_PerPage

                                        e.HasMorePages = True

                                        Return
                                    End If

                                Else

                                    CurY = CurY + TxtHgt

                                    Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                                    NoofDets = NoofDets + 1

                                    Printing_Format5_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, PCnt, False)

                                    prn_DetIndx = prn_DetIndx + NoofItems_PerPage

                                    e.HasMorePages = True

                                    Return

                                End If
                            End If

                            prn_DetIndx = prn_DetIndx + 1

                            If PCnt <> 2 Then

                                If Val(prn_DetAr(prn_DetIndx, 4)) <> 0 Then
                                    If PCnt = 1 And NoofDets < NoofItems_PerPage Then
                                        CurY = CurY + TxtHgt
                                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx, 1))), LMargin + 3, CurY, 0, 0, pFont)
                                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 6)), LMargin + ClArr(1) + 3, CurY, 0, 0, pFont)
                                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx, 2))), LMargin + ClArr(1) + ClArr(2) + 3, CurY, 0, 0, pFont)
                                        Common_Procedures.Print_To_PrintDocument(e, Trim((prn_DetAr(prn_DetIndx, 7))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 3, CurY, 0, 0, pFont)
                                        Common_Procedures.Print_To_PrintDocument(e, Trim((prn_DetAr(prn_DetIndx, 8))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 3, CurY, 0, 0, pFont)
                                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx, 4)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 5, CurY, 1, 0, pFont)
                                        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1263" Then '---- Meenashi Sizing (Kpati)
                                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 9)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + 3, CurY, 0, 0, pFont)
                                        Else
                                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 5)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + 3, CurY, 0, 0, pFont)
                                        End If

                                    End If
                                    prn_NoofBmDets = prn_NoofBmDets + 1

                                End If

                                If Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 4)) <> 0 Then
                                    If PCnt = 1 And NoofDets < NoofItems_PerPage Then
                                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 1))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + 7, CurY, 0, 0, pFont)
                                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 6)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + 3, CurY, 0, 0, pFont)
                                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 2))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + 3, CurY, 0, 0, pFont)
                                        Common_Procedures.Print_To_PrintDocument(e, Trim((prn_DetAr(prn_DetIndx + NoofItems_PerPage, 7))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) + 3, CurY, 0, 0, pFont)
                                        Common_Procedures.Print_To_PrintDocument(e, Trim((prn_DetAr(prn_DetIndx + NoofItems_PerPage, 8))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) + ClArr(11) + 3, CurY, 0, 0, pFont)
                                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 4)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) + ClArr(11) + ClArr(12) + ClArr(13) - 5, CurY, 1, 0, pFont)
                                        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1263" Then '---- Meenashi Sizing (Kapati)
                                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 9)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) + ClArr(11) + ClArr(12) + ClArr(13) + 3, CurY, 0, 0, pFont)
                                        Else
                                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 5)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) + ClArr(11) + ClArr(12) + ClArr(13) + 3, CurY, 0, 0, pFont)
                                        End If

                                    End If

                                    prn_NoofBmDets = prn_NoofBmDets + 1

                                End If

                                NoofDets = NoofDets + 1

                            End If

                            If PCnt = 2 Then
                                If Val(prn_DetAr(prn_DetIndx, 4)) <> 0 Then
                                    CurY = CurY + TxtHgt
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx, 1))), LMargin + 3, CurY, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 6)), LMargin + ClArr(1) + 3, CurY, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx, 2))), LMargin + ClArr(1) + ClArr(2) + 3, CurY, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Trim((prn_DetAr(prn_DetIndx, 7))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 3, CurY, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Trim((prn_DetAr(prn_DetIndx, 8))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 3, CurY, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx, 4)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 3, CurY, 1, 0, pFont)
                                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1263" Then
                                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 9)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + 3, CurY, 0, 0, pFont)
                                    Else
                                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 5)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + 3, CurY, 0, 0, pFont)
                                    End If
                                    prn_NoofBmDets = prn_NoofBmDets + 1

                                End If

                                If Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 4)) <> 0 Then

                                    Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 1))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + 7, CurY, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 6)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + 3, CurY, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 2))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + 3, CurY, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Trim((prn_DetAr(prn_DetIndx + NoofItems_PerPage, 7))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) + 3, CurY, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Trim((prn_DetAr(prn_DetIndx + NoofItems_PerPage, 8))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) + ClArr(11) + 3, CurY, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 4)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) + ClArr(11) + ClArr(12) + ClArr(13) - 3, CurY, 1, 0, pFont)
                                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1263" Then
                                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 9)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) + ClArr(11) + ClArr(12) + ClArr(13) + 3, CurY, 0, 0, pFont)
                                    Else
                                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 5)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) + ClArr(11) + ClArr(12) + ClArr(13) + 3, CurY, 0, 0, pFont)
                                    End If



                                    prn_NoofBmDets = prn_NoofBmDets + 1

                                End If

                                NoofDets = NoofDets + 1
                            End If
                        Loop
                    End If

                    Printing_Format5_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, PCnt, True)

                End If

            Catch ex As Exception

                MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End Try
            If Val(Common_Procedures.settings.PavuDelivery_Print_2Copy_In_SinglePage) = 1 Then

                If PCnt = 1 And PrntCnt = 2 And PrntCnt2ndPageSTS = False Then
                    If prn_DetDt.Rows.Count = cnt + 18 Then
                        PrntCnt2ndPageSTS = True
                        e.HasMorePages = True
                        cnt = 18
                        Return
                    End If
                End If
            End If
            PrntCnt2ndPageSTS = False

        Next PCnt
LOOP10:


        prn_Count = prn_Count + 1

        e.HasMorePages = False

        If Val(prn_TotCopies) > 1 Then
            If prn_Count < Val(prn_TotCopies) Then

                prn_DetIndx = 0
                prn_PageNo = 0
                prn_DetIndx = 0
                prn_PageNo = 0
                prn_NoofBmDets = 0


                e.HasMorePages = True
                Return

            End If

        End If

    End Sub

    Private Sub Printing_Format5_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim da3 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim p1Font As Font

        Dim strHeight As Single
        Dim W1 As Single, W2 As Single, W3 As Single, N1 As Single, M1 As Single
        Dim i As Integer, k As Integer = 0
        Dim Arr(300, 5) As String
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String, Cmp_Add3 As String, Cmp_Add4 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_Email As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim Led_Name As String, Led_Add1 As String, Led_Add2 As String, Led_Add3 As String, Led_Add4 As String
        Dim Led_GstNo As String, Led_PanNo As String
        Dim Hsn_Code As String = ""
        Dim Cnt_Name As String = ""
        'Dim Entry_dt As Date
        Dim CurX As Single = 0
        Dim strWidth As Single = 0
        Dim Ledname1 As String
        Dim Ledname2 As String
        'Dim ItmNm1 As String, ItmNm2 As String

        PageNo = PageNo + 1

        CurY = TMargin

        If prn_DetMxIndx > (2 * NoofItems_PerPage) Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If

        'da2 = New SqlClient.SqlDataAdapter("select DISTINCT(setcode_forSelection) from Stock_SizedPavu_Processing_Details where Pavu_Delivery_Code = '" & Trim(EntryCode) & "' Order by setcode_forSelection", con)
        'dt3 = New DataTable
        'da2.Fill(dt3)

        'vSetNo = ""
        'If dt3.Rows.Count > 0 Then
        '    For i = 0 To dt3.Rows.Count - 1
        '        k = InStr(1, dt3.Rows(i).Item("setcode_forSelection").ToString, "/")
        '        vSetNo = vSetNo & IIf(Trim(vSetNo) <> "", ", ", "") & Microsoft.VisualBasic.Left(dt3.Rows(i).Item("setcode_forSelection").ToString, k - 1)
        '    Next i
        'End If
        'dt3.Dispose()
        Hsn_Code = ""
        Cnt_Name = dgv_Details.Rows(0).Cells(8).Value
        da3 = New SqlClient.SqlDataAdapter("select a.*, b.* from Stock_SizedPavu_Processing_Details a LEFT OUTER JOIN Count_Head b ON a.Count_IdNo = b.Count_IdNo where a.Pavu_Delivery_Code = '" & Trim(EntryCode) & "' and b.Count_Name = '" & Trim(Cnt_Name) & "' Order by a.sl_no", con)
        dt4 = New DataTable
        da3.Fill(dt4)


        If dt4.Rows.Count > 0 Then
            Hsn_Code = dt4.Rows(0).Item("HSN_Code").ToString

            'For i = 0 To dt4.Rows.Count - 1
            '    'k = InStr(1, dt4.Rows(i).Item("Count_Hsn_Code").ToString, "/")
            '    'Hsn_Code = Hsn_Code & IIf(Trim(Hsn_Code) <> "", ", ", "") & Microsoft.VisualBasic.Left(dt4.Rows(i).Item("Count_Hsn_Code").ToString, k - 1)
            '    ' Cnt_Name = dt4.Rows(i).Item("Count_Name").ToString
            '    'Hsn_Code = dt4.Rows(0).Item("Count_Hsn_Code").ToString
            'Next
        End If
        dt4.Clear()
        dt4.Dispose()

        CurY = TMargin
        CurY = CurY + 5
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "SIZED PAVU DELIVERY NOTE", LMargin, CurY - TxtHgt, 2, PrintWidth, p1Font)
        ' strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = "" : Cmp_Add3 = "" : Cmp_Add4 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_Email = ""
        Cmp_StateCap = "" : Cmp_StateNm = "" : Cmp_StateCode = "" : Cmp_GSTIN_Cap = "" : Cmp_GSTIN_No = ""

        Led_Name = "" : Led_Add1 = "" : Led_Add2 = "" : Led_Add3 = "" : Led_Add4 = "" : Led_GstNo = ""

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString

        If Common_Procedures.settings.CustomerCode = "1078" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then
            Cmp_Add1 = prn_HdDt.Rows(0).Item("Sizing_Address1").ToString
            Cmp_Add2 = prn_HdDt.Rows(0).Item("Sizing_Address2").ToString
            Cmp_Add3 = prn_HdDt.Rows(0).Item("Sizing_Address3").ToString
            Cmp_Add4 = prn_HdDt.Rows(0).Item("Sizing_Address4").ToString

            If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
                Cmp_PhNo = "PHONE NO.:" & prn_HdDt.Rows(0).Item("Sizing_PhoneNo").ToString
            End If
            If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
                Cmp_Email = "E-mail : " & Trim(prn_HdDt.Rows(0).Item("Sizing_EMail").ToString)
            End If

        Else

            Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString
            Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address2").ToString
            Cmp_Add3 = prn_HdDt.Rows(0).Item("Company_Address3").ToString
            Cmp_Add4 = prn_HdDt.Rows(0).Item("Company_Address4").ToString

            If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
                Cmp_PhNo = "PHONE NO.:" & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
            End If
            If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
                Cmp_Email = "E-mail : " & Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString)
            End If


        End If



        If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
            Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_panNo").ToString) <> "" Then
            Cmp_CstNo = "PAN NO.: " & prn_HdDt.Rows(0).Item("Company_panNo").ToString
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
            Cmp_GSTIN_No = "GSTIN" & prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
        End If
        If Trim(UCase(Common_Procedures.User.Type)) = "UNACCOUNT" And (Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1263") Then
            Led_Name = prn_HdDt.Rows(0).Item("Ledger_MainName").ToString : Led_Add1 = "" : Led_Add2 = "" : Led_Add3 = "" : Led_Add4 = "" : Led_GstNo = ""
        Else
            Led_Name = prn_HdDt.Rows(0).Item("Ledger_MainName").ToString
            Led_Add1 = prn_HdDt.Rows(0).Item("Ledger_Address1").ToString
            Led_Add2 = prn_HdDt.Rows(0).Item("Ledger_Address2").ToString
            Led_Add3 = prn_HdDt.Rows(0).Item("Ledger_Address3").ToString & "," & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString
            Led_Add4 = prn_HdDt.Rows(0).Item("Ledger_Address4").ToString

            If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then

                Led_GstNo = "GSTIN" & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString

                Led_PanNo = "PAN NO : " & prn_HdDt.Rows(0).Item("PAN_no").ToString
            End If
        End If
        CurY = CurY + TxtHgt - 10

        M1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7)

        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin + 10, CurY, 0, 0, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height
        Common_Procedures.Print_To_PrintDocument(e, "PAVU SIZED TO :", LMargin + M1 + 10, CurY, 0, 0, pFont)
        Dim cury2 As Single
        cury2 = CurY
        ' p1Font = New Font("Calibri", 9, FontStyle.Regular)

        'If Trim(prn_HdDt.Rows(0).Item("Company_Name").ToString) <> "" Then
        '    CmpName1 = Trim(prn_HdDt.Rows(0).Item("Company_Name").ToString)
        'Else
        Ledname1 = Trim(prn_HdDt.Rows(0).Item("Ledger_MainName").ToString)
        '  End If

        Ledname2 = ""

        If Len(Ledname1) > 40 Then
            For i = 40 To 1 Step -1
                If Mid$(Trim(Ledname1), i, 1) = " " Or Mid$(Trim(Ledname1), i, 1) = "," Or Mid$(Trim(Ledname1), i, 1) = "." Or Mid$(Trim(Ledname1), i, 1) = "-" Or Mid$(Trim(Ledname1), i, 1) = "/" Or Mid$(Trim(Ledname1), i, 1) = "_" Or Mid$(Trim(Ledname1), i, 1) = "(" Or Mid$(Trim(Ledname1), i, 1) = ")" Or Mid$(Trim(Ledname1), i, 1) = "\" Or Mid$(Trim(Ledname1), i, 1) = "[" Or Mid$(Trim(Ledname1), i, 1) = "]" Or Mid$(Trim(Ledname1), i, 1) = "{" Or Mid$(Trim(Ledname1), i, 1) = "}" Then Exit For
            Next i
            If i = 0 Then i = 40
            Ledname2 = Microsoft.VisualBasic.Right(Trim(Ledname1), Len(Ledname1) - i)
            Ledname1 = Microsoft.VisualBasic.Left(Trim(Ledname1), i - 1)
        End If


        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin + 10, CurY, 0, 0, pFont)

        cury2 = cury2 + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "M/s." & Ledname1, LMargin + M1 + 10, cury2, 0, 0, p1Font)

        If Trim(Ledname2) <> "" Then
            cury2 = cury2 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(Ledname2), LMargin + M1 + 10, cury2, 0, 0, p1Font)
            'NoofDets = NoofDets + 1
        End If

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin + 10, CurY, 0, 0, pFont)
        cury2 = cury2 + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Led_Add1, LMargin + M1 + 10, cury2, 0, 0, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add3, LMargin + 10, CurY, 0, 0, pFont)
        cury2 = cury2 + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Led_Add2, LMargin + M1 + 10, cury2, 0, 0, pFont)
        If Trim(Cmp_Add4) <> "" Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Add4, LMargin + 10, CurY, 0, 0, pFont)
        End If
        If Trim(Led_Add3) <> "" Then
            cury2 = cury2 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Led_Add3, LMargin + M1 + 10, cury2, 0, 0, pFont)
        End If
        'CurY = CurY + TxtHgt
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, Led_Add4, LMargin + M1 + 10, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No & "    " & Cmp_CstNo, LMargin + 10, CurY, 0, 0, pFont)
        cury2 = cury2 + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Led_GstNo & "  " & Led_PanNo, LMargin + M1 + 10, cury2, 0, 0, pFont)
        If CurY > cury2 Then
            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(2) = CurY
        Else
            cury2 = cury2 + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, cury2, PageWidth, cury2)
            LnAr(2) = cury2
        End If

        'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        'LnAr(2) = CurY
        e.Graphics.DrawLine(Pens.Black, LMargin + M1, LnAr(2), LMargin + M1, LnAr(1))


        ' CurY = CurY + TxtHgt - 12



        ' Try

        N1 = e.Graphics.MeasureString("DATE & TIME  :", pFont).Width
        W1 = e.Graphics.MeasureString("HSC CODE  :", pFont).Width
        W2 = e.Graphics.MeasureString("VAN NO   :", pFont).Width
        W3 = e.Graphics.MeasureString("APPROX VALUE  :", pFont).Width

        M1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7)

        CurY = cury2

        CurY = CurY + TxtHgt - 11
        p1Font = New Font("Calibri", 10, FontStyle.Bold)


        Common_Procedures.Print_To_PrintDocument(e, "DC.NO", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
        p1Font = New Font("Calibri", 10, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Pavu_Delivery_No").ToString, LMargin + W1 + 25, CurY, 0, 0, p1Font)
        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1078" Then '---- Kalaimagal Sizing (Palladam)
        Common_Procedures.Print_To_PrintDocument(e, "DATE & TIME", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + (ClAr(4) \ 2) + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + (ClAr(4) \ 2) + N1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Pavu_Delivery_Date").ToString), "dd-MM-yyyy") & " & " & (prn_HdDt.Rows(0).Item("Date_And_Time_Of_Supply").ToString).ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + (ClAr(4) \ 2) + N1 + 20, CurY, 0, 0, pFont)
        'End If
        Common_Procedures.Print_To_PrintDocument(e, "VAN NO", LMargin + M1 + 20, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vehicle_No").ToString, LMargin + M1 + W1 + 25, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, "APPROX VALUE", LMargin + M1 + ClAr(6) + ClAr(7) + ClAr(8) + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + ClAr(6) + ClAr(7) + ClAr(8) + W3 + 15, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Approx_Value").ToString, LMargin + M1 + ClAr(6) + ClAr(7) + ClAr(8) + W3 + 25, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt

        Common_Procedures.Print_To_PrintDocument(e, "HSN CODE", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, (Hsn_Code), LMargin + W1 + 25, CurY, 0, 0, pFont)

        p1Font = New Font("Calibri", 10, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "SAC CODE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + (ClAr(4) \ 2) + 20, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + (ClAr(4) \ 2) + N1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, " 998821", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + (ClAr(4) \ 2) + N1 + 20, CurY, 0, 0, p1Font)

        Common_Procedures.Print_To_PrintDocument(e, "EwayBill", LMargin + M1 + 20, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Electronic_Reference_No").ToString, LMargin + M1 + W2 + 25, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, "TRANSPORT", LMargin + M1 + ClAr(6) + ClAr(7) + ClAr(8) + 15, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + ClAr(6) + ClAr(7) + ClAr(8) + W3 + 15, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Transport_Name").ToString, LMargin + M1 + ClAr(6) + ClAr(7) + ClAr(8) + W3 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt + 2
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        pFont = New Font("Calibri", 9, FontStyle.Regular)
        CurY = CurY + TxtHgt - 15
        Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin, CurY, 2, ClAr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "SET NO", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "BEAM NO", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "ENDS", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "COUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 3, CurY, 2, ClAr(6), pFont)
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1263" Then
            Common_Procedures.Print_To_PrintDocument(e, "MARK", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
        Else
            Common_Procedures.Print_To_PrintDocument(e, "DELIVERY PARTY", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
        End If

        Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 3, CurY, 2, ClAr(8), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "SET NO", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "BEAM NO", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, 2, ClAr(10), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "ENDS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, 2, ClAr(11), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "COUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), CurY, 2, ClAr(12), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), CurY, 2, ClAr(13), pFont)
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1263" Then
            Common_Procedures.Print_To_PrintDocument(e, "MARK", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13), CurY, 2, ClAr(14), pFont)
        Else
            Common_Procedures.Print_To_PrintDocument(e, "DELIVERY PARTY", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13), CurY, 2, ClAr(14), pFont)
        End If

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY
        pFont = New Font("Calibri", 10, FontStyle.Regular)
        ' Catch ex As Exception

        'MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        '  End Try

    End Sub

    Private Sub Printing_Format5_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal pcnt As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim Cmp_Name As String
        Dim I As Integer
        Dim M1 As Single
        Dim Del_Add1 As String = "", Del_Add2 As String = ""
        Dim cnt2 As Integer = 0
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        Dim vSgst_amt As String = 0
        Dim vCgst_amt As String = 0

        Try

            For I = NoofDets + 1 To NoofItems_PerPage
                CurY = CurY + TxtHgt
            Next

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

            M1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7)
            CurY = CurY + TxtHgt - 15

            If is_LastPage = True Then

                If (prn_DetMxIndx Mod (NoofItems_PerPage * 2)) <= NoofItems_PerPage Then

                    If Val(prn_HdDt.Rows(0).Item("Total_Beam").ToString) <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Beam").ToString), LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
                    End If

                    If Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString) <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 5, CurY, 1, 0, pFont)
                    End If

                Else

                    If Val(prn_HdDt.Rows(0).Item("Total_Beam").ToString) <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Beam").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + 10, CurY, 0, 0, pFont)
                    End If


                    If Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString) <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) - 5, CurY, 1, 0, pFont)
                    End If

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
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 4, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 4, LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13), LnAr(3))
            CurY = CurY + TxtHgt - 15

            If Val(prn_HdDt.Rows(0).Item("Rate").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, " Rate/Meters : " & Trim(prn_HdDt.Rows(0).Item("Rate").ToString), PageWidth - 510, CurY, 0, 0, pFont)
            End If

            pFont = New Font("Calibri", 9, FontStyle.Regular)

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1263" Then '---- Sri Meenakshi Sizing (Somanur)
                p1Font = New Font("Calibri", 9, FontStyle.Bold)
                If Common_Procedures.Vendor_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("Vendor_IdNo").ToString)) <> "" Then

                    da1 = New SqlClient.SqlDataAdapter("select a.* from Vendor_head a where  a.Vendor_IdNO = " & Val(prn_HdDt.Rows(0).Item("Vendor_IdNo").ToString) & " ", con)
                    dt = New DataTable
                    da1.Fill(dt)

                    If dt.Rows.Count > 0 Then

                        Del_Add1 = dt.Rows(0).Item("Vendor_Address1").ToString & "," & dt.Rows(0).Item("Vendor_Address2").ToString
                        Del_Add2 = dt.Rows(0).Item("Vendor_Address3").ToString & "," & dt.Rows(0).Item("Vendor_Address4").ToString

                    End If
                    Common_Procedures.Print_To_PrintDocument(e, "DELIVERY TO   : " & Common_Procedures.Vendor_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("Vendor_IdNo").ToString)), LMargin + 10, CurY, 0, 0, p1Font)
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, " " & Trim(Del_Add1), LMargin + 30, CurY, 0, 0, pFont)
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, " " & Trim(Del_Add2), LMargin + 30, CurY, 0, 0, pFont)

                Else

                    'Led_Name = prn_HdDt.Rows(0).Item("Ledger_MainName").ToString
                    'Led_Add1 = prn_HdDt.Rows(0).Item("Ledger_Address1").ToString
                    'Led_Add2 = prn_HdDt.Rows(0).Item("Ledger_Address2").ToString
                    'Led_Add3 = prn_HdDt.Rows(0).Item("Ledger_Address3").ToString & "," & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString
                    'Led_Add4 = prn_HdDt.Rows(0).Item("Ledger_Address4").ToString

                    Del_Add1 = prn_HdDt.Rows(0).Item("Ledger_Address1").ToString & "," & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString
                    Del_Add2 = prn_HdDt.Rows(0).Item("Ledger_Address3").ToString & "," & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString

                    Common_Procedures.Print_To_PrintDocument(e, "DELIVERY TO   : " & prn_HdDt.Rows(0).Item("Ledger_MainName").ToString, LMargin + 10, CurY, 0, 0, pFont)
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, " " & Trim(Del_Add1), LMargin + 30, CurY, 0, 0, pFont)
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, " " & Trim(Del_Add2), LMargin + 30, CurY, 0, 0, pFont)

                End If

            Else

                Del_Add1 = prn_HdDt.Rows(0).Item("DelAdd1").ToString & "," & prn_HdDt.Rows(0).Item("DelAdd2").ToString
                Del_Add2 = prn_HdDt.Rows(0).Item("DelAdd3").ToString & "," & prn_HdDt.Rows(0).Item("DelAdd4").ToString

                Common_Procedures.Print_To_PrintDocument(e, "DELIVERY TO   : " & prn_HdDt.Rows(0).Item("DelName").ToString, LMargin + 10, CurY, 0, 0, pFont)
                'If Common_Procedures.settings.CustomerCode = "1112" Then
                '    p1Font = New Font("Calibri", 10, FontStyle.Bold)
                '    Common_Procedures.Print_To_PrintDocument(e, "For Jobwork Only, Not For Sale", PageWidth - 200, CurY, 0, 0, p1Font)
                'End If

                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, " " & Trim(Del_Add1), LMargin + 30, CurY, 0, 0, pFont)
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, " " & Trim(Del_Add2), LMargin + 30, CurY, 0, 0, pFont)

            End If
            Dim vTxamt As String = 0
            Dim vNtAMt As String = 0
            '----------------
            If Val(prn_HdDt.Rows(0).Item("approx_Value").ToString) <> 0 Then

                vCgst_amt = Format((Val(prn_HdDt.Rows(0).Item("approx_Value").ToString) * 2.5 / 100), "############0")
                vSgst_amt = Format((Val(prn_HdDt.Rows(0).Item("approx_Value").ToString) * 2.5 / 100), "############0")


                Common_Procedures.Print_To_PrintDocument(e, " CGST 2.5 % : " & vCgst_amt, PageWidth - 530, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " SGST 2.5 % : " & vSgst_amt, PageWidth - 400, CurY, 0, 0, pFont)
            End If

            If Val(prn_HdDt.Rows(0).Item("approx_Value").ToString) <> 0 Then
                vTxamt = Val(vCgst_amt) + Val(vSgst_amt) 'Format((Val(prn_HdDt.Rows(0).Item("approx_Value").ToString) * 5 / 100), "############0.00")
                Common_Procedures.Print_To_PrintDocument(e, " Tax Amount : " & vTxamt, PageWidth - 280, CurY, 0, 0, pFont)
            End If

            If Val(vTxamt) <> 0 Then
                vNtAMt = Format(Val(prn_HdDt.Rows(0).Item("approx_Value").ToString) + vTxamt, "###########0.00")
                Common_Procedures.Print_To_PrintDocument(e, " Net Amount : " & vNtAMt, PageWidth - 150, CurY, 0, 0, pFont)
            End If

            '--------------

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "This is to certify that there is no sale indicated in this delivery and the pavu sized is returned back to party after warping and sizing job work.", LMargin + 10, CurY, 0, 0, pFont)

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

            Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 20, CurY, 0, 0, pFont)
            '  Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 250, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 5, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt + 5

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin, LnAr(1))
            e.Graphics.DrawLine(Pens.Black, PageWidth, CurY, PageWidth, LnAr(1))

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Cbo_DelTo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_DeliveryTo.GotFocus

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Sales_DeliveryAddress_Head", "Party_Name", "", "(Party_IdNo = 0)")
    End Sub

    Private Sub Cbo_DelTo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DeliveryTo.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_DeliveryTo, cbo_Delivered, cbo_VehicleNo, "Sales_DeliveryAddress_Head", "Party_Name", "", "(Party_IdNo = 0)")
    End Sub

    Private Sub Cbo_DelTo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_DeliveryTo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_DeliveryTo, cbo_VehicleNo, "Sales_DeliveryAddress_Head", "Party_Name", "", "(Party_IdNo = 0)")
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



    Private Sub btn_SMS_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_SMS.Click
        Send_SMS()

    End Sub

    Private Sub Printing_Format6(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim EntryCode As String
        Dim NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim cnt As Integer = 0
        Dim LnAr(20) As Single, ClArr(15) As Single
        'Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim PS As Printing.PaperSize
        Dim PCnt As Integer = 0, PrntCnt As Integer = 0
        Dim TpMargin As Single = 0
        Dim Cnt1 As Integer = 0

        PrntCnt = 1
        If Val(Common_Procedures.settings.PavuDelivery_Print_2Copy_In_SinglePage) = 1 Then
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

        pFont = New Font("Calibri", 7.5, FontStyle.Regular)

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






        NoofItems_PerPage = 9 ' 6


        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = Val(25) : ClArr(2) = 40 : ClArr(3) = 55 : ClArr(4) = 55 : ClArr(5) = 40 : ClArr(6) = 50 : ClArr(7) = 120
        ClArr(8) = 30 : ClArr(9) = 40 : ClArr(10) = 55 : ClArr(11) = 55 : ClArr(12) = 40 : ClArr(13) = 50
        ClArr(14) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) + ClArr(11) + ClArr(12) + ClArr(13))

        TxtHgt = 17 ' 17.2 ' 18.5 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        PrntCnt2ndPageSTS = False
        TpMargin = TMargin

        For PCnt = 1 To PrntCnt
            If Val(Common_Procedures.settings.PavuDelivery_Print_2Copy_In_SinglePage) = 1 Then
                If PCnt = 1 Then
                    prn_PageNo1 = prn_PageNo

                    prn_DetIndx1 = prn_DetIndx
                    prn_DetSNo1 = prn_DetSNo
                    prn_NoofBmDets1 = prn_NoofBmDets
                    TpMargin = TMargin


                Else

                    prn_PageNo = prn_PageNo1
                    prn_NoofBmDets = prn_NoofBmDets1
                    prn_DetIndx = prn_DetIndx1
                    prn_DetSNo = prn_DetSNo1

                    TpMargin = 580 + TMargin  ' 600 + TMargin

                End If
            End If
            Try

                If prn_HdDt.Rows.Count > 0 Then

                    Printing_Format6_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TpMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                    NoofDets = 0

                    CurY = CurY - 10

                    If prn_DetDt.Rows.Count > 0 Then


                        Do While prn_NoofBmDets < prn_DetMxIndx
                            If NoofDets >= NoofItems_PerPage Then
                                If Val(Common_Procedures.settings.PavuDelivery_Print_2Copy_In_SinglePage) = 1 Then

                                    If PCnt = 2 And PrntCnt = 2 And PrntCnt2ndPageSTS = False Then


                                        CurY = CurY + TxtHgt

                                        Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                                        NoofDets = NoofDets + 1

                                        Printing_Format6_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, PCnt, False)

                                        prn_DetIndx = prn_DetIndx + NoofItems_PerPage

                                        e.HasMorePages = True

                                        Return
                                    End If

                                Else

                                    CurY = CurY + TxtHgt

                                    Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                                    NoofDets = NoofDets + 1

                                    Printing_Format6_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, PCnt, False)

                                    prn_DetIndx = prn_DetIndx + NoofItems_PerPage

                                    e.HasMorePages = True

                                    Return

                                End If
                            End If




                            prn_DetIndx = prn_DetIndx + 1

                            If PCnt <> 2 Then

                                If Val(prn_DetAr(prn_DetIndx, 4)) <> 0 Then
                                    If PCnt = 1 And NoofDets < NoofItems_PerPage Then
                                        CurY = CurY + TxtHgt
                                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx, 1))), LMargin + 10, CurY, 0, 0, pFont)
                                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 6)), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx, 2))), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                                        Common_Procedures.Print_To_PrintDocument(e, Trim((prn_DetAr(prn_DetIndx, 7))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)
                                        Common_Procedures.Print_To_PrintDocument(e, Trim((prn_DetAr(prn_DetIndx, 8))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 10, CurY, 0, 0, pFont)
                                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx, 4)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 5, CurY, 1, 0, pFont)
                                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 5)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + 10, CurY, 0, 0, pFont)
                                    End If
                                    prn_NoofBmDets = prn_NoofBmDets + 1

                                End If

                                If Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 4)) <> 0 Then
                                    If PCnt = 1 And NoofDets < NoofItems_PerPage Then
                                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 1))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + 12, CurY, 0, 0, pFont)
                                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 6)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + 10, CurY, 0, 0, pFont)
                                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 2))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + 10, CurY, 0, 0, pFont)
                                        Common_Procedures.Print_To_PrintDocument(e, Trim((prn_DetAr(prn_DetIndx + NoofItems_PerPage, 7))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) + 10, CurY, 0, 0, pFont)
                                        Common_Procedures.Print_To_PrintDocument(e, Trim((prn_DetAr(prn_DetIndx + NoofItems_PerPage, 8))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) + ClArr(11) + 10, CurY, 0, 0, pFont)
                                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 4)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) + ClArr(11) + ClArr(12) + ClArr(13) - 5, CurY, 1, 0, pFont)
                                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 5)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) + ClArr(11) + ClArr(12) + ClArr(13) + 10, CurY, 0, 0, pFont)
                                    End If
                                    prn_NoofBmDets = prn_NoofBmDets + 1

                                End If

                                NoofDets = NoofDets + 1
                            End If

                            If PCnt = 2 Then
                                If Val(prn_DetAr(prn_DetIndx, 4)) <> 0 Then
                                    CurY = CurY + TxtHgt
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx, 1))), LMargin + 10, CurY, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 6)), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx, 2))), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Trim((prn_DetAr(prn_DetIndx, 7))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Trim((prn_DetAr(prn_DetIndx, 8))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 10, CurY, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx, 4)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 5)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + 10, CurY, 0, 0, pFont)

                                    prn_NoofBmDets = prn_NoofBmDets + 1

                                End If

                                If Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 4)) <> 0 Then

                                    Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 1))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + 12, CurY, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 6)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + 10, CurY, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 2))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + 10, CurY, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Trim((prn_DetAr(prn_DetIndx + NoofItems_PerPage, 7))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) + 10, CurY, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Trim((prn_DetAr(prn_DetIndx + NoofItems_PerPage, 8))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) + ClArr(11) + 10, CurY, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 4)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) + ClArr(11) + ClArr(12) + ClArr(13) - 10, CurY, 1, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 5)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) + ClArr(11) + ClArr(12) + ClArr(13) + 10, CurY, 0, 0, pFont)

                                    prn_NoofBmDets = prn_NoofBmDets + 1

                                End If

                                NoofDets = NoofDets + 1
                            End If


                        Loop


                    End If

                    Printing_Format6_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, PCnt, True)

                End If

            Catch ex As Exception

                MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End Try
            If Val(Common_Procedures.settings.PavuDelivery_Print_2Copy_In_SinglePage) = 1 Then

                If PCnt = 1 And PrntCnt = 2 And PrntCnt2ndPageSTS = False Then
                    If prn_DetDt.Rows.Count > 18 Then
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
                prn_NoofBmDets = 0


                e.HasMorePages = True
                Return

            End If

        End If

    End Sub

    Private Sub Printing_Format6_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim da3 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim p1Font As Font

        Dim strHeight As Single
        Dim W1 As Single, N1 As Single, M1 As Single, W2 As Single
        Dim i As Integer
        Dim Arr(300, 5) As String
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String, Cmp_Add3 As String, Cmp_Add4 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_Email As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim Led_Name As String, Led_Add1 As String, Led_Add2 As String, Led_Add3 As String, Led_Add4 As String
        Dim Led_GstNo As String
        Dim Hsn_Code As String = ""
        Dim Cnt_Name As String = ""
        Dim CurX As Single = 0
        Dim strWidth As Single = 0

        PageNo = PageNo + 1

        CurY = TMargin

        If prn_DetMxIndx > (2 * NoofItems_PerPage) Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If

        'da2 = New SqlClient.SqlDataAdapter("select DISTINCT(setcode_forSelection) from Stock_SizedPavu_Processing_Details where Pavu_Delivery_Code = '" & Trim(EntryCode) & "' Order by setcode_forSelection", con)
        'dt3 = New DataTable
        'da2.Fill(dt3)

        'vSetNo = ""
        'If dt3.Rows.Count > 0 Then
        '    For i = 0 To dt3.Rows.Count - 1
        '        k = InStr(1, dt3.Rows(i).Item("setcode_forSelection").ToString, "/")
        '        vSetNo = vSetNo & IIf(Trim(vSetNo) <> "", ", ", "") & Microsoft.VisualBasic.Left(dt3.Rows(i).Item("setcode_forSelection").ToString, k - 1)
        '    Next i
        'End If
        'dt3.Dispose()
        Hsn_Code = ""
        Cnt_Name = dgv_Details.Rows(0).Cells(8).Value
        da3 = New SqlClient.SqlDataAdapter("select a.*, b.* from Stock_SizedPavu_Processing_Details a LEFT OUTER JOIN Count_Head b ON a.Count_IdNo = b.Count_IdNo where a.Pavu_Delivery_Code = '" & Trim(EntryCode) & "' and b.Count_Name = '" & Trim(Cnt_Name) & "' Order by a.sl_no", con)
        dt4 = New DataTable
        da3.Fill(dt4)


        If dt4.Rows.Count > 0 Then
            For i = 0 To dt4.Rows.Count - 1
                'k = InStr(1, dt4.Rows(i).Item("Count_Hsn_Code").ToString, "/")
                'Hsn_Code = Hsn_Code & IIf(Trim(Hsn_Code) <> "", ", ", "") & Microsoft.VisualBasic.Left(dt4.Rows(i).Item("Count_Hsn_Code").ToString, k - 1)
                ' Cnt_Name = dt4.Rows(i).Item("Count_Name").ToString
                Hsn_Code = dt4.Rows(0).Item("Count_Hsn_Code").ToString
            Next
        End If
        dt4.Clear()
        dt4.Dispose()

        CurY = TMargin
        CurY = CurY + 5
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "SIZED PAVU DELIVERY NOTE", LMargin, CurY - TxtHgt, 2, PrintWidth, p1Font)
        ' strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height
        CurY = CurY + 3

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
        Led_Add3 = prn_HdDt.Rows(0).Item("Ledger_Address3").ToString & "," & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString
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
            Cmp_GSTIN_No = "GSTIN -: " & prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then

            Led_GstNo = "GSTIN-: " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString
        End If

        CurY = CurY + TxtHgt - 10
        M1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 60
        If (Common_Procedures.settings.CustomerCode) = "1282" Then
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Else
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
        End If
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin + 10, CurY, 0, 0, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height
        Common_Procedures.Print_To_PrintDocument(e, "PAVU SIZED TO :", LMargin + M1 + 10, CurY, 0, 0, pFont)

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
        'CurY = CurY + TxtHgt
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, Led_Add4, LMargin + M1 + 10, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Led_GstNo, LMargin + M1 + 10, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY
        e.Graphics.DrawLine(Pens.Black, LMargin + M1, LnAr(2), LMargin + M1, LnAr(1))


        ' CurY = CurY + TxtHgt - 12



        ' Try

        N1 = e.Graphics.MeasureString("DATE & TIME : ", pFont).Width
        W1 = e.Graphics.MeasureString("SET NO    :  ", pFont).Width
        W2 = e.Graphics.MeasureString("APPROX VALUE :  ", pFont).Width
        ' M1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5)

        CurY = CurY + TxtHgt - 8
        p1Font = New Font("Calibri", 8, FontStyle.Bold)


        Common_Procedures.Print_To_PrintDocument(e, "DC.NO", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
        p1Font = New Font("Calibri", 9, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Pavu_Delivery_No").ToString, LMargin + W1 + 25, CurY, 0, 0, p1Font)

        Common_Procedures.Print_To_PrintDocument(e, "DATE & TIME", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + N1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, (prn_HdDt.Rows(0).Item("Date_And_Time_Of_Supply").ToString).ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + N1 + 25, CurY, 0, 0, pFont)



        Common_Procedures.Print_To_PrintDocument(e, "VAN NO", LMargin + M1, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vehicle_No").ToString, LMargin + M1 + W1 + 15, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, "APPROX VALUE", LMargin + M1 + ClAr(6) + ClAr(7) - 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + ClAr(6) + ClAr(7) - 10 + W2, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Approx_Value").ToString, LMargin + M1 + ClAr(6) + ClAr(7) - 10 + W2 + 15, CurY, 0, 0, pFont)



        CurY = CurY + TxtHgt

        Common_Procedures.Print_To_PrintDocument(e, "HSN CODE", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, (Hsn_Code), LMargin + W1 + 25, CurY, 0, 0, pFont)

        p1Font = New Font("Calibri", 8, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "SAC CODE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + N1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, " 998821", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + N1 + 25, CurY, 0, 0, p1Font)


        Common_Procedures.Print_To_PrintDocument(e, "E.REF", LMargin + M1, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Electronic_Reference_No").ToString, LMargin + M1 + W1 + 15, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, "TRANSPORT", LMargin + M1 + ClAr(6) + ClAr(7) - 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + ClAr(6) + ClAr(7) - 10 + W2, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Transport_Name").ToString, LMargin + M1 + ClAr(6) + ClAr(7) - 10 + W2 + 15, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY


        CurY = CurY + TxtHgt - 15
        Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin, CurY, 2, ClAr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "SET NO", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "BEAM NO", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "ENDS", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "COUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 3, CurY, 2, ClAr(6), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "DELIVERY PARTY", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 3, CurY, 2, ClAr(8), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "SET NO", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "BEAM NO", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, 2, ClAr(10), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "ENDS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, 2, ClAr(11), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "COUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), CurY, 2, ClAr(12), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), CurY, 2, ClAr(13), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "DELIVERY PARTY", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13), CurY, 2, ClAr(14), pFont)
        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY

        ' Catch ex As Exception

        'MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        '  End Try

    End Sub

    Private Sub Printing_Format6_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal pcnt As Integer, ByVal is_LastPage As Boolean)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim p1Font As Font
        Dim Cmp_Name As String
        Dim I As Integer
        Dim M1 As Single
        Dim Del_Add1 As String = "", Del_Add2 As String = ""
        Dim cnt2 As Integer = 0
        Dim Del_GSTIN As String = ""

        Try

            For I = NoofDets + 1 To NoofItems_PerPage
                CurY = CurY + TxtHgt
            Next

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

            M1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7)
            CurY = CurY + TxtHgt - 15

            If is_LastPage = True Then

                If prn_DetMxIndx <= 9 Then


                    If Val(prn_HdDt.Rows(0).Item("Total_Beam").ToString) <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Beam").ToString), LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
                    End If



                    If Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString) <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 5, CurY, 1, 0, pFont)
                    End If

                Else

                    If Val(prn_HdDt.Rows(0).Item("Total_Beam").ToString) <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Beam").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + 10, CurY, 0, 0, pFont)
                    End If


                    If Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString) <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) - 5, CurY, 1, 0, pFont)
                    End If

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
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 4, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 4, LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13), LnAr(3))
            CurY = CurY + TxtHgt - 15

            Del_Add1 = prn_HdDt.Rows(0).Item("DelAdd1").ToString & " " & prn_HdDt.Rows(0).Item("DelAdd2").ToString
            Del_Add2 = prn_HdDt.Rows(0).Item("DelAdd3").ToString & " " & prn_HdDt.Rows(0).Item("DelAdd4").ToString
            Del_GSTIN = ""

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Or Trim(Common_Procedures.settings.CustomerCode) = "1282" Or Trim(Common_Procedures.settings.CustomerCode) = "1288" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1263" Then '---- Sri Meenakshi Sizing (Somanur)
                If Common_Procedures.Vendor_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("Vendor_IdNo").ToString)) <> "" Then
                    da1 = New SqlClient.SqlDataAdapter("select a.* from Vendor_head a where  a.Vendor_IdNO = " & Val(prn_HdDt.Rows(0).Item("Vendor_IdNo").ToString) & " ", con)
                    dt = New DataTable
                    da1.Fill(dt)
                    If dt.Rows.Count > 0 Then
                        Del_Add1 = dt.Rows(0).Item("Vendor_Address1").ToString & "," & dt.Rows(0).Item("Vendor_Address2").ToString
                        Del_Add2 = dt.Rows(0).Item("Vendor_Address3").ToString & "," & dt.Rows(0).Item("Vendor_Address4").ToString
                        If Trim(dt.Rows(0).Item("GST_No").ToString) <> "" Then Del_GSTIN = "GSTIN : " & dt.Rows(0).Item("GST_No").ToString
                    End If
                    dt.Clear()
                End If
            End If



            Common_Procedures.Print_To_PrintDocument(e, "DELIVERY TO   : " & prn_HdDt.Rows(0).Item("DelName").ToString, LMargin + 10, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & Trim(Del_Add1), LMargin + 30, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & Trim(Del_Add2), LMargin + 30, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & Trim(Del_GSTIN), LMargin + 30, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "This is to certify that there is no sale indicated in this delivery and the pavu sized is returned back to party after warping and sizing job work.", LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(7) = CurY

            CurY = CurY + TxtHgt
            'If Val(Common_Procedures.User.IdNo) <> 1 Then
            '    Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
            'End If
            CurY = CurY + TxtHgt

            If Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString) <> "" Then
                Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
                Cmp_Name = Replace(Cmp_Name, Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString), "")

            Else
                Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString

            End If

            Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 20, CurY, 0, 0, pFont)
            '  Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 250, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 5, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt + 5

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin, LnAr(1))
            e.Graphics.DrawLine(Pens.Black, PageWidth, CurY, PageWidth, LnAr(1))

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_VendorName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_VendorName.GotFocus

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1417" Then
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'WEAVER'  or Ledger_Type = 'GODOWN'  or Ledger_Type = 'JOBWORKER' or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_IdNo = 0)")
        Else
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Vendor_AlaisHead", "Vendor_DisplayName", "", "(Vendor_IdNo = 0)")
        End If

    End Sub

    Private Sub cbo_VendorName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_VendorName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1417" Then
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_VendorName, cbo_Delivered, cbo_VehicleNo, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = 'WEAVER'  or Ledger_Type = 'GODOWN'  or Ledger_Type = 'JOBWORKER' or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_IdNo = 0)")
        Else
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_VendorName, cbo_Delivered, cbo_VehicleNo, "Vendor_AlaisHead", "Vendor_DisplayName", "", "(Vendor_IdNo = 0)")
        End If

    End Sub

    Private Sub cbo_VendorName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_VendorName.KeyPress
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1417" Then
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_VendorName, cbo_VehicleNo, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = 'WEAVER'  or Ledger_Type = 'GODOWN'  or Ledger_Type = 'JOBWORKER' or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_IdNo = 0)")
        Else
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_VendorName, cbo_VehicleNo, "Vendor_AlaisHead", "Vendor_DisplayName", "", "(Vendor_IdNo = 0)")
        End If

    End Sub

    Private Sub cbo_VendorName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_VendorName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1417" Then

            '    Common_Procedures.MDI_LedType = "WEAVER"
            '    Dim f As New Ledger_Creation

            '    Common_Procedures.Master_Return.Form_Name = Me.Name
            '    Common_Procedures.Master_Return.Control_Name = cbo_VendorName.Name
            '    Common_Procedures.Master_Return.Return_Value = ""
            '    Common_Procedures.Master_Return.Master_Type = ""

            '    f.MdiParent = MDIParent1
            '    f.Show()
            'Else

            Dim f As New Vendor_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_VendorName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

            'End If

        End If

    End Sub




    Private Sub txt_Approx_Value_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If (e.KeyValue = 38) Then

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1263" Then
                cbo_VendorName.Focus()
            Else
                cbo_DeliveryTo.Focus()
            End If

        End If
        If (e.KeyValue = 40) Then
            txt_Remarks.Focus()
        End If

    End Sub

    Private Sub txt_Rate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Rate.KeyDown
        If e.KeyValue = 38 Then
            Cbo_RateFor.Focus()
        End If

        If e.KeyValue = 40 Then
            If Common_Procedures.settings.CustomerCode = "1282" Then
                chk_Loaded.Focus()
            Else
                If MessageBox.Show("Do you want to save? ", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                    save_record()
                Else
                    dtp_Date.Focus()
                End If
            End If

        End If
    End Sub

    Private Sub txt_Rate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Rate.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If Common_Procedures.settings.CustomerCode = "1282" Then
                chk_Loaded.Focus()
            Else
                If MessageBox.Show("Do you want to save? ", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                    save_record()

                End If
            End If
        End If
    End Sub




    Private Sub txt_Rate_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_Rate.TextChanged
        Amount_Calultation()
    End Sub


    Private Sub Amount_Calultation()
        Dim vTotMtrs As String
        Dim nTotWgt As String

        vTotMtrs = 0
        nTotWgt = 0
        If dgv_Details_Total.RowCount > 0 Then
            vTotMtrs = Val(dgv_Details_Total.Rows(0).Cells(4).Value())
            nTotWgt = Val(dgv_Details_Total.Rows(0).Cells(9).Value())
        End If

        If Trim(Cbo_RateFor.Text) = "METERS" Then
            lbl_value.Text = Format(Val(vTotMtrs) * Val(txt_Rate.Text), "########0.00")
        Else
            lbl_value.Text = Format(Val(nTotWgt) * Val(txt_Rate.Text), "########0.00")
        End If

    End Sub

    Private Sub cbo_Delivered_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Delivered.KeyDown

        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Delivered, Nothing, Nothing, "Sizing_Pavu_Delivery_Head", "Delivered_By", "", "")

        If (e.KeyValue = 40 And cbo_Delivered.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If Trim(Common_Procedures.settings.CustomerCode) = "1220" Or Trim(Common_Procedures.settings.CustomerCode) = "1282" Or Trim(Common_Procedures.settings.CustomerCode) = "1288" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1263" Then
                cbo_VendorName.Focus()
            Else
                cbo_DeliveryTo.Focus()
            End If
        End If

        If (e.KeyValue = 38 And cbo_Delivered.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
            cbo_Transport.Focus()
        End If
    End Sub

    Private Sub cbo_Delivered_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Delivered.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Delivered, cbo_DeliveryTo, "Sizing_Pavu_Delivery_Head", "Delivered_By", "", "", False)
        If Trim(Common_Procedures.settings.CustomerCode) = "1220" Or Trim(Common_Procedures.settings.CustomerCode) = "1282" Or Trim(Common_Procedures.settings.CustomerCode) = "1288" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1263" Then
            If Asc(e.KeyChar) = 13 Then
                cbo_VendorName.Focus()
            Else
                cbo_DeliveryTo.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_Type_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Type.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")
    End Sub

    Private Sub cbo_Type_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Type.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Type, cbo_Ledger, txt_BookNo, "", "", "", "")
    End Sub

    Private Sub cbo_Type_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Type.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If Trim(UCase(cbo_Type.Text)) <> "DIRECT" Then
                If MessageBox.Show("Do you want to select Pavu :", "FOR PAVU SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                    btn_Selection_Click(sender, e)

                Else
                    txt_BookNo.Focus()

                End If

            Else
                txt_BookNo.Focus()


            End If

        End If

    End Sub

    Private Sub cbo_Type_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Type.TextChanged
        With dgv_Details
            If Trim(UCase(cbo_Type.Text)) <> "DIRECT" Then
                dgv_Details.AllowUserToAddRows = False

                .Columns(1).ReadOnly = True
                .Columns(2).ReadOnly = True
                .Columns(3).ReadOnly = True
                .Columns(4).ReadOnly = True
                .Columns(5).ReadOnly = False
                .Columns(7).ReadOnly = True
                .Columns(8).ReadOnly = True
                .Columns(9).ReadOnly = True


            Else
                dgv_Details.AllowUserToAddRows = True

                .Columns(1).ReadOnly = False
                .Columns(2).ReadOnly = False
                .Columns(3).ReadOnly = False
                .Columns(4).ReadOnly = False
                .Columns(5).ReadOnly = False
                .Columns(7).ReadOnly = False
                .Columns(8).ReadOnly = False
                .Columns(9).ReadOnly = False
            End If

        End With
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

            cmd.CommandText = "Update Sizing_Pavu_Delivery_Head set PrintOut_Status = " & Str(Val(vPrnSTS)) & " where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Pavu_Delivery_Code = '" & Trim(NewCode) & "'"
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

    Private Sub cbo_Count_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Count.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
    End Sub

    Private Sub cbo_Count_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Count.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_Count, Nothing, Nothing, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

        With dgv_Details

            If (e.KeyValue = 38 And cbo_Grid_Count.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_Grid_Count.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(9)
            End If

        End With
    End Sub

    Private Sub cbo_Count_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_Count.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_Count, Nothing, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            With dgv_Details
                .Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(9)

            End With

        End If
    End Sub


    Private Sub cbo_Count_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Count.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Count_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_Count.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_Count_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Count.TextChanged
        Try
            If cbo_Grid_Count.Visible Then
                If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
                With dgv_Details
                    If Val(cbo_Grid_Count.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 8 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_Count.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format7(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim EntryCode As String
        Dim NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim cnt As Integer = 0
        Dim LnAr(20) As Single, ClArr(15) As Single
        'Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim PS As Printing.PaperSize
        Dim PCnt As Integer = 0, PrntCnt As Integer = 0
        Dim TpMargin As Single = 0
        Dim Cnt1 As Integer = 0

        PrntCnt = 1

        If Val(Common_Procedures.settings.PavuDelivery_Print_2Copy_In_SinglePage) = 1 Then
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

        pFont = New Font("Calibri", 10, FontStyle.Regular)

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

        NoofItems_PerPage = 20 ' 6

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1263" Then '---- Asia Sizing (Palladam)
            ClArr(1) = Val(25) : ClArr(2) = 50 : ClArr(3) = 60 : ClArr(4) = 55 : ClArr(5) = 50 : ClArr(6) = 70 : ClArr(7) = 70
            ClArr(8) = 30 : ClArr(9) = 50 : ClArr(10) = 60 : ClArr(11) = 55 : ClArr(12) = 50 : ClArr(13) = 70
            ClArr(14) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) + ClArr(11) + ClArr(12) + ClArr(13))
        Else

            ClArr(1) = Val(40) : ClArr(2) = 80 : ClArr(3) = 80 : ClArr(4) = 100 : ClArr(5) = 150 : ClArr(6) = 100

            ClArr(7) = 240

            ClArr(8) = 250
            ClArr(9) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8))


            'ClArr(1) = Val(25) : ClArr(2) = 40 : ClArr(3) = 55 : ClArr(4) = 55 : ClArr(5) = 40 : ClArr(6) = 70 : ClArr(7) = 100
            'ClArr(8) = 30 : ClArr(9) = 40 : ClArr(10) = 55 : ClArr(11) = 55 : ClArr(12) = 40 : ClArr(13) = 70
            'ClArr(14) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) + ClArr(11) + ClArr(12) + ClArr(13))
        End If

        TxtHgt = 18.2 ' 18.5 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        PrntCnt2ndPageSTS = False
        TpMargin = TMargin

        For PCnt = 1 To PrntCnt
            If Val(Common_Procedures.settings.PavuDelivery_Print_2Copy_In_SinglePage) = 1 Then
                If PCnt = 1 Then
                    prn_PageNo1 = prn_PageNo

                    prn_DetIndx1 = prn_DetIndx
                    prn_DetSNo1 = prn_DetSNo
                    prn_NoofBmDets1 = prn_NoofBmDets
                    TpMargin = TMargin

                Else

                    prn_PageNo = prn_PageNo1
                    prn_NoofBmDets = prn_NoofBmDets1
                    prn_DetIndx = prn_DetIndx1
                    prn_DetSNo = prn_DetSNo1

                    TpMargin = 580 + TMargin  ' 600 + TMargin

                End If
            End If

            Try

                If prn_HdDt.Rows.Count > 0 Then

                    Printing_Format7_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TpMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                    NoofDets = 0

                    CurY = CurY - 10

                    If prn_DetDt.Rows.Count > 0 Then


                        Do While prn_NoofBmDets < prn_DetMxIndx
                            If NoofDets >= NoofItems_PerPage Then
                                If Val(Common_Procedures.settings.PavuDelivery_Print_2Copy_In_SinglePage) = 1 Then

                                    If PCnt = 2 And PrntCnt = 2 And PrntCnt2ndPageSTS = False Then


                                        CurY = CurY + TxtHgt

                                        Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                                        NoofDets = NoofDets + 1

                                        Printing_Format7_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, PCnt, False)

                                        prn_DetIndx = prn_DetIndx + NoofItems_PerPage

                                        e.HasMorePages = True

                                        Return
                                    End If

                                Else

                                    CurY = CurY + TxtHgt

                                    Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                                    NoofDets = NoofDets + 1

                                    Printing_Format7_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, PCnt, False)

                                    prn_DetIndx = prn_DetIndx + NoofItems_PerPage

                                    e.HasMorePages = True

                                    Return

                                End If
                            End If

                            prn_DetIndx = prn_DetIndx + 1

                            If PCnt <> 2 Then

                                If Val(prn_DetAr(prn_DetIndx, 4)) <> 0 Then
                                    If PCnt = 1 And NoofDets < NoofItems_PerPage Then
                                        CurY = CurY + TxtHgt
                                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx, 1))), LMargin + 3, CurY, 0, 0, pFont)
                                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 6)), LMargin + ClArr(1) + 3, CurY, 0, 0, pFont)
                                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 2)), LMargin + ClArr(1) + ClArr(2) + 3, CurY, 0, 0, pFont)
                                        Common_Procedures.Print_To_PrintDocument(e, Trim((prn_DetAr(prn_DetIndx, 7))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 3, CurY, 0, 0, pFont)
                                        Common_Procedures.Print_To_PrintDocument(e, Trim((prn_DetAr(prn_DetIndx, 8))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 3, CurY, 0, 0, pFont)
                                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx, 4)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 5, CurY, 1, 0, pFont)
                                        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1263" Then '---- Meenashi Sizing (Kpati)
                                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 9)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + 3, CurY, 0, 0, pFont)
                                        Else
                                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 5)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + 3, CurY, 0, 0, pFont)
                                        End If

                                        Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Mill_IdNoToName(con, Val(prn_DetDt.Rows(prn_DetIndx - 1).Item("Mill_IdNo").ToString)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + 3, CurY, 0, 0, pFont)
                                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx - 1).Item("Sort_No").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + 3, CurY, 0, 0, pFont)


                                    End If
                                    prn_NoofBmDets = prn_NoofBmDets + 1

                                End If

                                If Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 4)) <> 0 Then
                                    If PCnt = 1 And NoofDets < NoofItems_PerPage Then
                                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 1))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + 7, CurY, 0, 0, pFont)
                                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 6)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + 3, CurY, 0, 0, pFont)
                                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 2))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + 3, CurY, 0, 0, pFont)
                                        Common_Procedures.Print_To_PrintDocument(e, Trim((prn_DetAr(prn_DetIndx + NoofItems_PerPage, 7))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) + 3, CurY, 0, 0, pFont)
                                        Common_Procedures.Print_To_PrintDocument(e, Trim((prn_DetAr(prn_DetIndx + NoofItems_PerPage, 8))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) + ClArr(11) + 3, CurY, 0, 0, pFont)
                                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 4)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) + ClArr(11) + ClArr(12) + ClArr(13) - 5, CurY, 1, 0, pFont)
                                        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1263" Then '---- Meenashi Sizing (Kapati)
                                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 9)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) + ClArr(11) + ClArr(12) + ClArr(13) + 3, CurY, 0, 0, pFont)
                                        Else
                                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 5)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) + ClArr(11) + ClArr(12) + ClArr(13) + 3, CurY, 0, 0, pFont)
                                        End If

                                    End If
                                    prn_NoofBmDets = prn_NoofBmDets + 1

                                End If

                                NoofDets = NoofDets + 1
                            End If

                            If PCnt = 2 Then
                                If Val(prn_DetAr(prn_DetIndx, 4)) <> 0 Then
                                    CurY = CurY + TxtHgt
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx, 1))), LMargin + 3, CurY, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 6)), LMargin + ClArr(1) + 3, CurY, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx, 2))), LMargin + ClArr(1) + ClArr(2) + 3, CurY, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Trim((prn_DetAr(prn_DetIndx, 7))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 3, CurY, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Trim((prn_DetAr(prn_DetIndx, 8))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 3, CurY, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx, 4)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 3, CurY, 1, 0, pFont)
                                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1263" Then
                                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 9)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + 3, CurY, 0, 0, pFont)
                                    Else
                                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 5)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + 3, CurY, 0, 0, pFont)
                                    End If
                                    prn_NoofBmDets = prn_NoofBmDets + 1

                                End If

                                If Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 4)) <> 0 Then

                                    Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 1))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + 7, CurY, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 6)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + 3, CurY, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 2))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + 3, CurY, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Trim((prn_DetAr(prn_DetIndx + NoofItems_PerPage, 7))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) + 3, CurY, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Trim((prn_DetAr(prn_DetIndx + NoofItems_PerPage, 8))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) + ClArr(11) + 3, CurY, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 4)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) + ClArr(11) + ClArr(12) + ClArr(13) - 3, CurY, 1, 0, pFont)
                                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1263" Then
                                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 9)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) + ClArr(11) + ClArr(12) + ClArr(13) + 3, CurY, 0, 0, pFont)
                                    Else
                                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 5)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) + ClArr(11) + ClArr(12) + ClArr(13) + 3, CurY, 0, 0, pFont)
                                    End If
                                    prn_NoofBmDets = prn_NoofBmDets + 1

                                End If

                                NoofDets = NoofDets + 1
                            End If


                        Loop


                    End If

                    Printing_Format7_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, PCnt, True)

                End If

            Catch ex As Exception

                MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End Try
            If Val(Common_Procedures.settings.PavuDelivery_Print_2Copy_In_SinglePage) = 1 Then

                If PCnt = 1 And PrntCnt = 2 And PrntCnt2ndPageSTS = False Then
                    If prn_DetDt.Rows.Count = cnt + 18 Then
                        PrntCnt2ndPageSTS = True
                        e.HasMorePages = True
                        cnt = 18
                        Return
                    End If
                End If
            End If
            PrntCnt2ndPageSTS = False

        Next PCnt
LOOP10:


        prn_Count = prn_Count + 1

        e.HasMorePages = False

        If Val(prn_TotCopies) > 1 Then
            If prn_Count < Val(prn_TotCopies) Then

                prn_DetIndx = 0
                prn_PageNo = 0
                prn_DetIndx = 0
                prn_PageNo = 0
                prn_NoofBmDets = 0


                e.HasMorePages = True
                Return

            End If

        End If

    End Sub

    Private Sub Printing_Format7_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim da3 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim p1Font As Font

        Dim strHeight As Single
        Dim W1 As Single, W2 As Single, W3 As Single, N1 As Single, M1 As Single
        Dim i As Integer, k As Integer = 0
        Dim Arr(300, 5) As String
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String, Cmp_Add3 As String, Cmp_Add4 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_Email As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim Led_Name As String, Led_Add1 As String, Led_Add2 As String, Led_Add3 As String, Led_Add4 As String
        Dim Led_GstNo As String
        Dim Hsn_Code As String = ""
        Dim Cnt_Name As String = ""
        'Dim Entry_dt As Date
        Dim CurX As Single = 0
        Dim strWidth As Single = 0

        PageNo = PageNo + 1

        CurY = TMargin

        If prn_DetMxIndx > (2 * NoofItems_PerPage) Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If

        'da2 = New SqlClient.SqlDataAdapter("select DISTINCT(setcode_forSelection) from Stock_SizedPavu_Processing_Details where Pavu_Delivery_Code = '" & Trim(EntryCode) & "' Order by setcode_forSelection", con)
        'dt3 = New DataTable
        'da2.Fill(dt3)

        'vSetNo = ""
        'If dt3.Rows.Count > 0 Then
        '    For i = 0 To dt3.Rows.Count - 1
        '        k = InStr(1, dt3.Rows(i).Item("setcode_forSelection").ToString, "/")
        '        vSetNo = vSetNo & IIf(Trim(vSetNo) <> "", ", ", "") & Microsoft.VisualBasic.Left(dt3.Rows(i).Item("setcode_forSelection").ToString, k - 1)
        '    Next i
        'End If
        'dt3.Dispose()
        Hsn_Code = ""
        Cnt_Name = dgv_Details.Rows(0).Cells(8).Value
        da3 = New SqlClient.SqlDataAdapter("select a.*, b.* from Stock_SizedPavu_Processing_Details a LEFT OUTER JOIN Count_Head b ON a.Count_IdNo = b.Count_IdNo where a.Pavu_Delivery_Code = '" & Trim(EntryCode) & "' and b.Count_Name = '" & Trim(Cnt_Name) & "' Order by a.sl_no", con)
        dt4 = New DataTable
        da3.Fill(dt4)


        If dt4.Rows.Count > 0 Then
            For i = 0 To dt4.Rows.Count - 1
                'k = InStr(1, dt4.Rows(i).Item("Count_Hsn_Code").ToString, "/")
                'Hsn_Code = Hsn_Code & IIf(Trim(Hsn_Code) <> "", ", ", "") & Microsoft.VisualBasic.Left(dt4.Rows(i).Item("Count_Hsn_Code").ToString, k - 1)
                ' Cnt_Name = dt4.Rows(i).Item("Count_Name").ToString
                Hsn_Code = dt4.Rows(0).Item("Count_Hsn_Code").ToString
            Next
        End If
        dt4.Clear()
        dt4.Dispose()

        CurY = TMargin
        CurY = CurY + 5
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "SIZED PAVU DELIVERY NOTE", LMargin, CurY - TxtHgt, 2, PrintWidth, p1Font)
        ' strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

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
            Cmp_GSTIN_No = "GSTIN" & prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
        End If
        If Trim(UCase(Common_Procedures.User.Type)) = "UNACCOUNT" And Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1263" Then
            Led_Name = prn_HdDt.Rows(0).Item("Ledger_MainName").ToString : Led_Add1 = "" : Led_Add2 = "" : Led_Add3 = "" : Led_Add4 = "" : Led_GstNo = ""
        Else
            Led_Name = prn_HdDt.Rows(0).Item("Ledger_MainName").ToString
            Led_Add1 = prn_HdDt.Rows(0).Item("Ledger_Address1").ToString
            Led_Add2 = prn_HdDt.Rows(0).Item("Ledger_Address2").ToString
            Led_Add3 = prn_HdDt.Rows(0).Item("Ledger_Address3").ToString & "," & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString
            Led_Add4 = prn_HdDt.Rows(0).Item("Ledger_Address4").ToString

            If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then

                Led_GstNo = "GSTIN" & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString
            End If
        End If
        CurY = CurY + TxtHgt - 10

        M1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4)

        'M1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7)

        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin + 10, CurY, 0, 0, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height
        Common_Procedures.Print_To_PrintDocument(e, "PAVU SIZED TO :", LMargin + M1 + (ClAr(5) / 2) + 10, CurY, 0, 0, pFont)

        ' p1Font = New Font("Calibri", 9, FontStyle.Regular)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "M/s." & Led_Name, LMargin + M1 + (ClAr(5) / 2) + 10, CurY, 0, 0, p1Font)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Led_Add1, LMargin + M1 + (ClAr(5) / 2) + 10, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add3, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Led_Add2, LMargin + M1 + (ClAr(5) / 2) + 10, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add4, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Led_Add3, LMargin + M1 + (ClAr(5) / 2) + 10, CurY, 0, 0, pFont)
        'CurY = CurY + TxtHgt
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, Led_Add4, LMargin + M1 + 10, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Led_GstNo, LMargin + M1 + (ClAr(5) / 2) + 10, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY
        e.Graphics.DrawLine(Pens.Black, LMargin + M1 + (ClAr(5) / 2), LnAr(2), LMargin + M1 + (ClAr(5) / 2), LnAr(1))


        ' CurY = CurY + TxtHgt - 12



        ' Try

        N1 = e.Graphics.MeasureString("DATE & TIME  :", pFont).Width
        W1 = e.Graphics.MeasureString("HSC CODE  :", pFont).Width
        W2 = e.Graphics.MeasureString("VAN NO   :", pFont).Width
        W3 = e.Graphics.MeasureString("APPROX VALUE  :", pFont).Width

        M1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5)

        CurY = CurY + TxtHgt - 11
        p1Font = New Font("Calibri", 10, FontStyle.Bold)


        Common_Procedures.Print_To_PrintDocument(e, "DC.NO", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
        p1Font = New Font("Calibri", 10, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Pavu_Delivery_No").ToString, LMargin + W1 + 25, CurY, 0, 0, p1Font)
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1078" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1087" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1112" Then '---- Kalaimagal Sizing (Palladam)
            Common_Procedures.Print_To_PrintDocument(e, "DATE & TIME", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + (ClAr(4) \ 2) + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + (ClAr(4) \ 2) + N1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, (prn_HdDt.Rows(0).Item("Date_And_Time_Of_Supply").ToString).ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + (ClAr(4) \ 2) + N1 + 20, CurY, 0, 0, pFont)
        End If
        'Common_Procedures.Print_To_PrintDocument(e, "VAN NO", LMargin + M1, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 - 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vehicle_No").ToString, LMargin + M1 + W1 + 5, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, "APPROX VALUE", LMargin + M1 + 10 + 50, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 84, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Approx_Value").ToString, LMargin + M1 + W1 + 120, CurY, 0, 0, pFont)


        'Common_Procedures.Print_To_PrintDocument(e, "APPROX VALUE", LMargin + M1 + ClAr(6) + 40, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + ClAr(6) + W3 + 40, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Approx_Value").ToString, LMargin + M1 + ClAr(6) + W3 + 70, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt

        Common_Procedures.Print_To_PrintDocument(e, "HSN CODE", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, (Hsn_Code), LMargin + W1 + 25, CurY, 0, 0, pFont)

        p1Font = New Font("Calibri", 10, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "SAC CODE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + (ClAr(4) \ 2) + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + (ClAr(4) \ 2) + N1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, " 998821", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + (ClAr(4) \ 2) + N1 + 20, CurY, 0, 0, p1Font)

        Common_Procedures.Print_To_PrintDocument(e, "E.WAY BILL NO", LMargin + M1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 34, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Electronic_Reference_No").ToString, LMargin + M1 + W2 + 50, CurY, 0, 0, pFont)

        'Common_Procedures.Print_To_PrintDocument(e, "TRANSPORT", LMargin + M1 + ClAr(6) + 40, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + ClAr(6) + W3 + 40, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Transport_Name").ToString, LMargin + M1 + ClAr(6) + W3 + 50, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt

        Common_Procedures.Print_To_PrintDocument(e, "VAN NO", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vehicle_No").ToString, LMargin + W1 + 25, CurY, 0, 0, pFont)

        p1Font = New Font("Calibri", 10, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "TRANSPORT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + (ClAr(4) \ 2) + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + (ClAr(4) \ 2) + N1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Transport_Name").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + (ClAr(4) \ 2) + N1 + 20, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt + 2
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        pFont = New Font("Calibri", 9, FontStyle.Regular)
        CurY = CurY + TxtHgt - 15
        Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin, CurY, 2, ClAr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "SET NO", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "BEAM NO", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "ENDS", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "COUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 3, CurY, 2, ClAr(6), pFont)
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1263" Then
            Common_Procedures.Print_To_PrintDocument(e, "MARK", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
        Else
            Common_Procedures.Print_To_PrintDocument(e, "DELIVERY PARTY", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1288" Then
            Common_Procedures.Print_To_PrintDocument(e, "MILL NAME", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "SORT NO", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)
        End If

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY
        pFont = New Font("Calibri", 10, FontStyle.Regular)
        ' Catch ex As Exception

        'MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        '  End Try

    End Sub

    Private Sub Printing_Format7_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal pcnt As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim Cmp_Name As String
        Dim I As Integer
        Dim M1 As Single
        Dim Del_Add1 As String = "", Del_Add2 As String = "", nGST_No As String = ""
        Dim cnt2 As Integer = 0
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        Try

            For I = NoofDets + 1 To NoofItems_PerPage
                CurY = CurY + TxtHgt
            Next

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

            M1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7)
            CurY = CurY + TxtHgt - 15

            If is_LastPage = True Then

                If (prn_DetMxIndx Mod (NoofItems_PerPage * 2)) <= NoofItems_PerPage Then

                    If Val(prn_HdDt.Rows(0).Item("Total_Beam").ToString) <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Beam").ToString), LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
                    End If

                    If Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString) <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 5, CurY, 1, 0, pFont)
                    End If

                Else

                    If Val(prn_HdDt.Rows(0).Item("Total_Beam").ToString) <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Beam").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + 10, CurY, 0, 0, pFont)
                    End If


                    If Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString) <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) - 5, CurY, 1, 0, pFont)
                    End If

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

            CurY = CurY + TxtHgt - 15

            pFont = New Font("Calibri", 9, FontStyle.Regular)

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Or Trim(Common_Procedures.settings.CustomerCode) = "1282" Or Trim(Common_Procedures.settings.CustomerCode) = "1288" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1263" Then '---- Sri Meenakshi Sizing (Somanur)
                p1Font = New Font("Calibri", 9, FontStyle.Bold)
                If Common_Procedures.Vendor_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("Vendor_IdNo").ToString)) <> "" Then

                    da1 = New SqlClient.SqlDataAdapter("select a.* from Vendor_head a where  a.Vendor_IdNO = " & Val(prn_HdDt.Rows(0).Item("Vendor_IdNo").ToString) & " ", con)
                    dt = New DataTable
                    da1.Fill(dt)

                    If dt.Rows.Count > 0 Then

                        Del_Add1 = dt.Rows(0).Item("Vendor_Address1").ToString & "," & dt.Rows(0).Item("Vendor_Address2").ToString
                        Del_Add2 = dt.Rows(0).Item("Vendor_Address3").ToString & "," & dt.Rows(0).Item("Vendor_Address4").ToString
                        If Trim(dt.Rows(0).Item("GST_No").ToString) <> "" Then nGST_No = "GSTIN : " & dt.Rows(0).Item("GST_No").ToString

                    End If
                    Common_Procedures.Print_To_PrintDocument(e, "DELIVERY TO   : " & Common_Procedures.Vendor_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("Vendor_IdNo").ToString)), LMargin + 10, CurY, 0, 0, p1Font)
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, " " & Trim(Del_Add1), LMargin + 30, CurY, 0, 0, pFont)
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, " " & Trim(Del_Add2), LMargin + 30, CurY, 0, 0, pFont)
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, " " & Trim(nGST_No), LMargin + 30, CurY, 0, 0, pFont)
                Else

                    'Led_Name = prn_HdDt.Rows(0).Item("Ledger_MainName").ToString
                    'Led_Add1 = prn_HdDt.Rows(0).Item("Ledger_Address1").ToString
                    'Led_Add2 = prn_HdDt.Rows(0).Item("Ledger_Address2").ToString
                    'Led_Add3 = prn_HdDt.Rows(0).Item("Ledger_Address3").ToString & "," & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString
                    'Led_Add4 = prn_HdDt.Rows(0).Item("Ledger_Address4").ToString

                    Del_Add1 = prn_HdDt.Rows(0).Item("Ledger_Address1").ToString & "," & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString
                    Del_Add2 = prn_HdDt.Rows(0).Item("Ledger_Address3").ToString & "," & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString

                    Common_Procedures.Print_To_PrintDocument(e, "DELIVERY TO   : " & prn_HdDt.Rows(0).Item("Ledger_MainName").ToString, LMargin + 10, CurY, 0, 0, pFont)
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, " " & Trim(Del_Add1), LMargin + 30, CurY, 0, 0, pFont)
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, " " & Trim(Del_Add2), LMargin + 30, CurY, 0, 0, pFont)

                End If

            Else

                Del_Add1 = prn_HdDt.Rows(0).Item("DelAdd1").ToString & "," & prn_HdDt.Rows(0).Item("DelAdd2").ToString
                Del_Add2 = prn_HdDt.Rows(0).Item("DelAdd3").ToString & "," & prn_HdDt.Rows(0).Item("DelAdd4").ToString

                Common_Procedures.Print_To_PrintDocument(e, "DELIVERY TO   : " & prn_HdDt.Rows(0).Item("DelName").ToString, LMargin + 10, CurY, 0, 0, pFont)
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, " " & Trim(Del_Add1), LMargin + 30, CurY, 0, 0, pFont)
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, " " & Trim(Del_Add2), LMargin + 30, CurY, 0, 0, pFont)

            End If



            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "This is to certify that there is no sale indicated in this delivery and the pavu sized is returned back to party after warping and sizing job work.", LMargin + 10, CurY, 0, 0, pFont)

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
                Common_Procedures.Print_To_PrintDocument(e, "Receiver's signature", LMargin + 20, CurY, 0, 0, pFont)
            End If

            If Trim(Common_Procedures.settings.CustomerCode) = "1288" Then
                Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 130, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "Checked By  ", LMargin + 270, CurY, 0, 0, pFont)
            End If

            '  Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 250, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 5, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt + 5

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin, LnAr(1))
            e.Graphics.DrawLine(Pens.Black, PageWidth, CurY, PageWidth, LnAr(1))

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub


    Private Sub Cbo_RateFor_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_RateFor.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")
    End Sub

    Private Sub Cbo_RateFor_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Cbo_RateFor.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, Cbo_RateFor, txt_Remarks, txt_Rate, "", "", "", "")
        'If (e.KeyValue = 40 And cbo_VehicleNo.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

        '    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Then
        '        cbo_VendorName.Focus()
        '    Else
        '        txt_Rate.Focus()
        '    End If

        'End If
    End Sub

    Private Sub Cbo_RateFor_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Cbo_RateFor.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, Cbo_RateFor, txt_Rate, "", "", "", "", False)
        'If Asc(e.KeyChar) = 13 Then
        '    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Then
        '        cbo_VendorName.Focus()
        '    Else
        '        txt_Rate.Focus()
        '    End If
        'End If
    End Sub

    Private Sub Cbo_RateFor_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_RateFor.TextChanged
        Amount_Calultation()
    End Sub

    Private Sub cbo_MillName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_MillName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")
    End Sub

    Private Sub cbo_MillName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_MillName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_MillName, txt_ElectronicRefNo, Nothing, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")
        If (e.KeyValue = 40 And cbo_MillName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
            dgv_Details.CurrentCell.Selected = True
        End If
    End Sub

    Private Sub cbo_MillName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_MillName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_MillName, Nothing, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
            dgv_Details.CurrentCell.Selected = True
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

    Private Sub Printing_Format8(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim EntryCode As String
        Dim NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim cnt As Integer = 0
        Dim LnAr(20) As Single, ClArr(15) As Single
        'Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim PS As Printing.PaperSize
        Dim PCnt As Integer = 0, PrntCnt As Integer = 0
        Dim TpMargin As Single = 0
        Dim Cnt1 As Integer = 0

        PrntCnt = 1

        PrintDocument1.DefaultPageSettings.Landscape = False

        If Val(Common_Procedures.settings.PavuDelivery_Print_2Copy_In_SinglePage) = 1 Then

            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A3 Then
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


        If Val(Common_Procedures.settings.PavuDelivery_Print_2Copy_In_SinglePage) = 1 Then
            If PrntCnt2ndPageSTS = False Then
                PrntCnt = 2
            End If
        End If

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 10
            .Right = 42
            .Top = 20
            .Bottom = 20
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

        NoofItems_PerPage = 22  ' 6

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}


        ClArr(1) = Val(40) : ClArr(2) = 130 : ClArr(3) = 130 : ClArr(4) = 130
        ClArr(5) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4))


        TxtHgt = 15.9 ' 18.5 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        PrntCnt2ndPageSTS = False
        TpMargin = TMargin

        For PCnt = 1 To PrntCnt
            If Val(Common_Procedures.settings.PavuDelivery_Print_2Copy_In_SinglePage) = 1 Then
                If PCnt = 1 Then
                    prn_PageNo1 = prn_PageNo

                    prn_DetIndx1 = prn_DetIndx
                    prn_DetSNo1 = prn_DetSNo
                    prn_NoofBmDets1 = prn_NoofBmDets
                    TpMargin = TMargin

                Else

                    prn_PageNo = prn_PageNo1
                    prn_NoofBmDets = prn_NoofBmDets1
                    prn_DetIndx = prn_DetIndx1
                    prn_DetSNo = prn_DetSNo1

                    TpMargin = 580 + TMargin  ' 600 + TMargin

                End If
            End If

            Try

                If prn_HdDt.Rows.Count > 0 Then

                    Printing_Format8_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TpMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                    NoofDets = 0

                    CurY = CurY - 10

                    If prn_DetDt.Rows.Count > 0 Then

                        Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                            If NoofDets >= NoofItems_PerPage Then

                                If Val(Common_Procedures.settings.PavuDelivery_Print_2Copy_In_SinglePage) = 1 Then

                                    If PCnt = 2 And PrntCnt = 2 And PrntCnt2ndPageSTS = False Then


                                        CurY = CurY + TxtHgt

                                        Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                                        NoofDets = NoofDets + 1

                                        Printing_Format8_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, PCnt, False)



                                        e.HasMorePages = True

                                        Return
                                    End If

                                Else

                                    CurY = CurY + TxtHgt

                                    Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                                    NoofDets = NoofDets + 1

                                    Printing_Format8_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, PCnt, False)


                                    e.HasMorePages = True

                                    Return

                                End If
                            End If

                            prn_DetIndx = prn_DetIndx + 1

                            If PCnt <> 2 Then

                                If Val(prn_DetAr(prn_DetIndx, 4)) <> 0 Then
                                    If PCnt = 1 And NoofDets < NoofItems_PerPage Then
                                        CurY = CurY + TxtHgt
                                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx, 1))), LMargin + 3, CurY, 0, 0, pFont)
                                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 6)), LMargin + ClArr(1) + 3, CurY, 0, 0, pFont)
                                        If Common_Procedures.settings.CustomerCode = "1282" Then
                                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 2)), LMargin + ClArr(1) + ClArr(2) + 3, CurY, 0, 0, pFont)
                                        Else
                                            Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx, 2))), LMargin + ClArr(1) + ClArr(2) + 3, CurY, 0, 0, pFont)
                                        End If

                                        Common_Procedures.Print_To_PrintDocument(e, Trim((prn_DetAr(prn_DetIndx, 7))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 3, CurY, 0, 0, pFont)
                                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx, 4)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 5, CurY, 1, 0, pFont)


                                    End If

                                    prn_NoofBmDets = prn_NoofBmDets + 1

                                End If



                                NoofDets = NoofDets + 1
                            End If

                            If PCnt = 2 Then
                                If Val(prn_DetAr(prn_DetIndx, 4)) <> 0 Then
                                    CurY = CurY + TxtHgt
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx, 1))), LMargin + 3, CurY, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 6)), LMargin + ClArr(1) + 3, CurY, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx, 2))), LMargin + ClArr(1) + ClArr(2) + 3, CurY, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Trim((prn_DetAr(prn_DetIndx, 7))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 3, CurY, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Trim((prn_DetAr(prn_DetIndx, 8))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 3, CurY, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx, 4)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 3, CurY, 1, 0, pFont)
                                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1263" Then
                                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 9)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + 3, CurY, 0, 0, pFont)
                                    Else
                                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 5)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + 3, CurY, 0, 0, pFont)
                                    End If
                                    prn_NoofBmDets = prn_NoofBmDets + 1

                                End If



                                NoofDets = NoofDets + 1
                            End If


                        Loop


                    End If

                    Printing_Format8_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, PCnt, True)

                End If

            Catch ex As Exception

                MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End Try
            If Val(Common_Procedures.settings.PavuDelivery_Print_2Copy_In_SinglePage) = 1 Then

                If PCnt = 1 And PrntCnt = 2 And PrntCnt2ndPageSTS = False Then
                    If prn_DetDt.Rows.Count = cnt + 18 Then
                        PrntCnt2ndPageSTS = True
                        e.HasMorePages = True
                        cnt = 18
                        Return
                    End If
                End If
            End If
            PrntCnt2ndPageSTS = False

        Next PCnt
LOOP10:


        prn_Count = prn_Count + 1

        e.HasMorePages = False

        If Val(prn_TotCopies) > 1 Then
            If prn_Count < Val(prn_TotCopies) Then

                prn_DetIndx = 0
                prn_PageNo = 0
                prn_DetIndx = 0
                prn_PageNo = 0
                prn_NoofBmDets = 0


                e.HasMorePages = True
                Return

            End If

        End If

    End Sub

    Private Sub Printing_Format8_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim da3 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim p1Font As Font
        Dim pFontBold As Font = New Font("Calibri", 8, FontStyle.Bold)

        Dim strHeight As Single
        Dim W1 As Single, W2 As Single, W3 As Single, N1 As Single, M1 As Single
        Dim i As Integer, k As Integer = 0
        Dim Arr(300, 5) As String
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String, Cmp_Add3 As String, Cmp_Add4 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_Email As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim Led_Name As String, Led_Add1 As String, Led_Add2 As String, Led_Add3 As String, Led_Add4 As String, led_phone As String
        Dim Led_GstNo As String
        Dim Hsn_Code As String = ""
        Dim Cnt_Name As String = ""
        'Dim Entry_dt As Date
        Dim CurX As Single = 0
        Dim strWidth As Single = 0
        Dim Cmp_Sac_Cap As String = ""
        Dim cmp_sac_no As Integer = 0
        Dim del_add1 As String, del_add2 As String, del_add3 As String, del_add4 As String
        Dim nGST_No As String = "", del_Name As String = "", nVenPhon_No As String = ""


        PageNo = PageNo + 1

        CurY = TMargin

        If prn_DetMxIndx > (NoofItems_PerPage) Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If

        'da2 = New SqlClient.SqlDataAdapter("select DISTINCT(setcode_forSelection) from Stock_SizedPavu_Processing_Details where Pavu_Delivery_Code = '" & Trim(EntryCode) & "' Order by setcode_forSelection", con)
        'dt3 = New DataTable
        'da2.Fill(dt3)

        'vSetNo = ""
        'If dt3.Rows.Count > 0 Then
        '    For i = 0 To dt3.Rows.Count - 1
        '        k = InStr(1, dt3.Rows(i).Item("setcode_forSelection").ToString, "/")
        '        vSetNo = vSetNo & IIf(Trim(vSetNo) <> "", ", ", "") & Microsoft.VisualBasic.Left(dt3.Rows(i).Item("setcode_forSelection").ToString, k - 1)
        '    Next i
        'End If
        'dt3.Dispose()


        Hsn_Code = ""
        Cnt_Name = dgv_Details.Rows(0).Cells(8).Value
        da3 = New SqlClient.SqlDataAdapter("select a.*, b.* from Stock_SizedPavu_Processing_Details a LEFT OUTER JOIN Count_Head b ON a.Count_IdNo = b.Count_IdNo where a.Pavu_Delivery_Code = '" & Trim(EntryCode) & "' and b.Count_Name = '" & Trim(Cnt_Name) & "' Order by a.sl_no", con)
        dt4 = New DataTable
        da3.Fill(dt4)
        If dt4.Rows.Count > 0 Then
            For i = 0 To dt4.Rows.Count - 1
                'k = InStr(1, dt4.Rows(i).Item("Count_Hsn_Code").ToString, "/")
                'Hsn_Code = Hsn_Code & IIf(Trim(Hsn_Code) <> "", ", ", "") & Microsoft.VisualBasic.Left(dt4.Rows(i).Item("Count_Hsn_Code").ToString, k - 1)
                ' Cnt_Name = dt4.Rows(i).Item("Count_Name").ToString
                Hsn_Code = dt4.Rows(0).Item("HSN_Code").ToString ' dt4.Rows(0).Item("Count_Hsn_Code").ToString
            Next
        End If
        dt4.Clear()
        dt4.Dispose()

        CurY = TMargin
        CurY = CurY + 5
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "JOB WORK-SIZING", LMargin, CurY - TxtHgt, 2, PrintWidth, p1Font)
        ' strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY


        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = "" : Cmp_Add3 = "" : Cmp_Add4 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_Email = ""
        Cmp_StateCap = "" : Cmp_StateNm = "" : Cmp_StateCode = "" : Cmp_GSTIN_Cap = "" : Cmp_GSTIN_No = ""

        Led_Name = "" : Led_Add1 = "" : Led_Add2 = "" : Led_Add3 = "" : Led_Add4 = "" : Led_GstNo = ""

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString
        Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address2").ToString
        Cmp_Add3 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " , " & prn_HdDt.Rows(0).Item("Company_Address4").ToString


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
            Cmp_GSTIN_No = "GSTIN" & prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
        End If

        Cmp_Sac_Cap = "SAC : "
        cmp_sac_no = "998821"



        CurY = CurY + strHeight
        p1Font = New Font("Calibri", 12, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        CurY = CurY + TxtHgt
        p1Font = New Font("Calibri", 9.5, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "(SIZING DIVISION)", LMargin, CurY, 2, PrintWidth, p1Font)
        CurY = CurY + TxtHgt - 2
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, p1Font)
        CurY = CurY + TxtHgt - 2
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, p1Font)
        CurY = CurY + TxtHgt - 2
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add3, LMargin, CurY, 2, PrintWidth, p1Font)




        p1Font = New Font("Calibri", 10, FontStyle.Bold)
        strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_GSTIN_Cap), p1Font).Width
        strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_StateNm & "     " & Cmp_GSTIN_Cap & Cmp_GSTIN_No), pFont).Width
        If PrintWidth > strWidth Then
            CurX = LMargin + (PrintWidth - strWidth) / 2
        Else
            CurX = LMargin
        End If

        p1Font = New Font("Calibri", 11, FontStyle.Bold)


        CurY = CurY + TxtHgt - 2
        Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_Cap & Cmp_GSTIN_No & " , " & Cmp_Sac_Cap & cmp_sac_no, LMargin, CurY, 2, PrintWidth, p1Font)

        CurY = CurY + TxtHgt - 2
        Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PhNo), LMargin, CurY, 2, PrintWidth, p1Font)

        strWidth = e.Graphics.MeasureString(Cmp_StateCap, p1Font).Width
        CurX = CurX + strWidth
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_StateNm, CurX, CurY, 0, 0, pFont)

        strWidth = e.Graphics.MeasureString(Cmp_StateNm, pFont).Width
        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        CurX = CurX + strWidth

        CurY = CurY + TxtHgt + 3
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        N1 = e.Graphics.MeasureString("DATE & TIME  :", pFontBold).Width
        W1 = e.Graphics.MeasureString("Mill Name  :", pFont).Width
        W2 = e.Graphics.MeasureString("VAN NO   :", pFont).Width
        W3 = e.Graphics.MeasureString("NO  :", pFont).Width

        M1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4)

        CurY = CurY + TxtHgt - 9
        p1Font = New Font("Calibri", 9, FontStyle.Bold)
        LnAr(2) = CurY

        Common_Procedures.Print_To_PrintDocument(e, "DATE & TIME", LMargin + 10, CurY, 0, 0, pFontBold)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + N1 + 10, CurY, 0, 0, pFontBold)
        p1Font = New Font("Calibri", 10, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Pavu_Delivery_Date").ToString)) & "  " & Format((prn_HdDt.Rows(0).Item("Date_And_Time_Of_Supply").ToString)), LMargin + N1 + 25, CurY, 0, 0, pFontBold)
        'Format(Convert.ToDateTime(dt2.Rows(i).Item("Pavu_Delivery_Date").ToString), "dd-MM-yyyy")

        Common_Procedures.Print_To_PrintDocument(e, "VEH NO", LMargin + ClAr(1) + ClAr(2) + 40, CurY, 0, 0, pFontBold)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + W2 + 60, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vehicle_No").ToString, LMargin + ClAr(1) + ClAr(2) + W2 + 75, CurY, 0, 0, pFontBold)

        Common_Procedures.Print_To_PrintDocument(e, "DC NO", PageWidth - 140, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, ":", PageWidth - 100, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Pavu_Delivery_No").ToString, PageWidth - 80, CurY, 0, 0, p1Font)

        'CurY = CurY + TxtHgt
        'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        CurY = CurY + TxtHgt - 1.5
        Common_Procedures.Print_To_PrintDocument(e, "COUNT", LMargin + 10, CurY, 0, 0, pFontBold)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + N1 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "", LMargin + N1 + 25, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim((prn_DetAr(prn_DetMxIndx, 8))), LMargin + N1 + 25, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, "MILL NAME", LMargin + ClAr(1) + ClAr(2) + 40, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + W2 + 60, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Mill_Name").ToString, LMargin + ClAr(1) + ClAr(2) + W2 + 75, CurY, 0, 0, pFont)


        CurY = CurY + TxtHgt + 3
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY


        p1Font = New Font("Calibri", 9, FontStyle.Bold)





        Led_Name = prn_HdDt.Rows(0).Item("Ledger_MainName").ToString
        Led_Add1 = prn_HdDt.Rows(0).Item("Ledger_Address1").ToString
        Led_Add2 = prn_HdDt.Rows(0).Item("Ledger_Address2").ToString
        Led_Add3 = prn_HdDt.Rows(0).Item("Ledger_Address3").ToString
        Led_Add4 = prn_HdDt.Rows(0).Item("Ledger_Address4").ToString
        led_phone = prn_HdDt.Rows(0).Item("Ledger_MobileNo").ToString

        If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
            Led_GstNo = "GSTIN" & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
            Led_GstNo = "GSTIN" & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString
        End If


        M1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4)


        p1Font = New Font("Calibri", 11, FontStyle.Bold)

        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height
        Common_Procedures.Print_To_PrintDocument(e, "Issue To :", LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "Delivery To :", LMargin + ClAr(1) + ClAr(2) + (ClAr(3) / 2) + 10, CurY, 0, 0, p1Font)






        del_Name = prn_HdDt.Rows(0).Item("Vendor_Name").ToString
        del_add1 = prn_HdDt.Rows(0).Item("Vendor_Address1").ToString
        del_add2 = prn_HdDt.Rows(0).Item("Vendor_Address2").ToString
        del_add3 = prn_HdDt.Rows(0).Item("Vendor_Address3").ToString
        del_add4 = prn_HdDt.Rows(0).Item("Vendor_Address4").ToString

        If Trim(prn_HdDt.Rows(0).Item("GST_No").ToString) <> "" Then
            nGST_No = "GSTIN : " & prn_HdDt.Rows(0).Item("GST_No").ToString
        End If

        If Trim(prn_HdDt.Rows(0).Item("Vendor_PhoneNo").ToString) <> "" Then
            nVenPhon_No = prn_HdDt.Rows(0).Item("Vendor_PhoneNo").ToString
        End If



        p1Font = New Font("Calibri", 10, FontStyle.Bold)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "M/s." & Led_Name, LMargin + 10, CurY, 0, 0, p1Font)

        If Trim(del_Name) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "M/s." & del_Name, LMargin + ClAr(1) + ClAr(2) + (ClAr(3) / 2) + 10, CurY, 0, 0, p1Font)
        Else
            Common_Procedures.Print_To_PrintDocument(e, "M/s." & Led_Name, LMargin + ClAr(1) + ClAr(2) + (ClAr(3) / 2) + 10, CurY, 0, 0, p1Font)
        End If

        p1Font = New Font("Calibri", 10, FontStyle.Bold)
        CurY = CurY + TxtHgt

        Common_Procedures.Print_To_PrintDocument(e, Led_Add1, LMargin + 10, CurY, 0, 0, p1Font)

        If Trim(del_add1) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, del_add1, LMargin + ClAr(1) + ClAr(2) + (ClAr(3) / 2) + 10, CurY, 0, 0, p1Font)
        Else
            Common_Procedures.Print_To_PrintDocument(e, Led_Add1, LMargin + ClAr(1) + ClAr(2) + (ClAr(3) / 2) + 10, CurY, 0, 0, p1Font)
        End If

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Led_Add2, LMargin + 10, CurY, 0, 0, p1Font)

        If Trim(del_add2) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, del_add2, LMargin + ClAr(1) + ClAr(2) + (ClAr(3) / 2) + 10, CurY, 0, 0, p1Font)
        Else
            Common_Procedures.Print_To_PrintDocument(e, Led_Add2, LMargin + ClAr(1) + ClAr(2) + (ClAr(3) / 2) + 10, CurY, 0, 0, p1Font)
        End If

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Led_Add3, LMargin + 10, CurY, 0, 0, p1Font)

        If Trim(del_add3) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, del_add3, LMargin + ClAr(1) + ClAr(2) + (ClAr(3) / 2) + 10, CurY, 0, 0, p1Font)
        Else
            Common_Procedures.Print_To_PrintDocument(e, Led_Add3, LMargin + ClAr(1) + ClAr(2) + (ClAr(3) / 2) + 10, CurY, 0, 0, p1Font)
        End If


        If Trim(Led_Add4) <> "" Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Led_Add4, LMargin + 10, CurY, 0, 0, p1Font)
        End If

        If Trim(del_add4) <> "" And Trim(Led_Add4) = "" Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, del_add4, LMargin + ClAr(1) + ClAr(2) + (ClAr(3) / 2) + 10, CurY, 0, 0, p1Font)

        ElseIf Trim(del_add4) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, del_add4, LMargin + ClAr(1) + ClAr(2) + (ClAr(3) / 2) + 10, CurY, 0, 0, p1Font)

        Else
            Common_Procedures.Print_To_PrintDocument(e, Led_Add4, LMargin + ClAr(1) + ClAr(2) + (ClAr(3) / 2) + 10, CurY, 0, 0, p1Font)
        End If

        If Trim(led_phone) <> "" Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Phone No : " & led_phone, LMargin + 10, CurY, 0, 0, p1Font)
        End If

        If Trim(nVenPhon_No) <> "" And Trim(led_phone) = "" Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Phone No : " & nVenPhon_No, LMargin + ClAr(1) + ClAr(2) + (ClAr(3) / 2) + 10, CurY, 0, 0, p1Font)
        ElseIf Trim(nVenPhon_No) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "Phone No : " & nVenPhon_No, LMargin + ClAr(1) + ClAr(2) + (ClAr(3) / 2) + 10, CurY, 0, 0, p1Font)
        End If

        If Trim(Led_GstNo) <> "" Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Led_GstNo, LMargin + 10, CurY, 0, 0, p1Font)
        End If


        If Trim(nGST_No) <> "" And Trim(Led_GstNo) = "" Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, nGST_No, LMargin + ClAr(1) + ClAr(2) + (ClAr(3) / 2) + 10, CurY, 0, 0, p1Font)
        ElseIf Trim(nGST_No) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, nGST_No, LMargin + ClAr(1) + ClAr(2) + (ClAr(3) / 2) + 10, CurY, 0, 0, p1Font)
        Else
            Common_Procedures.Print_To_PrintDocument(e, Led_GstNo, LMargin + ClAr(1) + ClAr(2) + (ClAr(3) / 2) + 10, CurY, 0, 0, p1Font)
        End If

        CurY = CurY + TxtHgt + 3
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + (ClAr(3) / 2), LnAr(4), LMargin + ClAr(1) + ClAr(2) + (ClAr(3) / 2), LnAr(3))


        '

        pFont = New Font("Calibri", 9, FontStyle.Regular)
        CurY = CurY + TxtHgt - 15
        Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin, CurY, 2, ClAr(1), p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "SET NO", LMargin + ClAr(1), CurY, 2, ClAr(2), p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "BEAM NO", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "ENDS", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 3, CurY, 2, ClAr(5), p1Font)


        CurY = CurY + TxtHgt + 3
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(5) = CurY
        pFont = New Font("Calibri", 10, FontStyle.Regular)


    End Sub

    Private Sub Printing_Format8_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal pcnt As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim Cmp_Name As String, Cmp_Divi As String
        Dim I As Integer
        Dim M1 As Single
        Dim Del_Add1 As String = "", Del_Add2 As String = "", nGST_No As String = ""
        Dim cnt2 As Integer = 0
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim cmp_UserName As String = ""

        Try

            For I = NoofDets + 1 To NoofItems_PerPage
                CurY = CurY + TxtHgt
            Next

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

            M1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7)
            CurY = CurY + TxtHgt - 15

            If is_LastPage = True Then



                If Val(prn_HdDt.Rows(0).Item("Total_Beam").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Beam").ToString), LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
                End If

                If Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 5, CurY, 1, 0, pFont)
                End If

            End If

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(4))

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(4))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 4, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 4, LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13), LnAr(3))
            CurY = CurY + TxtHgt - 15

            pFont = New Font("Calibri", 9, FontStyle.Regular)



            'Else

            '    Del_Add1 = prn_HdDt.Rows(0).Item("DelAdd1").ToString & "," & prn_HdDt.Rows(0).Item("DelAdd2").ToString
            '    Del_Add2 = prn_HdDt.Rows(0).Item("DelAdd3").ToString & "," & prn_HdDt.Rows(0).Item("DelAdd4").ToString

            '    Common_Procedures.Print_To_PrintDocument(e, "DELIVERY TO   : " & prn_HdDt.Rows(0).Item("DelName").ToString, LMargin + 10, CurY, 0, 0, pFont)
            '    CurY = CurY + TxtHgt
            '    Common_Procedures.Print_To_PrintDocument(e, " " & Trim(Del_Add1), LMargin + 30, CurY, 0, 0, pFont)
            '    CurY = CurY + TxtHgt
            '    Common_Procedures.Print_To_PrintDocument(e, " " & Trim(Del_Add2), LMargin + 30, CurY, 0, 0, pFont)

            'End If
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            If prn_HdDt.Rows(0).Item("Remarks").ToString <> "" Then
                CurY = CurY + TxtHgt - 15
                Common_Procedures.Print_To_PrintDocument(e, "Remarks  : " & prn_HdDt.Rows(0).Item("Remarks").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            End If


            CurY = CurY + TxtHgt
            pFont = New Font("Calibri", 6.3, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, "This is to certify that there is no sale indicated in this delivery and the pavu sized is returned back to party after warping and sizing job work.", LMargin + 6, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(7) = CurY

            If Val(Common_Procedures.User.IdNo) <> 1 Then
                cmp_UserName = Trim(Common_Procedures.User.Name)
                'Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
            End If

            If Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString) <> "" Then
                Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
                '    Cmp_Name = Replace(Cmp_Name, Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString), "")


            Else
                Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString

            End If
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 5, CurY, 1, 0, p1Font)
            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 9, FontStyle.Bold)

            Cmp_Divi = Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Divi, PageWidth - 60, CurY, 1, 0, p1Font)


            CurY = CurY + TxtHgt - 3
            Common_Procedures.Print_To_PrintDocument(e, cmp_UserName, PageWidth - 60, CurY, 1, 0, p1Font)

            pFont = New Font("Calibri", 8, FontStyle.Regular)


            CurY = CurY + TxtHgt + 10

            Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 20, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Checked By", LMargin + ClAr(1) + ClAr(2), CurY, 0, 0, pFont)
            '  Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 250, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 12, FontStyle.Bold)

            CurY = CurY + TxtHgt + 5

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin, LnAr(1))
            e.Graphics.DrawLine(Pens.Black, PageWidth, CurY, PageWidth, LnAr(1))

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub



    Private Sub Printing_Format9(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim EntryCode As String
        Dim NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim cnt As Integer = 0
        Dim LnAr(20) As Single, ClArr(15) As Single
        'Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim PS As Printing.PaperSize
        Dim PCnt As Integer = 0, PrntCnt As Integer = 0
        Dim TpMargin As Single = 0
        Dim Cnt1 As Integer = 0
        Dim LMargin2 As Single
        Dim PageWidth2 As Single
        Dim vyAxis As Single = 0

        PrntCnt = 1

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

        If PrntCnt2ndPageSTS = False Then
            PrntCnt = 2
        End If

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 15
            .Right = 60
            .Top = 20
            .Bottom = 30
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pFont = New Font("Calibri", 9, FontStyle.Regular)

        e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        With PrintDocument1.DefaultPageSettings.PaperSize
            PrintWidth = (.Width - RMargin - LMargin) / 2
            PrintHeight = .Height - TMargin - BMargin
            PageWidth = (.Width / 2) - RMargin
            PageHeight = .Height - BMargin

            LMargin2 = (.Width / 2) + LMargin
            PageWidth2 = .Width - RMargin
        End With
        If PrintDocument1.DefaultPageSettings.Landscape = True Then
            With PrintDocument1.DefaultPageSettings.PaperSize
                PrintWidth = .Height - TMargin - BMargin
                PrintHeight = .Width - RMargin - LMargin
                PageWidth = .Height - TMargin
                PageHeight = .Width - RMargin
            End With
        End If



        NoofItems_PerPage = 11 ' 6

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = 35 : ClArr(2) = 70 : ClArr(3) = 80 : ClArr(4) = 60
        ClArr(5) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4))

        TxtHgt = 17.2 ' 18.5 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        PrntCnt2ndPageSTS = False
        TpMargin = TMargin

        For PCnt = 1 To PrntCnt
            If Val(Common_Procedures.settings.PavuDelivery_Print_2Copy_In_SinglePage) = 1 Then
                If PCnt = 1 Then
                    prn_PageNo1 = prn_PageNo

                    prn_DetIndx1 = prn_DetIndx
                    prn_DetSNo1 = prn_DetSNo
                    prn_NoofBmDets1 = prn_NoofBmDets
                    TpMargin = TMargin

                Else

                    prn_PageNo = prn_PageNo1
                    prn_NoofBmDets = prn_NoofBmDets1
                    prn_DetIndx = prn_DetIndx1
                    prn_DetSNo = prn_DetSNo1

                    TpMargin = 560 + TMargin  ' 600 + TMargin

                End If
            End If

            Try

                If prn_HdDt.Rows.Count > 0 Then

                    Printing_Format9_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TpMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)
                    Printing_Format9_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin2, RMargin, TpMargin, BMargin, PageWidth2, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                    NoofDets = 0

                    CurY = CurY - 10

                    If prn_DetDt.Rows.Count > 0 Then

                        Do While prn_NoofBmDets < prn_DetMxIndx

                            If NoofDets >= NoofItems_PerPage Then

                                If Val(Common_Procedures.settings.PavuDelivery_Print_2Copy_In_SinglePage) = 1 Then

                                    If PCnt = 2 And PrntCnt = 2 And PrntCnt2ndPageSTS = False Then


                                        CurY = CurY + TxtHgt

                                        Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                                        NoofDets = NoofDets + 1

                                        Printing_Format9_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, PCnt, False)

                                        prn_DetIndx = prn_DetIndx + NoofItems_PerPage

                                        e.HasMorePages = True

                                        Return
                                    End If

                                Else

                                    CurY = CurY + TxtHgt

                                    Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                                    NoofDets = NoofDets + 1

                                    Printing_Format5_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, PCnt, False)

                                    prn_DetIndx = prn_DetIndx + NoofItems_PerPage

                                    e.HasMorePages = True

                                    Return

                                End If
                            End If

                            prn_DetIndx = prn_DetIndx + 1

                            If PCnt <> 2 Then

                                If Val(prn_DetAr(prn_DetIndx, 4)) <> 0 Then
                                    If PCnt = 1 And NoofDets < NoofItems_PerPage Then
                                        CurY = CurY + TxtHgt
                                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx, 1))), LMargin + 3, CurY, 0, 0, pFont)
                                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 6)), LMargin + ClArr(1) + 3, CurY, 0, 0, pFont)
                                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx, 2))), LMargin + ClArr(1) + ClArr(2) + 40, CurY, 1, 0, pFont)
                                        Common_Procedures.Print_To_PrintDocument(e, Trim((prn_DetAr(prn_DetIndx, 3))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 10, CurY, 1, 0, pFont)
                                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx, 4).ToString), "##,##,##,##,##0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)

                                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx, 1))), LMargin2 + 3, CurY, 0, 0, pFont)
                                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 6)), LMargin2 + ClArr(1) + 3, CurY, 0, 0, pFont)
                                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx, 2))), LMargin2 + ClArr(1) + ClArr(2) + 40, CurY, 1, 0, pFont)
                                        Common_Procedures.Print_To_PrintDocument(e, Trim((prn_DetAr(prn_DetIndx, 3))), LMargin2 + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 10, CurY, 1, 0, pFont)
                                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx, 4).ToString), "##,##,##,##,##0.00"), LMargin2 + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)


                                    End If
                                    prn_NoofBmDets = prn_NoofBmDets + 1

                                End If

                                If Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 4)) <> 0 Then
                                    If PCnt = 1 And NoofDets < NoofItems_PerPage Then
                                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx, 1))), LMargin + 3, CurY, 0, 0, pFont)
                                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 6)), LMargin + ClArr(1) + 3, CurY, 0, 0, pFont)
                                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx, 2))), LMargin + ClArr(1) + ClArr(2) + 30, CurY, 1, 0, pFont)
                                        Common_Procedures.Print_To_PrintDocument(e, Trim((prn_DetAr(prn_DetIndx, 3))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 3, CurY, 0, 0, pFont)
                                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx, 4).ToString), "##,##,##,##,##0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 3, CurY, 0, 0, pFont)

                                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx, 1))), LMargin2 + 3, CurY, 0, 0, pFont)
                                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 6)), LMargin2 + ClArr(1) + 3, CurY, 0, 0, pFont)
                                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx, 2))), LMargin2 + ClArr(1) + ClArr(2) + 30, CurY, 1, 0, pFont)
                                        Common_Procedures.Print_To_PrintDocument(e, Trim((prn_DetAr(prn_DetIndx, 3))), LMargin2 + ClArr(1) + ClArr(2) + ClArr(3) + 3, CurY, 0, 0, pFont)
                                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx, 4).ToString), "##,##,##,##,##0.00"), LMargin2 + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 3, CurY, 0, 0, pFont)

                                    End If

                                    prn_NoofBmDets = prn_NoofBmDets + 1

                                End If

                                NoofDets = NoofDets + 1

                            End If

                            'If PCnt = 2 Then

                            '    If Val(prn_DetAr(prn_DetIndx, 4)) <> 0 Then
                            '        CurY = CurY + TxtHgt
                            '        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx, 1))), LMargin + 3, CurY, 0, 0, pFont)
                            '        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 6)), LMargin + ClArr(1) + 3, CurY, 0, 0, pFont)
                            '        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx, 2))), LMargin + ClArr(1) + ClArr(2) + 3, CurY, 0, 0, pFont)
                            '        Common_Procedures.Print_To_PrintDocument(e, Trim((prn_DetAr(prn_DetIndx, 3))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 3, CurY, 0, 0, pFont)
                            '        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx, 9).ToString), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 3, CurY, 0, 0, pFont)
                            '        prn_NoofBmDets = prn_NoofBmDets + 1
                            '    End If

                            '    If Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 4)) <> 0 Then
                            '        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 1))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + 7, CurY, 0, 0, pFont)
                            '        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 6)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + 3, CurY, 0, 0, pFont)
                            '        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 2))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + 3, CurY, 0, 0, pFont)
                            '        Common_Procedures.Print_To_PrintDocument(e, Trim((prn_DetAr(prn_DetIndx + NoofItems_PerPage, 3))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) + 3, CurY, 0, 0, pFont)
                            '        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 9).ToString), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) + ClArr(11) + 3, CurY, 0, 0, pFont)
                            '        prn_NoofBmDets = prn_NoofBmDets + 1
                            '    End If
                            '    NoofDets = NoofDets + 1

                            'End If

                        Loop

                    End If



                    vyAxis = CurY
                    Printing_Format9_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, PCnt, True)
                    CurY = vyAxis
                    Printing_Format9_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin2, RMargin, TMargin, BMargin, PageWidth2, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, PCnt, True)

                End If

            Catch ex As Exception

                MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End Try
            If Val(Common_Procedures.settings.PavuDelivery_Print_2Copy_In_SinglePage) = 1 Then

                If PCnt = 1 And PrntCnt = 2 And PrntCnt2ndPageSTS = False Then
                    If prn_DetDt.Rows.Count = cnt + 18 Then
                        PrntCnt2ndPageSTS = True
                        e.HasMorePages = True
                        cnt = 18
                        Return
                    End If
                End If
            End If
            PrntCnt2ndPageSTS = False

        Next PCnt
LOOP10:


        prn_Count = prn_Count + 1

        e.HasMorePages = False

        If Val(prn_TotCopies) > 1 Then
            If prn_Count < Val(prn_TotCopies) Then

                prn_DetIndx = 0
                prn_PageNo = 0
                prn_DetIndx = 0
                prn_PageNo = 0
                prn_NoofBmDets = 0


                e.HasMorePages = True
                Return

            End If

        End If

    End Sub

    Private Sub Printing_Format9_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim da3 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim p1Font As Font

        Dim strHeight As Single
        Dim N1 As Single, M1 As Single
        Dim i As Integer = 0, k As Integer = 0
        Dim Arr(300, 5) As String
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String, Cmp_Add3 As String, Cmp_Add4 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_Email As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim Led_Name As String, Led_Add1 As String, Led_Add2 As String, Led_Add3 As String, Led_Add4 As String
        Dim Led_GstNo As String
        Dim ToAddress As String = ""
        Dim Cnt_Name As String = ""
        'Dim Entry_dt As Date
        Dim CurX As Single = 0
        Dim strWidth As Single = 0
        Dim Vendor_Name As String
        Dim Vendor_Add1 As String = "", Vendor_Add2 As String = "", Vendor_GstNo As String = ""

        PageNo = PageNo + 1

        CurY = TMargin

        If prn_DetMxIndx > (2 * NoofItems_PerPage) Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If

        CurY = TMargin
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


        If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "PH : " & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
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


        CurY = CurY + TxtHgt - 12
        M1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4)

        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        e.Graphics.DrawLine(Pens.Black, LMargin + 5, CurY, PageWidth - 5, CurY)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1 & "," & Cmp_Add2 & "," & Cmp_Add3 & "," & Cmp_Add4, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, LMargin + 5, CurY, 0, CurY, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, PageWidth - 10, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin + 5, CurY, PageWidth - 5, CurY)
        e.Graphics.DrawLine(Pens.Black, LMargin + 5, CurY, LMargin + 5, LnAr(1) + 5)
        e.Graphics.DrawLine(Pens.Black, PageWidth - 5, CurY, PageWidth - 5, LnAr(1) + 5)
        LnAr(2) = CurY

        Led_Name = prn_HdDt.Rows(0).Item("Ledger_MainName").ToString
        If Trim(prn_HdDt.Rows(0).Item("Ledger_Address4").ToString) <> "" Then
            Led_Add1 = prn_HdDt.Rows(0).Item("Ledger_Address4").ToString
        ElseIf Trim(prn_HdDt.Rows(0).Item("Ledger_Address3").ToString) <> "" Then
            Led_Add1 = prn_HdDt.Rows(0).Item("Ledger_Address3").ToString
        ElseIf Trim(prn_HdDt.Rows(0).Item("Ledger_Address2").ToString) <> "" Then
            Led_Add1 = prn_HdDt.Rows(0).Item("Ledger_Address2").ToString
        Else
            Led_Add1 = prn_HdDt.Rows(0).Item("Ledger_Address1").ToString
        End If

        Led_GstNo = ""
        If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
            Led_GstNo = prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString
        End If

        N1 = e.Graphics.MeasureString("DELY TO : ", pFont).Width
        'CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "Dc.No.", LMargin + 5, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + N1 + 10, CurY, 0, 0, pFont)
        p1Font = New Font("Calibri", 10, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "" & prn_HdDt.Rows(0).Item("Pavu_Delivery_No").ToString, LMargin + N1 + 20, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "Date :  " & Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Pavu_Delivery_Date").ToString), "dd-MM-yyyy"), LMargin + ClAr(1) + ClAr(2) + 30, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Ends :  " & prn_DetAr(prn_DetMxIndx, 7), PageWidth - 15, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "To", LMargin + 5, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + N1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Led_Name & IIf(Led_Add1 <> "", ", ", "") & Led_Add1, LMargin + N1 + 20, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        If Trim(Led_GstNo) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "GSTIN : ", LMargin + 5, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(Led_GstNo), LMargin + N1 + 20, CurY, 0, 0, pFont)
        End If

        Vendor_Name = prn_HdDt.Rows(0).Item("Vendor_Name").ToString
        If Trim(prn_HdDt.Rows(0).Item("Vendor_Address4").ToString) <> "" Then
            Vendor_Add1 = prn_HdDt.Rows(0).Item("Vendor_Address4").ToString
        ElseIf Trim(prn_HdDt.Rows(0).Item("Vendor_Address3").ToString) <> "" Then
            Vendor_Add1 = prn_HdDt.Rows(0).Item("Vendor_Address3").ToString
        ElseIf Trim(prn_HdDt.Rows(0).Item("Vendor_Address2").ToString) <> "" Then
            Vendor_Add1 = prn_HdDt.Rows(0).Item("Vendor_Address2").ToString
        Else
            Vendor_Add1 = prn_HdDt.Rows(0).Item("Vendor_Address1").ToString
        End If

        Vendor_GstNo = ""
        If Trim(prn_HdDt.Rows(0).Item("GST_No").ToString) <> "" Then
            Vendor_GstNo = prn_HdDt.Rows(0).Item("GST_No").ToString
        End If

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "DELY TO", LMargin + 5, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + N1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Vendor_Name & IIf(Vendor_Add1 <> "", ", ", "") & Vendor_Add1, LMargin + N1 + 20, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        If Vendor_GstNo <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "GSTIN : ", LMargin + 5, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(Vendor_GstNo), LMargin + N1 + 20, CurY, 0, 0, pFont)
        End If

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        pFont = New Font("Calibri", 9, FontStyle.Regular)
        CurY = CurY + TxtHgt - 15
        Common_Procedures.Print_To_PrintDocument(e, "S.No", LMargin, CurY, 2, ClAr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Set No", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Beam No", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Pieces", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Meter", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)

        'Common_Procedures.Print_To_PrintDocument(e, "S.No", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 3, CurY, 2, ClAr(8), pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "Set No", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "Beam No", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, 2, ClAr(10), pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "Pieces", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, 2, ClAr(11), pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "Meter", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), CurY, 2, ClAr(12), pFont)

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY
        pFont = New Font("Calibri", 10, FontStyle.Regular)

    End Sub

    Private Sub Printing_Format9_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClArr() As Single, ByVal NoofDets As Integer, ByVal pcnt As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim Cmp_Name As String
        Dim I As Integer
        Dim M1 As Single, W1 As Single
        Dim cnt2 As Integer = 0
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        Try

            For I = NoofDets + 1 To NoofItems_PerPage
                CurY = CurY + TxtHgt
            Next

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

            M1 = e.Graphics.MeasureString("Vech No. :", pFont).Width
            CurY = CurY + TxtHgt - 15

            If is_LastPage = True Then

                If (prn_DetMxIndx Mod (NoofItems_PerPage * 2)) <= NoofItems_PerPage Then

                    Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClArr(1) + ClArr(2) - 10, CurY, 1, 0, pFont)
                    If Val(prn_HdDt.Rows(0).Item("Total_Beam").ToString) <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Beam").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) - 10, CurY, 1, 0, pFont)
                    End If
                    If Val(prn_HdDt.Rows(0).Item("Total_Pcs").ToString) <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Pcs").ToString), "#,##,##,##,##0"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 10, CurY, 1, 0, pFont)
                    End If
                    If Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString) <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString), "#,##,##,##,##0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 5, CurY, 1, 0, pFont)
                    End If

                Else

                    Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClArr(1) + ClArr(2) - 10, CurY, 1, 0, pFont)
                    If Val(prn_HdDt.Rows(0).Item("Total_Beam").ToString) <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Beam").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 3, CurY, 0, 0, pFont)
                    End If
                    If Val(prn_HdDt.Rows(0).Item("Total_Pcs").ToString) <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Pcs").ToString), "#,##,##,##,##0"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 5, CurY, 1, 0, pFont)
                    End If
                    If Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString) <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString), "#,##,##,##,##0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) + ClArr(11) + ClArr(12) + ClArr(13) - 5, CurY, 1, 0, pFont)
                    End If

                End If
            End If

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + Clarr(1), CurY, LMargin + Clarr(1), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + Clarr(1) + Clarr(2), CurY, LMargin + Clarr(1) + Clarr(2), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + Clarr(1) + Clarr(2) + Clarr(3), CurY, LMargin + Clarr(1) + Clarr(2) + Clarr(3), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + Clarr(1) + Clarr(2) + Clarr(3) + Clarr(4), CurY, LMargin + Clarr(1) + Clarr(2) + Clarr(3) + Clarr(4), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + Clarr(1) + Clarr(2) + Clarr(3) + Clarr(4) + Clarr(5), CurY, LMargin + Clarr(1) + Clarr(2) + Clarr(3) + Clarr(4) + Clarr(5), LnAr(3))

            CurY = CurY + TxtHgt - 15

            pFont = New Font("Calibri", 9, FontStyle.Regular)

            W1 = Clarr(1) + Clarr(2) + Clarr(3)
            Common_Procedures.Print_To_PrintDocument(e, "Vech No.", LMargin + 5, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vehicle_No").ToString, LMargin + M1 + 20, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Note ", LMargin + 5, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Remarks").ToString, LMargin + M1 + 20, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(7) = CurY

            If Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString) <> "" Then
                Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
                Cmp_Name = Replace(Cmp_Name, Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString), "")
            Else
                Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            End If

            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 5, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt + 5
            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin, LnAr(1))
            e.Graphics.DrawLine(Pens.Black, PageWidth, CurY, PageWidth, LnAr(1))


        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub
    Public Sub New()
        FrmLdSTS = True
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
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

            smstxt = "PAVU DELIVERY" & vbCrLf

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

            If Common_Procedures.settings.CustomerCode = "1282" Then
                da2 = New SqlClient.SqlDataAdapter("select a.Ends_Name, b.Count_Name, count(a.beam_no) as Pavu_Beam, sum(a.meters) as Pavu_Mtrs from  Sizing_Pavu_Delivery_Details a LEFT OUTER JOIN Count_Head b ON a.Count_IdNo = b.Count_IdNo where a.Pavu_Delivery_Code = '" & Trim(NewCode) & "' Group by a.Ends_Name, b.Count_Name Having sum(a.meters) <> 0", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1
                        'If i <> 0 Then
                        smstxt = smstxt & vbCrLf
                        'End If
                        smstxt = smstxt & vbCrLf & "Ends  : " & Trim(dt2.Rows(i).Item("Ends_Name").ToString)
                        smstxt = smstxt & vbCrLf & "Count  : " & Trim(dt2.Rows(i).Item("Count_Name").ToString)
                        smstxt = smstxt & vbCrLf & "Total Pavu  : " & Trim(dt2.Rows(i).Item("Pavu_Beam").ToString)
                        smstxt = smstxt & vbCrLf & "Total Meter : " & Trim(dt2.Rows(i).Item("Pavu_Mtrs").ToString)
                    Next i

                End If
                dt2.Clear()

            Else
                da2 = New SqlClient.SqlDataAdapter("select Total_Beam,Total_Meters from Sizing_Pavu_Delivery_Head where Pavu_Delivery_Code = '" & Trim(NewCode) & "'", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1
                        smstxt = smstxt & vbCrLf & "Total Pavu  : " & Trim(dt2.Rows(i).Item("Total_Beam").ToString)
                        smstxt = smstxt & vbCrLf & "Total Meter : " & Trim(dt2.Rows(i).Item("Total_Meters").ToString)
                    Next i

                End If
                dt2.Clear()

            End If





            smstxt = smstxt & " " & vbCrLf & vbCrLf
            smstxt = smstxt & "Thanks! " & vbCrLf
            smstxt = smstxt & Common_Procedures.Company_IdNoToName(con, Val(lbl_Company.Tag))

            SMS_SenderID = ""
            SMS_Key = ""
            SMS_RouteID = ""
            SMS_Type = ""

            Common_Procedures.get_SMS_Provider_Details(con, Val(lbl_Company.Tag), SMS_SenderID, SMS_Key, SMS_RouteID, SMS_Type)

            If Common_Procedures.settings.CustomerCode = "1282" Then
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

    Private Sub chk_Loaded_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles chk_Loaded.KeyDown
        If e.KeyValue = 38 Then
            txt_Rate.Focus()
        End If

        If e.KeyValue = 40 Then
            If MessageBox.Show("Do you want to save? ", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()
            Else
                dtp_Date.Focus()
            End If


        End If
    End Sub

    Private Sub chk_Loaded_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles chk_Loaded.KeyPress
        If Asc(e.KeyChar) = 13 Then

            If MessageBox.Show("Do you want to save? ", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()
            Else
                dtp_Date.Focus()
            End If


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



    Private Sub txt_DateAndTimeOFSupply_GotFocus(sender As Object, e As System.EventArgs) Handles txt_DateAndTimeOFSupply.GotFocus
        If Trim(txt_DateAndTimeOFSupply.Text) = "" And New_Entry = True Then
            txt_DateAndTimeOFSupply.Text = Format(Now, "hh:mm tt")
        End If
    End Sub

    Private Sub btn_EWayBIll_Generation_Click(sender As Object, e As EventArgs) Handles btn_EWayBIll_Generation.Click
        btn_GENERATEEWB.Enabled = True
        Grp_EWB.Visible = True
        Grp_EWB.BringToFront()
        Grp_EWB.Left = (Me.Width - pnl_Back.Width) / 2 + 135
        Grp_EWB.Top = (Me.Height - pnl_Back.Height) / 2 + 160
    End Sub

    Private Sub btn_Close_EWB_Click(sender As Object, e As EventArgs) Handles btn_Close_EWB.Click
        Grp_EWB.Visible = False
    End Sub

    Private Sub btn_GENERATEEWB_Click(sender As Object, e As EventArgs) Handles btn_GENERATEEWB.Click

        Dim dt1 As New DataTable

        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()

        '   Dim NewCode As String = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        Dim NewCode As String = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Val(lbl_value.Text) = 0 Then
            MessageBox.Show("Invalid Amount", "DOES NOT GENERATE EWB...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If txt_Rate.Enabled And txt_Rate.Visible Then txt_Rate.Focus()
            Exit Sub
        End If


        Dim da As New SqlClient.SqlDataAdapter("Select Electronic_Reference_No from Sizing_Pavu_Delivery_Head where Pavu_Delivery_Code = '" & NewCode & "'", con)
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

        'Dim vSgst As String = 0
        'Dim vCgst As String = 0
        'Dim vIgst As String = 0

        'vSgst = ("a.TotalInvValue" * 5)

        'vSgst = vCgst

        'vIgst = 0

        CMD.CommandText = "Delete from EWB_Head Where InvCode = '" & NewCode & "'"
        CMD.ExecuteNonQuery()



        CMD.CommandText = "Insert into EWB_Head ([SupplyType]  ,[SubSupplyType]  , [SubSupplyDesc]  ,[DocType]  ,	[EWBGenDocNo]                           ,[EWBDocDate]        ,[FromGSTIN]       ,[FromTradeName]  ,[FromAddress1]   ,[FromAddress2]     ,[FromPlace] ," &
                         "[FromPINCode]     ,	[FromStateCode] ,[ActualFromStateCode] ,[ToGSTIN]       ,[ToTradeName],[ToAddress1]      ,[ToAddress2]    ,[ToPlace]       ,[ToPINCode]       ,[ToStateCode] , [ActualToStateCode] ," &
                         "[TransactionType],[OtherValue]                       ,	[Total_value]       ,	[CGST_Value],[SGST_Value],[IGST_Value]     ,	[CessValue],[CessNonAdvolValue],[TransporterID]    ,[TransporterName]," &
                         "[TransportDOCNo] ,[TransportDOCDate]    ,[TotalInvValue]    ,[TransMode]             ," &
                         "[VehicleNo]      ,[VehicleType]   , [InvCode], [ShippedToGSTIN], [ShippedToTradeName] ) " &
                         " " &
                         " " &
                         "  SELECT               'O'              , '4'             ,   'JOB WORK'              ,    'CHL'    , a.Pavu_Delivery_No ,a.Pavu_Delivery_Date          , C.Company_GSTINNo, C.Company_Name   ,C.Company_Address1+C.Company_Address2,c.Company_Address3+C.Company_Address4,C.Company_City ," &
                         " C.Company_PinCode    , FS.State_Code  ,FS.State_Code    ,L.Ledger_GSTINNo, L.Ledger_MainName, (case when a.DeliveryTo_IdNo <> 0 then tDELV.Ledger_Address1+tDELV.Ledger_Address2 else  L.Ledger_Address1+L.Ledger_Address2 end) as deliveryaddress1,  (case when a.DeliveryTo_IdNo <> 0 then tDELV.Ledger_Address3+tDELV.Ledger_Address4 else  L.Ledger_Address3+L.Ledger_Address4 end) as deliveryaddress2, (case when a.DeliveryTo_IdNo <> 0 then tDELV.City_Town else  L.City_Town end) as city_town_name, (case when a.DeliveryTo_IdNo <> 0 then tDELV.Pincode else  L.Pincode end) as pincodee, TS.State_Code, (case when a.DeliveryTo_IdNo <> 0 then TDCS.State_Code else TS.State_Code end) as actual_StateCode," &
                         " 1                    , 0 , a.Approx_Value     , 0 ,  0 , 0   ,   0         ,0                   , t.Ledger_GSTINNo  , t.Ledger_Name ," &
                         " ''        ,''            ,  0        ,     '1'  AS TrMode ," &
                         " a.Vehicle_No, 'R', '" & NewCode & "', tDELV.Ledger_GSTINNo as ShippedTo_GSTIN, tDELV.Ledger_MainName as ShippedTo_LedgerName from Sizing_Pavu_Delivery_Head a inner Join Company_Head C on a.Company_IdNo = C.Company_IdNo " &
                         " Inner Join Ledger_Head L ON a.Ledger_IdNo = L.Ledger_IdNo Left Outer Join Ledger_Head tDELV on a.DeliveryTo_IdNo <> 0 and a.DeliveryTo_IdNo = tDELV.Ledger_IdNo left Outer Join Ledger_Head T on a.Transport_IdNo = T.Ledger_IdNo " &
                         " Left Outer Join State_Head FS On C.Company_State_IdNo = fs.State_IdNo left Outer Join State_Head TS on L.Ledger_State_IdNo = TS.State_IdNo  left Outer Join State_Head TDCS on tDELV.Ledger_State_IdNo = TDCS.State_IdNo " &
                         " where a.Pavu_Delivery_Code = '" & Trim(NewCode) & "'"
        CMD.ExecuteNonQuery()

        'vSgst = 

        CMD.CommandText = " Update EWB_Head Set CGST_Value = ( (Total_value * 5 / 100 ) / 2 ) , SGST_Value = ( (Total_value * 5 / 100 ) / 2 ) , TotalInvValue = ( Total_value + SGST_Value + CGST_Value )  where InvCode = '" & Trim(NewCode) & "' "
        CMD.ExecuteNonQuery()


        CMD.CommandText = " Update EWB_Head Set TotalInvValue = ( Total_value + SGST_Value + CGST_Value )  where InvCode = '" & Trim(NewCode) & "' "
        CMD.ExecuteNonQuery()


        CMD.CommandText = "Delete from EWB_Details Where InvCode = '" & NewCode & "'"
        CMD.ExecuteNonQuery()



        da = New SqlClient.SqlDataAdapter(" Select  Ch.Count_Name, (ch.Count_Name + ' - WARP'  ) , IG.Item_HSN_Code , IG.Item_GST_Percentage , sum(SD.Approx_Value) As TaxableAmt,sum(SD.Total_Meters) as Qty, 1 , 'MTR' AS Units " &
                                          " from Sizing_Pavu_Delivery_Head SD Inner Join Sizing_Pavu_Delivery_Details Pd On Pd.Pavu_Delivery_Code = Sd.Pavu_Delivery_Code  INNER JOIN Count_Head Ch On Ch.Count_Idno = pd.Count_Idno " &
                                          " Inner Join ItemGroup_Head IG on Ch.ItemGroup_IdNo = IG.ItemGroup_IdNo Where SD.Pavu_Delivery_Code = '" & Trim(NewCode) & "' Group By " &
                                          " Ch.Count_Name,IG.ItemGroup_Name,IG.Item_HSN_Code, IG.Item_GST_Percentage ", con)
        dt1 = New DataTable
        da.Fill(dt1)


        If dt1.Rows.Count > 0 Then
            For I = 0 To dt1.Rows.Count - 1

                CMD.CommandText = "Insert into EWB_Details ([SlNo]                               , [Product_Name]           ,	[Product_Description]     ,	[HSNCode]                 ,	[Quantity]                                ,[QuantityUnit] ,  Tax_Perc                         ,	[CessRate]         ,	[CessNonAdvol]  ,	[TaxableAmount]               ,InvCode) " &
                                  " values                 (" & dt1.Rows(I).Item(6).ToString & ",'" & dt1.Rows(I).Item(0) & "', '" & dt1.Rows(I).Item(1) & "', '" & dt1.Rows(I).Item(2) & "', " & dt1.Rows(I).Item(5).ToString & ",'MTR'          ," & dt1.Rows(I).Item(3).ToString & ", 0                  , 0                   ," & dt1.Rows(I).Item(4) & ",'" & NewCode & "')"

                CMD.ExecuteNonQuery()

            Next
        End If


        'da = New SqlClient.SqlDataAdapter(" Select Ch.Count_Name, (Ch.Count_Name + ' - WARP'  ) , IG.Item_HSN_Code , IG.Item_GST_Percentage , (sum(SD.Pavu_Meters)*SD.Rate) As TaxableAmt, sum(SD.Pavu_Meters) as Qty, 201 as SlNo, 'MTR' AS Units " &
        '                                  " from Sizing_Pavu_Delivery_Head SD  INNER JOIN Count_Head Ch On Ch.Count_Idno = I.Count_Idno " &
        '                                  " Inner Join ItemGroup_Head IG on Ch.ItemGroup_IdNo = IG.ItemGroup_IdNo Where SD.Pavu_Delivery_Code = '" & Trim(NewCode) & "' and SD.Pavu_Meters > 0 Group By " &
        '                                  " Ch.Count_Name,IG.ItemGroup_Name,IG.Item_HSN_Code, IG.Item_GST_Percentage,SD.Rate ", con)
        'dt1 = New DataTable
        'da.Fill(dt1)
        'If dt1.Rows.Count > 0 Then
        '    For I = 0 To dt1.Rows.Count - 1

        '        CMD.CommandText = "Insert into EWB_Details ( [SlNo]                              ,     [Product_Name]           ,	[Product_Description]     ,	[HSNCode]                     ,	[Quantity]                          ,[QuantityUnit] ,  Tax_Perc                           ,	[CessRate]         ,	[CessNonAdvol]  ,	[TaxableAmount]          ,      InvCode     ) " &
        '                          " values                 ( " & dt1.Rows(I).Item(6).ToString & ", '" & dt1.Rows(I).Item(0) & "', '" & dt1.Rows(I).Item(1) & "', '" & dt1.Rows(I).Item(2) & "', " & dt1.Rows(I).Item(5).ToString & ", 'MTR'         , " & dt1.Rows(I).Item(3).ToString & ", 0                  , 0                   ," & dt1.Rows(I).Item(4) & ", '" & NewCode & "')"
        '        CMD.ExecuteNonQuery()

        '    Next
        'End If


        btn_GENERATEEWB.Enabled = False

        Dim ewb As New EWB(Val(lbl_Company.Tag))
        EWB.GenerateEWB(NewCode, con, rtbEWBResponse, txt_EWBNo, "Sizing_Pavu_Delivery_Head", "Electronic_Reference_No", "Pavu_Delivery_Code", Pk_Condition)



    End Sub

    Private Sub btn_CheckConnectivity_Click(sender As Object, e As EventArgs) Handles btn_CheckConnectivity.Click
        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()

        'Dim einv As New eInvoice(Val(lbl_Company.Tag))
        'einv.GetAuthToken(rtbEWBResponse)

        Dim ewb As New EWB(Val(lbl_Company.Tag))
        EWB.GetAuthToken(rtbEWBResponse)
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

        EWB.CancelEWB(txt_ElectronicRefNo.Text, NewCode, con, rtbEWBResponse, txt_EWBNo, "Sizing_Pavu_Delivery_Head", "Electronic_Reference_No", "Pavu_Delivery_Code")

    End Sub
    Private Sub txt_EWBNo_TextChanged(sender As Object, e As EventArgs) Handles txt_EWBNo.TextChanged
        txt_ElectronicRefNo.Text = txt_EWBNo.Text
    End Sub

    Private Sub btn_PDF_Click(sender As Object, e As EventArgs) Handles btn_PDF.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        Print_PDF_Status = True
        EMAIL_Status = False
        WHATSAPP_Status = False
        print_record()
        'Print_PDF_Status = False
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

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        save_record()
        If Trim(UCase(LastNo)) = Trim(UCase(lbl_DcNo.Text)) Then
            Timer1.Enabled = False
            SaveAll_STS = False
            MessageBox.Show("All entries saved sucessfully", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Else
            movenext_record()

        End If
    End Sub

    Private Sub btn_Selection_CausesValidationChanged(sender As Object, e As EventArgs) Handles btn_Selection.CausesValidationChanged

    End Sub
    Private Sub cbo_Delivered_KeyUp(sender As Object, e As KeyEventArgs) Handles cbo_Delivered.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            Dim f As New Delivery_Party_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Delivered.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub Printing_Format10(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim EntryCode As String
        Dim NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim cnt As Integer = 0
        Dim LnAr(20) As Single, ClArr(15) As Single
        'Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim PS As Printing.PaperSize
        Dim PCnt As Integer = 0, PrntCnt As Integer = 0
        Dim TpMargin As Single = 0
        Dim Cnt1 As Integer = 0


        PrntCnt = 1

        If Val(Common_Procedures.settings.PavuDelivery_Print_2Copy_In_SinglePage) = 1 Then
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
                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                    PS = PrintDocument1.PrinterSettings.PaperSizes(I)
                    PrintDocument1.DefaultPageSettings.PaperSize = PS
                    e.PageSettings.PaperSize = PS
                    Exit For
                End If
            Next

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

        pFont = New Font("Calibri", 10, FontStyle.Regular)

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

        NoofItems_PerPage = 11 ' 6

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}


        ClArr(1) = Val(25) : ClArr(2) = 40 : ClArr(3) = 55 : ClArr(4) = 55 : ClArr(5) = 70 : ClArr(6) = 40 : ClArr(7) = 100
        ClArr(8) = 30 : ClArr(9) = 40 : ClArr(10) = 55 : ClArr(11) = 55 : ClArr(12) = 70 : ClArr(13) = 40
        ClArr(14) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) + ClArr(11) + ClArr(12) + ClArr(13))


        TxtHgt = 17.2 ' 18.5 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        PrntCnt2ndPageSTS = False
        TpMargin = TMargin

        For PCnt = 1 To PrntCnt
            If Val(Common_Procedures.settings.PavuDelivery_Print_2Copy_In_SinglePage) = 1 Then
                If PCnt = 1 Then
                    prn_PageNo1 = prn_PageNo

                    prn_DetIndx1 = prn_DetIndx
                    prn_DetSNo1 = prn_DetSNo
                    prn_NoofBmDets1 = prn_NoofBmDets
                    TpMargin = TMargin

                Else

                    prn_PageNo = prn_PageNo1
                    prn_NoofBmDets = prn_NoofBmDets1
                    prn_DetIndx = prn_DetIndx1
                    prn_DetSNo = prn_DetSNo1

                    TpMargin = 560 + TMargin  ' 600 + TMargin

                End If
            End If

            Try

                If prn_HdDt.Rows.Count > 0 Then

                    Printing_Format10_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TpMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                    NoofDets = 0

                    CurY = CurY - 10

                    If prn_DetDt.Rows.Count > 0 Then


                        Do While prn_NoofBmDets < prn_DetMxIndx
                            If NoofDets >= NoofItems_PerPage Then
                                If Val(Common_Procedures.settings.PavuDelivery_Print_2Copy_In_SinglePage) = 1 Then

                                    If PCnt = 2 And PrntCnt = 2 And PrntCnt2ndPageSTS = False Then


                                        CurY = CurY + TxtHgt

                                        Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                                        NoofDets = NoofDets + 1

                                        Printing_Format10_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, PCnt, False)

                                        'prn_DetIndx = prn_DetIndx + NoofItems_PerPage

                                        e.HasMorePages = True

                                        Return
                                    End If

                                Else

                                    CurY = CurY + TxtHgt

                                    Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                                    NoofDets = NoofDets + 1

                                    Printing_Format10_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, PCnt, False)

                                    'prn_DetIndx = prn_DetIndx + NoofItems_PerPage

                                    e.HasMorePages = True

                                    Return

                                End If
                            End If

                            prn_DetIndx = prn_DetIndx + 1

                            If PCnt <> 2 Then

                                If Val(prn_DetAr(prn_DetIndx, 4)) <> 0 Then
                                    If PCnt = 1 And NoofDets < NoofItems_PerPage Then
                                        CurY = CurY + TxtHgt
                                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx, 1))), LMargin + 10, CurY, 0, 0, pFont)
                                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 6)), LMargin + ClArr(1) + ClArr(8) + 10, CurY, 0, 0, pFont)
                                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx, 2))), LMargin + ClArr(1) + ClArr(8) + ClArr(2) + ClArr(9) + 10, CurY, 0, 0, pFont)
                                        Common_Procedures.Print_To_PrintDocument(e, Trim((prn_DetAr(prn_DetIndx, 7))), LMargin + ClArr(1) + ClArr(8) + ClArr(2) + ClArr(9) + ClArr(3) + ClArr(10) + 10, CurY, 0, 0, pFont)
                                        Common_Procedures.Print_To_PrintDocument(e, Trim((prn_DetAr(prn_DetIndx, 8))), LMargin + ClArr(1) + ClArr(8) + ClArr(2) + ClArr(9) + ClArr(3) + ClArr(10) + ClArr(4) + ClArr(11) + 10, CurY, 0, 0, pFont)
                                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx, 4)), "#########0.00"), LMargin + ClArr(1) + ClArr(8) + ClArr(2) + ClArr(9) + ClArr(3) + ClArr(10) + ClArr(4) + ClArr(11) + ClArr(5) + ClArr(12) + ClArr(6) + ClArr(13) - 10, CurY, 1, 0, pFont)
                                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 5)), LMargin + ClArr(1) + ClArr(8) + ClArr(2) + ClArr(9) + ClArr(3) + ClArr(10) + ClArr(4) + ClArr(11) + ClArr(5) + ClArr(12) + ClArr(6) + ClArr(13) + 10, CurY, 0, 0, pFont)

                                    End If
                                    prn_NoofBmDets = prn_NoofBmDets + 1

                                End If

                                'If Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 4)) <> 0 Then
                                '    If PCnt = 1 And NoofDets < NoofItems_PerPage Then
                                '        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 1))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + 7, CurY, 0, 0, pFont)
                                '        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 6)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + 3, CurY, 0, 0, pFont)
                                '        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 2))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + 3, CurY, 0, 0, pFont)
                                '        Common_Procedures.Print_To_PrintDocument(e, Trim((prn_DetAr(prn_DetIndx + NoofItems_PerPage, 7))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) + 3, CurY, 0, 0, pFont)
                                '        Common_Procedures.Print_To_PrintDocument(e, Trim((prn_DetAr(prn_DetIndx + NoofItems_PerPage, 8))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) + ClArr(11) + 3, CurY, 0, 0, pFont)
                                '        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 4)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) + ClArr(11) + ClArr(12) + ClArr(13) - 5, CurY, 1, 0, pFont)
                                '        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1263" Then '---- Meenashi Sizing (Kapati)
                                '            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 9)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) + ClArr(11) + ClArr(12) + ClArr(13) + 3, CurY, 0, 0, pFont)
                                '        Else
                                '            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 5)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) + ClArr(11) + ClArr(12) + ClArr(13) + 3, CurY, 0, 0, pFont)
                                '        End If

                                '    End If

                                '    prn_NoofBmDets = prn_NoofBmDets + 1

                                'End If

                                NoofDets = NoofDets + 1

                            End If

                            If PCnt = 2 Then
                                If Val(prn_DetAr(prn_DetIndx, 4)) <> 0 Then
                                    CurY = CurY + TxtHgt

                                    Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx, 1))), LMargin + 10, CurY, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 6)), LMargin + ClArr(1) + ClArr(8) + 10, CurY, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx, 2))), LMargin + ClArr(1) + ClArr(8) + ClArr(2) + ClArr(9) + 10, CurY, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Trim((prn_DetAr(prn_DetIndx, 7))), LMargin + ClArr(1) + ClArr(8) + ClArr(2) + ClArr(9) + ClArr(3) + ClArr(10) + 10, CurY, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Trim((prn_DetAr(prn_DetIndx, 8))), LMargin + ClArr(1) + ClArr(8) + ClArr(2) + ClArr(9) + ClArr(3) + ClArr(10) + ClArr(4) + ClArr(11) + 10, CurY, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx, 4)), "#########0.00"), LMargin + ClArr(1) + ClArr(8) + ClArr(2) + ClArr(9) + ClArr(3) + ClArr(10) + ClArr(4) + ClArr(11) + ClArr(5) + ClArr(12) + ClArr(6) + ClArr(13) - 10, CurY, 1, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 5)), LMargin + ClArr(1) + ClArr(8) + ClArr(2) + ClArr(9) + ClArr(3) + ClArr(10) + ClArr(4) + ClArr(11) + ClArr(5) + ClArr(12) + ClArr(6) + ClArr(13) + 10, CurY, 0, 0, pFont)

                                    prn_NoofBmDets = prn_NoofBmDets + 1

                                End If

                                'If Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 4)) <> 0 Then

                                '    Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 1))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + 7, CurY, 0, 0, pFont)
                                '    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 6)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + 3, CurY, 0, 0, pFont)
                                '    Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 2))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + 3, CurY, 0, 0, pFont)
                                '    Common_Procedures.Print_To_PrintDocument(e, Trim((prn_DetAr(prn_DetIndx + NoofItems_PerPage, 7))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) + 3, CurY, 0, 0, pFont)
                                '    Common_Procedures.Print_To_PrintDocument(e, Trim((prn_DetAr(prn_DetIndx + NoofItems_PerPage, 8))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) + ClArr(11) + 3, CurY, 0, 0, pFont)
                                '    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 4)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) + ClArr(11) + ClArr(12) + ClArr(13) - 3, CurY, 1, 0, pFont)
                                '    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1263" Then
                                '        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 9)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) + ClArr(11) + ClArr(12) + ClArr(13) + 3, CurY, 0, 0, pFont)
                                '    Else
                                '        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 5)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) + ClArr(11) + ClArr(12) + ClArr(13) + 3, CurY, 0, 0, pFont)
                                '    End If



                                '    prn_NoofBmDets = prn_NoofBmDets + 1

                                'End If

                                NoofDets = NoofDets + 1

                            End If
                        Loop
                    End If

                    Printing_Format10_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, PCnt, True)

                End If

            Catch ex As Exception

                MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End Try
            If Val(Common_Procedures.settings.PavuDelivery_Print_2Copy_In_SinglePage) = 1 Then

                If PCnt = 1 And PrntCnt = 2 And PrntCnt2ndPageSTS = False Then
                    If prn_DetDt.Rows.Count = cnt + 18 Then
                        PrntCnt2ndPageSTS = True
                        e.HasMorePages = True
                        cnt = 18
                        Return
                    End If
                End If
            End If
            PrntCnt2ndPageSTS = False

        Next PCnt
LOOP10:


        prn_Count = prn_Count + 1

        e.HasMorePages = False

        If Val(prn_TotCopies) > 1 Then
            If prn_Count < Val(prn_TotCopies) Then

                prn_DetIndx = 0
                prn_PageNo = 0
                prn_DetIndx = 0
                prn_PageNo = 0
                prn_NoofBmDets = 0


                e.HasMorePages = True
                Return

            End If

        End If

    End Sub

    Private Sub Printing_Format10_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim da3 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim p1Font As Font

        Dim strHeight As Single
        Dim W1 As Single, W2 As Single, W3 As Single, N1 As Single, M1 As Single
        Dim i As Integer, k As Integer = 0
        Dim Arr(300, 5) As String
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String, Cmp_Add3 As String, Cmp_Add4 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_Email As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim Led_Name As String, Led_Add1 As String, Led_Add2 As String, Led_Add3 As String, Led_Add4 As String
        Dim Led_GstNo As String, Led_PanNo As String
        Dim Hsn_Code As String = ""
        Dim Cnt_Name As String = ""
        'Dim Entry_dt As Date
        Dim CurX As Single = 0
        Dim strWidth As Single = 0
        Dim Ledname1 As String
        Dim Ledname2 As String
        'Dim ItmNm1 As String, ItmNm2 As String

        PageNo = PageNo + 1

        CurY = TMargin

        If prn_DetMxIndx > (2 * NoofItems_PerPage) Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If

        'da2 = New SqlClient.SqlDataAdapter("select DISTINCT(setcode_forSelection) from Stock_SizedPavu_Processing_Details where Pavu_Delivery_Code = '" & Trim(EntryCode) & "' Order by setcode_forSelection", con)
        'dt3 = New DataTable
        'da2.Fill(dt3)

        'vSetNo = ""
        'If dt3.Rows.Count > 0 Then
        '    For i = 0 To dt3.Rows.Count - 1
        '        k = InStr(1, dt3.Rows(i).Item("setcode_forSelection").ToString, "/")
        '        vSetNo = vSetNo & IIf(Trim(vSetNo) <> "", ", ", "") & Microsoft.VisualBasic.Left(dt3.Rows(i).Item("setcode_forSelection").ToString, k - 1)
        '    Next i
        'End If
        'dt3.Dispose()
        Hsn_Code = ""
        Cnt_Name = dgv_Details.Rows(0).Cells(8).Value
        da3 = New SqlClient.SqlDataAdapter("select a.*, b.* from Stock_SizedPavu_Processing_Details a LEFT OUTER JOIN Count_Head b ON a.Count_IdNo = b.Count_IdNo where a.Pavu_Delivery_Code = '" & Trim(EntryCode) & "' and b.Count_Name = '" & Trim(Cnt_Name) & "' Order by a.sl_no", con)
        dt4 = New DataTable
        da3.Fill(dt4)


        If dt4.Rows.Count > 0 Then
            Hsn_Code = dt4.Rows(0).Item("HSN_Code").ToString

            'For i = 0 To dt4.Rows.Count - 1
            '    'k = InStr(1, dt4.Rows(i).Item("Count_Hsn_Code").ToString, "/")
            '    'Hsn_Code = Hsn_Code & IIf(Trim(Hsn_Code) <> "", ", ", "") & Microsoft.VisualBasic.Left(dt4.Rows(i).Item("Count_Hsn_Code").ToString, k - 1)
            '    ' Cnt_Name = dt4.Rows(i).Item("Count_Name").ToString
            '    'Hsn_Code = dt4.Rows(0).Item("Count_Hsn_Code").ToString
            'Next
        End If
        dt4.Clear()
        dt4.Dispose()

        CurY = TMargin
        CurY = CurY + 5
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "SIZED PAVU DELIVERY NOTE", LMargin, CurY - TxtHgt, 2, PrintWidth, p1Font)
        ' strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = "" : Cmp_Add3 = "" : Cmp_Add4 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_Email = ""
        Cmp_StateCap = "" : Cmp_StateNm = "" : Cmp_StateCode = "" : Cmp_GSTIN_Cap = "" : Cmp_GSTIN_No = ""

        Led_Name = "" : Led_Add1 = "" : Led_Add2 = "" : Led_Add3 = "" : Led_Add4 = "" : Led_GstNo = ""

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString

        If Common_Procedures.settings.CustomerCode = "1078" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then
            Cmp_Add1 = prn_HdDt.Rows(0).Item("Sizing_Address1").ToString
            Cmp_Add2 = prn_HdDt.Rows(0).Item("Sizing_Address2").ToString
            Cmp_Add3 = prn_HdDt.Rows(0).Item("Sizing_Address3").ToString
            Cmp_Add4 = prn_HdDt.Rows(0).Item("Sizing_Address4").ToString

            If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
                Cmp_PhNo = "PHONE NO.:" & prn_HdDt.Rows(0).Item("Sizing_PhoneNo").ToString
            End If
            If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
                Cmp_Email = "E-mail : " & Trim(prn_HdDt.Rows(0).Item("Sizing_EMail").ToString)
            End If

        Else

            Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString
            Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address2").ToString
            Cmp_Add3 = prn_HdDt.Rows(0).Item("Company_Address3").ToString
            Cmp_Add4 = prn_HdDt.Rows(0).Item("Company_Address4").ToString

            If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
                Cmp_PhNo = "PHONE NO.:" & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
            End If
            If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
                Cmp_Email = "E-mail : " & Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString)
            End If


        End If



        If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
            Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_panNo").ToString) <> "" Then
            Cmp_CstNo = "PAN NO.: " & prn_HdDt.Rows(0).Item("Company_panNo").ToString
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
            Cmp_GSTIN_No = "GSTIN" & prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
        End If
        If Trim(UCase(Common_Procedures.User.Type)) = "UNACCOUNT" And (Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1263") Then
            Led_Name = prn_HdDt.Rows(0).Item("Ledger_MainName").ToString : Led_Add1 = "" : Led_Add2 = "" : Led_Add3 = "" : Led_Add4 = "" : Led_GstNo = ""
        Else
            Led_Name = prn_HdDt.Rows(0).Item("Ledger_MainName").ToString
            Led_Add1 = prn_HdDt.Rows(0).Item("Ledger_Address1").ToString
            Led_Add2 = prn_HdDt.Rows(0).Item("Ledger_Address2").ToString
            Led_Add3 = prn_HdDt.Rows(0).Item("Ledger_Address3").ToString & "," & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString
            Led_Add4 = prn_HdDt.Rows(0).Item("Ledger_Address4").ToString

            If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then

                Led_GstNo = "GSTIN" & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString

                Led_PanNo = "PAN NO : " & prn_HdDt.Rows(0).Item("PAN_no").ToString
            End If
        End If
        CurY = CurY + TxtHgt - 10

        M1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7)

        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin + 10, CurY, 0, 0, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height
        Common_Procedures.Print_To_PrintDocument(e, "PAVU SIZED TO :", LMargin + M1 + 10, CurY, 0, 0, pFont)
        Dim cury2 As Single
        cury2 = CurY
        ' p1Font = New Font("Calibri", 9, FontStyle.Regular)

        'If Trim(prn_HdDt.Rows(0).Item("Company_Name").ToString) <> "" Then
        '    CmpName1 = Trim(prn_HdDt.Rows(0).Item("Company_Name").ToString)
        'Else
        Ledname1 = Trim(prn_HdDt.Rows(0).Item("Ledger_MainName").ToString)
        '  End If

        Ledname2 = ""

        If Len(Ledname1) > 40 Then
            For i = 40 To 1 Step -1
                If Mid$(Trim(Ledname1), i, 1) = " " Or Mid$(Trim(Ledname1), i, 1) = "," Or Mid$(Trim(Ledname1), i, 1) = "." Or Mid$(Trim(Ledname1), i, 1) = "-" Or Mid$(Trim(Ledname1), i, 1) = "/" Or Mid$(Trim(Ledname1), i, 1) = "_" Or Mid$(Trim(Ledname1), i, 1) = "(" Or Mid$(Trim(Ledname1), i, 1) = ")" Or Mid$(Trim(Ledname1), i, 1) = "\" Or Mid$(Trim(Ledname1), i, 1) = "[" Or Mid$(Trim(Ledname1), i, 1) = "]" Or Mid$(Trim(Ledname1), i, 1) = "{" Or Mid$(Trim(Ledname1), i, 1) = "}" Then Exit For
            Next i
            If i = 0 Then i = 40
            Ledname2 = Microsoft.VisualBasic.Right(Trim(Ledname1), Len(Ledname1) - i)
            Ledname1 = Microsoft.VisualBasic.Left(Trim(Ledname1), i - 1)
        End If


        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin + 10, CurY, 0, 0, pFont)

        cury2 = cury2 + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "M/s." & Ledname1, LMargin + M1 + 10, cury2, 0, 0, p1Font)

        If Trim(Ledname2) <> "" Then
            cury2 = cury2 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(Ledname2), LMargin + M1 + 10, cury2, 0, 0, p1Font)
            'NoofDets = NoofDets + 1
        End If

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin + 10, CurY, 0, 0, pFont)
        cury2 = cury2 + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Led_Add1, LMargin + M1 + 10, cury2, 0, 0, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add3, LMargin + 10, CurY, 0, 0, pFont)
        cury2 = cury2 + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Led_Add2, LMargin + M1 + 10, cury2, 0, 0, pFont)
        If Trim(Cmp_Add4) <> "" Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Add4, LMargin + 10, CurY, 0, 0, pFont)
        End If
        If Trim(Led_Add3) <> "" Then
            cury2 = cury2 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Led_Add3, LMargin + M1 + 10, cury2, 0, 0, pFont)
        End If
        'CurY = CurY + TxtHgt
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, Led_Add4, LMargin + M1 + 10, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No & "    " & Cmp_CstNo, LMargin + 10, CurY, 0, 0, pFont)
        cury2 = cury2 + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Led_GstNo & "  " & Led_PanNo, LMargin + M1 + 10, cury2, 0, 0, pFont)
        If CurY > cury2 Then
            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(2) = CurY
        Else
            cury2 = cury2 + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, cury2, PageWidth, cury2)
            LnAr(2) = cury2
        End If

        'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        'LnAr(2) = CurY
        e.Graphics.DrawLine(Pens.Black, LMargin + M1, LnAr(2), LMargin + M1, LnAr(1))


        ' CurY = CurY + TxtHgt - 12



        ' Try

        N1 = e.Graphics.MeasureString("DATE & TIME  :", pFont).Width
        W1 = e.Graphics.MeasureString("HSC CODE  :", pFont).Width
        W2 = e.Graphics.MeasureString("VAN NO   :", pFont).Width
        W3 = e.Graphics.MeasureString("APPROX VALUE  :", pFont).Width

        M1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7)

        CurY = cury2

        CurY = CurY + TxtHgt - 11
        p1Font = New Font("Calibri", 10, FontStyle.Bold)


        Common_Procedures.Print_To_PrintDocument(e, "DC.NO", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
        p1Font = New Font("Calibri", 10, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Pavu_Delivery_No").ToString, LMargin + W1 + 25, CurY, 0, 0, p1Font)
        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1078" Then '---- Kalaimagal Sizing (Palladam)
        Common_Procedures.Print_To_PrintDocument(e, "DATE & TIME", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + (ClAr(4) \ 2) + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + (ClAr(4) \ 2) + N1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Pavu_Delivery_Date").ToString), "dd-MM-yyyy") & " & " & (prn_HdDt.Rows(0).Item("Date_And_Time_Of_Supply").ToString).ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + (ClAr(4) \ 2) + N1 + 20, CurY, 0, 0, pFont)
        'End If
        Common_Procedures.Print_To_PrintDocument(e, "VAN NO", LMargin + M1 + 20, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vehicle_No").ToString, LMargin + M1 + W1 + 25, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, "APPROX VALUE", LMargin + M1 + ClAr(6) + ClAr(7) + ClAr(8) + 15, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + ClAr(6) + ClAr(7) + ClAr(8) + W3 + 15, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Approx_Value").ToString, LMargin + M1 + ClAr(6) + ClAr(7) + ClAr(8) + W3 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt

        Common_Procedures.Print_To_PrintDocument(e, "HSN CODE", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, (Hsn_Code), LMargin + W1 + 25, CurY, 0, 0, pFont)

        p1Font = New Font("Calibri", 10, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "SAC CODE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + (ClAr(4) \ 2) + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + (ClAr(4) \ 2) + N1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, " 998821", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + (ClAr(4) \ 2) + N1 + 20, CurY, 0, 0, p1Font)

        Common_Procedures.Print_To_PrintDocument(e, "MILL NAME", LMargin + M1 + 20 - 70, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10 - 70, CurY, 0, 0, pFont)
        If prn_DetDt.Rows.Count > 0 Then
            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(0).Item("set_Millname").ToString, LMargin + M1 + W2 + 25 - 70, CurY, 0, 0, pFont)
        End If


        Common_Procedures.Print_To_PrintDocument(e, "TRANSPORT", LMargin + M1 + ClAr(6) + ClAr(7) + ClAr(8) + 15, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + ClAr(6) + ClAr(7) + ClAr(8) + W3 + 15, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Transport_Name").ToString, LMargin + M1 + ClAr(6) + ClAr(7) + ClAr(8) + W3 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt + 2
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        pFont = New Font("Calibri", 9, FontStyle.Regular)
        CurY = CurY + TxtHgt - 15

        Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin, CurY, 2, ClAr(1) + ClAr(8), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "SET NO", LMargin + ClAr(1) + ClAr(8), CurY, 2, ClAr(2) + ClAr(9), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "BEAM NO", LMargin + ClAr(1) + ClAr(8) + ClAr(2) + ClAr(9), CurY, 2, ClAr(3) + ClAr(10), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "ENDS", LMargin + ClAr(1) + ClAr(8) + ClAr(2) + ClAr(9) + ClAr(3) + ClAr(10), CurY, 2, ClAr(4) + ClAr(11), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "COUNT", LMargin + ClAr(1) + ClAr(8) + ClAr(2) + ClAr(9) + ClAr(3) + ClAr(10) + ClAr(4) + ClAr(11), CurY, 2, ClAr(5) + ClAr(12), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(8) + ClAr(2) + ClAr(9) + ClAr(3) + ClAr(10) + ClAr(4) + ClAr(11) + ClAr(5) + ClAr(12) + 3, CurY, 2, ClAr(6) + ClAr(13), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "DELIVERY PARTY", LMargin + ClAr(1) + ClAr(8) + ClAr(2) + ClAr(9) + ClAr(3) + ClAr(10) + ClAr(4) + ClAr(11) + ClAr(5) + ClAr(12) + ClAr(6) + ClAr(13), CurY, 2, ClAr(7) + ClAr(14), pFont)

        'Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 3, CurY, 2, ClAr(8), pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "SET NO", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "BEAM NO", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, 2, ClAr(10), pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "ENDS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, 2, ClAr(11), pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "COUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), CurY, 2, ClAr(12), pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), CurY, 2, ClAr(13), pFont)
        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1263" Then
        '    Common_Procedures.Print_To_PrintDocument(e, "MARK", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13), CurY, 2, ClAr(14), pFont)
        'Else
        '    Common_Procedures.Print_To_PrintDocument(e, "DELIVERY PARTY", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13), CurY, 2, ClAr(14), pFont)
        'End If

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY
        pFont = New Font("Calibri", 10, FontStyle.Regular)
        ' Catch ex As Exception

        'MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        '  End Try

    End Sub

    Private Sub Printing_Format10_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal pcnt As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim Cmp_Name As String
        Dim I As Integer
        Dim M1 As Single
        Dim Del_Add1 As String = "", Del_Add2 As String = ""
        Dim cnt2 As Integer = 0
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        Try

            For I = NoofDets + 1 To NoofItems_PerPage
                CurY = CurY + TxtHgt
            Next

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

            M1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7)
            CurY = CurY + TxtHgt - 15

            If is_LastPage = True Then


                If Val(prn_HdDt.Rows(0).Item("Total_Beam").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Beam").ToString), LMargin + ClAr(1) + ClAr(8) + ClAr(2) + ClAr(9) + 10, CurY, 0, 0, pFont)
                End If

                If Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString), "#########0.00"), LMargin + ClAr(1) + ClAr(8) + ClAr(2) + ClAr(9) + ClAr(3) + ClAr(10) + ClAr(4) + ClAr(11) + ClAr(5) + ClAr(12) + ClAr(6) + ClAr(13) - 5, CurY, 1, 0, pFont)
                End If

            End If

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(8), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(8) + ClAr(2) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(8) + ClAr(2) + ClAr(9), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(8) + ClAr(2) + ClAr(9) + ClAr(3) + ClAr(10), CurY, LMargin + ClAr(1) + ClAr(8) + ClAr(2) + ClAr(9) + ClAr(3) + ClAr(10), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(8) + ClAr(2) + ClAr(9) + ClAr(3) + ClAr(10) + ClAr(4) + ClAr(11), CurY, LMargin + ClAr(1) + ClAr(8) + ClAr(2) + ClAr(9) + ClAr(3) + ClAr(10) + ClAr(4) + ClAr(11), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(8) + ClAr(2) + ClAr(9) + ClAr(3) + ClAr(10) + ClAr(4) + ClAr(11) + ClAr(5) + ClAr(12), CurY, LMargin + ClAr(1) + ClAr(8) + ClAr(2) + ClAr(9) + ClAr(3) + ClAr(10) + ClAr(4) + ClAr(11) + ClAr(5) + ClAr(12), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(8) + ClAr(2) + ClAr(9) + ClAr(3) + ClAr(10) + ClAr(4) + ClAr(11) + ClAr(5) + ClAr(12) + ClAr(6) + ClAr(13), CurY, LMargin + ClAr(1) + ClAr(8) + ClAr(2) + ClAr(9) + ClAr(3) + ClAr(10) + ClAr(4) + ClAr(11) + ClAr(5) + ClAr(12) + ClAr(6) + ClAr(13), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))




            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 4, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 4, LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13), LnAr(3))

            CurY = CurY + TxtHgt - 15

            pFont = New Font("Calibri", 9, FontStyle.Regular)

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1155" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1263" Then '---- Sri Meenakshi Sizing (Somanur)
                p1Font = New Font("Calibri", 9, FontStyle.Bold)
                If Common_Procedures.Vendor_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("Vendor_IdNo").ToString)) <> "" Then

                    da1 = New SqlClient.SqlDataAdapter("select a.* from Vendor_head a where  a.Vendor_IdNO = " & Val(prn_HdDt.Rows(0).Item("Vendor_IdNo").ToString) & " ", con)
                    dt = New DataTable
                    da1.Fill(dt)

                    If dt.Rows.Count > 0 Then

                        Del_Add1 = dt.Rows(0).Item("Vendor_Address1").ToString & "," & dt.Rows(0).Item("Vendor_Address2").ToString
                        Del_Add2 = dt.Rows(0).Item("Vendor_Address3").ToString & "," & dt.Rows(0).Item("Vendor_Address4").ToString

                    End If
                    Common_Procedures.Print_To_PrintDocument(e, "DELIVERY TO   : " & Common_Procedures.Vendor_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("Vendor_IdNo").ToString)), LMargin + 10, CurY, 0, 0, p1Font)
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, " " & Trim(Del_Add1), LMargin + 30, CurY, 0, 0, pFont)
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, " " & Trim(Del_Add2), LMargin + 30, CurY, 0, 0, pFont)

                Else

                    'Led_Name = prn_HdDt.Rows(0).Item("Ledger_MainName").ToString
                    'Led_Add1 = prn_HdDt.Rows(0).Item("Ledger_Address1").ToString
                    'Led_Add2 = prn_HdDt.Rows(0).Item("Ledger_Address2").ToString
                    'Led_Add3 = prn_HdDt.Rows(0).Item("Ledger_Address3").ToString & "," & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString
                    'Led_Add4 = prn_HdDt.Rows(0).Item("Ledger_Address4").ToString

                    Del_Add1 = prn_HdDt.Rows(0).Item("Ledger_Address1").ToString & "," & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString
                    Del_Add2 = prn_HdDt.Rows(0).Item("Ledger_Address3").ToString & "," & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString

                    Common_Procedures.Print_To_PrintDocument(e, "DELIVERY TO   : " & prn_HdDt.Rows(0).Item("Ledger_MainName").ToString, LMargin + 10, CurY, 0, 0, pFont)
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, " " & Trim(Del_Add1), LMargin + 30, CurY, 0, 0, pFont)
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, " " & Trim(Del_Add2), LMargin + 30, CurY, 0, 0, pFont)

                End If

            Else

                Del_Add1 = prn_HdDt.Rows(0).Item("DelAdd1").ToString & "," & prn_HdDt.Rows(0).Item("DelAdd2").ToString
                Del_Add2 = prn_HdDt.Rows(0).Item("DelAdd3").ToString & "," & prn_HdDt.Rows(0).Item("DelAdd4").ToString

                Common_Procedures.Print_To_PrintDocument(e, "DELIVERY TO   : " & prn_HdDt.Rows(0).Item("DelName").ToString, LMargin + 10, CurY, 0, 0, pFont)
                'If Common_Procedures.settings.CustomerCode = "1112" Then
                '    p1Font = New Font("Calibri", 10, FontStyle.Bold)
                '    Common_Procedures.Print_To_PrintDocument(e, "For Jobwork Only, Not For Sale", PageWidth - 200, CurY, 0, 0, p1Font)
                'End If

                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, " " & Trim(Del_Add1), LMargin + 30, CurY, 0, 0, pFont)
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, " " & Trim(Del_Add2), LMargin + 30, CurY, 0, 0, pFont)

            End If



            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "This is to certify that there is no sale indicated in this delivery and the pavu sized is returned back to party after warping and sizing job work.", LMargin + 10, CurY, 0, 0, pFont)

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

            Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 20, CurY, 0, 0, pFont)
            '  Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 250, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 5, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt + 5

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin, LnAr(1))
            e.Graphics.DrawLine(Pens.Black, PageWidth, CurY, PageWidth, LnAr(1))

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_VehicleNo_GotFocus(sender As Object, e As EventArgs) Handles cbo_VehicleNo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Sizing_Pavu_Delivery_Head", "Vehicle_No", "", "")
    End Sub

    Private Sub cbo_Delivered_GotFocus(sender As Object, e As EventArgs) Handles cbo_Delivered.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Sizing_Pavu_Delivery_Head", "Delivered_By", "", "")
    End Sub

    Private Sub Printing_Format1087(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim EntryCode As String
        Dim NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim cnt As Integer = 0
        Dim LnAr(20) As Single, ClArr(15) As Single
        'Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim PS As Printing.PaperSize
        Dim PCnt As Integer = 0, PrntCnt As Integer = 0
        Dim TpMargin As Single = 0
        Dim Cnt1 As Integer = 0


        PrntCnt = 1

        If Val(Common_Procedures.settings.PavuDelivery_Print_2Copy_In_SinglePage) = 1 Then
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

        NoofItems_PerPage = 11 ' 6

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1263" Then '---- Asia Sizing (Palladam)
            ClArr(1) = Val(25) : ClArr(2) = 50 : ClArr(3) = 60 : ClArr(4) = 55 : ClArr(5) = 50 : ClArr(6) = 70 : ClArr(7) = 70
            ClArr(8) = 30 : ClArr(9) = 50 : ClArr(10) = 60 : ClArr(11) = 55 : ClArr(12) = 50 : ClArr(13) = 70
            ClArr(14) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) + ClArr(11) + ClArr(12) + ClArr(13))
        Else
            ClArr(1) = Val(25) : ClArr(2) = 40 : ClArr(3) = 55 : ClArr(4) = 55 : ClArr(5) = 40 : ClArr(6) = 70 : ClArr(7) = 100
            ClArr(8) = 30 : ClArr(9) = 40 : ClArr(10) = 55 : ClArr(11) = 55 : ClArr(12) = 40 : ClArr(13) = 70
            ClArr(14) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) + ClArr(11) + ClArr(12) + ClArr(13))
        End If

        TxtHgt = 17.2 ' 18.5 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        PrntCnt2ndPageSTS = False
        TpMargin = TMargin

        For PCnt = 1 To PrntCnt
            If Val(Common_Procedures.settings.PavuDelivery_Print_2Copy_In_SinglePage) = 1 Then
                If PCnt = 1 Then
                    prn_PageNo1 = prn_PageNo

                    prn_DetIndx1 = prn_DetIndx
                    prn_DetSNo1 = prn_DetSNo
                    prn_NoofBmDets1 = prn_NoofBmDets
                    TpMargin = TMargin

                Else

                    prn_PageNo = prn_PageNo1
                    prn_NoofBmDets = prn_NoofBmDets1
                    prn_DetIndx = prn_DetIndx1
                    prn_DetSNo = prn_DetSNo1

                    TpMargin = 560 + TMargin  ' 600 + TMargin

                End If
            End If

            Try

                If prn_HdDt.Rows.Count > 0 Then

                    Printing_Format1087_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TpMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                    NoofDets = 0

                    CurY = CurY - 10

                    If prn_DetDt.Rows.Count > 0 Then


                        Do While prn_NoofBmDets < prn_DetMxIndx
                            If NoofDets >= NoofItems_PerPage Then
                                If Val(Common_Procedures.settings.PavuDelivery_Print_2Copy_In_SinglePage) = 1 Then

                                    If PCnt = 2 And PrntCnt = 2 And PrntCnt2ndPageSTS = False Then


                                        CurY = CurY + TxtHgt

                                        Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                                        NoofDets = NoofDets + 1

                                        Printing_Format1087_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, PCnt, False)

                                        prn_DetIndx = prn_DetIndx + NoofItems_PerPage

                                        e.HasMorePages = True

                                        Return
                                    End If

                                Else

                                    CurY = CurY + TxtHgt

                                    Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                                    NoofDets = NoofDets + 1

                                    Printing_Format1087_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, PCnt, False)

                                    prn_DetIndx = prn_DetIndx + NoofItems_PerPage

                                    e.HasMorePages = True

                                    Return

                                End If
                            End If

                            prn_DetIndx = prn_DetIndx + 1

                            If PCnt <> 2 Then

                                If Val(prn_DetAr(prn_DetIndx, 4)) <> 0 Then
                                    If PCnt = 1 And NoofDets < NoofItems_PerPage Then
                                        CurY = CurY + TxtHgt
                                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx, 1))), LMargin + 3, CurY, 0, 0, pFont)
                                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 6)), LMargin + ClArr(1) + 3, CurY, 0, 0, pFont)
                                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx, 2))), LMargin + ClArr(1) + ClArr(2) + 3, CurY, 0, 0, pFont)
                                        Common_Procedures.Print_To_PrintDocument(e, Trim((prn_DetAr(prn_DetIndx, 7))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 3, CurY, 0, 0, pFont)
                                        Common_Procedures.Print_To_PrintDocument(e, Trim((prn_DetAr(prn_DetIndx, 8))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 3, CurY, 0, 0, pFont)
                                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx, 4)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 5, CurY, 1, 0, pFont)
                                        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1263" Then '---- Meenashi Sizing (Kpati)
                                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 9)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + 3, CurY, 0, 0, pFont)
                                        Else
                                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 5)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + 3, CurY, 0, 0, pFont)
                                        End If

                                    End If
                                    prn_NoofBmDets = prn_NoofBmDets + 1

                                End If

                                If Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 4)) <> 0 Then
                                    If PCnt = 1 And NoofDets < NoofItems_PerPage Then
                                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 1))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + 7, CurY, 0, 0, pFont)
                                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 6)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + 3, CurY, 0, 0, pFont)
                                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 2))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + 3, CurY, 0, 0, pFont)
                                        Common_Procedures.Print_To_PrintDocument(e, Trim((prn_DetAr(prn_DetIndx + NoofItems_PerPage, 7))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) + 3, CurY, 0, 0, pFont)
                                        Common_Procedures.Print_To_PrintDocument(e, Trim((prn_DetAr(prn_DetIndx + NoofItems_PerPage, 8))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) + ClArr(11) + 3, CurY, 0, 0, pFont)
                                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 4)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) + ClArr(11) + ClArr(12) + ClArr(13) - 5, CurY, 1, 0, pFont)
                                        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1263" Then '---- Meenashi Sizing (Kapati)
                                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 9)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) + ClArr(11) + ClArr(12) + ClArr(13) + 3, CurY, 0, 0, pFont)
                                        Else
                                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 5)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) + ClArr(11) + ClArr(12) + ClArr(13) + 3, CurY, 0, 0, pFont)
                                        End If

                                    End If

                                    prn_NoofBmDets = prn_NoofBmDets + 1

                                End If

                                NoofDets = NoofDets + 1

                            End If

                            If PCnt = 2 Then
                                If Val(prn_DetAr(prn_DetIndx, 4)) <> 0 Then
                                    CurY = CurY + TxtHgt
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx, 1))), LMargin + 3, CurY, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 6)), LMargin + ClArr(1) + 3, CurY, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx, 2))), LMargin + ClArr(1) + ClArr(2) + 3, CurY, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Trim((prn_DetAr(prn_DetIndx, 7))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 3, CurY, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Trim((prn_DetAr(prn_DetIndx, 8))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 3, CurY, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx, 4)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 3, CurY, 1, 0, pFont)
                                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1263" Then
                                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 9)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + 3, CurY, 0, 0, pFont)
                                    Else
                                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 5)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + 3, CurY, 0, 0, pFont)
                                    End If
                                    prn_NoofBmDets = prn_NoofBmDets + 1

                                End If

                                If Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 4)) <> 0 Then

                                    Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 1))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + 7, CurY, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 6)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + 3, CurY, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 2))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + 3, CurY, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Trim((prn_DetAr(prn_DetIndx + NoofItems_PerPage, 7))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) + 3, CurY, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Trim((prn_DetAr(prn_DetIndx + NoofItems_PerPage, 8))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) + ClArr(11) + 3, CurY, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 4)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) + ClArr(11) + ClArr(12) + ClArr(13) - 3, CurY, 1, 0, pFont)
                                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1263" Then
                                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 9)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) + ClArr(11) + ClArr(12) + ClArr(13) + 3, CurY, 0, 0, pFont)
                                    Else
                                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 5)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) + ClArr(11) + ClArr(12) + ClArr(13) + 3, CurY, 0, 0, pFont)
                                    End If



                                    prn_NoofBmDets = prn_NoofBmDets + 1

                                End If

                                NoofDets = NoofDets + 1
                            End If
                        Loop
                    End If

                    Printing_Format1087_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, PCnt, True)

                End If

            Catch ex As Exception

                MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End Try
            If Val(Common_Procedures.settings.PavuDelivery_Print_2Copy_In_SinglePage) = 1 Then

                If PCnt = 1 And PrntCnt = 2 And PrntCnt2ndPageSTS = False Then
                    If prn_DetDt.Rows.Count = cnt + 18 Then
                        PrntCnt2ndPageSTS = True
                        e.HasMorePages = True
                        cnt = 18
                        Return
                    End If
                End If
            End If
            PrntCnt2ndPageSTS = False

        Next PCnt
LOOP10:


        prn_Count = prn_Count + 1

        e.HasMorePages = False

        If Val(prn_TotCopies) > 1 Then
            If prn_Count < Val(prn_TotCopies) Then

                prn_DetIndx = 0
                prn_PageNo = 0
                prn_DetIndx = 0
                prn_PageNo = 0
                prn_NoofBmDets = 0


                e.HasMorePages = True
                Return

            End If

        End If

    End Sub

    Private Sub Printing_Format1087_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim da3 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim p1Font As Font

        Dim strHeight As Single
        Dim W1 As Single, W2 As Single, W3 As Single, N1 As Single, M1 As Single
        Dim i As Integer, k As Integer = 0
        Dim Arr(300, 5) As String
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String, Cmp_Add3 As String, Cmp_Add4 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_Email As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim Led_Name As String, Led_Add1 As String, Led_Add2 As String, Led_Add3 As String, Led_Add4 As String
        Dim Led_GstNo As String, Led_PanNo As String
        Dim Hsn_Code As String = ""
        Dim Cnt_Name As String = ""
        'Dim Entry_dt As Date
        Dim CurX As Single = 0
        Dim strWidth As Single = 0
        Dim Ledname1 As String
        Dim Ledname2 As String
        'Dim ItmNm1 As String, ItmNm2 As String

        PageNo = PageNo + 1

        CurY = TMargin

        If prn_DetMxIndx > (2 * NoofItems_PerPage) Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If

        'da2 = New SqlClient.SqlDataAdapter("select DISTINCT(setcode_forSelection) from Stock_SizedPavu_Processing_Details where Pavu_Delivery_Code = '" & Trim(EntryCode) & "' Order by setcode_forSelection", con)
        'dt3 = New DataTable
        'da2.Fill(dt3)

        'vSetNo = ""
        'If dt3.Rows.Count > 0 Then
        '    For i = 0 To dt3.Rows.Count - 1
        '        k = InStr(1, dt3.Rows(i).Item("setcode_forSelection").ToString, "/")
        '        vSetNo = vSetNo & IIf(Trim(vSetNo) <> "", ", ", "") & Microsoft.VisualBasic.Left(dt3.Rows(i).Item("setcode_forSelection").ToString, k - 1)
        '    Next i
        'End If
        'dt3.Dispose()
        Hsn_Code = ""
        Cnt_Name = dgv_Details.Rows(0).Cells(8).Value
        da3 = New SqlClient.SqlDataAdapter("select a.*, b.* from Stock_SizedPavu_Processing_Details a LEFT OUTER JOIN Count_Head b ON a.Count_IdNo = b.Count_IdNo where a.Pavu_Delivery_Code = '" & Trim(EntryCode) & "' and b.Count_Name = '" & Trim(Cnt_Name) & "' Order by a.sl_no", con)
        dt4 = New DataTable
        da3.Fill(dt4)


        If dt4.Rows.Count > 0 Then
            Hsn_Code = dt4.Rows(0).Item("HSN_Code").ToString

            'For i = 0 To dt4.Rows.Count - 1
            '    'k = InStr(1, dt4.Rows(i).Item("Count_Hsn_Code").ToString, "/")
            '    'Hsn_Code = Hsn_Code & IIf(Trim(Hsn_Code) <> "", ", ", "") & Microsoft.VisualBasic.Left(dt4.Rows(i).Item("Count_Hsn_Code").ToString, k - 1)
            '    ' Cnt_Name = dt4.Rows(i).Item("Count_Name").ToString
            '    'Hsn_Code = dt4.Rows(0).Item("Count_Hsn_Code").ToString
            'Next
        End If
        dt4.Clear()
        dt4.Dispose()

        CurY = TMargin
        CurY = CurY + 5
        p1Font = New Font("Arial", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "SIZED PAVU DELIVERY NOTE", LMargin, CurY - TxtHgt, 2, PrintWidth, p1Font)
        ' strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = "" : Cmp_Add3 = "" : Cmp_Add4 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_Email = ""
        Cmp_StateCap = "" : Cmp_StateNm = "" : Cmp_StateCode = "" : Cmp_GSTIN_Cap = "" : Cmp_GSTIN_No = ""

        Led_Name = "" : Led_Add1 = "" : Led_Add2 = "" : Led_Add3 = "" : Led_Add4 = "" : Led_GstNo = ""

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString

        If Common_Procedures.settings.CustomerCode = "1078" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then
            Cmp_Add1 = prn_HdDt.Rows(0).Item("Sizing_Address1").ToString
            Cmp_Add2 = prn_HdDt.Rows(0).Item("Sizing_Address2").ToString
            Cmp_Add3 = prn_HdDt.Rows(0).Item("Sizing_Address3").ToString
            Cmp_Add4 = prn_HdDt.Rows(0).Item("Sizing_Address4").ToString

            If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
                Cmp_PhNo = "PHONE NO.:" & prn_HdDt.Rows(0).Item("Sizing_PhoneNo").ToString
            End If
            If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
                Cmp_Email = "E-mail : " & Trim(prn_HdDt.Rows(0).Item("Sizing_EMail").ToString)
            End If

        Else

            Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString
            Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address2").ToString
            Cmp_Add3 = prn_HdDt.Rows(0).Item("Company_Address3").ToString
            Cmp_Add4 = prn_HdDt.Rows(0).Item("Company_Address4").ToString

            If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
                Cmp_PhNo = "PHONE NO.:" & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
            End If
            If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
                Cmp_Email = "E-mail : " & Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString)
            End If


        End If



        If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
            Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_panNo").ToString) <> "" Then
            Cmp_CstNo = "PAN NO.: " & prn_HdDt.Rows(0).Item("Company_panNo").ToString
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
            Cmp_GSTIN_No = "GSTIN" & prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
        End If
        If Trim(UCase(Common_Procedures.User.Type)) = "UNACCOUNT" And (Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1263") Then
            Led_Name = prn_HdDt.Rows(0).Item("Ledger_MainName").ToString : Led_Add1 = "" : Led_Add2 = "" : Led_Add3 = "" : Led_Add4 = "" : Led_GstNo = ""
        Else
            Led_Name = prn_HdDt.Rows(0).Item("Ledger_MainName").ToString
            Led_Add1 = prn_HdDt.Rows(0).Item("Ledger_Address1").ToString
            Led_Add2 = prn_HdDt.Rows(0).Item("Ledger_Address2").ToString
            Led_Add3 = prn_HdDt.Rows(0).Item("Ledger_Address3").ToString & "," & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString
            Led_Add4 = prn_HdDt.Rows(0).Item("Ledger_Address4").ToString

            If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then

                Led_GstNo = "GSTIN" & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString

                Led_PanNo = "PAN NO : " & prn_HdDt.Rows(0).Item("PAN_no").ToString
            End If
        End If
        CurY = CurY + TxtHgt - 10

        M1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7)

        p1Font = New Font("Americana Std", 10, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, StrConv(Cmp_Name, VbStrConv.ProperCase), LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        p1Font = New Font("Arial", 8, FontStyle.Regular)
        Common_Procedures.Print_To_PrintDocument(e, "PAVU SIZED TO :", LMargin + M1 + 10, CurY, 0, 0, p1Font)
        Dim cury2 As Single
        cury2 = CurY


        'If Trim(prn_HdDt.Rows(0).Item("Company_Name").ToString) <> "" Then
        '    CmpName1 = Trim(prn_HdDt.Rows(0).Item("Company_Name").ToString)
        'Else
        Ledname1 = Trim(prn_HdDt.Rows(0).Item("Ledger_MainName").ToString)
        '  End If

        Ledname2 = ""

        If Len(Ledname1) > 40 Then
            For i = 40 To 1 Step -1
                If Mid$(Trim(Ledname1), i, 1) = " " Or Mid$(Trim(Ledname1), i, 1) = "," Or Mid$(Trim(Ledname1), i, 1) = "." Or Mid$(Trim(Ledname1), i, 1) = "-" Or Mid$(Trim(Ledname1), i, 1) = "/" Or Mid$(Trim(Ledname1), i, 1) = "_" Or Mid$(Trim(Ledname1), i, 1) = "(" Or Mid$(Trim(Ledname1), i, 1) = ")" Or Mid$(Trim(Ledname1), i, 1) = "\" Or Mid$(Trim(Ledname1), i, 1) = "[" Or Mid$(Trim(Ledname1), i, 1) = "]" Or Mid$(Trim(Ledname1), i, 1) = "{" Or Mid$(Trim(Ledname1), i, 1) = "}" Then Exit For
            Next i
            If i = 0 Then i = 40
            Ledname2 = Microsoft.VisualBasic.Right(Trim(Ledname1), Len(Ledname1) - i)
            Ledname1 = Microsoft.VisualBasic.Left(Trim(Ledname1), i - 1)
        End If


        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin + 10, CurY, 0, 0, pFont)

        cury2 = cury2 + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "M/s." & Ledname1, LMargin + M1 + 10, cury2, 0, 0, p1Font)

        If Trim(Ledname2) <> "" Then
            cury2 = cury2 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(Ledname2), LMargin + M1 + 10, cury2, 0, 0, p1Font)
            'NoofDets = NoofDets + 1
        End If

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin + 10, CurY, 0, 0, pFont)
        cury2 = cury2 + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Led_Add1, LMargin + M1 + 10, cury2, 0, 0, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add3, LMargin + 10, CurY, 0, 0, pFont)
        cury2 = cury2 + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Led_Add2, LMargin + M1 + 10, cury2, 0, 0, pFont)
        If Trim(Cmp_Add4) <> "" Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Add4, LMargin + 10, CurY, 0, 0, pFont)
        End If
        If Trim(Led_Add3) <> "" Then
            cury2 = cury2 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Led_Add3, LMargin + M1 + 10, cury2, 0, 0, pFont)
        End If
        'CurY = CurY + TxtHgt
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, Led_Add4, LMargin + M1 + 10, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No & "    " & Cmp_CstNo, LMargin + 10, CurY, 0, 0, pFont)
        cury2 = cury2 + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Led_GstNo & "  " & Led_PanNo, LMargin + M1 + 10, cury2, 0, 0, pFont)
        If CurY > cury2 Then
            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(2) = CurY
        Else
            cury2 = cury2 + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, cury2, PageWidth, cury2)
            LnAr(2) = cury2
        End If

        'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        'LnAr(2) = CurY
        e.Graphics.DrawLine(Pens.Black, LMargin + M1, LnAr(2), LMargin + M1, LnAr(1))


        ' CurY = CurY + TxtHgt - 12



        ' Try

        N1 = e.Graphics.MeasureString("DATE & TIME  :", pFont).Width
        W1 = e.Graphics.MeasureString("HSC CODE  :", pFont).Width
        W2 = e.Graphics.MeasureString("VAN NO   :", pFont).Width
        W3 = e.Graphics.MeasureString("APPROX VALUE  :", pFont).Width

        M1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7)

        CurY = cury2

        CurY = CurY + TxtHgt - 11
        p1Font = New Font("Arial", 8, FontStyle.Bold)


        Common_Procedures.Print_To_PrintDocument(e, "DC.NO", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
        p1Font = New Font("Arial", 8, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Pavu_Delivery_No").ToString, LMargin + W1 + 25, CurY, 0, 0, p1Font)
        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1078" Then '---- Kalaimagal Sizing (Palladam)
        Common_Procedures.Print_To_PrintDocument(e, "DATE & TIME", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + (ClAr(4) \ 2) + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + (ClAr(4) \ 2) + N1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Pavu_Delivery_Date").ToString), "dd-MM-yyyy") & " & " & (prn_HdDt.Rows(0).Item("Date_And_Time_Of_Supply").ToString).ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + (ClAr(4) \ 2) + N1 + 20, CurY, 0, 0, pFont)
        'End If
        Common_Procedures.Print_To_PrintDocument(e, "VAN NO", LMargin + M1 + 20, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vehicle_No").ToString, LMargin + M1 + W1 + 25, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, "APPROX VALUE", LMargin + M1 + ClAr(6) + ClAr(7) + ClAr(8) + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + ClAr(6) + ClAr(7) + ClAr(8) + W3 + 15, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Approx_Value").ToString, LMargin + M1 + ClAr(6) + ClAr(7) + ClAr(8) + W3 + 25, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt

        Common_Procedures.Print_To_PrintDocument(e, "HSN CODE", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, (Hsn_Code), LMargin + W1 + 25, CurY, 0, 0, pFont)

        p1Font = New Font("Arial", 8, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "SAC CODE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + (ClAr(4) \ 2) + 20, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + (ClAr(4) \ 2) + N1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, " 998821", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + (ClAr(4) \ 2) + N1 + 20, CurY, 0, 0, p1Font)

        Common_Procedures.Print_To_PrintDocument(e, "EwayBill", LMargin + M1 + 20, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Electronic_Reference_No").ToString, LMargin + M1 + W2 + 25, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, "TRANSPORT", LMargin + M1 + ClAr(6) + ClAr(7) + ClAr(8) + 15, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + ClAr(6) + ClAr(7) + ClAr(8) + W3 + 15, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Transport_Name").ToString, LMargin + M1 + ClAr(6) + ClAr(7) + ClAr(8) + W3 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt + 2
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        pFont = New Font("Arial", 7, FontStyle.Regular)
        CurY = CurY + TxtHgt - 15
        Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin, CurY, 2, ClAr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "SET NO", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "BEAM NO", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "ENDS", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "COUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 3, CurY, 2, ClAr(6), pFont)
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1263" Then
            Common_Procedures.Print_To_PrintDocument(e, "MARK", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
        Else
            Common_Procedures.Print_To_PrintDocument(e, "DELIVERY PARTY", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
        End If

        Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 3, CurY, 2, ClAr(8), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "SET NO", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "BEAM NO", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, 2, ClAr(10), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "ENDS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, 2, ClAr(11), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "COUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), CurY, 2, ClAr(12), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), CurY, 2, ClAr(13), pFont)
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1220" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1263" Then
            Common_Procedures.Print_To_PrintDocument(e, "MARK", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13), CurY, 2, ClAr(14), pFont)
        Else
            Common_Procedures.Print_To_PrintDocument(e, "DELIVERY PARTY", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13), CurY, 2, ClAr(14), pFont)
        End If

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY
        pFont = New Font("Arial", 8, FontStyle.Regular)
        ' Catch ex As Exception

        'MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        '  End Try

    End Sub

    Private Sub Printing_Format1087_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal pcnt As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim Cmp_Name As String
        Dim I As Integer
        Dim M1 As Single
        Dim Del_Add1 As String = "", Del_Add2 As String = ""
        Dim cnt2 As Integer = 0
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        Dim vSgst_amt As String = 0
        Dim vCgst_amt As String = 0

        Try

            For I = NoofDets + 1 To NoofItems_PerPage
                CurY = CurY + TxtHgt
            Next

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

            M1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7)
            CurY = CurY + TxtHgt - 15

            If is_LastPage = True Then

                If (prn_DetMxIndx Mod (NoofItems_PerPage * 2)) <= NoofItems_PerPage Then

                    If Val(prn_HdDt.Rows(0).Item("Total_Beam").ToString) <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Beam").ToString), LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
                    End If

                    If Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString) <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 5, CurY, 1, 0, pFont)
                    End If

                Else

                    If Val(prn_HdDt.Rows(0).Item("Total_Beam").ToString) <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Beam").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + 10, CurY, 0, 0, pFont)
                    End If


                    If Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString) <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) - 5, CurY, 1, 0, pFont)
                    End If

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
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 4, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 4, LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13), LnAr(3))
            CurY = CurY + TxtHgt - 15

            If Val(prn_HdDt.Rows(0).Item("Rate").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, " Rate/Meters : " & Trim(prn_HdDt.Rows(0).Item("Rate").ToString), PageWidth - 510, CurY, 0, 0, pFont)
            End If

            pFont = New Font("Arial", 8, FontStyle.Regular)



            Del_Add1 = prn_HdDt.Rows(0).Item("DelAdd1").ToString & "," & prn_HdDt.Rows(0).Item("DelAdd2").ToString
                Del_Add2 = prn_HdDt.Rows(0).Item("DelAdd3").ToString & "," & prn_HdDt.Rows(0).Item("DelAdd4").ToString

                Common_Procedures.Print_To_PrintDocument(e, "DELIVERY TO   : " & prn_HdDt.Rows(0).Item("DelName").ToString, LMargin + 10, CurY, 0, 0, pFont)
                'If Common_Procedures.settings.CustomerCode = "1112" Then
                '    p1Font = New Font("Calibri", 10, FontStyle.Bold)
                '    Common_Procedures.Print_To_PrintDocument(e, "For Jobwork Only, Not For Sale", PageWidth - 200, CurY, 0, 0, p1Font)
                'End If

                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, " " & Trim(Del_Add1), LMargin + 30, CurY, 0, 0, pFont)
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, " " & Trim(Del_Add2), LMargin + 30, CurY, 0, 0, pFont)


            Dim vTxamt As String = 0
            Dim vNtAMt As String = 0
            '----------------
            If Val(prn_HdDt.Rows(0).Item("approx_Value").ToString) <> 0 Then

                vCgst_amt = Format((Val(prn_HdDt.Rows(0).Item("approx_Value").ToString) * 2.5 / 100), "############0")
                vSgst_amt = Format((Val(prn_HdDt.Rows(0).Item("approx_Value").ToString) * 2.5 / 100), "############0")


                Common_Procedures.Print_To_PrintDocument(e, " CGST 2.5 % : " & vCgst_amt, PageWidth - 530, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " SGST 2.5 % : " & vSgst_amt, PageWidth - 400, CurY, 0, 0, pFont)
            End If

            If Val(prn_HdDt.Rows(0).Item("approx_Value").ToString) <> 0 Then
                vTxamt = Val(vCgst_amt) + Val(vSgst_amt) 'Format((Val(prn_HdDt.Rows(0).Item("approx_Value").ToString) * 5 / 100), "############0.00")
                Common_Procedures.Print_To_PrintDocument(e, " Tax Amount : " & vTxamt, PageWidth - 280, CurY, 0, 0, pFont)
            End If

            If Val(vTxamt) <> 0 Then
                vNtAMt = Format(Val(prn_HdDt.Rows(0).Item("approx_Value").ToString) + vTxamt, "###########0.00")
                Common_Procedures.Print_To_PrintDocument(e, " Net Amount : " & vNtAMt, PageWidth - 150, CurY, 0, 0, pFont)
            End If

            '--------------

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "This is to certify that there is no sale indicated in this delivery and the pavu sized is returned back to party after warping and sizing job work.", LMargin + 10, CurY, 0, 0, pFont)

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

            Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 20, CurY, 0, 0, pFont)
            '  Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 250, CurY, 0, 0, pFont)
            p1Font = New Font("Arial", 8, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 5, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt + 5

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin, LnAr(1))
            e.Graphics.DrawLine(Pens.Black, PageWidth, CurY, PageWidth, LnAr(1))

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

End Class