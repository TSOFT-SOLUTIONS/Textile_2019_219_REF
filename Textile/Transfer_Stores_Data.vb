Imports System.IO

Public Class Transfer_Stores_Data

    Private CnTo As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private CnFrm As New SqlClient.SqlConnection

    Private Sub Transfer_Master_Ledgers_From_Smart_TextileNT9_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        If cbo_DBFrom.Enabled And cbo_DBFrom.Visible Then cbo_DBFrom.Focus()
    End Sub

    Private Sub Transfer_Master_Ledgers_From_Smart_TextileNT9_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(cbo_DBFrom, CnTo, "master..sysdatabases", "name", "(name LIKE 'tsoft%store%')", "")

        cbo_DBFrom.Text = ""
        Me.Text = "STORES DATAS TRANSFER"

    End Sub

    Private Sub btn_Transfer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Transfer.Click
        Dim tr As SqlClient.SqlTransaction

        If Trim(cbo_DBFrom.Text) = "" Then
            MessageBox.Show("Invalid Database Name", "DOES NOT TRANSFER...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_DBFrom.Enabled Then cbo_DBFrom.Focus()
            Exit Sub
        End If

        If MessageBox.Show("All the master datas will lost " & Chr(13) & "Are you sure you want to Transfer?", "FOR MASTER TRANSFER...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

        MDIParent1.Cursor = Cursors.WaitCursor
        Me.Cursor = Cursors.WaitCursor

        btn_Transfer.Enabled = False
        Me.Text = ""

        CnFrm = New SqlClient.SqlConnection("Data Source=" & Trim(Common_Procedures.ServerName) & ";Initial Catalog=" & Trim(cbo_DBFrom.Text) & ";User ID=sa;Password=" & Trim(Common_Procedures.ServerPassword) & ";Integrated Security=False")

        CnFrm.Open()
        CnTo.Open()

        tr = CnTo.BeginTransaction

        'Try

        BrandHead_Transfer(tr)

        DepartmentHead_Transfer(tr)

        ReedWidthHead_Transfer(tr)

        MachineHead_Transfer(tr)

        UnitHead_Transfer(tr)

        ItemHead_Transfer(tr)

        AreaHead_Transfer(tr)

        LedgerHead_Transfer(tr)

        CountHead_Transfer(tr)

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1037" Then '---- Prakash Textiles (Somanur)
            PurchaseOrderHead_Transfer(tr)

            PurchaseOrderDetails_Transfer(tr)

            PurchaseInwardHead_Transfer(tr)

            PurchaseInwardDetails_Transfer(tr)

            PurchaseReturnHead_Transfer(tr)

            PurchaseReturnDetails_Transfer(tr)

            ItemIssueToMachineHead_Transfer(tr)

            ItemIssueToMachineDetails_Transfer(tr)

            ItemReturnFromMachineHead_Transfer(tr)

            ItemReturnFromMachineDetails_Transfer(tr)

            ItemDeliveryHead_Transfer(tr)

            ItemDeliveryDetails_Transfer(tr)

            ItemReceiptHead_Transfer(tr)

            ItemReceiptDetails_Transfer(tr)

            ServiceItemDeliveryHead_Transfer(tr)

            ServiceItemDeliveryHead_Transfer(tr)

            ServiceItemReceiptHead_Transfer(tr)

            ServiceItemReceiptDetails_Transfer(tr)

            GatePassDetails_Transfer(tr)

            GatePassHead_Transfer(tr)

            ItemExcessShortHead_Transfer(tr)

            ItemDisposeHead_Transfer(tr)

            OilServiceHead_Transfer(tr)


        End If

        tr.Commit()

        Me.Text = "STORES DATAS TRANSFER"

        MDIParent1.Cursor = Cursors.Default
        Me.Cursor = Cursors.Default

        MessageBox.Show("All STORES DATAS Transfered Sucessfully", "FOR STORES DATAS TRANSFER...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        btn_Transfer.Enabled = True

        'Catch ex As Exception

        '    tr.Rollback()
        '    Me.Text = "STORES DATAS TRANSFER"
        '    MDIParent1.Cursor = Cursors.Default
        '    Me.Cursor = Cursors.Default
        '    btn_Transfer.Enabled = True
        '    MessageBox.Show(ex.Message, "INVALID TRANSFER...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        'Finally

        '    CnFrm.Close()
        '    CnTo.Close()
        '    tr.Dispose()

        '    btn_Transfer.Enabled = True
        '    Me.Text = "STORES DATA TRANSFER"

        '    MDIParent1.Cursor = Cursors.Default
        '    Me.Cursor = Cursors.Default

        'End Try

    End Sub

    Private Sub AreaHead_Transfer(ByVal sqltr As SqlClient.SqlTransaction)
        Dim CmdFrm As New SqlClient.SqlCommand
        Dim CmdTo As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Da2 As New SqlClient.SqlDataAdapter
        Dim Dt2 As New DataTable
        Dim vArea_IdNo As Integer, vOldLID As Integer, vArea_Old_IdNo As Integer
        Dim vArea_Name As String, vSur_Name As String
        Dim vSurNm As String

        Me.Text = "Area_Head"

        CmdFrm.Connection = CnFrm
        CmdTo.Connection = CnTo
        CmdTo.Transaction = sqltr

        vArea_IdNo = Common_Procedures.get_MaxIdNo(CnTo, "Area_Head", "Area_IdNo", "", sqltr)

        Da1 = New SqlClient.SqlDataAdapter("select * from Area_Head where Area_Name <> '' Order by Area_Name", CnFrm)
        Dt1 = New DataTable
        Da1.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then

            For I = 0 To Dt1.Rows.Count - 1

                Me.Text = "Area_Head  -  " & I

                If Trim(Dt1.Rows(I).Item("Area_Name").ToString) <> "" Then

                    vSurNm = Common_Procedures.Remove_NonCharacters(Dt1.Rows(I).Item("Area_Name").ToString)

                    vOldLID = Val(Common_Procedures.get_FieldValue(CnTo, "Area_Head", "Area_IdNo", "(Sur_Name = '" & Trim(vSurNm) & "')", , sqltr))

                    If vOldLID = 0 Then

                        vArea_IdNo = Val(vArea_IdNo) + 1

                        vArea_Old_IdNo = Val(Dt1.Rows(I).Item("Area_IdNo").ToString)

                        vArea_Name = Replace(Dt1.Rows(I).Item("Area_Name").ToString, "'", "")

                        vSur_Name = Common_Procedures.Remove_NonCharacters(vArea_Name)

                        'Da2 = New SqlClient.SqlDataAdapter("select AREA_NAME from Area_Head Where Sur_Name = '" & Trim(vSur_Name) & "'", CnFrm)
                        'Dt2 = New DataTable
                        'Da2.Fill(Dt2)

                        'If Dt2.Rows.Count < 0 Then
                        CmdTo.CommandText = "Insert into Area_Head ( Area_Idno        ,            Area_Name      ,            Sur_Name    , Old_Area_Idno ) " & _
                                       "       Values (" & Str(Val(vArea_IdNo)) & ", '" & Trim(vArea_Name) & "', '" & Trim(vSur_Name) & "' , " & Str(Val(vArea_Old_IdNo)) & " ) "
                        CmdTo.ExecuteNonQuery()
                        'End If

                        Dt2.Clear()
                        Da2.Dispose()

                    Else

                        vArea_Old_IdNo = Val(Dt1.Rows(I).Item("Area_Idno").ToString)

                        CmdTo.CommandText = "Update Area_Head set Old_Area_Idno = " & Str(Val(vArea_Old_IdNo)) & " where Area_Idno  = " & Str(Val(vOldLID)) & ""
                        CmdTo.ExecuteNonQuery()

                    End If

                End If

            Next

        End If

        Me.Text = ""

    End Sub

    Private Sub UnitHead_Transfer(ByVal sqltr As SqlClient.SqlTransaction)
        Dim CmdFrm As New SqlClient.SqlCommand
        Dim CmdTo As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim vUnit_IdNo As Integer, vOldLID As Integer, vUnit_OldLID As Integer
        Dim vUnit_Name As String, vSur_Name As String
        Dim vRackNm As String

        Me.Text = "Unit_Head"

        CmdFrm.Connection = CnFrm
        CmdTo.Connection = CnTo
        CmdTo.Transaction = sqltr


        vUnit_IdNo = Common_Procedures.get_MaxIdNo(CnTo, "Unit_Head", "Unit_IdNo", "", sqltr)

        Da1 = New SqlClient.SqlDataAdapter("select * from Unit_Head where Unit_Name <> '' Order by Unit_Name", CnFrm)
        Dt1 = New DataTable
        Da1.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then

            For I = 0 To Dt1.Rows.Count - 1

                Me.Text = "Unit_Head  -  " & I

                vRackNm = Common_Procedures.Remove_NonCharacters(Dt1.Rows(I).Item("Unit_Name").ToString)
                vOldLID = Val(Common_Procedures.get_FieldValue(CnTo, "Unit_Head", "Unit_IdNo", "(Sur_Name = '" & Trim(vRackNm) & "')", , sqltr))

                If vOldLID = 0 Then

                    vUnit_IdNo = Val(vUnit_IdNo) + 1

                    vUnit_OldLID = Val(Dt1.Rows(I).Item("Unit_IdNo").ToString)

                    vUnit_Name = Replace(Dt1.Rows(I).Item("Unit_Name").ToString, "'", "")

                    vSur_Name = Common_Procedures.Remove_NonCharacters(vUnit_Name)

                    CmdTo.CommandText = "Insert into Unit_Head ( Unit_IdNo        ,            Unit_Name        ,            Sur_Name       , Old_Unit_Idno  ) " & _
                                        "       Values (" & Str(Val(vUnit_IdNo)) & ", '" & Trim(vUnit_Name) & "', '" & Trim(vSur_Name) & "' , " & Str(Val(vUnit_OldLID)) & "  ) "
                    CmdTo.ExecuteNonQuery()

                End If

            Next

        End If

        Me.Text = ""

    End Sub

    Private Sub CountHead_Transfer(ByVal sqltr As SqlClient.SqlTransaction)
        Dim CmdFrm As New SqlClient.SqlCommand
        Dim CmdTo As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Da2 As New SqlClient.SqlDataAdapter
        Dim Dt2 As New DataTable
        Dim vCount_IdNo As Integer, vOldLID As Integer, vCount_Old_IdNo As Integer, vCount_StkUn_IdNo As Integer
        Dim vCount_Name As String, vSur_Name As String, vCnt_Desc As String
        Dim vSurNm As String
        Dim vResultan_Cnt As Double

        Me.Text = "Count_Head"

        CmdFrm.Connection = CnFrm
        CmdTo.Connection = CnTo
        CmdTo.Transaction = sqltr

        vCount_IdNo = Common_Procedures.get_MaxIdNo(CnTo, "Count_Head", "Count_IdNo", "", sqltr)

        Da1 = New SqlClient.SqlDataAdapter("select * from Count_Head where Count_Name <> '' Order by Count_Name ", CnFrm)
        Dt1 = New DataTable
        Da1.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then

            For I = 0 To Dt1.Rows.Count - 1

                Me.Text = "Count_Head  -  " & I

                If Trim(Dt1.Rows(I).Item("Count_Name").ToString) <> "" Then

                    vSurNm = Common_Procedures.Remove_NonCharacters(Dt1.Rows(I).Item("Count_Name").ToString)

                    vOldLID = Val(Common_Procedures.get_FieldValue(CnTo, "Count_Head", "Count_IdNo", "(Sur_Name = '" & Trim(vSurNm) & "')", , sqltr))

                    If vOldLID = 0 Then

                        vCount_IdNo = Val(vCount_IdNo) + 1

                        vCount_Old_IdNo = Val(Dt1.Rows(I).Item("Count_IdNo").ToString)

                        vCount_Name = Replace(Dt1.Rows(I).Item("Count_Name").ToString, "'", "")

                        vSur_Name = Common_Procedures.Remove_NonCharacters(vCount_Name)

                        vCnt_Desc = Dt1.Rows(I).Item("Count_Description").ToString
                        vCount_StkUn_IdNo = Val(Dt1.Rows(I).Item("Count_StockUnder_IdNo").ToString)
                        vResultan_Cnt = Val(Dt1.Rows(I).Item("Resultant_Count").ToString)
                        'vCnt_Poly_Jari = Dt1.Rows(I).Item("Cotton_Polyester_Jari").ToString
                        'vRTE_kG = Val(Dt1.Rows(I).Item("Rate_Kg").ToString)

                        CmdTo.CommandText = "Insert into Count_Head ( Count_Idno        ,            Count_Name      ,            Sur_Name       , Count_Description        , Count_StockUnder_IdNo             , Resultant_Count               ,  Old_Count_Idno                   ) " & _
                                           "       Values (" & Str(Val(vCount_IdNo)) & ", '" & Trim(vCount_Name) & "', '" & Trim(vSur_Name) & "' ,'" & Trim(vCnt_Desc) & "' ," & Str(Val(vCount_StkUn_IdNo)) & "," & Str(Val(vResultan_Cnt)) & ", " & Str(Val(vCount_Old_IdNo)) & " ) "
                        CmdTo.ExecuteNonQuery()
                      
                        Dt2.Clear()
                        Da2.Dispose()

                    Else

                        vCount_Old_IdNo = Val(Dt1.Rows(I).Item("Count_IdNo").ToString)

                        CmdTo.CommandText = "Update Count_Head set Old_Count_Idno = " & Str(Val(vCount_Old_IdNo)) & " where Count_IdNo  = " & Str(Val(vOldLID)) & ""
                        CmdTo.ExecuteNonQuery()

                    End If

                End If

            Next

        End If

        Me.Text = ""

    End Sub

    Private Sub BrandHead_Transfer(ByVal sqltr As SqlClient.SqlTransaction)
        Dim CmdFrm As New SqlClient.SqlCommand
        Dim CmdTo As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim vBrand_IdNo As Integer, vOldLID As Integer
        Dim vBrand_Name As String, vSur_Name As String
        Dim vSurNm As String

        Me.Text = "Brand_Head"

        CmdFrm.Connection = CnFrm
        CmdTo.Connection = CnTo
        CmdTo.Transaction = sqltr

        Da1 = New SqlClient.SqlDataAdapter("select * from Brand_Head Where Brand_NAME <> '' Order by Brand_NAME", CnFrm)
        Dt1 = New DataTable
        Da1.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then

            For I = 0 To Dt1.Rows.Count - 1

                Me.Text = "Brand_Head  -  " & I

                If Trim(Dt1.Rows(I).Item("Brand_Name").ToString) <> "" Then

                    vSurNm = Common_Procedures.Remove_NonCharacters(Dt1.Rows(I).Item("Brand_Name").ToString)

                    vOldLID = Val(Common_Procedures.get_FieldValue(CnTo, "Brand_Head", "Brand_idno", "(Sur_Name = '" & Trim(vSurNm) & "')", , sqltr))

                    If vOldLID = 0 Then

                        vBrand_IdNo = Val(Dt1.Rows(I).Item("Brand_idno").ToString)

                        vBrand_Name = Replace(Dt1.Rows(I).Item("Brand_Name").ToString, "'", "")

                        vSur_Name = Common_Procedures.Remove_NonCharacters(vBrand_Name)

                        CmdTo.CommandText = "Insert into Brand_Head ( Brand_Idno        ,            Brand_Name      ,            Sur_Name   ) " & _
                                            "       Values (" & Str(Val(vBrand_IdNo)) & ", '" & Trim(vBrand_Name) & "', '" & Trim(vSur_Name) & "') "
                        CmdTo.ExecuteNonQuery()

                    End If

                End If

            Next

        End If

        Me.Text = ""

    End Sub

    Private Sub DepartmentHead_Transfer(ByVal sqltr As SqlClient.SqlTransaction)
        Dim CmdFrm As New SqlClient.SqlCommand
        Dim CmdTo As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim Dt1 As New DataTable
        Dim vDepartment_IdNo As Integer, vOldLID As Integer
        Dim vDepartment_Name As String, vSur_Name As String
        Dim vSurNm As String

        Dim cmd As New SqlClient.SqlCommand

        Me.Text = "Department_Head"

        CmdFrm.Connection = CnFrm
        CmdTo.Connection = CnTo
        CmdTo.Transaction = sqltr

        Da1 = New SqlClient.SqlDataAdapter("select * from Department_Head Where Department_NAME <> '' Order by Department_NAME", CnFrm)
        Dt1 = New DataTable
        Da1.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then

            For I = 0 To Dt1.Rows.Count - 1

                Me.Text = "Department_Head  -  " & I

                If Trim(Dt1.Rows(I).Item("Department_Name").ToString) <> "" Then

                    vSurNm = Common_Procedures.Remove_NonCharacters(Dt1.Rows(I).Item("Department_Name").ToString)

                    vOldLID = Val(Common_Procedures.get_FieldValue(CnTo, "Department_Head", "Department_idno", "(Sur_Name = '" & Trim(vSurNm) & "')", , sqltr))

                    If vOldLID = 0 Then

                        vDepartment_IdNo = Val(Dt1.Rows(I).Item("Department_idno").ToString)

                        vDepartment_Name = Replace(Dt1.Rows(I).Item("Department_Name").ToString, "'", "")

                        vSur_Name = Common_Procedures.Remove_NonCharacters(vDepartment_Name)

                        vSurNm = Replace(Dt1.Rows(I).Item("Department_Name").ToString, "'", "")

                        'If IsDBNull(Dt1.Rows(I).Item("Department_Image")) = False Then
                        '    Dim imageData As Byte() = DirectCast(Dt1.Rows(I).Item("Department_Image"), Byte())
                        '    If Not imageData Is Nothing Then
                        '        Using msd As New MemoryStream(imageData, 0, imageData.Length)
                        '            msd.Write(imageData, 0, imageData.Length)
                        '            If imageData.Length > 0 Then

                        '                vDep_imge = Image.FromStream(msd)

                        '            End If
                        '        End Using
                        '    End If
                        'End If

                        'Dim ms As New MemoryStream()
                        'If IsNothing(vDep_imge) = False Then
                        '    Dim bitmp As New Bitmap(vDep_imge)
                        '    bitmp.Save(ms, Drawing.Imaging.ImageFormat.Jpeg)
                        'End If

                        'Dim data As Byte() = ms.GetBuffer()
                        'Dim p As New SqlClient.SqlParameter("@photo", SqlDbType.Image)
                        'p.Value = data
                        'cmd.Parameters.Add(p)
                        'ms.Dispose()

                        CmdTo.CommandText = "Insert into Department_Head ( Department_Idno        ,            Department_Name              ,            Sur_Name           ) " & _
                                            "                       Values (" & Str(Val(vDepartment_IdNo)) & ", '" & Trim(vDepartment_Name) & "', '" & Trim(vSur_Name) & "'     ) "
                        CmdTo.ExecuteNonQuery()

                    End If

                End If

            Next

        End If

        Me.Text = ""

    End Sub

    Private Sub ReedWidthHead_Transfer(ByVal sqltr As SqlClient.SqlTransaction)
        Dim CmdFrm As New SqlClient.SqlCommand
        Dim CmdTo As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim vReedWidth_IdNo As Integer, vOldLID As Integer
        Dim vReedWidth_Name As String, vSur_Name As String
        Dim vSurNm As String

        Me.Text = "ReedWidth_Head"

        CmdFrm.Connection = CnFrm
        CmdTo.Connection = CnTo
        CmdTo.Transaction = sqltr


        Da1 = New SqlClient.SqlDataAdapter("select * from ReedWidth_Head Where ReedWidth_Name <> '' Order by ReedWidth_Name", CnFrm)
        Dt1 = New DataTable
        Da1.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then

            For I = 0 To Dt1.Rows.Count - 1

                Me.Text = "ReedWidth_Head  -  " & I

                If Trim(Dt1.Rows(I).Item("ReedWidth_Name").ToString) <> "" Then

                    vSurNm = Common_Procedures.Remove_NonCharacters(Dt1.Rows(I).Item("ReedWidth_Name").ToString)

                    vOldLID = Val(Common_Procedures.get_FieldValue(CnTo, "ReedWidth_Head", "ReedWidth_Idno", "(Sur_Name = '" & Trim(vSurNm) & "')", , sqltr))

                    If vOldLID = 0 Then

                        vReedWidth_IdNo = Val(Dt1.Rows(I).Item("ReedWidth_Idno").ToString)

                        vReedWidth_Name = Replace(Dt1.Rows(I).Item("ReedWidth_Name").ToString, "'", "")

                        vSur_Name = Common_Procedures.Remove_NonCharacters(vReedWidth_Name)

                        CmdTo.CommandText = "Insert into ReedWidth_Head ( ReedWidth_Idno        ,            ReedWidth_Name      ,            Sur_Name   ) " & _
                                            "       Values (" & Str(Val(vReedWidth_IdNo)) & ", '" & Trim(vReedWidth_Name) & "', '" & Trim(vSur_Name) & "') "
                        CmdTo.ExecuteNonQuery()

                    End If

                End If

            Next

        End If

        Me.Text = ""

    End Sub

    Private Sub MachineHead_Transfer(ByVal sqltr As SqlClient.SqlTransaction)
        Dim CmdFrm As New SqlClient.SqlCommand
        Dim CmdTo As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim vMchn_IdNo As Integer, vOldLID As Integer
        Dim vmchn_Name As String, vSur_Name As String
        Dim vSurNm As String
        Dim vOsDy As Integer

        Me.Text = "Machine_head"

        CmdFrm.Connection = CnFrm
        CmdTo.Connection = CnTo
        CmdTo.Transaction = sqltr

        Da1 = New SqlClient.SqlDataAdapter("select * from Machine_head Where Machine_Name <> '' Order by Machine_Name", CnFrm)
        Dt1 = New DataTable
        Da1.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then

            For I = 0 To Dt1.Rows.Count - 1

                Me.Text = "Machine_head  -  " & I

                If Trim(Dt1.Rows(I).Item("Machine_Name").ToString) <> "" Then

                    vSurNm = Common_Procedures.Remove_NonCharacters(Dt1.Rows(I).Item("Machine_Name").ToString)

                    vOldLID = Val(Common_Procedures.get_FieldValue(CnTo, "Machine_head", "Machine_IdNo", "(Sur_Name = '" & Trim(vSurNm) & "')", , sqltr))

                    If vOldLID = 0 Then

                        vMchn_IdNo = Val(Dt1.Rows(I).Item("Machine_IdNo").ToString)

                        vmchn_Name = Replace(Dt1.Rows(I).Item("Machine_Name").ToString, "'", "")

                        vSur_Name = Common_Procedures.Remove_NonCharacters(vmchn_Name)

                        vOsDy = Val(Dt1.Rows(I).Item("Oil_Service_Day").ToString)

                        CmdTo.CommandText = "Insert into Machine_head ( Machine_IdNo        ,            Machine_Name      ,      Sur_Name        ,    Oil_Service_Day  ) " & _
                                            "       Values (" & Str(Val(vMchn_IdNo)) & ", '" & Trim(vmchn_Name) & "', '" & Trim(vSur_Name) & "'   , " & Str(Val(vOsDy)) & ") "
                        CmdTo.ExecuteNonQuery()

                    End If

                End If

            Next

        End If

        Me.Text = ""

    End Sub

    Private Sub ItemHead_Transfer(ByVal sqltr As SqlClient.SqlTransaction)
        Dim CmdFrm As New SqlClient.SqlCommand
        Dim CmdTo As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim vItm_IdNo As Integer, vOldLID As Integer, vDep_IdNo As Integer, vRedCnt_idno As Integer, vRedWdth_idno As Integer, vunt_idno As Integer
        Dim vItm_Name As String, vSur_Name As String, vItm_DisName As String, vItm_Code As String, vItm_type As String, vDrw_NO As String
        Dim vSurNm As String
        Dim vMin_Stk As Single, vTax_per As Single, vRate As Single, vReOrd_Qty As Single, vRate_Old As Single, vRate_Scrp As Single
        Dim vOldUnit_SurNm As String = ""

        Me.Text = "Item_Head"

        CmdFrm.Connection = CnFrm
        CmdTo.Connection = CnTo
        CmdTo.Transaction = sqltr

        Da1 = New SqlClient.SqlDataAdapter("select * from Item_Head Where Item_Name <> '' Order by Item_Name", CnFrm)
        Dt1 = New DataTable
        Da1.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then

            For I = 0 To Dt1.Rows.Count - 1

                Me.Text = "Item_Head  -  " & I

                If Trim(Dt1.Rows(I).Item("Item_Name").ToString) <> "" Then

                    vSurNm = Common_Procedures.Remove_NonCharacters(Dt1.Rows(I).Item("Item_Name").ToString)

                    vOldLID = Val(Common_Procedures.get_FieldValue(CnTo, "Stores_Item_Head", "Item_IdNo", "(Sur_Name = '" & Trim(vSurNm) & "')", , sqltr))

                    If vOldLID = 0 Then

                        vItm_IdNo = Val(Dt1.Rows(I).Item("Item_IdNo").ToString)

                        vItm_Name = Replace(Dt1.Rows(I).Item("Item_Name").ToString, "'", "")

                        vSur_Name = Common_Procedures.Remove_NonCharacters(vItm_Name)

                        vItm_DisName = Dt1.Rows(I).Item("Item_DisplayName").ToString
                        vItm_Code = Dt1.Rows(I).Item("Item_Code").ToString
                        vItm_type = Dt1.Rows(I).Item("Item_Type").ToString
                        vDrw_NO = Dt1.Rows(I).Item("Drawing_No").ToString

                        vDep_IdNo = Val(Dt1.Rows(I).Item("Department_IdNo").ToString)
                        vRedCnt_idno = Val(Dt1.Rows(I).Item("ReedCount_IdNo").ToString)
                        vRedWdth_idno = Val(Dt1.Rows(I).Item("ReedWidth_IdNo").ToString)

                        vMin_Stk = Val(Dt1.Rows(I).Item("Minimum_Stock").ToString)
                        vTax_per = Val(Dt1.Rows(I).Item("Tax_Percentage").ToString)
                        vRate = Val(Dt1.Rows(I).Item("Rate").ToString)
                        vReOrd_Qty = Val(Dt1.Rows(I).Item("ReOrder_Quantity").ToString)
                        vRate_Old = Val(Dt1.Rows(I).Item("Rate_Old").ToString)
                        vRate_Scrp = Val(Dt1.Rows(I).Item("Rate_Scrap").ToString)


                        vOldUnit_SurNm = Val(Common_Procedures.get_FieldValue(CnFrm, "Unit_Head", "Sur_Name", "(Unit_IdNo = " & Str(Val(Val(Dt1.Rows(I).Item("Unit_IdNo").ToString))) & ")"))
                        'vunt_idno = Unit_OldIdNoToNewIdNo(CnTo, Val(Dt1.Rows(I).Item("Unit_IdNo").ToString), sqltr)
                        vunt_idno = Val(Common_Procedures.get_FieldValue(CnTo, "Unit_Head", "Unit_IdNo", "(Sur_Name = '" & Trim(vOldUnit_SurNm) & "')", , sqltr))



                        CmdTo.CommandText = "Insert into Stores_Item_Head ( Item_IdNo        ,            Item_Name      ,      Sur_Name               ,    Item_DisplayName        , Item_Code                 , Item_Type                  , Drawing_No               , Department_IdNo            , ReedCount_IdNo                  , ReedWidth_IdNo                 , Unit_IdNo                  , Minimum_Stock             , Tax_Percentage       , Rate                        , ReOrder_Quantity                , Rate_Old               ,          Rate_Scrap       ) " & _
                                            "       Values (" & Str(Val(vItm_IdNo)) & ", '" & Trim(vItm_Name) & "', '" & Trim(vSur_Name) & "'  , '" & Trim(vItm_DisName) & "' , '" & Trim(vItm_Code) & "' , '" & Trim(vItm_type) & "' , '" & Trim(vDrw_NO) & "' , " & Str(Val(vDep_IdNo)) & " ,  " & Str(Val(vRedCnt_idno)) & " , " & Str(Val(vRedWdth_idno)) & ", " & Str(Val(vunt_idno)) & ", " & Str(Val(vMin_Stk)) & ", " & Str(Val(vTax_per)) & ", " & Str(Val(vRate)) & ", " & Str(Val(vReOrd_Qty)) & ", " & Str(Val(vRate_Old)) & ", " & Str(Val(vRate_Scrp)) & ") "
                        CmdTo.ExecuteNonQuery()

                    End If

                End If

            Next

        End If

        CmdTo.CommandText = "delete from Stores_Item_AlaisHead "
        CmdTo.ExecuteNonQuery()

        Da1 = New SqlClient.SqlDataAdapter("select * from Stores_Item_Head Order by Item_IdNo", CnTo)
        Da1.SelectCommand.Transaction = sqltr
        Dt1 = New DataTable
        Da1.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then

            For i = 0 To Dt1.Rows.Count - 1
                CmdTo.CommandText = "Insert into Stores_Item_AlaisHead ( Item_IdNo, Sl_No, Item_DisplayName, Sur_Name, Department_IdNo, Drawing_No) Values (" & Str(Val(Dt1.Rows(i).Item("Item_IdNo").ToString)) & ", 1,  '" & Trim(Dt1.Rows(i).Item("Item_DisplayName").ToString) & "',   '" & Trim(Dt1.Rows(i).Item("Sur_Name").ToString) & "',  " & Str(Val(Dt1.Rows(i).Item("Department_IdNo").ToString)) & ",   '" & Trim(Dt1.Rows(i).Item("Drawing_No").ToString) & "' )"
                CmdTo.ExecuteNonQuery()
            Next

        End If

        CmdTo.Dispose()
        CmdFrm.Dispose()
        Dt1.Dispose()
        Da1.Dispose()

        Me.Text = ""

    End Sub

    Private Sub LedgerHead_Transfer(ByVal sqltr As SqlClient.SqlTransaction)
        Dim CmdFrm As New SqlClient.SqlCommand
        Dim CmdTo As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim vLedger_IdNo As Integer, vOldLID As Integer, vOld_Ledger_IdNo As Integer
        Dim vLedger_Name As String, vSur_Name As String, vLedger_MainName As String, vLedger_AlaisName As String
        Dim vArea_IdNo As Integer, vAccountsGroup_IdNo As String, vParent_Code As String
        Dim vLedger_Address1 As String, vLedger_Address2 As String, vLedger_Address3 As String, vLedger_Address4 As String, vLedger_PhoneNo As String
        Dim vLedger_TinNo As String, vLedger_CstNo As String, vLedger_Type As String
        'Dim vLedger_Emailid As String, vLedger_FaxNo As String, vLedger_MobileNo As String, vContact_Person As String
        'Dim vPackingType_CompanyIdNo As Integer, vLedger_AgentIdNo As Integer, vNote As String
        Dim vSurNm As String

        Me.Text = "Ledger_Head"

        CmdFrm.Connection = CnFrm
        CmdTo.Connection = CnTo
        CmdTo.Transaction = sqltr

        vLedger_IdNo = Common_Procedures.get_MaxIdNo(CnTo, "Ledger_Head", "Ledger_IdNo", "", sqltr)

        If Val(vLedger_IdNo) < 100 Then
            vLedger_IdNo = 100
        End If

        Da1 = New SqlClient.SqlDataAdapter("select * from Ledger_Head where Ledger_IdNo > 100 Order by Ledger_IdNo", CnFrm)
        Dt1 = New DataTable
        Da1.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then

            For I = 0 To Dt1.Rows.Count - 1

                Me.Text = "Ledger_Head  -  " & I

                vSurNm = Common_Procedures.Remove_NonCharacters(Dt1.Rows(I).Item("Ledger_Name").ToString)
                vOldLID = Val(Common_Procedures.get_FieldValue(CnTo, "Ledger_Head", "Ledger_IdNo", "(Sur_Name = '" & Trim(vSurNm) & "')", , sqltr))

                If vOldLID = 0 Then

                    vLedger_IdNo = vLedger_IdNo + 1

                    vOld_Ledger_IdNo = Val(Dt1.Rows(I).Item("Ledger_IdNo").ToString)

                    vLedger_Name = Replace(Dt1.Rows(I).Item("Ledger_Name").ToString, "'", "")
                    vLedger_MainName = vLedger_Name
                    vSur_Name = Common_Procedures.Remove_NonCharacters(vLedger_Name)
                    vLedger_AlaisName = ""

                    vArea_IdNo = 0
                    If Val(Dt1.Rows(I).Item("Area_Idno").ToString) <> 0 Then
                        vArea_IdNo = Val(Common_Procedures.get_FieldValue(CnTo, "Area_Head", "Area_IdNo", "(Area_IdNo = " & Val(Dt1.Rows(I).Item("Old_Area_IdNo").ToString) & ")", , sqltr))
                    End If

                    vParent_Code = Dt1.Rows(I).Item("Parent_Code").ToString

                    vAccountsGroup_IdNo = Common_Procedures.get_FieldValue(CnTo, "AccountsGroup_Head", "AccountsGroup_IdNo", "(Parent_Idno = '" & Trim(vParent_Code) & "')", , sqltr)

                    'If Trim(UCase(Dt1.Rows(I).Item("Bill_Maintenance").ToString)) = "BILL TO BILL CLEAR" Then
                    '    vBill_Type = "BILL TO BILL"
                    'Else
                    '    vBill_Type = "BALANCE ONLY"
                    'End If

                    vLedger_Address1 = Replace(Dt1.Rows(I).Item("Ledger_Address1").ToString, "'", "")
                    vLedger_Address2 = Replace(Dt1.Rows(I).Item("Ledger_Address2").ToString, "'", "")
                    vLedger_Address3 = Replace(Dt1.Rows(I).Item("Ledger_Address3").ToString, "'", "")
                    vLedger_Address4 = Replace(Dt1.Rows(I).Item("Ledger_Address4").ToString, "'", "")
                    vLedger_PhoneNo = Replace(Dt1.Rows(I).Item("Ledger_PhoneNo").ToString, "'", "")
                    vLedger_TinNo = Replace(Dt1.Rows(I).Item("Ledger_TinNo").ToString, "'", "")
                    vLedger_CstNo = Dt1.Rows(I).Item("Ledger_CstNo").ToString
                    vLedger_Type = Dt1.Rows(I).Item("Ledger_Type").ToString

                  
                    CmdTo.CommandText = "Insert into ledger_head ( Ledger_IdNo        ,            Ledger_Name      ,            Sur_Name      ,            Ledger_MainName      ,            Ledger_AlaisName      ,              Area_IdNo      ,              AccountsGroup_IdNo      ,            Parent_Code      ,     Ledger_Address1      ,            Ledger_Address2      ,            Ledger_Address3      ,            Ledger_Address4      ,            Ledger_PhoneNo      ,            Ledger_TinNo      ,            Ledger_CstNo      ,            Ledger_Type      ,      Verified_Status   , Old_Ledger_IdNo  ) " & _
                                        "       Values (" & Str(Val(vLedger_IdNo)) & ", '" & Trim(vLedger_Name) & "', '" & Trim(vSur_Name) & "', '" & Trim(vLedger_MainName) & "', '" & Trim(vLedger_AlaisName) & "', " & Str(Val(vArea_IdNo)) & ", " & Str(Val(vAccountsGroup_IdNo)) & ", '" & Trim(vParent_Code) & "',  '" & Trim(vLedger_Address1) & "', '" & Trim(vLedger_Address2) & "', '" & Trim(vLedger_Address3) & "', '" & Trim(vLedger_Address4) & "', '" & Trim(vLedger_PhoneNo) & "', '" & Trim(vLedger_TinNo) & "', '" & Trim(vLedger_CstNo) & "', '" & Trim(vLedger_Type) & "',  1                     ," & Str(Val(vOld_Ledger_IdNo)) & " ) "
                    CmdTo.ExecuteNonQuery()

                Else

                    vOld_Ledger_IdNo = Val(Dt1.Rows(I).Item("Ledger_IdNo").ToString)

                    CmdTo.CommandText = "Update ledger_head set Old_Ledger_IdNo = " & Str(Val(vOld_Ledger_IdNo)) & " where Ledger_IdNo  = " & Str(Val(vOldLID)) & ""

                    CmdTo.ExecuteNonQuery()

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
                    CmdTo.CommandText = "Insert into Ledger_AlaisHead ( Ledger_IdNo, Sl_No, Ledger_DisplayName, Ledger_Type, AccountsGroup_IdNo, Verified_Status ) Values (" & Str(Val(Dt1.Rows(i).Item("Ledger_IdNo").ToString)) & ",    1,      '" & Trim(Dt1.Rows(i).Item("Ledger_Name").ToString) & "',   '" & Trim(Dt1.Rows(i).Item("Ledger_Type").ToString) & "',    " & Str(Val(Dt1.Rows(i).Item("AccountsGroup_IdNo").ToString)) & ", 1 )"
                    CmdTo.ExecuteNonQuery()
                Next

            End If

            CmdTo.Dispose()
            Dt1.Dispose()
            Da1.Dispose()

        End If

        Me.Text = ""

    End Sub


    Private Sub PurchaseOrderHead_Transfer(ByVal sqltr As SqlClient.SqlTransaction)
        Dim CmdFrm As New SqlClient.SqlCommand
        Dim CmdTo As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim vCompany_IdNo As Integer, vLedger_IdNo As Integer
        Dim vPo_Code As String, vPo_No As String, vDelivery_Terms As String, vPayment_Terms As String, vExcise_Terms As String, vDelivery_Address1 As String, vDelivery_Address2 As String, vDelivery_Address3 As String, vRemarks As String
        Dim vTotal_Quantity As Single, vTotal_Amount As Single, vNet_Amount As Single

        Me.Text = "Item_PO_Head"

        CmdFrm.Connection = CnFrm
        CmdTo.Connection = CnTo
        CmdTo.Transaction = sqltr

        CmdTo.CommandText = "Delete from Stores_Item_PO_Head"
        CmdTo.ExecuteNonQuery()

        Da1 = New SqlClient.SqlDataAdapter("select * from Item_PO_Head Order by PO_No", CnFrm)
        Dt1 = New DataTable
        Da1.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then

            For I = 0 To Dt1.Rows.Count - 1

                Me.Text = "Item_PO_Head  -  " & I

                If Trim(Dt1.Rows(I).Item("PO_Code").ToString) <> "" Then


                    CmdTo.Parameters.Clear()
                    CmdTo.Parameters.AddWithValue("@PoDate", CDate(Dt1.Rows(I).Item("PO_Date").ToString))

                    vPo_Code = Dt1.Rows(I).Item("PO_Code").ToString
                    vPo_No = Dt1.Rows(I).Item("PO_No").ToString

                    vLedger_IdNo = Ledger_OldIdNoToNewIdNo(CnTo, Val(Dt1.Rows(I).Item("Ledger_IdNo").ToString), sqltr)

                    vCompany_IdNo = Dt1.Rows(I).Item("Company_IdNo").ToString

                    vDelivery_Address1 = Dt1.Rows(I).Item("Delivery_Address1").ToString
                    vDelivery_Address2 = Dt1.Rows(I).Item("Delivery_Address2").ToString
                    vDelivery_Address3 = Dt1.Rows(I).Item("Delivery_Address3").ToString

                    vDelivery_Terms = Dt1.Rows(I).Item("Delivery_Terms").ToString
                    vPayment_Terms = Dt1.Rows(I).Item("Payment_Terms").ToString
                    vExcise_Terms = Dt1.Rows(I).Item("Excise_Terms").ToString
                    vRemarks = Dt1.Rows(I).Item("Remarks").ToString

                    vTotal_Quantity = Val(Dt1.Rows(I).Item("Total_Quantity").ToString)
                    vTotal_Amount = Format(Val(Dt1.Rows(I).Item("Total_Amount").ToString), "########0.00")
                    vNet_Amount = Val(Dt1.Rows(I).Item("Net_Amount").ToString)

                    CmdTo.CommandText = "Insert into Stores_Item_PO_Head(  PO_Code             , Company_IdNo                      , PO_No                        , for_OrderBy                                                    , PO_Date , Ledger_IdNo                   , Delivery_Terms                 , Payment_Terms                 , Excise_Terms                 , Delivery_Address1                 , Delivery_Address2                 , Delivery_Address3                  , Remarks                 , Total_Quantity                 , Total_Amount                , Net_Amount)  " & _
                                                      " Values ( '" & Trim(vPo_Code) & "', " & Str(Val(vCompany_IdNo)) & ", '" & Trim(vPo_No) & "'         , " & Str(Val(Common_Procedures.OrderBy_CodeToValue(vPo_No))) & ", @PoDate , " & Str(Val(vLedger_IdNo)) & ", '" & Trim(vDelivery_Terms) & "', '" & Trim(vPayment_Terms) & "', '" & Trim(vExcise_Terms) & "', '" & Trim(vDelivery_Address1) & "', '" & Trim(vDelivery_Address2) & "', '" & Trim(vDelivery_Address3) & "', '" & Trim(vRemarks) & "', " & Str(Val(vTotal_Quantity)) & ", " & Str(Val(vTotal_Amount)) & ", " & Str(Val(CSng(vNet_Amount))) & " )"
                    CmdTo.ExecuteNonQuery()

                End If


            Next

        End If

        Me.Text = ""

    End Sub

    Private Sub PurchaseOrderDetails_Transfer(ByVal sqltr As SqlClient.SqlTransaction)
        Dim CmdFrm As New SqlClient.SqlCommand
        Dim CmdTo As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim vCompany_IdNo As Integer, vLedger_IdNo As Integer, vSl_No As Integer, vItem_Idno As Integer, vBrand_IdNo As Integer, vUnit_Idno As Integer, vPO_Details_SlNo As Integer
        Dim vPo_Code As String, vPo_No As String
        Dim vPO_Quantity As Single, vRate As Single, vAmount As Single, vCancel_Quantiy As Single, vPurchased_Quantity As Single, vPurchaseReturn_Quantity As Single

        Me.Text = "Item_PO_Details"

        CmdFrm.Connection = CnFrm
        CmdTo.Connection = CnTo
        CmdTo.Transaction = sqltr

        CmdTo.CommandText = "Delete from Stores_Item_PO_Details"
        CmdTo.ExecuteNonQuery()

        Da1 = New SqlClient.SqlDataAdapter("select * from Item_PO_Details Order by PO_No", CnFrm)
        Dt1 = New DataTable
        Da1.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then

            For I = 0 To Dt1.Rows.Count - 1

                Me.Text = "Item_PO_Details  -  " & I

                If Trim(Dt1.Rows(I).Item("PO_Code").ToString) <> "" Then

                    CmdTo.Parameters.Clear()
                    CmdTo.Parameters.AddWithValue("@PoDate", CDate(Dt1.Rows(I).Item("PO_Date").ToString))

                    vPo_Code = Dt1.Rows(I).Item("PO_Code").ToString
                    vPo_No = Dt1.Rows(I).Item("PO_No").ToString

                    vLedger_IdNo = Ledger_OldIdNoToNewIdNo(CnTo, Val(Dt1.Rows(I).Item("Ledger_IdNo").ToString), sqltr)
                    vCompany_IdNo = Dt1.Rows(I).Item("Company_IdNo").ToString

                    vSl_No = Dt1.Rows(I).Item("Sl_No").ToString
                    vItem_Idno = Dt1.Rows(I).Item("Item_Idno").ToString
                    vBrand_IdNo = Dt1.Rows(I).Item("Brand_Idno").ToString
                    vPO_Quantity = Val(Dt1.Rows(I).Item("PO_Quantity").ToString)
                    vUnit_Idno = Unit_OldIdNoToNewIdNo(CnTo, Val(Dt1.Rows(I).Item("Unit_Idno").ToString), sqltr)
                    vRate = Format(Val(Dt1.Rows(I).Item("Rate").ToString), "########0.00")
                    vAmount = Format(Val(Dt1.Rows(I).Item("Amount").ToString), "########0.00")
                    vCancel_Quantiy = Val(Dt1.Rows(I).Item("Cancel_Quantiy").ToString)
                    vPO_Details_SlNo = Val(Dt1.Rows(I).Item("PO_Details_SlNo").ToString)
                    vPurchased_Quantity = Val(Dt1.Rows(I).Item("Purchased_Quantity").ToString)
                    vPurchaseReturn_Quantity = Val(Dt1.Rows(I).Item("PurchaseReturn_Quantity").ToString)

                    CmdTo.CommandText = "Insert into Stores_Item_PO_Details ( PO_Code         , Company_IdNo                  , PO_No                          , for_OrderBy                                                   , PO_Date   , Ledger_IdNo                   , Sl_No                  , Item_IdNo                    , Brand_IdNo                    , PO_Quantity                 , Unit_idNo              , Rate                   , Amount                   , Cancel_Quantiy                        , Purchased_Quantity               , PurchaseReturn_Quantity )  " & _
                                                           " Values ( '" & Trim(vPo_Code) & "', " & Str(Val(vCompany_IdNo)) & ", '" & Trim(vPo_No) & "'         , " & Str(Val(Common_Procedures.OrderBy_CodeToValue(vPo_No))) & ", @PoDate , " & Str(Val(vLedger_IdNo)) & " , " & Str(Val(vSl_No)) & ", " & Str(Val(vItem_Idno)) & ", " & Str(Val(vBrand_IdNo)) & ", " & Str(Val(vPO_Quantity)) & ", " & Val(vUnit_Idno) & ", " & Str(Val(vRate)) & ", " & Str(Val(vAmount)) & ", " & Str(Val(vCancel_Quantiy)) & ",  " & Str(Val(vPurchased_Quantity)) & ",  " & Str(Val(vPurchaseReturn_Quantity)) & " )"
                    CmdTo.ExecuteNonQuery()

                End If


            Next

        End If

        Me.Text = ""

    End Sub

    Private Sub PurchaseInwardHead_Transfer(ByVal sqltr As SqlClient.SqlTransaction)
        Dim CmdFrm As New SqlClient.SqlCommand
        Dim CmdTo As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim vCompany_IdNo As Integer, vLedger_IdNo As Integer, vPurchaseAc_IdNo As Integer, vTaxAc_IdNo As Integer
        Dim vItem_Purchase_Code As String, vItem_Purchase_No As String, vEntry_Type As String, vBill_No As String, vTax_Type As String, vRemarks As String
        Dim vTotal_Quantity As Single, vTotal_Amount As Single, vNet_Amount As Single, vCashDisnt_Per As Single, vCashDiscount_Amount As Single, vAssessable_Value As Single, vTax_Percentage As Single, vTax_Amount As Single, vFreight_Amount As Single, vAddLess_Amount As Single, vRoundOff_Amount As Single

        Me.Text = "Item_Purchase_Head"

        CmdFrm.Connection = CnFrm
        CmdTo.Connection = CnTo
        CmdTo.Transaction = sqltr

        CmdTo.CommandText = "Delete from Stores_Item_Purchase_Head"
        CmdTo.ExecuteNonQuery()

        Da1 = New SqlClient.SqlDataAdapter("select * from Item_Purchase_Head  Order by Item_Purchase_No", CnFrm)
        Dt1 = New DataTable
        Da1.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then

            For I = 0 To Dt1.Rows.Count - 1

                Me.Text = "Item_Purchase_Head  -  " & I

                If Trim(Dt1.Rows(I).Item("Item_Purchase_Code").ToString) <> "" Then


                    CmdTo.Parameters.Clear()
                    CmdTo.Parameters.AddWithValue("@PurDate", CDate(Dt1.Rows(I).Item("Item_Purchase_Date").ToString))

                    vItem_Purchase_Code = Dt1.Rows(I).Item("Item_Purchase_Code").ToString
                    vItem_Purchase_No = Dt1.Rows(I).Item("Item_Purchase_No").ToString

                    vLedger_IdNo = Ledger_OldIdNoToNewIdNo(CnTo, Val(Dt1.Rows(I).Item("Ledger_IdNo").ToString), sqltr)
                    vCompany_IdNo = Dt1.Rows(I).Item("Company_IdNo").ToString


                    vEntry_Type = Dt1.Rows(I).Item("Entry_Type").ToString
                    vBill_No = Dt1.Rows(I).Item("Bill_No").ToString

                    vCashDisnt_Per = Val(Dt1.Rows(I).Item("CashDiscount_Percentage").ToString)
                    vCashDiscount_Amount = Format(Val(Dt1.Rows(I).Item("CashDiscount_Amount").ToString), "#########0.00")
                    vAssessable_Value = Format(Val(Dt1.Rows(I).Item("Assessable_Value").ToString), "#########0.00")

                    vTaxAc_IdNo = Dt1.Rows(I).Item("TaxAc_IdNo").ToString

                    vTax_Type = Dt1.Rows(I).Item("Tax_Type").ToString
                    If Trim(vTax_Type) = "" Then vTax_Type = "-NIL-"
                    vTax_Percentage = Val(Dt1.Rows(I).Item("Tax_Percentage").ToString)
                    vTax_Amount = Format(Val(Dt1.Rows(I).Item("Tax_Amount").ToString), "#########0.00")

                    vFreight_Amount = Format(Val(Dt1.Rows(I).Item("Freight_Amount").ToString), "#########0.00")
                    vAddLess_Amount = Format(Val(Dt1.Rows(I).Item("AddLess_Amount").ToString), "#########0.00")
                    vRoundOff_Amount = Format(Val(Dt1.Rows(I).Item("RoundOff_Amount").ToString), "#########0.00")

                    vRemarks = Dt1.Rows(I).Item("Remarks").ToString

                    vTotal_Quantity = Val(Dt1.Rows(I).Item("Total_Quantity").ToString)
                    vTotal_Amount = Format(Val(Dt1.Rows(I).Item("Total_Amount").ToString), "########0.00")
                    vNet_Amount = Val(Dt1.Rows(I).Item("Net_Amount").ToString)

                    CmdTo.CommandText = "Insert into Stores_Item_Purchase_Head(Item_Purchase_Code,             Company_IdNo         ,           Item_Purchase_No   ,                               for_OrderBy                                , Item_Purchase_Date,                 Entry_Type      ,           Ledger_IdNo     ,               Bill_No          ,           PurchaseAc_IdNo  ,        Total_Quantity    ,       Total_Amount       ,      CashDiscount_Percentage       ,       CashDiscount_Amount            ,            Assessable_Value               ,            TaxAc_IdNo    ,             Tax_Type            ,             Tax_Percentage        ,             Tax_Amount              ,               Freight_Amount       ,             AddLess_Amount    ,            RoundOff_Amount    ,    Net_Amount              ,       Remarks       ) " & _
                                    "              Values   ('" & Trim(vItem_Purchase_Code) & "', " & Str(Val(vCompany_IdNo)) & ", '" & Trim(vItem_Purchase_No) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(vItem_Purchase_No))) & ",     @PurDate     , '" & Trim(vEntry_Type) & "', " & Str(Val(vLedger_IdNo)) & ", '" & Trim(vBill_No) & "', " & Str(Val(vPurchaseAc_IdNo)) & ", " & Str(Val(vTotal_Quantity)) & ", " & Str(Val(vTotal_Amount)) & ", " & Str(Val(vCashDisnt_Per)) & ", " & Str(Val(vCashDiscount_Amount)) & ", " & Str(Val(vAssessable_Value)) & ", " & Str(Val(vTaxAc_IdNo)) & ", '" & Trim(vTax_Type) & "', " & Str(Val(vTax_Percentage)) & ", " & Str(Val(vTax_Amount)) & ", " & Str(Val(vFreight_Amount)) & ", " & Str(Val(vAddLess_Amount)) & ", " & Str(Val(vRoundOff_Amount)) & ", " & Str(Val(CSng(vNet_Amount))) & ", '" & Trim(vRemarks) & "' )"
                    CmdTo.ExecuteNonQuery()

                End If

            Next

        End If

        Me.Text = ""

    End Sub

    Private Sub PurchaseInwardDetails_Transfer(ByVal sqltr As SqlClient.SqlTransaction)
        Dim CmdFrm As New SqlClient.SqlCommand
        Dim CmdTo As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim vCompany_IdNo As Integer, vLedger_IdNo As Integer
        Dim vItem_Purchase_Code As String, vItem_Purchase_No As String, vEntry_Type As String, vBill_No As String
        Dim vSl_No As Integer, vItem_Idno As Integer, vBrand_IdNo As Integer, vUnit_Idno As Integer
        Dim vPo_Code As String, vPo_No As String
        Dim vQuantity As Single, vRate As Single, vAmount As Single, vPo_Details_SlNo As Single, vPurchaseReturn_Quantity As Single

        Me.Text = "Item_Purchase_Head"

        CmdFrm.Connection = CnFrm
        CmdTo.Connection = CnTo
        CmdTo.Transaction = sqltr

        CmdTo.CommandText = "Delete from Stores_Item_Purchase_Details"
        CmdTo.ExecuteNonQuery()

        Da1 = New SqlClient.SqlDataAdapter("select * from Item_Purchase_Details  Order by Item_Purchase_No", CnFrm)
        Dt1 = New DataTable
        Da1.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then

            For I = 0 To Dt1.Rows.Count - 1

                Me.Text = "Item_Purchase_Details  -  " & I

                If Trim(Dt1.Rows(I).Item("Item_Purchase_Code").ToString) <> "" Then


                    CmdTo.Parameters.Clear()
                    CmdTo.Parameters.AddWithValue("@PurDate", CDate(Dt1.Rows(I).Item("Item_Purchase_Date").ToString))

                    vItem_Purchase_Code = Dt1.Rows(I).Item("Item_Purchase_Code").ToString
                    vItem_Purchase_No = Dt1.Rows(I).Item("Item_Purchase_No").ToString

                    vLedger_IdNo = Ledger_OldIdNoToNewIdNo(CnTo, Val(Dt1.Rows(I).Item("Ledger_IdNo").ToString), sqltr)
                    vCompany_IdNo = Dt1.Rows(I).Item("Company_IdNo").ToString

                    vEntry_Type = Dt1.Rows(I).Item("Entry_Type").ToString
                    vBill_No = Dt1.Rows(I).Item("Bill_No").ToString

                    vSl_No = Dt1.Rows(I).Item("Sl_No").ToString
                    vItem_Idno = Dt1.Rows(I).Item("Item_Idno").ToString
                    vBrand_IdNo = Dt1.Rows(I).Item("Brand_Idno").ToString
                    vQuantity = Val(Dt1.Rows(I).Item("Quantity").ToString)
                    vUnit_Idno = Unit_OldIdNoToNewIdNo(CnTo, Val(Dt1.Rows(I).Item("Unit_IdNo").ToString), sqltr)
                    vRate = Format(Val(Dt1.Rows(I).Item("Rate").ToString), "########0.00")
                    vAmount = Format(Val(Dt1.Rows(I).Item("Amount").ToString), "########0.00")
                    vPo_No = Dt1.Rows(I).Item("Po_No").ToString
                    'vPurchase_Details_SlNo = Val(Dt1.Rows(I).Item("Purchase_Details_SlNo").ToString)
                    vPo_Code = Dt1.Rows(I).Item("Po_Code").ToString
                    vPo_Details_SlNo = Val(Dt1.Rows(I).Item("PO_Details_SlNo").ToString)
                    vPurchaseReturn_Quantity = Val(Dt1.Rows(I).Item("PurchaseReturn_Quantity").ToString)


                    CmdTo.CommandText = "Insert into Stores_Item_Purchase_Details ( Item_Purchase_Code                  , Company_IdNo                  ,               Item_Purchase_No,                                      for_OrderBy                   , Item_Purchase_Date  ,    Entry_Type             ,             Ledger_IdNo        ,        Bill_No          ,         Sl_No                , Item_IdNo              ,              Brand_IdNo     ,           Quantity          ,     Unit_idNo       ,            Rate           ,           Amount       ,        Po_No           ,            Po_Code        ,          PurchaseReturn_Quantity         ) " & _
                                                                         " Values ( '" & Trim(vItem_Purchase_Code) & "', " & Str(Val(vCompany_IdNo)) & ", '" & Trim(vItem_Purchase_No) & " ' , " & Str(Val(Common_Procedures.OrderBy_CodeToValue(vItem_Purchase_No))) & ", @PurDate            , '" & Trim(vEntry_Type) & "' , " & Str(Val(vLedger_IdNo)) & " ,'" & Trim(vBill_No) & "' , " & Str(Val(vSl_No)) & ", " & Str(Val(vItem_Idno)) & ", " & Str(Val(vBrand_IdNo)) & ", " & Str(Val(vQuantity)) & ", " & Val(vUnit_Idno) & ", " & Str(Val(vRate)) & ", " & Str(Val(vAmount)) & ", '" & Trim(vPo_No) & "',  '" & Trim(vPo_Code) & "',  " & Val(vPurchaseReturn_Quantity) & ")"
                    CmdTo.ExecuteNonQuery()

                End If

            Next

        End If

        Me.Text = ""

    End Sub

    Private Sub PurchaseReturnHead_Transfer(ByVal sqltr As SqlClient.SqlTransaction)
        Dim CmdFrm As New SqlClient.SqlCommand
        Dim CmdTo As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim vCompany_IdNo As Integer, vLedger_IdNo As Integer, vPurchaseReturnAc_IdNo As Integer, vTaxAc_IdNo As Integer
        Dim vItem_PurchaseReturn_Code As String, vItem_PurchaseReturn_No As String, vEntry_Type As String, vPo_Type As String, vTax_Type As String, vRemarks As String
        Dim vTotal_Quantity As Single, vTotal_Amount As Single, vNet_Amount As Single, vCashDisnt_Per As Single, vCashDiscount_Amount As Single, vAssessable_Value As Single, vTax_Percentage As Single, vTax_Amount As Single, vFreight_Amount As Single, vAddLess_Amount As Single, vRoundOff_Amount As Single

        Me.Text = "Item_PurchaseReturn_Head"

        CmdFrm.Connection = CnFrm
        CmdTo.Connection = CnTo
        CmdTo.Transaction = sqltr

        CmdTo.CommandText = "Delete from Stores_Item_PurchaseReturn_Head"
        CmdTo.ExecuteNonQuery()

        Da1 = New SqlClient.SqlDataAdapter("select * from Item_PurchaseReturn_Head  Order by Item_PurchaseReturn_No", CnFrm)
        Dt1 = New DataTable
        Da1.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then

            For I = 0 To Dt1.Rows.Count - 1

                Me.Text = "Item_PurchaseReturn_Head  -  " & I

                If Trim(Dt1.Rows(I).Item("Item_PurchaseReturn_Code").ToString) <> "" Then


                    CmdTo.Parameters.Clear()
                    CmdTo.Parameters.AddWithValue("@PurDate", CDate(Dt1.Rows(I).Item("Item_PurchaseReturn_Date").ToString))

                    vItem_PurchaseReturn_Code = Dt1.Rows(I).Item("Item_PurchaseReturn_Code").ToString
                    vItem_PurchaseReturn_No = Dt1.Rows(I).Item("Item_PurchaseReturn_No").ToString

                    vLedger_IdNo = Ledger_OldIdNoToNewIdNo(CnTo, Val(Dt1.Rows(I).Item("Ledger_IdNo").ToString), sqltr)
                    vCompany_IdNo = Dt1.Rows(I).Item("Company_IdNo").ToString


                    vEntry_Type = Dt1.Rows(I).Item("Entry_Type").ToString
                    vPo_Type = Dt1.Rows(I).Item("PO_Type").ToString
                    vPurchaseReturnAc_IdNo = Dt1.Rows(I).Item("PurchaseReturnAc_IdNo").ToString

                    vTotal_Quantity = Val(Dt1.Rows(I).Item("Total_Quantity").ToString)
                    vTax_Amount = Format(Val(Dt1.Rows(I).Item("Total_Amount").ToString), "#########0.00")
                    vCashDisnt_Per = Format(Val(Dt1.Rows(I).Item("CashDiscount_Percentage").ToString), "#########0.00")
                    vCashDiscount_Amount = Format(Val(Dt1.Rows(I).Item("CashDiscount_Amount").ToString), "############0.00")
                    vAssessable_Value = Format(Val(Dt1.Rows(I).Item("Assessable_Value").ToString), "###########0.00")

                    vTaxAc_IdNo = Val(Dt1.Rows(I).Item("TaxAc_IdNo").ToString)
                    vTax_Type = Dt1.Rows(I).Item("Tax_Type").ToString
                    If Trim(vTax_Type) = "" Then vTax_Type = "-NIL-"
                    vTax_Percentage = Val(Dt1.Rows(I).Item("Tax_Percentage").ToString)
                    vTax_Amount = Format(Val(Dt1.Rows(I).Item("Tax_Amount").ToString), "#########0.00")

                    vFreight_Amount = Format(Val(Dt1.Rows(I).Item("Freight_Amount").ToString), "#########0.00")
                    vAddLess_Amount = Format(Val(Dt1.Rows(I).Item("AddLess_Amount").ToString), "#########0.00")
                    vRoundOff_Amount = Format(Val(Dt1.Rows(I).Item("RoundOff_Amount").ToString), "#########0.00")
                    vNet_Amount = Format(Val(Dt1.Rows(I).Item("Net_Amount").ToString), "########0.00")

                    vRemarks = Dt1.Rows(I).Item("Remarks").ToString




                    CmdTo.CommandText = "Insert into Stores_Item_PurchaseReturn_Head(Item_PurchaseReturn_Code,             Company_IdNo         ,                   Item_PurchaseReturn_No    ,                               for_OrderBy                              , Item_PurchaseReturn_Date,                 Entry_Type      ,           Ledger_IdNo     ,               PO_Type          ,     PurchaseReturnAc_IdNo  ,                       Total_Quantity    ,       Total_Amount       ,      CashDiscount_Percentage       ,       CashDiscount_Amount            ,            Assessable_Value               ,            TaxAc_IdNo    ,             Tax_Type            ,             Tax_Percentage        ,             Tax_Amount              ,               Freight_Amount       ,             AddLess_Amount       ,             RoundOff_Amount         ,                   Net_Amount             ,               Remarks            ) " & _
                                    "              Values   (                  '" & Trim(vItem_PurchaseReturn_Code) & "', " & Str(Val(vCompany_IdNo)) & ", '" & Trim(vItem_PurchaseReturn_No) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(vItem_PurchaseReturn_No))) & ",     @PurDate     , '" & Trim(vEntry_Type) & "', " & Str(Val(vLedger_IdNo)) & ", '" & Trim(vPo_Type) & "', " & Str(Val(vPurchaseReturnAc_IdNo)) & ", " & Str(Val(vTotal_Quantity)) & ", " & Str(Val(vTotal_Amount)) & ", " & Str(Val(vCashDisnt_Per)) & ", " & Str(Val(vCashDiscount_Amount)) & ", " & Str(Val(vAssessable_Value)) & ", " & Str(Val(vTaxAc_IdNo)) & ", '" & Trim(vTax_Type) & "', " & Str(Val(vTax_Percentage)) & ", " & Str(Val(vTax_Amount)) & ", " & Str(Val(vFreight_Amount)) & ", " & Str(Val(vAddLess_Amount)) & ", " & Str(Val(vRoundOff_Amount)) & ", " & Str(Val(CSng(vNet_Amount))) & ", '" & Trim(vRemarks) & "' )"
                    CmdTo.ExecuteNonQuery()

                End If

            Next

        End If

        Me.Text = ""

    End Sub

    Private Sub PurchaseReturnDetails_Transfer(ByVal sqltr As SqlClient.SqlTransaction)
        Dim CmdFrm As New SqlClient.SqlCommand
        Dim CmdTo As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim vCompany_IdNo As Integer, vLedger_IdNo As Integer
        Dim vItem_PurchaseReturn_Code As String, vItem_PurchaseReturn_No As String, vEntry_Type As String, vPo_Type As String
        Dim vSl_No As Integer, vItem_Idno As Integer, vBrand_IdNo As Integer, vUnit_Idno As Integer, vPurchase_Details_SlNo As Integer
        Dim vPo_Code As String, vPo_No As String, vPurchase_No As String, vBill_No As String, vPurchase_Code As String
        Dim vQuantity As Single, vRate As Single, vAmount As Single, vPo_Details_SlNo As Single

        Me.Text = "Item_PurchaseReturn_Details"

        CmdFrm.Connection = CnFrm
        CmdTo.Connection = CnTo
        CmdTo.Transaction = sqltr

        CmdTo.CommandText = "Delete from Stores_Item_PurchaseReturn_Details"
        CmdTo.ExecuteNonQuery()

        Da1 = New SqlClient.SqlDataAdapter("select * from Item_PurchaseReturn_Details  Order by Item_PurchaseReturn_No", CnFrm)
        Dt1 = New DataTable
        Da1.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then

            For I = 0 To Dt1.Rows.Count - 1

                Me.Text = "Item_PurchaseReturn_Details  -  " & I

                If Trim(Dt1.Rows(I).Item("Item_PurchaseReturn_Code").ToString) <> "" Then


                    CmdTo.Parameters.Clear()
                    CmdTo.Parameters.AddWithValue("@PurDate", CDate(Dt1.Rows(I).Item("Item_PurchaseReturn_Date").ToString))

                    vItem_PurchaseReturn_Code = Dt1.Rows(I).Item("Item_PurchaseReturn_Code").ToString
                    vItem_PurchaseReturn_No = Dt1.Rows(I).Item("Item_PurchaseReturn_No").ToString

                    vLedger_IdNo = Ledger_OldIdNoToNewIdNo(CnTo, Val(Dt1.Rows(I).Item("Ledger_IdNo").ToString), sqltr)
                    vCompany_IdNo = Dt1.Rows(I).Item("Company_IdNo").ToString

                    vEntry_Type = Dt1.Rows(I).Item("Entry_Type").ToString
                    vPo_Type = Dt1.Rows(I).Item("PO_Type").ToString

                    vSl_No = Dt1.Rows(I).Item("Sl_No").ToString
                    vItem_Idno = Dt1.Rows(I).Item("Item_Idno").ToString
                    vBrand_IdNo = Dt1.Rows(I).Item("Brand_Idno").ToString
                    vQuantity = Val(Dt1.Rows(I).Item("Quantity").ToString)
                    vUnit_Idno = Unit_OldIdNoToNewIdNo(CnTo, Val(Dt1.Rows(I).Item("Unit_IdNo").ToString), sqltr)
                    vRate = Format(Val(Dt1.Rows(I).Item("Rate").ToString), "########0.00")
                    vAmount = Format(Val(Dt1.Rows(I).Item("Amount").ToString), "########0.00")
                    vPurchase_No = Dt1.Rows(I).Item("Purchase_No").ToString
                    vBill_No = Dt1.Rows(I).Item("Bill_No").ToString
                    vPo_No = Dt1.Rows(I).Item("Po_No").ToString
                    vPurchase_Code = Dt1.Rows(I).Item("Purchase_Code").ToString
                    vPurchase_Details_SlNo = Val(Dt1.Rows(I).Item("Purchase_Details_SlNo").ToString)
                    vPo_Code = Dt1.Rows(I).Item("Po_Code").ToString
                    vPo_Details_SlNo = Val(Dt1.Rows(I).Item("PO_Details_SlNo").ToString)


                    CmdTo.CommandText = "Insert into Stores_Item_PurchaseReturn_Details ( Item_PurchaseReturn_Code, Company_IdNo,                           Item_PurchaseReturn_No          ,                     for_OrderBy                                               , Item_PurchaseReturn_Date,      Entry_Type         ,     PO_Type          ,             Ledger_IdNo          , Sl_No               ,               Item_IdNo    ,         Brand_IdNo                 ,          Quantity    ,         Unit_idNo       ,          Rate           ,             Amount       ,          Purchase_No        ,         Bill_No        ,            Po_No        ,  Purchase_Code               ,             Po_Code      , PO_Details_SlNo) " & _
                                                           " Values ( '" & Trim(vItem_PurchaseReturn_Code) & "', " & Str(Val(vCompany_IdNo)) & ", '" & Trim(vItem_PurchaseReturn_No) & "'  , " & Str(Val(Common_Procedures.OrderBy_CodeToValue(vItem_PurchaseReturn_No))) & ", @PurDate            ,'" & Trim(vEntry_Type) & "','" & Trim(vPo_Type) & "', " & Str(Val(vLedger_IdNo)) & " , " & Str(Val(vSl_No)) & ", " & Str(Val(vItem_Idno)) & ", " & Str(Val(vBrand_IdNo)) & ", " & Str(Val(vQuantity)) & ", " & Val(vUnit_Idno) & ", " & Str(Val(vRate)) & ", " & Str(Val(vAmount)) & ", '" & Trim(vPurchase_No) & "','" & Trim(vBill_No) & "', '" & Trim(vPo_No) & "',  '" & Trim(vPurchase_Code) & "', '" & Trim(vPo_Code) & "', " & Val(vPo_Details_SlNo) & ")"
                    CmdTo.ExecuteNonQuery()

                End If


            Next

        End If

        Me.Text = ""

    End Sub

    Private Sub ItemIssueToMachineHead_Transfer(ByVal sqltr As SqlClient.SqlTransaction)
        Dim CmdFrm As New SqlClient.SqlCommand
        Dim CmdTo As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim vCompany_IdNo As Integer
        Dim vItem_Issue_Code As String, vItem_Issue_No As String, vNew_old As String, vReceiver_Name As String, vIssue_Name As String
        Dim vTotal_Quantity As Single

        Me.Text = "Item_Issue_Head"

        CmdFrm.Connection = CnFrm
        CmdTo.Connection = CnTo
        CmdTo.Transaction = sqltr

        CmdTo.CommandText = "Delete from Stores_Item_Issue_Head"
        CmdTo.ExecuteNonQuery()

        Da1 = New SqlClient.SqlDataAdapter("select * from Item_Issue_Head  Order by Issue_No", CnFrm)
        Dt1 = New DataTable
        Da1.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then

            For I = 0 To Dt1.Rows.Count - 1

                Me.Text = "Item_Issue_Head  -  " & I

                If Trim(Dt1.Rows(I).Item("Issue_Code").ToString) <> "" Then


                    CmdTo.Parameters.Clear()
                    CmdTo.Parameters.AddWithValue("@PurDate", CDate(Dt1.Rows(I).Item("Issue_Date").ToString))

                    vItem_Issue_Code = Dt1.Rows(I).Item("Issue_Code").ToString
                    vItem_Issue_No = Dt1.Rows(I).Item("Issue_No").ToString

                    ' vLedger_IdNo = Ledger_OldIdNoToNewIdNo(CnTo, Val(Dt1.Rows(I).Item("Ledger_IdNo").ToString), sqltr)
                    vCompany_IdNo = Dt1.Rows(I).Item("Company_IdNo").ToString


                    vNew_old = Dt1.Rows(I).Item("New_Old").ToString
                    vReceiver_Name = Dt1.Rows(I).Item("Received_Name").ToString
                    vIssue_Name = Dt1.Rows(I).Item("Issued_Name").ToString

                    vTotal_Quantity = Val(Dt1.Rows(I).Item("Total_Quantity").ToString)



                    CmdTo.CommandText = "Insert into Stores_Item_Issue_Head(Issue_Code,                      Company_IdNo           ,         Issue_No                  ,                              for_OrderBy                             , Issue_Date        ,     New_old        ,     Issued_Name     ,          Received_Name          , Total_Quantity                    ) " & _
                                    "              Values   ( '" & Trim(vItem_Issue_Code) & "', " & Str(Val(vCompany_IdNo)) & ", '" & Trim(vItem_Issue_No) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(vItem_Issue_No))) & ",     @PurDate     ,'" & Trim(vNew_old) & "' , '" & Trim(vIssue_Name) & "',  '" & Trim(vReceiver_Name) & "', " & Str(Val(vTotal_Quantity)) & ")"
                    CmdTo.ExecuteNonQuery()

                End If

            Next

        End If

        Me.Text = ""

    End Sub

    Private Sub ItemIssueToMachineDetails_Transfer(ByVal sqltr As SqlClient.SqlTransaction)
        Dim CmdFrm As New SqlClient.SqlCommand
        Dim CmdTo As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim vCompany_IdNo As Integer
        Dim vItem_Issue_Code As String, vItem_Issue_No As String
        Dim vSl_No As Integer, vItem_Idno As Integer, vBrand_IdNo As Integer, vUnit_Idno As Integer, vMachine_IdNo As Integer
        Dim vQuantity As Single

        Me.Text = "Item_Issue_Details"

        CmdFrm.Connection = CnFrm
        CmdTo.Connection = CnTo
        CmdTo.Transaction = sqltr

        CmdTo.CommandText = "Delete from Stores_Item_Issue_Details"
        CmdTo.ExecuteNonQuery()

        Da1 = New SqlClient.SqlDataAdapter("select * from Item_Issue_Details  Order by Issue_No", CnFrm)
        Dt1 = New DataTable
        Da1.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then

            For I = 0 To Dt1.Rows.Count - 1

                Me.Text = "Item_Issue_Details  -  " & I

                If Trim(Dt1.Rows(I).Item("Issue_Code").ToString) <> "" Then


                    CmdTo.Parameters.Clear()
                    CmdTo.Parameters.AddWithValue("@PoDate", CDate(Dt1.Rows(I).Item("Issue_Date").ToString))

                    vItem_Issue_Code = Dt1.Rows(I).Item("Issue_Code").ToString
                    vItem_Issue_No = Dt1.Rows(I).Item("Issue_No").ToString

                    '  vLedger_IdNo = Dt1.Rows(0).Item("Ledger_IdNo").ToString
                    vCompany_IdNo = Dt1.Rows(I).Item("Company_IdNo").ToString


                    vSl_No = Dt1.Rows(I).Item("Sl_No").ToString
                    vItem_Idno = Dt1.Rows(I).Item("Item_Idno").ToString
                    vBrand_IdNo = Dt1.Rows(I).Item("Brand_Idno").ToString
                    vQuantity = Val(Dt1.Rows(I).Item("Quantity").ToString)
                    vUnit_Idno = Unit_OldIdNoToNewIdNo(CnTo, Val(Dt1.Rows(I).Item("Unit_IdNo").ToString), sqltr)
                    vMachine_IdNo = Val(Dt1.Rows(I).Item("Machine_Idno").ToString)



                    CmdTo.CommandText = "Insert into Stores_Item_Issue_Details ( Issue_Code                       , Company_IdNo                   ,                 Issue_No       , for_OrderBy                                                        ,     Issue_Date ,         Sl_No           ,            Item_IdNo        ,            Brand_IdNo        ,             Quantity         ,       Unit_idNo      ,       Machine_idNo     ) " & _
                                                                       " Values ( '" & Trim(vItem_Issue_Code) & "', " & Str(Val(vCompany_IdNo)) & ", '" & Trim(vItem_Issue_No) & " ' , " & Str(Val(Common_Procedures.OrderBy_CodeToValue(vItem_Issue_No))) & ",  @PoDate   , " & Str(Val(vSl_No)) & ", " & Str(Val(vItem_Idno)) & ", " & Str(Val(vBrand_IdNo)) & ", " & Str(Val(vQuantity)) & "  , " & Val(vUnit_Idno) & ", " & Str(Val(vMachine_IdNo)) & ")"
                    CmdTo.ExecuteNonQuery()

                End If

            Next

        End If

        Me.Text = ""

    End Sub


    Private Sub ItemReturnFromMachineHead_Transfer(ByVal sqltr As SqlClient.SqlTransaction)
        Dim CmdFrm As New SqlClient.SqlCommand
        Dim CmdTo As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim vCompany_IdNo As Integer
        Dim vReturn_Code As String, vReturn_No As String, vNew_old As String, vReceiver_Name As String, vIssue_Name As String, vUsable_Scrap As String
        Dim vTotal_Quantity As Single

        Me.Text = "Item_Return_Head"

        CmdFrm.Connection = CnFrm
        CmdTo.Connection = CnTo
        CmdTo.Transaction = sqltr

        CmdTo.CommandText = "Delete from Stores_Item_Return_Head"
        CmdTo.ExecuteNonQuery()

        Da1 = New SqlClient.SqlDataAdapter("select * from Item_Return_Head  Order by Return_No", CnFrm)
        Dt1 = New DataTable
        Da1.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then

            For I = 0 To Dt1.Rows.Count - 1

                Me.Text = "Item_Return_Head -  " & I

                If Trim(Dt1.Rows(I).Item("Return_Code").ToString) <> "" Then


                    CmdTo.Parameters.Clear()
                    CmdTo.Parameters.AddWithValue("@PoDate", CDate(Dt1.Rows(I).Item("Return_Date").ToString))

                    vReturn_Code = Dt1.Rows(I).Item("Return_Code").ToString
                    vReturn_No = Dt1.Rows(I).Item("Return_No").ToString

                    'vLedger_IdNo = Dt1.Rows(0).Item("Ledger_IdNo").ToString
                    vCompany_IdNo = Dt1.Rows(I).Item("Company_IdNo").ToString

                    vReceiver_Name = Dt1.Rows(I).Item("Received_Name").ToString
                    vIssue_Name = Dt1.Rows(I).Item("Issued_Name").ToString
                    vNew_old = Dt1.Rows(I).Item("New_Old").ToString
                    vUsable_Scrap = Dt1.Rows(I).Item("Usable_Scrap").ToString
                    vTotal_Quantity = Val(Dt1.Rows(I).Item("Total_Quantity").ToString)



                    CmdTo.CommandText = "Insert into Stores_Item_Return_Head(Return_Code,          Company_IdNo          ,           Return_No        ,                                      for_OrderBy                        , Return_Date,            Issued_Name          ,  Received_Name            ,       New_Old          ,              Usable_Scrap        , Total_Quantity ) " & _
                                    "              Values   ( '" & Trim(vReturn_Code) & "', " & Str(Val(vCompany_IdNo)) & ", '" & Trim(vReturn_No) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(vReturn_No))) & ",     @PoDate     , '" & Trim(vIssue_Name) & "',  '" & Trim(vReceiver_Name) & "', '" & Trim(vNew_old) & "','" & Trim(vUsable_Scrap) & "',  " & Str(Val(vTotal_Quantity)) & ")"
                    CmdTo.ExecuteNonQuery()

                End If

            Next

        End If

        Me.Text = ""

    End Sub

    Private Sub ItemReturnFromMachineDetails_Transfer(ByVal sqltr As SqlClient.SqlTransaction)
        Dim CmdFrm As New SqlClient.SqlCommand
        Dim CmdTo As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim vCompany_IdNo As Integer
        Dim vReturn_Code As String, vReturn_No As String
        Dim vSl_No As Integer, vItem_Idno As Integer, vBrand_IdNo As Integer, vUnit_Idno As Integer, vMachine_IdNo As Integer
        Dim vQuantity As Single

        Me.Text = "Item_Return_Details"

        CmdFrm.Connection = CnFrm
        CmdTo.Connection = CnTo
        CmdTo.Transaction = sqltr

        CmdTo.CommandText = "Delete from Stores_Item_Return_Details"
        CmdTo.ExecuteNonQuery()

        Da1 = New SqlClient.SqlDataAdapter("select * from Item_Return_Details  Order by Return_No", CnFrm)
        Dt1 = New DataTable
        Da1.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then

            For I = 0 To Dt1.Rows.Count - 1

                Me.Text = "Item_Return_Details  -  " & I

                If Trim(Dt1.Rows(I).Item("Return_Code").ToString) <> "" Then


                    CmdTo.Parameters.Clear()
                    CmdTo.Parameters.AddWithValue("@PoDate", CDate(Dt1.Rows(I).Item("Return_Date").ToString))

                    vReturn_Code = Dt1.Rows(I).Item("Return_Code").ToString
                    vReturn_No = Dt1.Rows(I).Item("Return_No").ToString

                    '  vLedger_IdNo = Dt1.Rows(0).Item("Ledger_IdNo").ToString
                    vCompany_IdNo = Dt1.Rows(I).Item("Company_IdNo").ToString


                    vSl_No = Dt1.Rows(I).Item("Sl_No").ToString
                    vItem_Idno = Dt1.Rows(I).Item("Item_Idno").ToString
                    vBrand_IdNo = Dt1.Rows(I).Item("Brand_Idno").ToString
                    vQuantity = Val(Dt1.Rows(I).Item("Quantity").ToString)
                    vUnit_Idno = Unit_OldIdNoToNewIdNo(CnTo, Val(Dt1.Rows(I).Item("Unit_IdNo").ToString), sqltr)
                    vMachine_IdNo = Val(Dt1.Rows(I).Item("Machine_Idno").ToString)



                    CmdTo.CommandText = "Insert into Stores_Item_Return_Details ( Return_Code    ,       Company_IdNo        ,            Return_No              ,                    for_OrderBy                                   , Return_Date,              Sl_No          , Item_IdNo           ,         Brand_IdNo        ,                  Quantity       ,         Unit_idNo, Machine_idNo  ) " & _
                                                           " Values ( '" & Trim(vReturn_Code) & "', " & Str(Val(vCompany_IdNo)) & ", '" & Trim(vReturn_No) & "'  , " & Str(Val(Common_Procedures.OrderBy_CodeToValue(vReturn_No))) & ", @PoDate , " & Str(Val(vSl_No)) & ", " & Str(Val(vItem_Idno)) & ", " & Str(Val(vBrand_IdNo)) & ", " & Str(Val(vQuantity)) & ", " & Val(vUnit_Idno) & ", " & Str(Val(vMachine_IdNo)) & ")"
                    CmdTo.ExecuteNonQuery()
                End If

            Next

        End If

        Me.Text = ""

    End Sub


    Private Sub ItemDeliveryHead_Transfer(ByVal sqltr As SqlClient.SqlTransaction)
        Dim CmdFrm As New SqlClient.SqlCommand
        Dim CmdTo As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim vCompany_IdNo As Integer, vLedger_IdNo As Integer
        Dim vItemDelivery_Code As String, vItemDelivery_No As String, vNew_old As String, vReceiver_Name As String, vIssue_Name As String, vVehicle_No As String, vType As String
        Dim vTotal_Quantity As Single

        Me.Text = "Item_Delivery_Head"

        CmdFrm.Connection = CnFrm
        CmdTo.Connection = CnTo
        CmdTo.Transaction = sqltr

        CmdTo.CommandText = "Delete from Stores_Item_Delivery_Head"
        CmdTo.ExecuteNonQuery()

        Da1 = New SqlClient.SqlDataAdapter("select * from Item_Delivery_Head  Order by Item_Delivery_No", CnFrm)
        Dt1 = New DataTable
        Da1.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then

            For I = 0 To Dt1.Rows.Count - 1

                Me.Text = "Item_Delivery_Head -  " & I

                If Trim(Dt1.Rows(I).Item("Item_Delivery_Code").ToString) <> "" Then


                    CmdTo.Parameters.Clear()
                    CmdTo.Parameters.AddWithValue("@PoDate", CDate(Dt1.Rows(I).Item("Item_Delivery_Date").ToString))

                    vItemDelivery_Code = Dt1.Rows(I).Item("Item_Delivery_Code").ToString
                    vItemDelivery_No = Dt1.Rows(I).Item("Item_Delivery_No").ToString

                    vLedger_IdNo = Ledger_OldIdNoToNewIdNo(CnTo, Val(Dt1.Rows(I).Item("Ledger_IdNo").ToString), sqltr)
                    vCompany_IdNo = Dt1.Rows(I).Item("Company_IdNo").ToString

                    vReceiver_Name = Dt1.Rows(I).Item("Received_Name").ToString
                    vIssue_Name = Dt1.Rows(I).Item("Issued_Name").ToString
                    vNew_old = Dt1.Rows(I).Item("New_Old").ToString
                    vVehicle_No = Dt1.Rows(I).Item("Vehicle_No").ToString
                    vTotal_Quantity = Val(Dt1.Rows(I).Item("Total_Quantity").ToString)
                    vType = Dt1.Rows(I).Item("Type").ToString


                    CmdTo.CommandText = "Insert into Stores_Item_Delivery_Head(Item_Delivery_Code,         Company_IdNo          ,       Item_Delivery_No              ,              for_OrderBy                                    ,          Item_Delivery_Date,  Ledger_IdNo             ,        Type        ,    Received_Name              ,         Issued_Name        ,         New_Old           , Vehicle_No              , Total_Quantity) " & _
                                    "              Values   ( '" & Trim(vItemDelivery_Code) & "', " & Str(Val(vCompany_IdNo)) & ", '" & Trim(vItemDelivery_No) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(vItemDelivery_No))) & ",     @PoDate     , " & Val(vLedger_IdNo) & "  ,'" & Trim(vType) & "','" & Trim(vReceiver_Name) & "',  '" & Trim(vIssue_Name) & "', '" & Trim(vNew_old) & "','" & Trim(vVehicle_No) & "',  " & Str(Val(vTotal_Quantity)) & ")"
                    CmdTo.ExecuteNonQuery()

                End If

            Next

        End If

        Me.Text = ""

    End Sub

    Private Sub ItemDeliveryDetails_Transfer(ByVal sqltr As SqlClient.SqlTransaction)
        Dim CmdFrm As New SqlClient.SqlCommand
        Dim CmdTo As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim vCompany_IdNo As Integer, vLedger_IdNo As Integer
        Dim vItemdelivery_Code As String, vItemDelivery_No As String
        Dim vSl_No As Integer, vItem_Idno As Integer, vBrand_IdNo As Integer, vUnit_Idno As Integer, vItem_Delivery_Details_SlNo As Integer
        Dim vQuantity As Single, VReceipt_Quantity As Single

        Me.Text = "Item_Delivery_Details"

        CmdFrm.Connection = CnFrm
        CmdTo.Connection = CnTo
        CmdTo.Transaction = sqltr

        CmdTo.CommandText = "Delete from Stores_Item_Delivery_Details"
        CmdTo.ExecuteNonQuery()

        Da1 = New SqlClient.SqlDataAdapter("select * from Item_Delivery_Details  Order by Item_Delivery_No", CnFrm)
        Dt1 = New DataTable
        Da1.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then

            For I = 0 To Dt1.Rows.Count - 1

                Me.Text = "Item_Delivery_Details  -  " & I

                If Trim(Dt1.Rows(I).Item("Item_Delivery_Code").ToString) <> "" Then

                    CmdTo.Parameters.Clear()
                    CmdTo.Parameters.AddWithValue("@PoDate", CDate(Dt1.Rows(I).Item("Item_Delivery_Date").ToString))

                    vItemdelivery_Code = Dt1.Rows(I).Item("Item_Delivery_Code").ToString
                    vItemDelivery_No = Dt1.Rows(I).Item("Item_Delivery_No").ToString

                    vLedger_IdNo = Ledger_OldIdNoToNewIdNo(CnTo, Val(Dt1.Rows(I).Item("Ledger_IdNo").ToString), sqltr)
                    vCompany_IdNo = Dt1.Rows(I).Item("Company_IdNo").ToString

                    vSl_No = Dt1.Rows(I).Item("Sl_No").ToString
                    vItem_Idno = Dt1.Rows(I).Item("Item_Idno").ToString
                    vBrand_IdNo = Dt1.Rows(I).Item("Brand_Idno").ToString
                    vQuantity = Val(Dt1.Rows(I).Item("Quantity").ToString)
                    vUnit_Idno = Unit_OldIdNoToNewIdNo(CnTo, Val(Dt1.Rows(I).Item("Unit_IdNo").ToString), sqltr)
                    VReceipt_Quantity = Val(Dt1.Rows(I).Item("Receipt_Quantity").ToString)
                    vItem_Delivery_Details_SlNo = Val(Dt1.Rows(I).Item("Item_Delivery_Details_SlNo").ToString)
                    ' vUnit_Name = Dt1.Rows(I).Item("Unit_Name").ToString

                    CmdTo.CommandText = "Insert into Stores_Item_Delivery_Details ( Item_Delivery_Code,         Company_IdNo          ,              Item_Delivery_No        ,                            for_OrderBy                            , Item_Delivery_Date,        Ledger_idNo ,          Sl_No          ,              Item_IdNo                 , Brand_IdNo     ,                Quantity               , Unit_IdNo ) " & _
                                                           " Values ( '" & Trim(vItemdelivery_Code) & "', " & Str(Val(vCompany_IdNo)) & ", '" & Trim(vItemDelivery_No) & " ' , " & Str(Val(Common_Procedures.OrderBy_CodeToValue(vItemDelivery_No))) & ", @PoDate , " & Val(vLedger_IdNo) & ", " & Str(Val(vSl_No)) & ", " & Str(Val(vItem_Idno)) & ", " & Str(Val(vBrand_IdNo)) & ", " & Str(Val(vQuantity)) & ", " & Val(vUnit_Idno) & ")"
                    CmdTo.ExecuteNonQuery()

                End If

            Next

        End If

        Me.Text = ""

    End Sub


    Private Sub ItemReceiptHead_Transfer(ByVal sqltr As SqlClient.SqlTransaction)
        Dim CmdFrm As New SqlClient.SqlCommand
        Dim CmdTo As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim vCompany_IdNo As Integer, vLedger_IdNo As Integer
        Dim vItemReceipt_Code As String, vItemReceipt_No As String, vNew_old As String, vReceiver_Name As String, vIssue_Name As String, vParticulars As String, vParty_DcNo As String, vType As String
        Dim vTotal_Quantity As Single

        Me.Text = "Item_Receipt_Head"

        CmdFrm.Connection = CnFrm
        CmdTo.Connection = CnTo
        CmdTo.Transaction = sqltr

        CmdTo.CommandText = "Delete from Stores_Item_Receipt_Head"
        CmdTo.ExecuteNonQuery()

        Da1 = New SqlClient.SqlDataAdapter("select * from Item_Receipt_Head  Order by Item_Receipt_No", CnFrm)
        Dt1 = New DataTable
        Da1.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then

            For I = 0 To Dt1.Rows.Count - 1

                Me.Text = "Item_Receipt_Head -  " & I

                If Trim(Dt1.Rows(I).Item("Item_Receipt_Code").ToString) <> "" Then


                    CmdTo.Parameters.Clear()
                    CmdTo.Parameters.AddWithValue("@PoDate", CDate(Dt1.Rows(I).Item("Item_Receipt_Date").ToString))

                    vItemReceipt_Code = Dt1.Rows(I).Item("Item_Receipt_Code").ToString
                    vItemReceipt_No = Dt1.Rows(I).Item("Item_Receipt_No").ToString

                    vLedger_IdNo = Ledger_OldIdNoToNewIdNo(CnTo, Val(Dt1.Rows(I).Item("Ledger_IdNo").ToString), sqltr)
                    vCompany_IdNo = Dt1.Rows(I).Item("Company_IdNo").ToString
                    vParty_DcNo = Dt1.Rows(I).Item("Party_DcNo").ToString
                    vReceiver_Name = Dt1.Rows(I).Item("Received_Name").ToString
                    vIssue_Name = Dt1.Rows(I).Item("Issued_Name").ToString
                    vNew_old = Dt1.Rows(I).Item("New_Old").ToString
                    vParticulars = Dt1.Rows(I).Item("Particulars").ToString
                    vTotal_Quantity = Val(Dt1.Rows(I).Item("Total_Quantity").ToString)
                    vType = Dt1.Rows(I).Item("Type").ToString


                    CmdTo.CommandText = "Insert into Stores_Item_Receipt_Head(Item_Receipt_Code,               Company_IdNo        ,      Item_Receipt_No                 ,                       for_OrderBy                                    , Item_Receipt_Date,       Ledger_IdNo,           Type               ,          Party_DcNo      ,     Received_Name              ,         Issued_Name          ,         New_Old        ,       Particulars       ,                Total_Quantity) " & _
                                    "              Values   (       '" & Trim(vItemReceipt_Code) & "', " & Str(Val(vCompany_IdNo)) & ", '" & Trim(vItemReceipt_No) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(vItemReceipt_No))) & ",     @PoDate     , '" & Val(vLedger_IdNo) & "','" & Trim(vType) & "','" & Trim(vParty_DcNo) & "','" & Trim(vReceiver_Name) & "',  '" & Trim(vIssue_Name) & "', '" & Trim(vNew_old) & "','" & Trim(vParticulars) & "',  " & Str(Val(vTotal_Quantity)) & ")"
                    CmdTo.ExecuteNonQuery()

                End If

            Next

        End If

        Me.Text = ""

    End Sub

    Private Sub ItemReceiptDetails_Transfer(ByVal sqltr As SqlClient.SqlTransaction)
        Dim CmdFrm As New SqlClient.SqlCommand
        Dim CmdTo As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim vCompany_IdNo As Integer
        Dim vItemReceipt_Code As String, vItemReceipt_No As String, vItem_Delivery_Code As String
        Dim vSl_No As Integer, vItem_Idno As Integer, vBrand_IdNo As Integer, vUnit_Idno As Integer, vItem_Delivery_Details_SlNo As Integer
        Dim vQuantity As Single

        Me.Text = "Item_Receipt_Details"

        CmdFrm.Connection = CnFrm
        CmdTo.Connection = CnTo
        CmdTo.Transaction = sqltr

        CmdTo.CommandText = "Delete from Stores_Item_Receipt_Details"
        CmdTo.ExecuteNonQuery()

        Da1 = New SqlClient.SqlDataAdapter("select * from Item_Receipt_Details  Order by Item_Receipt_No", CnFrm)
        Dt1 = New DataTable
        Da1.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then

            For I = 0 To Dt1.Rows.Count - 1

                Me.Text = "Item_Receipt_Details -  " & I

                If Trim(Dt1.Rows(I).Item("Item_Receipt_Code").ToString) <> "" Then


                    CmdTo.Parameters.Clear()
                    CmdTo.Parameters.AddWithValue("@PoDate", CDate(Dt1.Rows(I).Item("Item_Receipt_Date").ToString))

                    vItemReceipt_Code = Dt1.Rows(I).Item("Item_Receipt_Code").ToString
                    vItemReceipt_No = Dt1.Rows(I).Item("Item_Receipt_No").ToString

                    '  vLedger_IdNo = Dt1.Rows(0).Item("Ledger_IdNo").ToString
                    vCompany_IdNo = Dt1.Rows(I).Item("Company_IdNo").ToString


                    vSl_No = Dt1.Rows(I).Item("Sl_No").ToString
                    vItem_Idno = Dt1.Rows(I).Item("Item_Idno").ToString
                    vBrand_IdNo = Dt1.Rows(I).Item("Brand_Idno").ToString
                    vQuantity = Val(Dt1.Rows(I).Item("Quantity").ToString)
                    vUnit_Idno = Unit_OldIdNoToNewIdNo(CnTo, Val(Dt1.Rows(I).Item("Unit_IdNo").ToString), sqltr)
                    ' vItem_Delivery_No = (Dt1.Rows(I).Item("Item_Delivery_No").ToString)
                    vItem_Delivery_Code = Dt1.Rows(I).Item("Item_Delivery_Code").ToString
                    vItem_Delivery_Details_SlNo = Val(Dt1.Rows(I).Item("Item_Delivery_Details_SlNo").ToString)
                    'vUnit_Name = Dt1.Rows(I).Item("Unit_Name").ToString

                    CmdTo.CommandText = "Insert into Stores_Item_Receipt_Details ( Item_Receipt_Code    ,           Company_IdNo        ,         Item_Receipt_No       ,                                         for_OrderBy                   , Item_Receipt_Date, Sl_No            ,                  Item_IdNo      ,                 Brand_IdNo      ,    Quantity           ,        Unit_IdNo        ,     Item_Delivery_Code              , Item_Delivery_Details_SlNo        ) " & _
                                                           " Values ( '" & Trim(vItemReceipt_Code) & "', " & Str(Val(vCompany_IdNo)) & ", '" & Trim(vItemReceipt_No) & " ' , " & Str(Val(Common_Procedures.OrderBy_CodeToValue(vItemReceipt_No))) & ", @PoDate ,  " & Str(Val(vSl_No)) & ", " & Str(Val(vItem_Idno)) & ", " & Str(Val(vBrand_IdNo)) & ", " & Str(Val(vQuantity)) & ", " & Val(vUnit_Idno) & " , '" & Trim(vItem_Delivery_Code) & "',   " & Val(vItem_Delivery_Details_SlNo) & " )"
                    CmdTo.ExecuteNonQuery()
                End If

            Next

        End If

        Me.Text = ""

    End Sub

    Private Sub ServiceItemDeliveryHead_Transfer(ByVal sqltr As SqlClient.SqlTransaction)
        Dim CmdFrm As New SqlClient.SqlCommand
        Dim CmdTo As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim vCompany_IdNo As Integer, vLedger_IdNo As Integer
        Dim vServiceDelivery_Code As String, vServiceDelivery_No As String, vNew_old As String, vReceiver_Name As String, vIssue_Name As String, vParticulars As String, vVehicle_No As String, vUnReturnable As String
        Dim vTotal_Quantity As Single

        Me.Text = "Service_Delivery_Head"

        CmdFrm.Connection = CnFrm
        CmdTo.Connection = CnTo
        CmdTo.Transaction = sqltr

        CmdTo.CommandText = "Delete from Stores_Service_Delivery_Head"
        CmdTo.ExecuteNonQuery()

        Da1 = New SqlClient.SqlDataAdapter("select * from Service_Delivery_Head  Order by Service_Delivery_No", CnFrm)
        Dt1 = New DataTable
        Da1.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then

            For I = 0 To Dt1.Rows.Count - 1

                Me.Text = "Service_Delivery_Head -  " & I

                If Trim(Dt1.Rows(I).Item("Service_Delivery_Code").ToString) <> "" Then


                    CmdTo.Parameters.Clear()
                    CmdTo.Parameters.AddWithValue("@PoDate", CDate(Dt1.Rows(I).Item("Service_Delivery_Date").ToString))

                    vServiceDelivery_Code = Dt1.Rows(I).Item("Service_Delivery_Code").ToString
                    vServiceDelivery_No = Dt1.Rows(I).Item("Service_Delivery_No").ToString

                    vLedger_IdNo = Ledger_OldIdNoToNewIdNo(CnTo, Val(Dt1.Rows(I).Item("Ledger_IdNo").ToString), sqltr)
                    vCompany_IdNo = Dt1.Rows(I).Item("Company_IdNo").ToString

                    vReceiver_Name = Dt1.Rows(I).Item("Received_Name").ToString
                    vIssue_Name = Dt1.Rows(I).Item("Issued_Name").ToString
                    vNew_old = Dt1.Rows(I).Item("New_Old").ToString
                    vParticulars = Dt1.Rows(I).Item("Particulars").ToString
                    vUnReturnable = Dt1.Rows(I).Item("UnReturnable").ToString
                    vTotal_Quantity = Val(Dt1.Rows(I).Item("Total_Quantity").ToString)
                    vVehicle_No = Dt1.Rows(I).Item("Vehicle_No").ToString


                    CmdTo.CommandText = "Insert into Stores_Service_Delivery_Head(Service_Delivery_Code,        Company_IdNo             ,          Service_Delivery_No        ,                       for_OrderBy                                  , Service_Delivery_Date,           Ledger_IdNo,                Received_Name,                Issued_Name         ,             New_Old          ,    Particulars        ,         UnReturnable           ,           Vehicle_No      ,                 Total_Quantity) " & _
                                    "              Values   (       '" & Trim(vServiceDelivery_Code) & "', " & Str(Val(vCompany_IdNo)) & ", '" & Trim(vServiceDelivery_No) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(vServiceDelivery_No))) & ",     @PoDate     , " & Val(vLedger_IdNo) & "','" & Trim(vReceiver_Name) & "','" & Trim(vIssue_Name) & "','" & Trim(vNew_old) & "',  '" & Trim(vParticulars) & "', '" & Trim(vUnReturnable) & "','" & Trim(vVehicle_No) & "',  " & Str(Val(vTotal_Quantity)) & ")"
                    CmdTo.ExecuteNonQuery()

                End If

            Next

        End If

        Me.Text = ""

    End Sub

    Private Sub ServiceItemDeliveryDetails_Transfer(ByVal sqltr As SqlClient.SqlTransaction)
        Dim CmdFrm As New SqlClient.SqlCommand
        Dim CmdTo As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim vCompany_IdNo As Integer, vLedger_IdNo As Integer
        Dim vServiceDelivery_Code As String, vServiceDelivery_No As String
        Dim vSl_No As Integer, vItem_Idno As Integer, vBrand_IdNo As Integer, vUnit_Idno As Integer, vService_Delivery_Details_SlNo As Integer, vMachine_IdNo As Integer
        Dim vQuantity As Single, vReceipt_Quantity As Single

        Me.Text = "Service_Delivery_Details"

        CmdFrm.Connection = CnFrm
        CmdTo.Connection = CnTo
        CmdTo.Transaction = sqltr

        CmdTo.CommandText = "Delete from Stores_Service_Delivery_Details"
        CmdTo.ExecuteNonQuery()

        Da1 = New SqlClient.SqlDataAdapter("select * from Service_Delivery_Details  Order by Service_Delivery_No", CnFrm)
        Dt1 = New DataTable
        Da1.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then

            For I = 0 To Dt1.Rows.Count - 1

                Me.Text = "Service_Delivery_Details -  " & I

                If Trim(Dt1.Rows(I).Item("Service_Delivery_Code").ToString) <> "" Then


                    CmdTo.Parameters.Clear()
                    CmdTo.Parameters.AddWithValue("@PoDate", CDate(Dt1.Rows(I).Item("Service_Delivery_Date").ToString))

                    vServiceDelivery_Code = Dt1.Rows(I).Item("Service_Delivery_Code").ToString
                    vServiceDelivery_No = Dt1.Rows(I).Item("Service_Delivery_No").ToString

                    vLedger_IdNo = Ledger_OldIdNoToNewIdNo(CnTo, Val(Dt1.Rows(I).Item("Ledger_IdNo").ToString), sqltr)
                    vCompany_IdNo = Dt1.Rows(I).Item("Company_IdNo").ToString


                    vSl_No = Dt1.Rows(I).Item("Sl_No").ToString
                    vItem_Idno = Dt1.Rows(I).Item("Item_Idno").ToString
                    vBrand_IdNo = Dt1.Rows(I).Item("Brand_Idno").ToString
                    vQuantity = Val(Dt1.Rows(I).Item("Quantity").ToString)
                    vUnit_Idno = Unit_OldIdNoToNewIdNo(CnTo, Val(Dt1.Rows(I).Item("Unit_IdNo").ToString), sqltr)
                    vMachine_IdNo = Val(Dt1.Rows(I).Item("Machine_Idno").ToString)

                    vService_Delivery_Details_SlNo = Val(Dt1.Rows(I).Item("Service_Delivery_Details_SlNo").ToString)
                    vReceipt_Quantity = Val(Dt1.Rows(0).Item("Receipt_Quantity").ToString)

                    CmdTo.CommandText = "Insert into Stores_Service_Delivery_Details ( Service_Delivery_Code,           Company_IdNo       ,       Service_Delivery_No       ,                                   for_OrderBy                                   , Service_Delivery_Date,          Ledger_IdNo     ,               Sl_No       ,       Item_IdNo            ,        Brand_IdNo          ,            Quantity         ,       Unit_idNo         ,      Machine_idNo        ,  Receipt_Quantity) " & _
                                                           " Values ( '" & Trim(vServiceDelivery_Code) & "', " & Str(Val(vCompany_IdNo)) & ", '" & Trim(vServiceDelivery_No) & " ' , " & Str(Val(Common_Procedures.OrderBy_CodeToValue(vServiceDelivery_No))) & ", @PoDate            , " & Val(vLedger_IdNo) & " , " & Str(Val(vSl_No)) & ", " & Str(Val(vItem_Idno)) & ", " & Str(Val(vBrand_IdNo)) & ", " & Str(Val(vQuantity)) & ", " & Val(vUnit_Idno) & " ," & Val(vMachine_IdNo) & ", " & Val(vReceipt_Quantity) & " )"
                    CmdTo.ExecuteNonQuery()
                End If

            Next

        End If

        Me.Text = ""

    End Sub

    Private Sub ServiceItemReceiptHead_Transfer(ByVal sqltr As SqlClient.SqlTransaction)
        Dim CmdFrm As New SqlClient.SqlCommand
        Dim CmdTo As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim vCompany_IdNo As Integer, vLedger_IdNo As Integer
        Dim vServiceReceipt_Code As String, vServiceReceipt_No As String, vNew_old As String, vReceiver_Name As String, vIssue_Name As String, vParticulars As String, vPartyDc_No As String
        Dim vTotal_Quantity As Single

        Me.Text = "Service_Receipt_Head"

        CmdFrm.Connection = CnFrm
        CmdTo.Connection = CnTo
        CmdTo.Transaction = sqltr

        CmdTo.CommandText = "Delete from Stores_Service_Receipt_Head"
        CmdTo.ExecuteNonQuery()

        Da1 = New SqlClient.SqlDataAdapter("select * from Service_Receipt_Head  Order by Service_Receipt_No", CnFrm)
        Dt1 = New DataTable
        Da1.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then

            For I = 0 To Dt1.Rows.Count - 1

                Me.Text = "Service_Receipt_Head -  " & I

                If Trim(Dt1.Rows(I).Item("Service_Receipt_Code").ToString) <> "" Then


                    CmdTo.Parameters.Clear()
                    CmdTo.Parameters.AddWithValue("@PoDate", CDate(Dt1.Rows(I).Item("Service_Receipt_Date").ToString))

                    vServiceReceipt_Code = Dt1.Rows(I).Item("Service_Receipt_Code").ToString
                    vServiceReceipt_No = Dt1.Rows(I).Item("Service_Receipt_No").ToString

                    vLedger_IdNo = Ledger_OldIdNoToNewIdNo(CnTo, Val(Dt1.Rows(I).Item("Ledger_IdNo").ToString), sqltr)
                    vCompany_IdNo = Dt1.Rows(I).Item("Company_IdNo").ToString

                    vPartyDc_No = Dt1.Rows(I).Item("Party_DcNo").ToString
                    vReceiver_Name = Dt1.Rows(I).Item("Received_Name").ToString
                    vIssue_Name = Dt1.Rows(I).Item("Issued_Name").ToString
                    vNew_old = Dt1.Rows(I).Item("New_Old").ToString
                    vParticulars = Dt1.Rows(I).Item("Particulars").ToString

                    vTotal_Quantity = Val(Dt1.Rows(I).Item("Total_Quantity").ToString)



                    CmdTo.CommandText = "Insert into Stores_Service_Receipt_Head( Service_Receipt_Code,             Company_IdNo             , Service_Receipt_No     ,                            for_OrderBy                                          , Service_Receipt_Date,      Ledger_IdNo    ,           Party_DcNo       ,           Received_Name         ,        Issued_Name        ,       New_Old             ,            Particulars      , Total_Quantity) " & _
                                    "              Values   (       '" & Trim(vServiceReceipt_Code) & "', " & Str(Val(vCompany_IdNo)) & ", '" & Trim(vServiceReceipt_No) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(vServiceReceipt_No))) & ",     @PoDate     , " & Val(vLedger_IdNo) & "','" & Trim(vPartyDc_No) & "','" & Trim(vReceiver_Name) & "','" & Trim(vIssue_Name) & "',  '" & Trim(vNew_old) & "', '" & Trim(vParticulars) & "',  " & Str(Val(vTotal_Quantity)) & ")"
                    CmdTo.ExecuteNonQuery()

                End If

            Next

        End If

        Me.Text = ""

    End Sub

    Private Sub ServiceItemReceiptDetails_Transfer(ByVal sqltr As SqlClient.SqlTransaction)
        Dim CmdFrm As New SqlClient.SqlCommand
        Dim CmdTo As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim vCompany_IdNo As Integer
        Dim vServiceReceipt_Code As String, vServiceReceipt_No As String, vService_Delivery_No As String, vServiceDelivery_Code As String
        Dim vSl_No As Integer, vItem_Idno As Integer, vBrand_IdNo As Integer, vUnit_Idno As Integer, vService_Delivery_Details_SlNo As Integer, vMachine_IdNo As Integer
        Dim vQuantity As Single

        Me.Text = "Service_Receipt_Details"

        CmdFrm.Connection = CnFrm
        CmdTo.Connection = CnTo
        CmdTo.Transaction = sqltr

        CmdTo.CommandText = "Delete from Stores_Service_Receipt_Details"
        CmdTo.ExecuteNonQuery()

        Da1 = New SqlClient.SqlDataAdapter("select * from Service_Receipt_Details  Order by Service_Receipt_No", CnFrm)
        Dt1 = New DataTable
        Da1.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then

            For I = 0 To Dt1.Rows.Count - 1

                Me.Text = "Service_Receipt_Details -  " & I

                If Trim(Dt1.Rows(I).Item("Service_Receipt_Code").ToString) <> "" Then


                    CmdTo.Parameters.Clear()
                    CmdTo.Parameters.AddWithValue("@PoDate", CDate(Dt1.Rows(I).Item("Service_Receipt_Date").ToString))

                    vServiceReceipt_Code = Dt1.Rows(I).Item("Service_Receipt_Code").ToString
                    vServiceReceipt_No = Dt1.Rows(I).Item("Service_Receipt_No").ToString

                    ' vLedger_IdNo = Dt1.Rows(0).Item("Ledger_IdNo").ToString
                    vCompany_IdNo = Dt1.Rows(I).Item("Company_IdNo").ToString


                    vSl_No = Dt1.Rows(I).Item("Sl_No").ToString
                    vItem_Idno = Dt1.Rows(I).Item("Item_Idno").ToString
                    vBrand_IdNo = Dt1.Rows(I).Item("Brand_Idno").ToString
                    vQuantity = Val(Dt1.Rows(I).Item("Quantity").ToString)
                    vUnit_Idno = Unit_OldIdNoToNewIdNo(CnTo, Val(Dt1.Rows(I).Item("Unit_IdNo").ToString), sqltr)
                    vMachine_IdNo = Val(Dt1.Rows(I).Item("Machine_Idno").ToString)


                    vService_Delivery_No = Dt1.Rows(I).Item("Service_Delivery_No").ToString
                    vServiceDelivery_Code = Dt1.Rows(I).Item("Service_Delivery_Code").ToString
                    vService_Delivery_Details_SlNo = Val(Dt1.Rows(I).Item("Service_Delivery_Details_SlNo").ToString)
                    ' vReceipt_Quantity = Val(Dt1.Rows(0).Item("Receipt_Quantity").ToString)

                    CmdTo.CommandText = "Insert into Stores_Service_Receipt_Details ( Service_Receipt_Code,       Company_IdNo             ,            Service_Receipt_No  ,                            for_OrderBy                                             , Service_Receipt_Date,             Sl_No         ,           Item_IdNo      ,            Brand_IdNo       ,             Quantity         ,          Unit_idNo      ,    Machine_idNo      ,            Service_Delivery_No       , Service_Delivery_Code,                                Service_Delivery_Details_SlNo) " & _
                                                           " Values ( '" & Trim(vServiceReceipt_Code) & "', " & Str(Val(vCompany_IdNo)) & ", '" & Trim(vServiceReceipt_No) & " ' , " & Str(Val(Common_Procedures.OrderBy_CodeToValue(vServiceReceipt_No))) & ", @PoDate            , " & Str(Val(vSl_No)) & ", " & Str(Val(vItem_Idno)) & ", " & Str(Val(vBrand_IdNo)) & ", " & Str(Val(vQuantity)) & ", " & Val(vUnit_Idno) & " ," & Val(vMachine_IdNo) & ", '" & Trim(vService_Delivery_No) & "' ,'" & Trim(vServiceDelivery_Code) & "', " & Val(vService_Delivery_Details_SlNo) & " )"
                    CmdTo.ExecuteNonQuery()
                End If

            Next

        End If

        Me.Text = ""

    End Sub
    Private Sub GatePassHead_Transfer(ByVal sqltr As SqlClient.SqlTransaction)
        Dim CmdFrm As New SqlClient.SqlCommand
        Dim CmdTo As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim vCompany_IdNo As Integer
        Dim vGatePass_Code As String, vGatePass_No As String, vSebd_To As String, vSend_Through As String, vVechile_No As String, vRemarks As String
        Dim vTotal_Quantity As Single

        Me.Text = "Gate_Pass_Head"

        CmdFrm.Connection = CnFrm
        CmdTo.Connection = CnTo
        CmdTo.Transaction = sqltr

        CmdTo.CommandText = "Delete from Stores_Gate_Pass_Head"
        CmdTo.ExecuteNonQuery()

        Da1 = New SqlClient.SqlDataAdapter("select * from Gate_Pass_Head  Order by Gate_Pass_No", CnFrm)
        Dt1 = New DataTable
        Da1.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then

            For I = 0 To Dt1.Rows.Count - 1

                Me.Text = "Gate_Pass_Head -  " & I

                If Trim(Dt1.Rows(I).Item("Service_Receipt_Code").ToString) <> "" Then


                    CmdTo.Parameters.Clear()
                    CmdTo.Parameters.AddWithValue("@PoDate", CDate(Dt1.Rows(I).Item("Gate_Pass_Date").ToString))

                    vGatePass_Code = Dt1.Rows(I).Item("Gate_Pass_Code").ToString
                    vGatePass_No = Dt1.Rows(I).Item("Gate_Pass_No").ToString

                    ' vLedger_IdNo = Dt1.Rows(0).Item("Ledger_IdNo").ToString
                    vCompany_IdNo = Dt1.Rows(I).Item("Company_IdNo").ToString

                    vSebd_To = Dt1.Rows(I).Item("Send_To").ToString
                    vSend_Through = Dt1.Rows(I).Item("Send_Through").ToString
                    vVechile_No = Dt1.Rows(I).Item("Vechile_No").ToString
                    vRemarks = Dt1.Rows(I).Item("Remarks").ToString
                   
                    vTotal_Quantity = Val(Dt1.Rows(I).Item("Total_Quantity").ToString)



                    CmdTo.CommandText = "Insert into Stores_Gate_Pass_Head(Gate_Pass_Code          ,             Company_IdNo         , Gate_Pass_No                ,                            for_OrderBy                           , Gate_Pass_Date          , Send_To          ,       Send_Through         ,           Vechile_No       ,    Remarks,         Total_Quantity) " & _
                                    "              Values   (       '" & Trim(vGatePass_Code) & "', " & Str(Val(vCompany_IdNo)) & ", '" & Trim(vGatePass_No) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(vGatePass_No))) & ",     @PoDate     , " & Val(vSebd_To) & "','" & Trim(vSend_Through) & "','" & Trim(vVechile_No) & "','" & Trim(vRemarks) & "', " & Str(Val(vTotal_Quantity)) & ")"
                    CmdTo.ExecuteNonQuery()

                End If

            Next

        End If

        Me.Text = ""

    End Sub

    Private Sub GatePassDetails_Transfer(ByVal sqltr As SqlClient.SqlTransaction)
        Dim CmdFrm As New SqlClient.SqlCommand
        Dim CmdTo As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim vCompany_IdNo As Integer
        Dim vGatePass_Code As String, vGatePass_No As String, vEntry_Id As String, vEntry_Code As String
        Dim vSl_No As Integer, vItem_Idno As Integer, vBrand_IdNo As Integer, vUnit_Idno As Integer
        Dim vQuantity As Single

        Me.Text = "Gate_Pass_Details"

        CmdFrm.Connection = CnFrm
        CmdTo.Connection = CnTo
        CmdTo.Transaction = sqltr

        CmdTo.CommandText = "Delete from Stores_Gate_Pass_Details"
        CmdTo.ExecuteNonQuery()

        Da1 = New SqlClient.SqlDataAdapter("select * from Gate_Pass_Details  Order by Gate_Pass_No", CnFrm)
        Dt1 = New DataTable
        Da1.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then

            For I = 0 To Dt1.Rows.Count - 1

                Me.Text = "Gate_Pass_Details -  " & I

                If Trim(Dt1.Rows(I).Item("Gate_Pass_Code").ToString) <> "" Then


                    CmdTo.Parameters.Clear()
                    CmdTo.Parameters.AddWithValue("@PoDate", CDate(Dt1.Rows(I).Item("Gate_Pass_Date").ToString))

                    vGatePass_Code = Dt1.Rows(I).Item("Gate_Pass_Code").ToString
                    vGatePass_No = Dt1.Rows(I).Item("Gate_Pass_No").ToString

                    'vLedger_IdNo = Dt1.Rows(0).Item("Ledger_IdNo").ToString
                    vCompany_IdNo = Dt1.Rows(I).Item("Company_IdNo").ToString


                    vSl_No = Dt1.Rows(I).Item("Sl_No").ToString
                    vItem_Idno = Dt1.Rows(I).Item("Item_Idno").ToString
                    vBrand_IdNo = Dt1.Rows(I).Item("Brand_Idno").ToString
                    vQuantity = Val(Dt1.Rows(I).Item("Quantity").ToString)
                    vUnit_Idno = Unit_OldIdNoToNewIdNo(CnTo, Val(Dt1.Rows(I).Item("Unit_IdNo").ToString), sqltr)
                    'vMachine_IdNo = Val(Dt1.Rows(I).Item("Machine_Idno").ToString)


                    vEntry_Id = Dt1.Rows(I).Item("Entry_Id").ToString
                    vEntry_Code = Dt1.Rows(I).Item("Entry_Code").ToString

                    CmdTo.CommandText = "Insert into Stores_Gate_Pass_Details ( Gate_Pass_Code      ,            Company_IdNo        ,         Gate_Pass_No       ,                           for_OrderBy                                  , Gate_Pass_Date      ,             Sl_No     ,           Item_IdNo          ,           Brand_IdNo           ,            Quantity       ,          Unit_idNo      ,             Entry_Id   , Entry_Code) " & _
                                                           " Values ( '" & Trim(vGatePass_Code) & "', " & Str(Val(vCompany_IdNo)) & ", '" & Trim(vGatePass_No) & "'  , " & Str(Val(Common_Procedures.OrderBy_CodeToValue(vGatePass_No))) & ", @PoDate            , " & Str(Val(vSl_No)) & ", " & Str(Val(vItem_Idno)) & ", " & Str(Val(vBrand_IdNo)) & ", " & Str(Val(vQuantity)) & ", " & Val(vUnit_Idno) & " ,'" & Trim(vEntry_Id) & "', '" & Trim(vEntry_Code) & "'  )"
                    CmdTo.ExecuteNonQuery()
                End If

            Next

        End If

        Me.Text = ""

    End Sub

    Private Sub ItemExcessShortHead_Transfer(ByVal sqltr As SqlClient.SqlTransaction)
        Dim CmdFrm As New SqlClient.SqlCommand
        Dim CmdTo As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim vCompany_IdNo As Integer
        Dim vItemExcessShort_Code As String, vItemExcessShort_No As String, vItem_Excess_Short As String
        Dim vItem_Idno As Integer, vBrand_IdNo As Integer, vUnit_Idno As Integer
        Dim vQty_New As Single, vQty_old_Usable As Single, vQty_Old_Scrap As Single

        Me.Text = "Item_Excess_Short_Entry_Head"

        CmdFrm.Connection = CnFrm
        CmdTo.Connection = CnTo
        CmdTo.Transaction = sqltr

        CmdTo.CommandText = "Delete from Stores_Item_Excess_Short_Entry_Head"
        CmdTo.ExecuteNonQuery()

        Da1 = New SqlClient.SqlDataAdapter("select * from Item_Excess_Short_Entry_Head Order by Item_Excess_Short_No", CnFrm)
        Dt1 = New DataTable
        Da1.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then

            For I = 0 To Dt1.Rows.Count - 1

                Me.Text = "Item_Excess_Short_Entry_Head -  " & I

                If Trim(Dt1.Rows(I).Item("Item_Excess_Short_Code").ToString) <> "" Then


                    CmdTo.Parameters.Clear()
                    CmdTo.Parameters.AddWithValue("@PoDate", CDate(Dt1.Rows(I).Item("Item_Excess_Short_Date").ToString))

                    vItemExcessShort_Code = Dt1.Rows(I).Item("Item_Excess_Short_Code").ToString
                    vItemExcessShort_No = Dt1.Rows(I).Item("Item_Excess_Short_No").ToString

                    'vLedger_IdNo = Dt1.Rows(0).Item("Ledger_IdNo").ToString
                    vCompany_IdNo = Dt1.Rows(I).Item("Company_IdNo").ToString


                    vItem_Idno = Dt1.Rows(I).Item("Item_Idno").ToString
                    vBrand_IdNo = Dt1.Rows(I).Item("Brand_Idno").ToString
                    vUnit_Idno = Unit_OldIdNoToNewIdNo(CnTo, Val(Dt1.Rows(I).Item("Unit_IdNo").ToString), sqltr)
                    vQty_New = Val(Dt1.Rows(I).Item("Qty_New").ToString)
                    vQty_Old_Scrap = Val(Dt1.Rows(I).Item("Qty_Old_Scrap").ToString)
                    vQty_old_Usable = Val(Dt1.Rows(I).Item("Qty_Old_Usable").ToString)
                    vItem_Excess_Short = Dt1.Rows(I).Item("Item_Excess_Short").ToString

                    CmdTo.CommandText = "Insert into Stores_Item_Excess_Short_Entry_Head(Item_Excess_Short_Code             ,            Company_IdNo            ,        Item_Excess_Short_No      ,                           for_OrderBy                                            , Item_Excess_Short_Date, Item_IdNo                     ,              Brand_IdNo    ,        Unit_IdNo         ,       Qty_New            , Qty_Old_Usable              , Qty_Old_Scrap         , Item_Excess_Short) " & _
                                                           " Values (                     '" & Trim(vItemExcessShort_Code) & "', " & Str(Val(vCompany_IdNo)) & ", '" & Trim(vItemExcessShort_No) & "'  , " & Str(Val(Common_Procedures.OrderBy_CodeToValue(vItemExcessShort_No))) & ", @PoDate            ,       " & Str(Val(vItem_Idno)) & ", " & Str(Val(vBrand_IdNo)) & ", " & Str(Val(vUnit_Idno)) & ", " & Val(vQty_New) & " ," & Val(vQty_old_Usable) & ", " & Val(vQty_Old_Scrap) & ",  '" & Trim(vItem_Excess_Short) & "'  )"
                    CmdTo.ExecuteNonQuery()
                End If

            Next

        End If

        Me.Text = ""

    End Sub

    Private Sub ItemDisposeHead_Transfer(ByVal sqltr As SqlClient.SqlTransaction)
        Dim CmdFrm As New SqlClient.SqlCommand
        Dim CmdTo As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim vCompany_IdNo As Integer
        Dim vDispose_Code As String, vDispose_No As String
        Dim vItem_Idno As Integer, vBrand_IdNo As Integer, vUnit_Idno As Integer
        Dim vQty_New As Single, vQty_old_Usable As Single, vQty_Old_Scrap As Single

        Me.Text = "Dispose_Entry_Head"

        CmdFrm.Connection = CnFrm
        CmdTo.Connection = CnTo
        CmdTo.Transaction = sqltr

        CmdTo.CommandText = "Delete from Stores_Dispose_Entry_Head"
        CmdTo.ExecuteNonQuery()

        Da1 = New SqlClient.SqlDataAdapter("select * from Dispose_Entry_Head Order by Dispose_No", CnFrm)
        Dt1 = New DataTable
        Da1.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then

            For I = 0 To Dt1.Rows.Count - 1

                Me.Text = "Dispose_Entry_Head -  " & I

                If Trim(Dt1.Rows(I).Item("Dispose_Code").ToString) <> "" Then


                    CmdTo.Parameters.Clear()
                    CmdTo.Parameters.AddWithValue("@PoDate", CDate(Dt1.Rows(I).Item("Dispose_Date").ToString))

                    vDispose_Code = Dt1.Rows(I).Item("Dispose_Code").ToString
                    vDispose_No = Dt1.Rows(I).Item("Dispose_No").ToString

                    'vLedger_IdNo = Dt1.Rows(0).Item("Ledger_IdNo").ToString
                    vCompany_IdNo = Dt1.Rows(I).Item("Company_IdNo").ToString


                    vItem_Idno = Dt1.Rows(I).Item("Item_Idno").ToString
                    vBrand_IdNo = Dt1.Rows(I).Item("Brand_Idno").ToString
                    vUnit_Idno = Unit_OldIdNoToNewIdNo(CnTo, Val(Dt1.Rows(I).Item("Unit_IdNo").ToString), sqltr)
                    vQty_New = Val(Dt1.Rows(I).Item("Qty_New").ToString)
                    vQty_Old_Scrap = Val(Dt1.Rows(I).Item("Qty_Old_Scrap").ToString)
                    vQty_old_Usable = Val(Dt1.Rows(I).Item("Qty_Old_Usable").ToString)
                    ' vItem_Excess_Short = Dt1.Rows(0).Item("Item_Excess_Short").ToString

                    CmdTo.CommandText = "Insert into Stores_Dispose_Entry_Head(Dispose_Code         ,              Company_IdNo      ,           Dispose_No          ,                                   for_OrderBy                      , Dispose_Date        ,            Item_IdNo              ,         Brand_IdNo          ,            Unit_IdNo            ,    Qty_New          ,         Qty_Old_Usable          , Qty_Old_Scrap) " & _
                                                           " Values (    '" & Trim(vDispose_Code) & "', " & Str(Val(vCompany_IdNo)) & ", '" & Trim(vDispose_No) & " ' , " & Str(Val(Common_Procedures.OrderBy_CodeToValue(vDispose_No))) & ", @PoDate            ,       " & Str(Val(vItem_Idno)) & ", " & Str(Val(vBrand_IdNo)) & ", " & Str(Val(vUnit_Idno)) & ", " & Val(vQty_New) & " ," & Val(vQty_old_Usable) & ", " & Val(vQty_Old_Scrap) & "  )"
                    CmdTo.ExecuteNonQuery()
                End If

            Next

        End If

        Me.Text = ""

    End Sub
    Private Sub OilServiceHead_Transfer(ByVal sqltr As SqlClient.SqlTransaction)
        Dim CmdFrm As New SqlClient.SqlCommand
        Dim CmdTo As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim vCompany_IdNo As Integer
        Dim vOil_Service_Code As String, vOil_Service_No As String, vEmployee_NAme As String, vRemarks As String
        Dim vMachine_Idno As Integer

        Me.Text = "Oil_Service_Entry_Head"

        CmdFrm.Connection = CnFrm
        CmdTo.Connection = CnTo
        CmdTo.Transaction = sqltr

        CmdTo.CommandText = "Delete from Stores_Oil_Service_Entry_Head"
        CmdTo.ExecuteNonQuery()

        Da1 = New SqlClient.SqlDataAdapter("select * from Oil_Service_Entry_Head  Order by Oil_Service_No", CnFrm)
        Dt1 = New DataTable
        Da1.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then

            For I = 0 To Dt1.Rows.Count - 1

                Me.Text = "Oil_Service_Entry_Head -  " & I

                If Trim(Dt1.Rows(I).Item("Oil_Service_Code").ToString) <> "" Then


                    CmdTo.Parameters.Clear()
                    CmdTo.Parameters.AddWithValue("@PoDate", CDate(Dt1.Rows(I).Item("Oil_Service_Date").ToString))

                    vOil_Service_Code = Dt1.Rows(I).Item("Oil_Service_Code").ToString
                    vOil_Service_No = Dt1.Rows(I).Item("Oil_Service_No").ToString

                    'vLedger_IdNo = Dt1.Rows(0).Item("Ledger_IdNo").ToString
                    vCompany_IdNo = Dt1.Rows(I).Item("Company_IdNo").ToString


                    vMachine_Idno = Dt1.Rows(I).Item("Machine_IdNo").ToString
                    vEmployee_NAme = Dt1.Rows(I).Item("Employe_Name").ToString
                    vRemarks = Dt1.Rows(I).Item("Remarks").ToString
                    

                    CmdTo.CommandText = "Insert into Stores_Oil_Service_Entry_Head(Oil_Service_Code       ,         Company_IdNo           ,        Oil_Service_No           ,                                    for_OrderBy                             , Oil_Service_Date,             Machine_IdNo              , Employe_Name              ,        Remarks               ) " & _
                                                           " Values (    '" & Trim(vOil_Service_Code) & "', " & Str(Val(vCompany_IdNo)) & ", '" & Trim(vOil_Service_No) & "'  , " & Str(Val(Common_Procedures.OrderBy_CodeToValue(vOil_Service_No))) & ", @PoDate            ,       " & Str(Val(vMachine_Idno)) & ", '" & Trim(vEmployee_NAme) & "', '" & Trim(vRemarks) & "'  )"
                    CmdTo.ExecuteNonQuery()
                End If

            Next

        End If

        Me.Text = ""

    End Sub

    Public Shared Function Ledger_OldIdNoToNewIdNo(ByVal Cn1 As SqlClient.SqlConnection, ByVal vLed_IdNo As Integer, Optional ByVal sqltr As SqlClient.SqlTransaction = Nothing) As Integer
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim vLede_Idno As Integer

        Da = New SqlClient.SqlDataAdapter("select Ledger_idno from Ledger_Head where Old_Ledger_IdNo = " & Str(Val(vLed_IdNo)), Cn1)
        If IsNothing(sqltr) = False Then
            Da.SelectCommand.Transaction = sqltr
        End If
        Da.Fill(Dt)

        vLede_Idno = 0
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                vLede_Idno = Val(Dt.Rows(0)(0).ToString)
            End If
        End If

        Dt.Dispose()
        Da.Dispose()

        Ledger_OldIdNoToNewIdNo = Val(vLede_Idno)

    End Function

    Public Shared Function Area_OldIdNoToNewIdNo(ByVal Cn1 As SqlClient.SqlConnection, ByVal vareaOld_IdNo As Integer, Optional ByVal sqltr As SqlClient.SqlTransaction = Nothing) As Integer
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim vArea_Idno As Integer

        Da = New SqlClient.SqlDataAdapter("select Area_idno from Area_Head where Old_Area_IdNo = " & Str(Val(vareaOld_IdNo)), Cn1)
        If IsNothing(sqltr) = False Then
            Da.SelectCommand.Transaction = sqltr
        End If
        Da.Fill(Dt)

        vArea_Idno = 0
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                vArea_Idno = Val(Dt.Rows(0)(0).ToString)
            End If
        End If

        Dt.Dispose()
        Da.Dispose()

        Area_OldIdNoToNewIdNo = Val(vArea_Idno)

    End Function

    Public Shared Function Count_OldIdNoToNewIdNo(ByVal Cn1 As SqlClient.SqlConnection, ByVal vCountOld_IdNo As Integer, Optional ByVal sqltr As SqlClient.SqlTransaction = Nothing) As Integer
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim vCount_Idno As Integer

        Da = New SqlClient.SqlDataAdapter("select Count_idno from Count_Head where Old_Count_IdNo = " & Str(Val(vCountOld_IdNo)), Cn1)
        If IsNothing(sqltr) = False Then
            Da.SelectCommand.Transaction = sqltr
        End If
        Da.Fill(Dt)

        vCount_Idno = 0
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                vCount_Idno = Val(Dt.Rows(0)(0).ToString)
            End If
        End If

        Dt.Dispose()
        Da.Dispose()

        Count_OldIdNoToNewIdNo = Val(vCount_Idno)

    End Function

    Public Shared Function Unit_OldIdNoToNewIdNo(ByVal Cn1 As SqlClient.SqlConnection, ByVal vUnitOld_IdNo As Integer, Optional ByVal sqltr As SqlClient.SqlTransaction = Nothing) As Integer
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim vUnit_Idno As Integer

        Da = New SqlClient.SqlDataAdapter("select Unit_idno from Unit_Head where Old_Unit_IdNo = " & Str(Val(vUnitOld_IdNo)), Cn1)
        If IsNothing(sqltr) = False Then
            Da.SelectCommand.Transaction = sqltr
        End If
        Da.Fill(Dt)

        vUnit_Idno = 0
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                vUnit_Idno = Val(Dt.Rows(0)(0).ToString)
            End If
        End If

        Dt.Dispose()
        Da.Dispose()

        Unit_OldIdNoToNewIdNo = Val(vUnit_Idno)

    End Function

End Class