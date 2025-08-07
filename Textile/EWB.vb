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

Public Class EWB

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

    Public Company_Id As Int16

    Public Shared CredentialsProvided_EWB As Boolean = True

    Public Shared WithEvents EwbSession As EWBSession = New EWBSession()

    Private Con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)

    Public Sub New(CmpId As Int16)

        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        Company_Id = CmpId
        Con.Open()

        LoadEWBAPICredentials()

        'DisplayApiLoginDetails()
        'DisplayApiSettings()

    End Sub

    Private Sub DisplayApiLoginDetails()

        Gstin = EWBSession.EwbApiLoginDetails.EwbGstin
        UserId = EWBSession.EwbApiLoginDetails.EwbUserID
        Password = EwbSession.EwbApiLoginDetails.EwbPassword
        AppKey = EWBSession.EwbApiLoginDetails.EwbAppKey
        AuthToken = EwbSession.EwbApiLoginDetails.EwbAuthToken
        TokenExp = Format(EwbSession.EwbApiLoginDetails.EwbTokenExp, "dd/MM/yyyy HH:mm:ss")
        SEK = EwbSession.EwbApiLoginDetails.EwbSEK

    End Sub

    Private Sub DisplayApiSettings()

        GSPName = EwbSession.EwbApiSetting.GSPName
        ASPUserID = EwbSession.EwbApiSetting.AspUserId
        AspPassword = EwbSession.EwbApiSetting.AspPassword
        ClientId = EwbSession.EwbApiSetting.EWBClientId
        ClientSecret = EwbSession.EwbApiSetting.EWBClientSecret
        GspUserId = EwbSession.EwbApiSetting.EWBGSPUserID
        BaseURL = EwbSession.EwbApiSetting.BaseUrl

    End Sub

    Public Shared Async Sub GetAuthToken(Optional ResponseObject As RichTextBox = Nothing)

        Dim TxnResp As TxnRespWithObjAndInfo(Of EWBSession) = Await EWBAPI.GetAuthTokenAsync(EwbSession)

        If Not IsNothing(ResponseObject) Then
            ResponseObject.Text = TxnResp.TxnOutcome
        Else
            MessageBox.Show(TxnResp.TxnOutcome, "EWB RESPONSE ...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
        End If

    End Sub

    Public Shared Async Sub GenerateEWB(InvoiceCode As String, Cn As SqlClient.SqlConnection, ResponseObject As RichTextBox, EWBTextBox As TextBox, SalesHead As String, EWBField As String, SalesHeadUniqueField As String, Entry_PkCondition As String)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim IsInterState As Boolean
        Dim Nr As Long
        Dim vNOOF_EINVS As String = 0
        Dim vSHIPEDTOGSTIN As String = ""
        Dim vSHIPEDTONAME As String = ""
        Dim vTRANSGSTINNO As String = ""
        Dim vTRANSPORT_NAME As String = ""
        Dim vBILLEDTOGSTIN As String = ""
        Dim vBILLEDTONAME As String = ""
        Dim vHSNCD As String = ""
        Dim vFROMGSTIN As String = ""
        Dim vSHIPEDTOCODE As String = ""
        Dim vBILLEDTOCODE As String = ""

        Dim vSHIPEDTO_IDNO As String = ""
        Dim vBILLEDTO_IDNO As String = ""
        Dim vDISPATCH_FROM_IDNO As String = ""

        Common_Procedures.check_Validating_for_eINVOICE_eWAY_GENERATION()

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
        Else
            If (Val(Common_Procedures.settings.EWayBill_API_TotalCredits_Per_Year) - Val(vNOOF_EINVS)) <= 25 Then
                MessageBox.Show("Your E-Way Bill API Credits will Expire Soon." & Chr(13) & "There are only " & (Val(Common_Procedures.settings.EWayBill_API_TotalCredits_Per_Year) - Val(vNOOF_EINVS)) & " API credits left out of " & Common_Procedures.settings.EWayBill_API_TotalCredits_Per_Year & " API credits," & Chr(13) & "so you need to Recharge your API credits immediately.", "E-WAY BILL INFORMATION....", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
            End If
        End If

        da1 = New SqlClient.SqlDataAdapter("Select * from EWB_Head Where InvCode = '" & InvoiceCode & "'", Cn)
        dt1 = New DataTable
        da1.Fill(dt1)
        If dt1.Rows.Count <= 0 Then
            MessageBox.Show("Please Save the Invoice before proceeding to generate EWB ! ", "CANNOT GENERATE EWB ...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If


        Dim vFROM_STATECODE As String
        Dim vTO_STATECODE As String

        vFROM_STATECODE = 0
        If Not IsDBNull(dt1.Rows(0).Item("FromStateCode")) Then
            vFROM_STATECODE = dt1.Rows(0).Item("FromStateCode")
        End If
        vTO_STATECODE = 0
        If Not IsDBNull(dt1.Rows(0).Item("ToStateCode")) Then
            vTO_STATECODE = dt1.Rows(0).Item("ToStateCode")
        End If


        If Len(Trim(vFROM_STATECODE)) <> 2 Then
            MsgBox("INVALID COMPANY STATE CODE")
            Exit Sub
        End If

        If Len(Trim(vTO_STATECODE)) <> 2 Then
            MsgBox("INVALID CONSIGNEE (PARTY) STATE CODE")
            Exit Sub
        End If

        If Val(Trim(vFROM_STATECODE)) <> Val(Trim(vTO_STATECODE)) Then
            IsInterState = True
        End If

        Dim ewbGen As ReqGenEwbPl = New ReqGenEwbPl()

        ewbGen.supplyType = dt1.Rows(0).Item("SupplyType")
        ewbGen.subSupplyType = dt1.Rows(0).Item("SubSupplyType")
        ewbGen.subSupplyDesc = dt1.Rows(0).Item("SubSupplyDesc")
        ewbGen.docType = dt1.Rows(0).Item("DocType")
        ewbGen.docNo = dt1.Rows(0).Item("EWBGenDocNo")
        ewbGen.docDate = Format(dt1.Rows(0).Item("EWBDocDate"), "dd/MM/yyyy")

        '-------------

        vFROMGSTIN = ""

        If IsDBNull(dt1.Rows(0).Item("FromGSTIN")) = False Then
            If Len(Trim(dt1.Rows(0).Item("FromGSTIN"))) = 15 Then
                vFROMGSTIN = dt1.Rows(0).Item("FromGSTIN")
            End If
        End If
        If Trim(vFROMGSTIN) = "" Then
            vFROMGSTIN = "URP"
        End If


        '--------------

        ewbGen.fromGstin = Trim(vFROMGSTIN) 'dt1.Rows(0).Item("FromGSTIN")
        ewbGen.fromTrdName = dt1.Rows(0).Item("FromTradeName")
        ewbGen.fromAddr1 = dt1.Rows(0).Item("FromAddress1")
        ewbGen.fromAddr2 = dt1.Rows(0).Item("FromAddress2")
        ewbGen.fromPlace = dt1.Rows(0).Item("FromPlace")

        If IsNumeric(dt1.Rows(0).Item("FromPINCode")) Then
            If Len(Trim(dt1.Rows(0).Item("FromPINCode"))) = 6 Then
                ewbGen.fromPincode = dt1.Rows(0).Item("FromPINCode")
            Else
                MessageBox.Show("ENTER A VALID PINCODE FOR COMPANY (COMPANY - CREATION)", "COMPANY PIN CODE REQUIRED", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub
            End If
        Else
            MessageBox.Show("ENTER A VALID PINCODE FOR COMPANY (COMPANY - CREATION)", "COMPANY PIN CODE REQUIRED", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If
        'ewbGen.fromPincode = dt1.Rows(0).Item("FromPINCode")

        ewbGen.fromStateCode = dt1.Rows(0).Item("FromStateCode")
        ewbGen.actFromStateCode = dt1.Rows(0).Item("ActualFromStateCode")

        '-----------------------------


        vBILLEDTOGSTIN = ""
        vBILLEDTONAME = ""
        If IsDBNull(dt1.Rows(0).Item("ToGSTIN")) = False Then
            If Len(Trim(dt1.Rows(0).Item("ToGSTIN"))) = 15 Then
                vBILLEDTOGSTIN = dt1.Rows(0).Item("ToGSTIN")
            End If
        End If
        If Trim(vBILLEDTOGSTIN) = "" Then
            vBILLEDTOGSTIN = "URP"
        End If


        If IsDBNull(dt1.Rows(0).Item("ToTradeName")) = False Then
            If Len(Trim(dt1.Rows(0).Item("ToTradeName"))) > 1 Then
                vBILLEDTONAME = dt1.Rows(0).Item("ToTradeName")
            End If
        End If
        vSHIPEDTOGSTIN = ""
        vSHIPEDTONAME = ""
        If IsDBNull(dt1.Rows(0).Item("ShippedToGSTIN")) = False Then
            If Len(Trim(dt1.Rows(0).Item("ShippedToGSTIN"))) = 15 Then
                ewbGen.shipToGSTIN = dt1.Rows(0).Item("ShippedToGSTIN")
                vSHIPEDTOGSTIN = dt1.Rows(0).Item("ShippedToGSTIN")
            End If
        End If
        If IsDBNull(dt1.Rows(0).Item("ShippedToTradeName")) = False Then
            If Len(Trim(dt1.Rows(0).Item("ShippedToTradeName"))) > 1 Then
                ewbGen.shipToTradeName = dt1.Rows(0).Item("ShippedToTradeName")
                vSHIPEDTONAME = dt1.Rows(0).Item("ShippedToTradeName")
            End If
        End If
        ewbGen.toGstin = Trim(vBILLEDTOGSTIN)  '  dt1.Rows(0).Item("ToGSTIN")
        ewbGen.toTrdName = Trim(vBILLEDTONAME)  ' dt1.Rows(0).Item("ToTradeName")
        ewbGen.toAddr1 = dt1.Rows(0).Item("ToAddress1")
        ewbGen.toAddr2 = dt1.Rows(0).Item("ToAddress2")
        ewbGen.toPlace = dt1.Rows(0).Item("ToPlace")
        If IsNumeric(dt1.Rows(0).Item("ToPINCode")) Then
            If Len(Trim(dt1.Rows(0).Item("ToPINCode"))) = 6 Then
                ewbGen.toPincode = dt1.Rows(0).Item("ToPINCode")

            Else
                MessageBox.Show("ENTER A VALID PINCODE FOR PARTY (LEDGER - CREATION)", "PARTY PIN CODE REQUIRED", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub
            End If

        Else

            MessageBox.Show("ENTER A VALID PINCODE FOR PARTY (LEDGER - CREATION)", "PARTY PIN CODE REQUIRED", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub

        End If
        ewbGen.toStateCode = dt1.Rows(0).Item("ToStateCode")
        ewbGen.actToStateCode = dt1.Rows(0).Item("ActualToStateCode")

        vSHIPEDTOCODE = dt1.Rows(0).Item("ToStateCode")
        vBILLEDTOCODE = dt1.Rows(0).Item("ActualToStateCode")



        vBILLEDTO_IDNO = 0
        vSHIPEDTO_IDNO = 0
        vDISPATCH_FROM_IDNO = 0


        vBILLEDTO_IDNO = dt1.Rows(0).Item("Billed_To_IdNo")
        vSHIPEDTO_IDNO = dt1.Rows(0).Item("Shipped_To_IdNo")
        vDISPATCH_FROM_IDNO = dt1.Rows(0).Item("Dispatch_From_IdNo")

        If Val(vSHIPEDTO_IDNO) = Val(vBILLEDTO_IDNO) Then
            vSHIPEDTO_IDNO = 0
        End If


        If Val(vDISPATCH_FROM_IDNO) <> 0 Then

            If Val(vSHIPEDTO_IDNO) <> 0 And Val(vSHIPEDTO_IDNO) <> Val(vBILLEDTO_IDNO) Then

                ewbGen.transactionType = 4
            Else
                ewbGen.transactionType = 3
            End If

        ElseIf Val(vSHIPEDTO_IDNO) <> 0 And Val(vSHIPEDTO_IDNO) <> Val(vBILLEDTO_IDNO) Then

            ewbGen.transactionType = 2
        Else

            ewbGen.transactionType = 1
        End If

        'If Trim(vSHIPEDTONAME) <> "" Then
        '    'If Trim(UCase(vSHIPEDTONAME)) = Trim(UCase(vBILLEDTONAME)) Then
        '    '    ewbGen.transactionType = 1
        '    'Else
        '    '    ewbGen.transactionType = 2
        '    'End If
        'Else
        '    ewbGen.transactionType = 1
        'End If

        'If Trim(UCase(vSHIPEDTOCODE)) = Trim(UCase(vBILLEDTOCODE)) And Trim(UCase(vSHIPEDTOGSTIN)) = Trim(UCase(vBILLEDTOGSTIN)) Then
        '    'If Trim(UCase(vSHIPEDTOCODE)) = Trim(UCase(vBILLEDTOCODE)) And Trim(UCase(vSHIPEDTONAME)) = Trim(UCase(vBILLEDTONAME)) Then
        '    ewbGen.transactionType = 1
        'Else
        '    ewbGen.transactionType = 2
        'End If

        If Trim(UCase(vFROMGSTIN)) = Trim(UCase(vBILLEDTOGSTIN)) Then
            ewbGen.subSupplyType = 5   '  --- For Own Use
        Else
            ewbGen.subSupplyType = dt1.Rows(0).Item("SubSupplyType")
        End If
        'ewbGen.transactionType = dt1.Rows(0).Item("TransactionType")
        'ewbGen.transactionType = 2

        '---ewbGen.dispatchFromGSTIN = "29AAAAA1303P1ZV"
        '---ewbGen.dispatchFromTradeName = "ABC Traders"

        '---ewbGen.shipToGSTIN = "29ALSPR1722R1Z3"
        '---ewbGen.shipToTradeName = "XYZ Traders"

        ewbGen.otherValue = dt1.Rows(0).Item("OtherValue") 'transport , discount etc
        ewbGen.totalValue = dt1.Rows(0).Item("Total_Value")
        ewbGen.cgstValue = dt1.Rows(0).Item("CGST_Value")
        ewbGen.sgstValue = dt1.Rows(0).Item("SGST_Value")
        ewbGen.igstValue = dt1.Rows(0).Item("IGST_Value")
        ewbGen.cessValue = dt1.Rows(0).Item("CessValue")
        ewbGen.cessNonAdvolValue = dt1.Rows(0).Item("CessNonAdvolValue")




        'ewbGen.transporterId = "05AAACG0904A1ZL"
        'ewbGen.transporterName = ""
        'ewbGen.transDocNo = ""

        'If IsDBNull(dt1.Rows(0).Item("TransporterGSTIN")) Then
        'ewbGen.transporterId = "05AAACG0904A1ZL"       'MANDATORY
        'ElseIf Len(Trim(dt1.Rows(0).Item("TransporterGSTIN"))) <> 15 Then
        'ewbGen.transporterId = "05AAACG0904A1ZL"       'MANDATORY
        'Else

        vTRANSGSTINNO = ""

        If IsDBNull(dt1.Rows(0).Item("TransporterID")) = False Then
            vTRANSGSTINNO = Trim(dt1.Rows(0).Item("TransporterID").ToString)
        End If
        'If IsDBNull(dt1.Rows(0).Item("TransporterGSTIN")) = False Then
        ' vTRANSGSTINNO = Trim(dt1.Rows(0).Item("TransporterGSTIN").ToString)
        ' End If
        vTRANSPORT_NAME = ""
        If IsDBNull(dt1.Rows(0).Item("TransporterName")) = False Then
            vTRANSPORT_NAME = Trim(dt1.Rows(0).Item("TransporterName").ToString)
        End If

        If Len(Trim(vTRANSGSTINNO)) = 15 Then
            ewbGen.transporterId = Trim(vTRANSGSTINNO)  ' dt1.Rows(0).Item("TransporterID")
            ewbGen.transporterName = dt1.Rows(0).Item("TransporterName")
        Else
            If Trim(vTRANSPORT_NAME) <> "" Then
                If Len(Trim(dt1.Rows(0).Item("VehicleNo"))) <= 0 Then
                    MessageBox.Show("Provide a valid GSTIN for transport (in transport creation), if not, provide the vehicle number.", "TRANSPORT GSTIN REQUIRED", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If

        If IsDBNull(dt1.Rows(0).Item("TransportDOCNo")) = False Then
        If Len(Trim(dt1.Rows(0).Item("TransportDOCNo"))) > 0 Then
            ewbGen.transDocNo = dt1.Rows(0).Item("TransportDOCNo")

            If IsDBNull(dt1.Rows(0).Item("TransportDOCDate")) = False Then

                If IsDate(dt1.Rows(0).Item("TransportDOCDate")) = True Then
                
                    Dim vLRDATE As Date
    
                    If Year(dt1.Rows(0).Item("TransportDOCDate")) <> 1900 Then
                        vLRDATE = dt1.Rows(0).Item("TransportDOCDate")
                    End If
                
                    If IsDate(vLRDATE) = False Then
                        vLRDATE = dt1.Rows(0).Item("EWBDocDate")
                    End If

                    ewbGen.transDocDate = Format(vLRDATE, "dd/MM/yyyy")
                    'ewbGen.transDocDate = Format(dt1.Rows(0).Item("TransportDOCDate"), "dd/MM/yyyy")

                End If
            End If
        End If
        End If

        Dim vVehicleNo As String
        vVehicleNo = ""

        vVehicleNo = Replace(Trim(dt1.Rows(0).Item("VehicleNo").ToString), " ", "")

        If Len(Trim(vVehicleNo)) > 0 Then
            ewbGen.vehicleNo = vVehicleNo 'dt1.Rows(0).Item("VehicleNo")
            ewbGen.vehicleType = dt1.Rows(0).Item("VehicleType")
            ewbGen.transMode = dt1.Rows(0).Item("TransMode")
        End If

        'old
        'If Len(Trim(dt1.Rows(0).Item("VehicleNo"))) > 0 Then
        '    ewbGen.vehicleNo = dt1.Rows(0).Item("VehicleNo")
        '    ewbGen.vehicleType = dt1.Rows(0).Item("VehicleType")
        '    ewbGen.transMode = dt1.Rows(0).Item("TransMode")
        'End If
        If Len(Trim(ewbGen.transporterId)) = 0 And Len(Trim(ewbGen.vehicleNo)) = 0 Then
            MessageBox.Show("Vehicle number has to be provided when TransporterName is not provided. ", "Vehicle Number?", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
            Exit Sub
        End If

        ewbGen.totInvValue = dt1.Rows(0).Item("TotalInvValue")

        Dim vDISTANCE As String = 0
        Dim vCOMP_PINCODE As String
        Dim vSHIP_PINCODE As String

        vDISTANCE = 0
        If IsDBNull(dt1.Rows(0).Item("Distance")) = False Then
            vDISTANCE = dt1.Rows(0).Item("Distance").ToString
        End If
        vCOMP_PINCODE = 0
        If Not IsDBNull(dt1.Rows(0).Item("FromPINCode")) Then
            vCOMP_PINCODE = dt1.Rows(0).Item("FromPINCode")
        End If
        vSHIP_PINCODE = 0
        If Not IsDBNull(dt1.Rows(0).Item("ToPINCode")) Then
            vSHIP_PINCODE = dt1.Rows(0).Item("ToPINCode")
        End If

        If Val(vDISTANCE) > 0 Then
            ewbGen.transDistance = dt1.Rows(0).Item("Distance")
        ElseIf Val(vCOMP_PINCODE) <> Val(vSHIP_PINCODE) Then
            ewbGen.transDistance = 0
        Else
            ewbGen.transDistance = 5 ' FormatNumber(dt1.Rows(0).Item("Distance"), 0, TriState.False, TriState.False, TriState.False)
        End If

        ewbGen.itemList = New List(Of ReqGenEwbPl.ItemListInReqEWBpl)()

        da2 = New SqlClient.SqlDataAdapter("Select * from EWB_Details Where InvCode = '" & InvoiceCode & "' Order BY SlNo", Cn)
        dt2 = New DataTable
        da2.Fill(dt2)

        For I = 0 To dt2.Rows.Count - 1
            vHSNCD = ""
            If IsDBNull(dt2.Rows(I).Item("HSNCode")) = False Then
                If Len(Trim(dt2.Rows(I).Item("HSNCode"))) > 1 Then
                    vHSNCD = dt2.Rows(I).Item("HSNCode")
                End If
            End If

            If Trim(vHSNCD) = "" Then
                MessageBox.Show("HSN Code has to be provided, for Product  - " & dt2.Rows(I).Item("Product_Name"), "HSN CODE?", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
                Exit Sub
            End If

            ewbGen.itemList.Add(New ReqGenEwbPl.ItemListInReqEWBpl With {
                    .productName = dt2.Rows(I).Item("Product_Name"),
                    .productDesc = dt2.Rows(I).Item("Product_Description"),
                    .hsnCode = dt2.Rows(I).Item("HSNCode"),
                    .quantity = dt2.Rows(I).Item("Quantity"),
                    .qtyUnit = dt2.Rows(I).Item("QuantityUnit"),
                    .cgstRate = IIf(IsInterState, 0, dt2.Rows(I).Item("Tax_Perc") / 2),
                    .sgstRate = IIf(IsInterState, 0, dt2.Rows(I).Item("Tax_Perc") / 2),
                    .igstRate = IIf(IsInterState, dt2.Rows(I).Item("Tax_Perc"), 0),
                    .cessRate = dt2.Rows(I).Item("CessRate"),
                    .cessNonAdvol = dt2.Rows(I).Item("CessNonAdvol"),
                    .taxableAmount = dt2.Rows(I).Item("TaxableAmount")
                })

        Next

        '.cgstRate = IIf(IsInterState, 0, dt2.Rows(I).Item("Tax_Perc") / 2),
        '.sgstRate = IIf(IsInterState, 0, dt2.Rows(I).Item("Tax_Perc") / 2),
        '.igstRate = IIf(IsInterState, dt2.Rows(I).Item("Tax_Perc"), 0),

        Dim TxnResp As TxnRespWithObjAndInfo(Of RespGenEwbPl) = Await EWBAPI.GenEWBAsync(EwbSession, ewbGen)

        If TxnResp.IsSuccess Then

            ResponseObject.Text = JsonConvert.SerializeObject(TxnResp.RespObj)

            Dim rawresp As String = ResponseObject.Text

            Dim jss As New JavaScriptSerializer()
            Dim dict As Dictionary(Of String, String) = jss.Deserialize(Of Dictionary(Of String, String))(rawresp)

            EWBTextBox.Text = dict("ewayBillNo")

            'txt_Electronic_RefNo.Text = txt_EWBNo.Text

            'Dim NewCode As String = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Dim cmd As New SqlClient.SqlCommand
            cmd.Connection = Cn

            'Dim CMD As New SqlClient.SqlCommand
            'cmd.Connection = con

            ' EWBTextBox As TextBox, SalesHead As String, EWBField As String, SalesHeadUniqueField As String

            Try

                cmd.CommandText = "Update " & SalesHead & " set   " & EWBField & "  = '" & Trim(EWBTextBox.Text) & "' Where " & SalesHeadUniqueField & "  = '" & Trim(InvoiceCode) & "'"
                'cmd.CommandText = "Update " & SalesHead & " set  Electronic_Reference_No = '" & Trim(EWBTextBox.Text) & "' Where " & SalesHeadUniqueField & "  = '" & Trim(InvoiceCode) & "'"
                cmd.ExecuteNonQuery()

                If Trim(EWBTextBox.Text) <> "" Then
                    Nr = 0
                    cmd.CommandText = "Update  " & Trim(Common_Procedures.CompanyDetailsDataBaseName) & "..eInvoice_eWay_Bill_Details SET EWay_BillNo = '" & Trim(EWBTextBox.Text) & "', EWay_BillDate = '' Where CompanyGroup_IdNo = '" & Trim(Common_Procedures.CompGroupIdNo) & "' and Year_Code = '" & Trim(Common_Procedures.FnYearCode) & "' and Entry_Type = 'EWAYBILL' and Document_Code = '" & Trim(Entry_PkCondition) & Trim(InvoiceCode) & "'"
                    Nr = cmd.ExecuteNonQuery()
                    If Nr = 0 Then
                        cmd.CommandText = "Insert into  " & Trim(Common_Procedures.CompanyDetailsDataBaseName) & "..eInvoice_eWay_Bill_Details ( CompanyGroup_IdNo, Year_Code, Entry_Type, Document_Code, EInvoice_IRN_No, EInvoice_ACK_No, EInvoice_ACK_Date, EWay_BillNo, EWay_BillDate) Values ( " & Str(Val(Common_Procedures.CompGroupIdNo)) & " , '" & Trim(Common_Procedures.FnYearCode) & "' , 'EWAYBILL' , '" & Trim(Entry_PkCondition) & Trim(InvoiceCode) & "' , '', '' , '', '" & Trim(EWBTextBox.Text) & "', '' )"
                        cmd.ExecuteNonQuery()
                    End If
                End If

            Catch EX As Exception
                MessageBox.Show("EWB Generated , Problem encountered in saving to Database" & Chr(13) & EX.Message, "ERROR IN SAVING E-WAY BILL DETAILS....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End Try

        Else

            If Microsoft.VisualBasic.Left(Trim(TxnResp.TxnOutcome), 3) = "721" Then

                MessageBox.Show("The Distance between the Consignor PIN and Consignee PIN is not available in the System. Please provide distance information in Ledger Creation form", "Provide Distance", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If

            If Microsoft.VisualBasic.Left(Trim(TxnResp.TxnOutcome), 3) = "724" Then

                MessageBox.Show("HSN code of at least one item should be of goods to generate e-Way Bill. ", "HSN CODE REQUIRED", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If

            ResponseObject.Text = TxnResp.TxnOutcome

        End If

    End Sub




    'Public Shared Async Sub PrintEWB(EWBNumber As String, ResponseObject As RichTextBox)

    '    Try

    '        Dim EwbNo As Long = CLng(EWBNumber)
    '        Dim TxnResp As TxnRespWithObjAndInfo(Of RespGetEWBDetail) = Await EWBAPI.GetEWBDetailAsync(EwbSession, EwbNo)
    '        Dim a = JsonConvert.SerializeObject(TxnResp.RespObj)

    '        If TxnResp.IsSuccess = True Then

    '            If Not System.IO.Directory.Exists(Application.StartupPath & "\EWB") Then
    '                System.IO.Directory.CreateDirectory(Application.StartupPath & "\EWB")
    '            End If

    '            EWBAPI.PrintEWB(EwbSession, TxnResp.RespObj, Application.StartupPath & "\EWB\", True, False)

    '        Else

    '            GetAuthToken()
    '            ResponseObject.Text = TxnResp.TxnOutcome

    '        End If

    '    Catch ex As Exception

    '        MsgBox(ex.Message & "... Error Occured")

    '    End Try

    'End Sub

    Public Shared Async Sub PrintEWB(EWBNumber As String, ResponseObject As RichTextBox, Optional EwbPrintSts As Integer = 0, Optional IRN As String = "")

        Try

            Dim EwbNo As Long = CLng(EWBNumber)
            Dim TxnResp As TxnRespWithObjAndInfo(Of RespGetEWBDetail) = Await EWBAPI.GetEWBDetailAsync(EwbSession, EwbNo)
            Dim a = JsonConvert.SerializeObject(TxnResp.RespObj)

            If TxnResp.IsSuccess = True Then

                If Not System.IO.Directory.Exists(Application.StartupPath & "\EWB") Then
                    System.IO.Directory.CreateDirectory(Application.StartupPath & "\EWB")
                End If

                Dim strJson = JsonConvert.SerializeObject(TxnResp.RespObj)
                Dim ReqPrintEWB = New ReqPrintEWB()

                ReqPrintEWB = JsonConvert.DeserializeObject(Of ReqPrintEWB)(strJson)

                'ReqPrintEWB = JsonConvert.DeserializeObject(strJson)

                If Len(Trim(IRN)) > 0 Then
                    ReqPrintEWB.Irn = IRN  ' irn
                End If
                If EwbPrintSts = 1 Then
                    EWBAPI.PrintEWB(EwbSession, ReqPrintEWB, Application.StartupPath & "\EWB\", True, True)
                Else
                    EWBAPI.PrintEWB(EwbSession, ReqPrintEWB, Application.StartupPath & "\EWB\", True, False)
                End If

            Else

                GetAuthToken()
                ResponseObject.Text = TxnResp.TxnOutcome

            End If

        Catch ex As Exception

            MsgBox(ex.Message & "... Error Occured")

        End Try

    End Sub

    Public Shared Async Sub CancelEWB(EWBNUMBER As String, InvoiceCode As String, Cn As SqlClient.SqlConnection, ResponseObject As RichTextBox, EWBTextBox As TextBox, SalesHead As String, EWBField As String, SalesHeadUniqueField As String)

        Dim reqCancelEWB As ReqCancelEwbPl = New ReqCancelEwbPl()
        reqCancelEWB.ewbNo = Val(EWBNUMBER)
        reqCancelEWB.cancelRsnCode = 2
        reqCancelEWB.cancelRmrk = "Cancelled the order"

        Dim respCancelEWB As TxnRespWithObjAndInfo(Of RespCancelEwbPl) = Await EWBAPI.CancelEWBAsync(EwbSession, reqCancelEWB)

        If respCancelEWB.IsSuccess Then

            ResponseObject.Text = JsonConvert.SerializeObject(respCancelEWB.RespObj)
            EWBTextBox.Text = ""

            Try

                Dim cmd As New SqlClient.SqlCommand
                cmd.Connection = Cn

                cmd.CommandText = "Update " & SalesHead & " set " & EWBField & " = '" & EWBTextBox.Text & "'  Where " & SalesHeadUniqueField & " = '" & Trim(InvoiceCode) & "'"

                cmd.ExecuteNonQuery()

            Catch EX As Exception

                MsgBox(EX.Message & "... EWAY BILL DELETED. BUT COULD'NT SAVE TO DATABASE. PLEASE SAVE THE INVOICE")

            End Try

        Else


            ResponseObject.Text = respCancelEWB.TxnOutcome

        End If

    End Sub

    Public Sub LoadEWBAPICredentials()

        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        da = New SqlClient.SqlDataAdapter("Select C.Company_IdNo,C.Company_Name,C.Company_ShortName,C.Company_GSTINNo,G.* from Company_Head C Left Outer Join GST_EWB_API_Settings G On C.Company_GSTINNo  COLLATE Latin1_General_CI_AI = G.GSTIN COLLATE Latin1_General_CI_AI  Where C.Company_IdNo = " & Company_Id.ToString & " And len(C.COMPANY_GSTINNo) = 15 ", Con)
        dt = New DataTable
        da.Fill(dt)

        If dt.Rows.Count > 0 Then

            If Not IsDBNull(dt.Rows(0).Item("GSPName")) Then
                GSPName = dt.Rows(0).Item("GSPName")
                EwbSession.EwbApiSetting.GSPName = GSPName
            Else
                CredentialsProvided_EWB = False
                MsgBox("Invalid GSP Name Provided", vbOK, "Insufficient Information")
                Exit Sub
            End If

            If Not IsDBNull(dt.Rows(0).Item("ASPUSERID")) Then
                ASPUserID = dt.Rows(0).Item("ASPUSERID")
                EwbSession.EwbApiSetting.AspUserId = ASPUserID
            Else
                CredentialsProvided_EWB = False
                MsgBox("Invalid ASP User ID Provided", vbOK, "Insufficient Information")
                Exit Sub
            End If

            If Not IsDBNull(dt.Rows(0).Item("ASPPASSWORD")) Then
                AspPassword = dt.Rows(0).Item("ASPPASSWORD")
                EwbSession.EwbApiSetting.AspPassword = AspPassword
            Else
                CredentialsProvided_EWB = False
                MsgBox("Invalid ASP Password Provided", vbOK, "Insufficient Information")
                Exit Sub
            End If

            If Not IsDBNull(dt.Rows(0).Item("EWBUserId")) Then
                UserId = dt.Rows(0).Item("EWBUserId")
                If Len(Trim(UserId)) > 0 Then
                    EwbSession.EwbApiLoginDetails.EwbUserID = UserId
                End If
            Else
                CredentialsProvided_EWB = False
                MsgBox("Invalid EWB User ID Provided", vbOK, "Insufficient Information")
                Exit Sub
            End If

            If Not IsDBNull(dt.Rows(0).Item("EWBPassword")) Then
                Password = dt.Rows(0).Item("EWBPassword")
                If Len(Trim(Password)) > 0 Then
                    EwbSession.EwbApiLoginDetails.EwbPassword = Password
                End If
            Else
                CredentialsProvided_EWB = False
                MsgBox("Invalid EWB Password Provided", vbOK, "Insufficient Information")
                Exit Sub
            End If

            If Not IsDBNull(dt.Rows(0).Item("GSTIN")) Then
                Gstin = dt.Rows(0).Item("GSTIN")
                If Len(Trim(Gstin)) > 0 Then
                    EwbSession.EwbApiLoginDetails.EwbGstin = Gstin
                End If
            Else
                CredentialsProvided_EWB = False
                MsgBox("Invalid EWB Password Provided", vbOK, "Insufficient Information")
                Exit Sub
            End If


            'If Not IsDBNull(dt.Rows(0).Item("BaseURL")) Then
            '    BaseURL = dt.Rows(0).Item("BaseURL")
            '    EwbSession.EwbApiSetting.BaseUrl = BaseURL
            'Else
            '    CredentialsProvided_EWB = False
            '    MsgBox("Invalid EWB Base URL Provided", vbOK, "Insufficient Information")
            '    Exit Sub
            'End If

        Else

            CredentialsProvided_EWB = False

        End If

        GSPName = EwbSession.EwbApiSetting.GSPName
        ASPUserID = EwbSession.EwbApiSetting.AspUserId
        AspPassword = EwbSession.EwbApiSetting.AspPassword
        BaseURL = EwbSession.EwbApiSetting.BaseUrl

        ClientId = EwbSession.EwbApiSetting.EWBClientId
        ClientSecret = EwbSession.EwbApiSetting.EWBClientSecret

        AppKey = EwbSession.EwbApiLoginDetails.EwbAppKey
        AuthToken = EwbSession.EwbApiLoginDetails.EwbAuthToken
        SEK = EwbSession.EwbApiLoginDetails.EwbSEK


    End Sub

End Class
