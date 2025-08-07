Public Class Form1

    '    Public Sub Save11()
    '        Dim Rs1 As ADODB.Recordset
    '        Dim Rs2 As ADODB.Recordset
    '        Dim New_Code As String
    '        Dim i As Integer, Nr As Long, SNo As Integer
    '        Dim pcs_code As String, a As String, b As String, c As String, r As String
    '        Dim Yr As String
    '        Dim OrdBy_Rec As Currency, OrdBy_PcsChk As Currency, Cons_Yarn As Currency
    '        Dim Cmp_IdNo As Integer, Wgs_Sts As Integer, Mis_Id As Integer, Clo_ID As Integer
    '        Dim Grd_UpSts As Boolean, PcsInsSts As Boolean
    '        Dim RecMtrs As Currency, T1_Mtrs As Currency, T2_Mtrs As Currency, T3_Mtrs As Currency
    '        Dim T4_Mtrs As Currency, T5_Mtrs As Currency, UC_Mtrs As Currency
    '        Dim led_id As Integer, Chk_WgsSts As Integer, Pcs_ChkSts As Integer, Full_ChkSts As Integer
    '        Dim Wev_BillNo As String
    '        Dim m() As String, Pc As String
    '        Dim z As Integer, pcno As Long
    '        Dim Tot_PcsMtr As String, Tot_PcsWt As Currency, Wt_Mtr As Currency
    '        Dim PcsChk_NewCode As String
    '        Dim Pcs_SlNo As Currency

    '    If fra_Back.Enabled = False Then MgB.Message [Error On Save], "Close all other window":   Exit Sub

    '        If Edit_Status = True Then
    '            If UserRight_Check(UR.PieceChecking, False) = False Then Exit Sub
    '        ElseIf Edit_Status = False Then
    '            If UserRight_Check(UR.PieceChecking, True) = False Then Exit Sub
    '        End If

    '    If Val(lbl_Company.Tag) = 0 Then MgB.Message [Error On Save], "Invalid Company Selection": Exit Sub

    '    If Trim(txt_PcsNo.Text) <> Trim(txt_PcsNo.Tag) Then MgB.Message [Error On Save], "Invalid Piece Details": txt_PcsNo.SetFocus: Exit Sub

    '    If Val(txt_lotNo.Text) = 0 Then MgB.Message [Error On Save], "Invalid LotNo": txt_lotNo.SetFocus: Exit Sub
    '        If IsDate(sdt_Date.GetDate) = False Then
    '        MgB.Message [Error On Save], "Invalid Date":
    '            If sdt_Date.Enabled Then sdt_Date.SetFocus()
    '            Exit Sub
    '        End If
    '        If Not (CmpDet.FromDate <= Trim(sdt_Date.GetDate) And CmpDet.ToDate >= Trim(sdt_Date.GetDate)) Then
    '        MgB.Message [Error On Save], "Date is Out of Financial Year"
    '            If sdt_Date.Enabled Then sdt_Date.SetFocus()
    '            Exit Sub
    '        End If
    '        led_id = Val(Cmpr.Ledger_NameToIdno(con, lbl_Ledger.Caption))
    '    If Val(led_id) = 0 Then MgB.Message [Error On Save], "Invalid PartyName": txt_lotNo.SetFocus: Exit Sub
    '        Clo_ID = Val(Cloth_NameToIdno(cmb_Cloth.Text))
    '    If Val(Clo_ID) = 0 Then MgB.Message [Error On Save], "Invalid ClothName": cmb_Cloth.SetFocus: Exit Sub
    '    If Val(s2d_Folding.GetValue) = 0 Then MgB.Message [Error On Save], "Invalid Folding": s2d_Folding.SetFocus: Exit Sub
    '    If Val(txt_PcsNo.Text) = 0 Then MgB.Message [Error On Save], "Invalid PieceNo": txt_PcsNo.SetFocus: Exit Sub
    '        If Val(txt_PcsNo.Text) = Trim(txt_PcsNo.Text) Then
    '            If Val(s2d_RecMeters.GetValue) = 0 Then
    '            MgB.Message [Error On Save], "Invalid Receipt Meters":
    '                If s2d_RecMeters.Enabled Then s2d_RecMeters.SetFocus()
    '                Exit Sub
    '            End If
    '        End If

    '        New_Code = Trim(txt_lotNo.Text)
    '        If Not (Trim(New_Code) Like "*/??-??") Then New_Code = New_Code & "/" & Trim(CmpDet.FnYear)

    '        OrdBy_Rec = Val(Cmpr.OrderBy_CodeToValue(Trim(txt_lotNo.Text)))

    '        Call Total_PcsMeters_Calculation()

    '        On Error GoTo Err_Save
    '        con.BeginTrans()

    '        PcsChk_NewCode = ""
    '        Rs1 = New ADODB.Recordset
    '        Rs1.Open("Select * from weaver_clothreceipt_head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and weaver_clothreceipt_code = '" & Trim(New_Code) & "'", con, adOpenStatic, adLockReadOnly)
    '        If Not (Rs1.BOF And Rs1.EOF) Then
    '            Rs1.MoveFirst()
    '            If Rs1!weaver_PieceChecking_Code <> "" Then PcsChk_NewCode = Rs1!weaver_PieceChecking_Code
    '        End If
    '        Rs1.Close()
    '        Rs1 = Nothing

    '        If Trim(PcsChk_NewCode) = "" Then
    '            PcsChk_NewCode = Trim(Cmpr.Max_PkField(con, "Weaver_PieceChecking_Head", "Weaver_PieceChecking_Code", CmpDet.FnYear, , , Val(lbl_Company.Tag)))
    '            OrdBy_PcsChk = Val(Cmpr.OrderBy_CodeToValue(Trim(PcsChk_NewCode)))
    '            PcsChk_NewCode = PcsChk_NewCode & "/" & Trim(CmpDet.FnYear)
    '        Else
    '            OrdBy_PcsChk = Val(Cmpr.OrderBy_CodeToValue(Trim(Left(PcsChk_NewCode, Len(PcsChk_NewCode) - 6))))
    '        End If

    '        con.Execute("Delete from Weaver_PieceChecking_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_PieceChecking_Code = '" & Trim(PcsChk_NewCode) & "'")
    '        con.Execute("insert into Weaver_PieceChecking_Head ( company_idno, Weaver_PieceChecking_Code, For_OrderBy, " _
    '                  & "Weaver_PieceChecking_Date, Weaver_ClothReceipt_Code, Receipt_Type, User_IdNo )" _
    '                  & " values ( " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(PcsChk_NewCode) & "', " _
    '                  & Str(Val(OrdBy_PcsChk)) & ", '" _
    '                  & Trim(Format(sdt_Date.GetDate, "mm/dd/yyyy")) & "', '" _
    '                  & Trim(New_Code) & "', '" & Trim(lbl_ReceiptType.Caption) & "', " & Str(Val(User.IdNo)) & ")")

    '        con.Execute(" update weaver_clothreceipt_head set " _
    '                  & " weaver_PieceChecking_Code = '" & Trim(PcsChk_NewCode) & "', " _
    '                  & " weaver_PieceChecking_Date = '" & Trim(Format(sdt_Date.GetDate, "mm/dd/yyyy")) & "', " _
    '                  & " piecechecking_status = 1 " _
    '                  & " where company_idno = " & Str(Val(lbl_Company.Tag)) & " and weaver_clothreceipt_code = '" & Trim(New_Code) & "'")

    '        '--------------------------  PIECE CODE GENERATION   ------------------------------
    '        '                 1-2                  3-6                         7-10                       11-12              13                14-19                                      20-21
    '        '             Company IdNo       year of piece entry        weaver receipt no             ReceiptSubNo         rec/pur             piece no                                   PCS subno (A..Z)

    '        a = Val(New_Code)
    '        b = Trim(New_Code)
    '        b = Left(b, (Len(Trim(b)) - 6))
    '        Yr = Right(New_Code, 5)
    '        Yr = "20" & Left(Yr, 2)
    '        c = "0"
    '        pcs_code = Format(Val(lbl_Company.Tag), "00") & Trim(Yr) & Format(Val(New_Code), "0000") & Trim(IIf(Val(a) <> Trim(b), Asc(Right(UCase(b), 1)), "00")) & Trim(c) & Format(Val(txt_PcsNo.Text), "000000") & Trim(IIf(Val(txt_PcsNo.Text) <> Trim(txt_PcsNo.Text), Asc(Right(UCase(txt_PcsNo.Text), 1)), "00"))

    '        Pcs_SlNo = Val(Cmpr.OrderBy_CodeToValue(Trim(txt_PcsNo.Text)))

    '        Nr = 0
    '        con.Execute(" update weaver_piecechecking_details set " & _
    '                    " Weaver_PieceChecking_Code = '" & Trim(PcsChk_NewCode) & "', " & _
    '                    " Weaver_PieceChecking_Date = '" & Trim(Format(sdt_Date.GetDate, "mm/dd/yyyy")) & "', " & _
    '                    " Receipt_Type = " & Str(Val(lbl_ReceiptType.Caption)) & ", " & _
    '                    " Ledger_Idno  = " & Str(led_id) & ", " & _
    '                    " Cloth_Idno  = " & Str(Clo_ID) & ", " & _
    '                    " Folding = " & Str(Val(s2d_Folding.GetValue)) & ", Folding1 = " & Str(Val(s2d_Folding.GetValue)) & ", Folding2 = " & Str(Val(s2d_Folding.GetValue)) & ", Folding3 = " & Str(Val(s2d_Folding.GetValue)) & ", Folding4 = " & Str(Val(s2d_Folding.GetValue)) & ", Folding5 = " & Str(Val(s2d_Folding.GetValue)) & ", " & _
    '                    " Sl_No = " & Str(Val(Pcs_SlNo)) & ", " & _
    '                    " Loom_No = " & Str(Val(txt_LoomNo.Text)) & ", " & _
    '                    " Cloth_Pick = " & Str(Val(txt_Pick.Text)) & ", " & _
    '                    " Cloth_Width = " & Str(Val(txt_Width.Text)) & ", " & _
    '                    " Receipt_Meters = " & Str(Val(s2d_RecMeters.GetValue)) & ", " & _
    '                    " Cloth_Type1_Meters = " & Str(Val(s2d_Type1.GetValue)) & ", " & _
    '                    " Cloth_Type2_Meters = " & Str(Val(s2d_Type2.GetValue)) & ", " & _
    '                    " Cloth_Type3_Meters = " & Str(Val(s2d_Type3.GetValue)) & ", " & _
    '                    " Cloth_Type4_Meters = " & Str(Val(s2d_Type4.GetValue)) & ", " & _
    '                    " Cloth_Type5_Meters = " & Str(Val(s2d_Type5.GetValue)) & ", " & _
    '                    " Total_Meters = " & Str(Val(lbl_Total.Caption)) & ", " & _
    '                    " Weight = " & Str(Val(s3d_Weight.GetValue)) & ", " & _
    '                    " Weight_Meter = " & Str(Val(lbl_WgtMtr.Caption)) & ", " & _
    '                    " User_IdNo = " & Str(User.IdNo) & " " & _
    '                    " where Weaver_ClothReceipt_Code = '" & Trim(New_Code) & "' and " & _
    '                    " company_idno = " & Str(Val(lbl_Company.Tag)) & " and piece_code = '" & Trim(pcs_code) & "'", Nr)

    '        If Nr = 0 Then

    '            If Val(txt_PcsNo.Text) > Val(grd_PcsDetails.TextMatrix(grd_PcsDetails.Rows - 1, 0)) Then Err.Description = "Invalid Piece No" : GoTo Err_Save

    '            con.Execute("Insert into weaver_piecechecking_details ( company_idno,  Weaver_PieceChecking_Code, Weaver_PieceChecking_Date, " _
    '                      & "Weaver_ClothReceipt_Code, For_OrderBy, Weaver_ClothReceipt_Date, Receipt_Type, lot_no, Ledger_IdNo, cloth_idno, Folding, Folding1, Folding2, Folding3, Folding4, Folding5, " _
    '                      & "Sl_No, piece_code, piece_no, loom_no, cloth_pick, cloth_width, Receipt_Meters, " _
    '                      & "Cloth_type1_meters, cloth_type2_meters, cloth_type3_meters, cloth_type4_meters, cloth_type5_meters, total_meters, " _
    '                      & "Weight, weight_Meter, User_IdNo ) values (" & Str(Val(lbl_Company.Tag)) & ", '" & Trim(PcsChk_NewCode) & "', '" _
    '                      & Trim(Format(sdt_Date.GetDate, "mm/dd/yyyy")) & "', '" & Trim(New_Code) & "', " & Str(Val(OrdBy_Rec)) & ", '" _
    '                      & Trim(Format(sdt_RecDate.GetDate, "mm/dd/yyyy")) & "', " & Str(Val(lbl_ReceiptType.Caption)) & ", '" & Trim(New_Code) & "', " & Str(led_id) & ", " _
    '                      & Str(Clo_ID) & ", " & Str(Val(s2d_Folding.GetValue)) & ", " & Str(Val(s2d_Folding.GetValue)) & ", " & Str(Val(s2d_Folding.GetValue)) & ", " & Str(Val(s2d_Folding.GetValue)) & ", " & Str(Val(s2d_Folding.GetValue)) & ", " & Str(Val(s2d_Folding.GetValue)) & ", " _
    '                      & Str(Val(Pcs_SlNo)) & ", '" & Trim(pcs_code) & "', '" & Trim(txt_PcsNo.Text) & "', " _
    '                      & Str(Val(txt_LoomNo.Text)) & ", " & Str(Val(txt_Pick.Text)) & ", " & Str(Val(txt_Width.Text)) & ", " _
    '                      & Str(Val(s2d_RecMeters.GetValue)) & ", " & Str(Val(s2d_Type1.GetValue)) & ", " _
    '                      & Str(Val(s2d_Type2.GetValue)) & ", " & Str(Val(s2d_Type3.GetValue)) & ", " _
    '                      & Str(Val(s2d_Type4.GetValue)) & ", " & Str(Val(s2d_Type5.GetValue)) & ", " _
    '                      & Str(Val(lbl_Total.Caption)) & ", " & Str(Val(s3d_Weight.GetValue)) & ", " _
    '                      & Str(Val(lbl_WgtMtr.Caption)) & ", " & Str(User.IdNo) & ")")

    '        End If

    '        Nr = 0
    '        con.Execute(" update weaver_piecechecking_details set " & _
    '                    " Folding = " & Str(Val(s2d_Folding.GetValue)) & ", Folding1 = " & Str(Val(s2d_Folding.GetValue)) & ", Folding2 = " & Str(Val(s2d_Folding.GetValue)) & ", Folding3 = " & Str(Val(s2d_Folding.GetValue)) & ", Folding4 = " & Str(Val(s2d_Folding.GetValue)) & ", Folding5 = " & Str(Val(s2d_Folding.GetValue)) & "  " & _
    '                    " where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(New_Code) & "'")

    '        Call Posting()

    '        Grd_UpSts = False
    '        With grd_PcsDetails
    '            For i = 0 To .Rows - 1
    '                If Trim(.TextMatrix(i, 0)) = Trim(txt_PcsNo.Text) Then
    '                    .TextMatrix(i, 1) = Trim(txt_LoomNo.Text)
    '                    .TextMatrix(i, 2) = Format(Val(lbl_WgtMtr.Caption), "#######0.000")
    '                    .TextMatrix(i, 3) = IIf(Val(s3d_Weight.GetValue) <> 0, Format(Val(s3d_Weight.GetValue), "#######0.000"), "")
    '                    .TextMatrix(i, 4) = IIf(Val(s2d_RecMeters.GetValue) <> 0, Format(Val(s2d_RecMeters.GetValue), "#######0.00"), "")
    '                    .TextMatrix(i, 5) = IIf(Val(s2d_Type1.GetValue) <> 0, Format(Val(s2d_Type1.GetValue), "#######0.00"), "")
    '                    .TextMatrix(i, 6) = IIf(Val(s2d_Type2.GetValue) <> 0, Format(Val(s2d_Type2.GetValue), "#######0.00"), "")
    '                    .TextMatrix(i, 7) = IIf(Val(s2d_Type3.GetValue) <> 0, Format(Val(s2d_Type3.GetValue), "#######0.00"), "")
    '                    .TextMatrix(i, 8) = IIf(Val(s2d_Type4.GetValue) <> 0, Format(Val(s2d_Type4.GetValue), "#######0.00"), "")
    '                    .TextMatrix(i, 9) = IIf(Val(s2d_Type5.GetValue) <> 0, Format(Val(s2d_Type5.GetValue), "#######0.00"), "")
    '                    .TextMatrix(i, 10) = IIf(Val(lbl_Total.Caption) <> 0, Format(Val(lbl_Total.Caption), "#######0.00"), "")
    '                    .TextMatrix(i, 11) = Trim(txt_Pick.Text)
    '                    .TextMatrix(i, 12) = Trim(txt_Width.Text)

    '                    If Val(lbl_Total.Caption) > 0 Then
    '                        Call Cmpr.Grids_CellBackColor(grd_PcsDetails, i, "245,245,245")
    '                    Else
    '                        Call Cmpr.Grids_CellBackColor(grd_PcsDetails, i, "251,255,246")
    '                    End If

    '                    grd_PcsDetails.Row = i
    '                    If Trim(lbl_PackSlipNo1.Caption) <> "" Then grd_PcsDetails.Col = 5 : grd_PcsDetails.CellForeColor = RGB(49, 151, 34)
    '                    If Trim(lbl_PackSlipNo2.Caption) <> "" Then grd_PcsDetails.Col = 6 : grd_PcsDetails.CellForeColor = RGB(49, 151, 34)
    '                    If Trim(lbl_PackSlipNo3.Caption) <> "" Then grd_PcsDetails.Col = 7 : grd_PcsDetails.CellForeColor = RGB(49, 151, 34)
    '                    If Trim(lbl_PackSlipNo4.Caption) <> "" Then grd_PcsDetails.Col = 8 : grd_PcsDetails.CellForeColor = RGB(49, 151, 34)
    '                    If Trim(lbl_PackSlipNo5.Caption) <> "" Then grd_PcsDetails.Col = 9 : grd_PcsDetails.CellForeColor = RGB(49, 151, 34)
    '                    If i > 11 Then .TopRow = i - 11 Else .TopRow = 0
    '                    Grd_UpSts = True
    '                    Exit For
    '                End If
    '            Next
    '            For i = 0 To .Rows - 1
    '                If Val(.TextMatrix(i, 0)) = Val(txt_PcsNo.Text) Then
    '                    .TextMatrix(i, 2) = Format(Val(lbl_WgtMtr.Caption), "#######0.000")
    '                End If
    '            Next
    '            If Grd_UpSts = False Then
    '                For i = 0 To .Rows - 1
    '                    If Val(.TextMatrix(i, 0)) = Val(txt_PcsNo.Text) Then
    '                        .AddItem(Trim(txt_PcsNo.Text) & vbTab & Trim(txt_LoomNo.Text) & vbTab & IIf(Val(lbl_WgtMtr.Caption) <> 0, Format(Val(lbl_WgtMtr.Caption), "#######0.000"), "") & vbTab & IIf(Val(s3d_Weight.GetValue) <> 0, Format(Val(s3d_Weight.GetValue), "#######0.000"), "") & vbTab & IIf(Val(s2d_RecMeters.GetValue) <> 0, Format(Val(s2d_RecMeters.GetValue), "#######0.00"), "") & vbTab & IIf(Val(s2d_Type1.GetValue) <> 0, Format(Val(s2d_Type1.GetValue), "#######0.00"), "") & vbTab & IIf(Val(s2d_Type2.GetValue) <> 0, Format(Val(s2d_Type2.GetValue), "#######0.00"), "") & vbTab & IIf(Val(s2d_Type3.GetValue) <> 0, Format(Val(s2d_Type3.GetValue), "#######0.00"), "") & vbTab & IIf(Val(s2d_Type4.GetValue) <> 0, Format(Val(s2d_Type4.GetValue), "#######0.00"), "") & vbTab & IIf(Val(s2d_Type5.GetValue) <> 0, Format(Val(s2d_Type5.GetValue), "#######0.00"), "") & vbTab & IIf(Val(lbl_Total.Caption) <> 0, Format(Val(lbl_Total.Caption), "#######0.00"), "") & vbTab & _
    '                                    Trim(txt_Pick.Text) & vbTab & Trim(txt_Width.Text), i + 1)
    '                        grd_PcsDetails.RowData(i + 1) = 0
    '                        Call Cmpr.Grids_CellBackColor(grd_PcsDetails, i + 1, "245,245,245")
    '                        If i > 11 Then .TopRow = i - 10 Else .TopRow = 0
    '                        Exit For
    '                    End If
    '                Next
    '            End If

    '        End With

    '        Call Total_Meters_Calculation()

    '        con.CommitTrans()
    '        Cmpr.Processing_Message(Saving)
    '        Change_Status = False
    '        pcno = Val(txt_PcsNo.Text)
    '        Call Clear_PcsDetails()

    '        If pcno < Val(grd_PcsDetails.TextMatrix(grd_PcsDetails.Rows - 1, 0)) Then txt_PcsNo.Text = pcno + 1

    '        If txt_PcsNo.Enabled Then txt_PcsNo.SetFocus() Else txt_lotNo.SetFocus()

    '        Exit Sub

    'Err_Save:
    '        con.RollbackTrans()
    '        MgB.Message [Error On Save], Err.Description
    '    End Sub


    '    Private Sub Stock_Posting()
    '        Dim Rs1 As ADODB.Recordset
    '        Dim Rs2 As ADODB.Recordset
    '        Dim New_Code As String
    '        Dim i As Integer, Nr As Long, SNo As Integer
    '        Dim pcs_code As String, a As String, b As String, c As String, r As String
    '        Dim Yr As String, Pc As String
    '        Dim OrdBy_Rec As Currency, Cons_Yarn As Currency
    '        Dim Wgs_Sts As Integer, Clo_ID As Integer
    '        Dim RecMtrs As Currency, T1_Mtrs As Currency, T2_Mtrs As Currency, T3_Mtrs As Currency
    '        Dim T4_Mtrs As Currency, T5_Mtrs As Currency, UC_Mtrs As Currency
    '        Dim led_id As Integer, Chk_WgsSts As Integer, Pcs_ChkSts As Integer, Full_ChkSts As Integer
    '        Dim z As Integer, RecType As String
    '        Dim Tot_PcsMtr As String, Tot_PcsWt As Currency, Wt_Mtr As Currency
    '        Dim Pv_Mtrs As Currency, LmTY As Currency, Crimp As Currency
    '        Dim Crmp_Perc As Currency, Crmp_Mtrs As Currency, PavuConsMtrs As Currency
    '        Dim Lm_Typ As Currency, CloRecFld As Currency
    '        Dim ForStk_Yrn As Currency
    '        Dim Mtrs5StkSts As String
    '        Dim AutLm_Noof_InpBms As Integer

    '        led_id = Val(Cmpr.Ledger_NameToIdno(con, lbl_Ledger.Caption))
    '        Clo_ID = Val(Cloth_NameToIdno(cmb_Cloth.Text))

    '        New_Code = Trim(txt_lotNo.Text)
    '        If Not (Trim(New_Code) Like "*/??-??") Then New_Code = New_Code & "/" & Trim(CmpDet.FnYear)

    '        OrdBy_Rec = Val(Cmpr.OrderBy_CodeToValue(Trim(txt_lotNo.Text)))


    '        '--------------------------  PIECE CODE GENERATION   ---------------------------------
    '        '                 1-2                  3-6                         7-10                       11-12              13                14-19                                      20-21
    '        '             Company IdNo       year of piece entry        weaver receipt no             ReceiptSubNo         rec/pur             piece no                                   PCS subno (A..Z)


    '        a = Val(New_Code)
    '        b = Trim(New_Code)
    '        b = Left(b, (Len(Trim(b)) - 6))
    '        Yr = Right(New_Code, 5)
    '        Yr = "20" & Left(Yr, 2)
    '        c = "0"
    '        pcs_code = Format(Val(lbl_Company.Tag), "00") & Trim(Yr) & Format(Val(New_Code), "0000") & Trim(IIf(Val(a) <> Trim(b), Asc(Right(UCase(b), 1)), "00")) & Trim(c) & Format(Val(txt_PcsNo.Text), "000000") & Trim(IIf(Val(txt_PcsNo.Text) <> Trim(txt_PcsNo.Text), Asc(Right(UCase(txt_PcsNo.Text), 1)), "00"))


    '        Tot_PcsMtr = 0 : Tot_PcsWt = 0 : Wt_Mtr = 0

    '        Pc = Left(pcs_code, 19)
    '        Rs1 = New ADODB.Recordset
    '        Rs1.Open("Select sum(Cloth_Type1_Meters+Cloth_Type2_Meters+Cloth_Type3_Meters+Cloth_Type4_Meters+Cloth_Type5_Meters) as TotMtrs, Sum(weight) as Tot_Wt from weaver_piecechecking_details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and weaver_clothreceipt_code = '" & Trim(New_Code) & "' and Piece_Code LIKE '" & Trim(Pc) & "%'", con, adOpenStatic, adLockReadOnly)
    '        If Not (Rs1.BOF And Rs1.EOF) Then
    '            Rs1.MoveFirst()
    '            Tot_PcsMtr = Rs1!totmtrs
    '            Tot_PcsWt = Rs1!Tot_Wt
    '        End If
    '        Rs1.Close()
    '        Rs1 = Nothing

    '        lbl_WgtMtr.Caption = "0.000"
    '        If Tot_PcsWt > 0 And Tot_PcsMtr > 0 Then
    '            Wt_Mtr = Tot_PcsWt / Tot_PcsMtr
    '            If Val(s2d_Folding.GetValue) > 0 And Val(s2d_Folding.GetValue) <> 100 And Wt_Mtr > 0 Then
    '                'lbl_WgtMtr.Caption = Format(Wt_Mtr + (Wt_Mtr * (100 - Val(s2d_Folding.GetValue)) / 100), "########0.000")
    '                Wt_Mtr = (Tot_PcsWt / Tot_PcsMtr) + ((Tot_PcsWt / Tot_PcsMtr) * (100 - Val(s2d_Folding.GetValue)) / 100)
    '                lbl_WgtMtr.Caption = Format(Wt_Mtr, "########0.000")
    '            Else
    '                lbl_WgtMtr.Caption = Format(Wt_Mtr, "########0.000")
    '            End If
    '        End If

    '        con.Execute(" update weaver_piecechecking_details set " & _
    '                    " Weight_Meter = " & Str(Val(lbl_WgtMtr.Caption)) & " " & _
    '                    " where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(New_Code) & "' and " & _
    '                    " piece_code LIKE '" & Trim(Pc) & "%'", z)

    '        RecMtrs = 0 : T1_Mtrs = 0 : T2_Mtrs = 0 : T3_Mtrs = 0 : T4_Mtrs = 0 : T5_Mtrs = 0
    '        UC_Mtrs = 0
    '        Pcs_ChkSts = 1 : Full_ChkSts = 0
    '        Wgs_Sts = 0
    '        RecType = 0
    '        Rs1 = New ADODB.Recordset
    '        Rs1.Open("Select sum(Receipt_Meters) as RecMtrs, sum(Cloth_Type1_Meters) as Type1Mtrs, sum(Cloth_Type2_Meters) as Type2Mtrs, sum(Cloth_Type3_Meters) as Type3Mtrs, sum(Cloth_Type4_Meters) as Type4Mtrs, sum(Cloth_Type5_Meters) as Type5Mtrs from weaver_piecechecking_details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and weaver_clothreceipt_code = '" & Trim(New_Code) & "'", con, adOpenStatic, adLockReadOnly)
    '        If Not (Rs1.BOF And Rs1.EOF) Then
    '            Rs1.MoveFirst()
    '            RecMtrs = Rs1!RecMtrs
    '            T1_Mtrs = Rs1!Type1Mtrs
    '            T2_Mtrs = Rs1!Type2Mtrs
    '            T3_Mtrs = Rs1!Type3Mtrs
    '            T4_Mtrs = Rs1!Type4Mtrs
    '            T5_Mtrs = Rs1!Type5Mtrs
    '            If Val(T1_Mtrs + T2_Mtrs + T3_Mtrs + T4_Mtrs + T5_Mtrs) = 0 Then Pcs_ChkSts = 0
    '        End If
    '        Rs1.Close()
    '        Rs1.Open("Select sum(Receipt_Meters) as RecMtrs from weaver_piecechecking_details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and weaver_clothreceipt_code = '" & Trim(New_Code) & "' and (Cloth_Type1_Meters+Cloth_Type2_Meters+Cloth_Type3_Meters+Cloth_Type4_Meters+Cloth_Type5_Meters)=0", con, adOpenStatic, adLockReadOnly)
    '        If Not (Rs1.BOF And Rs1.EOF) Then
    '            If Rs1(0).Value <> "" Then UC_Mtrs = Rs1(0).Value
    '        End If
    '        Rs1.Close()
    '        Rs1.Open("Select count(*) from weaver_piecechecking_details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and weaver_clothreceipt_code = '" & Trim(New_Code) & "' and ( Cloth_Type1_Meters + Cloth_Type2_Meters + Cloth_Type3_Meters + Cloth_Type4_Meters + Cloth_Type5_Meters ) = 0", con, adOpenStatic, adLockReadOnly)
    '        If Not (Rs1.BOF And Rs1.EOF) Then
    '            If Rs1(0).Value <> "" Then
    '                If Val(Rs1(0).Value) = 0 Then Full_ChkSts = 1
    '            End If
    '        End If
    '        Rs1.Close()
    '        Rs1.Open("Select * from weaver_clothreceipt_head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and weaver_clothreceipt_code = '" & Trim(New_Code) & "'", con, adOpenStatic, adLockReadOnly)
    '        If Not (Rs1.BOF And Rs1.EOF) Then
    '            Rs1.MoveFirst()
    '            If Rs1!weaver_wages_code <> "" Then Wgs_Sts = 1
    '        End If
    '        Rs1.Close()
    '        Rs1 = Nothing

    '        Cons_Yarn = ConsYarn_Calculation(RecMtrs)

    '        con.Execute(" update weaver_clothreceipt_head set " _
    '                  & " weaver_PieceChecking_Date = '" & Trim(Format(sdt_Date.GetDate, "mm/dd/yyyy")) & "', " _
    '                  & " Receipt_PavuPcs = " & Str(Val(txt_PavuPcs.Text)) & ", Receipt_Meters = " & Str(Val(RecMtrs)) & ", " _
    '                  & " Rough_Consumed_Yarn = " & Str(Val(Cons_Yarn)) & ", " _
    '                  & " pck_cloth_meters_type1 = " & Str(Val(T1_Mtrs)) & ", " _
    '                  & " pck_cloth_meters_type2 = " & Str(Val(T2_Mtrs)) & ", " _
    '                  & " pck_cloth_meters_type3 = " & Str(Val(T3_Mtrs)) & ", " _
    '                  & " pck_cloth_meters_type4 = " & Str(Val(T4_Mtrs)) & ", " _
    '                  & " pck_cloth_meters_type5 = " & Str(Val(T5_Mtrs)) & ", " _
    '                  & " folding = " & Str(Val(s2d_Folding.GetValue)) & "," _
    '                  & " piecechecking_status = " & Str(Pcs_ChkSts) & ", " _
    '                  & " Piecechecking_CompleteStatus = " & Str(Full_ChkSts) & " " _
    '                  & " where company_idno = " & Str(Val(lbl_Company.Tag)) & " and weaver_clothreceipt_code = '" & Trim(New_Code) & "'")

    '        Mtrs5StkSts = "Meters_Type5 = " & Str(Val(T5_Mtrs))
    '        If Trim(UCase(Settings.Name)) = "THIRUMAGAL-ERODE" Then Mtrs5StkSts = "Meters_Type5 = 0 "

    '        con.Execute("update Cloth_Processing_Details set " _
    '                  & "reference_date = '" & Trim(Format(sdt_Date.GetDate, "mm/dd/yyyy")) & "', " _
    '                  & "Folding = " & Str(Val(s2d_Folding.GetValue)) & ", " _
    '                  & "Meters_Type1 = " & Str(Val(UC_Mtrs + T1_Mtrs)) & ", " _
    '                  & "Meters_Type2 = " & Str(Val(T2_Mtrs)) & ", " _
    '                  & "Meters_Type3 = " & Str(Val(T3_Mtrs)) & ", " _
    '                  & "Meters_Type4 = " & Str(Val(T4_Mtrs)) & IIf(Mtrs5StkSts <> "", ", ", "") & Mtrs5StkSts _
    '                  & " Where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(New_Code) & "'")

    '        If Wgs_Sts = 0 And Val(lbl_ReceiptType.Caption) = 1 Then
    '            CloRecFld = Val(Cmpr.Get_FieldValue(con, "Weaver_ClothReceipt_Head", "ClothReceipt_Folding", "(company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(New_Code) & "')"))
    '            If Val(CloRecFld) = 0 Then CloRecFld = 100
    '            Crmp_Perc = 0 : Crmp_Mtrs = 0
    '            PavuConsMtrs = Val(RecMtrs) * CloRecFld / 100
    '            Lm_Typ = 1
    '            If Val(Settings.AutoLoom) = 1 And Trim(UCase(Settings.Pavu_Increment_forAutoLoom_In_Dc_Or_Rcpt)) = "RECEIPT" Then
    '                Rs2 = New ADODB.Recordset
    '                Rs2.Open("Select * from cloth_head where cloth_idno = " & Str(Val(Clo_ID)), con, adOpenStatic, adLockReadOnly)
    '                If Not (Rs2.BOF And Rs2.EOF) Then
    '                    Rs2.MoveFirst()
    '                    If Rs2!Loom_Type <> "" Then Lm_Typ = Val(Rs2!Loom_Type)
    '                    If Rs2!Crimp <> "" Then Crmp_Perc = Val(Rs2!Crimp)
    '                End If
    '                Rs2 = Nothing

    '                'AutLm_Noof_InpBms = Val(Settings.AutoLoom_Noof_InputBeams)
    '                'If Val(AutLm_Noof_InpBms) = 0 Then AutLm_Noof_InpBms = 2
    '                'If Lm_Typ <> 0 Then PavuConsMtrs = (PavuConsMtrs / Lm_Typ) * AutLm_Noof_InpBms

    '                ''If Lm_Typ <> 0 Then PavuConsMtrs = PavuConsMtrs / Lm_Typ

    '                PavuConsMtrs = PavuConsMtrs * Lm_Typ

    '                Crmp_Mtrs = Val(PavuConsMtrs) * Crmp_Perc / 100
    '                PavuConsMtrs = Format(PavuConsMtrs + Crmp_Mtrs, "#########0.00")
    '            End If

    '            If UCase(Settings.Name) = "NST" Then
    '                PavuConsMtrs = (UC_Mtrs + T1_Mtrs + T2_Mtrs + T3_Mtrs + T4_Mtrs + T5_Mtrs)
    '                Cons_Yarn = ConsYarn_Calculation(PavuConsMtrs)
    '            End If

    '            con.Execute("update Pavu_Processing_Details set " _
    '                      & "Pcs = " & Str(Val(txt_PavuPcs.Text)) & ", " _
    '                      & "Meters = " & Str(Val(PavuConsMtrs)) & " " _
    '                      & "Where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(New_Code) & "'")

    '            ForStk_Yrn = Val(Cons_Yarn)
    '            If Val(Settings.Weaver_YarnStock_InMeter) = 1 Then
    '                Wt_Mtr = Cmpr.Get_FieldValue(con, "Cloth_Head", "Weight_Meter_Weft", "(cloth_IdNo = " & Str(Clo_ID) & ")")
    '                ForStk_Yrn = 0
    '                If Wt_Mtr <> 0 Then ForStk_Yrn = Val(Cons_Yarn) / Wt_Mtr
    '            End If

    '            con.Execute("update Yarn_Processing_Details set " _
    '                      & "weight = " & Str(Cons_Yarn) & ", " _
    '                      & "for_stock = " & Str(ForStk_Yrn) & " " _
    '                      & "Where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(New_Code) & "'")

    '        End If

    '        For i = 0 To grd_PcsDetails.Rows - 1
    '            If Val(grd_PcsDetails.TextMatrix(i, 0)) = Val(txt_PcsNo.Text) Then
    '                grd_PcsDetails.TextMatrix(i, 2) = Format(Val(lbl_WgtMtr.Caption), "#######0.000")
    '            End If
    '        Next

    '    End Sub


End Class