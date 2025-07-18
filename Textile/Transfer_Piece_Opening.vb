Imports System.IO

Public Class Transfer_Piece_Opening

    Private CnTo As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private CnFrm As New SqlClient.SqlConnection

    Private Sub Transfer_Piece_Opening_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        If cbo_DBFrom.Enabled And cbo_DBFrom.Visible Then cbo_DBFrom.Focus()
    End Sub

    Private Sub Transfer_Piece_Opening_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(cbo_DBFrom, CnTo, "master..sysdatabases", "name", "(name LIKE 'tsoft%textile%' and name NOT LIKE 'tsoft%company%')", "")

        cbo_DBFrom.Text = ""
        Me.Text = "PIECE OPENING"

    End Sub

    Private Sub btn_All_Piece_Transfer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_All_Piece_Transfer.Click
        Dim tr As SqlClient.SqlTransaction

        If Trim(cbo_DBFrom.Text) = "" Then
            MessageBox.Show("Invalid Database Name", "DOES NOT TRANSFER...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_DBFrom.Enabled Then cbo_DBFrom.Focus()
            Exit Sub
        End If

        If MessageBox.Show("All the datas will lost " & Chr(13) & "Are you sure you want to Transfer?", "FOR MASTER TRANSFER...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

        MDIParent1.Cursor = Cursors.WaitCursor
        Me.Cursor = Cursors.WaitCursor

        btn_All_Piece_Transfer.Enabled = False
        Me.Text = ""

        CnFrm = New SqlClient.SqlConnection("Data Source=" & Trim(Common_Procedures.ServerName) & ";Initial Catalog=" & Trim(cbo_DBFrom.Text) & ";User ID=sa;Password=" & Trim(Common_Procedures.ServerPassword) & ";Integrated Security=False")

        CnFrm.Open()
        CnTo.Open()

        tr = CnTo.BeginTransaction

        Try

            PieceOpening_Updation(tr)

            tr.Commit()

            Me.Text = "PIECE OPENING TRANSFERED"

            MDIParent1.Cursor = Cursors.Default
            Me.Cursor = Cursors.Default

            MessageBox.Show("Piece Transfered Transfered Sucessfully", "FOR PIECE OPENING...", MessageBoxButtons.OK, MessageBoxIcon.Information)

            btn_All_Piece_Transfer.Enabled = True

        Catch ex As Exception

            tr.Rollback()
            Me.Text = "PIECE OPENING TRANSFERED"
            MDIParent1.Cursor = Cursors.Default
            Me.Cursor = Cursors.Default
            btn_All_Piece_Transfer.Enabled = True
            MessageBox.Show(ex.Message, "INVALID TRANSFER...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            CnFrm.Close()
            CnTo.Close()
            tr.Dispose()

            btn_All_Piece_Transfer.Enabled = True
            Me.Text = "PIECE OPENING TRANSFERED"

            MDIParent1.Cursor = Cursors.Default
            Me.Cursor = Cursors.Default

        End Try

    End Sub

    Private Sub PieceOpening_Updation(ByVal sqltr As SqlClient.SqlTransaction)
        Dim cmdfrm As New SqlClient.SqlCommand
        Dim cmdto As New SqlClient.SqlCommand
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim opdate As Date
        Dim opbal As Single = 0
        Dim i As Integer = 0
        Dim sno As Integer = 0
        Dim compidno As Integer = 0
        Dim ledidno As Integer = 0
        Dim clth_id As Integer = 0
        Dim newcode As String = ""
        Dim newno As String = ""
        Dim opyrcode As String = ""
        Dim pk_condition As String = ""
        Dim FldPerc As Single = 0
        Dim PcsNo As String = ""
        Dim PcsOrdByNo As Double = 0
        Dim T1_Mtrs As Double = 0
        Dim T2_Mtrs As Double = 0
        Dim T3_Mtrs As Double = 0
        Dim T4_Mtrs As Double = 0
        Dim T5_Mtrs As Double = 0
        Dim Tot_Mtrs As Double = 0
        Dim Wgt As Double = 0
        Dim Wt_Mtr As Double = 0
        Dim tt_pcs As Integer = 0
        Dim tt_typ1_mtrs As Double = 0
        Dim tt_typ2_mtrs As Double = 0
        Dim tt_typ3_mtrs As Double = 0
        Dim tt_typ4_mtrs As Double = 0
        Dim tt_typ5_mtrs As Double = 0
        Dim tt_wgt As Double = 0
        Dim Prevnewcd As String = ""
        Dim DupPcSubNo As Integer = 0


        Me.Text = "piece opening"

        pk_condition = "openi-"

        cmdfrm.Connection = CnFrm

        cmdto.Connection = CnTo
        cmdto.Transaction = sqltr


        opdate = New DateTime(Val(Microsoft.VisualBasic.Right(Common_Procedures.FnRange, 4)) - 1, 4, 1)
        opdate = DateAdd(DateInterval.Day, -1, opdate)

        opyrcode = Microsoft.VisualBasic.Left(Trim(Common_Procedures.FnRange), 4)
        opyrcode = Trim(Mid(Val(opyrcode) - 1, 3, 2)) & "-" & Trim(Microsoft.VisualBasic.Right(opyrcode, 2))


        cmdfrm.Parameters.Clear()
        cmdfrm.Parameters.AddWithValue("@uptodate", dtp_UpDate.Value.Date)

        cmdto.Parameters.Clear()
        cmdto.Parameters.AddWithValue("@opdate", opdate)


        cmdto.CommandText = "delete from piece_opening_head"
        cmdto.ExecuteNonQuery()

        cmdto.CommandText = "delete from weaver_clothreceipt_piece_details where weaver_clothreceipt_code LIKE '%/" & Trim(opyrcode) & "'"
        cmdto.ExecuteNonQuery()

        cmdfrm.CommandText = "truncate table " & Trim(Common_Procedures.ReportTempSubTable) & ""
        cmdfrm.ExecuteNonQuery()

        '-----  piece stock
        cmdfrm.CommandText = "insert into " & Trim(Common_Procedures.ReportTempSubTable) & " (int1, int2, int3, name1, name2,  name3, weight1, name4, weight2, currency1, meters1, meters2, meters3, meters4, meters5, weight6, weight7) select a.company_idno, a.cloth_idno, a.stockoff_idno, a.weaver_clothreceipt_code, a.lot_code, a.lot_no, a.for_orderby, a.piece_no, a.pieceno_orderby, a.folding, a.type1_meters, 0, 0, 0, 0, a.Weight, a.Weight_Meter from weaver_clothreceipt_piece_details a inner join company_head tz on a.company_idno <> 0 and a.company_idno = tz.company_idno inner join ledger_head tsp on a.stockoff_idno <> 0 and a.stockoff_idno = tsp.ledger_idno where a.packingslip_code_type1 = '' and a.type1_meters <> 0 and weaver_clothreceipt_date <= @uptodate"
        cmdfrm.ExecuteNonQuery()
        cmdfrm.CommandText = "insert into " & Trim(Common_Procedures.ReportTempSubTable) & " (int1, int2, int3, name1, name2,  name3, weight1, name4, weight2, currency1, meters1, meters2, meters3, meters4, meters5, weight6, weight7) select a.company_idno, a.cloth_idno, a.stockoff_idno, a.weaver_clothreceipt_code, a.lot_code, a.lot_no, a.for_orderby, a.piece_no, a.pieceno_orderby, a.folding, 0, a.type2_meters, 0, 0, 0, a.Weight, a.Weight_Meter from weaver_clothreceipt_piece_details a inner join company_head tz on a.company_idno <> 0 and a.company_idno = tz.company_idno inner join ledger_head tsp on a.stockoff_idno <> 0 and a.stockoff_idno = tsp.ledger_idno where a.packingslip_code_type2 = '' and a.type2_meters <> 0 and weaver_clothreceipt_date <= @uptodate"
        cmdfrm.ExecuteNonQuery()
        cmdfrm.CommandText = "insert into " & Trim(Common_Procedures.ReportTempSubTable) & " (int1, int2, int3, name1, name2,  name3, weight1, name4, weight2, currency1, meters1, meters2, meters3, meters4, meters5, weight6, weight7) select a.company_idno, a.cloth_idno, a.stockoff_idno, a.weaver_clothreceipt_code, a.lot_code, a.lot_no, a.for_orderby, a.piece_no, a.pieceno_orderby, a.folding, 0, 0, a.type3_meters, 0, 0, a.Weight, a.Weight_Meter from weaver_clothreceipt_piece_details a inner join company_head tz on a.company_idno <> 0 and a.company_idno = tz.company_idno inner join ledger_head tsp on a.stockoff_idno <> 0 and a.stockoff_idno = tsp.ledger_idno where a.packingslip_code_type3 = '' and a.type3_meters <> 0 and weaver_clothreceipt_date <= @uptodate"
        cmdfrm.ExecuteNonQuery()
        cmdfrm.CommandText = "insert into " & Trim(Common_Procedures.ReportTempSubTable) & " (int1, int2, int3, name1, name2,  name3, weight1, name4, weight2, currency1, meters1, meters2, meters3, meters4, meters5, weight6, weight7) select a.company_idno, a.cloth_idno, a.stockoff_idno, a.weaver_clothreceipt_code, a.lot_code, a.lot_no, a.for_orderby, a.piece_no, a.pieceno_orderby, a.folding, 0, 0, 0, a.type4_meters, 0, a.Weight, a.Weight_Meter from weaver_clothreceipt_piece_details a inner join company_head tz on a.company_idno <> 0 and a.company_idno = tz.company_idno inner join ledger_head tsp on a.stockoff_idno <> 0 and a.stockoff_idno = tsp.ledger_idno where a.packingslip_code_type4 = '' and a.type4_meters <> 0 and weaver_clothreceipt_date <= @uptodate"
        cmdfrm.ExecuteNonQuery()
        cmdfrm.CommandText = "insert into " & Trim(Common_Procedures.ReportTempSubTable) & " (int1, int2, int3, name1, name2,  name3, weight1, name4, weight2, currency1, meters1, meters2, meters3, meters4, meters5, weight6, weight7) select a.company_idno, a.cloth_idno, a.stockoff_idno, a.weaver_clothreceipt_code, a.lot_code, a.lot_no, a.for_orderby, a.piece_no, a.pieceno_orderby, a.folding, 0, 0, 0, 0, a.type5_meters, a.Weight, a.Weight_Meter from weaver_clothreceipt_piece_details a inner join company_head tz on a.company_idno <> 0 and a.company_idno = tz.company_idno inner join ledger_head tsp on a.stockoff_idno <> 0 and a.stockoff_idno = tsp.ledger_idno where a.packingslip_code_type5 = '' and a.type5_meters <> 0 and weaver_clothreceipt_date <= @uptodate"
        cmdfrm.ExecuteNonQuery()


        '-----  baled piece  after specified date
        cmdfrm.CommandText = "insert into " & Trim(Common_Procedures.ReportTempSubTable) & " (int1, int2, int3, name1, name2,  name3, weight1, name4, weight2, currency1, meters1, meters2, meters3, meters4, meters5, weight6, weight7) select a.company_idno, a.cloth_idno, a.stockoff_idno, a.weaver_clothreceipt_code, a.lot_code, a.lot_no, a.for_orderby, a.piece_no, a.pieceno_orderby, a.folding, a.type1_meters, 0, 0, 0, 0, a.Weight, a.Weight_Meter from weaver_clothreceipt_piece_details a inner join company_head tz on a.company_idno <> 0 and a.company_idno = tz.company_idno inner join ledger_head tsp on a.stockoff_idno <> 0 and a.stockoff_idno = tsp.ledger_idno inner join packing_slip_head b ON a.packingslip_code_type1 = b.delivery_code where a.packingslip_code_type1 <> '' and a.type1_meters <> 0 and a.weaver_clothreceipt_date <= @uptodate and b.delivery_date > @uptodate"
        cmdfrm.ExecuteNonQuery()
        cmdfrm.CommandText = "insert into " & Trim(Common_Procedures.ReportTempSubTable) & " (int1, int2, int3, name1, name2,  name3, weight1, name4, weight2, currency1, meters1, meters2, meters3, meters4, meters5, weight6, weight7) select a.company_idno, a.cloth_idno, a.stockoff_idno, a.weaver_clothreceipt_code, a.lot_code, a.lot_no, a.for_orderby, a.piece_no, a.pieceno_orderby, a.folding, 0, a.type2_meters, 0, 0, 0, a.Weight, a.Weight_Meter from weaver_clothreceipt_piece_details a inner join company_head tz on a.company_idno <> 0 and a.company_idno = tz.company_idno inner join ledger_head tsp on a.stockoff_idno <> 0 and a.stockoff_idno = tsp.ledger_idno inner join packing_slip_head b ON a.packingslip_code_type2 = b.delivery_code where a.packingslip_code_type2 <> '' and a.type2_meters <> 0 and a.weaver_clothreceipt_date <= @uptodate and b.delivery_date > @uptodate "
        cmdfrm.ExecuteNonQuery()
        cmdfrm.CommandText = "insert into " & Trim(Common_Procedures.ReportTempSubTable) & " (int1, int2, int3, name1, name2,  name3, weight1, name4, weight2, currency1, meters1, meters2, meters3, meters4, meters5, weight6, weight7) select a.company_idno, a.cloth_idno, a.stockoff_idno, a.weaver_clothreceipt_code, a.lot_code, a.lot_no, a.for_orderby, a.piece_no, a.pieceno_orderby, a.folding, 0, 0, a.type3_meters, 0, 0, a.Weight, a.Weight_Meter from weaver_clothreceipt_piece_details a inner join company_head tz on a.company_idno <> 0 and a.company_idno = tz.company_idno inner join ledger_head tsp on a.stockoff_idno <> 0 and a.stockoff_idno = tsp.ledger_idno inner join packing_slip_head b ON a.packingslip_code_type3 = b.delivery_code where a.packingslip_code_type3 <> '' and a.type3_meters <> 0 and a.weaver_clothreceipt_date <= @uptodate and b.delivery_date > @uptodate "
        cmdfrm.ExecuteNonQuery()
        cmdfrm.CommandText = "insert into " & Trim(Common_Procedures.ReportTempSubTable) & " (int1, int2, int3, name1, name2,  name3, weight1, name4, weight2, currency1, meters1, meters2, meters3, meters4, meters5, weight6, weight7) select a.company_idno, a.cloth_idno, a.stockoff_idno, a.weaver_clothreceipt_code, a.lot_code, a.lot_no, a.for_orderby, a.piece_no, a.pieceno_orderby, a.folding, 0, 0, 0, a.type4_meters, 0, a.Weight, a.Weight_Meter from weaver_clothreceipt_piece_details a inner join company_head tz on a.company_idno <> 0 and a.company_idno = tz.company_idno inner join ledger_head tsp on a.stockoff_idno <> 0 and a.stockoff_idno = tsp.ledger_idno inner join packing_slip_head b ON a.packingslip_code_type4 = b.delivery_code where a.packingslip_code_type4 <> '' and a.type4_meters <> 0 and a.weaver_clothreceipt_date <= @uptodate and b.delivery_date > @uptodate "
        cmdfrm.ExecuteNonQuery()
        cmdfrm.CommandText = "insert into " & Trim(Common_Procedures.ReportTempSubTable) & " (int1, int2, int3, name1, name2,  name3, weight1, name4, weight2, currency1, meters1, meters2, meters3, meters4, meters5, weight6, weight7) select a.company_idno, a.cloth_idno, a.stockoff_idno, a.weaver_clothreceipt_code, a.lot_code, a.lot_no, a.for_orderby, a.piece_no, a.pieceno_orderby, a.folding, 0, 0, 0, 0, a.type5_meters, a.Weight, a.Weight_Meter from weaver_clothreceipt_piece_details a inner join company_head tz on a.company_idno <> 0 and a.company_idno = tz.company_idno inner join ledger_head tsp on a.stockoff_idno <> 0 and a.stockoff_idno = tsp.ledger_idno inner join packing_slip_head b ON a.packingslip_code_type5 = b.delivery_code where a.packingslip_code_type5 <> '' and a.type5_meters <> 0 and a.weaver_clothreceipt_date <= @uptodate and b.delivery_date > @uptodate "
        cmdfrm.ExecuteNonQuery()

        '-----  jobwork piece delivered after specified date
        cmdfrm.CommandText = "insert into " & Trim(Common_Procedures.ReportTempSubTable) & " (int1, int2, int3, name1, name2,  name3, weight1, name4, weight2, currency1, meters1, meters2, meters3, meters4, meters5, weight6, weight7) select a.company_idno, a.cloth_idno, a.stockoff_idno, a.weaver_clothreceipt_code, a.lot_code, a.lot_no, a.for_orderby, a.piece_no, a.pieceno_orderby, a.folding, a.type1_meters, 0, 0, 0, 0, a.Weight, a.Weight_Meter from weaver_clothreceipt_piece_details a inner join company_head tz on a.company_idno <> 0 and a.company_idno = tz.company_idno inner join ledger_head tsp on a.stockoff_idno <> 0 and a.stockoff_idno = tsp.ledger_idno inner join JobWork_Piece_Delivery_Head b ON b.JobWork_Piece_Delivery_Date > @uptodate and a.packingslip_code_type1 = 'JPCDC-' + b.JobWork_Piece_Delivery_Code where a.packingslip_code_type1 <> '' and a.type1_meters <> 0 and a.weaver_clothreceipt_date <= @uptodate and b.JobWork_Piece_Delivery_Date > @uptodate"
        cmdfrm.ExecuteNonQuery()
        cmdfrm.CommandText = "insert into " & Trim(Common_Procedures.ReportTempSubTable) & " (int1, int2, int3, name1, name2,  name3, weight1, name4, weight2, currency1, meters1, meters2, meters3, meters4, meters5, weight6, weight7) select a.company_idno, a.cloth_idno, a.stockoff_idno, a.weaver_clothreceipt_code, a.lot_code, a.lot_no, a.for_orderby, a.piece_no, a.pieceno_orderby, a.folding, 0, a.type2_meters, 0, 0, 0, a.Weight, a.Weight_Meter from weaver_clothreceipt_piece_details a inner join company_head tz on a.company_idno <> 0 and a.company_idno = tz.company_idno inner join ledger_head tsp on a.stockoff_idno <> 0 and a.stockoff_idno = tsp.ledger_idno inner join JobWork_Piece_Delivery_Head b ON b.JobWork_Piece_Delivery_Date > @uptodate and a.packingslip_code_type2 = 'JPCDC-' + b.JobWork_Piece_Delivery_Code where a.packingslip_code_type2 <> '' and a.type2_meters <> 0 and a.weaver_clothreceipt_date <= @uptodate and b.JobWork_Piece_Delivery_Date > @uptodate "
        cmdfrm.ExecuteNonQuery()
        cmdfrm.CommandText = "insert into " & Trim(Common_Procedures.ReportTempSubTable) & " (int1, int2, int3, name1, name2,  name3, weight1, name4, weight2, currency1, meters1, meters2, meters3, meters4, meters5, weight6, weight7) select a.company_idno, a.cloth_idno, a.stockoff_idno, a.weaver_clothreceipt_code, a.lot_code, a.lot_no, a.for_orderby, a.piece_no, a.pieceno_orderby, a.folding, 0, 0, a.type3_meters, 0, 0, a.Weight, a.Weight_Meter from weaver_clothreceipt_piece_details a inner join company_head tz on a.company_idno <> 0 and a.company_idno = tz.company_idno inner join ledger_head tsp on a.stockoff_idno <> 0 and a.stockoff_idno = tsp.ledger_idno inner join JobWork_Piece_Delivery_Head b ON b.JobWork_Piece_Delivery_Date > @uptodate and a.packingslip_code_type3 = 'JPCDC-' + b.JobWork_Piece_Delivery_Code where a.packingslip_code_type3 <> '' and a.type3_meters <> 0 and a.weaver_clothreceipt_date <= @uptodate and b.JobWork_Piece_Delivery_Date > @uptodate "
        cmdfrm.ExecuteNonQuery()
        cmdfrm.CommandText = "insert into " & Trim(Common_Procedures.ReportTempSubTable) & " (int1, int2, int3, name1, name2,  name3, weight1, name4, weight2, currency1, meters1, meters2, meters3, meters4, meters5, weight6, weight7) select a.company_idno, a.cloth_idno, a.stockoff_idno, a.weaver_clothreceipt_code, a.lot_code, a.lot_no, a.for_orderby, a.piece_no, a.pieceno_orderby, a.folding, 0, 0, 0, a.type4_meters, 0, a.Weight, a.Weight_Meter from weaver_clothreceipt_piece_details a inner join company_head tz on a.company_idno <> 0 and a.company_idno = tz.company_idno inner join ledger_head tsp on a.stockoff_idno <> 0 and a.stockoff_idno = tsp.ledger_idno inner join JobWork_Piece_Delivery_Head b ON b.JobWork_Piece_Delivery_Date > @uptodate and a.packingslip_code_type4 = 'JPCDC-' + b.JobWork_Piece_Delivery_Code where a.packingslip_code_type4 <> '' and a.type4_meters <> 0 and a.weaver_clothreceipt_date <= @uptodate and b.JobWork_Piece_Delivery_Date > @uptodate "
        cmdfrm.ExecuteNonQuery()
        cmdfrm.CommandText = "insert into " & Trim(Common_Procedures.ReportTempSubTable) & " (int1, int2, int3, name1, name2,  name3, weight1, name4, weight2, currency1, meters1, meters2, meters3, meters4, meters5, weight6, weight7) select a.company_idno, a.cloth_idno, a.stockoff_idno, a.weaver_clothreceipt_code, a.lot_code, a.lot_no, a.for_orderby, a.piece_no, a.pieceno_orderby, a.folding, 0, 0, 0, 0, a.type5_meters, a.Weight, a.Weight_Meter from weaver_clothreceipt_piece_details a inner join company_head tz on a.company_idno <> 0 and a.company_idno = tz.company_idno inner join ledger_head tsp on a.stockoff_idno <> 0 and a.stockoff_idno = tsp.ledger_idno inner join JobWork_Piece_Delivery_Head b ON b.JobWork_Piece_Delivery_Date > @uptodate and a.packingslip_code_type5 = 'JPCDC-' + b.JobWork_Piece_Delivery_Code where a.packingslip_code_type5 <> '' and a.type5_meters <> 0 and a.weaver_clothreceipt_date <= @uptodate and b.JobWork_Piece_Delivery_Date > @uptodate "
        cmdfrm.ExecuteNonQuery()


        cmdfrm.CommandText = "truncate table " & Trim(Common_Procedures.ReportTempTable) & ""
        cmdfrm.ExecuteNonQuery()

        cmdfrm.CommandText = "insert into " & Trim(Common_Procedures.ReportTempTable) & " (int1, int2, int3, name1, name2,  name3, weight1, name4, weight2, currency1, meters1, meters2, meters3, meters4, meters5 ) select int1, int2, int3, name1, name2, name3, weight1, name4, weight2, currency1, sum(meters1), sum(meters2), sum(meters3), sum(meters4), sum(meters5) from " & Trim(Common_Procedures.ReportTempSubTable) & " group by int1, int2, int3, name1, name2, name3, weight1, name4, weight2, currency1 having sum(meters1) <> 0 or sum(meters2) <> 0 or sum(meters3) <> 0 or sum(meters4) <> 0 or sum(meters5) <> 0 "
        cmdfrm.ExecuteNonQuery()


        Prevnewcd = ""
        tt_pcs = 0
        tt_typ1_mtrs = 0
        tt_typ2_mtrs = 0
        tt_typ3_mtrs = 0
        tt_typ4_mtrs = 0
        tt_typ5_mtrs = 0
        tt_wgt = 0

        da1 = New SqlClient.SqlDataAdapter("select int1 as company_idno, int2 as cloth_idno, int3 as stockoff_idno, name1 as weaver_clothreceipt_code, name2 as lot_code, name3 as lot_no, weight1 as for_orderby, name4 as piece_no, weight2 as pieceno_orderby, currency1 as folding, meters1 as type1_meters, meters2 as type2_meters, meters3 as type3_meters, meters4 as type4_meters, meters5 as type5_meters, weight6 as Weight, weight7 as Weight_Meter from " & Trim(Common_Procedures.ReportTempTable) & " Where meters1 <> 0 or meters2 <> 0 or meters3 <> 0 or meters4 <> 0 or meters5 <> 0 Order by int1, name1, name2, name3, Weight1, weight2, name4", CnFrm)
        dt1 = New DataTable
        da1.Fill(dt1)

        If dt1.Rows.Count > 0 Then

            For i = 0 To dt1.Rows.Count - 1

                Me.Text = "Piece opening  -  " & dt1.Rows(i).Item("lot_no").ToString

                compidno = Val(dt1.Rows(i).Item("company_idno").ToString)
                ledidno = Val(dt1.Rows(i).Item("stockoff_idno").ToString)
                clth_id = Val(dt1.Rows(i).Item("cloth_idno").ToString)
                FldPerc = Val(dt1.Rows(i).Item("folding").ToString)

                PcsNo = dt1.Rows(i).Item("piece_no").ToString
                PcsOrdByNo = Format(Val(dt1.Rows(i).Item("pieceno_orderby").ToString), "##########0.00")

                T1_Mtrs = Format(Val(dt1.Rows(i).Item("type1_meters").ToString), "##########0.00")
                T2_Mtrs = Format(Val(dt1.Rows(i).Item("type2_meters").ToString), "##########0.00")
                T3_Mtrs = Format(Val(dt1.Rows(i).Item("type3_meters").ToString), "##########0.00")
                T4_Mtrs = Format(Val(dt1.Rows(i).Item("type4_meters").ToString), "##########0.00")
                T5_Mtrs = Format(Val(dt1.Rows(i).Item("type5_meters").ToString), "##########0.00")
                Tot_Mtrs = Format(T1_Mtrs + T2_Mtrs + T3_Mtrs + T4_Mtrs + T5_Mtrs, "##########0.00")

                Wgt = Format(Val(dt1.Rows(i).Item("Weight").ToString), "##########0.000")
                Wt_Mtr = Format(Val(dt1.Rows(i).Item("Weight_Meter").ToString), "##########0.000")

                newcode = Trim(Val(compidno)) & "-" & Trim(dt1.Rows(i).Item("lot_no").ToString) & "/" & Trim(opyrcode)
                newno = Trim(dt1.Rows(i).Item("lot_no").ToString)

                If Trim(UCase(newcode)) = "1-1227/15-16" Then
                    Debug.Print(dt1.Rows(i).Item("lot_code").ToString)
                End If

                If Trim(UCase(Prevnewcd)) <> Trim(UCase(newcode)) Then

                    If Trim(Prevnewcd) <> "" Then
                        cmdto.CommandText = "Update piece_opening_head set total_pieces = " & Str(Val(tt_pcs)) & ", total_type1_meters = " & Str(Val(tt_typ1_mtrs)) & ",  total_type2_meters = " & Str(Val(tt_typ2_mtrs)) & ",  total_type3_meters = " & Str(Val(tt_typ3_mtrs)) & ", total_type4_meters = " & Str(Val(tt_typ4_mtrs)) & ", total_type5_meters  = " & Str(Val(tt_typ5_mtrs)) & ", total_checking_meters  = " & Str(Val(tt_typ1_mtrs + tt_typ2_mtrs + tt_typ3_mtrs + tt_typ4_mtrs + tt_typ5_mtrs)) & ", total_weight = " & Str(Val(tt_typ5_mtrs)) & " Where piece_opening_code = '" & Trim(Prevnewcd) & "'"
                        cmdto.ExecuteNonQuery()
                    End If

                End If

                DupPcSubNo = 64


LOOP1:
                da1 = New SqlClient.SqlDataAdapter("select * from Weaver_ClothReceipt_Piece_Details Where Lot_Code = '" & Trim(newcode) & "' and Piece_No = '" & Trim(PcsNo) & "'", CnTo)
                da1.SelectCommand.Transaction = sqltr
                dt2 = New DataTable
                da1.Fill(dt2)
                If dt2.Rows.Count > 0 Then
                    DupPcSubNo = DupPcSubNo + 1
                    newcode = Trim(Val(compidno)) & "-" & Trim(dt1.Rows(i).Item("lot_no").ToString) & Trim(UCase(Chr(DupPcSubNo))) & "/" & Trim(opyrcode)
                    newno = Trim(dt1.Rows(i).Item("lot_no").ToString) & Trim(UCase(Chr(DupPcSubNo)))
                    If DupPcSubNo <= 90 Then
                        GoTo LOOP1

                    Else
                        newcode = Trim(Val(compidno)) & "-" & Trim(dt1.Rows(i).Item("lot_no").ToString) & "/" & Trim(opyrcode)
                        Throw New ApplicationException("Duplicate LotNo & Piece No : " & newcode & " ,   " & PcsNo)
                        Exit Sub

                    End If

                End If
                dt2.Clear()


                If Trim(UCase(Prevnewcd)) <> Trim(UCase(newcode)) Then

                    tt_pcs = 0
                    tt_typ1_mtrs = 0
                    tt_typ2_mtrs = 0
                    tt_typ3_mtrs = 0
                    tt_typ4_mtrs = 0
                    tt_typ5_mtrs = 0
                    tt_wgt = 0

LOOP2:
                    da1 = New SqlClient.SqlDataAdapter("select * from Weaver_ClothReceipt_Piece_Details Where Lot_Code = '" & Trim(newcode) & "' and Piece_No = '" & Trim(PcsNo) & "'", CnTo)
                    da1.SelectCommand.Transaction = sqltr
                    dt2 = New DataTable
                    da1.Fill(dt2)
                    If dt2.Rows.Count > 0 Then
                        DupPcSubNo = DupPcSubNo + 1
                        newcode = Trim(Val(compidno)) & "-" & Trim(dt1.Rows(i).Item("lot_no").ToString) & Trim(UCase(Chr(DupPcSubNo))) & "/" & Trim(opyrcode)
                        newno = Trim(dt1.Rows(i).Item("lot_no").ToString) & Trim(UCase(Chr(DupPcSubNo)))
                        If DupPcSubNo <= 90 Then
                            GoTo LOOP2

                        Else
                            newcode = Trim(Val(compidno)) & "-" & Trim(dt1.Rows(i).Item("lot_no").ToString) & "/" & Trim(opyrcode)
                            Throw New ApplicationException("Duplicate LotNo & Piece No : " & newcode & " ,   " & PcsNo)
                            Exit Sub

                        End If

                    End If
                    dt2.Clear()


                    da1 = New SqlClient.SqlDataAdapter("select * from piece_opening_head Where piece_opening_code = '" & Trim(newcode) & "'", CnTo)
                    da1.SelectCommand.Transaction = sqltr
                    dt2 = New DataTable
                    da1.Fill(dt2)
                    If dt2.Rows.Count = 0 Then
                        cmdto.CommandText = "Insert into piece_opening_head (    piece_opening_code,             company_idno  ,     piece_opening_no ,                               for_orderby                     ,   ledger_idno         ,       cloth_idno         ,             folding      , total_pieces ,  total_type1_meters ,  total_type2_meters ,  total_type3_meters, total_type4_meters, total_type5_meters, total_checking_meters , total_weight   ) " & _
                                                    "     Values          ( '" & Trim(newcode) & "', " & Str(Val(compidno)) & ", '" & Trim(newno) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(newno))) & ", " & Val(ledidno) & "  , " & Str(Val(clth_id)) & ", " & Str(Val(FldPerc)) & ",     0        ,          0          ,           0         ,           0        ,        0          ,           0       ,        0              ,       0        ) "
                        cmdto.ExecuteNonQuery()
                    End If
                    dt2.Clear()

                End If


                cmdto.CommandText = "Insert into Weaver_ClothReceipt_Piece_Details ( Weaver_Piece_Checking_Code ,       Company_IdNo        ,  Weaver_Piece_Checking_No ,           Weaver_ClothReceipt_Code           ,       Weaver_ClothReceipt_No ,                               for_orderby                     ,  Weaver_ClothReceipt_Date,         Lot_Code       ,          Lot_No      ,         StockOff_IdNo,       Ledger_IdNo   ,           Cloth_IdNo     ,             Folding       ,           Sl_No      ,           PieceNo_OrderBy      ,      Main_PieceNo       ,      Piece_No        ,          Type1_Meters,         Type2_Meters,         Type3_Meters,         Type4_Meters,  Type5_Meters    ,   Total_Checking_Meters ,     Weight      ,     Weight_Meter    ) " & _
                                    "     Values                                 (    '" & Trim(newcode) & "'   , " & Str(Val(compidno)) & ",  '" & Trim(newno) & "'    ,  '" & Trim(pk_condition) & Trim(newcode) & "',   '" & Trim(newno) & "'      , " & Str(Val(Common_Procedures.OrderBy_CodeToValue(newno))) & ",          @opdate         , '" & Trim(newcode) & "', '" & Trim(newno) & "',  " & Val(ledidno) & ", " & Val(ledidno) & " , " & Str(Val(clth_id)) & ", " & Str(Val(FldPerc)) & ",  " & Str(Val(sno)) & ",   " & Str(Val(PcsOrdByNo)) & ",  " & Str(Val(PcsNo)) & ", '" & Trim(PcsNo) & "',  " & Str(T1_Mtrs) & ", " & Str(T2_Mtrs) & ", " & Str(T3_Mtrs) & ", " & Str(T4_Mtrs) & ", " & Str(T5_Mtrs) & ", " & Str(Tot_Mtrs) & "   , " & Str(Wgt) & ", " & Str(Wt_Mtr) & " ) "
                cmdto.ExecuteNonQuery()


                Prevnewcd = newcode

                tt_pcs = tt_pcs + 1

                tt_typ1_mtrs = tt_typ1_mtrs + T1_Mtrs
                tt_typ2_mtrs = tt_typ2_mtrs + T2_Mtrs
                tt_typ3_mtrs = tt_typ3_mtrs + T3_Mtrs
                tt_typ4_mtrs = tt_typ4_mtrs + T4_Mtrs
                tt_typ5_mtrs = tt_typ5_mtrs + T5_Mtrs

                tt_wgt = tt_wgt + Wgt

            Next i

        End If

        Me.Text = ""

    End Sub

    Private Sub btn_JobWorker_Delivered_Piece_Transfer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_JobWorker_Delivered_Piece_Transfer.Click
        Dim tr As SqlClient.SqlTransaction

        If Trim(cbo_DBFrom.Text) = "" Then
            MessageBox.Show("Invalid Database Name", "DOES NOT TRANSFER...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_DBFrom.Enabled Then cbo_DBFrom.Focus()
            Exit Sub
        End If

        If MessageBox.Show("All the datas will lost " & Chr(13) & "Are you sure you want to Transfer?", "FOR MASTER TRANSFER...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

        MDIParent1.Cursor = Cursors.WaitCursor
        Me.Cursor = Cursors.WaitCursor

        btn_JobWorker_Delivered_Piece_Transfer.Enabled = False
        Me.Text = ""

        CnFrm = New SqlClient.SqlConnection("Data Source=" & Trim(Common_Procedures.ServerName) & ";Initial Catalog=" & Trim(cbo_DBFrom.Text) & ";User ID=sa;Password=" & Trim(Common_Procedures.ServerPassword) & ";Integrated Security=False")

        CnFrm.Open()
        CnTo.Open()

        tr = CnTo.BeginTransaction

        Try

            JobWork_Deliverd_PieceOpening_Updation(tr)

            tr.Commit()

            Me.Text = "PIECE OPENING TRANSFERED"

            MDIParent1.Cursor = Cursors.Default
            Me.Cursor = Cursors.Default

            MessageBox.Show("Piece Transfered Transfered Sucessfully", "FOR PIECE OPENING...", MessageBoxButtons.OK, MessageBoxIcon.Information)

            btn_JobWorker_Delivered_Piece_Transfer.Enabled = True

        Catch ex As Exception

            tr.Rollback()
            Me.Text = "PIECE OPENING TRANSFERED"
            MDIParent1.Cursor = Cursors.Default
            Me.Cursor = Cursors.Default
            btn_JobWorker_Delivered_Piece_Transfer.Enabled = True
            MessageBox.Show(ex.Message, "INVALID TRANSFER...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            CnFrm.Close()
            CnTo.Close()
            tr.Dispose()

            btn_JobWorker_Delivered_Piece_Transfer.Enabled = True
            Me.Text = "PIECE OPENING TRANSFERED"

            MDIParent1.Cursor = Cursors.Default
            Me.Cursor = Cursors.Default

        End Try

    End Sub

    Private Sub JobWork_Deliverd_PieceOpening_Updation(ByVal sqltr As SqlClient.SqlTransaction)
        Dim cmdfrm As New SqlClient.SqlCommand
        Dim cmdto As New SqlClient.SqlCommand
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim opdate As Date
        Dim opbal As Single = 0
        Dim i As Integer = 0
        Dim sno As Integer = 0
        Dim compidno As Integer = 0
        Dim ledidno As Integer = 0
        Dim clth_id As Integer = 0
        Dim newcode As String = ""
        Dim newno As String = ""
        Dim opyrcode As String = ""
        Dim pk_condition As String = ""
        Dim FldPerc As Single = 0
        Dim PcsNo As String = ""
        Dim PcsOrdByNo As Double = 0
        Dim T1_Mtrs As Double = 0
        Dim T2_Mtrs As Double = 0
        Dim T3_Mtrs As Double = 0
        Dim T4_Mtrs As Double = 0
        Dim T5_Mtrs As Double = 0
        Dim Tot_Mtrs As Double = 0
        Dim Wgt As Double = 0
        Dim Wt_Mtr As Double = 0
        Dim tt_pcs As Integer = 0
        Dim tt_typ1_mtrs As Double = 0
        Dim tt_typ2_mtrs As Double = 0
        Dim tt_typ3_mtrs As Double = 0
        Dim tt_typ4_mtrs As Double = 0
        Dim tt_typ5_mtrs As Double = 0
        Dim tt_wgt As Double = 0
        Dim Prevnewcd As String = ""
        Dim DupPcSubNo As Integer = 0


        Me.Text = "piece opening"

        pk_condition = "openi-"

        cmdfrm.Connection = CnFrm

        cmdto.Connection = CnTo
        cmdto.Transaction = sqltr


        opdate = New DateTime(Val(Microsoft.VisualBasic.Right(Common_Procedures.FnRange, 4)) - 1, 4, 1)
        opdate = DateAdd(DateInterval.Day, -1, opdate)

        opyrcode = Microsoft.VisualBasic.Left(Trim(Common_Procedures.FnRange), 4)
        opyrcode = Trim(Mid(Val(opyrcode) - 1, 3, 2)) & "-" & Trim(Microsoft.VisualBasic.Right(opyrcode, 2))


        cmdfrm.Parameters.Clear()
        cmdfrm.Parameters.AddWithValue("@uptodate", dtp_UpDate.Value.Date)

        cmdto.Parameters.Clear()
        cmdto.Parameters.AddWithValue("@opdate", opdate)


        'cmdto.CommandText = "delete from piece_opening_head"
        'cmdto.ExecuteNonQuery()

        'cmdto.CommandText = "delete from weaver_clothreceipt_piece_details where weaver_clothreceipt_code LIKE '%/" & Trim(opyrcode) & "'"
        'cmdto.ExecuteNonQuery()

        cmdfrm.CommandText = "truncate table " & Trim(Common_Procedures.ReportTempSubTable) & ""
        cmdfrm.ExecuteNonQuery()

        '-----  jobwork piece delivered after specified date
        cmdfrm.CommandText = "insert into " & Trim(Common_Procedures.ReportTempSubTable) & " (int1, int2, int3, name1, name2,  name3, weight1, name4, weight2, currency1, meters1, meters2, meters3, meters4, meters5, weight6, weight7) select a.company_idno, a.cloth_idno, a.stockoff_idno, a.weaver_clothreceipt_code, a.lot_code, a.lot_no, a.for_orderby, a.piece_no, a.pieceno_orderby, a.folding, a.type1_meters, 0, 0, 0, 0, a.Weight, a.Weight_Meter from weaver_clothreceipt_piece_details a inner join company_head tz on a.company_idno <> 0 and a.company_idno = tz.company_idno inner join ledger_head tsp on a.stockoff_idno <> 0 and a.stockoff_idno = tsp.ledger_idno inner join JobWork_Piece_Delivery_Head b ON b.JobWork_Piece_Delivery_Date > @uptodate and a.packingslip_code_type1 = 'JPCDC-' + b.JobWork_Piece_Delivery_Code where a.packingslip_code_type1 <> '' and a.type1_meters <> 0 and a.weaver_clothreceipt_date <= @uptodate and b.JobWork_Piece_Delivery_Date > @uptodate"
        cmdfrm.ExecuteNonQuery()
        cmdfrm.CommandText = "insert into " & Trim(Common_Procedures.ReportTempSubTable) & " (int1, int2, int3, name1, name2,  name3, weight1, name4, weight2, currency1, meters1, meters2, meters3, meters4, meters5, weight6, weight7) select a.company_idno, a.cloth_idno, a.stockoff_idno, a.weaver_clothreceipt_code, a.lot_code, a.lot_no, a.for_orderby, a.piece_no, a.pieceno_orderby, a.folding, 0, a.type2_meters, 0, 0, 0, a.Weight, a.Weight_Meter from weaver_clothreceipt_piece_details a inner join company_head tz on a.company_idno <> 0 and a.company_idno = tz.company_idno inner join ledger_head tsp on a.stockoff_idno <> 0 and a.stockoff_idno = tsp.ledger_idno inner join JobWork_Piece_Delivery_Head b ON b.JobWork_Piece_Delivery_Date > @uptodate and a.packingslip_code_type2 = 'JPCDC-' + b.JobWork_Piece_Delivery_Code where a.packingslip_code_type2 <> '' and a.type2_meters <> 0 and a.weaver_clothreceipt_date <= @uptodate and b.JobWork_Piece_Delivery_Date > @uptodate "
        cmdfrm.ExecuteNonQuery()
        cmdfrm.CommandText = "insert into " & Trim(Common_Procedures.ReportTempSubTable) & " (int1, int2, int3, name1, name2,  name3, weight1, name4, weight2, currency1, meters1, meters2, meters3, meters4, meters5, weight6, weight7) select a.company_idno, a.cloth_idno, a.stockoff_idno, a.weaver_clothreceipt_code, a.lot_code, a.lot_no, a.for_orderby, a.piece_no, a.pieceno_orderby, a.folding, 0, 0, a.type3_meters, 0, 0, a.Weight, a.Weight_Meter from weaver_clothreceipt_piece_details a inner join company_head tz on a.company_idno <> 0 and a.company_idno = tz.company_idno inner join ledger_head tsp on a.stockoff_idno <> 0 and a.stockoff_idno = tsp.ledger_idno inner join JobWork_Piece_Delivery_Head b ON b.JobWork_Piece_Delivery_Date > @uptodate and a.packingslip_code_type3 = 'JPCDC-' + b.JobWork_Piece_Delivery_Code where a.packingslip_code_type3 <> '' and a.type3_meters <> 0 and a.weaver_clothreceipt_date <= @uptodate and b.JobWork_Piece_Delivery_Date > @uptodate "
        cmdfrm.ExecuteNonQuery()
        cmdfrm.CommandText = "insert into " & Trim(Common_Procedures.ReportTempSubTable) & " (int1, int2, int3, name1, name2,  name3, weight1, name4, weight2, currency1, meters1, meters2, meters3, meters4, meters5, weight6, weight7) select a.company_idno, a.cloth_idno, a.stockoff_idno, a.weaver_clothreceipt_code, a.lot_code, a.lot_no, a.for_orderby, a.piece_no, a.pieceno_orderby, a.folding, 0, 0, 0, a.type4_meters, 0, a.Weight, a.Weight_Meter from weaver_clothreceipt_piece_details a inner join company_head tz on a.company_idno <> 0 and a.company_idno = tz.company_idno inner join ledger_head tsp on a.stockoff_idno <> 0 and a.stockoff_idno = tsp.ledger_idno inner join JobWork_Piece_Delivery_Head b ON b.JobWork_Piece_Delivery_Date > @uptodate and a.packingslip_code_type4 = 'JPCDC-' + b.JobWork_Piece_Delivery_Code where a.packingslip_code_type4 <> '' and a.type4_meters <> 0 and a.weaver_clothreceipt_date <= @uptodate and b.JobWork_Piece_Delivery_Date > @uptodate "
        cmdfrm.ExecuteNonQuery()
        cmdfrm.CommandText = "insert into " & Trim(Common_Procedures.ReportTempSubTable) & " (int1, int2, int3, name1, name2,  name3, weight1, name4, weight2, currency1, meters1, meters2, meters3, meters4, meters5, weight6, weight7) select a.company_idno, a.cloth_idno, a.stockoff_idno, a.weaver_clothreceipt_code, a.lot_code, a.lot_no, a.for_orderby, a.piece_no, a.pieceno_orderby, a.folding, 0, 0, 0, 0, a.type5_meters, a.Weight, a.Weight_Meter from weaver_clothreceipt_piece_details a inner join company_head tz on a.company_idno <> 0 and a.company_idno = tz.company_idno inner join ledger_head tsp on a.stockoff_idno <> 0 and a.stockoff_idno = tsp.ledger_idno inner join JobWork_Piece_Delivery_Head b ON b.JobWork_Piece_Delivery_Date > @uptodate and a.packingslip_code_type5 = 'JPCDC-' + b.JobWork_Piece_Delivery_Code where a.packingslip_code_type5 <> '' and a.type5_meters <> 0 and a.weaver_clothreceipt_date <= @uptodate and b.JobWork_Piece_Delivery_Date > @uptodate "
        cmdfrm.ExecuteNonQuery()

        '-----  piece transfered after specified date
        cmdfrm.CommandText = "insert into " & Trim(Common_Procedures.ReportTempSubTable) & " (int1, int2, int3, name1, name2,  name3, weight1, name4, weight2, currency1, meters1, meters2, meters3, meters4, meters5, weight6, weight7) select a.company_idno, a.cloth_idno, a.stockoff_idno, a.weaver_clothreceipt_code, a.lot_code, a.lot_no, a.for_orderby, a.piece_no, a.pieceno_orderby, a.folding, a.type1_meters, 0, 0, 0, 0, a.Weight, a.Weight_Meter from weaver_clothreceipt_piece_details a inner join company_head tz on a.company_idno <> 0 and a.company_idno = tz.company_idno inner join ledger_head tsp on a.stockoff_idno <> 0 and a.stockoff_idno = tsp.ledger_idno inner join Piece_Transfer_Head b ON b.Piece_Transfer_Date > @uptodate and a.packingslip_code_type1 = 'PCSTR-' + b.Piece_Transfer_Code where a.packingslip_code_type1 <> '' and a.type1_meters <> 0 and a.weaver_clothreceipt_date <= @uptodate and b.Piece_Transfer_Date > @uptodate"
        cmdfrm.ExecuteNonQuery()
        cmdfrm.CommandText = "insert into " & Trim(Common_Procedures.ReportTempSubTable) & " (int1, int2, int3, name1, name2,  name3, weight1, name4, weight2, currency1, meters1, meters2, meters3, meters4, meters5, weight6, weight7) select a.company_idno, a.cloth_idno, a.stockoff_idno, a.weaver_clothreceipt_code, a.lot_code, a.lot_no, a.for_orderby, a.piece_no, a.pieceno_orderby, a.folding, 0, a.type2_meters, 0, 0, 0, a.Weight, a.Weight_Meter from weaver_clothreceipt_piece_details a inner join company_head tz on a.company_idno <> 0 and a.company_idno = tz.company_idno inner join ledger_head tsp on a.stockoff_idno <> 0 and a.stockoff_idno = tsp.ledger_idno inner join Piece_Transfer_Head b ON b.Piece_Transfer_Date > @uptodate and a.packingslip_code_type2 = 'PCSTR-' + b.Piece_Transfer_Code where a.packingslip_code_type2 <> '' and a.type2_meters <> 0 and a.weaver_clothreceipt_date <= @uptodate and b.Piece_Transfer_Date > @uptodate "
        cmdfrm.ExecuteNonQuery()
        cmdfrm.CommandText = "insert into " & Trim(Common_Procedures.ReportTempSubTable) & " (int1, int2, int3, name1, name2,  name3, weight1, name4, weight2, currency1, meters1, meters2, meters3, meters4, meters5, weight6, weight7) select a.company_idno, a.cloth_idno, a.stockoff_idno, a.weaver_clothreceipt_code, a.lot_code, a.lot_no, a.for_orderby, a.piece_no, a.pieceno_orderby, a.folding, 0, 0, a.type3_meters, 0, 0, a.Weight, a.Weight_Meter from weaver_clothreceipt_piece_details a inner join company_head tz on a.company_idno <> 0 and a.company_idno = tz.company_idno inner join ledger_head tsp on a.stockoff_idno <> 0 and a.stockoff_idno = tsp.ledger_idno inner join Piece_Transfer_Head b ON b.Piece_Transfer_Date > @uptodate and a.packingslip_code_type3 = 'PCSTR-' + b.Piece_Transfer_Code where a.packingslip_code_type3 <> '' and a.type3_meters <> 0 and a.weaver_clothreceipt_date <= @uptodate and b.Piece_Transfer_Date > @uptodate "
        cmdfrm.ExecuteNonQuery()
        cmdfrm.CommandText = "insert into " & Trim(Common_Procedures.ReportTempSubTable) & " (int1, int2, int3, name1, name2,  name3, weight1, name4, weight2, currency1, meters1, meters2, meters3, meters4, meters5, weight6, weight7) select a.company_idno, a.cloth_idno, a.stockoff_idno, a.weaver_clothreceipt_code, a.lot_code, a.lot_no, a.for_orderby, a.piece_no, a.pieceno_orderby, a.folding, 0, 0, 0, a.type4_meters, 0, a.Weight, a.Weight_Meter from weaver_clothreceipt_piece_details a inner join company_head tz on a.company_idno <> 0 and a.company_idno = tz.company_idno inner join ledger_head tsp on a.stockoff_idno <> 0 and a.stockoff_idno = tsp.ledger_idno inner join Piece_Transfer_Head b ON b.Piece_Transfer_Date > @uptodate and a.packingslip_code_type4 = 'PCSTR-' + b.Piece_Transfer_Code where a.packingslip_code_type4 <> '' and a.type4_meters <> 0 and a.weaver_clothreceipt_date <= @uptodate and b.Piece_Transfer_Date > @uptodate "
        cmdfrm.ExecuteNonQuery()
        cmdfrm.CommandText = "insert into " & Trim(Common_Procedures.ReportTempSubTable) & " (int1, int2, int3, name1, name2,  name3, weight1, name4, weight2, currency1, meters1, meters2, meters3, meters4, meters5, weight6, weight7) select a.company_idno, a.cloth_idno, a.stockoff_idno, a.weaver_clothreceipt_code, a.lot_code, a.lot_no, a.for_orderby, a.piece_no, a.pieceno_orderby, a.folding, 0, 0, 0, 0, a.type5_meters, a.Weight, a.Weight_Meter from weaver_clothreceipt_piece_details a inner join company_head tz on a.company_idno <> 0 and a.company_idno = tz.company_idno inner join ledger_head tsp on a.stockoff_idno <> 0 and a.stockoff_idno = tsp.ledger_idno inner join Piece_Transfer_Head b ON b.Piece_Transfer_Date > @uptodate and a.packingslip_code_type5 = 'PCSTR-' + b.Piece_Transfer_Code where a.packingslip_code_type5 <> '' and a.type5_meters <> 0 and a.weaver_clothreceipt_date <= @uptodate and b.Piece_Transfer_Date > @uptodate "
        cmdfrm.ExecuteNonQuery()

        cmdfrm.CommandText = "truncate table " & Trim(Common_Procedures.ReportTempTable) & ""
        cmdfrm.ExecuteNonQuery()

        cmdfrm.CommandText = "insert into " & Trim(Common_Procedures.ReportTempTable) & " (int1, int2, int3, name1, name2,  name3, weight1, name4, weight2, currency1, meters1, meters2, meters3, meters4, meters5 ) select int1, int2, int3, name1, name2, name3, weight1, name4, weight2, currency1, sum(meters1), sum(meters2), sum(meters3), sum(meters4), sum(meters5) from " & Trim(Common_Procedures.ReportTempSubTable) & " group by int1, int2, int3, name1, name2, name3, weight1, name4, weight2, currency1 having sum(meters1) <> 0 or sum(meters2) <> 0 or sum(meters3) <> 0 or sum(meters4) <> 0 or sum(meters5) <> 0 "
        cmdfrm.ExecuteNonQuery()


        Prevnewcd = ""
        tt_pcs = 0
        tt_typ1_mtrs = 0
        tt_typ2_mtrs = 0
        tt_typ3_mtrs = 0
        tt_typ4_mtrs = 0
        tt_typ5_mtrs = 0
        tt_wgt = 0

        da1 = New SqlClient.SqlDataAdapter("select int1 as company_idno, int2 as cloth_idno, int3 as stockoff_idno, name1 as weaver_clothreceipt_code, name2 as lot_code, name3 as lot_no, weight1 as for_orderby, name4 as piece_no, weight2 as pieceno_orderby, currency1 as folding, meters1 as type1_meters, meters2 as type2_meters, meters3 as type3_meters, meters4 as type4_meters, meters5 as type5_meters, weight6 as Weight, weight7 as Weight_Meter from " & Trim(Common_Procedures.ReportTempTable) & " Where meters1 <> 0 or meters2 <> 0 or meters3 <> 0 or meters4 <> 0 or meters5 <> 0 Order by int1, name1, name2, name3, Weight1, weight2, name4", CnFrm)
        dt1 = New DataTable
        da1.Fill(dt1)

        If dt1.Rows.Count > 0 Then

            For i = 0 To dt1.Rows.Count - 1

                Me.Text = "Piece opening  -  " & dt1.Rows(i).Item("lot_no").ToString

                compidno = Val(dt1.Rows(i).Item("company_idno").ToString)
                ledidno = Val(dt1.Rows(i).Item("stockoff_idno").ToString)
                clth_id = Val(dt1.Rows(i).Item("cloth_idno").ToString)
                FldPerc = Val(dt1.Rows(i).Item("folding").ToString)

                PcsNo = dt1.Rows(i).Item("piece_no").ToString
                PcsOrdByNo = Format(Val(dt1.Rows(i).Item("pieceno_orderby").ToString), "##########0.00")

                T1_Mtrs = Format(Val(dt1.Rows(i).Item("type1_meters").ToString), "##########0.00")
                T2_Mtrs = Format(Val(dt1.Rows(i).Item("type2_meters").ToString), "##########0.00")
                T3_Mtrs = Format(Val(dt1.Rows(i).Item("type3_meters").ToString), "##########0.00")
                T4_Mtrs = Format(Val(dt1.Rows(i).Item("type4_meters").ToString), "##########0.00")
                T5_Mtrs = Format(Val(dt1.Rows(i).Item("type5_meters").ToString), "##########0.00")
                Tot_Mtrs = Format(T1_Mtrs + T2_Mtrs + T3_Mtrs + T4_Mtrs + T5_Mtrs, "##########0.00")

                Wgt = Format(Val(dt1.Rows(i).Item("Weight").ToString), "##########0.000")
                Wt_Mtr = Format(Val(dt1.Rows(i).Item("Weight_Meter").ToString), "##########0.000")

                newcode = Trim(Val(compidno)) & "-" & Trim(dt1.Rows(i).Item("lot_no").ToString) & "/" & Trim(opyrcode)
                newno = Trim(dt1.Rows(i).Item("lot_no").ToString)

                If Trim(UCase(newcode)) = "1-1227/15-16" Then
                    Debug.Print(dt1.Rows(i).Item("lot_code").ToString)
                End If

                If Trim(UCase(Prevnewcd)) <> Trim(UCase(newcode)) Then

                    If Trim(Prevnewcd) <> "" Then
                        cmdto.CommandText = "Update piece_opening_head set total_pieces = " & Str(Val(tt_pcs)) & ", total_type1_meters = " & Str(Val(tt_typ1_mtrs)) & ",  total_type2_meters = " & Str(Val(tt_typ2_mtrs)) & ",  total_type3_meters = " & Str(Val(tt_typ3_mtrs)) & ", total_type4_meters = " & Str(Val(tt_typ4_mtrs)) & ", total_type5_meters  = " & Str(Val(tt_typ5_mtrs)) & ", total_checking_meters  = " & Str(Val(tt_typ1_mtrs + tt_typ2_mtrs + tt_typ3_mtrs + tt_typ4_mtrs + tt_typ5_mtrs)) & ", total_weight = " & Str(Val(tt_typ5_mtrs)) & " Where piece_opening_code = '" & Trim(Prevnewcd) & "'"
                        cmdto.ExecuteNonQuery()
                    End If

                End If

                DupPcSubNo = 64


LOOP1:
                da1 = New SqlClient.SqlDataAdapter("select * from Weaver_ClothReceipt_Piece_Details Where Lot_Code = '" & Trim(newcode) & "' and Piece_No = '" & Trim(PcsNo) & "'", CnTo)
                da1.SelectCommand.Transaction = sqltr
                dt2 = New DataTable
                da1.Fill(dt2)
                If dt2.Rows.Count > 0 Then
                    DupPcSubNo = DupPcSubNo + 1
                    newcode = Trim(Val(compidno)) & "-" & Trim(dt1.Rows(i).Item("lot_no").ToString) & Trim(UCase(Chr(DupPcSubNo))) & "/" & Trim(opyrcode)
                    newno = Trim(dt1.Rows(i).Item("lot_no").ToString) & Trim(UCase(Chr(DupPcSubNo)))
                    If DupPcSubNo <= 90 Then
                        GoTo LOOP1

                    Else
                        newcode = Trim(Val(compidno)) & "-" & Trim(dt1.Rows(i).Item("lot_no").ToString) & "/" & Trim(opyrcode)
                        Throw New ApplicationException("Duplicate LotNo & Piece No : " & newcode & " ,   " & PcsNo)
                        Exit Sub

                    End If

                End If
                dt2.Clear()


                If Trim(UCase(Prevnewcd)) <> Trim(UCase(newcode)) Then

                    tt_pcs = 0
                    tt_typ1_mtrs = 0
                    tt_typ2_mtrs = 0
                    tt_typ3_mtrs = 0
                    tt_typ4_mtrs = 0
                    tt_typ5_mtrs = 0
                    tt_wgt = 0

LOOP2:
                    da1 = New SqlClient.SqlDataAdapter("select * from Weaver_ClothReceipt_Piece_Details Where Lot_Code = '" & Trim(newcode) & "' and Piece_No = '" & Trim(PcsNo) & "'", CnTo)
                    da1.SelectCommand.Transaction = sqltr
                    dt2 = New DataTable
                    da1.Fill(dt2)
                    If dt2.Rows.Count > 0 Then
                        DupPcSubNo = DupPcSubNo + 1
                        newcode = Trim(Val(compidno)) & "-" & Trim(dt1.Rows(i).Item("lot_no").ToString) & Trim(UCase(Chr(DupPcSubNo))) & "/" & Trim(opyrcode)
                        newno = Trim(dt1.Rows(i).Item("lot_no").ToString) & Trim(UCase(Chr(DupPcSubNo)))
                        If DupPcSubNo <= 90 Then
                            GoTo LOOP2

                        Else
                            newcode = Trim(Val(compidno)) & "-" & Trim(dt1.Rows(i).Item("lot_no").ToString) & "/" & Trim(opyrcode)
                            Throw New ApplicationException("Duplicate LotNo & Piece No : " & newcode & " ,   " & PcsNo)
                            Exit Sub

                        End If

                    End If
                    dt2.Clear()


                    da1 = New SqlClient.SqlDataAdapter("select * from piece_opening_head Where piece_opening_code = '" & Trim(newcode) & "'", CnTo)
                    da1.SelectCommand.Transaction = sqltr
                    dt2 = New DataTable
                    da1.Fill(dt2)
                    If dt2.Rows.Count = 0 Then
                        cmdto.CommandText = "Insert into piece_opening_head (    piece_opening_code,             company_idno  ,     piece_opening_no ,                               for_orderby                     ,   ledger_idno         ,       cloth_idno         ,             folding      , total_pieces ,  total_type1_meters ,  total_type2_meters ,  total_type3_meters, total_type4_meters, total_type5_meters, total_checking_meters , total_weight   ) " & _
                                                    "     Values          ( '" & Trim(newcode) & "', " & Str(Val(compidno)) & ", '" & Trim(newno) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(newno))) & ", " & Val(ledidno) & "  , " & Str(Val(clth_id)) & ", " & Str(Val(FldPerc)) & ",     0        ,          0          ,           0         ,           0        ,        0          ,           0       ,        0              ,       0        ) "
                        cmdto.ExecuteNonQuery()
                    End If
                    dt2.Clear()

                End If


                cmdto.CommandText = "Insert into Weaver_ClothReceipt_Piece_Details ( Weaver_Piece_Checking_Code ,       Company_IdNo        ,  Weaver_Piece_Checking_No ,           Weaver_ClothReceipt_Code           ,       Weaver_ClothReceipt_No ,                               for_orderby                     ,  Weaver_ClothReceipt_Date,         Lot_Code       ,          Lot_No      ,         StockOff_IdNo,       Ledger_IdNo   ,           Cloth_IdNo     ,             Folding       ,           Sl_No      ,           PieceNo_OrderBy      ,      Main_PieceNo       ,      Piece_No        ,          Type1_Meters,         Type2_Meters,         Type3_Meters,         Type4_Meters,  Type5_Meters    ,   Total_Checking_Meters ,     Weight      ,     Weight_Meter    ) " & _
                                    "     Values                                 (    '" & Trim(newcode) & "'   , " & Str(Val(compidno)) & ",  '" & Trim(newno) & "'    ,  '" & Trim(pk_condition) & Trim(newcode) & "',   '" & Trim(newno) & "'      , " & Str(Val(Common_Procedures.OrderBy_CodeToValue(newno))) & ",          @opdate         , '" & Trim(newcode) & "', '" & Trim(newno) & "',  " & Val(ledidno) & ", " & Val(ledidno) & " , " & Str(Val(clth_id)) & ", " & Str(Val(FldPerc)) & ",  " & Str(Val(sno)) & ",   " & Str(Val(PcsOrdByNo)) & ",  " & Str(Val(PcsNo)) & ", '" & Trim(PcsNo) & "',  " & Str(T1_Mtrs) & ", " & Str(T2_Mtrs) & ", " & Str(T3_Mtrs) & ", " & Str(T4_Mtrs) & ", " & Str(T5_Mtrs) & ", " & Str(Tot_Mtrs) & "   , " & Str(Wgt) & ", " & Str(Wt_Mtr) & " ) "
                cmdto.ExecuteNonQuery()


                Prevnewcd = newcode

                tt_pcs = tt_pcs + 1

                tt_typ1_mtrs = tt_typ1_mtrs + T1_Mtrs
                tt_typ2_mtrs = tt_typ2_mtrs + T2_Mtrs
                tt_typ3_mtrs = tt_typ3_mtrs + T3_Mtrs
                tt_typ4_mtrs = tt_typ4_mtrs + T4_Mtrs
                tt_typ5_mtrs = tt_typ5_mtrs + T5_Mtrs

                tt_wgt = tt_wgt + Wgt

            Next i

        End If

        Me.Text = ""

    End Sub

End Class