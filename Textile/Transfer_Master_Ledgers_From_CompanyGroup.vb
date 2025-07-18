Imports Excel = Microsoft.Office.Interop.Excel
Imports System.IO

Public Class Transfer_Master_Ledgers_From_CompanyGroup

    Private CnTo As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private CnFrm As New SqlClient.SqlConnection

    Private Sub Transfer_Master_Ledgers_From_CompanyGroup_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        If txt_DbIdNo_From.Enabled And txt_DbIdNo_From.Visible Then txt_DbIdNo_From.Focus()
    End Sub

    Private Sub Transfer_Master_Ledgers_From_CompanyGroup_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        btn_Transfer.Visible = True
        lbl_DbIdNo_From_Caption.Visible = True
        txt_DbIdNo_From.Visible = True

        btn_Transfer_Textile.Visible = False
        cbo_DBFrom_Textile.Visible = False
        lbl_DBFrom_Textile_Caption.Visible = False

        btn_Transfer_Sizing.Visible = False
        cbo_DBFrom_Sizing.Visible = False
        lbl_DBFrom_Sizing_Caption.Visible = False

        btn_Transfer_OE.Visible = False
        cbo_DBFrom_OE.Visible = False
        lbl_DBFrom_OE_Caption.Visible = False


        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then '---- Kalaimagal Textiles (Palladam)

            btn_Transfer.Visible = False
            lbl_DbIdNo_From_Caption.Visible = False
            txt_DbIdNo_From.Visible = False

            btn_Transfer_Textile.Visible = True
            cbo_DBFrom_Textile.Visible = True
            lbl_DBFrom_Textile_Caption.Visible = True

            btn_Transfer_Sizing.Visible = True
            cbo_DBFrom_Sizing.Visible = True
            lbl_DBFrom_Sizing_Caption.Visible = True

            btn_Transfer_OE.Visible = True
            cbo_DBFrom_OE.Visible = True
            lbl_DBFrom_OE_Caption.Visible = True

        End If

        btn_Import_From_Excel.Visible = False
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1035" Then '---- Kalaimagal Textiles (Avinashi)
            btn_Import_From_Excel.Visible = True
        End If

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(cbo_DBFrom_Textile, CnTo, "master..sysdatabases", "name", "(name LIKE 'tsoft%tex%')", "")
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(cbo_DBFrom_Sizing, CnTo, "master..sysdatabases", "name", "(name LIKE 'tsoft%siz%')", "")
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(cbo_DBFrom_OE, CnTo, "master..sysdatabases", "name", "(name LIKE 'tsoft%oe%')", "")

        txt_DbIdNo_From.Text = ""
        cbo_DBFrom_Textile.Text = ""
        cbo_DBFrom_Sizing.Text = ""
        cbo_DBFrom_OE.Text = ""

        Me.Text = "MASTERS TRANSFER"

    End Sub

    Private Sub btn_Transfer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Transfer.Click
        Dim tr As SqlClient.SqlTransaction
        Dim da2 As SqlClient.SqlDataAdapter
        Dim dt2 As DataTable
        Dim DbFrmName As String = ""
        Dim DbFrm_ConnStr As String = ""
        Dim Nr As Long = 0

        If Val(txt_DbIdNo_From.Text) = 0 And Trim(cbo_DBFrom_Textile.Text) = "" Then
            MessageBox.Show("Invalid CompanyGroup From", "DOES NOT TRANSFER...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If txt_DbIdNo_From.Visible And txt_DbIdNo_From.Enabled Then txt_DbIdNo_From.Focus()
            Exit Sub
        End If

        If MessageBox.Show("All the master datas will lost " & Chr(13) & "Are you sure you want to Transfer?", "FOR MASTER TRANSFER...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

        MDIParent1.Cursor = Cursors.WaitCursor
        Me.Cursor = Cursors.WaitCursor

        btn_Transfer.Enabled = False
        Me.Text = ""

        CnTo.Open()

        If Trim(cbo_DBFrom_Textile.Text) <> "" Then
            DbFrmName = Trim(cbo_DBFrom_Textile.Text)

        Else
            DbFrmName = Common_Procedures.get_Company_DataBaseName(Trim(Val(txt_DbIdNo_From.Text)))

        End If


        da2 = New SqlClient.SqlDataAdapter("Select name from master..sysdatabases where name = '" & Trim(DbFrmName) & "'", CnTo)
        dt2 = New DataTable
        da2.Fill(dt2)
        Nr = 0
        If dt2.Rows.Count > 0 Then
            If IsDBNull(dt2.Rows(0).Item("name").ToString) = False Then
                Nr = 1
            End If
        End If
        dt2.Dispose()
        da2.Dispose()

        If Nr = 0 Then
            MessageBox.Show("Invalid CompanyGroup From - Does not Exists", "DOES NOT TRANSFER...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If txt_DbIdNo_From.Visible And txt_DbIdNo_From.Enabled Then txt_DbIdNo_From.Focus()
            btn_Transfer.Enabled = True
            Exit Sub
        End If


        DbFrm_ConnStr = Common_Procedures.Create_Sql_ConnectionString(DbFrmName)
        CnFrm = New SqlClient.SqlConnection(DbFrm_ConnStr)




        CnFrm.Open()


        tr = CnTo.BeginTransaction

        'Try

        Transfer_Table(tr, DbFrmName, "Company_Head")

        Transfer_Table(tr, DbFrmName, "AccountsGroup_Head")

        Transfer_Table(tr, DbFrmName, "Area_Head")

        Transfer_Table(tr, DbFrmName, "Ledger_Head")
        Transfer_Table(tr, DbFrmName, "Ledger_AlaisHead")
        Transfer_Table(tr, DbFrmName, "Ledger_ItemName_Details")

        Transfer_Table(tr, DbFrmName, "ClothType_Head")

        Transfer_Table(tr, DbFrmName, "Count_Head")
        Transfer_Table(tr, DbFrmName, "Count_Jari_Consumption_Details")

        Transfer_Table(tr, DbFrmName, "EndsCount_Head")

        Transfer_Table(tr, DbFrmName, "Loom_Head")
        Transfer_Table(tr, DbFrmName, "LoomType_Head")

        Transfer_Table(tr, DbFrmName, "Mill_Head")
        Transfer_Table(tr, DbFrmName, "Mill_Count_Details")
        Transfer_Table(tr, DbFrmName, "Month_Head")

        Transfer_Table(tr, DbFrmName, "Cloth_Head")
        Transfer_Table(tr, DbFrmName, "Cloth_EndsCount_Details")
        Transfer_Table(tr, DbFrmName, "Cloth_Bobin_Details")
        Transfer_Table(tr, DbFrmName, "Cloth_Kuri_Details")

        Transfer_Table(tr, DbFrmName, "Cheque_Print_Positioning_Head")

        Transfer_Table(tr, DbFrmName, "Mail_Settings_Head")

        '--------------

        Transfer_Table(tr, DbFrmName, "Article_Head")
        Transfer_Table(tr, DbFrmName, "Beam_Width_Head")
        Transfer_Table(tr, DbFrmName, "BorderSize_Head")
        Transfer_Table(tr, DbFrmName, "Brand_Head")
        Transfer_Table(tr, DbFrmName, "Colour_Head")
        Transfer_Table(tr, DbFrmName, "Currency_Head")
        Transfer_Table(tr, DbFrmName, "Department_Head")
        Transfer_Table(tr, DbFrmName, "Employee_Head")
        Transfer_Table(tr, DbFrmName, "Item_AlaisHead")
        Transfer_Table(tr, DbFrmName, "ItemGroup_Head")
        Transfer_Table(tr, DbFrmName, "Machine_Head")
        Transfer_Table(tr, DbFrmName, "Packing_Type_Head")
        Transfer_Table(tr, DbFrmName, "PayRoll_Category_Details")
        Transfer_Table(tr, DbFrmName, "PayRoll_Category_Head")
        Transfer_Table(tr, DbFrmName, "PayRoll_Employee_Head")
        Transfer_Table(tr, DbFrmName, "PayRoll_Employee_Payment_Head")
        Transfer_Table(tr, DbFrmName, "PayRoll_Employee_Releave_Details")
        Transfer_Table(tr, DbFrmName, "PayRoll_Employee_Salary_Details")
        Transfer_Table(tr, DbFrmName, "PayRoll_Salary_Payment_Type_Head")
        Transfer_Table(tr, DbFrmName, "Process_Head")
        Transfer_Table(tr, DbFrmName, "Processed_Item_Head")
        Transfer_Table(tr, DbFrmName, "Processed_Item_Details")
        Transfer_Table(tr, DbFrmName, "Processed_Item_SalesName_Head")
        Transfer_Table(tr, DbFrmName, "Processed_Item_SalesName_Details")
        Transfer_Table(tr, DbFrmName, "Rack_Head")
        Transfer_Table(tr, DbFrmName, "ReedWidth_Head")
        Transfer_Table(tr, DbFrmName, "Shift_Head")
        Transfer_Table(tr, DbFrmName, "Size_Head")
        Transfer_Table(tr, DbFrmName, "Stores_Item_AlaisHead")
        Transfer_Table(tr, DbFrmName, "Stores_Item_Details")
        Transfer_Table(tr, DbFrmName, "Stores_Item_Head")
        Transfer_Table(tr, DbFrmName, "Transport_Head")
        Transfer_Table(tr, DbFrmName, "Unit_Head")
        Transfer_Table(tr, DbFrmName, "Variety_Head")
        Transfer_Table(tr, DbFrmName, "Vehicle_Head")
        Transfer_Table(tr, DbFrmName, "Working_Type_Head")
        Transfer_Table(tr, DbFrmName, "YarnType_Head")


        '---Ledger_Opening_Transfer(tr)

        tr.Commit()

        Me.Text = "MASTERS TRANSFER"

        'MDIParent1.vFldsChk_All_Status = True
        Me.Text = "Fields Check - 1-1"
        MDIParent1.mnu_Tools_FieldsCheck_1_Click(sender, e)
        Me.Text = "Fields Check - 2-1"
        MDIParent1.mnu_Tools_FieldsCheck_2_Click(sender, e)
        Me.Text = "Fields Check - 1-2"
        MDIParent1.mnu_Tools_FieldsCheck_1_Click(sender, e)
        Me.Text = "Fields Check - 2-2"
        MDIParent1.mnu_Tools_FieldsCheck_2_Click(sender, e)
        'MDIParent1.vFldsChk_All_Status = False

        Me.Text = "MASTERS TRANSFER"

        MDIParent1.Cursor = Cursors.Default
        Me.Cursor = Cursors.Default

        MessageBox.Show("All Masters Transfered Sucessfully", "FOR MASTERS TRANSFER...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        btn_Transfer.Enabled = True
        btn_Transfer_Textile.Enabled = True
        btn_Transfer_Sizing.Enabled = True
        btn_Transfer_OE.Enabled = True

        'Catch ex As Exception

        '    tr.Rollback()
        '    Me.Text = "MASTERS TRANSFER"
        '    MDIParent1.Cursor = Cursors.Default
        '    Me.Cursor = Cursors.Default
        '    btn_Transfer.Enabled = True
        '    MessageBox.Show(ex.Message, "INVALID TRANSFER...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        'Finally

        '    CnFrm.Close()
        '    CnTo.Close()
        '    tr.Dispose()

        '    btn_Transfer.Enabled = True
        '    Me.Text = "MASTERS TRANSFER"

        '    MDIParent1.Cursor = Cursors.Default
        '    Me.Cursor = Cursors.Default

        'End Try

    End Sub

    Private Sub Transfer_Table(ByVal sqltr As SqlClient.SqlTransaction, ByVal DbFrmName As String, ByVal TblName As String)
        Dim CmdTo As New SqlClient.SqlCommand

        Me.Text = TblName

        CmdTo.Connection = CnTo
        CmdTo.Transaction = sqltr

        CmdTo.CommandText = "Drop table " & TblName
        CmdTo.ExecuteNonQuery()

        CmdTo.CommandText = "Select * into " & Trim(Common_Procedures.DataBaseName) & ".." & TblName & " from " & Trim(DbFrmName) & ".." & TblName
        CmdTo.ExecuteNonQuery()

    End Sub

    Private Sub Ledger_Opening_Transfer(ByVal sqltr As SqlClient.SqlTransaction)
        Dim CmdFrm As New SqlClient.SqlCommand
        Dim CmdTo As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Cmp_FromDt As Date, OpDt As Date
        Dim OpDateCondt As String
        Dim OpBal As Single = 0
        Dim I As Integer, J As Integer
        Dim Sno As Integer = 0
        Dim CompIdNo As Integer = 0
        Dim LedIdNo As Integer = 0
        Dim L_Id As Integer = 0
        Dim NewCode As String = ""
        Dim OpYrCode As String = ""
        Dim Pk_Condition As String = ""
        Dim vou_bil_no As String
        Dim vou_bil_code As String
        Dim vAgt_ID As Integer
        Dim Bl_Amt As Single, Cr_Amt As Single, Dr_Amt As Single

        Me.Text = "Ledger Opening"
        Pk_Condition = "OPENI-"

        CmdFrm.Connection = CnFrm

        CmdTo.Connection = CnTo
        CmdTo.Transaction = sqltr

        CmdTo.CommandText = "delete from voucher_details where Entry_Identification LIKE '" & Trim(Pk_Condition) & "%'"
        CmdTo.ExecuteNonQuery()

        CmdTo.CommandText = "delete from voucher_bill_head where Entry_Identification LIKE '" & Trim(Pk_Condition) & "%'"
        CmdTo.ExecuteNonQuery()

        Cmp_FromDt = #4/1/2015#
        OpDt = #3/31/2015#

        OpYrCode = "14-15"

        Da1 = New SqlClient.SqlDataAdapter("select a.*, b.* from Ledger_Head a, Company_Head b where Ledger_Idno <> 0 and Company_IdNo <> 0 Order by Ledger_Idno, Company_IdNo", CnFrm)
        Dt1 = New DataTable
        Da1.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then

            For I = 0 To Dt1.Rows.Count - 1

                CmdFrm.Parameters.Clear()
                CmdFrm.Parameters.AddWithValue("@CompFromDate", Cmp_FromDt)
                CmdFrm.Parameters.AddWithValue("@OpeningDate", OpDt)

                CmdTo.Parameters.Clear()
                CmdTo.Parameters.AddWithValue("@CompFromDate", Cmp_FromDt)
                CmdTo.Parameters.AddWithValue("@OpeningDate", OpDt)


                CompIdNo = Val(Dt1.Rows(I).Item("Company_IdNo").ToString)
                LedIdNo = Val(Dt1.Rows(I).Item("Ledger_IdNo").ToString)

                If Val(LedIdNo) <= 20 Then
                    L_Id = LedIdNo

                Else
                    L_Id = LedIdNo + 80

                End If

                Me.Text = "Ledger Opening  -  " & LedIdNo

                NewCode = Trim(Val(CompIdNo)) & "-" & Trim(Val(L_Id)) & "/" & Trim(OpYrCode)

                OpDateCondt = ""
                If Trim(Dt1.Rows(I).Item("Parent_Code").ToString) Like "*~18~" Then
                    OpDateCondt = " a.Voucher_Date >= @CompFromDate"
                End If

                CmdFrm.CommandText = "Select sum(a.voucher_amount) from voucher_details a, company_head tz where " & OpDateCondt & IIf(OpDateCondt <> "", " and ", "") & " a.company_idno = " & Str(Val(CompIdNo)) & " and a.ledger_idno = " & Str(Val(LedIdNo)) & " and a.company_idno = tz.company_idno"
                Da1 = New SqlClient.SqlDataAdapter(CmdFrm)
                Dt2 = New DataTable
                Da1.Fill(Dt2)

                OpBal = 0
                If Dt2.Rows.Count > 0 Then
                    If IsDBNull(Dt2.Rows(0)(0).ToString) = False Then
                        OpBal = Val(Dt2.Rows(0)(0).ToString)
                    End If
                End If
                Dt2.Clear()

                'CmdTo.CommandText = "Delete from Voucher_Details Where Company_IdNo = " & Str(Val(CompIdNo)) & " and Ledger_IdNo = " & Str(Val(LedIdNo)) & " and Entry_Identification LIKE '" & Trim(Pk_Condition) & "%'"
                'CmdTo.ExecuteNonQuery()

                Sno = 0

                If Val(OpBal) <> 0 Then

                    Sno = Sno + 1


                    CmdTo.CommandText = "Insert into Voucher_Details (        Voucher_Code    ,       For_OrderByCode ,         Company_IdNo      ,           Voucher_No     ,         For_OrderBy   , Voucher_Type, Voucher_Date,         Sl_No        ,           Ledger_IdNo ,    Voucher_Amount      , Narration,             Year_For_Report                               ,           Entry_Identification               ) " & _
                                        "            Values          ( '" & Trim(NewCode) & "', " & Str(Val(L_Id)) & ", " & Str(Val(CompIdNo)) & ", '" & Trim(Val(L_Id)) & "', " & Str(Val(L_Id)) & ",    'Opening', @OpeningDate, " & Str(Val(Sno)) & ", " & Str(Val(L_Id)) & ", " & Str(Val(OpBal)) & ", 'Opening', " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(Pk_Condition) & Trim(NewCode) & "' ) "
                    CmdTo.ExecuteNonQuery()


                    If Trim(UCase(Dt1.Rows(I).Item("Bill_Maintenance").ToString)) = "BILL TO BILL CLEAR" Or Trim(UCase(Dt1.Rows(I).Item("Bill_Maintenance").ToString)) = "BILL TO BILL" Then

                        'CmdFrm.CommandText = "truncate table " & Trim(Common_Procedures.ReportTempSubTable) & ""
                        'CmdFrm.ExecuteNonQuery()

                        'CmdFrm.CommandText = "insert into " & Trim(Common_Procedures.ReportTempSubTable) & " ( int1, int2, name1, currency1 ) Select tZ.company_idno, tP.ledger_idno, a.Voucher_Bill_No, sum(a.Amount) from voucher_bill_details a INNER JOIN company_head tZ ON a.company_idno <> 0 and a.company_idno = tZ.company_idno INNER JOIN Ledger_Head tP ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = tP.Ledger_IdNo Where a.company_idno = " & Str(Val(CompIdNo)) & " and a.ledger_idno = " & Str(Val(LedIdNo)) & " group by tZ.company_idno, tP.ledger_idno, a.Voucher_Bill_No"
                        'CmdFrm.ExecuteNonQuery()

                        'CmdFrm.CommandText = "insert into " & Trim(Common_Procedures.ReportTempSubTable) & " ( int1, int2, name1, currency1 ) Select tZ.company_idno, tP.ledger_idno, a.Voucher_Bill_No, 0 from voucher_bill_head a INNER JOIN company_head tz ON a.company_idno <> 0 and a.company_idno = tZ.company_idno INNER JOIN Ledger_Head tP ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = tP.Ledger_IdNo where a.company_idno = " & Str(Val(CompIdNo)) & " and a.ledger_idno = " & Str(Val(LedIdNo)) & " group by tZ.company_idno, tP.ledger_idno, a.Voucher_Bill_No"
                        'CmdFrm.ExecuteNonQuery()

                        'CmdFrm.CommandText = "truncate table Entry_Temp"
                        'CmdFrm.ExecuteNonQuery()

                        'CmdFrm.CommandText = "insert into Entry_Temp ( SmallInt_1, SmallInt_2, Text_1, Amount_1 ) Select int1, int2, name1, sum(currency1) from " & Trim(Common_Procedures.ReportTempSubTable) & " group by int1, int2, name1"
                        'CmdFrm.ExecuteNonQuery()

                        'CmdFrm.CommandText = "truncate table " & Trim(Common_Procedures.ReportTempTable) & ""
                        'CmdFrm.ExecuteNonQuery()

                        'CmdFrm.CommandText = "insert into " & Trim(Common_Procedures.ReportTempTable) & " ( Int1,   Int2       ,   Int3      ,   name3        ,   Date1            ,                     Amount_1                                                                                                              ,                                          currency2                                                                                         ,       currency3                                                                            ,   name4      ) " & _
                        '                        " Select     a.Company_Idno, a.Ledger_Idno, a.Agent_Idno, a.party_bill_no, a.voucher_bill_date, (case when lower(a.crdr_type) = 'cr' then a.bill_amount else (case when b.Amount_1 is null then 0 else b.Amount_1 end ) end) as cr_amount, (case when lower(a.crdr_type) = 'dr' then a.bill_amount else (case when b.Amount_1 is null then 0 else b.Amount_1 end ) end) as db_amount, abs(a.bill_amount - (case when b.Amount_1 is null then 0 else b.Amount_1 end)) as balance, a.crdr_type from voucher_bill_head a, Entry_Temp b, company_head tz, ledger_head tp Where a.company_idno = " & Str(Val(CompIdNo)) & " and a.ledger_idno = " & Str(Val(LedIdNo)) & " and (a.bill_amount- (case when b.Amount_1 is null then 0 else b.Amount_1 end)) <> 0 and a.Voucher_Bill_No = b.text_1 and a.company_idno = b.SmallInt_1 and a.ledger_idno = tP.ledger_idno and a.company_idno = tZ.company_idno order by a.voucher_bill_date, a.For_OrderBy, a.Voucher_Bill_No"
                        'CmdFrm.ExecuteNonQuery()

                        Da1 = New SqlClient.SqlDataAdapter("Select a.* from voucher_bill_head a, company_head tz, ledger_head tp Where a.company_idno = " & Str(Val(CompIdNo)) & " and a.ledger_idno = " & Str(Val(LedIdNo)) & " and a.bill_amount <>  0 AND a.Credit_Amount <> a.Debit_Amount and a.ledger_idno = tP.ledger_idno and a.company_idno = tZ.company_idno order by a.voucher_bill_date, a.For_OrderBy, a.Voucher_Bill_No", CnFrm)
                        'Da1 = New SqlClient.SqlDataAdapter("Select a.Company_Idno, a.Ledger_Idno, a.Agent_Idno, a.party_bill_no, a.voucher_bill_date, (case when lower(a.crdr_type) = 'cr' then a.bill_amount else (case when b.Amount_1 is null then 0 else b.Amount_1 end ) end) as cr_amount, (case when lower(a.crdr_type) = 'dr' then a.bill_amount else (case when b.Amount_1 is null then 0 else b.Amount_1 end ) end) as db_amount, abs(a.bill_amount - (case when b.Amount_1 is null then 0 else b.Amount_1 end)) as balance, a.crdr_type from voucher_bill_head a, Entry_Temp b, company_head tz, ledger_head tp Where a.company_idno = " & Str(Val(CompIdNo)) & " and a.ledger_idno = " & Str(Val(LedIdNo)) & " and (a.bill_amount- (case when b.Amount_1 is null then 0 else b.Amount_1 end)) <> 0 and a.Voucher_Bill_No = b.text_1 and a.company_idno = b.SmallInt_1 and a.ledger_idno = tP.ledger_idno and a.company_idno = tZ.company_idno order by a.voucher_bill_date, a.For_OrderBy, a.Voucher_Bill_No", CnFrm)
                        'Da1 = New SqlClient.SqlDataAdapter("select name1, name2, name3, Date1, currency1, currency2, currency3, name4, int6, int7 from " & Trim(Common_Procedures.ReportTempTable) & " Order by name2, date1, name3, name1", CnFrm)
                        Dt2 = New DataTable
                        Da1.Fill(Dt2)

                        If Dt2.Rows.Count > 0 Then

                            For J = 0 To Dt2.Rows.Count - 1

                                CmdTo.Parameters.Clear()
                                CmdTo.Parameters.AddWithValue("@VouBillDate", CDate(Dt2.Rows(J).Item("Voucher_Bill_Date").ToString))

                                vAgt_ID = Val(Dt2.Rows(J).Item("Agent_Idno").ToString)
                                If Val(vAgt_ID) > 20 Then
                                    vAgt_ID = vAgt_ID + 80
                                End If

                                Bl_Amt = Math.Abs(Val(Dt2.Rows(J).Item("Credit_Amount").ToString) - Val(Dt2.Rows(J).Item("Debit_Amount").ToString))
                                Cr_Amt = 0
                                Dr_Amt = 0
                                If Trim(UCase(Dt2.Rows(J).Item("CrDr_Type").ToString)) = "CR" Then
                                    Cr_Amt = Bl_Amt
                                Else
                                    Dr_Amt = Bl_Amt
                                End If

                                vou_bil_no = Common_Procedures.get_MaxCode(CnTo, "Voucher_Bill_Head", "Voucher_Bill_Code", "For_OrderBy", "", Val(CompIdNo), OpYrCode, sqltr)
                                vou_bil_code = Trim(Val(CompIdNo)) & "-" & Trim(vou_bil_no) & "/" & Trim(OpYrCode)

                                CmdTo.CommandText = "Insert into voucher_bill_head ( voucher_bill_code ,         company_idno      ,        voucher_bill_no    ,            for_orderby      , voucher_bill_date,         ledger_idno   ,                             party_bill_no                 ,            agent_idno    ,         bill_amount     ,         Credit_Amount   ,         Debit_Amount    ,                                   crdr_type                  ,        entry_identification                  ) " _
                                                        & "      Values  ( '" & Trim(vou_bil_code) & "', " & Str(Val(CompIdNo)) & ", '" & Trim(vou_bil_no) & "', " & Str(Val(vou_bil_no)) & ",     @VouBillDate , " & Str(Val(L_Id)) & ", '" & Trim(Dt2.Rows(J).Item("Party_Bill_No").ToString) & "', " & Str(Val(vAgt_ID)) & ", " & Str(Val(Bl_Amt)) & ", " & Str(Val(Cr_Amt)) & ", " & Str(Val(Dr_Amt)) & ", '" & Trim(UCase(Dt2.Rows(J).Item("CrDr_Type").ToString)) & "', '" & Trim(Pk_Condition) & Trim(NewCode) & "' )"
                                CmdTo.ExecuteNonQuery()

                            Next

                        End If

                    End If

                End If

            Next I

        End If

        Me.Text = ""

    End Sub

    Private Sub txt_DbIdNo_From_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_DbIdNo_From.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub btn_Transfer_Textile_Click(sender As Object, e As EventArgs) Handles btn_Transfer_Textile.Click
        btn_Transfer_Click(sender, e)
    End Sub

    Private Sub btn_Transfer_Sizing_Click(sender As Object, e As EventArgs) Handles btn_Transfer_Sizing.Click
        Dim tr As SqlClient.SqlTransaction
        Dim da2 As SqlClient.SqlDataAdapter
        Dim dt2 As DataTable
        Dim DbFrmName As String = ""
        Dim DbFrm_ConnStr As String = ""
        Dim Nr As Long = 0

        If Trim(cbo_DBFrom_Sizing.Text) = "" Then
            MessageBox.Show("Invalid SIZING Database From", "DOES NOT TRANSFER...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_DBFrom_Sizing.Visible And cbo_DBFrom_Sizing.Enabled Then cbo_DBFrom_Sizing.Focus()
            Exit Sub
        End If

        If MessageBox.Show("All the master datas will lost " & Chr(13) & "Are you sure you want to Transfer?", "FOR MASTER TRANSFER...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

        MDIParent1.Cursor = Cursors.WaitCursor
        Me.Cursor = Cursors.WaitCursor


        CnTo.Open()

        DbFrmName = ""
        If Trim(cbo_DBFrom_Sizing.Text) <> "" Then
            DbFrmName = Trim(cbo_DBFrom_Sizing.Text)
        End If


        da2 = New SqlClient.SqlDataAdapter("Select name from master..sysdatabases where name = '" & Trim(DbFrmName) & "'", CnTo)
        dt2 = New DataTable
        da2.Fill(dt2)
        Nr = 0
        If dt2.Rows.Count > 0 Then
            If IsDBNull(dt2.Rows(0).Item("name").ToString) = False Then
                Nr = 1
            End If
        End If
        dt2.Dispose()
        da2.Dispose()

        If Nr = 0 Then
            MessageBox.Show("Invalid Sizing Database From - Does not Exists", "DOES NOT TRANSFER...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_DBFrom_Sizing.Visible And cbo_DBFrom_Sizing.Enabled Then cbo_DBFrom_Sizing.Focus()
            btn_Transfer_Sizing.Enabled = True
            Exit Sub
        End If


        DbFrm_ConnStr = Common_Procedures.Create_Sql_ConnectionString(DbFrmName)
        CnFrm = New SqlClient.SqlConnection(DbFrm_ConnStr)


        CnFrm.Open()

        btn_Transfer.Enabled = False
        btn_Transfer_Textile.Enabled = False
        Me.Text = ""


        tr = CnTo.BeginTransaction

        'Try


        'Sizing_AccountsGroupHead_Transfer(tr)

        'Sizing_AreaHead_Transfer(tr)

        'Sizing_LedgerHead_Transfer(tr)

        'Sizing_CountHead_Transfer(tr)

        'Sizing_MillHead_Transfer(tr)

        'Sizing_UnitHead_Transfer(tr)

        'Sizing_ItemGroupHead_Transfer(tr)

        Sizing_ItemHead_Transfer(tr)

        tr.Commit()

        Me.Text = "SIZING MASTERS TRANSFER"


        MDIParent1.Cursor = Cursors.Default
        Me.Cursor = Cursors.Default

        MessageBox.Show("All Masters Transfered Sucessfully", "FOR MASTERS TRANSFER...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        btn_Transfer.Enabled = True
        btn_Transfer_Textile.Enabled = True
        btn_Transfer_Sizing.Enabled = True
        btn_Transfer_OE.Enabled = True

        'Catch ex As Exception

        '    tr.Rollback()
        '    Me.Text = "MASTERS TRANSFER"
        '    MDIParent1.Cursor = Cursors.Default
        '    Me.Cursor = Cursors.Default
        '    btn_Transfer.Enabled = True
        '    MessageBox.Show(ex.Message, "INVALID TRANSFER...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        'Finally

        '    CnFrm.Close()
        '    CnTo.Close()
        '    tr.Dispose()

        '    btn_Transfer.Enabled = True
        '    Me.Text = "MASTERS TRANSFER"

        '    MDIParent1.Cursor = Cursors.Default
        '    Me.Cursor = Cursors.Default

        'End Try

    End Sub

    Private Sub Fields_Check_Sizing()
        Dim cmd As New SqlClient.SqlCommand

        On Error Resume Next

        cmd.Connection = CnFrm

        cmd.CommandText = "ALTER TABLE AccountsGroup_Head ADD AccountsGroup_NewIdNo int DEFAULT 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "UPDATE AccountsGroup_Head SET AccountsGroup_NewIdNo = 0 WHERE AccountsGroup_NewIdNo IS NULL"
        cmd.ExecuteNonQuery()

        cmd.Dispose()

    End Sub


    Private Sub Sizing_AccountsGroupHead_Transfer(ByVal sqltr As SqlClient.SqlTransaction)
        Dim CmdFrm As New SqlClient.SqlCommand
        Dim CmdTo As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim vAccountsGroup_IdNo As Integer, vOldLID As Integer
        Dim vSur_Name As String
        Dim vAccountsGroup_Name As String
        Dim vSurNm As String = ""
        Dim vParent_Name As String, vParent_Idno As String
        Dim vCarried_Balance As Integer, vIndicate As Integer, vOrder_Position As Single
        Dim vTallyName As String, vTallySubName As String
        Dim AccGrpAr() As String
        Dim Inc As Integer
        Dim AccGrp1 As String = ""
        Dim AccGrp2 As String = ""
        Dim AccGrp3 As String = ""
        Dim AccGrp4 As String = ""

        Me.Text = "Group_Head"

        CmdFrm.Connection = CnFrm
        CmdTo.Connection = CnTo
        CmdTo.Transaction = sqltr


        Fields_Check_Sizing()

        'CmdTo.CommandText = "Delete from AccountsGroup_Head where AccountsGroup_IdNo > 32 "
        'CmdTo.ExecuteNonQuery()

        Da1 = New SqlClient.SqlDataAdapter("Select * from AccountsGroup_Head where AccountsGroup_IdNo > 32 Order by AccountsGroup_IdNo", CnFrm)
        Dt1 = New DataTable
        Da1.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then

            For I = 0 To Dt1.Rows.Count - 1

                Me.Text = "Group_Head  -  " & I

                vSurNm = Common_Procedures.Remove_NonCharacters(Dt1.Rows(I).Item("AccountsGroup_Name").ToString)
                vOldLID = Val(Common_Procedures.get_FieldValue(CnTo, "AccountsGroup_Head", "AccountsGroup_IdNo", "(sur_name = '" & Trim(vSurNm) & "')", , sqltr))

                If vOldLID = 0 Then

                    vAccountsGroup_IdNo = Common_Procedures.get_MaxIdNo(CnTo, "AccountsGroup_Head", "AccountsGroup_IdNo", "", sqltr)
                    'vAccountsGroup_IdNo = Val(Dt1.Rows(I).Item("AccountsGroup_Idno").ToString)
                    'If Val(vAccountsGroup_IdNo) > 30 Then
                    '    vAccountsGroup_IdNo = vAccountsGroup_IdNo + 2
                    'End If
                    '***************************************************************************

                    CmdFrm.CommandText = "Update AccountsGroup_Head set AccountsGroup_NewIdNo = " & Str(Val(vAccountsGroup_IdNo)) & " Where AccountsGroup_IdNo = " & Str(Val(Dt1.Rows(I).Item("AccountsGroup_IdNo").ToString))
                    CmdFrm.ExecuteNonQuery()


                    vAccountsGroup_Name = Dt1.Rows(I).Item("AccountsGroup_Name").ToString

                    vSur_Name = Dt1.Rows(I).Item("Sur_Name").ToString

                    vParent_Name = Replace(Dt1.Rows(I).Item("Parent_Name").ToString, "'", "")

                    vParent_Idno = Replace(Dt1.Rows(I).Item("Parent_Idno").ToString, "'", "")

                    Erase AccGrpAr
                    AccGrpAr = Split(Trim(vParent_Idno), "~")
                    Inc = 0

                    Inc = Inc + 1
                    If UBound(AccGrpAr) >= Inc Then AccGrp1 = Trim(AccGrpAr(Inc))
                    If Val(AccGrp1) > 32 Then
                        AccGrp1 = Val(Common_Procedures.get_FieldValue(CnFrm, "AccountsGroup_Head", "AccountsGroup_NewIdNo", "(AccountsGroup_IdNo = " & Str(Val(AccGrp1)) & ")"))
                    End If

                    Inc = Inc + 1
                    If UBound(AccGrpAr) >= Inc Then AccGrp2 = Trim(AccGrpAr(Inc))
                    If Val(AccGrp2) > 32 Then
                        AccGrp2 = Val(Common_Procedures.get_FieldValue(CnFrm, "AccountsGroup_Head", "AccountsGroup_NewIdNo", "(AccountsGroup_IdNo = " & Str(Val(AccGrp2)) & ")"))
                    End If

                    Inc = Inc + 1
                    If UBound(AccGrpAr) >= Inc Then AccGrp3 = Trim(AccGrpAr(Inc))
                    If Val(AccGrp3) > 32 Then
                        AccGrp3 = Val(Common_Procedures.get_FieldValue(CnFrm, "AccountsGroup_Head", "AccountsGroup_NewIdNo", "(AccountsGroup_IdNo = " & Str(Val(AccGrp3)) & ")"))
                    End If

                    Inc = Inc + 1
                    If UBound(AccGrpAr) >= Inc Then AccGrp4 = Trim(AccGrpAr(Inc))
                    If Val(AccGrp4) > 32 Then
                        AccGrp4 = Val(Common_Procedures.get_FieldValue(CnFrm, "AccountsGroup_Head", "AccountsGroup_NewIdNo", "(AccountsGroup_IdNo = " & Str(Val(AccGrp4)) & ")"))
                    End If


                    If Trim(AccGrp4) <> "" Then
                        vParent_Idno = "~" & AccGrp1 & "~" & AccGrp2 & "~" & AccGrp3 & "~" & AccGrp4 & "~"
                    ElseIf Trim(AccGrp3) <> "" Then
                        vParent_Idno = "~" & AccGrp1 & "~" & AccGrp2 & "~" & AccGrp3 & "~"
                    ElseIf Trim(AccGrp2) <> "" Then
                        vParent_Idno = "~" & AccGrp1 & "~" & AccGrp2 & "~"
                    Else
                        vParent_Idno = "~" & AccGrp1 & "~"
                    End If

                    vCarried_Balance = Val(Dt1.Rows(I).Item("Carried_Balance").ToString)

                    vOrder_Position = Val(Dt1.Rows(I).Item("Order_Position").ToString)

                    vTallyName = Replace(Dt1.Rows(I).Item("TallyName").ToString, "'", "")

                    vTallySubName = Replace(Dt1.Rows(I).Item("TallySubName").ToString, "'", "")

                    vIndicate = Val(Dt1.Rows(I).Item("Indicate").ToString)

                    CmdTo.CommandText = "Insert into AccountsGroup_Head ( AccountsGroup_IdNo ,            AccountsGroup_Name      ,            Sur_Name      ,          Parent_Name        ,            Parent_Idno      ,              Carried_Balance      ,                Order_Position      ,            TallyName      ,            TallySubName      ,              Indicate       ) " &
                                        "       Values (" & Str(Val(vAccountsGroup_IdNo)) & ", '" & Trim(vAccountsGroup_Name) & "', '" & Trim(vSur_Name) & "', '" & Trim(vParent_Name) & "', '" & Trim(vParent_Idno) & "', " & Str(Val(vCarried_Balance)) & ",   " & Str(Val(vOrder_Position)) & ", '" & Trim(vTallyName) & "', '" & Trim(vTallySubName) & "', " & Str(Val(vIndicate)) & " ) "
                    CmdTo.ExecuteNonQuery()

                End If

            Next

        End If

        Me.Text = ""

    End Sub

    Private Sub Sizing_AreaHead_Transfer(ByVal sqltr As SqlClient.SqlTransaction)
        Dim CmdFrm As New SqlClient.SqlCommand
        Dim CmdTo As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim vArea_IdNo As Integer, vOldLID As Integer
        Dim vArea_Name As String, vSur_Name As String
        Dim vSurNm As String

        Me.Text = "Area_Head"

        CmdFrm.Connection = CnFrm
        CmdTo.Connection = CnTo
        CmdTo.Transaction = sqltr

        'CmdTo.CommandText = "Delete from Area_Head"
        'CmdTo.ExecuteNonQuery()

        Da1 = New SqlClient.SqlDataAdapter("select distinct(AREA_NAME) from Area_Head Where AREA_NAME <> '' Order by AREA_NAME", CnFrm)
        Dt1 = New DataTable
        Da1.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then

            For I = 0 To Dt1.Rows.Count - 1

                Me.Text = "Area_Head  -  " & I

                If Trim(Dt1.Rows(I).Item("Area_Name").ToString) <> "" Then

                    vSurNm = Common_Procedures.Remove_NonCharacters(Dt1.Rows(I).Item("Area_Name").ToString)

                    vOldLID = Val(Common_Procedures.get_FieldValue(CnTo, "Area_Head", "Area_IdNo", "(Sur_Name = '" & Trim(vSurNm) & "')", , sqltr))

                    If vOldLID = 0 Then

                        vArea_IdNo = Val(Common_Procedures.get_MaxIdNo(CnTo, "Area_Head", "Area_IdNo", ""))
                        'vArea_IdNo = Val(I) + 1

                        vArea_Name = Replace(Dt1.Rows(I).Item("Area_Name").ToString, "'", "")

                        vSur_Name = Common_Procedures.Remove_NonCharacters(vArea_Name)

                        CmdTo.CommandText = "Insert into Area_Head ( Area_Idno        ,            Area_Name      ,            Sur_Name   ) " &
                                            "       Values (" & Str(Val(vArea_IdNo)) & ", '" & Trim(vArea_Name) & "', '" & Trim(vSur_Name) & "') "
                        CmdTo.ExecuteNonQuery()

                    End If

                End If

            Next

        End If

        Me.Text = ""

    End Sub

    Private Sub Sizing_LedgerHead_Transfer(ByVal sqltr As SqlClient.SqlTransaction)
        Dim CmdFrm As New SqlClient.SqlCommand
        Dim CmdTo As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim vLedger_IdNo As Integer, vOldLID As Integer
        Dim vLedger_Name As String, vSur_Name As String, vLedger_MainName As String, vLedger_AlaisName As String
        Dim vArea_IdNo As Integer, vAccountsGroup_IdNo As String, vParent_Code As String, vBill_Type As String
        Dim vLedger_Address1 As String, vLedger_Address2 As String, vLedger_Address3 As String, vLedger_Address4 As String, vLedger_PhoneNo As String
        Dim vLedger_TinNo As String, vLedger_CstNo As String, vLedger_Type As String, vPan_No As String
        Dim vLedger_Emailid As String, vLedger_FaxNo As String, vLedger_MobileNo As String, vContact_Person As String
        Dim vPackingType_CompanyIdNo As Integer, vLedger_AgentIdNo As Integer, vNote As String = "", vLedgerGroup_Idno As Integer, vLedgerState_Idno As Integer
        Dim vMobileNo_Sms As String, vBilling_Type As String, vSticker_Type As String, vMrp_Perc As String
        Dim vSurNm As String
        Dim vShow_In_All_Entry As Integer, vVerified_Status As Integer
        Dim vTransport_IdNo As Integer, vNoOf_Looms As Integer
        Dim vFreight_Loom As Single
        Dim vOwn_Loom_Status As Integer
        Dim vTds_Percentage As Single
        Dim vOwner_Name As String
        Dim vPartner_Proprietor As String
        Dim vCloth_Comm_Meter As Single, vCloth_Comm_Percentage As Single
        Dim vYarn_Comm_Bag As Single, vYarn_Comm_Percentage As Single
        Dim AccGrpAr() As String
        Dim Inc As Integer
        Dim AccGrp1 As String = ""
        Dim AccGrp2 As String = ""
        Dim AccGrp3 As String = ""
        Dim AccGrp4 As String = ""
        Dim vAcgrp_NewID As Integer = 0
        Dim vLedger_GsTinNo As String
        Dim vLegal_Nameof_Business As String = "", vCity_Town As String = "", vPincode As String = "", vDistance As String = ""
        Dim vAcgrp_Name As String = ""


        Me.Text = "Ledger_Head"

        CmdFrm.Connection = CnFrm
        CmdTo.Connection = CnTo
        CmdTo.Transaction = sqltr

        'CmdTo.CommandText = "Delete from Ledger_Head where Ledger_IdNo > 100"
        'CmdTo.ExecuteNonQuery()

        Da1 = New SqlClient.SqlDataAdapter("select a.* from Ledger_Head a where a.Ledger_IdNo > 100 Order by a.Ledger_IdNo", CnFrm)
        'Da1 = New SqlClient.SqlDataAdapter("select a.*, b.state_name from Ledger_Head a left outer join state_head b ON b.state_idno = a.State_IdNo where Ledger_IdNo > 100 Order by Ledger_IdNo", CnFrm)
        Dt1 = New DataTable
        Da1.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then

            For I = 0 To Dt1.Rows.Count - 1

                Me.Text = "Ledger_Head  -  " & I

                vSurNm = Common_Procedures.Remove_NonCharacters(Dt1.Rows(I).Item("Ledger_Name").ToString)
                vOldLID = Val(Common_Procedures.get_FieldValue(CnTo, "Ledger_Head", "Ledger_IdNo", "(Sur_Name = '" & Trim(vSurNm) & "')", , sqltr))

                If vOldLID = 0 Then

                    vLedger_IdNo = Val(Common_Procedures.get_MaxIdNo(CnTo, "ledger_head", "ledger_idno", "", sqltr))
                    If Val(vLedger_IdNo) < 101 Then vLedger_IdNo = 101
                    'vLedger_IdNo = Val(Dt1.Rows(I).Item("Ledger_IdNo").ToString)

                    vLedger_Name = Replace(Dt1.Rows(I).Item("Ledger_Name").ToString, "'", "")
                    vLedger_MainName = Replace(Dt1.Rows(I).Item("Ledger_MainName").ToString, "'", "")
                    vSur_Name = Replace(Dt1.Rows(I).Item("Sur_Name").ToString, "'", "")
                    vLedger_AlaisName = Replace(Dt1.Rows(I).Item("Ledger_AlaisName").ToString, "'", "")

                    'vArea_IdNo = 0
                    'If Trim(Dt1.Rows(I).Item("Area_Name").ToString) <> "" Then
                    '    vArea_IdNo = Val(Common_Procedures.get_FieldValue(CnTo, "Area_Head", "Area_IdNo", "(Area_Name = '" & Trim(Dt1.Rows(I).Item("Area_Name").ToString) & "')", , sqltr))
                    'End If

                    vArea_IdNo = Val(Dt1.Rows(I).Item("Area_IdNo").ToString)


                    If vAcgrp_NewID > 32 Then
                        vAcgrp_Name = Trim(Common_Procedures.get_FieldValue(CnFrm, "AccountsGroup_Head", "AccountsGroup_Name", "(Parent_IdNo = '" & Trim(Dt1.Rows(I).Item("Parent_Code").ToString) & "')"))
                        vAccountsGroup_IdNo = Trim(Common_Procedures.get_FieldValue(CnTo, "AccountsGroup_Head", "Parent_IdNo", "(AccountsGroup_Name = '" & Trim(vAcgrp_Name) & "')", , sqltr))
                        vParent_Code = Trim(Common_Procedures.get_FieldValue(CnTo, "AccountsGroup_Head", "Parent_IdNo", "(AccountsGroup_IdNo = " & Str(Val(vAccountsGroup_IdNo)) & ")", , sqltr))
                        'vAccountsGroup_IdNo = vAcgrp_NewID ' Common_Procedures.get_FieldValue(CnTo, "AccountsGroup_Head", "AccountsGroup_IdNo", "(Parent_Idno = '" & Trim(vParent_Code) & "')", , sqltr)

                    Else
                        vParent_Code = Dt1.Rows(I).Item("Parent_Code").ToString
                        vAccountsGroup_IdNo = Common_Procedures.get_FieldValue(CnTo, "AccountsGroup_Head", "AccountsGroup_IdNo", "(Parent_Idno = '" & Trim(vParent_Code) & "')", , sqltr)

                    End If

                    If InStr(1, Trim(UCase(Dt1.Rows(I).Item("Bill_Type").ToString)), "BILL") > 0 Then
                        vBill_Type = "BILL TO BILL"
                    Else
                        vBill_Type = "BALANCE ONLY"
                    End If

                    vLedger_Address1 = Replace(Dt1.Rows(I).Item("Ledger_Address1").ToString, "'", "")
                    vLedger_Address2 = Replace(Dt1.Rows(I).Item("Ledger_Address2").ToString, "'", "")
                    vLedger_Address3 = Replace(Dt1.Rows(I).Item("Ledger_Address3").ToString, "'", "")
                    vLedger_Address4 = Replace(Dt1.Rows(I).Item("Ledger_Address4").ToString, "'", "")
                    vLedger_PhoneNo = Replace(Dt1.Rows(I).Item("Ledger_PhoneNo").ToString, "'", "")
                    vLedger_TinNo = Replace(Dt1.Rows(I).Item("Ledger_TinNo").ToString, "'", "")
                    vLedger_CstNo = Dt1.Rows(I).Item("Ledger_CstNo").ToString
                    vLedger_Type = Dt1.Rows(I).Item("Ledger_Type").ToString
                    vPan_No = "" ' Dt1.Rows(I).Item("Pan_No").ToString
                    vPartner_Proprietor = "" ' Replace(Dt1.Rows(I).Item("Proprietor_Partner").ToString, "'", "")
                    vYarn_Comm_Percentage = 0  'Dt1.Rows(I).Item("Yarn_Commission_Percentage").ToString
                    vYarn_Comm_Bag = 0  ' Dt1.Rows(I).Item("Commission_Bag").ToString
                    vCloth_Comm_Percentage = 0  ' Dt1.Rows(I).Item("Commission_Percentage").ToString
                    vCloth_Comm_Meter = 0  ' Dt1.Rows(I).Item("Cloth_Commission_Meter").ToString

                    vLedger_Emailid = Replace(Dt1.Rows(I).Item("Ledger_Mail").ToString, "'", "")
                    vLedger_FaxNo = "" 'Dt1.Rows(I).Item("Fax_No").ToString
                    vLedger_MobileNo = "" 'Dt1.Rows(I).Item("Ledger_MobileNo").ToString
                    vContact_Person = "" 'Replace(Dt1.Rows(I).Item("Contact_Person").ToString, "'", "")
                    vPackingType_CompanyIdNo = 0
                    vLedger_AgentIdNo = 0 ' Val(Dt1.Rows(I).Item("Agent_IdNo").ToString)

                    'vNote = Replace(Dt1.Rows(I).Item("Note1").ToString, "'", "")

                    vMobileNo_Sms = Dt1.Rows(I).Item("Ledger_MobileNo").ToString
                    vOwner_Name = "" ' Replace(Dt1.Rows(I).Item("Owner_Name").ToString, "'", "")

                    vLedger_GsTinNo = Replace(Dt1.Rows(I).Item("Ledger_GSTinNo").ToString, "'", "")
                    vLedgerGroup_Idno = 0 ' Val(Dt1.Rows(I).Item("LedgerGroup_Idno").ToString)
                    vLedgerState_Idno = Val(Dt1.Rows(I).Item("ledger_State_IdNo").ToString)



                    vLegal_Nameof_Business = Dt1.Rows(I).Item("Legal_Nameof_Business").ToString
                    vCity_Town = Dt1.Rows(I).Item("City_Town").ToString
                    vPincode = Dt1.Rows(I).Item("Pincode").ToString
                    vDistance = Val(Dt1.Rows(I).Item("Distance").ToString)



                    vTds_Percentage = 0  'Val(Dt1.Rows(I).Item("Tds_Percentage").ToString)
                    vOwn_Loom_Status = 0 'Val(Dt1.Rows(I).Item("Own_Loom").ToString)
                    vFreight_Loom = 0 'Val(Dt1.Rows(I).Item("Freight_Loom").ToString)
                    vNoOf_Looms = 0 ' Val(Dt1.Rows(I).Item("Noof_Looms").ToString)
                    vTransport_IdNo = 0
                    vVerified_Status = 1

                    vShow_In_All_Entry = 0
                    If Trim(UCase(Dt1.Rows(I).Item("Ledger_Type").ToString)) = "ALL" Then
                        vShow_In_All_Entry = 1
                    End If

                    vBilling_Type = ""
                    vSticker_Type = ""
                    vMrp_Perc = ""

                    If Val(vLedger_IdNo) > 100 Then

                        CmdTo.CommandText = "Insert into ledger_head ( Ledger_IdNo    ,            Ledger_Name      ,            Sur_Name      ,            Ledger_MainName      ,            Ledger_AlaisName      ,              Area_IdNo      ,              AccountsGroup_IdNo      ,            Parent_Code      ,            Bill_Type      ,            Ledger_Address1      ,            Ledger_Address2      ,            Ledger_Address3      ,            Ledger_Address4      ,            Ledger_PhoneNo      ,            Ledger_TinNo      ,            Ledger_CstNo      ,            Ledger_Type      ,            Pan_No      ,            Partner_Proprietor      ,              Yarn_Comm_Percentage      ,              Yarn_Comm_Bag      ,              Cloth_Comm_Percentage      ,            Ledger_Emailid      ,            Ledger_FaxNo      ,            Ledger_MobileNo  ,            MobileNo_Frsms    ,            MobileNo_Sms      ,            Contact_Person      ,             PackingType_CompanyIdNo       ,              Ledger_AgentIdNo      ,            Note      ,            Show_In_All_Entry        ,            Billing_Type      ,            Sticker_Type      ,            Mrp_Perc      ,              Own_Loom_Status      ,              Freight_Loom      ,              NoOf_Looms      ,              Transport_IdNo      , Verified_Status,            Owner_Name      ,              Tds_Percentage           ,           Ledger_GSTinNo       ,                LedgerGroup_Idno    ,             Ledger_State_IdNo      ,          Legal_Nameof_Business        ,            City_Town      ,            Pincode      ,            Distance       ) " &
                                        "       Values (" & Str(Val(vLedger_IdNo)) & ", '" & Trim(vLedger_Name) & "', '" & Trim(vSur_Name) & "', '" & Trim(vLedger_MainName) & "', '" & Trim(vLedger_AlaisName) & "', " & Str(Val(vArea_IdNo)) & ", " & Str(Val(vAccountsGroup_IdNo)) & ", '" & Trim(vParent_Code) & "', '" & Trim(vBill_Type) & "', '" & Trim(vLedger_Address1) & "', '" & Trim(vLedger_Address2) & "', '" & Trim(vLedger_Address3) & "', '" & Trim(vLedger_Address4) & "', '" & Trim(vLedger_PhoneNo) & "', '" & Trim(vLedger_TinNo) & "', '" & Trim(vLedger_CstNo) & "', '" & Trim(vLedger_Type) & "', '" & Trim(vPan_No) & "', '" & Trim(vPartner_Proprietor) & "', " & Str(Val(vYarn_Comm_Percentage)) & ", " & Str(Val(vYarn_Comm_Bag)) & ", " & Str(Val(vCloth_Comm_Percentage)) & ", '" & Trim(vLedger_Emailid) & "', '" & Trim(vLedger_FaxNo) & "', '" & Trim(vLedger_MobileNo) & "', '" & Trim(vMobileNo_Sms) & "', '" & Trim(vMobileNo_Sms) & "', '" & Trim(vContact_Person) & "', " & Str(Val(vPackingType_CompanyIdNo)) & ", " & Str(Val(vLedger_AgentIdNo)) & ", '" & Trim(vNote) & "', " & Str(Val(vShow_In_All_Entry)) & ", '" & Trim(vBilling_Type) & "', '" & Trim(vSticker_Type) & "', '" & Trim(vMrp_Perc) & "', " & Str(Val(vOwn_Loom_Status)) & ", " & Str(Val(vFreight_Loom)) & ", " & Str(Val(vNoOf_Looms)) & ", " & Str(Val(vTransport_IdNo)) & ",       1        , '" & Trim(vOwner_Name) & "', " & Str(Val(vTds_Percentage)) & " , '" & Trim(vLedger_GsTinNo) & "', " & Str(Val(vLedgerGroup_Idno)) & ", " & Str(Val(vLedgerState_Idno)) & ", '" & Trim(vLegal_Nameof_Business) & "', '" & Trim(vCity_Town) & "', '" & Trim(vPincode) & "', '" & Trim(vDistance) & "' ) "
                        CmdTo.ExecuteNonQuery()

                    End If

                End If

            Next

            CmdTo.CommandText = "truncate table Ledger_AlaisHead"
            CmdTo.ExecuteNonQuery()

            Da1 = New SqlClient.SqlDataAdapter("select * from Ledger_Head", CnTo)
            Da1.SelectCommand.Transaction = sqltr
            Dt1 = New DataTable
            Da1.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1
                    CmdTo.CommandText = "Insert into Ledger_AlaisHead (                              Ledger_IdNo                 , Sl_No,                       Ledger_DisplayName                ,                            Ledger_Type                  ,                                AccountsGroup_IdNo                 ,                                  Own_Loom_Status               ,                            Show_In_All_Entry                     ,                               Verified_Status                  ,                              Area_IdNo                   ,                                Close_status                 ,                              Stock_Maintenance_Status                    ) " &
                                        "            Values           (" & Str(Val(Dt1.Rows(i).Item("Ledger_IdNo").ToString)) & ",    1 , '" & Trim(Dt1.Rows(i).Item("Ledger_Name").ToString) & "', '" & Trim(Dt1.Rows(i).Item("Ledger_Type").ToString) & "',  " & Str(Val(Dt1.Rows(i).Item("AccountsGroup_IdNo").ToString)) & ",  " & Str(Val(Dt1.Rows(i).Item("Own_Loom_Status").ToString)) & ",  " & Str(Val(Dt1.Rows(i).Item("Show_In_All_Entry").ToString)) & ",  " & Str(Val(Dt1.Rows(i).Item("Verified_Status").ToString)) & ",  " & Str(Val(Dt1.Rows(i).Item("Area_IdNo").ToString)) & ",  " & Str(Val(Dt1.Rows(i).Item("Close_status").ToString)) & ",  " & Str(Val(Dt1.Rows(i).Item("Stock_Maintenance_Status").ToString)) & " ) "
                    CmdTo.ExecuteNonQuery()


                    If Trim(Dt1.Rows(i).Item("Ledger_AlaisName").ToString) <> "" Then
                        CmdTo.CommandText = "Insert into Ledger_AlaisHead (                              Ledger_IdNo                 , Sl_No,                       Ledger_DisplayName                     ,                            Ledger_Type                  ,                                AccountsGroup_IdNo                 ,                                  Own_Loom_Status               ,                            Show_In_All_Entry                     ,                               Verified_Status                  ,                              Area_IdNo                   ,                                Close_status                 ,                              Stock_Maintenance_Status                    ) " &
                                            "            Values           (" & Str(Val(Dt1.Rows(i).Item("Ledger_IdNo").ToString)) & ",    2 , '" & Trim(Dt1.Rows(i).Item("Ledger_AlaisName").ToString) & "', '" & Trim(Dt1.Rows(i).Item("Ledger_Type").ToString) & "',  " & Str(Val(Dt1.Rows(i).Item("AccountsGroup_IdNo").ToString)) & ",  " & Str(Val(Dt1.Rows(i).Item("Own_Loom_Status").ToString)) & ",  " & Str(Val(Dt1.Rows(i).Item("Show_In_All_Entry").ToString)) & ",  " & Str(Val(Dt1.Rows(i).Item("Verified_Status").ToString)) & ",  " & Str(Val(Dt1.Rows(i).Item("Area_IdNo").ToString)) & ",  " & Str(Val(Dt1.Rows(i).Item("Close_status").ToString)) & ",  " & Str(Val(Dt1.Rows(i).Item("Stock_Maintenance_Status").ToString)) & " ) "
                        CmdTo.ExecuteNonQuery()

                    End If

                Next

            End If

            CmdTo.Dispose()
            Dt1.Dispose()
            Da1.Dispose()

        End If

        Me.Text = ""

    End Sub

    Private Sub Sizing_CountHead_Transfer(ByVal sqltr As SqlClient.SqlTransaction)
        Dim CmdFrm As New SqlClient.SqlCommand
        Dim CmdTo As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim vCount_IdNo As Integer, vOldLID As Integer
        Dim vSur_Name As String
        Dim vCount_Name As String
        Dim vResultant_Count As Single
        Dim vCount_StockUnder_IdNo As Integer
        Dim vCount_Description As String = ""
        Dim vSurNm As String = ""
        Dim vItemGrp_IDno As Integer = 0
        Dim vHSNCode As String = 0
        Dim vGstPerc As String = 0

        Me.Text = "Count_Head"

        CmdFrm.Connection = CnFrm
        CmdTo.Connection = CnTo
        CmdTo.Transaction = sqltr

        'CmdTo.CommandText = "Delete from Count_Head"
        'CmdTo.ExecuteNonQuery()

        Da1 = New SqlClient.SqlDataAdapter("select * from Count_Head Order by Count_IdNo", CnFrm)
        Dt1 = New DataTable
        Da1.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then

            For I = 0 To Dt1.Rows.Count - 1

                Me.Text = "Count_Head  -  " & I

                vSurNm = Common_Procedures.Remove_NonCharacters(Dt1.Rows(I).Item("Count_Name").ToString)
                vOldLID = Val(Common_Procedures.get_FieldValue(CnTo, "Count_Head", "Count_IdNo", "(Sur_Name = '" & Trim(vSurNm) & "')", , sqltr))

                If vOldLID = 0 Then

                    vCount_IdNo = Common_Procedures.get_MaxIdNo(CnTo, "Count_Head", "Count_IdNo", "", sqltr)
                    'vCount_IdNo = Val(Dt1.Rows(I).Item("Count_IdNo").ToString)

                    vCount_Name = Replace(Dt1.Rows(I).Item("Count_Name").ToString, "'", "")

                    vSur_Name = Common_Procedures.Remove_NonCharacters(vCount_Name)

                    vCount_StockUnder_IdNo = vCount_IdNo

                    vItemGrp_IDno = 0

                    If InStr(1, Trim(UCase(CnFrm.Database)), "_OE_") > 0 Then

                        vHSNCode = Dt1.Rows(I).Item("HSN_Code").ToString
                        vGstPerc = Dt1.Rows(I).Item("GST_Percentege").ToString


                    Else

                        vHSNCode = Dt1.Rows(I).Item("Count_Hsn_Code").ToString
                        vGstPerc = Dt1.Rows(I).Item("Count_Gst_Perc").ToString


                    End If



                    If Trim(vHSNCode) <> "" Or Val(vGstPerc) <> 0 Then

                        If Trim(vHSNCode) <> "" And Val(vGstPerc) <> 0 Then
                            Da = New SqlClient.SqlDataAdapter("select a.* from itemgroup_head a Where a.Item_HSN_Code = '" & Trim(vHSNCode) & "' and Item_GST_Percentage = " & Str(Val(vGstPerc)) & " ", CnTo)
                            Da.SelectCommand.Transaction = sqltr
                            Dt2 = New DataTable
                            Da.Fill(Dt2)
                            If Dt2.Rows.Count > 0 Then
                                vItemGrp_IDno = Val(Dt2.Rows(0)("itemgroup_idno").ToString)
                            End If
                            Dt2.Clear()

                        ElseIf Val(vGstPerc) <> 0 Then

                            Da = New SqlClient.SqlDataAdapter("select a.* from itemgroup_head a Where a.Item_GST_Percentage = " & Str(Val(vGstPerc)) & " ", CnTo)
                            Da.SelectCommand.Transaction = sqltr
                            Dt2 = New DataTable
                            Da.Fill(Dt2)
                            If Dt2.Rows.Count > 0 Then
                                vItemGrp_IDno = Val(Dt2.Rows(0)("itemgroup_idno").ToString)
                            End If
                            Dt2.Clear()

                        End If


                    End If

                    CmdTo.CommandText = "Insert into Count_Head (               Count_IdNo  ,              Count_Name    ,      Sur_Name            ,             Count_Description     ,          Count_StockUnder_IdNo     ,          Resultant_Count     , Rate_Kg , Cotton_Polyester_Jari , Transfer_To_CountIdNo,       ItemGroup_Idno     ,  Sizing_To_CountIdNo ,         HSN_Code        ,        GST_Percentege      ) " &
                                        " values              (" & Str(Val(vCount_IdNo)) & ", '" & Trim(vCount_Name) & "', '" & Trim(vSur_Name) & "', '" & Trim(vCount_Description) & "', " & Val(vCount_StockUnder_IdNo) & ", " & Val(vResultant_Count) & ",    0    ,      'COTTON'         ,         0            , " & Val(vItemGrp_IDno) & ",              0      , '" & Trim(vHSNCode) & "', " & Str(Val(vGstPerc)) & " ) "
                    CmdTo.ExecuteNonQuery()

                    'CmdTo.CommandText = "Insert into Count_Head ( Count_IdNo         ,            Count_Name      ,            Sur_Name      ,          Count_Description        ,              Count_StockUnder_IdNo      ,                Resultant_Count      , Cotton_Polyester_Jari ) " &
                    '                    "       Values (" & Str(Val(vCount_IdNo)) & ", '" & Trim(vCount_Name) & "', '" & Trim(vSur_Name) & "', '" & Trim(vCount_Description) & "', " & Str(Val(vCount_StockUnder_IdNo)) & ",   " & Str(Val(vResultant_Count)) & ",          'COTTON'     ) "
                    'CmdTo.ExecuteNonQuery()

                End If

            Next

        End If

        Me.Text = ""

    End Sub

    Private Sub Sizing_MillHead_Transfer(ByVal sqltr As SqlClient.SqlTransaction)
        Dim CmdFrm As New SqlClient.SqlCommand
        Dim CmdTo As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim vMill_IdNo As Integer, vOldLID As Integer
        Dim vMill_Name As String, vSur_Name As String
        Dim vSurNm As String
        Dim vCount_IdNo As Integer
        Dim vWeight_Bag As Single, vCones_Bag As Single, vWeight_Cone As Single
        Dim vRate_Kg As Single, vRate_Thiri As Single

        Me.Text = "Mill_Head"

        CmdFrm.Connection = CnFrm
        CmdTo.Connection = CnTo
        CmdTo.Transaction = sqltr

        'CmdTo.CommandText = "Delete from Mill_Head"
        'CmdTo.ExecuteNonQuery()

        'CmdTo.CommandText = "Delete from Mill_Count_Details"
        'CmdTo.ExecuteNonQuery()

        Da1 = New SqlClient.SqlDataAdapter("select * from Mill_Head Order by Mill_IdNo", CnFrm)
        Dt1 = New DataTable
        Da1.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then

            For I = 0 To Dt1.Rows.Count - 1

                Me.Text = "Mill_Head  -  " & I

                vSurNm = Common_Procedures.Remove_NonCharacters(Dt1.Rows(I).Item("Mill_Name").ToString)
                vOldLID = Val(Common_Procedures.get_FieldValue(CnTo, "Mill_Head", "Mill_IdNo", "(Sur_Name = '" & Trim(vSurNm) & "')", , sqltr))

                If vOldLID = 0 Then

                    vMill_IdNo = Common_Procedures.get_MaxIdNo(CnTo, "Mill_Head", "Mill_IdNo", "", sqltr)
                    'vMill_IdNo = Val(Dt1.Rows(I).Item("Mill_IdNo").ToString)

                    vMill_Name = Replace(Dt1.Rows(I).Item("Mill_Name").ToString, "'", "")

                    vSur_Name = Common_Procedures.Remove_NonCharacters(vMill_Name)

                    CmdTo.CommandText = "Insert into Mill_Head ( Mill_IdNo          ,            Mill_Name      ,            Sur_Name      , Weight_EmptyBag, Weight_EmptyCone ) " &
                                        "       Values (" & Str(Val(vMill_IdNo)) & ", '" & Trim(vMill_Name) & "', '" & Trim(vSur_Name) & "',        0       ,         0        ) "
                    CmdTo.ExecuteNonQuery()

                    Da1 = New SqlClient.SqlDataAdapter("select * from Mill_Count_Details where mill_idno = " & Str(Val(Dt1.Rows(I).Item("Mill_IdNo").ToString)) & " Order by Sl_No", CnFrm)
                    Dt2 = New DataTable
                    Da1.Fill(Dt2)

                    If Dt2.Rows.Count > 0 Then

                        For J = 0 To Dt2.Rows.Count - 1

                            vCount_IdNo = Val(Dt2.Rows(J).Item("count_idno").ToString)

                            vWeight_Bag = Val(Dt2.Rows(J).Item("Weight_Bag").ToString)
                            vCones_Bag = Val(Dt2.Rows(J).Item("Cones_Bag").ToString)
                            vWeight_Cone = Val(Dt2.Rows(J).Item("Weight_Cone").ToString)
                            vRate_Kg = 0 'Val(Dt2.Rows(J).Item("Rate_Kg").ToString)
                            vRate_Thiri = 0 'Val(Dt2.Rows(J).Item("Rate_Thiri").ToString)

                            CmdTo.CommandText = "Insert into Mill_Count_Details ( Mill_IdNo ,            Sl_No       ,            Count_IdNo        ,              Weight_Bag      ,              Cones_Bag      ,              Weight_Cone      ,              Rate_Kg      ,              Rate_Thiri       ) " &
                                                "       Values (" & Str(Val(vMill_IdNo)) & ", " & Str(Val(J + 1)) & ", " & Str(Val(vCount_IdNo)) & ", " & Str(Val(vWeight_Bag)) & ", " & Str(Val(vCones_Bag)) & ", " & Str(Val(vWeight_Cone)) & ", " & Str(Val(vRate_Kg)) & ", " & Str(Val(vRate_Thiri)) & " ) "
                            CmdTo.ExecuteNonQuery()

                        Next

                    End If
                    Dt2.Clear()

                End If

            Next

        End If

        Me.Text = ""

    End Sub

    Private Sub Sizing_UnitHead_Transfer(ByVal sqltr As SqlClient.SqlTransaction)
        Dim CmdFrm As New SqlClient.SqlCommand
        Dim CmdTo As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim vUnit_IdNo As Integer, vOldLID As Integer
        Dim vUnit_Name As String, vSur_Name As String
        Dim vSurNm As String
        Dim vItem_HSN_Code As String = ""
        Dim vItem_GST_Percentage As String = ""


        Me.Text = "Unit_Head"


        CmdFrm.Connection = CnFrm
        CmdTo.Connection = CnTo
        CmdTo.Transaction = sqltr

        'CmdTo.CommandText = "Delete from ItemGroup_Head"
        'CmdTo.ExecuteNonQuery()

        Da1 = New SqlClient.SqlDataAdapter("select * from Unit_Head Order by Unit_idno", CnFrm)
        Dt1 = New DataTable
        Da1.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then

            For I = 0 To Dt1.Rows.Count - 1


                Me.Text = "Unit_Head  -  " & I


                vSurNm = Common_Procedures.Remove_NonCharacters(Dt1.Rows(I).Item("Unit_name").ToString)

                vOldLID = Val(Common_Procedures.get_FieldValue(CnTo, "Unit_Head", "Unit_idno", "(Sur_Name = '" & Trim(vSurNm) & "')", , sqltr))

                If vOldLID = 0 Then

                    vUnit_IdNo = Common_Procedures.get_MaxIdNo(CnTo, "Unit_Head", "Unit_idno", "", sqltr)
                    'vUnit_IdNo = Val(I) + 1

                    vUnit_Name = Replace(Dt1.Rows(I).Item("unit_name").ToString, "'", "")

                    vSur_Name = Common_Procedures.Remove_NonCharacters(vUnit_Name)


                    CmdTo.CommandText = "Insert into Unit_Head (    Unit_idno         ,         Unit_Name         ,      Sur_Name            ) " &
                                                " Values (" & Str(Val(vUnit_IdNo)) & ", '" & Trim(vUnit_Name) & "', '" & Trim(vSur_Name) & "'  ) "
                    CmdTo.ExecuteNonQuery()


                End If



            Next

        End If

        Me.Text = ""

    End Sub


    Private Sub Sizing_ItemGroupHead_Transfer(ByVal sqltr As SqlClient.SqlTransaction)
        Dim CmdFrm As New SqlClient.SqlCommand
        Dim CmdTo As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim vItemGroup_IdNo As Integer, vOldLID As Integer
        Dim vItemGroup_Name As String, vSur_Name As String
        Dim vSurNm As String
        Dim vItem_HSN_Code As String = ""
        Dim vItem_GST_Percentage As String = ""


        Me.Text = "ItemGroup_Head"


        CmdFrm.Connection = CnFrm
        CmdTo.Connection = CnTo
        CmdTo.Transaction = sqltr

        'CmdTo.CommandText = "Delete from ItemGroup_Head"
        'CmdTo.ExecuteNonQuery()

        Da1 = New SqlClient.SqlDataAdapter("select * from ItemGroup_Head Order by itemgroup_idno", CnFrm)
        Dt1 = New DataTable
        Da1.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then

            For I = 0 To Dt1.Rows.Count - 1

                Me.Text = "ItemGroup_Head  -  " & I

                'If Trim(Dt1.Rows(I).Item("Item_Type").ToString) <> "" Then

                vSurNm = Common_Procedures.Remove_NonCharacters(Dt1.Rows(I).Item("itemgroup_name").ToString)

                vOldLID = Val(Common_Procedures.get_FieldValue(CnTo, "ItemGroup_Head", "ItemGroup_IdNo", "(Sur_Name = '" & Trim(vSurNm) & "')", , sqltr))

                If vOldLID = 0 Then

                    vItemGroup_IdNo = Common_Procedures.get_MaxIdNo(CnTo, "ItemGroup_Head", "itemgroup_idno", "", sqltr)
                    'vItemGroup_IdNo = Val(I) + 1

                    vItemGroup_Name = Replace(Dt1.Rows(I).Item("itemgroup_name").ToString, "'", "")

                    vSur_Name = Common_Procedures.Remove_NonCharacters(vItemGroup_Name)

                    vItem_HSN_Code = Replace(Dt1.Rows(I).Item("itemgroup_name").ToString, "'", "")

                    vItem_GST_Percentage = Val(Dt1.Rows(I).Item("itemgroup_name").ToString)


                    CmdTo.CommandText = "Insert into ItemGroup_Head (  itemgroup_idno      ,         itemgroup_name         ,      Item_HSN_Code            ,       sur_name           ,        Item_GST_Percentage             ) " &
                                                " Values (" & Str(Val(vItemGroup_IdNo)) & ", '" & Trim(vItemGroup_Name) & "', '" & Trim(vItem_HSN_Code) & "', '" & Trim(vSur_Name) & "', " & Str(Val(vItem_GST_Percentage)) & " ) "
                    CmdTo.ExecuteNonQuery()


                End If


                'End If

            Next

        End If

        Me.Text = ""

    End Sub
    Private Sub Sizing_ItemHead_Transfer(ByVal sqltr As SqlClient.SqlTransaction)
        Dim CmdFrm As New SqlClient.SqlCommand
        Dim CmdTo As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim vItem_IdNo As Integer, vOldLID As Integer
        Dim vSur_Name As String
        Dim vItem_Name As String
        Dim vCount_StockUnder_IdNo As Integer
        Dim vCount_Description As String = ""
        Dim vSurNm As String = ""
        Dim vItemGroup_IdNo As Integer = 0
        Dim vItem_Code As String = ""
        Dim vItemGrp_IdNo As Integer = 0
        Dim vUnit_IdNo As Integer = 0

        Dim vMinimum_Stock As String = 0
        Dim vTax_Percentage As String = 0
        Dim vCost_Rate As String = 0
        Dim vSales_Rate As String = 0
        Dim vSale_TaxRate As String = 0
        Dim vGST_Percentage As String = 0
        Dim vHSN_Code As String = 0

        Me.Text = "Item_Head"

        CmdFrm.Connection = CnFrm
        CmdTo.Connection = CnTo
        CmdTo.Transaction = sqltr

        CmdTo.CommandText = "Delete from Sizing_Item_Head"
        CmdTo.ExecuteNonQuery()

        Da1 = New SqlClient.SqlDataAdapter("select a.*, b.ItemGroup_Name, c.Unit_Name from Item_Head a LEFT OUTER JOIN ItemGroup_Head b ON a.ItemGroup_IdNo = b.ItemGroup_IdNo LEFT OUTER JOIN Unit_Head c ON a.Unit_IdNo = c.Unit_IdNo Order by a.Item_IdNo", CnFrm)
        Dt1 = New DataTable
        Da1.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then

            For I = 0 To Dt1.Rows.Count - 1

                Me.Text = "Item_Head  -  " & I

                vSurNm = Common_Procedures.Remove_NonCharacters(Dt1.Rows(I).Item("Item_Name").ToString)
                vOldLID = Val(Common_Procedures.get_FieldValue(CnTo, "Item_Head", "Item_IdNo", "(Sur_Name = '" & Trim(vSurNm) & "')", , sqltr))

                If vOldLID = 0 Then

                    vItem_IdNo = Common_Procedures.get_MaxIdNo(CnTo, "Sizing_Item_Head", "Item_IdNo", "", sqltr)
                    'vItem_IdNo = Val(Dt1.Rows(I).Item("Item_IdNo").ToString)

                    vItem_Name = Replace(Dt1.Rows(I).Item("Item_Name").ToString, "'", "")

                    vSur_Name = Common_Procedures.Remove_NonCharacters(vItem_Name)

                    vCount_StockUnder_IdNo = vItem_IdNo

                    vItemGroup_IdNo = Val(Common_Procedures.get_FieldValue(CnTo, "ItemGroup_Head", "ItemGroup_IdNo", "(ItemGroup_Name = '" & Trim(Dt1.Rows(I).Item("ItemGroup_Name").ToString) & "')", , sqltr))

                    vItem_Code = Replace(Dt1.Rows(I).Item("Item_Code").ToString, "'", "")

                    vUnit_IdNo = Val(Common_Procedures.get_FieldValue(CnTo, "Unit_Head", "Unit_IdNo", "(Unit_Name = '" & Trim(Dt1.Rows(I).Item("Unit_Name").ToString) & "')", , sqltr))

                    vMinimum_Stock = Dt1.Rows(I).Item("Minimum_Stock").ToString
                    vTax_Percentage = Dt1.Rows(I).Item("Tax_Percentage").ToString
                    vCost_Rate = Dt1.Rows(I).Item("Cost_Rate").ToString
                    vSales_Rate = Dt1.Rows(I).Item("Sales_Rate").ToString
                    vSale_TaxRate = Dt1.Rows(I).Item("Sale_TaxRate").ToString
                    vGST_Percentage = Dt1.Rows(I).Item("GST_Percentage").ToString
                    vHSN_Code = Dt1.Rows(I).Item("HSN_Code").ToString

                    CmdTo.CommandText = "Insert into Sizing_Item_Head ( Item_IdNo   ,            Item_Name      ,            Sur_Name      ,          Item_Code         ,              ItemGroup_IdNo      ,             Unit_IdNo         ,               Tax_Percentage        ,              Sale_TaxRate       ,           Sales_Rate          ,              Cost_Rate       ,              Minimum_Stock       ,              GST_Percentage      ,            HSN_Code       ) " &
                                        "       Values (" & Str(Val(vItem_IdNo)) & ", '" & Trim(vItem_Name) & "', '" & Trim(vSur_Name) & "', '" & Trim(vItem_Code) & "', " & Str(Val(vItemGroup_IdNo)) & ",   " & Str(Val(vUnit_IdNo)) & " ,  " & Str(Val(vTax_Percentage)) & "  , " & Str(Val(vSale_TaxRate)) & " , " & Str(Val(vSales_Rate)) & " , " & Str(Val(vCost_Rate)) & " , " & Str(Val(vMinimum_Stock)) & " , " & Str(Val(vGST_Percentage)) & ", '" & Trim(vHSN_Code) & "' ) "
                    CmdTo.ExecuteNonQuery()

                End If

            Next

        End If

        Me.Text = ""

    End Sub

    Private Sub btn_Transfer_OE_Click(sender As Object, e As EventArgs) Handles btn_Transfer_OE.Click
        Dim tr As SqlClient.SqlTransaction
        Dim da2 As SqlClient.SqlDataAdapter
        Dim dt2 As DataTable
        Dim DbFrmName As String = ""
        Dim DbFrm_ConnStr As String = ""
        Dim Nr As Long = 0

        If Trim(cbo_DBFrom_OE.Text) = "" Then
            MessageBox.Show("Invalid OE Database From", "DOES NOT TRANSFER...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_DBFrom_OE.Visible And cbo_DBFrom_OE.Enabled Then cbo_DBFrom_OE.Focus()
            Exit Sub
        End If

        If MessageBox.Show("All the master datas will lost " & Chr(13) & "Are you sure you want to Transfer?", "FOR MASTER TRANSFER...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If



        CnTo.Open()

        DbFrmName = ""
        If Trim(cbo_DBFrom_OE.Text) <> "" Then
            DbFrmName = Trim(cbo_DBFrom_OE.Text)
        End If


        da2 = New SqlClient.SqlDataAdapter("Select name from master..sysdatabases where name = '" & Trim(DbFrmName) & "'", CnTo)
        dt2 = New DataTable
        da2.Fill(dt2)
        Nr = 0
        If dt2.Rows.Count > 0 Then
            If IsDBNull(dt2.Rows(0).Item("name").ToString) = False Then
                Nr = 1
            End If
        End If
        dt2.Dispose()
        da2.Dispose()

        If Nr = 0 Then
            MessageBox.Show("Invalid Sizing Database From - Does not Exists", "DOES NOT TRANSFER...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_DBFrom_OE.Visible And cbo_DBFrom_OE.Enabled Then cbo_DBFrom_OE.Focus()
            btn_Transfer_Sizing.Enabled = True
            Exit Sub
        End If


        DbFrm_ConnStr = Common_Procedures.Create_Sql_ConnectionString(DbFrmName)
        CnFrm = New SqlClient.SqlConnection(DbFrm_ConnStr)


        CnFrm.Open()

        MDIParent1.Cursor = Cursors.WaitCursor
        Me.Cursor = Cursors.WaitCursor


        btn_Transfer.Enabled = False
        btn_Transfer_Textile.Enabled = False
        Me.Text = ""


        tr = CnTo.BeginTransaction

        'Try


        'Sizing_AccountsGroupHead_Transfer(tr)

        'Sizing_AreaHead_Transfer(tr)

        'Sizing_LedgerHead_Transfer(tr)

        'Sizing_CountHead_Transfer(tr)

        OE_VarietyHead_Transfer(tr)

        tr.Commit()

        Me.Text = "SIZING MASTERS TRANSFER"


        MDIParent1.Cursor = Cursors.Default
        Me.Cursor = Cursors.Default

        MessageBox.Show("All Masters Transfered Sucessfully", "FOR MASTERS TRANSFER...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        btn_Transfer.Enabled = True
        btn_Transfer_Textile.Enabled = True
        btn_Transfer_Sizing.Enabled = True
        btn_Transfer_OE.Enabled = True

        'Catch ex As Exception

        '    tr.Rollback()
        '    Me.Text = "MASTERS TRANSFER"
        '    MDIParent1.Cursor = Cursors.Default
        '    Me.Cursor = Cursors.Default
        '    btn_Transfer.Enabled = True
        '    MessageBox.Show(ex.Message, "INVALID TRANSFER...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        'Finally

        '    CnFrm.Close()
        '    CnTo.Close()
        '    tr.Dispose()

        '    btn_Transfer.Enabled = True
        '    Me.Text = "MASTERS TRANSFER"

        '    MDIParent1.Cursor = Cursors.Default
        '    Me.Cursor = Cursors.Default

        'End Try

    End Sub

    Private Sub OE_VarietyHead_Transfer(ByVal sqltr As SqlClient.SqlTransaction)
        Dim CmdFrm As New SqlClient.SqlCommand
        Dim CmdTo As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim vVariety_IdNo As Integer, vOldLID As Integer
        Dim vVariety_Name As String, vSur_Name As String
        Dim vSurNm As String
        Dim vHSN_Code As String = ""
        Dim vGST_Percentege As String = ""


        Me.Text = "Unit_Head"


        CmdFrm.Connection = CnFrm
        CmdTo.Connection = CnTo
        CmdTo.Transaction = sqltr

        'CmdTo.CommandText = "Delete from ItemGroup_Head"
        'CmdTo.ExecuteNonQuery()

        Da1 = New SqlClient.SqlDataAdapter("select * from Variety_Head Order by Variety_IdNo", CnFrm)
        Dt1 = New DataTable
        Da1.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then

            For I = 0 To Dt1.Rows.Count - 1


                Me.Text = "Unit_Head  -  " & I


                vSurNm = Common_Procedures.Remove_NonCharacters(Dt1.Rows(I).Item("Variety_Name").ToString)

                vOldLID = Val(Common_Procedures.get_FieldValue(CnTo, "Variety_Head", "Variety_IdNo", "(Sur_Name = '" & Trim(vSurNm) & "')", , sqltr))

                If vOldLID = 0 Then

                    vVariety_IdNo = Common_Procedures.get_MaxIdNo(CnTo, "Variety_Head", "Variety_IdNo", "", sqltr)
                    If Val(vVariety_IdNo) < 11 Then vVariety_IdNo = 11

                    vVariety_Name = Replace(Dt1.Rows(I).Item("Variety_Name").ToString, "'", "")

                    vSur_Name = Common_Procedures.Remove_NonCharacters(vVariety_Name)

                    vHSN_Code = Replace(Dt1.Rows(I).Item("HSN_Code").ToString, "'", "")
                    vGST_Percentege = Val(Dt1.Rows(I).Item("GST_Percentege").ToString)

                    CmdTo.CommandText = "Insert into Variety_Head (    Variety_IdNo            ,         Variety_Name            ,      Sur_Name             ,         HSN_Code        ,           GST_Percentege        ) " &
                                                " Values (" & Str(Val(vVariety_IdNo)) & ", '" & Trim(vVariety_Name) & "', '" & Trim(vSur_Name) & "', '" & Trim(vHSN_Code) & "', " & Str(Val(vGST_Percentege)) & " ) "
                    CmdTo.ExecuteNonQuery()

                End If

            Next

        End If

        Me.Text = ""

    End Sub

    Private Sub btn_Import_From_Excel_Click(sender As Object, e As EventArgs) Handles btn_Import_From_Excel.Click
        Dim CmdTo As New SqlClient.SqlCommand
        Dim vLedger_IdNo As String

        Dim sqltr As SqlClient.SqlTransaction
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable

        Dim xlApp As Excel.Application
        Dim xlWorkBook As Excel.Workbook
        Dim xlWorkSheet As Excel.Worksheet

        Dim RowCnt As Long = 0
        Dim FileName As String = ""
        Dim I As Integer
        Dim vNEWID As Long
        Dim vDBNAMEFRM As String
        Dim vLEDNm As String, vSURNm As String
        Dim vOldLID As String

        Dim g As New Password
        g.ShowDialog()

        If Trim(UCase(Common_Procedures.Password_Input)) <> "TSIMP123" Then
            MessageBox.Show("Invalid Password", "PASSWORD FAILED...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        CnTo = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        CnTo.Open()

        CmdTo.Connection = CnTo

        sqltr = CnTo.BeginTransaction

        CmdTo.Transaction = sqltr

        'Try

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

            If RowCnt <= 1 Then
                MessageBox.Show("Data not found", "Data not found...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub
            End If

            vDBNAMEFRM = Common_Procedures.get_Company_DataBaseName(Trim(Val(txt_DbIdNo_From.Text)))

            CmdTo.CommandText = "delete from ledger_head where Ledger_IdNo > 100"
            CmdTo.ExecuteNonQuery()
            CmdTo.CommandText = "delete from Ledger_AlaisHead where Ledger_IdNo > 100"
            CmdTo.ExecuteNonQuery()

            For I = 1 To RowCnt

                vLedger_IdNo = xlWorkSheet.Cells(I, 1).value

                If Val(vLedger_IdNo) = 0 Then
                    Continue For
                End If



            vLEDNm = UCase(Trim(xlWorkSheet.Cells(I, 2).value))
            vSURNm = UCase(Trim(xlWorkSheet.Cells(I, 3).value))

            vOldLID = Common_Procedures.get_FieldValue(CnTo, "Ledger_Head", "Ledger_IdNo", "(Sur_Name = '" & Trim(vSURNm) & "')", , sqltr)

            If Val(vOldLID) <> 0 Then
                Continue For
            End If

            CmdTo.CommandText = "Insert into ledger_head ( Ledger_IdNo, Ledger_Name, Sur_Name, Ledger_MainName, Ledger_AlaisName, Area_IdNo, AccountsGroup_IdNo, Parent_Code, Bill_Type, Ledger_Address1, Ledger_Address2, Ledger_Address3, Ledger_Address4,
                         Ledger_PhoneNo, Ledger_TinNo, Ledger_CstNo, Ledger_Type, Pan_No, Partner_Proprietor, Yarn_Comm_Percentage, Yarn_Comm_Bag, Cloth_Comm_Percentage, Cloth_Comm_Meter, Ledger_Emailid, Ledger_FaxNo,
                         Ledger_MobileNo, Contact_Person, PackingType_CompanyIdNo, Ledger_AgentIdNo, Note, Show_In_All_Entry, MobileNo_Frsms, MobileNo_Sms, Billing_Type, Sticker_Type, Mrp_Perc, Ledger_Mail, Own_Loom_Status,
                         Freight_Loom, NoOf_Looms, Transport_IdNo, Verified_Status, Owner_Name, Tds_Percentage, Tds_Perc, Old_Ledger_IdNo, Close_status, Transfer_To_LedgerIdNo, Stock_Maintenance_Status, Tamil_Name,
                         Advance_deduction_amount, Ledger_GSTinNo, Ledger_State_IdNo, Ledger_RatePrReel, Ledger_MeterPrReel, Insurance_No, Ledger_WeightPrReel, Disc_Percentage, DueDate, LedgerAddress_Print_LeftMargin,
                         LedgerAddress_Print_TopMargin, LedgerAddress_Print_PaperOrientation, Freight_Pavu, Aadhar_No, Weaver_LoomType, Pan_Address1, Pan_Address2, Pan_Address3, Pan_Address4, Company_IdNo,
                         FromAddress_SetPosition_Sts, Paper_Orientation, FROMAddress_Topoint, TOAddress_Topoint, FROMAddress_LeftPoint, TOAddress_LeftPoint, Freight_Bundle, Pavu_Stock_Minimum_Level, Pavu_Stock_Maximum_Level,
                         Yarn_Stock_Maximum_Level, Yarn_Stock_Minimum_Level, Sizing_To_CompanyIdNo, Sizing_To_VendorIdNo, Ledger_Transfer_Neft, Ledger_bank_PartyName, Ledger_BankName, Ledger_BranchName, Ledger_AccountNo,
                         Ledger_IFSCCode, Remarks, Party_MailId2, Party_MobileNo2, Distance, LedgerGroup_Idno, Credit_Limit_Amount, Credit_Limit_Days, Production_Per_Day, Legal_Nameof_Business, City_Town, Pincode,
                         Ledger_GSTIN_Verified_Status, vehicle_no, Zone_IdNo, State_Idno, TCS_Sales_Status, WeavingBill_IR_Receipt_Meters_Sts, Ledger_Tamil_Address1, Ledger_Tamil_Address2, Ledgers_CompanyIdNo,
                         Contact_Designation_IdNo, Party_Category_IdNo, Ledger_ShortName, Marketting_Executive_IdNo, Transport_Name, PriceList_IdNo  ) " &
                                     "       SELECT  " &
                        " Ledger_IdNo, Ledger_Name, Sur_Name, Ledger_MainName, Ledger_AlaisName, Area_IdNo, AccountsGroup_IdNo, Parent_Code, Bill_Type, Ledger_Address1, Ledger_Address2, Ledger_Address3, Ledger_Address4,
                         Ledger_PhoneNo, Ledger_TinNo, Ledger_CstNo, Ledger_Type, Pan_No, Partner_Proprietor, Yarn_Comm_Percentage, Yarn_Comm_Bag, Cloth_Comm_Percentage, Cloth_Comm_Meter, Ledger_Emailid, Ledger_FaxNo,
                         Ledger_MobileNo, Contact_Person, PackingType_CompanyIdNo, Ledger_AgentIdNo, Note, Show_In_All_Entry, MobileNo_Frsms, MobileNo_Sms, Billing_Type, Sticker_Type, Mrp_Perc, Ledger_Mail, Own_Loom_Status,
                         Freight_Loom, NoOf_Looms, Transport_IdNo, Verified_Status, Owner_Name, Tds_Percentage, Tds_Perc, Old_Ledger_IdNo, Close_status, Transfer_To_LedgerIdNo, Stock_Maintenance_Status, Tamil_Name,
                         Advance_deduction_amount, Ledger_GSTinNo, Ledger_State_IdNo, Ledger_RatePrReel, Ledger_MeterPrReel, Insurance_No, Ledger_WeightPrReel, Disc_Percentage, DueDate, LedgerAddress_Print_LeftMargin,
                         LedgerAddress_Print_TopMargin, LedgerAddress_Print_PaperOrientation, Freight_Pavu, Aadhar_No, Weaver_LoomType, Pan_Address1, Pan_Address2, Pan_Address3, Pan_Address4, Company_IdNo,
                         FromAddress_SetPosition_Sts, Paper_Orientation, FROMAddress_Topoint, TOAddress_Topoint, FROMAddress_LeftPoint, TOAddress_LeftPoint, Freight_Bundle, Pavu_Stock_Minimum_Level, Pavu_Stock_Maximum_Level,
                         Yarn_Stock_Maximum_Level, Yarn_Stock_Minimum_Level, Sizing_To_CompanyIdNo, Sizing_To_VendorIdNo, Ledger_Transfer_Neft, Ledger_bank_PartyName, Ledger_BankName, Ledger_BranchName, Ledger_AccountNo,
                         Ledger_IFSCCode, Remarks, Party_MailId2, Party_MobileNo2, Distance, LedgerGroup_Idno, Credit_Limit_Amount, Credit_Limit_Days, Production_Per_Day, Legal_Nameof_Business, City_Town, Pincode,
                         Ledger_GSTIN_Verified_Status, vehicle_no, Zone_IdNo, State_Idno, TCS_Sales_Status, WeavingBill_IR_Receipt_Meters_Sts, Ledger_Tamil_Address1, Ledger_Tamil_Address2, Ledgers_CompanyIdNo,
                         Contact_Designation_IdNo, Party_Category_IdNo, Ledger_ShortName, Marketting_Executive_IdNo, Transport_Name, PriceList_IdNo FROM " & Trim(vDBNAMEFRM) & "..ledger_head Where Ledger_IdNo =  " & Str(Val(vLedger_IdNo))

                CmdTo.ExecuteNonQuery()


            Next I
            xlWorkBook.Close(False, FileName)
            xlApp.Quit()

            ReleaseComObject(xlWorkSheet)
            ReleaseComObject(xlWorkBook)
            ReleaseComObject(xlApp)

            Try
                CmdTo.CommandText = "Alter table ledger_head add Temp_LedgerIdNo_TransferCopy Int default 0"
                CmdTo.ExecuteNonQuery()

            Catch ex As Exception
                '----
            End Try

            CmdTo.CommandText = "Update ledger_head set Temp_LedgerIdNo_TransferCopy = Ledger_IdNo"
            CmdTo.ExecuteNonQuery()

            CmdTo.CommandText = "Update ledger_head set Ledger_IdNo = Ledger_IdNo+10000 Where Ledger_IdNo > 100"
            CmdTo.ExecuteNonQuery()

            vNEWID = 100
            Da1 = New SqlClient.SqlDataAdapter("select * from Ledger_Head Where Ledger_IdNo > 100 Order by Ledger_IdNo", CnTo)
            Da1.SelectCommand.Transaction = sqltr
            Dt1 = New DataTable
            Da1.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                For I = 0 To Dt1.Rows.Count - 1

                    vLedger_IdNo = Dt1.Rows(I).Item("Ledger_IdNo").ToString

                    vNEWID = vNEWID + 1

                    CmdTo.CommandText = "Update ledger_head set Ledger_IdNo = " & Str(Val(vNEWID)) & " Where Ledger_IdNo  = " & Str(Val(vLedger_IdNo))
                    CmdTo.ExecuteNonQuery()

                Next
            End If
            Dt1.Clear()

            CmdTo.CommandText = "Insert into Ledger_AlaisHead(Ledger_IdNo, Sl_No, Ledger_DisplayName, Ledger_Type, AccountsGroup_IdNo, Own_Loom_Status, Show_In_All_Entry, Verified_Status , Area_IdNo, Close_status) Select Ledger_IdNo, 1, Ledger_Name,   Ledger_Type,    AccountsGroup_IdNo, Own_Loom_Status, Show_In_All_Entry, Verified_Status, Area_IdNo, Close_status from Ledger_Head Where Ledger_IdNo > 100"
            CmdTo.ExecuteNonQuery()

            sqltr.Commit()

            CmdTo.Dispose()
            Dt1.Dispose()
            Da1.Dispose()
            CnTo.Dispose()

            MessageBox.Show("Imported Sucessfully!!!", "FOR IMPORTING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        'Catch ex As Exception
        '    sqltr.Rollback()
        '    MessageBox.Show(ex.Message, "DOES NOT IMPORT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        'End Try

    End Sub

    Private Sub ReleaseComObject(ByVal obj As Object)
        Try
            System.Runtime.InteropServices.Marshal.ReleaseComObject(obj)
            obj = Nothing
        Catch ex As Exception
            obj = Nothing
        End Try
    End Sub

End Class