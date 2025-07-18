Public Class Yarn_Purchase_VAT
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "YNPUR-"
    Private Pk_Condition2 As String = "YPAGC-"
    Private Pk_Condition3 As String = "YPFRG-"
    Private Pk_Condition4 As String = "YPATD-"
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
    Private SaveAll_STS As Boolean = False
    Private LastNo As String = ""
    Public vmskOldText As String = ""
    Public vmskSelStrt As Integer = -1
    Public vmskBillOldText As String = ""
    Public vmskBillSelStrt As Integer = -1
    Private DeleteAll_STS As Boolean = False
    Private vSPEC_KEYS As New HashSet(Of Keys)()
    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        clear()
    End Sub

    Private Sub clear()

        NoCalc_Status = True

        New_Entry = False
        Insert_Entry = False

        pnl_Back.Enabled = True
        pnl_Selection.Visible = False
        pnl_Filter.Visible = False
        pnl_YarnTest.Visible = False

        vmskOldText = ""
        vmskSelStrt = -1
        vmskBillOldText = ""
        vmskBillSelStrt = -1

        lbl_RefNo.Text = ""
        lbl_RefNo.ForeColor = Color.Black

        msk_Date.Text = ""
        dtp_Date.Text = ""
        cbo_PartyName.Text = ""

        cbo_Agent.Text = ""
        cbo_PurchaseAc.Text = ""
        txt_RecNo.Text = ""
        cbo_VehicleNo.Text = ""
        cbo_Delvat.Text = Common_Procedures.Ledger_IdNoToName(con, 4)
        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))
        txt_BillNo.Text = ""
        txt_CommRate.Text = ""
        cbo_CommType.Text = "BAG"
        lbl_CommAmount.Text = ""

        lbl_GrossAmount.Text = ""

        txt_DiscPerc.Text = ""
        lbl_DiscAmount.Text = ""

        txt_AssessableValue.Text = ""

        cbo_VatAc.Text = ""

        cbo_TaxType.Text = "-NIL-"
        txt_TaxPerc.Text = ""
        lbl_TaxAmount.Text = ""

        txt_Freight.Text = ""
        txt_AddLessAfterTax_Text.Text = "Add/Less"
        txt_AddLess_AfterTax.Text = ""
        txt_AddLess_BeforeTax.Text = ""
        lbl_RoundOff.Text = ""
        lbl_NetAmount.Text = "0.00"
        lbl_AmountInWords.Text = "Rupees  :  "

        cbo_Transport.Text = ""
        txt_Note.Text = ""

        msk_BillDate.Text = ""
        dtp_BillDate.Text = ""


        txt_Test1_Cone1_YarnWeight1.Text = ""
        txt_Test1_Cone1_YarnWeight2.Text = ""
        txt_Test1_Cone1_YarnWeight3.Text = ""
        txt_Test1_Cone1_YarnWeight4.Text = ""
        txt_Test1_Cone1_YarnWeight5.Text = ""

        txt_Test1_Cone2_YarnWeight1.Text = ""
        txt_Test1_Cone2_YarnWeight2.Text = ""
        txt_Test1_Cone2_YarnWeight3.Text = ""
        txt_Test1_Cone2_YarnWeight4.Text = ""
        txt_Test1_Cone2_YarnWeight5.Text = ""


        txt_Test1_Cone3_YarnWeight1.Text = ""
        txt_Test1_Cone3_YarnWeight2.Text = ""
        txt_Test1_Cone3_YarnWeight3.Text = ""
        txt_Test1_Cone3_YarnWeight4.Text = ""
        txt_Test1_Cone3_YarnWeight5.Text = ""


        txt_Test1_Cone4_YarnWeight1.Text = ""
        txt_Test1_Cone4_YarnWeight2.Text = ""
        txt_Test1_Cone4_YarnWeight3.Text = ""
        txt_Test1_Cone4_YarnWeight4.Text = ""
        txt_Test1_Cone4_YarnWeight5.Text = ""

        txt_Test1_Cone5_YarnWeight1.Text = ""
        txt_Test1_Cone5_YarnWeight2.Text = ""
        txt_Test1_Cone5_YarnWeight3.Text = ""
        txt_Test1_Cone5_YarnWeight4.Text = ""
        txt_Test1_Cone5_YarnWeight5.Text = ""

        txt_Test1_Cone6_YarnWeight1.Text = ""
        txt_Test1_Cone6_YarnWeight2.Text = ""
        txt_Test1_Cone6_YarnWeight3.Text = ""
        txt_Test1_Cone6_YarnWeight4.Text = ""
        txt_Test1_Cone6_YarnWeight5.Text = ""

        lbl_Test1_Cone1_YarnCount1.Text = ""
        lbl_Test1_Cone1_YarnCount2.Text = ""
        lbl_Test1_Cone1_YarnCount3.Text = ""
        lbl_Test1_Cone1_YarnCount4.Text = ""
        lbl_Test1_Cone1_YarnCount5.Text = ""

        lbl_Test1_Cone2_YarnCount1.Text = ""
        lbl_Test1_Cone2_YarnCount2.Text = ""
        lbl_Test1_Cone2_YarnCount3.Text = ""
        lbl_Test1_Cone2_YarnCount4.Text = ""
        lbl_Test1_Cone2_YarnCount5.Text = ""

        lbl_Test1_Cone3_YarnCount1.Text = ""
        lbl_Test1_Cone3_YarnCount2.Text = ""
        lbl_Test1_Cone3_YarnCount3.Text = ""
        lbl_Test1_Cone3_YarnCount4.Text = ""
        lbl_Test1_Cone3_YarnCount5.Text = ""

        lbl_Test1_Cone4_YarnCount1.Text = ""
        lbl_Test1_Cone4_YarnCount2.Text = ""
        lbl_Test1_Cone4_YarnCount3.Text = ""
        lbl_Test1_Cone4_YarnCount4.Text = ""
        lbl_Test1_Cone4_YarnCount5.Text = ""

        lbl_Test1_Cone5_YarnCount1.Text = ""
        lbl_Test1_Cone5_YarnCount2.Text = ""
        lbl_Test1_Cone5_YarnCount3.Text = ""
        lbl_Test1_Cone5_YarnCount4.Text = ""
        lbl_Test1_Cone5_YarnCount5.Text = ""

        lbl_Test1_Cone6_YarnCount1.Text = ""
        lbl_Test1_Cone6_YarnCount2.Text = ""
        lbl_Test1_Cone6_YarnCount3.Text = ""
        lbl_Test1_Cone6_YarnCount4.Text = ""
        lbl_Test1_Cone6_YarnCount5.Text = ""


        txt_Test2_Cone1_YarnWeight1.Text = ""
        txt_Test2_Cone1_YarnWeight2.Text = ""
        txt_Test2_Cone1_YarnWeight3.Text = ""
        txt_Test2_Cone1_YarnWeight4.Text = ""
        txt_Test2_Cone1_YarnWeight5.Text = ""

        txt_Test2_Cone2_YarnWeight1.Text = ""
        txt_Test2_Cone2_YarnWeight2.Text = ""
        txt_Test2_Cone2_YarnWeight3.Text = ""
        txt_Test2_Cone2_YarnWeight4.Text = ""
        txt_Test2_Cone2_YarnWeight5.Text = ""


        txt_Test2_Cone3_YarnWeight1.Text = ""
        txt_Test2_Cone3_YarnWeight2.Text = ""
        txt_Test2_Cone3_YarnWeight3.Text = ""
        txt_Test2_Cone3_YarnWeight4.Text = ""
        txt_Test2_Cone3_YarnWeight5.Text = ""


        txt_Test2_Cone4_YarnWeight1.Text = ""
        txt_Test2_Cone4_YarnWeight2.Text = ""
        txt_Test2_Cone4_YarnWeight3.Text = ""
        txt_Test2_Cone4_YarnWeight4.Text = ""
        txt_Test2_Cone4_YarnWeight5.Text = ""

        txt_Test1_Cone5_YarnWeight1.Text = ""
        txt_Test1_Cone5_YarnWeight2.Text = ""
        txt_Test1_Cone5_YarnWeight3.Text = ""
        txt_Test1_Cone5_YarnWeight4.Text = ""
        txt_Test1_Cone5_YarnWeight5.Text = ""

        txt_Test2_Cone6_YarnWeight1.Text = ""
        txt_Test2_Cone6_YarnWeight2.Text = ""
        txt_Test2_Cone6_YarnWeight3.Text = ""
        txt_Test2_Cone6_YarnWeight4.Text = ""
        txt_Test2_Cone6_YarnWeight5.Text = ""

        lbl_Test2_Cone1_YarnCount1.Text = ""
        lbl_Test2_Cone1_YarnCount2.Text = ""
        lbl_Test2_Cone1_YarnCount3.Text = ""
        lbl_Test2_Cone1_YarnCount4.Text = ""
        lbl_Test2_Cone1_YarnCount5.Text = ""

        lbl_Test2_Cone2_YarnCount1.Text = ""
        lbl_Test2_Cone2_YarnCount2.Text = ""
        lbl_Test2_Cone2_YarnCount3.Text = ""
        lbl_Test2_Cone2_YarnCount4.Text = ""
        lbl_Test2_Cone2_YarnCount5.Text = ""

        lbl_Test2_Cone3_YarnCount1.Text = ""
        lbl_Test2_Cone3_YarnCount2.Text = ""
        lbl_Test2_Cone3_YarnCount3.Text = ""
        lbl_Test2_Cone3_YarnCount4.Text = ""
        lbl_Test2_Cone3_YarnCount5.Text = ""

        lbl_Test2_Cone4_YarnCount1.Text = ""
        lbl_Test2_Cone4_YarnCount2.Text = ""
        lbl_Test2_Cone4_YarnCount3.Text = ""
        lbl_Test2_Cone4_YarnCount4.Text = ""
        lbl_Test2_Cone4_YarnCount5.Text = ""

        lbl_Test2_Cone5_YarnCount1.Text = ""
        lbl_Test2_Cone5_YarnCount2.Text = ""
        lbl_Test2_Cone5_YarnCount3.Text = ""
        lbl_Test2_Cone5_YarnCount4.Text = ""
        lbl_Test2_Cone5_YarnCount5.Text = ""

        lbl_Test2_Cone6_YarnCount1.Text = ""
        lbl_Test2_Cone6_YarnCount2.Text = ""
        lbl_Test2_Cone6_YarnCount3.Text = ""
        lbl_Test2_Cone6_YarnCount4.Text = ""
        lbl_Test2_Cone6_YarnCount5.Text = ""


        txt_Test3_Cone1_YarnWeight1.Text = ""
        txt_Test3_Cone1_YarnWeight2.Text = ""
        txt_Test3_Cone1_YarnWeight3.Text = ""
        txt_Test3_Cone1_YarnWeight4.Text = ""
        txt_Test3_Cone1_YarnWeight5.Text = ""

        txt_Test3_Cone2_YarnWeight1.Text = ""
        txt_Test3_Cone2_YarnWeight2.Text = ""
        txt_Test3_Cone2_YarnWeight3.Text = ""
        txt_Test3_Cone2_YarnWeight4.Text = ""
        txt_Test3_Cone2_YarnWeight5.Text = ""


        txt_Test3_Cone3_YarnWeight1.Text = ""
        txt_Test3_Cone3_YarnWeight2.Text = ""
        txt_Test3_Cone3_YarnWeight3.Text = ""
        txt_Test3_Cone3_YarnWeight4.Text = ""
        txt_Test3_Cone3_YarnWeight5.Text = ""


        txt_Test3_Cone4_YarnWeight1.Text = ""
        txt_Test3_Cone4_YarnWeight2.Text = ""
        txt_Test3_Cone4_YarnWeight3.Text = ""
        txt_Test3_Cone4_YarnWeight4.Text = ""
        txt_Test3_Cone4_YarnWeight5.Text = ""

        txt_Test3_Cone5_YarnWeight1.Text = ""
        txt_Test3_Cone5_YarnWeight2.Text = ""
        txt_Test3_Cone5_YarnWeight3.Text = ""
        txt_Test3_Cone5_YarnWeight4.Text = ""
        txt_Test3_Cone5_YarnWeight5.Text = ""

        txt_Test3_Cone6_YarnWeight1.Text = ""
        txt_Test3_Cone6_YarnWeight2.Text = ""
        txt_Test3_Cone6_YarnWeight3.Text = ""
        txt_Test3_Cone6_YarnWeight4.Text = ""
        txt_Test3_Cone6_YarnWeight5.Text = ""

        lbl_Test3_Cone1_YarnCount1.Text = ""
        lbl_Test3_Cone1_YarnCount2.Text = ""
        lbl_Test3_Cone1_YarnCount3.Text = ""
        lbl_Test3_Cone1_YarnCount4.Text = ""
        lbl_Test3_Cone1_YarnCount5.Text = ""

        lbl_Test3_Cone2_YarnCount1.Text = ""
        lbl_Test3_Cone2_YarnCount2.Text = ""
        lbl_Test3_Cone2_YarnCount3.Text = ""
        lbl_Test3_Cone2_YarnCount4.Text = ""
        lbl_Test3_Cone2_YarnCount5.Text = ""

        lbl_Test3_Cone3_YarnCount1.Text = ""
        lbl_Test3_Cone3_YarnCount2.Text = ""
        lbl_Test3_Cone3_YarnCount3.Text = ""
        lbl_Test3_Cone3_YarnCount4.Text = ""
        lbl_Test3_Cone3_YarnCount5.Text = ""

        lbl_Test3_Cone4_YarnCount1.Text = ""
        lbl_Test3_Cone4_YarnCount2.Text = ""
        lbl_Test3_Cone4_YarnCount3.Text = ""
        lbl_Test3_Cone4_YarnCount4.Text = ""
        lbl_Test3_Cone4_YarnCount5.Text = ""

        lbl_Test3_Cone5_YarnCount1.Text = ""
        lbl_Test3_Cone5_YarnCount2.Text = ""
        lbl_Test3_Cone5_YarnCount3.Text = ""
        lbl_Test3_Cone5_YarnCount4.Text = ""
        lbl_Test3_Cone5_YarnCount5.Text = ""

        lbl_Test3_Cone6_YarnCount1.Text = ""
        lbl_Test3_Cone6_YarnCount2.Text = ""
        lbl_Test3_Cone6_YarnCount3.Text = ""
        lbl_Test3_Cone6_YarnCount4.Text = ""
        lbl_Test3_Cone6_YarnCount5.Text = ""

        lbl_Test1_Cone1_Mean.Text = ""
        lbl_Test1_Cone1_SD.Text = ""
        lbl_Test1_Cone1_CV.Text = ""

        lbl_Test1_Cone2_mean.Text = ""
        lbl_Test1_Cone2_SD.Text = ""
        lbl_Test1_Cone2_Cv.Text = ""

        lbl_Test1_Cone3_mean.Text = ""
        lbl_Test1_Cone3_SD.Text = ""
        lbl_Test1_Cone3_Cv.Text = ""

        lbl_Test1_Cone4_mean.Text = ""
        lbl_Test1_Cone4_SD.Text = ""
        lbl_Test1_Cone4_Cv.Text = ""

        lbl_Test1_Cone5_mean.Text = ""
        lbl_Test1_Cone5_SD.Text = ""
        lbl_Test1_Cone5_Cv.Text = ""

        lbl_Test1_Cone6_mean.Text = ""
        lbl_Test1_Cone6_Sd.Text = ""
        lbl_Test1_Cone6_Cv.Text = ""

        lbl_Test2_Cone1_Mean.Text = ""
        lbl_Test2_Cone1_SD.Text = ""
        lbl_Test2_Cone1_CV.Text = ""

        lbl_Test2_Cone2_Mean.Text = ""
        lbl_Test2_Cone2_SD.Text = ""
        lbl_Test2_Cone2_CV.Text = ""

        lbl_Test2_Cone3_Mean.Text = ""
        lbl_Test2_Cone3_SD.Text = ""
        lbl_Test2_Cone3_CV.Text = ""

        lbl_Test2_Cone4_Mean.Text = ""
        lbl_Test2_Cone4_SD.Text = ""
        lbl_Test2_Cone4_CV.Text = ""

        lbl_Test2_Cone5_Mean.Text = ""
        lbl_Test2_Cone5_SD.Text = ""
        lbl_Test2_Cone5_CV.Text = ""

        lbl_Test2_Cone6_Mean.Text = ""
        lbl_Test2_Cone6_SD.Text = ""
        lbl_Test2_Cone6_CV.Text = ""

        lbl_Test3_Cone1_Mean.Text = ""
        lbl_Test3_Cone1_SD.Text = ""
        lbl_Test3_Cone1_CV.Text = ""

        lbl_Test3_Cone2_mean.Text = ""
        lbl_Test3_Cone2_SD.Text = ""
        lbl_Test3_Cone2_CV.Text = ""

        lbl_Test3_Cone3_mean.Text = ""
        lbl_Test3_Cone3_SD.Text = ""
        lbl_Test3_Cone3_CV.Text = ""

        lbl_Test3_Cone4_mean.Text = ""
        lbl_Test3_Cone4_SD.Text = ""
        lbl_Test3_Cone4_CV.Text = ""

        lbl_Test3_Cone5_Mean.Text = ""
        lbl_Test3_Cone5_SD.Text = ""
        lbl_Test3_Cone5_CV.Text = ""

        lbl_Test3_Cone6_Mean.Text = ""
        lbl_Test3_Cone6_SD.Text = ""
        lbl_Test3_Cone6_CV.Text = ""

        lbl_Test1_AvgCv.Text = ""
        lbl_test1_AvgSd.Text = ""
        lbl_Test1_AvgCoul.Text = ""

        lbl_Test2_AvgCv.Text = ""
        lbl_Test2_Avg_Sd.Text = ""
        lbl_Test2_avgCoul.Text = ""

        lbl_Test3_AvgCv.Text = ""
        lbl_Test3_AvgSd.Text = ""
        lbl_Test3_AvgCoul.Text = ""


        txt_test1No.Text = ""
        txt_Test2No.Text = ""
        txt_test3No.Text = ""
        txt_Transport_Freight.Text = ""
        cbo_Type.Text = "DIRECT"

        dgv_Details.Rows.Clear()
        dgv_Details_Total.Rows.Clear()
        dgv_Details_Total.Rows.Add()

        If Filter_Status = False Then
            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_DeliveryAt.Text = ""
            txt_BillNo.Text = ""
            cbo_Filter_DeliveryAt.SelectedIndex = -1
            cbo_Filter_PartyName.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()
        End If

        Grid_Cell_DeSelect()

        cbo_Colour.Visible = False
        cbo_Colour.Tag = -1
        cbo_Grid_CountName.Visible = False
        cbo_Grid_MillName.Visible = False

        NoCalc_Status = False

    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim mskdtxbx As MaskedTextBox
        Dim combobx As ComboBox

        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Me.ActiveControl Is Button Or TypeOf Me.ActiveControl Is MaskedTextBox Then
            Me.ActiveControl.BackColor = Color.Lime
            Me.ActiveControl.ForeColor = Color.Blue
        End If

        If TypeOf Me.ActiveControl Is TextBox Then
            txtbx = Me.ActiveControl
            txtbx.SelectAll()
        ElseIf TypeOf Me.ActiveControl Is MaskedTextBox Then
            mskdtxbx = Me.ActiveControl
            mskdtxbx.SelectionStart = 0
        ElseIf TypeOf Me.ActiveControl Is ComboBox Then
            combobx = Me.ActiveControl
            combobx.SelectAll()
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
        If Me.ActiveControl.Name <> cbo_Colour.Name Then
            cbo_Colour.Visible = False
        End If

        Grid_Cell_DeSelect()

        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        On Error Resume Next
        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Or TypeOf Prec_ActCtrl Is MaskedTextBox Then
                Prec_ActCtrl.BackColor = Color.White
                Prec_ActCtrl.ForeColor = Color.Black
            ElseIf TypeOf Prec_ActCtrl Is Button Then
                Prec_ActCtrl.BackColor = Color.DeepPink
                Prec_ActCtrl.ForeColor = Color.White
            End If
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
        If Not IsNothing(dgv_Filter_Details.CurrentCell) Then dgv_Filter_Details.CurrentCell.Selected = False
        If Not IsNothing(dgv_Details_Total.CurrentCell) Then dgv_Details_Total.CurrentCell.Selected = False


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

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.* from Yarn_Purchase_Head a Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Yarn_Purchase_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_RefNo.Text = dt1.Rows(0).Item("Yarn_Purchase_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Yarn_Purchase_Date")
                msk_Date.Text = dtp_Date.Text

                cbo_PartyName.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Ledger_IdNo").ToString))

                cbo_Agent.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Agent_IdNo").ToString))
                cbo_PurchaseAc.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("PurchaseAc_IdNo").ToString))

                cbo_Delvat.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("DeliveryTo_Idno").ToString))
                txt_RecNo.Text = dt1.Rows(0).Item("Delivery_Receipt_No").ToString
                txt_BillNo.Text = dt1.Rows(0).Item("Bill_No").ToString
                dtp_BillDate.Text = dt1.Rows(0).Item("Bill_Date").ToString
                msk_BillDate.Text = dtp_BillDate.Text
                txt_CommRate.Text = Val(dt1.Rows(0).Item("Agent_Commission_Rate").ToString)
                cbo_CommType.Text = dt1.Rows(0).Item("Agent_Commission_Type").ToString
                lbl_CommAmount.Text = dt1.Rows(0).Item("Agent_Commission_Commission").ToString

                lbl_GrossAmount.Text = Format(Val(dt1.Rows(0).Item("Total_Amount").ToString), "#########0.00")
                txt_DiscPerc.Text = Val(dt1.Rows(0).Item("Discount_Percentage").ToString)
                lbl_DiscAmount.Text = Format(Val(dt1.Rows(0).Item("Discount_Amount").ToString), "#########0.00")
                txt_AssessableValue.Text = Format(Val(dt1.Rows(0).Item("Assessable_Value").ToString), "#########0.00")
                cbo_TaxType.Text = dt1.Rows(0).Item("Tax_Type").ToString
                If Trim(cbo_TaxType.Text) = "" Then cbo_TaxType.Text = "-NIL-"
                txt_TaxPerc.Text = Val(dt1.Rows(0).Item("Tax_Percentage").ToString)
                lbl_TaxAmount.Text = Format(Val(dt1.Rows(0).Item("Tax_Amount").ToString), "#########0.00")
                txt_AddLess_BeforeTax.Text = Format(Val(dt1.Rows(0).Item("AddLess_BeforeTax_Amount").ToString), "#########0.00")
                txt_Freight.Text = Format(Val(dt1.Rows(0).Item("Freight_Amount").ToString), "#########0.00")
                txt_AddLessAfterTax_Text.Text = dt1.Rows(0).Item("AddLessAfterTax_Text").ToString
                If Trim(txt_AddLessAfterTax_Text.Text) = "" Then txt_AddLessAfterTax_Text.Text = "Add/Less"
                txt_AddLess_AfterTax.Text = Format(Val(dt1.Rows(0).Item("AddLess_Amount").ToString), "#########0.00")
                lbl_RoundOff.Text = Format(Val(dt1.Rows(0).Item("RoundOff_Amount").ToString), "#########0.00")
                lbl_NetAmount.Text = Common_Procedures.Currency_Format(Val(dt1.Rows(0).Item("Net_Amount").ToString))

                cbo_VatAc.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("TaxAc_IdNo").ToString))
                cbo_Transport.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Transport_IdNo").ToString))
                txt_Note.Text = dt1.Rows(0).Item("Note").ToString
                cbo_VehicleNo.Text = dt1.Rows(0).Item("Vehicle_No").ToString

                txt_test1No.Text = dt1.Rows(0).Item("Test1_No").ToString
                txt_Test2No.Text = dt1.Rows(0).Item("Test2_No").ToString
                txt_test3No.Text = dt1.Rows(0).Item("Test3_No").ToString
                dtp_Test1Date.Text = dt1.Rows(0).Item("Test1_Date").ToString
                dtp_Test2Date.Text = dt1.Rows(0).Item("Test2_Date").ToString
                dtp_test3date.Text = dt1.Rows(0).Item("Test3_Date").ToString

                txt_Test1_Cone1_YarnWeight1.Text = Format(Val(dt1.Rows(0).Item("Test1_Cone1_YarnWeight1").ToString), "#########0.000")
                txt_Test1_Cone1_YarnWeight2.Text = Format(Val(dt1.Rows(0).Item("Test1_Cone1_YarnWeight2").ToString), "#########0.000")
                txt_Test1_Cone1_YarnWeight3.Text = Format(Val(dt1.Rows(0).Item("Test1_Cone1_YarnWeight3").ToString), "#########0.000")
                txt_Test1_Cone1_YarnWeight4.Text = Format(Val(dt1.Rows(0).Item("Test1_Cone1_YarnWeight4").ToString), "#########0.000")
                txt_Test1_Cone1_YarnWeight5.Text = Format(Val(dt1.Rows(0).Item("Test1_Cone1_YarnWeight5").ToString), "#########0.000")

                txt_Test1_Cone2_YarnWeight1.Text = Format(Val(dt1.Rows(0).Item("Test1_Cone2_YarnWeight1").ToString), "#########0.000")
                txt_Test1_Cone2_YarnWeight2.Text = Format(Val(dt1.Rows(0).Item("Test1_Cone2_YarnWeight2").ToString), "#########0.000")
                txt_Test1_Cone2_YarnWeight3.Text = Format(Val(dt1.Rows(0).Item("Test1_Cone2_YarnWeight3").ToString), "#########0.000")
                txt_Test1_Cone2_YarnWeight4.Text = Format(Val(dt1.Rows(0).Item("Test1_Cone2_YarnWeight4").ToString), "#########0.000")
                txt_Test1_Cone2_YarnWeight5.Text = Format(Val(dt1.Rows(0).Item("Test1_Cone2_YarnWeight5").ToString), "#########0.000")

                txt_Test1_Cone3_YarnWeight1.Text = Format(Val(dt1.Rows(0).Item("Test1_Cone3_YarnWeight1").ToString), "#########0.000")
                txt_Test1_Cone3_YarnWeight2.Text = Format(Val(dt1.Rows(0).Item("Test1_Cone3_YarnWeight2").ToString), "#########0.000")
                txt_Test1_Cone3_YarnWeight3.Text = Format(Val(dt1.Rows(0).Item("Test1_Cone3_YarnWeight3").ToString), "#########0.000")
                txt_Test1_Cone3_YarnWeight4.Text = Format(Val(dt1.Rows(0).Item("Test1_Cone3_YarnWeight4").ToString), "#########0.000")
                txt_Test1_Cone3_YarnWeight5.Text = Format(Val(dt1.Rows(0).Item("Test1_Cone3_YarnWeight5").ToString), "#########0.000")

                txt_Test1_Cone4_YarnWeight1.Text = Format(Val(dt1.Rows(0).Item("Test1_Cone4_YarnWeight1").ToString), "#########0.000")
                txt_Test1_Cone4_YarnWeight2.Text = Format(Val(dt1.Rows(0).Item("Test1_Cone4_YarnWeight2").ToString), "#########0.000")
                txt_Test1_Cone4_YarnWeight3.Text = Format(Val(dt1.Rows(0).Item("Test1_Cone4_YarnWeight3").ToString), "#########0.000")
                txt_Test1_Cone4_YarnWeight4.Text = Format(Val(dt1.Rows(0).Item("Test1_Cone4_YarnWeight4").ToString), "#########0.000")
                txt_Test1_Cone4_YarnWeight5.Text = Format(Val(dt1.Rows(0).Item("Test1_Cone4_YarnWeight5").ToString), "#########0.000")

                txt_Test1_Cone5_YarnWeight1.Text = Format(Val(dt1.Rows(0).Item("Test1_Cone5_YarnWeight1").ToString), "#########0.000")
                txt_Test1_Cone5_YarnWeight2.Text = Format(Val(dt1.Rows(0).Item("Test1_Cone5_YarnWeight2").ToString), "#########0.000")
                txt_Test1_Cone5_YarnWeight3.Text = Format(Val(dt1.Rows(0).Item("Test1_Cone5_YarnWeight3").ToString), "#########0.000")
                txt_Test1_Cone5_YarnWeight4.Text = Format(Val(dt1.Rows(0).Item("Test1_Cone5_YarnWeight4").ToString), "#########0.000")
                txt_Test1_Cone5_YarnWeight5.Text = Format(Val(dt1.Rows(0).Item("Test1_Cone5_YarnWeight5").ToString), "#########0.000")

                txt_Test1_Cone6_YarnWeight1.Text = Format(Val(dt1.Rows(0).Item("Test1_Cone6_YarnWeight1").ToString), "#########0.000")
                txt_Test1_Cone6_YarnWeight2.Text = Format(Val(dt1.Rows(0).Item("Test1_Cone6_YarnWeight2").ToString), "#########0.000")
                txt_Test1_Cone6_YarnWeight3.Text = Format(Val(dt1.Rows(0).Item("Test1_Cone6_YarnWeight3").ToString), "#########0.000")
                txt_Test1_Cone6_YarnWeight4.Text = Format(Val(dt1.Rows(0).Item("Test1_Cone6_YarnWeight4").ToString), "#########0.000")
                txt_Test1_Cone6_YarnWeight5.Text = Format(Val(dt1.Rows(0).Item("Test1_Cone6_YarnWeight5").ToString), "#########0.000")

                lbl_Test1_Cone1_YarnCount1.Text = Format(Val(dt1.Rows(0).Item("Test1_Cone1_yarnCount1").ToString), "#########0.0000")
                lbl_Test1_Cone1_YarnCount2.Text = Format(Val(dt1.Rows(0).Item("Test1_Cone1_yarnCount2").ToString), "#########0.0000")
                lbl_Test1_Cone1_YarnCount3.Text = Format(Val(dt1.Rows(0).Item("Test1_Cone1_yarnCount3").ToString), "#########0.0000")
                lbl_Test1_Cone1_YarnCount4.Text = Format(Val(dt1.Rows(0).Item("Test1_Cone1_yarnCount4").ToString), "#########0.0000")
                lbl_Test1_Cone1_YarnCount5.Text = Format(Val(dt1.Rows(0).Item("Test1_Cone1_yarnCount5").ToString), "#########0.0000")

                lbl_Test1_Cone2_YarnCount1.Text = Format(Val(dt1.Rows(0).Item("Test1_Cone2_yarnCount1").ToString), "#########0.0000")
                lbl_Test1_Cone2_YarnCount2.Text = Format(Val(dt1.Rows(0).Item("Test1_Cone2_yarnCount2").ToString), "#########0.0000")
                lbl_Test1_Cone2_YarnCount3.Text = Format(Val(dt1.Rows(0).Item("Test1_Cone2_yarnCount3").ToString), "#########0.0000")
                lbl_Test1_Cone2_YarnCount4.Text = Format(Val(dt1.Rows(0).Item("Test1_Cone2_yarnCount4").ToString), "#########0.0000")
                lbl_Test1_Cone2_YarnCount5.Text = Format(Val(dt1.Rows(0).Item("Test1_Cone2_yarnCount5").ToString), "#########0.0000")

                lbl_Test1_Cone3_YarnCount1.Text = Format(Val(dt1.Rows(0).Item("Test1_Cone3_yarnCount1").ToString), "#########0.0000")
                lbl_Test1_Cone3_YarnCount2.Text = Format(Val(dt1.Rows(0).Item("Test1_Cone3_yarnCount2").ToString), "#########0.0000")
                lbl_Test1_Cone3_YarnCount3.Text = Format(Val(dt1.Rows(0).Item("Test1_Cone3_yarnCount3").ToString), "#########0.0000")
                lbl_Test1_Cone3_YarnCount4.Text = Format(Val(dt1.Rows(0).Item("Test1_Cone3_yarnCount4").ToString), "#########0.0000")
                lbl_Test1_Cone3_YarnCount5.Text = Format(Val(dt1.Rows(0).Item("Test1_Cone3_yarnCount5").ToString), "#########0.0000")

                lbl_Test1_Cone4_YarnCount1.Text = Format(Val(dt1.Rows(0).Item("Test1_Cone4_yarnCount1").ToString), "#########0.0000")
                lbl_Test1_Cone4_YarnCount2.Text = Format(Val(dt1.Rows(0).Item("Test1_Cone4_yarnCount2").ToString), "#########0.0000")
                lbl_Test1_Cone4_YarnCount3.Text = Format(Val(dt1.Rows(0).Item("Test1_Cone4_yarnCount3").ToString), "#########0.0000")
                lbl_Test1_Cone4_YarnCount4.Text = Format(Val(dt1.Rows(0).Item("Test1_Cone4_yarnCount4").ToString), "#########0.0000")
                lbl_Test1_Cone4_YarnCount5.Text = Format(Val(dt1.Rows(0).Item("Test1_Cone4_yarnCount5").ToString), "#########0.0000")


                lbl_Test1_Cone5_YarnCount1.Text = Format(Val(dt1.Rows(0).Item("Test1_Cone5_yarnCount1").ToString), "#########0.0000")
                lbl_Test1_Cone5_YarnCount2.Text = Format(Val(dt1.Rows(0).Item("Test1_Cone5_yarnCount2").ToString), "#########0.0000")
                lbl_Test1_Cone5_YarnCount3.Text = Format(Val(dt1.Rows(0).Item("Test1_Cone5_yarnCount3").ToString), "#########0.0000")
                lbl_Test1_Cone5_YarnCount4.Text = Format(Val(dt1.Rows(0).Item("Test1_Cone5_yarnCount4").ToString), "#########0.0000")
                lbl_Test1_Cone5_YarnCount5.Text = Format(Val(dt1.Rows(0).Item("Test1_Cone5_yarnCount5").ToString), "#########0.0000")


                lbl_Test1_Cone6_YarnCount1.Text = Format(Val(dt1.Rows(0).Item("Test1_Cone6_yarnCount1").ToString), "#########0.0000")
                lbl_Test1_Cone6_YarnCount2.Text = Format(Val(dt1.Rows(0).Item("Test1_Cone6_yarnCount2").ToString), "#########0.0000")
                lbl_Test1_Cone6_YarnCount3.Text = Format(Val(dt1.Rows(0).Item("Test1_Cone6_yarnCount3").ToString), "#########0.0000")
                lbl_Test1_Cone6_YarnCount4.Text = Format(Val(dt1.Rows(0).Item("Test1_Cone6_yarnCount4").ToString), "#########0.0000")
                lbl_Test1_Cone6_YarnCount5.Text = Format(Val(dt1.Rows(0).Item("Test1_Cone6_yarnCount5").ToString), "#########0.0000")

                txt_Test2_Cone1_YarnWeight1.Text = Format(Val(dt1.Rows(0).Item("Test2_Cone1_YarnWeight1").ToString), "#########0.000")
                txt_Test2_Cone1_YarnWeight2.Text = Format(Val(dt1.Rows(0).Item("Test2_Cone1_YarnWeight2").ToString), "#########0.000")
                txt_Test2_Cone1_YarnWeight3.Text = Format(Val(dt1.Rows(0).Item("Test2_Cone1_YarnWeight3").ToString), "#########0.000")
                txt_Test2_Cone1_YarnWeight4.Text = Format(Val(dt1.Rows(0).Item("Test2_Cone1_YarnWeight4").ToString), "#########0.000")
                txt_Test2_Cone1_YarnWeight5.Text = Format(Val(dt1.Rows(0).Item("Test2_Cone1_YarnWeight5").ToString), "#########0.000")

                txt_Test2_Cone2_YarnWeight1.Text = Format(Val(dt1.Rows(0).Item("Test2_Cone2_YarnWeight1").ToString), "#########0.000")
                txt_Test2_Cone2_YarnWeight2.Text = Format(Val(dt1.Rows(0).Item("Test2_Cone2_YarnWeight2").ToString), "#########0.000")
                txt_Test2_Cone2_YarnWeight3.Text = Format(Val(dt1.Rows(0).Item("Test2_Cone2_YarnWeight3").ToString), "#########0.000")
                txt_Test2_Cone2_YarnWeight4.Text = Format(Val(dt1.Rows(0).Item("Test2_Cone2_YarnWeight4").ToString), "#########0.000")
                txt_Test2_Cone2_YarnWeight5.Text = Format(Val(dt1.Rows(0).Item("Test2_Cone2_YarnWeight5").ToString), "#########0.000")

                txt_Test2_Cone3_YarnWeight1.Text = Format(Val(dt1.Rows(0).Item("Test2_Cone3_YarnWeight1").ToString), "#########0.000")
                txt_Test2_Cone3_YarnWeight2.Text = Format(Val(dt1.Rows(0).Item("Test2_Cone3_YarnWeight2").ToString), "#########0.000")
                txt_Test2_Cone3_YarnWeight3.Text = Format(Val(dt1.Rows(0).Item("Test2_Cone3_YarnWeight3").ToString), "#########0.000")
                txt_Test2_Cone3_YarnWeight4.Text = Format(Val(dt1.Rows(0).Item("Test2_Cone3_YarnWeight4").ToString), "#########0.000")
                txt_Test2_Cone3_YarnWeight5.Text = Format(Val(dt1.Rows(0).Item("Test2_Cone3_YarnWeight5").ToString), "#########0.000")

                txt_Test2_Cone4_YarnWeight1.Text = Format(Val(dt1.Rows(0).Item("Test2_Cone4_YarnWeight1").ToString), "#########0.000")
                txt_Test2_Cone4_YarnWeight2.Text = Format(Val(dt1.Rows(0).Item("Test2_Cone4_YarnWeight2").ToString), "#########0.000")
                txt_Test2_Cone4_YarnWeight3.Text = Format(Val(dt1.Rows(0).Item("Test2_Cone4_YarnWeight3").ToString), "#########0.000")
                txt_Test2_Cone4_YarnWeight4.Text = Format(Val(dt1.Rows(0).Item("Test2_Cone4_YarnWeight4").ToString), "#########0.000")
                txt_Test2_Cone4_YarnWeight5.Text = Format(Val(dt1.Rows(0).Item("Test2_Cone4_YarnWeight5").ToString), "#########0.000")

                txt_Test2_Cone5_YarnWeight1.Text = Format(Val(dt1.Rows(0).Item("Test2_Cone5_YarnWeight1").ToString), "#########0.000")
                txt_Test2_Cone5_YarnWeight2.Text = Format(Val(dt1.Rows(0).Item("Test2_Cone5_YarnWeight2").ToString), "#########0.000")
                txt_Test2_Cone5_YarnWeight3.Text = Format(Val(dt1.Rows(0).Item("Test2_Cone5_YarnWeight3").ToString), "#########0.000")
                txt_Test2_Cone5_YarnWeight4.Text = Format(Val(dt1.Rows(0).Item("Test2_Cone5_YarnWeight4").ToString), "#########0.000")
                txt_Test2_Cone5_YarnWeight5.Text = Format(Val(dt1.Rows(0).Item("Test2_Cone5_YarnWeight5").ToString), "#########0.000")

                txt_Test2_Cone6_YarnWeight1.Text = Format(Val(dt1.Rows(0).Item("Test2_Cone6_YarnWeight1").ToString), "#########0.000")
                txt_Test2_Cone6_YarnWeight2.Text = Format(Val(dt1.Rows(0).Item("Test2_Cone6_YarnWeight2").ToString), "#########0.000")
                txt_Test2_Cone6_YarnWeight3.Text = Format(Val(dt1.Rows(0).Item("Test2_Cone6_YarnWeight3").ToString), "#########0.000")
                txt_Test2_Cone6_YarnWeight4.Text = Format(Val(dt1.Rows(0).Item("Test2_Cone6_YarnWeight4").ToString), "#########0.000")
                txt_Test2_Cone6_YarnWeight5.Text = Format(Val(dt1.Rows(0).Item("Test2_Cone6_YarnWeight5").ToString), "#########0.000")

                lbl_Test2_Cone1_YarnCount1.Text = Format(Val(dt1.Rows(0).Item("Test2_Cone1_yarnCount1").ToString), "#########0.0000")
                lbl_Test2_Cone1_YarnCount2.Text = Format(Val(dt1.Rows(0).Item("Test2_Cone1_yarnCount2").ToString), "#########0.0000")
                lbl_Test2_Cone1_YarnCount3.Text = Format(Val(dt1.Rows(0).Item("Test2_Cone1_yarnCount3").ToString), "#########0.0000")
                lbl_Test2_Cone1_YarnCount4.Text = Format(Val(dt1.Rows(0).Item("Test2_Cone1_yarnCount4").ToString), "#########0.0000")
                lbl_Test2_Cone1_YarnCount5.Text = Format(Val(dt1.Rows(0).Item("Test2_Cone1_yarnCount5").ToString), "#########0.0000")

                lbl_Test2_Cone2_YarnCount1.Text = Format(Val(dt1.Rows(0).Item("Test2_Cone2_yarnCount1").ToString), "#########0.0000")
                lbl_Test2_Cone2_YarnCount2.Text = Format(Val(dt1.Rows(0).Item("Test2_Cone2_yarnCount2").ToString), "#########0.0000")
                lbl_Test2_Cone2_YarnCount3.Text = Format(Val(dt1.Rows(0).Item("Test2_Cone2_yarnCount3").ToString), "#########0.0000")
                lbl_Test2_Cone2_YarnCount4.Text = Format(Val(dt1.Rows(0).Item("Test2_Cone2_yarnCount4").ToString), "#########0.0000")
                lbl_Test2_Cone2_YarnCount5.Text = Format(Val(dt1.Rows(0).Item("Test2_Cone2_yarnCount5").ToString), "#########0.0000")

                lbl_Test2_Cone3_YarnCount1.Text = Format(Val(dt1.Rows(0).Item("Test2_Cone3_yarnCount1").ToString), "#########0.0000")
                lbl_Test2_Cone3_YarnCount2.Text = Format(Val(dt1.Rows(0).Item("Test2_Cone3_yarnCount2").ToString), "#########0.0000")
                lbl_Test2_Cone3_YarnCount3.Text = Format(Val(dt1.Rows(0).Item("Test2_Cone3_yarnCount3").ToString), "#########0.0000")
                lbl_Test2_Cone3_YarnCount4.Text = Format(Val(dt1.Rows(0).Item("Test2_Cone3_yarnCount4").ToString), "#########0.0000")
                lbl_Test2_Cone3_YarnCount5.Text = Format(Val(dt1.Rows(0).Item("Test2_Cone3_yarnCount5").ToString), "#########0.0000")

                lbl_Test2_Cone4_YarnCount1.Text = Format(Val(dt1.Rows(0).Item("Test2_Cone4_yarnCount1").ToString), "#########0.0000")
                lbl_Test2_Cone4_YarnCount2.Text = Format(Val(dt1.Rows(0).Item("Test2_Cone4_yarnCount2").ToString), "#########0.0000")
                lbl_Test2_Cone4_YarnCount3.Text = Format(Val(dt1.Rows(0).Item("Test2_Cone4_yarnCount3").ToString), "#########0.0000")
                lbl_Test2_Cone4_YarnCount4.Text = Format(Val(dt1.Rows(0).Item("Test2_Cone4_yarnCount4").ToString), "#########0.0000")
                lbl_Test2_Cone4_YarnCount5.Text = Format(Val(dt1.Rows(0).Item("Test2_Cone4_yarnCount5").ToString), "#########0.0000")


                lbl_Test2_Cone5_YarnCount1.Text = Format(Val(dt1.Rows(0).Item("Test2_Cone5_yarnCount1").ToString), "#########0.0000")
                lbl_Test2_Cone5_YarnCount2.Text = Format(Val(dt1.Rows(0).Item("Test2_Cone5_yarnCount2").ToString), "#########0.0000")
                lbl_Test2_Cone5_YarnCount3.Text = Format(Val(dt1.Rows(0).Item("Test2_Cone5_yarnCount3").ToString), "#########0.0000")
                lbl_Test2_Cone5_YarnCount4.Text = Format(Val(dt1.Rows(0).Item("Test2_Cone5_yarnCount4").ToString), "#########0.0000")
                lbl_Test2_Cone5_YarnCount5.Text = Format(Val(dt1.Rows(0).Item("Test2_Cone5_yarnCount5").ToString), "#########0.0000")


                lbl_Test2_Cone6_YarnCount1.Text = Format(Val(dt1.Rows(0).Item("Test2_Cone6_yarnCount1").ToString), "#########0.0000")
                lbl_Test2_Cone5_YarnCount2.Text = Format(Val(dt1.Rows(0).Item("Test2_Cone6_yarnCount2").ToString), "#########0.0000")
                lbl_Test2_Cone6_YarnCount3.Text = Format(Val(dt1.Rows(0).Item("Test2_Cone6_yarnCount3").ToString), "#########0.0000")
                lbl_Test2_Cone6_YarnCount4.Text = Format(Val(dt1.Rows(0).Item("Test2_Cone6_yarnCount4").ToString), "#########0.0000")
                lbl_Test2_Cone6_YarnCount5.Text = Format(Val(dt1.Rows(0).Item("Test2_Cone6_yarnCount5").ToString), "#########0.0000")

                txt_Test3_Cone1_YarnWeight1.Text = Format(Val(dt1.Rows(0).Item("Test3_Cone1_YarnWeight1").ToString), "#########0.000")
                txt_Test3_Cone1_YarnWeight2.Text = Format(Val(dt1.Rows(0).Item("Test3_Cone1_YarnWeight2").ToString), "#########0.000")
                txt_Test3_Cone1_YarnWeight3.Text = Format(Val(dt1.Rows(0).Item("Test3_Cone1_YarnWeight3").ToString), "#########0.000")
                txt_Test3_Cone1_YarnWeight4.Text = Format(Val(dt1.Rows(0).Item("Test3_Cone1_YarnWeight4").ToString), "#########0.000")
                txt_Test3_Cone1_YarnWeight5.Text = Format(Val(dt1.Rows(0).Item("Test3_Cone1_YarnWeight5").ToString), "#########0.000")

                txt_Test3_Cone2_YarnWeight1.Text = Format(Val(dt1.Rows(0).Item("Test3_Cone2_YarnWeight1").ToString), "#########0.000")
                txt_Test3_Cone2_YarnWeight2.Text = Format(Val(dt1.Rows(0).Item("Test3_Cone2_YarnWeight2").ToString), "#########0.000")
                txt_Test3_Cone2_YarnWeight3.Text = Format(Val(dt1.Rows(0).Item("Test3_Cone2_YarnWeight3").ToString), "#########0.000")
                txt_Test3_Cone2_YarnWeight4.Text = Format(Val(dt1.Rows(0).Item("Test3_Cone2_YarnWeight4").ToString), "#########0.000")
                txt_Test3_Cone2_YarnWeight5.Text = Format(Val(dt1.Rows(0).Item("Test3_Cone2_YarnWeight5").ToString), "#########0.000")

                txt_Test3_Cone3_YarnWeight1.Text = Format(Val(dt1.Rows(0).Item("Test3_Cone3_YarnWeight1").ToString), "#########0.000")
                txt_Test3_Cone3_YarnWeight2.Text = Format(Val(dt1.Rows(0).Item("Test3_Cone3_YarnWeight2").ToString), "#########0.000")
                txt_Test3_Cone3_YarnWeight3.Text = Format(Val(dt1.Rows(0).Item("Test3_Cone3_YarnWeight3").ToString), "#########0.000")
                txt_Test3_Cone3_YarnWeight4.Text = Format(Val(dt1.Rows(0).Item("Test3_Cone3_YarnWeight4").ToString), "#########0.000")
                txt_Test3_Cone3_YarnWeight5.Text = Format(Val(dt1.Rows(0).Item("Test3_Cone3_YarnWeight5").ToString), "#########0.000")

                txt_Test3_Cone4_YarnWeight1.Text = Format(Val(dt1.Rows(0).Item("Test3_Cone4_YarnWeight1").ToString), "#########0.000")
                txt_Test3_Cone4_YarnWeight2.Text = Format(Val(dt1.Rows(0).Item("Test3_Cone4_YarnWeight2").ToString), "#########0.000")
                txt_Test3_Cone4_YarnWeight3.Text = Format(Val(dt1.Rows(0).Item("Test3_Cone4_YarnWeight3").ToString), "#########0.000")
                txt_Test3_Cone4_YarnWeight4.Text = Format(Val(dt1.Rows(0).Item("Test3_Cone4_YarnWeight4").ToString), "#########0.000")
                txt_Test3_Cone4_YarnWeight5.Text = Format(Val(dt1.Rows(0).Item("Test3_Cone4_YarnWeight5").ToString), "#########0.000")

                txt_Test3_Cone5_YarnWeight1.Text = Format(Val(dt1.Rows(0).Item("Test3_Cone5_YarnWeight1").ToString), "#########0.000")
                txt_Test3_Cone5_YarnWeight2.Text = Format(Val(dt1.Rows(0).Item("Test3_Cone5_YarnWeight2").ToString), "#########0.000")
                txt_Test3_Cone5_YarnWeight3.Text = Format(Val(dt1.Rows(0).Item("Test3_Cone5_YarnWeight3").ToString), "#########0.000")
                txt_Test3_Cone5_YarnWeight4.Text = Format(Val(dt1.Rows(0).Item("Test3_Cone5_YarnWeight4").ToString), "#########0.000")
                txt_Test3_Cone5_YarnWeight5.Text = Format(Val(dt1.Rows(0).Item("Test3_Cone5_YarnWeight5").ToString), "#########0.000")

                txt_Test3_Cone6_YarnWeight1.Text = Format(Val(dt1.Rows(0).Item("Test3_Cone6_YarnWeight1").ToString), "#########0.000")
                txt_Test3_Cone6_YarnWeight2.Text = Format(Val(dt1.Rows(0).Item("Test3_Cone6_YarnWeight2").ToString), "#########0.000")
                txt_Test3_Cone6_YarnWeight3.Text = Format(Val(dt1.Rows(0).Item("Test3_Cone6_YarnWeight3").ToString), "#########0.000")
                txt_Test3_Cone6_YarnWeight4.Text = Format(Val(dt1.Rows(0).Item("Test3_Cone6_YarnWeight4").ToString), "#########0.000")
                txt_Test3_Cone6_YarnWeight5.Text = Format(Val(dt1.Rows(0).Item("Test3_Cone6_YarnWeight5").ToString), "#########0.000")

                lbl_Test3_Cone1_YarnCount1.Text = Format(Val(dt1.Rows(0).Item("Test3_Cone1_yarnCount1").ToString), "#########0.0000")
                lbl_Test3_Cone1_YarnCount2.Text = Format(Val(dt1.Rows(0).Item("Test3_Cone1_yarnCount2").ToString), "#########0.0000")
                lbl_Test3_Cone1_YarnCount3.Text = Format(Val(dt1.Rows(0).Item("Test3_Cone1_yarnCount3").ToString), "#########0.0000")
                lbl_Test3_Cone1_YarnCount4.Text = Format(Val(dt1.Rows(0).Item("Test3_Cone1_yarnCount4").ToString), "#########0.0000")
                lbl_Test3_Cone1_YarnCount5.Text = Format(Val(dt1.Rows(0).Item("Test3_Cone1_yarnCount5").ToString), "#########0.0000")

                lbl_Test3_Cone2_YarnCount1.Text = Format(Val(dt1.Rows(0).Item("Test3_Cone2_yarnCount1").ToString), "#########0.0000")
                lbl_Test3_Cone2_YarnCount2.Text = Format(Val(dt1.Rows(0).Item("Test3_Cone2_yarnCount2").ToString), "#########0.0000")
                lbl_Test3_Cone2_YarnCount3.Text = Format(Val(dt1.Rows(0).Item("Test3_Cone2_yarnCount3").ToString), "#########0.0000")
                lbl_Test3_Cone2_YarnCount4.Text = Format(Val(dt1.Rows(0).Item("Test3_Cone2_yarnCount4").ToString), "#########0.0000")
                lbl_Test3_Cone2_YarnCount5.Text = Format(Val(dt1.Rows(0).Item("Test3_Cone2_yarnCount5").ToString), "#########0.0000")

                lbl_Test3_Cone3_YarnCount1.Text = Format(Val(dt1.Rows(0).Item("Test3_Cone3_yarnCount1").ToString), "#########0.0000")
                lbl_Test3_Cone3_YarnCount2.Text = Format(Val(dt1.Rows(0).Item("Test3_Cone3_yarnCount2").ToString), "#########0.0000")
                lbl_Test3_Cone3_YarnCount3.Text = Format(Val(dt1.Rows(0).Item("Test3_Cone3_yarnCount3").ToString), "#########0.0000")
                lbl_Test3_Cone3_YarnCount4.Text = Format(Val(dt1.Rows(0).Item("Test3_Cone3_yarnCount4").ToString), "#########0.0000")
                lbl_Test3_Cone3_YarnCount5.Text = Format(Val(dt1.Rows(0).Item("Test3_Cone3_yarnCount5").ToString), "#########0.0000")

                lbl_Test3_Cone4_YarnCount1.Text = Format(Val(dt1.Rows(0).Item("Test3_Cone4_yarnCount1").ToString), "#########0.0000")
                lbl_Test3_Cone4_YarnCount2.Text = Format(Val(dt1.Rows(0).Item("Test3_Cone4_yarnCount2").ToString), "#########0.0000")
                lbl_Test3_Cone4_YarnCount3.Text = Format(Val(dt1.Rows(0).Item("Test3_Cone4_yarnCount3").ToString), "#########0.0000")
                lbl_Test3_Cone4_YarnCount4.Text = Format(Val(dt1.Rows(0).Item("Test3_Cone4_yarnCount4").ToString), "#########0.0000")
                lbl_Test3_Cone4_YarnCount5.Text = Format(Val(dt1.Rows(0).Item("Test3_Cone4_yarnCount5").ToString), "#########0.0000")

                lbl_Test3_Cone5_YarnCount1.Text = Format(Val(dt1.Rows(0).Item("Test3_Cone5_yarnCount1").ToString), "#########0.0000")
                lbl_Test3_Cone5_YarnCount2.Text = Format(Val(dt1.Rows(0).Item("Test3_Cone5_yarnCount2").ToString), "#########0.0000")
                lbl_Test3_Cone5_YarnCount3.Text = Format(Val(dt1.Rows(0).Item("Test3_Cone5_yarnCount3").ToString), "#########0.0000")
                lbl_Test3_Cone5_YarnCount4.Text = Format(Val(dt1.Rows(0).Item("Test3_Cone5_yarnCount4").ToString), "#########0.0000")
                lbl_Test3_Cone5_YarnCount5.Text = Format(Val(dt1.Rows(0).Item("Test3_Cone5_yarnCount5").ToString), "#########0.0000")

                lbl_Test3_Cone6_YarnCount1.Text = Format(Val(dt1.Rows(0).Item("Test3_Cone6_yarnCount1").ToString), "#########0.0000")
                lbl_Test3_Cone5_YarnCount2.Text = Format(Val(dt1.Rows(0).Item("Test3_Cone6_yarnCount2").ToString), "#########0.0000")
                lbl_Test3_Cone6_YarnCount3.Text = Format(Val(dt1.Rows(0).Item("Test3_Cone6_yarnCount3").ToString), "#########0.0000")
                lbl_Test3_Cone6_YarnCount4.Text = Format(Val(dt1.Rows(0).Item("Test3_Cone6_yarnCount4").ToString), "#########0.0000")
                lbl_Test3_Cone6_YarnCount5.Text = Format(Val(dt1.Rows(0).Item("Test3_Cone6_yarnCount5").ToString), "#########0.0000")

                lbl_Test1_Cone1_Mean.Text = Format(Val(dt1.Rows(0).Item("Test1_Cone1_Mean").ToString), "#########0.0000")
                lbl_Test1_Cone1_SD.Text = Format(Val(dt1.Rows(0).Item("Test1_Cone1_Sd").ToString), "#########0.0000")
                lbl_Test1_Cone1_CV.Text = Format(Val(dt1.Rows(0).Item("Test1_Cone1_Cv").ToString), "#########0.0000")

                lbl_Test1_Cone2_mean.Text = Format(Val(dt1.Rows(0).Item("Test1_Cone2_Mean").ToString), "#########0.0000")
                lbl_Test1_Cone2_SD.Text = Format(Val(dt1.Rows(0).Item("Test1_Cone2_Sd").ToString), "#########0.0000")
                lbl_Test1_Cone2_Cv.Text = Format(Val(dt1.Rows(0).Item("Test1_Cone2_Cv").ToString), "#########0.0000")

                lbl_Test1_Cone3_mean.Text = Format(Val(dt1.Rows(0).Item("Test1_Cone3_Mean").ToString), "#########0.0000")
                lbl_Test1_Cone3_SD.Text = Format(Val(dt1.Rows(0).Item("Test1_Cone3_Sd").ToString), "#########0.0000")
                lbl_Test1_Cone3_Cv.Text = Format(Val(dt1.Rows(0).Item("Test1_Cone3_Cv").ToString), "#########0.0000")

                lbl_Test1_Cone4_mean.Text = Format(Val(dt1.Rows(0).Item("Test1_Cone4_Mean").ToString), "#########0.0000")
                lbl_Test1_Cone4_SD.Text = Format(Val(dt1.Rows(0).Item("Test1_Cone4_Sd").ToString), "#########0.0000")
                lbl_Test1_Cone4_Cv.Text = Format(Val(dt1.Rows(0).Item("Test1_Cone4_Cv").ToString), "#########0.0000")

                lbl_Test1_Cone5_mean.Text = Format(Val(dt1.Rows(0).Item("Test1_Cone5_Mean").ToString), "#########0.0000")
                lbl_Test1_Cone5_SD.Text = Format(Val(dt1.Rows(0).Item("Test1_Cone5_Sd").ToString), "#########0.0000")
                lbl_Test1_Cone5_Cv.Text = Format(Val(dt1.Rows(0).Item("Test1_Cone5_Cv").ToString), "#########0.0000")

                lbl_Test1_Cone6_mean.Text = Format(Val(dt1.Rows(0).Item("Test1_Cone6_Mean").ToString), "#########0.0000")
                lbl_Test1_Cone6_Sd.Text = Format(Val(dt1.Rows(0).Item("Test1_Cone6_Sd").ToString), "#########0.0000")
                lbl_Test1_Cone6_Cv.Text = Format(Val(dt1.Rows(0).Item("Test1_Cone6_Cv").ToString), "#########0.0000")


                lbl_Test2_Cone1_Mean.Text = Format(Val(dt1.Rows(0).Item("Test2_Cone1_Mean").ToString), "#########0.0000")
                lbl_Test2_Cone1_SD.Text = Format(Val(dt1.Rows(0).Item("Test2_Cone1_Sd").ToString), "#########0.0000")
                lbl_Test2_Cone1_CV.Text = Format(Val(dt1.Rows(0).Item("Test2_Cone1_Cv").ToString), "#########0.0000")

                lbl_Test2_Cone2_Mean.Text = Format(Val(dt1.Rows(0).Item("Test2_Cone2_Mean").ToString), "#########0.0000")
                lbl_Test2_Cone2_SD.Text = Format(Val(dt1.Rows(0).Item("Test2_Cone2_Sd").ToString), "#########0.0000")
                lbl_Test2_Cone2_CV.Text = Format(Val(dt1.Rows(0).Item("Test2_Cone2_Cv").ToString), "#########0.0000")

                lbl_Test2_Cone3_Mean.Text = Format(Val(dt1.Rows(0).Item("Test2_Cone3_Mean").ToString), "#########0.0000")
                lbl_Test2_Cone3_SD.Text = Format(Val(dt1.Rows(0).Item("Test2_Cone3_Sd").ToString), "#########0.0000")
                lbl_Test2_Cone3_CV.Text = Format(Val(dt1.Rows(0).Item("Test2_Cone3_Cv").ToString), "#########0.0000")

                lbl_Test2_Cone4_Mean.Text = Format(Val(dt1.Rows(0).Item("Test2_Cone4_Mean").ToString), "#########0.0000")
                lbl_Test2_Cone4_SD.Text = Format(Val(dt1.Rows(0).Item("Test2_Cone4_Sd").ToString), "#########0.0000")
                lbl_Test2_Cone4_CV.Text = Format(Val(dt1.Rows(0).Item("Test2_Cone4_Cv").ToString), "#########0.0000")

                lbl_Test2_Cone5_Mean.Text = Format(Val(dt1.Rows(0).Item("Test2_Cone5_Mean").ToString), "#########0.0000")
                lbl_Test2_Cone5_SD.Text = Format(Val(dt1.Rows(0).Item("Test2_Cone5_Sd").ToString), "#########0.0000")
                lbl_Test2_Cone5_CV.Text = Format(Val(dt1.Rows(0).Item("Test2_Cone5_Cv").ToString), "#########0.0000")

                lbl_Test2_Cone6_Mean.Text = Format(Val(dt1.Rows(0).Item("Test2_Cone6_Mean").ToString), "#########0.0000")
                lbl_Test2_Cone6_SD.Text = Format(Val(dt1.Rows(0).Item("Test2_Cone6_Sd").ToString), "#########0.0000")
                lbl_Test2_Cone6_CV.Text = Format(Val(dt1.Rows(0).Item("Test2_Cone6_Cv").ToString), "#########0.0000")

                lbl_Test3_Cone1_Mean.Text = Format(Val(dt1.Rows(0).Item("Test3_Cone1_Mean").ToString), "#########0.0000")
                lbl_Test3_Cone1_SD.Text = Format(Val(dt1.Rows(0).Item("Test3_Cone1_Sd").ToString), "#########0.0000")
                lbl_Test3_Cone1_CV.Text = Format(Val(dt1.Rows(0).Item("Test3_Cone1_Cv").ToString), "#########0.0000")

                lbl_Test3_Cone2_mean.Text = Format(Val(dt1.Rows(0).Item("Test3_Cone2_Mean").ToString), "#########0.0000")
                lbl_Test3_Cone2_SD.Text = Format(Val(dt1.Rows(0).Item("Test3_Cone2_Sd").ToString), "#########0.0000")
                lbl_Test3_Cone2_CV.Text = Format(Val(dt1.Rows(0).Item("Test3_Cone2_Cv").ToString), "#########0.0000")

                lbl_Test3_Cone3_mean.Text = Format(Val(dt1.Rows(0).Item("Test3_Cone3_Mean").ToString), "#########0.0000")
                lbl_Test3_Cone3_SD.Text = Format(Val(dt1.Rows(0).Item("Test3_Cone3_Sd").ToString), "#########0.0000")
                lbl_Test3_Cone3_CV.Text = Format(Val(dt1.Rows(0).Item("Test3_Cone3_Cv").ToString), "#########0.0000")

                lbl_Test3_Cone4_mean.Text = Format(Val(dt1.Rows(0).Item("Test3_Cone4_Mean").ToString), "#########0.0000")
                lbl_Test3_Cone4_SD.Text = Format(Val(dt1.Rows(0).Item("Test3_Cone4_Sd").ToString), "#########0.0000")
                lbl_Test3_Cone4_CV.Text = Format(Val(dt1.Rows(0).Item("Test3_Cone4_Cv").ToString), "#########0.0000")

                lbl_Test3_Cone5_Mean.Text = Format(Val(dt1.Rows(0).Item("Test3_Cone5_Mean").ToString), "#########0.0000")
                lbl_Test3_Cone5_SD.Text = Format(Val(dt1.Rows(0).Item("Test3_Cone5_Sd").ToString), "#########0.0000")
                lbl_Test3_Cone5_CV.Text = Format(Val(dt1.Rows(0).Item("Test3_Cone5_Cv").ToString), "#########0.0000")

                lbl_Test3_Cone6_Mean.Text = Format(Val(dt1.Rows(0).Item("Test3_Cone6_Mean").ToString), "#########0.0000")
                lbl_Test3_Cone6_SD.Text = Format(Val(dt1.Rows(0).Item("Test3_Cone6_Sd").ToString), "#########0.0000")
                lbl_Test3_Cone6_CV.Text = Format(Val(dt1.Rows(0).Item("Test3_Cone6_Cv").ToString), "#########0.0000")

                lbl_Test1_AvgCv.Text = Format(Val(dt1.Rows(0).Item("Test1_Avg_cV").ToString), "#########0.0000")
                lbl_test1_AvgSd.Text = Format(Val(dt1.Rows(0).Item("Test1_Avg_sD").ToString), "#########0.0000")
                lbl_Test1_AvgCoul.Text = Format(Val(dt1.Rows(0).Item("Test1_Avg_Coul").ToString), "#########0.0000")

                lbl_Test2_AvgCv.Text = Format(Val(dt1.Rows(0).Item("Test2_Avg_Cv").ToString), "#########0.0000")
                lbl_Test2_Avg_Sd.Text = Format(Val(dt1.Rows(0).Item("Test2_Avg_Sd").ToString), "#########0.0000")
                lbl_Test2_avgCoul.Text = Format(Val(dt1.Rows(0).Item("Test2_Avg_Coul").ToString), "#########0.0000")

                lbl_Test3_AvgCv.Text = Format(Val(dt1.Rows(0).Item("Test3_Avg_Cv").ToString), "#########0.0000")
                lbl_Test3_AvgSd.Text = Format(Val(dt1.Rows(0).Item("Test3_Avg_Sd").ToString), "#########0.0000")
                lbl_Test3_AvgCoul.Text = Format(Val(dt1.Rows(0).Item("Test3_Avg_Coul").ToString), "#########0.0000")
                txt_Transport_Freight.Text = Format(Val(dt1.Rows(0).Item("Transport_Freight").ToString), "#########0.00")
                lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))
                cbo_Type.Text = dt1.Rows(0).Item("Purchase_Type").ToString

                da2 = New SqlClient.SqlDataAdapter("Select a.*, b.Mill_Name, c.Count_name , D.Colour_Name from Yarn_Purchase_details a INNER JOIN Mill_Head b ON a.Mill_IdNo = b.Mill_IdNo INNER JOIN Count_Head c ON a.Count_IdNo = c.Count_IdNo LEFT OUTER JOIN COLOUR_Head d ON a.Colour_IdNo = d.Colour_IdNo Where a.Yarn_Purchase_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
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
                            If dgv_Details.Columns(9).Visible = True Then
                                .Rows(n).Cells(9).Value = dt2.Rows(i).Item("Colour_Name").ToString
                            End If
                            .Rows(n).Cells(10).Value = dt2.Rows(i).Item("Yarn_Purchase_Receipt_No").ToString
                            .Rows(n).Cells(11).Value = dt2.Rows(i).Item("Yarn_Purchase_Receipt_Code").ToString
                            .Rows(n).Cells(12).Value = Val(dt2.Rows(i).Item("Yarn_Purchase_Receipt_Details_SlNo").ToString)
                        Next i

                    End If

                    If .RowCount = 0 Then .Rows.Add()


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
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally

            dt1.Dispose()
            da1.Dispose()

            dt2.Dispose()
            da2.Dispose()

            If msk_Date.Visible And msk_Date.Enabled Then msk_Date.Focus()
            'If dtp_Date.Visible And dtp_Date.Enabled Then dtp_Date.Focus()
        End Try

        NoCalc_Status = False

    End Sub

    Private Sub Yarn_Purchase_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_PartyName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_PartyName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Agent.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "AGENT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Agent.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_PurchaseAc.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_PurchaseAc.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_VatAc.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_VatAc.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Delvat.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Delvat.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_CountName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "COUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_CountName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Colour.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "COLOUR" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Colour.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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
            'MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        FrmLdSTS = False

    End Sub

    Private Sub Yarn_Purchase_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
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

        Me.Text = ""

        con.Open()

        ' Common_Procedures.get_CashPartyName_From_All_Entries(con)

        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 ) order by Ledger_DisplayName", con)
        da.Fill(dt1)
        cbo_PartyName.DataSource = dt1
        cbo_PartyName.DisplayMember = "Ledger_DisplayName"

        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or Ledger_Type = 'GODOWN') order by Ledger_DisplayName", con)
        da.Fill(dt2)
        cbo_Delvat.DataSource = dt2
        cbo_Delvat.DisplayMember = "Ledger_DisplayName"

        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or Ledger_Type = 'AGENT') order by Ledger_DisplayName", con)
        da.Fill(dt3)
        cbo_Agent.DataSource = dt3
        cbo_Agent.DisplayMember = "Ledger_DisplayName"

        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or AccountsGroup_IdNo = 27 ) order by Ledger_DisplayName", con)
        da.Fill(dt4)
        cbo_PurchaseAc.DataSource = dt4
        cbo_PurchaseAc.DisplayMember = "Ledger_DisplayName"

        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or AccountsGroup_IdNo = 12 ) order by Ledger_DisplayName", con)
        da.Fill(dt5)
        cbo_VatAc.DataSource = dt5
        cbo_VatAc.DisplayMember = "Ledger_DisplayName"

        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or Ledger_Type = 'TRANSPORT') order by Ledger_DisplayName", con)
        da.Fill(dt6)
        cbo_Transport.DataSource = dt6
        cbo_Transport.DisplayMember = "Ledger_DisplayName"

        cbo_CommType.Items.Clear()
        cbo_CommType.Items.Add("BAG")
        cbo_CommType.Items.Add("%")

        'Common_Procedures.get_VehicleNo_From_All_Entries(con)

        da = New SqlClient.SqlDataAdapter("select distinct(Count_Name) from Count_Head order by Count_Name", con)
        da.Fill(dt7)
        cbo_Grid_CountName.DataSource = dt7
        cbo_Grid_CountName.DisplayMember = "Count_Name"

        da = New SqlClient.SqlDataAdapter("select distinct(Mill_Name) from Mill_Head order by Mill_Name", con)
        da.Fill(dt8)
        cbo_Grid_MillName.DataSource = dt8
        cbo_Grid_MillName.DisplayMember = "Mill_Name"

        da = New SqlClient.SqlDataAdapter("select distinct(Vehicle_No) from Yarn_Purchase_Head order by Vehicle_No", con)
        da.Fill(dt9)
        cbo_VehicleNo.DataSource = dt9
        cbo_VehicleNo.DisplayMember = "Vehicle_No"

        cbo_Grid_RateFor.Items.Clear()
        cbo_Grid_RateFor.Items.Add("BAG")
        cbo_Grid_RateFor.Items.Add("KG")

        cbo_TaxType.Items.Clear()
        cbo_TaxType.Items.Add("-NIL-")
        cbo_TaxType.Items.Add("VAT")
        cbo_TaxType.Items.Add("CST")


        cbo_Type.Items.Clear()
        cbo_Type.Items.Add(" ")
        cbo_Type.Items.Add("DIRECT")
        cbo_Type.Items.Add("RECEIPT")

        pnl_Selection.Visible = False
        pnl_Selection.Left = (Me.Width - pnl_Selection.Width) \ 2
        pnl_Selection.Top = (Me.Height - pnl_Selection.Height) \ 2
        pnl_Selection.BringToFront()


        dgv_Details.Columns(9).Visible = False
        If Val(Common_Procedures.settings.Bobin_Zari_Kuri_Entries_Status) = 1 Then

            dgv_Details.Columns(9).Visible = True

        Else

            dgv_Details.Columns(1).Width = dgv_Details.Columns(1).Width + 30
            dgv_Details.Columns(2).Width = dgv_Details.Columns(2).Width + 60
            dgv_Details.Columns(8).Width = dgv_Details.Columns(8).Width + 10

            dgv_Details_Total.Columns(1).Width = dgv_Details_Total.Columns(1).Width + 30
            dgv_Details_Total.Columns(2).Width = dgv_Details_Total.Columns(2).Width + 60
            dgv_Details_Total.Columns(8).Width = dgv_Details_Total.Columns(8).Width + 10
        End If

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2
        pnl_Filter.BringToFront()

        pnl_YarnTest.Visible = False
        pnl_YarnTest.Left = (Me.Width - pnl_YarnTest.Width) \ 2
        pnl_YarnTest.Top = (Me.Height - pnl_YarnTest.Height) \ 2
        pnl_YarnTest.BringToFront()

        txt_AssessableValue.Enabled = False
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1035" Then
            txt_AssessableValue.Enabled = True
        End If

        AddHandler msk_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_BillDate.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_BillDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Agent.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PurchaseAc.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_CountName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Colour.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_MillName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_RateFor.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Delvat.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_RecNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_BillNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_CommRate.GotFocus, AddressOf ControlGotFocus
        AddHandler lbl_CommAmount.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DiscPerc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_TaxPerc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Freight.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_AddLessAfterTax_Text.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_AddLess_AfterTax.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_AddLess_BeforeTax.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_TaxType.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_VatAc.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Transport.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Note.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_Fromdate.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_ToDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_DeliveryAt.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_FilterBillNo.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_save.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_close.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_AssessableValue.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Transport_Freight.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_VehicleNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Type.GotFocus, AddressOf ControlGotFocus

        AddHandler msk_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_BillDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Type.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_AssessableValue.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Agent.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_PurchaseAc.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_CountName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Colour.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_MillName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_RateFor.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Delvat.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_RecNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_BillNo.LostFocus, AddressOf ControlLostFocus
        AddHandler lbl_CommAmount.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_CommRate.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DiscPerc.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_TaxType.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_TaxPerc.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Freight.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_AddLessAfterTax_Text.LostFocus, AddressOf ControlLostFocus1
        AddHandler txt_AddLess_AfterTax.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_AddLess_BeforeTax.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_VatAc.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Transport.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Note.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_Fromdate.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_ToDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_DeliveryAt.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_FilterBillNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Transport_Freight.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_save.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_close.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_VehicleNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus

        'AddHandler msk_Date.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler dtp_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler msk_BillDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_BillDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_RecNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_FilterBillNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_BillNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_TaxPerc.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Freight.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_AddLessAfterTax_Text.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_AddLess_AfterTax.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Transport_Freight.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown


        'AddHandler msk_Date.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler msk_BillDate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_BillDate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_RecNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_BillNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_FilterBillNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_DiscPerc.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_TaxPerc.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Freight.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_AddLessAfterTax_Text.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_AddLess_AfterTax.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Transport_Freight.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_Fromdate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_ToDate.KeyPress, AddressOf TextBoxControlKeyPress


        '-----------------yarn test
        AddHandler txt_Test1_Cone1_YarnWeight1.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Test1_Cone1_YarnWeight2.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Test1_Cone1_YarnWeight3.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Test1_Cone1_YarnWeight4.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Test1_Cone1_YarnWeight5.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_Test1_Cone2_YarnWeight1.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Test1_Cone2_YarnWeight2.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Test1_Cone2_YarnWeight3.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Test1_Cone2_YarnWeight4.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Test1_Cone2_YarnWeight5.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_Test1_Cone3_YarnWeight1.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Test1_Cone3_YarnWeight2.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Test1_Cone3_YarnWeight3.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Test1_Cone3_YarnWeight4.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Test1_Cone3_YarnWeight5.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_Test1_Cone4_YarnWeight1.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Test1_Cone4_YarnWeight2.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Test1_Cone4_YarnWeight3.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Test1_Cone4_YarnWeight4.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Test1_Cone4_YarnWeight5.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_Test1_Cone5_YarnWeight1.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Test1_Cone5_YarnWeight2.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Test1_Cone5_YarnWeight3.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Test1_Cone5_YarnWeight4.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Test1_Cone5_YarnWeight5.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_Test1_Cone6_YarnWeight1.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Test1_Cone6_YarnWeight2.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Test1_Cone6_YarnWeight3.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Test1_Cone6_YarnWeight4.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Test1_Cone6_YarnWeight5.GotFocus, AddressOf ControlGotFocus


        AddHandler txt_Test2_Cone1_YarnWeight1.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Test2_Cone1_YarnWeight2.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Test2_Cone1_YarnWeight3.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Test2_Cone1_YarnWeight4.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Test2_Cone1_YarnWeight5.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_Test2_Cone2_YarnWeight1.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Test2_Cone2_YarnWeight2.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Test2_Cone2_YarnWeight3.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Test2_Cone2_YarnWeight4.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Test2_Cone2_YarnWeight5.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_Test2_Cone3_YarnWeight1.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Test2_Cone3_YarnWeight2.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Test2_Cone3_YarnWeight3.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Test2_Cone3_YarnWeight4.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Test2_Cone3_YarnWeight5.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_Test2_Cone4_YarnWeight1.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Test2_Cone4_YarnWeight2.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Test2_Cone4_YarnWeight3.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Test2_Cone4_YarnWeight4.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Test2_Cone4_YarnWeight5.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_Test2_Cone5_YarnWeight1.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Test2_Cone5_YarnWeight2.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Test2_Cone5_YarnWeight3.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Test2_Cone5_YarnWeight4.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Test2_Cone5_YarnWeight5.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_Test2_Cone6_YarnWeight1.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Test2_Cone6_YarnWeight2.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Test2_Cone6_YarnWeight3.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Test2_Cone6_YarnWeight4.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Test2_Cone6_YarnWeight5.GotFocus, AddressOf ControlGotFocus


        AddHandler txt_Test3_Cone1_YarnWeight1.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Test3_Cone1_YarnWeight2.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Test3_Cone1_YarnWeight3.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Test3_Cone1_YarnWeight4.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Test3_Cone1_YarnWeight5.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_Test3_Cone2_YarnWeight1.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Test3_Cone2_YarnWeight2.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Test3_Cone2_YarnWeight3.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Test3_Cone2_YarnWeight4.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Test3_Cone2_YarnWeight5.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_Test3_Cone3_YarnWeight1.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Test3_Cone3_YarnWeight2.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Test2_Cone3_YarnWeight3.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Test3_Cone3_YarnWeight4.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Test3_Cone3_YarnWeight5.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_Test3_Cone4_YarnWeight1.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Test3_Cone4_YarnWeight2.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Test3_Cone4_YarnWeight3.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Test3_Cone4_YarnWeight4.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Test3_Cone4_YarnWeight5.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_Test3_Cone5_YarnWeight1.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Test3_Cone5_YarnWeight2.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Test3_Cone5_YarnWeight3.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Test3_Cone5_YarnWeight4.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Test3_Cone5_YarnWeight5.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_Test3_Cone6_YarnWeight1.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Test3_Cone6_YarnWeight2.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Test3_Cone6_YarnWeight3.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Test3_Cone6_YarnWeight4.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Test3_Cone6_YarnWeight5.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_test1No.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Test2No.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_test3No.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_Test1_Cone1_YarnWeight1.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Test1_Cone1_YarnWeight2.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Test1_Cone1_YarnWeight3.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Test1_Cone1_YarnWeight4.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Test1_Cone1_YarnWeight5.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_Test1_Cone2_YarnWeight1.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Test1_Cone2_YarnWeight2.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Test1_Cone2_YarnWeight3.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Test1_Cone2_YarnWeight4.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Test1_Cone2_YarnWeight5.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_Test1_Cone3_YarnWeight1.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Test1_Cone3_YarnWeight2.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Test1_Cone3_YarnWeight3.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Test1_Cone3_YarnWeight4.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Test1_Cone3_YarnWeight5.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_Test1_Cone4_YarnWeight1.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Test1_Cone4_YarnWeight2.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Test1_Cone4_YarnWeight3.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Test1_Cone4_YarnWeight4.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Test1_Cone4_YarnWeight5.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_Test1_Cone5_YarnWeight1.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Test1_Cone5_YarnWeight2.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Test1_Cone5_YarnWeight3.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Test1_Cone5_YarnWeight4.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Test1_Cone5_YarnWeight5.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_Test1_Cone6_YarnWeight1.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Test1_Cone6_YarnWeight2.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Test1_Cone6_YarnWeight3.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Test1_Cone6_YarnWeight4.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Test1_Cone6_YarnWeight5.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_Test2_Cone1_YarnWeight1.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Test2_Cone1_YarnWeight2.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Test2_Cone1_YarnWeight3.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Test2_Cone1_YarnWeight4.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Test2_Cone1_YarnWeight5.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_Test2_Cone2_YarnWeight1.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Test2_Cone2_YarnWeight2.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Test2_Cone2_YarnWeight3.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Test2_Cone2_YarnWeight4.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Test2_Cone2_YarnWeight5.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_Test2_Cone3_YarnWeight1.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Test2_Cone3_YarnWeight2.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Test2_Cone3_YarnWeight3.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Test2_Cone3_YarnWeight4.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Test2_Cone3_YarnWeight5.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_Test2_Cone4_YarnWeight1.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Test2_Cone4_YarnWeight2.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Test2_Cone4_YarnWeight3.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Test2_Cone4_YarnWeight4.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Test2_Cone4_YarnWeight5.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_Test2_Cone5_YarnWeight1.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Test2_Cone5_YarnWeight2.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Test2_Cone5_YarnWeight3.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Test2_Cone5_YarnWeight4.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Test2_Cone5_YarnWeight5.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_Test2_Cone6_YarnWeight1.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Test2_Cone6_YarnWeight2.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Test2_Cone6_YarnWeight3.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Test2_Cone6_YarnWeight4.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Test2_Cone6_YarnWeight5.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_Test3_Cone1_YarnWeight1.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Test3_Cone1_YarnWeight2.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Test3_Cone1_YarnWeight3.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Test3_Cone1_YarnWeight4.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Test3_Cone1_YarnWeight5.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_Test3_Cone2_YarnWeight1.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Test3_Cone2_YarnWeight2.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Test3_Cone2_YarnWeight3.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Test3_Cone2_YarnWeight4.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Test3_Cone2_YarnWeight5.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_Test3_Cone3_YarnWeight1.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Test3_Cone3_YarnWeight2.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Test3_Cone3_YarnWeight3.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Test3_Cone3_YarnWeight4.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Test3_Cone3_YarnWeight5.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_Test3_Cone4_YarnWeight1.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Test3_Cone4_YarnWeight2.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Test3_Cone4_YarnWeight3.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Test3_Cone4_YarnWeight4.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Test3_Cone4_YarnWeight5.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_Test3_Cone5_YarnWeight1.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Test3_Cone5_YarnWeight2.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Test3_Cone5_YarnWeight3.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Test3_Cone5_YarnWeight4.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Test3_Cone5_YarnWeight5.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_Test3_Cone6_YarnWeight1.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Test3_Cone6_YarnWeight2.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Test3_Cone6_YarnWeight3.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Test3_Cone6_YarnWeight4.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Test3_Cone6_YarnWeight5.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_test1No.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Test2No.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_test3No.LostFocus, AddressOf ControlLostFocus

        AddHandler dtp_Test1Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Test2Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_test3date.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_Test1_Cone1_YarnWeight1.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Test1_Cone1_YarnWeight2.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Test1_Cone1_YarnWeight3.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Test1_Cone1_YarnWeight4.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Test1_Cone1_YarnWeight5.KeyDown, AddressOf TextBoxControlKeyDown


        AddHandler txt_Test1_Cone2_YarnWeight1.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Test1_Cone2_YarnWeight2.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Test1_Cone2_YarnWeight3.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Test1_Cone2_YarnWeight4.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Test1_Cone2_YarnWeight5.KeyDown, AddressOf TextBoxControlKeyDown


        AddHandler txt_Test1_Cone3_YarnWeight1.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Test1_Cone3_YarnWeight2.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Test1_Cone3_YarnWeight3.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Test1_Cone3_YarnWeight4.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Test1_Cone3_YarnWeight5.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_Test1_Cone4_YarnWeight1.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Test1_Cone4_YarnWeight2.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Test1_Cone4_YarnWeight3.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Test1_Cone4_YarnWeight4.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Test1_Cone4_YarnWeight5.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_Test1_Cone5_YarnWeight1.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Test1_Cone5_YarnWeight2.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Test1_Cone5_YarnWeight3.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Test1_Cone5_YarnWeight4.KeyDown, AddressOf TextBoxControlKeyDown
        '  AddHandler txt_Test1_Cone5_YarnWeight5.KeyDown, AddressOf TextBoxControlKeyDown


        AddHandler txt_Test1_Cone6_YarnWeight1.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Test1_Cone6_YarnWeight2.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Test1_Cone6_YarnWeight3.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Test1_Cone6_YarnWeight4.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Test1_Cone6_YarnWeight5.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_Test2_Cone1_YarnWeight1.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Test2_Cone1_YarnWeight2.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Test2_Cone1_YarnWeight3.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Test2_Cone1_YarnWeight4.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Test2_Cone1_YarnWeight5.KeyDown, AddressOf TextBoxControlKeyDown


        AddHandler txt_Test2_Cone2_YarnWeight1.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Test2_Cone2_YarnWeight2.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Test2_Cone2_YarnWeight3.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Test2_Cone2_YarnWeight4.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Test2_Cone2_YarnWeight5.KeyDown, AddressOf TextBoxControlKeyDown


        AddHandler txt_Test2_Cone3_YarnWeight1.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Test2_Cone3_YarnWeight2.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Test2_Cone3_YarnWeight3.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Test2_Cone3_YarnWeight4.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Test2_Cone3_YarnWeight5.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_Test2_Cone4_YarnWeight1.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Test2_Cone4_YarnWeight2.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Test2_Cone4_YarnWeight3.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Test2_Cone4_YarnWeight4.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Test2_Cone4_YarnWeight5.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_Test2_Cone5_YarnWeight1.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Test2_Cone5_YarnWeight2.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Test2_Cone5_YarnWeight3.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Test2_Cone5_YarnWeight4.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Test2_Cone5_YarnWeight5.KeyDown, AddressOf TextBoxControlKeyDown


        AddHandler txt_Test2_Cone6_YarnWeight1.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Test2_Cone6_YarnWeight2.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Test2_Cone6_YarnWeight3.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Test2_Cone6_YarnWeight4.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler txt_Test2_Cone6_YarnWeight5.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_Test3_Cone1_YarnWeight1.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Test3_Cone1_YarnWeight2.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Test3_Cone1_YarnWeight3.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Test3_Cone1_YarnWeight4.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Test3_Cone1_YarnWeight5.KeyDown, AddressOf TextBoxControlKeyDown


        AddHandler txt_Test3_Cone2_YarnWeight1.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Test3_Cone2_YarnWeight2.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Test3_Cone2_YarnWeight3.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Test3_Cone2_YarnWeight4.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Test3_Cone2_YarnWeight5.KeyDown, AddressOf TextBoxControlKeyDown


        AddHandler txt_Test3_Cone3_YarnWeight1.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Test3_Cone3_YarnWeight2.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Test3_Cone3_YarnWeight3.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Test3_Cone3_YarnWeight4.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Test3_Cone3_YarnWeight5.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_Test3_Cone4_YarnWeight1.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Test3_Cone4_YarnWeight2.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Test3_Cone4_YarnWeight3.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Test3_Cone4_YarnWeight4.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Test3_Cone4_YarnWeight5.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_Test3_Cone5_YarnWeight1.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Test3_Cone5_YarnWeight2.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Test3_Cone5_YarnWeight3.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Test3_Cone5_YarnWeight4.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Test3_Cone5_YarnWeight5.KeyDown, AddressOf TextBoxControlKeyDown


        AddHandler txt_Test3_Cone6_YarnWeight1.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Test3_Cone6_YarnWeight2.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Test3_Cone6_YarnWeight3.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Test3_Cone6_YarnWeight4.KeyDown, AddressOf TextBoxControlKeyDown
        '' AddHandler txt_Test3_Cone6_YarnWeight5.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler dtp_Test1Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Test2Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_test3date.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler txt_Test1_Cone1_YarnWeight1.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Test1_Cone1_YarnWeight2.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Test1_Cone1_YarnWeight3.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Test1_Cone1_YarnWeight4.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Test1_Cone1_YarnWeight5.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler txt_Test1_Cone2_YarnWeight1.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Test1_Cone2_YarnWeight2.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Test1_Cone2_YarnWeight3.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Test1_Cone2_YarnWeight4.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Test1_Cone2_YarnWeight5.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler txt_Test1_Cone3_YarnWeight1.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Test1_Cone3_YarnWeight2.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Test1_Cone3_YarnWeight3.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Test1_Cone3_YarnWeight4.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Test1_Cone3_YarnWeight5.KeyPress, AddressOf TextBoxControlKeyPress


        AddHandler txt_Test1_Cone4_YarnWeight1.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Test1_Cone4_YarnWeight2.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Test1_Cone4_YarnWeight3.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Test1_Cone4_YarnWeight4.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Test1_Cone4_YarnWeight5.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler txt_Test1_Cone5_YarnWeight1.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Test1_Cone5_YarnWeight2.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Test1_Cone5_YarnWeight3.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Test1_Cone5_YarnWeight4.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Test1_Cone5_YarnWeight5.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler txt_Test1_Cone6_YarnWeight1.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Test1_Cone6_YarnWeight2.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Test1_Cone6_YarnWeight3.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Test1_Cone6_YarnWeight4.KeyPress, AddressOf TextBoxControlKeyPress
        ' AddHandler txt_Test1_Cone6_YarnWeight5.KeyPress, AddressOf TextBoxControlKeyPress


        AddHandler txt_Test2_Cone1_YarnWeight1.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Test2_Cone1_YarnWeight2.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Test2_Cone1_YarnWeight3.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Test2_Cone1_YarnWeight4.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Test2_Cone1_YarnWeight5.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler txt_Test2_Cone2_YarnWeight1.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Test2_Cone2_YarnWeight2.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Test2_Cone2_YarnWeight3.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Test2_Cone2_YarnWeight4.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Test2_Cone2_YarnWeight5.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler txt_Test2_Cone3_YarnWeight1.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Test2_Cone3_YarnWeight2.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Test2_Cone3_YarnWeight3.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Test2_Cone3_YarnWeight4.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Test2_Cone3_YarnWeight5.KeyPress, AddressOf TextBoxControlKeyPress


        AddHandler txt_Test2_Cone4_YarnWeight1.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Test2_Cone4_YarnWeight2.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Test2_Cone4_YarnWeight3.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Test2_Cone4_YarnWeight4.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Test2_Cone4_YarnWeight5.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler txt_Test2_Cone5_YarnWeight1.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Test2_Cone5_YarnWeight2.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Test2_Cone5_YarnWeight3.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Test2_Cone5_YarnWeight4.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Test2_Cone5_YarnWeight5.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler txt_Test2_Cone6_YarnWeight1.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Test2_Cone6_YarnWeight2.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Test2_Cone6_YarnWeight3.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Test2_Cone6_YarnWeight4.KeyPress, AddressOf TextBoxControlKeyPress
        '  AddHandler txt_Test2_Cone6_YarnWeight5.KeyPress, AddressOf TextBoxControlKeyPress


        AddHandler txt_Test3_Cone1_YarnWeight1.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Test3_Cone1_YarnWeight2.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Test3_Cone1_YarnWeight3.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Test3_Cone1_YarnWeight4.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Test3_Cone1_YarnWeight5.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler txt_Test3_Cone2_YarnWeight1.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Test3_Cone2_YarnWeight2.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Test3_Cone2_YarnWeight3.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Test3_Cone2_YarnWeight4.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Test3_Cone2_YarnWeight5.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler txt_Test3_Cone3_YarnWeight1.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Test3_Cone3_YarnWeight2.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Test3_Cone3_YarnWeight3.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Test3_Cone3_YarnWeight4.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Test3_Cone3_YarnWeight5.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler txt_Test3_Cone4_YarnWeight1.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Test3_Cone4_YarnWeight2.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Test3_Cone4_YarnWeight3.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Test3_Cone4_YarnWeight4.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Test3_Cone4_YarnWeight5.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler txt_Test3_Cone5_YarnWeight1.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Test3_Cone5_YarnWeight2.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Test3_Cone5_YarnWeight3.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Test3_Cone5_YarnWeight4.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Test3_Cone5_YarnWeight5.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler txt_Test3_Cone6_YarnWeight1.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Test3_Cone6_YarnWeight2.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Test3_Cone6_YarnWeight3.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Test3_Cone6_YarnWeight4.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_test1No.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Test2No.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_test3No.KeyPress, AddressOf TextBoxControlKeyPress

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True
        new_record()

    End Sub

    Private Sub Yarn_Purchase_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub Yarn_purchase_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        Try

            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
                    Exit Sub

                ElseIf pnl_YarnTest.Visible = True Then
                    btn_YarntestClose_Click(sender, e)
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
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

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
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
        Dim dgv1 As New DataGridView

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

            With dgv1
                If keyData = Keys.Enter Or keyData = Keys.Down Then

                    If dgv_Details.Columns(9).Visible = True Then

                        If .CurrentCell.ColumnIndex >= .ColumnCount - 4 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                txt_DiscPerc.Focus()

                            Else
                                If Trim(UCase(cbo_Type.Text)) = "RECEIPT" Then
                                    .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(3)
                                Else
                                    .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)
                                End If

                            End If
                        ElseIf .CurrentCell.ColumnIndex = 7 Then
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(9)
                        Else

                            If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then
                                txt_DiscPerc.Focus()

                            Else
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                            End If

                        End If

                        Return True
                    Else
                        If .CurrentCell.ColumnIndex >= .ColumnCount - 6 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                txt_DiscPerc.Focus()

                            Else
                                If Trim(UCase(cbo_Type.Text)) = "RECEIPT" Then
                                    .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(3)
                                Else
                                    .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)
                                End If


                            End If

                        Else

                            If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then
                                txt_DiscPerc.Focus()

                            Else
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                            End If

                        End If

                        Return True
                    End If

                ElseIf keyData = Keys.Up Then
                    If dgv_Details.Columns(9).Visible = True Then

                        If .CurrentCell.ColumnIndex <= 1 Then
                            If .CurrentCell.RowIndex = 0 Then
                                lbl_CommAmount.Focus()

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(9)

                            End If
                        ElseIf .CurrentCell.ColumnIndex = 3 Then
                            If Trim(UCase(cbo_Type.Text)) = "RECEIPT" Then
                                txt_CommRate.Focus()
                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(2)
                            End If
                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)

                        End If
                    Else
                        If .CurrentCell.ColumnIndex <= 1 Then
                            If .CurrentCell.RowIndex = 0 Then
                                lbl_CommAmount.Focus()

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(7)

                            End If
                        ElseIf .CurrentCell.ColumnIndex = 3 Then
                            If Trim(UCase(cbo_Type.Text)) = "RECEIPT" Then
                                txt_CommRate.Focus()
                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(2)
                            End If
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

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Yarn_Purchase_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Yarn_Purchase_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.Yarn_Purchase_Entry, New_Entry, Me, con, "Yarn_Purchase_Head", "Yarn_Purchase_Code", NewCode, "Yarn_Purchase_Date", "(Yarn_Purchase_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub


        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If
        If DeleteAll_STS <> True Then

        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If
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
            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition3) & Trim(NewCode), trans)
            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition4) & Trim(NewCode), trans)


            cmd.CommandText = "Update Yarn_Purchase_Receipt_Details set Purchase_Bags = a.Purchase_Bags - b.Bags,Purchase_Cones = a.Purchase_Cones - b.Cones,Purchase_Weight = a.Purchase_Weight - b.Weight from Yarn_Purchase_Receipt_Details a, Yarn_Purchase_Details b, Yarn_Purchase_Head c Where b.Yarn_Purchase_Code = '" & Trim(NewCode) & "' and c.Yarn_Purchase_Code = '" & Trim(NewCode) & "' and c.Purchase_Type = 'RECEIPT' and b.Yarn_Purchase_Code = c.Yarn_Purchase_Code and a.Yarn_Purchase_Receipt_Code = b.Yarn_Purchase_Receipt_Code and a.Yarn_Purchase_Receipt_Details_SlNo = b.Yarn_Purchase_Receipt_Details_SlNo"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from AgentCommission_Processing_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Empty_BeamBagCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Yarn_Purchase_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Purchase_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "delete from Yarn_Purchase_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Purchase_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            trans.Commit()

            If DeleteAll_STS <> True Then
            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
            End If

        Catch ex As Exception
            trans.Rollback()
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally

            Dt1.Dispose()
            Da.Dispose()
            trans.Dispose()
            cmd.Dispose()

            If msk_Date.Enabled = True And msk_Date.Visible = True Then msk_Date.Focus()
            'If dtp_Date.Enabled = True And dtp_Date.Visible = True Then dtp_Date.Focus()
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

            da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or Ledger_Type = 'GODOWN') order by Ledger_DisplayName", con)
            da.Fill(dt2)
            cbo_Filter_DeliveryAt.DataSource = dt2
            cbo_Filter_DeliveryAt.DisplayMember = "Ledger_DisplayName"


            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_DeliveryAt.Text = ""
            txt_FilterBillNo.Text = ""

            cbo_Filter_DeliveryAt.SelectedIndex = -1
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

            da = New SqlClient.SqlDataAdapter("select top 1 Yarn_Purchase_No from Yarn_Purchase_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " AND Entry_VAT_GST_Type <> 'GST' and Yarn_Purchase_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Yarn_Purchase_No", con)
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
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Yarn_Purchase_No from Yarn_Purchase_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " AND Entry_VAT_GST_Type <> 'GST' and Yarn_Purchase_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Yarn_Purchase_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Yarn_Purchase_No from Yarn_Purchase_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " AND Entry_VAT_GST_Type <> 'GST' and Yarn_Purchase_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Yarn_Purchase_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 Yarn_Purchase_No from Yarn_Purchase_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " AND Entry_VAT_GST_Type <> 'GST' and Yarn_Purchase_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Yarn_Purchase_No desc", con)
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

            lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Yarn_Purchase_Head", "Yarn_Purchase_Code", "For_OrderBy", "Entry_VAT_GST_Type <> 'GST'", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)
            lbl_RefNo.ForeColor = Color.Red


            msk_Date.Text = Date.Today.ToShortDateString
            Da1 = New SqlClient.SqlDataAdapter("select Top 1 a.*, b.ledger_name as PurchaseAcName, c.ledger_name as TaxAcName from Yarn_Purchase_Head a LEFT OUTER JOIN Ledger_Head b ON a.PurchaseAc_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Ledger_Head c ON a.TaxAc_IdNo = c.Ledger_IdNo where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Entry_VAT_GST_Type <> 'GST' AND  a.Yarn_Purchase_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by a.for_Orderby desc, a.Yarn_Purchase_No desc", con)
            Da1.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then
                If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)
                    If Dt1.Rows(0).Item("Yarn_Purchase_Date").ToString <> "" Then msk_Date.Text = Dt1.Rows(0).Item("Yarn_Purchase_Date").ToString
                End If
                If Dt1.Rows(0).Item("PurchaseAcName").ToString <> "" Then cbo_PurchaseAc.Text = Dt1.Rows(0).Item("PurchaseAcName").ToString
                If Dt1.Rows(0).Item("Tax_Type").ToString <> "" Then cbo_TaxType.Text = Dt1.Rows(0).Item("Tax_Type").ToString
                If Dt1.Rows(0).Item("Tax_Percentage").ToString <> "" Then txt_TaxPerc.Text = Val(Dt1.Rows(0).Item("Tax_Percentage").ToString)
                If Dt1.Rows(0).Item("TaxAcName").ToString <> "" Then cbo_VatAc.Text = Dt1.Rows(0).Item("TaxAcName").ToString
                If Dt1.Rows(0).Item("Agent_Commission_Type").ToString <> "" Then cbo_CommType.Text = Dt1.Rows(0).Item("Agent_Commission_Type").ToString
                If Dt1.Rows(0).Item("AddLessAfterTax_Text").ToString <> "" Then txt_AddLessAfterTax_Text.Text = Dt1.Rows(0).Item("AddLessAfterTax_Text").ToString
                If Trim(txt_AddLessAfterTax_Text.Text) = "" Then txt_AddLessAfterTax_Text.Text = "Add/Less"


                Da1 = New SqlClient.SqlDataAdapter("Select a.* from Yarn_Purchase_details a Where a.Yarn_Purchase_Code = '" & Trim(Dt1.Rows(0).Item("Yarn_Purchase_Code").ToString) & "' Order by a.sl_no", con)
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

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally

            Dt1.Dispose()
            Dt2.Dispose()
            Da1.Dispose()

            If msk_Date.Enabled And msk_Date.Visible Then msk_Date.Focus()
            'If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
        End Try

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim RefCode As String

        Try

            inpno = InputBox("Enter Ref No.", "FOR FINDING...")

            RefCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Yarn_Purchase_No from Yarn_Purchase_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Entry_VAT_GST_Type <> 'GST' and Yarn_Purchase_Code = '" & Trim(RefCode) & "'", con)
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
                MessageBox.Show("Ref No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

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

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Yarn_Purchase_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Yarn_Purchase_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.Yarn_Purchase_Entry, New_Entry, Me) = False Then Exit Sub


        Try

            inpno = InputBox("Enter New Ref No.", "FOR NEW Ref NO. INSERTION...")

            InvCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Yarn_Purchase_No from Yarn_Purchase_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and  Entry_VAT_GST_Type <> 'GST' and Yarn_Purchase_Code = '" & Trim(InvCode) & "'", con)
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
                    MessageBox.Show("Invalid Ref No.", "DOES NOT INSERT NEW Ref...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_RefNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT NEW BILL...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

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
        Dim PurAc_ID As Integer = 0
        Dim Rck_IdNo As Integer = 0
        Dim Fp_Id As Integer = 0
        Dim Led_ID As Integer = 0
        Dim Cnt_ID As Integer = 0
        Dim Mill_ID As Integer = 0
        Dim clr_ID As Integer = 0
        Dim Del_ID As Integer
        Dim Agt_Idno As Integer = 0
        Dim TxAc_ID As Integer = 0
        Dim Unt_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim EntID As String = ""
        Dim PBlNo As String = ""
        Dim Nr As Integer = 0
        Dim Partcls As String = ""
        Dim vTotCns As Single, vTotBgs As Single, vTotWght As Single
        Dim Cr_ID As Integer = 0
        Dim Dr_ID As Integer = 0
        Dim uSR_ID As Integer = 0
        Dim Trans_ID As Integer
        Dim VouBil As String = ""
        Dim YrnClthNm As String = ""
        Dim Comm_Amt As Double = 0
        Dim ag_Comm As Double = 0
        Dim agtds_perc As Double = 0
        Dim RecNo As String = ""
        Dim RecCd As String = ""
        Dim RecSlNo As Long = 0
        Dim vLed_IdNos As String = "", vVou_Amts As String = "", ErrMsg As String = ""

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If
        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Yarn_Purchase_Entry, New_Entry, Me, con, "Yarn_Purchase_Head", "Yarn_Purchase_Code", NewCode, "Yarn_Purchase_Date", "(Yarn_Purchase_code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Purchase_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, Yarn_Purchase_No desc", dtp_Date.Value.Date) = False Then Exit Sub

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(msk_Date.Text) = False Then
            MessageBox.Show("Invalid Purchase Date", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If msk_Date.Enabled And msk_Date.Visible Then msk_Date.Focus()
            Exit Sub
        End If

        If Not (Convert.ToDateTime(msk_Date.Text) >= Common_Procedures.Company_FromDate And Convert.ToDateTime(msk_Date.Text) <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Purchase Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If msk_Date.Enabled And msk_Date.Visible Then msk_Date.Focus()
            Exit Sub
        End If

        Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyName.Text)
        If Val(Led_ID) = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_PartyName.Enabled And cbo_PartyName.Visible Then cbo_PartyName.Focus()
            Exit Sub
        End If

        If Trim(txt_BillNo.Text) = "" Then
            MessageBox.Show("Invalid Bill No", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If txt_BillNo.Enabled And txt_BillNo.Visible Then txt_BillNo.Focus()
            Exit Sub
        End If

        If IsDate(msk_BillDate.Text) = False Then
            msk_BillDate.Text = msk_Date.Text
        End If

        If IsDate(msk_BillDate.Text) = False Then
            MessageBox.Show("Invalid Bill Date", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If msk_BillDate.Enabled And msk_BillDate.Visible Then msk_BillDate.Focus()
            Exit Sub
        End If

        If Not (Convert.ToDateTime(msk_BillDate.Text) >= Common_Procedures.Company_FromDate And Convert.ToDateTime(msk_BillDate.Text) <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Bill Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If msk_BillDate.Enabled And msk_BillDate.Visible Then msk_BillDate.Focus()
            Exit Sub
        End If

        Agt_Idno = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Agent.Text)
        Del_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Delvat.Text)
        If Del_ID = 0 Then Del_ID = 4
        PurAc_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PurchaseAc.Text)
        TxAc_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_VatAc.Text)
        Trans_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Transport.Text)
        lbl_UserName.Text = Common_Procedures.User.IdNo

        If Trim(lbl_NetAmount.Text) = "" Then lbl_NetAmount.Text = 0

        If PurAc_ID = 0 And Val(CSng(lbl_NetAmount.Text)) <> 0 Then
            MessageBox.Show("Invalid Purchase A/c name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_PurchaseAc.Enabled And cbo_PurchaseAc.Visible Then cbo_PurchaseAc.Focus()
            Exit Sub
        End If

        With dgv_Details

            For i = 0 To .RowCount - 1

                If Val(.Rows(i).Cells(3).Value) <> 0 Or Val(.Rows(i).Cells(5).Value) <> 0 Then

                    Cnt_ID = Common_Procedures.Count_NameToIdNo(con, .Rows(i).Cells(1).Value)
                    If Cnt_ID = 0 Then
                        MessageBox.Show("Invalid Count Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(1)
                        End If
                        Exit Sub
                    End If

                    Mill_ID = Common_Procedures.Mill_NameToIdNo(con, .Rows(i).Cells(2).Value)
                    If Mill_ID = 0 Then
                        MessageBox.Show("Invalid Mill Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(2)
                        End If
                        Exit Sub
                    End If

                    If Val(.Rows(i).Cells(5).Value) = 0 Then
                        MessageBox.Show("Invalid Weight", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
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
            MessageBox.Show("Invalid Tax A/c name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_VatAc.Enabled And cbo_VatAc.Visible Then cbo_VatAc.Focus()
            Exit Sub
        End If

        If Val(lbl_TaxAmount.Text) <> 0 And (Trim(cbo_TaxType.Text) = "" Or Trim(cbo_TaxType.Text) = "-NIL-") Then
            MessageBox.Show("Invalid Tax Type", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_TaxType.Enabled And cbo_TaxType.Visible Then cbo_TaxType.Focus()
            Exit Sub
        End If

        If Trim(txt_BillNo.Text) <> "" Then
            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            Da = New SqlClient.SqlDataAdapter("select * from Yarn_Purchase_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Ledger_IdNo = " & Str(Val(Led_ID)) & " and Bill_No = '" & Trim(txt_BillNo.Text) & "' and Yarn_Purchase_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' and Yarn_Purchase_Code <> '" & Trim(NewCode) & "'", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                MessageBox.Show("Duplicate Bill No to this Party", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If txt_BillNo.Enabled And txt_BillNo.Visible Then txt_BillNo.Focus()
                Exit Sub
            End If
            Dt1.Clear()
        End If

        If Trim(txt_AddLessAfterTax_Text.Text) = "" Then txt_AddLessAfterTax_Text.Text = "Add/Less"

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
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Yarn_Purchase_Head", "Yarn_Purchase_Code", "For_OrderBy", "Entry_VAT_GST_Type <> 'GST'", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()

            cmd.Parameters.AddWithValue("@test1Date", dtp_Test1Date.Value.Date)

            cmd.Parameters.AddWithValue("@test2Date", dtp_Test2Date.Value.Date)

            cmd.Parameters.AddWithValue("@test3Date", dtp_test3date.Value.Date)

            cmd.Parameters.AddWithValue("@PurchaseDate", Convert.ToDateTime(msk_Date.Text))
            cmd.Parameters.AddWithValue("@BillDate", Convert.ToDateTime(msk_BillDate.Text))

            If New_Entry = True Then

                cmd.CommandText = "Insert into Yarn_Purchase_Head (       Yarn_Purchase_Code ,               Company_IdNo       ,           Yarn_Purchase_No    ,                               for_OrderBy                              , Yarn_Purchase_Date,        Ledger_IdNo      ,          Agent_IdNo    ,        PurchaseAc_IdNo    ,        DeliveryTo_Idno  ,             Bill_No              ,        Delivery_Receipt_No     ,   Agent_Commission_Rate       ,         Agent_Commission_Type    ,   Agent_Commission_Commission   ,       Total_Bags     ,          Total_Cones   ,          Total_Weight      ,               Total_Amount            ,             Discount_Percentage    ,              Discount_Amount         ,                AddLess_BeforeTax_Amount        ,                 Assessable_Value          ,           TaxAc_IdNo      ,              Tax_Type           ,             Tax_Percentage       ,             Tax_Amount              ,           Freight_Amount          ,               AddLessAfterTax_Text         ,               AddLess_Amount       ,               RoundOff_Amount      ,                  Net_Amount               ,         Transport_IdNo    ,            Note                       ,    Test1_Date     , Test2_Date   , Test3_Date  ,         Test1_No                   ,           Test2_No           ,           Test3_No       ,      Test1_Cone1_YarnWeight1             ,                     Test1_Cone1_YarnWeight2      ,          Test1_Cone1_YarnWeight3                  ,    Test1_Cone1_YarnWeight4                     ,   Test1_Cone1_YarnWeight5                      ,   Test1_Cone1_YarnCount1            ,          Test1_Cone1_YarnCount2                    ,        Test1_Cone1_YarnCount3                  ,    Test1_Cone1_YarnCount4                          ,              Test1_Cone1_YarnCount5            ,               Test1_Cone2_YarnWeight1             ,                     Test1_Cone2_YarnWeight2      ,          Test1_Cone2_YarnWeight3                  ,    Test1_Cone2_YarnWeight4                     ,   Test1_Cone2_YarnWeight5                      ,   Test1_Cone2_YarnCount1            ,          Test1_Cone2_YarnCount2                    ,        Test1_Cone2_YarnCount3                  ,    Test1_Cone2_YarnCount4                          ,              Test1_Cone2_YarnCount5            ,          Test1_Cone3_YarnWeight1             ,                     Test1_Cone3_YarnWeight2      ,          Test1_Cone3_YarnWeight3                  ,    Test1_Cone3_YarnWeight4                     ,   Test1_Cone3_YarnWeight5                      ,                   Test1_Cone3_YarnCount1            ,          Test1_Cone3_YarnCount2                    ,        Test1_Cone3_YarnCount3                  ,    Test1_Cone3_YarnCount4                          ,              Test1_Cone3_YarnCount5            ,                  Test1_Cone4_YarnWeight1             ,                     Test1_Cone4_YarnWeight2      ,          Test1_Cone4_YarnWeight3                  ,    Test1_Cone4_YarnWeight4                     ,   Test1_Cone4_YarnWeight5                      ,   Test1_Cone4_YarnCount1            ,          Test1_Cone4_YarnCount2                    ,        Test1_Cone4_YarnCount3                  ,    Test1_Cone4_YarnCount4                          ,              Test1_Cone4_YarnCount5            ,         Test1_Cone5_YarnWeight1             ,                     Test1_Cone5_YarnWeight2      ,          Test1_Cone5_YarnWeight3                  ,    Test1_Cone5_YarnWeight4                     ,   Test1_Cone5_YarnWeight5                      ,   Test1_Cone5_YarnCount1            ,          Test1_Cone5_YarnCount2                    ,        Test1_Cone5_YarnCount3                  ,    Test1_Cone5_YarnCount4                          ,              Test1_Cone5_YarnCount5            ,           Test1_Cone6_YarnWeight1             ,                     Test1_Cone6_YarnWeight2      ,          Test1_Cone6_YarnWeight3                  ,    Test1_Cone6_YarnWeight4                     ,   Test1_Cone6_YarnWeight5                      ,   Test1_Cone6_YarnCount1            ,          Test1_Cone6_YarnCount2                    ,        Test1_Cone6_YarnCount3                  ,    Test1_Cone6_YarnCount4                     ,              Test1_Cone6_YarnCount5           ,                                  Test2_Cone1_YarnWeight1             ,                     Test2_Cone1_YarnWeight2      ,          Test2_Cone1_YarnWeight3                  ,    Test2_Cone1_YarnWeight4                     ,   Test2_Cone1_YarnWeight5                      ,   Test2_Cone1_YarnCount1            ,          Test2_Cone1_YarnCount2                    ,        Test2_Cone1_YarnCount3                  ,    Test2_Cone1_YarnCount4                          ,              Test2_Cone1_YarnCount5            ,               Test2_Cone2_YarnWeight1             ,                     Test2_Cone2_YarnWeight2      ,          Test2_Cone2_YarnWeight3                  ,    Test2_Cone2_YarnWeight4                     ,   Test2_Cone2_YarnWeight5                      ,                         Test2_Cone2_YarnCount1            ,          Test2_Cone2_YarnCount2                    ,        Test2_Cone2_YarnCount3                  ,    Test2_Cone2_YarnCount4                          ,              Test2_Cone2_YarnCount5            ,          Test2_Cone3_YarnWeight1             ,                     Test2_Cone3_YarnWeight2      ,          Test2_Cone3_YarnWeight3                  ,    Test2_Cone3_YarnWeight4                     ,   Test2_Cone3_YarnWeight5                      ,   Test2_Cone3_YarnCount1            ,          Test2_Cone3_YarnCount2                    ,        Test2_Cone3_YarnCount3                  ,    Test2_Cone3_YarnCount4                          ,              Test2_Cone3_YarnCount5            ,                  Test2_Cone4_YarnWeight1             ,                     Test2_Cone4_YarnWeight2      ,          Test2_Cone4_YarnWeight3                  ,    Test2_Cone4_YarnWeight4                     ,   Test2_Cone4_YarnWeight5                      ,   Test2_Cone4_YarnCount1            ,          Test2_Cone4_YarnCount2                    ,        Test2_Cone4_YarnCount3                  ,    Test2_Cone4_YarnCount4                          ,              Test2_Cone4_YarnCount5            ,         Test2_Cone5_YarnWeight1             ,                     Test2_Cone5_YarnWeight2      ,          Test2_Cone5_YarnWeight3                  ,    Test2_Cone5_YarnWeight4                     ,   Test2_Cone5_YarnWeight5                      ,   Test2_Cone5_YarnCount1            ,          Test2_Cone5_YarnCount2                    ,        Test2_Cone5_YarnCount3                  ,    Test2_Cone5_YarnCount4                          ,              Test2_Cone5_YarnCount5            ,           Test2_Cone6_YarnWeight1             ,                     Test2_Cone6_YarnWeight2      ,          Test2_Cone6_YarnWeight3                  ,    Test2_Cone6_YarnWeight4                     ,   Test2_Cone6_YarnWeight5                      ,   Test2_Cone6_YarnCount1            ,          Test2_Cone6_YarnCount2                    ,        Test2_Cone6_YarnCount3                  ,    Test2_Cone6_YarnCount4                          ,              Test2_Cone6_YarnCount5           ,                         Test3_Cone1_YarnWeight1             ,                     Test3_Cone1_YarnWeight2      ,          Test3_Cone1_YarnWeight3                  ,    Test3_Cone1_YarnWeight4                     ,   Test3_Cone1_YarnWeight5                      ,   Test3_Cone1_YarnCount1            ,          Test3_Cone1_YarnCount2                    ,                 Test3_Cone1_YarnCount3                  ,    Test3_Cone1_YarnCount4                          ,              Test3_Cone1_YarnCount5            ,               Test3_Cone2_YarnWeight1             ,                     Test3_Cone2_YarnWeight2      ,          Test3_Cone2_YarnWeight3                  ,    Test3_Cone2_YarnWeight4                     ,   Test3_Cone2_YarnWeight5                      ,   Test3_Cone2_YarnCount1            ,          Test3_Cone2_YarnCount2                    ,        Test3_Cone2_YarnCount3                  ,    Test3_Cone2_YarnCount4                          ,              Test3_Cone2_YarnCount5            ,          Test3_Cone3_YarnWeight1             ,                     Test3_Cone3_YarnWeight2      ,          Test3_Cone3_YarnWeight3                  ,    Test3_Cone3_YarnWeight4                     ,   Test3_Cone3_YarnWeight5                      ,          Test3_Cone3_YarnCount1            ,          Test3_Cone3_YarnCount2                    ,        Test3_Cone3_YarnCount3                  ,    Test3_Cone3_YarnCount4                          ,              Test3_Cone3_YarnCount5            ,                  Test3_Cone4_YarnWeight1             ,                     Test3_Cone4_YarnWeight2      ,          Test3_Cone4_YarnWeight3                  ,    Test3_Cone4_YarnWeight4                     ,   Test3_Cone4_YarnWeight5                      ,   Test3_Cone4_YarnCount1            ,          Test3_Cone4_YarnCount2                    ,        Test3_Cone4_YarnCount3                  ,    Test3_Cone4_YarnCount4                          ,              Test3_Cone4_YarnCount5            ,         Test3_Cone5_YarnWeight1             ,                     Test3_Cone5_YarnWeight2      ,          Test3_Cone5_YarnWeight3                  ,    Test3_Cone5_YarnWeight4                     ,   Test3_Cone5_YarnWeight5                      ,   Test3_Cone5_YarnCount1            ,          Test3_Cone5_YarnCount2                    ,        Test3_Cone5_YarnCount3                  ,    Test3_Cone5_YarnCount4                          ,              Test3_Cone5_YarnCount5            ,           Test3_Cone6_YarnWeight1             ,                     Test3_Cone6_YarnWeight2      ,          Test3_Cone6_YarnWeight3                  ,    Test3_Cone6_YarnWeight4                     ,   Test3_Cone6_YarnWeight5                      ,   Test3_Cone6_YarnCount1            ,          Test3_Cone6_YarnCount2                    ,        Test3_Cone6_YarnCount3                  ,    Test3_Cone6_YarnCount4                          ,              Test3_Cone6_YarnCount5            ,          Test1_Cone1_Mean                  ,    Test1_Cone1_Cv                          ,              Test1_Cone1_SD            ,          Test1_Cone2_Mean                  ,    Test1_Cone2_Cv                          ,              Test1_Cone2_SD            ,       Test1_Cone3_Mean                  ,    Test1_Cone3_Cv                          ,              Test1_Cone3_SD            ,            Test1_Cone4_Mean                  ,    Test1_Cone4_Cv                          ,              Test1_Cone4_SD            ,        Test1_Cone5_Mean                  ,    Test1_Cone5_Cv                          ,              Test1_Cone5_SD                 ,        Test1_Cone6_Mean                  ,    Test1_Cone6_Cv                          ,              Test1_Cone6_SD            ,        Test1_Avg_Cv                 ,    Test1_Avg_Sd                          ,              Test1_Avg_Coul             ,         Test2_Cone1_Mean                  ,    Test2_Cone1_Cv                          ,              Test2_Cone1_SD            ,          Test2_Cone2_Mean                  ,    Test2_Cone2_Cv                          ,              Test2_Cone2_SD            ,       Test2_Cone3_Mean                  ,    Test2_Cone3_Cv                          ,              Test2_Cone3_SD            ,            Test2_Cone4_Mean                  ,    Test2_Cone4_Cv                          ,              Test2_Cone4_SD            ,        Test2_Cone5_Mean                  ,    Test2_Cone5_Cv                          ,              Test2_Cone5_SD                 ,        Test2_Cone6_Mean                  ,    Test2_Cone6_Cv                          ,              Test2_Cone6_SD            ,  Test2_Avg_Cv                 ,    Test2_Avg_Sd                          ,              Test2_Avg_Coul                  ,          Test3_Cone1_Mean                  ,    Test3_Cone1_Cv                          ,              Test3_Cone1_SD            ,          Test3_Cone2_Mean                  ,    Test3_Cone2_Cv                          ,              Test3_Cone2_SD            ,       Test3_Cone3_Mean                  ,    Test3_Cone3_Cv                          ,              Test3_Cone3_SD            ,            Test3_Cone4_Mean                  ,    Test3_Cone4_Cv                          ,              Test3_Cone4_SD            ,        Test3_Cone5_Mean                  ,    Test3_Cone5_Cv                          ,              Test3_Cone5_SD                 ,        Test3_Cone6_Mean                  ,    Test3_Cone6_Cv                          ,              Test3_Cone6_SD            ,  Test3_Avg_Cv                 ,    Test3_Avg_Sd                          ,              Test3_Avg_Coul           ,     Transport_Freight                               ,   User_idNo                ,  Vehicle_No                            ,       Purchase_Type         ,    Bill_Date  ) " & _
                                    "     Values                  (   '" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",      @PurchaseDate    , " & Str(Val(Led_ID)) & ", " & Str(Val(Agt_Idno)) & ", " & Str(Val(PurAc_ID)) & ", " & Str(Val(Del_ID)) & ",   '" & Trim(txt_BillNo.Text) & "',  '" & Trim(txt_RecNo.Text) & "', " & Val(txt_CommRate.Text) & ", '" & Trim(cbo_CommType.Text) & "', " & Val(lbl_CommAmount.Text) & ",  " & Val(vTotBgs) & "," & Str(Val(vTotCns)) & ", " & Str(Val(vTotWght)) & ", " & Str(Val(lbl_GrossAmount.Text)) & ", " & Str(Val(txt_DiscPerc.Text)) & ", " & Str(Val(lbl_DiscAmount.Text)) & ", " & Str(Val(txt_AddLess_BeforeTax.Text)) & ", " & Str(Val(txt_AssessableValue.Text)) & ", " & Str(Val(TxAc_ID)) & ", '" & Trim(cbo_TaxType.Text) & "', " & Str(Val(txt_TaxPerc.Text)) & ", " & Str(Val(lbl_TaxAmount.Text)) & ", " & Str(Val(txt_Freight.Text)) & ", '" & Trim(txt_AddLessAfterTax_Text.Text) & "', " & Str(Val(txt_AddLess_AfterTax.Text)) & ", " & Str(Val(lbl_RoundOff.Text)) & ", " & Str(Val(CSng(lbl_NetAmount.Text))) & ", " & Str(Val(Trans_ID)) & ",'" & Trim(txt_Note.Text) & "' , @test1Date    ,    @test2Date  ,  @test3Date  ,'" & Trim(txt_test1No.Text) & "','" & Trim(txt_Test2No.Text) & "','" & Trim(txt_test3No.Text) & "'," & Str(Val(txt_Test1_Cone1_YarnWeight1.Text)) & ", " & Str(Val(txt_Test1_Cone1_YarnWeight2.Text)) & ", " & Val(txt_Test1_Cone1_YarnWeight3.Text) & ",  " & Val(txt_Test1_Cone1_YarnWeight4.Text) & ",  " & Val(txt_Test1_Cone1_YarnWeight5.Text) & ",  " & Val(lbl_Test1_Cone1_YarnCount1.Text) & "," & Str(Val(lbl_Test1_Cone1_YarnCount2.Text)) & ", " & Str(Val(lbl_Test1_Cone1_YarnCount3.Text)) & ", " & Str(Val(lbl_Test1_Cone1_YarnCount4.Text)) & ", " & Str(Val(lbl_Test1_Cone1_YarnCount5.Text)) & ",  " & Str(Val(txt_Test1_Cone2_YarnWeight1.Text)) & ", " & Str(Val(txt_Test1_Cone2_YarnWeight2.Text)) & ", " & Val(txt_Test1_Cone2_YarnWeight3.Text) & ",  " & Val(txt_Test1_Cone2_YarnWeight4.Text) & ",  " & Val(txt_Test1_Cone2_YarnWeight5.Text) & ",  " & Val(lbl_Test1_Cone2_YarnCount1.Text) & "," & Str(Val(lbl_Test1_Cone2_YarnCount2.Text)) & ", " & Str(Val(lbl_Test1_Cone2_YarnCount3.Text)) & ", " & Str(Val(lbl_Test1_Cone2_YarnCount4.Text)) & ", " & Str(Val(lbl_Test1_Cone2_YarnCount5.Text)) & "," & Str(Val(txt_Test1_Cone3_YarnWeight1.Text)) & ", " & Str(Val(txt_Test1_Cone3_YarnWeight2.Text)) & ", " & Val(txt_Test1_Cone3_YarnWeight3.Text) & ",  " & Val(txt_Test1_Cone3_YarnWeight4.Text) & ",  " & Val(txt_Test1_Cone3_YarnWeight5.Text) & ",  " & Val(lbl_Test1_Cone3_YarnCount1.Text) & "," & Str(Val(lbl_Test1_Cone3_YarnCount2.Text)) & ", " & Str(Val(lbl_Test1_Cone3_YarnCount3.Text)) & ", " & Str(Val(lbl_Test1_Cone3_YarnCount4.Text)) & ", " & Str(Val(lbl_Test1_Cone3_YarnCount5.Text)) & ", " & Str(Val(txt_Test1_Cone4_YarnWeight1.Text)) & ", " & Str(Val(txt_Test1_Cone4_YarnWeight2.Text)) & ", " & Val(txt_Test1_Cone4_YarnWeight3.Text) & ",  " & Val(txt_Test1_Cone4_YarnWeight4.Text) & ",  " & Val(txt_Test1_Cone4_YarnWeight5.Text) & ",  " & Val(lbl_Test1_Cone4_YarnCount1.Text) & "," & Str(Val(lbl_Test1_Cone4_YarnCount2.Text)) & ", " & Str(Val(lbl_Test1_Cone4_YarnCount3.Text)) & ", " & Str(Val(lbl_Test1_Cone4_YarnCount4.Text)) & ", " & Str(Val(lbl_Test1_Cone4_YarnCount5.Text)) & "," & Str(Val(txt_Test1_Cone5_YarnWeight1.Text)) & ", " & Str(Val(txt_Test1_Cone5_YarnWeight2.Text)) & ", " & Val(txt_Test1_Cone5_YarnWeight3.Text) & ",  " & Val(txt_Test1_Cone5_YarnWeight4.Text) & ",  " & Val(txt_Test1_Cone5_YarnWeight5.Text) & ",  " & Val(lbl_Test1_Cone5_YarnCount1.Text) & "," & Str(Val(lbl_Test1_Cone5_YarnCount2.Text)) & ", " & Str(Val(lbl_Test1_Cone5_YarnCount3.Text)) & ", " & Str(Val(lbl_Test1_Cone5_YarnCount4.Text)) & ", " & Str(Val(lbl_Test1_Cone5_YarnCount5.Text)) & "," & Str(Val(txt_Test1_Cone6_YarnWeight1.Text)) & ", " & Str(Val(txt_Test1_Cone6_YarnWeight2.Text)) & ", " & Val(txt_Test1_Cone6_YarnWeight3.Text) & ",  " & Val(txt_Test1_Cone6_YarnWeight4.Text) & ",  " & Val(txt_Test1_Cone6_YarnWeight5.Text) & ",  " & Val(lbl_Test1_Cone6_YarnCount1.Text) & "," & Str(Val(lbl_Test1_Cone6_YarnCount2.Text)) & ", " & Str(Val(lbl_Test1_Cone6_YarnCount3.Text)) & ", " & Str(Val(lbl_Test1_Cone6_YarnCount4.Text)) & ", " & Str(Val(lbl_Test1_Cone6_YarnCount5.Text)) & ", " & Str(Val(txt_Test2_Cone1_YarnWeight1.Text)) & ", " & Str(Val(txt_Test2_Cone1_YarnWeight2.Text)) & ", " & Val(txt_Test2_Cone1_YarnWeight3.Text) & ",  " & Val(txt_Test2_Cone1_YarnWeight4.Text) & ",  " & Val(txt_Test2_Cone1_YarnWeight5.Text) & ",  " & Val(lbl_Test2_Cone1_YarnCount1.Text) & "," & Str(Val(lbl_Test2_Cone1_YarnCount2.Text)) & ", " & Str(Val(lbl_Test2_Cone1_YarnCount3.Text)) & ", " & Str(Val(lbl_Test2_Cone1_YarnCount4.Text)) & ", " & Str(Val(lbl_Test2_Cone1_YarnCount5.Text)) & ",  " & Str(Val(txt_Test2_Cone2_YarnWeight1.Text)) & ", " & Str(Val(txt_Test2_Cone2_YarnWeight2.Text)) & ", " & Val(txt_Test2_Cone2_YarnWeight3.Text) & ",  " & Val(txt_Test2_Cone2_YarnWeight4.Text) & ",  " & Val(txt_Test2_Cone2_YarnWeight5.Text) & ",  " & Val(lbl_Test2_Cone2_YarnCount1.Text) & "," & Str(Val(lbl_Test2_Cone2_YarnCount2.Text)) & ", " & Str(Val(lbl_Test2_Cone2_YarnCount3.Text)) & ", " & Str(Val(lbl_Test2_Cone2_YarnCount4.Text)) & ", " & Str(Val(lbl_Test2_Cone2_YarnCount5.Text)) & "," & Str(Val(txt_Test2_Cone3_YarnWeight1.Text)) & ", " & Str(Val(txt_Test2_Cone3_YarnWeight2.Text)) & ", " & Val(txt_Test2_Cone3_YarnWeight3.Text) & ",  " & Val(txt_Test2_Cone3_YarnWeight4.Text) & ",  " & Val(txt_Test2_Cone3_YarnWeight5.Text) & ",  " & Val(lbl_Test2_Cone3_YarnCount1.Text) & "," & Str(Val(lbl_Test2_Cone3_YarnCount2.Text)) & ", " & Str(Val(lbl_Test2_Cone3_YarnCount3.Text)) & ", " & Str(Val(lbl_Test2_Cone3_YarnCount4.Text)) & ", " & Str(Val(lbl_Test2_Cone3_YarnCount5.Text)) & ", " & Str(Val(txt_Test2_Cone4_YarnWeight1.Text)) & ", " & Str(Val(txt_Test2_Cone4_YarnWeight2.Text)) & ", " & Val(txt_Test2_Cone4_YarnWeight3.Text) & ",  " & Val(txt_Test2_Cone4_YarnWeight4.Text) & ",  " & Val(txt_Test2_Cone4_YarnWeight5.Text) & ",  " & Val(lbl_Test2_Cone4_YarnCount1.Text) & "," & Str(Val(lbl_Test2_Cone4_YarnCount2.Text)) & ", " & Str(Val(lbl_Test2_Cone4_YarnCount3.Text)) & ", " & Str(Val(lbl_Test2_Cone4_YarnCount4.Text)) & ", " & Str(Val(lbl_Test2_Cone4_YarnCount5.Text)) & "," & Str(Val(txt_Test2_Cone5_YarnWeight1.Text)) & ", " & Str(Val(txt_Test2_Cone5_YarnWeight2.Text)) & ", " & Val(txt_Test2_Cone5_YarnWeight3.Text) & ",  " & Val(txt_Test2_Cone5_YarnWeight4.Text) & ",  " & Val(txt_Test2_Cone5_YarnWeight5.Text) & ",  " & Val(lbl_Test2_Cone5_YarnCount1.Text) & "," & Str(Val(lbl_Test2_Cone5_YarnCount2.Text)) & ", " & Str(Val(lbl_Test2_Cone5_YarnCount3.Text)) & ", " & Str(Val(lbl_Test2_Cone5_YarnCount4.Text)) & ", " & Str(Val(lbl_Test2_Cone5_YarnCount5.Text)) & "," & Str(Val(txt_Test2_Cone6_YarnWeight1.Text)) & ", " & Str(Val(txt_Test2_Cone6_YarnWeight2.Text)) & ", " & Val(txt_Test2_Cone6_YarnWeight3.Text) & ",  " & Val(txt_Test2_Cone6_YarnWeight4.Text) & ",  " & Val(txt_Test2_Cone6_YarnWeight5.Text) & ",  " & Val(lbl_Test2_Cone6_YarnCount1.Text) & "," & Str(Val(lbl_Test2_Cone6_YarnCount2.Text)) & ", " & Str(Val(lbl_Test2_Cone6_YarnCount3.Text)) & ", " & Str(Val(lbl_Test2_Cone6_YarnCount4.Text)) & ", " & Str(Val(lbl_Test2_Cone6_YarnCount5.Text)) & ", " & Str(Val(txt_Test3_Cone1_YarnWeight1.Text)) & ", " & Str(Val(txt_Test3_Cone1_YarnWeight2.Text)) & ", " & Val(txt_Test3_Cone1_YarnWeight3.Text) & ",  " & Val(txt_Test3_Cone1_YarnWeight4.Text) & ",  " & Val(txt_Test3_Cone1_YarnWeight5.Text) & ",  " & Val(lbl_Test3_Cone1_YarnCount1.Text) & "," & Str(Val(lbl_Test3_Cone1_YarnCount2.Text)) & ", " & Str(Val(lbl_Test3_Cone1_YarnCount3.Text)) & ", " & Str(Val(lbl_Test3_Cone1_YarnCount4.Text)) & ", " & Str(Val(lbl_Test3_Cone1_YarnCount5.Text)) & ",  " & Str(Val(txt_Test3_Cone2_YarnWeight1.Text)) & ", " & Str(Val(txt_Test3_Cone2_YarnWeight2.Text)) & ", " & Val(txt_Test3_Cone2_YarnWeight3.Text) & ",  " & Val(txt_Test3_Cone2_YarnWeight4.Text) & ",  " & Val(txt_Test3_Cone2_YarnWeight5.Text) & ",  " & Val(lbl_Test3_Cone2_YarnCount1.Text) & "," & Str(Val(lbl_Test3_Cone2_YarnCount2.Text)) & ", " & Str(Val(lbl_Test3_Cone2_YarnCount3.Text)) & ", " & Str(Val(lbl_Test3_Cone2_YarnCount4.Text)) & ", " & Str(Val(lbl_Test3_Cone2_YarnCount5.Text)) & "," & Str(Val(txt_Test3_Cone3_YarnWeight1.Text)) & ", " & Str(Val(txt_Test3_Cone3_YarnWeight2.Text)) & ", " & Val(txt_Test3_Cone3_YarnWeight3.Text) & ",  " & Val(txt_Test3_Cone3_YarnWeight4.Text) & ",  " & Val(txt_Test3_Cone3_YarnWeight5.Text) & ",  " & Val(lbl_Test3_Cone3_YarnCount1.Text) & "," & Str(Val(lbl_Test3_Cone3_YarnCount2.Text)) & ", " & Str(Val(lbl_Test3_Cone3_YarnCount3.Text)) & ", " & Str(Val(lbl_Test3_Cone3_YarnCount4.Text)) & ", " & Str(Val(lbl_Test3_Cone3_YarnCount5.Text)) & ", " & Str(Val(txt_Test3_Cone4_YarnWeight1.Text)) & ", " & Str(Val(txt_Test3_Cone4_YarnWeight2.Text)) & ", " & Val(txt_Test3_Cone4_YarnWeight3.Text) & ",  " & Val(txt_Test3_Cone4_YarnWeight4.Text) & ",  " & Val(txt_Test3_Cone4_YarnWeight5.Text) & ",  " & Val(lbl_Test3_Cone4_YarnCount1.Text) & "," & Str(Val(lbl_Test3_Cone4_YarnCount2.Text)) & ", " & Str(Val(lbl_Test3_Cone4_YarnCount3.Text)) & ", " & Str(Val(lbl_Test3_Cone4_YarnCount4.Text)) & ", " & Str(Val(lbl_Test3_Cone4_YarnCount5.Text)) & "," & Str(Val(txt_Test3_Cone5_YarnWeight1.Text)) & ", " & Str(Val(txt_Test3_Cone5_YarnWeight2.Text)) & ", " & Val(txt_Test3_Cone5_YarnWeight3.Text) & ",  " & Val(txt_Test3_Cone5_YarnWeight4.Text) & ",  " & Val(txt_Test3_Cone5_YarnWeight5.Text) & ",  " & Val(lbl_Test3_Cone5_YarnCount1.Text) & "," & Str(Val(lbl_Test3_Cone5_YarnCount2.Text)) & ", " & Str(Val(lbl_Test3_Cone5_YarnCount3.Text)) & ", " & Str(Val(lbl_Test3_Cone5_YarnCount4.Text)) & ", " & Str(Val(lbl_Test3_Cone5_YarnCount5.Text)) & "," & Str(Val(txt_Test3_Cone6_YarnWeight1.Text)) & ", " & Str(Val(txt_Test3_Cone6_YarnWeight2.Text)) & ", " & Val(txt_Test3_Cone6_YarnWeight3.Text) & ",  " & Val(txt_Test3_Cone6_YarnWeight4.Text) & ",  " & Val(txt_Test3_Cone6_YarnWeight5.Text) & ",  " & Val(lbl_Test3_Cone6_YarnCount1.Text) & "," & Str(Val(lbl_Test3_Cone6_YarnCount2.Text)) & ", " & Str(Val(lbl_Test3_Cone6_YarnCount3.Text)) & ", " & Str(Val(lbl_Test3_Cone6_YarnCount4.Text)) & ", " & Str(Val(lbl_Test3_Cone6_YarnCount5.Text)) & ",  " & Str(Val(lbl_Test1_Cone1_Mean.Text)) & ", " & Str(Val(lbl_Test1_Cone1_CV.Text)) & ", " & Str(Val(lbl_Test1_Cone1_SD.Text)) & ",  " & Str(Val(lbl_Test1_Cone2_mean.Text)) & ", " & Str(Val(lbl_Test1_Cone2_Cv.Text)) & ", " & Str(Val(lbl_Test1_Cone2_SD.Text)) & ", " & Str(Val(lbl_Test1_Cone3_mean.Text)) & ", " & Str(Val(lbl_Test1_Cone3_Cv.Text)) & ", " & Str(Val(lbl_Test1_Cone3_SD.Text)) & ", " & Str(Val(lbl_Test1_Cone4_mean.Text)) & ", " & Str(Val(lbl_Test1_Cone4_Cv.Text)) & ", " & Str(Val(lbl_Test1_Cone4_SD.Text)) & ", " & Str(Val(lbl_Test1_Cone5_mean.Text)) & ", " & Str(Val(lbl_Test1_Cone5_Cv.Text)) & ", " & Str(Val(lbl_Test1_Cone5_SD.Text)) & ", " & Str(Val(lbl_Test1_Cone6_mean.Text)) & ", " & Str(Val(lbl_Test1_Cone6_Cv.Text)) & ", " & Str(Val(lbl_Test1_Cone6_Sd.Text)) & "," & Str(Val(lbl_Test1_AvgCv.Text)) & ", " & Str(Val(lbl_test1_AvgSd.Text)) & ", " & Str(Val(lbl_Test1_AvgCoul.Text)) & "," & Str(Val(lbl_Test2_Cone1_Mean.Text)) & ", " & Str(Val(lbl_Test2_Cone1_CV.Text)) & ", " & Str(Val(lbl_Test2_Cone1_SD.Text)) & ",  " & Str(Val(lbl_Test2_Cone2_Mean.Text)) & ", " & Str(Val(lbl_Test2_Cone2_CV.Text)) & ", " & Str(Val(lbl_Test2_Cone2_SD.Text)) & ", " & Str(Val(lbl_Test2_Cone3_Mean.Text)) & ", " & Str(Val(lbl_Test2_Cone3_CV.Text)) & ", " & Str(Val(lbl_Test2_Cone3_SD.Text)) & ", " & Str(Val(lbl_Test2_Cone4_Mean.Text)) & ", " & Str(Val(lbl_Test2_Cone4_CV.Text)) & ", " & Str(Val(lbl_Test2_Cone4_SD.Text)) & ", " & Str(Val(lbl_Test2_Cone5_Mean.Text)) & ", " & Str(Val(lbl_Test2_Cone5_CV.Text)) & ", " & Str(Val(lbl_Test2_Cone5_SD.Text)) & ", " & Str(Val(lbl_Test2_Cone6_Mean.Text)) & ", " & Str(Val(lbl_Test2_Cone6_CV.Text)) & ", " & Str(Val(lbl_Test2_Cone6_SD.Text)) & "," & Str(Val(lbl_Test2_AvgCv.Text)) & ", " & Str(Val(lbl_Test2_Avg_Sd.Text)) & ", " & Str(Val(lbl_Test2_avgCoul.Text)) & "," & Str(Val(lbl_Test3_Cone1_Mean.Text)) & ", " & Str(Val(lbl_Test3_Cone1_CV.Text)) & ", " & Str(Val(lbl_Test3_Cone1_SD.Text)) & ",  " & Str(Val(lbl_Test3_Cone2_mean.Text)) & ", " & Str(Val(lbl_Test3_Cone2_CV.Text)) & ", " & Str(Val(lbl_Test3_Cone2_SD.Text)) & ", " & Str(Val(lbl_Test3_Cone3_mean.Text)) & ", " & Str(Val(lbl_Test3_Cone3_CV.Text)) & ", " & Str(Val(lbl_Test3_Cone3_SD.Text)) & ", " & Str(Val(lbl_Test3_Cone4_mean.Text)) & ", " & Str(Val(lbl_Test3_Cone4_CV.Text)) & ", " & Str(Val(lbl_Test3_Cone4_SD.Text)) & ", " & Str(Val(lbl_Test3_Cone5_Mean.Text)) & ", " & Str(Val(lbl_Test3_Cone5_CV.Text)) & ", " & Str(Val(lbl_Test3_Cone5_SD.Text)) & ", " & Str(Val(lbl_Test3_Cone6_Mean.Text)) & ", " & Str(Val(lbl_Test3_Cone6_CV.Text)) & ", " & Str(Val(lbl_Test3_Cone6_SD.Text)) & "," & Str(Val(lbl_Test3_AvgCv.Text)) & ", " & Str(Val(lbl_Test3_AvgSd.Text)) & ", " & Str(Val(lbl_Test3_AvgCoul.Text)) & " , " & Val(txt_Transport_Freight.Text) & "   , " & Val(lbl_UserName.Text) & " , '" & Trim(cbo_VehicleNo.Text) & "','" & Trim(cbo_Type.Text) & "' ,      @BillDate ) "
                cmd.ExecuteNonQuery()

            Else

                cmd.CommandText = "Update Yarn_Purchase_Head set Yarn_Purchase_Date = @PurchaseDate, Ledger_IdNo = " & Str(Val(Led_ID)) & ", Agent_IdNo = " & Str(Val(Agt_Idno)) & ", PurchaseAc_IdNo = " & Str(Val(PurAc_ID)) & ", DeliveryTo_Idno = " & Str(Val(Del_ID)) & ", Bill_No = '" & Trim(txt_BillNo.Text) & "', Delivery_Receipt_No = '" & Trim(txt_RecNo.Text) & "', Agent_Commission_Rate = " & Val(txt_CommRate.Text) & ",Vehicle_No = '" & Trim(cbo_VehicleNo.Text) & "' ,  Agent_Commission_Type = '" & Trim(cbo_CommType.Text) & "', Agent_Commission_Commission =" & Val(lbl_CommAmount.Text) & ", Total_Bags = " & Val(vTotBgs) & ",Total_Cones  = " & Str(Val(vTotCns)) & ", Total_Weight = " & Str(Val(vTotWght)) & ", Total_Amount = " & Str(Val(lbl_GrossAmount.Text)) & ", Discount_Percentage = " & Str(Val(txt_DiscPerc.Text)) & ", Discount_Amount = " & Str(Val(lbl_DiscAmount.Text)) & ", AddLess_BeforeTax_Amount = " & Str(Val(txt_AddLess_BeforeTax.Text)) & ", Assessable_Value = " & Str(Val(txt_AssessableValue.Text)) & ", Tax_Type = '" & Trim(cbo_TaxType.Text) & "', Tax_Percentage = " & Str(Val(txt_TaxPerc.Text)) & ", Tax_Amount = " & Str(Val(lbl_TaxAmount.Text)) & ", TaxAc_IdNo = " & Str(Val(TxAc_ID)) & ", Freight_Amount = " & Str(Val(txt_Freight.Text)) & ", AddLessAfterTax_Text = '" & Trim(txt_AddLessAfterTax_Text.Text) & "', AddLess_Amount = " & Str(Val(txt_AddLess_AfterTax.Text)) & ", RoundOff_Amount = " & Str(Val(lbl_RoundOff.Text)) & ", Net_Amount = " & Str(Val(CSng(lbl_NetAmount.Text))) & ", Transport_IdNo  = " & Str(Val(Trans_ID)) & ", Note = '" & Trim(txt_Note.Text) & "' , Test1_Date = @test1Date, Test2_Date = @test2Date, Test3_Date = @test3Date, Test1_No = '" & Trim(txt_test1No.Text) & "', Test2_No = '" & Trim(txt_Test2No.Text) & "', Test3_No = '" & Trim(txt_test3No.Text) & "',Test1_Cone1_YarnWeight1 = " & Str(Val(txt_Test1_Cone1_YarnWeight1.Text)) & "   , Test1_Cone1_YarnWeight2   = " & Str(Val(txt_Test1_Cone1_YarnWeight2.Text)) & "   ,   Test1_Cone1_YarnWeight3 = " & Str(Val(txt_Test1_Cone1_YarnWeight3.Text)) & "     ,    Test1_Cone1_YarnWeight4 =" & Str(Val(txt_Test1_Cone1_YarnWeight4.Text)) & "    ,   Test1_Cone1_YarnWeight5   =" & Str(Val(txt_Test1_Cone1_YarnWeight5.Text)) & "   ,   Test1_Cone1_YarnCount1 =  " & Val(lbl_Test1_Cone1_YarnCount1.Text) & " ,  Test1_Cone1_YarnCount2   =  " & Val(lbl_Test1_Cone1_YarnCount2.Text) & "     ,       Test1_Cone1_YarnCount3 = " & Val(lbl_Test1_Cone1_YarnCount3.Text) & "     ,    Test1_Cone1_YarnCount4  =  " & Val(lbl_Test1_Cone1_YarnCount4.Text) & "     ,Test1_Cone1_YarnCount5   = " & Val(lbl_Test1_Cone1_YarnCount5.Text) & " ,   Test1_Cone2_YarnWeight1  = " & Str(Val(txt_Test1_Cone2_YarnWeight1.Text)) & "   ,  Test1_Cone2_YarnWeight2  = " & Str(Val(txt_Test1_Cone2_YarnWeight2.Text)) & "   , Test1_Cone2_YarnWeight3   = " & Str(Val(txt_Test1_Cone2_YarnWeight3.Text)) & ",    Test1_Cone2_YarnWeight4   =" & Str(Val(txt_Test1_Cone2_YarnWeight4.Text)) & "   ,   Test1_Cone2_YarnWeight5 = " & Str(Val(txt_Test1_Cone2_YarnWeight5.Text)) & "    ,   Test1_Cone2_YarnCount1 = " & Str(Val(lbl_Test1_Cone2_YarnCount1.Text)) & "  ,  Test1_Cone2_YarnCount2  = " & Str(Val(lbl_Test1_Cone2_YarnCount2.Text)) & "   , Test1_Cone2_YarnCount3  = " & Str(Val(lbl_Test1_Cone2_YarnCount3.Text)) & "  ,    Test1_Cone2_YarnCount4 = " & Str(Val(lbl_Test1_Cone2_YarnCount4.Text)) & "   , Test1_Cone2_YarnCount5   = " & Str(Val(lbl_Test1_Cone2_YarnCount5.Text)) & "    ,  Test1_Cone3_YarnWeight1  = " & Str(Val(txt_Test1_Cone3_YarnWeight1.Text)) & "    ,   Test1_Cone3_YarnWeight2  = " & Str(Val(txt_Test1_Cone3_YarnWeight2.Text)) & "  ,Test1_Cone3_YarnWeight3  = " & Str(Val(txt_Test1_Cone3_YarnWeight3.Text)) & "   ,  Test1_Cone3_YarnWeight4    = " & Str(Val(txt_Test1_Cone3_YarnWeight4.Text)) & "  ,   Test1_Cone3_YarnWeight5  = " & Str(Val(txt_Test1_Cone3_YarnWeight5.Text)) & "     ,  Test1_Cone3_YarnCount1  = " & Str(Val(lbl_Test1_Cone3_YarnCount1.Text)) & "   , Test1_Cone3_YarnCount2  = " & Str(Val(lbl_Test1_Cone3_YarnCount2.Text)) & "  , Test1_Cone3_YarnCount3 =" & Str(Val(lbl_Test1_Cone3_YarnCount3.Text)) & "    ,    Test1_Cone3_YarnCount4  =" & Str(Val(lbl_Test1_Cone3_YarnCount4.Text)) & "    ,   Test1_Cone3_YarnCount5 = " & Str(Val(lbl_Test1_Cone3_YarnCount5.Text)) & "     ,   Test1_Cone4_YarnWeight1 = " & Val(txt_Test1_Cone4_YarnWeight1.Text) & " , Test1_Cone4_YarnWeight2 = " & Val(txt_Test1_Cone4_YarnWeight2.Text) & "  ,Test1_Cone4_YarnWeight3  = " & Val(txt_Test1_Cone4_YarnWeight3.Text) & "   ,    Test1_Cone4_YarnWeight4   =" & Val(txt_Test1_Cone4_YarnWeight4.Text) & "    ,   Test1_Cone4_YarnWeight5 =" & Val(txt_Test1_Cone4_YarnWeight5.Text) & "  ,   Test1_Cone4_YarnCount1  = " & Str(Val(lbl_Test1_Cone4_YarnCount1.Text)) & "  ,  Test1_Cone4_YarnCount2    = " & Str(Val(lbl_Test1_Cone4_YarnCount2.Text)) & "  ,Test1_Cone4_YarnCount3 = " & Str(Val(lbl_Test1_Cone4_YarnCount3.Text)) & " ,    Test1_Cone4_YarnCount4  = " & Str(Val(lbl_Test1_Cone4_YarnCount4.Text)) & " ,  Test1_Cone4_YarnCount5  = " & Str(Val(lbl_Test1_Cone4_YarnCount5.Text)) & "     ,Test1_Cone5_YarnWeight1  = " & Str(Val(txt_Test1_Cone5_YarnWeight1.Text)) & " , Test1_Cone5_YarnWeight2  = " & Str(Val(txt_Test1_Cone5_YarnWeight2.Text)) & " , Test1_Cone5_YarnWeight3  = " & Str(Val(txt_Test1_Cone5_YarnWeight3.Text)) & "  , Test1_Cone5_YarnWeight4  = " & Str(Val(txt_Test1_Cone5_YarnWeight1.Text)) & " ,   Test1_Cone5_YarnWeight5 = " & Str(Val(txt_Test1_Cone5_YarnWeight5.Text)) & " ,  Test1_Cone5_YarnCount1 = " & Str(Val(lbl_Test1_Cone5_YarnCount1.Text)) & " , Test1_Cone5_YarnCount2 = " & Str(Val(lbl_Test1_Cone5_YarnCount2.Text)) & "   , Test1_Cone5_YarnCount3 = " & Str(Val(lbl_Test1_Cone5_YarnCount3.Text)) & "    ,  Test1_Cone5_YarnCount4   = " & Str(Val(lbl_Test1_Cone5_YarnCount4.Text)) & "       , Test1_Cone5_YarnCount5 = " & Str(Val(lbl_Test1_Cone5_YarnCount5.Text)) & "   , Test1_Cone6_YarnWeight1  = " & Str(Val(txt_Test1_Cone6_YarnWeight1.Text)) & "   , Test1_Cone6_YarnWeight2  = " & Str(Val(txt_Test1_Cone6_YarnWeight2.Text)) & "   , Test1_Cone6_YarnWeight3  = " & Val(txt_Test1_Cone6_YarnWeight3.Text) & "  ,    Test1_Cone6_YarnWeight4 = " & Val(txt_Test1_Cone6_YarnWeight4.Text) & ", Test1_Cone6_YarnWeight5 = " & Val(txt_Test1_Cone6_YarnWeight5.Text) & "  ,   Test1_Cone6_YarnCount1 = " & Val(lbl_Test1_Cone6_YarnCount1.Text) & "  , Test1_Cone6_YarnCount2   = " & Val(lbl_Test1_Cone6_YarnCount2.Text) & "    ,   Test1_Cone6_YarnCount3  = " & Val(lbl_Test1_Cone6_YarnCount3.Text) & "    ,    Test1_Cone6_YarnCount4 = " & Val(lbl_Test1_Cone6_YarnCount4.Text) & "   , Test1_Cone6_YarnCount5 = " & Val(lbl_Test1_Cone6_YarnCount5.Text) & "    ,   Test2_Cone1_YarnWeight1  = " & Val(txt_Test2_Cone1_YarnWeight1.Text) & " ,  Test2_Cone1_YarnWeight2 = " & Val(txt_Test2_Cone1_YarnWeight2.Text) & " ,  Test2_Cone1_YarnWeight3 = " & Val(txt_Test2_Cone1_YarnWeight3.Text) & " ,   Test2_Cone1_YarnWeight4 = " & Val(txt_Test2_Cone1_YarnWeight4.Text) & "  ,   Test2_Cone1_YarnWeight5  =  " & Val(txt_Test2_Cone1_YarnWeight5.Text) & "   ,   Test2_Cone1_YarnCount1  = " & Val(lbl_Test2_Cone1_YarnCount1.Text) & "   ,          Test2_Cone1_YarnCount2  = " & Val(lbl_Test2_Cone1_YarnCount2.Text) & "     , Test2_Cone1_YarnCount3   = " & Val(lbl_Test2_Cone1_YarnCount1.Text) & "   ,Test2_Cone1_YarnCount4  = " & Str(Val(lbl_Test2_Cone1_YarnCount4.Text)) & " ,  Test2_Cone1_YarnCount5 = " & Str(Val(lbl_Test2_Cone1_YarnCount5.Text)) & "  ,   Test2_Cone2_YarnWeight1  = " & Val(txt_Test2_Cone2_YarnWeight1.Text) & " , Test2_Cone2_YarnWeight2   = " & Val(txt_Test2_Cone2_YarnWeight2.Text) & "   ,Test2_Cone2_YarnWeight3  = " & Val(txt_Test2_Cone2_YarnWeight3.Text) & "  ,  Test2_Cone2_YarnWeight4  = " & Val(txt_Test2_Cone2_YarnWeight4.Text) & "  ,   Test2_Cone2_YarnWeight5  = " & Val(txt_Test2_Cone2_YarnWeight5.Text) & "   ,  Test2_Cone2_YarnCount1 = " & Str(Val(lbl_Test2_Cone2_YarnCount1.Text)) & "  ,Test2_Cone2_YarnCount2  = " & Str(Val(lbl_Test2_Cone2_YarnCount2.Text)) & "  ,  Test2_Cone2_YarnCount3  = " & Str(Val(lbl_Test2_Cone2_YarnCount3.Text)) & "   ,Test2_Cone2_YarnCount4   = " & Str(Val(lbl_Test2_Cone2_YarnCount4.Text)) & "  , Test2_Cone2_YarnCount5 = " & Str(Val(lbl_Test2_Cone2_YarnCount5.Text)) & "   , Test2_Cone3_YarnWeight1 = " & Val(txt_Test2_Cone3_YarnWeight1.Text) & "  ,   Test2_Cone3_YarnWeight2 = " & Val(txt_Test2_Cone3_YarnWeight2.Text) & " ,  Test2_Cone3_YarnWeight3 = " & Val(txt_Test2_Cone3_YarnWeight3.Text) & "  ,   Test2_Cone3_YarnWeight4  = " & Val(txt_Test2_Cone3_YarnWeight4.Text) & "  ,   Test2_Cone3_YarnWeight5 = " & Val(txt_Test2_Cone3_YarnWeight5.Text) & "  ,   Test2_Cone3_YarnCount1 = " & Str(Val(lbl_Test2_Cone3_YarnCount1.Text)) & "   , Test2_Cone3_YarnCount2   = " & Str(Val(lbl_Test2_Cone3_YarnCount2.Text)) & "  ,  Test2_Cone3_YarnCount3  = " & Str(Val(lbl_Test2_Cone3_YarnCount3.Text)) & "     ,    Test2_Cone3_YarnCount4  = " & Str(Val(lbl_Test2_Cone3_YarnCount4.Text)) & "   ,  Test2_Cone3_YarnCount5 = " & Str(Val(lbl_Test2_Cone3_YarnCount5.Text)) & ", Test2_Cone4_YarnWeight1  = " & Val(txt_Test2_Cone4_YarnWeight1.Text) & "    ,Test2_Cone4_YarnWeight2 = " & Val(txt_Test2_Cone4_YarnWeight2.Text) & "  , Test2_Cone4_YarnWeight3 = " & Val(txt_Test2_Cone4_YarnWeight3.Text) & "    ,    Test2_Cone4_YarnWeight4   = " & Val(txt_Test2_Cone4_YarnWeight4.Text) & "  ,   Test2_Cone4_YarnWeight5 = " & Val(txt_Test2_Cone4_YarnWeight5.Text) & "  ,   Test2_Cone4_YarnCount1 = " & Val(lbl_Test2_Cone4_YarnCount1.Text) & "   ,  Test2_Cone4_YarnCount2   = " & Val(lbl_Test2_Cone4_YarnCount2.Text) & ", Test2_Cone4_YarnCount3 = " & Val(lbl_Test2_Cone4_YarnCount3.Text) & "   ,    Test2_Cone4_YarnCount4  = " & Val(lbl_Test2_Cone4_YarnCount4.Text) & "   ,    Test2_Cone4_YarnCount5 = " & Val(lbl_Test2_Cone4_YarnCount5.Text) & "   ,  Test2_Cone5_YarnWeight1  = " & Val(txt_Test2_Cone5_YarnWeight1.Text) & " ,   Test2_Cone5_YarnWeight2 = " & Val(txt_Test2_Cone5_YarnWeight2.Text) & " ,  Test2_Cone5_YarnWeight3  = " & Val(txt_Test2_Cone5_YarnWeight3.Text) & " ,  Test2_Cone5_YarnWeight4  = " & Val(txt_Test2_Cone5_YarnWeight4.Text) & "      ,   Test2_Cone5_YarnWeight5   = " & Str(Val(txt_Test3_Cone4_YarnWeight2.Text)) & "   ,   Test2_Cone5_YarnCount1   = " & Val(lbl_Test2_Cone5_YarnCount1.Text) & "   ,  Test2_Cone5_YarnCount2  = " & Val(lbl_Test2_Cone5_YarnCount2.Text) & "  , Test2_Cone5_YarnCount3  = " & Val(lbl_Test2_Cone5_YarnCount3.Text) & "   ,    Test2_Cone5_YarnCount4  = " & Val(lbl_Test2_Cone5_YarnCount4.Text) & "   ,   Test2_Cone5_YarnCount5  = " & Val(lbl_Test2_Cone5_YarnCount5.Text) & "          ,           Test2_Cone6_YarnWeight1 =  " & Str(Val(txt_Test2_Cone6_YarnWeight1.Text)) & "  ,   Test2_Cone6_YarnWeight2  = " & Str(Val(txt_Test2_Cone6_YarnWeight2.Text)) & "  ,Test2_Cone6_YarnWeight3 = " & Str(Val(txt_Test2_Cone6_YarnWeight3.Text)) & "  ,Test2_Cone6_YarnWeight4 = " & Str(Val(txt_Test2_Cone6_YarnWeight4.Text)) & "  ,  Test2_Cone6_YarnWeight5  = " & Str(Val(txt_Test2_Cone6_YarnWeight5.Text)) & "   ,   Test2_Cone6_YarnCount1  = " & Str(Val(lbl_Test2_Cone6_YarnCount1.Text)) & "  ,Test2_Cone6_YarnCount2  = " & Str(Val(lbl_Test2_Cone6_YarnCount2.Text)) & "  , Test2_Cone6_YarnCount3  = " & Str(Val(lbl_Test2_Cone6_YarnCount3.Text)) & "  ,    Test2_Cone6_YarnCount4  = " & Str(Val(lbl_Test2_Cone6_YarnCount4.Text)) & "  ,   Test2_Cone6_YarnCount5 = " & Str(Val(lbl_Test2_Cone6_YarnCount5.Text)) & " ,   Test3_Cone1_YarnWeight1 = " & Str(Val(txt_Test3_Cone1_YarnWeight1.Text)) & " ,    Test3_Cone1_YarnWeight2  = " & Str(Val(txt_Test3_Cone1_YarnWeight2.Text)) & "  ,Test3_Cone1_YarnWeight3  = " & Str(Val(txt_Test3_Cone1_YarnWeight3.Text)) & ",    Test3_Cone1_YarnWeight4   = " & Str(Val(txt_Test3_Cone1_YarnWeight4.Text)) & " ,   Test3_Cone1_YarnWeight5 = " & Str(Val(txt_Test3_Cone1_YarnWeight5.Text)) & "  ,   Test3_Cone1_YarnCount1   = " & Str(Val(lbl_Test3_Cone1_YarnCount1.Text)) & " , Test3_Cone1_YarnCount2   = " & Str(Val(lbl_Test3_Cone1_YarnCount2.Text)) & ",  Test3_Cone1_YarnCount3  = " & Str(Val(lbl_Test3_Cone1_YarnCount3.Text)) & "  ,   Test3_Cone1_YarnCount4   = " & Str(Val(lbl_Test3_Cone1_YarnCount4.Text)) & "   , Test3_Cone1_YarnCount5   = " & Str(Val(lbl_Test3_Cone1_YarnCount5.Text)) & "          ,               Test3_Cone2_YarnWeight1  = " & Str(Val(txt_Test3_Cone2_YarnWeight1.Text)) & ",Test3_Cone2_YarnWeight2 = " & Str(Val(txt_Test3_Cone2_YarnWeight2.Text)) & "    , Test3_Cone2_YarnWeight3  = " & Str(Val(txt_Test3_Cone2_YarnWeight3.Text)) & "  ,Test3_Cone2_YarnWeight4 = " & Str(Val(txt_Test3_Cone2_YarnWeight4.Text)) & ",   Test3_Cone2_YarnWeight5 = " & Str(Val(txt_Test3_Cone2_YarnWeight5.Text)) & "  ,   Test3_Cone2_YarnCount1  = " & Str(Val(lbl_Test3_Cone2_YarnCount1.Text)) & " , Test3_Cone2_YarnCount2= " & Str(Val(lbl_Test3_Cone2_YarnCount2.Text)) & "  ,Test3_Cone2_YarnCount3 = " & Str(Val(lbl_Test3_Cone2_YarnCount3.Text)) & "  ,    Test3_Cone2_YarnCount4  = " & Str(Val(lbl_Test3_Cone2_YarnCount4.Text)) & "   , Test3_Cone2_YarnCount5 = " & Str(Val(lbl_Test3_Cone2_YarnCount5.Text)) & " , Test3_Cone3_YarnWeight1    = " & Val(txt_Test3_Cone3_YarnWeight1.Text) & ",  Test3_Cone3_YarnWeight2 = " & Val(txt_Test3_Cone3_YarnWeight2.Text) & "   ,   Test3_Cone3_YarnWeight3  = " & Val(txt_Test3_Cone3_YarnWeight3.Text) & "  ,    Test3_Cone3_YarnWeight4 = " & Val(txt_Test3_Cone3_YarnWeight4.Text) & "   ,   Test3_Cone3_YarnWeight5 = " & Val(txt_Test3_Cone3_YarnWeight5.Text) & "   ,Test3_Cone3_YarnCount1 = " & Str(Val(lbl_Test3_Cone3_YarnCount1.Text)) & "  ,          Test3_Cone3_YarnCount2 = " & Str(Val(lbl_Test3_Cone3_YarnCount2.Text)) & "  , Test3_Cone3_YarnCount3 = " & Str(Val(lbl_Test3_Cone3_YarnCount3.Text)) & "   ,    Test3_Cone3_YarnCount4    = " & Str(Val(lbl_Test3_Cone3_YarnCount4.Text)) & "   ,  Test3_Cone3_YarnCount5 = " & Str(Val(lbl_Test3_Cone3_YarnCount5.Text)) & "      , Test3_Cone4_YarnWeight1  = " & Str(Val(txt_Test3_Cone4_YarnWeight1.Text)) & "   ,     Test3_Cone4_YarnWeight2 = " & Str(Val(txt_Test3_Cone4_YarnWeight2.Text)) & "   ,Test3_Cone4_YarnWeight3    = " & Str(Val(txt_Test3_Cone4_YarnWeight3.Text)) & " ,    Test3_Cone4_YarnWeight4  = " & Str(Val(txt_Test3_Cone4_YarnWeight4.Text)) & "  ,   Test3_Cone4_YarnWeight5  = " & Str(Val(txt_Test3_Cone4_YarnWeight5.Text)) & "       ,   Test3_Cone4_YarnCount1 = " & Str(Val(lbl_Test3_Cone4_YarnCount1.Text)) & "  , Test3_Cone4_YarnCount2  = " & Str(Val(lbl_Test3_Cone4_YarnCount2.Text)) & "  , Test3_Cone4_YarnCount3 = " & Str(Val(lbl_Test3_Cone4_YarnCount3.Text)) & "  ,   Test3_Cone4_YarnCount4  = " & Str(Val(lbl_Test3_Cone4_YarnCount4.Text)) & "    ,    Test3_Cone4_YarnCount5 = " & Str(Val(lbl_Test3_Cone4_YarnCount5.Text)) & "   ,         Test3_Cone5_YarnWeight1   = " & Str(Val(txt_Test3_Cone5_YarnWeight1.Text)) & "     ,Test3_Cone5_YarnWeight2  = " & Str(Val(txt_Test3_Cone5_YarnWeight2.Text)) & "  ,  Test3_Cone5_YarnWeight3  = " & Str(Val(txt_Test3_Cone5_YarnWeight3.Text)) & "   ,    Test3_Cone5_YarnWeight4  = " & Str(Val(txt_Test3_Cone5_YarnWeight4.Text)) & "  ,   Test3_Cone5_YarnWeight5  = " & Str(Val(txt_Test3_Cone5_YarnWeight5.Text)) & " ,   Test3_Cone5_YarnCount1  = " & Str(Val(lbl_Test3_Cone5_YarnCount1.Text)) & " ,  Test3_Cone5_YarnCount2  = " & Str(Val(lbl_Test3_Cone5_YarnCount2.Text)) & "  ,Test3_Cone5_YarnCount3  = " & Str(Val(lbl_Test3_Cone5_YarnCount3.Text)) & "    ,   Test3_Cone5_YarnCount4 = " & Str(Val(lbl_Test3_Cone5_YarnCount4.Text)) & " , Test3_Cone5_YarnCount5  = " & Str(Val(lbl_Test3_Cone5_YarnCount5.Text)) & "   ,Test3_Cone6_YarnWeight1   = " & Str(Val(txt_Test3_Cone6_YarnWeight1.Text)) & " ,   Test3_Cone6_YarnWeight2 = " & Str(Val(txt_Test3_Cone6_YarnWeight2.Text)) & "     ,Test3_Cone6_YarnWeight3 = " & Str(Val(txt_Test3_Cone6_YarnWeight3.Text)) & ",    Test3_Cone6_YarnWeight4  = " & Str(Val(txt_Test3_Cone6_YarnWeight4.Text)) & "  ,   Test3_Cone6_YarnWeight5  =" & Str(Val(txt_Test3_Cone6_YarnWeight5.Text)) & "  ,   Test3_Cone6_YarnCount1  =" & Str(Val(lbl_Test3_Cone6_YarnCount1.Text)) & "  ,Test3_Cone6_YarnCount2 = " & Str(Val(lbl_Test3_Cone6_YarnCount2.Text)) & " , Test3_Cone6_YarnCount3 = " & Str(Val(lbl_Test3_Cone6_YarnCount3.Text)) & "   ,  Test3_Cone6_YarnCount4 = " & Str(Val(lbl_Test3_Cone6_YarnCount4.Text)) & " ,  Test3_Cone6_YarnCount5 = " & Str(Val(lbl_Test3_Cone6_YarnCount5.Text)) & "  , Test1_Cone1_Mean  = " & Str(Val(lbl_Test1_Cone1_Mean.Text)) & "  ,   Test1_Cone1_Cv = " & Str(Val(lbl_Test1_Cone1_CV.Text)) & "  , Test1_Cone1_SD = " & Str(Val(lbl_Test1_Cone1_SD.Text)) & "   , Test1_Cone2_Mean = " & Str(Val(lbl_Test1_Cone2_mean.Text)) & "  ,    Test1_Cone2_Cv   = " & Str(Val(lbl_Test1_Cone2_Cv.Text)) & "  ,    Test1_Cone2_SD = " & Str(Val(lbl_Test1_Cone2_SD.Text)) & ",  Test1_Cone3_Mean = " & Str(Val(lbl_Test1_Cone3_mean.Text)) & " , Test1_Cone3_Cv = " & Str(Val(lbl_Test1_Cone3_Cv.Text)) & " ,   Test1_Cone3_SD = " & Str(Val(lbl_Test1_Cone3_SD.Text)) & " , Test1_Cone4_Mean = " & Str(Val(lbl_Test1_Cone4_mean.Text)) & "  ,    Test1_Cone4_Cv  = " & Str(Val(lbl_Test1_Cone4_Cv.Text)) & " ,  Test1_Cone4_SD  = " & Str(Val(lbl_Test1_Cone4_SD.Text)) & "   , Test1_Cone5_Mean  = " & Str(Val(lbl_Test1_Cone5_mean.Text)) & "   ,  Test1_Cone5_Cv   = " & Str(Val(lbl_Test1_Cone5_Cv.Text)) & "  ,   Test1_Cone5_SD   = " & Str(Val(lbl_Test1_Cone5_SD.Text)) & " ,   Test1_Cone6_Mean = " & Str(Val(lbl_Test1_Cone6_mean.Text)) & "  ,Test1_Cone6_Cv = " & Str(Val(lbl_Test1_Cone6_Cv.Text)) & "  ,  Test1_Cone6_SD = " & Str(Val(lbl_Test1_Cone6_Sd.Text)) & "    ,Test1_Avg_Cv  = " & Str(Val(lbl_Test1_AvgCv.Text)) & "   ,Test1_Avg_Sd = " & Str(Val(lbl_test1_AvgSd.Text)) & " , Test1_Avg_Coul = " & Str(Val(lbl_Test1_AvgCoul.Text)) & "   , Test2_Cone1_Mean = " & Str(Val(lbl_Test2_Cone1_Mean.Text)) & "  ,    Test2_Cone1_Cv = " & Str(Val(lbl_Test2_Cone1_CV.Text)) & ",  Test2_Cone1_SD  = " & Str(Val(lbl_Test2_Cone1_SD.Text)) & "    , Test2_Cone2_Mean = " & Str(Val(lbl_Test2_Cone2_Mean.Text)) & "  ,    Test2_Cone2_Cv  = " & Str(Val(lbl_Test2_Cone2_CV.Text)) & "  ,Test2_Cone2_SD  = " & Str(Val(lbl_Test2_Cone2_SD.Text)) & "   ,   Test2_Cone3_Mean  = " & Str(Val(lbl_Test2_Cone3_Mean.Text)) & "   ,    Test2_Cone3_Cv  = " & Str(Val(lbl_Test2_Cone3_CV.Text)) & "   , Test2_Cone3_SD  = " & Str(Val(lbl_Test2_Cone3_SD.Text)) & "    ,Test2_Cone4_Mean = " & Str(Val(lbl_Test2_Cone4_Mean.Text)) & "    ,    Test2_Cone4_Cv  = " & Str(Val(lbl_Test2_Cone4_CV.Text)) & " , Test2_Cone4_SD = " & Str(Val(lbl_Test2_Cone4_SD.Text)) & " ,  Test2_Cone5_Mean  = " & Str(Val(lbl_Test2_Cone5_Mean.Text)) & "   ,Test2_Cone5_Cv = " & Str(Val(lbl_Test2_Cone5_CV.Text)) & " ,Test2_Cone5_SD =" & Str(Val(lbl_Test2_Cone5_SD.Text)) & "    ,Test2_Cone6_Mean =" & Str(Val(lbl_Test2_Cone6_Mean.Text)) & " , Test2_Cone6_Cv = " & Str(Val(lbl_Test2_Cone6_CV.Text)) & "   , Test2_Cone6_SD = " & Str(Val(lbl_Test2_Cone6_SD.Text)) & "  ,  Test2_Avg_Cv = " & Str(Val(lbl_Test2_AvgCv.Text)) & "  ,    Test2_Avg_Sd = " & Str(Val(lbl_Test2_Avg_Sd.Text)) & "  ,    Test2_Avg_Coul =" & Str(Val(lbl_Test2_avgCoul.Text)) & " ,Test3_Cone1_Mean =" & Str(Val(lbl_Test3_Cone1_Mean.Text)) & "  ,  Test3_Cone1_Cv   =" & Str(Val(lbl_Test3_Cone1_CV.Text)) & ",  Test3_Cone1_SD  =" & Str(Val(lbl_Test3_Cone2_SD.Text)) & " ,  Test3_Cone2_Mean  =" & Str(Val(lbl_Test3_Cone2_mean.Text)) & "  ,    Test3_Cone2_Cv =" & Str(Val(lbl_Test3_Cone2_CV.Text)) & " ,  Test3_Cone2_SD = " & Str(Val(lbl_Test3_Cone2_SD.Text)) & "  , Test3_Cone3_Mean = " & Str(Val(lbl_Test3_Cone3_mean.Text)) & "  ,    Test3_Cone3_Cv  =" & Str(Val(lbl_Test3_Cone3_CV.Text)) & ",   Test3_Cone3_SD = " & Str(Val(lbl_Test3_Cone3_SD.Text)) & "   ,  Test3_Cone4_Mean  = " & Str(Val(lbl_Test3_Cone4_mean.Text)) & "   ,    Test3_Cone4_Cv  = " & Str(Val(lbl_Test3_Cone4_CV.Text)) & "   ,  Test3_Cone4_SD  = " & Str(Val(lbl_Test3_Cone4_SD.Text)) & "  ,   Test3_Cone5_Mean = " & Str(Val(lbl_Test3_Cone5_Mean.Text)) & "  ,    Test3_Cone5_Cv   = " & Str(Val(lbl_Test3_Cone5_CV.Text)) & ",  Test3_Cone5_SD   = " & Str(Val(lbl_Test3_Cone5_SD.Text)) & "  ,        Test3_Cone6_Mean  = " & Str(Val(lbl_Test3_Cone6_Mean.Text)) & "    ,    Test3_Cone6_Cv  = " & Str(Val(lbl_Test3_Cone6_CV.Text)) & "  ,  Test3_Cone6_SD  = " & Str(Val(lbl_Test3_Cone6_SD.Text)) & "  ,  Test3_Avg_Cv =  " & Str(Val(lbl_Test3_AvgCv.Text)) & "  ,   Test3_Avg_Sd  =  " & Str(Val(lbl_Test3_AvgSd.Text)) & ", Test3_Avg_Coul =  " & Str(Val(lbl_Test3_AvgCoul.Text)) & ", Transport_Freight = " & Val(txt_Transport_Freight.Text) & ", User_IdNo = " & Val(lbl_UserName.Text) & " ,Purchase_Type = '" & Trim(cbo_Type.Text) & "', Bill_Date = @BillDate  Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Purchase_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update Yarn_Purchase_Receipt_Details set Purchase_Bags = a.Purchase_Bags - b.Bags,Purchase_Cones = a.Purchase_Cones - b.Cones,Purchase_Weight = a.Purchase_Weight - b.Weight from Yarn_Purchase_Receipt_Details a, Yarn_Purchase_Details b, Yarn_Purchase_Head c Where b.Yarn_Purchase_Code = '" & Trim(NewCode) & "' and c.Yarn_Purchase_Code = '" & Trim(NewCode) & "' and c.Purchase_Type = 'RECEIPT' and b.Yarn_Purchase_Code = c.Yarn_Purchase_Code and a.Yarn_Purchase_Receipt_Code = b.Yarn_Purchase_Receipt_Code and a.Yarn_Purchase_Receipt_Details_SlNo = b.Yarn_Purchase_Receipt_Details_SlNo"
                cmd.ExecuteNonQuery()

            End If

            EntID = Trim(Pk_Condition) & Trim(lbl_RefNo.Text)
            PBlNo = Trim(txt_BillNo.Text)
            Partcls = "Purc : Ref No. " & Trim(lbl_RefNo.Text)

            cmd.CommandText = "Delete from Yarn_Purchase_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Purchase_Code = '" & Trim(NewCode) & "'"
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

                        Cnt_ID = Common_Procedures.Count_NameToIdNo(con, .Rows(i).Cells(1).Value, tr)
                        Mill_ID = Common_Procedures.Mill_NameToIdNo(con, .Rows(i).Cells(2).Value, tr)
                        If dgv_Details.Columns(9).Visible = True Then
                            clr_ID = Common_Procedures.Colour_NameToIdNo(con, .Rows(i).Cells(9).Value, tr)
                        End If

                        If Trim(YrnClthNm) = "" Then YrnClthNm = Trim(.Rows(i).Cells(1).Value) & "/" & Trim(.Rows(i).Cells(2).Value)
                        RecNo = ""
                        RecCd = ""
                        RecSlNo = 0
                        If Trim(UCase(cbo_Type.Text)) = "RECEIPT" Then
                            RecNo = Trim(.Rows(i).Cells(10).Value)
                            RecCd = Trim(.Rows(i).Cells(11).Value)
                            RecSlNo = Val(.Rows(i).Cells(12).Value)
                        End If

                        cmd.CommandText = "Insert into Yarn_Purchase_Details ( Yarn_Purchase_Code ,               Company_IdNo       ,   Yarn_Purchase_No    ,                     for_OrderBy                                            ,              Yarn_Purchase_Date,             Sl_No     ,              Count_IdNo         ,          Mill_IdNo       ,                     Bags            ,                 Cones                ,                        Weight         ,                   Rate_For                       ,                     Rate                 ,                  Amount                     , Colour_Idno            ,   Yarn_Purchase_Receipt_No ,  Yarn_Purchase_Receipt_Code,  Yarn_Purchase_Receipt_Details_SlNo ) " & _
                                            "     Values                 (   '" & Trim(NewCode) & "'      , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",       @PurchaseDate            ,  " & Str(Val(Sno)) & ", " & Str(Val(Cnt_ID)) & ", " & Str(Val(Mill_ID)) & ", " & Str(Val(.Rows(i).Cells(3).Value)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & ", " & Str(Val(.Rows(i).Cells(5).Value)) & ", '" & Trim(.Rows(i).Cells(6).Value) & "', " & Str(Val(.Rows(i).Cells(7).Value)) & ", " & Str(Val(.Rows(i).Cells(8).Value)) & " , " & Str(Val(clr_ID)) & ",  '" & Trim(RecNo) & "'    , '" & Trim(RecCd) & "'      , " & Val(RecSlNo) & "                ) "
                        cmd.ExecuteNonQuery()


                        If Trim(UCase(cbo_Type.Text)) = "RECEIPT" Then
                            Nr = 0
                            cmd.CommandText = "Update Yarn_Purchase_Receipt_Details set Purchase_Bags = Purchase_Bags + " & Str(Val(.Rows(i).Cells(3).Value)) & ",Purchase_Cones = Purchase_Cones + " & Str(Val(.Rows(i).Cells(4).Value)) & ",Purchase_Weight = Purchase_Weight + " & Str(Val(.Rows(i).Cells(5).Value)) & " Where Yarn_Purchase_Receipt_Code = '" & Trim(RecCd) & "' and Yarn_Purchase_Receipt_Details_SlNo = " & Str(Val(RecSlNo))
                            Nr = cmd.ExecuteNonQuery()
                            If Nr = 0 Then
                                Throw New ApplicationException("Mismatch of Receipt and Party Details")
                            End If
                        End If

                        If Trim(UCase(cbo_Type.Text)) <> "RECEIPT" Then
                            cmd.CommandText = "Insert into Stock_Yarn_Processing_Details(Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Entry_ID, Particulars, Party_Bill_No, Sl_No, Count_IdNo, Yarn_Type, Mill_IdNo, Bags, Cones, Weight , Colour_IdNo, DeliveryToIdno_ForParticulars, ReceivedFromIdno_ForParticulars ) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ", @PurchaseDate, " & Str(Val(Del_ID)) & ", 0, '" & Trim(EntID) & "', '" & Trim(Partcls) & "', '" & Trim(PBlNo) & "', " & Str(Val(Sno)) & ", " & Str(Val(Cnt_ID)) & ", 'MILL', " & Str(Val(Mill_ID)) & ", " & Str(Val(.Rows(i).Cells(3).Value)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & ", " & Str(Val(.Rows(i).Cells(5).Value)) & " , " & Str(Val(clr_ID)) & ", 0, " & Str(Val(Led_ID)) & " )"
                            cmd.ExecuteNonQuery()
                        End If


                    End If

                Next

            End With

            If Val(vTotBgs) <> 0 Or Val(vTotCns) <> 0 Then
                cmd.CommandText = "Insert into Stock_Empty_BeamBagCone_Processing_Details(Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Entry_ID, Party_Bill_No, Sl_No, Beam_Width_IdNo, Empty_Beam, Empty_Bags, Empty_Cones, Particulars) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ", @PurchaseDate, " & Str(Val(Del_ID)) & ", 0, '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', 1, 0, 0, " & Str(Val(vTotBgs)) & ", " & Str(Val(vTotCns)) & ", '" & Trim(Partcls) & "')"
                cmd.ExecuteNonQuery()
            End If

            'AgentCommission Posting
            cmd.CommandText = "delete from AgentCommission_Processing_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            If Val(Agt_Idno) <> 0 Then

                cmd.CommandText = "Insert into AgentCommission_Processing_Details (  Reference_Code   ,             Company_IdNo         ,            Reference_No       ,                               For_OrderBy                              , Reference_Date, Commission_For,     Ledger_IdNo     ,      Agent_IdNo      ,         Entry_ID     ,      Party_BillNo    ,       Particulars      ,      Yarn_Cloth_Name     ,         Bags_Meters       ,               Amount               ,              Commission_Type      ,       Commission_Rate              ,            Commission_Amount         ) " & _
                                                " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",   @PurchaseDate   ,     'YARN'    , " & Str(Led_ID) & ", " & Str(Agt_Idno) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', '" & Trim(YrnClthNm) & "', " & Str(Val(vTotBgs)) & ", " & Str(Val(lbl_NetAmount.Text)) & ", '" & Trim(cbo_CommType.Text) & "', " & Str(Val(txt_CommRate.Text)) & ", " & Str(Val(lbl_CommAmount.Text)) & ")"
                cmd.ExecuteNonQuery()

            End If


            '-----A/c Posting

            vLed_IdNos = Led_ID & "|" & PurAc_ID & "|" & TxAc_ID
            vVou_Amts = Val(CSng(lbl_NetAmount.Text)) & "|" & -1 * (Val(CSng(lbl_NetAmount.Text)) - Val(lbl_TaxAmount.Text)) & "|" & -1 * Val(lbl_TaxAmount.Text)
            If Common_Procedures.Voucher_Updation(con, "Yarn.Purc", Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), Trim(lbl_RefNo.Text), Convert.ToDateTime(msk_BillDate.Text), "Bill No : " & Trim(txt_BillNo.Text), vLed_IdNos, vVou_Amts, ErrMsg, tr) = False Then
                Throw New ApplicationException(ErrMsg)
            End If


            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition2) & Trim(NewCode), tr)
            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition4) & Trim(NewCode), tr)

            Comm_Amt = 0
            ag_Comm = 0
            agtds_perc = 0

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1035" Then '---- Kalaimagal Textiles (Avinashi)
                If Val(lbl_CommAmount.Text) <> 0 Then
                    agtds_perc = Val(Common_Procedures.get_FieldValue(con, "Ledger_HEAD", "Tds_Percentage", "(Ledger_IdNo = " & Str(Val(Agt_Idno)) & ")", , tr))
                    If Val(agtds_perc) <> 0 Then
                        Comm_Amt = Val(lbl_CommAmount.Text)
                        ag_Comm = Val(lbl_CommAmount.Text) * agtds_perc / 100
                        'Comm_Amt = Comm_Amt - ag_Comm

                    Else
                        Comm_Amt = Val(lbl_CommAmount.Text)
                        ag_Comm = 0

                    End If

                    vLed_IdNos = Agt_Idno & "|" & Val(Common_Procedures.CommonLedger.Agent_Commission_Ac)
                    vVou_Amts = Val(Comm_Amt) & "|" & -1 * Val(Comm_Amt)
                    If Common_Procedures.Voucher_Updation(con, "Ag.Comm", Val(lbl_Company.Tag), Trim(Pk_Condition2) & Trim(NewCode), Trim(lbl_RefNo.Text), Convert.ToDateTime(msk_BillDate.Text), "Bill No : " & Trim(txt_BillNo.Text), vLed_IdNos, vVou_Amts, ErrMsg, tr) = False Then
                        Throw New ApplicationException(ErrMsg)
                    End If


                    vLed_IdNos = Agt_Idno & "|" & Val(Common_Procedures.CommonLedger.TDS_Payable_Ac)
                    vVou_Amts = -1 * Val(ag_Comm) & "|" & Val(ag_Comm)
                    If Common_Procedures.Voucher_Updation(con, "Agnt.Tds", Val(lbl_Company.Tag), Trim(Pk_Condition4) & Trim(NewCode), Trim(lbl_RefNo.Text), Convert.ToDateTime(msk_BillDate.Text), "Bill No. : " & Trim(txt_BillNo.Text), vLed_IdNos, vVou_Amts, ErrMsg, tr) = False Then
                        Throw New ApplicationException(ErrMsg)
                        Exit Sub
                    End If

                End If
            End If

            vLed_IdNos = Trans_ID & "|" & Val(Common_Procedures.CommonLedger.Transport_Charges_Ac)
            vVou_Amts = Val(txt_Transport_Freight.Text) & "|" & -1 * Val(txt_Transport_Freight.Text)
            If Common_Procedures.Voucher_Updation(con, "YPur.Frgt", Val(lbl_Company.Tag), Trim(Pk_Condition3) & Trim(NewCode), Trim(lbl_RefNo.Text), Convert.ToDateTime(msk_BillDate.Text), Partcls, vLed_IdNos, vVou_Amts, ErrMsg, tr) = False Then
                Throw New ApplicationException(ErrMsg)
            End If

            '-----Bill Posting
            VouBil = Common_Procedures.VoucherBill_Posting(con, Val(lbl_Company.Tag), Convert.ToDateTime(msk_BillDate.Text), Led_ID, Trim(txt_BillNo.Text), Agt_Idno, Val(CSng(lbl_NetAmount.Text)), "CR", Trim(Pk_Condition) & Trim(NewCode), tr)
            If Trim(UCase(VouBil)) = "ERROR" Then
                Throw New ApplicationException("Error on Voucher Bill Posting")
            End If

            tr.Commit()

            If SaveAll_STS <> True Then
                MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
            End If

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
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally

            Dt1.Dispose()
            Da.Dispose()
            cmd.Dispose()
            tr.Dispose()
            Dt1.Clear()

            If msk_Date.Enabled And msk_Date.Visible Then msk_Date.Focus()

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
                    If .CurrentCell.ColumnIndex = 3 Then
                        If Val(Cn_bag) <> 0 Then
                            .Rows(.CurrentRow.Index).Cells(4).Value = .Rows(.CurrentRow.Index).Cells(3).Value * Val(Cn_bag)
                        End If

                        If Val(Wgt_Bag) <> 0 Then
                            .Rows(.CurrentRow.Index).Cells(5).Value = Format(Val(.Rows(.CurrentRow.Index).Cells(3).Value) * Val(Wgt_Bag), "#########0.000")
                        End If

                    End If

                    If .CurrentCell.ColumnIndex = 4 Then
                        If Val(Wgt_Cn) <> 0 Then
                            .Rows(.CurrentRow.Index).Cells(5).Value = Format(.Rows(.CurrentRow.Index).Cells(4).Value * Val(Wgt_Cn), "##########0.000")
                        End If

                    End If

                End If

            End With

        End If

    End Sub

    Private Sub get_YarnCount_Calculation()
        Dim Cne_Wgt As Single = 0
        Dim Cnt As Single = 0

        Cnt = 64.814 / (Cne_Wgt)

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
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Transport, txt_AddLess_AfterTax, txt_Transport_Freight, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Transport_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Transport.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Transport, txt_Transport_Freight, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PartyName.GotFocus
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1094" Then '---- SivaPrakash Cotton Mills (Somanur)
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( (  ( Ledger_Type = 'GODOWN' or Ledger_Type = 'SIZING' or Ledger_Type = 'WEAVER' or Ledger_Type = '' or  Ledger_Type = 'AGENT') and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_IdNo = 0)")
        Else
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( ( Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_IdNo = 0)")
        End If

    End Sub

    Private Sub cbo_Party_Name_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyName.KeyDown
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1094" Then '---- SivaPrakash Cotton Mills (Somanur)
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PartyName, msk_Date, cbo_Type, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( (( Ledger_Type = 'GODOWN' or Ledger_Type = 'SIZING' or Ledger_Type = 'WEAVER' or Ledger_Type = '' or  Ledger_Type = 'AGENT') and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_IdNo = 0)")
        Else
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PartyName, msk_Date, cbo_Type, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( ( Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_IdNo = 0)")
        End If

    End Sub

    Private Sub cbo_Party_Name_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PartyName.KeyPress
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1094" Then '---- SivaPrakash Cotton Mills (Somanur)
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PartyName, cbo_Type, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( ( ( Ledger_Type = 'GODOWN' or Ledger_Type = 'SIZING' or Ledger_Type = 'WEAVER' or Ledger_Type = '' or  Ledger_Type = 'AGENT') and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_IdNo = 0)")
        Else
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PartyName, cbo_Type, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( ( Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_IdNo = 0)")
        End If

    End Sub

    Private Sub cbo_Delvat_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Delvat.GotFocus
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1019" Then '---- SUBHAM Textiles (Somanur)
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'GODOWN')", "(Ledger_IdNo = 0)")
        Else
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'GODOWN' or Ledger_Type = 'SIZING' or Ledger_Type = 'WEAVER' or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_IdNo = 0)")
        End If
    End Sub

    Private Sub cbo_Delvat_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Delvat.KeyDown
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1019" Then '---- SUBHAM Textiles (Somanur)
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Delvat, msk_BillDate, txt_RecNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'GODOWN')", "(Ledger_IdNo = 0)")
        Else
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Delvat, msk_BillDate, txt_RecNo, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'GODOWN' or Ledger_Type = 'SIZING' or Ledger_Type = 'WEAVER' or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_IdNo = 0)")
        End If

    End Sub

    Private Sub cbo_Delvat_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Delvat.KeyPress
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1019" Then '---- SUBHAM Textiles (Somanur)
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Delvat, txt_RecNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'GODOWN')", "(Ledger_IdNo = 0)")
        Else
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Delvat, txt_RecNo, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'GODOWN' or Ledger_Type = 'SIZING' or Ledger_Type = 'WEAVER' or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_IdNo = 0)")
        End If
    End Sub

    Private Sub cbo_PurchaseAc_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PurchaseAc.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 27)", "(Ledger_IdNo = 0)")
    End Sub


    Private Sub cbo_PurchaseAc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PurchaseAc.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PurchaseAc, cbo_Type, txt_BillNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 27)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_PurchaseAc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PurchaseAc.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PurchaseAc, txt_BillNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 27)", "(Ledger_IdNo = 0)")
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
                Condt = "a.Yarn_Purchase_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Yarn_Purchase_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Yarn_Purchase_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If
            If Trim(cbo_Filter_DeliveryAt.Text) <> "" Then
                Del_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_DeliveryAt.Text)
            End If

            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Ledger_IdNo = " & Str(Val(Led_IdNo)) & " "
            End If
            If Val(Del_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.DeliveryTo_Idno = " & Str(Val(Del_IdNo)) & " "
            End If

            If Trim(txt_FilterBillNo.Text) <> "" Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Bill_No = '" & Trim(txt_FilterBillNo.Text) & "' "
            End If

            da = New SqlClient.SqlDataAdapter("select a.*, b.*, c.Ledger_Name as PartyName, d.Ledger_Name as Delv_Name from Yarn_Purchase_Head a INNER JOIN Yarn_Purchase_Details b ON a.Yarn_Purchase_Code = b.Yarn_Purchase_Code INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Ledger_Head d ON a.DeliveryTo_Idno = d.Ledger_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Entry_VAT_GST_Type <> 'GST' and a.Yarn_Purchase_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Yarn_Purchase_No", con)
            'da = New SqlClient.SqlDataAdapter("select a.*, b.*, c.Ledger_Name as Delv_Name from Yarn_Purchase_Head a INNER JOIN Yarn_Purchase_Details b ON a.Yarn_Purchase_Code = b.Yarn_Purchase_Code LEFT OUTER JOIN Ledger_Head c ON a.DeliveryTo_Idno = c.Ledger_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Yarn_Purchase_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Yarn_Purchase_No", con)
            dt2 = New DataTable
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    'dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(0).Value = dt2.Rows(i).Item("Yarn_Purchase_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(1).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Yarn_Purchase_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("PartyName").ToString
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Delv_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = dt2.Rows(i).Item("Bill_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(5).Value = Val(dt2.Rows(i).Item("Bags").ToString)
                    dgv_Filter_Details.Rows(n).Cells(6).Value = Val(dt2.Rows(i).Item("Cones").ToString)
                    dgv_Filter_Details.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Weight").ToString), "########0.000")
                    dgv_Filter_Details.Rows(n).Cells(8).Value = Format(Val(dt2.Rows(i).Item("Net_Amount").ToString), "########0.00")

                Next i

            End If

            dt2.Clear()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FILTER...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            dt2.Dispose()
            da.Dispose()

            If dgv_Filter_Details.Visible And dgv_Filter_Details.Enabled Then dgv_Filter_Details.Focus()

        End Try

    End Sub

    Private Sub cbo_Filter_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( ( Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, dtp_Filter_ToDate, txt_FilterBillNo, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( ( Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, txt_FilterBillNo, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( ( Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_IdNo = 0)")
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
        Try
            With dgv_Details
                If .Visible = True Then
                    If .Rows.Count > 0 Then
                        dgv_Details_CellLeave(sender, e)
                        If dgv_Details.CurrentCell.ColumnIndex = 3 Or dgv_Details.CurrentCell.ColumnIndex = 4 Then
                            get_MillCount_Details()
                        End If
                    End If
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL END EDIT....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub dgv_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim rect As Rectangle


        Try
            With dgv_Details
                If .Visible = True Then
                    If .Rows.Count > 0 Then

                        If Val(.CurrentRow.Cells(0).Value) = 0 Then
                            .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
                        End If

                        If e.ColumnIndex = 1 Then

                            If cbo_Grid_CountName.Visible = False And Trim(UCase(cbo_Type.Text)) <> "RECEIPT" Or Val(cbo_Grid_CountName.Tag) <> e.RowIndex Then

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

                            If cbo_Grid_MillName.Visible = False And Trim(UCase(cbo_Type.Text)) <> "RECEIPT" Or Val(cbo_Grid_MillName.Tag) <> e.RowIndex Then

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
                        If dgv_Details.Columns(9).Visible = True Then
                            If e.ColumnIndex = 9 Then
                                If cbo_Colour.Visible = False Or Val(cbo_Colour.Tag) <> e.RowIndex Then

                                    cbo_Colour.Tag = -1
                                    Da = New SqlClient.SqlDataAdapter("select Colour_Name from Colour_Head order by Colour_Name", con)
                                    Dt2 = New DataTable
                                    Da.Fill(Dt2)
                                    cbo_Colour.DataSource = Dt2
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

                                'cbo_Grid_MillName.Tag = -1
                                'cbo_Grid_MillName.Text = ""
                                cbo_Colour.Visible = False


                            End If
                        End If


                    End If
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL ENTER....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub dgv_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellLeave
        Try
            With dgv_Details
                If .Visible = True Then
                    If .Rows.Count > 0 Then
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
                    End If
                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL LEAVE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub dgv_Details_CellValidating(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellValidatingEventArgs) Handles dgv_Details.CellValidating

    End Sub

    Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged
        Try

            With dgv_Details
                If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
                If .Visible Then
                    If .Rows.Count > 0 Then
                        If .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 6 Or .CurrentCell.ColumnIndex = 7 Then

                            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1035" Then

                                Amount_Calculation(e.RowIndex, e.ColumnIndex)

                            End If

                        End If

                    End If

                End If

            End With

        Catch ex As NullReferenceException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As ObjectDisposedException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub dgv_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Details.EditingControlShowing
        Try
            With dgv_Details
                If .Rows.Count > 0 Then
                    dgtxt_Details = CType(dgv_Details.EditingControl, DataGridViewTextBoxEditingControl)
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS EDITING SHOWING....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.Enter
        Try
            dgv_Details.EditingControl.BackColor = Color.Lime
            dgv_Details.EditingControl.ForeColor = Color.Blue
            dgtxt_Details.SelectAll()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXT ENTER....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Details.KeyPress
        Try
            With dgv_Details
                If .Visible Then

                    If .Rows.Count > 0 Then

                        If .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 6 Then

                            If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                                e.Handled = True
                            End If

                        End If

                    End If

                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXT KEYPRESS....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub dgv_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyDown

        Try
            With dgv_Details

                If e.KeyCode = Keys.Up Then
                    If .CurrentCell.ColumnIndex <= 1 Then
                        If .CurrentCell.RowIndex = 0 Then
                            lbl_CommAmount.Focus()
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

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS KEYDOWN....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub dgv_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyUp
        Dim i As Integer
        Dim n As Integer

        Try
            If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

                With dgv_Details
                    If .Rows.Count > 0 Then

                        n = .CurrentRow.Index

                        If .CurrentCell.RowIndex = .Rows.Count - 1 Then
                            For i = 0 To .ColumnCount - 1
                                .Rows(n).Cells(i).Value = ""
                            Next

                        Else
                            .Rows.RemoveAt(n)

                        End If

                        Total_Calculation()

                    End If

                End With

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS KEYUP....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try



    End Sub

    Private Sub dgv_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.LostFocus
        On Error Resume Next
        If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
    End Sub

    Private Sub dgv_Details_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Details.RowsAdded
        Dim n As Integer = 0
        If Not IsNothing(dgv_Details.CurrentCell) Then Exit Sub
        Try
            With dgv_Details
                If .Rows.Count > 0 Then
                    .Rows(e.RowIndex).Cells(0).Value = Val(e.RowIndex) + 1
                    If e.RowIndex > 0 Then
                        .Rows(e.RowIndex).Cells(6).Value = Trim(UCase(.Rows(e.RowIndex - 1).Cells(6).Value))
                    Else
                        .Rows(e.RowIndex).Cells(6).Value = "KG"
                    End If
                    'n = .RowCount
                    '.Rows(n - 1).Cells(0).Value = Val(n)
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS ROWS ADD....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

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

    Private Sub btn_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        save_record()
    End Sub

    Private Sub btn_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Close()
    End Sub

    Private Sub txt_AddLess_BeforeTax_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_AddLess_BeforeTax.KeyDown
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1035" Then
                cbo_VatAc.Focus()
            Else
                txt_AssessableValue.Focus()
            End If
        End If

    End Sub

    Private Sub txt_AddLess_BeforeTax_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_AddLess_BeforeTax.KeyPress

        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1035" Then
                cbo_VatAc.Focus()
            Else
                txt_AssessableValue.Focus()
            End If

        End If

    End Sub

    Private Sub txt_AddLess_BeforeTax_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_AddLess_BeforeTax.LostFocus
        If Val(txt_AddLess_BeforeTax.Text) <> 0 Then
            txt_AddLess_BeforeTax.Text = Format(Val(txt_AddLess_BeforeTax.Text), "#########0.00")
        Else
            txt_AddLess_BeforeTax.Text = ""
        End If
    End Sub

    Private Sub txt_AddLess_BeforeTax_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_AddLess_BeforeTax.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub txt_AddLess_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_AddLess_AfterTax.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_AddLess_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_AddLess_AfterTax.LostFocus
        If Val(txt_AddLess_AfterTax.Text) <> 0 Then
            txt_AddLess_AfterTax.Text = Format(Val(txt_AddLess_AfterTax.Text), "#########0.00")
        Else
            txt_AddLess_AfterTax.Text = ""
        End If
    End Sub

    Private Sub txt_AddLess_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_AddLess_AfterTax.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub txt_Freight_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Freight.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Freight_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Freight.LostFocus
        If Val(txt_Freight.Text) <> 0 Then
            txt_Freight.Text = Format(Val(txt_Freight.Text), "#########0.00")
        Else
            txt_Freight.Text = ""
        End If

    End Sub

    Private Sub txt_Freight_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_Freight.TextChanged
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
            If Trim(UCase(cbo_Type.Text)) = "RECEIPT" Then
                If dgv_Details.Rows.Count > 0 Then


                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(3)
                Else
                    txt_CommRate.Focus()
                End If
            Else

                If dgv_Details.Rows.Count > 0 Then


                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
                Else
                    txt_CommRate.Focus()
                End If
            End If
        End If

        If e.KeyValue = 40 Then SendKeys.Send("{TAB}")

    End Sub

    Private Sub txt_DiscPerc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_DiscPerc.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_DiscPerc_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_DiscPerc.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub txt_Note_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Note.KeyDown
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()
            Else
                msk_Date.Focus()
                'dtp_Date.Focus()
            End If
        End If
    End Sub

    Private Sub txt_Note_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Note.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()
            Else
                msk_Date.Focus()
                'dtp_Date.Focus()
            End If
        End If
    End Sub

    Private Sub Amount_Calculation(ByVal CurRow As Integer, ByVal CurCol As Integer)
        Try
            With dgv_Details
                If .Visible Then
                    If .Rows.Count > 0 Then
                        If CurCol = 3 Or CurCol = 5 Or CurCol = 6 Or CurCol = 7 Then

                            If Trim(UCase(.Rows(CurRow).Cells(6).Value)) = "BAG" Then
                                .Rows(CurRow).Cells(8).Value = Format(Val(.Rows(CurRow).Cells(3).Value) * Val(.Rows(CurRow).Cells(7).Value), "#########0.00")
                            Else
                                .Rows(CurRow).Cells(8).Value = Format(Val(.Rows(CurRow).Cells(5).Value) * Val(.Rows(CurRow).Cells(7).Value), "#########0.00")
                            End If

                            Total_Calculation()

                        End If

                    End If

                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "AMOUNT CALCULATION....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try


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

        lbl_CommAmount.Text = Format(Val(AgCommAmt), "#########0.00")

    End Sub

    Private Sub NetAmount_Calculation()
        Dim NtAmt As Single

        If NoCalc_Status = True Then Exit Sub

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1035" Then

            lbl_DiscAmount.Text = Format(Val(lbl_GrossAmount.Text) * Val(txt_DiscPerc.Text) / 100, "########0.00")

            txt_AssessableValue.Text = Format(Val(lbl_GrossAmount.Text) - Val(lbl_DiscAmount.Text) + Val(txt_AddLess_BeforeTax.Text), "########0.00")

        End If

        lbl_TaxAmount.Text = Format(Val(txt_AssessableValue.Text) * Val(txt_TaxPerc.Text) / 100, "########0.00")

        NtAmt = Val(txt_AssessableValue.Text) + Val(lbl_TaxAmount.Text) + Val(txt_Freight.Text) + Val(txt_AddLess_AfterTax.Text)

        lbl_NetAmount.Text = Format(Val(NtAmt), "##########0")
        lbl_NetAmount.Text = Common_Procedures.Currency_Format(Val(lbl_NetAmount.Text))

        lbl_RoundOff.Text = Format(Val(CSng(lbl_NetAmount.Text)) - Val(NtAmt), "#########0.00")
        If Val(lbl_RoundOff.Text) = 0 Then lbl_RoundOff.Text = ""

        lbl_AmountInWords.Text = "Rupees  :  "
        If Val(CSng(lbl_NetAmount.Text)) <> 0 Then
            lbl_AmountInWords.Text = "Rupees  :  " & Common_Procedures.Rupees_Converstion(Val(CSng(lbl_NetAmount.Text)))
        End If

    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim ps As Printing.PaperSize
        Dim PpSzSTS As Boolean = False
        Dim NewCode As String = ""

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.PrintEntry, Common_Procedures.UR.Yarn_Purchase_Entry, New_Entry) = False Then Exit Sub

        Try

            da1 = New SqlClient.SqlDataAdapter("select * from Yarn_Purchase_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Purchase_Code = '" & Trim(NewCode) & "'", con)
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


        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then
            Try

                If Val(Common_Procedures.settings.Printing_Show_PrintDialogue) = 1 Then
                    PrintDialog1.PrinterSettings = PrintDocument1.PrinterSettings
                    If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then

                        PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings

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

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.*, d.Ledger_Name as Transport_Name , e.Ledger_Name as Agent_Name  from Yarn_Purchase_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Ledger_Head d ON d.Ledger_IdNo = a.Transport_IdNo LEFT OUTER JOIN Ledger_Head e ON e.Ledger_IdNo = a.Agent_IdNo  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Yarn_Purchase_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.count_name , c.Mill_Name  from Yarn_Purchase_Details a INNER JOIN Count_Head b ON a.Count_IdNo = b.Count_IdNo LEFT OUTER JOIN Mill_Head c ON a.Mill_IdNo = c.Mill_IdNo where a.Yarn_Purchase_Code = '" & Trim(NewCode) & "' Order by a.for_orderby, a.Yarn_Purchase_No", con)
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
        Printing_Format2(e)
        'Printing_Format1(e)
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
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                e.PageSettings.PaperSize = ps
                Exit For
            End If
        Next

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 40
            .Right = 45
            .Top = 45
            .Bottom = 45
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

        NoofItems_PerPage = 15 ' 6

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = Val(50) : ClArr(2) = 60 : ClArr(3) = 220 : ClArr(4) = 80 : ClArr(5) = 120 : ClArr(6) = 100
        ClArr(7) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6))

        TxtHgt = 18.5 ' 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format1_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)


                ' W1 = e.Graphics.MeasureString("No.of Beams  : ", pFont).Width

                NoofDets = 0

                CurY = CurY - 10

                'CurY = CurY + TxtHgt
                'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Mill_Name").ToString, LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)

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

                        SNo = SNo + 1
                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(SNo)), LMargin + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Count_Name").ToString, LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 10, CurY, 1, 0, pFont)
                        End If
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Weight").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Rate").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Amount").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

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


            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format1_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim strHeight As Single
        Dim C1 As Single, C2 As Single, W1 As Single, S1 As Single
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
        Dim DelvToName As String

        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name, c.Ledger_Name as Transport_Name from Yarn_Purchase_Head a  INNER JOIN Ledger_Head b ON b.Ledger_IdNo = a.Ledger_Idno LEFT OUTER JOIN Ledger_Head c ON c.Ledger_IdNo = a.Transport_IdNo  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Yarn_Purchase_Code = '" & Trim(EntryCode) & "' Order by a.For_OrderBy", con)
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
        Common_Procedures.Print_To_PrintDocument(e, " YARN PURCHASE RECEIPT", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try

            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 50
            C2 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 50
            W1 = e.Graphics.MeasureString("PURCHASE NO   : ", pFont).Width
            S1 = e.Graphics.MeasureString("TO     :    ", pFont).Width

            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "FROM :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "Purchase No", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Yarn_Purchase_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 14, FontStyle.Bold)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Purchase Date", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Yarn_Purchase_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Bill NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Bill_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + C1, LnAr(3), LMargin + C1, LnAr(2))
            CurY = CurY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, "Agent Name", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Agent_Name").ToString, LMargin + W1 + 30, CurY, 0, 0, pFont)

            'Common_Procedures.Print_To_PrintDocument(e, "Transport Name : " & Trim(prn_HdDt.Rows(0).Item("Transport_Name").ToString), LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Transport Name", LMargin + C2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Transport_Name").ToString, LMargin + C2 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 10

            DelvToName = Common_Procedures.Ledger_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("DeliveryTo_Idno").ToString))

            'Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Delivery_Address2").ToString), LMargin + S1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Delivery At", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, DelvToName, LMargin + W1 + 30, CurY, 0, 0, pFont)


            Common_Procedures.Print_To_PrintDocument(e, "Rec No", LMargin + C2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Delivery_Receipt_No").ToString, LMargin + C2 + W1 + 30, CurY, 0, 0, pFont)
            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("Delivery_Address3").ToString), LMargin + S1 + 10, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "COUNT", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "MILL NAME", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, " BAG", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "TOT WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "RATE/KG ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format1_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim I As Integer
        Dim Cmp_Name As String
        Dim C1 As Single, W1 As Single
        'Dim W1 As Single = 0
        Dim BmsInWrds As String

        ' W1 = e.Graphics.MeasureString("No.of Beams  : ", pFont).Width

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
            CurY = CurY + TxtHgt
            If Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Discount", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "( - )", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Discount_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            If Val(prn_HdDt.Rows(0).Item("Tax_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "VAT. 5 % ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "( + )", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Tax_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            If Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Freight", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "( + )", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Freight_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "AddLess", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "( + )", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            End If


            CurY = CurY + TxtHgt
            If Val(prn_HdDt.Rows(0).Item("RoundOff_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "RoundOff ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "( + )", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("RoundOff_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt + 5

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, PageWidth, CurY)
            LnAr(8) = CurY
            CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "E & O.E", LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Total", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("Net_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt + 15
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(9) = CurY
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))
            CurY = CurY + TxtHgt
            BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))
            BmsInWrds = Replace(Trim(LCase(BmsInWrds)), "", "")

            Common_Procedures.Print_To_PrintDocument(e, "Rupees            : " & BmsInWrds & " ", LMargin + 10, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            'CurY = CurY + 10
            'p1Font = New Font("Calibri", 12, FontStyle.Underline)
            'Common_Procedures.Print_To_PrintDocument(e, "Term & Conditions : ", LMargin + 10, CurY, 0, 0, p1Font)

            'CurY = CurY + TxtHgt + 10
            'Common_Procedures.Print_To_PrintDocument(e, "1. We are responsible for the quality of yarn only;If any running fault or quality  ", LMargin + 10, CurY, 0, 0, pFont)

            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "   defect noted in yarn please inform with firat fabric roll at once.We will", LMargin + 10, CurY, 0, 0, pFont)
            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "   accept only one roll at defect otherwise we do not hold ourself responsible. ", LMargin + 10, CurY, 0, 0, pFont)

            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "2. Our responsibility ceases when goods leave our permises.", LMargin + 10, CurY, 0, 0, pFont)

            'CurY = CurY + TxtHgt
            'If Val(Common_Procedures.User.IdNo) <> 1 Then
            '    Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
            'End If


            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)
            'Common_Procedures.Print_To_PrintDocument(e, "3. Subject to Tirupur jurisdiction. ", LMargin + 10, CurY, 0, 0, pFont)

            '' Common_Procedures.Print_To_PrintDocument(e, "For ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 30, CurY, 1, 0, p1Font)
            CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "4. Interest at the rate of 24% will be charge from the due date.", LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "4. All payment should be made by A/C payesr cheque or draft.", LMargin + 10, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt + 10
            Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 20, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 250, CurY, 0, 0, pFont)



            Common_Procedures.Print_To_PrintDocument(e, "AUTHORISED SIGNATORY ", PageWidth - 5, CurY, 1, 0, pFont)
            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format2(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
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
        Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim W1 As Single = 0

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
            If ps.Width = 800 And ps.Height = 600 Then
                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                e.PageSettings.PaperSize = ps
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
                    e.PageSettings.PaperSize = ps
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
                        e.PageSettings.PaperSize = ps
                        Exit For
                    End If
                Next
            End If

        End If

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 50
            .Right = 50

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1061" OR Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1558" Then
                .Top = 300
            Else
                .Top = 30
            End If

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

        NoofItems_PerPage = 4 ' 6

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = Val(60) : ClArr(2) = 210 : ClArr(3) = 150 : ClArr(4) = 150 : ClArr(5) = 160
        ClArr(6) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5))

        TxtHgt = 18.5 ' 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format2_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

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

                        CurY = CurY + TxtHgt

                        prn_DetSNo = prn_DetSNo + 1
                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetSNo)), LMargin + 15, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Count_Name").ToString, LMargin + ClArr(1) + 11, CurY, 0, 0, pFont)
                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) - 15, CurY, 1, 0, pFont)
                        End If

                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Cones").ToString) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Cones").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 15, CurY, 1, 0, pFont)
                        End If
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Weight").ToString, PageWidth - 15, CurY, 1, 0, pFont)

                        NoofDets = NoofDets + 1

                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If


                Printing_Format2_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)


            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format2_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim strHeight As Single
        Dim C1 As Single, C2 As Single, W1 As Single, S1 As Single
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
        Dim DelvToName As String

        PageNo = PageNo + 1

        CurY = TMargin

        If prn_DetDt.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If

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
        Common_Procedures.Print_To_PrintDocument(e, " YARN PURCHASE RECEIPT", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try
            C1 = ClAr(1) + ClAr(2) + ClAr(3)
            C2 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 50
            W1 = e.Graphics.MeasureString("PURCHASE NO   : ", pFont).Width
            S1 = e.Graphics.MeasureString("TO     :    ", pFont).Width

            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "TO :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            'Common_Procedures.Print_To_PrintDocument(e, "Purchase No", LMargin + C1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Yarn_Purchase_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "Bill NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Bill_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 14, FontStyle.Bold)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Bill Date", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Yarn_Purchase_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            DelvToName = Common_Procedures.Ledger_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("DeliveryTo_Idno").ToString))


            Common_Procedures.Print_To_PrintDocument(e, "Delivery To", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, DelvToName, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt - 5




            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY
            e.Graphics.DrawLine(Pens.Black, LMargin + C1, LnAr(4), LMargin + C1, LnAr(2))
            'CurY = CurY + TxtHgt

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "S.NO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "COUNT NAME", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "BAGS", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "CONES", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            ' Common_Procedures.Print_To_PrintDocument(e, "RATE/KG ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            ' Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format2_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim I As Integer
        Dim Cmp_Name As String
        Dim C1 As Single, W1 As Single

        Try

            For I = NoofDets + 1 To NoofItems_PerPage

                CurY = CurY + TxtHgt


            Next

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY



            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(4))
            ' e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(4))
            ' e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))
            C1 = ClAr(1) + ClAr(2) + ClAr(3) - 100
            W1 = e.Graphics.MeasureString("AGENT  : ", pFont).Width
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Agent :", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Agent_Name").ToString), LMargin + W1 + 30, CurY, 0, 0, pFont)

            If Val(prn_HdDt.Rows(0).Item("Agent_Commission_Rate").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Comm ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Agent_Commission_Rate").ToString), LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
            End If

            If Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Bill Amount   :  " & Trim(Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))), PageWidth - 15, CurY, 1, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "Bill Amount   :  ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 110, CurY, 0, 0, pFont)
            End If
            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(7) = CurY

            CurY = CurY + TxtHgt

            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt + 10
            Common_Procedures.Print_To_PrintDocument(e, "Signature of the Receiver", LMargin + 20, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 250, CurY, 0, 0, pFont)


            'Common_Procedures.Print_To_PrintDocument(e, "AUTHORISED SIGNATORY ", PageWidth - 5, CurY, 1, 0, pFont)
            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_Agent_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Agent.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'AGENT')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Agent_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Agent.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Agent, txt_RecNo, txt_CommRate, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'AGENT')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Agent_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Agent.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Agent, txt_CommRate, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'AGENT')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_VatAc_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_VatAc.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 12)", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_VatAc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_VatAc.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_VatAc, Nothing, cbo_TaxType, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 12)", "(Ledger_IdNo = 0)")

        If (e.KeyValue = 38 And cbo_Grid_CountName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1035" Then
                txt_AddLess_BeforeTax.Focus()
            Else
                txt_AssessableValue.Focus()
            End If
        End If

    End Sub

    Private Sub cbo_VatAc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_VatAc.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_VatAc, cbo_TaxType, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 12)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Agent_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Agent.KeyUp
        If e.Control = False And e.KeyValue = 17 Then

            Common_Procedures.MDI_LedType = ""
            Dim f As New Agent_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Agent.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If

    End Sub

    Private Sub cbo_SalesAc_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PurchaseAc.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Common_Procedures.MDI_LedType = ""
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_PurchaseAc.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_Delvat_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Delvat.KeyUp
        If e.Control = False And e.KeyValue = 17 Then

            Common_Procedures.MDI_LedType = "SIZING"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Delvat.Name
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
                    txt_CommRate.Focus()
                    'dgv_Details.Focus()
                    'dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
                    'dgv_Details.CurrentCell.Selected = True

                Else
                    If dgv_Details.Columns(9).Visible = True Then
                        .Focus()
                        .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(9)
                        .CurrentCell.Selected = True
                    Else
                        .Focus()
                        .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(6)
                        .CurrentCell.Selected = True
                    End If


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

    Private Sub cbo_Grid_CountName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_CountName.TextChanged
        Try
            If cbo_Grid_CountName.Visible Then
                With dgv_Details
                    If Val(cbo_Grid_CountName.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_CountName.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Colour_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Colour.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Colour_Head", "Colour_Name", "", "(Colour_IdNo = 0)")
    End Sub

    Private Sub cbo_Colour_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Colour.KeyDown
        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Colour, Nothing, Nothing, "Colour_Head", "Colour_Name", "", "(Colour_IdNo = 0)")

        With dgv_Details

            If (e.KeyValue = 38 And cbo_Colour.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_Colour.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                If .CurrentRow.Index = .Rows.Count - 1 Then

                    txt_DiscPerc.Focus()

                Else
                    dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentRow.Index + 1).Cells(1)

                End If


            End If

        End With
    End Sub

    Private Sub cbo_Colour_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Colour.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Colour, Nothing, "Colour_Head", "Colour_Name", "", "(Colour_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then

            With dgv_Details
                If .CurrentRow.Index = .Rows.Count - 1 Then

                    txt_DiscPerc.Focus()

                Else
                    dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentRow.Index + 1).Cells(1)

                End If
            End With

        End If
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
                With dgv_Details
                    If Val(cbo_Grid_MillName.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 2 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_MillName.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Grid_RateFor_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_RateFor.GotFocus
        vCbo_ItmNm = Trim(cbo_Grid_RateFor.Text)
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

    Private Sub cbo_Filter_DeliveryAt_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_DeliveryAt.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'GODOWN' or Ledger_Type = 'SIZING' or Ledger_Type = 'WEAVER')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_DeliveryAt_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_DeliveryAt.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_DeliveryAt, txt_FilterBillNo, btn_Filter_Show, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'GODOWN' or Ledger_Type = 'SIZING' or Ledger_Type = 'WEAVER')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_DeliveryAt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_DeliveryAt.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_DeliveryAt, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'GODOWN' or Ledger_Type = 'SIZING' or Ledger_Type = 'WEAVER')", "(Ledger_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            btn_Filter_Show_Click(sender, e)
        End If
    End Sub

    Private Sub txt_CommRate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_CommRate.KeyDown
        If e.KeyValue = 38 Then
            If cbo_Agent.Enabled = True Then
                cbo_Agent.Focus()
            ElseIf txt_RecNo.Enabled = True Then
                txt_RecNo.Focus()
            ElseIf cbo_Delvat.Enabled = True Then
                cbo_Delvat.Focus()
            Else
                txt_BillNo.Focus()

            End If
        End If

        If e.KeyValue = 40 Then
            If Trim(UCase(cbo_Type.Text)) = "RECEIPT" Then
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



    Private Sub txt_Commbag_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_CommRate.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            If Trim(UCase(cbo_Type.Text)) = "RECEIPT" Then
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

    Private Sub cbo_CommType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_CommType.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_CommType, txt_CommRate, Nothing, "", "", "", "")
        If e.KeyValue = 40 Then
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
            dgv_Details.CurrentCell.Selected = True
        End If

    End Sub

    Private Sub cbo_CommType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_CommType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_CommType, Nothing, "", "", "", "")
        If Asc(e.KeyChar) = 13 Then
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
            dgv_Details.CurrentCell.Selected = True
        End If
    End Sub

    Private Sub txt_CommRate_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_CommRate.TextChanged
        Agent_Commission_Calculation()
    End Sub

    Private Sub cbo_Grid_RateFor_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_RateFor.TextChanged
        Try
            If cbo_Grid_RateFor.Visible Then
                With dgv_Details
                    If Val(cbo_Grid_RateFor.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 6 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_RateFor.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

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

    Private Sub btn_Print_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        print_record()
    End Sub

    Private Sub btn_save_Click1(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_close_Click1(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub dgtxt_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyUp
        Try
            With dgv_Details
                If .Visible = True Then
                    If .Rows.Count > 0 Then
                        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
                            dgv_Details_KeyUp(sender, e)
                        End If
                    End If
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXT KEYUP....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_Filter_DeliveryAt_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_DeliveryAt.KeyUp
        If e.Control = False And e.KeyValue = 17 Then

            Common_Procedures.MDI_LedType = "SIZING"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Delvat.Name
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
                    If Val(cbo_Colour.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 9 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Colour.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
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

    Private Sub msk_Date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles msk_Date.KeyPress
        If Trim(UCase(e.KeyChar)) = "D" Then
            msk_date.Text = Date.Today
            msk_date.SelectionStart = 0
        End If
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            cbo_PartyName.Focus()
        End If
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



    Private Sub dtp_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Date.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            dtp_Date.Text = Date.Today
        End If
    End Sub
    Private Sub dtp_BillDate_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_BillDate.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            dtp_BillDate.Text = Date.Today
        End If
    End Sub
    Private Sub dtp_BillDate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_BillDate.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
    End Sub

    Private Sub msk_BillDate_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles msk_BillDate.KeyPress
        If Trim(UCase(e.KeyChar)) = "D" Then
            msk_Date.Text = Date.Today
            msk_BillDate.SelectionStart = 0
        End If
    End Sub

    Private Sub msk_BillDate_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_BillDate.KeyUp
        'If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
        '    msk_BillDate.Text = Date.Today
        '    msk_BillDate.SelectionStart = 0
        'End If
        If IsDate(msk_BillDate.Text) = True Then
            If e.KeyCode = 107 Then
                msk_BillDate.Text = DateAdd("D", 1, Convert.ToDateTime(msk_BillDate.Text))
                msk_BillDate.SelectionStart = 0
            ElseIf e.KeyCode = 109 Then
                msk_BillDate.Text = DateAdd("D", -1, Convert.ToDateTime(msk_BillDate.Text))
                msk_BillDate.SelectionStart = 0
            End If
        End If
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            Common_Procedures.maskEdit_Date_ON_DelBackSpace(sender, e, vmskBillOldText, vmskBillSelStrt)
        End If
    End Sub

    Private Sub txt_AssessableValue_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_AssessableValue.KeyDown
        If e.KeyValue = 38 Then
            txt_AddLess_BeforeTax.Focus()
        End If
        If e.KeyValue = 40 Then
            cbo_VatAc.Focus()
        End If
    End Sub

    Private Sub txt_AssessableValue_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_AssessableValue.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            cbo_VatAc.Focus()
        End If
    End Sub

    Private Sub txt_AssessableValue_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_AssessableValue.TextChanged
        NetAmount_Calculation()
    End Sub
    Private Sub dtp_Date_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtp_Date.ValueChanged
        msk_Date.Text = dtp_Date.Text
    End Sub

    Private Sub dtp_Date_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_Date.Enter
        msk_Date.Focus()
        msk_Date.SelectionStart = 0
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

    Private Sub msk_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_Date.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        If e.KeyCode = 40 Then
            e.Handled = True : e.SuppressKeyPress = True
            cbo_PartyName.Focus()
        End If

        If e.KeyCode = 38 Then
            e.Handled = True : e.SuppressKeyPress = True
            txt_Note.Focus()
        End If

        vmskOldText = ""
        vmskSelStrt = -1
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            vmskOldText = msk_Date.Text
            vmskSelStrt = msk_Date.SelectionStart
        End If
    End Sub

    Private Sub dtp_BillDate_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtp_BillDate.ValueChanged
        msk_BillDate.Text = dtp_BillDate.Text
    End Sub

    Private Sub dtp_BillDate_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_BillDate.Enter
        msk_BillDate.Focus()
        msk_BillDate.SelectionStart = 0
    End Sub

    Private Sub msk_BillDate_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles msk_BillDate.LostFocus
        If IsDate(msk_BillDate.Text) = True Then
            If Microsoft.VisualBasic.DateAndTime.Day(Convert.ToDateTime(msk_BillDate.Text)) <= 31 Or Microsoft.VisualBasic.DateAndTime.Month(Convert.ToDateTime(msk_BillDate.Text)) <= 31 Then
                If Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_BillDate.Text)) <= 2050 And Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_BillDate.Text)) >= 2000 Then
                    dtp_BillDate.Value = Convert.ToDateTime(msk_BillDate.Text)
                End If
            End If
        End If
    End Sub

    Private Sub msk_BillDate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_BillDate.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        vmskBillOldText = ""
        vmskBillSelStrt = -1
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            vmskBillOldText = msk_BillDate.Text
            vmskBillSelStrt = msk_BillDate.SelectionStart
        End If
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        MessageBox.Show("Tested OK")
    End Sub

    Private Sub cbo_Agent_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Agent.LostFocus
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim ag_Perc As Single = 0
        Dim Ag_BagRate As Single = 0
        Dim Ag_idno As Integer = 0

        Ag_idno = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Agent.Text)

        da = New SqlClient.SqlDataAdapter("select a.* from ledger_head a where a.ledger_idno = " & Str(Val(Ag_idno)) & "  and a.Ledger_Type='AGENT'", con)
        dt = New DataTable
        da.Fill(dt)

        ag_Perc = 0
        Ag_BagRate = 0

        If dt.Rows.Count > 0 Then
            If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                ag_Perc = Val(dt.Rows(0).Item("Yarn_Comm_Percentage").ToString)
                Ag_BagRate = Val(dt.Rows(0).Item("Yarn_Comm_Bag").ToString)
            End If
        End If
        dt.Dispose()
        da.Dispose()

        If Trim(UCase(cbo_CommType.Text)) = "BAG" Then

            txt_CommRate.Text = Val(Ag_BagRate)

        Else

            txt_CommRate.Text = Val(ag_Perc)

        End If
    End Sub

    Private Sub btn_YarnTest_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_YarnTest.Click
        pnl_YarnTest.Visible = True
        pnl_YarnTest.Enabled = True
        pnl_Back.Enabled = False
        If txt_Test1_Cone1_YarnWeight1.Enabled And txt_Test1_Cone1_YarnWeight1.Visible Then txt_Test1_Cone1_YarnWeight1.Focus()
    End Sub

    Private Sub btn_YarntestClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_YarntestClose.Click
        pnl_Back.Enabled = True
        pnl_YarnTest.Visible = False
    End Sub
    Private Sub txt_Test1_Cone1_YarnWeight1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Test1_Cone1_YarnWeight1.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Test1_Cone1_YarnWeight2_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Test1_Cone1_YarnWeight2.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Test1_Cone1_YarnWeight3_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Test1_Cone1_YarnWeight3.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Test1_Cone1_YarnWeight4_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Test1_Cone1_YarnWeight4.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Test1_Cone1_YarnWeight5_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Test1_Cone1_YarnWeight5.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Test1_Cone2_YarnWeight1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Test1_Cone2_YarnWeight1.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Test1_Cone2_YarnWeight2_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Test1_Cone2_YarnWeight2.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Test1_Cone2_YarnWeight3_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Test1_Cone2_YarnWeight3.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Test1_Cone2_YarnWeight4_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Test1_Cone2_YarnWeight4.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Test1_Cone2_YarnWeight5_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Test1_Cone2_YarnWeight5.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Test1_Cone3_YarnWeight1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Test1_Cone3_YarnWeight1.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Test1_Cone3_YarnWeight2_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Test1_Cone3_YarnWeight2.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Test1_Cone3_YarnWeight3_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Test1_Cone3_YarnWeight3.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Test1_Cone3_YarnWeight4_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Test1_Cone3_YarnWeight4.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Test1_Cone3_YarnWeight5_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Test1_Cone3_YarnWeight5.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Test1_Cone4_YarnWeight1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Test1_Cone4_YarnWeight1.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Test1_Cone4_YarnWeight2_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Test1_Cone4_YarnWeight2.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Test1_Cone4_YarnWeight3_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Test1_Cone4_YarnWeight3.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Test1_Cone4_YarnWeight4_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Test1_Cone4_YarnWeight4.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Test1_Cone4_YarnWeight5_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Test1_Cone4_YarnWeight5.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Test1_Cone5_YarnWeight1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Test1_Cone5_YarnWeight1.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Test1_Cone5_YarnWeight2_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Test1_Cone5_YarnWeight2.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Test1_Cone5_YarnWeight3_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Test1_Cone5_YarnWeight3.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Test1_Cone5_YarnWeight4_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Test1_Cone5_YarnWeight4.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Test1_Cone5_YarnWeight5_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Test1_Cone5_YarnWeight5.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Test1_Cone6_YarnWeight1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Test1_Cone6_YarnWeight1.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Test1_Cone6_YarnWeight2_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Test1_Cone6_YarnWeight2.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Test1_Cone6_YarnWeight3_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Test1_Cone6_YarnWeight3.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Test1_Cone6_YarnWeight4_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Test1_Cone6_YarnWeight4.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub


    Private Sub txt_Test2_Cone1_YarnWeight1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Test2_Cone1_YarnWeight1.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Test2_Cone1_YarnWeight2_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Test2_Cone1_YarnWeight2.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Test2_Cone1_YarnWeight3_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Test2_Cone1_YarnWeight3.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Test2_Cone1_YarnWeight4_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Test2_Cone1_YarnWeight4.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Test2_Cone1_YarnWeight5_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Test2_Cone1_YarnWeight5.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Test2_Cone2_YarnWeight1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Test2_Cone2_YarnWeight1.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Test2_Cone2_YarnWeight2_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Test2_Cone2_YarnWeight2.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Test2_Cone2_YarnWeight3_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Test2_Cone2_YarnWeight3.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Test2_Cone2_YarnWeight4_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Test2_Cone2_YarnWeight4.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Test2_Cone2_YarnWeight5_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Test2_Cone2_YarnWeight5.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Test2_Cone3_YarnWeight1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Test2_Cone3_YarnWeight1.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Test2_Cone3_YarnWeight2_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Test2_Cone3_YarnWeight2.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Test2_Cone3_YarnWeight3_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Test2_Cone3_YarnWeight3.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Test2_Cone3_YarnWeight4_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Test2_Cone3_YarnWeight4.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Test2_Cone3_YarnWeight5_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Test2_Cone3_YarnWeight5.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Test2_Cone4_YarnWeight1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Test2_Cone4_YarnWeight1.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Test2_Cone4_YarnWeight2_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Test2_Cone4_YarnWeight2.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Test2_Cone4_YarnWeight3_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Test2_Cone4_YarnWeight3.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Test2_Cone4_YarnWeight4_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Test2_Cone4_YarnWeight4.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Test2_Cone4_YarnWeight5_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Test2_Cone4_YarnWeight5.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Test2_Cone5_YarnWeight1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Test2_Cone5_YarnWeight1.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Test2_Cone5_YarnWeight2_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Test2_Cone5_YarnWeight2.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Test2_Cone5_YarnWeight3_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Test2_Cone5_YarnWeight3.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Test2_Cone5_YarnWeight4_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Test2_Cone5_YarnWeight4.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Test2_Cone5_YarnWeight5_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Test2_Cone5_YarnWeight5.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Test2_Cone6_YarnWeight1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Test2_Cone6_YarnWeight1.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Test2_Cone6_YarnWeight2_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Test2_Cone6_YarnWeight2.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Test2_Cone6_YarnWeight3_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Test2_Cone6_YarnWeight3.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Test2_Cone6_YarnWeight4_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Test2_Cone6_YarnWeight4.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Test3_Cone1_YarnWeight1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Test3_Cone1_YarnWeight1.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Test3_Cone1_YarnWeight2_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Test3_Cone1_YarnWeight2.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Test3_Cone1_YarnWeight3_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Test3_Cone1_YarnWeight3.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Test3_Cone1_YarnWeight4_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Test3_Cone1_YarnWeight4.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Test3_Cone1_YarnWeight5_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Test3_Cone1_YarnWeight5.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Test3_Cone2_YarnWeight1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Test3_Cone2_YarnWeight1.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Test3_Cone2_YarnWeight2_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Test3_Cone2_YarnWeight2.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Test3_Cone2_YarnWeight3_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Test3_Cone2_YarnWeight3.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Test3_Cone2_YarnWeight4_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Test3_Cone2_YarnWeight4.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Test3_Cone2_YarnWeight5_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Test3_Cone2_YarnWeight5.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Test3_Cone3_YarnWeight1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Test3_Cone3_YarnWeight1.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Test3_Cone3_YarnWeight2_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Test3_Cone3_YarnWeight2.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Test3_Cone3_YarnWeight3_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Test3_Cone3_YarnWeight3.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Test3_Cone3_YarnWeight4_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Test3_Cone3_YarnWeight4.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Test3_Cone3_YarnWeight5_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Test3_Cone3_YarnWeight5.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Test3_Cone4_YarnWeight1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Test3_Cone4_YarnWeight1.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Test3_Cone4_YarnWeight2_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Test3_Cone4_YarnWeight2.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Test3_Cone4_YarnWeight3_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Test3_Cone4_YarnWeight3.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Test3_Cone4_YarnWeight4_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Test3_Cone4_YarnWeight4.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Test3_Cone4_YarnWeight5_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Test3_Cone4_YarnWeight5.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Test3_Cone5_YarnWeight1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Test3_Cone5_YarnWeight1.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Test3_Cone5_YarnWeight2_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Test3_Cone5_YarnWeight2.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Test3_Cone5_YarnWeight3_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Test3_Cone5_YarnWeight3.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Test3_Cone5_YarnWeight4_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Test3_Cone5_YarnWeight4.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Test3_Cone5_YarnWeight5_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Test3_Cone5_YarnWeight5.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Test3_Cone6_YarnWeight1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Test3_Cone6_YarnWeight1.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Test3_Cone6_YarnWeight2_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Test3_Cone6_YarnWeight2.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Test3_Cone6_YarnWeight3_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Test3_Cone6_YarnWeight3.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Test3_Cone6_YarnWeight4_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Test3_Cone6_YarnWeight4.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub
    Private Sub txt_Test2No_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Test2No.KeyDown
        If e.KeyCode = 38 Then
            tab_Main.SelectTab(0)
            txt_Test1_Cone6_YarnWeight5.Focus()
        End If
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
    End Sub
    Private Sub txt_test3No_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_test3No.KeyDown
        If e.KeyCode = 38 Then
            tab_Main.SelectTab(1)
            txt_Test2_Cone6_YarnWeight5.Focus()
        End If
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
    End Sub
    Private Sub txt_Test1_Cone6_YarnWeight5_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Test1_Cone6_YarnWeight5.KeyDown
        If e.KeyCode = 40 Then
            tab_Main.SelectTab(1)
            txt_Test2No.Focus()
        End If
        ' SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then
            txt_Test1_Cone6_YarnWeight4.Focus()
        End If
    End Sub

    Private Sub txt_Test1_Cone6_YarnWeight5_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Test1_Cone6_YarnWeight5.KeyPress
        If Asc(e.KeyChar) = 13 Then
            tab_Main.SelectTab(1)
            txt_Test2No.Focus()
        End If
    End Sub

    Private Sub txt_Test2_Cone6_YarnWeight5_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Test2_Cone6_YarnWeight5.KeyDown
        If e.KeyCode = 40 Then
            tab_Main.SelectTab(2)
            txt_test3No.Focus()
        End If
        ' SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then
            txt_Test2_Cone6_YarnWeight4.Focus()
        End If
    End Sub

    Private Sub txt_Test2_Cone6_YarnWeight5_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Test2_Cone6_YarnWeight5.KeyPress
        If Asc(e.KeyChar) = 13 Then
            tab_Main.SelectTab(2)
            txt_test3No.Focus()
        End If
    End Sub

    Private Sub txt_Test1_Cone1_YarnWeight1_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Test1_Cone1_YarnWeight1.TextChanged
        If Val(txt_Test1_Cone1_YarnWeight1.Text) <> 0 Then
            lbl_Test1_Cone1_YarnCount1.Text = Format(64.814 / Val(txt_Test1_Cone1_YarnWeight1.Text), "#######0.0000")
        End If
        Test1_Cone1_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub txt_Test1_Cone1_YarnWeight2_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Test1_Cone1_YarnWeight2.TextChanged
        If Val(txt_Test1_Cone1_YarnWeight2.Text) <> 0 Then
            lbl_Test1_Cone1_YarnCount2.Text = Format(64.814 / Val(txt_Test1_Cone1_YarnWeight2.Text), "#######0.0000")
        End If
        Test1_Cone1_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub txt_Test1_Cone1_YarnWeight3_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Test1_Cone1_YarnWeight3.TextChanged
        If Val(txt_Test1_Cone1_YarnWeight3.Text) <> 0 Then
            lbl_Test1_Cone1_YarnCount3.Text = Format(64.814 / Val(txt_Test1_Cone1_YarnWeight3.Text), "#######0.0000")
        End If
        Test1_Cone1_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub txt_Test1_Cone1_YarnWeight4_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Test1_Cone1_YarnWeight4.TextChanged
        If Val(txt_Test1_Cone1_YarnWeight4.Text) <> 0 Then
            lbl_Test1_Cone1_YarnCount4.Text = Format(64.814 / Val(txt_Test1_Cone1_YarnWeight4.Text), "#######0.0000")
        End If
        Test1_Cone1_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub txt_Test1_Cone1_YarnWeight5_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Test1_Cone1_YarnWeight5.TextChanged
        If Val(txt_Test1_Cone1_YarnWeight5.Text) <> 0 Then
            lbl_Test1_Cone1_YarnCount5.Text = Format(64.814 / Val(txt_Test1_Cone1_YarnWeight5.Text), "#######0.0000")
        End If
        Test1_Cone1_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub Test1_Cone1_Average_Mean_SD_CV_Calculation()
        Dim Mean As Single = 0
        Dim SD As Single = 0
        Dim CV As Single = 0

        Dim x1 As Single = 0
        Dim x2 As Single = 0
        Dim x3 As Single = 0
        Dim x4 As Single = 0
        Dim x5 As Single = 0

        Dim y1 As Single = 0
        Dim y2 As Single = 0
        Dim y3 As Single = 0
        Dim y4 As Single = 0
        Dim y5 As Single = 0

        Dim z1 As Single = 0
        Dim z2 As Single = 0
        Dim z3 As Single = 0
        Dim z4 As Single = 0
        Dim z5 As Single = 0

        Dim a1 As Single = 0
        Dim b1 As Single = 0
        Dim c1 As Single = 0

        Mean = Format((Val(lbl_Test1_Cone1_YarnCount1.Text) + Val(lbl_Test1_Cone1_YarnCount2.Text) + Val(lbl_Test1_Cone1_YarnCount3.Text) + Val(lbl_Test1_Cone1_YarnCount4.Text) + Val(lbl_Test1_Cone1_YarnCount5.Text)) / 5, "#######0.0000")

        x1 = Val(lbl_Test1_Cone1_YarnCount1.Text) - Mean
        x2 = Val(lbl_Test1_Cone1_YarnCount2.Text) - Mean
        x3 = Val(lbl_Test1_Cone1_YarnCount3.Text) - Mean
        x4 = Val(lbl_Test1_Cone1_YarnCount4.Text) - Mean
        x5 = Val(lbl_Test1_Cone1_YarnCount5.Text) - Mean

        y1 = Format(x1 * x1, "#######0.0000")
        y2 = Format(x2 * x2, "#######0.0000")
        y3 = Format(x3 * x3, "#######0.0000")
        y4 = Format(x4 * x4, "#######0.0000")
        y5 = Format(x5 * x5, "#######0.0000")

        a1 = Format((y1 + y2 + y3 + y4 + y5) / 5, "#######0.0000")

        b1 = Format(Math.Sqrt(a1), "#######0.0000")

        SD = Format(b1, "#######0.0000")

        CV = Format(SD / Mean * 100, "#######0.0000")

        lbl_Test1_Cone1_Mean.Text = Mean
        lbl_Test1_Cone1_SD.Text = SD
        lbl_Test1_Cone1_CV.Text = CV

        Final_Test1_Average_Mean_SD_CV_Calculation()

    End Sub

    Private Sub txt_Test1_Cone2_YarnWeight1_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Test1_Cone2_YarnWeight1.TextChanged
        If Val(txt_Test1_Cone2_YarnWeight1.Text) <> 0 Then
            lbl_Test1_Cone2_YarnCount1.Text = Format(64.814 / Val(txt_Test1_Cone2_YarnWeight1.Text), "#######0.0000")
        End If
        Test1_Cone2_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub txt_Test1_Cone2_YarnWeight2_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Test1_Cone2_YarnWeight2.TextChanged
        If Val(txt_Test1_Cone2_YarnWeight2.Text) <> 0 Then
            lbl_Test1_Cone2_YarnCount2.Text = Format(64.814 / Val(txt_Test1_Cone2_YarnWeight2.Text), "#######0.0000")
        End If
        Test1_Cone2_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub txt_Test1_Cone2_YarnWeight3_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Test1_Cone2_YarnWeight3.TextChanged
        If Val(txt_Test1_Cone2_YarnWeight3.Text) <> 0 Then
            lbl_Test1_Cone2_YarnCount3.Text = Format(64.814 / Val(txt_Test1_Cone2_YarnWeight3.Text), "#######0.0000")
        End If
        Test1_Cone2_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub txt_Test1_Cone2_YarnWeight4_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Test1_Cone2_YarnWeight4.TextChanged
        If Val(txt_Test1_Cone2_YarnWeight4.Text) <> 0 Then
            lbl_Test1_Cone2_YarnCount4.Text = Format(64.814 / Val(txt_Test1_Cone2_YarnWeight4.Text), "#######0.0000")
        End If
        Test1_Cone2_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub txt_Test1_Cone2_YarnWeight5_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Test1_Cone2_YarnWeight5.TextChanged
        If Val(txt_Test1_Cone2_YarnWeight5.Text) <> 0 Then
            lbl_Test1_Cone2_YarnCount5.Text = Format(64.814 / Val(txt_Test1_Cone2_YarnWeight5.Text), "#######0.0000")
        End If
        Test1_Cone2_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub Test1_Cone2_Average_Mean_SD_CV_Calculation()
        Dim Mean As Single = 0
        Dim SD As Single = 0
        Dim CV As Single = 0

        Dim x1 As Single = 0
        Dim x2 As Single = 0
        Dim x3 As Single = 0
        Dim x4 As Single = 0
        Dim x5 As Single = 0

        Dim y1 As Single = 0
        Dim y2 As Single = 0
        Dim y3 As Single = 0
        Dim y4 As Single = 0
        Dim y5 As Single = 0

        Dim z1 As Single = 0
        Dim z2 As Single = 0
        Dim z3 As Single = 0
        Dim z4 As Single = 0
        Dim z5 As Single = 0

        Dim a1 As Single = 0
        Dim b1 As Single = 0
        Dim c1 As Single = 0

        Mean = Format((Val(lbl_Test1_Cone2_YarnCount1.Text) + Val(lbl_Test1_Cone2_YarnCount2.Text) + Val(lbl_Test1_Cone2_YarnCount3.Text) + Val(lbl_Test1_Cone2_YarnCount4.Text) + Val(lbl_Test1_Cone2_YarnCount5.Text)) / 5, "#######0.0000")

        x1 = Val(lbl_Test1_Cone2_YarnCount1.Text) - Mean
        x2 = Val(lbl_Test1_Cone2_YarnCount2.Text) - Mean
        x3 = Val(lbl_Test1_Cone2_YarnCount3.Text) - Mean
        x4 = Val(lbl_Test1_Cone2_YarnCount4.Text) - Mean
        x5 = Val(lbl_Test1_Cone2_YarnCount5.Text) - Mean

        y1 = Format(x1 * x1, "#######0.0000")
        y2 = Format(x2 * x2, "#######0.0000")
        y3 = Format(x3 * x3, "#######0.0000")
        y4 = Format(x4 * x4, "#######0.0000")
        y5 = Format(x5 * x5, "#######0.0000")

        a1 = Format((y1 + y2 + y3 + y4 + y5) / 5, "#######0.0000")

        b1 = Format(Math.Sqrt(a1), "#######0.0000")

        SD = Format(b1, "#######0.0000")

        CV = Format(SD / Mean * 100, "#######0.0000")

        lbl_Test1_Cone2_mean.Text = Mean
        lbl_Test1_Cone2_SD.Text = SD
        lbl_Test1_Cone2_Cv.Text = CV
        Final_Test1_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub txt_Test1_Cone3_YarnWeight1_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Test1_Cone3_YarnWeight1.TextChanged
        If Val(txt_Test1_Cone3_YarnWeight1.Text) <> 0 Then
            lbl_Test1_Cone3_YarnCount1.Text = Format(64.814 / Val(txt_Test1_Cone3_YarnWeight1.Text), "#######0.0000")
        End If
        Test1_Cone3_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub txt_Test1_Cone3_YarnWeight2_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Test1_Cone3_YarnWeight2.TextChanged
        If Val(txt_Test1_Cone3_YarnWeight2.Text) <> 0 Then
            lbl_Test1_Cone3_YarnCount2.Text = Format(64.814 / Val(txt_Test1_Cone3_YarnWeight2.Text), "#######0.0000")
        End If
        Test1_Cone3_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub txt_Test1_Cone3_YarnWeight3_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Test1_Cone3_YarnWeight3.TextChanged
        If Val(txt_Test1_Cone3_YarnWeight3.Text) <> 0 Then
            lbl_Test1_Cone3_YarnCount3.Text = Format(64.814 / Val(txt_Test1_Cone3_YarnWeight3.Text), "#######0.0000")
        End If
        Test1_Cone3_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub txt_Test1_Cone3_YarnWeight4_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Test1_Cone3_YarnWeight4.TextChanged
        If Val(txt_Test1_Cone3_YarnWeight4.Text) <> 0 Then
            lbl_Test1_Cone3_YarnCount4.Text = Format(64.814 / Val(txt_Test1_Cone3_YarnWeight4.Text), "#######0.0000")
        End If
        Test1_Cone3_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub txt_Test1_Cone3_YarnWeight5_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Test1_Cone3_YarnWeight5.TextChanged
        If Val(txt_Test1_Cone3_YarnWeight5.Text) <> 0 Then
            lbl_Test1_Cone3_YarnCount5.Text = Format(64.814 / Val(txt_Test1_Cone3_YarnWeight5.Text), "#######0.0000")
        End If
        Test1_Cone3_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub Test1_Cone3_Average_Mean_SD_CV_Calculation()
        Dim Mean As Single = 0
        Dim SD As Single = 0
        Dim CV As Single = 0

        Dim x1 As Single = 0
        Dim x2 As Single = 0
        Dim x3 As Single = 0
        Dim x4 As Single = 0
        Dim x5 As Single = 0

        Dim y1 As Single = 0
        Dim y2 As Single = 0
        Dim y3 As Single = 0
        Dim y4 As Single = 0
        Dim y5 As Single = 0

        Dim z1 As Single = 0
        Dim z2 As Single = 0
        Dim z3 As Single = 0
        Dim z4 As Single = 0
        Dim z5 As Single = 0

        Dim a1 As Single = 0
        Dim b1 As Single = 0
        Dim c1 As Single = 0

        Mean = Format((Val(lbl_Test1_Cone3_YarnCount1.Text) + Val(lbl_Test1_Cone3_YarnCount2.Text) + Val(lbl_Test1_Cone3_YarnCount3.Text) + Val(lbl_Test1_Cone3_YarnCount4.Text) + Val(lbl_Test1_Cone3_YarnCount5.Text)) / 5, "#######0.0000")

        x1 = Val(lbl_Test1_Cone3_YarnCount1.Text) - Mean
        x2 = Val(lbl_Test1_Cone3_YarnCount2.Text) - Mean
        x3 = Val(lbl_Test1_Cone3_YarnCount3.Text) - Mean
        x4 = Val(lbl_Test1_Cone3_YarnCount4.Text) - Mean
        x5 = Val(lbl_Test1_Cone3_YarnCount5.Text) - Mean

        y1 = Format(x1 * x1, "#######0.0000")
        y2 = Format(x2 * x2, "#######0.0000")
        y3 = Format(x3 * x3, "#######0.0000")
        y4 = Format(x4 * x4, "#######0.0000")
        y5 = Format(x5 * x5, "#######0.0000")

        a1 = Format((y1 + y2 + y3 + y4 + y5) / 5, "#######0.0000")

        b1 = Format(Math.Sqrt(a1), "#######0.0000")

        SD = Format(b1, "#######0.0000")

        CV = Format(SD / Mean * 100, "#######0.0000")

        lbl_Test1_Cone3_mean.Text = Mean
        lbl_Test1_Cone3_SD.Text = SD
        lbl_Test1_Cone3_Cv.Text = CV
        Final_Test1_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub txt_Test1_Cone4_YarnWeight1_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Test1_Cone4_YarnWeight1.TextChanged
        If Val(txt_Test1_Cone4_YarnWeight1.Text) <> 0 Then
            lbl_Test1_Cone4_YarnCount1.Text = Format(64.814 / Val(txt_Test1_Cone4_YarnWeight1.Text), "#######0.0000")
        End If
        Test1_Cone4_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub txt_Test1_Cone4_YarnWeight2_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Test1_Cone4_YarnWeight2.TextChanged
        If Val(txt_Test1_Cone4_YarnWeight2.Text) <> 0 Then
            lbl_Test1_Cone4_YarnCount2.Text = Format(64.814 / Val(txt_Test1_Cone4_YarnWeight2.Text), "#######0.0000")
        End If
        Test1_Cone4_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub txt_Test1_Cone4_YarnWeight3_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Test1_Cone4_YarnWeight3.TextChanged
        If Val(txt_Test1_Cone4_YarnWeight3.Text) <> 0 Then
            lbl_Test1_Cone4_YarnCount3.Text = Format(64.814 / Val(txt_Test1_Cone4_YarnWeight3.Text), "#######0.0000")
        End If
        Test1_Cone4_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub txt_Test1_Cone4_YarnWeight4_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Test1_Cone4_YarnWeight4.TextChanged
        If Val(txt_Test1_Cone4_YarnWeight4.Text) <> 0 Then
            lbl_Test1_Cone4_YarnCount4.Text = Format(64.814 / Val(txt_Test1_Cone4_YarnWeight4.Text), "#######0.0000")
        End If
        Test1_Cone4_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub txt_Test1_Cone4_YarnWeight5_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Test1_Cone4_YarnWeight5.TextChanged
        If Val(txt_Test1_Cone4_YarnWeight5.Text) <> 0 Then
            lbl_Test1_Cone4_YarnCount5.Text = Format(64.814 / Val(txt_Test1_Cone4_YarnWeight5.Text), "#######0.0000")
        End If
        Test1_Cone4_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub Test1_Cone4_Average_Mean_SD_CV_Calculation()
        Dim Mean As Single = 0
        Dim SD As Single = 0
        Dim CV As Single = 0

        Dim x1 As Single = 0
        Dim x2 As Single = 0
        Dim x3 As Single = 0
        Dim x4 As Single = 0
        Dim x5 As Single = 0

        Dim y1 As Single = 0
        Dim y2 As Single = 0
        Dim y3 As Single = 0
        Dim y4 As Single = 0
        Dim y5 As Single = 0

        Dim z1 As Single = 0
        Dim z2 As Single = 0
        Dim z3 As Single = 0
        Dim z4 As Single = 0
        Dim z5 As Single = 0

        Dim a1 As Single = 0
        Dim b1 As Single = 0
        Dim c1 As Single = 0

        Mean = Format((Val(lbl_Test1_Cone4_YarnCount1.Text) + Val(lbl_Test1_Cone4_YarnCount2.Text) + Val(lbl_Test1_Cone4_YarnCount3.Text) + Val(lbl_Test1_Cone4_YarnCount4.Text) + Val(lbl_Test1_Cone4_YarnCount5.Text)) / 5, "#######0.0000")

        x1 = Val(lbl_Test1_Cone4_YarnCount1.Text) - Mean
        x2 = Val(lbl_Test1_Cone4_YarnCount2.Text) - Mean
        x3 = Val(lbl_Test1_Cone4_YarnCount3.Text) - Mean
        x4 = Val(lbl_Test1_Cone4_YarnCount4.Text) - Mean
        x5 = Val(lbl_Test1_Cone4_YarnCount5.Text) - Mean

        y1 = Format(x1 * x1, "#######0.0000")
        y2 = Format(x2 * x2, "#######0.0000")
        y3 = Format(x3 * x3, "#######0.0000")
        y4 = Format(x4 * x4, "#######0.0000")
        y5 = Format(x5 * x5, "#######0.0000")

        a1 = Format((y1 + y2 + y3 + y4 + y5) / 5, "#######0.0000")

        b1 = Format(Math.Sqrt(a1), "#######0.0000")

        SD = Format(b1, "#######0.0000")

        CV = Format(SD / Mean * 100, "#######0.0000")

        lbl_Test1_Cone4_mean.Text = Mean
        lbl_Test1_Cone4_SD.Text = SD
        lbl_Test1_Cone4_Cv.Text = CV
        Final_Test1_Average_Mean_SD_CV_Calculation()
    End Sub
    Private Sub txt_Test1_Cone5_YarnWeight1_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Test1_Cone5_YarnWeight1.TextChanged
        If Val(txt_Test1_Cone5_YarnWeight1.Text) <> 0 Then
            lbl_Test1_Cone5_YarnCount1.Text = Format(64.814 / Val(txt_Test1_Cone5_YarnWeight1.Text), "#######0.0000")
        End If
        Test1_Cone5_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub txt_Test1_Cone5_YarnWeight2_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Test1_Cone5_YarnWeight2.TextChanged
        If Val(txt_Test1_Cone5_YarnWeight2.Text) <> 0 Then
            lbl_Test1_Cone5_YarnCount2.Text = Format(64.814 / Val(txt_Test1_Cone5_YarnWeight2.Text), "#######0.0000")
        End If
        Test1_Cone5_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub txt_Test1_Cone5_YarnWeight3_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Test1_Cone5_YarnWeight3.TextChanged
        If Val(txt_Test1_Cone5_YarnWeight3.Text) <> 0 Then
            lbl_Test1_Cone5_YarnCount3.Text = Format(64.814 / Val(txt_Test1_Cone5_YarnWeight3.Text), "#######0.0000")
        End If
        Test1_Cone5_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub txt_Test1_Cone5_YarnWeight4_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Test1_Cone5_YarnWeight4.TextChanged
        If Val(txt_Test1_Cone5_YarnWeight4.Text) <> 0 Then
            lbl_Test1_Cone5_YarnCount4.Text = Format(64.814 / Val(txt_Test1_Cone5_YarnWeight4.Text), "#######0.0000")
        End If
        Test1_Cone5_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub txt_Test1_Cone5_YarnWeight5_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Test1_Cone5_YarnWeight5.TextChanged
        If Val(txt_Test1_Cone5_YarnWeight5.Text) <> 0 Then
            lbl_Test1_Cone5_YarnCount5.Text = Format(64.814 / Val(txt_Test1_Cone5_YarnWeight5.Text), "#######0.0000")
        End If
        Test1_Cone5_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub Test1_Cone5_Average_Mean_SD_CV_Calculation()
        Dim Mean As Single = 0
        Dim SD As Single = 0
        Dim CV As Single = 0

        Dim x1 As Single = 0
        Dim x2 As Single = 0
        Dim x3 As Single = 0
        Dim x4 As Single = 0
        Dim x5 As Single = 0

        Dim y1 As Single = 0
        Dim y2 As Single = 0
        Dim y3 As Single = 0
        Dim y4 As Single = 0
        Dim y5 As Single = 0

        Dim z1 As Single = 0
        Dim z2 As Single = 0
        Dim z3 As Single = 0
        Dim z4 As Single = 0
        Dim z5 As Single = 0

        Dim a1 As Single = 0
        Dim b1 As Single = 0
        Dim c1 As Single = 0

        Mean = Format((Val(lbl_Test1_Cone5_YarnCount1.Text) + Val(lbl_Test1_Cone5_YarnCount2.Text) + Val(lbl_Test1_Cone5_YarnCount3.Text) + Val(lbl_Test1_Cone5_YarnCount4.Text) + Val(lbl_Test1_Cone5_YarnCount5.Text)) / 5, "#######0.0000")

        x1 = Val(lbl_Test1_Cone5_YarnCount1.Text) - Mean
        x2 = Val(lbl_Test1_Cone5_YarnCount2.Text) - Mean
        x3 = Val(lbl_Test1_Cone5_YarnCount3.Text) - Mean
        x4 = Val(lbl_Test1_Cone5_YarnCount4.Text) - Mean
        x5 = Val(lbl_Test1_Cone5_YarnCount5.Text) - Mean

        y1 = Format(x1 * x1, "#######0.0000")
        y2 = Format(x2 * x2, "#######0.0000")
        y3 = Format(x3 * x3, "#######0.0000")
        y4 = Format(x4 * x4, "#######0.0000")
        y5 = Format(x5 * x5, "#######0.0000")

        a1 = Format((y1 + y2 + y3 + y4 + y5) / 5, "#######0.0000")

        b1 = Format(Math.Sqrt(a1), "#######0.0000")

        SD = Format(b1, "#######0.0000")

        CV = Format(SD / Mean * 100, "#######0.0000")

        lbl_Test1_Cone5_mean.Text = Mean
        lbl_Test1_Cone5_SD.Text = SD
        lbl_Test1_Cone5_Cv.Text = CV
        Final_Test1_Average_Mean_SD_CV_Calculation()
    End Sub
    Private Sub txt_Test1_Cone6_YarnWeight1_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Test1_Cone6_YarnWeight1.TextChanged
        If Val(txt_Test1_Cone6_YarnWeight1.Text) <> 0 Then
            lbl_Test1_Cone6_YarnCount1.Text = Format(64.814 / Val(txt_Test1_Cone6_YarnWeight1.Text), "#######0.0000")
        End If
        Test1_Cone6_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub txt_Test1_Cone6_YarnWeight2_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Test1_Cone6_YarnWeight2.TextChanged
        If Val(txt_Test1_Cone6_YarnWeight2.Text) <> 0 Then
            lbl_Test1_Cone6_YarnCount2.Text = Format(64.814 / Val(txt_Test1_Cone6_YarnWeight2.Text), "#######0.0000")
        End If
        Test1_Cone6_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub txt_Test1_Cone6_YarnWeight3_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Test1_Cone6_YarnWeight3.TextChanged
        If Val(txt_Test1_Cone6_YarnWeight3.Text) <> 0 Then
            lbl_Test1_Cone6_YarnCount3.Text = Format(64.814 / Val(txt_Test1_Cone6_YarnWeight3.Text), "#######0.0000")
        End If
        Test1_Cone6_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub txt_Test1_Cone6_YarnWeight4_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Test1_Cone6_YarnWeight4.TextChanged
        If Val(txt_Test1_Cone6_YarnWeight4.Text) <> 0 Then
            lbl_Test1_Cone6_YarnCount4.Text = Format(64.814 / Val(txt_Test1_Cone6_YarnWeight4.Text), "#######0.0000")
        End If
        Test1_Cone6_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub txt_Test1_Cone6_YarnWeight5_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Test1_Cone6_YarnWeight5.TextChanged
        If Val(txt_Test1_Cone6_YarnWeight5.Text) <> 0 Then
            lbl_Test1_Cone6_YarnCount5.Text = Format(64.814 / Val(txt_Test1_Cone6_YarnWeight5.Text), "#######0.0000")
        End If
        Test1_Cone6_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub Test1_Cone6_Average_Mean_SD_CV_Calculation()
        Dim Mean As Single = 0
        Dim SD As Single = 0
        Dim CV As Single = 0

        Dim x1 As Single = 0
        Dim x2 As Single = 0
        Dim x3 As Single = 0
        Dim x4 As Single = 0
        Dim x5 As Single = 0

        Dim y1 As Single = 0
        Dim y2 As Single = 0
        Dim y3 As Single = 0
        Dim y4 As Single = 0
        Dim y5 As Single = 0

        Dim z1 As Single = 0
        Dim z2 As Single = 0
        Dim z3 As Single = 0
        Dim z4 As Single = 0
        Dim z5 As Single = 0

        Dim a1 As Single = 0
        Dim b1 As Single = 0
        Dim c1 As Single = 0

        Mean = Format((Val(lbl_Test1_Cone6_YarnCount1.Text) + Val(lbl_Test1_Cone6_YarnCount2.Text) + Val(lbl_Test1_Cone6_YarnCount3.Text) + Val(lbl_Test1_Cone6_YarnCount4.Text) + Val(lbl_Test1_Cone6_YarnCount5.Text)) / 5, "#######0.0000")

        x1 = Val(lbl_Test1_Cone6_YarnCount1.Text) - Mean
        x2 = Val(lbl_Test1_Cone6_YarnCount2.Text) - Mean
        x3 = Val(lbl_Test1_Cone6_YarnCount3.Text) - Mean
        x4 = Val(lbl_Test1_Cone6_YarnCount4.Text) - Mean
        x5 = Val(lbl_Test1_Cone6_YarnCount5.Text) - Mean

        y1 = Format(x1 * x1, "#######0.0000")
        y2 = Format(x2 * x2, "#######0.0000")
        y3 = Format(x3 * x3, "#######0.0000")
        y4 = Format(x4 * x4, "#######0.0000")
        y5 = Format(x5 * x5, "#######0.0000")

        a1 = Format((y1 + y2 + y3 + y4 + y5) / 5, "#######0.0000")

        b1 = Format(Math.Sqrt(a1), "#######0.0000")

        SD = Format(b1, "#######0.0000")

        CV = Format(SD / Mean * 100, "#######0.0000")

        lbl_Test1_Cone6_mean.Text = Mean
        lbl_Test1_Cone6_Sd.Text = SD
        lbl_Test1_Cone6_Cv.Text = CV
        Final_Test1_Average_Mean_SD_CV_Calculation()
    End Sub





    Private Sub txt_test2_Cone1_YarnWeight1_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Test2_Cone1_YarnWeight1.TextChanged
        If Val(txt_Test2_Cone1_YarnWeight1.Text) <> 0 Then
            lbl_Test2_Cone1_YarnCount1.Text = Format(64.814 / Val(txt_Test2_Cone1_YarnWeight1.Text), "#######0.0000")
        End If
        test2_Cone1_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub txt_test2_Cone1_YarnWeight2_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Test2_Cone1_YarnWeight2.TextChanged
        If Val(txt_Test2_Cone1_YarnWeight2.Text) <> 0 Then
            lbl_Test2_Cone1_YarnCount2.Text = Format(64.814 / Val(txt_Test2_Cone1_YarnWeight2.Text), "#######0.0000")
        End If
        test2_Cone1_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub txt_test2_Cone1_YarnWeight3_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Test2_Cone1_YarnWeight3.TextChanged
        If Val(txt_Test2_Cone1_YarnWeight3.Text) <> 0 Then
            lbl_Test2_Cone1_YarnCount3.Text = Format(64.814 / Val(txt_Test2_Cone1_YarnWeight3.Text), "#######0.0000")
        End If
        test2_Cone1_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub txt_test2_Cone1_YarnWeight4_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Test2_Cone1_YarnWeight4.TextChanged
        If Val(txt_Test2_Cone1_YarnWeight4.Text) <> 0 Then
            lbl_Test2_Cone1_YarnCount4.Text = Format(64.814 / Val(txt_Test2_Cone1_YarnWeight4.Text), "#######0.0000")
        End If
        test2_Cone1_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub txt_test2_Cone1_YarnWeight5_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Test2_Cone1_YarnWeight5.TextChanged
        If Val(txt_Test2_Cone1_YarnWeight5.Text) <> 0 Then
            lbl_Test2_Cone1_YarnCount5.Text = Format(64.814 / Val(txt_Test2_Cone1_YarnWeight5.Text), "#######0.0000")
        End If
        test2_Cone1_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub test2_Cone1_Average_Mean_SD_CV_Calculation()
        Dim Mean As Single = 0
        Dim SD As Single = 0
        Dim CV As Single = 0

        Dim x1 As Single = 0
        Dim x2 As Single = 0
        Dim x3 As Single = 0
        Dim x4 As Single = 0
        Dim x5 As Single = 0

        Dim y1 As Single = 0
        Dim y2 As Single = 0
        Dim y3 As Single = 0
        Dim y4 As Single = 0
        Dim y5 As Single = 0

        Dim z1 As Single = 0
        Dim z2 As Single = 0
        Dim z3 As Single = 0
        Dim z4 As Single = 0
        Dim z5 As Single = 0

        Dim a1 As Single = 0
        Dim b1 As Single = 0
        Dim c1 As Single = 0

        Mean = Format((Val(lbl_Test2_Cone1_YarnCount1.Text) + Val(lbl_Test2_Cone1_YarnCount2.Text) + Val(lbl_Test2_Cone1_YarnCount3.Text) + Val(lbl_Test2_Cone1_YarnCount4.Text) + Val(lbl_Test2_Cone1_YarnCount5.Text)) / 5, "#######0.0000")

        x1 = Val(lbl_Test2_Cone1_YarnCount1.Text) - Mean
        x2 = Val(lbl_Test2_Cone1_YarnCount2.Text) - Mean
        x3 = Val(lbl_Test2_Cone1_YarnCount3.Text) - Mean
        x4 = Val(lbl_Test2_Cone1_YarnCount4.Text) - Mean
        x5 = Val(lbl_Test2_Cone1_YarnCount5.Text) - Mean

        y1 = Format(x1 * x1, "#######0.0000")
        y2 = Format(x2 * x2, "#######0.0000")
        y3 = Format(x3 * x3, "#######0.0000")
        y4 = Format(x4 * x4, "#######0.0000")
        y5 = Format(x5 * x5, "#######0.0000")

        a1 = Format((y1 + y2 + y3 + y4 + y5) / 5, "#######0.0000")

        b1 = Format(Math.Sqrt(a1), "#######0.0000")

        SD = Format(b1, "#######0.0000")

        CV = Format(SD / Mean * 100, "#######0.0000")

        lbl_Test2_Cone1_Mean.Text = Mean
        lbl_Test2_Cone1_SD.Text = SD
        lbl_Test2_Cone1_CV.Text = CV
        Final_Test2_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub txt_test2_Cone2_YarnWeight1_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Test2_Cone2_YarnWeight1.TextChanged
        If Val(txt_Test2_Cone2_YarnWeight1.Text) <> 0 Then
            lbl_Test2_Cone2_YarnCount1.Text = Format(64.814 / Val(txt_Test2_Cone2_YarnWeight1.Text), "#######0.0000")
        End If
        test2_Cone2_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub txt_test2_Cone2_YarnWeight2_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Test2_Cone2_YarnWeight2.TextChanged
        If Val(txt_Test2_Cone2_YarnWeight2.Text) <> 0 Then
            lbl_Test2_Cone2_YarnCount2.Text = Format(64.814 / Val(txt_Test2_Cone2_YarnWeight2.Text), "#######0.0000")
        End If
        test2_Cone2_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub txt_test2_Cone2_YarnWeight3_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Test2_Cone2_YarnWeight3.TextChanged
        If Val(txt_Test2_Cone2_YarnWeight3.Text) <> 0 Then
            lbl_Test2_Cone2_YarnCount3.Text = Format(64.814 / Val(txt_Test2_Cone2_YarnWeight3.Text), "#######0.0000")
        End If
        test2_Cone2_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub txt_test2_Cone2_YarnWeight4_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Test2_Cone2_YarnWeight4.TextChanged
        If Val(txt_Test2_Cone2_YarnWeight4.Text) <> 0 Then
            lbl_Test2_Cone2_YarnCount4.Text = Format(64.814 / Val(txt_Test2_Cone2_YarnWeight4.Text), "#######0.0000")
        End If
        test2_Cone2_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub txt_test2_Cone2_YarnWeight5_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Test2_Cone2_YarnWeight5.TextChanged
        If Val(txt_Test2_Cone2_YarnWeight5.Text) <> 0 Then
            lbl_Test2_Cone2_YarnCount5.Text = Format(64.814 / Val(txt_Test2_Cone2_YarnWeight5.Text), "#######0.0000")
        End If
        test2_Cone2_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub test2_Cone2_Average_Mean_SD_CV_Calculation()
        Dim Mean As Single = 0
        Dim SD As Single = 0
        Dim CV As Single = 0

        Dim x1 As Single = 0
        Dim x2 As Single = 0
        Dim x3 As Single = 0
        Dim x4 As Single = 0
        Dim x5 As Single = 0

        Dim y1 As Single = 0
        Dim y2 As Single = 0
        Dim y3 As Single = 0
        Dim y4 As Single = 0
        Dim y5 As Single = 0

        Dim z1 As Single = 0
        Dim z2 As Single = 0
        Dim z3 As Single = 0
        Dim z4 As Single = 0
        Dim z5 As Single = 0

        Dim a1 As Single = 0
        Dim b1 As Single = 0
        Dim c1 As Single = 0

        Mean = Format((Val(lbl_Test2_Cone2_YarnCount1.Text) + Val(lbl_Test2_Cone2_YarnCount2.Text) + Val(lbl_Test2_Cone2_YarnCount3.Text) + Val(lbl_Test2_Cone2_YarnCount4.Text) + Val(lbl_Test2_Cone2_YarnCount5.Text)) / 5, "#######0.0000")

        x1 = Val(lbl_Test2_Cone2_YarnCount1.Text) - Mean
        x2 = Val(lbl_Test2_Cone2_YarnCount2.Text) - Mean
        x3 = Val(lbl_Test2_Cone2_YarnCount3.Text) - Mean
        x4 = Val(lbl_Test2_Cone2_YarnCount4.Text) - Mean
        x5 = Val(lbl_Test2_Cone2_YarnCount5.Text) - Mean

        y1 = Format(x1 * x1, "#######0.0000")
        y2 = Format(x2 * x2, "#######0.0000")
        y3 = Format(x3 * x3, "#######0.0000")
        y4 = Format(x4 * x4, "#######0.0000")
        y5 = Format(x5 * x5, "#######0.0000")

        a1 = Format((y1 + y2 + y3 + y4 + y5) / 5, "#######0.0000")

        b1 = Format(Math.Sqrt(a1), "#######0.0000")

        SD = Format(b1, "#######0.0000")

        CV = Format(SD / Mean * 100, "#######0.0000")

        lbl_Test2_Cone2_Mean.Text = Mean
        lbl_Test2_Cone2_SD.Text = SD
        lbl_Test2_Cone2_CV.Text = CV
        Final_Test2_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub txt_test2_Cone3_YarnWeight1_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Test2_Cone3_YarnWeight1.TextChanged
        If Val(txt_Test2_Cone3_YarnWeight1.Text) <> 0 Then
            lbl_Test2_Cone3_YarnCount1.Text = Format(64.814 / Val(txt_Test2_Cone3_YarnWeight1.Text), "#######0.0000")
        End If
        test2_Cone3_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub txt_test2_Cone3_YarnWeight2_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Test2_Cone3_YarnWeight2.TextChanged
        If Val(txt_Test2_Cone3_YarnWeight2.Text) <> 0 Then
            lbl_Test2_Cone3_YarnCount2.Text = Format(64.814 / Val(txt_Test2_Cone3_YarnWeight2.Text), "#######0.0000")
        End If
        test2_Cone3_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub txt_test2_Cone3_YarnWeight3_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Test2_Cone3_YarnWeight3.TextChanged
        If Val(txt_Test2_Cone3_YarnWeight3.Text) <> 0 Then
            lbl_Test2_Cone3_YarnCount3.Text = Format(64.814 / Val(txt_Test2_Cone3_YarnWeight3.Text), "#######0.0000")
        End If
        test2_Cone3_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub txt_test2_Cone3_YarnWeight4_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Test2_Cone3_YarnWeight4.TextChanged
        If Val(txt_Test2_Cone3_YarnWeight4.Text) <> 0 Then
            lbl_Test2_Cone3_YarnCount4.Text = Format(64.814 / Val(txt_Test2_Cone3_YarnWeight4.Text), "#######0.0000")
        End If
        test2_Cone3_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub txt_test2_Cone3_YarnWeight5_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Test2_Cone3_YarnWeight5.TextChanged
        If Val(txt_Test2_Cone3_YarnWeight5.Text) <> 0 Then
            lbl_Test2_Cone3_YarnCount5.Text = Format(64.814 / Val(txt_Test2_Cone3_YarnWeight5.Text), "#######0.0000")
        End If
        test2_Cone3_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub test2_Cone3_Average_Mean_SD_CV_Calculation()
        Dim Mean As Single = 0
        Dim SD As Single = 0
        Dim CV As Single = 0

        Dim x1 As Single = 0
        Dim x2 As Single = 0
        Dim x3 As Single = 0
        Dim x4 As Single = 0
        Dim x5 As Single = 0

        Dim y1 As Single = 0
        Dim y2 As Single = 0
        Dim y3 As Single = 0
        Dim y4 As Single = 0
        Dim y5 As Single = 0

        Dim z1 As Single = 0
        Dim z2 As Single = 0
        Dim z3 As Single = 0
        Dim z4 As Single = 0
        Dim z5 As Single = 0

        Dim a1 As Single = 0
        Dim b1 As Single = 0
        Dim c1 As Single = 0

        Mean = Format((Val(lbl_Test2_Cone3_YarnCount1.Text) + Val(lbl_Test2_Cone3_YarnCount2.Text) + Val(lbl_Test2_Cone3_YarnCount3.Text) + Val(lbl_Test2_Cone3_YarnCount4.Text) + Val(lbl_Test2_Cone3_YarnCount5.Text)) / 5, "#######0.0000")

        x1 = Val(lbl_Test2_Cone3_YarnCount1.Text) - Mean
        x2 = Val(lbl_Test2_Cone3_YarnCount2.Text) - Mean
        x3 = Val(lbl_Test2_Cone3_YarnCount3.Text) - Mean
        x4 = Val(lbl_Test2_Cone3_YarnCount4.Text) - Mean
        x5 = Val(lbl_Test2_Cone3_YarnCount5.Text) - Mean

        y1 = Format(x1 * x1, "#######0.0000")
        y2 = Format(x2 * x2, "#######0.0000")
        y3 = Format(x3 * x3, "#######0.0000")
        y4 = Format(x4 * x4, "#######0.0000")
        y5 = Format(x5 * x5, "#######0.0000")

        a1 = Format((y1 + y2 + y3 + y4 + y5) / 5, "#######0.0000")

        b1 = Format(Math.Sqrt(a1), "#######0.0000")

        SD = Format(b1, "#######0.0000")

        CV = Format(SD / Mean * 100, "#######0.0000")

        lbl_Test2_Cone3_Mean.Text = Mean
        lbl_Test2_Cone3_SD.Text = SD
        lbl_Test2_Cone3_CV.Text = CV
        Final_Test2_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub txt_test2_Cone4_YarnWeight1_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Test2_Cone4_YarnWeight1.TextChanged
        If Val(txt_Test2_Cone4_YarnWeight1.Text) <> 0 Then
            lbl_Test2_Cone4_YarnCount1.Text = Format(64.814 / Val(txt_Test2_Cone4_YarnWeight1.Text), "#######0.0000")
        End If
        test2_Cone4_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub txt_test2_Cone4_YarnWeight2_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Test2_Cone4_YarnWeight2.TextChanged
        If Val(txt_Test2_Cone4_YarnWeight2.Text) <> 0 Then
            lbl_Test2_Cone4_YarnCount2.Text = Format(64.814 / Val(txt_Test2_Cone4_YarnWeight2.Text), "#######0.0000")
        End If
        test2_Cone4_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub txt_test2_Cone4_YarnWeight3_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Test2_Cone4_YarnWeight3.TextChanged
        If Val(txt_Test2_Cone4_YarnWeight3.Text) <> 0 Then
            lbl_Test2_Cone4_YarnCount3.Text = Format(64.814 / Val(txt_Test2_Cone4_YarnWeight3.Text), "#######0.0000")
        End If
        test2_Cone4_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub txt_test2_Cone4_YarnWeight4_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Test2_Cone4_YarnWeight4.TextChanged
        If Val(txt_Test2_Cone4_YarnWeight4.Text) <> 0 Then
            lbl_Test2_Cone4_YarnCount4.Text = Format(64.814 / Val(txt_Test2_Cone4_YarnWeight4.Text), "#######0.0000")
        End If
        test2_Cone4_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub txt_test2_Cone4_YarnWeight5_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Test2_Cone4_YarnWeight5.TextChanged
        If Val(txt_Test2_Cone4_YarnWeight5.Text) <> 0 Then
            lbl_Test2_Cone4_YarnCount5.Text = Format(64.814 / Val(txt_Test2_Cone4_YarnWeight5.Text), "#######0.0000")
        End If
        test2_Cone4_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub test2_Cone4_Average_Mean_SD_CV_Calculation()
        Dim Mean As Single = 0
        Dim SD As Single = 0
        Dim CV As Single = 0

        Dim x1 As Single = 0
        Dim x2 As Single = 0
        Dim x3 As Single = 0
        Dim x4 As Single = 0
        Dim x5 As Single = 0

        Dim y1 As Single = 0
        Dim y2 As Single = 0
        Dim y3 As Single = 0
        Dim y4 As Single = 0
        Dim y5 As Single = 0

        Dim z1 As Single = 0
        Dim z2 As Single = 0
        Dim z3 As Single = 0
        Dim z4 As Single = 0
        Dim z5 As Single = 0

        Dim a1 As Single = 0
        Dim b1 As Single = 0
        Dim c1 As Single = 0

        Mean = Format((Val(lbl_Test2_Cone4_YarnCount1.Text) + Val(lbl_Test2_Cone4_YarnCount2.Text) + Val(lbl_Test2_Cone4_YarnCount3.Text) + Val(lbl_Test2_Cone4_YarnCount4.Text) + Val(lbl_Test2_Cone4_YarnCount5.Text)) / 5, "#######0.0000")

        x1 = Val(lbl_Test2_Cone4_YarnCount1.Text) - Mean
        x2 = Val(lbl_Test2_Cone4_YarnCount2.Text) - Mean
        x3 = Val(lbl_Test2_Cone4_YarnCount3.Text) - Mean
        x4 = Val(lbl_Test2_Cone4_YarnCount4.Text) - Mean
        x5 = Val(lbl_Test2_Cone4_YarnCount5.Text) - Mean

        y1 = Format(x1 * x1, "#######0.0000")
        y2 = Format(x2 * x2, "#######0.0000")
        y3 = Format(x3 * x3, "#######0.0000")
        y4 = Format(x4 * x4, "#######0.0000")
        y5 = Format(x5 * x5, "#######0.0000")

        a1 = Format((y1 + y2 + y3 + y4 + y5) / 5, "#######0.0000")

        b1 = Format(Math.Sqrt(a1), "#######0.0000")

        SD = Format(b1, "#######0.0000")

        CV = Format(SD / Mean * 100, "#######0.0000")

        lbl_Test2_Cone4_Mean.Text = Mean
        lbl_Test2_Cone4_SD.Text = SD
        lbl_Test2_Cone4_CV.Text = CV
        Final_Test2_Average_Mean_SD_CV_Calculation()
    End Sub
    Private Sub txt_test2_Cone5_YarnWeight1_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Test2_Cone5_YarnWeight1.TextChanged
        If Val(txt_Test2_Cone5_YarnWeight1.Text) <> 0 Then
            lbl_Test2_Cone5_YarnCount1.Text = Format(64.814 / Val(txt_Test2_Cone5_YarnWeight1.Text), "#######0.0000")
        End If
        test2_Cone5_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub txt_test2_Cone5_YarnWeight2_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Test2_Cone5_YarnWeight2.TextChanged
        If Val(txt_Test2_Cone5_YarnWeight2.Text) <> 0 Then
            lbl_Test2_Cone5_YarnCount2.Text = Format(64.814 / Val(txt_Test2_Cone5_YarnWeight2.Text), "#######0.0000")
        End If
        test2_Cone5_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub txt_test2_Cone5_YarnWeight3_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Test2_Cone5_YarnWeight3.TextChanged
        If Val(txt_Test2_Cone5_YarnWeight3.Text) <> 0 Then
            lbl_Test2_Cone5_YarnCount3.Text = Format(64.814 / Val(txt_Test2_Cone5_YarnWeight3.Text), "#######0.0000")
        End If
        test2_Cone5_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub txt_test2_Cone5_YarnWeight4_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Test2_Cone5_YarnWeight4.TextChanged
        If Val(txt_Test2_Cone5_YarnWeight4.Text) <> 0 Then
            lbl_Test2_Cone5_YarnCount4.Text = Format(64.814 / Val(txt_Test2_Cone5_YarnWeight4.Text), "#######0.0000")
        End If
        test2_Cone5_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub txt_test2_Cone5_YarnWeight5_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Test2_Cone5_YarnWeight5.TextChanged
        If Val(txt_Test2_Cone5_YarnWeight5.Text) <> 0 Then
            lbl_Test2_Cone5_YarnCount5.Text = Format(64.814 / Val(txt_Test2_Cone5_YarnWeight5.Text), "#######0.0000")
        End If
        test2_Cone5_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub test2_Cone5_Average_Mean_SD_CV_Calculation()
        Dim Mean As Single = 0
        Dim SD As Single = 0
        Dim CV As Single = 0

        Dim x1 As Single = 0
        Dim x2 As Single = 0
        Dim x3 As Single = 0
        Dim x4 As Single = 0
        Dim x5 As Single = 0

        Dim y1 As Single = 0
        Dim y2 As Single = 0
        Dim y3 As Single = 0
        Dim y4 As Single = 0
        Dim y5 As Single = 0

        Dim z1 As Single = 0
        Dim z2 As Single = 0
        Dim z3 As Single = 0
        Dim z4 As Single = 0
        Dim z5 As Single = 0

        Dim a1 As Single = 0
        Dim b1 As Single = 0
        Dim c1 As Single = 0

        Mean = Format((Val(lbl_Test2_Cone5_YarnCount1.Text) + Val(lbl_Test2_Cone5_YarnCount2.Text) + Val(lbl_Test2_Cone5_YarnCount3.Text) + Val(lbl_Test2_Cone5_YarnCount4.Text) + Val(lbl_Test2_Cone5_YarnCount5.Text)) / 5, "#######0.0000")

        x1 = Val(lbl_Test2_Cone5_YarnCount1.Text) - Mean
        x2 = Val(lbl_Test2_Cone5_YarnCount2.Text) - Mean
        x3 = Val(lbl_Test2_Cone5_YarnCount3.Text) - Mean
        x4 = Val(lbl_Test2_Cone5_YarnCount4.Text) - Mean
        x5 = Val(lbl_Test2_Cone5_YarnCount5.Text) - Mean

        y1 = Format(x1 * x1, "#######0.0000")
        y2 = Format(x2 * x2, "#######0.0000")
        y3 = Format(x3 * x3, "#######0.0000")
        y4 = Format(x4 * x4, "#######0.0000")
        y5 = Format(x5 * x5, "#######0.0000")

        a1 = Format((y1 + y2 + y3 + y4 + y5) / 5, "#######0.0000")

        b1 = Format(Math.Sqrt(a1), "#######0.0000")

        SD = Format(b1, "#######0.0000")

        CV = Format(SD / Mean * 100, "#######0.0000")

        lbl_Test2_Cone5_Mean.Text = Mean
        lbl_Test2_Cone5_SD.Text = SD
        lbl_Test2_Cone5_CV.Text = CV
        Final_Test2_Average_Mean_SD_CV_Calculation()
    End Sub
    Private Sub txt_test2_Cone6_YarnWeight1_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Test2_Cone6_YarnWeight1.TextChanged
        If Val(txt_Test2_Cone6_YarnWeight1.Text) <> 0 Then
            lbl_Test2_Cone6_YarnCount1.Text = Format(64.814 / Val(txt_Test2_Cone6_YarnWeight1.Text), "#######0.0000")
        End If
        test2_Cone6_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub txt_test2_Cone6_YarnWeight2_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Test2_Cone6_YarnWeight2.TextChanged
        If Val(txt_Test2_Cone6_YarnWeight2.Text) <> 0 Then
            lbl_Test2_Cone6_YarnCount2.Text = Format(64.814 / Val(txt_Test2_Cone6_YarnWeight2.Text), "#######0.0000")
        End If
        test2_Cone6_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub txt_test2_Cone6_YarnWeight3_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Test2_Cone6_YarnWeight3.TextChanged
        If Val(txt_Test2_Cone6_YarnWeight3.Text) <> 0 Then
            lbl_Test2_Cone6_YarnCount3.Text = Format(64.814 / Val(txt_Test2_Cone6_YarnWeight3.Text), "#######0.0000")
        End If
        test2_Cone6_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub txt_test2_Cone6_YarnWeight4_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Test2_Cone6_YarnWeight4.TextChanged
        If Val(txt_Test2_Cone6_YarnWeight4.Text) <> 0 Then
            lbl_Test2_Cone6_YarnCount4.Text = Format(64.814 / Val(txt_Test2_Cone6_YarnWeight4.Text), "#######0.0000")
        End If
        test2_Cone6_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub txt_test2_Cone6_YarnWeight5_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Test2_Cone6_YarnWeight5.TextChanged
        If Val(txt_Test2_Cone6_YarnWeight5.Text) <> 0 Then
            lbl_Test2_Cone6_YarnCount5.Text = Format(64.814 / Val(txt_Test2_Cone6_YarnWeight5.Text), "#######0.0000")
        End If
        test2_Cone6_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub test2_Cone6_Average_Mean_SD_CV_Calculation()
        Dim Mean As Single = 0
        Dim SD As Single = 0
        Dim CV As Single = 0

        Dim x1 As Single = 0
        Dim x2 As Single = 0
        Dim x3 As Single = 0
        Dim x4 As Single = 0
        Dim x5 As Single = 0

        Dim y1 As Single = 0
        Dim y2 As Single = 0
        Dim y3 As Single = 0
        Dim y4 As Single = 0
        Dim y5 As Single = 0

        Dim z1 As Single = 0
        Dim z2 As Single = 0
        Dim z3 As Single = 0
        Dim z4 As Single = 0
        Dim z5 As Single = 0

        Dim a1 As Single = 0
        Dim b1 As Single = 0
        Dim c1 As Single = 0

        Mean = Format((Val(lbl_Test2_Cone6_YarnCount1.Text) + Val(lbl_Test2_Cone6_YarnCount2.Text) + Val(lbl_Test2_Cone6_YarnCount3.Text) + Val(lbl_Test2_Cone6_YarnCount4.Text) + Val(lbl_Test2_Cone6_YarnCount5.Text)) / 5, "#######0.0000")

        x1 = Val(lbl_Test2_Cone6_YarnCount1.Text) - Mean
        x2 = Val(lbl_Test2_Cone6_YarnCount2.Text) - Mean
        x3 = Val(lbl_Test2_Cone6_YarnCount3.Text) - Mean
        x4 = Val(lbl_Test2_Cone6_YarnCount4.Text) - Mean
        x5 = Val(lbl_Test2_Cone6_YarnCount5.Text) - Mean

        y1 = Format(x1 * x1, "#######0.0000")
        y2 = Format(x2 * x2, "#######0.0000")
        y3 = Format(x3 * x3, "#######0.0000")
        y4 = Format(x4 * x4, "#######0.0000")
        y5 = Format(x5 * x5, "#######0.0000")

        a1 = Format((y1 + y2 + y3 + y4 + y5) / 5, "#######0.0000")

        b1 = Format(Math.Sqrt(a1), "#######0.0000")

        SD = Format(b1, "#######0.0000")

        CV = Format(SD / Mean * 100, "#######0.0000")

        lbl_Test2_Cone6_Mean.Text = Mean
        lbl_Test2_Cone6_SD.Text = SD
        lbl_Test2_Cone6_CV.Text = CV
        Final_Test2_Average_Mean_SD_CV_Calculation()
    End Sub





    Private Sub txt_test3_Cone1_YarnWeight1_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Test3_Cone1_YarnWeight1.TextChanged
        If Val(txt_Test3_Cone1_YarnWeight1.Text) <> 0 Then
            lbl_Test3_Cone1_YarnCount1.Text = Format(64.814 / Val(txt_Test3_Cone1_YarnWeight1.Text), "#######0.0000")
        End If
        test3_Cone1_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub txt_test3_Cone1_YarnWeight2_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Test3_Cone1_YarnWeight2.TextChanged
        If Val(txt_Test3_Cone1_YarnWeight2.Text) <> 0 Then
            lbl_Test3_Cone1_YarnCount2.Text = Format(64.814 / Val(txt_Test3_Cone1_YarnWeight2.Text), "#######0.0000")
        End If
        test3_Cone1_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub txt_test3_Cone1_YarnWeight3_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Test3_Cone1_YarnWeight3.TextChanged
        If Val(txt_Test3_Cone1_YarnWeight3.Text) <> 0 Then
            lbl_Test3_Cone1_YarnCount3.Text = Format(64.814 / Val(txt_Test3_Cone1_YarnWeight3.Text), "#######0.0000")
        End If
        test3_Cone1_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub txt_test3_Cone1_YarnWeight4_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Test3_Cone1_YarnWeight4.TextChanged
        If Val(txt_Test3_Cone1_YarnWeight4.Text) <> 0 Then
            lbl_Test3_Cone1_YarnCount4.Text = Format(64.814 / Val(txt_Test3_Cone1_YarnWeight4.Text), "#######0.0000")
        End If
        test3_Cone1_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub txt_test3_Cone1_YarnWeight5_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Test3_Cone1_YarnWeight5.TextChanged
        If Val(txt_Test3_Cone1_YarnWeight5.Text) <> 0 Then
            lbl_Test3_Cone1_YarnCount5.Text = Format(64.814 / Val(txt_Test3_Cone1_YarnWeight5.Text), "#######0.0000")
        End If
        test3_Cone1_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub test3_Cone1_Average_Mean_SD_CV_Calculation()
        Dim Mean As Single = 0
        Dim SD As Single = 0
        Dim CV As Single = 0

        Dim x1 As Single = 0
        Dim x2 As Single = 0
        Dim x3 As Single = 0
        Dim x4 As Single = 0
        Dim x5 As Single = 0

        Dim y1 As Single = 0
        Dim y2 As Single = 0
        Dim y3 As Single = 0
        Dim y4 As Single = 0
        Dim y5 As Single = 0

        Dim z1 As Single = 0
        Dim z2 As Single = 0
        Dim z3 As Single = 0
        Dim z4 As Single = 0
        Dim z5 As Single = 0

        Dim a1 As Single = 0
        Dim b1 As Single = 0
        Dim c1 As Single = 0

        Mean = Format((Val(lbl_Test3_Cone1_YarnCount1.Text) + Val(lbl_Test3_Cone1_YarnCount2.Text) + Val(lbl_Test3_Cone1_YarnCount3.Text) + Val(lbl_Test3_Cone1_YarnCount4.Text) + Val(lbl_Test3_Cone1_YarnCount5.Text)) / 5, "#######0.0000")

        x1 = Val(lbl_Test3_Cone1_YarnCount1.Text) - Mean
        x2 = Val(lbl_Test3_Cone1_YarnCount2.Text) - Mean
        x3 = Val(lbl_Test3_Cone1_YarnCount3.Text) - Mean
        x4 = Val(lbl_Test3_Cone1_YarnCount4.Text) - Mean
        x5 = Val(lbl_Test3_Cone1_YarnCount5.Text) - Mean

        y1 = Format(x1 * x1, "#######0.0000")
        y2 = Format(x2 * x2, "#######0.0000")
        y3 = Format(x3 * x3, "#######0.0000")
        y4 = Format(x4 * x4, "#######0.0000")
        y5 = Format(x5 * x5, "#######0.0000")

        a1 = Format((y1 + y2 + y3 + y4 + y5) / 5, "#######0.0000")

        b1 = Format(Math.Sqrt(a1), "#######0.0000")

        SD = Format(b1, "#######0.0000")

        CV = Format(SD / Mean * 100, "#######0.0000")

        lbl_Test3_Cone1_Mean.Text = Mean
        lbl_Test3_Cone1_SD.Text = SD
        lbl_Test3_Cone1_CV.Text = CV
        Final_Test3_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub txt_test3_Cone2_YarnWeight1_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Test3_Cone2_YarnWeight1.TextChanged
        If Val(txt_Test3_Cone2_YarnWeight1.Text) <> 0 Then
            lbl_Test3_Cone2_YarnCount1.Text = Format(64.814 / Val(txt_Test3_Cone2_YarnWeight1.Text), "#######0.0000")
        End If
        test3_Cone2_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub txt_test3_Cone2_YarnWeight2_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Test3_Cone2_YarnWeight2.TextChanged
        If Val(txt_Test3_Cone2_YarnWeight2.Text) <> 0 Then
            lbl_Test3_Cone2_YarnCount2.Text = Format(64.814 / Val(txt_Test3_Cone2_YarnWeight2.Text), "#######0.0000")
        End If
        test3_Cone2_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub txt_test3_Cone2_YarnWeight3_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Test3_Cone2_YarnWeight3.TextChanged
        If Val(txt_Test3_Cone2_YarnWeight3.Text) <> 0 Then
            lbl_Test3_Cone2_YarnCount3.Text = Format(64.814 / Val(txt_Test3_Cone2_YarnWeight3.Text), "#######0.0000")
        End If
        test3_Cone2_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub txt_test3_Cone2_YarnWeight4_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Test3_Cone2_YarnWeight4.TextChanged
        If Val(txt_Test3_Cone2_YarnWeight4.Text) <> 0 Then
            lbl_Test3_Cone2_YarnCount4.Text = Format(64.814 / Val(txt_Test3_Cone2_YarnWeight4.Text), "#######0.0000")
        End If
        test3_Cone2_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub txt_test3_Cone2_YarnWeight5_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Test3_Cone2_YarnWeight5.TextChanged
        If Val(txt_Test3_Cone2_YarnWeight5.Text) <> 0 Then
            lbl_Test3_Cone2_YarnCount5.Text = Format(64.814 / Val(txt_Test3_Cone2_YarnWeight5.Text), "#######0.0000")
        End If
        test3_Cone2_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub test3_Cone2_Average_Mean_SD_CV_Calculation()
        Dim Mean As Single = 0
        Dim SD As Single = 0
        Dim CV As Single = 0

        Dim x1 As Single = 0
        Dim x2 As Single = 0
        Dim x3 As Single = 0
        Dim x4 As Single = 0
        Dim x5 As Single = 0

        Dim y1 As Single = 0
        Dim y2 As Single = 0
        Dim y3 As Single = 0
        Dim y4 As Single = 0
        Dim y5 As Single = 0

        Dim z1 As Single = 0
        Dim z2 As Single = 0
        Dim z3 As Single = 0
        Dim z4 As Single = 0
        Dim z5 As Single = 0

        Dim a1 As Single = 0
        Dim b1 As Single = 0
        Dim c1 As Single = 0

        Mean = Format((Val(lbl_Test3_Cone2_YarnCount1.Text) + Val(lbl_Test3_Cone2_YarnCount2.Text) + Val(lbl_Test3_Cone2_YarnCount3.Text) + Val(lbl_Test3_Cone2_YarnCount4.Text) + Val(lbl_Test3_Cone2_YarnCount5.Text)) / 5, "#######0.0000")

        x1 = Val(lbl_Test3_Cone2_YarnCount1.Text) - Mean
        x2 = Val(lbl_Test3_Cone2_YarnCount2.Text) - Mean
        x3 = Val(lbl_Test3_Cone2_YarnCount3.Text) - Mean
        x4 = Val(lbl_Test3_Cone2_YarnCount4.Text) - Mean
        x5 = Val(lbl_Test3_Cone2_YarnCount5.Text) - Mean

        y1 = Format(x1 * x1, "#######0.0000")
        y2 = Format(x2 * x2, "#######0.0000")
        y3 = Format(x3 * x3, "#######0.0000")
        y4 = Format(x4 * x4, "#######0.0000")
        y5 = Format(x5 * x5, "#######0.0000")

        a1 = Format((y1 + y2 + y3 + y4 + y5) / 5, "#######0.0000")

        b1 = Format(Math.Sqrt(a1), "#######0.0000")

        SD = Format(b1, "#######0.0000")

        CV = Format(SD / Mean * 100, "#######0.0000")

        lbl_Test3_Cone2_mean.Text = Mean
        lbl_Test3_Cone2_SD.Text = SD
        lbl_Test3_Cone2_CV.Text = CV
        Final_Test3_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub txt_test3_Cone3_YarnWeight1_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Test3_Cone3_YarnWeight1.TextChanged
        If Val(txt_Test3_Cone3_YarnWeight1.Text) <> 0 Then
            lbl_Test3_Cone3_YarnCount1.Text = Format(64.814 / Val(txt_Test3_Cone3_YarnWeight1.Text), "#######0.0000")
        End If
        test3_Cone3_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub txt_test3_Cone3_YarnWeight2_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Test3_Cone3_YarnWeight2.TextChanged
        If Val(txt_Test3_Cone3_YarnWeight2.Text) <> 0 Then
            lbl_Test3_Cone3_YarnCount2.Text = Format(64.814 / Val(txt_Test3_Cone3_YarnWeight2.Text), "#######0.0000")
        End If
        test3_Cone3_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub txt_test3_Cone3_YarnWeight3_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Test3_Cone3_YarnWeight3.TextChanged
        If Val(txt_Test3_Cone3_YarnWeight3.Text) <> 0 Then
            lbl_Test3_Cone3_YarnCount3.Text = Format(64.814 / Val(txt_Test3_Cone3_YarnWeight3.Text), "#######0.0000")
        End If
        test3_Cone3_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub txt_test3_Cone3_YarnWeight4_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Test3_Cone3_YarnWeight4.TextChanged
        If Val(txt_Test3_Cone3_YarnWeight4.Text) <> 0 Then
            lbl_Test3_Cone3_YarnCount4.Text = Format(64.814 / Val(txt_Test3_Cone3_YarnWeight4.Text), "#######0.0000")
        End If
        test3_Cone3_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub txt_test3_Cone3_YarnWeight5_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Test3_Cone3_YarnWeight5.TextChanged
        If Val(txt_Test3_Cone3_YarnWeight5.Text) <> 0 Then
            lbl_Test3_Cone3_YarnCount5.Text = Format(64.814 / Val(txt_Test3_Cone3_YarnWeight5.Text), "#######0.0000")
        End If
        test3_Cone3_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub test3_Cone3_Average_Mean_SD_CV_Calculation()
        Dim Mean As Single = 0
        Dim SD As Single = 0
        Dim CV As Single = 0

        Dim x1 As Single = 0
        Dim x2 As Single = 0
        Dim x3 As Single = 0
        Dim x4 As Single = 0
        Dim x5 As Single = 0

        Dim y1 As Single = 0
        Dim y2 As Single = 0
        Dim y3 As Single = 0
        Dim y4 As Single = 0
        Dim y5 As Single = 0

        Dim z1 As Single = 0
        Dim z2 As Single = 0
        Dim z3 As Single = 0
        Dim z4 As Single = 0
        Dim z5 As Single = 0

        Dim a1 As Single = 0
        Dim b1 As Single = 0
        Dim c1 As Single = 0

        Mean = Format((Val(lbl_Test3_Cone3_YarnCount1.Text) + Val(lbl_Test3_Cone3_YarnCount2.Text) + Val(lbl_Test3_Cone3_YarnCount3.Text) + Val(lbl_Test3_Cone3_YarnCount4.Text) + Val(lbl_Test3_Cone3_YarnCount5.Text)) / 5, "#######0.0000")

        x1 = Val(lbl_Test3_Cone3_YarnCount1.Text) - Mean
        x2 = Val(lbl_Test3_Cone3_YarnCount2.Text) - Mean
        x3 = Val(lbl_Test3_Cone3_YarnCount3.Text) - Mean
        x4 = Val(lbl_Test3_Cone3_YarnCount4.Text) - Mean
        x5 = Val(lbl_Test3_Cone3_YarnCount5.Text) - Mean

        y1 = Format(x1 * x1, "#######0.0000")
        y2 = Format(x2 * x2, "#######0.0000")
        y3 = Format(x3 * x3, "#######0.0000")
        y4 = Format(x4 * x4, "#######0.0000")
        y5 = Format(x5 * x5, "#######0.0000")

        a1 = Format((y1 + y2 + y3 + y4 + y5) / 5, "#######0.0000")

        b1 = Format(Math.Sqrt(a1), "#######0.0000")

        SD = Format(b1, "#######0.0000")

        CV = Format(SD / Mean * 100, "#######0.0000")

        lbl_Test3_Cone3_mean.Text = Mean
        lbl_Test3_Cone3_SD.Text = SD
        lbl_Test3_Cone3_CV.Text = CV

    End Sub

    Private Sub txt_test3_Cone4_YarnWeight1_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Test3_Cone4_YarnWeight1.TextChanged
        If Val(txt_Test3_Cone4_YarnWeight1.Text) <> 0 Then
            lbl_Test3_Cone4_YarnCount1.Text = Format(64.814 / Val(txt_Test3_Cone4_YarnWeight1.Text), "#######0.0000")
        End If
        test3_Cone4_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub txt_test3_Cone4_YarnWeight2_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Test3_Cone4_YarnWeight2.TextChanged
        If Val(txt_Test3_Cone4_YarnWeight2.Text) <> 0 Then
            lbl_Test3_Cone4_YarnCount2.Text = Format(64.814 / Val(txt_Test3_Cone4_YarnWeight2.Text), "#######0.0000")
        End If
        test3_Cone4_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub txt_test3_Cone4_YarnWeight3_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Test3_Cone4_YarnWeight3.TextChanged
        If Val(txt_Test3_Cone4_YarnWeight3.Text) <> 0 Then
            lbl_Test3_Cone4_YarnCount3.Text = Format(64.814 / Val(txt_Test3_Cone4_YarnWeight3.Text), "#######0.0000")
        End If
        test3_Cone4_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub txt_test3_Cone4_YarnWeight4_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Test3_Cone4_YarnWeight4.TextChanged
        If Val(txt_Test3_Cone4_YarnWeight4.Text) <> 0 Then
            lbl_Test3_Cone4_YarnCount4.Text = Format(64.814 / Val(txt_Test3_Cone4_YarnWeight4.Text), "#######0.0000")
        End If
        test3_Cone4_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub txt_test3_Cone4_YarnWeight5_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Test3_Cone4_YarnWeight5.TextChanged
        If Val(txt_Test3_Cone4_YarnWeight5.Text) <> 0 Then
            lbl_Test3_Cone4_YarnCount5.Text = Format(64.814 / Val(txt_Test3_Cone4_YarnWeight5.Text), "#######0.0000")
        End If
        test3_Cone4_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub test3_Cone4_Average_Mean_SD_CV_Calculation()
        Dim Mean As Single = 0
        Dim SD As Single = 0
        Dim CV As Single = 0

        Dim x1 As Single = 0
        Dim x2 As Single = 0
        Dim x3 As Single = 0
        Dim x4 As Single = 0
        Dim x5 As Single = 0

        Dim y1 As Single = 0
        Dim y2 As Single = 0
        Dim y3 As Single = 0
        Dim y4 As Single = 0
        Dim y5 As Single = 0

        Dim z1 As Single = 0
        Dim z2 As Single = 0
        Dim z3 As Single = 0
        Dim z4 As Single = 0
        Dim z5 As Single = 0

        Dim a1 As Single = 0
        Dim b1 As Single = 0
        Dim c1 As Single = 0

        Mean = Format((Val(lbl_Test3_Cone4_YarnCount1.Text) + Val(lbl_Test3_Cone4_YarnCount2.Text) + Val(lbl_Test3_Cone4_YarnCount3.Text) + Val(lbl_Test3_Cone4_YarnCount4.Text) + Val(lbl_Test3_Cone4_YarnCount5.Text)) / 5, "#######0.0000")

        x1 = Val(lbl_Test3_Cone4_YarnCount1.Text) - Mean
        x2 = Val(lbl_Test3_Cone4_YarnCount2.Text) - Mean
        x3 = Val(lbl_Test3_Cone4_YarnCount3.Text) - Mean
        x4 = Val(lbl_Test3_Cone4_YarnCount4.Text) - Mean
        x5 = Val(lbl_Test3_Cone4_YarnCount5.Text) - Mean

        y1 = Format(x1 * x1, "#######0.0000")
        y2 = Format(x2 * x2, "#######0.0000")
        y3 = Format(x3 * x3, "#######0.0000")
        y4 = Format(x4 * x4, "#######0.0000")
        y5 = Format(x5 * x5, "#######0.0000")

        a1 = Format((y1 + y2 + y3 + y4 + y5) / 5, "#######0.0000")

        b1 = Format(Math.Sqrt(a1), "#######0.0000")

        SD = Format(b1, "#######0.0000")

        CV = Format(SD / Mean * 100, "#######0.0000")

        lbl_Test3_Cone4_mean.Text = Mean
        lbl_Test3_Cone4_SD.Text = SD
        lbl_Test3_Cone4_CV.Text = CV
        Final_Test3_Average_Mean_SD_CV_Calculation()
    End Sub
    Private Sub txt_test3_Cone5_YarnWeight1_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Test3_Cone5_YarnWeight1.TextChanged
        If Val(txt_Test3_Cone5_YarnWeight1.Text) <> 0 Then
            lbl_Test3_Cone5_YarnCount1.Text = Format(64.814 / Val(txt_Test3_Cone5_YarnWeight1.Text), "#######0.0000")
        End If
        test3_Cone5_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub txt_test3_Cone5_YarnWeight2_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Test3_Cone5_YarnWeight2.TextChanged
        If Val(txt_Test3_Cone5_YarnWeight2.Text) <> 0 Then
            lbl_Test3_Cone5_YarnCount2.Text = Format(64.814 / Val(txt_Test3_Cone5_YarnWeight2.Text), "#######0.0000")
        End If
        test3_Cone5_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub txt_test3_Cone5_YarnWeight3_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Test3_Cone5_YarnWeight3.TextChanged
        If Val(txt_Test3_Cone5_YarnWeight3.Text) <> 0 Then
            lbl_Test3_Cone5_YarnCount3.Text = Format(64.814 / Val(txt_Test3_Cone5_YarnWeight3.Text), "#######0.0000")
        End If
        test3_Cone5_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub txt_test3_Cone5_YarnWeight4_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Test3_Cone5_YarnWeight4.TextChanged
        If Val(txt_Test3_Cone5_YarnWeight4.Text) <> 0 Then
            lbl_Test3_Cone5_YarnCount4.Text = Format(64.814 / Val(txt_Test3_Cone5_YarnWeight4.Text), "#######0.0000")
        End If
        test3_Cone5_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub txt_test3_Cone5_YarnWeight5_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Test3_Cone5_YarnWeight5.TextChanged
        If Val(txt_Test3_Cone5_YarnWeight5.Text) <> 0 Then
            lbl_Test3_Cone5_YarnCount5.Text = Format(64.814 / Val(txt_Test3_Cone5_YarnWeight5.Text), "#######0.0000")
        End If
        test3_Cone5_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub test3_Cone5_Average_Mean_SD_CV_Calculation()
        Dim Mean As Single = 0
        Dim SD As Single = 0
        Dim CV As Single = 0

        Dim x1 As Single = 0
        Dim x2 As Single = 0
        Dim x3 As Single = 0
        Dim x4 As Single = 0
        Dim x5 As Single = 0

        Dim y1 As Single = 0
        Dim y2 As Single = 0
        Dim y3 As Single = 0
        Dim y4 As Single = 0
        Dim y5 As Single = 0

        Dim z1 As Single = 0
        Dim z2 As Single = 0
        Dim z3 As Single = 0
        Dim z4 As Single = 0
        Dim z5 As Single = 0

        Dim a1 As Single = 0
        Dim b1 As Single = 0
        Dim c1 As Single = 0

        Mean = Format((Val(lbl_Test3_Cone5_YarnCount1.Text) + Val(lbl_Test3_Cone5_YarnCount2.Text) + Val(lbl_Test3_Cone5_YarnCount3.Text) + Val(lbl_Test3_Cone5_YarnCount4.Text) + Val(lbl_Test3_Cone5_YarnCount5.Text)) / 5, "#######0.0000")

        x1 = Val(lbl_Test3_Cone5_YarnCount1.Text) - Mean
        x2 = Val(lbl_Test3_Cone5_YarnCount2.Text) - Mean
        x3 = Val(lbl_Test3_Cone5_YarnCount3.Text) - Mean
        x4 = Val(lbl_Test3_Cone5_YarnCount4.Text) - Mean
        x5 = Val(lbl_Test3_Cone5_YarnCount5.Text) - Mean

        y1 = Format(x1 * x1, "#######0.0000")
        y2 = Format(x2 * x2, "#######0.0000")
        y3 = Format(x3 * x3, "#######0.0000")
        y4 = Format(x4 * x4, "#######0.0000")
        y5 = Format(x5 * x5, "#######0.0000")

        a1 = Format((y1 + y2 + y3 + y4 + y5) / 5, "#######0.0000")

        b1 = Format(Math.Sqrt(a1), "#######0.0000")

        SD = Format(b1, "#######0.0000")

        CV = Format(SD / Mean * 100, "#######0.0000")

        lbl_Test3_Cone5_Mean.Text = Mean
        lbl_Test3_Cone5_SD.Text = SD
        lbl_Test3_Cone5_CV.Text = CV
        Final_Test3_Average_Mean_SD_CV_Calculation()
    End Sub
    Private Sub txt_test3_Cone6_YarnWeight1_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Test3_Cone6_YarnWeight1.TextChanged
        If Val(txt_Test3_Cone6_YarnWeight1.Text) <> 0 Then
            lbl_Test3_Cone6_YarnCount1.Text = Format(64.814 / Val(txt_Test3_Cone6_YarnWeight1.Text), "#######0.0000")
        End If
        test3_Cone6_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub txt_test3_Cone6_YarnWeight2_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Test3_Cone6_YarnWeight2.TextChanged
        If Val(txt_Test3_Cone6_YarnWeight2.Text) <> 0 Then
            lbl_Test3_Cone6_YarnCount2.Text = Format(64.814 / Val(txt_Test3_Cone6_YarnWeight2.Text), "#######0.0000")
        End If
        test3_Cone6_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub txt_test3_Cone6_YarnWeight3_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Test3_Cone6_YarnWeight3.TextChanged
        If Val(txt_Test3_Cone6_YarnWeight3.Text) <> 0 Then
            lbl_Test3_Cone6_YarnCount3.Text = Format(64.814 / Val(txt_Test3_Cone6_YarnWeight3.Text), "#######0.0000")
        End If
        test3_Cone6_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub txt_test3_Cone6_YarnWeight4_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Test3_Cone6_YarnWeight4.TextChanged
        If Val(txt_Test3_Cone6_YarnWeight4.Text) <> 0 Then
            lbl_Test3_Cone6_YarnCount4.Text = Format(64.814 / Val(txt_Test3_Cone6_YarnWeight4.Text), "#######0.0000")
        End If
        test3_Cone6_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub txt_test3_Cone6_YarnWeight5_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Test3_Cone6_YarnWeight5.TextChanged
        If Val(txt_Test3_Cone6_YarnWeight5.Text) <> 0 Then
            lbl_Test3_Cone6_YarnCount5.Text = Format(64.814 / Val(txt_Test3_Cone6_YarnWeight5.Text), "#######0.0000")
        End If
        test3_Cone6_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub test3_Cone6_Average_Mean_SD_CV_Calculation()
        Dim Mean As Single = 0
        Dim SD As Single = 0
        Dim CV As Single = 0

        Dim x1 As Single = 0
        Dim x2 As Single = 0
        Dim x3 As Single = 0
        Dim x4 As Single = 0
        Dim x5 As Single = 0

        Dim y1 As Single = 0
        Dim y2 As Single = 0
        Dim y3 As Single = 0
        Dim y4 As Single = 0
        Dim y5 As Single = 0

        Dim z1 As Single = 0
        Dim z2 As Single = 0
        Dim z3 As Single = 0
        Dim z4 As Single = 0
        Dim z5 As Single = 0

        Dim a1 As Single = 0
        Dim b1 As Single = 0
        Dim c1 As Single = 0

        Mean = Format((Val(lbl_Test3_Cone6_YarnCount1.Text) + Val(lbl_Test3_Cone6_YarnCount2.Text) + Val(lbl_Test3_Cone6_YarnCount3.Text) + Val(lbl_Test3_Cone6_YarnCount4.Text) + Val(lbl_Test3_Cone6_YarnCount5.Text)) / 5, "#######0.0000")

        x1 = Val(lbl_Test3_Cone6_YarnCount1.Text) - Mean
        x2 = Val(lbl_Test3_Cone6_YarnCount2.Text) - Mean
        x3 = Val(lbl_Test3_Cone6_YarnCount3.Text) - Mean
        x4 = Val(lbl_Test3_Cone6_YarnCount4.Text) - Mean
        x5 = Val(lbl_Test3_Cone6_YarnCount5.Text) - Mean

        y1 = Format(x1 * x1, "#######0.0000")
        y2 = Format(x2 * x2, "#######0.0000")
        y3 = Format(x3 * x3, "#######0.0000")
        y4 = Format(x4 * x4, "#######0.0000")
        y5 = Format(x5 * x5, "#######0.0000")

        a1 = Format((y1 + y2 + y3 + y4 + y5) / 5, "#######0.0000")

        b1 = Format(Math.Sqrt(a1), "#######0.0000")

        SD = Format(b1, "#######0.0000")

        CV = Format(SD / Mean * 100, "#######0.0000")

        lbl_Test3_Cone6_Mean.Text = Mean
        lbl_Test3_Cone6_SD.Text = SD
        lbl_Test3_Cone6_CV.Text = CV
        Final_Test3_Average_Mean_SD_CV_Calculation()
    End Sub

    Private Sub Final_Test1_Average_Mean_SD_CV_Calculation()
        Dim Mean As Single = 0
        Dim SD As Single = 0
        Dim CV As Single = 0

        Dim x1 As Single = 0
        Dim x2 As Single = 0
        Dim x3 As Single = 0
        Dim x4 As Single = 0
        Dim x5 As Single = 0
        Dim x6 As Single = 0

        Dim y1 As Single = 0
        Dim y2 As Single = 0
        Dim y3 As Single = 0
        Dim y4 As Single = 0
        Dim y5 As Single = 0
        Dim y6 As Single = 0

        Dim z1 As Single = 0
        Dim z2 As Single = 0
        Dim z3 As Single = 0
        Dim z4 As Single = 0
        Dim z5 As Single = 0
        Dim z6 As Single = 0

        Dim a1 As Single = 0
        Dim b1 As Single = 0
        Dim c1 As Single = 0

        If Val(lbl_Test1_Cone1_Mean.Text) <> 0 And Val(lbl_Test1_Cone2_mean.Text) <> 0 And Val(lbl_Test1_Cone3_mean.Text) <> 0 And Val(lbl_Test1_Cone4_mean.Text) <> 0 And Val(lbl_Test1_Cone5_mean.Text) <> 0 And Val(lbl_Test1_Cone6_mean.Text) <> 0 Then
            Mean = Format((Val(lbl_Test1_Cone1_Mean.Text) + Val(lbl_Test1_Cone2_mean.Text) + Val(lbl_Test1_Cone3_mean.Text) + Val(lbl_Test1_Cone4_mean.Text) + Val(lbl_Test1_Cone5_mean.Text) + Val(lbl_Test1_Cone6_mean.Text)) / 6, "#######0.0000")

            x1 = Val(lbl_Test1_Cone1_Mean.Text) - Mean
            x2 = Val(lbl_Test1_Cone2_mean.Text) - Mean
            x3 = Val(lbl_Test1_Cone3_mean.Text) - Mean
            x4 = Val(lbl_Test1_Cone4_mean.Text) - Mean
            x5 = Val(lbl_Test1_Cone5_mean.Text) - Mean
            x6 = Val(lbl_Test1_Cone6_mean.Text) - Mean

            y1 = Format(x1 * x1, "#######0.0000")
            y2 = Format(x2 * x2, "#######0.0000")
            y3 = Format(x3 * x3, "#######0.0000")
            y4 = Format(x4 * x4, "#######0.0000")
            y5 = Format(x5 * x5, "#######0.0000")
            y6 = Format(x6 * x6, "#######0.0000")


            a1 = Format((y1 + y2 + y3 + y4 + y5 + y6) / 6, "#######0.0000")

        ElseIf Val(lbl_Test1_Cone1_Mean.Text) <> 0 And Val(lbl_Test1_Cone2_mean.Text) <> 0 And Val(lbl_Test1_Cone3_mean.Text) <> 0 And Val(lbl_Test1_Cone4_mean.Text) <> 0 And Val(lbl_Test1_Cone5_mean.Text) <> 0 Then
            Mean = Format((Val(lbl_Test1_Cone1_Mean.Text) + Val(lbl_Test1_Cone2_mean.Text) + Val(lbl_Test1_Cone3_mean.Text) + Val(lbl_Test1_Cone4_mean.Text) + Val(lbl_Test1_Cone5_mean.Text)) / 5, "#######0.0000")

            x1 = Val(lbl_Test1_Cone1_Mean.Text) - Mean
            x2 = Val(lbl_Test1_Cone2_mean.Text) - Mean
            x3 = Val(lbl_Test1_Cone3_mean.Text) - Mean
            x4 = Val(lbl_Test1_Cone4_mean.Text) - Mean
            x5 = Val(lbl_Test1_Cone5_mean.Text) - Mean

            y1 = Format(x1 * x1, "#######0.0000")
            y2 = Format(x2 * x2, "#######0.0000")
            y3 = Format(x3 * x3, "#######0.0000")
            y4 = Format(x4 * x4, "#######0.0000")
            y5 = Format(x5 * x5, "#######0.0000")


            a1 = Format((y1 + y2 + y3 + y4 + y5) / 5, "#######0.0000")

        ElseIf Val(lbl_Test1_Cone1_Mean.Text) <> 0 And Val(lbl_Test1_Cone2_mean.Text) <> 0 And Val(lbl_Test1_Cone3_mean.Text) <> 0 And Val(lbl_Test1_Cone4_mean.Text) <> 0 Then

            Mean = Format((Val(lbl_Test1_Cone1_Mean.Text) + Val(lbl_Test1_Cone2_mean.Text) + Val(lbl_Test1_Cone3_mean.Text) + Val(lbl_Test1_Cone4_mean.Text)) / 4, "#######0.0000")

            x1 = Val(lbl_Test1_Cone1_Mean.Text) - Mean
            x2 = Val(lbl_Test1_Cone2_mean.Text) - Mean
            x3 = Val(lbl_Test1_Cone3_mean.Text) - Mean
            x4 = Val(lbl_Test1_Cone4_mean.Text) - Mean

            y1 = Format(x1 * x1, "#######0.0000")
            y2 = Format(x2 * x2, "#######0.0000")
            y3 = Format(x3 * x3, "#######0.0000")
            y4 = Format(x4 * x4, "#######0.0000")


            a1 = Format((y1 + y2 + y3 + y4) / 4, "#######0.0000")

        ElseIf Val(lbl_Test1_Cone1_Mean.Text) <> 0 And Val(lbl_Test1_Cone2_mean.Text) <> 0 And Val(lbl_Test1_Cone3_mean.Text) <> 0 Then
            Mean = Format((Val(lbl_Test1_Cone1_Mean.Text) + Val(lbl_Test1_Cone2_mean.Text) + Val(lbl_Test1_Cone3_mean.Text)) / 3, "#######0.0000")

            x1 = Val(lbl_Test1_Cone1_Mean.Text) - Mean
            x2 = Val(lbl_Test1_Cone2_mean.Text) - Mean
            x3 = Val(lbl_Test1_Cone3_mean.Text) - Mean

            y1 = Format(x1 * x1, "#######0.0000")
            y2 = Format(x2 * x2, "#######0.0000")
            y3 = Format(x3 * x3, "#######0.0000")


            a1 = Format((y1 + y2 + y3) / 3, "#######0.0000")

        ElseIf Val(lbl_Test1_Cone1_Mean.Text) <> 0 And Val(lbl_Test1_Cone2_mean.Text) <> 0 Then
            Mean = Format((Val(lbl_Test1_Cone1_Mean.Text) + Val(lbl_Test1_Cone2_mean.Text)) / 2, "#######0.0000")

            x1 = Val(lbl_Test1_Cone1_Mean.Text) - Mean
            x2 = Val(lbl_Test1_Cone2_mean.Text) - Mean

            y1 = Format(x1 * x1, "#######0.0000")
            y2 = Format(x2 * x2, "#######0.0000")


            a1 = Format((y1 + y2) / 2, "#######0.0000")

        ElseIf Val(lbl_Test1_Cone1_Mean.Text) <> 0 Then
            Mean = Format((Val(lbl_Test1_Cone1_Mean.Text)), "#######0.0000")

            x1 = Val(lbl_Test1_Cone1_Mean.Text) - Mean

            y1 = Format(x1 * x1, "#######0.0000")


            a1 = Format(y1, "#######0.0000")

        End If

        b1 = Format(Math.Sqrt(a1), "#######0.0000")

        SD = Format(b1, "#######0.0000")

        CV = Format(SD / Mean * 100, "#######0.0000")

        lbl_Test1_AvgCoul.Text = Mean
        lbl_test1_AvgSd.Text = SD
        lbl_Test1_AvgCv.Text = CV

    End Sub



    Private Sub Final_Test2_Average_Mean_SD_CV_Calculation()
        Dim Mean As Single = 0
        Dim SD As Single = 0
        Dim CV As Single = 0

        Dim x1 As Single = 0
        Dim x2 As Single = 0
        Dim x3 As Single = 0
        Dim x4 As Single = 0
        Dim x5 As Single = 0
        Dim x6 As Single = 0

        Dim y1 As Single = 0
        Dim y2 As Single = 0
        Dim y3 As Single = 0
        Dim y4 As Single = 0
        Dim y5 As Single = 0
        Dim y6 As Single = 0

        Dim z1 As Single = 0
        Dim z2 As Single = 0
        Dim z3 As Single = 0
        Dim z4 As Single = 0
        Dim z5 As Single = 0
        Dim z6 As Single = 0

        Dim a1 As Single = 0
        Dim b1 As Single = 0
        Dim c1 As Single = 0

        If Val(lbl_Test2_Cone1_Mean.Text) <> 0 And Val(lbl_Test2_Cone2_Mean.Text) <> 0 And Val(lbl_Test2_Cone3_Mean.Text) <> 0 And Val(lbl_Test2_Cone4_Mean.Text) <> 0 And Val(lbl_Test2_Cone5_Mean.Text) <> 0 And Val(lbl_Test2_Cone6_Mean.Text) <> 0 Then
            Mean = Format((Val(lbl_Test2_Cone1_Mean.Text) + Val(lbl_Test2_Cone2_Mean.Text) + Val(lbl_Test2_Cone3_Mean.Text) + Val(lbl_Test2_Cone4_Mean.Text) + Val(lbl_Test2_Cone5_Mean.Text) + Val(lbl_Test2_Cone6_Mean.Text)) / 6, "#######0.0000")

            x1 = Val(lbl_Test2_Cone1_Mean.Text) - Mean
            x2 = Val(lbl_Test2_Cone2_Mean.Text) - Mean
            x3 = Val(lbl_Test2_Cone3_Mean.Text) - Mean
            x4 = Val(lbl_Test2_Cone4_Mean.Text) - Mean
            x5 = Val(lbl_Test2_Cone5_Mean.Text) - Mean
            x6 = Val(lbl_Test2_Cone6_Mean.Text) - Mean

            y1 = Format(x1 * x1, "#######0.0000")
            y2 = Format(x2 * x2, "#######0.0000")
            y3 = Format(x3 * x3, "#######0.0000")
            y4 = Format(x4 * x4, "#######0.0000")
            y5 = Format(x5 * x5, "#######0.0000")
            y6 = Format(x6 * x6, "#######0.0000")


            a1 = Format((y1 + y2 + y3 + y4 + y5 + y6) / 6, "#######0.0000")

        ElseIf Val(lbl_Test2_Cone1_Mean.Text) <> 0 And Val(lbl_Test2_Cone2_Mean.Text) <> 0 And Val(lbl_Test2_Cone3_Mean.Text) <> 0 And Val(lbl_Test2_Cone4_Mean.Text) <> 0 And Val(lbl_Test2_Cone5_Mean.Text) <> 0 Then
            Mean = Format((Val(lbl_Test2_Cone1_Mean.Text) + Val(lbl_Test2_Cone2_Mean.Text) + Val(lbl_Test2_Cone3_Mean.Text) + Val(lbl_Test2_Cone4_Mean.Text) + Val(lbl_Test2_Cone5_Mean.Text)) / 5, "#######0.0000")

            x1 = Val(lbl_Test2_Cone1_Mean.Text) - Mean
            x2 = Val(lbl_Test2_Cone2_Mean.Text) - Mean
            x3 = Val(lbl_Test2_Cone3_Mean.Text) - Mean
            x4 = Val(lbl_Test2_Cone4_Mean.Text) - Mean
            x5 = Val(lbl_Test2_Cone5_Mean.Text) - Mean

            y1 = Format(x1 * x1, "#######0.0000")
            y2 = Format(x2 * x2, "#######0.0000")
            y3 = Format(x3 * x3, "#######0.0000")
            y4 = Format(x4 * x4, "#######0.0000")
            y5 = Format(x5 * x5, "#######0.0000")


            a1 = Format((y1 + y2 + y3 + y4 + y5) / 5, "#######0.0000")

        ElseIf Val(lbl_Test2_Cone1_Mean.Text) <> 0 And Val(lbl_Test2_Cone2_Mean.Text) <> 0 And Val(lbl_Test2_Cone3_Mean.Text) <> 0 And Val(lbl_Test2_Cone4_Mean.Text) <> 0 Then

            Mean = Format((Val(lbl_Test2_Cone1_Mean.Text) + Val(lbl_Test2_Cone2_Mean.Text) + Val(lbl_Test2_Cone3_Mean.Text) + Val(lbl_Test2_Cone4_Mean.Text)) / 4, "#######0.0000")

            x1 = Val(lbl_Test2_Cone1_Mean.Text) - Mean
            x2 = Val(lbl_Test2_Cone2_Mean.Text) - Mean
            x3 = Val(lbl_Test2_Cone3_Mean.Text) - Mean
            x4 = Val(lbl_Test2_Cone4_Mean.Text) - Mean

            y1 = Format(x1 * x1, "#######0.0000")
            y2 = Format(x2 * x2, "#######0.0000")
            y3 = Format(x3 * x3, "#######0.0000")
            y4 = Format(x4 * x4, "#######0.0000")


            a1 = Format((y1 + y2 + y3 + y4) / 4, "#######0.0000")

        ElseIf Val(lbl_Test2_Cone1_Mean.Text) <> 0 And Val(lbl_Test2_Cone2_Mean.Text) <> 0 And Val(lbl_Test2_Cone3_Mean.Text) <> 0 Then
            Mean = Format((Val(lbl_Test2_Cone1_Mean.Text) + Val(lbl_Test2_Cone2_Mean.Text) + Val(lbl_Test2_Cone3_Mean.Text)) / 3, "#######0.0000")

            x1 = Val(lbl_Test2_Cone1_Mean.Text) - Mean
            x2 = Val(lbl_Test2_Cone2_Mean.Text) - Mean
            x3 = Val(lbl_Test2_Cone3_Mean.Text) - Mean

            y1 = Format(x1 * x1, "#######0.0000")
            y2 = Format(x2 * x2, "#######0.0000")
            y3 = Format(x3 * x3, "#######0.0000")


            a1 = Format((y1 + y2 + y3) / 3, "#######0.0000")

        ElseIf Val(lbl_Test2_Cone1_Mean.Text) <> 0 And Val(lbl_Test2_Cone2_Mean.Text) <> 0 Then
            Mean = Format((Val(lbl_Test2_Cone1_Mean.Text) + Val(lbl_Test2_Cone2_Mean.Text)) / 2, "#######0.0000")

            x1 = Val(lbl_Test2_Cone1_Mean.Text) - Mean
            x2 = Val(lbl_Test2_Cone2_Mean.Text) - Mean

            y1 = Format(x1 * x1, "#######0.0000")
            y2 = Format(x2 * x2, "#######0.0000")


            a1 = Format((y1 + y2) / 2, "#######0.0000")

        ElseIf Val(lbl_Test2_Cone1_Mean.Text) <> 0 Then
            Mean = Format((Val(lbl_Test2_Cone1_Mean.Text)), "#######0.0000")

            x1 = Val(lbl_Test2_Cone1_Mean.Text) - Mean

            y1 = Format(x1 * x1, "#######0.0000")


            a1 = Format(y1, "#######0.0000")

        End If

        b1 = Format(Math.Sqrt(a1), "#######0.0000")

        SD = Format(b1, "#######0.0000")

        CV = Format(SD / Mean * 100, "#######0.0000")

        lbl_Test2_avgCoul.Text = Mean
        lbl_Test2_Avg_Sd.Text = SD
        lbl_Test2_AvgCv.Text = CV

    End Sub

    Private Sub Final_Test3_Average_Mean_SD_CV_Calculation()
        Dim Mean As Single = 0
        Dim SD As Single = 0
        Dim CV As Single = 0

        Dim x1 As Single = 0
        Dim x2 As Single = 0
        Dim x3 As Single = 0
        Dim x4 As Single = 0
        Dim x5 As Single = 0
        Dim x6 As Single = 0

        Dim y1 As Single = 0
        Dim y2 As Single = 0
        Dim y3 As Single = 0
        Dim y4 As Single = 0
        Dim y5 As Single = 0
        Dim y6 As Single = 0

        Dim z1 As Single = 0
        Dim z2 As Single = 0
        Dim z3 As Single = 0
        Dim z4 As Single = 0
        Dim z5 As Single = 0
        Dim z6 As Single = 0

        Dim a1 As Single = 0
        Dim b1 As Single = 0
        Dim c1 As Single = 0

        If Val(lbl_Test3_Cone1_Mean.Text) <> 0 And Val(lbl_Test3_Cone2_mean.Text) <> 0 And Val(lbl_Test3_Cone3_mean.Text) <> 0 And Val(lbl_Test3_Cone4_mean.Text) <> 0 And Val(lbl_Test3_Cone5_Mean.Text) <> 0 And Val(lbl_Test3_Cone6_Mean.Text) <> 0 Then
            Mean = Format((Val(lbl_Test3_Cone1_Mean.Text) + Val(lbl_Test3_Cone2_mean.Text) + Val(lbl_Test3_Cone3_mean.Text) + Val(lbl_Test3_Cone4_mean.Text) + Val(lbl_Test3_Cone5_Mean.Text) + Val(lbl_Test3_Cone6_Mean.Text)) / 6, "#######0.0000")

            x1 = Val(lbl_Test3_Cone1_Mean.Text) - Mean
            x2 = Val(lbl_Test3_Cone2_mean.Text) - Mean
            x3 = Val(lbl_Test3_Cone3_mean.Text) - Mean
            x4 = Val(lbl_Test3_Cone4_mean.Text) - Mean
            x5 = Val(lbl_Test3_Cone5_Mean.Text) - Mean
            x6 = Val(lbl_Test3_Cone6_Mean.Text) - Mean

            y1 = Format(x1 * x1, "#######0.0000")
            y2 = Format(x2 * x2, "#######0.0000")
            y3 = Format(x3 * x3, "#######0.0000")
            y4 = Format(x4 * x4, "#######0.0000")
            y5 = Format(x5 * x5, "#######0.0000")
            y6 = Format(x6 * x6, "#######0.0000")


            a1 = Format((y1 + y2 + y3 + y4 + y5 + y6) / 6, "#######0.0000")

        ElseIf Val(lbl_Test3_Cone1_Mean.Text) <> 0 And Val(lbl_Test3_Cone2_mean.Text) <> 0 And Val(lbl_Test3_Cone3_mean.Text) <> 0 And Val(lbl_Test3_Cone4_mean.Text) <> 0 And Val(lbl_Test3_Cone5_Mean.Text) <> 0 Then
            Mean = Format((Val(lbl_Test3_Cone1_Mean.Text) + Val(lbl_Test3_Cone2_mean.Text) + Val(lbl_Test3_Cone3_mean.Text) + Val(lbl_Test3_Cone4_mean.Text) + Val(lbl_Test3_Cone5_Mean.Text)) / 5, "#######0.0000")

            x1 = Val(lbl_Test3_Cone1_Mean.Text) - Mean
            x2 = Val(lbl_Test3_Cone2_mean.Text) - Mean
            x3 = Val(lbl_Test3_Cone3_mean.Text) - Mean
            x4 = Val(lbl_Test3_Cone4_mean.Text) - Mean
            x5 = Val(lbl_Test3_Cone5_Mean.Text) - Mean

            y1 = Format(x1 * x1, "#######0.0000")
            y2 = Format(x2 * x2, "#######0.0000")
            y3 = Format(x3 * x3, "#######0.0000")
            y4 = Format(x4 * x4, "#######0.0000")
            y5 = Format(x5 * x5, "#######0.0000")


            a1 = Format((y1 + y2 + y3 + y4 + y5) / 5, "#######0.0000")

        ElseIf Val(lbl_Test3_Cone1_Mean.Text) <> 0 And Val(lbl_Test3_Cone2_mean.Text) <> 0 And Val(lbl_Test3_Cone3_mean.Text) <> 0 And Val(lbl_Test3_Cone4_mean.Text) <> 0 Then

            Mean = Format((Val(lbl_Test3_Cone1_Mean.Text) + Val(lbl_Test3_Cone2_mean.Text) + Val(lbl_Test3_Cone3_mean.Text) + Val(lbl_Test3_Cone4_mean.Text)) / 4, "#######0.0000")

            x1 = Val(lbl_Test3_Cone1_Mean.Text) - Mean
            x2 = Val(lbl_Test3_Cone2_mean.Text) - Mean
            x3 = Val(lbl_Test3_Cone3_mean.Text) - Mean
            x4 = Val(lbl_Test3_Cone4_mean.Text) - Mean

            y1 = Format(x1 * x1, "#######0.0000")
            y2 = Format(x2 * x2, "#######0.0000")
            y3 = Format(x3 * x3, "#######0.0000")
            y4 = Format(x4 * x4, "#######0.0000")


            a1 = Format((y1 + y2 + y3 + y4) / 4, "#######0.0000")

        ElseIf Val(lbl_Test3_Cone1_Mean.Text) <> 0 And Val(lbl_Test3_Cone2_mean.Text) <> 0 And Val(lbl_Test3_Cone3_mean.Text) <> 0 Then
            Mean = Format((Val(lbl_Test3_Cone1_Mean.Text) + Val(lbl_Test3_Cone2_mean.Text) + Val(lbl_Test3_Cone3_mean.Text)) / 3, "#######0.0000")

            x1 = Val(lbl_Test3_Cone1_Mean.Text) - Mean
            x2 = Val(lbl_Test3_Cone2_mean.Text) - Mean
            x3 = Val(lbl_Test3_Cone3_mean.Text) - Mean

            y1 = Format(x1 * x1, "#######0.0000")
            y2 = Format(x2 * x2, "#######0.0000")
            y3 = Format(x3 * x3, "#######0.0000")


            a1 = Format((y1 + y2 + y3) / 3, "#######0.0000")

        ElseIf Val(lbl_Test3_Cone1_Mean.Text) <> 0 And Val(lbl_Test3_Cone2_mean.Text) <> 0 Then
            Mean = Format((Val(lbl_Test3_Cone1_Mean.Text) + Val(lbl_Test3_Cone2_mean.Text)) / 2, "#######0.0000")

            x1 = Val(lbl_Test3_Cone1_Mean.Text) - Mean
            x2 = Val(lbl_Test3_Cone2_mean.Text) - Mean

            y1 = Format(x1 * x1, "#######0.0000")
            y2 = Format(x2 * x2, "#######0.0000")


            a1 = Format((y1 + y2) / 2, "#######0.0000")

        ElseIf Val(lbl_Test3_Cone1_Mean.Text) <> 0 Then
            Mean = Format((Val(lbl_Test3_Cone1_Mean.Text)), "#######0.0000")

            x1 = Val(lbl_Test3_Cone1_Mean.Text) - Mean

            y1 = Format(x1 * x1, "#######0.0000")


            a1 = Format(y1, "#######0.0000")

        End If

        b1 = Format(Math.Sqrt(a1), "#######0.0000")

        SD = Format(b1, "#######0.0000")

        CV = Format(SD / Mean * 100, "#######0.0000")

        lbl_Test3_AvgCoul.Text = Mean
        lbl_Test3_AvgSd.Text = SD
        lbl_Test3_AvgCv.Text = CV

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

        LastNo = lbl_RefNo.Text

        movefirst_record()
        Timer1.Enabled = True

    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        If SaveAll_STS = True Then
        save_record()
        If Trim(UCase(LastNo)) = Trim(UCase(lbl_RefNo.Text)) Then
            Timer1.Enabled = False
            SaveAll_STS = False
                MessageBox.Show("All entries saved Successfully", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

            Else
                movenext_record()

            End If
        ElseIf DeleteAll_STS = True Then

            delete_record()
            If Trim(UCase(LastNo)) = Trim(UCase(lbl_RefNo.Text)) Then
                Timer1.Enabled = False
                DeleteAll_STS = False

                new_record()
                MessageBox.Show("All entries Deleted Successfully", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Else
            movenext_record()

        End If
        End If
    End Sub

    Private Sub cbo_Vechile_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_VehicleNo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Yarn_Sales_Head", "Vehicle_No", "", "")
    End Sub

    Private Sub cbo_VehicleNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_VehicleNo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_VehicleNo, txt_Freight, txt_Note, "Yarn_Sales_Head", "Vehicle_No", "", "")

    End Sub

    Private Sub cbo_VehicleNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_VehicleNo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_VehicleNo, txt_Note, "Yarn_Sales_Head", "Vehicle_No", "", "", False)

    End Sub
    Private Sub cbo_Type_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Type.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")

    End Sub

    Private Sub cbo_Type_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Type.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Type, cbo_PartyName, cbo_PurchaseAc, "", "", "", "")

    End Sub

    Private Sub cbo_Type_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Type.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Type, Nothing, "", "", "", "")
        If Asc(e.KeyChar) = 13 Then
            If Trim(UCase(cbo_Type.Text)) = "RECEIPT" Then

                cbo_PurchaseAc.Focus()

                If MessageBox.Show("Do you want to select receipt:", "FOR RECEIPT SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                    btn_Selection_Click(sender, e)

                Else
                    cbo_PurchaseAc.Focus()

                End If
            Else
                cbo_PurchaseAc.Focus()

            End If
        End If

    End Sub

    Private Sub btn_Selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Selection.Click
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim i As Integer, j As Integer, n As Integer, SNo As Integer
        Dim LedIdNo As Integer
        Dim NewCode As String
        Dim CompIDCondt As String
        Dim Ent_Bags As Single = 0
        Dim Ent_Cones As String = ""
        Dim Ent_Pcs As Single = 0
        Dim Ent_Wgt As Single = 0
        Dim Ent_Rate As Single = 0

        LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyName.Text)

        'If LedIdNo = 0 Then
        '    MessageBox.Show("Invalid Party Name", "DOES NOT SELECT ORDER...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '    If cbo_PartyName.Enabled And cbo_PartyName.Visible Then cbo_PartyName.Focus()
        '    Exit Sub
        'End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)


        CompIDCondt = "(a.company_idno = " & Str(Val(lbl_Company.Tag)) & ")"
        If Trim(UCase(Common_Procedures.settings.CompanyName)) = "-----~~~" Then
            CompIDCondt = ""
        End If

        'If Val(LedIdNo) <> 0 Then
        '    CompIDCondt = CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.ledger_Idno = " & Str(Val(LedIdNo))
        'End If

        With dgv_Selection

            .Rows.Clear()
            SNo = 0

            Da = New SqlClient.SqlDataAdapter("select a.*, b.*, c.Count_nAME, d.Mill_Name,e.Ledger_Name as DeliveryAt ,f.Ledger_Name as Transport,h.Bags as Ent_Bags, h.Cones as Ent_Cones, h.Weight as Ent_Wgt,h.Rate as Ent_Rate from Yarn_Purchase_Receipt_Head a INNER JOIN Yarn_Purchase_Receipt_details b ON a.Yarn_Purchase_Receipt_Code = b.Yarn_Purchase_Receipt_Code INNER JOIN Count_Head c ON b.Count_IdNo = c.Count_IdNo INNER JOIN Mill_Head d ON b.Mill_IdNo = d.Mill_IdNo  LEFT OUTER JOIN Ledger_Head e ON e.Ledger_IdNo = a.DeliveryAt_IdNo LEFT OUTER JOIN Ledger_Head f ON f.Ledger_IdNo = a.Transport_IdNo LEFT OUTER JOIN Yarn_Purchase_details h ON h.Yarn_Purchase_Code = '" & Trim(NewCode) & "' and b.Yarn_Purchase_Receipt_Code = h.Yarn_Purchase_Receipt_Code and b.Yarn_Purchase_Receipt_Details_SlNo = h.Yarn_Purchase_Receipt_Details_SlNo Where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & "  ((b.Weight - b.Purchase_Weight) > 0 or h.Weight > 0 ) order by a.Yarn_Purchase_Receipt_Date, a.for_orderby, a.Yarn_Purchase_Receipt_No", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()



                    Ent_Bags = 0
                    Ent_Cones = ""
                    Ent_Pcs = 0
                    Ent_Wgt = 0
                    Ent_Rate = 0
                    If IsDBNull(Dt1.Rows(i).Item("Ent_Bags").ToString) = False Then
                        Ent_Bags = Val(Dt1.Rows(i).Item("Ent_Bags").ToString)
                    End If
                    If IsDBNull(Dt1.Rows(i).Item("Ent_Cones").ToString) = False Then
                        Ent_Cones = Dt1.Rows(i).Item("Ent_Cones").ToString
                    End If
                    If IsDBNull(Dt1.Rows(i).Item("Ent_Wgt").ToString) = False Then
                        Ent_Wgt = Val(Dt1.Rows(i).Item("Ent_Wgt").ToString)
                    End If

                    If IsDBNull(Dt1.Rows(i).Item("Ent_Rate").ToString) = False Then
                        Ent_Rate = Val(Dt1.Rows(i).Item("Ent_Rate").ToString)
                    End If
                    SNo = SNo + 1
                    .Rows(n).Cells(0).Value = Val(SNo)
                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Yarn_Purchase_Receipt_No").ToString
                    .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Yarn_Purchase_Receipt_Date").ToString), "dd-MM-yyyy")
                    .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Count_Name").ToString
                    .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Yarn_Type").ToString
                    .Rows(n).Cells(5).Value = (Dt1.Rows(i).Item("Mill_Name").ToString)
                    .Rows(n).Cells(6).Value = (Val(Dt1.Rows(i).Item("Bags").ToString) - Val(Dt1.Rows(i).Item("Purchase_Bags").ToString) + Val(Ent_Bags))
                    .Rows(n).Cells(7).Value = (Val(Dt1.Rows(i).Item("Cones").ToString) - Val(Dt1.Rows(i).Item("Purchase_Cones").ToString) + Val(Ent_Cones))
                    .Rows(n).Cells(8).Value = Format(Val(Dt1.Rows(i).Item("Weight").ToString) - Val(Dt1.Rows(i).Item("Purchase_Weight").ToString) + Val(Ent_Wgt), "#########0.00")

                    If Ent_Wgt > 0 Then
                        .Rows(n).Cells(9).Value = "1"
                        For j = 0 To .ColumnCount - 1
                            .Rows(n).Cells(j).Style.ForeColor = Color.Red
                        Next

                    Else
                        .Rows(n).Cells(9).Value = ""

                    End If

                    .Rows(n).Cells(10).Value = Dt1.Rows(i).Item("DeliveryAt").ToString
                    .Rows(n).Cells(11).Value = Dt1.Rows(i).Item("Yarn_Purchase_Receipt_Code").ToString
                    .Rows(n).Cells(12).Value = Dt1.Rows(i).Item("Yarn_Purchase_Receipt_Details_SlNo").ToString


                    .Rows(n).Cells(13).Value = Val(Ent_Bags)
                    .Rows(n).Cells(14).Value = Ent_Cones
                    .Rows(n).Cells(15).Value = Ent_Wgt
                    .Rows(n).Cells(16).Value = Dt1.Rows(i).Item("Transport").ToString
                    .Rows(n).Cells(17).Value = Dt1.Rows(i).Item("Vechile_No").ToString
                    .Rows(n).Cells(18).Value = Ent_Rate
                    .Rows(n).Cells(19).Value = Common_Procedures.Ledger_IdNoToName(con, Val(Dt1.Rows(i).Item("Agent_IdNo").ToString))
                    '.Rows(n).Cells(21).Value = Val(Dt1.Rows(i).Item("Rate").ToString)
                    '.Rows(n).Cells(22).Value = Ent_Rate
                Next

            End If
            Dt1.Clear()

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
        Cloth_Invoice_Selection()
    End Sub

    Private Sub Cloth_Invoice_Selection()
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim n As Integer = 0
        Dim sno As Integer = 0
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim vRec_Nos As String = ""
        Dim Rec_No As String = ""
        dgv_Details.Rows.Clear()
        vRec_Nos = ""
        For i = 0 To dgv_Selection.RowCount - 1

            If Val(dgv_Selection.Rows(i).Cells(9).Value) = 1 Then



                If Trim(dgv_Selection.Rows(i).Cells(1).Value) <> "" Then
                    vRec_Nos = Trim(vRec_Nos) & IIf(Trim(vRec_Nos) <> "", ", ", "") & Trim(dgv_Selection.Rows(i).Cells(1).Value)
                End If



                txt_RecNo.Text = Trim(vRec_Nos)
                cbo_Delvat.Text = dgv_Selection.Rows(i).Cells(10).Value
                cbo_Transport.Text = dgv_Selection.Rows(i).Cells(16).Value
                cbo_VehicleNo.Text = dgv_Selection.Rows(i).Cells(17).Value
                cbo_Agent.Text = dgv_Selection.Rows(i).Cells(19).Value

                n = dgv_Details.Rows.Add()
                sno = sno + 1
                dgv_Details.Rows(n).Cells(0).Value = Val(sno)
                dgv_Details.Rows(n).Cells(1).Value = dgv_Selection.Rows(i).Cells(3).Value
                dgv_Details.Rows(n).Cells(2).Value = dgv_Selection.Rows(i).Cells(5).Value
                dgv_Details.Rows(n).Cells(3).Value = dgv_Selection.Rows(i).Cells(5).Value

                If Val(dgv_Selection.Rows(i).Cells(13).Value) <> 0 Then
                    dgv_Details.Rows(n).Cells(3).Value = dgv_Selection.Rows(i).Cells(13).Value
                Else
                    dgv_Details.Rows(n).Cells(3).Value = dgv_Selection.Rows(i).Cells(6).Value
                End If
                If Val(dgv_Selection.Rows(i).Cells(14).Value) <> 0 Then
                    dgv_Details.Rows(n).Cells(4).Value = dgv_Selection.Rows(i).Cells(14).Value
                Else
                    dgv_Details.Rows(n).Cells(4).Value = dgv_Selection.Rows(i).Cells(7).Value
                End If

                If Val(dgv_Selection.Rows(i).Cells(15).Value) <> 0 Then
                    dgv_Details.Rows(n).Cells(5).Value = dgv_Selection.Rows(i).Cells(15).Value
                Else
                    dgv_Details.Rows(n).Cells(5).Value = dgv_Selection.Rows(i).Cells(8).Value
                End If

                dgv_Details.Rows(n).Cells(7).Value = dgv_Selection.Rows(i).Cells(18).Value

                dgv_Details.Rows(n).Cells(10).Value = dgv_Selection.Rows(i).Cells(1).Value
                dgv_Details.Rows(n).Cells(11).Value = dgv_Selection.Rows(i).Cells(11).Value
                dgv_Details.Rows(n).Cells(12).Value = dgv_Selection.Rows(i).Cells(12).Value


            End If

        Next

        Total_Calculation()

        pnl_Back.Enabled = True
        pnl_Selection.Visible = False
        If cbo_PurchaseAc.Enabled And cbo_PurchaseAc.Visible Then cbo_PurchaseAc.Focus()

    End Sub

    Private Sub cbo_Type_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Type.TextChanged
        If Trim(UCase(cbo_Type.Text)) = "RECEIPT" Then
            dgv_Details.AllowUserToAddRows = False
            cbo_Delvat.Enabled = False
            cbo_Transport.Enabled = False
            cbo_VehicleNo.Enabled = False
            txt_RecNo.Enabled = False
            cbo_Agent.Enabled = False
            dgv_Details.Columns(1).ReadOnly = True
            dgv_Details.Columns(2).ReadOnly = True
        Else
            dgv_Details.AllowUserToAddRows = True
            cbo_Delvat.Enabled = True
            cbo_Transport.Enabled = True
            cbo_VehicleNo.Enabled = True
            txt_RecNo.Enabled = True
            cbo_Agent.Enabled = True
            dgv_Details.Columns(1).ReadOnly = False
            dgv_Details.Columns(2).ReadOnly = False
        End If
    End Sub

    Private Sub dgtxt_Details_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.TextChanged
        Try
            With dgv_Details

                If .Visible Then
                    If .Rows.Count > 0 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(dgtxt_Details.Text)
                    End If
                End If
            End With

        Catch ex As NullReferenceException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXTBOX TEXTCHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As ObjectDisposedException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXTBOX TEXTCHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXTBOX TEXTCHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub dtp_Date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_Date.KeyPress
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            msk_date.Focus()
        End If
    End Sub

    Private Sub dtp_Date_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_Date.TextChanged
        If IsDate(dtp_Date.Text) = True Then
            msk_date.Text = dtp_Date.Text
            msk_date.SelectionStart = 0
        End If
    End Sub
    Private Sub DeleteAll()
        Dim pwd As String = ""

        If MessageBox.Show("Do you want to Delete All Data's?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

        Dim g As New Password
        g.ShowDialog()

        pwd = Trim(Common_Procedures.Password_Input)

        If Trim(UCase(pwd)) <> "TSDA7417" Then
            MessageBox.Show("Invalid Password", "FAILED...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If


        DeleteAll_STS = True

        LastNo = ""
        movelast_record()

        LastNo = lbl_RefNo.Text

        movefirst_record()
        Timer1.Enabled = True
    End Sub

    Private Sub Yarn_Purchase_GST_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        'vSPEC_KEYS.Add(e.KeyCode)
        If e.Control AndAlso e.Alt AndAlso e.KeyCode = Keys.D Then
            'MessageBox.Show("Shortcut Ctrl + Alt + N activated!")
            DeleteAll()
        End If
    End Sub

    Private Sub Yarn_Purchase_GST_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        'If Control.ModifierKeys AndAlso vSPEC_KEYS.Contains(Keys.A) AndAlso vSPEC_KEYS.Contains(Keys.D) Then
        '    'MessageBox.Show("Ctrl+A or Ctrl+D was pressed!")
        '    DeleteAll()
        'End If

        'vSPEC_KEYS.Remove(e.KeyCode)
        'vSPEC_KEYS.Clear()
    End Sub
End Class