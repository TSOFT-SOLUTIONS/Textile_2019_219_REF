Public Class Script_Generation




    Private Sub AEROCODE()
        '        Option Explicit

        '    Public Sub Smart_Clear()
        '        Call Clear(Me)
        '        rtxt_SqlScript.Text = ""
        '        Fra_Back.Enabled = True
        '    End Sub

        '    Private Sub cmb_Software_KeyPress(ByVal KeyAscii As Integer)
        '        If KeyAscii = 13 Then Call Cmd_GenerateScript_Click()
        '    End Sub

        '    Private Sub cmd_Copy_Click()
        '        Clipboard.Clear()
        '        Clipboard.SetText(rtxt_SqlScript.Text)
        '    End Sub

        '    Private Sub Form_Load()
        '        Dim i As Integer

        '        FrmSts.Count = FrmSts.Count + 1
        '        License_Information(Me)

        '        Grid_Designing(Grd_Head, 13, 4, 90, 200, 600, "2000|3500|3000|4000", "0,0,2,3|1,1,0,3|2,2,0,3")
        '        ControlSet_OnGrid(Grd_Head, 0, 0, "SOFTWARE NAME", Cmb_Software)
        '        ControlSet_OnGrid(Grd_Head, 0, 1, " ", Cmd_GenerateScript)
        '        ControlSet_OnGrid(Grd_Head, 0, 2, "    ", cmd_Copy)

        '        With Grd_Head

        '            .Row = 0 : .Col = 3 : .CellAlignment = 4 : .CellBackColor = RGB(240, 240, 240) : .CellForeColor = RGB(110, 110, 110)

        '            For i = 0 To .Cols - 1
        '                .TextMatrix(1, i) = "SQL SCRIPT"
        '            Next
        '            .Row = 1 : .Col = 0 : .CellAlignment = 4 : .CellBackColor = RGB(240, 240, 240) : .CellForeColor = RGB(110, 110, 110)
        '        End With

        '        MergerCells_ByControl(Grd_Head, 2, 0, 12, 3, rtxt_SqlScript)
        '        rtxt_SqlScript.BackColor = RGB(250, 250, 250)

        '        Cmb_Software.Table_Name = "SoftWare_Head"
        '        Cmb_Software.Field_Name = "SoftWare_Name"

        '        Fra_Back.Width = Grd_Head.Width + Grd_Head.Left + 120
        '        Fra_Back.Height = Grd_Head.Top + Grd_Head.Height + 150
        '        Form_Designing(Me)

        '        Me.Top = Me.Top + 1000
        '        Me.Left = Me.Left - 75

        '        Call Me.Smart_Clear()

        '    End Sub

        '    Private Sub Form_KeyPress(ByVal KeyAscii As Integer)
        '        If KeyAscii = 27 Then Unload(Me)
        '    End Sub

        '    Private Sub cmb_Software_GotFocus()
        '        Cmb_Software.ZOrder(0)
        '    End Sub

        '    Private Sub Cmd_GenerateScript_Click()
        '        Dim Cn1 As ADODB.Connection
        '        Dim Rs As ADODB.Recordset, Rs1 As ADODB.Recordset, Rs2 As ADODB.Recordset
        '        Dim Rt1 As ADODB.Recordset, Rt2 As ADODB.Recordset, Rt3 As ADODB.Recordset
        '        Dim New_Code As String, DBName As String, DBMethod As String
        '        Dim Ind As Integer, MaxInd As Integer, Blk_Idno As Integer, Col_Indx As Integer
        '        Dim PKeyCode As String, TblCreateStr As String, DefConsNm As String, DefConsVal As String, KeyColNms As String
        '        Dim TblName As String, FldDatTyp As String, Nul_NtNul_Sts As String, FldIdentity_Sts As String, Str_Disp As String
        '        Dim AryInd As Long
        '        Dim TblCreateQry As String, ArQry() As String, Txt As String
        '        Dim i As Integer, m1 As Integer, Cnt As Integer, k As Integer, j As Integer
        '        Dim Soft_Idno As Integer

        '        rtxt_SqlScript.Text = "'----- Script Generated On " & Now

        '        Soft_Idno = Val(Cmpr.Get_FieldValue(Con, "Software_Head", "Software_Idno", "Software_Name = '" & Trim(Cmb_Software.Text) & "'"))
        '    If Soft_Idno = 0 Then MgB.Message [Error On Save], "Invalid Software Name": Cmb_Software.SetFocus: Exit Sub

        '        '====================  Metrs-Smart, Weight-Smart, AmountSmart --> DataType Creation

        '        rtxt_SqlScript.Text = rtxt_SqlScript.Text & IIf(rtxt_SqlScript.Text <> "", Chr(13), "") & "Public Sub Software_AllTables_Creation_Script(pvl_Cn As ADODB.Connection)" & Chr(13)
        '        rtxt_SqlScript.Text = rtxt_SqlScript.Text & "     Dim pvl_s1 as String, pvl_s2 as String, pvl_s3 as String, pvl_s4 as String, pvl_s5 as String, pvl_s6 as String, pvl_s7 as String, pvl_s8 as String, pvl_s9 as String, pvl_s10 as String" & Chr(13)

        '        Call Smart_DataType_Creation()

        '        Rs = New ADODB.Recordset
        '        Rt1 = New ADODB.Recordset
        '        Rs1 = New ADODB.Recordset
        '        Rs2 = New ADODB.Recordset
        '        Rt1.Open("select * from Other_Software_Table_Head Where Software_Idno = " & Str(Soft_Idno) & " Order by Other_Software_Table_Name ", Con, adOpenStatic, adLockReadOnly)
        '        If Not (Rt1.BOF And Rt1.EOF) Then
        '            Rt1.MoveFirst()
        '            Do While Not Rt1.EOF

        '                If Rt1!Other_Software_Table_Name = "AgentCommission_Processing_Details" Then
        '                    Debug.Print(Rt1!Other_Software_Table_Name)
        '                End If

        '                AryInd = AryInd + 1
        '                TblCreateQry = Creation_Table(Rt1!Other_Software_Table_Idno)

        '                Erase ArQry

        '                If Trim(TblCreateQry) <> "" Then
        '                    If Len(TblCreateQry) > 1000 Then
        '                        ArQry = Wrap_the_Query(TblCreateQry)

        '                        '12 Lines are limit

        '                        If UBound(ArQry) > 11 And UBound(ArQry) <> 12 Then

        '                            Cnt = Round(UBound(ArQry) / 11)

        '                            Str_Disp = ""
        '                            k = 0
        '                            For i = 1 To Cnt
        '                                Str_Disp = Str_Disp & IIf(Str_Disp <> "", " & ", "") & " s" & Trim(i)

        '                                j = 0
        '                                For k = k To k + 10
        '                                    If k > UBound(ArQry) Then Exit For
        '                                    Txt = IIf(j = 0, " s" & Trim(i) & " = ", "") & Replace(LCase(ArQry(k)), "pvl_cn.execute", "")

        '                                    If j = 10 And i <> Cnt And Right(Txt, 3) = "& _" Then
        '                                        Txt = Left(Txt, Len(Txt) - 3)
        '                                    End If

        '                                    rtxt_SqlScript.Text = rtxt_SqlScript.Text & IIf(rtxt_SqlScript.Text <> "", Chr(13), "") & Txt
        '                                    j = j + 1
        '                                Next
        '                            Next

        '                            For k = k To UBound(ArQry)
        '                                rtxt_SqlScript.Text = rtxt_SqlScript.Text & IIf(rtxt_SqlScript.Text <> "", Chr(13), "") & ArQry(k)
        '                            Next

        '                            rtxt_SqlScript.Text = rtxt_SqlScript.Text & IIf(rtxt_SqlScript.Text <> "", Chr(13), "") & " pvl_cn.execute " & Str_Disp

        '                        Else
        '                            For i = 0 To UBound(ArQry)
        '                                rtxt_SqlScript.Text = rtxt_SqlScript.Text & IIf(rtxt_SqlScript.Text <> "", Chr(13), "") & ArQry(i)
        '                            Next
        '                        End If

        '                        rtxt_SqlScript.Text = rtxt_SqlScript.Text & Chr(13)
        '                    Else
        '                        rtxt_SqlScript.Text = rtxt_SqlScript.Text & IIf(rtxt_SqlScript.Text <> "", Chr(13), "") & TblCreateQry & Chr(13)

        '                    End If
        '                    rtxt_SqlScript.Text = rtxt_SqlScript.Text & Chr(13)
        '                End If

        '                Rt1.MoveNext()
        '            Loop
        '        End If
        '        Rt1.Close()
        '        Rs = Nothing
        '        Rt1 = Nothing
        '        Rs1 = Nothing
        '        Rs2 = Nothing

        '        rtxt_SqlScript.Text = rtxt_SqlScript.Text & IIf(rtxt_SqlScript.Text <> "", Chr(13), "") & "End Sub" & Chr(13)

        '    End Sub

        '    Private Sub Smart_DataType_Creation()

        '        rtxt_SqlScript.Text = rtxt_SqlScript.Text & IIf(rtxt_SqlScript.Text <> "", Chr(13), "") & "    pvl_cn.execute """ & "EXEC sp_addtype N'PKey_Smart', N'varchar (25)', N'not null'""" & Chr(13)

        '        rtxt_SqlScript.Text = rtxt_SqlScript.Text & IIf(rtxt_SqlScript.Text <> "", Chr(13), "") & "    pvl_cn.execute """ & "EXEC sp_addtype N'Count_Smart', N'varchar (20)', N'null'""" & Chr(13)
        '        rtxt_SqlScript.Text = rtxt_SqlScript.Text & IIf(rtxt_SqlScript.Text <> "", Chr(13), "") & "    pvl_cn.execute """ & "EXEC sp_addtype N'Weight_Smart', N'numeric(18,3)', N'null'""" & Chr(13)
        '        rtxt_SqlScript.Text = rtxt_SqlScript.Text & IIf(rtxt_SqlScript.Text <> "", Chr(13), "") & "    pvl_cn.execute """ & "EXEC sp_addtype N'Master_Smart', N'varchar (50)', N'null'""" & Chr(13)

        '        rtxt_SqlScript.Text = rtxt_SqlScript.Text & IIf(rtxt_SqlScript.Text <> "", Chr(13), "") & "    pvl_cn.execute """ & "EXEC sp_addtype N'Meters_Smart', N'numeric(18,2)', N'null'""" & Chr(13)
        '        rtxt_SqlScript.Text = rtxt_SqlScript.Text & IIf(rtxt_SqlScript.Text <> "", Chr(13), "") & "    pvl_cn.execute """ & "EXEC sp_addtype N'EndsCount_Smart', N'varchar (30)', N'null'""" & Chr(13)
        '        rtxt_SqlScript.Text = rtxt_SqlScript.Text & IIf(rtxt_SqlScript.Text <> "", Chr(13), "") & "    pvl_cn.execute """ & "EXEC sp_addtype N'Amount_Smart', N'numeric(18,2)', N'null'""" & Chr(13)

        '    End Sub


        '    Private Function Creation_Table(ByVal Tbl_Idno As Integer) As String
        '        Dim Rt1 As ADODB.Recordset, Rt2 As ADODB.Recordset, Rt3 As ADODB.Recordset
        '        Dim Rs1 As ADODB.Recordset, Rs2 As ADODB.Recordset
        '        Dim DbAr(1000, 20) As String
        '        Dim PKeyCode As String, TblCreateStr As String, DefConsNm As String, DefConsVal As String, KeyColNms As String
        '        Dim TblName As String, FldDatTyp As String, Nul_NtNul_Sts As String, FldIdentity_Sts As String
        '        Dim AryInd As Long
        '        Dim New_Code As String, DBName As String, DBMethod As String
        '        Dim Ind As Integer, MaxInd As Integer, Blk_Idno As Integer, Col_Indx As Integer
        '        Dim i As Integer



        '        Rt1 = New ADODB.Recordset
        '        Rs1 = New ADODB.Recordset
        '        Rs2 = New ADODB.Recordset

        '        '====================  Storing All the Fileds in a Tempory Array
        '        Erase DbAr
        '        Ind = 0 : Blk_Idno = 0
        '        PKeyCode = "" : TblName = ""
        '        Rs1.Open("Select a.*, b.Other_Software_Field_Name from Other_Software_Table_Field_Details a, Other_Software_Field_Head b where a.Other_Software_Table_Idno = " & Str(Tbl_Idno) & " and a.Other_Software_Field_Idno = b.Other_Software_Field_Idno Order by a.Sl_No", Con, adOpenStatic, adLockReadOnly)
        '        If Not (Rs1.BOF And Rs1.EOF) Then
        '            Rs1.MoveFirst()
        '            Do While Not Rs1.EOF

        '                If Rs1!Other_Software_Field_Name = "Accounts_Status" Then
        '                    Debug.Print(Rs1!Other_Software_Field_Name)
        '                End If

        '                FldDatTyp = Rs1!Field_Data_type
        '                If Trim(UCase(Rs1!Field_Data_type)) = "VARCHAR" Then
        '                    FldDatTyp = Rs1!Field_Data_type & "(" & IIf(Val(Rs1!Field_Precision_Value) > 0, Val(Rs1!Field_Precision_Value), 50) & ")"
        '                ElseIf Trim(UCase(Rs1!Field_Data_type)) = "NUMERIC" Then
        '                    FldDatTyp = Rs1!Field_Data_type & "(18," & IIf(Val(Rs1!Field_Scale_Value) > 0, Val(Rs1!Field_Scale_Value), 4) & ")"
        '                End If

        '                Nul_NtNul_Sts = "NOT NULL"
        '                If Rs1!Field_Is_AllowNull = "YES" Then Nul_NtNul_Sts = "NULL"

        '                FldIdentity_Sts = ""
        '                If Rs1!Field_Is_Identity = "YES" Then FldIdentity_Sts = "IDENTITY"

        '                Ind = Ind + 1
        '                DbAr(Ind, 1) = Rs1!Other_Software_Table_Name
        '                DbAr(Ind, 2) = "FIELD"
        '                DbAr(Ind, 15) = Rs1!Other_Software_Field_Name
        '                DbAr(Ind, 3) = Trim(Rs1!Other_Software_Field_Name)
        '                DbAr(Ind, 4) = FldDatTyp
        '                DbAr(Ind, 5) = Nul_NtNul_Sts
        '                DbAr(Ind, 6) = Rs1!Field_Default_Value
        '                DbAr(Ind, 7) = FldIdentity_Sts

        '                TblName = Rs1!Other_Software_Table_Name

        '                Rs1.MoveNext()

        '                If Rs1.EOF Then
        '                            GoSub Get_Primary_Unique_Constraint_Fields
        '                ElseIf TblName <> Rs1!Other_Software_Table_Name Then
        '                            GoSub Get_Primary_Unique_Constraint_Fields
        '                End If

        '            Loop
        '        End If
        '        Rs1.Close()

        '        '====================  Table Creation
        '        MaxInd = Ind
        '        TblName = ""
        '        For Ind = 1 To MaxInd

        '            If Trim(DbAr(Ind, 1)) <> "" Then

        '                If TblName <> DbAr(Ind, 1) Then

        '                            If Trim(TblName) <> "" Then GoSub TableCreationn

        '                    TblCreateStr = "CREATE TABLE [" & Trim(DbAr(Ind, 1)) & "] ( "

        '                Else
        '                    TblCreateStr = TblCreateStr & " ,  "

        '                End If

        '                If DbAr(Ind, 2) = "FIELD" Then
        '                    TblCreateStr = TblCreateStr & " [" & DbAr(Ind, 3) & "] " & DbAr(Ind, 4) & " "

        '                    If Trim(DbAr(Ind, 7)) = "IDENTITY" Then
        '                        TblCreateStr = TblCreateStr & " IDENTITY(1, 1) NOT NULL"

        '                    Else
        '                        TblCreateStr = TblCreateStr & "  " & DbAr(Ind, 5)


        '                        If Trim(DbAr(Ind, 6)) <> "" Then  '--

        '                            DefConsNm = "[DF_" & Replace(Trim(DbAr(Ind, 1)), "_", "") & "_" & Replace(Trim(DbAr(Ind, 3)), "_", "") & "]"

        '                            If IsNumeric(DbAr(Ind, 6)) = True Then
        '                                If InStr(1, Trim(DbAr(Ind, 6)), "(") > 0 And InStr(1, Trim(DbAr(Ind, 6)), ")") > 0 Then
        '                                    DefConsVal = Trim(DbAr(Ind, 6))
        '                                Else
        '                                    DefConsVal = "(" & Trim(Val(DbAr(Ind, 6))) & ")"
        '                                End If
        '                            Else
        '                                If InStr(1, Trim(DbAr(Ind, 6)), "'") > 0 Then
        '                                    DefConsVal = "(" & Trim(DbAr(Ind, 6)) & ")"
        '                                Else
        '                                    DefConsVal = "('" & Trim(DbAr(Ind, 6)) & "')"
        '                                End If
        '                            End If

        '                            TblCreateStr = TblCreateStr & "  CONSTRAINT " & DefConsNm & " DEFAULT " & DefConsVal

        '                        End If

        '                    End If

        '                ElseIf DbAr(Ind, 2) = "KEY" Then

        '                    KeyColNms = ""
        '                    If Trim(DbAr(Ind, 5)) <> "" Then KeyColNms = DbAr(Ind, 5)
        '                    If Trim(DbAr(Ind, 6)) <> "" Then KeyColNms = KeyColNms & IIf(KeyColNms <> "", ", ", "") & DbAr(Ind, 6)
        '                    If Trim(DbAr(Ind, 7)) <> "" Then KeyColNms = KeyColNms & IIf(KeyColNms <> "", ", ", "") & DbAr(Ind, 7)
        '                    If Trim(DbAr(Ind, 8)) <> "" Then KeyColNms = KeyColNms & IIf(KeyColNms <> "", ", ", "") & DbAr(Ind, 8)
        '                    If Trim(DbAr(Ind, 9)) <> "" Then KeyColNms = KeyColNms & IIf(KeyColNms <> "", ", ", "") & DbAr(Ind, 9)
        '                    If Trim(DbAr(Ind, 10)) <> "" Then KeyColNms = KeyColNms & IIf(KeyColNms <> "", ", ", "") & DbAr(Ind, 10)
        '                    If Trim(DbAr(Ind, 11)) <> "" Then KeyColNms = KeyColNms & IIf(KeyColNms <> "", ", ", "") & DbAr(Ind, 11)
        '                    If Trim(DbAr(Ind, 12)) <> "" Then KeyColNms = KeyColNms & IIf(KeyColNms <> "", ", ", "") & DbAr(Ind, 12)
        '                    If Trim(DbAr(Ind, 13)) <> "" Then KeyColNms = KeyColNms & IIf(KeyColNms <> "", ", ", "") & DbAr(Ind, 13)
        '                    If Trim(DbAr(Ind, 14)) <> "" Then KeyColNms = KeyColNms & IIf(KeyColNms <> "", ", ", "") & DbAr(Ind, 14)

        '                    If Trim(KeyColNms) <> "" Then
        '                        TblCreateStr = TblCreateStr & "  CONSTRAINT [" & DbAr(Ind, 3) & "] " & DbAr(Ind, 4) & " NONCLUSTERED (" & KeyColNms & ") ON [PRIMARY]"
        '                    End If

        '                ElseIf DbAr(Ind, 2) = "CONSTRAINT" Then

        '                    TblCreateStr = TblCreateStr & "  CONSTRAINT [" & DbAr(Ind, 3) & "] CHECK ( " & DbAr(Ind, 4) & " )"

        '                End If

        '            End If

        '            TblName = DbAr(Ind, 1)

        '        Next

        '                GoSub TableCreationn

        '        Rt1 = Nothing
        '        Rs1 = Nothing
        '        Rs2 = Nothing


        '        Exit Function

        '        '====================  Storing All the Primary , Unique & Constraint Fileds in a Tempory Array

        'Get_Primary_Unique_Constraint_Fields:

        '        Rt2 = New ADODB.Recordset
        '        Rt3 = New ADODB.Recordset
        '        Rt2.Open("Select a.*, b.Other_Software_Table_Name from Other_Software_Unique_Head a, Other_Software_Table_Head b where a.Other_Software_Table_Idno = " & Str(Tbl_Idno) & " and a.Other_Software_Table_Idno = b.Other_Software_Table_Idno Order by a.Other_Software_Unique_Idno", Con, adOpenStatic, adLockReadOnly)
        '        If Not (Rt2.BOF And Rt2.EOF) Then
        '            Rt2.MoveFirst()
        '            Do While Not Rt2.EOF

        '                Ind = Ind + 1
        '                DbAr(Ind, 1) = Trim(Rt2!Other_Software_Table_Name)     ' Other_Software_Table_IdnoToName(Tbl_Idno)
        '                DbAr(Ind, 2) = "KEY"
        '                DbAr(Ind, 3) = Rt2!Other_Software_Unique_Name
        '                DbAr(Ind, 4) = Rt2!PrimaryKey_Unique
        '                DbAr(Ind, 5) = ""
        '                DbAr(Ind, 6) = ""
        '                DbAr(Ind, 7) = ""
        '                DbAr(Ind, 8) = ""
        '                DbAr(Ind, 9) = ""
        '                DbAr(Ind, 10) = ""
        '                DbAr(Ind, 11) = ""
        '                DbAr(Ind, 12) = ""
        '                DbAr(Ind, 13) = ""
        '                DbAr(Ind, 14) = ""

        '                Col_Indx = 4
        '                Rt3.Open("Select a.*, b.Other_Software_Field_Name from Other_Software_Unique_Details a, Other_Software_Field_Head b where a.Other_Software_Unique_Idno  = " & Str(Rt2!Other_Software_Unique_Idno) & " and a.Other_Software_Field_Idno = b.Other_Software_Field_Idno Order by a.Sl_No", Con, adOpenStatic, adLockReadOnly)
        '                If Not (Rt3.BOF And Rt3.EOF) Then
        '                    Rt3.MoveFirst()
        '                    Do While Not Rt3.EOF
        '                        Col_Indx = Col_Indx + 1
        '                        DbAr(Ind, Col_Indx) = Trim(Rt3!Other_Software_Field_Name)
        '                        Rt3.MoveNext()
        '                    Loop
        '                End If
        '                Rt3.Close()

        '                Rt2.MoveNext()

        '            Loop
        '        End If
        '        Rt2.Close()

        '        Rt2.Open("Select a.*, b.Other_Software_Table_Name from Other_Software_Table_Constraint_Details a, Other_Software_Table_Head b where a.Other_Software_Table_Idno = " & Str(Tbl_Idno) & " and a.Other_Software_Table_Idno = b.Other_Software_Table_Idno Order by a.Sl_No", Con, adOpenStatic, adLockReadOnly)
        '        If Not (Rt2.BOF And Rt2.EOF) Then
        '            Rt2.MoveFirst()
        '            Do While Not Rt2.EOF

        '                Ind = Ind + 1
        '                DbAr(Ind, 1) = Rt2!Other_Software_Table_Name   ' &  " Other_Software_Table_IdnoToName(Tbl_Idno)"
        '                DbAr(Ind, 2) = "CONSTRAINT"
        '                DbAr(Ind, 3) = Rt2!CONSTRAINT_NAME
        '                DbAr(Ind, 4) = Rt2!Table_Constraint

        '                For i = 1 To UBound(DbAr)
        '                    If Trim(DbAr(i, 1)) = "" Then Exit For

        '                    If Trim(DbAr(i, 15)) <> "" Then
        '                        If InStr(1, LCase(DbAr(Ind, 4)), "[" & LCase(DbAr(i, 15)) & "]") > 0 Then
        '                            DbAr(Ind, 4) = Replace(LCase(DbAr(Ind, 4)), "[" & LCase(DbAr(i, 15)) & "]", "[" & DbAr(i, 3) & "]")
        '                        End If
        '                    End If

        '                Next i

        '                Rt2.MoveNext()

        '            Loop
        '        End If
        '        Rt2.Close()
        '        Rt2 = Nothing
        '        Rt3 = Nothing
        '        Return

        'TableCreationn:
        '        If Trim(TblCreateStr) <> "" Then
        '            TblCreateStr = TblCreateStr & "  ) ON [PRIMARY]"
        '            AryInd = AryInd + 1
        '            Creation_Table = "    pvl_cn.execute """ & TblCreateStr & ""
        '            TblCreateStr = ""
        '        End If
        '        Return

        '    End Function


        '    Public Function Wrap_the_Query(ByVal Qry As String) As String()
        '        Dim ArQry() As String
        '        Dim i As Integer, m1 As Integer

        '        m1 = -1

        '        Do

        '            For i = 1000 To 1 Step -1
        '                If Mid$(Qry, i, 1) = "," Then Exit For
        '            Next i

        '            If i = 0 Then i = 1000

        '            m1 = m1 + 1

        '        ReDim Preserve ArQry(Val(m1 + 1)) As String

        '            ArQry(m1) = Left$(Qry, i) & " "" & _"

        '            Qry = Space(16) & """ " & Right$(Qry, Len(Qry) - i)

        '        Loop While Len(Qry) > 1000

        '        If Trim(Qry) <> "" Then
        '            m1 = m1 + 1
        '        ReDim Preserve ArQry(Val(m1 + 1)) As String
        '            ArQry(m1) = Qry
        '        End If

        '        Wrap_the_Query = ArQry

        '    End Function



    End Sub



End Class