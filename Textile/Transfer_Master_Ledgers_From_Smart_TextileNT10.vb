Imports System.IO

Public Class Transfer_Master_Ledgers_From_Smart_TextileNT10

    Private CnTo As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private CnFrm As New SqlClient.SqlConnection

    Private Sub Transfer_Master_Ledgers_From_Smart_TextileNT10_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        If cbo_DBFrom.Enabled And cbo_DBFrom.Visible Then cbo_DBFrom.Focus()
    End Sub

    Private Sub Transfer_Master_Ledgers_From_Smart_TextileNT10_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(cbo_DBFrom, CnTo, "master..sysdatabases", "name", "(name LIKE 'Smart%NT10%')", "")

        cbo_DBFrom.Text = ""
        Me.Text = "MASTERS TRANSFER"

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

        Try

            AccountsGroupHead_Transfer(tr)
            LedgerHead_Transfer(tr)
            LedgerDetails_Transfer(tr)

            AreaHead_Transfer(tr)
            ItemGroupHead_Transfer(tr)
            ProcessHead_Transfer(tr)
            ColourHead_Transfer(tr)
            LotNoHead_Transfer(tr)
            RackHead_Transfer(tr)
            UnitHead_Transfer(tr)

            ProcessItemHead_Transfer(tr)
            ProcessedItemSalesName_Transfer(tr)
            ProcessedItemSalesNameDetails_Transfer(tr)
            ProcessedItemDetails_Transfer(tr)

            Ledger_Opening_Transfer(tr)

            tr.Commit()

            Me.Text = "MASTERS TRANSFER"

            MDIParent1.Cursor = Cursors.Default
            Me.Cursor = Cursors.Default

            MessageBox.Show("All Masters Transfered Sucessfully", "FOR MASTERS TRANSFER...", MessageBoxButtons.OK, MessageBoxIcon.Information)

            btn_Transfer.Enabled = True

        Catch ex As Exception

            tr.Rollback()
            Me.Text = "MASTERS TRANSFER"
            MDIParent1.Cursor = Cursors.Default
            Me.Cursor = Cursors.Default
            btn_Transfer.Enabled = True
            MessageBox.Show(ex.Message, "INVALID TRANSFER...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            CnFrm.Close()
            CnTo.Close()
            tr.Dispose()

            btn_Transfer.Enabled = True
            Me.Text = "MASTERS TRANSFER"

            MDIParent1.Cursor = Cursors.Default
            Me.Cursor = Cursors.Default

        End Try

    End Sub

    Private Sub AccountsGroupHead_Transfer(ByVal sqltr As SqlClient.SqlTransaction)
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
      
        Me.Text = "Group_Head"

        CmdFrm.Connection = CnFrm
        CmdTo.Connection = CnTo
        CmdTo.Transaction = sqltr

        CmdTo.CommandText = "Delete from AccountsGroup_Head where AccountsGroup_IdNo > 32 "
        CmdTo.ExecuteNonQuery()

        Da1 = New SqlClient.SqlDataAdapter("select * from Group_Head where GROUP_IDNO > 30 Order by GROUP_IDNO", CnFrm)
        Dt1 = New DataTable
        Da1.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then

            For I = 0 To Dt1.Rows.Count - 1

                Me.Text = "Group_Head  -  " & I

                vSurNm = Common_Procedures.Remove_NonCharacters(Dt1.Rows(I).Item("Group_Name").ToString)
                vOldLID = Val(Common_Procedures.get_FieldValue(CnTo, "AccountsGroup_Head", "AccountsGroup_IdNo", "(AccountsGroup_Name = '" & Trim(vSurNm) & "')", , sqltr))
                If vOldLID = 0 Then

                    vAccountsGroup_IdNo = Val(Dt1.Rows(I).Item("GROUP_IDNO").ToString)
                    If Val(vAccountsGroup_IdNo) > 30 Then
                        vAccountsGroup_IdNo = vAccountsGroup_IdNo + 2
                    End If

                    vAccountsGroup_Name = Replace(Dt1.Rows(I).Item("Group_Name").ToString, "'", "")

                    vSur_Name = Common_Procedures.Remove_NonCharacters(vAccountsGroup_Name)

                    vParent_Name = Replace(Dt1.Rows(I).Item("Parent_Name").ToString, "'", "")

                    vParent_Idno = Replace(Dt1.Rows(I).Item("Parent_Idno").ToString, "'", "")

                    Erase AccGrpAr
                    AccGrpAr = Split(Trim(vParent_Idno), "~")
                    Inc = -1
                    Inc = Inc + 1
                    If UBound(AccGrpAr) >= Inc Then AccGrp1 = Trim(AccGrpAr(Inc))
                    If Val(AccGrp1) > 30 Then AccGrp1 = AccGrp1 + 2
                    Inc = Inc + 1
                    If UBound(AccGrpAr) >= Inc Then AccGrp2 = Trim(AccGrpAr(Inc))
                    If Val(AccGrp2) > 30 Then AccGrp2 = AccGrp2 + 2
                    Inc = Inc + 1
                    If UBound(AccGrpAr) >= Inc Then AccGrp3 = Trim(AccGrpAr(Inc))

                    If Trim(AccGrp3) <> "" Then
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

                    CmdTo.CommandText = "Insert into AccountsGroup_Head ( AccountsGroup_IdNo ,            AccountsGroup_Name      ,            Sur_Name      ,          Parent_Name        ,            Parent_Idno      ,              Carried_Balance      ,                Order_Position      ,            TallyName      ,            TallySubName      ,              Indicate       ) " & _
                                        "       Values (" & Str(Val(vAccountsGroup_IdNo)) & ", '" & Trim(vAccountsGroup_Name) & "', '" & Trim(vSur_Name) & "', '" & Trim(vParent_Name) & "', '" & Trim(vParent_Idno) & "', " & Str(Val(vCarried_Balance)) & ",   " & Str(Val(vOrder_Position)) & ", '" & Trim(vTallyName) & "', '" & Trim(vTallySubName) & "', " & Str(Val(vIndicate)) & " ) "
                    CmdTo.ExecuteNonQuery()

                End If

            Next

        End If

        Me.Text = ""

    End Sub

    Private Sub LedgerHead_Transfer(ByVal sqltr As SqlClient.SqlTransaction)
        Dim CmdFrm As New SqlClient.SqlCommand
        Dim CmdTo As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim vLedger_IdNo As Integer, vOldLID As Integer
        Dim vLedger_Name As String, vSur_Name As String, vLedger_MainName As String, vLedger_AlaisName As String
        Dim vArea_IdNo As Integer, vAccountsGroup_IdNo As Integer, vParent_Code As String, vBill_Type As String
        Dim vLedger_Address1 As String, vLedger_Address2 As String, vLedger_Address3 As String, vLedger_Address4 As String, vLedger_PhoneNo As String
        Dim vLedger_TinNo As String, vLedger_CstNo As String, vLedger_Type As String, vPan_No As String
        Dim vLedger_Emailid As String, vLedger_FaxNo As String, vLedger_MobileNo As String, vContact_Person As String
        Dim vPackingType_CompanyIdNo As Integer, vLedger_AgentIdNo As Integer, vTransport_Name As String, vNote As String
        Dim vMobileNo_Sms As String, vBilling_Type As String, vSticker_Type As String, vMrp_Perc As String
        Dim vLedNm As String

        Me.Text = "Ledger_Head"

        CmdFrm.Connection = CnFrm
        CmdTo.Connection = CnTo
        CmdTo.Transaction = sqltr

        CmdTo.CommandText = "Delete from Ledger_Head where Ledger_IdNo > 20"
        CmdTo.ExecuteNonQuery()

        Da1 = New SqlClient.SqlDataAdapter("select * from Ledger_Head where Ledger_IdNo > 20 Order by Ledger_IdNo", CnFrm)
        Dt1 = New DataTable
        Da1.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then

            For I = 0 To Dt1.Rows.Count - 1

                Me.Text = "Ledger_Head  -  " & I

                vLedNm = Common_Procedures.Remove_NonCharacters(Dt1.Rows(I).Item("Ledger_Name").ToString)
                vOldLID = Val(Common_Procedures.get_FieldValue(CnTo, "Ledger_Head", "Ledger_IdNo", "(Sur_Name = '" & Trim(vLedNm) & "')", , sqltr))

                If vOldLID = 0 Then

                    vLedger_IdNo = Val(Dt1.Rows(I).Item("Ledger_IdNo").ToString) + 80

                    vLedger_Name = Replace(Dt1.Rows(I).Item("Ledger_Name").ToString, "'", "")
                    vLedger_MainName = vLedger_Name
                    vSur_Name = Common_Procedures.Remove_NonCharacters(vLedger_Name)
                    vLedger_AlaisName = ""
                    vArea_IdNo = Val(Dt1.Rows(I).Item("Area_Idno").ToString)

                    vParent_Code = Dt1.Rows(I).Item("Parent_Code").ToString

                    vAccountsGroup_IdNo = Common_Procedures.get_FieldValue(CnTo, "AccountsGroup_Head", "AccountsGroup_IdNo", "(Parent_Idno = '" & Trim(vParent_Code) & "')", , sqltr)

                    If Trim(UCase(Dt1.Rows(I).Item("Bill_Maintenance").ToString)) = "BILL TO BILL CLEAR" Then
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
                    vPan_No = Dt1.Rows(I).Item("Pan_No").ToString
                    vLedger_Emailid = Replace(Dt1.Rows(I).Item("Email_Id").ToString, "'", "")
                    vLedger_FaxNo = Dt1.Rows(I).Item("Fax_No").ToString
                    vLedger_MobileNo = Dt1.Rows(I).Item("Mobile_No").ToString
                    vContact_Person = Replace(Dt1.Rows(I).Item("Contact_Person").ToString, "'", "")
                    vPackingType_CompanyIdNo = Val(Dt1.Rows(I).Item("PackingType_Idno").ToString)
                    vLedger_AgentIdNo = Val(Dt1.Rows(I).Item("Agent_Idno").ToString) + 80
                    vTransport_Name = Replace(Dt1.Rows(I).Item("Transport_Name").ToString, "'", "")
                    vNote = Replace(Dt1.Rows(I).Item("Note").ToString, "'", "")
                    vMobileNo_Sms = Dt1.Rows(I).Item("Mobile_No").ToString
                    vBilling_Type = ""
                    vSticker_Type = ""
                    vMrp_Perc = ""

                    CmdTo.CommandText = "Insert into ledger_head ( Ledger_IdNo        ,            Ledger_Name      ,            Sur_Name      ,            Ledger_MainName      ,            Ledger_AlaisName      ,              Area_IdNo      ,              AccountsGroup_IdNo      ,            Parent_Code      ,            Bill_Type      ,            Ledger_Address1      ,            Ledger_Address2      ,            Ledger_Address3      ,            Ledger_Address4      ,            Ledger_PhoneNo      ,            Ledger_TinNo      ,            Ledger_CstNo      ,            Ledger_Type      ,            Pan_No      ,            Ledger_Emailid      ,            Ledger_FaxNo      ,            Ledger_MobileNo      ,            Contact_Person      ,             PackingType_CompanyIdNo       ,              Ledger_AgentIdNo      ,            Transport_Name      ,            Note      ,            MobileNo_Sms      ,            Billing_Type      ,            Sticker_Type      ,            Mrp_Perc      , Verified_Status ) " & _
                                        "       Values (" & Str(Val(vLedger_IdNo)) & ", '" & Trim(vLedger_Name) & "', '" & Trim(vSur_Name) & "', '" & Trim(vLedger_MainName) & "', '" & Trim(vLedger_AlaisName) & "', " & Str(Val(vArea_IdNo)) & ", " & Str(Val(vAccountsGroup_IdNo)) & ", '" & Trim(vParent_Code) & "', '" & Trim(vBill_Type) & "', '" & Trim(vLedger_Address1) & "', '" & Trim(vLedger_Address2) & "', '" & Trim(vLedger_Address3) & "', '" & Trim(vLedger_Address4) & "', '" & Trim(vLedger_PhoneNo) & "', '" & Trim(vLedger_TinNo) & "', '" & Trim(vLedger_CstNo) & "', '" & Trim(vLedger_Type) & "', '" & Trim(vPan_No) & "', '" & Trim(vLedger_Emailid) & "', '" & Trim(vLedger_FaxNo) & "', '" & Trim(vLedger_MobileNo) & "', '" & Trim(vContact_Person) & "', " & Str(Val(vPackingType_CompanyIdNo)) & ", " & Str(Val(vLedger_AgentIdNo)) & ", '" & Trim(vTransport_Name) & "', '" & Trim(vNote) & "', '" & Trim(vMobileNo_Sms) & "', '" & Trim(vBilling_Type) & "', '" & Trim(vSticker_Type) & "', '" & Trim(vMrp_Perc) & "',        1        ) "
                    CmdTo.ExecuteNonQuery()

                End If

            Next

            CmdTo.CommandText = "truncate table Ledger_AlaisHead"
            CmdTo.ExecuteNonQuery()

            Da1 = New SqlClient.SqlDataAdapter("select * from Ledger_Head ", CnTo)
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
    Private Sub AreaHead_Transfer(ByVal sqltr As SqlClient.SqlTransaction)
        Dim CmdFrm As New SqlClient.SqlCommand
        Dim CmdTo As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim vArea_IdNo As Integer, vOldLID As Integer
        Dim vArea_Name As String, vSur_Name As String
        Dim vareaNm As String

        Me.Text = "Area_Head"

        CmdFrm.Connection = CnFrm
        CmdTo.Connection = CnTo
        CmdTo.Transaction = sqltr

        CmdTo.CommandText = "Delete from Area_Head "
        CmdTo.ExecuteNonQuery()

        Da1 = New SqlClient.SqlDataAdapter("select * from Area_Head Order by Area_IdNo", CnFrm)
        Dt1 = New DataTable
        Da1.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then

            For I = 0 To Dt1.Rows.Count - 1

                Me.Text = "Area_Head  -  " & I

                vareaNm = Common_Procedures.Remove_NonCharacters(Dt1.Rows(I).Item("Area_Name").ToString)
                vOldLID = Val(Common_Procedures.get_FieldValue(CnTo, "Area_Head", "Area_IdNo", "(Sur_Name = '" & Trim(vareaNm) & "')", , sqltr))

                If vOldLID = 0 Then

                    vArea_IdNo = Val(Dt1.Rows(I).Item("Area_Idno").ToString)

                    vArea_Name = Replace(Dt1.Rows(I).Item("Area_Name").ToString, "'", "")

                    vSur_Name = Common_Procedures.Remove_NonCharacters(vArea_Name)


                    CmdTo.CommandText = "Insert into Area_Head ( Area_Idno        ,            Area_Name      ,            Sur_Name   ) " & _
                                        "       Values (" & Str(Val(vArea_IdNo)) & ", '" & Trim(vArea_Name) & "', '" & Trim(vSur_Name) & "') "
                    CmdTo.ExecuteNonQuery()

                End If

            Next

        End If

        Me.Text = ""

    End Sub
    Private Sub ItemGroupHead_Transfer(ByVal sqltr As SqlClient.SqlTransaction)
        Dim CmdFrm As New SqlClient.SqlCommand
        Dim CmdTo As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim vItmGrp_IdNo As Integer, vOldLID As Integer
        Dim vItmGrp_Name As String, vSur_Name As String
        Dim vItmGrpNm As String

        Me.Text = "ItemGroup_Head"

        CmdFrm.Connection = CnFrm
        CmdTo.Connection = CnTo
        CmdTo.Transaction = sqltr

        CmdTo.CommandText = "Delete from ItemGroup_Head "
        CmdTo.ExecuteNonQuery()

        Da1 = New SqlClient.SqlDataAdapter("select * from ItemGroup_Head Order by ItemGroup_Idno", CnFrm)
        Dt1 = New DataTable
        Da1.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then

            For I = 0 To Dt1.Rows.Count - 1

                Me.Text = "ItemGroup_Head  -  " & I

                vItmGrpNm = Common_Procedures.Remove_NonCharacters(Dt1.Rows(I).Item("ItemGroup_Name").ToString)
                vOldLID = Val(Common_Procedures.get_FieldValue(CnTo, "ItemGroup_Head", "ItemGroup_Idno", "(Sur_Name = '" & Trim(vItmGrpNm) & "')", , sqltr))

                If vOldLID = 0 Then

                    vItmGrp_IdNo = Val(Dt1.Rows(I).Item("ItemGroup_Idno").ToString)

                    vItmGrp_Name = Replace(Dt1.Rows(I).Item("ItemGroup_Name").ToString, "'", "")

                    vSur_Name = Common_Procedures.Remove_NonCharacters(vItmGrp_Name)

                    CmdTo.CommandText = "Insert into ItemGroup_Head ( ItemGroup_Idno        ,            ItemGroup_Name      ,            Sur_Name   ) " & _
                                        "       Values (" & Str(Val(vItmGrp_IdNo)) & ", '" & Trim(vItmGrp_Name) & "', '" & Trim(vSur_Name) & "') "
                    CmdTo.ExecuteNonQuery()

                End If

            Next

        End If

        Me.Text = ""

    End Sub

    Private Sub ProcessHead_Transfer(ByVal sqltr As SqlClient.SqlTransaction)
        Dim CmdFrm As New SqlClient.SqlCommand
        Dim CmdTo As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim vProcess_IdNo As Integer, vOldLID As Integer
        Dim vProcess_Name As String, vSur_Name As String
        Dim vProcessNm As String

        Me.Text = "Process_Head"

        CmdFrm.Connection = CnFrm
        CmdTo.Connection = CnTo
        CmdTo.Transaction = sqltr

        CmdTo.CommandText = "Delete from Process_Head "
        CmdTo.ExecuteNonQuery()

        Da1 = New SqlClient.SqlDataAdapter("select * from Processing_Head Order by Processing_Idno", CnFrm)
        Dt1 = New DataTable
        Da1.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then

            For I = 0 To Dt1.Rows.Count - 1

                Me.Text = "Process_Head  -  " & I

                vProcessNm = Common_Procedures.Remove_NonCharacters(Dt1.Rows(I).Item("Processing_Name").ToString)
                vOldLID = Val(Common_Procedures.get_FieldValue(CnTo, "Process_Head", "Process_Idno", "(Sur_Name = '" & Trim(vProcessNm) & "')", , sqltr))

                If vOldLID = 0 Then

                    vProcess_IdNo = Val(Dt1.Rows(I).Item("Processing_Idno").ToString)

                    vProcess_Name = Replace(Dt1.Rows(I).Item("Processing_Name").ToString, "'", "")

                    vSur_Name = Common_Procedures.Remove_NonCharacters(vProcess_Name)

                    CmdTo.CommandText = "Insert into Process_Head ( Process_Idno        ,            Process_Name      ,            Sur_Name   ) " & _
                                        "       Values (" & Str(Val(vProcess_IdNo)) & ", '" & Trim(vProcess_Name) & "', '" & Trim(vSur_Name) & "') "
                    CmdTo.ExecuteNonQuery()

                End If

            Next

        End If

        Me.Text = ""

    End Sub
    Private Sub ColourHead_Transfer(ByVal sqltr As SqlClient.SqlTransaction)
        Dim CmdFrm As New SqlClient.SqlCommand
        Dim CmdTo As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim vColour_IdNo As Integer, vOldLID As Integer
        Dim vColour_Name As String, vSur_Name As String
        Dim vColourNm As String

        Me.Text = "Colour_Head"

        CmdFrm.Connection = CnFrm
        CmdTo.Connection = CnTo
        CmdTo.Transaction = sqltr

        CmdTo.CommandText = "Delete from Colour_Head "
        CmdTo.ExecuteNonQuery()

        Da1 = New SqlClient.SqlDataAdapter("select * from Colour_Head Order by Colour_Idno", CnFrm)
        Dt1 = New DataTable
        Da1.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then

            For I = 0 To Dt1.Rows.Count - 1

                Me.Text = "Colour_Head  -  " & I

                vColourNm = Common_Procedures.Remove_NonCharacters(Dt1.Rows(I).Item("Colour_Name").ToString)
                vOldLID = Val(Common_Procedures.get_FieldValue(CnTo, "Colour_Head", "Colour_Idno", "(Sur_Name = '" & Trim(vColourNm) & "')", , sqltr))

                If vOldLID = 0 Then

                    vColour_IdNo = Val(Dt1.Rows(I).Item("Colour_Idno").ToString)

                    vColour_Name = Replace(Dt1.Rows(I).Item("Colour_Name").ToString, "'", "")

                    vSur_Name = Common_Procedures.Remove_NonCharacters(vColour_Name)

                    CmdTo.CommandText = "Insert into Colour_Head ( Colour_Idno        ,            Colour_Name      ,            Sur_Name   ) " & _
                                        "       Values (" & Str(Val(vColour_IdNo)) & ", '" & Trim(vColour_Name) & "', '" & Trim(vSur_Name) & "') "
                    CmdTo.ExecuteNonQuery()

                End If

            Next

        End If

        Me.Text = ""

    End Sub
    Private Sub LotNoHead_Transfer(ByVal sqltr As SqlClient.SqlTransaction)
        Dim CmdFrm As New SqlClient.SqlCommand
        Dim CmdTo As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim vLot_IdNo As Integer, vOldLID As Integer
        Dim vLot_Name As String, vSur_Name As String
        Dim vLotNm As String, vLotDescNm As String

        Me.Text = "Lot_Head"

        CmdFrm.Connection = CnFrm
        CmdTo.Connection = CnTo
        CmdTo.Transaction = sqltr

        CmdTo.CommandText = "Delete from Lot_Head "
        CmdTo.ExecuteNonQuery()

        Da1 = New SqlClient.SqlDataAdapter("select * from Lot_Head Order by LotNo_Idno", CnFrm)
        Dt1 = New DataTable
        Da1.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then

            For I = 0 To Dt1.Rows.Count - 1

                Me.Text = "Lot_Head  -  " & I

                vLotNm = Common_Procedures.Remove_NonCharacters(Dt1.Rows(I).Item("LotNo_Name").ToString)
                vOldLID = Val(Common_Procedures.get_FieldValue(CnTo, "Lot_Head", "Lot_IdNo", "(Sur_Name = '" & Trim(vLotNm) & "')", , sqltr))

                If vOldLID = 0 Then

                    vLot_IdNo = Val(Dt1.Rows(I).Item("LotNo_Idno").ToString)

                    vLot_Name = Replace(Dt1.Rows(I).Item("LotNo_Name").ToString, "'", "")

                    vSur_Name = Common_Procedures.Remove_NonCharacters(vLot_Name)

                    vLotDescNm = Dt1.Rows(I).Item("Description").ToString

                    CmdTo.CommandText = "Insert into Lot_Head ( Lot_IdNo        ,            Lot_No      ,            Sur_Name                 , Lot_Description ) " & _
                                        "       Values (" & Str(Val(vLot_IdNo)) & ", '" & Trim(vLot_Name) & "', '" & Trim(vSur_Name) & "' , '" & Trim(vLotDescNm) & "') "
                    CmdTo.ExecuteNonQuery()

                End If

            Next

        End If

        Me.Text = ""

    End Sub
    Private Sub RackHead_Transfer(ByVal sqltr As SqlClient.SqlTransaction)
        Dim CmdFrm As New SqlClient.SqlCommand
        Dim CmdTo As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim vRack_IdNo As Integer, vOldLID As Integer
        Dim vRack_Name As String, vSur_Name As String
        Dim vRackNm As String

        Me.Text = "Rack_Head"

        CmdFrm.Connection = CnFrm
        CmdTo.Connection = CnTo
        CmdTo.Transaction = sqltr

        CmdTo.CommandText = "Delete from Rack_Head "
        CmdTo.ExecuteNonQuery()

        Da1 = New SqlClient.SqlDataAdapter("select * from Rack_Head Order by Rack_Idno", CnFrm)
        Dt1 = New DataTable
        Da1.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then

            For I = 0 To Dt1.Rows.Count - 1

                Me.Text = "Rack_Head  -  " & I

                vRackNm = Common_Procedures.Remove_NonCharacters(Dt1.Rows(I).Item("Rack_Name").ToString)
                vOldLID = Val(Common_Procedures.get_FieldValue(CnTo, "Rack_Head", "Rack_Idno", "(Sur_Name = '" & Trim(vRackNm) & "')", , sqltr))

                If vOldLID = 0 Then

                    vRack_IdNo = Val(Dt1.Rows(I).Item("Rack_Idno").ToString)

                    vRack_Name = Replace(Dt1.Rows(I).Item("Rack_Name").ToString, "'", "")

                    vSur_Name = Common_Procedures.Remove_NonCharacters(vRack_Name)

                    CmdTo.CommandText = "Insert into Rack_Head ( Rack_Idno        ,            Rack_No      ,            Sur_Name   ) " & _
                                        "       Values (" & Str(Val(vRack_IdNo)) & ", '" & Trim(vRack_Name) & "', '" & Trim(vSur_Name) & "') "
                    CmdTo.ExecuteNonQuery()

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
        Dim vUnit_IdNo As Integer, vOldLID As Integer
        Dim vUnit_Name As String, vSur_Name As String
        Dim vRackNm As String

        Me.Text = "Unit_Head"

        CmdFrm.Connection = CnFrm
        CmdTo.Connection = CnTo
        CmdTo.Transaction = sqltr

        CmdTo.CommandText = "Delete from Unit_Head"
        CmdTo.ExecuteNonQuery()

        Da1 = New SqlClient.SqlDataAdapter("select distinct(Unit) as Unit_Name from Processing_Item_Head Order by Unit", CnFrm)
        Dt1 = New DataTable
        Da1.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then

            For I = 0 To Dt1.Rows.Count - 1

                Me.Text = "Unit_Head  -  " & I

                vRackNm = Common_Procedures.Remove_NonCharacters(Dt1.Rows(I).Item("Unit_Name").ToString)
                vOldLID = Val(Common_Procedures.get_FieldValue(CnTo, "Unit_Head", "Unit_IdNo", "(Sur_Name = '" & Trim(vRackNm) & "')", , sqltr))

                If vOldLID = 0 Then

                    vUnit_IdNo = Val(I) + 1

                    vUnit_Name = Replace(Dt1.Rows(I).Item("Unit_Name").ToString, "'", "")

                    vSur_Name = Common_Procedures.Remove_NonCharacters(vUnit_Name)

                    CmdTo.CommandText = "Insert into Unit_Head ( Unit_IdNo        ,            Unit_Name        ,            Sur_Name   ) " & _
                                        "       Values (" & Str(Val(vUnit_IdNo)) & ", '" & Trim(vUnit_Name) & "', '" & Trim(vSur_Name) & "') "
                    CmdTo.ExecuteNonQuery()

                End If

            Next

        End If

        Me.Text = ""

    End Sub

    Private Sub ProcessItemHead_Transfer(ByVal sqltr As SqlClient.SqlTransaction)

        Dim CmdFrm As New SqlClient.SqlCommand
        Dim CmdTo As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim VProcessed_Item_IdNo As Integer, vOldLID As Integer
        Dim VProcessed_Item_DisplaySlNo As Integer, vSur_Name As String, VProcessed_Item_Type As String, VProcessed_Item_Name As String
        Dim vProcessed_Item_Nm As String, vProcessed_Item_Code As String, vProcessed_ItemGroup_IdNo As Integer, vUnit_IdNo As Integer
        Dim vLot_IdNo As Integer, vTax_Percentage As Integer, vSale_TaxRate As Integer, vSales_Rate As Integer, vCost_Rate As Integer
        Dim vMinimum_Stock As Integer, vMeter_Qty As Single, vWeight_Piece As Integer, vWidth As Integer
        Dim vProcessed_Item_Image As Image
        Dim vLedNm As String

        Me.Text = "Processed_Item_Head"

        CmdFrm.Connection = CnFrm
        CmdTo.Connection = CnTo
        CmdTo.Transaction = sqltr

        CmdTo.CommandText = "Delete from Processed_Item_Head"
        CmdTo.ExecuteNonQuery()

        Dim ms As New MemoryStream()
        Dim data As Byte() = ms.GetBuffer()
        Dim p As New SqlClient.SqlParameter("@photo", SqlDbType.Image)
        p.Value = data
        CmdTo.Parameters.Add(p)
        ms.Dispose()

        Da1 = New SqlClient.SqlDataAdapter("select * from Processing_Item_Head Order by Processing_Item_Idno", CnFrm)
        Dt1 = New DataTable
        Da1.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then

            For I = 0 To Dt1.Rows.Count - 1

                Me.Text = "Processed_Item_Head  -  " & I

                vLedNm = Common_Procedures.Remove_NonCharacters(Dt1.Rows(I).Item("Processing_Item_Name").ToString)
                vOldLID = Val(Common_Procedures.get_FieldValue(CnTo, "Processed_Item_Head", "Processed_Item_IdNo", "(Sur_Name = '" & Trim(vLedNm) & "')", , sqltr))

                If vOldLID = 0 Then

                    VProcessed_Item_IdNo = Val(Dt1.Rows(I).Item("Processing_Item_Idno").ToString)

                    VProcessed_Item_Name = Replace(Dt1.Rows(I).Item("Processing_Item_Name").ToString, "'", "")

                    vSur_Name = Common_Procedures.Remove_NonCharacters(VProcessed_Item_Name)

                    If Trim(UCase(Dt1.Rows(I).Item("Item_Type").ToString)) = "F PRODUCT" Then
                        VProcessed_Item_Type = "FP"
                    Else
                        VProcessed_Item_Type = "GREY"
                    End If

                    VProcessed_Item_DisplaySlNo = Common_Procedures.get_MaxIdNo(CnTo, "Processed_Item_Head", "Processed_Item_DisplaySlNo", "(Processed_Item_Type= '" & Trim(VProcessed_Item_Type) & "')", sqltr)

                    vProcessed_Item_Nm = Dt1.Rows(I).Item("Processing_Item_Nm").ToString

                    vProcessed_Item_Code = Dt1.Rows(I).Item("Item_Code").ToString

                    vProcessed_ItemGroup_IdNo = Val(Dt1.Rows(I).Item("Itemgroup_Idno").ToString)

                    vUnit_IdNo = Common_Procedures.Unit_NameToIdNo(CnTo, Dt1.Rows(I).Item("Unit").ToString, sqltr)

                    'vLot_IdNo = Common_Procedures.Lot_NoToIdNo(CnFrm, Dt1.Rows(I).Item("Lot_No").ToString)

                    vTax_Percentage = 0

                    vSale_TaxRate = Val(Dt1.Rows(I).Item("Rate").ToString)

                    vSales_Rate = Val(Dt1.Rows(I).Item("Rate").ToString)

                    vCost_Rate = 0

                    vMinimum_Stock = Val(Dt1.Rows(I).Item("Opening_Stock").ToString)

                    vMeter_Qty = Val(Dt1.Rows(I).Item("Metre_Qty").ToString)

                    vWeight_Piece = Val(Dt1.Rows(I).Item("Weight_Pcs").ToString)

                    vWidth = Val(Dt1.Rows(I).Item("Width").ToString)

                    vProcessed_Item_Image = Nothing

                    CmdTo.CommandText = "Insert into Processed_Item_Head ( Processed_Item_IdNo                 ,       Processed_Item_DisplaySlNo              ,  Processed_Item_Type                ,     Processed_Item_Name             , Processed_Item_Nm                 ,            Sur_Name      ,     Processed_Item_Code             ,  Processed_ItemGroup_IdNo                 , Unit_IdNo                    , Lot_IdNo                     , Tax_Percentage                   , Sale_TaxRate                   , Sales_Rate                  , Cost_Rate                  , Minimum_Stock                   , Meter_Qty                 , Weight_Piece                    , Width                , Processed_Item_Image  ) " & _
                                                               " Values (" & Str(Val(VProcessed_Item_IdNo)) & ", " & Str(Val(VProcessed_Item_DisplaySlNo)) & "  , '" & Trim(VProcessed_Item_Type) & "', '" & Trim(VProcessed_Item_Name) & "', '" & Trim(vProcessed_Item_Nm) & "','" & Trim(vSur_Name) & "' , '" & Trim(vProcessed_Item_Code) & "'," & Str(Val(vProcessed_ItemGroup_IdNo)) & " ," & Str(Val(vUnit_IdNo)) & " , " & Str(Val(vLot_IdNo)) & " , " & Str(Val(vTax_Percentage)) & " ," & Str(Val(vSale_TaxRate)) & " ," & Str(Val(vSales_Rate)) & "," & Str(Val(vCost_Rate)) & "," & Str(Val(vMinimum_Stock)) & "," & Str(Val(vMeter_Qty)) & "," & Str(Val(vWeight_Piece)) & "," & Str(Val(vWidth)) & ", @photo   ) "
                    CmdTo.ExecuteNonQuery()

                End If

            Next


        End If

        Me.Text = ""

    End Sub

    Private Sub LedgerDetails_Transfer(ByVal sqltr As SqlClient.SqlTransaction)
        Dim CmdFrm As New SqlClient.SqlCommand
        Dim CmdTo As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim VLedger_Idno As Integer, vSl_No, vOldLID As Integer
        Dim VItem_Idno As Integer, VParty_ItemName As String

        Me.Text = "LEDGER ITEM DETAILS"

        CmdFrm.Connection = CnFrm
        CmdTo.Connection = CnTo
        CmdTo.Transaction = sqltr

        CmdTo.CommandText = "Delete from Ledger_ItemName_Details"
        CmdTo.ExecuteNonQuery()

        Da1 = New SqlClient.SqlDataAdapter("select * from Ledger_Item_Details where Party_Name <> '' And Item_Idno <> 0 Order by Ledger_Idno", CnFrm)
        Dt1 = New DataTable
        Da1.Fill(Dt1)


        If Dt1.Rows.Count > 0 Then

            For I = 0 To Dt1.Rows.Count - 1

                Me.Text = "LEDGER ITEM DETAILS  -  " & I

                VLedger_Idno = Val(Dt1.Rows(I).Item("Ledger_Idno").ToString) + 80
                VItem_Idno = Dt1.Rows(I).Item("Item_Idno").ToString


                vOldLID = Val(Common_Procedures.get_FieldValue(CnTo, "Ledger_ItemName_Details", "Ledger_Idno", "(Ledger_Idno = '" & Trim(VLedger_Idno) & "' and Item_Idno = " & Trim(VItem_Idno) & ")", , sqltr))

                If vOldLID = 0 Then

                    vSl_No = Val(Dt1.Rows(I).Item("Sl_No").ToString)

                    VParty_ItemName = Dt1.Rows(I).Item("Party_Name").ToString


                    CmdTo.CommandText = "Insert into Ledger_ItemName_Details ( Ledger_Idno                 ,       Sl_No               ,  Item_Idno                   ,     Party_ItemName               ) " & _
                                                               " Values (" & Str(Val(VLedger_Idno)) & ", " & Str(Val(vSl_No)) & "  , " & Str(Val(VItem_Idno)) & " , '" & Trim(VParty_ItemName) & "'  ) "
                    CmdTo.ExecuteNonQuery()

                End If


            Next

        End If

        Me.Text = ""

    End Sub

    Private Sub ProcessedItemSalesName_Transfer(ByVal sqltr As SqlClient.SqlTransaction)
        Dim CmdFrm As New SqlClient.SqlCommand
        Dim CmdTo As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim vProcessed_Item_SalesIdNo As Integer, vOldLID As Integer
        Dim vProcessed_Item_SalesName As String, vSur_Name As String
        Dim vRackNm As String

        Me.Text = "Processed_Item_SalesName_Head"

        CmdFrm.Connection = CnFrm
        CmdTo.Connection = CnTo
        CmdTo.Transaction = sqltr

        CmdTo.CommandText = "Delete from Processed_Item_SalesName_Head"
        CmdTo.ExecuteNonQuery()

        Da1 = New SqlClient.SqlDataAdapter("select distinct(Item_SalesName) as ItemSalesName from Processing_Item_Details Order by Item_SalesName", CnFrm)
        Dt1 = New DataTable
        Da1.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then

            For I = 0 To Dt1.Rows.Count - 1

                Me.Text = "Processed_Item_SalesName_Head  -  " & I

                vRackNm = Common_Procedures.Remove_NonCharacters(Dt1.Rows(I).Item("ItemSalesName").ToString)
                vOldLID = Val(Common_Procedures.get_FieldValue(CnTo, "Processed_Item_SalesName_Head", "Processed_Item_SalesIdNo", "(Sur_Name = '" & Trim(vRackNm) & "')", , sqltr))

                If vOldLID = 0 Then

                    vProcessed_Item_SalesIdNo = Val(I) + 1

                    vProcessed_Item_SalesName = Replace(Dt1.Rows(I).Item("ItemSalesName").ToString, "'", "")

                    vSur_Name = Common_Procedures.Remove_NonCharacters(vProcessed_Item_SalesName)

                    CmdTo.CommandText = "Insert into Processed_Item_SalesName_Head ( Processed_Item_SalesIdNo          ,            Processed_Item_SalesName      ,               Sur_Name   ) " & _
                                        "       Values (" & Str(Val(vProcessed_Item_SalesIdNo)) & ", '" & Trim(vProcessed_Item_SalesName) & "', '" & Trim(vSur_Name) & "') "
                    CmdTo.ExecuteNonQuery()

                End If

            Next

        End If

        Me.Text = ""

    End Sub
    Private Sub ProcessedItemSalesNameDetails_Transfer(ByVal sqltr As SqlClient.SqlTransaction)
        Dim CmdFrm As New SqlClient.SqlCommand
        Dim CmdTo As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim VProcessed_Item_IdNo As Integer, vSl_No, vOldLID As Integer
        Dim VCompany_IdNo As Integer, VProcessed_Item_SalesIdNo As Integer

        Me.Text = "Processed_Item_SalesName_Details"

        CmdFrm.Connection = CnFrm
        CmdTo.Connection = CnTo
        CmdTo.Transaction = sqltr

        CmdTo.CommandText = "Delete from Processed_Item_SalesName_Details"
        CmdTo.ExecuteNonQuery()

        Da1 = New SqlClient.SqlDataAdapter("select * from Processing_Item_Details where Item_SalesName <> '' And Company_Idno <> 0 Order by Processing_Item_Idno", CnFrm)
        Dt1 = New DataTable
        Da1.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then

            For I = 0 To Dt1.Rows.Count - 1

                Me.Text = "Processed_Item_SalesName_Details  -  " & I

                VProcessed_Item_IdNo = Val(Dt1.Rows(I).Item("Processing_Item_Idno").ToString)
                VCompany_IdNo = Dt1.Rows(I).Item("Company_Idno").ToString

                vOldLID = Val(Common_Procedures.get_FieldValue(CnTo, "Processed_Item_SalesName_Details", "Processed_Item_IdNo", "(Processed_Item_IdNo = '" & Trim(VProcessed_Item_IdNo) & "' and Company_IdNo = " & Trim(VCompany_IdNo) & ")", , sqltr))

                If vOldLID = 0 Then

                    vSl_No = Val(Dt1.Rows(I).Item("Sl_No").ToString)

                    VProcessed_Item_SalesIdNo = Common_Procedures.Processed_Item_SalesNameToIdNo(CnTo, Dt1.Rows(I).Item("Item_SalesName").ToString, sqltr)

                    CmdTo.CommandText = "Insert into Processed_Item_SalesName_Details ( Processed_Item_IdNo   ,       Sl_No               ,  Company_Idno                   ,     Processed_Item_SalesIdNo               ) " & _
                                                               " Values (" & Str(Val(VProcessed_Item_IdNo)) & ", " & Str(Val(vSl_No)) & "  , " & Str(Val(VCompany_IdNo)) & " ," & Str(Val(VProcessed_Item_SalesIdNo)) & " ) "
                    CmdTo.ExecuteNonQuery()

                End If

            Next

        End If

        Me.Text = ""

    End Sub
    Private Sub ProcessedItemDetails_Transfer(ByVal sqltr As SqlClient.SqlTransaction)
        Dim CmdFrm As New SqlClient.SqlCommand
        Dim CmdTo As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim VProcessed_Item_IdNo As Integer, vSl_No, vOldLID As Integer
        Dim VFinished_Product_IdNo As Integer

        Me.Text = "Processed_Item_Details"

        CmdFrm.Connection = CnFrm
        CmdTo.Connection = CnTo
        CmdTo.Transaction = sqltr

        CmdTo.CommandText = "Delete from Processed_Item_Details"
        CmdTo.ExecuteNonQuery()

        Da1 = New SqlClient.SqlDataAdapter("select * from FinishedProduct_Details where FinishedProduct_Idno <> 0 Order by Processing_Item_Idno", CnFrm)
        Dt1 = New DataTable
        Da1.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then

            For I = 0 To Dt1.Rows.Count - 1

                Me.Text = "Processed_Item_Details  -  " & I

                VProcessed_Item_IdNo = Val(Dt1.Rows(I).Item("Processing_Item_Idno").ToString)
                VFinished_Product_IdNo = Val(Dt1.Rows(I).Item("FinishedProduct_Idno").ToString)

                vOldLID = Val(Common_Procedures.get_FieldValue(CnTo, "Processed_Item_Details", "Processed_Item_IdNo", "(Processed_Item_IdNo = '" & Trim(VProcessed_Item_IdNo) & "' And Finished_Product_IdNo = '" & Trim(VFinished_Product_IdNo) & "')", , sqltr))

                If vOldLID = 0 Then

                    vSl_No = Val(Dt1.Rows(I).Item("Sl_No").ToString)

                    CmdTo.CommandText = "Insert into Processed_Item_Details ( Processed_Item_IdNo              ,       Sl_No               ,  Finished_Product_IdNo                   ) " & _
                                                               " Values (" & Str(Val(VProcessed_Item_IdNo)) & ", " & Str(Val(vSl_No)) & "  , " & Str(Val(VFinished_Product_IdNo)) & " ) "
                    CmdTo.ExecuteNonQuery()

                End If

            Next

        End If

        Me.Text = ""

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

End Class