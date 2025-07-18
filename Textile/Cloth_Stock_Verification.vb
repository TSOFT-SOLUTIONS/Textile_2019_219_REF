Public Class Cloth_Stock_Verification
    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)

    Private Sub Cloth_Stock_Verification_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        con.Open()

        ClothStock_Verification()

        ClothPosting_Verification()

        PieceChecking_To_PackingSlip_Verification()

        PackingSlip_To_Invoice_Verification()

    End Sub

    Private Sub Cloth_Stock_Verification_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        With dgv_Stock
            If .Visible = True And .Enabled = True Then
                If .Rows.Count = 0 Then .Rows.Add()
                .Focus()
                .CurrentCell = .Rows(0).Cells(0)
            End If
            If .Rows.Count > 0 Then
                .Focus()
                .CurrentCell = .Rows(0).Cells(0)
            End If
        End With
    End Sub

    Private Sub Cloth_Stock_Verification_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        Try
            If Asc(e.KeyChar) = 27 Then
                btn_close_Click(sender, e)
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Cloth_Stock_Verification_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()

    End Sub


    Private Sub btn_Calculate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Calculate.Click
        ClothStock_Verification()

        ClothPosting_Verification()

        PieceChecking_To_PackingSlip_Verification()

        PackingSlip_To_Invoice_Verification()
    End Sub


    Private Sub btn_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub ClothStock_Verification()
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Cmd As New SqlClient.SqlCommand
        Dim I As Integer = 0
        Dim n As Integer = 0
        Dim SNo As Integer = 0
        Dim Par_Name As String = ""
        Dim Cl_Name As String = ""
        Dim Tot_UcPcsBale As Double = 0
        Dim Tot_StkPost As Double = 0
        Dim Tot_Diff As Double = 0
        Dim OpYrCode As String = ""
        Dim SQL1 As String = ""
        Dim GrpBy_ColNm As String = ""


        OpYrCode = Microsoft.VisualBasic.Left(Trim(Common_Procedures.FnRange), 4)
        OpYrCode = Trim(Mid(Val(OpYrCode) - 1, 3, 2)) & "-" & Trim(Microsoft.VisualBasic.Right(OpYrCode, 2))

        Cmd.Connection = con

        Cmd.CommandText = "truncate table " & Trim(Common_Procedures.ReportTempSubTable) & ""
        Cmd.ExecuteNonQuery()

        Cmd.CommandText = "truncate table " & Trim(Common_Procedures.ReportTempTable) & ""
        Cmd.ExecuteNonQuery()

        '-----------------------------------------------------------------------------------------------------------------------------------------------------------------
        '**************************************      UNCHECKED      **************************************
        '-----------------------------------------------------------------------------------------------------------------------------------------------------------------
        '---InWard (Weaver Cloth Receipt)
        Cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & " ( Int3, Int1, Int2, Int4, Meters1 ) select a.Company_IdNo, a.StockOff_IdNo, a.cloth_idno, a.WareHouse_IdNo, sum(a.ReceiptMeters_Receipt) from Weaver_Cloth_Receipt_Head a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON tP.Ledger_Type = 'GODOWN' and a.WareHouse_IdNo <> 0 and a.WareHouse_IdNo = tP.Ledger_IdNo INNER JOIN Ledger_Head tSP ON a.StockOff_IdNo <> 0 and a.StockOff_IdNo = tSP.Ledger_IdNo INNER JOIN Cloth_Head tQ ON a.Cloth_IdNo <> 0 and a.Cloth_IdNo = tQ.Cloth_IdNo Where a.ReceiptMeters_Receipt <> 0 group by a.Company_IdNo, a.StockOff_IdNo, a.cloth_idno, a.WareHouse_IdNo having sum(a.ReceiptMeters_Receipt)  <> 0 "
        Cmd.ExecuteNonQuery()
        '---OutWard (Weaver Cloth Receipt)
        Cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & " ( Int3, Int1, Int2, Int4, Meters1 ) select a.Company_IdNo, a.StockOff_IdNo, a.cloth_idno, a.WareHouse_IdNo, -1*sum(a.ReceiptMeters_Receipt) from Weaver_Cloth_Receipt_Head a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON tP.Ledger_Type = 'GODOWN' and a.WareHouse_IdNo <> 0 and a.WareHouse_IdNo = tP.Ledger_IdNo INNER JOIN Ledger_Head tSP ON a.StockOff_IdNo <> 0 and a.StockOff_IdNo = tSP.Ledger_IdNo INNER JOIN Cloth_Head tQ ON a.Cloth_IdNo <> 0 and a.Cloth_IdNo = tQ.Cloth_IdNo Where a.Weaver_Piece_Checking_Code <> '' and a.ReceiptMeters_Receipt <> 0 group by a.Company_IdNo, a.StockOff_IdNo, a.cloth_idno, a.WareHouse_IdNo having sum(a.ReceiptMeters_Receipt)  <> 0 "
        Cmd.ExecuteNonQuery()

        '---InWard (Cloth Purchase)
        Cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & " ( Int3, Int1, Int2, Int4, Meters1 ) select a.Company_IdNo, a.StockOff_IdNo, a.cloth_idno, a.WareHouse_IdNo, sum(a.ReceiptMeters_Receipt) from Cloth_Purchase_Receipt_Head a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON tP.Ledger_Type = 'GODOWN' and a.WareHouse_IdNo <> 0 and a.WareHouse_IdNo = tP.Ledger_IdNo INNER JOIN Ledger_Head tSP ON a.StockOff_IdNo <> 0 and a.StockOff_IdNo = tSP.Ledger_IdNo INNER JOIN Cloth_Head tQ ON a.Cloth_IdNo <> 0 and a.Cloth_IdNo = tQ.Cloth_IdNo Where a.ReceiptMeters_Receipt <> 0 group by a.Company_IdNo, a.StockOff_IdNo, a.cloth_idno, a.WareHouse_IdNo having sum(a.ReceiptMeters_Receipt)  <> 0 "
        Cmd.ExecuteNonQuery()
        '---OutWard (Cloth Purchase)
        Cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & " ( Int3, Int1, Int2, Int4, Meters1 ) select a.Company_IdNo, a.StockOff_IdNo, a.cloth_idno, a.WareHouse_IdNo, -1*sum(a.ReceiptMeters_Receipt) from Cloth_Purchase_Receipt_Head a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON tP.Ledger_Type = 'GODOWN' and a.WareHouse_IdNo <> 0 and a.WareHouse_IdNo = tP.Ledger_IdNo INNER JOIN Ledger_Head tSP ON a.StockOff_IdNo <> 0 and a.StockOff_IdNo = tSP.Ledger_IdNo INNER JOIN Cloth_Head tQ ON a.Cloth_IdNo <> 0 and a.Cloth_IdNo = tQ.Cloth_IdNo Where a.Weaver_Piece_Checking_Code <> '' and a.ReceiptMeters_Receipt <> 0 group by a.Company_IdNo, a.StockOff_IdNo, a.cloth_idno, a.WareHouse_IdNo having sum(a.ReceiptMeters_Receipt)  <> 0 "
        Cmd.ExecuteNonQuery()

        '---InWard (Cloth Sale Delivery Return)
        Cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & " ( Int3, Int1, Int2, Int4, Meters1 ) select a.Company_IdNo, a.StockOff_IdNo, a.cloth_idno, a.WareHouse_IdNo, sum(a.Return_Meters) from ClothSales_Delivery_Return_Head a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON tP.Ledger_Type = 'GODOWN' and a.WareHouse_IdNo <> 0 and a.WareHouse_IdNo = tP.Ledger_IdNo INNER JOIN Ledger_Head tSP ON a.StockOff_IdNo <> 0 and a.StockOff_IdNo = tSP.Ledger_IdNo INNER JOIN Cloth_Head tQ ON a.Cloth_IdNo <> 0 and a.Cloth_IdNo = tQ.Cloth_IdNo Where a.Return_Meters <> 0 or a.Total_Checking_Meters <> 0 group by a.Company_IdNo, a.StockOff_IdNo, a.cloth_idno, a.WareHouse_IdNo having sum(a.Return_Meters)  <> 0 "
        Cmd.ExecuteNonQuery()
        '---OutWard (Cloth Sale Delivery Return)
        Cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & " ( Int3, Int1, Int2, Int4, Meters1 ) select a.Company_IdNo, a.StockOff_IdNo, a.cloth_idno, a.WareHouse_IdNo, -1*sum(a.Return_Meters) from ClothSales_Delivery_Return_Head a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON tP.Ledger_Type = 'GODOWN' and a.WareHouse_IdNo <> 0 and a.WareHouse_IdNo = tP.Ledger_IdNo INNER JOIN Ledger_Head tSP ON a.StockOff_IdNo <> 0 and a.StockOff_IdNo = tSP.Ledger_IdNo INNER JOIN Cloth_Head tQ ON a.Cloth_IdNo <> 0 and a.Cloth_IdNo = tQ.Cloth_IdNo Where a.Weaver_Piece_Checking_Code <> '' and a.Return_Meters <> 0 group by a.Company_IdNo, a.StockOff_IdNo, a.cloth_idno, a.WareHouse_IdNo having sum(a.Return_Meters)  <> 0 "
        Cmd.ExecuteNonQuery()

        '---InWard (Cloth Sale Invoice Return)
        Cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & " ( Int3, Int1, Int2, Int4, Meters1 ) select a.Company_IdNo, a.StockOff_IdNo, a.cloth_idno, a.WareHouse_IdNo, sum(a.Return_Meters) from ClothSales_Return_Head a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON tP.Ledger_Type = 'GODOWN' and a.WareHouse_IdNo <> 0 and a.WareHouse_IdNo = tP.Ledger_IdNo INNER JOIN Ledger_Head tSP ON a.StockOff_IdNo <> 0 and a.StockOff_IdNo = tSP.Ledger_IdNo INNER JOIN Cloth_Head tQ ON a.Cloth_IdNo <> 0 and a.Cloth_IdNo = tQ.Cloth_IdNo Where a.Return_Meters <> 0 group by a.Company_IdNo, a.StockOff_IdNo, a.cloth_idno, a.WareHouse_IdNo having sum(a.Return_Meters)  <> 0 "
        Cmd.ExecuteNonQuery()
        '---OutWard (Cloth Sale Invoice Return)
        Cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & " ( Int3, Int1, Int2, Int4, Meters1 ) select a.Company_IdNo, a.StockOff_IdNo, a.cloth_idno, a.WareHouse_IdNo, -1*sum(a.Return_Meters) from ClothSales_Return_Head a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON tP.Ledger_Type = 'GODOWN' and a.WareHouse_IdNo <> 0 and a.WareHouse_IdNo = tP.Ledger_IdNo INNER JOIN Ledger_Head tSP ON a.StockOff_IdNo <> 0 and a.StockOff_IdNo = tSP.Ledger_IdNo INNER JOIN Cloth_Head tQ ON a.Cloth_IdNo <> 0 and a.Cloth_IdNo = tQ.Cloth_IdNo Where a.Weaver_Piece_Checking_Code <> '' and a.Return_Meters <> 0 group by a.Company_IdNo, a.StockOff_IdNo, a.cloth_idno, a.WareHouse_IdNo having sum(a.Return_Meters)  <> 0 "
        Cmd.ExecuteNonQuery()

        '---InWard (from Individual Pcs)
        SQL1 = "Insert into " & Trim(Common_Procedures.ReportTempTable) & "(Int3, Int1, Int2, Int4, Meters1) select a.Company_IdNo, a.StockOff_IdNo, a.cloth_idno, a.WareHouse_IdNo, sum(a.Receipt_Meters) from Weaver_ClothReceipt_Piece_Details a  INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON tP.Ledger_Type = 'GODOWN' and a.WareHouse_IdNo <> 0 and a.WareHouse_IdNo = tP.Ledger_IdNo INNER JOIN Ledger_Head tSP ON a.StockOff_IdNo <> 0 and a.StockOff_IdNo = tSP.Ledger_IdNo INNER JOIN Cloth_Head tQ ON a.Cloth_IdNo <> 0 and a.Cloth_IdNo = tQ.Cloth_IdNo Where a.Receipt_Meters <> 0 and (a.Type1_Meters + a.Type2_Meters + a.Type3_Meters + a.Type4_Meters + a.Type5_Meters ) = 0 group by a.Company_IdNo, a.StockOff_IdNo, a.cloth_idno, a.WareHouse_IdNo Having sum(a.Receipt_Meters) <> 0 "
        Cmd.CommandText = "EXEC SP_ExecuteQuery '" & Replace(Trim(SQL1), "'", "''") & "'"
        Cmd.ExecuteNonQuery()
        'Cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & "(Int3, Int1, Int2, Int4, Meters1) select a.Company_IdNo, a.StockOff_IdNo, a.cloth_idno, a.WareHouse_IdNo, sum(a.Receipt_Meters) from Weaver_ClothReceipt_Piece_Details a  INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON tP.Ledger_Type = 'GODOWN' and a.WareHouse_IdNo <> 0 and a.WareHouse_IdNo = tP.Ledger_IdNo INNER JOIN Ledger_Head tSP ON a.StockOff_IdNo <> 0 and a.StockOff_IdNo = tSP.Ledger_IdNo INNER JOIN Cloth_Head tQ ON a.Cloth_IdNo <> 0 and a.Cloth_IdNo = tQ.Cloth_IdNo Where a.Receipt_Meters <> 0 and (a.Type1_Meters + a.Type2_Meters + a.Type3_Meters + a.Type4_Meters + a.Type5_Meters ) = 0 group by a.Company_IdNo, a.StockOff_IdNo, a.cloth_idno, a.WareHouse_IdNo having sum(a.Receipt_Meters) <> 0 "
        'Cmd.ExecuteNonQuery()

        '---Unchecked
        'Cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & "(Int1, Int2, Meters1) select (CASE WHEN b.ledger_type = 'JOBWORKER' THEN a.ledger_idno ELSE " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & " END ), a.cloth_idno, sum(a.Receipt_Meters) from Weaver_Cloth_Receipt_Head a, Ledger_Head b where a.Weaver_Piece_Checking_Code = '' and a.ledger_idno = b.ledger_idno group by b.ledger_type, a.ledger_idno, a.Cloth_IdNo having sum(a.Receipt_Meters) <> 0 "
        ''Cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & "(Int1, Meters1) select cloth_idno, sum(a.Receipt_Meters) from Weaver_ClothReceipt_Piece_Details a where (a.Type1_Meters + a.Type2_Meters + a.Type3_Meters + a.Type4_Meters + a.Type5_Meters) = 0 group by Cloth_IdNo having sum(a.Receipt_Meters) <> 0 "
        'Cmd.ExecuteNonQuery()

        '---Piece Stock
        SQL1 = "insert into " & Trim(Common_Procedures.ReportTempTable) & " (Int1, Int2, Meters2) select a.StockOff_IdNo, a.Cloth_Idno, sum(a.Type1_Meters) from Weaver_ClothReceipt_Piece_Details a where a.PackingSlip_Code_Type1 = '' and a.Type1_Meters <> 0 group by a.StockOff_IdNo, a.Cloth_Idno"
        Cmd.CommandText = "EXEC SP_ExecuteQuery '" & Replace(Trim(SQL1), "'", "''") & "'"
        Cmd.ExecuteNonQuery()
        SQL1 = "insert into " & Trim(Common_Procedures.ReportTempTable) & " (Int1, Int2, Meters3) select a.StockOff_IdNo, a.Cloth_Idno, sum(a.Type2_Meters) from Weaver_ClothReceipt_Piece_Details a where a.PackingSlip_Code_Type2 = '' and a.Type2_Meters <> 0 group by a.StockOff_IdNo, a.Cloth_Idno"
        Cmd.CommandText = "EXEC SP_ExecuteQuery '" & Replace(Trim(SQL1), "'", "''") & "'"
        Cmd.ExecuteNonQuery()
        SQL1 = "insert into " & Trim(Common_Procedures.ReportTempTable) & " (Int1, Int2, Meters5) select a.StockOff_IdNo, a.Cloth_Idno, sum(a.Type4_Meters) from Weaver_ClothReceipt_Piece_Details a where a.PackingSlip_Code_Type4 = '' and a.Type4_Meters <> 0 group by a.StockOff_IdNo, a.Cloth_Idno"
        Cmd.CommandText = "EXEC SP_ExecuteQuery '" & Replace(Trim(SQL1), "'", "''") & "'"
        Cmd.ExecuteNonQuery()
        SQL1 = "insert into " & Trim(Common_Procedures.ReportTempTable) & " (Int1, Int2, Meters5) select a.StockOff_IdNo, a.Cloth_Idno, sum(a.Type4_Meters) from Weaver_ClothReceipt_Piece_Details a where a.PackingSlip_Code_Type4 = '' and a.Type4_Meters <> 0 group by a.StockOff_IdNo, a.Cloth_Idno"
        Cmd.CommandText = "EXEC SP_ExecuteQuery '" & Replace(Trim(SQL1), "'", "''") & "'"
        Cmd.ExecuteNonQuery()
        SQL1 = "insert into " & Trim(Common_Procedures.ReportTempTable) & " (Int1, Int2, Meters6) select a.StockOff_IdNo, a.Cloth_Idno, sum(a.Type5_Meters) from Weaver_ClothReceipt_Piece_Details a where a.PackingSlip_Code_Type5 = '' and a.Type5_Meters <> 0 group by a.StockOff_IdNo, a.Cloth_Idno"
        Cmd.CommandText = "EXEC SP_ExecuteQuery '" & Replace(Trim(SQL1), "'", "''") & "'"
        Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "insert into " & Trim(Common_Procedures.ReportTempTable) & " (Int1, Int2, Meters2) select a.StockOff_IdNo, a.Cloth_Idno, sum(a.Type1_Meters) from Weaver_ClothReceipt_Piece_Details a where a.PackingSlip_Code_Type1 = '' and a.Type1_Meters <> 0 group by a.StockOff_IdNo, a.Cloth_Idno"
        'Cmd.ExecuteNonQuery()
        'Cmd.CommandText = "insert into " & Trim(Common_Procedures.ReportTempTable) & " (Int1, Int2, Meters3) select a.StockOff_IdNo, a.Cloth_Idno, sum(a.Type2_Meters) from Weaver_ClothReceipt_Piece_Details a where a.PackingSlip_Code_Type2 = '' and a.Type2_Meters <> 0 group by a.StockOff_IdNo, a.Cloth_Idno"
        'Cmd.ExecuteNonQuery()
        'Cmd.CommandText = "insert into " & Trim(Common_Procedures.ReportTempTable) & " (Int1, Int2, Meters4) select a.StockOff_IdNo, a.Cloth_Idno, sum(a.Type3_Meters) from Weaver_ClothReceipt_Piece_Details a where a.PackingSlip_Code_Type3 = '' and a.Type3_Meters <> 0 group by a.StockOff_IdNo, a.Cloth_Idno"
        'Cmd.ExecuteNonQuery()
        'Cmd.CommandText = "insert into " & Trim(Common_Procedures.ReportTempTable) & " (Int1, Int2, Meters5) select a.StockOff_IdNo, a.Cloth_Idno, sum(a.Type4_Meters) from Weaver_ClothReceipt_Piece_Details a where a.PackingSlip_Code_Type4 = '' and a.Type4_Meters <> 0 group by a.StockOff_IdNo, a.Cloth_Idno"
        'Cmd.ExecuteNonQuery()
        'Cmd.CommandText = "insert into " & Trim(Common_Procedures.ReportTempTable) & " (Int1, Int2, Meters6) select a.StockOff_IdNo, a.Cloth_Idno, sum(a.Type5_Meters) from Weaver_ClothReceipt_Piece_Details a where a.PackingSlip_Code_Type5 = '' and a.Type5_Meters <> 0 group by a.StockOff_IdNo, a.Cloth_Idno"
        'Cmd.ExecuteNonQuery()


        '---Baled
        '--- From bale Opening
        Cmd.CommandText = "insert into " & Trim(Common_Procedures.ReportTempTable) & " (Int1, Int2, Meters2) select a.Ledger_IdNo, a.Cloth_Idno, sum(a.Total_Meters) from Packing_Slip_Head a where a.Delivery_Code = '' and a.ClothType_IdNo = 1 and a.Packing_Slip_Code like '%/" & Trim(OpYrCode) & "' group by a.Ledger_IdNo, a.Cloth_Idno"
        Cmd.ExecuteNonQuery()
        Cmd.CommandText = "insert into " & Trim(Common_Procedures.ReportTempTable) & " (Int1, Int2, Meters3) select a.Ledger_IdNo, a.Cloth_Idno, sum(a.Total_Meters) from Packing_Slip_Head a where a.Delivery_Code = '' and a.ClothType_IdNo = 2 and a.Packing_Slip_Code like '%/" & Trim(OpYrCode) & "' group by a.Ledger_IdNo, a.Cloth_Idno"
        Cmd.ExecuteNonQuery()
        Cmd.CommandText = "insert into " & Trim(Common_Procedures.ReportTempTable) & " (Int1, Int2, Meters4) select a.Ledger_IdNo, a.Cloth_Idno, sum(a.Total_Meters) from Packing_Slip_Head a where a.Delivery_Code = '' and a.ClothType_IdNo = 3 and a.Packing_Slip_Code like '%/" & Trim(OpYrCode) & "' group by a.Ledger_IdNo, a.Cloth_Idno"
        Cmd.ExecuteNonQuery()
        Cmd.CommandText = "insert into " & Trim(Common_Procedures.ReportTempTable) & " (Int1, Int2, Meters5) select a.Ledger_IdNo, a.Cloth_Idno, sum(a.Total_Meters) from Packing_Slip_Head a where a.Delivery_Code = '' and a.ClothType_IdNo = 4 and a.Packing_Slip_Code like '%/" & Trim(OpYrCode) & "' group by a.Ledger_IdNo, a.Cloth_Idno"
        Cmd.ExecuteNonQuery()
        Cmd.CommandText = "insert into " & Trim(Common_Procedures.ReportTempTable) & " (Int1, Int2, Meters6) select a.Ledger_IdNo, a.Cloth_Idno, sum(a.Total_Meters) from Packing_Slip_Head a where a.Delivery_Code = '' and a.ClothType_IdNo = 5 and a.Packing_Slip_Code like '%/" & Trim(OpYrCode) & "' group by a.Ledger_IdNo, a.Cloth_Idno"
        Cmd.ExecuteNonQuery()

        '--- From packing Slip Entry
        Cmd.CommandText = "insert into " & Trim(Common_Procedures.ReportTempTable) & " (Int1, Int2, Meters2) select b.Ledger_IdNo, a.Cloth_Idno, sum(a.Meters) from Packing_Slip_Details a, Packing_Slip_Head b where b.Delivery_Code = '' and a.ClothType_IdNo = 1 and a.Company_IdNo = b.Company_IdNo and a.Packing_Slip_Code = b.Packing_Slip_Code and b.Packing_Slip_Code not like '%/" & Trim(OpYrCode) & "' group by b.Ledger_IdNo, a.Cloth_Idno"
        Cmd.ExecuteNonQuery()
        Cmd.CommandText = "insert into " & Trim(Common_Procedures.ReportTempTable) & " (Int1, Int2, Meters3) select b.Ledger_IdNo, a.Cloth_Idno, sum(a.Meters) from Packing_Slip_Details a, Packing_Slip_Head b where b.Delivery_Code = '' and a.ClothType_IdNo = 2 and a.Company_IdNo = b.Company_IdNo and a.Packing_Slip_Code = b.Packing_Slip_Code and b.Packing_Slip_Code not like '%/" & Trim(OpYrCode) & "' group by b.Ledger_IdNo, a.Cloth_Idno"
        Cmd.ExecuteNonQuery()
        Cmd.CommandText = "insert into " & Trim(Common_Procedures.ReportTempTable) & " (Int1, Int2, Meters4) select b.Ledger_IdNo, a.Cloth_Idno, sum(a.Meters) from Packing_Slip_Details a, Packing_Slip_Head b where b.Delivery_Code = '' and a.ClothType_IdNo = 3 and a.Company_IdNo = b.Company_IdNo and a.Packing_Slip_Code = b.Packing_Slip_Code and b.Packing_Slip_Code not like '%/" & Trim(OpYrCode) & "' group by b.Ledger_IdNo, a.Cloth_Idno"
        Cmd.ExecuteNonQuery()
        Cmd.CommandText = "insert into " & Trim(Common_Procedures.ReportTempTable) & " (Int1, Int2, Meters5) select b.Ledger_IdNo, a.Cloth_Idno, sum(a.Meters) from Packing_Slip_Details a, Packing_Slip_Head b where b.Delivery_Code = '' and a.ClothType_IdNo = 4 and a.Company_IdNo = b.Company_IdNo and a.Packing_Slip_Code = b.Packing_Slip_Code and b.Packing_Slip_Code not like '%/" & Trim(OpYrCode) & "' group by b.Ledger_IdNo, a.Cloth_Idno"
        Cmd.ExecuteNonQuery()
        Cmd.CommandText = "insert into " & Trim(Common_Procedures.ReportTempTable) & " (Int1, Int2, Meters6) select b.Ledger_IdNo, a.Cloth_Idno, sum(a.Meters) from Packing_Slip_Details a, Packing_Slip_Head b where b.Delivery_Code = '' and a.ClothType_IdNo = 5 and a.Company_IdNo = b.Company_IdNo and a.Packing_Slip_Code = b.Packing_Slip_Code and b.Packing_Slip_Code not like '%/" & Trim(OpYrCode) & "' group by b.Ledger_IdNo, a.Cloth_Idno"
        Cmd.ExecuteNonQuery()


        'Cloth Stock From ClothProcessing



        Cmd.CommandText = "Truncate table " & Trim(Common_Procedures.ReportTempSubTable) & ""
        Cmd.ExecuteNonQuery()

        Cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempSubTable) & " (Int1, Name1, Int2, Name2 , Name3 , Weight1, meters1, meters2, meters3, meters4, meters5) Select tQ.cloth_IdNo , tQ.cloth_name , tSP.Ledger_IdNo as StockOff_IdNo , tSP.Ledger_Name  as StockOff_Name , tP.Ledger_Name as GodownName_StockIN, sum(a.UnChecked_Meters), sum(a.Meters_Type1), sum(a.Meters_Type2), sum(a.Meters_Type3), sum(a.Meters_Type4), sum(a.Meters_Type5) from Stock_Cloth_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON a.DeliveryTo_Idno <> 0 and a.DeliveryTo_Idno = tP.Ledger_IdNo INNER JOIN Ledger_Head tSP ON a.StockOff_IdNo <> 0 and a.StockOff_IdNo = tSP.Ledger_IdNo INNER JOIN Cloth_Head tQ ON a.Cloth_IdNo <> 0 and a.Cloth_IdNo = tQ.Cloth_IdNo Where tP.Ledger_Type = 'GODOWN' and ( a.UnChecked_Meters <> 0 or a.Meters_Type1 <> 0 or a.Meters_Type2 <> 0 or a.Meters_Type3 <> 0 or a.Meters_Type4 <> 0 or a.Meters_Type5 <> 0 ) group by tQ.cloth_IdNo , tQ.cloth_name, tSP.Ledger_IdNo, tSP.Ledger_Name , tP.Ledger_Name  having sum(a.UnChecked_Meters)  <> 0 or  sum(a.Meters_Type1)  <> 0 or  sum(a.Meters_Type2)  <> 0 or  sum(a.Meters_Type3) <> 0 or  sum(a.Meters_Type4) <> 0 or sum(a.Meters_Type5) <> 0"
        Cmd.ExecuteNonQuery()
        Cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempSubTable) & " ( Int1, Name1, Int2, Name2 ,  Name3 , Weight1, meters1, meters2, meters3, meters4, meters5) Select tQ.cloth_IdNo , tQ.cloth_name , tSP.Ledger_IdNo as StockOff_IdNo , tSP.Ledger_Name as StockOff_Name , tP.Ledger_Name as GodownName_StockIN, -1*sum(a.UnChecked_Meters), -1*sum(a.Meters_Type1), -1*sum(a.Meters_Type2), -1*sum(a.Meters_Type3), -1*sum(a.Meters_Type4), -1*sum(a.Meters_Type5) from Stock_Cloth_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON a.ReceivedFrom_Idno <> 0 and a.ReceivedFrom_Idno = tP.Ledger_IdNo  INNER JOIN Ledger_Head tSP ON a.StockOff_IdNo <> 0 and a.StockOff_IdNo = tSP.Ledger_IdNo INNER JOIN Cloth_Head tQ ON a.Cloth_IdNo <> 0 and a.Cloth_IdNo = tQ.Cloth_IdNo Where tP.Ledger_Type = 'GODOWN' and ( a.UnChecked_Meters <> 0 or a.Meters_Type1 <> 0 or a.Meters_Type2 <> 0 or a.Meters_Type3 <> 0 or a.Meters_Type4 <> 0 or a.Meters_Type5 <> 0 ) group by tQ.cloth_IdNo , tQ.cloth_name , tSP.Ledger_IdNo, tSP.Ledger_Name, tP.Ledger_Name  having sum(a.UnChecked_Meters)  <> 0 or  sum(a.Meters_Type1)  <> 0 or  sum(a.Meters_Type2)  <> 0 or  sum(a.Meters_Type3) <> 0 or  sum(a.Meters_Type4) <> 0 or sum(a.Meters_Type5) <> 0"
        Cmd.ExecuteNonQuery()

        'If Trim(LCase(RptIpDet_ReportName)) = Trim(LCase("Godown Cloth Stock Summary - GodownWise")) Then
        '    Cmd.CommandText = "Update " & Trim(Common_Procedures.ReportTempSubTable) & " set Int2 = 5 Where Int2 = 4"
        '    Cmd.ExecuteNonQuery()

        '    GrpBy_ColNm = ", Name3"

        'Else
        Cmd.CommandText = "Update " & Trim(Common_Procedures.ReportTempSubTable) & " set Int2 = 4 Where Int2 = 5"
        Cmd.ExecuteNonQuery()

        GrpBy_ColNm = ""
        'End If

        Cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & " ( Int1, Int2, Currency1, Currency2, Currency3, Currency4, Currency5, Currency6 ) Select a.Int2,  a.Int1, sum(a.Weight1), sum(a.meters1), sum(a.meters2), sum(a.meters3), sum(a.meters4), sum(a.meters5) from " & Trim(Common_Procedures.ReportTempSubTable) & " a, Ledger_Head tSP Where a.Int2 = tSP.Ledger_IdNo group by a.Int1, a.Int2 having sum(a.Weight1) <> 0 or sum(a.meters1) <> 0 or sum(a.meters2) <> 0 or sum(a.meters3) <> 0 or sum(a.meters4) <> 0 or sum(a.meters5) <> 0 "
        'Cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & " ( Weight1, Name1, Name2, Name3, currency1, meters1, meters2, meters3, meters4, meters5 ) Select (case when tSP.Ledger_Type = 'GODOWN' OR tSP.Ledger_Type = 'OWNSORT' THEN 1 ELSE 2 END), a.Name1, tSP.Ledger_Name  as StockOff_Name ,  " & IIf(Trim(GrpBy_ColNm) <> "", "a.Name3", "''") & " , sum(a.Weight1), sum(a.meters1), sum(a.meters2), sum(a.meters3), sum(a.meters4), sum(a.meters5) from " & Trim(Common_Procedures.ReportTempSubTable) & " a, Ledger_Head tSP Where a.Int2 = tSP.Ledger_IdNo group by tSP.Ledger_Type, a.name1 , tSP.Ledger_Name " & GrpBy_ColNm & " having sum(a.Weight1) <> 0 or sum(a.meters1) <> 0 or sum(a.meters2) <> 0 or sum(a.meters3) <> 0 or sum(a.meters4) <> 0 or sum(a.meters5) <> 0 "
        Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "insert into " & Trim(Common_Procedures.ReportTempTable) & "(Int1, Int2, Currency1, Currency2, Currency3, Currency4, Currency5, Currency6) select a.StockOff_IdNo, a.cloth_idno, sum(a.UnChecked_Meters), sum(a.Meters_Type1),  sum(a.Meters_Type2) , sum(a.Meters_Type3) , sum(a.Meters_Type4) , sum(a.Meters_Type5) from Stock_Cloth_Processing_Details a where a.StockOff_IdNo <> 0 and a.DeliveryTo_Idno = " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & " group by a.StockOff_IdNo, a.cloth_idno"
        'Cmd.ExecuteNonQuery()
        'Cmd.CommandText = "insert into " & Trim(Common_Procedures.ReportTempTable) & "(Int1, Int2, Currency1, Currency2, Currency3, Currency4, Currency5, Currency6) select a.StockOff_IdNo, a.cloth_idno, -1*sum(a.UnChecked_Meters), -1*sum(a.Meters_Type1),  -1*sum(a.Meters_Type2) , -1*sum(a.Meters_Type3) , -1*sum(a.Meters_Type4) , -1*sum(a.Meters_Type5) from Stock_Cloth_Processing_Details a where a.StockOff_IdNo <> 0 and a.ReceivedFrom_Idno = " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & " group by a.StockOff_IdNo, a.cloth_idno"
        'Cmd.ExecuteNonQuery()

        Cmd.CommandText = "Update " & Trim(Common_Procedures.ReportTempTable) & " set Int1 = 5 Where Int1 = 4"
        Cmd.ExecuteNonQuery()

        Da1 = New SqlClient.SqlDataAdapter("Select b.Ledger_Name, c.Cloth_Name, sum(Meters1) as UcPcsBale_UC, sum(Meters2) as UcPcsBale_Type1, sum(Meters3) as UcPcsBale_Type2, sum(Meters4) as UcPcsBale_Type3, sum(Meters5) as UcPcsBale_Type4, sum(Meters6) as UcPcsBale_Type5, sum(Currency1) as StkPost_UC, sum(Currency2) as StkPost_type1, sum(Currency3) as StkPost_type2, sum(Currency4) as StkPost_type3, sum(Currency5) as StkPost_type4, sum(Currency6) as StkPost_type5 from " & Trim(Common_Procedures.ReportTempTable) & " a, Ledger_Head b, Cloth_Head c Where b.Ledger_Idno <> 0 and a.Int1 = b.Ledger_Idno and c.Cloth_IdNo <> 0 and a.Int2 = c.Cloth_IdNo group by b.Ledger_Name, c.Cloth_Name having sum(Meters1) <> 0 or sum(Meters2) <> 0 or sum(Meters3) <> 0 or sum(Meters4) <> 0 or sum(Meters5) <> 0 or sum(Meters6) <> 0 or sum(Currency1) <> 0 or sum(Currency2) <> 0 or sum(Currency3) <> 0 or sum(Currency4) <> 0 or sum(Currency5) <> 0 or sum(Currency6) <> 0 order by b.Ledger_Name, c.Cloth_Name", con)
        Dt1 = New DataTable
        Da1.Fill(Dt1)

        With dgv_Stock

            .Rows.Clear()
            dgv_StockTotal.Rows.Clear()
            dgv_StockTotal.Rows.Add()

            If Dt1.Rows.Count > 0 Then

                Par_Name = ""
                Cl_Name = ""
                SNo = 0
                For I = 0 To Dt1.Rows.Count - 1

                    If Trim(UCase(Par_Name)) <> Trim(UCase(Dt1.Rows(I).Item("Ledger_Name").ToString)) Then
                        If Trim(UCase(Par_Name)) <> "" Then
                            n = .Rows.Add()
                            .Rows(n).Cells(8).Style.BackColor = Color.LightGreen
                            .Rows(n).Cells(15).Style.BackColor = Color.LightGreen
                        End If
                        n = .Rows.Add()
                        .Rows(n).Cells(1).Value = Dt1.Rows(I).Item("Ledger_Name").ToString
                        .Rows(n).Cells(1).Style.ForeColor = Color.Red
                        .Rows(n).Cells(1).Style.Font = New Font("Tahoma", 12, FontStyle.Bold)
                        .Rows(n).Cells(8).Style.BackColor = Color.LightGreen
                        .Rows(n).Cells(15).Style.BackColor = Color.LightGreen
                    End If

                    n = .Rows.Add()

                    SNo = SNo + 1

                    .Rows(n).Cells(0).Value = "  " & Val(SNo)

                    .Rows(n).Cells(1).Value = Dt1.Rows(I).Item("Cloth_Name").ToString

                    .Rows(n).Cells(2).Value = Format(Val(Dt1.Rows(I).Item("UcPcsBale_UC").ToString), "#########0.00")
                    If Val(.Rows(n).Cells(2).Value) = 0 Then .Rows(n).Cells(2).Value = ""
                    .Rows(n).Cells(3).Value = Format(Val(Dt1.Rows(I).Item("UcPcsBale_Type1").ToString), "#########0.00")
                    If Val(.Rows(n).Cells(3).Value) = 0 Then .Rows(n).Cells(3).Value = ""
                    .Rows(n).Cells(4).Value = Format(Val(Dt1.Rows(I).Item("UcPcsBale_Type2").ToString), "#########0.00")
                    If Val(.Rows(n).Cells(4).Value) = 0 Then .Rows(n).Cells(4).Value = ""
                    .Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(I).Item("UcPcsBale_Type3").ToString), "#########0.00")
                    If Val(.Rows(n).Cells(5).Value) = 0 Then .Rows(n).Cells(5).Value = ""
                    .Rows(n).Cells(6).Value = Format(Val(Dt1.Rows(I).Item("UcPcsBale_Type4").ToString), "#########0.00")
                    If Val(.Rows(n).Cells(6).Value) = 0 Then .Rows(n).Cells(6).Value = ""
                    .Rows(n).Cells(7).Value = Format(Val(Dt1.Rows(I).Item("UcPcsBale_Type5").ToString), "#########0.00")
                    If Val(.Rows(n).Cells(7).Value) = 0 Then .Rows(n).Cells(7).Value = ""

                    Tot_UcPcsBale = Val(Dt1.Rows(I).Item("UcPcsBale_UC").ToString) + Val(Dt1.Rows(I).Item("UcPcsBale_Type1").ToString) + Val(Dt1.Rows(I).Item("UcPcsBale_Type2").ToString) + Val(Dt1.Rows(I).Item("UcPcsBale_Type3").ToString) + Val(Dt1.Rows(I).Item("UcPcsBale_Type4").ToString) + Val(Dt1.Rows(I).Item("UcPcsBale_Type5").ToString)

                    .Rows(n).Cells(8).Value = Format(Val(Tot_UcPcsBale), "#########0.00")
                    If Val(.Rows(n).Cells(8).Value) = 0 Then .Rows(n).Cells(8).Value = ""
                    .Rows(n).Cells(8).Style.BackColor = Color.LightGreen

                    .Rows(n).Cells(9).Value = Format(Val(Dt1.Rows(I).Item("StkPost_UC").ToString), "#########0.00")
                    If Val(.Rows(n).Cells(9).Value) = 0 Then .Rows(n).Cells(9).Value = ""
                    .Rows(n).Cells(10).Value = Format(Val(Dt1.Rows(I).Item("StkPost_type1").ToString), "#########0.00")
                    If Val(.Rows(n).Cells(10).Value) = 0 Then .Rows(n).Cells(10).Value = ""
                    .Rows(n).Cells(11).Value = Format(Val(Dt1.Rows(I).Item("StkPost_type2").ToString), "#########0.00")
                    If Val(.Rows(n).Cells(11).Value) = 0 Then .Rows(n).Cells(11).Value = ""
                    .Rows(n).Cells(12).Value = Format(Val(Dt1.Rows(I).Item("StkPost_type3").ToString), "#########0.00")
                    If Val(.Rows(n).Cells(12).Value) = 0 Then .Rows(n).Cells(12).Value = ""
                    .Rows(n).Cells(13).Value = Format(Val(Dt1.Rows(I).Item("StkPost_type4").ToString), "#########0.00")
                    If Val(.Rows(n).Cells(13).Value) = 0 Then .Rows(n).Cells(13).Value = ""
                    .Rows(n).Cells(14).Value = Format(Val(Dt1.Rows(I).Item("StkPost_type5").ToString), "#########0.00")
                    If Val(.Rows(n).Cells(14).Value) = 0 Then .Rows(n).Cells(14).Value = ""

                    Tot_StkPost = Val(Dt1.Rows(I).Item("StkPost_UC").ToString) + Val(Dt1.Rows(I).Item("StkPost_type1").ToString) + Val(Dt1.Rows(I).Item("StkPost_type2").ToString) + Val(Dt1.Rows(I).Item("StkPost_type3").ToString) + Val(Dt1.Rows(I).Item("StkPost_type4").ToString) + Val(Dt1.Rows(I).Item("StkPost_type5").ToString)
                    .Rows(n).Cells(15).Value = Format(Val(Tot_StkPost), "#########0.00")
                    If Val(.Rows(n).Cells(15).Value) = 0 Then .Rows(n).Cells(15).Value = ""
                    .Rows(n).Cells(15).Style.BackColor = Color.LightGreen

                    Tot_Diff = (Tot_UcPcsBale - Tot_StkPost)

                    .Rows(n).Cells(16).Value = Format(Val(Tot_Diff), "#########0.00")
                    '.Rows(n).Cells(16).Value = Format(Math.Abs(Val(Tot_Diff)), "#########0.00")
                    If Val(.Rows(n).Cells(16).Value) = 0 Then .Rows(n).Cells(16).Value = ""


                    Par_Name = Dt1.Rows(I).Item("Ledger_Name").ToString

                Next

            End If

            If .Rows.Count = 0 Then .Rows.Add()

            .Focus()
            .CurrentCell = .Rows(0).Cells(5)
            .CurrentCell.Selected = True

        End With
        Dt1.Clear()

        Total_Stock_Calculation()

        Cmd.Dispose()
        Dt1.Dispose()
        Da1.Dispose()

    End Sub

    Private Sub Total_Stock_Calculation()
        Dim Tt_UcPcsBale_Uc As Double = 0
        Dim Tt_UcPcsBale_Type1 As Double = 0
        Dim Tt_UcPcsBale_Type2 As Double = 0
        Dim Tt_UcPcsBale_Type3 As Double = 0
        Dim Tt_UcPcsBale_Type4 As Double = 0
        Dim Tt_UcPcsBale_Type5 As Double = 0
        Dim Tt_StkPost_Uc As Double = 0
        Dim Tt_StkPost_Type1 As Double = 0
        Dim Tt_StkPost_Type2 As Double = 0
        Dim Tt_StkPost_Type3 As Double = 0
        Dim Tt_StkPost_Type4 As Double = 0
        Dim Tt_StkPost_Type5 As Double = 0
        Dim Tt_Diff As Double = 0
        Dim i As Integer

        Tt_UcPcsBale_Uc = 0
        Tt_UcPcsBale_Type1 = 0
        Tt_UcPcsBale_Type2 = 0
        Tt_UcPcsBale_Type3 = 0
        Tt_UcPcsBale_Type4 = 0
        Tt_UcPcsBale_Type5 = 0
        Tt_StkPost_Uc = 0
        Tt_StkPost_Type1 = 0
        Tt_StkPost_Type2 = 0
        Tt_StkPost_Type3 = 0
        Tt_StkPost_Type4 = 0
        Tt_StkPost_Type5 = 0
        Tt_Diff = 0

        With dgv_Stock
            For i = 0 To dgv_Stock.Rows.Count - 1
                If .Rows(i).Cells(1).Value <> "" Then

                    Tt_UcPcsBale_Uc = Tt_UcPcsBale_Uc + Val(dgv_Stock.Rows(i).Cells(2).Value)
                    Tt_UcPcsBale_Type1 = Tt_UcPcsBale_Type1 + Val(dgv_Stock.Rows(i).Cells(3).Value)
                    Tt_UcPcsBale_Type2 = Tt_UcPcsBale_Type2 + Val(dgv_Stock.Rows(i).Cells(4).Value)
                    Tt_UcPcsBale_Type3 = Tt_UcPcsBale_Type3 + Val(dgv_Stock.Rows(i).Cells(5).Value)
                    Tt_UcPcsBale_Type4 = Tt_UcPcsBale_Type4 + Val(dgv_Stock.Rows(i).Cells(6).Value)
                    Tt_UcPcsBale_Type5 = Tt_UcPcsBale_Type5 + Val(dgv_Stock.Rows(i).Cells(7).Value)
                    Tt_StkPost_Uc = Tt_StkPost_Uc + Val(dgv_Stock.Rows(i).Cells(9).Value)
                    Tt_StkPost_Type1 = Tt_StkPost_Type1 + Val(dgv_Stock.Rows(i).Cells(10).Value)
                    Tt_StkPost_Type2 = Tt_StkPost_Type2 + Val(dgv_Stock.Rows(i).Cells(11).Value)
                    Tt_StkPost_Type3 = Tt_StkPost_Type3 + Val(dgv_Stock.Rows(i).Cells(12).Value)
                    Tt_StkPost_Type4 = Tt_StkPost_Type4 + Val(dgv_Stock.Rows(i).Cells(13).Value)
                    Tt_StkPost_Type5 = Tt_StkPost_Type5 + Val(dgv_Stock.Rows(i).Cells(14).Value)
                    Tt_Diff = Tt_Diff + Val(dgv_Stock.Rows(i).Cells(16).Value)

                End If
            Next
        End With

        With dgv_StockTotal
            If .Rows.Count <= 0 Then .Rows.Add()
            .Rows(0).Cells(2).Value = Format(Val(Tt_UcPcsBale_Uc), "#########0.00")
            .Rows(0).Cells(3).Value = Format(Val(Tt_UcPcsBale_Type1), "#########0.00")
            .Rows(0).Cells(4).Value = Format(Val(Tt_UcPcsBale_Type2), "#########0.00")
            .Rows(0).Cells(5).Value = Format(Val(Tt_UcPcsBale_Type3), "#########0.00")
            .Rows(0).Cells(6).Value = Format(Val(Tt_UcPcsBale_Type4), "#########0.00")
            .Rows(0).Cells(7).Value = Format(Val(Tt_UcPcsBale_Type5), "#########0.00")
            .Rows(0).Cells(8).Value = Format(Val(Tt_UcPcsBale_Uc) + Val(Tt_UcPcsBale_Type1) + Val(Tt_UcPcsBale_Type2) + Val(Tt_UcPcsBale_Type3) + Val(Tt_UcPcsBale_Type4) + Val(Tt_UcPcsBale_Type5), "#########0.00")

            .Rows(0).Cells(9).Value = Format(Val(Tt_StkPost_Uc), "#########0.00")
            .Rows(0).Cells(10).Value = Format(Val(Tt_StkPost_Type1), "#########0.00")
            .Rows(0).Cells(11).Value = Format(Val(Tt_StkPost_Type2), "#########0.00")
            .Rows(0).Cells(12).Value = Format(Val(Tt_StkPost_Type3), "#########0.00")
            .Rows(0).Cells(13).Value = Format(Val(Tt_StkPost_Type4), "#########0.00")
            .Rows(0).Cells(14).Value = Format(Val(Tt_StkPost_Type5), "#########0.00")
            .Rows(0).Cells(15).Value = Format(Val(Tt_StkPost_Uc) + Val(Tt_StkPost_Type1) + Val(Tt_StkPost_Type2) + Val(Tt_StkPost_Type3) + Val(Tt_StkPost_Type4) + Val(Tt_StkPost_Type5), "#########0.00")
            .Rows(0).Cells(16).Value = Format(Val(Tt_Diff), "#########0.00")

        End With

    End Sub


    Private Sub ClothPosting_Verification()
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Cmd As New SqlClient.SqlCommand
        Dim I As Integer = 0
        Dim n As Integer = 0
        Dim SNo As Integer = 0
        Dim OpDate As Date
        Dim OpYrCode As String
        Dim DbName As String = ""
        Dim Nr As Long = 0
        Dim SQL1 As String = ""


        OpYrCode = Microsoft.VisualBasic.Left(Trim(Common_Procedures.FnRange), 4)
        OpYrCode = Trim(Mid(Val(OpYrCode) - 1, 3, 2)) & "-" & Trim(Microsoft.VisualBasic.Right(OpYrCode, 2))

        OpDate = New DateTime(Microsoft.VisualBasic.Left(Trim(Common_Procedures.FnRange), 4), 4, 1)
        OpDate = DateAdd(DateInterval.Day, -1, OpDate)

        Cmd.Connection = con


        Cmd.CommandText = "truncate table " & Trim(Common_Procedures.ReportTempSubTable) & ""
        Cmd.ExecuteNonQuery()

        Cmd.CommandText = "truncate table " & Trim(Common_Procedures.ReportTempTable) & ""
        Cmd.ExecuteNonQuery()

        '---Opening Piece from PackingSlipHead
        Cmd.CommandText = "insert into " & Trim(Common_Procedures.ReportTempTable) & " (Currency1, Name1, Meters1, Meters2) select a.for_OrderBy, 'OPPCS-' + a.Piece_Opening_Code, sum(a.Total_Checking_Meters), 0 from Piece_Opening_Head a group by a.for_OrderBy, a.Piece_Opening_Code"
        Cmd.ExecuteNonQuery()
        '---From Cloth Processing
        'Cmd.CommandText = "insert into " & Trim(Common_Procedures.ReportTempTable) & "(Currency1, Name1, Meters1, Meters2) select a.for_OrderBy, a.Reference_Code, 0, sum(a.Meters_Type1+a.Meters_Type2+a.Meters_Type3+a.Meters_Type4+a.Meters_Type5) from Stock_Cloth_Processing_Details a where a.Reference_Code LIKE 'OPENI-%/" & Trim(OpYrCode) & "' group by a.for_OrderBy, a.Reference_Code"
        'Cmd.ExecuteNonQuery()


        '---Opening Bales from PackingSlipHead
        Cmd.CommandText = "insert into " & Trim(Common_Procedures.ReportTempTable) & " (Currency1, Name1, Meters1, Meters2) select a.for_OrderBy, 'OBALE-' + a.Packing_Slip_Code, sum(a.Total_Meters), 0 from Packing_Slip_Head a where a.Packing_Slip_Code LIKE '%/" & Trim(OpYrCode) & "' group by a.for_OrderBy, a.Packing_Slip_Code"
        Cmd.ExecuteNonQuery()
        '---From Cloth Processing
        'Cmd.CommandText = "insert into " & Trim(Common_Procedures.ReportTempTable) & "(Currency1, Name1, Meters1, Meters2) select a.for_OrderBy, a.Reference_Code, 0, sum(a.Meters_Type1+a.Meters_Type2+a.Meters_Type3+a.Meters_Type4+a.Meters_Type5) from Stock_Cloth_Processing_Details a where a.Reference_Code LIKE 'OPENI-%/" & Trim(OpYrCode) & "' group by a.for_OrderBy, a.Reference_Code"
        'Cmd.ExecuteNonQuery()

        '---Weaver Cloth receipt from Weaver_Cloth_Receipt_Head
        Cmd.CommandText = "insert into " & Trim(Common_Procedures.ReportTempTable) & " (Currency1, Name1, Meters1, Meters2) select a.for_OrderBy, 'WCLRC-' + a.Weaver_ClothReceipt_Code, sum(CASE WHEN (a.Type1_Checking_Meters+a.Type2_Checking_Meters+a.Type3_Checking_Meters+a.Type4_Checking_Meters+a.Type5_Checking_Meters) <> 0 THEN (a.Type1_Checking_Meters+a.Type2_Checking_Meters+a.Type3_Checking_Meters+a.Type4_Checking_Meters+a.Type5_Checking_Meters) ELSE a.Receipt_Meters END), 0 from Weaver_Cloth_Receipt_Head a where a.Receipt_Type = 'W' and a.Weaver_ClothReceipt_Code not like '%/" & Trim(OpYrCode) & "' group by a.for_OrderBy, a.Weaver_ClothReceipt_Code"
        Cmd.ExecuteNonQuery()
        '---From Cloth Processing
        'Cmd.CommandText = "insert into " & Trim(Common_Procedures.ReportTempTable) & "(Currency1, Name1, Meters1, Meters2) select a.for_OrderBy, a.Reference_Code, 0, sum(a.Meters_Type1+a.Meters_Type2+a.Meters_Type3+a.Meters_Type4+a.Meters_Type5) from Stock_Cloth_Processing_Details a where a.Reference_Code LIKE 'WCLRC-%' and a.Reference_Code not like '%/" & Trim(OpYrCode) & "' group by a.for_OrderBy, a.Reference_Code"
        'Cmd.ExecuteNonQuery()

        '---Doffing from Weaver_Cloth_Receipt_Head
        Cmd.CommandText = "insert into " & Trim(Common_Procedures.ReportTempTable) & " (Currency1, Name1, Meters1, Meters2) select a.for_OrderBy, 'PCDOF-' + a.Weaver_ClothReceipt_Code, sum(CASE WHEN (a.Type1_Checking_Meters+a.Type2_Checking_Meters+a.Type3_Checking_Meters+a.Type4_Checking_Meters+a.Type5_Checking_Meters) <> 0 THEN (a.Type1_Checking_Meters+a.Type2_Checking_Meters+a.Type3_Checking_Meters+a.Type4_Checking_Meters+a.Type5_Checking_Meters) ELSE a.Receipt_Meters END), 0 from Weaver_Cloth_Receipt_Head a where a.Receipt_Type = 'L' and a.Weaver_ClothReceipt_Code not like '%/" & Trim(OpYrCode) & "' group by a.for_OrderBy, a.Weaver_ClothReceipt_Code"
        Cmd.ExecuteNonQuery()
        '---From Cloth Processing
        'Cmd.CommandText = "insert into " & Trim(Common_Procedures.ReportTempTable) & "(Currency1, Name1, Meters1, Meters2) select a.for_OrderBy, a.Reference_Code, 0, sum(a.Meters_Type1+a.Meters_Type2+a.Meters_Type3+a.Meters_Type4+a.Meters_Type5) from Stock_Cloth_Processing_Details a where a.Reference_Code LIKE 'PCDOF-%' and a.Reference_Code not like '%/" & Trim(OpYrCode) & "' group by a.for_OrderBy, a.Reference_Code"
        'Cmd.ExecuteNonQuery()

        '---Sales Delivery from ClothSales_Delivery_Head
        Cmd.CommandText = "insert into " & Trim(Common_Procedures.ReportTempTable) & " (Currency1, Name1, Meters1, Meters2) select a.for_OrderBy, 'CSDLV-' + a.ClothSales_Delivery_Code, sum(a.Total_Meters), 0 from ClothSales_Delivery_Head a where a.ClothSales_Delivery_Code not like '%/" & Trim(OpYrCode) & "' group by a.for_OrderBy, a.ClothSales_Delivery_Code"
        Cmd.ExecuteNonQuery()
        '---From Cloth Processing
        'Cmd.CommandText = "insert into " & Trim(Common_Procedures.ReportTempTable) & "(Currency1, Name1, Meters1, Meters2) select a.for_OrderBy, a.Reference_Code, 0, sum(a.Meters_Type1+a.Meters_Type2+a.Meters_Type3+a.Meters_Type4+a.Meters_Type5) from Stock_Cloth_Processing_Details a where a.Reference_Code LIKE 'CSDLV-%' and a.Reference_Code not like '%/" & Trim(OpYrCode) & "' group by a.for_OrderBy, a.Reference_Code"
        'Cmd.ExecuteNonQuery()


        '---Sales Invoice from ClothSales_Invoice_Head
        Cmd.CommandText = "insert into " & Trim(Common_Procedures.ReportTempTable) & " (Currency1, Name1, Meters1, Meters2) select a.for_OrderBy, (CASE WHEN a.ClothSales_Invoice_Code LIKE 'GCINV-%' OR a.ClothSales_Invoice_Code LIKE 'GSSINS-%' THEN '' ELSE 'CSINV-' END) + a.ClothSales_Invoice_Code, sum(a.Total_Meters), 0 from ClothSales_Invoice_Head a where a.Invoice_Selection_Type <> 'DELIVERY' and a.ClothSales_Invoice_Code NOT LIKE '%/" & Trim(OpYrCode) & "' and a.No_Stock_Posting_Status = 0 group by a.for_OrderBy, a.ClothSales_Invoice_Code"
        Cmd.ExecuteNonQuery()
        '---From Cloth Processing
        'Cmd.CommandText = "insert into " & Trim(Common_Procedures.ReportTempTable) & "(Currency1, Name1, Meters1, Meters2) select a.for_OrderBy, a.Reference_Code, 0, sum(a.Meters_Type1+a.Meters_Type2+a.Meters_Type3+a.Meters_Type4+a.Meters_Type5) from Stock_Cloth_Processing_Details a where a.Reference_Code LIKE 'CSINV-%' and a.Reference_Code not like '%/" & Trim(OpYrCode) & "' group by a.for_OrderBy, a.Reference_Code"
        'Cmd.ExecuteNonQuery()

        '---Sales Delivery Return from ClothSales_Delivery_Return_Head
        Cmd.CommandText = "insert into " & Trim(Common_Procedures.ReportTempTable) & " (Currency1, Name1, Meters1, Meters2) select a.for_OrderBy, 'CLDRT-' + a.ClothSales_Delivery_Return_Code, sum(case when a.Total_Checking_Meters <> 0 then a.Total_Checking_Meters else a.Return_Meters end), 0 from ClothSales_Delivery_Return_Head a group by a.for_OrderBy, a.ClothSales_Delivery_Return_Code"
        Nr = Cmd.ExecuteNonQuery()

        '---Sales Invoice Return from ClothSales_Return_Head
        Cmd.CommandText = "insert into " & Trim(Common_Procedures.ReportTempTable) & " (Currency1, Name1, Meters1, Meters2) select a.for_OrderBy, (CASE WHEN a.ClothSales_Return_Code LIKE 'GCLSR-%' THEN '' ELSE 'CLSRT-' END) + a.ClothSales_Return_Code, sum(case when a.Total_Checking_Meters <> 0 then a.Total_Checking_Meters else a.Return_Meters end), 0 from ClothSales_Return_Head a group by a.for_OrderBy, a.ClothSales_Return_Code"
        nr = Cmd.ExecuteNonQuery()


        '---Piece Transfer from Piece_Transfer_Head
        Cmd.CommandText = "insert into " & Trim(Common_Procedures.ReportTempTable) & " (Currency1, Name1, Meters1, Meters2) select a.for_OrderBy, 'PCSTR-' + a.Piece_Transfer_Code, sum(a.Total_Meters+a.Total_New_Meters), 0 from Piece_Transfer_Head a group by a.for_OrderBy, a.Piece_Transfer_Code"
        Cmd.ExecuteNonQuery()


        '---Jobwork Pcs Delivery from JobWork_Piece_Delivery_Head
        Cmd.CommandText = "insert into " & Trim(Common_Procedures.ReportTempTable) & " (Currency1, Name1, Meters1, Meters2) select a.for_OrderBy, 'JPCDC-' + a.JobWork_Piece_Delivery_Code, sum(a.Total_Delivery_Meters), 0 from JobWork_Piece_Delivery_Head a where a.JobWork_Piece_Delivery_Code not like '%/" & Trim(OpYrCode) & "' group by a.for_OrderBy, a.JobWork_Piece_Delivery_Code"
        Cmd.ExecuteNonQuery()
        '---From Cloth Processing
        'Cmd.CommandText = "insert into " & Trim(Common_Procedures.ReportTempTable) & "(Currency1, Name1, Meters1, Meters2) select a.for_OrderBy, a.Reference_Code, 0, sum(a.Meters_Type1+a.Meters_Type2+a.Meters_Type3+a.Meters_Type4+a.Meters_Type5) from Stock_Cloth_Processing_Details a where a.Reference_Code LIKE 'JPCDC-%' and a.Reference_Code not like '%/" & Trim(OpYrCode) & "' group by a.for_OrderBy, a.Reference_Code"
        'Cmd.ExecuteNonQuery()

        '---Cloth Purcashe from Cloth_Purchase_Head
        Cmd.CommandText = "insert into " & Trim(Common_Procedures.ReportTempTable) & " (Currency1, Name1, Meters1, Meters2) select a.for_OrderBy, 'CPREC-' + a.Cloth_Purchase_Receipt_Code, sum(case when a.Total_Checking_Meters <> 0 then a.Total_Checking_Meters else a.ReceiptMeters_Receipt end), 0 from Cloth_Purchase_Receipt_Head a  group by a.for_OrderBy, a.Cloth_Purchase_Receipt_Code"
        Cmd.ExecuteNonQuery()
        '---Cloth Purcashe from Cloth_Purchase_Head
        Cmd.CommandText = "insert into " & Trim(Common_Procedures.ReportTempTable) & " (Currency1, Name1, Meters1, Meters2) select a.for_OrderBy, (CASE WHEN a.Cloth_Purchase_Code LIKE 'GCLPR-%' THEN '' ELSE 'CLPUR-' END)  + a.Cloth_Purchase_Code, sum(a.Total_Meters), 0 from Cloth_Purchase_Head a Where a.Purchase_Selection_Type <> 'RECEIPT' group by a.for_OrderBy, a.Cloth_Purchase_Code"
        Cmd.ExecuteNonQuery()


        '---Bale Transfer from Bale_Transfer_Head
        Cmd.CommandText = "insert into " & Trim(Common_Procedures.ReportTempTable) & " (Currency1, Name1, Meters1, Meters2) select a.for_OrderBy, 'BLTRF-' + a.Bale_Transfer_Code , sum(a.Total_Meters), 0 from Bale_Transfer_Head a group by a.for_OrderBy, a.Bale_Transfer_Code"
        Cmd.ExecuteNonQuery()

        '---Jobwork Pcs delivery as Transfer from JobWork_Piece_Delivery_Head
        Da1 = New SqlClient.SqlDataAdapter("Select Transfer_From_CompanyGroupIdNo, count(*) from Stock_Cloth_Processing_Details where Transfer_From_EntryCode <> '' and Transfer_From_CompanyGroupIdNo <> 0 and Transfer_From_CompanyGroupIdNo <> " & Str(Val(Common_Procedures.CompGroupIdNo)) & " group by Transfer_From_CompanyGroupIdNo having count(*) <> 0", con)
        Dt1 = New DataTable
        Da1.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then

            For I = 0 To Dt1.Rows.Count - 1

                DbName = Common_Procedures.get_Company_DataBaseName(Trim(Val(Dt1.Rows(I).Item("Transfer_From_CompanyGroupIdNo").ToString)))

                Cmd.CommandText = "insert into " & Trim(Common_Procedures.ReportTempTable) & " (Currency1, Name1, Meters1, Meters2) select a.for_OrderBy, 'JPCTR-' + a.JobWork_Piece_Delivery_Code, sum(a.Total_Delivery_Meters), 0 from  " & Trim(DbName) & "..JobWork_Piece_Delivery_Head a where a.JobWork_Piece_Delivery_Code not like '%/" & Trim(OpYrCode) & "' group by a.for_OrderBy, a.JobWork_Piece_Delivery_Code"
                Cmd.ExecuteNonQuery()

            Next

        End If



        '---ALL Entries From Cloth Processing Details
        Cmd.CommandText = "insert into " & Trim(Common_Procedures.ReportTempTable) & "(Currency1, Name1, Meters1, Meters2) select a.for_OrderBy, a.Reference_Code, 0, sum(a.UnChecked_Meters+a.Meters_Type1+a.Meters_Type2+a.Meters_Type3+a.Meters_Type4+a.Meters_Type5) from Stock_Cloth_Processing_Details a group by a.for_OrderBy, a.Reference_Code"
        Cmd.ExecuteNonQuery()



        ''---Piece Details of Transfer from concern to concern From Weaver_ClothReceipt_Piece_Details
        SQL1 = "insert into " & Trim(Common_Procedures.ReportTempTable) & "(Currency1, Name1, Meters1, Meters2) select 0, replace(a.Transfer_From_EntryCode, 'JPCDC-', 'JPTRA-'), sum(a.Type1_Meters+a.Type2_Meters+a.Type3_Meters+a.Type4_Meters+a.Type5_Meters), 0 from Weaver_ClothReceipt_Piece_Details a WHERE a.Transfer_From_EntryCode <> '' and a.Transfer_From_EntryCode LIKE 'JPCDC-%' group by a.for_OrderBy, a.Transfer_From_EntryCode"
        Cmd.CommandText = "EXEC SP_ExecuteQuery '" & Replace(Trim(SQL1), "'", "''") & "'"
        Cmd.ExecuteNonQuery()
        'Cmd.CommandText = "insert into " & Trim(Common_Procedures.ReportTempTable) & "(Currency1, Name1, Meters1, Meters2) select 0, replace(a.Transfer_From_EntryCode, 'JPCDC-', 'JPTRA-'), sum(a.Type1_Meters+a.Type2_Meters+a.Type3_Meters+a.Type4_Meters+a.Type5_Meters), 0 from Weaver_ClothReceipt_Piece_Details a WHERE a.Transfer_From_EntryCode <> '' and a.Transfer_From_EntryCode LIKE 'JPCDC-%' group by a.for_OrderBy, a.Transfer_From_EntryCode"
        'Cmd.ExecuteNonQuery()

        '---Piece Transfer from concern to concern From Cloth Processing Details
        Cmd.CommandText = "insert into " & Trim(Common_Procedures.ReportTempTable) & "(Currency1, Name1, Meters1, Meters2) select 0, replace(a.Reference_Code, 'JPCTR-', 'JPTRA-'), 0, sum(a.UnChecked_Meters+a.Meters_Type1+a.Meters_Type2+a.Meters_Type3+a.Meters_Type4+a.Meters_Type5) from Stock_Cloth_Processing_Details a WHERE Reference_Code LIKE 'JPCTR-%' group by a.for_OrderBy, a.Reference_Code"
        Cmd.ExecuteNonQuery()

        Da1 = New SqlClient.SqlDataAdapter("Select Currency1, Name1 as EntryCode, sum(Meters1) as EntryMeters, sum(Meters2) as PostingMeters from " & Trim(Common_Procedures.ReportTempTable) & " a group by Currency1, Name1 having sum(Meters1) <> sum(Meters2) order by left(Name1, 6), Currency1, Name1", con)
        Dt1 = New DataTable
        Da1.Fill(Dt1)

        With dgv_Posting

            .Rows.Clear()

            If Dt1.Rows.Count > 0 Then

                SNo = 0
                For I = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()

                    SNo = SNo + 1

                    .Rows(n).Cells(0).Value = "  " & Val(SNo)

                    .Rows(n).Cells(1).Value = Dt1.Rows(I).Item("EntryCode").ToString

                    .Rows(n).Cells(2).Value = Format(Val(Dt1.Rows(I).Item("EntryMeters").ToString), "#########0.00")
                    If Val(.Rows(n).Cells(2).Value) = 0 Then .Rows(n).Cells(2).Value = ""
                    .Rows(n).Cells(3).Value = Format(Val(Dt1.Rows(I).Item("PostingMeters").ToString), "#########0.00")
                    If Val(.Rows(n).Cells(3).Value) = 0 Then .Rows(n).Cells(3).Value = ""

                Next

            End If

            If .Rows.Count = 0 Then .Rows.Add()

        End With
        Dt1.Clear()

        Cmd.Dispose()
        Dt1.Dispose()
        Da1.Dispose()

    End Sub

    Private Sub PieceChecking_To_PackingSlip_Verification()
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Cmd As New SqlClient.SqlCommand
        Dim I As Integer = 0
        Dim n As Integer = 0
        Dim SNo As Integer = 0
        Dim OpYrCode As String
        Dim DbName As String = ""
        Dim SQL1 As String = ""


        OpYrCode = Microsoft.VisualBasic.Left(Trim(Common_Procedures.FnRange), 4)
        OpYrCode = Trim(Mid(Val(OpYrCode) - 1, 3, 2)) & "-" & Trim(Microsoft.VisualBasic.Right(OpYrCode, 2))

        Cmd.Connection = con

        Cmd.CommandText = "truncate table " & Trim(Common_Procedures.ReportTempSubTable) & ""
        Cmd.ExecuteNonQuery()

        Cmd.CommandText = "truncate table " & Trim(Common_Procedures.ReportTempTable) & ""
        Cmd.ExecuteNonQuery()

        '---Baled Pieces
        SQL1 = "insert into " & Trim(Common_Procedures.ReportTempTable) & " (Int1, name1, name2, name3, Meters1) select a.Cloth_Idno, a.Lot_Code, a.Piece_No, a.PackingSlip_Code_Type1, a.Type1_Meters from Weaver_ClothReceipt_Piece_Details a where a.Type1_Meters <> 0 and a.PackingSlip_Code_Type1 <> ''"
        Cmd.CommandText = "EXEC SP_ExecuteQuery '" & Replace(Trim(SQL1), "'", "''") & "'"
        Cmd.ExecuteNonQuery()
        SQL1 = "insert into " & Trim(Common_Procedures.ReportTempTable) & " (Int1, name1, name2, name3, Meters1) select a.Cloth_Idno, a.Lot_Code, a.Piece_No, a.PackingSlip_Code_Type2, a.Type2_Meters from Weaver_ClothReceipt_Piece_Details a where a.Type2_Meters <> 0 and a.PackingSlip_Code_Type2 <> ''"
        Cmd.CommandText = "EXEC SP_ExecuteQuery '" & Replace(Trim(SQL1), "'", "''") & "'"
        Cmd.ExecuteNonQuery()
        SQL1 = "insert into " & Trim(Common_Procedures.ReportTempTable) & " (Int1, name1, name2, name3, Meters1) select a.Cloth_Idno, a.Lot_Code, a.Piece_No, a.PackingSlip_Code_Type3, a.Type3_Meters from Weaver_ClothReceipt_Piece_Details a where a.Type3_Meters <> 0 and a.PackingSlip_Code_Type3 <> ''"
        Cmd.CommandText = "EXEC SP_ExecuteQuery '" & Replace(Trim(SQL1), "'", "''") & "'"
        Cmd.ExecuteNonQuery()
        SQL1 = "insert into " & Trim(Common_Procedures.ReportTempTable) & " (Int1, name1, name2, name3, Meters1) select a.Cloth_Idno, a.Lot_Code, a.Piece_No, a.PackingSlip_Code_Type4, a.Type4_Meters from Weaver_ClothReceipt_Piece_Details a where a.Type4_Meters <> 0 and a.PackingSlip_Code_Type4 <> ''"
        Cmd.CommandText = "EXEC SP_ExecuteQuery '" & Replace(Trim(SQL1), "'", "''") & "'"
        Cmd.ExecuteNonQuery()
        SQL1 = "insert into " & Trim(Common_Procedures.ReportTempTable) & " (Int1, name1, name2, name3, Meters1) select a.Cloth_Idno, a.Lot_Code, a.Piece_No, a.PackingSlip_Code_Type5, a.Type5_Meters from Weaver_ClothReceipt_Piece_Details a where a.Type5_Meters <> 0 and a.PackingSlip_Code_Type5 <> ''"
        Cmd.CommandText = "EXEC SP_ExecuteQuery '" & Replace(Trim(SQL1), "'", "''") & "'"
        Cmd.ExecuteNonQuery()

        'Cmd.CommandText = "insert into " & Trim(Common_Procedures.ReportTempTable) & " (Int1, name1, name2, name3, Meters1) select a.Cloth_Idno, a.Lot_Code, a.Piece_No, a.PackingSlip_Code_Type1, a.Type1_Meters from Weaver_ClothReceipt_Piece_Details a where a.Type1_Meters <> 0 and a.PackingSlip_Code_Type1 <> ''"
        'Cmd.ExecuteNonQuery()
        'Cmd.CommandText = "insert into " & Trim(Common_Procedures.ReportTempTable) & " (Int1, name1, name2, name3, Meters1) select a.Cloth_Idno, a.Lot_Code, a.Piece_No, a.PackingSlip_Code_Type2, a.Type2_Meters from Weaver_ClothReceipt_Piece_Details a where a.Type2_Meters <> 0 and a.PackingSlip_Code_Type2 <> ''"
        'Cmd.ExecuteNonQuery()
        'Cmd.CommandText = "insert into " & Trim(Common_Procedures.ReportTempTable) & " (Int1, name1, name2, name3, Meters1) select a.Cloth_Idno, a.Lot_Code, a.Piece_No, a.PackingSlip_Code_Type3, a.Type3_Meters from Weaver_ClothReceipt_Piece_Details a where a.Type3_Meters <> 0 and a.PackingSlip_Code_Type3 <> ''"
        'Cmd.ExecuteNonQuery()
        'Cmd.CommandText = "insert into " & Trim(Common_Procedures.ReportTempTable) & " (Int1, name1, name2, name3, Meters1) select a.Cloth_Idno, a.Lot_Code, a.Piece_No, a.PackingSlip_Code_Type4, a.Type4_Meters from Weaver_ClothReceipt_Piece_Details a where a.Type4_Meters <> 0 and a.PackingSlip_Code_Type4 <> ''"
        'Cmd.ExecuteNonQuery()
        'Cmd.CommandText = "insert into " & Trim(Common_Procedures.ReportTempTable) & " (Int1, name1, name2, name3, Meters1) select a.Cloth_Idno, a.Lot_Code, a.Piece_No, a.PackingSlip_Code_Type5, a.Type5_Meters from Weaver_ClothReceipt_Piece_Details a where a.Type5_Meters <> 0 and a.PackingSlip_Code_Type5 <> ''"
        'Cmd.ExecuteNonQuery()


        '---Baled Packing Slip
        Cmd.CommandText = "insert into " & Trim(Common_Procedures.ReportTempTable) & "(Int1, name1, name2, name3, Meters2) select a.cloth_idno, a.Lot_Code, a.Pcs_No, a.Packing_Slip_Code, a.Meters from Packing_Slip_Details a where a.Meters <> 0"
        Cmd.ExecuteNonQuery()
        '---Jobwork Pcs Delivery from JobWork_Piece_Delivery_Details
        Cmd.CommandText = "insert into " & Trim(Common_Procedures.ReportTempTable) & "(Int1, name1, name2, name3, Meters2) select b.cloth_idno, a.Lot_Code, a.Pcs_No, 'JPCDC-'+a.JobWork_Piece_Delivery_Code, a.Meters from JobWork_Piece_Delivery_Details a, JobWork_Piece_Delivery_Head b where a.Meters <> 0 and a.JobWork_Piece_Delivery_Code = b.JobWork_Piece_Delivery_Code"
        Cmd.ExecuteNonQuery()
        '---Piece Transfer from JobWork_Piece_Delivery_Details
        Cmd.CommandText = "insert into " & Trim(Common_Procedures.ReportTempTable) & "(Int1, name1, name2, name3, Meters2) select b.ClothFrom_IdNo, a.Lot_Code, a.Pcs_No, 'PCSTR-'+a.Piece_Transfer_Code, a.Meters from Piece_Transfer_Details a, Piece_Transfer_Head b where a.Meters <> 0 and a.Piece_Transfer_Code = b.Piece_Transfer_Code"
        Cmd.ExecuteNonQuery()
        '---Piece_Excess_Short
        Cmd.CommandText = "insert into " & Trim(Common_Procedures.ReportTempTable) & "(Int1, name1, name2, name3, Meters2) select a.cloth_idno, a.Lot_Code, a.Pcs_No, 'PCSES-' + a.Piece_Excess_Short_Code, a.Meters from Piece_Excess_Short_Details a where a.Meters <> 0"
        Cmd.ExecuteNonQuery()


        Da1 = New SqlClient.SqlDataAdapter("Select b.cloth_name, name1 as LotNo, name2 as PcsNo, name3 as Baleno, sum(Meters1) as PcMeters, sum(Meters2) as BaleMeters from " & Trim(Common_Procedures.ReportTempTable) & " a, cloth_head b where a.int1 = b.cloth_idno group by b.cloth_name, Name1, Name2, Name3 having sum(Meters1) <> sum(Meters2) order by b.cloth_name, Name1, Name2, Name3", con)
        Dt1 = New DataTable
        Da1.Fill(Dt1)

        With dgv_PieceVerification

            .Rows.Clear()

            If Dt1.Rows.Count > 0 Then

                SNo = 0
                For I = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()

                    SNo = SNo + 1

                    .Rows(n).Cells(0).Value = "  " & Val(SNo)

                    .Rows(n).Cells(1).Value = Dt1.Rows(I).Item("cloth_name").ToString
                    .Rows(n).Cells(2).Value = Dt1.Rows(I).Item("LotNo").ToString
                    .Rows(n).Cells(3).Value = Dt1.Rows(I).Item("PcsNo").ToString
                    .Rows(n).Cells(4).Value = Format(Val(Dt1.Rows(I).Item("PcMeters").ToString), "#########0.00")
                    If Val(.Rows(n).Cells(4).Value) = 0 Then .Rows(n).Cells(4).Value = ""
                    .Rows(n).Cells(5).Value = Dt1.Rows(I).Item("Baleno").ToString
                    .Rows(n).Cells(6).Value = Format(Val(Dt1.Rows(I).Item("BaleMeters").ToString), "#########0.00")
                    If Val(.Rows(n).Cells(6).Value) = 0 Then .Rows(n).Cells(6).Value = ""

                Next

            End If

            If .Rows.Count = 0 Then .Rows.Add()

        End With
        Dt1.Clear()

        Cmd.Dispose()
        Dt1.Dispose()
        Da1.Dispose()

    End Sub

    Private Sub PackingSlip_To_Invoice_Verification()
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Cmd As New SqlClient.SqlCommand
        Dim I As Integer = 0
        Dim n As Integer = 0
        Dim SNo As Integer = 0
        Dim OpYrCode As String
        Dim DbName As String = ""

        OpYrCode = Microsoft.VisualBasic.Left(Trim(Common_Procedures.FnRange), 4)
        OpYrCode = Trim(Mid(Val(OpYrCode) - 1, 3, 2)) & "-" & Trim(Microsoft.VisualBasic.Right(OpYrCode, 2))

        Cmd.Connection = con

        Cmd.CommandText = "truncate table " & Trim(Common_Procedures.ReportTempSubTable) & ""
        Cmd.ExecuteNonQuery()

        Cmd.CommandText = "truncate table " & Trim(Common_Procedures.ReportTempTable) & ""
        Cmd.ExecuteNonQuery()

        '---Baled Meters
        Cmd.CommandText = "insert into " & Trim(Common_Procedures.ReportTempTable) & "(Name1, Meters1) select Delivery_Code, Total_Meters from Packing_Slip_Head Where Delivery_Code <> ''"
        Cmd.ExecuteNonQuery()
        '---Delivery meters
        Cmd.CommandText = "insert into " & Trim(Common_Procedures.ReportTempTable) & "(Name1, Meters2) select 'CSDLV-' + ClothSales_Delivery_Code, Meters from ClothSales_Delivery_Details where PackingSlip_Codes <> ''"
        Cmd.ExecuteNonQuery()
        '---Invoiced meters
        Cmd.CommandText = "insert into " & Trim(Common_Procedures.ReportTempTable) & "(Name1, Meters2) select 'CSINV-' + ClothSales_Invoice_Code, Meters from ClothSales_Invoice_Details where PackingSlip_Codes <> ''"
        Cmd.ExecuteNonQuery()
        '---Bale Transfered meters
        Cmd.CommandText = "insert into " & Trim(Common_Procedures.ReportTempTable) & "(Name1, Meters2) select 'BLTRF-' + Bale_Transfer_Code, Meters from Bale_Transfer_Details where PackingSlip_Codes <> ''"
        Cmd.ExecuteNonQuery()


        Da1 = New SqlClient.SqlDataAdapter("Select name1 as Dc_Inv_No, sum(Meters1) as BaleMeters, sum(Meters2) as Dc_Inv_Meters from " & Trim(Common_Procedures.ReportTempTable) & " a group by Name1 having sum(Meters1) <> sum(Meters2) order by Name1", con)
        Dt1 = New DataTable
        Da1.Fill(Dt1)

        With dgv_BaleVerification

            .Rows.Clear()

            If Dt1.Rows.Count > 0 Then

                SNo = 0
                For I = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()

                    SNo = SNo + 1

                    .Rows(n).Cells(0).Value = "  " & Val(SNo)

                    .Rows(n).Cells(1).Value = Dt1.Rows(I).Item("Dc_Inv_No").ToString
                    .Rows(n).Cells(2).Value = Format(Val(Dt1.Rows(I).Item("BaleMeters").ToString), "#########0.00")
                    If Val(.Rows(n).Cells(2).Value) = 0 Then .Rows(n).Cells(2).Value = ""
                    .Rows(n).Cells(3).Value = Format(Val(Dt1.Rows(I).Item("Dc_Inv_Meters").ToString), "#########0.00")
                    If Val(.Rows(n).Cells(3).Value) = 0 Then .Rows(n).Cells(3).Value = ""

                Next

            End If

            If .Rows.Count = 0 Then .Rows.Add()

        End With
        Dt1.Clear()

        Cmd.Dispose()
        Dt1.Dispose()
        Da1.Dispose()

    End Sub

End Class