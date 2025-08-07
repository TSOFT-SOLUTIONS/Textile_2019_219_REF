

Imports System.IO
Imports Newtonsoft.Json


Module EInvoice_JSON_Generator1
    Sub Main()
        ' Create e-Invoice object
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
                .Typ = "INV",
                .No = "INV-123456",
                .Dt = "2025-03-13"
            },
            .SellerDtls = New PartyDetails With {
                .Gstin = "29ABCDE1234F1Z5",
                .LglNm = "XYZ Pvt Ltd",
                .TrdNm = "XYZ Enterprises",
                .Addr1 = "123, Main Road",
                .Addr2 = "Near Mall",
                .Loc = "Bengaluru",
                .Pin = 560001,
                .Stcd = "29"
            },
            .BuyerDtls = New PartyDetails With {
                .Gstin = "07ABCDE5678K1Z2",
                .LglNm = "ABC Ltd",
                .TrdNm = "ABC Traders",
                .Addr1 = "456, Market Street",
                .Addr2 = "Near Bus Stand",
                .Loc = "Delhi",
                .Pin = 110001,
                .Stcd = "07"
            },
            .ItemList = New List(Of Item) From {
                New Item With {
                    .SlNo = "1",
                    .PrdDesc = "Laptop",
                    .IsServc = "N",
                    .HsnCd = "84713010",
                    .Qty = 1,
                    .Unit = "NOS",
                    .UnitPrice = 50000,
                    .TotAmt = 50000,
                    .Discount = 0,
                    .PreTaxVal = 50000,
                    .AssAmt = 50000,
                    .GstRt = 18,
                    .IgstAmt = 0,
                    .CgstAmt = 4500,
                    .SgstAmt = 4500,
                    .TotItemVal = 59000
                }
            },
            .ValDtls = New ValueDetails With {
                .AssVal = 50000,
                .CgstVal = 4500,
                .SgstVal = 4500,
                .IgstVal = 0,
                .TotInvVal = 59000
            },
            .PayDtls = New PaymentDetails With {
                .Nm = "XYZ Bank",
                .AccNo = "123456789",
                .Mode = "Cash",
                .Ifsc = "XYZB0001234"
            },
            .EwbDtls = New EWayBillDetails With {
                .TransId = "T12345",
                .TransName = "XYZ Transport",
                .TransMode = "1",
                .Distance = 200,
                .VehNo = "KA01AB1234",
                .VehType = "R"
            }
        }

        ' Convert the e-Invoice object to JSON format
        Dim jsonOutput As String = JsonConvert.SerializeObject(eInvoice, Formatting.Indented)

        ' Save JSON to a file
        Dim filePath As String = "d:\temp\eInvoice.json"
        File.WriteAllText(filePath, jsonOutput)

        ' Output JSON to console (optional)
        Console.WriteLine("E-Invoice JSON generated successfully!")
        Console.WriteLine(jsonOutput)
        Console.ReadLine()
    End Sub

    ' Define classes for e-Invoice JSON structure
    Public Class EInvoice
        Public Property Version As String
        Public Property TranDtls As TransactionDetails
        Public Property DocDtls As DocumentDetails
        Public Property SellerDtls As PartyDetails
        Public Property BuyerDtls As PartyDetails
        Public Property ItemList As List(Of Item)
        Public Property ValDtls As ValueDetails
        Public Property PayDtls As PaymentDetails
        Public Property EwbDtls As EWayBillDetails
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

    Public Class Item
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

    Public Class PaymentDetails
        Public Property Nm As String
        Public Property AccNo As String
        Public Property Mode As String
        Public Property Ifsc As String
    End Class

    Public Class EWayBillDetails
        Public Property TransId As String
        Public Property TransName As String
        Public Property TransMode As String
        Public Property Distance As Integer
        Public Property VehNo As String
        Public Property VehType As String
    End Class

End Module


