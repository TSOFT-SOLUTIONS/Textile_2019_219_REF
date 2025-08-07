
Imports System.IO
Imports Newtonsoft.Json

Module EInvoice_JSON_Generator
    ' Function to generate e-Invoice JSON dynamically
    Public Sub GenerateEInvoice(docTyp As String, docNo As String, docDate As Date,
                                sellerGstin As String, sellerLglNm As String, sellerTrdNm As String, sellerAddr1 As String, sellerAddr2 As String, sellerLoc As String, sellerPin As String, sellerStcd As String,
                                buyerGstin As String, buyerLglNm As String, buyerTrdNm As String, buyerAddr1 As String, buyerAddr2 As String, buyerLoc As String, buyerPin As String, buyerStcd As String,
                                vShiptoIDNO As String, ShipGstin As String, ShipLglNm As String, ShipTrdNm As String, ShipAddr1 As String, ShipAddr2 As String, ShipLoc As String, ShipPin As String, ShipStcd As String,
                                eInvitems As List(Of EInvoiceItem),
                                Total_AssVal As String, Total_CgstVal As String, Total_SgstVal As String, Total_IgstVal As String, Total_TotInvVal As String)


        ' Create e-Invoice object with dynamic inputs
        Dim eInvoice As New EInvoice With {
            .Version = "1.1",
            .TranDtls = New TransactionDetails With {
                .TaxSch = "GST",
                .SupTyp = "B2B",
                .RegRev = "N",
                .EcmGstin = Nothing,
                .IgstOnIntra = "N"
            },
            .DocDtls = New DocumentDetails With {
                .Typ = docTyp,
                .No = docNo,
                .Dt = docDate.Date
            },
            .SellerDtls = New PartyDetails With {
                .Gstin = sellerGstin,
                .LglNm = sellerLglNm,
                .TrdNm = sellerTrdNm,
                .Addr1 = sellerAddr1,
                .Addr2 = sellerAddr2,
                .Loc = sellerLoc,
                .Pin = Val(sellerPin),
                .Stcd = sellerStcd
            },
            .BuyerDtls = New PartyDetails With {
                .Gstin = buyerGstin,
                .LglNm = buyerLglNm,
                .TrdNm = buyerTrdNm,
                .Addr1 = buyerAddr1,
                .Addr2 = buyerAddr2,
                .Loc = buyerLoc,
                .Pin = Val(buyerPin),
                .Stcd = buyerStcd
            },
            .ValDtls = New ValueDetails With {
                .AssVal = Val(Total_AssVal),
                .CgstVal = Val(Total_CgstVal),
                .SgstVal = Val(Total_SgstVal),
                .IgstVal = Val(Total_IgstVal),
                .TotInvVal = Val(Total_TotInvVal)
            },
            .ItemList = eInvitems
        }


        ' Convert to JSON format
        Dim jsonOutput As String = JsonConvert.SerializeObject(eInvoice, Formatting.Indented)


        ' Save JSON to a file
        Dim filePath As String = Trim(Common_Procedures.AppPath) & "\eInvoice.json"
        If File.Exists(filePath) = True Then
            File.Delete(filePath)
        End If
        File.WriteAllText(filePath, jsonOutput)

    End Sub

    ' Classes for e-Invoice structure
    Public Class EInvoice
        Public Property Version As String
        Public Property TranDtls As TransactionDetails
        Public Property DocDtls As DocumentDetails
        Public Property SellerDtls As PartyDetails
        Public Property BuyerDtls As PartyDetails
        Public Property ItemList As List(Of EInvoiceItem)
        Public Property ValDtls As ValueDetails
    End Class

    Public Class TransactionDetails
        Public Property TaxSch As String
        Public Property SupTyp As String
        Public Property RegRev As String
        Public Property EcmGstin As String
        Public Property IgstOnIntra As String
    End Class

    Public Class DocumentDetails
        Public Property Typ As String
        Public Property No As String
        Public Property Dt As String
    End Class

    Public Class PartyDetails
        Public Property Gstin As String
        Public Property LglNm As String
        Public Property TrdNm As String
        Public Property Addr1 As String
        Public Property Addr2 As String
        Public Property Loc As String
        Public Property Pin As Integer
        Public Property Stcd As String
    End Class

    Public Class EInvoiceItem
        Public Property SlNo As String
        Public Property PrdDesc As String
        Public Property IsServc As String
        Public Property HsnCd As String
        Public Property Qty As Integer
        Public Property Unit As String
        Public Property UnitPrice As Decimal
        Public Property TotAmt As Decimal
        Public Property Discount As Decimal
        Public Property PreTaxVal As Decimal
        Public Property AssAmt As Decimal
        Public Property GstRt As Decimal
        Public Property IgstAmt As Decimal
        Public Property CgstAmt As Decimal
        Public Property SgstAmt As Decimal
        Public Property TotItemVal As Decimal
    End Class



    Public Class ValueDetails
        Public Property AssVal As Decimal
        Public Property CgstVal As Decimal
        Public Property SgstVal As Decimal
        Public Property IgstVal As Decimal
        Public Property TotInvVal As Decimal
    End Class

End Module