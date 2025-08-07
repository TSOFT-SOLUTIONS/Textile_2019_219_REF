Imports Newtonsoft.Json
Imports RestSharp
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Drawing
Imports System.IO
Imports System.Net
Imports System.Windows.Forms
Imports TaxProEInvoice.API
Imports TaxProEWB.API
Imports System.Web.Script.Serialization
Public Class eInvoice

    Public Shared eInvSession As New eInvoiceSession()
    Public Shared ewbSession As New EWBSession()

    Public GSPName As String = ""
    Public ASPUserId As String = ""
    Public ASPPassWord As String = ""
    Public ClientId As String = ""
    Public ClientSecret As String = ""
    Public AuthURL As String = ""
    Public BaseURL As String = ""
    Public EWBBaseURL As String = ""
    Public CancelEWBURL As String = ""
    Public EIUserName As String = ""
    Public EIPassword As String = ""
    Public GSTIN As String = ""

    Public Shared AppKey As String = ""
    Public Shared AuthToken As String = ""
    Public Shared SEK As String = ""
    Public Shared AuthTokenExp As String = ""
    Public Shared AuthTokenReturnMsg As String = ""
    Public Shared IRNMessage As String = ""
    '  Public Shared CredentialsProvided_EINV As Boolean = True
    ' Public Shared CredentialsProvided_EWB As Boolean = True

    Public Company_Id As Int16
    Private Con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)

    Private CredentialsProvided As Boolean = False

    Public Sub New(CmpId As Int16)
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        Company_Id = CmpId
        Con.Open()
        LoadEIAPICredentials()
    End Sub

    Public Sub LoadEIAPICredentials()

        'eInvSession.LoadAPILoginDetailsFromConfigFile = False
        'eInvSession.LoadAPISettingsFromConfigFile = False

        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        da = New SqlClient.SqlDataAdapter("Select C.Company_IdNo,C.Company_Name,C.Company_ShortName,C.Company_GSTINNo,G.* from Company_Head C Left Outer Join GST_EWB_API_Settings G On C.Company_GSTINNo COLLATE Latin1_General_CI_AI  = G.GSTIN COLLATE Latin1_General_CI_AI Where C.Company_IdNo = " & Company_Id.ToString & " And len(C.COMPANY_GSTINNo) = 15 ", Con)
        dt = New DataTable
        da.Fill(dt)

        If dt.Rows.Count > 0 Then

            If Not IsDBNull(dt.Rows(0).Item("Company_GSTINNo")) Then
                GSTIN = dt.Rows(0).Item("Company_GSTINNo")
                eInvSession.eInvApiLoginDetails.GSTIN = GSTIN
            Else
                MsgBox("Invalid GSTIN Provided", vbOK, "Insufficient Information")
                Exit Sub
            End If

            'If Not IsDBNull(dt.Rows(0).Item("e_Invoice_GSPName")) Then
            '    GSPName = dt.Rows(0).Item("e_Invoice_GSPName")
            '    eInvSession.eInvApiSetting.GSPName = GSPName
            'Else
            '    MsgBox("Invalid GSP Name Provided", vbOK, "Insufficient Information")
            '    Exit Sub
            'End If

            If Not IsDBNull(dt.Rows(0).Item("ASPUSERID")) Then
                ASPUserId = dt.Rows(0).Item("ASPUSERID")
                eInvSession.eInvApiSetting.AspUserId = ASPUserId
            Else
                MsgBox("Invalid ASP User ID Provided", vbOK, "Insufficient Information")
                Exit Sub
            End If

            If Not IsDBNull(dt.Rows(0).Item("ASPPASSWORD")) Then
                ASPPassWord = dt.Rows(0).Item("ASPPASSWORD")
                eInvSession.eInvApiSetting.AspPassword = ASPPassWord
            Else
                MsgBox("Invalid ASP Password Provided", vbOK, "Insufficient Information")
                Exit Sub
            End If

            If Not IsDBNull(dt.Rows(0).Item("eInvoice_UserId")) Then
                EIUserName = dt.Rows(0).Item("eInvoice_UserId")
                If Len(Trim(EIUserName)) > 0 Then
                    eInvSession.eInvApiLoginDetails.UserName = EIUserName
                End If
            Else
                MsgBox("Invalid e-Invoice User ID Provided", vbOK, "Insufficient Information")
                Exit Sub
            End If

            If Not IsDBNull(dt.Rows(0).Item("eInvoice_Password")) Then
                EIPassword = dt.Rows(0).Item("eInvoice_Password")
                If Len(Trim(EIPassword)) > 0 Then
                    eInvSession.eInvApiLoginDetails.Password = EIPassword
                End If
            Else
                MsgBox("Invalid e-Invoice Password Provided", vbOK, "Insufficient Information")
                Exit Sub
            End If

            'If Not IsDBNull(dt.Rows(0).Item("eInvoice_AuthURL")) Then
            '    AuthURL = dt.Rows(0).Item("eInvoice_AuthURL")
            '    eInvSession.eInvApiSetting.AuthUrl = AuthURL
            'Else
            '    MsgBox("Invalid e-Invoice Auth URL Provided", vbOK, "Insufficient Information")
            '    Exit Sub
            'End If
            'If Not IsDBNull(dt.Rows(0).Item("eInvoice_BaseURL")) Then
            '    BaseURL = dt.Rows(0).Item("eInvoice_BaseURL")
            '    eInvSession.eInvApiSetting.BaseUrl = BaseURL
            'Else
            '    MsgBox("Invalid e-Invoice Base URL Provided", vbOK, "Insufficient Information")
            '    Exit Sub
            'End If

            'If Not IsDBNull(dt.Rows(0).Item("eInvoice_EWBURL")) Then
            '    EWBBaseURL = dt.Rows(0).Item("eInvoice_EWBURL")
            '    eInvSession.eInvApiSetting.EwbByIRN = EWBBaseURL
            'Else
            '    MsgBox("Invalid e-Invoice Password Provided", vbOK, "Insufficient Information")
            '    Exit Sub
            'End If
            'If Not IsDBNull(dt.Rows(0).Item("eInvoice_CancelEWBURL")) Then
            '    CancelEWBURL = dt.Rows(0).Item("eInvoice_CancelEWBURL")
            '    eInvSession.eInvApiSetting.CancelEwbUrl = CancelEWBURL
            'Else
            '    MsgBox("Invalid e-Invoice Password Provided", vbOK, "Insufficient Information")
            '    Exit Sub
            'End If


        End If

        ''GSPName = eInvSession.eInvApiSetting.GSPName
        ''ASPUserId = eInvSession.eInvApiSetting.AspUserId
        ''ASPPassWord = eInvSession.eInvApiSetting.AspPassword

        ''AuthURL = eInvSession.eInvApiSetting.AuthUrl
        ''BaseURL = eInvSession.eInvApiSetting.BaseUrl
        ''EWBBaseURL = eInvSession.eInvApiSetting.EwbByIRN
        ''CancelEWBURL = eInvSession.eInvApiSetting.CancelEwbUrl

        'ClientId = eInvSession.eInvApiSetting.client_id
        'ClientSecret = eInvSession.eInvApiSetting.client_secret

        ''EIUserName = eInvSession.eInvApiLoginDetails.UserName
        ''EIPassword = eInvSession.eInvApiLoginDetails.Password
        ''GSTIN = eInvSession.eInvApiLoginDetails.GSTIN

        'AppKey = eInvSession.eInvApiLoginDetails.AppKey
        'AuthToken = eInvSession.eInvApiLoginDetails.AuthToken
        'SEK = eInvSession.eInvApiLoginDetails.Sek
        'AuthTokenExp = eInvSession.eInvApiLoginDetails.E_InvoiceTokenExp?.ToString("yyyy-MM-dd HH:mm:ss")

    End Sub




    Public Shared Async Sub GetAuthToken(ResponseObject As RichTextBox)

        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12

        'Dim txnRespWithObj As TxnRespWithObjAndInfo(eInvoiceSession)
        'txnRespWithObj = Await eInvoiceAPI.GetAuthTokenAsync(eInvSession)

        'Dim TxnResp As TxnRespWithObjAndInfo(Of EWBSession) = Await EWBAPI.GetAuthTokenAsync(ewbSession)

        'If TxnResp.IsSuccess Then
        '    AppKey = eInvSession.eInvApiLoginDetails.AppKey
        '    AuthToken = eInvSession.eInvApiLoginDetails.AuthToken
        '    SEK = eInvSession.eInvApiLoginDetails.Sek
        '    AuthTokenExp = eInvSession.eInvApiLoginDetails.E_InvoiceTokenExp?.ToString("yyyy-MM-dd HH:mm:ss")
        '    AuthTokenReturnMsg = "SUCCESS"
        'End If

        'ResponseObject.Text = TxnResp.TxnOutcome
        '-------------------------------

        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12

        'Dim txnRespWithObj As TxnRespWithObjAndInfo(Of eInvoiceSession)
        'txnRespWithObj = Await eInvoiceAPI.GetAuthTokenAsync(eInvSession, True)

        Dim txnRespWithObj As TaxProEInvoice.API.TxnRespWithObj(Of eInvoiceSession) = Await eInvoiceAPI.GetAuthTokenAsync(eInvSession, True)

        If txnRespWithObj.IsSuccess Then
            AppKey = eInvSession.eInvApiLoginDetails.AppKey
            AuthToken = eInvSession.eInvApiLoginDetails.AuthToken
            SEK = eInvSession.eInvApiLoginDetails.Sek
            AuthTokenExp = eInvSession.eInvApiLoginDetails.E_InvoiceTokenExp?.ToString("yyyy-MM-dd HH:mm:ss")
            AuthTokenReturnMsg = "SUCCESS"
        End If

        'AuthTokenReturnMsg = txnRespWithObj.TxnOutcome
        ResponseObject.Text = txnRespWithObj.TxnOutcome

    End Sub

    Public Shared Async Sub GenerateIRN(Company_IdNo As String, InvCode As String, Cn As SqlClient.SqlConnection, ResponseObject As RichTextBox, QRCodePictureBox As PictureBox, IRNTextbox As TextBox, IRNACKNoTextBox As TextBox, IRNACKDateTextBox As TextBox, IRNCancelStatusTextBox As TextBox, InvoiceHeadTable As String, InvoiceHeadTableUniqueCode As String, Entry_PkCondition As String, Optional DocDetailType As String = "INV")
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim arPHNO() As String
        Dim vPHNO As String = ""
        Dim vPHNO_TO_IRN As String = ""
        Dim Nr As Long
        Dim vNOOF_EINVS As String = 0

        Common_Procedures.check_Validating_for_eINVOICE_eWAY_GENERATION()

        If Trim(Entry_PkCondition) = "" Then
            MessageBox.Show("Invalid Entry Pk-Condition", "DOES NOT GENERATE AN E-INVOICE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Val(Common_Procedures.settings.EInvoice_API_TotalCredits_Per_Year) <= 0 Then
            MessageBox.Show("There is no API credits for e-Invoice generation", "DOES NOT GENERATE AN E-INVOICE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        da1 = New SqlClient.SqlDataAdapter("select COUNT(*) from " & Trim(Common_Procedures.CompanyDetailsDataBaseName) & "..eInvoice_eWay_Bill_Details Where Year_Code = '" & Trim(Common_Procedures.FnYearCode) & "' and Entry_Type = 'EINVOICE'", Cn)
        dt1 = New DataTable
        da1.Fill(dt1)

        vNOOF_EINVS = 0
        If dt1.Rows.Count > 0 Then
            If IsDBNull(dt1.Rows(0)(0).ToString) = False Then
                vNOOF_EINVS = dt1.Rows(0)(0).ToString
            End If
        End If
        dt1.Clear()

        If Val(vNOOF_EINVS) >= Val(Common_Procedures.settings.EInvoice_API_TotalCredits_Per_Year) Then
            MessageBox.Show(Common_Procedures.settings.EInvoice_API_TotalCredits_Per_Year & " API Credit for E-Invoices has expired", "DOES NOT GENERATE AN E-INVOICE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        Else
            If (Val(Common_Procedures.settings.EInvoice_API_TotalCredits_Per_Year) - Val(vNOOF_EINVS)) <= 25 Then
                MessageBox.Show("Your E-Invoices API Credits will Expire Soon." & Chr(13) & "There are only " & (Val(Common_Procedures.settings.EInvoice_API_TotalCredits_Per_Year) - Val(vNOOF_EINVS)) & " API credits left out of " & Common_Procedures.settings.EInvoice_API_TotalCredits_Per_Year & " API credits," & Chr(13) & "so you need to Recharge your API credits immediately.", "E-INVOICE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
            End If
        End If


        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12

        If Len(Trim(InvoiceHeadTable)) = 0 Or Len(Trim(InvoiceHeadTableUniqueCode)) = 0 Then
            MessageBox.Show("Provide a valid Invoice Head / Details Table", "Invalid Invoice Table", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
            Exit Sub
        End If

        Dim dt As New DataTable
        Dim da As New SqlClient.SqlDataAdapter("Select  a.*,c.*,s.* from e_Invoice_Head a inner join " &
                                               " Company_Head c on c.Company_IdNo = " & Str(Val(Company_IdNo)) & " inner join State_Head s on c.Company_State_IdNo = s.State_IdNo where Ref_Sales_Code = '" & InvCode.ToString & "'", Cn)
        da.Fill(dt)

        If dt.Rows.Count = 0 Then
            MessageBox.Show("No Valid Invoice found with is reference, No Invoice Found", "DOES NOT GENERATE AN E-INVOICE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        Dim reqPlGenIRN As ReqPlGenIRN = New ReqPlGenIRN()

        reqPlGenIRN.Version = "1.1"
        reqPlGenIRN.TranDtls = New ReqPlGenIRN.TranDetails()
        reqPlGenIRN.TranDtls.TaxSch = "GST"
        reqPlGenIRN.TranDtls.SupTyp = "B2B"    ' "EXPWP"


        reqPlGenIRN.DocDtls = New ReqPlGenIRN.DocSetails()
        reqPlGenIRN.DocDtls.Typ = DocDetailType
        If InStr(1, Trim(dt.Rows(0).Item("e_Invoice_No").ToString), " ") > 0 Then
            MessageBox.Show("Invalid Document No., Spaces not allowed", "Cannot Generate IRN...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If
        If Left(Trim(dt.Rows(0).Item("e_Invoice_No").ToString), 1) = "0" Then
            MessageBox.Show("Invalid Document No., cannot begin with Zero", "Cannot Generate IRN...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If
        If InStr(1, Trim(dt.Rows(0).Item("e_Invoice_No").ToString), "") > 0 Then
            reqPlGenIRN.DocDtls.No = dt.Rows(0).Item("e_Invoice_No").ToString
        Else
            MessageBox.Show("Invalid Document No., Spaces not allowed", "Cannot Generate IRN...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If
        reqPlGenIRN.DocDtls.Dt = Format(dt.Rows(0).Item("e_Invoice_Date"), "dd/MM/yyyy")
        reqPlGenIRN.SellerDtls = New ReqPlGenIRN.SellerDetails()

        If Len(Trim(dt.Rows(0).Item("Company_GSTINNo").ToString)) = 15 Then
            reqPlGenIRN.SellerDtls.Gstin = dt.Rows(0).Item("Company_GSTINNo").ToString
        Else
            MessageBox.Show("Invalid Supplier GSTIN . Cannot Generate IRN", "Provide GSTIN", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Len(Trim(dt.Rows(0).Item("Legal_Nameof_Business").ToString)) >= 3 Then
            reqPlGenIRN.SellerDtls.LglNm = dt.Rows(0).Item("Legal_Nameof_Business").ToString
        Else
            reqPlGenIRN.SellerDtls.LglNm = dt.Rows(0).Item("Company_Name").ToString
            'MessageBox.Show("Invalid Supplier Legal Name. Cannot Generate IRN", "Provide Legal Name", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            'Exit Sub
        End If

        If Len(Trim(dt.Rows(0).Item("Company_Name").ToString)) >= 3 Then
            reqPlGenIRN.SellerDtls.TrdNm = dt.Rows(0).Item("Company_Name").ToString
        Else
            MessageBox.Show("Invalid Company Name", "Cannot Generate IRN...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If


        Dim Cmp_Add As String = ""
        Cmp_Add = Trim(dt.Rows(0).Item("Company_Address1").ToString)
        Cmp_Add = Cmp_Add.TrimEnd(",")

        If Len(Trim(Cmp_Add)) > 0 And Len(Trim(dt.Rows(0).Item("Company_Address2").ToString)) > 10 Then
            Cmp_Add = Cmp_Add + "," + Trim(dt.Rows(0).Item("Company_Address2").ToString)
        End If

        If Len(Trim(Cmp_Add)) > 1 Then
            reqPlGenIRN.SellerDtls.Addr1 = Microsoft.VisualBasic.Left(Trim(Cmp_Add), 100)
        Else
            MessageBox.Show("Invalid Supplier Address. Cannot Generate IRN", "Provide Address", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        Cmp_Add = ""
        Cmp_Add = Trim(dt.Rows(0).Item("Company_Address3").ToString)
        Cmp_Add = Cmp_Add.TrimEnd(",")

        If Len(Trim(Cmp_Add)) > 0 And Len(Trim(dt.Rows(0).Item("Company_Address4").ToString)) > 0 Then
            Cmp_Add = Cmp_Add + "," + Trim(dt.Rows(0).Item("Company_Address4").ToString)
        End If

        If Len(Trim(Cmp_Add)) > 0 Then
            reqPlGenIRN.SellerDtls.Addr2 = Microsoft.VisualBasic.Left(Trim(Cmp_Add), 100)
        End If

        If Len(Trim(dt.Rows(0).Item("Company_City").ToString)) >= 3 Then
            reqPlGenIRN.SellerDtls.Loc = Trim(dt.Rows(0).Item("Company_City").ToString)
        Else
            MessageBox.Show("Invalid City/Town provided for Seller. Cannot Generate IRN", "Provide Location", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        Dim vCOMPPINCODE As String = ""

        vCOMPPINCODE = Replace(Trim(dt.Rows(0).Item("Company_PinCode").ToString), " ", "")
        vCOMPPINCODE = Replace(Trim(vCOMPPINCODE), "-", "")
        If Len(Trim(vCOMPPINCODE)) = 6 Then
            reqPlGenIRN.SellerDtls.Pin = Val(vCOMPPINCODE)
        Else
            MessageBox.Show("Invalid PINCODE provided For Seller. Cannot Generate IRN", "Provide Town/City", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Val(Trim(dt.Rows(0).Item("State_Code").ToString)) <> 0 And Len(Trim(dt.Rows(0).Item("State_Code").ToString)) > 0 Then
            reqPlGenIRN.SellerDtls.Stcd = Trim(dt.Rows(0).Item("State_Code").ToString)
        Else
            MessageBox.Show("Invalid State provided For Seller. Cannot Generate IRN", "Provide State", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Len(Trim(dt.Rows(0).Item("Company_PhoneNo").ToString)) > 0 Then


            vPHNO = Trim(dt.Rows(0).Item("Company_PhoneNo").ToString)
            vPHNO = Replace(Trim(vPHNO), " ", "")
            vPHNO = Replace(Trim(vPHNO), "-", "")
            vPHNO = Replace(Trim(vPHNO), "/", "")
            vPHNO = Replace(Trim(vPHNO), "\", "")
            vPHNO = Replace(Trim(vPHNO), "*", "")

            arPHNO = Split(Trim(vPHNO), ",")

            vPHNO_TO_IRN = ""
            If UBound(arPHNO) >= 0 Then
                vPHNO_TO_IRN = arPHNO(0)
            End If
            If Trim(vPHNO_TO_IRN) <> "" Then
                If IsNumeric(vPHNO_TO_IRN) = True Then
                    reqPlGenIRN.SellerDtls.Ph = Trim(vPHNO_TO_IRN)   ' dt.Rows(0).Item("Company_PhoneNo").ToString
                End If
            End If

        End If

        If Len(Trim(dt.Rows(0).Item("Company_Email").ToString)) > 0 Then
            reqPlGenIRN.SellerDtls.Em = dt.Rows(0).Item("Company_Email").ToString
        End If

        dt.Rows.Clear()

        'Buyer

        da = New SqlClient.SqlDataAdapter("Select  a.*, L.*, s.* From e_Invoice_Head a inner Join " &
                                               "Ledger_Head L On a.Buyer_IdNo <> 0 and a.Buyer_IdNo = L.Ledger_IdNo inner Join State_Head s on L.Ledger_State_IdNo = s.State_IdNo Where Ref_Sales_Code = '" & Trim(InvCode.ToString) & "'", Cn)
        dt = New DataTable
        da.Fill(dt)

        reqPlGenIRN.BuyerDtls = New ReqPlGenIRN.BuyerDetails()

        If Len(Trim(dt.Rows(0).Item("Ledger_GSTINNo").ToString)) = 15 Or Trim(UCase(dt.Rows(0).Item("Ledger_GSTINNo").ToString)) = "URP" Then
            reqPlGenIRN.BuyerDtls.Gstin = dt.Rows(0).Item("Ledger_GSTINNo").ToString
        Else
            MessageBox.Show("Invalid Buyer GSTIN . Cannot Generate IRN", "Provide GSTIN", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Len(Trim(dt.Rows(0).Item("Legal_Nameof_Business").ToString)) > 0 Then
            reqPlGenIRN.BuyerDtls.LglNm = dt.Rows(0).Item("Legal_Nameof_Business").ToString
        Else
            reqPlGenIRN.BuyerDtls.LglNm = dt.Rows(0).Item("Ledger_MainName").ToString
            'MessageBox.Show("Invalid Legal Name of Buyer . Cannot Generate IRN", "Provide GSTIN", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            'Exit Sub
        End If

        If Len(Trim(dt.Rows(0).Item("Ledger_MainName").ToString)) > 0 Then
            reqPlGenIRN.BuyerDtls.TrdNm = dt.Rows(0).Item("Ledger_MainName").ToString
        Else
            MessageBox.Show("Invalid Buyer Name . Cannot Generate IRN", "Provide BUYER NAME", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If


        If Val(Trim(dt.Rows(0).Item("State_Code").ToString)) <> 0 And Len(Trim(dt.Rows(0).Item("State_Code").ToString)) > 0 Then
            reqPlGenIRN.BuyerDtls.Pos = dt.Rows(0).Item("State_Code").ToString

        Else
            MessageBox.Show("Invalid Buyer State Code . Cannot Generate IRN", "Provide State Code", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Val(reqPlGenIRN.BuyerDtls.Pos) = 96 Then
            reqPlGenIRN.TranDtls.SupTyp = "EXPWP"   '"B2B"    
        End If

        Dim Buy_Add As String

        Buy_Add = Trim(dt.Rows(0).Item("Ledger_Address1").ToString)
        Buy_Add = Buy_Add.TrimEnd(",")
        If Len(Trim(Buy_Add)) > 0 And Len(Trim(dt.Rows(0).Item("Ledger_Address2").ToString)) > 0 Then
            Buy_Add = Buy_Add + "," + Trim(dt.Rows(0).Item("Ledger_Address2").ToString)
        End If

        If Len(Trim(Buy_Add)) > 1 Then
            reqPlGenIRN.BuyerDtls.Addr1 = Microsoft.VisualBasic.Left(Trim(Buy_Add), 100)
        Else
            MessageBox.Show("Invalid Buyer Address . Cannot Generate IRN", "Provide Address", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        Buy_Add = ""
        Buy_Add = Trim(dt.Rows(0).Item("Ledger_Address3").ToString)
        Buy_Add = Buy_Add.TrimEnd(",")

        If Len(Trim(Buy_Add)) > 0 And Len(Trim(dt.Rows(0).Item("Ledger_Address4").ToString)) > 0 Then
            Buy_Add = Buy_Add + "," + Trim(dt.Rows(0).Item("Ledger_Address4").ToString)
        End If

        If Len(Trim(Buy_Add)) > 0 Then
            reqPlGenIRN.BuyerDtls.Addr2 = Microsoft.VisualBasic.Left(Trim(Buy_Add), 100)
        End If

        If Len(Trim(dt.Rows(0).Item("City_Town").ToString)) > 0 Then
            reqPlGenIRN.BuyerDtls.Loc = Trim(dt.Rows(0).Item("City_Town").ToString)
        Else
            MessageBox.Show("Invalid Buyer City / Town . Cannot Generate IRN", "Provide City/Town ", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        Dim vLEDPINCODE As String = ""

        vLEDPINCODE = Replace(Trim(dt.Rows(0).Item("PINCODE").ToString), " ", "")
        vLEDPINCODE = Replace(Trim(vLEDPINCODE), "-", "")

        If Len(Trim(vLEDPINCODE)) = 6 Then
            reqPlGenIRN.BuyerDtls.Pin = Val(vLEDPINCODE)
        Else
            MessageBox.Show("Invalid Buyer PINCODE . Cannot Generate IRN", "Provide PINCODE ", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Val(Trim(dt.Rows(0).Item("State_Code").ToString)) <> 0 And Len(Trim(dt.Rows(0).Item("State_Code").ToString)) > 0 Then
            reqPlGenIRN.BuyerDtls.Stcd = dt.Rows(0).Item("State_Code").ToString
        Else
            MessageBox.Show("Invalid Buyer State Code . Cannot Generate IRN", "Provide State Code ", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If


        If Len(Trim(dt.Rows(0).Item("Ledger_PhoneNo").ToString)) > 0 Then

            vPHNO = Trim(dt.Rows(0).Item("Ledger_PhoneNo").ToString)
            vPHNO = Replace(Trim(vPHNO), " ", "")
            vPHNO = Replace(Trim(vPHNO), "-", "")
            vPHNO = Replace(Trim(vPHNO), "/", "")
            vPHNO = Replace(Trim(vPHNO), "\", "")
            vPHNO = Replace(Trim(vPHNO), "*", "")

            arPHNO = Split(Trim(vPHNO), ",")

            vPHNO_TO_IRN = ""
            If UBound(arPHNO) >= 0 Then
                vPHNO_TO_IRN = arPHNO(0)
            End If
            If Trim(vPHNO_TO_IRN) <> "" Then
                If IsNumeric(vPHNO_TO_IRN) = True Then
                    reqPlGenIRN.BuyerDtls.Ph = Trim(vPHNO_TO_IRN)   'dt.Rows(0).Item("Ledger_PhoneNo").ToString
                End If
            End If

            'reqPlGenIRN.BuyerDtls.Ph = dt.Rows(0).Item("Ledger_PhoneNo").ToString

        End If

        If Len(Trim(dt.Rows(0).Item("Ledger_Mail").ToString)) > 0 Then
            reqPlGenIRN.BuyerDtls.Em = dt.Rows(0).Item("Ledger_Mail").ToString
        End If

        dt.Rows.Clear()



        'Dispatcher

        da = New SqlClient.SqlDataAdapter("Select  a.*, L.*, s.* From e_Invoice_Head a inner Join " &
                                               "Ledger_Head L On a.Dispatcher_IdNo <> 0 and a.Dispatcher_IdNo = L.Ledger_IdNo inner Join State_Head s on L.Ledger_State_IdNo = s.State_IdNo Where Ref_Sales_Code = '" & Trim(InvCode.ToString) & "'", Cn)
        dt = New DataTable
        da.Fill(dt)

        If dt.Rows.Count > 0 Then

            reqPlGenIRN.DispDtls = New ReqPlGenIRN.DispatchedDetails()

            If Len(Trim(dt.Rows(0).Item("Ledger_MainName").ToString)) > 0 Then
                reqPlGenIRN.DispDtls.Nm = dt.Rows(0).Item("Ledger_MainName").ToString
            Else
                MessageBox.Show("Invalid Dispatcher Name . Cannot Generate IRN", "Provide DISPATCHER NAME", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub
            End If

            Dim Disp_Add As String

            Disp_Add = Trim(dt.Rows(0).Item("Ledger_Address1").ToString)
            Disp_Add = Disp_Add.TrimEnd(",")

            If Len(Trim(Disp_Add)) > 0 And Len(Trim(dt.Rows(0).Item("Ledger_Address2").ToString)) > 0 Then
                Disp_Add = Disp_Add + "," + Trim(dt.Rows(0).Item("Ledger_Address2").ToString)
            End If

            If Len(Trim(Disp_Add)) > 1 Then
                reqPlGenIRN.DispDtls.Addr1 = Microsoft.VisualBasic.Left(Trim(Disp_Add), 100)
            Else
                MessageBox.Show("Invalid Dispatcher Address . Cannot Generate IRN", "Provide Dispatcher Address", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub
            End If

            Disp_Add = ""
            Disp_Add = Trim(dt.Rows(0).Item("Ledger_Address3").ToString)
            Disp_Add = Disp_Add.TrimEnd(",")

            If Len(Trim(Disp_Add)) > 0 And Len(Trim(dt.Rows(0).Item("Ledger_Address4").ToString)) > 0 Then
                Disp_Add = Disp_Add + "," + Trim(dt.Rows(0).Item("Ledger_Address4").ToString)
            End If

            If Len(Trim(Disp_Add)) > 0 Then
                reqPlGenIRN.DispDtls.Addr2 = Microsoft.VisualBasic.Left(Trim(Disp_Add), 100)
            End If

            If Len(Trim(dt.Rows(0).Item("City_Town").ToString)) > 0 Then
                reqPlGenIRN.DispDtls.Loc = Trim(dt.Rows(0).Item("City_Town").ToString)
            Else
                MessageBox.Show("Invalid Dispatcher City / Town . Cannot Generate IRN", "Provide Dispatcher City/Town ", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub
            End If


            Dim vDISPLEDPINCODE As String = ""

            vDISPLEDPINCODE = Replace(Trim(dt.Rows(0).Item("PINCODE").ToString), " ", "")
            vDISPLEDPINCODE = Replace(Trim(vDISPLEDPINCODE), "-", "")

            If Len(Trim(vDISPLEDPINCODE)) = 6 Then
                reqPlGenIRN.DispDtls.Pin = Val(vDISPLEDPINCODE)
            Else
                MessageBox.Show("Invalid Dispatcher PINCODE . Cannot Generate IRN", "Provide Dispatcher PINCODE ", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub
            End If

            If Val(Trim(dt.Rows(0).Item("State_Code").ToString)) <> 0 And Len(Trim(dt.Rows(0).Item("State_Code").ToString)) > 0 Then
                reqPlGenIRN.DispDtls.Stcd = dt.Rows(0).Item("State_Code").ToString
            Else
                MessageBox.Show("Invalid Dispatcher State Code . Cannot Generate IRN", "Provide Dispatcher State Code ", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub
            End If

        End If

        dt.Rows.Clear()

        'reqPlGenIRN.DispDtls = New ReqPlGenIRN.DispatchedDetails()
        'reqPlGenIRN.DispDtls.Nm = reqPlGenIRN.SellerDtls.TrdNm
        'reqPlGenIRN.DispDtls.Addr1 = reqPlGenIRN.SellerDtls.Addr1

        'If Len(Trim(reqPlGenIRN.SellerDtls.Addr2)) > 0 Then
        'reqPlGenIRN.DispDtls.Addr2 = reqPlGenIRN.SellerDtls.Addr2
        'End If

        'reqPlGenIRN.DispDtls.Loc = reqPlGenIRN.SellerDtls.Loc
        'reqPlGenIRN.DispDtls.Pin = reqPlGenIRN.SellerDtls.Pin
        'reqPlGenIRN.DispDtls.Stcd = reqPlGenIRN.SellerDtls.Stcd

        da = New SqlClient.SqlDataAdapter("Select  a.*,L.*,s.* from e_Invoice_Head a inner join " &
                                               "Ledger_Head L on a.Consignee_IdNo <> 0 and a.Consignee_IdNo = L.Ledger_IdNo inner join State_Head s on L.Ledger_State_IdNo = s.State_IdNo where Ref_Sales_Code = '" & InvCode.ToString & "' and not a.Consignee_IdNo = 0 and a.Consignee_IdNo<>a.Buyer_IdNo ", Cn)
        dt = New DataTable
        da.Fill(dt)

        If dt.Rows.Count > 0 Then

            reqPlGenIRN.ShipDtls = New ReqPlGenIRN.ShippedDetails()
            If Len(Trim(dt.Rows(0).Item("Ledger_GSTINNo").ToString)) = 15 Or Trim(UCase(dt.Rows(0).Item("Ledger_GSTINNo").ToString)) = "URP" Then
                reqPlGenIRN.ShipDtls.Gstin = dt.Rows(0).Item("Ledger_GSTINNo").ToString
            End If

            If Len(Trim(dt.Rows(0).Item("Legal_Nameof_Business").ToString)) > 0 Then
                reqPlGenIRN.ShipDtls.LglNm = dt.Rows(0).Item("Legal_Nameof_Business").ToString
            Else
                reqPlGenIRN.ShipDtls.LglNm = dt.Rows(0).Item("Ledger_MainName").ToString
                'MessageBox.Show("Invalid Shipped Party Legal Name. Cannot Generate IRN", "Provide Legal Name ", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                'Exit Sub
            End If

            reqPlGenIRN.ShipDtls.TrdNm = dt.Rows(0).Item("Ledger_MainName").ToString

            Dim Con_Add As String

            Con_Add = Trim(dt.Rows(0).Item("Ledger_Address1").ToString)
            Con_Add = Con_Add.TrimEnd(",")

            If Len(Trim(Con_Add)) > 0 And Len(Trim(dt.Rows(0).Item("Ledger_Address2").ToString)) > 0 Then
                Con_Add = Con_Add + "," + Trim(dt.Rows(0).Item("Ledger_Address2").ToString)
            End If

            If Len(Trim(Con_Add)) > 2 Then
                reqPlGenIRN.ShipDtls.Addr1 = Con_Add
            Else
                MessageBox.Show("Invalid Shipped Party LAddress. Cannot Generate IRN", "Provide Address ", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub
            End If

            Con_Add = ""
            Con_Add = Trim(dt.Rows(0).Item("Ledger_Address3").ToString)
            Con_Add = Con_Add.TrimEnd(",")

            If Len(Trim(Con_Add)) > 0 And Len(Trim(dt.Rows(0).Item("Ledger_Address4").ToString)) > 0 Then
                Con_Add = Con_Add + "," + Trim(dt.Rows(0).Item("Ledger_Address4").ToString)
            End If

            If Len(Trim(Con_Add)) > 0 Then
                reqPlGenIRN.ShipDtls.Addr2 = Con_Add
            End If

            If Len(Trim(dt.Rows(0).Item("City_Town").ToString)) > 0 Then
                reqPlGenIRN.ShipDtls.Loc = dt.Rows(0).Item("City_Town").ToString ' dt.Rows(0).Item("Place").ToString
            Else
                MessageBox.Show("Invalid Shipped Party City/Town. Cannot Generate IRN", "Provide City/Town ", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub
            End If

            Dim vSHIPPINCODE As String = ""

            vSHIPPINCODE = Replace(Trim(dt.Rows(0).Item("Pincode").ToString), " ", "")
            vSHIPPINCODE = Replace(Trim(vSHIPPINCODE), "-", "")

            If Len(Trim(Val(vSHIPPINCODE))) > 0 Then
                reqPlGenIRN.ShipDtls.Pin = Val(vSHIPPINCODE)  ' Val(dt.Rows(0).Item("Pincode").ToString) 'Val(dt.Rows(0).Item("PIN").ToString)
            Else
                MessageBox.Show("Invalid Shipped Party PINCODE . Cannot Generate IRN", "Provide PINCODE ", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub
            End If

            If Val(Trim(Val(dt.Rows(0).Item("State_Code").ToString))) <> 0 And Len(Trim(Val(dt.Rows(0).Item("State_Code").ToString))) > 0 Then
                reqPlGenIRN.ShipDtls.Stcd = dt.Rows(0).Item("State_Code").ToString
            Else
                MessageBox.Show("Invalid Shipped Party State Name/Code . Cannot Generate IRN", "Provide State name ", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub
            End If



            'reqPlGenIRN.ShipDtls = New ReqPlGenIRN.ShippedDetails()

            'reqPlGenIRN.ShipDtls.Gstin = reqPlGenIRN.BuyerDtls.Gstin
            'reqPlGenIRN.ShipDtls.LglNm = reqPlGenIRN.BuyerDtls.LglNm
            'reqPlGenIRN.ShipDtls.TrdNm = reqPlGenIRN.BuyerDtls.TrdNm
            'reqPlGenIRN.ShipDtls.Addr1 = reqPlGenIRN.BuyerDtls.Addr1

            'If Len(Trim(reqPlGenIRN.BuyerDtls.Addr2)) > 0 Then
            '    reqPlGenIRN.ShipDtls.Addr2 = reqPlGenIRN.BuyerDtls.Addr2
            'End If

            'reqPlGenIRN.ShipDtls.Loc = reqPlGenIRN.BuyerDtls.Loc
            'reqPlGenIRN.ShipDtls.Pin = reqPlGenIRN.BuyerDtls.Pin
            'reqPlGenIRN.ShipDtls.Stcd = reqPlGenIRN.BuyerDtls.Stcd



        End If


        dt.Rows.Clear()

        da = New SqlClient.SqlDataAdapter("Select * from e_Invoice_Details  where Ref_Sales_Code = '" & InvCode.ToString & "' Order by Sl_No", Cn)
        dt = New DataTable
        da.Fill(dt)

        reqPlGenIRN.ItemList = New List(Of ReqPlGenIRN.ItmList)()

        For I As Integer = 0 To dt.Rows.Count - 1

            Dim itm As ReqPlGenIRN.ItmList = New ReqPlGenIRN.ItmList()

            If Val(dt.Rows(I).Item("Sl_No")) > 0 Then
                itm.SlNo = Val(dt.Rows(I).Item("Sl_No"))
            Else
                MessageBox.Show("Invalid Serial Number. Cannot Generate IRN", "Provide Serial Number ", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub
            End If


            If Len(Trim(dt.Rows(I).Item("Product_Description").ToString)) >= 3 Then
                itm.PrdDesc = dt.Rows(I).Item("Product_Description").ToString
            Else
                MessageBox.Show("Invalid Product Description at Serial No " & Val(dt.Rows(I).Item("Sl_No")) & ". Cannot Generate IRN", "Provide Serial Number ", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub
            End If

            If Len(Trim(dt.Rows(I).Item("HSN_Code").ToString)) >= 2 Then

                itm.HsnCd = dt.Rows(I).Item("HSN_Code").ToString

                If Microsoft.VisualBasic.Left(Trim(dt.Rows(I).Item("HSN_Code").ToString), 2) = "99" Then
                    itm.IsServc = "Y"
                Else
                    itm.IsServc = "N"
                End If
                'itm.IsServc = IIf(dt.Rows(I).Item("IsService") = True, "Y", "N")

            Else
                MessageBox.Show("Invalid HSN Code at Serial No " & Val(dt.Rows(I).Item("Sl_No")) & ". Cannot Generate IRN", "Provide Serial Number ", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub

            End If


            If Val(dt.Rows(I).Item("Quantity")) > 0 Then
                itm.Qty = dt.Rows(I).Item("Quantity")
            Else
                itm.Qty = 1
                'MessageBox.Show("Invalid Quantity at Serial No " & Val(dt.Rows(I).Item("Sl_No")) & ". Cannot Generate IRN", "Provide Serial Number ", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                'Exit Sub
            End If

            If Len(Trim(dt.Rows(I).Item("Unit").ToString)) >= 2 Then
                itm.Unit = dt.Rows(I).Item("Unit")
            Else
                MessageBox.Show("Invalid Unit at Serial No " & Val(dt.Rows(I).Item("Sl_No")) & ".  Cannot Generate IRN", "Provide Serial Number ", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub
            End If

            itm.UnitPrice = dt.Rows(I).Item("Unit_Price")
            itm.TotAmt = dt.Rows(I).Item("Total_Amount")
            itm.Discount = dt.Rows(I).Item("Discount")

            If dt.Rows(I).Item("Assessable_Amount") > 0 Then
                itm.AssAmt = dt.Rows(I).Item("Assessable_Amount")
            End If

            itm.GstRt = dt.Rows(I).Item("GST_Rate")

            If reqPlGenIRN.SellerDtls.Stcd = reqPlGenIRN.BuyerDtls.Stcd Then
                itm.SgstAmt = Format(dt.Rows(I).Item("Assessable_Amount") * dt.Rows(I).Item("GST_Rate") / 100 / 2, "##########0.00")
                itm.CgstAmt = itm.SgstAmt
                itm.IgstAmt = 0.00
            Else
                itm.IgstAmt = Format(dt.Rows(I).Item("Assessable_Amount") * dt.Rows(I).Item("GST_Rate") / 100, "##########0.00")
                'itm.IgstAmt = Math.Round(dt.Rows(I).Item("Assessable_Amount") * dt.Rows(I).Item("GST_Rate") / 100, 2)
                itm.SgstAmt = 0
                itm.CgstAmt = 0
            End If


            itm.CesRt = dt.Rows(I).Item("Cess_Rate")
            itm.CesAmt = dt.Rows(I).Item("Cess_Amount")
            itm.CesNonAdvlAmt = dt.Rows(I).Item("CessNonAdvlAmount")
            itm.StateCesRt = dt.Rows(I).Item("State_Cess_Rate")
            itm.StateCesAmt = dt.Rows(I).Item("State_Cess_Amount")
            itm.StateCesNonAdvlAmt = dt.Rows(I).Item("StateCessNonAdvlAmount")
            itm.OthChrg = dt.Rows(I).Item("Other_Charge")
            itm.TotItemVal = Format(itm.AssAmt + itm.SgstAmt + itm.IgstAmt + itm.CgstAmt + itm.OthChrg + itm.CesAmt + itm.StateCesAmt, "##########0.00")
            'itm.TotItemVal = Math.Round(itm.AssAmt + itm.SgstAmt + itm.IgstAmt + itm.CgstAmt + itm.OthChrg + itm.CesAmt + itm.StateCesAmt, 2)

            If Len(Trim(dt.Rows(I).Item("AttributesDetails"))) Then
                itm.AttribDtls = dt.Rows(I).Item("AttributesDetails")
            End If

            reqPlGenIRN.ItemList.Add(itm)

        Next

        reqPlGenIRN.PayDtls = Nothing
        reqPlGenIRN.RefDtls = Nothing
        reqPlGenIRN.AddlDocDtls = Nothing
        reqPlGenIRN.ExpDtls = Nothing
        reqPlGenIRN.EwbDtls = Nothing


        dt.Rows.Clear()

        da = New SqlClient.SqlDataAdapter("Select * from e_Invoice_Head  where Ref_Sales_Code = '" & InvCode.ToString & "' ", Cn)
        da.Fill(dt)

        reqPlGenIRN.ValDtls = New ReqPlGenIRN.ValDetails()

        reqPlGenIRN.ValDtls.AssVal = Val(dt.Rows(0).Item("Assessable_Value"))
        reqPlGenIRN.ValDtls.CgstVal = Val(dt.Rows(0).Item("CGST"))
        reqPlGenIRN.ValDtls.SgstVal = Val(dt.Rows(0).Item("SGST"))
        reqPlGenIRN.ValDtls.IgstVal = Val(dt.Rows(0).Item("IGST"))
        reqPlGenIRN.ValDtls.CesVal = Val(dt.Rows(0).Item("Cess"))
        reqPlGenIRN.ValDtls.StCesVal = Val(dt.Rows(0).Item("State_Cess"))
        reqPlGenIRN.ValDtls.OthChrg = Val(dt.Rows(0).Item("Other_Charges"))
        reqPlGenIRN.ValDtls.RndOffAmt = Val(dt.Rows(0).Item("Round_Off"))

        reqPlGenIRN.ValDtls.TotInvVal = Format(Val(dt.Rows(0).Item("Nett_Invoice_Value")), "##########0.00")
        'reqPlGenIRN.ValDtls.TotInvVal = Math.Round(Val(dt.Rows(0).Item("Nett_Invoice_Value")), 2)

        Dim txnRespWithObj As TaxProEInvoice.API.TxnRespWithObj(Of RespPlGenIRN)
        txnRespWithObj = Await eInvoiceAPI.GenIRNAsync(eInvSession, reqPlGenIRN)

        Dim respPlGenIRN As RespPlGenIRN = txnRespWithObj.RespObj
        Dim ErrorCodes As String = ""
        Dim ErrorDesc As String = ""

        ResponseObject.Text = ""

        If txnRespWithObj.IsSuccess Then

            ResponseObject.Text = JsonConvert.SerializeObject(respPlGenIRN)

            If Not IsNothing(respPlGenIRN.QrCodeImage) Then
                Dim Fldr_Name As String = Common_Procedures.AppPath & "\IRNQRCODES"

                If System.IO.Directory.Exists(Fldr_Name) = False Then
                    System.IO.Directory.CreateDirectory(Fldr_Name)
                End If
                respPlGenIRN.QrCodeImage.Save(Fldr_Name & "\QRCODE_" & InvCode.Replace("/", "") & ".png")
                'respPlGenIRN.QrCodeImage.Save(Common_Procedures.AppPath & "\IRNQRCODES\" & InvCode.Replace("/", "") & ".png")
                QRCodePictureBox.BackgroundImage = respPlGenIRN.QrCodeImage
            End If

            IRNTextbox.Text = txnRespWithObj.RespObj.Irn
            IRNACKNoTextBox.Text = txnRespWithObj.RespObj.AckNo
            IRNACKDateTextBox.Text = txnRespWithObj.RespObj.AckDt
            IRNCancelStatusTextBox.Text = "Active"


            Try

                Dim CMD As New SqlClient.SqlCommand
                CMD.Connection = Cn

                Dim ms As New MemoryStream()
                If IsNothing(respPlGenIRN.QrCodeImage) = False Then
                    Dim bitmp As New Bitmap(respPlGenIRN.QrCodeImage)
                    bitmp.Save(ms, Drawing.Imaging.ImageFormat.Jpeg)
                End If
                Dim data As Byte() = ms.GetBuffer()
                Dim p As New SqlClient.SqlParameter("@QrCode", SqlDbType.Image)
                p.Value = data
                CMD.Parameters.Add(p)
                ms.Dispose()

                CMD.CommandText = "Update " & InvoiceHeadTable & " SET E_Invoice_IRNO = '" & IRNTextbox.Text & "', E_Invoice_QR_Image =  @QrCode, E_Invoice_ACK_No = '" & IRNACKNoTextBox.Text & "' , E_Invoice_ACK_Date = '" & IRNACKDateTextBox.Text & "'  Where " & InvoiceHeadTableUniqueCode & "  = '" & Trim(InvCode) & "'"
                CMD.ExecuteNonQuery()

                If Trim(IRNTextbox.Text) <> "" Then
                    Nr = 0
                    CMD.CommandText = "Update  " & Trim(Common_Procedures.CompanyDetailsDataBaseName) & "..eInvoice_eWay_Bill_Details SET EInvoice_IRN_No = '" & Trim(IRNTextbox.Text) & "', EInvoice_IRN_QRCode_Image =  @QrCode, EInvoice_ACK_No = '" & Trim(IRNACKNoTextBox.Text) & "' , EInvoice_ACK_Date = '" & Trim(IRNACKDateTextBox.Text) & "'  Where CompanyGroup_IdNo = '" & Trim(Common_Procedures.CompGroupIdNo) & "' and Year_Code = '" & Trim(Common_Procedures.FnYearCode) & "' and Entry_Type = 'EINVOICE' and Document_Code = '" & Trim(Entry_PkCondition) & Trim(InvCode) & "'"
                    Nr = CMD.ExecuteNonQuery()
                    If Nr = 0 Then
                        CMD.CommandText = "Insert into  " & Trim(Common_Procedures.CompanyDetailsDataBaseName) & "..eInvoice_eWay_Bill_Details ( CompanyGroup_IdNo, Year_Code, Entry_Type, Document_Code, EInvoice_IRN_No, EInvoice_IRN_QRCode_Image, EInvoice_ACK_No, EInvoice_ACK_Date, EWay_BillNo, EWay_BillDate) Values ( " & Str(Val(Common_Procedures.CompGroupIdNo)) & " , '" & Trim(Common_Procedures.FnYearCode) & "' , 'EINVOICE' , '" & Trim(Entry_PkCondition) & Trim(InvCode) & "' , '" & Trim(IRNTextbox.Text) & "', @QrCode, '" & Trim(IRNACKNoTextBox.Text) & "' , '" & Trim(IRNACKDateTextBox.Text) & "', '', '' )"
                        CMD.ExecuteNonQuery()
                    End If
                End If


            Catch ex As Exception

                MessageBox.Show("IRN Generated , Problem encountered in saving to Database" & Chr(13) & ex.Message, "ERROR IN SAVING E-INVOICE DETAILS....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End Try


        Else

            'Error has occured
            'Display TxnOutCome in text box - process or show msg to user
            'Process error codes

            If txnRespWithObj.ErrorDetails IsNot Nothing Then

                IRNMessage = ""

                For Each errPl As TaxProEInvoice.API.RespErrDetailsPl In txnRespWithObj.ErrorDetails

                    'Process errPl item here

                    ResponseObject.Text += errPl.ErrorCode & ","
                    ResponseObject.Text += errPl.ErrorCode & ": " + errPl.ErrorMessage + Environment.NewLine

                Next

            Else

                If Not IsNothing(respPlGenIRN.QrCodeImage) Then
                    Dim Fldr_Name As String = Common_Procedures.AppPath & "\IRNQRCODES"

                    If System.IO.Directory.Exists(Fldr_Name) = False Then
                        System.IO.Directory.CreateDirectory(Fldr_Name)
                    End If
                    respPlGenIRN.QrCodeImage.Save(Fldr_Name & "\QRCODE_" & InvCode.Replace("/", "") & ".png")
                    'respPlGenIRN.QrCodeImage.Save(Common_Procedures.AppPath & "\IRNQRCODES\" & InvCode.Replace("/", "") & ".png")
                    QRCodePictureBox.BackgroundImage = txnRespWithObj.RespObj.QrCodeImage
                End If

                If Len(Trim(txnRespWithObj.RespObj.Irn)) > 60 Then
                    IRNTextbox.Text = txnRespWithObj.RespObj.Irn
                    IRNACKNoTextBox.Text = txnRespWithObj.RespObj.AckNo
                    IRNACKDateTextBox.Text = txnRespWithObj.RespObj.AckDt
                    IRNCancelStatusTextBox.Text = "Active"
                    ResponseObject.Text = "Successfully Generated"

                Else

                    IRNTextbox.Text = ""
                    IRNACKNoTextBox.Text = ""
                    IRNACKDateTextBox.Text = ""
                    IRNCancelStatusTextBox.Text = ""
                    ResponseObject.Text = "no response from https://einvoice1.gst.gov.in/ " & Chr(13) & "Please check einvoice website for IRN number (or) Please try later"

                End If

                Try

                    Dim CMD As New SqlClient.SqlCommand
                    CMD.Connection = Cn

                    Dim ms As New MemoryStream()
                    If IsNothing(QRCodePictureBox.BackgroundImage) = False Then
                        Dim bitmp As New Bitmap(QRCodePictureBox.BackgroundImage)
                        bitmp.Save(ms, Drawing.Imaging.ImageFormat.Jpeg)
                    End If
                    Dim data As Byte() = ms.GetBuffer()
                    Dim p As New SqlClient.SqlParameter("@QrCode", SqlDbType.Image)
                    p.Value = data
                    CMD.Parameters.Add(p)
                    ms.Dispose()

                    CMD.CommandText = "Update " & InvoiceHeadTable & " SET E_Invoice_IRNO = '" & Trim(IRNTextbox.Text) & "', E_Invoice_QR_Image =  @QrCode, E_Invoice_ACK_No = '" & IRNACKNoTextBox.Text & "' , E_Invoice_ACK_Date = '" & IRNACKDateTextBox.Text & "'  Where " & InvoiceHeadTableUniqueCode & "  = '" & Trim(InvCode) & "'"
                    CMD.ExecuteNonQuery()

                    If Trim(IRNTextbox.Text) <> "" Then
                        Nr = 0
                        CMD.CommandText = "Update  " & Trim(Common_Procedures.CompanyDetailsDataBaseName) & "..eInvoice_eWay_Bill_Details SET EInvoice_IRN_No = '" & Trim(IRNTextbox.Text) & "', EInvoice_IRN_QRCode_Image =  @QrCode, EInvoice_ACK_No = '" & Trim(IRNACKNoTextBox.Text) & "' , EInvoice_ACK_Date = '" & Trim(IRNACKDateTextBox.Text) & "'  Where CompanyGroup_IdNo = '" & Trim(Common_Procedures.CompGroupIdNo) & "' and Year_Code = '" & Trim(Common_Procedures.FnYearCode) & "' and Entry_Type = 'EINVOICE' and Document_Code = '" & Trim(Entry_PkCondition) & Trim(InvCode) & "'"
                        Nr = CMD.ExecuteNonQuery()
                        If Nr = 0 Then
                            CMD.CommandText = "Insert into  " & Trim(Common_Procedures.CompanyDetailsDataBaseName) & "..eInvoice_eWay_Bill_Details ( CompanyGroup_IdNo, Year_Code, Entry_Type, Document_Code, EInvoice_IRN_No, EInvoice_IRN_QRCode_Image, EInvoice_ACK_No, EInvoice_ACK_Date, EWay_BillNo, EWay_BillDate) Values ( " & Str(Val(Common_Procedures.CompGroupIdNo)) & " , '" & Trim(Common_Procedures.FnYearCode) & "' , 'EINVOICE' , '" & Trim(Entry_PkCondition) & Trim(InvCode) & "' , '" & Trim(IRNTextbox.Text) & "', @QrCode, '" & Trim(IRNACKNoTextBox.Text) & "' , '" & Trim(IRNACKDateTextBox.Text) & "', '', '' )"
                            CMD.ExecuteNonQuery()
                        End If
                    End If


                Catch ex As Exception
                    MessageBox.Show("IRN Generated , Problem encountered in saving to Database" & Chr(13) & ex.Message, "ERROR IN SAVING E-INVOICE DETAILS....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                End Try

            End If


            'Process InfoDetails here
            Dim respInfoDtlsPl As TaxProEInvoice.API.RespInfoDtlsPl = New TaxProEInvoice.API.RespInfoDtlsPl()

            'Serialize Desc object from InfoDtls as per InfCd
            If txnRespWithObj.InfoDetails IsNot Nothing Then

                For Each infoPl As TaxProEInvoice.API.RespInfoDtlsPl In txnRespWithObj.InfoDetails
                    Dim strDupIrnPl = JsonConvert.SerializeObject(infoPl.Desc)   'Convert object type to json string
                    Select Case infoPl.InfCd
                        Case "DUPIRN"
                            Dim dupIrnPl As DupIrnPl = JsonConvert.DeserializeObject(Of DupIrnPl)(strDupIrnPl)
                        Case "EWBERR"
                            Dim ewbErrPl As List(Of EwbErrPl) = JsonConvert.DeserializeObject(Of List(Of EwbErrPl))(strDupIrnPl)
                        Case "ADDNLNFO"
                            'Deserialize infoPl.Desc as string type and then if this string contains json object, it may be desirilized again as per future releases
                            Dim strDesc As String = CStr(infoPl.Desc)
                    End Select
                Next
            End If
        End If

        'txtResponceHdr.Text = "Generate IRN Responce..."

    End Sub

    Public Shared Async Sub GetIRNByDocNo(InvNo As String, ResponseObject As RichTextBox, QRCodePictureBox As PictureBox)
        '---
    End Sub

    Public Shared Async Sub CancelIRNByIRN(IRN As String, ResponseObject As RichTextBox, InvoiceHeadTable As String, InvoiceHeadTableUniqueCode As String, Cn As SqlClient.SqlConnection, IRNCancelStatusTextBox As TextBox, InvCode As String, CancellationRemarks As String)

        'If Not CredentialsProvided_EINV Then
        '    MessageBox.Show("Insufficient Credentials / Information", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '    Exit Sub
        'End If

        If Len(Trim(InvoiceHeadTable)) = 0 Or Len(Trim(InvoiceHeadTableUniqueCode)) = 0 Then
            MessageBox.Show("Provide a valid Invoice Head / Details Table", "Invalid Invoice Table", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
            Exit Sub
        End If

        Dim reqPlCancelIRN As ReqPlCancelIRN = New ReqPlCancelIRN()
        reqPlCancelIRN.CnlRem = CancellationRemarks
        reqPlCancelIRN.CnlRsn = "2"
        reqPlCancelIRN.Irn = IRN

        Dim txnRespWithObj As TaxProEInvoice.API.TxnRespWithObj(Of RespPlCancelIRN) = Await eInvoiceAPI.CancelIRNIRNAsync(eInvSession, reqPlCancelIRN)

        If txnRespWithObj.IsSuccess Then

            ResponseObject.Text = JsonConvert.SerializeObject(txnRespWithObj.RespObj)

            Try

                Dim CMD As New SqlClient.SqlCommand
                CMD.Connection = Cn

                CMD.CommandText = "Update " & InvoiceHeadTable & " SET E_Invoice_Cancelled_Status = 1, E_Invoice_Cancellation_Reason = '" & CancellationRemarks & "' Where " & InvoiceHeadTableUniqueCode & " = '" & Trim(InvCode) & "'"
                CMD.ExecuteNonQuery()

                IRNCancelStatusTextBox.Text = "Cancelled"

            Catch ex As Exception

                MsgBox(ex.Message & ". IRN Deleted . Problem encountered in saving to Database")

            End Try



        Else

            ResponseObject.Text = txnRespWithObj.TxnOutcome

        End If



    End Sub

    Public Shared Async Sub RefresheInvoiceInfoByIRN(IRN As String, Company_IdNo As String, InvCode As String, Cn As SqlClient.SqlConnection, ResponseObject As RichTextBox, QRCodePictureBox As PictureBox, IRNTextbox As TextBox, IRNACKNoTextBox As TextBox, IRNACKDateTextBox As TextBox, IRNCancelStatusTextBox As TextBox, InvoiceHeadTable As String, InvoiceHeadTableUniqueCode As String)

        If Len(Trim(InvoiceHeadTable)) = 0 Or Len(Trim(InvoiceHeadTableUniqueCode)) = 0 Then
            MessageBox.Show("Provide a valid Invoice Head / Details Table", "Invalid Invoice Table", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
            Exit Sub
        End If

        Dim txnRespWithObj As TaxProEInvoice.API.TxnRespWithObj(Of RespPlGenIRN)
        txnRespWithObj = Await eInvoiceAPI.GetEInvDetailsAsync(eInvSession, IRN, 0)

        Dim respPlGenIRN As RespPlGenIRN = txnRespWithObj.RespObj
        Dim ErrorCodes As String = ""
        Dim ErrorDesc As String = ""

        ResponseObject.Text = ""

        If txnRespWithObj.IsSuccess Then

            ResponseObject.Text = JsonConvert.SerializeObject(respPlGenIRN)

            Dim Fldr_Name1 As String = Common_Procedures.AppPath & "\IRNQRCODES"

            If System.IO.Directory.Exists(Fldr_Name1) = False Then
                System.IO.Directory.CreateDirectory(Fldr_Name1)
            End If
            respPlGenIRN.QrCodeImage.Save(Fldr_Name1 & "\QRCODE_" & InvCode.Replace("/", "") & ".png")

            'respPlGenIRN.QrCodeImage.Save(Common_Procedures.AppPath & "\IRNQRCODES\" & InvCode.Replace("/", "") & ".png")

            QRCodePictureBox.BackgroundImage = respPlGenIRN.QrCodeImage

            Try

                Dim CMD As New SqlClient.SqlCommand
                CMD.Connection = Cn

                Dim ms As New MemoryStream()
                If IsNothing(respPlGenIRN.QrCodeImage) = False Then
                    Dim bitmp As New Bitmap(respPlGenIRN.QrCodeImage)
                    bitmp.Save(ms, Drawing.Imaging.ImageFormat.Jpeg)
                End If
                Dim data As Byte() = ms.GetBuffer()
                Dim p As New SqlClient.SqlParameter("@QrCode", SqlDbType.Image)
                p.Value = data
                CMD.Parameters.Add(p)
                ms.Dispose()

                CMD.CommandText = "Update Cotton_Invoice_Head SET E_Invoice_IRNO = '" & IRNTextbox.Text & "', E_Invoice_QR_Image =  @QrCode, E_Invoice_ACK_No = '" & IRNACKNoTextBox.Text & "' , E_Invoice_ACK_Date = '" & IRNACKDateTextBox.Text & "'  Where  Cotton_Invoice_Code = '" & Trim(InvCode) & "'"
                CMD.ExecuteNonQuery()


            Catch ex As Exception

                MsgBox(ex.Message & ". IRN Generated . Problem encountered in saving to Database")

            End Try

            ''Store respPlGenIRN (manditory - AckDate and SignedInvoice) to verify signed invoice
            ''below code is to show how to verify signed invoice

            'respPlGenIRN.QrCodeImage = Nothing
            'Dim txnRespWithObj1 As TxnRespWithObj(Of PARSEIRN) = Await eInvoiceAPI.ParseIrnResp(ResponseObject.Text, QRCodePictureBox.Image.Size)
            'Dim verifyRespPl As VerifyRespPl = New VerifyRespPl()
            'eInvoiceAPI.ParseIrnResp()
            'If txnRespWithObj.IsSuccess Then
            '    verifyRespPl.IsVerified = txnRespWithObj1.RespObj.IsVerified
            '    verifyRespPl.JwtIssuerIRP = txnRespWithObj1.RespObj.JwtIssuerIRP
            '    verifyRespPl.VerifiedWithCertificateEffectiveFrom = txnRespWithObj1.RespObj.VerifiedWithCertificateEffectiveFrom
            '    verifyRespPl.CertificateName = txnRespWithObj1.RespObj.CertificateName
            '    verifyRespPl.CertStartDate = txnRespWithObj1.RespObj.CertStartDate
            '    verifyRespPl.CertExpiryDate = txnRespWithObj1.RespObj.CertExpiryDate
            'End If




        Else

            'Error has occured
            'Display TxnOutCome in text box - process or show msg to user

            'Process error codes
            If txnRespWithObj.ErrorDetails IsNot Nothing Then

                IRNMessage = ""

                For Each errPl As TaxProEInvoice.API.RespErrDetailsPl In txnRespWithObj.ErrorDetails

                    'Process errPl item here

                    ResponseObject.Text += errPl.ErrorCode & ","
                    ResponseObject.Text += errPl.ErrorCode & ": " + errPl.ErrorMessage + Environment.NewLine

                Next

            Else

                Dim Fldr_Name2 As String = Common_Procedures.AppPath & "\IRNQRCODES"

                If System.IO.Directory.Exists(Fldr_Name2) = False Then
                    System.IO.Directory.CreateDirectory(Fldr_Name2)
                End If
                respPlGenIRN.QrCodeImage.Save(Fldr_Name2 & "\QRCODE_" & InvCode.Replace("/", "") & ".png")
                'respPlGenIRN.QrCodeImage.Save(Common_Procedures.AppPath & "\IRNQRCODES\" & InvCode.Replace("/", "") & ".png")
                QRCodePictureBox.BackgroundImage = txnRespWithObj.RespObj.QrCodeImage


                IRNTextbox.Text = txnRespWithObj.RespObj.Irn
                IRNACKNoTextBox.Text = txnRespWithObj.RespObj.AckNo
                IRNACKDateTextBox.Text = txnRespWithObj.RespObj.AckDt
                IRNCancelStatusTextBox.Text = "Active"
                ResponseObject.Text = "Successfully Generated"

                Try

                    Dim CMD As New SqlClient.SqlCommand
                    CMD.Connection = Cn

                    Dim ms As New MemoryStream()
                    If IsNothing(QRCodePictureBox.BackgroundImage) = False Then
                        Dim bitmp As New Bitmap(QRCodePictureBox.BackgroundImage)
                        bitmp.Save(ms, Drawing.Imaging.ImageFormat.Jpeg)
                    End If
                    Dim data As Byte() = ms.GetBuffer()
                    Dim p As New SqlClient.SqlParameter("@QrCode", SqlDbType.Image)
                    p.Value = data
                    CMD.Parameters.Add(p)
                    ms.Dispose()

                    CMD.CommandText = "Update " & InvoiceHeadTable & " SET E_Invoice_IRNO = '" & IRNTextbox.Text & "', E_Invoice_QR_Image =  @QrCode, E_Invoice_ACK_No = '" & IRNACKNoTextBox.Text & "' , E_Invoice_ACK_Date = '" & IRNACKDateTextBox.Text & "'  Where " & InvoiceHeadTableUniqueCode & "  = '" & Trim(InvCode) & "'"
                    CMD.ExecuteNonQuery()

                Catch ex As Exception

                    MessageBox.Show(ex.Message & ". IRN Generated . Problem encountered in saving to Database", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

                End Try

            End If

        End If

    End Sub


    'Public Shared Async Sub GetIRNInfoByDOCNo(IRN As String, ResponseObject As RichTextBox, IRNCancelStatusTextBox As TextBox, Cn As SqlClient.SqlConnection, InvCode As String)


    '    Dim DocType As String = "INV"
    '    Dim DocNum As String = "DOC/1661296396"
    '    Dim DocDate As String = "08/10/2020"
    '    Dim txnRespWithObj As TxnRespWithObj(Of RespPlGenIRN) = Await eInvoiceAPI.GetIRNDetailsByDocDetailsAsync(eInvSession, DocType, DocNum, DocDate)

    '    If txnRespWithObj.IsSuccess Then
    '        rtbResponce.Text = JsonConvert.SerializeObject(txnRespWithObj.RespObj)
    '    Else
    '        rtbResponce.Text = txnRespWithObj.TxnOutcome
    '    End If

    'End Sub


    Public Shared Async Sub GenerateEWBByIRN(INVCODE As String, ResponseObject As RichTextBox, EWBTexBox As TextBox, EWBDateTextBox As TextBox, EWBValidUptoTextBox As TextBox, Cn As SqlClient.SqlConnection, InvoiceHeadTable As String, InvoiceHeadTableUniqueCode As String, EWBCancelledReasonText As TextBox, EWBStatusText As TextBox, Entry_PkCondition As String)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim vTransgstno As String = ""
        Dim Nr As Long
        Dim vNOOF_EINVS As String = 0

        If Trim(Entry_PkCondition) = "" Then
            MessageBox.Show("Invalid Entry Pk-Condition", "DOES NOT GENERATE AN E-WAY BILL....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Val(Common_Procedures.settings.EWayBill_API_TotalCredits_Per_Year) <= 0 Then
            MessageBox.Show("There is no API credits for EWay Bill generation", "DOES NOT GENERATE AN E-WAY BILL....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        da1 = New SqlClient.SqlDataAdapter("select COUNT(*) from " & Trim(Common_Procedures.CompanyDetailsDataBaseName) & "..eInvoice_eWay_Bill_Details Where Year_Code = '" & Trim(Common_Procedures.FnYearCode) & "' and Entry_Type = 'EWAYBILL'", Cn)
        dt1 = New DataTable
        da1.Fill(dt1)

        vNOOF_EINVS = 0
        If dt1.Rows.Count > 0 Then
            If IsDBNull(dt1.Rows(0)(0).ToString) = False Then
                vNOOF_EINVS = dt1.Rows(0)(0).ToString
            End If
        End If
        dt1.Clear()

        If Val(vNOOF_EINVS) >= Val(Common_Procedures.settings.EWayBill_API_TotalCredits_Per_Year) Then
            MessageBox.Show(Common_Procedures.settings.EWayBill_API_TotalCredits_Per_Year & " Credits for EWay Bill API has expired", "DOES NOT GENERATE AN E-WAY BILL....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If


        'If Not CredentialsProvided_EWB Then
        '  MessageBox.Show("Insufficient Credentials / Information", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '   Exit Sub
        'End If

        'If Len(Trim(InvoiceHeadTable)) = 0 Or Len(Trim(InvoiceHeadTableUniqueCode)) = 0 Then
        '  MessageBox.Show("Provide a valid Invoice Head / Details Table", "Invalid Invoice Table", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
        ' Exit Sub
        'End If

        Dim dt As New DataTable
        Dim da As New SqlClient.SqlDataAdapter("Select * from EWB_by_IRN where INVCODE = '" & INVCODE & "' ", Cn)

        da.Fill(dt)

        If dt.Rows.Count = 0 Then
            MessageBox.Show("No Valid Invoice found with is reference, No Invoice Found", "DOES NOT GENERATE AN E-WAY BILL....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        Dim reqPlGenEwbByIRN As ReqPlGenEwbByIRN = New ReqPlGenEwbByIRN()

        reqPlGenEwbByIRN.Irn = dt.Rows(0).Item("IRN")

        vTransgstno = ""

        If IsDBNull(dt.Rows(0).Item("TransId")) = False Then
            vTransgstno = dt.Rows(0).Item("TransId").ToString
        End If

        If Len(Trim(vTransgstno)) = 15 Then

            reqPlGenEwbByIRN.TransId = vTransgstno

            If Not IsDBNull(dt.Rows(0).Item("TransName")) Then
                If Len(Trim(dt.Rows(0).Item("TransName"))) > 0 Then
                    reqPlGenEwbByIRN.TransName = dt.Rows(0).Item("TransName")
                End If
            End If


            If Not IsDBNull(dt.Rows(0).Item("VehicleNo")) Then

                If Len(Trim(dt.Rows(0).Item("VehicleNo"))) > 0 Then

                    reqPlGenEwbByIRN.VehNo = dt.Rows(0).Item("VehicleNo")

                    If Not IsDBNull(dt.Rows(0).Item("VehType")) Then
                        If Len(Trim(dt.Rows(0).Item("VehType"))) > 0 Then
                            reqPlGenEwbByIRN.VehType = dt.Rows(0).Item("VehType")
                        End If
                    End If

                    If Not IsDBNull(dt.Rows(0).Item("TransMode")) Then
                        If Len(Trim(dt.Rows(0).Item("TransMode"))) > 0 Then
                            reqPlGenEwbByIRN.TransMode = dt.Rows(0).Item("TransMode")
                        End If
                    End If

                    If Not IsDBNull(dt.Rows(0).Item("TransDocNo")) Then

                        If Len(Trim(dt.Rows(0).Item("TransDocNo"))) > 0 Then

                            reqPlGenEwbByIRN.TransDocNo = dt.Rows(0).Item("TransDocNo")

                            If Not IsDBNull(dt.Rows(0).Item("TransDocDate")) Then
                                If Len(Trim(dt.Rows(0).Item("TransDocDate"))) > 0 Then
                                    If IsDate(dt.Rows(0).Item("TransDocDate")) Then
                                        If Year(dt.Rows(0).Item("TransDocDate")) <> 1900 Then
                                            reqPlGenEwbByIRN.TransDocDt = Format(Convert.ToDateTime(dt.Rows(0).Item("TransDocDate")), "dd/MM/yyyy")
                                        End If
                                    End If
                                End If
                            End If

                        End If

                    End If

                End If



            End If



        Else



            If Not IsDBNull(dt.Rows(0).Item("VehicleNo")) Then

                If Len(Trim(dt.Rows(0).Item("VehicleNo"))) > 0 Then

                    reqPlGenEwbByIRN.VehNo = dt.Rows(0).Item("VehicleNo")

                    If Not IsDBNull(dt.Rows(0).Item("VehType")) Then
                        If Len(Trim(dt.Rows(0).Item("VehType"))) > 0 Then
                            reqPlGenEwbByIRN.VehType = dt.Rows(0).Item("VehType")
                        End If
                    End If

                    If Not IsDBNull(dt.Rows(0).Item("TransMode")) Then
                        If Len(Trim(dt.Rows(0).Item("TransMode"))) > 0 Then
                            reqPlGenEwbByIRN.TransMode = dt.Rows(0).Item("TransMode")
                        End If
                    End If

                    If Not IsDBNull(dt.Rows(0).Item("TransDocNo")) Then

                        If Len(Trim(dt.Rows(0).Item("TransDocNo"))) > 0 Then

                            reqPlGenEwbByIRN.TransDocNo = dt.Rows(0).Item("TransDocNo")

                            If Not IsDBNull(dt.Rows(0).Item("TransDocDate")) Then
                                If Len(Trim(dt.Rows(0).Item("TransDocDate"))) > 0 Then
                                    If IsDate(dt.Rows(0).Item("TransDocDate")) Then
                                        If Year(dt.Rows(0).Item("TransDocDate")) <> 1900 Then
                                            reqPlGenEwbByIRN.TransDocDt = Format(Convert.ToDateTime(dt.Rows(0).Item("TransDocDate")), "dd/MM/yyyy")
                                        End If
                                    End If
                                End If

                            End If

                        End If

                    End If


                Else

                    If Not IsDBNull(dt.Rows(0).Item("TransName")) Then
                        If Len(Trim(dt.Rows(0).Item("TransName"))) > 0 Then
                            MessageBox.Show("Provide a valid GSTIN for transport (in transport creation), if not, provide the vehicle number.", "TRANSPORT GSTIN REQUIRED", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                            Exit Sub
                        End If
                    End If

                End If

            Else

                If Not IsDBNull(dt.Rows(0).Item("TransName")) Then
                    If Len(Trim(dt.Rows(0).Item("TransName"))) > 0 Then
                        MessageBox.Show("Provide a valid GSTIN for transport (in transport creation), if not, provide the vehicle number.", "TRANSPORT GSTIN REQUIRED", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If

            End If

        End If

        If Len(Trim(reqPlGenEwbByIRN.TransId)) = 0 And Len(Trim(reqPlGenEwbByIRN.VehNo)) = 0 Then
            MessageBox.Show("Vehicle number has to be provided when TransporterName is not provided. ", "Vehicle Number?", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
            Exit Sub
        End If

        If Len(Trim(reqPlGenEwbByIRN.VehNo)) > 0 Then
            If Val(dt.Rows(0).Item("TransMode")) = 0 Or Len(Trim(dt.Rows(0).Item("TransMode"))) = 0 Then
                MessageBox.Show("Transport mode has to be provided, when Vehicle number is provided. ", "Transport Mode ?", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
                Exit Sub
            End If
        End If

        Dim vDISTANCE As String
        Dim vCOMP_PINCODE As String
        Dim vSHIP_PINCODE As String

        vDISTANCE = 0
        If Not IsDBNull(dt.Rows(0).Item("Distance")) Then
            vDISTANCE = dt.Rows(0).Item("Distance")
        End If
        vCOMP_PINCODE = 0
        If Not IsDBNull(dt.Rows(0).Item("Company_Pincode")) Then
            vCOMP_PINCODE = dt.Rows(0).Item("Company_Pincode")
        End If
        vSHIP_PINCODE = 0
        If Not IsDBNull(dt.Rows(0).Item("Shipped_To_Pincode")) Then
            vSHIP_PINCODE = dt.Rows(0).Item("Shipped_To_Pincode")
        End If

        If Val(vDISTANCE) > 0 Then
            reqPlGenEwbByIRN.Distance = dt.Rows(0).Item("Distance")
        ElseIf Val(Trim(vCOMP_PINCODE)) = Val(Trim(vSHIP_PINCODE)) Then
            reqPlGenEwbByIRN.Distance = 5 ' dt.Rows(0).Item("Distance")
        Else
            reqPlGenEwbByIRN.Distance = 0 ' dt.Rows(0).Item("Distance")
        End If


        Dim txnRespWithObj As TaxProEInvoice.API.TxnRespWithObj(Of RespPlGenEwbByIRN) = Await eInvoiceAPI.GenEwbByIRNAsync(eInvSession, reqPlGenEwbByIRN)
        Dim ErrorCodes As String = ""
        Dim ErrorDesc As String = ""

        If txnRespWithObj.IsSuccess Then

            ResponseObject.Text = JsonConvert.SerializeObject(txnRespWithObj.RespObj)

            EWBTexBox.Text = txnRespWithObj.RespObj.EwbNo
            EWBDateTextBox.Text = txnRespWithObj.RespObj.EwbDt
            EWBValidUptoTextBox.Text = txnRespWithObj.RespObj.EwbValidTill
            EWBCancelledReasonText.Text = ""
            EWBStatusText.Text = "Active"

            Try

                Dim CMD As New SqlClient.SqlCommand
                CMD.Connection = Cn

                CMD.CommandText = "Update " & InvoiceHeadTable & " SET EWB_No = '" & Trim(EWBTexBox.Text) & "',EWB_Date = '" & Trim(EWBDateTextBox.Text) & "'" &
                                      " , EWB_Valid_Upto = '" & Trim(EWBValidUptoTextBox.Text) & "',EWB_Cancelled = 0, EWBCancellation_Reason = '' Where " & InvoiceHeadTableUniqueCode & "  = '" & Trim(INVCODE) & "'"
                CMD.ExecuteNonQuery()

                If Trim(EWBTexBox.Text) <> "" Then
                    Nr = 0
                    CMD.CommandText = "Update  " & Trim(Common_Procedures.CompanyDetailsDataBaseName) & "..eInvoice_eWay_Bill_Details SET EWay_BillNo = '" & Trim(EWBTexBox.Text) & "', EWay_BillDate = '" & Trim(EWBDateTextBox.Text) & "' Where CompanyGroup_IdNo = '" & Trim(Common_Procedures.CompGroupIdNo) & "' and Year_Code = '" & Trim(Common_Procedures.FnYearCode) & "' and Entry_Type = 'EWAYBILL' and Document_Code = '" & Trim(Entry_PkCondition) & Trim(INVCODE) & "'"
                    Nr = CMD.ExecuteNonQuery()
                    If Nr = 0 Then
                        CMD.CommandText = "Insert into  " & Trim(Common_Procedures.CompanyDetailsDataBaseName) & "..eInvoice_eWay_Bill_Details ( CompanyGroup_IdNo, Year_Code, Entry_Type, Document_Code, EInvoice_IRN_No, EInvoice_ACK_No, EInvoice_ACK_Date, EWay_BillNo, EWay_BillDate) Values ( " & Str(Val(Common_Procedures.CompGroupIdNo)) & " , '" & Trim(Common_Procedures.FnYearCode) & "' , 'EWAYBILL' , '" & Trim(Entry_PkCondition) & Trim(INVCODE) & "' , '', '' , '', '" & Trim(EWBTexBox.Text) & "', '" & Trim(EWBDateTextBox.Text) & "' )"
                        CMD.ExecuteNonQuery()
                    End If
                End If

            Catch ex As Exception

                MessageBox.Show("EWB Generated , Problem encountered in saving to Database" & Chr(13) & ex.Message, "ERROR IN SAVING E-WAY BILL DETAILS....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End Try

        Else

            ' rtbResponce.Text = txnRespWithObj.TxnOutcome;
            If txnRespWithObj.ErrorDetails IsNot Nothing Then

                For Each errPl As TaxProEInvoice.API.RespErrDetailsPl In txnRespWithObj.ErrorDetails
                    'Process errPl item here
                    ErrorCodes += errPl.ErrorCode & ","
                    ErrorDesc += errPl.ErrorCode & ": " + errPl.ErrorMessage + Environment.NewLine
                    ResponseObject.Text = ErrorDesc
                Next

            Else

                Try

                    Dim CMD As New SqlClient.SqlCommand
                    CMD.Connection = Cn

                    EWBTexBox.Text = txnRespWithObj.RespObj.EwbNo
                    EWBDateTextBox.Text = txnRespWithObj.RespObj.EwbDt
                    EWBValidUptoTextBox.Text = txnRespWithObj.RespObj.EwbValidTill
                    EWBCancelledReasonText.Text = ""
                    EWBStatusText.Text = "Active"

                    CMD.CommandText = "Update " & InvoiceHeadTable & " SET EWB_No = '" & Trim(EWBTexBox.Text) & "', EWB_Date = '" & EWBDateTextBox.Text & "'" &
                                      " , EWB_Valid_Upto = '" & EWBValidUptoTextBox.Text & "',EWB_Cancelled = 0, EWBCancellation_Reason = ''  Where " & InvoiceHeadTableUniqueCode & "  = '" & Trim(INVCODE) & "'"
                    CMD.ExecuteNonQuery()

                    If Trim(EWBTexBox.Text) <> "" Then
                        Nr = 0
                        CMD.CommandText = "Update " & Trim(Common_Procedures.CompanyDetailsDataBaseName) & "..eInvoice_eWay_Bill_Details SET EWay_BillNo = '" & Trim(EWBTexBox.Text) & "', EWay_BillDate = '" & Trim(EWBDateTextBox.Text) & "' Where CompanyGroup_IdNo = '" & Trim(Common_Procedures.CompGroupIdNo) & "' and Year_Code = '" & Trim(Common_Procedures.FnYearCode) & "' and Entry_Type = 'EWAYBILL' and Document_Code = '" & Trim(Entry_PkCondition) & Trim(INVCODE) & "'"
                        Nr = CMD.ExecuteNonQuery()
                        If Nr = 0 Then
                            CMD.CommandText = "Insert into " & Trim(Common_Procedures.CompanyDetailsDataBaseName) & "..eInvoice_eWay_Bill_Details (                       CompanyGroup_IdNo           ,                             Year_Code        , Entry_Type ,             Document_Code                         , EInvoice_IRN_No, EInvoice_ACK_No,  EInvoice_ACK_Date ,               EWay_BillNo     ,              EWay_BillDate          ) " &
                                                " Values                                                                                          ( " & Str(Val(Common_Procedures.CompGroupIdNo)) & " , '" & Trim(Common_Procedures.FnYearCode) & "' , 'EWAYBILL' , '" & Trim(Entry_PkCondition) & Trim(INVCODE) & "' ,         ''     ,           ''   ,           ''       , '" & Trim(EWBTexBox.Text) & "', '" & Trim(EWBDateTextBox.Text) & "' ) "
                            CMD.ExecuteNonQuery()
                        End If
                    End If

                Catch ex As Exception
                    MessageBox.Show("EWB Generated , Problem encountered in saving to Database" & Chr(13) & ex.Message, "ERROR IN SAVING E-WAY BILL DETAILS....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                End Try

            End If

        End If

    End Sub

    Public Shared Async Sub Cancel_EWB_IRN(INVCODE As String, EWBNo As String, ResponseObject As RichTextBox, EWBCANCELTextBox As TextBox, Cn As SqlClient.SqlConnection, InvoiceHeadTable As String, InvoiceHeadTableUniqueCode As String, CancellationRemarks As String)

        '   If Not CredentialsProvided_EWB Then
        '   MessageBox.Show("Insufficient Credentials / Information", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '   Exit Sub
        '    End If

        Dim reqCancelEWB As ReqCancelEwbPl = New ReqCancelEwbPl()
        reqCancelEWB.ewbNo = EWBNo  ' 311001074490
        reqCancelEWB.cancelRsnCode = 2
        reqCancelEWB.cancelRmrk = CancellationRemarks
        Dim respCancelEWB As TxnRespWithObjAndInfo(Of RespCancelEwbPl) = Await EWBAPI.CancelEWBAsync(EwbSession, reqCancelEWB)

        If respCancelEWB.IsSuccess Then

            'rtbResponce.Text = JsonConvert.SerializeObject(respCancelEWB.RespObj)

            ResponseObject.Text = JsonConvert.SerializeObject(respCancelEWB.RespObj)

            Dim CMD As New SqlClient.SqlCommand
            CMD.Connection = Cn

            CMD.CommandText = "Update " & InvoiceHeadTable & " SET EWB_Cancelled = 1, EWBCancellation_Reason = '" & CancellationRemarks & "' Where " & InvoiceHeadTableUniqueCode & " = '" & Trim(INVCODE) & "'"
            CMD.ExecuteNonQuery()

            EWBCANCELTextBox.Text = "Cancelled"

        Else
            ResponseObject.Text = respCancelEWB.TxnOutcome
        End If

    End Sub

    Public Shared Async Sub GetIRNDetails(IRN As String, INVCODE As String, Cn As SqlClient.SqlConnection, ResponseObject As RichTextBox, QRCodePictureBox As PictureBox, IRNTextbox As TextBox, IRNACKNoTextBox As TextBox, IRNACKDateTextBox As TextBox, IRNCancelStatusTextBox As TextBox, InvoiceHeadTable As String, InvoiceHeadTableUniqueCode As String, Optional DocDetailType As String = "INV")

        Dim txnRespWithObj As TaxProEInvoice.API.TxnRespWithObj(Of RespPlGenIRN) = Await eInvoiceAPI.GetEInvDetailsAsync(eInvSession, IRN)

        Dim respPlGenIRN As RespPlGenIRN = txnRespWithObj.RespObj
        Dim ErrorCodes As String = ""
        Dim ErrorDesc As String = ""

        ResponseObject.Text = ""

        If txnRespWithObj.IsSuccess Then

            ResponseObject.Text = JsonConvert.SerializeObject(respPlGenIRN)

            If Not IsNothing(respPlGenIRN.QrCodeImage) Then
                Dim Fldr_Name As String = Common_Procedures.AppPath & "\IRNQRCODES"

                If System.IO.Directory.Exists(Fldr_Name) = False Then
                    System.IO.Directory.CreateDirectory(Fldr_Name)
                End If
                respPlGenIRN.QrCodeImage.Save(Fldr_Name & "\QRCODE_" & INVCODE.Replace("/", "") & ".png")
                'respPlGenIRN.QrCodeImage.Save(Common_Procedures.AppPath & "\IRNQRCODES\" & InvCode.Replace("/", "") & ".png")
                QRCodePictureBox.BackgroundImage = respPlGenIRN.QrCodeImage
            End If

            'IRNTextbox.Text = txnRespWithObj.RespObj.Irn
            'IRNACKNoTextBox.Text = txnRespWithObj.RespObj.AckNo
            'IRNACKDateTextBox.Text = txnRespWithObj.RespObj.AckDt
            'IRNCancelStatusTextBox.Text = txnRespWithObj.RespObj.C


            Try

                Dim CMD As New SqlClient.SqlCommand
                CMD.Connection = Cn

                Dim ms As New MemoryStream()
                If IsNothing(respPlGenIRN.QrCodeImage) = False Then
                    Dim bitmp As New Bitmap(respPlGenIRN.QrCodeImage)
                    bitmp.Save(ms, Drawing.Imaging.ImageFormat.Jpeg)
                End If
                Dim data As Byte() = ms.GetBuffer()
                Dim p As New SqlClient.SqlParameter("@QrCode", SqlDbType.Image)
                p.Value = data
                CMD.Parameters.Add(p)
                ms.Dispose()

                CMD.CommandText = "Update " & InvoiceHeadTable & " SET E_Invoice_IRNO = '" & IRNTextbox.Text & "', E_Invoice_QR_Image =  @QrCode, E_Invoice_ACK_No = '" & IRNACKNoTextBox.Text & "' , E_Invoice_ACK_Date = '" & IRNACKDateTextBox.Text & "'  Where " & InvoiceHeadTableUniqueCode & "  = '" & Trim(INVCODE) & "'"
                CMD.ExecuteNonQuery()

            Catch ex As Exception

                MsgBox(ex.Message & ". IRN Generated . Problem encountered in saving to Database")

            End Try


        Else

            'Error has occured
            'Display TxnOutCome in text box - process or show msg to user
            'Process error codes

            If txnRespWithObj.ErrorDetails IsNot Nothing Then

                IRNMessage = ""

                For Each errPl As TaxProEInvoice.API.RespErrDetailsPl In txnRespWithObj.ErrorDetails

                    'Process errPl item here

                    ResponseObject.Text += errPl.ErrorCode & ","
                    ResponseObject.Text += errPl.ErrorCode & ": " + errPl.ErrorMessage + Environment.NewLine

                Next

            Else

                If Not IsNothing(respPlGenIRN.QrCodeImage) Then
                    Dim Fldr_Name As String = Common_Procedures.AppPath & "\IRNQRCODES"

                    If System.IO.Directory.Exists(Fldr_Name) = False Then
                        System.IO.Directory.CreateDirectory(Fldr_Name)
                    End If
                    respPlGenIRN.QrCodeImage.Save(Fldr_Name & "\QRCODE_" & INVCODE.Replace("/", "") & ".png")
                    'respPlGenIRN.QrCodeImage.Save(Common_Procedures.AppPath & "\IRNQRCODES\" & InvCode.Replace("/", "") & ".png")
                    QRCodePictureBox.BackgroundImage = txnRespWithObj.RespObj.QrCodeImage
                End If

                'IRNTextbox.Text = txnRespWithObj.RespObj.Irn
                'IRNACKNoTextBox.Text = txnRespWithObj.RespObj.AckNo
                'IRNACKDateTextBox.Text = txnRespWithObj.RespObj.AckDt
                'IRNCancelStatusTextBox.Text = "Active"
                'ResponseObject.Text = "Successfully Generated"


                Try

                    Dim CMD As New SqlClient.SqlCommand
                    CMD.Connection = Cn

                    Dim ms As New MemoryStream()
                    If IsNothing(QRCodePictureBox.BackgroundImage) = False Then
                        Dim bitmp As New Bitmap(QRCodePictureBox.BackgroundImage)
                        bitmp.Save(ms, Drawing.Imaging.ImageFormat.Jpeg)
                    End If
                    Dim data As Byte() = ms.GetBuffer()
                    Dim p As New SqlClient.SqlParameter("@QrCode", SqlDbType.Image)
                    p.Value = data
                    CMD.Parameters.Add(p)
                    ms.Dispose()

                    CMD.CommandText = "Update " & InvoiceHeadTable & " SET E_Invoice_IRNO = '" & IRNTextbox.Text & "', E_Invoice_QR_Image =  @QrCode, E_Invoice_ACK_No = '" & IRNACKNoTextBox.Text & "' , E_Invoice_ACK_Date = '" & IRNACKDateTextBox.Text & "'  Where " & InvoiceHeadTableUniqueCode & "  = '" & Trim(INVCODE) & "'"
                    CMD.ExecuteNonQuery()

                Catch ex As Exception

                    MessageBox.Show(ex.Message & ". IRN Generated . Problem encountered in saving to Database", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

                End Try

            End If

            Dim respInfoDtlsPl As TaxProEInvoice.API.RespInfoDtlsPl = New TaxProEInvoice.API.RespInfoDtlsPl()

            If txnRespWithObj.InfoDetails IsNot Nothing Then

                For Each infoPl As TaxProEInvoice.API.RespInfoDtlsPl In txnRespWithObj.InfoDetails
                    Dim strDupIrnPl = JsonConvert.SerializeObject(infoPl.Desc)   'Convert object type to json string
                    Select Case infoPl.InfCd
                        Case "DUPIRN"
                            Dim dupIrnPl As DupIrnPl = JsonConvert.DeserializeObject(Of DupIrnPl)(strDupIrnPl)
                        Case "EWBERR"
                            Dim ewbErrPl As List(Of EwbErrPl) = JsonConvert.DeserializeObject(Of List(Of EwbErrPl))(strDupIrnPl)
                        Case "ADDNLNFO"
                            'Deserialize infoPl.Desc as string type and then if this string contains json object, it may be desirilized again as per future releases
                            Dim strDesc As String = CStr(infoPl.Desc)
                    End Select
                Next
            End If
        End If
    End Sub

End Class
