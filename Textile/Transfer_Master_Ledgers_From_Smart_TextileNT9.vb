Imports System.IO

Public Class Transfer_Master_Ledgers_From_Smart_TextileNT9

    Private CnTo As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private CnFrm As New SqlClient.SqlConnection

    Private Sub Transfer_Master_Ledgers_From_Smart_TextileNT9_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        If cbo_DBFrom.Enabled And cbo_DBFrom.Visible Then cbo_DBFrom.Focus()
    End Sub

    Private Sub Transfer_Master_Ledgers_From_Smart_TextileNT9_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(cbo_DBFrom, CnTo, "master..sysdatabases", "name", "(name LIKE 'Smart%NT%')", "")

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

            AreaHead_Transfer(tr)

            LedgerHead_Transfer(tr)

            CountHead_Transfer(tr)

            EndsCountHead_Transfer(tr)

            MillHead_Transfer(tr)

            ClothHead_Transfer(tr)

            'Ledger_Opening_Transfer(tr)

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
        Dim AccGrp4 As String = ""

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
                    Inc = 0

                    Inc = Inc + 1
                    If UBound(AccGrpAr) >= Inc Then AccGrp1 = Trim(AccGrpAr(Inc))
                    If Val(AccGrp1) > 30 Then AccGrp1 = AccGrp1 + 2

                    Inc = Inc + 1
                    If UBound(AccGrpAr) >= Inc Then AccGrp2 = Trim(AccGrpAr(Inc))
                    If Val(AccGrp2) > 30 Then AccGrp2 = AccGrp2 + 2

                    Inc = Inc + 1
                    If UBound(AccGrpAr) >= Inc Then AccGrp3 = Trim(AccGrpAr(Inc))
                    If Val(AccGrp3) > 30 Then AccGrp3 = AccGrp3 + 2

                    Inc = Inc + 1
                    If UBound(AccGrpAr) >= Inc Then AccGrp4 = Trim(AccGrpAr(Inc))

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

                    CmdTo.CommandText = "Insert into AccountsGroup_Head ( AccountsGroup_IdNo ,            AccountsGroup_Name      ,            Sur_Name      ,          Parent_Name        ,            Parent_Idno      ,              Carried_Balance      ,                Order_Position      ,            TallyName      ,            TallySubName      ,              Indicate       ) " & _
                                        "       Values (" & Str(Val(vAccountsGroup_IdNo)) & ", '" & Trim(vAccountsGroup_Name) & "', '" & Trim(vSur_Name) & "', '" & Trim(vParent_Name) & "', '" & Trim(vParent_Idno) & "', " & Str(Val(vCarried_Balance)) & ",   " & Str(Val(vOrder_Position)) & ", '" & Trim(vTallyName) & "', '" & Trim(vTallySubName) & "', " & Str(Val(vIndicate)) & " ) "
                    CmdTo.ExecuteNonQuery()

                End If

            Next

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
        Dim vSurNm As String

        Me.Text = "Area_Head"

        CmdFrm.Connection = CnFrm
        CmdTo.Connection = CnTo
        CmdTo.Transaction = sqltr

        CmdTo.CommandText = "Delete from Area_Head"
        CmdTo.ExecuteNonQuery()

        Da1 = New SqlClient.SqlDataAdapter("select distinct(AREA_NAME) from Ledger_Head Where AREA_NAME <> '' Order by AREA_NAME", CnFrm)
        Dt1 = New DataTable
        Da1.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then

            For I = 0 To Dt1.Rows.Count - 1

                Me.Text = "Area_Head  -  " & I

                If Trim(Dt1.Rows(I).Item("Area_Name").ToString) <> "" Then

                    vSurNm = Common_Procedures.Remove_NonCharacters(Dt1.Rows(I).Item("Area_Name").ToString)

                    vOldLID = Val(Common_Procedures.get_FieldValue(CnTo, "Area_Head", "Area_IdNo", "(Sur_Name = '" & Trim(vSurNm) & "')", , sqltr))

                    If vOldLID = 0 Then

                        vArea_IdNo = Val(I) + 1

                        vArea_Name = Replace(Dt1.Rows(I).Item("Area_Name").ToString, "'", "")

                        vSur_Name = Common_Procedures.Remove_NonCharacters(vArea_Name)

                        CmdTo.CommandText = "Insert into Area_Head ( Area_Idno        ,            Area_Name      ,            Sur_Name   ) " & _
                                            "       Values (" & Str(Val(vArea_IdNo)) & ", '" & Trim(vArea_Name) & "', '" & Trim(vSur_Name) & "') "
                        CmdTo.ExecuteNonQuery()

                    End If

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
        Dim vArea_IdNo As Integer, vAccountsGroup_IdNo As String, vParent_Code As String, vBill_Type As String
        Dim vLedger_Address1 As String, vLedger_Address2 As String, vLedger_Address3 As String, vLedger_Address4 As String, vLedger_PhoneNo As String
        Dim vLedger_TinNo As String, vLedger_CstNo As String, vLedger_Type As String, vPan_No As String
        Dim vLedger_Emailid As String, vLedger_FaxNo As String, vLedger_MobileNo As String, vContact_Person As String
        Dim vPackingType_CompanyIdNo As Integer, vLedger_AgentIdNo As Integer, vNote As String = ""
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
        Dim vLedger_State As String, vLedger_GSTinNo As String

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

                vSurNm = Common_Procedures.Remove_NonCharacters(Dt1.Rows(I).Item("Ledger_Name").ToString)
                vOldLID = Val(Common_Procedures.get_FieldValue(CnTo, "Ledger_Head", "Ledger_IdNo", "(Sur_Name = '" & Trim(vSurNm) & "')", , sqltr))

                If vOldLID = 0 Then

                    vLedger_IdNo = Val(Dt1.Rows(I).Item("Ledger_IdNo").ToString) + 80

                    vLedger_Name = Replace(Dt1.Rows(I).Item("Ledger_Name").ToString, "'", "")
                    vLedger_MainName = vLedger_Name
                    vSur_Name = Common_Procedures.Remove_NonCharacters(vLedger_Name)
                    vLedger_AlaisName = ""

                    vArea_IdNo = 0
                    If Trim(Dt1.Rows(I).Item("Area_Name").ToString) <> "" Then
                        vArea_IdNo = Val(Common_Procedures.get_FieldValue(CnTo, "Area_Head", "Area_IdNo", "(Area_Name = '" & Trim(Dt1.Rows(I).Item("Area_Name").ToString) & "')", , sqltr))
                    End If

                    vParent_Code = Dt1.Rows(I).Item("Parent_Code").ToString

                    Erase AccGrpAr
                    AccGrpAr = Split(Trim(vParent_Code), "~")
                    Inc = 0
                    Inc = Inc + 1
                    If UBound(AccGrpAr) >= Inc Then AccGrp1 = Trim(AccGrpAr(Inc))
                    If Val(AccGrp1) > 30 Then AccGrp1 = AccGrp1 + 2
                    Inc = Inc + 1
                    If UBound(AccGrpAr) >= Inc Then AccGrp2 = Trim(AccGrpAr(Inc))
                    If Val(AccGrp2) > 30 Then AccGrp2 = AccGrp2 + 2
                    Inc = Inc + 1
                    If UBound(AccGrpAr) >= Inc Then AccGrp3 = Trim(AccGrpAr(Inc))
                    If Val(AccGrp3) > 30 Then AccGrp3 = AccGrp3 + 2
                    Inc = Inc + 1
                    If UBound(AccGrpAr) >= Inc Then AccGrp4 = Trim(AccGrpAr(Inc))

                    If Trim(AccGrp4) <> "" Then
                        vParent_Code = "~" & AccGrp1 & "~" & AccGrp2 & "~" & AccGrp3 & "~" & AccGrp4 & "~"
                    ElseIf Trim(AccGrp3) <> "" Then
                        vParent_Code = "~" & AccGrp1 & "~" & AccGrp2 & "~" & AccGrp3 & "~"
                    ElseIf Trim(AccGrp2) <> "" Then
                        vParent_Code = "~" & AccGrp1 & "~" & AccGrp2 & "~"
                    Else
                        vParent_Code = "~" & AccGrp1 & "~"
                    End If

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
                    vPartner_Proprietor = Replace(Dt1.Rows(I).Item("Proprietor_Partner").ToString, "'", "")
                    vYarn_Comm_Percentage = Dt1.Rows(I).Item("Yarn_Commission_Percentage").ToString
                    vYarn_Comm_Bag = Dt1.Rows(I).Item("Commission_Bag").ToString
                    vCloth_Comm_Percentage = Dt1.Rows(I).Item("Commission_Percentage").ToString
                    vCloth_Comm_Meter = Dt1.Rows(I).Item("Cloth_Commission_Meter").ToString

                    vLedger_Emailid = Replace(Dt1.Rows(I).Item("Email_Id").ToString, "'", "")
                    vLedger_FaxNo = "" 'Dt1.Rows(I).Item("Fax_No").ToString
                    vLedger_MobileNo = "" 'Dt1.Rows(I).Item("Ledger_MobileNo").ToString
                    vContact_Person = "" 'Replace(Dt1.Rows(I).Item("Contact_Person").ToString, "'", "")
                    vPackingType_CompanyIdNo = 0
                    vLedger_AgentIdNo = 0

                    '  vNote = Replace(Dt1.Rows(I).Item("Note1").ToString, "'", "")

                    vMobileNo_Sms = "" 'Dt1.Rows(I).Item("Ledger_MobileNo").ToString
                    vOwner_Name = Replace(Dt1.Rows(I).Item("Owner_Name").ToString, "'", "")
                    vTds_Percentage = Val(Dt1.Rows(I).Item("Tds_Percentage").ToString)
                    vOwn_Loom_Status = Val(Dt1.Rows(I).Item("Own_Loom").ToString)
                    vFreight_Loom = Val(Dt1.Rows(I).Item("Freight_Loom").ToString)
                    vNoOf_Looms = Val(Dt1.Rows(I).Item("Noof_Looms").ToString)
                    vTransport_IdNo = 0
                    vVerified_Status = 1

                    vShow_In_All_Entry = 0
                    If Trim(UCase(Dt1.Rows(I).Item("Ledger_Type1").ToString)) = "ALL" Then
                        vShow_In_All_Entry = 1
                    End If

                    vLedger_GSTinNo = Replace(Dt1.Rows(I).Item("ledger_GSTin").ToString, "'", "")


                    vLedger_State = Common_Procedures.State_NameToIdNo(CnTo, Dt1.Rows(I).Item("StateName").ToString, sqltr)


                    vBilling_Type = ""
                    vSticker_Type = ""
                    vMrp_Perc = ""


                    CmdTo.CommandText = "Insert into ledger_head ( Ledger_IdNo        ,            Ledger_Name      ,            Sur_Name      ,            Ledger_MainName      ,            Ledger_AlaisName      ,              Area_IdNo      ,              AccountsGroup_IdNo      ,            Parent_Code      ,            Bill_Type      ,            Ledger_Address1      ,            Ledger_Address2      ,            Ledger_Address3      ,            Ledger_Address4      ,            Ledger_PhoneNo      ,            Ledger_TinNo      ,            Ledger_CstNo      ,            Ledger_Type      ,            Pan_No      ,            Partner_Proprietor      ,              Yarn_Comm_Percentage      ,              Yarn_Comm_Bag      ,              Cloth_Comm_Percentage      ,            Ledger_Emailid      ,            Ledger_FaxNo      ,            Ledger_MobileNo      ,            MobileNo_Frsms    ,            MobileNo_Sms      ,            Contact_Person      ,             PackingType_CompanyIdNo       ,              Ledger_AgentIdNo      ,            Note      ,            Show_In_All_Entry        ,            Billing_Type      ,            Sticker_Type      ,            Mrp_Perc      ,              Own_Loom_Status      ,              Freight_Loom      ,              NoOf_Looms      ,              Transport_IdNo      , Verified_Status,            Owner_Name      ,              Tds_Percentage       ,         Ledger_GSTinNo          ,    Ledger_state_idno) " & _
                                        "       Values (" & Str(Val(vLedger_IdNo)) & ", '" & Trim(vLedger_Name) & "', '" & Trim(vSur_Name) & "', '" & Trim(vLedger_MainName) & "', '" & Trim(vLedger_AlaisName) & "', " & Str(Val(vArea_IdNo)) & ", " & Str(Val(vAccountsGroup_IdNo)) & ", '" & Trim(vParent_Code) & "', '" & Trim(vBill_Type) & "', '" & Trim(vLedger_Address1) & "', '" & Trim(vLedger_Address2) & "', '" & Trim(vLedger_Address3) & "', '" & Trim(vLedger_Address4) & "', '" & Trim(vLedger_PhoneNo) & "', '" & Trim(vLedger_TinNo) & "', '" & Trim(vLedger_CstNo) & "', '" & Trim(vLedger_Type) & "', '" & Trim(vPan_No) & "', '" & Trim(vPartner_Proprietor) & "', " & Str(Val(vYarn_Comm_Percentage)) & ", " & Str(Val(vYarn_Comm_Bag)) & ", " & Str(Val(vCloth_Comm_Percentage)) & ", '" & Trim(vLedger_Emailid) & "', '" & Trim(vLedger_FaxNo) & "', '" & Trim(vLedger_MobileNo) & "', '" & Trim(vMobileNo_Sms) & "', '" & Trim(vMobileNo_Sms) & "', '" & Trim(vContact_Person) & "', " & Str(Val(vPackingType_CompanyIdNo)) & ", " & Str(Val(vLedger_AgentIdNo)) & ", '" & Trim(vNote) & "', " & Str(Val(vShow_In_All_Entry)) & ", '" & Trim(vBilling_Type) & "', '" & Trim(vSticker_Type) & "', '" & Trim(vMrp_Perc) & "', " & Str(Val(vOwn_Loom_Status)) & ", " & Str(Val(vFreight_Loom)) & ", " & Str(Val(vNoOf_Looms)) & ", " & Str(Val(vTransport_IdNo)) & ",       1        , '" & Trim(vOwner_Name) & "', " & Str(Val(vTds_Percentage)) & " , '" & Trim(vLedger_GSTinNo) & "',  '" & Trim(vLedger_State) & "' ) "
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
                    CmdTo.CommandText = "Insert into Ledger_AlaisHead(Ledger_IdNo, Sl_No, Ledger_DisplayName, Ledger_Type, AccountsGroup_IdNo, Own_Loom_Status, Show_In_All_Entry, Verified_Status , Area_IdNo, Close_status) Values (" & Str(Val(Dt1.Rows(i).Item("Ledger_IdNo").ToString)) & ",    1,      '" & Trim(Dt1.Rows(i).Item("Ledger_Name").ToString) & "',   '" & Trim(Dt1.Rows(i).Item("Ledger_Type").ToString) & "',    " & Str(Val(Dt1.Rows(i).Item("AccountsGroup_IdNo").ToString)) & ", 0, 0, 1,  0, 0)"
                    CmdTo.ExecuteNonQuery()
                Next

            End If

            CmdTo.Dispose()
            Dt1.Dispose()
            Da1.Dispose()

        End If

        Me.Text = ""

    End Sub

    Private Sub CountHead_Transfer(ByVal sqltr As SqlClient.SqlTransaction)
        Dim CmdFrm As New SqlClient.SqlCommand
        Dim CmdTo As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim vCount_IdNo As Integer, vOldLID As Integer
        Dim vSur_Name As String
        Dim vCount_Name As String
        Dim vResultant_Count As Single
        Dim vCount_StockUnder_IdNo As Integer
        Dim vCount_Description As String = ""
        Dim vSurNm As String = ""

        Me.Text = "Count_Head"

        CmdFrm.Connection = CnFrm
        CmdTo.Connection = CnTo
        CmdTo.Transaction = sqltr

        CmdTo.CommandText = "Delete from Count_Head"
        CmdTo.ExecuteNonQuery()

        Da1 = New SqlClient.SqlDataAdapter("select * from Count_Head Order by Count_IdNo", CnFrm)
        Dt1 = New DataTable
        Da1.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then

            For I = 0 To Dt1.Rows.Count - 1

                Me.Text = "Count_Head  -  " & I

                vSurNm = Common_Procedures.Remove_NonCharacters(Dt1.Rows(I).Item("Count_Name").ToString)
                vOldLID = Val(Common_Procedures.get_FieldValue(CnTo, "Count_Head", "Count_IdNo", "(Sur_Name = '" & Trim(vSurNm) & "')", , sqltr))

                If vOldLID = 0 Then

                    vCount_IdNo = Val(Dt1.Rows(I).Item("Count_IdNo").ToString)

                    vCount_Name = Replace(Dt1.Rows(I).Item("Count_Name").ToString, "'", "")

                    vSur_Name = Common_Procedures.Remove_NonCharacters(vCount_Name)

                    vCount_StockUnder_IdNo = vCount_IdNo

                    CmdTo.CommandText = "Insert into Count_Head ( Count_IdNo         ,            Count_Name      ,            Sur_Name      ,          Count_Description        ,              Count_StockUnder_IdNo      ,                Resultant_Count      , Cotton_Polyester_Jari ) " & _
                                        "       Values (" & Str(Val(vCount_IdNo)) & ", '" & Trim(vCount_Name) & "', '" & Trim(vSur_Name) & "', '" & Trim(vCount_Description) & "', " & Str(Val(vCount_StockUnder_IdNo)) & ",   " & Str(Val(vResultant_Count)) & ",          'COTTON'     ) "
                    CmdTo.ExecuteNonQuery()

                End If

            Next

        End If

        Me.Text = ""

    End Sub

    Private Sub EndsCountHead_Transfer(ByVal sqltr As SqlClient.SqlTransaction)
        Dim CmdFrm As New SqlClient.SqlCommand
        Dim CmdTo As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim vEndsCount_IdNo As Integer, vOldLID As Integer
        Dim vEnds_Count As String, vSur_Name As String
        Dim vSurNm As String
        Dim vEnds As String
        Dim vCount_IdNo As Integer
        Dim vRate As Single
        Dim vCloth_Idno As Integer = 0
        Dim vCloth_Warp_Count As String = ""


        Me.Text = "EndsCount_Head"

        CmdFrm.Connection = CnFrm
        CmdTo.Connection = CnTo
        CmdTo.Transaction = sqltr

        CmdTo.CommandText = "Delete from EndsCount_Head"
        CmdTo.ExecuteNonQuery()

        Da1 = New SqlClient.SqlDataAdapter("select distinct(Ends_Count) from EndsCount_Head Where Ends_Count <> '' Order by Ends_Count", CnFrm)
        Dt1 = New DataTable
        Da1.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then

            For I = 0 To Dt1.Rows.Count - 1

                Me.Text = "EndsCount_Head  -  " & I

                If Trim(Dt1.Rows(I).Item("Ends_Count").ToString) <> "" Then

                    vSurNm = Common_Procedures.Remove_NonCharacters(Dt1.Rows(I).Item("Ends_Count").ToString)

                    vOldLID = Val(Common_Procedures.get_FieldValue(CnTo, "EndsCount_Head", "EndsCount_IdNo", "(Sur_Name = '" & Trim(vSurNm) & "')", , sqltr))

                    If vOldLID = 0 Then

                        vEndsCount_IdNo = Val(I) + 1

                        vEnds_Count = Replace(Dt1.Rows(I).Item("Ends_Count").ToString, "'", "")

                        vSur_Name = Common_Procedures.Remove_NonCharacters(vEnds_Count)

                        vEnds = Val(Common_Procedures.get_FieldValue(CnFrm, "EndsCount_Head", "Ends", "(Ends_Count = '" & Trim(vEnds_Count) & "')"))

                        vCloth_Idno = Val(Common_Procedures.get_FieldValue(CnFrm, "EndsCount_Head", "Cloth_Idno", "(Ends_Count = '" & Trim(vEnds_Count) & "')"))
                        vCloth_Warp_Count = Common_Procedures.get_FieldValue(CnFrm, "Cloth_Head", "Cloth_Warp_Count", "(Cloth_Idno = " & Str(Val(vCloth_Idno)) & ")")
                        vCount_IdNo = Val(Common_Procedures.get_FieldValue(CnTo, "Count_Head", "Count_IdNo", "(Count_Name = '" & Trim(vCloth_Warp_Count) & "')", , sqltr))

                        vRate = Val(Common_Procedures.get_FieldValue(CnFrm, "EndsCount_Head", "Rate_Meter", "(Ends_Count = '" & Trim(vEnds_Count) & "' and Rate_Meter <> 0)"))

                        CmdTo.CommandText = "Insert into EndsCount_Head ( EndsCount_IdNo     ,            EndsCount_Name  ,            Sur_Name      ,          Ends_Name     ,              Count_IdNo      ,              Rate      , Stock_In, Meters_Pcs, Cotton_Polyester_Jari ) " & _
                                            "       Values (" & Str(Val(vEndsCount_IdNo)) & ", '" & Trim(vEnds_Count) & "', '" & Trim(vSur_Name) & "', " & Str(Val(vEnds)) & ", " & Str(Val(vCount_IdNo)) & ", " & Str(Val(vRate)) & ",  'METER',       0   ,    'COTTON'           ) "
                        CmdTo.ExecuteNonQuery()

                    End If

                End If

            Next

        End If

        Me.Text = ""

    End Sub

    Private Sub MillHead_Transfer(ByVal sqltr As SqlClient.SqlTransaction)
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

        CmdTo.CommandText = "Delete from Mill_Head"
        CmdTo.ExecuteNonQuery()

        CmdTo.CommandText = "Delete from Mill_Count_Details"
        CmdTo.ExecuteNonQuery()

        Da1 = New SqlClient.SqlDataAdapter("select * from Mill_Head Order by Mill_IdNo", CnFrm)
        Dt1 = New DataTable
        Da1.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then

            For I = 0 To Dt1.Rows.Count - 1

                Me.Text = "Mill_Head  -  " & I

                vSurNm = Common_Procedures.Remove_NonCharacters(Dt1.Rows(I).Item("Mill_Name").ToString)
                vOldLID = Val(Common_Procedures.get_FieldValue(CnTo, "Mill_Head", "Mill_IdNo", "(Sur_Name = '" & Trim(vSurNm) & "')", , sqltr))

                If vOldLID = 0 Then

                    vMill_IdNo = Val(Dt1.Rows(I).Item("Mill_IdNo").ToString)

                    vMill_Name = Replace(Dt1.Rows(I).Item("Mill_Name").ToString, "'", "")

                    vSur_Name = Common_Procedures.Remove_NonCharacters(vMill_Name)

                    CmdTo.CommandText = "Insert into Mill_Head ( Mill_IdNo          ,            Mill_Name      ,            Sur_Name      , Weight_EmptyBag, Weight_EmptyCone ) " & _
                                        "       Values (" & Str(Val(vMill_IdNo)) & ", '" & Trim(vMill_Name) & "', '" & Trim(vSur_Name) & "',        0       ,         0        ) "
                    CmdTo.ExecuteNonQuery()

                    Da1 = New SqlClient.SqlDataAdapter("select * from Mill_Count_Head where mill_idno = " & Str(Val(vMill_IdNo)) & " Order by Sl_No", CnFrm)
                    Dt2 = New DataTable
                    Da1.Fill(Dt2)

                    If Dt2.Rows.Count > 0 Then

                        For J = 0 To Dt2.Rows.Count - 1

                            vCount_IdNo = Val(Common_Procedures.get_FieldValue(CnTo, "Count_Head", "Count_IdNo", "(Count_Name = '" & Trim(Dt2.Rows(J).Item("Count_Name").ToString) & "')", , sqltr))

                            vWeight_Bag = Val(Dt2.Rows(J).Item("Weight_Bag").ToString)
                            vCones_Bag = Val(Dt2.Rows(J).Item("Cones_Bag").ToString)
                            vWeight_Cone = Val(Dt2.Rows(J).Item("Weight_Cone").ToString)
                            vRate_Kg = Val(Dt2.Rows(J).Item("Rate_Kg").ToString)
                            vRate_Thiri = Val(Dt2.Rows(J).Item("Rate_Thiri").ToString)

                            CmdTo.CommandText = "Insert into Mill_Count_Details ( Mill_IdNo ,            Sl_No       ,            Count_IdNo        ,              Weight_Bag      ,              Cones_Bag      ,              Weight_Cone      ,              Rate_Kg      ,              Rate_Thiri       ) " & _
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

    Private Sub ClothHead_Transfer(ByVal sqltr As SqlClient.SqlTransaction)
        Dim CmdFrm As New SqlClient.SqlCommand
        Dim CmdTo As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim vCloth_IdNo As Integer, vOldLID As Integer
        Dim vCloth_Name As String, vSur_Name As String
        Dim vSurNm As String
        Dim vCloth_Description As String
        Dim vCloth_WarpCount_IdNo As Integer, vCloth_WeftCount_IdNo As Integer
        Dim vCloth_ReedSpace As Single, vCloth_Reed As Single, vCloth_Pick As Single, vCloth_Width As Single
        Dim vWeight_Meter_Warp As Single, vWeight_Meter_Weft As Single
        Dim vBeam_Length As Single, vTape_Length As Single
        Dim vCrimp_Percentage As Single
        Dim vWages_For_Type1 As Single, vWages_For_Type2 As Single, vWages_For_Type3 As Single, vWages_For_Type4 As Single, vWages_For_Type5 As Single
        Dim vStock_In As String = "", vMeters_Pcs As Single
        Dim vActualCloth_Pick As Single, vActualCrimp_Percentage As Single, vActualWeight_Meter_Weft As Single
        Dim vCloth_Stockunder_IdNo As Integer
        Dim vEndsCount_IdNo As Integer
        Dim vMark As Single

        Me.Text = "Cloth_Head"

        CmdFrm.Connection = CnFrm
        CmdTo.Connection = CnTo
        CmdTo.Transaction = sqltr

        CmdTo.CommandText = "Delete from Cloth_Head"
        CmdTo.ExecuteNonQuery()

        CmdTo.CommandText = "Delete from Cloth_EndsCount_Details"
        CmdTo.ExecuteNonQuery()


        Da1 = New SqlClient.SqlDataAdapter("select * from Cloth_Head Order by Cloth_IdNo", CnFrm)
        Dt1 = New DataTable
        Da1.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then

            For I = 0 To Dt1.Rows.Count - 1

                Me.Text = "Cloth_Head  -  " & I

                vSurNm = Common_Procedures.Remove_NonCharacters(Dt1.Rows(I).Item("Cloth_Name").ToString)
                vOldLID = Val(Common_Procedures.get_FieldValue(CnTo, "Cloth_Head", "Cloth_IdNo", "(Sur_Name = '" & Trim(vSurNm) & "')", , sqltr))

                If vOldLID = 0 Then

                    vCloth_IdNo = Val(Dt1.Rows(I).Item("Cloth_IdNo").ToString)

                    vCloth_Name = Replace(Dt1.Rows(I).Item("Cloth_Name").ToString, "'", "")

                    vSur_Name = Common_Procedures.Remove_NonCharacters(vCloth_Name)

                    vCloth_Description = Replace(Dt1.Rows(I).Item("Cloth_Description").ToString, "'", "")


                    If vCloth_IdNo = 78 Then
                        Debug.Print(Trim(Dt1.Rows(I).Item("Cloth_Warp_Count").ToString))
                    End If
                    'If Trim(UCase(Dt1.Rows(I).Item("Cloth_Warp_Count").ToString)) = "25S" Then
                    '    Debug.Print(Trim(Dt1.Rows(I).Item("Cloth_Warp_Count").ToString))
                    'End If

                    vCloth_WarpCount_IdNo = Val(Common_Procedures.get_FieldValue(CnTo, "Count_Head", "Count_IdNo", "(Count_Name = '" & Trim(Replace(Dt1.Rows(I).Item("Cloth_Warp_Count").ToString, "*", "")) & "')", , sqltr))
                    vCloth_WeftCount_IdNo = Val(Common_Procedures.get_FieldValue(CnTo, "Count_Head", "Count_IdNo", "(Count_Name = '" & Trim(Replace(Dt1.Rows(I).Item("Cloth_Weft_Count").ToString, "*", "")) & "')", , sqltr))
                    vCloth_ReedSpace = Val(Dt1.Rows(I).Item("Cloth_ReedSpace").ToString)
                    vCloth_Reed = Val(Dt1.Rows(I).Item("Cloth_Reed").ToString)
                    vCloth_Pick = Val(Dt1.Rows(I).Item("Cloth_Pick").ToString)
                    vCloth_Width = Val(Dt1.Rows(I).Item("Cloth_Width").ToString)
                    vWeight_Meter_Warp = Val(Dt1.Rows(I).Item("Weight_Meter_Warp").ToString)
                    vWeight_Meter_Weft = Val(Dt1.Rows(I).Item("Weight_Meter_Weft").ToString)
                    vBeam_Length = Val(Dt1.Rows(I).Item("Beam_Length").ToString)
                    vTape_Length = Val(Dt1.Rows(I).Item("Tape_Length").ToString)
                    vCrimp_Percentage = Val(Dt1.Rows(I).Item("Crimp_Percentage").ToString)
                    vWages_For_Type1 = Val(Dt1.Rows(I).Item("Wages_For_Type1").ToString)
                    vWages_For_Type2 = Val(Dt1.Rows(I).Item("Wages_For_Type2").ToString)
                    vWages_For_Type3 = Val(Dt1.Rows(I).Item("Wages_For_Type3").ToString)
                    vWages_For_Type4 = Val(Dt1.Rows(I).Item("Wages_For_Type4").ToString)
                    vWages_For_Type5 = Val(Dt1.Rows(I).Item("Wages_For_Type5").ToString)
                    '  vStock_In = Dt1.Rows(I).Item("Pcs_Meter").ToString
                    '  vMeters_Pcs = Val(Dt1.Rows(I).Item("Meters_Pc").ToString)
                    vActualCloth_Pick = Val(vCloth_Pick)
                    vActualCrimp_Percentage = Val(vCrimp_Percentage)
                    vActualWeight_Meter_Weft = Val(vWeight_Meter_Weft)
                    vCloth_Stockunder_IdNo = Val(vCloth_IdNo)

                    CmdTo.CommandText = "Insert into Cloth_Head ( Cloth_IdNo        ,            ClothMain_Name   ,            Cloth_Name      ,            Sur_Name       ,            Cloth_Description      ,              Cloth_WarpCount_IdNo      ,              Cloth_WeftCount_IdNo      ,              Cloth_ReedSpace      ,              Cloth_Reed      ,              Cloth_Pick      ,              Cloth_Width      ,              Weight_Meter_Warp      ,              Weight_Meter_Weft      ,              Beam_Length      ,              Tape_Length      ,              Crimp_Percentage      ,              Wages_For_Type1      ,              Wages_For_Type2      ,              Wages_For_Type3      ,              Wages_For_Type4      ,              Wages_For_Type5      ,            Stock_In      ,              Meters_Pcs      ,              ActualCloth_Pick      ,              ActualCrimp_Percentage      ,              ActualWeight_Meter_Weft      ,              Cloth_Stockunder_IdNo       ) " &
                                        "       Values (" & Str(Val(vCloth_IdNo)) & ", '" & Trim(vCloth_Name) & "', '" & Trim(vCloth_Name) & "', '" & Trim(vSur_Name) & "', '" & Trim(vCloth_Description) & "', " & Str(Val(vCloth_WarpCount_IdNo)) & ", " & Str(Val(vCloth_WeftCount_IdNo)) & ", " & Str(Val(vCloth_ReedSpace)) & ", " & Str(Val(vCloth_Reed)) & ", " & Str(Val(vCloth_Pick)) & ", " & Str(Val(vCloth_Width)) & ", " & Str(Val(vWeight_Meter_Warp)) & ", " & Str(Val(vWeight_Meter_Weft)) & ", " & Str(Val(vBeam_Length)) & ", " & Str(Val(vTape_Length)) & ", " & Str(Val(vCrimp_Percentage)) & ", " & Str(Val(vWages_For_Type1)) & ", " & Str(Val(vWages_For_Type2)) & ", " & Str(Val(vWages_For_Type3)) & ", " & Str(Val(vWages_For_Type4)) & ", " & Str(Val(vWages_For_Type5)) & ", '" & Trim(vStock_In) & "', " & Str(Val(vMeters_Pcs)) & ", " & Str(Val(vActualCloth_Pick)) & ", " & Str(Val(vActualCrimp_Percentage)) & ", " & Str(Val(vActualWeight_Meter_Weft)) & ", " & Str(Val(vCloth_Stockunder_IdNo)) & " ) "
                    CmdTo.ExecuteNonQuery()


                    Da1 = New SqlClient.SqlDataAdapter("select * from EndsCount_Head where Cloth_Idno = " & Str(Val(vCloth_IdNo)) & " Order by Ends", CnFrm)
                    Dt2 = New DataTable
                    Da1.Fill(Dt2)

                    If Dt2.Rows.Count > 0 Then

                        For J = 0 To Dt2.Rows.Count - 1

                            vEndsCount_IdNo = Val(Common_Procedures.get_FieldValue(CnTo, "EndsCount_Head", "EndsCount_IdNo", "(EndsCount_Name = '" & Trim(Dt2.Rows(J).Item("Ends_Count").ToString) & "')", , sqltr))

                            vMark = Val(Dt2.Rows(J).Item("Mark").ToString)

                            CmdTo.CommandText = "Insert into Cloth_EndsCount_Details ( Cloth_Idno ,            Sl_No       ,            EndsCount_IdNo        ,              Mark       ) " & _
                                                "          Values  ( " & Str(Val(vCloth_IdNo)) & ", " & Str(Val(J + 1)) & ", " & Str(Val(vEndsCount_IdNo)) & ", " & Str(Val(vMark)) & "  ) "
                            CmdTo.ExecuteNonQuery()

                        Next

                    End If
                    Dt2.Clear()


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