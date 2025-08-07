Public Class Yarn_Test

    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "YNTST-"
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

        pnl_YarnTest.Enabled = True
        pnl_Filter.Visible = False

        lbl_InvoiceNo.Text = ""
        lbl_InvoiceNo.ForeColor = Color.Black

        cbo_MillName1.Text = ""
        cbo_MillName2.Text = ""
        cbo_MillName3.Text = ""

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


        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))


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

    Private Sub move_record(ByVal no As String)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim NewCode As String

        Dim I As Integer = 0

        If Val(no) = 0 Then Exit Sub

        clear()

        NoCalc_Status = True

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.* from Yarn_test_Head a Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Yarn_Test_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_InvoiceNo.Text = dt1.Rows(0).Item("Yarn_Test_No").ToString
                txt_test1No.Text = dt1.Rows(0).Item("Test1_No").ToString
                txt_Test2No.Text = dt1.Rows(0).Item("Test2_No").ToString
                txt_test3No.Text = dt1.Rows(0).Item("Test3_No").ToString
                dtp_Test1Date.Text = dt1.Rows(0).Item("Test1_Date").ToString
                dtp_Test2Date.Text = dt1.Rows(0).Item("Test2_Date").ToString
                dtp_test3date.Text = dt1.Rows(0).Item("Test3_Date").ToString

                cbo_MillName1.Text = Common_Procedures.Mill_IdNoToName(con, Val(dt1.Rows(0).Item("Mill_Idno1").ToString))
                cbo_MillName2.Text = Common_Procedures.Mill_IdNoToName(con, Val(dt1.Rows(0).Item("Mill_Idno2").ToString))
                cbo_MillName3.Text = Common_Procedures.Mill_IdNoToName(con, Val(dt1.Rows(0).Item("Mill_Idno3").ToString))


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

                lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))

            End If


        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            dt1.Dispose()
            da1.Dispose()

            dt2.Dispose()
            da2.Dispose()

            If txt_test1No.Visible And txt_test1No.Enabled Then txt_test1No.Focus()

        End Try

        NoCalc_Status = False



    End Sub

    Private Sub Yarn_Purchase_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Try



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

    Private Sub Yarn_Purchase_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        Me.Text = ""

        con.Open()



        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2
        pnl_Filter.BringToFront()



        btn_UserModification.Visible = False
        If Common_Procedures.settings.User_Modifications_Show_Status = 1 Then
            If Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_UserModification_Status = 1 Then
                btn_UserModification.Visible = True
            End If
        End If
        AddHandler cbo_MillName1.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_MillName2.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_MillName3.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_MillName1.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_MillName2.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_MillName3.LostFocus, AddressOf ControlLostFocus


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

        'AddHandler dtp_Test1Date.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler dtp_Test2Date.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler dtp_test3date.KeyDown, AddressOf TextBoxControlKeyDown

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
        AddHandler txt_Test3_Cone6_YarnWeight5.KeyDown, AddressOf TextBoxControlKeyDown

        'AddHandler dtp_Test1Date.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler dtp_Test2Date.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler dtp_test3date.KeyPress, AddressOf TextBoxControlKeyPress

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
        con.Close()
        con.Dispose()
    End Sub

    Private Sub Yarn_purchase_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    '    btn_Filter_Close_Click(sender, e)
                    '    Exit Sub

                    'ElseIf pnl_Print.Visible = True Then
                    '    btn_print_Close_Click(sender, e)
                    '    Exit Sub

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

    Public Sub print_record() Implements Interface_MDIActions.print_record
    End Sub
    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim vOrdByNo As String = ""

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text)

        If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Yarn_Sales_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Yarn_Sales_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If pnl_YarnTest.Enabled = False Then
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
        ' Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "DELETE", "Yarn_test_Head", "Yarn_test_Code", Val(lbl_Company.Tag), NewCode, lbl_InvoiceNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "", "", "Yarn_test_Code, Company_IdNo, for_OrderBy", trans)


        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.Transaction = trans


            cmd.CommandText = "delete from Yarn_test_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Test_Code = '" & Trim(NewCode) & "'"
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

            If txt_test1No.Enabled = True And txt_test1No.Visible = True Then txt_test1No.Focus()

        End Try

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record


    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String

        Try

            da = New SqlClient.SqlDataAdapter("select top 1 Yarn_Test_No from Yarn_test_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Test_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Yarn_Test_No", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 Yarn_Test_No from Yarn_test_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Test_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Yarn_Test_No", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 Yarn_Test_No from Yarn_test_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Test_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Yarn_Test_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 Yarn_Test_No from Yarn_test_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Test_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Yarn_Test_No desc", con)
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

            lbl_InvoiceNo.Text = Common_Procedures.get_MaxCode(con, "Yarn_test_Head", "Yarn_Test_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)
            lbl_InvoiceNo.ForeColor = Color.Red


        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            Dt1.Dispose()
            Dt2.Dispose()
            Da1.Dispose()

            If txt_test1No.Enabled And txt_test1No.Visible Then txt_test1No.Focus()

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

            Da = New SqlClient.SqlDataAdapter("select Yarn_Test_No from Yarn_test_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Test_Code = '" & Trim(RefCode) & "'", con)
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

        '  If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Yarn_Sales_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Yarn_Sales_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub


        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.Yarn_test_Entry, New_Entry, Me) = False Then Exit Sub


        Try

            inpno = InputBox("Enter New Invoice No.", "FOR NEW Ref NO. INSERTION...")

            InvCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Yarn_Test_No from Yarn_test_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Test_Code = '" & Trim(InvCode) & "'", con)
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
        Dim mill1_Idno As Integer = 0
        Dim mill2_Idno As Integer = 0
        Dim mill3_Idno As Integer = 0
        Dim Usr_ID As Integer = 0
        Dim vOrdByNo As String = ""

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text)

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        'If Common_Procedures.UserRight_Check(Common_Procedures.UR.Yarn_Sales_Entry, New_Entry) = False Then Exit Sub

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Yarn_test_Entry, New_Entry, Me, con, "Yarn_test_Head", "Yarn_test_Code", NewCode, "Test_Date1", "(Yarn_test_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Yarn_test_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, Yarn_test_No desc", dtp_Test1Date.Value.Date) = False Then Exit Sub


        If pnl_YarnTest.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        mill1_Idno = Common_Procedures.Mill_NameToIdNo(con, cbo_MillName1.Text)
        mill2_Idno = Common_Procedures.Mill_NameToIdNo(con, cbo_MillName2.Text)
        mill3_Idno = Common_Procedures.Mill_NameToIdNo(con, cbo_MillName3.Text)

        'If IsDate(msk_date.Text) = False Then
        '    MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()
        '    Exit Sub
        'End If

        'If Not (Convert.ToDateTime(msk_date.Text) >= Common_Procedures.Company_FromDate And Convert.ToDateTime(msk_date.Text) <= Common_Procedures.Company_ToDate) Then
        '    MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()
        '    Exit Sub
        'End If

        lbl_UserName.Text = Common_Procedures.User.IdNo



        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_InvoiceNo.Text = Common_Procedures.get_MaxCode(con, "Yarn_test_Head", "Yarn_Test_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@test1Date", dtp_Test1Date.Value.Date)
            ' cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@test2Date", dtp_Test2Date.Value.Date)

            ' cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@test3Date", dtp_test3date.Value.Date)


            If New_Entry = True Then
                cmd.CommandText = "Insert into Yarn_test_Head (       Yarn_Test_Code ,               Company_IdNo       ,           Yarn_Test_No    ,                               for_OrderBy                              ,                           Test1_Date     , Test2_Date   , Test3_Date  ,         Test1_No                   ,           Test2_No           ,           Test3_No       ,      Test1_Cone1_YarnWeight1             ,                     Test1_Cone1_YarnWeight2      ,          Test1_Cone1_YarnWeight3                  ,    Test1_Cone1_YarnWeight4                     ,   Test1_Cone1_YarnWeight5                      ,   Test1_Cone1_YarnCount1            ,          Test1_Cone1_YarnCount2                    ,        Test1_Cone1_YarnCount3                  ,    Test1_Cone1_YarnCount4                          ,              Test1_Cone1_YarnCount5            ,               Test1_Cone2_YarnWeight1             ,                     Test1_Cone2_YarnWeight2      ,          Test1_Cone2_YarnWeight3                  ,    Test1_Cone2_YarnWeight4                     ,   Test1_Cone2_YarnWeight5                      ,   Test1_Cone2_YarnCount1            ,          Test1_Cone2_YarnCount2                    ,        Test1_Cone2_YarnCount3                  ,    Test1_Cone2_YarnCount4                          ,              Test1_Cone2_YarnCount5            ,          Test1_Cone3_YarnWeight1             ,                     Test1_Cone3_YarnWeight2      ,          Test1_Cone3_YarnWeight3                  ,    Test1_Cone3_YarnWeight4                     ,   Test1_Cone3_YarnWeight5                      ,                   Test1_Cone3_YarnCount1            ,          Test1_Cone3_YarnCount2                    ,        Test1_Cone3_YarnCount3                  ,    Test1_Cone3_YarnCount4                          ,              Test1_Cone3_YarnCount5            ,                  Test1_Cone4_YarnWeight1             ,                     Test1_Cone4_YarnWeight2      ,          Test1_Cone4_YarnWeight3                  ,    Test1_Cone4_YarnWeight4                     ,   Test1_Cone4_YarnWeight5                      ,   Test1_Cone4_YarnCount1            ,          Test1_Cone4_YarnCount2                    ,        Test1_Cone4_YarnCount3                  ,    Test1_Cone4_YarnCount4                          ,              Test1_Cone4_YarnCount5            ,         Test1_Cone5_YarnWeight1             ,                     Test1_Cone5_YarnWeight2      ,          Test1_Cone5_YarnWeight3                  ,    Test1_Cone5_YarnWeight4                     ,   Test1_Cone5_YarnWeight5                      ,   Test1_Cone5_YarnCount1            ,          Test1_Cone5_YarnCount2                    ,        Test1_Cone5_YarnCount3                  ,    Test1_Cone5_YarnCount4                          ,              Test1_Cone5_YarnCount5            ,           Test1_Cone6_YarnWeight1             ,                     Test1_Cone6_YarnWeight2      ,          Test1_Cone6_YarnWeight3                  ,    Test1_Cone6_YarnWeight4                     ,   Test1_Cone6_YarnWeight5                      ,   Test1_Cone6_YarnCount1            ,          Test1_Cone6_YarnCount2                    ,        Test1_Cone6_YarnCount3                  ,    Test1_Cone6_YarnCount4                     ,              Test1_Cone6_YarnCount5           ,                                  Test2_Cone1_YarnWeight1             ,                     Test2_Cone1_YarnWeight2      ,          Test2_Cone1_YarnWeight3                  ,    Test2_Cone1_YarnWeight4                     ,   Test2_Cone1_YarnWeight5                      ,   Test2_Cone1_YarnCount1            ,          Test2_Cone1_YarnCount2                    ,        Test2_Cone1_YarnCount3                  ,    Test2_Cone1_YarnCount4                          ,              Test2_Cone1_YarnCount5            ,               Test2_Cone2_YarnWeight1             ,                     Test2_Cone2_YarnWeight2      ,          Test2_Cone2_YarnWeight3                  ,    Test2_Cone2_YarnWeight4                     ,   Test2_Cone2_YarnWeight5                      ,                         Test2_Cone2_YarnCount1            ,          Test2_Cone2_YarnCount2                    ,        Test2_Cone2_YarnCount3                  ,    Test2_Cone2_YarnCount4                          ,              Test2_Cone2_YarnCount5            ,          Test2_Cone3_YarnWeight1             ,                     Test2_Cone3_YarnWeight2      ,          Test2_Cone3_YarnWeight3                  ,    Test2_Cone3_YarnWeight4                     ,   Test2_Cone3_YarnWeight5                      ,   Test2_Cone3_YarnCount1            ,          Test2_Cone3_YarnCount2                    ,        Test2_Cone3_YarnCount3                  ,    Test2_Cone3_YarnCount4                          ,              Test2_Cone3_YarnCount5            ,                  Test2_Cone4_YarnWeight1             ,                     Test2_Cone4_YarnWeight2      ,          Test2_Cone4_YarnWeight3                  ,    Test2_Cone4_YarnWeight4                     ,   Test2_Cone4_YarnWeight5                      ,   Test2_Cone4_YarnCount1            ,          Test2_Cone4_YarnCount2                    ,        Test2_Cone4_YarnCount3                  ,    Test2_Cone4_YarnCount4                          ,              Test2_Cone4_YarnCount5            ,         Test2_Cone5_YarnWeight1             ,                     Test2_Cone5_YarnWeight2      ,          Test2_Cone5_YarnWeight3                  ,    Test2_Cone5_YarnWeight4                     ,   Test2_Cone5_YarnWeight5                      ,   Test2_Cone5_YarnCount1            ,          Test2_Cone5_YarnCount2                    ,        Test2_Cone5_YarnCount3                  ,    Test2_Cone5_YarnCount4                          ,              Test2_Cone5_YarnCount5            ,           Test2_Cone6_YarnWeight1             ,                     Test2_Cone6_YarnWeight2      ,          Test2_Cone6_YarnWeight3                  ,    Test2_Cone6_YarnWeight4                     ,   Test2_Cone6_YarnWeight5                      ,   Test2_Cone6_YarnCount1            ,          Test2_Cone6_YarnCount2                    ,        Test2_Cone6_YarnCount3                  ,    Test2_Cone6_YarnCount4                          ,              Test2_Cone6_YarnCount5           ,                         Test3_Cone1_YarnWeight1             ,                     Test3_Cone1_YarnWeight2      ,          Test3_Cone1_YarnWeight3                  ,    Test3_Cone1_YarnWeight4                     ,   Test3_Cone1_YarnWeight5                      ,   Test3_Cone1_YarnCount1            ,          Test3_Cone1_YarnCount2                    ,                 Test3_Cone1_YarnCount3                  ,    Test3_Cone1_YarnCount4                          ,              Test3_Cone1_YarnCount5            ,               Test3_Cone2_YarnWeight1             ,                     Test3_Cone2_YarnWeight2      ,          Test3_Cone2_YarnWeight3                  ,    Test3_Cone2_YarnWeight4                     ,   Test3_Cone2_YarnWeight5                      ,   Test3_Cone2_YarnCount1            ,          Test3_Cone2_YarnCount2                    ,        Test3_Cone2_YarnCount3                  ,    Test3_Cone2_YarnCount4                          ,              Test3_Cone2_YarnCount5            ,          Test3_Cone3_YarnWeight1             ,                     Test3_Cone3_YarnWeight2      ,          Test3_Cone3_YarnWeight3                  ,    Test3_Cone3_YarnWeight4                     ,   Test3_Cone3_YarnWeight5                      ,          Test3_Cone3_YarnCount1            ,          Test3_Cone3_YarnCount2                    ,        Test3_Cone3_YarnCount3                  ,    Test3_Cone3_YarnCount4                          ,              Test3_Cone3_YarnCount5            ,                  Test3_Cone4_YarnWeight1             ,                     Test3_Cone4_YarnWeight2      ,          Test3_Cone4_YarnWeight3                  ,    Test3_Cone4_YarnWeight4                     ,   Test3_Cone4_YarnWeight5                      ,   Test3_Cone4_YarnCount1            ,          Test3_Cone4_YarnCount2                    ,        Test3_Cone4_YarnCount3                  ,    Test3_Cone4_YarnCount4                          ,              Test3_Cone4_YarnCount5            ,         Test3_Cone5_YarnWeight1             ,                     Test3_Cone5_YarnWeight2      ,          Test3_Cone5_YarnWeight3                  ,    Test3_Cone5_YarnWeight4                     ,   Test3_Cone5_YarnWeight5                      ,   Test3_Cone5_YarnCount1            ,          Test3_Cone5_YarnCount2                    ,        Test3_Cone5_YarnCount3                  ,    Test3_Cone5_YarnCount4                          ,              Test3_Cone5_YarnCount5            ,           Test3_Cone6_YarnWeight1             ,                     Test3_Cone6_YarnWeight2      ,          Test3_Cone6_YarnWeight3                  ,    Test3_Cone6_YarnWeight4                     ,   Test3_Cone6_YarnWeight5                      ,   Test3_Cone6_YarnCount1            ,          Test3_Cone6_YarnCount2                    ,        Test3_Cone6_YarnCount3                  ,    Test3_Cone6_YarnCount4                          ,              Test3_Cone6_YarnCount5            ,          Test1_Cone1_Mean                  ,    Test1_Cone1_Cv                          ,              Test1_Cone1_SD            ,          Test1_Cone2_Mean                  ,    Test1_Cone2_Cv                          ,              Test1_Cone2_SD            ,       Test1_Cone3_Mean                  ,    Test1_Cone3_Cv                          ,              Test1_Cone3_SD            ,            Test1_Cone4_Mean                  ,    Test1_Cone4_Cv                          ,              Test1_Cone4_SD            ,        Test1_Cone5_Mean                  ,    Test1_Cone5_Cv                          ,              Test1_Cone5_SD                 ,        Test1_Cone6_Mean                  ,    Test1_Cone6_Cv                          ,              Test1_Cone6_SD            ,        Test1_Avg_Cv                 ,    Test1_Avg_Sd                          ,              Test1_Avg_Coul             ,         Test2_Cone1_Mean                  ,    Test2_Cone1_Cv                          ,              Test2_Cone1_SD            ,          Test2_Cone2_Mean                  ,    Test2_Cone2_Cv                          ,              Test2_Cone2_SD            ,       Test2_Cone3_Mean                  ,    Test2_Cone3_Cv                          ,              Test2_Cone3_SD            ,            Test2_Cone4_Mean                  ,    Test2_Cone4_Cv                          ,              Test2_Cone4_SD            ,        Test2_Cone5_Mean                  ,    Test2_Cone5_Cv                          ,              Test2_Cone5_SD                 ,        Test2_Cone6_Mean                  ,    Test2_Cone6_Cv                          ,              Test2_Cone6_SD            ,  Test2_Avg_Cv                 ,    Test2_Avg_Sd                          ,              Test2_Avg_Coul                  ,          Test3_Cone1_Mean                  ,    Test3_Cone1_Cv                          ,              Test3_Cone1_SD            ,          Test3_Cone2_Mean                  ,    Test3_Cone2_Cv                          ,              Test3_Cone2_SD            ,       Test3_Cone3_Mean                  ,    Test3_Cone3_Cv                          ,              Test3_Cone3_SD            ,            Test3_Cone4_Mean                  ,    Test3_Cone4_Cv                          ,              Test3_Cone4_SD            ,        Test3_Cone5_Mean                  ,    Test3_Cone5_Cv                          ,              Test3_Cone5_SD                 ,        Test3_Cone6_Mean                  ,    Test3_Cone6_Cv                          ,              Test3_Cone6_SD            ,  Test3_Avg_Cv                 ,    Test3_Avg_Sd                          ,              Test3_Avg_Coul            ,                        Mill_Idno1                ,Mill_Idno2            ,Mill_Idno3               ,User_idNo ) " & _
                                    "     Values                  (   '" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ",      @test1Date    ,    @test2Date  ,  @test3Date  ,'" & Trim(txt_test1No.Text) & "','" & Trim(txt_Test2No.Text) & "','" & Trim(txt_test3No.Text) & "'," & Str(Val(txt_Test1_Cone1_YarnWeight1.Text)) & ", " & Str(Val(txt_Test1_Cone1_YarnWeight2.Text)) & ", " & Val(txt_Test1_Cone1_YarnWeight3.Text) & ",  " & Val(txt_Test1_Cone1_YarnWeight4.Text) & ",  " & Val(txt_Test1_Cone1_YarnWeight5.Text) & ",  " & Val(lbl_Test1_Cone1_YarnCount1.Text) & "," & Str(Val(lbl_Test1_Cone1_YarnCount2.Text)) & ", " & Str(Val(lbl_Test1_Cone1_YarnCount3.Text)) & ", " & Str(Val(lbl_Test1_Cone1_YarnCount4.Text)) & ", " & Str(Val(lbl_Test1_Cone1_YarnCount5.Text)) & ",  " & Str(Val(txt_Test1_Cone2_YarnWeight1.Text)) & ", " & Str(Val(txt_Test1_Cone2_YarnWeight2.Text)) & ", " & Val(txt_Test1_Cone2_YarnWeight3.Text) & ",  " & Val(txt_Test1_Cone2_YarnWeight4.Text) & ",  " & Val(txt_Test1_Cone2_YarnWeight5.Text) & ",  " & Val(lbl_Test1_Cone2_YarnCount1.Text) & "," & Str(Val(lbl_Test1_Cone2_YarnCount2.Text)) & ", " & Str(Val(lbl_Test1_Cone2_YarnCount3.Text)) & ", " & Str(Val(lbl_Test1_Cone2_YarnCount4.Text)) & ", " & Str(Val(lbl_Test1_Cone2_YarnCount5.Text)) & "," & Str(Val(txt_Test1_Cone3_YarnWeight1.Text)) & ", " & Str(Val(txt_Test1_Cone3_YarnWeight2.Text)) & ", " & Val(txt_Test1_Cone3_YarnWeight3.Text) & ",  " & Val(txt_Test1_Cone3_YarnWeight4.Text) & ",  " & Val(txt_Test1_Cone3_YarnWeight5.Text) & ",  " & Val(lbl_Test1_Cone3_YarnCount1.Text) & "," & Str(Val(lbl_Test1_Cone3_YarnCount2.Text)) & ", " & Str(Val(lbl_Test1_Cone3_YarnCount3.Text)) & ", " & Str(Val(lbl_Test1_Cone3_YarnCount4.Text)) & ", " & Str(Val(lbl_Test1_Cone3_YarnCount5.Text)) & ", " & Str(Val(txt_Test1_Cone4_YarnWeight1.Text)) & ", " & Str(Val(txt_Test1_Cone4_YarnWeight2.Text)) & ", " & Val(txt_Test1_Cone4_YarnWeight3.Text) & ",  " & Val(txt_Test1_Cone4_YarnWeight4.Text) & ",  " & Val(txt_Test1_Cone4_YarnWeight5.Text) & ",  " & Val(lbl_Test1_Cone4_YarnCount1.Text) & "," & Str(Val(lbl_Test1_Cone4_YarnCount2.Text)) & ", " & Str(Val(lbl_Test1_Cone4_YarnCount3.Text)) & ", " & Str(Val(lbl_Test1_Cone4_YarnCount4.Text)) & ", " & Str(Val(lbl_Test1_Cone4_YarnCount5.Text)) & "," & Str(Val(txt_Test1_Cone5_YarnWeight1.Text)) & ", " & Str(Val(txt_Test1_Cone5_YarnWeight2.Text)) & ", " & Val(txt_Test1_Cone5_YarnWeight3.Text) & ",  " & Val(txt_Test1_Cone5_YarnWeight4.Text) & ",  " & Val(txt_Test1_Cone5_YarnWeight5.Text) & ",  " & Val(lbl_Test1_Cone5_YarnCount1.Text) & "," & Str(Val(lbl_Test1_Cone5_YarnCount2.Text)) & ", " & Str(Val(lbl_Test1_Cone5_YarnCount3.Text)) & ", " & Str(Val(lbl_Test1_Cone5_YarnCount4.Text)) & ", " & Str(Val(lbl_Test1_Cone5_YarnCount5.Text)) & "," & Str(Val(txt_Test1_Cone6_YarnWeight1.Text)) & ", " & Str(Val(txt_Test1_Cone6_YarnWeight2.Text)) & ", " & Val(txt_Test1_Cone6_YarnWeight3.Text) & ",  " & Val(txt_Test1_Cone6_YarnWeight4.Text) & ",  " & Val(txt_Test1_Cone6_YarnWeight5.Text) & ",  " & Val(lbl_Test1_Cone6_YarnCount1.Text) & "," & Str(Val(lbl_Test1_Cone6_YarnCount2.Text)) & ", " & Str(Val(lbl_Test1_Cone6_YarnCount3.Text)) & ", " & Str(Val(lbl_Test1_Cone6_YarnCount4.Text)) & ", " & Str(Val(lbl_Test1_Cone6_YarnCount5.Text)) & ", " & Str(Val(txt_Test2_Cone1_YarnWeight1.Text)) & ", " & Str(Val(txt_Test2_Cone1_YarnWeight2.Text)) & ", " & Val(txt_Test2_Cone1_YarnWeight3.Text) & ",  " & Val(txt_Test2_Cone1_YarnWeight4.Text) & ",  " & Val(txt_Test2_Cone1_YarnWeight5.Text) & ",  " & Val(lbl_Test2_Cone1_YarnCount1.Text) & "," & Str(Val(lbl_Test2_Cone1_YarnCount2.Text)) & ", " & Str(Val(lbl_Test2_Cone1_YarnCount3.Text)) & ", " & Str(Val(lbl_Test2_Cone1_YarnCount4.Text)) & ", " & Str(Val(lbl_Test2_Cone1_YarnCount5.Text)) & ",  " & Str(Val(txt_Test2_Cone2_YarnWeight1.Text)) & ", " & Str(Val(txt_Test2_Cone2_YarnWeight2.Text)) & ", " & Val(txt_Test2_Cone2_YarnWeight3.Text) & ",  " & Val(txt_Test2_Cone2_YarnWeight4.Text) & ",  " & Val(txt_Test2_Cone2_YarnWeight5.Text) & ",  " & Val(lbl_Test2_Cone2_YarnCount1.Text) & "," & Str(Val(lbl_Test2_Cone2_YarnCount2.Text)) & ", " & Str(Val(lbl_Test2_Cone2_YarnCount3.Text)) & ", " & Str(Val(lbl_Test2_Cone2_YarnCount4.Text)) & ", " & Str(Val(lbl_Test2_Cone2_YarnCount5.Text)) & "," & Str(Val(txt_Test2_Cone3_YarnWeight1.Text)) & ", " & Str(Val(txt_Test2_Cone3_YarnWeight2.Text)) & ", " & Val(txt_Test2_Cone3_YarnWeight3.Text) & ",  " & Val(txt_Test2_Cone3_YarnWeight4.Text) & ",  " & Val(txt_Test2_Cone3_YarnWeight5.Text) & ",  " & Val(lbl_Test2_Cone3_YarnCount1.Text) & "," & Str(Val(lbl_Test2_Cone3_YarnCount2.Text)) & ", " & Str(Val(lbl_Test2_Cone3_YarnCount3.Text)) & ", " & Str(Val(lbl_Test2_Cone3_YarnCount4.Text)) & ", " & Str(Val(lbl_Test2_Cone3_YarnCount5.Text)) & ", " & Str(Val(txt_Test2_Cone4_YarnWeight1.Text)) & ", " & Str(Val(txt_Test2_Cone4_YarnWeight2.Text)) & ", " & Val(txt_Test2_Cone4_YarnWeight3.Text) & ",  " & Val(txt_Test2_Cone4_YarnWeight4.Text) & ",  " & Val(txt_Test2_Cone4_YarnWeight5.Text) & ",  " & Val(lbl_Test2_Cone4_YarnCount1.Text) & "," & Str(Val(lbl_Test2_Cone4_YarnCount2.Text)) & ", " & Str(Val(lbl_Test2_Cone4_YarnCount3.Text)) & ", " & Str(Val(lbl_Test2_Cone4_YarnCount4.Text)) & ", " & Str(Val(lbl_Test2_Cone4_YarnCount5.Text)) & "," & Str(Val(txt_Test2_Cone5_YarnWeight1.Text)) & ", " & Str(Val(txt_Test2_Cone5_YarnWeight2.Text)) & ", " & Val(txt_Test2_Cone5_YarnWeight3.Text) & ",  " & Val(txt_Test2_Cone5_YarnWeight4.Text) & ",  " & Val(txt_Test2_Cone5_YarnWeight5.Text) & ",  " & Val(lbl_Test2_Cone5_YarnCount1.Text) & "," & Str(Val(lbl_Test2_Cone5_YarnCount2.Text)) & ", " & Str(Val(lbl_Test2_Cone5_YarnCount3.Text)) & ", " & Str(Val(lbl_Test2_Cone5_YarnCount4.Text)) & ", " & Str(Val(lbl_Test2_Cone5_YarnCount5.Text)) & "," & Str(Val(txt_Test2_Cone6_YarnWeight1.Text)) & ", " & Str(Val(txt_Test2_Cone6_YarnWeight2.Text)) & ", " & Val(txt_Test2_Cone6_YarnWeight3.Text) & ",  " & Val(txt_Test2_Cone6_YarnWeight4.Text) & ",  " & Val(txt_Test2_Cone6_YarnWeight5.Text) & ",  " & Val(lbl_Test2_Cone6_YarnCount1.Text) & "," & Str(Val(lbl_Test2_Cone6_YarnCount2.Text)) & ", " & Str(Val(lbl_Test2_Cone6_YarnCount3.Text)) & ", " & Str(Val(lbl_Test2_Cone6_YarnCount4.Text)) & ", " & Str(Val(lbl_Test2_Cone6_YarnCount5.Text)) & ", " & Str(Val(txt_Test3_Cone1_YarnWeight1.Text)) & ", " & Str(Val(txt_Test3_Cone1_YarnWeight2.Text)) & ", " & Val(txt_Test3_Cone1_YarnWeight3.Text) & ",  " & Val(txt_Test3_Cone1_YarnWeight4.Text) & ",  " & Val(txt_Test3_Cone1_YarnWeight5.Text) & ",  " & Val(lbl_Test3_Cone1_YarnCount1.Text) & "," & Str(Val(lbl_Test3_Cone1_YarnCount2.Text)) & ", " & Str(Val(lbl_Test3_Cone1_YarnCount3.Text)) & ", " & Str(Val(lbl_Test3_Cone1_YarnCount4.Text)) & ", " & Str(Val(lbl_Test3_Cone1_YarnCount5.Text)) & ",  " & Str(Val(txt_Test3_Cone2_YarnWeight1.Text)) & ", " & Str(Val(txt_Test3_Cone2_YarnWeight2.Text)) & ", " & Val(txt_Test3_Cone2_YarnWeight3.Text) & ",  " & Val(txt_Test3_Cone2_YarnWeight4.Text) & ",  " & Val(txt_Test3_Cone2_YarnWeight5.Text) & ",  " & Val(lbl_Test3_Cone2_YarnCount1.Text) & "," & Str(Val(lbl_Test3_Cone2_YarnCount2.Text)) & ", " & Str(Val(lbl_Test3_Cone2_YarnCount3.Text)) & ", " & Str(Val(lbl_Test3_Cone2_YarnCount4.Text)) & ", " & Str(Val(lbl_Test3_Cone2_YarnCount5.Text)) & "," & Str(Val(txt_Test3_Cone3_YarnWeight1.Text)) & ", " & Str(Val(txt_Test3_Cone3_YarnWeight2.Text)) & ", " & Val(txt_Test3_Cone3_YarnWeight3.Text) & ",  " & Val(txt_Test3_Cone3_YarnWeight4.Text) & ",  " & Val(txt_Test3_Cone3_YarnWeight5.Text) & ",  " & Val(lbl_Test3_Cone3_YarnCount1.Text) & "," & Str(Val(lbl_Test3_Cone3_YarnCount2.Text)) & ", " & Str(Val(lbl_Test3_Cone3_YarnCount3.Text)) & ", " & Str(Val(lbl_Test3_Cone3_YarnCount4.Text)) & ", " & Str(Val(lbl_Test3_Cone3_YarnCount5.Text)) & ", " & Str(Val(txt_Test3_Cone4_YarnWeight1.Text)) & ", " & Str(Val(txt_Test3_Cone4_YarnWeight2.Text)) & ", " & Val(txt_Test3_Cone4_YarnWeight3.Text) & ",  " & Val(txt_Test3_Cone4_YarnWeight4.Text) & ",  " & Val(txt_Test3_Cone4_YarnWeight5.Text) & ",  " & Val(lbl_Test3_Cone4_YarnCount1.Text) & "," & Str(Val(lbl_Test3_Cone4_YarnCount2.Text)) & ", " & Str(Val(lbl_Test3_Cone4_YarnCount3.Text)) & ", " & Str(Val(lbl_Test3_Cone4_YarnCount4.Text)) & ", " & Str(Val(lbl_Test3_Cone4_YarnCount5.Text)) & "," & Str(Val(txt_Test3_Cone5_YarnWeight1.Text)) & ", " & Str(Val(txt_Test3_Cone5_YarnWeight2.Text)) & ", " & Val(txt_Test3_Cone5_YarnWeight3.Text) & ",  " & Val(txt_Test3_Cone5_YarnWeight4.Text) & ",  " & Val(txt_Test3_Cone5_YarnWeight5.Text) & ",  " & Val(lbl_Test3_Cone5_YarnCount1.Text) & "," & Str(Val(lbl_Test3_Cone5_YarnCount2.Text)) & ", " & Str(Val(lbl_Test3_Cone5_YarnCount3.Text)) & ", " & Str(Val(lbl_Test3_Cone5_YarnCount4.Text)) & ", " & Str(Val(lbl_Test3_Cone5_YarnCount5.Text)) & "," & Str(Val(txt_Test3_Cone6_YarnWeight1.Text)) & ", " & Str(Val(txt_Test3_Cone6_YarnWeight2.Text)) & ", " & Val(txt_Test3_Cone6_YarnWeight3.Text) & ",  " & Val(txt_Test3_Cone6_YarnWeight4.Text) & ",  " & Val(txt_Test3_Cone6_YarnWeight5.Text) & ",  " & Val(lbl_Test3_Cone6_YarnCount1.Text) & "," & Str(Val(lbl_Test3_Cone6_YarnCount2.Text)) & ", " & Str(Val(lbl_Test3_Cone6_YarnCount3.Text)) & ", " & Str(Val(lbl_Test3_Cone6_YarnCount4.Text)) & ", " & Str(Val(lbl_Test3_Cone6_YarnCount5.Text)) & ",  " & Str(Val(lbl_Test1_Cone1_Mean.Text)) & ", " & Str(Val(lbl_Test1_Cone1_CV.Text)) & ", " & Str(Val(lbl_Test1_Cone1_SD.Text)) & ",  " & Str(Val(lbl_Test1_Cone2_mean.Text)) & ", " & Str(Val(lbl_Test1_Cone2_Cv.Text)) & ", " & Str(Val(lbl_Test1_Cone2_SD.Text)) & ", " & Str(Val(lbl_Test1_Cone3_mean.Text)) & ", " & Str(Val(lbl_Test1_Cone3_Cv.Text)) & ", " & Str(Val(lbl_Test1_Cone3_SD.Text)) & ", " & Str(Val(lbl_Test1_Cone4_mean.Text)) & ", " & Str(Val(lbl_Test1_Cone4_Cv.Text)) & ", " & Str(Val(lbl_Test1_Cone4_SD.Text)) & ", " & Str(Val(lbl_Test1_Cone5_mean.Text)) & ", " & Str(Val(lbl_Test1_Cone5_Cv.Text)) & ", " & Str(Val(lbl_Test1_Cone5_SD.Text)) & ", " & Str(Val(lbl_Test1_Cone6_mean.Text)) & ", " & Str(Val(lbl_Test1_Cone6_Cv.Text)) & ", " & Str(Val(lbl_Test1_Cone6_Sd.Text)) & "," & Str(Val(lbl_Test1_AvgCv.Text)) & ", " & Str(Val(lbl_test1_AvgSd.Text)) & ", " & Str(Val(lbl_Test1_AvgCoul.Text)) & "," & Str(Val(lbl_Test2_Cone1_Mean.Text)) & ", " & Str(Val(lbl_Test2_Cone1_CV.Text)) & ", " & Str(Val(lbl_Test2_Cone1_SD.Text)) & ",  " & Str(Val(lbl_Test2_Cone2_Mean.Text)) & ", " & Str(Val(lbl_Test2_Cone2_CV.Text)) & ", " & Str(Val(lbl_Test2_Cone2_SD.Text)) & ", " & Str(Val(lbl_Test2_Cone3_Mean.Text)) & ", " & Str(Val(lbl_Test2_Cone3_CV.Text)) & ", " & Str(Val(lbl_Test2_Cone3_SD.Text)) & ", " & Str(Val(lbl_Test2_Cone4_Mean.Text)) & ", " & Str(Val(lbl_Test2_Cone4_CV.Text)) & ", " & Str(Val(lbl_Test2_Cone4_SD.Text)) & ", " & Str(Val(lbl_Test2_Cone5_Mean.Text)) & ", " & Str(Val(lbl_Test2_Cone5_CV.Text)) & ", " & Str(Val(lbl_Test2_Cone5_SD.Text)) & ", " & Str(Val(lbl_Test2_Cone6_Mean.Text)) & ", " & Str(Val(lbl_Test2_Cone6_CV.Text)) & ", " & Str(Val(lbl_Test2_Cone6_SD.Text)) & "," & Str(Val(lbl_Test2_AvgCv.Text)) & ", " & Str(Val(lbl_Test2_Avg_Sd.Text)) & ", " & Str(Val(lbl_Test2_avgCoul.Text)) & "," & Str(Val(lbl_Test3_Cone1_Mean.Text)) & ", " & Str(Val(lbl_Test3_Cone1_CV.Text)) & ", " & Str(Val(lbl_Test3_Cone1_SD.Text)) & ",  " & Str(Val(lbl_Test3_Cone2_mean.Text)) & ", " & Str(Val(lbl_Test3_Cone2_CV.Text)) & ", " & Str(Val(lbl_Test3_Cone2_SD.Text)) & ", " & Str(Val(lbl_Test3_Cone3_mean.Text)) & ", " & Str(Val(lbl_Test3_Cone3_CV.Text)) & ", " & Str(Val(lbl_Test3_Cone3_SD.Text)) & ", " & Str(Val(lbl_Test3_Cone4_mean.Text)) & ", " & Str(Val(lbl_Test3_Cone4_CV.Text)) & ", " & Str(Val(lbl_Test3_Cone4_SD.Text)) & ", " & Str(Val(lbl_Test3_Cone5_Mean.Text)) & ", " & Str(Val(lbl_Test3_Cone5_CV.Text)) & ", " & Str(Val(lbl_Test3_Cone5_SD.Text)) & ", " & Str(Val(lbl_Test3_Cone6_Mean.Text)) & ", " & Str(Val(lbl_Test3_Cone6_CV.Text)) & ", " & Str(Val(lbl_Test3_Cone6_SD.Text)) & "," & Str(Val(lbl_Test3_AvgCv.Text)) & ", " & Str(Val(lbl_Test3_AvgSd.Text)) & ", " & Str(Val(lbl_Test3_AvgCoul.Text)) & ", " & Str(Val(mill1_Idno)) & ", " & Str(Val(mill2_Idno)) & ", " & Str(Val(mill3_Idno)) & ", " & Val(lbl_UserName.Text) & ") "
                cmd.ExecuteNonQuery()

            Else

                ' Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "OLD", "Yarn_test_Head", "Yarn_test_Code", Val(lbl_Company.Tag), NewCode, lbl_InvoiceNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Yarn_test_Code, Company_IdNo, for_OrderBy", tr)
                '  Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "OLD", "Yarn_Sales_Prroforma_Details", "Yarn_Sales_Code", Val(lbl_Company.Tag), NewCode, lbl_InvoiceNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "Count_IdNo,Mill_IdNo,Bags,Cones,Weight,Rate_For,Rate,Amount,Bag_No,Delivery_No,Weaver_Sales_Yarn_Delivery_Code,Weaver_Sales_Yarn_Delivery_Detail_SlNo", "Sl_No", "Yarn_Sales_Code, For_OrderBy, Company_IdNo, Yarn_Sales_No,Yarn_Sales_Date, Ledger_Idno", tr)

                cmd.CommandText = "Update Yarn_test_Head set Test1_Date = @test1Date, Test2_Date = @test2Date, Test3_Date = @test3Date, Test1_No = '" & Trim(txt_test1No.Text) & "', Test2_No = '" & Trim(txt_Test2No.Text) & "', Test3_No = '" & Trim(txt_test3No.Text) & "',Test1_Cone1_YarnWeight1 = " & Str(Val(txt_Test1_Cone1_YarnWeight1.Text)) & "   , Test1_Cone1_YarnWeight2   = " & Str(Val(txt_Test1_Cone1_YarnWeight2.Text)) & "   ,   Test1_Cone1_YarnWeight3 = " & Str(Val(txt_Test1_Cone1_YarnWeight3.Text)) & "     ,    Test1_Cone1_YarnWeight4 =" & Str(Val(txt_Test1_Cone1_YarnWeight4.Text)) & "    ,   Test1_Cone1_YarnWeight5   =" & Str(Val(txt_Test1_Cone1_YarnWeight5.Text)) & "   ,   Test1_Cone1_YarnCount1 =  " & Val(lbl_Test1_Cone1_YarnCount1.Text) & " ,  Test1_Cone1_YarnCount2   =  " & Val(lbl_Test1_Cone1_YarnCount2.Text) & "     ,       Test1_Cone1_YarnCount3 = " & Val(lbl_Test1_Cone1_YarnCount3.Text) & "     ,    Test1_Cone1_YarnCount4  =  " & Val(lbl_Test1_Cone1_YarnCount4.Text) & "     ,Test1_Cone1_YarnCount5   = " & Val(lbl_Test1_Cone1_YarnCount5.Text) & " ,   Test1_Cone2_YarnWeight1  = " & Str(Val(txt_Test1_Cone2_YarnWeight1.Text)) & "   ,  Test1_Cone2_YarnWeight2  = " & Str(Val(txt_Test1_Cone2_YarnWeight2.Text)) & "   , Test1_Cone2_YarnWeight3   = " & Str(Val(txt_Test1_Cone2_YarnWeight3.Text)) & ",    Test1_Cone2_YarnWeight4   =" & Str(Val(txt_Test1_Cone2_YarnWeight4.Text)) & "   ,   Test1_Cone2_YarnWeight5 = " & Str(Val(txt_Test1_Cone2_YarnWeight5.Text)) & "    ,   Test1_Cone2_YarnCount1 = " & Str(Val(lbl_Test1_Cone2_YarnCount1.Text)) & "  ,  Test1_Cone2_YarnCount2  = " & Str(Val(lbl_Test1_Cone2_YarnCount2.Text)) & "   , Test1_Cone2_YarnCount3  = " & Str(Val(lbl_Test1_Cone2_YarnCount3.Text)) & "  ,    Test1_Cone2_YarnCount4 = " & Str(Val(lbl_Test1_Cone2_YarnCount4.Text)) & "   , Test1_Cone2_YarnCount5   = " & Str(Val(lbl_Test1_Cone2_YarnCount5.Text)) & "    ,  Test1_Cone3_YarnWeight1  = " & Str(Val(txt_Test1_Cone3_YarnWeight1.Text)) & "    ,   Test1_Cone3_YarnWeight2  = " & Str(Val(txt_Test1_Cone3_YarnWeight2.Text)) & "  ,Test1_Cone3_YarnWeight3  = " & Str(Val(txt_Test1_Cone3_YarnWeight3.Text)) & "   ,  Test1_Cone3_YarnWeight4    = " & Str(Val(txt_Test1_Cone3_YarnWeight4.Text)) & "  ,   Test1_Cone3_YarnWeight5  = " & Str(Val(txt_Test1_Cone3_YarnWeight5.Text)) & "     ,  Test1_Cone3_YarnCount1  = " & Str(Val(lbl_Test1_Cone3_YarnCount1.Text)) & "   , Test1_Cone3_YarnCount2  = " & Str(Val(lbl_Test1_Cone3_YarnCount2.Text)) & "  , Test1_Cone3_YarnCount3 =" & Str(Val(lbl_Test1_Cone3_YarnCount3.Text)) & "    ,    Test1_Cone3_YarnCount4  =" & Str(Val(lbl_Test1_Cone3_YarnCount4.Text)) & "    ,   Test1_Cone3_YarnCount5 = " & Str(Val(lbl_Test1_Cone3_YarnCount5.Text)) & "     ,   Test1_Cone4_YarnWeight1 = " & Val(txt_Test1_Cone4_YarnWeight1.Text) & " , Test1_Cone4_YarnWeight2 = " & Val(txt_Test1_Cone4_YarnWeight2.Text) & "  ,Test1_Cone4_YarnWeight3  = " & Val(txt_Test1_Cone4_YarnWeight3.Text) & "   ,    Test1_Cone4_YarnWeight4   =" & Val(txt_Test1_Cone4_YarnWeight4.Text) & "    ,   Test1_Cone4_YarnWeight5 =" & Val(txt_Test1_Cone4_YarnWeight5.Text) & "  ,   Test1_Cone4_YarnCount1  = " & Str(Val(lbl_Test1_Cone4_YarnCount1.Text)) & "  ,  Test1_Cone4_YarnCount2    = " & Str(Val(lbl_Test1_Cone4_YarnCount2.Text)) & "  ,Test1_Cone4_YarnCount3 = " & Str(Val(lbl_Test1_Cone4_YarnCount3.Text)) & " ,    Test1_Cone4_YarnCount4  = " & Str(Val(lbl_Test1_Cone4_YarnCount4.Text)) & " ,  Test1_Cone4_YarnCount5  = " & Str(Val(lbl_Test1_Cone4_YarnCount5.Text)) & "     ,Test1_Cone5_YarnWeight1  = " & Str(Val(txt_Test1_Cone5_YarnWeight1.Text)) & " , Test1_Cone5_YarnWeight2  = " & Str(Val(txt_Test1_Cone5_YarnWeight2.Text)) & " , Test1_Cone5_YarnWeight3  = " & Str(Val(txt_Test1_Cone5_YarnWeight3.Text)) & "  , Test1_Cone5_YarnWeight4  = " & Str(Val(txt_Test1_Cone5_YarnWeight1.Text)) & " ,   Test1_Cone5_YarnWeight5 = " & Str(Val(txt_Test1_Cone5_YarnWeight5.Text)) & " ,  Test1_Cone5_YarnCount1 = " & Str(Val(lbl_Test1_Cone5_YarnCount1.Text)) & " , Test1_Cone5_YarnCount2 = " & Str(Val(lbl_Test1_Cone5_YarnCount2.Text)) & "   , Test1_Cone5_YarnCount3 = " & Str(Val(lbl_Test1_Cone5_YarnCount3.Text)) & "    ,  Test1_Cone5_YarnCount4   = " & Str(Val(lbl_Test1_Cone5_YarnCount4.Text)) & "       , Test1_Cone5_YarnCount5 = " & Str(Val(lbl_Test1_Cone5_YarnCount5.Text)) & "   , Test1_Cone6_YarnWeight1  = " & Str(Val(txt_Test1_Cone6_YarnWeight1.Text)) & "   , Test1_Cone6_YarnWeight2  = " & Str(Val(txt_Test1_Cone6_YarnWeight2.Text)) & "   , Test1_Cone6_YarnWeight3  = " & Val(txt_Test1_Cone6_YarnWeight3.Text) & "  ,    Test1_Cone6_YarnWeight4 = " & Val(txt_Test1_Cone6_YarnWeight4.Text) & ", Test1_Cone6_YarnWeight5 = " & Val(txt_Test1_Cone6_YarnWeight5.Text) & "  ,   Test1_Cone6_YarnCount1 = " & Val(lbl_Test1_Cone6_YarnCount1.Text) & "  , Test1_Cone6_YarnCount2   = " & Val(lbl_Test1_Cone6_YarnCount2.Text) & "    ,   Test1_Cone6_YarnCount3  = " & Val(lbl_Test1_Cone6_YarnCount3.Text) & "    ,    Test1_Cone6_YarnCount4 = " & Val(lbl_Test1_Cone6_YarnCount4.Text) & "   , Test1_Cone6_YarnCount5 = " & Val(lbl_Test1_Cone6_YarnCount5.Text) & "    ,   Test2_Cone1_YarnWeight1  = " & Val(txt_Test2_Cone1_YarnWeight1.Text) & " ,  Test2_Cone1_YarnWeight2 = " & Val(txt_Test2_Cone1_YarnWeight2.Text) & " ,  Test2_Cone1_YarnWeight3 = " & Val(txt_Test2_Cone1_YarnWeight3.Text) & " ,   Test2_Cone1_YarnWeight4 = " & Val(txt_Test2_Cone1_YarnWeight4.Text) & "  ,   Test2_Cone1_YarnWeight5  =  " & Val(txt_Test2_Cone1_YarnWeight5.Text) & "   ,   Test2_Cone1_YarnCount1  = " & Val(lbl_Test2_Cone1_YarnCount1.Text) & "   ,          Test2_Cone1_YarnCount2  = " & Val(lbl_Test2_Cone1_YarnCount2.Text) & "     , Test2_Cone1_YarnCount3   = " & Val(lbl_Test2_Cone1_YarnCount1.Text) & "   ,Test2_Cone1_YarnCount4  = " & Str(Val(lbl_Test2_Cone1_YarnCount4.Text)) & " ,  Test2_Cone1_YarnCount5 = " & Str(Val(lbl_Test2_Cone1_YarnCount5.Text)) & "  ,   Test2_Cone2_YarnWeight1  = " & Val(txt_Test2_Cone2_YarnWeight1.Text) & " , Test2_Cone2_YarnWeight2   = " & Val(txt_Test2_Cone2_YarnWeight2.Text) & "   ,Test2_Cone2_YarnWeight3  = " & Val(txt_Test2_Cone2_YarnWeight3.Text) & "  ,  Test2_Cone2_YarnWeight4  = " & Val(txt_Test2_Cone2_YarnWeight4.Text) & "  ,   Test2_Cone2_YarnWeight5  = " & Val(txt_Test2_Cone2_YarnWeight5.Text) & "   ,  Test2_Cone2_YarnCount1 = " & Str(Val(lbl_Test2_Cone2_YarnCount1.Text)) & "  ,Test2_Cone2_YarnCount2  = " & Str(Val(lbl_Test2_Cone2_YarnCount2.Text)) & "  ,  Test2_Cone2_YarnCount3  = " & Str(Val(lbl_Test2_Cone2_YarnCount3.Text)) & "   ,Test2_Cone2_YarnCount4   = " & Str(Val(lbl_Test2_Cone2_YarnCount4.Text)) & "  , Test2_Cone2_YarnCount5 = " & Str(Val(lbl_Test2_Cone2_YarnCount5.Text)) & "   , Test2_Cone3_YarnWeight1 = " & Val(txt_Test2_Cone3_YarnWeight1.Text) & "  ,   Test2_Cone3_YarnWeight2 = " & Val(txt_Test2_Cone3_YarnWeight2.Text) & " ,  Test2_Cone3_YarnWeight3 = " & Val(txt_Test2_Cone3_YarnWeight3.Text) & "  ,   Test2_Cone3_YarnWeight4  = " & Val(txt_Test2_Cone3_YarnWeight4.Text) & "  ,   Test2_Cone3_YarnWeight5 = " & Val(txt_Test2_Cone3_YarnWeight5.Text) & "  ,   Test2_Cone3_YarnCount1 = " & Str(Val(lbl_Test2_Cone3_YarnCount1.Text)) & "   , Test2_Cone3_YarnCount2   = " & Str(Val(lbl_Test2_Cone3_YarnCount2.Text)) & "  ,  Test2_Cone3_YarnCount3  = " & Str(Val(lbl_Test2_Cone3_YarnCount3.Text)) & "     ,    Test2_Cone3_YarnCount4  = " & Str(Val(lbl_Test2_Cone3_YarnCount4.Text)) & "   ,  Test2_Cone3_YarnCount5 = " & Str(Val(lbl_Test2_Cone3_YarnCount5.Text)) & ", Test2_Cone4_YarnWeight1  = " & Val(txt_Test2_Cone4_YarnWeight1.Text) & "    ,Test2_Cone4_YarnWeight2 = " & Val(txt_Test2_Cone4_YarnWeight2.Text) & "  , Test2_Cone4_YarnWeight3 = " & Val(txt_Test2_Cone4_YarnWeight3.Text) & "    ,    Test2_Cone4_YarnWeight4   = " & Val(txt_Test2_Cone4_YarnWeight4.Text) & "  ,   Test2_Cone4_YarnWeight5 = " & Val(txt_Test2_Cone4_YarnWeight5.Text) & "  ,   Test2_Cone4_YarnCount1 = " & Val(lbl_Test2_Cone4_YarnCount1.Text) & "   ,  Test2_Cone4_YarnCount2   = " & Val(lbl_Test2_Cone4_YarnCount2.Text) & ", Test2_Cone4_YarnCount3 = " & Val(lbl_Test2_Cone4_YarnCount3.Text) & "   ,    Test2_Cone4_YarnCount4  = " & Val(lbl_Test2_Cone4_YarnCount4.Text) & "   ,    Test2_Cone4_YarnCount5 = " & Val(lbl_Test2_Cone4_YarnCount5.Text) & "   ,  Test2_Cone5_YarnWeight1  = " & Val(txt_Test2_Cone5_YarnWeight1.Text) & " ,   Test2_Cone5_YarnWeight2 = " & Val(txt_Test2_Cone5_YarnWeight2.Text) & " ,  Test2_Cone5_YarnWeight3  = " & Val(txt_Test2_Cone5_YarnWeight3.Text) & " ,  Test2_Cone5_YarnWeight4  = " & Val(txt_Test2_Cone5_YarnWeight4.Text) & "      ,   Test2_Cone5_YarnWeight5   = " & Str(Val(txt_Test3_Cone4_YarnWeight2.Text)) & "   ,   Test2_Cone5_YarnCount1   = " & Val(lbl_Test2_Cone5_YarnCount1.Text) & "   ,  Test2_Cone5_YarnCount2  = " & Val(lbl_Test2_Cone5_YarnCount2.Text) & "  , Test2_Cone5_YarnCount3  = " & Val(lbl_Test2_Cone5_YarnCount3.Text) & "   ,    Test2_Cone5_YarnCount4  = " & Val(lbl_Test2_Cone5_YarnCount4.Text) & "   ,   Test2_Cone5_YarnCount5  = " & Val(lbl_Test2_Cone5_YarnCount5.Text) & "          ,           Test2_Cone6_YarnWeight1 =  " & Str(Val(txt_Test2_Cone6_YarnWeight1.Text)) & "  ,   Test2_Cone6_YarnWeight2  = " & Str(Val(txt_Test2_Cone6_YarnWeight2.Text)) & "  ,Test2_Cone6_YarnWeight3 = " & Str(Val(txt_Test2_Cone6_YarnWeight3.Text)) & "  ,Test2_Cone6_YarnWeight4 = " & Str(Val(txt_Test2_Cone6_YarnWeight4.Text)) & "  ,  Test2_Cone6_YarnWeight5  = " & Str(Val(txt_Test2_Cone6_YarnWeight5.Text)) & "   ,   Test2_Cone6_YarnCount1  = " & Str(Val(lbl_Test2_Cone6_YarnCount1.Text)) & "  ,Test2_Cone6_YarnCount2  = " & Str(Val(lbl_Test2_Cone6_YarnCount2.Text)) & "  , Test2_Cone6_YarnCount3  = " & Str(Val(lbl_Test2_Cone6_YarnCount3.Text)) & "  ,    Test2_Cone6_YarnCount4  = " & Str(Val(lbl_Test2_Cone6_YarnCount4.Text)) & "  ,   Test2_Cone6_YarnCount5 = " & Str(Val(lbl_Test2_Cone6_YarnCount5.Text)) & " ,   Test3_Cone1_YarnWeight1 = " & Str(Val(txt_Test3_Cone1_YarnWeight1.Text)) & " ,    Test3_Cone1_YarnWeight2  = " & Str(Val(txt_Test3_Cone1_YarnWeight2.Text)) & "  ,Test3_Cone1_YarnWeight3  = " & Str(Val(txt_Test3_Cone1_YarnWeight3.Text)) & ",    Test3_Cone1_YarnWeight4   = " & Str(Val(txt_Test3_Cone1_YarnWeight4.Text)) & " ,   Test3_Cone1_YarnWeight5 = " & Str(Val(txt_Test3_Cone1_YarnWeight5.Text)) & "  ,   Test3_Cone1_YarnCount1   = " & Str(Val(lbl_Test3_Cone1_YarnCount1.Text)) & " , Test3_Cone1_YarnCount2   = " & Str(Val(lbl_Test3_Cone1_YarnCount2.Text)) & ",  Test3_Cone1_YarnCount3  = " & Str(Val(lbl_Test3_Cone1_YarnCount3.Text)) & "  ,   Test3_Cone1_YarnCount4   = " & Str(Val(lbl_Test3_Cone1_YarnCount4.Text)) & "   , Test3_Cone1_YarnCount5   = " & Str(Val(lbl_Test3_Cone1_YarnCount5.Text)) & "          ,               Test3_Cone2_YarnWeight1  = " & Str(Val(txt_Test3_Cone2_YarnWeight1.Text)) & ",Test3_Cone2_YarnWeight2 = " & Str(Val(txt_Test3_Cone2_YarnWeight2.Text)) & "    , Test3_Cone2_YarnWeight3  = " & Str(Val(txt_Test3_Cone2_YarnWeight3.Text)) & "  ,Test3_Cone2_YarnWeight4 = " & Str(Val(txt_Test3_Cone2_YarnWeight4.Text)) & ",   Test3_Cone2_YarnWeight5 = " & Str(Val(txt_Test3_Cone2_YarnWeight5.Text)) & "  ,   Test3_Cone2_YarnCount1  = " & Str(Val(lbl_Test3_Cone2_YarnCount1.Text)) & " , Test3_Cone2_YarnCount2= " & Str(Val(lbl_Test3_Cone2_YarnCount2.Text)) & "  ,Test3_Cone2_YarnCount3 = " & Str(Val(lbl_Test3_Cone2_YarnCount3.Text)) & "  ,    Test3_Cone2_YarnCount4  = " & Str(Val(lbl_Test3_Cone2_YarnCount4.Text)) & "   , Test3_Cone2_YarnCount5 = " & Str(Val(lbl_Test3_Cone2_YarnCount5.Text)) & " , Test3_Cone3_YarnWeight1    = " & Val(txt_Test3_Cone3_YarnWeight1.Text) & ",  Test3_Cone3_YarnWeight2 = " & Val(txt_Test3_Cone3_YarnWeight2.Text) & "   ,          Test3_Cone3_YarnWeight3  = " & Val(txt_Test3_Cone3_YarnWeight3.Text) & "  ,    Test3_Cone3_YarnWeight4 = " & Val(txt_Test3_Cone3_YarnWeight4.Text) & "   ,   Test3_Cone3_YarnWeight5 = " & Val(txt_Test3_Cone3_YarnWeight5.Text) & "   ,Test3_Cone3_YarnCount1 = " & Str(Val(lbl_Test3_Cone3_YarnCount1.Text)) & "  ,          Test3_Cone3_YarnCount2 = " & Str(Val(lbl_Test3_Cone3_YarnCount2.Text)) & "  , Test3_Cone3_YarnCount3 = " & Str(Val(lbl_Test3_Cone3_YarnCount3.Text)) & "   , Mill_Idno1 = " & Str(Val(mill1_Idno)) & ",Mill_Idno2 = " & Str(Val(mill2_Idno)) & ",Mill_Idno3 = " & Str(Val(mill3_Idno)) & " ,  Test3_Cone3_YarnCount4    = " & Str(Val(lbl_Test3_Cone3_YarnCount4.Text)) & "   ,  Test3_Cone3_YarnCount5 = " & Str(Val(lbl_Test3_Cone3_YarnCount5.Text)) & "      , Test3_Cone4_YarnWeight1  = " & Str(Val(txt_Test3_Cone4_YarnWeight1.Text)) & "   ,     Test3_Cone4_YarnWeight2 = " & Str(Val(txt_Test3_Cone4_YarnWeight2.Text)) & "   ,Test3_Cone4_YarnWeight3    = " & Str(Val(txt_Test3_Cone4_YarnWeight3.Text)) & " ,    Test3_Cone4_YarnWeight4  = " & Str(Val(txt_Test3_Cone4_YarnWeight4.Text)) & "  ,   Test3_Cone4_YarnWeight5  = " & Str(Val(txt_Test3_Cone4_YarnWeight5.Text)) & "       ,   Test3_Cone4_YarnCount1 = " & Str(Val(lbl_Test3_Cone4_YarnCount1.Text)) & "  , Test3_Cone4_YarnCount2  = " & Str(Val(lbl_Test3_Cone4_YarnCount2.Text)) & "  , Test3_Cone4_YarnCount3 = " & Str(Val(lbl_Test3_Cone4_YarnCount3.Text)) & "  ,   Test3_Cone4_YarnCount4  = " & Str(Val(lbl_Test3_Cone4_YarnCount4.Text)) & "    ,    Test3_Cone4_YarnCount5 = " & Str(Val(lbl_Test3_Cone4_YarnCount5.Text)) & "   ,         Test3_Cone5_YarnWeight1   = " & Str(Val(txt_Test3_Cone5_YarnWeight1.Text)) & "     ,Test3_Cone5_YarnWeight2  = " & Str(Val(txt_Test3_Cone5_YarnWeight2.Text)) & "  ,  Test3_Cone5_YarnWeight3  = " & Str(Val(txt_Test3_Cone5_YarnWeight3.Text)) & "   ,    Test3_Cone5_YarnWeight4  = " & Str(Val(txt_Test3_Cone5_YarnWeight4.Text)) & "  ,   Test3_Cone5_YarnWeight5  = " & Str(Val(txt_Test3_Cone5_YarnWeight5.Text)) & " ,   Test3_Cone5_YarnCount1  = " & Str(Val(lbl_Test3_Cone5_YarnCount1.Text)) & " ,  Test3_Cone5_YarnCount2  = " & Str(Val(lbl_Test3_Cone5_YarnCount2.Text)) & "  ,Test3_Cone5_YarnCount3  = " & Str(Val(lbl_Test3_Cone5_YarnCount3.Text)) & "    ,   Test3_Cone5_YarnCount4 = " & Str(Val(lbl_Test3_Cone5_YarnCount4.Text)) & " , Test3_Cone5_YarnCount5  = " & Str(Val(lbl_Test3_Cone5_YarnCount5.Text)) & "   ,Test3_Cone6_YarnWeight1   = " & Str(Val(txt_Test3_Cone6_YarnWeight1.Text)) & " ,   Test3_Cone6_YarnWeight2 = " & Str(Val(txt_Test3_Cone6_YarnWeight2.Text)) & "     ,Test3_Cone6_YarnWeight3 = " & Str(Val(txt_Test3_Cone6_YarnWeight3.Text)) & ",    Test3_Cone6_YarnWeight4  = " & Str(Val(txt_Test3_Cone6_YarnWeight4.Text)) & "  ,   Test3_Cone6_YarnWeight5  =" & Str(Val(txt_Test3_Cone6_YarnWeight5.Text)) & "  ,   Test3_Cone6_YarnCount1  =" & Str(Val(lbl_Test3_Cone6_YarnCount1.Text)) & "  ,Test3_Cone6_YarnCount2 = " & Str(Val(lbl_Test3_Cone6_YarnCount2.Text)) & " , Test3_Cone6_YarnCount3 = " & Str(Val(lbl_Test3_Cone6_YarnCount3.Text)) & "   ,  Test3_Cone6_YarnCount4 = " & Str(Val(lbl_Test3_Cone6_YarnCount4.Text)) & " ,  Test3_Cone6_YarnCount5 = " & Str(Val(lbl_Test3_Cone6_YarnCount5.Text)) & "  , Test1_Cone1_Mean  = " & Str(Val(lbl_Test1_Cone1_Mean.Text)) & "  ,   Test1_Cone1_Cv = " & Str(Val(lbl_Test1_Cone1_CV.Text)) & "  , Test1_Cone1_SD = " & Str(Val(lbl_Test1_Cone1_SD.Text)) & "   , Test1_Cone2_Mean = " & Str(Val(lbl_Test1_Cone2_mean.Text)) & "  ,    Test1_Cone2_Cv   = " & Str(Val(lbl_Test1_Cone2_Cv.Text)) & "  ,    Test1_Cone2_SD = " & Str(Val(lbl_Test1_Cone2_SD.Text)) & ",  Test1_Cone3_Mean = " & Str(Val(lbl_Test1_Cone3_mean.Text)) & " , Test1_Cone3_Cv = " & Str(Val(lbl_Test1_Cone3_Cv.Text)) & " ,   Test1_Cone3_SD = " & Str(Val(lbl_Test1_Cone3_SD.Text)) & " , Test1_Cone4_Mean = " & Str(Val(lbl_Test1_Cone4_mean.Text)) & "  ,    Test1_Cone4_Cv  = " & Str(Val(lbl_Test1_Cone4_Cv.Text)) & " ,  Test1_Cone4_SD  = " & Str(Val(lbl_Test1_Cone4_SD.Text)) & "   , Test1_Cone5_Mean  = " & Str(Val(lbl_Test1_Cone5_mean.Text)) & "   ,  Test1_Cone5_Cv   = " & Str(Val(lbl_Test1_Cone5_Cv.Text)) & "  ,   Test1_Cone5_SD   = " & Str(Val(lbl_Test1_Cone5_SD.Text)) & " ,   Test1_Cone6_Mean = " & Str(Val(lbl_Test1_Cone6_mean.Text)) & "  ,Test1_Cone6_Cv = " & Str(Val(lbl_Test1_Cone6_Cv.Text)) & "  ,  Test1_Cone6_SD = " & Str(Val(lbl_Test1_Cone6_Sd.Text)) & "    ,Test1_Avg_Cv  = " & Str(Val(lbl_Test1_AvgCv.Text)) & "   ,Test1_Avg_Sd = " & Str(Val(lbl_test1_AvgSd.Text)) & " , Test1_Avg_Coul = " & Str(Val(lbl_Test1_AvgCoul.Text)) & "   , Test2_Cone1_Mean = " & Str(Val(lbl_Test2_Cone1_Mean.Text)) & "  ,    Test2_Cone1_Cv = " & Str(Val(lbl_Test2_Cone1_CV.Text)) & ",  Test2_Cone1_SD  = " & Str(Val(lbl_Test2_Cone1_SD.Text)) & "    , Test2_Cone2_Mean = " & Str(Val(lbl_Test2_Cone2_Mean.Text)) & "  ,    Test2_Cone2_Cv  = " & Str(Val(lbl_Test2_Cone2_CV.Text)) & "  ,Test2_Cone2_SD  = " & Str(Val(lbl_Test2_Cone2_SD.Text)) & "   ,   Test2_Cone3_Mean  = " & Str(Val(lbl_Test2_Cone3_Mean.Text)) & "   ,    Test2_Cone3_Cv  = " & Str(Val(lbl_Test2_Cone3_CV.Text)) & "   , Test2_Cone3_SD  = " & Str(Val(lbl_Test2_Cone3_SD.Text)) & "    ,Test2_Cone4_Mean = " & Str(Val(lbl_Test2_Cone4_Mean.Text)) & "    ,    Test2_Cone4_Cv  = " & Str(Val(lbl_Test2_Cone4_CV.Text)) & " , Test2_Cone4_SD = " & Str(Val(lbl_Test2_Cone4_SD.Text)) & " ,  Test2_Cone5_Mean  = " & Str(Val(lbl_Test2_Cone5_Mean.Text)) & "   ,Test2_Cone5_Cv = " & Str(Val(lbl_Test2_Cone5_CV.Text)) & " ,Test2_Cone5_SD =" & Str(Val(lbl_Test2_Cone5_SD.Text)) & "    ,Test2_Cone6_Mean =" & Str(Val(lbl_Test2_Cone6_Mean.Text)) & " , Test2_Cone6_Cv = " & Str(Val(lbl_Test2_Cone6_CV.Text)) & "   , Test2_Cone6_SD = " & Str(Val(lbl_Test2_Cone6_SD.Text)) & "  ,  Test2_Avg_Cv = " & Str(Val(lbl_Test2_AvgCv.Text)) & "  ,    Test2_Avg_Sd = " & Str(Val(lbl_Test2_Avg_Sd.Text)) & "  ,    Test2_Avg_Coul =" & Str(Val(lbl_Test2_avgCoul.Text)) & " ,Test3_Cone1_Mean =" & Str(Val(lbl_Test3_Cone1_Mean.Text)) & "  ,  Test3_Cone1_Cv   =" & Str(Val(lbl_Test3_Cone1_CV.Text)) & ",  Test3_Cone1_SD  =" & Str(Val(lbl_Test3_Cone2_SD.Text)) & " ,  Test3_Cone2_Mean  =" & Str(Val(lbl_Test3_Cone2_mean.Text)) & "  ,    Test3_Cone2_Cv =" & Str(Val(lbl_Test3_Cone2_CV.Text)) & " ,  Test3_Cone2_SD = " & Str(Val(lbl_Test3_Cone2_SD.Text)) & "  , Test3_Cone3_Mean = " & Str(Val(lbl_Test3_Cone3_mean.Text)) & "  ,    Test3_Cone3_Cv  =" & Str(Val(lbl_Test3_Cone3_CV.Text)) & ",   Test3_Cone3_SD = " & Str(Val(lbl_Test3_Cone3_SD.Text)) & "   ,  Test3_Cone4_Mean  = " & Str(Val(lbl_Test3_Cone4_mean.Text)) & "   ,    Test3_Cone4_Cv  = " & Str(Val(lbl_Test3_Cone4_CV.Text)) & "   ,  Test3_Cone4_SD  = " & Str(Val(lbl_Test3_Cone4_SD.Text)) & "  ,   Test3_Cone5_Mean = " & Str(Val(lbl_Test3_Cone5_Mean.Text)) & "  ,    Test3_Cone5_Cv   = " & Str(Val(lbl_Test3_Cone5_CV.Text)) & ",  Test3_Cone5_SD   = " & Str(Val(lbl_Test3_Cone5_SD.Text)) & "  ,        Test3_Cone6_Mean  = " & Str(Val(lbl_Test3_Cone6_Mean.Text)) & "    ,    Test3_Cone6_Cv  = " & Str(Val(lbl_Test3_Cone6_CV.Text)) & "  ,  Test3_Cone6_SD  = " & Str(Val(lbl_Test3_Cone6_SD.Text)) & "  ,  Test3_Avg_Cv =  " & Str(Val(lbl_Test3_AvgCv.Text)) & "  ,   Test3_Avg_Sd  =  " & Str(Val(lbl_Test3_AvgSd.Text)) & ", Test3_Avg_Coul =  " & Str(Val(lbl_Test3_AvgCoul.Text)) & " ,User_idNo = " & Val(lbl_UserName.Text) & "   Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Test_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If

            '  Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "NEW", "Yarn_test_Head", "Yarn_test_Code", Val(lbl_Company.Tag), NewCode, lbl_InvoiceNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Yarn_test_Code, Company_IdNo, for_OrderBy", tr)


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

            If txt_test1No.Enabled And txt_test1No.Visible Then txt_test1No.Focus()


        End Try

    End Sub




    Private Sub btn_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

   


    Private Sub txt_Test3_Cone6_YarnWeight5_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Test3_Cone6_YarnWeight5.KeyDown
        If e.KeyCode = 40 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()
            Else
                txt_test1No.Focus()
            End If
        End If
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_Test3_Cone6_YarnWeight5_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Test3_Cone6_YarnWeight5.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()
            Else
                txt_test1No.Focus()
            End If

        End If
    End Sub

    'Private Sub dtp_Test1Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Test1Date.KeyDown
    '    If e.KeyCode = 40 Then
    '        '  tab_Main.SelectTab(0)
    '        txt_Test1_Cone1_YarnWeight1.Focus()
    '    End If
    '    If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    'End Sub

    'Private Sub dtp_Test1Date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_Test1Date.KeyPress
    '    If Asc(e.KeyChar) = 13 Then
    '        ' tab_Main.SelectTab(0)
    '        txt_Test1_Cone1_YarnWeight1.Focus()
    '    End If
    'End Sub

    'Private Sub dtp_test3date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_test3date.KeyDown
    '    If e.KeyCode = 40 Then
    '        ' tab_Main.SelectTab(2)
    '        txt_Test3_Cone1_YarnWeight1.Focus()
    '    End If
    '    If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    'End Sub

    'Private Sub dtp_test3date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_test3date.KeyPress
    '    If Asc(e.KeyChar) = 13 Then
    '        'tab_Main.SelectTab(2)
    '        txt_Test3_Cone1_YarnWeight1.Focus()
    '    End If
    'End Sub

    'Private Sub dtp_Test2Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Test2Date.KeyDown
    '    If e.KeyCode = 40 Then
    '        ' tab_Main.SelectTab(2)
    '        txt_Test3_Cone1_YarnWeight1.Focus()
    '    End If
    '    If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    'End Sub

    'Private Sub dtp_Test2Date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_Test2Date.KeyPress
    '    If Asc(e.KeyChar) = 13 Then
    '        ' tab_Main.SelectTab(0)
    '        txt_Test2_Cone1_YarnWeight1.Focus()
    '    End If
    'End Sub


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
    Private Sub cbo_Grid_millName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_MillName1.GotFocus
        vCbo_ItmNm = Trim(cbo_MillName1.Text)
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")
    End Sub

    Private Sub cbo_Grid_RackNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_MillName1.KeyDown

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_MillName1, Nothing, Nothing, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")

        If (e.KeyValue = 38 And cbo_MillName1.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
            txt_test1No.Focus()
        End If

        If (e.KeyValue = 40 And cbo_MillName1.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            dtp_Test1Date.Focus()
        End If



    End Sub

    Private Sub cbo_Grid_RackNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_MillName1.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_MillName1, Nothing, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then
            dtp_Test1Date.Focus()
        End If

    End Sub

    Private Sub cbo_Grid_RackNo_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_MillName1.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New Mill_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_MillName1.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If

    End Sub
    Private Sub cbo_MillName2_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_MillName2.GotFocus
        vCbo_ItmNm = Trim(cbo_MillName2.Text)
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")
    End Sub

    Private Sub cbo_MillName2_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_MillName2.KeyDown

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_MillName2, Nothing, Nothing, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")

        If (e.KeyValue = 38 And cbo_MillName2.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
            txt_Test2No.Focus()
        End If

        If (e.KeyValue = 40 And cbo_MillName2.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            dtp_Test2Date.Focus()
        End If

    End Sub

    Private Sub cbo_MillName2_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_MillName2.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_MillName2, Nothing, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then
            dtp_Test2Date.Focus()
        End If

    End Sub

    Private Sub cbo_MillName2_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_MillName2.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New Mill_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_MillName2.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If

    End Sub
    Private Sub cbo_MillName3_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_MillName3.GotFocus
        vCbo_ItmNm = Trim(cbo_MillName3.Text)
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")
    End Sub

    Private Sub cbo_MillName3_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_MillName3.KeyDown

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_MillName3, Nothing, Nothing, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")

        If (e.KeyValue = 38 And cbo_MillName3.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
            txt_test3No.Focus()
        End If

        If (e.KeyValue = 40 And cbo_MillName3.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            dtp_test3date.Focus()
        End If

    End Sub

    Private Sub cbo_MillName3_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_MillName3.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_MillName3, Nothing, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then
            dtp_test3date.Focus()
        End If

    End Sub

    Private Sub cbo_MillName3_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_MillName3.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New Mill_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_MillName3.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub dtp_Test1Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Test1Date.KeyDown
        If (e.KeyValue = 38) Then
            cbo_MillName1.Focus()
        End If

        If (e.KeyValue = 40) Then
            txt_Test1_Cone1_YarnWeight1.Focus()
        End If
    End Sub

    Private Sub dtp_Test1Date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_Test1Date.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_Test1_Cone1_YarnWeight1.Focus()
        End If
    End Sub
    Private Sub dtp_Test2Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Test2Date.KeyDown
        If (e.KeyValue = 38) Then
            cbo_MillName2.Focus()
        End If

        If (e.KeyValue = 40) Then
            txt_Test2_Cone2_YarnWeight2.Focus()
        End If
    End Sub

    Private Sub dtp_Test2Date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_Test2Date.KeyPress
        If Asc(e.KeyChar) = 23 Then
            txt_Test2_Cone2_YarnWeight2.Focus()
        End If
    End Sub
    Private Sub dtp_Test3Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_test3date.KeyDown
        If (e.KeyValue = 38) Then
            cbo_MillName3.Focus()
        End If

        If (e.KeyValue = 40) Then
            txt_Test3_Cone3_YarnWeight3.Focus()
        End If
    End Sub

    Private Sub dtp_Test3Date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_test3date.KeyPress
        If Asc(e.KeyChar) = 33 Then
            txt_Test3_Cone3_YarnWeight3.Focus()
        End If
    End Sub

    Private Sub btn_UserModification_Click(sender As System.Object, e As System.EventArgs) Handles btn_UserModification.Click
        If Val(Common_Procedures.User.IdNo) = 1 Then
            Dim f1 As New User_Modifications
            f1.Entry_Name = Me.Name
            f1.Entry_PkValue = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            f1.ShowDialog()
        End If
    End Sub
End Class