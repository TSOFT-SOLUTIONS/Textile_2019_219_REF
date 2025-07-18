Imports Excel = Microsoft.Office.Interop.Excel
Imports System.IO
Imports Newtonsoft.Json
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Linq
Imports System.Net
Imports System.Text
Imports System.Threading.Tasks
Imports System.Windows.Forms
Imports TaxProEWB.API
Imports System.Diagnostics
Imports System.Web.Script.Serialization
Imports System.Runtime.Remoting

Public Class Ledger_Creation
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private New_Entry As Boolean = False
    Private vLedType As String
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Private OwmLoom_STS As Integer = 0

    Private Show_STS As Integer = 0
    Private Verified_STS As Integer = 0
    Private Stock_STS As Integer = 0
    Private TrnTo_DbName As String = ""
    Private SaveAll_STS As Boolean = False
    Private LastNo As String = ""
    Private FrmLdSTS As Boolean = False
    Private dgv_ActiveCtrl_Name As String = ""
    Private WithEvents dgtxt_KnittingDetails As New DataGridViewTextBoxEditingControl
    Private WithEvents dgtxt_FreihtChargeDetails As New DataGridViewTextBoxEditingControl
    Private WithEvents dgtxt_WeaverWagesDetails As New DataGridViewTextBoxEditingControl
    Private WithEvents dgtxt_ContactPersonDetails As New DataGridViewTextBoxEditingControl
    Private prn_HdDt As New DataTable
    Private WithEvents dgtxt_LoomDetails As New DataGridViewTextBoxEditingControl
    Private SizTo_DbName As String = ""
    Private TCS_STS As Integer = 0
    Private prn_DetDt As New DataTable
    Private prn_DetIndx As Integer
    Private prn_DetSNo As Integer
    Private prn_PageNo As Integer
    Private prn_count As Integer = 0

    Private TDS_STS As Integer = 0
    Private IsAbove_10CR_STS As Integer = 0

    Dim GSPName As String
    Dim ASPUserID As String
    Dim AspPassword As String
    Dim ClientId As String
    Dim ClientSecret As String
    Dim GspUserId As String
    Dim BaseURL As String

    Dim Gstin As String
    Dim UserId As String
    Dim Password As String
    Dim AppKey As String
    Dim AuthToken As String
    Dim TokenExp As String
    Dim SEK As String
    '  Public WithEvents EwbSession As EWBSession = New EWBSession()
    Private IsSandboxGSTAc As Boolean = False
    Private PrntFormat1_STS As Boolean = False
    Private PrntFormat2_STS As Boolean = False

    Public Sub New()
        FrmLdSTS = True
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Private Sub clear()
        Dim obj As Object
        Dim ctrl As Object
        Dim grpbx As GroupBox

        For Each obj In Me.Controls
            If TypeOf obj Is TextBox Then
                obj.text = ""
            ElseIf TypeOf obj Is ComboBox Then
                obj.text = ""
            ElseIf TypeOf obj Is GroupBox Then
                grpbx = obj
                For Each ctrl In grpbx.Controls
                    If TypeOf ctrl Is TextBox Then
                        ctrl.text = ""
                    ElseIf TypeOf ctrl Is ComboBox Then
                        ctrl.text = ""
                    End If
                Next

            End If
        Next
        txt_Production_per_Day.Text = ""

        cbo_Sizing_To_CompanyName.Text = ""
        cbo_Sizing_To_VendorName.Text = ""
        txt_InsuranceNo.Text = ""
        txt_Freight_per_Loom.Text = ""
        txt_NoofLoom.Text = ""
        txt_TdsPerc.Text = ""
        txt_MobileSms.Text = ""
        txt_Freight_Pavu.Text = ""
        txt_freight_Bundle.Text = ""
        cbo_Grid_cloth_name.Text = ""
        txt_PanAddress1.Text = ""
        txt_PanAddress2.Text = ""
        Txt_TamilAddress_1.Text = ""
        Txt_TamilAddress_2.Text = ""
        txt_PanAddress3.Text = ""
        txt_PanAddress4.Text = ""
        txt_MeterPrReel.Text = ""
        Show_In_All_Entry.Checked = False
        Own_Loom_Status.Checked = False
        Close_Status.Checked = False
        Chk_Tcs_for_Sales_sts.Checked = False
        chk_Stock_Maintenance.Checked = False
        New_Entry = False
        pnl_Freight_Charge_Details.Visible = False
        pnl_Wages_Charge_Details.Visible = False
        pnl_PYStockLimit.Visible = False
        cbo_Grid_cloth_name.Visible = False
        cbo_Weaver_LoomType.Text = ""
        pnl_PrintSetup.Visible = False
        txt_vehicleNo.Text = ""
        txt_PavuMax.Text = ""
        txt_PavuMin.Text = ""
        txt_YarnMax.Text = ""
        txt_YarnMin.Text = ""
        ' cbo_LedgerGroup.Text = ""
        txt_TopFromAdds.Text = ""
        txt_TopFromAdds.Enabled = False
        txt_TOPToAdds.Text = ""
        txt_LeftFromAdds.Text = ""
        txt_LeftFromAdds.Enabled = False
        txt_LeftToAdds.Text = ""
        cbo_PaperOrientation.Text = ""
        cbo_FromAddress.Text = Common_Procedures.Company_IdNoToName(con, 1)
        chk_FromAddress.Checked = False
        Txt_Remarks.Text = ""
        txt_contact_person_name.Text = ""
        cbo_designation.Text = ""
        cbo_party_category.Text = ""

        txt_CreditLimit.Text = ""
        txt_creditLimitDays.Text = ""

        txt_Bank_Acc_Name.Text = ""
        txt_bankName.Text = ""
        txt_AccountNo.Text = ""
        txt_Branch.Text = ""
        txt_Ifsc_Code.Text = ""


        cbo_BillType.Text = "BALANCE ONLY"

        chk_WeavingBill_IR_Receipt_Mtrs.Checked = False

        txt_LegalName_Business.Text = ""
        txt_city.Text = ""
        txt_pincode.Text = ""
        txt_Distance.Text = ""
        Cbo_Gird_desigantion.Text = ""
        dgv_Contact_Person_Details.Rows.Clear()

        txt_Ledger_ShortName.Text = ""
        cbo_marketting_Exec_Name.Text = ""

        cbo_Grid_ClothName.Text = ""

        Chk_Sales_Tds_Deduction_sts.Checked = False
        Chk_Is_TurnOver_Above_10_Crore_Sts.Checked = False
        chk_GSTIN_Verified.Checked = False
        chk_GSTIN_Verified.Enabled = True

        Chk_Tcs_for_Purchase_sts.Checked = False
        Chk_Purchase_Tds_Deduction_sts.Checked = False

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1461" Then


            If Trim(UCase(vLedType)) = "SIZING" Or Trim(UCase(vLedType)) = "WEAVER" Or Trim(UCase(vLedType)) = "REWINDING" Or Trim(UCase(vLedType)) = "TRANSPORT" Then
                cbo_AcGroup.Text = Common_Procedures.AccountsGroup_IdNoToName(con, 14)

            ElseIf Trim(UCase(vLedType)) = "SALESPARTY" Or Trim(UCase(vLedType)) = "JOBWORKER" Then
                cbo_AcGroup.Text = Common_Procedures.AccountsGroup_IdNoToName(con, 10)
                cbo_BillType.Text = "BILL TO BILL"

            ElseIf Trim(UCase(vLedType)) = "GODOWN" Then
                cbo_AcGroup.Text = "STOCK-IN-HAND"

            ElseIf Trim(UCase(vLedType)) = "SEWING" Or Trim(UCase(vLedType)) = "SPINNING" Then
                cbo_AcGroup.Text = Common_Procedures.AccountsGroup_IdNoToName(con, 14)
                cbo_BillType.Text = "BILL TO BILL"
            Else
                cbo_AcGroup.Text = Common_Procedures.AccountsGroup_IdNoToName(con, 10)
                cbo_BillType.Text = "BILL TO BILL"

            End If
        End If


        If Trim(Common_Procedures.settings.CustomerCode) <> "1105" Then '----GANGA WEAVING MILLS
            cbo_State.Text = Common_Procedures.State_IdNoToName(con, 32)
        End If

        If Trim(Common_Procedures.settings.CustomerCode) = "1357" Then '----GANGA WEAVING MILLS
            cbo_State.Text = ""
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1398" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1492" Then
            cbo_BillType.Text = "BALANCE ONLY"
        End If
        lbl_IdNo.ForeColor = Color.Black
        dgv_Freight_Charge_Details.Rows.Clear()
        dgv_Wages_Charge_Details.Rows.Clear()
        dgv_RateDetails.Rows.Clear()
        grp_Back.Enabled = True
        grp_Open.Visible = False
        grp_Filter.Visible = False
        pnl_BobinReelDetails.Visible = False
        pnl_Loom_Details.Visible = False
        dgv_ActiveCtrl_Name = ""
        dgv_Loom_Details.Rows.Clear()
        pnl_bank_Details.Visible = False

    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox

        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Then
            Me.ActiveControl.BackColor = Color.SpringGreen ' Color.MistyRose ' Color.lime
            Me.ActiveControl.ForeColor = Color.Blue
        End If

        If TypeOf Me.ActiveControl Is TextBox Then
            txtbx = Me.ActiveControl
            txtbx.SelectAll()

        ElseIf TypeOf Me.ActiveControl Is ComboBox Then
            combobx = Me.ActiveControl
            combobx.SelectAll()

        End If

        If Me.ActiveControl.Name <> dgv_Filter.Name Then
            Grid_Cell_DeSelect()
        End If
        If Me.ActiveControl.Name <> dgv_Contact_Person_Details.Name Then
            Grid_DeSelect()
        End If

        If Me.ActiveControl.Name <> Cbo_Gird_desigantion.Name Then
            Cbo_Gird_desigantion.Visible = False
            'cbo_Grid_ClothName.Tag = -100
        End If

        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)

        On Error Resume Next

        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Then
                Prec_ActCtrl.BackColor = Color.White
                Prec_ActCtrl.ForeColor = Color.Black
            End If
        End If

    End Sub

    Private Sub ControlLostFocus1(ByVal sender As Object, ByVal e As System.EventArgs)

        On Error Resume Next

        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Then
                Prec_ActCtrl.BackColor = Color.LightBlue
                Prec_ActCtrl.ForeColor = Color.Blue
            End If
        End If

    End Sub
    Private Sub Grid_DeSelect()
        On Error Resume Next
        If Not IsNothing(dgv_Contact_Person_Details.CurrentCell) Then dgv_Contact_Person_Details.CurrentCell.Selected = False
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


        If Not IsNothing(dgv_Filter.CurrentCell) Then dgv_Filter.CurrentCell.Selected = False
        If Not IsNothing(dgv_Freight_Charge_Details.CurrentCell) Then dgv_Freight_Charge_Details.CurrentCell.Selected = False
        If Not IsNothing(dgv_Wages_Charge_Details.CurrentCell) Then dgv_Wages_Charge_Details.CurrentCell.Selected = False
        If Not IsNothing(dgv_Loom_Details.CurrentCell) Then dgv_Loom_Details.CurrentCell.Selected = False
    End Sub

    Public Sub move_record(ByVal idno As Integer)
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim da3 As New SqlClient.SqlDataAdapter
        Dim dt3 As New DataTable
        Dim da4 As New SqlClient.SqlDataAdapter
        Dim dt4 As New DataTable
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim n As Integer = 0
        Dim SNo As Integer = 0

        If Val(idno) = 0 Then Exit Sub

        clear()

        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()

        da = New SqlClient.SqlDataAdapter("select a.*, b.AccountsGroup_Name, c.Area_Name , Ch.Company_Name , Me.Marketting_Executive_Name from Ledger_Head a Left Join Company_Head Ch ON a.Company_IdNo = Ch.Company_IdNo LEFT OUTER JOIN Area_Head c ON a.Area_IdNo = c.Area_IdNo LEFT OUTER JOIN AccountsGroup_Head b ON a.AccountsGroup_IdNo = b.AccountsGroup_IdNo LEFT OUTER JOIN Marketting_Executive_Head Me On Me.Marketting_Executive_IdNo = a.Marketting_Executive_IdNo  where a.ledger_idno = " & Str(Val(idno)) & " and a.ledger_type = '" & Trim(vLedType) & "'", con)
        dt = New DataTable
        da.Fill(dt)

        If dt.Rows.Count > 0 Then
            lbl_IdNo.Text = dt.Rows(0).Item("Ledger_IdNo").ToString
            txt_Name.Text = dt.Rows(0).Item("Ledger_MainName").ToString
            txt_AlaisName.Text = dt.Rows(0).Item("Ledger_AlaisName").ToString
            cbo_Area.Text = dt.Rows(0)("Area_Name").ToString
            cbo_AcGroup.Text = dt.Rows(0)("AccountsGroup_Name").ToString
            If Val(dt.Rows(0).Item("Ledger_IdNo").ToString) <> Val(dt.Rows(0).Item("LedgerGroup_Idno").ToString) Then
                cbo_LedgerGroup.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt.Rows(0)("LedgerGroup_Idno").ToString))
            End If

            cbo_BillType.Text = dt.Rows(0)("Bill_Type").ToString
            txt_Address1.Text = dt.Rows(0)("Ledger_Address1").ToString
            txt_Address2.Text = dt.Rows(0)("Ledger_Address2").ToString
            txt_Address3.Text = dt.Rows(0)("Ledger_Address3").ToString
            txt_Address4.Text = dt.Rows(0)("Ledger_Address4").ToString
            txt_PhoneNo.Text = dt.Rows(0)("Ledger_PhoneNo").ToString
            txt_TinNo.Text = dt.Rows(0)("Ledger_TinNo").ToString
            txt_CstNo.Text = dt.Rows(0)("Ledger_CstNo").ToString
            txt_Mail.Text = dt.Rows(0)("Ledger_Mail").ToString
            txt_MobileSms.Text = dt.Rows(0)("MobileNo_Frsms").ToString
            txt_PanNo.Text = dt.Rows(0)("Pan_No").ToString
            txt_OwnerName.Text = dt.Rows(0)("Owner_Name").ToString
            txt_TamilName.Text = dt.Rows(0)("Tamil_Name").ToString
            txt_AdvanceLess_Amount.Text = dt.Rows(0)("Advance_deduction_amount").ToString
            Txt_Remarks.Text = dt.Rows(0)("Remarks").ToString

            txt_Bank_Acc_Name.Text = dt.Rows(0)("Ledger_bank_Ac_Name").ToString
            txt_bankName.Text = dt.Rows(0)("Ledger_BankName").ToString
            txt_AccountNo.Text = dt.Rows(0)("Ledger_AccountNo").ToString
            txt_Branch.Text = dt.Rows(0)("Ledger_BranchName").ToString
            txt_Ifsc_Code.Text = dt.Rows(0)("Ledger_IFSCCode").ToString

            If Val(dt.Rows(0).Item("Show_In_All_Entry").ToString) = 1 Then Show_In_All_Entry.Checked = True

            txt_InsuranceNo.Text = Trim(dt.Rows(0)("Insurance_No").ToString)
            txt_NoofLoom.Text = Val(dt.Rows(0)("NoOf_Looms").ToString)
            txt_Freight_per_Loom.Text = Format(Val(dt.Rows(0)("Freight_Loom").ToString), "########0.00")
            txt_Production_per_Day.Text = Format(Val(dt.Rows(0)("Production_Per_Day").ToString), "########0.00")
            If Val(dt.Rows(0).Item("Own_Loom_Status").ToString) = 1 Then Own_Loom_Status.Checked = True
            txt_TdsPerc.Text = Format(Val(dt.Rows(0)("Tds_Perc").ToString), "########0.00")
            If Val(dt.Rows(0).Item("Close_Status").ToString) = 1 Then Close_Status.Checked = True
            If Val(dt.Rows(0).Item("Stock_Maintenance_Status").ToString) = 1 Then chk_Stock_Maintenance.Checked = True
            cbo_Transfer_StockTo.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt.Rows(0).Item("Transfer_To_LedgerIdNo").ToString), , TrnTo_DbName)
            If Val(dt.Rows(0).Item("TCS_Sales_Status").ToString) = 1 Then Chk_Tcs_for_Sales_sts.Checked = True
            If Val(dt.Rows(0).Item("Sales_TDS_Deduction_Status").ToString) = 1 Then Chk_Sales_Tds_Deduction_sts.Checked = True
            If Val(dt.Rows(0).Item("IsAbove_10_Crore_Status").ToString) = 1 Then Chk_Is_TurnOver_Above_10_Crore_Sts.Checked = True
            '-----------------GST ALTER------------------------------------
            txt_GSTIN_No.Text = dt.Rows(0)("Ledger_GSTinNo").ToString
            cbo_State.Text = Common_Procedures.State_IdNoToName(con, Val(dt.Rows(0)("Ledger_State_IdNo").ToString))
            '---------------------------------------------------------------
            txt_MeterPrReel.Text = dt.Rows(0)("Ledger_MeterPrReel").ToString
            txt_WeightPrReel.Text = dt.Rows(0)("Ledger_WeightPrReel").ToString
            txt_AadharNo.Text = dt.Rows(0)("Aadhar_No").ToString
            txt_Freight_Pavu.Text = dt.Rows(0)("Freight_Pavu").ToString
            txt_freight_Bundle.Text = dt.Rows(0)("Freight_Bundle").ToString
            txt_PanAddress1.Text = dt.Rows(0)("Pan_Address1").ToString
            txt_PanAddress2.Text = dt.Rows(0)("Pan_Address2").ToString
            txt_PanAddress3.Text = dt.Rows(0)("Pan_Address3").ToString
            txt_PanAddress4.Text = dt.Rows(0)("Pan_Address4").ToString

            Txt_TamilAddress_1.Text = dt.Rows(0)("Ledger_Tamil_Address1").ToString
            Txt_TamilAddress_2.Text = dt.Rows(0)("Ledger_Tamil_Address2").ToString

            '---------PAVU YARN STOCK LIMIT DETAILS--------
            txt_PavuMax.Text = dt.Rows(0).Item("Pavu_Stock_Maximum_Level").ToString
            txt_PavuMin.Text = dt.Rows(0).Item("Pavu_Stock_Minimum_Level").ToString
            txt_YarnMax.Text = dt.Rows(0).Item("Yarn_Stock_Maximum_Level").ToString
            txt_YarnMin.Text = dt.Rows(0).Item("Yarn_Stock_Minimum_Level").ToString

            '---------LEDGER ADDRESS PRINT SETUP--------
            txt_TopFromAdds.Text = dt.Rows(0).Item("FROMAddress_Topoint").ToString
            txt_TOPToAdds.Text = dt.Rows(0).Item("TOAddress_Topoint").ToString
            txt_LeftFromAdds.Text = dt.Rows(0).Item("FROMAddress_LeftPoint").ToString
            txt_LeftToAdds.Text = dt.Rows(0).Item("TOAddress_LeftPoint").ToString
            cbo_PaperOrientation.Text = dt.Rows(0).Item("Paper_Orientation").ToString
            cbo_FromAddress.Text = dt.Rows(0).Item("Company_Name").ToString
            If Val(dt.Rows(0).Item("FromAddress_SetPosition_Sts").ToString) = 1 Then
                chk_FromAddress.Checked = True
                txt_TopFromAdds.Enabled = True
                txt_LeftFromAdds.Enabled = True
            End If
            '---------LEDGER ADDRESS PRINT SETUP--------

            cbo_Weaver_LoomType.Text = dt.Rows(0).Item("Weaver_LoomType").ToString
            cbo_Sizing_To_CompanyName.Text = Common_Procedures.Company_IdNoToName(con, Val(dt.Rows(0).Item("Sizing_To_CompanyIdNo").ToString), SizTo_DbName)
            cbo_Sizing_To_VendorName.Text = Common_Procedures.Vendor_IdNoToName(con, Val(dt.Rows(0).Item("Sizing_To_VendorIdNo").ToString), , SizTo_DbName)



            txt_CreditLimit.Text = dt.Rows(0).Item("Credit_Limit_Amount").ToString
            txt_creditLimitDays.Text = dt.Rows(0).Item("Credit_Limit_Days").ToString

            txt_LegalName_Business.Text = dt.Rows(0)("Legal_Nameof_Business").ToString
            txt_city.Text = dt.Rows(0)("City_Town").ToString
            txt_pincode.Text = dt.Rows(0)("Pincode").ToString
            txt_vehicleNo.Text = dt.Rows(0)("vehicle_no").ToString
            txt_Distance.Text = dt.Rows(0)("Distance").ToString
            If Val(txt_Distance.Text) = 0 Then txt_Distance.Text = ""
            If Val(dt.Rows(0).Item("Ledger_GSTIN_Verified_Status").ToString) = 1 Then chk_GSTIN_Verified.Checked = True

            If Val(dt.Rows(0).Item("WeavingBill_IR_Receipt_Meters_Sts").ToString) = 1 Then chk_WeavingBill_IR_Receipt_Mtrs.Checked = True

            txt_contact_person_name.Text = dt.Rows(0)("Contact_Person").ToString
            ' cbo_designation.Text = Common_Procedures.Contact_Designation_IdNoToName(con, Val(dt.Rows(0)("Contact_Designation_IdNo").ToString))

            cbo_party_category.Text = Common_Procedures.Party_Category_IdNoToName(con, Val(dt.Rows(0)("Party_Category_IdNo").ToString))

            txt_Ledger_ShortName.Text = dt.Rows(0)("Ledger_ShortName").ToString
            cbo_marketting_Exec_Name.Text = dt.Rows(0)("Marketting_Executive_Name").ToString

            If Val(dt.Rows(0).Item("TCS_PURCHASE_Status").ToString) = 1 Then Chk_Tcs_for_Purchase_sts.Checked = True
            If Val(dt.Rows(0).Item("PURCHASE_TDS_Deduction_Status").ToString) = 1 Then Chk_Purchase_Tds_Deduction_sts.Checked = True

            da3 = New SqlClient.SqlDataAdapter("Select a.* , b.Colour_Name from Ledger_Rate_Details a INNER JOIN Colour_Head b ON a.Colour_IdNo = b.Colour_IdNo   Where  a.ledger_idno = " & Str(Val(idno)) & " Order by a.sl_no", con)
            dt3 = New DataTable
            da3.Fill(dt3)



            With dgv_RateDetails

                .Rows.Clear()
                SNo = 0

                If dt3.Rows.Count > 0 Then

                    For i = 0 To dt3.Rows.Count - 1

                        n = .Rows.Add()

                        SNo = SNo + 1

                        .Rows(n).Cells(0).Value = Val(SNo)
                        .Rows(n).Cells(1).Value = dt3.Rows(i).Item("Colour_Name").ToString
                        .Rows(n).Cells(2).Value = Format(Val(dt3.Rows(i).Item("RATE").ToString), "########0.000")

                    Next i

                End If

            End With

            da = New SqlClient.SqlDataAdapter("select a.*, b.Contact_Designation_Name  from Ledger_ContactName_Details a INNER JOIN Contact_Designation_Head b ON a.Contact_Designation_IdNo = b.Contact_Designation_IdNo   where a.Ledger_IdNo = " & Val(idno), con)
            da.Fill(dt2)

            dgv_Contact_Person_Details.Rows.Clear()
            SNo = 0

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Contact_Person_Details.Rows.Add()

                    SNo = SNo + 1
                    dgv_Contact_Person_Details.Rows(n).Cells(0).Value = Val(SNo)
                    dgv_Contact_Person_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Contact_Person").ToString
                    dgv_Contact_Person_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Phone_No").ToString
                    dgv_Contact_Person_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Ledger_Emailid").ToString
                    dgv_Contact_Person_Details.Rows(n).Cells(4).Value = dt2.Rows(i).Item("Contact_Designation_Name").ToString
                Next i

                For i = 0 To dgv_Contact_Person_Details.RowCount - 1
                    dgv_Contact_Person_Details.Rows(i).Cells(0).Value = Val(i) + 1
                Next

            End If

            da2 = New SqlClient.SqlDataAdapter("Select a.*  from Ledger_Freight_Charge_Details a    Where  a.ledger_idno = " & Str(Val(idno)) & " Order by a.sl_no", con)
            dt2 = New DataTable
            da2.Fill(dt2)

            With dgv_Freight_Charge_Details

                .Rows.Clear()
                SNo = 0

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = .Rows.Add()

                        SNo = SNo + 1

                        .Rows(n).Cells(0).Value = Val(SNo)
                        .Rows(n).Cells(1).Value = Format(Val(dt2.Rows(i).Item("From_Weight").ToString), "########0.000")
                        .Rows(n).Cells(2).Value = Format(Val(dt2.Rows(i).Item("To_Weight").ToString), "########0.000")
                        .Rows(n).Cells(3).Value = Format(Val(dt2.Rows(i).Item("Freight_Bag").ToString), "########0.00")
                    Next i

                End If

            End With

            da4 = New SqlClient.SqlDataAdapter("Select a.*, ch.Cloth_Name  from Ledger_Weaver_Wages_Details a INNER JOIN Cloth_Head CH on ch.cloth_IdNo = a.Cloth_IdNo Where  a.ledger_idno = " & Str(Val(idno)) & " Order by a.sl_no", con)
            dt4 = New DataTable
            da4.Fill(dt4)

            With dgv_Wages_Charge_Details

                .Rows.Clear()
                SNo = 0

                If dt4.Rows.Count > 0 Then

                    For i = 0 To dt4.Rows.Count - 1

                        n = .Rows.Add()

                        SNo = SNo + 1

                        .Rows(n).Cells(0).Value = Val(SNo)
                        '.Rows(n).Cells(1).Value = Format(Val(dt4.Rows(i).Item("From_Weight").ToString), "########0.000")
                        .Rows(n).Cells(1).Value = dt4.Rows(i).Item("Cloth_Name").ToString
                        .Rows(n).Cells(2).Value = Format(Val(dt4.Rows(i).Item("Type1_Wages_Rate").ToString), "########0.00")
                        .Rows(n).Cells(3).Value = Format(Val(dt4.Rows(i).Item("Type2_Wages_Rate").ToString), "########0.00")
                        .Rows(n).Cells(4).Value = Format(Val(dt4.Rows(i).Item("Type3_Wages_Rate").ToString), "########0.00")
                        .Rows(n).Cells(5).Value = Format(Val(dt4.Rows(i).Item("Type4_Wages_Rate").ToString), "########0.00")
                        .Rows(n).Cells(6).Value = Format(Val(dt4.Rows(i).Item("Type5_Wages_Rate").ToString), "########0.00")

                        .Rows(n).Cells(7).Value = dt4.Rows(i).Item("FROM_DATE").ToString
                        .Rows(n).Cells(8).Value = dt4.Rows(i).Item("TO_DATE").ToString
                        .Rows(n).Cells(9).Value = dt4.Rows(i).Item("NO_OF_LOOMS").ToString
                        .Rows(n).Cells(10).Value = Format(Val(dt4.Rows(i).Item("Production_Capacity").ToString), "########0.00")


                    Next i

                End If

            End With

            da1 = New SqlClient.SqlDataAdapter("Select a.*  from Weaver_Loom_Details a    Where  a.ledger_idno = " & Str(Val(idno)) & " Order by a.sl_no", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            With dgv_Loom_Details

                .Rows.Clear()
                SNo = 0

                If dt1.Rows.Count > 0 Then

                    For i = 0 To dt1.Rows.Count - 1

                        n = .Rows.Add()

                        SNo = SNo + 1

                        .Rows(n).Cells(0).Value = Val(SNo)
                        .Rows(n).Cells(1).Value = dt1.Rows(i).Item("Loom_No").ToString
                        .Rows(n).Cells(2).Value = Common_Procedures.Cloth_IdNoToName(con, Val(dt1.Rows(i).Item("Cloth_idno").ToString))


                    Next i


                End If

                dt1.Clear()

            End With

        End If

        Grid_Cell_DeSelect()
        dgv_ActiveCtrl_Name = ""
        dt.Dispose()
        da.Dispose()

        If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()

    End Sub


    Public Sub new_record() Implements Interface_MDIActions.new_record

        Call clear()

        lbl_IdNo.ForeColor = Color.Red
        New_Entry = True

        lbl_IdNo.Text = Common_Procedures.get_MaxIdNo(con, "ledger_head", "ledger_idno", "")

        If Val(lbl_IdNo.Text) < 101 Then lbl_IdNo.Text = 101

        If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim cmd As New SqlClient.SqlCommand
        Dim da As SqlClient.SqlDataAdapter
        Dim dt As DataTable

        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()

        If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Ledr_Wea_Siz_Rw_Trans_JbWrk_Creation, "~L~") = 0 And InStr(Common_Procedures.UR.Ledr_Wea_Siz_Rw_Trans_JbWrk_Creation, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub

        If MessageBox.Show("Do you want to delete", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

        Try

            If Val(lbl_IdNo.Text) < 101 Then
                MessageBox.Show("Cannot delete this default Ledger", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub
            End If

            da = New SqlClient.SqlDataAdapter("select count(*) from Voucher_Details where Ledger_Idno = " & Str(Val(lbl_IdNo.Text)), con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    If Val(dt.Rows(0)(0).ToString) > 0 Then
                        MessageBox.Show("Already used this Ledger", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If
            End If

            da = New SqlClient.SqlDataAdapter("select count(*) from Stock_Empty_BeamBagCone_Processing_Details where  DeliveryTo_Idno = " & Str(Val(lbl_IdNo.Text)) & " or ReceivedFrom_Idno = " & Str(Val(lbl_IdNo.Text)), con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    If Val(dt.Rows(0)(0).ToString) > 0 Then
                        MessageBox.Show("Already used this Ledger", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If
            End If

            da = New SqlClient.SqlDataAdapter("select count(*) from Stock_Item_Processing_Details where DeliveryTo_StockIdNo = " & Str(Val(lbl_IdNo.Text)) & " or ReceivedFrom_StockIdNo = " & Str(Val(lbl_IdNo.Text)) & " or Delivery_PartyIdNo = " & Str(Val(lbl_IdNo.Text)) & " or Received_PartyIdNo = " & Str(Val(lbl_IdNo.Text)), con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    If Val(dt.Rows(0)(0).ToString) > 0 Then
                        MessageBox.Show("Already used this Ledger", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If
            End If

            da = New SqlClient.SqlDataAdapter("select count(*) from Stock_Pavu_Processing_Details where DeliveryTo_Idno = " & Str(Val(lbl_IdNo.Text)) & " or ReceivedFrom_Idno = " & Str(Val(lbl_IdNo.Text)), con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    If Val(dt.Rows(0)(0).ToString) > 0 Then
                        MessageBox.Show("Already used this Ledger", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If
            End If

            da = New SqlClient.SqlDataAdapter("select count(*) from Stock_SizedPavu_Processing_Details where Ledger_IdNo = " & Str(Val(lbl_IdNo.Text)) & " or StockAt_IdNo = " & Str(Val(lbl_IdNo.Text)), con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    If Val(dt.Rows(0)(0).ToString) > 0 Then
                        MessageBox.Show("Already used this Ledger", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If
            End If

            da = New SqlClient.SqlDataAdapter("select count(*) from Stock_Yarn_Processing_Details where DeliveryTo_Idno = " & Str(Val(lbl_IdNo.Text)) & " or ReceivedFrom_Idno = " & Str(Val(lbl_IdNo.Text)), con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    If Val(dt.Rows(0)(0).ToString) > 0 Then
                        MessageBox.Show("Already used this Ledger", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If
            End If

            da = New SqlClient.SqlDataAdapter("select count(*) from Stock_Cloth_Processing_Details where DeliveryTo_Idno = " & Str(Val(lbl_IdNo.Text)) & " or ReceivedFrom_Idno = " & Str(Val(lbl_IdNo.Text)), con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    If Val(dt.Rows(0)(0).ToString) > 0 Then
                        MessageBox.Show("Already used this Ledger", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If
            End If

            da = New SqlClient.SqlDataAdapter("select count(*) from AgentCommission_Processing_Details where Agent_IdNo = " & Str(Val(lbl_IdNo.Text)) & " or Ledger_IdNo = " & Str(Val(lbl_IdNo.Text)), con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    If Val(dt.Rows(0)(0).ToString) > 0 Then
                        MessageBox.Show("Already used this Ledger", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If
            End If

            cmd.Connection = con





            cmd.CommandText = "delete from Weaver_Loom_Details where Ledger_IdNo = " & Str(Val(lbl_IdNo.Text))
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Ledger_Freight_Charge_Details where ledger_idno = " & Str(Val(lbl_IdNo.Text))
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Ledger_Rate_Details where ledger_idno = " & Str(Val(lbl_IdNo.Text))
            cmd.ExecuteNonQuery()


            cmd.CommandText = "delete from Ledger_AlaisHead where Ledger_IdNo = " & Str(Val(lbl_IdNo.Text))
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from ledger_head where ledger_idno = " & Str(Val(lbl_IdNo.Text))
            cmd.ExecuteNonQuery()

            If Trim(UCase(vLedType)) = Trim(UCase("WEAVER")) And Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then '---- BRT SPINNERS PRIVATE LIMITED (FABRIC DIVISION)
                If cbo_Sizing_To_VendorName.Visible = True Then
                    If Trim(SizTo_DbName) <> "" Then
                        cmd.CommandText = "Update " & Trim(SizTo_DbName) & "..Vendor_head SET Close_Status = 1 Where Textile_To_WeaverIdNo = " & Str(Val(lbl_IdNo.Text))
                        cmd.ExecuteNonQuery()
                        cmd.CommandText = "Update " & Trim(SizTo_DbName) & "..Vendor_AlaisHead SET Close_Status = 1 Where Textile_To_WeaverIdNo = " & Str(Val(lbl_IdNo.Text))
                        cmd.ExecuteNonQuery()
                    End If
                End If
            End If

            cmd.Dispose()
            dt.Dispose()
            da.Dispose()

            new_record()

            MessageBox.Show("Deleted Successfully!!", "FOR DELETION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT DELETE..", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()
            Exit Sub

        End Try

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()

        Dim da As New SqlClient.SqlDataAdapter("select ledger_idno, ledger_name from ledger_head where ledger_idno <> 0 and ledger_type = '" & Trim(vLedType) & "' order by ledger_idno", con)
        Dim dt As New DataTable

        da.Fill(dt)

        dgv_Filter.Columns.Clear()
        dgv_Filter.DataSource = dt
        dgv_Filter.RowHeadersVisible = False

        dgv_Filter.Columns(0).HeaderText = "IDNO"
        dgv_Filter.Columns(1).HeaderText = "LEDGER NAME"

        dgv_Filter.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
        dgv_Filter.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

        dgv_Filter.Columns(0).FillWeight = 35
        dgv_Filter.Columns(1).FillWeight = 165

        grp_Filter.Visible = True
        grp_Filter.BringToFront()
        dgv_Filter.Focus()

        grp_Back.Enabled = False

        da.Dispose()

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim cmd As New SqlClient.SqlCommand
        Dim movid As Integer

        Try

            con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
            con.Open()

            cmd.Connection = con
            cmd.CommandText = "select min(ledger_idno) from ledger_head Where ledger_idno <> 0 and ledger_type = '" & Trim(vLedType) & "'"

            movid = 0
            If IsDBNull(cmd.ExecuteScalar()) = False Then
                movid = cmd.ExecuteScalar
            End If

            cmd.Dispose()

            If movid <> 0 Then Call move_record(movid)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim cmd As New SqlClient.SqlCommand
        Dim movid As Integer

        Try

            con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
            con.Open()

            cmd.Connection = con
            cmd.CommandText = "select max(ledger_idno) from ledger_head where ledger_idno <> 0 and ledger_type = '" & Trim(vLedType) & "'"

            movid = 0
            If IsDBNull(cmd.ExecuteScalar()) = False Then
                movid = cmd.ExecuteScalar
            End If

            cmd.Dispose()

            If movid <> 0 Then move_record(movid)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()

        Dim cmd As New SqlClient.SqlCommand("select min(ledger_idno) from ledger_head where ledger_idno <> 0 and ledger_idno > " & Str(Val(lbl_IdNo.Text)) & " and ledger_type = '" & Trim(vLedType) & "'", con)
        Dim movid As Integer = 0

        Try


            movid = 0
            If IsDBNull(cmd.ExecuteScalar()) = False Then
                movid = cmd.ExecuteScalar
            End If

            cmd.Dispose()

            If movid <> 0 Then move_record(movid)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim cmd As New SqlClient.SqlCommand
        Dim movid As Integer = 0

        Try

            con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
            con.Open()

            cmd.Connection = con
            cmd.CommandText = "select max(ledger_idno) from ledger_head where ledger_idno <> 0 and ledger_idno < " & Str((lbl_IdNo.Text)) & " and ledger_type = '" & Trim(vLedType) & "'"

            movid = 0
            If IsDBNull(cmd.ExecuteScalar()) = False Then
                movid = cmd.ExecuteScalar()
            End If

            cmd.Dispose()

            If movid <> 0 Then move_record(movid)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()

        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or Ledger_type = '" & Trim(vLedType) & "') order by Ledger_DisplayName", con)
        dt = New DataTable
        da.Fill(dt)

        cbo_Open.DataSource = dt
        cbo_Open.DisplayMember = "Ledger_DisplayName"

        da.Dispose()

        grp_Open.Visible = True
        grp_Open.BringToFront()
        If cbo_Open.Enabled And cbo_Open.Visible Then cbo_Open.Focus()
        grp_Back.Enabled = False

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        'MessageBox.Show("insert record")
    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim dt As New DataTable
        Dim nr As Long = 0
        Dim acgrp_idno As Integer = 0
        Dim ar_idno As Integer = 0
        Dim Parnt_CD As String = ""
        Dim LedName As String = ""
        Dim SurName As String = ""
        Dim clth_ID As Integer = 0
        Dim LedArName As String = ""
        Dim LedPhNo As String = ""
        Dim Transtk_Id As Integer = 0
        Dim Grp_idno As Integer = 0
        Dim Sno As Integer = 0
        Dim undgrp_ParntCD As String = ""
        Dim LedAls_AcGrp_idno As Integer = 0
        Dim vState_ID As Integer = 0
        Dim Color_Id As Integer = 0
        Dim Slno As Integer = 0
        Dim SizCmpstk_Id As Integer = 0
        Dim SizVndrstk_Id As Integer = 0
        Dim Vdesignation_Id As Integer = 0
        Dim VPartyCatg_Id As Integer = 0
        Dim FrmAddschk_Sts As Integer = 0
        Dim Cmp_Name As String = ""
        Dim vGST_Verfy_STS As Integer = 0
        Dim vWeaverBill_IR_Receipt_STS As Integer = 0
        Dim MarkExec_Id As Integer = 0
        Dim vEMAILID As String
        Dim vCLOSE_STS As Integer = 0
        Dim VCloth_Idno As Integer = 0
        Dim vTCS_PUR_STS As Integer = 0
        Dim VTDS_PUR_STS As Integer = 0

        If grp_Back.Enabled = False Then
            MessageBox.Show("Close other window", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Trim(txt_Name.Text) = "" Then
            MessageBox.Show("Invalid Legder Name", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If txt_Name.Enabled Then txt_Name.Focus()
            Exit Sub
        End If


        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Ledr_Wea_Siz_Rw_Trans_JbWrk_Creation, New_Entry, Me) = False Then Exit Sub

        acgrp_idno = Common_Procedures.AccountsGroup_NameToIdNo(con, cbo_AcGroup.Text)
        If acgrp_idno = 0 Then
            MessageBox.Show("Invalid Accounts Group", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_AcGroup.Enabled Then cbo_AcGroup.Focus()
            Exit Sub
        End If

        Parnt_CD = Common_Procedures.AccountsGroup_IdNoToCode(con, acgrp_idno)

        vState_ID = Common_Procedures.State_NameToIdNo(con, cbo_State.Text)

        Grp_idno = Common_Procedures.Ledger_NameToIdNo(con, cbo_LedgerGroup.Text)

        Vdesignation_Id = Common_Procedures.Contact_Designation_NameToIdNo(con, cbo_designation.Text)
        VPartyCatg_Id = Common_Procedures.Party_Category_NameToIdNo(con, cbo_party_category.Text)

        MarkExec_Id = Common_Procedures.MarketingExecutive_NameToIdNo(con, cbo_marketting_Exec_Name.Text)

        If Val(Grp_idno) = 0 Then
            Grp_idno = Val(lbl_IdNo.Text)
        End If
        If InStr(1, Parnt_CD, "~10~4~") = 0 And InStr(1, Parnt_CD, "~14~11~") = 0 Then
            cbo_BillType.Text = "BALANCE ONLY"
        End If

        If Trim(cbo_BillType.Text) = "" Then
            cbo_BillType.Text = "BALANCE ONLY"
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1132" Then
            If InStr(1, Parnt_CD, "~10~4~") > 0 Or InStr(1, Parnt_CD, "~14~11~") > 0 Then
                If vState_ID = 0 Then
                    MessageBox.Show("Invalid State", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    If cbo_State.Enabled Then cbo_State.Focus()
                    Exit Sub
                End If
            End If
        End If

        ar_idno = Common_Procedures.Area_NameToIdNo(con, cbo_Area.Text)

        Transtk_Id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Transfer_StockTo.Text, , TrnTo_DbName)
        If cbo_Transfer_StockTo.Visible Then
            If Trim(cbo_Transfer_StockTo.Text) <> "" Then
                If Val(Transtk_Id) = 0 Then
                    MessageBox.Show("Invalid Transfer Stock To", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    If cbo_Transfer_StockTo.Enabled Then cbo_Transfer_StockTo.Focus()
                    Exit Sub
                End If
            End If
        End If

        If Trim(txt_GSTIN_No.Text) <> "" Then
            txt_GSTIN_No.Text = Trim(txt_GSTIN_No.Text)
            txt_GSTIN_No.Text = Replace(Trim(txt_GSTIN_No.Text), "  ", "")
            txt_GSTIN_No.Text = Replace(Trim(txt_GSTIN_No.Text), " ", "")
            If Trim(txt_GSTIN_No.Text) <> "URP" Then
                If Len(Trim(txt_GSTIN_No.Text)) <> 15 Then
                    MessageBox.Show("Invalid GSTIN Number  (should be 15 digit)", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    If txt_GSTIN_No.Enabled Then txt_GSTIN_No.Focus()
                    Exit Sub
                End If
            End If
        End If

        If Trim(txt_PanNo.Text) <> "" Then
            txt_PanNo.Text = Trim(txt_PanNo.Text)
            txt_PanNo.Text = Replace(Trim(txt_PanNo.Text), " ", "")
            If Len(Trim(txt_PanNo.Text)) <> 10 Then
                MessageBox.Show("Invalid PAN Number  (should be 10 digit)", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If txt_PanNo.Enabled Then txt_PanNo.Focus()
                Exit Sub
            End If
        End If

        If Trim(txt_AadharNo.Text) <> "" Then
            txt_AadharNo.Text = Trim(txt_AadharNo.Text)
            txt_AadharNo.Text = Replace(Trim(txt_AadharNo.Text), " ", "")
            If Len(Trim(txt_AadharNo.Text)) <> 12 Then
                MessageBox.Show("Invalid Aadhar Number (should be 12 digit)", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If txt_AadharNo.Enabled Then txt_AadharNo.Focus()
                Exit Sub
            End If
        End If

        LedName = Trim(txt_Name.Text)
        If Val(ar_idno) <> 0 Then
            LedName = Trim(txt_Name.Text) & " (" & Trim(cbo_Area.Text) & ")"
        End If

        'If Trim(txt_GSTIN_No.Text) <> "" And Trim(txt_GSTIN_No.Text) <> "URP" Then
        '    If chk_GSTIN_Verified.Checked = False Then
        '        If My.Computer.Network.IsAvailable = True Then
        '            If My.Computer.Network.Ping("www.Google.com") = True Then
        '                If Verify_GSTIN(True) = False Then
        '                    If txt_GSTIN_No.Enabled Then txt_GSTIN_No.Focus()
        '                    Exit Sub
        '                End If
        '            End If
        '        End If
        '    End If
        'End If

        Show_STS = 0
        If Show_In_All_Entry.Checked = True Then Show_STS = 1

        OwmLoom_STS = 0
        If Own_Loom_Status.Checked = True Then OwmLoom_STS = 1

        vCLOSE_STS = 0
        If Close_Status.Checked = True Then vCLOSE_STS = 1

        TCS_STS = 0
        If Chk_Tcs_for_Sales_sts.Checked = True Then TCS_STS = 1

        Stock_STS = 0
        If chk_Stock_Maintenance.Checked = True Then Stock_STS = 1

        FrmAddschk_Sts = 0
        If chk_FromAddress.Checked = True Then FrmAddschk_Sts = 1

        vGST_Verfy_STS = 0
        If chk_GSTIN_Verified.Checked = True Then vGST_Verfy_STS = 1

        vWeaverBill_IR_Receipt_STS = 0
        If chk_WeavingBill_IR_Receipt_Mtrs.Checked = True Then vWeaverBill_IR_Receipt_STS = 1

        TDS_STS = 0
        If Chk_Sales_Tds_Deduction_sts.Checked = True Then TDS_STS = 1

        IsAbove_10CR_STS = 0
        If Chk_Is_TurnOver_Above_10_Crore_Sts.Checked = True Then IsAbove_10CR_STS = 1

        If TDS_STS = 1 And TCS_STS = 1 Then
            MessageBox.Show("Invalid TCS/TDS Deduction Selection" & Chr(13) & "Should not select both TDS/TCS", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If Chk_Sales_Tds_Deduction_sts.Enabled And Chk_Sales_Tds_Deduction_sts.Visible Then Chk_Sales_Tds_Deduction_sts.Focus()
            Exit Sub
        End If


        vTCS_PUR_STS = 0
        If Chk_Tcs_for_Purchase_sts.Checked = True Then vTCS_PUR_STS = 1

        VTDS_PUR_STS = 0
        If Chk_Purchase_Tds_Deduction_sts.Checked = True Then VTDS_PUR_STS = 1

        If vTCS_PUR_STS = 1 And VTDS_PUR_STS = 1 Then
            MessageBox.Show("Invalid TCS/TDS Deduction Selection" & Chr(13) & "Should not select both TDS/TCS", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If Chk_Purchase_Tds_Deduction_sts.Enabled And Chk_Purchase_Tds_Deduction_sts.Visible Then Chk_Purchase_Tds_Deduction_sts.Focus()
            Exit Sub
        End If

        SurName = Common_Procedures.Remove_NonCharacters(LedName)

        If Common_Procedures.Check_Duplicate_LedgerName(con, Val(lbl_IdNo.Text), SurName) = True Then
            'MessageBox.Show("Duplicate Ledger Name", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If txt_Name.Enabled Then txt_Name.Focus()
            Exit Sub
        End If

        SizCmpstk_Id = 0
        If cbo_Sizing_To_CompanyName.Visible Then
            If Trim(cbo_Sizing_To_CompanyName.Text) <> "" Then
                SizCmpstk_Id = Common_Procedures.Company_NameToIdNo(con, cbo_Sizing_To_CompanyName.Text, SizTo_DbName)
                If Val(SizCmpstk_Id) = 0 Then
                    MessageBox.Show("Invalid Sizing Company Name", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    If cbo_Sizing_To_CompanyName.Enabled Then cbo_Sizing_To_CompanyName.Focus()
                    Exit Sub
                End If
            End If
        End If

        SizVndrstk_Id = Common_Procedures.Vendor_NameToIdNo(con, cbo_Sizing_To_VendorName.Text, , SizTo_DbName)
        If cbo_Sizing_To_VendorName.Visible Then
            If Trim(cbo_Sizing_To_VendorName.Text) <> "" Then
                If Val(SizVndrstk_Id) = 0 Then
                    MessageBox.Show("Invalid Sizing Vendor Name", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    If cbo_Sizing_To_VendorName.Enabled Then cbo_Sizing_To_VendorName.Focus()
                    Exit Sub
                End If
            End If
        End If

        With dgv_Contact_Person_Details

            For i = 0 To .RowCount - 1

                If Trim(.Rows(i).Cells(2).Value) <> "" Then

                    If Trim(.Rows(i).Cells(1).Value) = "" Then

                        Timer1.Enabled = False
                        SaveAll_STS = False

                        MessageBox.Show("Invalid Contact Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If dgv_Contact_Person_Details.Enabled Then
                            dgv_Contact_Person_Details.Focus()
                            dgv_Contact_Person_Details.CurrentCell = dgv_Contact_Person_Details.Rows(i).Cells(1)
                            dgv_Contact_Person_Details.CurrentCell.Selected = True
                        End If

                        Timer1.Enabled = False
                        SaveAll_STS = False

                        Exit Sub

                    End If

                    If Trim(dgv_Contact_Person_Details.Rows(i).Cells(3).Value) <> "" Then

                        vEMAILID = Replace(Trim(dgv_Contact_Person_Details.Rows(i).Cells(3).Value), "   ", "")
                        vEMAILID = Replace(Trim(vEMAILID), "  ", "")
                        vEMAILID = Replace(Trim(vEMAILID), " ", "")
                        If Microsoft.VisualBasic.Right(Trim(vEMAILID), 1) = ";" Or Microsoft.VisualBasic.Right(vEMAILID, 1) = "," Then
                            vEMAILID = Microsoft.VisualBasic.Left(Trim(vEMAILID), Len(Trim(vEMAILID)) - 1)
                        End If

                        dgv_Contact_Person_Details.Rows(i).Cells(3).Value = Trim(vEMAILID)

                        If Common_Procedures.Validate_Email_ID(Trim(dgv_Contact_Person_Details.Rows(i).Cells(3).Value)) = False Then

                            Timer1.Enabled = False
                            SaveAll_STS = False

                            MessageBox.Show("Invalid E-Mail ID", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                            If dgv_Contact_Person_Details.Enabled Then
                                dgv_Contact_Person_Details.Focus()
                                dgv_Contact_Person_Details.CurrentCell = dgv_Contact_Person_Details.Rows(i).Cells(1)
                                dgv_Contact_Person_Details.CurrentCell.Selected = True
                            End If




                            Exit Sub

                        End If

                    End If




                End If


            Next

        End With



        Cmp_Name = Val(Common_Procedures.Company_NameToIdNo(con, cbo_FromAddress.Text))


        trans = con.BeginTransaction

        Try

            cmd.Transaction = trans

            cmd.Connection = con

            If New_Entry = True Then
                lbl_IdNo.Text = Common_Procedures.get_MaxIdNo(con, "ledger_head", "ledger_idno", "", trans)
                If Val(lbl_IdNo.Text) < 101 Then lbl_IdNo.Text = 101

                cmd.CommandText = "Insert into ledger_head(         Ledger_IdNo           ,       Ledger_Name      ,           Sur_Name     ,    Ledger_MainName           ,        Ledger_AlaisName           ,             Area_IdNo    ,     AccountsGroup_IdNo      ,       Parent_Code       ,               Bill_Type          ,        Ledger_Address1           ,        Ledger_Address2           ,        Ledger_Address3           ,        Ledger_Address4           ,        Ledger_PhoneNo           ,        Ledger_TinNo           ,        Ledger_CstNo           ,        Ledger_Type       ,         MobileNo_Frsms             ,        Show_In_All_Entry   ,        Ledger_Mail           ,                 NoOf_Looms         ,                  Freight_Loom       ,             Own_Loom_Status   , Verified_Status ,               Pan_No            ,               Owner_Name          ,                 Tds_perc            ,              Close_Status   , Transfer_To_LedgerIdNo, Stock_Maintenance_Status,              Tamil_Name          ,            Advance_deduction_amount    ,       Ledger_GSTinNo            ,      Ledger_State_IdNo    ,         Ledger_MeterPrReel           ,         Ledger_WeightPrReel            ,               Insurance_No          ,              Aadhar_No          ,             Freight_Pavu          ,              Pan_Address1          ,               Pan_Address2          ,               Pan_Address3          ,               Pan_Address4          ,               Weaver_LoomType           , Sizing_To_CompanyIdNo   ,   Sizing_To_VendorIdNo    ,   Pavu_Stock_Maximum_Level    ,    Pavu_Stock_Minimum_Level     ,    Yarn_Stock_Maximum_Level   ,   Yarn_Stock_Minimum_Level    ,             Freight_Bundle           ,           FROMAddress_Topoint    ,         FROMAddress_LeftPoint      ,           TOAddress_Topoint     ,        TOAddress_LeftPoint        ,               Paper_Orientation           , FromAddress_SetPosition_Sts ,      Company_IdNo        ,LedgerGroup_Idno     ,                 Credit_Limit_Amount ,                Credit_Limit_Days,                                   Production_Per_Day      , Legal_Nameof_Business , City_Town , Pincode ,       Distance          ,                                                                  Ledger_GSTIN_Verified_Status,                 vehicle_no               ,      WeavingBill_IR_Receipt_Meters_Sts   ,      TCS_Sales_Status    ,       Ledger_tamil_Address1            ,           Ledger_tamil_Address2        ,              Remarks           ,                Contact_Person ,                                Party_Category_IdNo        ,             Ledger_ShortName            ,    Marketting_Executive_IdNo       ,      Sales_TDS_Deduction_Status ,        IsAbove_10_Crore_Status     ,   TCS_purchase_Status          ,  purchase_TDS_Deduction_Status ,            Ledger_bank_Ac_Name        ,             Ledger_BankName       ,            Ledger_AccountNo        ,         Ledger_BranchName       ,             Ledger_IFSCCode          )  " &
                                  "Values                 (" & Str(Val(lbl_IdNo.Text)) & ", '" & Trim(LedName) & "', '" & Trim(SurName) & "', '" & Trim(txt_Name.Text) & "', '" & Trim(txt_AlaisName.Text) & "', " & Str(Val(ar_idno)) & ", " & Str(Val(acgrp_idno)) & ", '" & Trim(Parnt_CD) & "', '" & Trim(cbo_BillType.Text) & "', '" & Trim(txt_Address1.Text) & "', '" & Trim(txt_Address2.Text) & "', '" & Trim(txt_Address3.Text) & "', '" & Trim(txt_Address4.Text) & "', '" & Trim(txt_PhoneNo.Text) & "', '" & Trim(txt_TinNo.Text) & "', '" & Trim(txt_CstNo.Text) & "', '" & Trim(vLedType) & "' , '" & Trim(txt_MobileSms.Text) & "' , " & Str(Val(Show_STS)) & " , '" & Trim(txt_Mail.Text) & "', " & Str(Val(txt_NoofLoom.Text)) & ",  " & Str(Val(txt_Freight_per_Loom.Text)) & ", " & Str(Val(OwmLoom_STS)) & " ,     1           , '" & Trim(txt_PanNo.Text) & "'  , '" & Trim(txt_OwnerName.Text) & "', " & Str(Val(txt_TdsPerc.Text)) & " ,  " & Str(Val(vCLOSE_STS)) & "," & Val(Transtk_Id) & "," & Val(Stock_STS) & "   ,'" & Trim(txt_TamilName.Text) & "'," & Val(txt_AdvanceLess_Amount.Text) & ",'" & Trim(txt_GSTIN_No.Text) & "'," & Str(vState_ID) & "," & Str(Val(txt_MeterPrReel.Text)) & "," & Str(Val(txt_WeightPrReel.Text)) & " , '" & Trim(txt_InsuranceNo.Text) & "','" & Trim(txt_AadharNo.Text) & "', " & Val(txt_Freight_Pavu.Text) & ",'" & Trim(txt_PanAddress1.Text) & "', '" & Trim(txt_PanAddress2.Text) & "', '" & Trim(txt_PanAddress3.Text) & "', '" & Trim(txt_PanAddress4.Text) & "', '" & Trim(cbo_Weaver_LoomType.Text) & "'," & Val(SizCmpstk_Id) & "," & Val(SizVndrstk_Id) & " , " & Val(txt_PavuMax.Text) & " ,   " & Val(txt_PavuMin.Text) & " , " & Val(txt_YarnMax.Text) & " , " & Val(txt_YarnMin.Text) & " , " & Val(txt_freight_Bundle.Text) & " ," & Val(txt_TopFromAdds.Text) & " , " & Val(txt_LeftFromAdds.Text) & " , " & Val(txt_TOPToAdds.Text) & " , " & Val(txt_LeftToAdds.Text) & " , '" & Trim(cbo_PaperOrientation.Text) & "' , " & Val(FrmAddschk_Sts) & " , " & Val(Cmp_Name) & "     , " & Val(Grp_idno) & "  ,   " & Val(txt_CreditLimit.Text) & " ," & Val(txt_creditLimitDays.Text) & " ,  " & Str(Val(txt_Production_per_Day.Text)) & " , '" & Trim(txt_LegalName_Business.Text) & "', '" & Trim(txt_city.Text) & "' , '" & Trim(txt_pincode.Text) & "' , " & Val(txt_Distance.Text) & ",  " & Val(vGST_Verfy_STS) & " ,'" & Trim(txt_vehicleNo.Text) & "' ,  " & Val(vWeaverBill_IR_Receipt_STS) & " ," & Str(Val(TCS_STS)) & " , '" & Trim(Txt_TamilAddress_1.Text) & "', '" & Trim(Txt_TamilAddress_2.Text) & "', '" & Trim(Txt_Remarks.Text) & "',      '" & Trim(txt_contact_person_name.Text) & "',   " & Str(Val(VPartyCatg_Id)) & "  , '" & Trim(txt_Ledger_ShortName.Text) & "' , " & Str(Val(MarkExec_Id)) & "      ,   " & Str(Val(TDS_STS)) & "     ,  " & Str(Val(IsAbove_10CR_STS)) & "," & Str(Val(vTCS_PUR_STS)) & "  , " & Str(Val(VTDS_PUR_STS)) & " , '" & Trim(txt_Bank_Acc_Name.Text) & "', '" & Trim(txt_bankName.Text) & "' , '" & Trim(txt_AccountNo.Text) & "' ,  '" & Trim(txt_Branch.Text) & "',    '" & Trim(txt_Ifsc_Code.Text) & "') "
                cmd.ExecuteNonQuery()

            Else

                cmd.CommandText = "Update ledger_head set Ledger_Name = '" & Trim(LedName) & "', Sur_Name = '" & Trim(SurName) & "', Ledger_MainName = '" & Trim(txt_Name.Text) & "', Ledger_AlaisName = '" & Trim(txt_AlaisName.Text) & "', Area_IdNo = " & Str(Val(ar_idno)) & ", AccountsGroup_IdNo = " & Str(Val(acgrp_idno)) & ", Parent_Code = '" & Trim(Parnt_CD) & "', Bill_Type = '" & Trim(cbo_BillType.Text) & "', Pan_No = '" & Trim(txt_PanNo.Text) & "'  , Owner_Name = '" & Trim(txt_OwnerName.Text) & "' , Ledger_Address1 = '" & Trim(txt_Address1.Text) & "', Ledger_Address2 = '" & Trim(txt_Address2.Text) & "', Ledger_Address3 = '" & Trim(txt_Address3.Text) & "', Ledger_Address4 = '" & Trim(txt_Address4.Text) & "', Ledger_PhoneNo = '" & Trim(txt_PhoneNo.Text) & "', Ledger_TinNo = '" & Trim(txt_TinNo.Text) & "', Ledger_CstNo = '" & Trim(txt_CstNo.Text) & "' , MobileNo_Frsms = '" & Trim(txt_MobileSms.Text) & "' , Show_In_All_Entry = " & Str(Val(Show_STS)) & " , Ledger_Mail =  '" & Trim(txt_Mail.Text) & "'  ,  NoOf_Looms =   " & Str(Val(txt_NoofLoom.Text)) & " , Freight_Loom =   " & Str(Val(txt_Freight_per_Loom.Text)) & " , Own_Loom_Status = " & Str(Val(OwmLoom_STS)) & " , Verified_Status = 1, Tds_Perc =  " & Str(Val(txt_TdsPerc.Text)) & " , Close_Status =   " & Str(Val(vCLOSE_STS)) & ",Transfer_To_LedgerIdNo = " & Val(Transtk_Id) & ",Stock_Maintenance_Status = " & Val(Stock_STS) & ",Tamil_Name ='" & Trim(txt_TamilName.Text) & "' ,Advance_deduction_amount =" & Val(txt_AdvanceLess_Amount.Text) & ",Ledger_GSTinNo='" & Trim(txt_GSTIN_No.Text) & "',Ledger_State_IdNo=" & Str(vState_ID) & ",Ledger_MeterPrReel=" & Str(Val(txt_MeterPrReel.Text)) & ",Ledger_WeightPrReel=" & Str(Val(txt_WeightPrReel.Text)) & ", Insurance_No ='" & Trim(txt_InsuranceNo.Text) & "' ,Aadhar_No  = '" & Trim(txt_AadharNo.Text) & "',Freight_Pavu = " & Val(txt_Freight_Pavu.Text) & " ,Pan_Address1 = '" & Trim(txt_PanAddress1.Text) & "', Pan_Address2 = '" & Trim(txt_PanAddress2.Text) & "', Pan_Address3 = '" & Trim(txt_PanAddress3.Text) & "', Pan_Address4 = '" & Trim(txt_PanAddress4.Text) & "' , Weaver_LoomType = '" & Trim(cbo_Weaver_LoomType.Text) & "',Sizing_To_CompanyIdNo = " & Val(SizCmpstk_Id) & " ,Sizing_To_VendorIdNo = " & Val(SizVndrstk_Id) & ",   Pavu_Stock_Maximum_Level = " & Val(txt_PavuMax.Text) & " , Pavu_Stock_Minimum_Level = " & Val(txt_PavuMin.Text) & " , Yarn_Stock_Maximum_Level = " & Val(txt_YarnMax.Text) & " , Yarn_Stock_Minimum_Level = " & Val(txt_YarnMin.Text) & " , Freight_Bundle = " & Val(txt_freight_Bundle.Text) & " , FROMAddress_Topoint = " & Val(txt_TopFromAdds.Text) & " , FROMAddress_LeftPoint = " & Val(txt_LeftFromAdds.Text) & " , TOAddress_Topoint = " & Val(txt_TOPToAdds.Text) & " , TOAddress_LeftPoint = " & Val(txt_LeftToAdds.Text) & " , Paper_Orientation = '" & Trim(cbo_PaperOrientation.Text) & "' ,FromAddress_SetPosition_Sts = " & Val(FrmAddschk_Sts) & ",LedgerGroup_Idno=" & Val(Grp_idno) & " , Company_IdNo = " & Val(Cmp_Name) & " ,Credit_Limit_Amount =" & Val(txt_CreditLimit.Text) & " ,Credit_Limit_Days= " & Val(txt_creditLimitDays.Text) & " , Production_Per_Day= " & Str(Val(txt_Production_per_Day.Text)) & " ,  Legal_Nameof_Business = '" & Trim(txt_LegalName_Business.Text) & "' , City_Town = '" & Trim(txt_city.Text) & "' , Pincode = '" & Trim(txt_pincode.Text) & "' ,  Distance = " & Val(txt_Distance.Text) & ", Ledger_GSTIN_Verified_Status = " & Val(vGST_Verfy_STS) & ",  vehicle_no = '" & Trim(txt_vehicleNo.Text) & "' , WeavingBill_IR_Receipt_Meters_Sts = " & Val(vWeaverBill_IR_Receipt_STS) & " ,TCS_Sales_Status=" & Str(Val(TCS_STS)) & " ,Ledger_tamil_Address1='" & Trim(Txt_TamilAddress_1.Text) & "', Ledger_tamil_Address2='" & Trim(Txt_TamilAddress_2.Text) & "' , Remarks = '" & Trim(Txt_Remarks.Text) & "', Contact_Person = '" & Trim(txt_contact_person_name.Text) & "' ,  Party_Category_IdNo = " & Str(Val(VPartyCatg_Id)) & "  , Ledger_ShortName = '" & Trim(txt_Ledger_ShortName.Text) & "' , Marketting_Executive_IdNo = " & Str(Val(MarkExec_Id)) & " , Sales_TDS_Deduction_Status= " & Str(Val(TDS_STS)) & " ,IsAbove_10_Crore_Status=" & Str(Val(IsAbove_10CR_STS)) & " ,TCS_purchase_Status  =" & Str(Val(vTCS_PUR_STS)) & " , purchase_TDS_Deduction_Status=" & Str(Val(VTDS_PUR_STS)) & " ,  Ledger_bank_Ac_Name = '" & Trim(txt_Bank_Acc_Name.Text) & "', Ledger_BankName = '" & Trim(txt_bankName.Text) & "', Ledger_AccountNo = '" & Trim(txt_AccountNo.Text) & "',Ledger_BranchName = '" & Trim(txt_Branch.Text) & "', Ledger_IFSCCode = '" & Trim(txt_Ifsc_Code.Text) & "' where Ledger_IdNo = " & Str(Val(lbl_IdNo.Text))
                cmd.ExecuteNonQuery()

            End If

            cmd.CommandText = "delete from Ledger_Rate_Details where Ledger_IdNo = " & Str(Val(lbl_IdNo.Text))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "delete from Weaver_Loom_Details where Ledger_IdNo = " & Str(Val(lbl_IdNo.Text))
            cmd.ExecuteNonQuery()
            With dgv_RateDetails

                Slno = 0
                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(2).Value) <> 0 Then

                        Slno = Slno + 1

                        Color_Id = Common_Procedures.Colour_NameToIdNo(con, .Rows(i).Cells(1).Value, trans)

                        cmd.CommandText = "Insert into Ledger_Rate_Details (    ledger_idno              ,           Sl_No     ,         Colour_IdNo    ,             RATE                            ) " &
                                                "     Values                 ( " & Str(Val(lbl_IdNo.Text)) & "," & Str(Val(Slno)) & ",     " & Str(Val(Color_Id)) & ",       " & Str(Val(.Rows(i).Cells(2).Value)) & "  ) "
                        cmd.ExecuteNonQuery()

                    End If

                Next
            End With

            cmd.CommandText = "delete from Ledger_ContactName_Details where Ledger_Idno = " & Str(Val(lbl_IdNo.Text))
            cmd.ExecuteNonQuery()

            With dgv_Contact_Person_Details

                Sno = 0

                For i = 0 To .RowCount - 1



                    If Trim(.Rows(i).Cells(1).Value) <> "" Then

                        Sno = Sno + 1

                        Vdesignation_Id = Common_Procedures.Contact_Designation_NameToIdNo(con, dgv_Contact_Person_Details.Rows(i).Cells(4).Value, trans)

                        cmd.CommandText = "Insert into Ledger_ContactName_Details(Ledger_Idno, sl_No, Contact_Person   ,Phone_No ,Ledger_Emailid , Contact_Designation_IdNo )values (" & Str(Val(lbl_IdNo.Text)) & ", " & Str(Val(Sno)) & ", '" & Trim(.Rows(i).Cells(1).Value) & "' , '" & Trim(.Rows(i).Cells(2).Value) & "' ,'" & Trim(.Rows(i).Cells(3).Value) & "',  " & Str(Val(Vdesignation_Id)) & " )"
                        cmd.ExecuteNonQuery()

                    End If

                Next

            End With

            cmd.CommandText = "delete from Ledger_Freight_Charge_Details where Ledger_IdNo = " & Str(Val(lbl_IdNo.Text))
            cmd.ExecuteNonQuery()
            With dgv_Freight_Charge_Details

                Slno = 0
                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(1).Value) <> 0 Then


                        Slno = Slno + 1



                        cmd.CommandText = "Insert into Ledger_Freight_Charge_Details (    ledger_idno              ,           Sl_No     ,         From_Weight                        ,             To_Weight                    ,          Freight_Bag      ) " &
                                                "     Values                 ( " & Str(Val(lbl_IdNo.Text)) & "," & Str(Val(Slno)) & ",       " & Str(Val(.Rows(i).Cells(1).Value)) & " , " & Str(Val(.Rows(i).Cells(2).Value)) & " , " & Str(Val(.Rows(i).Cells(3).Value)) & ") "
                        cmd.ExecuteNonQuery()

                    End If

                Next
            End With

            cmd.CommandText = "delete from Ledger_Weaver_Wages_Details where Ledger_IdNo = " & Str(Val(lbl_IdNo.Text))
            cmd.ExecuteNonQuery()


            With dgv_Wages_Charge_Details



                Slno = 0
                For i = 0 To .RowCount - 1
                    cmd.Parameters.Clear()

                    If Trim(.Rows(i).Cells(7).Value) <> "" Then
                        If IsDate(.Rows(i).Cells(7).Value) = True Then
                            cmd.Parameters.AddWithValue("@FromDate", CDate(.Rows(i).Cells(7).Value))
                        End If
                    End If

                    If Trim(.Rows(i).Cells(8).Value) <> "" Then
                        If IsDate(.Rows(i).Cells(8).Value) = True Then
                            cmd.Parameters.AddWithValue("@toDate", CDate(.Rows(i).Cells(8).Value))
                        End If
                    End If
                    clth_ID = Common_Procedures.Cloth_NameToIdNo(con, .Rows(i).Cells(1).Value, trans)

                    If Val(clth_ID) <> 0 Then


                        Slno = Slno + 1

                        cmd.CommandText = "Insert into Ledger_Weaver_Wages_Details (   ledger_idno           ,           Sl_No     ,         Cloth_IdNo          ,             Type1_Wages_Rate             ,          Type2_Wages_Rate                ,   Type3_Wages_Rate                         ,  Type4_Wages_Rate                           ,    Type5_Wages_Rate                                 ,FROM_DATE                                  ,TO_DATE                                ,NO_OF_LOOMS                               ,FROM_DATE_time                                                              ,                                                To_DATE_time           ,           Production_Capacity)  " &
                                                "     Values                 ( " & Str(Val(lbl_IdNo.Text)) & "," & Str(Val(Slno)) & ",        " & Str(Val(clth_ID)) & " , " & Str(Val(.Rows(i).Cells(2).Value)) & " , " & Str(Val(.Rows(i).Cells(3).Value)) & "  ,   " & Str(Val(.Rows(i).Cells(4).Value)) & ",  " & Str(Val(.Rows(i).Cells(5).Value)) & ",  " & Str(Val(.Rows(i).Cells(6).Value)) & ",'" & Trim(.Rows(i).Cells(7).Value) & "','" & Trim(.Rows(i).Cells(8).Value) & "' ," & Str(Val(.Rows(i).Cells(9).Value)) & "   ," & IIf(IsDate(.Rows(i).Cells(7).Value) = True, "@fromDate", "Null") & "          ," & IIf(IsDate(.Rows(i).Cells(8).Value) = True, "@toDate", "Null") & "  ," & Str(Val(.Rows(i).Cells(10).Value)) & " ) "
                        cmd.ExecuteNonQuery()

                    End If

                Next
            End With
            With dgv_Loom_Details

                Sno = 0
                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(1).Value) <> 0 Then

                        Sno = Sno + 1

                        VCloth_Idno = Val(Common_Procedures.Cloth_NameToIdNo(con, .Rows(i).Cells(2).Value, trans))

                        cmd.CommandText = "Insert into Weaver_Loom_Details (    ledger_idno              ,           Sl_No           ,   Loom_No                                  ,        Cloth_Idno   ) " &
                                                "     Values                 ( " & Str(Val(lbl_IdNo.Text)) & "," & Str(Val(Sno)) & ",   " & Str(Val(.Rows(i).Cells(1).Value)) & " , " & Str(Val(VCloth_Idno)) & " ) "
                        cmd.ExecuteNonQuery()

                    End If

                Next

            End With

            LedAls_AcGrp_idno = acgrp_idno
            undgrp_ParntCD = Trim(Parnt_CD)

            While LedAls_AcGrp_idno > 32
                undgrp_ParntCD = Replace(undgrp_ParntCD, "~" & Trim(Val(LedAls_AcGrp_idno)) & "~", "")

                undgrp_ParntCD = "~" & Trim(undgrp_ParntCD)

                LedAls_AcGrp_idno = Val(Common_Procedures.get_FieldValue(con, "AccountsGroup_Head", "AccountsGroup_IdNo", "(Parent_Idno = '" & Trim(undgrp_ParntCD) & "')", , trans))
            End While

            'If acgrp_idno > 30 Then
            '    undgrp_ParntCD = Replace(Parnt_CD, "~" & Trim(Val(acgrp_idno)) & "~", "")

            '    undgrp_ParntCD = "~" & Trim(undgrp_ParntCD)

            '    LedAls_AcGrp_idno = Val(Common_Procedures.get_FieldValue(con, "AccountsGroup_Head", "AccountsGroup_IdNo", "(Parent_Idno = '" & Trim(undgrp_ParntCD) & "')", , trans))

            'Else
            '    LedAls_AcGrp_idno = acgrp_idno

            'End If

            cmd.CommandText = "delete from Ledger_AlaisHead where Ledger_IdNo = " & Str(Val(lbl_IdNo.Text))
            cmd.ExecuteNonQuery()


            LedArName = Trim(txt_Name.Text)
            If Val(ar_idno) <> 0 Then
                LedArName = Trim(txt_Name.Text) & " (" & Trim(cbo_Area.Text) & ")"
            End If

            cmd.CommandText = "Insert into Ledger_AlaisHead(Ledger_IdNo, Sl_No, Ledger_DisplayName, AccountsGroup_IdNo, Ledger_Type, Own_Loom_Status, Show_In_All_Entry , Verified_Status, Close_Status ,Stock_Maintenance_Status) Values (" & Str(Val(lbl_IdNo.Text)) & ", 1, '" & Trim(LedArName) & "', " & Str(Val(LedAls_AcGrp_idno)) & ", '" & Trim(vLedType) & "', " & Str(Val(OwmLoom_STS)) & ", " & Str(Val(Show_STS)) & " , " & Str(Val(Show_STS)) & ", " & Str(Val(vCLOSE_STS)) & "," & Val(Stock_STS) & ")"
            cmd.ExecuteNonQuery()

            If Trim(txt_AlaisName.Text) <> "" Then

                LedArName = Trim(txt_AlaisName.Text)
                If Val(ar_idno) <> 0 Then
                    LedArName = Trim(txt_AlaisName.Text) & " (" & Trim(cbo_Area.Text) & ")"
                End If

                cmd.CommandText = "Insert into Ledger_AlaisHead(Ledger_IdNo, Sl_No, Ledger_DisplayName, AccountsGroup_IdNo, Ledger_Type, Own_Loom_Status, Show_In_All_Entry , Verified_Status,Close_Status,Stock_Maintenance_Status ) Values (" & Str(Val(lbl_IdNo.Text)) & ", 2, '" & Trim(LedArName) & "', " & Str(Val(LedAls_AcGrp_idno)) & ", '" & Trim(vLedType) & "', " & Str(Val(OwmLoom_STS)) & ", " & Str(Val(Show_STS)) & " , " & Str(Val(Show_STS)) & "," & Str(Val(vCLOSE_STS)) & " ," & Val(Stock_STS) & ")"
                cmd.ExecuteNonQuery()

            End If


            If Trim(UCase(vLedType)) = Trim(UCase("WEAVER")) And Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then '---- BRT SPINNERS PRIVATE LIMITED (FABRIC DIVISION)
                If cbo_Sizing_To_VendorName.Visible = True Then
                    If Trim(SizTo_DbName) <> "" Then
                        cmd.CommandText = "Update " & Trim(SizTo_DbName) & "..Vendor_head SET Close_Status = " & Str(Val(vCLOSE_STS)) & " Where Textile_To_WeaverIdNo = " & Str(Val(lbl_IdNo.Text))
                        cmd.ExecuteNonQuery()
                    End If
                End If
            End If



            trans.Commit()

            trans.Dispose()
            dt.Dispose()

            Common_Procedures.Master_Return.Return_Value = Trim(LedName)
            Common_Procedures.Master_Return.Master_Type = "LEDGER"



            If SaveAll_STS <> True Then
                MessageBox.Show("Saved Successfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
            End If


            If Val(Common_Procedures.settings.OnSave_MoveTo_NewEntry_Status) = 1 Then
                If New_Entry = True Then
                    new_record()
                Else
                    move_record(lbl_IdNo.Text)
                End If
            Else
                move_record(lbl_IdNo.Text)
            End If

        Catch ex As Exception
            trans.Rollback()

            Timer1.Enabled = False
            SaveAll_STS = False

            If InStr(1, Trim(LCase(ex.Message)), "ix_ledger_head") > 0 Then
                MessageBox.Show("Duplicate Ledger Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            ElseIf InStr(1, Trim(LCase(ex.Message)), "ix_ledger_alaishead") > 0 Then
                MessageBox.Show("Duplicate Ledger Alais Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            ElseIf InStr(1, Trim(LCase(ex.Message)), Trim(LCase("PK_Weaver_Loom_Details"))) > 0 Then
                MessageBox.Show("Duplicate Loom No", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Else
                MessageBox.Show(ex.Message, "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            End If
            Exit Sub

        End Try

    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        save_record()
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub Ledger_Creation_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Area.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "AREA" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
            cbo_Area.Text = Trim(Common_Procedures.Master_Return.Return_Value)
        End If
        'If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_designation.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "DESIGNATION NAME" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
        '    cbo_designation.Text = Trim(Common_Procedures.Master_Return.Return_Value)
        'End If
        If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_party_category.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "PARTY CATEGORY " And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
            cbo_party_category.Text = Trim(Common_Procedures.Master_Return.Return_Value)
        End If
        If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(Cbo_Gird_desigantion.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "DESIGNATION NAME" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
            Cbo_Gird_desigantion.Text = Trim(Common_Procedures.Master_Return.Return_Value)
        End If

        If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_marketting_Exec_Name.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "MAREXEC" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
            cbo_marketting_Exec_Name.Text = Trim(Common_Procedures.Master_Return.Return_Value)
        End If

        If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_ClothName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CLOTH" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
            cbo_Grid_ClothName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
        End If

        Common_Procedures.Master_Return.Master_Type = ""
        Common_Procedures.Master_Return.Return_Value = ""

        FrmLdSTS = False
    End Sub

    Private Sub Ledger_Creation_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim TrnTo_CmpGrpIdNo As Integer = 0
        con.Open()

        FrmLdSTS = True

        cbo_Transfer_StockTo.Visible = False
        lbl_Transfer_StockTo_Caption.Visible = False


        TrnTo_CmpGrpIdNo = Val(Common_Procedures.get_FieldValue(con, Trim(Common_Procedures.CompanyDetailsDataBaseName) & "..CompanyGroup_Head", "Transfer_To_CompanyGroupIdNo", "(CompanyGroup_IdNo = " & Str(Val(Common_Procedures.CompGroupIdNo)) & ")"))
        If Val(TrnTo_CmpGrpIdNo) <> 0 Then
            TrnTo_DbName = Common_Procedures.get_Company_DataBaseName(Trim(Val(TrnTo_CmpGrpIdNo)))
            cbo_Transfer_StockTo.Visible = True
            cbo_Transfer_StockTo.BackColor = Color.White
            cbo_Transfer_StockTo.Width = txt_AadharNo.Width

            lbl_Transfer_StockTo_Caption.Visible = True
            lbl_Transfer_StockTo_Caption.Text = "Transfer StockTo"
            lbl_Transfer_StockTo_Caption.BackColor = grp_Back.BackColor


        Else
            TrnTo_DbName = Common_Procedures.get_Company_DataBaseName(Trim(Val(Common_Procedures.CompGroupIdNo)))

        End If

        If IsNothing(Common_Procedures.MDI_LedType) = True Then
            vLedType = ""
        ElseIf Trim(UCase(Common_Procedures.MDI_LedType)) = Trim(UCase("Ledger_Creation")) Then
            vLedType = ""
        Else
            vLedType = Trim(Common_Procedures.MDI_LedType)
        End If
        lbl_Production_per_day.Visible = False
        txt_Production_per_Day.Visible = False


        lbl_AdvanceLess_Amount.Visible = False
        txt_AdvanceLess_Amount.Visible = False
        If Trim(UCase(vLedType)) = "WEAVER" And Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1114" Then '---- Sundara mills
            lbl_AdvanceLess_Amount.Visible = True
            txt_AdvanceLess_Amount.Visible = True

            btn_Tamil_Address.Visible = True

        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1413" Then
            btn_SaveAll.Visible = True
            btn_BobinReelDetails.Visible = True
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1135" Then
            btn_BobinReelDetails.Visible = True
        End If
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1428" Then

            If Trim(UCase(vLedType)) = "WEAVER" Then
                lbl_ledger_ShortName.Visible = True
                txt_Ledger_ShortName.Visible = True
                txt_Name.Width = 252
            End If

            lbl_ExecutingName.Visible = True
            cbo_marketting_Exec_Name.Visible = True

        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1417" Then
            If Trim(UCase(vLedType)) = "GODOWN" Then
                lbl_ledger_ShortName.Visible = True
                txt_Ledger_ShortName.Visible = True
                txt_Name.Width = 252
            End If

        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "-1438-" Then
            btn_Import_Master.Visible = True
        End If


        If Trim(UCase(vLedType)) = "WEAVER" And Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then '---- BRT SPINNERS PRIVATE LIMITED (FABRIC DIVISION)
            btn_PavuYarn_Stock_MinMax_Level.Visible = True
            chk_WeavingBill_IR_Receipt_Mtrs.Visible = True
        End If

        txt_TamilName.Visible = False
        lbl_tamilname.Visible = False
        Try
            txt_TamilName.Font = New Font("SaiIndira", 10)
            'txt_TamilName.Font = New Font("Baamini", 10)
            'txt_TamilName.Font = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
        Catch ex As Exception
            '---
        End Try
        If Trim(UCase(vLedType)) = "WEAVER" Then
            txt_TamilName.Visible = True
            lbl_tamilname.Visible = True
            btn_Tamil_Address.Visible = True
            btn_Print_Address_2.Visible = True
            'btn_bank_Details.Visible = True

        End If
        If Trim(UCase(vLedType)) = "TRANSPORT" Then
            txt_vehicleNo.Visible = True
            lbl_vehicleNo_caption.Visible = True
        Else
            Txt_Remarks.Width = txt_Address1.Width
        End If

        If Trim(UCase(vLedType)) = "" And (Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1116" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1380" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1357") Then

            lbl_cl_amnt.Visible = True
            lbl_cl_amnt.BackColor = grp_Back.BackColor
            txt_CreditLimit.Visible = True
            txt_CreditLimit.BackColor = Color.White
            txt_CreditLimit.Width = txt_GSTIN_No.Width

            lbl_CreditLimitDays_Caption.Visible = True
            lbl_CreditLimitDays_Caption.BackColor = grp_Back.BackColor
            txt_creditLimitDays.Visible = True
            txt_creditLimitDays.BackColor = Color.White
            txt_creditLimitDays.Width = txt_AadharNo.Width
            txt_creditLimitDays.Left = txt_AadharNo.Left
            txt_creditLimitDays.Left = txt_creditLimitDays.Left + 50
            txt_creditLimitDays.Width = txt_creditLimitDays.Width - 50

        Else

            lbl_cl_amnt.Visible = False
            lbl_CreditLimitDays_Caption.Visible = False
            txt_CreditLimit.Visible = False
            txt_creditLimitDays.Visible = False

        End If


        lbl_Freight_Per_Loom_Caption.Visible = False
        txt_Freight_per_Loom.Visible = False

        Own_Loom_Status.Visible = False

        lbl_NoofLoom.Visible = False
        txt_NoofLoom.Visible = False

        lbl_TdsPerc_Caption.Visible = False
        txt_TdsPerc.Visible = False

        cbo_BillType.Enabled = False
        chk_Stock_Maintenance.Visible = False
        btn_Loom_Details.Visible = False

        lbl_Weaver_LoomType_Caption.Visible = False
        cbo_Weaver_LoomType.Visible = False

        cbo_State.Width = txt_Address4.Width

        cbo_Sizing_To_CompanyName.Visible = False
        lbl_Sizing_To_CompanyName_Caption.Visible = False


        cbo_Sizing_To_VendorName.Visible = False


        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1118" Then '----Kasturi laxmi textiles
            lbl_InsuranceNo_Caption.Visible = True
            lbl_InsuranceNo_Caption.BackColor = grp_Back.BackColor
            txt_InsuranceNo.Visible = True
            txt_InsuranceNo.BackColor = Color.White
        End If

        If Trim(UCase(vLedType)) = "TRANSPORT" Then
            Me.BackColor = Color.LightCyan   'Color.LightSteelBlue   'Color.LightSeaGreen
            lbl_Heading.Text = "TRANSPORT CREATION"
            Me.Text = "TRANSPORT CREATION"
            chk_Stock_Maintenance.Visible = False

        ElseIf Trim(UCase(vLedType)) = "GODOWN" Then

            Me.BackColor = Color.PapayaWhip
            lbl_Heading.Text = "GODOWN CREATION"
            Me.Text = "GODOWN CREATION"
            cbo_AcGroup.Text = "STOCK-IN-HAND"
            cbo_AcGroup.Enabled = False
            cbo_BillType.Enabled = False

        ElseIf Trim(UCase(vLedType)) = "SIZING" Then
            Me.BackColor = Color.LightSkyBlue
            lbl_Heading.Text = "SIZING CREATION"
            Me.Text = "SIZING CREATION"


            lbl_TdsPerc_Caption.Visible = True
            txt_TdsPerc.Visible = True
            txt_TdsPerc.BackColor = Color.White
            txt_TdsPerc.Width = txt_GSTIN_No.Width

            chk_Stock_Maintenance.Visible = False
            cbo_BillType.Enabled = True

            If Common_Procedures.settings.Combine_Textile_SizingSOftware = 1 Then
                SizTo_DbName = Common_Procedures.get_Company_SizingDataBaseName(Trim(Val(Common_Procedures.CompGroupIdNo)))
                cbo_Sizing_To_CompanyName.Visible = True
                cbo_Sizing_To_CompanyName.BackColor = Color.White
                cbo_Sizing_To_CompanyName.Width = txt_AadharNo.Width

                lbl_Sizing_To_CompanyName_Caption.Visible = True
                lbl_Sizing_To_CompanyName_Caption.Text = "Sizing Company Name"
                lbl_Sizing_To_CompanyName_Caption.BackColor = grp_Back.BackColor

            Else
                SizTo_DbName = Common_Procedures.get_Company_DataBaseName(Trim(Val(Common_Procedures.CompGroupIdNo)))

            End If

        ElseIf Trim(UCase(vLedType)) = "WEAVER" Then

            Me.BackColor = Color.PapayaWhip
            lbl_Heading.Text = "WEAVER CREATION"
            Me.Text = "WEAVER CREATION"


            Own_Loom_Status.Visible = True

            lbl_Freight_Per_Loom_Caption.Visible = True
            txt_Freight_per_Loom.Visible = True

            lbl_NoofLoom.Visible = True
            txt_NoofLoom.Visible = True

            lbl_TdsPerc_Caption.Visible = True
            txt_TdsPerc.Visible = True
            txt_TdsPerc.BackColor = Color.White
            txt_TdsPerc.Width = txt_GSTIN_No.Width
            ' txt_Production_per_Day.Visible = True

            txt_InsuranceNo.Visible = False

            btn_Loom_Details.Visible = True
            btn_PanAddress.Visible = True
            btn_Freight_Charge_Details.Visible = True

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1242" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then
                btn_Weaver_Wages_Details.Visible = True
            End If
            chk_Stock_Maintenance.Visible = False

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then '---- BRT SIZING (SOMANUR)
                lbl_Weaver_LoomType_Caption.Visible = True
                cbo_Weaver_LoomType.Visible = True
                'lbl_Production_per_day.Visible = True
                'txt_Production_per_Day.Visible = True
                cbo_State.Width = txt_PhoneNo.Width



            End If

            If Common_Procedures.settings.Combine_Textile_SizingSOftware = 1 Then

                SizTo_DbName = Common_Procedures.get_Company_SizingDataBaseName(Trim(Val(Common_Procedures.CompGroupIdNo)))

                cbo_Sizing_To_VendorName.Visible = True
                cbo_Sizing_To_VendorName.BackColor = Color.White
                cbo_Sizing_To_VendorName.Width = txt_AadharNo.Width

                lbl_Sizing_To_CompanyName_Caption.Visible = True
                lbl_Sizing_To_CompanyName_Caption.Text = "Sizing Vendor Name"
                lbl_Sizing_To_CompanyName_Caption.BackColor = grp_Back.BackColor

            Else

                SizTo_DbName = Common_Procedures.get_Company_DataBaseName(Trim(Val(Common_Procedures.CompGroupIdNo)))

            End If

            btn_Print_Address_2.Visible = True

        ElseIf Trim(UCase(vLedType)) = "JOBWORKER" Then
            Me.BackColor = Color.Khaki  ' Color.LightGreen
            lbl_Heading.Text = "JOBWORKER CREATION"
            Me.Text = "JOBWORKER CREATION"
            chk_Stock_Maintenance.Visible = False
            cbo_BillType.Enabled = True

        ElseIf Trim(UCase(vLedType)) = "REWINDING" Then
            Me.BackColor = Color.AliceBlue
            lbl_Heading.Text = "REWINDING CREATION"
            Me.Text = "REWINDING CREATION"
            chk_Stock_Maintenance.Visible = False
            cbo_BillType.Enabled = True

        ElseIf Trim(UCase(vLedType)) = "SPINNING" Then
            Me.BackColor = Color.LightGray
            lbl_Heading.Text = "SPINNING CREATION"
            Me.Text = "SPINNING CREATION"
            chk_Stock_Maintenance.Visible = False

        ElseIf Trim(UCase(vLedType)) = "SALESPARTY" Then
            Me.BackColor = Color.LightSalmon
            lbl_Heading.Text = "SALESPARTY CREATION"
            Me.Text = "SALESPARTY CREATION"
            chk_Stock_Maintenance.Visible = False
            cbo_BillType.Enabled = True

        ElseIf Trim(UCase(vLedType)) = "SEWING" Then
            Me.BackColor = Color.LightCoral
            lbl_Heading.Text = "SEWING CREATION"
            Me.Text = "SEWING CREATION"
            chk_Stock_Maintenance.Visible = False

        ElseIf Trim(UCase(vLedType)) = "FIREWOOD" Then
            Me.BackColor = Color.LightPink
            lbl_Heading.Text = "FIREWOOD PARTY CREATION"
            Me.Text = "FIREWOOD PARTY CREATION"
        Else

            Me.BackColor = Color.LightBlue
            lbl_Heading.Text = "LEDGER CREATION"
            Me.Text = "LEDGER CREATION"
            chk_Stock_Maintenance.Visible = False
            cbo_BillType.Enabled = True




        End If


        If Trim(Common_Procedures.settings.CustomerCode) = "1530" Then
            'txt_TdsPerc.Visible = True
            'lbl_TdsPerc_Caption.Visible = True

            lbl_TdsPerc_Caption.Visible = True
            txt_TdsPerc.Visible = True
            txt_TdsPerc.BackColor = Color.White
            txt_TdsPerc.Width = txt_GSTIN_No.Width

        End If

        If Trim(UCase(vLedType)) = "" And Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1155" Then
            lbl_LedgerGroup_Caption.Visible = True
            cbo_LedgerGroup.Visible = True

        Else
            txt_LegalName_Business.Width = txt_Name.Width
            lbl_LedgerGroup_Caption.Visible = False
            cbo_LedgerGroup.Visible = False

        End If

        dgv_Contact_Person_Details.Visible = False
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1413" Then

            txt_contact_person_name.Visible = True
            lbl_contact_person_name.Visible = True
            lbl_designation.Visible = True
            cbo_designation.Visible = True
            lbl_party_category.Visible = True
            cbo_party_category.Visible = True

            Label8.Visible = False
            txt_MobileSms.Visible = False
            txt_Mail.Visible = False
            Label12.Visible = False
            Label13.Visible = False
            txt_PanNo.Visible = False
            Label50.Visible = False
            txt_Distance.Visible = False
            'Label22.Visible = False
            'txt_GSTIN_No.Visible = False
            Label24.Visible = False
            txt_AadharNo.Visible = False
            Label14.Visible = False
            txt_OwnerName.Visible = False
            lbl_contact_person_name.Visible = False
            txt_contact_person_name.Visible = False
            lbl_designation.Visible = False
            cbo_designation.Visible = False
            Label4.Visible = False
            cbo_BillType.Visible = False

            lbl_party_category.Location = New Point(300, 125)
            cbo_party_category.Location = New Point(381, 121)
            cbo_party_category.Size = New Size(200, 23)
            Txt_Remarks.Visible = False
            Label52.Visible = False
            Chk_Tcs_for_Sales_sts.Visible = False
            Close_Status.Visible = False
            chk_Stock_Maintenance.Visible = False
            Show_In_All_Entry.Visible = False
            Label9.Location = New Point(300, 260)
            txt_PhoneNo.Location = New Point(382, 255)
            txt_PhoneNo.Size = New Size(200, 25)
            chk_GSTIN_Verified.Visible = False
            btn_Verify.Visible = False
            btn_Extract.Visible = False
            lbl_PanNo_From_GSTNo.Visible = False

            Chk_Sales_Tds_Deduction_sts.Visible = False
            Chk_Is_TurnOver_Above_10_Crore_Sts.Visible = False

            dgv_Contact_Person_Details.Visible = True
            dgv_Contact_Person_Details.Left = 7
            dgv_Contact_Person_Details.Top = 310

            Chk_Tcs_for_Purchase_sts.Visible = False
            Chk_Purchase_Tds_Deduction_sts.Visible = False

        End If



        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1105" Then '---- Ganga Weaving (Dindugal)
            cbo_BillType.Enabled = True
        End If

        If Trim(UCase(vLedType)) = "WEAVER" And (Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1033" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1204") Then
            lbl_Weaver_LoomType_Caption.Visible = True
            cbo_Weaver_LoomType.Visible = True
            cbo_State.Width = txt_PhoneNo.Width
        End If

        If Trim(UCase(vLedType)) = "WEAVER" And (Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267") Then
            lbl_IdNo.Width = (Me.Width - lbl_IdNo.Width) - 50   '  450
            lbl_Weaver_LoomType_Caption.Visible = True
            cbo_Weaver_LoomType.Visible = True
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1417" Then
            lbl_LedgerGroup_Caption.Visible = True
            cbo_LedgerGroup.Visible = True
            txt_LegalName_Business.Width = 173
        End If

        cbo_BillType.Items.Clear()
        cbo_BillType.Items.Add("BALANCE ONLY")
        cbo_BillType.Items.Add("BILL TO BILL")

        cbo_Weaver_LoomType.Items.Clear()
        cbo_Weaver_LoomType.Items.Add("")
        cbo_Weaver_LoomType.Items.Add("POWERLOOM")
        cbo_Weaver_LoomType.Items.Add("AUTOLOOM")

        cbo_PaperOrientation.Items.Clear()
        cbo_PaperOrientation.Items.Add("PORTRAIT")
        cbo_PaperOrientation.Items.Add("LANDSCAPE")

        dgv_Wages_Charge_Details.Columns(2).HeaderText = Trim(UCase(Common_Procedures.ClothType.Type1))
        dgv_Wages_Charge_Details.Columns(3).HeaderText = Trim(UCase(Common_Procedures.ClothType.Type2))
        dgv_Wages_Charge_Details.Columns(4).HeaderText = Trim(UCase(Common_Procedures.ClothType.Type3))
        dgv_Wages_Charge_Details.Columns(5).HeaderText = Trim(UCase(Common_Procedures.ClothType.Type4))
        dgv_Wages_Charge_Details.Columns(6).HeaderText = Trim(UCase(Common_Procedures.ClothType.Type5))


        grp_Open.Visible = False
        grp_Open.Left = (Me.Width - grp_Open.Width) - 100
        grp_Open.Top = (Me.Height - grp_Open.Height) - 100

        grp_Filter.Visible = False
        grp_Filter.Left = (Me.Width - grp_Filter.Width) - 100
        grp_Filter.Top = (Me.Height - grp_Filter.Height) - 100


        pnl_BobinReelDetails.Visible = False
        pnl_BobinReelDetails.Left = (Me.Width - pnl_BobinReelDetails.Width) \ 2
        pnl_BobinReelDetails.Top = (Me.Height - pnl_BobinReelDetails.Height) \ 2

        pnl_Loom_Details.Visible = False
        pnl_Loom_Details.Left = (Me.Width - pnl_Loom_Details.Width) \ 2
        pnl_Loom_Details.Top = (Me.Height - pnl_Loom_Details.Height) \ 2
        pnl_Loom_Details.BringToFront()

        pnl_tamil_Address.Visible = False
        pnl_tamil_Address.Left = (Me.Width - pnl_tamil_Address.Width) \ 2
        pnl_tamil_Address.Top = (Me.Height - pnl_tamil_Address.Height) \ 2
        pnl_tamil_Address.BringToFront()

        pnl_PYStockLimit.Visible = False
        pnl_PYStockLimit.Top = (Me.Height - pnl_PYStockLimit.Height) \ 2
        pnl_PYStockLimit.Left = (Me.Width - pnl_PYStockLimit.Width) \ 2
        pnl_PYStockLimit.BringToFront()

        pnl_PanAddress.Visible = False
        pnl_PanAddress.Left = (Me.Width - pnl_PanAddress.Width) \ 2
        pnl_PanAddress.Top = (Me.Height - pnl_PanAddress.Height) \ 2
        pnl_PanAddress.BringToFront()

        pnl_Freight_Charge_Details.Visible = False
        pnl_Freight_Charge_Details.Left = (Me.Width - pnl_Freight_Charge_Details.Width) \ 2
        pnl_Freight_Charge_Details.Top = (Me.Height - pnl_Freight_Charge_Details.Height) \ 2

        pnl_Wages_Charge_Details.Visible = False
        pnl_Wages_Charge_Details.Left = (Me.Width - pnl_Wages_Charge_Details.Width) \ 2
        pnl_Wages_Charge_Details.Top = (Me.Height - pnl_Wages_Charge_Details.Height) \ 2

        pnl_PrintSetup.Visible = False
        pnl_PrintSetup.Left = (Me.Width - pnl_PrintSetup.Width) \ 2
        pnl_PrintSetup.Top = (Me.Height - pnl_PrintSetup.Height) \ 2
        pnl_PrintSetup.BringToFront()

        pnl_bank_Details.Visible = False
        pnl_bank_Details.Left = (Me.Width - pnl_bank_Details.Width) \ 2
        pnl_bank_Details.Top = (Me.Height - pnl_bank_Details.Height) \ 2
        pnl_bank_Details.BringToFront()

        If chk_FromAddress.Checked = True Then
            txt_LeftFromAdds.Enabled = True
            txt_TopFromAdds.Enabled = True
        End If


        AddHandler txt_Name.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_AlaisName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Area.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Sizing_To_CompanyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Sizing_To_VendorName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_AcGroup.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_BillType.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Address1.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Address2.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Address3.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Address4.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PanAddress1.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PanAddress2.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PanAddress3.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PanAddress4.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Open.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Transfer_StockTo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_NoofLoom.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Freight_per_Loom.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_OwnerName.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PanNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_WeightPrReel.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_AadharNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Freight_Pavu.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_freight_Bundle.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_cloth_name.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_LedgerGroup.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_MeterPrReel.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_Colour.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_CreditLimit.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_creditLimitDays.GotFocus, AddressOf ControlGotFocus
        AddHandler Txt_TamilAddress_1.GotFocus, AddressOf ControlGotFocus
        AddHandler Txt_TamilAddress_2.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Production_per_Day.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_contact_person_name.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_designation.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_party_category.GotFocus, AddressOf ControlGotFocus
        AddHandler Cbo_Gird_desigantion.Enter, AddressOf ControlGotFocus

        AddHandler txt_InsuranceNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Mail.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PhoneNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_TinNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_CstNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_MobileSms.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_TdsPerc.GotFocus, AddressOf ControlGotFocus
        AddHandler btnSave.GotFocus, AddressOf ControlGotFocus
        AddHandler btnClose.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Open.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_TamilName.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_Bank_Acc_Name.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_bankName.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Branch.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_AccountNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Ifsc_Code.GotFocus, AddressOf ControlGotFocus


        AddHandler txt_GSTIN_No.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_State.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_YarnMin.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_YarnMax.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PavuMin.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PavuMax.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_TopFromAdds.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_TOPToAdds.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_LeftFromAdds.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_LeftToAdds.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PaperOrientation.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_FromAddress.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Weaver_LoomType.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_LegalName_Business.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_city.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_pincode.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Distance.GotFocus, AddressOf ControlGotFocus
        AddHandler chk_GSTIN_Verified.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_vehicleNo.GotFocus, AddressOf ControlGotFocus
        AddHandler chk_WeavingBill_IR_Receipt_Mtrs.GotFocus, AddressOf ControlGotFocus
        AddHandler Txt_Remarks.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Ledger_ShortName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_marketting_Exec_Name.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_Bank_Acc_Name.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_bankName.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Branch.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_AccountNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Ifsc_Code.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_Production_per_Day.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_LedgerGroup.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Name.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_AlaisName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Area.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Sizing_To_CompanyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Sizing_To_VendorName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_AcGroup.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_BillType.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Transfer_StockTo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Mail.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Address1.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Address2.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Address3.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Address4.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PanAddress1.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PanAddress2.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PanAddress3.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PanAddress4.LostFocus, AddressOf ControlLostFocus
        AddHandler Txt_TamilAddress_1.LostFocus, AddressOf ControlLostFocus
        AddHandler Txt_TamilAddress_2.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_contact_person_name.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_designation.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_party_category.LostFocus, AddressOf ControlLostFocus
        AddHandler Cbo_Gird_desigantion.Leave, AddressOf ControlLostFocus

        AddHandler txt_PhoneNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_TinNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_MobileSms.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_CstNo.LostFocus, AddressOf ControlLostFocus
        AddHandler btnSave.LostFocus, AddressOf ControlLostFocus
        AddHandler btnClose.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_NoofLoom.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Freight_per_Loom.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_OwnerName.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PanNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_TdsPerc.LostFocus, AddressOf ControlLostFocus
        'AddHandler txt_TamilName.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Freight_Pavu.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_freight_Bundle.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_AadharNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_cloth_name.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_GSTIN_No.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_State.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_Colour.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_WeightPrReel.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_MeterPrReel.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_YarnMin.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_YarnMax.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PavuMin.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PavuMax.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_CreditLimit.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_creditLimitDays.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_TopFromAdds.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_TOPToAdds.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_LeftFromAdds.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_LeftToAdds.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_PaperOrientation.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_FromAddress.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Weaver_LoomType.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_TamilName.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_InsuranceNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_LegalName_Business.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_TamilName.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_city.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_pincode.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Distance.LostFocus, AddressOf ControlLostFocus
        AddHandler chk_GSTIN_Verified.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_vehicleNo.LostFocus, AddressOf ControlLostFocus
        AddHandler chk_WeavingBill_IR_Receipt_Mtrs.LostFocus, AddressOf ControlLostFocus
        AddHandler Txt_Remarks.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Ledger_ShortName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_marketting_Exec_Name.LostFocus, AddressOf ControlLostFocus

        ' AddHandler txt_Name.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_AlaisName.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Address1.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Address2.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Address3.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Address4.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_PanAddress2.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_PanAddress3.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler txt_PhoneNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_TinNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_MobileSms.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Mail.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_CstNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_PanNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Freight_per_Loom.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler txt_GSTIN_No.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_TamilName.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_AadharNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_MeterPrReel.KeyDown, AddressOf TextBoxControlKeyDown
        '  AddHandler txt_InsuranceNo.KeyDown, AddressOf TextBoxControlKeyDown

        'AddHandler txt_OwnerName.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_TOPToAdds.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_LeftFromAdds.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_LeftToAdds.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler txt_vehicleNo.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler txt_LegalName_Business.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_city.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_TdsPerc.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_pincode.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Distance.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler chk_GSTIN_Verified.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler chk_WeavingBill_IR_Receipt_Mtrs.KeyDown, AddressOf TextBoxControlKeyDown


        'AddHandler txt_Name.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_AlaisName.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Address1.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Address2.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Address3.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_PanAddress1.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_PanAddress2.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_PanAddress3.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler Txt_TamilAddress_1.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Mail.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Address4.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_PhoneNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_TinNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_MobileSms.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_CstNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler Txt_TamilAddress_2.KeyPress, AddressOf TextBoxControlKeyPress
        ' AddHandler txt_FrgtLoom.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_GSTIN_No.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_TamilName.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_AadharNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_MeterPrReel.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_TdsPerc.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_TopFromAdds.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_TOPToAdds.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_LeftFromAdds.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_LeftToAdds.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_TamilName.KeyPress, AddressOf TextBoxControlKeyPress
        '  AddHandler txt_InsuranceNo.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_vehicleNo.KeyPress, AddressOf TextBoxControlKeyPress
        '  AddHandler txt_LegalName_Business.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_city.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_pincode.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Distance.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler chk_GSTIN_Verified.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler chk_WeavingBill_IR_Receipt_Mtrs.KeyPress, AddressOf TextBoxControlKeyPress


        AddHandler cbo_Grid_ClothName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_ClothName.LostFocus, AddressOf ControlLostFocus

        FrmLdSTS = True
        new_record()

    End Sub

    Private Sub Ledger_Creation_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.MDI_LedType = Me.Name
    End Sub

    Private Sub Ledger_Creation_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If Asc(e.KeyChar) = 27 Then
            If grp_Open.Visible Then
                btn_CloseOpen_Click(sender, e)
            ElseIf grp_Filter.Visible Then
                btn_CloseFilter_Click(sender, e)
            ElseIf pnl_BobinReelDetails.Visible Then
                btn_BobinReel_Close_Click(sender, e)
            ElseIf pnl_Loom_Details.Visible Then
                btn_Close_LoomDetails_Click(sender, e)
            ElseIf pnl_Freight_Charge_Details.Visible Then
                btn_Close_Freight_Charge_Details_Click(sender, e)
            ElseIf pnl_PanAddress.Visible Then
                btn_Close_PanAddress_Details_Click(sender, e)
            ElseIf pnl_Wages_Charge_Details.Visible Then
                btn_Close_Weaver_Wages_Charge_Click(sender, e)
            ElseIf btn_PYStockLmt_Close.Visible Then
                btn_PYStockLmt_Close_Click(sender, e)
            ElseIf pnl_PrintSetup.Visible Then

                btn_PrintClose_Click(sender, e)
            ElseIf pnl_tamil_Address.Visible Then

                btn_pnl_tamil_Address_Click(sender, e)

            ElseIf pnl_bank_Details.Visible Then

                btn_close_bankdetails_Click_1(sender, e)

            Else

                If MessageBox.Show("Do you want to Close?", "FOR CLOSING ENTRY...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) <> Windows.Forms.DialogResult.Yes Then
                    Exit Sub
                Else
                    Me.Close()
                End If

            End If

        End If
    End Sub

    Private Sub btn_CloseOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_CloseOpen.Click
        grp_Back.Enabled = True
        grp_Open.Visible = False
        If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()
    End Sub

    Private Sub btn_Find_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Find.Click
        Dim movid As Integer

        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()

        movid = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Open.Text)

        If movid <> 0 Then move_record(movid)

        grp_Back.Enabled = True
        grp_Open.Visible = False

    End Sub

    Private Sub cbo_Open_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Open.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Open, Nothing, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = '" & Trim(vLedType) & "')", "(ledger_idno = 0)")
    End Sub

    Private Sub cbo_Open_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Open.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Open, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = '" & Trim(vLedType) & "')", "(ledger_idno = 0)")

        If Asc(e.KeyChar) = 13 Then
            Call btn_Find_Click(sender, e)
        End If

    End Sub

    Private Sub btn_CloseFilter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_CloseFilter.Click
        grp_Filter.Visible = False
    End Sub

    Private Sub btn_Filter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter.Click
        Dim idno As Integer

        If IsNothing(dgv_Filter.CurrentCell) Then Exit Sub

        idno = Val(dgv_Filter.CurrentRow.Cells(0).Value)

        If Val(idno) <> 0 Then
            move_record(idno)
            grp_Back.Enabled = True
            grp_Filter.Visible = False
        End If

    End Sub

    Private Sub dgv_Filter_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Filter.KeyDown
        If e.KeyValue = 13 Then
            Call btn_Filter_Click(sender, e)
        End If
    End Sub

    Private Sub txt_Name_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Name.KeyPress
        If Asc(e.KeyChar) = 34 Or Asc(e.KeyChar) = 39 Then   '-- Single Quotes and double quotes blocked
            e.Handled = True
        End If
        If Asc(e.KeyChar) = 13 Then
            If txt_Ledger_ShortName.Visible Then
                txt_Ledger_ShortName.Focus()
            Else
                txt_LegalName_Business.Focus()
            End If
        End If

    End Sub

    Private Sub cbo_AcGroup_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_AcGroup.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "AccountsGroup_Head", "AccountsGroup_Name", "(AccountsGroup_IdNo <> 30)", "(AccountsGroup_IdNo = 0)")
    End Sub

    Private Sub cbo_AcGroup_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_AcGroup.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_AcGroup, cbo_Area, cbo_LedgerGroup, "AccountsGroup_Head", "AccountsGroup_Name", "(AccountsGroup_IdNo <> 30)", "(AccountsGroup_IdNo = 0)")

        If (e.KeyValue = 40 And cbo_AcGroup.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            'If cbo_LedgerGroup.Visible And cbo_LedgerGroup.Enabled Then
            '    cbo_LedgerGroup.Focus()
            If cbo_party_category.Enabled And cbo_party_category.Visible Then
                cbo_party_category.Focus()
            ElseIf cbo_BillType.Enabled And cbo_BillType.Visible Then
                cbo_BillType.Focus()

            Else

                txt_Address1.Focus()
            End If
        End If

    End Sub

    Private Sub cbo_AcGroup_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_AcGroup.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_AcGroup, Nothing, "AccountsGroup_Head", "AccountsGroup_Name", "(AccountsGroup_IdNo <> 30)", "(AccountsGroup_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then
            'If cbo_LedgerGroup.Visible And cbo_LedgerGroup.Enabled Then
            '    cbo_LedgerGroup.Focus()
            If cbo_party_category.Enabled And cbo_party_category.Visible Then
                cbo_party_category.Focus()
            ElseIf cbo_BillType.Enabled And cbo_BillType.Visible Then
                cbo_BillType.Focus()

            Else
                txt_Address1.Focus()
            End If
        End If

    End Sub

    Private Sub cbo_BillType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_BillType.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_BillType, cbo_AcGroup, txt_Address1, "", "", "", "")
    End Sub

    Private Sub cbo_BillType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_BillType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_BillType, txt_Address1, "", "", "", "")
    End Sub

    Private Sub txt_AlaisName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_AlaisName.KeyPress
        If Asc(e.KeyChar) = 34 Or Asc(e.KeyChar) = 39 Then
            e.Handled = True
        End If
    End Sub
    Private Sub cbo_Area_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Area.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "area_head", "area_name", "", "(area_idno = 0)")

    End Sub
    Private Sub cbo_Area_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Area.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Area, txt_AlaisName, cbo_AcGroup, "area_head", "area_name", "", "(area_idno = 0)")
    End Sub

    Private Sub cbo_Area_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Area.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Area, cbo_AcGroup, "area_head", "area_name", "", "(area_idno = 0)")
    End Sub

    Private Sub cbo_Area_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Area.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            Dim f As New Area_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Area.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If

    End Sub

    Private Sub txt_NoofLoom_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_NoofLoom.KeyDown

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1116" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1380" Then
            If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
            If e.KeyValue = 40 Then
                txt_OwnerName.Focus()
            End If
        ElseIf Trim(UCase(vLedType)) = "WEAVER" Then
            If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
            If e.KeyValue = 40 Then SendKeys.Send("{TAB}")
        Else
            If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
            If e.KeyValue = 40 Then SendKeys.Send("{TAB}")
        End If


    End Sub

    Private Sub txt_NoofLoom_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_NoofLoom.KeyPress

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1116" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1380" Then

            If Asc(e.KeyChar) = 13 Then
                txt_CreditLimit.Focus()
            End If
        ElseIf Trim(UCase(vLedType)) = "WEAVER" Then
            If Asc(e.KeyChar) = 13 Then
                txt_Freight_per_Loom.Focus()
            End If
        Else
            If Asc(e.KeyChar) = 13 Then
                save_record()

            End If
        End If

    End Sub

    'Private Sub txt_FrgtLoom_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_FrgtLoom.KeyDown
    '    If Trim(UCase(vLedType)) = "WEAVER" Then
    '        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
    '    End If
    '    If Trim(UCase(vLedType)) = "WEAVER" Then
    '        If e.KeyValue = 40 Then
    '            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
    '                save_record()
    '            Else
    '                txt_Name.Focus()
    '            End If
    '        End If
    '    End If
    'End Sub

    'Private Sub txt_FrgtLoom_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_FrgtLoom.KeyPress
    '    If Trim(UCase(vLedType)) = "WEAVER" Then
    '        If Asc(e.KeyChar) = 13 Then
    '            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
    '                save_record()
    '            Else
    '                txt_Name.Focus()
    '            End If
    '        End If
    '    End If
    'End Sub

    Private Sub txt_tds_perc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_TdsPerc.KeyDown
        If Trim(UCase(vLedType)) = "WEAVER" Or Trim(UCase(vLedType)) = "SIZING" Then
            If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        End If
        If Trim(UCase(vLedType)) = "WEAVER" Or Trim(UCase(vLedType)) = "SIZING" Then
            If e.KeyValue = 40 Then
                If cbo_Sizing_To_CompanyName.Visible = True Then
                    cbo_Sizing_To_CompanyName.Focus()
                ElseIf cbo_Sizing_To_VendorName.Visible = True Then
                    cbo_Sizing_To_VendorName.Focus()
                ElseIf Txt_Remarks.Visible = True And Txt_Remarks.Enabled = True Then
                    Txt_Remarks.Focus()
                ElseIf txt_Production_per_Day.Visible = True Then
                    txt_Production_per_Day.Focus()

                Else
                    If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                        save_record()
                    Else
                        txt_Name.Focus()
                    End If
                End If
            End If
        End If


    End Sub

    Private Sub txt_TDS_perc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_TdsPerc.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
        If Trim(UCase(vLedType)) = "WEAVER" Or Trim(UCase(vLedType)) = "SIZING" Then
            If Asc(e.KeyChar) = 13 Then
                If cbo_Sizing_To_CompanyName.Visible = True Then
                    cbo_Sizing_To_CompanyName.Focus()
                ElseIf cbo_Sizing_To_VendorName.Visible = True Then
                    cbo_Sizing_To_VendorName.Focus()
                ElseIf Txt_Remarks.Visible = True And Txt_Remarks.Enabled = True Then
                    Txt_Remarks.Focus()
                ElseIf txt_Production_per_Day.Visible = True Then
                    txt_Production_per_Day.Focus()
                Else

                    If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                        save_record()
                    Else
                        txt_Name.Focus()
                    End If
                End If
            End If
        End If

    End Sub


    Private Sub cbo_TransferStock_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Transfer_StockTo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, TrnTo_DbName & "..Ledger_Head", "Ledger_Name", "", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_TransferStock_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Transfer_StockTo.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Transfer_StockTo, txt_OwnerName, Nothing, TrnTo_DbName & "..Ledger_Head", "Ledger_Name", "", "(Ledger_idno = 0)")
        If (e.KeyValue = 40 And cbo_Transfer_StockTo.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                txt_Name.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_TransferStock_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Transfer_StockTo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Transfer_StockTo, Nothing, TrnTo_DbName & "..Ledger_Head", "Ledger_Name", "", "(Ledger_idno = 0)")
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                txt_Name.Focus()
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


        LastNo = lbl_IdNo.Text

        movefirst_record()
        Timer1.Enabled = True

    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        save_record()
        If Trim(UCase(LastNo)) = Trim(UCase(lbl_IdNo.Text)) Then
            Timer1.Enabled = False
            SaveAll_STS = False
            MessageBox.Show("All entries saved Successfully", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Else
            movenext_record()

        End If
    End Sub

    Private Sub cbo_State_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_State.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "State_Head", "State_Name", "", "(State_Idno = 0)")

    End Sub

    Private Sub cbo_State_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_State.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_State, txt_pincode, Nothing, "State_Head", "State_Name", "", "(State_Idno = 0)")

        'If (e.KeyValue = 38 And cbo_Grid_Colour.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
        '    If .CurrentCell.RowIndex <= 0 Then
        '        txt_Name.Focus()
        '    Else
        '        .Focus()
        '        .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(2)
        '        .CurrentCell.Selected = True
        '    End If

        'End If

        If (e.KeyValue = 40 And cbo_Grid_Colour.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1413" Then
                txt_PhoneNo.Focus()

            Else
                txt_Distance.Focus()
            End If
        End If


    End Sub

    Private Sub cbo_State_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_State.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_State, Nothing, "State_Head", "State_Name", "", "(State_Idno = 0)")
        If Asc(e.KeyChar) = 13 Then
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1413" Then
                txt_PhoneNo.Focus()

            Else
                txt_Distance.Focus()
            End If
        End If
    End Sub

    Private Sub btn_BobinReelDetails_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_BobinReelDetails.Click
        pnl_BobinReelDetails.Visible = True
        pnl_BobinReelDetails.BringToFront()
        pnl_BobinReelDetails.Enabled = True
        'txt_RatePrReel.Focus()
        grp_Back.Enabled = False
    End Sub

    Private Sub btn_BobinReel_Close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_BobinReel_Close.Click
        pnl_BobinReelDetails.Visible = False
        grp_Back.Enabled = True
    End Sub
    Private Sub dgv_KnittingDetails_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_RateDetails.CellEndEdit
        dgv_KnittingDetails_CellLeave(sender, e)
    End Sub

    Private Sub dgv_KnittingDetails_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_RateDetails.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim Dt4 As New DataTable
        Dim Dt5 As New DataTable

        Dim Rect As Rectangle

        With dgv_RateDetails
            dgv_ActiveCtrl_Name = .Name

            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If

            If e.ColumnIndex = 1 Then


                If cbo_Grid_Colour.Visible = False Or Val(cbo_Grid_Colour.Tag) <> e.RowIndex Then

                    cbo_Grid_Colour.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Colour_Name from Colour_Head order by Colour_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Grid_Colour.DataSource = Dt1
                    cbo_Grid_Colour.DisplayMember = "Colour_Name"

                    Rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_Colour.Left = .Left + Rect.Left
                    cbo_Grid_Colour.Top = .Top + Rect.Top

                    cbo_Grid_Colour.Width = Rect.Width
                    cbo_Grid_Colour.Height = Rect.Height
                    cbo_Grid_Colour.Text = .CurrentCell.Value

                    cbo_Grid_Colour.Tag = Val(e.RowIndex)
                    cbo_Grid_Colour.Visible = True

                    cbo_Grid_Colour.BringToFront()
                    cbo_Grid_Colour.Focus()


                End If

            Else
                cbo_Grid_Colour.Visible = False

            End If

        End With

    End Sub

    Private Sub dgv_KnittingDetails_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_RateDetails.CellLeave
        With dgv_RateDetails
            If .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 6 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                Else
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
                End If
            End If
        End With
    End Sub

    Private Sub dgv_KnittingDetails_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_RateDetails.EditingControlShowing
        dgtxt_KnittingDetails = CType(dgv_RateDetails.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgtxt_KnittingDetails_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_KnittingDetails.Enter
        Try
            dgv_ActiveCtrl_Name = dgv_RateDetails.Name
            dgv_RateDetails.EditingControl.BackColor = Color.Lime
            dgv_RateDetails.EditingControl.ForeColor = Color.Blue
            dgv_RateDetails.SelectAll()
        Catch ex As Exception
            '--
        End Try
    End Sub

    Private Sub dgtxt_KnittingDetails_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_KnittingDetails.KeyDown
        Try
            With dgv_RateDetails
                vcbo_KeyDwnVal = e.KeyValue
                If .Visible Then
                    If e.KeyValue = Keys.Delete Then

                        'If .CurrentCell.ColumnIndex = 1 Or .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3 Then
                        '    If Trim(UCase(cbo_Type.Text)) = "DIRECT" Or Trim(UCase(cbo_Type.Text)) = "RECEIPT" Then
                        '        e.Handled = True
                        '        e.SuppressKeyPress = True
                        '    End If
                        'End If

                    End If
                End If
            End With

        Catch ex As Exception
            '--
        End Try

    End Sub

    Private Sub dgtxt_KnittingDetails_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_KnittingDetails.KeyPress
        Try
            With dgv_RateDetails
                If .Visible Then

                    If .CurrentCell.ColumnIndex = 2 Then

                        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                            e.Handled = True
                        End If

                    End If

                End If
            End With
        Catch ex As Exception
            '---
        End Try


    End Sub

    Private Sub dgtxt_KnittingDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_KnittingDetails.KeyUp
        Try
            If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
                dgv_KnittingDetails_KeyUp(sender, e)
            End If
        Catch ex As Exception
            '---
        End Try
    End Sub

    Private Sub dgv_KnittingDetails_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_RateDetails.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
    End Sub

    Private Sub dgv_KnittingDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_RateDetails.KeyUp
        Dim n As Integer

        Try
            If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
                With dgv_RateDetails

                    n = .CurrentRow.Index

                    If n = .Rows.Count - 1 Then
                        For i = 0 To .ColumnCount - 1
                            .Rows(n).Cells(i).Value = ""
                        Next

                    Else
                        .Rows.RemoveAt(n)

                    End If

                    ' Total_DyesChemicalCalculation()

                End With

            End If

        Catch ex As Exception
            '---
        End Try


    End Sub

    Private Sub dgv_KnittingDetails_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_RateDetails.LostFocus
        On Error Resume Next
        dgv_RateDetails.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_Knitting_Details_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_RateDetails.RowsAdded
        Dim n As Integer = 0
        Try
            If FrmLdSTS = True Then Exit Sub
            With dgv_RateDetails
                n = .RowCount
                .Rows(n - 1).Cells(0).Value = Val(n)
            End With
        Catch ex As Exception
            '---
        End Try

    End Sub
    Private Sub cbo_Grid_Colour_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Colour.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Colour_Head", "Colour_Name", "", "(Colour_IdNo = 0)")
    End Sub

    Private Sub cbo_Grid_Colour_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Colour.KeyDown

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_Colour, Nothing, Nothing, "Colour_Head", "Colour_Name", "", "(Colour_IdNo = 0)")

        With dgv_RateDetails

            If (e.KeyValue = 38 And cbo_Grid_Colour.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                If .CurrentCell.RowIndex <= 0 Then
                    txt_Name.Focus()
                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(2)
                    .CurrentCell.Selected = True
                End If

            End If

            If (e.KeyValue = 40 And cbo_Grid_Colour.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                If .CurrentCell.RowIndex = .Rows.Count - 1 And Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(1).Value) = "" Then
                    btn_BobinReel_Close_Click(sender, e)

                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                    .CurrentCell.Selected = True

                End If

            End If

        End With
    End Sub

    Private Sub cbo_Grid_Colour_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_Colour.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_Colour, Nothing, "Colour_Head", "Colour_Name", "", "(Colour_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            e.Handled = True

            With dgv_RateDetails
                If Trim(.Rows(.CurrentRow.Index).Cells(1).Value) = "" And .CurrentRow.Index = .Rows.Count - 1 Then
                    btn_BobinReel_Close_Click(sender, e)

                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

                End If
            End With

        End If
    End Sub

    Private Sub cbo_Grid_Colour_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Colour.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            Dim f As New Color_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_Colour.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_Grid_Colour_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Colour.TextChanged
        Try
            If cbo_Grid_Colour.Visible Then
                With dgv_RateDetails
                    If Val(cbo_Grid_Colour.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_Colour.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub
    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
        Dim dgv1 As New DataGridView

        'If keyData = Keys.Enter Then

        On Error Resume Next

        If ActiveControl.Name = dgv_RateDetails.Name Or ActiveControl.Name = dgv_Freight_Charge_Details.Name Or ActiveControl.Name = dgv_Wages_Charge_Details.Name Or ActiveControl.Name = dgv_Contact_Person_Details.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then


            dgv1 = Nothing


            If ActiveControl.Name = dgv_RateDetails.Name Then
                dgv1 = dgv_RateDetails

            ElseIf dgv_RateDetails.IsCurrentRowDirty = True Then
                dgv1 = dgv_RateDetails


            ElseIf pnl_BobinReelDetails.Visible = True Then
                dgv1 = dgv_RateDetails

            ElseIf dgv_ActiveCtrl_Name = dgv_RateDetails.Name Then
                dgv1 = dgv_RateDetails

            ElseIf ActiveControl.Name = dgv_Freight_Charge_Details.Name Then
                dgv1 = dgv_Freight_Charge_Details

            ElseIf ActiveControl.Name = dgv_Wages_Charge_Details.Name Then
                dgv1 = dgv_Wages_Charge_Details
            ElseIf dgv_Freight_Charge_Details.IsCurrentRowDirty = True Then
                dgv1 = dgv_Freight_Charge_Details

            ElseIf dgv_Wages_Charge_Details.IsCurrentRowDirty = True Then
                dgv1 = dgv_Wages_Charge_Details
            ElseIf pnl_Freight_Charge_Details.Visible = True Then
                dgv1 = dgv_Freight_Charge_Details

            ElseIf pnl_Wages_Charge_Details.Visible = True Then
                dgv1 = dgv_Wages_Charge_Details
            ElseIf dgv_ActiveCtrl_Name = dgv_Freight_Charge_Details.Name Then
                dgv1 = dgv_Freight_Charge_Details
            ElseIf dgv_ActiveCtrl_Name = dgv_Wages_Charge_Details.Name Then
                dgv1 = dgv_Wages_Charge_Details
            ElseIf ActiveControl.Name = dgv_Loom_Details.Name Then
                dgv1 = dgv_Loom_Details

            ElseIf dgv_Loom_Details.IsCurrentRowDirty = True Then
                dgv1 = dgv_Loom_Details

            ElseIf dgv_ActiveCtrl_Name = dgv_Loom_Details.Name Then
                dgv1 = dgv_Loom_Details
            ElseIf pnl_Loom_Details.Visible = True Then
                dgv1 = dgv_Loom_Details

            ElseIf ActiveControl.Name = dgv_Contact_Person_Details.Name Then
                dgv1 = dgv_Contact_Person_Details

            ElseIf dgv_Contact_Person_Details.IsCurrentRowDirty = True Then
                dgv1 = dgv_Contact_Person_Details

            ElseIf dgv_ActiveCtrl_Name = dgv_Contact_Person_Details.Name Then
                dgv1 = dgv_Contact_Person_Details

            End If

            With dgv1


                If dgv1.Name = dgv_RateDetails.Name Then

                    If keyData = Keys.Enter Or keyData = Keys.Down Then
                        If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                .Rows.Add()
                                'dgv_FabricDetails.Focus()
                                'dgv_FabricDetails.CurrentCell = dgv_FabricDetails.Rows(0).Cells(1)

                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                            End If

                        Else

                            If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And ((.CurrentCell.ColumnIndex <> 1 And Val(.CurrentRow.Cells(1).Value) = 0) Or (.CurrentCell.ColumnIndex = 1 And Val(dgtxt_KnittingDetails.Text) = 0)) Then
                                For i = 0 To .Columns.Count - 1
                                    .Rows(.CurrentCell.RowIndex).Cells(i).Value = ""
                                Next

                                'btn_Save.Focus()
                            Else
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                            End If

                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then

                        If .CurrentCell.ColumnIndex <= 1 Then
                            If .CurrentCell.RowIndex = 0 Then
                                txt_MeterPrReel.Focus()
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


                ElseIf dgv1.Name = dgv_Loom_Details.Name Then

                    If keyData = Keys.Enter Or keyData = Keys.Down Then
                        If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                Close_LoomSelection()

                            Else
                                .Focus()
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)


                            End If

                        Else

                            If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And ((.CurrentCell.ColumnIndex <> 1 And Val(.CurrentRow.Cells(1).Value) = 0) Or (.CurrentCell.ColumnIndex = 1 And Val(dgtxt_LoomDetails.Text) = 0)) Then
                                For i = 0 To .Columns.Count - 1
                                    .Rows(.CurrentCell.RowIndex).Cells(i).Value = ""
                                Next

                                Close_LoomSelection()
                            Else
                                .Focus()
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                            End If


                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then

                        If .CurrentCell.ColumnIndex <= 1 Then
                            If .CurrentCell.RowIndex = 0 Then
                                Close_LoomSelection()

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

                ElseIf dgv1.Name = dgv_Freight_Charge_Details.Name Then

                    If keyData = Keys.Enter Or keyData = Keys.Down Then

                        If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                pnl_Freight_Charge_Details.Visible = False
                                grp_Back.Enabled = True
                                txt_Name.Focus()
                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                            End If

                        Else

                            If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then

                                pnl_Freight_Charge_Details.Visible = False
                                grp_Back.Enabled = True
                                txt_Name.Focus()

                            Else
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                            End If

                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then
                        If .CurrentCell.ColumnIndex <= 1 Then
                            If .CurrentCell.RowIndex = 0 Then

                                txt_Freight_Pavu.Focus()

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(2)

                            End If

                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)

                        End If

                        Return True



                    End If





                ElseIf dgv1.Name = dgv_Contact_Person_Details.Name Then



                    If keyData = Keys.Enter Or keyData = Keys.Down Then
                        If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                'If dgv_Contact_Person_Details.RowCount > 0 Then
                                '    btnSave.Focus()
                                '    ' dgv_SoftwareDetails.CurrentCell = dgv_SoftwareDetails.Rows(0).Cells(1)
                                'Else
                                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.Yes Then
                                    save_record()
                                Else
                                    txt_Name.Focus()
                                End If
                                'End If


                            Else

                                .Focus()
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                            End If

                        ElseIf .CurrentCell.ColumnIndex = 1 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then
                                ' If Trim(.CurrentRow.Cells(1).Value) = "" Then
                                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.Yes Then
                                    save_record()

                                End If

                            Else
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                            End If

                        Else
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then
                        If .CurrentCell.ColumnIndex <= 1 Then
                            If .CurrentCell.RowIndex = 0 Then

                                txt_GSTIN_No.Focus()

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



                ElseIf dgv1.Name = dgv_Wages_Charge_Details.Name Then

                    If keyData = Keys.Enter Or keyData = Keys.Down Then

                        If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                pnl_Wages_Charge_Details.Visible = False
                                grp_Back.Enabled = True
                                txt_Name.Focus()
                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                            End If

                        Else

                            If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then

                                pnl_Wages_Charge_Details.Visible = False
                                grp_Back.Enabled = True
                                txt_Name.Focus()

                            Else
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                            End If

                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then
                        If .CurrentCell.ColumnIndex <= 1 Then
                            If .CurrentCell.RowIndex = 0 Then

                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(2)

                            End If

                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)

                        End If

                        Return True
                    Else
                        Return MyBase.ProcessCmdKey(msg, keyData)

                    End If

                End If


            End With
            'Return True

        Else

            Return MyBase.ProcessCmdKey(msg, keyData)
            'SendKeys.Send("{TAB}")

        End If





        'Else

        '    Return MyBase.ProcessCmdKey(msg, keyData)

        'End If


    End Function

    Private Sub txt_MeterPrReel_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_MeterPrReel.KeyDown
        On Error Resume Next
        'If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub txt_MeterPrReel_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_MeterPrReel.KeyPress
        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub txt_WeightPrReel_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_WeightPrReel.KeyDown
        On Error Resume Next
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then
            If dgv_RateDetails.Visible = True Then
                If dgv_RateDetails.RowCount >= 0 Then
                    dgv_RateDetails.Focus()
                    dgv_RateDetails.CurrentCell = dgv_RateDetails.Rows(0).Cells(1)
                End If
            End If
        End If
    End Sub

    Private Sub txt_WeightPrReel_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_WeightPrReel.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If dgv_RateDetails.Visible = True Then
                If dgv_RateDetails.RowCount >= 0 Then
                    dgv_RateDetails.Focus()
                    dgv_RateDetails.CurrentCell = dgv_RateDetails.Rows(0).Cells(1)
                End If
            End If
        End If
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        print_record()

    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        pnl_PrintSetup.Visible = True
        grp_Back.Enabled = False

        If cbo_FromAddress.Visible And cbo_FromAddress.Focus Then cbo_FromAddress.Focus()
    End Sub

    Public Sub Printing_LedgerAddress_Print()
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim ps As Printing.PaperSize

        Try

            da1 = New SqlClient.SqlDataAdapter("select * from Ledger_Head Where Ledger_IdNo <> 0 and Ledger_IdNo = " & Str(Val(lbl_IdNo.Text)), con)
            dt1 = New DataTable
            da1.Fill(dt1)
            If dt1.Rows.Count <= 0 Then
                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub
            End If
            dt1.Dispose()
            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try


        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If Trim(cbo_PaperOrientation.Text) = "LANDSCAPE" Then
                PrintDocument1.DefaultPageSettings.Landscape = True
                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                    PrintDocument1.DefaultPageSettings.PaperSize = ps
                    'e.PageSettings.PaperSize = ps
                    Exit For
                End If
            Else
                PrintDocument1.DefaultPageSettings.Landscape = False
                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                    PrintDocument1.DefaultPageSettings.PaperSize = ps
                    'e.PageSettings.PaperSize = ps
                    Exit For
                End If
            End If
        Next

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
                ppd.ClientSize = New Size(900, 800)

                ppd.ShowDialog()

            Catch ex As Exception
                MsgBox("The printing operation failed" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "DOES NOT SHOW PRINT PREVIEW...")

            End Try

        End If

    End Sub

    Private Sub PrintDocument1_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument1.BeginPrint
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim Cmd As New SqlClient.SqlDataAdapter
        Dim W1 As Single = 0


        prn_HdDt.Clear()
        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0
        prn_count = 0

        Try

            da1 = New SqlClient.SqlDataAdapter("Select  a.* ,a.Weaver_LoomType, b.Area_Name , c.AccountsGroup_Name as Ac_Group_Name, d.State_Name , Ch.* , d.State_Name from Ledger_Head a INNER JOIN Area_Head b ON a.Area_IdNo = b.Area_IdNo LEFT JOIN AccountsGroup_Head c ON a.AccountsGroup_IdNo = c.AccountsGroup_IdNo LEFT JOIN  State_Head d ON a.Ledger_State_IdNo = d.State_IdNo INNER JOIN Company_Head Ch ON a.Company_IdNo = Ch.Company_IdNo LEFT JOIN Company_Head St ON d.State_IdNo = Ch.Company_State_IdNo where a.Ledger_IdNo <> 0 and a.Ledger_IdNo = " & Str(Val(lbl_IdNo.Text)), con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count < 0 Then

                MessageBox.Show("This is New Entry", "FOR PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If
            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub PrintDocument1_PrintPage(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        If prn_HdDt.Rows.Count <= 0 Then Exit Sub

        If PrntFormat1_STS = True Then

            Printing_Format1(e)

        ElseIf PrntFormat2_STS = True Then

            Printing_Format2(e)

        End If

    End Sub

    Private Sub Printing_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim pFont As Font
        Dim p1Font As Font
        Dim ps As Printing.PaperSize
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim I As Integer
        Dim TxtHgt As Single
        Dim CurY As Single = 0
        Dim ItmNm1 As String = "", ItmNm2 As String = ""
        Dim S1 As Single = 0
        Dim S2 As Single = 0
        Dim S3 As Single = 0
        Dim PhNo1 As String = ""
        Dim PhNo2 As String = ""
        Dim PhNo3 As String = ""
        Dim vLftMrgn_INCM As Single = 0
        Dim vTpMrgn_INCM As Single = 0
        Dim vLftMrgn_INPixel As Single = 0
        Dim vTpMrgn_INPixel As Single = 0

        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_PanNo As String, Cmp_GSTIN_No As String
        Dim Cmp_StateNm As String, Cmp_Mail As String

        vLftMrgn_INCM = 0
        vTpMrgn_INCM = 0

        If Trim(cbo_PaperOrientation.Text) = "LANDSCAPE" Then
            PrintDocument1.DefaultPageSettings.Landscape = True

            vLftMrgn_INCM = Val(txt_LeftToAdds.Text)
            vTpMrgn_INCM = Val(txt_TOPToAdds.Text)
            If vLftMrgn_INCM = 0 Then vLftMrgn_INCM = 19 '-----in Cm
            If vTpMrgn_INCM = 0 Then vTpMrgn_INCM = 6 '----in Cm

            vLftMrgn_INPixel = Val(vLftMrgn_INCM) / 2.54 * 100
            vTpMrgn_INPixel = Val(vTpMrgn_INCM) / 2.54 * 100

        Else
            PrintDocument1.DefaultPageSettings.Landscape = False

            vLftMrgn_INCM = Val(txt_LeftToAdds.Text)
            vTpMrgn_INCM = Val(txt_TOPToAdds.Text)

            If vLftMrgn_INCM = 0 Then vLftMrgn_INCM = 11 '-----5 inch
            If vTpMrgn_INCM = 0 Then vTpMrgn_INCM = 0.5 '----1 Inch

            vLftMrgn_INPixel = Val(vLftMrgn_INCM) / 2.54 * 100
            vTpMrgn_INPixel = Val(vTpMrgn_INCM) / 2.54 * 100

        End If

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                e.PageSettings.PaperSize = ps
                Exit For
            End If
        Next

        If Trim(cbo_PaperOrientation.Text) = "LANDSCAPE" Then
            With PrintDocument1.DefaultPageSettings.Margins
                .Left = vLftMrgn_INPixel
                .Right = 0
                .Top = vTpMrgn_INPixel
                .Bottom = 0
                LMargin = .Left
                RMargin = .Right
                TMargin = .Top
                BMargin = .Bottom
            End With
        Else
            With PrintDocument1.DefaultPageSettings.Margins
                .Left = vLftMrgn_INPixel
                .Right = 0
                .Top = vTpMrgn_INPixel
                .Bottom = 0
                LMargin = .Left
                RMargin = .Right
                TMargin = .Top
                BMargin = .Bottom
            End With
        End If


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

        TxtHgt = 20

        CurY = TMargin

        Try

            If prn_HdDt.Rows.Count > 0 Then

                S1 = e.Graphics.MeasureString("TO    :  ", pFont).Width

                CurY = CurY + TxtHgt - 10
                p1Font = New Font("Calibri", 12, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "TO  :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_MainName").ToString, LMargin + 10, CurY, 0, 0, p1Font)
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)


                Cmp_Name = Trim(prn_HdDt.Rows(0).Item("Company_Name").ToString)

                If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1087" And Cmp_Name <> "SRI BHAGAVAN TEXTILES " Then

                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, " GSTIN No : " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, " Email-Id : " & prn_HdDt.Rows(0).Item("Ledger_Mail").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

                End If



                PhNo1 = ""
                If prn_HdDt.Rows(0).Item("Ledger_PhoneNo").ToString <> "" Then PhNo1 = "PHONE : " & prn_HdDt.Rows(0).Item("Ledger_PhoneNo").ToString

                PhNo2 = ""
                S2 = 0
                If prn_HdDt.Rows(0).Item("MobileNo_Frsms").ToString <> "" Then
                    If Trim(PhNo1) = "" Then
                        PhNo2 = "PHONE : " & prn_HdDt.Rows(0).Item("MobileNo_Frsms").ToString
                    Else
                        PhNo2 = prn_HdDt.Rows(0).Item("MobileNo_Frsms").ToString
                        S2 = e.Graphics.MeasureString("PHONE : ", pFont).Width
                    End If
                End If

                PhNo3 = ""
                S3 = 0
                If prn_HdDt.Rows(0).Item("Ledger_MobileNo").ToString <> "" Then
                    If Trim(PhNo1) = "" And Trim(PhNo2) = "" Then
                        PhNo3 = "PHONE : " & prn_HdDt.Rows(0).Item("Ledger_MobileNo").ToString
                    Else
                        PhNo3 = prn_HdDt.Rows(0).Item("Ledger_MobileNo").ToString
                        S3 = e.Graphics.MeasureString("PHONE : ", pFont).Width
                    End If

                End If

                If Trim(PhNo1) <> "" Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, " " & Trim(PhNo1), LMargin + S1 + 10, CurY, 0, 0, pFont)
                End If
                If Trim(PhNo2) <> "" Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, " " & Trim(PhNo2), LMargin + S1 + S2 + 10, CurY, 0, 0, pFont)
                End If
                If Trim(PhNo3) <> "" Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, " " & Trim(PhNo3), LMargin + S1 + S3 + 10, CurY, 0, 0, pFont)
                End If


                '----- FROM ADDRESS POSITION -----

                vLftMrgn_INCM = 0
                vTpMrgn_INCM = 0

                If Trim(cbo_PaperOrientation.Text) = "LANDSCAPE" Then
                    PrintDocument1.DefaultPageSettings.Landscape = True

                    If chk_FromAddress.Checked = True And Val(txt_LeftFromAdds.Text) <> 0 And Val(txt_TopFromAdds.Text) <> 0 Then
                        vLftMrgn_INCM = Val(txt_LeftFromAdds.Text)
                        vTpMrgn_INCM = Val(txt_TopFromAdds.Text)
                    Else
                        vLftMrgn_INCM = 10.5
                        vTpMrgn_INCM = 9
                    End If

                    vLftMrgn_INPixel = Val(vLftMrgn_INCM) / 2.54 * 100
                    vTpMrgn_INPixel = Val(vTpMrgn_INCM) / 2.54 * 100

                Else
                    PrintDocument1.DefaultPageSettings.Landscape = False

                    vLftMrgn_INCM = Val(txt_LeftFromAdds.Text)
                    vTpMrgn_INCM = Val(txt_TopFromAdds.Text)

                    If chk_FromAddress.Checked = True And Val(txt_LeftFromAdds.Text) <> 0 And Val(txt_TopFromAdds.Text) <> 0 Then
                        vLftMrgn_INCM = Val(txt_LeftFromAdds.Text)
                        vTpMrgn_INCM = Val(txt_TopFromAdds.Text)
                    Else
                        vLftMrgn_INCM = 1.5
                        vTpMrgn_INCM = 2.8
                    End If

                    vLftMrgn_INPixel = Val(vLftMrgn_INCM) / 2.54 * 100
                    vTpMrgn_INPixel = Val(vTpMrgn_INCM) / 2.54 * 100

                End If


                For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                        ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                        PrintDocument1.DefaultPageSettings.PaperSize = ps
                        e.PageSettings.PaperSize = ps
                        Exit For
                    End If
                Next

                With PrintDocument1.DefaultPageSettings.Margins
                    .Left = vLftMrgn_INPixel
                    .Right = 0
                    .Top = vTpMrgn_INPixel
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

                If PrintDocument1.DefaultPageSettings.Landscape = True Then
                    With PrintDocument1.DefaultPageSettings.PaperSize
                        PrintWidth = .Height - TMargin - BMargin
                        PrintHeight = .Width - RMargin - LMargin
                        PageWidth = .Height - TMargin
                        PageHeight = .Width - RMargin
                    End With
                End If

                Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
                Cmp_StateNm = "" : Cmp_GSTIN_No = "" : Cmp_PanNo = "" : Cmp_PhNo = "" : Cmp_Mail = ""

                TxtHgt = 20

                CurY = TMargin

                S1 = e.Graphics.MeasureString("FROM  :  ", pFont).Width

                Cmp_Name = Trim(prn_HdDt.Rows(0).Item("Company_Name").ToString)

                CurY = CurY + TxtHgt - 10
                p1Font = New Font("Calibri", 12, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "FROM  :  " & prn_HdDt.Rows(0).Item("Company_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Company_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Company_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
                'CurY = CurY + TxtHgt
                'Common_Procedures.Print_To_PrintDocument(e, "STATE   : " & prn_HdDt.Rows(0).Item("State_Name").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)


                PhNo1 = ""
                If prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString <> "" Then PhNo1 = "Phone : " & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
                Cmp_Mail = ""


                If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1087" And Cmp_Name <> "SRI BHAGAVAN TEXTILES " Then
                    If prn_HdDt.Rows(0).Item("Company_EMail").ToString <> "" Then Cmp_Mail = "Email : " & prn_HdDt.Rows(0).Item("Company_EMail").ToString
                End If



                If Trim(PhNo1) <> "" Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, " " & Trim(PhNo1), LMargin + S1 + 10, CurY, 0, 0, pFont)
                End If
                If Trim(Cmp_Mail) <> "" Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, " " & Trim(Cmp_Mail), LMargin + S1 + 10, CurY, 0, 0, pFont)
                End If
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format2(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim pFont As Font
        Dim pFontB As Font
        Dim p1Font As Font
        Dim p2Font As Font
        Dim ps As Printing.PaperSize
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim I As Integer
        Dim TxtHgt As Single
        Dim CurY As Single = 0
        Dim ItmNm1 As String = "", ItmNm2 As String = ""
        Dim C1 As Single = 0
        Dim C2 As Single = 0
        Dim S1 As Single = 0
        Dim S2 As Single = 0
        Dim S3 As Single = 0
        Dim PhNo1 As String = ""
        Dim PhNo2 As String = ""
        Dim PhNo3 As String = ""
        Dim LnAr(15) As Single, ClAr(15) As Single



        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                e.PageSettings.PaperSize = ps
                Exit For
            End If
        Next

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 20  'vLftMrgn_INPixl
            .Right = 0
            .Top = 30 'vTpMrgn_INPixl
            .Bottom = 0
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        'pFont = New Font("ARIAL", 10, FontStyle.Regular)
        'pFontB = New Font("ARIAL", 10, FontStyle.Bold)
        'p1Font = New Font("CAMBRIA", 10, FontStyle.Bold)
        p2Font = New Font("ARIAL", 13, FontStyle.Bold)

        pFont = New Font("Calibri", 12, FontStyle.Regular)

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

        Erase LnAr
        Erase ClAr

        ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClAr(1) = 50 : ClAr(2) = 150 : ClAr(3) = 80 : ClAr(4) = 80 : ClAr(5) = 80 : ClAr(6) = 80 : ClAr(7) = 80 : ClAr(8) = 80 : ClAr(9) = 80
        ClAr(10) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9))
        C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5)
        C2 = C1 + ClAr(6) + ClAr(7)

        TxtHgt = 40

        CurY = TMargin

        Try

            If prn_HdDt.Rows.Count > 0 Then

                S1 = e.Graphics.MeasureString("", pFont).Width

                e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth - 50, CurY)         '-------TOP Border
                'e.Graphics.DrawLine(Pens.Black, LMargin, CurY + 30, PageWidth - 50, CurY + 30)

                LnAr(1) = CurY

                If Trim(UCase(vLedType)) = "WEAVER" Then
                    CurY = CurY + 10
                    Common_Procedures.Print_To_PrintDocument(e, "WEAVER DETAILS", LMargin + C1 - 150, CurY, 0, ClAr(1), p2Font)
                    e.Graphics.DrawLine(Pens.Black, LMargin, CurY + 30, PageWidth - 50, CurY + 30)
                    LnAr(2) = CurY
                Else
                    CurY = CurY + 10
                    Common_Procedures.Print_To_PrintDocument(e, "LEDGER DETAILS", LMargin + C1 - 150, CurY, 0, ClAr(1), p2Font)
                    e.Graphics.DrawLine(Pens.Black, LMargin, CurY + 30, PageWidth - 50, CurY + 30)
                    LnAr(2) = CurY
                End If


                Common_Procedures.Print_To_PrintDocument(e, "Name", LMargin + 20, CurY + 40, 0, LMargin + 20, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + 180, CurY + 40, 0, PageHeight, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + ClAr(2) + 100, CurY + 40, 0, 0, pFont)


                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Address", LMargin + 20, CurY + 40, 0, LMargin + 20, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + 180, CurY + 40, 0, PageHeight, pFont)

                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + ClAr(2) + 100, CurY + 40, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + ClAr(2) + 100, CurY + 60, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + ClAr(2) + 270, CurY + 60, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + ClAr(2) + 100, CurY + 80, 0, 0, pFont)


                CurY = CurY + TxtHgt + 20
                Common_Procedures.Print_To_PrintDocument(e, "Proprietor Name", LMargin + 20, CurY + 40, 0, LMargin + 20, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + 180, CurY + 40, 0, PageHeight, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Owner_Name").ToString, LMargin + ClAr(2) + 100, CurY + 40, 0, 0, pFont)


                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Phone No", LMargin + 20, CurY + 40, 0, LMargin + 20, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + 180, CurY + 40, 0, PageHeight, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Ledger_PhoneNo").ToString), LMargin + ClAr(2) + 100, CurY + 40, 0, 0, pFont)

                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Gmail-id", LMargin + 20, CurY + 40, 0, LMargin + 20, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + 180, CurY + 40, 0, PageHeight, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Mail").ToString, LMargin + ClAr(2) + 100, CurY + 40, 0, 0, pFont)


                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Aadhar No", LMargin + 20, CurY + 40, 0, LMargin + 20, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + 180, CurY + 40, 0, PageHeight, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Aadhar_No").ToString, LMargin + ClAr(2) + 100, CurY + 40, 0, 0, pFont)

                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "PAN No", LMargin + 20, CurY + 40, 0, LMargin + 20, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + 180, CurY + 40, 0, PageHeight, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Pan_No").ToString, LMargin + ClAr(2) + 100, CurY + 40, 0, 0, pFont)


                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "GSTIN No", LMargin + 20, CurY + 40, 0, LMargin + 20, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + 180, CurY + 40, 0, PageHeight, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + ClAr(2) + 100, CurY + 40, 0, 0, pFont)


                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Bank Account No", LMargin + 20, CurY + 40, 0, LMargin + 20, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + 180, CurY + 40, 0, PageHeight, pFont)
                '  Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Bank_Account_No").ToString, LMargin + ClAr(2) + 100, CurY + 40, 0, 0, pFont)


                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "IFSC Code", LMargin + 20, CurY + 40, 0, LMargin + 20, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + 180, CurY + 40, 0, PageHeight, pFont)
                '  Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("IFSC_Code").ToString, LMargin + ClAr(2) + 100, CurY + 40, 0, 0, pFont)

                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Loom No", LMargin + 20, CurY + 40, 0, LMargin + 20, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + 180, CurY + 40, 0, PageHeight, pFont)
                '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Loom_No").ToString, LMargin + ClAr(2) + 100, CurY + 40, 0, 0, pFont)


                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Loom Model", LMargin + 20, CurY + 40, 0, LMargin + 20, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + 180, CurY + 40, 0, PageHeight, pFont)
                '  Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("IFSC_Code").ToString, LMargin + ClAr(2) + 100, CurY + 40, 0, 0, pFont)



                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Production Capacity", LMargin + 20, CurY + 40, 0, LMargin + 20, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + 180, CurY + 40, 0, PageHeight, pFont)
                'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("IFSC_Code").ToString, LMargin + ClAr(2) + 100, CurY + 40, 0, 0, pFont)



                '    PhNo2 = ""
                '    S2 = 0
                '    If prn_HdDt.Rows(0).Item("MobileNo_Frsms").ToString <> "" Then
                '        If Trim(PhNo1) = "" Then
                '            PhNo2 = prn_HdDt.Rows(0).Item("MobileNo_Frsms").ToString
                '        Else
                '            PhNo2 = prn_HdDt.Rows(0).Item("MobileNo_Frsms").ToString
                '            S2 = e.Graphics.MeasureString("PHONE", pFontB).Width
                '        End If
                '    End If

                '    PhNo3 = ""
                '    S3 = 0
                '    If prn_HdDt.Rows(0).Item("Ledger_MobileNo").ToString <> "" Then
                '        If Trim(PhNo1) = "" And Trim(PhNo2) = "" Then
                '            PhNo3 = prn_HdDt.Rows(0).Item("Ledger_MobileNo").ToString
                '        Else
                '            PhNo3 = prn_HdDt.Rows(0).Item("Ledger_MobileNo").ToString
                '            S3 = e.Graphics.MeasureString("PHONE : ", pFontB).Width
                '        End If

                '    End If

                '    If Trim(PhNo1) <> "" Then
                '        CurY = CurY + TxtHgt
                '        Common_Procedures.Print_To_PrintDocument(e, "PHONE No.", LMargin + 20, CurY + 40, 0, LMargin + 20, pFontB)
                '        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Ledger_PhoneNo").ToString), LMargin + ClAr(2) + 100, CurY + 40, 0, 0, pFont)
                '    End If

                '    CurY = CurY + TxtHgt
                '    Common_Procedures.Print_To_PrintDocument(e, "GSTIN No", LMargin + 20, CurY + 40, 0, LMargin + 20, pFontB)
                '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + ClAr(2) + 100, CurY + 40, 0, 0, pFont)
                '    CurY = CurY + TxtHgt
                '    Common_Procedures.Print_To_PrintDocument(e, "AADHAR No", LMargin + 20, CurY + 40, 0, LMargin + 20, pFontB)
                '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Aadhar_No").ToString, LMargin + ClAr(2) + 100, CurY + 40, 0, 0, pFont)
                '    CurY = CurY + TxtHgt
                '    Common_Procedures.Print_To_PrintDocument(e, "Mail-ID", LMargin + 20, CurY + 40, 0, LMargin + 20, pFontB)
                '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Mail").ToString, LMargin + ClAr(2) + 100, CurY + 40, 0, 0, pFont)
                '    CurY = CurY + TxtHgt
                '    Common_Procedures.Print_To_PrintDocument(e, "PAN No", LMargin + 20, CurY + 40, 0, LMargin + 20, pFontB)
                '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Pan_No").ToString, LMargin + ClAr(2) + 100, CurY + 40, 0, 0, pFont)
                '    CurY = CurY + TxtHgt
                '    Common_Procedures.Print_To_PrintDocument(e, "CST No", LMargin + 20, CurY + 40, 0, LMargin + 20, pFontB)
                '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_CstNo").ToString, LMargin + ClAr(2) + 100, CurY + 40, 0, 0, pFont)
                '    CurY = CurY + TxtHgt
                '    Common_Procedures.Print_To_PrintDocument(e, "TIN No", LMargin + 20, CurY + 40, 0, LMargin + 20, pFontB)
                '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString, LMargin + ClAr(2) + 100, CurY + 40, 0, 0, pFont)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY + 100, PageWidth - 50, CurY + 100) '-----Bottom Border
        '' e.Graphics.DrawLine(Pens.Black, LMargin + 200, LnAr(2) + 30, LMargin + 200, CurY + 100) '-------Center Line

        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY + 100) '-------Left Border
        e.Graphics.DrawLine(Pens.Black, PageWidth - 50, LnAr(1), PageWidth - 50, CurY + 100) '-------Right Border
        'e.Graphics.DrawLine(Pens.Black, LMargin, CurY - 50, PageWidth - 50, CurY - 50) '-----TableLast Border
        LnAr(3) = CurY

        e.HasMorePages = False

    End Sub

    Private Sub btn_Loom_Details_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Loom_Details.Click
        pnl_Loom_Details.Visible = True
        pnl_Loom_Details.BringToFront()
        pnl_Loom_Details.Enabled = True
        'txt_RatePrReel.Focus()
        grp_Back.Enabled = False
        If dgv_Loom_Details.Rows.Count > 0 Then
            dgv_Loom_Details.Focus()
            dgv_Loom_Details.CurrentCell = dgv_Loom_Details.Rows(0).Cells(1)

        End If

    End Sub

    Private Sub btn_Tamil_Address_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Tamil_Address.Click
        pnl_tamil_Address.Visible = True
        pnl_tamil_Address.BringToFront()
        pnl_tamil_Address.Enabled = True
        txt_TamilName.Focus()
        grp_Back.Enabled = False


    End Sub

    Private Sub btn_Close_LoomDetails_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_LoomDetails.Click
        Close_LoomSelection()
        'pnl_Loom_Details.Visible = False
        'grp_Back.Enabled = True
        'If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()
    End Sub

    Private Sub btn_pnl_tamil_Address_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_pnl_Close_tamil_Address.Click
        pnl_tamil_Address.Visible = False
        grp_Back.Enabled = True
        If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()
    End Sub

    Private Sub Close_LoomSelection()
        pnl_Loom_Details.Visible = False
        grp_Back.Enabled = True
        If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()
    End Sub
    Private Sub dgtxt_Loomdetails_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_LoomDetails.Enter
        dgv_ActiveCtrl_Name = dgv_Loom_Details.Name
        dgv_Loom_Details.EditingControl.BackColor = Color.Lime
        dgv_Loom_Details.EditingControl.ForeColor = Color.Blue
    End Sub

    Private Sub dgtxt_Kuridetails_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_LoomDetails.KeyPress
        Try

            With dgv_Loom_Details

                If Val(dgv_Loom_Details.CurrentCell.ColumnIndex.ToString) = 1 Then

                    If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                        e.Handled = True
                    End If
                End If

            End With

        Catch ex As Exception
            '---

        End Try
    End Sub

    Private Sub dgv_Loom_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Loom_Details.CellEnter

        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Rect As Rectangle
        With dgv_Loom_Details


            dgv_ActiveCtrl_Name = dgv_Loom_Details.Name
            If Val(dgv_Loom_Details.CurrentRow.Cells(0).Value) = 0 Then
                dgv_Loom_Details.CurrentRow.Cells(0).Value = dgv_Loom_Details.CurrentRow.Index + 1
            End If

            If e.ColumnIndex = 2 Then

                If cbo_Grid_ClothName.Visible = False Or Val(cbo_Grid_ClothName.Tag) <> e.RowIndex Then

                    cbo_Grid_ClothName.Tag = -100
                    Da = New SqlClient.SqlDataAdapter("select Cloth_Name from Cloth_Head order by Cloth_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Grid_ClothName.DataSource = Dt1
                    cbo_Grid_ClothName.DisplayMember = "Cloth_Name"

                    Rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_ClothName.Left = .Left + Rect.Left
                    cbo_Grid_ClothName.Top = .Top + Rect.Top

                    cbo_Grid_ClothName.Width = Rect.Width
                    cbo_Grid_ClothName.Height = Rect.Height
                    cbo_Grid_ClothName.Text = .CurrentCell.Value

                    cbo_Grid_ClothName.Tag = Val(e.RowIndex)
                    cbo_Grid_ClothName.Visible = True

                    cbo_Grid_ClothName.BringToFront()
                    cbo_Grid_ClothName.Focus()

                Else

                    'If cbo_Grid_ClothName.Visible = True Then
                    '    cbo_Grid_ClothName.BringToFront()
                    '    cbo_Grid_ClothName.Focus()
                    'End If

                End If

            Else
                cbo_Grid_ClothName.Visible = False

            End If

        End With

    End Sub
    Private Sub dgv_LoomDetails_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Loom_Details.EditingControlShowing
        dgtxt_LoomDetails = CType(dgv_Loom_Details.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub btn_Import_Master_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Import_Master.Click
        Dim CmdFrm As New SqlClient.SqlCommand
        Dim CmdTo As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim vLedger_IdNo As Integer = 0, vOldLID As Integer = 0
        Dim vLedger_Name As String = "", vSur_Name As String, vLedger_MainName As String, vLedger_AlaisName As String, vLedger_StateName As String
        Dim vArea_IdNo As Integer, vAccountsGroup_IdNo As Integer, vState_IdNo As Integer, vParent_Code As String, vBill_Type As String
        Dim vLedger_Address1 As String, vLedger_Address2 As String, vLedger_Address3 As String, vLedger_Address4 As String, vLedger_PhoneNo As String
        Dim vLedger_TinNo As String, vLedger_CstNo As String, vLedger_Type As String, vPan_No As String
        Dim vLedger_Emailid As String, vLedger_FaxNo As String, vLedger_MobileNo As String, vContact_Person As String, vLedger_GSTNo As String
        Dim vPackingType_CompanyIdNo As Integer, vLedger_AgentIdNo As Integer, vTransport_Name As String, vNote As String
        Dim vMobileNo_Sms As String, vBilling_Type As String, vSticker_Type As String, vMrp_Perc As String
        Dim vLedNm As String
        Dim sqltr As SqlClient.SqlTransaction
        Dim cmd As New SqlClient.SqlCommand
        'Dim tr As SqlClient.SqlTransaction
        Dim da As New SqlClient.SqlDataAdapter

        Dim xlApp As Excel.Application
        Dim xlWorkBook As Excel.Workbook
        Dim xlWorkSheet As Excel.Worksheet

        Dim RowCnt As Long = 0
        Dim FileName As String = ""
        Dim NewCode As String = ""
        Dim ShfId As Integer = 0
        Dim ItmId As Integer = 0
        Dim j, k, l As Integer
        Dim Sn As Integer = 0
        Dim vSurNm As String = ""
        Dim vShow_In_All_Entry As Integer, vVerified_Status As Integer = 0
        Dim vTransport_IdNo As Integer, vNoOf_Looms As Integer
        Dim vFreight_Loom As Single
        Dim vOwn_Loom_Status As Integer
        Dim vTds_Percentage As Single
        Dim vOwner_Name As String
        Dim vPartner_Proprietor As String
        Dim vCloth_Comm_Meter As Single = 0, vCloth_Comm_Percentage As Single
        Dim vYarn_Comm_Bag As Single, vYarn_Comm_Percentage As Single
        Dim AccGrpAr() As String
        Dim Inc As Integer = 0
        Dim AccGrp1 As String = ""
        Dim AccGrp2 As String = ""
        Dim AccGrp3 As String = ""
        Dim AccGrp4 As String = ""
        Dim vLedger_StateCode As String = ""
        Dim vSALES_PUR_Type As String
        Dim vADD_ar As String()
        Dim vADD As String = ""
        Dim INDX As Integer = -1

        txt_Name.Focus()
        btn_Import_Master.Enabled = False

        Dim g As New Password
        g.ShowDialog()

        If Trim(UCase(Common_Procedures.Password_Input)) <> "TSIMP7417" Then
            MessageBox.Show("Invalid Password", "PASSWORD FAILED...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            btn_Import_Master.Enabled = True
            Exit Sub
        End If

        CmdTo.Connection = con

        sqltr = con.BeginTransaction

        CmdTo.Transaction = sqltr

        Try

            OpenFileDialog1.ShowDialog()
            FileName = OpenFileDialog1.FileName

            If Not IO.File.Exists(FileName) Then
                MessageBox.Show(FileName & " File not found", "File not found...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub
            End If

            xlApp = New Excel.Application
            xlWorkBook = xlApp.Workbooks.Open(FileName)
            xlWorkSheet = xlWorkBook.Worksheets("sheet1")

            With xlWorkSheet
                RowCnt = .Range("A" & .Rows.Count).End(Excel.XlDirection.xlUp).Row
            End With

            'RowCnt = xlWorkSheet.UsedRange.Rows.Count

            If RowCnt <= 1 Then
                MessageBox.Show("Data not found", "Data not found...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub
            End If


            For i = 2 To RowCnt

                vLedger_Address1 = ""
                vLedger_Address2 = ""
                vLedger_Address3 = ""
                vLedger_Address4 = ""

                vLedNm = UCase(Trim(xlWorkSheet.Cells(i, 2).value))
                vLedNm = Replace(UCase(Trim(vLedNm)), "'", "")

                If Trim(vLedNm) = "" Then
                    Continue For
                End If

                vSur_Name = Common_Procedures.Remove_NonCharacters(vLedNm)

                vOldLID = Val(Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_IdNo", "(Sur_Name = '" & Trim(vSur_Name) & "')", , sqltr))

                If vOldLID = 0 Then

                    vLedger_IdNo = Common_Procedures.get_MaxIdNo(con, "ledger_head", "ledger_idno", "", sqltr)
                    If Val(vLedger_IdNo) < 100 Then
                        vLedger_IdNo = 101
                    End If

                    vLedger_Name = Trim(Replace(UCase(Trim(xlWorkSheet.Cells(i, 2).value)), "'", ""))
                    vLedger_MainName = vLedger_Name
                    vSur_Name = Common_Procedures.Remove_NonCharacters(vLedger_Name)
                    vLedger_AlaisName = ""
                    vArea_IdNo = 0

                    vSALES_PUR_Type = xlWorkSheet.Cells(i, 8).value

                    If Val(vSALES_PUR_Type) = 2 Or Trim(UCase(vSALES_PUR_Type)) = "SUNDRY CREDITORS" Then
                        vAccountsGroup_IdNo = 14
                        vParent_Code = "~14~11~"
                    Else
                        vAccountsGroup_IdNo = 10
                        vParent_Code = "~10~4~"
                    End If

                    vBill_Type = "BILL TO BILL"
                    'vBill_Type = "BALANCE ONLY"

                    vADD = Trim(xlWorkSheet.Cells(i, 3).value)
                    vADD = Replace(Trim(vADD), "'", "`")

                    Erase vADD_ar
                    vADD_ar = Split(vADD, ",")


                    vLedger_Address1 = ""
                    vLedger_Address2 = ""
                    vLedger_Address3 = ""
                    vLedger_Address4 = ""

                    INDX = -1



                    'If vLedger_IdNo = 214 Then
                    '    Debug.Print(vLedger_IdNo)
                    'End If

LOOP1:
                    INDX = INDX + 1
                    If UBound(vADD_ar) >= INDX Then
                        If Trim(vADD_ar(INDX)) <> "" Then
                            vLedger_Address1 = Trim(vLedger_Address1) & vADD_ar(INDX) & IIf(UBound(vADD_ar) = INDX, IIf(Microsoft.VisualBasic.Right(Trim(vADD_ar(INDX)), 1) <> ".", ".", ""), ",")
                            If Len(Trim(vLedger_Address1)) < 5 Then
                                GoTo LOOP1
                            ElseIf Len(Trim(vLedger_Address2)) <= 15 And ((INDX + 5) > UBound(vADD_ar) And ((INDX + 1) <> UBound(vADD_ar)) And (INDX + 2) <> UBound(vADD_ar) And (INDX + 3) <> UBound(vADD_ar) And (INDX + 4) <> UBound(vADD_ar)) Then
                                GoTo LOOP1
                            End If

                        Else
                            GoTo LOOP1

                        End If


                    End If

LOOP2:
                    INDX = INDX + 1
                    If UBound(vADD_ar) >= INDX Then
                        If Trim(vADD_ar(INDX)) <> "" Then
                            vLedger_Address2 = Trim(vLedger_Address2) & vADD_ar(INDX) & IIf(UBound(vADD_ar) = INDX, IIf(Microsoft.VisualBasic.Right(Trim(vADD_ar(INDX)), 1) <> ".", ".", ""), ",")
                            If Len(Trim(vLedger_Address2)) <= 5 Then
                                GoTo LOOP2
                            ElseIf Len(Trim(vLedger_Address2)) <= 15 And ((INDX + 5) > UBound(vADD_ar) And ((INDX + 1) <> UBound(vADD_ar)) And (INDX + 2) <> UBound(vADD_ar) And (INDX + 3) <> UBound(vADD_ar) And (INDX + 4) <> UBound(vADD_ar)) Then
                                GoTo LOOP2
                            End If

                        Else
                            GoTo LOOP2
                        End If

                    End If

                    If vLedger_IdNo = 213 Then
                        Debug.Print(vLedger_IdNo)
                    End If

LOOP3:
                    INDX = INDX + 1
                    If UBound(vADD_ar) >= INDX Then
                        If Trim(vADD_ar(INDX)) <> "" Then
                            vLedger_Address3 = Trim(vLedger_Address3) & vADD_ar(INDX) & IIf(UBound(vADD_ar) = INDX, IIf(Microsoft.VisualBasic.Right(Trim(vADD_ar(INDX)), 1) <> ".", ".", ""), ",")
                            If Len(Trim(vLedger_Address3)) <= 5 Then
                                GoTo LOOP3
                            ElseIf Len(Trim(vLedger_Address3)) <= 15 And ((INDX + 3) > UBound(vADD_ar) And ((INDX + 1) <> UBound(vADD_ar)) And (INDX + 2) <> UBound(vADD_ar)) Then
                                GoTo LOOP3
                            End If

                        Else
                            GoTo LOOP3
                        End If

                    End If

LOOP4:
                    INDX = INDX + 1
                    If UBound(vADD_ar) >= INDX Then
                        If Trim(vADD_ar(INDX)) <> "" Then
                            vLedger_Address4 = Trim(vLedger_Address4) & vADD_ar(INDX) & IIf(UBound(vADD_ar) = INDX, IIf(Microsoft.VisualBasic.Right(Trim(vADD_ar(INDX)), 1) <> ".", ".", ""), ",")
                            GoTo LOOP4
                        Else
                            GoTo LOOP4
                        End If
                    End If


                    'If Trim(vLedger_Address1) <> "" Then
                    '    If Len(vLedger_Address1) > 40 Then
                    '        For j = 40 To 1 Step -1
                    '            If Mid$(Trim(vLedger_Address1), j, 1) = " " Or Mid$(Trim(vLedger_Address1), j, 1) = "," Then Exit For
                    '        Next j
                    '        If j = 0 Then j = 40
                    '        vLedger_Address2 = Microsoft.VisualBasic.Right(Trim(vLedger_Address1), Len(vLedger_Address1) - j)
                    '        vLedger_Address1 = Microsoft.VisualBasic.Left(Trim(vLedger_Address1), j - 1)

                    '    End If
                    'End If


                    'If Trim(vLedger_Address2) <> "" Then
                    '    If Len(vLedger_Address2) > 40 Then
                    '        For k = 40 To 1 Step -1
                    '            If Mid$(Trim(vLedger_Address1), k, 1) = " " Or Mid$(Trim(vLedger_Address1), k, 1) = "," Or Mid$(Trim(vLedger_Address1), k, 1) = "." Then Exit For
                    '        Next k
                    '        If k = 0 Then k = 40
                    '        vLedger_Address3 = Microsoft.VisualBasic.Right(Trim(vLedger_Address2), Len(vLedger_Address2) - k)
                    '        vLedger_Address2 = Microsoft.VisualBasic.Left(Trim(vLedger_Address2), k - 1)
                    '    End If
                    'End If


                    'If Trim(vLedger_Address3) <> "" Then
                    '    If Len(vLedger_Address3) > 70 Then
                    '        For l = 70 To 1 Step -1
                    '            If Mid$(Trim(vLedger_Address1), l, 1) = " " Or Mid$(Trim(vLedger_Address1), l, 1) = "," Or Mid$(Trim(vLedger_Address1), l, 1) = "." Then Exit For
                    '        Next l
                    '        If l = 0 Then l = 70
                    '        vLedger_Address4 = Microsoft.VisualBasic.Right(Trim(vLedger_Address3), Len(vLedger_Address3) - l)
                    '        vLedger_Address3 = Microsoft.VisualBasic.Left(Trim(vLedger_Address3), l - 1)
                    '    End If
                    'End If


                    vLedger_GSTNo = Trim(xlWorkSheet.Cells(i, 7).value)
                    vState_IdNo = 0
                    vLedger_StateName = ""
                    If Trim(vLedger_GSTNo) <> "" Then
                        vLedger_StateCode = Microsoft.VisualBasic.Left(vLedger_GSTNo, 2)
                        vState_IdNo = Val(Common_Procedures.get_FieldValue(con, "State_Head", "State_Idno", "(State_Code = '" & Trim(vLedger_StateCode) & "')", , sqltr))
                    Else
                        vLedger_StateName = Trim(xlWorkSheet.Cells(i, 4).value)
                        vState_IdNo = Common_Procedures.State_NameToIdNo(con, Trim(vLedger_StateName), sqltr)

                    End If




                    vLedger_PhoneNo = ""
                    vLedger_TinNo = ""
                    vLedger_CstNo = ""
                    vLedger_Type = ""
                    vPan_No = ""
                    vLedger_Emailid = ""
                    vLedger_FaxNo = ""
                    vLedger_MobileNo = ""
                    vContact_Person = ""
                    vPackingType_CompanyIdNo = 0
                    vLedger_AgentIdNo = 0
                    vTransport_Name = ""
                    vNote = ""
                    vMobileNo_Sms = ""
                    vBilling_Type = ""

                    Me.Text = vLedger_IdNo & "  -  " & Trim(vLedger_Name)

                    CmdTo.CommandText = "Insert into ledger_head ( Ledger_IdNo        ,            Ledger_Name      ,            Sur_Name      ,            Ledger_MainName      ,            Ledger_AlaisName      ,              Area_IdNo      ,              AccountsGroup_IdNo      ,            Parent_Code      ,            Bill_Type      ,            Ledger_Address1      ,            Ledger_Address2      ,            Ledger_Address3      ,            Ledger_Address4      ,            Ledger_PhoneNo      ,            Ledger_TinNo      ,            Ledger_CstNo      ,            Ledger_Type      ,            Pan_No      ,            Partner_Proprietor      ,              Yarn_Comm_Percentage      ,              Yarn_Comm_Bag      ,              Cloth_Comm_Percentage      ,            Ledger_Emailid      ,            Ledger_FaxNo      ,            Ledger_MobileNo      ,            MobileNo_Frsms    ,            MobileNo_Sms      ,            Contact_Person      ,             PackingType_CompanyIdNo       ,              Ledger_AgentIdNo      ,            Note      ,            Show_In_All_Entry        ,            Billing_Type      ,            Sticker_Type      ,            Mrp_Perc      ,              Own_Loom_Status      ,              Freight_Loom      ,              NoOf_Looms      ,              Transport_IdNo      , Verified_Status,            Owner_Name      ,              Tds_Percentage     , Ledger_GSTinNo                 ,  Ledger_State_IdNo   ) " &
                                 "       Values (" & Str(Val(vLedger_IdNo)) & ", '" & Trim(vLedger_Name) & "', '" & Trim(vSur_Name) & "', '" & Trim(vLedger_MainName) & "', '" & Trim(vLedger_AlaisName) & "', " & Str(Val(vArea_IdNo)) & ", " & Str(Val(vAccountsGroup_IdNo)) & ", '" & Trim(vParent_Code) & "', '" & Trim(vBill_Type) & "', '" & Trim(vLedger_Address1) & "', '" & Trim(vLedger_Address2) & "', '" & Trim(vLedger_Address3) & "', '" & Trim(vLedger_Address4) & "', '" & Trim(vLedger_PhoneNo) & "', '" & Trim(vLedger_TinNo) & "', '" & Trim(vLedger_CstNo) & "', '" & Trim(vLedger_Type) & "', '" & Trim(vPan_No) & "', '" & Trim(vPartner_Proprietor) & "', " & Str(Val(vYarn_Comm_Percentage)) & ", " & Str(Val(vYarn_Comm_Bag)) & ", " & Str(Val(vCloth_Comm_Percentage)) & ", '" & Trim(vLedger_Emailid) & "', '" & Trim(vLedger_FaxNo) & "', '" & Trim(vLedger_MobileNo) & "', '" & Trim(vMobileNo_Sms) & "', '" & Trim(vMobileNo_Sms) & "', '" & Trim(vContact_Person) & "', " & Str(Val(vPackingType_CompanyIdNo)) & ", " & Str(Val(vLedger_AgentIdNo)) & ", '" & Trim(vNote) & "', " & Str(Val(vShow_In_All_Entry)) & ", '" & Trim(vBilling_Type) & "', '" & Trim(vSticker_Type) & "', '" & Trim(vMrp_Perc) & "', " & Str(Val(vOwn_Loom_Status)) & ", " & Str(Val(vFreight_Loom)) & ", " & Str(Val(vNoOf_Looms)) & ", " & Str(Val(vTransport_IdNo)) & ",       1        , '" & Trim(vOwner_Name) & "', " & Str(Val(vTds_Percentage)) & " , '" & Trim(vLedger_GSTNo) & "'  ," & Str(Val(vState_IdNo)) & " ) "
                    CmdTo.ExecuteNonQuery()

                    'CmdTo.CommandText = "Insert into Ledger_AlaisHead ( Ledger_IdNo, Sl_No, Ledger_DisplayName, Ledger_Type, AccountsGroup_IdNo, Verified_Status ) Values (" & Str(Val(vLedger_IdNo)) & ",   1,     '" & Trim(vLedger_Name) & "',   '" & Trim(vLedger_Type) & "',  " & Str(Val(vAccountsGroup_IdNo)) & ",  1 )"
                    'CmdTo.ExecuteNonQuery()

                End If


            Next i


            CmdTo.CommandText = "delete from Ledger_AlaisHead Where Ledger_IdNo > 100"
            CmdTo.ExecuteNonQuery()

            Da1 = New SqlClient.SqlDataAdapter("select * from Ledger_Head where Ledger_IdNo > 100", con)
            Da1.SelectCommand.Transaction = sqltr
            Dt1 = New DataTable
            Da1.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1
                    CmdTo.CommandText = "Insert into Ledger_AlaisHead(Ledger_IdNo, Sl_No, Ledger_DisplayName, Ledger_Type, AccountsGroup_IdNo, Own_Loom_Status, Show_In_All_Entry, Verified_Status , Area_IdNo, Close_status) Values (" & Str(Val(Dt1.Rows(i).Item("Ledger_IdNo").ToString)) & ",    1,      '" & Trim(Dt1.Rows(i).Item("Ledger_Name").ToString) & "',   '" & Trim(Dt1.Rows(i).Item("Ledger_Type").ToString) & "',    " & Str(Val(Dt1.Rows(i).Item("AccountsGroup_IdNo").ToString)) & ", 0, 0, 1,  0, 0)"
                    CmdTo.ExecuteNonQuery()
                Next

            End If


            sqltr.Commit()

            CmdTo.Dispose()
            Dt1.Dispose()
            Da1.Dispose()

            xlWorkBook.Close(False, FileName)
            xlApp.Quit()

            ReleaseComObject(xlWorkSheet)
            ReleaseComObject(xlWorkBook)
            ReleaseComObject(xlApp)

            txt_Name.Focus()

            MessageBox.Show("Imported Successfully!!!", "FOR IMPORTING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

            btn_Import_Master.Enabled = True


        Catch ex As Exception
            txt_Name.Focus()
            btn_Import_Master.Enabled = True
            sqltr.Rollback()
            MessageBox.Show("LedgerIdNo : " & vLedger_IdNo & "  -  " & Trim(vLedger_Name) & Chr(13) & ex.Message, "DOES NOT IMPORT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub btn_Import_Master_Click_222(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Import_Master.Click
        Dim CmdFrm As New SqlClient.SqlCommand
        Dim CmdTo As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim vLedger_IdNo As Integer, vOldLID As Integer
        Dim vLedger_Name As String, vSur_Name As String, vLedger_MainName As String, vLedger_AlaisName As String, vLedger_StateName As String
        Dim vArea_IdNo As Integer, vAccountsGroup_IdNo As Integer, vState_IdNo As Integer, vParent_Code As String, vBill_Type As String
        Dim vLedger_Address1 As String, vLedger_Address2 As String, vLedger_Address3 As String, vLedger_Address4 As String, vLedger_PhoneNo As String
        Dim vLedger_TinNo As String, vLedger_CstNo As String, vLedger_Type As String, vPan_No As String
        Dim vLedger_Emailid As String, vLedger_FaxNo As String, vLedger_MobileNo As String, vContact_Person As String, vLedger_GSTNo As String
        Dim vPackingType_CompanyIdNo As Integer, vLedger_AgentIdNo As Integer, vTransport_Name As String, vNote As String
        Dim vMobileNo_Sms As String, vBilling_Type As String, vSticker_Type As String, vMrp_Perc As String
        Dim vLedNm As String
        Dim sqltr As SqlClient.SqlTransaction
        Dim cmd As New SqlClient.SqlCommand
        'Dim tr As SqlClient.SqlTransaction
        Dim da As New SqlClient.SqlDataAdapter

        Dim xlApp As Excel.Application
        Dim xlWorkBook As Excel.Workbook
        Dim xlWorkSheet As Excel.Worksheet

        Dim RowCnt As Long = 0
        Dim FileName As String = ""
        Dim NewCode As String = ""
        Dim ShfId As Integer = 0
        Dim ItmId As Integer = 0
        Dim j, k, l As Integer
        Dim Sn As Integer = 0
        Dim vSurNm As String = ""
        Dim vShow_In_All_Entry As Integer, vVerified_Status As Integer = 0
        Dim vTransport_IdNo As Integer, vNoOf_Looms As Integer
        Dim vFreight_Loom As Single
        Dim vOwn_Loom_Status As Integer
        Dim vTds_Percentage As Single
        Dim vOwner_Name As String
        Dim vPartner_Proprietor As String
        Dim vCloth_Comm_Meter As Single = 0, vCloth_Comm_Percentage As Single
        Dim vYarn_Comm_Bag As Single, vYarn_Comm_Percentage As Single
        Dim AccGrpAr() As String
        Dim Inc As Integer = 0
        Dim AccGrp1 As String = ""
        Dim AccGrp2 As String = ""
        Dim AccGrp3 As String = ""
        Dim AccGrp4 As String = ""
        Dim vLedger_StateCode As String = ""
        Dim vSALES_PUR_Type As String

        Dim g As New Password
        g.ShowDialog()

        If Trim(UCase(Common_Procedures.Password_Input)) <> "TSIMP7417" Then
            MessageBox.Show("Invalid Password", "PASSWORD FAILED...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If


        CmdTo.Connection = con

        sqltr = con.BeginTransaction

        CmdTo.Transaction = sqltr

        Try

            OpenFileDialog1.ShowDialog()
            FileName = OpenFileDialog1.FileName

            If Not IO.File.Exists(FileName) Then
                MessageBox.Show(FileName & " File not found", "File not found...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub
            End If

            xlApp = New Excel.Application
            xlWorkBook = xlApp.Workbooks.Open(FileName)
            xlWorkSheet = xlWorkBook.Worksheets("sheet1")

            With xlWorkSheet
                RowCnt = .Range("A" & .Rows.Count).End(Excel.XlDirection.xlUp).Row
            End With

            'RowCnt = xlWorkSheet.UsedRange.Rows.Count

            If RowCnt <= 1 Then
                MessageBox.Show("Data not found", "Data not found...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub
            End If

            For i = 2 To RowCnt

                vLedger_Address1 = ""
                vLedger_Address2 = ""
                vLedger_Address3 = ""
                vLedger_Address4 = ""

                vLedNm = UCase(Trim(xlWorkSheet.Cells(i, 1).value))
                vLedNm = Replace(UCase(Trim(vLedNm)), "'", "")

                If Trim(vLedNm) = "" Then
                    Continue For
                End If

                vSur_Name = Common_Procedures.Remove_NonCharacters(vLedNm)

                vOldLID = Val(Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_IdNo", "(Sur_Name = '" & Trim(vSur_Name) & "')", , sqltr))

                If vOldLID = 0 Then

                    vLedger_IdNo = Common_Procedures.get_MaxIdNo(con, "ledger_head", "ledger_idno", "", sqltr)
                    If Val(vLedger_IdNo) < 100 Then
                        vLedger_IdNo = 101
                    End If

                    vLedger_Name = Trim(Replace(UCase(Trim(xlWorkSheet.Cells(i, 1).value)), "'", ""))
                    vLedger_MainName = vLedger_Name
                    vSur_Name = Common_Procedures.Remove_NonCharacters(vLedger_Name)
                    vLedger_AlaisName = ""
                    vArea_IdNo = 0

                    vSALES_PUR_Type = xlWorkSheet.Cells(i, 4).value

                    If Val(vSALES_PUR_Type) = 2 Then
                        vAccountsGroup_IdNo = 14
                        vParent_Code = "~14~11~"
                    Else
                        vAccountsGroup_IdNo = 10
                        vParent_Code = "~10~4~"
                    End If

                    vBill_Type = "BILL TO BILL"
                    'vBill_Type = "BALANCE ONLY"

                    vLedger_Address1 = Trim(xlWorkSheet.Cells(i, 3).value)
                    vLedger_Address2 = ""
                    If Trim(vLedger_Address1) <> "" Then
                        If Len(vLedger_Address1) > 40 Then

                            For j = 40 To 1 Step -1
                                If Mid$(Trim(vLedger_Address1), j, 1) = " " Or Mid$(Trim(vLedger_Address1), j, 1) = "," Then Exit For
                            Next j
                            If j = 0 Then j = 40

                            vLedger_Address2 = Microsoft.VisualBasic.Right(Trim(vLedger_Address1), Len(vLedger_Address1) - j)
                            vLedger_Address1 = Microsoft.VisualBasic.Left(Trim(vLedger_Address1), j - 1)

                        End If

                    End If

                    If Trim(vLedger_Address2) <> "" Then
                        If Len(vLedger_Address2) > 40 Then
                            For k = 40 To 1 Step -1
                                If Mid$(Trim(vLedger_Address1), k, 1) = " " Or Mid$(Trim(vLedger_Address1), k, 1) = "," Or Mid$(Trim(vLedger_Address1), k, 1) = "." Then Exit For
                            Next k
                            If k = 0 Then k = 40
                            vLedger_Address3 = Microsoft.VisualBasic.Right(Trim(vLedger_Address2), Len(vLedger_Address2) - k)
                            vLedger_Address2 = Microsoft.VisualBasic.Left(Trim(vLedger_Address2), k - 1)
                        End If
                    End If

                    If Trim(vLedger_Address3) <> "" Then
                        If Len(vLedger_Address3) > 70 Then
                            For l = 70 To 1 Step -1
                                If Mid$(Trim(vLedger_Address1), l, 1) = " " Or Mid$(Trim(vLedger_Address1), l, 1) = "," Or Mid$(Trim(vLedger_Address1), l, 1) = "." Then Exit For
                            Next l
                            If l = 0 Then l = 70
                            vLedger_Address4 = Microsoft.VisualBasic.Right(Trim(vLedger_Address3), Len(vLedger_Address3) - l)
                            vLedger_Address3 = Microsoft.VisualBasic.Left(Trim(vLedger_Address3), l - 1)
                        End If
                    End If

                    vLedger_GSTNo = Trim(xlWorkSheet.Cells(i, 2).value)


                    vLedger_StateCode = Microsoft.VisualBasic.Left(vLedger_GSTNo, 2)

                    vState_IdNo = Val(Common_Procedures.get_FieldValue(con, "State_Head", "State_Idno", "(State_Code = '" & Trim(vLedger_StateCode) & "')", , sqltr))

                    'vLedger_StateName = Trim(xlWorkSheet.Cells(i, 4).value)

                    'vState_IdNo = Common_Procedures.State_NameToIdNo(con, Trim(vLedger_StateName))



                    vLedger_PhoneNo = ""
                    vLedger_TinNo = ""
                    vLedger_CstNo = ""
                    vLedger_Type = ""
                    vPan_No = ""
                    vLedger_Emailid = ""
                    vLedger_FaxNo = ""
                    vLedger_MobileNo = ""
                    vContact_Person = ""
                    vPackingType_CompanyIdNo = 0
                    vLedger_AgentIdNo = 0
                    vTransport_Name = ""
                    vNote = ""
                    vMobileNo_Sms = ""
                    vBilling_Type = ""

                    CmdTo.CommandText = "Insert into ledger_head ( Ledger_IdNo        ,            Ledger_Name      ,            Sur_Name      ,            Ledger_MainName      ,            Ledger_AlaisName      ,              Area_IdNo      ,              AccountsGroup_IdNo      ,            Parent_Code      ,            Bill_Type      ,            Ledger_Address1      ,            Ledger_Address2      ,            Ledger_Address3      ,            Ledger_Address4      ,            Ledger_PhoneNo      ,            Ledger_TinNo      ,            Ledger_CstNo      ,            Ledger_Type      ,            Pan_No      ,            Partner_Proprietor      ,              Yarn_Comm_Percentage      ,              Yarn_Comm_Bag      ,              Cloth_Comm_Percentage      ,            Ledger_Emailid      ,            Ledger_FaxNo      ,            Ledger_MobileNo      ,            MobileNo_Frsms    ,            MobileNo_Sms      ,            Contact_Person      ,             PackingType_CompanyIdNo       ,              Ledger_AgentIdNo      ,            Note      ,            Show_In_All_Entry        ,            Billing_Type      ,            Sticker_Type      ,            Mrp_Perc      ,              Own_Loom_Status      ,              Freight_Loom      ,              NoOf_Looms      ,              Transport_IdNo      , Verified_Status,            Owner_Name      ,              Tds_Percentage     , Ledger_GSTinNo                 ,  Ledger_State_IdNo   ) " &
                                     "       Values (" & Str(Val(vLedger_IdNo)) & ", '" & Trim(vLedger_Name) & "', '" & Trim(vSur_Name) & "', '" & Trim(vLedger_MainName) & "', '" & Trim(vLedger_AlaisName) & "', " & Str(Val(vArea_IdNo)) & ", " & Str(Val(vAccountsGroup_IdNo)) & ", '" & Trim(vParent_Code) & "', '" & Trim(vBill_Type) & "', '" & Trim(vLedger_Address1) & "', '" & Trim(vLedger_Address2) & "', '" & Trim(vLedger_Address3) & "', '" & Trim(vLedger_Address4) & "', '" & Trim(vLedger_PhoneNo) & "', '" & Trim(vLedger_TinNo) & "', '" & Trim(vLedger_CstNo) & "', '" & Trim(vLedger_Type) & "', '" & Trim(vPan_No) & "', '" & Trim(vPartner_Proprietor) & "', " & Str(Val(vYarn_Comm_Percentage)) & ", " & Str(Val(vYarn_Comm_Bag)) & ", " & Str(Val(vCloth_Comm_Percentage)) & ", '" & Trim(vLedger_Emailid) & "', '" & Trim(vLedger_FaxNo) & "', '" & Trim(vLedger_MobileNo) & "', '" & Trim(vMobileNo_Sms) & "', '" & Trim(vMobileNo_Sms) & "', '" & Trim(vContact_Person) & "', " & Str(Val(vPackingType_CompanyIdNo)) & ", " & Str(Val(vLedger_AgentIdNo)) & ", '" & Trim(vNote) & "', " & Str(Val(vShow_In_All_Entry)) & ", '" & Trim(vBilling_Type) & "', '" & Trim(vSticker_Type) & "', '" & Trim(vMrp_Perc) & "', " & Str(Val(vOwn_Loom_Status)) & ", " & Str(Val(vFreight_Loom)) & ", " & Str(Val(vNoOf_Looms)) & ", " & Str(Val(vTransport_IdNo)) & ",       1        , '" & Trim(vOwner_Name) & "', " & Str(Val(vTds_Percentage)) & " , '" & Trim(vLedger_GSTNo) & "'  ," & Str(Val(vState_IdNo)) & " ) "
                    CmdTo.ExecuteNonQuery()

                    CmdTo.CommandText = "Insert into Ledger_AlaisHead ( Ledger_IdNo, Sl_No, Ledger_DisplayName, Ledger_Type, AccountsGroup_IdNo, Verified_Status ) Values (" & Str(Val(vLedger_IdNo)) & ",   1,     '" & Trim(vLedger_Name) & "',   '" & Trim(vLedger_Type) & "',  " & Str(Val(vAccountsGroup_IdNo)) & ",  1 )"
                    CmdTo.ExecuteNonQuery()

                End If

            Next i

            sqltr.Commit()

            CmdTo.Dispose()
            Dt1.Dispose()
            Da1.Dispose()

            xlWorkBook.Close(False, FileName)
            xlApp.Quit()

            ReleaseComObject(xlWorkSheet)
            ReleaseComObject(xlWorkBook)
            ReleaseComObject(xlApp)


            MessageBox.Show("Imported Successfully!!!", "FOR IMPORTING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception
            sqltr.Rollback()
            MessageBox.Show(ex.Message, "DOES NOT IMPORT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub



    Private Sub btn_Import_Master_Click_111(ByVal sender As System.Object, ByVal e As System.EventArgs)  '---- SANTHA EXPORTS
        Dim CmdFrm As New SqlClient.SqlCommand
        Dim CmdTo As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim vLedger_IdNo As Integer, vOldLID As Integer
        Dim vLedger_Name As String, vSur_Name As String, vLedger_MainName As String, vLedger_AlaisName As String, vLedger_StateName As String
        Dim vArea_IdNo As Integer, vAccountsGroup_IdNo As Integer, vState_IdNo As Integer, vParent_Code As String, vBill_Type As String
        Dim vLedger_Address1 As String, vLedger_Address2 As String, vLedger_Address3 As String, vLedger_Address4 As String, vLedger_PhoneNo As String
        Dim vLedger_TinNo As String, vLedger_CstNo As String, vLedger_Type As String, vPan_No As String
        Dim vLedger_Emailid As String, vLedger_FaxNo As String, vLedger_MobileNo As String, vContact_Person As String, vLedger_GSTNo As String
        Dim vPackingType_CompanyIdNo As Integer, vLedger_AgentIdNo As Integer, vTransport_Name As String, vNote As String
        Dim vMobileNo_Sms As String, vBilling_Type As String, vSticker_Type As String, vMrp_Perc As String
        Dim vLedNm As String
        Dim sqltr As SqlClient.SqlTransaction
        Dim cmd As New SqlClient.SqlCommand
        'Dim tr As SqlClient.SqlTransaction
        Dim da As New SqlClient.SqlDataAdapter

        Dim xlApp As Excel.Application
        Dim xlWorkBook As Excel.Workbook
        Dim xlWorkSheet As Excel.Worksheet

        Dim RowCnt As Long = 0
        Dim FileName As String = ""
        Dim NewCode As String = ""
        Dim ShfId As Integer = 0
        Dim ItmId As Integer = 0
        Dim j, k, l As Integer
        Dim Sn As Integer = 0
        Dim vSurNm As String = ""
        Dim vShow_In_All_Entry As Integer, vVerified_Status As Integer = 0
        Dim vTransport_IdNo As Integer, vNoOf_Looms As Integer
        Dim vFreight_Loom As Single
        Dim vOwn_Loom_Status As Integer
        Dim vTds_Percentage As Single
        Dim vOwner_Name As String
        Dim vPartner_Proprietor As String
        Dim vCloth_Comm_Meter As Single = 0, vCloth_Comm_Percentage As Single
        Dim vYarn_Comm_Bag As Single, vYarn_Comm_Percentage As Single
        Dim AccGrpAr() As String
        Dim Inc As Integer = 0
        Dim AccGrp1 As String = ""
        Dim AccGrp2 As String = ""
        Dim AccGrp3 As String = ""
        Dim AccGrp4 As String = ""

        Dim g As New Password
        g.ShowDialog()

        If Trim(UCase(Common_Procedures.Password_Input)) <> "TSIMP7417" Then
            MessageBox.Show("Invalid Password", "PASSWORD FAILED...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If




        CmdTo.Connection = con

        sqltr = con.BeginTransaction

        CmdTo.Transaction = sqltr

        Try

            OpenFileDialog1.ShowDialog()
            FileName = OpenFileDialog1.FileName

            If Not IO.File.Exists(FileName) Then
                MessageBox.Show(FileName & " File not found", "File not found...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub
            End If

            xlApp = New Excel.Application
            xlWorkBook = xlApp.Workbooks.Open(FileName)
            xlWorkSheet = xlWorkBook.Worksheets("sheet1")

            With xlWorkSheet
                RowCnt = .Range("A" & .Rows.Count).End(Excel.XlDirection.xlUp).Row
            End With

            'RowCnt = xlWorkSheet.UsedRange.Rows.Count

            If RowCnt <= 1 Then
                MessageBox.Show("Data not found", "Data not found...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub
            End If

            For i = 2 To RowCnt

                'On Error Resume Next
                vLedger_Address1 = ""
                vLedger_Address2 = ""
                vLedger_Address3 = ""
                vLedger_Address4 = ""

                vLedNm = UCase(Trim(xlWorkSheet.Cells(i, 2).value))
                vOldLID = Val(Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_IdNo", "(Sur_Name = '" & Trim(vLedNm) & "')", , sqltr))

                'If Trim(vLedNm) = "" Then
                '    GoTo loop1
                'End If

                If vOldLID = 0 Then

                    vLedger_IdNo = Common_Procedures.get_MaxIdNo(con, "ledger_head", "ledger_idno", "", sqltr)
                    If Val(vLedger_IdNo) < 100 Then
                        vLedger_IdNo = 101
                    End If

                    vLedger_Name = Replace(UCase(Trim(xlWorkSheet.Cells(i, 2).value)), "'", "")
                    vLedger_MainName = vLedger_Name
                    vSur_Name = Common_Procedures.Remove_NonCharacters(vLedger_Name)
                    vLedger_AlaisName = ""
                    vArea_IdNo = 0

                    vAccountsGroup_IdNo = 14
                    vParent_Code = "~14~11~"

                    vBill_Type = "BILL TO BILL"
                    ' vBill_Type = "BALANCE ONLY"

                    vLedger_Address1 = Trim(xlWorkSheet.Cells(i, 3).value)
                    vLedger_Address2 = ""
                    If Trim(vLedger_Address1) <> "" Then
                        If Len(vLedger_Address1) > 40 Then

                            For j = 40 To 1 Step -1
                                If Mid$(Trim(vLedger_Address1), j, 1) = " " Or Mid$(Trim(vLedger_Address1), j, 1) = "," Then Exit For
                            Next j
                            If j = 0 Then j = 40

                            vLedger_Address2 = Microsoft.VisualBasic.Right(Trim(vLedger_Address1), Len(vLedger_Address1) - j)
                            vLedger_Address1 = Microsoft.VisualBasic.Left(Trim(vLedger_Address1), j - 1)

                        End If

                    End If

                    If Trim(vLedger_Address2) <> "" Then
                        If Len(vLedger_Address2) > 40 Then
                            For k = 40 To 1 Step -1
                                If Mid$(Trim(vLedger_Address1), k, 1) = " " Or Mid$(Trim(vLedger_Address1), k, 1) = "," Or Mid$(Trim(vLedger_Address1), k, 1) = "." Then Exit For
                            Next k
                            If k = 0 Then k = 40
                            vLedger_Address3 = Microsoft.VisualBasic.Right(Trim(vLedger_Address2), Len(vLedger_Address2) - k)
                            vLedger_Address2 = Microsoft.VisualBasic.Left(Trim(vLedger_Address2), k - 1)
                        End If
                    End If

                    If Trim(vLedger_Address3) <> "" Then
                        If Len(vLedger_Address3) > 70 Then
                            For l = 70 To 1 Step -1
                                If Mid$(Trim(vLedger_Address1), l, 1) = " " Or Mid$(Trim(vLedger_Address1), l, 1) = "," Or Mid$(Trim(vLedger_Address1), l, 1) = "." Then Exit For
                            Next l
                            If l = 0 Then l = 70
                            vLedger_Address4 = Microsoft.VisualBasic.Right(Trim(vLedger_Address3), Len(vLedger_Address3) - l)
                            vLedger_Address3 = Microsoft.VisualBasic.Left(Trim(vLedger_Address3), l - 1)
                        End If
                    End If

                    vLedger_StateName = Trim(xlWorkSheet.Cells(i, 4).value)

                    vState_IdNo = Common_Procedures.State_NameToIdNo(con, Trim(vLedger_StateName))

                    vLedger_GSTNo = Trim(xlWorkSheet.Cells(i, 5).value)

                    vLedger_PhoneNo = ""
                    vLedger_TinNo = ""
                    vLedger_CstNo = ""
                    vLedger_Type = ""
                    vPan_No = ""
                    vLedger_Emailid = ""
                    vLedger_FaxNo = ""
                    vLedger_MobileNo = ""
                    vContact_Person = ""
                    vPackingType_CompanyIdNo = 0
                    vLedger_AgentIdNo = 0
                    vTransport_Name = ""
                    vNote = ""
                    vMobileNo_Sms = ""
                    vBilling_Type = ""

                    CmdTo.CommandText = "Insert into ledger_head ( Ledger_IdNo        ,            Ledger_Name      ,            Sur_Name      ,            Ledger_MainName      ,            Ledger_AlaisName      ,              Area_IdNo      ,              AccountsGroup_IdNo      ,            Parent_Code      ,            Bill_Type      ,            Ledger_Address1      ,            Ledger_Address2      ,            Ledger_Address3      ,            Ledger_Address4      ,            Ledger_PhoneNo      ,            Ledger_TinNo      ,            Ledger_CstNo      ,            Ledger_Type      ,            Pan_No      ,            Partner_Proprietor      ,              Yarn_Comm_Percentage      ,              Yarn_Comm_Bag      ,              Cloth_Comm_Percentage      ,            Ledger_Emailid      ,            Ledger_FaxNo      ,            Ledger_MobileNo      ,            MobileNo_Frsms    ,            MobileNo_Sms      ,            Contact_Person      ,             PackingType_CompanyIdNo       ,              Ledger_AgentIdNo      ,            Note      ,            Show_In_All_Entry        ,            Billing_Type      ,            Sticker_Type      ,            Mrp_Perc      ,              Own_Loom_Status      ,              Freight_Loom      ,              NoOf_Looms      ,              Transport_IdNo      , Verified_Status,            Owner_Name      ,              Tds_Percentage     , Ledger_GSTinNo                 ,  Ledger_State_IdNo   ) " &
                                     "       Values (" & Str(Val(vLedger_IdNo)) & ", '" & Trim(vLedger_Name) & "', '" & Trim(vSur_Name) & "', '" & Trim(vLedger_MainName) & "', '" & Trim(vLedger_AlaisName) & "', " & Str(Val(vArea_IdNo)) & ", " & Str(Val(vAccountsGroup_IdNo)) & ", '" & Trim(vParent_Code) & "', '" & Trim(vBill_Type) & "', '" & Trim(vLedger_Address1) & "', '" & Trim(vLedger_Address2) & "', '" & Trim(vLedger_Address3) & "', '" & Trim(vLedger_Address4) & "', '" & Trim(vLedger_PhoneNo) & "', '" & Trim(vLedger_TinNo) & "', '" & Trim(vLedger_CstNo) & "', '" & Trim(vLedger_Type) & "', '" & Trim(vPan_No) & "', '" & Trim(vPartner_Proprietor) & "', " & Str(Val(vYarn_Comm_Percentage)) & ", " & Str(Val(vYarn_Comm_Bag)) & ", " & Str(Val(vCloth_Comm_Percentage)) & ", '" & Trim(vLedger_Emailid) & "', '" & Trim(vLedger_FaxNo) & "', '" & Trim(vLedger_MobileNo) & "', '" & Trim(vMobileNo_Sms) & "', '" & Trim(vMobileNo_Sms) & "', '" & Trim(vContact_Person) & "', " & Str(Val(vPackingType_CompanyIdNo)) & ", " & Str(Val(vLedger_AgentIdNo)) & ", '" & Trim(vNote) & "', " & Str(Val(vShow_In_All_Entry)) & ", '" & Trim(vBilling_Type) & "', '" & Trim(vSticker_Type) & "', '" & Trim(vMrp_Perc) & "', " & Str(Val(vOwn_Loom_Status)) & ", " & Str(Val(vFreight_Loom)) & ", " & Str(Val(vNoOf_Looms)) & ", " & Str(Val(vTransport_IdNo)) & ",       1        , '" & Trim(vOwner_Name) & "', " & Str(Val(vTds_Percentage)) & " , '" & Trim(vLedger_GSTNo) & "'  ," & Str(Val(vState_IdNo)) & " ) "
                    CmdTo.ExecuteNonQuery()

                    CmdTo.CommandText = "Insert into Ledger_AlaisHead ( Ledger_IdNo, Sl_No, Ledger_DisplayName, Ledger_Type, AccountsGroup_IdNo, Verified_Status ) Values (" & Str(Val(vLedger_IdNo)) & ",   1,     '" & Trim(vLedger_Name) & "',   '" & Trim(vLedger_Type) & "',  " & Str(Val(vAccountsGroup_IdNo)) & ",  1 )"
                    CmdTo.ExecuteNonQuery()

                End If

            Next i


            CmdTo.Dispose()
            Dt1.Dispose()
            Da1.Dispose()

            xlWorkBook.Close(False, FileName)
            xlApp.Quit()

            ReleaseComObject(xlWorkSheet)
            ReleaseComObject(xlWorkBook)
            ReleaseComObject(xlApp)


            MessageBox.Show("Imported Successfully!!!", "FOR IMPORTING...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT IMPORT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub ReleaseComObject(ByVal obj As Object)
        Try
            System.Runtime.InteropServices.Marshal.ReleaseComObject(obj)
            obj = Nothing
        Catch ex As Exception
            obj = Nothing
        End Try
    End Sub
    Private Sub txt_Freight_Pavu_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Freight_Pavu.KeyDown
        On Error Resume Next
        If e.KeyValue = 38 Then
            pnl_Freight_Charge_Details.Visible = False
            grp_Back.Enabled = True
            txt_Name.Focus()
        End If
        If e.KeyValue = 40 Then

            If dgv_Freight_Charge_Details.RowCount >= 0 Then
                dgv_Freight_Charge_Details.Focus()
                dgv_Freight_Charge_Details.CurrentCell = dgv_Freight_Charge_Details.Rows(0).Cells(1)
            End If

        End If
    End Sub

    Private Sub txt_Freight_Pavu_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Freight_Pavu.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
        If Asc(e.KeyChar) = 13 Then

            If dgv_Freight_Charge_Details.RowCount >= 0 Then
                dgv_Freight_Charge_Details.Focus()
                dgv_Freight_Charge_Details.CurrentCell = dgv_Freight_Charge_Details.Rows(0).Cells(1)
            End If

        End If
    End Sub

    Private Sub btn_Close_Freight_Charge_Details_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Freight_Charge_Details.Click
        pnl_Freight_Charge_Details.Visible = False
        grp_Back.Enabled = True
        txt_Name.Focus()
    End Sub

    Private Sub btn_Freight_Charge_Details_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Freight_Charge_Details.Click
        grp_Back.Enabled = False
        pnl_Freight_Charge_Details.Visible = True
        pnl_Freight_Charge_Details.BringToFront()
        txt_Freight_Pavu.Focus()
    End Sub
    Private Sub dgv_Freight_Charge_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Freight_Charge_Details.EditingControlShowing
        dgtxt_FreihtChargeDetails = CType(dgv_Freight_Charge_Details.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgtxt_FreihtChargeDetails_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_FreihtChargeDetails.Enter
        Try
            dgv_ActiveCtrl_Name = dgv_Freight_Charge_Details.Name
            dgv_Freight_Charge_Details.EditingControl.BackColor = Color.Lime
            dgv_Freight_Charge_Details.EditingControl.ForeColor = Color.Blue
            dgv_Freight_Charge_Details.SelectAll()
        Catch ex As Exception
            '--
        End Try
    End Sub

    Private Sub dgtxt_FreihtChargeDetails_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_FreihtChargeDetails.KeyDown
        Try
            With dgv_Freight_Charge_Details
                vcbo_KeyDwnVal = e.KeyValue
                If .Visible Then
                    If e.KeyValue = Keys.Delete Then

                        'If .CurrentCell.ColumnIndex = 1 Or .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3 Then
                        '    If Trim(UCase(cbo_Type.Text)) = "DIRECT" Or Trim(UCase(cbo_Type.Text)) = "RECEIPT" Then
                        '        e.Handled = True
                        '        e.SuppressKeyPress = True
                        '    End If
                        'End If

                    End If
                End If
            End With

        Catch ex As Exception
            '--
        End Try

    End Sub

    Private Sub dgtxt_FreihtChargeDetails_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_FreihtChargeDetails.KeyPress
        Try
            With dgv_Freight_Charge_Details
                If .Visible Then

                    If .CurrentCell.ColumnIndex = 1 Or .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3 Then

                        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                            e.Handled = True
                        End If

                    End If

                End If
            End With
        Catch ex As Exception
            '---
        End Try


    End Sub

    Private Sub dgtxt_FreihtChargeDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_FreihtChargeDetails.KeyUp
        Try
            If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
                dgv_Freight_Charge_Details_KeyUp(sender, e)
            End If
        Catch ex As Exception
            '---
        End Try
    End Sub

    Private Sub dgv_Freight_Charge_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Freight_Charge_Details.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
    End Sub

    Private Sub dgv_Freight_Charge_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Freight_Charge_Details.KeyUp
        Dim n As Integer

        Try
            If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
                With dgv_Freight_Charge_Details

                    n = .CurrentRow.Index

                    If n = .Rows.Count - 1 Then
                        For i = 0 To .ColumnCount - 1
                            .Rows(n).Cells(i).Value = ""
                        Next

                    Else
                        .Rows.RemoveAt(n)

                    End If

                    ' Total_DyesChemicalCalculation()

                End With

            End If

        Catch ex As Exception
            '---
        End Try


    End Sub

    Private Sub dgv_Freight_Charge_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Freight_Charge_Details.LostFocus
        On Error Resume Next
        dgv_Freight_Charge_Details.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_Freight_Charge_Details_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Freight_Charge_Details.RowsAdded
        Dim n As Integer = 0
        Try
            If FrmLdSTS = True Then Exit Sub
            With dgv_Freight_Charge_Details
                n = .RowCount
                .Rows(n - 1).Cells(0).Value = Val(n)
            End With
        Catch ex As Exception
            '---
        End Try

    End Sub
    Private Sub dgv_Freight_Charge_Details_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Freight_Charge_Details.CellEndEdit
        dgv_Freight_Charge_Details_CellLeave(sender, e)
    End Sub

    Private Sub dgv_Freight_Charge_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Freight_Charge_Details.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim Dt4 As New DataTable
        Dim Dt5 As New DataTable


        With dgv_Freight_Charge_Details
            dgv_ActiveCtrl_Name = .Name

            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If



        End With

    End Sub

    Private Sub dgv_Freight_Charge_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Freight_Charge_Details.CellLeave
        With dgv_Freight_Charge_Details
            If .CurrentCell.ColumnIndex = 1 Or .CurrentCell.ColumnIndex = 2 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.000")
                Else
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
                End If
            End If
            If .CurrentCell.ColumnIndex = 3 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                Else
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
                End If
            End If
        End With
    End Sub

    Private Sub btn_PanAddress_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_PanAddress.Click
        grp_Back.Enabled = False
        pnl_PanAddress.Visible = True
        pnl_PanAddress.BringToFront()
        ' pnl_BobinReelDetails.Enabled = True
        txt_PanAddress1.Focus()

    End Sub

    Private Sub txt_PanAddress4_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_PanAddress4.KeyDown
        On Error Resume Next
        If e.KeyValue = 38 Then
            txt_PanAddress3.Focus()
        End If
        If e.KeyValue = 40 Then

            pnl_PanAddress.Visible = False
            grp_Back.Enabled = True
            txt_OwnerName.Focus()
        End If
    End Sub

    Private Sub txt_PanAddress4_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_PanAddress4.KeyPress
        If Asc(e.KeyChar) = 13 Then

            pnl_PanAddress.Visible = False
            grp_Back.Enabled = True
            txt_OwnerName.Focus()
        End If
    End Sub

    Private Sub txt_PanAddress1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_PanAddress1.KeyDown
        On Error Resume Next
        If e.KeyValue = 40 Then
            txt_PanAddress2.Focus()
        End If
        If e.KeyValue = 38 Then

            pnl_PanAddress.Visible = False
            grp_Back.Enabled = True
            txt_PanNo.Focus()
        End If
    End Sub

    Private Sub btn_Close_PanAddress_Details_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_PanAddress_Details.Click
        pnl_PanAddress.Visible = False
        grp_Back.Enabled = True
        If txt_OwnerName.Enabled And txt_OwnerName.Visible Then txt_OwnerName.Focus()
    End Sub

    Private Sub txt_PanNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_PanNo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If Trim(UCase(vLedType)) = "WEAVER" Then
                btn_PanAddress_Click(sender, e)
            Else
                txt_OwnerName.Focus()
            End If
        End If
    End Sub

    'Private Sub txt_TamilName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_TamilName.KeyDown
    '    If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
    '    If e.KeyValue = 40 Then
    '        txt_PanNo.Focus()
    '    End If
    'End Sub

    'Private Sub txt_TamilName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_TamilName.KeyPress
    '    If Asc(e.KeyChar) = 13 Then
    '        txt_PanNo.Focus()
    '    End If
    'End Sub

    Private Sub txt_FrgtLoom_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Freight_per_Loom.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_TdsPerc.Focus()
        End If
    End Sub

    Private Sub btn_Weaver_Wages_Details_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Weaver_Wages_Details.Click
        grp_Back.Enabled = False
        pnl_Wages_Charge_Details.Visible = True
        cbo_Grid_cloth_name.Focus()
    End Sub

    Private Sub btn_Close_Weaver_Wages_Charge_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Weaver_Wages_Charge.Click
        pnl_Wages_Charge_Details.Visible = False
        grp_Back.Enabled = True
        txt_Name.Focus()
    End Sub



    Private Sub dgv_Wages_Charge_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Wages_Charge_Details.CellEnter

        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim Dt4 As New DataTable
        Dim Dt5 As New DataTable

        Dim Rect As Rectangle

        With dgv_Wages_Charge_Details

            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If

            If e.ColumnIndex = 1 Then

                If cbo_Grid_cloth_name.Visible = False Or Val(cbo_Grid_cloth_name.Tag) <> e.RowIndex Then

                    cbo_Grid_cloth_name.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Cloth_Name from Cloth_Head order by Cloth_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Grid_cloth_name.DataSource = Dt1
                    cbo_Grid_cloth_name.DisplayMember = "Cloth_Name"

                    Rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_cloth_name.Left = .Left + Rect.Left
                    cbo_Grid_cloth_name.Top = .Top + Rect.Top

                    cbo_Grid_cloth_name.Width = Rect.Width
                    cbo_Grid_cloth_name.Height = Rect.Height
                    cbo_Grid_cloth_name.Text = .CurrentCell.Value

                    cbo_Grid_cloth_name.Tag = Val(e.RowIndex)
                    cbo_Grid_cloth_name.Visible = True

                    cbo_Grid_cloth_name.BringToFront()
                    cbo_Grid_cloth_name.Focus()

                End If
            End If

            'With dgv_Wages_Charge_Details
            '    dgv_ActiveCtrl_Name = .Name

            '    If Val(.CurrentRow.Cells(0).Value) = 0 Then
            '        .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            '    End If



        End With
    End Sub

    Private Sub dgv_Wages_Charge_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Wages_Charge_Details.CellLeave

        With dgv_Wages_Charge_Details
            If .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 4 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value)
                Else
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
                End If
            End If
            If .CurrentCell.ColumnIndex = 5 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value)
                Else
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
                End If
            End If
        End With
    End Sub



    Private Sub dgv_Wages_Charge_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Wages_Charge_Details.EditingControlShowing
        dgtxt_WeaverWagesDetails = CType(dgv_Wages_Charge_Details.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgtxt_WeaverWagesDetails_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_WeaverWagesDetails.Enter
        Try
            dgv_ActiveCtrl_Name = dgv_Wages_Charge_Details.Name
            dgv_Wages_Charge_Details.EditingControl.BackColor = Color.Lime
            dgv_Wages_Charge_Details.EditingControl.ForeColor = Color.Blue
            dgv_Wages_Charge_Details.SelectAll()
        Catch ex As Exception
            '--
        End Try
    End Sub

    Private Sub dgtxt_WeaverWagesDetails_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_WeaverWagesDetails.KeyDown
        Try
            With dgv_Wages_Charge_Details
                vcbo_KeyDwnVal = e.KeyValue
                If .Visible Then
                    If e.KeyValue = Keys.Delete Then

                        'If .CurrentCell.ColumnIndex = 1 Or .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3 Then
                        '    If Trim(UCase(cbo_Type.Text)) = "DIRECT" Or Trim(UCase(cbo_Type.Text)) = "RECEIPT" Then
                        '        e.Handled = True
                        '        e.SuppressKeyPress = True
                        '    End If
                        'End If

                    End If
                End If
            End With

        Catch ex As Exception
            '--
        End Try
    End Sub

    Private Sub dgtxt_WeaverWagesDetails_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_WeaverWagesDetails.KeyPress
        Try
            With dgv_Wages_Charge_Details
                If .Visible Then

                    If .CurrentCell.ColumnIndex = 1 Or .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 10 Then

                        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                            e.Handled = True
                        End If

                    End If

                End If
            End With
        Catch ex As Exception
            '---
        End Try
    End Sub

    Private Sub dgtxt_WeaverWagesDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_WeaverWagesDetails.KeyUp
        Try
            If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
                dgv_Wages_Charge_Details_KeyUp(sender, e)
            End If
        Catch ex As Exception
            '---
        End Try
    End Sub

    Private Sub dgv_Wages_Charge_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Wages_Charge_Details.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
    End Sub


    Private Sub dgv_Wages_Charge_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Wages_Charge_Details.KeyUp
        Dim n As Integer

        Try
            If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
                With dgv_Wages_Charge_Details

                    n = .CurrentRow.Index

                    If n = .Rows.Count - 1 Then
                        For i = 0 To .ColumnCount - 1
                            .Rows(n).Cells(i).Value = ""
                        Next

                    Else
                        .Rows.RemoveAt(n)

                    End If

                    ' Total_DyesChemicalCalculation()

                End With

            End If

        Catch ex As Exception
            '---
        End Try

    End Sub

    Private Sub dgv_Wages_Charge_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Wages_Charge_Details.LostFocus
        On Error Resume Next
        dgv_Wages_Charge_Details.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_Wages_Charge_Details_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Wages_Charge_Details.RowsAdded
        Dim n As Integer = 0
        Try
            If FrmLdSTS = True Then Exit Sub
            With dgv_Wages_Charge_Details
                n = .RowCount
                .Rows(n - 1).Cells(0).Value = Val(n)
            End With
        Catch ex As Exception
            '---
        End Try

    End Sub

    Private Sub cbo_Grid_cloth_name_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_cloth_name.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_head", "Cloth_Name", "", "(Cloth_idno = 0)")
    End Sub

    Private Sub cbo_Grid_cloth_name_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_cloth_name.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_cloth_name, Nothing, Nothing, "Cloth_head", "Cloth_Name", "", "(Cloth_idno = 0)")
    End Sub

    Private Sub cbo_Grid_cloth_name_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_cloth_name.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_cloth_name, Nothing, "Cloth_head", "Cloth_Name", "", "(Cloth_idno = 0)")

        If Asc(e.KeyChar) = 13 Then

            With dgv_Wages_Charge_Details
                If .CurrentCell.RowIndex = .Rows.Count - 1 And Trim(.Rows(.CurrentCell.RowIndex).Cells(1).Value) = "" Then
                    pnl_Wages_Charge_Details.Visible = False
                    grp_Back.Enabled = True
                    txt_Name.Focus()

                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

                End If
            End With
        End If
    End Sub


    Private Sub cbo_Grid_cloth_name_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_cloth_name.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Cloth_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_cloth_name.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_Grid_cloth_name_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_cloth_name.TextChanged
        Try
            If cbo_Grid_cloth_name.Visible Then
                With dgv_Wages_Charge_Details
                    If Val(cbo_Grid_cloth_name.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(1).Value = Trim(cbo_Grid_cloth_name.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Weaver_LoomType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Weaver_LoomType.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")
    End Sub

    Private Sub cbo_Weaver_LoomType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Weaver_LoomType.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Weaver_LoomType, cbo_State, txt_Name, "", "", "", "")
    End Sub

    Private Sub cbo_Weaver_LoomType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Weaver_LoomType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Weaver_LoomType, txt_Name, "", "", "", "")
    End Sub

    Private Sub cbo_Sizing_CompanyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Sizing_To_CompanyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, SizTo_DbName & "..Company_Head", "Company_Name", "", "(Company_idno = 0)")
    End Sub

    Private Sub cbo_Sizing_CompanyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Sizing_To_CompanyName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Sizing_To_CompanyName, txt_TdsPerc, Nothing, SizTo_DbName & "..Company_Head", "Company_Name", "", "(Company_idno = 0)")

        If (e.KeyValue = 40 And cbo_Sizing_To_CompanyName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                txt_Name.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_Sizing_CompanyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Sizing_To_CompanyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Sizing_To_CompanyName, Nothing, SizTo_DbName & "..Company_Head", "Company_Name", "", "(Company_idno = 0)")
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                txt_Name.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_Sizing_VendorName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Sizing_To_VendorName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, SizTo_DbName & "..Vendor_Head", "Vendor_Name", "", "(Vendor_idno = 0)")
    End Sub

    Private Sub cbo_Sizing_VendorName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Sizing_To_VendorName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Sizing_To_VendorName, txt_TdsPerc, Nothing, SizTo_DbName & "..Vendor_Head", "Vendor_Name", "", "(Vendor_idno = 0)")

        If (e.KeyValue = 40 And cbo_Sizing_To_VendorName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If Txt_Remarks.Visible = True And Txt_Remarks.Enabled = True Then
                Txt_Remarks.Focus()
            ElseIf txt_vehicleNo.Visible = True Then
                txt_vehicleNo.Focus()
            ElseIf txt_Production_per_Day.Visible = True Then
                txt_Production_per_Day.Focus()
            Else
                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    save_record()
                Else
                    txt_Name.Focus()
                End If
            End If


        End If
    End Sub

    Private Sub cbo_Sizing_VendorName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Sizing_To_VendorName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Sizing_To_VendorName, Nothing, SizTo_DbName & "..Vendor_Head", "Vendor_Name", "", "(Vendor_idno = 0)")
        If Asc(e.KeyChar) = 13 Then
            If Txt_Remarks.Visible = True And Txt_Remarks.Enabled = True Then
                Txt_Remarks.Focus()
            ElseIf txt_vehicleNo.Visible = True Then
                txt_vehicleNo.Focus()
            ElseIf txt_Production_per_Day.Visible = True Then
                txt_Production_per_Day.Focus()
            Else
                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    save_record()
                Else
                    txt_Name.Focus()
                End If
            End If
        End If
    End Sub

    Private Sub btn_PavuYarn_Stock_MinMax_Level_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_PavuYarn_Stock_MinMax_Level.Click
        pnl_PYStockLimit.Visible = True
        grp_Back.Enabled = False
        txt_PavuMin.Focus()
    End Sub

    Private Sub btn_PYStockLmt_Close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_PYStockLmt_Close.Click
        pnl_PYStockLimit.Visible = False
        grp_Back.Enabled = True
        txt_Name.Focus()
    End Sub

    Private Sub txt_PavuMin_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_PavuMin.KeyDown
        If Trim(UCase(vLedType)) = "WEAVER" Then
            If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
            If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_PavuMin_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_PavuMin.KeyPress
        If Trim(UCase(vLedType)) = "WEAVER" Then
            If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_PavuMax_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_PavuMax.KeyDown
        If Trim(UCase(vLedType)) = "WEAVER" Then
            If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
            If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_PavuMax_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_PavuMax.KeyPress
        If Trim(UCase(vLedType)) = "WEAVER" Then
            If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_YarnMin_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_YarnMin.KeyDown
        If Trim(UCase(vLedType)) = "WEAVER" Then
            If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
            If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_YarnMin_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_YarnMin.KeyPress
        If Trim(UCase(vLedType)) = "WEAVER" Then
            If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_YarnMax_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_YarnMax.KeyDown
        If Trim(UCase(vLedType)) = "WEAVER" Then
            If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
            If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_YarnMax_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_YarnMax.KeyPress
        If Trim(UCase(vLedType)) = "WEAVER" Then
            If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_freight_Bundle_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_freight_Bundle.KeyDown

        On Error Resume Next

        If e.KeyValue = 38 Then
            ' pnl_Freight_Charge_Details.Visible = False
            'grp_Back.Enabled = True
            txt_Freight_Pavu.Focus()
        End If

        If e.KeyValue = 40 Then
            If dgv_Freight_Charge_Details.RowCount >= 0 Then
                dgv_Freight_Charge_Details.Focus()
                dgv_Freight_Charge_Details.CurrentCell = dgv_Freight_Charge_Details.Rows(0).Cells(1)
            End If
        End If
    End Sub

    Private Sub txt_freight_Bundle_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_freight_Bundle.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If

        If Asc(e.KeyChar) = 13 Then
            If dgv_Freight_Charge_Details.RowCount >= 0 Then
                dgv_Freight_Charge_Details.Focus()
                dgv_Freight_Charge_Details.CurrentCell = dgv_Freight_Charge_Details.Rows(0).Cells(1)
            End If
        End If
    End Sub

    Private Sub btn_PrintClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_PrintClose.Click
        pnl_PrintSetup.Visible = False
        grp_Back.Enabled = True
        If txt_Name.Visible And txt_Name.Enabled Then txt_Name.Focus()
    End Sub

    Private Sub cbo_PaperOrientation_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PaperOrientation.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")
    End Sub

    Private Sub cbo_PaperOrientation_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PaperOrientation.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PaperOrientation, txt_LeftToAdds, chk_FromAddress, "", "", "", "")
    End Sub

    Private Sub cbo_PaperOrientation_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PaperOrientation.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PaperOrientation, chk_FromAddress, "", "", "", "")
    End Sub

    Private Sub chk_FromAddress_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles chk_FromAddress.Click
        If chk_FromAddress.Checked = True Then
            txt_LeftFromAdds.Enabled = True
            txt_TopFromAdds.Enabled = True
        Else
            txt_LeftFromAdds.Enabled = False
            txt_TopFromAdds.Enabled = False
        End If
    End Sub

    Private Sub txt_LeftFromAdds_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_LeftFromAdds.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_LeftToAdds_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_LeftToAdds.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_TopFromAdds_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_TopFromAdds.KeyDown
        If e.KeyCode = 38 Then e.Handled = True : SendKeys.Send("+{TAB}")
        If e.KeyCode = 40 Then e.Handled = True : btn_PrintClose_Click(sender, e)
    End Sub

    Private Sub txt_TopFromAdds_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_TopFromAdds.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then btn_PrintClose_Click(sender, e)
    End Sub

    Private Sub txt_TOPToAdds_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_TOPToAdds.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub btn_Print_Address_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_Address.Click
        PrntFormat1_STS = True
        Printing_LedgerAddress_Print()
    End Sub

    Private Sub btn_Cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Cancel.Click
        Call btn_PrintClose_Click(sender, e)
    End Sub

    Private Sub cbo_FromAddress_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_FromAddress.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Company_Head", "Company_Name", "", "(Company_IdNo = 0)")
    End Sub

    Private Sub cbo_FromAddress_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_FromAddress.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_FromAddress, Nothing, cbo_PaperOrientation, "Company_Head", "Company_Name", "", "(Company_IdNo = 0)")
    End Sub

    Private Sub cbo_FromAddress_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_FromAddress.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_FromAddress, cbo_PaperOrientation, "Company_Head", "Company_Name", "", "(Company_IdNo = 0)")
    End Sub

    Private Sub cbo_LedgerGroup_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_LedgerGroup.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_LedgerGroup_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_LedgerGroup.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_LedgerGroup, txt_LegalName_Business, txt_AlaisName, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_IdNo = 0)")
        'If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        'If e.KeyValue = 40 Then
        '    If Trim(UCase(vLedType)) = "WEAVER" Then
        '        txt_Address1.Focus()
        '    Else
        '        cbo_BillType.Focus()

        '    End If
        'End If

    End Sub

    Private Sub cbo_LedgerGroup_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_LedgerGroup.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_LedgerGroup, txt_AlaisName, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_IdNo = 0)")

        'If Asc(e.KeyChar) = 13 Then
        '    If Trim(UCase(vLedType)) = "WEAVER" Then
        '        txt_Address1.Focus()
        '    Else
        '        cbo_BillType.Focus()

        '    End If
        'End If
    End Sub

    Private Sub txt_creditLimitDays_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_creditLimitDays.KeyDown
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then
            If Trim(UCase(vLedType)) = "WEAVER" Then
                txt_NoofLoom.Focus()
            ElseIf Trim(UCase(vLedType)) = "SIZING" Then
                txt_TdsPerc.Focus()
            ElseIf cbo_Transfer_StockTo.Visible = True Then

                cbo_Transfer_StockTo.Focus()
            Else
                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    save_record()
                Else
                    txt_Name.Focus()
                End If

            End If
        End If
    End Sub

    Private Sub txt_creditLimitDays_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_creditLimitDays.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If Trim(UCase(vLedType)) = "WEAVER" Then
                txt_NoofLoom.Focus()

            ElseIf Trim(UCase(vLedType)) = "SIZING" Then
                txt_TdsPerc.Focus()

            ElseIf cbo_Transfer_StockTo.Visible = True Then
                cbo_Transfer_StockTo.Focus()

            Else
                Txt_Remarks.Focus()
                'If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                '    save_record()
                'Else
                '    txt_Name.Focus()
                'End If

            End If
        End If
    End Sub



    Private Sub txt_CreditLimit_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_CreditLimit.KeyDown
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then
            txt_creditLimitDays.Focus()
        End If

    End Sub

    Private Sub txt_CreditLimit_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_CreditLimit.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_creditLimitDays.Focus()
        End If
    End Sub


    Private Sub txt_InsuranceNo_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles txt_InsuranceNo.KeyDown

        If Common_Procedures.settings.CustomerCode = "1309" Then

            If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
            If e.KeyValue = 40 Then save_record()

        Else
            If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
            If e.KeyValue = 40 Then SendKeys.Send("{TAB}")
        End If



    End Sub

    Private Sub txt_InsuranceNo_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txt_InsuranceNo.KeyPress
        If Common_Procedures.settings.CustomerCode = "1309" Then
            If Asc(e.KeyChar) = 13 Then
                save_record()
            End If

        Else
            SendKeys.Send("{TAB}")

        End If
    End Sub

    Private Sub txt_OwnerName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_OwnerName.KeyDown
        If e.KeyValue = 38 Then e.Handled = True : e.SuppressKeyPress = True : SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then
            e.Handled = True : e.SuppressKeyPress = True

            If cbo_marketting_Exec_Name.Visible Then
                cbo_marketting_Exec_Name.Focus()
            ElseIf txt_InsuranceNo.Visible Then
                txt_InsuranceNo.Focus()

            ElseIf txt_NoofLoom.Visible = True Then
                txt_NoofLoom.Focus()

            ElseIf txt_Freight_per_Loom.Visible = True Then
                txt_Freight_per_Loom.Focus()

            ElseIf txt_TdsPerc.Visible = True Then
                txt_TdsPerc.Focus()

            ElseIf txt_CreditLimit.Visible = True Then
                txt_CreditLimit.Focus()

            ElseIf cbo_Sizing_To_CompanyName.Visible = True Then
                cbo_Sizing_To_CompanyName.Focus()

            ElseIf cbo_Sizing_To_VendorName.Visible = True Then
                cbo_Sizing_To_VendorName.Focus()

            ElseIf cbo_Transfer_StockTo.Visible = True Then
                cbo_Transfer_StockTo.Focus()

            ElseIf txt_contact_person_name.Visible = True Then
                txt_contact_person_name.Focus()


            ElseIf Txt_Remarks.Visible = True Then
                Txt_Remarks.Focus()

            ElseIf txt_vehicleNo.Visible = True Then
                txt_vehicleNo.Focus()

            Else

                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    save_record()
                Else
                    txt_Name.Focus()
                End If

            End If
        End If
    End Sub

    Private Sub txt_OwnerName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_OwnerName.KeyPress
        If Asc(e.KeyChar) = 13 Then

            If cbo_marketting_Exec_Name.Visible Then
                cbo_marketting_Exec_Name.Focus()
            ElseIf txt_InsuranceNo.Visible Then
                txt_InsuranceNo.Focus()

            ElseIf txt_NoofLoom.Visible = True Then
                txt_NoofLoom.Focus()

            ElseIf txt_Freight_per_Loom.Visible = True Then
                txt_Freight_per_Loom.Focus()

            ElseIf txt_TdsPerc.Visible = True Then
                txt_TdsPerc.Focus()

            ElseIf txt_CreditLimit.Visible = True Then
                txt_CreditLimit.Focus()

            ElseIf cbo_Sizing_To_CompanyName.Visible = True Then
                cbo_Sizing_To_CompanyName.Focus()

            ElseIf cbo_Sizing_To_VendorName.Visible = True Then
                cbo_Sizing_To_VendorName.Focus()

            ElseIf cbo_Transfer_StockTo.Visible = True Then
                cbo_Transfer_StockTo.Focus()


            ElseIf txt_contact_person_name.Visible = True Then
                txt_contact_person_name.Focus()


            ElseIf Txt_Remarks.Visible = True Then
                Txt_Remarks.Focus()

            ElseIf txt_vehicleNo.Visible = True Then
                txt_vehicleNo.Focus()


            Else

                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    save_record()
                Else
                    txt_Name.Focus()
                End If

            End If

        End If

    End Sub


    Private Sub txt_Production_per_Day_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles txt_Production_per_Day.KeyDown
        If (e.KeyValue = 38) Or (e.Control = True And e.KeyValue = 38) Then
            cbo_Sizing_To_VendorName.Focus()



        End If
        If (e.KeyValue = 40) Or (e.Control = True And e.KeyValue = 40) Then

            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                txt_Name.Focus()
            End If


        End If
    End Sub

    Private Sub txt_Production_per_Day_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Production_per_Day.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                txt_Name.Focus()
            End If
        End If
    End Sub
    Private Sub txt_Distance_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Distance.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_GSTIN_No_TextChanged(sender As Object, e As EventArgs) Handles txt_GSTIN_No.TextChanged
        chk_GSTIN_Verified.Checked = False
        'If Len(Trim(txt_GSTIN_No.Text)) <> 15 Then
        '    txt_GSTIN_No.BackColor = Color.Red
        '    txt_GSTIN_No.ForeColor = Color.Yellow
        'End If
    End Sub

    Private Sub chk_GSTIN_Verified_CheckedChanged(sender As Object, e As EventArgs) Handles chk_GSTIN_Verified.CheckedChanged
        'If chk_GSTIN_Verified.Checked Then
        '    txt_GSTIN_No.BackColor = Color.White
        '    txt_GSTIN_No.ForeColor = Color.Black
        'Else
        '    txt_GSTIN_No.BackColor = Color.Red
        '    txt_GSTIN_No.ForeColor = Color.Yellow
        'End If
    End Sub

    Private Sub btn_Verify_Click(sender As Object, e As EventArgs) Handles btn_Verify.Click

        Verify_GSTIN(False)

        '--------------------------

        'Dim GSTIN As String = txt_GSTIN_No.Text
        '     Dim TxnResp As TxnRespWithObj(Of GSTINDetail) = Await EWBAPI.GetGSTNDetailAsync(EwbSession, GSTIN)

        'If TxnResp.IsSuccess Then

        '    '      Dim rawResp As String = JsonConvert.SerializeObject(TxnResp.RespObj)

        '    '   Dim jss As New JavaScriptSerializer()
        '    Dim dict As Dictionary(Of String, String) = jss.Deserialize(Of Dictionary(Of String, String))(rawResp)

        '    If UCase(Trim(dict("tradeName"))) = UCase(Trim(txt_Name.Text)) Then
        '        txt_GSTIN_No.BackColor = Color.White
        '        txt_GSTIN_No.ForeColor = Color.Black
        '        chk_GSTIN_Verified.Checked = True
        '        MessageBox.Show("GSTIN and LEDGER NAME Matched" & Chr(13) & "Verified Successfully", "GSTIN NO VERIFICATION", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        '    Else

        '        MessageBox.Show("GSTIN and LEDGER NAME does not Matched" & Chr(13) & "The Trade Name for this GSTIN is """ & UCase(Trim(dict("tradeName"))) & """" & Chr(13) & "Please Verify manually", "GSTIN NO VERIFICATION", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation)

        '    End If

        'Else

        '    MessageBox.Show("Invalid GSTIN " & Chr(13) & "GSTIN Number does not exists - Verification Failed", "GSTIN NO VERIFICATION", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        'End If

    End Sub


    Private Function Verify_GSTIN(ByVal vSAVING_STS As Boolean) As Boolean
        Dim v_Name As String = ""
        Dim v_LegalName_Business As String = ""
        Dim v_Address1 As String = ""
        Dim v_Address2 As String = ""
        Dim v_Address3 As String = ""
        Dim v_Address4 As String = ""
        Dim v_city As String = ""
        Dim v_StateName As String = ""
        Dim v_pincode As String = ""
        Dim v_ERRMSG_SHOWN_STS As Boolean = False

        txt_GSTIN_No.Text = Trim(txt_GSTIN_No.Text)
        txt_GSTIN_No.Text = Replace(Trim(txt_GSTIN_No.Text), "  ", "")
        txt_GSTIN_No.Text = Replace(Trim(txt_GSTIN_No.Text), " ", "")

        chk_GSTIN_Verified.Checked = False

        GSTIN_Search.SEARCHGSTIN(Trim(txt_GSTIN_No.Text), v_Name, v_LegalName_Business, v_Address1, v_Address2, v_Address3, v_Address4, v_city, v_StateName, v_pincode, v_ERRMSG_SHOWN_STS, vSAVING_STS)

        If Trim(v_Name) <> "" Then

            txt_GSTIN_No.BackColor = Color.White
            txt_GSTIN_No.ForeColor = Color.Black
            chk_GSTIN_Verified.Checked = True

            If vSAVING_STS = True Then
                chk_GSTIN_Verified.Checked = True

            ElseIf Trim(txt_Name.Text) = "" Then
                txt_Name.Text = v_Name
                txt_LegalName_Business.Text = v_LegalName_Business
                txt_Address1.Text = v_Address1
                txt_Address2.Text = v_Address2
                txt_Address3.Text = v_Address3
                txt_Address4.Text = v_Address4
                txt_city.Text = v_city
                cbo_State.Text = v_StateName
                txt_pincode.Text = v_pincode

                MessageBox.Show("GSTIN Verified Successfully", "FOR GSTIN VERIFICATION....", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)


            ElseIf UCase(Trim(v_Name)) = UCase(Trim(txt_Name.Text)) Then
                MessageBox.Show("GSTIN and LEDGER NAME Matched" & Chr(13) & "Verified Successfully", "FOR GSTIN VERIFICATION....", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

            Else

                MessageBox.Show("GSTIN Verified Successfully, " & Chr(13) & "But GSTIN and LEDGER NAME does not Matched" & Chr(13) & "The Trade Name for this GSTIN is """ & UCase(Trim(v_Name)) & """" & Chr(13) & "Please Verify manually", "FOR GSTIN VERIFICATION....", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation)

            End If


        Else

            chk_GSTIN_Verified.Checked = False
            If vSAVING_STS = False Then
                If v_ERRMSG_SHOWN_STS = False Then
                    MessageBox.Show("Invalid GSTIN " & Chr(13) & "GSTIN Number does not exists - Verification Failed", "FOR GSTIN VERIFICATION....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                End If
            End If

        End If

        Return chk_GSTIN_Verified.Checked


        '--------------------------
        'Dim GSTIN As String = txt_GSTIN_No.Text
        '     Dim TxnResp As TxnRespWithObj(Of GSTINDetail) = Await EWBAPI.GetGSTNDetailAsync(EwbSession, GSTIN)

        'If TxnResp.IsSuccess Then

        '    '      Dim rawResp As String = JsonConvert.SerializeObject(TxnResp.RespObj)

        '    '   Dim jss As New JavaScriptSerializer()
        '    Dim dict As Dictionary(Of String, String) = jss.Deserialize(Of Dictionary(Of String, String))(rawResp)

        '    If UCase(Trim(dict("tradeName"))) = UCase(Trim(txt_Name.Text)) Then
        '        txt_GSTIN_No.BackColor = Color.White
        '        txt_GSTIN_No.ForeColor = Color.Black
        '        chk_GSTIN_Verified.Checked = True
        '        MessageBox.Show("GSTIN and LEDGER NAME Matched" & Chr(13) & "Verified Successfully", "GSTIN NO VERIFICATION", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        '    Else

        '        MessageBox.Show("GSTIN and LEDGER NAME does not Matched" & Chr(13) & "The Trade Name for this GSTIN is """ & UCase(Trim(dict("tradeName"))) & """" & Chr(13) & "Please Verify manually", "GSTIN NO VERIFICATION", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation)

        '    End If

        'Else

        '    MessageBox.Show("Invalid GSTIN " & Chr(13) & "GSTIN Number does not exists - Verification Failed", "GSTIN NO VERIFICATION", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        'End If

    End Function

    Private Sub btn_Extract_Click(sender As Object, e As EventArgs) Handles btn_Extract.Click
        Dim v_Name As String = ""
        Dim v_LegalName_Business As String = ""
        Dim v_Address1 As String = ""
        Dim v_Address2 As String = ""
        Dim v_Address3 As String = ""
        Dim v_Address4 As String = ""
        Dim v_city As String = ""
        Dim v_StateName As String = ""
        Dim v_pincode As String = ""
        Dim v_ERRMSG_SHOWN_STS As Boolean = False

        GSTIN_Search.SEARCHGSTIN(Trim(txt_GSTIN_No.Text), v_Name, v_LegalName_Business, v_Address1, v_Address2, v_Address3, v_Address4, v_city, v_StateName, v_pincode, v_ERRMSG_SHOWN_STS, False)

        If Trim(v_Name) <> "" Then
            txt_Name.Text = v_Name
            txt_LegalName_Business.Text = v_LegalName_Business
            txt_Address1.Text = v_Address1
            txt_Address2.Text = v_Address2
            txt_Address3.Text = v_Address3
            txt_Address4.Text = v_Address4
            txt_city.Text = v_city
            cbo_State.Text = v_StateName
            txt_pincode.Text = v_pincode

            chk_GSTIN_Verified.Checked = True

        Else

            chk_GSTIN_Verified.Checked = False
            If v_ERRMSG_SHOWN_STS = False Then
                MessageBox.Show("Invalid GSTIN " & Chr(13) & "GSTIN Number does not exists - Verification Failed", "FOR GSTIN VERIFICATION....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            End If

        End If

        'Dim VERIFICATION_STS As Boolean = False
        'GSTIN_Search.SEARCHGSTIN(Trim(txt_GSTIN_No.Text), txt_Name, txt_LegalName_Business, txt_Address1, txt_Address2, txt_Address3, txt_Address4, txt_city, cbo_State, txt_pincode, False, VERIFICATION_STS)
        'SEARCHGSTIN(txt_GSTIN_No.Text)
    End Sub

    Private Sub btn_Extract_Click_111(sender As Object, e As EventArgs)
        'Dim GSTIN As String = txt_GSTIN_No.Text
        'Dim TxnResp As TxnRespWithObj(Of GSTINDetail) = Await EWBAPI.GetGSTNDetailAsync(EwbSession, GSTIN)

        'If TxnResp.IsSuccess Then

        '    'Dim rawResp As String = JsonConvert.SerializeObject(TxnResp.RespObj)

        '    'Dim jss As New JavaScriptSerializer()
        '    Dim dict As Dictionary(Of String, String) = jss.Deserialize(Of Dictionary(Of String, String))(rawResp)

        '    txt_Name.Text = UCase(Trim(dict("tradeName")))
        '    txt_Address1.Text = UCase(Trim(dict("address1")))
        '    txt_Address2.Text = UCase(Trim(dict("address2")))
        '    txt_Address3.Text = "" ' UCase(Trim(dict("city")))
        '    txt_Address4.Text = "" ' UCase(Trim(dict("address4")))
        '    txt_pincode.Text = UCase(Trim(dict("pinCode")))
        '    'txt_Address2.Text = Replace(txt_Address2.Text, txt_Address4.Text, "", 1)
        '    cbo_State.Text = Common_Procedures.get_FieldValue(con, "State_Head", "State_Name", "State_Code = '" & Trim(dict("stateCode")) & "'")
        '    chk_GSTIN_Verified.Checked = True
        '    txt_GSTIN_No.BackColor = Color.White
        '    txt_GSTIN_No.ForeColor = Color.Black

        'Else
        '    MessageBox.Show("Invalid GSTIN" & Chr(13) & "GSTIN Number does not exists", "DOES NOT GET GSTIN DETAILS", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        'End If

    End Sub

    Private Sub DisplayApiLoginDetails()

        'Gstin = EwbSession.EwbApiLoginDetails.EwbGstin
        'UserId = EwbSession.EwbApiLoginDetails.EwbUserID
        'Password = EwbSession.EwbApiLoginDetails.EwbPassword

        'AppKey = EwbSession.EwbApiLoginDetails.EwbAppKey
        'AuthToken = EwbSession.EwbApiLoginDetails.EwbAuthToken
        'TokenExp = EwbSession.EwbApiLoginDetails.EwbTokenExp.ToString("dd/MM/yyyy HH:mm:ss")
        'SEK = EwbSession.EwbApiLoginDetails.EwbSEK

    End Sub

    Private Sub DisplayApiSettings()
        'GSPName = EwbSession.EwbApiSetting.GSPName
        'ASPUserID = EwbSession.EwbApiSetting.AspUserId
        'AspPassword = EwbSession.EwbApiSetting.AspPassword
        'ClientId = EwbSession.EwbApiSetting.EWBClientId
        'ClientSecret = EwbSession.EwbApiSetting.EWBClientSecret
        'GspUserId = EwbSession.EwbApiSetting.EWBGSPUserID
        'BaseURL = EwbSession.EwbApiSetting.BaseUrl
    End Sub

    Private Sub txt_vehicleNo_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_vehicleNo.KeyDown
        If e.KeyValue = 38 Then
            e.Handled = True
            e.SuppressKeyPress = True
            If Txt_Remarks.Visible Then
                Txt_Remarks.Focus()
            Else
                txt_OwnerName.Focus()
            End If

        End If


        If e.KeyValue = 40 Then
            e.Handled = True : e.SuppressKeyPress = True


            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                txt_Name.Focus()
            End If

        End If

    End Sub

    Private Sub txt_vehicleNo_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_vehicleNo.KeyPress

        If Asc(e.KeyChar) = 13 Then


            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                txt_Name.Focus()
            End If

        End If
    End Sub


    Private Sub Txt_Remarks_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Txt_Remarks.KeyDown

        If (e.KeyValue = 38) Or (e.Control = True And e.KeyValue = 38) Then

            If cbo_marketting_Exec_Name.Visible Then
                cbo_marketting_Exec_Name.Focus()
            ElseIf cbo_Sizing_To_VendorName.Visible And cbo_Sizing_To_VendorName.Enabled Then
                cbo_Sizing_To_VendorName.Focus()

            ElseIf txt_creditLimitDays.Visible And txt_creditLimitDays.Enabled Then
                txt_creditLimitDays.Focus()

            ElseIf cbo_Sizing_To_CompanyName.Visible = True Then
                cbo_Sizing_To_CompanyName.Focus()

            ElseIf cbo_Sizing_To_VendorName.Visible = True Then
                cbo_Sizing_To_VendorName.Focus()

            ElseIf cbo_Transfer_StockTo.Visible = True Then
                cbo_Transfer_StockTo.Focus()

            ElseIf txt_TdsPerc.Visible And txt_TdsPerc.Enabled Then
                txt_TdsPerc.Focus()

            ElseIf txt_CreditLimit.Visible = True Then
                txt_CreditLimit.Focus()
            ElseIf cbo_party_category.Visible And cbo_party_category.Enabled Then
                cbo_party_category.Focus()

            ElseIf cbo_designation.Visible And cbo_designation.Enabled Then
                cbo_designation.Focus()

            ElseIf txt_OwnerName.Visible And txt_OwnerName.Enabled Then
                txt_OwnerName.Focus()


            Else
                txt_PanNo.Focus()

            End If
        End If

        If (e.KeyValue = 40) Or (e.Control = True And e.KeyValue = 40) Then
            If txt_vehicleNo.Visible And txt_vehicleNo.Enabled Then
                txt_vehicleNo.Focus()

            Else
                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    save_record()
                Else
                    txt_Name.Focus()
                End If

            End If

        End If
    End Sub

    Private Sub Txt_Remarks_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Txt_Remarks.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If txt_vehicleNo.Visible And txt_vehicleNo.Enabled Then
                txt_vehicleNo.Focus()

            Else

                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    save_record()
                Else
                    txt_Name.Focus()
                End If

            End If
        End If
    End Sub

    Private Sub lbl_PanNo_From_GSTNo_Click(sender As Object, e As EventArgs) Handles lbl_PanNo_From_GSTNo.Click

        Dim cn1 As New SqlClient.SqlConnection
        Dim cmd As New SqlClient.SqlCommand
        Dim tr As SqlClient.SqlTransaction

        Dim g As New Password
        g.ShowDialog()

        If Trim(UCase(Common_Procedures.Password_Input)) <> "GOLD12345" Then
            MessageBox.Show("Invalid Password", "PASSWORD FAILED.....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        cn1 = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        cn1.Open()

        cmd.Connection = cn1

        tr = cn1.BeginTransaction

        Try

            cmd.Transaction = tr

            cmd.CommandText = "Update ledger_head set Pan_No = SUBSTRING(Ledger_GSTinNo, 3, 10) Where Ledger_GSTinNo <> '' and len(Ledger_GSTinNo) >=14"
            cmd.ExecuteNonQuery()

            tr.Commit()

            cn1.Close()

            MessageBox.Show("Successfully Updated", "FOR FIELDS CREATION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR FIELDS CREATION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            cmd.Dispose()
            tr.Dispose()

        End Try

    End Sub

    Private Sub txt_contact_person_name_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_contact_person_name.KeyDown
        If e.KeyCode = 38 Then
            txt_OwnerName.Focus()
        End If
        If e.KeyCode = 40 Then
            cbo_designation.Focus()
        End If
    End Sub

    Private Sub txt_contact_person_name_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_contact_person_name.KeyPress
        If Asc(e.KeyChar) = 13 Then
            cbo_designation.Focus()
        End If
    End Sub

    Private Sub cbo_designation_GotFocus(sender As Object, e As EventArgs) Handles cbo_designation.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Contact_Designation_Head", "Contact_Designation_Name", "", "(Contact_Contact_Designation_IdNo = 0)")
    End Sub

    Private Sub cbo_designation_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_designation.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_designation, txt_contact_person_name, cbo_party_category, "Contact_Designation_Head", "Contact_Designation_Name", "", "(Contact_Contact_Designation_IdNo = 0)")



    End Sub

    Private Sub cbo_designation_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_designation.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_designation, cbo_party_category, "Contact_Designation_Head", "Contact_Designation_Name", "", "(Contact_Contact_Designation_IdNo = 0)")

    End Sub

    Private Sub cbo_designation_KeyUp(sender As Object, e As KeyEventArgs) Handles cbo_designation.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            Dim f As New Contact_Designation_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_designation.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_party_category_GotFocus(sender As Object, e As EventArgs) Handles cbo_party_category.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Party_Category_Head", "Party_Category_Name", "", "(Party_Category_IdNo = 0)")
    End Sub

    Private Sub cbo_party_category_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_party_category.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_party_category, cbo_AcGroup, txt_Address1, "Party_Category_Head", "Party_Category_Name", "", "(Party_Category_IdNo = 0)")

    End Sub

    Private Sub cbo_party_category_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_party_category.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_party_category, txt_Address1, "Party_Category_Head", "Party_Category_Name", "", "(Party_Category_IdNo = 0)")
    End Sub

    Private Sub cbo_party_category_KeyUp(sender As Object, e As KeyEventArgs) Handles cbo_party_category.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            Dim f As New Party_Category_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_party_category.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub txt_Name_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Name.KeyDown

        If e.KeyValue = 38 Then
            If dgv_Contact_Person_Details.Rows.Count <= 0 Then dgv_Contact_Person_Details.Rows.Add()
            dgv_Contact_Person_Details.Focus()
            dgv_Contact_Person_Details.CurrentCell = dgv_Contact_Person_Details.Rows(0).Cells(1)
        End If

        If e.KeyValue = 40 Then
            e.Handled = True
            If txt_Ledger_ShortName.Visible Then
                txt_Ledger_ShortName.Focus()
            Else
                txt_LegalName_Business.Focus()
            End If
        End If
    End Sub

    Private Sub dgv_Contact_Person_Details_CellEnter(sender As Object, e As DataGridViewCellEventArgs) Handles dgv_Contact_Person_Details.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Rect As Rectangle

        With dgv_Contact_Person_Details
            dgv_ActiveCtrl_Name = .Name

            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If




            If e.ColumnIndex = 4 Then

                If Cbo_Gird_desigantion.Visible = False Or Val(Cbo_Gird_desigantion.Tag) <> e.RowIndex Then

                    Cbo_Gird_desigantion.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Contact_Designation_Name from Contact_Designation_Head order by Contact_Designation_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    Cbo_Gird_desigantion.DataSource = Dt1
                    Cbo_Gird_desigantion.DisplayMember = "Contact_Designation_Name"

                    Rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    Cbo_Gird_desigantion.Left = .Left + Rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    Cbo_Gird_desigantion.Top = .Top + Rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                    Cbo_Gird_desigantion.Width = Rect.Width  ' .CurrentCell.Size.Width
                    Cbo_Gird_desigantion.Height = Rect.Height  ' rect.Height

                    Cbo_Gird_desigantion.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    Cbo_Gird_desigantion.Tag = Val(e.RowIndex)
                    Cbo_Gird_desigantion.Visible = True

                    Cbo_Gird_desigantion.BringToFront()
                    Cbo_Gird_desigantion.Focus()

                End If

            Else

                Cbo_Gird_desigantion.Visible = False

            End If

        End With
    End Sub

    Private Sub dgv_Contact_Person_Details_KeyDown(sender As Object, e As KeyEventArgs) Handles dgv_Contact_Person_Details.KeyDown
        With dgv_Contact_Person_Details
            If e.KeyCode = Keys.Up Then
                If .CurrentRow.Index <= 0 Then
                    txt_PhoneNo.Focus()
                End If
            End If

            If e.KeyCode = Keys.Left Then
                If .CurrentCell.RowIndex <= 0 And .CurrentCell.ColumnIndex <= 1 Then
                    txt_PhoneNo.Focus()
                End If
            End If

        End With
    End Sub

    Private Sub dgv_Contact_Person_Details_KeyUp(sender As Object, e As KeyEventArgs) Handles dgv_Contact_Person_Details.KeyUp
        Dim n As Integer
        Dim i As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_Contact_Person_Details
                If .CurrentRow.Index = .RowCount - 1 Then
                    For i = 1 To .ColumnCount - 1
                        .Rows(.CurrentRow.Index).Cells(i).Value = ""
                    Next

                Else
                    n = .CurrentRow.Index
                    .Rows.RemoveAt(n)

                End If

                For i = 0 To .Rows.Count - 1
                    .Rows(i).Cells(0).Value = i + 1
                Next

            End With
        End If
    End Sub

    Private Sub dgv_Contact_Person_Details_LostFocus(sender As Object, e As EventArgs) Handles dgv_Contact_Person_Details.LostFocus
        Grid_DeSelect()
    End Sub

    Private Sub dgv_Contact_Person_Details_RowsAdded(sender As Object, e As DataGridViewRowsAddedEventArgs) Handles dgv_Contact_Person_Details.RowsAdded
        With dgv_Contact_Person_Details
            If Val(.Rows(.RowCount - 1).Cells(0).Value) = 0 Then
                .Rows(.RowCount - 1).Cells(0).Value = .RowCount
            End If
        End With
    End Sub

    Private Sub txt_PhoneNo_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_PhoneNo.KeyDown

        If e.KeyValue = 38 Then
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1413" Then

                cbo_State.Focus()
            Else
                txt_GSTIN_No.Focus()
            End If
        End If

        If e.KeyValue = 40 Then
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1413" Then
                txt_GSTIN_No.Focus()
            Else
                txt_MobileSms.Focus()
            End If
        End If
    End Sub

    Private Sub txt_PhoneNo_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_PhoneNo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1413" Then
                txt_GSTIN_No.Focus()
            Else
                txt_MobileSms.Focus()
            End If
        End If
    End Sub
    Private Sub dgtxt_ContactPersonDetails_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_ContactPersonDetails.Enter
        dgv_ActiveCtrl_Name = dgv_Contact_Person_Details.Name
        dgv_Contact_Person_Details.EditingControl.BackColor = Color.Lime
        dgv_Contact_Person_Details.EditingControl.ForeColor = Color.Blue
    End Sub
    Private Sub dgv_Contact_Person_Details_EditingControlShowing(sender As Object, e As DataGridViewEditingControlShowingEventArgs) Handles dgv_Contact_Person_Details.EditingControlShowing
        dgtxt_ContactPersonDetails = CType(dgv_Contact_Person_Details.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub Cbo_Gird_desigantion_GotFocus(sender As Object, e As EventArgs) Handles Cbo_Gird_desigantion.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Contact_Designation_Head", "Contact_Designation_Name", "", "(Contact_Designation_IdNo = 0)")
    End Sub
    Private Sub Cbo_Gird_desigantion_KeyDown(sender As Object, e As KeyEventArgs) Handles Cbo_Gird_desigantion.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, Cbo_Gird_desigantion, Nothing, Nothing, "Contact_Designation_Head", "Contact_Designation_Name", "", "(Contact_Designation_IdNo = 0)")

        With dgv_Contact_Person_Details

            If (e.KeyValue = 38 And Cbo_Gird_desigantion.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                'If .CurrentCell.RowIndex <= 0 Then
                '    txt_GSTIN_No.Focus()
                'Else
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)

                ' End If
            End If

            If (e.KeyValue = 40 And Cbo_Gird_desigantion.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                If .CurrentCell.RowIndex = .Rows.Count - 1 And Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(1).Value) = "" Then
                    'If .CurrentCell.RowIndex = .RowCount - 1 Then

                    If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.Yes Then
                        save_record()

                    Else
                        txt_Name.Focus()


                    End If
                Else
                    .Focus()
                    dgv_Contact_Person_Details.CurrentCell = dgv_Contact_Person_Details.Rows(dgv_Contact_Person_Details.CurrentRow.Index + 1).Cells(1)
                End If
            End If

        End With
    End Sub
    Private Sub Cbo_Gird_desigantion_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Cbo_Gird_desigantion.KeyPress
        Dim Vdesignation_Id As Integer = 0

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, Cbo_Gird_desigantion, Nothing, "Contact_Designation_Head", "Contact_Designation_Name", "", "(Contact_Designation_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then
            With dgv_Contact_Person_Details

                If .CurrentCell.RowIndex = .RowCount - 1 Then

                    If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.Yes Then
                        save_record()

                    Else
                        txt_Name.Focus()


                    End If
                Else
                    .Focus()
                    dgv_Contact_Person_Details.CurrentCell = dgv_Contact_Person_Details.Rows(dgv_Contact_Person_Details.CurrentRow.Index + 1).Cells(1)
                End If
            End With

        End If
    End Sub

    Private Sub Cbo_Gird_desigantion_KeyUp(sender As Object, e As KeyEventArgs) Handles Cbo_Gird_desigantion.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Contact_Designation_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = Cbo_Gird_desigantion.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub Cbo_Gird_desigantion_Enter(sender As Object, e As EventArgs) Handles Cbo_Gird_desigantion.Enter
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Contact_Designation_Head", "Contact_Designation_Name", "", "(Contact_Designation_IdNo = 0)")
    End Sub

    Private Sub Cbo_Gird_desigantion_TextChanged(sender As Object, e As EventArgs) Handles Cbo_Gird_desigantion.TextChanged
        Try
            If Cbo_Gird_desigantion.Visible Then
                With dgv_Contact_Person_Details
                    If Val(Cbo_Gird_desigantion.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 4 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(Cbo_Gird_desigantion.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub txt_GSTIN_No_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_GSTIN_No.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1413" Then
                If dgv_Contact_Person_Details.Enabled = True Then
                    If dgv_Contact_Person_Details.RowCount > 0 Then

                        dgv_Contact_Person_Details.Focus()
                        dgv_Contact_Person_Details.CurrentCell = dgv_Contact_Person_Details.Rows(0).Cells(1)
                        dgv_Contact_Person_Details.CurrentCell.Selected = True
                    Else
                        save_record()
                    End If
                End If
            Else
                txt_PhoneNo.Focus()
            End If
        End If
    End Sub

    Private Sub txt_GSTIN_No_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_GSTIN_No.KeyDown
        If e.KeyCode = 38 Then
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1413" Then
                txt_PhoneNo.Focus()
            Else
                txt_Distance.Focus()
            End If
        End If

        If e.KeyCode = 40 Then
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1413" Then
                If dgv_Contact_Person_Details.Enabled = True Then
                    If dgv_Contact_Person_Details.RowCount > 0 Then

                        dgv_Contact_Person_Details.Focus()
                        dgv_Contact_Person_Details.CurrentCell = dgv_Contact_Person_Details.Rows(0).Cells(1)
                        dgv_Contact_Person_Details.CurrentCell.Selected = True
                    Else
                        save_record()
                    End If
                End If
            Else
                txt_PhoneNo.Focus()
            End If
        End If
    End Sub

    Private Sub btn_Import_LedgerAddress_Click(sender As Object, e As EventArgs) Handles btn_Import_LedgerAddress.Click
        Dim CmdFrm As New SqlClient.SqlCommand
        Dim CmdTo As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim vLedger_IdNo As Integer, vOldLID As Integer
        Dim vLedger_Name As String, vSur_Name As String, vLedger_MainName As String, vLedger_AlaisName As String, vLedger_StateName As String
        Dim vArea_IdNo As Integer, vAccountsGroup_IdNo As Integer, vState_IdNo As Integer, vParent_Code As String, vBill_Type As String
        Dim vLedger_Address1 As String, vLedger_Address2 As String, vLedger_Address3 As String, vLedger_Address4 As String, vLedger_PhoneNo As String
        Dim vLedger_TinNo As String, vLedger_CstNo As String, vLedger_Type As String, vPan_No As String
        Dim vLedger_Emailid As String, vLedger_FaxNo As String, vLedger_MobileNo As String, vContact_Person As String, vLedger_GSTNo As String
        Dim vPackingType_CompanyIdNo As Integer, vLedger_AgentIdNo As Integer, vTransport_Name As String, vNote As String
        Dim vMobileNo_Sms As String, vBilling_Type As String, vSticker_Type As String, vMrp_Perc As String
        Dim vLedNm As String
        Dim sqltr As SqlClient.SqlTransaction
        Dim cmd As New SqlClient.SqlCommand
        'Dim tr As SqlClient.SqlTransaction
        Dim da As New SqlClient.SqlDataAdapter

        Dim xlApp As Excel.Application
        Dim xlWorkBook As Excel.Workbook
        Dim xlWorkSheet As Excel.Worksheet

        Dim RowCnt As Long = 0
        Dim FileName As String = ""
        Dim NewCode As String = ""
        Dim ShfId As Integer = 0
        Dim ItmId As Integer = 0
        Dim j, k, l As Integer
        Dim Sn As Integer = 0
        Dim vSurNm As String = ""
        Dim vShow_In_All_Entry As Integer, vVerified_Status As Integer = 0
        Dim vTransport_IdNo As Integer, vNoOf_Looms As Integer
        Dim vFreight_Loom As Single
        Dim vOwn_Loom_Status As Integer
        Dim vTds_Percentage As Single
        Dim vOwner_Name As String
        Dim vPartner_Proprietor As String
        Dim vCloth_Comm_Meter As Single = 0, vCloth_Comm_Percentage As Single
        Dim vYarn_Comm_Bag As Single, vYarn_Comm_Percentage As Single
        Dim AccGrpAr() As String
        Dim Inc As Integer = 0
        Dim AccGrp1 As String = ""
        Dim AccGrp2 As String = ""
        Dim AccGrp3 As String = ""
        Dim AccGrp4 As String = ""
        Dim vLedger_StateCode As String = ""
        Dim vSALES_PUR_Type As String

        Dim g As New Password
        g.ShowDialog()

        If Trim(UCase(Common_Procedures.Password_Input)) <> "TSIMP123" Then
            MessageBox.Show("Invalid Password", "PASSWORD FAILED...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If


        CmdTo.Connection = con

        sqltr = con.BeginTransaction

        CmdTo.Transaction = sqltr

        Try

            OpenFileDialog1.ShowDialog()
            FileName = OpenFileDialog1.FileName

            If Not IO.File.Exists(FileName) Then
                MessageBox.Show(FileName & " File not found", "File not found...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub
            End If

            xlApp = New Excel.Application
            xlWorkBook = xlApp.Workbooks.Open(FileName)
            xlWorkSheet = xlWorkBook.Worksheets("sheet1")

            With xlWorkSheet
                RowCnt = .Range("A" & .Rows.Count).End(Excel.XlDirection.xlUp).Row
            End With

            'RowCnt = xlWorkSheet.UsedRange.Rows.Count

            If RowCnt <= 1 Then
                MessageBox.Show("Data not found", "Data not found...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub
            End If

            For i = 2 To RowCnt

                vLedger_Address1 = ""
                vLedger_Address2 = ""
                vLedger_Address3 = ""
                vLedger_Address4 = ""

                vLedNm = UCase(Trim(xlWorkSheet.Cells(i, 1).value))
                vLedNm = Replace(UCase(Trim(vLedNm)), "'", "")

                If Trim(vLedNm) = "" Then
                    Continue For
                End If

                vSur_Name = Common_Procedures.Remove_NonCharacters(vLedNm)

                vOldLID = Val(Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_IdNo", "(Sur_Name = '" & Trim(vSur_Name) & "')", , sqltr))

                vLedger_Address1 = Trim(Replace(Trim(xlWorkSheet.Cells(i, 3).value), "'", ""))
                vLedger_Address2 = ""
                If Trim(vLedger_Address1) <> "" Then
                    If Len(vLedger_Address1) > 40 Then

                        For j = 40 To 1 Step -1
                            If Mid$(Trim(vLedger_Address1), j, 1) = " " Or Mid$(Trim(vLedger_Address1), j, 1) = "," Then Exit For
                        Next j
                        If j = 0 Then j = 40

                        vLedger_Address2 = Microsoft.VisualBasic.Right(Trim(vLedger_Address1), Len(vLedger_Address1) - j)
                        vLedger_Address1 = Microsoft.VisualBasic.Left(Trim(vLedger_Address1), j - 1)

                    End If

                End If

                If Trim(vLedger_Address2) <> "" Then
                    If Len(vLedger_Address2) > 40 Then
                        For k = 40 To 1 Step -1
                            If Mid$(Trim(vLedger_Address1), k, 1) = " " Or Mid$(Trim(vLedger_Address1), k, 1) = "," Or Mid$(Trim(vLedger_Address1), k, 1) = "." Then Exit For
                        Next k
                        If k = 0 Then k = 40
                        vLedger_Address3 = Microsoft.VisualBasic.Right(Trim(vLedger_Address2), Len(vLedger_Address2) - k)
                        vLedger_Address2 = Microsoft.VisualBasic.Left(Trim(vLedger_Address2), k - 1)
                    End If
                End If

                If Trim(vLedger_Address3) <> "" Then
                    If Len(vLedger_Address3) > 70 Then
                        For l = 70 To 1 Step -1
                            If Mid$(Trim(vLedger_Address1), l, 1) = " " Or Mid$(Trim(vLedger_Address1), l, 1) = "," Or Mid$(Trim(vLedger_Address1), l, 1) = "." Then Exit For
                        Next l
                        If l = 0 Then l = 70
                        vLedger_Address4 = Microsoft.VisualBasic.Right(Trim(vLedger_Address3), Len(vLedger_Address3) - l)
                        vLedger_Address3 = Microsoft.VisualBasic.Left(Trim(vLedger_Address3), l - 1)
                    End If
                End If

                If vOldLID = 0 Then

                    Debug.Print(i)
                    Continue For

                    vLedger_IdNo = Common_Procedures.get_MaxIdNo(con, "ledger_head", "ledger_idno", "", sqltr)
                    If Val(vLedger_IdNo) < 100 Then
                        vLedger_IdNo = 101
                    End If

                    vLedger_Name = Trim(Replace(UCase(Trim(xlWorkSheet.Cells(i, 1).value)), "'", ""))
                    vLedger_MainName = vLedger_Name
                    vSur_Name = Common_Procedures.Remove_NonCharacters(vLedger_Name)
                    vLedger_AlaisName = ""
                    vArea_IdNo = 0

                    vSALES_PUR_Type = xlWorkSheet.Cells(i, 4).value

                    If Val(vSALES_PUR_Type) = 2 Then
                        vAccountsGroup_IdNo = 14
                        vParent_Code = "~14~11~"
                    Else
                        vAccountsGroup_IdNo = 10
                        vParent_Code = "~10~4~"
                    End If

                    vBill_Type = "BILL TO BILL"
                    'vBill_Type = "BALANCE ONLY"

                    vLedger_Address1 = Trim(xlWorkSheet.Cells(i, 3).value)
                    vLedger_Address2 = ""
                    If Trim(vLedger_Address1) <> "" Then
                        If Len(vLedger_Address1) > 40 Then

                            For j = 40 To 1 Step -1
                                If Mid$(Trim(vLedger_Address1), j, 1) = " " Or Mid$(Trim(vLedger_Address1), j, 1) = "," Then Exit For
                            Next j
                            If j = 0 Then j = 40

                            vLedger_Address2 = Microsoft.VisualBasic.Right(Trim(vLedger_Address1), Len(vLedger_Address1) - j)
                            vLedger_Address1 = Microsoft.VisualBasic.Left(Trim(vLedger_Address1), j - 1)

                        End If

                    End If

                    If Trim(vLedger_Address2) <> "" Then
                        If Len(vLedger_Address2) > 40 Then
                            For k = 40 To 1 Step -1
                                If Mid$(Trim(vLedger_Address1), k, 1) = " " Or Mid$(Trim(vLedger_Address1), k, 1) = "," Or Mid$(Trim(vLedger_Address1), k, 1) = "." Then Exit For
                            Next k
                            If k = 0 Then k = 40
                            vLedger_Address3 = Microsoft.VisualBasic.Right(Trim(vLedger_Address2), Len(vLedger_Address2) - k)
                            vLedger_Address2 = Microsoft.VisualBasic.Left(Trim(vLedger_Address2), k - 1)
                        End If
                    End If

                    If Trim(vLedger_Address3) <> "" Then
                        If Len(vLedger_Address3) > 70 Then
                            For l = 70 To 1 Step -1
                                If Mid$(Trim(vLedger_Address1), l, 1) = " " Or Mid$(Trim(vLedger_Address1), l, 1) = "," Or Mid$(Trim(vLedger_Address1), l, 1) = "." Then Exit For
                            Next l
                            If l = 0 Then l = 70
                            vLedger_Address4 = Microsoft.VisualBasic.Right(Trim(vLedger_Address3), Len(vLedger_Address3) - l)
                            vLedger_Address3 = Microsoft.VisualBasic.Left(Trim(vLedger_Address3), l - 1)
                        End If
                    End If

                    vLedger_GSTNo = Trim(xlWorkSheet.Cells(i, 2).value)


                    vLedger_StateCode = Microsoft.VisualBasic.Left(vLedger_GSTNo, 2)

                    vState_IdNo = Val(Common_Procedures.get_FieldValue(con, "State_Head", "State_Idno", "(State_Code = '" & Trim(vLedger_StateCode) & "')", , sqltr))

                    'vLedger_StateName = Trim(xlWorkSheet.Cells(i, 4).value)

                    'vState_IdNo = Common_Procedures.State_NameToIdNo(con, Trim(vLedger_StateName))



                    vLedger_PhoneNo = ""
                    vLedger_TinNo = ""
                    vLedger_CstNo = ""
                    vLedger_Type = ""
                    vPan_No = ""
                    vLedger_Emailid = ""
                    vLedger_FaxNo = ""
                    vLedger_MobileNo = ""
                    vContact_Person = ""
                    vPackingType_CompanyIdNo = 0
                    vLedger_AgentIdNo = 0
                    vTransport_Name = ""
                    vNote = ""
                    vMobileNo_Sms = ""
                    vBilling_Type = ""

                    CmdTo.CommandText = "Insert into ledger_head ( Ledger_IdNo        ,            Ledger_Name      ,            Sur_Name      ,            Ledger_MainName      ,            Ledger_AlaisName      ,              Area_IdNo      ,              AccountsGroup_IdNo      ,            Parent_Code      ,            Bill_Type      ,            Ledger_Address1      ,            Ledger_Address2      ,            Ledger_Address3      ,            Ledger_Address4      ,            Ledger_PhoneNo      ,            Ledger_TinNo      ,            Ledger_CstNo      ,            Ledger_Type      ,            Pan_No      ,            Partner_Proprietor      ,              Yarn_Comm_Percentage      ,              Yarn_Comm_Bag      ,              Cloth_Comm_Percentage      ,            Ledger_Emailid      ,            Ledger_FaxNo      ,            Ledger_MobileNo      ,            MobileNo_Frsms    ,            MobileNo_Sms      ,            Contact_Person      ,             PackingType_CompanyIdNo       ,              Ledger_AgentIdNo      ,            Note      ,            Show_In_All_Entry        ,            Billing_Type      ,            Sticker_Type      ,            Mrp_Perc      ,              Own_Loom_Status      ,              Freight_Loom      ,              NoOf_Looms      ,              Transport_IdNo      , Verified_Status,            Owner_Name      ,              Tds_Percentage     , Ledger_GSTinNo                 ,  Ledger_State_IdNo   ) " &
                                     "       Values (" & Str(Val(vLedger_IdNo)) & ", '" & Trim(vLedger_Name) & "', '" & Trim(vSur_Name) & "', '" & Trim(vLedger_MainName) & "', '" & Trim(vLedger_AlaisName) & "', " & Str(Val(vArea_IdNo)) & ", " & Str(Val(vAccountsGroup_IdNo)) & ", '" & Trim(vParent_Code) & "', '" & Trim(vBill_Type) & "', '" & Trim(vLedger_Address1) & "', '" & Trim(vLedger_Address2) & "', '" & Trim(vLedger_Address3) & "', '" & Trim(vLedger_Address4) & "', '" & Trim(vLedger_PhoneNo) & "', '" & Trim(vLedger_TinNo) & "', '" & Trim(vLedger_CstNo) & "', '" & Trim(vLedger_Type) & "', '" & Trim(vPan_No) & "', '" & Trim(vPartner_Proprietor) & "', " & Str(Val(vYarn_Comm_Percentage)) & ", " & Str(Val(vYarn_Comm_Bag)) & ", " & Str(Val(vCloth_Comm_Percentage)) & ", '" & Trim(vLedger_Emailid) & "', '" & Trim(vLedger_FaxNo) & "', '" & Trim(vLedger_MobileNo) & "', '" & Trim(vMobileNo_Sms) & "', '" & Trim(vMobileNo_Sms) & "', '" & Trim(vContact_Person) & "', " & Str(Val(vPackingType_CompanyIdNo)) & ", " & Str(Val(vLedger_AgentIdNo)) & ", '" & Trim(vNote) & "', " & Str(Val(vShow_In_All_Entry)) & ", '" & Trim(vBilling_Type) & "', '" & Trim(vSticker_Type) & "', '" & Trim(vMrp_Perc) & "', " & Str(Val(vOwn_Loom_Status)) & ", " & Str(Val(vFreight_Loom)) & ", " & Str(Val(vNoOf_Looms)) & ", " & Str(Val(vTransport_IdNo)) & ",       1        , '" & Trim(vOwner_Name) & "', " & Str(Val(vTds_Percentage)) & " , '" & Trim(vLedger_GSTNo) & "'  ," & Str(Val(vState_IdNo)) & " ) "
                    CmdTo.ExecuteNonQuery()

                    CmdTo.CommandText = "Insert into Ledger_AlaisHead ( Ledger_IdNo, Sl_No, Ledger_DisplayName, Ledger_Type, AccountsGroup_IdNo, Verified_Status ) Values (" & Str(Val(vLedger_IdNo)) & ",   1,     '" & Trim(vLedger_Name) & "',   '" & Trim(vLedger_Type) & "',  " & Str(Val(vAccountsGroup_IdNo)) & ",  1 )"
                    CmdTo.ExecuteNonQuery()

                Else

                    If Trim(vLedger_Address1) <> "" Or Trim(vLedger_Address2) <> "" Or Trim(vLedger_Address3) <> "" Or Trim(vLedger_Address4) <> "" Then

                        CmdTo.CommandText = "Update ledger_head set Ledger_Address1 = '" & Trim(vLedger_Address1) & "', Ledger_Address2 = '" & Trim(vLedger_Address2) & "', Ledger_Address3 = '" & Trim(vLedger_Address3) & "', Ledger_Address4 = '" & Trim(vLedger_Address4) & "' where Ledger_IdNo = " & Str(Val(vOldLID))
                        CmdTo.ExecuteNonQuery()

                    End If

                End If

            Next i

            sqltr.Commit()

            CmdTo.Dispose()
            Dt1.Dispose()
            Da1.Dispose()

            xlWorkBook.Close(False, FileName)
            xlApp.Quit()

            ReleaseComObject(xlWorkSheet)
            ReleaseComObject(xlWorkBook)
            ReleaseComObject(xlApp)


            MessageBox.Show("Imported Successfully!!!", "FOR IMPORTING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception
            sqltr.Rollback()
            MessageBox.Show(ex.Message, "DOES NOT IMPORT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub txt_Ledger_ShortName_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Ledger_ShortName.KeyDown
        If e.KeyValue = 38 Then
            txt_Name.Focus()
        End If
        If e.KeyValue = 40 Then
            txt_LegalName_Business.Focus()
        End If

    End Sub

    Private Sub txt_Ledger_ShortName_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_Ledger_ShortName.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_LegalName_Business.Focus()
        End If
    End Sub

    Private Sub cbo_marketting_Exec_Name_GotFocus(sender As Object, e As EventArgs) Handles cbo_marketting_Exec_Name.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Marketting_Executive_Head", "Marketting_Executive_Name", "", "(Marketting_Executive_IdNo = 0)")
    End Sub

    Private Sub cbo_marketting_Exec_Name_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_marketting_Exec_Name.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_marketting_Exec_Name, txt_OwnerName, Txt_Remarks, "Marketting_Executive_Head", "Marketting_Executive_Name", "", "(Marketting_Executive_IdNo = 0)")
    End Sub

    Private Sub cbo_marketting_Exec_Name_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_marketting_Exec_Name.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_marketting_Exec_Name, Txt_Remarks, "Marketting_Executive_Head", "Marketting_Executive_Name", "", "(Marketting_Executive_IdNo = 0)")
    End Sub

    Private Sub cbo_marketting_Exec_Name_KeyUp(sender As Object, e As KeyEventArgs) Handles cbo_marketting_Exec_Name.KeyUp
        'If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

        '    Dim f As New Marketting_Executive_Creation

        '    Common_Procedures.Master_Return.Form_Name = Me.Name
        '    Common_Procedures.Master_Return.Control_Name = cbo_marketting_Exec_Name.Name
        '    Common_Procedures.Master_Return.Return_Value = ""
        '    Common_Procedures.Master_Return.Master_Type = ""

        '    f.MdiParent = MDIParent1
        '    f.Show()

        'End If
    End Sub
    Private Sub txt_LegalName_Business_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_LegalName_Business.KeyDown

        If e.KeyValue = 38 Then

            If txt_Ledger_ShortName.Visible Then
                txt_Ledger_ShortName.Focus()
            Else
                txt_Name.Focus()
            End If

        End If

        If e.KeyValue = 40 Then

            If cbo_LedgerGroup.Visible Then
                cbo_LedgerGroup.Focus()
            Else
                txt_AlaisName.Focus()
            End If
        End If

    End Sub

    Private Sub txt_LegalName_Business_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_LegalName_Business.KeyPress

        If Asc(e.KeyChar) = 13 Then

            If cbo_LedgerGroup.Visible Then
                cbo_LedgerGroup.Focus()
            Else
                txt_AlaisName.Focus()
            End If

        End If

    End Sub

    Private Sub btn_Print_Address_2_Click(sender As Object, e As EventArgs) Handles btn_Print_Address_2.Click
        Dim LedName = ""

        PrntFormat2_STS = True

        If Trim(Common_Procedures.settings.CustomerCode) = "1267" Then

            Dim f As New Report_Details
            Common_Procedures.RptInputDet.ReportGroupName = "Register"
            Common_Procedures.RptInputDet.ReportName = "master weaver details"
            Common_Procedures.RptInputDet.ReportHeading = "Master Weaver Details"
            Common_Procedures.RptInputDet.ReportInputs = "W"
            f.MdiParent = MDIParent1
            f.Show()

            LedName = Trim(txt_Name.Text)

            If Trim(cbo_Area.Text) <> "" Then
                LedName = Trim(txt_Name.Text) & " (" & Trim(cbo_Area.Text) & ")"
            End If

            f.cbo_Inputs1.Text = LedName
            f.Show_Report()
        Else
            Printing_LedgerAddress_Print()
        End If

    End Sub

    Private Sub dgtxt_LoomDetails_TextChanged(sender As Object, e As EventArgs) Handles dgtxt_LoomDetails.TextChanged
        Try



            With dgv_Loom_Details
                If .Rows.Count <> 0 Then
                    .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(dgtxt_LoomDetails.Text)
                End If
            End With

        Catch ex As Exception

            '---------

        End Try
    End Sub
    Private Sub cbo_Grid_ClothName_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_ClothName.Enter
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")
    End Sub
    Private Sub cbo_Grid_ClothName_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_Grid_ClothName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_ClothName, Nothing, Nothing, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")


        With dgv_Loom_Details

            If (e.KeyValue = 38 And cbo_Grid_ClothName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)
                .CurrentCell.Selected = True

            ElseIf (e.KeyValue = 40 And cbo_Grid_ClothName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)
                .CurrentCell.Selected = True

            End If

        End With

    End Sub

    Private Sub cbo_Grid_ClothName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_ClothName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_ClothName, Nothing, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            With dgv_Loom_Details

                .Rows(.CurrentCell.RowIndex).Cells.Item(2).Value = Trim(cbo_Grid_ClothName.Text)
                If Trim(.Rows(.CurrentRow.Index).Cells(1).Value) = "" And .CurrentRow.Index = .Rows.Count - 1 Then
                    txt_Name.Focus()
                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)
                End If

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

    Private Sub cbo_Grid_ClothName_TextChanged(sender As Object, e As EventArgs) Handles cbo_Grid_ClothName.TextChanged

        Try
            If cbo_Grid_ClothName.Visible Then
                With dgv_Loom_Details
                    If IsNothing(dgv_Loom_Details.CurrentCell) Then Exit Sub
                    If Val(cbo_Grid_ClothName.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 2 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_ClothName.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            '-------------
        End Try
    End Sub

    Private Sub grp_Back_Enter(sender As Object, e As EventArgs) Handles grp_Back.Enter

    End Sub

    Private Sub btn_bank_Details_Click(sender As Object, e As EventArgs) Handles btn_bank_Details.Click

        grp_Back.Enabled = False
        pnl_bank_Details.Visible = True
        pnl_bank_Details.BringToFront()
        txt_Bank_Acc_Name.Focus()

    End Sub

    Private Sub txt_Bank_Acc_Name_TextChanged(sender As Object, e As EventArgs) Handles txt_Bank_Acc_Name.TextChanged

    End Sub

    Private Sub txt_Bank_Acc_Name_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_Bank_Acc_Name.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_bankName.Focus()
        End If
    End Sub

    Private Sub txt_Bank_Acc_Name_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Bank_Acc_Name.KeyDown
        If e.KeyCode = 40 Then
            txt_bankName.Focus()
        End If
    End Sub

    Private Sub txt_bankName_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_bankName.KeyPress

        If Asc(e.KeyChar) = 13 Then
            txt_AccountNo.Focus()
        End If

    End Sub

    Private Sub txt_bankName_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_bankName.KeyDown
        If e.KeyCode = 40 Then
            txt_AccountNo.Focus()
        End If

        If e.KeyCode = 38 Then
            txt_Bank_Acc_Name.Focus()
        End If
    End Sub

    Private Sub txt_AccountNo_TextChanged(sender As Object, e As EventArgs) Handles txt_AccountNo.TextChanged

    End Sub

    Private Sub txt_AccountNo_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_AccountNo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_Branch.Focus()
        End If
    End Sub

    Private Sub txt_AccountNo_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_AccountNo.KeyDown
        If e.KeyCode = 40 Then
            txt_Branch.Focus()
        End If

        If e.KeyCode = 38 Then
            txt_bankName.Focus()
        End If
    End Sub

    Private Sub txt_Branch_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_Branch.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_Ifsc_Code.Focus()
        End If
    End Sub

    Private Sub txt_Branch_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Branch.KeyDown
        If e.KeyCode = 40 Then
            txt_Ifsc_Code.Focus()
        End If

        If e.KeyCode = 38 Then
            txt_AccountNo.Focus()
        End If

    End Sub

    Private Sub txt_Ifsc_Code_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_Ifsc_Code.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_Bank_Acc_Name.Focus()
        End If
    End Sub

    Private Sub txt_Ifsc_Code_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Ifsc_Code.KeyDown
        If e.KeyCode = 40 Then
            txt_Bank_Acc_Name.Focus()
        End If

        If e.KeyCode = 38 Then
            txt_Branch.Focus()
        End If

    End Sub


    Private Sub btn_close_bankdetails_Click_1(sender As Object, e As EventArgs) Handles btn_close_bankdetails.Click
        pnl_bank_Details.Visible = False
        grp_Back.Enabled = True
    End Sub

    Private Sub pnl_tamil_Address_Paint(sender As Object, e As PaintEventArgs) Handles pnl_tamil_Address.Paint

    End Sub
End Class
